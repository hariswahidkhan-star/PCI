using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Tests;

/// <summary>
/// The forum write path (Core/ForumPosts.cs; docs/pciworld/CCP_PHASE3_DESIGN.md §2, §4).
///
/// The tests that carry the weight are the ones that try to make trust change an OUTCOME rather
/// than a timing. That is the failure this design could most plausibly slide into — "trusted
/// authors get a softer rule" is a sentence that sounds reasonable in a hurry — so it is asserted
/// against directly: the same refused words are refused at every level, including staff.
///
/// The second group is about edits. §28.11's value is entirely in a moderator being able to see the
/// revision that was reported; an edit that mutated text in place, or that silently reverted when
/// refused, would destroy exactly that.
/// </summary>
[Collection(DbCollection.Name)]
public class ForumPostsTests
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

    static long Thread(Db db, string slug, string minTrust = "new", string threadState = "open",
                       string categoryState = "open")
    {
        var cat = db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_categories(slug,title,min_trust_to_post,state) VALUES(?,?,?,?)",
            "cat-" + slug, "Cat " + slug, minTrust, categoryState);
        return db.ExecuteReturningId(
            "INSERT INTO pciworld_forum_threads(category_id,slug,title,author_user_id,state) VALUES(?,?,?,1,?)",
            cat, slug, "Thread " + slug, threadState);
    }

    static string Body(Db db, long postId) =>
        H.Str(db.QueryOne(
            @"SELECT r.body FROM pciworld_forum_post_revisions r
              JOIN pciworld_forum_posts p ON p.current_revision_id = r.id
              WHERE p.id=?", postId)!["body"])!;

    const string Clean = "Float on activity B is eight days; the critical path runs through piling.";
    const string Abusive = "you are an idiot and a loser";

    // ── trust changes timing, never outcome ───────────────────────────────────────────────────

    [Fact]
    public void RefusedWordsAreRefusedAtEveryTrustLevelIncludingStaff()
    {
        // The single assertion this design most needs. A head start is not an exemption.
        foreach (var level in Enum.GetValues<ForumTrust.Level>())
        {
            var db = NewDb();
            var r = ForumPosts.Create(db, Thread(db, "t-" + level), 1, Abusive, level, Mod, Policy);
            Assert.True(r.Ok);
            Assert.False(r.Visible, $"{level} published refused words");
        }
    }

    [Fact]
    public void AnAllowedPostWaitsForANewAccountAndPublishesForAMember()
    {
        // The whole of trust's influence, in one comparison: same words, same verdict, different
        // timing.
        var db = NewDb();
        var thread = Thread(db, "t-timing");

        var newbie = ForumPosts.Create(db, thread, 1, Clean, ForumTrust.Level.New, Mod, Policy);
        Assert.Equal("pending", newbie.State);
        Assert.False(newbie.Visible);

        var member = ForumPosts.Create(db, thread, 2, Clean, ForumTrust.Level.Member, Mod, Policy);
        Assert.Equal("published", member.State);
        Assert.True(member.Visible);
    }

    [Fact]
    public void APendingPostDoesNotBumpTheThread()
    {
        // A pending post that moved "last activity" would advertise content nobody can read, and
        // would let a withheld post push its thread up the listing — a free promotion for exactly
        // the wrong content.
        var db = NewDb();
        var thread = Thread(db, "t-bump");
        ForumPosts.Create(db, thread, 1, Clean, ForumTrust.Level.New, Mod, Policy);

        var row = db.QueryOne("SELECT reply_count,last_post_at FROM pciworld_forum_threads WHERE id=?", thread);
        Assert.Equal(0, H.L(row!["reply_count"]));
        Assert.Null(row["last_post_at"]);
    }

    [Fact]
    public void AWithheldPostDoesNotBumpTheThreadEither()
    {
        var db = NewDb();
        var thread = Thread(db, "t-bump2");
        ForumPosts.Create(db, thread, 1, Abusive, ForumTrust.Level.Trusted, Mod, Policy);
        Assert.Equal(0, H.L(db.QueryOne("SELECT reply_count FROM pciworld_forum_threads WHERE id=?", thread)!["reply_count"]));
    }

    [Fact]
    public void AnUnavailableModeratorPublishesNothingAtAnyLevel()
    {
        // Fail-closed, and trust does not buy a way past it.
        var db = NewDb();
        var thread = Thread(db, "t-null");
        foreach (var level in new[] { ForumTrust.Level.New, ForumTrust.Level.Member, ForumTrust.Level.Staff })
            Assert.False(ForumPosts.Create(db, thread, 1, Clean, level,
                new CommunityModerators.NullModerator(), Policy).Visible);
    }

    // ── the category floor ────────────────────────────────────────────────────────────────────

    [Fact]
    public void ACategoryFloorAppliesToRepliesAndNotOnlyToNewThreads()
    {
        // "Post a reply instead" would otherwise be the bypass that makes an announcements category
        // writable by anybody.
        var db = NewDb();
        var thread = Thread(db, "t-floor", minTrust: "trusted");

        Assert.Equal("insufficient_trust_for_category",
            ForumPosts.Create(db, thread, 1, Clean, ForumTrust.Level.Member, Mod, Policy, kind: "reply").Reason);
        Assert.True(ForumPosts.Create(db, thread, 2, Clean, ForumTrust.Level.Trusted, Mod, Policy).Visible);
    }

    [Fact]
    public void ALockedThreadOrClosedCategoryRefusesPosts()
    {
        var db = NewDb();
        Assert.Equal("thread_not_open",
            ForumPosts.Create(db, Thread(db, "t-locked", threadState: "locked"), 1, Clean,
                              ForumTrust.Level.Member, Mod, Policy).Reason);
        Assert.Equal("category_not_open",
            ForumPosts.Create(db, Thread(db, "t-catro", categoryState: "read_only"), 1, Clean,
                              ForumTrust.Level.Member, Mod, Policy).Reason);
    }

    [Fact]
    public void TheForumRefusesEverythingWhileDisabled()
    {
        var db = NewDb();
        var thread = Thread(db, "t-off");
        Settings.Put(db, "pciworld_forum_enabled", "0");
        Assert.Equal("forum_disabled",
            ForumPosts.Create(db, thread, 1, Clean, ForumTrust.Level.Staff, Mod, Policy).Reason);
    }

    // ── links ─────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void ABrandNewAccountCannotPostLinksButABasicOneCan()
    {
        var db = NewDb();
        var thread = Thread(db, "t-links");
        var withLink = "Useful reference: https://example.com/guide";
        Assert.Equal("links_not_allowed_yet",
            ForumPosts.Create(db, thread, 1, withLink, ForumTrust.Level.New, Mod, Policy).Reason);
        Assert.True(ForumPosts.Create(db, thread, 2, withLink, ForumTrust.Level.Basic, Mod, Policy).Ok);
    }

    [Fact]
    public void ObfuscatedLinksAreCaughtToo()
    {
        // The standard ways round a naive check. This is a trust gate, not a URL parser — catching
        // these matters more than avoiding an occasional false positive.
        foreach (var text in new[]
                 {
                     "visit example[.]com now", "reach me at example dot com",
                     "see www.example.org", "buy at cheapstuff.xyz",
                 })
            Assert.True(ForumPosts.ContainsLink(text), $"missed: {text}");
    }

    [Fact]
    public void OrdinaryProseIsNotMistakenForALink()
    {
        Assert.False(ForumPosts.ContainsLink("See clause 4.2 above and section 3.1 of the contract."));
    }

    // ── edits ─────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void AnEditWritesANewRevisionAndLeavesTheOldOneIntact()
    {
        // §28.11's value is entirely in this: a moderator reviewing a report can see the text that
        // was reported rather than whatever it has since become.
        var db = NewDb();
        var post = ForumPosts.Create(db, Thread(db, "t-edit"), 1, Clean, ForumTrust.Level.Member, Mod, Policy);
        var edited = ForumPosts.Edit(db, post.PostId, 1, "Revised: the float is nine days.",
                                     ForumTrust.Level.Member, Mod, Policy, "corrected a number");

        Assert.True(edited.Ok, edited.Reason);
        Assert.Equal(2, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_forum_post_revisions WHERE post_id=?", post.PostId));
        Assert.Equal(Clean, H.Str(db.QueryOne(
            "SELECT body FROM pciworld_forum_post_revisions WHERE post_id=? AND revision_no=1", post.PostId)!["body"]));
        Assert.StartsWith("Revised:", Body(db, post.PostId));
    }

    [Fact]
    public void AnEditThatFailsModerationWithdrawsRatherThanSilentlyReverting()
    {
        // Silently reverting would hide from the author that their edit was refused, and hide from
        // a moderator that it was ever attempted.
        var db = NewDb();
        var post = ForumPosts.Create(db, Thread(db, "t-editbad"), 1, Clean, ForumTrust.Level.Member, Mod, Policy);
        var edited = ForumPosts.Edit(db, post.PostId, 1, Abusive, ForumTrust.Level.Member, Mod, Policy);

        Assert.False(edited.Visible);
        Assert.Equal(2, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_forum_post_revisions WHERE post_id=?", post.PostId));
    }

    [Fact]
    public void SomebodyElsesPostCannotBeEdited()
    {
        var db = NewDb();
        var post = ForumPosts.Create(db, Thread(db, "t-other"), 1, Clean, ForumTrust.Level.Member, Mod, Policy);
        Assert.Equal("not_your_post",
            ForumPosts.Edit(db, post.PostId, 99, "hijacked", ForumTrust.Level.Staff, Mod, Policy).Reason);
    }

    [Fact]
    public void EditingRequiresTheTrustLevelThatGrantsIt()
    {
        var db = NewDb();
        var post = ForumPosts.Create(db, Thread(db, "t-noedit"), 1, Clean, ForumTrust.Level.Member, Mod, Policy);
        Assert.Equal("editing_not_allowed_yet",
            ForumPosts.Edit(db, post.PostId, 1, "changed", ForumTrust.Level.Basic, Mod, Policy).Reason);
    }

    [Fact]
    public void ARenderedCopyIsStoredAlongsideTheRawBody()
    {
        // Sanitised at write time, so the read path never renders unsanitised author input and a
        // later change to the sanitiser cannot retroactively expose an old post.
        var db = NewDb();
        var post = ForumPosts.Create(db, Thread(db, "t-render"), 1,
            "Careful: <script>alert(1)</script> in a quote", ForumTrust.Level.Member, Mod, Policy);
        var rendered = H.Str(db.QueryOne(
            "SELECT body_rendered FROM pciworld_forum_post_revisions WHERE post_id=?", post.PostId)!["body_rendered"]);
        Assert.DoesNotContain("<script", rendered ?? "", StringComparison.OrdinalIgnoreCase);
    }

    // ── withdrawal ────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void AnAuthorDeletingHidesWithoutErasingTheHistory()
    {
        var db = NewDb();
        var post = ForumPosts.Create(db, Thread(db, "t-del"), 1, Clean, ForumTrust.Level.Member, Mod, Policy);
        Assert.True(ForumPosts.Withdraw(db, post.PostId, 1).Ok);

        Assert.Equal("deleted", H.Str(db.QueryOne(
            "SELECT state FROM pciworld_forum_posts WHERE id=?", post.PostId)!["state"]));
        // The words and the moderation decision survive — an appeal needs something to be about.
        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_forum_post_revisions WHERE post_id=?", post.PostId));
        Assert.NotNull(db.QueryOne("SELECT decision_id FROM pciworld_forum_posts WHERE id=?", post.PostId)!["decision_id"]);
    }

    [Fact]
    public void SomebodyElsesPostCannotBeWithdrawn()
    {
        var db = NewDb();
        var post = ForumPosts.Create(db, Thread(db, "t-delother"), 1, Clean, ForumTrust.Level.Member, Mod, Policy);
        Assert.Equal("not_your_post", ForumPosts.Withdraw(db, post.PostId, 99).Reason);
    }

    // ── the decision record ───────────────────────────────────────────────────────────────────

    [Fact]
    public void TheDecisionRecordsAHashRatherThanTheContent()
    {
        // Withheld text belongs in the post's own revision under the retention policy, not copied
        // into an audit row that outlives it.
        var db = NewDb();
        var post = ForumPosts.Create(db, Thread(db, "t-hash"), 1, Abusive, ForumTrust.Level.Member, Mod, Policy);
        var d = db.QueryOne(
            @"SELECT d.content_hash, d.scope FROM pciworld_moderation_decisions d
              JOIN pciworld_forum_posts p ON p.decision_id = d.id WHERE p.id=?", post.PostId);
        Assert.Equal("forum_post", H.Str(d!["scope"]));
        Assert.Equal(Security.Sha(Abusive), H.Str(d["content_hash"]));
        Assert.NotEqual(Abusive, H.Str(d["content_hash"]));
    }
}
