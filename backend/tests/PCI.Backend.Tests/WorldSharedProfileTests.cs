using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — shared canonical student profile (journey repair P1-10 groundwork).
///
/// ONE profile, both products: the World surface reads and writes the SAME canonical
/// users/student_profiles records the PCI portal uses. A save made on either side is simply THE
/// profile on the other side's next read — no copy, no sync job, no pciworld_profile table. The
/// Passport's disclosure remains a separate consent layer: nothing read through this bridge
/// becomes public anywhere by existing.
/// </summary>
public class WorldSharedProfileTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static Dictionary<string, JsonElement> Body(string json) =>
        JsonDocument.Parse(json).RootElement.EnumerateObject()
            .ToDictionary(p => p.Name, p => p.Value.Clone());

    [Fact]
    public void A_world_side_edit_lands_on_the_canonical_row_and_recomputes_completion()
    {
        var db = NewWorldDb();
        // A learner registers on World → one canonical identity is minted (it2 behaviour).
        var (_, wid, _) = WorldAccount.Register(db, "shared@x.test", "long-password-1", "S P", null);
        var uid = WorldIdentity.CanonicalUserFor(db, wid)!.Value;

        var err = WorldIdentity.UpdateSharedProfile(db, wid,
            Body("""{"current_role":"Senior Planner","company":"Meridian Rail","country":"DE","linkedin_url":"https://linkedin.com/in/sp"}"""));
        Assert.Null(err);

        // The CANONICAL row carries the values — the same row the portal's /api/me reads.
        var p = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", uid)!;
        Assert.Equal("Senior Planner", H.Str(p["current_role"]));
        Assert.Equal("Meridian Rail", H.Str(p["company"]));
        Assert.Equal("DE", H.Str(p["country"]));
        // Completion is the portal's own weighted formula, recomputed on this write.
        Assert.True(H.L(p["profile_completion_percentage"]) > 20);
        // No duplicate profile store anywhere in the World realm.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM student_profiles WHERE user_id=?", uid));
    }

    [Fact]
    public void A_portal_side_edit_is_visible_on_the_next_world_read()
    {
        var db = NewWorldDb();
        var uid = db.ExecuteReturningId(@"INSERT INTO users(email,first_name,last_name,password_hash,role,status)
            VALUES('portal-edit@x.test','Pat','Lee',?, 'student','active')", BCrypt.Net.BCrypt.HashPassword("portal-password-1"));
        db.Execute("INSERT INTO student_profiles(user_id, city) VALUES(?, 'Lisbon')", uid);
        var (err, wid) = WorldAccount.LinkStudent(db, uid, "portal-edit@x.test", "Pat Lee");
        Assert.Null(err);

        var first = WorldIdentity.ReadSharedProfile(db, wid)!;
        Assert.Equal("Pat", H.Str(first["first_name"]));
        Assert.Equal("Lisbon", H.Str(first["city"]));

        // The portal edits the canonical record (what PATCH /api/me/profile does)…
        db.Execute("UPDATE student_profiles SET city='Porto', industry_sector='Energy' WHERE user_id=?", uid);
        // …and the World surface sees it on the very next read. No sync, same row.
        var second = WorldIdentity.ReadSharedProfile(db, wid)!;
        Assert.Equal("Porto", H.Str(second["city"]));
        Assert.Equal("Energy", H.Str(second["industry_sector"]));
    }

    [Fact]
    public void An_unmapped_world_account_reads_null_and_cannot_write()
    {
        var db = NewWorldDb();
        // A quarantined conflict: same email as an existing canonical user, never merged.
        db.ExecuteReturningId(@"INSERT INTO users(email,first_name,password_hash,role,status)
            VALUES('clash-prof@x.test','C', 'x','student','active')");
        var wid = db.ExecuteReturningId(@"INSERT INTO pciworld_users(email,password_hash)
            VALUES('clash-prof@x.test', ?)", BCrypt.Net.BCrypt.HashPassword("world-password-1"));
        WorldIdentity.Run(db);

        Assert.Null(WorldIdentity.ReadSharedProfile(db, wid));
        Assert.Equal("not_linked", WorldIdentity.UpdateSharedProfile(db, wid, Body("""{"city":"Nowhere"}""")));
        // The stranger's canonical profile was never touched.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM student_profiles WHERE city='Nowhere'"));
    }

    [Fact]
    public void Only_the_allow_listed_fields_are_writable()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "strict@x.test", "long-password-1", null, null);
        var uid = WorldIdentity.CanonicalUserFor(db, wid)!.Value;

        // Attempts to smuggle identity/credential/photo fields are simply ignored.
        var err = WorldIdentity.UpdateSharedProfile(db, wid, Body("""
            {"city":"Berlin","first_name":"Hacked","password_hash":"x","profile_photo":"evil.png",
             "profile_completion_percentage":"100","user_id":"1"}
            """));
        Assert.Null(err);
        Assert.Equal("Berlin", db.Scalar<string>("SELECT city FROM student_profiles WHERE user_id=?", uid));
        Assert.NotEqual("Hacked", db.Scalar<string>("SELECT first_name FROM users WHERE id=?", uid));
        Assert.Null(db.Scalar<string>("SELECT profile_photo FROM student_profiles WHERE user_id=?", uid));
        Assert.True(Security.VerifyPassword("long-password-1",
            db.Scalar<string>("SELECT password_hash FROM users WHERE id=?", uid)));
    }

    [Fact]
    public void Shared_profile_data_never_reaches_the_public_passport_by_existing()
    {
        var db = NewWorldDb();
        var (_, wid, _) = WorldAccount.Register(db, "priv-prof@x.test", "long-password-1", "Priva Te", null);
        WorldIdentity.UpdateSharedProfile(db, wid, Body("""{"company":"SecretCo GmbH","mobile":"+49-555-0100"}"""));
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", wid);

        // Publish a Passport with one visible attempt; the shared profile fields must not appear.
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-priv-prof')");
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!;
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,user_id,passport_visible,completed_at)
            VALUES(?,?,?, 'completed', 90, ?, 1, datetime('now'))", sess, ch["id"], ch["current_version"], wid);
        var (perr, url) = WorldAccount.PublishPassport(db, wid);
        Assert.Null(perr);

        var rows = WorldAccount.EvidenceRows(db, wid, visibleOnly: true);
        var page = WorldPages.PublicPassport(db, "Priva Te", rows, WorldPassport.Disclosure.From(
            db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", wid)!));
        Assert.DoesNotContain("SecretCo", page);
        Assert.DoesNotContain("555-0100", page);
        Assert.DoesNotContain("priv-prof@x.test", page);
        _ = url;
    }
}
