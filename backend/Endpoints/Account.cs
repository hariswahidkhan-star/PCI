using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>Open account creation and profile building. Students can now create an account for
/// free — with basic information or via Google sign-in — before paying anything; membership and
/// exam fees remain separate purchases whose effects the Stripe webhook applies to the same
/// account by email. Also hosts the profile wizard's repeaters: multiple work experiences,
/// academic qualifications and externally-held certifications per user.</summary>
public static class Account
{
    static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };

    /// <summary>Weighted profile completeness, stored on student_profiles so every surface
    /// (portal ring, admin view) reads one number: 20 base + up to 30 for profile fields
    /// + 20 first experience + 15 first qualification + 15 first held certification.</summary>
    public static void RecomputeCompletion(Db db, long uid)
    {
        var p = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", uid);
        string[] fields = { "mobile", "country", "city", "current_role", "company", "industry_sector",
                            "years_experience", "highest_qualification", "project_controls_area", "linkedin_url" };
        var filled = p is null ? 0 : fields.Count(f => !string.IsNullOrWhiteSpace(H.Str(p[f])));
        var score = 20 + (int)Math.Round(filled * 3.0);
        if (db.Scalar<long>("SELECT COUNT(*) FROM work_experiences WHERE user_id=?", uid) > 0) score += 20;
        if (db.Scalar<long>("SELECT COUNT(*) FROM qualifications WHERE user_id=?", uid) > 0) score += 15;
        if (db.Scalar<long>("SELECT COUNT(*) FROM held_certifications WHERE user_id=?", uid) > 0) score += 15;
        db.Execute("UPDATE student_profiles SET profile_completion_percentage=? WHERE user_id=?", Math.Min(100, score), uid);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);
        var rx = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        // Mint a 30-day session immediately after signup/Google sign-in — same shape as /api/login,
        // so both front-ends treat all three entry paths identically.
        (string token, object user) StartSession(Dictionary<string, object?> u, HttpRequest req)
        {
            var session = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+30 day'))",
                u["id"], Security.Sha(session));
            try
            {
                var ua = req.Headers.UserAgent.ToString();
                var dev = System.Text.RegularExpressions.Regex.IsMatch(ua, "Mobile|iPhone|Android") ? "Mobile"
                        : System.Text.RegularExpressions.Regex.IsMatch(ua, "iPad|Tablet") ? "Tablet" : "Desktop";
                var ip = H.LastHopIp(req.Headers["x-forwarded-for"].ToString(), req.HttpContext.Connection.RemoteIpAddress?.ToString());
                db.Execute("INSERT INTO login_events(user_id,ip,user_agent,device,outcome) VALUES(?,?,?,?,?)", u["id"], ip, ua.Length > 300 ? ua[..300] : ua, dev, "success");
            }
            catch { }
            return (session, new { id = u["id"], email = u["email"], firstName = u["first_name"], lastName = u["last_name"] });
        }

        Dictionary<string, object?> CreateStudent(string email, string? first, string? last, string? passwordHash, string? country, string? mobile)
        {
            var uid = db.ExecuteReturningId(
                "INSERT INTO users(email,first_name,last_name,role,status,password_hash) VALUES(?,?,?, 'student','active',?)",
                email, first ?? "", last ?? "", passwordHash);
            db.Execute("INSERT INTO student_profiles(user_id,country,mobile) VALUES(?,?,?)", uid, country ?? "", mobile ?? "");
            return db.QueryOne("SELECT * FROM users WHERE id=?", uid)!;
        }

        // ---- which optional sign-in providers are configured (safe to expose: a client id is public) ----
        app.MapGet("/api/auth/config", () =>
            J(new { googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") }));

        // ---- open self-signup: basic information, no payment required ----
        app.MapPost("/api/register", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").ToLowerInvariant().Trim();
            var first = (H.GetS(b, "firstName", "first_name") ?? "").Trim();
            var last = (H.GetS(b, "lastName", "last_name") ?? "").Trim();
            var password = H.GetS(b, "password") ?? "";
            if (!rx.IsMatch(email)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            if (first.Length is 0 or > 80 || last.Length > 80) return Results.Json(new { error = "name_required" }, statusCode: 400);
            if (password.Length < 8) return Results.Json(new { error = "weak_password" }, statusCode: 400);
            // When the client sends a confirmation (the sign-up form does), re-check the match server-side
            // so a mismatched pair can never create an account even if the client validation is bypassed.
            if (H.GetS(b, "confirmPassword", "confirm_password") is { } confirm && confirm != password)
                return Results.Json(new { error = "password_mismatch" }, statusCode: 400);
            if (db.QueryOne("SELECT id FROM users WHERE email=?", email) is not null)
                return Results.Json(new { error = "account_exists" }, statusCode: 409);

            var u = CreateStudent(email, first, last, BCrypt.Net.BCrypt.HashPassword(password),
                H.GetS(b, "country"), H.GetS(b, "mobile"));
            log(H.Ln(u["id"]), "account_created", "self signup");
            var baseUrl = Mailer.BaseUrl(req);
            Mailer.Send(db, H.Ln(u["id"]), email, "account_created", "Welcome to PCI Global — your student account is ready",
                $"<p>Hi {System.Net.WebUtility.HtmlEncode(first)},</p><p>Your PCI Global student account is ready. " +
                $"Sign in any time at <a href=\"{baseUrl}/app/\">{baseUrl}/app/</a> to build your profile, explore certifications " +
                "and activate your membership when you are ready — there is nothing to pay until you choose to enrol.</p>" +
                "<p>— Project Controls Institute Global</p>");
            var (token, user) = StartSession(u, req);
            Analytics.Track(db, req.HttpContext, "registration_completed", H.Ln(u["id"]));
            // ERP / integrations outbox (Phase 9): a canonical new-member event for CRM/ERP sync.
            Integrations.Emit(db, "member.registered", "user", H.Ln(u["id"]), new
            {
                user_id = H.Ln(u["id"]), email, first_name = first, last_name = last,
                country = H.GetS(b, "country"), occurred_at = H.IsoNow,
            });
            return J(new { ok = true, token, user });
        });

        // ---- Google sign-in: verifies the Google-issued ID token server-side, then signs the
        // student in (creating the account on first use). Enabled by setting GOOGLE_CLIENT_ID. ----
        app.MapPost("/api/auth/google", async (HttpRequest req) =>
        {
            var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
            if (string.IsNullOrEmpty(clientId))
                return Results.Json(new { error = "google_not_configured" }, statusCode: 503);
            var b = await H.Body(req);
            var idToken = H.GetS(b, "credential", "id_token") ?? "";
            if (idToken.Length is < 20 or > 4096) return Results.Json(new { error = "invalid_google_token" }, statusCode: 401);

            JsonElement claims;
            try
            {
                var resp = await Http.GetAsync("https://oauth2.googleapis.com/tokeninfo?id_token=" + Uri.EscapeDataString(idToken));
                if (!resp.IsSuccessStatusCode) return Results.Json(new { error = "invalid_google_token" }, statusCode: 401);
                claims = JsonSerializer.Deserialize<JsonElement>(await resp.Content.ReadAsStringAsync());
            }
            catch { return Results.Json(new { error = "google_unavailable" }, statusCode: 502); }

            string? C(string k) => claims.TryGetProperty(k, out var v) ? v.GetString() : null;
            if (C("aud") != clientId) return Results.Json(new { error = "invalid_google_token" }, statusCode: 401);
            if (C("iss") is not ("accounts.google.com" or "https://accounts.google.com"))
                return Results.Json(new { error = "invalid_google_token" }, statusCode: 401);
            if (C("email_verified") != "true") return Results.Json(new { error = "email_unverified" }, statusCode: 403);
            var email = (C("email") ?? "").ToLowerInvariant().Trim();
            if (!rx.IsMatch(email)) return Results.Json(new { error = "invalid_google_token" }, statusCode: 401);

            var u = db.QueryOne("SELECT * FROM users WHERE email=?", email);
            if (u is null)
            {
                u = CreateStudent(email, C("given_name"), C("family_name"), null, null, null);
                log(H.Ln(u["id"]), "account_created", "google sign-in");
            }
            else if (H.Str(u["status"]) == "deactivated")
                return Results.Json(new { error = "inactive" }, statusCode: 403);
            else if (H.Str(u["status"]) == "pending")
            {
                // A pending account (admin-created, never activated) with a Google-verified email is
                // safely activated: the identity holder has proven control of the address.
                db.Execute("UPDATE users SET status='active', updated_at=datetime('now') WHERE id=?", u["id"]);
                u["status"] = "active";
            }
            log(H.Ln(u["id"]), "login_google", email);
            var (token, user) = StartSession(u, req);
            return J(new { ok = true, token, user });
        });

        // ---- profile wizard repeaters: experiences / qualifications / held certifications ----
        MapRepeater(app, db, log, "experiences", "work_experiences",
            fields: new[] { "company", "title", "start_date", "end_date", "country", "industry", "hours_per_week", "summary" },
            required: new[] { "company", "title" }, intFields: new[] { "is_current" });
        MapRepeater(app, db, log, "qualifications", "qualifications",
            fields: new[] { "institution", "degree", "field", "year_completed", "country" },
            required: new[] { "institution", "degree" }, intFields: Array.Empty<string>());
        MapRepeater(app, db, log, "certifications-held", "held_certifications",
            fields: new[] { "name", "issuer", "credential_ref", "issued_year", "expires_year" },
            required: new[] { "name" }, intFields: Array.Empty<string>());
    }

    /// <summary>One CRUD surface per repeater: GET list, POST create, PATCH /{id}, DELETE /{id} —
    /// always scoped to the authenticated user, capped at 50 rows, completion recomputed after
    /// every change so the profile ring is never stale.</summary>
    static void MapRepeater(WebApplication app, Db db, Action<long?, string, string?> log,
        string route, string table, string[] fields, string[] required, string[] intFields)
    {
        IResult J(object o) => Results.Json(o);
        IResult NoToken() => Results.Json(new { error = "no_token" }, statusCode: 401);
        var all = fields.Concat(intFields).ToArray();

        (Dictionary<string, object?> vals, string? err) ReadBody(Dictionary<string, JsonElement> b, bool creating)
        {
            var vals = new Dictionary<string, object?>();
            foreach (var f in fields)
            {
                var v = H.GetS(b, f);
                if (v is null) { if (creating) vals[f] = ""; continue; }
                var cap = f == "summary" ? 2000 : 300;
                if (v.Length > cap) v = v[..cap];
                vals[f] = v.Trim();
            }
            foreach (var f in intFields)
            {
                var v = H.GetS(b, f);
                if (v is null) { if (creating) vals[f] = 0; continue; }
                vals[f] = (v == "1" || v.Equals("true", StringComparison.OrdinalIgnoreCase)) ? 1 : 0;
            }
            foreach (var f in required)
                if (creating && string.IsNullOrWhiteSpace(H.Str(vals.GetValueOrDefault(f))))
                    return (vals, f + "_required");
            return (vals, null);
        }

        app.MapGet($"/api/me/{route}", (HttpContext ctx) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db); if (u is null) return NoToken();
            return J(new { rows = db.Query($"SELECT * FROM {table} WHERE user_id=? ORDER BY id DESC", u.Id) });
        });

        app.MapPost($"/api/me/{route}", async (HttpContext ctx) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db); if (u is null) return NoToken();
            if (db.Scalar<long>($"SELECT COUNT(*) FROM {table} WHERE user_id=?", u.Id) >= 50)
                return Results.Json(new { error = "limit_reached" }, statusCode: 400);
            var (vals, err) = ReadBody(await H.Body(ctx.Request), creating: true);
            if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
            var cols = vals.Keys.ToList();
            var id = db.ExecuteReturningId(
                $"INSERT INTO {table}(user_id,{string.Join(",", cols)}) VALUES(?,{string.Join(",", cols.Select(_ => "?"))})",
                new object?[] { u.Id }.Concat(cols.Select(c => vals[c])).ToArray());
            RecomputeCompletion(db, u.Id);
            log(u.Id, table + "_added", H.Str(vals.GetValueOrDefault(required[0])));
            return J(new { ok = true, id });
        });

        app.MapPatch($"/api/me/{route}/{{id:long}}", async (HttpContext ctx, long id) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db); if (u is null) return NoToken();
            if (db.QueryOne($"SELECT id FROM {table} WHERE id=? AND user_id=?", id, u.Id) is null)
                return Results.Json(new { error = "not_found" }, statusCode: 404);
            var (vals, _) = ReadBody(await H.Body(ctx.Request), creating: false);
            foreach (var f in required)
                if (vals.ContainsKey(f) && string.IsNullOrWhiteSpace(H.Str(vals[f])))
                    return Results.Json(new { error = f + "_required" }, statusCode: 400);
            if (vals.Count > 0)
                db.Execute($"UPDATE {table} SET {string.Join(",", vals.Keys.Select(k => k + "=?"))} WHERE id=? AND user_id=?",
                    vals.Values.Append((object?)id).Append(u.Id).ToArray());
            RecomputeCompletion(db, u.Id);
            return J(new { ok = true });
        });

        app.MapDelete($"/api/me/{route}/{{id:long}}", (HttpContext ctx, long id) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db); if (u is null) return NoToken();
            db.Execute($"DELETE FROM {table} WHERE id=? AND user_id=?", id, u.Id);
            RecomputeCompletion(db, u.Id);
            return J(new { ok = true });
        });
    }
}
