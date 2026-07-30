using System.Text.RegularExpressions;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Phase 5A (Release 1) — downloading the printable Passport documents from MyPCI, and the opaque
/// verification landing their QR codes point at (spec §7A.3–§7A.5).
///
///   GET /api/me/world-passport/documents/{format}   format = wallet | badge   (student bearer)
///   GET /world/pd/{key}                             the printed-document verification page
///
/// Resolution is canonical-first, the SAME two links Endpoints/PassportSummary.cs honours (the
/// direct student_user_id link, or a resolved pciworld_user_map row), so the document always
/// describes the account both products agree is this student's.
///
/// The publication gate: ONLY a PUBLISHED, unexpired Passport on an active account gets a
/// verifiable document. Draft, private, expired, suspended, unlinked and numberless states all
/// collapse to ONE refusal (404 not_available) — a printable artefact that outlives wallets and
/// lanyards must never be minted for a record that does not currently verify, and the refusal must
/// not say which gate failed. The private-preview variant is a later slice.
///
/// Why the QR encodes /world/pd/{sha} and not /world/p/{token}: the published Passport token is
/// stored HASHED ONLY (passport_token_sha) — by design nobody but the owner holds the raw link, so
/// this endpoint cannot reconstruct it, and re-minting it here would rotate the owner's published
/// link as a side effect of a READ (the exact rule PassportSummary pins). The stored hash is
/// derived one-way FROM the token, so it is a strictly weaker capability than the link the owner
/// already shares; it carries no PII; and it lives and dies with the token itself — publish
/// rotates it, unpublish clears it, expiry kills it, so a withdrawn Passport instantly kills every
/// printed QR. The landing page resolves the hash by exact match against the same predicate the
/// token route enforces (published, active, unexpired → otherwise one neutral 404).
/// </summary>
public static class PassportDocs
{
    /// <summary>Exact shape of a stored token hash (Security.Sha: 64 lowercase hex). Anything
    /// else never reaches the database.</summary>
    static readonly Regex KeyShape = new("^[0-9a-f]{64}$", RegexOptions.Compiled);

    public sealed record Resolution(Dictionary<string, object?> World, long WorldId, string TokenSha, string StudentNumber);

    /// <summary>
    /// The whole document-eligibility predicate in one place: NULL for every state that must
    /// refuse (and callers map every NULL to the same 404 — never branch on WHY), a
    /// <see cref="Resolution"/> only when every gate passes: linked World account, active on both
    /// sides, published, a live token hash, not expired, and a canonical Student Number to print.
    /// </summary>
    public static Resolution? ResolvePublished(Db db, long studentId)
    {
        var w = db.QueryOne(@"SELECT w.* FROM pciworld_users w
            WHERE w.student_user_id=? OR EXISTS(
                SELECT 1 FROM pciworld_user_map m
                WHERE m.legacy_world_id=w.id AND m.canonical_user_id=?)
            ORDER BY w.id LIMIT 1", studentId, studentId);
        if (w is null || H.Str(w["status"]) != "active") return null;
        var participant = db.QueryOne("SELECT status FROM pciworld_participants WHERE user_id=?", studentId);
        if (participant is not null && H.Str(participant["status"]) != "active") return null;
        if (H.L(w["passport_public"]) != 1) return null;
        if (WorldPassport.Expired(w)) return null;
        var sha = H.Str(w["passport_token_sha"]);
        if (sha is null || !KeyShape.IsMatch(sha)) return null;
        // The Student Number is printed on the document face (never in the QR). Read-side only —
        // a document download must never trigger issuance.
        var number = StudentNumbers.Read(db, studentId);
        if (string.IsNullOrWhiteSpace(number)) return null;
        return new Resolution(w, H.L(w["id"]), sha, number!);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        // ── the authenticated download ──
        app.MapGet("/api/me/world-passport/documents/{format}", (HttpContext ctx, string format) =>
        {
            // A per-person generated artefact: never cacheable by any intermediary.
            ctx.Response.Headers["Cache-Control"] = "no-store";
            if (!Settings.Bool(db, "world_enabled", true))
                return Results.Json(new { error = "world_disabled" }, statusCode: 503);
            var s = Auth.UserFromReq(ctx.Request, db);
            if (s is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (format is not ("wallet" or "badge"))
                return Results.Json(new { error = "unknown_format" }, statusCode: 404);

            var r = ResolvePublished(db, s.Id);
            if (r is null) return Results.Json(new { error = "not_available" }, statusCode: 404);

            // Whole-history published counts from the SAME aggregate the Passport page, the PDF
            // and the summary all use — the headline can only say what the Passport already says.
            var stats = WorldAccount.EvidenceStats(db, r.WorldId, visibleOnly: true);
            var baseUrl = WorldUrl.Base(ctx.Request);
            var d = new PassportDocuments.DocData
            {
                Name = H.Str(r.World["display_name"]) ?? "PCI World participant",
                StudentNumber = r.StudentNumber,
                // The ONLY QR payload: the opaque HTTPS verification URL. Never the Student
                // Number alone, never email, never any internal id.
                VerifyUrl = baseUrl + PassportDocuments.VerifyPath(r.TokenSha),
                ShortVerifyUrl = baseUrl.Replace("https://", "").Replace("http://", "") + "/world/verify",
                Headline = stats.Completed > 0
                    ? $"{stats.Completed} completed PCI World challenge{(stats.Completed == 1 ? "" : "s")} across {stats.Industries} industr{(stats.Industries == 1 ? "y" : "ies")}"
                    : null,
                ExpiresOn = H.Str(r.World["passport_expires_at"])?.Split(' ')[0],
            };
            var (bytes, kind) = format == "wallet"
                ? (PassportDocuments.WalletCard(d), "Wallet")
                : (PassportDocuments.EventBadge(d), "Event-Badge");
            log(s.Id, "passport_document_download", $"{format} student #{s.Id}");
            // Results.File with a download name sets Content-Disposition: attachment.
            return Results.File(bytes, "application/pdf", $"PCI-Passport-{r.StudentNumber}-{kind}.pdf");
        });

        // ── the printed QR's landing: opaque, revocable, one neutral 404 for every dead state ──
        app.MapGet("/world/pd/{key}", (HttpContext ctx, string key) =>
        {
            ctx.Response.Headers["Cache-Control"] = "no-store";
            ctx.Response.Headers["X-Robots-Tag"] = "noindex";
            if (!Settings.Bool(db, "world_enabled", true)) return Results.NotFound();
            key = (key ?? "").Trim().ToLowerInvariant();
            if (!KeyShape.IsMatch(key)) return Results.NotFound();

            // Exact-match on the stored hash, under the SAME predicate the token route
            // (WorldAccount.PassportByToken) enforces: absent, unpublished, suspended and expired
            // all collapse to the same NotFound — "withdrawn" must be indistinguishable from
            // "never existed".
            var u = db.QueryOne(@"SELECT * FROM pciworld_users
                WHERE passport_token_sha=? AND passport_public=1 AND status='active'", key);
            if (u is null || WorldPassport.Expired(u)) return Results.NotFound();

            var wid = H.L(u["id"]);
            var show = WorldPassport.Disclosure.From(u);
            var stats = WorldAccount.EvidenceStats(db, wid, visibleOnly: true);
            var rows = WorldAccount.EvidenceRows(db, wid, visibleOnly: true, limit: 20);

            // The Student Number, so a verifier can match the record against the printed front —
            // shown only when the canonical identity passes the SAME gates the by-number verifier
            // (WorldVerify.Resolve) applies: active, non-test student with active participation.
            string? number = null;
            if (WorldIdentity.CanonicalUserFor(db, wid) is { } uid)
            {
                var owner = db.QueryOne(
                    "SELECT role, status, COALESCE(is_test,0) AS is_test FROM users WHERE id=?", uid);
                var part = db.QueryOne("SELECT status FROM pciworld_participants WHERE user_id=?", uid);
                if (owner is not null && H.Str(owner["role"]) == "student" && H.Str(owner["status"]) == "active"
                    && H.L(owner["is_test"]) == 0 && part is not null && H.Str(part["status"]) == "active")
                    number = StudentNumbers.Read(db, uid);
            }

            var name = H.Str(u["display_name"]) ?? "PCI World participant";
            var expires = H.Str(u["passport_expires_at"])?.Split(' ')[0];
            string E(string? v) => WorldPages.E(v);

            // Field-level disclosure enforced at render, exactly like the public Passport page: a
            // value the owner did not publish never reaches this page at all.
            var items = string.Join("", rows.Select(row => $"""
                <tr>
                  <td><b>{E(H.Str(row["title"]))}</b><br>
                    <small class="num" style="color:var(--slate)">{E(H.Str(row["code"]))} &middot; v{H.L(row["version"])}</small></td>
                  <td>{E(H.Str(row["industry"]))}</td>
                  {(show.Scores ? $"<td class=\"num\">{H.D(row["score"]):0.#}</td>" : "")}
                  {(show.Dates ? $"<td class=\"num\">{E((H.Str(row["completed_at"]) ?? "").Split(' ')[0])}</td>" : "")}
                </tr>
                """));
            var truncated = stats.Completed > rows.Count
                ? $"<p class=\"meta\"><span>Showing the {rows.Count} most recent of {stats.Completed} published challenges.</span></p>"
                : "";

            var body = $"""
                <span class="kicker">Printed-document verification</span>
                <h1>{E(name)}</h1>
                <p class="lede">You scanned a printed PCI Passport document (wallet card or event badge).
                   This page is the live record it refers to — served from PCI World at the moment you
                   loaded it. If the owner withdraws or expires their Passport link, this page stops
                   resolving, and so does the printed code.</p>
                <div class="card">
                  <span class="kicker">Status</span>
                  <h2 style="margin-top:0">Published and current</h2>
                  {(number is null ? "" : $"<p>PCI Student Number: <b class=\"num\">{E(number)}</b> — match it against the number printed on the document.</p>")}
                  <p><b class="num">{stats.Completed}</b> completed challenge{(stats.Completed == 1 ? "" : "s")}
                     &middot; <b class="num">{stats.Industries}</b> industr{(stats.Industries == 1 ? "y" : "ies")}
                     &middot; <b class="num">{stats.Tracks}</b> track{(stats.Tracks == 1 ? "" : "s")}</p>
                  <p class="meta"><span>Shows: {E(show.Summary)}</span>{(expires is null ? "" : $"<span>Link expires {E(expires)}</span>")}</p>
                </div>
                <div class="card">
                  <h2 style="margin-top:0">Published evidence</h2>
                  <p style="color:var(--slate);margin-bottom:6px">Each row is a completed challenge its owner chose to publish.</p>
                  <div class="tbl-wrap">
                  <table>
                    <caption class="visually-hidden">Challenges this participant chose to publish</caption>
                    <thead><tr><th scope="col">Challenge</th><th scope="col">Industry</th>
                    {(show.Scores ? "<th scope=\"col\">Score</th>" : "")}
                    {(show.Dates ? "<th scope=\"col\">Date</th>" : "")}</tr></thead>
                    <tbody>{items}</tbody>
                  </table>
                  </div>
                  {truncated}
                </div>
                <p class="meta"><span>You can also verify a PCI Student Number directly at
                   <a href="/world/verify">/world/verify</a>.</span></p>
                <p class="notice">The printed card or badge is a copy; this live record is the authority.
                   {E(WorldPages.PracticeNotice)}</p>
                """;

            return Results.Content(WorldPages.Layout(db,
                $"{name} — verified PCI World Passport",
                "Live verification record for a printed PCI Passport document.",
                body,
                // Its OWN opaque address as canonical (ID-09) — and noindex: the key is revocable
                // and rotatable, and a search index would outlive both.
                "/world/pd/" + Uri.EscapeDataString(key),
                noindex: true), "text/html; charset=utf-8");
        });
    }
}
