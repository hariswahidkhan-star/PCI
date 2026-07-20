using System.Net;
using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Shortlist-gated identity verification for the Honorary Route. ONLY candidates the board shortlists
/// are invited — via a one-time, time-limited secure link — to submit a recent passport-style photograph
/// and one valid government-issued ID, and to accept a truthfulness + background declaration.
///
/// Privacy-by-design choices (kept deliberately conservative pending jurisdiction-specific legal review —
/// notably Saudi Arabia (PDPL), the United States and Pakistan — BEFORE production launch):
///   • Data minimisation — collected only at the shortlist stage, never at first application; no ID number
///     or criminal-history detail is stored, only the document image + a self-declaration attestation.
///   • Purpose limitation — used solely to verify identity for honorary recognition; manual board review.
///   • Secure storage — files go to a dedicated, purge-protected <c>honorary-idv</c> category; the DB keeps
///     only metadata + a reference (never the bytes, never the raw token — only its SHA-256).
///   • Access control — every read is owner/board-only and audited.
///   • Retention & deletion — an owner can delete the documents at any time (one click), and an optional
///     <c>honorary_idv_retention_days</c> setting auto-deletes them after a window (off by default).
///   • Confidentiality & due process — the applicant is told what is collected, why, how long it is kept and
///     that they may withdraw; a decision is a human one.
/// </summary>
public static class HonoraryIdv
{
    static readonly HashSet<string> Kinds = new() { "photo", "government_id" };
    const int TokenDays = 14;

    static bool Flag(Dictionary<string, JsonElement> b, string key) =>
        H.GetEl(b, key) is { } e && (e.ValueKind == JsonValueKind.True
            || (e.ValueKind == JsonValueKind.String && e.GetString() is "1" or "true" or "yes"));

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);

        (AdminCtx? adm, IResult? deny) Owner(HttpRequest req)
        {
            var a = Auth.AdminFromReq(req, db);
            if (a is null) return (null, Results.Json(new { error = "unauthorised" }, statusCode: 401));
            if (!a.IsOwner) return (null, Results.Json(new { error = "forbidden" }, statusCode: 403));
            return (a, null);
        }

        // ---------------- ADMIN: shortlist a candidate → mint a one-time verification link ----------------
        app.MapPost("/api/admin/honorary-applications/{id:long}/shortlist", (HttpContext ctx, long id) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var a = db.QueryOne("SELECT * FROM honorary_applications WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);

            // Fresh one-time token each call (invalidates any previous link). Only its SHA-256 is stored.
            var raw = Security.RandomHex(32);
            db.Execute(@"UPDATE honorary_applications SET shortlisted=1, shortlisted_at=COALESCE(shortlisted_at,datetime('now')),
                idv_token=?, idv_token_expires=datetime('now','+" + TokenDays + @" day'),
                idv_status=CASE WHEN idv_status='submitted' THEN 'submitted' ELSE 'invited' END, updated_at=datetime('now') WHERE id=?",
                Security.Sha(raw), id);

            var baseUrl = Mailer.BaseUrl(ctx.Request);
            var link = $"{baseUrl}/honorary-verification.html?token={raw}";
            var first = H.Str(a["first_name"]) ?? "";
            var email = H.Str(a["email"]) ?? "";

            // The invite reads correctly at either stage: shortlisted (pre-decision) or approved
            // (the board has decided and identity verification finalises the recognition).
            var approved = H.Str(a["status"]) == "approved";
            if (Notify.Enabled(db, "honorary") && email.Length > 0)
                try
                {
                    var opening = approved
                        ? "<p>Congratulations — the board has approved your application for <strong>Honorary Fellow (PCI)</strong>. To finalise your recognition, please complete a short, secure identity verification: a recent passport-style photograph and one valid government-issued ID, together with a brief declaration.</p>"
                        : "<p>Your application has been <strong>shortlisted</strong> for consideration. To continue, please complete a short, secure identity verification: a recent passport-style photograph and one valid government-issued ID, together with a brief declaration.</p>";
                    Notify.Email(db, null, email, approved
                            ? "Final step for your Honorary Fellow (PCI) recognition"
                            : "Next step for your Honorary Fellow (PCI) application",
                        $"<p>Dear {WebUtility.HtmlEncode(first)},</p>" + opening +
                        $"<p><a href=\"{WebUtility.HtmlEncode(link)}\">Complete your secure verification</a> (this personal link expires in {TokenDays} days and can be used once).</p>" +
                        $"<p>We collect this only to verify your identity for the honorary recognition. It is stored securely, accessible only to the board, and you may withdraw at any time. Please do not share this link.</p>" +
                        $"<p>— Project Controls Institute Global</p>",
                        "honorary_idv_invite", id);
                }
                catch { /* email best-effort; the link is also returned for manual sending */ }

            log(adm!.Id, "honorary_shortlisted", $"{H.Str(a["reference"])} by admin {adm!.Id}");
            return J(new { ok = true, link, expires_days = TokenDays, emailed = Notify.Enabled(db, "honorary") && email.Length > 0 });
        });

        // ---------------- ADMIN: revoke the outstanding verification link ----------------
        app.MapPost("/api/admin/honorary-applications/{id:long}/idv/revoke", (HttpContext ctx, long id) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var a = db.QueryOne("SELECT id FROM honorary_applications WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            db.Execute("UPDATE honorary_applications SET idv_token=NULL, idv_token_expires=NULL, updated_at=datetime('now') WHERE id=?", id);
            log(adm!.Id, "honorary_idv_link_revoked", $"app {id} by admin {adm!.Id}");
            return J(new { ok = true });
        });

        // ---------------- ADMIN: list IDV documents (metadata only) ----------------
        app.MapGet("/api/admin/honorary-applications/{id:long}/idv", (HttpContext ctx, long id) =>
        {
            var (_, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var docs = db.Query("SELECT id,doc_kind,filename,mime,size_bytes,created_at FROM honorary_idv_documents WHERE application_id=? ORDER BY id", id);
            return J(new { documents = docs });
        });

        // ---------------- ADMIN: download an IDV document (owner-only, audited) ----------------
        app.MapGet("/api/admin/honorary-applications/{id:long}/idv/{docId:long}/file", (HttpContext ctx, long id, long docId) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var d = db.QueryOne("SELECT storage_ref,mime,doc_kind FROM honorary_idv_documents WHERE id=? AND application_id=?", docId, id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var got = Storage.Get(H.Str(d["storage_ref"]));
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "file_unavailable" }, statusCode: 404);
            log(adm!.Id, "honorary_idv_document_viewed", $"app {id} doc {docId} ({H.Str(d["doc_kind"])}) by admin {adm!.Id}");
            return Results.File(got.Value.bytes!, got.Value.mime);
        });

        // ---------------- ADMIN: delete the IDV documents (data minimisation / post-decision) ----------------
        app.MapPost("/api/admin/honorary-applications/{id:long}/idv/delete", (HttpContext ctx, long id) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var n = PurgeForApplication(db, id);
            db.Execute("UPDATE honorary_applications SET idv_status='deleted', idv_deleted_at=datetime('now'), idv_token=NULL, idv_token_expires=NULL, updated_at=datetime('now') WHERE id=?", id);
            log(adm!.Id, "honorary_idv_deleted", $"app {id} ({n} file(s)) by admin {adm!.Id}");
            return J(new { ok = true, deleted = n });
        });

        // ================= PUBLIC (tokenised, one-time) =================

        // Validate the link and return the MINIMUM context the page needs (first name + reference only).
        app.MapGet("/api/honorary-idv/{token}", (string token) =>
        {
            var a = LookupByToken(db, token, out var reason);
            if (a is null) return Results.Json(new { error = reason }, statusCode: reason == "expired" ? 410 : 404);
            var already = H.Str(a["idv_status"]) == "submitted";
            // stage lets the page word itself correctly: pre-decision shortlist vs post-approval finalisation.
            var stage = H.Str(a["status"]) == "approved" ? "approved" : "shortlisted";
            return J(new { ok = true, first_name = H.Str(a["first_name"]), reference = H.Str(a["reference"]), already_submitted = already, stage });
        });

        // Accept the photo + government ID + declarations, store securely, and burn the token.
        app.MapPost("/api/honorary-idv/{token}", async (HttpContext ctx, string token) =>
        {
            var a = LookupByToken(db, token, out var reason);
            if (a is null) return Results.Json(new { error = reason }, statusCode: reason == "expired" ? 410 : 404);
            var id = H.L(a["id"]);
            if (H.Str(a["idv_status"]) == "submitted")
                return Results.Json(new { error = "already_submitted", message = "This verification has already been completed." }, statusCode: 409);

            var b = await H.Body(ctx.Request);
            if (!Flag(b, "declaration_truthful"))
                return Results.Json(new { error = "declaration_required", message = "Please confirm the truthfulness declaration." }, statusCode: 400);
            if (!Flag(b, "background_declaration"))
                return Results.Json(new { error = "background_required", message = "Please complete the background declaration." }, statusCode: 400);
            if (!Flag(b, "consent"))
                return Results.Json(new { error = "consent_required", message = "Please give consent to process your identity documents." }, statusCode: 400);

            // Exactly one photo and one government ID, each validated by the Storage intake (MIME allow-list
            // + magic-byte sniff + size cap) and written to the protected honorary-idv category.
            var stored = new List<(string kind, string name, Storage.StoredObject obj)>();
            foreach (var kind in Kinds)
            {
                var dataUri = H.GetS(b, kind, kind == "government_id" ? "governmentId" : "photograph");
                if (string.IsNullOrWhiteSpace(dataUri))
                    return Results.Json(new { error = kind + "_required", message = kind == "photo" ? "A passport-style photograph is required." : "One government-issued ID is required." }, statusCode: 400);
                var (bytes, mime, err) = Storage.DecodeDataUri(dataUri);
                if (err is not null) return Results.Json(new { error = err, doc_kind = kind }, statusCode: 400);
                var name = (H.GetS(b, kind + "_filename") ?? (kind + ".bin")).Trim();
                if (name.Length > 200) name = name[..200];
                stored.Add((kind, name, Storage.Put(bytes!, mime, "honorary-idv")));
            }

            // Replace any prior partial submission for this application (idempotency + no orphans).
            PurgeForApplication(db, id);
            foreach (var (kind, name, obj) in stored)
                db.Execute("INSERT INTO honorary_idv_documents(application_id,doc_kind,filename,mime,size_bytes,storage_ref,sha256) VALUES(?,?,?,?,?,?,?)",
                    id, kind, name, obj.Mime, obj.SizeBytes, obj.Reference, obj.Sha256);

            // Record attestations + burn the one-time token so the link cannot be reused.
            db.Execute(@"UPDATE honorary_applications SET idv_status='submitted', idv_submitted_at=datetime('now'),
                background_declaration=1, background_declared_at=datetime('now'),
                idv_token=NULL, idv_token_expires=NULL, idv_deleted_at=NULL, updated_at=datetime('now') WHERE id=?", id);
            log(null, "honorary_idv_submitted", $"app {id} ({stored.Count} file(s))");

            if (Notify.Enabled(db, "honorary"))
                try
                {
                    var adminTo = Notify.AdminEmail(db);
                    Notify.Email(db, null, adminTo, "Honorary identity verification submitted",
                        $"<p>A shortlisted applicant has completed identity verification for honorary application " +
                        $"<strong>{WebUtility.HtmlEncode(H.Str(a["reference"]))}</strong>. Review it in the admin portal → Honorary applications.</p>",
                        "honorary_idv", id);
                }
                catch { }

            return J(new { ok = true, message = "Thank you — your verification has been received securely." });
        });
    }

    /// <summary>Look up a live application by the raw token (matched on SHA-256). Sets <paramref name="reason"/>
    /// to "invalid_token" or "expired" when null.</summary>
    static Dictionary<string, object?>? LookupByToken(Db db, string rawToken, out string reason)
    {
        reason = "invalid_token";
        if (string.IsNullOrWhiteSpace(rawToken) || rawToken.Length is < 16 or > 128) return null;
        var a = db.QueryOne("SELECT * FROM honorary_applications WHERE idv_token=?", Security.Sha(rawToken));
        if (a is null) return null;
        var exp = H.Str(a["idv_token_expires"]);
        if (!string.IsNullOrEmpty(exp) && DateTime.TryParse(exp, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AdjustToUniversal, out var when) && when < DateTime.UtcNow)
        { reason = "expired"; return null; }
        return a;
    }

    /// <summary>Delete the stored IDV files for one application and drop their rows. Returns files removed.</summary>
    static int PurgeForApplication(Db db, long appId)
    {
        int n = 0;
        foreach (var d in db.Query("SELECT id,storage_ref FROM honorary_idv_documents WHERE application_id=?", appId))
            if (Storage.DeleteOne(H.Str(d["storage_ref"]))) n++;
        db.Execute("DELETE FROM honorary_idv_documents WHERE application_id=?", appId);
        return n;
    }

    /// <summary>Retention sweep (called from RetentionService): delete IDV documents whose submission is older
    /// than <paramref name="days"/>. Off unless an operator sets a positive window. Returns files removed.</summary>
    public static int PurgeExpired(Db db, int days)
    {
        if (days <= 0) return 0;
        int n = 0;
        var rows = db.Query("SELECT id FROM honorary_applications WHERE idv_submitted_at IS NOT NULL AND idv_status='submitted' AND idv_submitted_at < datetime('now','-" + days + " day')");
        foreach (var r in rows)
        {
            var id = H.L(r["id"]);
            n += PurgeForApplication(db, id);
            db.Execute("UPDATE honorary_applications SET idv_status='deleted', idv_deleted_at=datetime('now') WHERE id=?", id);
        }
        if (n > 0) Console.WriteLine($"[retention] purged {n} honorary identity document(s) older than {days}d");
        return n;
    }
}
