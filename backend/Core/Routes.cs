using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Per-certification application routes (Phase 4). A route is a configurable pathway to a credential —
/// Standard, Founding, Honorary, Sponsored, Complimentary, Fully/Partially Waived, Test User — each
/// enabled, priced and dated independently per certification by the admin. route_key is a clean,
/// stable identifier (no trademark symbols).
/// </summary>
public static class Routes
{
    /// <summary>Enabled routes for a certification. publicOnly filters out internal routes (e.g. Test User)
    /// and any route outside its open window, for public display.</summary>
    public static List<Dictionary<string, object?>> For(Db db, long certId, bool publicOnly = false)
    {
        var rows = db.Query("SELECT * FROM certification_routes WHERE certification_id=? AND enabled=1 ORDER BY sort_order, id", certId);
        if (!publicOnly) return rows;
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        return rows.Where(r =>
        {
            if (!H.B(r["public"])) return false;
            if (H.Str(r["opens_at"]) is { Length: > 0 } o && string.Compare(o, today, StringComparison.Ordinal) > 0) return false;
            if (H.Str(r["closes_at"]) is { Length: > 0 } c && string.Compare(c, today, StringComparison.Ordinal) < 0) return false;
            return true;
        }).ToList();
    }

    /// <summary>A single route by (certification, key), or null.</summary>
    public static Dictionary<string, object?>? Get(Db db, long certId, string routeKey) =>
        db.QueryOne("SELECT * FROM certification_routes WHERE certification_id=? AND route_key=?", certId, routeKey);
}
