using System.Text;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — discoverability (EXPANSION_PHASE0.md §6, EXPANSION_GOVERNANCE §8).
///
/// The Phase 0 audit found that not one `/world` URL appeared in any sitemap, and that a
/// world-only deployment served the Institute's robots.txt — advertising sitemaps on a different
/// domain and disallowing paths that do not exist on it. So PCI World was, in the literal sense,
/// undiscoverable except by people who already had a link.
///
/// This module fixes that with its own sitemap over its own content and a robots file that
/// describes the host it is actually served from. The policy stays people-first: only pages that
/// exist, are published, and are genuinely useful are listed. Token-addressed pages (shared
/// results, Passports) are deliberately absent — they carry `noindex` because their owners can
/// revoke them and no index can be un-published on request.
/// </summary>
public static class WorldSeo
{
    /// <summary>
    /// The world sitemap: core pages, every servable challenge, and every published article.
    /// Cached against a cheap content signature so a crawl does not re-query the catalogue each time.
    /// </summary>
    public static string Sitemap(Db db)
    {
        var sig = Signature(db);
        lock (_lock)
        {
            if (sig == _sig && _xml.Length > 0) return _xml;
            var host = WorldUrl.Base();
            var sb = new StringBuilder(8192);
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            sb.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n");

            void Url(string path, string? lastmod, string changefreq, string priority)
            {
                sb.Append("  <url><loc>").Append(Esc(host + path)).Append("</loc>");
                if (lastmod is { Length: >= 10 }) sb.Append("<lastmod>").Append(Esc(lastmod[..10])).Append("</lastmod>");
                sb.Append("<changefreq>").Append(changefreq).Append("</changefreq>");
                sb.Append("<priority>").Append(priority).Append("</priority></url>\n");
            }

            // Core surfaces. The home page changes daily by construction — the rotation opens a new
            // period every day — so `daily` here is a fact rather than a hint.
            Url("/world", null, "daily", "1.0");
            Url("/world/archive", null, "weekly", "0.9");
            Url("/world/about", null, "monthly", "0.5");
            Url("/world/verify", null, "yearly", "0.3");

            foreach (var r in db.Query(@"SELECT code, updated_at FROM pciworld_challenges
                    WHERE current_version>=1 AND retired=0 ORDER BY id"))
                Url("/world/challenge/" + H.Str(r["code"]), H.Str(r["updated_at"]), "monthly", "0.8");

            var blogCount = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_articles WHERE kind='blog' AND status='published'");
            if (blogCount > 0) Url("/world/blog", null, "weekly", "0.7");
            var newsCount = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_articles WHERE kind='news' AND status='published'");
            if (newsCount > 0) Url("/world/news", null, "daily", "0.7");

            foreach (var r in db.Query(@"SELECT slug, kind, updated_at FROM pciworld_articles
                    WHERE status='published' ORDER BY published_at DESC, id DESC"))
                Url($"/world/{H.Str(r["kind"])}/{H.Str(r["slug"])}", H.Str(r["updated_at"]), "monthly", "0.6");

            // Careers (CCP_PHASE4_DESIGN.md §7): published, OPEN postings of VERIFIED employers,
            // and nothing else — no closed postings, no drafts, no employer pages. Liveness is
            // computed here at render time, never from a state a background sweep maintains: the
            // predicates live IN the query, a row carrying a deadline is additionally re-checked
            // through the single honesty predicate (a lexical compare cannot fail closed on an
            // unparsable closes_at; CareersState.IsLive can and does), and the cache signature
            // below counts live postings so a deadline passing or a suspension landing invalidates
            // the cached XML on its own.
            if (Settings.Str(db, "pciworld_careers_enabled", "0") == "1")
            {
                var nowUtc = H.StampInMinutes(0);
                foreach (var r in db.Query(@"SELECT p.slug AS pslug, p.state, p.closes_at, p.updated_at,
                               e.slug AS eslug, e.state AS estate
                        FROM pciworld_job_postings p
                        JOIN pciworld_employers e ON e.id = p.employer_id
                        WHERE p.state='published' AND e.state='verified'
                          AND (p.closes_at IS NULL OR p.closes_at='' OR p.closes_at >= ?)
                        ORDER BY p.published_at DESC, p.id DESC", nowUtc))
                {
                    var closes = H.Str(r["closes_at"]);
                    if (!string.IsNullOrWhiteSpace(closes)
                        && !CareersState.IsLive(H.Str(r["state"]) ?? "", H.Str(r["estate"]) ?? "",
                                                closes, nowUtc)) continue;
                    Url($"/world/careers/{Uri.EscapeDataString(H.Str(r["eslug"]) ?? "")}/{Uri.EscapeDataString(H.Str(r["pslug"]) ?? "")}",
                        H.Str(r["updated_at"]), "daily", "0.6");
                }
            }

            sb.Append("</urlset>\n");
            _sig = sig;
            return _xml = sb.ToString();
        }
    }

    /// <summary>
    /// robots.txt for a PCI World-only host. Deliberately NOT the Institute's file: that one names
    /// sitemaps on another domain and disallows paths this deployment does not serve, which is
    /// worse than having no robots file at all.
    /// </summary>
    public static string Robots()
    {
        var host = WorldUrl.Base();
        return $"""
            # PCI World — a free project-challenge platform operated by the Project Controls Institute.
            User-agent: *
            Allow: /
            # The administration realm is a separate application and must never be indexed.
            Disallow: /world-admin
            Disallow: /api/
            # Token-addressed pages belong to the person who minted them. They also carry a noindex
            # meta tag; this is the belt to that pair of braces.
            Disallow: /world/r/
            Disallow: /world/p/
            Disallow: /world/i/
            Disallow: /world/account

            Sitemap: {host}/world-sitemap.xml
            """;
    }

    /// <summary>
    /// Organization + WebSite structured data for the world home page. Only claims the visible page
    /// supports: who operates PCI World and what it is. No ratings, no counts, no awards.
    /// </summary>
    public static string HomeJsonLd(Db db)
    {
        var host = WorldUrl.Base();
        var org = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Organization",
            ["name"] = "PCI World",
            ["url"] = host + "/world",
            ["description"] = "A free, anonymous-first project-challenge platform operated by the Project Controls Institute.",
            ["parentOrganization"] = new Dictionary<string, object?>
            {
                ["@type"] = "Organization",
                ["name"] = "Project Controls Institute",
                ["url"] = WorldPages.InstituteUrl(db),
            },
        };
        var site = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "WebSite",
            ["name"] = "PCI World",
            ["url"] = host + "/world",
        };
        return $"<script type=\"application/ld+json\">{WorldPages.Json(org)}</script>" +
               $"<script type=\"application/ld+json\">{WorldPages.Json(site)}</script>";
    }

    /// <summary>
    /// LearningResource structured data for a challenge. Declares it as free educational practice —
    /// which is exactly what the page says — and never as an assessment or a credential.
    /// </summary>
    public static string ChallengeJsonLd(Db db, string code, Dictionary<string, object?> version)
    {
        var o = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "LearningResource",
            ["name"] = H.Str(version["title"]),
            ["description"] = H.Str(version["hook"]),
            ["url"] = WorldUrl.Base() + "/world/challenge/" + code,
            ["learningResourceType"] = "Practice exercise",
            ["educationalLevel"] = H.Str(version["difficulty"]),
            ["timeRequired"] = $"PT{Math.Max(1, H.L(version["est_minutes"]))}M",
            ["isAccessibleForFree"] = true,
            ["inLanguage"] = "en",
            ["provider"] = new Dictionary<string, object?> { ["@type"] = "Organization", ["name"] = "PCI World" },
        };
        return $"<script type=\"application/ld+json\">{WorldPages.Json(o)}</script>";
    }

    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

    // ── cache ──
    static readonly object _lock = new();
    static string _xml = "";
    static string _sig = "";

    /// <summary>A cheap signature of everything the sitemap contains: if none of these move, the
    /// XML cannot have changed.</summary>
    static string Signature(Db db)
    {
        try
        {
            var ch = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_challenges WHERE current_version>=1 AND retired=0");
            var art = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_articles WHERE status='published'");
            var chMax = db.Scalar<string>("SELECT MAX(updated_at) FROM pciworld_challenges") ?? "";
            var artMax = db.Scalar<string>("SELECT MAX(updated_at) FROM pciworld_articles") ?? "";
            // Careers honesty (§7): the count of LIVE postings — same predicates as the entries
            // themselves — so a deadline passing changes the signature even though no row changed,
            // and a stale sitemap can never keep advertising a job past its close.
            var careersOn = Settings.Str(db, "pciworld_careers_enabled", "0");
            var jobs = db.Scalar<long>(@"SELECT COUNT(*) FROM pciworld_job_postings p
                    JOIN pciworld_employers e ON e.id = p.employer_id
                    WHERE p.state='published' AND e.state='verified'
                      AND (p.closes_at IS NULL OR p.closes_at='' OR p.closes_at >= ?)", H.StampInMinutes(0));
            var jobsMax = db.Scalar<string>("SELECT MAX(updated_at) FROM pciworld_job_postings") ?? "";
            return $"{ch}|{art}|{chMax}|{artMax}|{careersOn}|{jobs}|{jobsMax}";
        }
        catch { return Guid.NewGuid().ToString(); }   // never cache on an error path
    }
}
