using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Optional export of earned credentials to the Credly (Acclaim) badge network, ON TOP of PCI's own native
/// Open Badges. Honest capability: it activates only when the operator sets CREDLY_API_TOKEN + CREDLY_ORG_ID
/// (never stored in the DB or the browser), and each certification maps to a Credly badge template
/// (certifications.credly_template_id). Until configured every call reports "not_configured" — nothing is faked.
///
/// Credly Issuing API: HTTP Basic auth with the API token as the username (empty password);
/// POST /v1/organizations/{org}/badges to issue, PUT .../badges/{id}/revoke to revoke. CREDLY_API_BASE
/// overrides the endpoint (defaults to https://api.credly.com) so the integration can be pointed at a sandbox.
/// </summary>
public static class CredlyConnector
{
    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(30));
    static string? E(string k) => Environment.GetEnvironmentVariable(k);

    public static string ApiBase => (E("CREDLY_API_BASE") ?? "https://api.credly.com").TrimEnd('/');
    static string? Token => E("CREDLY_API_TOKEN");
    static string? Org => E("CREDLY_ORG_ID");
    public static bool Enabled => !string.IsNullOrWhiteSpace(Token) && !string.IsNullOrWhiteSpace(Org);

    static void Authorize(HttpRequestMessage r)
    {
        var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes((Token ?? "") + ":"));   // token as username, empty password
        r.Headers.Authorization = new AuthenticationHeaderValue("Basic", basic);
        r.Headers.Accept.ParseAdd("application/json");
    }

    public record Result(bool Ok, string? BadgeId, string? Error);
    static string Trunc(string s, int n) => s.Length <= n ? s : s[..n];

    /// <summary>Issue (export) one credential to Credly. Idempotent — a credential already pushed returns its
    /// existing badge id without a second call. Records credly_badge_id / credly_state / credly_error.</summary>
    public static async Task<Result> IssueBadge(Db db, long credentialRowId)
    {
        if (!Enabled) return new(false, null, "not_configured");
        var c = db.QueryOne(@"SELECT ic.id,ic.credential_id,ic.user_id,ic.holder_name,ic.status,ic.issued_at,ic.expires_at,ic.credly_badge_id,
                   ct.credly_template_id FROM issued_credentials ic LEFT JOIN certifications ct ON ct.id=COALESCE(ic.certification_id,1) WHERE ic.id=?", credentialRowId);
        if (c is null) return new(false, null, "credential_not_found");
        if (H.Str(c["status"]) != "active") return new(false, null, "credential_not_active");
        if (H.Str(c["credly_badge_id"]) is { Length: > 0 } existing) return new(true, existing, null);   // already issued
        var template = H.Str(c["credly_template_id"]);
        if (string.IsNullOrWhiteSpace(template)) return new(false, null, "no_template_mapped");
        var email = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", c["user_id"])?["email"]);
        if (string.IsNullOrWhiteSpace(email)) return new(false, null, "no_recipient_email");
        var name = (H.Str(c["holder_name"]) ?? "").Trim();
        var sp = name.IndexOf(' ');
        var body = new Dictionary<string, object?>
        {
            ["recipient_email"] = email,
            ["issued_to_first_name"] = sp > 0 ? name[..sp] : name,
            ["issued_to_last_name"] = sp > 0 ? name[(sp + 1)..] : "",
            ["badge_template_id"] = template,
            ["issued_at"] = H.Str(c["issued_at"]) is { Length: >= 10 } iss ? iss[..10] : DateTime.UtcNow.ToString("yyyy-MM-dd"),
        };
        if (H.Str(c["expires_at"]) is { Length: >= 10 } ex) body["expires_at"] = ex[..10];
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, $"{ApiBase}/v1/organizations/{Org}/badges")
            { Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json") };
            Authorize(req);
            using var resp = await Http.SendAsync(req);
            var txt = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
            {
                var err = $"HTTP {(int)resp.StatusCode}";
                db.Execute("UPDATE issued_credentials SET credly_state='error', credly_error=? WHERE id=?", Trunc(err + ": " + txt, 400), credentialRowId);
                return new(false, null, err);
            }
            using var doc = JsonDocument.Parse(txt);
            var badgeId = doc.RootElement.TryGetProperty("data", out var data) && data.TryGetProperty("id", out var idv) ? idv.GetString() : null;
            db.Execute("UPDATE issued_credentials SET credly_badge_id=?, credly_state='issued', credly_error=NULL WHERE id=?", badgeId, credentialRowId);
            return new(true, badgeId, null);
        }
        catch (Exception e)
        {
            db.Execute("UPDATE issued_credentials SET credly_state='error', credly_error=? WHERE id=?", Trunc(e.Message, 400), credentialRowId);
            return new(false, null, e.Message);
        }
    }

    /// <summary>Revoke a credential's Credly badge (no-op if it was never pushed).</summary>
    public static async Task<Result> RevokeBadge(Db db, long credentialRowId, string reason = "Credential revoked by the issuer")
    {
        if (!Enabled) return new(false, null, "not_configured");
        var badgeId = H.Str(db.QueryOne("SELECT credly_badge_id FROM issued_credentials WHERE id=?", credentialRowId)?["credly_badge_id"]);
        if (string.IsNullOrWhiteSpace(badgeId)) return new(true, null, null);
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Put, $"{ApiBase}/v1/organizations/{Org}/badges/{badgeId}/revoke")
            { Content = new StringContent(JsonSerializer.Serialize(new { reason }), Encoding.UTF8, "application/json") };
            Authorize(req);
            using var resp = await Http.SendAsync(req);
            if (!resp.IsSuccessStatusCode) return new(false, null, $"HTTP {(int)resp.StatusCode}");
            db.Execute("UPDATE issued_credentials SET credly_state='revoked' WHERE id=?", credentialRowId);
            return new(true, badgeId, null);
        }
        catch (Exception e) { return new(false, null, e.Message); }
    }

    /// <summary>Push all active, template-mapped, not-yet-exported credentials (bounded batch). Returns counts.</summary>
    public static async Task<(int pushed, int failed, int skipped)> SyncPending(Db db, int limit = 50)
    {
        if (!Enabled) return (0, 0, 0);
        var rows = db.Query(@"SELECT ic.id FROM issued_credentials ic
            JOIN certifications ct ON ct.id=COALESCE(ic.certification_id,1)
            LEFT JOIN users u ON u.id=ic.user_id
            WHERE ic.status='active' AND (ic.credly_badge_id IS NULL OR ic.credly_badge_id='')
              AND ct.credly_template_id IS NOT NULL AND ct.credly_template_id<>'' AND COALESCE(u.is_test,0)=0
            ORDER BY ic.id DESC LIMIT " + Math.Clamp(limit, 1, 200));
        int pushed = 0, failed = 0;
        foreach (var r in rows)
        {
            var res = await IssueBadge(db, H.L(r["id"]));
            if (res.Ok) pushed++; else failed++;
        }
        return (pushed, failed, 0);
    }
}
