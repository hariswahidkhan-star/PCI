using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-13 — the daily reminder sweep (<see cref="CommsReminderService.SweepOnce"/>): the code that scans the
/// platform for time-based conditions and fires each one through <see cref="Comms.Fire"/>, so every reminder
/// inherits the same per-trigger channel toggles, consent gating and duplicate suppression as any other event.
/// SweepOnce evaluates FIVE independent rules, each with its own window and dedup key:
///   • membership.expiry_reminder            — active membership expiring within the next 30 days
///   • membership.expired                    — membership lapsed within the last 30 days
///   • exam.scheduling_deadline_approaching  — paid/waived exam whose schedule-by date is within 7 days AND not yet booked
///   • application.incomplete_reminder       — application sitting in info_requested for more than 7 days
///   • cert.recert_due                       — active credential inside 90 days of its recertification date
///
/// SweepOnce takes only (Db db) and reads datetime('now') INTERNALLY (no injectable clock), so every candidate
/// date is seeded RELATIVE to now via datetime('now', ...) to place it precisely in or out of each window.
///
/// Two environment facts drive the seeding (mirrors CommsDecisionTests):
///   1. FK ON — a real users row must exist before anything references user_id, so each test seeds a user first.
///   2. The test schema starts with an EMPTY comm_triggers catalogue (CommsSeed runs only at app startup, never
///      in Migrate.Run), and Comms.Fire is a no-op for a missing/inactive trigger. Every reminder therefore
///      only fans out when its trigger is seeded explicitly here — so each test seeds exactly the trigger under
///      test with EMAIL-ONLY toggles (whatsapp/inapp off), giving exactly ONE comm_outbox row per fired
///      reminder. Assertions count comm_outbox rows by trigger_code; Fire/SweepOnce side effects are the queue.
///
/// Db is sealed and NOT IDisposable, so each test takes a fresh migrated Db by value (never `using var`).
/// No application code is changed and nothing dials the network (the email leg uses the outbox path, not Mailer).
/// </summary>
public class CommsReminderTests
{
    // Rule → trigger code (read verbatim from CommsReminders.cs).
    const string MembershipExpiring = "membership.expiry_reminder";
    const string MembershipLapsed   = "membership.expired";
    const string ExamScheduling     = "exam.scheduling_deadline_approaching";
    const string AppIncomplete      = "application.incomplete_reminder";
    const string CredentialRecert   = "cert.recert_due";

    static Db Fresh() => TestEnv.NewMigratedDb();

    // Gotcha 1: FK ON — seed a real user before referencing user_id. status defaults 'active' (the sweep filters
    // on u.status='active'); pass 'deactivated' to exercise the inactive-user exclusion.
    static long SeedUser(Db db, string email = "rem@test.io", string status = "active")
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)",
                   email, "Test", "student", status);
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    // Gotcha 2: the catalogue is empty by default. Seed the one trigger under test, EMAIL-ONLY + active, so a
    // fired reminder writes exactly one email row (no whatsapp/inapp legs to disambiguate) with consent_category
    // 'transactional' (reminders bypass the marketing consent gate).
    static void SeedTrigger(Db db, string code)
    {
        db.Execute(@"INSERT INTO comm_triggers
            (code,name,email_enabled,whatsapp_enabled,inapp_enabled,consent_category,active)
            VALUES(?,?,1,0,0,'transactional',1)", code, code);
    }

    // The outbox is the queue SweepOnce writes to; count deduped rows for one rule's trigger_code.
    static long Queued(Db db, string triggerCode)
        => db.Scalar<long>("SELECT COUNT(*) FROM comm_outbox WHERE trigger_code=?", triggerCode);

    // -- Seed helpers: offsets are datetime() modifiers ("+29 day", "-31 day", ...) resolved at seed time by
    // TestEnv.Stamp. They are resolved in C# rather than passed to datetime('now', ?) because a modifier bound
    // as a PARAMETER is invisible to the SQLite-to-MySQL rewrite, so the statement only works on SQLite.
    static void SeedMembership(Db db, long uid, string status, string expiryOffset)
        => db.Execute(@"INSERT INTO memberships(user_id,membership_type,status,expiry_date)
                        VALUES(?, 'Student Membership', ?, ?)", uid, status, TestEnv.Stamp(expiryOffset));

    static long SeedExamPayment(Db db, long uid, string deadlineOffset)
        => db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,payment_status,exam_schedule_deadline)
                        VALUES(?, 'exam', 'paid', ?)", uid, TestEnv.Stamp(deadlineOffset));

    static void SeedBooking(Db db, long uid, long payId, string status)
        => db.Execute(@"INSERT INTO exam_bookings(user_id,payment_id,certification_id,scheduled_at,status)
                        VALUES(?,?,1, datetime('now','+1 day'), ?)", uid, payId, status);

    static void SeedApplication(Db db, long uid, string appNo, string status, string updatedOffset)
        => db.Execute(@"INSERT INTO certification_applications
                        (application_no,user_id,certification_id,route_key,status,updated_at)
                        VALUES(?,?,1,'standard',?, ?)", appNo, uid, status, TestEnv.Stamp(updatedOffset));

    static void SeedCredential(Db db, long uid, string credId, string status, string expiresOffset)
        => db.Execute(@"INSERT INTO issued_credentials(credential_id,user_id,credential,status,expires_at)
                        VALUES(?,?, 'PCP-AI', ?, ?)", credId, uid, status, TestEnv.Stamp(expiresOffset));

    // ================================================================================================
    //  Rule 1 — membership.expiry_reminder : active membership, now < expiry_date <= now+30 days
    // ================================================================================================

    [Fact]
    public void MembershipExpiry_ActiveMemberExpiringInsideThe30DayWindow_QueuesReminder()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, MembershipExpiring);
        // +29 day: strictly inside the 30-day window (and still in the future).
        SeedMembership(db, uid, "active", "+29 day");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(1L, Queued(db, MembershipExpiring));
    }

    [Fact]
    public void MembershipExpiry_ExpiryBeyond30Days_IsNotQueued()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, MembershipExpiring);
        // +31 day: one day past the upper bound → outside the window.
        SeedMembership(db, uid, "active", "+31 day");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(0L, Queued(db, MembershipExpiring));
    }

    [Fact]
    public void MembershipExpiry_ExpiryAlreadyInThePast_IsNotQueued()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, MembershipExpiring);
        // Lower bound is strict: expiry_date must be > now. A still-active row with a past date is excluded here
        // (and, being status='active', also excluded from the membership.expired rule).
        SeedMembership(db, uid, "active", "-1 day");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(0L, Queued(db, MembershipExpiring));
    }

    [Fact]
    public void MembershipExpiry_DeactivatedUser_IsExcluded()
    {
        var db = Fresh();
        // Every rule joins u.status='active'; a deactivated holder never receives a reminder.
        var uid = SeedUser(db, "gone@test.io", status: "deactivated");
        SeedTrigger(db, MembershipExpiring);
        SeedMembership(db, uid, "active", "+10 day");   // squarely in-window; only the user status blocks it.
        CommsReminderService.SweepOnce(db);
        Assert.Equal(0L, Queued(db, MembershipExpiring));
    }

    [Fact]
    public void MembershipExpiry_SecondSweep_DoesNotResend_DailyRunIsIdempotent()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, MembershipExpiring);
        SeedMembership(db, uid, "active", "+10 day");
        // The dedup key (expiry:{uid}:{date}) is stable across sweeps, so a daily cadence can never resend.
        CommsReminderService.SweepOnce(db);
        CommsReminderService.SweepOnce(db);
        Assert.Equal(1L, Queued(db, MembershipExpiring));
    }

    // ================================================================================================
    //  Rule 2 — membership.expired : status='expired', now-30 days < expiry_date <= now
    // ================================================================================================

    [Fact]
    public void MembershipExpired_LapsedWithinTheLast30Days_QueuesNotice()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, MembershipLapsed);
        // -10 day: lapsed recently, inside the 30-day recovery window.
        SeedMembership(db, uid, "expired", "-10 day");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(1L, Queued(db, MembershipLapsed));
    }

    [Fact]
    public void MembershipExpired_LapsedMoreThan30DaysAgo_IsNotQueued()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, MembershipLapsed);
        // -31 day: one day past the -30 day lower bound → outside the window.
        SeedMembership(db, uid, "expired", "-31 day");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(0L, Queued(db, MembershipLapsed));
    }

    // ================================================================================================
    //  Rule 3 — exam.scheduling_deadline_approaching : paid/waived exam, now < deadline <= now+7 days,
    //           excluded once a scheduled/completed booking exists ("book your exam" nudge)
    // ================================================================================================

    [Fact]
    public void ExamScheduling_PaidUnbookedDeadlineWithin7Days_QueuesReminder()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, ExamScheduling);
        SeedExamPayment(db, uid, "+3 day");   // paid exam, deadline inside the 7-day window, no booking yet.
        CommsReminderService.SweepOnce(db);
        Assert.Equal(1L, Queued(db, ExamScheduling));
    }

    [Fact]
    public void ExamScheduling_DeadlineBeyond7Days_IsNotQueued()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, ExamScheduling);
        SeedExamPayment(db, uid, "+10 day");  // outside the 7-day window.
        CommsReminderService.SweepOnce(db);
        Assert.Equal(0L, Queued(db, ExamScheduling));
    }

    [Fact]
    public void ExamScheduling_AlreadyScheduled_IsExcludedFromTheBookYourExamReminder()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, ExamScheduling);
        var pid = SeedExamPayment(db, uid, "+3 day");
        // A scheduled booking satisfies the NOT EXISTS guard → the candidate is dropped: no point nudging
        // someone who has already booked.
        SeedBooking(db, uid, pid, "scheduled");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(0L, Queued(db, ExamScheduling));
    }

    [Fact]
    public void ExamScheduling_CancelledBooking_StillQueues_OnlyScheduledOrCompletedExcludes()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, ExamScheduling);
        var pid = SeedExamPayment(db, uid, "+3 day");
        // The exclusion matches only status IN ('scheduled','completed'); a cancelled booking leaves the
        // candidate un-booked, so the reminder still fires.
        SeedBooking(db, uid, pid, "cancelled");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(1L, Queued(db, ExamScheduling));
    }

    [Fact]
    public void ExamScheduling_ExtendedDeadline_EarnsAFreshReminder()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, ExamScheduling);
        var pid = SeedExamPayment(db, uid, "+3 day");
        // First sweep fires against the original deadline date (dedup key sched:{pid}:{date1}).
        CommsReminderService.SweepOnce(db);
        Assert.Equal(1L, Queued(db, ExamScheduling));
        // Extend the deadline to a DIFFERENT calendar date still inside the 7-day window. The date is part of the
        // dedup key, so the new key (sched:{pid}:{date2}) is distinct and a fresh reminder is queued.
        db.Execute("UPDATE payments SET exam_schedule_deadline = datetime('now','+6 day') WHERE id=?", pid);
        CommsReminderService.SweepOnce(db);
        Assert.Equal(2L, Queued(db, ExamScheduling));
    }

    // ================================================================================================
    //  Rule 4 — application.incomplete_reminder : status='info_requested', updated_at <= now-7 days
    // ================================================================================================

    [Fact]
    public void ApplicationInfoRequested_AwaitingCandidateOver7Days_QueuesReminder()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, AppIncomplete);
        // Waiting on the candidate for 10 days → past the 7-day threshold.
        SeedApplication(db, uid, "APP-1", "info_requested", "-10 day");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(1L, Queued(db, AppIncomplete));
    }

    [Fact]
    public void ApplicationInfoRequested_UpdatedWithinTheLast7Days_IsNotQueued()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, AppIncomplete);
        // Only 3 days stale — inside the grace period, so no nudge yet.
        SeedApplication(db, uid, "APP-2", "info_requested", "-3 day");
        CommsReminderService.SweepOnce(db);
        Assert.Equal(0L, Queued(db, AppIncomplete));
    }

    // ================================================================================================
    //  Rule 5 — cert.recert_due : active credential, now < expires_at <= now+90 days
    // ================================================================================================

    [Fact]
    public void CredentialExpiry_ActiveCredentialWithin90Days_QueuesRecertReminder()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, CredentialRecert);
        SeedCredential(db, uid, "PCP-AI-2026-00001", "active", "+30 day");   // inside the 90-day recert window.
        CommsReminderService.SweepOnce(db);
        Assert.Equal(1L, Queued(db, CredentialRecert));
    }

    [Fact]
    public void CredentialExpiry_ExpiryBeyond90Days_IsNotQueued()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, CredentialRecert);
        SeedCredential(db, uid, "PCP-AI-2026-00002", "active", "+100 day");  // outside the 90-day window.
        CommsReminderService.SweepOnce(db);
        Assert.Equal(0L, Queued(db, CredentialRecert));
    }
}
