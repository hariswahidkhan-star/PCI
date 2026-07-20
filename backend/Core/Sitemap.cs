using System.Text;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Dynamic sitemap.xml generated from the pages table, so it always reflects real, indexable content:
/// only published pages that are not flagged noindex, on the canonical (non-www) host, and never the
/// private app shells or error pages. Cached and rebuilt when page content changes (follows
/// PageContent's version). Replaces the previously hand-maintained static file.
/// </summary>
public static class Sitemap
{
    // App shells + non-content pages are never in the sitemap regardless of their pages-table flags.
    static readonly HashSet<string> Exclude = new(StringComparer.OrdinalIgnoreCase)
    { "student.html", "admin.html", "exam-ui.html", "404.html", "500.html", "offline.html" };

    static int _ver = -1;
    static string _xml = "";
    static readonly object _lock = new();

    public static string Xml(Db db)
    {
        var v = PageContent.Version;
        lock (_lock)
        {
            if (_ver == v && _xml.Length > 0) return _xml;
            var host = Redirects.CanonicalBase;
            var sb = new StringBuilder(4096);
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            sb.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n");
            foreach (var r in db.Query("SELECT slug,updated_at FROM pages WHERE published=1 AND (noindex IS NULL OR noindex=0) ORDER BY slug"))
            {
                var slug = H.Str(r["slug"]) ?? "";
                if (slug.Length == 0 || Exclude.Contains(slug) || slug.Contains("..")) continue;
                var loc = slug.Equals("index.html", StringComparison.OrdinalIgnoreCase) ? host + "/" : host + "/" + slug;
                sb.Append("  <url><loc>").Append(Esc(loc)).Append("</loc>");
                if (H.Str(r["updated_at"]) is { Length: >= 10 } u) sb.Append("<lastmod>").Append(Esc(u[..10])).Append("</lastmod>");
                sb.Append("<changefreq>weekly</changefreq></url>\n");
            }
            // Dynamic blog posts (Content Centre) — published, indexable, canonical URLs only. Kept in the
            // main sitemap so Search Console/Bing discover them via the already-submitted /sitemap.xml.
            try
            {
                var bp = Blog.BasePath(db);
                foreach (var r in db.Query("SELECT slug,updated_at FROM blog_posts WHERE status='published' AND published=1 AND COALESCE(robots_noindex,0)=0 ORDER BY COALESCE(published_at,created_at) DESC"))
                {
                    var s = H.Str(r["slug"]) ?? "";
                    if (s.Length == 0 || s.Contains("..")) continue;
                    sb.Append("  <url><loc>").Append(Esc(host + bp + "/" + s)).Append("</loc>");
                    if (H.Str(r["updated_at"]) is { Length: >= 10 } bu) sb.Append("<lastmod>").Append(Esc(bu[..10])).Append("</lastmod>");
                    sb.Append("<changefreq>monthly</changefreq></url>\n");
                }
            }
            catch { /* blog tables absent on a very early boot — sitemap still valid */ }
            sb.Append("</urlset>\n");
            _xml = sb.ToString();
            _ver = v;
            return _xml;
        }
    }

    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
}
