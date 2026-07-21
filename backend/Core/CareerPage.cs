using System.Text;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Server-rendered per-job landing page at /careers/{code}/{slug}. The chrome comes from the tokenised
/// careers-detail.html template; the job-specific content — title, meta, canonical, full description and
/// the Google-for-Jobs JobPosting structured data — is generated from the job_postings row. Publishing a
/// posting in the admin console therefore yields a crawlable, richly-marked-up public page with no new
/// source files, while the interactive apply flow stays on the careers board (careers.html?job=id).
/// </summary>
public static class CareerPage
{
    static readonly Dictionary<string, string> TypeLabel = new()
    { ["full_time"] = "Full-time", ["part_time"] = "Part-time", ["contract"] = "Contract", ["internship"] = "Internship", ["temporary"] = "Temporary" };
    static readonly Dictionary<string, string> RemoteLabel = new()
    { ["onsite"] = "On-site", ["remote"] = "Remote", ["hybrid"] = "Hybrid" };

    // A published posting that is still open (no close date, or not yet past it).
    public static Dictionary<string, object?>? ByCode(Db db, string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;
        return db.QueryOne(@"SELECT * FROM job_postings WHERE lower(job_code)=? AND status='published'
            AND (closes_at IS NULL OR closes_at='' OR closes_at > datetime('now'))", code.Trim().ToLowerInvariant());
    }

    // URL-safe slug from the title (cosmetic; the code segment is what identifies the posting).
    public static string Slug(Dictionary<string, object?> j)
    {
        var stored = H.Str(j["slug"]);
        var basis = string.IsNullOrWhiteSpace(stored) ? (H.Str(j["title"]) ?? "") : stored!;
        var sb = new StringBuilder();
        foreach (var ch in basis.ToLowerInvariant())
            if (char.IsLetterOrDigit(ch)) sb.Append(ch);
            else if (sb.Length > 0 && sb[^1] != '-') sb.Append('-');
        return sb.ToString().Trim('-') is { Length: > 0 } s ? (s.Length > 80 ? s[..80].Trim('-') : s) : "role";
    }

    // The canonical clean path for a posting, e.g. /careers/pci-2026-0001/senior-planning-engineer
    public static string Path(Dictionary<string, object?> j) =>
        "/careers/" + (H.Str(j["job_code"]) ?? "").ToLowerInvariant() + "/" + Slug(j);

    public static (string title, string desc, string canonical) Meta(Dictionary<string, object?> j)
    {
        var title = (H.Str(j["title"]) ?? "Career opportunity");
        var org = H.Str(j["organisation"]);
        var loc = new[] { H.Str(j["location"]), H.Str(j["country"]) }.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        var where = loc.Length > 0 ? " — " + string.Join(", ", loc) : "";
        var pageTitle = title + (string.IsNullOrWhiteSpace(org) ? "" : " at " + org) + where + " | PCI Careers";
        var desc = H.Str(j["description"]) ?? "";
        if (string.IsNullOrWhiteSpace(desc)) desc = $"{title}{(string.IsNullOrWhiteSpace(org) ? "" : " at " + org)}. Apply through the Project Controls Institute careers board.";
        if (desc.Length > 300) desc = desc[..297].TrimEnd() + "…";
        return (pageTitle, desc, Redirects.CanonicalBase + Path(j));
    }

    /// <summary>Render the full page HTML, or null when no open posting matches the code.</summary>
    public static string? Render(Db db, string webRoot, string code, string lang)
    {
        var job = ByCode(db, code);
        if (job is null) return null;
        var tplPath = System.IO.Path.Combine(webRoot, "careers-detail.html");
        if (!File.Exists(tplPath)) return null;
        var (title, desc, canonical) = Meta(job);
        var html = File.ReadAllText(tplPath)
            .Replace("<!--PCI-JOB-DETAIL-->", BuildBody(db, job))
            .Replace("{{TITLE}}", Esc(title))
            .Replace("{{DESC}}", Esc(desc))
            .Replace("{{CANONICAL}}", Esc(canonical));
        html = ListSections.Inject(db, html, lang);
        html = SeoTags.Inject(db, html, "careers/" + code.ToLowerInvariant() + ".html");
        return html;
    }

    static string BuildBody(Db db, Dictionary<string, object?> j)
    {
        string? S(string k) => H.Str(j.TryGetValue(k, out var v) ? v : null);
        var id = H.L(j["id"]);
        var title = S("title") ?? "Role";
        var org = S("organisation");
        var code = S("job_code") ?? "";
        var locBits = new[] { S("location"), S("country") }.Where(s => !string.IsNullOrWhiteSpace(s));
        var loc = string.Join(", ", locBits);
        var type = TypeLabel.GetValueOrDefault(S("employment_type") ?? "", S("employment_type") ?? "");
        var remote = RemoteLabel.GetValueOrDefault(S("remote_type") ?? "", "");
        var applyUrl = "/careers.html?job=" + id;

        var sb = new StringBuilder();

        // ── hero ──
        sb.Append("<section class=\"phead\"><div class=\"wrap\">");
        sb.Append("<div class=\"crumbbar-inline\"><a href=\"/careers.html\">Careers</a> · ").Append(Esc(code)).Append("</div>");
        var eyebrow = new[] { S("department"), S("sector") }.Where(s => !string.IsNullOrWhiteSpace(s));
        if (eyebrow.Any()) sb.Append("<span class=\"eyebrow\">").Append(Esc(string.Join(" · ", eyebrow))).Append("</span>");
        sb.Append("<h1>").Append(Esc(title)).Append("</h1>");
        if (!string.IsNullOrWhiteSpace(org)) sb.Append("<p class=\"phead-lead\">").Append(Esc(org!)).Append("</p>");
        var sub = new List<string>();
        if (loc.Length > 0) sub.Add("📍 " + loc);
        if (type.Length > 0) sub.Add(type);
        if (remote.Length > 0) sub.Add(remote);
        if (!string.IsNullOrWhiteSpace(S("experience_level"))) sub.Add(Cap(S("experience_level")!));
        if (sub.Count > 0) sb.Append("<p class=\"phead-sub\">").Append(Esc(string.Join(" · ", sub))).Append("</p>");
        sb.Append("<div class=\"phead-cta\"><a class=\"btn btn-red\" href=\"").Append(applyUrl).Append("\">Apply for this role</a>");
        sb.Append("<a class=\"btn btn-ghost\" href=\"/careers.html\">All open roles</a></div>");
        sb.Append("</div></section>");

        // ── key facts ──
        var facts = new List<(string, string)>();
        if (type.Length > 0) facts.Add(("Employment type", type));
        if (remote.Length > 0) facts.Add(("Work location", remote));
        if (loc.Length > 0) facts.Add(("Location", loc));
        var mn = H.D(j.GetValueOrDefault("salary_min")); var mx = H.D(j.GetValueOrDefault("salary_max"));
        if (H.B(j.GetValueOrDefault("salary_visible")) && (mn > 0 || mx > 0))
        {
            var cur = S("salary_currency") ?? "USD"; var per = S("salary_period") ?? "year";
            var range = mn > 0 && mx > 0 ? $"{cur} {mn:0} – {mx:0}" : $"{cur} {(mn > 0 ? mn : mx):0}";
            facts.Add(("Salary", $"{range} / {per}"));
        }
        if (H.L(j.GetValueOrDefault("vacancies")) > 1) facts.Add(("Vacancies", H.L(j["vacancies"]).ToString()));
        if (!string.IsNullOrWhiteSpace(S("expected_start"))) facts.Add(("Expected start", S("expected_start")!));
        if (!string.IsNullOrWhiteSpace(S("closes_at"))) facts.Add(("Closing date", S("closes_at")!));
        if (facts.Count > 0)
        {
            sb.Append("<section class=\"sec sec-alt\"><div class=\"wrap\"><span class=\"eyebrow\">At a glance</span><div class=\"uline\"></div><div class=\"cert-facts\">");
            foreach (var (k, v) in facts) sb.Append(Fact(k, v));
            sb.Append("</div></div></section>");
        }

        // ── narrative sections ──
        Section(sb, "About the role", S("description"), false);
        Section(sb, "Responsibilities", S("responsibilities"), true);
        Section(sb, "Requirements", S("requirements"), false);
        Section(sb, "Education", S("education"), true);
        Section(sb, "Benefits", S("benefits"), false);
        Section(sb, "Languages", S("languages"), true);
        Section(sb, "Certifications", S("certifications"), false);
        Section(sb, "Reporting line", S("reporting_line"), true);
        Section(sb, "How to apply", S("application_instructions"), false);
        Section(sb, "Equal opportunity", S("eo_statement"), true);

        // ── closing CTA ──
        sb.Append("<section class=\"sec\"><div class=\"wrap cert-final-cta\">");
        sb.Append("<h2>Ready to apply?</h2>");
        sb.Append("<a class=\"btn btn-red\" href=\"").Append(applyUrl).Append("\">Apply for this role</a>");
        sb.Append("</div></section>");

        // ── structured data: JobPosting (Google for Jobs) — server-rendered, no JS required ──
        sb.Append("<script type=\"application/ld+json\">").Append(PCI.Backend.Endpoints.Careers.JobJsonLd(db, j)).Append("</script>");
        return sb.ToString();
    }

    // Render a titled prose section. Multi-line values become paragraphs; alt shading alternates.
    static void Section(StringBuilder sb, string heading, string? body, bool alt)
    {
        if (string.IsNullOrWhiteSpace(body)) return;
        sb.Append("<section class=\"sec").Append(alt ? " sec-alt" : "").Append("\"><div class=\"wrap\"><span class=\"eyebrow\">")
          .Append(Esc(heading)).Append("</span><div class=\"uline\"></div>");
        foreach (var para in body!.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries))
            sb.Append("<p class=\"lead\">").Append(Esc(para.Trim())).Append("</p>");
        sb.Append("</div></section>");
    }

    static string Fact(string k, string v) =>
        "<div class=\"cert-fact\"><span class=\"cert-fact-v\">" + Esc(v) + "</span><span class=\"cert-fact-k\">" + Esc(k) + "</span></div>";

    static string Cap(string s) => s.Length == 0 ? s : char.ToUpperInvariant(s[0]) + s[1..];
    static string Esc(string s) => (s ?? "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
}
