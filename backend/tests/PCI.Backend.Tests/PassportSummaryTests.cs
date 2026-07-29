using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Phase 4 — the MyPCI Passport summary read model (Endpoints/PassportSummary.cs).
///
/// The contract under test: ONE authoritative Passport, read from inside MyPCI with no iframe and
/// no data copy. The summary's counts and status must be the values the World side's own functions
/// (WorldAccount.EvidenceStats / EvidenceRows, WorldPassport.Expired / Disclosure) compute for the
/// same canonical user; a student with no World participation gets a well-formed not_created
/// summary rather than an error; a quarantined email collision surfaces as link_pending and leaks
/// nothing of the unproven account; and the serialized payload carries no internal identifier and
/// no token material — the stored passport token is a hash, so publicUrl is omitted rather than
/// re-minted.
/// </summary>
public class PassportSummaryTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long MkStudent(Db db, string email)
    {
        var uid = db.ExecuteReturningId(@"INSERT INTO users(email,first_name,last_name,password_hash,role,status)
            VALUES(?,?,?,?,'student','active')", email, "Jordan", "Q", BCrypt.Net.BCrypt.HashPassword("portal-pw-1"));
        StudentNumbers.GetOrIssue(db, uid, "test_fixture");
        return uid;
    }

    static long LinkWorld(Db db, long uid, string email)
    {
        var (err, wid) = WorldAccount.LinkStudent(db, uid, email, "Jordan Q.");
        Assert.Null(err);
        return wid;
    }

    static void MkEvidence(Db db, long wid, long uid, string code, bool visible = true,
        string completed = "2026-06-01 09:00:00", double score = 82.5)
    {
        var cid = db.QueryOne("SELECT id FROM pciworld_challenges WHERE code=?", code) is { } ch
            ? H.L(ch["id"])
            : db.ExecuteReturningId("INSERT INTO pciworld_challenges(code,title) VALUES(?,?)", code, "T " + code);
        if (db.QueryOne("SELECT id FROM pciworld_challenge_versions WHERE challenge_id=? AND version=1", cid) is null)
            db.Execute(@"INSERT INTO pciworld_challenge_versions(challenge_id,version,title,industry,track,difficulty,config_json)
                VALUES(?,?,?,?,?,?,'{}')", cid, 1, "T " + code, "Energy", "project_controls", "professional");
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,profile_key,
                completed_at,user_id,passport_visible,canonical_user_id)
            VALUES(0,?,1,'completed',?,'steady_navigator',?,?,?,?)",
            cid, score, completed, wid, visible ? 1 : 0, uid);
    }

    [Fact]
    public void No_world_participation_is_a_well_formed_not_created_summary()
    {
        var db = NewWorldDb();
        var uid = MkStudent(db, "solo@x.test");

        var s = PassportSummary.Build(db, uid, "solo@x.test");

        Assert.Equal(1, s.SchemaVersion);
        Assert.Equal("none", s.ParticipantState);
        Assert.Equal("not_created", s.PassportState);
        Assert.Equal(new[] { "create" }, s.AvailableActions);
        Assert.Equal(StudentNumbers.Read(db, uid), s.StudentNumber);
        Assert.Equal(0L, s.CompletedChallengeCount);
        Assert.Empty(s.EvidenceHighlights);
        Assert.False(s.CanShowPublicUrl);
        Assert.Null(s.PublicUrl);
    }

    [Fact]
    public void A_quarantined_email_collision_surfaces_link_pending_and_leaks_nothing()
    {
        var db = NewWorldDb();
        var uid = MkStudent(db, "taken@x.test");
        // A standalone World registration with the SAME email is quarantined, never merged.
        var (err, wid, _) = WorldAccount.Register(db, "taken@x.test", "long-password-1", "Unproven", null);
        Assert.Null(err);
        Assert.True(WorldIdentity.LinkPending(db, wid));

        var s = PassportSummary.Build(db, uid, "taken@x.test");

        Assert.Equal("link_pending", s.ParticipantState);
        Assert.Equal("not_created", s.PassportState);
        Assert.Equal(new[] { "link" }, s.AvailableActions);
        // Nothing of the unproven account: no display name, no counts, no evidence.
        Assert.Null(s.DisplayName);
        Assert.Equal(0L, s.CompletedChallengeCount);
        Assert.Empty(s.EvidenceHighlights);
    }

    [Fact]
    public void Counts_and_state_are_the_world_sides_own_values_for_the_same_user()
    {
        var db = NewWorldDb();
        var uid = MkStudent(db, "same@x.test");
        var wid = LinkWorld(db, uid, "same@x.test");
        // One row owned in the legacy namespace only, one canonical-only, one hidden — the union
        // predicate must count each exactly once, same as every World surface.
        MkEvidence(db, wid, 0, "WC-A-1", completed: "2026-06-02 08:00:00");
        db.Execute("UPDATE pciworld_attempts SET canonical_user_id=NULL WHERE user_id=?", wid);
        MkEvidence(db, 0, uid, "WC-B-2", completed: "2026-06-03 08:00:00");
        db.Execute("UPDATE pciworld_attempts SET user_id=NULL WHERE challenge_id IN (SELECT id FROM pciworld_challenges WHERE code='WC-B-2')");
        MkEvidence(db, wid, uid, "WC-C-3", visible: false, completed: "2026-06-04 08:00:00");

        var s = PassportSummary.Build(db, uid, "same@x.test");
        var all = WorldAccount.EvidenceStats(db, wid, visibleOnly: false);
        var sel = WorldAccount.EvidenceStats(db, wid, visibleOnly: true);

        Assert.Equal(all.Completed, s.CompletedChallengeCount);
        Assert.Equal(3L, s.CompletedChallengeCount);
        Assert.Equal(sel.Completed, s.SelectedEvidenceCount);
        Assert.Equal(2L, s.SelectedEvidenceCount);
        // Highlights are DISCLOSED rows only, newest first — the same rows EvidenceRows returns.
        Assert.Equal(2, s.EvidenceHighlights.Count);
        Assert.Equal("T WC-B-2", s.EvidenceHighlights[0].Title);
        // lastUpdatedAt is the newest completed evidence in either visibility.
        Assert.Equal("2026-06-04 08:00:00", s.LastUpdatedAt);
        // LinkStudent marks the bridge account verified with a display name and there is selected
        // evidence, so the readiness state is private — the dashboard's private_ready.
        Assert.Equal("private", s.PassportState);
        Assert.Equal(new[] { "manage", "publish" }, s.AvailableActions);
    }

    [Fact]
    public void Published_and_expired_states_follow_the_existing_publication_functions()
    {
        var db = NewWorldDb();
        var uid = MkStudent(db, "pub@x.test");
        var wid = LinkWorld(db, uid, "pub@x.test");
        MkEvidence(db, wid, uid, "WC-A-1");

        var (perr, url) = WorldAccount.PublishPassport(db, wid);
        Assert.Null(perr);
        Assert.NotNull(url);

        var s = PassportSummary.Build(db, uid, "pub@x.test");
        Assert.Equal("published", s.PassportState);
        // Only the token HASH is stored — the raw /world/p/… path cannot be reconstructed and a
        // read must never rotate the owner's link by minting a new one.
        Assert.False(s.CanShowPublicUrl);
        Assert.Null(s.PublicUrl);

        db.Execute("UPDATE pciworld_users SET passport_expires_at='2020-01-01 00:00:00' WHERE id=?", wid);
        var e = PassportSummary.Build(db, uid, "pub@x.test");
        Assert.Equal("expired", e.PassportState);
        Assert.Equal("2020-01-01", e.ExpiresAt);
        Assert.Equal(new[] { "manage", "renew" }, e.AvailableActions);
    }

    [Fact]
    public void Suspension_on_either_record_reports_suspended()
    {
        var db = NewWorldDb();
        var uid = MkStudent(db, "susp@x.test");
        var wid = LinkWorld(db, uid, "susp@x.test");

        db.Execute("UPDATE pciworld_users SET status='suspended' WHERE id=?", wid);
        Assert.Equal("suspended", PassportSummary.Build(db, uid, "susp@x.test").PassportState);

        db.Execute("UPDATE pciworld_users SET status='active' WHERE id=?", wid);
        db.Execute("UPDATE pciworld_participants SET status='suspended' WHERE user_id=?", uid);
        var s = PassportSummary.Build(db, uid, "susp@x.test");
        Assert.Equal("suspended", s.PassportState);
        Assert.Equal(new[] { "contact_support" }, s.AvailableActions);
    }

    [Fact]
    public void Disclosure_switches_shape_the_highlights_exactly_like_the_public_page()
    {
        var db = NewWorldDb();
        var uid = MkStudent(db, "disc@x.test");
        var wid = LinkWorld(db, uid, "disc@x.test");
        MkEvidence(db, wid, uid, "WC-A-1");
        WorldPassport.ApplyDisclosure(db, wid, showScores: true, showProfiles: true, showDates: true, expiresInDays: null);

        var on = PassportSummary.Build(db, uid, "disc@x.test").EvidenceHighlights[0];
        Assert.Equal(82.5, on.Score);
        Assert.Equal("steady navigator", on.Profile);
        Assert.Equal("2026-06-01", on.CompletedOn);

        WorldPassport.ApplyDisclosure(db, wid, showScores: false, showProfiles: false, showDates: false, expiresInDays: null);
        var off = PassportSummary.Build(db, uid, "disc@x.test");
        Assert.Null(off.EvidenceHighlights[0].Score);
        Assert.Null(off.EvidenceHighlights[0].Profile);
        Assert.Null(off.EvidenceHighlights[0].CompletedOn);
        Assert.False(off.Disclosure.Scores);

        // Absent-when-off in the wire shape too: the switched-off keys are not serialized.
        var json = JsonSerializer.Serialize(off);
        Assert.DoesNotContain("\"score\"", json);
        Assert.DoesNotContain("\"profile\"", json);
        Assert.DoesNotContain("\"completedOn\"", json);
    }

    [Fact]
    public void The_serialized_payload_exposes_no_internal_id_email_or_token_material()
    {
        var db = NewWorldDb();
        var uid = MkStudent(db, "leak@x.test");
        var wid = LinkWorld(db, uid, "leak@x.test");
        MkEvidence(db, wid, uid, "WC-A-1");
        var (perr, _) = WorldAccount.PublishPassport(db, wid);
        Assert.Null(perr);
        var sha = H.Str(db.QueryOne("SELECT passport_token_sha FROM pciworld_users WHERE id=?", wid)!["passport_token_sha"])!;

        var json = JsonSerializer.Serialize(PassportSummary.Build(db, uid, "leak@x.test"));

        Assert.DoesNotContain("leak@x.test", json);
        Assert.DoesNotContain(sha, json);
        Assert.DoesNotContain("user_id", json);
        Assert.DoesNotContain("\"id\"", json);
        Assert.DoesNotContain("publicUrl", json);          // hash-only → omitted, not null
        // The one public identifier IS present.
        Assert.Contains(StudentNumbers.Read(db, uid)!, json);
    }
}
