using System.Globalization;
using System.Text.Json;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — challenge content model and publication validator (docs/pciworld/DATA_AND_CONTENT_MODEL.md).
///
/// A challenge config carries: `context` + `evidence` (what the participant sees), an optional
/// deterministic numeric block (`task`/`given`/`ask`/`tolerance` — resolved by SimCalc, the same
/// engine the Simulation Lab grades with), and `decisions` (judgement calls scored by an authored
/// option-quality rubric). The validator is the publication gate: nothing reaches
/// pciworld_challenge_versions unless every ask resolves, every decision is well-formed and no
/// reference value leaks into participant-visible text. PublicView is the ONLY shape served to the
/// workspace — an allow-list, so qualities, consequences and reference values cannot leak by
/// accident.
/// </summary>
public static class WorldContent
{
    public enum Severity { Error, Warning }
    public sealed record Issue(Severity Sev, string Code, string Message);

    public static readonly string[] Difficulties = { "foundation", "developing", "professional", "advanced", "expert" };
    public static readonly string[] Tracks = { "project_controls", "project_management", "project_finance", "governed_ai", "cross_functional" };

    public sealed record ChallengeInput(
        string Code, string? Title, string? Hook, string? Industry, string? Track, string? Difficulty,
        long Minutes, bool SyntheticDeclared, string? ConfigJson);

    public static bool Publishable(IEnumerable<Issue> issues) => !issues.Any(i => i.Sev == Severity.Error);

    public static List<Issue> Validate(ChallengeInput c)
    {
        var issues = new List<Issue>();
        void Err(string code, string msg) => issues.Add(new Issue(Severity.Error, code, msg));
        void Warn(string code, string msg) => issues.Add(new Issue(Severity.Warning, code, msg));

        if (string.IsNullOrWhiteSpace(c.Code) || c.Code.Trim().Length < 4) Err("code", "Challenge code is required (min 4 chars).");
        if (string.IsNullOrWhiteSpace(c.Title)) Err("title", "Title is required.");
        if (!Difficulties.Contains(c.Difficulty ?? "")) Err("difficulty", $"Difficulty must be one of: {string.Join(", ", Difficulties)}.");
        if (!Tracks.Contains(c.Track ?? "")) Err("track", $"Track must be one of: {string.Join(", ", Tracks)}.");
        if (string.IsNullOrWhiteSpace(c.Industry)) Warn("industry", "Industry is recommended for catalogue coverage.");
        if (c.Minutes <= 0) Err("minutes", "Estimated minutes must be positive.");
        if (!c.SyntheticDeclared) Err("synthetic", "The synthetic-data declaration is required — PCI World never publishes real project data.");
        if (string.IsNullOrWhiteSpace(c.ConfigJson)) { Err("config", "config_json is required."); return issues; }

        JsonElement root;
        try { root = JsonDocument.Parse(c.ConfigJson!).RootElement; }
        catch (JsonException e) { Err("json", $"config_json is not valid JSON: {e.Message}"); return issues; }

        var context = Str(root, "context");
        if (string.IsNullOrWhiteSpace(context)) Err("context", "A project context is required — the participant must see the situation.");
        if (string.IsNullOrWhiteSpace(Str(root, "share_line"))) Err("share", "A factual share_line is required for the result card.");

        // Participant-visible text, used below for the answer-leakage scan.
        var visible = new System.Text.StringBuilder(context ?? "");
        if (root.TryGetProperty("evidence", out var ev) && ev.ValueKind == JsonValueKind.Array)
            foreach (var e in ev.EnumerateArray())
            { visible.Append(' ').Append(Str(e, "label")).Append(' ').Append(Str(e, "value")); }

        var askCount = 0;
        if (root.TryGetProperty("task", out var taskEl) || root.TryGetProperty("ask", out _))
        {
            var task = taskEl.ValueKind == JsonValueKind.String ? taskEl.GetString() ?? "" : "";
            if (!SimCalc.KnownTasks.Contains(task))
                Err("task", $"Task '{task}' is not one the deterministic engine can grade.");
            else if (!root.TryGetProperty("given", out var given))
                Err("given", "A numeric task requires `given` inputs.");
            else
            {
                var tol = root.TryGetProperty("tolerance", out var t) && t.ValueKind == JsonValueKind.Number ? t.GetDouble() : 0;
                if (tol <= 0) Err("tolerance", "A positive tolerance is required for numeric asks.");
                foreach (var ask in SimGrade.ParseAsk(root))
                {
                    askCount++;
                    var answer = SimCalc.Resolve(task, ask.Key, given);
                    if (answer is null) { Err("ask", $"Ask '{ask.Key}' does not resolve through the reference solver."); continue; }
                    // The declared type decides which input control the participant gets and which
                    // grader runs — it must exist and it must match what the solver returns, or the
                    // question ships un-answerable (e.g. a yes/no behind a numeric keypad).
                    var declared = string.IsNullOrWhiteSpace(ask.Type) ? "number" : ask.Type;
                    var expected = answer switch { double => "number", string[] => "set", bool => "bool", _ => "?" };
                    if (declared is not ("number" or "set" or "bool"))
                        Err("ask_type", $"Ask '{ask.Key}' declares unsupported type '{declared}' (number, set or bool).");
                    else if (declared != expected)
                        Err("ask_type", $"Ask '{ask.Key}' declares type '{declared}' but the reference solver returns a {expected}.");
                    if (answer is double d)
                    {
                        if (double.IsNaN(d) || double.IsInfinity(d)) Err("ask", $"Ask '{ask.Key}' resolves to a non-finite value.");
                        // Leakage scan: the reference value must not appear verbatim in visible text.
                        var s = d.ToString("0.####", CultureInfo.InvariantCulture);
                        if (s.Length >= 3 && visible.ToString().Contains(s, StringComparison.Ordinal))
                            Err("leak", $"Reference value for '{ask.Key}' ({s}) appears in participant-visible text.");
                    }
                }
            }
        }

        var decisionCount = 0;
        if (root.TryGetProperty("decisions", out var decs))
        {
            if (decs.ValueKind != JsonValueKind.Array) Err("decisions", "`decisions` must be an array.");
            else foreach (var d in decs.EnumerateArray())
            {
                decisionCount++;
                var dk = Str(d, "key") ?? $"#{decisionCount}";
                if (string.IsNullOrWhiteSpace(Str(d, "prompt"))) Err("decision", $"Decision '{dk}' needs a prompt.");
                if (!d.TryGetProperty("options", out var opts) || opts.ValueKind != JsonValueKind.Array || opts.GetArrayLength() < 2)
                { Err("decision", $"Decision '{dk}' needs at least two options."); continue; }
                var qualities = new List<double>();
                foreach (var o in opts.EnumerateArray())
                {
                    var ok = Str(o, "key") ?? "?";
                    if (string.IsNullOrWhiteSpace(Str(o, "label"))) Err("option", $"Decision '{dk}' option '{ok}' needs a label.");
                    if (string.IsNullOrWhiteSpace(Str(o, "consequence"))) Err("option", $"Decision '{dk}' option '{ok}' needs a consequence for the debrief.");
                    var q = o.TryGetProperty("quality", out var qv) && qv.ValueKind == JsonValueKind.Number ? qv.GetDouble() : -1;
                    if (q < 0 || q > 100) Err("option", $"Decision '{dk}' option '{ok}' quality must be 0–100.");
                    qualities.Add(q);
                }
                if (qualities.Distinct().Count() < 2)
                    Err("decision", $"Decision '{dk}' options must not all share the same quality — the choice must matter.");
            }
        }

        if (askCount + decisionCount == 0) Err("empty", "A challenge needs at least one graded ask or decision.");
        if (!root.TryGetProperty("profile_map", out _)) Warn("profile", "No profile_map — generic decision profiles will be used.");
        return issues;
    }

    /// <summary>The ONLY payload shape the participant workspace receives: an explicit allow-list.
    /// No option qualities, no consequences, no principles, no reference values, no share_line.</summary>
    public static object PublicView(string configJson)
    {
        var root = JsonDocument.Parse(configJson).RootElement;
        var evidence = new List<object>();
        if (root.TryGetProperty("evidence", out var ev) && ev.ValueKind == JsonValueKind.Array)
            foreach (var e in ev.EnumerateArray())
                evidence.Add(new { label = Str(e, "label"), value = Str(e, "value") });

        var asks = SimGrade.ParseAsk(root).Select(a => new { key = a.Key, label = a.Label, type = a.Type }).ToList();

        var decisions = new List<object>();
        if (root.TryGetProperty("decisions", out var decs) && decs.ValueKind == JsonValueKind.Array)
            foreach (var d in decs.EnumerateArray())
            {
                var options = new List<object>();
                if (d.TryGetProperty("options", out var opts) && opts.ValueKind == JsonValueKind.Array)
                    foreach (var o in opts.EnumerateArray())
                        options.Add(new { key = Str(o, "key"), label = Str(o, "label") });
                decisions.Add(new { key = Str(d, "key"), prompt = Str(d, "prompt"), options });
            }

        return new { context = Str(root, "context"), evidence, ask = asks, decisions };
    }

    static string? Str(JsonElement el, string key) =>
        el.ValueKind == JsonValueKind.Object && el.TryGetProperty(key, out var v) && v.ValueKind == JsonValueKind.String
            ? v.GetString() : null;
}
