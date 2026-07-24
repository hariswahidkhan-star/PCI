using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI World — public surface (docs/pciworld/ARCHITECTURE.md §3).
///
///   HTML: /world · /world/challenge/{code} · /world/archive · /world/about · /world/r/{token} · /world/i/{token}
///   API:  session · today · attempts start/save/submit/load · share · invite
///
/// Anonymous-first: an opaque session token (sha-stored) is the only identity. Attempts pin
/// (challenge_id, version) at start and replay ONLY from pciworld_challenge_versions. Public
/// result and invitation tokens are opaque, sha-stored and revocable; URLs never carry emails or
/// ids. This module never reads or writes exam, entitlement, credential, membership or `users`
/// tables — PCI World practice cannot touch formal certification records by construction.
/// </summary>
public static class World
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult Html(string html) => Results.Content(html, "text/html; charset=utf-8");
        bool Enabled() => Settings.Bool(db, "world_enabled", true);
        IResult Disabled() => Results.Json(new { error = "world_disabled", message = "PCI World is not currently open." }, statusCode: 403);

        // Per-key throttle (SimLab/Certuvo pattern): far above human pace, low enough to blunt scripts.
        var rl = new System.Collections.Concurrent.ConcurrentDictionary<string, (int count, long start)>();
        bool Throttled(string key, int limit, long windowMs = 600_000)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (rl.Count > 20_000)
                foreach (var kv in rl) if (now - kv.Value.start >= windowMs) rl.TryRemove(kv.Key, out _);
            var e = rl.AddOrUpdate(key, (1, now), (_, c) => now - c.start >= windowMs ? (1, now) : (c.count + 1, c.start));
            return e.count > limit;
        }
        string Ip(HttpContext ctx) => ctx.Connection.RemoteIpAddress?.ToString() ?? "?";

        Dictionary<string, object?>? Session(HttpContext ctx)
        {
            var tok = ctx.Request.Headers["X-World-Session"].ToString();
            if (string.IsNullOrWhiteSpace(tok)) return null;
            var s = db.QueryOne("SELECT * FROM pciworld_sessions WHERE token_sha=?", Security.Sha(tok));
            if (s is not null)
                db.Execute("UPDATE pciworld_sessions SET last_seen_at=datetime('now') WHERE id=?", s["id"]);
            return s;
        }

        void Track(string ev, long? challengeId = null, long? sessionId = null)
        {
            try { db.Execute("INSERT INTO pciworld_events(event,challenge_id,session_id) VALUES(?,?,?)", ev, challengeId, sessionId); }
            catch { }
        }

        // ───────────────────────────── public HTML ─────────────────────────────

        app.MapGet("/world", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var today = WorldLifecycle.Today(db, DateTime.UtcNow);
            var version = today is null ? null : WorldLifecycle.LiveVersion(db, H.L(today["id"]));
            Track("home_viewed");
            return Html(WorldPages.Home(db, today, version));
        });

        app.MapGet("/world/challenge/{code}", (HttpContext ctx, string code) =>
        {
            if (!Enabled()) return Disabled();
            var ch = db.QueryOne("SELECT * FROM pciworld_challenges WHERE code=? AND current_version>=1 AND retired=0", code);
            if (ch is null) return Results.NotFound();
            var invite = ctx.Request.Query["i"].ToString();
            var version = WorldLifecycle.LiveVersion(db, H.L(ch["id"]));
            if (invite.Length > 0)
            {
                // An invitation plays the exact version the inviter played.
                var inv = db.QueryOne(@"SELECT a.challenge_id, a.version FROM pciworld_invites i
                    JOIN pciworld_attempts a ON a.id=i.attempt_id
                    WHERE i.token_sha=? AND i.revoked=0", Security.Sha(invite));
                if (inv is not null && H.L(inv["challenge_id"]) == H.L(ch["id"]))
                    version = WorldLifecycle.PinnedVersion(db, H.L(ch["id"]), H.L(inv["version"]));
            }
            if (version is null) return Results.NotFound();
            Track("challenge_viewed", H.L(ch["id"]));
            var view = WorldContent.PublicView(H.Str(version["config_json"])!);
            return Html(WorldPages.Workspace(db, code, version, view, invite.Length > 0 ? invite : null));
        });

        app.MapGet("/world/archive", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            // Server-side filters; values are matched exactly as parameters (never concatenated),
            // and difficulty/track are additionally constrained to the known enums.
            var industry = ctx.Request.Query["industry"].ToString();
            var difficulty = ctx.Request.Query["difficulty"].ToString();
            var track = ctx.Request.Query["track"].ToString();
            if (!WorldContent.Difficulties.Contains(difficulty)) difficulty = "";
            if (!WorldContent.Tracks.Contains(track)) track = "";

            var sql = "SELECT code,title,industry,track,difficulty,est_minutes FROM pciworld_challenges WHERE current_version>=1 AND retired=0";
            var args = new List<object?>();
            if (industry.Length > 0) { sql += " AND industry=?"; args.Add(industry); }
            if (difficulty.Length > 0) { sql += " AND difficulty=?"; args.Add(difficulty); }
            if (track.Length > 0) { sql += " AND track=?"; args.Add(track); }
            sql += " ORDER BY id ASC LIMIT 200";
            var rows = db.Query(sql, args.ToArray());
            var industries = db.Query(@"SELECT DISTINCT industry FROM pciworld_challenges
                    WHERE current_version>=1 AND retired=0 AND industry IS NOT NULL ORDER BY industry")
                .Select(r => H.Str(r["industry"]) ?? "").Where(s => s.Length > 0).ToList();
            return Html(WorldPages.Archive(db, rows, industries, industry, difficulty, track));
        });

        app.MapGet("/world/about", () => !Enabled() ? Disabled() : Html(WorldPages.About(db)));

        app.MapGet("/world/r/{token}", (string token) =>
        {
            if (!Enabled()) return Disabled();
            var att = db.QueryOne(@"SELECT * FROM pciworld_attempts
                WHERE result_token_sha=? AND result_revoked=0 AND status='completed'", Security.Sha(token));
            if (att is null) return Results.NotFound();   // absent and revoked are indistinguishable
            var version = WorldLifecycle.PinnedVersion(db, H.L(att["challenge_id"]), H.L(att["version"]));
            if (version is null) return Results.NotFound();
            Track("result_viewed", H.L(att["challenge_id"]));
            return Html(WorldPages.PublicResult(db, att, version));
        });

        app.MapGet("/world/i/{token}", (string token) =>
        {
            if (!Enabled()) return Disabled();
            var inv = db.QueryOne(@"SELECT i.inviter_name, a.challenge_id, a.version FROM pciworld_invites i
                JOIN pciworld_attempts a ON a.id=i.attempt_id
                WHERE i.token_sha=? AND i.revoked=0", Security.Sha(token));
            if (inv is null) return Results.NotFound();
            var ch = db.QueryOne("SELECT code,retired FROM pciworld_challenges WHERE id=?", inv["challenge_id"]);
            if (ch is null || H.L(ch["retired"]) == 1) return Results.NotFound();
            var version = WorldLifecycle.PinnedVersion(db, H.L(inv["challenge_id"]), H.L(inv["version"]));
            if (version is null) return Results.NotFound();
            Track("invite_opened", H.L(inv["challenge_id"]));
            return Html(WorldPages.InvitePage(db, H.Str(inv["inviter_name"]), version, H.Str(ch["code"])!, token));
        });

        // ───────────────────────────── public API ─────────────────────────────

        app.MapPost("/api/world/session", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("sess|" + Ip(ctx), 60))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", Security.Sha(token));
            return Results.Json(new { token });
        });

        app.MapGet("/api/world/today", () =>
        {
            if (!Enabled()) return Disabled();
            var today = WorldLifecycle.Today(db, DateTime.UtcNow);
            var version = today is null ? null : WorldLifecycle.LiveVersion(db, H.L(today["id"]));
            if (today is null || version is null) return Results.Json(new { available = false });
            return Results.Json(new
            {
                available = true,
                code = H.Str(today["code"]),
                title = H.Str(version["title"]),
                hook = H.Str(version["hook"]),
                industry = H.Str(version["industry"]),
                difficulty = H.Str(version["difficulty"]),
                est_minutes = H.L(version["est_minutes"]),
                version = H.L(version["version"]),
                changes_at_utc = "00:00",
            });
        });

        app.MapPost("/api/world/attempts", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            var sess = Session(ctx);
            if (sess is null) return Results.Json(new { error = "no_session", message = "Start a session first." }, statusCode: 401);
            if (Throttled("start|" + H.L(sess["id"]), 40))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);

            var b = await H.Body(ctx.Request);
            var code = (H.GetS(b, "code") ?? "").Trim();
            var inviteTok = (H.GetS(b, "invite") ?? "").Trim();

            var ch = db.QueryOne("SELECT * FROM pciworld_challenges WHERE code=? AND current_version>=1 AND retired=0", code);
            if (ch is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var challengeId = H.L(ch["id"]);
            var version = H.L(ch["current_version"]);
            long? inviteId = null;
            if (inviteTok.Length > 0)
            {
                var inv = db.QueryOne(@"SELECT i.id, a.challenge_id, a.version FROM pciworld_invites i
                    JOIN pciworld_attempts a ON a.id=i.attempt_id
                    WHERE i.token_sha=? AND i.revoked=0", Security.Sha(inviteTok));
                if (inv is not null && H.L(inv["challenge_id"]) == challengeId)
                { version = H.L(inv["version"]); inviteId = H.L(inv["id"]); }
            }
            var snapshot = WorldLifecycle.PinnedVersion(db, challengeId, version);
            if (snapshot is null) return Results.Json(new { error = "not_found" }, statusCode: 404);

            var existing = db.QueryOne(@"SELECT * FROM pciworld_attempts
                WHERE session_id=? AND challenge_id=? AND version=? AND status='in_progress'
                ORDER BY id DESC LIMIT 1", H.L(sess["id"]), challengeId, version);
            long attemptId;
            object? saved = null;
            if (existing is not null)
            {
                attemptId = H.L(existing["id"]);
                try { saved = JsonDocument.Parse(H.Str(existing["answers_json"]) ?? "{}").RootElement.Clone(); }
                catch { saved = new { }; }
            }
            else
            {
                attemptId = db.ExecuteReturningId(@"INSERT INTO pciworld_attempts
                    (session_id,challenge_id,version,invite_id,answers_json) VALUES(?,?,?,?, '{}')",
                    H.L(sess["id"]), challengeId, version, inviteId);
                Track("challenge_started", challengeId, H.L(sess["id"]));
                log(null, "world_attempt_start", $"{code} v{version} #{attemptId}");
            }
            return Results.Json(new
            {
                attempt_id = attemptId,
                resumed = existing is not null,
                completed = false,
                answers = saved,
                title = H.Str(snapshot["title"]),
                version,
                view = WorldContent.PublicView(H.Str(snapshot["config_json"])!),
            });
        });

        app.MapPost("/api/world/attempts/{id:long}/save", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Disabled();
            var sess = Session(ctx);
            if (sess is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var answers = H.GetEl(b, "answers") is { ValueKind: JsonValueKind.Object } a ? a.GetRawText() : "{}";
            var n = db.Execute(@"UPDATE pciworld_attempts SET answers_json=?, updated_at=datetime('now')
                WHERE id=? AND session_id=? AND status='in_progress'", answers, id, H.L(sess["id"]));
            return n == 0
                ? Results.Json(new { error = "not_found" }, statusCode: 404)
                : Results.Json(new { ok = true });
        });

        app.MapPost("/api/world/attempts/{id:long}/submit", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Disabled();
            var sess = Session(ctx);
            if (sess is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (Throttled("submit|" + H.L(sess["id"]), 40))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);

            var att = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=? AND session_id=?", id, H.L(sess["id"]));
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(att["status"]) == "completed")
                return Results.Json(new { error = "already_submitted", message = "This attempt is already graded." }, statusCode: 409);

            var snapshot = WorldLifecycle.PinnedVersion(db, H.L(att["challenge_id"]), H.L(att["version"]));
            if (snapshot is null) return Results.Json(new { error = "not_found" }, statusCode: 404);

            var b = await H.Body(ctx.Request);
            var answersEl = H.GetEl(b, "answers") is { ValueKind: JsonValueKind.Object } a ? a : default;
            var answersRaw = answersEl.ValueKind == JsonValueKind.Object ? answersEl.GetRawText() : "{}";
            var config = JsonDocument.Parse(H.Str(snapshot["config_json"])!).RootElement;
            var r = WorldScore.Grade(config, answersEl);

            var dims = JsonSerializer.Serialize(new { calculation = r.Calculation, decision = r.Decision });
            // First submit wins — a second submit races into the 0-rows branch and gets 409.
            var n = db.Execute(@"UPDATE pciworld_attempts SET status='completed', answers_json=?, score=?,
                    dimensions_json=?, profile_key=?, completed_at=datetime('now'), updated_at=datetime('now')
                WHERE id=? AND status='in_progress'",
                answersRaw, r.Score, dims, r.ProfileKey, id);
            if (n == 0) return Results.Json(new { error = "already_submitted" }, statusCode: 409);

            Track("challenge_completed", H.L(att["challenge_id"]), H.L(sess["id"]));
            log(null, "world_attempt_submit", $"#{id} {r.Score}%");
            string shareLine = "";
            if (config.TryGetProperty("share_line", out var sl) && sl.ValueKind == JsonValueKind.String)
                shareLine = sl.GetString() ?? "";
            return Results.Json(ResultPayload(r, shareLine));
        });

        app.MapGet("/api/world/attempts/{id:long}", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Disabled();
            var sess = Session(ctx);
            if (sess is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var att = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=? AND session_id=?", id, H.L(sess["id"]));
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var snapshot = WorldLifecycle.PinnedVersion(db, H.L(att["challenge_id"]), H.L(att["version"]));
            if (snapshot is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            object? result = null;
            if (H.Str(att["status"]) == "completed")
            {
                // Deterministic replay from the pinned snapshot and the stored answers.
                var config = JsonDocument.Parse(H.Str(snapshot["config_json"])!).RootElement;
                JsonElement answers = default;
                try { answers = JsonDocument.Parse(H.Str(att["answers_json"]) ?? "{}").RootElement; } catch { }
                var r = WorldScore.Grade(config, answers);
                string shareLine = "";
                if (config.TryGetProperty("share_line", out var sl) && sl.ValueKind == JsonValueKind.String)
                    shareLine = sl.GetString() ?? "";
                result = ResultPayload(r, shareLine);
            }
            return Results.Json(new
            {
                attempt_id = id,
                status = H.Str(att["status"]),
                title = H.Str(snapshot["title"]),
                version = H.L(att["version"]),
                view = WorldContent.PublicView(H.Str(snapshot["config_json"])!),
                result,
            });
        });

        app.MapPost("/api/world/attempts/{id:long}/share", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Disabled();
            var sess = Session(ctx);
            if (sess is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (Throttled("share|" + H.L(sess["id"]), 20))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);
            var att = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=? AND session_id=? AND status='completed'", id, H.L(sess["id"]));
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);

            var b = await H.Body(ctx.Request);
            if (H.GetEl(b, "revoke") is { ValueKind: JsonValueKind.True })
            {
                db.Execute("UPDATE pciworld_attempts SET result_revoked=1, updated_at=datetime('now') WHERE id=?", id);
                return Results.Json(new { ok = true, revoked = true });
            }
            var name = (H.GetS(b, "display_name") ?? "").Trim();
            if (name.Length > 80) name = name[..80];
            var token = Security.RandomHex(32);
            db.Execute(@"UPDATE pciworld_attempts SET result_token_sha=?, result_revoked=0, display_name=?,
                updated_at=datetime('now') WHERE id=?", Security.Sha(token), name.Length > 0 ? name : null, id);
            Track("share_created", H.L(att["challenge_id"]), H.L(sess["id"]));
            return Results.Json(new { ok = true, url = "/world/r/" + token });
        });

        // ---- content reports: anonymous-friendly, no PII required, rate-limited ----
        app.MapPost("/api/world/report", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            if (Throttled("report|" + Ip(ctx), 10))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);
            var b = await H.Body(ctx.Request);
            var category = (H.GetS(b, "category") ?? "").Trim();
            if (!WorldReportCategories.Contains(category))
                return Results.Json(new { error = "bad_category", message = $"Category must be one of: {string.Join(", ", WorldReportCategories)}." }, statusCode: 400);
            var message = (H.GetS(b, "message") ?? "").Trim();
            if (message.Length < 10 || message.Length > 2000)
                return Results.Json(new { error = "bad_message", message = "Describe the issue in 10–2000 characters." }, statusCode: 400);
            long? challengeId = null;
            var code = (H.GetS(b, "code") ?? "").Trim();
            if (code.Length > 0)
            {
                var ch = db.QueryOne("SELECT id FROM pciworld_challenges WHERE code=?", code);
                if (ch is not null) challengeId = H.L(ch["id"]);
            }
            var sess = Session(ctx);
            var id = db.ExecuteReturningId("INSERT INTO pciworld_reports(challenge_id,category,message,session_id) VALUES(?,?,?,?)",
                challengeId, category, message, sess is null ? null : H.L(sess["id"]));
            Track("content_reported", challengeId, sess is null ? null : H.L(sess["id"]));
            log(null, "world_report", $"#{id} {category} {code}");
            return Results.Json(new { ok = true, reference = $"WR-{id}",
                message = "Thank you — the PCI World content team reviews every report." });
        });

        app.MapPost("/api/world/attempts/{id:long}/invite", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Disabled();
            var sess = Session(ctx);
            if (sess is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (Throttled("invite|" + H.L(sess["id"]), 20))
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);
            var att = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=? AND session_id=? AND status='completed'", id, H.L(sess["id"]));
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);

            var b = await H.Body(ctx.Request);
            var name = (H.GetS(b, "name") ?? "").Trim();
            if (name.Length > 80) name = name[..80];
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO pciworld_invites(attempt_id,token_sha,inviter_name) VALUES(?,?,?)",
                id, Security.Sha(token), name.Length > 0 ? name : null);
            Track("invite_created", H.L(att["challenge_id"]), H.L(sess["id"]));
            return Results.Json(new { ok = true, url = "/world/i/" + token });
        });
    }

    public static readonly string[] WorldReportCategories =
        { "content_error", "calculation", "accessibility", "inappropriate", "other" };

    /// <summary>The post-submit payload — the ONLY place reference values and consequences are
    /// serialized, and only after grading (reveal policy: after_submit).</summary>
    static object ResultPayload(WorldScore.Result r, string shareLine) => new
    {
        ok = true,
        score = r.Score,
        calculation = r.Calculation,
        decision = r.Decision,
        profile = r.ProfileKey,
        profile_reason = r.ProfileReason,
        improvement = r.ImprovementArea,
        share_line = shareLine,
        measures = r.Measures.Select(m => new { m.Key, label = m.Label, correct = m.Correct, reference = m.Reference, yours = m.Yours }),
        decisions = r.Decisions.Select(d => new
        {
            d.Key, prompt = d.Prompt, chosen = d.Chosen, chosen_label = d.ChosenLabel,
            quality = d.Quality, consequence = d.Consequence, principle = d.Principle, best_label = d.BestLabel,
        }),
    };
}
