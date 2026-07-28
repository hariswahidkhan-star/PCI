using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The employer tenancy layer (Core/CareersEmployers.cs; docs/pciworld/CCP_PHASE4_DESIGN.md §4),
/// under whichever provider TEST_DB_PROVIDER selects.
///
/// The assertions that matter most here are the NEGATIVE ones, because this layer is the tenant
/// boundary the whole marketplace leans on: a member can never raise their own employer's state, a
/// member of employer A can never act on employer B, a stale write can never overwrite a
/// verification state, and suspension bites on the very next call rather than the next login. Each
/// of those is asserted by name — they are §2 prohibitions, not implementation details.
/// </summary>
[Collection(DbCollection.Name)]
public class CareersEmployersTests
{
    static Db NewDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        // The layer under test refuses tenant-side writes while the kill switch is off; these
        // tests exercise the layer, so they turn the feature on. The one test about the switch
        // itself leaves it off.
        Settings.Put(db, "pciworld_careers_enabled", "1");
        return db;
    }

    static long Admin(Db db, string email, string role) =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_admin_users(email,name,role,password_hash) VALUES(?,?,?,'x')",
            email, "Admin " + email, role);

    static int Version(Db db, long employerId) =>
        (int)db.Scalar<long>("SELECT version FROM pciworld_employers WHERE id=?", employerId);

    static string? State(Db db, long employerId) =>
        db.Scalar<string>("SELECT state FROM pciworld_employers WHERE id=?", employerId);

    /// <summary>An employer walked to 'verified' the only honest way: created by a member, then
    /// moved pending → verified by a real admin, version predicates and all.</summary>
    static (long EmployerId, long AdminId) VerifiedEmployer(Db db, long ownerUserId, string slug)
    {
        var created = CareersEmployers.Create(db, ownerUserId, "Employer " + slug, null, slug);
        Assert.True(created.Ok);
        var admin = Admin(db, slug + "@pci.test", "owner");
        Assert.Null(CareersEmployers.SetState(db, created.EmployerId, "pending_verification", admin, 1));
        Assert.Null(CareersEmployers.SetState(db, created.EmployerId, "verified", admin, 2));
        return (created.EmployerId, admin);
    }

    // ── creation (§4.1) ───────────────────────────────────────────────────────────────────────

    [Fact]
    public void ANewEmployerStartsInDraftNeverVerified()
    {
        // 'verified' is an assertion PCI makes to candidates (§4.2); a creation path that could
        // reach it directly would make that assertion without anyone having made it.
        var db = NewDb();
        var r = CareersEmployers.Create(db, 7, "Acme Controls", "https://acme.example", "acme-controls");
        Assert.True(r.Ok);

        var row = db.QueryOne("SELECT state,verified_at,verified_by_admin_id FROM pciworld_employers WHERE id=?", r.EmployerId)!;
        Assert.Equal("draft", H.Str(row["state"]));
        Assert.Null(row["verified_at"]);
        Assert.Null(row["verified_by_admin_id"]);
    }

    [Fact]
    public void TheCreatorBecomesTheOwnerMemberInTheSameTransaction()
    {
        // An employer with nobody able to act for it is a dead row — so the membership is part of
        // creation, not a follow-up call that can be forgotten or fail separately.
        var db = NewDb();
        var r = CareersEmployers.Create(db, 7, "Acme Controls", null, "acme-controls");
        Assert.True(r.Ok);
        Assert.True(CareersEmployers.IsMember(db, r.EmployerId, 7));
        Assert.Equal("owner", CareersEmployers.RoleOf(db, r.EmployerId, 7));
    }

    [Fact]
    public void ADuplicateSlugIsRefusedAndLeavesNoOrphanRows()
    {
        // The employer page is a crawlable URL; a duplicate slug is a permanently ambiguous
        // address. The refusal must also not leave a half-created tenant behind.
        var db = NewDb();
        Assert.True(CareersEmployers.Create(db, 7, "Acme Controls", null, "acme-controls").Ok);

        var r = CareersEmployers.Create(db, 8, "Acme Controls Ltd", null, "acme-controls");
        Assert.False(r.Ok);
        Assert.Equal("slug_taken", r.Reason);
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_employers WHERE slug='acme-controls'"));
        Assert.False(CareersEmployers.IsMember(db, 0, 8));
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_employer_members WHERE user_id=8"));
    }

    [Fact]
    public void ASlugIsDerivedFromTheNameWhenNoneIsGiven()
    {
        var db = NewDb();
        var r = CareersEmployers.Create(db, 7, "Acme Controls Ltd.", null, null);
        Assert.True(r.Ok);
        Assert.Equal("acme-controls-ltd", r.Slug);
    }

    [Fact]
    public void ACallerSuppliedSlugIsCanonicalisedToLowercase()
    {
        // Case-folding is canonicalisation, not repair: the result is deterministic from the
        // input, and it strengthens global uniqueness — "ACME" and "acme" can never coexist as
        // two different crawlable addresses.
        var db = NewDb();
        var r = CareersEmployers.Create(db, 7, "Acme Controls", null, "ACME-Controls");
        Assert.True(r.Ok);
        Assert.Equal("acme-controls", r.Slug);
    }

    [Theory]
    [InlineData("Has Spaces")]
    [InlineData("a")]                 // too short to be an address
    [InlineData("-leading-hyphen")]
    [InlineData("double--hyphen")]
    [InlineData("dot.com")]
    public void AMalformedSlugIsRefusedNotRepaired(string slug)
    {
        // Silently rewriting the address the caller asked for would mint a permanent URL nobody
        // chose. Refusal keeps the caller in charge of their own address.
        var db = NewDb();
        var r = CareersEmployers.Create(db, 7, "Acme Controls", null, slug);
        Assert.False(r.Ok);
        Assert.Equal("bad_slug", r.Reason);
    }

    [Fact]
    public void AWebsiteMustBeAnAbsoluteHttpUrlBecauseItIsEmittedToCrawlers()
    {
        // §7 emits the recorded website as the verified employer's `sameAs`; javascript: or a bare
        // word must be refused at the door, not published later.
        var db = NewDb();
        Assert.Equal("bad_website", CareersEmployers.Create(db, 7, "Acme", "javascript:alert(1)", "acme-a").Reason);
        Assert.Equal("bad_website", CareersEmployers.Create(db, 7, "Acme", "not a url", "acme-b").Reason);
        Assert.True(CareersEmployers.Create(db, 7, "Acme", "https://acme.example/jobs", "acme-c").Ok);
    }

    [Fact]
    public void CreationIsRefusedWhileTheKillSwitchIsOff()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);   // seeds the flag '0'
        var r = CareersEmployers.Create(db, 7, "Acme Controls", null, "acme-controls");
        Assert.False(r.Ok);
        Assert.Equal("careers_disabled", r.Reason);
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_employers"));
    }

    // ── membership: acting rights are rows (§4.1) ─────────────────────────────────────────────

    [Fact]
    public void ARoleChangeIsAnUpdateNeverASecondRow()
    {
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        Assert.Null(CareersEmployers.AddMember(db, e, 7, 8, "member"));
        Assert.Null(CareersEmployers.AddMember(db, e, 7, 8, "owner"));

        Assert.Equal("owner", CareersEmployers.RoleOf(db, e, 8));
        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_employer_members WHERE employer_id=? AND user_id=8", e));
    }

    [Fact]
    public void OnlyAnOwnerMayAddOrPromoteMembers()
    {
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        Assert.Null(CareersEmployers.AddMember(db, e, 7, 8, "member"));

        // A plain member cannot grow the tenant's set of acting accounts…
        Assert.Equal("not_employer_owner", CareersEmployers.AddMember(db, e, 8, 9, "member"));
        // …and cannot promote themselves.
        Assert.Equal("not_employer_owner", CareersEmployers.AddMember(db, e, 8, 8, "owner"));
        Assert.Equal("member", CareersEmployers.RoleOf(db, e, 8));
        Assert.False(CareersEmployers.IsMember(db, e, 9));
    }

    [Fact]
    public void AnOwnerOfEmployerACannotAddMembersToEmployerB()
    {
        // §2: never let a member of employer A act on employer B — the acting user's role is read
        // WITH the employer_id predicate, so owning A is worth nothing at B.
        var db = NewDb();
        CareersEmployers.Create(db, 7, "Acme", null, "acme");
        var b = CareersEmployers.Create(db, 8, "Borealis", null, "borealis").EmployerId;

        Assert.Equal("not_employer_owner", CareersEmployers.AddMember(db, b, 7, 9, "member"));
        Assert.False(CareersEmployers.IsMember(db, b, 9));
        Assert.False(CareersEmployers.IsMember(db, b, 7));
    }

    [Fact]
    public void TheLastOwnerCanNeitherBeRemovedNorDemoted()
    {
        // Removing (or demoting) the only owner recreates the dead-row problem creation exists to
        // prevent: a tenant nobody can administer.
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        Assert.Null(CareersEmployers.AddMember(db, e, 7, 8, "member"));

        Assert.Equal("last_owner", CareersEmployers.RemoveMember(db, e, 7, 7));
        Assert.Equal("last_owner", CareersEmployers.AddMember(db, e, 7, 7, "member"));
        Assert.Equal("owner", CareersEmployers.RoleOf(db, e, 7));

        // With a second owner in place, the first may step down.
        Assert.Null(CareersEmployers.AddMember(db, e, 7, 8, "owner"));
        Assert.Null(CareersEmployers.RemoveMember(db, e, 7, 7));
        Assert.False(CareersEmployers.IsMember(db, e, 7));
    }

    [Fact]
    public void AMemberMayRemoveThemselvesButNotOthers()
    {
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        Assert.Null(CareersEmployers.AddMember(db, e, 7, 8, "member"));
        Assert.Null(CareersEmployers.AddMember(db, e, 7, 9, "member"));

        Assert.Equal("not_employer_owner", CareersEmployers.RemoveMember(db, e, 8, 9));
        Assert.True(CareersEmployers.IsMember(db, e, 9));
        Assert.Null(CareersEmployers.RemoveMember(db, e, 8, 8));
        Assert.False(CareersEmployers.IsMember(db, e, 8));
    }

    // ── verification is an attributable admin act (§4.2) ──────────────────────────────────────

    [Fact]
    public void AnEmployerMemberCanNeverVerifyTheirOwnEmployer()
    {
        // The §2 prohibition, by name. A member holds a pciworld_users id; verification demands an
        // active pciworld_admin_users row with careers.verify — so every value a member could pass
        // as adminId is refused: null, and their own (non-admin) user id.
        var db = NewDb();
        var e = CareersEmployers.Create(db, 77, "Acme", null, "acme").EmployerId;

        Assert.Equal("admin_required", CareersEmployers.SetState(db, e, "pending_verification", null, 1));
        Assert.Equal("forbidden", CareersEmployers.SetState(db, e, "verified", 77, 1));
        Assert.Equal("draft", State(db, e));
        Assert.Null(db.QueryOne("SELECT verified_at FROM pciworld_employers WHERE id=?", e)!["verified_at"]);
    }

    [Fact]
    public void AnAdminWithoutCareersVerifyCannotChangeEmployerState()
    {
        // §4.1 keeps the community precedent: a person who moderates a forum has no business
        // verifying an employer. Editorial and community roles all lack careers.verify.
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        foreach (var role in new[] { "viewer", "author", "live_moderator", "trust_safety" })
        {
            var admin = Admin(db, role + "@pci.test", role);
            Assert.Equal("forbidden", CareersEmployers.SetState(db, e, "pending_verification", admin, 1));
        }
        Assert.Equal("draft", State(db, e));
    }

    [Fact]
    public void VerificationRecordsWhoAssertedItAndWhen()
    {
        // A bare boolean would be an unattributable claim nobody could later explain to a
        // candidate who relied on it (§4.2).
        var db = NewDb();
        var (e, admin) = VerifiedEmployer(db, 7, "acme");

        var row = db.QueryOne("SELECT state,verified_at,verified_by_admin_id FROM pciworld_employers WHERE id=?", e)!;
        Assert.Equal("verified", H.Str(row["state"]));
        Assert.Equal(admin, H.L(row["verified_by_admin_id"]));
        Assert.False(string.IsNullOrEmpty(H.Str(row["verified_at"])));
    }

    [Fact]
    public void EveryStateChangeWritesAnAuditRow()
    {
        // Suspension and re-verification are audited state changes (§4.2): the assertion's history
        // must survive every subsequent edit of the row.
        var db = NewDb();
        var before = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_audit WHERE action='careers_employer_state'");
        var (e, admin) = VerifiedEmployer(db, 7, "acme");
        Assert.Null(CareersEmployers.SetState(db, e, "suspended", admin, 3));

        Assert.Equal(before + 3, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_audit WHERE action='careers_employer_state'"));
        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_audit WHERE action='careers_employer_state' AND detail=? AND admin_id=?",
            $"employer:{e} verified->suspended", admin));
    }

    [Fact]
    public void TheStateMachineRefusesASkipStraightToVerified()
    {
        // draft → verified without pending_verification would be a verification nobody queued,
        // reviewed or could be budgeted (§8) — the transition table owns this, SetState enforces it.
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        var admin = Admin(db, "owner@pci.test", "owner");

        Assert.Equal("invalid_transition", CareersEmployers.SetState(db, e, "verified", admin, 1));
        Assert.Equal("draft", State(db, e));
    }

    // ── optimistic concurrency (§13.6) ────────────────────────────────────────────────────────

    [Fact]
    public void AStaleWriteReportsAConflictRatherThanOverwriting()
    {
        // A lost update on a verification state is a candidate being told a fake employer is real.
        // The second writer holding yesterday's version must be told so, not silently win.
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        var admin = Admin(db, "owner@pci.test", "owner");

        Assert.Equal(1, Version(db, e));
        Assert.Null(CareersEmployers.SetState(db, e, "pending_verification", admin, 1));
        Assert.Equal(2, Version(db, e));

        // Replaying the same call with the version it already consumed must conflict…
        Assert.Equal("conflict", CareersEmployers.SetState(db, e, "pending_verification", admin, 1));
        // …and so must any other transition carrying the stale version, even a valid one.
        Assert.Equal("conflict", CareersEmployers.SetState(db, e, "verified", admin, 1));
        Assert.Equal("pending_verification", State(db, e));
        Assert.Equal(2, Version(db, e));
    }

    [Fact]
    public void ASuspensionCannotBeUndoneByAConcurrentEditorsStaleWrite()
    {
        // The §28 scenario spelled out: admin A suspends; admin B, still holding the pre-suspension
        // version, tries to (re-)verify. B must conflict — the employer stays suspended.
        var db = NewDb();
        var (e, adminA) = VerifiedEmployer(db, 7, "acme");
        var adminB = Admin(db, "second@pci.test", "owner");

        var versionBRead = Version(db, e);                                   // B reads: 3, verified
        Assert.Null(CareersEmployers.SetState(db, e, "suspended", adminA, versionBRead));
        Assert.Equal("conflict", CareersEmployers.SetState(db, e, "verified", adminB, versionBRead));
        Assert.Equal("suspended", State(db, e));
    }

    // ── the publish gate (§4.3) ───────────────────────────────────────────────────────────────

    [Fact]
    public void MayPublishIsTrueOnlyForVerified()
    {
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        var admin = Admin(db, "owner@pci.test", "owner");

        Assert.False(CareersEmployers.MayPublish(db, e));                    // draft
        Assert.Null(CareersEmployers.SetState(db, e, "pending_verification", admin, 1));
        Assert.False(CareersEmployers.MayPublish(db, e));                    // pending
        Assert.Null(CareersEmployers.SetState(db, e, "verified", admin, 2));
        Assert.True(CareersEmployers.MayPublish(db, e));
        Assert.Null(CareersEmployers.SetState(db, e, "suspended", admin, 3));
        Assert.False(CareersEmployers.MayPublish(db, e));                    // suspended
        Assert.False(CareersEmployers.MayPublish(db, 999_999));              // no such employer
    }

    // ── acting for an employer: §6 conditions (a) and (d) ────────────────────────────────────

    [Fact]
    public void MayActForRequiresMembershipOfThatVeryEmployer()
    {
        // §2 by name: a member of employer A — even its owner — can never act on employer B, by
        // id. Both employers are verified, so the ONLY thing separating true from false is the
        // employer_id predicate on the membership read.
        var db = NewDb();
        var (a, _) = VerifiedEmployer(db, 7, "acme");
        var (b, _) = VerifiedEmployer(db, 8, "borealis");

        Assert.True(CareersEmployers.MayActFor(db, a, 7));
        Assert.True(CareersEmployers.MayActFor(db, b, 8));
        Assert.False(CareersEmployers.MayActFor(db, b, 7));   // A's owner, B's data: never
        Assert.False(CareersEmployers.MayActFor(db, a, 8));
        Assert.False(CareersEmployers.MayActFor(db, a, 9));   // a stranger
    }

    [Fact]
    public void SuspensionCutsActingRightsOnTheVeryNextCall()
    {
        // Per request, not per login (§8): the same member, the same session, one suspension
        // in between — the second answer must already be no. Nothing may cache the first answer.
        var db = NewDb();
        var (e, admin) = VerifiedEmployer(db, 7, "acme");

        Assert.True(CareersEmployers.MayActFor(db, e, 7));
        Assert.Null(CareersEmployers.SetState(db, e, "suspended", admin, 3));
        Assert.False(CareersEmployers.MayActFor(db, e, 7));

        // Membership itself survives — it is the employer's standing that fell, not the row.
        Assert.True(CareersEmployers.IsMember(db, e, 7));
    }

    [Fact]
    public void AnUnverifiedEmployersMembersCannotActEitherWay()
    {
        // Pre-verification there is no applicant data to reach (§8), and MayActFor says so for
        // draft and pending alike: (d) fails exactly like suspension.
        var db = NewDb();
        var e = CareersEmployers.Create(db, 7, "Acme", null, "acme").EmployerId;
        Assert.False(CareersEmployers.MayActFor(db, e, 7));

        var admin = Admin(db, "owner@pci.test", "owner");
        Assert.Null(CareersEmployers.SetState(db, e, "pending_verification", admin, 1));
        Assert.False(CareersEmployers.MayActFor(db, e, 7));
    }

    [Fact]
    public void AFormerMemberLosesActingRightsAtRemoval()
    {
        var db = NewDb();
        var (e, _) = VerifiedEmployer(db, 7, "acme");
        Assert.Null(CareersEmployers.AddMember(db, e, 7, 8, "member"));
        Assert.True(CareersEmployers.MayActFor(db, e, 8));

        Assert.Null(CareersEmployers.RemoveMember(db, e, 7, 8));
        Assert.False(CareersEmployers.MayActFor(db, e, 8));
    }
}
