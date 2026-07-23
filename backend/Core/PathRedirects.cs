using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Managed per-path redirects (Admin → SEO → Redirects), applied server-side before page serving —
/// for renamed/retired pages so old URLs keep their SEO value. Exact-path match, GET/HEAD only.
/// Chains are rejected at write time (AdminSeo), so every redirect is a single hop; a same-target
/// row is ignored at lookup as a final loop guard. Cached; any admin edit bumps the version.
/// </summary>
public static class PathRedirects
{
    static int _version = 1;
    public static void Bump() => Interlocked.Increment(ref _version);

    static int _cacheVer = -1;
    static Dictionary<string, (string to, int status)> _map = new(StringComparer.OrdinalIgnoreCase);
    static readonly object _lock = new();

    static Dictionary<string, (string to, int status)> Load(Db db)
    {
        var v = Volatile.Read(ref _version);
        lock (_lock)
        {
            if (_cacheVer == v) return _map;
            var m = new Dictionary<string, (string to, int status)>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in db.Query("SELECT from_path,to_url,status FROM seo_redirects WHERE active=1"))
            {
                var from = Norm(H.Str(r["from_path"]));
                var to = (H.Str(r["to_url"]) ?? "").Trim();
                var st = (int)H.L(r["status"]); if (st is not (301 or 302 or 308 or 410)) st = 301;
                if (from.Length == 0) continue;
                if (st != 410 && to.Length == 0) continue;    // a redirect needs a target; a 410 Gone does not
                m[from] = (to, st);
            }
            _map = m; _cacheVer = v;
            return m;
        }
    }

    /// <summary>Normalise a path for matching: leading slash, no trailing slash (except root), no query.</summary>
    public static string Norm(string? p)
    {
        var s = (p ?? "").Trim();
        if (s.Length == 0) return "";
        var q = s.IndexOf('?'); if (q >= 0) s = s[..q];
        if (!s.StartsWith('/')) s = "/" + s;
        if (s.Length > 1) s = s.TrimEnd('/');
        return s;
    }

    /// <summary>The (target, status) if this request path has an active redirect, else null.</summary>
    public static (string to, int status)? Target(Db db, HttpRequest req)
    {
        if (req.Method != "GET" && req.Method != "HEAD") return null;
        var map = Load(db);
        if (map.Count == 0) return null;
        if (!map.TryGetValue(Norm(req.Path.Value), out var hit)) return null;
        if (hit.status != 410 && Norm(hit.to) == Norm(req.Path.Value)) return null;    // self-redirect guard (never loop); a 410 has no target
        return hit;
    }

    /// <summary>Carry the incoming query string across a redirect without placing it after a fragment.</summary>
    public static string WithQuery(string target, QueryString query)
    {
        if (!query.HasValue) return target;
        var hash = target.IndexOf('#');
        var beforeHash = hash < 0 ? target : target[..hash];
        var fragment = hash < 0 ? "" : target[hash..];
        var separator = beforeHash.Contains('?') ? "&" : "?";
        return beforeHash + separator + query.Value!.TrimStart('?') + fragment;
    }
}
