using PCI.Backend.Data;

namespace PCI.Backend.Core;

public record AdminCtx(long Id, string Email, string? Name, string Role, string? PermissionsJson, string Status, bool MustChangePw)
{
    public List<string> Perms => Rbac.PermsFor(Role, PermissionsJson);
    public bool IsOwner => Role == "owner";
}

public record UserCtx(long Id, string Email, string? FirstName, string? LastName, string Status, bool Impersonated = false);

/// <summary>An institution-portal login — wholly separate from admin_users and students. Everything a
/// partner session can see is scoped to its own PartnerId; there is no cross-institution read path.</summary>
public record PartnerCtx(long Id, long PartnerId, string Email, string? Name, string Role, string Status, bool MustChangePw);

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
        var sha = Security.Sha(bearer);
        var row = db.QueryOne("SELECT * FROM login_tokens WHERE token=? AND purpose IN ('session','impersonation') AND expires_at>datetime('now')", sha);
        if (row is null) return null;
        var u = db.QueryOne("SELECT * FROM users WHERE id=?", row["user_id"]);
        if (u is null || (u["status"] as string) != "active") return null;
        var impersonated = (row["purpose"] as string) == "impersonation";
        if (impersonated)
        {
            // The impersonation ledger records every page/API the staff session touches ("pages
            // visited, actions taken"), best-effort so auditing never breaks the request itself.
            try
            {
                var sess = db.QueryOne("SELECT id FROM impersonation_sessions WHERE token_sha=?", sha);
                if (sess is not null)
                {
                    db.Execute("UPDATE impersonation_sessions SET last_seen_at=datetime('now') WHERE id=?", sess["id"]);
                    db.Execute("INSERT INTO impersonation_events(session_id,method,path) VALUES(?,?,?)",
                        sess["id"], req.Method, req.Path.ToString());
                }
            }
            catch { }
        }
        return new UserCtx(Convert.ToInt64(u["id"]), (string)u["email"]!, u["first_name"] as string, u["last_name"] as string, (string)u["status"]!,
            impersonated);
    }

    /// <summary>Bearer institution-portal session. Requires an active partner user AND an active,
    /// in-agreement institution — a suspended/terminated institution locks all of its logins out.</summary>
    public static PartnerCtx? PartnerFromReq(HttpRequest req, Db db)
    {
        var bearer = Bearer(req);
        if (bearer is null) return null;
        var sess = db.QueryOne("SELECT * FROM partner_sessions WHERE token=? AND expires_at>datetime('now')", Security.Sha(bearer));
        if (sess is null) return null;
        var pu = db.QueryOne("SELECT * FROM partner_users WHERE id=?", sess["partner_user_id"]);
        if (pu is null || (pu["status"] as string) != "active") return null;
        var partner = db.QueryOne("SELECT status,agreement_end FROM training_partners WHERE id=?", pu["partner_id"]);
        if (partner is null || (partner["status"] as string ?? "active") != "active") return null;
        if (partner["agreement_end"] is string ae && ae.Length > 0 && string.Compare(ae, DateTime.UtcNow.ToString("yyyy-MM-dd"), StringComparison.Ordinal) < 0) return null;
        return new PartnerCtx(Convert.ToInt64(pu["id"]), Convert.ToInt64(pu["partner_id"]), (string)pu["email"]!,
            pu["name"] as string, pu["role"] as string ?? "admin", (string)pu["status"]!,
            pu.TryGetValue("must_change_pw", out var m) && m is not null && Convert.ToInt64(m) == 1);
    }
}
