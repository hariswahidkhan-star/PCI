using System.Text;
using System.Text.RegularExpressions;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;

namespace PCI.Backend.Core;

/// <summary>
/// The public certification catalogue — a single, backend-controlled source of truth for the credentials
/// the Institute offers. Any page that carries the <!--PCI-CERTS--> … <!--/PCI-CERTS--> marker gets that
/// region replaced, server-side, with live cards generated from the `certifications` table. So when the
/// operator adds or edits a certification in the admin console it appears on the public website immediately,
/// is seen by search engines (the HTML is final before it reaches the browser), and works with JavaScript off.
///
/// Nothing about a certification is hardcoded in the HTML: the name, description, price, exam parameters and
/// enrolment link all come from the database. The static marker only carries a neutral fallback that shows
/// if the catalogue is momentarily empty. Generation is cached and only recomputed when a certification
/// actually changes (a version counter the admin cert endpoints bump).
/// </summary>
public static class CertCatalogue
{
    // Pages that carry a catalogue marker. The render path reads only these few files when they have no
    // other content overrides, and the result is cached — so steady-state serving stays cheap.
    static readonly HashSet<string> Pages = new(StringComparer.OrdinalIgnoreCase)
    { "certification.html", "certification-scheme.html", "certifications.html", "index.html", "enrol.html", "enroll.html" };

    public static bool Applies(string slug) => Pages.Contains(slug);

    const string Marker = "<!--PCI-CERTS-->";
    static readonly Regex RxBlock = new(@"<!--PCI-CERTS-->.*?<!--/PCI-CERTS-->", RegexOptions.Singleline);

    static int _version = 1;
    /// <summary>Called by the admin certification create/update/delete endpoints so the public cards refresh.</summary>
    public static void Bump() => Interlocked.Increment(ref _version);

    static int _cardsVer = -1;
    static string _cards = "";
    static readonly object _lock = new();

    /// <summary>Replace every catalogue marker region in the HTML with live cards. A page with no marker is
    /// returned unchanged, so this is safe to call on any page.</summary>
    public static string Inject(Db db, string html)
    {
        if (string.IsNullOrEmpty(html) || !html.Contains(Marker)) return html;
        var cards = Cards(db);
        return RxBlock.Replace(html, Marker + cards + "<!--/PCI-CERTS-->");
    }

    static string Cards(Db db)
    {
        var v = Volatile.Read(ref _version);
        lock (_lock) { if (_cardsVer == v && _cards.Length > 0) return _cards; }

        string html;
        try
        {
            var rows = db.Query("SELECT id,code,name,description,short_description,acronym,status,slug,expiry_years FROM certifications WHERE active=1 ORDER BY sort_order, id");
            var sb = new StringBuilder();
            foreach (var r in rows)
            {
                var id = H.L(r["id"]);
                var cert = Certs.ById(db, id);
                var cfg = Certs.Cfg(db, id);
                var price = cert is null ? 0 : Public.Pricing(db, "exam", null, cert).final;
                var code = H.Str(r["code"]) ?? "";
                var name = H.Str(r["name"]) ?? code;
                // prefer the concise catalogue blurb; fall back to the long description
                var desc = H.Str(r["short_description"]) ?? H.Str(r["description"]) ?? "";
                var years = r["expiry_years"] is null ? 3 : (int)H.L(r["expiry_years"]);
                var status = (H.Str(r["status"]) ?? "Active").Trim();
                var slug = H.Str(r["slug"]) ?? code.ToLowerInvariant();
                // A credential is enrolable only when the operator has opened it. Draft / Under Development /
                // Coming Soon / Suspended / Closed / Archived never expose an Enrol button or a fee.
                var openForEnrol = status is "Active" or "Open for Applications";

                sb.Append("<article class=\"qcard cert-card\">");
                sb.Append("<div class=\"cert-card-top\"><span class=\"cert-card-code\">").Append(Esc(code)).Append("</span>");
                if (!openForEnrol) sb.Append("<span class=\"cert-card-status\">").Append(Esc(status)).Append("</span>");
                sb.Append("</div>");
                sb.Append("<h3 class=\"cert-card-name\">").Append(Esc(name)).Append("</h3>");
                if (desc.Length > 0) sb.Append("<p class=\"cert-card-desc\">").Append(Esc(desc)).Append("</p>");
                sb.Append("<ul class=\"cert-card-meta\">");
                if (openForEnrol)
                {
                    sb.Append("<li><strong>").Append(FmtPrice(price)).Append("</strong> exam fee</li>");
                    sb.Append("<li>").Append((int)cfg.Duration).Append(" minutes · ").Append(FmtPct(cfg.Pass)).Append(" to pass</li>");
                    sb.Append("<li>Valid for ").Append(years).Append(years == 1 ? " year" : " years").Append("</li>");
                }
                else
                {
                    sb.Append("<li>Examination and enrolment opening soon</li>");
                    sb.Append("<li>Register your interest to be notified</li>");
                }
                sb.Append("</ul>");
                var learn = "certifications/" + Uri.EscapeDataString(slug) + ".html";
                if (openForEnrol)
                    // Enrolment happens in the portal: carry the certification code through the free-account
                    // signup so the in-portal purchase prices and books THIS credential.
                    sb.Append("<a class=\"btn btn-red cert-card-cta\" href=\"/app/register?product=exam&amp;cert=")
                      .Append(Uri.EscapeDataString(code)).Append("\">Enrol in ").Append(Esc(code)).Append("</a>");
                else
                    sb.Append("<a class=\"btn btn-ghost cert-card-cta\" href=\"request-info.html?cert=")
                      .Append(Uri.EscapeDataString(code)).Append("\">Register interest</a>");
                sb.Append("</article>");
            }
            html = sb.ToString();
            if (html.Length == 0)
                html = "<p class=\"cert-card-empty\">New certifications will appear here soon. <a href=\"enrol.html\">Enrol</a> or <a href=\"contact.html\">contact us</a>.</p>";
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"[cert-catalogue] render skipped: {e.Message}");
            return "<p class=\"cert-card-empty\"><a href=\"enrol.html\">View enrolment options</a>.</p>";
        }

        lock (_lock) { _cardsVer = v; _cards = html; }
        return html;
    }

    static string FmtPrice(double n) => "USD " + (n == Math.Floor(n) ? ((long)n).ToString() : n.ToString("0.00"));
    static string FmtPct(double n) => (n == Math.Floor(n) ? ((long)n).ToString() : n.ToString("0.#")) + "%";

    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
}
