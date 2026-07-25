using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Hermetic process environment for the unit layer. Several app classes read environment variables in
/// static initialisers (Egress.AllowPrivate, Redirects.CanonicalHost, Security's credential key), so the
/// variables must be pinned BEFORE any app type is touched — a module initialiser runs ahead of every
/// test in the assembly. Storage and SQLite files all live under a per-run temp directory; nothing
/// depends on the repo-relative cwd and nothing dials the network.
///
/// <b>Provider.</b> <c>TEST_DB_PROVIDER=mysql</c> runs the suite against a real MySQL/MariaDB instead of
/// SQLite, matching the convention the Python suites already use. This matters because production runs
/// MySQL only — SQLite is a dev/test convenience — so logic verified solely on SQLite is logic nobody has
/// checked against the database that actually holds the money. The <c>ix_plc_link</c> defect (an index
/// MySQL rejected while SQLite accepted it, swallowed by the installer) is precisely what that blind spot
/// produces.
///
/// On MySQL the suite uses ONE database for the run and resets it between tests, rather than a database
/// per test: creating and migrating a 175-table schema per test would dominate the run time. The reset
/// restores the database to its exact post-<see cref="Migrate.Run"/> state, which is precisely the state
/// a brand-new SQLite file is in — so a test cannot tell the two providers apart.
///
/// <b>Run the MySQL leg one collection at a time</b>, because that one database is shared:
/// <code>dotnet test … -- xUnit.ParallelizeTestCollections=false</code>
/// Without it two collections interleave and each resets the other's fixtures mid-test, which surfaces as
/// assertion failures scattered across unrelated suites. The SQLite leg has no such constraint (a file per
/// test) and should stay parallel — it is the slower of the two by an order of magnitude.
/// </summary>
public static class TestEnv
{
    public static readonly string Root = Path.Combine(Path.GetTempPath(), "pci-backend-tests-" + Guid.NewGuid().ToString("n"));
    public static string StorageRoot => Path.Combine(Root, "storage");

    /// <summary>"sqlite" (default) or "mysql".</summary>
    public static readonly string Provider =
        (Environment.GetEnvironmentVariable("TEST_DB_PROVIDER") ?? "sqlite").Trim().ToLowerInvariant() is "mysql" or "mariadb"
            ? "mysql" : "sqlite";

    public static bool IsMySql => Provider == "mysql";

    [ModuleInitializer]
    public static void Init()
    {
        Directory.CreateDirectory(StorageRoot);
        Environment.SetEnvironmentVariable("DB_PROVIDER", Provider);
        Environment.SetEnvironmentVariable("STORAGE_PROVIDER", "local");
        Environment.SetEnvironmentVariable("STORAGE_ROOT", StorageRoot);
        Environment.SetEnvironmentVariable("INTEGRATIONS_ALLOW_PRIVATE_EGRESS", "false");
        // empty string removes the variable → the app defaults apply deterministically
        Environment.SetEnvironmentVariable("CANONICAL_HOST", "");
        Environment.SetEnvironmentVariable("CANONICAL_REDIRECT", "");
        Environment.SetEnvironmentVariable("REDIRECT_HOSTS", "");
        Environment.SetEnvironmentVariable("SEED_DEMO_EXAM", "");
        if (IsMySql)
        {
            // A dedicated database so a unit run can never touch the one the integration suites use.
            var name = (Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "pci") + "_unit";
            Environment.SetEnvironmentVariable("MYSQL_DATABASE", name);
        }
    }

    /// <summary>
    /// A UTC timestamp in the format the platform stores dates in, offset by a SQLite datetime() modifier
    /// ("+29 day", "-31 day", "+30 minutes").
    ///
    /// A fixture that needs a relative date must bind a value computed here rather than write
    /// <c>datetime('now', ?)</c>. <see cref="Db.Translate"/> rewrites the LITERAL form for MySQL by regex,
    /// so a modifier that arrives as a bound parameter is invisible to it and the statement reaches MariaDB
    /// as a call to a function it does not have. SQLite evaluates it happily, which is what made the whole
    /// class of breakage provider-specific and therefore unseen.
    /// </summary>
    public static string Stamp(string modifier)
    {
        var m = Regex.Match(modifier.Trim(),
                            @"^([+-]?\d+)\s+(year|month|day|hour|minute|second)s?$",
                            RegexOptions.IgnoreCase);
        if (!m.Success) throw new ArgumentException($"unsupported datetime modifier '{modifier}'", nameof(modifier));
        var n = int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
        var now = DateTime.UtcNow;
        var at = m.Groups[2].Value.ToLowerInvariant() switch
        {
            "year"   => now.AddYears(n),
            "month"  => now.AddMonths(n),
            "day"    => now.AddDays(n),
            "hour"   => now.AddHours(n),
            "minute" => now.AddMinutes(n),
            _        => now.AddSeconds(n),
        };
        return at.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// The app's schema file for the active provider — the SAME file the app boots from, so the tests
    /// exercise the production schema (including the hand-tuned DECIMAL money columns and index prefix
    /// lengths in schema.mysql.sql) rather than a translated approximation of it.
    /// </summary>
    public static string SchemaPath()
    {
        var file = IsMySql ? "schema.mysql.sql" : "schema.sql";
        var local = Path.Combine(AppContext.BaseDirectory, file);
        if (File.Exists(local)) return local;
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "backend", file);
            if (File.Exists(candidate)) return candidate;
        }
        throw new FileNotFoundException($"{file} not found for the test database");
    }

    // ---- MySQL: one migrated database per run, reset to its post-migration state between tests ----

    static readonly object MySqlGate = new();
    static Db? _mysql;

    /// <summary>Sibling database holding the post-migration content of every table that was seeded.</summary>
    static string _template = "";

    /// <summary>
    /// The tables the migration left with rows in them — the ones a reset must REFILL rather than merely
    /// empty — mapped to the column list captured at snapshot time. Everything else the reset just empties.
    ///
    /// The columns are named rather than restored with <c>SELECT *</c> so that a module installer which
    /// later adds a column to one of these tables does not turn every MySQL test into "Column count doesn't
    /// match value count". The new column simply takes its default, which IS its post-migration state.
    /// </summary>
    static readonly Dictionary<string, string> Seeded = new(StringComparer.OrdinalIgnoreCase);

    static Db MySqlDb()
    {
        lock (MySqlGate)
        {
            if (_mysql is not null) return _mysql;
            var db = new Db("unused-for-mysql");
            Migrate.Run(db, SchemaPath());
            // The snapshot is taken HERE, before any module Ensure runs, because this is the state a fresh
            // SQLite file is in when NewMigratedDb hands it over. Tables a module installer adds later
            // (finance, PCI World, SimLab, …) are absent from the template and so are emptied outright by a
            // reset — which is what the test then repairs by calling that module's Ensure itself, exactly
            // as it does on SQLite.
            SnapshotTemplate(db);
            FinanceSchema.Ensure(db);
            _mysql = db;
            return db;
        }
    }

    /// <summary>Every base table in <paramref name="schema"/>, read live so tables created after boot count.</summary>
    static List<string> BaseTables(Db db, string schema) =>
        db.Query("SELECT table_name AS t FROM information_schema.tables " +
                 "WHERE table_schema=? AND table_type='BASE TABLE'", schema)
          .Select(r => Convert.ToString(r["t"]) ?? "")
          .Where(t => t.Length > 0)
          .ToList();

    /// <summary>
    /// Copy the seeded rows aside once, so a reset can restore them server-side with INSERT … SELECT and
    /// never move reference data across the wire. Only non-empty tables are copied: an empty one needs
    /// nothing but a DELETE to get back to its post-migration state.
    /// </summary>
    static void SnapshotTemplate(Db db)
    {
        var live = db.Scalar<string>("SELECT DATABASE()") ?? "";
        _template = live + "_tpl";
        db.Exec($"DROP DATABASE IF EXISTS `{_template}`");
        db.Exec($"CREATE DATABASE `{_template}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");
        foreach (var t in BaseTables(db, live))
        {
            if (db.Scalar<long>($"SELECT COUNT(*) FROM `{t}`") == 0) continue;
            db.Exec($"CREATE TABLE `{_template}`.`{t}` LIKE `{live}`.`{t}`");
            db.Exec($"INSERT INTO `{_template}`.`{t}` SELECT * FROM `{live}`.`{t}`");
            var cols = db.Query("SELECT column_name AS c FROM information_schema.columns " +
                                "WHERE table_schema=? AND table_name=? ORDER BY ordinal_position", live, t)
                         .Select(r => $"`{Convert.ToString(r["c"])}`");
            Seeded[t] = string.Join(",", cols);
        }
    }

    /// <summary>
    /// Put the run database back into its post-migration state: every table emptied, then the seeded ones
    /// refilled from the template.
    ///
    /// This deliberately replaces the hand-maintained allow-list of "tables the tests own" that came before
    /// it. That list only ever covered the finance module, so every other suite ran against whatever the
    /// previous test had left behind — which is why the MySQL unit job could only be pointed at the finance
    /// filter. A list also rots silently: a new table is a new leak, and the symptom (a duplicate key, a
    /// count that is one too high) reads like a product bug rather than a harness bug.
    ///
    /// FK checks are suspended so the delete order does not have to encode the platform's referential
    /// graph, and the whole reset is sent as ONE statement batch — 175 round trips per test would cost more
    /// than the tests themselves.
    /// </summary>
    static void ResetToTemplate(Db db)
    {
        var live = db.Scalar<string>("SELECT DATABASE()") ?? "";
        var sql = new StringBuilder("SET FOREIGN_KEY_CHECKS=0;\n");
        foreach (var t in BaseTables(db, live))
        {
            sql.Append($"DELETE FROM `{t}`;\n");
            if (Seeded.TryGetValue(t, out var cols))
                sql.Append($"INSERT INTO `{t}` ({cols}) SELECT {cols} FROM `{_template}`.`{t}`;\n");
        }
        sql.Append("SET FOREIGN_KEY_CHECKS=1;\n");
        db.Exec(sql.ToString());
    }

    /// <summary>
    /// A migrated database in its post-migration state.
    ///
    /// On SQLite that is a brand-new file per call. On MySQL it is the run's shared database reset to the
    /// same state — which is why the DB-using suites share the <see cref="DbCollection"/> collection, so no
    /// two of them run concurrently against it.
    /// </summary>
    public static Db NewMigratedDb()
    {
        if (IsMySql)
        {
            var db = MySqlDb();
            ResetToTemplate(db);
            return db;
        }
        var path = Path.Combine(Root, "db-" + Guid.NewGuid().ToString("n") + ".db");
        var sqlite = new Db(path);
        Migrate.Run(sqlite, SchemaPath());
        return sqlite;
    }
}

/// <summary>
/// A test whose PREMISE only exists on SQLite — it reads SQLite's own catalogue, or it relies on SQLite
/// accepting data MySQL's column types reject. Running it under TEST_DB_PROVIDER=mysql would not be a
/// stronger check; it would fail for a reason that has nothing to do with the behaviour under test. Each
/// use must say why, so the skip is a documented decision in the run output rather than a silent hole.
/// </summary>
public sealed class SqliteOnlyFactAttribute : FactAttribute
{
    /// <param name="because">Why this test cannot mean anything on MySQL. Becomes the skip reason.</param>
    public SqliteOnlyFactAttribute(string because)
    {
        if (TestEnv.IsMySql) Skip = because;
    }
}

/// <summary>
/// Serialises the suites that share the MySQL run database. xUnit runs distinct collections in parallel,
/// so without this the between-test wipe of one suite would delete another suite's fixtures mid-test. On
/// SQLite it costs only the parallelism of these few classes.
/// </summary>
[CollectionDefinition(DbCollection.Name)]
public sealed class DbCollection
{
    public const string Name = "database";
}

/// <summary>One migrated database per test class (xUnit IClassFixture).</summary>
public sealed class DbFixture
{
    public Db Db { get; } = TestEnv.NewMigratedDb();
}
