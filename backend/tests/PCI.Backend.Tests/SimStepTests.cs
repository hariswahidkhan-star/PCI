using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — P1 multi-step engine tests. These pin the linked-scenario guarantees: steps are graded
/// deterministically from the effective (post-decision-effect) given via the same SimCalc source of truth;
/// a learner's decision changes the numbers they must then compute downstream; the validator reference-solves
/// every task under every decision branch; and single-task scenarios are completely unaffected. The seeded
/// Moderate scenario (MS-RECOVERY-001) is exercised end-to-end so a real published multi-step lab is proven
/// gradable to 100.
/// </summary>
public class SimStepTests
{
    const string Config = """
        {"multistep":true,"prompt":"linked recovery","pass_pct":70,
         "competencies":["earned_value","forecasting","decision_making"],
         "steps":[
           {"id":"s1","title":"Baseline","task":"evm","competencies":["earned_value"],
            "given":{"pv":240000,"ev":210000,"ac":250000,"bac":600000},"tolerance":0.01,
            "ask":[{"key":"sv","label":"SV","type":"number"},{"key":"cpi","label":"CPI","type":"number"}]},
           {"id":"s2","title":"Recover","task":"evm","competencies":["decision_making"],
            "given":{"pv":240000,"ev":210000,"ac":250000,"bac":600000},"tolerance":0.01,
            "ask":[{"key":"cv","label":"CV","type":"number"}],
            "decision":{"key":"recovery","prompt":"path?","options":[
              {"value":"crash","label":"crash","effects":[{"step":"s3","path":"ac","op":"add","value":60000}]},
              {"value":"descope","label":"descope","effects":[{"step":"s3","path":"bac","op":"add","value":-50000}]}]}},
           {"id":"s3","title":"Forecast","task":"evm","competencies":["forecasting"],
            "given":{"pv":300000,"ev":260000,"ac":300000,"bac":600000},"tolerance":0.01,
            "ask":[{"key":"eac","label":"EAC","type":"number"},{"key":"vac","label":"VAC","type":"number"},{"key":"tcpi","label":"TCPI","type":"number"}]}]}
        """;

    static JsonElement Parse(string s) => JsonDocument.Parse(s).RootElement;

    // Build a fully-correct answer set by resolving every measure on each step's effective given — the exact
    // deterministic answers the engine expects, proving "engine-derived answers → full marks".
    static JsonElement CorrectAnswers(string configJson, Dictionary<string, string> decisions)
    {
        var config = Parse(configJson);
        var steps = SimStep.ParseSteps(config);
        var stepMap = new Dictionary<string, Dictionary<string, object?>>();
        foreach (var s in steps)
        {
            var given = SimStep.EffectiveGiven(steps, s, decisions);
            var m = new Dictionary<string, object?>();
            foreach (var a in s.Ask) m[a.Key] = SimCalc.Resolve(s.Task, a.Key, given);
            stepMap[s.Id] = m;
        }
        var root = new Dictionary<string, object?> { ["steps"] = stepMap, ["decisions"] = decisions };
        return JsonDocument.Parse(JsonSerializer.Serialize(root)).RootElement.Clone();
    }

    [Fact]
    public void Engine_derived_answers_score_full_marks()
    {
        var decisions = new Dictionary<string, string> { ["recovery"] = "crash" };
        var g = SimGrade.Grade(Parse(Config), CorrectAnswers(Config, decisions), "training");
        Assert.Equal(6, g.Total);          // 2 + 1 + 3 measures across three steps
        Assert.Equal(100, g.Score);
        Assert.True(g.Passed);
    }

    [Fact]
    public void Decision_changes_the_downstream_correct_answer()
    {
        // Answers computed for CRASH; graded under DESCOPE the downstream forecast is now wrong.
        var crashAnswers = CorrectAnswers(Config, new() { ["recovery"] = "crash" });
        var underCrash = SimGrade.Grade(Parse(Config), crashAnswers, "training");
        Assert.Equal(100, underCrash.Score);

        // Same submitted numbers, but the recorded decision is descope → s3 (eac/vac) no longer matches.
        var mismatched = ReplaceDecision(crashAnswers, "recovery", "descope");
        var underDescope = SimGrade.Grade(Parse(Config), mismatched, "training");
        Assert.True(underDescope.Score < 100);
        Assert.Contains(underDescope.Measures, m => m.Key == "s3.eac" && !m.Correct);
    }

    [Fact]
    public void EffectiveGiven_applies_decision_effects_deterministically()
    {
        var steps = SimStep.ParseSteps(Parse(Config));
        var s3 = steps.First(s => s.Id == "s3");
        var crash = SimStep.EffectiveGiven(steps, s3, new Dictionary<string, string> { ["recovery"] = "crash" });
        var descope = SimStep.EffectiveGiven(steps, s3, new Dictionary<string, string> { ["recovery"] = "descope" });
        var baseline = SimStep.EffectiveGiven(steps, s3, new Dictionary<string, string>());
        Assert.Equal(360000, crash.GetProperty("ac").GetDouble());       // 300000 + 60000
        Assert.Equal(300000, baseline.GetProperty("ac").GetDouble());     // untouched
        Assert.Equal(550000, descope.GetProperty("bac").GetDouble());     // 600000 - 50000
    }

    [Fact]
    public void Assessment_mode_never_reveals_correct_values()
    {
        var g = SimGrade.Grade(Parse(Config), CorrectAnswers(Config, new() { ["recovery"] = "crash" }), "assessment");
        Assert.All(g.Measures, m => Assert.Null(m.Correct_Value));
    }

    [Fact]
    public void Single_task_scenario_is_not_treated_as_multistep()
    {
        var single = Parse("""{"task":"evm","given":{"pv":1,"ev":1,"ac":1,"bac":2},"ask":[{"key":"cpi","type":"number"}]}""");
        Assert.False(SimStep.IsMultiStep(single));
    }

    [Fact]
    public void Validator_accepts_a_sound_multistep_and_rejects_an_ungradable_branch()
    {
        var okIssues = SimContent.Validate(Input(Config));
        Assert.True(SimContent.Publishable(okIssues));

        // A step that asks for a measure the engine cannot resolve has no answer key → not publishable.
        var broken = Config.Replace("""{"key":"eac","label":"EAC","type":"number"}""",
                                    """{"key":"not_a_measure","label":"X","type":"number"}""");
        var badIssues = SimContent.Validate(Input(broken));
        Assert.False(SimContent.Publishable(badIssues));
    }

    [Fact]
    public void Seeded_recovery_scenario_is_published_valid_and_gradable_to_100()
    {
        var db = TestEnv.NewMigratedDb();
        SimLabSchema.Ensure(db);
        var s = db.QueryOne("SELECT config_json,status,synthetic_declared FROM simulation_scenarios WHERE scenario_code=?", "MS-RECOVERY-001");
        Assert.NotNull(s);
        Assert.Equal("published", H.Str(s!["status"]));
        var cfg = H.Str(s["config_json"])!;
        Assert.True(SimStep.IsMultiStep(Parse(cfg)));
        Assert.True(SimContent.Publishable(SimContent.Validate(Input(cfg,
            "[\"earned_value\",\"forecasting\",\"decision_making\"]"))));

        var g = SimGrade.Grade(Parse(cfg), CorrectAnswers(cfg, new() { ["recovery"] = "crash", ["funding"] = "request" }), "training");
        Assert.Equal(9, g.Total);          // 3 + 1 + 3 + 2 measures
        Assert.Equal(100, g.Score);
    }

    static SimContent.ScenarioInput Input(string config, string competencies = "[\"earned_value\",\"forecasting\",\"decision_making\"]") =>
        new("MS-TEST-001", "Recovery multi-step", "A synthetic linked EVM recovery scenario.", "intermediate", 1, competencies, config, SyntheticDeclared: true);

    static JsonElement ReplaceDecision(JsonElement answers, string key, string value)
    {
        var node = System.Text.Json.Nodes.JsonNode.Parse(answers.GetRawText())!.AsObject();
        node["decisions"]!.AsObject()[key] = value;
        return JsonDocument.Parse(node.ToJsonString()).RootElement.Clone();
    }
}
