using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>Secure-exam desktop client pipeline: authorize (create-or-resume), evidence, identity.</summary>
public static class ExamClient
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        UserCtx? Auth(HttpContext c) => Core.Auth.UserFromReq(c.Request, db);
        IResult J(object o) => Results.Json(o);

        app.MapPost("/api/exam/authorize", async (HttpContext ctx) =>
        {
            // The desktop client authenticates the SITTING with the single-use launch CODE (not a
            // pre-existing browser session). We resolve the user FROM the code, then mint a short-lived
            // exam session token the client uses for heartbeat/submit/evidence/identity. A Bearer session
            // is also accepted (e.g. browser-launched) and must match the code's owner.
            var b = await H.Body(ctx.Request);
            var code = H.GetS(b, "code", "Code") ?? "";
            var lc = db.QueryOne("SELECT * FROM exam_launch_codes WHERE code_hash=?", Security.Sha(code));
            if (lc is null) return Results.Json(new { error = "invalid_code" }, statusCode: 401);
            // Compare expiry numerically. expires_at is stored via SQLite datetime('now','+15 minutes')
            // as "YYYY-MM-DD HH:MM:SS" (space), whereas H.IsoNow is ISO-8601 with a 'T'. A lexical
            // string compare put the space (0x20) before 'T' (0x54), so within the same day EVERY code
            // read as already-expired — the desktop launch could never authorize. JsMillis parses the
            // SQLite format robustly, matching how booking/start timing is already done.
            if (lc["expires_at"] is not null && H.JsMillis(H.Str(lc["expires_at"])) < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                return Results.Json(new { error = "code_expired" }, statusCode: 401);
            var sessionUser = Auth(ctx);
            if (sessionUser is not null && sessionUser.Id != H.L(lc["user_id"]))
                return Results.Json(new { error = "code_user_mismatch" }, statusCode: 403);
            var u = db.QueryOne("SELECT * FROM users WHERE id=? AND status='active'", lc["user_id"]);
            if (u is null) return Results.Json(new { error = "user_inactive" }, statusCode: 403);
            var uid = H.L(u["id"]);
            var bk = db.QueryOne("SELECT * FROM exam_bookings WHERE id=?", lc["booking_id"]);
            if (bk is null) return Results.Json(new { error = "no_booking" }, statusCode: 400);

            var att = db.QueryOne("SELECT * FROM exam_attempts WHERE user_id=? AND booking_id=? AND kind='exam' ORDER BY id DESC", uid, lc["booking_id"]);
            if (att is not null && (att["status"] as string) == "submitted") return Results.Json(new { error = "already_submitted" }, statusCode: 400);
            if (lc["redeemed_at"] is not null && !(att is not null && (att["status"] as string) == "in_progress"))
                return Results.Json(new { error = "code_used" }, statusCode: 401);

            // ── STRICT LAUNCH TIMING (parity with browser start) ──
            // Enforce the scheduled window on the SERVER before an attempt can be created or resumed.
            var cfg = H.Cfg(db);
            var slotMs = H.JsMillis(H.Str(bk["scheduled_at"]));
            var nowMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var resuming = att is not null && (att["status"] as string) == "in_progress";
            if (!resuming)
            {
                if (nowMs < slotMs - cfg.OpenBefore * 60_000)
                    return Results.Json(new { error = "not_open", OpensAt = H.IsoFromMillis((long)(slotMs - cfg.OpenBefore * 60_000)) }, statusCode: 400);
                if (nowMs > slotMs + cfg.Grace * 60_000)
                {
                    db.Execute("UPDATE exam_bookings SET status='missed', updated_at=datetime('now') WHERE id=? AND status='scheduled'", bk["id"]);
                    return Results.Json(new { error = "missed" }, statusCode: 400);
                }
            }

            // Live exam bank ONLY — never practice items, never the answer key.
            var items = db.Query("SELECT id,domain,question,options,option_a,option_b,option_c,option_d FROM sample_questions WHERE published=1 AND is_practice=0 ORDER BY sort_order,id")
                .Select(it => new { Id = it["id"], Domain = it["domain"], Question = it["question"], Options = H.OptionsFor(it) }).ToList();

            if (att is null)
            {
                if (!Lifecycle.ReadinessSatisfied(db, uid)) return Results.Json(new { error = "readiness_required" }, statusCode: 400);
                var dur = cfg.Duration + Lifecycle.ApprovedExtraMinutes(db, uid); // approved accommodations apply on desktop too
                var itemIds = JsonSerializer.Serialize(items.Select(i => i.Id));
                // status set explicitly (see StudentExam note) so the desktop attempt is submittable.
                var id = db.ExecuteReturningId("INSERT INTO exam_attempts(user_id,booking_id,kind,duration_minutes,item_ids,client_kind,status) VALUES(?,?,?,?,?,'desktop','in_progress')", uid, lc["booking_id"], "exam", dur, itemIds);
                att = db.QueryOne("SELECT * FROM exam_attempts WHERE id=?", id);
                log(uid, "exam_started", $"attempt {id} (desktop)");
            }
            db.Execute("UPDATE exam_launch_codes SET redeemed_at=COALESCE(redeemed_at,datetime('now')), attempt_id=? WHERE id=?", att!["id"], lc["id"]);

            // Mint a short-lived exam SESSION token bound to this candidate so the client can call the
            // session-protected endpoints (heartbeat/submit/evidence/identity). Stored hashed; lifetime
            // covers the exam window. Reuse an existing live exam session if one is already active.
            var examSession = Security.RandomHex(32);
            db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+6 hours'))", uid, Security.Sha(examSession));

            var deadline = H.JsMillis(H.Str(att["started_at"])) + H.L(att["duration_minutes"]) * 60_000;
            var savedAnswers = new Dictionary<string, JsonElement>();
            try { savedAnswers = H.ToMap(JsonDocument.Parse(H.Str(att["answers"]) ?? "{}").RootElement); } catch { }
            var candidateName = ($"{H.Str(u["first_name"])} {H.Str(u["last_name"])}").Trim();
            var candidateEmail = H.Str(u["email"]);
            return J(new
            {
                SessionToken = examSession, session_token = examSession,
                AttemptToken = code, CandidateName = candidateName, CandidateEmail = candidateEmail,
                ExamCode = "PCP-AI", ExamTitle = "PCP-AI Examination", DurationMinutes = att["duration_minutes"],
                RemainingSeconds = Math.Max(0, (int)((deadline - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 1000)),
                SavedAnswers = savedAnswers, Resumed = !string.IsNullOrEmpty(H.Str(att["answers"])),
                ScheduledAt = H.IsoFromMillis(slotMs), OpensAt = H.IsoFromMillis(slotMs - 15 * 60_000), ClosesAt = H.IsoFromMillis(slotMs + 30 * 60_000),
                RequiresIdentity = cfg.RequireIdentity, RequiresRoomScan = cfg.RequireRoomScan, Items = items
            });
        });

        app.MapPost("/api/exam/evidence", async (HttpContext ctx) =>
        {
            var u = Auth(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var token = H.GetEl(b, "attemptToken", "attempt_id", "AttemptToken");
            object? tk = token?.ValueKind == JsonValueKind.Number ? token.Value.GetInt64() : token?.GetString();
            var att = H.AttemptForToken(db, tk, u.Id);
            if (att is null) return Results.Json(new { error = "no_attempt" }, statusCode: 400);
            var kind = H.GetS(b, "kind") ?? "frame";
            var dataUri = H.GetS(b, "data_uri", "dataUri");
            // Store the frame bytes in blob storage and keep only a reference + metadata in the DB.
            // This prevents the database from accumulating multi-megabyte base64 blobs per attempt.
            string? storageRef = null, mime = null, sha = null; long size = 0;
            if (!string.IsNullOrEmpty(dataUri))
            {
                var (bytes, m, err) = Storage.DecodeDataUri(dataUri);
                if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
                var obj = Storage.Put(bytes!, m, "evidence");
                storageRef = obj.Reference; mime = obj.Mime; size = obj.SizeBytes; sha = obj.Sha256;
            }
            db.Execute("INSERT INTO exam_evidence(attempt_id,user_id,kind,mime,storage_ref,size_bytes,sha256,note) VALUES(?,?,?,?,?,?,?,?)",
                att["id"], u.Id, kind, mime ?? "image/jpeg", storageRef, size, sha, H.GetS(b, "note"));
            db.Execute("UPDATE exam_attempts SET evidence_count=COALESCE(evidence_count,0)+1 WHERE id=?", att["id"]);
            return J(new { ok = true });
        });

        app.MapPost("/api/exam/identity", async (HttpContext ctx) =>
        {
            var u = Auth(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var token = H.GetEl(b, "attemptToken", "attempt_id", "AttemptToken");
            object? tk = token?.ValueKind == JsonValueKind.Number ? token.Value.GetInt64() : token?.GetString();
            var att = H.AttemptForToken(db, tk, u.Id);
            if (att is null) return Results.Json(new { error = "no_attempt" }, statusCode: 400);
            var checkEl = H.GetEl(b, "check", "Check");
            var c = checkEl is { ValueKind: JsonValueKind.Object } ? H.ToMap(checkEl.Value) : b;
            var result = H.GetS(c, "Result", "result") ?? "Inconclusive";
            var conf = H.GetNum(c, "Confidence", "confidence") ?? 0;
            db.Execute("INSERT INTO identity_checks(attempt_id,user_id,result,confidence,note,face_ref,id_ref) VALUES(?,?,?,?,?,?,?)",
                att["id"], u.Id, result, conf, H.GetS(c, "Note", "note"), H.GetS(c, "FacePhotoRef", "facePhotoRef"), H.GetS(c, "IdPhotoRef", "idPhotoRef"));
            db.Execute("UPDATE exam_attempts SET identity_result=?, identity_confidence=? WHERE id=?", result, conf, att["id"]);
            return J(new { ok = true });
        });
    }
}
