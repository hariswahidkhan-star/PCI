using System.Text.Json;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — deterministic grading and decision profiles (docs/pciworld/DATA_AND_CONTENT_MODEL.md §10–11).
///
/// Numeric asks are graded against SimCalc — the same relative-or-absolute tolerance convention as
/// the Simulation Lab (threshold = max(0.01, tol·|ref|)) so the two products can never disagree
/// about a number. Decisions are graded by the authored option-quality rubric. The decision
/// profile is a fixed rule over the dimension scores — explainable and reproducible, never random.
/// No model output grades anything.
/// </summary>
public static class WorldScore
{
    public sealed record MeasureRow(string Key, string Label, bool Correct, object Reference, object? Yours);
    public sealed record DecisionRow(string Key, string Prompt, string? Chosen, string? ChosenLabel,
        double Quality, string? Consequence, string? Principle, string BestLabel);
    public sealed record Result(
        double Score, double? Calculation, double? Decision, string ProfileKey, string ProfileReason,
        string ImprovementArea, List<MeasureRow> Measures, List<DecisionRow> Decisions);

    public static Result Grade(JsonElement config, JsonElement answers)
    {
        var measures = new List<MeasureRow>();
        var task = config.TryGetProperty("task", out var t) && t.ValueKind == JsonValueKind.String ? t.GetString()! : "";
        var relTol = config.TryGetProperty("tolerance", out var tol) && tol.ValueKind == JsonValueKind.Number ? tol.GetDouble() : 0.01;
        if (task.Length > 0 && config.TryGetProperty("given", out var given))
        {
            foreach (var ask in SimGrade.ParseAsk(config))
            {
                var reference = SimCalc.Resolve(task, ask.Key, given);
                JsonElement a = default;
                var has = answers.ValueKind == JsonValueKind.Object && answers.TryGetProperty(ask.Key, out a);
                var raw = !has ? "" : a.ValueKind switch
                {
                    JsonValueKind.String => a.GetString() ?? "",
                    JsonValueKind.Number => a.GetRawText(),
                    JsonValueKind.True => "yes",
                    JsonValueKind.False => "no",
                    _ => "",
                };
                switch (reference)
                {
                    case double want:
                    {
                        var got = ParseNumber(raw);
                        var correct = got is { } g && Math.Abs(g - want) <= Math.Max(0.01, relTol * Math.Abs(want));
                        measures.Add(new MeasureRow(ask.Key, ask.Label, correct, want, got ?? (object?)(raw.Length > 0 ? raw : null)));
                        break;
                    }
                    case string[] wantSet:
                    {
                        // Ordered set (e.g. the critical path): comma-separated, whitespace/case-insensitive.
                        var gotSet = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        var correct = gotSet.Length == wantSet.Length &&
                            gotSet.Zip(wantSet).All(p => string.Equals(p.First, p.Second, StringComparison.OrdinalIgnoreCase));
                        measures.Add(new MeasureRow(ask.Key, ask.Label, correct,
                            string.Join(",", wantSet), raw.Length > 0 ? raw : null));
                        break;
                    }
                    case bool wantBool:
                    {
                        // Yes/no judgement (e.g. "does the WBS satisfy the 100% rule?").
                        var got = ParseBool(raw);
                        measures.Add(new MeasureRow(ask.Key, ask.Label, got is { } b && b == wantBool,
                            wantBool ? "yes" : "no", got is { } g ? (g ? "yes" : "no") : (raw.Length > 0 ? raw : null)));
                        break;
                    }
                }
            }
        }

        var decisions = new List<DecisionRow>();
        if (config.TryGetProperty("decisions", out var decs) && decs.ValueKind == JsonValueKind.Array)
        {
            foreach (var d in decs.EnumerateArray())
            {
                var dk = Str(d, "key") ?? "";
                string? chosen = null;
                if (answers.ValueKind == JsonValueKind.Object && answers.TryGetProperty("decision_" + dk, out var c) &&
                    c.ValueKind == JsonValueKind.String) chosen = c.GetString();

                double quality = 0; string? chosenLabel = null, consequence = null, principle = null;
                double bestQ = -1; string bestLabel = "";
                if (d.TryGetProperty("options", out var opts) && opts.ValueKind == JsonValueKind.Array)
                    foreach (var o in opts.EnumerateArray())
                    {
                        var q = o.TryGetProperty("quality", out var qv) && qv.ValueKind == JsonValueKind.Number ? qv.GetDouble() : 0;
                        if (q > bestQ) { bestQ = q; bestLabel = Str(o, "label") ?? ""; }
                        if (Str(o, "key") == chosen)
                        { quality = q; chosenLabel = Str(o, "label"); consequence = Str(o, "consequence"); principle = Str(o, "principle"); }
                    }
                decisions.Add(new DecisionRow(dk, Str(d, "prompt") ?? "", chosen, chosenLabel, quality, consequence, principle, bestLabel));
            }
        }

        double? calc = measures.Count > 0 ? Math.Round(100.0 * measures.Count(m => m.Correct) / measures.Count, 1) : null;
        double? deci = decisions.Count > 0 ? Math.Round(decisions.Average(d => d.Quality), 1) : null;
        var weightN = measures.Count + decisions.Count;
        var score = weightN == 0 ? 0 :
            Math.Round(((calc ?? 0) * measures.Count + (deci ?? 0) * decisions.Count) / weightN, 1);

        var (profile, reason, improve) = Profile(config, calc, deci);
        return new Result(score, calc, deci, profile, reason, improve, measures, decisions);
    }

    /// <summary>Fixed profile rule: the dominant dimension names the profile via the authored
    /// profile_map (with defensible generic fallbacks); within 10 points the profile is the
    /// balanced identity. The weakest dimension names the improvement area.</summary>
    static (string profile, string reason, string improve) Profile(JsonElement config, double? calc, double? deci)
    {
        string Map(string key, string fallback) =>
            config.TryGetProperty("profile_map", out var pm) && pm.ValueKind == JsonValueKind.Object &&
            pm.TryGetProperty(key, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString()! : fallback;

        const string calcImprove = "quantitative accuracy — rework the calculations against the method reference";
        const string deciImprove = "decision judgement — weigh evidence and consequences before committing";
        if (calc is null && deci is null) return ("—", "No graded work was found in this attempt.", "—");
        if (deci is null || (calc is not null && calc - deci >= 10))
            return (Map("calculation", "Analytical Controller"),
                "Your calculation accuracy was the strongest dimension of this attempt.", deciImprove);
        if (calc is null || deci - calc >= 10)
            return (Map("decision", "Decisive Project Leader"),
                "Your decision quality was the strongest dimension of this attempt.", calcImprove);
        return (Map("balanced", "Evidence-Based Decision Maker"),
            "Your calculation accuracy and decision quality were evenly matched.",
            calc <= deci ? calcImprove : deciImprove);
    }

    /// <summary>Forgiving numeric entry: people type numbers the way the evidence shows them —
    /// "1,200,000", "1 200 000", "88.6%", "$450,000". Strip the formatting, keep the number.</summary>
    public static double? ParseNumber(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        var s = raw.Trim()
            .Replace(",", "").Replace(" ", "").Replace(" ", "").Replace("_", "").Replace("'", "")
            .Replace("$", "").Replace("£", "").Replace("€", "");
        if (s.EndsWith('%')) s = s[..^1];
        return double.TryParse(s, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : null;
    }

    /// <summary>Yes/no entry in the forms people actually type it.</summary>
    public static bool? ParseBool(string? raw) => raw?.Trim().ToLowerInvariant() switch
    {
        "yes" or "y" or "true" or "t" or "1" => true,
        "no" or "n" or "false" or "f" or "0" => false,
        _ => null,
    };

    static string? Str(JsonElement el, string key) =>
        el.ValueKind == JsonValueKind.Object && el.TryGetProperty(key, out var v) && v.ValueKind == JsonValueKind.String
            ? v.GetString() : null;
}
