using System.Collections.Generic;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Resolves the exact base scenario config an attempt must replay against (P0 — deterministic replay).
/// An attempt is pinned to the <c>config_snapshot</c> frozen when it started, so grading, review-reload and
/// the AI Coach always work from the content the student actually saw — never the live catalogue row, which
/// a later revision, republish or seed refresh may have changed. Legacy attempts recorded before the
/// snapshot column existed fall back to the current scenario config (their historical behaviour), which the
/// P0 migration also backfills into the snapshot so they too become immutable.
/// </summary>
public static class SimReplay
{
    /// <summary>The frozen base config JSON for an attempt row (loaded with SELECT *), or the live scenario
    /// config as a defensive fallback for un-backfilled legacy rows. Null when neither exists.</summary>
    public static string? BaseConfig(Db db, IDictionary<string, object?> attempt)
    {
        var snap = attempt.TryGetValue("config_snapshot", out var v) ? H.Str(v) : null;
        if (!string.IsNullOrWhiteSpace(snap)) return snap;
        var s = db.QueryOne("SELECT config_json FROM simulation_scenarios WHERE id=?", H.L(attempt["scenario_id"]));
        return s is null ? null : H.Str(s["config_json"]);
    }
}
