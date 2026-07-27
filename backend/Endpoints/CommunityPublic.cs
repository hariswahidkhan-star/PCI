using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI World community — the public participant API (spec §14.1; CCP_PHASE1_DESIGN §5).
///
///   GET  /api/world/community/rooms                      room catalogue
///   GET  /api/world/community/rooms/{slug}               one room + presence + rules
///   POST /api/world/community/guest-sessions             validated guest entry
///   DELETE /api/world/community/guest-sessions/current   leave, releasing the display name
///   GET  /api/world/community/rooms/{slug}/messages      ordered replay from a sequence
///   POST /api/world/community/rooms/{slug}/messages      send (moderated before publication)
///   POST /api/world/community/reports                    report a message or participant
///
/// Every path already falls inside WorldOnly.Allowed() (it is all under /api/world), so this adds
/// no deployment-boundary surface.
///
/// TWO THINGS THIS LAYER IS RESPONSIBLE FOR, and which the service layer deliberately is not:
///
///   1. Never returning content the caller is not entitled to see. The service returns a verdict;
///      this decides what the AUTHOR is told versus what a room replay contains. A blocked message
///      gets its reason code back to its own author and appears to nobody else.
///
///   2. Deriving the risk key. Raw IPs never enter the database (§19.2) — the request's trusted hop
///      is hashed with a rotating pepper here, at the edge, so nothing downstream can leak one.
/// </summary>
public static class CommunityPublic
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult Disabled() => Results.Json(
            new { error = "community_disabled", message = "PCI World community rooms are not currently open." },
            statusCode: 404);

        bool Enabled() => Settings.Bool(db, "world_enabled", true) && CommunityRooms.Enabled(db);

        // Per-key throttle, same shape as the rest of the World surface: far above human pace, low
        // enough to blunt a script.
        var rl = new System.Collections.Concurrent.ConcurrentDictionary<string, (int count, long start)>();
        bool Throttled(string key, int limit, long windowMs = 60_000)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (rl.Count > 20_000)
                foreach (var kv in rl) if (now - kv.Value.start >= windowMs) rl.TryRemove(kv.Key, out _);
            var e = rl.AddOrUpdate(key, (1, now), (_, c) => now - c.start >= windowMs ? (1, now) : (c.count + 1, c.start));
            return e.count > limit;
        }

        /// The rotating, peppered abuse identifier. Derived per request and never stored raw: a
        /// deleted cookie or a new proxy defeats it, which is stated honestly rather than described
        /// as ban enforcement (§7.1). The rotation period bounds how long one identifier survives.
        string RiskKey(HttpContext ctx)
        {
            var pepper = Environment.GetEnvironmentVariable("COMMUNITY_RISK_PEPPER")
                         ?? Environment.GetEnvironmentVariable("FORUM_SALT") ?? "pciworld-community";
            var period = DateTime.UtcNow.ToString("yyyy-MM");
            return Security.Sha($"{pepper}|{Security.ClientIp(ctx)}|{period}")[..32];
        }

        string? GuestToken(HttpContext ctx)
        {
            var h = ctx.Request.Headers["X-Community-Session"].ToString();
            if (!string.IsNullOrWhiteSpace(h)) return h;
            return ctx.Request.Cookies["pciworld_room"];
        }

        // The active policy. Falls back to the shipped default matrix when an operator has not
        // staged one, so a room is never running with NO policy — which would quarantine everything.
        IReadOnlyList<CommunityModeration.Rule> Policy()
        {
            var rows = db.Query(
                @"SELECT r.* FROM pciworld_policy_rules r
                  JOIN pciworld_policy_versions v ON v.id = r.policy_version_id
                  WHERE v.status='active' ORDER BY r.sort");
            if (rows.Count == 0) return CommunityModeration.DefaultMatrix();
            var parsed = new List<CommunityModeration.Rule>(rows.Count);
            foreach (var r in rows)
                parsed.Add(new CommunityModeration.Rule(
                    H.L(r["id"]), H.Str(r["content_type"]) ?? "text", H.Str(r["category"]) ?? "",
                    H.Str(r["severity"]) ?? "medium",
                    Enum.TryParse<CommunityModeration.Band>(H.Str(r["confidence_band"]), true, out var b) ? b : CommunityModeration.Band.Low,
                    H.Str(r["context_rule"]), (int)H.L(r["repetition_min"]),
                    Enum.TryParse<CommunityModeration.Outcome>(H.Str(r["outcome"]), true, out var o) ? o : CommunityModeration.Outcome.Quarantine,
                    H.Str(r["reason_code"]) ?? "policy", (int)H.L(r["sort"])));
            return parsed;
        }

        // The configured moderator. Absent configuration yields NullModerator, which classifies
        // nothing and therefore publishes nothing — the fail-closed default (§15).
        CommunityModeration.ITextModerator Moderator() =>
            Settings.Str(db, "world_community_moderator", "none") switch
            {
                "deterministic" => new CommunityModerators.DeterministicModerator(),
                _ => new CommunityModerators.NullModerator(),
            };

        // ── Catalogue ─────────────────────────────────────────────────────────────────────────

        app.MapGet("/api/world/community/rooms", () =>
        {
            if (!Enabled()) return Disabled();
            var rooms = CommunityRooms.Discoverable(db).Select(r => new
            {
                slug = H.Str(r["slug"]),
                title = H.Str(r["title"]),
                description = H.Str(r["description"]),
                topic = H.Str(r["topic"]),
                category = H.Str(r["category"]),
                locale = H.Str(r["locale"]),
                room_type = H.Str(r["room_type"]),
                state = H.Str(r["state"]),
                capacity = H.L(r["capacity"]),
                guests_welcome = H.B(r["guest_allowed"]),
                slow_mode_seconds = H.L(r["slow_mode_seconds"]),
                participants = CommunityRooms.Presence(db, H.L(r["id"])),
            });
            return Results.Json(new { rooms });
        });

        app.MapGet("/api/world/community/rooms/{slug}", (string slug) =>
        {
            if (!Enabled()) return Disabled();
            var room = CommunityRooms.BySlug(db, slug);
            if (room is null || H.Str(room["state"]) is "draft" or "archived") return Results.NotFound();
            return Results.Json(new
            {
                slug = H.Str(room["slug"]),
                title = H.Str(room["title"]),
                description = H.Str(room["description"]),
                topic = H.Str(room["topic"]),
                locale = H.Str(room["locale"]),
                room_type = H.Str(room["room_type"]),
                state = H.Str(room["state"]),
                accepting = CommunityRooms.AcceptsMessages(room),
                guests_welcome = H.B(room["guest_allowed"]),
                images_allowed = false,   // Phase 2 is not built; never advertise it as available
                slow_mode_seconds = H.L(room["slow_mode_seconds"]),
                rules_version = H.Str(room["rules_version"]),
                pinned_welcome = H.Str(room["pinned_welcome"]),
                retention_class = H.Str(room["retention_class"]),
                participants = CommunityRooms.Presence(db, H.L(room["id"])),
                capacity = H.L(room["capacity"]),
            });
        });

        // ── Guest entry ───────────────────────────────────────────────────────────────────────

        app.MapPost("/api/world/community/guest-sessions", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("join|" + Security.ClientIp(ctx), 10))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);

            var b = await H.Body(ctx.Request);
            var slug = (H.GetS(b, "room") ?? "").Trim();
            var name = H.GetS(b, "display_name");
            var acceptedVersion = (H.GetS(b, "rules_version") ?? "").Trim();

            var room = CommunityRooms.BySlug(db, slug);
            if (room is null) return Results.NotFound();
            if (!H.B(room["guest_allowed"]))
                return Results.Json(new { error = "guests_not_allowed" }, statusCode: 403);
            if (H.Str(room["state"]) is not ("open" or "slow_mode" or "read_only"))
                return Results.Json(new { error = "room_not_open" }, statusCode: 409);

            // Accepting the CURRENT rules is a precondition, not a formality: a stale acceptance
            // means the participant agreed to different terms than the ones now in force.
            var current = H.Str(room["rules_version"]) ?? "v1";
            if (acceptedVersion != current)
                return Results.Json(new { error = "rules_version_stale", rules_version = current }, statusCode: 409);

            if (CommunityRooms.Presence(db, H.L(room["id"])) >= H.L(room["capacity"]))
                return Results.Json(new { error = "room_full" }, statusCode: 409);

            var r = CommunityRooms.Join(db, H.L(room["id"]), name, current, RiskKey(ctx),
                                        H.Str(room["locale"]) ?? "en");
            if (!r.Ok)
                return Results.Json(new { error = r.ErrorCode, suggestion = r.Suggestion }, statusCode: 400);

            // HttpOnly so page script cannot read it, SameSite=Strict so it is not sent
            // cross-site — the same posture as the World account cookie.
            ctx.Response.Cookies.Append("pciworld_room", r.Token!, new CookieOptions
            {
                HttpOnly = true,
                Secure = ctx.Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromHours(12),
                Path = "/",
            });
            return Results.Json(new { ok = true, token = r.Token, display_name = CommunityNames.Display(name) });
        });

        app.MapDelete("/api/world/community/guest-sessions/current", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var s = CommunityRooms.SessionByToken(db, GuestToken(ctx));
            if (s is not null) CommunityRooms.EndSession(db, H.L(s["id"]), "left", "user_left");
            ctx.Response.Cookies.Delete("pciworld_room");
            return Results.Json(new { ok = true });
        });

        // ── Messages ──────────────────────────────────────────────────────────────────────────

        app.MapGet("/api/world/community/rooms/{slug}/messages", (HttpContext ctx, string slug) =>
        {
            if (!Enabled()) return Disabled();
            var room = CommunityRooms.BySlug(db, slug);
            if (room is null) return Results.NotFound();

            var after = long.TryParse(ctx.Request.Query["afterSequence"], out var a) ? a : 0;
            var msgs = CommunityRooms.Since(db, H.L(room["id"]), after).Select(m => new
            {
                sequence = H.L(m["sequence"]),
                body = H.Str(m["body"]),
                author = H.Str(m["author_name"]),
                reply_to = m["reply_to_message_id"] is null ? (long?)null : H.L(m["reply_to_message_id"]),
                at = H.Str(m["published_at"]),
            });
            return Results.Json(new { messages = msgs });
        });

        app.MapPost("/api/world/community/rooms/{slug}/messages", async (HttpContext ctx, string slug) =>
        {
            if (!Enabled()) return Disabled();
            var room = CommunityRooms.BySlug(db, slug);
            if (room is null) return Results.NotFound();

            var session = CommunityRooms.SessionByToken(db, GuestToken(ctx));
            if (session is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (H.L(session["room_id"]) != H.L(room["id"]))
                return Results.Json(new { error = "wrong_room" }, statusCode: 403);

            // Flood control is per session, not per IP: a shared office NAT must not throttle
            // everyone because one participant is noisy.
            if (Throttled("msg|" + H.L(session["id"]), 30))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);

            var b = await H.Body(ctx.Request);
            var body = H.GetS(b, "body") ?? "";
            var clientId = (H.GetS(b, "client_message_id") ?? "").Trim();
            if (clientId.Length is 0 or > 64)
                return Results.Json(new { error = "bad_client_message_id" }, statusCode: 400);

            // Slow mode is enforced server-side against the stored last-message time; a client that
            // ignores the countdown gains nothing.
            var slow = H.L(room["slow_mode_seconds"]);
            if (slow > 0 && H.Str(session["last_message_at"]) is { Length: > 0 } last
                && H.JsMillis(last) + slow * 1000 > H.NowMillis)
                return Results.Json(new { error = "slow_mode", retry_after_seconds = slow }, statusCode: 429);

            // Repetition drives deterministic escalation in the policy matrix (§8.4.1) — a repeat
            // offender escalates without the classifier needing to become more certain.
            var repetition = (int)db.Scalar<long>(
                @"SELECT COUNT(*) FROM pciworld_moderation_decisions d
                  JOIN pciworld_community_messages m ON m.decision_id = d.id
                  WHERE m.guest_session_id=? AND d.outcome IN ('block','eject','escalate')",
                H.L(session["id"]));

            var result = CommunityRooms.Accept(db, room, H.L(session["id"]), null, clientId, body,
                                               Moderator(), Policy(), repetition: repetition,
                                               correlationId: ctx.TraceIdentifier);

            if (result.Ejected)
            {
                CommunityRooms.EndSession(db, H.L(session["id"]), "ejected", result.ReasonCode);
                ctx.Response.Cookies.Delete("pciworld_room");
                log(null, "world_community_eject", result.ReasonCode);
            }

            // The author learns their own message's fate — and only their own. A blocked or
            // quarantined message is never echoed into anyone else's replay.
            return Results.Json(new
            {
                published = result.Published,
                sequence = result.Sequence,
                status = result.Status,
                reason = result.ReasonCode,
                ejected = result.Ejected,
            }, statusCode: result.ReasonCode is "room_not_accepting" or "message_too_long" or "empty_message" ? 400 : 200);
        });

        // ── Reports ───────────────────────────────────────────────────────────────────────────

        app.MapPost("/api/world/community/reports", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var session = CommunityRooms.SessionByToken(db, GuestToken(ctx));
            if (session is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (Throttled("report|" + H.L(session["id"]), 10))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);

            var b = await H.Body(ctx.Request);
            var sequence = H.GetNum(b, "sequence");
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (reason.Length is 0 or > 48) return Results.Json(new { error = "bad_reason" }, statusCode: 400);

            var roomId = H.L(session["room_id"]);
            long? messageId = null;
            if (sequence is not null)
            {
                var m = db.QueryOne(
                    "SELECT id FROM pciworld_community_messages WHERE room_id=? AND sequence=? AND status='allowed'",
                    roomId, (long)sequence);
                if (m is null) return Results.NotFound();
                messageId = H.L(m["id"]);
            }

            try
            {
                db.Execute(
                    @"INSERT INTO pciworld_community_reports(room_id,message_id,reporter_session_id,reason_code,note)
                      VALUES(?,?,?,?,?)",
                    roomId, messageId, H.L(session["id"]), reason, H.GetS(b, "note"));
            }
            catch
            {
                // The unique index makes a repeat report a no-op rather than an error: one reporter
                // must not be able to inflate a count, and telling them "already reported" leaks
                // nothing useful either way.
            }

            // Deliberately no status in the response. Whether a report led to action is not the
            // reporter's to know — that would let a coordinated group probe moderation state.
            return Results.Json(new { ok = true });
        });
    }
}
