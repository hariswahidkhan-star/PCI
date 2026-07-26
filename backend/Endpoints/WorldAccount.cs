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

    /// <summary>The World session cookie (journey repair P0-07, transitional). HttpOnly + Strict:
    /// scripts cannot read it and cross-site requests never carry it, which is the CSRF posture
    /// until the header token is retired and a full CSRF token ships with the React app.</summary>
    public const string SessionCookie = "pciworld_sess";

    public static void SetSessionCookie(HttpContext ctx, string token) =>
        ctx.Response.Cookies.Append(SessionCookie, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = ctx.Request.IsHttps ||
                     string.Equals(ctx.Request.Headers["X-Forwarded-Proto"].ToString(), "https", StringComparison.OrdinalIgnoreCase),
            SameSite = SameSiteMode.Strict,
            Path = "/",
            MaxAge = TimeSpan.FromDays(30),
        });

    public static UserCtx? FromReq(HttpRequest req, Db db)
    {
        var h = req.Headers["X-World-Account"].ToString();
        // Transitional dual acceptance (P0-07): the explicit header wins; the HttpOnly cookie set
        // at login/register/handoff authenticates when no header is present, so the participant
        // app can migrate off localStorage without a breaking cutover.
        if (string.IsNullOrWhiteSpace(h)) h = req.Cookies[SessionCookie] ?? "";
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
        // Canonical identity from the moment the account exists (P0-00): a standalone World
        // registration creates its canonical users/student_profiles pair immediately (same bcrypt
        // hash, so the one password works on both products); an email already registered on the
        // platform is quarantined in the map, never silently merged.
        try { WorldIdentity.MapOne(db, id); } catch { /* mapping must never break registration */ }
        return (null, id, MintSession(db, id));
    }

    public static (string? Error, long UserId, string Token) Login(Db db, string email, string password, long? worldSessionId)
    {
        email = email.Trim().ToLowerInvariant();
        var u = db.QueryOne("SELECT * FROM pciworld_users WHERE email=?", email);

        // Legacy World credential first (transitional back-compat while pciworld_users remains).
        if (u is not null)
        {
            if (LoginGuard.IsLocked(db, "pciworld_users", u["id"])) return ("account_locked", 0, "");
            if (Security.VerifyPassword(password, u["password_hash"] as string))
            {
                LoginGuard.OnSuccess(db, "pciworld_users", u["id"]);
                if (H.Str(u["status"]) != "active") return ("account_suspended", 0, "");
                var id = H.L(u["id"]);
                ClaimSession(db, id, worldSessionId);
                db.Execute("UPDATE pciworld_users SET last_login_at=datetime('now') WHERE id=?", id);
                return (null, id, MintSession(db, id));
            }
        }

        // Canonical PCI credentials (journey repair P0-00/P0-02): the same email/password a student
        // uses on the PCI AI portal signs them into PCI World — one credential authority, no second
        // password. Verification happens against `users`; on success the World account is linked or
        // created through the same guarded bridge the portal SSO uses (an email held by a DIFFERENT
        // student is refused, never hijacked). This also makes a portal-side password change
        // effective here: the canonical hash is the one that wins.
        var cu = db.QueryOne("SELECT * FROM users WHERE lower(email)=?", email);
        if (cu is not null && !LoginGuard.IsLocked(db, "users", cu["id"]) &&
            Security.VerifyPassword(password, cu["password_hash"] as string))
        {
            if (H.Str(cu["status"]) != "active") return ("account_suspended", 0, "");
            LoginGuard.OnSuccess(db, "users", cu["id"]);
            var (lerr, wid) = LinkStudent(db, H.L(cu["id"]), email,
                ($"{H.Str(cu["first_name"])} {H.Str(cu["last_name"])}").Trim());
            if (lerr is not null) return (lerr, 0, "");
            ClaimSession(db, wid, worldSessionId);
            db.Execute("UPDATE pciworld_users SET last_login_at=datetime('now') WHERE id=?", wid);
            return (null, wid, MintSession(db, wid));
        }

        // Failure: one generic answer (no enumeration), counted against whichever record exists so
        // central lockout cannot be bypassed by alternating product login pages.
        if (u is null && cu is null) { LoginGuard.BurnTime(password); return ("invalid_credentials", 0, ""); }
        if (u is not null) LoginGuard.OnFail(db, "pciworld_users", u["id"]);
        else LoginGuard.OnFail(db, "users", cu!["id"]);
        return ("invalid_credentials", 0, "");
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

    /// <summary>Mint a one-time, two-minute cross-surface handoff code (journey repair P0-02).
    /// The RAW code is returned once and never stored; only the allow-listed return path rides
    /// along. This replaces handing a reusable 30-day bearer token through the portal origin.</summary>
    public static string CreateHandoff(Db db, long worldUserId, string? returnTo = null)
    {
        var code = Security.RandomHex(32);
        db.Execute(@"INSERT INTO pciworld_handoff_codes(code_sha,world_user_id,return_to,expires_at)
            VALUES(?,?,?, datetime('now','+2 minutes'))",
            Security.Sha(code), worldUserId, AllowedReturnTo(returnTo));
        return code;
    }

    /// <summary>Redeem a handoff code EXACTLY once: the consumption UPDATE is the lock, so a
    /// concurrent replay races into 0 rows and learns nothing. Expired, replayed and never-existed
    /// are deliberately indistinguishable.</summary>
    public static (string? Error, long UserId, string? ReturnTo) RedeemHandoff(Db db, string? code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length < 32) return ("invalid_code", 0, null);
        var sha = Security.Sha(code);
        var n = db.Execute(@"UPDATE pciworld_handoff_codes SET consumed_at=datetime('now')
            WHERE code_sha=? AND consumed_at IS NULL AND expires_at>datetime('now')", sha);
        if (n == 0) return ("invalid_code", 0, null);
        var row = db.QueryOne("SELECT world_user_id, return_to FROM pciworld_handoff_codes WHERE code_sha=?", sha)!;
        return (null, H.L(row["world_user_id"]), H.Str(row["return_to"]));
    }

    /// <summary>Relative, same-origin World destinations only — never an open redirect.</summary>
    static string AllowedReturnTo(string? p) =>
        p is not null && p.StartsWith("/world") && !p.StartsWith("//") && !p.Contains("://")
            ? (p.Length <= 128 ? p : p[..128]) : "/world/account";

    /// <summary>
    /// Verify the password for a sensitive World action (delete, password change). The canonical
    /// PCI credential is the authority (journey repair P0-03): accounts created through the portal
    /// bridge hold a random World hash the person never saw, so their REAL password — the one they
    /// use on the platform — must work here. The legacy World hash is still accepted
    /// transitionally for standalone accounts.
    /// </summary>
    public static bool VerifyAccountPassword(Db db, long worldUserId, string? password)
    {
        if (string.IsNullOrEmpty(password)) return false;
        var row = db.QueryOne("SELECT password_hash, student_user_id FROM pciworld_users WHERE id=?", worldUserId);
        if (row is null) return false;
        if (Security.VerifyPassword(password, row["password_hash"] as string)) return true;
        var canonical = WorldIdentity.CanonicalFor(db, worldUserId)
                        ?? (row["student_user_id"] is null ? (long?)null : H.L(row["student_user_id"]));
        if (canonical is null) return false;
        var cu = db.QueryOne("SELECT password_hash FROM users WHERE id=? AND status='active'", canonical);
        return cu is not null && Security.VerifyPassword(password, cu["password_hash"] as string);
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

    /// <summary>
    /// Find-or-create the PCI World account behind a student sign-in — the one-login path from the
    /// student portal to the Passport. Resolution order: an account already linked to this student;
    /// then a standalone account with the student's own email, which is adopted (same person, and
    /// the portal already delivers exam and credential mail to that address, so it counts as
    /// verified); otherwise a fresh account is created, verified, with no usable password — the
    /// student portal login IS the login. An email linked to a DIFFERENT student is refused, never
    /// hijacked. Returns an error key or the world user id.
    /// </summary>
    public static (string? Error, long UserId) LinkStudent(Db db, long studentId, string email, string? displayName)
    {
        email = email.Trim().ToLowerInvariant();
        var u = db.QueryOne("SELECT * FROM pciworld_users WHERE student_user_id=?", studentId);
        if (u is null)
        {
            var byEmail = db.QueryOne("SELECT * FROM pciworld_users WHERE email=?", email);
            if (byEmail is null)
            {
                var fresh = db.ExecuteReturningId(
                    "INSERT INTO pciworld_users(email,password_hash,display_name,email_verified,student_user_id) VALUES(?,?,?,1,?)",
                    email, BCrypt.Net.BCrypt.HashPassword(Security.RandomHex(24)), Trunc(displayName, 80), studentId);
                try { WorldIdentity.MapOne(db, fresh); WorldIdentity.EnsureParticipant(db, studentId); } catch { }
                return (null, fresh);
            }
            var linked = H.L(byEmail["student_user_id"]);
            if (linked != 0 && linked != studentId) return ("email_in_use", 0);
            db.Execute("UPDATE pciworld_users SET student_user_id=?, email_verified=1 WHERE id=?", studentId, byEmail["id"]);
            u = byEmail;
        }
        if (H.Str(u["status"]) != "active") return ("account_suspended", 0);
        // The bridge proved the canonical identity (the student is signed in): record the mapping
        // and the participation row keyed by canonical users.id (P0-00).
        try { WorldIdentity.MapOne(db, H.L(u["id"])); WorldIdentity.EnsureParticipant(db, studentId); } catch { }
        return (null, H.L(u["id"]));
    }

    /// <summary>The account's evidence rows (own view — includes hidden items so they can be toggled).
    /// Paginated (journey repair P1-06): the page size is a window, never the truth — totals come
    /// from <see cref="EvidenceStats"/> so an account with 1,000 attempts counts 1,000.</summary>
    public static List<Dictionary<string, object?>> EvidenceRows(Db db, long userId, bool visibleOnly,
        int limit = 200, int offset = 0) =>
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
            ORDER BY a.completed_at DESC, a.id DESC
            LIMIT {Math.Clamp(limit, 1, 500)} OFFSET {Math.Max(0, offset)}", userId);

    public sealed record Stats(long Completed, long Industries, long Tracks);

    /// <summary>Whole-history aggregates straight from SQL — independent of any page of evidence
    /// (P1-06). These feed the account view, the public Passport and the PDF alike.</summary>
    public static Stats EvidenceStats(Db db, long userId, bool visibleOnly)
    {
        var row = db.QueryOne($@"SELECT COUNT(*) AS c,
                COUNT(DISTINCT v.industry) AS i, COUNT(DISTINCT v.track) AS t
            FROM pciworld_attempts a
            JOIN pciworld_challenge_versions v ON v.challenge_id=a.challenge_id AND v.version=a.version
            WHERE a.user_id=? AND a.status='completed' {(visibleOnly ? "AND a.passport_visible=1" : "")}", userId)!;
        return new(H.L(row["c"]), H.L(row["i"]), H.L(row["t"]));
    }

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
        // The Passport photograph is PII sitting in the object store, not just a row: erase the
        // stored bytes themselves, not merely the reference to them.
        var photoRef = H.Str(db.QueryOne("SELECT passport_photo_ref FROM pciworld_users WHERE id=?", userId)
            ?["passport_photo_ref"]);
        if (!string.IsNullOrEmpty(photoRef)) Storage.DeleteOne(photoRef);
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
        db.Execute("DELETE FROM pciworld_handoff_codes WHERE world_user_id=?", userId);
        // WORLD-ONLY scope (journey repair P0-03): the participation record goes; the canonical
        // PCI identity, student profile and every platform record survive untouched. The map row
        // stays as the audit ledger of what was deleted and when it had been linked.
        var canonical = WorldIdentity.CanonicalFor(db, userId);
        if (canonical is not null)
            db.Execute("DELETE FROM pciworld_participants WHERE user_id=?", canonical);
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
            SetSessionCookie(ctx, token);
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
            SetSessionCookie(ctx, token);
            return J(new { ok = true, token, display_name = H.Str(me["display_name"]),
                email_verified = H.L(me["email_verified"]) == 1, passport_public = H.L(me["passport_public"]) == 1 });
        });

        app.MapPost("/api/world/account/logout", (HttpContext ctx) =>
        {
            // Revoke BOTH carriers of the session (P0-07): the header token and the HttpOnly
            // cookie, then clear the cookie so a shared browser holds nothing after sign-out.
            foreach (var tok in new[] { ctx.Request.Headers["X-World-Account"].ToString(),
                                        ctx.Request.Cookies[SessionCookie] ?? "" })
                if (!string.IsNullOrWhiteSpace(tok))
                    db.Execute("DELETE FROM pciworld_user_sessions WHERE token=?", Security.Sha(tok));
            ctx.Response.Cookies.Delete(SessionCookie, new CookieOptions { Path = "/" });
            return J(new { ok = true });
        });

        // Deliberate verification (journey repair P1-03): the GET only LOOKS — mail scanners and
        // link-preview bots that prefetch the URL can no longer consume the token. State changes
        // happen exclusively in the POST below, on the person's explicit button press.
        app.MapGet("/world/verify-email", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var t = ctx.Request.Query["t"].ToString();
            var row = t.Length > 0 ? db.QueryOne(@"SELECT * FROM pciworld_user_tokens
                WHERE token_sha=? AND purpose='verify' AND expires_at>datetime('now')", Security.Sha(t)) : null;
            var state = "invalid";
            if (row is not null)
            {
                var u = db.QueryOne("SELECT email_verified FROM pciworld_users WHERE id=?", row["user_id"]);
                state = u is not null && H.L(u["email_verified"]) == 1 ? "already" : "confirm";
            }
            return Results.Content(WorldPages.VerifyEmail(db, state), "text/html; charset=utf-8");
        });

        app.MapPost("/api/world/account/verify-email", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("verifygo|" + Ip(ctx), 20)) return Err("rate_limited", 429);
            var b = await H.Body(ctx.Request);
            var t = H.GetS(b, "token") ?? "";
            var row = t.Length > 0 ? db.QueryOne(@"SELECT * FROM pciworld_user_tokens
                WHERE token_sha=? AND purpose='verify' AND expires_at>datetime('now')", Security.Sha(t)) : null;
            if (row is null) return Err("invalid_token", 400,
                "That link is no longer valid — request a new verification email from your account page.");
            // Single use, race-safe: the DELETE is the lock; a concurrent duplicate deletes 0 rows.
            var n = db.Execute("DELETE FROM pciworld_user_tokens WHERE id=?", row["id"]);
            if (n == 0) return Err("invalid_token", 400);
            db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", row["user_id"]);
            log(null, "world_email_verified", $"#{H.L(row["user_id"])}");
            return J(new { ok = true });
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

        // The participant dashboard aggregate (P1-01/§9): one call, one server-computed primary
        // action, whole-history progress — never an N+1 waterfall from the client.
        app.MapGet("/api/world/me/dashboard", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var s = WorldDashboard.Build(db, u, DateTime.UtcNow);
            return J(new
            {
                display_name = u.DisplayName,
                email_verified = u.EmailVerified,
                primary_action = s.PrimaryAction,
                primary_attempt_id = s.PrimaryAttemptId,
                primary_code = s.PrimaryCode,
                today = new
                {
                    period_id = s.TodayState.PeriodId, day = s.TodayState.DayKey,
                    timezone = s.TodayState.Timezone, changes_at = s.TodayState.ChangesAtUtc,
                    paused = s.TodayState.Paused, code = s.TodayState.Code, title = s.TodayState.Title,
                    difficulty = s.TodayState.Difficulty, est_minutes = s.TodayState.EstMinutes,
                    version = s.TodayState.Version, state = s.TodayState.State, attempt_id = s.TodayState.AttemptId,
                },
                passport = new
                {
                    state = s.Passport.State, is_public = s.Passport.Public, expires_at = s.Passport.ExpiresAt,
                    email_verified = s.Passport.EmailVerified, has_display_name = s.Passport.HasDisplayName,
                    visible_evidence = s.Passport.VisibleEvidence,
                },
                progress = new { completed = s.Progress.Completed, industries = s.Progress.Industries, tracks = s.Progress.Tracks },
                recent = s.RecentResults.Select(r => new
                {
                    attempt_id = H.L(r["id"]), code = H.Str(r["code"]), version = H.L(r["version"]),
                    title = H.Str(r["title"]), difficulty = H.Str(r["difficulty"]), score = r["score"],
                    profile = H.Str(r["profile_key"]), completed_at = H.Str(r["completed_at"]),
                    passport_visible = H.L(r["passport_visible"]) == 1,
                }),
                in_progress = s.InProgress.Select(r => new
                {
                    attempt_id = H.L(r["id"]), code = H.Str(r["code"]), version = H.L(r["version"]),
                    title = H.Str(r["title"]), updated_at = H.Str(r["updated_at"]),
                }),
                recommended = s.RecommendedCode is null ? null : new { code = s.RecommendedCode, title = s.RecommendedTitle },
            });
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
            // Canonical credential is the authority (P0-03): a bridge-created account's hidden
            // random World hash can never be the only key to this door.
            if (!VerifyAccountPassword(db, u.Id, H.GetS(b, "current")))
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
            // Step-up via the canonical PCI credential (P0-03): the password the person actually
            // knows confirms deletion; bridge-created accounts are no longer undeletable.
            if (!VerifyAccountPassword(db, u.Id, H.GetS(b, "password")))
                return Err("invalid_credentials", 401);
            DeleteAccount(db, u.Id);
            log(null, "world_account_delete", $"#{u.Id}");
            return J(new { ok = true, deleted = true });
        });

        // ───────────── student-login SSO (portal → Passport) ─────────────
        //
        // A signed-in STUDENT reaches their Passport without a second sign-in (owner decision).
        // The realms stay separate — this endpoint authenticates on the student side of the fence,
        // links or creates the matching world account once (LinkStudent), and mints an ordinary
        // PCI World session for the browser to carry to /world/account. World code still never
        // reads exam, entitlement or credential data.
        app.MapPost("/api/me/world-passport/sso", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var s = Auth.UserFromReq(ctx.Request, db);
            if (s is null) return Err("no_token", 401);
            // A support "view as student" session must never mint the student's Passport session:
            // the Passport is consent-controlled personal evidence, not case data.
            if (s.Impersonated) return Err("impersonation_readonly", 403, "This action is disabled in support view.");
            var (err, worldId) = LinkStudent(db, s.Id, s.Email, ($"{s.FirstName} {s.LastName}").Trim());
            if (err is not null) return Err(err, err == "email_in_use" ? 409 : 403, err == "email_in_use"
                ? "A PCI World account with your email address is linked to a different sign-in — contact support."
                : null);
            // Claim the browser's CURRENT anonymous World session too (journey repair P0-01): the
            // portal bridge used to link the account without adopting the anonymous work sitting in
            // this very browser, so challenges completed before pressing "open my Passport" never
            // appeared in it. Claim-only-unowned, identical to register/login. The portal client
            // passes the anonymous token in the body (its typed wrapper sets no custom headers).
            var body = await H.Body(ctx.Request);
            var sessionId = WorldSessionId(ctx);
            if (sessionId is null)
            {
                var anon = H.GetS(body, "world_session");
                if (!string.IsNullOrWhiteSpace(anon))
                {
                    var row = db.QueryOne("SELECT id FROM pciworld_sessions WHERE token_sha=?", Security.Sha(anon));
                    if (row is not null) sessionId = H.L(row["id"]);
                }
            }
            ClaimSession(db, worldId, sessionId);
            // One-time handoff CODE, not a reusable bearer token (journey repair P0-02): the code
            // rides in the URL FRAGMENT (never sent to the server or its logs), lives two minutes,
            // and dies at first redemption. The World page exchanges it for its own session, then
            // continues to the allow-listed destination the caller asked for (deep entry into
            // today's challenge from the portal Overview, for example).
            var code = CreateHandoff(db, worldId, H.GetS(body, "return_to"));
            log(s.Id, "world_passport_sso", $"student #{s.Id} → world #{worldId}");
            return J(new { ok = true, url = "/world/account#h=" + code });
        });

        // Redeem a one-time handoff code for a World session. Deliberately quiet on failure:
        // expired, replayed and never-existed all answer the same 401.
        app.MapPost("/api/world/account/handoff", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("handoff|" + Ip(ctx), 20)) return Err("rate_limited", 429);
            var b = await H.Body(ctx.Request);
            var (err, worldId, returnTo) = RedeemHandoff(db, H.GetS(b, "code"));
            if (err is not null) return Err("invalid_code", 401,
                "That sign-in link has expired or was already used — reopen your Passport from the student portal.");
            var u = db.QueryOne("SELECT * FROM pciworld_users WHERE id=? AND status='active'", worldId);
            if (u is null) return Err("invalid_code", 401);
            ClaimSession(db, worldId, WorldSessionId(ctx));
            var token = MintSession(db, worldId);
            SetSessionCookie(ctx, token);
            log(null, "world_handoff_redeem", $"world #{worldId}");
            return J(new { ok = true, token, return_to = returnTo ?? "/world/account",
                display_name = H.Str(u["display_name"]),
                email_verified = H.L(u["email_verified"]) == 1, passport_public = H.L(u["passport_public"]) == 1 });
        });

        // ───────────── passport ─────────────

        app.MapGet("/api/world/passport", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            // Whole-history statistics from SQL, a paged window of evidence (P1-06): an account
            // with 1,000 completed challenges reports 1,000 and can reach every one of them.
            var offset = (int)(H.QL(ctx, "offset") ?? 0);
            var stats = EvidenceStats(db, u.Id, visibleOnly: false);
            var rows = EvidenceRows(db, u.Id, visibleOnly: false, limit: 200, offset: offset);
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
                completed = stats.Completed,
                industries = stats.Industries,
                tracks = stats.Tracks,
                evidence_total = stats.Completed,
                evidence_offset = offset,
                evidence,
                show_scores = show.Scores, show_profiles = show.Profiles, show_dates = show.Dates,
                expires_at = H.Str(me["passport_expires_at"])?.Split(' ')[0],
                expired = WorldPassport.Expired(me),
                has_photo = !string.IsNullOrEmpty(H.Str(me["passport_photo_ref"])),
            });
        });

        // The optional Passport photograph. Uploading is the consent — it exists only because the
        // owner chose it, it is served publicly only through the published token, and removing it
        // (or the account) erases the stored bytes, not just the reference. Images only, through
        // Core/Storage's sniff/size/traversal guards like every other binary artefact.
        app.MapPost("/api/world/passport/photo", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var b = await H.Body(ctx.Request);
            var old = H.Str(db.QueryOne("SELECT passport_photo_ref FROM pciworld_users WHERE id=?", u.Id)
                ?["passport_photo_ref"]);
            if (H.GetEl(b, "remove") is { ValueKind: JsonValueKind.True })
            {
                if (!string.IsNullOrEmpty(old)) Storage.DeleteOne(old);
                db.Execute("UPDATE pciworld_users SET passport_photo_ref=NULL, passport_photo_mime=NULL WHERE id=?", u.Id);
                return J(new { ok = true, has_photo = false });
            }
            if (Throttled("photo|" + u.Id, 10)) return Err("rate_limited", 429);
            var (bytes, mime, err) = Storage.DecodeDataUri(H.GetS(b, "photo"));
            if (err is not null) return Err(err, 400, err == "file_too_large" ? "Use an image under 3 MB." : null);
            if (!mime.StartsWith("image/")) return Err("file_type_not_allowed", 400, "Use a JPEG, PNG or WebP image.");
            var obj = Storage.Put(bytes!, mime, "world-passport");
            // Content-addressed store: replacing with the same picture yields the same reference,
            // in which case the "old" file IS the new one and must not be deleted.
            if (!string.IsNullOrEmpty(old) && old != obj.Reference) Storage.DeleteOne(old);
            db.Execute("UPDATE pciworld_users SET passport_photo_ref=?, passport_photo_mime=? WHERE id=?",
                obj.Reference, mime, u.Id);
            log(null, "world_passport_photo", $"#{u.Id}");
            return J(new { ok = true, has_photo = true });
        });

        // The owner's own preview. Authenticated by the X-World-Account header — which an <img>
        // navigation never sends — so the account page fetches it as a blob.
        app.MapGet("/api/world/passport/photo", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var r = db.QueryOne("SELECT passport_photo_ref, passport_photo_mime FROM pciworld_users WHERE id=?", u.Id);
            var got = Storage.Get(H.Str(r?["passport_photo_ref"]));
            if (got is null || got.Value.bytes is null) return Err("not_found", 404);
            return Results.File(got.Value.bytes!, H.Str(r?["passport_photo_mime"]) ?? got.Value.mime);
        });

        // Field-level disclosure and link expiry. Consent is not a single switch: a person can be
        // happy to show WHAT they have practised without publishing their scores.
        app.MapPost("/api/world/passport/disclosure", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var u = FromReq(ctx.Request, db);
            if (u is null) return Err("no_token", 401);
            var b = await H.Body(ctx.Request);
            // Independent fields (P0-06): absent keys leave stored values — including the link
            // expiry — untouched. Sending expires_in_days is the ONLY way to change the expiry;
            // 0 clears it deliberately, a positive value sets one (capped at five years).
            WorldPassport.ApplyDisclosure(db, u.Id,
                H.GetBool(b, "show_scores"), H.GetBool(b, "show_profiles"), H.GetBool(b, "show_dates"),
                H.GetNum(b, "expires_in_days"));
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
            var stats = EvidenceStats(db, H.L(u["id"]), visibleOnly: true);
            var verifyUrl = WorldUrl.Abs(ctx.Request, "/world/p/" + token);
            return Results.Content(
                WorldPages.PublicPassport(db, H.Str(u["display_name"]) ?? "PCI World participant", rows,
                    WorldPassport.Disclosure.From(u), verifyUrl, token, H.Str(u["passport_expires_at"]),
                    string.IsNullOrEmpty(H.Str(u["passport_photo_ref"])) ? null : $"/world/p/{token}/photo",
                    stats.Completed, stats.Industries, stats.Tracks),
                "text/html; charset=utf-8");
        });

        // The Passport photograph, addressed by the same revocable token as the page itself:
        // unpublishing, rotating and expiry all cut off the image at the same instant they cut
        // off the record it belongs to.
        app.MapGet("/world/p/{token}/photo", (string token) =>
        {
            if (!Enabled()) return Disabled();
            var u = PassportByToken(token);
            if (u is null) return Results.NotFound();
            var got = Storage.Get(H.Str(u["passport_photo_ref"]));
            if (got is null || got.Value.bytes is null) return Results.NotFound();
            return Results.File(got.Value.bytes!, H.Str(u["passport_photo_mime"]) ?? got.Value.mime);
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
            var stats = EvidenceStats(db, H.L(u["id"]), visibleOnly: true);
            var show = WorldPassport.Disclosure.From(u);
            var doc = new WorldPassport.PassportDoc
            {
                Name = H.Str(u["display_name"]) ?? "PCI World participant",
                VerifyUrl = WorldUrl.Abs(ctx.Request, "/world/p/" + token),
                // Whole-history counts from SQL (P1-06) — the page of rows is only the window the
                // one-page document can draw, and it says so when it cannot draw them all.
                Completed = (int)stats.Completed,
                Industries = (int)stats.Industries,
                Tracks = (int)stats.Tracks,
                TotalRows = stats.Completed,
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
