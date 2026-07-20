using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Content syndication (Phase 3) — connect partner CMS destinations whose official APIs need no provider
/// review (WordPress self-hosted / Ghost / Forem-DEV), then cross-post an approved PCI blog article with its
/// canonical pointing back to the original, delivered through the SyndicationDispatcher outbox with
/// idempotency + retry. Credentials are encrypted at rest and never returned; approval-gated destinations are
/// refused honestly at connect time. All actions gated by cc_syndicate and audit-logged. A failed
/// syndication never affects the source article.
/// </summary>
public static class Syndication
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate, Func<HttpRequest, AdminCtx?> adminFromReq)
    {
        IResult J(object o) => Results.Json(o);
        bool IsOk(IResult r) => r is Microsoft.AspNetCore.Http.HttpResults.Ok;

        object Redact(Dictionary<string, object?> d) => new
        {
            id = d["id"], platform_key = d["platform_key"], label = d["label"], base_url = d["base_url"],
            config = d["config"], mode = d["mode"], default_status = d["default_status"], status = d["status"],
            last_error = d["last_error"], active = d["active"], has_secret = !string.IsNullOrEmpty(H.Str(d["secret_enc"])),
            created_at = d["created_at"],
        };

        // ── destinations ──────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/syndication/destinations", (HttpRequest req) => gate(req, "cc_syndicate", adm =>
            J(new { rows = db.Query("SELECT * FROM cc_syndication_destinations ORDER BY id DESC").Select(Redact), live_platforms = SyndicationConnectors.Live })));

        app.MapPost("/api/admin/content/syndication/destinations", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "cc_syndicate", adm =>
            {
                var platform = (H.GetS(b, "platform_key") ?? "").ToLowerInvariant();
                if (!SyndicationConnectors.IsLive(platform))
                    return Results.Json(new { error = "requires_approval", message = $"{platform} needs provider onboarding and is not available in this phase. See the capability registry." }, statusCode: 400);
                var baseUrl = (H.GetS(b, "base_url") ?? "").Trim();
                if (baseUrl.Length == 0) return Results.Json(new { error = "base_url_required" }, statusCode: 400);
                var secret = H.GetS(b, "secret") ?? "";     // app password (WP) / admin key (Ghost) / api key (Forem)
                if (secret.Trim().Length == 0) return Results.Json(new { error = "secret_required" }, statusCode: 400);
                var cfg = new Dictionary<string, string>();
                if (H.GetS(b, "username") is { Length: > 0 } u) cfg["username"] = u.Trim();   // WordPress
                var mode = H.GetS(b, "mode") is "create_update" ? "create_update" : "create";
                var defStatus = H.GetS(b, "default_status") is "published" ? "published" : "draft";
                var id = db.ExecuteReturningId(@"INSERT INTO cc_syndication_destinations(platform_key,label,base_url,config,secret_enc,mode,default_status,status,connected_by,active,created_at,updated_at)
                    VALUES(?,?,?,?,?,?,?, 'connected', ?, 1, datetime('now'), datetime('now'))",
                    platform, H.GetS(b, "label") ?? platform, baseUrl, JsonSerializer.Serialize(cfg), Security.EncryptSecret(secret), mode, defStatus, adm.Id);
                log(adm.Id, "syndication_connect", platform + " #" + id);
                return J(new { ok = true, id });
            });
        });

        app.MapPost("/api/admin/content/syndication/destinations/{id:long}/test", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "cc_syndicate", _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            var d = db.QueryOne("SELECT * FROM cc_syndication_destinations WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var res = await SyndicationConnectors.Test(d);
            db.Execute("UPDATE cc_syndication_destinations SET status=?, last_error=?, updated_at=datetime('now') WHERE id=?",
                res.Ok ? "connected" : "error", res.Error, id);
            return J(new { ok = res.Ok, error = res.Error });
        });

        app.MapPost("/api/admin/content/syndication/destinations/{id:long}/disconnect", (HttpRequest req, long id) => gate(req, "cc_syndicate", adm =>
        {
            db.Execute("UPDATE cc_syndication_destinations SET active=0, status='disconnected', updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "syndication_disconnect", "#" + id);
            return J(new { ok = true });
        }));

        // ── syndicated posts ──────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/syndication/posts", (HttpRequest req) => gate(req, "cc_syndicate", adm =>
        {
            var postId = req.Query["post_id"].ToString();
            var sql = "SELECT s.*, d.platform_key AS dest_platform, d.label AS dest_label FROM cc_syndicated_posts s LEFT JOIN cc_syndication_destinations d ON d.id=s.destination_id";
            var rows = postId.Length > 0 ? db.Query(sql + " WHERE s.post_id=? ORDER BY s.id DESC", long.Parse(postId))
                                         : db.Query(sql + " ORDER BY s.id DESC LIMIT 100");
            return J(new { rows });
        }));

        // Queue a post to one/all active destinations. Only published posts can be syndicated.
        app.MapPost("/api/admin/content/posts/{id:long}/syndicate", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_syndicate", adm =>
            {
                var post = db.QueryOne("SELECT id,slug,status,published,version,canonical_url FROM blog_posts WHERE id=?", id);
                if (post is null) return Results.Json(new { error = "post_not_found" }, statusCode: 404);
                if (H.L(post["published"]) != 1) return Results.Json(new { error = "post_not_published", message = "Only a published article can be syndicated." }, statusCode: 400);
                var canonical = H.Str(post["canonical_url"]);
                if (string.IsNullOrEmpty(canonical)) canonical = Blog.PublicUrl(db, H.Str(post["slug"]) ?? "");
                var version = H.L(post["version"]);
                // explicit destination_ids, else every active destination
                var destIds = new List<long>();
                if (H.GetEl(b, "destination_ids") is { ValueKind: JsonValueKind.Array } arr)
                    foreach (var e in arr.EnumerateArray()) if (e.TryGetInt64(out var v)) destIds.Add(v);
                var dests = destIds.Count > 0
                    ? db.Query("SELECT * FROM cc_syndication_destinations WHERE active=1 AND id IN (" + string.Join(",", destIds.Select(_ => "?")) + ")", destIds.Cast<object?>().ToArray())
                    : db.Query("SELECT * FROM cc_syndication_destinations WHERE active=1");
                var queued = new List<object>();
                foreach (var d in dests)
                {
                    var destId = H.L(d["id"]);
                    // upsert the syndicated-post row (unique on post+dest)
                    db.Execute(@"INSERT OR IGNORE INTO cc_syndicated_posts(post_id,destination_id,canonical_url,mode,status,created_by,created_at,updated_at)
                        VALUES(?,?,?,?, 'pending', ?, datetime('now'), datetime('now'))", id, destId, canonical, H.Str(d["mode"]), adm.Id);
                    var synId = db.Scalar<long>("SELECT id FROM cc_syndicated_posts WHERE post_id=? AND destination_id=?", id, destId);
                    db.Execute("UPDATE cc_syndicated_posts SET canonical_url=?, updated_at=datetime('now') WHERE id=?", canonical, synId);
                    var (jobId, changes) = db.ExecuteWithChanges(@"INSERT OR IGNORE INTO content_jobs(job_type,idempotency_key,post_id,target,payload,status,next_attempt_at,created_by,created_at,updated_at)
                        VALUES('syndicate', ?, ?, ?, ?, 'pending', datetime('now'), ?, datetime('now'), datetime('now'))",
                        $"syndicate:{synId}:v{version}", id, H.Str(d["platform_key"]), "{\"syndicated_id\":" + synId + "}", adm.Id);
                    if (changes > 0) { db.Execute("UPDATE cc_syndicated_posts SET job_id=?, status='pending' WHERE id=?", jobId, synId); queued.Add(new { syndicated_id = synId, destination = H.Str(d["label"]) }); }
                }
                log(adm.Id, "syndicate_queue", "post " + id + " → " + queued.Count);
                return J(new { ok = true, queued });
            });
        });

        // Retry a failed/stuck syndication (re-arms its job and drains once).
        app.MapPost("/api/admin/content/syndication/posts/{id:long}/retry", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "cc_syndicate", _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            var synId = long.Parse((string)ctx.Request.RouteValues["id"]!);
            db.Execute("UPDATE content_jobs SET status='pending', next_attempt_at=datetime('now'), updated_at=datetime('now') WHERE job_type='syndicate' AND idempotency_key LIKE ? AND status IN ('failed','retrying')", $"syndicate:{synId}:%");
            db.Execute("UPDATE cc_syndicated_posts SET status='pending', updated_at=datetime('now') WHERE id=? AND status IN ('failed','retrying')", synId);
            var n = await SyndicationDispatcher.DrainOnce(db, 5);
            return J(new { ok = true, delivered = n });
        });

        // Manual drain (the background dispatcher also runs every ~20s).
        app.MapPost("/api/admin/content/syndication/drain", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "cc_syndicate", _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            var n = await SyndicationDispatcher.DrainOnce(db, 25);
            return J(new { ok = true, delivered = n });
        });
    }
}
