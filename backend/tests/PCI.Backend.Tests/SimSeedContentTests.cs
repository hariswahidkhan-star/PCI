using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Guardrail for the SHIPPED Simulation Lab catalogue: every scenario the app seeds
/// (<see cref="SimLabSchema"/>) must pass the Phase-5A content validator (<see cref="SimContent"/>), which
/// also runs the reference solver (<see cref="SimCalc"/>) across the validation seeds. So a malformed
/// given/ask, an unknown task, a retired credential name, or a bad variant range in house content fails
/// here — in CI — never in production. This also covers the content-expansion batch.
/// </summary>
public class SimSeedContentTests
{
    static SimContent.ScenarioInput InputFrom(Dictionary<string, object?> s) => new(
        H.Str(s["scenario_code"]) ?? "", H.Str(s["title"]), H.Str(s["summary"]), H.Str(s["difficulty"]),
        s.TryGetValue("certification_id", out var cv) && cv is not null ? H.L(cv) : (long?)null,
        H.Str(s["competencies_json"]), H.Str(s["config_json"]),
        s.TryGetValue("synthetic_declared", out var sd) && sd is not null && H.L(sd) == 1);

    [Fact]
    public void EverySeededScenario_IsPublishableAndGradable()
    {
        var db = TestEnv.NewMigratedDb();
        SimLabSchema.Ensure(db);   // create tables + seed the house catalogue (idempotent)

        var rows = db.Query("SELECT * FROM simulation_scenarios");
        Assert.True(rows.Count >= 25, $"expected the expanded house catalogue (>=25), found {rows.Count}");

        var codes = rows.Select(r => H.Str(r["scenario_code"]) ?? "").ToList();
        // No duplicate scenario_code among the seeded rows (the validator's uniqueness check relies on this).
        Assert.Equal(codes.Count, codes.Distinct().Count());

        foreach (var r in rows)
        {
            var code = H.Str(r["scenario_code"]);
            var others = codes.Where(c => c != code);
            var issues = SimContent.Validate(InputFrom(r), others);
            Assert.True(SimContent.Publishable(issues),
                $"seeded scenario {code} is not publishable: {string.Join("; ", issues.Select(i => i.Code + " — " + i.Message))}");
        }
    }
}
