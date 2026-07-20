using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Social publishing (Phase 2) — connect accounts for the platforms whose official APIs need no platform
/// review (Discord / Telegram / Mastodon / Bluesky), generate platform-specific drafts per blog post,
/// approve them, and deliver through the SocialDispatcher outbox with idempotency + retry. Credentials are
/// encrypted at rest and never returned. Approval-gated platforms are refused honestly at connect time.
/// All actions gated by cc_social and audit-logged. A failed social delivery never affects the article.
/// </summary>
public static class SocialPublishing
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate, Func<HttpRequest, AdminCtx?> adminFromReq)
    {
        IResult J(object o) => Results.Json(o);
        bool IsOk(IResult r) => r is Microsoft.AspNetCore.Http.HttpResults.Ok;

        object Redact(Dictionary<string, object?> a) => new
        {
            id = a["id"], platform_key = a["platform_key"], label = a["label"], external_id = a["external_id"],
            config = a["config"], status = a["status"], last_error = a["last_error"], active = a["active"],
            has_secret = !string.IsNullOrEmpty(H.Str(a["secret_enc"])), created_at = a["created_at"],
        };

        // ── accounts ────────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/social/accounts", (HttpRequest req) => gate(req, "cc_social", adm =>
            J(new { rows = db.Query("SELECT * FROM cc_social_accounts ORDER BY id DESC").Select(Redact), live_platforms = SocialConnectors.Live })));

        app.MapPost("/api/admin/content/social/accounts", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "cc_social", adm =>
            {
                var platform = (H.GetS(b, "platform_key") ?? "").ToLowerInvariant();
                if (!SocialConnectors.IsLive(platform))
                    return Results.Json(new { error = "requires_approval", message = $"{platform} publishing needs provider approval and is not available in this phase. See the capability registry." }, statusCode: 400);
                var secret = H.GetS(b, "secret") ?? "";           // webhook URL / bot token / access token / app password
                if (secret.Trim().Length == 0) return Results.Json(new { error = "secret_required" }, statusCode: 400);
                // non-secret config (chat_id / instance / handle / pds / channel_username / api_base)
                var cfg = new Dictionary<string, string>();
                foreach (var k in new[] { "chat_id", "channel_username", "instance", "handle", "pds", "api_base" })
                    if (H.GetS(b, k) is { Length: > 0 } v) cfg[k] = v.Trim();
                var id = db.ExecuteReturningId(@"INSERT INTO cc_social_accounts(platform_key,label,external_id,config,secret_enc,status,connected_by,active,created_at,updated_at)
                    VALUES(?,?,?,?,?, 'connected', ?, 1, datetime('now'), datetime('now'))",
                    platform, H.GetS(b, "label") ?? platform, H.GetS(b, "external_id"),
                    JsonSerializer.Serialize(cfg), Security.EncryptSecret(secret), adm.Id);
                log(adm.Id, "social_account_connect", platform + " #" + id);
                return J(new { ok = true, id });
            });
        });

        app.MapPost("/api/admin/content/social/accounts/{id:long}/test", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "cc_social", _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            var a = db.QueryOne("SELECT * FROM cc_social_accounts WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var res = await SocialConnectors.Test(db, a);
            db.Execute("UPDATE cc_social_accounts SET status=?, last_error=?, updated_at=datetime('now') WHERE id=?",
                res.Ok ? "connected" : "error", res.Error, id);
            return J(new { ok = res.Ok, error = res.Error });
        });

        app.MapPost("/api/admin/content/social/accounts/{id:long}/disconnect", (HttpRequest req, long id) => gate(req, "cc_social", adm =>
        {
            db.Execute("UPDATE cc_social_accounts SET active=0, status='disconnected', updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "social_account_disconnect", "#" + id);
            return J(new { ok = true });
        }));

        // ── drafts ──────────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/social/drafts", (HttpRequest req) => gate(req, "cc_social", adm =>
        {
            var postId = req.Query["post_id"].ToString();
            var sql = "SELECT d.*, a.platform_key AS acct_platform, a.label AS acct_label FROM cc_social_drafts d LEFT JOIN cc_social_accounts a ON a.id=d.account_id";
            var rows = postId.Length > 0 ? db.Query(sql + " WHERE d.post_id=? ORDER BY d.id DESC", long.Parse(postId))
                                         : db.Query(sql + " ORDER BY d.id DESC LIMIT 100");
            return J(new { rows });
        }));

        // Generate one draft per connected live account for a post (platform-tailored text).
        app.MapPost("/api/admin/content/posts/{id:long}/social/generate", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_social", adm =>
            {
                var post = db.QueryOne("SELECT * FROM blog_posts WHERE id=?", id);
                if (post is null) return Results.Json(new { error = "post_not_found" }, statusCode: 404);
                var link = Blog.PublicUrl(db, H.Str(post["slug"]) ?? "");
                var tags = Blog.Tags(db, id).Select(t => H.Str(t["name"]) ?? "").Where(s => s.Length > 0).ToList();
                var accounts = db.Query("SELECT * FROM cc_social_accounts WHERE active=1");
                var created = new List<object>();
                foreach (var a in accounts)
                {
                    var platform = H.Str(a["platform_key"]) ?? "";
                    // skip if a draft already exists for this (post, account) that isn't published/failed
                    var dup = db.QueryOne("SELECT id FROM cc_social_drafts WHERE post_id=? AND account_id=? AND status NOT IN ('published','failed','cancelled')", id, H.L(a["id"]));
                    if (dup is not null) continue;
                    var (text, hashtags) = Compose(platform, H.Str(post["title"]) ?? "", H.Str(post["summary"]) ?? "", tags, link);
                    var did = db.ExecuteReturningId(@"INSERT INTO cc_social_drafts(post_id,platform_key,account_id,text,link,hashtags,status,created_by,created_at,updated_at)
                        VALUES(?,?,?,?,?,?, 'draft', ?, datetime('now'), datetime('now'))",
                        id, platform, H.L(a["id"]), text, link, hashtags, adm.Id);
                    created.Add(new { id = did, platform, account = H.Str(a["label"]) });
                }
                log(adm.Id, "social_drafts_generate", "post " + id + " → " + created.Count);
                return J(new { ok = true, created });
            });
        });

        app.MapPatch("/api/admin/content/social/drafts/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_social", adm =>
            {
                foreach (var k in new[] { "text", "link", "hashtags", "first_comment", "scheduled_at" })
                    if (H.GetS(b, k) is { } v) db.Execute($"UPDATE cc_social_drafts SET {k}=? WHERE id=?", v, id);
                db.Execute("UPDATE cc_social_drafts SET updated_at=datetime('now') WHERE id=?", id);
                return J(new { ok = true });
            });
        });

        // Approve + queue for delivery (idempotent — the job's unique key blocks a double-queue).
        app.MapPost("/api/admin/content/social/drafts/{id:long}/publish", (HttpRequest req, long id) => gate(req, "cc_social", adm =>
        {
            var d = db.QueryOne("SELECT * FROM cc_social_drafts WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var acct = db.QueryOne("SELECT platform_key FROM cc_social_accounts WHERE id=? AND active=1", H.L(d["account_id"]));
            if (acct is null) return Results.Json(new { error = "account_inactive" }, statusCode: 400);
            if (!SocialConnectors.IsLive(H.Str(acct["platform_key"]) ?? ""))
                return Results.Json(new { error = "requires_approval" }, statusCode: 400);
            var (jobId, changes) = db.ExecuteWithChanges(@"INSERT OR IGNORE INTO content_jobs(job_type,idempotency_key,post_id,target,payload,status,next_attempt_at,created_by,created_at,updated_at)
                VALUES('social_publish', ?, ?, ?, ?, 'pending', datetime('now'), ?, datetime('now'), datetime('now'))",
                "socialdraft:" + id, H.L(d["post_id"]), H.Str(d["platform_key"]), "{\"draft_id\":" + id + "}", adm.Id);
            db.Execute("UPDATE cc_social_drafts SET status=?, approved_by=?, job_id=?, updated_at=datetime('now') WHERE id=?",
                "approved", adm.Id, changes > 0 ? jobId : H.L(d["job_id"]), id);
            log(adm.Id, "social_draft_publish", "draft " + id + (changes > 0 ? " queued" : " already_queued"));
            return J(new { ok = true, queued = changes > 0 });
        }));

        app.MapPost("/api/admin/content/social/drafts/{id:long}/cancel", (HttpRequest req, long id) => gate(req, "cc_social", adm =>
        {
            db.Execute("UPDATE cc_social_drafts SET status='cancelled', updated_at=datetime('now') WHERE id=? AND status IN ('draft','approved','retrying')", id);
            db.Execute("UPDATE content_jobs SET status='cancelled', updated_at=datetime('now') WHERE idempotency_key=? AND status IN ('pending','retrying')", "socialdraft:" + id);
            log(adm.Id, "social_draft_cancel", "draft " + id);
            return J(new { ok = true });
        }));

        // Manual drain / retry (the background dispatcher also runs every ~20s).
        app.MapPost("/api/admin/content/social/drain", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "cc_social", _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            var n = await SocialDispatcher.DrainOnce(db, 25);
            return J(new { ok = true, delivered = n });
        });
    }

    /// <summary>Platform-tailored copy. LinkedIn-style long form isn't here (approval-gated); the live
    /// platforms get concise, accurate copy with the canonical link and (where used) hashtags.</summary>
    static (string text, string hashtags) Compose(string platform, string title, string summary, List<string> tags, string link)
    {
        var tagline = summary.Length > 0 ? summary : title;
        var hashtags = string.Join(" ", tags.Take(4).Select(t => "#" + t.Replace(" ", "")));
        return platform switch
        {
            "telegram" => ($"📊 {title}\n\n{Cap(tagline, 500)}\n\n{link}", hashtags),
            "discord" => ($"**{title}**\n{Cap(tagline, 400)}\n{link}", hashtags),
            "mastodon" => ($"{title}\n\n{Cap(tagline, 300)}\n\n{link}" + (hashtags.Length > 0 ? "\n\n" + hashtags : ""), hashtags),
            "bluesky" => ($"{title} — {Cap(tagline, 180)}", hashtags),   // link appended by the connector; 300-char cap
            _ => ($"{title}\n\n{Cap(tagline, 280)}\n\n{link}", hashtags),
        };
    }
    static string Cap(string s, int n) => s.Length <= n ? s : s[..(n - 1)].TrimEnd() + "…";
}
