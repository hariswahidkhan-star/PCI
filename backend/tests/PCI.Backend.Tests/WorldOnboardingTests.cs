using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — the onboarding state machine (§7.3).
///
///   not_started → welcome → goal → preferences → privacy → completed
///
/// The ORDER is server-enforced: a manipulated route can never mark a missing required step
/// complete; re-posting a passed step is a harmless resume; completion is idempotent and stamps
/// onboarded_at exactly once; and accounts already practising when the machine shipped were
/// grandfathered once so no established participant is gated behind a welcome tour.
/// </summary>
public class WorldOnboardingTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    [Fact]
    public void Steps_advance_in_order_and_route_manipulation_is_refused()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "ob@x.test", "long-password-1", "O", null);
        Assert.Equal("not_started", WorldIdentity.OnboardingState(db, wid));

        // Jumping ahead is refused — from ANY position.
        Assert.Equal("out_of_order", WorldIdentity.AdvanceOnboarding(db, wid, "privacy"));
        Assert.Equal("out_of_order", WorldIdentity.AdvanceOnboarding(db, wid, "preferences"));
        Assert.Equal("out_of_order", WorldIdentity.CompleteOnboarding(db, wid));
        Assert.Equal("not_started", WorldIdentity.OnboardingState(db, wid));
        Assert.Equal("bad_step", WorldIdentity.AdvanceOnboarding(db, wid, "not_started"));
        Assert.Equal("bad_step", WorldIdentity.AdvanceOnboarding(db, wid, "hack"));

        // The legitimate walk.
        foreach (var (step, expect) in new[] { ("welcome", "welcome"), ("goal", "goal"),
                                               ("preferences", "preferences"), ("privacy", "privacy") })
        {
            Assert.Null(WorldIdentity.AdvanceOnboarding(db, wid, step));
            Assert.Equal(expect, WorldIdentity.OnboardingState(db, wid));
        }
        Assert.Null(WorldIdentity.CompleteOnboarding(db, wid));
        Assert.Equal("completed", WorldIdentity.OnboardingState(db, wid));
    }

    [Fact]
    public void Resume_is_a_no_op_and_completion_is_idempotent_with_one_timestamp()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "ob-idem@x.test", "long-password-1", "O", null);
        Assert.Null(WorldIdentity.AdvanceOnboarding(db, wid, "welcome"));
        Assert.Null(WorldIdentity.AdvanceOnboarding(db, wid, "goal"));

        // A refresh re-posts an already-passed step: nothing changes, nothing errors.
        Assert.Null(WorldIdentity.AdvanceOnboarding(db, wid, "welcome"));
        Assert.Equal("goal", WorldIdentity.OnboardingState(db, wid));

        Assert.Null(WorldIdentity.AdvanceOnboarding(db, wid, "preferences"));
        Assert.Null(WorldIdentity.AdvanceOnboarding(db, wid, "privacy"));
        Assert.Null(WorldIdentity.AdvanceOnboarding(db, wid, "completed"));
        var uid = WorldIdentity.CanonicalUserFor(db, wid)!.Value;
        var stamp = db.Scalar<string>("SELECT onboarded_at FROM pciworld_participants WHERE user_id=?", uid);
        Assert.False(string.IsNullOrEmpty(stamp));

        // Completing again changes nothing — including the timestamp.
        Assert.Null(WorldIdentity.CompleteOnboarding(db, wid));
        Assert.Equal(stamp, db.Scalar<string>("SELECT onboarded_at FROM pciworld_participants WHERE user_id=?", uid));
    }

    [Fact]
    public void Established_accounts_were_grandfathered_once_new_accounts_still_onboard()
    {
        var db = NewWorldDb();
        // An account with completed practice BEFORE the backfill flag is set…
        db.Execute("DELETE FROM site_settings WHERE skey='world_onboarding_backfilled'");
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-grand')");
        var (_, oldWid, _) = WorldAccount.Register(db, "grand@x.test", "long-password-1", "G", null);
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!;
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,user_id,completed_at)
            VALUES(?,?,?, 'completed', 70, ?, datetime('now'))", sess, ch["id"], ch["current_version"], oldWid);
        WorldIdentity.Run(db);   // the one-time backfill pass
        Assert.Equal("completed", WorldIdentity.OnboardingState(db, oldWid));

        // …but an account created AFTER the flag goes through onboarding normally, even having
        // claimed completed anonymous work at registration.
        var anonSess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-post')");
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,completed_at)
            VALUES(?,?,?, 'completed', 60, datetime('now'))", anonSess, ch["id"], ch["current_version"]);
        var (_, newWid, _) = WorldAccount.Register(db, "fresh-ob@x.test", "long-password-1", "F", anonSess);
        WorldIdentity.Run(db);
        Assert.Equal("not_started", WorldIdentity.OnboardingState(db, newWid));
    }

    [Fact]
    public void Support_diagnostics_show_states_and_counts_never_private_content()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-diag')");
        var (_, wid, _) = WorldAccount.Register(db, "diag@x.test", "long-password-1", "D", null);
        db.Execute("UPDATE pciworld_users SET email_verified=1, passport_public=1, passport_token_sha='secret-sha', passport_photo_ref='photos/x.jpg' WHERE id=?", wid);
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!;
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,user_id,passport_visible,answers_json,completed_at)
            VALUES(?,?,?, 'completed', 88, ?, 1, '{""secret"":""answer-text""}', datetime('now'))",
            sess, ch["id"], ch["current_version"], wid);

        // Lookup by id and by email resolve the same record.
        var byId = WorldIdentity.SupportDiagnostics(db, wid.ToString())!;
        var byEmail = WorldIdentity.SupportDiagnostics(db, "DIAG@x.test")!;
        Assert.Equal(byId["world_id"], byEmail["world_id"]);

        Assert.Equal("created", byId["mapping_outcome"]);
        Assert.NotNull(byId["canonical_user_id"]);
        Assert.Equal("not_started", byId["onboarding_state"]);
        // Journey timestamps support can anchor a conversation on.
        Assert.True(byId.ContainsKey("first_entered_at"));
        Assert.True(byId.ContainsKey("onboarded_at"));
        Assert.Equal(1L, byId["attempts_completed"]);
        Assert.Equal(true, byId["passport_public"]);
        Assert.Equal(1L, byId["visible_evidence"]);
        Assert.Equal(1L, byId["live_sessions"]);

        // The privacy boundary: no answers, no token material, no disclosure, no photo reference —
        // not as values and not as keys.
        var json = System.Text.Json.JsonSerializer.Serialize(byId);
        Assert.DoesNotContain("answer-text", json);
        Assert.DoesNotContain("secret-sha", json);
        Assert.DoesNotContain("photos/x.jpg", json);
        foreach (var forbidden in new[] { "answers", "token", "photo", "show_scores", "disclosure" })
            Assert.DoesNotContain(forbidden, json, StringComparison.OrdinalIgnoreCase);

        Assert.Null(WorldIdentity.SupportDiagnostics(db, "nobody@x.test"));
        Assert.Null(WorldIdentity.SupportDiagnostics(db, ""));
    }

    [Fact]
    public void The_sweep_clears_expired_and_consumed_handoff_codes()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "sweep-h@x.test", "long-password-1", "S", null);
        var live = WorldAccount.CreateHandoff(db, wid);
        var dead = WorldAccount.CreateHandoff(db, wid);
        var used = WorldAccount.CreateHandoff(db, wid);
        db.Execute("UPDATE pciworld_handoff_codes SET expires_at=datetime('now','-1 minutes') WHERE code_sha=?", Security.Sha(dead));
        WorldAccount.RedeemHandoff(db, used);

        WorldRetentionService.Sweep(db);
        // The unexpired, unconsumed code survives; residue is gone.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_handoff_codes WHERE world_user_id=?", wid));
        Assert.Null(WorldAccount.RedeemHandoff(db, live).Error);
    }

    [Fact]
    public void An_unlinked_account_cannot_onboard_and_the_switcher_state_says_why()
    {
        var db = NewWorldDb();
        db.ExecuteReturningId(@"INSERT INTO users(email,first_name,password_hash,role,status)
            VALUES('clash-ob@x.test','C','x','student','active')");
        var wid = db.ExecuteReturningId(@"INSERT INTO pciworld_users(email,password_hash)
            VALUES('clash-ob@x.test','x')");
        WorldIdentity.Run(db);

        Assert.Null(WorldIdentity.OnboardingState(db, wid));
        Assert.Equal("not_linked", WorldIdentity.AdvanceOnboarding(db, wid, "welcome"));

        var ctx = new WorldAccount.UserCtx(wid, "clash-ob@x.test", "C", true, false);
        var s = WorldDashboard.Build(db, ctx, new DateTime(2026, 11, 20, 12, 0, 0, DateTimeKind.Utc));
        Assert.Null(s.OnboardingState);
        Assert.False(s.CanonicalLinked);
        // No participation → the engine never demands onboarding it cannot perform.
        Assert.NotEqual("continue_onboarding", s.PrimaryAction);
    }
}
