using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Student Documents &amp; Resources module. Admins upload documents into private, content-addressed
/// storage, assign them to a student or a resolved group, publish them (which materialises per-student
/// grants and notifies), and version them without ever overwriting history. Students see only the
/// documents assigned to them and download them over an authenticated, audited channel; some require an
/// acknowledgement first, some are locked until a date, some are view-only. Customer-service staff get a
/// read-only window into a student's documents. Every view/download and acknowledgement is audited.
///
/// This is distinct from the existing URL-based public <c>resources</c> list — those are public links;
/// these are private, per-student assigned FILES with a full access-control and audit model.
/// </summary>
public static class Documents
{
    // The eleven lifecycle statuses. Publish/expiry/scheduling are driven by dedicated actions; the
    // generic status action covers the rest with validation.
    static readonly HashSet<string> AllStatuses = new(StringComparer.OrdinalIgnoreCase)
    { "draft", "pending", "published", "active", "scheduled", "expired", "archived", "replaced", "suspended", "rejected", "test" };

    // Short-lived signed download links (time-limited, no Authorization header needed) are signed with a
    // boot-stable server secret derived from existing configuration.
    static readonly string LinkSecret =
        (Environment.GetEnvironmentVariable("DOC_LINK_SECRET") is { Length: > 0 } s ? s
         : (Environment.GetEnvironmentVariable("CREDENTIAL_ENCRYPTION_KEY") ?? "")
           + "|" + (Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET") ?? "")
           + "|pci-document-link-v1");

    static string SignLink(long docId, long uid, long expUnix)
        => $"{docId}.{uid}.{expUnix}." + Security.Sha($"{docId}.{uid}.{expUnix}.{LinkSecret}");

    static (long docId, long uid)? VerifyLink(string? token)
    {
        if (string.IsNullOrEmpty(token)) return null;
        var p = token.Split('.');
        if (p.Length != 4 || !long.TryParse(p[0], out var docId) || !long.TryParse(p[1], out var uid) || !long.TryParse(p[2], out var exp)) return null;
        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > exp) return null;
        if (!Security.FixedTimeEquals(p[3], Security.Sha($"{docId}.{uid}.{exp}.{LinkSecret}"))) return null;
        return (docId, uid);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        static string? Ip(HttpContext ctx) => ctx.Connection.RemoteIpAddress?.ToString();
        UserCtx? User(HttpRequest r) => Auth.UserFromReq(r, db);

        // Admin section guard (async handlers): null = allowed, else the 401/403 result. Accepts ANY of
        // the listed permissions so customer-service (read paths) and document managers can share a route.
        IResult? Deny(HttpRequest req, params string[] anyOf)
        {
            var a = adminFromReq(req);
            if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            if (a.IsOwner) return null;
            foreach (var s in anyOf) if (a.Perms.Contains(s)) return null;
            return Results.Json(new { error = "forbidden", section = anyOf.FirstOrDefault() }, statusCode: 403);
        }

        // Raise the request-body cap for the admin upload routes only (the global cap stays tight).
        static void AllowUpload(HttpContext ctx)
        {
            var f = ctx.Features.Get<IHttpMaxRequestBodySizeFeature>();
            if (f is not null && !f.IsReadOnly) f.MaxRequestBodySize = 40_000_000; // ~25 MiB file as base64 + JSON
        }

        void DlAudit(long? docId, long? uid, string actor, string role, string? ip, string action, string result, long? version = null)
        { try { db.Execute("INSERT INTO document_downloads(document_id,user_id,actor,role,ip,action,result,version) VALUES(?,?,?,?,?,?,?,?)", docId, uid, actor, role, ip, action, result, version); } catch { } }

        // Effective availability for a live document: is it downloadable right now?
        (bool ok, string? reason) Available(Dictionary<string, object?> d)
        {
            var status = (H.Str(d["status"]) ?? "").ToLowerInvariant();
            if (!DocAccess.LiveStatuses.Contains(status)) return (false, status is "suspended" or "archived" or "replaced" or "rejected" or "scheduled" or "expired" ? status : "unavailable");
            if (H.Str(d["publish_at"]) is { Length: > 0 } pa && !H.IsPast(pa)) return (false, "scheduled");
            if (H.Str(d["expires_at"]) is { Length: > 0 } ex && H.IsPast(ex)) return (false, "expired");
            return (true, null);
        }

        // Notify every active assignee of a freshly-published document: in-app for all (reliable), email
        // best-effort and capped. Never throws.
        void NotifyAssignees(long docId, string title)
        {
            List<Dictionary<string, object?>> assignees;
            try { assignees = db.Query("SELECT a.user_id, u.email, u.first_name FROM document_assignments a JOIN users u ON u.id=a.user_id WHERE a.document_id=? AND a.status='active'", docId); }
            catch { return; }
            var cap = (int)Settings.Num(db, "doc_email_notify_cap", 500);
            var emailOn = Notify.Enabled(db, "documents");
            var emailed = 0;
            foreach (var a in assignees)
            {
                var uid = H.L(a["user_id"]);
                try { db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Documents', 'New document available', ?, 'Open My Documents', '/documents')",
                    uid, $"“{title}” is now available in your documents."); } catch { }
                if (emailOn && emailed < cap && H.Str(a["email"]) is { Length: > 0 } to)
                {
                    var name = H.Str(a["first_name"]);
                    var html = $"<p>Hello {System.Net.WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(name) ? "there" : name)},</p>" +
                               $"<p>A new document, <strong>{System.Net.WebUtility.HtmlEncode(title)}</strong>, is now available in your student portal.</p>" +
                               "<p>Sign in and open <em>My Documents</em> to view or download it.</p>";
                    Notify.Email(db, uid, to, "A new document is available", html, "document", docId);
                    emailed++;
                }
            }
        }

        // ─────────────────────────── Admin: categories ───────────────────────────
        app.MapGet("/api/admin/document-categories", (HttpRequest req) => gate(req, "documents", _ =>
            J(new { rows = db.Query("SELECT * FROM document_categories ORDER BY sort_order, id") })));
        app.MapPost("/api/admin/document-categories", (HttpRequest req) => gate(req, "documents", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var name = (H.GetS(b, "name") ?? "").Trim();
            if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
            var id = db.ExecuteReturningId("INSERT INTO document_categories(name,slug,description,active,sort_order) VALUES(?,?,?,?,?)",
                name, (H.GetS(b, "slug") ?? name.ToLowerInvariant().Replace(' ', '-')), H.GetS(b, "description"),
                b.ContainsKey("active") && !H.B(b["active"].GetRawText()) ? 0 : 1, (long)(H.GetNum(b, "sort_order") ?? 0));
            log(adm.Id, "document_category_create", name);
            return J(new { id });
        }));
        app.MapPatch("/api/admin/document-categories/{id}", (HttpRequest req, long id) => gate(req, "documents", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var set = new List<string>(); var vals = new List<object?>();
            if (b.ContainsKey("name")) { set.Add("name=?"); vals.Add(H.GetS(b, "name")); }
            if (b.ContainsKey("description")) { set.Add("description=?"); vals.Add(H.GetS(b, "description")); }
            if (b.ContainsKey("active")) { set.Add("active=?"); vals.Add(H.B(b["active"].GetRawText()) ? 1 : 0); }
            if (b.ContainsKey("sort_order")) { set.Add("sort_order=?"); vals.Add((long)(H.GetNum(b, "sort_order") ?? 0)); }
            if (set.Count == 0) return J(new { ok = true });
            vals.Add(id);
            db.Execute($"UPDATE document_categories SET {string.Join(",", set)} WHERE id=?", vals.ToArray());
            log(adm.Id, "document_category_update", id.ToString());
            return J(new { ok = true });
        }));
        app.MapDelete("/api/admin/document-categories/{id}", (HttpRequest req, long id) => gate(req, "documents", adm =>
        {
            db.Execute("DELETE FROM document_categories WHERE id=?", id);
            log(adm.Id, "document_category_delete", id.ToString());
            return J(new { ok = true });
        }));

        // ─────────────────────────── Admin: list + detail ───────────────────────────
        string LikeQ(string? q) => "%" + System.Text.RegularExpressions.Regex.Replace(q ?? "", "[%_]", "") + "%";
        app.MapGet("/api/admin/documents", (HttpRequest req) => gate(req, "documents", _ =>
        {
            var status = req.Query["status"].ToString(); var category = req.Query["category"].ToString(); var q = req.Query["q"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(status)) { where.Add("d.status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(category)) { where.Add("d.category=?"); args.Add(category); }
            if (!string.IsNullOrEmpty(q)) { where.Add("(d.title LIKE ? OR d.description LIKE ?)"); args.Add(LikeQ(q)); args.Add(LikeQ(q)); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            var rows = db.Query($@"SELECT d.*,
                (SELECT COUNT(*) FROM document_assignments a WHERE a.document_id=d.id AND a.status='active') assigned_count,
                (SELECT COUNT(*) FROM document_acknowledgements k WHERE k.document_id=d.id) ack_count,
                (SELECT COUNT(*) FROM document_downloads dl WHERE dl.document_id=d.id AND dl.action='download' AND dl.result LIKE 'ok%') download_count
                FROM documents d {w} ORDER BY d.id DESC", args.ToArray());
            return J(new { rows });
        }));
        app.MapGet("/api/admin/documents/{id}", (HttpRequest req, long id) => gate(req, "documents", _ =>
        {
            var d = db.QueryOne("SELECT * FROM documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var rootId = H.Ln(d["root_id"]) ?? id;
            var versions = db.Query("SELECT id,version,filename,mime,size_bytes,sha256,status,created_at FROM documents WHERE root_id=? OR id=? ORDER BY version", rootId, rootId);
            var recipients = db.Query(@"SELECT a.user_id, a.assignment_type, a.source, a.status, a.assigned_at, a.revoked_at,
                    u.first_name, u.last_name, u.email,
                    (SELECT acknowledged_at FROM document_acknowledgements k WHERE k.document_id=a.document_id AND k.user_id=a.user_id) acknowledged_at,
                    (SELECT MAX(created_at) FROM document_downloads dl WHERE dl.document_id=a.document_id AND dl.user_id=a.user_id AND dl.action='download' AND dl.result LIKE 'ok%') last_download
                FROM document_assignments a JOIN users u ON u.id=a.user_id WHERE a.document_id=? ORDER BY a.status, a.id DESC", id);
            return J(new { document = d, versions, recipients });
        }));

        // ─────────────────────────── Admin: create / upload ───────────────────────────
        // Shared creation logic. Returns the new document id (in 'draft' unless publishNow).
        long CreateDocument(AdminCtx adm, Dictionary<string, JsonElement> b, out IResult? error, long? forceAssignUser = null)
        {
            error = null;
            var title = (H.GetS(b, "title") ?? "").Trim();
            if (title.Length == 0) { error = Results.Json(new { error = "title_required", message = "A document title is required." }, statusCode: 400); return 0; }
            var dataUri = H.GetS(b, "file", "data", "file_data");
            var dec = DocStore.Decode(dataUri);
            if (dec.file is null)
            {
                error = Results.Json(new { error = dec.error, message = DocErrorMessage(dec.error) }, statusCode: 400);
                return 0;
            }
            var stored = DocStore.Put(dec.file);
            var filename = DocStore.SafeName(H.GetS(b, "filename", "file_name") ?? title, dec.file.Ext);

            var assignType = forceAssignUser is not null ? "student" : (H.GetS(b, "assignment_type") ?? "all");
            if (!DocAccess.IsValidType(assignType)) { error = Results.Json(new { error = "bad_assignment_type" }, statusCode: 400); return 0; }
            var assignConfig = forceAssignUser is not null
                ? JsonSerializer.Serialize(new { user_id = forceAssignUser.Value })
                : ConfigJson(b);
            var includeTest = b.ContainsKey("include_test") && H.B(b["include_test"].GetRawText());
            var isTest = (b.ContainsKey("is_test") && H.B(b["is_test"].GetRawText())) || assignType == "test_users";

            var id = db.ExecuteReturningId(@"INSERT INTO documents(title,description,category,doc_type,status,storage_ref,filename,mime,size_bytes,sha256,version,
                    assignment_type,assignment_config,view_only,restricted_until,ack_required,watermark,include_test,publish_at,expires_at,visible_to_cs,created_by,is_test)
                VALUES(?,?,?,?, 'draft', ?,?,?,?,?, 1, ?,?,?,?,?,?,?,?,?,?,?,?)",
                title, H.GetS(b, "description"), H.GetS(b, "category"), H.GetS(b, "doc_type") ?? "general",
                stored.Reference, filename, dec.file.Mime, stored.SizeBytes, dec.file.Sha256,
                assignType, assignConfig,
                b.ContainsKey("view_only") && H.B(b["view_only"].GetRawText()) ? 1 : 0,
                H.GetS(b, "restricted_until"),
                b.ContainsKey("ack_required") && H.B(b["ack_required"].GetRawText()) ? 1 : 0,
                b.ContainsKey("watermark") && H.B(b["watermark"].GetRawText()) ? 1 : 0,
                includeTest ? 1 : 0,
                H.GetS(b, "publish_at"), H.GetS(b, "expires_at"),
                b.ContainsKey("visible_to_cs") && !H.B(b["visible_to_cs"].GetRawText()) ? 0 : 1,
                adm.Id, isTest ? 1 : 0);
            // v1 is the root of its own version chain.
            db.Execute("UPDATE documents SET root_id=? WHERE id=?", id, id);
            log(adm.Id, "document_create", $"{title} (id {id})");
            return id;
        }

        app.MapPost("/api/admin/documents", async (HttpContext ctx) =>
        {
            AllowUpload(ctx);
            var g = Deny(ctx.Request, "documents"); if (g is not null) return g;
            var adm = adminFromReq(ctx.Request)!;
            var b = await H.Body(ctx.Request);
            var id = CreateDocument(adm, b, out var err);
            if (err is not null) return err;
            // Optional immediate publish.
            if (b.ContainsKey("publish") && H.B(b["publish"].GetRawText()))
                return DoPublish(id, adm);
            return J(new { id, status = "draft" });
        });

        // ─────────────────────────── Admin: edit metadata ───────────────────────────
        app.MapPatch("/api/admin/documents/{id}", (HttpRequest req, long id) => gate(req, "documents", adm =>
        {
            var d = db.QueryOne("SELECT id FROM documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(req).GetAwaiter().GetResult();
            var set = new List<string>(); var vals = new List<object?>();
            void Str(string col, params string[] keys) { if (keys.Any(b.ContainsKey)) { set.Add(col + "=?"); vals.Add(H.GetS(b, keys)); } }
            void Flag(string col) { if (b.ContainsKey(col)) { set.Add(col + "=?"); vals.Add(H.B(b[col].GetRawText()) ? 1 : 0); } }
            Str("title", "title"); Str("description", "description"); Str("category", "category"); Str("doc_type", "doc_type");
            Str("restricted_until", "restricted_until"); Str("publish_at", "publish_at"); Str("expires_at", "expires_at");
            Flag("view_only"); Flag("ack_required"); Flag("watermark"); Flag("visible_to_cs");
            if (set.Count == 0) return J(new { ok = true });
            set.Add("updated_at=datetime('now')"); set.Add("updated_by=?"); vals.Add(adm.Id);
            vals.Add(id);
            db.Execute($"UPDATE documents SET {string.Join(",", set)} WHERE id=?", vals.ToArray());
            log(adm.Id, "document_update", id.ToString());
            return J(new { ok = true });
        }));

        // ─────────────────────────── Admin: new version (never overwrites) ───────────────────────────
        app.MapPost("/api/admin/documents/{id}/version", async (HttpContext ctx, long id) =>
        {
            AllowUpload(ctx);
            var g = Deny(ctx.Request, "documents"); if (g is not null) return g;
            var adm = adminFromReq(ctx.Request)!;
            var cur = db.QueryOne("SELECT * FROM documents WHERE id=?", id);
            if (cur is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            var dec = DocStore.Decode(H.GetS(b, "file", "data", "file_data"));
            if (dec.file is null) return Results.Json(new { error = dec.error, message = DocErrorMessage(dec.error) }, statusCode: 400);
            var stored = DocStore.Put(dec.file);
            var rootId = H.Ln(cur["root_id"]) ?? id;
            var maxV = db.Scalar<long>("SELECT COALESCE(MAX(version),1) FROM documents WHERE root_id=? OR id=?", rootId, rootId);
            var newV = maxV + 1;
            var title = H.Str(cur["title"]) ?? "document";
            var filename = DocStore.SafeName(H.GetS(b, "filename") ?? title, dec.file.Ext);
            // A new version inherits the prior version's metadata + assignment target, starts published if
            // the prior was live (so recipients keep access seamlessly), and supersedes the old row.
            var priorLive = DocAccess.LiveStatuses.Contains((H.Str(cur["status"]) ?? "").ToLowerInvariant());
            var newId = db.ExecuteReturningId(@"INSERT INTO documents(title,description,category,doc_type,status,storage_ref,filename,mime,size_bytes,sha256,version,root_id,supersedes_id,
                    assignment_type,assignment_config,view_only,restricted_until,ack_required,watermark,include_test,publish_at,expires_at,visible_to_cs,created_by,is_test)
                VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?, ?,?,?,?,?,?,?,?,?,?,?,?)",
                title, cur["description"], cur["category"], cur["doc_type"],
                priorLive ? "published" : "draft",
                stored.Reference, filename, dec.file.Mime, stored.SizeBytes, dec.file.Sha256, newV, rootId, id,
                cur["assignment_type"], cur["assignment_config"], cur["view_only"], cur["restricted_until"], cur["ack_required"],
                cur["watermark"], cur["include_test"], cur["publish_at"], cur["expires_at"], cur["visible_to_cs"], adm.Id, cur["is_test"]);
            db.Execute("UPDATE documents SET status='replaced', superseded_by=?, updated_at=datetime('now'), updated_by=? WHERE id=?", newId, adm.Id, id);
            if (priorLive)
            {
                db.Execute("UPDATE documents SET published_at=datetime('now') WHERE id=?", newId);
                DocAccess.Materialize(db, newId, H.Str(cur["assignment_type"]) ?? "all", H.Str(cur["assignment_config"]), H.L(cur["include_test"]) == 1, adm.Id);
            }
            log(adm.Id, "document_version", $"doc {rootId} → v{newV} (id {newId})");
            return J(new { id = newId, version = newV, status = priorLive ? "published" : "draft" });
        });

        // ─────────────────────────── Admin: assignment target + preview ───────────────────────────
        app.MapPost("/api/admin/documents/preview-recipients", async (HttpContext ctx) =>
        {
            var g = Deny(ctx.Request, "documents"); if (g is not null) return g;
            var b = await H.Body(ctx.Request);
            var type = H.GetS(b, "assignment_type") ?? "all";
            if (!DocAccess.IsValidType(type)) return Results.Json(new { error = "bad_assignment_type" }, statusCode: 400);
            var includeTest = b.ContainsKey("include_test") && H.B(b["include_test"].GetRawText());
            var (count, sample) = DocAccess.Preview(db, type, ConfigJson(b), includeTest);
            return J(new { count, sample });
        });
        app.MapPost("/api/admin/documents/{id}/assign", async (HttpContext ctx, long id) =>
        {
            var g = Deny(ctx.Request, "documents"); if (g is not null) return g;
            var adm = adminFromReq(ctx.Request)!;
            var d = db.QueryOne("SELECT * FROM documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            var type = H.GetS(b, "assignment_type") ?? "all";
            if (!DocAccess.IsValidType(type)) return Results.Json(new { error = "bad_assignment_type" }, statusCode: 400);
            var includeTest = b.ContainsKey("include_test") ? H.B(b["include_test"].GetRawText()) : H.L(d["include_test"]) == 1;
            var config = ConfigJson(b);
            db.Execute("UPDATE documents SET assignment_type=?, assignment_config=?, include_test=?, updated_at=datetime('now'), updated_by=? WHERE id=?",
                type, config, includeTest ? 1 : 0, adm.Id, id);
            // If the document is already live, re-materialise so newly-qualifying students gain access now.
            var added = 0;
            if (DocAccess.LiveStatuses.Contains((H.Str(d["status"]) ?? "").ToLowerInvariant()))
                added = DocAccess.Materialize(db, id, type, config, includeTest, adm.Id);
            log(adm.Id, "document_assign", $"doc {id} → {type} (+{added})");
            var (count, sample) = DocAccess.Preview(db, type, config, includeTest);
            return J(new { ok = true, resolved = count, newly_granted = added, sample });
        });

        // ─────────────────────────── Admin: publish ───────────────────────────
        IResult DoPublish(long id, AdminCtx adm)
        {
            var d = db.QueryOne("SELECT * FROM documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(d["storage_ref"]) is not { Length: > 0 }) return Results.Json(new { error = "no_file", message = "Upload a file before publishing." }, statusCode: 400);
            var type = H.Str(d["assignment_type"]) ?? "all";
            var config = H.Str(d["assignment_config"]);
            var includeTest = H.L(d["include_test"]) == 1;
            // Scheduled publish: a future publish_at means the document is 'scheduled' (visible/locked) until then.
            var scheduled = H.Str(d["publish_at"]) is { Length: > 0 } pa && !H.IsPast(pa);
            var newStatus = scheduled ? "scheduled" : "published";
            db.Execute("UPDATE documents SET status=?, published_at=datetime('now'), updated_at=datetime('now'), updated_by=? WHERE id=?", newStatus, adm.Id, id);
            var added = DocAccess.Materialize(db, id, type, config, includeTest, adm.Id);
            // Notify only when actually live now (a scheduled document notifies when it goes live — an
            // operator re-publishes at that point, or extends this with a scheduler later).
            if (!scheduled) NotifyAssignees(id, H.Str(d["title"]) ?? "A document");
            log(adm.Id, "document_publish", $"doc {id} status={newStatus} recipients={added}");
            return J(new { ok = true, id, status = newStatus, recipients_granted = added });
        }
        app.MapPost("/api/admin/documents/{id}/publish", (HttpRequest req, long id) => gate(req, "documents", adm => DoPublish(id, adm)));

        // ─────────────────────────── Admin: status change ───────────────────────────
        app.MapPost("/api/admin/documents/{id}/status", async (HttpContext ctx, long id) =>
        {
            var g = Deny(ctx.Request, "documents"); if (g is not null) return g;
            var adm = adminFromReq(ctx.Request)!;
            var d = db.QueryOne("SELECT id,title FROM documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            var status = (H.GetS(b, "status") ?? "").ToLowerInvariant();
            if (!AllStatuses.Contains(status)) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var extra = status == "archived" ? ", archived_at=datetime('now')" : "";
            var reason = status == "rejected" ? H.GetS(b, "reason") : null;
            db.Execute($"UPDATE documents SET status=?, reject_reason=COALESCE(?,reject_reason), updated_at=datetime('now'), updated_by=?{extra} WHERE id=?", status, reason, adm.Id, id);
            // Publishing via this route also materialises + notifies (parity with the dedicated action).
            if (DocAccess.LiveStatuses.Contains(status))
            {
                var full = db.QueryOne("SELECT * FROM documents WHERE id=?", id)!;
                DocAccess.Materialize(db, id, H.Str(full["assignment_type"]) ?? "all", H.Str(full["assignment_config"]), H.L(full["include_test"]) == 1, adm.Id);
            }
            log(adm.Id, "document_status_" + status, id.ToString());
            return J(new { ok = true, status });
        });

        // ─────────────────────────── Admin: revoke access (keeps audit) ───────────────────────────
        app.MapPost("/api/admin/documents/{id}/revoke", async (HttpContext ctx, long id) =>
        {
            var g = Deny(ctx.Request, "documents"); if (g is not null) return g;
            var adm = adminFromReq(ctx.Request)!;
            var d = db.QueryOne("SELECT id FROM documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            var reason = H.GetS(b, "reason");
            var userId = H.GetNum(b, "user_id");
            int n;
            if (userId is not null)
                n = db.Execute("UPDATE document_assignments SET status='revoked', revoked_at=datetime('now'), revoked_by=?, revoke_reason=? WHERE document_id=? AND user_id=? AND status='active'",
                    adm.Id, reason, id, (long)userId.Value);
            else
                n = db.Execute("UPDATE document_assignments SET status='revoked', revoked_at=datetime('now'), revoked_by=?, revoke_reason=? WHERE document_id=? AND status='active'",
                    adm.Id, reason, id);
            log(adm.Id, "document_revoke", $"doc {id} user={(userId is null ? "all" : userId.Value.ToString())} n={n}");
            return J(new { ok = true, revoked = n });
        });

        // ─────────────────────────── Admin: download the file (support/verification) ───────────────────────────
        app.MapGet("/api/admin/documents/{id}/download", (HttpContext ctx, long id) => gate(ctx.Request, "documents", adm =>
        {
            var d = db.QueryOne("SELECT storage_ref,filename,mime FROM documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var bytes = DocStore.Get(H.Str(d["storage_ref"]));
            if (bytes is null) return Results.Json(new { error = "file_missing" }, statusCode: 404);
            DlAudit(id, null, "admin#" + adm.Id, "admin", Ip(ctx), "download", "ok");
            return Results.Bytes(bytes, H.Str(d["mime"]) ?? "application/octet-stream", H.Str(d["filename"]) ?? ("document-" + id));
        }));

        // ─────────────────────────── Admin: audit + report ───────────────────────────
        app.MapGet("/api/admin/documents/{id}/audit", (HttpRequest req, long id) => gate(req, "documents", _ =>
        {
            var d = db.QueryOne("SELECT id,title,status FROM documents WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var summary = db.QueryOne(@"SELECT
                (SELECT COUNT(*) FROM document_assignments a WHERE a.document_id=? AND a.status='active') active_grants,
                (SELECT COUNT(*) FROM document_assignments a WHERE a.document_id=? AND a.status='revoked') revoked_grants,
                (SELECT COUNT(*) FROM document_acknowledgements k WHERE k.document_id=?) acknowledgements,
                (SELECT COUNT(DISTINCT user_id) FROM document_downloads dl WHERE dl.document_id=? AND dl.action='download' AND dl.result LIKE 'ok%') distinct_downloaders,
                (SELECT COUNT(*) FROM document_downloads dl WHERE dl.document_id=? AND dl.action='download' AND dl.result LIKE 'ok%') total_downloads,
                (SELECT COUNT(*) FROM document_downloads dl WHERE dl.document_id=? AND dl.action='view') total_views",
                id, id, id, id, id, id);
            var recent = db.Query("SELECT user_id,actor,role,action,result,version,created_at FROM document_downloads WHERE document_id=? ORDER BY id DESC LIMIT 200", id);
            return J(new { document = d, summary, recent });
        }));

        // ─────────────────────────── Admin/CS: a student's documents (read) ───────────────────────────
        // Customer-service (inbox) and member managers may READ which documents a student holds.
        app.MapGet("/api/admin/students/{userId}/documents", (HttpRequest req, long userId) =>
        {
            var g = Deny(req, "documents", "members", "inbox"); if (g is not null) return g;
            var rows = db.Query(@"SELECT d.id,d.title,d.category,d.doc_type,d.status,d.filename,d.mime,d.size_bytes,d.version,d.view_only,d.ack_required,d.restricted_until,d.expires_at,d.published_at,
                    a.status assignment_status, a.assignment_type, a.source, a.assigned_at, a.revoked_at,
                    (SELECT acknowledged_at FROM document_acknowledgements k WHERE k.document_id=d.id AND k.user_id=a.user_id) acknowledged_at
                FROM document_assignments a JOIN documents d ON d.id=a.document_id WHERE a.user_id=? ORDER BY a.id DESC", userId);
            return J(new { rows });
        });
        // Admin: create a student-specific document straight from the student profile (upload → assign → publish).
        app.MapPost("/api/admin/students/{userId}/documents", async (HttpContext ctx, long userId) =>
        {
            AllowUpload(ctx);
            var g = Deny(ctx.Request, "documents"); if (g is not null) return g;
            var adm = adminFromReq(ctx.Request)!;
            if (db.QueryOne("SELECT id FROM users WHERE id=?", userId) is null) return Results.Json(new { error = "no_user" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            var id = CreateDocument(adm, b, out var err, forceAssignUser: userId);
            if (err is not null) return err;
            // Student-specific documents publish immediately by default (they are private to one student).
            if (!b.ContainsKey("publish") || H.B(b["publish"].GetRawText()))
                return DoPublish(id, adm);
            return J(new { id, status = "draft" });
        });

        // ═══════════════════════════ Student: My Documents ═══════════════════════════
        app.MapGet("/api/me/documents", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            // Only documents assigned (active grant) to this student, in a visible status, newest first.
            // When several versions target the student, the superseded ('replaced') ones are hidden.
            var rows = db.Query(@"SELECT d.id,d.title,d.description,d.category,d.doc_type,d.status,d.filename,d.mime,d.size_bytes,d.version,
                    d.view_only,d.ack_required,d.restricted_until,d.expires_at,d.publish_at,d.published_at,d.watermark,
                    a.assigned_at,
                    (SELECT acknowledged_at FROM document_acknowledgements k WHERE k.document_id=d.id AND k.user_id=a.user_id) acknowledged_at
                FROM document_assignments a JOIN documents d ON d.id=a.document_id
                WHERE a.user_id=? AND a.status='active' AND d.status IN ('published','active','scheduled','expired','test')
                ORDER BY d.id DESC", u.Id);
            var outp = rows.Select(d =>
            {
                var (ok, reason) = Available(d);
                var locked = !ok;
                var restrictedUntil = H.Str(d["restricted_until"]);
                var restricted = restrictedUntil is { Length: > 0 } && !H.IsPast(restrictedUntil);
                return new
                {
                    id = H.L(d["id"]), title = H.Str(d["title"]), description = H.Str(d["description"]),
                    category = H.Str(d["category"]), doc_type = H.Str(d["doc_type"]),
                    filename = H.Str(d["filename"]), mime = H.Str(d["mime"]), size_bytes = H.Ln(d["size_bytes"]),
                    version = H.Ln(d["version"]), view_only = H.L(d["view_only"]) == 1,
                    ack_required = H.L(d["ack_required"]) == 1, acknowledged = d["acknowledged_at"] is not null,
                    restricted_until = restrictedUntil, restricted, expires_at = H.Str(d["expires_at"]),
                    published_at = H.Str(d["published_at"]),
                    downloadable = ok && !restricted && !(H.L(d["ack_required"]) == 1 && d["acknowledged_at"] is null),
                    locked, lock_reason = restricted ? "restricted" : reason,
                };
            }).ToList();
            return J(new { rows = outp });
        });

        // Student: acknowledge a document (records who + when + ip). Idempotent.
        app.MapPost("/api/me/documents/{id}/acknowledge", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (u.Impersonated) return Results.Json(new { error = "impersonation_readonly", message = "This action is disabled in support view." }, statusCode: 403);
            if (DocAccess.ActiveAssignment(db, id, u.Id) is null) return Results.Json(new { error = "not_assigned" }, statusCode: 403);
            db.Execute("INSERT OR IGNORE INTO document_acknowledgements(document_id,user_id,ip) VALUES(?,?,?)", id, u.Id, Ip(ctx));
            log(u.Id, "document_acknowledge", id.ToString());
            return J(new { ok = true, acknowledged_at = db.Scalar<string>("SELECT acknowledged_at FROM document_acknowledgements WHERE document_id=? AND user_id=?", id, u.Id) });
        });

        // Student: mint a short-lived, signed download link (usable without the Authorization header, e.g.
        // to open in a new tab or a view-only viewer). Still re-checks access when redeemed.
        app.MapPost("/api/me/documents/{id}/link", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var d = db.QueryOne("SELECT * FROM documents WHERE id=?", id);
            if (d is null || DocAccess.ActiveAssignment(db, id, u.Id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var (ok, reason) = Available(d);
            if (!ok) return Results.Json(new { error = reason ?? "unavailable" }, statusCode: 403);
            var ttl = 300; // 5 minutes
            var exp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + ttl;
            var token = SignLink(id, u.Id, exp);
            return J(new { url = $"/api/documents/download?t={Uri.EscapeDataString(token)}", expires_in = ttl });
        });

        // Student: authenticated secure download of an assigned document (Bearer). Enforces assignment,
        // status/schedule/expiry, restriction window, and acknowledgement; audits every attempt.
        app.MapGet("/api/me/documents/{id}/download", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return ServeToStudent(ctx, id, u.Id, u.Email, "student");
        });

        // Redeem a signed link (no Authorization header). Same access checks as the authenticated path.
        app.MapGet("/api/documents/download", (HttpContext ctx) =>
        {
            var v = VerifyLink(ctx.Request.Query["t"].ToString());
            if (v is null) return Results.Json(new { error = "invalid_or_expired_link" }, statusCode: 401);
            var email = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", v.Value.uid)?["email"]) ?? "";
            return ServeToStudent(ctx, v.Value.docId, v.Value.uid, email, "student_link");
        });

        // ═══════════════════════════ Institution (partner) portal ═══════════════════════════
        // A document whose audience is `institution` targets a partner two ways at once: its REGISTERED
        // STUDENTS get personal grants on publish (existing behaviour), and the institution's own portal
        // logins can see/download it here — so agreements, invoices and marketing kits reach the partner
        // even before they have any students. Strictly scoped to the partner's own id; nothing else leaks.
        List<Dictionary<string, object?>> PartnerDocs(long partnerId) =>
            db.Query(@"SELECT id,title,description,category,doc_type,status,filename,mime,size_bytes,version,
                       view_only,watermark,restricted_until,publish_at,expires_at,published_at,assignment_config
                       FROM documents WHERE assignment_type='institution'
                       AND status IN ('published','active','scheduled','expired') ORDER BY id DESC")
              .Where(d => ConfigPartnerId(H.Str(d["assignment_config"])) == partnerId).ToList();

        app.MapGet("/api/partner/documents", (HttpContext ctx) =>
        {
            var p = Auth.PartnerFromReq(ctx.Request, db);
            if (p is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var rows = PartnerDocs(p.PartnerId).Select(d =>
            {
                var (ok, reason) = Available(d);
                var ru = H.Str(d["restricted_until"]);
                var restricted = ru is { Length: > 0 } && !H.IsPast(ru);
                return new
                {
                    id = H.L(d["id"]), title = H.Str(d["title"]), description = H.Str(d["description"]),
                    category = H.Str(d["category"]), doc_type = H.Str(d["doc_type"]),
                    filename = H.Str(d["filename"]), mime = H.Str(d["mime"]), size_bytes = H.Ln(d["size_bytes"]),
                    version = H.Ln(d["version"]), view_only = H.L(d["view_only"]) == 1,
                    restricted_until = ru, restricted, published_at = H.Str(d["published_at"]),
                    downloadable = ok && !restricted, locked = !ok || restricted,
                    lock_reason = restricted ? "restricted" : reason,
                };
            }).ToList();
            return J(new { rows });
        });

        app.MapGet("/api/partner/documents/{id}/download", (HttpContext ctx, long id) =>
        {
            var p = Auth.PartnerFromReq(ctx.Request, db);
            if (p is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var d = db.QueryOne("SELECT * FROM documents WHERE id=?", id);
            if (d is null || H.Str(d["assignment_type"]) != "institution" || ConfigPartnerId(H.Str(d["assignment_config"])) != p.PartnerId)
            { DlAudit(id, null, p.Email, "partner", Ip(ctx), "download", "not_assigned"); return Results.Json(new { error = "not_found" }, statusCode: 404); }
            var (ok, reason) = Available(d);
            if (!ok) { DlAudit(id, null, p.Email, "partner", Ip(ctx), "download", "blocked_" + reason); return Results.Json(new { error = reason, message = BlockMessage(reason) }, statusCode: 403); }
            if (H.Str(d["restricted_until"]) is { Length: > 0 } ru && !H.IsPast(ru))
            { DlAudit(id, null, p.Email, "partner", Ip(ctx), "download", "restricted"); return Results.Json(new { error = "restricted", available_from = ru }, statusCode: 403); }
            var bytes = DocStore.Get(H.Str(d["storage_ref"]));
            if (bytes is null) { DlAudit(id, null, p.Email, "partner", Ip(ctx), "download", "file_missing"); return Results.Json(new { error = "file_missing" }, statusCode: 500); }
            var mime = H.Str(d["mime"]) ?? "application/octet-stream";
            // Institution watermark: flagged PDFs carry the licensee's identity, exactly as for students.
            var wmResult = "ok";
            if (H.L(d["watermark"]) == 1 && mime == "application/pdf")
            {
                var pname = H.Str(db.QueryOne("SELECT name FROM training_partners WHERE id=?", p.PartnerId)?["name"]) ?? ("institution #" + p.PartnerId);
                var wm = PdfWatermark.Apply(bytes,
                    $"Licensed to {pname}",
                    $"Issued to {pname} (institution #{p.PartnerId}) via the PCI partner portal - {DateTime.UtcNow:yyyy-MM-dd} - not for redistribution");
                if (wm is not null) bytes = wm; else wmResult = "ok_unwatermarked";
            }
            var viewOnly = H.L(d["view_only"]) == 1;
            DlAudit(id, null, p.Email, "partner", Ip(ctx), viewOnly ? "view" : "download", wmResult, H.Ln(d["version"]));
            var name = H.Str(d["filename"]) ?? ("document-" + id);
            if (viewOnly)
            {
                ctx.Response.Headers["Content-Disposition"] = "inline; filename=\"" + name.Replace("\"", "") + "\"";
                return Results.Bytes(bytes, mime);
            }
            return Results.Bytes(bytes, mime, name);
        });

        IResult ServeToStudent(HttpContext ctx, long id, long uid, string actorEmail, string role)
        {
            var d = db.QueryOne("SELECT * FROM documents WHERE id=?", id);
            if (d is null) { DlAudit(id, uid, actorEmail, role, Ip(ctx), "download", "not_found"); return Results.Json(new { error = "not_found" }, statusCode: 404); }
            if (DocAccess.ActiveAssignment(db, id, uid) is null)
            { DlAudit(id, uid, actorEmail, role, Ip(ctx), "download", "not_assigned"); return Results.Json(new { error = "not_assigned", message = "You do not have access to this document." }, statusCode: 403); }
            var (ok, reason) = Available(d);
            if (!ok) { DlAudit(id, uid, actorEmail, role, Ip(ctx), "download", "blocked_" + reason); return Results.Json(new { error = reason, message = BlockMessage(reason) }, statusCode: 403); }
            if (H.Str(d["restricted_until"]) is { Length: > 0 } ru && !H.IsPast(ru))
            { DlAudit(id, uid, actorEmail, role, Ip(ctx), "download", "restricted"); return Results.Json(new { error = "restricted", message = "This document is not available yet.", available_from = ru }, statusCode: 403); }
            if (H.L(d["ack_required"]) == 1 && db.QueryOne("SELECT id FROM document_acknowledgements WHERE document_id=? AND user_id=?", id, uid) is null)
            { DlAudit(id, uid, actorEmail, role, Ip(ctx), "download", "ack_required"); return Results.Json(new { error = "acknowledgement_required", message = "Please acknowledge this document before downloading." }, statusCode: 403); }
            var bytes = DocStore.Get(H.Str(d["storage_ref"]));
            if (bytes is null) { DlAudit(id, uid, actorEmail, role, Ip(ctx), "download", "file_missing"); return Results.Json(new { error = "file_missing" }, statusCode: 500); }
            var mime = H.Str(d["mime"]) ?? "application/octet-stream";
            // Per-recipient watermark: flagged PDFs are stamped with the recipient's identity on every
            // page at download time — the stored master is never modified and never exposed. Best-effort:
            // an unparseable/encrypted PDF falls back to the original, and the audit records that.
            var wmResult = "ok";
            if (H.L(d["watermark"]) == 1 && mime == "application/pdf")
            {
                var u2 = db.QueryOne("SELECT first_name,last_name,email FROM users WHERE id=?", uid);
                var fullName = ((H.Str(u2?["first_name"]) ?? "") + " " + (H.Str(u2?["last_name"]) ?? "")).Trim();
                if (fullName.Length == 0) fullName = actorEmail;
                var wm = PdfWatermark.Apply(bytes,
                    $"{fullName} - {H.Str(u2?["email"]) ?? actorEmail}",
                    $"Issued to {fullName} (member #{uid}) via the PCI student portal - {DateTime.UtcNow:yyyy-MM-dd} - not for redistribution");
                if (wm is not null) bytes = wm; else wmResult = "ok_unwatermarked";
            }
            var viewOnly = H.L(d["view_only"]) == 1;
            DlAudit(id, uid, actorEmail, role, Ip(ctx), viewOnly ? "view" : "download", wmResult, H.Ln(d["version"]));
            var name = H.Str(d["filename"]) ?? ("document-" + id);
            // View-only documents are served for INLINE display (no attachment prompt). True copy-prevention
            // is not achievable for a downloaded file; the master is never exposed and access stays audited.
            if (viewOnly)
            {
                ctx.Response.Headers["Content-Disposition"] = "inline; filename=\"" + name.Replace("\"", "") + "\"";
                return Results.Bytes(bytes, mime);
            }
            return Results.Bytes(bytes, mime, name);
        }
    }

    // Build the assignment_config JSON from whatever targeting fields the admin supplied.
    static string ConfigJson(Dictionary<string, JsonElement> b)
    {
        var cfg = new Dictionary<string, object?>();
        void N(string k) { var v = H.GetNum(b, k); if (v is not null) cfg[k] = (long)v.Value; }
        void S(string k) { var v = H.GetS(b, k); if (!string.IsNullOrWhiteSpace(v)) cfg[k] = v; }
        N("user_id"); N("certification_id"); N("partner_id");
        S("membership_type"); S("code"); S("country");
        // user_ids may arrive as an array or a CSV string; normalise to a long[].
        if (b.TryGetValue("user_ids", out var el))
        {
            var ids = new List<long>();
            if (el.ValueKind == JsonValueKind.Array)
                foreach (var e in el.EnumerateArray())
                {
                    if (e.ValueKind == JsonValueKind.Number && e.TryGetInt64(out var n)) ids.Add(n);
                    else if (e.ValueKind == JsonValueKind.String && long.TryParse(e.GetString(), out var n2)) ids.Add(n2);
                }
            else if (el.ValueKind == JsonValueKind.String)
                foreach (var part in (el.GetString() ?? "").Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    if (long.TryParse(part.Trim(), out var n)) ids.Add(n);
            if (ids.Count > 0) cfg["user_ids"] = ids;
        }
        return JsonSerializer.Serialize(cfg);
    }

    /// <summary>The partner_id an `institution` assignment targets, from its config JSON. Exact-parse
    /// (never a LIKE match) so partner 1 can never see partner 12's documents.</summary>
    static long? ConfigPartnerId(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("partner_id", out var v))
            {
                if (v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out var n)) return n;
                if (v.ValueKind == JsonValueKind.String && long.TryParse(v.GetString(), out var n2)) return n2;
            }
        }
        catch { }
        return null;
    }

    static string DocErrorMessage(string? error) => error switch
    {
        "no_file" => "Please choose a file to upload.",
        "file_type_not_allowed" => "That file type is not allowed. Accepted: PDF, Word, Excel, PowerPoint, CSV, text, images and ZIP.",
        "file_too_large" => "That file is too large. The maximum size is 25 MB.",
        "content_mime_mismatch" => "The file contents do not match its type — it may be corrupted or renamed.",
        "malware_suspected" => "That file was rejected by the security scan.",
        "empty_file" => "The file appears to be empty.",
        _ => "The file could not be accepted. Please try a different file.",
    };

    static string BlockMessage(string? reason) => reason switch
    {
        "suspended" => "This document is temporarily suspended. Please contact support.",
        "archived" => "This document has been archived and is no longer available.",
        "replaced" => "A newer version of this document is available.",
        "expired" => "This document has expired and is no longer available.",
        "scheduled" => "This document is not available yet.",
        "rejected" => "This document is not available.",
        _ => "This document is not currently available.",
    };
}
