using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin Console → AI Visibility (master-plan Phase 6). Surfaces and controls how AI answer engines
/// see PCI:
///
///   GET  /api/admin/ai-visibility/overview  — readiness (llms.txt / robots.txt / sitemap, structured
///                                              -data coverage, allowed vs blocked crawlers, hit totals)
///   GET  /api/admin/ai-visibility/crawlers   — observed AI-crawler traffic (by bot / page / day)
///   GET  /api/admin/ai-visibility/policy      — the AI-crawler catalogue with each bot's block state
///   POST /api/admin/ai-visibility/policy      — set which crawlers are blocked in robots.txt
///
/// Read/observe is fully self-contained (no external credentials). Gated by the existing 'pages'
/// permission (same surface as SEO); the policy write is audit-logged.
/// </summary>
public static class AdminAiVisibility
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate, string webRoot)
    {
        IResult J(object o) => Results.Json(o);
        IResult? Deny(HttpRequest req)
        {
            var r = gate(req, "pages", _ => Results.Ok());
            return r is Microsoft.AspNetCore.Http.HttpResults.Ok ? null : r;
        }
        string Since(HttpRequest req)
        {
            var days = int.TryParse(req.Query["days"], out var d) ? Math.Clamp(d, 1, 365) : 30;
            return DateTime.UtcNow.AddDays(-days).ToString("yyyy-MM-dd HH:mm:ss");
        }

        // ---------- overview / readiness ----------
        app.MapGet("/api/admin/ai-visibility/overview", (HttpRequest req) => gate(req, "pages", _ =>
        {
            var host = Redirects.CanonicalBase;
            var blocked = AiVisibility.Blocked(db);

            // structured-data coverage: how many public pages carry JSON-LD the AI engines can extract
            int total = 0, withJsonLd = 0;
            var shells = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "student.html", "admin.html", "exam-ui.html", "404.html", "500.html", "offline.html" };
            if (Directory.Exists(webRoot))
                foreach (var f in Directory.EnumerateFiles(webRoot, "*.html"))
                {
                    var name = Path.GetFileName(f);
                    if (name is null || shells.Contains(name)) continue;
                    total++;
                    try { if (File.ReadAllText(f).Contains("application/ld+json", StringComparison.OrdinalIgnoreCase)) withJsonLd++; }
                    catch { /* unreadable file — counts as without */ }
                }

            var hits30 = db.Scalar<long>("SELECT COUNT(*) FROM analytics_events WHERE event='ai_crawler' AND created_at>=?",
                DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"));
            var botsSeen = db.Scalar<long>("SELECT COUNT(DISTINCT detail) FROM analytics_events WHERE event='ai_crawler'");

            return J(new
            {
                canonical_host = host,
                llms_url = host + "/llms.txt",
                robots_url = host + "/robots.txt",
                sitemap_url = host + "/sitemap.xml",
                structured_data = new { pages = total, with_json_ld = withJsonLd, coverage_pct = total > 0 ? (int)Math.Round(100.0 * withJsonLd / total) : 0 },
                crawlers = new { known = AiVisibility.Crawlers.Count, blocked = blocked.Count, allowed = AiVisibility.Crawlers.Count - blocked.Count },
                traffic = new { hits_30d = hits30, distinct_bots = botsSeen },
            });
        }));

        // ---------- observed AI-crawler traffic ----------
        app.MapGet("/api/admin/ai-visibility/crawlers", (HttpRequest req) => gate(req, "pages", _ =>
        {
            var since = Since(req);
            List<Dictionary<string, object?>> Q(string sql) => db.Query(sql, since);
            return J(new
            {
                total = db.Scalar<long>("SELECT COUNT(*) FROM analytics_events WHERE event='ai_crawler' AND created_at>=?", since),
                by_bot = Q("SELECT COALESCE(detail,'unknown') bot, COUNT(*) n, MAX(created_at) last_seen FROM analytics_events WHERE event='ai_crawler' AND created_at>=? GROUP BY detail ORDER BY n DESC"),
                by_page = Q("SELECT path, COUNT(*) n FROM analytics_events WHERE event='ai_crawler' AND created_at>=? GROUP BY path ORDER BY n DESC LIMIT 20"),
                daily = Q("SELECT substr(created_at,1,10) day, COUNT(*) n FROM analytics_events WHERE event='ai_crawler' AND created_at>=? GROUP BY substr(created_at,1,10) ORDER BY day"),
            });
        }));

        // ---------- crawler policy (robots.txt) ----------
        app.MapGet("/api/admin/ai-visibility/policy", (HttpRequest req) => gate(req, "pages", _ =>
        {
            var blocked = AiVisibility.Blocked(db);
            var rows = AiVisibility.Crawlers.Select(c => new
            {
                token = c.Token, label = c.Label, vendor = c.Vendor, purpose = c.Purpose,
                detectable = c.Ua is not null,          // false = robots-controllable but fetches under a shared UA
                blocked = blocked.Contains(c.Token),
            });
            return J(new { rows, blocked_count = blocked.Count, robots_preview = AiVisibility.Robots(db) });
        }));

        app.MapPost("/api/admin/ai-visibility/policy", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var valid = AiVisibility.Crawlers.Select(c => c.Token).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var tokens = new List<string>();
            if (H.GetEl(b, "blocked") is { ValueKind: System.Text.Json.JsonValueKind.Array } arr)
                foreach (var e in arr.EnumerateArray())
                    if (e.GetString() is { } s && valid.Contains(s) && !tokens.Contains(s, StringComparer.OrdinalIgnoreCase))
                        // store the catalogue's canonical casing, not the client's
                        tokens.Add(AiVisibility.Crawlers.First(c => c.Token.Equals(s, StringComparison.OrdinalIgnoreCase)).Token);
            var csv = string.Join(',', tokens);
            db.Execute("DELETE FROM site_settings WHERE skey='ai_bot_blocklist'");
            db.Execute("INSERT INTO site_settings(skey,svalue) VALUES('ai_bot_blocklist',?)", csv);
            AiVisibility.Bump();
            log(null, "ai_visibility.policy", tokens.Count == 0 ? "all allowed" : "blocked: " + csv);
            return J(new { ok = true, blocked = tokens });
        });
    }
}
