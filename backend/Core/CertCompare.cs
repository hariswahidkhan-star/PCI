using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The dynamic certification comparison page (/certifications/compare) — a single server-rendered
/// view of the whole PCI AI Project Leadership Certification Suite, generated entirely from the
/// certifications, certification_routes and pricing tables. Nothing here is hardcoded per credential:
/// a certification added or edited in the admin console appears in the comparison automatically.
/// Sections carry stable anchors (#compare, #routes, #fees, #exams) so the suite's auxiliary URLs
/// (/certifications/routes|fees|exams) can deep-link into the one authoritative page.
/// </summary>
public static class CertCompare
{
    static string Esc(string? s) => System.Net.WebUtility.HtmlEncode(s ?? "");

    public static string? Render(Db db, string webRoot, string lang)
    {
        var tplPath = Path.Combine(webRoot, "certification-detail.html");
        if (!File.Exists(tplPath)) return null;
        var html = File.ReadAllText(tplPath)
            .Replace("<!--PCI-CERT-DETAIL-->", BuildBody(db))
            .Replace("{{TITLE}}", "Compare PCI Certifications | " + MultiCert.PortfolioName)
            .Replace("{{DESC}}", "Side-by-side comparison of the PCI AI Project Leadership Certification Suite — PCI PCL-AI, PCI PFL-AI and PCI PDL-AI: audience, competencies, routes, fees and examinations.")
            .Replace("{{CANONICAL}}", "/certifications/compare");
        html = ListSections.Inject(db, html, lang);
        html = SeoTags.Inject(db, html, "certifications/compare.html");
        return html;
    }

    static List<Dictionary<string, object?>> ActiveCerts(Db db) =>
        db.Query("SELECT * FROM certifications WHERE active=1 ORDER BY sort_order, id");

    static string BuildBody(Db db)
    {
        var certs = ActiveCerts(db);
        var sb = new System.Text.StringBuilder();

        sb.Append("<section class=\"phead\"><div class=\"wrap\">");
        sb.Append($"<span class=\"eyebrow\">{Esc(MultiCert.PortfolioName)}</span>");
        sb.Append("<h1>Compare the PCI certifications.</h1>");
        sb.Append($"<p class=\"lead\">{Esc(MultiCert.PortfolioTagline)} Choose the credential that matches your role — or combine them.</p>");
        sb.Append("</div></section>");

        // ── #compare: the side-by-side table ──
        sb.Append("<section class=\"sec\" id=\"compare\"><div class=\"wrap\">");
        sb.Append("<span class=\"eyebrow\">Side by side</span><h2>What each credential is for</h2><div class=\"uline\"></div>");
        sb.Append("<div style=\"overflow-x:auto\"><table class=\"cmp-table\" style=\"width:100%;border-collapse:collapse;font-size:14px\">");
        sb.Append("<thead><tr><th style=\"text-align:left;padding:12px 14px;border-bottom:2px solid var(--line)\"></th>");
        foreach (var c in certs)
            sb.Append($"<th style=\"text-align:left;padding:12px 14px;border-bottom:2px solid var(--line)\"><div style=\"font-family:var(--display);font-weight:800;font-size:16px\">{Esc(H.Str(c["acronym"]) ?? H.Str(c["code"]))}</div><div style=\"font-weight:600;color:var(--slate)\">{Esc(H.Str(c["public_title"]) ?? H.Str(c["name"]))}</div></th>");
        sb.Append("</tr></thead><tbody>");

        void Row(string label, Func<Dictionary<string, object?>, string> cell)
        {
            sb.Append($"<tr><td style=\"padding:11px 14px;border-bottom:1px solid var(--line);font-weight:700;white-space:nowrap\">{Esc(label)}</td>");
            foreach (var c in certs)
                sb.Append($"<td style=\"padding:11px 14px;border-bottom:1px solid var(--line);vertical-align:top\">{cell(c)}</td>");
            sb.Append("</tr>");
        }

        string[] Comps(Dictionary<string, object?> c)
        {
            try
            {
                var doc = System.Text.Json.JsonDocument.Parse(H.Str(c["content_json"]) ?? "{}");
                if (doc.RootElement.TryGetProperty("competencies", out var arr))
                    return arr.EnumerateArray().Select(e => e.GetString() ?? "").Where(s => s.Length > 0).ToArray();
            }
            catch { }
            return Array.Empty<string>();
        }

        Row("Designation", c => $"<strong>{Esc(H.Str(c["acronym"]) ?? H.Str(c["code"]))}</strong>");
        Row("Discipline", c => Esc(H.Str(c["category"]) ?? "—"));
        Row("Purpose", c => Esc(H.Str(c["short_description"]) ?? ""));
        Row("Intended audience", c => Esc(H.Str(c["audience"]) ?? "—"));
        Row("Competency areas", c =>
        {
            var comps = Comps(c);
            if (comps.Length == 0) return "—";
            var shown = comps.Take(6).Select(Esc);
            var more = comps.Length > 6 ? $"<div style=\"color:var(--mist);font-size:12.5px;margin-top:4px\">+ {comps.Length - 6} more on the certification page</div>" : "";
            return "<ul style=\"margin:0;padding-left:18px\">" + string.Join("", shown.Select(s => $"<li style=\"margin:2px 0\">{s}</li>")) + "</ul>" + more;
        });
        Row("Status", c => $"<span class=\"cert-status-pill\">{Esc(H.Str(c["status"]) ?? "Active")}</span>");
        sb.Append("</tbody></table></div></div></section>");

        // ── #routes: availability matrix straight from certification_routes ──
        sb.Append("<section class=\"sec\" id=\"routes\" style=\"background:var(--paper,#FAFBFD)\"><div class=\"wrap\">");
        sb.Append("<span class=\"eyebrow\">Application routes</span><h2>Every route, for every certification</h2><div class=\"uline\"></div>");
        sb.Append("<p>Each certification carries its own independently configured routes. A tick means the route is currently open for that credential.</p>");
        var routeKeys = db.Query(@"SELECT route_key, MAX(label) label, MIN(sort_order) so FROM certification_routes
            WHERE enabled=1 AND public=1 GROUP BY route_key ORDER BY so");
        sb.Append("<div style=\"overflow-x:auto\"><table style=\"width:100%;border-collapse:collapse;font-size:14px\">");
        sb.Append("<thead><tr><th style=\"text-align:left;padding:10px 14px;border-bottom:2px solid var(--line)\">Route</th>");
        foreach (var c in certs) sb.Append($"<th style=\"text-align:center;padding:10px 14px;border-bottom:2px solid var(--line)\">{Esc(H.Str(c["acronym"]) ?? H.Str(c["code"]))}</th>");
        sb.Append("</tr></thead><tbody>");
        foreach (var r in routeKeys)
        {
            sb.Append($"<tr><td style=\"padding:10px 14px;border-bottom:1px solid var(--line);font-weight:600\">{Esc(H.Str(r["label"]))}</td>");
            foreach (var c in certs)
            {
                var open = db.Scalar<long>("SELECT COUNT(*) FROM certification_routes WHERE certification_id=? AND route_key=? AND enabled=1 AND public=1", c["id"], r["route_key"]) > 0;
                sb.Append($"<td style=\"text-align:center;padding:10px 14px;border-bottom:1px solid var(--line)\">{(open ? "<span style=\"color:#1a7d3c;font-weight:700\">✓</span>" : "<span style=\"color:var(--mist)\">—</span>")}</td>");
            }
            sb.Append("</tr>");
        }
        sb.Append("</tbody></table></div>");
        sb.Append("<p class=\"small\" style=\"color:var(--slate)\">Route details — eligibility, documents, fees and approval — are on each certification page: "
            + string.Join(" · ", certs.Select(c => $"<a href=\"/certifications/{Esc(H.Str(c["slug"]))}\">{Esc(H.Str(c["acronym"]) ?? H.Str(c["code"]))} routes</a>")) + ".</p>");
        sb.Append("</div></section>");

        // ── #fees: live pricing per certification ──
        sb.Append("<section class=\"sec\" id=\"fees\"><div class=\"wrap\">");
        sb.Append("<span class=\"eyebrow\">Fees</span><h2>Current fees by certification</h2><div class=\"uline\"></div>");
        sb.Append("<div style=\"overflow-x:auto\"><table style=\"width:100%;border-collapse:collapse;font-size:14px\">");
        sb.Append("<thead><tr><th style=\"text-align:left;padding:10px 14px;border-bottom:2px solid var(--line)\">Fee</th>");
        foreach (var c in certs) sb.Append($"<th style=\"text-align:left;padding:10px 14px;border-bottom:2px solid var(--line)\">{Esc(H.Str(c["acronym"]) ?? H.Str(c["code"]))}</th>");
        sb.Append("</tr></thead><tbody>");
        Row("Examination fee", c =>
        {
            var pr = Endpoints.Public.Pricing(db, "exam", null, c);
            return $"<strong>USD {pr.final:0}</strong>" + (pr.defaultDiscount > 0 ? $" <span style=\"color:var(--mist)\">(USD {pr.standard:0} less current discount)</span>" : "");
        });
        Row("Application fee", c => c["application_fee"] is null ? "Included" : $"USD {H.D(c["application_fee"]):0}");
        Row("Validity", c => H.D(c["expiry_years"]) is var y && y > 0 ? $"{y:0} years, then recertification" : "—");
        sb.Append("</tbody></table></div>");
        sb.Append("<p class=\"small\" style=\"color:var(--slate)\">Fees are live values from PCI's pricing configuration. Discount codes, waivers and sponsorships are applied at checkout; membership pricing is on the <a href=\"membership.html\">membership page</a>.</p>");
        sb.Append("</div></section>");

        // ── #exams: examination facts per certification ──
        sb.Append("<section class=\"sec\" id=\"exams\" style=\"background:var(--paper,#FAFBFD)\"><div class=\"wrap\">");
        sb.Append("<span class=\"eyebrow\">Examinations</span><h2>Examination at a glance</h2><div class=\"uline\"></div>");
        sb.Append("<div style=\"overflow-x:auto\"><table style=\"width:100%;border-collapse:collapse;font-size:14px\">");
        sb.Append("<thead><tr><th style=\"text-align:left;padding:10px 14px;border-bottom:2px solid var(--line)\"></th>");
        foreach (var c in certs) sb.Append($"<th style=\"text-align:left;padding:10px 14px;border-bottom:2px solid var(--line)\">{Esc(H.Str(c["acronym"]) ?? H.Str(c["code"]))}</th>");
        sb.Append("</tr></thead><tbody>");
        Row("Duration", c => { var cfg = Certs.Cfg(db, H.L(c["id"])); return $"{cfg.Duration:0} minutes"; });
        Row("Pass mark", c => { var cfg = Certs.Cfg(db, H.L(c["id"])); return $"{cfg.Pass:0}%"; });
        Row("Delivery", _ => "Online, remotely proctored");
        Row("Question style", _ => "Scenario-based, mapped to the certification's Body of Knowledge");
        sb.Append("</tbody></table></div>");
        sb.Append("<p class=\"small\" style=\"color:var(--slate)\">Exam rules, scheduling and conduct are in the <a href=\"exam-structure.html\">exam structure</a> and <a href=\"exam-rules.html\">examination rules</a>.</p>");
        sb.Append("</div></section>");

        // ── closing CTA cards from the live catalogue ──
        sb.Append("<section class=\"sec\"><div class=\"wrap\">");
        sb.Append("<span class=\"eyebrow\">Ready?</span><h2>Choose your certification</h2><div class=\"uline\"></div>");
        sb.Append("<div class=\"qcards cert-catalogue\"><!--PCI-CERTS--><!--/PCI-CERTS--></div>");
        sb.Append("</div></section>");

        var body = sb.ToString();
        // The closing cards reuse the live catalogue injection.
        return CertCatalogue.Inject(db, body);
    }
}
