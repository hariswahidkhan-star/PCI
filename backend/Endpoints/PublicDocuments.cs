using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Public Downloads Centre. Governance, policy and legal documents that PCI distributes to ANYONE without
/// a login, plus the admin module that manages them dynamically (create → legal review → publish → replace →
/// supersede → withdraw/archive) with full version history, download analytics and an audit trail.
///
/// Security model: a document is only ever served publicly when it is BOTH status='published' AND
/// visibility='public'. Drafts, internal-only, withdrawn and archived documents are never distributed, and
/// the master storage reference is never exposed to clients. Versions are chained by a stable
/// <c>doc_group</c> id; <c>is_current</c> marks the live version, older published versions become
/// 'superseded' and remain retrievable by their explicit version id (behind a superseded notice) so old
/// links keep resolving, but they never appear in the current list.
/// </summary>
public static class PublicDocuments
{
    // Public download categories (also the /downloads/{category} sub-routes). Anything else is coerced to 'general'.
    static readonly HashSet<string> Categories = new(StringComparer.OrdinalIgnoreCase)
    {
        "certification-governance", "candidate-handbooks", "application-routes", "exams",
        "privacy-and-legal", "fees-and-refunds", "marketing-partners", "institutions",
        "accessibility", "policies", "templates", "general",
    };
    static readonly HashSet<string> Statuses = new(StringComparer.OrdinalIgnoreCase)
    { "draft", "under_review", "legal_review_required", "approved", "scheduled", "published", "superseded", "archived", "withdrawn" };
    static readonly HashSet<string> LegalStatuses = new(StringComparer.OrdinalIgnoreCase)
    { "draft", "internal_review_completed", "external_legal_review_pending", "external_legal_review_completed", "approved_for_publication" };
    static readonly Dictionary<string, string> RouteLabels = new(StringComparer.OrdinalIgnoreCase)
    {
        ["standard"] = "Standard Route", ["founding"] = "Founding Route", ["honorary"] = "Honorary Route",
        ["sponsored"] = "Sponsored Route", ["complimentary"] = "Complimentary & Waived Routes",
    };

    static string Clip(string? s, int max) => (s ?? "").Trim() is { } t && t.Length > max ? t[..max] : (s ?? "").Trim();
    static string Cat(string? c) => c is { } v && Categories.Contains(v.Trim()) ? v.Trim().ToLowerInvariant() : "general";

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);

        // The admin permission that guards this module. Reuses the existing "documents" section so document
        // managers (and owners) can run the Downloads Centre without a new permission having to be granted.
        const string SECTION = "documents";

        static void AllowUpload(HttpContext ctx)
        {
            var f = ctx.Features.Get<IHttpMaxRequestBodySizeFeature>();
            if (f is not null && !f.IsReadOnly) f.MaxRequestBodySize = 40_000_000;
        }

        // Project a row for PUBLIC consumption — safe fields only, with resolved cert/route names. Never
        // includes storage_ref/sha or any internal review field.
        object PubProj(Dictionary<string, object?> r)
        {
            var certId = r["certification_id"];
            var certName = certId is null ? null : H.Str(db.QueryOne("SELECT name FROM certifications WHERE id=?", certId)?["name"]);
            var routeKey = H.Str(r["route_key"]);
            return new
            {
                doc_group = r["doc_group"], id = r["id"], title = r["title"], description = r["description"],
                category = r["category"], certification_id = certId, certification_name = certName,
                route_key = routeKey, route_label = routeKey is { Length: > 0 } && RouteLabels.TryGetValue(routeKey, out var rl) ? rl : null,
                language = r["language"], version = r["version"], status = r["status"],
                effective_date = r["effective_date"], published_at = r["published_at"], updated_at = r["updated_at"],
                next_review_date = r["next_review_date"], filename = r["filename"], mime = r["mime"],
                size_bytes = r["size_bytes"], download_count = r["download_count"], is_current = H.B(r["is_current"]),
                has_file = H.Str(r["storage_ref"]) is { Length: > 0 },
            };
        }

        // ─────────────────────────── PUBLIC (no login) ───────────────────────────

        // Catalogue of currently-published, public documents with filters + sort. This is the /downloads feed.
        app.MapGet("/api/public/documents", (HttpRequest req) =>
        {
            var category = req.Query["category"].ToString();
            var certSel = req.Query["cert"].ToString();
            var route = req.Query["route"].ToString();
            var lang = req.Query["lang"].ToString();
            var q = req.Query["q"].ToString().Trim();
            var sort = req.Query["sort"].ToString();

            var where = new List<string> { "status='published'", "visibility='public'", "is_current=1" };
            var args = new List<object?>();
            if (!string.IsNullOrEmpty(category) && Categories.Contains(category)) { where.Add("category=?"); args.Add(category.ToLowerInvariant()); }
            if (!string.IsNullOrEmpty(route)) { where.Add("route_key=?"); args.Add(route.ToLowerInvariant()); }
            if (!string.IsNullOrEmpty(lang)) { where.Add("language=?"); args.Add(lang.ToLowerInvariant()); }
            if (!string.IsNullOrEmpty(certSel))
            {
                if (certSel.Equals("general", StringComparison.OrdinalIgnoreCase)) where.Add("certification_id IS NULL");
                else { var cid = Certs.TryResolve(db, certSel); if (cid is null) return J(new { rows = Array.Empty<object>() }); where.Add("certification_id=?"); args.Add(cid.Value); }
            }
            if (!string.IsNullOrEmpty(q)) { where.Add("(title LIKE ? OR description LIKE ?)"); args.Add("%" + q + "%"); args.Add("%" + q + "%"); }
            var order = sort switch { "title" => "title ASC", "category" => "category ASC, title ASC", _ => "COALESCE(published_at, updated_at) DESC" };
            var rows = db.Query($"SELECT * FROM public_documents WHERE {string.Join(" AND ", where)} ORDER BY {order} LIMIT 1000", args.ToArray());
            return J(new { rows = rows.Select(PubProj).ToList() });
        });

        // Public metadata + published version history for one document group.
        app.MapGet("/api/public/documents/{group}", (string group) =>
        {
            var cur = db.QueryOne("SELECT * FROM public_documents WHERE doc_group=? AND status='published' AND visibility='public' AND is_current=1", group);
            if (cur is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var history = db.Query("SELECT id,version,status,published_at,effective_date FROM public_documents WHERE doc_group=? AND visibility='public' AND status IN ('published','superseded') ORDER BY id DESC", group);
            return J(new { document = PubProj(cur), versions = history });
        });

        // Stream the current published public file (or a specific superseded version via ?v=id). Increments
        // the download counter and writes an audit row. Never serves a non-public / non-published document.
        app.MapGet("/api/public/documents/{group}/file", (HttpContext ctx, string group) =>
        {
            Dictionary<string, object?>? row;
            var v = ctx.Request.Query["v"].ToString();
            if (long.TryParse(v, out var vid))
                row = db.QueryOne("SELECT * FROM public_documents WHERE id=? AND doc_group=? AND visibility='public' AND status IN ('published','superseded')", vid, group);
            else
                row = db.QueryOne("SELECT * FROM public_documents WHERE doc_group=? AND status='published' AND visibility='public' AND is_current=1", group);
            if (row is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var sref = H.Str(row["storage_ref"]);
            if (string.IsNullOrEmpty(sref)) return Results.Json(new { error = "file_unavailable" }, statusCode: 404);
            var got = Storage.Get(sref);
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "file_unavailable" }, statusCode: 404);
            db.Execute("UPDATE public_documents SET download_count=download_count+1 WHERE id=?", row["id"]);
            db.Execute("INSERT INTO public_document_downloads(document_id,doc_group,ip,ua) VALUES(?,?,?,?)",
                row["id"], group, H.LastHopIp(ctx.Request.Headers["X-Forwarded-For"].ToString(), ctx.Connection.RemoteIpAddress?.ToString()),
                Clip(ctx.Request.Headers.UserAgent.ToString(), 255));
            var fn = H.Str(row["filename"]); if (string.IsNullOrEmpty(fn)) fn = group + ".pdf";
            var disposition = ctx.Request.Query["dl"] == "1" ? "attachment" : "inline";
            ctx.Response.Headers.ContentDisposition = $"{disposition}; filename=\"{fn.Replace('"', '_')}\"";
            return Results.File(got.Value.bytes!, got.Value.mime ?? "application/pdf");
        });

        // ─────────────────────────── ADMIN (owner or "documents") ───────────────────────────

        object AdmProj(Dictionary<string, object?> r)
        {
            var certId = r["certification_id"];
            return new
            {
                id = r["id"], doc_group = r["doc_group"], title = r["title"], description = r["description"],
                category = r["category"], certification_id = certId,
                certification_name = certId is null ? null : H.Str(db.QueryOne("SELECT name FROM certifications WHERE id=?", certId)?["name"]),
                route_key = r["route_key"], language = r["language"], version = r["version"],
                status = r["status"], legal_review_status = r["legal_review_status"], visibility = r["visibility"],
                owner = r["owner"], approver = r["approver"], effective_date = r["effective_date"],
                published_at = r["published_at"], scheduled_at = r["scheduled_at"], next_review_date = r["next_review_date"],
                filename = r["filename"], mime = r["mime"], size_bytes = r["size_bytes"], sha256 = r["sha256"],
                is_current = H.B(r["is_current"]), supersedes_id = r["supersedes_id"], related_groups = r["related_groups"],
                download_count = r["download_count"], has_file = H.Str(r["storage_ref"]) is { Length: > 0 },
                created_at = r["created_at"], updated_at = r["updated_at"],
            };
        }

        app.MapGet("/api/admin/public-documents", (HttpRequest req) => gate(req, SECTION, _ =>
        {
            var status = req.Query["status"].ToString();
            var category = req.Query["category"].ToString();
            var group = req.Query["group"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (!string.IsNullOrEmpty(status)) { where.Add("status=?"); args.Add(status); }
            if (!string.IsNullOrEmpty(category)) { where.Add("category=?"); args.Add(category); }
            if (!string.IsNullOrEmpty(group)) { where.Add("doc_group=?"); args.Add(group); }
            var w = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "";
            var rows = db.Query($"SELECT * FROM public_documents {w} ORDER BY doc_group, id DESC LIMIT 2000", args.ToArray());
            return J(new { rows = rows.Select(AdmProj).ToList() });
        }));

        app.MapGet("/api/admin/public-documents/{id:long}", (HttpRequest req, long id) => gate(req, SECTION, _ =>
        {
            var r = db.QueryOne("SELECT * FROM public_documents WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var versions = db.Query("SELECT id,version,status,is_current,published_at,effective_date,download_count FROM public_documents WHERE doc_group=? ORDER BY id DESC", r["doc_group"]);
            var recent = db.Query("SELECT ip,ua,created_at FROM public_document_downloads WHERE doc_group=? ORDER BY id DESC LIMIT 25", r["doc_group"]);
            return J(new { document = AdmProj(r), versions, recent_downloads = recent });
        }));

        // Create a new document (metadata; file optional and can be added/replaced later).
        app.MapPost("/api/admin/public-documents", (HttpContext ctx) =>
        {
            AllowUpload(ctx);
            return gate(ctx.Request, SECTION, adm => CreateOrFile(ctx, adm, db, log, null).GetAwaiter().GetResult());
        });

        // Update metadata / lifecycle fields on a specific version row.
        app.MapMethods("/api/admin/public-documents/{id:long}", new[] { "PATCH", "PUT" }, async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var r = db.QueryOne("SELECT * FROM public_documents WHERE id=?", id);
                if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var sets = new List<string>(); var args = new List<object?>();
                void Set(string col, object? val) { sets.Add($"{col}=?"); args.Add(val); }
                if (H.GetEl(b, "title") is not null) Set("title", Clip(H.GetS(b, "title"), 240));
                if (H.GetEl(b, "description") is not null) Set("description", Clip(H.GetS(b, "description"), 2000));
                if (H.GetEl(b, "category") is not null) Set("category", Cat(H.GetS(b, "category")));
                if (H.GetEl(b, "route_key") is not null) { var rk = Clip(H.GetS(b, "route_key"), 40).ToLowerInvariant(); Set("route_key", string.IsNullOrEmpty(rk) ? null : rk); }
                if (H.GetEl(b, "language") is not null) Set("language", Clip(H.GetS(b, "language"), 10).ToLowerInvariant());
                if (H.GetEl(b, "version") is not null) Set("version", Clip(H.GetS(b, "version"), 20));
                if (H.GetEl(b, "owner") is not null) Set("owner", Clip(H.GetS(b, "owner"), 160));
                if (H.GetEl(b, "approver") is not null) Set("approver", Clip(H.GetS(b, "approver"), 160));
                if (H.GetEl(b, "effective_date") is not null) Set("effective_date", Clip(H.GetS(b, "effective_date"), 40));
                if (H.GetEl(b, "next_review_date") is not null) Set("next_review_date", Clip(H.GetS(b, "next_review_date"), 40));
                if (H.GetEl(b, "scheduled_at") is not null) Set("scheduled_at", Clip(H.GetS(b, "scheduled_at"), 40));
                if (H.GetEl(b, "related_groups") is not null) Set("related_groups", Clip(H.GetS(b, "related_groups"), 500));
                if (H.GetEl(b, "visibility") is not null) { var vis = H.GetS(b, "visibility") == "internal" ? "internal" : "public"; Set("visibility", vis); }
                if (H.GetEl(b, "legal_review_status") is { } && H.GetS(b, "legal_review_status") is { } lrs && LegalStatuses.Contains(lrs)) Set("legal_review_status", lrs.ToLowerInvariant());
                if (H.GetEl(b, "certification") is not null || H.GetEl(b, "certification_id") is not null)
                {
                    var cs = H.GetS(b, "certification", "certification_id") ?? "";
                    if (string.IsNullOrWhiteSpace(cs) || cs.Equals("general", StringComparison.OrdinalIgnoreCase)) Set("certification_id", null);
                    else { var cid = Certs.TryResolve(db, cs); if (cid is null) return Results.Json(new { error = "invalid_certification" }, statusCode: 400); Set("certification_id", cid.Value); }
                }
                if (sets.Count == 0) return J(new { ok = true });
                sets.Add("updated_at=datetime('now')"); args.Add(id);
                db.Execute($"UPDATE public_documents SET {string.Join(",", sets)} WHERE id=?", args.ToArray());
                log(adm.Id, "public_document_updated", $"#{id}");
                return J(new { ok = true });
            });
        });

        // Upload / attach the file bytes on a specific row (does not create a new version). Allowed
        // ONLY while the row is still pre-publication: once a version has been published (or has left
        // the pre-publication pipeline), its bytes are immutable history — replacing the file requires
        // the /replace route, which creates a NEW version and preserves this one. This closes the gap
        // where a published, legally-reviewed public PDF could be silently swapped in place.
        app.MapPost("/api/admin/public-documents/{id:long}/file", async (HttpContext ctx, long id) =>
        {
            AllowUpload(ctx);
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var r = db.QueryOne("SELECT id,status FROM public_documents WHERE id=?", id);
                if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var status = (H.Str(r["status"]) ?? "draft").ToLowerInvariant();
                if (status is not ("draft" or "under_review" or "legal_review_required" or "approved"))
                    return Results.Json(new
                    {
                        error = "published_file_immutable",
                        message = "This version has been published — its file is immutable history. Use “Replace” to create a new version instead.",
                    }, statusCode: 409);
                var (bytes, mime, err) = Storage.DecodeDataUri(H.GetS(b, "data_uri", "dataUri", "data"));
                if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
                var obj = Storage.Put(bytes!, mime, "public-docs");
                db.Execute("UPDATE public_documents SET storage_ref=?,filename=?,mime=?,size_bytes=?,sha256=?,updated_at=datetime('now') WHERE id=?",
                    obj.Reference, Clip(H.GetS(b, "filename", "name"), 200), obj.Mime, obj.SizeBytes, obj.Sha256, id);
                log(adm.Id, "public_document_file_set", $"#{id} {obj.SizeBytes}b");
                return J(new { ok = true, filename = H.GetS(b, "filename", "name"), size_bytes = obj.SizeBytes });
            });
        });

        // Create a NEW version in the same doc_group from an existing row (optionally with a new file).
        // The old version is marked superseded; the new one is a draft until explicitly published.
        app.MapPost("/api/admin/public-documents/{id:long}/replace", async (HttpContext ctx, long id) =>
        {
            AllowUpload(ctx);
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var old = db.QueryOne("SELECT * FROM public_documents WHERE id=?", id);
                if (old is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var newVersion = Clip(H.GetS(b, "version"), 20);
                if (newVersion.Length == 0) newVersion = BumpVersion(H.Str(old["version"]));
                // Copy file forward unless a new one is supplied.
                string? sref = H.Str(old["storage_ref"]), fname = H.Str(old["filename"]), mime = H.Str(old["mime"]), sha = H.Str(old["sha256"]);
                long? size = old["size_bytes"] is null ? null : H.L(old["size_bytes"]);
                if (H.GetS(b, "data_uri", "dataUri", "data") is { Length: > 0 } du)
                {
                    var (bytes, m, err) = Storage.DecodeDataUri(du);
                    if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
                    var obj = Storage.Put(bytes!, m, "public-docs");
                    sref = obj.Reference; mime = obj.Mime; size = obj.SizeBytes; sha = obj.Sha256;
                    fname = Clip(H.GetS(b, "filename", "name"), 200); if (fname.Length == 0) fname = H.Str(old["filename"]);
                }
                var newId = db.ExecuteReturningId(@"INSERT INTO public_documents
                    (doc_group,title,description,category,certification_id,route_key,language,version,status,legal_review_status,visibility,owner,approver,effective_date,next_review_date,storage_ref,filename,mime,size_bytes,sha256,is_current,supersedes_id,related_groups,sort_order,created_by)
                    VALUES(?,?,?,?,?,?,?,?, 'draft','draft',?,?,?,?,?,?,?,?,?,?, 0,?,?,?,?)",
                    old["doc_group"], old["title"], old["description"], old["category"], old["certification_id"], old["route_key"], old["language"], newVersion,
                    old["visibility"], old["owner"], old["approver"], old["effective_date"], old["next_review_date"],
                    sref, fname, mime, size, sha, id, old["related_groups"], old["sort_order"], adm.Id);
                log(adm.Id, "public_document_new_version", $"group={old["doc_group"]} #{id}→#{newId} v{newVersion}");
                return J(new { ok = true, id = newId, version = newVersion });
            });
        });

        // Lifecycle transition. Publishing enforces a single current version per group and demotes the prior
        // published version to 'superseded'. Withdraw/archive clear is_current so the document leaves the
        // public list immediately.
        app.MapPost("/api/admin/public-documents/{id:long}/status", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var r = db.QueryOne("SELECT * FROM public_documents WHERE id=?", id);
                if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var status = (H.GetS(b, "status") ?? "").Trim().ToLowerInvariant();
                if (!Statuses.Contains(status)) return Results.Json(new { error = "bad_status" }, statusCode: 400);
                var group = H.Str(r["doc_group"]);
                if (status == "published")
                {
                    if (string.IsNullOrEmpty(H.Str(r["storage_ref"])))
                        return Results.Json(new { error = "no_file", message = "Attach a file before publishing." }, statusCode: 400);
                    // Demote any currently-published version in this group.
                    db.Execute("UPDATE public_documents SET is_current=0, status='superseded', updated_at=datetime('now') WHERE doc_group=? AND id<>? AND status='published'", group, id);
                    db.Execute("UPDATE public_documents SET is_current=0 WHERE doc_group=? AND id<>?", group, id);
                    db.Execute("UPDATE public_documents SET status='published', is_current=1, visibility=CASE WHEN visibility='internal' THEN 'public' ELSE visibility END, published_at=COALESCE(published_at, datetime('now')), updated_at=datetime('now') WHERE id=?", id);
                }
                else if (status is "withdrawn" or "archived" or "superseded")
                {
                    db.Execute("UPDATE public_documents SET status=?, is_current=0, updated_at=datetime('now') WHERE id=?", status, id);
                }
                else
                {
                    db.Execute("UPDATE public_documents SET status=?, updated_at=datetime('now') WHERE id=?", status, id);
                }
                log(adm.Id, "public_document_status", $"#{id} → {status}");
                return J(new { ok = true, status });
            });
        });

        // Admin file fetch — any status (for preview/QA before publishing). Logged: this is the one
        // route that can read internal/withdrawn versions, so every access leaves an audit trail.
        app.MapGet("/api/admin/public-documents/{id:long}/file", (HttpContext ctx, long id) => gate(ctx.Request, SECTION, adm =>
        {
            var r = db.QueryOne("SELECT storage_ref,mime,filename,doc_group,version,status FROM public_documents WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var got = Storage.Get(H.Str(r["storage_ref"]));
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "file_unavailable" }, statusCode: 404);
            log(adm.Id, "public_document_file_view", $"#{id} {H.Str(r["doc_group"])} v{H.Str(r["version"])} ({H.Str(r["status"])})");
            if (ctx.Request.Query["dl"].ToString() == "1")
                return Results.Bytes(got.Value.bytes!, got.Value.mime ?? "application/pdf", H.Str(r["filename"]) ?? ("document-" + id + ".pdf"));
            return Results.File(got.Value.bytes!, got.Value.mime ?? "application/pdf");
        }));

        // Delete — only a draft with no published history may be hard-deleted; otherwise archive.
        app.MapDelete("/api/admin/public-documents/{id:long}", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            var r = db.QueryOne("SELECT status FROM public_documents WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (!string.Equals(H.Str(r["status"]), "draft", StringComparison.OrdinalIgnoreCase))
                return Results.Json(new { error = "not_deletable", message = "Only a draft can be deleted; archive or withdraw instead." }, statusCode: 400);
            db.Execute("DELETE FROM public_documents WHERE id=?", id);
            log(adm.Id, "public_document_deleted", $"#{id}");
            return J(new { ok = true });
        }));
    }

    // Shared create handler (used by POST create). Optionally accepts an inline file.
    static async Task<IResult> CreateOrFile(HttpContext ctx, AdminCtx adm, Db db, Action<long?, string, string?> log, long? _)
    {
        var b = await H.Body(ctx.Request);
        var title = Clip(H.GetS(b, "title"), 240);
        if (title.Length == 0) return Results.Json(new { error = "title_required" }, statusCode: 400);
        var group = Clip(H.GetS(b, "doc_group", "group"), 64);
        if (group.Length == 0) group = Slug(title);
        // Ensure the group is unique unless the caller is explicitly adding a version (handled by /replace).
        if (db.QueryOne("SELECT id FROM public_documents WHERE doc_group=?", group) is not null)
            group = group + "-" + Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(title + DateTime.UtcNow.Ticks)))[..6].ToLowerInvariant();

        long? certId = null;
        var cs = H.GetS(b, "certification", "certification_id") ?? "";
        if (!string.IsNullOrWhiteSpace(cs) && !cs.Equals("general", StringComparison.OrdinalIgnoreCase))
        {
            certId = Certs.TryResolve(db, cs);
            if (certId is null) return Results.Json(new { error = "invalid_certification" }, statusCode: 400);
        }
        var routeKey = Clip(H.GetS(b, "route_key"), 40).ToLowerInvariant();
        var lang = Clip(H.GetS(b, "language"), 10).ToLowerInvariant(); if (lang.Length == 0) lang = "en";
        var version = Clip(H.GetS(b, "version"), 20); if (version.Length == 0) version = "1.0";
        var legal = H.GetS(b, "legal_review_status") is { } l && LegalStatuses.Contains(l) ? l.ToLowerInvariant() : "draft";
        var visibility = H.GetS(b, "visibility") == "internal" ? "internal" : "public";

        string? sref = null, fname = null, mime = null, sha = null; long? size = null;
        if (H.GetS(b, "data_uri", "dataUri", "data") is { Length: > 0 } du)
        {
            var (bytes, m, err) = Storage.DecodeDataUri(du);
            if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
            var obj = Storage.Put(bytes!, m, "public-docs");
            sref = obj.Reference; mime = obj.Mime; size = obj.SizeBytes; sha = obj.Sha256;
            fname = Clip(H.GetS(b, "filename", "name"), 200);
        }
        var id = db.ExecuteReturningId(@"INSERT INTO public_documents
            (doc_group,title,description,category,certification_id,route_key,language,version,status,legal_review_status,visibility,owner,approver,effective_date,next_review_date,storage_ref,filename,mime,size_bytes,sha256,related_groups,created_by)
            VALUES(?,?,?,?,?,?,?,?, 'draft',?,?,?,?,?,?,?,?,?,?,?,?,?)",
            group, title, Clip(H.GetS(b, "description"), 2000), Cat(H.GetS(b, "category")), certId,
            string.IsNullOrEmpty(routeKey) ? null : routeKey, lang, version, legal, visibility,
            Clip(H.GetS(b, "owner"), 160), Clip(H.GetS(b, "approver"), 160),
            Clip(H.GetS(b, "effective_date"), 40), Clip(H.GetS(b, "next_review_date"), 40),
            sref, fname, mime, size, sha, Clip(H.GetS(b, "related_groups"), 500), adm.Id);
        log(adm.Id, "public_document_created", $"#{id} {group}");
        return Results.Json(new { ok = true, id, doc_group = group });
    }

    static string Slug(string s)
    {
        var chars = s.Trim().ToLowerInvariant().Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray();
        var slug = new string(chars);
        while (slug.Contains("--")) slug = slug.Replace("--", "-");
        slug = slug.Trim('-');
        return slug.Length > 60 ? slug[..60].Trim('-') : (slug.Length == 0 ? "document" : slug);
    }

    static string BumpVersion(string? v)
    {
        if (string.IsNullOrEmpty(v)) return "1.1";
        var parts = v.Split('.');
        if (parts.Length >= 2 && int.TryParse(parts[0], out var major) && int.TryParse(parts[1], out var minor))
            return $"{major}.{minor + 1}";
        return v + ".1";
    }
}
