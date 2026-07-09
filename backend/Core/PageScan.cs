using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Universal page-content capture — the engine that makes EVERY visible text region of the static
/// website database-editable ("100% dynamic"). A page is parsed into a lightweight DOM; every text
/// region is captured outer-first as a block keyed by a hash of its ORIGINAL content, so keys are
/// stable across restarts and reseeds. Regions whose content is identical across many pages (the
/// shared header, footer, CTAs) become ONE global binding in site_content — edited once, changed
/// everywhere. Per-page regions live in page_blocks and are edited in the admin Pages editor.
///
/// At render time the same scanner runs over the same file bytes, so regions map 1:1 to their
/// blocks; a block whose value still equals the file's original content injects nothing (the page
/// stays byte-identical), and an edited block replaces exactly its region — escaped for plain text,
/// whitelist-sanitized for rich text. Reverting is deleting the row: the original text returns.
/// </summary>
public static class PageScan
{
    // ---- capture policy ----

    /// <summary>Content elements captured whole (inner HTML) when they contain any text.</summary>
    static readonly HashSet<string> CaptureTags = new(StringComparer.OrdinalIgnoreCase)
    {
        "h1","h2","h3","h4","h5","h6","p","li","td","th","dt","dd",
        "blockquote","figcaption","caption","summary","label","legend","a","button"
    };

    /// <summary>Subtrees that are never content: code, styling, vector art, alternates.</summary>
    static readonly HashSet<string> SkipTags = new(StringComparer.OrdinalIgnoreCase)
    { "script", "style", "svg", "template", "noscript", "head", "select", "textarea" };

    static readonly HashSet<string> VoidTags = new(StringComparer.OrdinalIgnoreCase)
    { "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr" };

    /// <summary>An element region shared verbatim by at least this many pages becomes ONE global
    /// site_content binding (edit once → changes everywhere) instead of hundreds of page blocks.</summary>
    public const int GlobalMinPages = 20;

    // ---- public shapes ----

    /// <summary>A replaceable region of a page. Start/End delimit the exact span to substitute:
    /// the inner HTML for element regions, the attribute value for 'attr' regions.</summary>
    public sealed record Region(string Key, string GKey, string CType, string Value, string Label, int Start, int End, int Order);

    // ---- minimal DOM ----

    sealed class Node
    {
        public string Tag = "";
        public string Attrs = "";
        public int TagStart;                               // where the element's own open tag begins
        public int InnerStart, InnerEnd;
        public List<Node> Children = new();
        public List<(int start, int end)> Texts = new();   // direct text spans
    }

    static readonly Regex RxTok = new(
        @"<!--.*?-->|<!\[CDATA\[.*?\]\]>|<!DOCTYPE[^>]*>|<(/?)([a-zA-Z][a-zA-Z0-9-]*)((?:[^>""']|""[^""]*""|'[^']*')*?)(/?)>",
        RegexOptions.Singleline | RegexOptions.Compiled);

    static readonly Regex RxMarker = new(@"<!--\s*(/?)PCI-([A-Z-]+)\s*-->", RegexOptions.Compiled);
    static readonly Regex RxStripTags = new("<[^>]+>", RegexOptions.Singleline | RegexOptions.Compiled);
    static readonly Regex RxWs = new(@"\s+", RegexOptions.Compiled);

    /// <summary>Scan a page and return every capturable region, in document order.</summary>
    public static List<Region> Scan(string html)
    {
        var (root, markers) = Parse(html);
        var regions = new List<Region>();
        bool firstH1Seen = false;
        var occ = new Dictionary<string, int>(StringComparer.Ordinal);

        void Collect(Node n)
        {
            foreach (var c in n.Children)
            {
                if (SkipTags.Contains(c.Tag)) continue;
                if (c.Attrs.Contains("data-cms", StringComparison.OrdinalIgnoreCase)) continue;      // legacy region — already editable
                if (c.Attrs.Contains("data-price", StringComparison.OrdinalIgnoreCase)) continue;    // pricing-engine-owned (Admin → Pricing)
                // skip elements whose OWN tag lies inside a server-rendered zone (cert catalogue,
                // nav, FAQ list, …) — the zone's fallback is replaced from the DB. A container that
                // merely STARTS its content with a marker still descends: siblings after the close
                // marker are ordinary content.
                if (InMarker(markers, c.TagStart)) continue;

                if (c.Tag.Equals("h1", StringComparison.OrdinalIgnoreCase) && !firstH1Seen)
                { firstH1Seen = true; continue; }                                                    // the '_h1' block owns the headline

                if (c.Tag.Equals("img", StringComparison.OrdinalIgnoreCase))
                { CaptureImg(html, c, regions, occ); continue; }

                var innerRaw = html.Substring(c.InnerStart, c.InnerEnd - c.InnerStart);
                var text = Collapse(System.Net.WebUtility.HtmlDecode(RxStripTags.Replace(innerRaw, " ")));
                var directText = c.Texts.Any(t => !IsWs(html, t.start, t.end));

                // capture: a known content tag with text, or ANY element with bare text of its own
                if ((CaptureTags.Contains(c.Tag) && text.Length > 0) || directText)
                {
                    var isHtml = innerRaw.IndexOf('<') >= 0;
                    var hash = Hash(c.Tag.ToLowerInvariant() + "|" + Collapse(innerRaw));
                    var baseKey = $"t:{c.Tag.ToLowerInvariant()}:{hash}";
                    var nth = occ[baseKey] = occ.TryGetValue(baseKey, out var k) ? k + 1 : 1;
                    var key = nth == 1 ? baseKey : $"{baseKey}:{nth}";
                    var val = isHtml ? innerRaw.Trim() : Collapse(System.Net.WebUtility.HtmlDecode(innerRaw));
                    regions.Add(new Region(key, $"g:{c.Tag.ToLowerInvariant()}:{hash}", isHtml ? "html" : "text",
                        val, Trunc(text, 80), c.InnerStart, c.InnerEnd, regions.Count));
                    continue;                                                                        // outer-first: no overlaps
                }
                Collect(c);
            }
        }

        Collect(root);
        return regions;
    }

    static void CaptureImg(string html, Node img, List<Region> regions, Dictionary<string, int> occ)
    {
        var src = AttrSpan(html, img, "src");
        var alt = AttrSpan(html, img, "alt");
        if (src is null) return;
        var srcVal = html.Substring(src.Value.start, src.Value.end - src.Value.start);
        var altVal = alt is null ? "" : html.Substring(alt.Value.start, alt.Value.end - alt.Value.start);
        if (srcVal.StartsWith("data:", StringComparison.OrdinalIgnoreCase)) return;                  // inline art, not content
        var hash = Hash("img|" + srcVal + "|" + altVal);
        var baseKey = $"t:img:{hash}";
        var nth = occ[baseKey] = occ.TryGetValue(baseKey, out var k) ? k + 1 : 1;
        var key = nth == 1 ? baseKey : $"{baseKey}:{nth}";
        var label = Trunc(altVal.Length > 0 ? altVal : srcVal, 80);
        regions.Add(new Region(key + "@src", $"g:img:{hash}@src", "attr", srcVal, "Image: " + label, src.Value.start, src.Value.end, regions.Count));
        if (alt is not null)
            regions.Add(new Region(key + "@alt", $"g:img:{hash}@alt", "attr", System.Net.WebUtility.HtmlDecode(altVal), "Image alt: " + label, alt.Value.start, alt.Value.end, regions.Count));
    }

    static (int start, int end)? AttrSpan(string html, Node n, string name)
    {
        // find name="value" inside the tag's attribute source; n.InnerStart is right after '>' for
        // void tags we store attr text separately, so search the raw attrs with offset bookkeeping
        var m = Regex.Match(n.Attrs, $@"\b{name}\s*=\s*(""([^""]*)""|'([^']*)')", RegexOptions.IgnoreCase);
        if (!m.Success) return null;
        var g = m.Groups[2].Success && m.Groups[1].Value.StartsWith('"') ? m.Groups[2] : m.Groups[3];
        var attrsStart = n.InnerStart - 1 - n.Attrs.Length;                                          // '>' sits at InnerStart-1
        return (attrsStart + g.Index, attrsStart + g.Index + g.Length);
    }

    // ---- parse ----

    static (Node root, List<(int, int)> markers) Parse(string html)
    {
        var root = new Node { Tag = "#root", InnerStart = 0, InnerEnd = html.Length };
        var stack = new List<Node> { root };
        var markers = new List<(int, int)>();
        var openMarkers = new Dictionary<string, int>(StringComparer.Ordinal);
        int lastIndex = 0;

        void TextTo(int end)
        {
            if (end > lastIndex) stack[^1].Texts.Add((lastIndex, end));
        }

        foreach (Match m in RxTok.Matches(html))
        {
            TextTo(m.Index);
            lastIndex = m.Index + m.Length;

            if (m.Value.StartsWith("<!--"))
            {
                var mk = RxMarker.Match(m.Value);
                if (mk.Success)
                {
                    var name = mk.Groups[2].Value;
                    if (mk.Groups[1].Value == "/") { if (openMarkers.Remove(name, out var s)) markers.Add((s, lastIndex)); }
                    else openMarkers[name] = m.Index;
                }
                continue;
            }
            if (!m.Groups[2].Success) continue;                                                      // doctype/cdata

            var tag = m.Groups[2].Value;
            var closing = m.Groups[1].Value == "/";
            var selfClose = m.Groups[4].Value == "/" || VoidTags.Contains(tag);

            if (closing)
            {
                for (int i = stack.Count - 1; i >= 1; i--)
                {
                    if (stack[i].Tag.Equals(tag, StringComparison.OrdinalIgnoreCase))
                    {
                        for (int j = stack.Count - 1; j >= i; j--) { stack[j].InnerEnd = m.Index; stack.RemoveAt(j); }
                        break;
                    }
                }
                continue;
            }

            var node = new Node { Tag = tag, Attrs = m.Groups[3].Value, TagStart = m.Index, InnerStart = m.Index + m.Length, InnerEnd = m.Index + m.Length };
            stack[^1].Children.Add(node);
            if (!selfClose) stack.Add(node);
        }
        TextTo(html.Length);
        for (int j = stack.Count - 1; j >= 1; j--) stack[j].InnerEnd = html.Length;
        return (root, markers);
    }

    static bool InMarker(List<(int s, int e)> markers, int pos)
    {
        foreach (var (s, e) in markers) if (pos >= s && pos < e) return true;
        return false;
    }

    // ---- seeding ----

    /// <summary>Capture every page's regions into the database (idempotent — safe on every boot).
    /// New regions are inserted with their original content; edited rows are never touched; rows
    /// whose region no longer exists in the shipped HTML are removed (their override cannot apply).
    /// Regions identical across ≥ <see cref="GlobalMinPages"/> pages are stored once, globally.</summary>
    public static void SeedAll(Db db, string webRoot, HashSet<string> appShells)
    {
        var perPage = new Dictionary<string, List<Region>>(StringComparer.OrdinalIgnoreCase);
        var pagesByG = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        var h1s = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var file in Directory.EnumerateFiles(webRoot, "*.html"))
        {
            var slug = Path.GetFileName(file);
            if (appShells.Contains(slug)) continue;
            string html; try { html = File.ReadAllText(file); } catch { continue; }
            var regions = Scan(html);
            perPage[slug] = regions;
            foreach (var r in regions)
            {
                if (!pagesByG.TryGetValue(r.GKey, out var set)) pagesByG[r.GKey] = set = new(StringComparer.OrdinalIgnoreCase);
                set.Add(slug);
            }
            var h1 = PageContent.RxH1.Match(html);
            if (h1.Success)
            {
                var t = Collapse(System.Net.WebUtility.HtmlDecode(RxStripTags.Replace(h1.Groups[2].Value, " ")));
                if (t.Length is > 0 and <= 300) h1s[slug] = t;
            }
        }

        var globals = new HashSet<string>(pagesByG.Where(kv => kv.Value.Count >= GlobalMinPages).Select(kv => kv.Key), StringComparer.Ordinal);

        // desired state
        var wantPage = new Dictionary<(string slug, string key), Region>();
        var wantGlobal = new Dictionary<string, Region>(StringComparer.Ordinal);
        foreach (var (slug, regions) in perPage)
            foreach (var r in regions)
            {
                if (globals.Contains(r.GKey)) wantGlobal.TryAdd(r.GKey, r);
                else wantPage[(slug, r.Key)] = r;
            }

        // current state
        var havePage = new HashSet<(string, string)>();
        foreach (var row in db.Query("SELECT slug,block_key FROM page_blocks WHERE block_key LIKE 't:%'"))
            havePage.Add((H.Str(row["slug"]) ?? "", H.Str(row["block_key"]) ?? ""));
        var haveGlobal = new HashSet<string>(StringComparer.Ordinal);
        foreach (var row in db.Query("SELECT ckey FROM site_content WHERE ckey LIKE 'g:%'"))
            haveGlobal.Add(H.Str(row["ckey"]) ?? "");

        var addPage = wantPage.Where(kv => !havePage.Contains(kv.Key)).ToList();
        var dropPage = havePage.Where(k => !wantPage.ContainsKey(k)).ToList();
        var addGlobal = wantGlobal.Where(kv => !haveGlobal.Contains(kv.Key)).ToList();
        var dropGlobal = haveGlobal.Where(k => !wantGlobal.ContainsKey(k)).ToList();

        if (addPage.Count + dropPage.Count + addGlobal.Count + dropGlobal.Count == 0 && perPage.Count == 0) return;

        db.Transaction(() =>
        {
            foreach (var ((slug, key), r) in addPage)
                db.Execute("INSERT OR IGNORE INTO page_blocks(slug,block_key,label,ctype,cvalue,sort_order) VALUES(?,?,?,?,?,?)",
                    slug, key, r.Label, r.CType, r.Value, r.Order);
            foreach (var (slug, key) in dropPage)
                db.Execute("DELETE FROM page_blocks WHERE slug=? AND block_key=?", slug, key);
            foreach (var (gkey, r) in addGlobal)
                db.Execute("INSERT OR IGNORE INTO site_content(ckey,cgroup,label,ctype,cvalue) VALUES(?,?,?,?,?)",
                    gkey, "Shared elements", r.Label, r.CType, r.Value);
            foreach (var gkey in dropGlobal)
                db.Execute("DELETE FROM site_content WHERE ckey=?", gkey);
            foreach (var (slug, text) in h1s)
                db.Execute("INSERT OR IGNORE INTO page_blocks(slug,block_key,label,ctype,cvalue) VALUES(?,?,?,?,?)",
                    slug, "_h1", "Headline", "text", text);
            foreach (var slug in perPage.Keys)
                db.Execute("INSERT OR IGNORE INTO pages(slug) VALUES(?)", slug);
        });

        if (addPage.Count + dropPage.Count + addGlobal.Count + dropGlobal.Count > 0)
            Console.WriteLine($"[content] page scan: +{addPage.Count} blocks, -{dropPage.Count} stale, +{addGlobal.Count} shared, -{dropGlobal.Count} stale shared across {perPage.Count} pages");
    }

    // ---- render-time injection ----

    /// <summary>Apply block overrides to a page's HTML. <paramref name="lookup"/> resolves a region
    /// to its stored (value, ctype) — page block first, then global — or null when unset. Regions
    /// whose stored value still equals the file's content are left untouched, so an unedited page
    /// is served byte-identical.</summary>
    public static string Apply(string html, Func<Region, (string val, string ctype)?> lookup)
    {
        var regions = Scan(html);
        List<(int s, int e, string repl)>? edits = null;
        foreach (var r in regions)
        {
            var hit = lookup(r);
            if (hit is null || hit.Value.val == r.Value) continue;
            var (val, ctype) = hit.Value;
            var repl = ctype switch
            {
                "attr" => AttrEsc(val),
                "html" => HtmlSanitize.Clean(val),
                _ => Esc(val),
            };
            (edits ??= new()).Add((r.Start, r.End, repl));
        }
        if (edits is null) return html;
        edits.Sort((a, b) => a.s.CompareTo(b.s));
        var sb = new StringBuilder(html.Length + 256);
        int pos = 0;
        foreach (var (s, e, repl) in edits)
        {
            if (s < pos) continue;                                                                   // overlap guard (never expected)
            sb.Append(html, pos, s - pos).Append(repl);
            pos = e;
        }
        sb.Append(html, pos, html.Length - pos);
        return sb.ToString();
    }

    // ---- helpers ----

    static string Hash(string s)
    {
        var b = SHA256.HashData(Encoding.UTF8.GetBytes(s));
        return Convert.ToHexString(b, 0, 5).ToLowerInvariant();                                       // 10 hex chars
    }

    static bool IsWs(string html, int start, int end)
    {
        for (int i = start; i < end; i++) if (!char.IsWhiteSpace(html[i])) return false;
        return true;
    }

    static string Collapse(string s) => RxWs.Replace(s, " ").Trim();
    static string Trunc(string s, int n) => s.Length <= n ? s : s[..(n - 1)] + "…";
    static string Esc(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    static string AttrEsc(string s)
    {
        var v = Esc(s).Replace("\"", "&quot;");
        // an attribute override must never smuggle a scriptable scheme (src/href live here)
        var probe = new string(v.Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c)).ToArray()).ToLowerInvariant();
        return probe.StartsWith("javascript:") || probe.StartsWith("vbscript:") || probe.StartsWith("data:") ? "" : v;
    }
}
