using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Database-driven resolution of the public social-media links, shared by the public API
/// (Endpoints/Social.cs) and the server-side footer render (Core/ListSections.cs) so both apply the
/// SAME display rules. An icon is shown only when the master switch is on AND the account is active,
/// approved, in its effective/expiry window, not known-broken, and targets the requested location.
/// Nothing here is hardcoded — every value comes from social_accounts.
/// </summary>
public static class SocialLinks
{
    public sealed record PublicLink(string Platform, string Url, string Label, string Tooltip,
        string Svg, string Rel, string Target, bool Official, string? Handle, string Color);

    static string S(Db db, string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k) ?? "";
    public static bool Enabled(Db db) => S(db, "social_enabled") == "1";
    public static bool AnalyticsEnabled(Db db) => S(db, "social_analytics_enabled") != "0";

    /// <summary>The qualifying, ordered public links for a display location (e.g. "footer").</summary>
    public static List<PublicLink> ForLocation(Db db, string location)
    {
        var list = new List<PublicLink>();
        if (!Enabled(db)) return list;
        List<Dictionary<string, object?>> rows;
        try { rows = db.Query("SELECT * FROM social_accounts WHERE active=1 AND approval_status='approved' ORDER BY display_order, id"); }
        catch { return list; }

        foreach (var r in rows)
        {
            var url = (H.Str(r["url"]) ?? "").Trim();
            if (!(url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))) continue;
            var status = H.Str(r["link_status"]) ?? "";
            if (status is "invalid" or "disabled") continue;                 // never surface a known-bad link

            var eff = H.Str(r["effective_date"]);
            if (!string.IsNullOrEmpty(eff) && !H.IsPast(eff)) continue;       // effective date not reached yet
            if (H.IsPast(H.Str(r["expiry_date"]))) continue;                 // past expiry

            var locs = (H.Str(r["locations"]) ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!locs.Contains(location, StringComparer.OrdinalIgnoreCase)) continue;

            var platform = H.Str(r["platform"]) ?? "custom";
            var plat = SocialIcons.Get(platform);
            var platLabel = plat?.Label ?? platform;
            var handle = H.Str(r["handle"]);
            var openNew = H.B(r["open_new_tab"]);
            var baseLabel = H.Str(r["aria_label"]);
            if (string.IsNullOrWhiteSpace(baseLabel)) baseLabel = H.Str(r["display_name"]);
            if (string.IsNullOrWhiteSpace(baseLabel)) baseLabel = $"Project Controls Institute on {platLabel}";
            var label = openNew ? baseLabel + " (opens in a new tab)" : baseLabel!;
            var tooltip = H.Str(r["tooltip"]);
            if (string.IsNullOrWhiteSpace(tooltip)) tooltip = baseLabel!;

            var rel = new List<string>();
            if (openNew) { rel.Add("noopener"); rel.Add("noreferrer"); }
            if (H.B(r["rel_nofollow"])) rel.Add("nofollow");

            list.Add(new PublicLink(
                platform, url, label!, tooltip!,
                SocialIcons.Svg(platform, H.Str(r["icon_type"]), H.Str(r["custom_icon"])),
                string.Join(" ", rel), openNew ? "_blank" : "",
                H.B(r["is_official"]), handle, plat?.Color ?? "#334155"));
        }
        return list;
    }

    /// <summary>Server-rendered footer social bar + Organization sameAs structured data, or null when
    /// nothing qualifies (so the container is simply absent — never an empty box). Emitted into the
    /// &lt;!--PCI-SOCIAL--&gt; marker by ListSections.</summary>
    public static string? RenderFooter(Db db)
    {
        var links = ForLocation(db, "footer");
        if (links.Count == 0) return null;
        var analytics = AnalyticsEnabled(db);

        var sb = new StringBuilder();
        sb.Append("<div class=\"ft-social\" role=\"list\" aria-label=\"Project Controls Institute on social media\">");
        foreach (var l in links)
        {
            sb.Append("<a class=\"ft-social-lnk\" role=\"listitem\" href=\"").Append(EscAttr(l.Url)).Append('"');
            sb.Append(" aria-label=\"").Append(Esc(l.Label)).Append('"');
            sb.Append(" title=\"").Append(Esc(l.Tooltip)).Append('"');
            if (l.Rel.Length > 0) sb.Append(" rel=\"").Append(l.Rel).Append('"');
            if (l.Target.Length > 0) sb.Append(" target=\"").Append(l.Target).Append('"');
            if (analytics) sb.Append(" data-social=\"").Append(Esc(l.Platform)).Append('"');
            sb.Append('>').Append(l.Svg).Append("</a>");
        }
        sb.Append("</div>");

        // sameAs: only official, active, approved profiles (already filtered above).
        var same = links.Where(l => l.Official).Select(l => l.Url).ToList();
        if (same.Count > 0)
        {
            var baseUrl = S(db, "site_base_url");
            if (string.IsNullOrWhiteSpace(baseUrl)) baseUrl = "https://www.projectcontrolsinstitute.org";
            var json = JsonSerializer.Serialize(new Dictionary<string, object?>
            {
                ["@context"] = "https://schema.org",
                ["@type"] = "Organization",
                ["name"] = "Project Controls Institute",
                ["url"] = baseUrl,
                ["sameAs"] = same,
            });
            sb.Append("<script type=\"application/ld+json\">").Append(json.Replace("<", "\\u003c")).Append("</script>");
        }
        return sb.ToString();
    }

    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
    static string EscAttr(string s)
    {
        var v = Esc(s);
        var probe = new string(v.Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c)).ToArray()).ToLowerInvariant();
        return probe.StartsWith("javascript:") || probe.StartsWith("vbscript:") || probe.StartsWith("data:") ? "#" : v;
    }
}
