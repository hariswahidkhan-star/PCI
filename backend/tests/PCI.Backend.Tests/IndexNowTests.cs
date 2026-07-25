using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

public class IndexNowTests : IClassFixture<DbFixture>
{
    readonly Db _db;
    public IndexNowTests(DbFixture f) => _db = f.Db;

    [Fact]
    public void EnsureKey_GeneratesOnceThenStaysStable()
    {
        IndexNowService.EnsureKey(_db);   // Migrate already ran it; must be a no-op now
        var key = IndexNowService.Key(_db);
        Assert.Equal(32, key.Length);
        Assert.All(key, c => Assert.True(Uri.IsHexDigit(c) && !char.IsUpper(c)));
        IndexNowService.EnsureKey(_db);
        Assert.Equal(key, IndexNowService.Key(_db));
    }

    [Fact]
    public void Submit_FailsFastWithoutUrlsOrKey_NeverTouchingTheNetwork()
    {
        IndexNowService.EnsureKey(_db);
        var (ok, detail) = IndexNowService.Submit(_db, Array.Empty<string>());
        Assert.False(ok);
        Assert.Equal("no URLs to submit", detail);

        _db.Execute("DELETE FROM site_settings WHERE skey='indexnow_key'");
        (ok, detail) = IndexNowService.Submit(_db, new[] { "https://projectcontrolsinstitute.org/" });
        Assert.False(ok);
        Assert.Equal("no IndexNow key configured", detail);

        // both early returns happen before the submission ledger is written
        Assert.Equal(0L, _db.Scalar<long>("SELECT COUNT(*) FROM seo_submissions WHERE engine='indexnow'"));
        IndexNowService.EnsureKey(_db);   // restore for the other tests in this fixture
    }

    [Fact]
    public void AllPublicUrls_AppliesSitemapInclusionRules()
    {
        _db.Execute("INSERT OR IGNORE INTO pages(slug,title) VALUES('index.html','Home')");
        _db.Execute("INSERT OR IGNORE INTO pages(slug,title) VALUES('student.html','Shell')");
        _db.Execute("INSERT INTO pages(slug,title,published,noindex) VALUES('ut-visible.html','Visible',1,0)");
        _db.Execute("INSERT INTO pages(slug,title,published,noindex) VALUES('ut-hidden.html','Hidden',1,1)");
        _db.Execute("INSERT INTO pages(slug,title,published,noindex) VALUES('ut-draft.html','Draft',0,0)");
        _db.Execute("UPDATE pages SET published=1, noindex=0 WHERE slug IN ('index.html','student.html')");

        const string Base = "https://projectcontrolsinstitute.org";
        var urls = IndexNowService.AllPublicUrls(_db);
        Assert.Contains(Base + "/ut-visible.html", urls);
        Assert.Contains(Base + "/", urls);                       // index.html → site root
        Assert.DoesNotContain(Base + "/ut-hidden.html", urls);   // noindex excluded
        Assert.DoesNotContain(Base + "/ut-draft.html", urls);    // unpublished excluded
        Assert.DoesNotContain(Base + "/student.html", urls);     // app shells excluded
        Assert.All(urls, u => Assert.StartsWith(Base + "/", u));
    }
}
