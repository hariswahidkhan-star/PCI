using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World community — the durable broadcast outbox (Core/CommunityOutbox.cs).
///
/// The property under test is not "delivery works". It is that delivery is allowed to fail, retry,
/// duplicate or never happen at all WITHOUT losing an accepted message, because MySQL and the room
/// sequence are the source of truth rather than the connection. Several tests below deliberately
/// break delivery and then assert the message is still recoverable.
/// </summary>
[Collection(DbCollection.Name)]
public class CommunityOutboxTests
{
    static readonly System.Collections.Generic.IReadOnlyList<Rule> Policy = DefaultMatrix();

    sealed class Clean : ITextModerator
    {
        public string ProviderName => "clean";
        public string ProviderVersion => "1";
        public Signal Classify(string t, string l) => Signal.Clean();
    }

    /// <summary>A sink that records what it was asked to deliver and can be told to fail or throw.</summary>
    sealed class RecordingSink : CommunityOutbox.IBroadcastSink
    {
        public readonly System.Collections.Generic.List<(long RoomId, string Type, string Payload)> Delivered = new();
        public bool Fail;
        public bool Throw;
        public bool Deliver(long roomId, string eventType, string payloadJson)
        {
            if (Throw) throw new System.InvalidOperationException("transport down");
            if (Fail) return false;
            Delivered.Add((roomId, eventType, payloadJson));
            return true;
        }
    }

    static (Db db, System.Collections.Generic.Dictionary<string, object?> room) Setup(string slug)
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        db.Execute("INSERT INTO pciworld_community_rooms(slug,title,state) VALUES(?,?,'open')", slug, "Room");
        return (db, CommunityRooms.BySlug(db, slug)!);
    }

    static void Publish(Db db, System.Collections.Generic.Dictionary<string, object?> room, string clientId, string body)
        => CommunityRooms.Accept(db, room, null, null, clientId, body, new Clean(), Policy);

    // ── Normal delivery ───────────────────────────────────────────────────────────────────────

    [Fact]
    public void AnAcceptedMessageIsBroadcastOnceAndMarkedSent()
    {
        var (db, room) = Setup("o-basic");
        Publish(db, room, "c1", "hello");

        var sink = new RecordingSink();
        Assert.Equal(1, CommunityOutbox.DrainOnce(db, sink));
        Assert.Single(sink.Delivered);
        Assert.Equal("CommunityMessageAllowed", sink.Delivered[0].Type);
        Assert.Equal(H.L(room["id"]), sink.Delivered[0].RoomId);
        Assert.Equal("sent", H.Str(db.QueryOne("SELECT status FROM pciworld_community_outbox")!["status"]));
    }

    [Fact]
    public void ASentEventIsNotDeliveredAgainOnTheNextDrain()
    {
        var (db, room) = Setup("o-once");
        Publish(db, room, "c1", "hello");

        var sink = new RecordingSink();
        CommunityOutbox.DrainOnce(db, sink);
        Assert.Equal(0, CommunityOutbox.DrainOnce(db, sink));
        Assert.Single(sink.Delivered);
    }

    [Fact]
    public void OnlyPublishedMessagesEverEnterTheQueue()
    {
        var (db, room) = Setup("o-blocked");
        CommunityRooms.Accept(db, room, null, null, "c1", "bad",
            new ScriptedHate(), Policy);
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_outbox"));
    }

    sealed class ScriptedHate : ITextModerator
    {
        public string ProviderName => "s";
        public string ProviderVersion => "1";
        public Signal Classify(string t, string l) => new(Categories.Hate, "high", Band.High, null, true);
    }

    // ── Failure does not lose the message ─────────────────────────────────────────────────────

    [Fact]
    public void AMessageStaysRecoverableEvenWhenBroadcastNeverSucceeds()
    {
        // The property that makes an at-least-once outbox sufficient. Break delivery entirely and
        // the participant can still replay the message from the durable sequence.
        var (db, room) = Setup("o-durable");
        Publish(db, room, "c1", "hello");

        var sink = new RecordingSink { Fail = true };
        for (var i = 0; i < 10; i++)
        {
            db.Execute("UPDATE pciworld_community_outbox SET next_attempt_at=NULL");
            CommunityOutbox.DrainOnce(db, sink);
        }

        Assert.Empty(sink.Delivered);
        Assert.Equal("failed", H.Str(db.QueryOne("SELECT status FROM pciworld_community_outbox")!["status"]));
        // …and yet:
        var history = CommunityRooms.Since(db, H.L(room["id"]), 0);
        Assert.Single(history);
        Assert.Equal("hello", H.Str(history[0]["body"]));
    }

    [Fact]
    public void AFailedDeliveryIsRetriedWithBackoffRatherThanDropped()
    {
        var (db, room) = Setup("o-retry");
        Publish(db, room, "c1", "hello");

        var sink = new RecordingSink { Fail = true };
        CommunityOutbox.DrainOnce(db, sink);

        var row = db.QueryOne("SELECT * FROM pciworld_community_outbox")!;
        Assert.Equal("queued", H.Str(row["status"]));
        Assert.Equal(1, H.L(row["attempts"]));
        Assert.False(string.IsNullOrWhiteSpace(H.Str(row["next_attempt_at"])));
    }

    [Fact]
    public void ABackedOffEventIsNotRetriedBeforeItsTime()
    {
        var (db, room) = Setup("o-backoff");
        Publish(db, room, "c1", "hello");

        var sink = new RecordingSink { Fail = true };
        CommunityOutbox.DrainOnce(db, sink);
        // Immediately draining again must skip it — otherwise backoff is decorative and a dead
        // transport gets hammered.
        sink.Fail = false;
        Assert.Equal(0, CommunityOutbox.DrainOnce(db, sink));
        Assert.Empty(sink.Delivered);
    }

    [Fact]
    public void AnEventIsDeadLetteredAfterRepeatedFailureRatherThanRetriedForever()
    {
        var (db, room) = Setup("o-dead");
        Publish(db, room, "c1", "hello");

        var sink = new RecordingSink { Fail = true };
        for (var i = 0; i < 10; i++)
        {
            db.Execute("UPDATE pciworld_community_outbox SET next_attempt_at=NULL");
            CommunityOutbox.DrainOnce(db, sink);
        }
        var row = db.QueryOne("SELECT * FROM pciworld_community_outbox")!;
        Assert.Equal("failed", H.Str(row["status"]));
        Assert.False(string.IsNullOrWhiteSpace(H.Str(row["last_error"])));
    }

    [Fact]
    public void ASinkThatThrowsDoesNotStopTheQueue()
    {
        // One poisoned row must not block a whole room's broadcasts.
        var (db, room) = Setup("o-throw");
        Publish(db, room, "c1", "one");
        Publish(db, room, "c2", "two");

        var throwing = new RecordingSink { Throw = true };
        CommunityOutbox.DrainOnce(db, throwing);   // must not propagate

        var good = new RecordingSink();
        db.Execute("UPDATE pciworld_community_outbox SET next_attempt_at=NULL");
        Assert.Equal(2, CommunityOutbox.DrainOnce(db, good));
    }

    // ── Operator recovery ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void AnOperatorCanReplayADeadLetteredEvent()
    {
        var (db, room) = Setup("o-replay");
        Publish(db, room, "c1", "hello");
        db.Execute("UPDATE pciworld_community_outbox SET status='failed', attempts=8");

        var id = H.L(db.QueryOne("SELECT id FROM pciworld_community_outbox")!["id"]);
        Assert.True(CommunityOutbox.Replay(db, id));

        var sink = new RecordingSink();
        Assert.Equal(1, CommunityOutbox.DrainOnce(db, sink));
        Assert.Single(sink.Delivered);
    }

    [Fact]
    public void ReplayingSomethingThatIsNotDeadLetteredDoesNothing()
    {
        var (db, room) = Setup("o-replay-noop");
        Publish(db, room, "c1", "hello");
        var id = H.L(db.QueryOne("SELECT id FROM pciworld_community_outbox")!["id"]);
        Assert.False(CommunityOutbox.Replay(db, id));   // still queued, not failed
    }

    // ── Health ────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void HealthReportsDepthDeadLettersAndTheOldestPendingEvent()
    {
        var (db, room) = Setup("o-health");
        Publish(db, room, "c1", "one");
        Publish(db, room, "c2", "two");

        var before = CommunityOutbox.Health(db);
        Assert.Equal(2L, before["queued"]);
        Assert.Equal(0L, before["failed"]);
        Assert.NotNull(before["oldest_pending_at"]);

        CommunityOutbox.DrainOnce(db, new RecordingSink());
        var after = CommunityOutbox.Health(db);
        Assert.Equal(0L, after["queued"]);
        Assert.Equal(2L, after["sent"]);
        Assert.Null(after["oldest_pending_at"]);
    }

    // ── The default sink ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void WithNoTransportWiredUpEventsStillDrainRatherThanAccumulate()
    {
        // A deployment without realtime must not grow an unbounded queue. The message is already
        // durable, so draining to the null sink loses latency, not content.
        var (db, room) = Setup("o-null");
        Publish(db, room, "c1", "hello");

        Assert.Equal(1, CommunityOutbox.DrainOnce(db, new CommunityOutbox.NullSink()));
        Assert.Equal(0L, CommunityOutbox.Health(db)["queued"]);
        Assert.Single(CommunityRooms.Since(db, H.L(room["id"]), 0));
    }

    [Fact]
    public void DrainingAnEmptyQueueIsHarmless()
    {
        var (db, _) = Setup("o-empty");
        Assert.Equal(0, CommunityOutbox.DrainOnce(db, new RecordingSink()));
    }

    // ── Payload ───────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void TheBroadcastPayloadCarriesTheSequenceClientsDeduplicateOn()
    {
        var (db, room) = Setup("o-payload");
        Publish(db, room, "c1", "hello");

        var sink = new RecordingSink();
        CommunityOutbox.DrainOnce(db, sink);

        var payload = sink.Delivered[0].Payload;
        Assert.Contains("\"sequence\":1", payload);
        Assert.Contains("\"clientMessageId\":\"c1\"", payload);
        // The body is NOT in the broadcast envelope — clients read it from the durable history, so
        // an event sitting in a queue or a log never carries message content.
        Assert.DoesNotContain("hello", payload);
    }
}
