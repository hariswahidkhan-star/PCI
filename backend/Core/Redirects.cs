namespace PCI.Backend.Core;

/// <summary>
/// Canonical-domain + HTTPS enforcement for the public website. The approved canonical is
/// https://projectcontrolsinstitute.org (no www). Requests on a KNOWN alternate host — the www
/// variant — are 301-redirected page-to-page to the canonical host; http is upgraded to https on
/// the canonical/alternate hosts. Requests on any OTHER host
/// (the Render URL, localhost, a staging domain) pass through unchanged, so the single service keeps
/// working everywhere during the DNS transition and there is never a redirect loop.
///
/// Config: CANONICAL_HOST (default projectcontrolsinstitute.org), REDIRECT_HOSTS (extra comma-listed
/// hosts to fold in), CANONICAL_REDIRECT=off to disable. Only GET/HEAD are redirected (never a POST
/// webhook), and the target is always the FINAL canonical https URL (one hop, no chains).
/// </summary>
public static class Redirects
{
    public static readonly string CanonicalHost =
        (Environment.GetEnvironmentVariable("CANONICAL_HOST")?.Trim().ToLowerInvariant() is { Length: > 0 } h)
            ? h : "projectcontrolsinstitute.org";

    static readonly bool Enabled =
        !string.Equals(Environment.GetEnvironmentVariable("CANONICAL_REDIRECT"), "off", StringComparison.OrdinalIgnoreCase);

    static readonly HashSet<string> AltHosts = BuildAlts();

    public static string CanonicalBase => "https://" + CanonicalHost;

    static HashSet<string> BuildAlts()
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "www." + CanonicalHost,
        };
        if (Environment.GetEnvironmentVariable("REDIRECT_HOSTS") is { Length: > 0 } extra)
            foreach (var x in extra.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                set.Add(x.ToLowerInvariant());
        set.Remove(CanonicalHost);                 // never redirect the canonical host to itself (loop guard)
        return set;
    }

    /// <summary>The absolute 301 target if this request must be redirected, else null (pass through).</summary>
    public static string? Target(HttpRequest req)
    {
        if (!Enabled) return null;
        if (req.Method != "GET" && req.Method != "HEAD") return null;   // never redirect POST/webhooks
        var host = req.Host.Host.ToLowerInvariant();                    // host without port
        var proto = req.Headers["X-Forwarded-Proto"].ToString().Split(',')[0].Trim();
        var isHttps = proto.Length > 0
            ? string.Equals(proto, "https", StringComparison.OrdinalIgnoreCase)
            : req.IsHttps;

        var isAlt = AltHosts.Contains(host);
        var needsHttpsUpgrade = !isHttps && (isAlt || host == CanonicalHost);
        if (!isAlt && !needsHttpsUpgrade) return null;                  // canonical-on-https, or an unknown host

        return $"{CanonicalBase}{req.Path}{req.QueryString}";           // straight to the final canonical URL
    }

    /// <summary>Private/authenticated paths that must carry X-Robots-Tag: noindex (defence in depth
    /// alongside auth, meta tags and robots.txt).</summary>
    public static bool IsPrivatePath(string path)
    {
        if (string.IsNullOrEmpty(path)) return false;
        var p = path.TrimEnd('/');
        return p.StartsWith("/admin", StringComparison.OrdinalIgnoreCase)
            // The PCI World admin realm — noindex header as defence in depth alongside its meta tag
            // and robots.txt. The public "/world" surfaces are NOT private and must not match here.
            || p.Equals("/world-admin", StringComparison.OrdinalIgnoreCase)
            || p.StartsWith("/world-admin/", StringComparison.OrdinalIgnoreCase)
            || p.StartsWith("/app", StringComparison.OrdinalIgnoreCase)
            || p.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
            || p.Equals("/student.html", StringComparison.OrdinalIgnoreCase)
            || p.Equals("/student-login.html", StringComparison.OrdinalIgnoreCase)
            || p.Equals("/student-dashboard.html", StringComparison.OrdinalIgnoreCase)
            || p.Equals("/admin.html", StringComparison.OrdinalIgnoreCase)
            || p.Equals("/exam-ui.html", StringComparison.OrdinalIgnoreCase);
    }
}
