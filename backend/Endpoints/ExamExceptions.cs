using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin Console → Exam Exceptions &amp; Authorizations. Manage exceptional exam situations for any candidate and
/// certification: extend an expired scheduling deadline, reopen scheduling, reschedule on the candidate's
/// behalf, grant replacement/additional attempts, restore an incorrectly-consumed attempt, waive exam/retake/
/// rescheduling fees or the retake waiting period, and run the exam-incident decision workflow. Every action is
/// gated by a granular <c>ex_*</c> permission (see Rbac.Sections["exam_exceptions"]), audited with the acting
/// admin + previous/new values + reason, and notifies the candidate (in-app + email). Integrity is preserved:
/// attempts and results are never deleted or silently overwritten — invalidation/restoration are explicit and
/// recorded, and the configurable window (Core/ExamAuthorization) is honoured throughout.
/// </summary>
public static class ExamExceptions
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        static string? S(Dictionary<string, System.Text.Json.JsonElement> b, string k) => H.GetS(b, k);

        Dictionary<string, object?>? UserRow(long uid) => db.QueryOne("SELECT id,email,first_name,last_name FROM users WHERE id=?", uid);
        string CertName(long certId) => H.Str(db.QueryOne("SELECT name FROM certifications WHERE id=?", certId)?["name"]) ?? ("Certification " + certId);

        // Candidate notification: always an in-app message; email is best-effort and honours the master toggle.
        void NotifyStudent(long uid, string title, string body, string cta = "View exam status", string route = "/certifications")
        {
            try { db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Exam Exception', ?, ?, ?, ?)", uid, title, body, cta, route); } catch { }
            if (!Notify.Enabled(db, "exam_exception")) return;
            try
            {
                var u = UserRow(uid); var email = H.Str(u?["email"]);
                if (string.IsNullOrWhiteSpace(email)) return;
                var first = H.Str(u?["first_name"]);
                var portal = Mailer.BaseUrl() + "/app" + route;
                var html = $@"<div style=""font-family:-apple-system,Segoe UI,Roboto,Arial,sans-serif;max-width:600px;margin:0 auto;padding:24px;color:#0E1525"">
<h1 style=""font-size:20px"">{System.Net.WebUtility.HtmlEncode(title)}</h1>
<p style=""font-size:15px;line-height:1.7;color:#434b57"">Hello {System.Net.WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(first) ? "there" : first!)}, {System.Net.WebUtility.HtmlEncode(body)}</p>
<p style=""font-size:15px""><a href=""{portal}"" style=""color:#1D4ED8"">{System.Net.WebUtility.HtmlEncode(cta)}</a></p></div>";
                Mailer.Send(db, uid, email!, "exam_exception", title, html);
            }
            catch { }
        }

        // Resolve the authorization a request targets and its owning user/cert (404 helper baked into caller).
        Dictionary<string, object?>? Auth(long authId) => db.QueryOne("SELECT * FROM exam_authorizations WHERE id=?", authId);

        // ── LIST: searchable/filterable authorizations (one row per exam seat) ──────────────────────────
        app.MapGet("/api/admin/exam-exceptions", (HttpContext ctx) => gate(ctx.Request, "ex_view", adm =>
        {
            var q = ctx.Request.Query;
            var where = new List<string> { "1=1" }; var args = new List<object?>();
            if (!string.IsNullOrWhiteSpace(q["q"])) { where.Add("(u.email LIKE ? OR u.first_name LIKE ? OR u.last_name LIKE ?)"); var like = "%" + q["q"] + "%"; args.Add(like); args.Add(like); args.Add(like); }
            if (long.TryParse(q["certification_id"], out var cid) && cid > 0) { where.Add("a.certification_id=?"); args.Add(cid); }
            if (!string.IsNullOrWhiteSpace(q["status"])) { where.Add("a.status=?"); args.Add(q["status"].ToString()); }
            if (!string.IsNullOrWhiteSpace(q["country"])) { where.Add("a.country=?"); args.Add(q["country"].ToString()); }
            if (long.TryParse(q["institution_id"], out var inst) && inst > 0) { where.Add("a.institution_id=?"); args.Add(inst); }
            if (!string.IsNullOrWhiteSpace(q["from"])) { where.Add("a.current_deadline>=?"); args.Add(q["from"].ToString()); }
            if (!string.IsNullOrWhiteSpace(q["to"])) { where.Add("a.current_deadline<=?"); args.Add(q["to"].ToString()); }
            var certFilter = adm.CertFilterSql("a.certification_id");
            var rows = db.Query($@"SELECT a.id,a.user_id,a.certification_id,a.payment_id,a.original_deadline,a.current_deadline,a.access_expiry,
                        a.attempts_permitted,a.attempts_used,a.retake_wait_until,a.window_days,a.window_source,a.status,a.institution_id,a.country,a.created_at,
                        u.email,u.first_name,u.last_name, c.name certification_name, c.acronym certification_acronym,
                        (SELECT COUNT(*) FROM exam_extension_history e WHERE e.authorization_id=a.id) extensions,
                        (SELECT COUNT(*) FROM exam_reschedule_history r WHERE r.authorization_id=a.id) reschedules,
                        (SELECT COUNT(*) FROM exam_incidents i WHERE i.authorization_id=a.id) incidents,
                        (SELECT COUNT(*) FROM exam_attempt_grants g WHERE g.authorization_id=a.id) grants
                    FROM exam_authorizations a
                    LEFT JOIN users u ON u.id=a.user_id
                    LEFT JOIN certifications c ON c.id=a.certification_id
                    WHERE {string.Join(" AND ", where)}{certFilter}
                    ORDER BY a.id DESC LIMIT 300", args.ToArray());
            // Attach live attempts used/permitted aggregate per candidate+cert.
            foreach (var r in rows)
            {
                var uid = H.L(r["user_id"]); var cc = H.L(r["certification_id"]);
                r["attempts_used_total"] = ExamAuthorization.AttemptsUsed(db, uid, cc);
                r["attempts_permitted_total"] = ExamAuthorization.AttemptsPermitted(db, uid, cc);
            }
            return J(new { rows });
        }));

        // ── DETAIL: one authorization with full preserved history ──────────────────────────────────────
        app.MapGet("/api/admin/exam-exceptions/{authId}", (HttpContext ctx, long authId) => gate(ctx.Request, "ex_view", adm =>
        {
            var a = Auth(authId); if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (!adm.CanCert(a["certification_id"])) return Results.Json(new { error = "forbidden" }, statusCode: 403);
            var uid = H.L(a["user_id"]); var cid = H.L(a["certification_id"]);
            return J(new
            {
                authorization = a,
                student = UserRow(uid),
                certification = CertName(cid),
                attempts_used = ExamAuthorization.AttemptsUsed(db, uid, cid),
                attempts_permitted = ExamAuthorization.AttemptsPermitted(db, uid, cid),
                extensions = db.Query("SELECT * FROM exam_extension_history WHERE authorization_id=? ORDER BY id DESC", authId),
                reschedules = db.Query("SELECT * FROM exam_reschedule_history WHERE authorization_id=? ORDER BY id DESC", authId),
                attempt_grants = db.Query("SELECT * FROM exam_attempt_grants WHERE authorization_id=? ORDER BY id DESC", authId),
                incidents = db.Query("SELECT * FROM exam_incidents WHERE authorization_id=? OR user_id=? ORDER BY id DESC", authId, uid),
                attempts = db.Query("SELECT id,booking_id,kind,status,result_status,attempt_class,counts_as_attempt,invalidation_reason,started_at,submitted_at FROM exam_attempts WHERE user_id=? AND certification_id=? ORDER BY id DESC", uid, cid),
                bookings = db.Query("SELECT id,scheduled_at,timezone,status,reschedule_count,created_at FROM exam_bookings WHERE user_id=? AND certification_id=? ORDER BY id DESC", uid, cid),
                waivers = db.Query("SELECT * FROM fee_waivers WHERE user_id=? AND COALESCE(certification_id,0) IN (0,?) ORDER BY id DESC", uid, cid),
            });
        }));

        // ── EXTEND DEADLINE ──────────────────────────────────────────────────────────────────────────
        app.MapPost("/api/admin/exam-authorizations/{authId}/extend", async (HttpContext ctx, long authId) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_extend", adm =>
            {
                var a = Auth(authId); if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (!adm.CanCert(a["certification_id"])) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var newDeadline = S(b, "new_deadline"); var reason = S(b, "reason");
                if (string.IsNullOrWhiteSpace(newDeadline)) return Results.Json(new { error = "new_deadline_required" }, statusCode: 400);
                if (string.IsNullOrWhiteSpace(reason)) return Results.Json(new { error = "reason_required" }, statusCode: 400);
                var res = ExamAuthorization.Extend(db, authId, newDeadline!, S(b, "new_expiry"), reason, S(b, "note"),
                    H.GetBool(b, "fee_applies") ?? false, H.GetBool(b, "is_free") ?? true, adm.Id, S(b, "evidence_ref"));
                var uid = H.L(a["user_id"]); var cid = H.L(a["certification_id"]);
                log(adm.Id, "exam_deadline_extended", $"auth {authId} user {uid} → {newDeadline} — {reason}");
                NotifyStudent(uid, "Your exam scheduling deadline has been extended",
                    $"your new deadline to schedule the {CertName(cid)} exam is {newDeadline}. You can schedule your sitting now.");
                return J(res);
            });
        });

        // ── RESCHEDULE (admin, on the candidate's behalf) — preserves the old appointment as history ─────
        app.MapPost("/api/admin/exam-authorizations/{authId}/reschedule", async (HttpContext ctx, long authId) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_reschedule", adm =>
            {
                var a = Auth(authId); if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (!adm.CanCert(a["certification_id"])) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var when = S(b, "scheduled_at"); var reason = S(b, "reason");
                if (string.IsNullOrWhiteSpace(when)) return Results.Json(new { error = "scheduled_at_required" }, statusCode: 400);
                if (string.IsNullOrWhiteSpace(reason)) return Results.Json(new { error = "reason_required" }, statusCode: 400);
                var uid = H.L(a["user_id"]); var cid = H.L(a["certification_id"]); var tz = S(b, "timezone") ?? "UTC";
                var open = db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND certification_id=? AND status='scheduled' ORDER BY id DESC", uid, cid);
                long bookingId;
                if (open is not null)
                {
                    // Preserve the original appointment as a cancelled history row; create the new one fresh.
                    db.Execute("UPDATE exam_bookings SET status='rescheduled', updated_at=datetime('now') WHERE id=?", open["id"]);
                    bookingId = db.ExecuteReturningId("INSERT INTO exam_bookings(user_id,payment_id,certification_id,scheduled_at,timezone,reschedule_count,authorization_id) VALUES(?,?,?,?,?,?,?)",
                        uid, open["payment_id"], cid, when, tz, H.L(open["reschedule_count"]) + 1, authId);
                    ExamAuthorization.RecordReschedule(db, bookingId, authId, uid, cid, H.Str(open["scheduled_at"]), H.Str(open["timezone"]), "scheduled", when, tz, S(b, "delivery_change"), S(b, "provider_change"), reason, S(b, "note"), H.GetBool(b, "fee_applies") ?? false, H.GetBool(b, "fee_waived") ?? true, adm.Id);
                }
                else
                {
                    bookingId = db.ExecuteReturningId("INSERT INTO exam_bookings(user_id,payment_id,certification_id,scheduled_at,timezone,authorization_id) VALUES(?,?,?,?,?,?)",
                        uid, a["payment_id"], cid, when, tz, authId);
                    ExamAuthorization.RecordReschedule(db, bookingId, authId, uid, cid, null, null, null, when, tz, S(b, "delivery_change"), S(b, "provider_change"), reason, S(b, "note"), H.GetBool(b, "fee_applies") ?? false, H.GetBool(b, "fee_waived") ?? true, adm.Id);
                }
                db.Execute("UPDATE exam_entitlements SET status='booked', booking_id=? WHERE payment_id=? AND status IN ('available','booked')", bookingId, a["payment_id"]);
                try { PCI.Backend.Core.ExamDelivery.RescheduleBooking(db, bookingId, when, tz).GetAwaiter().GetResult(); } catch { }
                log(adm.Id, "exam_admin_rescheduled", $"auth {authId} user {uid} → {when} — {reason}");
                NotifyStudent(uid, "Your exam has been rescheduled", $"your {CertName(cid)} exam is now scheduled for {when}.");
                return J(new { ok = true, booking_id = bookingId, scheduled_at = when });
            });
        });

        // ── REOPEN SCHEDULING (single candidate) ─────────────────────────────────────────────────────
        app.MapPost("/api/admin/exam-authorizations/reopen", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_reopen", adm =>
            {
                var uid = (long)(H.GetNum(b, "user_id") ?? 0);
                var cid = Certs.Resolve(db, S(b, "certification_id") ?? S(b, "certification") ?? "1");
                if (uid <= 0 || UserRow(uid) is null) return Results.Json(new { error = "student_not_found" }, statusCode: 404);
                if (!adm.CanCert(cid)) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var reason = S(b, "reason") ?? "reopened by admin";
                var res = Reopen(db, uid, cid, reason, S(b, "new_deadline"), H.GetBool(b, "waive_fee") ?? false, adm.Id, NotifyStudent);
                log(adm.Id, "exam_scheduling_reopened", $"user {uid} cert {cid} — {reason}");
                return J(res);
            });
        });

        // ── BULK REOPEN (campaign/institution/incident) — guarded against accidental mass changes ───────
        app.MapPost("/api/admin/exam-authorizations/bulk-reopen", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_bulk", adm =>
            {
                var reason = S(b, "reason");
                if (string.IsNullOrWhiteSpace(reason)) return Results.Json(new { error = "reason_required" }, statusCode: 400);
                List<long> ids = new();
                if (H.GetEl(b, "user_ids") is { ValueKind: System.Text.Json.JsonValueKind.Array } arr)
                    ids = arr.EnumerateArray().Where(e => e.ValueKind == System.Text.Json.JsonValueKind.Number).Select(e => e.GetInt64()).Distinct().ToList();
                var cid = Certs.Resolve(db, S(b, "certification_id") ?? "1");
                if (ids.Count == 0) return Results.Json(new { error = "no_targets", message = "Provide user_ids[]." }, statusCode: 400);
                // Safeguard: an explicit confirm_count must match, so a mis-sized batch cannot fire by accident.
                var confirm = (int)(H.GetNum(b, "confirm_count") ?? -1);
                if (confirm != ids.Count) return Results.Json(new { error = "confirm_count_mismatch", expected = ids.Count }, statusCode: 400);
                if (ids.Count > 500) return Results.Json(new { error = "batch_too_large", max = 500 }, statusCode: 400);
                int done = 0;
                foreach (var uid in ids)
                {
                    if (UserRow(uid) is null || !adm.CanCert(cid)) continue;
                    try { Reopen(db, uid, cid, reason!, S(b, "new_deadline"), H.GetBool(b, "waive_fee") ?? false, adm.Id, NotifyStudent); done++; } catch { }
                }
                log(adm.Id, "exam_bulk_reopened", $"{done}/{ids.Count} cert {cid} — {reason}");
                return J(new { ok = true, reopened = done, requested = ids.Count });
            });
        });

        // ── GRANT ATTEMPT (replacement / additional / complimentary / paid / technical / …) ─────────────
        app.MapPost("/api/admin/exam-attempts/grant", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            // Replacement attempts (usually a verified failure) need ex_grant_replacement; all others additional.
            var grantType = (S(b, "grant_type") ?? "additional").Trim().ToLowerInvariant();
            var perm = grantType == "replacement" ? "ex_grant_replacement" : "ex_grant_additional";
            return gate(ctx.Request, perm, adm =>
            {
                var uid = (long)(H.GetNum(b, "user_id") ?? 0);
                var cid = Certs.Resolve(db, S(b, "certification_id") ?? S(b, "certification") ?? "1");
                if (uid <= 0 || UserRow(uid) is null) return Results.Json(new { error = "student_not_found" }, statusCode: 404);
                if (!adm.CanCert(cid)) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var reason = S(b, "reason"); if (string.IsNullOrWhiteSpace(reason)) return Results.Json(new { error = "reason_required" }, statusCode: 400);
                // A replacement for a verified system/provider failure does not consume the allowance.
                var countsAsAttempt = H.GetBool(b, "counts_as_attempt") ?? (grantType != "replacement" && grantType != "technical");
                var incidentId = H.GetNum(b, "incident_id") is double iid && iid > 0 ? (long?)(long)iid : null;
                var res = ExamAuthorization.GrantAttempt(db, uid, cid, grantType, countsAsAttempt, reason, S(b, "note"), incidentId,
                    H.GetBool(b, "fee_applies") ?? false, H.GetBool(b, "fee_waived") ?? true, adm.Id);
                log(adm.Id, "exam_attempt_granted", $"user {uid} cert {cid} {grantType} counts={countsAsAttempt} — {reason}");
                NotifyStudent(uid, "You have been granted another exam attempt",
                    $"a {grantType} attempt for the {CertName(cid)} exam has been approved. You can schedule your sitting now.");
                return J(res);
            });
        });

        // ── RESTORE a consumed attempt (invalidate the bad attempt, reopen the seat) ────────────────────
        app.MapPost("/api/admin/exam-attempts/{attemptId}/restore", async (HttpContext ctx, long attemptId) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_restore", adm =>
            {
                var at = db.QueryOne("SELECT user_id,certification_id FROM exam_attempts WHERE id=?", attemptId);
                if (at is null) return Results.Json(new { error = "attempt_not_found" }, statusCode: 404);
                if (!adm.CanCert(at["certification_id"])) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var reason = S(b, "reason"); if (string.IsNullOrWhiteSpace(reason)) return Results.Json(new { error = "reason_required" }, statusCode: 400);
                var res = ExamAuthorization.RestoreAttempt(db, attemptId, reason!, adm.Id);
                var uid = H.L(at["user_id"]);
                log(adm.Id, "exam_attempt_restored", $"attempt {attemptId} user {uid} — {reason}");
                NotifyStudent(uid, "Your exam attempt has been restored", "an incorrectly-used attempt has been restored — you can schedule again.");
                return J(res);
            });
        });

        // ── INVALIDATE an attempt (integrity: preserved, never deleted; result not overwritten to pass/fail) ─
        app.MapPost("/api/admin/exam-attempts/{attemptId}/invalidate", async (HttpContext ctx, long attemptId) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_invalidate", adm =>
            {
                var at = db.QueryOne("SELECT user_id,certification_id,result FROM exam_attempts WHERE id=?", attemptId);
                if (at is null) return Results.Json(new { error = "attempt_not_found" }, statusCode: 404);
                if (!adm.CanCert(at["certification_id"])) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var reason = S(b, "reason"); if (string.IsNullOrWhiteSpace(reason)) return Results.Json(new { error = "reason_required" }, statusCode: 400);
                db.Execute("UPDATE exam_attempts SET result_status='invalidated', review_status='invalidated', counts_as_attempt=0, invalidation_reason=?, invalidated_by=? WHERE id=?", reason, adm.Id, attemptId);
                db.Execute("UPDATE issued_credentials SET status='revoked' WHERE attempt_id=? AND status='active'", attemptId);
                var uid = H.L(at["user_id"]);
                log(adm.Id, "exam_attempt_invalidated", $"attempt {attemptId} user {uid} (was {H.Str(at["result"]) ?? "-"}) — {reason}");
                NotifyStudent(uid, "An exam attempt has been placed under review", "an exam attempt has been invalidated pending review. Support will be in touch if action is needed.");
                return J(new { ok = true, attempt_id = attemptId, invalidated = true });
            });
        });

        // ── WAIVE the retake waiting period ────────────────────────────────────────────────────────────
        app.MapPost("/api/admin/exam-authorizations/{authId}/waive-waiting-period", async (HttpContext ctx, long authId) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_waive_wait", adm =>
            {
                var a = Auth(authId); if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (!adm.CanCert(a["certification_id"])) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var uid = H.L(a["user_id"]); var cid = H.L(a["certification_id"]);
                var res = ExamAuthorization.WaiveWaitingPeriod(db, uid, cid, adm.Id);
                log(adm.Id, "exam_waiting_period_waived", $"user {uid} cert {cid} — {S(b, "reason")}");
                NotifyStudent(uid, "Retake waiting period waived", "the waiting period before your next attempt has been waived.");
                return J(res);
            });
        });

        // ── FEE WAIVER (exam / retake / rescheduling) — payable 0 skips checkout ─────────────────────────
        app.MapPost("/api/admin/exam-fee-waiver", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var feeType = (S(b, "fee_type") ?? "exam").Trim().ToLowerInvariant();
            var perm = feeType == "retake" ? "ex_waive_retake" : feeType == "reschedule" ? "ex_waive_resched" : "ex_waive_exam";
            return gate(ctx.Request, perm, adm =>
            {
                var uid = (long)(H.GetNum(b, "user_id") ?? 0);
                var cid = Certs.Resolve(db, S(b, "certification_id") ?? "1");
                if (uid <= 0 || UserRow(uid) is null) return Results.Json(new { error = "student_not_found" }, statusCode: 404);
                if (!adm.CanCert(cid)) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var reason = S(b, "reason"); if (string.IsNullOrWhiteSpace(reason)) return Results.Json(new { error = "reason_required" }, statusCode: 400);
                var percent = Math.Clamp((int)(H.GetNum(b, "percent") ?? 100), 1, 100);
                var waiverType = S(b, "waiver_type") ?? (percent >= 100 ? "full" : "partial");
                // Partial and reschedule-only paths never materialise a seat under a uniqueness guard of
                // their own — require a durable client idempotency key so retries cannot duplicate them.
                var requiresIdem = percent < 100 || feeType == "reschedule";
                var idemKey = FeeWaiverLedger.ResolveKey(ctx.Request, b);
                if (requiresIdem && idemKey is null)
                    return Results.Json(new { error = "idempotency_key_required", message = "Partial and reschedule-only waivers require an Idempotency-Key (header or body)." }, statusCode: 400);
                if (idemKey is not null)
                {
                    var prior = FeeWaiverLedger.Find(db, idemKey);
                    if (prior is not null) return J(FeeWaiverLedger.ReplayResponse(db, prior));
                }
                var original = Settlement.ListPrice(db, "exam");
                var waived = Math.Round(original * percent / 100.0, 2);
                var payable = Math.Round(original - waived, 2);
                var incidentId = H.GetNum(b, "incident_id") is double iid && iid > 0 ? (long?)(long)iid : null;
                var institutionId = H.GetNum(b, "institution_id") is double i2 && i2 > 0 ? (long?)(long)i2 : null;
                long? seatPayId = null;
                long waiverId = 0;
                var raced = false;
                db.Transaction(() =>
                {
                    // Claim the idempotency key on the ledger first so a concurrent retry cannot grant a
                    // second seat / second ledger line after the first request already succeeded.
                    bool created;
                    (waiverId, created) = FeeWaiverLedger.TryInsert(db, idemKey, uid, "exam", cid,
                        percent >= 100 ? "full" : "partial", feeType, waiverType,
                        original, waived, payable, payable, reason, S(b, "note"), adm.Id,
                        expiresAt: S(b, "expires"), sponsor: S(b, "sponsor"),
                        institutionId: institutionId, incidentId: incidentId, evidenceRef: S(b, "evidence_ref"));
                    if (!created && idemKey is not null)
                    {
                        raced = true;
                        return;
                    }
                    // A full exam/retake waiver materialises a schedulable $0 seat immediately (no checkout);
                    // reschedule fees never gate booking, so those are recorded but grant no new seat.
                    // recordFeeWaiver:false — this handler owns the single ledger row (with idempotency).
                    if (percent >= 100 && feeType != "reschedule")
                    {
                        var g = ExamAuthorization.GrantAttempt(db, uid, cid, "complimentary", true, reason, S(b, "note"), incidentId, false, true, adm.Id,
                            recordFeeWaiver: false, feeType: feeType);
                        seatPayId = g.GetType().GetProperty("payment_id")?.GetValue(g) as long?;
                        if (seatPayId is not null)
                            db.Execute("UPDATE fee_waivers SET payment_id=? WHERE id=?", seatPayId, waiverId);
                    }
                });
                if (raced)
                    return J(FeeWaiverLedger.ReplayResponse(db, FeeWaiverLedger.Find(db, idemKey!)!));
                log(adm.Id, "exam_fee_waived", $"user {uid} cert {cid} {feeType} {percent}% payable {payable} — {reason}");
                NotifyStudent(uid, "An exam fee has been waived",
                    payable <= 0 ? $"your {feeType} fee for the {CertName(cid)} exam has been fully waived — you can schedule without payment." : $"a {percent}% waiver has been applied to your {feeType} fee; the remaining ${payable:0.##} is payable at checkout.");
                return J(new { ok = true, waiver_id = waiverId, fee_type = feeType, percent, original, waived_amount = waived, payable, skips_checkout = payable <= 0, replayed = false });
            });
        });

        // ── WINDOW RULES (configurable scheduling windows) ──────────────────────────────────────────────
        app.MapGet("/api/admin/exam-window-rules", (HttpContext ctx) => gate(ctx.Request, "ex_view", _ =>
            J(new { rows = db.Query("SELECT * FROM exam_window_rules ORDER BY id DESC"),
                    defaults = new { window_days = Settings.Num(db, "exam_default_window_days", 365), access_expiry_days = Settings.Str(db, "exam_default_access_expiry_days", ""), attempts = Settings.Num(db, "exam_default_attempts", 1), retake_wait_days = Settings.Num(db, "exam_default_retake_wait_days", 0) },
                    scope_types = new[] { "certification", "route", "exam", "country", "institution", "campaign", "individual" } })));

        app.MapPost("/api/admin/exam-window-rules", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_approve", adm =>
            {
                var id = (long)(H.GetNum(b, "id") ?? 0);
                var scope = (S(b, "scope_type") ?? "").Trim().ToLowerInvariant();
                if (scope is not ("certification" or "route" or "exam" or "country" or "institution" or "campaign" or "individual"))
                    return Results.Json(new { error = "bad_scope_type" }, statusCode: 400);
                var vals = new object?[] { scope, S(b, "scope_value"),
                    H.GetNum(b, "window_days") is double wd ? (int)wd : null,
                    H.GetNum(b, "access_expiry_days") is double ad ? (int)ad : null,
                    H.GetNum(b, "attempts_permitted") is double ap ? (int)ap : null,
                    H.GetNum(b, "retake_wait_days") is double rw ? (int)rw : null,
                    H.GetBool(b, "active") ?? true ? 1 : 0, S(b, "note") };
                if (id > 0)
                    db.Execute("UPDATE exam_window_rules SET scope_type=?,scope_value=?,window_days=?,access_expiry_days=?,attempts_permitted=?,retake_wait_days=?,active=?,note=? WHERE id=?", vals.Append((object?)id).ToArray());
                else
                    id = db.ExecuteReturningId("INSERT INTO exam_window_rules(scope_type,scope_value,window_days,access_expiry_days,attempts_permitted,retake_wait_days,active,note,created_by) VALUES(?,?,?,?,?,?,?,?,?)", vals.Append((object?)adm.Id).ToArray());
                log(adm.Id, "exam_window_rule_saved", $"rule {id} {scope}:{S(b, "scope_value")}");
                return J(new { ok = true, id });
            });
        });

        app.MapPost("/api/admin/exam-window-rules/{id}/delete", (HttpContext ctx, long id) => gate(ctx.Request, "ex_approve", adm =>
        {
            db.Execute("DELETE FROM exam_window_rules WHERE id=?", id);
            log(adm.Id, "exam_window_rule_deleted", id.ToString());
            return J(new { ok = true });
        }));

        // ── INCIDENTS ───────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/exam-incidents", (HttpContext ctx) => gate(ctx.Request, "ex_incidents", adm =>
        {
            var q = ctx.Request.Query;
            var where = new List<string> { "1=1" }; var args = new List<object?>();
            if (!string.IsNullOrWhiteSpace(q["status"])) { where.Add("i.status=?"); args.Add(q["status"].ToString()); }
            if (!string.IsNullOrWhiteSpace(q["severity"])) { where.Add("i.severity=?"); args.Add(q["severity"].ToString()); }
            if (!string.IsNullOrWhiteSpace(q["category"])) { where.Add("i.category=?"); args.Add(q["category"].ToString()); }
            if (long.TryParse(q["certification_id"], out var cid) && cid > 0) { where.Add("i.certification_id=?"); args.Add(cid); }
            if (!string.IsNullOrWhiteSpace(q["q"])) { where.Add("(u.email LIKE ? OR u.first_name LIKE ?)"); var like = "%" + q["q"] + "%"; args.Add(like); args.Add(like); }
            return J(new { rows = db.Query($@"SELECT i.*, u.email, u.first_name, u.last_name, c.name certification_name
                    FROM exam_incidents i LEFT JOIN users u ON u.id=i.user_id LEFT JOIN certifications c ON c.id=i.certification_id
                    WHERE {string.Join(" AND ", where)}{adm.CertFilterSql("i.certification_id")} ORDER BY i.id DESC LIMIT 300", args.ToArray()) });
        }));

        app.MapGet("/api/admin/exam-incidents/{id}", (HttpContext ctx, long id) => gate(ctx.Request, "ex_incidents", adm =>
        {
            var i = db.QueryOne("SELECT * FROM exam_incidents WHERE id=?", id);
            if (i is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (!adm.CanCert(i["certification_id"])) return Results.Json(new { error = "forbidden" }, statusCode: 403);
            var uid = H.L(i["user_id"]);
            return J(new { incident = i, student = UserRow(uid),
                attempt = i["attempt_id"] is { } aid ? db.QueryOne("SELECT id,status,result_status,attempt_class,started_at,submitted_at FROM exam_attempts WHERE id=?", aid) : null,
                proctor_events = i["attempt_id"] is { } aid2 ? db.Query("SELECT type,severity,detail,at FROM proctor_events WHERE attempt_id=? ORDER BY id DESC LIMIT 50", aid2) : new() });
        }));

        // Incident decision: applies the chosen remedy through the Core service (never silently alters a result).
        app.MapPost("/api/admin/exam-incidents/{id}/decide", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_incidents", adm =>
            {
                var i = db.QueryOne("SELECT * FROM exam_incidents WHERE id=?", id);
                if (i is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (!adm.CanCert(i["certification_id"])) return Results.Json(new { error = "forbidden" }, statusCode: 403);
                var decision = (S(b, "decision") ?? "").Trim().ToLowerInvariant();
                var valid = new[] { "no_action", "extend_deadline", "reopen_scheduling", "reschedule", "grant_replacement", "grant_additional", "waive_waiting_period", "waive_exam_fee", "waive_reschedule_fee", "waive_retake_fee", "restore_attempt", "cancel_invalid_attempt", "under_review", "escalate", "request_information", "reject" };
                if (!valid.Contains(decision)) return Results.Json(new { error = "bad_decision", allowed = valid }, statusCode: 400);
                var uid = H.L(i["user_id"]); var cid = H.L(i["certification_id"]);
                object? remedy = null;
                // Each remedy checks its own granular permission so an incident reviewer cannot escalate privilege.
                bool Can(string p) => adm.IsOwner || adm.Perms.Contains(p);
                switch (decision)
                {
                    case "grant_replacement":
                    case "grant_additional":
                        if (!Can(decision == "grant_replacement" ? "ex_grant_replacement" : "ex_grant_additional")) return Results.Json(new { error = "forbidden_remedy" }, statusCode: 403);
                        remedy = ExamAuthorization.GrantAttempt(db, uid, cid, decision == "grant_replacement" ? "replacement" : "additional", decision != "grant_replacement", "incident #" + id, S(b, "note"), id, false, true, adm.Id);
                        break;
                    case "restore_attempt":
                    case "cancel_invalid_attempt":
                        if (!Can("ex_restore")) return Results.Json(new { error = "forbidden_remedy" }, statusCode: 403);
                        if (i["attempt_id"] is { } aid) remedy = ExamAuthorization.RestoreAttempt(db, H.L(aid), "incident #" + id, adm.Id);
                        break;
                    case "waive_waiting_period":
                        if (!Can("ex_waive_wait")) return Results.Json(new { error = "forbidden_remedy" }, statusCode: 403);
                        remedy = ExamAuthorization.WaiveWaitingPeriod(db, uid, cid, adm.Id);
                        break;
                    case "reopen_scheduling":
                        if (!Can("ex_reopen")) return Results.Json(new { error = "forbidden_remedy" }, statusCode: 403);
                        remedy = Reopen(db, uid, cid, "incident #" + id, null, false, adm.Id, NotifyStudent);
                        break;
                    // extend_deadline / reschedule / fee waivers are actioned from their own drawers after this
                    // decision is recorded, so the reviewer picks the disposition here without over-broad rights.
                }
                var status = decision is "no_action" or "reject" ? "decided" : decision is "under_review" ? "under_review" : decision == "request_information" ? "info_required" : decision == "escalate" ? "escalated" : "decided";
                db.Execute("UPDATE exam_incidents SET status=?, decision=?, remedy=?, investigation_result=COALESCE(?,investigation_result), decided_by=?, decided_at=datetime('now') WHERE id=?",
                    status, decision, S(b, "note"), S(b, "investigation_result"), adm.Id, id);
                log(adm.Id, "exam_incident_decided", $"incident {id} user {uid} → {decision}");
                NotifyStudent(uid, "Your exam incident has been reviewed", $"a decision has been recorded for your exam incident: {decision.Replace('_', ' ')}.", "View exam status", "/support");
                return J(new { ok = true, incident_id = id, decision, status, remedy });
            });
        });

        app.MapPost("/api/admin/exam-incidents/{id}/note", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "ex_incidents", adm =>
            {
                var note = S(b, "note"); if (string.IsNullOrWhiteSpace(note)) return Results.Json(new { error = "note_required" }, statusCode: 400);
                db.Execute("UPDATE exam_incidents SET investigation_result=COALESCE(investigation_result,'') || ? WHERE id=?", $"\n[admin {adm.Id}] {note}", id);
                log(adm.Id, "exam_incident_note", $"incident {id}");
                return J(new { ok = true });
            });
        });
    }

    /// <summary>Shared reopen: reactivate the latest authorization for a (user, cert), extend the deadline to a
    /// fresh window (or the provided date), return the seat to schedulable, and — if none exists and a waiver is
    /// authorised — materialise a complimentary seat. Immediately clears the scheduling blocker.</summary>
    static object Reopen(Db db, long userId, long certId, string reason, string? newDeadline, bool waiveFee, long adminId, Action<long, string, string, string, string> notify)
    {
        var a = db.QueryOne("SELECT id,payment_id,eligibility_start FROM exam_authorizations WHERE user_id=? AND certification_id=? ORDER BY id DESC", userId, certId);
        string deadline;
        long authId;
        if (a is not null)
        {
            authId = H.L(a["id"]);
            var w = ExamAuthorization.ResolveWindow(db, userId, certId);
            deadline = newDeadline ?? DateTime.UtcNow.AddDays(w.WindowDays).ToString("yyyy-MM-dd HH:mm:ss");
            ExamAuthorization.Extend(db, authId, deadline, null, reason, "reopen scheduling", false, true, adminId, null);
            db.Execute("UPDATE exam_authorizations SET status='active', updated_at=datetime('now') WHERE id=?", authId);
            if (a["payment_id"] is { } pid)
                db.Execute("UPDATE exam_entitlements SET status='available', booking_id=NULL WHERE payment_id=? AND status IN ('expired','booked')", pid);
        }
        else if (waiveFee)
        {
            var g = ExamAuthorization.GrantAttempt(db, userId, certId, "complimentary", true, reason, "reopen (no prior seat)", null, false, true, adminId);
            authId = g.GetType().GetProperty("authorization_id")?.GetValue(g) is long al ? al : 0;
            deadline = H.Str(db.QueryOne("SELECT current_deadline FROM exam_authorizations WHERE id=?", authId)?["current_deadline"]) ?? "";
        }
        else
        {
            return new { ok = false, error = "no_seat", message = "This candidate has no exam seat. Grant a complimentary seat (waive_fee=true) or record a payment first." };
        }
        notify(userId, "Exam scheduling reopened", "you can schedule your exam again from your portal.", "Schedule your exam", "/certifications");
        return new { ok = true, authorization_id = authId, new_deadline = deadline };
    }
}
