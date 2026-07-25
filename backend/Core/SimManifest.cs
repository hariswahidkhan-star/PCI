using System.Globalization;
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
            ["objectives"] = Canon(Parse(Str(s, "objectives_json"))),
            ["provenance"] = Str(s, "provenance"),
            ["disclaimers"] = Str(s, "disclaimers"),
            ["worked_solution"] = Str(s, "worked_solution"),
            ["synthetic_declared"] = Flag(s, "synthetic_declared"),
            ["config"] = Canon(Parse(Str(s, "config_json"))),
        };
        return o;
    }

    /// <summary>
    /// Normalises numbers so the checksum survives a JSON round-trip.
    ///
    /// JsonNode re-emits a number's ORIGINAL token — 1.50 stays "1.50", 2e3 stays "2e3". Any honest
    /// tool in a transfer pipeline (jq, Python, a text editor's formatter) rewrites those to 1.5 and
    /// 2000, which changes nothing about the value but changed the hash, so a genuine file came back
    /// as checksum_mismatch. Re-creating each number from its parsed value gives one spelling per
    /// value on both sides of the wire. Integers stay integers so a large id keeps its precision.
    /// </summary>
    static JsonNode? Canon(JsonNode? n)
    {
        switch (n)
        {
            case null: return null;
            case JsonObject o:
            {
                var res = new JsonObject();
                foreach (var kv in o) res[kv.Key] = Canon(kv.Value);
                return res;
            }
            case JsonArray a:
            {
                var res = new JsonArray();
                foreach (var item in a) res.Add(Canon(item));
                return res;
            }
            default:
                if (n.GetValueKind() != JsonValueKind.Number) return JsonNode.Parse(n.ToJsonString());
                var raw = n.ToJsonString();
                return long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var l)
                    ? JsonValue.Create(l)
                    : JsonValue.Create(double.Parse(raw, CultureInfo.InvariantCulture));
        }
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

    public const string BundleKind = "pci.simulation.bundle";

    /// <summary>
    /// A whole-catalogue bundle (§5B.6): every supplied scenario's manifest in one deterministic document,
    /// so an operator can back up or migrate the authored catalogue instead of exporting row by row.
    ///
    /// Scenarios are emitted in scenario_code order, not database order, so two environments holding the
    /// same content produce the same bundle regardless of the ids they happen to have assigned. The bundle
    /// checksum is the hash of the ordered per-scenario checksums, which makes "is this catalogue the same
    /// as that one" a single string comparison — and, because each entry keeps its own checksum, a
    /// difference can still be narrowed to the scenario that caused it.
    /// </summary>
    public static string Bundle(IEnumerable<(Dictionary<string, object?> Row, IReadOnlyList<SimContent.Issue> Issues)> scenarios)
    {
        var entries = scenarios
            .Select(s => (Code: H.Str(s.Row["scenario_code"]) ?? "", Sum: Checksum(s.Row), s.Row, s.Issues))
            .OrderBy(e => e.Code, StringComparer.Ordinal)
            .ToList();

        var items = new JsonArray();
        foreach (var e in entries) items.Add(JsonNode.Parse(Build(e.Row, e.Issues))!);

        var doc = new JsonObject
        {
            ["manifest_version"] = ManifestVersion,
            ["kind"] = BundleKind,
            // One comparison answers "same catalogue?"; each entry's own checksum localises any difference.
            ["checksum"] = Security.Sha(string.Join("\n", entries.Select(e => $"{e.Code}:{e.Sum}"))),
            ["count"] = entries.Count,
            ["scenarios"] = items,
        };
        return doc.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>Reasons a manifest cannot be imported.</summary>
    public enum Reject { None, BadManifest, WrongKind, UnsupportedVersion, ChecksumMismatch }

    /// <summary>The API error code for a rejection — the wire contract lives with the enum it describes.</summary>
    public static string Code(Reject r) => r switch
    {
        Reject.BadManifest => "bad_manifest",
        Reject.WrongKind => "wrong_kind",
        Reject.UnsupportedVersion => "unsupported_version",
        Reject.ChecksumMismatch => "checksum_mismatch",
        _ => "ok",
    };

    /// <summary>
    /// Turn an exported manifest's `scenario` block back into a row-shaped dictionary — the same shape
    /// <see cref="Content"/> and <see cref="Checksum"/> read, and the same shape the INSERT needs.
    ///
    /// Going back through the canonical projection (rather than hashing the uploaded bytes) is what makes
    /// the checksum check meaningful: a file that was pretty-printed, re-ordered or re-encoded on its way
    /// between environments still verifies, while a file whose VALUES were altered does not.
    /// </summary>
    public static Dictionary<string, object?> RowFromManifest(JsonObject scenario)
    {
        string? S(string k) => scenario[k] is { } n && n.GetValueKind() == JsonValueKind.String ? n.GetValue<string>() : null;
        long? N(string k) => scenario[k] is { } n && n.GetValueKind() == JsonValueKind.Number ? n.GetValue<long>() : null;
        // A nested object/array is carried as its JSON text (the column format); a plain string is carried
        // as-is, which is how the exporter preserves content it could not parse.
        string? J(string k) => scenario[k] switch
        {
            null => null,
            var n when n.GetValueKind() is JsonValueKind.Object or JsonValueKind.Array => n.ToJsonString(),
            var n when n.GetValueKind() == JsonValueKind.String => n.GetValue<string>(),
            _ => null,
        };

        return new Dictionary<string, object?>
        {
            ["scenario_code"] = S("scenario_code"),
            ["version"] = N("version") ?? 1,
            ["title"] = S("title"),
            ["kind"] = S("kind"),
            ["industry"] = S("industry"),
            ["project_type"] = S("project_type"),
            ["difficulty"] = S("difficulty"),
            ["est_minutes"] = N("est_minutes") ?? 0,
            ["certification_id"] = N("certification_id"),
            ["competencies_json"] = J("competencies"),
            ["summary"] = S("summary"),
            ["brief"] = S("brief"),
            ["objectives_json"] = J("objectives"),
            ["provenance"] = S("provenance"),
            ["disclaimers"] = S("disclaimers"),
            ["worked_solution"] = S("worked_solution"),
            ["synthetic_declared"] = scenario["synthetic_declared"]?.GetValueKind() == JsonValueKind.True ? 1L : 0L,
            ["config_json"] = J("config"),
        };
    }

    /// <summary>
    /// Validate an uploaded manifest and hand back the row it describes. Checks the envelope (kind and a
    /// manifest version this build understands), that the scenario block carries the minimum identity a
    /// scenario needs, and that the recomputed content checksum matches the one the file claims — so a
    /// truncated, corrupted or edited manifest is refused rather than silently imported as different content.
    ///
    /// It deliberately says nothing about lifecycle: the caller always lands an import in draft. Nothing in
    /// the file can grant published state.
    /// </summary>
    /// <summary>One verified entry of a bundle: the row to insert, and the checksum that vouches for it.</summary>
    public sealed record BundleEntry(string Code, Dictionary<string, object?> Row, string Checksum, long Version);

    /// <summary>
    /// Verifies a whole-catalogue bundle (§5B.8) — envelope, then every entry, then the bundle checksum.
    ///
    /// Integrity is all-or-nothing on purpose. A bundle whose checksum does not reconcile cannot be
    /// partially trusted: if one entry was altered in transit there is no reason to believe the rest
    /// survived, so the caller must import none of it rather than guess which half is genuine. Whether
    /// an entry's CODE is already taken is a different question — that is this environment's state, not
    /// the file's integrity — and is left to the caller.
    /// </summary>
    public static Reject VerifyBundle(JsonObject? bundle, out List<BundleEntry> entries)
    {
        entries = new List<BundleEntry>();
        if (bundle is null) return Reject.BadManifest;

        if (!string.Equals(bundle["kind"]?.GetValue<string>(), BundleKind, StringComparison.Ordinal))
            return Reject.WrongKind;

        var version = bundle["manifest_version"]?.GetValueKind() == JsonValueKind.Number
            ? bundle["manifest_version"]!.GetValue<int>() : 0;
        if (version <= 0 || version > ManifestVersion) return Reject.UnsupportedVersion;

        if (bundle["scenarios"] is not JsonArray items) return Reject.BadManifest;

        var found = new List<BundleEntry>();
        foreach (var item in items)
        {
            if (item is not JsonObject one) return Reject.BadManifest;
            var verdict = Verify(one, out var row, out var sum);
            if (verdict != Reject.None) return verdict;   // one bad entry condemns the file
            found.Add(new BundleEntry(H.Str(row["scenario_code"]) ?? "", row, sum,
                one["scenario"]?["version"]?.GetValueKind() == JsonValueKind.Number
                    ? one["scenario"]!["version"]!.GetValue<long>() : 0));
        }

        // Recompute the bundle checksum exactly as Bundle() derives it: the ordered per-entry checksums.
        // Entries verified individually above, so this is what catches a reordered or truncated file.
        var ordered = found.OrderBy(e => e.Code, StringComparer.Ordinal).ToList();
        var recomputed = Security.Sha(string.Join("\n", ordered.Select(e => $"{e.Code}:{e.Checksum}")));
        if (!string.Equals(recomputed, bundle["checksum"]?.GetValue<string>(), StringComparison.OrdinalIgnoreCase))
            return Reject.ChecksumMismatch;

        entries = ordered;
        return Reject.None;
    }

    public static Reject Verify(JsonObject? manifest, out Dictionary<string, object?> row, out string checksum)
    {
        row = new Dictionary<string, object?>();
        checksum = "";
        if (manifest is null) return Reject.BadManifest;

        var kind = manifest["kind"]?.GetValue<string>();
        if (!string.Equals(kind, Kind, StringComparison.Ordinal)) return Reject.WrongKind;

        var version = manifest["manifest_version"]?.GetValueKind() == JsonValueKind.Number
            ? manifest["manifest_version"]!.GetValue<int>() : 0;
        // A newer manifest may carry fields this build would silently drop, so refuse it rather than
        // import a lossy copy. Older versions stay importable.
        if (version <= 0 || version > ManifestVersion) return Reject.UnsupportedVersion;

        if (manifest["scenario"] is not JsonObject scenario) return Reject.BadManifest;
        var built = RowFromManifest(scenario);
        if (string.IsNullOrWhiteSpace(H.Str(built["scenario_code"])) || string.IsNullOrWhiteSpace(H.Str(built["title"])))
            return Reject.BadManifest;

        var recomputed = Checksum(built);
        var claimed = manifest["checksum"]?.GetValue<string>();
        if (!string.Equals(recomputed, claimed, StringComparison.OrdinalIgnoreCase)) return Reject.ChecksumMismatch;

        row = built;
        checksum = recomputed;
        return Reject.None;
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
