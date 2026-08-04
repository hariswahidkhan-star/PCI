using Microsoft.AspNetCore.Http.Features;
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
///   flagged PDFs: "{name} | PCI Student Number: PCI-YYYY-NNNNNN | {designation}" with a
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

        // Raise the request-body cap for this one admin upload route (the global cap stays tight).
        // Without it the global 6 MB Kestrel limit rejects the request before the handler runs, so a
        // book over ~4.4 MB decoded answers 413 and DocStore's 25 MiB allowance is unreachable — which
        // is every Body of Knowledge this module exists to carry.
        //
        // Raised inside the gate, not before it as Documents.cs does: GateFn only reads the
        // Authorization header, so the cap is still lifted before H.Body ever touches the stream, and
        // an unauthenticated or under-permissioned caller stays held at the tight global cap.
        static void AllowUpload(HttpContext ctx)
        {
            var f = ctx.Features.Get<IHttpMaxRequestBodySizeFeature>();
            if (f is not null && !f.IsReadOnly) f.MaxRequestBodySize = 40_000_000; // ~25 MiB as base64 + JSON
        }

        // ── Admin: upload/replace the file behind a book (or create the row + file in one call) ──
        app.MapPost("/api/admin/cert-documents/upload", (HttpContext ctx) => gate(ctx.Request, "resources", adm =>
        {
            AllowUpload(ctx);
            var req = ctx.Request;
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

            // Replacing an existing file snapshots the outgoing one into cert_document_versions first,
            // so a book's bytes are never silently lost: the history lists every superseded file with
            // who replaced it and why, and any snapshot can be viewed or restored. The audit-log line
            // additionally records the old→new checksum for grep-ability.
            var prevSha = existing is null ? null : db.Scalar<string>("SELECT sha256 FROM cert_documents WHERE id=?", id);
            var stored = DocStore.Put(file);
            if (existing is not null)
                SnapshotCurrentFile(id, adm.Id, H.GetS(b, "reason", "replace_reason"), null, stored.Sha256);
            db.Execute("UPDATE cert_documents SET storage_ref=?, filename=?, mime=?, size_bytes=?, sha256=?, updated_at=datetime('now') WHERE id=?",
                stored.Reference, DocStore.SafeName(H.GetS(b, "filename"), file.Ext), file.Mime, file.Bytes.LongLength, stored.Sha256, id);
            log(adm.Id, "cert_document_file_uploaded", $"cert_document {id} ({file.Mime}, {file.Bytes.LongLength} bytes)"
                + (string.IsNullOrEmpty(prevSha) || prevSha == stored.Sha256 ? "" : $" replaced sha256 {prevSha[..Math.Min(prevSha.Length, 12)]}… → {stored.Sha256[..12]}…"));
            var row = db.QueryOne("SELECT id,certification_id,kind,title,watermark,published,filename,mime,size_bytes,sha256 FROM cert_documents WHERE id=?", id)!;
            return J(new { ok = true, row });
        }));

        // Snapshot the book's CURRENT file (if any) into cert_document_versions as the next version
        // number. Skipped when there is no file or the incoming bytes are identical (no-op replace).
        void SnapshotCurrentFile(long docId, long adminId, string? reason, long? restoredFromId, string? incomingSha)
        {
            var cur = db.QueryOne("SELECT storage_ref,filename,mime,size_bytes,sha256 FROM cert_documents WHERE id=?", docId);
            if (cur is null || H.Str(cur["storage_ref"]) is not { Length: > 0 }) return;
            if (incomingSha is not null && H.Str(cur["sha256"]) == incomingSha) return;
            var nextV = db.Scalar<long>("SELECT COALESCE(MAX(version),0)+1 FROM cert_document_versions WHERE cert_document_id=?", docId);
            db.Execute(@"INSERT INTO cert_document_versions(cert_document_id,version,storage_ref,filename,mime,size_bytes,sha256,replaced_by,replace_reason,restored_from_id)
                VALUES(?,?,?,?,?,?,?,?,?,?)",
                docId, nextV, cur["storage_ref"], cur["filename"], cur["mime"], cur["size_bytes"], cur["sha256"], adminId, reason, restoredFromId);
        }

        // Per-certification admin scoping shared by the history routes.
        IResult? DenyCertScope(AdminCtx adm, Dictionary<string, object?> doc) =>
            adm.CanCert(doc["certification_id"] is null ? Certs.DefaultId : H.L(doc["certification_id"]))
                ? null : Results.Json(new { error = "certification_forbidden" }, statusCode: 403);

        // ── Admin: superseded-file history for a book ──
        app.MapGet("/api/admin/cert-documents/{id:long}/versions", (HttpRequest req, long id) => gate(req, "resources", adm =>
        {
            var d = db.QueryOne("SELECT certification_id FROM cert_documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (DenyCertScope(adm, d) is { } deny) return deny;
            return J(new { rows = db.Query("SELECT id,version,filename,mime,size_bytes,sha256,replaced_by,replace_reason,restored_from_id,created_at FROM cert_document_versions WHERE cert_document_id=? ORDER BY version DESC", id) });
        }));

        // ── Admin: view/download a superseded file (logged, inline-capable) ──
        app.MapGet("/api/admin/cert-documents/{id:long}/versions/{vid:long}/file", (HttpContext ctx, long id, long vid) => gate(ctx.Request, "resources", adm =>
        {
            var d = db.QueryOne("SELECT certification_id FROM cert_documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (DenyCertScope(adm, d) is { } deny) return deny;
            var v = db.QueryOne("SELECT storage_ref,filename,mime FROM cert_document_versions WHERE id=? AND cert_document_id=?", vid, id);
            if (v is null) return Results.Json(new { error = "version_not_found" }, statusCode: 404);
            var bytes = DocStore.Get(H.Str(v["storage_ref"]));
            if (bytes is null) return Results.Json(new { error = "file_missing" }, statusCode: 404);
            log(adm.Id, "cert_document_version_view", $"cert_document {id} version row {vid}");
            var mime = H.Str(v["mime"]) ?? "application/octet-stream";
            var name = H.Str(v["filename"]) ?? ("book-" + id + "-v" + vid + ".pdf");
            if (ctx.Request.Query["inline"].ToString() == "1")
            {
                ctx.Response.Headers["Content-Disposition"] = "inline; filename=\"" + name.Replace("\"", "") + "\"";
                return Results.Bytes(bytes, mime);
            }
            return Results.Bytes(bytes, mime, name);
        }));

        // ── Admin: restore a superseded file as the book's current file ──
        // The current file is snapshotted first, so restore never erases history either — it only
        // moves the "current" pointer, exactly like the assigned-documents restore.
        app.MapPost("/api/admin/cert-documents/{id:long}/restore", (HttpRequest req, long id) => gate(req, "resources", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var d = db.QueryOne("SELECT certification_id FROM cert_documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (DenyCertScope(adm, d) is { } deny) return deny;
            var vid = (long)(H.GetNum(b, "version_id") ?? 0);
            var v = db.QueryOne("SELECT * FROM cert_document_versions WHERE id=? AND cert_document_id=?", vid, id);
            if (v is null) return Results.Json(new { error = "version_not_found" }, statusCode: 404);
            if (H.Str(v["storage_ref"]) is not { Length: > 0 } vref) return Results.Json(new { error = "file_missing" }, statusCode: 404);
            var reason = H.GetS(b, "reason") ?? $"Restored from v{H.Ln(v["version"]) ?? 0}";
            SnapshotCurrentFile(id, adm.Id, reason, vid, H.Str(v["sha256"]));
            db.Execute("UPDATE cert_documents SET storage_ref=?, filename=?, mime=?, size_bytes=?, sha256=?, updated_at=datetime('now') WHERE id=?",
                vref, v["filename"], v["mime"], v["size_bytes"], v["sha256"], id);
            log(adm.Id, "cert_document_restore", $"cert_document {id} ← version row {vid} (v{H.Ln(v["version"])})");
            return J(new { ok = true, restored_from = vid });
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
                // The canonical public Student Number — never users.id. The database primary key is
                // an internal identifier; printing it on a distributable PDF both mislabels it and
                // leaks a value that other endpoints treat as non-public. A user who somehow has no
                // number yet is watermarked without one rather than with the wrong one.
                var studentNo = StudentNumbers.Read(db, u.Id);
                var wm = PdfWatermark.Apply(bytes,
                    $"{fullName}{(studentNo is null ? "" : $" | PCI Student Number: {studentNo}")} | {designation}",
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
