using Microsoft.AspNetCore.Http;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

public class PathRedirectsNormTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData("/", "/")]                      // root keeps its slash
    [InlineData("about.html", "/about.html")]   // leading slash added
    [InlineData("/about/", "/about")]           // trailing slash dropped
    [InlineData("/a/b//", "/a/b")]
    [InlineData("/page?utm=x&y=1", "/page")]    // query stripped
    [InlineData("  /page/  ", "/page")]
    public void Norm_CanonicalisesPaths(string? input, string expected) =>
        Assert.Equal(expected, PathRedirects.Norm(input));
}

/// <summary>DB-backed redirect lookup — the cache/version handshake and the loop guards.</summary>
public class PathRedirectsTargetTests : IClassFixture<DbFixture>
{
    readonly Db _db;
    public PathRedirectsTargetTests(DbFixture f) => _db = f.Db;

    static HttpRequest Req(string method, string path)
    {
        var ctx = new DefaultHttpContext();
        ctx.Request.Method = method;
        ctx.Request.Path = path;
        return ctx.Request;
    }

    void Seed(string from, string to, int status, int active = 1)
    {
        _db.Execute("DELETE FROM seo_redirects WHERE from_path=?", from);
        _db.Execute("INSERT INTO seo_redirects(from_path,to_url,status,active) VALUES(?,?,?,?)", from, to, status, active);
        PathRedirects.Bump();
    }

    [Fact]
    public void Target_MatchesNormalisedPathOnGetOnly()
    {
        Seed("/old-page", "/new-page", 301);
        var hit = PathRedirects.Target(_db, Req("GET", "/old-page"));
        Assert.NotNull(hit);
        Assert.Equal(("/new-page", 301), hit!.Value);
        Assert.NotNull(PathRedirects.Target(_db, Req("HEAD", "/old-page/")));   // trailing slash still matches
        Assert.Null(PathRedirects.Target(_db, Req("POST", "/old-page")));       // never a POST/webhook
        Assert.Null(PathRedirects.Target(_db, Req("GET", "/no-redirect-here")));
    }

    [Fact]
    public void Target_IgnoresInactiveRowsAndSelfRedirects()
    {
        Seed("/retired", "/somewhere", 301, active: 0);
        Assert.Null(PathRedirects.Target(_db, Req("GET", "/retired")));
        Seed("/loopy", "/loopy/", 301);          // normalises to itself → final loop guard
        Assert.Null(PathRedirects.Target(_db, Req("GET", "/loopy")));
    }

    [Fact]
    public void Target_410GoneNeedsNoTargetAndBadStatusFallsBackTo301()
    {
        Seed("/gone-page", "", 410);
        var gone = PathRedirects.Target(_db, Req("GET", "/gone-page"));
        Assert.Equal(410, gone!.Value.status);
        Seed("/weird-status", "/dest", 307);     // not in {301,302,308,410} → coerced to 301
        Assert.Equal(("/dest", 301), PathRedirects.Target(_db, Req("GET", "/weird-status"))!.Value);
    }
}

public class RedirectsTests
{
    [Theory]
    [InlineData("/admin")]
    [InlineData("/admin/settings")]
    [InlineData("/ADMIN")]           // case-insensitive
    [InlineData("/app")]
    [InlineData("/app/portal")]
    [InlineData("/api/health")]
    [InlineData("/api/")]
    [InlineData("/student.html")]
    [InlineData("/admin.html")]
    [InlineData("/exam-ui.html")]
    [InlineData("/exam-ui.html/")]   // trailing slash trimmed before compare
    public void IsPrivatePath_TaggedNoindex(string path) =>
        Assert.True(Redirects.IsPrivatePath(path));

    [Theory]
    [InlineData("/")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("/index.html")]
    [InlineData("/about.html")]
    [InlineData("/student")]         // only the exact shell filename is private
    [InlineData("/contact.html")]
    public void IsPrivatePath_PublicPagesPassThrough(string? path) =>
        Assert.False(Redirects.IsPrivatePath(path!));

    [Fact]
    public void CanonicalBase_DefaultsToApexHttps()
    {
        // TestEnv clears CANONICAL_HOST, so the compiled-in canonical applies
        Assert.Equal("projectcontrolsinstitute.org", Redirects.CanonicalHost);
        Assert.Equal("https://projectcontrolsinstitute.org", Redirects.CanonicalBase);
    }
}
