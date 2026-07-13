using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Certuvo (master-plan Phase 8) — PCI's official study &amp; practice engine, in the student portal.
/// Formative, un-proctored practice that mirrors the PCP-AI exam format (scenario-based MCQs), with
/// immediate feedback and teaching explanations, drawn from the PRACTICE pool
/// (sample_questions.is_practice=1) which is deliberately SEPARATE from the secure examination bank.
/// The answer key and explanations are never sent to the client until an attempt is submitted.
///
///   GET  /api/me/certuvo/overview  — domains + counts, the student's readiness, recent attempts
///   POST /api/me/certuvo/start     — begin a quiz (by domain) or a full-length mock; returns questions
///   POST /api/me/certuvo/submit    — server-side grading + per-domain breakdown + explanations
///   GET  /api/me/certuvo/history   — past completed attempts
///
/// Practice never touches exam_attempts, entitlements or issued credentials — a pass is still earned
/// only on the real examination.
/// </summary>
public static class Certuvo
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        UserCtx? Auth(HttpContext ctx) => Core.Auth.UserFromReq(ctx.Request, db);
        IResult J(object o) => Results.Json(o);
        bool Enabled() => Settings.Bool(db, "sp_practice_enabled", true);
        IResult? Gate(HttpContext ctx, out UserCtx? u)
        {
            u = Auth(ctx);
            if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!Enabled()) return Results.Json(new { error = "practice_disabled" }, statusCode: 403);
            return null;
        }

        // ---------------- overview: domains, readiness, recent ----------------
        app.MapGet("/api/me/certuvo/overview", (HttpContext ctx) =>
        {
            var deny = Gate(ctx, out var u); if (deny is not null) return deny;
            var domains = db.Query("SELECT domain, COUNT(*) n FROM sample_questions WHERE published=1 AND is_practice=1 GROUP BY domain ORDER BY domain")
                .Where(r => H.Str(r["domain"]) is { Length: > 0 })
                .Select(r => new { domain = H.Str(r["domain"]), count = H.L(r["n"]) }).ToList();
            var total = domains.Sum(d => d.count);

            var attempts = db.Query("SELECT mode,domain,score,total,domain_breakdown,completed_at FROM practice_attempts WHERE user_id=? AND status='completed' ORDER BY id DESC", u!.Id);
            long sumScore = 0, sumTotal = 0;
            var perDomain = new Dictionary<string, (long correct, long total)>();
            foreach (var a in attempts)
            {
                sumScore += H.L(a["score"]); sumTotal += H.L(a["total"]);
                if (H.Str(a["domain_breakdown"]) is { Length: > 0 } bd)
                {
                    try
                    {
                        foreach (var p in JsonDocument.Parse(bd).RootElement.EnumerateObject())
                        {
                            var c = p.Value.TryGetProperty("correct", out var cc) ? cc.GetInt64() : 0;
                            var t = p.Value.TryGetProperty("total", out var tt) ? tt.GetInt64() : 0;
                            var cur = perDomain.GetValueOrDefault(p.Name);
                            perDomain[p.Name] = (cur.correct + c, cur.total + t);
                        }
                    }
                    catch { /* skip a malformed breakdown */ }
                }
            }
            var pass = (int)Settings.Num(db, "exam_pass_mark_pct", 65);
            var overallPct = sumTotal > 0 ? (int)Math.Round(100.0 * sumScore / sumTotal) : 0;
            string label = attempts.Count == 0 ? "Not started"
                : overallPct >= pass + 10 ? "Ready"
                : overallPct >= pass ? "Approaching"
                : "Building";
            var recent = attempts.Take(6).Select(a => new
            {
                mode = H.Str(a["mode"]), domain = H.Str(a["domain"]),
                score = H.L(a["score"]), total = H.L(a["total"]),
                pct = H.L(a["total"]) > 0 ? (int)Math.Round(100.0 * H.L(a["score"]) / H.L(a["total"])) : 0,
                completed_at = H.Str(a["completed_at"]),
            });
            return J(new
            {
                enabled = true, total_questions = total, pass_mark = pass,
                domains,
                readiness = new
                {
                    attempts = attempts.Count, overall_pct = overallPct, label,
                    per_domain = perDomain.OrderByDescending(kv => kv.Value.total).Select(kv => new
                    {
                        domain = kv.Key, answered = kv.Value.total,
                        pct = kv.Value.total > 0 ? (int)Math.Round(100.0 * kv.Value.correct / kv.Value.total) : 0,
                    }),
                },
                recent,
            });
        });

        // ---------------- start a practice session ----------------
        app.MapPost("/api/me/certuvo/start", async (HttpContext ctx) =>
        {
            var deny = Gate(ctx, out var u); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var mode = (H.GetS(b, "mode") ?? "quiz").Trim().ToLowerInvariant();
            if (mode is not ("quiz" or "mock")) mode = "quiz";
            var domain = H.GetS(b, "domain");
            if (string.IsNullOrWhiteSpace(domain)) domain = null;

            var pool = domain is null
                ? db.Query("SELECT id,question,options,option_a,option_b,option_c,option_d,domain FROM sample_questions WHERE published=1 AND is_practice=1")
                : db.Query("SELECT id,question,options,option_a,option_b,option_c,option_d,domain FROM sample_questions WHERE published=1 AND is_practice=1 AND domain=?", domain);
            if (pool.Count == 0) return Results.Json(new { error = "no_questions", message = "No practice questions are available yet." }, statusCode: 400);

            // quiz: a short focused set; mock: a full-length mixed sitting. Clamp to what exists.
            var want = mode == "mock" ? 40 : (int)Math.Clamp(H.GetNum(b, "count") ?? 10, 5, 25);
            var rng = new Random();
            var picked = pool.OrderBy(_ => rng.Next()).Take(Math.Min(want, pool.Count)).ToList();
            var qids = picked.Select(r => H.L(r["id"])).ToList();

            var attemptId = db.ExecuteReturningId(
                "INSERT INTO practice_attempts(user_id,mode,domain,question_ids,status) VALUES(?,?,?,?,'in_progress')",
                u!.Id, mode, domain, JsonSerializer.Serialize(qids));

            var questions = picked.Select(r => new { id = H.L(r["id"]), question = H.Str(r["question"]), options = H.OptionsFor(r), domain = H.Str(r["domain"]) });
            // Advisory timer for mocks (mirrors sitting under time); quizzes are untimed.
            int? durationMinutes = mode == "mock" ? Math.Max(30, qids.Count * 2) : null;
            log(u.Id, "certuvo_start", $"{mode} x{qids.Count}{(domain is null ? "" : " · " + domain)}");
            return J(new { attempt_id = attemptId, mode, domain, count = qids.Count, duration_minutes = durationMinutes, questions });
        });

        // ---------------- submit + grade ----------------
        app.MapPost("/api/me/certuvo/submit", async (HttpContext ctx) =>
        {
            var deny = Gate(ctx, out var u); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var attemptId = (long)(H.GetNum(b, "attempt_id") ?? 0);
            var att = db.QueryOne("SELECT * FROM practice_attempts WHERE id=? AND user_id=?", attemptId, u!.Id);
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(att["status"]) == "completed") return Results.Json(new { error = "already_submitted" }, statusCode: 409);

            var qids = new List<long>();
            try { qids = JsonSerializer.Deserialize<List<long>>(H.Str(att["question_ids"]) ?? "[]") ?? new(); } catch { }
            if (qids.Count == 0) return Results.Json(new { error = "empty_attempt" }, statusCode: 400);

            // Load the served questions WITH their answer key (only now, server-side).
            var ph = string.Join(",", qids.Select(_ => "?"));
            var rows = db.Query($"SELECT id,question,options,option_a,option_b,option_c,option_d,domain,answer_index,explanation FROM sample_questions WHERE id IN ({ph})", qids.Cast<object?>().ToArray());
            var byId = rows.ToDictionary(r => H.L(r["id"]), r => r);

            var answersEl = H.GetEl(b, "answers");
            int? Chosen(long qid)
            {
                if (answersEl is { ValueKind: JsonValueKind.Object } o && o.TryGetProperty(qid.ToString(), out var v))
                {
                    if (v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out var n)) return n;
                    if (v.ValueKind == JsonValueKind.String && int.TryParse(v.GetString(), out var n2)) return n2;
                }
                return null;
            }

            long score = 0; var total = qids.Count;
            var breakdown = new Dictionary<string, (long correct, long total)>();
            var results = new List<object>();
            foreach (var qid in qids)
            {
                if (!byId.TryGetValue(qid, out var r)) continue;
                var dom = H.Str(r["domain"]) ?? "General";
                var correctIdx = (int)H.L(r["answer_index"]);
                var chosen = Chosen(qid);
                var ok = chosen is not null && chosen == correctIdx;
                if (ok) score++;
                var cur = breakdown.GetValueOrDefault(dom);
                breakdown[dom] = (cur.correct + (ok ? 1 : 0), cur.total + 1);
                results.Add(new
                {
                    id = qid, question = H.Str(r["question"]), options = H.OptionsFor(r), domain = dom,
                    chosen, correct_index = correctIdx, is_correct = ok, explanation = H.Str(r["explanation"]),
                });
            }

            var bd = breakdown.ToDictionary(kv => kv.Key, kv => new { correct = kv.Value.correct, total = kv.Value.total });
            var durationSeconds = (long?)(H.GetNum(b, "duration_seconds"));
            var answersRaw = answersEl is { ValueKind: JsonValueKind.Object } aeObj ? aeObj.GetRawText() : "{}";  // already JSON — store verbatim
            db.Execute("UPDATE practice_attempts SET status='completed', answers=?, score=?, total=?, domain_breakdown=?, duration_seconds=?, completed_at=datetime('now') WHERE id=?",
                answersRaw, score, total, JsonSerializer.Serialize(bd), durationSeconds, attemptId);
            // First-party analytics (Phase 5): a practice completion is a real engagement event.
            Analytics.Track(db, ctx, "practice_completed", u.Id, score, null, H.Str(att["mode"]));
            log(u.Id, "certuvo_submit", $"{H.Str(att["mode"])} {score}/{total}");

            var pct = total > 0 ? (int)Math.Round(100.0 * score / total) : 0;
            return J(new
            {
                ok = true, score, total, pct, mode = H.Str(att["mode"]),
                domain_breakdown = bd.Select(kv => new { domain = kv.Key, correct = kv.Value.correct, total = kv.Value.total,
                    pct = kv.Value.total > 0 ? (int)Math.Round(100.0 * kv.Value.correct / kv.Value.total) : 0 }),
                results,
            });
        });

        // ---------------- history ----------------
        app.MapGet("/api/me/certuvo/history", (HttpContext ctx) =>
        {
            var deny = Gate(ctx, out var u); if (deny is not null) return deny;
            var rows = db.Query(@"SELECT id,mode,domain,score,total,duration_seconds,completed_at
                                  FROM practice_attempts WHERE user_id=? AND status='completed' ORDER BY id DESC LIMIT 50", u!.Id)
                .Select(r => new
                {
                    id = H.L(r["id"]), mode = H.Str(r["mode"]), domain = H.Str(r["domain"]),
                    score = H.L(r["score"]), total = H.L(r["total"]),
                    pct = H.L(r["total"]) > 0 ? (int)Math.Round(100.0 * H.L(r["score"]) / H.L(r["total"])) : 0,
                    completed_at = H.Str(r["completed_at"]),
                });
            return J(new { rows });
        });
    }
}
