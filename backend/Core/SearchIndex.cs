using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The site-search index, served dynamically so it follows content edits. The shipped
/// wwwroot/search-index.json is the baseline (every route with its original title/description);
/// any page whose title or meta description has been edited in the admin console is overlaid, so
/// the header search on every page finds pages by their CURRENT names. Cached per content version.
/// </summary>
public static class SearchIndex
{
    sealed record Entry(string u, string t, string d);

    static int _ver = -1;
    static string _json = "[]";
    static readonly object _lock = new();

    public static string Json(Db db, string webRoot)
    {
        var v = PageContent.Version;
        lock (_lock) { if (_ver == v) return _json; }

        List<Entry> entries = new();
        try
        {
            var baseline = Path.Combine(webRoot, "search-index.json");
            if (File.Exists(baseline))
                entries = JsonSerializer.Deserialize<List<Entry>>(File.ReadAllText(baseline)) ?? new();
        }
        catch (Exception e) { Console.Error.WriteLine($"[search] baseline index unreadable: {e.Message}"); }

        try
        {
            var overrides = new Dictionary<string, (string? t, string? d)>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in db.Query("SELECT slug,title,meta_description FROM pages WHERE title IS NOT NULL OR meta_description IS NOT NULL"))
                overrides[H.Str(r["slug"]) ?? ""] = (H.Str(r["title"]), H.Str(r["meta_description"]));
            if (overrides.Count > 0)
                entries = entries.Select(e => overrides.TryGetValue(e.u, out var o)
                    ? new Entry(e.u, o.t is { Length: > 0 } ? o.t : e.t, o.d is { Length: > 0 } ? o.d : e.d)
                    : e).ToList();
        }
        catch (Exception e) { Console.Error.WriteLine($"[search] overlay skipped: {e.Message}"); }

        var json = JsonSerializer.Serialize(entries);
        lock (_lock) { _ver = v; _json = json; }
        return json;
    }
}
