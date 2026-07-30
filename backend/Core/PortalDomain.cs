using System.Text.RegularExpressions;

namespace PCI.Backend.Core;

/// <summary>
/// Student-portal domain separation (RES-013): the public marketing site lives on the institute
/// domain (projectcontrolsinstitute.org) while the logged-in student panel lives on its own domain
/// (mypci.org). One deployment still serves both — this class decides, per request and per link,
/// which hostname a URL belongs to.
///
/// Configure with either:
///   PORTAL_BASE_URL = https://mypci.org      (the portal's public origin — preferred)
///   PORTAL_HOSTS    = mypci.org,www.mypci.org (extra hostnames that should serve the portal)
///
/// With neither set the feature is OFF and every link stays relative, which is exactly the
/// single-domain behaviour that shipped before — nothing changes until an operator opts in.
///
/// What separation buys, and why each piece is here:
///   • a student who lands on an /app link from the public site arrives on the portal domain, so
///     session cookies and storage never straddle two origins;
///   • the portal domain is marked noindex, so the logged-in surface cannot be indexed or outrank
///     the marketing pages;
///   • marketing paths requested on the portal domain redirect back to the institute domain (and
///     vice versa for /app), so each page has exactly ONE canonical home rather than two live
///     copies competing in search results.
/// </summary>
public static class PortalDomain
{
    static string? E(string k) => Environment.GetEnvironmentVariable(k) is { Length: > 0 } v ? v.Trim() : null;

    /// <summary>The portal's public origin (scheme + host), or null when separation is off.</summary>
    public static string? BaseUrl
    {
        get
        {
            var raw = E("PORTAL_BASE_URL");
            if (raw is null) return null;
            return Uri.TryCreate(raw, UriKind.Absolute, out var u) ? u.GetLeftPart(UriPartial.Authority) : null;
        }
    }

    /// <summary>The institute (public marketing) origin — the existing APP_BASE_URL/SITE_BASE_URL.</summary>
    public static string? PublicBaseUrl
    {
        get
        {
            var raw = E("APP_BASE_URL") ?? E("SITE_BASE_URL");
            if (raw is null) return null;
            return Uri.TryCreate(raw, UriKind.Absolute, out var u) ? u.GetLeftPart(UriPartial.Authority) : null;
        }
    }

    /// <summary>Hostnames that serve the portal: PORTAL_HOSTS plus the host of PORTAL_BASE_URL.</summary>
    public static HashSet<string> Hosts
    {
        get
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var h in (E("PORTAL_HOSTS") ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                set.Add(h);
            if (E("PORTAL_BASE_URL") is { } raw && Uri.TryCreate(raw, UriKind.Absolute, out var u)) set.Add(u.Host);
            return set;
        }
    }

    /// <summary>Separation is active only when a portal origin is configured AND it differs from the
    /// public one. Pointing both at the same host is a no-op, not a redirect loop.</summary>
    public static bool Enabled
    {
        get
        {
            var portal = BaseUrl;
            if (portal is null) return false;
            var pub = PublicBaseUrl;
            return pub is null || !string.Equals(portal, pub, StringComparison.OrdinalIgnoreCase);
        }
    }

    public static bool IsPortalHost(string? host) => host is not null && Hosts.Contains(host);

    /// <summary>Paths the portal domain owns. Everything else on that host is marketing content and
    /// belongs on the institute domain.</summary>
    public static bool IsPortalPath(PathString path) =>
        path.StartsWithSegments("/app")
        || path.Equals("/student.html", StringComparison.OrdinalIgnoreCase)
        || path.StartsWithSegments("/reset-password.html")
        || path.Equals("/reset-password.html", StringComparison.OrdinalIgnoreCase);

    /// <summary>Paths that must keep working on BOTH domains: the API the portal calls, its bundled
    /// assets, health, and the admin console (moving admin would lock operators out of whichever
    /// host they have bookmarked, which is not this change's job).</summary>
    public static bool IsSharedPath(PathString path) =>
        path.StartsWithSegments("/api")
        || path.StartsWithSegments("/assets")
        || path.StartsWithSegments("/admin")
        || path.StartsWithSegments("/world")
        || path.StartsWithSegments("/world-admin")
        || path.StartsWithSegments("/uploads")
        || path.StartsWithSegments("/images")
        || path.StartsWithSegments("/downloads")
        || path.Equals("/favicon.ico", StringComparison.OrdinalIgnoreCase)
        || path.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase)
        || path.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase);

    /// <summary>An absolute portal URL for <paramref name="path"/>, or the path unchanged when
    /// separation is off (relative links are correct on a single-domain deployment).</summary>
    public static string Url(string path)
    {
        if (!path.StartsWith('/')) path = "/" + path;
        return Enabled ? BaseUrl + path : path;
    }

    /// <summary>An absolute institute URL for <paramref name="path"/>, or the path unchanged.</summary>
    public static string PublicUrl(string path)
    {
        if (!path.StartsWith('/')) path = "/" + path;
        return PublicBaseUrl is { } b && Enabled ? b + path : path;
    }

    // href="/app/…" / href="/student.html" — the portal links the marketing pages carry. Matched on
    // the quote boundaries so "/apply" and "/appeals" cannot be caught by a naive prefix match.
    static readonly Regex PortalHref = new(
        @"(?<attr>\b(?:href|action)\s*=\s*)(?<q>[""'])(?<path>/(?:app(?:/[^""'\s]*)?|student\.html(?:[?#][^""'\s]*)?))\k<q>",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Rewrite portal links in a served marketing page to absolute portal-domain URLs, so a visitor
    /// clicking "Sign in" or "Join" lands on mypci.org directly instead of being redirected a moment
    /// later. A no-op when separation is off.
    /// </summary>
    public static string RewriteLinks(string html)
    {
        if (!Enabled || string.IsNullOrEmpty(html)) return html;
        var b = BaseUrl!;
        return PortalHref.Replace(html, m =>
            $"{m.Groups["attr"].Value}{m.Groups["q"].Value}{b}{m.Groups["path"].Value}{m.Groups["q"].Value}");
    }
}
