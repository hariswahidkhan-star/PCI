using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — deliberate email verification, whole-history statistics and cookie sessions
/// (journey repair P1-03 / P1-06 / P0-07 groundwork).
/// </summary>
public class WorldVerificationAndScaleTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    // ── P1-06: totals are SQL truth, never a page of rows ──

    [Fact]
    public void Statistics_count_the_whole_history_beyond_the_page_window()
    {
        var db = NewWorldDb();
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-scale')");
        var (_, uid, _) = WorldAccount.Register(db, "scale@x.test", "long-password-1", "S", null);
        var ch = db.QueryOne(@"SELECT c.id, c.current_version FROM pciworld_challenges c
            JOIN pciworld_challenge_versions v ON v.challenge_id=c.id AND v.version=c.current_version
            WHERE c.current_version>=1 LIMIT 1")!;
        for (var i = 0; i < 260; i++)
            db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,user_id,passport_visible,completed_at)
                VALUES(?,?,?, 'completed', 60, ?, 1, datetime('now'))", sess, ch["id"], ch["current_version"], uid);

        var stats = WorldAccount.EvidenceStats(db, uid, visibleOnly: false);
        Assert.Equal(260, stats.Completed);

        // The default window is 200; every older row stays reachable through the offset.
        Assert.Equal(200, WorldAccount.EvidenceRows(db, uid, false).Count);
        Assert.Equal(60, WorldAccount.EvidenceRows(db, uid, false, limit: 200, offset: 200).Count);
        Assert.Empty(WorldAccount.EvidenceRows(db, uid, false, limit: 200, offset: 260));

        // Visible-only statistics drive the public Passport and the PDF the same way.
        db.Execute("UPDATE pciworld_attempts SET passport_visible=0 WHERE user_id=? AND id IN (SELECT id FROM pciworld_attempts WHERE user_id=? LIMIT 10)", uid, uid);
        Assert.Equal(250, WorldAccount.EvidenceStats(db, uid, visibleOnly: true).Completed);
    }

    [Fact]
    public void The_pdf_names_its_window_when_it_cannot_draw_every_row()
    {
        var doc = new WorldPassport.PassportDoc
        {
            Name = "Window Test", VerifyUrl = "https://example.org/world/p/x",
            Completed = 300, Industries = 5, Tracks = 3, TotalRows = 300,
            IssuedOn = "2026-07-26",
        };
        for (var i = 0; i < 60; i++)
            doc.Rows.Add(($"Challenge {i}", $"WC-{i:000} · v1", "Energy", "expert", "88", "profile", "2026-07-01"));
        var pdf = WorldPassport.Pdf(doc);
        var text = System.Text.Encoding.Latin1.GetString(pdf);
        Assert.Contains("of 300 published challenges", text);

        // A short history states nothing extra.
        var small = new WorldPassport.PassportDoc
        {
            Name = "Small", VerifyUrl = "https://example.org/world/p/y",
            Completed = 2, Industries = 1, Tracks = 1, TotalRows = 2,
        };
        small.Rows.Add(("One", "WC-001 · v1", "Rail", "foundation", "70", "p", "2026-07-01"));
        small.Rows.Add(("Two", "WC-002 · v1", "Rail", "foundation", "75", "p", "2026-07-02"));
        Assert.DoesNotContain("published challenges — the live record",
            System.Text.Encoding.Latin1.GetString(WorldPassport.Pdf(small)));
    }

    // ── P1-03: verification is deliberate — reads never consume ──

    [Fact]
    public void Verification_tokens_survive_reads_and_die_exactly_once_on_confirm()
    {
        var db = NewWorldDb();
        var (_, uid, _) = WorldAccount.Register(db, "ver@x.test", "long-password-1", "V", null);
        var raw = Security.RandomHex(32);
        db.Execute(@"INSERT INTO pciworld_user_tokens(user_id,purpose,token_sha,expires_at)
            VALUES(?, 'verify', ?, datetime('now','+2 days'))", uid, Security.Sha(raw));

        // The GET-shaped lookup (what a scanner triggers) reads without consuming.
        for (var i = 0; i < 3; i++)
            Assert.NotNull(db.QueryOne(@"SELECT id FROM pciworld_user_tokens
                WHERE token_sha=? AND purpose='verify' AND expires_at>datetime('now')", Security.Sha(raw)));
        Assert.Equal(0L, db.Scalar<long>("SELECT email_verified FROM pciworld_users WHERE id=?", uid));

        // The POST-shaped consumption: single-use DELETE is the lock.
        var row = db.QueryOne("SELECT * FROM pciworld_user_tokens WHERE token_sha=?", Security.Sha(raw))!;
        Assert.Equal(1, db.Execute("DELETE FROM pciworld_user_tokens WHERE id=?", row["id"]));
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", row["user_id"]);
        Assert.Equal(0, db.Execute("DELETE FROM pciworld_user_tokens WHERE id=?", row["id"]));   // replay loses
        Assert.Equal(1L, db.Scalar<long>("SELECT email_verified FROM pciworld_users WHERE id=?", uid));
    }

    [Fact]
    public void The_verification_page_states_are_distinct()
    {
        var db = NewWorldDb();
        Assert.Contains("Verify my email", WorldPages.VerifyEmail(db, "confirm"));
        Assert.Contains("already verified", WorldPages.VerifyEmail(db, "already"));
        Assert.Contains("didn&rsquo;t work", WorldPages.VerifyEmail(db, "invalid"));
        // The confirm page never carries the token in its HTML — the script reads the URL itself.
        Assert.DoesNotContain("token=", WorldPages.VerifyEmail(db, "confirm").Split("</script>")[0].Split("<script>")[0]);
    }

    // ── P0-07 groundwork: cookie sessions, header wins ──

    [Fact]
    public void The_session_cookie_authenticates_and_the_header_wins_when_both_exist()
    {
        var db = NewWorldDb();
        var (_, uid, token) = WorldAccount.Register(db, "cookie@x.test", "long-password-1", "C", null);

        var ctx = new Microsoft.AspNetCore.Http.DefaultHttpContext();
        ctx.Request.Headers["Cookie"] = $"{WorldAccount.SessionCookie}={token}";
        var viaCookie = WorldAccount.FromReq(ctx.Request, db);
        Assert.NotNull(viaCookie);
        Assert.Equal(uid, viaCookie!.Id);

        // Garbage cookie → anonymous, never an exception.
        var bad = new Microsoft.AspNetCore.Http.DefaultHttpContext();
        bad.Request.Headers["Cookie"] = $"{WorldAccount.SessionCookie}=deadbeef";
        Assert.Null(WorldAccount.FromReq(bad.Request, db));

        // The explicit header outranks a (different) cookie.
        var (_, uid2, token2) = WorldAccount.Register(db, "cookie2@x.test", "long-password-1", "C2", null);
        var both = new Microsoft.AspNetCore.Http.DefaultHttpContext();
        both.Request.Headers["X-World-Account"] = token2;
        both.Request.Headers["Cookie"] = $"{WorldAccount.SessionCookie}={token}";
        Assert.Equal(uid2, WorldAccount.FromReq(both.Request, db)!.Id);
    }
}
