using System.Collections.Concurrent;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// WorldSchema.Ensure must survive being called by two processes at once (CCP-P2-008).
///
/// WHY THIS IS NOT A HYPOTHETICAL. Ensure() runs at every boot and at the top of every World test
/// class. xUnit runs distinct collections in parallel, and on MySQL every class shares one run
/// database, so two callers genuinely do execute the bootstrap seeds against the same tables at the
/// same time. At the time of writing ~20 World suites still lack [Collection(DbCollection.Name)],
/// so the serialisation that was added elsewhere does not cover them.
///
/// Both seeds were check-then-act:
///
///   WorldSchema      SELECT COUNT(*) FROM pciworld_admin_users == 0   →   INSERT owner@pciworld.local
///   WorldArticlePack SELECT id FROM pciworld_articles WHERE slug=? IS NULL → INSERT that slug
///
/// Two callers both read the empty state and both insert; the loser dies on a duplicate key. The
/// register reproduced exactly that:
///   MySqlException: Duplicate entry 'owner@pciworld.local' for key 'email'
///   MySqlException: Duplicate entry 'why-your-spi-recovers-as-the-project-ends' for key 'slug'
///
/// A SEQUENTIAL double-Ensure would pass even with the bug, because the second call sees a non-empty
/// table and skips. Only concurrency reproduces it, so these tests are concurrent. They are not
/// serialised into DbCollection deliberately: this suite owns its own database (a private SQLite
/// file, or the shared MySQL one it resets first) and its entire subject is what happens when
/// several connections hit it together.
///
/// The assertions are about STATE, not about the absence of an exception alone. A seed that silently
/// wrote nothing would also throw nothing.
/// </summary>
public class WorldSeedConcurrencyTests
{
    const int Racers = 6;

    /// <summary>A migrated database plus a factory for INDEPENDENT connections to that same database.
    /// Independence is the point — Db serialises its own statements behind a lock, so racing one
    /// instance against itself would prove nothing about two processes.</summary>
    static (Db first, Func<Db> another) SharedDatabase()
    {
        if (TestEnv.IsMySql)
        {
            // Resets the run database to the migration template; every new Db() then connects to
            // that same database, because the connection target comes from the environment.
            var db = TestEnv.NewMigratedDb();
            return (db, () => new Db("ignored-on-mysql"));
        }
        var path = Path.Combine(TestEnv.Root, "seedrace-" + Guid.NewGuid().ToString("n") + ".db");
        var seed = new Db(path);
        Migrate.Run(seed, TestEnv.SchemaPath());
        return (seed, () => new Db(path));
    }

    static IReadOnlyList<Exception> EnsureFromManyConnections(Func<Db> another)
    {
        var errors = new ConcurrentBag<Exception>();
        var ready = new Barrier(Racers);
        var threads = new Thread[Racers];
        for (var i = 0; i < Racers; i++)
        {
            threads[i] = new Thread(() =>
            {
                try
                {
                    var db = another();
                    // Open the connection and reach the seeds together. Without the barrier the
                    // first caller usually finishes before the second starts, and the race — which
                    // is the entire subject of this test — never happens.
                    ready.SignalAndWait();
                    WorldSchema.Ensure(db);
                }
                catch (Exception e) { errors.Add(e); }
            });
            threads[i].Start();
        }
        foreach (var t in threads) t.Join();
        return errors.ToList();
    }

    [Fact]
    public void ConcurrentEnsureDoesNotDuplicateTheBootstrapOwner()
    {
        var (db, another) = SharedDatabase();

        var errors = EnsureFromManyConnections(another);

        Assert.True(errors.Count == 0,
            "Ensure() threw when run concurrently: " + string.Join(" | ", errors.Select(e => e.Message)));

        // Exactly one — not zero (every caller skipped and nobody bootstrapped) and not several.
        Assert.Equal(1L, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_admin_users WHERE email='owner@pciworld.local'"));
    }

    [Fact]
    public void ConcurrentEnsureDoesNotDuplicateSeededArticles()
    {
        var (db, another) = SharedDatabase();

        var errors = EnsureFromManyConnections(another);

        Assert.True(errors.Count == 0,
            "Ensure() threw when run concurrently: " + string.Join(" | ", errors.Select(e => e.Message)));

        Assert.Equal((long)WorldArticlePack.Count, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_articles WHERE author_id IS NULL"));
    }

    /// <summary>
    /// The version chain must survive the race: no orphan versions, no version carrying another
    /// article's body, and no seeded article left without its version 1.
    ///
    /// WHAT THIS TEST DOES AND DOES NOT PROVE. It guards the invariant, not one implementation of
    /// it. The seeders re-read the article id by slug rather than trusting last_insert_rowid() after
    /// a possibly-ignored insert; I checked by mutation whether this test distinguishes that from
    /// the naive version that trusts the returned id, and IT DOES NOT — all three tests pass either
    /// way. The unique index on (article_id, version) absorbs a wrong-id insert, because the winner
    /// has already written version 1.
    ///
    /// That is worth stating rather than leaving implied. The re-read is kept on soundness grounds,
    /// not because a test failure demands it, and the protection currently comes from the index. If
    /// house articles ever gain a second version at seed time, the index stops absorbing it and this
    /// test becomes the thing that catches it.
    /// </summary>
    [Fact]
    public void ConcurrentEnsureNeverAttachesAVersionToTheWrongArticle()
    {
        var (db, another) = SharedDatabase();

        EnsureFromManyConnections(another);

        // An orphan version — one whose article_id matches no article — is what the naive fix
        // actually produces: a racer that lost every insert on its own connection has
        // last_insert_rowid() == 0, and writes version 1 against article 0.
        var orphans = db.Scalar<long>(
            @"SELECT COUNT(*) FROM pciworld_article_versions v
              WHERE NOT EXISTS(SELECT 1 FROM pciworld_articles a WHERE a.id = v.article_id)");
        Assert.Equal(0L, orphans);

        var mismatched = db.Scalar<long>(
            @"SELECT COUNT(*) FROM pciworld_articles a
              JOIN pciworld_article_versions v ON v.article_id = a.id AND v.version = 1
              WHERE a.author_id IS NULL AND v.body_md <> a.body_md");
        Assert.Equal(0L, mismatched);

        // And every seeded article actually has its version 1 — a seeder that lost the race must not
        // leave an article with no version at all.
        Assert.Equal((long)WorldArticlePack.Count, db.Scalar<long>(
            @"SELECT COUNT(*) FROM pciworld_articles a
              WHERE a.author_id IS NULL
                AND EXISTS(SELECT 1 FROM pciworld_article_versions v
                           WHERE v.article_id = a.id AND v.version = 1)"));
    }
}
