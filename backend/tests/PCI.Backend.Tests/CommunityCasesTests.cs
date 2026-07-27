using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World community — moderation cases, sanctions and appeals (Core/CommunityCases.cs).
///
/// These target the controls that are easy to implement plausibly and wrongly: maker-checker that
/// can be satisfied by the same person, an appeal credential that a public reference alone
/// unlocks, an overturn that quietly rewrites the record it should be adding to, and a restricted
/// case that leaks into the ordinary queue.
/// </summary>
[Collection(DbCollection.Name)]
public class CommunityCasesTests
{
    static Db Fresh()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    // ── Cases ─────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void RepeatedReportsAboutOneParticipantProduceOneCase()
    {
        // A brigading burst must give moderators one thing to work, not twenty.
        var db = Fresh();
        var a = CommunityCases.OpenOrGet(db, 1, "guest_session", "42", "normal", "abuse_blocked");
        var b = CommunityCases.OpenOrGet(db, 1, "guest_session", "42", "normal", "abuse_blocked");
        Assert.Equal(a, b);
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_moderation_cases"));
    }

    [Fact]
    public void ClosingACaseFreesTheSubjectForAFreshOne()
    {
        var db = Fresh();
        var first = CommunityCases.OpenOrGet(db, 1, "guest_session", "42", "normal", "abuse_blocked");
        CommunityCases.Close(db, first, 1, "dismissed", "not a violation");
        var second = CommunityCases.OpenOrGet(db, 1, "guest_session", "42", "normal", "abuse_blocked");
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void EveryStateChangeLeavesATrail()
    {
        var db = Fresh();
        var id = CommunityCases.OpenOrGet(db, 1, "guest_session", "7", "high", "hate_severe");
        CommunityCases.Assign(db, id, 5);
        CommunityCases.Close(db, id, 5, "actioned", "ejected");

        var events = db.Query("SELECT event FROM pciworld_moderation_case_events WHERE case_id=? ORDER BY id", id);
        Assert.Equal(new[] { "case_opened", "assigned", "case_closed" }, events.Select(e => H.Str(e["event"])).ToArray());
    }

    // ── Restricted cases stay out of the ordinary queue ───────────────────────────────────────

    [Fact]
    public void ARestrictedCaseNeverAppearsInTheOrdinaryQueue()
    {
        // §8.7/§28.7: suspected child-safety material must not reach a routine moderator's list.
        var db = Fresh();
        CommunityCases.OpenOrGet(db, 1, "guest_session", "1", "normal", "spam_blocked");
        CommunityCases.OpenOrGet(db, 1, "guest_session", "2", "critical", "grooming_severe", restricted: true);

        var ordinary = CommunityCases.Queue(db);
        Assert.Single(ordinary);
        Assert.Equal("2", H.Str(CommunityCases.RestrictedQueue(db)[0]["subject_ref"]));
        Assert.DoesNotContain(ordinary, c => H.B(c["restricted"]));
    }

    [Fact]
    public void TheQueueSurfacesTheWorstAndTheOldestFirst()
    {
        var db = Fresh();
        CommunityCases.OpenOrGet(db, 1, "guest_session", "low", "low", "spam_review");
        CommunityCases.OpenOrGet(db, 1, "guest_session", "crit", "critical", "hate_severe");
        CommunityCases.OpenOrGet(db, 1, "guest_session", "norm", "normal", "abuse_blocked");

        var q = CommunityCases.Queue(db);
        Assert.Equal("crit", H.Str(q[0]["subject_ref"]));
    }

    // ── Maker-checker ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public void APermanentSanctionHasNoEffectUntilSomeoneElseApprovesIt()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "9", "critical", "hate_severe");
        var s = CommunityCases.Issue(db, "risk_key", "rk-9", 1, "permanent", "hate_severe", 10, caseId, null);
        Assert.True(s.Ok);

        var row = db.QueryOne("SELECT * FROM pciworld_sanctions WHERE id=?", s.SanctionId)!;
        Assert.False(CommunityCases.IsInEffect(row), "a pending permanent sanction must not bite");
        Assert.Empty(CommunityCases.ActiveFor(db, "risk_key", "rk-9"));

        Assert.True(CommunityCases.Approve(db, s.SanctionId, 11).Ok);
        var approved = db.QueryOne("SELECT * FROM pciworld_sanctions WHERE id=?", s.SanctionId)!;
        Assert.True(CommunityCases.IsInEffect(approved));
        Assert.Single(CommunityCases.ActiveFor(db, "risk_key", "rk-9"));
    }

    [Fact]
    public void TheIssuerCannotApproveTheirOwnPermanentSanction()
    {
        // The entire point of the control. One account must not be able to exclude someone alone.
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "9", "critical", "hate_severe");
        var s = CommunityCases.Issue(db, "risk_key", "rk-9", 1, "permanent", "hate_severe", 10, caseId, null);

        var self = CommunityCases.Approve(db, s.SanctionId, 10);
        Assert.False(self.Ok);
        Assert.Equal("maker_checker", self.ErrorCode);

        var row = db.QueryOne("SELECT * FROM pciworld_sanctions WHERE id=?", s.SanctionId)!;
        Assert.False(CommunityCases.IsInEffect(row));
    }

    [Fact]
    public void AnApprovedSanctionCannotBeApprovedAgain()
    {
        var db = Fresh();
        var s = CommunityCases.Issue(db, "risk_key", "rk-1", 1, "permanent", "hate_severe", 10, null, null);
        Assert.True(CommunityCases.Approve(db, s.SanctionId, 11).Ok);
        Assert.Equal("already_approved", CommunityCases.Approve(db, s.SanctionId, 12).ErrorCode);
    }

    [Fact]
    public void AnImmediateSanctionTakesEffectWithoutASecondSignature()
    {
        // A live room cannot wait for a second reviewer before ejecting someone abusive.
        var db = Fresh();
        var s = CommunityCases.Issue(db, "risk_key", "rk-2", 1, "eject", "abuse_severe", 10, null, 60);
        var row = db.QueryOne("SELECT * FROM pciworld_sanctions WHERE id=?", s.SanctionId)!;
        Assert.True(CommunityCases.IsInEffect(row));
    }

    [Fact]
    public void APermanentSanctionCannotCarryAnExpiry()
    {
        var db = Fresh();
        var s = CommunityCases.Issue(db, "risk_key", "rk-3", 1, "permanent", "hate_severe", 10, null, 60);
        Assert.False(s.Ok);
        Assert.Equal("permanent_cannot_expire", s.ErrorCode);
    }

    [Fact]
    public void ASanctionAlwaysCarriesAReason()
    {
        var db = Fresh();
        Assert.Equal("reason_required", CommunityCases.Issue(db, "risk_key", "rk", 1, "mute", "  ", 10, null, 30).ErrorCode);
    }

    [Fact]
    public void AnExpiredSanctionStopsBiting()
    {
        // A time-bounded sanction that never ends is a permanent one imposed without approval.
        var db = Fresh();
        var s = CommunityCases.Issue(db, "risk_key", "rk-4", 1, "mute", "spam_blocked", 10, null, 1);
        db.Execute("UPDATE pciworld_sanctions SET expires_at=? WHERE id=?", TestEnv.Stamp("-1 hour"), s.SanctionId);
        Assert.Empty(CommunityCases.ActiveFor(db, "risk_key", "rk-4"));
    }

    // ── Appeals: the reference grants nothing, the credential is the key ──────────────────────

    [Fact]
    public void ThePublicReferenceAloneUnlocksNothing()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        var ticket = CommunityCases.IssueAppeal(db, caseId);

        Assert.Null(CommunityCases.ByCredential(db, ticket.PublicReference, null));
        Assert.Null(CommunityCases.ByCredential(db, ticket.PublicReference, "not-the-credential"));
        Assert.NotNull(CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential));
    }

    [Fact]
    public void TheCredentialIsNeverStoredInTheClear()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        var ticket = CommunityCases.IssueAppeal(db, caseId);
        Assert.Equal(0, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_appeals WHERE credential_sha=?", ticket.Credential));
        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_appeals WHERE credential_sha=?", Security.Sha(ticket.Credential)));
    }

    [Fact]
    public void ACredentialCannotBeBruteForced()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        var ticket = CommunityCases.IssueAppeal(db, caseId);

        for (var i = 0; i < 25; i++) CommunityCases.ByCredential(db, ticket.PublicReference, "guess" + i);
        // Even the RIGHT credential is refused once the attempt budget is gone — a lockout that the
        // real holder can escape would be no lockout at all.
        Assert.Null(CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential));
    }

    [Fact]
    public void AnExpiredCredentialFailsExactlyLikeAWrongOne()
    {
        // No oracle: a guess must not be able to distinguish "expired" from "incorrect".
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        var ticket = CommunityCases.IssueAppeal(db, caseId);
        db.Execute("UPDATE pciworld_appeals SET credential_expires_at=? WHERE case_id=?", TestEnv.Stamp("-1 day"), caseId);
        Assert.Null(CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential));
    }

    [Fact]
    public void TheGuestFacingViewCarriesNoEvidence()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        var ticket = CommunityCases.IssueAppeal(db, caseId);
        var appeal = CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential)!;

        var view = CommunityCases.PublicView(appeal);
        Assert.Equal(new[] { "reference", "status", "outcome", "submitted", "decided_at" }, view.Keys.ToArray());
        Assert.DoesNotContain("credential_sha", view.Keys);
        Assert.DoesNotContain("case_id", view.Keys);
    }

    // ── Overturning adds to history rather than rewriting it ──────────────────────────────────

    [Fact]
    public void AnOverturnedAppealLiftsTheSanctionAndTheRestriction()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        var s = CommunityCases.Issue(db, "risk_key", "rk-5", 1, "restrict", "abuse_severe", 10, caseId, 1440);
        db.Execute(@"INSERT INTO pciworld_risk_restrictions(risk_key,scope,room_id,reason_code,case_id,expires_at)
                     VALUES('rk-5','room',1,'abuse_severe',?,?)", caseId, TestEnv.Stamp("+1 day"));

        var ticket = CommunityCases.IssueAppeal(db, caseId);
        var appeal = CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential)!;
        Assert.True(CommunityCases.SubmitAppeal(db, H.L(appeal["id"]), "I was quoting someone else."));
        Assert.True(CommunityCases.Decide(db, H.L(appeal["id"]), 99, upheld: false, "context accepted"));

        Assert.Empty(CommunityCases.ActiveFor(db, "risk_key", "rk-5"));
        Assert.False(CommunityRooms.IsRestricted(db, "rk-5", 1), "the room restriction must be lifted too");
    }

    [Fact]
    public void AnOverturnAddsANewDecisionRatherThanEditingTheOldOne()
    {
        // The audit property: what we decided, and that we changed our mind, must both survive.
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        db.Execute(@"INSERT INTO pciworld_moderation_decisions(scope,subject_ref,outcome,reason_code,category,confidence_band)
                     VALUES('message','1','eject','abuse_severe','targeted_abuse','high')");
        var originalId = db.Scalar<long>("SELECT id FROM pciworld_moderation_decisions ORDER BY id DESC");

        var ticket = CommunityCases.IssueAppeal(db, caseId);
        var appeal = CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential)!;
        CommunityCases.SubmitAppeal(db, H.L(appeal["id"]), "please reconsider");
        CommunityCases.Decide(db, H.L(appeal["id"]), 99, upheld: false, "overturned on context");

        var original = db.QueryOne("SELECT * FROM pciworld_moderation_decisions WHERE id=?", originalId)!;
        Assert.Equal("eject", H.Str(original["outcome"]));          // untouched
        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_moderation_decisions"));
        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_moderation_decisions WHERE reason_code='appeal_overturned'"));
    }

    [Fact]
    public void AnUpheldAppealLeavesTheSanctionStanding()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        CommunityCases.Issue(db, "risk_key", "rk-6", 1, "restrict", "abuse_severe", 10, caseId, 1440);
        var ticket = CommunityCases.IssueAppeal(db, caseId);
        var appeal = CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential)!;
        CommunityCases.SubmitAppeal(db, H.L(appeal["id"]), "it wasn't me");
        Assert.True(CommunityCases.Decide(db, H.L(appeal["id"]), 99, upheld: true, "violation confirmed"));

        Assert.Single(CommunityCases.ActiveFor(db, "risk_key", "rk-6"));
    }

    [Fact]
    public void AnAppealCannotBeDecidedTwice()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        var ticket = CommunityCases.IssueAppeal(db, caseId);
        var appeal = CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential)!;
        CommunityCases.SubmitAppeal(db, H.L(appeal["id"]), "please reconsider");

        Assert.True(CommunityCases.Decide(db, H.L(appeal["id"]), 99, upheld: false, "first"));
        Assert.False(CommunityCases.Decide(db, H.L(appeal["id"]), 98, upheld: true, "second"));
    }

    [Fact]
    public void AnUnsubmittedAppealCannotBeDecided()
    {
        var db = Fresh();
        var caseId = CommunityCases.OpenOrGet(db, 1, "guest_session", "5", "high", "abuse_severe");
        var ticket = CommunityCases.IssueAppeal(db, caseId);
        var appeal = CommunityCases.ByCredential(db, ticket.PublicReference, ticket.Credential)!;
        Assert.False(CommunityCases.Decide(db, H.L(appeal["id"]), 99, upheld: false, "premature"));
    }

    // ── RBAC separation of duties ─────────────────────────────────────────────────────────────

    [Fact]
    public void AnAppealsReviewerCannotAlsoSanction()
    {
        // Independence (§8.6): whoever hears the appeal must not be who imposed the penalty.
        Assert.True(WorldRbac.Allowed("appeals_reviewer", "community.appeal"));
        Assert.False(WorldRbac.Allowed("appeals_reviewer", "community.sanction"));
    }

    [Fact]
    public void ALiveModeratorCannotMakeAnythingPermanent()
    {
        Assert.True(WorldRbac.Allowed("live_moderator", "community.moderate"));
        Assert.False(WorldRbac.Allowed("live_moderator", "community.sanction"));
        Assert.False(WorldRbac.Allowed("live_moderator", "community.sanction.approve"));
    }

    [Fact]
    public void TrustAndSafetyCannotApproveTheirOwnPermanentSanctions()
    {
        // They may issue; only a safety lead or owner may counter-sign.
        Assert.True(WorldRbac.Allowed("trust_safety", "community.sanction"));
        Assert.False(WorldRbac.Allowed("trust_safety", "community.sanction.approve"));
        Assert.True(WorldRbac.Allowed("safety_lead", "community.sanction.approve"));
    }

    [Fact]
    public void OnlyASafetyLeadOrOwnerReachesRestrictedCases()
    {
        Assert.True(WorldRbac.Allowed("safety_lead", "community.restricted"));
        Assert.True(WorldRbac.Allowed("owner", "community.restricted"));
        foreach (var role in new[] { "live_moderator", "trust_safety", "appeals_reviewer", "viewer", "author" })
            Assert.False(WorldRbac.Allowed(role, "community.restricted"), $"{role} must not reach restricted evidence");
    }

    [Fact]
    public void TheEditorialRolesGainNoCommunityPowers()
    {
        // Approving an article is not a licence to eject a participant.
        foreach (var role in new[] { "author", "reviewer", "publisher" })
        {
            Assert.False(WorldRbac.Allowed(role, "community.moderate"));
            Assert.False(WorldRbac.Allowed(role, "community.sanction"));
        }
    }

    [Fact]
    public void TheExistingEditorialPermissionsAreUnchanged()
    {
        // This file extended a live RBAC table; the editorial half must behave exactly as before.
        Assert.True(WorldRbac.Allowed("author", "author"));
        Assert.True(WorldRbac.Allowed("reviewer", "review"));
        Assert.True(WorldRbac.Allowed("publisher", "publish"));
        Assert.True(WorldRbac.Allowed("owner", "admin"));
        Assert.False(WorldRbac.Allowed("viewer", "author"));
        Assert.False(WorldRbac.Allowed("author", "publish"));
        Assert.False(WorldRbac.Allowed(null, "read"));
    }
}
