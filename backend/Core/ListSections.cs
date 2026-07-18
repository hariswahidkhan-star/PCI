using System.Text;
using System.Text.RegularExpressions;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Server-rendered, database-backed sections for the public website — the same pattern as the
/// certification catalogue, generalised. A page region wrapped in <!--PCI-X--> … <!--/PCI-X-->
/// markers is replaced at serve time with markup generated from its admin-managed table:
///
///   PCI-NAV-HEADER   header navigation links          ← nav_items (Header / Header account)
///   PCI-NAV-FOOTER   footer link columns (all pages)  ← nav_items (every other group)
///   PCI-FAQS         FAQ accordion                    ← faqs
///   PCI-BOK          Body-of-Knowledge domain cards   ← bok_domains
///   PCI-GOVERNANCE   governance role cards            ← governance_roles
///   PCI-RESOURCES    policy / document link groups    ← resources
///   PCI-NEWS         news & updates section           ← news
///
/// So adding a footer link, an FAQ, a news item or a BoK domain in the admin console changes the
/// public website immediately — server-side, SEO-visible, no JavaScript required. The static HTML
/// between the markers is only a fallback shown if a table is empty. Rendering is cached per
/// version; any admin edit to these tables bumps the version.
/// </summary>
public static class ListSections
{
    static int _version = 1;
    public static void Bump() => Interlocked.Increment(ref _version);

    static int _cacheVer = -1;
    static readonly Dictionary<string, string> _cache = new(StringComparer.Ordinal);
    static readonly object _lock = new();

    static readonly Regex RxMarker = new(@"<!--PCI-(NAV-HEADER|NAV-FOOTER|FAQS|BOK|GOVERNANCE|RESOURCES|NEWS|PARTNERS)-->.*?<!--/PCI-\1-->",
        RegexOptions.Singleline | RegexOptions.Compiled);

    /// <summary>Replace every known marker region with its table-rendered markup. A section whose
    /// table has no rows keeps the static fallback between the markers. Safe on any page. The header
    /// and footer navigation are rendered in the active <paramref name="lang"/> (labels translated,
    /// language switcher appended); other sections are language-independent.</summary>
    public static string Inject(Db db, string html, string lang = "en")
    {
        if (string.IsNullOrEmpty(html) || !html.Contains("<!--PCI-", StringComparison.Ordinal)) return html;
        return RxMarker.Replace(html, m =>
        {
            var name = m.Groups[1].Value;
            var body = Section(db, name, lang);
            return body is null ? m.Value : $"<!--PCI-{name}-->{body}<!--/PCI-{name}-->";
        });
    }

    static string? Section(Db db, string name, string lang)
    {
        var v = Volatile.Read(ref _version);
        // Only the navigation sections vary by language; cache those per (lang, name), the rest per name.
        var cacheKey = name is "NAV-HEADER" or "NAV-FOOTER" ? lang + "\n" + name : name;
        lock (_lock)
        {
            if (_cacheVer != v) { _cache.Clear(); _cacheVer = v; }
            if (_cache.TryGetValue(cacheKey, out var hit)) return hit.Length == 0 ? null : hit;
        }
        string? html;
        try
        {
            html = name switch
            {
                "NAV-HEADER" => NavHeader(db, lang),
                "NAV-FOOTER" => NavFooter(db, lang),
                "FAQS" => Faqs(db),
                "BOK" => Bok(db),
                "GOVERNANCE" => Governance(db),
                "RESOURCES" => Resources(db),
                "NEWS" => News(db),
                "PARTNERS" => Partners(db),
                _ => null,
            };
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"[sections] {name} render skipped: {e.Message}");
            html = null;                                                   // keep the static fallback
        }
        lock (_lock) { _cache[cacheKey] = html ?? ""; }
        return html;
    }

    // ---- navigation ----

    static string? NavHeader(Db db, string lang)
    {
        var main = db.Query("SELECT label,url FROM nav_items WHERE visible=1 AND nav_group='Header' ORDER BY sort_order,id");
        if (main.Count == 0) return null;
        var acct = db.Query("SELECT label,url FROM nav_items WHERE visible=1 AND nav_group='Header account' ORDER BY sort_order,id");
        var sb = new StringBuilder();
        foreach (var r in main)
        {
            var label = H.Str(r["label"]) ?? "";
            var url = H.Str(r["url"]) ?? "#";
            var tlabel = I18nContent.NavLabel(db, lang, label);
            // A top-level item whose label names a nav_group gets a dropdown of that group's pages
            // (e.g. Membership → Standard/Founding/Honorary routes + tiers). CSS reveals it on hover /
            // focus on desktop and expanded within the mobile menu; the top item itself stays a link.
            var subs = string.IsNullOrEmpty(label) ? new List<Dictionary<string, object?>>()
                : db.Query("SELECT label,url FROM nav_items WHERE visible=1 AND nav_group=? ORDER BY sort_order,id", label);
            if (subs.Count > 0)
            {
                // Desktop: hover reveals the submenu. Mobile: the caret button toggles it (accordion) —
                // premium.js flips 'open' on .nav-item.has-sub; the parent label stays a real link.
                sb.Append("<span class=\"nav-item has-sub\"><a href=\"").Append(EscAttr(url)).Append("\">").Append(Esc(tlabel))
                  .Append("</a><button type=\"button\" class=\"sub-toggle\" aria-expanded=\"false\" aria-label=\"Show ").Append(EscAttr(tlabel))
                  .Append(" menu\"></button><span class=\"submenu\">");
                foreach (var s in subs)
                {
                    var sl = H.Str(s["label"]) ?? "";
                    sb.Append("<a href=\"").Append(EscAttr(H.Str(s["url"]) ?? "#")).Append("\">").Append(Esc(I18nContent.NavLabel(db, lang, sl))).Append("</a>");
                }
                sb.Append("</span></span>");
            }
            else
            {
                sb.Append("<a href=\"").Append(EscAttr(url)).Append("\">").Append(Esc(tlabel)).Append("</a>");
            }
        }
        foreach (var r in acct)
        {
            var al = H.Str(r["label"]) ?? "";
            sb.Append("<a class=\"nav-acct\" href=\"").Append(EscAttr(H.Str(r["url"]) ?? "#")).Append("\">").Append(Esc(I18nContent.NavLabel(db, lang, al))).Append("</a>");
        }
        // NOTE: the language switcher no longer renders here — it lives on the top promo bar,
        // injected client-side by assets/premium.js (PCI-LANGBAR block).
        return sb.ToString();
    }

    static string? NavFooter(Db db, string lang)
    {
        var rows = db.Query(@"SELECT label,url,nav_group FROM nav_items
            WHERE visible=1 AND nav_group NOT IN ('Header','Header account')
            ORDER BY sort_order,id");
        if (rows.Count == 0) return null;
        var sb = new StringBuilder();
        string? current = null;
        foreach (var r in rows)
        {
            var g = H.Str(r["nav_group"]) ?? "More";
            if (g != current)
            {
                if (current is not null) sb.Append("</ul></div>");
                sb.Append("<div><h4>").Append(Esc(I18nContent.NavLabel(db, lang, g))).Append("</h4><ul>");
                current = g;
            }
            var fl = H.Str(r["label"]) ?? "";
            sb.Append("<li><a href=\"").Append(EscAttr(H.Str(r["url"]) ?? "#")).Append("\">")
              .Append(Esc(I18nContent.NavLabel(db, lang, fl))).Append("</a></li>");
        }
        sb.Append("</ul></div>");
        return sb.ToString();
    }

    // ---- content lists ----

    static string? Faqs(Db db)
    {
        var rows = db.Query("SELECT question,answer,category FROM faqs WHERE published=1 ORDER BY sort_order,id");
        if (rows.Count == 0) return null;
        var sb = new StringBuilder();
        string? current = null;
        foreach (var r in rows)
        {
            var cat = H.Str(r["category"]) is { Length: > 0 } c ? c : "General";
            if (cat != current)
            {
                if (current is not null) sb.Append("</div>");
                sb.Append("<div class=\"faqgroup\"><span class=\"eyebrow\">").Append(Esc(cat)).Append("</span>");
                current = cat;
            }
            var answer = H.Str(r["answer"]) ?? "";
            var body = answer.Contains('<') ? HtmlSanitize.Clean(answer) : "<p>" + Esc(answer) + "</p>";
            sb.Append("<details class=\"faq\"><summary><span>").Append(Esc(H.Str(r["question"]) ?? ""))
              .Append("</span><span class=\"faq-ic\"></span></summary><div class=\"faq-a\">").Append(body).Append("</div></details>");
        }
        sb.Append("</div>");
        return sb.ToString();
    }

    static string? Bok(Db db)
    {
        var rows = db.Query("SELECT code,name,weight,description,bullets FROM bok_domains ORDER BY sort_order,id");
        if (rows.Count == 0) return null;
        var sb = new StringBuilder();
        foreach (var r in rows)
        {
            // A row without a code is a group header (weight = the group's exam percentage);
            // a row with a code (D1..D13) is a domain card. Matches the shipped page design.
            if (string.IsNullOrEmpty(H.Str(r["code"])))
            {
                sb.Append("<div style=\"grid-column:1/-1\"><h3 style=\"margin:6px 0 2px\">").Append(Esc(H.Str(r["name"]) ?? ""));
                if (r["weight"] is not null && H.L(r["weight"]) > 0)
                    sb.Append(" — <span style=\"color:var(--crimson)\">").Append(H.L(r["weight"])).Append("&nbsp;%</span>");
                sb.Append("</h3>");
                if (H.Str(r["description"]) is { Length: > 0 } hd)
                    sb.Append("<p class=\"muted\" style=\"margin:0 0 8px\">").Append(Esc(hd)).Append("</p>");
                sb.Append("</div>");
                continue;
            }
            sb.Append("<div class=\"gcard\">");
            sb.Append("<span class=\"gtag wt\">").Append(Esc(H.Str(r["code"])!)).Append("</span>");
            sb.Append("<h4>").Append(Esc(H.Str(r["name"]) ?? "")).Append("</h4>");
            if (H.Str(r["description"]) is { Length: > 0 } d) sb.Append("<p>").Append(Esc(d)).Append("</p>");
            var bullets = (H.Str(r["bullets"]) ?? "").Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (bullets.Length > 0)
            {
                sb.Append("<ul>");
                foreach (var b in bullets) sb.Append("<li>").Append(Esc(b)).Append("</li>");
                sb.Append("</ul>");
            }
            sb.Append("</div>");
        }
        return sb.ToString();
    }

    static string? Governance(Db db)
    {
        var rows = db.Query("SELECT role,holder,status,remit FROM governance_roles ORDER BY sort_order,id");
        if (rows.Count == 0) return null;
        var sb = new StringBuilder();
        foreach (var r in rows)
        {
            var holder = H.Str(r["holder"]);
            var status = H.Str(r["status"]) ?? "open";
            var tag = holder is { Length: > 0 } ? holder : status == "open" ? "Appointment in progress" : status;
            var cls = holder is { Length: > 0 } ? "wt" : "dev";
            var remit = H.Str(r["remit"]) ?? "";
            var body = remit.Contains('<') ? HtmlSanitize.Clean(remit) : Esc(remit);
            sb.Append("<div class=\"gcard\"><span class=\"gtag ").Append(cls).Append("\">").Append(Esc(tag))
              .Append("</span><h4>").Append(Esc(H.Str(r["role"]) ?? "")).Append("</h4><p>").Append(body).Append("</p></div>");
        }
        return sb.ToString();
    }

    static string? Resources(Db db)
    {
        var rows = db.Query("SELECT title,category,url,description FROM resources WHERE published=1 ORDER BY sort_order,id");
        if (rows.Count == 0) return null;
        var sb = new StringBuilder();
        string? current = null;
        foreach (var r in rows)
        {
            var cat = H.Str(r["category"]) is { Length: > 0 } c ? c : "Documents";
            if (cat != current)
            {
                if (current is not null) sb.Append("</ul></div>");
                sb.Append("<div class=\"dlg\"><h3>").Append(Esc(cat)).Append("</h3><ul>");
                current = cat;
            }
            sb.Append("<li><a href=\"").Append(EscAttr(H.Str(r["url"]) ?? "#")).Append("\">").Append(Esc(H.Str(r["title"]) ?? "")).Append("</a>");
            if (H.Str(r["description"]) is { Length: > 0 } d) sb.Append("<span>").Append(Esc(d)).Append("</span>");
            sb.Append("</li>");
        }
        sb.Append("</ul></div>");
        return sb.ToString();
    }

    static string? News(Db db)
    {
        var rows = db.Query("SELECT title,body,published_date,url FROM news WHERE published=1 ORDER BY COALESCE(published_date,'') DESC, sort_order, id DESC LIMIT 12");
        if (rows.Count == 0) return null;
        var sb = new StringBuilder();
        sb.Append("<section class=\"sec\"><div class=\"wrap\"><div class=\"hblock\"><span class=\"eyebrow\">News</span><h2>Latest updates</h2><div class=\"uline\"></div></div><div class=\"gx c3\">");
        foreach (var r in rows)
        {
            var title = Esc(H.Str(r["title"]) ?? "");
            var url = H.Str(r["url"]);
            sb.Append("<div class=\"gcard\">");
            if (H.Str(r["published_date"]) is { Length: > 0 } d) sb.Append("<span class=\"gtag dev\">").Append(Esc(d)).Append("</span>");
            sb.Append("<h4>").Append(url is { Length: > 0 } ? $"<a href=\"{EscAttr(url)}\">{title}</a>" : title).Append("</h4>");
            if (H.Str(r["body"]) is { Length: > 0 } b)
            {
                var text = Regex.Replace(b, "<[^>]+>", " ");
                if (text.Length > 220) text = text[..219].TrimEnd() + "…";
                sb.Append("<p>").Append(Esc(text)).Append("</p>");
            }
            sb.Append("</div>");
        }
        sb.Append("</div></div></section>");
        return sb.ToString();
    }

    // ---- training partner directory (Phase 7) ----
    // Published (listed) partners grouped by tier, most prestigious first. Empty ⇒ keep the static
    // fallback between the markers (e.g. "no partners yet — apply to become one").
    static readonly (string tier, string label)[] PartnerTiers =
    { ("premier", "Premier Training Partners"), ("authorized", "Authorized Training Partners"), ("registered", "Registered Training Partners") };

    static string? Partners(Db db)
    {
        var rows = db.Query("SELECT name,tier,country,region,city,website,summary,specialties FROM training_partners WHERE listed=1 ORDER BY sort_order, name");
        if (rows.Count == 0) return null;
        var sb = new StringBuilder();
        foreach (var (tier, label) in PartnerTiers)
        {
            var inTier = rows.Where(r => (H.Str(r["tier"]) ?? "registered") == tier).ToList();
            if (inTier.Count == 0) continue;
            sb.Append("<div class=\"hblock\"><h3>").Append(Esc(label)).Append("</h3><div class=\"uline\"></div></div><div class=\"gx c3\">");
            foreach (var r in inTier)
            {
                var name = Esc(H.Str(r["name"]) ?? "");
                var loc = string.Join(", ", new[] { H.Str(r["city"]), H.Str(r["region"]), H.Str(r["country"]) }
                    .Where(x => !string.IsNullOrWhiteSpace(x)));
                var website = H.Str(r["website"]);
                sb.Append("<div class=\"gcard\"><span class=\"gtag wt\">").Append(Esc(TierLabel(tier))).Append("</span>");
                sb.Append("<h4>").Append(website is { Length: > 0 } ? $"<a href=\"{EscAttr(website)}\" rel=\"nofollow noreferrer\" target=\"_blank\">{name}</a>" : name).Append("</h4>");
                if (loc.Length > 0) sb.Append("<p class=\"muted small\">").Append(Esc(loc)).Append("</p>");
                if (H.Str(r["summary"]) is { Length: > 0 } s) sb.Append("<p>").Append(Esc(s)).Append("</p>");
                if (H.Str(r["specialties"]) is { Length: > 0 } sp)
                {
                    var tags = sp.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Take(6);
                    sb.Append("<ul>");
                    foreach (var t in tags) sb.Append("<li>").Append(Esc(t)).Append("</li>");
                    sb.Append("</ul>");
                }
                sb.Append("</div>");
            }
            sb.Append("</div>");
        }
        return sb.ToString();
    }

    static string TierLabel(string tier) => tier switch { "premier" => "Premier", "authorized" => "Authorized", _ => "Registered" };

    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    static string EscAttr(string s)
    {
        var v = Esc(s).Replace("\"", "&quot;");
        var probe = new string(v.Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c)).ToArray()).ToLowerInvariant();
        return probe.StartsWith("javascript:") || probe.StartsWith("vbscript:") || probe.StartsWith("data:") ? "#" : v;
    }
}
