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

        // Decode an image data URI under the COMMUNITY rules rather than Storage's.
        // Storage.DecodeDataUri exists for evidence and attachments: it allows PDF, uses a different
        // size cap, and is not the policy for an anonymous guest room. Reusing it would have quietly
        // made "no PDFs in rooms" depend on a constant that belongs to another feature.
        (byte[]? bytes, string mime, string? error) DecodeImageDataUri(string? dataUri)
        {
            if (string.IsNullOrEmpty(dataUri)) return (null, "", "no_file");
            if (!dataUri.StartsWith("data:", StringComparison.Ordinal)) return (null, "", "not_a_data_uri");
            var comma = dataUri.IndexOf(',');
            var semi = dataUri.IndexOf(';');
            if (comma < 0 || semi < 0 || semi > comma) return (null, "", "malformed_data_uri");
            var mime = dataUri[5..semi];
            // Size is bounded BEFORE allocating: base64 is about 4/3 of the bytes it encodes, so a
            // payload that could not possibly fit is refused without materialising it.
            if ((long)(dataUri.Length - comma) * 3 / 4 > CommunityMedia.MaxBytes + 1024)
                return (null, mime, "file_too_large");
            byte[] decoded;
            try { decoded = Convert.FromBase64String(dataUri[(comma + 1)..]); }
            catch { return (null, mime, "invalid_base64"); }
            // Everything else — allow-list, sniff, bounds, re-encode — is CommunityMedia.Sanitise's
            // job, called inside the pipeline. This function only gets the bytes out of the string.
            return (decoded, mime, null);
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
            CommunityBootstrap.EnsureCatalogueNotEmpty(db);
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
            // Eligibility metadata travels with the catalogue so the join form can offer a real
            // country list instead of a blank two-letter box that always fails on an empty allowlist.
            var jurisdictions = CommunityEligibility.Jurisdictions(db).OrderBy(c => c).ToArray();
            var mod = Settings.Str(db, "world_community_moderator", "none") ?? "none";
            return Results.Json(new
            {
                rooms,
                served_jurisdictions = jurisdictions,
                minimum_age = CommunityEligibility.MinAge(db),
                publishes_messages = mod is not ("none" or ""),
            });
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
                // Advertise images only when the global flag AND this room both allow them.
                images_allowed = CommunityMediaPipeline.ImagesEnabled(db) && H.B(room["image_allowed"]),
                slow_mode_seconds = H.L(room["slow_mode_seconds"]),
                rules_version = H.Str(room["rules_version"]),
                pinned_welcome = H.Str(room["pinned_welcome"]),
                retention_class = H.Str(room["retention_class"]),
                participants = CommunityRooms.Presence(db, H.L(room["id"])),
                capacity = H.L(room["capacity"]),
                served_jurisdictions = CommunityEligibility.Jurisdictions(db).OrderBy(c => c).ToArray(),
                minimum_age = CommunityEligibility.MinAge(db),
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

            // ── Eligibility (CCP-P1-003) ──
            //
            // Evaluated BEFORE a session exists, so an ineligible person never gets a token and
            // never occupies a display name. The gate is a declaration gate, not verification —
            // said plainly in the response as well as in the code, so nobody downstream mistakes a
            // recorded answer for a checked one.
            var eligibility = CommunityEligibility.Evaluate(
                db, H.GetS(b, "birth_date"), H.GetS(b, "jurisdiction"), DateTime.UtcNow);
            if (!eligibility.Allowed)
            {
                // Logged with no session id: the attempt is evidence that the gate was applied, and
                // there is no session to attach it to precisely because it was refused.
                CommunityEligibility.RecordDeclaration(db, 0, eligibility,
                    Settings.Str(db, CommunityEligibility.PolicyVersionKey, "v1") ?? "v1");
                return Results.Json(new
                {
                    error = eligibility.ReasonCode,
                    minimum_age = eligibility.MinAge,
                    // An operator needs to tell "nobody has configured this" apart from "you are in
                    // the wrong country"; a participant does not, and the reason code above does not
                    // distinguish them.
                    configured = CommunityEligibility.Configured(db),
                }, statusCode: 403);
            }

            var r = CommunityRooms.Join(db, H.L(room["id"]), name, current, RiskKey(ctx),
                                        H.Str(room["locale"]) ?? "en");
            if (!r.Ok)
                return Results.Json(new { error = r.ErrorCode, suggestion = r.Suggestion }, statusCode: 400);

            CommunityEligibility.RecordDeclaration(db, r.SessionId, eligibility,
                Settings.Str(db, CommunityEligibility.PolicyVersionKey, "v1") ?? "v1");

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
                // An image message carries its alternative text in `body` — the same field a text
                // message uses — so a client that has never heard of images still renders something
                // meaningful rather than an empty bubble, and a screen reader gets the description
                // either way. `media_id` is additive.
                kind = H.Str(m["message_kind"]) ?? "text",
                media_id = m.TryGetValue("media_id", out var mid) && mid is not null ? H.L(mid) : (long?)null,
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

            // An ejection is not just a disconnect. §8.6 and PW-US-053 require that the person is
            // told what happened and given a way to challenge it — and a guest has no account to
            // come back to, so the appeal credential is minted HERE and returned exactly once. If
            // it is not handed over in this response it is unrecoverable, which would make the
            // right of appeal theoretical.
            CommunityCases.AppealTicket? ticket = null;
            string? restrictedUntil = null;
            if (result.Ejected)
            {
                var riskKey = RiskKey(ctx);
                var severe = result.ReasonCode is "grooming_severe" or "grooming_restricted"
                                              or "sexual_severe" or "hate_severe" or "violence_credible_threat";
                db.Transaction(() =>
                {
                    // A severe finding opens a RESTRICTED case, which keeps it out of the ordinary
                    // moderator queue entirely (§8.7) rather than merely flagging it.
                    var caseId = CommunityCases.OpenOrGet(db, H.L(room["id"]), "guest_session",
                        H.L(session["id"]).ToString(), severe ? "critical" : "high",
                        result.ReasonCode, restricted: severe, correlationId: ctx.TraceIdentifier);

                    CommunityCases.Issue(db, "risk_key", riskKey, H.L(room["id"]), "eject",
                        result.ReasonCode, issuedBy: 0, caseId: caseId, durationMinutes: 24 * 60,
                        correlationId: ctx.TraceIdentifier);

                    // The time-bounded consequence. Evadable by clearing a cookie or changing
                    // network, which is stated honestly as a deterrent rather than enforcement.
                    restrictedUntil = H.StampInMinutes(24 * 60);
                    db.Execute(
                        @"INSERT INTO pciworld_risk_restrictions(risk_key,scope,room_id,reason_code,case_id,expires_at)
                          VALUES(?,'room',?,?,?,?)",
                        riskKey, H.L(room["id"]), result.ReasonCode, caseId, restrictedUntil);

                    ticket = CommunityCases.IssueAppeal(db, caseId);
                    CommunityRooms.EndSession(db, H.L(session["id"]), "ejected", result.ReasonCode);
                });
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
                // Shown once. The reference is safe to quote in support mail; the credential is the
                // only thing that unlocks the appeal, and it is never retrievable again.
                appeal = ticket is null ? null : new
                {
                    reference = ticket.Value.PublicReference,
                    credential = ticket.Value.Credential,
                    restricted_until = restrictedUntil,
                    how = "/world/appeals",
                },
            }, statusCode: result.ReasonCode is "room_not_accepting" or "message_too_long" or "empty_message" ? 400 : 200);
        });

        // ── Images (Phase 2) ──────────────────────────────────────────────────────────────────
        //
        // The upload endpoint returns a PENDING handle and never a URL. That is the whole contract:
        // there is nothing in this response another participant could load, because at this point
        // no renderable copy exists anywhere (see CommunityMediaPipeline).

        app.MapPost("/api/world/community/rooms/{slug}/images", async (HttpContext ctx, string slug) =>
        {
            if (!Enabled()) return Disabled();
            if (!CommunityMediaPipeline.ImagesEnabled(db))
                return Results.Json(new { error = "images_disabled" }, statusCode: 503);

            var room = CommunityRooms.BySlug(db, slug);
            if (room is null) return Results.NotFound();

            var session = CommunityRooms.SessionByToken(db, GuestToken(ctx));
            if (session is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (H.L(session["room_id"]) != H.L(room["id"]))
                return Results.Json(new { error = "wrong_room" }, statusCode: 403);

            // Deliberately tighter than the text limit. An upload costs a sanitise, a store and a
            // classifier call; text costs none of those.
            if (Throttled("img|" + H.L(session["id"]), 6))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);

            var b = await H.Body(ctx.Request);
            var uploadId = (H.GetS(b, "client_upload_id") ?? "").Trim();
            if (uploadId.Length is 0 or > 64)
                return Results.Json(new { error = "bad_client_upload_id" }, statusCode: 400);

            var (bytes, mime, decodeError) = DecodeImageDataUri(H.GetS(b, "data"));
            if (decodeError is not null)
                return Results.Json(new { error = decodeError }, statusCode: 400);

            var r = CommunityMediaPipeline.Upload(db, room, H.L(session["id"]), null, uploadId,
                                                  bytes, mime, H.GetS(b, "alt"),
                                                  correlationId: ctx.TraceIdentifier);

            // A refusal is a 400 with a reason the participant can act on; a duplicate is a 200
            // reporting the ORIGINAL outcome, because a retry is not an error.
            var status = r.Ok || r.Reason == "duplicate" ? 200 : 400;
            return Results.Json(new
            {
                ok = r.Ok,
                media_id = r.MediaId == 0 ? (long?)null : r.MediaId,
                state = r.State,
                reason = r.Reason,
                // Said plainly, because "uploaded" reads as "posted" and it is not: nobody else can
                // see this yet, and they may never be able to.
                notice = r.Ok ? "Your image is being checked and is not visible to the room yet." : null,
            }, statusCode: status);
        });

        // Serving. Two conditions, checked in ONE place (CommunityMediaPipeline.IsServable) so a
        // handler cannot satisfy itself with only one of them: state must be 'allowed' AND a
        // derivative must exist. A pending, withheld or restricted item is a 404 — not a 403,
        // because "this exists but you can't have it" is itself information about someone's
        // moderation outcome.
        app.MapGet("/api/world/community/media/{id:long}", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Disabled();
            if (!CommunityMediaPipeline.ImagesEnabled(db))
                return Results.Json(new { error = "images_disabled" }, statusCode: 503);

            var media = db.QueryOne(
                "SELECT id,state,derivative_ref,declared_mime FROM pciworld_community_media WHERE id=?", id);
            if (!CommunityMediaPipeline.IsServable(media)) return Results.NotFound();

            var fetched = Storage.Get(H.Str(media!["derivative_ref"]));
            if (fetched?.bytes is null) return Results.NotFound();

            // no-store rather than a long cache: a published image can later be withdrawn by a
            // moderator or an appeal, and a year-long cache entry would outlive that decision.
            ctx.Response.Headers["Cache-Control"] = "no-store";
            return Results.File(fetched.Value.bytes, H.Str(media["declared_mime"]) ?? "image/png");
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
                // The report and the case it belongs to are written together: a report that never
                // reaches a queue is a complaint into a void, and a case created without its report
                // has no evidence attached. OpenOrGet keeps a brigading burst to ONE case to work.
                db.Transaction(() =>
                {
                    var reportedSession = messageId is null ? null : H.Str(db.QueryOne(
                        "SELECT guest_session_id FROM pciworld_community_messages WHERE id=?", messageId)?["guest_session_id"]);

                    var caseId = CommunityCases.OpenOrGet(db, roomId, "guest_session",
                        reportedSession ?? $"message:{messageId}", "normal", "participant_report",
                        correlationId: ctx.TraceIdentifier);

                    db.Execute(
                        @"INSERT INTO pciworld_community_reports(room_id,message_id,reported_session_id,reporter_session_id,reason_code,note,case_id)
                          VALUES(?,?,?,?,?,?,?)",
                        roomId, messageId,
                        reportedSession is null ? null : (object)long.Parse(reportedSession),
                        H.L(session["id"]), reason, H.GetS(b, "note"), caseId);
                });
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
