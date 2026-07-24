using System.Text.Json;
using System.Text.Json.Nodes;

namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — deterministic scenario manifest (§5B.4). Serialises one scenario version into a
/// self-contained, human-readable JSON document an operator can archive, diff, review offline or carry
/// between environments.
///
/// Three properties make the manifest trustworthy, and each one is a deliberate exclusion:
///
///  • <b>Deterministic.</b> Exporting the same scenario twice produces byte-identical output. There is no
///    export timestamp and no exporter identity anywhere in the document — a manifest that changed on every
///    download could not be diffed or checksum-compared, which is the whole point of having one.
///  • <b>Checksummed over CONTENT only.</b> <see cref="Checksum"/> hashes the scenario block alone, so it is
///    the fingerprint of what the engine actually grades. Governance dates may be re-set on a published
///    scenario (they are metadata, not content, and §18 immutability does not freeze them), so folding them
///    into the hash would make a frozen version appear to change. Two exports of the same published version
///    therefore always agree, and any drift in the graded content is visible as a checksum change.
///  • <b>No identities and no usage.</b> Reviewer / author / approver columns are reduced to boolean
///    sign-off flags, and attempt counts are omitted entirely. That keeps admin and student data out of a
///    file built for sharing — and keeps the export stable while students practise.
///
/// The document is pure content + governance + the live §14 validation verdict; it never contains an answer
/// key (grading stays derived from `given` at grade time) and never contains student data.
/// </summary>
public static class SimManifest
{
    /// <summary>Bumped only on a breaking change to the document shape, so a future importer can branch.</summary>
    public const int ManifestVersion = 1;
    public const string Kind = "pci.simulation.scenario";

    /// <summary>The graded content of one scenario version — the block the checksum covers.</summary>
    public static JsonObject Content(Dictionary<string, object?> s)
    {
        var o = new JsonObject
        {
            ["scenario_code"] = Str(s, "scenario_code"),
            ["version"] = Num(s, "version") ?? 1,
            ["title"] = Str(s, "title"),
            ["kind"] = Str(s, "kind"),
            ["industry"] = Str(s, "industry"),
            ["project_type"] = Str(s, "project_type"),
            ["difficulty"] = Str(s, "difficulty"),
            ["est_minutes"] = Num(s, "est_minutes") ?? 0,
            ["certification_id"] = Num(s, "certification_id"),
            ["competencies"] = Parse(Str(s, "competencies_json")) ?? new JsonArray(),
            ["summary"] = Str(s, "summary"),
            ["brief"] = Str(s, "brief"),
            ["objectives"] = Parse(Str(s, "objectives_json")),
            ["provenance"] = Str(s, "provenance"),
            ["disclaimers"] = Str(s, "disclaimers"),
            ["worked_solution"] = Str(s, "worked_solution"),
            ["synthetic_declared"] = Flag(s, "synthetic_declared"),
            ["config"] = Parse(Str(s, "config_json")),
        };
        return o;
    }

    /// <summary>SHA-256 of the canonical (compact) content block — the fingerprint of what gets graded.</summary>
    public static string Checksum(Dictionary<string, object?> s) => Security.Sha(Content(s).ToJsonString());

    /// <summary>The full manifest: content + governance state + the live validation verdict.</summary>
    public static string Build(Dictionary<string, object?> s, IReadOnlyList<SimContent.Issue> issues)
    {
        var content = Content(s);
        var checksum = Security.Sha(content.ToJsonString());   // hash before attaching — a node has one parent

        var doc = new JsonObject
        {
            ["manifest_version"] = ManifestVersion,
            ["kind"] = Kind,
            ["checksum"] = checksum,
            ["scenario"] = content,
            ["governance"] = new JsonObject
            {
                ["status"] = Str(s, "status"),
                ["review_state"] = Str(s, "review_state") ?? "draft",
                ["approved_at"] = Str(s, "approved_at"),
                ["published_at"] = Str(s, "published_at"),
                ["review_due"] = Str(s, "review_due"),
                ["expires_at"] = Str(s, "expires_at"),
                // Sign-off is recorded as "did this stage happen", never as who did it.
                ["signed_off"] = new JsonObject
                {
                    ["calc_review"] = Num(s, "calc_reviewed_by") is not null,
                    ["learning_review"] = Num(s, "learning_reviewed_by") is not null,
                    ["safety_review"] = Num(s, "safety_reviewed_by") is not null,
                    ["approved"] = Num(s, "approved_by") is not null,
                },
            },
            ["validation"] = new JsonObject
            {
                ["publishable"] = SimContent.Publishable(issues),
                ["errors"] = issues.Count(i => i.Severity == SimContent.Severity.Error),
                ["warnings"] = issues.Count(i => i.Severity == SimContent.Severity.Warning),
                ["issues"] = new JsonArray(issues.Select(i => (JsonNode)new JsonObject
                {
                    ["severity"] = i.Severity.ToString().ToLowerInvariant(),
                    ["code"] = i.Code,
                    ["message"] = i.Message,
                }).ToArray()),
            },
        };
        return doc.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>Download filename for a scenario version. The code is operator-authored, so it is reduced to
    /// a safe alphabet before it reaches a Content-Disposition header — a quote or newline in a scenario code
    /// must never be able to forge a response header.</summary>
    public static string FileName(string? scenarioCode, long version)
    {
        var safe = new string((scenarioCode ?? "").Where(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.').ToArray());
        if (safe.Length == 0) safe = "scenario";
        if (safe.Length > 64) safe = safe[..64];
        return $"{safe}-v{version}.pcisim.json";
    }

    static string? Str(Dictionary<string, object?> s, string col) =>
        s.TryGetValue(col, out var v) && v is not null ? H.Str(v) : null;

    static long? Num(Dictionary<string, object?> s, string col) =>
        s.TryGetValue(col, out var v) && v is not null ? H.L(v) : null;

    static bool Flag(Dictionary<string, object?> s, string col) => Num(s, col) == 1;

    /// <summary>Re-serialise stored JSON through the parser so the manifest carries canonical, compact JSON
    /// rather than whatever whitespace the column happens to hold. Unparseable text is preserved verbatim as
    /// a string — an export must never silently drop authored content.</summary>
    static JsonNode? Parse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try { return JsonNode.Parse(json); }
        catch (JsonException) { return JsonValue.Create(json); }
    }
}
