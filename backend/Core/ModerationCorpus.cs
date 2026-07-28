namespace PCI.Backend.Core;

/// <summary>
/// Reading a labelled benchmark corpus off disk (CCP-P1-004, §8.4.1, §22).
///
/// The point of this file is to make the corpus a THING TRUST &amp; SAFETY DROPS IN rather than a
/// system somebody has to build. The format is deliberately boring — one JSON object per line — so
/// it can be produced by a spreadsheet export, a labelling tool, or a person with a text editor,
/// and reviewed in a diff.
///
///   {"text":"…","locale":"ar","category":"hate","harmful":true,"obfuscated":false}
///
/// STRICT BY DEFAULT, BECAUSE A CORPUS THAT SILENTLY LOSES ROWS IS WORSE THAN ONE THAT FAILS.
///
/// A malformed line is an error carrying its line number, not a row quietly dropped. Half a corpus
/// that looks whole would produce a calibration report over data nobody intended, and the report
/// would look exactly as convincing as a correct one. <see cref="Load"/> therefore returns every
/// problem it found rather than the first, so fixing a corpus is one pass.
///
/// An unknown category is an error too. A typo like "hatespeech" would otherwise become a category
/// with no configured thresholds — which the calibration gate correctly refuses to score, but the
/// person would see "no category was scored" and have no idea why.
///
/// WHAT THIS FILE IS NOT.
///
/// It is not a corpus, and this repository does not ship one. Authoring and labelling real examples
/// across English, Arabic, Urdu, code-switched and obfuscated text is Trust &amp; Safety's work, and
/// synthesising labelled harmful text here would produce a benchmark that measures nothing while
/// looking like it measures something.
/// </summary>
public static class ModerationCorpus
{
    public readonly record struct LoadResult(
        IReadOnlyList<ModerationCalibration.Sample> Samples,
        IReadOnlyList<string> Problems)
    {
        public bool Ok => Problems.Count == 0;
    }

    /// <summary>
    /// Parse JSON-lines corpus content. Blank lines and lines beginning with <c>#</c> are ignored,
    /// so a corpus can carry comments explaining a labelling decision — which is exactly the kind of
    /// context that is lost when a reviewer moves on.
    /// </summary>
    public static LoadResult Load(string content)
    {
        var samples = new List<ModerationCalibration.Sample>();
        var problems = new List<string>();
        var known = new HashSet<string>(CommunityModeration.Categories.All, StringComparer.OrdinalIgnoreCase);

        var lineNo = 0;
        foreach (var raw in content.Split('\n'))
        {
            lineNo++;
            var line = raw.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;

            System.Text.Json.JsonElement el;
            try { el = System.Text.Json.JsonDocument.Parse(line).RootElement; }
            catch (Exception e) { problems.Add($"line {lineNo}: not valid JSON ({e.Message})"); continue; }

            var text = Str(el, "text");
            var locale = Str(el, "locale");
            var category = Str(el, "category");

            // Each missing field is reported separately: "line 12 is wrong" sends someone hunting,
            // "line 12 has no locale" does not.
            if (string.IsNullOrWhiteSpace(text)) problems.Add($"line {lineNo}: 'text' is missing or empty");
            if (string.IsNullOrWhiteSpace(locale)) problems.Add($"line {lineNo}: 'locale' is missing (§22 coverage cannot be checked without it)");
            if (string.IsNullOrWhiteSpace(category)) problems.Add($"line {lineNo}: 'category' is missing");
            else if (!known.Contains(category))
                problems.Add($"line {lineNo}: unknown category '{category}' — a typo here becomes a category the gate cannot score");

            if (!el.TryGetProperty("harmful", out var h)
                || (h.ValueKind != System.Text.Json.JsonValueKind.True && h.ValueKind != System.Text.Json.JsonValueKind.False))
            {
                // Defaulting this would invent a human label, which is the one thing a corpus loader
                // must never do — the label IS the ground truth.
                problems.Add($"line {lineNo}: 'harmful' must be true or false (it is the human label, and cannot be defaulted)");
                continue;
            }

            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(locale)
                || string.IsNullOrWhiteSpace(category) || !known.Contains(category ?? "")) continue;

            var obfuscated = el.TryGetProperty("obfuscated", out var o)
                             && o.ValueKind == System.Text.Json.JsonValueKind.True;

            samples.Add(new ModerationCalibration.Sample(
                text!, locale!, category!, h.ValueKind == System.Text.Json.JsonValueKind.True, obfuscated));
        }

        if (samples.Count == 0 && problems.Count == 0)
            problems.Add("corpus contains no samples");

        return new LoadResult(samples, problems);
    }

    static string? Str(System.Text.Json.JsonElement el, string name) =>
        el.TryGetProperty(name, out var v) && v.ValueKind == System.Text.Json.JsonValueKind.String
            ? v.GetString() : null;

    /// <summary>
    /// A human-readable summary of what a corpus covers, for the person assembling it. Reports the
    /// shape only — it says nothing about whether the labels are correct, which no program can.
    /// </summary>
    public static string Describe(IReadOnlyList<ModerationCalibration.Sample> samples)
    {
        if (samples.Count == 0) return "empty corpus";
        var byLocale = samples.GroupBy(s => Primary(s.Locale)).OrderBy(g => g.Key)
            .Select(g => $"{g.Key}={g.Count()}");
        var byCategory = samples.GroupBy(s => s.Category, StringComparer.OrdinalIgnoreCase).OrderBy(g => g.Key)
            .Select(g => $"{g.Key}={g.Count()}");
        return $"{samples.Count} samples | locales: {string.Join(", ", byLocale)} "
             + $"| categories: {string.Join(", ", byCategory)} "
             + $"| obfuscated: {samples.Count(s => s.Obfuscated)} "
             + $"| labelled harmful: {samples.Count(s => s.ExpectedHarmful)}";
    }

    static string Primary(string? locale)
    {
        var l = (locale ?? "").Trim();
        var dash = l.IndexOfAny(new[] { '-', '_' });
        return (dash > 0 ? l[..dash] : l).ToLowerInvariant();
    }
}
