using System.Runtime.CompilerServices;
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
/// On MySQL the suite uses ONE database for the run and wipes the test-owned tables between tests, rather
/// than a database per test: creating and migrating a 70-plus-table schema per test would dominate the
/// run time. Seeded reference data (certifications, pricing, settings) is deliberately left in place —
/// tests read it and never own it.
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

    // ---- MySQL: one migrated database per run, wiped between tests ----

    static readonly object MySqlGate = new();
    static Db? _mysql;

    /// <summary>
    /// Tables the unit tests OWN and may therefore truncate. Reference data seeded by the migration
    /// (certifications, pricing_rules, site_settings, …) is absent on purpose: tests read it, so wiping it
    /// would break them in a way that looks like a product bug.
    /// </summary>
    static readonly string[] Owned =
    {
        // finance module
        "partner_link_clicks", "partner_campaign_links", "partner_dispute_messages", "partner_disputes",
        "partner_settlement_items", "partner_settlements", "partner_commission_events",
        "partner_commission_transactions", "partner_commission_rules", "partner_agreements",
        // partner + attribution
        "partner_payouts", "partner_sponsorships", "partner_sessions", "partner_users", "partner_notices",
        "training_partners", "code_redemptions", "discount_codes",
        // student-side rows the finance fixtures create
        "issued_credentials", "exam_attempts", "exam_bookings", "exam_entitlements",
        "certification_applications", "memberships", "fee_waivers", "payments",
        "student_profiles", "candidate_consents", "notifications", "login_events", "login_tokens",
        "analytics_events", "users",
    };

    static Db MySqlDb()
    {
        lock (MySqlGate)
        {
            if (_mysql is not null) return _mysql;
            var db = new Db("unused-for-mysql");
            Migrate.Run(db, SchemaPath());
            FinanceSchema.Ensure(db);
            _mysql = db;
            return db;
        }
    }

    static void WipeOwned(Db db)
    {
        // FK checks are suspended for the wipe so the order of Owned does not have to encode the whole
        // platform's referential graph — the alternative is a list that silently rots as relations change.
        try { db.Exec("SET FOREIGN_KEY_CHECKS=0"); } catch { }
        foreach (var t in Owned)
            try { db.Execute($"DELETE FROM {t}"); } catch { /* table may not exist on an older schema */ }
        try { db.Exec("SET FOREIGN_KEY_CHECKS=1"); } catch { }
    }

    /// <summary>
    /// A migrated database in a known-empty state for the tables the tests own.
    ///
    /// On SQLite that is a brand-new file per call. On MySQL it is the run's shared database with the
    /// owned tables emptied — which is why the DB-using suites share the <see cref="DbCollection"/>
    /// collection, so no two of them run concurrently against it.
    /// </summary>
    public static Db NewMigratedDb()
    {
        if (IsMySql)
        {
            var db = MySqlDb();
            WipeOwned(db);
            return db;
        }
        var path = Path.Combine(Root, "db-" + Guid.NewGuid().ToString("n") + ".db");
        var sqlite = new Db(path);
        Migrate.Run(sqlite, SchemaPath());
        return sqlite;
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
