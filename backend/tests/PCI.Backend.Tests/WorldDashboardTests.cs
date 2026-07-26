using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — dashboard aggregate and the one-primary-action engine (journey repair P1-01 / §9.1).
///
/// The server computes exactly one primary action from the participant's REAL state, in the
/// specified priority order: verify_email → continue_today → view_result → start_today →
/// continue_passport → recommended. These tests walk one account through the whole returning-user
/// loop and assert the action, the today-state and the Passport readiness at every step.
/// </summary>
public class WorldDashboardTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static DateTime Day => new(2026, 9, 10, 12, 0, 0, DateTimeKind.Utc);

    static WorldAccount.UserCtx Ctx(Db db, long uid)
    {
        var u = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", uid)!;
        return new WorldAccount.UserCtx(uid, H.Str(u["email"])!, H.Str(u["display_name"]),
            H.L(u["email_verified"]) == 1, H.L(u["passport_public"]) == 1);
    }

    [Fact]
    public void The_priority_engine_returns_exactly_one_action_through_the_daily_loop()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-dash')");
        var (_, uid, _) = WorldAccount.Register(db, "dash@x.test", "long-password-1", "Dash", null);

        // 1. Unverified email outranks everything.
        var s = WorldDashboard.Build(db, Ctx(db, uid), Day);
        Assert.Equal("verify_email", s.PrimaryAction);
        Assert.Equal("not_started", s.TodayState.State);
        Assert.NotNull(s.TodayState.Code);           // today exists and is pinned
        Assert.False(string.IsNullOrEmpty(s.TodayState.ChangesAtUtc));

        // 2. Verified, nothing started → start today.
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", uid);
        s = WorldDashboard.Build(db, Ctx(db, uid), Day);
        Assert.Equal("start_today", s.PrimaryAction);
        Assert.Equal(s.TodayState.Code, s.PrimaryCode);

        // 3. Today in progress → continue beats start.
        var start = WorldAttempts.Start(db, sess, uid, s.TodayState.Code!, null, false, Day);
        s = WorldDashboard.Build(db, Ctx(db, uid), Day);
        Assert.Equal("continue_today", s.PrimaryAction);
        Assert.Equal(start.AttemptId, s.PrimaryAttemptId);
        Assert.Equal("in_progress", s.TodayState.State);
        Assert.Single(s.InProgress);

        // 4. Completed today → view result replaces start/continue.
        WorldAttempts.Submit(db, sess, uid, start.AttemptId, System.Text.Json.JsonDocument.Parse("""{"a":"1"}""").RootElement);
        s = WorldDashboard.Build(db, Ctx(db, uid), Day);
        Assert.Equal("view_result", s.PrimaryAction);
        Assert.Equal("completed", s.TodayState.State);
        Assert.Equal(start.AttemptId, s.PrimaryAttemptId);
        Assert.Empty(s.InProgress);
        Assert.Single(s.RecentResults);
        Assert.Equal(1, (int)s.Progress.Completed);
    }

    [Fact]
    public void Passport_readiness_states_follow_the_specified_ladder()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-ppt')");
        var (_, uid, _) = WorldAccount.Register(db, "ppt-dash@x.test", "long-password-1", null, null);

        // No completed work at all → private_empty.
        Assert.Equal("private_empty", WorldDashboard.Build(db, Ctx(db, uid), Day).Passport.State);

        // Completed work but unverified/no name/no selection → private_incomplete.
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!;
        var att = db.ExecuteReturningId(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,user_id,completed_at)
            VALUES(?,?,?, 'completed', 70, ?, datetime('now'))", sess, ch["id"], ch["current_version"], uid);
        Assert.Equal("private_incomplete", WorldDashboard.Build(db, Ctx(db, uid), Day).Passport.State);

        // Verified + display name + one selected item → private_ready, and the primary action
        // (with today untouched … today outranks it, so check the passport state only).
        db.Execute("UPDATE pciworld_users SET email_verified=1, display_name='P' WHERE id=?", uid);
        db.Execute("UPDATE pciworld_attempts SET passport_visible=1 WHERE id=?", att);
        var s = WorldDashboard.Build(db, Ctx(db, uid), Day);
        Assert.Equal("private_ready", s.Passport.State);
        Assert.Equal(1, (int)s.Passport.VisibleEvidence);

        // Published → published_active; past expiry → expired.
        WorldAccount.PublishPassport(db, uid);
        Assert.Equal("published_active", WorldDashboard.Build(db, Ctx(db, uid), Day).Passport.State);
        db.Execute("UPDATE pciworld_users SET passport_expires_at=datetime('now','-1 days') WHERE id=?", uid);
        Assert.Equal("expired", WorldDashboard.Build(db, Ctx(db, uid), Day).Passport.State);
    }

    [Fact]
    public void The_recommendation_is_a_challenge_the_account_has_not_completed()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-rec')");
        var (_, uid, _) = WorldAccount.Register(db, "rec@x.test", "long-password-1", "R", null);
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", uid);

        var s = WorldDashboard.Build(db, Ctx(db, uid), Day);
        Assert.NotNull(s.RecommendedCode);
        // Complete the recommended one — the next snapshot recommends something else.
        var start = WorldAttempts.Start(db, sess, uid, s.RecommendedCode!, null, false, Day);
        WorldAttempts.Submit(db, sess, uid, start.AttemptId, System.Text.Json.JsonDocument.Parse("{}").RootElement);
        var s2 = WorldDashboard.Build(db, Ctx(db, uid), Day);
        Assert.NotEqual(s.RecommendedCode, s2.RecommendedCode);
    }
}
