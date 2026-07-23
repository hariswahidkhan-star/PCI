using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Exam Exceptions &amp; Authorizations — the admin-facing layer over the exam entitlement/booking/attempt
/// machinery. An <b>authorization</b> is one exam "seat": 1:1 with an exam entitlement/payment (enforced by
/// ux_examauth_payment), carrying its configurable scheduling window, deadlines, attempt policy and status.
///
/// Design: the existing student booking flow keeps reading <c>payments.exam_schedule_deadline</c> and
/// <c>exam_entitlements.status</c>, so nothing about normal scheduling changes. This layer (a) computes the
/// real window from <see cref="ResolveWindow"/> (no hardcoded one-year), writing the resulting deadline
/// THROUGH to the payment; (b) records every extension/reschedule/attempt-grant/incident as first-class,
/// preserved history; and (c) grants extra sittings by materialising a fresh entitlement through
/// <see cref="Settlement.Grant"/> — so a granted attempt flows through the exact same book/launch/submit path.
/// </summary>
public static class ExamAuthorization
{
    public record Window(int WindowDays, int? AccessExpiryDays, int AttemptsPermitted, int RetakeWaitDays, string Source);

    static string Now => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    static string Days(int n) => DateTime.UtcNow.AddDays(n).ToString("yyyy-MM-dd HH:mm:ss");
    static string DaysFrom(DateTime from, int n) => from.AddDays(n).ToString("yyyy-MM-dd HH:mm:ss");

    /// <summary>Resolve the effective scheduling window for a (user, certification) by precedence:
    /// individual &gt; campaign &gt; institution &gt; country &gt; exam &gt; route &gt; certification &gt; global default.
    /// Each field takes the first non-null value found walking that precedence; global defaults come from
    /// site_settings (exam_default_window_days / _access_expiry_days / _attempts / _retake_wait_days).</summary>
    public static Window ResolveWindow(Db db, long userId, long certId, string? routeKey = null,
        string? country = null, long? institutionId = null, string? campaign = null)
    {
        // (scope_type, scope_value) pairs, most specific first.
        var scopes = new List<(string type, string? val)>
        {
            ("individual", userId.ToString()),
            ("campaign", campaign),
            ("institution", institutionId?.ToString()),
            ("country", country),
            ("exam", certId.ToString()),
            ("route", routeKey),
            ("certification", certId.ToString()),
        };
        int? win = null, exp = null, att = null, wait = null;
        string src = "default";
        foreach (var (type, val) in scopes)
        {
            if (val is null) continue;
            var r = db.QueryOne("SELECT window_days,access_expiry_days,attempts_permitted,retake_wait_days FROM exam_window_rules WHERE scope_type=? AND scope_value=? AND active=1 ORDER BY id DESC", type, val);
            if (r is null) continue;
            if (win is null && r["window_days"] is not null) { win = (int)H.L(r["window_days"]); src = $"{type}:{val}"; }
            if (exp is null && r["access_expiry_days"] is not null) exp = (int)H.L(r["access_expiry_days"]);
            if (att is null && r["attempts_permitted"] is not null) att = (int)H.L(r["attempts_permitted"]);
            if (wait is null && r["retake_wait_days"] is not null) wait = (int)H.L(r["retake_wait_days"]);
        }
        var winDays = win ?? (int)Settings.Num(db, "exam_default_window_days", 365);
        var expDays = exp ?? (Settings.Str(db, "exam_default_access_expiry_days", "") is { Length: > 0 } e && int.TryParse(e, out var ei) ? ei : (int?)null);
        var attempts = att ?? (int)Settings.Num(db, "exam_default_attempts", 1);
        var waitDays = wait ?? (int)Settings.Num(db, "exam_default_retake_wait_days", 0);
        return new Window(winDays, expDays, Math.Max(1, attempts), Math.Max(0, waitDays), win is null ? "default" : src);
    }

    /// <summary>Ensure an authorization exists for a settled exam payment, computing the configured window and
    /// writing the resulting deadline through to payments.exam_schedule_deadline (overriding the generic
    /// +1 year EnsureDownstream sets when it has no window rule). Idempotent (ux_examauth_payment). Returns
    /// the authorization id, or 0 if the payment is not an exam payment.</summary>
    public static long EnsureForPayment(Db db, long paymentId)
    {
        var existing = db.QueryOne("SELECT id FROM exam_authorizations WHERE payment_id=?", paymentId);
        if (existing is not null) return H.L(existing["id"]);
        var p = db.QueryOne("SELECT id,user_id,product_type,payment_status,exam_schedule_deadline,created_at FROM payments WHERE id=?", paymentId);
        if (p is null) return 0;
        var product = (H.Str(p["product_type"]) ?? "").ToLowerInvariant();
        if (product is not ("exam" or "bundle")) return 0;
        var userId = H.L(p["user_id"]);
        var ent = db.QueryOne("SELECT id,certification_id,route_key FROM exam_entitlements WHERE payment_id=?", paymentId);
        var certId = ent is not null ? H.L(ent["certification_id"]) : 1;
        var routeKey = ent is not null ? H.Str(ent["route_key"]) : null;
        var country = H.Str(db.QueryOne("SELECT country FROM student_profiles WHERE user_id=?", userId)?["country"]);
        var w = ResolveWindow(db, userId, certId, routeKey, country);

        // eligibility start = when the payment settled; deadline = start + window (configured, not hardcoded).
        var startRaw = H.Str(p["created_at"]);
        DateTime start = DateTime.TryParse(startRaw, out var s) ? s.ToUniversalTime() : DateTime.UtcNow;
        var deadline = DaysFrom(start, w.WindowDays);
        var expiry = w.AccessExpiryDays is int ed ? DaysFrom(start, ed) : null;

        // Write the configured deadline through to the payment + entitlement (this is what removes the
        // hardcoded one-year behaviour in practice — a rule of 180 days yields a 180-day deadline).
        db.Execute("UPDATE payments SET exam_schedule_deadline=? WHERE id=?", deadline, paymentId);
        if (ent is not null)
        {
            db.Execute("UPDATE exam_entitlements SET valid_until=? WHERE id=?", deadline, H.L(ent["id"]));
        }
        // Retake waiting period (P0-7): when THIS seat is a retake — i.e. the candidate already has a
        // finalized FAILED sitting for this certification — it cannot be scheduled until the configured
        // cool-off has elapsed since that failure. Persist the concrete date so the booking gate can enforce
        // it and an admin can audibly waive it. The FIRST seat (no prior failure) has no wait → stays NULL.
        var retakeWaitUntil = ResolveRetakeWaitUntil(db, userId, certId, w.RetakeWaitDays);
        var authId = db.ExecuteReturningId(@"INSERT INTO exam_authorizations(user_id,certification_id,payment_id,entitlement_id,eligibility_start,original_deadline,current_deadline,access_expiry,attempts_permitted,attempts_used,window_days,window_source,route_key,country,retake_wait_until,status,created_at,updated_at)
            VALUES(?,?,?,?,?,?,?,?,?,0,?,?,?,?,?, 'active', datetime('now'), datetime('now'))",
            userId, certId, paymentId, ent is not null ? H.L(ent["id"]) : null, start.ToString("yyyy-MM-dd HH:mm:ss"),
            deadline, deadline, expiry, w.AttemptsPermitted, w.WindowDays, w.Source, routeKey, country, retakeWaitUntil);
        if (ent is not null) db.Execute("UPDATE exam_entitlements SET authorization_id=? WHERE id=?", authId, H.L(ent["id"]));
        return authId;
    }

    /// <summary>The retake cool-off expiry for a NEW (user, cert) seat, or null when it does not apply.
    /// Returns <c>last finalized failed sitting + waitDays</c> when that date is still in the future; null when
    /// there is no prior failure, the configured wait is zero, or the cool-off has already elapsed. A held or
    /// invalidated attempt is not a "failure" (result not yet final / does not count), so it never starts a wait.</summary>
    public static string? ResolveRetakeWaitUntil(Db db, long userId, long certId, int waitDays)
    {
        if (waitDays <= 0) return null;
        var lastFail = db.QueryOne(@"SELECT submitted_at FROM exam_attempts
            WHERE user_id=? AND COALESCE(certification_id,1)=? AND kind='exam' AND status='submitted'
              AND COALESCE(counts_as_attempt,1)=1 AND result='fail' AND COALESCE(result_status,'')!='auto_held'
            ORDER BY id DESC LIMIT 1", userId, certId);
        var failAt = H.Str(lastFail?["submitted_at"]);
        if (string.IsNullOrEmpty(failAt)) return null;
        var from = DateTime.TryParse(failAt, out var fd) ? fd.ToUniversalTime() : DateTime.UtcNow;
        var until = DaysFrom(from, waitDays);
        return H.After(until, Now) ? until : null;
    }

    /// <summary>Re-point an authorization to a certification when a settlement retargets its entitlement AFTER
    /// creation (mark-paid/waive call RetargetEntitlement after Settlement.Grant). Recomputes the window from
    /// the new certification's rules, but only while the deadline is still the original (never overrides a
    /// manual extension).</summary>
    public static void SyncCert(Db db, long paymentId, long certId)
    {
        if (certId <= 0) return;
        var a = db.QueryOne("SELECT id,user_id,eligibility_start,original_deadline,current_deadline,route_key,country FROM exam_authorizations WHERE payment_id=?", paymentId);
        if (a is null) return;
        var authId = H.L(a["id"]);
        db.Execute("UPDATE exam_authorizations SET certification_id=? WHERE id=?", certId, authId);
        var orig = H.Str(a["original_deadline"]); var cur = H.Str(a["current_deadline"]);
        var extended = db.Scalar<long>("SELECT COUNT(*) FROM exam_extension_history WHERE authorization_id=?", authId) > 0;
        if (extended || orig != cur) return;   // a manual extension is in effect — leave the deadline alone.
        var uid = H.L(a["user_id"]);
        var w = ResolveWindow(db, uid, certId, H.Str(a["route_key"]), H.Str(a["country"]));
        DateTime start = DateTime.TryParse(H.Str(a["eligibility_start"]), out var s) ? s.ToUniversalTime() : DateTime.UtcNow;
        var deadline = DaysFrom(start, w.WindowDays);
        var expiry = w.AccessExpiryDays is int ed ? DaysFrom(start, ed) : null;
        db.Execute("UPDATE exam_authorizations SET original_deadline=?, current_deadline=?, access_expiry=?, window_days=?, window_source=? WHERE id=?", deadline, deadline, expiry, w.WindowDays, w.Source, authId);
        db.Execute("UPDATE payments SET exam_schedule_deadline=? WHERE id=?", deadline, paymentId);
        db.Execute("UPDATE exam_entitlements SET valid_until=? WHERE payment_id=?", deadline, paymentId);
    }

    /// <summary>Boot backfill: ensure every settled exam payment has an authorization. Best-effort.</summary>
    public static void BackfillAll(Db db)
    {
        try
        {
            var rows = db.Query("SELECT id FROM payments WHERE product_type IN ('exam','bundle') AND payment_status IN ('paid','waived') AND id NOT IN (SELECT COALESCE(payment_id,0) FROM exam_authorizations)");
            foreach (var r in rows) EnsureForPayment(db, H.L(r["id"]));
        }
        catch { /* backfill is best-effort; never blocks boot */ }
    }

    /// <summary>Count exam sittings that consumed an attempt for a (user, cert) — submitted certifying
    /// attempts flagged counts_as_attempt=1. Used for the aggregate "attempts used" the portal shows.</summary>
    public static int AttemptsUsed(Db db, long userId, long certId) =>
        (int)db.Scalar<long>("SELECT COUNT(*) FROM exam_attempts WHERE user_id=? AND certification_id=? AND kind='exam' AND status='submitted' AND COALESCE(counts_as_attempt,1)=1", userId, certId);

    /// <summary>Total exam seats (entitlements) a candidate holds for a cert = permitted sittings.</summary>
    public static int AttemptsPermitted(Db db, long userId, long certId) =>
        (int)db.Scalar<long>("SELECT COUNT(*) FROM exam_entitlements e JOIN payments p ON p.id=e.payment_id WHERE e.user_id=? AND COALESCE(e.certification_id,1)=? AND p.product_type IN ('exam','bundle') AND p.payment_status IN ('paid','waived') AND e.status!='refunded'", userId, certId);

    /// <summary>Extend an authorization's scheduling deadline (and optionally the access-expiry). Preserves the
    /// original deadline, writes the new deadline through to the payment/entitlement so the booking gate uses
    /// it, records an extension-history row, and reactivates an expired authorization.</summary>
    public static object Extend(Db db, long authId, string newDeadline, string? newExpiry,
        string? reason, string? note, bool feeApplies, bool isFree, long? approverId, string? evidenceRef)
    {
        var a = db.QueryOne("SELECT id,user_id,certification_id,payment_id,current_deadline,access_expiry FROM exam_authorizations WHERE id=?", authId);
        if (a is null) return new { ok = false, error = "authorization_not_found" };
        var prevDeadline = H.Str(a["current_deadline"]);
        var prevExpiry = H.Str(a["access_expiry"]);
        var uid = H.L(a["user_id"]);
        var payId = a["payment_id"];
        var added = (int)Math.Round((H.JsMillis(newDeadline) - (prevDeadline is not null ? H.JsMillis(prevDeadline) : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())) / 86_400_000.0);
        db.Execute("UPDATE exam_authorizations SET current_deadline=?, access_expiry=COALESCE(?,access_expiry), status='active', updated_at=datetime('now') WHERE id=?", newDeadline, newExpiry, authId);
        if (payId is not null)
        {
            db.Execute("UPDATE payments SET exam_schedule_deadline=? WHERE id=?", newDeadline, payId);
            db.Execute("UPDATE exam_entitlements SET valid_until=?, status=CASE WHEN status='expired' THEN 'available' ELSE status END WHERE payment_id=?", newDeadline, payId);
        }
        db.Execute(@"INSERT INTO exam_extension_history(authorization_id,user_id,certification_id,previous_deadline,new_deadline,previous_expiry,new_expiry,added_days,reason,note,fee_applies,is_free,evidence_ref,approved_by,approved_at)
            VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?, datetime('now'))",
            authId, uid, H.L(a["certification_id"]), prevDeadline, newDeadline, prevExpiry, newExpiry, added, reason, note, feeApplies ? 1 : 0, isFree ? 1 : 0, evidenceRef, approverId);
        return new { ok = true, authorization_id = authId, previous_deadline = prevDeadline, new_deadline = newDeadline, added_days = added };
    }

    /// <summary>Record a reschedule in the preserved history (the caller performs the booking change).</summary>
    public static void RecordReschedule(Db db, long? bookingId, long? authId, long userId, long certId,
        string? prevAt, string? prevTz, string? prevStatus, string? newAt, string? newTz,
        string? deliveryChange, string? providerChange, string? reason, string? note, bool feeApplies, bool feeWaived, long? changedBy)
    {
        db.Execute(@"INSERT INTO exam_reschedule_history(booking_id,authorization_id,user_id,certification_id,previous_scheduled_at,previous_timezone,previous_status,new_scheduled_at,new_timezone,delivery_change,provider_change,reason,note,fee_applies,fee_waived,changed_by,created_at)
            VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?, datetime('now'))",
            bookingId, authId, userId, certId, prevAt, prevTz, prevStatus, newAt, newTz, deliveryChange, providerChange, reason, note, feeApplies ? 1 : 0, feeWaived ? 1 : 0, changedBy);
    }

    /// <summary>Grant an additional/replacement/complimentary/paid/etc. exam opportunity. Materialises a fresh
    /// exam seat through Settlement.Grant (waived when feeWaived, else a payable entitlement), links it to a
    /// new authorization, and records a classified exam_attempt_grants row. When a replacement is for a verified
    /// system/provider failure, pass countsAsAttempt=false so it does not consume the candidate's allowance.</summary>
    public static object GrantAttempt(Db db, long userId, long certId, string grantType, bool countsAsAttempt,
        string? reason, string? note, long? incidentId, bool feeApplies, bool feeWaived, long? approverId)
    {
        var email = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", userId)?["email"]);
        long payId;
        if (feeWaived)
        {
            var reference = "GRANT-" + Security.RandomHex(5).ToUpperInvariant();
            payId = Settlement.Grant(db, userId, email, "exam", certId, 0, reference, "admin_waiver",
                new Settlement.Meta { RecordedBy = approverId, Note = $"attempt grant: {grantType}" + (reason is { Length: > 0 } ? " — " + reason : "") });
            try
            {
                var lp = Settlement.ListPrice(db, "exam");
                db.Execute(@"INSERT INTO fee_waivers(user_id,product_type,certification_id,kind,fee_type,waiver_type,original_amount,waived_amount,final_amount,payable_amount,reason,note,approved_by,payment_id,incident_id,status)
                    VALUES(?, 'exam', ?, 'full', 'retake', ?, ?, ?, 0, 0, ?, ?, ?, ?, ?, 'granted')",
                    userId, certId, grantType, lp, lp, reason ?? grantType, note, approverId, payId, incidentId);
            }
            catch { }
        }
        else
        {
            // A payable grant: create the entitlement but leave it as an unpaid retake seat is not supported by
            // the settlement engine, so a "paid" grant is still recorded as a $0 authorization the student then
            // settles via checkout — represented here as a waived seat flagged fee_applies for the ledger.
            var reference = "GRANT-" + Security.RandomHex(5).ToUpperInvariant();
            payId = Settlement.Grant(db, userId, email, "exam", certId, 0, reference, "admin_waiver",
                new Settlement.Meta { RecordedBy = approverId, Note = $"attempt grant (fee applies): {grantType}" });
        }
        var authId = EnsureForPayment(db, payId);
        var grantId = db.ExecuteReturningId(@"INSERT INTO exam_attempt_grants(authorization_id,user_id,certification_id,grant_type,counts_as_attempt,reason,note,incident_id,payment_id,fee_applies,fee_waived,status,approved_by,created_at)
            VALUES(?,?,?,?,?,?,?,?,?,?,?, 'granted', ?, datetime('now'))",
            authId, userId, certId, grantType, countsAsAttempt ? 1 : 0, reason, note, incidentId, payId, feeApplies ? 1 : 0, feeWaived ? 1 : 0, approverId);
        return new { ok = true, grant_id = grantId, authorization_id = authId, payment_id = payId, grant_type = grantType, counts_as_attempt = countsAsAttempt, fee_waived = feeWaived };
    }

    /// <summary>Restore an incorrectly-consumed attempt: invalidate the bad attempt (preserved, never deleted),
    /// mark it not-counting, and reopen its entitlement so the candidate can sit again. Integrity: the original
    /// score/result row is kept; the result is not overwritten to a pass/fail.</summary>
    public static object RestoreAttempt(Db db, long attemptId, string reason, long? approverId)
    {
        var at = db.QueryOne("SELECT id,user_id,certification_id,booking_id,result FROM exam_attempts WHERE id=?", attemptId);
        if (at is null) return new { ok = false, error = "attempt_not_found" };
        db.Execute("UPDATE exam_attempts SET result_status='invalidated', review_status='invalidated', counts_as_attempt=0, invalidation_reason=?, invalidated_by=? WHERE id=?", reason, approverId, attemptId);
        // Revoke any credential the invalidated attempt issued (no duplicate/leftover credential).
        db.Execute("UPDATE issued_credentials SET status='revoked' WHERE attempt_id=? AND status='active'", attemptId);
        // Reopen the seat: the booking's entitlement returns to 'available' so book() permits a fresh sitting.
        var bk = at["booking_id"];
        if (bk is not null)
        {
            var b = db.QueryOne("SELECT payment_id FROM exam_bookings WHERE id=?", bk);
            if (b?["payment_id"] is { } pid)
                db.Execute("UPDATE exam_entitlements SET status='available', booking_id=NULL, attempt_id=NULL WHERE payment_id=? AND status IN ('consumed','booked')", pid);
            db.Execute("UPDATE exam_bookings SET status='cancelled', updated_at=datetime('now') WHERE id=?", bk);
        }
        return new { ok = true, attempt_id = attemptId, restored = true };
    }

    /// <summary>Waive the retake waiting period for a (user, cert): clear retake_wait_until on live authorizations.</summary>
    public static object WaiveWaitingPeriod(Db db, long userId, long certId, long? approverId)
    {
        db.Execute("UPDATE exam_authorizations SET retake_wait_until=NULL, updated_at=datetime('now') WHERE user_id=? AND certification_id=? AND status='active'", userId, certId);
        return new { ok = true, user_id = userId, certification_id = certId, waiting_period_waived = true };
    }
}
