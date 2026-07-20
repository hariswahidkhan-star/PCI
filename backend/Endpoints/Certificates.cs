using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Verifiable-certificate delivery: authenticated, audited PDF downloads for students, and an admin
/// regenerate action. The PDF is generated on demand if it is missing, so historical credentials and any
/// that failed at issuance still deliver. Revoked/suspended certificates are not downloadable as valid;
/// test-account certificates are never public.
/// </summary>
public static class Certificates
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        UserCtx? User(HttpRequest r) => Auth.UserFromReq(r, db);
        static string? Ip(HttpContext ctx) => ctx.Connection.RemoteIpAddress?.ToString();
        void Audit(string? credId, long? uid, string actor, string role, string? ip, string kind, string result)
        { try { db.Execute("INSERT INTO certificate_downloads(credential_id,user_id,actor,role,ip,kind,result) VALUES(?,?,?,?,?,?,?)", credId, uid, actor, role, ip, kind, result); } catch { } }

        // ── Student: download my examination certificate PDF (latest, or a specific one I own) ──
        app.MapGet("/api/me/certificate/pdf", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var wantId = (ctx.Request.Query["id"].ToString() ?? "").Trim();
            var c = wantId.Length > 0
                ? db.QueryOne("SELECT credential_id,status FROM issued_credentials WHERE credential_id=? AND user_id=?", wantId, u.Id)
                : db.QueryOne("SELECT credential_id,status FROM issued_credentials WHERE user_id=? ORDER BY id DESC", u.Id);
            if (c is null) { Audit(wantId, u.Id, u.Email, "student", Ip(ctx), "certificate", "not_found"); return Results.Json(new { error = "not_found" }, statusCode: 404); }
            var credId = H.Str(c["credential_id"])!;
            // Status gate: a revoked/suspended certificate is not downloadable as a valid document.
            var status = H.Str(c["status"]) ?? "active";
            if (status is "revoked" or "suspended")
            { Audit(credId, u.Id, u.Email, "student", Ip(ctx), "certificate", "blocked_" + status);
              return Results.Json(new { error = status, message = status == "revoked" ? "This certificate has been revoked and can no longer be downloaded." : "This certificate is currently suspended. Please contact support." }, statusCode: 403); }
            var pdf = CertIssue.EnsureCredentialPdf(db, credId);
            if (pdf is null) { Audit(credId, u.Id, u.Email, "student", Ip(ctx), "certificate", "generation_failed"); return Results.Json(new { error = "generation_failed", message = "We could not produce your certificate right now. Please try again shortly or contact support." }, statusCode: 500); }
            Audit(credId, u.Id, u.Email, "student", Ip(ctx), "certificate", "ok");
            return Results.Bytes(pdf.Value.bytes, "application/pdf", pdf.Value.name);
        });

        // ── Student: download my honorary certificate PDF ──
        app.MapGet("/api/me/honorary-certificate/pdf", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var a = db.QueryOne("SELECT award_no,status FROM honorary_awards WHERE user_id=? AND status='active' ORDER BY id DESC", u.Id);
            if (a is null) { Audit(null, u.Id, u.Email, "student", Ip(ctx), "honorary", "not_found"); return Results.Json(new { error = "not_found" }, statusCode: 404); }
            var awardNo = H.Str(a["award_no"])!;
            var pdf = CertIssue.EnsureHonoraryPdf(db, awardNo);
            if (pdf is null) { Audit(awardNo, u.Id, u.Email, "student", Ip(ctx), "honorary", "generation_failed"); return Results.Json(new { error = "generation_failed" }, statusCode: 500); }
            Audit(awardNo, u.Id, u.Email, "student", Ip(ctx), "honorary", "ok");
            return Results.Bytes(pdf.Value.bytes, "application/pdf", pdf.Value.name);
        });

        // ── Admin: regenerate a certificate PDF (e.g. after a template/name correction). Gated 'credentials'. ──
        app.MapPost("/api/admin/credentials/{credId}/regenerate-pdf", (HttpContext ctx, string credId) => gate(ctx.Request, "credentials", adm =>
        {
            var isHon = credId.StartsWith(Endpoints.Honorary.AwardPrefix + "-", StringComparison.OrdinalIgnoreCase);
            var pdf = isHon ? CertIssue.EnsureHonoraryPdf(db, credId, force: true) : CertIssue.EnsureCredentialPdf(db, credId, force: true);
            if (pdf is null) return Results.Json(new { error = "generation_failed" }, statusCode: 500);
            log(adm.Id, "certificate_pdf_regenerated", credId + " sha " + pdf.Value.sha[..12]);
            Audit(credId, null, "admin#" + adm.Id, "admin", Ip(ctx), isHon ? "honorary" : "certificate", "regenerated");
            return Results.Json(new { ok = true, sha256 = pdf.Value.sha, bytes = pdf.Value.bytes.LongLength });
        }));

        // ── Admin: download any certificate PDF (support/verification). Gated 'credentials'. ──
        app.MapGet("/api/admin/credentials/{credId}/pdf", (HttpContext ctx, string credId) => gate(ctx.Request, "credentials", adm =>
        {
            var isHon = credId.StartsWith(Endpoints.Honorary.AwardPrefix + "-", StringComparison.OrdinalIgnoreCase);
            var pdf = isHon ? CertIssue.EnsureHonoraryPdf(db, credId) : CertIssue.EnsureCredentialPdf(db, credId);
            if (pdf is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            Audit(credId, null, "admin#" + adm.Id, "admin", Ip(ctx), isHon ? "honorary" : "certificate", "admin_download");
            return Results.Bytes(pdf.Value.bytes, "application/pdf", pdf.Value.name);
        }));

        // ── Admin: UPLOAD a custom certificate PDF for any credential — examined OR honorary. Gated
        // 'credentials'. Replaces the auto-generated PDF: the uploaded file is stored (encrypted at rest,
        // in the protected 'certificates' category) and served by the same student/admin download routes.
        // 'Regenerate PDF' can still overwrite it with a freshly generated one if the operator wants. ──
        app.MapPost("/api/admin/credentials/{credId}/upload-certificate", async (HttpContext ctx, string credId) =>
        {
            var b = await H.Body(ctx.Request);   // read the body before entering the (synchronous) gate
            return gate(ctx.Request, "credentials", adm =>
            {
                var (bytes, mime, err) = Storage.DecodeDataUri(H.GetS(b, "data_uri"));
                if (bytes is null) return Results.Json(new { error = err ?? "no_file" }, statusCode: 400);
                if (mime != "application/pdf") return Results.Json(new { error = "pdf_required", message = "Upload a PDF certificate." }, statusCode: 400);
                var isHon = credId.StartsWith(Endpoints.Honorary.AwardPrefix + "-", StringComparison.OrdinalIgnoreCase);
                var exists = isHon
                    ? db.QueryOne("SELECT award_no FROM honorary_awards WHERE award_no=?", credId) is not null
                    : db.QueryOne("SELECT credential_id FROM issued_credentials WHERE credential_id=?", credId) is not null;
                if (!exists) return Results.Json(new { error = "not_found", message = "No credential with that id." }, statusCode: 404);
                var stored = Storage.Put(bytes, mime, "certificates");
                if (isHon)
                    db.Execute("UPDATE honorary_awards SET pdf_ref=?, pdf_sha256=?, pdf_generated_at=datetime('now') WHERE award_no=?", stored.Reference, stored.Sha256, credId);
                else
                    db.Execute("UPDATE issued_credentials SET pdf_ref=?, pdf_sha256=?, pdf_generated_at=datetime('now') WHERE credential_id=?", stored.Reference, stored.Sha256, credId);
                log(adm.Id, "certificate_uploaded", credId + " sha " + stored.Sha256[..12]);
                Audit(credId, null, "admin#" + adm.Id, "admin", Ip(ctx), isHon ? "honorary" : "certificate", "uploaded");
                return Results.Json(new { ok = true, sha256 = stored.Sha256, bytes = bytes.LongLength });
            });
        });
    }
}
