using System.Linq;
using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// P3 content-scale gate. Seeds the full published Simulation Lab catalogue exactly as boot does — the
/// house pack + multi-step family (SimLabSchema.Ensure → SimLabContentPack.Seed) plus the validated JSON
/// library (SimLabContentSeed.Apply) — and pins the market-launch content target: at least 120 published
/// scenarios and 1,000+ gradable tasks (asked measures, counting every step of a multi-step scenario).
/// Locks the target so a future seed regression that thins the catalogue is caught in CI.
/// </summary>
public class SimContentScaleTests
{
    static int AskCount(string? configJson)
    {
        if (string.IsNullOrWhiteSpace(configJson)) return 0;
        using var doc = JsonDocument.Parse(configJson);
        var root = doc.RootElement;
        // Multi-step: sum the asks across every step. Single-task: the top-level ask array.
        if (root.TryGetProperty("steps", out var steps) && steps.ValueKind == JsonValueKind.Array)
            return steps.EnumerateArray()
                .Sum(s => s.TryGetProperty("ask", out var a) && a.ValueKind == JsonValueKind.Array ? a.GetArrayLength() : 0);
        return root.TryGetProperty("ask", out var ask) && ask.ValueKind == JsonValueKind.Array ? ask.GetArrayLength() : 0;
    }

    [Fact]
    public void Published_catalogue_meets_the_launch_content_target()
    {
        var db = TestEnv.NewMigratedDb();
        SimLabSchema.Ensure(db);         // house pack (30) + multi-step family (3)
        SimLabContentSeed.Apply(db);     // validated JSON library

        var rows = db.Query("SELECT scenario_code,config_json FROM simulation_scenarios WHERE status='published'");
        var scenarios = rows.Count;
        var tasks = rows.Sum(r => AskCount(H.Str(r["config_json"])));

        Assert.True(scenarios >= 120, $"published scenarios = {scenarios} (P3 target is >=120)");
        Assert.True(tasks >= 1000, $"published gradable tasks = {tasks} (P3 target is >=1000)");

        // Breadth: every difficulty band is stocked at launch scale, not just the easy end.
        foreach (var band in new[] { "foundation", "intermediate", "advanced", "expert" })
        {
            var n = db.Scalar<long>("SELECT COUNT(*) FROM simulation_scenarios WHERE status='published' AND difficulty=?", band);
            Assert.True(n >= 20, $"difficulty band '{band}' has only {n} published scenarios at launch scale");
        }
    }
}
