using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — share/invitation management, goal-aware recommendations, weekly progress and
/// reminder consent (PW-US-039/040/030, P1-07 finishing touches).
/// </summary>
public class WorldSharingTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static (long Uid, long Sess, long Att) OwnerWithShare(Db db, string email, string sha)
    {
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", sha);
        var (_, uid, _) = WorldAccount.Register(db, email, "long-password-1", "O", null);
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!;
        var att = db.ExecuteReturningId(@"INSERT INTO pciworld_attempts
                (session_id,challenge_id,version,status,score,user_id,result_token_sha,completed_at)
            VALUES(?,?,?, 'completed', 80, ?, ?, datetime('now'))",
            sess, ch["id"], ch["current_version"], uid, Security.Sha("share-" + email));
        db.Execute("INSERT INTO pciworld_invites(attempt_id,token_sha) VALUES(?,?)", att, Security.Sha("inv-" + email));
        return (uid, sess, att);
    }

    [Fact]
    public void Shares_and_invitations_list_only_the_owners_and_revocation_is_immediate()
    {
        var db = NewWorldDb();
        var a = OwnerWithShare(db, "share-a@x.test", "s-sh-a");
        var b = OwnerWithShare(db, "share-b@x.test", "s-sh-b");

        // Each account sees exactly its own surfaces — never a stranger's.
        Assert.Single(WorldAccount.ShareLinks(db, a.Uid));
        Assert.Single(WorldAccount.Invitations(db, a.Uid));
        Assert.DoesNotContain(WorldAccount.ShareLinks(db, a.Uid), r => H.L(r["id"]) == b.Att);

        // Revoke-all for A: A's result link and invitation die at once; B's stay live — and the
        // PUBLIC resolution paths (the queries /world/r and /world/i use) honour it immediately.
        db.Execute(@"UPDATE pciworld_attempts SET result_revoked=1 WHERE user_id=? AND result_token_sha IS NOT NULL", a.Uid);
        db.Execute(@"UPDATE pciworld_invites SET revoked=1 WHERE attempt_id IN (SELECT id FROM pciworld_attempts WHERE user_id=?)", a.Uid);
        Assert.Null(db.QueryOne(@"SELECT id FROM pciworld_attempts
            WHERE result_token_sha=? AND result_revoked=0 AND status='completed'", Security.Sha("share-share-a@x.test")));
        Assert.Null(db.QueryOne("SELECT id FROM pciworld_invites WHERE token_sha=? AND revoked=0", Security.Sha("inv-share-a@x.test")));
        Assert.NotNull(db.QueryOne(@"SELECT id FROM pciworld_attempts
            WHERE result_token_sha=? AND result_revoked=0 AND status='completed'", Security.Sha("share-share-b@x.test")));
        Assert.NotNull(db.QueryOne("SELECT id FROM pciworld_invites WHERE token_sha=? AND revoked=0", Security.Sha("inv-share-b@x.test")));
    }

    [Fact]
    public void The_recommendation_follows_the_chosen_goal_and_stays_rule_based()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "goal-rec@x.test", "long-password-1", "G", null);
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", wid);
        var u = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", wid)!;
        var ctx = new WorldAccount.UserCtx(wid, "goal-rec@x.test", "G", true, false);
        var day = new DateTime(2026, 11, 5, 12, 0, 0, DateTimeKind.Utc);

        // Certification preparation prefers professional/advanced material.
        Assert.Null(WorldIdentity.UpdatePreferences(db, wid, "certification_prep", null, null));
        var s = WorldDashboard.Build(db, ctx, day);
        Assert.NotNull(s.RecommendedCode);
        var diff = db.Scalar<string>("SELECT difficulty FROM pciworld_challenges WHERE code=?", s.RecommendedCode);
        Assert.Contains(diff, new[] { "professional", "advanced" });

        // Exploration prefers an industry the account has NOT practised: complete one challenge,
        // switch goals, and the next recommendation avoids that industry.
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-goal-rec')");
        var done = db.QueryOne(@"SELECT c.id, c.current_version, v.industry FROM pciworld_challenges c
            JOIN pciworld_challenge_versions v ON v.challenge_id=c.id AND v.version=c.current_version
            WHERE c.current_version>=1 AND v.industry IS NOT NULL LIMIT 1")!;
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,user_id,completed_at)
            VALUES(?,?,?, 'completed', 70, ?, datetime('now'))", sess, done["id"], done["current_version"], wid);
        Assert.Null(WorldIdentity.UpdatePreferences(db, wid, "explore", null, null));
        var s2 = WorldDashboard.Build(db, ctx, day);
        var recIndustry = db.Scalar<string>(@"SELECT v.industry FROM pciworld_challenges c
            JOIN pciworld_challenge_versions v ON v.challenge_id=c.id AND v.version=c.current_version
            WHERE c.code=?", s2.RecommendedCode);
        Assert.NotEqual(H.Str(done["industry"]), recIndustry);
        _ = u;
    }

    [Fact]
    public void Weekly_progress_counts_the_last_seven_rotation_days_against_the_target()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-week')");
        var (_, wid, _) = WorldAccount.Register(db, "week@x.test", "long-password-1", "W", null);
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", wid);
        Assert.Null(WorldIdentity.UpdatePreferences(db, wid, null, null, 3));

        var json = System.Text.Json.JsonDocument.Parse("""{"a":"1"}""").RootElement;
        for (var d = 0; d < 2; d++)
        {
            var day = new DateTime(2026, 12, 1 + d, 12, 0, 0, DateTimeKind.Utc);
            var code = H.Str(db.QueryOne("SELECT code FROM pciworld_challenges WHERE id=?",
                WorldRotation.CurrentPeriod(db, day)!["challenge_id"])!["code"])!;
            var start = WorldAttempts.Start(db, sess, wid, code, null, false, day);
            WorldAttempts.Submit(db, sess, wid, start.AttemptId, json);
        }

        var ctx = new WorldAccount.UserCtx(wid, "week@x.test", "W", true, false);
        var s = WorldDashboard.Build(db, ctx, new DateTime(2026, 12, 2, 18, 0, 0, DateTimeKind.Utc));
        Assert.Equal(2, s.WeekDone);
        Assert.Equal(3, s.WeekTarget);
    }

    [Fact]
    public void Reminder_consent_is_explicit_opt_in_and_survives_partial_updates()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "remind@x.test", "long-password-1", "R", null);

        // Off by default — absent means off, always.
        Assert.False((bool)WorldIdentity.ReadPreferences(db, wid)!["reminders_enabled"]!);

        // Opt in with an hour; then a partial update (hour only) keeps the consent.
        Assert.Null(WorldIdentity.UpdatePreferences(db, wid, null, null, null, true, 7));
        var p = WorldIdentity.ReadPreferences(db, wid)!;
        Assert.True((bool)p["reminders_enabled"]!);
        Assert.Equal(7L, p["reminder_hour"]);
        Assert.Null(WorldIdentity.UpdatePreferences(db, wid, null, null, null, null, 19));
        p = WorldIdentity.ReadPreferences(db, wid)!;
        Assert.True((bool)p["reminders_enabled"]!);
        Assert.Equal(19L, p["reminder_hour"]);

        // Garbage refused; opting out sticks.
        Assert.Equal("bad_hour", WorldIdentity.UpdatePreferences(db, wid, null, null, null, null, 99));
        Assert.Null(WorldIdentity.UpdatePreferences(db, wid, null, null, null, false, null));
        Assert.False((bool)WorldIdentity.ReadPreferences(db, wid)!["reminders_enabled"]!);
    }
}
