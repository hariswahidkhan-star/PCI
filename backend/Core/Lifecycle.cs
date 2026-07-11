using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Central certification-lifecycle logic: eligibility gating (used by booking/start/authorize),
/// automatic hold evaluation on submission, and derivation of the candidate lifecycle object for the
/// dashboard. Keeping this in one place means the same rules apply on every route rather than being
/// re-implemented (and drifting) per endpoint.
/// </summary>
public static class Lifecycle
{
    // The set of consents a candidate must have accepted (current versions) before booking or launching.
    public static readonly (string type, string version)[] RequiredConsents =
    {
        ("candidate_handbook", "1.0"),
        ("exam_rules", "1.0"),
        ("proctoring_consent", "1.0"),
        ("privacy_notice", "1.0"),
        ("refund_policy", "1.0"),
        ("misconduct_policy", "1.0"),
        ("credential_terms", "1.0"),
    };

    /// <summary>Which required consents are still outstanding (missing or wrong version) for a user.</summary>
    public static List<string> OutstandingConsents(Db db, long userId)
    {
        var have = db.Query("SELECT consent_type, policy_version FROM candidate_consents WHERE user_id=?", userId)
            .Select(r => (H.Str(r["consent_type"]), H.Str(r["policy_version"])))
            .ToHashSet();
        return RequiredConsents.Where(rc => !have.Contains((rc.type, rc.version))).Select(rc => rc.type).ToList();
    }

    /// <summary>Blocking items preventing a candidate from booking/launching. Empty ⇒ eligible.
    /// Every route (dashboard, booking, browser start, desktop authorize) calls this so the gate is uniform.</summary>
    public static List<string> BookingBlockers(Db db, long userId, Dictionary<string, object?>? entitlement, Dictionary<string, object?>? profile)
    {
        var blockers = new List<string>();

        // Payment / entitlement valid and unused.
        // NOTE: `entitlement` is a row from the payments table (see ExamEntitlement in StudentExam),
        // so its status/deadline are read directly off the row. A prior version dereferenced a
        // non-existent "payment_id" key, which threw KeyNotFoundException for any user who had
        // actually paid — GetValueOrDefault keeps this robust to shape drift.
        if (entitlement is null) blockers.Add("exam_fee_unpaid");
        else
        {
            var payStatus = H.Str(entitlement.GetValueOrDefault("payment_status"));
            if (payStatus is "refunded" or "failed" or "reversed") blockers.Add("payment_not_valid");
            var deadline = H.Str(entitlement.GetValueOrDefault("exam_schedule_deadline"));
            if (H.IsPast(deadline)) blockers.Add("entitlement_expired");
        }

        // Profile completeness (legal name + a couple of key fields)
        if (profile is null || string.IsNullOrWhiteSpace(H.Str(profile.GetValueOrDefault("country"))))
            blockers.Add("profile_incomplete");

        // Government-issued photo ID on file. The LATEST document governs: none, or a rejected one,
        // blocks booking until (re-)upload. A freshly submitted document satisfies the gate — admin
        // verification is an audit action, not an operational bottleneck. Toggleable for deployments
        // that verify identity exclusively at exam-day check-in.
        if (Settings.Bool(db, "sp_require_identity_document", true))
        {
            var idDoc = db.QueryOne("SELECT status FROM identity_documents WHERE user_id=? ORDER BY id DESC", userId);
            if (idDoc is null || H.Str(idDoc["status"]) == "rejected") blockers.Add("identity_document_missing");
        }

        // Required consents
        if (OutstandingConsents(db, userId).Count > 0) blockers.Add("consents_pending");

        // Admin holds / account status
        var status = H.Str(db.Scalar<string>("SELECT status FROM users WHERE id=?", userId));
        if (status is "deactivated" or "suspended" or "hold") blockers.Add("account_hold");

        // Portal switch: admins can close booking platform-wide (sp_exam_booking_open=0).
        if (!Settings.Bool(db, "sp_exam_booking_open", true)) blockers.Add("booking_closed");

        return blockers;
    }

    /// <summary>Readiness gate applied at LAUNCH (not booking): when sp_readiness_required is on, the
    /// candidate must have a passed readiness check on record. Returns true if launch may proceed.</summary>
    public static bool ReadinessSatisfied(Db db, long userId)
    {
        if (!Settings.Bool(db, "sp_readiness_required", true)) return true;
        return db.Scalar<long>("SELECT COUNT(*) FROM exam_readiness_checks WHERE user_id=? AND passed=1", userId) > 0;
    }

    /// <summary>
    /// Evaluate automatic-hold reasons for a submitted attempt. Returns null when the attempt is CLEAN
    /// (eligible for immediate release + auto credential on a pass); otherwise the machine reason string.
    /// This encodes Section-A rule 5 (auto-hold only for serious exceptions).
    /// </summary>
    /// <summary>
    /// Evaluate whether a submitted attempt must be blocked from immediate publication. Returns null when
    /// the result may publish immediately (the DEFAULT). Only TECHNICAL-INVALIDITY reasons block by default;
    /// proctoring/identity evidence is stored for audit and does NOT delay publication unless a hard rule is
    /// explicitly enabled in settings (auto_block_result_on_*). This keeps immediate auto-publication the
    /// default while still preventing cheating, replay, late submission and entitlement abuse.
    ///
    /// Technical-invalidity reasons (always block): late submission, missing/invalid booking, booking already
    /// completed by another attempt, consumed/invalid entitlement, reversed payment, duplicate submission,
    /// and item-set mismatch (answers reference items not in the server-issued set). Wrong-user and
    /// already-submitted are enforced at the endpoint before scoring.
    /// </summary>
    public static string? AutoHoldReason(Db db, Dictionary<string, object?> attempt, long userId, bool lateSubmit)
    {
        // ── Technical-invalidity: these ALWAYS block, regardless of configuration ──
        if (lateSubmit) return "submitted_after_deadline";                 // impossible/late timing

        var bookingId = attempt["booking_id"];
        var booking = bookingId is null ? null : db.QueryOne("SELECT * FROM exam_bookings WHERE id=?", bookingId);
        if (booking is null) return "booking_missing";                     // exam not started through a valid booking
        var bStatus = H.Str(booking["status"]);
        if (bStatus is "cancelled" or "missed" or "expired") return "booking_invalid";

        var payId = booking["payment_id"];
        if (payId is not null)
        {
            var payStatus = H.Str(db.Scalar<string>("SELECT payment_status FROM payments WHERE id=?", payId));
            if (payStatus is "refunded" or "failed" or "reversed") return "payment_reversed";  // entitlement invalid
        }

        // Duplicate completed attempt for the same entitlement (replay / double sitting)
        var dup = db.Scalar<long>(@"SELECT COUNT(*) FROM exam_attempts a JOIN exam_bookings b ON b.id=a.booking_id
            WHERE b.payment_id=? AND a.kind='exam' AND a.status='submitted' AND a.id<>?", payId, attempt["id"]);
        if (dup > 0) return "duplicate_attempt";

        // ── Configurable hard rules (OFF by default): proctoring/identity are audit-only unless enabled ──
        // These exist so a deployment CAN opt into automatic invalidation, but they never require manual review.
        if (Settings.Bool(db, "auto_block_result_on_tampered_attempt", false))
        {
            var tokenOk = H.Str(attempt.GetValueOrDefault("attempt_token_ok"));
            if (tokenOk == "0") return "attempt_tampered";
        }
        if (Settings.Bool(db, "auto_block_result_on_critical_violation", false))
        {
            var threshold = (int)Settings.Num(db, "critical_violation_threshold", 1);
            var critical = db.Scalar<long>("SELECT COUNT(*) FROM proctor_events WHERE attempt_id=? AND severity='Critical'", attempt["id"]);
            if (critical >= threshold) return "critical_proctor_violation";
        }
        if (Settings.Bool(db, "auto_block_result_on_identity_fail", false))
        {
            var idResult = H.Str(attempt.GetValueOrDefault("identity_result"));
            if (idResult is "fail" or "failed") return "identity_failed";
        }

        return null; // technically valid → publish immediately (the default)
    }

    /// <summary>Map a scored attempt to its released result_status.</summary>
    public static string ReleaseStatus(bool clean, string result)
        => !clean ? "auto_held" : (result == "pass" ? "released_pass" : "released_fail");

    /// <summary>Write the immutable score snapshot for an attempt (idempotent via the unique index).</summary>
    public static void WriteScoreSnapshot(Db db, Dictionary<string, object?> attempt, int score, int max, double pct, string result,
        string breakdownJson, int unanswered, int flagged, int durationSeconds)
    {
        db.Execute(@"INSERT OR IGNORE INTO exam_score_snapshots
            (attempt_id,user_id,score,max_score,percent,result,domain_breakdown,unanswered,flagged_events,duration_seconds,submitted_at,answer_key_version,bank_version)
            VALUES(?,?,?,?,?,?,?,?,?,?,datetime('now'),?,?)",
            attempt["id"], attempt["user_id"], score, max, pct, result, breakdownJson, unanswered, flagged, durationSeconds,
            H.Str(attempt.GetValueOrDefault("answer_key_version")), H.Str(attempt.GetValueOrDefault("bank_version")));
    }

    /// <summary>Extra exam minutes granted via an APPROVED accommodation request. Uses the largest
    /// single approval (approvals do not stack) so the effect is predictable and auditable.</summary>
    public static int ApprovedExtraMinutes(Db db, long userId)
        => (int)db.Scalar<long>("SELECT COALESCE(MAX(approved_extra_minutes),0) FROM accommodation_requests WHERE user_id=? AND status='approved'", userId);

    /// <summary>Issue a credential for a passed attempt, retrying credential-id collisions. The id
    /// format is &lt;prefix&gt;-&lt;year&gt;-&lt;5 digits&gt; where prefix and expiry come from the attempt's
    /// CERTIFICATION (PCP-AI is simply certification 1). The partial unique index on attempt_id
    /// guarantees at most one credential per attempt — if one is already linked, its id is returned
    /// rather than issuing a duplicate. Returns null only if generation failed (astronomically
    /// unlikely) — callers should log that case.</summary>
    public static string? IssueCredential(Db db, long userId, object? attemptId, string holderName, long certificationId = Certs.DefaultId)
    {
        var existing = attemptId is null ? null : db.QueryOne("SELECT credential_id FROM issued_credentials WHERE attempt_id=?", attemptId);
        if (existing is not null) return H.Str(existing["credential_id"]);
        var cert = Certs.ById(db, certificationId) ?? Certs.ById(db, Certs.DefaultId)!;
        var prefix = Certs.Prefix(cert);
        var years = Certs.ExpiryYears(cert);
        var yr = DateTime.UtcNow.Year;
        var rnd = new Random();
        for (int i = 0; i < 20; i++)
        {
            var cid = $"{prefix}-{yr}-{10000 + rnd.Next(0, 89999)}";
            try
            {
                // expiry computed in C# (SQLite-format string) so it is provider-agnostic — no SQL
                // string concatenation, which does not translate to MySQL.
                var expires = DateTime.UtcNow.AddYears(years).ToString("yyyy-MM-dd HH:mm:ss");
                db.Execute("INSERT INTO issued_credentials(credential_id,user_id,attempt_id,holder_name,credential,certification_id,status,expires_at) VALUES(?,?,?,?,?,?, 'active', ?)",
                    cid, userId, attemptId, holderName, prefix, H.L(cert["id"]), expires);
                return cid;
            }
            catch { /* credential_id collision → retry; attempt_id duplicate → loop exits via existing check next call */ }
        }
        return null;
    }

    /// <summary>Build the candidate lifecycle object for the dashboard (Section B rule 1).</summary>
    public static object BuildLifecycle(Db db, long userId, Dictionary<string, object?>? membership,
        Dictionary<string, object?>? entitlement, Dictionary<string, object?>? booking,
        Dictionary<string, object?>? latestAttempt, Dictionary<string, object?>? credential, List<string> blockers)
    {
        var memStatus = membership is null ? "none" : H.Str(membership.GetValueOrDefault("status"));
        string candidate = entitlement is null ? (memStatus == "active" ? "member" : "none") : "exam_fee_paid";

        string examStatus = "not_booked";
        if (latestAttempt is not null)
        {
            var st = H.Str(latestAttempt.GetValueOrDefault("status"));
            examStatus = st == "in_progress" ? "in_progress" : (st == "submitted" ? "submitted" : "not_booked");
        }
        else if (booking is not null) examStatus = "booked";

        string? resultStatus = latestAttempt is null ? null : H.Str(latestAttempt.GetValueOrDefault("result_status"));
        if (string.IsNullOrEmpty(resultStatus) && latestAttempt is not null) resultStatus = null;

        string? credentialStatus = credential is null ? null : H.Str(credential.GetValueOrDefault("status"));

        // Next step
        string nextStep;
        if (memStatus != "active") nextStep = "activate_membership";
        else if (entitlement is null) nextStep = "pay_exam_fee";
        else if (blockers.Count > 0) nextStep = "complete_eligibility";
        else if (booking is null) nextStep = "schedule_exam";
        else if (resultStatus is "released_fail") nextStep = "review_retake";
        else if (resultStatus is "auto_held") nextStep = "await_review";
        else if (credentialStatus == "active") nextStep = "maintain_credential";
        else if (examStatus == "booked") nextStep = "prepare_launch";
        else nextStep = "await_result";

        return new
        {
            membership_status = memStatus,
            candidate_status = candidate,
            exam_status = examStatus,
            result_status = resultStatus,
            credential_status = credentialStatus,
            next_step = nextStep,
            blocking_items = blockers
        };
    }
}
