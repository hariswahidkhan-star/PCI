using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Phase 6 Release-1 (premium share experience, §8.7–8.8) — the server-truth share caption and the
/// extended share listing. The caption is built from PUBLIC fields only: the display name the
/// public Passport already shows, the product name, and the disclaimer sentence — never the email,
/// never a token (even hashed), never the Student Number. A hostile display name arrives neutral.
/// Mirrors tests/world_share_caption_test.py, which runs where the SDK does not.
/// </summary>
public class WorldShareCaptionTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long User(Db db, string email, string? name) =>
        db.ExecuteReturningId(@"INSERT INTO pciworld_users
                (email,password_hash,display_name,email_verified,passport_public,passport_token_sha,student_user_id)
            VALUES(?,?,?,1,1,?,4217)", email, "x", name, Security.Sha("passport-" + email));

    [Fact]
    public void Caption_is_public_fields_only_and_never_carries_email_token_or_number()
    {
        var db = NewWorldDb();
        var uid = User(db, "cap-owner@x.test", "Sam Rivera");
        var cap = WorldAccount.ShareCaption(db, uid);

        Assert.Equal("Sam Rivera — PCI World Passport. " + WorldAccount.ShareCaptionDisclaimer, cap);
        Assert.DoesNotContain("cap-owner", cap);
        Assert.DoesNotContain("@", cap);
        Assert.DoesNotContain(Security.Sha("passport-cap-owner@x.test"), cap);
        Assert.DoesNotContain("4217", cap);
    }

    [Fact]
    public void A_hostile_display_name_arrives_neutral()
    {
        var db = NewWorldDb();
        var uid = User(db, "cap-evil@x.test", "\"><script>alert(1)</script>\r\nFAKE LINE\tZoë");
        var cap = WorldAccount.ShareCaption(db, uid);

        Assert.DoesNotContain("<", cap);
        Assert.DoesNotContain(">", cap);
        Assert.DoesNotContain("\n", cap);
        Assert.DoesNotContain("\r", cap);
        Assert.DoesNotContain("\t", cap);
        Assert.EndsWith(WorldAccount.ShareCaptionDisclaimer, cap);
        Assert.Contains("Zoë", cap);   // legitimate Unicode is untouched
    }

    [Fact]
    public void Without_a_display_name_the_caption_is_the_neutral_product_line()
    {
        var db = NewWorldDb();
        var expected = "PCI World Passport. " + WorldAccount.ShareCaptionDisclaimer;
        Assert.Equal(expected, WorldAccount.ShareCaption(db, User(db, "cap-none@x.test", null)));
        Assert.Equal(expected, WorldAccount.ShareCaption(db, User(db, "cap-blank@x.test", "   ")));
        // an unknown account leaks nothing and does not throw
        Assert.Equal(expected, WorldAccount.ShareCaption(db, 999_999));
    }

    [Fact]
    public void An_overlong_name_is_capped_but_the_disclaimer_always_survives()
    {
        var db = NewWorldDb();
        var cap = WorldAccount.ShareCaption(db, User(db, "cap-long@x.test", new string('N', 500)));
        Assert.Equal(80, cap.Split(" — ")[0].Length);
        Assert.EndsWith("PCI World Passport. " + WorldAccount.ShareCaptionDisclaimer, cap);
    }

    [Fact]
    public void Share_link_rows_carry_the_mint_instant_and_never_a_raw_token()
    {
        var db = NewWorldDb();
        var uid = User(db, "cap-links@x.test", "Owner");
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", Security.Sha("s-cap"));
        var ch = db.QueryOne("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!;
        db.Execute(@"INSERT INTO pciworld_attempts
                (session_id,challenge_id,version,status,score,user_id,result_token_sha,completed_at,updated_at)
            VALUES(?,?,?, 'completed', 80, ?, ?, datetime('now'), datetime('now'))",
            sess, ch["id"], ch["current_version"], uid, Security.Sha("share-cap"));

        var row = Assert.Single(WorldAccount.ShareLinks(db, uid));
        // updated_at is stamped by the mint endpoint — it is the link's creation instant the
        // /api/world/me/shares listing now serves as created_at (no new column, no new token).
        Assert.False(string.IsNullOrEmpty(H.Str(row["updated_at"])));
        Assert.DoesNotContain(row.Keys, k => k.Contains("token", StringComparison.OrdinalIgnoreCase));
    }
}
