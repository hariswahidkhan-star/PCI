using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI World — SEPARATE administration (docs/pciworld/ARCHITECTURE.md §2, decision 2).
///
/// A wholly separate realm from the PCI admin: its own users (pciworld_admin_users), its own
/// sessions, its own login at /world-admin, its own audit log — deployable behind
/// admin.pciworld.org. A PCI admin_sessions token is meaningless here and vice versa. The PCI
/// admin SPA contains no link to any of this, and nothing here links back.
///
/// Lifecycle (server-enforced, WorldLifecycle): draft → in_review → approved → published, with
/// independent maker-checker at approve, re-validation at publish, immutable version snapshots,
/// revise-as-new-version and retire/restore. RBAC (WorldRbac) is checked per endpoint — hiding a
/// button is never the authorization.
/// </summary>
public static class WorldAdmin
{
    public sealed record Ctx(long Id, string Email, string? Name, string Role);

    /// <summary>Resolve a world-admin bearer token — pciworld_admin_sessions ONLY.</summary>
    public static Ctx? FromReq(HttpRequest req, Db db)
    {
        var h = req.Headers.Authorization.ToString();
        var bearer = h.StartsWith("Bearer ") ? h[7..] : null;
        if (bearer is null) return null;
        var sess = db.QueryOne("SELECT * FROM pciworld_admin_sessions WHERE token=? AND expires_at>datetime('now')", Security.Sha(bearer));
        if (sess is null) return null;
        var a = db.QueryOne("SELECT * FROM pciworld_admin_users WHERE id=?", sess["admin_id"]);
        if (a is null || H.Str(a["status"]) != "active") return null;
        return new Ctx(H.L(a["id"]), H.Str(a["email"])!, H.Str(a["name"]), H.Str(a["role"]) ?? "viewer");
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);
        void Audit(long? adminId, string action, string? detail)
        {
            try { db.Execute("INSERT INTO pciworld_audit(admin_id,action,detail) VALUES(?,?,?)", adminId, action, detail); } catch { }
        }

        /// Gate: authenticated world-admin with the required action group.
        IResult? Gate(HttpContext ctx, string action, out Ctx? adm)
        {
            adm = FromReq(ctx.Request, db);
            if (adm is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!WorldRbac.Allowed(adm.Role, action)) return Results.Json(new { error = "forbidden" }, statusCode: 403);
            return null;
        }

        // ───────────────────────────── auth ─────────────────────────────

        app.MapPost("/api/world-admin/auth/login", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant();
            var password = H.GetS(b, "password") ?? "";
            var a = db.QueryOne("SELECT * FROM pciworld_admin_users WHERE email=?", email);
            if (a is null) { LoginGuard.BurnTime(password); return Results.Json(new { error = "invalid_credentials" }, statusCode: 401); }
            if (LoginGuard.IsLocked(db, "pciworld_admin_users", a["id"]))
                return Results.Json(new { error = "account_locked", message = "Too many failed attempts. Try again in a few minutes." }, statusCode: 429);
            if (!Security.VerifyPassword(password, a["password_hash"] as string))
            {
                LoginGuard.OnFail(db, "pciworld_admin_users", a["id"]);
                return Results.Json(new { error = "invalid_credentials" }, statusCode: 401);
            }
            LoginGuard.OnSuccess(db, "pciworld_admin_users", a["id"]);
            if (H.Str(a["status"]) != "active") return Results.Json(new { error = "account_suspended" }, statusCode: 403);
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO pciworld_admin_sessions(admin_id,token,expires_at) VALUES(?,?, datetime('now','+8 hours'))",
                a["id"], Security.Sha(token));
            db.Execute("UPDATE pciworld_admin_users SET last_login_at=datetime('now') WHERE id=?", a["id"]);
            Audit(H.L(a["id"]), "login", email);
            return J(new { token, name = H.Str(a["name"]), role = H.Str(a["role"]) });
        });

        app.MapPost("/api/world-admin/auth/logout", (HttpContext ctx) =>
        {
            var h = ctx.Request.Headers.Authorization.ToString();
            if (h.StartsWith("Bearer "))
                db.Execute("DELETE FROM pciworld_admin_sessions WHERE token=?", Security.Sha(h[7..]));
            return J(new { ok = true });
        });

        app.MapPost("/api/world-admin/auth/password", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var current = H.GetS(b, "current") ?? "";
            var next = H.GetS(b, "next") ?? "";
            if (next.Length < 12) return Results.Json(new { error = "weak_password", message = "Use at least 12 characters." }, statusCode: 400);
            var a = db.QueryOne("SELECT password_hash FROM pciworld_admin_users WHERE id=?", adm!.Id);
            if (!Security.VerifyPassword(current, a?["password_hash"] as string))
                return Results.Json(new { error = "invalid_credentials" }, statusCode: 401);
            db.Execute("UPDATE pciworld_admin_users SET password_hash=? WHERE id=?", BCrypt.Net.BCrypt.HashPassword(next), adm.Id);
            Audit(adm.Id, "password_change", null);
            return J(new { ok = true });
        });

        // ───────────────────────────── challenges ─────────────────────────────

        app.MapGet("/api/world-admin/challenges", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var rows = db.Query(@"SELECT id,code,title,industry,track,difficulty,status,retired,current_version,
                    author_id,approved_by,updated_at FROM pciworld_challenges ORDER BY id ASC")
                .Select(r => new
                {
                    id = H.L(r["id"]), code = H.Str(r["code"]), title = H.Str(r["title"]),
                    industry = H.Str(r["industry"]), track = H.Str(r["track"]), difficulty = H.Str(r["difficulty"]),
                    status = H.Str(r["status"]), retired = H.L(r["retired"]) == 1,
                    current_version = H.L(r["current_version"]), updated_at = H.Str(r["updated_at"]),
                }).ToList();
            return J(new { rows, total = rows.Count });
        });

        app.MapPost("/api/world-admin/challenges", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "author", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var code = (H.GetS(b, "code") ?? "").Trim().ToUpperInvariant();
            if (code.Length < 4) return Results.Json(new { error = "bad_code" }, statusCode: 400);
            if (db.QueryOne("SELECT id FROM pciworld_challenges WHERE code=?", code) is not null)
                return Results.Json(new { error = "duplicate_code" }, statusCode: 409);
            var id = db.ExecuteReturningId(@"INSERT INTO pciworld_challenges
                    (code,title,hook,industry,role,track,difficulty,est_minutes,competencies_json,synthetic_declared,config_json,author_id)
                VALUES(?,?,?,?,?,?,?,?,?,?,?,?)",
                code, H.GetS(b, "title") ?? code, H.GetS(b, "hook"), H.GetS(b, "industry"), H.GetS(b, "role"),
                H.GetS(b, "track") ?? "project_controls", H.GetS(b, "difficulty") ?? "foundation",
                (long)(H.GetNum(b, "est_minutes") ?? 8), RawOrNull(b, "competencies"),
                H.GetEl(b, "synthetic_declared") is { ValueKind: JsonValueKind.True } ? 1 : 0,
                RawOrNull(b, "config"), adm!.Id);
            Audit(adm.Id, "challenge_create", code);
            return J(new { id, code, status = "draft" });
        });

        app.MapGet("/api/world-admin/challenges/{id:long}", (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var r = db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var versions = db.Query("SELECT version,created_at FROM pciworld_challenge_versions WHERE challenge_id=? ORDER BY version", id)
                .Select(v => new { version = H.L(v["version"]), created_at = H.Str(v["created_at"]) });
            return J(new
            {
                id = H.L(r["id"]), code = H.Str(r["code"]), title = H.Str(r["title"]), hook = H.Str(r["hook"]),
                industry = H.Str(r["industry"]), role = H.Str(r["role"]), track = H.Str(r["track"]),
                difficulty = H.Str(r["difficulty"]), est_minutes = H.L(r["est_minutes"]),
                competencies_json = H.Str(r["competencies_json"]), synthetic_declared = H.L(r["synthetic_declared"]) == 1,
                config_json = H.Str(r["config_json"]), status = H.Str(r["status"]), retired = H.L(r["retired"]) == 1,
                current_version = H.L(r["current_version"]), review_note = H.Str(r["review_note"]), versions,
            });
        });

        app.MapPut("/api/world-admin/challenges/{id:long}", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "author", out var adm) is { } blocked) return blocked;
            var r = db.QueryOne("SELECT status,code FROM pciworld_challenges WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (!WorldLifecycle.CanEdit(H.Str(r["status"])))
                return Results.Json(new { error = "immutable", message = "Only drafts can be edited — revise a published challenge into a new draft first." }, statusCode: 409);
            var b = await H.Body(ctx.Request);
            // Explicit field mapping only — lifecycle columns are not writable here (no mass assignment).
            db.Execute(@"UPDATE pciworld_challenges SET title=?, hook=?, industry=?, role=?, track=?, difficulty=?,
                    est_minutes=?, competencies_json=?, synthetic_declared=?, config_json=?, updated_at=datetime('now')
                WHERE id=? AND status='draft'",
                H.GetS(b, "title") ?? H.Str(r["code"]), H.GetS(b, "hook"), H.GetS(b, "industry"), H.GetS(b, "role"),
                H.GetS(b, "track") ?? "project_controls", H.GetS(b, "difficulty") ?? "foundation",
                (long)(H.GetNum(b, "est_minutes") ?? 8), RawOrNull(b, "competencies"),
                H.GetEl(b, "synthetic_declared") is { ValueKind: JsonValueKind.True } ? 1 : 0,
                RawOrNull(b, "config"), id);
            Audit(adm!.Id, "challenge_edit", H.Str(r["code"]));
            return J(new { ok = true });
        });

        app.MapGet("/api/world-admin/challenges/{id:long}/validate", (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var r = db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var issues = WorldContent.Validate(WorldLifecycle.InputFor(r));
            return J(new
            {
                publishable = WorldContent.Publishable(issues),
                errors = issues.Count(i => i.Sev == WorldContent.Severity.Error),
                issues = issues.Select(i => new { severity = i.Sev.ToString().ToLowerInvariant(), code = i.Code, message = i.Message }),
            });
        });

        IResult Transition(HttpContext ctx, long id, string action, Func<long, string?> run, string gate)
        {
            if (Gate(ctx, gate, out var adm) is { } blocked) return blocked;
            var err = run(adm!.Id);
            if (err is not null) return Results.Json(new { error = err }, statusCode: err == "not_found" ? 404 : 409);
            var row = db.QueryOne("SELECT code,status,current_version FROM pciworld_challenges WHERE id=?", id);
            Audit(adm.Id, "challenge_" + action, H.Str(row?["code"]));
            log(null, "world_admin_" + action, H.Str(row?["code"]));
            return J(new { ok = true, status = H.Str(row?["status"]), current_version = H.L(row?["current_version"]) });
        }

        app.MapPost("/api/world-admin/challenges/{id:long}/submit-review", (HttpContext ctx, long id) =>
            Transition(ctx, id, "submit_review", _ => WorldLifecycle.SubmitReview(db, id), "author"));

        app.MapPost("/api/world-admin/challenges/{id:long}/approve", (HttpContext ctx, long id) =>
            Transition(ctx, id, "approve", adminId => WorldLifecycle.Approve(db, id, adminId), "review"));

        app.MapPost("/api/world-admin/challenges/{id:long}/reject", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            var note = H.GetS(b, "note");
            return Transition(ctx, id, "reject", adminId => WorldLifecycle.Reject(db, id, adminId, note), "review");
        });

        app.MapPost("/api/world-admin/challenges/{id:long}/publish", (HttpContext ctx, long id) =>
            Transition(ctx, id, "publish", adminId => WorldLifecycle.Publish(db, id, adminId), "publish"));

        app.MapPost("/api/world-admin/challenges/{id:long}/revise", (HttpContext ctx, long id) =>
            Transition(ctx, id, "revise", _ => WorldLifecycle.Revise(db, id), "author"));

        app.MapPost("/api/world-admin/challenges/{id:long}/retire", (HttpContext ctx, long id) =>
            Transition(ctx, id, "retire", _ => { WorldLifecycle.Retire(db, id); return null; }, "publish"));

        app.MapPost("/api/world-admin/challenges/{id:long}/restore", (HttpContext ctx, long id) =>
            Transition(ctx, id, "restore", _ => { WorldLifecycle.Restore(db, id); return null; }, "publish"));

        // ───────────────────────────── calendar / audit / overview / users ─────────────────────────────

        app.MapGet("/api/world-admin/calendar", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var rows = db.Query(@"SELECT c.day_utc, c.challenge_id, c.note, ch.code, ch.title
                FROM pciworld_calendar c LEFT JOIN pciworld_challenges ch ON ch.id=c.challenge_id
                WHERE c.day_utc >= date('now','-1 day') ORDER BY c.day_utc LIMIT 60")
                .Select(r => new { day_utc = H.Str(r["day_utc"]), challenge_id = H.L(r["challenge_id"]),
                                   code = H.Str(r["code"]), title = H.Str(r["title"]), note = H.Str(r["note"]) });
            return J(new { rows });
        });

        app.MapPost("/api/world-admin/calendar", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "publish", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var day = (H.GetS(b, "day_utc") ?? "").Trim();
            var challengeId = (long)(H.GetNum(b, "challenge_id") ?? 0);
            if (!DateTime.TryParseExact(day, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                return Results.Json(new { error = "bad_day", message = "day_utc must be YYYY-MM-DD (UTC)." }, statusCode: 400);
            var ch = db.QueryOne("SELECT id FROM pciworld_challenges WHERE id=? AND current_version>=1 AND retired=0", challengeId);
            if (ch is null) return Results.Json(new { error = "not_servable", message = "Only a published, non-retired challenge can be scheduled." }, statusCode: 409);
            db.Execute("DELETE FROM pciworld_calendar WHERE day_utc=?", day);
            db.Execute("INSERT INTO pciworld_calendar(day_utc,challenge_id,note) VALUES(?,?,?)", day, challengeId, H.GetS(b, "note"));
            Audit(adm!.Id, "calendar_set", $"{day} → #{challengeId}");
            return J(new { ok = true });
        });

        // ───────────── content reports ─────────────

        app.MapGet("/api/world-admin/reports", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var status = ctx.Request.Query["status"].ToString();
            if (status != "resolved") status = "open";
            var rows = db.Query(@"SELECT r.id, r.category, r.message, r.status, r.resolution, r.created_at, r.resolved_at,
                    c.code, c.title
                FROM pciworld_reports r LEFT JOIN pciworld_challenges c ON c.id=r.challenge_id
                WHERE r.status=? ORDER BY r.id DESC LIMIT 200", status)
                .Select(r => new
                {
                    id = H.L(r["id"]), category = H.Str(r["category"]), message = H.Str(r["message"]),
                    status = H.Str(r["status"]), resolution = H.Str(r["resolution"]),
                    code = H.Str(r["code"]), title = H.Str(r["title"]),
                    created_at = H.Str(r["created_at"]), resolved_at = H.Str(r["resolved_at"]),
                });
            return J(new { rows });
        });

        app.MapPost("/api/world-admin/reports/{id:long}/resolve", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "review", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var note = (H.GetS(b, "note") ?? "").Trim();
            if (note.Length < 3) return Results.Json(new { error = "note_required",
                message = "Record what was checked or changed — the resolution is the audit trail." }, statusCode: 400);
            var n = db.Execute(@"UPDATE pciworld_reports SET status='resolved', resolution=?, resolved_by=?,
                resolved_at=datetime('now') WHERE id=? AND status='open'", note, adm!.Id, id);
            if (n == 0) return Results.Json(new { error = "not_found" }, statusCode: 404);
            Audit(adm.Id, "report_resolve", $"#{id}");
            return J(new { ok = true });
        });

        app.MapGet("/api/world-admin/audit", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var rows = db.Query(@"SELECT a.id, a.admin_id, u.email, a.action, a.detail, a.created_at
                FROM pciworld_audit a LEFT JOIN pciworld_admin_users u ON u.id=a.admin_id
                ORDER BY a.id DESC LIMIT 200")
                .Select(r => new { id = H.L(r["id"]), email = H.Str(r["email"]), action = H.Str(r["action"]),
                                   detail = H.Str(r["detail"]), created_at = H.Str(r["created_at"]) });
            return J(new { rows });
        });

        app.MapGet("/api/world-admin/overview", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            long Count(string sql) => db.Scalar<long>(sql);
            return J(new
            {
                challenges = new
                {
                    total = Count("SELECT COUNT(*) FROM pciworld_challenges"),
                    draft = Count("SELECT COUNT(*) FROM pciworld_challenges WHERE status='draft'"),
                    in_review = Count("SELECT COUNT(*) FROM pciworld_challenges WHERE status='in_review'"),
                    approved = Count("SELECT COUNT(*) FROM pciworld_challenges WHERE status='approved'"),
                    published = Count("SELECT COUNT(*) FROM pciworld_challenges WHERE status='published'"),
                    servable = Count("SELECT COUNT(*) FROM pciworld_challenges WHERE current_version>=1 AND retired=0"),
                    retired = Count("SELECT COUNT(*) FROM pciworld_challenges WHERE retired=1"),
                },
                attempts = new
                {
                    total = Count("SELECT COUNT(*) FROM pciworld_attempts"),
                    completed = Count("SELECT COUNT(*) FROM pciworld_attempts WHERE status='completed'"),
                    shared = Count("SELECT COUNT(*) FROM pciworld_attempts WHERE result_token_sha IS NOT NULL AND result_revoked=0"),
                    invites = Count("SELECT COUNT(*) FROM pciworld_invites WHERE revoked=0"),
                },
            });
        });

        app.MapGet("/api/world-admin/users", (HttpContext ctx) =>
        {
            if (Gate(ctx, "admin", out _) is { } blocked) return blocked;
            var rows = db.Query("SELECT id,email,name,role,status,last_login_at FROM pciworld_admin_users ORDER BY id")
                .Select(r => new { id = H.L(r["id"]), email = H.Str(r["email"]), name = H.Str(r["name"]),
                                   role = H.Str(r["role"]), status = H.Str(r["status"]), last_login_at = H.Str(r["last_login_at"]) });
            return J(new { rows });
        });

        app.MapPost("/api/world-admin/users", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "admin", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var email = (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant();
            var role = H.GetS(b, "role") ?? "viewer";
            var password = H.GetS(b, "password") ?? "";
            if (!email.Contains('@')) return Results.Json(new { error = "bad_email" }, statusCode: 400);
            if (!WorldRbac.Roles.Contains(role)) return Results.Json(new { error = "bad_role" }, statusCode: 400);
            if (password.Length < 12) return Results.Json(new { error = "weak_password", message = "Use at least 12 characters." }, statusCode: 400);
            if (db.QueryOne("SELECT id FROM pciworld_admin_users WHERE email=?", email) is not null)
                return Results.Json(new { error = "duplicate_email" }, statusCode: 409);
            var id = db.ExecuteReturningId("INSERT INTO pciworld_admin_users(email,name,role,password_hash) VALUES(?,?,?,?)",
                email, H.GetS(b, "name"), role, BCrypt.Net.BCrypt.HashPassword(password));
            Audit(adm!.Id, "admin_user_create", $"{email} ({role})");
            return J(new { id, email, role });
        });

        app.MapPost("/api/world-admin/users/{id:long}/status", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "admin", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var status = H.GetS(b, "status") == "suspended" ? "suspended" : "active";
            if (id == adm!.Id) return Results.Json(new { error = "self", message = "You cannot change your own status." }, statusCode: 409);
            db.Execute("UPDATE pciworld_admin_users SET status=? WHERE id=?", status, id);
            db.Execute("DELETE FROM pciworld_admin_sessions WHERE admin_id=?", id);
            Audit(adm.Id, "admin_user_status", $"#{id} → {status}");
            return J(new { ok = true });
        });

        // ───────────────────────────── admin application shell ─────────────────────────────

        app.MapGet("/world-admin", () => Results.Content(AdminShell, "text/html; charset=utf-8"));
    }

    static string? RawOrNull(Dictionary<string, JsonElement> b, string key) =>
        H.GetEl(b, key) is { ValueKind: JsonValueKind.Object or JsonValueKind.Array } el ? el.GetRawText()
        : H.GetS(b, key);

    /// <summary>The PCI World admin application: separate login, separate session storage key,
    /// no shared chrome with the PCI admin. Server-rendered shell + fetch — deployable as-is
    /// behind admin.pciworld.org.</summary>
    const string AdminShell = """
        <!doctype html>
        <html lang="en">
        <head>
        <meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1">
        <meta name="robots" content="noindex">
        <title>PCI World Administration</title>
        <style>
        :root{--bg:#f6f5f2;--ink:#191c1f;--muted:#5b6167;--line:#e3e1da;--accent:#0d5c8d;--bad:#9f2d24;--ok:#186f47}
        *{box-sizing:border-box;margin:0}
        body{background:var(--bg);color:var(--ink);font:15px/1.5 system-ui,-apple-system,"Segoe UI",Roboto,Arial,sans-serif}
        header{background:#12212e;color:#e9edf1;padding:12px 20px;display:flex;gap:16px;align-items:center}
        header b{font-size:16px} header small{color:#9fb1c1}
        header button{margin-left:auto}
        main{max-width:1080px;margin:0 auto;padding:22px 20px}
        .card{background:#fff;border:1px solid var(--line);border-radius:10px;padding:18px;margin:14px 0}
        h2{font-size:17px;margin:0 0 10px}
        table{border-collapse:collapse;width:100%;font-size:14px}
        th,td{padding:7px 8px;border-bottom:1px solid var(--line);text-align:left;vertical-align:top}
        th{font-size:11px;text-transform:uppercase;letter-spacing:.5px;color:var(--muted)}
        button{background:var(--accent);border:0;color:#fff;border-radius:6px;padding:7px 12px;font-size:13px;cursor:pointer}
        button.ghost{background:transparent;color:var(--accent);border:1px solid var(--accent)}
        button:focus-visible,input:focus-visible,textarea:focus-visible,select:focus-visible{outline:3px solid var(--accent);outline-offset:2px}
        input,select,textarea{padding:8px 10px;border:1px solid var(--line);border-radius:6px;font-size:14px;width:100%}
        textarea{font-family:ui-monospace,Menlo,Consolas,monospace;min-height:220px}
        label{display:block;font-weight:600;margin:10px 0 4px;font-size:13px}
        .row{display:grid;grid-template-columns:repeat(auto-fit,minmax(180px,1fr));gap:10px}
        .bad{color:var(--bad)} .ok{color:var(--ok)}
        .pill{display:inline-block;border:1px solid var(--line);border-radius:999px;padding:1px 9px;font-size:12px}
        #login{max-width:380px;margin:60px auto}
        nav.tabs{display:flex;gap:8px;flex-wrap:wrap;margin:16px 0 4px}
        nav.tabs button{background:transparent;color:var(--ink);border:1px solid var(--line)}
        nav.tabs button[aria-selected=true]{background:var(--accent);color:#fff;border-color:var(--accent)}
        </style>
        </head>
        <body>
        <header><b>PCI World Administration</b><small>separate from the PCI Institute admin</small>
          <button id="logout" hidden>Sign out</button></header>
        <main>
        <div id="login" class="card">
          <h2>Sign in</h2>
          <label for="em">Email</label><input id="em" type="email" autocomplete="username">
          <label for="pw">Password</label><input id="pw" type="password" autocomplete="current-password">
          <p style="margin-top:12px"><button id="doLogin">Sign in</button> <span id="loginerr" class="bad" role="alert"></span></p>
        </div>
        <div id="appmain" hidden>
          <nav class="tabs" role="tablist">
            <button role="tab" data-tab="overview" aria-selected="true">Overview</button>
            <button role="tab" data-tab="challenges" aria-selected="false">Challenges</button>
            <button role="tab" data-tab="editor" aria-selected="false">Editor</button>
            <button role="tab" data-tab="calendar" aria-selected="false">Calendar</button>
            <button role="tab" data-tab="reports" aria-selected="false">Reports</button>
            <button role="tab" data-tab="audit" aria-selected="false">Audit</button>
          </nav>
          <div id="tab-overview" class="card"></div>
          <div id="tab-challenges" class="card" hidden></div>
          <div id="tab-editor" class="card" hidden>
            <h2>Challenge editor</h2>
            <div class="row">
              <div><label for="f_code">Code</label><input id="f_code" placeholder="WC-XXX-000"></div>
              <div><label for="f_title">Title</label><input id="f_title"></div>
              <div><label for="f_industry">Industry</label><input id="f_industry"></div>
              <div><label for="f_role">Role</label><input id="f_role"></div>
              <div><label for="f_track">Track</label>
                <select id="f_track"><option>project_controls</option><option>project_management</option>
                <option>project_finance</option><option>governed_ai</option><option>cross_functional</option></select></div>
              <div><label for="f_diff">Difficulty</label>
                <select id="f_diff"><option>foundation</option><option>developing</option><option>professional</option>
                <option>advanced</option><option>expert</option></select></div>
              <div><label for="f_min">Minutes</label><input id="f_min" type="number" value="8"></div>
              <div><label for="f_syn">Synthetic data declared</label><select id="f_syn"><option value="1">yes</option><option value="0">no</option></select></div>
            </div>
            <label for="f_hook">Hook</label><input id="f_hook">
            <label for="f_comp">Competencies (JSON array)</label><input id="f_comp" placeholder='["earned_value"]'>
            <label for="f_config">config_json</label><textarea id="f_config" spellcheck="false"></textarea>
            <p style="margin-top:12px">
              <button id="save">Save draft</button>
              <button class="ghost" id="validate">Validate</button>
              <span id="edmsg" role="status"></span></p>
            <div id="valout"></div>
          </div>
          <div id="tab-calendar" class="card" hidden></div>
          <div id="tab-reports" class="card" hidden></div>
          <div id="tab-audit" class="card" hidden></div>
        </div>
        </main>
        <script>
        (function(){
        'use strict';
        var KEY='world_admin_token', editingId=null;
        function $(id){return document.getElementById(id);}
        function esc(s){var d=document.createElement('span');d.textContent=s==null?'':String(s);return d.innerHTML;}
        function api(path,method,body){
          return fetch(path,{method:method||'GET',headers:{'Content-Type':'application/json',
            'Authorization':'Bearer '+(localStorage.getItem(KEY)||'')},
            body:body?JSON.stringify(body):undefined})
          .then(function(r){return r.json().then(function(j){if(!r.ok)throw j;return j;});});
        }
        function show(logged){$('login').hidden=logged;$('appmain').hidden=!logged;$('logout').hidden=!logged;}
        function tab(name){
          ['overview','challenges','editor','calendar','reports','audit'].forEach(function(t){
            $('tab-'+t).hidden=t!==name;
            document.querySelector('[data-tab='+t+']').setAttribute('aria-selected',t===name?'true':'false');
          });
          if(name==='overview')loadOverview(); if(name==='challenges')loadChallenges();
          if(name==='calendar')loadCalendar(); if(name==='reports')loadReports(); if(name==='audit')loadAudit();
        }
        document.querySelectorAll('[data-tab]').forEach(function(b){b.addEventListener('click',function(){tab(b.dataset.tab);});});
        $('doLogin').addEventListener('click',function(){
          $('loginerr').textContent='';
          api('/api/world-admin/auth/login','POST',{email:$('em').value,password:$('pw').value})
            .then(function(r){localStorage.setItem(KEY,r.token);show(true);tab('overview');})
            .catch(function(e){$('loginerr').textContent=(e&&e.message)||'Sign-in failed.';});
        });
        $('logout').addEventListener('click',function(){
          api('/api/world-admin/auth/logout','POST',{}).catch(function(){});
          localStorage.removeItem(KEY);show(false);
        });
        function loadOverview(){
          api('/api/world-admin/overview').then(function(o){
            $('tab-overview').innerHTML='<h2>Overview</h2>'+
            '<p>Challenges: total '+o.challenges.total+' · servable '+o.challenges.servable+
            ' · draft '+o.challenges.draft+' · in review '+o.challenges.in_review+
            ' · approved '+o.challenges.approved+' · retired '+o.challenges.retired+'</p>'+
            '<p>Attempts: '+o.attempts.total+' · completed '+o.attempts.completed+
            ' · shared results '+o.attempts.shared+' · live invitations '+o.attempts.invites+'</p>';
          }).catch(function(){show(false);});
        }
        function lifecycleButtons(r){
          var b='';
          function act(a,label,ghost){b+='<button '+(ghost?'class="ghost" ':'')+'data-act="'+a+'" data-id="'+r.id+'">'+label+'</button> ';}
          act('open','Open',true);
          if(r.status==='draft')act('submit-review','Submit for review');
          if(r.status==='in_review'){act('approve','Approve');act('reject','Reject',true);}
          if(r.status==='approved')act('publish','Publish');
          if(r.status==='published')act('revise','Revise',true);
          if(!r.retired)act('retire','Retire',true); else act('restore','Restore',true);
          return b;
        }
        function loadChallenges(){
          api('/api/world-admin/challenges').then(function(o){
            var h='<h2>Challenges ('+o.total+') <button id="newch" class="ghost">New draft</button></h2>'+
              '<table><thead><tr><th>Code</th><th>Title</th><th>Difficulty</th><th>Status</th><th>v</th><th>Actions</th></tr></thead><tbody>';
            o.rows.forEach(function(r){
              h+='<tr><td>'+esc(r.code)+'</td><td>'+esc(r.title)+'</td><td>'+esc(r.difficulty)+'</td>'+
                 '<td><span class="pill">'+esc(r.status)+(r.retired?' · retired':'')+'</span></td>'+
                 '<td>'+r.current_version+'</td><td>'+lifecycleButtons(r)+'</td></tr>';
            });
            h+='</tbody></table>';
            $('tab-challenges').innerHTML=h;
            $('newch').addEventListener('click',function(){editingId=null;clearEditor();tab('editor');});
            $('tab-challenges').querySelectorAll('[data-act]').forEach(function(btn){
              btn.addEventListener('click',function(){doAction(btn.dataset.act,btn.dataset.id);});
            });
          });
        }
        function doAction(act,id){
          if(act==='open'){openChallenge(id);return;}
          var body={};
          if(act==='reject'){var note=prompt('Rejection note for the author:')||'';body={note:note};}
          api('/api/world-admin/challenges/'+id+'/'+act,'POST',body)
            .then(loadChallenges)
            .catch(function(e){alert((e&&(e.message||e.error))||'Action failed');});
        }
        function clearEditor(){
          ['f_code','f_title','f_industry','f_role','f_hook','f_comp','f_config'].forEach(function(f){$(f).value='';});
          $('f_min').value=8;$('f_syn').value='1';$('valout').innerHTML='';$('edmsg').textContent='';
        }
        function openChallenge(id){
          api('/api/world-admin/challenges/'+id).then(function(c){
            editingId=c.id;
            $('f_code').value=c.code;$('f_title').value=c.title||'';$('f_industry').value=c.industry||'';
            $('f_role').value=c.role||'';$('f_track').value=c.track||'project_controls';
            $('f_diff').value=c.difficulty||'foundation';$('f_min').value=c.est_minutes||8;
            $('f_syn').value=c.synthetic_declared?'1':'0';$('f_hook').value=c.hook||'';
            $('f_comp').value=c.competencies_json||'';$('f_config').value=c.config_json||'';
            $('valout').innerHTML='';$('edmsg').textContent='Status: '+c.status+' · v'+c.current_version+
              (c.review_note?' · note: '+c.review_note:'');
            tab('editor');
          });
        }
        function editorBody(){
          var body={code:$('f_code').value,title:$('f_title').value,industry:$('f_industry').value,
            role:$('f_role').value,track:$('f_track').value,difficulty:$('f_diff').value,
            est_minutes:parseInt($('f_min').value,10)||8,hook:$('f_hook').value,
            synthetic_declared:$('f_syn').value==='1'};
          try{body.competencies=JSON.parse($('f_comp').value);}catch(e){body.competencies=$('f_comp').value;}
          try{body.config=JSON.parse($('f_config').value);}catch(e){body.config=$('f_config').value;}
          return body;
        }
        $('save').addEventListener('click',function(){
          var body=editorBody();
          var p=editingId?api('/api/world-admin/challenges/'+editingId,'PUT',body)
                         :api('/api/world-admin/challenges','POST',body);
          p.then(function(r){if(r.id)editingId=r.id;$('edmsg').textContent='Saved.';})
           .catch(function(e){$('edmsg').textContent=(e&&(e.message||e.error))||'Save failed.';});
        });
        $('validate').addEventListener('click',function(){
          if(!editingId){$('edmsg').textContent='Save the draft first.';return;}
          api('/api/world-admin/challenges/'+editingId+'/validate').then(function(v){
            var h='<p class="'+(v.publishable?'ok':'bad')+'">'+(v.publishable?'Publishable.':'Not publishable — '+v.errors+' error(s).')+'</p><ul>';
            v.issues.forEach(function(i){h+='<li class="'+(i.severity==='error'?'bad':'')+'">['+i.severity+'] '+esc(i.code)+': '+esc(i.message)+'</li>';});
            $('valout').innerHTML=h+'</ul>';
          });
        });
        function loadCalendar(){
          api('/api/world-admin/calendar').then(function(o){
            var h='<h2>Daily calendar (UTC)</h2><table><thead><tr><th>Day</th><th>Challenge</th><th>Note</th></tr></thead><tbody>';
            o.rows.forEach(function(r){h+='<tr><td>'+esc(r.day_utc)+'</td><td>'+esc(r.code)+' — '+esc(r.title)+'</td><td>'+esc(r.note)+'</td></tr>';});
            h+='</tbody></table><div class="row" style="margin-top:12px">'+
              '<div><label for="c_day">Day (YYYY-MM-DD)</label><input id="c_day"></div>'+
              '<div><label for="c_id">Challenge id</label><input id="c_id" type="number"></div>'+
              '<div><label for="c_note">Note</label><input id="c_note"></div></div>'+
              '<p style="margin-top:10px"><button id="c_set">Schedule</button> <span id="c_msg" role="status"></span></p>'+
              '<p>Days without an entry rotate automatically over the published set.</p>';
            $('tab-calendar').innerHTML=h;
            $('c_set').addEventListener('click',function(){
              api('/api/world-admin/calendar','POST',{day_utc:$('c_day').value,
                challenge_id:parseInt($('c_id').value,10),note:$('c_note').value})
                .then(function(){$('c_msg').textContent='Scheduled.';loadCalendar();})
                .catch(function(e){$('c_msg').textContent=(e&&(e.message||e.error))||'Failed.';});
            });
          });
        }
        function loadReports(){
          api('/api/world-admin/reports?status=open').then(function(o){
            var h='<h2>Open content reports ('+o.rows.length+')</h2>';
            if(!o.rows.length)h+='<p>No open reports — the queue is clear.</p>';
            else{
              h+='<table><thead><tr><th>Ref</th><th>When (UTC)</th><th>Challenge</th><th>Category</th><th>Report</th><th></th></tr></thead><tbody>';
              o.rows.forEach(function(r){
                h+='<tr><td>WR-'+r.id+'</td><td>'+esc(r.created_at)+'</td><td>'+esc(r.code||'—')+'</td>'+
                   '<td>'+esc(r.category)+'</td><td style="max-width:380px">'+esc(r.message)+'</td>'+
                   '<td><button data-resolve="'+r.id+'">Resolve</button></td></tr>';
              });
              h+='</tbody></table>';
            }
            $('tab-reports').innerHTML=h;
            $('tab-reports').querySelectorAll('[data-resolve]').forEach(function(btn){
              btn.addEventListener('click',function(){
                var note=prompt('Resolution note (what was checked or changed):')||'';
                api('/api/world-admin/reports/'+btn.dataset.resolve+'/resolve','POST',{note:note})
                  .then(loadReports)
                  .catch(function(e){alert((e&&(e.message||e.error))||'Could not resolve');});
              });
            });
          });
        }
        function loadAudit(){
          api('/api/world-admin/audit').then(function(o){
            var h='<h2>Audit log</h2><table><thead><tr><th>When (UTC)</th><th>Who</th><th>Action</th><th>Detail</th></tr></thead><tbody>';
            o.rows.forEach(function(r){h+='<tr><td>'+esc(r.created_at)+'</td><td>'+esc(r.email)+'</td><td>'+esc(r.action)+'</td><td>'+esc(r.detail)+'</td></tr>';});
            $('tab-audit').innerHTML=h+'</tbody></table>';
          });
        }
        if(localStorage.getItem(KEY)){show(true);tab('overview');}else{show(false);}
        })();
        </script>
        </body>
        </html>
        """;
}
