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

public record UserCtx(long Id, string Email, string? FirstName, string? LastName, string Status, bool Impersonated = false);

/// <summary>A resolved "view as student" (impersonation) bearer token — a staff member acting with a
/// student's READ access. Carries the ledger session id so a blocked mutation can still be audited.
/// <c>SessionId == 0</c> means the token is a valid, unexpired impersonation token whose ledger row
/// could not be located (still treated as read-only — fail closed).</summary>
public record ImpersonationRef(long SessionId, long AdminId, long UserId, string TokenSha);

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
    /// <summary>Upsert a setting, provider-safely (delete + insert — no ON CONFLICT dialect differences).
    /// The pair runs in one transaction so concurrent writers can't interleave (duplicate rows / unique
    /// violations) and a crash between the statements can't lose the setting.</summary>
    public static void Put(Db db, string key, string? value) => db.Transaction(() =>
    {
        db.Execute("DELETE FROM site_settings WHERE skey=?", key);
        db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", key, value ?? "");
    });
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

    /// <summary>Resolve an <b>impersonation</b> ("view as student") bearer token to its ledger session,
    /// or null when the request carries no bearer, a non-impersonation token, or an expired token. Used by
    /// the central read-only guard so a support session can never reach a state-changing endpoint even if
    /// that endpoint forgot its own check. Cheap when there is no bearer (returns immediately).</summary>
    public static ImpersonationRef? ImpersonationToken(HttpRequest req, Db db)
    {
        var bearer = Bearer(req);
        if (bearer is null) return null;
        var sha = Security.Sha(bearer);
        var tok = db.QueryOne("SELECT user_id FROM login_tokens WHERE token=? AND purpose='impersonation' AND expires_at>datetime('now')", sha);
        if (tok is null) return null;
        var sess = db.QueryOne("SELECT id,admin_id,user_id FROM impersonation_sessions WHERE token_sha=? AND ended_at IS NULL", sha);
        return sess is null
            ? new ImpersonationRef(0, 0, Convert.ToInt64(tok["user_id"]), sha)
            : new ImpersonationRef(Convert.ToInt64(sess["id"]), Convert.ToInt64(sess["admin_id"]), Convert.ToInt64(sess["user_id"]), sha);
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

/// <summary>
/// Anti-brute-force + anti-enumeration for the password login endpoints. Complements the per-IP rate
/// limiter with a PER-ACCOUNT lockout (so rotating IPs cannot brute one account) and a constant-time
/// path for unknown accounts (so response timing does not reveal which emails exist).
/// Table names are compile-time constants ("users"/"admin_users"/"partner_users"), never request input.
/// </summary>
public static class LoginGuard
{
    const int MaxFails = 10;      // consecutive wrong passwords before a temporary lock
    const int LockMinutes = 15;   // lock duration; auto-clears on expiry or a correct password

    // A real cost-11 bcrypt hash generated at boot; verifying a submitted password against it costs the
    // same as a genuine check, so a login for a non-existent account takes the same time as a real one.
    static readonly string DummyHash = BCrypt.Net.BCrypt.HashPassword("pci-login-timing-equaliser-v1");

    /// <summary>Run a throwaway bcrypt verify to equalise timing when the account does not exist.</summary>
    public static void BurnTime(string? password)
    {
        try { BCrypt.Net.BCrypt.Verify(password ?? "x", DummyHash); } catch { }
    }

    /// <summary>True while this account is temporarily locked out after too many failures.</summary>
    public static bool IsLocked(Db db, string table, object? id)
    {
        if (id is null) return false;
        return db.Scalar<long>($"SELECT COUNT(*) FROM {table} WHERE id=? AND lockout_until IS NOT NULL AND lockout_until > datetime('now')", id) > 0;
    }

    /// <summary>Record a failed password attempt; lock the account once the threshold is reached.</summary>
    public static void OnFail(Db db, string table, object? id)
    {
        if (id is null) return;
        try
        {
            db.Execute($"UPDATE {table} SET failed_logins = COALESCE(failed_logins,0) + 1 WHERE id=?", id);
            if (db.Scalar<long>($"SELECT COALESCE(failed_logins,0) FROM {table} WHERE id=?", id) >= MaxFails)
                db.Execute($"UPDATE {table} SET lockout_until = datetime('now','+{LockMinutes} minutes'), failed_logins = 0 WHERE id=?", id);
        }
        catch { }
    }

    /// <summary>Clear the failure counter on a correct password.</summary>
    public static void OnSuccess(Db db, string table, object? id)
    {
        if (id is null) return;
        try { db.Execute($"UPDATE {table} SET failed_logins = 0, lockout_until = NULL WHERE id=?", id); } catch { }
    }
}
