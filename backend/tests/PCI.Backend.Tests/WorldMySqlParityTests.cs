using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World on MySQL 8 AND MariaDB.
///
/// The schema is written once in the SQLite dialect and translated at runtime, so a statement that
/// is valid on one engine and not another is invisible until a production boot. Two divergences
/// have already bitten this codebase:
///
///   • `CREATE INDEX IF NOT EXISTS` is valid MariaDB and a SYNTAX ERROR on MySQL 8.
///   • An index over a TEXT column is silently prefixed by MariaDB when it is the only column, and
///     rejected outright by both engines when it is part of a COMPOSITE key.
///
/// Because schema installation is wrapped in a catch-and-log, one rejection aborts every statement
/// after it — the failure mode is not an error page, it is tables that quietly do not exist. These
/// tests run on SQLite and still catch both classes, so they work in ordinary CI with no server.
/// </summary>
public class WorldMySqlParityTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    // ───────────────────── the schema must not index a TEXT column ─────────────────────

    /// <summary>
    /// SQLite records the DECLARED type of every column, so it can answer a MySQL question: does any
    /// PCI World index cover a column declared TEXT? If one does, MySQL 8 refuses it (error 1170) —
    /// or, in a composite key, both engines do (error 1071) — and the installer stops there.
    /// </summary>
    [SqliteOnlyFact("Reads SQLite's own catalogue (sqlite_master / PRAGMA) to answer a MySQL question without needing a server. On MySQL the equivalent live check is migration_integrity_test.py \u00a74c.")]
    public void No_pciworld_index_covers_a_text_column()
    {
        var db = NewWorldDb();
        var offenders = new List<string>();

        foreach (var t in db.Query(
            "SELECT name FROM sqlite_master WHERE type='table' AND name LIKE 'pciworld%'"))
        {
            var table = H.Str(t["name"])!;
            var declared = db.Query($"PRAGMA table_info({table})")
                .ToDictionary(c => H.Str(c["name"])!, c => (H.Str(c["type"]) ?? "").ToUpperInvariant());

            foreach (var idx in db.Query($"PRAGMA index_list({table})"))
            {
                var index = H.Str(idx["name"])!;
                if (index.StartsWith("sqlite_autoindex", StringComparison.Ordinal)) continue;
                foreach (var col in db.Query($"PRAGMA index_info({index})"))
                {
                    var name = H.Str(col["name"]);
                    if (name is null) continue;
                    if (declared.TryGetValue(name, out var type) && type.Contains("TEXT"))
                        offenders.Add($"{table}.{name} (index {index}) is declared TEXT");
                }
            }
        }

        Assert.True(offenders.Count == 0,
            "MySQL cannot index a TEXT column without a prefix, and rejects one inside a composite key " +
            "on both engines. Declare these as a bounded VARCHAR:\n  " + string.Join("\n  ", offenders));
    }

    /// <summary>The columns that carry a datetime are bounded deliberately, not by accident — an
    /// ISO-8601 stamp is 19 characters and never needs TEXT.</summary>
    [SqliteOnlyFact("Reads SQLite's own catalogue (sqlite_master / PRAGMA) to answer a MySQL question without needing a server. On MySQL the equivalent live check is migration_integrity_test.py \u00a74c.")]
    public void Indexed_datetime_columns_are_bounded()
    {
        var db = NewWorldDb();
        foreach (var (table, column) in new[]
        {
            ("pciworld_articles", "published_at"),
            ("pciworld_attempts", "completed_at"),
            ("pciworld_audit", "created_at"),
            ("pciworld_events", "created_at"),
            ("pciworld_sessions", "last_seen_at"),
            ("pciworld_user_sessions", "expires_at"),
            ("pciworld_rotation_runs", "ran_at"),
        })
        {
            var type = db.Query($"PRAGMA table_info({table})")
                .Where(c => H.Str(c["name"]) == column)
                .Select(c => (H.Str(c["type"]) ?? "").ToUpperInvariant())
                .FirstOrDefault();
            Assert.True(type is not null && type.StartsWith("VARCHAR", StringComparison.Ordinal),
                $"{table}.{column} must be a bounded VARCHAR for MySQL to index it (found '{type}')");
        }
    }

    // ───────────────────── the translation rules ─────────────────────

    [Fact]
    public void Create_index_if_not_exists_is_stripped_for_mysql_and_kept_for_mariadb()
    {
        const string sql = "CREATE INDEX IF NOT EXISTS ix_a ON t(a)";
        const string uniq = "CREATE UNIQUE INDEX IF NOT EXISTS ux_a ON t(a, b)";

        // MySQL 8 cannot parse the clause, so it goes and Exec() absorbs the duplicate-key error.
        Assert.Equal("CREATE INDEX ix_a ON t(a)", Db.TranslateFor(sql, mariaDb: false));
        Assert.Equal("CREATE UNIQUE INDEX ux_a ON t(a, b)", Db.TranslateFor(uniq, mariaDb: false));

        // MariaDB understands it, and there the clause is load-bearing: the core schema declares some
        // indexes inline AND again as a guarded CREATE INDEX. Removing the guard turns a skip into a
        // real creation attempt that fails on key length — which is exactly how a first attempt at
        // this fix broke the boot.
        Assert.Equal(sql, Db.TranslateFor(sql, mariaDb: true));
        Assert.Equal(uniq, Db.TranslateFor(uniq, mariaDb: true));

        // CREATE TABLE IF NOT EXISTS is valid on both and must never be touched.
        const string tbl = "CREATE TABLE IF NOT EXISTS t(id INTEGER PRIMARY KEY AUTOINCREMENT)";
        foreach (var maria in new[] { true, false })
            Assert.Contains("CREATE TABLE IF NOT EXISTS", Db.TranslateFor(tbl, maria));
    }

    [Fact]
    public void The_shared_dialect_rules_apply_on_both_engines()
    {
        foreach (var maria in new[] { true, false })
        {
            Assert.Contains("BIGINT PRIMARY KEY AUTO_INCREMENT",
                Db.TranslateFor("CREATE TABLE t(id INTEGER PRIMARY KEY AUTOINCREMENT)", maria));
            Assert.Contains("INSERT IGNORE", Db.TranslateFor("INSERT OR IGNORE INTO t(a) VALUES(1)", maria));
            Assert.Contains("UTC_TIMESTAMP()", Db.TranslateFor("SELECT datetime('now')", maria));
            // The retention sweep and the worker lease both build relative times this way; if the
            // rewrite missed them the statements would be valid SQL that deletes nothing.
            Assert.Contains("INTERVAL -180 DAY", Db.TranslateFor("DELETE FROM t WHERE a<=datetime('now','-180 days')", maria));
            Assert.Contains("INTERVAL +5 MINUTE", Db.TranslateFor("UPDATE t SET x=datetime('now','+5 minutes')", maria));
            Assert.Contains("INTERVAL +8 HOUR", Db.TranslateFor("INSERT INTO t(e) VALUES(datetime('now','+8 hours'))", maria));
        }
    }

    /// <summary>Every relative-time expression the world modules actually emit must translate. A
    /// missed one is silent: valid SQL that compares against the literal string.</summary>
    [Theory]
    [InlineData("datetime('now','-1 day')")]
    [InlineData("datetime('now','+2 hours')")]
    [InlineData("datetime('now','-400 days')")]
    [InlineData("datetime('now','+30 days')")]
    [InlineData("datetime('now','+1825 days')")]
    [InlineData("date('now','-1 day')")]
    public void Every_relative_time_expression_translates(string expr)
    {
        var translated = Db.TranslateFor($"SELECT * FROM t WHERE a<={expr}", mariaDb: false);
        Assert.DoesNotContain("datetime('now'", translated);
        Assert.DoesNotContain("date('now'", translated);
        Assert.Contains("UTC_TIMESTAMP()", translated);
    }
}
