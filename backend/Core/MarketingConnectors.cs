using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Official provider API connectors (Phase 2). Each call is a real HTTPS request to the platform's
/// documented endpoint, authenticated with the connection's decrypted access token. They are strictly
/// token-gated: with no connected account / token they return a clear, honest failure — never a fake
/// success. Only organisation-level, approved actions are performed here; nothing touches personal DMs.
/// </summary>
public static class MarketingConnectors
{
    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(30));

    public record Result(bool Ok, int Status, string? ProviderId, string Response);
    static Result Fail(string why) => new(false, 0, null, why);

    static Dictionary<string, object?>? LiveConnection(Db db, string platformCode)
        => db.QueryOne("SELECT * FROM mkt_connections WHERE platform_code=? AND status='connected' ORDER BY id DESC LIMIT 1", platformCode);

    /// <summary>Publish an organisation post via the LinkedIn Posts API. Requires a connected LinkedIn
    /// Company Page with a granted organisation-social scope and a stored organisation id.</summary>
    public static Result LinkedInPublishPost(Db db, Dictionary<string, object?> post)
    {
        var conn = LiveConnection(db, "linkedin_page");
        if (conn is null) return Fail("no_connected_linkedin_page");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        var orgId = H.Str(conn["external_org_id"]);
        if (string.IsNullOrWhiteSpace(orgId)) return Fail("no_organisation_id");

        var author = $"urn:li:organization:{orgId}";
        var commentary = H.Str(post["body"]) ?? "";
        var articleUrl = H.Str(post["article_url"]);
        object payload = string.IsNullOrWhiteSpace(articleUrl)
            ? new { author, commentary, visibility = "PUBLIC", distribution = new { feedDistribution = "MAIN_FEED" }, lifecycleState = "PUBLISHED" }
            : new { author, commentary, visibility = "PUBLIC", distribution = new { feedDistribution = "MAIN_FEED" }, lifecycleState = "PUBLISHED",
                content = new { article = new { source = articleUrl, title = H.Str(post["article_title"]) ?? "" } } };
        try
        {
            using var msg = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/rest/posts");
            msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            msg.Headers.TryAddWithoutValidation("LinkedIn-Version", "202401");
            msg.Headers.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
            msg.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            using var resp = Http.Send(msg);
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            // LinkedIn returns the new post id in the x-restli-id / x-linkedin-id header.
            var postId = resp.Headers.TryGetValues("x-restli-id", out var v) ? v.FirstOrDefault()
                       : resp.Headers.TryGetValues("x-linkedin-id", out var v2) ? v2.FirstOrDefault() : null;
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, postId, body.Length > 4000 ? body[..4000] : body);
        }
        catch (Exception ex) { return Fail("linkedin_call_failed: " + ex.Message); }
    }

    /// <summary>Submit a sitemap to Google Search Console (webmasters v3 sitemaps.submit — HTTP PUT).</summary>
    public static Result GscSubmitSitemap(Db db, string property, string sitemapUrl)
    {
        var conn = LiveConnection(db, "google_search_console");
        if (conn is null) return Fail("no_connected_property");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        try
        {
            var url = $"https://www.googleapis.com/webmasters/v3/sites/{Uri.EscapeDataString(property)}/sitemaps/{Uri.EscapeDataString(sitemapUrl)}";
            using var msg = new HttpRequestMessage(HttpMethod.Put, url);
            msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            using var resp = Http.Send(msg);
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, sitemapUrl, body);
        }
        catch (Exception ex) { return Fail("gsc_call_failed: " + ex.Message); }
    }

    /// <summary>Inspect a URL's index status via the Search Console URL Inspection API. Reports the status
    /// the API exposes — this is NOT a guaranteed request for immediate indexing.</summary>
    public static Result GscInspectUrl(Db db, string property, string inspectionUrl)
    {
        var conn = LiveConnection(db, "google_search_console");
        if (conn is null) return Fail("no_connected_property");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        try
        {
            var payload = new { inspectionUrl, siteUrl = property };
            using var msg = new HttpRequestMessage(HttpMethod.Post, "https://searchconsole.googleapis.com/v1/urlInspection/index:inspect");
            msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            msg.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            using var resp = Http.Send(msg);
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, inspectionUrl, body.Length > 8000 ? body[..8000] : body);
        }
        catch (Exception ex) { return Fail("gsc_inspect_failed: " + ex.Message); }
    }
}
