using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-8 — direct unit coverage of the certification-lifecycle decision core (<see cref="Lifecycle"/>):
/// the booking/launch eligibility gates, the automatic-hold evaluator, the result-status map, and the
/// launch readiness gate. Each test runs against a fresh migrated temp-SQLite database
/// (TestEnv.NewMigratedDb — the same schema CI migrates) and seeds only the rows the decision reads.
///
/// The gate tests use ONE-BLOCKER ISOLATION: seed a fully-eligible baseline, break exactly one
/// precondition, and assert that precisely that blocker surfaces (and that the clean baseline yields an
/// empty list). Blocker/reason strings are stable machine identifiers, so assertions match on them
/// directly. No application code is changed; this pins behaviour that is already shipping.
/// </summary>
public class LifecycleEligibilityTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static long SeedUser(Db db, string email = "life@test.io")
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)",
                   email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    static void SeedAllConsents(Db db, long uid)
    {
        // Drive off the source-of-truth list so the baseline never drifts from the required set.
        foreach (var (type, version) in Lifecycle.RequiredConsents)
            db.Execute("INSERT INTO candidate_consents(user_id,consent_type,policy_version) VALUES(?,?,?)",
                       uid, type, version);
    }

    static void SeedIdentityDoc(Db db, long uid, string status = "verified")
        => db.Execute("INSERT INTO identity_documents(user_id,status) VALUES(?,?)", uid, status);

    /// <summary>A user who passes every DB-side gate: all consents on file, a non-rejected identity
    /// document, account active. The entitlement + profile are supplied per-test as method arguments.</summary>
    static long SeedEligible(Db db, string email = "life@test.io")
    {
        var uid = SeedUser(db, email);
        SeedAllConsents(db, uid);
        SeedIdentityDoc(db, uid, "verified");
        return uid;
    }

    // Entitlement is a payments-row shape: BookingBlockers reads payment_status + exam_schedule_deadline.
    static Dictionary<string, object?> Ent(string paymentStatus = "paid", string? deadline = "2999-01-01 00:00:00")
        => new() { ["payment_status"] = paymentStatus, ["exam_schedule_deadline"] = deadline };

    // Profile completeness is judged on a non-blank country.
    static Dictionary<string, object?> Prof(string? country = "United States")
        => new() { ["country"] = country };

    // ================================================================================================
    //  BookingBlockers — one-blocker isolation
    // ================================================================================================

    [Fact]
    public void BookingBlockers_FullyEligibleBaseline_IsEmpty()
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        Assert.Empty(Lifecycle.BookingBlockers(db, uid, Ent(), Prof()));
    }

    [Fact]
    public void BookingBlockers_NoEntitlement_ExamFeeUnpaid()
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        var b = Lifecycle.BookingBlockers(db, uid, null, Prof());
        Assert.Contains("exam_fee_unpaid", b);
    }

    [Theory]
    [InlineData("refunded")]
    [InlineData("failed")]
    [InlineData("reversed")]
    public void BookingBlockers_InvalidPaymentStatus_PaymentNotValid(string status)
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        var b = Lifecycle.BookingBlockers(db, uid, Ent(paymentStatus: status), Prof());
        Assert.Contains("payment_not_valid", b);
        Assert.DoesNotContain("exam_fee_unpaid", b);
    }

    [Fact]
    public void BookingBlockers_PastDeadline_EntitlementExpired()
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        var b = Lifecycle.BookingBlockers(db, uid, Ent(deadline: "2000-01-01 00:00:00"), Prof());
        Assert.Contains("entitlement_expired", b);
    }

    [Theory]
    [InlineData(false, null)]   // no profile row at all
    [InlineData(true, null)]    // profile present but country null
    [InlineData(true, "")]      // profile present but country blank
    [InlineData(true, "   ")]   // profile present but country whitespace
    public void BookingBlockers_IncompleteProfile_ProfileIncomplete(bool hasProfile, string? country)
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        var profile = hasProfile ? Prof(country) : null;
        var b = Lifecycle.BookingBlockers(db, uid, Ent(), profile);
        Assert.Contains("profile_incomplete", b);
    }

    [Theory]
    [InlineData(null)]        // no identity document on file
    [InlineData("rejected")]  // latest document was rejected
    public void BookingBlockers_MissingOrRejectedIdentityDoc_IdentityDocumentMissing(string? docStatus)
    {
        var db = Fresh();
        // Seed the baseline WITHOUT the verified document so this precondition is the only one broken.
        var uid = SeedUser(db);
        SeedAllConsents(db, uid);
        if (docStatus is not null) SeedIdentityDoc(db, uid, docStatus);

        var b = Lifecycle.BookingBlockers(db, uid, Ent(), Prof());
        Assert.Contains("identity_document_missing", b);
    }

    [Fact]
    public void BookingBlockers_IdentityRequirementDisabled_DoesNotBlock()
    {
        var db = Fresh();
        var uid = SeedUser(db);           // deliberately NO identity document
        SeedAllConsents(db, uid);
        Settings.Put(db, "sp_require_identity_document", "0");
        var b = Lifecycle.BookingBlockers(db, uid, Ent(), Prof());
        Assert.DoesNotContain("identity_document_missing", b);
        Assert.Empty(b);
    }

    [Fact]
    public void BookingBlockers_OutstandingConsent_ConsentsPending()
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        // Drop exactly one required consent.
        db.Execute("DELETE FROM candidate_consents WHERE user_id=? AND consent_type=?", uid, "exam_rules");
        var b = Lifecycle.BookingBlockers(db, uid, Ent(), Prof());
        Assert.Contains("consents_pending", b);
    }

    [Theory]
    [InlineData("deactivated")]
    [InlineData("suspended")]
    [InlineData("hold")]
    public void BookingBlockers_AccountNotActive_AccountHold(string status)
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        db.Execute("UPDATE users SET status=? WHERE id=?", status, uid);
        var b = Lifecycle.BookingBlockers(db, uid, Ent(), Prof());
        Assert.Contains("account_hold", b);
    }

    [Fact]
    public void BookingBlockers_BookingClosedPlatformWide_BookingClosed()
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        Settings.Put(db, "sp_exam_booking_open", "0");
        var b = Lifecycle.BookingBlockers(db, uid, Ent(), Prof());
        Assert.Contains("booking_closed", b);
    }

    // ================================================================================================
    //  LaunchBlockers — the post-booking re-check subset (identity, consents, account hold)
    // ================================================================================================

    [Fact]
    public void LaunchBlockers_FullyEligibleBaseline_IsEmpty()
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        Assert.Empty(Lifecycle.LaunchBlockers(db, uid));
    }

    [Fact]
    public void LaunchBlockers_IdentityRejectedAfterBooking_IdentityDocumentMissing()
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        // Admin rejects the document post-booking: the latest doc governs.
        SeedIdentityDoc(db, uid, "rejected");
        var b = Lifecycle.LaunchBlockers(db, uid);
        Assert.Contains("identity_document_missing", b);
    }

    [Fact]
    public void LaunchBlockers_ConsentVersionBumpedAfterBooking_ConsentsPending()
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        db.Execute("DELETE FROM candidate_consents WHERE user_id=? AND consent_type=?", uid, "proctoring_consent");
        var b = Lifecycle.LaunchBlockers(db, uid);
        Assert.Contains("consents_pending", b);
    }

    [Theory]
    [InlineData("deactivated")]
    [InlineData("suspended")]
    [InlineData("hold")]
    public void LaunchBlockers_AccountPutOnHoldAfterBooking_AccountHold(string status)
    {
        var db = Fresh();
        var uid = SeedEligible(db);
        db.Execute("UPDATE users SET status=? WHERE id=?", status, uid);
        var b = Lifecycle.LaunchBlockers(db, uid);
        Assert.Contains("account_hold", b);
    }

    // ================================================================================================
    //  AutoHoldReason — technical-invalidity (always) + configurable hard rules (off by default)
    // ================================================================================================

    static long SeedBooking(Db db, long uid, string status = "scheduled", object? paymentId = null)
        => db.ExecuteReturningId(
            "INSERT INTO exam_bookings(user_id,payment_id,scheduled_at,status) VALUES(?,?,datetime('now'),?)",
            uid, paymentId, status);

    static Dictionary<string, object?> Attempt(long id, object? bookingId,
        string? tokenOk = null, string? identityResult = null)
        => new()
        {
            ["id"] = id,
            ["booking_id"] = bookingId,
            ["attempt_token_ok"] = tokenOk,
            ["identity_result"] = identityResult,
        };

    [Fact]
    public void AutoHold_CleanValidAttempt_ReturnsNull()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var bid = SeedBooking(db, uid);                 // scheduled, no payment attached
        Assert.Null(Lifecycle.AutoHoldReason(db, Attempt(1, bid), uid, lateSubmit: false));
    }

    [Fact]
    public void AutoHold_LateSubmission_Blocks()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var bid = SeedBooking(db, uid);
        Assert.Equal("submitted_after_deadline",
            Lifecycle.AutoHoldReason(db, Attempt(1, bid), uid, lateSubmit: true));
    }

    [Fact]
    public void AutoHold_NoBooking_BookingMissing()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        Assert.Equal("booking_missing",
            Lifecycle.AutoHoldReason(db, Attempt(1, bookingId: null), uid, lateSubmit: false));
    }

    [Theory]
    [InlineData("cancelled")]
    [InlineData("missed")]
    [InlineData("expired")]
    public void AutoHold_BookingInInvalidState_BookingInvalid(string status)
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var bid = SeedBooking(db, uid, status: status);
        Assert.Equal("booking_invalid",
            Lifecycle.AutoHoldReason(db, Attempt(1, bid), uid, lateSubmit: false));
    }

    [Theory]
    [InlineData("refunded")]
    [InlineData("failed")]
    [InlineData("reversed")]
    public void AutoHold_EntitlementPaymentReversed_PaymentReversed(string payStatus)
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = db.ExecuteReturningId(
            "INSERT INTO payments(user_id,product_type,payment_provider,payment_status) VALUES(?,?,?,?)",
            uid, "exam", "admin_manual", payStatus);
        var bid = SeedBooking(db, uid, paymentId: pid);
        Assert.Equal("payment_reversed",
            Lifecycle.AutoHoldReason(db, Attempt(1, bid), uid, lateSubmit: false));
    }

    [Fact]
    public void AutoHold_SecondSittingOnSameEntitlement_DuplicateAttempt()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = db.ExecuteReturningId(
            "INSERT INTO payments(user_id,product_type,payment_provider,payment_status) VALUES(?,?,?,?)",
            uid, "exam", "admin_manual", "paid");
        var bid = SeedBooking(db, uid, paymentId: pid);
        // A prior SUBMITTED exam attempt tied to the same entitlement already exists.
        var priorId = db.ExecuteReturningId(
            "INSERT INTO exam_attempts(user_id,booking_id,kind,status) VALUES(?,?,?,?)",
            uid, bid, "exam", "submitted");
        // The attempt under evaluation is a different row on the same entitlement.
        Assert.Equal("duplicate_attempt",
            Lifecycle.AutoHoldReason(db, Attempt(priorId + 1, bid), uid, lateSubmit: false));
    }

    [Fact]
    public void AutoHold_CriticalViolation_IsAuditOnlyByDefaultButBlocksWhenEnabled()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var bid = SeedBooking(db, uid);
        var attempt = Attempt(500, bid);
        db.Execute("INSERT INTO proctor_events(attempt_id,user_id,type,severity) VALUES(?,?,?,?)",
                   500, uid, "focus_lost", "Critical");

        // Default (auto_block_result_on_critical_violation=0): proctoring evidence never delays publication.
        Assert.Null(Lifecycle.AutoHoldReason(db, attempt, uid, lateSubmit: false));

        // Deployment opts in → the critical violation now blocks.
        Settings.Put(db, "auto_block_result_on_critical_violation", "1");
        Assert.Equal("critical_proctor_violation",
            Lifecycle.AutoHoldReason(db, attempt, uid, lateSubmit: false));
    }

    [Fact]
    public void AutoHold_CriticalViolation_RespectsConfiguredThreshold()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var bid = SeedBooking(db, uid);
        var attempt = Attempt(600, bid);
        Settings.Put(db, "auto_block_result_on_critical_violation", "1");
        Settings.Put(db, "critical_violation_threshold", "2");

        // One critical event is below the threshold of 2 → still clean.
        db.Execute("INSERT INTO proctor_events(attempt_id,user_id,type,severity) VALUES(?,?,?,?)",
                   600, uid, "focus_lost", "Critical");
        Assert.Null(Lifecycle.AutoHoldReason(db, attempt, uid, lateSubmit: false));

        // A second critical event reaches the threshold → blocks.
        db.Execute("INSERT INTO proctor_events(attempt_id,user_id,type,severity) VALUES(?,?,?,?)",
                   600, uid, "second_face", "Critical");
        Assert.Equal("critical_proctor_violation",
            Lifecycle.AutoHoldReason(db, attempt, uid, lateSubmit: false));
    }

    [Fact]
    public void AutoHold_TamperedToken_BlocksOnlyWhenEnabled()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var bid = SeedBooking(db, uid);
        var attempt = Attempt(1, bid, tokenOk: "0");

        // Off by default → audit-only.
        Assert.Null(Lifecycle.AutoHoldReason(db, attempt, uid, lateSubmit: false));

        Settings.Put(db, "auto_block_result_on_tampered_attempt", "1");
        Assert.Equal("attempt_tampered",
            Lifecycle.AutoHoldReason(db, attempt, uid, lateSubmit: false));
    }

    [Theory]
    [InlineData("fail")]
    [InlineData("failed")]
    public void AutoHold_IdentityFailure_BlocksOnlyWhenEnabled(string idResult)
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var bid = SeedBooking(db, uid);
        var attempt = Attempt(1, bid, identityResult: idResult);

        // Off by default → identity evidence is stored for audit, not a publication gate.
        Assert.Null(Lifecycle.AutoHoldReason(db, attempt, uid, lateSubmit: false));

        Settings.Put(db, "auto_block_result_on_identity_fail", "1");
        Assert.Equal("identity_failed",
            Lifecycle.AutoHoldReason(db, attempt, uid, lateSubmit: false));
    }

    // ================================================================================================
    //  ReleaseStatus — pure clean × result matrix
    // ================================================================================================

    [Theory]
    [InlineData(true, "pass", "released_pass")]
    [InlineData(true, "fail", "released_fail")]
    [InlineData(true, "", "released_fail")]      // anything other than "pass" on a clean attempt releases as fail
    [InlineData(false, "pass", "auto_held")]     // not clean => held regardless of result
    [InlineData(false, "fail", "auto_held")]
    public void ReleaseStatus_MapsCleanAndResult(bool clean, string result, string expected)
        => Assert.Equal(expected, Lifecycle.ReleaseStatus(clean, result));

    // ================================================================================================
    //  ReadinessSatisfied — launch readiness gate
    // ================================================================================================

    [Fact]
    public void ReadinessSatisfied_RequiresARecordedPass()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // sp_readiness_required defaults ON (unseeded) => a passing check is required.
        Assert.False(Lifecycle.ReadinessSatisfied(db, uid));

        // A failed check does not satisfy the gate.
        db.Execute("INSERT INTO exam_readiness_checks(user_id,passed) VALUES(?,0)", uid);
        Assert.False(Lifecycle.ReadinessSatisfied(db, uid));

        // A passed check does.
        db.Execute("INSERT INTO exam_readiness_checks(user_id,passed) VALUES(?,1)", uid);
        Assert.True(Lifecycle.ReadinessSatisfied(db, uid));
    }

    [Fact]
    public void ReadinessSatisfied_WhenNotRequired_IsAlwaysTrue()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        Settings.Put(db, "sp_readiness_required", "0");
        // No readiness check on record, yet the gate passes because the requirement is disabled.
        Assert.True(Lifecycle.ReadinessSatisfied(db, uid));
    }
}
