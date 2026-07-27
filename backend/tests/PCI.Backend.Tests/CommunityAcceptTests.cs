using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World community — the message accept path (Core/CommunityRooms.cs).
///
/// The policy engine decides WHETHER text may publish; this decides HOW that verdict is committed,
/// and a correct verdict written non-atomically still leaks. These tests therefore assert what a
/// SECOND participant can observe, not what the function returned — the return value is the
/// author's view, and the safety property is about everyone else.
/// </summary>
[Collection(DbCollection.Name)]
public class CommunityAcceptTests
{
    static readonly System.Collections.Generic.IReadOnlyList<Rule> Policy = DefaultMatrix();

    static (Db db, System.Collections.Generic.Dictionary<string, object?> room) Setup(string slug)
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        db.Execute("INSERT INTO pciworld_community_rooms(slug,title,state) VALUES(?,?,'open')", slug, "Room");
        return (db, CommunityRooms.BySlug(db, slug)!);
    }

    /// <summary>A moderator that returns exactly what a test asks for, so the accept path can be
    /// driven through every outcome without depending on pattern matching.</summary>
    sealed class Scripted : ITextModerator
    {
        readonly Signal _s;
        public Scripted(Signal s) => _s = s;
        public string ProviderName => "scripted";
        public string ProviderVersion => "1";
        public Signal Classify(string t, string l) => _s;
    }

    sealed class Exploding : ITextModerator
    {
        public string ProviderName => "exploding";
        public string ProviderVersion => "1";
        public Signal Classify(string t, string l) => throw new System.InvalidOperationException("provider down");
    }

    static ITextModerator Clean => new Scripted(Signal.Clean());
    static ITextModerator Hateful => new Scripted(new Signal(Categories.Hate, "high", Band.High, null, true));
    static ITextModerator Uncertain => new Scripted(new Signal(Categories.ProfanityMild, "low", Band.Low, null, true));

    // ── The invariant the whole slice exists for (E2E-014) ────────────────────────────────────

    [Fact]
    public void NoOtherParticipantCanSeeABlockedMessage()
    {
        var (db, room) = Setup("a-blocked");
        var r = CommunityRooms.Accept(db, room, null, null, "c1", "a hateful thing", Hateful, Policy);

        Assert.False(r.Published);
        Assert.Null(r.Sequence);
        // The observable check: a second participant replaying from the start sees nothing.
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    [Fact]
    public void NoOtherParticipantCanSeeAQuarantinedMessage()
    {
        var (db, room) = Setup("a-quarantined");
        var r = CommunityRooms.Accept(db, room, null, null, "c1", "ambiguous wording", Uncertain, Policy);

        Assert.False(r.Published);
        Assert.Equal("quarantined", r.Status);
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    [Fact]
    public void AnAllowedMessageIsPublishedExactlyOnce()
    {
        var (db, room) = Setup("a-allowed");
        var r = CommunityRooms.Accept(db, room, null, null, "c1", "a normal question", Clean, Policy);

        Assert.True(r.Published);
        Assert.Equal(1L, r.Sequence);
        var seen = CommunityRooms.Since(db, H.L(room["id"]), 0);
        Assert.Single(seen);
        Assert.Equal("a normal question", H.Str(seen[0]["body"]));
    }

    // ── Fail-closed at the accept layer ───────────────────────────────────────────────────────

    [Fact]
    public void AModeratorThatThrowsPublishesNothingAndDoesNotLoseTheMessage()
    {
        // An exception must not escape as a 500 (the author would lose their message) and must not
        // publish. It has to land as a retained, unpublished row.
        var (db, room) = Setup("a-explode");
        var r = CommunityRooms.Accept(db, room, null, null, "c1", "hello", new Exploding(), Policy);

        Assert.False(r.Published);
        Assert.Equal("moderation_unavailable", r.ReasonCode);
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_messages WHERE id=?", r.MessageId));
    }

    [Fact]
    public void TheDefaultNoProviderConfigurationPublishesNothing()
    {
        var (db, room) = Setup("a-null");
        var r = CommunityRooms.Accept(db, room, null, null, "c1", "hello",
                                      new CommunityModerators.NullModerator(), Policy);
        Assert.False(r.Published);
        Assert.Empty(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    [Fact]
    public void AClosedRoomAcceptsNothing()
    {
        var (db, room) = Setup("a-closed");
        db.Execute("UPDATE pciworld_community_rooms SET state='read_only' WHERE id=?", H.L(room["id"]));
        var reloaded = CommunityRooms.BySlug(db, "a-closed")!;
        var r = CommunityRooms.Accept(db, reloaded, null, null, "c1", "hello", Clean, Policy);
        Assert.Equal("room_not_accepting", r.ReasonCode);
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_messages"));
    }

    [Fact]
    public void AnEmergencyLockBeatsAnOpenSchedule()
    {
        // A kill switch must win over the room's own state, or an incident cannot be stopped.
        var (db, room) = Setup("a-locked");
        db.Execute("UPDATE pciworld_community_rooms SET emergency_state='locked' WHERE id=?", H.L(room["id"]));
        var reloaded = CommunityRooms.BySlug(db, "a-locked")!;
        Assert.False(CommunityRooms.AcceptsMessages(reloaded));
        var r = CommunityRooms.Accept(db, reloaded, null, null, "c1", "hello", Clean, Policy);
        Assert.Equal("room_not_accepting", r.ReasonCode);
    }

    // ── Ordering and idempotency (E2E-028) ────────────────────────────────────────────────────

    [Fact]
    public void SequencesAreUniqueAndContiguousAcrossManyAccepts()
    {
        var (db, room) = Setup("a-seq");
        for (var i = 0; i < 25; i++)
            CommunityRooms.Accept(db, room, null, null, "c" + i, "message " + i, Clean, Policy);

        var seen = CommunityRooms.Since(db, H.L(room["id"]), 0);
        Assert.Equal(25, seen.Count);
        for (var i = 0; i < 25; i++) Assert.Equal(i + 1L, H.L(seen[i]["sequence"]));
    }

    [Fact]
    public void UnpublishedMessagesConsumeNoSequence()
    {
        // A blocked message between two allowed ones must not leave a hole, or a reconnecting client
        // waits forever for a sequence that will never arrive.
        var (db, room) = Setup("a-nohole");
        CommunityRooms.Accept(db, room, null, null, "c1", "first", Clean, Policy);
        CommunityRooms.Accept(db, room, null, null, "c2", "hateful", Hateful, Policy);
        CommunityRooms.Accept(db, room, null, null, "c3", "second", Clean, Policy);

        var seen = CommunityRooms.Since(db, H.L(room["id"]), 0);
        Assert.Equal(2, seen.Count);
        Assert.Equal(1L, H.L(seen[0]["sequence"]));
        Assert.Equal(2L, H.L(seen[1]["sequence"]));
    }

    [Fact]
    public void ARetriedRequestProducesOneMessageNotTwo()
    {
        var (db, room) = Setup("a-retry");
        var first = CommunityRooms.Accept(db, room, null, null, "same-id", "hello", Clean, Policy);
        var retry = CommunityRooms.Accept(db, room, null, null, "same-id", "hello", Clean, Policy);

        Assert.Equal(first.MessageId, retry.MessageId);
        Assert.Equal(first.Sequence, retry.Sequence);
        Assert.Equal("duplicate", retry.ReasonCode);
        Assert.Single(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    [Fact]
    public void ReconnectReplaysOnlyWhatTheClientHasNotSeen()
    {
        var (db, room) = Setup("a-reconnect");
        for (var i = 0; i < 5; i++)
            CommunityRooms.Accept(db, room, null, null, "c" + i, "m" + i, Clean, Policy);

        var tail = CommunityRooms.Since(db, H.L(room["id"]), 3);
        Assert.Equal(2, tail.Count);
        Assert.Equal(4L, H.L(tail[0]["sequence"]));
        Assert.Equal(5L, H.L(tail[1]["sequence"]));
    }

    // ── The outbox is written with the message, delivered after ───────────────────────────────

    [Fact]
    public void OnlyAPublishedMessageProducesABroadcastEvent()
    {
        var (db, room) = Setup("a-outbox");
        CommunityRooms.Accept(db, room, null, null, "c1", "fine", Clean, Policy);
        CommunityRooms.Accept(db, room, null, null, "c2", "hateful", Hateful, Policy);

        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_outbox WHERE event_type='CommunityMessageAllowed'"));
        Assert.Equal("queued", H.Str(db.QueryOne("SELECT status FROM pciworld_community_outbox")!["status"]));
    }

    // ── Evidence handling ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void ADecisionRecordsAHashRatherThanTheContent()
    {
        // Blocked text must not be copied into the decision trail (§8.7).
        var (db, room) = Setup("a-eviden");
        const string body = "a hateful thing that must not be copied";
        CommunityRooms.Accept(db, room, null, null, "c1", body, Hateful, Policy);

        var d = db.QueryOne("SELECT * FROM pciworld_moderation_decisions WHERE scope='message'")!;
        Assert.Equal(Security.Sha(body), H.Str(d["content_hash"]));
        Assert.Equal("hate", H.Str(d["category"]));
        Assert.Equal("escalate", H.Str(d["outcome"]));
        Assert.False(string.IsNullOrWhiteSpace(H.Str(d["reason_code"])));
    }

    [Fact]
    public void EveryMessageIsLinkedToItsDecision()
    {
        // There must be no window, and no row, where a message exists without a verdict.
        var (db, room) = Setup("a-linked");
        foreach (var (id, mod) in new (string, ITextModerator)[] { ("c1", Clean), ("c2", Hateful), ("c3", Uncertain) })
            CommunityRooms.Accept(db, room, null, null, id, "text", mod, Policy);

        Assert.Equal(0, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_messages WHERE decision_id IS NULL"));
    }

    // ── Guest sessions ────────────────────────────────────────────────────────────────────────

    [Fact]
    public void AGuestJoinsAndReceivesAnOpaqueToken()
    {
        var (db, room) = Setup("g-join");
        var r = CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", null);

        Assert.True(r.Ok);
        Assert.False(string.IsNullOrWhiteSpace(r.Token));
        // Stored hashed, never in the clear.
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_guest_sessions WHERE token_sha=?", r.Token!));
        Assert.NotNull(CommunityRooms.SessionByToken(db, r.Token));
    }

    [Fact]
    public void AnUnsafeDisplayNameIsRefusedWithItsReason()
    {
        var (db, room) = Setup("g-unsafe");
        Assert.Equal("name_reserved", CommunityRooms.Join(db, H.L(room["id"]), "admin", "v1", null).ErrorCode);
        Assert.Equal("name_impersonation", CommunityRooms.Join(db, H.L(room["id"]), "PCI Support Team", "v1", null).ErrorCode);
        Assert.Equal("name_contact_details", CommunityRooms.Join(db, H.L(room["id"]), "me@example.com", "v1", null).ErrorCode);
    }

    [Fact]
    public void TwoActiveGuestsCannotShareADisplayName()
    {
        var (db, room) = Setup("g-dup");
        Assert.True(CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", null).Ok);

        var second = CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", null);
        Assert.False(second.Ok);
        Assert.Equal("name_taken", second.ErrorCode);
        Assert.False(string.IsNullOrWhiteSpace(second.Suggestion));
    }

    [Fact]
    public void LeavingReleasesTheNameForSomeoneElse()
    {
        var (db, room) = Setup("g-release");
        var first = CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", null);
        CommunityRooms.EndSession(db, first.SessionId, "left", "user_left");

        Assert.True(CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", null).Ok);
    }

    [Fact]
    public void AnEndedSessionsTokenStopsWorking()
    {
        var (db, room) = Setup("g-revoke");
        var r = CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", null);
        CommunityRooms.EndSession(db, r.SessionId, "ejected", "abuse_severe");
        Assert.Null(CommunityRooms.SessionByToken(db, r.Token));
    }

    [Fact]
    public void ARestrictedRiskKeyCannotRejoin()
    {
        var (db, room) = Setup("g-restrict");
        db.Execute(@"INSERT INTO pciworld_risk_restrictions(risk_key,scope,room_id,reason_code,expires_at)
                     VALUES('rk-1','room',?,'abuse_severe',datetime('now','+1 day'))", H.L(room["id"]));

        Assert.Equal("access_restricted", CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", "rk-1").ErrorCode);
        Assert.True(CommunityRooms.Join(db, H.L(room["id"]), "Sara Khan", "v1", "rk-2").Ok);
    }

    [Fact]
    public void AnExpiredRestrictionNoLongerBlocks()
    {
        // A time-bounded sanction must actually end, or every temporary restriction is permanent.
        var (db, room) = Setup("g-expired");
        db.Execute(@"INSERT INTO pciworld_risk_restrictions(risk_key,scope,room_id,reason_code,expires_at)
                     VALUES('rk-old','room',?,'abuse_severe',datetime('now','-1 day'))", H.L(room["id"]));
        Assert.True(CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", "rk-old").Ok);
    }

    // ── Presence and discovery ────────────────────────────────────────────────────────────────

    [Fact]
    public void PresenceCountsActiveParticipantsOnly()
    {
        var (db, room) = Setup("g-presence");
        var a = CommunityRooms.Join(db, H.L(room["id"]), "Ali Hassan", "v1", null);
        CommunityRooms.Join(db, H.L(room["id"]), "Sara Khan", "v1", null);
        Assert.Equal(2, CommunityRooms.Presence(db, H.L(room["id"])));

        CommunityRooms.EndSession(db, a.SessionId, "left", "user_left");
        Assert.Equal(1, CommunityRooms.Presence(db, H.L(room["id"])));
    }

    [Fact]
    public void DraftAndArchivedRoomsAreNotListed()
    {
        var (db, _) = Setup("d-open");
        db.Execute("INSERT INTO pciworld_community_rooms(slug,title,state) VALUES('d-draft','Draft','draft')");
        db.Execute("INSERT INTO pciworld_community_rooms(slug,title,state) VALUES('d-arch','Archived','archived')");

        var slugs = CommunityRooms.Discoverable(db).Select(r => H.Str(r["slug"])).ToList();
        Assert.Contains("d-open", slugs);
        Assert.DoesNotContain("d-draft", slugs);
        Assert.DoesNotContain("d-arch", slugs);
    }

    [Fact]
    public void TheFeatureIsOffUntilAnOperatorEnablesIt()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        Assert.False(CommunityRooms.Enabled(db));
    }
}
