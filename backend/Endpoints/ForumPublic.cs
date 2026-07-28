using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// The forum's public surface (docs/pciworld/CCP_PHASE3_DESIGN.md §2, §6).
///
/// TWO KINDS OF ROUTE, DELIBERATELY DIFFERENT.
///
///   <c>/world/forum/…</c>      server-rendered HTML for readers and crawlers (§18.3, D-009)
///   <c>/api/world/forum/…</c>  JSON for the authenticated React composer
///
/// Reading needs no account: the forum is a public professional archive, and putting a sign-in wall
/// in front of it would defeat the point of making it crawlable at all. WRITING always needs a
/// Passport session, which is the difference between this and the Phase 1 guest rooms.
///
/// TRUST IS RESOLVED SERVER-SIDE, ALWAYS. A client never says what level it is. It is computed from
/// the standing row by <see cref="ForumTrust.Of"/>, so a forged or stale claim from the browser
/// cannot buy immediate publication.
/// </summary>
public static class ForumPublic
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        bool Enabled() => ForumPosts.Enabled(db);
        IResult Disabled() => Results.NotFound();

        // The active post policy, falling back to the shipped matrix so a forum is never running
        // with NO policy — which would quarantine everything and look like an outage.
        IReadOnlyList<CommunityModeration.Rule> Policy()
        {
            var rows = db.Query(
                @"SELECT r.* FROM pciworld_policy_rules r
                  JOIN pciworld_policy_versions v ON v.id = r.policy_version_id
                  WHERE v.status='active' AND r.content_type='post' ORDER BY r.sort");
            if (rows.Count == 0) return CommunityModeration.DefaultPostMatrix();
            var parsed = new List<CommunityModeration.Rule>(rows.Count);
            foreach (var r in rows)
                parsed.Add(new CommunityModeration.Rule(
                    H.L(r["id"]), "post", H.Str(r["category"]) ?? "", H.Str(r["severity"]) ?? "medium",
                    Enum.TryParse<CommunityModeration.Band>(H.Str(r["confidence_band"]), true, out var b) ? b : CommunityModeration.Band.Low,
                    H.Str(r["context_rule"]), (int)H.L(r["repetition_min"]),
                    Enum.TryParse<CommunityModeration.Outcome>(H.Str(r["outcome"]), true, out var o) ? o : CommunityModeration.Outcome.Quarantine,
                    H.Str(r["reason_code"]) ?? "policy", (int)H.L(r["sort"])));
            return parsed;
        }

        // Absent configuration yields NullModerator, which classifies nothing and therefore
        // publishes nothing — the fail-closed default the whole increment uses.
        CommunityModeration.ITextModerator Moderator() =>
            Settings.Str(db, "world_community_moderator", "none") switch
            {
                "deterministic" => new CommunityModerators.DeterministicModerator(),
                _ => new CommunityModerators.NullModerator(),
            };

        /// Computed from recorded facts, never taken from the client.
        ForumTrust.Level LevelOf(WorldAccount.UserCtx user)
        {
            var s = db.QueryOne("SELECT * FROM pciworld_forum_standing WHERE user_id=?", user.Id);
            if (s is null)
                // No standing row yet: a brand-new account, whose posts are held. Email
                // verification still counts, because it is a fact we already hold about them.
                return ForumTrust.Of(new ForumTrust.Facts(user.EmailVerified, 0, 0, 0, 0, false));

            var firstPost = H.Str(s["first_post_at"]);
            var days = string.IsNullOrEmpty(firstPost) ? 0
                : (int)((H.NowMillis - H.JsMillis(firstPost)) / 86_400_000L);

            return ForumTrust.Of(new ForumTrust.Facts(
                user.EmailVerified,
                (int)H.L(s["accepted_posts"]),
                (int)H.L(s["upheld_reports"]),
                days,
                (int)H.L(s["accurate_flags"]),
                H.B(s["staff_granted"])));
        }

        // ── Server-rendered reading (no account required) ──────────────────────────────────────

        app.MapGet("/world/forum/{category}/{thread}", (HttpContext ctx, string category, string thread) =>
        {
            var html = ForumRender.Thread(db, category, thread, WorldUrl.Base(ctx.Request));
            // ForumRender returns null for everything the public may not see, so this handler does
            // not need to know the rules — and cannot get them subtly different from the renderer.
            if (html is null) return Results.NotFound();
            ctx.Response.ContentType = "text/html; charset=utf-8";
            return Results.Content(html, "text/html; charset=utf-8");
        });

        // ── JSON for the composer ─────────────────────────────────────────────────────────────

        app.MapGet("/api/world/forum/categories", () =>
        {
            if (!Enabled()) return Disabled();
            var rows = db.Query(
                @"SELECT slug,title,description,min_trust_to_post,state,sort
                  FROM pciworld_forum_categories WHERE state IN ('open','read_only') ORDER BY sort, title");
            return Results.Json(new { categories = rows.Select(c => new
            {
                slug = H.Str(c["slug"]),
                title = H.Str(c["title"]),
                description = H.Str(c["description"]),
                // Told to the client so the composer can explain WHY it is disabled rather than
                // just refusing — "you need N accepted posts" is actionable; a greyed button is not.
                min_trust = H.Str(c["min_trust_to_post"]),
                read_only = H.Str(c["state"]) == "read_only",
            }) });
        });

        app.MapGet("/api/world/forum/me", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var user = WorldAccount.FromReq(ctx.Request, db);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var level = LevelOf(user);
            return Results.Json(new
            {
                level = ForumTrust.Name(level),
                // Stated plainly so a new member is not confused by their first post vanishing.
                posts_are_reviewed_first = ForumTrust.HoldsBeforePublication(level),
                may_post_links = ForumTrust.MayPostLinks(level),
                may_edit = ForumTrust.MayEditOwnPosts(level),
                may_flag = ForumTrust.MayFlag(level),
                hourly_post_budget = ForumTrust.HourlyPostBudget(level),
            });
        });

        app.MapPost("/api/world/forum/threads/{threadId:long}/posts", async (HttpContext ctx, long threadId) =>
        {
            if (!Enabled()) return Disabled();
            var user = WorldAccount.FromReq(ctx.Request, db);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);

            var level = LevelOf(user);
            // The budget is enforced against RECORDED posts rather than an in-memory counter, so a
            // restart does not hand everybody a fresh allowance.
            var lastHour = db.Scalar<long>(
                @"SELECT COUNT(*) FROM pciworld_forum_posts
                  WHERE author_user_id=? AND created_at > ?",
                user.Id, H.StampInMinutes(-60));
            if (lastHour >= ForumTrust.HourlyPostBudget(level))
                return Results.Json(new { error = "rate_limited", retry_after_minutes = 60 }, statusCode: 429);

            var b = await H.Body(ctx.Request);
            var r = ForumPosts.Create(db, threadId, user.Id, H.GetS(b, "body"), level,
                                      Moderator(), Policy(), correlationId: ctx.TraceIdentifier);
            if (!r.Ok) return Results.Json(new { error = r.Reason }, statusCode: 400);

            RecordActivity(user.Id, r.Visible);
            return Results.Json(new
            {
                ok = true,
                post_id = r.PostId,
                state = r.State,
                published = r.Visible,
                reason = r.Reason,
                // The author learns their own post's fate, and only their own.
                notice = r.Visible ? null
                       : r.State == "pending" ? "Your post is waiting for a moderator. New accounts are reviewed before publication."
                       : "Your post was not published.",
            });
        });

        app.MapPatch("/api/world/forum/posts/{postId:long}", async (HttpContext ctx, long postId) =>
        {
            if (!Enabled()) return Disabled();
            var user = WorldAccount.FromReq(ctx.Request, db);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);

            var b = await H.Body(ctx.Request);
            var r = ForumPosts.Edit(db, postId, user.Id, H.GetS(b, "body"), LevelOf(user),
                                    Moderator(), Policy(), H.GetS(b, "edit_reason"));
            return r.Ok
                ? Results.Json(new { ok = true, state = r.State, published = r.Visible, reason = r.Reason })
                : Results.Json(new { error = r.Reason }, statusCode: r.Reason == "not_your_post" ? 403 : 400);
        });

        app.MapDelete("/api/world/forum/posts/{postId:long}", (HttpContext ctx, long postId) =>
        {
            if (!Enabled()) return Disabled();
            var user = WorldAccount.FromReq(ctx.Request, db);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);

            var r = ForumPosts.Withdraw(db, postId, user.Id);
            if (!r.Ok) return Results.Json(new { error = r.Reason }, statusCode: r.Reason == "not_your_post" ? 403 : 400);
            log(null, "world_forum_post_withdrawn", postId.ToString());
            // Said out loud, because "deleted" invites the assumption that it is gone. It is hidden;
            // the moderation record and the revisions survive for the report and appeal windows.
            return Results.Json(new
            {
                ok = true,
                notice = "Your post is no longer visible. Its moderation record is kept while any report or appeal is open.",
            });
        });

        /// <summary>
        /// Keep the standing facts current. Called after a post is created, so the ladder reflects
        /// what somebody has actually done rather than being recomputed from a full history scan on
        /// every read.
        /// </summary>
        void RecordActivity(long userId, bool accepted)
        {
            db.Execute(
                @"INSERT OR IGNORE INTO pciworld_forum_standing(user_id,level,first_post_at)
                  VALUES(?,'new',datetime('now'))", userId);
            if (accepted)
                db.Execute(
                    @"UPDATE pciworld_forum_standing
                      SET accepted_posts = accepted_posts + 1, updated_at = datetime('now')
                      WHERE user_id=?", userId);
        }
    }
}
