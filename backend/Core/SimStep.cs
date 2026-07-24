using System.Text.Json;
using System.Text.Json.Nodes;

namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — deterministic multi-step scenario engine (Phase P1).
///
/// A single-task scenario is one prompt + one <c>given</c> + one <c>ask[]</c> (see <see cref="SimGrade"/>).
/// A multi-step scenario adds a <c>steps[]</c> array where each step is itself a gradable task, and steps
/// are LINKED: a step may carry a <c>decision</c> whose chosen option applies deterministic <c>effects</c>
/// to a LATER step's <c>given</c> (set / add / mul on a numeric input). The student's decisions therefore
/// change the numbers they must then compute downstream — a real project consequence, not a hidden answer.
///
/// Determinism: every answer is still derived server-side by <see cref="SimCalc.Resolve"/> from the
/// effective (post-effect) given, so grading, review and Coach reproduce identical results from the same
/// recorded (snapshot + decisions). Effects are scenario mechanics (shown to the learner), never the answer
/// key. Single-task scenarios are untouched — the whole engine is opt-in via the presence of <c>steps</c>.
/// </summary>
public static class SimStep
{
    public static bool IsMultiStep(JsonElement config) =>
        config.ValueKind == JsonValueKind.Object && config.TryGetProperty("steps", out var s)
        && s.ValueKind == JsonValueKind.Array && s.GetArrayLength() > 0;

    public sealed record Effect(string Step, string Path, string Op, double Value);
    public sealed record Option(string Value, string Label, IReadOnlyList<Effect> Effects);
    public sealed record Decision(string Key, string Prompt, IReadOnlyList<Option> Options);
    public sealed record StepDef(string Id, string Title, string Task, string Prompt, JsonElement Given,
        List<SimGrade.Ask> Ask, double Tolerance, IReadOnlyList<string> Competencies, Decision? Decision);

    public static List<StepDef> ParseSteps(JsonElement config)
    {
        var steps = new List<StepDef>();
        if (!config.TryGetProperty("steps", out var arr) || arr.ValueKind != JsonValueKind.Array) return steps;
        int i = 0;
        foreach (var st in arr.EnumerateArray())
        {
            i++;
            var id = st.TryGetProperty("id", out var idv) && idv.ValueKind == JsonValueKind.String ? idv.GetString()! : "s" + i;
            var title = st.TryGetProperty("title", out var t) ? t.GetString() ?? "" : "";
            var task = st.TryGetProperty("task", out var tk) ? tk.GetString() ?? "" : "";
            var prompt = st.TryGetProperty("prompt", out var pr) ? pr.GetString() ?? "" : "";
            var given = st.TryGetProperty("given", out var g) ? g : default;
            var tol = st.TryGetProperty("tolerance", out var to) && to.ValueKind == JsonValueKind.Number ? to.GetDouble() : 0.01;
            var comps = new List<string>();
            if (st.TryGetProperty("competencies", out var c) && c.ValueKind == JsonValueKind.Array)
                foreach (var e in c.EnumerateArray()) if (e.GetString() is { Length: > 0 } k) comps.Add(k);
            steps.Add(new StepDef(id, title, task, prompt, given, SimGrade.ParseAsk(st), tol, comps, ParseDecision(st)));
        }
        return steps;
    }

    static Decision? ParseDecision(JsonElement step)
    {
        if (!step.TryGetProperty("decision", out var d) || d.ValueKind != JsonValueKind.Object) return null;
        var key = d.TryGetProperty("key", out var k) ? k.GetString() ?? "" : "";
        if (key.Length == 0) return null;
        var prompt = d.TryGetProperty("prompt", out var p) ? p.GetString() ?? "" : "";
        var options = new List<Option>();
        if (d.TryGetProperty("options", out var opts) && opts.ValueKind == JsonValueKind.Array)
            foreach (var o in opts.EnumerateArray())
            {
                var val = o.TryGetProperty("value", out var v) ? v.GetString() ?? "" : "";
                if (val.Length == 0) continue;
                var label = o.TryGetProperty("label", out var l) ? l.GetString() ?? val : val;
                var effects = new List<Effect>();
                if (o.TryGetProperty("effects", out var efs) && efs.ValueKind == JsonValueKind.Array)
                    foreach (var ef in efs.EnumerateArray())
                    {
                        var estep = ef.TryGetProperty("step", out var es) ? es.GetString() ?? "" : "";
                        var path = ef.TryGetProperty("path", out var pa) ? pa.GetString() ?? "" : "";
                        var op = ef.TryGetProperty("op", out var op2) ? (op2.GetString() ?? "set") : "set";
                        var num = ef.TryGetProperty("value", out var nv) && nv.ValueKind == JsonValueKind.Number ? nv.GetDouble() : 0;
                        if (estep.Length > 0 && path.Length > 0) effects.Add(new Effect(estep, path, op, num));
                    }
                options.Add(new Option(val, label, effects));
            }
        return new Decision(key, prompt, options);
    }

    /// <summary>All effects that earlier decisions (as recorded in <paramref name="decisions"/>) apply to
    /// <paramref name="targetStepId"/>, in step order — deterministic given the recorded choices.</summary>
    static List<Effect> EffectsFor(List<StepDef> steps, string targetStepId, IReadOnlyDictionary<string, string> decisions)
    {
        var acc = new List<Effect>();
        foreach (var s in steps)
        {
            if (s.Decision is null) continue;
            if (!decisions.TryGetValue(s.Decision.Key, out var chosen)) continue;
            var opt = s.Decision.Options.FirstOrDefault(o => o.Value == chosen);
            if (opt is null) continue;
            foreach (var e in opt.Effects) if (e.Step == targetStepId) acc.Add(e);
        }
        return acc;
    }

    /// <summary>The step's given with all applicable decision effects applied (set / add / mul on a numeric
    /// field). Returns the base given unchanged when nothing targets the step.</summary>
    public static JsonElement EffectiveGiven(List<StepDef> steps, StepDef step, IReadOnlyDictionary<string, string> decisions)
    {
        var effects = EffectsFor(steps, step.Id, decisions);
        if (effects.Count == 0 || step.Given.ValueKind != JsonValueKind.Object) return step.Given;
        var node = JsonNode.Parse(step.Given.GetRawText())!.AsObject();
        foreach (var e in effects)
        {
            double cur = node.TryGetPropertyValue(e.Path, out var pv) && pv is not null && double.TryParse(pv.ToString(), out var c) ? c : 0;
            double next = e.Op switch { "add" => cur + e.Value, "mul" => cur * e.Value, _ => e.Value };
            node[e.Path] = next;
        }
        return JsonDocument.Parse(node.ToJsonString()).RootElement.Clone();
    }

    static IReadOnlyDictionary<string, string> ReadDecisions(JsonElement answers)
    {
        var map = new Dictionary<string, string>();
        if (answers.ValueKind == JsonValueKind.Object && answers.TryGetProperty("decisions", out var d) && d.ValueKind == JsonValueKind.Object)
            foreach (var p in d.EnumerateObject())
                map[p.Name] = p.Value.ValueKind == JsonValueKind.String ? p.Value.GetString() ?? "" : p.Value.ToString();
        return map;
    }

    static JsonElement StepAnswers(JsonElement answers, string stepId)
    {
        if (answers.ValueKind == JsonValueKind.Object && answers.TryGetProperty("steps", out var s)
            && s.ValueKind == JsonValueKind.Object && s.TryGetProperty(stepId, out var sa)) return sa;
        return default;
    }

    /// <summary>The student-facing multi-step descriptor: every step's prompt, base given, ask (answer-key
    /// free) and decision options with their (visible) effects, so the runner can show the consequence of a
    /// choice. The authoritative answers are never included — they are resolved server-side at grade time.</summary>
    public static object TaskFor(JsonElement config, string mode)
    {
        var steps = ParseSteps(config);
        return new
        {
            multistep = true,
            prompt = config.TryGetProperty("prompt", out var p) ? p.GetString() ?? "" : "",
            mode,
            assessment = mode == "assessment",
            steps = steps.Select(s => new
            {
                id = s.Id, title = s.Title, task = s.Task, prompt = s.Prompt,
                given = s.Given.ValueKind == JsonValueKind.Object ? (object)JsonSerializer.Deserialize<object>(s.Given.GetRawText())! : new { },
                ask = s.Ask.Select(a => new { key = a.Key, label = a.Label, type = a.Type }),
                decision = s.Decision is null ? null : (object)new
                {
                    key = s.Decision.Key, prompt = s.Decision.Prompt,
                    options = s.Decision.Options.Select(o => new
                    {
                        value = o.Value, label = o.Label,
                        effects = o.Effects.Select(e => new { step = e.Step, path = e.Path, op = e.Op, value = e.Value }),
                    }),
                },
            }),
        };
    }

    /// <summary>Grade a multi-step attempt deterministically. <paramref name="answers"/> carries
    /// <c>{ "steps": { stepId: { key: value } }, "decisions": { decisionKey: optionValue } }</c>. Each step
    /// is graded against the effective (post-decision-effect) given via <see cref="SimCalc.Resolve"/>; the
    /// overall score is the share of correct measures across every step. Competency evidence is emitted per
    /// step at that step's sub-score (averaged per competency), so mastery is fed by many pieces of evidence.
    /// </summary>
    public static SimGrade.GradeResult Grade(JsonElement config, JsonElement answers, string mode)
    {
        var steps = ParseSteps(config);
        var decisions = ReadDecisions(answers);
        var passPct = config.TryGetProperty("pass_pct", out var pp) && pp.ValueKind == JsonValueKind.Number ? pp.GetDouble() : 70;
        var reveal = mode != "assessment";

        var measures = new List<SimGrade.MeasureResult>();
        int correct = 0, total = 0;
        var compAcc = new Dictionary<string, (double sum, int n)>();

        foreach (var step in steps)
        {
            var given = EffectiveGiven(steps, step, decisions);
            var stepAns = StepAnswers(answers, step.Id);
            int stepCorrect = 0;
            foreach (var a in step.Ask)
            {
                total++;
                var answer = SimCalc.Resolve(step.Task, a.Key, given);
                var submitted = stepAns.ValueKind == JsonValueKind.Object && stepAns.TryGetProperty(a.Key, out var sv) ? (JsonElement?)sv : null;
                var (ok, your) = SimGrade.CompareAnswer(a.Type, answer, submitted, step.Tolerance);
                if (ok) { correct++; stepCorrect++; }
                measures.Add(new SimGrade.MeasureResult($"{step.Id}.{a.Key}", string.IsNullOrEmpty(step.Title) ? a.Label : $"{step.Title}: {a.Label}",
                    ok, reveal ? answer : null, reveal ? your : null));
            }
            var stepScore = step.Ask.Count == 0 ? 0 : 100.0 * stepCorrect / step.Ask.Count;
            foreach (var comp in step.Competencies)
            {
                var (sum, n) = compAcc.TryGetValue(comp, out var cur) ? cur : (0, 0);
                compAcc[comp] = (sum + stepScore, n + 1);
            }
        }

        var score = total == 0 ? 0 : Math.Round(100.0 * correct / total, 2);
        var comps = compAcc.Select(kv =>
        {
            var avg = Math.Round(kv.Value.sum / kv.Value.n, 2);
            return (kv.Key, avg, SimGrade.Level(avg));
        }).ToList();
        return new SimGrade.GradeResult(score, score >= passPct, correct, total, measures, comps);
    }
}
