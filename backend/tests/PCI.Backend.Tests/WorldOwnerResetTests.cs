using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Break-glass owner recovery (PCIWORLD_OWNER_RESET_PASSWORD → WorldSchema.OwnerPasswordReset).
///
/// The production bootstrap prints a random owner password exactly once; when that log line is
/// gone, the operator is locked out of /world-admin with no self-service reset. These tests prove
/// the recovery does everything a recovery must (new hash, lockout cleared, suspension lifted,
/// every session revoked) and nothing it must not (no account creation, no short passwords, no
/// change when the owner is absent).
/// </summary>
public class WorldOwnerResetTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static string OwnerHash(Db db) =>
        (string)db.QueryOne("SELECT password_hash FROM pciworld_admin_users WHERE email='owner@pciworld.local'")!["password_hash"]!;

    static long OwnerId(Db db) =>
        db.Scalar<long>("SELECT id FROM pciworld_admin_users WHERE email='owner@pciworld.local'");

    [Fact]
    public void Reset_sets_the_new_password_and_only_the_new_password()
    {
        var db = NewWorldDb();
        var before = OwnerHash(db);

        Assert.True(WorldSchema.OwnerPasswordReset(db, "recovered-password-123"));

        var after = OwnerHash(db);
        Assert.NotEqual(before, after);
        Assert.True(BCrypt.Net.BCrypt.Verify("recovered-password-123", after));
    }

    [Fact]
    public void Reset_clears_lockout_lifts_suspension_and_revokes_every_session()
    {
        var db = NewWorldDb();
        var id = OwnerId(db);
        // The exact state a locked-out operator's account can be in: mid-lockout (an attacker can
        // trigger that deliberately), suspended, with a live session an attacker might hold.
        db.Execute("UPDATE pciworld_admin_users SET failed_logins=7, lockout_until=datetime('now','+10 minutes'), status='suspended' WHERE id=?", id);
        db.Execute("INSERT INTO pciworld_admin_sessions(admin_id,token,expires_at) VALUES(?, 'stolen-token-sha', datetime('now','+8 hours'))", id);
        Assert.True(LoginGuard.IsLocked(db, "pciworld_admin_users", id));

        Assert.True(WorldSchema.OwnerPasswordReset(db, "recovered-password-123"));

        Assert.False(LoginGuard.IsLocked(db, "pciworld_admin_users", id));
        var row = db.QueryOne("SELECT status, failed_logins FROM pciworld_admin_users WHERE id=?", id)!;
        Assert.Equal("active", Convert.ToString(row["status"]));
        Assert.Equal(0L, Convert.ToInt64(row["failed_logins"]));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_admin_sessions WHERE admin_id=?", id));
    }

    [Fact]
    public void A_password_under_twelve_characters_is_refused_and_nothing_changes()
    {
        var db = NewWorldDb();
        var before = OwnerHash(db);

        Assert.False(WorldSchema.OwnerPasswordReset(db, "elevenchars"));

        Assert.Equal(before, OwnerHash(db));
    }

    [Fact]
    public void Reset_never_creates_an_account_when_the_owner_is_gone()
    {
        var db = NewWorldDb();
        // An operator who renamed or replaced the bootstrap owner: the reset must not resurrect it.
        db.Execute("DELETE FROM pciworld_admin_users WHERE email='owner@pciworld.local'");

        Assert.False(WorldSchema.OwnerPasswordReset(db, "recovered-password-123"));

        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_admin_users WHERE email='owner@pciworld.local'"));
    }
}
