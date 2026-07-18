using PCI.Backend.Data;

namespace PCI.Backend.Core;

public record AdminCtx(long Id, string Email, string? Name, string Role, string? PermissionsJson, string Status, bool MustChangePw)
{
    public List<string> Perms => Rbac.PermsFor(Role, PermissionsJson);
    public bool IsOwner => Role == "owner";
}

public record UserCtx(long Id, string Email, string? FirstName, string? LastName, string Status, bool Impersonated = false);

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
    public static string Str(Db db, string key, string def)
    {
        var v = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", key);
        return string.IsNullOrEmpty(v) ? def : v!;
    }
    /// <summary>Upsert a setting, provider-safely (delete + insert — no ON CONFLICT dialect differences).</summary>
    public static void Put(Db db, string key, string? value)
    {
        db.Execute("DELETE FROM site_settings WHERE skey=?", key);
        db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", key, value ?? "");
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
        a.TryGetValue("must_change_pw", out var m) && m is not null && Convert.ToInt64(m) == 1);

    /// <summary>Bearer student session token. Parity with student() middleware.</summary>
    public static UserCtx? UserFromReq(HttpRequest req, Db db)
    {
        var bearer = Bearer(req);
        if (bearer is null) return null;
        // An 'impersonation' token is a short-lived staff session minted by an authorised admin
        // ("view as student"): same read access as the student, flagged so the UI shows a permanent
        // banner and sensitive endpoints can refuse it.
        var row = db.QueryOne("SELECT * FROM login_tokens WHERE token=? AND purpose IN ('session','impersonation') AND expires_at>datetime('now')", Security.Sha(bearer));
        if (row is null) return null;
        var u = db.QueryOne("SELECT * FROM users WHERE id=?", row["user_id"]);
        if (u is null || (u["status"] as string) != "active") return null;
        return new UserCtx(Convert.ToInt64(u["id"]), (string)u["email"]!, u["first_name"] as string, u["last_name"] as string, (string)u["status"]!,
            (row["purpose"] as string) == "impersonation");
    }
}
