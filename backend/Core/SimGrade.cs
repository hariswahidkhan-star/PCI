using System.Text.Json;

namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — task shaping + deterministic grading (Phase 1).
///
/// A scenario's <c>config_json</c> defines an applied task: an engine selector (evm | cpm | wbs), a prompt,
/// the synthetic <c>given</c> inputs, and the list of measures the student must compute (<c>ask</c>). This
/// layer turns that into (a) a student-facing task descriptor that NEVER carries the answer key, and (b) a
/// grade computed by comparing the student's submitted values to the authoritative answers that
/// <see cref="SimCalc.Resolve"/> derives from the same inputs — one source of truth, no arithmetic here.
///
/// Mode matters: in Assessment Mode the grade reports only right/wrong (never the correct value or an
/// explanation), honouring the spec's "must not expose answers in Assessment Mode". Training / challenge /
/// sandbox modes return the correct value and teaching feedback so the Lab can coach.
/// </summary>
public static class SimGrade
{
    public sealed record Ask(string Key, string Label, string Type);

    public sealed record MeasureResult(string Key, string Label, bool Correct, object? Correct_Value, object? Your_Value);

    public sealed record GradeResult(
        double Score, bool Passed, int Correct, int Total,
        IReadOnlyList<MeasureResult> Measures,
        IReadOnlyList<(string competency, double score, string level)> Competencies);

    static bool RevealsAnswers(string mode) => mode != "assessment";

    /// <summary>Parse the <c>ask</c> array into typed descriptors (defaults to a numeric measure).</summary>
    public static List<Ask> ParseAsk(JsonElement config)
    {
        var list = new List<Ask>();
        if (config.TryGetProperty("ask", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var a in arr.EnumerateArray())
            {
                var key = a.TryGetProperty("key", out var k) ? k.GetString() ?? "" : "";
                if (key.Length == 0) continue;
                var label = a.TryGetProperty("label", out var l) ? l.GetString() ?? key : key;
                var type = a.TryGetProperty("type", out var t) ? t.GetString() ?? "number" : "number";
                list.Add(new Ask(key, label, type));
            }
        return list;
    }

    /// <summary>
    /// The student-facing task descriptor: the prompt, the given inputs, and what to compute — with the
    /// answer key deliberately absent (answers live only on the server, resolved at grade time).
    /// </summary>
    public static object TaskFor(JsonElement config, string mode)
    {
        if (SimStep.IsMultiStep(config)) return SimStep.TaskFor(config, mode);   // linked multi-step scenario
        var task = config.TryGetProperty("task", out var tk) ? tk.GetString() ?? "" : "";
        var prompt = config.TryGetProperty("prompt", out var pr) ? pr.GetString() ?? "" : "";
        var given = config.TryGetProperty("given", out var g) ? (JsonElement?)g : null;
        var ask = ParseAsk(config).Select(a => new { key = a.Key, label = a.Label, type = a.Type });
        // A schedule-risk task whose given carries seed + iterations gets the Monte-Carlo distribution
        // attached for the histogram dashboard. This is analysis OUTPUT computed from the given inputs (not
        // an answer key) — it shows the real distribution beside the PERT normal approximation.
        object? montecarlo = given is JsonElement mg ? McPayload(SimCalc.MonteCarloFrom(mg)) : null;
        return new
        {
            task,
            prompt,
            given = given is JsonElement ge ? (object)JsonSerializer.Deserialize<object>(ge.GetRawText())! : new { },
            ask,
            mode,
            assessment = !RevealsAnswers(mode),   // the UI hides teaching hints when true
            montecarlo,
        };
    }

    static object? McPayload(SimCalc.McResult? r) => r is null ? null : new
    {
        iterations = r.Iterations, mean = r.Mean, min = r.Min, max = r.Max,
        p10 = r.P10, p50 = r.P50, p80 = r.P80, p90 = r.P90,
        histogram = r.Histogram.Select(b => new { lo = b.Lo, hi = b.Hi, count = b.Count }),
    };

    /// <summary>
    /// Grade the submitted answers deterministically. <paramref name="answers"/> is a JSON object keyed by
    /// measure key. Numeric measures pass within a relative-or-absolute tolerance; set measures
    /// (critical path) match as unordered, case-insensitive sets; boolean measures match true/false.
    /// </summary>
    public static GradeResult Grade(JsonElement config, JsonElement answers, string mode)
    {
        if (SimStep.IsMultiStep(config)) return SimStep.Grade(config, answers, mode);   // linked multi-step scenario
        var task = config.TryGetProperty("task", out var tk) ? tk.GetString() ?? "" : "";
        var given = config.TryGetProperty("given", out var g) ? g : default;
        var ask = ParseAsk(config);
        var relTol = config.TryGetProperty("tolerance", out var tol) && tol.ValueKind == JsonValueKind.Number ? tol.GetDouble() : 0.01;
        var passPct = config.TryGetProperty("pass_pct", out var pp) && pp.ValueKind == JsonValueKind.Number ? pp.GetDouble() : 70;
        var reveal = RevealsAnswers(mode);

        var results = new List<MeasureResult>();
        var correctCount = 0;
        foreach (var a in ask)
        {
            var answer = SimCalc.Resolve(task, a.Key, given);
            var submitted = answers.ValueKind == JsonValueKind.Object && answers.TryGetProperty(a.Key, out var sv) ? (JsonElement?)sv : null;
            var (ok, yourVal) = CompareAnswer(a.Type, answer, submitted, relTol);
            if (ok) correctCount++;
            results.Add(new MeasureResult(a.Key, a.Label, ok,
                reveal ? answer : null,           // Assessment Mode: never surface the correct value
                reveal ? yourVal : null));
        }

        var total = ask.Count;
        var score = total == 0 ? 0 : Math.Round(100.0 * correctCount / total, 2);
        var passed = score >= passPct;

        // Competency evidence: each listed competency is scored at this attempt's score (foundation labs
        // exercise a single competency). Mastery LEVEL is derived here but, per §25.4, a durable mastery
        // claim is only ever made from many pieces of evidence in the aggregation layer — never one score.
        var comps = new List<(string, double, string)>();
        if (config.TryGetProperty("competencies", out var carr) && carr.ValueKind == JsonValueKind.Array)
            foreach (var c in carr.EnumerateArray())
                if (c.GetString() is { Length: > 0 } key)
                    comps.Add((key, score, Level(score)));

        return new GradeResult(score, passed, correctCount, total, results, comps);
    }

    /// <summary>Provisional mastery band for a single score (introduced → advanced).</summary>
    public static string Level(double score) => score switch
    {
        >= 90 => "advanced",
        >= 75 => "proficient",
        >= 60 => "competent",
        >= 40 => "developing",
        _ => "introduced",
    };

    /// <summary>Compare one submitted measure to its authoritative answer (numeric tolerance, unordered set,
    /// or boolean). Public so the multi-step engine (<see cref="SimStep"/>) grades each step's measures with
    /// the identical rule.</summary>
    public static (bool ok, object? your) CompareAnswer(string type, object? answer, JsonElement? submitted, double relTol)
    {
        if (submitted is not JsonElement s) return (false, null);
        switch (type)
        {
            case "set":
            {
                var want = (answer as string[]) ?? Array.Empty<string>();
                var got = s.ValueKind == JsonValueKind.Array
                    ? s.EnumerateArray().Select(e => e.GetString() ?? e.ToString()).ToArray()
                    : (s.ValueKind == JsonValueKind.String ? SplitList(s.GetString()) : Array.Empty<string>());
                var wantSet = new HashSet<string>(want.Select(Norm));
                var gotSet = new HashSet<string>(got.Select(Norm));
                return (wantSet.SetEquals(gotSet), got);
            }
            case "bool":
            {
                bool? want = answer as bool?;
                bool? got = s.ValueKind switch
                {
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    JsonValueKind.String => ParseBool(s.GetString()),
                    _ => null,
                };
                return (want is not null && got is not null && want == got, got);
            }
            default: // "number"
            {
                if (answer is not double want) return (false, null);
                double? got = s.ValueKind == JsonValueKind.Number && s.TryGetDouble(out var n) ? n
                    : s.ValueKind == JsonValueKind.String && double.TryParse(s.GetString(), out var n2) ? n2
                    : (double?)null;
                if (got is not double gv) return (false, null);
                var threshold = Math.Max(0.01, relTol * Math.Abs(want));
                return (Math.Abs(gv - want) <= threshold, gv);
            }
        }
    }

    static string Norm(string s) => (s ?? "").Trim().ToUpperInvariant();
    static string[] SplitList(string? s) => (s ?? "").Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
    static bool? ParseBool(string? s) => (s ?? "").Trim().ToLowerInvariant() switch
    {
        "true" or "yes" or "y" or "1" or "valid" => true,
        "false" or "no" or "n" or "0" or "invalid" => false,
        _ => null,
    };
}
