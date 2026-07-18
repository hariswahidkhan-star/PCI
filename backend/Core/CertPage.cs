using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Server-rendered per-certification landing page at /certifications/{slug}. The page chrome comes from
/// the tokenised template (certification-detail.html); everything specific to the credential — title,
/// meta, overview, audience, competency areas, exam/fee summary, routes and FAQs — is generated from the
/// certifications row (columns + content_json). Adding a certification in the admin console therefore
/// publishes a full public page automatically, with no new source files.
/// </summary>
public static class CertPage
{
    public static Dictionary<string, object?>? BySlug(Db db, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return null;
        return db.QueryOne("SELECT * FROM certifications WHERE lower(slug)=? AND active=1", slug.Trim().ToLowerInvariant());
    }

    public static (string title, string desc, string canonical) Meta(Dictionary<string, object?> c)
    {
        var slug = H.Str(c["slug"]) ?? (H.Str(c["code"]) ?? "").ToLowerInvariant();
        var title = H.Str(c["meta_title"]) ?? ((H.Str(c["public_title"]) ?? H.Str(c["name"])) + " | Project Controls Institute Global");
        var desc = H.Str(c["meta_description"]) ?? H.Str(c["short_description"]) ?? H.Str(c["description"]) ?? "";
        var canonical = "https://projectcontrolsinstitute.org/certifications/" + slug;
        return (title!, desc!, canonical);
    }

    /// <summary>Render the full page HTML, or null when no active certification matches the slug.</summary>
    public static string? Render(Db db, string webRoot, string slug, string lang)
    {
        var cert = BySlug(db, slug);
        if (cert is null) return null;
        var tplPath = Path.Combine(webRoot, "certification-detail.html");
        if (!File.Exists(tplPath)) return null;
        var (title, desc, canonical) = Meta(cert);
        var html = File.ReadAllText(tplPath)
            .Replace("<!--PCI-CERT-DETAIL-->", BuildBody(db, cert))
            .Replace("{{TITLE}}", Esc(title))
            .Replace("{{DESC}}", Esc(desc))
            .Replace("{{CANONICAL}}", Esc(canonical));
        // live header/footer navigation from nav_items, then admin SEO/analytics tags
        html = ListSections.Inject(db, html, lang);
        html = SeoTags.Inject(db, html, "certifications/" + slug + ".html");
        return html;
    }

    static string BuildBody(Db db, Dictionary<string, object?> c)
    {
        var code = H.Str(c["code"]) ?? "";
        var acronym = H.Str(c["acronym"]) ?? code;
        var name = H.Str(c["public_title"]) ?? H.Str(c["name"]) ?? code;
        var tagline = H.Str(c["tagline"]) ?? "";
        var shortDesc = H.Str(c["short_description"]) ?? "";
        var overview = H.Str(c["description"]) ?? "";
        var category = H.Str(c["category"]) ?? "";
        var level = H.Str(c["level"]) ?? "Professional";
        var status = (H.Str(c["status"]) ?? "Active").Trim();
        var audience = H.Str(c["audience"]) ?? "";
        var slug = H.Str(c["slug"]) ?? code.ToLowerInvariant();
        var open = status is "Active" or "Open for Applications";
        var id = H.L(c["id"]);

        // content_json → competencies + faqs
        List<string> comps = new(); List<(string q, string a)> faqs = new();
        try
        {
            if (H.Str(c["content_json"]) is { Length: > 0 } cj)
            {
                using var doc = JsonDocument.Parse(cj);
                if (doc.RootElement.TryGetProperty("competencies", out var ca) && ca.ValueKind == JsonValueKind.Array)
                    foreach (var e in ca.EnumerateArray()) if (e.GetString() is { } s) comps.Add(s);
                if (doc.RootElement.TryGetProperty("faqs", out var fa) && fa.ValueKind == JsonValueKind.Array)
                    foreach (var e in fa.EnumerateArray())
                        faqs.Add((e.TryGetProperty("q", out var q) ? q.GetString() ?? "" : "",
                                  e.TryGetProperty("a", out var a) ? a.GetString() ?? "" : ""));
            }
        }
        catch { /* malformed content_json → sections simply omit */ }

        var cfg = Certs.Cfg(db, id);
        var cert = Certs.ById(db, id);
        var price = cert is null ? 0 : Endpoints.Public.Pricing(db, "exam", null, cert).final;

        var sb = new StringBuilder();

        // ── hero / page head ──
        sb.Append("<section class=\"phead\"><div class=\"wrap\">");
        sb.Append("<div class=\"crumbbar-inline\"><a href=\"/certifications\">Certifications</a> · ").Append(Esc(acronym)).Append("</div>");
        sb.Append("<span class=\"eyebrow\">").Append(Esc(category)).Append(category.Length > 0 && level.Length > 0 ? " · " : "").Append(Esc(level)).Append("</span>");
        if (!open) sb.Append("<span class=\"cert-status-pill\">").Append(Esc(status)).Append("</span>");
        sb.Append("<h1>").Append(Esc(name)).Append("</h1>");
        if (tagline.Length > 0) sb.Append("<p class=\"phead-lead\">").Append(Esc(tagline)).Append("</p>");
        if (shortDesc.Length > 0) sb.Append("<p class=\"phead-sub\">").Append(Esc(shortDesc)).Append("</p>");
        sb.Append("<div class=\"phead-cta\">");
        if (open)
        {
            sb.Append("<a class=\"btn btn-red\" href=\"/app/register?product=exam&amp;cert=").Append(Uri.EscapeDataString(code)).Append("\">Apply now</a>");
            sb.Append("<a class=\"btn btn-ghost\" href=\"/verify-certificate\">Verify a certificate</a>");
        }
        else
        {
            sb.Append("<a class=\"btn btn-red\" href=\"request-info.html?cert=").Append(Uri.EscapeDataString(code)).Append("\">Register interest</a>");
            sb.Append("<a class=\"btn btn-ghost\" href=\"/certifications\">All certifications</a>");
        }
        sb.Append("</div></div></section>");

        // ── overview ──
        if (overview.Length > 0)
            sb.Append("<section class=\"sec\"><div class=\"wrap\"><span class=\"eyebrow\">Overview</span><div class=\"uline\"></div>")
              .Append("<p class=\"lead\">").Append(Esc(overview)).Append("</p></div></section>");

        // ── who it's for ──
        if (audience.Length > 0)
            sb.Append("<section class=\"sec sec-alt\"><div class=\"wrap\"><span class=\"eyebrow\">Who it's for</span><div class=\"uline\"></div>")
              .Append("<p class=\"lead\">").Append(Esc(audience)).Append("</p></div></section>");

        // ── competency areas ──
        if (comps.Count > 0)
        {
            sb.Append("<section class=\"sec\"><div class=\"wrap\"><span class=\"eyebrow\">What you'll master</span><div class=\"uline\"></div>");
            sb.Append("<div class=\"komp-grid\">");
            int i = 1;
            foreach (var k in comps)
            {
                sb.Append("<div class=\"komp\"><span class=\"komp-n\">").Append(i.ToString("00")).Append("</span><span class=\"komp-l\">").Append(Esc(k)).Append("</span></div>");
                i++;
            }
            sb.Append("</div></div></section>");
        }

        // ── exam & fee summary (honest per status) ──
        sb.Append("<section class=\"sec sec-alt\"><div class=\"wrap\"><span class=\"eyebrow\">Examination & fees</span><div class=\"uline\"></div>");
        sb.Append("<div class=\"cert-facts\">");
        if (open)
        {
            sb.Append(Fact("Examination fee", FmtPrice(price)));
            sb.Append(Fact("Exam duration", ((int)cfg.Duration) + " minutes"));
            sb.Append(Fact("Pass mark", FmtPct(cfg.Pass)));
            sb.Append(Fact("Validity", Certs.ExpiryYears(c) + " years"));
        }
        else
        {
            sb.Append(Fact("Status", status));
            sb.Append(Fact("Examination", "Blueprint in development"));
            sb.Append(Fact("Fees", "Announced at launch"));
            sb.Append(Fact("Register interest", "Be notified when it opens"));
        }
        sb.Append("</div></div></section>");

        // ── routes & how to apply (from the per-certification routes configuration) ──
        sb.Append("<section class=\"sec\"><div class=\"wrap\"><span class=\"eyebrow\">Application routes</span><div class=\"uline\"></div>");
        sb.Append("<p class=\"lead\">").Append(Esc(acronym)).Append(" offers the following application routes:</p>");
        sb.Append("<ul class=\"cert-routes\">");
        foreach (var rt in Routes.For(db, id, publicOnly: true))
        {
            var rl = H.Str(rt["label"]) ?? "";
            var rd = H.Str(rt["description"]) ?? "";
            sb.Append("<li><strong>").Append(Esc(rl)).Append("</strong>");
            if (rd.Length > 0) sb.Append(" — ").Append(Esc(rd));
            sb.Append("</li>");
        }
        sb.Append("</ul>");
        if (id == Certs.DefaultId)
            // PCP-AI has dedicated public resource pages
            sb.Append("<p class=\"lead\">See <a href=\"eligibility-requirements.html\">eligibility requirements</a>, the <a href=\"exam-structure.html\">exam structure</a>, the <a href=\"body-of-knowledge.html\">Body of Knowledge</a> and the <a href=\"handbook.html\">candidate handbook</a>.</p>");
        else if (open)
            sb.Append("<p class=\"lead\">Full eligibility criteria, the examination blueprint and the ").Append(Esc(acronym)).Append(" Body of Knowledge are confirmed during application. <a href=\"/app/register?product=exam&amp;cert=").Append(Uri.EscapeDataString(code)).Append("\">Begin your application</a> or <a href=\"request-info.html?cert=").Append(Uri.EscapeDataString(code)).Append("\">request more information</a>.</p>");
        else
            sb.Append("<p class=\"lead\">Applications for ").Append(Esc(acronym)).Append(" are not open yet. <a href=\"request-info.html?cert=").Append(Uri.EscapeDataString(code)).Append("\">Register your interest</a> to be notified.</p>");
        sb.Append("</div></section>");

        // ── FAQs ──
        if (faqs.Count > 0)
        {
            sb.Append("<section class=\"sec sec-alt\"><div class=\"wrap\"><span class=\"eyebrow\">Frequently asked questions</span><div class=\"uline\"></div>");
            foreach (var (q, a) in faqs)
                sb.Append("<details class=\"faq\"><summary>").Append(Esc(q)).Append("</summary><p>").Append(Esc(a)).Append("</p></details>");
            sb.Append("</div></section>");
        }

        // ── closing CTA ──
        sb.Append("<section class=\"sec\"><div class=\"wrap cert-final-cta\">");
        sb.Append("<h2>").Append(open ? "Ready to apply for " : "Be first to hear about ").Append(Esc(acronym)).Append("?</h2>");
        if (open)
            sb.Append("<a class=\"btn btn-red\" href=\"/app/register?product=exam&amp;cert=").Append(Uri.EscapeDataString(code)).Append("\">Apply now</a>");
        else
            sb.Append("<a class=\"btn btn-red\" href=\"request-info.html?cert=").Append(Uri.EscapeDataString(code)).Append("\">Register interest</a>");
        sb.Append("</div></section>");

        // structured data: FAQ schema (only when there are FAQs)
        if (faqs.Count > 0)
        {
            sb.Append("<script type=\"application/ld+json\">");
            var faqJson = new
            {
                context = "https://schema.org",
                type = "FAQPage",
                mainEntity = faqs.Select(f => new { type = "Question", name = f.q, acceptedAnswer = new { type = "Answer", text = f.a } })
            };
            sb.Append(JsonSerializer.Serialize(faqJson).Replace("\"context\"", "\"@context\"").Replace("\"type\"", "\"@type\""));
            sb.Append("</script>");
        }
        return sb.ToString();
    }

    static string Fact(string k, string v) =>
        "<div class=\"cert-fact\"><span class=\"cert-fact-v\">" + Esc(v) + "</span><span class=\"cert-fact-k\">" + Esc(k) + "</span></div>";

    static string FmtPrice(double n) => "USD " + (n == Math.Floor(n) ? ((long)n).ToString() : n.ToString("0.00"));
    static string FmtPct(double n) => (n == Math.Floor(n) ? ((long)n).ToString() : n.ToString("0.#")) + "%";
    static string Esc(string s) => (s ?? "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
}
