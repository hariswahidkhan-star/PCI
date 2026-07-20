using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Read-only analytics connectors (Phase 6) — pull search/traffic metrics from providers whose official APIs
/// are read-only: Google Search Console (searchAnalytics.query), Bing Webmaster Tools (GetQueryStats), and
/// Google Analytics 4 (runReport). This surface ONLY reads — nothing is ever written back to any provider.
/// Credentials (a bearer OAuth token for Google, an API key for Bing) are decrypted only at call time and
/// never logged; every request goes through the SSRF-guarded egress client. Google access tokens are
/// short-lived — an operator mints one via a service account or OAuth; automatic token refresh is a
/// documented follow-up, so a source whose token has expired simply reports an error on its next sync.
/// </summary>
public static class AnalyticsConnectors
{
    public static readonly string[] Providers = { "gsc", "bing", "ga4" };
    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(30));

    public record Metric(string Dimension, string? DimValue, string? Date, long? Clicks, long? Impressions,
        double? Ctr, double? Position, long? Sessions, long? Users, long? Pageviews);

    static string ApiBase(Dictionary<string, object?> s, string dflt)
    {
        var b = (H.Str(s["api_base"]) ?? "").TrimEnd('/');
        return b.Length > 0 ? b : dflt;
    }

    /// <summary>Fetch the latest snapshot for a source and replace its stored metrics. Returns (ok, rows, err).
    /// Never throws; records status / last_error / last_synced_at on the source.</summary>
    public static async Task<(bool ok, int rows, string? err)> Sync(Db db, long sourceId)
    {
        var s = db.QueryOne("SELECT * FROM cc_analytics_sources WHERE id=?", sourceId);
        if (s is null) return (false, 0, "not_found");
        var secret = Security.DecryptSecret(H.Str(s["secret_enc"])) ?? "";
        if (secret.Length == 0) { Fail(db, sourceId, "no_credential", "not_connected"); return (false, 0, "no_credential"); }
        var provider = (H.Str(s["provider"]) ?? "").ToLowerInvariant();
        var property = H.Str(s["property"]) ?? "";
        int days = (int)H.L(s["range_days"]); if (days <= 0) days = 28;
        var end = DateTime.UtcNow.Date; var start = end.AddDays(-days);
        string sd = start.ToString("yyyy-MM-dd"), ed = end.ToString("yyyy-MM-dd");

        try
        {
            var rows = provider switch
            {
                "gsc" => await Gsc(ApiBase(s, "https://www.googleapis.com/webmasters/v3"), property, secret, sd, ed),
                "bing" => await Bing(ApiBase(s, "https://ssl.bing.com/webmaster/api.svc/json"), property, secret),
                "ga4" => await Ga4(ApiBase(s, "https://analyticsdata.googleapis.com/v1beta"), property, secret, sd, ed),
                _ => throw new Exception("unsupported_provider"),
            };
            // Synthesize a single 'overview' row so dashboards total without double-counting across dimensions.
            var baseRows = rows.Where(m => m.Dimension == "date").ToList();
            if (baseRows.Count == 0) baseRows = rows.Where(m => m.Dimension == "query").ToList();
            if (baseRows.Count > 0)
                rows.Add(new Metric("overview", null, null, Sum(baseRows, m => m.Clicks), Sum(baseRows, m => m.Impressions),
                    Avg(baseRows, m => m.Ctr), Avg(baseRows, m => m.Position), Sum(baseRows, m => m.Sessions),
                    Sum(baseRows, m => m.Users), Sum(baseRows, m => m.Pageviews)));

            // Replace snapshot: delete old rows, insert the fresh pull.
            db.Execute("DELETE FROM cc_analytics_metrics WHERE source_id=?", sourceId);
            foreach (var m in rows)
                db.Execute(@"INSERT INTO cc_analytics_metrics(source_id,dimension,dim_value,metric_date,clicks,impressions,ctr,position,sessions,users,pageviews,fetched_at)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?, datetime('now'))",
                    sourceId, m.Dimension, Cap(m.DimValue, 400), m.Date, m.Clicks, m.Impressions, m.Ctr, m.Position, m.Sessions, m.Users, m.Pageviews);
            db.Execute("UPDATE cc_analytics_sources SET status='connected', last_synced_at=datetime('now'), last_error=NULL, updated_at=datetime('now') WHERE id=?", sourceId);
            return (true, rows.Count, null);
        }
        catch (Exception e) { Fail(db, sourceId, Trim(e.Message), "error"); return (false, 0, e.Message); }
    }

    static void Fail(Db db, long id, string err, string status) =>
        db.Execute("UPDATE cc_analytics_sources SET status=?, last_error=?, updated_at=datetime('now') WHERE id=?", status, err, id);

    // ---- Google Search Console: searchAnalytics.query across query, page and date dimensions ----
    static async Task<List<Metric>> Gsc(string apiBase, string siteUrl, string token, string sd, string ed)
    {
        var outp = new List<Metric>();
        foreach (var dim in new[] { "query", "page", "date" })
        {
            var body = JsonSerializer.Serialize(new { startDate = sd, endDate = ed, dimensions = new[] { dim }, rowLimit = 25 });
            using var req = new HttpRequestMessage(HttpMethod.Post, $"{apiBase}/sites/{Uri.EscapeDataString(siteUrl)}/searchAnalytics/query")
            { Content = new StringContent(body, Encoding.UTF8, "application/json") };
            req.Headers.TryAddWithoutValidation("Authorization", "Bearer " + token);
            using var r = await Http.SendAsync(req);
            var text = await r.Content.ReadAsStringAsync();
            if (!r.IsSuccessStatusCode) throw new Exception($"gsc {(int)r.StatusCode}: {Trim(text)}");
            using var doc = JsonDocument.Parse(text);
            if (doc.RootElement.TryGetProperty("rows", out var rowsEl) && rowsEl.ValueKind == JsonValueKind.Array)
                foreach (var row in rowsEl.EnumerateArray())
                {
                    var key = row.TryGetProperty("keys", out var k) && k.ValueKind == JsonValueKind.Array && k.GetArrayLength() > 0 ? k[0].GetString() : null;
                    outp.Add(new Metric(dim, dim == "date" ? null : key, dim == "date" ? key : null,
                        Num(row, "clicks"), Num(row, "impressions"), Dbl(row, "ctr"), Dbl(row, "position"), null, null, null));
                }
        }
        return outp;
    }

    // ---- Bing Webmaster Tools: GetQueryStats (api key) ----
    static async Task<List<Metric>> Bing(string apiBase, string siteUrl, string apiKey)
    {
        var url = $"{apiBase}/GetQueryStats?apikey={Uri.EscapeDataString(apiKey)}&siteUrl={Uri.EscapeDataString(siteUrl)}";
        using var r = await Http.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
        var text = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) throw new Exception($"bing {(int)r.StatusCode}: {Trim(text)}");
        var outp = new List<Metric>();
        using var doc = JsonDocument.Parse(text);
        if (doc.RootElement.TryGetProperty("d", out var d) && d.ValueKind == JsonValueKind.Array)
            foreach (var row in d.EnumerateArray())
                outp.Add(new Metric("query", row.TryGetProperty("Query", out var q) ? q.GetString() : null, null,
                    Num(row, "Clicks"), Num(row, "Impressions"), null, Dbl(row, "AvgPosition") ?? Dbl(row, "Position"), null, null, null));
        return outp;
    }

    // ---- Google Analytics 4: runReport by date (sessions / users / pageviews) ----
    static async Task<List<Metric>> Ga4(string apiBase, string property, string token, string sd, string ed)
    {
        var body = JsonSerializer.Serialize(new
        {
            dateRanges = new[] { new { startDate = sd, endDate = ed } },
            dimensions = new[] { new { name = "date" } },
            metrics = new[] { new { name = "sessions" }, new { name = "totalUsers" }, new { name = "screenPageViews" } },
        });
        using var req = new HttpRequestMessage(HttpMethod.Post, $"{apiBase}/properties/{Uri.EscapeDataString(property)}:runReport")
        { Content = new StringContent(body, Encoding.UTF8, "application/json") };
        req.Headers.TryAddWithoutValidation("Authorization", "Bearer " + token);
        using var r = await Http.SendAsync(req);
        var text = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) throw new Exception($"ga4 {(int)r.StatusCode}: {Trim(text)}");
        var outp = new List<Metric>();
        using var doc = JsonDocument.Parse(text);
        if (doc.RootElement.TryGetProperty("rows", out var rowsEl) && rowsEl.ValueKind == JsonValueKind.Array)
            foreach (var row in rowsEl.EnumerateArray())
            {
                string? date = row.TryGetProperty("dimensionValues", out var dv) && dv.ValueKind == JsonValueKind.Array && dv.GetArrayLength() > 0
                    ? dv[0].GetProperty("value").GetString() : null;
                var mv = row.TryGetProperty("metricValues", out var mvEl) && mvEl.ValueKind == JsonValueKind.Array
                    ? mvEl.EnumerateArray().Select(x => long.TryParse(x.GetProperty("value").GetString(), out var n) ? n : 0L).ToArray()
                    : Array.Empty<long>();
                outp.Add(new Metric("date", null, date, null, null, null, null,
                    mv.Length > 0 ? mv[0] : null, mv.Length > 1 ? mv[1] : null, mv.Length > 2 ? mv[2] : null));
            }
        return outp;
    }

    static long? Sum(List<Metric> r, Func<Metric, long?> f) { long t = 0; bool any = false; foreach (var m in r) if (f(m) is long v) { t += v; any = true; } return any ? t : (long?)null; }
    static double? Avg(List<Metric> r, Func<Metric, double?> f) { double t = 0; int n = 0; foreach (var m in r) if (f(m) is double v) { t += v; n++; } return n > 0 ? t / n : (double?)null; }

    static long? Num(JsonElement o, string k)
    {
        if (!o.TryGetProperty(k, out var v)) return null;
        if (v.ValueKind == JsonValueKind.Number) return (long)v.GetDouble();
        return v.ValueKind == JsonValueKind.String && long.TryParse(v.GetString(), out var n) ? n : (long?)null;
    }
    static double? Dbl(JsonElement o, string k)
    {
        if (!o.TryGetProperty(k, out var v)) return null;
        if (v.ValueKind == JsonValueKind.Number) return v.GetDouble();
        return v.ValueKind == JsonValueKind.String && double.TryParse(v.GetString(), out var d) ? d : (double?)null;
    }
    static string? Cap(string? s, int n) => s is null ? null : (s.Length <= n ? s : s[..n]);
    static string Trim(string s) => s.Length > 300 ? s[..300] : s;
}
