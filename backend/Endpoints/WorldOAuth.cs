using System.Security.Cryptography;
using System.Text;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI World — OAuth 2.1-shaped authorization (the dedicated-domain groundwork).
///
/// The transitional fragment-handoff bridge stays in place for the embedded pages; THIS is the
/// standard-shaped path a separate PCI World application uses: authorization code + PKCE (S256
/// only), a client registry with EXACT redirect-URI matching, single-use codes that die in two
/// minutes, and replay detection that revokes the session a stolen code already minted.
/// No token ever appears in a URL the server logs: the code is bound to the verifier the client
/// never transmitted at issue time.
/// </summary>
public static class WorldOAuth
{
    public const int CodeLifetimeSeconds = 120;

    // ───────────────────────── core (testable without HTTP) ─────────────────────────

    /// <summary>RFC 7636 S256: BASE64URL(SHA256(ASCII(verifier))), no padding.</summary>
    public static string S256(string verifier) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.ASCII.GetBytes(verifier)))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');

    static bool ValidChallenge(string s) =>
        s.Length is >= 43 and <= 128 && s.All(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.' or '~');

    public static bool RedirectAllowed(Db db, string clientId, string redirectUri)
    {
        var c = db.QueryOne("SELECT redirect_uris FROM pciworld_oauth_clients WHERE client_id=?", clientId);
        if (c is null) return false;
        // EXACT string match against the registered list — no prefixes, no wildcards, no
        // "starts with" (the classic open-redirect hole in OAuth deployments).
        return (H.Str(c["redirect_uris"]) ?? "").Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Contains(redirectUri, StringComparer.Ordinal);
    }

    /// <summary>Issue a single-use authorization code bound to client + redirect + S256 challenge.
    /// S256 is the ONLY accepted method (OAuth 2.1 drops 'plain').</summary>
    public static (string? Err, string Code) IssueCode(Db db, long worldUserId, string clientId,
        string redirectUri, string codeChallenge, string method)
    {
        if (method != "S256") return ("unsupported_challenge_method", "");
        if (!ValidChallenge(codeChallenge)) return ("invalid_challenge", "");
        if (!RedirectAllowed(db, clientId, redirectUri)) return ("invalid_client_or_redirect", "");
        var code = "WOA" + Convert.ToHexString(RandomNumberGenerator.GetBytes(24));
        db.Execute($@"INSERT INTO pciworld_oauth_codes
                (code_sha, client_id, world_user_id, redirect_uri, code_challenge, expires_at)
            VALUES(?,?,?,?,?, datetime('now','+{CodeLifetimeSeconds} seconds'))",
            Security.Sha(code), clientId, worldUserId, redirectUri, codeChallenge);
        return (null, code);
    }

    /// <summary>Exchange a code + verifier for a World session token. Every failure answers the
    /// SAME invalid_grant (no oracle). A failed PKCE attempt consumes the code (no second guess);
    /// a replay of an already-consumed code revokes the session that code minted (OAuth 2.1
    /// stolen-code mitigation).</summary>
    public static (string? Err, long WorldId, string Token) ExchangeCode(Db db, string code,
        string verifier, string clientId, string redirectUri)
    {
        var sha = Security.Sha(code ?? "");
        var row = db.QueryOne("SELECT * FROM pciworld_oauth_codes WHERE code_sha=?", sha);
        if (row is null) return ("invalid_grant", 0, "");
        if (row["consumed_at"] is not null)
        {
            // Replay: someone is presenting a code that already succeeded (or already burned a
            // guess). Kill whatever session the first presentation minted — if the second caller
            // is the legitimate client, it can re-authorize; if the first was the thief, its
            // session just died.
            if (row["minted_token_sha"] is not null)
                db.Execute("DELETE FROM pciworld_user_sessions WHERE token=?", H.Str(row["minted_token_sha"]));
            return ("invalid_grant", 0, "");
        }
        // Consume FIRST, race-safely: two concurrent presentations cannot both pass this gate.
        var (_, changed) = db.ExecuteWithChanges(
            "UPDATE pciworld_oauth_codes SET consumed_at=datetime('now') WHERE code_sha=? AND consumed_at IS NULL", sha);
        if (changed == 0) return ("invalid_grant", 0, "");
        if (H.IsPast(H.Str(row["expires_at"]))) return ("invalid_grant", 0, "");
        if (H.Str(row["client_id"]) != clientId || H.Str(row["redirect_uri"]) != redirectUri)
            return ("invalid_grant", 0, "");
        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.ASCII.GetBytes(S256(verifier ?? "")),
                Encoding.ASCII.GetBytes(H.Str(row["code_challenge"]) ?? "")))
            return ("invalid_grant", 0, "");
        var worldId = H.L(row["world_user_id"]);
        if (db.QueryOne("SELECT id FROM pciworld_users WHERE id=? AND status='active'", worldId) is null)
            return ("invalid_grant", 0, "");
        var token = WorldAccount.MintSession(db, worldId);
        db.Execute("UPDATE pciworld_oauth_codes SET minted_token_sha=? WHERE code_sha=?", Security.Sha(token), sha);
        return (null, worldId, token);
    }

    // ───────────────────────── endpoints ─────────────────────────

    public static void Map(WebApplication app, Db db, Action<long?, string, string> log)
    {
        bool Enabled() => Settings.Str(db, "world_enabled", "1") == "1";
        IResult Disabled() => Results.Json(new { error = "world_disabled" }, statusCode: 503);
        IResult Err(string e, int status, string? message = null) =>
            Results.Json(message is null ? new { error = e } : (object)new { error = e, message }, statusCode: status);

        // Client metadata for a consent/confirmation screen: name and whether the redirect is
        // registered — never the full registry.
        app.MapGet("/api/oauth/world/client", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var clientId = ctx.Request.Query["client_id"].ToString();
            var c = db.QueryOne("SELECT client_id, name FROM pciworld_oauth_clients WHERE client_id=?", clientId);
            if (c is null) return Err("unknown_client", 404);
            return Results.Json(new { client_id = H.Str(c["client_id"]), name = H.Str(c["name"]) });
        });

        // Authorization: the PORTAL identity (canonical student session) authorizes the World
        // client. Same identity bridge as the SSO endpoint — LinkStudent quarantines conflicts and
        // never merges; impersonated support sessions can never authorize.
        app.MapPost("/api/oauth/world/authorize", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var s = Auth.UserFromReq(ctx.Request, db);
            if (s is null) return Err("no_token", 401);
            if (s.Impersonated) return Err("impersonation_readonly", 403, "This action is disabled in support view.");
            var b = await H.Body(ctx.Request);
            var (lerr, worldId) = WorldAccount.LinkStudent(db, s.Id, s.Email, ($"{s.FirstName} {s.LastName}").Trim());
            if (lerr is not null) return Err(lerr, lerr == "email_in_use" ? 409 : 403);
            var (err, code) = IssueCode(db, worldId,
                H.GetS(b, "client_id") ?? "", H.GetS(b, "redirect_uri") ?? "",
                H.GetS(b, "code_challenge") ?? "", H.GetS(b, "code_challenge_method") ?? "");
            if (err is not null) return Err(err, 400);
            log(s.Id, "world_oauth_authorize", $"student #{s.Id} → world #{worldId}");
            return Results.Json(new { ok = true, code, expires_in = CodeLifetimeSeconds });
        });

        // Token exchange: public client + PKCE, uniform invalid_grant on every failure path.
        app.MapPost("/api/oauth/world/token", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var b = await H.Body(ctx.Request);
            if ((H.GetS(b, "grant_type") ?? "") != "authorization_code")
                return Err("unsupported_grant_type", 400);
            var (err, worldId, token) = ExchangeCode(db,
                H.GetS(b, "code") ?? "", H.GetS(b, "code_verifier") ?? "",
                H.GetS(b, "client_id") ?? "", H.GetS(b, "redirect_uri") ?? "");
            if (err is not null) return Err(err, 400);
            log(null, "world_oauth_token", $"world #{worldId}");
            return Results.Json(new { access_token = token, token_type = "Bearer",
                expires_in = 30 * 24 * 3600 });
        });
    }
}
