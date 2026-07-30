using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — participant-journey repair regressions (P0-01, P0-04, P0-05, P0-06).
///
/// These tests pin the four data-flow guarantees the journey audit found broken:
///
///   P0-01  An attempt started or finished while signed in belongs to the account IMMEDIATELY —
///          never "until some later login claims it". Ownership is claim-only-unowned and is never
///          reassigned between accounts. The portal SSO bridge claims the browser's current
///          anonymous session the same way register/login do.
///   P0-04  Submit is idempotent: a retry after a lost response replays the stored result; a
///          different payload against a graded attempt is refused. Reloading a completed challenge
///          returns the result instead of silently opening a duplicate; a fresh attempt after
///          completion is an explicit retake with recorded lineage.
///   P0-05  The rotation period is the version authority: a midday publish never changes the
///          challenge (or version) participants face, and pausing rotation across the boundary
///          keeps the previous period on the air instead of serving nothing.
///   P0-06  Passport link expiry is an independent persisted field: disclosure saves that do not
///          mention it can never change it.
/// </summary>
public class WorldJourneyTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static DateTime Utc(int y, int m, int d, int h = 12) => new(y, m, d, h, 0, 0, DateTimeKind.Utc);

    static long NewSession(Db db, string sha) =>
        db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", sha);

    static JsonElement Json(string raw) => JsonDocument.Parse(raw).RootElement;

    // Any published challenge with a real snapshot; the content pack guarantees these exist.
    static (long Id, string Code, long Version) AnyChallenge(Db db, int skip = 0)
    {
        var r = db.Query(@"SELECT c.id, c.code, c.current_version FROM pciworld_challenges c
            JOIN pciworld_challenge_versions v ON v.challenge_id=c.id AND v.version=c.current_version
            WHERE c.current_version>=1 AND c.retired=0 ORDER BY c.id LIMIT 1 OFFSET " + skip)[0];
        return (H.L(r["id"]), H.Str(r["code"])!, H.L(r["current_version"]));
    }

    // ───────────────────────── P0-01: ownership at creation ─────────────────────────

    [Fact]
    public void Signed_in_start_owns_the_attempt_immediately()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-own");
        var (_, uid, _) = WorldAccount.Register(db, "own@x.test", "long-password-1", "O", null);
        var (_, code, _) = AnyChallenge(db);

        var o = WorldAttempts.Start(db, sess, uid, code, null, false, Utc(2026, 5, 1));
        Assert.Null(o.Error);
        Assert.False(o.Resumed);
        Assert.Equal(uid, Convert.ToInt64(
            db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", o.AttemptId)!["user_id"]));
    }

    [Fact]
    public void Resuming_an_unowned_attempt_while_signed_in_adopts_it_but_never_reassigns()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-adopt");
        var (_, code, _) = AnyChallenge(db);

        // Anonymous start, then the same browser signs in and reopens the challenge.
        var anon = WorldAttempts.Start(db, sess, null, code, null, false, Utc(2026, 5, 1));
        var (_, uid, _) = WorldAccount.Register(db, "adopt@x.test", "long-password-1", "A", null);
        var resumed = WorldAttempts.Start(db, sess, uid, code, null, false, Utc(2026, 5, 1));
        Assert.Equal(anon.AttemptId, resumed.AttemptId);
        Assert.True(resumed.Resumed);
        Assert.Equal(uid, Convert.ToInt64(
            db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", anon.AttemptId)!["user_id"]));

        // A DIFFERENT account resuming on the same shared browser must not steal the ownership.
        var (_, uid2, _) = WorldAccount.Register(db, "thief@x.test", "long-password-1", "T", null);
        WorldAttempts.Start(db, sess, uid2, code, null, false, Utc(2026, 5, 1));
        Assert.Equal(uid, Convert.ToInt64(
            db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", anon.AttemptId)!["user_id"]));
    }

    [Fact]
    public void Signed_in_submit_of_an_unowned_attempt_adopts_it_at_completion()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-late");
        var (_, code, _) = AnyChallenge(db);
        var o = WorldAttempts.Start(db, sess, null, code, null, false, Utc(2026, 5, 1));

        var (_, uid, _) = WorldAccount.Register(db, "late@x.test", "long-password-1", "L", null);
        var s = WorldAttempts.Submit(db, sess, uid, o.AttemptId, Json("""{"a":"1"}"""));
        Assert.Null(s.Error);
        Assert.Equal(uid, Convert.ToInt64(
            db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", o.AttemptId)!["user_id"]));
    }

    [Fact]
    public void Cross_device_resume_finds_the_accounts_attempt_from_a_new_session()
    {
        var db = NewWorldDb();
        var (_, uid, _) = WorldAccount.Register(db, "xdev@x.test", "long-password-1", "X", null);
        var (_, code, _) = AnyChallenge(db);
        var laptop = NewSession(db, "s-laptop");
        var started = WorldAttempts.Start(db, laptop, uid, code, null, false, Utc(2026, 5, 1));

        var phone = NewSession(db, "s-phone");
        var resumed = WorldAttempts.Start(db, phone, uid, code, null, false, Utc(2026, 5, 1));
        Assert.True(resumed.Resumed);
        Assert.Equal(started.AttemptId, resumed.AttemptId);

        // And the owned lookup honours the account from the new session, but never a stranger's.
        Assert.NotNull(WorldAttempts.Owned(db, phone, uid, started.AttemptId));
        var (_, other, _) = WorldAccount.Register(db, "other-x@x.test", "long-password-1", "O", null);
        Assert.Null(WorldAttempts.Owned(db, phone + 1000, other, started.AttemptId));
    }

    [Fact]
    public void Sso_bridge_claims_the_browsers_current_anonymous_work_exactly_once()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-sso");
        var (chId, _, ver) = AnyChallenge(db);
        var attempt = db.ExecuteReturningId(@"INSERT INTO pciworld_attempts
                (session_id,challenge_id,version,status,score,completed_at) VALUES(?,?,?, 'completed', 70, datetime('now'))",
            sess, chId, ver);

        // The portal bridge: link-or-create, then claim the current anonymous session (the step the
        // audit found missing). Idempotent, and never adopts another account's rows.
        var (err, worldId) = WorldAccount.LinkStudent(db, 4242, "student@x.test", "S T");
        Assert.Null(err);
        WorldAccount.ClaimSession(db, worldId, sess);
        WorldAccount.ClaimSession(db, worldId, sess);   // a second SSO round-trip changes nothing

        Assert.Equal(worldId, Convert.ToInt64(
            db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", attempt)!["user_id"]));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts WHERE user_id=?", worldId));
    }

    // ───────────────────────── P0-04: idempotent submit / reload / retake ─────────────────────────

    [Fact]
    public void Identical_resubmit_replays_the_stored_result_instead_of_409()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-idem");
        var (_, code, _) = AnyChallenge(db);
        var o = WorldAttempts.Start(db, sess, null, code, null, false, Utc(2026, 5, 1));

        var first = WorldAttempts.Submit(db, sess, null, o.AttemptId, Json("""{"a":"1","b":"x"}"""));
        Assert.Null(first.Error);
        Assert.False(first.Replayed);
        var storedScore = Convert.ToDouble(first.Attempt!["score"]);

        // The lost-response retry: same payload (any key order), and the empty-body reload.
        foreach (var retry in new[] { """{"a":"1","b":"x"}""", """{"b":"x","a":"1"}""", "{}" })
        {
            var again = WorldAttempts.Submit(db, sess, null, o.AttemptId,
                retry == "{}" ? default : Json(retry));
            Assert.Null(again.Error);
            Assert.True(again.Replayed);
            Assert.Equal(storedScore, Convert.ToDouble(again.Attempt!["score"]));
        }

        // A DIFFERENT payload cannot rewrite a graded result.
        Assert.Equal("already_submitted",
            WorldAttempts.Submit(db, sess, null, o.AttemptId, Json("""{"a":"999"}""")).Error);

        // Exactly one completed attempt exists, and its answers are the first submission's.
        Assert.Equal(1L, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_attempts WHERE session_id=? AND status='completed'", sess));
        Assert.True(WorldAttempts.JsonEquivalent("""{"a":"1","b":"x"}""",
            H.Str(db.QueryOne("SELECT answers_json FROM pciworld_attempts WHERE id=?", o.AttemptId)!["answers_json"])!));
    }

    [Fact]
    public void Reload_after_completion_returns_the_result_and_retake_is_explicit_with_lineage()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-reload");
        var (_, code, _) = AnyChallenge(db);
        var o = WorldAttempts.Start(db, sess, null, code, null, false, Utc(2026, 5, 1));
        WorldAttempts.Submit(db, sess, null, o.AttemptId, Json("""{"a":"1"}"""));

        // Reload → the completed attempt comes back; no duplicate row appears.
        var reload = WorldAttempts.Start(db, sess, null, code, null, false, Utc(2026, 5, 1));
        Assert.True(reload.Completed);
        Assert.Equal(o.AttemptId, reload.AttemptId);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts WHERE session_id=?", sess));

        // Retake → a NEW linked attempt; the original result is untouched.
        var retake = WorldAttempts.Start(db, sess, null, code, null, true, Utc(2026, 5, 1));
        Assert.False(retake.Completed);
        Assert.NotEqual(o.AttemptId, retake.AttemptId);
        Assert.Equal(o.AttemptId, Convert.ToInt64(
            db.QueryOne("SELECT parent_attempt_id FROM pciworld_attempts WHERE id=?", retake.AttemptId)!["parent_attempt_id"]));
        Assert.Equal("completed", H.Str(
            db.QueryOne("SELECT status FROM pciworld_attempts WHERE id=?", o.AttemptId)!["status"]));

        // With a retake in progress, plain start resumes IT rather than replaying the old result.
        var resume = WorldAttempts.Start(db, sess, null, code, null, false, Utc(2026, 5, 1));
        Assert.Equal(retake.AttemptId, resume.AttemptId);
        Assert.True(resume.Resumed);
        Assert.False(resume.Completed);
    }

    // ───────────────────────── P0-05: the period is the version authority ─────────────────────────

    [Fact]
    public void Midday_publish_never_changes_the_pinned_day_and_start_plays_the_pinned_version()
    {
        var db = NewWorldDb();
        var day = Utc(2026, 6, 10);
        var pinned = WorldLifecycle.TodayPinned(db, day);
        Assert.NotNull(pinned);
        var chId = H.L(pinned!.Value.Challenge["id"]);
        var code = H.Str(pinned.Value.Challenge["code"])!;
        var pinnedVersion = H.L(pinned.Value.Version["version"]);

        // A revision is published mid-day: current_version moves, the period must not.
        var next = pinnedVersion + 1;
        db.Execute(@"INSERT INTO pciworld_challenge_versions(challenge_id,version,title,config_json)
            SELECT challenge_id, ?, title || ' (rev)', config_json FROM pciworld_challenge_versions
            WHERE challenge_id=? AND version=?", next, chId, pinnedVersion);
        db.Execute("UPDATE pciworld_challenges SET current_version=? WHERE id=?", next, chId);

        var after = WorldLifecycle.TodayPinned(db, day)!.Value;
        Assert.Equal(chId, H.L(after.Challenge["id"]));
        Assert.Equal(pinnedVersion, H.L(after.Version["version"]));

        // Attempt start on today's challenge pins the period's version, not current_version.
        var sess = NewSession(db, "s-pin");
        var o = WorldAttempts.Start(db, sess, null, code, null, false, day);
        Assert.Equal(pinnedVersion, o.Version);
    }

    [Fact]
    public void Pause_across_the_boundary_keeps_the_previous_period_on_the_air()
    {
        var db = NewWorldDb();
        var day1 = Utc(2026, 6, 20);
        var served = WorldRotation.CurrentPeriod(db, day1);
        Assert.NotNull(served);

        Settings.Put(db, "world_rotation_enabled", "0");
        var day2 = Utc(2026, 6, 21);
        // No new period opens, but the product is NOT offline: yesterday's period keeps serving.
        var paused = WorldRotation.CurrentPeriod(db, day2);
        Assert.NotNull(paused);
        Assert.Equal(H.L(served!["id"]), H.L(paused!["id"]));
        Assert.NotNull(WorldRotation.Current(db, day2));
        Assert.Null(WorldRotation.ActivePeriod(db, "2026-06-21"));
        Assert.NotNull(WorldLifecycle.TodayPinned(db, day2));

        // Resume opens the next day normally.
        Settings.Put(db, "world_rotation_enabled", "1");
        var resumed = WorldRotation.CurrentPeriod(db, day2);
        Assert.Equal("2026-06-21", H.Str(resumed!["day_key"]));
    }

    // ───────────────────────── P0-06: expiry is independent ─────────────────────────

    [Fact]
    public void Disclosure_saves_that_do_not_mention_expiry_never_change_it()
    {
        var db = NewWorldDb();
        var (_, uid, _) = WorldAccount.Register(db, "exp@x.test", "long-password-1", "E", null);
        WorldPassport.ApplyDisclosure(db, uid, null, null, null, 90);
        var stored = H.Str(db.QueryOne("SELECT passport_expires_at FROM pciworld_users WHERE id=?", uid)!["passport_expires_at"]);
        Assert.False(string.IsNullOrEmpty(stored));

        // Toggling every visibility switch, the display name, evidence and the photo reference
        // leaves the expiry byte-for-byte identical.
        WorldPassport.ApplyDisclosure(db, uid, false, true, false, null);
        db.Execute("UPDATE pciworld_users SET display_name='New Name' WHERE id=?", uid);
        Assert.Equal(stored, H.Str(db.QueryOne("SELECT passport_expires_at FROM pciworld_users WHERE id=?", uid)!["passport_expires_at"]));

        // Only an explicit choice moves it: 0 clears, a positive value re-arms.
        WorldPassport.ApplyDisclosure(db, uid, null, null, null, 0);
        Assert.Null(db.QueryOne("SELECT passport_expires_at FROM pciworld_users WHERE id=?", uid)!["passport_expires_at"]);
        WorldPassport.ApplyDisclosure(db, uid, null, null, null, 30);
        Assert.NotNull(db.QueryOne("SELECT passport_expires_at FROM pciworld_users WHERE id=?", uid)!["passport_expires_at"]);
    }
}
