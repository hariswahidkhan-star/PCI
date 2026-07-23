using System.Text.Json;
using System.Text.RegularExpressions;

namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — deterministic scenario content validator (Phase 5A governance foundation).
///
/// Publication of a scenario is gated on this check (§14 "Automated publication validators must reject …").
/// It is a PURE function of the scenario's authored fields — no database, no clock, no randomness — so the
/// same scenario always yields the same verdict and the whole thing is unit-testable. The strongest check
/// runs the scenario through the real engine: every measure the author asks the student to compute
/// (<c>ask</c>) must resolve to a deterministic value of the right shape via <see cref="SimCalc.Resolve"/>.
/// If it does not, the "answer key" is missing or non-deterministic and the scenario cannot be graded — so
/// it must never publish. This layer computes NO project-controls arithmetic itself; SimCalc stays the one
/// source of truth for every answer.
/// </summary>
public static class SimContent
{
    public enum Severity { Error, Warning }

    /// <summary>A single validation finding. <c>Code</c> is a stable machine slug; <c>Message</c> is the
    /// operator-facing explanation. An <c>Error</c> blocks publication; a <c>Warning</c> is advisory.</summary>
    public sealed record Issue(Severity Severity, string Code, string Message);

    /// <summary>The authored scenario fields the validator needs, decoupled from the DB row shape so it can
    /// be exercised directly from tests and from either an existing row or an unsaved draft.</summary>
    public sealed record ScenarioInput(
        string ScenarioCode,
        string? Title,
        string? Summary,
        string? Difficulty,
        long? CertificationId,
        string? CompetenciesJson,
        string? ConfigJson,
        bool SyntheticDeclared);

    /// <summary>The four authored difficulty bands (§5).</summary>
    public static readonly IReadOnlySet<string> Difficulties =
        new HashSet<string>(StringComparer.Ordinal) { "foundation", "intermediate", "advanced", "expert" };

    /// <summary>The three live certifications the Lab maps to: 1 = PCL-AI, 2 = PFL-AI, 3 = PML-AI. NULL is
    /// allowed on a scenario (any certification); any other id is a dangling mapping.</summary>
    public static readonly IReadOnlySet<long> CertificationIds = new HashSet<long> { 1, 2, 3 };

    // Retired credential codes that must never be served from Lab content (§1 "Remove any newly encountered
    // Simulation Lab reference to retired PDL-AI"). Scoped to the unambiguous legacy forms so ordinary
    // project-controls prose can never false-positive; the canonical replacements are PCL-AI/PFL-AI/PML-AI.
    static readonly Regex RetiredNames =
        new(@"\b(PDL[-_ ]?AI|CPMD[-_ ]?AI|CPMD|PCP[-_ ]?AI|PFIP[-_ ]?AI|PFIP)\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    /// <summary>Validate one scenario. <paramref name="otherCodes"/> is every OTHER scenario code in scope
    /// (for the uniqueness check); pass null/empty when duplicate detection is not needed. The result is
    /// ordered errors-first so the most important findings surface at the top of an operator report.</summary>
    public static List<Issue> Validate(ScenarioInput s, IEnumerable<string>? otherCodes = null)
    {
        var issues = new List<Issue>();
        void Err(string code, string msg) => issues.Add(new Issue(Severity.Error, code, msg));
        void Warn(string code, string msg) => issues.Add(new Issue(Severity.Warning, code, msg));

        // ---- identity & required metadata ----
        if (string.IsNullOrWhiteSpace(s.ScenarioCode))
            Err("code_missing", "Scenario has no code.");
        else if (otherCodes is not null && otherCodes.Any(c => string.Equals(c, s.ScenarioCode, StringComparison.Ordinal)))
            Err("code_duplicate", $"Scenario code '{s.ScenarioCode}' is already used by another scenario.");

        if (string.IsNullOrWhiteSpace(s.Title))
            Err("title_missing", "Scenario has no title.");
        if (string.IsNullOrWhiteSpace(s.Summary))
            Warn("summary_missing", "Scenario has no summary — learners see this in the catalogue.");

        if (string.IsNullOrWhiteSpace(s.Difficulty) || !Difficulties.Contains(s.Difficulty))
            Err("difficulty_invalid", $"Difficulty must be one of: {string.Join(", ", Difficulties)}.");

        if (s.CertificationId is long cid && !CertificationIds.Contains(cid))
            Err("certification_invalid", $"certification_id {cid} is not a live certification (expected 1=PCL-AI, 2=PFL-AI, 3=PML-AI, or none).");

        if (CountCompetencies(s.CompetenciesJson) == 0)
            Err("competencies_missing", "Scenario declares no competencies — a scenario must map to at least one competency.");

        if (!s.SyntheticDeclared)
            Err("synthetic_undeclared", "Synthetic-data use is not declared — every published scenario must declare synthetic data only.");

        // ---- retired credential names in any served text (§1) ----
        foreach (var (field, text) in new[] { ("title", s.Title), ("summary", s.Summary), ("config", s.ConfigJson) })
            if (text is not null && RetiredNames.IsMatch(text))
                Err("retired_name", $"A retired certification name appears in the {field}; use PCL-AI / PFL-AI / PML-AI.");

        // ---- the task itself: config must parse, name a known engine task, and every asked measure must
        //      resolve to a deterministic answer of the right shape (the reference-solver check) ----
        ValidateConfig(s.ConfigJson, Err, Warn);

        // Errors first, then warnings; stable within each band.
        return issues.OrderBy(i => i.Severity).ToList();
    }

    /// <summary>True when no <see cref="Severity.Error"/> issue is present — i.e. the scenario is safe to
    /// publish. Warnings do not block.</summary>
    public static bool Publishable(IEnumerable<Issue> issues) => !issues.Any(i => i.Severity == Severity.Error);

    static int CountCompetencies(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return 0;
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return 0;
            return doc.RootElement.EnumerateArray().Count(e => e.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(e.GetString()));
        }
        catch { return 0; }
    }

    static void ValidateConfig(string? configJson, Action<string, string> err, Action<string, string> warn)
    {
        if (string.IsNullOrWhiteSpace(configJson)) { err("config_missing", "Scenario has no task definition (config_json)."); return; }

        JsonDocument doc;
        try { doc = JsonDocument.Parse(configJson); }
        catch (JsonException e) { err("config_invalid", $"Task definition is not valid JSON: {e.Message}"); return; }

        using (doc)
        {
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object) { err("config_invalid", "Task definition must be a JSON object."); return; }

            var task = root.TryGetProperty("task", out var tk) ? tk.GetString() ?? "" : "";
            if (string.IsNullOrWhiteSpace(task)) { err("task_missing", "Task definition has no 'task' selector."); return; }
            if (!SimCalc.KnownTasks.Contains(task))
            {
                err("task_unknown", $"Task '{task}' is not one the engine can grade ({string.Join(", ", SimCalc.KnownTasks)}).");
                return; // nothing downstream can be resolved for an unknown engine
            }

            if (!root.TryGetProperty("prompt", out var pr) || string.IsNullOrWhiteSpace(pr.GetString()))
                warn("prompt_missing", "Task has no prompt — the learner has nothing to read.");

            if (!root.TryGetProperty("given", out var given) || given.ValueKind != JsonValueKind.Object)
            { err("given_missing", "Task has no 'given' inputs object."); return; }

            var ask = SimGrade.ParseAsk(root);
            if (ask.Count == 0) { err("ask_missing", "Task asks the learner to compute nothing — no gradable 'ask' measures."); return; }

            // Reference-solver check: resolve every asked measure through the real engine and confirm the
            // answer exists and matches the measure's type. A null result, wrong type, or non-finite number
            // means the answer key is missing or non-deterministic — the scenario cannot be graded.
            foreach (var a in ask)
            {
                object? answer;
                try { answer = SimCalc.Resolve(task, a.Key, given); }
                catch (Exception e) { err("answer_error", $"Resolving '{a.Key}' threw: {e.Message}"); continue; }

                switch (a.Type)
                {
                    case "set":
                        if (answer is not string[] set || set.Length == 0)
                            err("answer_unresolved", $"Measure '{a.Key}' (set) has no deterministic answer from the engine.");
                        break;
                    case "bool":
                        if (answer is not bool)
                            err("answer_unresolved", $"Measure '{a.Key}' (bool) has no deterministic answer from the engine.");
                        break;
                    default: // number
                        if (answer is not double d)
                            err("answer_unresolved", $"Measure '{a.Key}' has no deterministic numeric answer from the engine.");
                        else if (double.IsNaN(d) || double.IsInfinity(d))
                            err("answer_nonfinite", $"Measure '{a.Key}' resolves to a non-finite value ({d}) — the inputs are impossible or inconsistent.");
                        break;
                }
            }

            if (root.TryGetProperty("pass_pct", out var pp) && pp.ValueKind == JsonValueKind.Number)
            {
                var v = pp.GetDouble();
                if (v <= 0 || v > 100) warn("pass_pct_range", $"pass_pct {v} is outside (0, 100].");
            }
            if (root.TryGetProperty("tolerance", out var tol) && tol.ValueKind == JsonValueKind.Number && tol.GetDouble() < 0)
                warn("tolerance_negative", "tolerance is negative.");
        }
    }
}
