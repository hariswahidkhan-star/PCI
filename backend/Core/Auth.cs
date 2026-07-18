using PCI.Backend.Data;

namespace PCI.Backend.Core;

public record AdminCtx(long Id, string Email, string? Name, string Role, string? PermissionsJson, string Status, bool MustChangePw, string? CertScopeJson = null)
{
    public List<string> Perms => Rbac.PermsFor(Role, PermissionsJson);
    public bool IsOwner => Role == "owner";

    /// <summary>Certifications this admin may see and act on. null = unrestricted (all certifications,
    /// the default and the only possible state for owners). A non-empty array restricts every
    /// certification-scoped admin surface — lists filter to these ids, mutations on anything else 403.</summary>
    public long[]? CertScope
    {
        get
        {
            if (IsOwner || string.IsNullOrWhiteSpace(CertScopeJson)) return null;
            try
            {
                var ids = System.Text.Json.JsonSerializer.Deserialize<long[]>(CertScopeJson!);
                return ids is { Length: > 0 } ? ids : null;
            }
            catch { return null; }
        }
    }

    /// <summary>May this admin act on the given certification? Legacy rows with a NULL
    /// certification_id belong to the founding certification (id 1).</summary>
    public bool CanCert(object? certId) =>
        CertScope is not { } s || s.Contains(certId is null ? 1L : Convert.ToInt64(certId));

    /// <summary>SQL fragment (starting with " AND ") restricting a query to this admin's scope, or ""
    /// when unrestricted. The ids come from a parsed long[] — never from request text — so inlining
    /// them is not an injection surface.</summary>
    public string CertFilterSql(string colExpr) =>
        CertScope is { } s ? $" AND COALESCE({colExpr},1) IN ({string.Join(",", s)})" : "";
}

public record UserCtx(long Id, string Email, string? FirstName, string? LastName, string Status);

public static class Settings
{
    public static double Num(Db db, string key, double def)
    {
        var v = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", key);
        return double.TryParse(v, out var n) ? n : def;
    }
    public static bool Bool(Db db, string key, bool def)
    {
        var v = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", key);
        if (v is null) return def;
        return v == "1" || v.Equals("true", StringComparison.OrdinalIgnoreCase);
    }
}

public static class Auth
{
    private static string? Bearer(HttpRequest req)
    {
        var h = req.Headers.Authorization.ToString();
        return h.StartsWith("Bearer ") ? h.Substring(7) : null;
    }

    /// <summary>Bearer admin-session token only. The legacy shared env token has been REMOVED —
    /// every admin must authenticate with their own session so actions are attributable and RBAC applies.</summary>
    public static AdminCtx? AdminFromReq(HttpRequest req, Db db)
    {
        var bearer = Bearer(req);
        if (bearer is null) return null;
        var sess = db.QueryOne("SELECT * FROM admin_sessions WHERE token=? AND expires_at>datetime('now')", Security.Sha(bearer));
        if (sess is null) return null;
        var a = db.QueryOne("SELECT * FROM admin_users WHERE id=?", sess["admin_id"]);
        if (a is not null && (a["status"] as string) == "active") return ToAdmin(a);
        return null;
    }

    public static AdminCtx ToAdmin(Dictionary<string, object?> a) => new(
        Convert.ToInt64(a["id"]), (string)a["email"]!, a["name"] as string, (string)a["role"]!,
        a["permissions"] as string ?? "[]", (string)a["status"]!,
        a.TryGetValue("must_change_pw", out var m) && m is not null && Convert.ToInt64(m) == 1,
        a.TryGetValue("cert_scope", out var cs) ? cs as string : null);

    /// <summary>Bearer student session token. Parity with student() middleware.</summary>
    public static UserCtx? UserFromReq(HttpRequest req, Db db)
    {
        var bearer = Bearer(req);
        if (bearer is null) return null;
        var row = db.QueryOne("SELECT * FROM login_tokens WHERE token=? AND purpose='session' AND expires_at>datetime('now')", Security.Sha(bearer));
        if (row is null) return null;
        var u = db.QueryOne("SELECT * FROM users WHERE id=?", row["user_id"]);
        if (u is null || (u["status"] as string) != "active") return null;
        return new UserCtx(Convert.ToInt64(u["id"]), (string)u["email"]!, u["first_name"] as string, u["last_name"] as string, (string)u["status"]!);
    }
}
