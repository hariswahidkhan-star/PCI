using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — content-library pack tests. simlab_scenarios_seed.json ships a large synthetic
/// practice library that SimLabContentSeed publishes at boot; these tests re-run EVERY entry through the
/// real §14 publication gate (SimContent.Validate — metadata, retired-name scan, reference-solver answer
/// check, variant-seed gradability) so the pack can never drift from the engine, plus uniqueness and
/// governance-completeness checks the seeder relies on. If an entry stops resolving (e.g. an engine task
/// changes shape), this fails loudly naming the scenario code.
/// </summary>
public class SimLabContentSeedTests
{
    // House starter catalogue seeded in-code by SimLabSchema and SimLabContentPack — pack codes must never
    // collide with these. House content owns the -001/-002/-900 numbering; this library owns -101 upward.
    static readonly string[] HouseCodes =
    {
        "GL-WBS-001", "GL-EVM-001", "SD-EVM-001", "GL-SCH-001", "SD-FCT-001", "GL-CBS-001", "SC-EVM-001",
        "GL-PRG-001", "SD-RSK-001", "GL-PRT-001", "SC-MC-001", "GL-CHG-001", "GL-CASH-001", "SC-EVT-001",
        "SD-ESC-001", "GL-BOQ-001", "GL-DQ-001", "GL-PORT-001", "GL-PROC-001", "GL-PROD-001", "GL-RES-001",
        "SD-DEC-001", "SC-CASH-002", "SC-CBS-002", "SC-PORT-002", "SC-PRG-002", "SC-RSK-002",
        "CP-EVM-900", "CP-INT-900", "CP-SCH-900", "MS-RECOVERY-001",
    };

    static readonly string[] Kinds = { "guided_lab", "skill_drill", "scenario", "capstone", "team" };

    sealed record PackRow(
        string Code, string? Title, string? Kind, string? Industry, string? Difficulty, int Minutes,
        long? CertificationId, string? CompetenciesJson, string? Summary, string? Brief,
        string? ObjectivesJson, string? Provenance, string? Disclaimers, string? WorkedSolution,
        string? ConfigJson);

    static string PackPath()
    {
        var local = Path.Combine(AppContext.BaseDirectory, "simlab_scenarios_seed.json");
        if (File.Exists(local)) return local;
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "backend", "simlab_scenarios_seed.json");
            if (File.Exists(candidate)) return candidate;
        }
        throw new FileNotFoundException("simlab_scenarios_seed.json not found — the SimLab content pack must ship with the backend");
    }

    static List<PackRow> Load()
    {
        using var doc = JsonDocument.Parse(File.ReadAllText(PackPath()));
        var rows = new List<PackRow>();
        foreach (var s in doc.RootElement.GetProperty("scenarios").EnumerateArray())
        {
            string? S(string k) => s.TryGetProperty(k, out var el) && el.ValueKind == JsonValueKind.String ? el.GetString() : null;
            string? Raw(string k) => s.TryGetProperty(k, out var el) && el.ValueKind is JsonValueKind.Array or JsonValueKind.Object ? el.GetRawText() : null;
            rows.Add(new PackRow(
                S("scenario_code") ?? "", S("title"), S("kind"), S("industry"), S("difficulty"),
                s.TryGetProperty("est_minutes", out var m) && m.TryGetInt32(out var mi) ? mi : 0,
                s.TryGetProperty("certification_id", out var c) && c.ValueKind == JsonValueKind.Number ? c.GetInt64() : null,
                Raw("competencies"), S("summary"), S("brief"), Raw("objectives"),
                S("provenance"), S("disclaimers"), S("worked_solution"), Raw("config")));
        }
        return rows;
    }

    [Fact]
    public void Pack_is_substantial_with_unique_codes_that_never_collide_with_house_content()
    {
        var rows = Load();
        Assert.True(rows.Count >= 80, $"expected a substantial library, got {rows.Count} scenarios");
        Assert.Equal(rows.Count, rows.Select(r => r.Code).Distinct(StringComparer.Ordinal).Count());
        Assert.Empty(rows.Select(r => r.Code).Intersect(HouseCodes, StringComparer.Ordinal));
    }

    [Fact]
    public void Every_pack_scenario_passes_the_publication_validator()
    {
        var rows = Load();
        var codes = rows.Select(r => r.Code).ToList();
        foreach (var r in rows)
        {
            var others = codes.Where(c => !string.Equals(c, r.Code, StringComparison.Ordinal)).Concat(HouseCodes);
            var issues = SimContent.Validate(new SimContent.ScenarioInput(
                r.Code, r.Title, r.Summary, r.Difficulty, r.CertificationId, r.CompetenciesJson,
                r.ConfigJson, SyntheticDeclared: true), others);
            var errors = issues.Where(i => i.Severity == SimContent.Severity.Error).ToList();
            Assert.True(SimContent.Publishable(issues),
                $"{r.Code} is not publishable: {string.Join("; ", errors.Select(i => $"{i.Code}: {i.Message}"))}");
        }
    }

    /// <summary>
    /// Curriculum breadth. The first library shipped 48 foundation + 48 intermediate scenarios and nothing
    /// harder — a generation bug that silently capped the practice ceiling well below the certifications the
    /// Lab feeds. Pin both axes so that regression cannot recur unnoticed: every difficulty band is stocked,
    /// and the pack spans a broad range of engine task types rather than clustering on the easy ones.
    /// Deliberately NOT asserted against SimCalc.KnownTasks in full — a newly added engine should not fail
    /// this suite before its content is authored; it should show up as a thin band here instead.
    /// </summary>
    [Fact]
    public void Pack_stocks_every_difficulty_band_and_spans_the_engine_task_range()
    {
        var rows = Load();
        foreach (var band in new[] { "foundation", "intermediate", "advanced", "expert" })
            Assert.True(rows.Count(r => r.Difficulty == band) >= 5,
                $"difficulty band '{band}' has only {rows.Count(r => r.Difficulty == band)} scenarios — the library must stock every band");

        var tasks = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var r in rows)
        {
            if (string.IsNullOrWhiteSpace(r.ConfigJson)) continue;
            using var doc = JsonDocument.Parse(r.ConfigJson);
            if (doc.RootElement.TryGetProperty("task", out var t) && t.GetString() is { Length: > 0 } name)
                tasks.Add(name);
        }
        Assert.True(tasks.Count >= 15, $"pack covers only {tasks.Count} engine task types: {string.Join(", ", tasks)}");
    }

    [Fact]
    public void Every_pack_scenario_carries_complete_catalogue_and_governance_metadata()
    {
        foreach (var r in Load())
        {
            Assert.False(string.IsNullOrWhiteSpace(r.Title), $"{r.Code}: title");
            Assert.Contains(r.Kind, Kinds);
            Assert.False(string.IsNullOrWhiteSpace(r.Industry), $"{r.Code}: industry");
            Assert.True(r.Minutes > 0, $"{r.Code}: est_minutes");
            Assert.True(r.CertificationId is 1 or 2 or 3,
                $"{r.Code}: certification_id must map to a live certification (1=PCL-AI, 2=PFL, 3=PML-AI)");
            Assert.False(string.IsNullOrWhiteSpace(r.Summary), $"{r.Code}: summary");
            Assert.False(string.IsNullOrWhiteSpace(r.Brief), $"{r.Code}: brief");
            Assert.False(string.IsNullOrWhiteSpace(r.ObjectivesJson), $"{r.Code}: objectives");
            Assert.False(string.IsNullOrWhiteSpace(r.Provenance), $"{r.Code}: provenance");
            Assert.False(string.IsNullOrWhiteSpace(r.Disclaimers), $"{r.Code}: disclaimers");
            Assert.False(string.IsNullOrWhiteSpace(r.WorkedSolution), $"{r.Code}: worked_solution");
        }
    }
}
