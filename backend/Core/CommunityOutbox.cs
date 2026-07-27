using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World community — the durable broadcast outbox (spec §8.3/§15; CCP_PHASE1_DESIGN §2.3).
///
/// The accept path commits a message, its moderation decision and a broadcast event in ONE
/// transaction, then returns. This drains that event afterwards. Delivery is deliberately outside
/// the transaction (§28.18): a slow or dead broadcast must never roll back a message a participant
/// was already told was accepted.
///
/// WHY THIS CAN BE UNRELIABLE WITHOUT LOSING ANYTHING. MySQL is the source of truth for accepted
/// messages, not the connection. If a delivery fails, is delivered twice, or arrives out of order,
/// a client still converges on one ordered history by replaying <see cref="CommunityRooms.Since"/>
/// from its last acknowledged sequence. That is what makes an at-least-once outbox sufficient here,
/// and why the dispatcher is allowed to be simple.
///
/// Claiming reuses <see cref="WorkerLease"/> unchanged — a single conditional UPDATE that only one
/// worker wins, with lease expiry recovering rows stranded by a crash. Two instances draining the
/// same room cannot broadcast the same event twice from the same row.
/// </summary>
public static class CommunityOutbox
{
    /// <summary>A sink that actually pushes an event to connected clients. SignalR implements this;
    /// tests implement it to observe delivery without a transport. Implementations must not throw —
    /// a sink failure is reported so the row retries, never propagated into the drain loop.</summary>
    public interface IBroadcastSink
    {
        /// <summary>Deliver one event. Return false to have the row retried with backoff.</summary>
        bool Deliver(long roomId, string eventType, string payloadJson);
    }

    /// <summary>The default sink for a deployment with no realtime transport wired up. It reports
    /// success so events drain rather than piling up forever, which is correct: the durable message
    /// is already committed and every client recovers it by sequence on next poll or reconnect. A
    /// missing transport degrades latency, not correctness.</summary>
    public sealed class NullSink : IBroadcastSink
    {
        public bool Deliver(long roomId, string eventType, string payloadJson) => true;
    }

    const int MaxAttempts = 8;

    /// <summary>
    /// Drain up to <paramref name="limit"/> due events. Safe to call concurrently and repeatedly.
    /// Never throws: one poisoned row must not stop the queue for a whole room.
    /// </summary>
    public static int DrainOnce(Db db, IBroadcastSink sink, int limit = 50)
    {
        try { WorkerLease.RecoverExpired(db, "pciworld_community_outbox", "queued"); } catch { }

        var rows = db.Query(
            @"SELECT id FROM pciworld_community_outbox
              WHERE status IN ('queued','processing')
                AND (next_attempt_at IS NULL OR next_attempt_at <= datetime('now'))
                AND (lease_until IS NULL OR lease_until <= datetime('now'))
              ORDER BY id LIMIT ?", Math.Clamp(limit, 1, 500));

        var owner = WorkerLease.NewOwner();
        var delivered = 0;
        foreach (var r in rows)
        {
            var id = H.L(r["id"]);
            if (!WorkerLease.TryClaim(db, "pciworld_community_outbox", id, owner, "'queued','processing'")) continue;

            var row = db.QueryOne("SELECT * FROM pciworld_community_outbox WHERE id=?", id);
            if (row is null) continue;

            var attempt = (int)H.L(row["attempts"]) + 1;
            bool ok;
            string? error = null;
            try
            {
                ok = sink.Deliver(H.L(row["room_id"]), H.Str(row["event_type"]) ?? "", H.Str(row["payload_json"]) ?? "{}");
            }
            catch (Exception e)
            {
                // A sink that throws is a failed delivery, not a failed drain.
                ok = false;
                error = e.GetType().Name;
            }

            if (ok)
            {
                db.Execute(
                    @"UPDATE pciworld_community_outbox
                      SET status='sent', attempts=?, sent_at=datetime('now'), lease_owner=NULL, lease_until=NULL, last_error=NULL
                      WHERE id=?", attempt, id);
                delivered++;
            }
            else if (attempt >= MaxAttempts)
            {
                // Dead letter. The message itself is unaffected and still recoverable by sequence;
                // what is lost is the push, not the content. Surfaced for an operator to replay.
                db.Execute(
                    @"UPDATE pciworld_community_outbox
                      SET status='failed', attempts=?, lease_owner=NULL, lease_until=NULL, last_error=?
                      WHERE id=?", attempt, error ?? "delivery_failed", id);
            }
            else
            {
                // Exponential backoff with a cap. No jitter is added here because the retry time is
                // stored per row and rows are claimed individually, so a thundering herd cannot form
                // from a single failed batch the way it would with a shared in-memory timer.
                //
                // The instant is computed with H.StampInMinutes and bound as a VALUE. Db.Translate's
                // SQLite→MySQL rewrite is textual, so it can only see a modifier written as a
                // literal; one arriving through a concatenation reaches MySQL as a function it does
                // not have, while SQLite accepts it happily. That is what makes this failure mode
                // provider-specific and easy to ship, and it is why the helper exists.
                var delaySeconds = Math.Min(300, Math.Pow(2, attempt));
                db.Execute(
                    @"UPDATE pciworld_community_outbox
                      SET status='queued', attempts=?, lease_owner=NULL, lease_until=NULL, last_error=?,
                          next_attempt_at=?
                      WHERE id=?", attempt, error ?? "delivery_failed", H.StampInMinutes(delaySeconds / 60.0), id);
            }
        }
        return delivered;
    }

    /// <summary>Re-queue a dead-lettered event. Manual, authorised replay (§15) — idempotent by the
    /// same reasoning as the rest of the queue: a duplicate broadcast is harmless because clients
    /// deduplicate by sequence.</summary>
    public static bool Replay(Db db, long id) =>
        db.Execute(
            @"UPDATE pciworld_community_outbox
              SET status='queued', attempts=0, next_attempt_at=NULL, lease_owner=NULL, lease_until=NULL
              WHERE id=? AND status='failed'", id) == 1;

    /// <summary>Queue health for the admin command centre (§12.1): depth, dead letters, and the age
    /// of the oldest undelivered event — the number that actually tells an operator whether the room
    /// is falling behind.</summary>
    public static Dictionary<string, object?> Health(Db db)
    {
        var oldest = db.Scalar<string>(
            @"SELECT MIN(created_at) FROM pciworld_community_outbox WHERE status IN ('queued','processing')");
        return new Dictionary<string, object?>
        {
            ["queued"] = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_outbox WHERE status IN ('queued','processing')"),
            ["failed"] = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_outbox WHERE status='failed'"),
            ["sent"] = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_community_outbox WHERE status='sent'"),
            ["oldest_pending_at"] = oldest,
        };
    }
}
