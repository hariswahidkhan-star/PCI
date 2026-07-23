using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-11 — direct unit coverage of the two credential-lifecycle decision cores the live-HTTP suite only
/// touches through the happy path: <see cref="Lifecycle.IssueCredential"/> (certificate-number assignment,
/// idempotency, route snapshotting) and <see cref="CpdPolicy"/> (the recertification CPD gate — the total
/// vs AI-subset requirement and the cycle-window filter). Each test runs against a fresh migrated SQLite
/// database (TestEnv.NewMigratedDb) — the same schema CI migrates — with foreign keys ON, so every test
/// seeds a real user (and, where the code reads it, a real exam attempt) before exercising the decision.
/// Assertions read the issued_credentials table / the returned Status record rather than any transient
/// object. No application code is changed; this pins behaviour that is already shipping.
/// </summary>
public class CredentialCpdTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static long SeedUser(Db db, string email = "cred@test.io")
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)", email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    // issued_credentials.attempt_id has a FK to exam_attempts(id) and FKs are ON, so a non-null attempt id
    // must reference a real attempt row. exam_attempts only requires user_id; everything else defaults.
    static long SeedAttempt(Db db, long uid) => db.ExecuteReturningId("INSERT INTO exam_attempts(user_id) VALUES(?)", uid);

    static Dictionary<string, object?> Row(Db db, string cid) => db.QueryOne("SELECT * FROM issued_credentials WHERE credential_id=?", cid)!;

    static int Suffix(string cid) => int.Parse(cid[^6..]);

    // ForCredential reads only certification_id + expires_at off the credential dictionary, so the CPD tests
    // build one directly with a chosen expiry — the cert's 3-year cycle window then straddles known dates.
    static Dictionary<string, object?> Cred(string expiresAt = "2030-01-01 00:00:00", long certId = 1)
        => new() { ["certification_id"] = certId, ["expires_at"] = expiresAt };

    static void SetCpd(Db db, double required, double aiRequired = 0, long certId = 1)
        => db.Execute("UPDATE certifications SET cpd_required_hours=?, cpd_ai_hours_required=? WHERE id=?", required, aiRequired, certId);

    // Default date sits well inside the cycle window for a credential expiring 2030-01-01 (cycle start ~2027).
    static void SeedCpd(Db db, long uid, double hours, string status = "approved",
                        string? category = "Cost & planning", string? date = "2029-06-01")
        => db.Execute("INSERT INTO cpd_entries(user_id,activity_date,category,hours,status) VALUES(?,?,?,?,?)",
                      uid, date, category, hours, status);

    // ================================ Lifecycle.IssueCredential ================================

    [Fact]
    public void IssueCredential_RecordsActiveRow_WithFormattedNumber()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var att = SeedAttempt(db, uid);
        var cid = Lifecycle.IssueCredential(db, uid, att, "Ada Holder");

        Assert.NotNull(cid);
        var yr = DateTime.UtcNow.Year;
        // Certificate number: PCI-<cleanprefix>-<year>-<6 digits>. Cert 1's prefix is PCL-AI -> PCLAI.
        Assert.StartsWith($"PCI-PCLAI-{yr}-", cid);
        Assert.EndsWith("000001", cid!);   // first credential in a fresh DB

        var r = Row(db, cid!);
        Assert.Equal("active", H.Str(r["status"]));
        Assert.Equal("Ada Holder", H.Str(r["holder_name"]));
        Assert.Equal(1L, H.L(r["certification_id"]));
        Assert.Equal("PCL-AI", H.Str(r["credential"]));
        Assert.Equal(att, H.L(r["attempt_id"]));
    }

    [Fact]
    public void IssueCredential_ExpiresThreeYearsOut_IsRecorded()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var att = SeedAttempt(db, uid);
        var cid = Lifecycle.IssueCredential(db, uid, att, "Ada Holder");
        var expires = H.Str(Row(db, cid!)["expires_at"]);
        Assert.False(string.IsNullOrEmpty(expires));
        // Cert 1 has expiry_years=3, so the credential must expire in the future, not today.
        Assert.False(H.IsPast(expires));
    }

    [Fact]
    public void IssueCredential_SecondCallSameAttempt_IsIdempotent()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var att = SeedAttempt(db, uid);
        var first = Lifecycle.IssueCredential(db, uid, att, "Ada Holder");
        var second = Lifecycle.IssueCredential(db, uid, att, "Ada Holder");
        // attempt_id is the idempotency key: the second call returns the SAME number and issues no new row.
        Assert.Equal(first, second);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM issued_credentials WHERE attempt_id=?", att));
    }

    [Fact]
    public void IssueCredential_SequentialNumbering_AcrossTwoUsers()
    {
        var db = Fresh();
        var uidA = SeedUser(db, "a@cred.io");
        var uidB = SeedUser(db, "b@cred.io");
        var cidA = Lifecycle.IssueCredential(db, uidA, SeedAttempt(db, uidA), "Alpha");
        var cidB = Lifecycle.IssueCredential(db, uidB, SeedAttempt(db, uidB), "Beta");

        Assert.NotEqual(cidA, cidB);                     // no collision
        Assert.Equal(Suffix(cidA!) + 1, Suffix(cidB!));  // the running number advances by one
        Assert.Equal(2L, db.Scalar<long>("SELECT COUNT(*) FROM issued_credentials"));
    }

    [Fact]
    public void IssueCredential_RouteMarker_AppearsInCertificateNumber()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var att = SeedAttempt(db, uid);
        // A route marker is upper-cased and inserted into the number's own sub-space: PCI-PCLAI-FND-<year>-...
        var cid = Lifecycle.IssueCredential(db, uid, att, "Ada Holder", routeMarker: "fnd");
        var yr = DateTime.UtcNow.Year;
        Assert.StartsWith($"PCI-PCLAI-FND-{yr}-", cid);
    }

    [Fact]
    public void IssueCredential_RouteWording_SnapshottedFromEntitlementRoute()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var att = SeedAttempt(db, uid);
        // The 'founding' route exists for every cert (EnsureRoutes); give it certificate wording to snapshot.
        const string wording = "Awarded under the PCI Founding Route.";
        db.Execute("UPDATE certification_routes SET certificate_wording=? WHERE certification_id=1 AND route_key='founding'", wording);
        // A route-granted entitlement carries the route_key that drives the snapshot.
        db.Execute("INSERT INTO exam_entitlements(user_id,attempt_id,route_key,product_type,status) VALUES(?,?,?,?,?)",
                   uid, att, "founding", "exam", "available");

        var cid = Lifecycle.IssueCredential(db, uid, att, "Ada Holder");
        var r = Row(db, cid!);
        Assert.Equal("founding", H.Str(r["route_key"]));
        Assert.Equal(wording, H.Str(r["certificate_wording"]));
    }

    [Fact]
    public void IssueCredential_NoEntitlementRoute_LeavesWordingNull()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var att = SeedAttempt(db, uid);
        // An ordinary paid entitlement has no route_key => default wording (NULL snapshot on the row).
        var cid = Lifecycle.IssueCredential(db, uid, att, "Ada Holder");
        var r = Row(db, cid!);
        Assert.Null(r["route_key"]);
        Assert.Null(r["certificate_wording"]);
    }

    [Fact]
    public void IssueCredential_NullAttempt_IssuesButIsNotIdempotent()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // With no attempt there is no idempotency key, so each call mints a fresh sequential number.
        var first = Lifecycle.IssueCredential(db, uid, null, "Ada Holder");
        var second = Lifecycle.IssueCredential(db, uid, null, "Ada Holder");
        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.NotEqual(first, second);
        Assert.Equal(2L, db.Scalar<long>("SELECT COUNT(*) FROM issued_credentials WHERE user_id=?", uid));
    }

    // ================================ CpdPolicy.ForCredential ================================

    [Fact]
    public void ForCredential_NoRequirement_IsAlwaysMet()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // Cert 1 defaults to cpd_required_hours=0 / cpd_ai_hours_required=0 — no requirement.
        var s = CpdPolicy.ForCredential(db, uid, Cred());
        Assert.False(s.HasRequirement);
        Assert.True(s.Met);
    }

    [Fact]
    public void ForCredential_ApprovedEqualsRequired_BoundarySatisfied()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SetCpd(db, required: 10);
        SeedCpd(db, uid, 10);                 // exactly the required hours, dated inside the cycle
        var s = CpdPolicy.ForCredential(db, uid, Cred());
        Assert.True(s.HasRequirement);
        Assert.Equal(10, s.Required, 3);
        Assert.Equal(10, s.Approved, 3);
        Assert.True(s.Met);                   // meeting the bar exactly satisfies (epsilon boundary)
    }

    [Fact]
    public void ForCredential_JustBelowRequired_NotMet()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SetCpd(db, required: 10);
        SeedCpd(db, uid, 9.5);
        var s = CpdPolicy.ForCredential(db, uid, Cred());
        Assert.Equal(9.5, s.Approved, 3);
        Assert.False(s.Met);
    }

    [Fact]
    public void ForCredential_TotalMetButAiSubsetUnmet_Blocks()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SetCpd(db, required: 10, aiRequired: 4);
        SeedCpd(db, uid, 2, category: CpdPolicy.AiCategory);   // only 2 AI-currency hours
        SeedCpd(db, uid, 10, category: "Cost & planning");     // plenty of general hours
        var s = CpdPolicy.ForCredential(db, uid, Cred());
        Assert.Equal(12, s.Approved, 3);   // total requirement is met...
        Assert.Equal(2, s.AiApproved, 3);
        Assert.Equal(4, s.AiRequired, 3);
        Assert.False(s.AiMet);             // ...but the mandatory AI-currency subset is not
        Assert.False(s.Met);               // Met requires BOTH
    }

    [Fact]
    public void ForCredential_AiSubsetMet_Passes()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SetCpd(db, required: 10, aiRequired: 4);
        SeedCpd(db, uid, 6, category: CpdPolicy.AiCategory);   // AI subset satisfied
        SeedCpd(db, uid, 6, category: "Cost & planning");
        var s = CpdPolicy.ForCredential(db, uid, Cred());
        Assert.Equal(12, s.Approved, 3);
        Assert.Equal(6, s.AiApproved, 3);
        Assert.True(s.AiMet);
        Assert.True(s.Met);
    }

    [Fact]
    public void ForCredential_OutOfWindowEntry_ExcludedFromApproved()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SetCpd(db, required: 10);
        SeedCpd(db, uid, 20, date: "2020-01-01");   // dated years before the cycle start -> excluded
        SeedCpd(db, uid, 5, date: "2029-06-01");    // inside the cycle -> counted
        var s = CpdPolicy.ForCredential(db, uid, Cred());
        Assert.Equal(5, s.Approved, 3);             // only the in-window entry contributes
        Assert.False(s.Met);
    }

    [Fact]
    public void ForCredential_UndatedApprovedEntry_Counts()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SetCpd(db, required: 10);
        SeedCpd(db, uid, 10, date: null);           // undated approved entries count toward the total
        var s = CpdPolicy.ForCredential(db, uid, Cred());
        Assert.Equal(10, s.Approved, 3);
        Assert.True(s.Met);
    }

    [Fact]
    public void ForCredential_NonApprovedEntries_DoNotCount()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SetCpd(db, required: 10);
        SeedCpd(db, uid, 20, status: "recorded");   // pending review — never counts
        SeedCpd(db, uid, 20, status: "rejected");   // rejected — never counts
        var s = CpdPolicy.ForCredential(db, uid, Cred());
        Assert.Equal(0, s.Approved, 3);
        Assert.False(s.Met);
    }

    // ================================ CpdPolicy.ForUserCert ================================

    [Fact]
    public void ForUserCert_NoCredential_ReturnsNull()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        Assert.Null(CpdPolicy.ForUserCert(db, uid, 1));
    }

    [Fact]
    public void ForUserCert_WithActiveCredential_ReturnsStatus()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SetCpd(db, required: 10);
        db.Execute("INSERT INTO issued_credentials(credential_id,user_id,certification_id,status,expires_at,holder_name) VALUES(?,?,?,?,?,?)",
                   "PCI-PCLAI-2030-000001", uid, 1, "active", "2030-01-01 00:00:00", "Ada Holder");
        SeedCpd(db, uid, 10);
        var s = CpdPolicy.ForUserCert(db, uid, 1);
        Assert.NotNull(s);
        Assert.Equal(10, s!.Approved, 3);
        Assert.True(s.Met);
    }

    [Fact]
    public void ForUserCert_OnlyRevokedCredential_ReturnsNull()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        db.Execute("INSERT INTO issued_credentials(credential_id,user_id,certification_id,status,expires_at,holder_name) VALUES(?,?,?,?,?,?)",
                   "PCI-PCLAI-2030-000009", uid, 1, "revoked", "2030-01-01 00:00:00", "Ada Holder");
        // A revoked credential is not one the holder would recertify -> treated as no credential.
        Assert.Null(CpdPolicy.ForUserCert(db, uid, 1));
    }
}
