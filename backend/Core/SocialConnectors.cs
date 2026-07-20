using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Live social-publishing connectors for the platforms whose OFFICIAL APIs need no platform review — a
/// PCI-controlled webhook/token is enough: Discord (incoming webhook), Telegram (Bot API), Mastodon
/// (statuses API) and Bluesky (AT Protocol). Each takes a connected account (credentials decrypted from
/// cc_social_accounts.secret_enc at call time — never logged) plus the post text/link and returns a result
/// with the public URL. All outbound calls go through the SSRF-guarded egress client because instance /
/// webhook URLs are operator-supplied. Approval-gated platforms (LinkedIn/Meta/X/…) are deliberately NOT
/// here — they stay framework-only in the capability registry until the operator completes provider review.
/// </summary>
public static class SocialConnectors
{
    public static readonly string[] Live = { "discord", "telegram", "mastodon", "bluesky" };
    public static bool IsLive(string platform) => Array.IndexOf(Live, (platform ?? "").ToLowerInvariant()) >= 0;

    public record Result(bool Ok, string? PublicUrl, int Code, string? Error);

    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(30));

    /// <summary>Publish one draft to its account's platform. Throws never.</summary>
    public static async Task<Result> Publish(Db db, Dictionary<string, object?> account, string text, string? link)
    {
        try
        {
            var platform = (H.Str(account["platform_key"]) ?? "").ToLowerInvariant();
            var cfg = ParseCfg(H.Str(account["config"]));
            var secret = Security.DecryptSecret(H.Str(account["secret_enc"])) ?? "";
            var body = string.IsNullOrWhiteSpace(link) ? text : (text + "\n\n" + link);
            return platform switch
            {
                "discord" => await Discord(secret, body),
                "telegram" => await Telegram(secret, cfg, body),
                "mastodon" => await Mastodon(secret, cfg, body),
                "bluesky" => await Bluesky(secret, cfg, text, link),
                _ => new Result(false, null, 0, "unsupported_platform"),
            };
        }
        catch (Exception e) { return new Result(false, null, 0, e.Message); }
    }

    /// <summary>Lightweight connectivity check for the account (no public post where avoidable).</summary>
    public static async Task<Result> Test(Db db, Dictionary<string, object?> account)
    {
        try
        {
            var platform = (H.Str(account["platform_key"]) ?? "").ToLowerInvariant();
            var cfg = ParseCfg(H.Str(account["config"]));
            var secret = Security.DecryptSecret(H.Str(account["secret_enc"])) ?? "";
            switch (platform)
            {
                case "telegram":
                    {
                        var r = await Http.GetAsync(TgBase(cfg) + "/bot" + secret + "/getMe");
                        return new Result(r.IsSuccessStatusCode, null, (int)r.StatusCode, r.IsSuccessStatusCode ? null : await r.Content.ReadAsStringAsync());
                    }
                case "mastodon":
                    {
                        using var req = new HttpRequestMessage(HttpMethod.Get, TrimSlash(cfg.GetValueOrDefault("instance", "")) + "/api/v1/accounts/verify_credentials");
                        req.Headers.TryAddWithoutValidation("Authorization", "Bearer " + secret);
                        var r = await Http.SendAsync(req);
                        return new Result(r.IsSuccessStatusCode, null, (int)r.StatusCode, r.IsSuccessStatusCode ? null : Trim(await r.Content.ReadAsStringAsync()));
                    }
                case "bluesky":
                    {
                        var (ok, _, _, _, err) = await BskySession(cfg, secret);
                        return new Result(ok, null, ok ? 200 : 401, err);
                    }
                case "discord":
                    // A webhook URL can only be verified by posting; treat a well-formed https webhook as configured.
                    return new Result(secret.StartsWith("https://", StringComparison.OrdinalIgnoreCase), null, 0,
                        secret.StartsWith("https://") ? null : "invalid_webhook_url");
                default:
                    return new Result(false, null, 0, "unsupported_platform");
            }
        }
        catch (Exception e) { return new Result(false, null, 0, e.Message); }
    }

    // ── Discord (incoming webhook) ────────────────────────────────────────────────────────────────────
    static async Task<Result> Discord(string webhookUrl, string content)
    {
        if (!webhookUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) return new Result(false, null, 0, "invalid_webhook_url");
        var payload = JsonSerializer.Serialize(new { content = Cap(content, 1900) });
        // ?wait=true makes Discord return the created message so we can surface a public reference.
        var url = webhookUrl + (webhookUrl.Contains('?') ? "&" : "?") + "wait=true";
        using var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(payload, Encoding.UTF8, "application/json") };
        var r = await Http.SendAsync(req);
        var txt = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) return new Result(false, null, (int)r.StatusCode, Trim(txt));
        string? mid = null; try { using var d = JsonDocument.Parse(txt); if (d.RootElement.TryGetProperty("id", out var i)) mid = i.GetString(); } catch { }
        return new Result(true, mid is null ? null : "discord:message:" + mid, (int)r.StatusCode, null);
    }

    // ── Telegram (Bot API sendMessage) ────────────────────────────────────────────────────────────────
    static async Task<Result> Telegram(string token, Dictionary<string, string> cfg, string text)
    {
        var chat = cfg.GetValueOrDefault("chat_id", "");
        if (chat.Length == 0) return new Result(false, null, 0, "chat_id_required");
        var payload = JsonSerializer.Serialize(new { chat_id = chat, text = Cap(text, 4000), disable_web_page_preview = false });
        using var req = new HttpRequestMessage(HttpMethod.Post, TgBase(cfg) + "/bot" + token + "/sendMessage") { Content = new StringContent(payload, Encoding.UTF8, "application/json") };
        var r = await Http.SendAsync(req);
        var txt = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) return new Result(false, null, (int)r.StatusCode, Trim(txt));
        string? url = null;
        try
        {
            using var d = JsonDocument.Parse(txt);
            if (d.RootElement.TryGetProperty("result", out var res) && res.TryGetProperty("message_id", out var mid))
            {
                var uname = cfg.GetValueOrDefault("channel_username", "");
                url = uname.Length > 0 ? $"https://t.me/{uname.TrimStart('@')}/{mid.GetRawText()}" : "telegram:message:" + mid.GetRawText();
            }
        }
        catch { }
        return new Result(true, url, (int)r.StatusCode, null);
    }

    // ── Mastodon (POST /api/v1/statuses) ──────────────────────────────────────────────────────────────
    static async Task<Result> Mastodon(string token, Dictionary<string, string> cfg, string status)
    {
        var instance = TrimSlash(cfg.GetValueOrDefault("instance", ""));
        if (instance.Length == 0) return new Result(false, null, 0, "instance_required");
        var payload = JsonSerializer.Serialize(new { status = Cap(status, 500) });
        using var req = new HttpRequestMessage(HttpMethod.Post, instance + "/api/v1/statuses") { Content = new StringContent(payload, Encoding.UTF8, "application/json") };
        req.Headers.TryAddWithoutValidation("Authorization", "Bearer " + token);
        // Idempotency-Key protects against duplicate posts on retry.
        req.Headers.TryAddWithoutValidation("Idempotency-Key", Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(instance + status)))[..32]);
        var r = await Http.SendAsync(req);
        var txt = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) return new Result(false, null, (int)r.StatusCode, Trim(txt));
        string? url = null; try { using var d = JsonDocument.Parse(txt); if (d.RootElement.TryGetProperty("url", out var u)) url = u.GetString(); } catch { }
        return new Result(true, url, (int)r.StatusCode, null);
    }

    // ── Bluesky (AT Protocol: createSession → createRecord) ───────────────────────────────────────────
    static async Task<Result> Bluesky(string appPassword, Dictionary<string, string> cfg, string text, string? link)
    {
        var (ok, jwt, did, pds, err) = await BskySession(cfg, appPassword);
        if (!ok) return new Result(false, null, 401, err);
        var createdAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        var record = new Dictionary<string, object?> { ["$type"] = "app.bsky.feed.post", ["text"] = Cap(text, 300), ["createdAt"] = createdAt };
        var payload = JsonSerializer.Serialize(new { repo = did, collection = "app.bsky.feed.post", record });
        using var req = new HttpRequestMessage(HttpMethod.Post, pds + "/xrpc/com.atproto.repo.createRecord") { Content = new StringContent(payload, Encoding.UTF8, "application/json") };
        req.Headers.TryAddWithoutValidation("Authorization", "Bearer " + jwt);
        var r = await Http.SendAsync(req);
        var txt = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) return new Result(false, null, (int)r.StatusCode, Trim(txt));
        string? url = null;
        try
        {
            using var d = JsonDocument.Parse(txt);
            if (d.RootElement.TryGetProperty("uri", out var u) && u.GetString() is { } at)
            {
                var rkey = at.Split('/').LastOrDefault();
                var handle = cfg.GetValueOrDefault("handle", did);
                url = rkey is null ? at : $"https://bsky.app/profile/{handle}/post/{rkey}";
            }
        }
        catch { }
        return new Result(true, url, (int)r.StatusCode, null);
    }

    static async Task<(bool ok, string jwt, string did, string pds, string? err)> BskySession(Dictionary<string, string> cfg, string appPassword)
    {
        var pds = TrimSlash(cfg.GetValueOrDefault("pds", "https://bsky.social"));
        var identifier = cfg.GetValueOrDefault("handle", "");
        if (identifier.Length == 0) return (false, "", "", pds, "handle_required");
        var payload = JsonSerializer.Serialize(new { identifier, password = appPassword });
        using var req = new HttpRequestMessage(HttpMethod.Post, pds + "/xrpc/com.atproto.server.createSession") { Content = new StringContent(payload, Encoding.UTF8, "application/json") };
        var r = await Http.SendAsync(req);
        var txt = await r.Content.ReadAsStringAsync();
        if (!r.IsSuccessStatusCode) return (false, "", "", pds, Trim(txt));
        try { using var d = JsonDocument.Parse(txt); return (true, d.RootElement.GetProperty("accessJwt").GetString() ?? "", d.RootElement.GetProperty("did").GetString() ?? "", pds, null); }
        catch (Exception e) { return (false, "", "", pds, e.Message); }
    }

    // ── helpers ───────────────────────────────────────────────────────────────────────────────────────
    static Dictionary<string, string> ParseCfg(string? json)
    {
        var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(json)) return d;
        try { using var doc = JsonDocument.Parse(json); foreach (var p in doc.RootElement.EnumerateObject()) d[p.Name] = p.Value.ValueKind == JsonValueKind.String ? p.Value.GetString() ?? "" : p.Value.ToString(); }
        catch { }
        return d;
    }
    static string TgBase(Dictionary<string, string> cfg) => TrimSlash(cfg.GetValueOrDefault("api_base", "https://api.telegram.org"));
    static string TrimSlash(string s) => (s ?? "").TrimEnd('/');
    static string Cap(string s, int n) => s.Length <= n ? s : s[..(n - 1)] + "…";
    static string Trim(string s) => s.Length > 300 ? s[..300] : s;
}
