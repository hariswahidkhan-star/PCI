using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI World — participant accounts and the PCI World Passport (Phase 1b; docs/pciworld/PLAN.md).
///
/// A PCI World account is PRACTICE IDENTITY ONLY: it lives in pciworld_users, wholly separate from
/// the platform's `users`, and can never reach exam, entitlement or credential data. Registration
/// and login "claim" the caller's anonymous session — attempts made before signing up become
/// account evidence, so the anonymous-first journey loses nothing.
///
/// The Passport is consent-based evidence, never a credential: each completed attempt is opt-in
/// per item (`passport_visible`), publication requires a verified email AND a chosen display
/// name, the public URL is an opaque revocable token, and the page language is fixed to
/// "verified virtual project experience". Export and delete are self-service.
/// </summary>
public static class WorldAccount
{
    public sealed record UserCtx(long Id, string Email, string? DisplayName, bool EmailVerified, bool PassportPublic);

    public static UserCtx? FromReq(HttpRequest req, Db db)
    {
        var h = req.Headers["X-World-Account"].ToString();
        if (string.IsNullOrWhiteSpace(h)) return null;
        var sess = db.QueryOne("SELECT * FROM pciworld_user_sessions WHERE token=? AND expires_at>datetime('now')", Security.Sha(h));
        if (sess is null) return null;
        var u = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", sess["user_id"]);
        if (u is null || H.Str(u["status"]) != "active") return null;
        return new UserCtx(H.L(u["id"]), H.Str(u["email"])!, H.Str(u["display_name"]),
            H.L(u["email_verified"]) == 1, H.L(u["passport_public"]) == 1);
    }

    // ───────────────────────── core (testable without HTTP) ─────────────────────────

    /// <summary>Create an account and claim the anonymous session's attempts. Returns an error key
    /// or null, plus the new user id and a session token.</summary>
    public static (string? Error, long UserId, string Token) Register(Db db, string email, string password, string? displayName, long? worldSessionId)
    {
        email = email.Trim().ToLowerInvariant();
        if (!email.Contains('@') || email.Length < 6) return ("bad_email", 0, "");
        if (password.Length < 10) return ("weak_password", 0, "");
        if (db.QueryOne("SELECT id FROM pciworld_users WHERE email=?", email) is not null) return ("duplicate_email", 0, "");
        var id = db.ExecuteReturningId("INSERT INTO pciworld_users(email,password_hash,display_name) VALUES(?,?,?)",
            email, BCrypt.Net.BCrypt.HashPassword(password), Trunc(displayName, 80));
        ClaimSession(db, id, worldSessionId);
        return (null, id, MintSession(db, id));
    }

    public static (string? Error, long UserId, string Token) Login(Db db, string email, string password, long? worldSessionId)
    {
        email = email.Trim().ToLowerInvariant();
        var u = db.QueryOne("SELECT * FROM pciworld_users WHERE email=?", email);
        if (u is null) { LoginGuard.BurnTime(password); return ("invalid_credentials", 0, ""); }
        if (LoginGuard.IsLocked(db, "pciworld_users", u["id"])) return ("account_locked", 0, "");
        if (!Security.VerifyPassword(password, u["password_hash"] as string))
        {
            LoginGuard.OnFail(db, "pciworld_users", u["id"]);
            return ("invalid_credentials", 0, "");
        }
        LoginGuard.OnSuccess(db, "pciworld_users", u["id"]);
        if (H.Str(u["status"]) != "active") return ("account_suspended", 0, "");
        var id = H.L(u["id"]);
        ClaimSession(db, id, worldSessionId);
        db.Execute("UPDATE pciworld_users SET last_login_at=datetime('now') WHERE id=?", id);
        return (null, id, MintSession(db, id));
    }

    /// <summary>Adopt the anonymous session's attempts into the account — only unclaimed ones;
    /// attempts already owned by another account never move.</summary>
    public static void ClaimSession(Db db, long userId, long? worldSessionId)
    {
        if (worldSessionId is null) return;
        db.Execute("UPDATE pciworld_attempts SET user_id=? WHERE session_id=? AND user_id IS NULL",
            userId, worldSessionId);
    }

    static string MintSession(Db db, long userId)
    {
        var token = Security.RandomHex(32);
        db.Execute("INSERT INTO pciworld_user_sessions(user_id,token,expires_at) VALUES(?,?, datetime('now','+30 days'))",
            userId, Security.Sha(token));
        return token;
    }

    /// <summary>Publish the Passport: consent + verified email + a display name are all required.
    /// Returns the public path or an error key. Republishing rotates the token (old links die).</summary>
    public static (string? Error, string? Url) PublishPassport(Db db, long userId)
    {
        var u = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", userId);
        if (u is null) return ("not_found", null);
        if (H.L(u["email_verified"]) != 1) return ("email_unverified", null);
        if (string.IsNullOrWhiteSpace(H.Str(u["display_name"]))) return ("no_display_name", null);
        var token = Security.RandomHex(32);
        db.Execute("UPDATE pciworld_users SET passport_public=1, passport_token_sha=? WHERE id=?",
            Security.Sha(token), userId);
        return (null, "/world/p/" + token);
    }

    public static void UnpublishPassport(Db db, long userId) =>
        db.Execute("UPDATE pciworld_users SET passport_public=0, passport_token_sha=NULL WHERE id=?", userId);

    /// <summary>The account's evidence rows (own view — includes hidden items so they can be toggled).</summary>
    public static List<Dictionary<string, object?>> EvidenceRows(Db db, long userId, bool visibleOnly) =>
        db.Query($@"SELECT a.id, a.score, a.profile_key, a.completed_at, a.passport_visible,
                a.result_token_sha, a.result_revoked, v.title, v.industry, v.track, v.difficulty
            FROM pciworld_attempts a
            JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
            WHERE a.user_id=? AND a.status='completed' {(visibleOnly ? "AND a.passport_visible=1" : "")}
            ORDER BY a.completed_at DESC, a.id DESC LIMIT 200", userId);

    /// <summary>Self-service deletion: unlink and de-identify. Attempts stay as anonymous
    /// statistics, but every public surface tied to the identity dies with the account.</summary>
    public static void DeleteAccount(Db db, long userId)
    {
        db.Execute(@"UPDATE pciworld_attempts SET user_id=NULL, passport_visible=0, display_name=NULL,
            result_token_sha=NULL, result_revoked=1 WHERE user_id=?", userId);
        db.Execute("DELETE FROM pciworld_user_sessions WHERE user_id=?", userId);
        db.Execute("DELETE FROM pciworld_user_tokens WHERE user_id=?", userId);
        db.Execute("DELETE FROM pciworld_users WHERE id=?", userId);
    }

    // ───────────────────────── endpoints ─────────────────────────

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);
        bool Enabled() => Settings.Bool(db, "world_enabled", true);
        IResult Disabled() => Results.Json(new { error = "world_disabled" }, statusCode: 403);
        IResult Err(string key, int code, string? msg = null) => Results.Json(new { error = key, message = msg }, statusCode: code);

        var rl = new System.Collections.Concurrent.ConcurrentDictionary<string, (int count, long start)>();
        bool Throttled(string key, int limit, long windowMs = 600_000)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (rl.Count > 20_000)
                foreach (var kv in rl) if (now - kv.Value.start >= windowMs) rl.TryRemove(kv.Key, out _);
            var e = rl.AddOrUpdate(key, (1, now), (_, c) => now - c.start >= windowMs ? (1, now) : (c.count + 1, c.start));
            return e.count > limit;
        }
        string Ip(HttpContext ctx) => ctx.Connection.RemoteIpAddress?.ToString() ?? "?";

        long? WorldSessionId(HttpContext ctx)
        {
            var tok = ctx.Request.Headers["X-World-Session"].ToString();
            if (string.IsNullOrWhiteSpace(tok)) return null;
            var s = db.QueryOne("SELECT id FROM pciworld_sessions WHERE token_sha=?", Security.Sha(tok));
            return s is null ? null : H.L(s["id"]);
        }

        void SendVerification(HttpContext ctx, long userId, string email)
        {
            var token = Security.RandomHex(32);
            db.Execute("DELETE FROM pciworld_user_tokens WHERE user_id=? AND purpose='verify'", userId);
            db.Execute("INSERT INTO pciworld_user_tokens(user_id,purpose,token_sha,expires_at) VALUES(?, 'verify', ?, datetime('now','+2 days'))",
                userId, Security.Sha(token));
            var url = $"{ctx.Request.Scheme}://{ctx.Request.Host}/world/verify-email?t={token}";
            Mailer.Send(db, null, email, "world_verify",
                "Verify your PCI World email",
                $"<p>Welcome to PCI World.</p><p><a href=\"{url}\">Verify your email address</a> (link valid for 48 hours).</p>" +
                $"<p>{WorldPages.OperatedBy}</p>");
        }

        app.MapPost("/api/world/account/register", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("reg|" + Ip(ctx), 10)) return Err("rate_limited", 429);
            var b = await H.Body(ctx.Request);
            var (err, userId, token) = Register(db, H.GetS(b, "email") ?? "", H.GetS(b, "password") ?? "",
                H.GetS(b, "display_name"), WorldSessionId(ctx));
            if (err is not null) return Err(err, err == "duplicate_email" ? 409 : 400,
                err == "weak_password" ? "Use at least 10 characters." : null);
            SendVerification(ctx, userId, (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant());
            log(null, "world_register", $"#{userId}");
            return J(new { ok = true, token, email_verified = false });
        });

        app.MapPost("/api/world/account/login", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("login|" + Ip(ctx), 30)) return Err("rate_limited", 429);
            var b = await H.Body(ctx.Request);
            var (err, userId, token) = Login(db, H.GetS(b, "email") ?? "", H.GetS(b, "password") ?? "", WorldSessionId(ctx));
            if (err is not null) return Err(err, err == "account_locked" ? 429 : err == "account_suspended" ? 403 : 401);
            var me = db.QueryOne("SELECT display_name,email_verified,passport_public FROM pciworld_users WHERE id=?", userId)!;
            return J(new { ok = true, token, display_name = H.Str(me["display_name"]),
                email_verified = H.L(me["email_verified"]) == 1, passport_public = H.L(me["passport_public"]) == 1 });
        });

        app.MapPost("/api/world/account/logout", (HttpContext ctx) =>
        {
            var h = ctx.Request.Headers["X-World-Account"].ToString();
            if (!string.IsNullOrWhiteSpace(h))
                db.Execute("DELETE FROM pciworld_user_sessions WHERE token=?", Security.Sha(h));
            return J(new { ok = true });
        });

        app.MapGet("/world/verify-email", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var t = ctx.Request.Query["t"].ToString();
            var row = t.Length > 0 ? db.QueryOne(@"SELECT * FROM pciworld_user_tokens
                WHERE token_sha=? AND purpose='verify' AND expires_at>datetime('now')", Security.Sha(t)) : null;
            var ok = row is not null;
            if (ok)
            {
                db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", row!["user_id"]);
                db.Execute("DELETE FROM pciworld_user_tokens WHERE id=?", row["id"]);
            }
            return Results.Content(WorldPages.VerifyEmail(db, ok), "text/html; charset=utf-8");
        });

        app.MapPost("/api/world/account/resend-verification", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            if (u.EmailVerified) return J(new { ok = true, already = true });
            if (Throttled("verify|" + u.Id, 3)) return Err("rate_limited", 429);
            SendVerification(ctx, u.Id, u.Email);
            return J(new { ok = true });
        });

        app.MapGet("/api/world/account", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            return J(new { email = u.Email, display_name = u.DisplayName,
                email_verified = u.EmailVerified, passport_public = u.PassportPublic });
        });

        app.MapPost("/api/world/account/profile", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var b = await H.Body(ctx.Request);
            db.Execute("UPDATE pciworld_users SET display_name=? WHERE id=?", Trunc(H.GetS(b, "display_name"), 80), u.Id);
            return J(new { ok = true });
        });

        app.MapPost("/api/world/account/password", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var b = await H.Body(ctx.Request);
            var next = H.GetS(b, "next") ?? "";
            if (next.Length < 10) return Err("weak_password", 400, "Use at least 10 characters.");
            var row = db.QueryOne("SELECT password_hash FROM pciworld_users WHERE id=?", u.Id);
            if (!Security.VerifyPassword(H.GetS(b, "current") ?? "", row?["password_hash"] as string))
                return Err("invalid_credentials", 401);
            db.Execute("UPDATE pciworld_users SET password_hash=? WHERE id=?", BCrypt.Net.BCrypt.HashPassword(next), u.Id);
            db.Execute("DELETE FROM pciworld_user_sessions WHERE user_id=?", u.Id);   // re-login everywhere
            return J(new { ok = true, reauth = true });
        });

        app.MapGet("/api/world/account/export", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var attempts = EvidenceRows(db, u.Id, visibleOnly: false).Select(r => new
            {
                title = H.Str(r["title"]), industry = H.Str(r["industry"]), track = H.Str(r["track"]),
                difficulty = H.Str(r["difficulty"]), score = r["score"], profile = H.Str(r["profile_key"]),
                completed_at = H.Str(r["completed_at"]), passport_visible = H.L(r["passport_visible"]) == 1,
            });
            return J(new { email = u.Email, display_name = u.DisplayName, email_verified = u.EmailVerified,
                passport_public = u.PassportPublic, attempts });
        });

        app.MapPost("/api/world/account/delete", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var b = await H.Body(ctx.Request);
            var row = db.QueryOne("SELECT password_hash FROM pciworld_users WHERE id=?", u.Id);
            if (!Security.VerifyPassword(H.GetS(b, "password") ?? "", row?["password_hash"] as string))
                return Err("invalid_credentials", 401);
            DeleteAccount(db, u.Id);
            log(null, "world_account_delete", $"#{u.Id}");
            return J(new { ok = true, deleted = true });
        });

        // ───────────── passport ─────────────

        app.MapGet("/api/world/passport", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var rows = EvidenceRows(db, u.Id, visibleOnly: false);
            var evidence = rows.Select(r => new
            {
                attempt_id = H.L(r["id"]), title = H.Str(r["title"]), industry = H.Str(r["industry"]),
                track = H.Str(r["track"]), difficulty = H.Str(r["difficulty"]),
                score = r["score"], profile = H.Str(r["profile_key"]),
                completed_at = H.Str(r["completed_at"]), passport_visible = H.L(r["passport_visible"]) == 1,
            });
            return J(new
            {
                display_name = u.DisplayName, email_verified = u.EmailVerified, passport_public = u.PassportPublic,
                completed = rows.Count,
                industries = rows.Select(r => H.Str(r["industry"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count(),
                tracks = rows.Select(r => H.Str(r["track"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count(),
                evidence,
            });
        });

        app.MapPost("/api/world/passport/evidence", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var b = await H.Body(ctx.Request);
            var attemptId = (long)(H.GetNum(b, "attempt_id") ?? 0);
            var visible = H.GetEl(b, "visible") is { ValueKind: JsonValueKind.True };
            var n = db.Execute("UPDATE pciworld_attempts SET passport_visible=? WHERE id=? AND user_id=? AND status='completed'",
                visible ? 1 : 0, attemptId, u.Id);
            return n == 0 ? Err("not_found", 404) : J(new { ok = true });
        });

        app.MapPost("/api/world/passport/publish", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var b = await H.Body(ctx.Request);
            if (H.GetEl(b, "publish") is { ValueKind: JsonValueKind.False })
            {
                UnpublishPassport(db, u.Id);
                return J(new { ok = true, passport_public = false });
            }
            var (err, url) = PublishPassport(db, u.Id);
            if (err is not null) return Err(err, 409, err switch
            {
                "email_unverified" => "Verify your email before publishing a public Passport.",
                "no_display_name" => "Choose the display name that should appear on your public Passport first.",
                _ => null,
            });
            log(null, "world_passport_publish", $"#{u.Id}");
            return J(new { ok = true, passport_public = true, url });
        });

        app.MapGet("/world/p/{token}", (string token) =>
        {
            if (!Enabled()) return Disabled();
            var u = db.QueryOne(@"SELECT * FROM pciworld_users
                WHERE passport_token_sha=? AND passport_public=1 AND status='active'", Security.Sha(token));
            if (u is null) return Results.NotFound();
            var rows = EvidenceRows(db, H.L(u["id"]), visibleOnly: true);
            return Results.Content(WorldPages.PublicPassport(db, H.Str(u["display_name"]) ?? "PCI World participant", rows),
                "text/html; charset=utf-8");
        });

        app.MapGet("/world/account", () => !Enabled() ? Disabled()
            : Results.Content(WorldPages.Account(db), "text/html; charset=utf-8"));
    }

    static string? Trunc(string? s, int n)
    {
        s = s?.Trim();
        if (string.IsNullOrEmpty(s)) return null;
        return s.Length <= n ? s : s[..n];
    }
}
