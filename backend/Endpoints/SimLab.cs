using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI AI Project Controls Simulation Lab — student surface (Phase 1 foundation).
///
///   GET  /api/me/lab/access                 — whether the student can enter the Lab, and why (live entitlement)
///   GET  /api/me/lab/catalogue              — the published labs/drills/scenarios + this student's latest status
///   POST /api/me/lab/attempts               — start (or resume) an attempt at a scenario; returns the task
///   GET  /api/me/lab/attempts               — this student's recent attempts
///   GET  /api/me/lab/attempts/{id}          — load one attempt (resume in-progress, or review a completed one)
///   POST /api/me/lab/attempts/{id}/submit   — deterministic grading + competency evidence
///
/// Reuses the existing account (Core.Auth.UserFromReq) — no separate Lab login. Gated by the operator flag
/// `sp_simlab_enabled` and the `simlab_requires` entitlement rule (Core.SimLab). Grading is deterministic
/// (Core.SimCalc / Core.SimGrade); the answer key is never stored or served — it is derived from the
/// scenario's synthetic inputs at grade time, and withheld entirely in Assessment Mode. Applied simulation
/// only; never touches exam_attempts, entitlements or issued credentials. Distinct from Certuvo (MCQ prep).
/// </summary>
public static class SimLab
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        UserCtx? Auth(HttpContext ctx) => Core.Auth.UserFromReq(ctx.Request, db);
        IResult J(object o) => Results.Json(o);

        // Auth + operator-flag gate. (Access-level entitlement is reported by /access itself, not gated
        // here, so the student can always learn *why* they don't yet have access.)
        IResult? Gate(HttpContext ctx, out UserCtx? u)
        {
            u = Auth(ctx);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!Core.SimLab.Enabled(db)) return Results.Json(new { error = "lab_disabled" }, statusCode: 403);
            return null;
        }

        // Per-user throttle on attempt writes — start/submit both INSERT/UPDATE rows; the window is far
        // above any human pace (a lab takes minutes). Same shape as Certuvo's limiter.
        var rlHits = new System.Collections.Concurrent.ConcurrentDictionary<string, (int count, long windowStart)>();
        const int RL_LIMIT = 30; const long RL_WINDOW_MS = 600_000;   // 30 writes per action per 10 minutes
        IResult? Throttle(long userId, string action)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (rlHits.Count > 10_000)
                foreach (var kv in rlHits)
                    if (now - kv.Value.windowStart >= RL_WINDOW_MS) rlHits.TryRemove(kv.Key, out _);
            var entry = rlHits.AddOrUpdate(userId + "|" + action, (1, now), (_, cur) =>
                now - cur.windowStart >= RL_WINDOW_MS ? (1, now) : (cur.count + 1, cur.windowStart));
            if (entry.count <= RL_LIMIT) return null;
            var retryAfter = Math.Max(1, (RL_WINDOW_MS - (now - entry.windowStart)) / 1000);
            return Results.Json(new { error = "rate_limited", retry_after = retryAfter,
                message = "You're moving through labs faster than we can grade — please take a short break." }, statusCode: 429);
        }

        object ScenarioMeta(IDictionary<string, object?> s) => new
        {
            id = H.L(s["id"]),
            scenario_code = H.Str(s["scenario_code"]),
            title = H.Str(s["title"]),
            kind = H.Str(s["kind"]),
            difficulty = s.TryGetValue("difficulty", out var d) ? H.Str(d) : null,
            summary = s.TryGetValue("summary", out var sm) ? H.Str(sm) : null,
            version = H.L(s["version"]),
        };

        // ---- access: live entitlement descriptor (never a raw error; friendly student-facing copy) ----
        app.MapGet("/api/me/lab/access", (HttpContext ctx) =>
        {
            var u = Auth(ctx);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(Core.SimLab.AccessFor(db, u.Id));
        });

        // ---- catalogue: published labs/drills/scenarios + this student's latest attempt per scenario ----
        app.MapGet("/api/me/lab/catalogue", (HttpContext ctx) =>
        {
            if (Gate(ctx, out var u) is { } blocked) return blocked;
            if (!Core.SimLab.Eligible(db, u!.Id))
                return Results.Json(new { error = "no_access", access = Core.SimLab.AccessFor(db, u.Id) }, statusCode: 403);

            // The student's latest attempt per scenario (for status/score badges), keyed by scenario_id.
            var latest = new Dictionary<long, (string status, object? score, long attemptId)>();
            foreach (var a in db.Query(@"SELECT id,scenario_id,status,score FROM simulation_attempts
                WHERE user_id=? ORDER BY id ASC", u.Id))
                latest[H.L(a["scenario_id"])] = (H.Str(a["status"]) ?? "in_progress", a["score"], H.L(a["id"]));

            var rows = new List<object>();
            foreach (var s in db.Query(@"SELECT id,scenario_code,title,kind,industry,project_type,difficulty,
                est_minutes,competencies_json,certification_id,summary,version,config_json FROM simulation_scenarios
                WHERE status='published' ORDER BY sort_order ASC, id ASC"))
            {
                var sid = H.L(s["id"]);
                var has = latest.TryGetValue(sid, out var at);
                rows.Add(new
                {
                    id = sid,
                    scenario_code = H.Str(s["scenario_code"]),
                    title = H.Str(s["title"]),
                    kind = H.Str(s["kind"]),
                    industry = H.Str(s["industry"]),
                    project_type = H.Str(s["project_type"]),
                    difficulty = H.Str(s["difficulty"]),
                    est_minutes = H.L(s["est_minutes"]),
                    competencies = ParseArray(H.Str(s["competencies_json"])),
                    certification_id = s["certification_id"] is null ? (long?)null : H.L(s["certification_id"]),
                    summary = H.Str(s["summary"]),
                    version = H.L(s["version"]),
                    interactive = !string.IsNullOrWhiteSpace(H.Str(s["config_json"])),   // has a runnable task
                    attempt_status = has ? at.status : (string?)null,
                    attempt_id = has ? at.attemptId : (long?)null,
                    score = has ? at.score : null,
                });
            }
            return J(new { rows });
        });

        // ---- start (or resume) an attempt ----
        app.MapPost("/api/me/lab/attempts", async (HttpContext ctx) =>
        {
            if (Gate(ctx, out var u) is { } blocked) return blocked;
            if (!Core.SimLab.Eligible(db, u!.Id))
                return Results.Json(new { error = "no_access", access = Core.SimLab.AccessFor(db, u.Id) }, statusCode: 403);
            if (Throttle(u.Id, "start") is { } limited) return limited;

            var b = await H.Body(ctx.Request);
            var code = (H.GetS(b, "scenario_code") ?? "").Trim();
            var mode = NormMode(H.GetS(b, "mode"));

            var s = db.QueryOne(@"SELECT id,scenario_code,title,kind,difficulty,summary,config_json,version,status
                FROM simulation_scenarios WHERE scenario_code=?", code);
            if (s is null || H.Str(s["status"]) != "published")
                return Results.Json(new { error = "not_found" }, statusCode: 404);
            var configRaw = H.Str(s["config_json"]);
            if (string.IsNullOrWhiteSpace(configRaw))
                return Results.Json(new { error = "not_interactive", message = "This lab is not yet interactive." }, statusCode: 409);

            JsonElement config;
            try { config = JsonDocument.Parse(configRaw).RootElement; }
            catch { return Results.Json(new { error = "bad_config" }, statusCode: 500); }

            var scenarioId = H.L(s["id"]);
            var version = H.L(s["version"]);
            // Resume an existing in-progress attempt for this scenario+mode rather than spawning duplicates.
            var existing = db.QueryOne(@"SELECT id,seed FROM simulation_attempts
                WHERE user_id=? AND scenario_id=? AND mode=? AND status='in_progress' ORDER BY id DESC LIMIT 1",
                u.Id, scenarioId, mode);
            long attemptId, seed;
            if (existing is not null) { attemptId = H.L(existing["id"]); seed = H.L(existing["seed"]); }
            else
            {
                attemptId = db.ExecuteReturningId(@"INSERT INTO simulation_attempts
                    (user_id,scenario_id,scenario_version,mode,status,seed,state_json)
                    VALUES(?,?,?,?, 'in_progress', 0, '{}')", u.Id, scenarioId, version, mode);
                // A scenario with a variant block draws a per-attempt instance: the seed is the attempt id —
                // unique, non-zero and stored — so serving, resume and grading all re-derive the same numbers.
                // Non-variant scenarios keep seed 0, so existing content behaves exactly as before.
                seed = Core.SimVariant.HasVariants(config) ? attemptId : 0;
                if (seed != 0) db.Execute("UPDATE simulation_attempts SET seed=? WHERE id=?", seed, attemptId);
                log(u.Id, "sim_attempt_start", $"{code} · {mode} · #{attemptId}");
            }
            return J(new { attempt_id = attemptId, resumed = existing is not null,
                scenario = ScenarioMeta(s), task = Core.SimGrade.TaskFor(EffectiveConfig(configRaw!, seed), mode) });
        });

        // ---- list this student's attempts ----
        app.MapGet("/api/me/lab/attempts", (HttpContext ctx) =>
        {
            if (Gate(ctx, out var u) is { } blocked) return blocked;
            var rows = db.Query(@"SELECT a.id,a.mode,a.status,a.score,a.started_at,a.completed_at,
                s.scenario_code,s.title,s.kind FROM simulation_attempts a
                JOIN simulation_scenarios s ON s.id=a.scenario_id
                WHERE a.user_id=? ORDER BY a.id DESC LIMIT 50", u!.Id)
                .Select(r => new
                {
                    id = H.L(r["id"]), mode = H.Str(r["mode"]), status = H.Str(r["status"]),
                    score = r["score"], scenario_code = H.Str(r["scenario_code"]),
                    title = H.Str(r["title"]), kind = H.Str(r["kind"]),
                    started_at = H.Str(r["started_at"]), completed_at = H.Str(r["completed_at"]),
                });
            return J(new { rows });
        });

        // ---- load one attempt (resume in-progress; review a completed one, re-graded deterministically) ----
        app.MapGet("/api/me/lab/attempts/{id:long}", (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, out var u) is { } blocked) return blocked;
            var att = db.QueryOne("SELECT * FROM simulation_attempts WHERE id=? AND user_id=?", id, u!.Id);
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var s = db.QueryOne(@"SELECT id,scenario_code,title,kind,difficulty,summary,config_json,version
                FROM simulation_scenarios WHERE id=?", H.L(att["scenario_id"]));
            if (s is null || string.IsNullOrWhiteSpace(H.Str(s["config_json"])))
                return Results.Json(new { error = "not_interactive" }, statusCode: 409);

            var config = EffectiveConfig(H.Str(s["config_json"])!, H.L(att["seed"]));
            var mode = H.Str(att["mode"]) ?? "training";
            var status = H.Str(att["status"]) ?? "in_progress";
            var completed = status is "completed" or "passed" or "failed";

            object? grade = null;
            if (completed)
            {
                var answers = LoadAnswers(H.Str(att["state_json"]));
                var g = Core.SimGrade.Grade(config, answers, mode);   // deterministic replay from stored answers
                grade = GradePayload(g, mode, CpmSchedule(config, mode));
            }
            return J(new
            {
                attempt_id = id, status, mode,
                scenario = ScenarioMeta(s),
                task = Core.SimGrade.TaskFor(config, mode),
                score = att["score"],
                started_at = H.Str(att["started_at"]), completed_at = H.Str(att["completed_at"]),
                grade,
            });
        });

        // ---- submit + grade (deterministic; writes competency evidence + audit + analytics) ----
        app.MapPost("/api/me/lab/attempts/{id:long}/submit", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, out var u) is { } blocked) return blocked;
            if (Throttle(u!.Id, "submit") is { } limited) return limited;

            var att = db.QueryOne("SELECT * FROM simulation_attempts WHERE id=? AND user_id=?", id, u.Id);
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(att["status"]) is "completed" or "passed" or "failed")
                return Results.Json(new { error = "already_submitted" }, statusCode: 409);

            var s = db.QueryOne("SELECT id,scenario_code,config_json FROM simulation_scenarios WHERE id=?", H.L(att["scenario_id"]));
            if (s is null || string.IsNullOrWhiteSpace(H.Str(s["config_json"])))
                return Results.Json(new { error = "not_interactive" }, statusCode: 409);
            var config = EffectiveConfig(H.Str(s["config_json"])!, H.L(att["seed"]));
            var mode = H.Str(att["mode"]) ?? "training";

            var b = await H.Body(ctx.Request);
            var answersEl = H.GetEl(b, "answers") ?? default;
            var answersRaw = answersEl.ValueKind == JsonValueKind.Object ? answersEl.GetRawText() : "{}";
            var grade = Core.SimGrade.Grade(config, answersEl, mode);

            var status = grade.Passed ? "passed" : "completed";
            var stateJson = "{\"answers\":" + answersRaw + "}";   // answersRaw is valid JSON → composite is valid
            db.Execute(@"UPDATE simulation_attempts SET status=?, score=?, state_json=?,
                submitted_at=datetime('now'), completed_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
                status, grade.Score, stateJson, id);

            // Competency evidence — one row per competency the scenario exercised (§25.4: mastery is derived
            // from many pieces of evidence downstream, never a single score).
            foreach (var (comp, cscore, level) in grade.Competencies)
                db.Execute("INSERT INTO simulation_competency(attempt_id,user_id,competency,score,level) VALUES(?,?,?,?,?)",
                    id, u.Id, comp, cscore, level);

            Analytics.Track(db, ctx, "sim_attempt_completed", u.Id, grade.Score, null, H.Str(s["scenario_code"]));
            log(u.Id, "sim_attempt_submit", $"{H.Str(s["scenario_code"])} {grade.Score}%{(grade.Passed ? " pass" : "")} #{id}");

            return J(GradePayload(grade, mode, CpmSchedule(config, mode)));
        });

        // ---- AI Coach: grounded explanation of an attempt (deterministic engine supplies every number;
        //      refused in Assessment Mode; degrades to a built-in explainer when no provider is configured) ----
        app.MapPost("/api/me/lab/attempts/{id:long}/coach", async (HttpContext ctx, long id) =>
        {
            if (Gate(ctx, out var u) is { } blocked) return blocked;
            if (!Core.SimCoach.Enabled(db)) return Results.Json(new { error = "coach_disabled" }, statusCode: 403);
            if (Throttle(u!.Id, "coach") is { } limited) return limited;

            var att = db.QueryOne("SELECT * FROM simulation_attempts WHERE id=? AND user_id=?", id, u.Id);
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var s = db.QueryOne("SELECT scenario_code,config_json FROM simulation_scenarios WHERE id=?", H.L(att["scenario_id"]));
            if (s is null || string.IsNullOrWhiteSpace(H.Str(s["config_json"])))
                return Results.Json(new { error = "not_interactive" }, statusCode: 409);
            var config = EffectiveConfig(H.Str(s["config_json"])!, H.L(att["seed"]));
            var mode = H.Str(att["mode"]) ?? "training";

            var b = await H.Body(ctx.Request);
            // The answers the student is asking about: what they pass now, else what they last submitted.
            var answers = H.GetEl(b, "answers") ?? LoadAnswers(H.Str(att["state_json"]));
            var question = H.GetS(b, "question");

            var coach = await Core.SimCoach.Coach(db, config, answers, mode, question);
            log(u.Id, "sim_coach", $"{H.Str(s["scenario_code"])} · {coach.Source} #{id}");
            return Results.Json(new { ok = coach.Ok, message = coach.Message, source = coach.Source, ai = coach.Ai });
        });
    }

    static string NormMode(string? raw)
    {
        var mode = (raw ?? "training").Trim().ToLowerInvariant();
        return mode is "training" or "challenge" or "assessment" or "sandbox" ? mode : "training";
    }

    /// <summary>The exact task instance an attempt runs: the scenario's config with its variant applied for
    /// the attempt's stored <c>seed</c>. Seed 0 — or a scenario with no variant block — yields the canonical
    /// instance, so re-deriving from the stored seed reproduces identical numbers for serving, resume and
    /// grading. SimCalc still derives every answer from these inputs; no answer key is ever stored.</summary>
    static JsonElement EffectiveConfig(string configRaw, long seed) =>
        JsonDocument.Parse(Core.SimVariant.Derive(configRaw, seed)).RootElement;

    static JsonElement LoadAnswers(string? stateJson)
    {
        if (string.IsNullOrWhiteSpace(stateJson)) return default;
        try
        {
            var root = JsonDocument.Parse(stateJson).RootElement;
            return root.TryGetProperty("answers", out var a) ? a.Clone() : default;
        }
        catch { return default; }
    }

    // Mode-aware grade payload: Assessment Mode reports only right/wrong (SimGrade already nulls the
    // correct/your values), so nothing here re-introduces the answer key. The optional `schedule` is the
    // computed Gantt for a graded CPM lab — supplied only in reveal (non-assessment) modes.
    static object GradePayload(Core.SimGrade.GradeResult g, string mode, object? schedule = null) => new
    {
        ok = true,
        score = g.Score, passed = g.Passed, correct = g.Correct, total = g.Total, mode,
        assessment = mode == "assessment",
        measures = g.Measures.Select(m => new
        {
            key = m.Key, label = m.Label, is_correct = m.Correct,
            correct_value = m.Correct_Value, your_value = m.Your_Value,
        }),
        competencies = g.Competencies.Select(c => new { competency = c.competency, score = c.score, level = c.level }),
        schedule,
    };

    // The computed schedule for a graded CPM lab, for the Gantt in the result view. This IS the answer, so
    // it is withheld in Assessment Mode and only ever attached after an attempt has been submitted/graded.
    static object? CpmSchedule(JsonElement config, string mode)
    {
        if (mode == "assessment") return null;
        var task = config.TryGetProperty("task", out var tk) ? tk.GetString() : "";
        if (task != "cpm" || !config.TryGetProperty("given", out var given)) return null;
        try
        {
            var r = Core.SimCalc.ScheduleFrom(given);
            return new
            {
                project_duration = r.ProjectDuration,
                critical_path = r.CriticalPath,
                bars = r.Nodes.Select(n => new
                {
                    id = n.Id, duration = n.Duration, es = n.Es, ef = n.Ef,
                    ls = n.Ls, lf = n.Lf, total_float = n.TotalFloat, critical = n.Critical,
                }),
            };
        }
        catch { return null; }
    }

    static string[] ParseArray(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
        try { return JsonSerializer.Deserialize<string[]>(json) ?? Array.Empty<string>(); }
        catch { return Array.Empty<string>(); }
    }
}
