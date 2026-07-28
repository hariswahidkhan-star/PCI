using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World registration must never report success on a split identity (issue ID-08).
///
/// The path used to be `try { WorldIdentity.MapOne(db, id); } catch { }`, annotated "mapping must
/// never break registration". Swallowing the failure is what broke it: the World account existed,
/// the canonical identity did not, registration returned OK, and nothing noticed until some later
/// read expected an identity that was never created.
///
/// The replacement draws a line between two outcomes that the single catch had conflated:
///
///   • a genuine failure to establish identity  → nothing is created, registration fails
///   • a quarantined email collision            → the account IS created, in identity_link_pending,
///                                                and cannot publish a Passport until ownership is proven
/// </summary>
public class WorldIdentityLinkTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    [Fact]
    public void A_standalone_registration_is_linked_and_carries_a_canonical_student_number()
    {
        var db = NewWorldDb();

        var (err, wid, _) = WorldAccount.Register(db, "solo@x.test", "long-password-1", "Solo", null);

        Assert.Null(err);
        Assert.False(WorldIdentity.LinkPending(db, wid));
        var canonical = WorldIdentity.CanonicalUserFor(db, wid);
        Assert.NotNull(canonical);
        // The canonical identity is a full PCI student: it has the one canonical Student Number,
        // issued in the creating transaction rather than lazily on some later read.
        Assert.StartsWith("PCI-", StudentNumbers.Read(db, canonical!.Value));
    }

    [Fact]
    public void An_email_collision_is_quarantined_into_link_pending_not_merged()
    {
        var db = NewWorldDb();
        // An existing canonical PCI account. Registering the same email on World must not attach
        // to it on a string match alone — that would hand one person's evidence to another.
        db.ExecuteReturningId(@"INSERT INTO users(email,first_name,last_name,password_hash,role,status)
            VALUES('taken@x.test','Ex','Isting',?,'student','active')", BCrypt.Net.BCrypt.HashPassword("portal-pw-1"));

        var (err, wid, _) = WorldAccount.Register(db, "taken@x.test", "long-password-1", "Imposter", null);

        // The account is created — this is a recoverable state, not a rejection.
        Assert.Null(err);
        Assert.True(wid > 0);
        Assert.True(WorldIdentity.LinkPending(db, wid));
        Assert.Null(WorldIdentity.CanonicalUserFor(db, wid));
    }

    [Fact]
    public void A_link_pending_account_cannot_publish_a_passport()
    {
        var db = NewWorldDb();
        db.ExecuteReturningId(@"INSERT INTO users(email,first_name,last_name,password_hash,role,status)
            VALUES('collide@x.test','Ex','Isting',?,'student','active')", BCrypt.Net.BCrypt.HashPassword("portal-pw-1"));
        var (_, wid, _) = WorldAccount.Register(db, "collide@x.test", "long-password-1", "Pending", null);
        // Satisfy every OTHER publication precondition, so the only thing left refusing is identity.
        db.Execute("UPDATE pciworld_users SET email_verified=1, display_name='Pending' WHERE id=?", wid);

        var (err, url) = WorldAccount.PublishPassport(db, wid);

        Assert.Equal("identity_link_pending", err);
        Assert.Null(url);
        Assert.Equal(0, db.Scalar<long>("SELECT passport_public FROM pciworld_users WHERE id=?", wid));
    }

    [Fact]
    public void A_linked_account_is_not_blocked_by_the_identity_gate()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "ok@x.test", "long-password-1", "Fine", null);
        db.Execute("UPDATE pciworld_users SET email_verified=1, display_name='Fine' WHERE id=?", wid);

        var (err, _) = WorldAccount.PublishPassport(db, wid);

        // It still fails — on evidence, which is the correct next gate — but never on identity.
        Assert.NotEqual("identity_link_pending", err);
    }

    [Fact]
    public void A_failed_registration_leaves_no_half_built_account_behind()
    {
        // SQLite only. The forcing function below is DDL, and on MySQL two things go wrong: DDL
        // implicitly commits (so the rollback under test would not be the thing observed), and
        // TestEnv shares one template database across the suite, so the drop would leak into every
        // later test. The behaviour is provider-independent; the way of provoking it is not.
        if (TestEnv.IsMySql) return;

        var db = NewWorldDb();
        // Force MapOne to fail from inside the registration transaction by removing the table its
        // canonical-creation path writes to. Anything that makes identity unestablishable will do;
        // what is under test is that the World row does not survive the failure.
        db.Exec("DROP TABLE student_profiles");

        var (err, wid, token) = WorldAccount.Register(db, "doomed@x.test", "long-password-1", "Doomed", null);

        Assert.Equal("identity_unavailable", err);
        Assert.Equal(0, wid);
        Assert.Equal("", token);
        // The whole point: no orphaned World account, and therefore no identity to be split later.
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_users WHERE email='doomed@x.test'"));
    }
}
