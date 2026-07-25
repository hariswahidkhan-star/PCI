using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-9 — direct unit coverage of the exam authorization core (<see cref="ExamAuthorization"/>): the
/// eight-scope scheduling-window resolver (<see cref="ExamAuthorization.ResolveWindow"/>) and its per-field
/// first-non-null mixing; the deadline write-through that removes the hardcoded +1 year
/// (<see cref="ExamAuthorization.EnsureForPayment"/>); the cert-retarget sync that must never clobber a
/// manual extension (<see cref="ExamAuthorization.SyncCert"/>); the attempt counters; and the
/// waiting-period waiver. Each test runs against a fresh migrated SQLite database (TestEnv.NewMigratedDb —
/// the same schema CI migrates), seeds a real user first (FK enforcement is ON), and asserts on the returned
/// Window fields, database state, or counts. No application code is changed; this pins shipping behaviour.
/// </summary>
public class ExamAuthorizationTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    const string Email = "exauth@test.io";

    static long SeedUser(Db db, string email = Email)
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)", email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    // A single exam_window_rules row at a given scope. Unset fields (null) are what exercises per-field mixing.
    static void SeedRule(Db db, string scopeType, string? scopeValue, int? windowDays,
        int? expiryDays = null, int? attempts = null, int? retakeWait = null, int active = 1)
        => db.Execute(@"INSERT INTO exam_window_rules(scope_type,scope_value,window_days,access_expiry_days,attempts_permitted,retake_wait_days,active)
            VALUES(?,?,?,?,?,?,?)", scopeType, scopeValue, windowDays, expiryDays, attempts, retakeWait, active);

    static Dictionary<string, object?> Auth(Db db, long payId) =>
        db.QueryOne("SELECT * FROM exam_authorizations WHERE payment_id=?", payId)!;

    static Dictionary<string, object?> Payment(Db db, long payId) =>
        db.QueryOne("SELECT * FROM payments WHERE id=?", payId)!;

    // Whole days between two SQLite datetime strings, compared by parsed instant (H.JsMillis) — the same
    // math the window resolver uses, so a 180-day window reads back as exactly 180.
    static double DaysBetween(string? from, string? to) => (H.JsMillis(to) - H.JsMillis(from)) / 86_400_000.0;

    // ---- ResolveWindow: precedence + per-field mixing ---------------------------------------------

    [Fact]
    public void ResolveWindow_NoRules_UsesSiteSettingDefaults()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // No exam_window_rules seeded → every field falls through to the seeded site_settings defaults.
        var w = ExamAuthorization.ResolveWindow(db, uid, 1);
        Assert.Equal(365, w.WindowDays);
        Assert.Equal(1, w.AttemptsPermitted);
        Assert.Equal(0, w.RetakeWaitDays);
        Assert.Null(w.AccessExpiryDays);           // exam_default_access_expiry_days is seeded empty → null
        Assert.Equal("default", w.Source);
    }

    [Fact]
    public void ResolveWindow_CertificationScope_SuppliesEveryFieldAndSource()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedRule(db, "certification", "1", windowDays: 200, expiryDays: 150, attempts: 3, retakeWait: 7);
        var w = ExamAuthorization.ResolveWindow(db, uid, 1);
        Assert.Equal(200, w.WindowDays);
        Assert.Equal(3, w.AttemptsPermitted);
        Assert.Equal(7, w.RetakeWaitDays);
        Assert.Equal(150, w.AccessExpiryDays!.Value);
        Assert.Equal("certification:1", w.Source);
    }

    [Fact]
    public void ResolveWindow_IndividualScope_OverridesCertification()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedRule(db, "certification", "1", windowDays: 200);
        SeedRule(db, "individual", uid.ToString(), windowDays: 30);
        // individual is the most-specific scope, so its window_days wins over the certification rule.
        var w = ExamAuthorization.ResolveWindow(db, uid, 1);
        Assert.Equal(30, w.WindowDays);
        Assert.Equal($"individual:{uid}", w.Source);
    }

    [Fact]
    public void ResolveWindow_PerFieldMixing_UnsetFieldFallsThroughToLowerScope()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // High-precedence (individual) rule sets ONLY window_days; every other field is null on it.
        SeedRule(db, "individual", uid.ToString(), windowDays: 30);
        // Lower-precedence (certification) rule supplies the rest.
        SeedRule(db, "certification", "1", windowDays: 200, expiryDays: 400, attempts: 5, retakeWait: 14);
        var w = ExamAuthorization.ResolveWindow(db, uid, 1);
        Assert.Equal(30, w.WindowDays);                 // from individual (highest scope that set it)
        Assert.Equal(5, w.AttemptsPermitted);           // fell through to certification
        Assert.Equal(14, w.RetakeWaitDays);             // fell through to certification
        Assert.Equal(400, w.AccessExpiryDays!.Value);   // fell through to certification
        Assert.Equal($"individual:{uid}", w.Source);    // source tracks where window_days came from
    }

    [Fact]
    public void ResolveWindow_ExamScope_OutranksCertificationScope()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // Both scopes are keyed by the certification id, but 'exam' sits above 'certification' in precedence.
        SeedRule(db, "certification", "1", windowDays: 200);
        SeedRule(db, "exam", "1", windowDays: 100);
        var w = ExamAuthorization.ResolveWindow(db, uid, 1);
        Assert.Equal(100, w.WindowDays);
        Assert.Equal("exam:1", w.Source);
    }

    [Fact]
    public void ResolveWindow_RouteScope_AppliesOnlyWhenRouteKeyProvided()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedRule(db, "certification", "1", windowDays: 200);
        SeedRule(db, "route", "fast-track", windowDays: 120);
        // With the route key, the route rule (above certification) wins.
        var withRoute = ExamAuthorization.ResolveWindow(db, uid, 1, routeKey: "fast-track");
        Assert.Equal(120, withRoute.WindowDays);
        Assert.Equal("route:fast-track", withRoute.Source);
        // Without a route key, the route scope is skipped entirely and certification supplies the window.
        var noRoute = ExamAuthorization.ResolveWindow(db, uid, 1);
        Assert.Equal(200, noRoute.WindowDays);
        Assert.Equal("certification:1", noRoute.Source);
    }

    [Fact]
    public void ResolveWindow_Campaign_Institution_Country_Precedence()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedRule(db, "country", "GB", windowDays: 210);
        SeedRule(db, "institution", "55", windowDays: 150);
        SeedRule(db, "campaign", "launch2026", windowDays: 90);
        // Precedence among these three: campaign > institution > country.
        var all = ExamAuthorization.ResolveWindow(db, uid, 1, country: "GB", institutionId: 55, campaign: "launch2026");
        Assert.Equal(90, all.WindowDays);
        Assert.Equal("campaign:launch2026", all.Source);
        // Drop the campaign → institution becomes the highest scope that resolves.
        var noCampaign = ExamAuthorization.ResolveWindow(db, uid, 1, country: "GB", institutionId: 55);
        Assert.Equal(150, noCampaign.WindowDays);
        Assert.Equal("institution:55", noCampaign.Source);
    }

    [Fact]
    public void ResolveWindow_InactiveRule_IsIgnored()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A deactivated individual rule must not shadow the active certification rule (WHERE active=1).
        SeedRule(db, "individual", uid.ToString(), windowDays: 30, active: 0);
        SeedRule(db, "certification", "1", windowDays: 200);
        var w = ExamAuthorization.ResolveWindow(db, uid, 1);
        Assert.Equal(200, w.WindowDays);
        Assert.Equal("certification:1", w.Source);
    }

    // ---- EnsureForPayment: configured deadline written through, not the hardcoded +1 year ----------

    [Fact]
    public void EnsureForPayment_NonDefaultRule_WritesResolvedDeadlineThrough()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A 180-day rule must exist BEFORE the seat is created so the authorization is computed under it.
        SeedRule(db, "certification", "1", windowDays: 180);
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-1", "admin_manual");
        var authId = ExamAuthorization.EnsureForPayment(db, pid);   // idempotent — returns the seat Grant made
        Assert.True(authId > 0);

        var a = Auth(db, pid);
        Assert.Equal(180L, H.L(a["window_days"]));
        Assert.Equal("certification:1", H.Str(a["window_source"]));
        // The resolved deadline is exactly the 180-day window from eligibility start — NOT the generic +1 year.
        var days = DaysBetween(H.Str(a["eligibility_start"]), H.Str(a["current_deadline"]));
        Assert.Equal(180d, days, 0);
        Assert.True(days < 300, "a configured 180-day rule must not leave the hardcoded ~365-day deadline");
        // The deadline is written THROUGH to the payment the booking gate reads.
        Assert.Equal(H.Str(a["current_deadline"]), H.Str(Payment(db, pid)["exam_schedule_deadline"]));
    }

    [Fact]
    public void EnsureForPayment_IsIdempotent()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-2", "admin_manual");
        var authId = ExamAuthorization.EnsureForPayment(db, pid);
        var again = ExamAuthorization.EnsureForPayment(db, pid);
        Assert.Equal(authId, again);
        // ux_examauth_payment guarantees exactly one authorization per payment no matter how often it runs.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM exam_authorizations WHERE payment_id=?", pid));
    }

    // ---- SyncCert: recompute on retarget, but never over a manual extension ------------------------

    [Fact]
    public void SyncCert_UnextendedAuthorization_RecomputesWindowForNewCert()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedRule(db, "certification", "2", windowDays: 90);       // the retarget cert has a 90-day rule
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-3", "admin_manual");
        ExamAuthorization.EnsureForPayment(db, pid);

        ExamAuthorization.SyncCert(db, pid, 2);

        var a = Auth(db, pid);
        Assert.Equal(2L, H.L(a["certification_id"]));            // re-pointed
        Assert.Equal(90L, H.L(a["window_days"]));               // recomputed from cert 2's rule
        Assert.Equal("certification:2", H.Str(a["window_source"]));
        Assert.Equal(90d, DaysBetween(H.Str(a["eligibility_start"]), H.Str(a["current_deadline"])), 0);
        // Payment deadline follows the recomputed authorization deadline.
        Assert.Equal(H.Str(a["current_deadline"]), H.Str(Payment(db, pid)["exam_schedule_deadline"]));
    }

    [Fact]
    public void SyncCert_ManuallyExtendedAuthorization_RepointsCertButKeepsDeadline()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedRule(db, "certification", "2", windowDays: 90);       // would shorten the deadline if applied
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-4", "admin_manual");
        var authId = ExamAuthorization.EnsureForPayment(db, pid);

        // Simulate a manual extension: a pushed-out deadline plus a preserved extension-history row.
        const string extended = "2099-01-01 00:00:00";
        db.Execute("UPDATE exam_authorizations SET current_deadline=? WHERE id=?", extended, authId);
        db.Execute("INSERT INTO exam_extension_history(authorization_id,user_id) VALUES(?,?)", authId, uid);

        ExamAuthorization.SyncCert(db, pid, 2);

        var a = Auth(db, pid);
        Assert.Equal(2L, H.L(a["certification_id"]));           // cert still re-pointed
        Assert.Equal(extended, H.Str(a["current_deadline"]));  // extension left untouched (never overridden)
    }

    // ---- AttemptsUsed / AttemptsPermitted counters ------------------------------------------------

    [Fact]
    public void AttemptsUsed_CountsOnlySubmittedCountingExamSittings()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        long Insert(int cert, string kind, string status, object? counts) =>
            db.ExecuteReturningId("INSERT INTO exam_attempts(user_id,certification_id,kind,status,counts_as_attempt) VALUES(?,?,?,?,?)",
                uid, cert, kind, status, counts);

        Insert(1, "exam", "submitted", 1);         // counts
        Insert(1, "exam", "submitted", null);      // counts — COALESCE(counts_as_attempt,1)=1
        Insert(1, "exam", "in_progress", 1);       // excluded — not submitted
        Insert(1, "exam", "submitted", 0);         // excluded — flagged not-counting (e.g. verified tech failure)
        Insert(1, "practice", "submitted", 1);     // excluded — practice, not an exam sitting

        Assert.Equal(2, ExamAuthorization.AttemptsUsed(db, uid, 1));
        Assert.Equal(0, ExamAuthorization.AttemptsUsed(db, uid, 2));   // scoped by certification
    }

    [Fact]
    public void AttemptsPermitted_CountsPaidAndWaivedSeats()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-5a", "admin_manual");   // paid seat
        Settlement.Grant(db, uid, Email, "exam", 1, amount: 0, "EA-5b", "admin_waiver");     // waived seat
        // Both settled entitlements are permitted sittings for the cert.
        Assert.Equal(2, ExamAuthorization.AttemptsPermitted(db, uid, 1));
    }

    [Fact]
    public void AttemptsPermitted_ExcludesRefundedEntitlement()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-6", "admin_manual");
        Assert.Equal(1, ExamAuthorization.AttemptsPermitted(db, uid, 1));
        db.Execute("UPDATE exam_entitlements SET status='refunded' WHERE payment_id=?", pid);
        Assert.Equal(0, ExamAuthorization.AttemptsPermitted(db, uid, 1));   // refunded seats are not permitted
    }

    // ---- Retake waiting period: persisted on a retake seat, enforced, and waivable (P0-7 / DEF-2) --

    [Fact]
    public void EnsureForPayment_FirstSeat_NoPriorFailure_LeavesRetakeWaitNull()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // A rule configures a 45-day retake wait; the resolver reports it faithfully...
        SeedRule(db, "certification", "1", windowDays: 180, retakeWait: 45);
        Assert.Equal(45, ExamAuthorization.ResolveWindow(db, uid, 1).RetakeWaitDays);

        // ...but the FIRST seat has no prior failed sitting, so there is nothing to wait for → stays NULL.
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-7", "admin_manual");
        ExamAuthorization.EnsureForPayment(db, pid);
        var a = Auth(db, pid);
        Assert.Equal(180L, H.L(a["window_days"]));       // proves the authorization was built under the rule
        Assert.Null(a["retake_wait_until"]);             // no prior failure ⇒ no cool-off on the first seat
    }

    [Fact]
    public void EnsureForPayment_RetakeAfterFailedSitting_PersistsRetakeWaitUntil()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedRule(db, "certification", "1", windowDays: 180, retakeWait: 45);
        // The candidate has a finalized FAILED sitting for this certification (submitted just now)...
        db.Execute("INSERT INTO exam_attempts(user_id,certification_id,kind,status,result,counts_as_attempt,submitted_at) VALUES(?,?, 'exam','submitted','fail',1, datetime('now'))", uid, 1);
        // ...so the NEXT (retake) seat's authorization is stamped with a cool-off ~45 days out.
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-7b", "admin_manual");
        ExamAuthorization.EnsureForPayment(db, pid);
        var a = Auth(db, pid);
        var wait = H.Str(a["retake_wait_until"]);
        Assert.False(string.IsNullOrEmpty(wait));                 // the cool-off is now PERSISTED, not inert
        var days = DaysBetween(H.IsoNow, wait);
        Assert.True(days is > 44 and < 46, $"retake wait should be ~45 days out, was {days}");
    }

    [Fact]
    public void EnsureForPayment_RetakeAfterHeldSitting_DoesNotStartWait()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedRule(db, "certification", "1", windowDays: 180, retakeWait: 45);
        // A held attempt is not a finalized failure (result pending review) → no cool-off is started.
        db.Execute("INSERT INTO exam_attempts(user_id,certification_id,kind,status,result,result_status,counts_as_attempt,submitted_at) VALUES(?,?, 'exam','submitted','fail','auto_held',1, datetime('now'))", uid, 1);
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-7c", "admin_manual");
        ExamAuthorization.EnsureForPayment(db, pid);
        Assert.Null(Auth(db, pid)["retake_wait_until"]);
    }

    [Fact]
    public void WaiveWaitingPeriod_ClearsManuallySetWaitOnActiveAuthorization()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-8", "admin_manual");
        ExamAuthorization.EnsureForPayment(db, pid);
        // EnsureForPayment now persists a retake wait after a failure; here we set it directly (no prior
        // failure in this test) to exercise the waiver's one job: clearing it on the live authorization.
        db.Execute("UPDATE exam_authorizations SET retake_wait_until=datetime('now','+30 day') WHERE payment_id=?", pid);
        Assert.NotNull(Auth(db, pid)["retake_wait_until"]);

        ExamAuthorization.WaiveWaitingPeriod(db, uid, 1, approverId: 1);
        Assert.Null(Auth(db, pid)["retake_wait_until"]);
    }

    [Fact]
    public void WaiveWaitingPeriod_LeavesNonActiveAuthorizationUntouched()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var pid = Settlement.Grant(db, uid, Email, "exam", 1, amount: 500, "EA-9", "admin_manual");
        ExamAuthorization.EnsureForPayment(db, pid);
        // A non-active authorization is outside the waiver's WHERE status='active' scope.
        db.Execute("UPDATE exam_authorizations SET retake_wait_until=datetime('now','+30 day'), status='expired' WHERE payment_id=?", pid);

        ExamAuthorization.WaiveWaitingPeriod(db, uid, 1, approverId: 1);
        Assert.NotNull(Auth(db, pid)["retake_wait_until"]);   // untouched — only active authorizations are waived
    }
}
