using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — Phase 7: the share → challenge → anonymous response → claim journey, hardened.
///
/// Proves, against the real code paths (WorldAccount.ClaimSession, WorldAttempts.Start/Submit,
/// World.RecordReferralStart/RecordReferralCompletion):
///
///   • one anonymous attempt is claimed by exactly ONE account — the guarded UPDATE
///     (WHERE user_id IS NULL) is the lock; the loser's claim is a no-op
///   • the winner's claim is idempotent: re-login duplicates and moves nothing
///   • a completed attempt can never be re-scored: retries replay, different payloads are refused
///   • scoring is backend-only: a client-supplied score field never reaches the stored result
///   • a revoked result share resolves nothing; an invitation pins the immutable version even
///     after the challenge is edited
///   • referral attribution is privacy-safe (opaque share_ref, numeric ids, aggregate counts for
///     the sharer) and single-winner on claim
///   • the claim/score paths write pciworld_* tables only — certification exam tables untouched
///
/// The same invariants are transcribed to SQL in tests/world_claim_referral_test.py, which runs
/// in this sandbox; this suite is the CI gate over the actual C#.
/// </summary>
public class WorldClaimReferralTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long NewSession(Db db, string sha) =>
        db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", sha);

    static JsonElement Json(string raw) => JsonDocument.Parse(raw).RootElement;

    static (long Id, string Code, long Version) AnyChallenge(Db db, int skip = 0)
    {
        var r = db.Query(@"SELECT c.id, c.code, c.current_version FROM pciworld_challenges c
            JOIN pciworld_challenge_versions v ON v.challenge_id=c.id AND v.version=c.current_version
            WHERE c.current_version>=1 AND c.retired=0 ORDER BY c.id LIMIT 1 OFFSET " + skip)[0];
        return (H.L(r["id"]), H.Str(r["code"])!, H.L(r["current_version"]));
    }

    static long CompletedAttempt(Db db, long sess, long challengeId, long version, long? userId = null) =>
        db.ExecuteReturningId(@"INSERT INTO pciworld_attempts
                (session_id,challenge_id,version,status,score,answers_json,user_id,completed_at)
            VALUES(?,?,?, 'completed', 66, '{}', ?, datetime('now'))", sess, challengeId, version, userId);

    [Fact]
    public void Claim_race_has_one_winner_and_the_losers_claim_is_a_noop()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-race");
        var (chId, _, ver) = AnyChallenge(db);
        var a1 = CompletedAttempt(db, sess, chId, ver);
        var a2 = CompletedAttempt(db, sess, chId, ver);
        var (_, winner, _) = WorldAccount.Register(db, "race-w@x.test", "long-password-1", "W", null);
        var (_, loser, _) = WorldAccount.Register(db, "race-l@x.test", "long-password-1", "L", null);

        // Sequenced guarded updates ARE the race outcome: whichever claim lands first flips
        // user_id from NULL; the second claim's WHERE user_id IS NULL then matches nothing.
        WorldAccount.ClaimSession(db, winner, sess);
        WorldAccount.ClaimSession(db, loser, sess);

        foreach (var att in new[] { a1, a2 })
            Assert.Equal(winner, Convert.ToInt64(
                db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", att)!["user_id"]));
        // The loser stole nothing and duplicated nothing.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts WHERE user_id=?", loser));
        Assert.Equal(2L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts WHERE session_id=?", sess));
    }

    [Fact]
    public void Claim_is_idempotent_for_the_winner()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-idem");
        var (chId, _, ver) = AnyChallenge(db);
        var att = CompletedAttempt(db, sess, chId, ver);
        var (_, uid, _) = WorldAccount.Register(db, "idem@x.test", "long-password-1", "I", sess);

        var before = db.Query("SELECT id,user_id,score,status FROM pciworld_attempts WHERE session_id=? ORDER BY id", sess);
        // Re-login claims again; a retried claim replays too. Nothing may change.
        WorldAccount.Login(db, "idem@x.test", "long-password-1", sess);
        WorldAccount.ClaimSession(db, uid, sess);
        var after = db.Query("SELECT id,user_id,score,status FROM pciworld_attempts WHERE session_id=? ORDER BY id", sess);

        Assert.Equal(before.Count, after.Count);
        for (var i = 0; i < before.Count; i++)
        {
            Assert.Equal(H.L(before[i]["id"]), H.L(after[i]["id"]));
            Assert.Equal(H.L(before[i]["user_id"]), H.L(after[i]["user_id"]));
        }
        Assert.Equal(uid, Convert.ToInt64(db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", att)!["user_id"]));
    }

    [Fact]
    public void A_client_supplied_score_is_ignored_and_a_graded_result_is_immutable()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-tamper");
        var (chId, code, _) = AnyChallenge(db);
        var o = WorldAttempts.Start(db, sess, null, code, null, false, DateTime.UtcNow);
        Assert.Null(o.Error);

        // The submit body's only read field is `answers`; a score smuggled inside it is an unknown
        // key the grader never looks at. The stored score must equal the snapshot-derived grade.
        var answers = Json("""{"score": 100, "result": "pass", "cheat": true}""");
        var s1 = WorldAttempts.Submit(db, sess, null, o.AttemptId, answers);
        Assert.Null(s1.Error);
        // Re-derive independently from the immutable snapshot: that, and only that, is the score.
        var expected = WorldScore.Grade(Json(H.Str(db.QueryOne(
            "SELECT config_json FROM pciworld_challenge_versions WHERE challenge_id=? AND version=?",
            chId, o.Version)!["config_json"])!), answers).Score;
        var stored = Convert.ToDouble(db.QueryOne("SELECT score FROM pciworld_attempts WHERE id=?", o.AttemptId)!["score"]);
        Assert.Equal(expected, stored);

        // Idempotent retry (double click / lost response): replayed, unchanged, still one attempt.
        var s2 = WorldAttempts.Submit(db, sess, null, o.AttemptId, answers);
        Assert.True(s2.Replayed);
        Assert.Equal(stored, Convert.ToDouble(db.QueryOne("SELECT score FROM pciworld_attempts WHERE id=?", o.AttemptId)!["score"]));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts WHERE session_id=?", sess));

        // A different payload against the graded attempt is refused and changes nothing.
        var s3 = WorldAttempts.Submit(db, sess, null, o.AttemptId, Json("""{"different": 1}"""));
        Assert.Equal("already_submitted", s3.Error);
        Assert.Equal(stored, Convert.ToDouble(db.QueryOne("SELECT score FROM pciworld_attempts WHERE id=?", o.AttemptId)!["score"]));
    }

    [Fact]
    public void Revoked_share_404s_and_invitations_pin_the_version_after_an_edit()
    {
        var db = NewWorldDb();
        var sess = NewSession(db, "s-links");
        var (chId, code, ver) = AnyChallenge(db);
        var att = CompletedAttempt(db, sess, chId, ver);
        db.Execute("UPDATE pciworld_attempts SET result_token_sha=? WHERE id=?", Security.Sha("r-tok"), att);
        db.Execute("INSERT INTO pciworld_invites(attempt_id,token_sha) VALUES(?,?)", att, Security.Sha("i-tok"));

        // The exact /world/r/{token} resolution: live resolves, revoked resolves nothing, and a
        // revoked token is indistinguishable from one that never existed.
        string resultSql = @"SELECT id FROM pciworld_attempts
            WHERE result_token_sha=? AND result_revoked=0 AND status='completed'";
        Assert.NotNull(db.QueryOne(resultSql, Security.Sha("r-tok")));
        db.Execute("UPDATE pciworld_attempts SET result_revoked=1 WHERE id=?", att);
        Assert.Null(db.QueryOne(resultSql, Security.Sha("r-tok")));
        Assert.Null(db.QueryOne(resultSql, Security.Sha("never-existed")));

        // The author edits the challenge and publishes a new version.
        var newVer = ver + 1;
        db.Execute(@"INSERT INTO pciworld_challenge_versions(challenge_id,version,title,config_json)
            SELECT challenge_id, ?, 'EDITED', config_json FROM pciworld_challenge_versions
            WHERE challenge_id=? AND version=?", newVer, chId, ver);
        db.Execute("UPDATE pciworld_challenges SET current_version=? WHERE id=?", newVer, chId);

        // The invited friend still plays the inviter's immutable version; a plain start plays the new one.
        var friend = NewSession(db, "s-friend-pin");
        var viaInvite = WorldAttempts.Start(db, friend, null, code, "i-tok", false, DateTime.UtcNow);
        Assert.Null(viaInvite.Error);
        Assert.Equal(ver, viaInvite.Version);
        var plain = WorldAttempts.Start(db, NewSession(db, "s-plain-pin"), null, code, null, false, DateTime.UtcNow);
        Assert.Equal(newVer, plain.Version);

        // Revoking the invitation kills the pin AND the landing page lookup.
        db.Execute("UPDATE pciworld_invites SET revoked=1 WHERE token_sha=?", Security.Sha("i-tok"));
        Assert.Null(db.QueryOne(@"SELECT i.id FROM pciworld_invites i
            JOIN pciworld_attempts a ON a.id=i.attempt_id
            WHERE i.token_sha=? AND i.revoked=0", Security.Sha("i-tok")));
        var afterRevoke = WorldAttempts.Start(db, NewSession(db, "s-after-rev"), null, code, "i-tok", false, DateTime.UtcNow);
        Assert.Equal(newVer, afterRevoke.Version);   // the revoked pin no longer applies
    }

    [Fact]
    public void Referral_lifecycle_is_privacy_safe_and_single_winner_on_claim()
    {
        var db = NewWorldDb();
        var inviterSess = NewSession(db, "s-inviter");
        var (chId, code, ver) = AnyChallenge(db);
        var inviterAtt = CompletedAttempt(db, inviterSess, chId, ver);
        db.Execute("INSERT INTO pciworld_invites(attempt_id,token_sha,inviter_name) VALUES(?,?,?)",
            inviterAtt, Security.Sha("ref-raw-token"), "Aisha");
        // The inviter must be able to SEE the counts — own the attempt.
        var (_, sharer, _) = WorldAccount.Register(db, "sharer@x.test", "long-password-1", "S", inviterSess);

        // Anonymous friend opens the link and starts (exactly what POST /api/world/attempts does).
        var friend = NewSession(db, "s-ref-friend");
        var o = WorldAttempts.Start(db, friend, null, code, "ref-raw-token", false, DateTime.UtcNow);
        World.RecordReferralStart(db, "ref-raw-token", friend, null, o.ChallengeId, o.Version);
        World.RecordReferralStart(db, "ref-raw-token", friend, null, o.ChallengeId, o.Version);   // refresh
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_referrals"));
        var row = db.QueryOne("SELECT * FROM pciworld_referrals")!;
        Assert.Equal("started", H.Str(row["conversion_state"]));
        Assert.Null(row["referred_user_id"]);
        // Privacy by schema: the opaque hash, numeric ids, nothing else — no raw token, no name,
        // no email anywhere in the row.
        Assert.Equal(Security.Sha("ref-raw-token"), H.Str(row["share_ref"]));
        foreach (var v in row.Values)
        {
            var s = Convert.ToString(v) ?? "";
            Assert.DoesNotContain("ref-raw-token", s);
            Assert.DoesNotContain("@", s);
            Assert.DoesNotContain("Aisha", s);
        }

        // Completion advances it; a replayed submit never advances it twice.
        var sub = WorldAttempts.Submit(db, friend, null, o.AttemptId, Json("""{"a":"1"}"""));
        Assert.Null(sub.Error);
        World.RecordReferralCompletion(db, friend, o.ChallengeId, o.Version);
        var completedAt = H.Str(db.QueryOne("SELECT completed_at FROM pciworld_referrals")!["completed_at"]);
        Assert.False(string.IsNullOrEmpty(completedAt));
        World.RecordReferralCompletion(db, friend, o.ChallengeId, o.Version);
        Assert.Equal(completedAt, H.Str(db.QueryOne("SELECT completed_at FROM pciworld_referrals")!["completed_at"]));
        Assert.Equal("completed", H.Str(db.QueryOne("SELECT conversion_state FROM pciworld_referrals")!["conversion_state"]));

        // Claim: exactly one account wins the attribution; the rival's claim is a no-op.
        var (_, claimer, _) = WorldAccount.Register(db, "claimer@x.test", "long-password-1", "C", friend);
        var (_, rival, _) = WorldAccount.Register(db, "rival@x.test", "long-password-1", "R", null);
        WorldAccount.ClaimSession(db, rival, friend);
        row = db.QueryOne("SELECT * FROM pciworld_referrals")!;
        Assert.Equal(claimer, H.L(row["referred_user_id"]));
        Assert.Equal("claimed", H.Str(row["conversion_state"]));

        // The sharer reads aggregate counts only — never who.
        var inv = WorldAccount.Invitations(db, sharer).Single();
        Assert.Equal(1L, H.L(inv["ref_started"]));
        Assert.Equal(1L, H.L(inv["ref_completed"]));
        Assert.Equal(1L, H.L(inv["ref_claimed"]));
        Assert.False(inv.ContainsKey("referred_user_id"));
        Assert.False(inv.ContainsKey("anonymous_world_session_id"));
    }

    [Fact]
    public void Claim_and_score_paths_write_only_world_tables()
    {
        var db = NewWorldDb();
        var (chId, code, ver) = AnyChallenge(db);
        // Accounts exist first: registration legitimately creates canonical identity; the paths
        // under test here are claim + score, which must stay inside the pciworld_ realm.
        var (_, uid, _) = WorldAccount.Register(db, "realm@x.test", "long-password-1", "R", null);
        var inviterSess = NewSession(db, "s-realm-inviter");
        var inviterAtt = CompletedAttempt(db, inviterSess, chId, ver);
        db.Execute("INSERT INTO pciworld_invites(attempt_id,token_sha) VALUES(?,?)", inviterAtt, Security.Sha("realm-inv"));

        string[] fence = { "exam_attempts", "exam_entitlements", "exam_bookings", "exam_score_snapshots",
            "exam_launch_codes", "exam_evidence", "issued_credentials", "practice_attempts",
            "users", "student_profiles", "login_tokens", "audit_logs" };
        var before = fence.ToDictionary(t => t, t => db.Scalar<long>($"SELECT COUNT(*) FROM {t}"));

        // The full journey: start via share link, save, double-submit (tampered), claim, re-claim.
        var sess = NewSession(db, "s-realm");
        var o = WorldAttempts.Start(db, sess, null, code, "realm-inv", false, DateTime.UtcNow);
        World.RecordReferralStart(db, "realm-inv", sess, null, o.ChallengeId, o.Version);
        db.Execute(@"UPDATE pciworld_attempts SET answers_json='{""a"":""1""}' WHERE id=?", o.AttemptId);
        WorldAttempts.Submit(db, sess, null, o.AttemptId, Json("""{"a":"1","score":100}"""));
        WorldAttempts.Submit(db, sess, null, o.AttemptId, Json("""{"a":"1","score":100}"""));
        World.RecordReferralCompletion(db, sess, o.ChallengeId, o.Version);
        WorldAccount.ClaimSession(db, uid, sess);
        WorldAccount.ClaimSession(db, uid, sess);

        foreach (var t in fence)
            Assert.Equal(before[t], db.Scalar<long>($"SELECT COUNT(*) FROM {t}"));
        // And the journey really happened, inside the realm.
        Assert.Equal("completed", H.Str(db.QueryOne("SELECT status FROM pciworld_attempts WHERE id=?", o.AttemptId)!["status"]));
        Assert.Equal(uid, H.L(db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", o.AttemptId)!["user_id"]));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_referrals WHERE referred_user_id=?", uid));
    }
}
