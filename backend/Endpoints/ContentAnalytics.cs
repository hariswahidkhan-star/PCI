using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Read-only analytics connectors (Phase 6) — register Google Search Console, Bing Webmaster Tools and
/// Google Analytics 4 sources, sync their metrics on demand, and read them back on a dashboard. The whole
/// surface is gated by the cc_seo permission and every mutation is audited. Credentials are stored encrypted
/// and NEVER returned to the browser (only a has_secret boolean). This surface only reads from providers;
/// nothing is written back.
/// </summary>
public static class ContentAnalytics
{
    const string PERM = "cc_seo";

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate, Func<HttpRequest, AdminCtx?> adminFromReq)
    {
        IResult J(object o) => Results.Json(o);
        bool IsOk(IResult r) => r is Microsoft.AspNetCore.Http.HttpResults.Ok;
        string? Cap(string? s, int n) => s is null ? null : (s.Length <= n ? s : s[..n]);

        // Public projection — secret is exposed only as a boolean, never the value.
        object Pub(Dictionary<string, object?> s) => new
        {
            id = s["id"], provider = s["provider"], label = s["label"], property = s["property"], api_base = s["api_base"],
            auth_kind = s["auth_kind"], range_days = s["range_days"], status = s["status"], active = s["active"],
            last_synced_at = s["last_synced_at"], last_error = s["last_error"], has_secret = !string.IsNullOrEmpty(H.Str(s["secret_enc"])),
        };

        app.MapGet("/api/admin/content/analytics/sources", (HttpRequest req) => gate(req, PERM, _ =>
            J(new { rows = db.Query("SELECT * FROM cc_analytics_sources WHERE active=1 ORDER BY id DESC").Select(Pub) })));

        app.MapPost("/api/admin/content/analytics/sources", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, PERM, adm =>
            {
                var provider = (H.GetS(b, "provider") ?? "").ToLowerInvariant();
                if (Array.IndexOf(AnalyticsConnectors.Providers, provider) < 0) return Results.Json(new { error = "unsupported_provider" }, statusCode: 400);
                var secret = H.GetS(b, "secret");
                var authKind = provider == "bing" ? "api_key" : "bearer";
                var days = H.GetNum(b, "range_days") is double d ? (int)d : 28;
                var id = db.ExecuteReturningId(@"INSERT INTO cc_analytics_sources(provider,label,property,api_base,auth_kind,secret_enc,range_days,status,created_by,created_at,updated_at)
                    VALUES(?,?,?,?,?,?,?,?, ?, datetime('now'), datetime('now'))",
                    provider, Cap(H.GetS(b, "label") ?? provider.ToUpperInvariant(), 160), Cap(H.GetS(b, "property"), 300), Cap(H.GetS(b, "api_base"), 300),
                    authKind, Security.EncryptSecret(secret), days, string.IsNullOrEmpty(secret) ? "not_connected" : "connected", adm.Id);
                log(adm.Id, "analytics_source_create", provider + " #" + id);
                return J(new { ok = true, id });
            });
        });

        app.MapPatch("/api/admin/content/analytics/sources/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, PERM, adm =>
            {
                foreach (var k in new[] { "label", "property", "api_base" })
                    if (H.GetS(b, k) is { } v) db.Execute($"UPDATE cc_analytics_sources SET {k}=? WHERE id=?", v, id);
                if (H.GetNum(b, "range_days") is double d) db.Execute("UPDATE cc_analytics_sources SET range_days=? WHERE id=?", (int)d, id);
                // Only overwrite the stored secret when a new one is supplied (write-only field).
                if (H.GetS(b, "secret") is { Length: > 0 } sec) db.Execute("UPDATE cc_analytics_sources SET secret_enc=?, status='connected' WHERE id=?", Security.EncryptSecret(sec), id);
                db.Execute("UPDATE cc_analytics_sources SET updated_at=datetime('now') WHERE id=?", id);
                log(adm.Id, "analytics_source_update", "#" + id);
                return J(new { ok = true });
            });
        });

        app.MapPost("/api/admin/content/analytics/sources/{id:long}/delete", (HttpRequest req, long id) => gate(req, PERM, adm =>
        {
            db.Execute("UPDATE cc_analytics_sources SET active=0, updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "analytics_source_delete", "#" + id);
            return J(new { ok = true });
        }));

        // Pull the latest snapshot from the provider now (read-only).
        app.MapPost("/api/admin/content/analytics/sources/{id:long}/sync", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, PERM, _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            var adm = adminFromReq(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            var (ok, rows, err) = await AnalyticsConnectors.Sync(db, id);
            log(adm?.Id, "analytics_sync", $"#{id}: {(ok ? rows + " rows" : "error: " + err)}");
            return ok ? J(new { ok = true, rows }) : Results.Json(new { error = "sync_failed", message = err }, statusCode: 400);
        });

        app.MapGet("/api/admin/content/analytics/metrics", (HttpRequest req) => gate(req, PERM, _ =>
        {
            var sid = req.Query["source_id"].ToString();
            var dim = req.Query["dimension"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (sid.Length > 0) { where.Add("source_id=?"); args.Add(long.Parse(sid)); }
            if (dim.Length > 0) { where.Add("dimension=?"); args.Add(dim); }
            var sql = "SELECT * FROM cc_analytics_metrics" + (where.Count > 0 ? " WHERE " + string.Join(" AND ", where) : "")
                    + " ORDER BY COALESCE(clicks,sessions,impressions,0) DESC, metric_date DESC LIMIT 200";
            return J(new { rows = db.Query(sql, args.ToArray()) });
        }));

        // Dashboard: each source + its single synthesized 'overview' totals row (no cross-dimension double counting).
        app.MapGet("/api/admin/content/analytics/overview", (HttpRequest req) => gate(req, PERM, _ =>
        {
            var sources = db.Query("SELECT * FROM cc_analytics_sources WHERE active=1 ORDER BY id DESC").Select(Pub);
            var totals = db.Query(@"SELECT s.id AS source_id, s.provider, s.label, s.status,
                    m.clicks, m.impressions, m.ctr, m.position, m.sessions, m.users, m.pageviews, s.last_synced_at
                    FROM cc_analytics_sources s LEFT JOIN cc_analytics_metrics m ON m.source_id=s.id AND m.dimension='overview'
                    WHERE s.active=1 ORDER BY s.id DESC");
            return J(new { sources, totals });
        }));
    }
}
