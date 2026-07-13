using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// IndexNow (Bing, Yandex, Seznam, Naver…): instant URL-change notification. The protocol needs no
/// account — a self-generated key served at /{key}.txt proves site ownership, and changed URLs are
/// POSTed to api.indexnow.org. The key is generated once at boot (site_settings 'indexnow_key');
/// every submission is recorded in seo_submissions (status: submitted | failed) with the engine's
/// HTTP response, so the admin log reflects reality. Never throws — a submission failure is logged
/// and reported, not fatal.
/// </summary>
public static class IndexNowService
{
    static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(20) };

    public static string Key(Db db) =>
        db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='indexnow_key'")?.Trim() ?? "";

    /// <summary>Generate the site key once (hex, 32 chars) — idempotent, called from Migrate.</summary>
    public static void EnsureKey(Db db)
    {
        if (Key(db).Length > 0) return;
        var key = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();
        db.Execute("DELETE FROM site_settings WHERE skey='indexnow_key'");
        db.Execute("INSERT INTO site_settings(skey,svalue) VALUES('indexnow_key',?)", key);
    }

    /// <summary>Submit up to 10,000 URLs in one call per the protocol. Returns (ok, detail).</summary>
    public static (bool ok, string detail) Submit(Db db, IReadOnlyList<string> urls)
    {
        var key = Key(db);
        if (key.Length == 0) return (false, "no IndexNow key configured");
        if (urls.Count == 0) return (false, "no URLs to submit");
        var host = Redirects.CanonicalHost;
        var body = JsonSerializer.Serialize(new
        {
            host,
            key,
            keyLocation = $"{Redirects.CanonicalBase}/{key}.txt",
            urlList = urls.Take(10000),
        });
        string detail; bool ok;
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.indexnow.org/indexnow")
            { Content = new StringContent(body, Encoding.UTF8, "application/json") };
            using var resp = _http.Send(req);
            ok = resp.IsSuccessStatusCode;
            detail = $"HTTP {(int)resp.StatusCode}";
            if (!ok)
            {
                using var sr = new StreamReader(resp.Content.ReadAsStream());
                var t = sr.ReadToEnd(); if (t.Length > 0) detail += ": " + (t.Length > 200 ? t[..200] : t);
            }
        }
        catch (Exception e) { ok = false; detail = e.Message; }
        try
        {
            db.Execute("INSERT INTO seo_submissions(engine,url_count,status,detail) VALUES('indexnow',?,?,?)",
                urls.Count, ok ? "submitted" : "failed", detail);
        }
        catch { /* the log row must never break the caller */ }
        return (ok, detail);
    }

    /// <summary>All indexable public URLs (same inclusion rules as the sitemap) on the canonical host.</summary>
    public static List<string> AllPublicUrls(Db db)
    {
        var shells = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "student.html", "admin.html", "exam-ui.html", "404.html" };
        var list = new List<string>();
        foreach (var r in db.Query("SELECT slug FROM pages WHERE published=1 AND (noindex IS NULL OR noindex=0) ORDER BY slug"))
        {
            var slug = H.Str(r["slug"]) ?? "";
            if (slug.Length == 0 || shells.Contains(slug)) continue;
            list.Add(slug.Equals("index.html", StringComparison.OrdinalIgnoreCase)
                ? Redirects.CanonicalBase + "/" : Redirects.CanonicalBase + "/" + slug);
        }
        return list;
    }
}
