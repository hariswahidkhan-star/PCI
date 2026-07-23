using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-5 — the membership-grade (professional standing) core (<see cref="MembershipGrades"/>): the pure
/// grade lookup table (Student → Associate → Professional → Fellow) and the small set of database-backed
/// decisions that drive progression — current grade, whether a member holds an active PCI credential, the
/// self-service upgrade to Professional, Fellowship-application eligibility, and setting a grade. Each test
/// runs against a fresh migrated SQLite database (TestEnv.NewMigratedDb) and seeds only the rows the
/// decision reads. Assertions read the returned Grade record / database state rather than any anonymous
/// snapshot shape. Nothing here changes application code; it pins the behaviour that is already shipping.
/// </summary>
public class MembershipGradesTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static long SeedUser(Db db, string email = "grade@test.io")
    {
        // FK enforcement is ON, so every membership / credential / upgrade row needs a real user first.
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)", email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    // Seed a membership row carrying an explicit grade (the column the grade logic reads). membership_type
    // is the product and is irrelevant here; status is set active for realism.
    static void SeedMembership(Db db, long uid, string grade)
        => db.Execute("INSERT INTO memberships(user_id,membership_type,status,grade) VALUES(?, 'Annual Membership','active',?)", uid, grade);

    // Seed an issued PCI credential. credential_id is UNIQUE NOT NULL, so give each a distinct value.
    static void SeedCredential(Db db, long uid, string status = "active")
        => db.Execute("INSERT INTO issued_credentials(credential_id,user_id,status) VALUES(?,?,?)",
                      "CRED-" + System.Guid.NewGuid().ToString("n"), uid, status);

    static object? Prop(object o, string name) => o.GetType().GetProperty(name)!.GetValue(o);

    // ---- ByKey: known keys, and the associate default ---------------------------------------------

    [Theory]
    [InlineData("student", 0, "SPCI")]
    [InlineData("associate", 1, "APCI")]
    [InlineData("professional", 2, "MPCI")]
    [InlineData("fellow", 3, "FPCI")]
    public void ByKey_KnownKeys_MapToRankAndPostNominal(string key, int rank, string postNominal)
    {
        var g = MembershipGrades.ByKey(key);
        Assert.Equal(key, g.Key);
        Assert.Equal(rank, g.Rank);
        Assert.Equal(postNominal, g.PostNominal);
    }

    [Theory]
    [InlineData("nonsense")]
    [InlineData("")]
    [InlineData(null)]
    public void ByKey_UnknownOrNull_DefaultsToAssociate(string? key)
    {
        // FirstOrDefault(...) ?? All[1] — the fallback is Associate (rank 1), the default paid grade.
        var g = MembershipGrades.ByKey(key);
        Assert.Equal("associate", g.Key);
        Assert.Equal(1, g.Rank);
    }

    // ---- IsGrade ----------------------------------------------------------------------------------

    [Theory]
    [InlineData("student")]
    [InlineData("associate")]
    [InlineData("professional")]
    [InlineData("fellow")]
    public void IsGrade_RealKeys_True(string key)
        => Assert.True(MembershipGrades.IsGrade(key));

    [Theory]
    [InlineData("platinum")]
    [InlineData("Associate")]   // case-sensitive: exact key match only
    [InlineData("")]
    [InlineData(null)]
    public void IsGrade_UnknownOrNull_False(string? key)
        => Assert.False(MembershipGrades.IsGrade(key));

    // ---- CurrentKey -------------------------------------------------------------------------------

    [Fact]
    public void CurrentKey_NoMembershipRow_DefaultsToAssociate()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // No membership row at all → the ?? "associate" fallback applies.
        Assert.Equal("associate", MembershipGrades.CurrentKey(db, uid));
    }

    [Fact]
    public void CurrentKey_ReflectsLatestMembershipGrade()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // Two rows; CurrentKey reads the newest (ORDER BY id DESC), so the second grade wins.
        SeedMembership(db, uid, "student");
        SeedMembership(db, uid, "professional");
        Assert.Equal("professional", MembershipGrades.CurrentKey(db, uid));
    }

    // ---- HoldsActiveCredential --------------------------------------------------------------------

    [Fact]
    public void HoldsActiveCredential_NoCredential_False()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        Assert.False(MembershipGrades.HoldsActiveCredential(db, uid));
    }

    [Fact]
    public void HoldsActiveCredential_ActiveCredential_True()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedCredential(db, uid, "active");
        Assert.True(MembershipGrades.HoldsActiveCredential(db, uid));
    }

    [Theory]
    [InlineData("revoked")]
    [InlineData("expired")]
    public void HoldsActiveCredential_NonActiveStatus_False(string status)
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedCredential(db, uid, status);   // only status='active' counts
        Assert.False(MembershipGrades.HoldsActiveCredential(db, uid));
    }

    // ---- SelfServiceUpgrade -----------------------------------------------------------------------

    [Fact]
    public void SelfServiceUpgrade_AssociateWithNoCredential_ReturnsNull()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, "associate");
        // Rank < 2 but no active credential → no self-service upgrade.
        Assert.Null(MembershipGrades.SelfServiceUpgrade(db, uid));
    }

    [Fact]
    public void SelfServiceUpgrade_AssociateWithActiveCredential_ReturnsProfessional()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, "associate");   // rank 1 < 2
        SeedCredential(db, uid, "active");
        var up = MembershipGrades.SelfServiceUpgrade(db, uid);
        Assert.NotNull(up);
        Assert.Equal("professional", up!.Key);
        Assert.Equal(2, up.Rank);
    }

    [Fact]
    public void SelfServiceUpgrade_AlreadyProfessional_ReturnsNull()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, "professional");   // rank 2 is NOT < 2
        SeedCredential(db, uid, "active");
        // No further self-service step exists above Professional (Fellow is nomination only).
        Assert.Null(MembershipGrades.SelfServiceUpgrade(db, uid));
    }

    // ---- CanApplyForFellow ------------------------------------------------------------------------

    [Theory]
    [InlineData("student")]     // rank 0
    [InlineData("associate")]   // rank 1
    [InlineData("fellow")]      // rank 3 — already Fellow, still not rank 2
    public void CanApplyForFellow_NonProfessionalGrade_False(string grade)
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, grade);
        // The gate is Rank == 2 exactly (Professional), so every other grade is refused.
        Assert.False(MembershipGrades.CanApplyForFellow(db, uid));
    }

    [Fact]
    public void CanApplyForFellow_ProfessionalNoPending_True()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, "professional");
        Assert.True(MembershipGrades.CanApplyForFellow(db, uid));
    }

    [Fact]
    public void CanApplyForFellow_ProfessionalWithPendingApplication_False()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, "professional");
        // A pending Fellowship nomination already exists → cannot apply again.
        db.Execute("INSERT INTO membership_upgrades(user_id,from_grade,to_grade,status) VALUES(?, 'professional','fellow','pending')", uid);
        Assert.False(MembershipGrades.CanApplyForFellow(db, uid));
    }

    // ---- SetGrade ---------------------------------------------------------------------------------

    [Fact]
    public void SetGrade_NoMembershipRow_ReturnsFalse()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // SetGrade updates the latest membership row; with none it fails and creates nothing.
        Assert.False(MembershipGrades.SetGrade(db, uid, "professional"));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM memberships WHERE user_id=?", uid));
        Assert.Equal("associate", MembershipGrades.CurrentKey(db, uid));
    }

    [Fact]
    public void SetGrade_WithMembershipRow_PersistsAndReflectsInCurrentKey()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, "associate");
        Assert.True(MembershipGrades.SetGrade(db, uid, "professional"));
        Assert.Equal("professional", MembershipGrades.CurrentKey(db, uid));
    }

    [Fact]
    public void SetGrade_InvalidKey_Rejected()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, "associate");
        // A grade key that is not one of the four is guarded out; the stored grade is unchanged.
        Assert.False(MembershipGrades.SetGrade(db, uid, "platinum"));
        Assert.Equal("associate", MembershipGrades.CurrentKey(db, uid));
    }

    // ---- Snapshot ---------------------------------------------------------------------------------

    [Fact]
    public void Snapshot_DoesNotThrow_AndReflectsCurrentKey()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedMembership(db, uid, "professional");
        var snap = MembershipGrades.Snapshot(db, uid);   // anonymous object — assert loosely via reflection
        Assert.NotNull(snap);
        Assert.Equal("professional", Prop(snap, "current"));
        Assert.Equal("Professional Member", Prop(snap, "label"));
        Assert.Equal(2, Prop(snap, "rank"));
    }
}
