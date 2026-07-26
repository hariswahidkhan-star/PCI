using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Zoho Books connector (RES-008). Plugs into the same integration pipeline as the generic webhook and
/// QuickBooks connectors — outbox, delivery ledger, retry/backoff, dispatcher and egress guard are all
/// unchanged; this class only adds Zoho's authentication, regional routing and canonical→Zoho mapping.
///
///   member.registered    → Contact  (contact_name + email + contact_persons)
///   payment.recorded     → Invoice  (one line item, against the member's contact)
///   membership.activated → (no Books object — skipped, exactly like QuickBooks)
///
/// Zoho Books is customer-centric: an invoice must belong to a contact. So a payment delivery resolves
/// the contact by email first (GET /contacts?email=…) and creates one when absent — the same two steps an
/// operator would perform by hand. Both calls go through the egress-guarded HttpClient.
///
/// Auth: a manually-set access_token wins (short-lived tokens / testing against a stand-in); otherwise an
/// access token is minted from the stored OAuth refresh_token with the app's client_id/client_secret.
/// With neither, the delivery fails with a clear "not authorized" message — never a fake success.
///
/// Region matters: Zoho runs separate data centres and a token minted in one is invalid in another, so the
/// accounts host and the API host are both derived from the configured region (com | eu | in | com.au |
/// jp | ca), and `api_base` can override the API host to point at a test receiver.
/// </summary>
public static class ZohoConnector
{
    /// <summary>Zoho data centres. The key is what an operator picks; the value is the domain suffix.</summary>
    static readonly Dictionary<string, string> Regions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["com"] = "com", ["us"] = "com",
        ["eu"] = "eu",
        ["in"] = "in",
        ["au"] = "com.au", ["com.au"] = "com.au",
        ["jp"] = "jp",
        ["ca"] = "ca",
    };

    public static string NormaliseRegion(string? raw)
    {
        var key = (raw ?? "").Trim();
        return key.Length > 0 && Regions.TryGetValue(key, out var dom) ? dom : "com";
    }

    sealed record Config(string organizationId, string region, string? apiBase, string? clientId);

    static Config ParseConfig(string? json)
    {
        string G(JsonElement e, string k) => e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
        try
        {
            var e = JsonDocument.Parse(json ?? "{}").RootElement;
            return new Config(G(e, "organization_id"), NormaliseRegion(G(e, "region")),
                G(e, "api_base") is { Length: > 0 } ab ? ab : null,
                G(e, "client_id") is { Length: > 0 } ci ? ci : null);
        }
        catch { return new Config("", "com", null, null); }
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

    /// <summary>The Books API host for a region, unless an explicit api_base override is configured.</summary>
    public static string ApiBase(string region, string? apiBase) =>
        (apiBase ?? $"https://www.zohoapis.{region}").TrimEnd('/');

    /// <summary>The OAuth accounts host for a region. Never overridden by api_base — tokens are minted
    /// by Zoho's accounts service, not by whatever receiver the API base points at.</summary>
    public static string AccountsBase(string region) => $"https://accounts.zoho.{region}";

    /// <summary>The Contact body for a member (also used when a payment has to create its contact).</summary>
    public static string ContactBody(string firstName, string lastName, string email)
    {
        var display = $"{firstName} {lastName}".Trim();
        if (display.Length == 0) display = email.Length > 0 ? email : "PCI Member";
        var obj = new Dictionary<string, object?> { ["contact_name"] = display, ["contact_type"] = "customer" };
        if (email.Length > 0) obj["email"] = email;
        if (firstName.Length > 0 || lastName.Length > 0 || email.Length > 0)
            obj["contact_persons"] = new[] { new Dictionary<string, object?>
            {
                ["first_name"] = firstName, ["last_name"] = lastName,
                ["email"] = email, ["is_primary_contact"] = true,
            } };
        return JsonSerializer.Serialize(obj);
    }

    /// <summary>The Invoice body for a settled payment, against an already-resolved contact.</summary>
    public static string InvoiceBody(string contactId, double amount, string product, string? reference)
    {
        var obj = new Dictionary<string, object?>
        {
            ["customer_id"] = contactId,
            ["line_items"] = new[] { new Dictionary<string, object?>
            {
                ["name"] = product.Length > 0 ? product : "PCI fee",
                ["description"] = product.Length > 0 ? product : "PCI fee",
                ["rate"] = Math.Round(amount, 2),
                ["quantity"] = 1,
            } },
        };
        if (!string.IsNullOrWhiteSpace(reference)) obj["reference_number"] = reference;
        return obj is null ? "{}" : JsonSerializer.Serialize(obj);
    }

    /// <summary>Which Books resource a canonical event maps to. Null = no Books representation (skip).</summary>
    public static string? ResourceFor(string eventType) => eventType switch
    {
        "member.registered" => "contacts",
        "payment.recorded" => "invoices",
        _ => null,                       // membership.activated is a CRM state, not an accounting entry
    };

    /// <summary>Build the Zoho Books request for a delivery, or null when the event is unmapped (skip).
    /// Throws with a clear message when the connector is misconfigured / not authorized.</summary>
    public static async Task<HttpRequestMessage?> BuildRequest(Db db, HttpClient http, Dictionary<string, object?> integ, string eventType, string payloadRaw)
    {
        var config = ParseConfig(H.Str(integ["config"]));
        if (config.organizationId.Length == 0) throw new Exception("Zoho Books not configured — set the organization id.");
        var resource = ResourceFor(eventType);
        if (resource is null) return null;

        var data = JsonDocument.Parse(payloadRaw).RootElement;
        string S(string k) => data.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
        double N(string k) => data.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetDouble(out var d) ? d : 0;

        var token = await EnsureAccessToken(db, http, H.L(integ["id"]), config, ParseSecret(H.Str(integ["secret"])));
        var apiBase = ApiBase(config.region, config.apiBase);
        var org = Uri.EscapeDataString(config.organizationId);

        string body;
        if (resource == "contacts")
        {
            body = ContactBody(S("first_name"), S("last_name"), S("email"));
        }
        else
        {
            // An invoice needs its contact. Resolve by email, creating the contact when Books has none —
            // the same two steps an operator would take, and the reason a payment delivery can make more
            // than one call.
            var email = S("email");
            if (email.Length == 0) throw new Exception("Zoho Books invoice needs the payer's email to resolve the contact.");
            var contactId = await ResolveContactId(http, apiBase, org, token, email)
                            ?? await CreateContactId(http, apiBase, org, token, ContactBody(S("first_name"), S("last_name"), email));
            body = InvoiceBody(contactId, N("amount"), S("product"), S("reference") is { Length: > 0 } r ? r : null);
        }

        var req = new HttpRequestMessage(HttpMethod.Post, $"{apiBase}/books/v3/{resource}?organization_id={org}")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };
        Authorize(req, token);
        return req;
    }

    static void Authorize(HttpRequestMessage req, string token)
    {
        // Zoho's scheme is "Zoho-oauthtoken <token>", not Bearer.
        req.Headers.TryAddWithoutValidation("Authorization", "Zoho-oauthtoken " + token);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        req.Headers.TryAddWithoutValidation("User-Agent", "PCI-Integrations/1.0");
    }

    static async Task<string?> ResolveContactId(HttpClient http, string apiBase, string org, string token, string email)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get,
            $"{apiBase}/books/v3/contacts?organization_id={org}&email={Uri.EscapeDataString(email)}");
        Authorize(req, token);
        using var resp = await http.SendAsync(req);
        if (!resp.IsSuccessStatusCode) return null;
        using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        if (doc.RootElement.TryGetProperty("contacts", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var c in arr.EnumerateArray())
                if (c.TryGetProperty("contact_id", out var idEl))
                    return idEl.ValueKind == JsonValueKind.String ? idEl.GetString() : idEl.ToString();
        return null;
    }

    static async Task<string> CreateContactId(HttpClient http, string apiBase, string org, string token, string contactBody)
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, $"{apiBase}/books/v3/contacts?organization_id={org}")
        {
            Content = new StringContent(contactBody, Encoding.UTF8, "application/json"),
        };
        Authorize(req, token);
        using var resp = await http.SendAsync(req);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
            throw new Exception($"Zoho Books contact create failed (HTTP {(int)resp.StatusCode})");
        using var doc = JsonDocument.Parse(txt);
        if (doc.RootElement.TryGetProperty("contact", out var c) && c.TryGetProperty("contact_id", out var idEl))
            return idEl.ValueKind == JsonValueKind.String ? idEl.GetString() ?? "" : idEl.ToString();
        throw new Exception("Zoho Books contact create returned no contact id");
    }

    /// <summary>A manually-set access token wins; otherwise mint one from the refresh token. With neither,
    /// the connector is not authorized — surface that clearly instead of failing opaquely. A freshly minted
    /// access token is persisted encrypted so the next delivery reuses it.</summary>
    static async Task<string> EnsureAccessToken(Db db, HttpClient http, long integId, Config config,
        (string? clientSecret, string? refreshToken, string? accessToken) secret)
    {
        if (secret.accessToken is { Length: > 0 } at) return at;
        if (secret.refreshToken is { Length: > 0 } rt && config.clientId is { Length: > 0 } cid && secret.clientSecret is { Length: > 0 } cs)
        {
            using var tr = new HttpRequestMessage(HttpMethod.Post, $"{AccountsBase(config.region)}/oauth/v2/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["refresh_token"] = rt, ["client_id"] = cid, ["client_secret"] = cs, ["grant_type"] = "refresh_token",
                }),
            };
            tr.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using var resp = await http.SendAsync(tr);
            var txt = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode) throw new Exception($"Zoho token refresh failed (HTTP {(int)resp.StatusCode})");
            using var doc = JsonDocument.Parse(txt);
            // Zoho answers 200 with an {"error":"invalid_code"} body on failure — treat that as a failure,
            // not as a token, or every delivery would carry an empty Authorization header.
            if (doc.RootElement.TryGetProperty("error", out var errEl))
                throw new Exception($"Zoho token refresh rejected: {errEl.GetString() ?? "error"}");
            if (doc.RootElement.TryGetProperty("access_token", out var atEl) && atEl.GetString() is { Length: > 0 } fresh)
            {
                try
                {
                    var blob = JsonSerializer.Serialize(new Dictionary<string, string>
                    {
                        ["client_secret"] = cs, ["refresh_token"] = rt, ["access_token"] = fresh,
                    });
                    db.Execute("UPDATE integrations SET secret=?, updated_at=datetime('now') WHERE id=?",
                        Security.EncryptSecret(blob) ?? blob, integId);
                }
                catch (Exception e) { Console.Error.WriteLine($"[zoho] token persist failed: {e.Message}"); }
                return fresh;
            }
            throw new Exception("Zoho token refresh returned no access token");
        }
        throw new Exception("Zoho Books not authorized — provide an access token, or a refresh token with the app's client id/secret (OAuth).");
    }
}
