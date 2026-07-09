using System.Text;
using System.Text.RegularExpressions;

namespace PCI.Backend.Core;

/// <summary>
/// Whitelist sanitizer for operator-edited rich-text blocks. Edited content is written by admins,
/// but it is served to every visitor — so nothing scriptable may pass: only a fixed set of
/// formatting tags survives, attributes are rebuilt from a per-tag whitelist, URL attributes must
/// carry a safe scheme, and tags are re-balanced so a half-closed edit cannot break the page
/// structure around it. Unknown structural tags are unwrapped (their text is kept); script-like
/// subtrees are dropped whole.
/// </summary>
public static class HtmlSanitize
{
    static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        "a","abbr","b","blockquote","br","caption","cite","code","dd","div","dl","dt","em",
        "figcaption","figure","h2","h3","h4","h5","h6","hr","i","img","li","mark","ol","p","q",
        "s","small","span","strong","sub","summary","sup","table","tbody","td","th","thead",
        "time","tr","u","ul"
    };

    /// <summary>Subtrees dropped INCLUDING their content.</summary>
    static readonly HashSet<string> Dropped = new(StringComparer.OrdinalIgnoreCase)
    { "script","style","iframe","object","embed","svg","math","template","noscript","form","input","select","textarea","button","link","meta","base","head" };

    static readonly HashSet<string> Void = new(StringComparer.OrdinalIgnoreCase) { "br", "hr", "img" };

    static readonly Dictionary<string, string[]> TagAttrs = new(StringComparer.OrdinalIgnoreCase)
    {
        ["a"] = new[] { "href", "title", "target", "rel", "aria-label" },
        ["img"] = new[] { "src", "alt", "width", "height", "loading" },
        ["td"] = new[] { "colspan", "rowspan" },
        ["th"] = new[] { "colspan", "rowspan" },
        ["time"] = new[] { "datetime" },
    };
    static readonly string[] GlobalAttrs = { "class", "style" };

    static readonly Regex RxTag = new(@"<(/?)([a-zA-Z][a-zA-Z0-9-]*)((?:[^>""']|""[^""]*""|'[^']*')*?)(/?)>|<!--.*?-->",
        RegexOptions.Singleline | RegexOptions.Compiled);
    static readonly Regex RxAttr = new(@"([a-zA-Z][a-zA-Z0-9-]*)\s*=\s*(""[^""]*""|'[^']*'|[^\s""'>]+)", RegexOptions.Compiled);

    public static string Clean(string html)
    {
        if (string.IsNullOrEmpty(html)) return "";
        var sb = new StringBuilder(html.Length);
        var stack = new List<string>();
        int pos = 0, dropUntilDepth = -1;
        var dropStack = new List<string>();

        void Text(string t)
        {
            if (dropUntilDepth >= 0) return;
            sb.Append(t.Replace("<", "&lt;"));
        }

        foreach (Match m in RxTag.Matches(html))
        {
            Text(html[pos..m.Index]);
            pos = m.Index + m.Length;
            if (m.Value.StartsWith("<!--")) continue;

            var closing = m.Groups[1].Value == "/";
            var tag = m.Groups[2].Value.ToLowerInvariant();
            var self = m.Groups[4].Value == "/" || Void.Contains(tag);

            if (dropUntilDepth >= 0)
            {
                // inside a dropped subtree: track nesting of the dropped tag only
                if (!closing && !self && tag == dropStack[^1]) dropStack.Add(tag);
                else if (closing && tag == dropStack[^1])
                {
                    dropStack.RemoveAt(dropStack.Count - 1);
                    if (dropStack.Count == 0) dropUntilDepth = -1;
                }
                continue;
            }

            if (Dropped.Contains(tag))
            {
                if (!closing && !self) { dropUntilDepth = stack.Count; dropStack.Add(tag); }
                continue;
            }
            if (!Allowed.Contains(tag)) continue;                       // unwrap: keep inner content

            if (closing)
            {
                var idx = stack.FindLastIndex(t => t == tag);
                if (idx < 0) continue;                                  // stray close — drop
                for (int i = stack.Count - 1; i >= idx; i--)
                { sb.Append("</").Append(stack[i]).Append('>'); stack.RemoveAt(i); }
                continue;
            }

            sb.Append('<').Append(tag);
            var attrs = m.Groups[3].Value;
            bool hasTargetBlank = false, hasRel = false;
            var kept = new List<(string name, string val)>();
            foreach (Match a in RxAttr.Matches(attrs))
            {
                var name = a.Groups[1].Value.ToLowerInvariant();
                var raw = a.Groups[2].Value.Trim();
                if (raw.Length >= 2 && (raw[0] == '"' || raw[0] == '\'')) raw = raw[1..^1];
                var val = System.Net.WebUtility.HtmlDecode(raw);
                var ok = GlobalAttrs.Contains(name) || (TagAttrs.TryGetValue(tag, out var extra) && extra.Contains(name));
                if (!ok) continue;
                if (name is "href" or "src" && !SafeUrl(val)) continue;
                if (name == "style" && Regex.IsMatch(val, @"expression|javascript|url\s*\(", RegexOptions.IgnoreCase)) continue;
                if (name == "target") { hasTargetBlank = val == "_blank"; }
                if (name == "rel") hasRel = true;
                kept.Add((name, val));
            }
            foreach (var (name, val) in kept)
                sb.Append(' ').Append(name).Append("=\"").Append(AttrEsc(val)).Append('"');
            if (hasTargetBlank && !hasRel) sb.Append(" rel=\"noopener\"");
            if (self) { sb.Append(tag == "br" || tag == "hr" || tag == "img" ? "/>" : ">"); }
            else { sb.Append('>'); stack.Add(tag); }
        }
        Text(html[pos..]);
        for (int i = stack.Count - 1; i >= 0; i--) sb.Append("</").Append(stack[i]).Append('>');
        return sb.ToString();
    }

    static bool SafeUrl(string url)
    {
        var probe = new string(url.Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c)).ToArray()).ToLowerInvariant();
        if (probe.StartsWith("javascript:") || probe.StartsWith("vbscript:") || probe.StartsWith("data:")) return false;
        return true;
    }

    static string AttrEsc(string s) =>
        s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
}
