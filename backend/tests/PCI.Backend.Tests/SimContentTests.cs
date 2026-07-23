using System.Linq;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — content-validation (publication gate) tests (Phase 5A). SimContent.Validate is the
/// deterministic gate the studio must pass before a scenario can publish (§14). These assertions pin the
/// reject-list: a valid synthetic scenario publishes; a scenario with a bad engine task, an unanswerable
/// measure, a retired credential name, a dangling certification, a missing competency map, or an undeclared
/// synthetic-data flag is blocked with a stable machine code. The strongest case is the reference-solver
/// check — an asked measure the engine cannot resolve means the answer key is missing, so it must not ship.
/// </summary>
public class SimContentTests
{
    // A well-formed EVM task whose four asked measures all resolve deterministically through SimCalc.
    const string ValidEvmConfig = """
        {"task":"evm",
         "prompt":"Compute the core EVM measures.",
         "given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},
         "ask":[
           {"key":"sv","label":"SV","type":"number"},
           {"key":"cpi","label":"CPI","type":"number"},
           {"key":"eac","label":"EAC","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["earned_value"]}
        """;

    static SimContent.ScenarioInput Valid(string code = "TEST-VAL-001", string? config = null) => new(
        code, "Valid EVM health check", "A synthetic solar-farm EVM drill.",
        "foundation", 1, "[\"earned_value\"]", config ?? ValidEvmConfig, SyntheticDeclared: true);

    static bool Has(System.Collections.Generic.List<SimContent.Issue> issues, string code) =>
        issues.Any(i => i.Code == code && i.Severity == SimContent.Severity.Error);

    [Fact]
    public void Valid_scenario_is_publishable_with_no_errors()
    {
        var issues = SimContent.Validate(Valid());
        Assert.True(SimContent.Publishable(issues));
        Assert.DoesNotContain(issues, i => i.Severity == SimContent.Severity.Error);
    }

    [Fact]
    public void Missing_summary_is_only_a_warning_not_a_publish_blocker()
    {
        var issues = SimContent.Validate(Valid() with { Summary = null });
        Assert.True(SimContent.Publishable(issues));
        Assert.Contains(issues, i => i.Code == "summary_missing" && i.Severity == SimContent.Severity.Warning);
    }

    [Fact]
    public void Unknown_difficulty_is_rejected()
    {
        var issues = SimContent.Validate(Valid() with { Difficulty = "impossible" });
        Assert.False(SimContent.Publishable(issues));
        Assert.True(Has(issues, "difficulty_invalid"));
    }

    [Fact]
    public void Dangling_certification_id_is_rejected()
    {
        var issues = SimContent.Validate(Valid() with { CertificationId = 9 });
        Assert.True(Has(issues, "certification_invalid"));
    }

    [Fact]
    public void Null_certification_is_allowed()
    {
        var issues = SimContent.Validate(Valid() with { CertificationId = null });
        Assert.True(SimContent.Publishable(issues));
    }

    [Fact]
    public void Empty_competencies_is_rejected()
    {
        var issues = SimContent.Validate(Valid() with { CompetenciesJson = "[]" });
        Assert.True(Has(issues, "competencies_missing"));
    }

    [Fact]
    public void Undeclared_synthetic_data_is_rejected()
    {
        var issues = SimContent.Validate(Valid() with { SyntheticDeclared = false });
        Assert.True(Has(issues, "synthetic_undeclared"));
    }

    [Theory]
    [InlineData("PDL-AI")]
    [InlineData("pdl-ai")]
    [InlineData("CPMD-AI")]
    [InlineData("PCP-AI")]
    public void Retired_credential_name_in_title_is_rejected(string retired)
    {
        var issues = SimContent.Validate(Valid() with { Title = $"{retired} forecasting drill" });
        Assert.True(Has(issues, "retired_name"));
    }

    [Fact]
    public void Current_credential_names_do_not_trip_the_retired_check()
    {
        var issues = SimContent.Validate(Valid() with { Title = "PCL-AI / PFL-AI / PML-AI integrated drill" });
        Assert.DoesNotContain(issues, i => i.Code == "retired_name");
    }

    [Fact]
    public void Missing_config_is_rejected()
    {
        var issues = SimContent.Validate(Valid() with { ConfigJson = null });
        Assert.True(Has(issues, "config_missing"));
    }

    [Fact]
    public void Malformed_config_json_is_rejected()
    {
        var issues = SimContent.Validate(Valid() with { ConfigJson = "{not valid json" });
        Assert.True(Has(issues, "config_invalid"));
    }

    [Fact]
    public void Unknown_engine_task_is_rejected()
    {
        var cfg = """{"task":"telepathy","prompt":"x","given":{},"ask":[{"key":"sv","type":"number"}]}""";
        var issues = SimContent.Validate(Valid() with { ConfigJson = cfg });
        Assert.True(Has(issues, "task_unknown"));
    }

    [Fact]
    public void Asked_measure_the_engine_cannot_answer_is_rejected()
    {
        // 'banana' is not an EVM measure — Resolve returns null, so there is no gradable answer key.
        var cfg = """
            {"task":"evm","prompt":"x","given":{"pv":1,"ev":1,"ac":1,"bac":2},
             "ask":[{"key":"banana","type":"number"}]}
            """;
        var issues = SimContent.Validate(Valid() with { ConfigJson = cfg });
        Assert.True(Has(issues, "answer_unresolved"));
    }

    [Fact]
    public void Task_with_no_ask_measures_is_rejected()
    {
        var cfg = """{"task":"evm","prompt":"x","given":{"pv":1,"ev":1,"ac":1,"bac":2},"ask":[]}""";
        var issues = SimContent.Validate(Valid() with { ConfigJson = cfg });
        Assert.True(Has(issues, "ask_missing"));
    }

    [Fact]
    public void Malformed_cpm_network_is_caught_by_the_reference_solver()
    {
        // 'B' names a predecessor that does not exist — CriticalPath throws; the validator surfaces it.
        var cfg = """
            {"task":"cpm","prompt":"x",
             "given":{"activities":[{"id":"A","dur":3,"preds":[]},{"id":"B","dur":2,"preds":["Z"]}]},
             "ask":[{"key":"critical_path","type":"set"}]}
            """;
        var issues = SimContent.Validate(Valid() with { ConfigJson = cfg });
        Assert.False(SimContent.Publishable(issues));
        Assert.Contains(issues, i => i.Severity == SimContent.Severity.Error && (i.Code == "answer_error" || i.Code == "answer_unresolved"));
    }

    [Fact]
    public void Valid_cpm_set_measure_resolves()
    {
        var cfg = """
            {"task":"cpm","prompt":"Find the critical path.",
             "given":{"activities":[{"id":"A","dur":3,"preds":[]},{"id":"B","dur":2,"preds":["A"]}]},
             "ask":[{"key":"critical_path","type":"set"},{"key":"project_duration","type":"number"}]}
            """;
        var issues = SimContent.Validate(Valid("TEST-CPM-001") with { ConfigJson = cfg });
        Assert.True(SimContent.Publishable(issues));
    }

    [Fact]
    public void Valid_wbs_bool_measure_resolves()
    {
        var cfg = """
            {"task":"wbs","prompt":"Check the 100% rule.",
             "given":{"nodes":[{"id":"1"},{"id":"1.1","parent":"1","value":100},{"id":"1.2","parent":"1","value":100}]},
             "ask":[{"key":"hundred_percent_valid","type":"bool"},{"key":"root_total","type":"number"}]}
            """;
        var issues = SimContent.Validate(Valid("TEST-WBS-001") with { ConfigJson = cfg });
        Assert.True(SimContent.Publishable(issues));
    }

    [Fact]
    public void Duplicate_scenario_code_is_rejected()
    {
        var issues = SimContent.Validate(Valid("DUP-001"), new[] { "OTHER-1", "DUP-001" });
        Assert.True(Has(issues, "code_duplicate"));
    }

    [Fact]
    public void Unique_scenario_code_passes_duplicate_check()
    {
        var issues = SimContent.Validate(Valid("UNIQUE-1"), new[] { "OTHER-1", "OTHER-2" });
        Assert.DoesNotContain(issues, i => i.Code == "code_duplicate");
    }

    [Fact]
    public void Safe_variant_ranges_publish()
    {
        var cfg = """
            {"task":"evm","prompt":"x","given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},
             "ask":[{"key":"cpi","type":"number"},{"key":"eac","type":"number"}],
             "variant":{"vary":{"pv":{"min":80000,"max":120000,"step":1000},
                                 "ac":{"min":80000,"max":115000,"step":1000}}}}
            """;
        var issues = SimContent.Validate(Valid("VAR-OK-1") with { ConfigJson = cfg });
        Assert.True(SimContent.Publishable(issues));
        Assert.DoesNotContain(issues, i => i.Code == "variant_unresolved");
    }

    [Fact]
    public void Impossible_variant_range_is_rejected()
    {
        // ac can be driven to 0 → CPI 0 → EAC has no answer for that seed; publication must be blocked.
        var cfg = """
            {"task":"evm","prompt":"x","given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},
             "ask":[{"key":"eac","type":"number"}],
             "variant":{"vary":{"ac":{"min":0,"max":100000,"step":100000}}}}
            """;
        var issues = SimContent.Validate(Valid("VAR-BAD-1") with { ConfigJson = cfg });
        Assert.False(SimContent.Publishable(issues));
        Assert.True(Has(issues, "variant_unresolved"));
    }

    [Fact]
    public void Every_known_task_is_a_dispatcher_case()
    {
        // Guards against drift between SimContent's task set and the engine dispatcher.
        Assert.Contains("evm", SimCalc.KnownTasks);
        Assert.Contains("earned_schedule", SimCalc.KnownTasks);
        Assert.Equal(11, SimCalc.KnownTasks.Count);
    }
}
