using System.Runtime.CompilerServices;
using PCI.Backend.Data;

namespace PCI.Backend.Tests;

/// <summary>
/// Hermetic process environment for the unit layer. Several app classes read environment variables in
/// static initialisers (Egress.AllowPrivate, Redirects.CanonicalHost, Security's credential key), so the
/// variables must be pinned BEFORE any app type is touched — a module initialiser runs ahead of every
/// test in the assembly. Storage and SQLite files all live under a per-run temp directory; nothing
/// depends on the repo-relative cwd and nothing dials the network.
/// </summary>
public static class TestEnv
{
    public static readonly string Root = Path.Combine(Path.GetTempPath(), "pci-backend-tests-" + Guid.NewGuid().ToString("n"));
    public static string StorageRoot => Path.Combine(Root, "storage");

    [ModuleInitializer]
    public static void Init()
    {
        Directory.CreateDirectory(StorageRoot);
        Environment.SetEnvironmentVariable("DB_PROVIDER", "sqlite");
        Environment.SetEnvironmentVariable("STORAGE_PROVIDER", "local");
        Environment.SetEnvironmentVariable("STORAGE_ROOT", StorageRoot);
        Environment.SetEnvironmentVariable("INTEGRATIONS_ALLOW_PRIVATE_EGRESS", "false");
        // empty string removes the variable → the app defaults apply deterministically
        Environment.SetEnvironmentVariable("CANONICAL_HOST", "");
        Environment.SetEnvironmentVariable("CANONICAL_REDIRECT", "");
        Environment.SetEnvironmentVariable("REDIRECT_HOSTS", "");
        Environment.SetEnvironmentVariable("SEED_DEMO_EXAM", "");
    }

    /// <summary>The app's SQLite schema file: copied next to the test assembly by the ProjectReference
    /// content flow, with a walk-up fallback to backend/schema.sql for safety.</summary>
    public static string SchemaPath()
    {
        var local = Path.Combine(AppContext.BaseDirectory, "schema.sql");
        if (File.Exists(local)) return local;
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "backend", "schema.sql");
            if (File.Exists(candidate)) return candidate;
        }
        throw new FileNotFoundException("schema.sql not found for the test database");
    }

    /// <summary>A fresh, fully-migrated SQLite database on a unique temp file.</summary>
    public static Db NewMigratedDb()
    {
        var path = Path.Combine(Root, "db-" + Guid.NewGuid().ToString("n") + ".db");
        var db = new Db(path);
        Migrate.Run(db, SchemaPath());
        return db;
    }
}

/// <summary>One migrated database per test class (xUnit IClassFixture).</summary>
public sealed class DbFixture
{
    public Db Db { get; } = TestEnv.NewMigratedDb();
}
