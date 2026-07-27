using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI World community — the moderation console API (spec §12.2/§14; CCP_PHASE1_DESIGN).
///
/// Admin routes live under <c>/api/world-admin/community/*</c>; the two guest-facing appeal routes
/// live under <c>/api/world/community/appeals*</c>. Both prefixes already sit inside
/// WorldOnly.Allowed(), so the deployment boundary is unchanged.
///
/// AUTHORIZATION IS PER ACTION, NOT PER CONSOLE. Every route names the specific permission it
/// needs — community.read, community.moderate, community.sanction, community.sanction.approve,
/// community.appeal, community.restricted, community.rooms — so the separation of duties encoded in
/// WorldRbac is enforced by the endpoint rather than implied by which buttons a UI renders. Hiding
/// a control is not authorization.
///
/// THE GUEST APPEAL ROUTES ARE DELIBERATELY UNAUTHENTICATED. A guest who was ejected has no
/// account, and requiring one to appeal would make the right to appeal conditional on joining the
/// thing you were just removed from. They are protected instead by a high-entropy credential that
/// is checked by hash, expires and is attempt-limited (§8.6).
/// </summary>
public static class CommunityAdmin
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        void Audit(long? adminId, string action, string? detail)
        {
            try { db.Execute("INSERT INTO pciworld_audit(admin_id,action,detail) VALUES(?,?,?)", adminId, action, detail); }
            catch { /* auditing must never break the action it records */ }
        }

        /// Resolve a world-admin and check ONE named permission. Returns the failing result, or null
        /// with the context populated.
        IResult? Gate(HttpContext ctx, string action, out WorldAdmin.Ctx? adm)
        {
            adm = WorldAdmin.FromReq(ctx.Request, db);
            if (adm is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!WorldRbac.Allowed(adm.Role, action))
                return Results.Json(new { error = "forbidden", required = action }, statusCode: 403);
            return null;
        }

        // ── Queues ────────────────────────────────────────────────────────────────────────────

        app.MapGet("/api/world-admin/community/queue", (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.read", out _) is { } deny) return deny;
            var status = ctx.Request.Query["status"].ToString();
            var cases = CommunityCases.Queue(db, string.IsNullOrWhiteSpace(status) ? null : status);
            return Results.Json(new { cases = cases.Select(Summary) });
        });

        // Separate route AND separate permission. Restricted evidence must not be reachable by
        // adding a query parameter to the ordinary queue (§8.7).
        app.MapGet("/api/world-admin/community/queue/restricted", (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.restricted", out var adm) is { } deny) return deny;
            Audit(adm!.Id, "world_community_restricted_queue_viewed", null);
            return Results.Json(new { cases = CommunityCases.RestrictedQueue(db).Select(Summary) });
        });

        app.MapGet("/api/world-admin/community/cases/{id:long}", (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "community.read", out var adm) is { } deny) return deny;
            var c = db.QueryOne("SELECT * FROM pciworld_moderation_cases WHERE id=?", id);
            if (c is null) return Results.NotFound();

            // A restricted case needs the restricted permission even when reached by direct id, or
            // the separate queue would be a formality.
            if (H.B(c["restricted"]) && !WorldRbac.Allowed(adm!.Role, "community.restricted"))
                return Results.Json(new { error = "forbidden", required = "community.restricted" }, statusCode: 403);

            var events = db.Query(
                "SELECT event,detail,previous_state,new_state,actor_kind,created_at FROM pciworld_moderation_case_events WHERE case_id=? ORDER BY id", id);
            var sanctions = db.Query("SELECT * FROM pciworld_sanctions WHERE case_id=? ORDER BY id", id);

            return Results.Json(new
            {
                @case = Summary(c),
                events,
                sanctions = sanctions.Select(s => new
                {
                    id = H.L(s["id"]),
                    type = H.Str(s["sanction_type"]),
                    reason = H.Str(s["reason_code"]),
                    scope = H.Str(s["scope"]),
                    issued_by = H.L(s["issued_by"]),
                    approved_by = s["approved_by"] is null ? (long?)null : H.L(s["approved_by"]),
                    // The distinction that makes maker-checker visible in the console rather than
                    // buried: a pending permanent sanction is NOT in effect.
                    in_effect = CommunityCases.IsInEffect(s),
                    awaiting_approval = CommunityCases.RequiresApproval(H.Str(s["sanction_type"]) ?? "") && s["approved_by"] is null,
                    expires_at = H.Str(s["expires_at"]),
                    revoked_at = H.Str(s["revoked_at"]),
                }),
            });
        });

        app.MapPost("/api/world-admin/community/cases/{id:long}/assign", (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "community.moderate", out var adm) is { } deny) return deny;
            if (!CommunityCases.Assign(db, id, adm!.Id, ctx.TraceIdentifier))
                return Results.Json(new { error = "not_assignable" }, statusCode: 409);
            Audit(adm.Id, "world_community_case_assigned", id.ToString());
            return Results.Json(new { ok = true });
        });

        app.MapPost("/api/world-admin/community/cases/{id:long}/close", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "community.moderate", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var outcome = (H.GetS(b, "outcome") ?? "").Trim();
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (outcome is not ("actioned" or "dismissed" or "escalated"))
                return Results.Json(new { error = "bad_outcome" }, statusCode: 400);
            // A reason is mandatory: every action must carry one so the trail explains itself (§8.6).
            if (reason.Length is 0 or > 500) return Results.Json(new { error = "reason_required" }, statusCode: 400);

            if (!CommunityCases.Close(db, id, adm!.Id, outcome, reason, ctx.TraceIdentifier))
                return Results.Json(new { error = "not_open" }, statusCode: 409);
            Audit(adm.Id, "world_community_case_closed", $"{id}:{outcome}");
            return Results.Json(new { ok = true });
        });

        // ── Sanctions ─────────────────────────────────────────────────────────────────────────

        app.MapPost("/api/world-admin/community/sanctions", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.sanction", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);

            var type = (H.GetS(b, "type") ?? "").Trim();
            if (type is not ("warning" or "slow_mode" or "mute" or "eject" or "restrict" or "permanent"))
                return Results.Json(new { error = "bad_type" }, statusCode: 400);

            var result = CommunityCases.Issue(db,
                subjectKind: (H.GetS(b, "subject_kind") ?? "risk_key").Trim(),
                subjectRef: (H.GetS(b, "subject_ref") ?? "").Trim(),
                roomId: H.GetNum(b, "room_id") is { } r ? (long)r : null,
                sanctionType: type,
                reasonCode: (H.GetS(b, "reason") ?? "").Trim(),
                issuedBy: adm!.Id,
                caseId: H.GetNum(b, "case_id") is { } c ? (long)c : null,
                durationMinutes: H.GetNum(b, "duration_minutes") is { } d ? (int)d : null,
                scope: (H.GetS(b, "scope") ?? "room").Trim(),
                correlationId: ctx.TraceIdentifier);

            if (!result.Ok) return Results.Json(new { error = result.ErrorCode }, statusCode: 400);
            Audit(adm.Id, "world_community_sanction_issued", $"{result.SanctionId}:{type}");
            return Results.Json(new
            {
                ok = true,
                id = result.SanctionId,
                // Told plainly, so an operator is never left believing a permanent sanction is live
                // when it is waiting for a second signature.
                awaiting_approval = CommunityCases.RequiresApproval(type),
            });
        });

        app.MapPost("/api/world-admin/community/sanctions/{id:long}/approve", (HttpContext ctx, long id) =>
        {
            // A DIFFERENT permission from issuing, held by different roles. The service also refuses
            // when approver == issuer, so neither layer alone is the whole control.
            if (Gate(ctx, "community.sanction.approve", out var adm) is { } deny) return deny;
            var result = CommunityCases.Approve(db, id, adm!.Id, ctx.TraceIdentifier);
            if (!result.Ok)
                return Results.Json(new { error = result.ErrorCode },
                                    statusCode: result.ErrorCode == "maker_checker" ? 403 : 409);
            Audit(adm.Id, "world_community_sanction_approved", id.ToString());
            return Results.Json(new { ok = true });
        });

        app.MapPost("/api/world-admin/community/sanctions/{id:long}/revoke", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "community.sanction", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (reason.Length is 0 or > 500) return Results.Json(new { error = "reason_required" }, statusCode: 400);
            if (!CommunityCases.Revoke(db, id, adm!.Id, reason, ctx.TraceIdentifier))
                return Results.Json(new { error = "not_found_or_revoked" }, statusCode: 409);
            Audit(adm.Id, "world_community_sanction_revoked", id.ToString());
            return Results.Json(new { ok = true });
        });

        // ── Appeals (admin side) ──────────────────────────────────────────────────────────────

        app.MapGet("/api/world-admin/community/appeals", (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.appeal", out _) is { } deny) return deny;
            var rows = db.Query(
                @"SELECT a.id,a.public_reference,a.status,a.submission,a.created_at,a.reviewed_at,
                         c.subject_kind,c.subject_ref,c.severity,c.reason_code
                  FROM pciworld_appeals a JOIN pciworld_moderation_cases c ON c.id=a.case_id
                  WHERE a.status IN ('submitted','under_review')
                    AND c.restricted=0
                  ORDER BY a.created_at LIMIT 200");
            return Results.Json(new { appeals = rows });
        });

        app.MapPost("/api/world-admin/community/appeals/{id:long}/decide", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, "community.appeal", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var upheld = H.GetS(b, "decision") == "uphold";
            var outcome = (H.GetS(b, "outcome") ?? "").Trim();
            if (outcome.Length is 0 or > 2000) return Results.Json(new { error = "outcome_required" }, statusCode: 400);

            if (!CommunityCases.Decide(db, id, adm!.Id, upheld, outcome, ctx.TraceIdentifier))
                return Results.Json(new { error = "not_decidable" }, statusCode: 409);
            Audit(adm.Id, upheld ? "world_community_appeal_upheld" : "world_community_appeal_overturned", id.ToString());
            return Results.Json(new { ok = true });
        });

        // ── Rooms ─────────────────────────────────────────────────────────────────────────────

        app.MapPost("/api/world-admin/community/rooms", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.rooms", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var slug = (H.GetS(b, "slug") ?? "").Trim().ToLowerInvariant();
            var title = (H.GetS(b, "title") ?? "").Trim();
            if (!System.Text.RegularExpressions.Regex.IsMatch(slug, "^[a-z0-9-]{3,60}$"))
                return Results.Json(new { error = "bad_slug" }, statusCode: 400);
            if (title.Length is < 3 or > 120) return Results.Json(new { error = "bad_title" }, statusCode: 400);

            try
            {
                var id = db.ExecuteReturningId(
                    @"INSERT INTO pciworld_community_rooms(slug,title,description,topic,locale,room_type,state,capacity,rules_version)
                      VALUES(?,?,?,?,?,?,'draft',?,?)",
                    slug, title, H.GetS(b, "description"), H.GetS(b, "topic"),
                    (H.GetS(b, "locale") ?? "en").Trim(), (H.GetS(b, "room_type") ?? "community").Trim(),
                    H.GetNum(b, "capacity") is { } cap ? (long)cap : 200L,
                    (H.GetS(b, "rules_version") ?? "v1").Trim());
                Audit(adm!.Id, "world_community_room_created", slug);
                // Created as DRAFT, never open. A room becomes reachable only by an explicit state
                // change, so a mistyped create cannot put an unmoderated room live.
                return Results.Json(new { ok = true, id, state = "draft" });
            }
            catch { return Results.Json(new { error = "slug_taken" }, statusCode: 409); }
        });

        app.MapPost("/api/world-admin/community/rooms/{slug}/state", async (HttpContext ctx, string slug) =>
        {
            if (Gate(ctx, "community.rooms", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var state = (H.GetS(b, "state") ?? "").Trim();
            if (state is not ("draft" or "scheduled" or "open" or "slow_mode" or "read_only" or "closed" or "archived"))
                return Results.Json(new { error = "bad_state" }, statusCode: 400);

            var changed = db.Execute(
                "UPDATE pciworld_community_rooms SET state=?, version=version+1, updated_at=datetime('now') WHERE slug=?",
                state, slug);
            if (changed == 0) return Results.NotFound();
            Audit(adm!.Id, "world_community_room_state", $"{slug}:{state}");
            return Results.Json(new { ok = true, state });
        });

        // The kill switch. Separate from `state` on purpose: an incident must not overwrite the
        // room's real schedule, and clearing the emergency must give back the room as it was.
        app.MapPost("/api/world-admin/community/rooms/{slug}/emergency", async (HttpContext ctx, string slug) =>
        {
            if (Gate(ctx, "community.rooms", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var mode = (H.GetS(b, "mode") ?? "").Trim();
            if (mode is not ("" or "degraded" or "locked" or "globally_disabled"))
                return Results.Json(new { error = "bad_mode" }, statusCode: 400);

            var changed = db.Execute(
                "UPDATE pciworld_community_rooms SET emergency_state=?, version=version+1, updated_at=datetime('now') WHERE slug=?",
                mode.Length == 0 ? null : mode, slug);
            if (changed == 0) return Results.NotFound();
            Audit(adm!.Id, "world_community_room_emergency", $"{slug}:{(mode.Length == 0 ? "cleared" : mode)}");
            return Results.Json(new { ok = true, emergency_state = mode.Length == 0 ? null : mode });
        });

        // ── Settings ──────────────────────────────────────────────────────────────────────────

        app.MapGet("/api/world-admin/community/settings", (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.read", out _) is { } deny) return deny;
            return Results.Json(new
            {
                enabled = CommunityRooms.Enabled(db),
                moderator = Settings.Str(db, "world_community_moderator", "none"),
                // Stated rather than implied: with no provider configured the rooms accept messages
                // and publish none of them, which is the fail-closed posture and looks like a fault
                // to an operator who has not been told.
                publishes_messages = Settings.Str(db, "world_community_moderator", "none") != "none",
            });
        });

        app.MapPatch("/api/world-admin/community/settings", async (HttpContext ctx) =>
        {
            if (Gate(ctx, "community.rooms", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);

            // An allowlist, not a passthrough. A settings endpoint that writes whatever key it is
            // given is a privilege-escalation primitive: world_community_* is this role's business,
            // and everything else on the settings table is not.
            var changed = new List<string>();
            if (H.GetS(b, "enabled") is { } en && en is "0" or "1")
            {
                Settings.Put(db, "world_community_enabled", en);
                changed.Add($"enabled={en}");
            }
            if (H.GetS(b, "moderator") is { } mod)
            {
                // Only providers that actually exist. A typo must not silently leave the rooms on
                // the null moderator while an operator believes moderation is configured.
                if (mod is not ("none" or "deterministic"))
                    return Results.Json(new { error = "unknown_moderator" }, statusCode: 400);
                Settings.Put(db, "world_community_moderator", mod);
                changed.Add($"moderator={mod}");
            }
            if (changed.Count == 0) return Results.Json(new { error = "nothing_to_change" }, statusCode: 400);

            Audit(adm!.Id, "world_community_settings", string.Join(",", changed));
            return Results.Json(new
            {
                ok = true,
                enabled = CommunityRooms.Enabled(db),
                moderator = Settings.Str(db, "world_community_moderator", "none"),
            });
        });

        // ── Appeals (guest side, no account) ──────────────────────────────────────────────────

        app.MapPost("/api/world/community/appeals", async (HttpContext ctx) =>
        {
            if (!CommunityRooms.Enabled(db)) return Results.NotFound();
            var b = await H.Body(ctx.Request);
            var appeal = CommunityCases.ByCredential(db, H.GetS(b, "reference"), H.GetS(b, "credential"));
            // A wrong credential, an expired one and an exhausted one are indistinguishable here —
            // deliberately, so the endpoint is not an oracle for guessing.
            if (appeal is null) return Results.Json(new { error = "invalid_credential" }, statusCode: 404);

            var submission = (H.GetS(b, "submission") ?? "").Trim();
            if (!CommunityCases.SubmitAppeal(db, H.L(appeal["id"]), submission))
                return Results.Json(new { error = "not_submittable" }, statusCode: 409);
            return Results.Json(new { ok = true, reference = H.Str(appeal["public_reference"]) });
        });

        app.MapPost("/api/world/community/appeals/status", async (HttpContext ctx) =>
        {
            if (!CommunityRooms.Enabled(db)) return Results.NotFound();
            var b = await H.Body(ctx.Request);
            var appeal = CommunityCases.ByCredential(db, H.GetS(b, "reference"), H.GetS(b, "credential"));
            if (appeal is null) return Results.Json(new { error = "invalid_credential" }, statusCode: 404);
            // The minimal view: status and outcome only. No evidence, no other participants, no
            // classifier internals, no network data (§8.6).
            return Results.Json(CommunityCases.PublicView(appeal));
        });

        // A case summary carries no message bodies and no risk keys — the queue is for triage, and
        // evidence lives behind its own permission and its own access log (§8.7).
        static object Summary(Dictionary<string, object?> c) => new
        {
            id = H.L(c["id"]),
            room_id = c["room_id"] is null ? (long?)null : H.L(c["room_id"]),
            subject_kind = H.Str(c["subject_kind"]),
            severity = H.Str(c["severity"]),
            status = H.Str(c["status"]),
            restricted = H.B(c["restricted"]),
            reason_code = H.Str(c["reason_code"]),
            assigned_to = c["assigned_to"] is null ? (long?)null : H.L(c["assigned_to"]),
            report_count = c.ContainsKey("report_count") ? H.L(c["report_count"]) : 0,
            created_at = H.Str(c["created_at"]),
        };
    }
}
