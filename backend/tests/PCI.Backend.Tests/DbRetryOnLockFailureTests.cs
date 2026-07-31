using Microsoft.Data.Sqlite;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Db.WithRetryOnLockFailure — the retry that makes concurrent schema installation survive an
/// InnoDB deadlock.
///
/// WHY THIS EXISTS SEPARATELY FROM THE CONCURRENCY SUITE. A deadlock is timing-dependent: six
/// concurrent Ensure() calls produced one on CI's MariaDB and would not reproduce locally across
/// fourteen attempts, fresh database each time. A test that can only pass by winning a race is not a
/// regression guard — it is a coin toss that happens to be green. So the retry's CONTRACT is pinned
/// here with a synthetic failure, and the concurrency suite covers the integration.
///
/// The contract has two halves and both matter. Retrying something the database called retryable is
/// correct; retrying anything else is how a genuine defect becomes an intermittent one that takes
/// five times as long to fail.
/// </summary>
[Collection(DbCollection.Name)]
public class DbRetryOnLockFailureTests
{
    static Db NewDb() => TestEnv.NewMigratedDb();

    /// <summary>A real lock failure, constructed rather than forged.
    ///
    /// The predicate reads the provider's error NUMBER, and SqliteException is the one of the two
    /// provider exceptions with a public constructor that takes one — MySqlException has to be built
    /// by the connector from a server packet. Both providers go through the same predicate and the
    /// same loop, so exercising the SQLite numbers exercises the branch that matters. SQLITE_BUSY (5)
    /// and SQLITE_LOCKED (6) are the same contract as InnoDB's 1213/1205: the work was refused, not
    /// rejected, and the caller is expected to try again.</summary>
    static SqliteException LockException(int sqliteErrorCode) =>
        new("database is locked", sqliteErrorCode);

    [Fact]
    public void ARetryableLockFailureIsRetriedAndTheWorkEventuallySucceeds()
    {
        var db = NewDb();
        var attempts = 0;
        db.WithRetryOnLockFailure(() =>
        {
            attempts++;
            if (attempts < 3) throw LockException(5);   // SQLITE_BUSY — same contract as InnoDB's 1213
        });
        Assert.Equal(3, attempts);
    }

    [Fact]
    public void ALockWaitTimeoutIsRetriedToo()
    {
        var db = NewDb();
        var attempts = 0;
        db.WithRetryOnLockFailure(() =>
        {
            attempts++;
            if (attempts < 2) throw LockException(6);   // SQLITE_LOCKED — same contract as InnoDB's 1205
        });
        Assert.Equal(2, attempts);
    }

    [Fact]
    public void AnythingElseIsNotRetried()
    {
        // The failure that must NOT be swallowed into a slow loop: a duplicate key, a syntax error, a
        // missing column. Retrying those hides a real defect behind four extra attempts.
        var db = NewDb();
        var attempts = 0;
        Assert.Throws<InvalidOperationException>(() => db.WithRetryOnLockFailure(() =>
        {
            attempts++;
            throw new InvalidOperationException("not a lock failure");
        }));
        Assert.Equal(1, attempts);
    }

    [Fact]
    public void APersistentDeadlockGivesUpRatherThanLoopingForever()
    {
        var db = NewDb();
        var attempts = 0;
        Assert.Throws<SqliteException>(() => db.WithRetryOnLockFailure(() =>
        {
            attempts++;
            throw LockException(5);
        }, attempts: 3));
        Assert.Equal(3, attempts);
    }

    /// <summary>Ensure() is what the retry wraps, so re-running it must be harmless. This is the
    /// premise the whole retry rests on: a retry of non-idempotent work would corrupt rather than
    /// recover.</summary>
    [SqliteOnlyFact(
        "the assertion is an exact global row count for the seeded packs. On SQLite that is a fresh " +
        "database per call, so the count means what it says. On MySQL every unit test shares one run " +
        "database that the harness restores from a post-migration template, so the count reflects the " +
        "template rather than this test's own installs — it would be asserting the harness, not Ensure.")]
    public void EnsureIsSafeToRunRepeatedly()
    {
        var db = NewDb();
        WorldSchema.Ensure(db);
        WorldSchema.Ensure(db);
        WorldSchema.Ensure(db);

        Assert.Equal(1L, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_admin_users WHERE email='owner@pciworld.local'"));
        Assert.Equal((long)WorldArticlePack.Count, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_articles WHERE author_id IS NULL"));
    }
}
