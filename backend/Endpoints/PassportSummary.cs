using System.Text.Json.Serialization;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Phase 4 — the SAME authoritative PCI World Passport, read from inside MyPCI (spec §6).
///
/// One versioned read model keyed by the canonical users.id: the student portal renders the
/// Passport from this summary instead of copying data or framing the World surface. Everything the
/// DTO carries is computed by the SAME functions the World product already uses —
/// WorldAccount.EvidenceRows / EvidenceStats (union-ownership predicate included),
/// WorldPassport.Disclosure / Expired — so the number a student sees in MyPCI can never drift from
/// the number PCI World shows for the same account. Nothing here re-derives a count or a status
/// with its own SQL when an existing function computes it.
///
/// Privacy contract:
///   • never exposes users.id, pciworld_users.id or any other internal identifier;
///   • evidence highlights are DISCLOSED items only (passport_visible=1), shaped by the owner's
///     field-level disclosure exactly like the public page;
///   • the public URL is reported ONLY when the raw token is known. The stored value is a hash
///     (passport_token_sha) from which the /world/p/… path cannot be reconstructed, and minting a
///     fresh token here would rotate the owner's published link as a side effect of a READ — so
///     when only the hash exists, publicUrl is omitted and canShowPublicUrl is false;
///   • Cache-Control: no-store — a per-person read model must never be cached by an intermediary.
/// </summary>
public static class PassportSummary
{
    public sealed record EvidenceHighlight(
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("industry")] string? Industry,
        [property: JsonPropertyName("track")] string? Track,
        [property: JsonPropertyName("difficulty")] string? Difficulty,
        [property: JsonPropertyName("score"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] double? Score,
        [property: JsonPropertyName("profile"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Profile,
        [property: JsonPropertyName("completedOn"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? CompletedOn);

    public sealed record DisclosureSettings(
        [property: JsonPropertyName("scores")] bool Scores,
        [property: JsonPropertyName("profiles")] bool Profiles,
        [property: JsonPropertyName("dates")] bool Dates);

    public sealed record SummaryDto(
        [property: JsonPropertyName("schemaVersion")] int SchemaVersion,
        [property: JsonPropertyName("studentNumber")] string? StudentNumber,
        [property: JsonPropertyName("displayName")] string? DisplayName,
        [property: JsonPropertyName("participantState")] string ParticipantState,   // none | active | link_pending
        [property: JsonPropertyName("passportState")] string PassportState,         // not_created | draft | private | published | expired | suspended
        [property: JsonPropertyName("completedChallengeCount")] long CompletedChallengeCount,
        [property: JsonPropertyName("selectedEvidenceCount")] long SelectedEvidenceCount,
        [property: JsonPropertyName("evidenceHighlights")] IReadOnlyList<EvidenceHighlight> EvidenceHighlights,
        [property: JsonPropertyName("disclosureSettings")] DisclosureSettings Disclosure,
        [property: JsonPropertyName("publicUrl"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? PublicUrl,
        [property: JsonPropertyName("canShowPublicUrl")] bool CanShowPublicUrl,
        [property: JsonPropertyName("expiresAt")] string? ExpiresAt,
        [property: JsonPropertyName("lastUpdatedAt")] string? LastUpdatedAt,
        [property: JsonPropertyName("availableActions")] IReadOnlyList<string> AvailableActions);

    /// <summary>
    /// Build the summary for a canonical student. Resolution is canonical-first: the student's
    /// World account is found through the SAME two links WorldIdentity.CanonicalUserFor honours in
    /// the other direction (the direct student_user_id link, or a resolved pciworld_user_map row) —
    /// so both products agree on which account is "theirs". A student with no World participation
    /// gets a well-formed not_created summary, never an error: the absence of a Passport is a
    /// state of the journey, not a failure of the read.
    /// </summary>
    public static SummaryDto Build(Db db, long studentId, string studentEmail)
    {
        var studentNumber = StudentNumbers.Read(db, studentId);   // read-side helper — never issues

        var w = db.QueryOne(@"SELECT w.* FROM pciworld_users w
            WHERE w.student_user_id=? OR EXISTS(
                SELECT 1 FROM pciworld_user_map m
                WHERE m.legacy_world_id=w.id AND m.canonical_user_id=?)
            ORDER BY w.id LIMIT 1", studentId, studentId);

        if (w is null)
        {
            // link_pending, seen from the canonical side: a World account holds this student's
            // email but has never proven ownership (WorldIdentity's quarantined-conflict state).
            // Surfaced so MyPCI can offer the linking journey; nothing OF that account (name,
            // evidence, counts) is shown — it has not been shown to be this person's.
            var pending = db.QueryOne(@"SELECT w.id FROM pciworld_users w
                LEFT JOIN pciworld_user_map m
                    ON m.legacy_world_id=w.id AND m.canonical_user_id IS NOT NULL
                WHERE lower(w.email)=? AND w.student_user_id IS NULL AND m.id IS NULL",
                (studentEmail ?? "").Trim().ToLowerInvariant());
            return new SummaryDto(1, studentNumber, null,
                pending is null ? "none" : "link_pending", "not_created",
                0, 0, Array.Empty<EvidenceHighlight>(),
                // Privacy-safe defaults, matching what a NEW account starts with (P1-05).
                new DisclosureSettings(false, false, false),
                null, false, null, null,
                new[] { pending is null ? "create" : "link" });
        }

        var wid = H.L(w["id"]);
        var participant = db.QueryOne("SELECT status FROM pciworld_participants WHERE user_id=?", studentId);
        var suspended = H.Str(w["status"]) != "active"
                        || (participant is not null && H.Str(participant["status"]) != "active");

        // Whole-history counts from the EXISTING aggregate functions — the same values the World
        // dashboard, the account page and the public Passport report for this account.
        var statsAll = WorldAccount.EvidenceStats(db, wid, visibleOnly: false);
        var statsSelected = WorldAccount.EvidenceStats(db, wid, visibleOnly: true);
        var show = WorldPassport.Disclosure.From(w);
        var isPublic = H.L(w["passport_public"]) == 1;
        var expired = WorldPassport.Expired(w);
        var emailVerified = H.L(w["email_verified"]) == 1;
        var hasName = !string.IsNullOrWhiteSpace(H.Str(w["display_name"]));

        // The same readiness derivation WorldDashboard.Build uses (§7.6), folded to this DTO's
        // vocabulary: published_active → published; private_ready → private;
        // private_empty/private_incomplete → draft.
        var state = suspended ? "suspended"
            : isPublic && !expired ? "published"
            : isPublic ? "expired"
            : emailVerified && hasName && statsSelected.Completed > 0 ? "private"
            : "draft";

        // Top 3 DISCLOSED items, from the same publication-safe row shape the public page and the
        // PDF draw from, honouring the owner's field-level disclosure switches.
        var highlights = WorldAccount.EvidenceRows(db, wid, visibleOnly: true, limit: 3)
            .Select(r => new EvidenceHighlight(
                H.Str(r["title"]) ?? "",
                $"{H.Str(r["code"])} · v{H.L(r["version"])}",
                H.Str(r["industry"]), H.Str(r["track"]), H.Str(r["difficulty"]),
                show.Scores && r["score"] is not null ? Math.Round(Convert.ToDouble(r["score"]), 1) : null,
                show.Profiles ? H.Str(r["profile_key"])?.Replace('_', ' ') : null,
                show.Dates ? (H.Str(r["completed_at"]) ?? "").Split(' ')[0] : null))
            .ToList();

        // The newest completed evidence (either visibility) — EvidenceRows already orders by
        // completed_at DESC, so the first row of a one-row window IS the answer.
        var newest = WorldAccount.EvidenceRows(db, wid, visibleOnly: false, limit: 1).FirstOrDefault();

        var actions = state switch
        {
            "suspended" => new[] { "contact_support" },
            "published" => new[] { "manage" },
            "expired" => new[] { "manage", "renew" },
            "private" => new[] { "manage", "publish" },
            _ => new[] { "manage", "select_evidence" },
        };

        return new SummaryDto(1, studentNumber, H.Str(w["display_name"]),
            "active", state, statsAll.Completed, statsSelected.Completed, highlights,
            new DisclosureSettings(show.Scores, show.Profiles, show.Dates),
            // Only the token's HASH is stored; the raw /world/p/… path cannot be reconstructed
            // and must never be re-minted by a read — omitted, and said so.
            null, false,
            H.Str(w["passport_expires_at"])?.Split(' ')[0],
            newest is null ? null : H.Str(newest["completed_at"]),
            actions);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        app.MapGet("/api/me/world-passport/summary", (HttpContext ctx) =>
        {
            // A per-person read model: never cacheable, whatever the answer.
            ctx.Response.Headers["Cache-Control"] = "no-store";
            if (!Settings.Bool(db, "world_enabled", true))
                return Results.Json(new { error = "world_disabled" }, statusCode: 503);
            var s = Auth.UserFromReq(ctx.Request, db);
            if (s is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return Results.Json(Build(db, s.Id, s.Email));
        });
    }
}
