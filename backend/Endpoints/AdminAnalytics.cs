using System.Text;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin Console → Analytics (master-plan Phase 5): reporting over the first-party analytics_events
/// ledger — visitors, page views, sources/campaigns, devices, countries, conversions and revenue
/// attribution, plus CSV export. Gated by the existing 'reports' permission. Read-only.
/// </summary>
public static class AdminAnalytics
{
    public static void Map(WebApplication app, Db db,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string Since(HttpRequest req)
        {
            var days = int.TryParse(req.Query["days"], out var d) ? Math.Clamp(d, 1, 365) : 30;
            return DateTime.UtcNow.AddDays(-days).ToString("yyyy-MM-dd HH:mm:ss");
        }

        app.MapGet("/api/admin/analytics/summary", (HttpRequest req) => gate(req, "reports", _ =>
        {
            var since = Since(req);
            List<Dictionary<string, object?>> Q(string sql) => db.Query(sql, since);
            long C(string sql) => db.Scalar<long>(sql, since);
            var pageViews = C("SELECT COUNT(*) FROM analytics_events WHERE event='page_view' AND created_at>=?");
            var visitors = C("SELECT COUNT(DISTINCT visitor) FROM analytics_events WHERE event='page_view' AND created_at>=?");
            var registrations = C("SELECT COUNT(*) FROM analytics_events WHERE event='registration_completed' AND created_at>=?");
            var logins = C("SELECT COUNT(*) FROM analytics_events WHERE event='login' AND created_at>=?");
            var purchases = C("SELECT COUNT(*) FROM analytics_events WHERE event='purchase_completed' AND created_at>=?");
            var revenue = db.Scalar<double>("SELECT COALESCE(SUM(value),0) FROM analytics_events WHERE event='purchase_completed' AND created_at>=?", since);
            var memberships = C("SELECT COUNT(*) FROM analytics_events WHERE event='membership_activated' AND created_at>=?");
            return J(new
            {
                page_views = pageViews, visitors, registrations, logins, purchases,
                revenue = Math.Round(revenue, 2), memberships_activated = memberships,
                top_pages = Q("SELECT path, COUNT(*) n FROM analytics_events WHERE event='page_view' AND created_at>=? GROUP BY path ORDER BY n DESC LIMIT 15"),
                sources = Q("SELECT COALESCE(utm_source,'(direct)') source, COUNT(*) n FROM analytics_events WHERE event='page_view' AND created_at>=? GROUP BY utm_source ORDER BY n DESC LIMIT 12"),
                campaigns = Q("SELECT utm_campaign campaign, COUNT(*) n FROM analytics_events WHERE event='page_view' AND utm_campaign IS NOT NULL AND created_at>=? GROUP BY utm_campaign ORDER BY n DESC LIMIT 12"),
                devices = Q("SELECT COALESCE(device,'unknown') device, COUNT(*) n FROM analytics_events WHERE event='page_view' AND created_at>=? GROUP BY device ORDER BY n DESC"),
                browsers = Q("SELECT COALESCE(browser,'other') browser, COUNT(*) n FROM analytics_events WHERE event='page_view' AND created_at>=? GROUP BY browser ORDER BY n DESC LIMIT 8"),
                // NB: Db.Bind rewrites every '?' character (even inside string literals) to a
                // positional parameter, so the unknown-country label must not contain one.
                countries = Q("SELECT COALESCE(country,'(unknown)') country, COUNT(*) n FROM analytics_events WHERE event='page_view' AND created_at>=? GROUP BY country ORDER BY n DESC LIMIT 12"),
                daily = Q("SELECT substr(created_at,1,10) day, COUNT(*) n FROM analytics_events WHERE event='page_view' AND created_at>=? GROUP BY substr(created_at,1,10) ORDER BY day"),
                conversions_by_source = Q(@"SELECT COALESCE(utm_source,'(direct)') source,
                        SUM(CASE WHEN event='registration_completed' THEN 1 ELSE 0 END) registrations,
                        SUM(CASE WHEN event='purchase_completed' THEN 1 ELSE 0 END) purchases,
                        COALESCE(SUM(CASE WHEN event='purchase_completed' THEN value ELSE 0 END),0) revenue
                        FROM analytics_events WHERE event IN ('registration_completed','purchase_completed') AND created_at>=?
                        GROUP BY utm_source ORDER BY revenue DESC, purchases DESC LIMIT 12"),
            });
        }));

        app.MapGet("/api/admin/analytics/export.csv", (HttpRequest req) => gate(req, "reports", _ =>
        {
            var since = Since(req);
            var sb = new StringBuilder("created_at,event,path,visitor,country,device,browser,utm_source,utm_medium,utm_campaign,referrer,landing,value,currency\n");
            foreach (var r in db.Query("SELECT * FROM analytics_events WHERE created_at>=? ORDER BY id DESC LIMIT 50000", since))
            {
                sb.Append(string.Join(',', new[] { "created_at", "event", "path", "visitor", "country", "device", "browser", "utm_source", "utm_medium", "utm_campaign", "referrer", "landing", "value", "currency" }
                    .Select(k => Csv.Cell(r[k])))).Append('\n');
            }
            return Results.Text(sb.ToString(), "text/csv", Encoding.UTF8);
        }));
    }
}
