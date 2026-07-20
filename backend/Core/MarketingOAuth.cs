using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Official 3-legged OAuth for the marketing platforms (Phase 2). Client id/secret come only from Render
/// environment variables — never from MySQL or the browser. The authorize URL carries a signed, expiring
/// CSRF <c>state</c> that binds the callback to a specific connection row, and the callback exchanges the
/// code for tokens that are stored ENCRYPTED on the connection (Security.EncryptSecret). Access/refresh
/// tokens never leave the backend.
///
/// This is a real implementation; it activates the moment the operator sets the provider app credentials
/// and registers the redirect URL. Until then, AuthorizeUrl returns null (the endpoint reports an honest
/// operator action) — nothing is faked as connected.
/// </summary>
public static class MarketingOAuth
{
    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(30));
    static string? E(string k) => Environment.GetEnvironmentVariable(k);

    static string StateSecret =>
        (E("MARKETING_OAUTH_SECRET") ?? E("CREDENTIAL_ENCRYPTION_KEY") ?? "pci-marketing-oauth-v1") + "|pci-mkt-oauth";

    /// <summary>Signed, self-verifying, 1-hour state: <c>{connId}.{expUnix}.{sig}</c>.</summary>
    public static string State(long connId, long nowUnix)
    {
        var exp = nowUnix + 3600;
        return $"{connId}.{exp}.{Security.Sha($"{connId}.{exp}.{StateSecret}")[..24]}";
    }
    /// <summary>Verify state and return the connection id (null if bad/expired). Caller supplies now.</summary>
    public static long? VerifyState(string? state, long nowUnix)
    {
        if (string.IsNullOrEmpty(state)) return null;
        var p = state.Split('.');
        if (p.Length != 3 || !long.TryParse(p[0], out var connId) || !long.TryParse(p[1], out var exp)) return null;
        if (exp < nowUnix) return null;
        return Security.FixedTimeEquals(p[2], Security.Sha($"{connId}.{exp}.{StateSecret}")[..24]) ? connId : null;
    }

    // Per-family endpoints + env var names. Scopes are chosen per platform_code below.
    record Provider(string ClientIdEnv, string ClientSecretEnv, string AuthUrl, string TokenUrl);
    static Provider? ForFamily(string family) => family switch
    {
        "linkedin" => new("LINKEDIN_CLIENT_ID", "LINKEDIN_CLIENT_SECRET",
            "https://www.linkedin.com/oauth/v2/authorization", "https://www.linkedin.com/oauth/v2/accessToken"),
        "google" => new("GOOGLE_OAUTH_CLIENT_ID", "GOOGLE_OAUTH_CLIENT_SECRET",
            "https://accounts.google.com/o/oauth2/v2/auth", "https://oauth2.googleapis.com/token"),
        "meta" => new("META_APP_ID", "META_APP_SECRET",
            "https://www.facebook.com/v21.0/dialog/oauth", "https://graph.facebook.com/v21.0/oauth/access_token"),
        _ => null,
    };

    // The official scopes each platform_code needs. Kept minimal per feature.
    public static string Scopes(string platformCode) => platformCode switch
    {
        "linkedin_page" => "r_organization_social w_organization_social rw_organization_admin",
        "linkedin_ads" or "linkedin_conversation" => "rw_ads r_ads_reporting",
        "google_search_console" => "https://www.googleapis.com/auth/webmasters",
        "google_ads" => "https://www.googleapis.com/auth/adwords",
        "meta_ads" or "facebook_page" or "instagram" => "ads_management,ads_read,business_management,pages_show_list,pages_read_engagement,leads_retrieval",
        _ => "",
    };

    /// <summary>The fixed redirect URL registered with each provider. Derived from APP_BASE_URL/SITE_BASE_URL.</summary>
    public static string RedirectUri(HttpRequest? req = null) => Mailer.BaseUrl(req) + "/api/marketing/oauth/callback";

    /// <summary>Build a real authorize URL for a connection, or null when the provider app is not configured.</summary>
    public static string? AuthorizeUrl(Db db, long connId, long nowUnix, HttpRequest? req = null)
    {
        var c = db.QueryOne("SELECT platform_code FROM mkt_connections WHERE id=?", connId);
        if (c is null) return null;
        var platform = H.Str(c["platform_code"]) ?? "";
        var family = H.Str(db.QueryOne("SELECT family FROM mkt_platforms WHERE code=?", platform)?["family"]) ?? "";
        var p = ForFamily(family);
        if (p is null) return null;
        var clientId = E(p.ClientIdEnv);
        if (string.IsNullOrWhiteSpace(clientId)) return null;   // not configured → honest null
        var q = new Dictionary<string, string?>
        {
            ["response_type"] = "code",
            ["client_id"] = clientId,
            ["redirect_uri"] = RedirectUri(req),
            ["scope"] = Scopes(platform),
            ["state"] = State(connId, nowUnix),
        };
        if (family == "google") { q["access_type"] = "offline"; q["prompt"] = "consent"; q["include_granted_scopes"] = "true"; }
        var qs = string.Join("&", q.Where(kv => kv.Value is not null).Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}"));
        return p.AuthUrl + "?" + qs;
    }

    public record TokenResult(string? AccessToken, string? RefreshToken, long? ExpiresInSeconds, string? Scope, string Raw);

    /// <summary>Exchange an authorization code for tokens at the provider token endpoint. Returns null on
    /// failure (recorded by the caller). Env-gated: no client secret → no call.</summary>
    public static async Task<TokenResult?> Exchange(string family, string code, string redirectUri)
    {
        var p = ForFamily(family);
        if (p is null) return null;
        var clientId = E(p.ClientIdEnv); var clientSecret = E(p.ClientSecretEnv);
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret)) return null;
        var form = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = redirectUri,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
        };
        try
        {
            using var reqMsg = new HttpRequestMessage(HttpMethod.Post, p.TokenUrl) { Content = new FormUrlEncodedContent(form) };
            reqMsg.Headers.Accept.ParseAdd("application/json");
            using var resp = await Http.SendAsync(reqMsg);
            var body = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode) { Console.Error.WriteLine($"[mkt-oauth] {family} token {(int)resp.StatusCode}: {body}"); return null; }
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            string? Str(string k) => root.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
            long? exp = root.TryGetProperty("expires_in", out var ev) && ev.TryGetInt64(out var e) ? e : null;
            return new TokenResult(Str("access_token"), Str("refresh_token"), exp, Str("scope"), body);
        }
        catch (Exception ex) { Console.Error.WriteLine($"[mkt-oauth] {family} token exchange failed: {ex.Message}"); return null; }
    }
}
