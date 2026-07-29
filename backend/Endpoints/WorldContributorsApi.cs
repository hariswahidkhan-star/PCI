using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI World contributor publishing — the write path (docs/pciworld/CCP_PHASE6_DESIGN.md §4, §5).
///
/// Two audiences, one manuscript:
///   /api/world/contributor/…        a Passport account writing under PCI's byline
///   /api/world-admin/editorial/…    the staff editors who decide whether it publishes
///
/// THE RULE THIS FILE ENFORCES: a contributor may never approve or publish their own article, and
/// no route here reaches approval or publication at all. Those live in <c>WorldEditorial</c> behind
/// the maker-checker, and this module deliberately does NOT expose a contributor-side shortcut to
/// them — the surest way to be sure a control cannot be bypassed is not to build the bypass.
///
/// A contributor's queries are scoped to <c>contributor_user_id = their own account</c> on every
/// read and every write. Internal review notes, legal-review rows, other people's manuscripts and
/// anything in the moderation evidence store are not merely filtered out — they are not in these
/// queries at all, which is a different and stronger property.
/// </summary>
public static class WorldContributorsApi
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        bool Enabled() => WorldContributors.Enabled(db);
        IResult Off() => Results.NotFound();

        void Audit(long? adminId, string action, string? detail)
        {
            try { db.Execute("INSERT INTO pciworld_audit(admin_id,action,detail) VALUES(?,?,?)", adminId, action, detail); }
            catch { /* auditing must never break the action it records */ }
        }

        WorldAccount.UserCtx? Who(HttpContext ctx) => WorldAccount.FromReq(ctx.Request, db);

        IResult? AdminGate(HttpContext ctx, string action, out WorldAdmin.Ctx? adm)
        {
            adm = WorldAdmin.FromReq(ctx.Request, db);
            if (adm is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!WorldRbac.Allowed(adm.Role, action))
                return Results.Json(new { error = "forbidden", required = action }, statusCode: 403);
            return null;
        }

        void Event(long articleId, string ev, string? from, string? to, string kind, long? actor, string? note, string? decl = null) =>
            db.Execute(
                @"INSERT INTO pciworld_contributor_events
                    (article_id,event,from_status,to_status,actor_kind,actor_id,note,declarations_json)
                  VALUES(?,?,?,?,?,?,?,?)",
                articleId, ev, from, to, kind, actor, note, decl);

        // ── Applying to contribute ─────────────────────────────────────────────────────────────

        app.MapPost("/api/world/contributor/apply", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var reason = WorldContributors.Apply(db, user.Id, Str(b, "statement"));
            if (reason == "terms_unavailable")
                // Fail closed rather than record acceptance of a placeholder. terms_version must
                // pin words the applicant actually saw (CCP-P6-019).
                return Results.Json(new
                {
                    error = reason,
                    notice = "Contributor applications are not open yet: the contributor terms have not been published.",
                }, statusCode: 503);
            if (reason is not null)
                return Results.Json(new { error = reason }, statusCode: reason is "email_not_verified" ? 403 : 409);
            Audit(null, "contributor.applied", $"user={user.Id}");
            return Results.Json(new
            {
                ok = true, state = "applied",
                notice = "Your application is with the editorial team. Contributor status is granted by a person, "
                       + "not automatically, and there is no guaranteed timescale.",
            });
        });

        app.MapGet("/api/world/contributor/me", (HttpContext ctx) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var c = db.QueryOne(
                "SELECT state,statement,terms_version,granted_at,revoked_at,revoke_reason,version FROM pciworld_contributors WHERE user_id=?",
                user.Id);
            return Results.Json(new
            {
                state = c is null ? "none" : H.Str(c["state"]),
                may_write = c is not null && H.Str(c["state"]) == "granted",
                terms_version = c is null ? null : H.Str(c["terms_version"]),
                granted_at = c is null ? null : H.Str(c["granted_at"]),
                // A revoked contributor is told, in words and with the reason recorded at the time.
                // A silent revocation looks like a bug to the person it happened to.
                revoked_at = c is null ? null : H.Str(c["revoked_at"]),
                revoke_reason = c is null ? null : H.Str(c["revoke_reason"]),
                terms_available = !string.IsNullOrWhiteSpace(WorldContributors.TermsVersion(db)),
            });
        });

        // ── The manuscript ─────────────────────────────────────────────────────────────────────

        app.MapPost("/api/world/contributor/articles", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (!WorldContributors.MayPublish(db, user.Id))
                return Results.Json(new { error = "not_a_contributor" }, statusCode: 403);

            var b = await H.Body(ctx.Request);
            var title = Str(b, "title");
            if (string.IsNullOrWhiteSpace(title) || title!.Trim().Length < 8)
                return Results.Json(new { error = "bad_title" }, statusCode: 400);

            // Per-account budget (§10). Editor attention is the scarce resource this protects, and
            // a per-IP limit would punish a shared office for one person's behaviour.
            var open = db.Scalar<long>(
                @"SELECT COUNT(*) FROM pciworld_articles
                   WHERE contributor_user_id=? AND status NOT IN ('published','archived')", user.Id);
            if (open >= 10) return Results.Json(new { error = "draft_budget_exhausted" }, statusCode: 429);

            var slug = WorldEditorial.Slugify(title);
            if (db.Scalar<long>("SELECT COUNT(*) FROM pciworld_articles WHERE slug=?", slug) > 0)
                slug = $"{slug}-{H.NowMillis % 100000}";

            // kind is forced to 'blog'. News carries source-tracing obligations the contributor
            // pipeline does not implement, and letting a submission claim to be news would be
            // claiming an editorial standard nothing enforces.
            var id = db.ExecuteReturningId(
                @"INSERT INTO pciworld_articles(slug,kind,title,dek,body_md,author_name,status,contributor_user_id,contributor_terms_version)
                  VALUES(?,'blog',?,?,?,?,'drafting',?,?)",
                slug, title.Trim(), Str(b, "dek"), Str(b, "body_md"),
                user.DisplayName ?? "PCI World contributor", user.Id, WorldContributors.TermsVersion(db));
            Event(id, "note", null, "drafting", "contributor", user.Id, "manuscript created");
            return Results.Json(new { ok = true, article_id = id, slug, status = "drafting" });
        });

        app.MapPatch("/api/world/contributor/articles/{id:long}", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            // Ownership is a PREDICATE on the read, not a check after it: a row belonging to
            // somebody else is never in memory to be leaked by a later logging change.
            var row = db.QueryOne(
                "SELECT status FROM pciworld_articles WHERE id=? AND contributor_user_id=?", id, user.Id);
            if (row is null) return Results.NotFound();
            var status = H.Str(row["status"]);
            // Once it is with an editor — or published — the contributor's copy is closed. A
            // published article is never edited in place by anyone; corrections are a new version
            // with a dated public note, and that path belongs to the editorial engine.
            if (!WorldEditorial.CanEdit(status))
                return Results.Json(new { error = "not_editable", status }, statusCode: 409);

            var b = await H.Body(ctx.Request);
            db.Execute(
                @"UPDATE pciworld_articles SET title=COALESCE(?,title), dek=COALESCE(?,dek),
                         body_md=COALESCE(?,body_md), updated_at=datetime('now') WHERE id=?",
                Str(b, "title"), Str(b, "dek"), Str(b, "body_md"), id);
            return Results.Json(new { ok = true });
        });

        app.MapPost("/api/world/contributor/articles/{id:long}/submit", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (!WorldContributors.MayPublish(db, user.Id))
                return Results.Json(new { error = "not_a_contributor" }, statusCode: 403);
            var row = db.QueryOne(
                "SELECT status FROM pciworld_articles WHERE id=? AND contributor_user_id=?", id, user.Id);
            if (row is null) return Results.NotFound();
            var from = H.Str(row["status"]);
            // Submittable is NARROWER than editable, deliberately. WorldEditorial.CanEdit still
            // allows edits during technical_review — a first-stage reviewer working with the author
            // is normal — but SUBMITTING again from there is not: it would let someone re-answer the
            // declarations after an editor had already read the first set, which is precisely what
            // freezing them at submission exists to prevent. The event rows keep every version, so
            // the record survives either way; what must not happen is the editor being shown a
            // different declaration from the one they reviewed.
            if (from is not ("idea" or "drafting"))
                return Results.Json(new { error = "already_submitted", status = from }, statusCode: 409);

            var b = await H.Body(ctx.Request);
            // Declarations are frozen into the submission event rather than kept as a mutable
            // column: what a contributor declared AT SUBMISSION is the thing a later dispute turns
            // on, and a column would let it be edited afterwards.
            var decl = b.TryGetValue("declarations", out var d) ? d.ToString() : null;
            if (string.IsNullOrWhiteSpace(decl))
                return Results.Json(new { error = "declarations_required" }, statusCode: 400);

            var reason = WorldEditorial.Submit(db, id, "technical_review");
            if (reason is not null) return Results.Json(new { error = reason }, statusCode: 400);
            db.Execute("UPDATE pciworld_articles SET declarations_json=? WHERE id=?", decl, id);
            Event(id, "submitted", from, "technical_review", "contributor", user.Id, null, decl);
            return Results.Json(new
            {
                ok = true, status = "technical_review",
                notice = "Submitted for editorial review. Every contributor article is read by an editor before "
                       + "publication; nothing you submit appears on the site until a person accepts it.",
            });
        });

        app.MapGet("/api/world/contributor/articles", (HttpContext ctx) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var rows = db.Query(
                @"SELECT id,slug,title,status,current_version,published_at,updated_at
                  FROM pciworld_articles WHERE contributor_user_id=? ORDER BY id DESC", user.Id);
            return Results.Json(new { articles = rows.Select(a => new
            {
                id = H.L(a["id"]), slug = H.Str(a["slug"]), title = H.Str(a["title"]),
                status = H.Str(a["status"]), version = H.L(a["current_version"]),
                published_at = H.Str(a["published_at"]), updated_at = H.Str(a["updated_at"]),
                editable = WorldEditorial.CanEdit(H.Str(a["status"])),
            }) });
        });

        app.MapGet("/api/world/contributor/articles/{id:long}", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var a = db.QueryOne(
                @"SELECT id,slug,title,dek,body_md,status,current_version,published_at
                  FROM pciworld_articles WHERE id=? AND contributor_user_id=?", id, user.Id);
            if (a is null) return Results.NotFound();
            // The history addressed to THEM. Internal review rows (pciworld_article_reviews) are
            // not queried here at all — an editor's candid technical note is written on the
            // assumption it is internal, and surfacing it would change what editors are willing to
            // write down.
            var events = db.Query(
                @"SELECT event,from_status,to_status,actor_kind,note,created_at
                  FROM pciworld_contributor_events WHERE article_id=? ORDER BY id", id);
            var msgs = db.Query(
                "SELECT sender_kind,body,created_at FROM pciworld_contributor_messages WHERE article_id=? ORDER BY id", id);
            return Results.Json(new
            {
                id = H.L(a["id"]), slug = H.Str(a["slug"]), title = H.Str(a["title"]),
                dek = H.Str(a["dek"]), body_md = H.Str(a["body_md"]), status = H.Str(a["status"]),
                version = H.L(a["current_version"]), published_at = H.Str(a["published_at"]),
                editable = WorldEditorial.CanEdit(H.Str(a["status"])),
                history = events.Select(e => new
                {
                    @event = H.Str(e["event"]), from = H.Str(e["from_status"]), to = H.Str(e["to_status"]),
                    by = H.Str(e["actor_kind"]), note = H.Str(e["note"]), at = H.Str(e["created_at"]),
                }),
                messages = msgs.Select(m => new
                {
                    from = H.Str(m["sender_kind"]), body = H.Str(m["body"]), at = H.Str(m["created_at"]),
                }),
            });
        });

        app.MapPost("/api/world/contributor/articles/{id:long}/messages", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (db.QueryOne("SELECT id FROM pciworld_articles WHERE id=? AND contributor_user_id=?", id, user.Id) is null)
                return Results.NotFound();
            var b = await H.Body(ctx.Request);
            var body = Str(b, "body");
            if (string.IsNullOrWhiteSpace(body)) return Results.Json(new { error = "empty" }, statusCode: 400);
            if (body!.Length > 4000) return Results.Json(new { error = "too_long" }, statusCode: 400);
            db.Execute(
                "INSERT INTO pciworld_contributor_messages(article_id,sender_kind,sender_id,body) VALUES(?,'contributor',?,?)",
                id, user.Id, body.Trim());
            return Results.Json(new { ok = true });
        });

        // ── Editorial staff ────────────────────────────────────────────────────────────────────

        app.MapGet("/api/world-admin/editorial/contributors", (HttpContext ctx) =>
        {
            if (AdminGate(ctx, "editorial.contributors", out _) is { } deny) return deny;
            var rows = db.Query(
                @"SELECT c.user_id,c.state,c.statement,c.terms_version,c.granted_at,c.revoked_at,
                         c.revoke_reason,c.version,u.display_name,u.email_verified
                  FROM pciworld_contributors c JOIN pciworld_users u ON u.id=c.user_id
                  ORDER BY CASE c.state WHEN 'applied' THEN 0 ELSE 1 END, c.id DESC");
            return Results.Json(new { contributors = rows.Select(c => new
            {
                user_id = H.L(c["user_id"]), state = H.Str(c["state"]),
                display_name = H.Str(c["display_name"]), statement = H.Str(c["statement"]),
                terms_version = H.Str(c["terms_version"]), email_verified = H.B(c["email_verified"]),
                granted_at = H.Str(c["granted_at"]), revoked_at = H.Str(c["revoked_at"]),
                revoke_reason = H.Str(c["revoke_reason"]), version = H.L(c["version"]),
                // Forum standing travels with the application as EVIDENCE for the editor. It is
                // shown, never applied: a threshold crossing cannot make the assertion that this
                // person may write under PCI's byline (§4.1).
                forum_accepted_posts = db.Scalar<long>(
                    "SELECT COALESCE(MAX(accepted_posts),0) FROM pciworld_forum_standing WHERE user_id=?", H.L(c["user_id"])),
                forum_upheld_reports = db.Scalar<long>(
                    "SELECT COALESCE(MAX(upheld_reports),0) FROM pciworld_forum_standing WHERE user_id=?", H.L(c["user_id"])),
            }) });
        });

        // The editor's queue, which the generic /api/world-admin/articles list cannot serve: it knows
        // nothing about contributors, so it cannot show WHOSE submission a manuscript is, what they
        // declared, or — the important one — that the person looking at it is the person who wrote
        // it. Surfacing that conflict in the LIST rather than only on the refusal is the point: an
        // editor should see it before they reach for the approve button, not after.
        app.MapGet("/api/world-admin/editorial/queue", (HttpContext ctx) =>
        {
            if (AdminGate(ctx, "read", out var adm) is { } deny) return deny;
            var rows = db.Query(
                @"SELECT a.id,a.slug,a.title,a.status,a.updated_at,a.declarations_json,
                         a.contributor_user_id,u.display_name
                  FROM pciworld_articles a
                  JOIN pciworld_users u ON u.id = a.contributor_user_id
                  WHERE a.contributor_user_id IS NOT NULL AND a.status NOT IN ('published','archived')
                  ORDER BY a.updated_at");
            return Results.Json(new { queue = rows.Select(r => new
            {
                id = H.L(r["id"]), slug = H.Str(r["slug"]), title = H.Str(r["title"]),
                status = H.Str(r["status"]), updated_at = H.Str(r["updated_at"]),
                contributor = H.Str(r["display_name"]),
                // What they declared AT SUBMISSION, so the editor reads the same words the record
                // froze rather than a later edit of them.
                declarations = H.Str(r["declarations_json"]),
                // The maker-checker, evaluated per row for the admin actually looking. Advisory in
                // the UI; the server refuses independently in Review/Approve/Publish.
                self_review = WorldContributors.IsSelfReview(db, H.L(r["id"]), adm!.Id),
            }) });
        });

        app.MapPost("/api/world-admin/editorial/contributors/{userId:long}/decision", async (HttpContext ctx, long userId) =>
        {
            if (AdminGate(ctx, "editorial.contributors", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var reason = WorldContributors.Decide(
                db, userId, (Str(b, "decision") ?? "").ToLowerInvariant(), adm!.Id,
                Str(b, "reason"), (int)(Lng(b, "version") ?? 0));
            if (reason is not null)
                return Results.Json(new { error = reason }, statusCode: reason switch
                {
                    "conflict" => 409, "forbidden" => 403, "not_found" => 404, _ => 400,
                });
            return Results.Json(new { ok = true });
        });

        app.MapPost("/api/world-admin/editorial/articles/{id:long}/assign", async (HttpContext ctx, long id) =>
        {
            if (AdminGate(ctx, "review", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var editorId = Lng(b, "editor_admin_id") ?? adm!.Id;
            // An editor may not take a manuscript they themselves submitted. Refusing the
            // ASSIGNMENT as well as the approval is deliberate: it keeps the conflict visible at
            // the point a human would notice it, rather than only at the moment of publication.
            if (WorldContributors.IsSelfReview(db, id, editorId))
                return Results.Json(new { error = "maker_checker" }, statusCode: 403);
            db.Transaction(() =>
            {
                db.Execute(
                    "UPDATE pciworld_contributor_assignments SET unassigned_at=datetime('now') WHERE article_id=? AND unassigned_at IS NULL",
                    id);
                db.Execute(
                    "INSERT INTO pciworld_contributor_assignments(article_id,editor_admin_id) VALUES(?,?)",
                    id, editorId);
                Event(id, "assigned", null, null, "editor", adm!.Id, null);
            });
            Audit(adm!.Id, "contributor.article.assigned", $"article={id} editor={editorId}");
            return Results.Json(new { ok = true });
        });

        app.MapPost("/api/world-admin/editorial/articles/{id:long}/messages", async (HttpContext ctx, long id) =>
        {
            if (AdminGate(ctx, "review", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var body = Str(b, "body");
            if (string.IsNullOrWhiteSpace(body)) return Results.Json(new { error = "empty" }, statusCode: 400);
            db.Execute(
                "INSERT INTO pciworld_contributor_messages(article_id,sender_kind,sender_id,body) VALUES(?,'editor',?,?)",
                id, adm!.Id, body.Trim());
            return Results.Json(new { ok = true });
        });

        // The link the maker-checker is made in. Owner-only and audited, because an admin who could
        // set their OWN link could also clear it the moment it became inconvenient — which would
        // turn the check into something the person it constrains can switch off.
        app.MapPost("/api/world-admin/editorial/admins/{adminId:long}/world-link", async (HttpContext ctx, long adminId) =>
        {
            var adm = WorldAdmin.FromReq(ctx.Request, db);
            if (adm is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (adm.Role != "owner") return Results.Json(new { error = "owner_only" }, statusCode: 403);
            var b = await H.Body(ctx.Request);
            var worldUserId = Lng(b, "world_user_id");
            if (worldUserId is not null &&
                db.QueryOne("SELECT id FROM pciworld_users WHERE id=?", worldUserId) is null)
                return Results.Json(new { error = "no_such_account" }, statusCode: 404);
            db.Execute("UPDATE pciworld_admin_users SET world_user_id=? WHERE id=?", worldUserId, adminId);
            Audit(adm.Id, "admin.world_link", $"admin={adminId} world_user={worldUserId?.ToString() ?? "cleared"}");
            return Results.Json(new { ok = true });
        });
    }

    static string? Str(Dictionary<string, JsonElement> b, string key) =>
        b.TryGetValue(key, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;

    static long? Lng(Dictionary<string, JsonElement> b, string key) =>
        b.TryGetValue(key, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out var l) ? l : null;
}
