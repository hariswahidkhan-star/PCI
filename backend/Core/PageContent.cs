using System.Text.RegularExpressions;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Server-side content injection for the static website — the engine behind "fully dynamic content".
/// For a content page it applies, from the database, the page's editable title, meta description and any
/// tagged content regions (data-cms), producing the final HTML BEFORE it reaches the browser. So content
/// edited in the admin panel is live immediately, is seen by search engines, and works with JavaScript off.
///
/// Everything is cached and only recomputed when content actually changes (a global version counter that
/// the admin edit endpoints bump), so steady-state page serving stays a dictionary lookup + cached string.
/// </summary>
public static class PageContent
{
    // The application UIs (portals + exam runner) are not content pages; never inject into them.
    // The checkout, enrolment wizard and launcher ARE captured: their static copy is editable like
    // any page — only the JS-rendered parts (order summary, price maths) stay engine-driven.
    static readonly HashSet<string> AppShells = new(StringComparer.OrdinalIgnoreCase)
    { "student.html", "admin.html", "exam-ui.html" };

    static int _version = 1;
    /// <summary>Called by any admin content edit so cached injections refresh on the next request.</summary>
    public static void Bump() => Interlocked.Increment(ref _version);
    /// <summary>Current content version — lets other caches (e.g. the search index) follow content edits.</summary>
    public static int Version => Volatile.Read(ref _version);

    // caches, keyed by content version so a bump transparently invalidates them
    static int _cachedVersion = -1;
    static Dictionary<string, string> _globalContent = new();                               // ckey → value (data-cms bindings)
    static Dictionary<string, (string val, string ctype)> _globalElems = new();             // 'g:…' shared element → value
    static Dictionary<string, (string? title, string? meta)> _pageMeta = new();             // slug → title/meta
    static HashSet<string> _noindex = new(StringComparer.OrdinalIgnoreCase);                 // slugs flagged noindex
    static Dictionary<string, Dictionary<string, string>> _pageBlocks = new();              // slug → {block_key → value}
    static Dictionary<string, Dictionary<string, (string val, string ctype)>> _elemBlocks = new(); // slug → {'t:…' → value}
    static readonly Dictionary<string, (int ver, string html)> _rendered = new(StringComparer.OrdinalIgnoreCase);
    static readonly object _lock = new();

    static void EnsureLoaded(Db db)
    {
        var v = Volatile.Read(ref _version);
        if (_cachedVersion == v) return;
        lock (_lock)
        {
            if (_cachedVersion == v) return;
            var gc = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var ge = new Dictionary<string, (string, string)>(StringComparer.Ordinal);
            foreach (var r in db.Query("SELECT ckey,cvalue,ctype FROM site_content WHERE cvalue IS NOT NULL"))
            {
                var key = H.Str(r["ckey"]) ?? "";
                var val = H.Str(r["cvalue"]) ?? "";
                if (key.StartsWith("g:", StringComparison.Ordinal)) ge[key] = (val, H.Str(r["ctype"]) ?? "text");
                else if (val.Length > 0) gc[key] = val;
            }
            var pm = new Dictionary<string, (string?, string?)>(StringComparer.OrdinalIgnoreCase);
            var ni = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in db.Query("SELECT slug,title,meta_description,noindex FROM pages"))
            {
                var slug = H.Str(r["slug"]) ?? "";
                pm[slug] = (H.Str(r["title"]), H.Str(r["meta_description"]));
                if (H.L(r["noindex"]) == 1) ni.Add(slug);
            }
            var pb = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            var eb = new Dictionary<string, Dictionary<string, (string, string)>>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in db.Query("SELECT slug,block_key,cvalue,ctype FROM page_blocks WHERE cvalue IS NOT NULL"))
            {
                var slug = H.Str(r["slug"]) ?? "";
                var key = H.Str(r["block_key"]) ?? "";
                var val = H.Str(r["cvalue"]) ?? "";
                if (key.StartsWith("t:", StringComparison.Ordinal))
                {
                    if (!eb.TryGetValue(slug, out var em)) eb[slug] = em = new(StringComparer.Ordinal);
                    em[key] = (val, H.Str(r["ctype"]) ?? "text");
                }
                else if (val.Length > 0)
                {
                    if (!pb.TryGetValue(slug, out var m)) pb[slug] = m = new(StringComparer.OrdinalIgnoreCase);
                    m[key] = val;
                }
            }
            _globalContent = gc; _globalElems = ge; _pageMeta = pm; _pageBlocks = pb; _elemBlocks = eb; _noindex = ni;
            _rendered.Clear();
            _cachedVersion = v;
        }
    }

    /// <summary>Structured overrides for one page (used by the /api/page-content endpoint).</summary>
    public static object ForApi(Db db, string slug)
    {
        EnsureLoaded(db);
        _pageMeta.TryGetValue(slug, out var meta);
        _pageBlocks.TryGetValue(slug, out var blocks);
        return new { slug, title = meta.title, meta_description = meta.meta, blocks = blocks ?? new(StringComparer.OrdinalIgnoreCase) };
    }

    /// <summary>True if the page has any dynamic content that must be injected (so the middleware can
    /// cheaply skip untouched pages and let the static-file middleware serve them).</summary>
    public static bool HasOverrides(Db db, string slug)
    {
        if (AppShells.Contains(slug)) return false;
        EnsureLoaded(db);
        if (_globalContent.Count > 0 || _globalElems.Count > 0) return true; // global bindings may apply anywhere
        if (_pageMeta.TryGetValue(slug, out var m) && (m.title is not null || m.meta is not null)) return true;
        if (_noindex.Contains(slug)) return true;                             // a noindex-only page still needs its meta injected
        return _pageBlocks.ContainsKey(slug) || _elemBlocks.ContainsKey(slug);
    }

    /// <summary>Inject title/meta/data-cms overrides into a page's HTML. Cached per (slug, version);
    /// the raw file is read (via <paramref name="readHtml"/>) only on a cache miss.</summary>
    public static string Render(Db db, string slug, Func<string> readHtml)
    {
        EnsureLoaded(db);
        var v = _cachedVersion;
        lock (_lock)
        {
            if (_rendered.TryGetValue(slug, out var hit) && hit.ver == v) return hit.html;
        }
        var outHtml = Inject(slug, readHtml());
        lock (_lock) { _rendered[slug] = (v, outHtml); }
        return outHtml;
    }

    static readonly Regex RxTitle = new(@"<title>.*?</title>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    static readonly Regex RxMeta = new("(<meta\\s+name=[\"']description[\"']\\s+content=[\"']).*?([\"'])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    // social-card mirrors of title/description: og:title, og:description, twitter:title, twitter:description
    static readonly Regex RxOgTitle = new("(<meta\\s+(?:property|name)=[\"'](?:og|twitter):title[\"']\\s+content=[\"']).*?([\"'])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    static readonly Regex RxOgDesc = new("(<meta\\s+(?:property|name)=[\"'](?:og|twitter):description[\"']\\s+content=[\"']).*?([\"'])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    // an element carrying a data-cms attribute: g1=open tag, g2=tag name, g4=key, g6=inner, g7=close tag
    static readonly Regex RxCms = new(@"(<([a-zA-Z0-9]+)([^>]*?)\sdata-cms=""([^""]+)""([^>]*)>)(.*?)(</\2>)", RegexOptions.Singleline);
    // the first <h1> on the page (positional headline override, key '_h1' — no HTML tagging needed)
    public static readonly Regex RxH1 = new(@"(<h1[^>]*>)(.*?)(</h1>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    static readonly Regex RxHeadOpen = new(@"<head[^>]*>", RegexOptions.IgnoreCase);
    static readonly Regex RxRobotsNoindex = new(@"<meta\s+name=[""']robots[""'][^>]*noindex", RegexOptions.IgnoreCase);

    static string Inject(string slug, string html)
    {
        _pageMeta.TryGetValue(slug, out var meta);
        _pageBlocks.TryGetValue(slug, out var blocks);
        _elemBlocks.TryGetValue(slug, out var elems);

        // 1) universal element blocks — every text region of the page, resolved page-block first,
        //    then shared element. Runs on the raw file so region offsets/hashes line up; regions
        //    whose stored value is unchanged inject nothing (unedited pages stay byte-identical).
        if ((elems is { Count: > 0 }) || _globalElems.Count > 0)
        {
            var globals = _globalElems;
            html = PageScan.Apply(html, r =>
            {
                if (elems is not null && elems.TryGetValue(r.Key, out var pv)) return pv;
                if (globals.TryGetValue(r.GKey, out var gv)) return gv;
                return null;
            });
        }

        if (!string.IsNullOrEmpty(meta.title))
        {
            html = RxTitle.Replace(html, "<title>" + Esc(meta.title!) + "</title>", 1);
            html = RxOgTitle.Replace(html, m => m.Groups[1].Value + EscAttr(meta.title!) + m.Groups[2].Value);
        }
        if (!string.IsNullOrEmpty(meta.meta))
        {
            html = RxMeta.Replace(html, m => m.Groups[1].Value + EscAttr(meta.meta!) + m.Groups[2].Value, 1);
            html = RxOgDesc.Replace(html, m => m.Groups[1].Value + EscAttr(meta.meta!) + m.Groups[2].Value);
        }

        // Admin "hide from search engines" flag → inject a robots noindex meta (once) so the toggle
        // actually takes effect on the served page. Skip if the file already declares one.
        if (_noindex.Contains(slug) && !RxRobotsNoindex.IsMatch(html))
            html = RxHeadOpen.Replace(html, m => m.Value + "<meta name=\"robots\" content=\"noindex, nofollow\"/>", 1);

        // positional headline: replace the first <h1>'s inner text (edits every page's headline with no
        // template tagging). data-cms regions below still take precedence for finer control.
        // An unedited headline (stored value == the file h1's own plain-text reading) injects nothing,
        // preserving inline markup such as <br/> and styled spans.
        if (blocks is not null && blocks.TryGetValue("_h1", out var h1) && !string.IsNullOrEmpty(h1))
            html = RxH1.Replace(html, m => PageScan.FlattenText(m.Groups[2].Value) == h1
                ? m.Value
                : m.Groups[1].Value + Esc(h1) + m.Groups[3].Value, 1);

        if ((blocks is { Count: > 0 }) || _globalContent.Count > 0)
        {
            html = RxCms.Replace(html, m =>
            {
                var key = m.Groups[4].Value;
                string? val = null;
                if (blocks is not null && blocks.TryGetValue(key, out var pv)) val = pv;
                else if (_globalContent.TryGetValue(key, out var gv)) val = gv;
                if (val is null) return m.Value;                        // no override → leave the static content
                return m.Groups[1].Value + Esc(val) + m.Groups[7].Value; // replace inner text only (safe)
            });
        }
        return html;
    }

    /// <summary>Idempotent boot seed: capture EVERY text region of every content page as an editable
    /// block (universal capture via <see cref="PageScan"/>), plus each page's headline ('_h1'). Runs on
    /// every boot: new regions are added, stale ones removed, operator edits always preserved.</summary>
    public static void SeedFromFiles(Db db, string webRoot)
    {
        try
        {
            if (!Directory.Exists(webRoot)) return;
            PageScan.SeedAll(db, webRoot, AppShells);
            Bump();
        }
        catch (Exception e) { Console.Error.WriteLine($"[content] page capture seed skipped: {e.Message}"); }
    }

    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    static string EscAttr(string s) => Esc(s).Replace("\"", "&quot;");
}
