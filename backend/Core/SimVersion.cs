using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — immutable per-version scenario snapshots (the P0 replay guarantee).
///
/// An attempt pins (scenario_id, scenario_version, seed), but until now the config those keys named was
/// read live from simulation_scenarios — so a Studio revision or a content-pack deploy that rewrites
/// config_json in place could change how a historical attempt loads, grades and coaches. This class
/// freezes the meaning: the first time a (scenario, version) pair is served it is written once into
/// simulation_scenario_versions, and load/submit/coach replay from that snapshot forever after.
///
/// Two invariants keep this sound:
///  • the snapshot is append-only — first write wins, never updated;
///  • any seeder that changes a house scenario's config MUST bump `version` (SimLabContentPack does),
///    so one version number never names two different configs.
/// Attempts recorded before the snapshot table existed are frozen by the boot-time backfill
/// (SimLabSchema.Ensure) at whatever the live row said — exactly what they were being served.
/// </summary>
public static class SimVersion
{
    /// <summary>Freeze this (scenario, version) pair's config if it is not frozen already.</summary>
    public static void Snapshot(Db db, long scenarioId, long version, string? configJson)
    {
        if (string.IsNullOrWhiteSpace(configJson)) return;   // a non-interactive row has nothing to pin
        db.Execute(@"INSERT OR IGNORE INTO simulation_scenario_versions(scenario_id,version,config_json)
            VALUES(?,?,?)", scenarioId, version, configJson);
    }

    /// <summary>The config an attempt pinned at start: its frozen (scenario, version) snapshot, or the
    /// live row's config for an attempt that predates snapshotting (the boot backfill freezes those, so
    /// this fallback only carries a mid-upgrade boot).</summary>
    public static string? PinnedConfig(Db db, long scenarioId, long version, string? liveConfig)
    {
        var row = db.QueryOne(@"SELECT config_json FROM simulation_scenario_versions
            WHERE scenario_id=? AND version=?", scenarioId, version);
        var frozen = row is null ? null : H.Str(row["config_json"]);
        return string.IsNullOrWhiteSpace(frozen) ? liveConfig : frozen;
    }
}
