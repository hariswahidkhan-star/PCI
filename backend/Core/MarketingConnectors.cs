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

    /// <summary>Query Search Console Search Analytics (searchanalytics.query) for a date range + dimension.
    /// Returns the raw JSON rows; the caller stores them into mkt_gsc_query_data.</summary>
    public static Result GscSearchAnalytics(Db db, string property, string startDate, string endDate, string dimension)
    {
        var conn = LiveConnection(db, "google_search_console");
        if (conn is null) return Fail("no_connected_property");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        try
        {
            var url = $"https://www.googleapis.com/webmasters/v3/sites/{Uri.EscapeDataString(property)}/searchAnalytics/query";
            var payload = new { startDate, endDate, dimensions = new[] { dimension }, rowLimit = 250 };
            using var msg = new HttpRequestMessage(HttpMethod.Post, url);
            msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            msg.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            using var resp = Http.Send(msg);
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, dimension, body);
        }
        catch (Exception ex) { return Fail("gsc_analytics_failed: " + ex.Message); }
    }

    /// <summary>Create a paused Meta campaign via the Marketing API (act_&lt;id&gt;/campaigns). Created PAUSED
    /// so nothing spends before an operator activates it in the connected account.</summary>
    public static Result MetaCreateCampaign(Db db, string name, string objective)
    {
        var conn = LiveConnection(db, "meta_ads");
        if (conn is null) return Fail("no_connected_meta_account");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        var adAccount = H.Str(conn["external_ad_account_id"]);
        if (string.IsNullOrWhiteSpace(adAccount)) return Fail("no_ad_account_id");
        try
        {
            var acct = adAccount.StartsWith("act_") ? adAccount : "act_" + adAccount;
            var url = $"https://graph.facebook.com/v21.0/{acct}/campaigns";
            var form = new Dictionary<string, string>
            {
                ["name"] = name,
                ["objective"] = objective,
                ["status"] = "PAUSED",
                ["special_ad_categories"] = "[]",
                ["access_token"] = token,
            };
            using var msg = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(form) };
            using var resp = Http.Send(msg);
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            string? cid = null;
            try { using var d = JsonDocument.Parse(body); if (d.RootElement.TryGetProperty("id", out var idv)) cid = idv.GetString(); } catch { }
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, cid, body.Length > 4000 ? body[..4000] : body);
        }
        catch (Exception ex) { return Fail("meta_call_failed: " + ex.Message); }
    }

    /// <summary>Fetch a Meta lead's field data by leadgen id (Graph API) using the connected page token.</summary>
    public static Result MetaFetchLead(Db db, string leadgenId)
    {
        var conn = LiveConnection(db, "meta_ads");
        if (conn is null) return Fail("no_connected_meta_account");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        try
        {
            var url = $"https://graph.facebook.com/v21.0/{Uri.EscapeDataString(leadgenId)}?fields=field_data,campaign_name,form_id,created_time&access_token={Uri.EscapeDataString(token)}";
            using var resp = Http.Send(new HttpRequestMessage(HttpMethod.Get, url));
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, leadgenId, body.Length > 6000 ? body[..6000] : body);
        }
        catch (Exception ex) { return Fail("meta_lead_fetch_failed: " + ex.Message); }
    }

    // ───────────────────────── Phase 4: Google Ads, insights sync, LinkedIn leads ─────────────────────────
    static string? E(string k) => Environment.GetEnvironmentVariable(k);

    /// <summary>Create a paused Google Ads Search campaign via the Ads REST API: a campaign budget then a
    /// campaign referencing it. Requires the developer token (env) + a connected customer id + token.</summary>
    public static Result GoogleAdsCreateCampaign(Db db, string name, double dailyBudget)
    {
        var conn = LiveConnection(db, "google_ads");
        if (conn is null) return Fail("no_connected_google_ads");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        var devToken = E("GOOGLE_ADS_DEVELOPER_TOKEN");
        if (string.IsNullOrWhiteSpace(devToken)) return Fail("no_developer_token");
        var customerId = (H.Str(conn["external_ad_account_id"]) ?? "").Replace("-", "");
        if (customerId.Length == 0) return Fail("no_customer_id");
        var loginCustomer = (H.Str(conn["external_business_id"]) ?? customerId).Replace("-", "");
        try
        {
            long micros = (long)(dailyBudget <= 0 ? 10 : dailyBudget) * 1_000_000;
            // 1) Campaign budget
            var budgetReq = new { operations = new[] { new { create = new { name = name + " budget", amountMicros = micros, deliveryMethod = "STANDARD" } } } };
            var budget = GoogleAdsMutate(db, customerId, loginCustomer, token, devToken!, "campaignBudgets", budgetReq);
            if (!budget.Ok) return budget;
            var budgetRes = ExtractResourceName(budget.Response);
            if (budgetRes is null) return new Result(false, budget.Status, null, "budget_resource_missing: " + budget.Response);
            // 2) Campaign (PAUSED) referencing the budget
            var campReq = new { operations = new[] { new { create = new {
                name, status = "PAUSED", advertisingChannelType = "SEARCH", campaignBudget = budgetRes,
                networkSettings = new { targetGoogleSearch = true, targetSearchNetwork = true, targetContentNetwork = false },
                manualCpc = new { } } } } };
            var camp = GoogleAdsMutate(db, customerId, loginCustomer, token, devToken!, "campaigns", campReq);
            return new Result(camp.Ok, camp.Status, ExtractResourceName(camp.Response), camp.Response);
        }
        catch (Exception ex) { return Fail("google_ads_call_failed: " + ex.Message); }
    }

    static Result GoogleAdsMutate(Db db, string customerId, string loginCustomer, string token, string devToken, string resource, object payload)
    {
        var url = $"https://googleads.googleapis.com/v17/customers/{customerId}/{resource}:mutate";
        using var msg = new HttpRequestMessage(HttpMethod.Post, url);
        msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        msg.Headers.TryAddWithoutValidation("developer-token", devToken);
        if (!string.IsNullOrEmpty(loginCustomer)) msg.Headers.TryAddWithoutValidation("login-customer-id", loginCustomer);
        msg.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var resp = Http.Send(msg);
        var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
        return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, null, body.Length > 4000 ? body[..4000] : body);
    }

    static string? ExtractResourceName(string json)
    {
        try { using var d = JsonDocument.Parse(json);
            if (d.RootElement.TryGetProperty("results", out var rs) && rs.ValueKind == JsonValueKind.Array && rs.GetArrayLength() > 0
                && rs[0].TryGetProperty("resourceName", out var rn)) return rn.GetString();
        } catch { }
        return null;
    }

    /// <summary>Pull Meta campaign insights (spend/impressions/clicks/leads) for a provider campaign id.</summary>
    public static Result MetaInsights(Db db, string providerCampaignId)
    {
        var conn = LiveConnection(db, "meta_ads");
        if (conn is null) return Fail("no_connected_meta_account");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        try
        {
            var fields = "impressions,reach,clicks,spend,actions";
            var url = $"https://graph.facebook.com/v21.0/{Uri.EscapeDataString(providerCampaignId)}/insights?fields={fields}&date_preset=last_30d&access_token={Uri.EscapeDataString(token)}";
            using var resp = Http.Send(new HttpRequestMessage(HttpMethod.Get, url));
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, providerCampaignId, body.Length > 6000 ? body[..6000] : body);
        }
        catch (Exception ex) { return Fail("meta_insights_failed: " + ex.Message); }
    }

    /// <summary>Pull LinkedIn ad analytics for a campaign (impressions/clicks/cost) via the Advertising API.</summary>
    public static Result LinkedInAdAnalytics(Db db, string providerCampaignId)
    {
        var conn = LiveConnection(db, "linkedin_ads");
        if (conn is null) return Fail("no_connected_linkedin_ads");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        try
        {
            var url = "https://api.linkedin.com/rest/adAnalytics?q=analytics&pivot=CAMPAIGN&timeGranularity=ALL"
                + $"&campaigns=List(urn%3Ali%3AsponsoredCampaign%3A{Uri.EscapeDataString(providerCampaignId)})&fields=impressions,clicks,costInUsd,externalWebsiteConversions";
            using var msg = new HttpRequestMessage(HttpMethod.Get, url);
            msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            msg.Headers.TryAddWithoutValidation("LinkedIn-Version", "202401");
            msg.Headers.TryAddWithoutValidation("X-Restli-Protocol-Version", "2.0.0");
            using var resp = Http.Send(msg);
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, providerCampaignId, body.Length > 6000 ? body[..6000] : body);
        }
        catch (Exception ex) { return Fail("linkedin_analytics_failed: " + ex.Message); }
    }

    /// <summary>Fetch LinkedIn Lead Gen form responses for a form via the Advertising API.</summary>
    public static Result LinkedInFetchLeads(Db db, string formId)
    {
        var conn = LiveConnection(db, "linkedin_ads");
        if (conn is null) return Fail("no_connected_linkedin_ads");
        var token = Security.DecryptSecret(H.Str(conn["access_token_enc"]));
        if (string.IsNullOrWhiteSpace(token)) return Fail("no_access_token");
        try
        {
            var url = $"https://api.linkedin.com/rest/leadFormResponses?q=owner&owner=(sponsoredAccount:urn:li:sponsoredAccount:{Uri.EscapeDataString(H.Str(conn["external_ad_account_id"]) ?? "")})&leadType=(leadFormResponse)";
            using var msg = new HttpRequestMessage(HttpMethod.Get, url);
            msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            msg.Headers.TryAddWithoutValidation("LinkedIn-Version", "202401");
            using var resp = Http.Send(msg);
            var body = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
            return new Result(resp.IsSuccessStatusCode, (int)resp.StatusCode, formId, body.Length > 8000 ? body[..8000] : body);
        }
        catch (Exception ex) { return Fail("linkedin_leads_failed: " + ex.Message); }
    }
}
