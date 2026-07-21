using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Content, SEO &amp; Distribution Centre — admin CMS + public blog API + AI Content Studio + the Integration
/// Capability Registry. Posts move through an editorial workflow (draft → review → approved → scheduled →
/// published) with a full version snapshot on every save (the previous version is never overwritten). SEO
/// fields are per-post; publishing bumps the sitemap cache and (optionally) queues an IndexNow ping. The AI
/// Studio assists only — it returns text for a human to review and records an audit row; it never publishes.
/// Every privileged action is gated by a granular cc_* permission and audit-logged.
/// </summary>
public static class ContentCentre
{
    // Post columns persisted directly (the rest of the spec's fields ride in extra_json).
    static readonly string[] TextCols =
    {
        "title","seo_title","subtitle","summary","body","body_format","blocks_json","featured_image",
        "featured_image_alt","social_image","topic_cluster","primary_keyword","secondary_keywords","search_intent",
        "target_audience","language","route_key","industry","meta_description","canonical_url","original_source_url",
        "og_title","og_description","og_image","structured_type","content_ownership","copyright_status","license",
        "attribution","syndication_status","ai_disclosure","internal_notes"
    };
    static readonly string[] IntCols = { "author_id","reviewer_id","editor_id","category_id","certification_id","robots_noindex","ai_assisted","social_distribution","newsletter" };

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate, Func<HttpRequest, AdminCtx?> adminFromReq)
    {
        IResult J(object o) => Results.Json(o);
        bool IsOk(IResult r) => r is Microsoft.AspNetCore.Http.HttpResults.Ok;

        // ══════════════════════════ PUBLIC BLOG API ══════════════════════════
        app.MapGet("/api/blog/posts", (HttpRequest req) =>
        {
            int.TryParse(req.Query["page"], out var page); if (page < 1) page = 1;
            var per = Blog.PerPage(db);
            long? catId = null, authId = null;
            if (req.Query["category"].ToString() is { Length: > 0 } cs && db.QueryOne("SELECT id FROM blog_categories WHERE slug=? AND active=1", cs.ToLowerInvariant()) is { } c) catId = H.L(c["id"]);
            if (req.Query["author"].ToString() is { Length: > 0 } au && db.QueryOne("SELECT id FROM blog_authors WHERE slug=? AND active=1", au.ToLowerInvariant()) is { } a) authId = H.L(a["id"]);
            var rows = Blog.ListPublished(db, per, (page - 1) * per, catId, authId, null, null);
            return J(new
            {
                page,
                total = Blog.CountPublished(db, catId, authId, null),
                base_path = Blog.BasePath(db),
                posts = rows.Select(p => PublicCard(db, p)),
            });
        });

        app.MapGet("/api/blog/posts/{slug}", (string slug) =>
        {
            var p = Blog.PublishedBySlug(db, slug);
            if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(PublicFull(db, p));
        });

        app.MapGet("/api/blog/meta", () => J(new
        {
            base_path = Blog.BasePath(db),
            categories = db.Query("SELECT slug,name FROM blog_categories WHERE active=1 ORDER BY sort_order,name"),
            authors = db.Query("SELECT slug,name,title FROM blog_authors WHERE active=1 ORDER BY sort_order,name"),
            feeds = new { rss = Blog.BasePath(db) + "/feed.xml", atom = Blog.BasePath(db) + "/atom.xml", json = Blog.BasePath(db) + "/feed.json" },
        }));

        // ══════════════════════════ ADMIN: overview ══════════════════════════
        app.MapGet("/api/admin/content/overview", (HttpRequest req) => gate(req, "cc_view", adm =>
        {
            var byStatus = db.Query("SELECT status, COUNT(*) n FROM blog_posts GROUP BY status");
            long C(string sql) => db.Scalar<long>(sql);
            return J(new
            {
                posts_total = C("SELECT COUNT(*) FROM blog_posts"),
                published = C("SELECT COUNT(*) FROM blog_posts WHERE status='published' AND published=1"),
                drafts = C("SELECT COUNT(*) FROM blog_posts WHERE status IN ('draft','author_review')"),
                in_review = C("SELECT COUNT(*) FROM blog_posts WHERE status IN ('editorial_review','technical_review','seo_review','legal_review')"),
                scheduled = C("SELECT COUNT(*) FROM blog_posts WHERE status='scheduled'"),
                authors = C("SELECT COUNT(*) FROM blog_authors WHERE active=1"),
                categories = C("SELECT COUNT(*) FROM blog_categories WHERE active=1"),
                by_status = byStatus,
                ai = new { openai = AiContent.OpenAiReady, anthropic = AiContent.AnthropicReady },
                recent = db.Query("SELECT id,slug,title,status,published,updated_at FROM blog_posts ORDER BY updated_at DESC, id DESC LIMIT 10"),
            });
        }));

        // ══════════════════════════ ADMIN: posts ══════════════════════════
        app.MapGet("/api/admin/content/posts", (HttpRequest req) => gate(req, "cc_view", adm =>
        {
            var status = req.Query["status"].ToString();
            var q = req.Query["q"].ToString();
            var sql = "SELECT id,slug,title,status,published,author_id,category_id,language,published_at,updated_at,version FROM blog_posts WHERE 1=1";
            var args = new List<object?>();
            if (status.Length > 0) { sql += " AND status=?"; args.Add(status); }
            else sql += " AND status<>'deleted'";     // soft-deleted posts are hidden unless explicitly filtered
            if (q.Length > 0) { sql += " AND (title LIKE ? OR slug LIKE ?)"; args.Add("%" + q + "%"); args.Add("%" + q + "%"); }
            sql += " ORDER BY updated_at DESC, id DESC LIMIT 200";
            var rows = db.Query(sql, args.ToArray());
            foreach (var r in rows)
            {
                r["author_name"] = Blog.Author(db, r["author_id"]) is { } a ? H.Str(a["name"]) : null;
                r["category_name"] = Blog.Category(db, r["category_id"]) is { } c ? H.Str(c["name"]) : null;
            }
            return J(new { rows });
        }));

        app.MapGet("/api/admin/content/posts/{id:long}", (HttpRequest req, long id) => gate(req, "cc_view", adm =>
        {
            var p = db.QueryOne("SELECT * FROM blog_posts WHERE id=?", id);
            if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(new
            {
                post = p,
                tags = Blog.Tags(db, id).Select(t => H.Str(t["name"])),
                versions = db.Query("SELECT id,version,status_at,change_reason,editor_id,created_at FROM blog_post_versions WHERE post_id=? ORDER BY version DESC", id),
                reviews = db.Query("SELECT id,stage,decision,reviewer_id,note,created_at FROM blog_reviews WHERE post_id=? ORDER BY id DESC", id),
                public_url = Blog.PublicUrl(db, H.Str(p["slug"]) ?? ""),
                seo = SeoCheck(db, p),
            });
        }));

        app.MapPost("/api/admin/content/posts", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "cc_author", adm =>
            {
                var title = (H.GetS(b, "title") ?? "").Trim();
                if (title.Length == 0) return Results.Json(new { error = "title_required" }, statusCode: 400);
                var slug = Blog.UniqueSlug(db, H.GetS(b, "slug") is { Length: > 0 } s ? s : title);
                var id = db.ExecuteReturningId("INSERT INTO blog_posts(slug,title,status,created_by,created_at,updated_at) VALUES(?,?, 'draft', ?, datetime('now'), datetime('now'))", slug, title, adm.Id);
                ApplyFields(db, id, b);
                Snapshot(db, id, "created", adm.Id);
                log(adm.Id, "content_post_create", "post " + id + " " + slug);
                return J(new { ok = true, id, slug });
            });
        });

        app.MapPatch("/api/admin/content/posts/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_edit", adm =>
            {
                var p = db.QueryOne("SELECT id,version,status FROM blog_posts WHERE id=?", id);
                if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                Snapshot(db, id, H.GetS(b, "change_reason") ?? "edit", adm.Id);      // preserve prior version first
                if (H.GetS(b, "slug") is { Length: > 0 } newSlugRaw)
                {
                    var oldSlug = H.Str(db.QueryOne("SELECT slug FROM blog_posts WHERE id=?", id)?["slug"]) ?? "";
                    var newSlug = Blog.UniqueSlug(db, newSlugRaw, id);
                    if (!string.Equals(oldSlug, newSlug, StringComparison.OrdinalIgnoreCase))
                    {
                        db.Execute("UPDATE blog_posts SET slug=? WHERE id=?", newSlug, id);
                        // A live post keeps its old public URL working via a 301 to the new slug (links + SEO survive).
                        if (H.Str(p["status"]) == "published") AddSlugRedirect(db, id, oldSlug, newSlug);
                    }
                }
                ApplyFields(db, id, b);
                db.Execute("UPDATE blog_posts SET version=version+1, updated_at=datetime('now') WHERE id=?", id);
                log(adm.Id, "content_post_update", "post " + id);
                return J(new { ok = true, id });
            });
        });

        // Workflow transitions -------------------------------------------------
        app.MapPost("/api/admin/content/posts/{id:long}/submit", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_author", adm =>
            {
                var stage = H.GetS(b, "stage") ?? "editorial_review";
                db.Execute("UPDATE blog_posts SET status=?, updated_at=datetime('now') WHERE id=?", stage, id);
                db.Execute("INSERT INTO blog_reviews(post_id,stage,decision,reviewer_id,note,created_at) VALUES(?,?, 'submitted', ?, ?, datetime('now'))", id, stage, adm.Id, H.GetS(b, "note"));
                log(adm.Id, "content_post_submit", "post " + id + " → " + stage);
                return J(new { ok = true, status = stage });
            });
        });

        app.MapPost("/api/admin/content/posts/{id:long}/review", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_review", adm =>
            {
                var p = db.QueryOne("SELECT author_id FROM blog_posts WHERE id=?", id);
                if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (SelfAuthored(db, p, adm)) return Results.Json(new { error = "self_review_forbidden", message = "You cannot review your own article." }, statusCode: 403);
                var decision = H.GetS(b, "decision") ?? "approved";     // approved | changes_requested | rejected
                var stage = H.GetS(b, "stage") ?? "editorial_review";
                var nextStatus = decision == "approved" ? "approved" : decision == "rejected" ? "withdrawn" : "changes_requested";
                db.Execute("UPDATE blog_posts SET status=?, updated_at=datetime('now') WHERE id=?", nextStatus, id);
                db.Execute("INSERT INTO blog_reviews(post_id,stage,decision,reviewer_id,note,created_at) VALUES(?,?,?,?,?, datetime('now'))", id, stage, decision, adm.Id, H.GetS(b, "note"));
                log(adm.Id, "content_post_review", "post " + id + " " + stage + "=" + decision);
                return J(new { ok = true, status = nextStatus });
            });
        });

        app.MapPost("/api/admin/content/posts/{id:long}/publish", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_publish", adm =>
            {
                var p = db.QueryOne("SELECT author_id FROM blog_posts WHERE id=?", id);
                if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (SelfAuthored(db, p, adm)) return Results.Json(new { error = "self_publish_forbidden", message = "You cannot publish your own article." }, statusCode: 403);
                var (ok, error, url) = PublishPost(db, id, adm.Id, log);
                if (!ok) return error == "incomplete"
                    ? Results.Json(new { error = "incomplete", message = "A title and body are required before publishing." }, statusCode: 400)
                    : Results.Json(new { error = error ?? "error" }, statusCode: 404);
                return J(new { ok = true, status = "published", url });
            });
        });

        app.MapPost("/api/admin/content/posts/{id:long}/schedule", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_publish", adm =>
            {
                var at = H.GetS(b, "scheduled_at");
                if (string.IsNullOrWhiteSpace(at)) return Results.Json(new { error = "scheduled_at_required" }, statusCode: 400);
                // Enforce separation of duties at schedule time (mirrors /publish): a person can't schedule
                // their own article, so the ScheduledPublisher worker can later publish it as a system actor
                // without re-checking authorship.
                var p = db.QueryOne("SELECT author_id FROM blog_posts WHERE id=?", id);
                if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (SelfAuthored(db, p, adm)) return Results.Json(new { error = "self_publish_forbidden", message = "You cannot schedule your own article for publishing." }, statusCode: 403);
                db.Execute("UPDATE blog_posts SET status='scheduled', scheduled_at=?, updated_at=datetime('now') WHERE id=?", at, id);
                log(adm.Id, "content_post_schedule", "post " + id + " @ " + at);
                return J(new { ok = true, status = "scheduled", scheduled_at = at });
            });
        });

        app.MapPost("/api/admin/content/posts/{id:long}/unpublish", (HttpRequest req, long id) => gate(req, "cc_publish", adm =>
        {
            var p = db.QueryOne("SELECT slug FROM blog_posts WHERE id=?", id);
            if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            Snapshot(db, id, "unpublish", adm.Id);
            db.Execute("UPDATE blog_posts SET status='unpublished', published=0, updated_at=datetime('now') WHERE id=?", id);
            PageContent.Bump();
            log(adm.Id, "content_post_unpublish", "post " + id);
            return J(new { ok = true, status = "unpublished" });
        }));

        app.MapPost("/api/admin/content/posts/{id:long}/restore/{ver:long}", (HttpRequest req, long id, long ver) => gate(req, "cc_edit", adm =>
        {
            var v = db.QueryOne("SELECT snapshot_json FROM blog_post_versions WHERE post_id=? AND version=?", id, ver);
            if (v is null) return Results.Json(new { error = "version_not_found" }, statusCode: 404);
            Snapshot(db, id, "restore v" + ver, adm.Id);            // keep current before restoring (never destroy history)
            try
            {
                var snap = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(H.Str(v["snapshot_json"]) ?? "{}") ?? new();
                foreach (var col in TextCols) if (snap.TryGetValue(col, out var el)) db.Execute($"UPDATE blog_posts SET {col}=? WHERE id=?", el.ValueKind == JsonValueKind.Null ? null : el.ToString(), id);
                foreach (var col in IntCols) if (snap.TryGetValue(col, out var el) && el.ValueKind == JsonValueKind.Number) db.Execute($"UPDATE blog_posts SET {col}=? WHERE id=?", el.GetInt64(), id);
                db.Execute("UPDATE blog_posts SET version=version+1, updated_at=datetime('now') WHERE id=?", id);
            }
            catch (Exception e) { return Results.Json(new { error = "restore_failed", message = e.Message }, statusCode: 500); }
            log(adm.Id, "content_post_restore", "post " + id + " ← v" + ver);
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/content/posts/{id:long}/tags", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_edit", adm =>
            {
                db.Execute("DELETE FROM blog_post_tags WHERE post_id=?", id);
                if (H.GetEl(b, "tags") is { ValueKind: JsonValueKind.Array } arr)
                    foreach (var el in arr.EnumerateArray())
                    {
                        var name = el.GetString(); if (string.IsNullOrWhiteSpace(name)) continue;
                        var tslug = Blog.Slugify(name);
                        db.Execute("INSERT OR IGNORE INTO blog_tags(slug,name,created_at) VALUES(?,?, datetime('now'))", tslug, name.Trim());
                        var t = db.QueryOne("SELECT id FROM blog_tags WHERE slug=?", tslug);
                        if (t is not null) db.Execute("INSERT OR IGNORE INTO blog_post_tags(post_id,tag_id) VALUES(?,?)", id, H.L(t["id"]));
                    }
                return J(new { ok = true });
            });
        });

        // ── Lifecycle: archive (reversible), soft-delete (recoverable), purge (owner-only, permanent) ──
        // Every state change snapshots first, so nothing is lost; a soft-deleted post stays in the DB with
        // its full version history. Public + default admin lists exclude 'deleted'.
        app.MapPost("/api/admin/content/posts/{id:long}/archive", (HttpRequest req, long id) => gate(req, "cc_archive", adm =>
        {
            if (db.QueryOne("SELECT id FROM blog_posts WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            Snapshot(db, id, "archive", adm.Id);
            db.Execute("UPDATE blog_posts SET status='archived', published=0, updated_at=datetime('now') WHERE id=?", id);
            PageContent.Bump();
            log(adm.Id, "content_post_archive", "post " + id);
            return J(new { ok = true, status = "archived" });
        }));

        app.MapPost("/api/admin/content/posts/{id:long}/delete", (HttpRequest req, long id) => gate(req, "cc_delete", adm =>
        {
            if (db.QueryOne("SELECT id FROM blog_posts WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            Snapshot(db, id, "delete", adm.Id);
            db.Execute("UPDATE blog_posts SET status='deleted', published=0, updated_at=datetime('now') WHERE id=?", id);
            PageContent.Bump();
            log(adm.Id, "content_post_delete", "post " + id);
            return J(new { ok = true, status = "deleted" });
        }));

        app.MapPost("/api/admin/content/posts/{id:long}/purge", (HttpRequest req, long id) => gate(req, "cc_delete", adm =>
        {
            if (!adm.IsOwner) return Results.Json(new { error = "owner_only", message = "Permanent deletion is restricted to owners." }, statusCode: 403);
            var p = db.QueryOne("SELECT slug FROM blog_posts WHERE id=?", id);
            if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            db.Execute("DELETE FROM blog_post_tags WHERE post_id=?", id);
            db.Execute("DELETE FROM blog_post_versions WHERE post_id=?", id);
            db.Execute("DELETE FROM blog_reviews WHERE post_id=?", id);
            db.Execute("DELETE FROM cc_content_links WHERE post_id=?", id);
            db.Execute("DELETE FROM blog_posts WHERE id=?", id);
            PageContent.Bump();
            log(adm.Id, "content_post_purge", "post " + id + " " + (H.Str(p["slug"]) ?? ""));
            return J(new { ok = true, status = "purged" });
        }));

        // ── Content link registry: extract a post's outbound links, set rel/citation policy, HTTP-check ──
        app.MapPost("/api/admin/content/posts/{id:long}/links/scan", (HttpRequest req, long id) => gate(req, "cc_links", adm =>
        {
            if (db.QueryOne("SELECT id FROM blog_posts WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var (found, added) = ContentLinks.Scan(db, id, adm.Id);
            log(adm.Id, "content_links_scan", "post " + id + " found=" + found + " added=" + added);
            return J(new { ok = true, found, added });
        }));

        app.MapGet("/api/admin/content/posts/{id:long}/links", (HttpRequest req, long id) => gate(req, "cc_view", adm =>
            J(new { rows = db.Query("SELECT id,post_id,url,kind,anchor_text,rel,is_citation,approved,status,http_code,last_checked_at,active FROM cc_content_links WHERE post_id=? ORDER BY kind, id", id) })));

        app.MapPatch("/api/admin/content/links/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_links", adm =>
            {
                if (db.QueryOne("SELECT id FROM cc_content_links WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (H.GetS(b, "rel") is { } rel && new[] { "auto", "dofollow", "nofollow", "sponsored", "ugc" }.Contains(rel)) db.Execute("UPDATE cc_content_links SET rel=? WHERE id=?", rel, id);
                if (H.GetBool(b, "is_citation") is { } ic) db.Execute("UPDATE cc_content_links SET is_citation=? WHERE id=?", ic ? 1 : 0, id);
                if (H.GetBool(b, "approved") is { } ap) db.Execute("UPDATE cc_content_links SET approved=? WHERE id=?", ap ? 1 : 0, id);
                db.Execute("UPDATE cc_content_links SET updated_at=datetime('now') WHERE id=?", id);
                log(adm.Id, "content_link_update", "link " + id);
                return J(new { ok = true });
            });
        });

        app.MapPost("/api/admin/content/links/{id:long}/check", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "cc_links", _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            var adm = adminFromReq(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            var (status, code, err) = await ContentLinks.Check(db, id);
            log(adm?.Id, "content_link_check", "link " + id + " → " + status + (code > 0 ? " (" + code + ")" : ""));
            return J(new { ok = err is null, status, code, error = err });
        });

        // ══════════════════════════ ADMIN: authors / categories ══════════════════════════
        app.MapGet("/api/admin/content/authors", (HttpRequest req) => gate(req, "cc_view", adm => J(new { rows = db.Query("SELECT * FROM blog_authors ORDER BY sort_order, name") })));
        app.MapPost("/api/admin/content/authors", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "cc_edit", adm =>
            {
                var name = (H.GetS(b, "name") ?? "").Trim();
                if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var slug = Blog.Slugify(H.GetS(b, "slug") is { Length: > 0 } s ? s : name);
                var idExisting = db.QueryOne("SELECT id FROM blog_authors WHERE slug=?", slug);
                if (idExisting is not null) slug += "-" + DateTime.UtcNow.Ticks % 1000;
                var id = db.ExecuteReturningId("INSERT INTO blog_authors(slug,name,title,bio,credentials,email,links,admin_user_id,active,created_at,updated_at) VALUES(?,?,?,?,?,?,?,?,1, datetime('now'), datetime('now'))",
                    slug, name, H.GetS(b, "title"), H.GetS(b, "bio"), H.GetS(b, "credentials"), H.GetS(b, "email"), H.GetS(b, "links"), H.GetNum(b, "admin_user_id"));
                log(adm.Id, "content_author_create", name);
                return J(new { ok = true, id, slug });
            });
        });
        app.MapPatch("/api/admin/content/authors/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_edit", adm =>
            {
                foreach (var k in new[] { "name", "title", "bio", "credentials", "email", "links" })
                    if (H.GetS(b, k) is { } v) db.Execute($"UPDATE blog_authors SET {k}=? WHERE id=?", v, id);
                if (H.GetBool(b, "active") is { } act) db.Execute("UPDATE blog_authors SET active=? WHERE id=?", act ? 1 : 0, id);
                db.Execute("UPDATE blog_authors SET updated_at=datetime('now') WHERE id=?", id);
                return J(new { ok = true });
            });
        });
        app.MapGet("/api/admin/content/categories", (HttpRequest req) => gate(req, "cc_view", adm => J(new { rows = db.Query("SELECT * FROM blog_categories ORDER BY sort_order, name") })));
        app.MapPost("/api/admin/content/categories", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "cc_edit", adm =>
            {
                var name = (H.GetS(b, "name") ?? "").Trim();
                if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var slug = Blog.Slugify(H.GetS(b, "slug") is { Length: > 0 } s ? s : name);
                if (db.QueryOne("SELECT id FROM blog_categories WHERE slug=?", slug) is not null) slug += "-" + DateTime.UtcNow.Ticks % 1000;
                var id = db.ExecuteReturningId("INSERT INTO blog_categories(slug,name,description,parent_id,certification_id,sort_order,active,seo_title,meta_description,created_at) VALUES(?,?,?,?,?,?,1,?,?, datetime('now'))",
                    slug, name, H.GetS(b, "description"), H.GetNum(b, "parent_id"), H.GetNum(b, "certification_id"), (long)(H.GetNum(b, "sort_order") ?? 0), H.GetS(b, "seo_title"), H.GetS(b, "meta_description"));
                log(adm.Id, "content_category_create", name);
                return J(new { ok = true, id, slug });
            });
        });
        app.MapPatch("/api/admin/content/categories/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_edit", adm =>
            {
                foreach (var k in new[] { "name", "description", "seo_title", "meta_description" })
                    if (H.GetS(b, k) is { } v) db.Execute($"UPDATE blog_categories SET {k}=? WHERE id=?", v, id);
                if (H.GetBool(b, "active") is { } act) db.Execute("UPDATE blog_categories SET active=? WHERE id=?", act ? 1 : 0, id);
                return J(new { ok = true });
            });
        });

        // ══════════════════════════ ADMIN: Integration Capability Registry ══════════════════════════
        app.MapGet("/api/admin/content/capabilities", (HttpRequest req) => gate(req, "cc_integrations", adm =>
        {
            var rows = db.Query("SELECT * FROM content_capabilities ORDER BY kind, sort_order");
            foreach (var r in rows) r["connected"] = LiveConnected(H.Str(r["platform_key"]) ?? "", H.L(r["connected"]) == 1);
            return J(new { rows });
        }));
        app.MapPatch("/api/admin/content/capabilities/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_integrations", adm =>
            {
                if (H.GetS(b, "capability") is { } cap) db.Execute("UPDATE content_capabilities SET capability=? WHERE id=?", cap, id);
                if (H.GetS(b, "notes") is { } n) db.Execute("UPDATE content_capabilities SET notes=? WHERE id=?", n, id);
                if (H.GetBool(b, "active") is { } act) db.Execute("UPDATE content_capabilities SET active=? WHERE id=?", act ? 1 : 0, id);
                db.Execute("UPDATE content_capabilities SET updated_at=datetime('now') WHERE id=?", id);
                log(adm.Id, "content_capability_update", "cap " + id);
                return J(new { ok = true });
            });
        });

        // ══════════════════════════ ADMIN: AI Content Studio (assist-only) ══════════════════════════
        app.MapGet("/api/admin/content/ai/status", (HttpRequest req) => gate(req, "cc_ai", adm => J(new
        {
            openai = AiContent.OpenAiReady, anthropic = AiContent.AnthropicReady,
            providers = db.Query("SELECT id,provider,label,model,use_case,active FROM ai_content_providers ORDER BY sort_order,id"),
            note = "AI assists with drafting, editing, SEO and social variants. It never publishes — every output is reviewed by a human.",
        })));

        app.MapPost("/api/admin/content/ai/generate", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "cc_ai", _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            if ((db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='ai_content_enabled'") ?? "1") != "1")
                return Results.Json(new { error = "ai_disabled", message = "AI Content Studio is disabled in settings." }, statusCode: 403);
            var adm = adminFromReq(ctx.Request);
            var b = await H.Body(ctx.Request);
            var provider = (H.GetS(b, "provider") ?? "openai").ToLowerInvariant();
            if (!AiContent.Ready(provider))
                return Results.Json(new { error = "provider_not_configured", message = $"Set the {(provider == "openai" ? "OPENAI_API_KEY" : "ANTHROPIC_API_KEY")} environment variable to enable {provider}." }, statusCode: 400);
            var useCase = H.GetS(b, "use_case") ?? "draft";
            var prompt = (H.GetS(b, "prompt") ?? "").Trim();
            if (prompt.Length == 0) return Results.Json(new { error = "prompt_required" }, statusCode: 400);
            var cfg = db.QueryOne("SELECT * FROM ai_content_providers WHERE provider=? AND use_case=? AND active=1", provider, useCase)
                   ?? db.QueryOne("SELECT * FROM ai_content_providers WHERE provider=? AND active=1 ORDER BY id LIMIT 1", provider);
            var model = H.GetS(b, "model") ?? (cfg is null ? null : H.Str(cfg["model"]));
            var system = cfg is null ? DefaultSystem(useCase) : (H.Str(cfg["system_prompt"]) ?? DefaultSystem(useCase));
            var maxTokens = cfg is null ? 1200 : (int)H.L(cfg["max_tokens"]); if (maxTokens <= 0) maxTokens = 1200;
            var temp = cfg is null ? 0.7 : H.D(cfg["temperature"]);
            var res = await AiContent.Generate(provider, model, system, prompt, maxTokens, temp);
            var postId = H.GetNum(b, "post_id");
            db.Execute("INSERT INTO ai_content_generations(provider,model,use_case,post_id,prompt,output,tokens_in,tokens_out,status,created_by,created_at) VALUES(?,?,?,?,?,?,?,?,?,?, datetime('now'))",
                provider, model, useCase, postId, prompt, res.Ok ? res.Text : null, res.TokensIn, res.TokensOut, res.Ok ? "generated" : "error", adm?.Id);
            log(adm?.Id, "content_ai_generate", provider + "/" + useCase + (res.Ok ? " ok" : " err"));
            if (!res.Ok) return Results.Json(new { error = "generation_failed", message = res.Error }, statusCode: 502);
            return J(new { ok = true, text = res.Text, tokens_in = res.TokensIn, tokens_out = res.TokensOut, provider, model, disclaimer = "AI-assisted draft — review before use." });
        });

        app.MapGet("/api/admin/content/ai/generations", (HttpRequest req) => gate(req, "cc_ai", adm =>
            J(new { rows = db.Query("SELECT id,provider,model,use_case,post_id,status,tokens_out,human_approved,created_by,created_at FROM ai_content_generations ORDER BY id DESC LIMIT 100") })));

        // ══════════════════════════ ADMIN: SEO + IndexNow ══════════════════════════
        app.MapGet("/api/admin/content/seo/audit", (HttpRequest req) => gate(req, "cc_seo", adm =>
        {
            var posts = db.Query("SELECT * FROM blog_posts WHERE status='published' AND published=1");
            var issues = posts.Select(p => new { id = p["id"], slug = p["slug"], title = p["title"], checks = SeoCheck(db, p) }).Where(x => ((List<object>)x.checks).Count > 0).ToList();
            return J(new { audited = posts.Count, flagged = issues.Count, issues });
        }));

        app.MapPost("/api/admin/content/indexnow", (HttpRequest req) => gate(req, "cc_seo", adm =>
        {
            var urls = Blog.PublishedForFeed(db, 5000).Select(p => Blog.PublicUrl(db, H.Str(p["slug"]) ?? "")).ToList();
            if (urls.Count == 0) return J(new { ok = false, message = "No published posts to submit." });
            var (ok, detail) = IndexNowService.Submit(db, urls);
            log(adm.Id, "content_indexnow_submit", urls.Count + " urls: " + detail);
            return J(new { ok, submitted = urls.Count, detail });
        }));
    }

    // ══════════════════════════ helpers ══════════════════════════
    static void ApplyFields(Db db, long id, Dictionary<string, JsonElement> b)
    {
        foreach (var col in TextCols)
            if (b.TryGetValue(col, out var el))
                db.Execute($"UPDATE blog_posts SET {col}=? WHERE id=?", el.ValueKind == JsonValueKind.Null ? null : (col == "body" ? SanitizeBody(el.GetString(), b) : el.GetString()), id);
        foreach (var col in IntCols)
            if (b.TryGetValue(col, out var el))
            {
                long? v = el.ValueKind switch
                {
                    JsonValueKind.Number => el.GetInt64(),
                    JsonValueKind.True => 1, JsonValueKind.False => 0,
                    JsonValueKind.Null => null,
                    JsonValueKind.String => long.TryParse(el.GetString(), out var n) ? n : null,
                    _ => null,
                };
                db.Execute($"UPDATE blog_posts SET {col}=? WHERE id=?", v, id);
            }
        if (b.TryGetValue("extra_json", out var ex) && ex.ValueKind == JsonValueKind.Object)
            db.Execute("UPDATE blog_posts SET extra_json=? WHERE id=?", ex.ToString(), id);
    }

    static string? SanitizeBody(string? body, Dictionary<string, JsonElement> b)
    {
        if (string.IsNullOrEmpty(body)) return body;
        var fmt = b.TryGetValue("body_format", out var f) ? f.GetString() : "html";
        // Markdown bodies are converted+sanitised at render; HTML bodies are sanitised on save.
        return (fmt ?? "html").ToLowerInvariant() == "markdown" ? body : HtmlSanitize.Clean(body);
    }

    static void Snapshot(Db db, long id, string reason, long? editorId)
    {
        var p = db.QueryOne("SELECT * FROM blog_posts WHERE id=?", id);
        if (p is null) return;
        var ver = (int)H.L(p["version"]);
        var json = JsonSerializer.Serialize(p);
        db.Execute("INSERT INTO blog_post_versions(post_id,version,status_at,snapshot_json,change_reason,editor_id,created_at) VALUES(?,?,?,?,?,?, datetime('now'))",
            id, ver, H.Str(p["status"]), json, reason, editorId);
    }

    /// <summary>
    /// Publish a post — the single source of truth used by BOTH the /publish endpoint (after the caller has
    /// enforced the gate + separation-of-duties) and the ScheduledPublisher background worker (adminId=null →
    /// system actor; the human enforced SoD when they scheduled it). Idempotent-safe: republishing an already
    /// published post just refreshes reading time / sitemap. Returns (ok, error, publicUrl); error is
    /// "not_found" or "incomplete" when ok is false.
    /// </summary>
    public static (bool ok, string? error, string? url) PublishPost(Db db, long id, long? adminId, Action<long?, string, string?>? log)
    {
        var p = db.QueryOne("SELECT * FROM blog_posts WHERE id=?", id);
        if (p is null) return (false, "not_found", null);
        if ((H.Str(p["title"]) ?? "").Trim().Length == 0 || (H.Str(p["body"]) ?? "").Trim().Length == 0)
            return (false, "incomplete", null);
        Snapshot(db, id, "publish", adminId);
        var readMin = Blog.ReadingTime(H.Str(p["body"]));
        db.Execute(@"UPDATE blog_posts SET status='published', published=1,
            published_at=COALESCE(published_at, datetime('now')),
            original_published_at=COALESCE(original_published_at, published_at, datetime('now')),
            reading_time=?, robots_noindex=0, version=version+1, updated_at=datetime('now'),
            approved_by=COALESCE(?, approved_by), approved_at=datetime('now') WHERE id=?", readMin, adminId, id);
        PageContent.Bump();                                   // refresh sitemap (blog URLs included)
        try { ContentLinks.Scan(db, id, adminId); } catch { /* link registry is best-effort; never blocks publish */ }
        var slug = H.Str(p["slug"]) ?? "";
        QueueIndexNow(db, id, Blog.PublicUrl(db, slug), adminId);
        NotifyStaff(db, "Article published", "<p><strong>" + Esc(H.Str(p["title"]) ?? "") + "</strong> is now live at " + Esc(Blog.PublicUrl(db, slug)) + "</p>");
        log?.Invoke(adminId, "content_post_publish", "post " + id + " " + slug);
        return (true, null, Blog.PublicUrl(db, slug));
    }

    /// <summary>
    /// Keep a renamed LIVE post's old URL working: write an idempotent 301 from the old blog path to the new
    /// one, collapse any existing redirect that pointed at the old path (so the chain stays single-hop), and
    /// clear any stale redirect whose source is now the live new URL. Best-effort — a failure never blocks
    /// the rename.
    /// </summary>
    static void AddSlugRedirect(Db db, long postId, string oldSlug, string newSlug)
    {
        if (oldSlug.Length == 0 || newSlug.Length == 0 || string.Equals(oldSlug, newSlug, StringComparison.OrdinalIgnoreCase)) return;
        try
        {
            var bp = Blog.BasePath(db);
            var from = bp + "/" + oldSlug;
            var to = bp + "/" + newSlug;
            db.Execute("DELETE FROM seo_redirects WHERE from_path=?", to);                        // new URL is a live page, not a redirect source
            db.Execute("UPDATE seo_redirects SET to_url=? WHERE to_url=? AND active=1", to, from); // collapse chains old→new
            if (db.QueryOne("SELECT id FROM seo_redirects WHERE from_path=?", from) is null)
                db.Execute("INSERT INTO seo_redirects(from_path,to_url,status,active,note) VALUES(?,?,301,1,?)", from, to, "auto: blog slug change (post " + postId + ")");
            else
                db.Execute("UPDATE seo_redirects SET to_url=?, status=301, active=1 WHERE from_path=?", to, from);
            PathRedirects.Bump();
        }
        catch (Exception e) { Console.Error.WriteLine($"[content] slug redirect skipped: {e.Message}"); }
    }

    /// <summary>Enforce separation of duties: an admin linked to the post's author can't review/publish it.</summary>
    static bool SelfAuthored(Db db, Dictionary<string, object?> post, AdminCtx adm)
    {
        if (adm.IsOwner) return false;                                 // owner override
        var a = Blog.Author(db, post["author_id"]);
        return a is not null && a["admin_user_id"] is not null && H.L(a["admin_user_id"]) == adm.Id;
    }

    static void QueueIndexNow(Db db, long postId, string url, long? adminId)
    {
        if ((db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='blog_indexnow_on_publish'") ?? "1") != "1") return;
        try { IndexNowService.Submit(db, new[] { url }); } catch { /* best effort; sitemap remains the primary discovery path */ }
    }

    static void NotifyStaff(Db db, string subject, string html)
    { try { Notify.Alert(db, "content", subject, html, "content", null); } catch { } }

    /// <summary>Live connection state from env for the handful of destinations we can detect; else stored flag.</summary>
    static bool LiveConnected(string key, bool stored) => key switch
    {
        "indexnow" or "pagespeed" => true,
        "openai_api" => AiContent.OpenAiReady,
        "anthropic_api" => AiContent.AnthropicReady,
        "email_newsletter" => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RESEND_API_KEY")) || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SMTP_HOST")),
        _ => stored,
    };

    static List<object> SeoCheck(Db db, Dictionary<string, object?> p)
    {
        var issues = new List<object>();
        void Add(string code, string sev, string msg) => issues.Add(new { code, severity = sev, message = msg });
        var title = H.Str(p["seo_title"]) ?? H.Str(p["title"]) ?? "";
        var meta = H.Str(p["meta_description"]) ?? "";
        var body = H.Str(p["body"]) ?? "";
        if (title.Trim().Length == 0) Add("missing_title", "high", "No SEO title.");
        else if (title.Length > 65) Add("title_long", "low", "SEO title exceeds ~65 characters.");
        if (meta.Trim().Length == 0) Add("missing_meta", "medium", "No meta description.");
        else if (meta.Length > 165) Add("meta_long", "low", "Meta description exceeds ~165 characters.");
        if (H.Str(p["featured_image"]) is not { Length: > 0 }) Add("missing_image", "low", "No featured image (weak social preview).");
        else if (H.Str(p["featured_image_alt"]) is not { Length: > 0 }) Add("missing_alt", "medium", "Featured image has no alt text.");
        if (System.Text.RegularExpressions.Regex.Replace(body, "<[^>]+>", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries).Length < 300) Add("thin_content", "medium", "Fewer than ~300 words (thin content).");
        if (H.Str(p["primary_keyword"]) is not { Length: > 0 }) Add("no_keyword", "low", "No primary keyword set.");
        if (H.L(p["author_id"]) == 0) Add("no_author", "low", "No author assigned.");
        return issues;
    }

    static string DefaultSystem(string useCase) => useCase switch
    {
        "translate" => "You are a professional translator for a project-controls certification body. Translate faithfully; keep terminology and proper nouns intact. Output only the translation.",
        "seo_meta" => "You write concise, non-clickbait SEO titles (≤60 chars) and meta descriptions (≤155 chars) for professional certification content. Output plainly.",
        "social" => "You write platform-appropriate social copy for a professional body (LinkedIn/X/etc.). Accurate, no hype, no unverified claims. Output only the post text.",
        "citation_check" => "You identify claims that need a source and flag any that look unsupported. You never invent citations.",
        _ => "You are an expert editorial assistant for the Project Controls Institute (project controls, project finance, project delivery and AI). Write accurate, people-first, well-structured content. Do not fabricate facts, statistics or citations. Output is a draft for human review.",
    };

    // ── public projections ──
    static object PublicCard(Db db, Dictionary<string, object?> p)
    {
        var a = Blog.Author(db, p["author_id"]);
        var c = Blog.Category(db, p["category_id"]);
        return new
        {
            slug = p["slug"], title = p["title"], summary = p["summary"], featured_image = p["featured_image"],
            author = a is null ? null : H.Str(a["name"]), category = c is null ? null : H.Str(c["name"]),
            category_slug = c is null ? null : H.Str(c["slug"]),
            published_at = p["published_at"], reading_time = p["reading_time"], language = p["language"],
        };
    }

    static object PublicFull(Db db, Dictionary<string, object?> p)
    {
        var a = Blog.Author(db, p["author_id"]);
        var c = Blog.Category(db, p["category_id"]);
        return new
        {
            slug = p["slug"], title = p["title"], subtitle = p["subtitle"], summary = p["summary"],
            body_html = BlogBodyHtml(H.Str(p["body"]), H.Str(p["body_format"])),
            featured_image = p["featured_image"], featured_image_alt = p["featured_image_alt"],
            author = a is null ? null : new { name = H.Str(a["name"]), title = H.Str(a["title"]), slug = H.Str(a["slug"]), bio = H.Str(a["bio"]) },
            category = c is null ? null : new { name = H.Str(c["name"]), slug = H.Str(c["slug"]) },
            tags = Blog.Tags(db, H.L(p["id"])).Select(t => H.Str(t["name"])),
            published_at = p["published_at"], updated_at = p["updated_at"], reading_time = p["reading_time"],
            language = p["language"], canonical = Blog.PublicUrl(db, H.Str(p["slug"]) ?? ""),
            ai_assisted = H.L(p["ai_assisted"]) == 1, ai_disclosure = H.Str(p["ai_disclosure"]),
        };
    }

    static string BlogBodyHtml(string? body, string? fmt)
    {
        if (string.IsNullOrEmpty(body)) return "";
        return (fmt ?? "html").ToLowerInvariant() == "markdown" ? HtmlSanitize.Clean(body) : HtmlSanitize.Clean(body);
    }

    static string Esc(string s) => (s ?? "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
}
