using System.Security.Cryptography;
using System.Text;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Immutable published-scenario version ledger (P0 — deterministic replay foundation). Every time a
/// scenario is published, the exact (version, config_json) it was published with is frozen into
/// <c>simulation_scenario_versions</c> and never updated. Attempts additionally carry their own
/// <c>config_snapshot</c> (see <see cref="SimReplay"/>), so a later revision, republish or seed refresh can
/// never alter a historical attempt's grading, coaching or replay. The ledger is the auditable record of
/// what each published version contained (with an integrity checksum); the per-attempt snapshot is the
/// authoritative replay source.
/// </summary>
public static class SimVersion
{
    /// <summary>Stable lowercase hex SHA-256 of a scenario's config JSON, for integrity/provenance.</summary>
    public static string Checksum(string? configJson) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(configJson ?? ""))).ToLowerInvariant();

    /// <summary>Idempotently record the immutable ledger row for a scenario's current (version, config).
    /// Returns the ledger row id (0 when the scenario is missing). Safe to call on every publish and during
    /// backfill: a row for (scenario_id, version) is written once and never mutated.</summary>
    public static long EnsureVersion(Db db, long scenarioId, long? publishedBy = null)
    {
        var s = db.QueryOne(@"SELECT id,scenario_code,title,difficulty,competencies_json,config_json,version,published_at
            FROM simulation_scenarios WHERE id=?", scenarioId);
        if (s is null) return 0;
        var version = (int)H.L(s["version"]);
        var existing = db.QueryOne("SELECT id FROM simulation_scenario_versions WHERE scenario_id=? AND version=?", scenarioId, version);
        if (existing is not null) return H.L(existing["id"]);
        var config = H.Str(s["config_json"]);
        try
        {
            return db.ExecuteReturningId(@"INSERT INTO simulation_scenario_versions
                (scenario_id,scenario_code,version,title,difficulty,competencies_json,config_json,checksum,published_at,published_by)
                VALUES(?,?,?,?,?,?,?,?,?,?)",
                scenarioId, H.Str(s["scenario_code"]), version, H.Str(s["title"]), H.Str(s["difficulty"]),
                H.Str(s["competencies_json"]), config, Checksum(config),
                H.Str(s["published_at"]) ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), publishedBy);
        }
        catch
        {
            // A concurrent publish/backfill won the unique (scenario_id, version) key — read it back.
            var row = db.QueryOne("SELECT id FROM simulation_scenario_versions WHERE scenario_id=? AND version=?", scenarioId, version);
            return row is null ? 0 : H.L(row["id"]);
        }
    }
}
