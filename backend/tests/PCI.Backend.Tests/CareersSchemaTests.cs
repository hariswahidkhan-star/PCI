using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The careers schema's structural guarantees (Data/CareersSchema.cs;
/// docs/pciworld/CCP_PHASE4_DESIGN.md §10), asserted under whichever provider TEST_DB_PROVIDER
/// selects so the MySQL leg proves parity rather than assuming it.
///
/// These assert CONSTRAINTS rather than columns, because the constraints are the safety property.
/// Application code can be reviewed for races; a unique index cannot be raced. The two that carry
/// the most weight: one application per person per posting (a double-submit must not give an
/// employer two versions of the same candidacy), and answers as SNAPSHOTS — §28 forbids modifying
/// a submitted application snapshot in place, and the whole value of that rule is that editing or
/// deleting a screening question later cannot change what an application appears to have said.
/// </summary>
[Collection(DbCollection.Name)]
public class CareersSchemaTests
{
    static Db NewDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long Employer(Db db, string slug, string state = "verified") =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_employers(slug,name,state) VALUES(?,?,?)",
            slug, "Employer " + slug, state);

    static long Posting(Db db, long employerId, string slug) =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_job_postings(employer_id,slug,title,state,salary_min_minor,salary_max_minor,currency)
              VALUES(?,?,?,'published',4500000,6000000,'GBP')",
            employerId, slug, "Posting " + slug);

    static long Question(Db db, long postingId, string prompt) =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_job_questions(posting_id,kind,prompt,required) VALUES(?,'text',?,1)",
            postingId, prompt);

    static long Application(Db db, long postingId, long applicantUserId, string state = "submitted") =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_applications(posting_id,applicant_user_id,state,submitted_at)
              VALUES(?,?,?,datetime('now'))",
            postingId, applicantUserId, state);

    static long Answer(Db db, long applicationId, long questionId, string promptSnapshot, string answerSnapshot) =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_application_answers(application_id,question_id,prompt_snapshot,answer_snapshot)
              VALUES(?,?,?,?)",
            applicationId, questionId, promptSnapshot, answerSnapshot);

    // ── one application per person per posting ────────────────────────────────────────────────

    [Fact]
    public void OnePersonCannotApplyToTheSamePostingTwice()
    {
        // Invariant 1. If both rows landed, an employer comparing candidates would have two
        // versions of the same candidacy and no way to know which one the person stands behind.
        var db = NewDb();
        var posting = Posting(db, Employer(db, "e-once"), "p-once");
        Application(db, posting, 7);

        Assert.ThrowsAny<Exception>(() => Application(db, posting, 7));
    }

    [Fact]
    public void AWithdrawnApplicationStillHoldsTheSlot()
    {
        // The unique index deliberately has no WHERE state!='withdrawn' clause — Db.Translate
        // strips predicates from partial indexes on MySQL, so a partial index would mean the two
        // providers agree only by accident. Reapplying is a state change on the row the person
        // already owns, not a second row.
        var db = NewDb();
        var posting = Posting(db, Employer(db, "e-withdrawn"), "p-withdrawn");
        var app = Application(db, posting, 8);
        db.Execute("UPDATE pciworld_applications SET state='withdrawn', withdrawn_at=datetime('now') WHERE id=?", app);

        Assert.ThrowsAny<Exception>(() => Application(db, posting, 8));
    }

    [Fact]
    public void TheSamePersonMayApplyToDifferentPostings()
    {
        var db = NewDb();
        var employer = Employer(db, "e-multi");
        Application(db, Posting(db, employer, "p-a"), 9);
        Application(db, Posting(db, employer, "p-b"), 9);
        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_applications WHERE applicant_user_id=9"));
    }

    [Fact]
    public void DifferentPeopleMayEachApplyToTheSamePosting()
    {
        var db = NewDb();
        var posting = Posting(db, Employer(db, "e-two"), "p-two");
        Application(db, posting, 10);
        Application(db, posting, 11);
        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_applications WHERE posting_id=?", posting));
    }

    // ── answers are snapshots (§28) ───────────────────────────────────────────────────────────

    [Fact]
    public void EditingAQuestionCannotChangeWhatAnApplicationSaid()
    {
        // Invariant 2. The prompt the applicant actually answered is copied into the answer row at
        // submission; the question row is the employer's working copy, free to change without
        // rewriting anyone's history.
        var db = NewDb();
        var posting = Posting(db, Employer(db, "e-snap"), "p-snap");
        var question = Question(db, posting, "Do you hold a current driving licence?");
        var app = Application(db, posting, 12);
        var answer = Answer(db, app, question, "Do you hold a current driving licence?", "Yes");

        db.Execute("UPDATE pciworld_job_questions SET prompt=? WHERE id=?",
            "Do you hold a current FORKLIFT licence?", question);

        var row = db.QueryOne("SELECT prompt_snapshot,answer_snapshot FROM pciworld_application_answers WHERE id=?", answer);
        Assert.Equal("Do you hold a current driving licence?", H.Str(row!["prompt_snapshot"]));
        Assert.Equal("Yes", H.Str(row["answer_snapshot"]));
    }

    [Fact]
    public void DeletingAQuestionLeavesTheAnswerReadable()
    {
        // The snapshot makes the answer row self-contained evidence: it does not need the question
        // row to exist to say what was asked. Without the copy, deleting a question would silently
        // delete the meaning of every answer to it.
        var db = NewDb();
        var posting = Posting(db, Employer(db, "e-del"), "p-del");
        var question = Question(db, posting, "Why this role?");
        var app = Application(db, posting, 13);
        Answer(db, app, question, "Why this role?", "Because of the schedule risk work.");

        db.Execute("DELETE FROM pciworld_job_questions WHERE id=?", question);

        var row = db.QueryOne("SELECT prompt_snapshot,answer_snapshot FROM pciworld_application_answers WHERE application_id=?", app);
        Assert.Equal("Why this role?", H.Str(row!["prompt_snapshot"]));
        Assert.Equal("Because of the schedule risk work.", H.Str(row["answer_snapshot"]));
    }

    [Fact]
    public void TheAnswerRowCarriesItsOwnPromptRatherThanBorrowingTheQuestions()
    {
        // If prompt_snapshot ever disappeared, the join back to pciworld_job_questions would
        // quietly become the source of truth — which is exactly the in-place mutability §28
        // forbids. The column existing is the precondition for the rule being enforceable.
        var cols = NewDb().Columns("pciworld_application_answers");
        Assert.Contains("prompt_snapshot", cols);
        Assert.Contains("answer_snapshot", cols);
    }

    // ── slugs, because this surface is crawlable ──────────────────────────────────────────────

    [Fact]
    public void EmployerSlugsAreUnique()
    {
        var db = NewDb();
        Employer(db, "acme-controls");
        Assert.ThrowsAny<Exception>(() => Employer(db, "acme-controls"));
    }

    [Fact]
    public void PostingSlugsAreUniqueWithinAnEmployerButNotAcrossThem()
    {
        // Two employers may each legitimately post a 'senior-planner' role; within one employer a
        // duplicate slug is a permanently ambiguous URL.
        var db = NewDb();
        var a = Employer(db, "e-slug-a");
        var b = Employer(db, "e-slug-b");
        Posting(db, a, "senior-planner");
        Posting(db, b, "senior-planner");
        Assert.ThrowsAny<Exception>(() => Posting(db, a, "senior-planner"));
    }

    // ── acting rights are rows ────────────────────────────────────────────────────────────────

    [Fact]
    public void OneMembershipRowPerPersonPerEmployer()
    {
        // A role change is an UPDATE; a second row would make "may this person act for this
        // employer, and as what" have two answers.
        var db = NewDb();
        var employer = Employer(db, "e-mem");
        db.Execute("INSERT INTO pciworld_employer_members(employer_id,user_id,role) VALUES(?,?,'owner')", employer, 20);
        Assert.ThrowsAny<Exception>(() =>
            db.Execute("INSERT INTO pciworld_employer_members(employer_id,user_id,role) VALUES(?,?,'member')", employer, 20));
    }

    // ── money is integers ─────────────────────────────────────────────────────────────────────

    [SqliteOnlyFact("Reads SQLite's own catalogue (PRAGMA table_info) to check DECLARED column types. On MySQL the type system enforces this at the engine level, covered by the parity leg.")]
    public void SalaryColumnsAreIntegerMinorUnitsNotFloatingPoint()
    {
        // A REAL salary rounds, and a rounded salary is a wrong salary shown to a person deciding
        // whether to apply. Declaring the columns INTEGER is what makes that mistake structural
        // rather than a code-review catch.
        var db = NewDb();
        var types = db.Query("PRAGMA table_info(pciworld_job_postings)")
            .ToDictionary(c => H.Str(c["name"])!, c => (H.Str(c["type"]) ?? "").ToUpperInvariant());
        Assert.Equal("INTEGER", types["salary_min_minor"]);
        Assert.Equal("INTEGER", types["salary_max_minor"]);
    }

    [Fact]
    public void SalariesTravelWithTheirCurrency()
    {
        // A number without its currency is not an amount. 4500000 minor units is a very different
        // offer in GBP and in JPY.
        var cols = NewDb().Columns("pciworld_job_postings");
        Assert.Contains("salary_min_minor", cols);
        Assert.Contains("salary_max_minor", cols);
        Assert.Contains("currency", cols);
    }

    // ── consent is a record, not a checkbox ───────────────────────────────────────────────────

    [Fact]
    public void ConsentWithdrawalIsAStampRatherThanADeletion()
    {
        // "Was there consent on the day the employer read this?" must stay answerable after the
        // consent is withdrawn — so withdrawal is a timestamp on the record, never its removal.
        // policy_version pins WHICH policy text was consented to.
        var db = NewDb();
        var employer = Employer(db, "e-consent");
        var app = Application(db, Posting(db, employer, "p-consent"), 30);
        db.Execute(@"INSERT INTO pciworld_application_consents(application_id,employer_id,purpose,granted_at,policy_version)
                     VALUES(?,?,'share_profile',datetime('now'),'v1')", app, employer);
        db.Execute("UPDATE pciworld_application_consents SET withdrawn_at=datetime('now') WHERE application_id=?", app);

        var row = db.QueryOne("SELECT granted_at,withdrawn_at,policy_version FROM pciworld_application_consents WHERE application_id=?", app);
        Assert.NotNull(row!["granted_at"]);
        Assert.NotNull(row["withdrawn_at"]);
        Assert.Equal("v1", H.Str(row["policy_version"]));
    }

    // ── install-time behaviour ────────────────────────────────────────────────────────────────

    [Fact]
    public void CareersShipsDisabled()
    {
        // Installing the schema is not launching the feature — a fresh database must not have the
        // marketplace reachable until an operator turns it on.
        Assert.False(Settings.Bool(NewDb(), "pciworld_careers_enabled", false));
    }

    [Fact]
    public void AnOperatorsChoiceSurvivesAReboot()
    {
        var db = NewDb();
        Settings.Put(db, "pciworld_careers_enabled", "1");
        CareersSchema.Ensure(db);
        Assert.True(Settings.Bool(db, "pciworld_careers_enabled", false));
    }

    [Fact]
    public void EnsureIsIdempotent()
    {
        // It runs on every boot, so running it twice must be a no-op rather than an error.
        var db = NewDb();
        CareersSchema.Ensure(db);
        CareersSchema.Ensure(db);
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_job_postings"));
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_applications"));
    }
}
