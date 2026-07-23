using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab AI-Coach EVALS (Phase 4). These are behavioural assertions on the coach's grounding
/// contract, run on the deterministic (no-provider) path — the path CI actually exercises, and the same
/// contract the model path must honour when a provider key is present. They encode the spec's hard rules
/// as checks rather than prose:
///   • COVERAGE  — every task type produces a grounded explanation, never the generic fallback.
///   • GROUNDING — the coach cites the ENGINE's correct value, not the student's wrong one.
///   • NO HALLUCINATION — every scenario-specific number the coach prints traces to the deterministic
///     engine (given inputs, authoritative measures, or the score); no invented figures slip through.
///   • NO LEAK — in Assessment Mode the model context withholds the correct values entirely.
/// The cases below span all eleven task engines so a new engine cannot ship without coach grounding.
/// </summary>
public class SimCoachEvalTests
{
    static JsonElement J(string json) => JsonDocument.Parse(json).RootElement;

    // A representative (config, deliberately-wrong-answers) case per task type. Each wrong answer is a value
    // the engine will mark incorrect, so the coach must explain at least one missed measure.
    public static IEnumerable<object[]> AllTasks() => new[]
    {
        new object[] { "evm",
            "{\"task\":\"evm\",\"prompt\":\"p\",\"given\":{\"pv\":100000,\"ev\":90000,\"ac\":95000,\"bac\":200000},\"ask\":[{\"key\":\"spi\",\"label\":\"SPI\",\"type\":\"number\"},{\"key\":\"cpi\",\"label\":\"CPI\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"earned_value\"]}",
            "{\"spi\":9,\"cpi\":9}" },
        new object[] { "cpm",
            "{\"task\":\"cpm\",\"prompt\":\"p\",\"given\":{\"activities\":[{\"id\":\"A\",\"dur\":3,\"preds\":[]},{\"id\":\"B\",\"dur\":5,\"preds\":[\"A\"]}]},\"ask\":[{\"key\":\"project_duration\",\"label\":\"Duration\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"schedule_development\"]}",
            "{\"project_duration\":1}" },
        new object[] { "wbs",
            "{\"task\":\"wbs\",\"prompt\":\"p\",\"given\":{\"nodes\":[{\"id\":\"1\",\"parent\":null},{\"id\":\"1.1\",\"parent\":\"1\",\"value\":100},{\"id\":\"1.2\",\"parent\":\"1\",\"value\":200}]},\"ask\":[{\"key\":\"root_total\",\"label\":\"Root\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"scope_structuring\"]}",
            "{\"root_total\":1}" },
        new object[] { "cbs",
            "{\"task\":\"cbs\",\"prompt\":\"p\",\"given\":{\"nodes\":[{\"id\":\"1\",\"parent\":null},{\"id\":\"1.1\",\"parent\":\"1\",\"budget\":100,\"actual\":120}]},\"ask\":[{\"key\":\"root_variance\",\"label\":\"Variance\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"cost_control\"]}",
            "{\"root_variance\":999}" },
        new object[] { "progress",
            "{\"task\":\"progress\",\"prompt\":\"p\",\"given\":{\"nodes\":[{\"id\":\"1.1\",\"weight\":40000,\"percent\":100},{\"id\":\"1.2\",\"weight\":60000,\"percent\":0}]},\"ask\":[{\"key\":\"overall_percent\",\"label\":\"Overall\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"progress_measurement\"]}",
            "{\"overall_percent\":99}" },
        new object[] { "risk",
            "{\"task\":\"risk\",\"prompt\":\"p\",\"given\":{\"risks\":[{\"id\":\"R1\",\"probability\":0.3,\"impact\":-20000},{\"id\":\"R2\",\"probability\":0.2,\"impact\":15000}]},\"ask\":[{\"key\":\"emv\",\"label\":\"EMV\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"risk_management\"]}",
            "{\"emv\":999}" },
        new object[] { "pert",
            "{\"task\":\"pert\",\"prompt\":\"p\",\"given\":{\"activities\":[{\"o\":2,\"m\":4,\"p\":6},{\"o\":3,\"m\":5,\"p\":13}],\"deadline\":14},\"ask\":[{\"key\":\"expected_duration\",\"label\":\"Expected\",\"type\":\"number\"}],\"tolerance\":0.02,\"pass_pct\":70,\"competencies\":[\"schedule_analysis\"]}",
            "{\"expected_duration\":99}" },
        new object[] { "change",
            "{\"task\":\"change\",\"prompt\":\"p\",\"given\":{\"baseline_bac\":500000,\"baseline_duration\":100,\"changes\":[{\"id\":\"C1\",\"status\":\"approved\",\"cost_delta\":30000,\"schedule_delta\":5}]},\"ask\":[{\"key\":\"revised_bac\",\"label\":\"Revised BAC\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"change_control\"]}",
            "{\"revised_bac\":1}" },
        new object[] { "cashflow",
            "{\"task\":\"cashflow\",\"prompt\":\"p\",\"given\":{\"periods\":[{\"period\":1,\"inflow\":0,\"outflow\":50000},{\"period\":2,\"inflow\":120000,\"outflow\":30000}]},\"ask\":[{\"key\":\"peak_funding\",\"label\":\"Peak funding\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"cash_flow\"]}",
            "{\"peak_funding\":1}" },
        new object[] { "timeline",
            "{\"task\":\"timeline\",\"prompt\":\"p\",\"given\":{\"bac\":600000,\"series\":[{\"period\":1,\"pv\":100000,\"ev\":90000,\"ac\":100000},{\"period\":2,\"pv\":220000,\"ev\":200000,\"ac\":230000}]},\"ask\":[{\"key\":\"final_cpi\",\"label\":\"Final CPI\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"earned_value\"]}",
            "{\"final_cpi\":9}" },
        new object[] { "earned_schedule",
            "{\"task\":\"earned_schedule\",\"prompt\":\"p\",\"given\":{\"planned_duration\":6,\"at\":4,\"ev\":500,\"plan\":[{\"period\":1,\"pv\":100},{\"period\":2,\"pv\":250},{\"period\":3,\"pv\":450},{\"period\":4,\"pv\":650}]},\"ask\":[{\"key\":\"spi_time\",\"label\":\"SPI(t)\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"schedule_analysis\"]}",
            "{\"spi_time\":9}" },
    };

    [Theory]
    [MemberData(nameof(AllTasks))]
    public void Eval_EveryTaskGetsAGroundedExplanation_NotTheGenericFallback(string task, string configJson, string answersJson)
    {
        var config = J(configJson);
        var grade = SimGrade.Grade(config, J(answersJson), "training");
        var msg = SimCoach.DeterministicCoach(config, grade, null);

        Assert.False(string.IsNullOrWhiteSpace(msg));
        // The generic fallback means this task/measure had no grounded concept — a coverage gap.
        Assert.DoesNotContain("Revisit the definition of this measure", msg);
        Assert.Equal(task, config.GetProperty("task").GetString());   // sanity: the case is the task it claims
    }

    [Theory]
    [MemberData(nameof(AllTasks))]
    public void Eval_NoUngroundedScenarioNumberIsEmitted(string task, string configJson, string answersJson)
    {
        _ = task;
        var config = J(configJson);
        var grade = SimGrade.Grade(config, J(answersJson), "training");
        var msg = SimCoach.DeterministicCoach(config, grade, null);

        // Grounded values: the score, the correct/total counts, and every authoritative correct value the
        // coach is allowed to cite (rendered exactly as the coach renders them). Plus a small set of
        // pedagogical constants that appear in concept formulae ("EV ÷ PV", "(O + 4M + P) ÷ 6", "100% rule").
        var grounded = new HashSet<double> { grade.Score, grade.Correct, grade.Total, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 100 };
        foreach (var m in grade.Measures)
        {
            AddGrounded(grounded, m.Correct_Value);
            AddGrounded(grounded, m.Your_Value);
        }

        foreach (Match tok in Regex.Matches(msg, @"-?\d+(?:\.\d+)?"))
        {
            var n = double.Parse(tok.Value, CultureInfo.InvariantCulture);
            Assert.True(grounded.Any(g => Math.Abs(g - n) <= 0.001),
                $"[{task}] coach printed an ungrounded number {n}: \"{msg}\"");
        }
    }

    static void AddGrounded(HashSet<double> set, object? v)
    {
        if (v is double d)
        {
            set.Add(d);
            // The coach prints via a 2-dp / integer format; add that rendered form so the token matches exactly.
            set.Add(double.Parse(Fmt(d), CultureInfo.InvariantCulture));
        }
        else if (v is string[] arr)
        {
            foreach (var s in arr)
                if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var n)) set.Add(n);
        }
    }

    static string Fmt(double d) => d == Math.Floor(d) ? ((long)d).ToString(CultureInfo.InvariantCulture)
        : Math.Round(d, 2).ToString(CultureInfo.InvariantCulture);

    [Fact]
    public void Eval_CoachCitesTheEnginesValue_NotTheStudentsWrongOne()
    {
        var config = J("{\"task\":\"evm\",\"prompt\":\"p\",\"given\":{\"pv\":100000,\"ev\":90000,\"ac\":95000,\"bac\":200000}," +
            "\"ask\":[{\"key\":\"cpi\",\"label\":\"Cost Performance Index (CPI)\",\"type\":\"number\"}]," +
            "\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"earned_value\"]}");
        var grade = SimGrade.Grade(config, J("{\"cpi\":2.0}"), "training");
        var msg = SimCoach.DeterministicCoach(config, grade, null);

        Assert.Contains("0.95", msg);          // the engine's CPI (90000/95000 = 0.9474 → 0.95)
        Assert.DoesNotContain("2.0", msg);     // never echoes the student's wrong figure as the answer
    }

    [Fact]
    public void Eval_AssessmentModeContext_WithholdsTheCorrectValues()
    {
        var config = J("{\"task\":\"evm\",\"prompt\":\"p\",\"given\":{\"pv\":100000,\"ev\":90000,\"ac\":95000,\"bac\":200000}," +
            "\"ask\":[{\"key\":\"cpi\",\"label\":\"CPI\",\"type\":\"number\"}],\"tolerance\":0.01,\"pass_pct\":70,\"competencies\":[\"earned_value\"]}");
        var grade = SimGrade.Grade(config, J("{\"cpi\":2.0}"), "assessment");
        var ctx = SimCoach.BuildContext(config, grade, null);

        // Assessment Mode strips Correct_Value/Your_Value from the grade, so the model context cannot leak them.
        Assert.Contains("correct = —", ctx);
        Assert.DoesNotContain("0.95", ctx);
    }
}
