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

        // Who am I — lets the shell restore the signed-in admin's role after a reload, so the UI
        // can hide what WorldRbac will refuse anyway (the backend stays the authority).
        app.MapGet("/api/world-admin/auth/me", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out var adm) is { } blocked) return blocked;
            return J(new { name = adm!.Name, email = adm.Email, role = adm.Role });
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
            // A password change is the response to a suspected compromise: every OTHER session for
            // this admin dies with the old password. Keeping them alive would let a stolen bearer
            // token outlive the very action taken to revoke it. The caller's own session survives.
            var mine = ctx.Request.Headers.Authorization.ToString();
            var mineSha = mine.StartsWith("Bearer ") ? Security.Sha(mine[7..]) : "";
            var revoked = db.Execute("DELETE FROM pciworld_admin_sessions WHERE admin_id=? AND token<>?", adm.Id, mineSha);
            Audit(adm.Id, "password_change", revoked > 0 ? $"revoked {revoked} other session(s)" : null);
            return J(new { ok = true, sessions_revoked = revoked });
        });

        // ───────────────────────────── challenges ─────────────────────────────

        app.MapGet("/api/world-admin/challenges", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            // Paginated in SQL. The unbounded list was the sharpest admin-scale break in the Phase 0
            // audit: at a 10,000-challenge bank it loaded every row and built a 10,000-row table.
            var limit = Math.Clamp((int)(H.QL(ctx, "limit") ?? 100), 1, 500);
            var offset = Math.Max(0, (int)(H.QL(ctx, "offset") ?? 0));
            var q = (ctx.Request.Query["q"].ToString() ?? "").Trim();
            var where = ""; var args = new List<object?>();
            if (q.Length > 0) { where = " WHERE (code LIKE ? OR title LIKE ?)"; args.Add("%" + q + "%"); args.Add("%" + q + "%"); }
            var total = db.Scalar<long>($"SELECT COUNT(*) FROM pciworld_challenges{where}", args.ToArray());
            var rows = db.Query($@"SELECT id,code,title,industry,track,difficulty,status,retired,current_version,
                    author_id,approved_by,updated_at FROM pciworld_challenges{where} ORDER BY id ASC LIMIT {limit} OFFSET {offset}", args.ToArray())
                .Select(r => new
                {
                    id = H.L(r["id"]), code = H.Str(r["code"]), title = H.Str(r["title"]),
                    industry = H.Str(r["industry"]), track = H.Str(r["track"]), difficulty = H.Str(r["difficulty"]),
                    status = H.Str(r["status"]), retired = H.L(r["retired"]) == 1,
                    current_version = H.L(r["current_version"]), updated_at = H.Str(r["updated_at"]),
                }).ToList();
            return J(new { rows, total, limit, offset });
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

        // ───────────── editorial: articles, sources, entities, reviews ─────────────

        app.MapGet("/api/world-admin/articles", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var limit = Math.Clamp((int)(H.QL(ctx, "limit") ?? 100), 1, 500);
            var offset = Math.Max(0, (int)(H.QL(ctx, "offset") ?? 0));
            var kind = ctx.Request.Query["kind"].ToString();
            var where = WorldEditorial.Kinds.Contains(kind) ? " WHERE kind=?" : "";
            var args = where.Length > 0 ? new object?[] { kind } : Array.Empty<object?>();
            var total = db.Scalar<long>($"SELECT COUNT(*) FROM pciworld_articles{where}", args);
            var rows = db.Query($@"SELECT id,slug,kind,title,status,current_version,author_name,published_at,updated_at
                    FROM pciworld_articles{where} ORDER BY id DESC LIMIT {limit} OFFSET {offset}", args)
                .Select(r => new { id = H.L(r["id"]), slug = H.Str(r["slug"]), kind = H.Str(r["kind"]),
                                   title = H.Str(r["title"]), status = H.Str(r["status"]),
                                   version = H.L(r["current_version"]), author = H.Str(r["author_name"]),
                                   published_at = H.Str(r["published_at"]), updated_at = H.Str(r["updated_at"]) });
            return J(new { rows, total, limit, offset });
        });

        app.MapGet("/api/world-admin/articles/{id:long}", (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var a = db.QueryOne("SELECT * FROM pciworld_articles WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var issues = WorldEditorial.Validate(WorldEditorial.InputFor(db, a));
            return J(new
            {
                id, slug = H.Str(a["slug"]), kind = H.Str(a["kind"]), title = H.Str(a["title"]),
                dek = H.Str(a["dek"]), body_md = H.Str(a["body_md"]), author_name = H.Str(a["author_name"]),
                tags_json = H.Str(a["tags_json"]), seo_title = H.Str(a["seo_title"]), seo_desc = H.Str(a["seo_desc"]),
                status = H.Str(a["status"]), version = H.L(a["current_version"]),
                words = WorldEditorial.WordCount(H.Str(a["body_md"])),
                corrections = WorldEditorial.ParseCorrections(H.Str(a["corrections_json"])),
                issues = issues.Select(i => new { code = i.Code, message = i.Message }),
                sources = db.Query(@"SELECT s.id, s.url, s.publisher, s.title, s.published_at, s.tier, asrc.claim
                        FROM pciworld_article_sources asrc JOIN pciworld_sources s ON s.id=asrc.source_id
                        WHERE asrc.article_id=? ORDER BY asrc.id", id)
                    .Select(s => new { id = H.L(s["id"]), url = H.Str(s["url"]), publisher = H.Str(s["publisher"]),
                                       title = H.Str(s["title"]), published_at = H.Str(s["published_at"]),
                                       tier = H.Str(s["tier"]), claim = H.Str(s["claim"]) }),
                reviews = db.Query("SELECT * FROM pciworld_article_reviews WHERE article_id=? ORDER BY id DESC", id)
                    .Select(r => new { kind = H.Str(r["kind"]), outcome = H.Str(r["outcome"]),
                                       note = H.Str(r["note"]), at = H.Str(r["created_at"]) }),
            });
        });

        app.MapPost("/api/world-admin/articles", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "author", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var kind = H.GetS(b, "kind") ?? "blog";
            if (!WorldEditorial.Kinds.Contains(kind)) return Results.Json(new { error = "bad_kind" }, statusCode: 400);
            var title = (H.GetS(b, "title") ?? "").Trim();
            var slug = (H.GetS(b, "slug") ?? "").Trim();
            if (slug.Length == 0) slug = WorldEditorial.Slugify(title);
            if (slug.Length < 3) return Results.Json(new { error = "bad_slug", message = "A slug of at least three characters is required." }, statusCode: 400);
            if (db.QueryOne("SELECT id FROM pciworld_articles WHERE slug=?", slug) is not null)
                return Results.Json(new { error = "duplicate_slug", message = "That address is already in use." }, statusCode: 409);
            // author_id is stamped at creation so maker-checker has something to compare against.
            var id = db.ExecuteReturningId(@"INSERT INTO pciworld_articles(slug,kind,title,dek,body_md,author_name,author_id,status)
                VALUES(?,?,?,?,?,?,?, 'drafting')",
                slug, kind, title, H.GetS(b, "dek"), H.GetS(b, "body_md"),
                H.GetS(b, "author_name") ?? WorldEditorial.EditorialByline, adm!.Id);
            Audit(adm.Id, "article_create", $"{kind} #{id} {slug}");
            return J(new { ok = true, id, slug });
        });

        app.MapPut("/api/world-admin/articles/{id:long}", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "author", out var adm) is { } blocked) return blocked;
            var a = db.QueryOne("SELECT status FROM pciworld_articles WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            // Published text changes ONLY through the correction path, which records why.
            if (!WorldEditorial.CanEdit(H.Str(a["status"])))
                return Results.Json(new { error = "not_editable", message = "Published or in-review articles change through the correction path, which records the reason." }, statusCode: 409);
            var b = await H.Body(ctx.Request);
            db.Execute(@"UPDATE pciworld_articles SET title=?, dek=?, body_md=?, author_name=?, tags_json=?,
                    seo_title=?, seo_desc=?, author_id=?, updated_at=datetime('now') WHERE id=?",
                H.GetS(b, "title"), H.GetS(b, "dek"), H.GetS(b, "body_md"),
                H.GetS(b, "author_name") ?? WorldEditorial.EditorialByline, H.GetS(b, "tags_json"),
                H.GetS(b, "seo_title"), H.GetS(b, "seo_desc"), adm!.Id, id);
            Audit(adm.Id, "article_update", $"#{id}");
            return J(new { ok = true });
        });

        app.MapPost("/api/world-admin/articles/{id:long}/status", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "author", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var to = H.GetS(b, "status") ?? "";
            var err = WorldEditorial.Submit(db, id, to);
            if (err is not null) return Results.Json(new { error = err }, statusCode: 409);
            Audit(adm!.Id, "article_status", $"#{id} → {to}");
            return J(new { ok = true });
        });

        app.MapPost("/api/world-admin/articles/{id:long}/review", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "review", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var err = WorldEditorial.Review(db, id, H.GetS(b, "kind") ?? "", adm!.Id,
                H.GetS(b, "outcome") ?? "", H.GetS(b, "note"));
            if (err is not null) return Results.Json(new { error = err }, statusCode: 409);
            Audit(adm.Id, "article_review", $"#{id} {H.GetS(b, "kind")}={H.GetS(b, "outcome")}");
            return J(new { ok = true });
        });

        app.MapPost("/api/world-admin/articles/{id:long}/approve", (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "review", out var adm) is { } blocked) return blocked;
            var err = WorldEditorial.Approve(db, id, adm!.Id);
            if (err is not null) return Results.Json(new { error = err }, statusCode: 409);
            Audit(adm.Id, "article_approve", $"#{id}");
            return J(new { ok = true });
        });

        app.MapPost("/api/world-admin/articles/{id:long}/publish", (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "publish", out var adm) is { } blocked) return blocked;
            var err = WorldEditorial.Publish(db, id, adm!.Id);
            if (err is not null) return Results.Json(new { error = err }, statusCode: 409);
            Audit(adm.Id, "article_publish", $"#{id}");
            return J(new { ok = true });
        });

        app.MapPost("/api/world-admin/articles/{id:long}/correct", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "publish", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var err = WorldEditorial.Correct(db, id, adm!.Id, H.GetS(b, "note") ?? "",
                H.GetS(b, "title"), H.GetS(b, "dek"), H.GetS(b, "body_md"));
            if (err is not null) return Results.Json(new { error = err, message = err == "note_required"
                ? "A correction must say what was corrected — the note is published." : null }, statusCode: 409);
            Audit(adm.Id, "article_correct", $"#{id}: {H.GetS(b, "note")}");
            return J(new { ok = true });
        });

        app.MapPost("/api/world-admin/articles/{id:long}/sources", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "author", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var url = (H.GetS(b, "url") ?? "").Trim();
            if (!url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                return Results.Json(new { error = "bad_url", message = "A source must be a reachable http(s) address — model memory is not a source." }, statusCode: 400);
            var claim = (H.GetS(b, "claim") ?? "").Trim();
            if (claim.Length < 8)
                return Results.Json(new { error = "claim_required", message = "Record WHICH claim this source supports. A bibliography proves nothing." }, statusCode: 400);
            var sourceId = db.ExecuteReturningId(@"INSERT INTO pciworld_sources(url,publisher,title,published_at,tier,note)
                VALUES(?,?,?,?,?,?)", url, H.GetS(b, "publisher"), H.GetS(b, "title"),
                H.GetS(b, "published_at"), H.GetS(b, "tier"), H.GetS(b, "note"));
            db.Execute("INSERT INTO pciworld_article_sources(article_id,source_id,claim,confidence) VALUES(?,?,?,?)",
                id, sourceId, claim, H.GetS(b, "confidence"));
            Audit(adm!.Id, "article_source_add", $"#{id} ← {url}");
            return J(new { ok = true, source_id = sourceId });
        });

        // ───────────── rotation console (docs/pciworld/EXPANSION_PHASE0.md §12) ─────────────

        object? RotationCard(Dictionary<string, object?>? ch, Dictionary<string, object?>? period) =>
            ch is null ? null : new
            {
                id = H.L(ch["id"]), code = H.Str(ch["code"]), title = H.Str(ch["title"]),
                difficulty = H.Str(ch["difficulty"]), industry = H.Str(ch["industry"]),
                day_key = period is null ? null : H.Str(period["day_key"]),
                cycle_no = period is null ? (long?)null : H.L(period["cycle_no"]),
                seq_no = period is null ? (long?)null : H.L(period["seq_no"]),
                source = period is null ? null : H.Str(period["source"]),
                reason = period is null ? null : H.Str(period["reason"]),
                // Operator clarity (P2): the version the DAY pinned versus the challenge's current
                // published version — when they differ, a revision shipped mid-day and participants
                // are (correctly) still playing the pinned one.
                pinned_version = period is null ? (long?)null : H.L(period["version"]),
                current_version = H.L(ch["current_version"]),
            };

        app.MapGet("/api/world-admin/rotation", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var now = DateTime.UtcNow;
            var day = WorldRotation.DayKey(db, now);
            var period = WorldRotation.ActivePeriod(db, day);
            var current = period is null ? null : db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", period["challenge_id"]);
            var next = WorldRotation.PeekNext(db, now);
            var lastRun = db.QueryOne("SELECT * FROM pciworld_rotation_runs ORDER BY id DESC LIMIT 1");
            var cycle = period is null ? 1 : H.L(period["cycle_no"]);
            return J(new
            {
                day_key = day,
                timezone = Settings.Str(db, "world_rotation_timezone", "UTC"),
                changes_at = WorldRotation.NextBoundaryUtc(WorldRotation.Zone(db), now).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                paused = WorldRotation.Paused(db),
                shuffle = WorldRotation.ShuffleEnabled(db),
                cycle_no = cycle,
                cycle_length = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_rotation_order WHERE cycle_no=?", cycle),
                eligible = WorldRotation.Eligible(db).Count,
                periods = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_rotation_periods"),
                current = RotationCard(current, period),
                next = RotationCard(next, null),
                last_run = lastRun is null ? null : new
                {
                    outcome = H.Str(lastRun["outcome"]), day_key = H.Str(lastRun["day_key"]),
                    created = H.L(lastRun["periods_created"]), detail = H.Str(lastRun["detail"]),
                    ran_at = H.Str(lastRun["ran_at"]), duration_ms = H.L(lastRun["duration_ms"]),
                },
            });
        });

        app.MapGet("/api/world-admin/rotation/periods", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var limit = Math.Clamp((int)(H.QL(ctx, "limit") ?? 60), 1, 365);
            var offset = Math.Max(0, (int)(H.QL(ctx, "offset") ?? 0));
            var total = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_rotation_periods");
            var rows = db.Query($@"SELECT p.*, c.code, c.title FROM pciworld_rotation_periods p
                    LEFT JOIN pciworld_challenges c ON c.id=p.challenge_id
                    ORDER BY p.day_key DESC, p.revision DESC LIMIT {limit} OFFSET {offset}")
                .Select(r => new
                {
                    day_key = H.Str(r["day_key"]), revision = H.L(r["revision"]),
                    code = H.Str(r["code"]), title = H.Str(r["title"]),
                    version = H.L(r["version"]), cycle_no = H.L(r["cycle_no"]), seq_no = H.L(r["seq_no"]),
                    source = H.Str(r["source"]), reason = H.Str(r["reason"]),
                    superseded = H.Str(r["superseded_at"]) is not null, opened_at = H.Str(r["opened_at"]),
                });
            return J(new { rows, total, limit, offset });
        });

        app.MapGet("/api/world-admin/rotation/runs", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out _) is { } blocked) return blocked;
            var limit = Math.Clamp((int)(H.QL(ctx, "limit") ?? 40), 1, 200);
            var rows = db.Query($"SELECT * FROM pciworld_rotation_runs ORDER BY id DESC LIMIT {limit}")
                .Select(r => new { outcome = H.Str(r["outcome"]), day_key = H.Str(r["day_key"]),
                                   created = H.L(r["periods_created"]), detail = H.Str(r["detail"]),
                                   owner = H.Str(r["owner"]), duration_ms = H.L(r["duration_ms"]), ran_at = H.Str(r["ran_at"]) });
            return J(new { rows });
        });

        app.MapPost("/api/world-admin/rotation/run", (HttpContext ctx) =>
        {
            if (Gate(ctx, "publish", out var adm) is { } blocked) return blocked;
            var r = WorldRotation.RunDue(db, DateTime.UtcNow);
            Audit(adm!.Id, "rotation_run", $"{r.Detail}; {r.Created} period(s) created");
            return J(new { ok = true, created = r.Created, outcome = r.Detail });
        });

        app.MapPost("/api/world-admin/rotation/pause", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "publish", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var paused = H.GetBool(b, "paused") ?? true;
            // Pausing rotation freezes the featured challenge; it does NOT take PCI World offline
            // (that is world_enabled) and it never touches attempts already in progress.
            Settings.Put(db, "world_rotation_enabled", paused ? "0" : "1");
            Audit(adm!.Id, paused ? "rotation_pause" : "rotation_resume", null);
            return J(new { ok = true, paused });
        });

        app.MapPost("/api/world-admin/rotation/settings", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "publish", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var tz = (H.GetS(b, "timezone") ?? "").Trim();
            if (tz.Length > 0)
            {
                // Validate by resolution, not by pattern: an unresolvable id would silently fall back
                // to UTC at read time and the operator would never know the boundary they set is not
                // the boundary in force.
                Settings.Put(db, "world_rotation_timezone", tz);
                var resolved = WorldRotation.Zone(db).Id;
                if (!tz.Equals("UTC", StringComparison.OrdinalIgnoreCase) && resolved == TimeZoneInfo.Utc.Id)
                {
                    Settings.Put(db, "world_rotation_timezone", "UTC");
                    return Results.Json(new { error = "bad_timezone", message = $"'{tz}' is not a timezone this host can resolve. Use an IANA id (Europe/London) or a fixed offset (+04:00)." }, statusCode: 400);
                }
            }
            if (H.GetBool(b, "shuffle") is { } sh) Settings.Put(db, "world_rotation_shuffle", sh ? "1" : "0");
            Audit(adm!.Id, "rotation_settings", $"tz={Settings.Str(db, "world_rotation_timezone", "UTC")} shuffle={WorldRotation.ShuffleEnabled(db)}");
            return J(new { ok = true });
        });

        app.MapPost("/api/world-admin/rotation/substitute", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "publish", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var day = (H.GetS(b, "day_key") ?? WorldRotation.DayKey(db, DateTime.UtcNow)).Trim();
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (!DateTime.TryParseExact(day, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                return Results.Json(new { error = "bad_day", message = "day_key must be YYYY-MM-DD." }, statusCode: 400);
            if (reason.Length < 4)
                return Results.Json(new { error = "reason_required", message = "Record why the day is being changed — the ledger keeps it." }, statusCode: 400);
            var code = (H.GetS(b, "code") ?? "").Trim();
            var ch = db.QueryOne("SELECT id FROM pciworld_challenges WHERE code=?", code);
            if (ch is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var err = WorldRotation.Substitute(db, day, H.L(ch["id"]), adm!.Id, reason);
            if (err is not null) return Results.Json(new { error = err }, statusCode: 409);
            Audit(adm.Id, "rotation_substitute", $"{day} → {code}: {reason}");
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

        // Support diagnostics (PW-US-050): safe lookup for a stuck participant journey — identity
        // references, mapping outcome, states and counts. Never answers, tokens, disclosure
        // choices or photo references; support sees THAT a Passport exists, not its content.
        app.MapGet("/api/world-admin/participants", (HttpContext ctx) =>
        {
            if (Gate(ctx, "read", out var adm) is { } blocked) return blocked;
            var q = ctx.Request.Query["q"].ToString();
            var diag = Core.WorldIdentity.SupportDiagnostics(db, q);
            if (diag is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            Audit(adm!.Id, "participant_lookup", $"#{diag["world_id"]}");
            return J(new { participant = diag });
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
            // Never suspend the final active owner (PW-US-049): a realm with no owner cannot
            // manage itself again. (Suspending yourself is already blocked above, so the acting
            // owner survives — this guards against suspending the OTHER last owner by mistake
            // when the actor is not an owner-by-row, e.g. after their own role changed mid-session.)
            if (status == "suspended" &&
                db.QueryOne("SELECT role FROM pciworld_admin_users WHERE id=?", id) is { } target &&
                H.Str(target["role"]) == "owner" &&
                db.Scalar<long>("SELECT COUNT(*) FROM pciworld_admin_users WHERE role='owner' AND status='active' AND id<>?", id) == 0)
                return Results.Json(new { error = "last_owner", message = "This is the last active owner — appoint another owner first." }, statusCode: 409);
            db.Execute("UPDATE pciworld_admin_users SET status=? WHERE id=?", status, id);
            db.Execute("DELETE FROM pciworld_admin_sessions WHERE admin_id=?", id);
            Audit(adm.Id, "admin_user_status", $"#{id} → {status}");
            return J(new { ok = true });
        });

        // Role changes (PW-US-049): audited, never your own role, never demoting the last active
        // owner. The UI offers this on the Users tab; the guard lives here, where it counts.
        app.MapPost("/api/world-admin/users/{id:long}/role", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "admin", out var adm) is { } blocked) return blocked;
            var b = await H.Body(ctx.Request);
            var role = H.GetS(b, "role") ?? "";
            if (!WorldRbac.Roles.Contains(role)) return Results.Json(new { error = "bad_role" }, statusCode: 400);
            if (id == adm!.Id) return Results.Json(new { error = "self", message = "You cannot change your own role." }, statusCode: 409);
            var target = db.QueryOne("SELECT role,status FROM pciworld_admin_users WHERE id=?", id);
            if (target is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(target["role"]) == "owner" && role != "owner" &&
                db.Scalar<long>("SELECT COUNT(*) FROM pciworld_admin_users WHERE role='owner' AND status='active' AND id<>?", id) == 0)
                return Results.Json(new { error = "last_owner", message = "This is the last active owner — appoint another owner first." }, statusCode: 409);
            db.Execute("UPDATE pciworld_admin_users SET role=? WHERE id=?", role, id);
            // A narrower role must apply immediately — live sessions carry the OLD role otherwise.
            db.Execute("DELETE FROM pciworld_admin_sessions WHERE admin_id=?", id);
            Audit(adm.Id, "admin_user_role", $"#{id} → {role}");
            return J(new { ok = true, role });
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
        <meta name="robots" content="noindex"><meta name="color-scheme" content="light only">
        <title>PCI World Administration</title>
        <link rel="preconnect" href="https://fonts.googleapis.com">
        <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
        <script>window.addEventListener('load',function(){var l=document.createElement('link');l.rel='stylesheet';
          l.href='https://fonts.googleapis.com/css2?family=Archivo:wght@700;800;900&family=Inter:wght@400;500;600;700&display=swap';
          document.head.appendChild(l);});</script>
        <style>
        :root{--bg:#F1F5F9;--ink:#0F172A;--muted:#475569;--line:#E3E8EF;--field:#94A3B8;--accent:#1D4ED8;--accent-deep:#1E3A8A;
              --noir:#0E1525;--crimson:#C13329;--bad:#C2410C;--ok:#15803D;
              --display:'Archivo',system-ui,sans-serif;--sans:'Inter',system-ui,sans-serif}
        *{box-sizing:border-box;margin:0}
        body{background:var(--bg);color:var(--ink);font:15px/1.55 var(--sans);-webkit-font-smoothing:antialiased}
        header{background:var(--noir);color:#E2E8F0;padding:16px 22px;display:flex;gap:14px;align-items:center}
        a.skip{position:absolute;width:1px;height:1px;overflow:hidden;clip:rect(0 0 0 0)}
        a.skip:focus{position:fixed;top:12px;left:12px;width:auto;height:auto;clip:auto;z-index:100;
             background:#fff;color:var(--ink);border:2px solid var(--accent);padding:10px 16px;
             font-weight:600;text-decoration:none}
        :focus-visible{outline:3px solid var(--accent);outline-offset:2px}
        header :focus-visible{outline-color:#93C5FD}
        @media (prefers-reduced-motion:reduce){*{transition:none!important;animation:none!important}}
        header h1{font-family:var(--display);font-weight:900;font-size:17px;letter-spacing:-.02em;color:#fff;margin:0}
        header .bar{width:2px;height:24px;background:var(--crimson);border-radius:2px}
        header small{color:#94A3B8;font-weight:500}
        header button{margin-left:auto}
        main{max-width:1120px;margin:0 auto;padding:26px 22px}
        .card{background:#fff;border:1.5px solid var(--line);border-radius:12px;padding:22px;margin:16px 0;
              box-shadow:0 1px 2px rgba(13,32,90,.05),0 10px 28px -20px rgba(29,78,216,.14)}
        h2{font-family:var(--display);font-weight:800;font-size:17px;letter-spacing:-.01em;margin:0 0 12px}
        table{border-collapse:collapse;width:100%;font-size:14px}
        th,td{padding:10px 10px;border-bottom:1px solid var(--line);text-align:left;vertical-align:top}
        th{font-family:var(--display);font-weight:800;font-size:11px;text-transform:uppercase;letter-spacing:.06em;color:var(--ink)}
        tbody tr:hover{background:var(--bg)}
        button{background:var(--accent);border:2px solid var(--accent);color:#fff;border-radius:0;
               padding:8px 15px;font:600 13px var(--sans);cursor:pointer}
        button:hover{background:var(--accent-deep);border-color:var(--accent-deep)}
        button.ghost{background:transparent;color:var(--ink);border:1.5px solid var(--field)}
        button.ghost:hover{border-color:var(--ink);background:#fff}
        button:focus-visible,input:focus-visible,textarea:focus-visible,select:focus-visible{outline:3px solid var(--accent);outline-offset:2px}
        input,select,textarea{padding:11px 12px;border:1.5px solid var(--field);border-radius:0;font-size:14px;width:100%;
               font-family:var(--sans);color:var(--ink);background:#fff}
        textarea{font-family:ui-monospace,Menlo,Consolas,monospace;min-height:220px}
        label{display:block;font-weight:600;margin:12px 0 5px;font-size:13px}
        .row{display:grid;grid-template-columns:repeat(auto-fit,minmax(180px,1fr));gap:12px}
        .bad{color:var(--bad);font-weight:600} .ok{color:var(--ok);font-weight:600}
        .pill{display:inline-block;border:1.5px solid var(--field);border-radius:999px;padding:2px 11px;font-size:12px;font-weight:600}
        #login{max-width:400px;margin:70px auto;padding:30px}
        #login h2{font-size:22px}
        nav.tabs{display:flex;gap:8px;flex-wrap:wrap;margin:18px 0 4px}
        nav.tabs button{background:transparent;color:var(--ink);border:1.5px solid var(--field)}
        nav.tabs button[aria-selected=true]{background:var(--noir);color:#fff;border-color:var(--noir)}
        </style>
        </head>
        <body>
        <a class="skip" href="#main">Skip to content</a>
        <header><h1>PCI World Administration</h1><span class="bar" aria-hidden="true"></span>
          <small>separate from the PCI Institute admin</small>
          <button id="logout" hidden>Sign out</button></header>
        <main id="main" tabindex="-1">
        <div id="login" class="card" tabindex="-1">
          <h2>Sign in</h2>
          <label for="em">Email</label><input id="em" type="email" autocomplete="username">
          <label for="pw">Password</label><input id="pw" type="password" autocomplete="current-password">
          <p style="margin-top:12px"><button id="doLogin">Sign in</button> <span id="loginerr" class="bad" role="alert"></span></p>
        </div>
        <div id="appmain" hidden>
          <!-- A complete ARIA tabs pattern: every tab points at its panel, every panel names its
               tab, and only the selected tab is in the tab order (arrow keys move between them).
               Half a pattern — role="tab" with no aria-controls or key handling, as this was —
               reads as broken to assistive technology rather than as plain buttons. -->
          <nav class="tabs" role="tablist" aria-label="Administration sections">
            <button role="tab" id="t-overview" aria-controls="tab-overview" data-tab="overview" aria-selected="true" tabindex="0">Overview</button>
            <button role="tab" id="t-challenges" aria-controls="tab-challenges" data-tab="challenges" aria-selected="false" tabindex="-1">Challenges</button>
            <button role="tab" id="t-editor" aria-controls="tab-editor" data-tab="editor" aria-selected="false" tabindex="-1">Editor</button>
            <button role="tab" id="t-rotation" aria-controls="tab-rotation" data-tab="rotation" aria-selected="false" tabindex="-1">Rotation</button>
            <button role="tab" id="t-editorial" aria-controls="tab-editorial" data-tab="editorial" aria-selected="false" tabindex="-1">Editorial</button>
            <button role="tab" id="t-calendar" aria-controls="tab-calendar" data-tab="calendar" aria-selected="false" tabindex="-1">Calendar</button>
            <button role="tab" id="t-reports" aria-controls="tab-reports" data-tab="reports" aria-selected="false" tabindex="-1">Reports</button>
            <button role="tab" id="t-users" aria-controls="tab-users" data-tab="users" aria-selected="false" tabindex="-1" hidden>Users</button>
            <button role="tab" id="t-audit" aria-controls="tab-audit" data-tab="audit" aria-selected="false" tabindex="-1">Audit</button>
          </nav>
          <div id="tab-overview" class="card" role="tabpanel" aria-labelledby="t-overview" tabindex="0"></div>
          <div id="tab-challenges" class="card" role="tabpanel" aria-labelledby="t-challenges" tabindex="0" hidden></div>
          <div id="tab-editor" class="card" role="tabpanel" aria-labelledby="t-editor" tabindex="0" hidden>
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
          <div id="tab-rotation" class="card" role="tabpanel" aria-labelledby="t-rotation" tabindex="0" hidden></div>
          <div id="tab-editorial" class="card" role="tabpanel" aria-labelledby="t-editorial" tabindex="0" hidden></div>
          <div id="tab-calendar" class="card" role="tabpanel" aria-labelledby="t-calendar" tabindex="0" hidden></div>
          <div id="tab-reports" class="card" role="tabpanel" aria-labelledby="t-reports" tabindex="0" hidden></div>
          <div id="tab-users" class="card" role="tabpanel" aria-labelledby="t-users" tabindex="0" hidden></div>
          <div id="tab-audit" class="card" role="tabpanel" aria-labelledby="t-audit" tabindex="0" hidden></div>
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
        // Role-aware UI (PW-US-047): the shell mirrors WorldRbac so people see only what they can
        // do — a hidden button is comfort, never authorization; every endpoint re-checks.
        var ROLE=localStorage.getItem('world_admin_role')||'';
        function can(action){
          if(ROLE==='owner')return true;
          if(ROLE==='author')return action==='read'||action==='author';
          if(ROLE==='reviewer')return action==='read'||action==='review';
          if(ROLE==='publisher')return action==='read'||action==='publish';
          if(ROLE==='viewer')return action==='read';
          return false;
        }
        function applyRole(){
          $('t-editor').hidden=!(can('author'));
          $('t-users').hidden=!(can('admin'));
        }
        var TABS=['overview','challenges','editor','rotation','editorial','calendar','reports','users','audit'];
        function tab(name,moveFocus){
          TABS.forEach(function(t){
            $('tab-'+t).hidden=t!==name;
            var b=document.querySelector('[data-tab='+t+']');
            b.setAttribute('aria-selected',t===name?'true':'false');
            // Roving tabindex: exactly one tab is in the document tab order; the arrow keys move
            // between the tabs themselves. This is what the ARIA tabs pattern requires.
            b.tabIndex=t===name?0:-1;
            if(t===name&&moveFocus)b.focus();
          });
          if(name==='overview')loadOverview(); if(name==='challenges')loadChallenges();
          if(name==='rotation')loadRotation();
          if(name==='editorial')loadEditorial();
          if(name==='calendar')loadCalendar(); if(name==='reports')loadReports(); if(name==='audit')loadAudit();
          if(name==='users')loadUsers();
        }
        document.querySelectorAll('[data-tab]').forEach(function(b){
          b.addEventListener('click',function(){tab(b.dataset.tab);});
          b.addEventListener('keydown',function(ev){
            var i=TABS.indexOf(b.dataset.tab),n=TABS.length;
            if(ev.key==='ArrowRight'||ev.key==='ArrowDown')  {ev.preventDefault();tab(TABS[(i+1)%n],true);}
            else if(ev.key==='ArrowLeft'||ev.key==='ArrowUp'){ev.preventDefault();tab(TABS[(i-1+n)%n],true);}
            else if(ev.key==='Home')                         {ev.preventDefault();tab(TABS[0],true);}
            else if(ev.key==='End')                          {ev.preventDefault();tab(TABS[n-1],true);}
          });
        });
        $('doLogin').addEventListener('click',function(){
          $('loginerr').textContent='';
          api('/api/world-admin/auth/login','POST',{email:$('em').value,password:$('pw').value})
            .then(function(r){localStorage.setItem(KEY,r.token);
              ROLE=r.role||'';localStorage.setItem('world_admin_role',ROLE);applyRole();
              show(true);tab('overview');})
            .catch(function(e){$('loginerr').textContent=(e&&e.message)||'Sign-in failed.';});
        });
        $('logout').addEventListener('click',function(){
          api('/api/world-admin/auth/logout','POST',{}).catch(function(){});
          localStorage.removeItem(KEY);localStorage.removeItem('world_admin_role');ROLE='';show(false);
        });
        function loadOverview(){
          api('/api/world-admin/overview').then(function(o){
            $('tab-overview').innerHTML='<h2>Overview</h2>'+
            '<p>Challenges: total '+o.challenges.total+' · servable '+o.challenges.servable+
            ' · draft '+o.challenges.draft+' · in review '+o.challenges.in_review+
            ' · approved '+o.challenges.approved+' · retired '+o.challenges.retired+'</p>'+
            '<p>Attempts: '+o.attempts.total+' · completed '+o.attempts.completed+
            ' · shared results '+o.attempts.shared+' · live invitations '+o.attempts.invites+'</p>'+
            '<h2 style="margin-top:18px">Security</h2>'+
            '<div class="row" style="max-width:640px">'+
            '<div><label for="pw_cur">Current password</label><input id="pw_cur" type="password" autocomplete="current-password"></div>'+
            '<div><label for="pw_new">New password (min 12 chars)</label><input id="pw_new" type="password" autocomplete="new-password"></div>'+
            '</div>'+
            '<p style="margin-top:10px"><button id="pw_go">Change password</button> <span id="pw_msg" role="status"></span></p>';
            $('pw_go').addEventListener('click',function(){
              api('/api/world-admin/auth/password','POST',{current:$('pw_cur').value,next:$('pw_new').value})
                .then(function(){$('pw_msg').textContent='Password changed — sign in again.';
                  localStorage.removeItem(KEY);setTimeout(function(){show(false);},1200);})
                .catch(function(e){$('pw_msg').textContent=(e&&(e.message||e.error))||'Could not change the password.';});
            });
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
        function loadEditorial(){
          api('/api/world-admin/articles?limit=200').then(function(o){
            var h='<h2>Editorial ('+o.total+') <button id="newart" class="ghost">New article</button> '+
                  '<button id="newnews" class="ghost">New news item</button></h2>'+
              '<p><small>Published text changes only through the correction path, which records a dated, public note. '+
              'A news item cannot be approved without at least one recorded source.</small></p>'+
              '<table><thead><tr><th scope="col">Slug</th><th scope="col">Title</th><th scope="col">Kind</th>'+
              '<th scope="col">Status</th><th scope="col">v</th><th scope="col">Author</th><th scope="col">Actions</th></tr></thead><tbody>';
            o.rows.forEach(function(r){
              h+='<tr><td>'+esc(r.slug)+'</td><td>'+esc(r.title)+'</td><td>'+esc(r.kind)+'</td>'+
                 '<td><span class="pill">'+esc(r.status)+'</span></td><td>'+r.version+'</td>'+
                 '<td>'+esc(r.author||'')+'</td><td>'+articleButtons(r)+'</td></tr>';
            });
            h+='</tbody></table><div id="artedit"></div>';
            $('tab-editorial').innerHTML=h;
            $('newart').addEventListener('click',function(){newArticle('blog');});
            $('newnews').addEventListener('click',function(){newArticle('news');});
            $('tab-editorial').querySelectorAll('[data-aact]').forEach(function(btn){
              btn.addEventListener('click',function(){articleAction(btn.dataset.aact,btn.dataset.id);});
            });
          });
        }
        function articleButtons(r){
          var b='<button class="ghost" data-aact="open" data-id="'+r.id+'">Open</button> ';
          if(r.status==='drafting')b+='<button class="ghost" data-aact="fact_check" data-id="'+r.id+'">To fact-check</button> ';
          if(r.status!=='published')b+='<button class="ghost" data-aact="approve" data-id="'+r.id+'">Approve</button> ';
          if(r.status==='approved')b+='<button data-aact="publish" data-id="'+r.id+'">Publish</button> ';
          if(r.status==='published')b+='<button class="ghost" data-aact="correct" data-id="'+r.id+'">Correct</button> ';
          return b;
        }
        function newArticle(kind){
          var title=window.prompt('Working title for the new '+(kind==='news'?'news item':'article')+':');
          if(!title)return;
          api('/api/world-admin/articles','POST',{kind:kind,title:title})
            .then(function(r){loadEditorial();openArticle(r.id);})
            .catch(function(e){alert((e&&(e.message||e.error))||'Could not create');});
        }
        function articleAction(act,id){
          if(act==='open'){openArticle(id);return;}
          if(act==='fact_check'){
            api('/api/world-admin/articles/'+id+'/status','POST',{status:'fact_check'})
              .then(loadEditorial).catch(function(e){alert((e&&(e.message||e.error))||'Failed');});
            return;
          }
          if(act==='correct'){
            var note=window.prompt('What is being corrected? This note is published with a date.');
            if(!note)return;
            api('/api/world-admin/articles/'+id+'/correct','POST',{note:note})
              .then(loadEditorial).catch(function(e){alert((e&&(e.message||e.error))||'Failed');});
            return;
          }
          api('/api/world-admin/articles/'+id+'/'+act,'POST',{})
            .then(loadEditorial).catch(function(e){alert((e&&(e.message||e.error))||'Failed');});
        }
        function openArticle(id){
          api('/api/world-admin/articles/'+id).then(function(a){
            var issues=a.issues.length
              ? '<p class="bad">Not publishable: '+a.issues.map(function(i){return esc(i.message);}).join(' ')+'</p>'
              : '<p class="ok">Passes the publication gate.</p>';
            var srcs=a.sources.length
              ? '<ul>'+a.sources.map(function(s){return '<li>'+esc(s.url)+' — <small>supports: '+esc(s.claim||'')+'</small></li>';}).join('')+'</ul>'
              : '<p><small>No sources recorded.</small></p>';
            $('artedit').innerHTML='<h2 style="margin-top:26px">'+esc(a.title)+' <span class="pill">'+esc(a.status)+'</span></h2>'+
              '<p><small>/world/'+esc(a.kind)+'/'+esc(a.slug)+' · '+a.words+' words · v'+a.version+'</small></p>'+issues+
              '<label for="a_title">Title</label><input id="a_title" value="'+esc(a.title||'')+'">'+
              '<label for="a_dek">Standfirst</label><input id="a_dek" value="'+esc(a.dek||'')+'">'+
              '<label for="a_author">Author (a real name, or PCI World Editorial)</label><input id="a_author" value="'+esc(a.author_name||'')+'">'+
              '<label for="a_body">Body (Markdown subset)</label><textarea id="a_body" rows="14">'+esc(a.body_md||'')+'</textarea>'+
              '<p style="margin-top:12px"><button id="a_save">Save draft</button> <span id="a_msg" role="status"></span></p>'+
              '<h2 style="margin-top:22px">Sources</h2>'+srcs+
              '<div class="row"><div><label for="s_url">Source URL</label><input id="s_url" placeholder="https://"></div>'+
              '<div><label for="s_claim">Claim it supports</label><input id="s_claim"></div>'+
              '<div><label for="s_pub">Publisher</label><input id="s_pub"></div></div>'+
              '<p style="margin-top:10px"><button id="s_add" class="ghost">Add source</button> <span id="s_msg" role="status"></span></p>';
            $('a_save').addEventListener('click',function(){
              api('/api/world-admin/articles/'+id,'PUT',{title:$('a_title').value,dek:$('a_dek').value,
                   author_name:$('a_author').value,body_md:$('a_body').value})
                .then(function(){$('a_msg').textContent='Saved.';openArticle(id);})
                .catch(function(e){$('a_msg').textContent=(e&&(e.message||e.error))||'Save failed';});
            });
            $('s_add').addEventListener('click',function(){
              api('/api/world-admin/articles/'+id+'/sources','POST',
                  {url:$('s_url').value,claim:$('s_claim').value,publisher:$('s_pub').value})
                .then(function(){openArticle(id);})
                .catch(function(e){$('s_msg').textContent=(e&&(e.message||e.error))||'Failed';});
            });
          });
        }
        function loadRotation(){
          Promise.all([api('/api/world-admin/rotation'),api('/api/world-admin/rotation/periods?limit=30'),
                       api('/api/world-admin/rotation/runs?limit=12')])
          .then(function(res){
            var s=res[0],p=res[1],r=res[2];
            function card(c,label){
              if(!c)return '<div><span class="kicker">'+label+'</span><p>None</p></div>';
              // Pinned vs current (operator clarity): when a revision shipped mid-day the day KEEPS
              // its pinned version — participants are not moved, and the card says so plainly.
              var ver=c.pinned_version?(' · playing v'+c.pinned_version+
                (c.current_version&&c.current_version!==c.pinned_version
                  ?' <b>(a newer v'+c.current_version+' is published — today stays pinned)</b>':'')):'';
              return '<div><span class="kicker">'+label+'</span><p><b>'+esc(c.code)+'</b> — '+esc(c.title)+'<br>'+
                '<small>'+esc(c.difficulty||'')+(c.source?' · '+esc(c.source):'')+ver+
                (c.reason?' · '+esc(c.reason):'')+'</small></p></div>';
            }
            var health=s.last_run
              ? esc(s.last_run.outcome)+' · '+esc(s.last_run.ran_at||'')+' · '+s.last_run.created+' created'+
                (s.last_run.detail?' · '+esc(s.last_run.detail):'')
              : 'never run';
            var h='<h2>Daily rotation</h2>'+
              '<p>Day <b>'+esc(s.day_key)+'</b> ('+esc(s.timezone)+') · changes at '+esc(s.changes_at)+
              ' · cycle <b>'+s.cycle_no+'</b> of '+s.cycle_length+' · '+s.eligible+' eligible · '+s.periods+' recorded days'+
              (s.paused?' · <b>PAUSED</b>':'')+'</p>'+
              '<div class="row">'+card(s.current,'Today')+card(s.next,'Next')+'</div>'+
              '<p><small>Health: '+health+'</small></p>'+
              // Operating controls are a publisher's tools (PW-US-047): other roles see the state
              // of the day — never buttons the backend would refuse anyway.
              (can('publish')
                ?'<p><button id="rot_run">Run the boundary now</button> '+
                 '<button id="rot_pause" class="ghost">'+(s.paused?'Resume rotation':'Pause rotation')+'</button></p>'+
                 '<h2 style="margin-top:18px">Emergency substitution</h2>'+
                 '<p><small>Replaces what is featured on a day. The day it replaces is kept in the ledger, never deleted.</small></p>'+
                 '<div class="row" style="max-width:720px">'+
                 '<div><label for="sub_day">Day (YYYY-MM-DD)</label><input id="sub_day" value="'+esc(s.day_key)+'"></div>'+
                 '<div><label for="sub_code">Challenge code</label><input id="sub_code" placeholder="WC-EVM-001"></div>'+
                 '<div><label for="sub_why">Reason (recorded)</label><input id="sub_why" placeholder="Calculation error reported"></div>'+
                 '</div><p><button id="rot_sub">Substitute</button> <span id="rot_msg" role="status"></span></p>'
                :'<p><small>Rotation controls are available to publishers and owners.</small></p>')+
              '<h2 style="margin-top:18px">Recorded days</h2>'+
              '<table><thead><tr><th scope="col">Day</th><th scope="col">Challenge</th><th scope="col">Cycle</th>'+
              '<th scope="col">Source</th><th scope="col">Note</th></tr></thead><tbody>';
            p.rows.forEach(function(x){
              h+='<tr'+(x.superseded?' style="opacity:.55"':'')+'><td>'+esc(x.day_key)+(x.revision>1?' r'+x.revision:'')+'</td>'+
                 '<td>'+esc(x.code||'—')+' <small>'+esc(x.title||'')+'</small></td>'+
                 '<td>'+x.cycle_no+'.'+x.seq_no+'</td><td><span class="pill">'+esc(x.source)+'</span></td>'+
                 '<td>'+esc(x.reason||'')+(x.superseded?' (superseded)':'')+'</td></tr>';
            });
            h+='</tbody></table><h2 style="margin-top:18px">Scheduler log</h2><table><thead><tr>'+
               '<th scope="col">When</th><th scope="col">Outcome</th><th scope="col">Detail</th></tr></thead><tbody>';
            r.rows.forEach(function(x){
              h+='<tr><td>'+esc(x.ran_at||'')+'</td><td><span class="pill">'+esc(x.outcome)+'</span></td>'+
                 '<td>'+esc(x.detail||'')+'</td></tr>';
            });
            h+='</tbody></table>';
            $('tab-rotation').innerHTML=h;
            if($('rot_run'))$('rot_run').addEventListener('click',function(){
              api('/api/world-admin/rotation/run','POST',{}).then(loadRotation)
                .catch(function(e){$('rot_msg').textContent=(e&&(e.message||e.error))||'Failed';});
            });
            if($('rot_pause'))$('rot_pause').addEventListener('click',function(){
              api('/api/world-admin/rotation/pause','POST',{paused:!s.paused}).then(loadRotation)
                .catch(function(e){$('rot_msg').textContent=(e&&(e.message||e.error))||'Failed';});
            });
            if($('rot_sub'))$('rot_sub').addEventListener('click',function(){
              $('rot_msg').textContent='';
              api('/api/world-admin/rotation/substitute','POST',
                  {day_key:$('sub_day').value,code:$('sub_code').value,reason:$('sub_why').value})
                .then(function(){$('rot_msg').textContent='Substituted.';loadRotation();})
                .catch(function(e){$('rot_msg').textContent=(e&&(e.message||e.error))||'Failed';});
            });
          });
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
            // Support lookup (PW-US-050): read-gated diagnostics for a stuck participant journey.
            // The answer is states and counts — never answers, tokens, disclosure or photos.
            h+='<h2 style="margin-top:22px">Participant lookup (support)</h2>'+
               '<div class="row" style="max-width:560px">'+
               '<div><label for="pl_q">World account id or email</label><input id="pl_q" placeholder="42 or person@example.org"></div>'+
               '</div><p><button id="pl_go">Look up</button></p><div id="pl_out"></div>';
            $('tab-reports').innerHTML=h;
            $('pl_go').addEventListener('click',function(){
              $('pl_out').innerHTML='';
              api('/api/world-admin/participants?q='+encodeURIComponent($('pl_q').value))
                .then(function(o){
                  var p=o.participant,rows=[
                    ['World account','#'+p.world_id+' · '+p.email+' · '+p.status+(p.email_verified?' · verified':' · unverified')],
                    ['Canonical identity',p.canonical_user_id?('users #'+p.canonical_user_id+' ('+p.mapping_outcome+')'):('none — '+p.mapping_outcome+(p.mapping_detail?': '+p.mapping_detail:''))],
                    ['Participation',(p.participation_status||'—')+' · onboarding '+(p.onboarding_state||'—')+(p.goal?' · goal '+p.goal:'')],
                    ['Attempts',p.attempts_total+' total · '+p.attempts_completed+' completed · '+p.attempts_in_progress+' in progress'],
                    ['Passport',(p.passport_public?'published':'private')+(p.passport_expired?' (expired)':'')+' · '+p.visible_evidence+' selected evidence item(s)'],
                    ['Sessions',p.live_sessions+' live'],
                    ['Signed up',p.created_at+(p.last_login_at?' · last sign-in '+p.last_login_at:'')]];
                  var t='<table><tbody>';
                  rows.forEach(function(r2){t+='<tr><th style="text-align:left;padding-right:12px">'+esc(r2[0])+'</th><td>'+esc(r2[1])+'</td></tr>';});
                  $('pl_out').innerHTML=t+'</tbody></table>'+
                    '<p><small>Diagnostics only: answers, tokens, disclosure choices and photographs are never shown here.</small></p>';
                })
                .catch(function(e){$('pl_out').innerHTML='<p class="bad">'+esc((e&&e.error)==='not_found'?'No PCI World account matches that.':'Lookup failed — try again.')+'</p>';});
            });
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
        function loadUsers(){
          api('/api/world-admin/users').then(function(o){
            var h='<h2>World admin users</h2>'+
              '<p><small>Roles: owner (everything) · author (draft &amp; edit) · reviewer (approve/reject) · '+
              'publisher (publish, rotation, calendar) · viewer (read only). Every change is audited; '+
              'the last active owner can never be demoted or suspended.</small></p>'+
              '<table><thead><tr><th scope="col">Email</th><th scope="col">Name</th><th scope="col">Role</th>'+
              '<th scope="col">Status</th><th scope="col">Last sign-in</th><th scope="col">Actions</th></tr></thead><tbody>';
            o.rows.forEach(function(u){
              var roleSel='<select data-role-for="'+u.id+'">'+
                ['owner','author','reviewer','publisher','viewer'].map(function(x){
                  return '<option value="'+x+'"'+(u.role===x?' selected':'')+'>'+x+'</option>';}).join('')+'</select>';
              h+='<tr><td>'+esc(u.email)+'</td><td>'+esc(u.name||'')+'</td>'+
                 '<td>'+roleSel+' <button class="ghost" data-setrole="'+u.id+'">Set</button></td>'+
                 '<td><span class="pill">'+esc(u.status)+'</span></td>'+
                 '<td><small>'+esc(u.last_login_at||'never')+'</small></td>'+
                 '<td><button class="ghost" data-status="'+u.id+'" data-next="'+(u.status==='active'?'suspended':'active')+'">'+
                 (u.status==='active'?'Suspend':'Reactivate')+'</button></td></tr>';
            });
            h+='</tbody></table>'+
               '<h2 style="margin-top:18px">Invite an admin</h2>'+
               '<div class="row" style="max-width:820px">'+
               '<div><label for="nu_email">Email</label><input id="nu_email" type="email"></div>'+
               '<div><label for="nu_name">Name</label><input id="nu_name"></div>'+
               '<div><label for="nu_role">Role</label><select id="nu_role">'+
               ['viewer','author','reviewer','publisher','owner'].map(function(x){return '<option>'+x+'</option>';}).join('')+'</select></div>'+
               '<div><label for="nu_pw">Initial password (min 12)</label><input id="nu_pw" type="password" autocomplete="new-password"></div>'+
               '</div><p><button id="nu_go">Create admin</button> <span id="nu_msg" role="status"></span></p>';
            $('tab-users').innerHTML=h;
            $('nu_go').addEventListener('click',function(){
              $('nu_msg').textContent='';
              api('/api/world-admin/users','POST',{email:$('nu_email').value,name:$('nu_name').value,
                role:$('nu_role').value,password:$('nu_pw').value})
                .then(loadUsers)
                .catch(function(e){$('nu_msg').textContent=(e&&(e.message||e.error))||'Could not create the admin.';});
            });
            document.querySelectorAll('[data-status]').forEach(function(b){
              b.addEventListener('click',function(){
                api('/api/world-admin/users/'+b.dataset.status+'/status','POST',{status:b.dataset.next})
                  .then(loadUsers)
                  .catch(function(e){alert((e&&(e.message||e.error))||'Refused.');});
              });
            });
            document.querySelectorAll('[data-setrole]').forEach(function(b){
              b.addEventListener('click',function(){
                var sel=document.querySelector('[data-role-for="'+b.dataset.setrole+'"]');
                api('/api/world-admin/users/'+b.dataset.setrole+'/role','POST',{role:sel.value})
                  .then(loadUsers)
                  .catch(function(e){alert((e&&(e.message||e.error))||'Refused.');});
              });
            });
          });
        }
        if(localStorage.getItem(KEY)){
          show(true);applyRole();tab('overview');
          // Restore the role after a reload — the shell must never guess it from stale storage
          // alone when the server can say for sure (a mid-session role change signs sessions out,
          // so a live answer here is always current).
          api('/api/world-admin/auth/me').then(function(m){
            ROLE=m.role||'';localStorage.setItem('world_admin_role',ROLE);applyRole();
          }).catch(function(){});
        }else{show(false);}
        })();
        </script>
        </body>
        </html>
        """;
}
