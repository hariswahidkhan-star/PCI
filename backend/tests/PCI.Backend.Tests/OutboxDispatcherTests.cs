using System.Globalization;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-15 — the Communications outbox drain (<see cref="OutboxDispatcher"/>): the code that claims due
/// rows, sends each through the per-channel provider seam, and — on failure — retries with exponential
/// backoff up to max_attempts before giving up. The background loop is untested here; the decisions are
/// pinned directly against the migrated temp-SQLite Db via the public <c>DrainOnce(db, limit)</c> entry
/// point an admin retry also calls. No network is touched: the failure path is driven by the in-app
/// channel with no recipient (a deterministic 'failed' result), and the success path by the console/in-app
/// no-op sinks. Assertions read comm_outbox state (status / attempts / next_attempt_at) after a pass.
/// No application code is changed.
/// </summary>
public class OutboxDispatcherTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static long SeedUser(Db db, string email = "outbox@test.io")
    {
        // FK enforcement is ON (PRAGMA foreign_keys=ON); a real recipient keeps in-app delivery honest.
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)", email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    /// <summary>Seed one outbox row. <paramref name="scheduledMod"/>/<paramref name="nextAttemptMod"/> are
    /// SQLite datetime modifiers (e.g. "+30 minutes", "-5 minutes"), resolved by <see cref="TestEnv.Stamp"/> so
    /// the statement runs on MySQL too; null leaves the column NULL (= due now).</summary>
    static long SeedOutbox(Db db, string channel, string status,
        long? userId = null, string? toEmail = null,
        long attempts = 0, long maxAttempts = 5,
        string? scheduledMod = null, string? nextAttemptMod = null)
    {
        var id = db.ExecuteReturningId(
            @"INSERT INTO comm_outbox(channel,status,user_id,to_email,subject,body,attempts,max_attempts)
              VALUES(?,?,?,?,?,?,?,?)",
            channel, status, userId, toEmail, "Subject", "Body", attempts, maxAttempts);
        if (scheduledMod is not null)
            db.Execute("UPDATE comm_outbox SET scheduled_at=? WHERE id=?", TestEnv.Stamp(scheduledMod), id);
        if (nextAttemptMod is not null)
            db.Execute("UPDATE comm_outbox SET next_attempt_at=? WHERE id=?", TestEnv.Stamp(nextAttemptMod), id);
        return id;
    }

    static Dictionary<string, object?> Row(Db db, long id) => db.QueryOne("SELECT * FROM comm_outbox WHERE id=?", id)!;
    static string? Status(Db db, long id) => db.Scalar<string>("SELECT status FROM comm_outbox WHERE id=?", id);
    static long Attempts(Db db, long id) => db.Scalar<long>("SELECT attempts FROM comm_outbox WHERE id=?", id);

    /// <summary>Whole minutes from now until next_attempt_at (rounded). The dispatcher stamps the column
    /// moments earlier, so the tiny elapsed fraction rounds cleanly back to N. The arithmetic is done here
    /// rather than in SQL because julianday() is SQLite-only.</summary>
    static long BackoffMinutes(Db db, long id)
    {
        var at = db.Scalar<string>("SELECT next_attempt_at FROM comm_outbox WHERE id=?", id);
        var t = DateTime.Parse(at!, CultureInfo.InvariantCulture,
                               DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
        return (long)Math.Round((t - DateTime.UtcNow).TotalMinutes);
    }

    // ---- Retry backoff progression ---------------------------------------------------------------

    [Fact]
    public void FirstFailure_SchedulesRetry_WithTwoMinuteBackoff()
    {
        var db = Fresh();
        // in-app with no recipient → the provider seam returns 'failed' (no network), driving the retry path.
        var id = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 0, maxAttempts: 5);
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("retrying", Status(db, id));
        Assert.Equal(1, Attempts(db, id));
        Assert.Equal(2, BackoffMinutes(db, id));   // min(60, 2^1)
    }

    [Fact]
    public void Retry_RecordsADeliveryAttemptRow()
    {
        var db = Fresh();
        var id = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 0, maxAttempts: 5);
        OutboxDispatcher.DrainOnce(db, 25);
        // Every send outcome (success or failure) is journaled as a comm_delivery_attempts row.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM comm_delivery_attempts WHERE outbox_id=? AND status='failed'", id));
    }

    [Fact]
    public void Backoff_GrowsAcrossSuccessiveDuePasses()
    {
        var db = Fresh();
        // max_attempts high so it never terminates while we watch the backoff double: 2 → 4 → 8 minutes.
        var id = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 0, maxAttempts: 20);

        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal(1, Attempts(db, id));
        Assert.Equal(2, BackoffMinutes(db, id));

        db.Execute("UPDATE comm_outbox SET next_attempt_at=datetime('now','-1 minutes') WHERE id=?", id);   // make it due again
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal(2, Attempts(db, id));
        Assert.Equal(4, BackoffMinutes(db, id));

        db.Execute("UPDATE comm_outbox SET next_attempt_at=datetime('now','-1 minutes') WHERE id=?", id);
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal(3, Attempts(db, id));
        Assert.Equal(8, BackoffMinutes(db, id));
    }

    [Fact]
    public void Backoff_CapsAtSixtyMinutes()
    {
        var db = Fresh();
        // One pass over three rows already deep into their retry count (max_attempts high so none terminate).
        var a = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 4, maxAttempts: 20);   // → attempt 5 → 32 min
        var b = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 5, maxAttempts: 20);   // → attempt 6 → 60 (cap)
        var c = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 6, maxAttempts: 20);   // → attempt 7 → 60 (cap)
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal(32, BackoffMinutes(db, a));
        Assert.Equal(60, BackoffMinutes(db, b));
        Assert.Equal(60, BackoffMinutes(db, c));
        Assert.Equal("retrying", Status(db, c));
    }

    [Fact]
    public void BelowMaxAttempts_StillRetries()
    {
        var db = Fresh();
        var id = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 3, maxAttempts: 5);   // attempt 4 of 5
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("retrying", Status(db, id));
        Assert.Equal(4, Attempts(db, id));
        Assert.Equal(16, BackoffMinutes(db, id));   // min(60, 2^4)
    }

    // ---- Terminal failure at the attempt ceiling -------------------------------------------------

    [Fact]
    public void AtMaxAttempts_MarkedFailed_WithNoFurtherRetryScheduled()
    {
        var db = Fresh();
        var id = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 4, maxAttempts: 5);   // attempt 5 == max
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("failed", Status(db, id));
        Assert.Equal(5, Attempts(db, id));
        // Terminal: no backoff is scheduled — the row will never be re-selected.
        Assert.Null(Row(db, id)["next_attempt_at"]);
    }

    [Fact]
    public void TerminalFailed_IsNotRetriedOnALaterPass()
    {
        var db = Fresh();
        var id = SeedOutbox(db, "inapp", "queued", userId: null, attempts: 4, maxAttempts: 5);
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("failed", Status(db, id));
        Assert.Equal(5, Attempts(db, id));
        // A second drain must not touch a terminal row (status 'failed' is outside the claim set).
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("failed", Status(db, id));
        Assert.Equal(5, Attempts(db, id));
    }

    // ---- Scheduling: not-due rows are skipped ----------------------------------------------------

    [Fact]
    public void FutureNextAttemptAt_IsSkipped()
    {
        var db = Fresh();
        // A retrying row whose backoff has not elapsed yet must be left exactly as-is this pass.
        var id = SeedOutbox(db, "inapp", "retrying", userId: null, attempts: 1, maxAttempts: 5, nextAttemptMod: "+30 minutes");
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("retrying", Status(db, id));   // not advanced to 'processing'/'failed'
        Assert.Equal(1, Attempts(db, id));          // no extra attempt consumed
        Assert.Equal(30, BackoffMinutes(db, id));   // schedule untouched
    }

    [Fact]
    public void FutureScheduledAt_IsSkipped()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var id = SeedOutbox(db, "inapp", "queued", userId: uid, attempts: 0, maxAttempts: 5, scheduledMod: "+2 hours");
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("queued", Status(db, id));     // held back
        Assert.Equal(0, Attempts(db, id));
        // Nothing was delivered, so no in-app notification was written for the recipient.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM notifications WHERE user_id=?", uid));
    }

    [Fact]
    public void PastScheduledAt_IsProcessed()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A 'scheduled' row whose time has arrived is due and gets delivered this pass.
        var id = SeedOutbox(db, "inapp", "scheduled", userId: uid, attempts: 0, maxAttempts: 5, scheduledMod: "-5 minutes");
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("sent", Status(db, id));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM notifications WHERE user_id=?", uid));
    }

    // ---- Robustness: one bad row never stops the batch -------------------------------------------

    [SqliteOnlyFact("The corrupt row this needs cannot exist on MySQL: an INTEGER column there refuses "
        + "non-numeric text, so the classify-time throw the batch fail-safe catches never happens.")]
    public void MalformedRow_IsMarkedFailed_WithoutStoppingTheBatch()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A corrupt row: non-numeric text lands in the INTEGER-affinity 'attempts' column (SQLite stores it
        // as text), so the dispatcher's Convert.ToInt64 throws while classifying it — the batch's fail-safe.
        var bad = db.ExecuteReturningId(
            @"INSERT INTO comm_outbox(channel,status,subject,body,attempts,max_attempts)
              VALUES('inapp','queued','S','B',?,5)", "not-a-number");
        // A perfectly good row queued right after it (higher id → processed later in the same pass).
        var good = SeedOutbox(db, "inapp", "queued", userId: uid);

        OutboxDispatcher.DrainOnce(db, 25);

        // The bad row is isolated as failed (with the error captured)...
        Assert.Equal("failed", Status(db, bad));
        Assert.NotNull(Row(db, bad)["last_error"]);
        // ...and the following row still delivered — the batch was not aborted.
        Assert.Equal("sent", Status(db, good));
        Assert.Equal(1, Attempts(db, good));
    }

    // ---- Deterministic success via the no-op sinks -----------------------------------------------

    [Fact]
    public void InAppWithRecipient_IsDeliveredAndMarkedSent()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var id = SeedOutbox(db, "inapp", "queued", userId: uid, attempts: 0, maxAttempts: 5);
        OutboxDispatcher.DrainOnce(db, 25);
        var r = Row(db, id);
        Assert.Equal("sent", Status(db, id));
        Assert.Equal(1, Attempts(db, id));
        Assert.Equal("inapp", H.Str(r["provider"]));
        Assert.NotNull(r["sent_at"]);
        Assert.Null(r["last_error"]);
        // In-app delivery writes the recipient's notification row.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM notifications WHERE user_id=?", uid));
    }

    [Fact]
    public void EmailWithNoProviderConfigured_UsesConsoleSink_AndIsMarkedSent()
    {
        var db = Fresh();
        // The hermetic test env sets neither RESEND_API_KEY nor SMTP_HOST, so the email seam falls through
        // to its console sink ('console' counts as delivered-for-history) — a deterministic, no-network send.
        var id = SeedOutbox(db, "email", "queued", toEmail: "someone@test.io", attempts: 0, maxAttempts: 5);
        OutboxDispatcher.DrainOnce(db, 25);
        Assert.Equal("sent", Status(db, id));
        Assert.Equal(1, Attempts(db, id));
        Assert.NotNull(Row(db, id)["sent_at"]);
    }
}
