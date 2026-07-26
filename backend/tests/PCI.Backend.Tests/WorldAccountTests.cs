using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — Phase 1b: participant accounts + Passport (docs/pciworld/PLAN.md).
///
/// Proves: registration/login hygiene (bcrypt, lockout, no plaintext anywhere); anonymous-session
/// claiming adopts only unclaimed attempts; Passport evidence is opt-in per item; publication is
/// gated on verified email AND a display name, its token is opaque/sha-stored/rotating; deletion
/// de-identifies attempts and kills every public surface; and the account realm stays wholly
/// separate from platform users and exam data (no shared tables at all).
/// </summary>
public class WorldAccountTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long Attempt(Db db, long sessionId, string code = "WC-EVM-001", long? userId = null)
    {
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE code=?", code)!;
        return db.ExecuteReturningId(@"INSERT INTO pciworld_attempts
                (session_id,challenge_id,version,status,score,profile_key,user_id,completed_at)
            VALUES(?,?,?, 'completed', 88.5, 'Cost Guardian', ?, datetime('now'))",
            sessionId, ch["id"], ch["current_version"], userId);
    }

    [Fact]
    public void Register_claims_anonymous_attempts_but_never_someone_elses()
    {
        var db = NewWorldDb();
        var mySession = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-mine')");
        var otherSession = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-other')");
        var mine = Attempt(db, mySession);
        var (e0, otherUser, _) = WorldAccount.Register(db, "other@x.test", "long-password-1", "Other", null);
        Assert.Null(e0);
        var theirs = Attempt(db, otherSession, "WC-SCH-002", otherUser);

        var (err, uid, token) = WorldAccount.Register(db, "me@x.test", "long-password-1", "Me", mySession);
        Assert.Null(err);
        Assert.True(token.Length == 64);

        Assert.Equal(uid, Convert.ToInt64(db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", mine)!["user_id"]));
        Assert.Equal(otherUser, Convert.ToInt64(db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", theirs)!["user_id"]));

        // Hygiene: bcrypt only, session tokens sha-stored, duplicate email refused, weak password refused.
        Assert.StartsWith("$2", Convert.ToString(db.QueryOne("SELECT password_hash FROM pciworld_users WHERE id=?", uid)!["password_hash"]));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_sessions WHERE token=?", token));
        Assert.Equal("duplicate_email", WorldAccount.Register(db, "me@x.test", "long-password-1", null, null).Error);
        Assert.Equal("weak_password", WorldAccount.Register(db, "w@x.test", "short", null, null).Error);
        Assert.Equal("bad_email", WorldAccount.Register(db, "nope", "long-password-1", null, null).Error);
    }

    [Fact]
    public void Login_verifies_passwords_locks_out_and_claims_the_current_session()
    {
        var db = NewWorldDb();
        var (_, uid, _) = WorldAccount.Register(db, "login@x.test", "long-password-1", "L", null);

        Assert.Equal("invalid_credentials", WorldAccount.Login(db, "login@x.test", "wrong", null).Error);
        Assert.Equal("invalid_credentials", WorldAccount.Login(db, "ghost@x.test", "whatever", null).Error);

        var laterSession = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-later')");
        var att = Attempt(db, laterSession, "WC-RSK-003");
        var (err, uid2, token) = WorldAccount.Login(db, "login@x.test", "long-password-1", laterSession);
        Assert.Null(err);
        Assert.Equal(uid, uid2);
        Assert.Equal(uid, Convert.ToInt64(db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", att)!["user_id"]));
        Assert.NotEqual("", token);

        // Ten straight failures lock the account (LoginGuard), and the lock reports as locked.
        for (var i = 0; i < 10; i++) WorldAccount.Login(db, "login@x.test", "wrong", null);
        Assert.Equal("account_locked", WorldAccount.Login(db, "login@x.test", "long-password-1", null).Error);
    }

    [Fact]
    public void Passport_publication_requires_consent_verification_and_a_name()
    {
        var db = NewWorldDb();
        var (_, uid, _) = WorldAccount.Register(db, "pp@x.test", "long-password-1", null, null);

        // Unverified email → refused; verified but no display name → refused; named but with no
        // selected evidence → refused (P1-05: a Passport cannot publish empty).
        Assert.Equal("email_unverified", WorldAccount.PublishPassport(db, uid).Error);
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", uid);
        Assert.Equal("no_display_name", WorldAccount.PublishPassport(db, uid).Error);
        db.Execute("UPDATE pciworld_users SET display_name='Haris' WHERE id=?", uid);
        Assert.Equal("no_evidence", WorldAccount.PublishPassport(db, uid).Error);
        var evSess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-pp-ev')");
        db.Execute("UPDATE pciworld_attempts SET passport_visible=1 WHERE id=?", Attempt(db, evSess, "WC-EVM-001", uid));

        var (err, url) = WorldAccount.PublishPassport(db, uid);
        Assert.Null(err);
        Assert.StartsWith("/world/p/", url);
        var raw1 = url!["/world/p/".Length..];
        // Opaque + sha-stored: the raw token is never in the database.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_users WHERE passport_token_sha=?", raw1));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_users WHERE passport_token_sha=?", Security.Sha(raw1)));

        // Republish rotates the token — the old link dies.
        var raw2 = WorldAccount.PublishPassport(db, uid).Url!["/world/p/".Length..];
        Assert.NotEqual(raw1, raw2);
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_users WHERE passport_token_sha=?", Security.Sha(raw1)));

        // Unpublish revokes entirely.
        WorldAccount.UnpublishPassport(db, uid);
        var u = db.QueryOne("SELECT passport_public,passport_token_sha FROM pciworld_users WHERE id=?", uid)!;
        Assert.Equal(0L, Convert.ToInt64(u["passport_public"]));
        Assert.Null(u["passport_token_sha"]);
    }

    [Fact]
    public void Passport_evidence_is_opt_in_per_item_and_public_rows_respect_it()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-ev')");
        var (_, uid, _) = WorldAccount.Register(db, "ev@x.test", "long-password-1", "Ev", sess);
        var a1 = Attempt(db, sess, "WC-EVM-001", uid);
        var a2 = Attempt(db, sess, "WC-SCH-002", uid);

        // Nothing is public by default.
        Assert.Empty(WorldAccount.EvidenceRows(db, uid, visibleOnly: true));
        Assert.Equal(2, WorldAccount.EvidenceRows(db, uid, visibleOnly: false).Count);

        db.Execute("UPDATE pciworld_attempts SET passport_visible=1 WHERE id=? AND user_id=?", a1, uid);
        var pub = WorldAccount.EvidenceRows(db, uid, visibleOnly: true);
        Assert.Single(pub);
        Assert.Equal("The metro package is bleeding money — or is it?", Convert.ToString(pub[0]["title"]));
        // The evidence row carries display fields only — no answers, no session, no email.
        Assert.False(pub[0].ContainsKey("answers_json"));
        Assert.False(pub[0].ContainsKey("session_id"));
        _ = a2;
    }

    [Fact]
    public void Delete_account_deidentifies_attempts_and_kills_every_public_surface()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-del')");
        var (_, uid, tok) = WorldAccount.Register(db, "del@x.test", "long-password-1", "Del", sess);
        var att = Attempt(db, sess, "WC-EVM-001", uid);
        db.Execute("UPDATE pciworld_attempts SET passport_visible=1, result_token_sha='rsha', display_name='Del' WHERE id=?", att);
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", uid);
        WorldAccount.PublishPassport(db, uid);

        WorldAccount.DeleteAccount(db, uid);

        Assert.Null(db.QueryOne("SELECT id FROM pciworld_users WHERE id=?", uid));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_sessions WHERE user_id=?", uid));
        var a = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=?", att)!;
        Assert.Null(a["user_id"]);
        Assert.Null(a["display_name"]);
        Assert.Null(a["result_token_sha"]);
        Assert.Equal(1L, Convert.ToInt64(a["result_revoked"]));
        Assert.Equal(0L, Convert.ToInt64(a["passport_visible"]));
        _ = tok;
    }

    [Fact]
    public void Account_realm_is_wholly_separate_from_platform_users()
    {
        var db = NewWorldDb();
        WorldAccount.Register(db, "sep@x.test", "long-password-1", "S", null);
        // Canonical-identity model (journey repair P0-00): a World registration creates exactly ONE
        // canonical users row — the same credentials work on both products, and there is never a
        // duplicate. Product SESSIONS stay separate (below); the identity does not.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM users WHERE email='sep@x.test'"));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_users WHERE email='sep@x.test'"));
        // And a platform student session token means nothing to the world-account resolver.
        var u = db.ExecuteReturningId(
            "INSERT INTO users(email,password_hash,status,first_name) VALUES('stud@pci.local','x','active','S')");
        var raw = Security.RandomHex(32);
        db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 hour'))",
            u, Security.Sha(raw));
        var ctx = new Microsoft.AspNetCore.Http.DefaultHttpContext();
        ctx.Request.Headers["X-World-Account"] = raw;
        Assert.Null(WorldAccount.FromReq(ctx.Request, db));
    }
}
