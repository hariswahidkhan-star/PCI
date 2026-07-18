using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Public discussion forum (anonymous display-name posting → community flagging → admin moderation).
/// No accounts and no emails: authors choose a display name per post; abuse control is a salted,
/// truncated IP hash (never returned to clients) driving fixed-window rate limits, an invisible
/// honeypot field, a link cap, and flag-based auto-hold for admin review.
/// </summary>
public static class Forum
{
    // Categories are fixed server-side (key → display label); the frontend renders whatever this returns.
    static readonly (string Key, string Label)[] Cats =
    {
        ("general",     "General discussion"),
        ("exam-prep",   "Exam preparation"),
        ("domains",     "Body of Knowledge domains"),
        ("ai-controls", "AI in project controls"),
        ("careers",     "Careers"),
    };

    const int PAGE_SIZE = 20;

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        IResult Err(string code, int status) => Results.Json(new { error = code }, statusCode: status);

        // Salted, truncated IP hash: enough to bucket rate limits, deliberately NOT a reversible or
        // full identifier. Uses the trusted last X-Forwarded-For hop (see H.LastHopIp) + a server salt.
        var salt = Environment.GetEnvironmentVariable("FORUM_SALT") ?? "pci-forum-salt-v1";
        string IpHash(HttpRequest req) => Security.Sha(
            H.LastHopIp(req.Headers["x-forwarded-for"].ToString(), req.HttpContext.Connection.RemoteIpAddress?.ToString())
            + "|" + salt)[..16];

        // Fixed-window rate limiting backed by forum_actions (works across restarts, both DB providers).
        long CountSince(string ipHash, string action, string window) =>
            db.Scalar<long>($"SELECT COUNT(*) FROM forum_actions WHERE ip_hash=? AND action=? AND created_at > datetime('now','{window}')", ipHash, action);
        void RecordAction(string ipHash, string action) =>
            db.Execute("INSERT INTO forum_actions(ip_hash,action) VALUES(?,?)", ipHash, action);

        // ─────────────────────────── PUBLIC: read ───────────────────────────
        app.MapGet("/api/forum/categories", () => J(new
        {
            categories = Cats.Select(c => new
            {
                key = c.Key,
                label = c.Label,
                threads = db.Scalar<long>("SELECT COUNT(*) FROM forum_threads WHERE category=? AND status<>'hidden'", c.Key)
            })
        }));

        app.MapGet("/api/forum/threads", (HttpRequest req) =>
        {
            var category = req.Query["category"].ToString();
            var page = int.TryParse(req.Query["page"].ToString(), out var p) ? Math.Max(p, 1) : 1;
            var hasCat = Cats.Any(c => c.Key == category);
            var where = "status<>'hidden'" + (hasCat ? " AND category=?" : "");
            var args = hasCat ? new object?[] { category } : Array.Empty<object?>();
            var total = db.Scalar<long>($"SELECT COUNT(*) FROM forum_threads WHERE {where}", args);
            var rows = db.Query($"SELECT id,category,title,author_name,status,reply_count,created_at,last_post_at FROM forum_threads WHERE {where} ORDER BY last_post_at DESC, id DESC LIMIT {PAGE_SIZE} OFFSET {(page - 1) * PAGE_SIZE}", args);
            return J(new { threads = rows, total, page, page_size = PAGE_SIZE });
        });

        app.MapGet("/api/forum/threads/{id}", (long id) =>
        {
            var t = db.QueryOne("SELECT id,category,title,author_name,status,reply_count,created_at,last_post_at FROM forum_threads WHERE id=?", id);
            if (t is null || (t["status"] as string) == "hidden") return Err("not_found", 404);
            var posts = db.Query("SELECT id,author_name,body,created_at FROM forum_posts WHERE thread_id=? AND status='live' ORDER BY id ASC", id);
            return J(new { thread = t, posts });
        });

        // ─────────────────────────── PUBLIC: write ───────────────────────────
        app.MapPost("/api/forum/threads", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            // Honeypot: real users never see the 'website' field. A filled value is a bot — answer
            // ok so the bot believes it succeeded, but store nothing (and burn no rate budget).
            if (!string.IsNullOrEmpty(H.GetS(b, "website"))) return J(new { ok = true });
            var name = Clean(H.GetS(b, "name") ?? "");
            var title = Clean(H.GetS(b, "title") ?? "");
            var body = Clean(H.GetS(b, "body") ?? "");
            var category = (H.GetS(b, "category") ?? "").Trim();
            if (name.Length is < 2 or > 60) return Err("bad_name", 400);
            if (title.Length is < 8 or > 140) return Err("bad_title", 400);
            if (body.Length is < 20 or > 5000) return Err("bad_body", 400);
            if (!Cats.Any(c => c.Key == category)) return Err("bad_category", 400);
            if (LinkCount(body) > 2) return Err("too_many_links", 400);
            var ip = IpHash(req);
            if (CountSince(ip, "thread", "-2 minutes") >= 1 || CountSince(ip, "thread", "-1 hours") >= 3)
                return Err("rate_limited", 429);
            long id = 0;
            db.Transaction(() =>
            {
                id = db.ExecuteReturningId("INSERT INTO forum_threads(category,title,author_name) VALUES(?,?,?)", category, title, name);
                db.Execute("INSERT INTO forum_posts(thread_id,author_name,body,ip_hash) VALUES(?,?,?,?)", id, name, body, ip);
                RecordAction(ip, "thread");
            });
            log(null, "forum_thread", id.ToString());
            return J(new { ok = true, id });
        });

        app.MapPost("/api/forum/threads/{id}/posts", async (HttpRequest req, long id) =>
        {
            var b = await H.Body(req);
            if (!string.IsNullOrEmpty(H.GetS(b, "website"))) return J(new { ok = true });   // honeypot (see above)
            var name = Clean(H.GetS(b, "name") ?? "");
            var body = Clean(H.GetS(b, "body") ?? "");
            if (name.Length is < 2 or > 60) return Err("bad_name", 400);
            if (body.Length is < 5 or > 5000) return Err("bad_body", 400);
            if (LinkCount(body) > 2) return Err("too_many_links", 400);
            var t = db.QueryOne("SELECT id,status FROM forum_threads WHERE id=?", id);
            if (t is null || (t["status"] as string) == "hidden") return Err("not_found", 404);
            if ((t["status"] as string) == "locked") return Err("locked", 409);
            var ip = IpHash(req);
            if (CountSince(ip, "post", "-30 seconds") >= 1 || CountSince(ip, "post", "-1 hours") >= 20)
                return Err("rate_limited", 429);
            long pid = 0;
            db.Transaction(() =>
            {
                pid = db.ExecuteReturningId("INSERT INTO forum_posts(thread_id,author_name,body,ip_hash) VALUES(?,?,?,?)", id, name, body, ip);
                db.Execute("UPDATE forum_threads SET reply_count=reply_count+1, last_post_at=datetime('now') WHERE id=?", id);
                RecordAction(ip, "post");
            });
            log(null, "forum_post", pid.ToString());
            return J(new { ok = true, id = pid });
        });

        // Community flagging: 3 flags auto-hold a live post (hidden pending admin review). The response
        // never reveals the resulting state, so reporters can't probe the threshold.
        app.MapPost("/api/forum/posts/{id}/report", (HttpRequest req, long id) =>
        {
            var ip = IpHash(req);
            if (CountSince(ip, "report", "-1 hours") >= 5) return Err("rate_limited", 429);
            var post = db.QueryOne("SELECT id,flags,status FROM forum_posts WHERE id=?", id);
            if (post is null) return Err("not_found", 404);
            db.Execute("UPDATE forum_posts SET flags=flags+1 WHERE id=?", id);
            db.Execute("UPDATE forum_posts SET status='hidden' WHERE id=? AND status='live' AND flags>=3", id);
            RecordAction(ip, "report");
            log(null, "forum_report", id.ToString());
            return J(new { ok = true });
        });

        // ─────────────────────────── ADMIN: moderate (gated: content, like Reviews) ───────────────────────────
        // ip_hash is NEVER selected — not even for admins (the spec: never return it to clients).
        app.MapGet("/api/admin/forum/queue", (HttpRequest req) => gate(req, "content", _ => J(new
        {
            posts = db.Query(@"SELECT p.id,p.thread_id,p.author_name,p.body,p.status,p.flags,p.created_at,t.title AS thread_title
                               FROM forum_posts p LEFT JOIN forum_threads t ON t.id=p.thread_id
                               WHERE p.status='hidden' OR p.flags>0 ORDER BY (p.status='hidden') DESC, p.flags DESC, p.id DESC"),
            threads = db.Query("SELECT id,category,title,author_name,status,reply_count,created_at,last_post_at FROM forum_threads WHERE status='hidden' ORDER BY id DESC")
        })));

        app.MapPost("/api/admin/forum/posts/{id}", (HttpRequest req, long id) => gate(req, "content", a =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var action = H.GetS(b, "action");
            var post = db.QueryOne("SELECT id,thread_id FROM forum_posts WHERE id=?", id);
            if (post is null) return Err("not_found", 404);
            switch (action)
            {
                case "hide": db.Execute("UPDATE forum_posts SET status='hidden' WHERE id=?", id); break;
                case "restore": db.Execute("UPDATE forum_posts SET status='live', flags=0 WHERE id=?", id); break;
                case "delete":
                    db.Execute("DELETE FROM forum_posts WHERE id=?", id);
                    db.Execute("UPDATE forum_threads SET reply_count=CASE WHEN reply_count>0 THEN reply_count-1 ELSE 0 END WHERE id=?", post["thread_id"]);
                    break;
                default: return Err("bad_action", 400);
            }
            log(a.Id, "forum_post_" + action, id.ToString());
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/forum/threads/{id}", (HttpRequest req, long id) => gate(req, "content", a =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var action = H.GetS(b, "action");
            var t = db.QueryOne("SELECT id FROM forum_threads WHERE id=?", id);
            if (t is null) return Err("not_found", 404);
            switch (action)
            {
                case "hide": db.Execute("UPDATE forum_threads SET status='hidden' WHERE id=?", id); break;
                case "restore": case "unlock": db.Execute("UPDATE forum_threads SET status='live' WHERE id=?", id); break;
                case "lock": db.Execute("UPDATE forum_threads SET status='locked' WHERE id=?", id); break;
                case "delete":
                    db.Transaction(() =>
                    {
                        db.Execute("DELETE FROM forum_posts WHERE thread_id=?", id);
                        db.Execute("DELETE FROM forum_threads WHERE id=?", id);
                    });
                    break;
                default: return Err("bad_action", 400);
            }
            log(a.Id, "forum_thread_" + action, id.ToString());
            return J(new { ok = true });
        }));
    }

    // Strip control characters (keep newlines/tabs so post bodies keep their paragraphs), collapse \r\n.
    static string Clean(string s) =>
        new string(s.Replace("\r\n", "\n").Where(c => !char.IsControl(c) || c is '\n' or '\t').ToArray()).Trim();

    // Anti-spam link cap: count "http" occurrences (covers http:// and https://), case-insensitive.
    static int LinkCount(string body)
    {
        int n = 0, i = 0;
        while ((i = body.IndexOf("http", i, StringComparison.OrdinalIgnoreCase)) >= 0) { n++; i += 4; }
        return n;
    }
}
