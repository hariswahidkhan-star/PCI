using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

public static class StudentExam
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        // require a student session; returns null + writes 401 if absent
        UserCtx? Auth401(HttpContext ctx)
        {
            var u = Core.Auth.UserFromReq(ctx.Request, db);
            if (u is null) { ctx.Response.StatusCode = 401; }
            return u;
        }
        IResult J(object o) => Results.Json(o);

        // A paid exam/bundle payment is the entitlement proxy. certification_id comes from the
        // formal exam_entitlements ledger; legacy rows without one belong to certification 1.
        Dictionary<string, object?>? ExamEntitlement(long uid, long? certId = null) =>
            certId is null
            ? db.QueryOne("SELECT p.*, COALESCE(e.certification_id,1) certification_id FROM payments p LEFT JOIN exam_entitlements e ON e.payment_id=p.id WHERE p.user_id=? AND p.payment_status='paid' AND p.product_type IN ('exam','bundle') ORDER BY p.id DESC", uid)
            : db.QueryOne("SELECT p.*, COALESCE(e.certification_id,1) certification_id FROM payments p LEFT JOIN exam_entitlements e ON e.payment_id=p.id WHERE p.user_id=? AND p.payment_status='paid' AND p.product_type IN ('exam','bundle') AND COALESCE(e.certification_id,1)=? ORDER BY p.id DESC", uid, certId);
        // Bookings are per certification: holding a slot for one credential must not block another.
        Dictionary<string, object?>? ActiveBooking(long uid, long? certId = null) =>
            certId is null
            ? db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", uid)
            : db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND status='scheduled' AND COALESCE(certification_id,1)=? ORDER BY id DESC", uid, certId);

        // ---------------- GET /api/me ----------------
        app.MapGet("/api/me", (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var ent = ExamEntitlement(u.Id);
            var booking = ActiveBooking(u.Id);
            var passAtt = db.QueryOne("SELECT * FROM exam_attempts WHERE user_id=? AND kind='exam' AND result='pass' ORDER BY id DESC", u.Id);
            var cpdRows = db.Query("SELECT hours FROM cpd_entries WHERE user_id=?", u.Id);
            var total = cpdRows.Sum(r => H.D(r["hours"]));
            var unread = db.Scalar<long>("SELECT COUNT(*) FROM notifications WHERE user_id=? AND read_at IS NULL", u.Id);
            // Ensure a stable registration number exists (lazy backfill for both new and pre-existing users).
            var regNo = db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", u.Id);
            if (string.IsNullOrWhiteSpace(regNo))
            {
                regNo = $"PCI-{DateTime.UtcNow:yyyy}-{u.Id:D6}";
                db.Execute("UPDATE users SET registration_no=? WHERE id=? AND (registration_no IS NULL OR registration_no='')", regNo, u.Id);
            }
            return J(new
            {
                user = new { id = u.Id, email = u.Email, first_name = u.FirstName, last_name = u.LastName, registration_no = regNo, created_at = db.Scalar<string>("SELECT created_at FROM users WHERE id=?", u.Id) },
                profile = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", u.Id),
                lifecycle = Lifecycle.BuildLifecycle(db, u.Id,
                    db.QueryOne("SELECT * FROM memberships WHERE user_id=? ORDER BY id DESC", u.Id),
                    ent, booking,
                    db.QueryOne("SELECT * FROM exam_attempts WHERE user_id=? AND kind='exam' ORDER BY id DESC", u.Id),
                    db.QueryOne("SELECT * FROM issued_credentials WHERE user_id=? ORDER BY id DESC", u.Id),
                    Lifecycle.BookingBlockers(db, u.Id, ent, db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", u.Id))),
                consents = new {
                    required = Lifecycle.RequiredConsents.Select(c => new { type = c.type, version = c.version }),
                    outstanding = Lifecycle.OutstandingConsents(db, u.Id)
                },
                membership = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", u.Id),
                payments = db.Query("SELECT id,product_type,final_amount,currency,payment_status,payment_date,reference,exam_schedule_deadline FROM payments WHERE user_id=? ORDER BY id DESC", u.Id),
                exam = new { entitled = ent != null, deadline = ent?["exam_schedule_deadline"], payment_ref = ent?["reference"], booking, passed = passAtt != null,
                    certification_id = ent is null ? null : ent["certification_id"],
                    certification = ent is null ? null : (object?)Certs.ById(db, ent["certification_id"])?["name"] },
                // Multi-certification view: one entry per paid entitlement, each with its own
                // certification, booking, latest attempt and credential. The legacy `exam` object
                // above remains for existing UI paths (it reflects the most recent entitlement).
                exams = db.Query(@"SELECT p.id payment_id, p.reference, p.exam_schedule_deadline, p.payment_status,
                        COALESCE(e.certification_id,1) certification_id, e.status entitlement_status
                        FROM payments p LEFT JOIN exam_entitlements e ON e.payment_id=p.id
                        WHERE p.user_id=? AND p.payment_status='paid' AND p.product_type IN ('exam','bundle') ORDER BY p.id DESC", u.Id)
                    .Select(r => {
                        var cid = H.L(r["certification_id"]);
                        var cert = Certs.ById(db, cid);
                        var bk = db.QueryOne("SELECT id,scheduled_at,timezone,status FROM exam_bookings WHERE user_id=? AND payment_id=? ORDER BY id DESC", u.Id, r["payment_id"]);
                        var att = db.QueryOne("SELECT id,status,result_status,submitted_at FROM exam_attempts WHERE user_id=? AND kind='exam' AND COALESCE(certification_id,1)=? ORDER BY id DESC", u.Id, cid);
                        var cred = db.QueryOne("SELECT credential_id,status,expires_at FROM issued_credentials WHERE user_id=? AND COALESCE(certification_id,1)=? AND status='active' ORDER BY id DESC", u.Id, cid);
                        return new { certification_id = cid, certification_code = cert?["code"], certification_name = cert?["name"],
                            payment_id = r["payment_id"], reference = r["reference"], deadline = r["exam_schedule_deadline"],
                            entitlement_status = r["entitlement_status"], booking = bk, latest_attempt = att, credential = cred };
                    }).ToList(),
                // Held attempts must not disclose pass/fail/score until released (Section A rule 6) —
                // redacted SERVER-side, not merely hidden by the front-end.
                attempts = db.Query("SELECT id,kind,started_at,submitted_at,percent,result,status,result_status,hold_reason,released_at,domain_breakdown,violations,duration_minutes FROM exam_attempts WHERE user_id=? ORDER BY id DESC LIMIT 25", u.Id)
                    .Select(a => {
                        if (H.Str(a["result_status"]) == "auto_held") { a["percent"] = null; a["result"] = null; a["domain_breakdown"] = null; }
                        return a;
                    }).ToList(),
                credentials = db.Query("SELECT credential_id,credential,status,issued_at,expires_at,holder_name FROM issued_credentials WHERE user_id=? ORDER BY id DESC", u.Id),
                tickets = db.Query("SELECT id,reference,subject,category,status,updated_at FROM tickets WHERE user_id=? ORDER BY updated_at DESC LIMIT 10", u.Id),
                referral = db.QueryOne("SELECT code FROM discount_codes WHERE owner_user_id=? AND code_type='referral' AND active=1", u.Id),
                cpd = new { total, target = 60 },
                two_factor = false, two_factor_coming_soon = true,
                unread,
                enrollment = db.QueryOne("SELECT current_step,selected_product,last_activity_at FROM enrollment_sessions WHERE email=? AND session_status='in_progress' ORDER BY id DESC", u.Email.ToLowerInvariant()),
                site_base_url = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='site_base_url'") ?? ""
            });
        });

        // ---------------- PATCH /api/me/profile ----------------
        app.MapPatch("/api/me/profile", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var allowed = new[]{ "mobile","country","city","preferred_language","current_role","company","industry_sector","years_experience","highest_qualification","project_controls_area","enrollment_purpose","linkedin_url","profile_photo" };
            if (db.QueryOne("SELECT user_id FROM student_profiles WHERE user_id=?", u.Id) is null)
                db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", u.Id);
            var set = allowed.Where(k => b.ContainsKey(k)).ToList();
            if (set.Count > 0)
            {
                var vals = set.Select(k => (object?)(H.GetS(b, k) ?? "")).Append(u.Id).ToArray();
                db.Execute($"UPDATE student_profiles SET {string.Join(",", set.Select(k => k + "=?"))} WHERE user_id=?", vals);
            }
            log(u.Id, "profile_update", string.Join(",", set));
            return J(new { ok = true });
        });

        app.MapGet("/api/me/downloads", (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { rows = db.Query("SELECT title,category,doc_type,url FROM resources WHERE published=1 ORDER BY sort_order,id") });
        });

        // ── Candidate consents (Section B rule 3) ──
        app.MapGet("/api/me/consents", (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new
            {
                required = Lifecycle.RequiredConsents.Select(c => new { type = c.type, version = c.version }),
                accepted = db.Query("SELECT consent_type, policy_version, accepted_at FROM candidate_consents WHERE user_id=? ORDER BY id DESC", u.Id),
                outstanding = Lifecycle.OutstandingConsents(db, u.Id)
            });
        });
        app.MapPost("/api/me/consents", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            // Accept either a single {consent_type,policy_version} or {accept:[...types]} against current versions.
            var accepted = new List<string>();
            var ip = ctx.Connection.RemoteIpAddress?.ToString();
            var ua = ctx.Request.Headers["User-Agent"].ToString();
            void Accept(string type)
            {
                var ver = Lifecycle.RequiredConsents.FirstOrDefault(c => c.type == type).version;
                if (ver is null) return;
                db.Execute("INSERT INTO candidate_consents(user_id,consent_type,policy_version,ip_address,user_agent) VALUES(?,?,?,?,?)", u.Id, type, ver, ip, ua);
                accepted.Add(type);
            }
            var single = H.GetS(b, "consent_type");
            if (single is not null) Accept(single);
            var arr = H.GetEl(b, "accept", "types");
            if (arr is { ValueKind: JsonValueKind.Array })
                foreach (var el in arr.Value.EnumerateArray()) if (el.ValueKind == JsonValueKind.String) Accept(el.GetString()!);
            if (b.TryGetValue("accept_all", out var allEl) && allEl.ValueKind == JsonValueKind.True)
                foreach (var c in Lifecycle.RequiredConsents) Accept(c.type);
            log(u.Id, "consents_accepted", string.Join(",", accepted));
            return J(new { ok = true, accepted, outstanding = Lifecycle.OutstandingConsents(db, u.Id) });
        });

        // ── Downloadable score report (Section B rule 4) ──
        app.MapGet("/api/me/results/{attemptId}/report", (HttpContext ctx, long attemptId) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!Settings.Bool(db, "sp_results_visible", true)) return Results.Json(new { error = "results_hidden", message = "Results are temporarily unavailable. Please check back shortly." }, statusCode: 403);
            var att = db.QueryOne("SELECT * FROM exam_attempts WHERE id=? AND user_id=?", attemptId, u.Id);
            if (att is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var rs = H.Str(att["result_status"]);
            // Do not disclose pass/fail for a held attempt.
            if (rs is "auto_held" or "in_progress" or "not_started" or "")
                return J(new { held = rs == "auto_held", status = rs, message = rs == "auto_held" ? "Result on hold pending integrity review." : "Result not yet available." });
            var snap = db.QueryOne("SELECT * FROM exam_score_snapshots WHERE attempt_id=?", attemptId);
            return J(new
            {
                attempt_id = attemptId,
                result = att["result"], percent = att["percent"], score = att["score"], max_score = att["max_score"],
                result_status = rs,
                domain_breakdown = TryJson(H.Str(att["domain_breakdown"])),
                unanswered = snap?["unanswered"], flagged_events = snap?["flagged_events"],
                duration_seconds = snap?["duration_seconds"], submitted_at = att["submitted_at"], released_at = att["released_at"],
                registration_no = db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", u.Id)
            });
        });

        app.MapGet("/api/me/practice", (HttpContext ctx) =>
        {
            if (!Settings.Bool(db, "sp_practice_enabled", true)) return Results.Json(new { error = "practice_disabled" }, statusCode: 403);
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            // Practice pool is SEPARATE from the live exam bank (is_practice=1) and NEVER includes the answer key.
            var rows = db.Query("SELECT id,question,options,option_a,option_b,option_c,option_d,domain FROM sample_questions WHERE published=1 AND is_practice=1 ORDER BY sort_order,id")
                .Select(r => new { id = r["id"], question = r["question"], options = H.OptionsFor(r), domain = r["domain"] });
            return J(new { rows });
        });

        // ---------------- exam booking (per certification) ----------------
        app.MapPost("/api/me/exam/book", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            // Which credential is being booked: explicit certification (id or code) or, for
            // single-certification candidates, the one their entitlement belongs to.
            var certId = Certs.Resolve(db, H.GetS(b, "certification_id", "certification", "cert"));
            var ent = ExamEntitlement(u.Id, certId) ?? (H.GetS(b, "certification_id", "certification", "cert") is null ? ExamEntitlement(u.Id) : null);
            if (ent is null) return Results.Json(new { error = "no_entitlement" }, statusCode: 400);
            certId = H.L(ent["certification_id"]);
            // Uniform eligibility gate (consents, profile, payment validity, holds) — same rules as launch.
            var blockers = Lifecycle.BookingBlockers(db, u.Id, ent, db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", u.Id));
            if (blockers.Count > 0) return Results.Json(new { error = "not_eligible", blocking_items = blockers }, statusCode: 400);
            var deadline = H.Str(ent["exam_schedule_deadline"]);
            if (H.IsPast(deadline)) return Results.Json(new { error = "window_lapsed" }, statusCode: 400);
            // "already booked" is scoped to THIS certification — a slot for one credential must not
            // block booking a different credential the candidate has also paid for.
            if (ActiveBooking(u.Id, certId) is not null) return Results.Json(new { error = "already_booked" }, statusCode: 400);
            // #5 — one exam payment yields at most one live sitting. If this payment already has any
            // booking that is scheduled or completed (or a submitted attempt), a new booking is refused;
            // rescheduling must go through /reschedule, and a fresh sitting requires a fresh payment.
            var priorForPayment = db.QueryOne("SELECT id FROM exam_bookings WHERE payment_id=? AND status IN ('scheduled','completed') ORDER BY id DESC", ent["id"]);
            if (priorForPayment is not null) return Results.Json(new { error = "payment_already_used" }, statusCode: 400);
            var submittedForPayment = db.QueryOne(@"SELECT a.id FROM exam_attempts a JOIN exam_bookings bk ON bk.id=a.booking_id WHERE bk.payment_id=? AND a.kind='exam' AND a.status='submitted' LIMIT 1", ent["id"]);
            if (submittedForPayment is not null) return Results.Json(new { error = "exam_already_taken" }, statusCode: 400);
            var scheduledAt = H.GetS(b, "scheduled_at");
            var timezone = H.GetS(b, "timezone");
            if (scheduledAt is null || H.JsMillis(scheduledAt) < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 2 * 3600_000)
                return Results.Json(new { error = "bad_slot" }, statusCode: 400);
            if (deadline is not null && H.After(scheduledAt, deadline)) return Results.Json(new { error = "beyond_window" }, statusCode: 400);
            var id = db.ExecuteReturningId("INSERT INTO exam_bookings(user_id,payment_id,certification_id,scheduled_at,timezone) VALUES(?,?,?,?,?)", u.Id, ent["id"], certId, scheduledAt, timezone);
            // Link the formal entitlement to this booking and mark it booked, so submit can consume it
            // by booking_id (one-attempt-per-entitlement).
            db.Execute("UPDATE exam_entitlements SET status='booked', booking_id=? WHERE payment_id=? AND status IN ('available','booked')", id, ent["id"]);
            log(u.Id, "exam_booked", $"{scheduledAt} (cert {certId})");
            return J(new { ok = true, id, scheduled_at = scheduledAt, certification_id = certId });
        });

        app.MapPost("/api/me/exam/reschedule", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            // Optional certification scope for candidates holding bookings for several credentials;
            // default stays "the latest scheduled booking" for existing single-cert clients.
            var certSel = H.GetS(b, "certification_id", "certification", "cert");
            var bk = certSel is null ? ActiveBooking(u.Id) : ActiveBooking(u.Id, Certs.Resolve(db, certSel));
            if (bk is null) return Results.Json(new { error = "no_booking" }, statusCode: 400);
            var hoursTo = (H.JsMillis(H.Str(bk["scheduled_at"])) - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 3600_000.0;
            if (hoursTo < H.RESCHED_LOCK_H) return Results.Json(new { error = "locked", lock_hours = H.RESCHED_LOCK_H }, statusCode: 400);
            if (H.L(bk["reschedule_count"]) >= H.MAX_RESCHED) return Results.Json(new { error = "max_reschedules" }, statusCode: 400);
            var scheduledAt = H.GetS(b, "scheduled_at");
            var timezone = H.GetS(b, "timezone");
            if (scheduledAt is null || H.JsMillis(scheduledAt) < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 2 * 3600_000)
                return Results.Json(new { error = "bad_slot" }, statusCode: 400);
            var ent = ExamEntitlement(u.Id, H.L(bk.GetValueOrDefault("certification_id") ?? 1L));
            var dl = ent is null ? null : H.Str(ent["exam_schedule_deadline"]);
            if (dl is not null && H.After(scheduledAt, dl)) return Results.Json(new { error = "beyond_window" }, statusCode: 400);
            db.Execute("UPDATE exam_bookings SET scheduled_at=?, timezone=?, reschedule_count=reschedule_count+1, updated_at=datetime('now') WHERE id=?", scheduledAt, timezone ?? H.Str(bk["timezone"]), bk["id"]);
            log(u.Id, "exam_rescheduled", scheduledAt);
            return J(new { ok = true, free = hoursTo >= H.FREE_RESCHED_H, reschedule_count = H.L(bk["reschedule_count"]) + 1 });
        });

        // ---------------- exam start (create-or-resume; certification-aware) ----------------
        app.MapPost("/api/me/exam/start", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var certSel = H.GetS(b, "certification_id", "certification", "cert");
            var bk = certSel is null ? ActiveBooking(u.Id) : ActiveBooking(u.Id, Certs.Resolve(db, certSel));
            if (bk is null) return Results.Json(new { error = "no_booking" }, statusCode: 400);
            if (!Lifecycle.ReadinessSatisfied(db, u.Id)) return Results.Json(new { error = "readiness_required", message = "Please complete the system readiness check before launching your exam." }, statusCode: 400);
            var certId = H.L(bk.GetValueOrDefault("certification_id") ?? 1L);
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var slot = H.JsMillis(H.Str(bk["scheduled_at"]));
            var ec = Certs.Cfg(db, certId);   // duration/pass per certification; windows stay global
            if (now < slot - ec.OpenBefore * 60_000) return Results.Json(new { error = "not_open", opens_at = H.IsoFromMillis((long)(slot - ec.OpenBefore * 60_000)) }, statusCode: 400);
            if (now > slot + ec.Grace * 60_000) { db.Execute("UPDATE exam_bookings SET status='missed', updated_at=datetime('now') WHERE id=?", bk["id"]); return Results.Json(new { error = "missed" }, statusCode: 400); }
            var existing = db.QueryOne("SELECT * FROM exam_attempts WHERE user_id=? AND booking_id=? AND kind='exam'", u.Id, bk["id"]);
            // The live bank for THIS certification only — cross-certification leakage would both spoil
            // the sitting and break the item-set integrity check on submit.
            var items = db.Query("SELECT id,question,options,option_a,option_b,option_c,option_d,domain FROM sample_questions WHERE published=1 AND is_practice=0 AND COALESCE(certification_id,1)=? ORDER BY sort_order,id", certId)
                .Select(r => new { id = r["id"], question = r["question"], options = H.OptionsFor(r), domain = r["domain"] }).ToList();
            if (items.Count == 0) return Results.Json(new { error = "no_items", message = "This certification's examination bank is not yet published." }, statusCode: 400);
            if (existing is not null && (existing["status"] as string) == "in_progress")
            {
                var saved = new Dictionary<string, JsonElement>();
                try { saved = H.ToMap(JsonDocument.Parse(H.Str(existing["answers"]) ?? "{}").RootElement); } catch { }
                return J(new { attempt_id = existing["id"], duration_minutes = existing["duration_minutes"], started_at = existing["started_at"], items, saved_answers = saved, violations = H.L(existing["violations"]), resumed = true, certification_id = certId });
            }
            if (existing is not null) return Results.Json(new { error = "already_submitted" }, statusCode: 400);
            var itemIds = JsonSerializer.Serialize(items.Select(i => i.id));
            var durMin = ec.Duration + Lifecycle.ApprovedExtraMinutes(db, u.Id); // approved accommodations genuinely extend the sitting
            // status MUST be set explicitly to 'in_progress'; relying on a column default is fragile
            // (the schema default was historically mis-attached to bank_version, leaving status NULL,
            // which made submit reject every attempt as already_submitted).
            var id = db.ExecuteReturningId("INSERT INTO exam_attempts(user_id,booking_id,certification_id,kind,duration_minutes,item_ids,status) VALUES(?,?,?,?,?,?, 'in_progress')", u.Id, bk["id"], certId, "exam", durMin, itemIds);
            log(u.Id, "exam_started", $"attempt {id} (cert {certId})");
            return J(new { attempt_id = id, duration_minutes = durMin, started_at = H.IsoNow, items, certification_id = certId });
        });

        // ---------------- submit (scoring + credential) ----------------
        app.MapPost("/api/me/exam/submit", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var attemptId = H.GetEl(b, "attempt_id", "attemptToken", "AttemptToken");
            object? attIdObj = attemptId?.ValueKind == JsonValueKind.Number ? attemptId.Value.GetInt64() : attemptId?.GetString();
            var att = H.AttemptForToken(db, attIdObj, u.Id);
            if (att is not null && (att["kind"] as string) != "exam") att = null;
            if (att is null) return Results.Json(new { error = "no_attempt" }, statusCode: 404);
            if ((att["status"] as string) != "in_progress") return Results.Json(new { error = "already_submitted" }, statusCode: 400);
            // ── STRICT SERVER-SIDE TIMING ──
            // The server clock is authoritative. Answers are accepted only up to the hard deadline
            // (duration + a small network grace). A submission that arrives after the deadline is
            // finalised on the answers already persisted (via heartbeat) BEFORE the deadline — the
            // late payload is ignored, so a client cannot buy extra time by delaying submit.
            var nowMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var hardStop = H.JsMillis(H.Str(att["started_at"])) + (H.L(att["duration_minutes"]) + 1) * 60_000;
            var lateSubmit = nowMs > hardStop;
            Dictionary<long, long> answers;
            if (lateSubmit)
            {
                // ignore the late payload; use the last answers saved on the server before the deadline
                try { answers = ParseAnswers(JsonDocument.Parse(H.Str(att["answers"]) ?? "{}").RootElement); } catch { answers = new(); }
            }
            else
            {
                var answersEl2 = H.GetEl(b, "answers", "Answers");
                answers = ParseAnswers(answersEl2);
            }
            // Item-set integrity (technical invalidity): every answered item must be one the server issued
            // for THIS attempt. Answers referencing unknown items indicate a tampered/replayed payload.
            var issuedIds = new HashSet<long>();
            try { foreach (var x in JsonSerializer.Deserialize<List<long>>(H.Str(att["item_ids"]) ?? "[]") ?? new()) issuedIds.Add(x); } catch { }
            var itemSetMismatch = issuedIds.Count > 0 && answers.Keys.Any(k => !issuedIds.Contains(k));
            var r = ScoreAttempt(att, answers);
            // Guard the finalisation against a concurrent heartbeat auto-timeout: only this request may
            // move the attempt from in_progress→submitted. If 0 rows change, another path already
            // finalised it (e.g. auto_timeout) and we must NOT issue a credential.
            // ── Result publication: immediate by default; block ONLY on technical invalidity ──
            var holdReason = itemSetMismatch ? "item_set_mismatch" : Lifecycle.AutoHoldReason(db, att, u.Id, lateSubmit);
            var clean = holdReason is null;
            var resultStatus = Lifecycle.ReleaseStatus(clean, r.Result);
            // Unanswered + flagged counts for the score report.
            var itemIdsForCount = new List<long>();
            try { itemIdsForCount = JsonSerializer.Deserialize<List<long>>(H.Str(att["item_ids"]) ?? "[]") ?? new(); } catch { }
            var unanswered = Math.Max(0, itemIdsForCount.Count - answers.Count);
            var flagged = (int)db.Scalar<long>("SELECT COUNT(*) FROM proctor_events WHERE attempt_id=?", att["id"]);
            var durationSeconds = (int)Math.Max(0, (nowMs - H.JsMillis(H.Str(att["started_at"]))) / 1000);

            // Guard the finalisation against a concurrent heartbeat auto-timeout: only this request may
            // move the attempt from in_progress→submitted.
            var finalized = db.Execute(@"UPDATE exam_attempts SET submitted_at=datetime('now'), answers=?, score=?, max_score=?, percent=?, result=?, domain_breakdown=?, status='submitted', review_status=?, result_status=?, hold_reason=?, released_at=CASE WHEN ?='auto_held' THEN NULL ELSE datetime('now') END WHERE id=? AND status='in_progress'",
                JsonSerializer.Serialize(answers), r.Score, r.Max, r.Pct, r.Result, JsonSerializer.Serialize(r.Breakdown),
                clean ? "unreviewed" : "held", resultStatus, holdReason, resultStatus, att["id"]);
            if (finalized == 0) return Results.Json(new { error = "already_submitted" }, statusCode: 400);
            db.Execute("UPDATE exam_bookings SET status='completed', updated_at=datetime('now') WHERE id=?", att["booking_id"]);
            // Immutable score snapshot — protects the result even if questions are later edited.
            Lifecycle.WriteScoreSnapshot(db, att, r.Score, r.Max, r.Pct, r.Result, JsonSerializer.Serialize(r.Breakdown), unanswered, flagged, durationSeconds);
            // Mark the entitlement consumed (formal one-attempt-per-entitlement).
            if (att["booking_id"] is not null)
                db.Execute("UPDATE exam_entitlements SET status='consumed', attempt_id=? WHERE booking_id=? AND status IN ('available','booked')", att["id"], att["booking_id"]);

            string? credential = null;
            // Auto-issue a credential ONLY for a clean pass; held or late attempts issue nothing.
            if (r.Result == "pass" && clean)
            {
                credential = Lifecycle.IssueCredential(db, u.Id, att["id"], ($"{u.FirstName} {u.LastName}").Trim(), H.L(att.GetValueOrDefault("certification_id") ?? 1L));
                if (credential is not null)
                {
                    db.Execute("UPDATE exam_attempts SET result_status='credential_issued' WHERE id=?", att["id"]);
                    resultStatus = "credential_issued";
                }
                log(u.Id, "credential_issued", credential ?? "gen_failed");
            }
            log(u.Id, clean ? "result_released" : "result_held", $"{resultStatus} {r.Pct}% {holdReason}");
            log(u.Id, "exam_submitted", $"{r.Result} {r.Pct}%");
            var ck = H.GetS(b, "client_kind", "clientKind", "ClientKind");
            if (ck is not null) { try { db.Execute("UPDATE exam_attempts SET client_kind=? WHERE id=?", ck, att["id"]); } catch { } }
            // One key per logical field. Browser reads lowercase/snake_case; the desktop client
            // deserializes case-insensitively, so it binds Ok/Result/Breakdown/Held from the lowercase
            // keys and Percent/CredentialId/ResultStatus from the distinctly-named camelCase aliases.
            // (The previous dual-cased objects — ok+Ok, result+Result — collided under ASP.NET's
            // camelCase policy and 500'd on EVERY submit, and would also have tripped the desktop's
            // case-insensitive duplicate-key check.)
            // Held attempts must NOT reveal pass/fail to the candidate until released.
            if (!clean)
                return J(new { ok = true, held = true, result_status = resultStatus, resultStatus,
                    hold_reason = holdReason,
                    message = "Your result has been submitted and is currently on hold due to an examination integrity check. PCI will notify you once the review is complete." });
            return J(new { ok = true, held = false, result_status = resultStatus, resultStatus,
                score = r.Score, max = r.Max, pct = r.Pct, percent = r.Pct, result = r.Result,
                breakdown = r.Breakdown, credential, credentialId = credential,
                unanswered, flagged_events = flagged, late_submit = lateSubmit });
        });

        app.MapGet("/api/me/attempts/{id}", (HttpContext ctx, long id) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var a = db.QueryOne("SELECT * FROM exam_attempts WHERE id=? AND user_id=?", id, u.Id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            // A held attempt must not reveal pass/fail/score anywhere — same server-side redaction as
            // /api/me and the score report. Without this, GET /api/me/attempts/{id} returned the raw row
            // (percent, result, domain_breakdown) for an auto_held attempt.
            var redactHeld = H.Str(a["result_status"]) == "auto_held";
            object? bd = null; try { bd = JsonSerializer.Deserialize<object>(H.Str(a["domain_breakdown"]) ?? "[]"); } catch { }
            var copy = new Dictionary<string, object?>(a) { ["domain_breakdown"] = redactHeld ? null : bd };
            if (redactHeld) { copy["percent"] = null; copy["result"] = null; copy["score"] = null; copy["max_score"] = null; }
            return J(copy);
        });

        // ---------------- heartbeat (dual-case, chat, proctor messages) ----------------
        app.MapPost("/api/me/exam/heartbeat", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var attemptId = H.GetEl(b, "attempt_id", "attemptToken", "AttemptToken");
            object? attIdObj = attemptId?.ValueKind == JsonValueKind.Number ? attemptId.Value.GetInt64() : attemptId?.GetString();
            var att = H.AttemptForToken(db, attIdObj, u.Id);
            if (att is not null && (att["kind"] as string) != "exam") att = null;
            if (att is null || (att["status"] as string) != "in_progress") return Results.Json(new { error = "not_active" }, statusCode: 400);
            var idStr = attIdObj?.ToString() ?? "";
            if (!System.Text.RegularExpressions.Regex.IsMatch(idStr, @"^\d+$") && (att["client_kind"] as string) != "desktop")
                { try { db.Execute("UPDATE exam_attempts SET client_kind='desktop' WHERE id=?", att["id"]); } catch { } }
            var deadline = H.JsMillis(H.Str(att["started_at"])) + H.L(att["duration_minutes"]) * 60_000;
            var answersEl = H.GetEl(b, "answers", "Answers");
            if (answersEl is not null) db.Execute("UPDATE exam_attempts SET answers=? WHERE id=?", answersEl.Value.GetRawText(), att["id"]);
            var violations = H.GetNum(b, "violations", "Violations");
            if (violations is not null) db.Execute("UPDATE exam_attempts SET violations=? WHERE id=?", Math.Max(H.L(att["violations"]), (long)violations.Value), att["id"]);
            var eventsEl = H.GetEl(b, "events", "PendingEvents", "pending_events");
            if (eventsEl is { ValueKind: JsonValueKind.Array })
                foreach (var e in eventsEl.Value.EnumerateArray())
                {
                    var em = H.ToMap(e);
                    var type = H.GetS(em, "Type", "type") ?? "Unknown";
                    var sev = H.GetS(em, "Severity", "severity") ?? "Info";
                    var detail = H.GetS(em, "Detail", "detail");
                    var reff = H.GetS(em, "EvidenceRef", "evidence_ref");
                    var at = H.GetS(em, "At", "at") ?? H.IsoNow;
                    try { db.Execute("INSERT INTO proctor_events(attempt_id,user_id,type,severity,detail,evidence_ref,at) VALUES(?,?,?,?,?,?,?)", att["id"], u.Id, type, sev, detail, reff, at); } catch { }
                }
            var chatEl = H.GetEl(b, "chat_out", "ChatOut");
            if (chatEl is { ValueKind: JsonValueKind.Array })
                foreach (var m in chatEl.Value.EnumerateArray())
                {
                    var body = (m.ValueKind == JsonValueKind.String ? m.GetString() : m.GetRawText()) ?? "";
                    if (body.Length > 1000) body = body[..1000];
                    if (body.Length > 0) { try { db.Execute("INSERT INTO proctor_messages(attempt_id,user_id,sender,body,delivered_at) VALUES(?,?,?,?,datetime('now'))", att["id"], u.Id, "candidate", body); } catch { } }
                }
            db.Execute("UPDATE exam_attempts SET last_heartbeat_at=datetime('now') WHERE id=?", att["id"]);
            var undelivered = db.Query("SELECT id,body,created_at FROM proctor_messages WHERE attempt_id=? AND sender='proctor' AND delivered_at IS NULL ORDER BY id ASC", att["id"]);
            foreach (var m in undelivered) db.Execute("UPDATE proctor_messages SET delivered_at=datetime('now') WHERE id=?", m["id"]);
            // single lowercase keys; desktop ChatMessage(From,Body,At) binds case-insensitively.
            // `at` MUST be ISO-8601: created_at is SQLite "YYYY-MM-DD HH:MM:SS", which System.Text.Json
            // cannot bind to the desktop's DateTimeOffset — an un-converted value throws and fails the
            // ENTIRE HeartbeatResponse deserialization whenever a proctor message is pending.
            var messages = undelivered.Select(m => new { from = "PCI Support", body = m["body"], at = H.IsoFromMillis(H.JsMillis(H.Str(m["created_at"]))) }).ToList();
            var nowMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            // Fire server-side finalisation only past the SAME hard stop the manual submit honours
            // (duration + 1 min network grace), so a heartbeat can't finalise an attempt while the
            // candidate's own on-time submit is still in flight. The DISPLAYED deadline stays at
            // exactly `duration`, so the clock the candidate sees is unchanged.
            var hardStop = deadline + 60_000;
            var forceSubmit = nowMs >= hardStop;
            // #3 — once the hard stop passes, finalise the attempt on the server using the answers saved
            // so far. This closes abandoned/disconnected sittings and blocks any further answer writes.
            // Server-side finalisation at time-up is a legitimate ON-TIME submission (all answers were
            // saved before the deadline), so it PUBLISHES immediately under the same rules as a manual
            // submit: block only on technical invalidity, else release, and issue a credential on a
            // clean pass. Previously this left result_status unset (no publication, no credential, the
            // entitlement never consumed) — an auto-timed-out pass was silently stranded.
            if (forceSubmit && (att["status"] as string) == "in_progress")
            {
                Dictionary<long, long> finalAns;
                try { finalAns = ParseAnswers(JsonDocument.Parse(H.Str(att["answers"]) ?? "{}").RootElement); } catch { finalAns = new(); }
                var fr = ScoreAttempt(att, finalAns);
                var holdReason = Lifecycle.AutoHoldReason(db, att, u.Id, lateSubmit: false);
                var clean = holdReason is null;
                var resultStatus = Lifecycle.ReleaseStatus(clean, fr.Result);
                var itemIdsForCount = new List<long>();
                try { itemIdsForCount = JsonSerializer.Deserialize<List<long>>(H.Str(att["item_ids"]) ?? "[]") ?? new(); } catch { }
                var unanswered = Math.Max(0, itemIdsForCount.Count - finalAns.Count);
                var flagged = (int)db.Scalar<long>("SELECT COUNT(*) FROM proctor_events WHERE attempt_id=?", att["id"]);
                var durationSeconds = (int)Math.Max(0, (nowMs - H.JsMillis(H.Str(att["started_at"]))) / 1000);
                var finalized = db.Execute(@"UPDATE exam_attempts SET submitted_at=datetime('now'), score=?, max_score=?, percent=?, result=?, domain_breakdown=?, status='submitted', review_status=?, result_status=?, hold_reason=?, released_at=CASE WHEN ?='auto_held' THEN NULL ELSE datetime('now') END WHERE id=? AND status='in_progress'",
                    fr.Score, fr.Max, fr.Pct, fr.Result, JsonSerializer.Serialize(fr.Breakdown),
                    clean ? "auto_timeout" : "held", resultStatus, holdReason, resultStatus, att["id"]);
                if (finalized > 0)
                {
                    db.Execute("UPDATE exam_bookings SET status='completed', updated_at=datetime('now') WHERE id=?", att["booking_id"]);
                    Lifecycle.WriteScoreSnapshot(db, att, fr.Score, fr.Max, fr.Pct, fr.Result, JsonSerializer.Serialize(fr.Breakdown), unanswered, flagged, durationSeconds);
                    if (att["booking_id"] is not null)
                        db.Execute("UPDATE exam_entitlements SET status='consumed', attempt_id=? WHERE booking_id=? AND status IN ('available','booked')", att["id"], att["booking_id"]);
                    if (fr.Result == "pass" && clean)
                    {
                        var cred = Lifecycle.IssueCredential(db, u.Id, att["id"], ($"{u.FirstName} {u.LastName}").Trim(), H.L(att.GetValueOrDefault("certification_id") ?? 1L));
                        if (cred is not null) db.Execute("UPDATE exam_attempts SET result_status='credential_issued' WHERE id=?", att["id"]);
                        log(u.Id, "credential_issued", cred ?? "gen_failed");
                    }
                    log(u.Id, clean ? "result_released" : "result_held", $"auto_timeout {resultStatus} {fr.Pct}% {holdReason}");
                }
            }
            var remaining = Math.Max(0, (int)((deadline - nowMs) / 1000));
            // Distinctly-named keys only (no ok+Ok / messages+Messages collisions). Desktop
            // HeartbeatResponse binds Ok/Deadline/ForceSubmit/Messages from lowercase and
            // ServerTime/RemainingSeconds from the camelCase aliases.
            return J(new { ok = true, messages,
                server_time = H.IsoNow, serverTime = H.IsoNow,
                deadline = H.IsoFromMillis(deadline),
                remaining_s = remaining, remainingSeconds = remaining,
                forceSubmit });
        });

        app.MapGet("/api/me/config", (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var keys = new[]{ "sp_login_enabled","sp_exam_booking_open","sp_reschedule_enabled","sp_reschedule_cutoff_hours","sp_results_visible","sp_certificate_download","sp_cpd_enabled","sp_cpd_target_hours","sp_support_tickets_enabled","sp_practice_enabled","sp_banner_enabled","sp_banner_text" };
            var o = new Dictionary<string, object?>();
            foreach (var k in keys) { var v = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k); if (v is not null) o[k] = v; }
            o["sp_readiness_required"] = Settings.Bool(db, "sp_readiness_required", true) ? "1" : "0";
            return J(o);
        });

        // Record a pre-exam readiness/system check (Section C9). The browser performs the actual capability
        // probes; the server stores the outcome and, when required, gates launch on a passed check.
        app.MapGet("/api/me/readiness", (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var latest = db.QueryOne("SELECT camera,microphone,network,fullscreen,environment,browser,screen,passed,created_at FROM exam_readiness_checks WHERE user_id=? ORDER BY id DESC LIMIT 1", u.Id);
            return J(new { required = Settings.Bool(db, "sp_readiness_required", true), latest });
        });
        app.MapPost("/api/me/readiness", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            int F(string k) => H.B(H.GetEl(b, k)?.GetRawText()) ? 1 : 0;
            int camera = F("camera"), mic = F("microphone"), net = F("network"), fs = F("fullscreen"), env = F("environment");
            // Camera + microphone + network are the mandatory checks for a remote-proctored sitting.
            int passed = (camera == 1 && mic == 1 && net == 1) ? 1 : 0;
            var bk = ActiveBooking(u.Id);
            var id = db.ExecuteReturningId("INSERT INTO exam_readiness_checks(user_id,booking_id,camera,microphone,network,fullscreen,environment,browser,screen,passed) VALUES(?,?,?,?,?,?,?,?,?,?)",
                u.Id, bk?["id"], camera, mic, net, fs, env, H.GetS(b, "browser"), H.GetS(b, "screen"), passed);
            log(u.Id, "readiness_check", passed == 1 ? "passed" : "failed");
            return J(new { ok = true, id, passed = passed == 1 });
        });

        app.MapPost("/api/me/exam/launch-code", (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var bk = db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", u.Id);
            if (bk is null) return Results.Json(new { error = "no_booking" }, statusCode: 400);
            // #15 — redesigned launch token: high-entropy, HASHED at rest, short-lived (15 min),
            // single-use. The plaintext is returned exactly once here and never stored.
            var code = "PCI-" + Security.RandomHex(20).ToUpperInvariant();
            db.Execute("INSERT INTO exam_launch_codes(code_hash,user_id,booking_id,expires_at) VALUES(?,?,?,datetime('now','+15 minutes'))", Security.Sha(code), u.Id, bk["id"]);
            return J(new { code, uri = "pciexam://start?code=" + code, expires_in_seconds = 900 });
        });

        // ---------------- CPD ----------------
        app.MapGet("/api/me/cpd", (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { rows = db.Query("SELECT id,activity_date,category,hours,description,evidence_name,status,admin_note,created_at FROM cpd_entries WHERE user_id=? ORDER BY id DESC", u.Id) });
        });
        app.MapPost("/api/me/cpd", async (HttpContext ctx) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var cols = db.Columns("cpd_entries");
            var map = new Dictionary<string, object?> {
                ["user_id"] = u.Id,
                ["description"] = H.GetS(b, "description", "title", "activity") ?? "CPD activity",
                ["category"] = H.GetS(b, "category") ?? "General",
                ["hours"] = H.GetNum(b, "hours") ?? 0,
                ["activity_date"] = H.GetS(b, "activity_date", "date") ?? DateTime.UtcNow.ToString("yyyy-MM-dd")
            };
            var use = map.Keys.Where(k => cols.Contains(k)).ToList();
            db.Execute($"INSERT INTO cpd_entries({string.Join(",", use)}) VALUES({string.Join(",", use.Select(_ => "?"))})", use.Select(k => map[k]).ToArray());
            return J(new { ok = true });
        });
        app.MapDelete("/api/me/cpd/{id}", (HttpContext ctx, long id) =>
        {
            var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            db.Execute("DELETE FROM cpd_entries WHERE id=? AND user_id=?", id, u.Id);
            return J(new { ok = true });
        });

        // ---------------- messages / security / invoices / faqs / account-data ----------------
        app.MapGet("/api/me/messages", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { rows = db.Query("SELECT * FROM notifications WHERE user_id=? ORDER BY id DESC LIMIT 50", u.Id) }); });
        app.MapPost("/api/me/messages/{id}/read", (HttpContext ctx, long id) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            db.Execute("UPDATE notifications SET read_at=datetime('now') WHERE id=? AND user_id=?", id, u.Id); return J(new { ok = true }); });
        app.MapPost("/api/me/messages/read-all", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            db.Execute("UPDATE notifications SET read_at=datetime('now') WHERE user_id=? AND read_at IS NULL", u.Id); return J(new { ok = true }); });
        app.MapGet("/api/me/security", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { logins = db.Query("SELECT created_at,ip,user_agent,device,outcome FROM login_events WHERE user_id=? ORDER BY id DESC LIMIT 20", u.Id),
                two_factor = false, two_factor_coming_soon = true }); });
        app.MapPost("/api/me/2fa", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            // Two-factor auth is not yet enforced at sign-in. Rather than persist a state that would
            // misleadingly display as "on", we register interest and report it as coming soon, so the
            // portal never tells a candidate they are protected when they are not.
            db.Execute("UPDATE users SET two_factor_enabled=0 WHERE id=?", u.Id);
            log(u.Id, "twofa_interest", "requested");
            return J(new { ok = false, coming_soon = true, two_factor = false, message = "Two-factor authentication is coming soon. We'll notify you when it's available." }); });
        app.MapPost("/api/me/sessions/revoke-others", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var h = ctx.Request.Headers.Authorization.ToString(); var cur = h.StartsWith("Bearer ") ? Security.Sha(h[7..]) : "";
            db.Execute("DELETE FROM login_tokens WHERE user_id=? AND purpose='session' AND token!=?", u.Id, cur); return J(new { ok = true }); });
        app.MapGet("/api/me/account-data", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { user = db.QueryOne("SELECT id,email,first_name,last_name,created_at FROM users WHERE id=?", u.Id),
                profile = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", u.Id),
                payments = db.Query("SELECT * FROM payments WHERE user_id=? ORDER BY id DESC", u.Id),
                attempts = db.Query("SELECT id,kind,started_at,submitted_at,percent,result,status,result_status,violations,duration_minutes FROM exam_attempts WHERE user_id=? ORDER BY id DESC", u.Id)
                    .Select(a => { if (H.Str(a["result_status"]) == "auto_held") { a["percent"] = null; a["result"] = null; } return a; }).ToList(),
                cpd = db.Query("SELECT * FROM cpd_entries WHERE user_id=?", u.Id) }); });
        app.MapPost("/api/me/delete-request", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            log(u.Id, "delete_request", u.Email); return J(new { ok = true, note = "A data deletion request has been recorded." }); });
        app.MapGet("/api/me/invoices", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var rows = db.Query("SELECT id,product_type,final_amount,currency,payment_status,payment_date,reference FROM payments WHERE user_id=? AND payment_status='paid' ORDER BY id DESC", u.Id);
            return J(new { rows }); });
        app.MapGet("/api/me/faqs", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { rows = db.Query("SELECT question,answer,category FROM faqs WHERE published=1 ORDER BY sort_order,id") }); });

        // ---------------- tickets ----------------
        app.MapGet("/api/me/tickets", (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var rows = db.Query("SELECT * FROM tickets WHERE user_id=? ORDER BY updated_at DESC", u.Id);
            foreach (var t in rows) t["messages"] = db.Query("SELECT sender,body,created_at FROM ticket_messages WHERE ticket_id=? ORDER BY id ASC", t["id"]);
            return J(new { rows }); });
        app.MapPost("/api/me/tickets", async (HttpContext ctx) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!Settings.Bool(db, "sp_support_tickets_enabled", true)) return Results.Json(new { error = "tickets_disabled", message = "Support ticket submission is temporarily unavailable." }, statusCode: 403);
            var b = await H.Body(ctx.Request);
            var reference = "TKT-" + Security.RandomHex(4).ToUpperInvariant();
            var id = db.ExecuteReturningId("INSERT INTO tickets(user_id,reference,subject,category,status) VALUES(?,?,?,?, 'open')", u.Id, reference, H.GetS(b, "subject") ?? "Support request", H.GetS(b, "category") ?? "general");
            var body = H.GetS(b, "body", "message");
            if (body is not null) db.Execute("INSERT INTO ticket_messages(ticket_id,sender,body) VALUES(?,?,?)", id, "user", body);
            return J(new { ok = true, id, reference }); });
        app.MapPost("/api/me/tickets/{id}/reply", async (HttpContext ctx, long id) => { var u = Auth401(ctx); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var t = db.QueryOne("SELECT * FROM tickets WHERE id=? AND user_id=?", id, u.Id);
            if (t is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            db.Execute("INSERT INTO ticket_messages(ticket_id,sender,body) VALUES(?,?,?)", id, "user", H.GetS(b, "body", "message") ?? "");
            db.Execute("UPDATE tickets SET status='open', updated_at=datetime('now') WHERE id=?", id);
            return J(new { ok = true }); });
    }

    // ---- scoring (parity with scoreAttempt) ----
    // `pct` is what the browser shells read (student.html domain bars); `percent` is an alias emitted
    // for the desktop client, whose DomainBand.Percent binds case-insensitively and would otherwise
    // stay 0. Emitting both keeps browser and desktop breakdowns correct without a schema change.
    public record Band(string domain, int pct, string band)
    {
        public int percent => pct;
    }
    public record Scored(int Score, int Max, double Pct, string Result, List<Band> Breakdown);

    static object? TryJson(string? s) { try { return string.IsNullOrEmpty(s) ? null : JsonSerializer.Deserialize<object>(s); } catch { return null; } }

    static Dictionary<long, long> ParseAnswers(JsonElement? el)
    {
        var d = new Dictionary<long, long>();
        if (el is { ValueKind: JsonValueKind.Object })
            foreach (var p in el.Value.EnumerateObject())
                if (long.TryParse(p.Name, out var qid))
                {
                    if (p.Value.ValueKind == JsonValueKind.Number && p.Value.TryGetInt64(out var v)) d[qid] = v;
                    else if (p.Value.ValueKind == JsonValueKind.String && long.TryParse(p.Value.GetString(), out var v2)) d[qid] = v2;
                }
        return d;
    }

    static Scored ScoreAttempt(Dictionary<string, object?> att, Dictionary<long, long> answers)
    {
        // needs db — resolved via closure isn't available here; recompute using a static reference
        return _scorer!(att, answers);
    }

    static Func<Dictionary<string, object?>, Dictionary<long, long>, Scored>? _scorer;
    public static void InitScorer(Db db)
    {
        _scorer = (att, answers) =>
        {
            var ids = new List<long>();
            try { ids = JsonSerializer.Deserialize<List<long>>(H.Str(att["item_ids"]) ?? "[]") ?? new(); } catch { }
            var rows = ids.Count > 0
                ? db.Query($"SELECT id,answer_index,domain FROM sample_questions WHERE id IN ({string.Join(",", ids.Select(_ => "?"))})", ids.Cast<object?>().ToArray())
                : new List<Dictionary<string, object?>>();
            int sc = 0; var dom = new Dictionary<string, (int n, int ok)>();
            foreach (var r in rows)
            {
                var qid = H.L(r["id"]);
                var ok = answers.TryGetValue(qid, out var a) && a == H.L(r["answer_index"]);
                var d = H.Str(r["domain"]) ?? "General";
                var cur = dom.TryGetValue(d, out var e) ? e : (n: 0, ok: 0);
                cur.n++; if (ok) { sc++; cur.ok++; }
                dom[d] = cur;
            }
            int max = rows.Count > 0 ? rows.Count : 1;
            double pct = Math.Round(sc / (double)max * 1000) / 10;
            // Pass mark comes from the attempt's CERTIFICATION (falls back to the global setting).
            double passMark = Certs.Cfg(db, H.L(att.GetValueOrDefault("certification_id") ?? 1L)).Pass;
            var breakdown = dom.Select(kv => new Band(kv.Key, (int)Math.Round(kv.Value.ok / (double)kv.Value.n * 100),
                kv.Value.ok / (double)kv.Value.n >= 0.8 ? "above" : (kv.Value.ok / (double)kv.Value.n >= passMark / 100 ? "at" : "below"))).ToList();
            return new Scored(sc, max, pct, pct >= passMark ? "pass" : "fail", breakdown);
        };
    }
}
