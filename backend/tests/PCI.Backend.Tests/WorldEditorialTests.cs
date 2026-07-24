using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — the editorial platform (EXPANSION_GOVERNANCE §5–§8).
///
/// The governance makes four promises about published writing: nothing is edited silently, nobody
/// approves their own work, a news claim can always be traced to a source, and authorship is never
/// invented. Each is tested here as a property of the code rather than of the style guide, because
/// a rule that lives only in a document is a rule that survives exactly as long as the person who
/// remembers it.
/// </summary>
public class WorldEditorialTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    const string GoodBody = """
        ## A heading

        A paragraph that exists purely so this article clears the minimum length the SEO policy
        requires, because thin pages are prohibited rather than merely discouraged.
        """ + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word"
            + " word word word word word word word word word word word word word word word word word word word word";

    static long Draft(Db db, string slug, string kind = "blog", long? authorId = 1) =>
        db.ExecuteReturningId(@"INSERT INTO pciworld_articles(slug,kind,title,dek,body_md,author_name,author_id,status)
            VALUES(?,?,?,?,?,?,?, 'drafting')",
            slug, kind, "A title", "A standfirst that summarises the piece.", GoodBody,
            WorldEditorial.EditorialByline, authorId);

    // ───────────────────────── the seeded batch ─────────────────────────

    [Fact]
    public void The_founding_batch_is_published_valid_and_honestly_attributed()
    {
        var db = NewWorldDb();
        var rows = db.Query("SELECT * FROM pciworld_articles WHERE author_id IS NULL");
        Assert.Equal(WorldArticlePack.Count, rows.Count);
        Assert.Equal(10, rows.Count);

        foreach (var r in rows)
        {
            Assert.Equal("published", H.Str(r["status"]));
            Assert.Equal("blog", H.Str(r["kind"]));
            Assert.True(H.L(r["current_version"]) >= 1);
            Assert.Empty(WorldEditorial.Validate(WorldEditorial.InputFor(db, r)));

            // Authorship is the transparent editorial byline — never an invented person.
            Assert.Equal(WorldEditorial.EditorialByline, H.Str(r["author_name"]));

            // Blog articles in this batch make no sourced claims, so they carry no sources. That is
            // the point of the blog/news split: an unsourced NEWS item could not have been published.
            Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_article_sources WHERE article_id=?", H.L(r["id"])));

            // The immutable snapshot exists and matches the working copy at seed time.
            var v = WorldEditorial.LiveVersion(db, H.L(r["id"]));
            Assert.NotNull(v);
            Assert.Equal(H.Str(r["body_md"]), H.Str(v!["body_md"]));
        }

        // Re-seeding is idempotent: no new versions, no duplicated rows.
        var versionsBefore = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_article_versions");
        WorldArticlePack.Seed(db);
        Assert.Equal(10, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_articles"));
        Assert.Equal(versionsBefore, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_article_versions"));
    }

    // ───────────────────────── the publication gate ─────────────────────────

    [Fact]
    public void The_gate_rejects_thin_pages_missing_authors_and_bad_addresses()
    {
        var ok = new WorldEditorial.ArticleInput("a-good-slug", "blog", "Title", "Standfirst", GoodBody,
            WorldEditorial.EditorialByline, null, null, 0, false, false);
        Assert.Empty(WorldEditorial.Validate(ok));

        Assert.Contains(WorldEditorial.Validate(ok with { Body = "Too short." }), i => i.Code == "body");
        Assert.Contains(WorldEditorial.Validate(ok with { AuthorName = "" }), i => i.Code == "author");
        Assert.Contains(WorldEditorial.Validate(ok with { Title = "" }), i => i.Code == "title");
        Assert.Contains(WorldEditorial.Validate(ok with { Dek = "" }), i => i.Code == "dek");
        Assert.Contains(WorldEditorial.Validate(ok with { Slug = "Not A Slug" }), i => i.Code == "slug");
        Assert.Contains(WorldEditorial.Validate(ok with { Kind = "podcast" }), i => i.Code == "kind");
        Assert.Contains(WorldEditorial.Validate(ok with { SeoDesc = new string('x', 200) }), i => i.Code == "seo_desc");
    }

    [Fact]
    public void A_news_item_cannot_be_approved_without_a_recorded_source()
    {
        var news = new WorldEditorial.ArticleInput("a-news-item", "news", "Title", "Standfirst", GoodBody,
            "A Real Reporter", null, null, SourceCount: 0, MentionsEntity: false, LegalReviewed: false);
        Assert.Contains(WorldEditorial.Validate(news), i => i.Code == "sources");
        Assert.Empty(WorldEditorial.Validate(news with { SourceCount = 1 }));

        // The same article as a blog piece is fine — the obligation is on news, by design.
        Assert.Empty(WorldEditorial.Validate(news with { Kind = "blog" }));
    }

    [Fact]
    public void Naming_a_company_requires_a_recorded_legal_review()
    {
        var a = new WorldEditorial.ArticleInput("mentions-a-company", "blog", "Title", "Standfirst", GoodBody,
            WorldEditorial.EditorialByline, null, null, 1, MentionsEntity: true, LegalReviewed: false);
        Assert.Contains(WorldEditorial.Validate(a), i => i.Code == "legal");
        Assert.Empty(WorldEditorial.Validate(a with { LegalReviewed = true }));
    }

    [Fact]
    public void The_gate_is_enforced_end_to_end_through_the_database()
    {
        var db = NewWorldDb();
        var id = db.ExecuteReturningId(@"INSERT INTO pciworld_articles(slug,kind,title,dek,body_md,author_name,author_id,status)
            VALUES('news-without-sources','news','T','D',?, 'A Reporter', 1, 'drafting')", GoodBody);

        // No source recorded → approval refuses, whatever the workflow status says.
        Assert.Equal("not_publishable", WorldEditorial.Approve(db, id, approverId: 2));

        var src = db.ExecuteReturningId("INSERT INTO pciworld_sources(url,publisher) VALUES('https://example.test/a','Example')");
        db.Execute("INSERT INTO pciworld_article_sources(article_id,source_id,claim) VALUES(?,?,'The thing that was claimed.')", id, src);
        Assert.Null(WorldEditorial.Approve(db, id, approverId: 2));

        // An entity mention added AFTER approval blocks publication, because publish re-validates.
        var ent = db.ExecuteReturningId("INSERT INTO pciworld_entities(legal_name) VALUES('Example Holdings plc')");
        db.Execute("INSERT INTO pciworld_entity_mentions(article_id,entity_id) VALUES(?,?)", id, ent);
        Assert.Equal("not_publishable", WorldEditorial.Publish(db, id, publisherId: 3));

        Assert.Null(WorldEditorial.Review(db, id, "legal", reviewerId: 4, "pass", "Cleared."));
        Assert.Null(WorldEditorial.Publish(db, id, publisherId: 3));
        Assert.Equal("published", H.Str(db.QueryOne("SELECT status FROM pciworld_articles WHERE id=?", id)!["status"]));
    }

    // ───────────────────────── maker-checker ─────────────────────────

    [Fact]
    public void Nobody_approves_or_reviews_their_own_work()
    {
        var db = NewWorldDb();
        var id = Draft(db, "maker-checker", authorId: 7);

        Assert.Equal("maker_checker", WorldEditorial.Approve(db, id, approverId: 7));
        Assert.Equal("maker_checker", WorldEditorial.Review(db, id, "fact_check", reviewerId: 7, "pass", null));

        Assert.Null(WorldEditorial.Review(db, id, "fact_check", reviewerId: 8, "pass", "Checked."));
        Assert.Null(WorldEditorial.Approve(db, id, approverId: 8));
    }

    // ───────────────────────── corrections ─────────────────────────

    [Fact]
    public void Published_text_changes_only_through_a_dated_public_correction()
    {
        var db = NewWorldDb();
        var id = Draft(db, "correction-path", authorId: 1);
        Assert.Null(WorldEditorial.Approve(db, id, 2));
        Assert.Null(WorldEditorial.Publish(db, id, 3));
        var v1 = WorldEditorial.LiveVersion(db, id)!;

        // A correction with no explanation is refused: the note is the deliverable.
        Assert.Equal("note_required", WorldEditorial.Correct(db, id, 3, "oops", null, null, null));

        Assert.Null(WorldEditorial.Correct(db, id, 3, "Corrected the standfirst, which named the wrong index.",
            null, "A corrected standfirst.", null));

        var after = db.QueryOne("SELECT * FROM pciworld_articles WHERE id=?", id)!;
        Assert.Equal(2L, H.L(after["current_version"]));
        var corrections = WorldEditorial.ParseCorrections(H.Str(after["corrections_json"]));
        Assert.Single(corrections);
        Assert.Contains("wrong index", corrections[0].Note);
        Assert.Equal(DateTime.UtcNow.ToString("yyyy-MM-dd"), corrections[0].Date);

        // The version a reader was served earlier still exists, unchanged. That is the whole point.
        var stillThere = db.QueryOne("SELECT * FROM pciworld_article_versions WHERE article_id=? AND version=1", id)!;
        Assert.Equal(H.Str(v1["dek"]), H.Str(stillThere["dek"]));
        Assert.Equal("A corrected standfirst.", H.Str(WorldEditorial.LiveVersion(db, id)!["dek"]));
    }

    [Fact]
    public void A_published_article_cannot_be_edited_through_the_draft_path()
    {
        // CanEdit is what the endpoint gates on; the correction path is the only route afterwards.
        Assert.True(WorldEditorial.CanEdit("drafting"));
        Assert.True(WorldEditorial.CanEdit("technical_review"));
        Assert.False(WorldEditorial.CanEdit("approved"));
        Assert.False(WorldEditorial.CanEdit("published"));
    }

    // ───────────────────────── rendering safety ─────────────────────────

    [Fact]
    public void Authored_markup_cannot_inject_html_script_or_a_dangerous_link()
    {
        var hostile = """
            ## Heading with <script>alert(1)</script>

            A paragraph with <img src=x onerror=alert(1)> and a [bad link](javascript:alert(1)) in it.

            [protocol relative](//evil.example/x) and [data uri](data:text/html,<script>) too.

            - A list item with **bold** and `code`
            """;
        var html = WorldEditorial.RenderBody(hostile);

        // No markup is produced. The hostile strings survive only as visible ESCAPED text, which is
        // the correct outcome: the renderer neither executes them nor silently deletes the author's
        // words, so a reviewer can see exactly what was written.
        Assert.DoesNotContain("<script>", html);
        Assert.DoesNotContain("<img", html);
        Assert.DoesNotContain("href=\"javascript:", html);
        Assert.DoesNotContain("href=\"data:", html);
        Assert.DoesNotContain("href=\"//", html);
        Assert.DoesNotContain("<a href", html.Split("- A list item")[0]);   // none of the three became links
        Assert.Contains("&lt;script&gt;", html);
        Assert.Contains("&lt;img src=x onerror=alert(1)&gt;", html);
        // And the legitimate vocabulary still renders.
        Assert.Contains("<h2>", html);
        Assert.Contains("<li>", html);
        Assert.Contains("<strong>bold</strong>", html);
        Assert.Contains("<code>code</code>", html);
    }

    [Fact]
    public void External_links_are_marked_and_internal_links_are_not()
    {
        var html = WorldEditorial.RenderBody("See [the archive](/world/archive) and [a source](https://example.test/x).");
        Assert.Contains("<a href=\"/world/archive\">the archive</a>", html);
        Assert.Contains("rel=\"nofollow noopener\" target=\"_blank\"", html);
        Assert.Contains("https://example.test/x", html);
    }

    [Fact]
    public void Slugs_and_reading_time_behave()
    {
        Assert.Equal("why-your-spi-recovers", WorldEditorial.Slugify("Why your SPI recovers"));
        Assert.Equal("the-100-rule-is-not-bureaucracy", WorldEditorial.Slugify("The 100% rule is not bureaucracy!"));
        Assert.Equal("", WorldEditorial.Slugify(null));
        Assert.Equal(1, WorldEditorial.ReadingMinutes("a few words only"));
        Assert.True(WorldEditorial.ReadingMinutes(string.Join(" ", Enumerable.Repeat("word", 900))) >= 4);
    }

    // ───────────────────────── public serving ─────────────────────────

    [Fact]
    public void Readers_are_served_the_snapshot_and_see_corrections_and_structured_data()
    {
        var db = NewWorldDb();
        var article = db.QueryOne("SELECT * FROM pciworld_articles WHERE author_id IS NULL LIMIT 1")!;
        var version = WorldEditorial.LiveVersion(db, H.L(article["id"]))!;
        var page = WorldPages.ArticlePage(db, "blog", article, version, new());

        Assert.Contains("application/ld+json", page);
        Assert.Contains("\"@type\":\"BlogPosting\"", page);
        Assert.Contains("\"@type\":\"BreadcrumbList\"", page);
        Assert.Contains("aria-label=\"Breadcrumb\"", page);
        Assert.Contains(WorldEditorial.EditorialByline, page);
        Assert.Contains(WorldPages.PracticeNotice, page);
        // No fabricated author identity in the structured data — the byline is an Organization.
        Assert.Contains("\"@type\":\"Organization\"", page);

        // A news page declares itself as news.
        Assert.Contains("\"@type\":\"NewsArticle\"", WorldPages.ArticlePage(db, "news", article, version, new()));
    }
}
