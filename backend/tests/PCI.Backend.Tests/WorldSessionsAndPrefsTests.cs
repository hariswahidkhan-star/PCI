using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — participant sessions view and World preferences (PW-US-043 / §10.5).
///
/// Sessions: an account can see every live World session (metadata only — tokens are hashed and
/// can never be reprinted) and end any of them, immediately. Preferences: goal, timezone and
/// weekly target live on the participation row keyed by canonical users.id — product data,
/// validated, never on the canonical profile; absent keys never change stored values.
/// </summary>
public class WorldSessionsAndPrefsTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static Microsoft.AspNetCore.Http.HttpRequest Req(string token)
    {
        var ctx = new Microsoft.AspNetCore.Http.DefaultHttpContext();
        ctx.Request.Headers["X-World-Account"] = token;
        return ctx.Request;
    }

    [Fact]
    public void Sessions_are_listable_without_tokens_and_revocation_is_immediate()
    {
        var db = NewWorldDb();
        var (_, uid, tok1) = WorldAccount.Register(db, "sess-list@x.test", "long-password-1", "S", null);
        var (_, _, tok2) = WorldAccount.Login(db, "sess-list@x.test", "long-password-1", null);

        var rows = WorldAccount.Sessions(db, uid);
        Assert.Equal(2, rows.Count);
        // The stored value is the HASH — the raw token appears nowhere in the listing data.
        foreach (var r in rows)
        {
            Assert.NotEqual(tok1, H.Str(r["token"]));
            Assert.NotEqual(tok2, H.Str(r["token"]));
        }
        // The caller's own row is identifiable by hashing the presented token (the endpoint's
        // "current" flag).
        Assert.Contains(rows, r => H.Str(r["token"]) == Security.Sha(tok2));

        // Revoking a session kills it IMMEDIATELY — the very next request with that token is
        // anonymous, while the other session lives on.
        var victim = rows.First(r => H.Str(r["token"]) == Security.Sha(tok1));
        db.Execute("DELETE FROM pciworld_user_sessions WHERE id=? AND user_id=?", victim["id"], uid);
        Assert.Null(WorldAccount.FromReq(Req(tok1), db));
        Assert.NotNull(WorldAccount.FromReq(Req(tok2), db));

        // Revoke-all-others keeps exactly the caller's session.
        WorldAccount.Login(db, "sess-list@x.test", "long-password-1", null);
        db.Execute("DELETE FROM pciworld_user_sessions WHERE user_id=? AND token<>?", uid, Security.Sha(tok2));
        Assert.Single(WorldAccount.Sessions(db, uid));
        Assert.NotNull(WorldAccount.FromReq(Req(tok2), db));
    }

    [Fact]
    public void Sign_out_everywhere_revokes_all_world_and_canonical_sessions_for_this_account_only()
    {
        var db = NewWorldDb();
        // This account: two World sessions plus a canonical portal session.
        var (_, wid, tok1) = WorldAccount.Register(db, "so-all@x.test", "long-password-1", "S", null);
        WorldAccount.Login(db, "so-all@x.test", "long-password-1", null);
        var uid = WorldIdentity.CanonicalUserFor(db, wid)!.Value;
        db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?, 'sha-mine', 'session', datetime('now','+1 day'))", uid);
        // A bystander with their own sessions on both sides.
        var (_, otherWid, otherTok) = WorldAccount.Register(db, "so-other@x.test", "long-password-1", "O", null);
        var otherUid = WorldIdentity.CanonicalUserFor(db, otherWid)!.Value;
        db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?, 'sha-other', 'session', datetime('now','+1 day'))", otherUid);

        // The coordinated central sign-out, exactly as the endpoint performs it.
        db.Execute("DELETE FROM pciworld_user_sessions WHERE user_id=?", wid);
        db.Execute("DELETE FROM login_tokens WHERE user_id=? AND purpose='session'", uid);

        // Every one of THIS account's sessions is gone, on both sides…
        Assert.Empty(WorldAccount.Sessions(db, wid));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM login_tokens WHERE user_id=? AND purpose='session'", uid));
        Assert.Null(WorldAccount.FromReq(Req(tok1), db));
        // …and the bystander's are untouched, on both sides.
        Assert.Single(WorldAccount.Sessions(db, otherWid));
        Assert.NotNull(WorldAccount.FromReq(Req(otherTok), db));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM login_tokens WHERE user_id=? AND purpose='session'", otherUid));

        // World-only sign-out (the default) leaves canonical sessions alone.
        var (_, w2, t2) = WorldAccount.Register(db, "so-solo@x.test", "long-password-1", "W", null);
        var u2 = WorldIdentity.CanonicalUserFor(db, w2)!.Value;
        db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?, 'sha-solo', 'session', datetime('now','+1 day'))", u2);
        db.Execute("DELETE FROM pciworld_user_sessions WHERE token=?", Security.Sha(t2));
        Assert.Null(WorldAccount.FromReq(Req(t2), db));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM login_tokens WHERE user_id=? AND purpose='session'", u2));
    }

    [Fact]
    public void Evidence_reads_follow_both_ownership_namespaces_without_double_counting()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-ev-union')");
        var (_, wid, _) = WorldAccount.Register(db, "ev-union@x.test", "long-password-1", "E", null);
        var uid = WorldIdentity.CanonicalUserFor(db, wid)!.Value;
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!;

        // A dual-stamped row counts once; a canonical-only row (the case the legacy namespace
        // loses) is included; a stranger's rows never are.
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,user_id,canonical_user_id,passport_visible,completed_at)
            VALUES(?,?,?, 'completed', 80, ?, ?, 1, datetime('now'))", sess, ch["id"], ch["current_version"], wid, uid);
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,canonical_user_id,passport_visible,completed_at)
            VALUES(?,?,?, 'completed', 90, ?, 1, datetime('now'))", sess, ch["id"], ch["current_version"], uid);
        var (_, stranger, _) = WorldAccount.Register(db, "ev-stranger@x.test", "long-password-1", "X", null);
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,user_id,passport_visible,completed_at)
            VALUES(?,?,?, 'completed', 70, ?, 1, datetime('now'))", sess, ch["id"], ch["current_version"], stranger);

        Assert.Equal(2, WorldAccount.EvidenceStats(db, wid, visibleOnly: false).Completed);
        var rows = WorldAccount.EvidenceRows(db, wid, visibleOnly: true);
        Assert.Equal(2, rows.Count);
        Assert.DoesNotContain(rows, r => Convert.ToDouble(r["score"]) == 70.0);
        Assert.Equal(1, WorldAccount.EvidenceStats(db, stranger, visibleOnly: false).Completed);
    }

    [Fact]
    public void Streak_publish_shares_and_deletion_follow_both_namespaces()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-un-all')");
        var (_, wid, _) = WorldAccount.Register(db, "un-all@x.test", "long-password-1", "U N", null);
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", wid);
        var uid = WorldIdentity.CanonicalUserFor(db, wid)!.Value;

        // One canonical-only completed DAILY attempt (rotation provenance), shared publicly.
        var day = new DateTime(2026, 12, 20, 12, 0, 0, DateTimeKind.Utc);
        var period = WorldRotation.CurrentPeriod(db, day)!;
        var att = db.ExecuteReturningId(@"INSERT INTO pciworld_attempts
                (session_id,challenge_id,version,status,score,canonical_user_id,rotation_period_id,
                 passport_visible,result_token_sha,answers_json,completed_at)
            VALUES(?,?,?, 'completed', 85, ?, ?, 1, ?, '{""x"":""private""}', datetime('now'))",
            sess, period["challenge_id"], period["version"], uid, period["id"], Security.Sha("un-share"));

        // Streak sees the canonical-only daily completion.
        Assert.Equal(1, WorldDashboard.Streak(db, wid, day));
        // The publish guard counts it as selected evidence — publication succeeds.
        Assert.Null(WorldAccount.PublishPassport(db, wid).Error);
        // The sharing list reaches it.
        Assert.Contains(WorldAccount.ShareLinks(db, wid), r => H.L(r["id"]) == att);
        // A stranger sees none of it.
        var (_, strangerWid, _) = WorldAccount.Register(db, "un-stranger@x.test", "long-password-1", "X", null);
        Assert.Empty(WorldAccount.ShareLinks(db, strangerWid));
        Assert.Equal(0, WorldDashboard.Streak(db, strangerWid, day));

        // World-only deletion de-identifies the canonical-only row too — nothing personal survives.
        WorldAccount.DeleteAccount(db, wid);
        var gone = db.QueryOne("SELECT * FROM pciworld_attempts WHERE id=?", att)!;
        Assert.Null(gone["user_id"]);
        Assert.Null(gone["canonical_user_id"]);
        Assert.Null(gone["answers_json"]);
        Assert.Equal(1L, Convert.ToInt64(gone["result_revoked"]));
        Assert.Equal(0L, Convert.ToInt64(gone["passport_visible"]));
    }

    [Fact]
    public void Preferences_live_on_the_participation_row_and_are_validated()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "prefs@x.test", "long-password-1", "P", null);
        var uid = WorldIdentity.CanonicalUserFor(db, wid)!.Value;

        // Fresh account: linked, nothing chosen yet.
        var p0 = WorldIdentity.ReadPreferences(db, wid)!;
        Assert.Null(p0["goal"]);

        // Valid updates land on pciworld_participants — never anywhere near the profile.
        Assert.Null(WorldIdentity.UpdatePreferences(db, wid, "certification_prep", "Europe/Berlin", 4));
        var row = db.QueryOne("SELECT * FROM pciworld_participants WHERE user_id=?", uid)!;
        Assert.Equal("certification_prep", H.Str(row["goal"]));
        Assert.Equal("Europe/Berlin", H.Str(row["timezone"]));
        Assert.Equal(4L, H.L(row["weekly_target"]));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM student_profiles WHERE user_id=? AND enrollment_purpose='certification_prep'", uid));

        // Absent keys change nothing (the P0-06 rule, everywhere).
        Assert.Null(WorldIdentity.UpdatePreferences(db, wid, null, null, 2));
        row = db.QueryOne("SELECT * FROM pciworld_participants WHERE user_id=?", uid)!;
        Assert.Equal("certification_prep", H.Str(row["goal"]));
        Assert.Equal("Europe/Berlin", H.Str(row["timezone"]));
        Assert.Equal(2L, H.L(row["weekly_target"]));

        // Validation refuses garbage without touching anything.
        Assert.Equal("bad_goal", WorldIdentity.UpdatePreferences(db, wid, "win_at_life", null, null));
        Assert.Equal("bad_target", WorldIdentity.UpdatePreferences(db, wid, null, null, 9));
        Assert.Equal("bad_timezone", WorldIdentity.UpdatePreferences(db, wid, null, new string('x', 70), null));
        Assert.Equal("certification_prep", db.Scalar<string>("SELECT goal FROM pciworld_participants WHERE user_id=?", uid));
    }

    [Fact]
    public void An_unlinked_account_has_no_participation_row_to_prefer_into()
    {
        var db = NewWorldDb();
        db.ExecuteReturningId(@"INSERT INTO users(email,first_name,password_hash,role,status)
            VALUES('clash-pref@x.test','C','x','student','active')");
        var wid = db.ExecuteReturningId(@"INSERT INTO pciworld_users(email,password_hash)
            VALUES('clash-pref@x.test','x')");
        WorldIdentity.Run(db);

        Assert.Null(WorldIdentity.ReadPreferences(db, wid));
        Assert.Equal("not_linked", WorldIdentity.UpdatePreferences(db, wid, "explore", null, null));
    }
}
