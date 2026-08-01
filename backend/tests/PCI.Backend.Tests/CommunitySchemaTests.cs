using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World community — the schema invariants that ordering, idempotency and name-uniqueness rest
/// on (Data/CommunitySchema.cs; docs/pciworld/CCP_PHASE1_DESIGN.md §2).
///
/// These assert the CONSTRAINTS rather than the columns, because the constraints are the safety
/// property. Application code can be reviewed for races; a unique index cannot be raced.
///
/// They matter most on MySQL. Db.Translate strips the WHERE clause from a partial unique index
/// (Data/Db.cs:199), so a constraint expressed as `... WHERE status='active'` would quietly become
/// a different constraint there — correct on SQLite, wrong on MySQL, and only visible in
/// production. The schema therefore carries its predicate in a nullable column instead, and these
/// tests run under whichever provider TEST_DB_PROVIDER selects, so the MySQL leg proves the two
/// agree rather than assuming it.
/// </summary>
[Collection(DbCollection.Name)]
public class CommunitySchemaTests
{
    /// <summary>A migrated database with the World schema installed. TestEnv.NewMigratedDb runs the
    /// platform migration only; the World tables are installed by WorldSchema.Ensure, exactly as the
    /// app does at boot — same call, so these tests exercise the real install path.</summary>
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long NewRoom(Db db, string slug) =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_community_rooms(slug,title,state) VALUES(?,?,'open')", slug, "Room " + slug);

    static long NewGuest(Db db, long roomId, string folded, string status = "active") =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_guest_sessions(token_sha,room_id,display_name,display_name_folded,
                  active_name_key,rules_version_accepted,status)
              VALUES(?,?,?,?,?,'v1',?)",
            Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"), roomId, folded, folded,
            status == "active" ? folded : null, status);

    // ── One active display name per room, and released when the session ends ──────────────────

    [Fact]
    public void ActiveDisplayName_IsUniquePerRoom()
    {
        var db = NewWorldDb();
        var room = NewRoom(db, "n-unique");
        NewGuest(db, room, "alihassan");

        // The whole point of PW-US-018: a second active participant cannot take the same name.
        Assert.ThrowsAny<Exception>(() => NewGuest(db, room, "alihassan"));
    }

    [Fact]
    public void TheSameNameIsFreeInADifferentRoom()
    {
        var db = NewWorldDb();
        var a = NewRoom(db, "n-room-a");
        var b = NewRoom(db, "n-room-b");
        NewGuest(db, a, "alihassan");
        NewGuest(db, b, "alihassan");   // scoped per room, not globally
    }

    [Fact]
    public void AnEndedSessionReleasesItsName()
    {
        // The behaviour a `WHERE status='active'` partial index would have delivered on SQLite and
        // silently NOT delivered on MySQL. If this passes on both providers, the portable form works.
        var db = NewWorldDb();
        var room = NewRoom(db, "n-release");
        var first = NewGuest(db, room, "alihassan");

        db.Execute("UPDATE pciworld_guest_sessions SET status='left', active_name_key=NULL WHERE id=?", first);

        NewGuest(db, room, "alihassan");   // must now be available again
        Assert.Equal(2, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_guest_sessions WHERE room_id=? AND display_name_folded=?",
            room, "alihassan"));
    }

    [Fact]
    public void ManyEndedSessionsWithTheSameNameCoexist()
    {
        // Several NULL active_name_key rows must not collide with each other — the property both
        // providers give us, and the reason the predicate lives in the data.
        var db = NewWorldDb();
        var room = NewRoom(db, "n-many-ended");
        for (var i = 0; i < 3; i++)
        {
            var id = NewGuest(db, room, "alihassan");
            db.Execute("UPDATE pciworld_guest_sessions SET status='left', active_name_key=NULL WHERE id=?", id);
        }
        Assert.Equal(3, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_guest_sessions WHERE room_id=? AND active_name_key IS NULL", room));
    }

    // ── Message ordering ──────────────────────────────────────────────────────────────────────

    static long NewMessage(Db db, long roomId, string clientId, long? sequence, string status) =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_community_messages(room_id,client_message_id,body,sequence,status)
              VALUES(?,?,?,?,?)", roomId, clientId, "hello", sequence, status);

    [Fact]
    public void ASequenceIdentifiesExactlyOneMessagePerRoom()
    {
        // Ordered reconnect recovery is only sound if this holds. Two concurrent accepts that both
        // computed sequence 1 must have one FAIL rather than quietly interleave.
        var db = NewWorldDb();
        var room = NewRoom(db, "s-unique");
        NewMessage(db, room, "c1", 1, "allowed");
        Assert.ThrowsAny<Exception>(() => NewMessage(db, room, "c2", 1, "allowed"));
    }

    [Fact]
    public void UnpublishedMessagesTakeNoSequenceSlot()
    {
        // Blocked and pending text is retained for review but never broadcast, so it holds no
        // sequence. Many such rows must coexist — on both providers.
        var db = NewWorldDb();
        var room = NewRoom(db, "s-null");
        NewMessage(db, room, "c1", null, "blocked");
        NewMessage(db, room, "c2", null, "quarantined");
        NewMessage(db, room, "c3", null, "pending");
        Assert.Equal(3, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_messages WHERE room_id=? AND sequence IS NULL", room));
    }

    [Fact]
    public void TheSameSequenceIsFineInADifferentRoom()
    {
        var db = NewWorldDb();
        var a = NewRoom(db, "s-room-a");
        var b = NewRoom(db, "s-room-b");
        NewMessage(db, a, "c1", 1, "allowed");
        NewMessage(db, b, "c1", 1, "allowed");   // sequences are per room
    }

    // ── Idempotency ───────────────────────────────────────────────────────────────────────────

    [Fact]
    public void AClientMessageIdCannotProduceTwoMessages()
    {
        // A double-click, a retried request or a hub reconnect must not duplicate a message.
        var db = NewWorldDb();
        var room = NewRoom(db, "i-dup");
        NewMessage(db, room, "same-client-id", 1, "allowed");
        Assert.ThrowsAny<Exception>(() => NewMessage(db, room, "same-client-id", 2, "allowed"));
    }

    // ── Reports ───────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void OneReporterCannotReportTheSameMessageTwice()
    {
        var db = NewWorldDb();
        var room = NewRoom(db, "r-once");
        var msg = NewMessage(db, room, "c1", 1, "allowed");
        var reporter = NewGuest(db, room, "reporter");

        db.Execute(@"INSERT INTO pciworld_community_reports(room_id,message_id,reporter_session_id,reason_code)
                     VALUES(?,?,?,'abuse')", room, msg, reporter);
        Assert.ThrowsAny<Exception>(() =>
            db.Execute(@"INSERT INTO pciworld_community_reports(room_id,message_id,reporter_session_id,reason_code)
                         VALUES(?,?,?,'abuse')", room, msg, reporter));
    }

    [Fact]
    public void DifferentReportersMayReportTheSameMessage()
    {
        var db = NewWorldDb();
        var room = NewRoom(db, "r-many");
        var msg = NewMessage(db, room, "c1", 1, "allowed");
        var one = NewGuest(db, room, "reporterone");
        var two = NewGuest(db, room, "reportertwo");

        foreach (var r in new[] { one, two })
            db.Execute(@"INSERT INTO pciworld_community_reports(room_id,message_id,reporter_session_id,reason_code)
                         VALUES(?,?,?,'abuse')", room, msg, r);
        Assert.Equal(2, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_reports WHERE message_id=?", msg));
    }

    // ── Install-time properties ───────────────────────────────────────────────────────────────

    [Fact]
    public void TheFeatureShipsDisabled()
    {
        // Installing the tables is not launching the feature. Legal prerequisites (CCP-P1-003) and
        // the moderation provider (CCP-P1-004) are unresolved, so a fresh database must not have
        // guest rooms reachable.
        var db = NewWorldDb();
        Assert.False(PCI.Backend.Core.Settings.Bool(db, "world_community_enabled", false));
    }

    [Fact]
    public void EnsureIsIdempotent()
    {
        // It runs on every boot, so running it twice must be a no-op rather than an error.
        // A single draft "general" room is seeded so enabling the feature has somewhere to land —
        // the second Ensure must not create a second copy.
        var db = NewWorldDb();
        CommunitySchema.Ensure(db);
        CommunitySchema.Ensure(db);
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_rooms"));
        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_rooms WHERE slug='general' AND state='draft'"));
    }

    [Fact]
    public void AnOperatorsChoiceSurvivesAReboot()
    {
        // Re-running the installer must never overwrite the flag: a deploy must not silently turn
        // rooms on, nor silently turn them off under an operator who enabled them.
        var db = NewWorldDb();
        PCI.Backend.Core.Settings.Put(db, "world_community_enabled", "1");
        CommunitySchema.Ensure(db);
        Assert.True(PCI.Backend.Core.Settings.Bool(db, "world_community_enabled", false));
    }
}
