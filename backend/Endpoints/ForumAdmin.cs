using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Forum moderation, plus the member-facing flag (docs/pciworld/CCP_PHASE3_DESIGN.md §7).
///
/// A FLAG IS NOT A REPORT, AND NEITHER IS A SANCTION.
///
///   flag   — "this is low quality". A member's opinion. Weighted by standing, capped, and able to
///            do exactly one thing: hide a post PENDING HUMAN REVIEW.
///   report — "this breaks the rules". Reuses the community case machinery unchanged, so a forum
///            report opens the same kind of case, in the same queue, with the same append-only
///            decision history as a room report. There is no second moderation console.
///
/// The flag's ceiling is the control. Weight is capped below the hide threshold, so no single
/// account — including a compromised trusted one — can hide arbitrary content, and crossing the
/// threshold opens a case for a person rather than deciding anything. A flag never demotes, never
/// bans, and never deletes.
///
/// RELEASING AND WITHHOLDING ARE BOTH RECORDED. A moderator publishing a held post and a moderator
/// taking one down are the same kind of act — a decision about somebody's words — so both write an
/// append-only decision row. Only one of them feels like an intervention, which is exactly why the
/// other is easy to forget to record.
/// </summary>
public static class ForumAdmin
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        void Audit(long? adminId, string action, string? detail)
        {
            try { db.Execute("INSERT INTO pciworld_audit(admin_id,action,detail) VALUES(?,?,?)", adminId, action, detail); }
            catch { /* auditing must never break the action it records */ }
        }

        IResult? Gate(HttpContext ctx, string action, out WorldAdmin.Ctx? adm)
        {
            adm = WorldAdmin.FromReq(ctx.Request, db);
            if (adm is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!WorldRbac.Allowed(adm.Role, action))
                return Results.Json(new { error = "forbidden", required = action }, statusCode: 403);
            return null;
        }

        // ── The review queue ──────────────────────────────────────────────────────────────────

        app.MapGet("/api/world-admin/forum/queue", (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.read", out _) is { } deny) return deny;

            var rows = db.Query(
                @"SELECT p.id, p.state, p.kind, p.flag_weight, p.created_at, p.author_user_id,
                         r.body, r.revision_no,
                         t.title AS thread_title, t.slug AS thread_slug,
                         c.slug AS category_slug,
                         d.reason_code, d.confidence_band, d.category AS decision_category
                  FROM pciworld_forum_posts p
                  JOIN pciworld_forum_post_revisions r ON r.id = p.current_revision_id
                  JOIN pciworld_forum_threads t ON t.id = p.thread_id
                  JOIN pciworld_forum_categories c ON c.id = t.category_id
                  LEFT JOIN pciworld_moderation_decisions d ON d.id = p.decision_id
                  WHERE p.state IN ('pending','withheld')
                  ORDER BY p.flag_weight DESC, p.created_at
                  LIMIT 200");

            return Results.Json(new { posts = rows.Select(p => new
            {
                id = H.L(p["id"]),
                state = H.Str(p["state"]),
                // The reviewer sees the CURRENT revision's raw body. Raw rather than rendered
                // because a moderator is judging what was written, not how it displays.
                body = H.Str(p["body"]),
                revision = H.L(p["revision_no"]),
                // Flagged is surfaced, and so is the fact that flags are advisory.
                flag_weight = H.L(p["flag_weight"]),
                thread = H.Str(p["thread_title"]),
                url = $"/world/forum/{H.Str(p["category_slug"])}/{H.Str(p["thread_slug"])}",
                reason = H.Str(p["reason_code"]),
                band = H.Str(p["confidence_band"]),
                category = H.Str(p["decision_category"]),
                at = H.Str(p["created_at"]),
            }) });
        });

        // ── Release / withhold ────────────────────────────────────────────────────────────────

        app.MapPost("/api/world-admin/forum/posts/{id:long}/release", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "community.moderate", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);

            var post = db.QueryOne("SELECT id,thread_id,state,kind,author_user_id FROM pciworld_forum_posts WHERE id=?", id);
            if (post is null) return Results.NotFound();
            if (H.Str(post["state"]) is not ("pending" or "withheld"))
                return Results.Json(new { error = "post_not_held" }, statusCode: 400);

            db.Transaction(() =>
            {
                db.Execute(
                    @"UPDATE pciworld_forum_posts
                      SET state='published', published_at=datetime('now'), flag_weight=0,
                          updated_at=datetime('now'), version=version+1
                      WHERE id=?", id);

                // Releasing bumps the thread, because now there IS something to read. This is the
                // moment the post becomes activity, not the moment it was written.
                db.Execute(
                    @"UPDATE pciworld_forum_threads
                      SET reply_count = reply_count + ?, last_post_at = datetime('now'),
                          last_post_user_id = ?, updated_at = datetime('now')
                      WHERE id=?",
                    H.Str(post["kind"]) == "reply" ? 1 : 0, H.L(post["author_user_id"]), H.L(post["thread_id"]));

                // A release is a decision about somebody's words and is recorded like any other.
                // It is the easy one to forget precisely because it feels like doing nothing.
                db.Execute(
                    @"INSERT INTO pciworld_moderation_decisions
                          (scope,subject_ref,outcome,reason_code,provider,provider_version)
                      VALUES('forum_post',?,'allow',?,'human',?)",
                    id.ToString(), H.GetS(b, "reason") ?? "released_by_moderator", adm!.Id.ToString());

                // Standing follows the outcome: a released post is an accepted post, which is how
                // somebody climbs out of pre-moderation by writing acceptably.
                db.Execute(
                    @"INSERT OR IGNORE INTO pciworld_forum_standing(user_id,level,first_post_at)
                      VALUES(?,'new',datetime('now'))", H.L(post["author_user_id"]));
                db.Execute(
                    @"UPDATE pciworld_forum_standing SET accepted_posts = accepted_posts + 1,
                             updated_at = datetime('now') WHERE user_id=?", H.L(post["author_user_id"]));
            });

            Audit(adm!.Id, "world_forum_post_released", id.ToString());
            log(adm.Id, "world_forum_post_released", id.ToString());
            return Results.Json(new { ok = true, state = "published" });
        });

        app.MapPost("/api/world-admin/forum/posts/{id:long}/withhold", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "community.moderate", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            // A takedown without a stated reason is one the author cannot appeal and a colleague
            // cannot review. Requiring it costs a sentence.
            if (reason.Length < 3) return Results.Json(new { error = "reason_required" }, statusCode: 400);

            var post = db.QueryOne("SELECT id,thread_id,state,kind FROM pciworld_forum_posts WHERE id=?", id);
            if (post is null) return Results.NotFound();
            var wasPublished = H.Str(post["state"]) == "published";

            db.Transaction(() =>
            {
                db.Execute(
                    @"UPDATE pciworld_forum_posts SET state='withheld', updated_at=datetime('now'), version=version+1
                      WHERE id=?", id);
                if (wasPublished)
                    db.Execute(
                        @"UPDATE pciworld_forum_threads
                          SET reply_count = CASE WHEN reply_count > 0 THEN reply_count - ? ELSE 0 END,
                              updated_at = datetime('now')
                          WHERE id=?",
                        H.Str(post["kind"]) == "reply" ? 1 : 0, H.L(post["thread_id"]));

                db.Execute(
                    @"INSERT INTO pciworld_moderation_decisions
                          (scope,subject_ref,outcome,reason_code,provider,provider_version)
                      VALUES('forum_post',?,'block',?,'human',?)",
                    id.ToString(), reason, adm!.Id.ToString());
            });

            Audit(adm!.Id, "world_forum_post_withheld", $"{id}: {reason}");
            log(adm.Id, "world_forum_post_withheld", id.ToString());
            return Results.Json(new { ok = true, state = "withheld" });
        });

        // ── The member-facing flag ────────────────────────────────────────────────────────────

        app.MapPost("/api/world/forum/posts/{id:long}/flag", async (HttpContext ctx, long id) =>
        {
            if (!ForumPosts.Enabled(db)) return Results.NotFound();
            var user = WorldAccount.FromReq(ctx.Request, db);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);

            var standing = db.QueryOne("SELECT level FROM pciworld_forum_standing WHERE user_id=?", user.Id);
            var level = ForumTrust.Parse(H.Str(standing?["level"]));
            if (!ForumTrust.MayFlag(level)) return Results.Json(new { error = "flagging_not_allowed_yet" }, statusCode: 403);

            var post = db.QueryOne("SELECT id,state,author_user_id FROM pciworld_forum_posts WHERE id=?", id);
            if (post is null) return Results.NotFound();
            if (H.Str(post["state"]) != "published")
                // Nothing to flag. Answering 404 rather than "already hidden" keeps the endpoint
                // from reporting other people's moderation outcomes.
                return Results.NotFound();
            if (H.L(post["author_user_id"]) == user.Id)
                return Results.Json(new { error = "cannot_flag_your_own_post" }, statusCode: 400);

            var b = await H.Body(ctx.Request);
            var weight = ForumTrust.FlagWeight(level);

            try
            {
                db.Execute(
                    "INSERT INTO pciworld_forum_flags(post_id,flagged_by_user_id,weight,reason) VALUES(?,?,?,?)",
                    id, user.Id, weight, H.GetS(b, "reason"));
            }
            catch
            {
                // The unique index did its job: one flag per person per post. Reported as success
                // so a double-tap is not an error, and so the endpoint does not confirm whether a
                // previous flag exists.
                return Results.Json(new { ok = true, already_flagged = true });
            }

            var total = db.Scalar<long>("SELECT COALESCE(SUM(weight),0) FROM pciworld_forum_flags WHERE post_id=?", id);
            db.Execute("UPDATE pciworld_forum_posts SET flag_weight=? WHERE id=?", total, id);

            var hidden = false;
            if (total >= ForumTrust.HideThreshold)
            {
                // The ONLY thing a flag can do. It hides pending human review — it does not
                // sanction the author, does not touch their standing, and does not delete anything.
                db.Execute(
                    @"UPDATE pciworld_forum_posts SET state='pending', updated_at=datetime('now'), version=version+1
                      WHERE id=? AND state='published'", id);
                hidden = true;
                log(null, "world_forum_post_flag_hidden", id.ToString());
            }

            return Results.Json(new
            {
                ok = true,
                // Deliberately does not reveal the running total or the threshold: publishing them
                // would tell a coordinating group exactly how many accounts they need.
                notice = hidden
                    ? "Thanks — this post has been hidden while a moderator reviews it."
                    : "Thanks. A moderator will look at this.",
            });
        });

        // ── Categories ────────────────────────────────────────────────────────────────────────

        app.MapPost("/api/world-admin/forum/categories", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.rooms", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var slug = (H.GetS(b, "slug") ?? "").Trim().ToLowerInvariant();
            if (!System.Text.RegularExpressions.Regex.IsMatch(slug, @"^[a-z0-9][a-z0-9-]{1,118}$"))
                return Results.Json(new { error = "bad_slug" }, statusCode: 400);

            var minTrust = (H.GetS(b, "min_trust_to_post") ?? "new").Trim().ToLowerInvariant();
            // Rejected rather than silently defaulted: an operator who typed 'trusteed' must not end
            // up with a category open to everybody while believing it is restricted.
            if (!Enum.TryParse<ForumTrust.Level>(minTrust, true, out _))
                return Results.Json(new { error = "unknown_trust_level" }, statusCode: 400);

            try
            {
                var id = db.ExecuteReturningId(
                    @"INSERT INTO pciworld_forum_categories(slug,title,description,min_trust_to_post,sort)
                      VALUES(?,?,?,?,?)",
                    slug, (H.GetS(b, "title") ?? slug).Trim(), H.GetS(b, "description"), minTrust,
                    int.TryParse(H.GetS(b, "sort"), out var sortOrder) ? sortOrder : 100);
                Audit(adm!.Id, "world_forum_category_created", slug);
                return Results.Json(new { ok = true, id, slug });
            }
            catch { return Results.Json(new { error = "slug_taken" }, statusCode: 409); }
        });
    }
}
