using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// QuickBooks Online connector (master-plan Phase 10 — the first concrete ERP connector). It plugs into
/// the Phase 9 integration pipeline: the outbox, delivery ledger, retry/backoff and the dispatcher are
/// unchanged — this class only adds QBO's authentication and its canonical→QBO object mapping.
///
///   member.registered  → Customer      (DisplayName + PrimaryEmailAddr)
///   payment.recorded   → SalesReceipt  (one line: Amount + ItemRef + description, BillEmail)
///   membership.activated → (no QBO object — skipped)
///
/// Auth: a delivery uses a manually-set access_token when present (short-lived tokens, or testing against
/// a stand-in), otherwise it refreshes an access token from the stored OAuth refresh_token
/// (client_id/client_secret). With neither, the delivery fails with a clear "not authorized" message —
/// never a fake success. The API host follows environment (production | sandbox) and can be overridden
/// (api_base) to point at a test receiver; the mapped request is identical either way.
/// </summary>
public static class QuickBooksConnector
{
    const string ProdBase = "https://quickbooks.api.intuit.com";
    const string SandboxBase = "https://sandbox-quickbooks.api.intuit.com";
    const string TokenEndpoint = "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer";
    const int MinorVersion = 65;

    sealed record Config(string realmId, string environment, string itemRef, string? apiBase, string? clientId);

    static Config ParseConfig(string? json)
    {
        string G(JsonElement e, string k) => e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
        try
        {
            var e = JsonDocument.Parse(json ?? "{}").RootElement;
            return new Config(G(e, "realm_id"), G(e, "environment") is { Length: > 0 } env ? env : "production",
                G(e, "item_ref") is { Length: > 0 } it ? it : "1", G(e, "api_base") is { Length: > 0 } ab ? ab : null,
                G(e, "client_id") is { Length: > 0 } ci ? ci : null);
        }
        catch { return new Config("", "production", "1", null, null); }
    }

    static (string? clientSecret, string? refreshToken, string? accessToken) ParseSecret(string? json)
    {
        var plain = Security.DecryptSecret(json) ?? json;
        try
        {
            var e = JsonDocument.Parse(plain ?? "{}").RootElement;
            string? G(string k) => e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String && v.GetString() is { Length: > 0 } s ? s : null;
            return (G("client_secret"), G("refresh_token"), G("access_token"));
        }
        catch { return (null, null, null); }
    }

    /// <summary>Map a canonical event to a QBO API entity + JSON body. Null = no QBO representation (skip).</summary>
    static (string entity, string body)? Map(string eventType, JsonElement data, string itemRef)
    {
        string S(string k) => data.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
        double N(string k) => data.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetDouble(out var d) ? d : 0;
        switch (eventType)
        {
            case "member.registered":
            {
                var email = S("email");
                var first = S("first_name"); var last = S("last_name");
                var display = $"{first} {last}".Trim();
                if (display.Length == 0) display = email.Length > 0 ? email : "PCI Member";
                var obj = new Dictionary<string, object?> { ["DisplayName"] = display };
                if (first.Length > 0) obj["GivenName"] = first;
                if (last.Length > 0) obj["FamilyName"] = last;
                if (email.Length > 0) obj["PrimaryEmailAddr"] = new { Address = email };
                return ("customer", JsonSerializer.Serialize(obj));
            }
            case "payment.recorded":
            {
                var amount = N("amount");
                var product = S("product"); if (product.Length == 0) product = "PCI fee";
                var line = new
                {
                    Amount = Math.Round(amount, 2),
                    DetailType = "SalesItemLineDetail",
                    Description = product,
                    SalesItemLineDetail = new { ItemRef = new { value = itemRef } },
                };
                var obj = new Dictionary<string, object?> { ["Line"] = new[] { line } };
                if (S("email") is { Length: > 0 } em) obj["BillEmail"] = new { Address = em };
                return ("salesreceipt", JsonSerializer.Serialize(obj));
            }
            default: return null;   // e.g. membership.activated — a CRM state, not an accounting entry
        }
    }

    /// <summary>Build the QBO API request for a delivery, or null when the event is unmapped (skip).
    /// Throws with a clear message when the connector is misconfigured / not authorized.</summary>
    public static async Task<HttpRequestMessage?> BuildRequest(Db db, HttpClient http, Dictionary<string, object?> integ, string eventType, string payloadRaw)
    {
        var config = ParseConfig(H.Str(integ["config"]));
        if (config.realmId.Length == 0) throw new Exception("QuickBooks not configured — set the company (realm) id.");
        var data = JsonDocument.Parse(payloadRaw).RootElement;
        var mapped = Map(eventType, data, config.itemRef);
        if (mapped is null) return null;

        var integId = H.L(integ["id"]);
        var token = await EnsureAccessToken(db, http, integId, config, ParseSecret(H.Str(integ["secret"])));
        var apiBase = (config.apiBase ?? (config.environment == "sandbox" ? SandboxBase : ProdBase)).TrimEnd('/');
        var url = $"{apiBase}/v3/company/{Uri.EscapeDataString(config.realmId)}/{mapped.Value.entity}?minorversion={MinorVersion}";
        var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(mapped.Value.body, Encoding.UTF8, "application/json") };
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        req.Headers.TryAddWithoutValidation("User-Agent", "PCI-Integrations/1.0");
        return req;
    }

    /// <summary>A manually-set access token wins; otherwise exchange the refresh token for one. With neither,
    /// the connector is not authorized — surface that clearly instead of failing opaquely.
    /// Rotated refresh/access tokens from Intuit are persisted encrypted (EXT-P1-05).</summary>
    static async Task<string> EnsureAccessToken(Db db, HttpClient http, long integId, Config config, (string? clientSecret, string? refreshToken, string? accessToken) secret)
    {
        if (secret.accessToken is { Length: > 0 } at) return at;
        if (secret.refreshToken is { Length: > 0 } rt && config.clientId is { Length: > 0 } cid && secret.clientSecret is { Length: > 0 } cs)
        {
            var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cid}:{cs}"));
            using var tr = new HttpRequestMessage(HttpMethod.Post, TokenEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["grant_type"] = "refresh_token", ["refresh_token"] = rt }),
            };
            tr.Headers.Authorization = new AuthenticationHeaderValue("Basic", basic);
            tr.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using var resp = await http.SendAsync(tr);
            var txt = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode) throw new Exception($"QuickBooks token refresh failed (HTTP {(int)resp.StatusCode})");
            using var doc = JsonDocument.Parse(txt);
            if (doc.RootElement.TryGetProperty("access_token", out var atEl) && atEl.GetString() is { Length: > 0 } fresh)
            {
                var newRefresh = doc.RootElement.TryGetProperty("refresh_token", out var rtEl) && rtEl.GetString() is { Length: > 0 } nr ? nr : rt;
                try
                {
                    var blob = JsonSerializer.Serialize(new Dictionary<string, string>
                    {
                        ["client_secret"] = cs,
                        ["refresh_token"] = newRefresh,
                        ["access_token"] = fresh,
                    });
                    db.Execute("UPDATE integrations SET secret=?, updated_at=datetime('now') WHERE id=?",
                        Security.EncryptSecret(blob) ?? blob, integId);
                }
                catch (Exception e) { Console.Error.WriteLine($"[qbo] token persist failed: {e.Message}"); }
                return fresh;
            }
            throw new Exception("QuickBooks token refresh returned no access token");
        }
        throw new Exception("QuickBooks not authorized — provide an access token, or a refresh token with the app's client id/secret (OAuth).");
    }
}
