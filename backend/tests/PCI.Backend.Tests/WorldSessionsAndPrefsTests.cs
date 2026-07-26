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
