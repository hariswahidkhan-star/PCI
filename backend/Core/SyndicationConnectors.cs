using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Outbound content-syndication connectors for the partner CMS platforms whose official APIs need no
/// provider review — a PCI-controlled credential is enough: WordPress self-hosted (REST + application
/// password, HTTP Basic), Ghost (Admin API, JWT signed from the admin key) and Forem/DEV (api-key header).
/// Every syndicated copy sets its canonical back to the original PCI article so search engines consolidate
/// duplicate content to the source; Ghost and Forem carry a native canonical_url field, WordPress core has
/// none over REST so a visible "originally published at" attribution link is appended instead. All calls go
/// through the SSRF-guarded egress client (base URLs are operator-supplied); secrets are decrypted only at
/// call time and never logged. Approval-gated destinations (WordPress.com, Medium, Substack, Hashnode) stay
/// framework-only in the capability registry until the operator completes provider onboarding.
/// </summary>
public static class SyndicationConnectors
{
    public static readonly string[] Live = { "wordpress_selfhosted", "ghost", "forem_dev" };
    public static bool IsLive(string p) => Array.IndexOf(Live, (p ?? "").ToLowerInvariant()) >= 0;

    public record Post(string Title, string Html, string CanonicalUrl, List<string> Tags, string Status);
    public record Result(bool Ok, string? ExternalId, string? ExternalUrl, int Code, string? Error);

    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(30));

    /// <summary>Publish (create, or update when externalId is set) a post to a destination. Never throws.</summary>
    public static async Task<Result> Publish(Dictionary<string, object?> dest, Post post, string? externalId)
    {
        try
        {
            var platform = (H.Str(dest["platform_key"]) ?? "").ToLowerInvariant();
            var baseUrl = (H.Str(dest["base_url"]) ?? "").TrimEnd('/');
            if (baseUrl.Length == 0) return new Result(false, null, null, 0, "base_url_required");
            var cfg = ParseCfg(H.Str(dest["config"]));
            var secret = Security.DecryptSecret(H.Str(dest["secret_enc"])) ?? "";
            return platform switch
            {
                "wordpress_selfhosted" => await WordPress(baseUrl, cfg, secret, post, externalId),
                "ghost" => await Ghost(baseUrl, secret, post, externalId),
                "forem_dev" => await Forem(baseUrl, secret, post, externalId),
                _ => new Result(false, null, null, 0, "unsupported_platform"),
            };
        }
        catch (Exception e) { return new Result(false, null, null, 0, e.Message); }
    }

    /// <summary>Lightweight connectivity + auth check for a destination.</summary>
    public static async Task<Result> Test(Dictionary<string, object?> dest)
    {
        try
        {
            var platform = (H.Str(dest["platform_key"]) ?? "").ToLowerInvariant();
            var baseUrl = (H.Str(dest["base_url"]) ?? "").TrimEnd('/');
            if (baseUrl.Length == 0) return new Result(false, null, null, 0, "base_url_required");
            var cfg = ParseCfg(H.Str(dest["config"]));
            var secret = Security.DecryptSecret(H.Str(dest["secret_enc"])) ?? "";
            switch (platform)
            {
                case "wordpress_selfhosted":
                    {
                        using var req = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/wp-json/wp/v2/users/me?context=edit");
                        req.Headers.TryAddWithoutValidation("Authorization", "Basic " + WpBasic(cfg, secret));
                        var r = await Http.SendAsync(req);
                        return new Result(r.IsSuccessStatusCode, null, null, (int)r.StatusCode, r.IsSuccessStatusCode ? null : Trim(await r.Content.ReadAsStringAsync()));
                    }
                case "ghost":
                    {
                        using var req = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/ghost/api/admin/site/");
                        req.Headers.TryAddWithoutValidation("Authorization", "Ghost " + GhostJwt(secret));
                        var r = await Http.SendAsync(req);
                        return new Result(r.IsSuccessStatusCode, null, null, (int)r.StatusCode, r.IsSuccessStatusCode ? null : Trim(await r.Content.ReadAsStringAsync()));
                    }
                case "forem_dev":
                    {
                        using var req = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/api/users/me");
                        req.Headers.TryAddWithoutValidation("api-key", secret);
                        req.Headers.TryAddWithoutValidation("Accept", "application/vnd.forem.api-v1+json");
                        var r = await Http.SendAsync(req);
                        return new Result(r.IsSuccessStatusCode, null, null, (int)r.StatusCode, r.IsSuccessStatusCode ? null : Trim(await r.Content.ReadAsStringAsync()));
                    }
                default: return new Result(false, null, null, 0, "unsupported_platform");
            }
        }
        catch (Exception e) { return new Result(false, null, null, 0, e.Message); }
    }

    // ── WordPress self-hosted (REST v2 + application password over HTTP Basic) ─────────────────────────
    static async Task<Result> WordPress(string baseUrl, Dictionary<string, string> cfg, string appPassword, Post post, string? externalId)
    {
        // WordPress core exposes no cross-domain canonical over REST, so append a visible attribution link.
        var html = post.Html + $"\n<p><em>Originally published at <a href=\"{post.CanonicalUrl}\">projectcontrolsinstitute.org</a>.</em></p>";
        var payload = JsonSerializer.Serialize(new { title = post.Title, content = html, status = MapWpStatus(post.Status), tags = Array.Empty<int>() });
        var url = baseUrl + "/wp-json/wp/v2/posts" + (externalId is { Length: > 0 } ? "/" + externalId : "");
        using var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(payload, Encoding.UTF8, "application/json") };
        req.Headers.TryAddWithoutValidation("Authorization", "Basic " + WpBasic(cfg, appPassword));
        var r = await Http.SendAsync(req);
        var txt = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) return new Result(false, null, null, (int)r.StatusCode, Trim(txt));
        try { using var d = JsonDocument.Parse(txt); var root = d.RootElement; return new Result(true, root.GetProperty("id").GetRawText(), root.TryGetProperty("link", out var l) ? l.GetString() : null, (int)r.StatusCode, null); }
        catch { return new Result(true, null, null, (int)r.StatusCode, null); }
    }
    static string MapWpStatus(string s) => s == "published" ? "publish" : "draft";
    static string WpBasic(Dictionary<string, string> cfg, string appPassword) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(cfg.GetValueOrDefault("username", "") + ":" + appPassword));

    // ── Ghost (Admin API; JWT signed from the "id:secret" admin key, canonical_url native) ─────────────
    static async Task<Result> Ghost(string baseUrl, string adminKey, Post post, string? externalId)
    {
        var jwt = GhostJwt(adminKey);
        if (externalId is { Length: > 0 })
        {
            // Ghost update needs the current updated_at (collision protection): read it, then PUT.
            using var g = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/ghost/api/admin/posts/" + externalId + "/?fields=id,updated_at,url");
            g.Headers.TryAddWithoutValidation("Authorization", "Ghost " + jwt);
            var gr = await Http.SendAsync(g);
            var gt = await gr.Content.ReadAsStringAsync();
            if (!gr.IsSuccessStatusCode) return new Result(false, null, null, (int)gr.StatusCode, Trim(gt));
            string updatedAt = "";
            try { using var d = JsonDocument.Parse(gt); updatedAt = d.RootElement.GetProperty("posts")[0].GetProperty("updated_at").GetString() ?? ""; } catch { }
            var upBody = JsonSerializer.Serialize(new { posts = new[] { new { title = post.Title, html = post.Html, status = post.Status, canonical_url = post.CanonicalUrl, updated_at = updatedAt } } });
            using var pr = new HttpRequestMessage(HttpMethod.Put, baseUrl + "/ghost/api/admin/posts/" + externalId + "/?source=html") { Content = new StringContent(upBody, Encoding.UTF8, "application/json") };
            pr.Headers.TryAddWithoutValidation("Authorization", "Ghost " + jwt);
            var prr = await Http.SendAsync(pr);
            var prt = await prr.Content.ReadAsStringAsync();
            if (!prr.IsSuccessStatusCode) return new Result(false, null, null, (int)prr.StatusCode, Trim(prt));
            return GhostParse(prt, (int)prr.StatusCode);
        }
        var body = JsonSerializer.Serialize(new { posts = new[] { new { title = post.Title, html = post.Html, status = post.Status, canonical_url = post.CanonicalUrl } } });
        using var req = new HttpRequestMessage(HttpMethod.Post, baseUrl + "/ghost/api/admin/posts/?source=html") { Content = new StringContent(body, Encoding.UTF8, "application/json") };
        req.Headers.TryAddWithoutValidation("Authorization", "Ghost " + jwt);
        var r = await Http.SendAsync(req);
        var txt = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) return new Result(false, null, null, (int)r.StatusCode, Trim(txt));
        return GhostParse(txt, (int)r.StatusCode);
    }
    static Result GhostParse(string txt, int code)
    {
        try { using var d = JsonDocument.Parse(txt); var p = d.RootElement.GetProperty("posts")[0]; return new Result(true, p.GetProperty("id").GetString(), p.TryGetProperty("url", out var u) ? u.GetString() : null, code, null); }
        catch { return new Result(true, null, null, code, null); }
    }
    /// <summary>Build a short-lived Ghost Admin API JWT (HS256) from an "id:secretHex" admin key.</summary>
    static string GhostJwt(string adminKey)
    {
        var parts = adminKey.Split(':');
        if (parts.Length != 2) throw new InvalidOperationException("ghost admin key must be id:secret");
        var kid = parts[0];
        var secret = Convert.FromHexString(parts[1]);
        long iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string header = B64Url(JsonSerializer.SerializeToUtf8Bytes(new { alg = "HS256", typ = "JWT", kid }));
        string payload = B64Url(JsonSerializer.SerializeToUtf8Bytes(new { iat, exp = iat + 300, aud = "/admin/" }));
        var signingInput = header + "." + payload;
        var sig = new HMACSHA256(secret).ComputeHash(Encoding.ASCII.GetBytes(signingInput));
        return signingInput + "." + B64Url(sig);
    }
    static string B64Url(byte[] b) => Convert.ToBase64String(b).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    // ── Forem / DEV (POST /api/articles, api-key header, canonical_url + body_markdown native) ─────────
    static async Task<Result> Forem(string baseUrl, string apiKey, Post post, string? externalId)
    {
        var article = new Dictionary<string, object?>
        {
            ["title"] = post.Title,
            ["body_markdown"] = HtmlToText(post.Html),
            ["published"] = post.Status == "published",
            ["canonical_url"] = post.CanonicalUrl,
            ["tags"] = post.Tags.Take(4).Select(t => t.Replace(" ", "").ToLowerInvariant()).ToArray(),
        };
        var payload = JsonSerializer.Serialize(new { article });
        var method = externalId is { Length: > 0 } ? HttpMethod.Put : HttpMethod.Post;
        var url = baseUrl + "/api/articles" + (externalId is { Length: > 0 } ? "/" + externalId : "");
        using var req = new HttpRequestMessage(method, url) { Content = new StringContent(payload, Encoding.UTF8, "application/json") };
        req.Headers.TryAddWithoutValidation("api-key", apiKey);
        req.Headers.TryAddWithoutValidation("Accept", "application/vnd.forem.api-v1+json");
        var r = await Http.SendAsync(req);
        var txt = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) return new Result(false, null, null, (int)r.StatusCode, Trim(txt));
        try { using var d = JsonDocument.Parse(txt); var root = d.RootElement; return new Result(true, root.GetProperty("id").GetRawText(), root.TryGetProperty("url", out var u) ? u.GetString() : null, (int)r.StatusCode, null); }
        catch { return new Result(true, null, null, (int)r.StatusCode, null); }
    }

    // ── helpers ────────────────────────────────────────────────────────────────────────────────────────
    static Dictionary<string, string> ParseCfg(string? json)
    {
        var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(json)) return d;
        try { using var doc = JsonDocument.Parse(json); foreach (var p in doc.RootElement.EnumerateObject()) d[p.Name] = p.Value.ValueKind == JsonValueKind.String ? p.Value.GetString() ?? "" : p.Value.ToString(); }
        catch { }
        return d;
    }
    static readonly System.Text.RegularExpressions.Regex RxTag = new("<[^>]+>", System.Text.RegularExpressions.RegexOptions.Compiled);
    static string HtmlToText(string html) => System.Net.WebUtility.HtmlDecode(RxTag.Replace(html.Replace("</p>", "\n\n").Replace("<br>", "\n").Replace("<br/>", "\n"), "")).Trim();
    static string Trim(string s) => s.Length > 300 ? s[..300] : s;
}
