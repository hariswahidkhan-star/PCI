using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — discoverability (EXPANSION_PHASE0.md §6).
///
/// The audit's finding was blunt: not one /world URL appeared in any sitemap, and a world-only
/// deployment served the Institute's robots.txt, advertising sitemaps on a different domain. These
/// tests assert the fix and, just as importantly, assert what must NOT be listed — token-addressed
/// pages belong to the people who minted them and can be revoked, and no index can be un-published
/// on request.
/// </summary>
public class WorldSeoTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    [Fact]
    public void The_world_sitemap_lists_every_servable_challenge_and_published_article()
    {
        var db = NewWorldDb();
        var xml = WorldSeo.Sitemap(db);

        Assert.StartsWith("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", xml);
        Assert.Contains("<urlset", xml);
        Assert.Contains("/world</loc>", xml);
        Assert.Contains("/world/archive</loc>", xml);
        Assert.Contains("/world/about</loc>", xml);
        Assert.Contains("/world/blog</loc>", xml);

        foreach (var r in db.Query("SELECT code FROM pciworld_challenges WHERE current_version>=1 AND retired=0"))
            Assert.Contains($"/world/challenge/{H.Str(r["code"])}</loc>", xml);
        foreach (var r in db.Query("SELECT slug FROM pciworld_articles WHERE status='published'"))
            Assert.Contains($"/world/blog/{H.Str(r["slug"])}</loc>", xml);

        // Absolute URLs — a sitemap of relative paths is not a sitemap.
        Assert.DoesNotContain("<loc>/world", xml);
    }

    [Fact]
    public void Retired_and_unpublished_content_leaves_the_sitemap()
    {
        var db = NewWorldDb();
        var code = H.Str(db.QueryOne("SELECT code FROM pciworld_challenges WHERE current_version>=1 LIMIT 1")!["code"])!;
        Assert.Contains($"/world/challenge/{code}</loc>", WorldSeo.Sitemap(db));

        db.Execute("UPDATE pciworld_challenges SET retired=1, updated_at=datetime('now','+1 second') WHERE code=?", code);
        Assert.DoesNotContain($"/world/challenge/{code}</loc>", WorldSeo.Sitemap(db));
    }

    [Fact]
    public void Token_addressed_pages_are_never_listed_or_allowed()
    {
        var db = NewWorldDb();
        var xml = WorldSeo.Sitemap(db);
        // Shared results, Passports and invitations are revocable. They must not be in a sitemap,
        // and robots must discourage crawling them even though each also carries a noindex tag.
        Assert.DoesNotContain("/world/r/", xml);
        Assert.DoesNotContain("/world/p/", xml);
        Assert.DoesNotContain("/world/i/", xml);
        Assert.DoesNotContain("/world/account", xml);
        Assert.DoesNotContain("/world-admin", xml);

        var robots = WorldSeo.Robots();
        Assert.Contains("Disallow: /world-admin", robots);
        Assert.Contains("Disallow: /world/r/", robots);
        Assert.Contains("Disallow: /world/p/", robots);
        Assert.Contains("Disallow: /world/account", robots);
        Assert.Contains("Allow: /", robots);
    }

    [Fact]
    public void World_robots_advertises_a_sitemap_on_its_own_host()
    {
        var robots = WorldSeo.Robots();
        var host = WorldUrl.Base();
        Assert.Contains($"Sitemap: {host}/world-sitemap.xml", robots);
        // Exactly one sitemap line, on this host — the audit's finding was a robots file naming
        // four sitemaps that all lived on a different domain.
        Assert.Equal(1, robots.Split('\n').Count(l => l.StartsWith("Sitemap:", StringComparison.Ordinal)));
    }

    [Fact]
    public void A_world_only_host_serves_its_own_sitemap_path_rather_than_redirecting()
    {
        // The allowlist decides what a PCIWORLD_ONLY deployment answers at all. Without these,
        // /sitemap.xml 302'd to /world and the host published no sitemap whatsoever.
        Assert.True(WorldOnly.Allowed("/world-sitemap.xml"));
        Assert.True(WorldOnly.Allowed("/sitemap.xml"));
        Assert.True(WorldOnly.Allowed("/robots.txt"));
        Assert.False(WorldOnly.Allowed("/blog-sitemap.xml"));   // the Institute's, not this host's
    }

    [Fact]
    public void The_platform_sitemap_index_references_the_world_sitemap()
    {
        var db = NewWorldDb();
        Assert.Contains("/world-sitemap.xml", Sitemap.Index(db));
        Assert.Contains("/world-sitemap.xml", AiVisibility.Robots(db));
    }

    [Fact]
    public void Indexable_pages_declare_index_and_token_pages_declare_noindex()
    {
        var db = NewWorldDb();
        var about = WorldPages.About(db);
        Assert.Contains("content=\"index, follow, max-image-preview:large", about);
        Assert.Contains("property=\"og:url\"", about);
        Assert.Contains("name=\"twitter:card\"", about);

        Assert.Contains("content=\"noindex, nofollow\"", WorldPages.NotFound(db));
    }

    [Fact]
    public void Structured_data_claims_only_what_the_visible_page_supports()
    {
        var db = NewWorldDb();
        var home = WorldSeo.HomeJsonLd(db);
        Assert.Contains("\"@type\":\"Organization\"", home);
        Assert.Contains("\"@type\":\"WebSite\"", home);
        Assert.Contains("Project Controls Institute", home);
        // No invented credibility signals: PCI World publishes no ratings, counts or awards, so its
        // structured data must not either.
        Assert.DoesNotContain("aggregateRating", home);
        Assert.DoesNotContain("review", home, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("award", home, StringComparison.OrdinalIgnoreCase);

        var version = db.QueryOne("SELECT * FROM pciworld_challenge_versions LIMIT 1")!;
        var ld = WorldSeo.ChallengeJsonLd(db, "WC-EVM-001", version);
        Assert.Contains("\"@type\":\"LearningResource\"", ld);
        Assert.Contains("\"isAccessibleForFree\":true", ld);
        // A practice exercise is never described as an assessment or a credential.
        Assert.DoesNotContain("EducationalOccupationalCredential", ld);
        Assert.DoesNotContain("\"@type\":\"Quiz\"", ld);
    }

    [Fact]
    public void The_sitemap_is_cached_but_invalidates_when_content_changes()
    {
        var db = NewWorldDb();
        var first = WorldSeo.Sitemap(db);
        Assert.Same(first, WorldSeo.Sitemap(db));   // identical instance: the cache is live

        db.Execute(@"INSERT INTO pciworld_articles(slug,kind,title,dek,body_md,author_name,status,current_version,published_at,updated_at)
            VALUES('a-new-piece','blog','T','D','B','PCI World Editorial','published',1,datetime('now'),datetime('now','+1 second'))");
        var second = WorldSeo.Sitemap(db);
        Assert.NotSame(first, second);
        Assert.Contains("/world/blog/a-new-piece</loc>", second);
    }
}
