using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The forum schema's structural guarantees (Data/ForumSchema.cs;
/// docs/pciworld/CCP_PHASE3_DESIGN.md §4–§6), asserted under whichever provider TEST_DB_PROVIDER
/// selects so the MySQL leg proves parity rather than assuming it.
///
/// These assert CONSTRAINTS rather than columns, because the constraints are the safety property.
/// Application code can be reviewed for races; a unique index cannot be raced. The one that carries
/// the most weight is the revision numbering: §28.11 forbids modifying published content in place,
/// and the whole value of that rule is that a moderator reviewing a report can say which text was
/// reported. Two concurrent edits that both land revision 4 would destroy exactly that.
/// </summary>
[Collection(DbCollection.Name)]
public class ForumSchemaTests
{
    static Db NewDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long Category(Db db, string slug, string minTrust = "new") =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_categories(slug,title,min_trust_to_post) VALUES(?,?,?)",
            slug, "Category " + slug, minTrust);

    static long Thread(Db db, long categoryId, string slug) =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_threads(category_id,slug,title,author_user_id) VALUES(?,?,?,1)",
            categoryId, slug, "Thread " + slug);

    static long Post(Db db, long threadId, long authorId = 1) =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_posts(thread_id,author_user_id,kind) VALUES(?,?,'reply')",
            threadId, authorId);

    static long Revision(Db db, long postId, int no, string body) =>
        db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_post_revisions(post_id,revision_no,body,edited_by_user_id) VALUES(?,?,?,1)",
            postId, no, body);

    // ── the revision chain ────────────────────────────────────────────────────────────────────

    [Fact]
    public void TwoEditsCannotClaimTheSameRevisionNumber()
    {
        // The invariant §28.11 rests on. If both landed, "which text was reported" would have two
        // answers, and a moderation decision would be unattributable to any particular words.
        var db = NewDb();
        var post = Post(db, Thread(db, Category(db, "c-rev"), "t-rev"));
        Revision(db, post, 1, "first");

        Assert.ThrowsAny<Exception>(() => Revision(db, post, 1, "a racing second edit"));
    }

    [Fact]
    public void AnEditAddsAHistoryEntryRatherThanReplacingOne()
    {
        // Editing is append-only: the prior revision survives, which is what makes a silent edit
        // visible and gives an appeal something to appeal about.
        var db = NewDb();
        var post = Post(db, Thread(db, Category(db, "c-hist"), "t-hist"));
        var r1 = Revision(db, post, 1, "original wording");
        var r2 = Revision(db, post, 2, "revised wording");
        db.Execute("UPDATE pciworld_forum_posts SET current_revision_id=?, edited_at=datetime('now') WHERE id=?", r2, post);

        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_forum_post_revisions WHERE post_id=?", post));
        Assert.Equal("original wording",
            H.Str(db.QueryOne("SELECT body FROM pciworld_forum_post_revisions WHERE id=?", r1)!["body"]));
        Assert.Equal(r2, H.L(db.QueryOne("SELECT current_revision_id FROM pciworld_forum_posts WHERE id=?", post)!["current_revision_id"]));
    }

    [Fact]
    public void DifferentPostsNumberTheirRevisionsIndependently()
    {
        // Revision 1 belongs to a post, not to the forum — a global sequence would make the number
        // meaningless and couple unrelated edits.
        var db = NewDb();
        var thread = Thread(db, Category(db, "c-indep"), "t-indep");
        Revision(db, Post(db, thread), 1, "a");
        Revision(db, Post(db, thread), 1, "b");
        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_forum_post_revisions WHERE revision_no=1"));
    }

    [Fact]
    public void ThePostRowCarriesNoBodyOfItsOwn()
    {
        // A post is a container with a state; the words live in revisions. A body column here would
        // immediately become the place someone edits in place, which is the whole thing §28.11
        // forbids.
        var cols = NewDb().Columns("pciworld_forum_posts");
        Assert.DoesNotContain("body", cols);
        Assert.Contains("current_revision_id", cols);
        Assert.Contains("state", cols);
    }

    [Fact]
    public void DeletingHidesWithoutErasingTheHistory()
    {
        // An author's delete is a state change. Revisions and any moderation decision survive for
        // the report and appeal windows — the same distinction Phase 2 draws between hiding a
        // message and destroying evidence.
        var db = NewDb();
        var post = Post(db, Thread(db, Category(db, "c-del"), "t-del"));
        Revision(db, post, 1, "words that were said");
        db.Execute("UPDATE pciworld_forum_posts SET state='deleted' WHERE id=?", post);

        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_forum_post_revisions WHERE post_id=?", post));
        Assert.Equal("deleted", H.Str(db.QueryOne("SELECT state FROM pciworld_forum_posts WHERE id=?", post)!["state"]));
    }

    // ── slugs, because this surface is crawlable ──────────────────────────────────────────────

    [Fact]
    public void CategoryAndTagSlugsAreUnique()
    {
        var db = NewDb();
        Category(db, "estimating");
        Assert.ThrowsAny<Exception>(() => Category(db, "estimating"));

        db.Execute("INSERT INTO pciworld_forum_tags(slug,label) VALUES('eot','EOT')");
        Assert.ThrowsAny<Exception>(() =>
            db.Execute("INSERT INTO pciworld_forum_tags(slug,label) VALUES('eot','Extension of time')"));
    }

    [Fact]
    public void ThreadSlugsAreUniqueWithinACategoryButNotAcrossThem()
    {
        // A duplicate slug in one category is a permanently ambiguous URL. Across categories it is
        // not — two categories may each legitimately have a 'welcome' thread, and a global
        // constraint would let whichever existed first win for ever.
        var db = NewDb();
        var a = Category(db, "c-a");
        var b = Category(db, "c-b");
        Thread(db, a, "welcome");
        Thread(db, b, "welcome");
        Assert.ThrowsAny<Exception>(() => Thread(db, a, "welcome"));
    }

    // ── flags are not sanctions ───────────────────────────────────────────────────────────────

    [Fact]
    public void OnePersonCanFlagAPostOnlyOnce()
    {
        // Without this, one account refreshing a page reaches the hide threshold alone — which is
        // exactly what capping flag weight exists to prevent.
        var db = NewDb();
        var post = Post(db, Thread(db, Category(db, "c-flag"), "t-flag"));
        db.Execute("INSERT INTO pciworld_forum_flags(post_id,flagged_by_user_id,weight) VALUES(?,?,1)", post, 7);
        Assert.ThrowsAny<Exception>(() =>
            db.Execute("INSERT INTO pciworld_forum_flags(post_id,flagged_by_user_id,weight) VALUES(?,?,1)", post, 7));
    }

    [Fact]
    public void DifferentPeopleMayEachFlagTheSamePost()
    {
        var db = NewDb();
        var post = Post(db, Thread(db, Category(db, "c-flag2"), "t-flag2"));
        db.Execute("INSERT INTO pciworld_forum_flags(post_id,flagged_by_user_id,weight) VALUES(?,?,1)", post, 1);
        db.Execute("INSERT INTO pciworld_forum_flags(post_id,flagged_by_user_id,weight) VALUES(?,?,2)", post, 2);
        Assert.Equal(2, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_forum_flags WHERE post_id=?", post));
    }

    // ── trust standing ────────────────────────────────────────────────────────────────────────

    [Fact]
    public void StandingStoresTheLevelByNameRatherThanAsANumber()
    {
        // A numeric column silently reinterprets itself if the ladder ever gains a rung; 'trusted'
        // cannot. Same reasoning for min_trust_to_post on a category.
        var db = NewDb();
        Assert.Contains("level", db.Columns("pciworld_forum_standing"));
        db.Execute("INSERT INTO pciworld_forum_standing(user_id,level,accepted_posts) VALUES(1,'trusted',40)");
        Assert.Equal(ForumTrust.Level.Trusted,
            ForumTrust.Parse(H.Str(db.QueryOne("SELECT level FROM pciworld_forum_standing WHERE user_id=1")!["level"])));
    }

    [Fact]
    public void ADemotionIsRecordedAsAnEventRatherThanOnlyOverwritingTheLevel()
    {
        // A level that changes with no history behind it cannot be explained to the person it
        // happened to, and an unexplainable demotion is indistinguishable from a bug.
        var db = NewDb();
        db.Execute("INSERT INTO pciworld_forum_standing(user_id,level) VALUES(2,'member')");
        db.Execute(@"INSERT INTO pciworld_forum_standing_events(user_id,from_level,to_level,reason)
                     VALUES(2,'member','new','upheld_report')");
        db.Execute("UPDATE pciworld_forum_standing SET level='new' WHERE user_id=2");

        var ev = db.QueryOne("SELECT from_level,to_level,reason FROM pciworld_forum_standing_events WHERE user_id=2");
        Assert.Equal("member", H.Str(ev!["from_level"]));
        Assert.Equal("upheld_report", H.Str(ev["reason"]));
    }

    // ── install-time behaviour ────────────────────────────────────────────────────────────────

    [Fact]
    public void TheForumShipsDisabled()
    {
        Assert.False(Settings.Bool(NewDb(), "pciworld_forum_enabled", false));
    }

    [Fact]
    public void AnOperatorsChoiceSurvivesAReboot()
    {
        var db = NewDb();
        Settings.Put(db, "pciworld_forum_enabled", "1");
        ForumSchema.Ensure(db);
        Assert.True(Settings.Bool(db, "pciworld_forum_enabled", false));
    }

    [Fact]
    public void EnsureIsIdempotent()
    {
        // It runs on every boot, so running it twice must be a no-op rather than an error.
        var db = NewDb();
        ForumSchema.Ensure(db);
        ForumSchema.Ensure(db);
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_forum_threads"));
    }

    [Fact]
    public void TheLegacyForumIsLeftEntirelyAlone()
    {
        // D-002: the main-PCI forum keeps serving the Institute site, unmigrated. If these tables
        // ever started sharing rows with it, §9.1's prohibition on attributing anonymous historical
        // content to real members would be broken by construction.
        var db = NewDb();
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_forum_threads"));
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_forum_posts"));
    }
}
