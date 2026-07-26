using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — the OAuth 2.1-shaped authorization layer (dedicated-domain groundwork).
///
/// Attack matrix: S256-only PKCE, exact redirect matching against the client registry, uniform
/// invalid_grant (no oracle), single-use codes where a failed guess burns the code, replay
/// detection that revokes the stolen code's minted session, and expiry.
/// </summary>
public class WorldOAuthTests
{
    const string Client = "pciworld-app";
    const string Redirect = "/world/account";
    const string Verifier = "correct-horse-battery-staple-correct-horse-battery-staple";

    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long NewWorldUser(Db db, string email)
    {
        var (_, wid, _) = WorldAccount.Register(db, email, "long-password-1", "O", null);
        return wid;
    }

    [Fact]
    public void The_full_pkce_round_trip_mints_a_working_session()
    {
        var db = NewWorldDb();
        var wid = NewWorldUser(db, "oauth-ok@x.test");
        var (ierr, code) = WorldOAuth.IssueCode(db, wid, Client, Redirect, WorldOAuth.S256(Verifier), "S256");
        Assert.Null(ierr);
        Assert.StartsWith("WOA", code);

        var (xerr, gotWid, token) = WorldOAuth.ExchangeCode(db, code, Verifier, Client, Redirect);
        Assert.Null(xerr);
        Assert.Equal(wid, gotWid);
        // The minted token is a real World session.
        Assert.Equal(1L, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_user_sessions WHERE token=? AND expires_at>datetime('now')", Security.Sha(token)));
        // The raw code is stored only as a hash.
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_oauth_codes WHERE code_sha=?", code));
    }

    [Fact]
    public void Issue_refuses_plain_method_bad_challenges_unknown_clients_and_foreign_redirects()
    {
        var db = NewWorldDb();
        var wid = NewWorldUser(db, "oauth-issue@x.test");
        var ch = WorldOAuth.S256(Verifier);
        Assert.Equal("unsupported_challenge_method", WorldOAuth.IssueCode(db, wid, Client, Redirect, ch, "plain").Err);
        Assert.Equal("invalid_challenge", WorldOAuth.IssueCode(db, wid, Client, Redirect, "short", "S256").Err);
        Assert.Equal("invalid_challenge", WorldOAuth.IssueCode(db, wid, Client, Redirect, new string('a', 43) + "<script>", "S256").Err);
        Assert.Equal("invalid_client_or_redirect", WorldOAuth.IssueCode(db, wid, "evil-app", Redirect, ch, "S256").Err);
        // Exact match only: a prefix extension of a registered URI is NOT registered.
        Assert.Equal("invalid_client_or_redirect", WorldOAuth.IssueCode(db, wid, Client, Redirect + "/../evil", ch, "S256").Err);
        Assert.Equal("invalid_client_or_redirect", WorldOAuth.IssueCode(db, wid, Client, "https://evil.example/world/account", ch, "S256").Err);
    }

    [Fact]
    public void A_wrong_verifier_burns_the_code_and_every_failure_answers_the_same_error()
    {
        var db = NewWorldDb();
        var wid = NewWorldUser(db, "oauth-burn@x.test");
        var (_, code) = WorldOAuth.IssueCode(db, wid, Client, Redirect, WorldOAuth.S256(Verifier), "S256");

        // Wrong verifier: uniform invalid_grant…
        Assert.Equal("invalid_grant", WorldOAuth.ExchangeCode(db, code, "wrong-" + Verifier, Client, Redirect).Err);
        // …and the code is CONSUMED — the right verifier gets no second chance on the same code.
        Assert.Equal("invalid_grant", WorldOAuth.ExchangeCode(db, code, Verifier, Client, Redirect).Err);

        // Client and redirect mismatches at exchange are equally quiet.
        var (_, code2) = WorldOAuth.IssueCode(db, wid, Client, Redirect, WorldOAuth.S256(Verifier), "S256");
        Assert.Equal("invalid_grant", WorldOAuth.ExchangeCode(db, code2, Verifier, "evil-app", Redirect).Err);
        var (_, code3) = WorldOAuth.IssueCode(db, wid, Client, Redirect, WorldOAuth.S256(Verifier), "S256");
        Assert.Equal("invalid_grant", WorldOAuth.ExchangeCode(db, code3, Verifier, Client, "/world/other").Err);
        // A code that never existed answers identically.
        Assert.Equal("invalid_grant", WorldOAuth.ExchangeCode(db, "WOAnope", Verifier, Client, Redirect).Err);
    }

    [Fact]
    public void Replaying_a_redeemed_code_revokes_the_session_it_minted()
    {
        var db = NewWorldDb();
        var wid = NewWorldUser(db, "oauth-replay@x.test");
        var (_, code) = WorldOAuth.IssueCode(db, wid, Client, Redirect, WorldOAuth.S256(Verifier), "S256");
        var (_, _, token) = WorldOAuth.ExchangeCode(db, code, Verifier, Client, Redirect);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_sessions WHERE token=?", Security.Sha(token)));

        // The replay fails AND the first presentation's session dies with it — a thief who raced
        // the legitimate client keeps nothing.
        Assert.Equal("invalid_grant", WorldOAuth.ExchangeCode(db, code, Verifier, Client, Redirect).Err);
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_sessions WHERE token=?", Security.Sha(token)));
    }

    [Fact]
    public void The_registry_admin_validates_hard_and_protects_the_seeded_client()
    {
        var db = NewWorldDb();

        // Redirect validation: every hole an attacker would try is refused.
        foreach (var bad in new[] {
            "http://pciworld.org/cb",                 // https only
            "javascript:alert(1)",                    // scheme smuggling
            "https://pciworld.org/cb#frag",           // fragments
            "https://pciworld.org/*",                 // wildcards
            "//evil.example/cb",                      // protocol-relative
            "/cb one",                                // whitespace
            "/a,/b,/c,/d,/e,/f",                      // more than five
            "",                                       // nothing
            "https://pciworld.org/" + new string('a', 220) })  // overlong
            Assert.NotNull(WorldOAuth.ValidateRedirects(bad, out _));
        Assert.Null(WorldOAuth.ValidateRedirects("/world/account, https://pciworld.org/auth/callback", out var norm));
        Assert.Equal("/world/account,https://pciworld.org/auth/callback", norm);

        // Create + immediate effect on the authorization path.
        Assert.Null(WorldOAuth.UpsertClient(db, "pciworld-site", "PCI World site", "https://pciworld.org/cb", create: true));
        Assert.True(WorldOAuth.RedirectAllowed(db, "pciworld-site", "https://pciworld.org/cb"));
        Assert.Equal("already_exists", WorldOAuth.UpsertClient(db, "pciworld-site", "x", "/cb", create: true));
        Assert.Equal("bad_client_id", WorldOAuth.UpsertClient(db, "Bad Id!", "x", "/cb", create: true));

        // Update swaps the allow-list atomically — the old URI stops matching at once.
        Assert.Null(WorldOAuth.UpsertClient(db, "pciworld-site", "PCI World site", "https://pciworld.org/cb2", create: false));
        Assert.False(WorldOAuth.RedirectAllowed(db, "pciworld-site", "https://pciworld.org/cb"));
        Assert.True(WorldOAuth.RedirectAllowed(db, "pciworld-site", "https://pciworld.org/cb2"));

        // Deletion kills the client and its outstanding codes; the seeded client is untouchable.
        var wid = NewWorldUser(db, "oauth-admin@x.test");
        WorldOAuth.IssueCode(db, wid, "pciworld-site", "https://pciworld.org/cb2", WorldOAuth.S256(Verifier), "S256");
        Assert.Null(WorldOAuth.DeleteClient(db, "pciworld-site"));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_oauth_codes WHERE client_id='pciworld-site'"));
        Assert.Equal("load_bearing", WorldOAuth.DeleteClient(db, "pciworld-app"));
        Assert.Equal("not_found", WorldOAuth.DeleteClient(db, "never-was"));
        Assert.True(WorldOAuth.RedirectAllowed(db, "pciworld-app", "/world/account"));
    }

    [Fact]
    public void Expired_codes_fail_and_the_sweep_keeps_consumed_rows_for_replay_detection()
    {
        var db = NewWorldDb();
        var wid = NewWorldUser(db, "oauth-exp@x.test");
        var (_, code) = WorldOAuth.IssueCode(db, wid, Client, Redirect, WorldOAuth.S256(Verifier), "S256");
        db.Execute("UPDATE pciworld_oauth_codes SET expires_at=datetime('now','-1 minutes') WHERE code_sha=?", Security.Sha(code));
        Assert.Equal("invalid_grant", WorldOAuth.ExchangeCode(db, code, Verifier, Client, Redirect).Err);

        // A freshly consumed code SURVIVES the sweep (replay detection needs it)…
        var (_, live) = WorldOAuth.IssueCode(db, wid, Client, Redirect, WorldOAuth.S256(Verifier), "S256");
        WorldOAuth.ExchangeCode(db, live, Verifier, Client, Redirect);
        WorldRetentionService.Sweep(db);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_oauth_codes WHERE code_sha=?", Security.Sha(live)));
        // …while rows a day past expiry are residue and go.
        db.Execute("UPDATE pciworld_oauth_codes SET expires_at=datetime('now','-2 days') WHERE code_sha=?", Security.Sha(code));
        WorldRetentionService.Sweep(db);
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_oauth_codes WHERE code_sha=?", Security.Sha(code)));
    }
}
