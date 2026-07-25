using System.Globalization;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The Marketing queue's retry scheduler (<see cref="MarketingJobDispatcher.ScheduleRetry"/>).
///
/// Every ordinary route into this code needs a live provider call, so it had no offline coverage and shipped
/// written as <c>next_attempt_at=datetime('now', ?)</c> — valid SQLite, and on MySQL a call to a function
/// that does not exist. Because DrainOnce dead-letters any job whose run throws, the effect in production
/// was that the FIRST transient provider failure marked the job permanently failed and it was never retried.
///
/// These tests run on whichever provider TEST_DB_PROVIDER selects, so on the MySQL leg they are the check
/// that the statement is executable at all; on both legs they pin the backoff curve.
/// </summary>
[Collection(DbCollection.Name)]
public class MarketingJobRetryTests
{
    static Db Fresh()
    {
        var db = TestEnv.NewMigratedDb();
        MarketingSchema.Ensure(db);
        return db;
    }

    static long SeedJob(Db db, string key) =>
        db.ExecuteReturningId(
            @"INSERT INTO mkt_jobs(idempotency_key,job_type,platform_code,entity_type,entity_id,status)
              VALUES(?,'linkedin_post_publish','linkedin','post',1,'processing')", key);

    /// <summary>Whole minutes from now until the job's next_attempt_at. Computed here, not in SQL —
    /// julianday() is SQLite-only and would reintroduce the very problem under test.</summary>
    static double MinutesOut(Db db, long id)
    {
        var at = db.Scalar<string>("SELECT next_attempt_at FROM mkt_jobs WHERE id=?", id);
        Assert.False(string.IsNullOrWhiteSpace(at), "next_attempt_at was not written");
        var t = DateTime.Parse(at!, CultureInfo.InvariantCulture,
                               DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
        return (t - DateTime.UtcNow).TotalMinutes;
    }

    [Fact]
    public void FirstFailure_ParksTheJobForAnotherPass_OneMinuteOut()
    {
        var db = Fresh();
        var id = SeedJob(db, "retry-first");

        MarketingJobDispatcher.ScheduleRetry(db, id, attempt: 1, error: "provider_timeout");

        Assert.Equal("retrying", db.Scalar<string>("SELECT status FROM mkt_jobs WHERE id=?", id));
        Assert.Equal(1L, db.Scalar<long>("SELECT attempts FROM mkt_jobs WHERE id=?", id));
        Assert.Equal("provider_timeout", db.Scalar<string>("SELECT last_error FROM mkt_jobs WHERE id=?", id));
        Assert.InRange(MinutesOut(db, id), 0.5, 1.0);   // 2^1 × 30s = 60s
    }

    [Fact]
    public void Backoff_DoublesWithEachAttempt_AndCapsAtOneHour()
    {
        var db = Fresh();
        foreach (var (attempt, lowMin, highMin) in new[]
                 {
                     (2,  1.5,  2.0),    // 120s
                     (3,  3.5,  4.0),    // 240s
                     (6, 31.5, 32.0),    // 1920s
                     (9, 59.5, 60.0),    // 15360s → capped at 3600s
                 })
        {
            var id = SeedJob(db, $"retry-backoff-{attempt}");
            MarketingJobDispatcher.ScheduleRetry(db, id, attempt, "provider_timeout");
            Assert.InRange(MinutesOut(db, id), lowMin, highMin);
        }
    }

    /// <summary>
    /// The point of a retry is that the row comes back. DrainOnce only picks up jobs whose next_attempt_at
    /// has passed, so a scheduled job must be invisible now and visible once its moment arrives — which is
    /// also what proves the column holds a value the DUE-row comparison can actually compare against.
    /// </summary>
    [Fact]
    public void AScheduledJob_IsNotYetDue_ButBecomesDueWhenItsMomentPasses()
    {
        var db = Fresh();
        var id = SeedJob(db, "retry-due");
        MarketingJobDispatcher.ScheduleRetry(db, id, attempt: 4, error: "provider_timeout");

        long Due() => db.Scalar<long>(
            @"SELECT COUNT(*) FROM mkt_jobs
              WHERE id=? AND status IN('queued','retrying')
                AND (next_attempt_at IS NULL OR next_attempt_at<=datetime('now'))", id);

        Assert.Equal(0L, Due());
        db.Execute("UPDATE mkt_jobs SET next_attempt_at=datetime('now','-1 minutes') WHERE id=?", id);
        Assert.Equal(1L, Due());
    }
}
