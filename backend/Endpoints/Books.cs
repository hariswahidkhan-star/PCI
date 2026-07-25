using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Books &amp; study materials delivery for cert_documents (candidate handbooks, Bodies of Knowledge,
/// study guides). Two halves:
///
///   Admin upload — attaches real file bytes to a cert_documents row through the same hardened
///   intake as the assigned-documents module (type allow-list, size cap, magic-byte check, malware
///   seam), stored privately; `url` stays for plain external links.
///
///   Student download — authenticated, entitlement-scoped (same visibility rule as the
///   /api/me/cert-documents list), optional route restriction, and a per-recipient watermark on
///   flagged PDFs: "{name} | PCI Student ID: NNNNNN | {designation}" with a
///   "Personal Copy — Not for Redistribution" footer carrying a stable per-copy id and the download
///   date. The stored master is never modified and never exposed; an unparseable PDF falls back to
///   the original with the audit recording it. Every download lands in cert_document_downloads.
/// </summary>
public static class Books
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string Ip(HttpContext c) => c.Connection.RemoteIpAddress?.ToString() ?? "";
        void DlAudit(long docId, long userId, string? copyId, string result, string ip) =>
            db.Execute("INSERT INTO cert_document_downloads(cert_document_id,user_id,copy_id,result,ip) VALUES(?,?,?,?,?)",
                docId, userId, copyId, result, ip);

        // ── Admin: upload/replace the file behind a book (or create the row + file in one call) ──
        app.MapPost("/api/admin/cert-documents/upload", (HttpRequest req) => gate(req, "resources", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var (file, err) = DocStore.Decode(H.GetS(b, "file"));
            if (file is null) return Results.Json(new { error = err ?? "file_required" }, statusCode: 400);
            if (!DocStore.ScanClean(file.Bytes, file.Mime, out var reason))
                return Results.Json(new { error = "file_rejected", message = reason }, statusCode: 400);

            long id;
            var existing = b.ContainsKey("id") ? db.QueryOne("SELECT id,certification_id FROM cert_documents WHERE id=?", H.GetNum(b, "id")) : null;
            if (existing is not null)
            {
                id = H.L(existing["id"]);
                if (!adm.CanCert(existing["certification_id"] is null ? Certs.DefaultId : H.L(existing["certification_id"])))
                    return Results.Json(new { error = "certification_forbidden" }, statusCode: 403);
            }
            else
            {
                var certId = Certs.TryResolve(db, H.GetS(b, "certification_id", "certification"));
                if (H.GetS(b, "certification_id", "certification") is { Length: > 0 } && certId is null)
                    return Results.Json(new { error = "bad_certification" }, statusCode: 400);
                if (certId is not null && !adm.CanCert(certId.Value))
                    return Results.Json(new { error = "certification_forbidden" }, statusCode: 403);
                var title = (H.GetS(b, "title") ?? "").Trim();
                if (title.Length == 0) return Results.Json(new { error = "title_required" }, statusCode: 400);
                id = db.ExecuteReturningId(@"INSERT INTO cert_documents(certification_id,kind,title,description,route_key,watermark,published,sort_order)
                    VALUES(?,?,?,?,?,?,?,?)",
                    certId, H.GetS(b, "kind") ?? "book", title, H.GetS(b, "description"), H.GetS(b, "route_key"),
                    b.ContainsKey("watermark") && H.B(b["watermark"].GetRawText()) ? 1 : 0,
                    b.ContainsKey("published") && !H.B(b["published"].GetRawText()) ? 0 : 1,
                    (long)(H.GetNum(b, "sort_order") ?? 0));
            }

            // Replacement provenance: the previous checksum is recorded in the audit log before the
            // pointer moves, and content-addressed storage keeps the old bytes — so a prior file
            // remains identifiable and recoverable even though cert_documents has no version chain.
            var prevSha = existing is null ? null : db.Scalar<string>("SELECT sha256 FROM cert_documents WHERE id=?", id);
            var stored = DocStore.Put(file);
            db.Execute("UPDATE cert_documents SET storage_ref=?, filename=?, mime=?, size_bytes=?, sha256=?, updated_at=datetime('now') WHERE id=?",
                stored.Reference, DocStore.SafeName(H.GetS(b, "filename"), file.Ext), file.Mime, file.Bytes.LongLength, stored.Sha256, id);
            log(adm.Id, "cert_document_file_uploaded", $"cert_document {id} ({file.Mime}, {file.Bytes.LongLength} bytes)"
                + (string.IsNullOrEmpty(prevSha) || prevSha == stored.Sha256 ? "" : $" replaced sha256 {prevSha[..Math.Min(prevSha.Length, 12)]}… → {stored.Sha256[..12]}…"));
            var row = db.QueryOne("SELECT id,certification_id,kind,title,watermark,published,filename,mime,size_bytes,sha256 FROM cert_documents WHERE id=?", id)!;
            return J(new { ok = true, row });
        }));

        // ── Student: authenticated, entitlement-scoped, watermarked download ──
        app.MapGet("/api/me/cert-documents/{id:long}/download", (HttpContext ctx, long id) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var d = db.QueryOne("SELECT * FROM cert_documents WHERE id=?", id);
            if (d is null || H.L(d["published"]) != 1)
            { DlAudit(id, u.Id, null, "not_found", Ip(ctx)); return Results.Json(new { error = "not_found" }, statusCode: 404); }

            // Same visibility rule as the list: general documents, or a certification the student holds
            // an entitlement or credential for.
            var certId = d["certification_id"] is null ? (long?)null : H.L(d["certification_id"]);
            if (certId is not null)
            {
                var entitled = db.QueryOne(@"SELECT 1 x FROM exam_entitlements WHERE user_id=? AND COALESCE(certification_id,1)=?
                    UNION SELECT 1 FROM issued_credentials WHERE user_id=? AND COALESCE(certification_id,1)=? LIMIT 1",
                    u.Id, certId, u.Id, certId);
                if (entitled is null)
                { DlAudit(id, u.Id, null, "not_entitled", Ip(ctx)); return Results.Json(new { error = "not_entitled", message = "This material belongs to a certification you are not enrolled in." }, statusCode: 403); }
            }
            // Optional route restriction: the material is limited to candidates who came through that route.
            if (H.Str(d["route_key"]) is { Length: > 0 } rk)
            {
                var viaRoute = db.QueryOne(@"SELECT 1 x FROM exam_entitlements WHERE user_id=? AND route_key=? AND COALESCE(certification_id,1)=COALESCE(?,COALESCE(certification_id,1))
                    UNION SELECT 1 FROM issued_credentials WHERE user_id=? AND route_key=? LIMIT 1",
                    u.Id, rk, certId, u.Id, rk);
                if (viaRoute is null)
                { DlAudit(id, u.Id, null, "route_restricted", Ip(ctx)); return Results.Json(new { error = "route_restricted", message = "This material is limited to a different application route." }, statusCode: 403); }
            }

            // Link-only rows redirect to their external URL (nothing to watermark or serve).
            if (H.Str(d["storage_ref"]) is not { Length: > 0 } sref)
            {
                if (H.Str(d["url"]) is { Length: > 0 } url)
                { DlAudit(id, u.Id, null, "redirect", Ip(ctx)); return Results.Redirect(url); }
                DlAudit(id, u.Id, null, "file_missing", Ip(ctx));
                return Results.Json(new { error = "file_missing" }, statusCode: 404);
            }
            var bytes = DocStore.Get(sref);
            if (bytes is null) { DlAudit(id, u.Id, null, "file_missing", Ip(ctx)); return Results.Json(new { error = "file_missing" }, statusCode: 500); }
            var mime = H.Str(d["mime"]) ?? "application/octet-stream";

            // Stable per-copy id: the same student re-downloading gets the same copy number, so a
            // leaked file traces to exactly one grant.
            var copyId = Security.Sha($"pci-copy|{id}|{u.Id}")[..10].ToUpperInvariant();
            var wmResult = "ok";
            if (H.L(d["watermark"]) == 1 && mime == "application/pdf")
            {
                var fullName = ($"{u.FirstName} {u.LastName}").Trim();
                if (fullName.Length == 0) fullName = u.Email;
                var cert = certId is null ? null : Certs.ById(db, certId.Value);
                var designation = H.Str(cert?["acronym"]) ?? H.Str(cert?["code"]) ?? "PCI";
                var wm = PdfWatermark.Apply(bytes,
                    $"{fullName} | PCI Student ID: {u.Id:D6} | {designation}",
                    $"Personal Copy - Not for Redistribution | Copy {copyId} | Downloaded {DateTime.UtcNow:yyyy-MM-dd}");
                if (wm is not null) { bytes = wm; wmResult = "ok_watermarked"; } else wmResult = "ok_unwatermarked";
            }
            // `?inline=1` serves for the in-app viewer (still watermarked, still audited — the copy id
            // makes a screen-captured page as traceable as a saved file).
            var inline = ctx.Request.Query["inline"].ToString() == "1";
            DlAudit(id, u.Id, copyId, inline ? wmResult + "_view" : wmResult, Ip(ctx));
            var name = H.Str(d["filename"]) ?? ("book-" + id + ".pdf");
            if (inline)
            {
                ctx.Response.Headers["Content-Disposition"] = "inline; filename=\"" + name.Replace("\"", "") + "\"";
                return Results.Bytes(bytes, mime);
            }
            return Results.Bytes(bytes, mime, name);
        });

        // ── Admin: view/download the stored file behind a book (verify what was uploaded) ──
        app.MapGet("/api/admin/cert-documents/{id:long}/file", (HttpContext ctx, long id) => gate(ctx.Request, "resources", adm =>
        {
            var d = db.QueryOne("SELECT certification_id,storage_ref,filename,mime FROM cert_documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (!adm.CanCert(d["certification_id"] is null ? Certs.DefaultId : H.L(d["certification_id"])))
                return Results.Json(new { error = "certification_forbidden" }, statusCode: 403);
            var bytes = DocStore.Get(H.Str(d["storage_ref"]));
            if (bytes is null) return Results.Json(new { error = "file_missing" }, statusCode: 404);
            log(adm.Id, "cert_document_file_view", $"cert_document {id}");
            var mime = H.Str(d["mime"]) ?? "application/octet-stream";
            var name = H.Str(d["filename"]) ?? ("book-" + id + ".pdf");
            if (ctx.Request.Query["inline"].ToString() == "1")
            {
                ctx.Response.Headers["Content-Disposition"] = "inline; filename=\"" + name.Replace("\"", "") + "\"";
                return Results.Bytes(bytes, mime);
            }
            return Results.Bytes(bytes, mime, name);
        }));
    }
}
