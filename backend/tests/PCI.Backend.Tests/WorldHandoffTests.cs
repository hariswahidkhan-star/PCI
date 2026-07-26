using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — one-time cross-surface handoff and canonical step-up (journey repair P0-02 / P0-03).
///
/// The portal→World bridge no longer transports a reusable 30-day bearer token: it mints a hashed,
/// two-minute, single-consumption code. These tests pin the consumption semantics (once and only
/// once, replay/expiry/never-existed indistinguishable, raw code never stored), the open-redirect
/// allow-list on the return path, the canonical-credential step-up for sensitive actions (a
/// bridge-created account's hidden random World password can never be the only key), and the
/// WORLD-ONLY deletion scope (the canonical identity and student profile survive untouched).
/// </summary>
public class WorldHandoffTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long CanonicalUser(Db db, string email) =>
        db.ExecuteReturningId(@"INSERT INTO users(email,first_name,last_name,password_hash,role,status)
            VALUES(?,?,?,?,'student','active')", email, "Cano", "Nical", BCrypt.Net.BCrypt.HashPassword("portal-password-1"));

    [Fact]
    public void A_handoff_code_redeems_exactly_once_and_is_stored_only_as_a_hash()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "hand@x.test", "long-password-1", "H", null);
        var code = WorldAccount.CreateHandoff(db, wid, "/world/account");

        // The raw code exists nowhere in the database.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_handoff_codes WHERE code_sha=?", code));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_handoff_codes WHERE code_sha=?", Security.Sha(code)));

        var first = WorldAccount.RedeemHandoff(db, code);
        Assert.Null(first.Error);
        Assert.Equal(wid, first.UserId);
        Assert.Equal("/world/account", first.ReturnTo);

        // Replay, garbage and empty are indistinguishable failures.
        Assert.Equal("invalid_code", WorldAccount.RedeemHandoff(db, code).Error);
        Assert.Equal("invalid_code", WorldAccount.RedeemHandoff(db, new string('a', 64)).Error);
        Assert.Equal("invalid_code", WorldAccount.RedeemHandoff(db, "").Error);
    }

    [Fact]
    public void An_expired_handoff_code_is_dead()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "exp-h@x.test", "long-password-1", "E", null);
        var code = WorldAccount.CreateHandoff(db, wid);
        db.Execute("UPDATE pciworld_handoff_codes SET expires_at=datetime('now','-1 minutes') WHERE code_sha=?",
            Security.Sha(code));
        Assert.Equal("invalid_code", WorldAccount.RedeemHandoff(db, code).Error);
    }

    [Fact]
    public void The_return_path_is_allow_listed_never_an_open_redirect()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "redir@x.test", "long-password-1", "R", null);
        foreach (var evil in new[] { "https://evil.example/phish", "//evil.example", "/admin.html", null, "" })
        {
            var code = WorldAccount.CreateHandoff(db, wid, evil);
            var r = WorldAccount.RedeemHandoff(db, code);
            Assert.Null(r.Error);
            Assert.Equal("/world/account", r.ReturnTo);
        }
        var ok = WorldAccount.RedeemHandoff(db, WorldAccount.CreateHandoff(db, wid, "/world/challenge/WC-EVM-001"));
        Assert.Equal("/world/challenge/WC-EVM-001", ok.ReturnTo);
    }

    [Fact]
    public void Sensitive_actions_accept_the_canonical_password_for_bridge_created_accounts()
    {
        var db = NewWorldDb();
        var student = CanonicalUser(db, "stepup@x.test");
        // The bridge mints a World row with a RANDOM password the person never saw.
        var (err, wid) = WorldAccount.LinkStudent(db, student, "stepup@x.test", "S Up");
        Assert.Null(err);

        // Their real (canonical) password verifies; a wrong one does not.
        Assert.True(WorldAccount.VerifyAccountPassword(db, wid, "portal-password-1"));
        Assert.False(WorldAccount.VerifyAccountPassword(db, wid, "not-their-password"));
        Assert.False(WorldAccount.VerifyAccountPassword(db, wid, null));

        // Standalone accounts still verify with their own World password (transitional).
        var (_, solo, _) = WorldAccount.Register(db, "solo-pw@x.test", "long-password-1", "Solo", null);
        Assert.True(WorldAccount.VerifyAccountPassword(db, solo, "long-password-1"));
    }

    [Fact]
    public void World_only_deletion_preserves_the_canonical_identity_and_profile()
    {
        var db = NewWorldDb();
        var student = CanonicalUser(db, "keepme@x.test");
        var (err, wid) = WorldAccount.LinkStudent(db, student, "keepme@x.test", "K M");
        Assert.Null(err);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", student));
        db.Execute("INSERT INTO student_profiles(user_id, country) VALUES(?, 'DE')", student);

        WorldAccount.DeleteAccount(db, wid);

        // World participation is gone…
        Assert.Null(db.QueryOne("SELECT id FROM pciworld_users WHERE id=?", wid));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", student));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_handoff_codes WHERE world_user_id=?", wid));
        // …and the canonical PCI identity, credentials and profile are untouched.
        var u = db.QueryOne("SELECT * FROM users WHERE id=?", student)!;
        Assert.Equal("keepme@x.test", H.Str(u["email"]));
        Assert.Equal("active", H.Str(u["status"]));
        Assert.True(Security.VerifyPassword("portal-password-1", H.Str(u["password_hash"])));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM student_profiles WHERE user_id=? AND country='DE'", student));
        // The map ledger survives as the audit record of what was deleted.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_map WHERE legacy_world_id=?", wid));
    }

    [Fact]
    public void Redeeming_a_handoff_claims_nothing_by_itself_ownership_rules_still_hold()
    {
        var db = NewWorldDb();
        // Another account's completed work sits in a session; a handoff for a different user must
        // never move it (claim-only-unowned is enforced at the claim, not trusted at the call).
        var sessA = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-a-h')");
        var (_, userA, _) = WorldAccount.Register(db, "owner-a@x.test", "long-password-1", "A", sessA);
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!;
        var att = db.ExecuteReturningId(@"INSERT INTO pciworld_attempts
                (session_id,challenge_id,version,status,score,user_id,completed_at)
            VALUES(?,?,?, 'completed', 50, ?, datetime('now'))", sessA, ch["id"], ch["current_version"], userA);

        var (_, userB, _) = WorldAccount.Register(db, "owner-b@x.test", "long-password-1", "B", null);
        WorldAccount.ClaimSession(db, userB, sessA);   // what a hostile redeem would attempt
        Assert.Equal(userA, Convert.ToInt64(
            db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", att)!["user_id"]));
    }
}
