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
        // Deliberately NO answers_json and NO session_id: this shape feeds the PUBLIC Passport and
        // the PDF, so anything selected here is one careless interpolation away from publication.
        // The data export fetches answers on its own, separate path.
        db.Query($@"SELECT a.id, a.score, a.profile_key, a.completed_at, a.passport_visible,
                a.result_token_sha, a.result_revoked, c.code, a.version,
                v.title, v.industry, v.track, v.difficulty
            FROM pciworld_attempts a
            JOIN pciworld_challenges c ON c.id=a.challenge_id
            JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
            WHERE a.user_id=? AND a.status='completed' {(visibleOnly ? "AND a.passport_visible=1" : "")}
            ORDER BY a.completed_at DESC, a.id DESC LIMIT 200", userId);

    /// <summary>
    /// Erase the account and DE-IDENTIFY everything it leaves behind.
    ///
    /// Detaching `user_id` is not de-identification on its own: `session_id` is a durable
    /// pseudonymous key that still links every one of these attempts to each other, to the
    /// browser holding the raw session token, and to any content report filed from it. Answer
    /// text is personal content the person asked us to delete. Both go. What remains is a
    /// completed-challenge statistic with nothing that points back to anyone (Phase 0 §7).
    ///
    /// Completed scores are deliberately KEPT, unlinked: they are anonymous aggregate evidence of
    /// how a challenge performs, which the content-quality gates depend on.
    /// </summary>
    public static void DeleteAccount(Db db, long userId)
    {
        // Any public link minted from this account's attempts stops resolving first.
        db.Execute(@"UPDATE pciworld_invites SET revoked=1
            WHERE attempt_id IN (SELECT id FROM pciworld_attempts WHERE user_id=?)", userId);
        // Content reports and analytics events lose the session linkage that could re-identify them.
        db.Execute(@"UPDATE pciworld_reports SET session_id=NULL
            WHERE session_id IN (SELECT session_id FROM pciworld_attempts WHERE user_id=?)", userId);
        db.Execute(@"UPDATE pciworld_events SET session_id=NULL
            WHERE session_id IN (SELECT session_id FROM pciworld_attempts WHERE user_id=?)", userId);
        // The browser sessions themselves are removed, then the attempts are stripped.
        db.Execute(@"DELETE FROM pciworld_sessions
            WHERE id IN (SELECT session_id FROM pciworld_attempts WHERE user_id=?)", userId);
        db.Execute(@"UPDATE pciworld_attempts SET user_id=NULL, passport_visible=0, display_name=NULL,
            result_token_sha=NULL, result_revoked=1, answers_json=NULL, session_id=0 WHERE user_id=?", userId);
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
        // Trusted proxy-appended hop — see Security.ClientIp. The socket address behind Render's
        // proxy is identical for every visitor, which would collapse these limits into one bucket.
        string Ip(HttpContext ctx) => Security.ClientIp(ctx);

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
            // Never the request Host header — see WorldUrl: a forged Host would mail the recipient
            // a valid token pointing at an attacker-controlled origin.
            var url = WorldUrl.Abs(ctx.Request, $"/world/verify-email?t={token}");
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

        // ───────────── password reset ─────────────

        app.MapPost("/api/world/account/forgot", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("forgot|" + Ip(ctx), 10)) return Err("rate_limited", 429);
            var b = await H.Body(ctx.Request);
            var email = (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant();
            var u = db.QueryOne("SELECT id FROM pciworld_users WHERE email=? AND status='active'", email);
            if (u is not null)
            {
                var token = Security.RandomHex(32);
                db.Execute("DELETE FROM pciworld_user_tokens WHERE user_id=? AND purpose='reset'", u["id"]);
                db.Execute("INSERT INTO pciworld_user_tokens(user_id,purpose,token_sha,expires_at) VALUES(?, 'reset', ?, datetime('now','+2 hours'))",
                    u["id"], Security.Sha(token));
                // Host-header independent (WorldUrl): the reset token is the account, so the link
                // target must come from configuration, never from the requester.
                var url = WorldUrl.Abs(ctx.Request, $"/world/reset-password?t={token}");
                Mailer.Send(db, null, email, "world_reset", "Reset your PCI World password",
                    $"<p>Someone asked to reset the password for this PCI World account.</p>" +
                    $"<p><a href=\"{url}\">Choose a new password</a> (link valid for 2 hours). If this wasn't you, ignore this email.</p>" +
                    $"<p>{WorldPages.OperatedBy}</p>");
            }
            // Same response whether or not the account exists — no enumeration.
            return J(new { ok = true, message = "If that address has an account, a reset link is on its way." });
        });

        app.MapPost("/api/world/account/reset", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("reset|" + Ip(ctx), 10)) return Err("rate_limited", 429);
            var b = await H.Body(ctx.Request);
            var token = H.GetS(b, "token") ?? "";
            var next = H.GetS(b, "password") ?? "";
            if (next.Length < 10) return Err("weak_password", 400, "Use at least 10 characters.");
            var row = token.Length > 0 ? db.QueryOne(@"SELECT * FROM pciworld_user_tokens
                WHERE token_sha=? AND purpose='reset' AND expires_at>datetime('now')", Security.Sha(token)) : null;
            if (row is null) return Err("invalid_token", 400, "This reset link is invalid or has expired — request a new one.");
            db.Execute("UPDATE pciworld_users SET password_hash=?, failed_logins=0, lockout_until=NULL WHERE id=?",
                BCrypt.Net.BCrypt.HashPassword(next), row["user_id"]);
            db.Execute("DELETE FROM pciworld_user_tokens WHERE user_id=?", row["user_id"]);
            db.Execute("DELETE FROM pciworld_user_sessions WHERE user_id=?", row["user_id"]);   // sign out everywhere
            log(null, "world_password_reset", $"#{H.L(row["user_id"])}");
            return J(new { ok = true });
        });

        app.MapGet("/world/reset-password", () => !Enabled() ? Disabled()
            : Results.Content(WorldPages.ResetPassword(db), "text/html; charset=utf-8"));

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
            // The export is what a person gets under a data-access request, so unlike every other
            // read path it DOES include the answers they gave. Queried here rather than through
            // EvidenceRows, which must stay publication-safe.
            var attempts = db.Query(@"SELECT a.score, a.profile_key, a.completed_at, a.passport_visible,
                    a.answers_json, c.code, v.title, v.industry, v.track, v.difficulty
                FROM pciworld_attempts a
                JOIN pciworld_challenges c ON c.id=a.challenge_id
                JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
                WHERE a.user_id=? AND a.status='completed' ORDER BY a.completed_at DESC, a.id DESC", u.Id)
                .Select(r => new
                {
                    code = H.Str(r["code"]), title = H.Str(r["title"]), industry = H.Str(r["industry"]),
                    track = H.Str(r["track"]), difficulty = H.Str(r["difficulty"]),
                    score = r["score"], profile = H.Str(r["profile_key"]),
                    completed_at = H.Str(r["completed_at"]), passport_visible = H.L(r["passport_visible"]) == 1,
                    answers = H.Str(r["answers_json"]),
                });
            // Content reports are user-authored free text tied to this account's sessions: they
            // belong in the export too, or the export is not a complete copy of their data.
            var reports = db.Query(@"SELECT r.category, r.message, r.status, r.created_at, c.code
                    FROM pciworld_reports r LEFT JOIN pciworld_challenges c ON c.id=r.challenge_id
                    WHERE r.session_id IN (SELECT session_id FROM pciworld_attempts WHERE user_id=?)
                    ORDER BY r.id", u.Id)
                .Select(r => new { challenge = H.Str(r["code"]), category = H.Str(r["category"]),
                                   message = H.Str(r["message"]), status = H.Str(r["status"]),
                                   created_at = H.Str(r["created_at"]) });
            ctx.Response.Headers["Content-Disposition"] = "attachment; filename=\"pciworld-my-data.json\"";
            return J(new { exported_at = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                email = u.Email, display_name = u.DisplayName, email_verified = u.EmailVerified,
                passport_public = u.PassportPublic, attempts, reports });
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
                // Traceability: the challenge code and the immutable published version this attempt
                // was graded against — the record a reader can follow back to the source.
                code = H.Str(r["code"]), version = H.L(r["version"]),
            });
            var me = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", u.Id)!;
            var show = WorldPassport.Disclosure.From(me);
            return J(new
            {
                display_name = u.DisplayName, email_verified = u.EmailVerified, passport_public = u.PassportPublic,
                completed = rows.Count,
                industries = rows.Select(r => H.Str(r["industry"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count(),
                tracks = rows.Select(r => H.Str(r["track"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count(),
                evidence,
                show_scores = show.Scores, show_profiles = show.Profiles, show_dates = show.Dates,
                expires_at = H.Str(me["passport_expires_at"])?.Split(' ')[0],
                expired = WorldPassport.Expired(me),
            });
        });

        // Field-level disclosure and link expiry. Consent is not a single switch: a person can be
        // happy to show WHAT they have practised without publishing their scores.
        app.MapPost("/api/world/passport/disclosure", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var b = await H.Body(ctx.Request);
            foreach (var (key, col) in new[] { ("show_scores", "passport_show_scores"),
                                               ("show_profiles", "passport_show_profiles"),
                                               ("show_dates", "passport_show_dates") })
                if (H.GetBool(b, key) is { } v)
                    db.Execute($"UPDATE pciworld_users SET {col}=? WHERE id=?", v ? 1 : 0, u.Id);

            // expires_in_days: 0 or absent clears the expiry; a positive value sets one. Capped at
            // five years so "expiry" always means something.
            if (H.GetNum(b, "expires_in_days") is { } days)
            {
                if (days <= 0) db.Execute("UPDATE pciworld_users SET passport_expires_at=NULL WHERE id=?", u.Id);
                else
                {
                    var when = DateTime.UtcNow.AddDays(Math.Min(days, 1825)).ToString("yyyy-MM-dd HH:mm:ss");
                    db.Execute("UPDATE pciworld_users SET passport_expires_at=? WHERE id=?", when, u.Id);
                }
            }
            var me = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", u.Id)!;
            var show = WorldPassport.Disclosure.From(me);
            return J(new { ok = true, show_scores = show.Scores, show_profiles = show.Profiles,
                           show_dates = show.Dates, expires_at = H.Str(me["passport_expires_at"])?.Split(' ')[0] });
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

        /// Resolve a published Passport token. Absent, unpublished, suspended and EXPIRED all
        /// collapse to the same NULL — a viewer must not be able to distinguish "never existed"
        /// from "withdrawn", which would leak that a Passport once existed at that address.
        Dictionary<string, object?>? PassportByToken(string token)
        {
            var u = db.QueryOne(@"SELECT * FROM pciworld_users
                WHERE passport_token_sha=? AND passport_public=1 AND status='active'", Security.Sha(token));
            return u is null || WorldPassport.Expired(u) ? null : u;
        }

        app.MapGet("/world/p/{token}", (HttpContext ctx, string token) =>
        {
            if (!Enabled()) return Disabled();
            var u = PassportByToken(token);
            if (u is null) return Results.NotFound();
            var rows = EvidenceRows(db, H.L(u["id"]), visibleOnly: true);
            var verifyUrl = WorldUrl.Abs(ctx.Request, "/world/p/" + token);
            return Results.Content(
                WorldPages.PublicPassport(db, H.Str(u["display_name"]) ?? "PCI World participant", rows,
                    WorldPassport.Disclosure.From(u), verifyUrl, token, H.Str(u["passport_expires_at"])),
                "text/html; charset=utf-8");
        });

        // The same Passport as a one-page document. It carries the verification QR and says plainly
        // that the live record — not the file — is the authority, so a stale copy can never be
        // passed off as current.
        app.MapGet("/world/p/{token}.pdf", (HttpContext ctx, string token) =>
        {
            if (!Enabled()) return Disabled();
            var u = PassportByToken(token);
            if (u is null) return Results.NotFound();
            var rows = EvidenceRows(db, H.L(u["id"]), visibleOnly: true);
            var show = WorldPassport.Disclosure.From(u);
            var doc = new WorldPassport.PassportDoc
            {
                Name = H.Str(u["display_name"]) ?? "PCI World participant",
                VerifyUrl = WorldUrl.Abs(ctx.Request, "/world/p/" + token),
                Completed = rows.Count,
                Industries = rows.Select(r => H.Str(r["industry"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count(),
                Tracks = rows.Select(r => H.Str(r["track"])).Where(s => !string.IsNullOrEmpty(s)).Distinct().Count(),
                IssuedOn = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                ExpiresOn = H.Str(u["passport_expires_at"])?.Split(' ')[0],
                Show = show,
            };
            foreach (var r in rows)
                doc.Rows.Add((H.Str(r["title"]) ?? "",
                    $"{H.Str(r["code"])} · v{H.L(r["version"])}",
                    H.Str(r["industry"]) ?? "", H.Str(r["difficulty"]) ?? "",
                    r["score"] is null ? "" : Convert.ToDouble(r["score"]).ToString("0.#"),
                    H.Str(r["profile_key"])?.Replace('_', ' ') ?? "",
                    (H.Str(r["completed_at"]) ?? "").Split(' ')[0]));
            return Results.File(WorldPassport.Pdf(doc), "application/pdf", "pci-world-passport.pdf");
        });

        // A verification entry point for someone who was handed a Passport: paste the link or the
        // code from the document and land on the live record, or be told plainly that it does not
        // resolve. Recruiters should never have to trust the artefact in their hand.
        app.MapGet("/world/verify", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var q = (ctx.Request.Query["t"].ToString() ?? "").Trim();
            if (q.Length == 0) return Results.Content(WorldPages.VerifyPassport(db, null, null), "text/html; charset=utf-8");
            // Accept a full URL as readily as a bare token — people paste what they were sent.
            var token = q.Contains('/') ? q.TrimEnd('/').Split('/')[^1] : q;
            token = token.Split('?')[0].Trim();
            var u = token.Length >= 16 ? PassportByToken(token) : null;
            if (u is null) return Results.Content(WorldPages.VerifyPassport(db, q, null), "text/html; charset=utf-8");
            return Results.Redirect("/world/p/" + token);
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
