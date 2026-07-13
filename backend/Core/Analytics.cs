using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// First-party, cookieless product analytics (Admin → Analytics). Page views are recorded
/// server-side when public pages are served; conversion events (registration, login, purchases,
/// membership activation) are recorded at their server endpoints — so reporting works with no
/// client script and no consent-requiring cookie.
///
/// Privacy by construction (§13.5): no raw IP is ever stored here — visitors are counted via a
/// SHA-256 of (IP, UA, UTC-day, boot salt), which cannot be reversed and rotates daily. Country
/// comes only from a trusted CDN geo header when one exists (Cloudflare/Vercel/AWS), else null.
/// Obvious bots are excluded. Security logs (login_events) keep full IPs separately, unchanged.
///
/// Attribution (§13.3): the FIRST public visit stores source/medium/campaign/referrer/landing in a
/// first-party cookie (pci_attr, 90 days, no personal data); conversion events copy it, so
/// campaign → registration → purchase is joined without confidential data in URLs. The portal and
/// website share this origin today; when mypci.org is split out, the same cookie rides on the
/// shared parent domain or link decoration (documented in the phase report).
/// </summary>
public static class Analytics
{
    public const string AttrCookie = "pci_attr";
    static readonly string BootSalt = Convert.ToHexString(RandomNumberGenerator.GetBytes(8));
    static readonly string[] BotMarkers = { "bot", "crawler", "spider", "slurp", "curl", "wget", "python-requests", "httpclient", "lighthouse", "headless" };

    public static bool IsBot(string ua) =>
        ua.Length == 0 || BotMarkers.Any(b => ua.Contains(b, StringComparison.OrdinalIgnoreCase));

    static string VisitorHash(HttpContext ctx)
    {
        var ip = ctx.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0].Trim();
        if (ip.Length == 0) ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "";
        var ua = ctx.Request.Headers.UserAgent.ToString();
        var day = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"{ip}|{ua}|{day}|{BootSalt}"));
        return Convert.ToHexString(bytes, 0, 8).ToLowerInvariant();       // 16 hex chars, non-reversible
    }

    static string? Country(HttpContext ctx)
    {
        foreach (var h in new[] { "CF-IPCountry", "X-Vercel-IP-Country", "CloudFront-Viewer-Country", "X-Country-Code" })
            if (ctx.Request.Headers[h].ToString() is { Length: 2 } c && c != "XX") return c.ToUpperInvariant();
        return null;                                                       // no trusted geo source → unknown (never guess)
    }

    static (string device, string browser) Client(string ua)
    {
        var device = ua.Contains("Mobi", StringComparison.OrdinalIgnoreCase) || ua.Contains("Android", StringComparison.OrdinalIgnoreCase) ? "mobile"
                   : ua.Contains("iPad", StringComparison.OrdinalIgnoreCase) || ua.Contains("Tablet", StringComparison.OrdinalIgnoreCase) ? "tablet" : "desktop";
        var browser = ua.Contains("Edg/") ? "Edge" : ua.Contains("OPR/") ? "Opera"
                    : ua.Contains("Chrome/") ? "Chrome" : ua.Contains("Safari/") ? "Safari"
                    : ua.Contains("Firefox/") ? "Firefox" : "other";
        return (device, browser);
    }

    sealed record Attr(string? s, string? m, string? c, string? r, string? l);

    /// <summary>Read the attribution cookie; on a first visit with acquisition signals, set it.</summary>
    static Attr Attribution(HttpContext ctx, string path)
    {
        var raw = ctx.Request.Cookies[AttrCookie];
        if (!string.IsNullOrEmpty(raw))
        {
            try
            {
                var el = JsonDocument.Parse(raw).RootElement;
                string? G(string k) => el.TryGetProperty(k, out var v) ? v.GetString() : null;
                return new Attr(G("s"), G("m"), G("c"), G("r"), G("l"));
            }
            catch { /* corrupt cookie → treat as new visit */ }
        }
        var q = ctx.Request.Query;
        string? Clip(string? v) => string.IsNullOrWhiteSpace(v) ? null : (v.Length > 120 ? v[..120] : v);
        var refHost = Uri.TryCreate(ctx.Request.Headers.Referer.ToString(), UriKind.Absolute, out var u)
            && !u.Host.Equals(ctx.Request.Host.Host, StringComparison.OrdinalIgnoreCase) ? u.Host : null;
        var attr = new Attr(
            Clip(q["utm_source"].ToString()) ?? refHost,
            Clip(q["utm_medium"].ToString()),
            Clip(q["utm_campaign"].ToString()),
            refHost, Clip(path));
        if (attr.s is null && attr.m is null && attr.c is null && attr.r is null) return attr with { l = null }; // direct visit: nothing worth pinning
        try
        {
            ctx.Response.Cookies.Append(AttrCookie,
                JsonSerializer.Serialize(new { attr.s, attr.m, attr.c, attr.r, attr.l }),
                new CookieOptions { Path = "/", MaxAge = TimeSpan.FromDays(90), HttpOnly = true, SameSite = SameSiteMode.Lax, IsEssential = false });
        }
        catch { /* headers already sent → skip */ }
        return attr;
    }

    /// <summary>Record a public page view (called from the page-serving middleware). Never throws.</summary>
    public static void PageView(Db db, HttpContext ctx, string slug)
    {
        try
        {
            var ua = ctx.Request.Headers.UserAgent.ToString();
            if (IsBot(ua)) return;
            var (device, browser) = Client(ua);
            var attr = Attribution(ctx, "/" + slug);
            db.Execute(@"INSERT INTO analytics_events(event,path,visitor,country,device,browser,utm_source,utm_medium,utm_campaign,referrer,landing)
                         VALUES('page_view',?,?,?,?,?,?,?,?,?,?)",
                "/" + slug, VisitorHash(ctx), Country(ctx), device, browser, attr.s, attr.m, attr.c, attr.r, attr.l);
        }
        catch { /* analytics must never break serving */ }
    }

    /// <summary>Record a conversion/business event (registration, login, purchase…). Attribution is
    /// copied from the visitor's cookie; value/currency only for revenue events. Never throws.</summary>
    public static void Track(Db db, HttpContext? ctx, string ev, long? userId = null, double? value = null, string? currency = null, string? detail = null)
    {
        try
        {
            string? visitor = null, country = null, device = null, browser = null;
            Attr attr = new(null, null, null, null, null);
            if (ctx is not null)
            {
                var ua = ctx.Request.Headers.UserAgent.ToString();
                if (IsBot(ua)) return;
                (device, browser) = Client(ua);
                visitor = VisitorHash(ctx);
                country = Country(ctx);
                attr = Attribution(ctx, ctx.Request.Path.Value ?? "");
            }
            db.Execute(@"INSERT INTO analytics_events(event,path,visitor,user_id,country,device,browser,utm_source,utm_medium,utm_campaign,referrer,landing,value,currency,detail)
                         VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                ev, ctx?.Request.Path.Value, visitor, userId, country, device, browser,
                attr.s, attr.m, attr.c, attr.r, attr.l, value, currency, detail);
        }
        catch { /* analytics must never break the caller */ }
    }
}
