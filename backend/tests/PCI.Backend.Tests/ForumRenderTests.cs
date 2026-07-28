using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Tests;

/// <summary>
/// The public, crawlable forum read path (Core/ForumRender.cs; §18.3, D-009).
///
/// The first test is the one that matters, and it matters MORE here than anywhere else in this
/// increment: a leak in a transient chat transcript ends when the message is removed, but this
/// output is cached by search engines, so a post that escapes into it can outlive its deletion by
/// months in a cache nobody at PCI controls. The test therefore builds a thread containing one post
/// of every state and asserts that exactly one of them appears.
///
/// The second group is about structured data telling the truth. Claiming an accepted answer that
/// does not exist — or one that has since been withdrawn — is a lie to a machine that will repeat
/// it to people.
/// </summary>
[Collection(DbCollection.Name)]
public class ForumRenderTests
{
    static readonly IReadOnlyList<Rule> Policy = DefaultPostMatrix();
    static ITextModerator Mod => new CommunityModerators.DeterministicModerator();

    static Db NewDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        Settings.Put(db, "pciworld_forum_enabled", "1");
        return db;
    }

    static (long cat, long thread) Thread(Db db, string slug, string threadState = "open",
                                          string categoryState = "open")
    {
        var cat = db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_categories(slug,title,state) VALUES(?,?,?)",
            "c-" + slug, "Estimating", categoryState);
        var thread = db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_threads(category_id,slug,title,author_user_id,state) VALUES(?,?,?,1,?)",
            cat, slug, "How do you model SPI recovery?", threadState);
        return (cat, thread);
    }

    /// <summary>Insert a post directly in a chosen state — the render path must be correct for
    /// states the write path reaches by other routes (a moderator withdrawing, an author deleting).</summary>
    static long PostInState(Db db, long threadId, string state, string body, string kind = "reply")
    {
        var post = db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_posts(thread_id,author_user_id,state,kind,published_at) VALUES(?,1,?,?,datetime('now'))",
            threadId, state, kind);
        var rev = db.ExecuteReturningId(
            @"INSERT INTO pciworld_forum_post_revisions(post_id,revision_no,body,body_rendered,edited_by_user_id)
              VALUES(?,1,?,?,1)", post, body, "<p>" + body + "</p>");
        db.Execute("UPDATE pciworld_forum_posts SET current_revision_id=? WHERE id=?", rev, post);
        return post;
    }

    // ── the rule that must not break ──────────────────────────────────────────────────────────

    [Fact]
    public void OnlyPublishedPostsReachTheCrawlablePage()
    {
        // One post of every state, one page, one survivor. This output is cached by search engines,
        // so a leak here can outlive the deletion that was supposed to fix it.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-states");
        PostInState(db, thread, "published", "PUBLISHED-TEXT", "opening");
        PostInState(db, thread, "pending", "PENDING-TEXT");
        PostInState(db, thread, "withheld", "WITHHELD-TEXT");
        PostInState(db, thread, "withdrawn", "WITHDRAWN-TEXT");
        PostInState(db, thread, "deleted", "DELETED-TEXT");

        var html = ForumRender.Thread(db, "c-t-states", "t-states");
        Assert.NotNull(html);
        Assert.Contains("PUBLISHED-TEXT", html);
        foreach (var leaked in new[] { "PENDING-TEXT", "WITHHELD-TEXT", "WITHDRAWN-TEXT", "DELETED-TEXT" })
            Assert.DoesNotContain(leaked, html);
    }

    [Fact]
    public void AThreadWithNothingPublishableIsNotAPage()
    {
        // A bare title with no readable content is not worth indexing, and rendering one would
        // advertise that a discussion exists while showing none of it.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-empty");
        PostInState(db, thread, "pending", "not yet", "opening");
        Assert.Null(ForumRender.Thread(db, "c-t-empty", "t-empty"));
    }

    [Fact]
    public void AHiddenThreadOrCategoryDoesNotRender()
    {
        var db = NewDb();
        var (_, hidden) = Thread(db, "t-hidden", threadState: "hidden");
        PostInState(db, hidden, "published", "x", "opening");
        Assert.Null(ForumRender.Thread(db, "c-t-hidden", "t-hidden"));

        var (_, inHiddenCat) = Thread(db, "t-hidcat", categoryState: "hidden");
        PostInState(db, inHiddenCat, "published", "x", "opening");
        Assert.Null(ForumRender.Thread(db, "c-t-hidcat", "t-hidcat"));
    }

    [Fact]
    public void ALockedThreadStillReads()
    {
        // Locking stops new replies; it does not unpublish a discussion people may still need.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-locked", threadState: "locked");
        PostInState(db, thread, "published", "SETTLED-ANSWER", "opening");

        var html = ForumRender.Thread(db, "c-t-locked", "t-locked");
        Assert.NotNull(html);
        Assert.Contains("SETTLED-ANSWER", html);
        // And the state is conveyed in words rather than by a padlock glyph alone.
        Assert.Contains("closed to new replies", html);
    }

    [Fact]
    public void NothingRendersWhileTheForumIsDisabled()
    {
        var db = NewDb();
        var (_, thread) = Thread(db, "t-off");
        PostInState(db, thread, "published", "x", "opening");
        Settings.Put(db, "pciworld_forum_enabled", "0");
        Assert.Null(ForumRender.Thread(db, "c-t-off", "t-off"));
    }

    // ── structured data must not overclaim ────────────────────────────────────────────────────

    [Fact]
    public void AThreadWithoutAnAcceptedAnswerIsNotAdvertisedAsAQAPage()
    {
        var db = NewDb();
        var (_, thread) = Thread(db, "t-noqa");
        PostInState(db, thread, "published", "a question", "opening");
        PostInState(db, thread, "published", "a suggestion");

        var html = ForumRender.Thread(db, "c-t-noqa", "t-noqa")!;
        Assert.Contains("DiscussionForumPosting", html);
        Assert.DoesNotContain("acceptedAnswer", html);
    }

    [Fact]
    public void AnAcceptedAnswerIsAdvertisedOnlyWhenItIsStillPublished()
    {
        // A withdrawn accepted answer must stop being the accepted answer. Otherwise the page keeps
        // telling a search engine there is a settled answer that nobody can read.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-qa");
        PostInState(db, thread, "published", "a question", "opening");
        var answer = PostInState(db, thread, "published", "THE ANSWER");
        db.Execute("UPDATE pciworld_forum_threads SET solved_post_id=? WHERE id=?", answer, thread);

        var html = ForumRender.Thread(db, "c-t-qa", "t-qa")!;
        Assert.Contains("QAPage", html);
        Assert.Contains("acceptedAnswer", html);
        Assert.Contains("Accepted answer", html);

        db.Execute("UPDATE pciworld_forum_posts SET state='withdrawn' WHERE id=?", answer);
        var after = ForumRender.Thread(db, "c-t-qa", "t-qa")!;
        Assert.DoesNotContain("acceptedAnswer", after);
        Assert.DoesNotContain("THE ANSWER", after);
        Assert.Contains("DiscussionForumPosting", after);
    }

    [Fact]
    public void TheReplyCountCountsOnlyWhatRendered()
    {
        // Counting withheld replies would advertise a livelier discussion than exists.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-count");
        PostInState(db, thread, "published", "q", "opening");
        PostInState(db, thread, "published", "r1");
        PostInState(db, thread, "withheld", "r2");

        var html = ForumRender.Thread(db, "c-t-count", "t-count")!;
        Assert.Contains("\"commentCount\":1", html);
    }

    [Fact]
    public void StructuredDataUsesTheSchemaOrgKeys()
    {
        // Anonymous types cannot carry '@', so the keys are renamed on the way out — if that ever
        // stopped happening, every page would emit structured data no crawler recognises.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-keys");
        PostInState(db, thread, "published", "q", "opening");

        var html = ForumRender.Thread(db, "c-t-keys", "t-keys")!;
        Assert.Contains("\"@context\":\"https://schema.org\"", html);
        Assert.Contains("\"@type\":", html);
        Assert.DoesNotContain("\"type\":\"BreadcrumbList\"", html);
    }

    [Fact]
    public void BreadcrumbsAreEmittedForBothTheReaderAndTheCrawler()
    {
        var db = NewDb();
        var (_, thread) = Thread(db, "t-crumbs");
        PostInState(db, thread, "published", "q", "opening");

        var html = ForumRender.Thread(db, "c-t-crumbs", "t-crumbs")!;
        Assert.Contains("aria-label=\"Breadcrumb\"", html);
        Assert.Contains("BreadcrumbList", html);
    }

    // ── the write path feeds it ───────────────────────────────────────────────────────────────

    [Fact]
    public void APostFromANewAccountIsNotCrawlableUntilItIsReleased()
    {
        // End to end through the real write path: trust decides timing, and the crawlable page is
        // the surface where that timing is most consequential.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-e2e");
        PostInState(db, thread, "published", "OPENING", "opening");

        var newbie = ForumPosts.Create(db, thread, 42, "A careful reply about float.",
                                       ForumTrust.Level.New, Mod, Policy);
        Assert.Equal("pending", newbie.State);
        Assert.DoesNotContain("careful reply", ForumRender.Thread(db, "c-t-e2e", "t-e2e")!);

        db.Execute("UPDATE pciworld_forum_posts SET state='published' WHERE id=?", newbie.PostId);
        Assert.Contains("careful reply", ForumRender.Thread(db, "c-t-e2e", "t-e2e")!);
    }

    [Fact]
    public void AnEditedPostIsDisclosedAsEdited()
    {
        // A silently edited post is how quote-mining works.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-edited");
        var post = ForumPosts.Create(db, thread, 1, "Original point about float.",
                                     ForumTrust.Level.Member, Mod, Policy, kind: "opening");
        ForumPosts.Edit(db, post.PostId, 1, "Revised point about float.", ForumTrust.Level.Member, Mod, Policy);

        var html = ForumRender.Thread(db, "c-t-edited", "t-edited")!;
        Assert.Contains("Edited", html);
        Assert.Contains("Revised point", html);
        // The superseded revision stays in the database for moderation, and stays OFF the page.
        Assert.DoesNotContain("Original point", html);
    }

    // ── it must be a real document, not a fragment ────────────────────────────────────────────

    [Fact]
    public void ThePageIsACompleteDocumentWithATitleAndALanguage()
    {
        // The first version of the renderer returned a bare fragment. A page with no <title>, no
        // lang and no canonical is not "crawlable" in any sense that matters — a crawler indexes it
        // under whatever it can guess, a screen reader announces an untitled document, and axe
        // flags html-has-lang and document-title on sight.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-doc");
        PostInState(db, thread, "published", "The float on activity B is eight days.", "opening");

        var html = ForumRender.Thread(db, "c-t-doc", "t-doc", "https://world.example")!;
        Assert.StartsWith("<!DOCTYPE html>", html);
        Assert.Contains("<html lang=\"en\">", html);
        Assert.Contains("<title>How do you model SPI recovery? — PCI World forum</title>", html);
        Assert.Contains("<link rel=\"canonical\" href=\"https://world.example/world/forum/c-t-doc/t-doc\"/>", html);
        Assert.Contains("<main id=\"wf-main\">", html);
    }

    [Fact]
    public void TheMetaDescriptionComesFromTheOpeningPost()
    {
        // The words that actually answer "what is this thread about", rather than a generic site
        // line repeated on every page — which is what search engines discard.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-desc");
        PostInState(db, thread, "published", "Rebaselining after a six-week slip on the piling works.", "opening");

        var html = ForumRender.Thread(db, "c-t-desc", "t-desc")!;
        Assert.Contains("Rebaselining after a six-week slip", html);
        Assert.Contains("<meta name=\"description\"", html);
    }

    [Fact]
    public void AKeyboardUserCanSkipTheBreadcrumb()
    {
        // A thread page is mostly a long list; traversing the breadcrumb on every one is the kind
        // of small tax that makes a site unusable by keyboard rather than merely annoying.
        var db = NewDb();
        var (_, thread) = Thread(db, "t-skip");
        PostInState(db, thread, "published", "x", "opening");

        var html = ForumRender.Thread(db, "c-t-skip", "t-skip")!;
        Assert.Contains("href=\"#wf-main\"", html);
        Assert.Contains("id=\"wf-main\"", html);
    }

    [Fact]
    public void TheTitleIsEscapedRatherThanTrusted()
    {
        var db = NewDb();
        var cat = db.ExecuteReturningId("INSERT INTO pciworld_forum_categories(slug,title) VALUES('c-esc','Cat')");
        var thread = db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_threads(category_id,slug,title,author_user_id) VALUES(?,'t-esc',?,1)",
            cat, "<script>alert(1)</script>");
        PostInState(db, thread, "published", "body", "opening");

        var html = ForumRender.Thread(db, "c-esc", "t-esc")!;
        Assert.DoesNotContain("<script>alert(1)</script>", html);
        Assert.Contains("&lt;script&gt;", html);
    }
}
