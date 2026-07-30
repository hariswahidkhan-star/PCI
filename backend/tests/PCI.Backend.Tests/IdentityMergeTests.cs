using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Maker-checker duplicate-account merges (Core/IdentityMerge.cs, spec §4.11).
///
/// Two invariants dominate: the TWO-PERSON RULE (the admin who requested a merge can never be the
/// one who approves it, even if they hold both permissions — the check is on identity, not on
/// permission overlap) and NUMBER PERMANENCE (the survivor's Student Number never changes, and the
/// loser's is never freed — it goes to state='merged' in the registry, resolving to the survivor).
/// Runs on SQLite and MySQL/MariaDB via TestEnv, so the deterministic-order locking and the
/// guarded decision claim are exercised on the provider that actually runs production.
/// </summary>
public class IdentityMergeTests
{
    const long Maker = 11, Checker = 22;

    static Db NewDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long Student(Db db, string email)
    {
        long id = 0;
        db.Transaction(() =>
        {
            id = db.ExecuteReturningId(
                "INSERT INTO users(email,first_name,last_name,role,status) VALUES(?, 'S','N','student','active')", email);
            StudentNumbers.GetOrIssue(db, id, "test");
        });
        return id;
    }

    [Fact]
    public void The_maker_can_never_approve_their_own_request()
    {
        var db = NewDb();
        var loser = Student(db, "m1-loser@x.test");
        var survivor = Student(db, "m1-survivor@x.test");
        var (id, err) = IdentityMerge.CreateRequest(db, loser, survivor, "dupe", Maker);
        Assert.Null(err);

        var (refusal, _) = IdentityMerge.Approve(db, id, Maker, "trying anyway");

        Assert.Equal("maker_checker", refusal);
        // Nothing executed: the request is still pending and both accounts untouched.
        Assert.Equal("pending", db.Scalar<string>("SELECT status FROM pci_identity_merges WHERE id=?", id));
        Assert.Equal("active", db.Scalar<string>("SELECT status FROM users WHERE id=?", loser));
    }

    [Fact]
    public void A_different_admin_approves_and_the_merge_executes_atomically()
    {
        var db = NewDb();
        var loser = Student(db, "m2-loser@x.test");
        var survivor = Student(db, "m2-survivor@x.test");
        var survivorNumber = db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", survivor);
        var loserNumber = db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", loser);
        db.Execute("INSERT INTO payments(user_id,product_type,final_amount,payment_status) VALUES(?, 'exam',300,'paid')", loser);
        db.Execute("INSERT INTO login_tokens(user_id,token,purpose) VALUES(?, 'm2-tok-l','session')", loser);
        db.Execute("INSERT INTO login_tokens(user_id,token,purpose) VALUES(?, 'm2-tok-s','session')", survivor);

        var (id, _) = IdentityMerge.CreateRequest(db, loser, survivor, "dupe", Maker);
        var (err, result) = IdentityMerge.Approve(db, id, Checker, "verified");
        Assert.Null(err);
        Assert.NotNull(result);

        // The survivor keeps their number untouched; the loser's registry row records the merge.
        Assert.Equal(survivorNumber, db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", survivor));
        var reg = db.QueryOne("SELECT state,merged_into_student_number,resolves_to_user_id FROM pci_student_number_registry WHERE student_number=?", loserNumber)!;
        Assert.Equal("merged", H.Str(reg["state"]));
        Assert.Equal(survivorNumber, H.Str(reg["merged_into_student_number"]));
        Assert.Equal(survivor, H.L(reg["resolves_to_user_id"]));

        // The loser is an inert shell whose email is freed; children re-pointed; sessions gone.
        var shell = db.QueryOne("SELECT email,status,registration_no FROM users WHERE id=?", loser)!;
        Assert.Equal("merged", H.Str(shell["status"]));
        Assert.Equal($"merged-{loser}-m2-loser@x.test", H.Str(shell["email"]));
        Assert.True(string.IsNullOrEmpty(H.Str(shell["registration_no"])));
        Assert.True(db.ExecuteReturningId(
            "INSERT INTO users(email,role,status) VALUES('m2-loser@x.test','student','active')") > 0);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM payments WHERE user_id=?", survivor));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM login_tokens WHERE user_id IN (?,?)", loser, survivor));

        // The decision and the before/after snapshots are on the record.
        var row = db.QueryOne("SELECT decided_by,before_json,after_json FROM pci_identity_merges WHERE id=?", id)!;
        Assert.Equal(Checker, H.L(row["decided_by"]));
        Assert.Contains("\"counts\"", H.Str(row["before_json"]));
        Assert.Contains("\"merged\"", H.Str(row["after_json"]));
    }

    [Fact]
    public void A_decided_request_can_never_be_decided_again()
    {
        var db = NewDb();
        var loser = Student(db, "m3-loser@x.test");
        var survivor = Student(db, "m3-survivor@x.test");
        var (id, _) = IdentityMerge.CreateRequest(db, loser, survivor, "dupe", Maker);
        Assert.Null(IdentityMerge.Approve(db, id, Checker, null).error);

        Assert.Equal("already_decided", IdentityMerge.Approve(db, id, Checker, null).error);
        Assert.Equal("already_decided", IdentityMerge.Reject(db, id, Checker, null));
    }

    [Fact]
    public void Reject_is_terminal_and_touches_neither_account()
    {
        var db = NewDb();
        var loser = Student(db, "m4-loser@x.test");
        var survivor = Student(db, "m4-survivor@x.test");
        var loserNumber = db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", loser);
        var (id, _) = IdentityMerge.CreateRequest(db, loser, survivor, "mistake", Maker);

        Assert.Null(IdentityMerge.Reject(db, id, Checker, "not the same person"));

        var row = db.QueryOne("SELECT status,decided_by,decision_note FROM pci_identity_merges WHERE id=?", id)!;
        Assert.Equal("rejected", H.Str(row["status"]));
        Assert.Equal(Checker, H.L(row["decided_by"]));
        Assert.Equal("not the same person", H.Str(row["decision_note"]));
        Assert.Equal("already_decided", IdentityMerge.Approve(db, id, Checker, null).error);
        Assert.Equal(loserNumber, db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", loser));
        // A rejected pair may be requested again — rejection closes the request, not the case.
        Assert.Null(IdentityMerge.CreateRequest(db, loser, survivor, "second look", Maker).error);
    }

    [Fact]
    public void Requests_refuse_self_merges_unknown_users_non_students_and_busy_pairs()
    {
        var db = NewDb();
        var a = Student(db, "m5-a@x.test");
        var b = Student(db, "m5-b@x.test");
        var c = Student(db, "m5-c@x.test");
        var admin = db.ExecuteReturningId("INSERT INTO users(email,role,status) VALUES('m5-adm@x.test','admin','active')");

        Assert.Equal("self_merge", IdentityMerge.CreateRequest(db, a, a, null, Maker).error);
        Assert.Equal("unknown_user", IdentityMerge.CreateRequest(db, a, 987654, null, Maker).error);
        Assert.Equal("not_student", IdentityMerge.CreateRequest(db, a, admin, null, Maker).error);
        Assert.Null(IdentityMerge.CreateRequest(db, a, b, null, Maker).error);
        // Anyone already inside a pending merge is refused on either side of a new one.
        Assert.Equal("already_pending", IdentityMerge.CreateRequest(db, a, c, null, Maker).error);
        Assert.Equal("already_pending", IdentityMerge.CreateRequest(db, c, b, null, Maker).error);
    }

    [Fact]
    public void Colliding_unique_rows_keep_the_survivors_and_mark_the_losers()
    {
        var db = NewDb();
        var loser = Student(db, "m6-loser@x.test");
        var survivor = Student(db, "m6-survivor@x.test");
        // Both were credited CPD for event 500 (UNIQUE(source_event_id,user_id)); the loser alone
        // holds a credit for 501. Both registered for event 600 (UNIQUE(event_id,user_id)).
        db.Execute("INSERT INTO cpd_entries(user_id,hours,source_event_id,status) VALUES(?,1,500,'approved')", loser);
        db.Execute("INSERT INTO cpd_entries(user_id,hours,source_event_id,status) VALUES(?,1,501,'approved')", loser);
        db.Execute("INSERT INTO cpd_entries(user_id,hours,source_event_id,status) VALUES(?,1,500,'approved')", survivor);
        db.Execute("INSERT INTO event_registrations(event_id,user_id,status) VALUES(600,?,'attended')", loser);
        db.Execute("INSERT INTO event_registrations(event_id,user_id,status) VALUES(600,?,'registered')", survivor);
        db.Execute("INSERT INTO student_profiles(user_id,city) VALUES(?, 'LoserCity')", loser);
        db.Execute("INSERT INTO student_profiles(user_id,city) VALUES(?, 'SurvivorCity')", survivor);

        var (id, _) = IdentityMerge.CreateRequest(db, loser, survivor, "dupe", Maker);
        Assert.Null(IdentityMerge.Approve(db, id, Checker, null).error);

        // Exactly one credit per (event, person) survives; the loser's duplicate is marked, not moved.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM cpd_entries WHERE user_id=? AND source_event_id=500", survivor));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM cpd_entries WHERE user_id=? AND source_event_id=501", survivor));
        Assert.Equal("merged_duplicate", db.Scalar<string>(
            "SELECT status FROM cpd_entries WHERE user_id=? AND source_event_id=500", loser));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM event_registrations WHERE event_id=600 AND user_id=?", survivor));
        Assert.Equal("registered", db.Scalar<string>(
            "SELECT status FROM event_registrations WHERE event_id=600 AND user_id=?", survivor));
        Assert.Equal("merged_duplicate", db.Scalar<string>(
            "SELECT status FROM event_registrations WHERE event_id=600 AND user_id=?", loser));
        Assert.Equal("SurvivorCity", db.Scalar<string>("SELECT city FROM student_profiles WHERE user_id=?", survivor));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM student_profiles WHERE user_id=?", loser));
    }

    [Fact]
    public void World_evidence_follows_the_person_and_handoff_codes_die_for_both()
    {
        var db = NewDb();
        var loser = Student(db, "m7-loser@x.test");
        var survivor = Student(db, "m7-survivor@x.test");
        var wid = db.ExecuteReturningId(
            "INSERT INTO pciworld_users(email,password_hash,student_user_id) VALUES('m7-loser@x.test','x',?)", loser);
        db.Execute("INSERT INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome) VALUES(?,?, 'linked')", wid, loser);
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,user_id,canonical_user_id,status,passport_visible)
            VALUES(1,1,1,?,?, 'completed',1)", wid, loser);
        db.Execute("INSERT INTO pciworld_handoff_codes(code_sha,world_user_id,expires_at) VALUES('m7sha',?,datetime('now','+2 minutes'))", wid);

        var (id, _) = IdentityMerge.CreateRequest(db, loser, survivor, "dupe", Maker);
        Assert.Null(IdentityMerge.Approve(db, id, Checker, null).error);

        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts WHERE canonical_user_id=?", survivor));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts WHERE canonical_user_id=?", loser));
        Assert.Equal(survivor, db.Scalar<long>("SELECT student_user_id FROM pciworld_users WHERE id=?", wid));
        Assert.Equal(survivor, db.Scalar<long>("SELECT canonical_user_id FROM pciworld_user_map WHERE legacy_world_id=?", wid));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_handoff_codes WHERE world_user_id=?", wid));
    }

    [Fact]
    public void The_detail_preview_reports_counts_for_both_accounts()
    {
        var db = NewDb();
        var loser = Student(db, "m8-loser@x.test");
        var survivor = Student(db, "m8-survivor@x.test");
        db.Execute("INSERT INTO payments(user_id,product_type,final_amount,payment_status) VALUES(?, 'exam',300,'paid')", loser);
        db.Execute("INSERT INTO login_tokens(user_id,token,purpose) VALUES(?, 'm8-tok','session')", survivor);
        var (id, _) = IdentityMerge.CreateRequest(db, loser, survivor, "dupe", Maker);

        var detail = IdentityMerge.Get(db, id);

        Assert.NotNull(detail);
        var preview = (Dictionary<string, object?>)detail!["preview"]!;
        var src = (Dictionary<string, object?>)preview["source"]!;
        var dst = (Dictionary<string, object?>)preview["target"]!;
        Assert.Equal(1L, H.L(src["payments"]));
        Assert.Equal(1L, H.L(dst["sessions"]));
        // Counts only — the preview never carries member records.
        Assert.DoesNotContain("email", src.Keys, StringComparer.OrdinalIgnoreCase);
        Assert.Null(IdentityMerge.Get(db, 987654));
    }
}
