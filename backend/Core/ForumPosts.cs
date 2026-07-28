using PCI.Backend.Data;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Core;

/// <summary>
/// The forum write path (docs/pciworld/CCP_PHASE3_DESIGN.md §2, §4).
///
/// TRUST CHANGES TIMING, NEVER OUTCOME — and this is the file where that could most easily stop
/// being true, so it is worth being exact about what each half does.
///
///   • <see cref="CommunityModeration.Resolve"/> decides whether the words may be published. Trust
///     is not passed to it and there is no parameter through which it could be.
///   • Trust decides only what happens to a post the policy ALLOWED: a new account's post waits in
///     a human review queue; an established member's publishes.
///
/// So a post the policy refuses is refused for everybody, and a post the policy allows is never
/// published *sooner* than the policy permits — only later, for people who have not earned the
/// benefit of the doubt. A brand-new account is the cheapest thing an abuser has, which is why the
/// friction sits there rather than on the classifier's threshold.
///
/// EDITS NEVER MUTATE PUBLISHED TEXT (§28.11). <see cref="Edit"/> writes a new revision and
/// repoints the post. The prior revision stays, which is what lets a moderator reviewing a report
/// see the text that was actually reported rather than whatever it has since become.
/// </summary>
public static class ForumPosts
{
    public static bool Enabled(Db db) => Settings.Bool(db, "pciworld_forum_enabled", false);

    /// <summary>Bodies are bounded generously — a forum post is meant to be an argument, not a
    /// chat line — but bounded, because an unbounded TEXT column is a denial-of-service lever.</summary>
    public const int MaxBodyLength = 20_000;

    public record Result(bool Ok, long PostId, long RevisionId, string State, string Reason)
    {
        public static Result Rejected(string reason) => new(false, 0, 0, "rejected", reason);
        /// <summary>Whether anyone other than the author can currently see this.</summary>
        public bool Visible => State == "published";
    }

    /// <summary>
    /// Create a post. Returns a refusal as an ordinary result with a reason code — never an
    /// exception — because every branch here is reachable by an authenticated stranger.
    /// </summary>
    public static Result Create(Db db, long threadId, long authorUserId, string? body,
                                ForumTrust.Level level, ITextModerator moderator,
                                IReadOnlyList<Rule> policy, long? replyTo = null,
                                string kind = "reply", string? correlationId = null)
    {
        if (!Enabled(db)) return Result.Rejected("forum_disabled");

        var thread = db.QueryOne(
            @"SELECT t.id, t.state AS thread_state, c.id AS category_id, c.state AS category_state,
                     c.min_trust_to_post
              FROM pciworld_forum_threads t
              JOIN pciworld_forum_categories c ON c.id = t.category_id
              WHERE t.id=?", threadId);
        if (thread is null) return Result.Rejected("thread_not_found");

        if (H.Str(thread["thread_state"]) != "open") return Result.Rejected("thread_not_open");
        if (H.Str(thread["category_state"]) != "open") return Result.Rejected("category_not_open");

        // The category's own floor. This is what makes an announcements category read-only without a
        // second permission system beside WorldRbac — and it is checked for REPLIES as well as for
        // new threads, because "post a reply instead" would otherwise be the bypass.
        var required = ForumTrust.Parse(H.Str(thread["min_trust_to_post"]));
        if (!ForumTrust.AtLeast(level, required))
            return Result.Rejected("insufficient_trust_for_category");

        var text = (body ?? "").Trim();
        if (text.Length == 0) return Result.Rejected("empty_post");
        if (text.Length > MaxBodyLength) return Result.Rejected("post_too_long");

        // Links are the single most common spam payload. Keeping them away from brand-new accounts
        // removes most of it before a classifier has to catch it — cheaper and more reliable than
        // asking a model to tell promotional links from useful ones.
        if (!ForumTrust.MayPostLinks(level) && ContainsLink(text))
            return Result.Rejected("links_not_allowed_yet");

        // A moderator that throws is treated as unavailable rather than crashing the write path: an
        // exception here would lose the post entirely, which is worse than failing closed.
        Signal signal;
        try { signal = moderator.Classify(text, "en"); }
        catch { signal = Signal.Unavailable; }

        var decision = Resolve(policy, signal, Repetition(db, authorUserId), contentType: "post");

        // The two halves, kept visibly separate.
        //
        //   `decision.Publishes` — may these words be published at all? Trust cannot influence it.
        //   `HoldsBeforePublication` — should THIS AUTHOR's allowed post wait for a human?
        var state = !decision.Publishes ? StateFor(decision.Outcome)
                  : ForumTrust.HoldsBeforePublication(level) ? "pending"
                  : "published";

        long postId = 0, revisionId = 0;
        db.Transaction(() =>
        {
            postId = db.ExecuteReturningId(
                @"INSERT INTO pciworld_forum_posts(thread_id,author_user_id,state,kind,reply_to_post_id,published_at)
                  VALUES(?,?,?,?,?,?)",
                threadId, authorUserId, state, kind, replyTo,
                state == "published" ? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") : null);

            revisionId = WriteRevision(db, postId, 1, text, authorUserId, null);
            db.Execute("UPDATE pciworld_forum_posts SET current_revision_id=? WHERE id=?", revisionId, postId);

            // The decision carries a HASH of the content, never the content: withheld text belongs
            // in the post's own revision under the retention policy, not copied into an audit row.
            var decisionId = db.ExecuteReturningId(
                @"INSERT INTO pciworld_moderation_decisions
                      (scope,subject_ref,content_hash,rule_id,provider,provider_version,category,
                       severity,confidence_band,context_rule,repetition_count,outcome,reason_code,correlation_id)
                  VALUES('forum_post',?,?,?,?,?,?,?,?,?,?,?,?,?)",
                postId.ToString(), Security.Sha(text), decision.RuleId,
                moderator.ProviderName, moderator.ProviderVersion, decision.Category, signal.Severity,
                decision.Confidence.ToString().ToLowerInvariant(), decision.ContextRule, decision.Repetition,
                decision.Outcome.ToString().ToLowerInvariant(), decision.ReasonCode, correlationId);
            db.Execute("UPDATE pciworld_forum_posts SET decision_id=? WHERE id=?", decisionId, postId);

            // Thread counters move ONLY for a post people can actually see. A pending post that
            // bumped "last activity" would advertise content nobody can read, and would let a
            // withheld post push a thread up the listing — a free promotion for the worst content.
            if (state == "published")
                db.Execute(
                    @"UPDATE pciworld_forum_threads
                      SET reply_count = reply_count + ?, last_post_at = datetime('now'),
                          last_post_user_id = ?, updated_at = datetime('now')
                      WHERE id=?",
                    kind == "reply" ? 1 : 0, authorUserId, threadId);
        });

        return new Result(true, postId, revisionId, state, decision.ReasonCode);
    }

    /// <summary>
    /// Edit a post: a NEW revision, never a mutation of the old one (§28.11).
    ///
    /// Re-moderated, because an edit is new words. An edit that fails moderation withdraws the post
    /// rather than reverting silently to the previous revision — silently reverting would hide from
    /// the author that their edit was refused, and hide from a moderator that it was attempted.
    /// </summary>
    public static Result Edit(Db db, long postId, long editorUserId, string? body,
                              ForumTrust.Level level, ITextModerator moderator,
                              IReadOnlyList<Rule> policy, string? editReason = null)
    {
        if (!Enabled(db)) return Result.Rejected("forum_disabled");
        if (!ForumTrust.MayEditOwnPosts(level)) return Result.Rejected("editing_not_allowed_yet");

        var post = db.QueryOne("SELECT id,author_user_id,state FROM pciworld_forum_posts WHERE id=?", postId);
        if (post is null) return Result.Rejected("post_not_found");
        if (H.L(post["author_user_id"]) != editorUserId) return Result.Rejected("not_your_post");
        // A withdrawn or deleted post is not editable back into existence. Restoring one is a
        // moderator action with its own record, not something the author does by typing.
        if (H.Str(post["state"]) is not ("published" or "pending")) return Result.Rejected("post_not_editable");

        var text = (body ?? "").Trim();
        if (text.Length == 0) return Result.Rejected("empty_post");
        if (text.Length > MaxBodyLength) return Result.Rejected("post_too_long");
        if (!ForumTrust.MayPostLinks(level) && ContainsLink(text)) return Result.Rejected("links_not_allowed_yet");

        Signal signal;
        try { signal = moderator.Classify(text, "en"); }
        catch { signal = Signal.Unavailable; }
        var decision = Resolve(policy, signal, Repetition(db, editorUserId), contentType: "post");

        long revisionId = 0;
        var state = decision.Publishes ? H.Str(post["state"]) ?? "published" : StateFor(decision.Outcome);

        db.Transaction(() =>
        {
            // Allocate the next number under the row, and let the unique index be the backstop: two
            // concurrent edits that both computed the same number must have one fail loudly rather
            // than quietly interleave, or "which text was reported" has two answers.
            var next = db.Scalar<long>(
                "SELECT COALESCE(MAX(revision_no),0)+1 FROM pciworld_forum_post_revisions WHERE post_id=?", postId);
            revisionId = WriteRevision(db, postId, (int)next, text, editorUserId, editReason);

            db.Execute(
                @"UPDATE pciworld_forum_posts
                  SET current_revision_id=?, state=?, edited_at=datetime('now'),
                      updated_at=datetime('now'), version=version+1
                  WHERE id=?",
                revisionId, state, postId);
        });

        return new Result(true, postId, revisionId, state, decision.ReasonCode);
    }

    /// <summary>
    /// An author hiding their own post. A state change, never a delete: revisions and the
    /// moderation history survive, because an appeal needs something to be about and a report on a
    /// post that vanished is unanswerable.
    /// </summary>
    public static Result Withdraw(Db db, long postId, long authorUserId)
    {
        var post = db.QueryOne("SELECT id,author_user_id,state FROM pciworld_forum_posts WHERE id=?", postId);
        if (post is null) return Result.Rejected("post_not_found");
        if (H.L(post["author_user_id"]) != authorUserId) return Result.Rejected("not_your_post");

        db.Execute(
            "UPDATE pciworld_forum_posts SET state='deleted', updated_at=datetime('now'), version=version+1 WHERE id=?",
            postId);
        return new Result(true, postId, 0, "deleted", "withdrawn_by_author");
    }

    // ── internals ─────────────────────────────────────────────────────────────────────────────

    static long WriteRevision(Db db, long postId, int no, string text, long editorId, string? reason) =>
        db.ExecuteReturningId(
            @"INSERT INTO pciworld_forum_post_revisions
                  (post_id,revision_no,body,body_rendered,body_hash,edited_by_user_id,edit_reason)
              VALUES(?,?,?,?,?,?,?)",
            postId, no, text,
            // Sanitised at WRITE time so the read path never renders unsanitised author input, and a
            // later change to the sanitiser cannot retroactively expose an old post. The raw body is
            // kept beside it for editing and for evidence.
            HtmlSanitize.Clean(text), Security.Sha(text), editorId, reason);

    static string StateFor(Outcome outcome) => outcome switch
    {
        Outcome.Quarantine => "pending",   // held for a human, not refused
        _ => "withheld",
    };

    /// <summary>Prior adverse findings against this author, which the matrix uses to escalate a
    /// repeat offender deterministically rather than waiting for the classifier to grow more
    /// certain.</summary>
    static int Repetition(Db db, long userId) => (int)db.Scalar<long>(
        @"SELECT COUNT(*) FROM pciworld_moderation_decisions d
          JOIN pciworld_forum_posts p ON p.decision_id = d.id
          WHERE p.author_user_id=? AND d.outcome IN ('block','eject','escalate')", userId);

    /// <summary>
    /// Deliberately broad. This is a *trust gate*, not a URL parser: catching bare domains and
    /// obfuscated dots matters more than avoiding the occasional false positive on someone writing
    /// "see clause 4.2 above", because the cost of a false positive is a new account waiting one
    /// accepted post before it may include links.
    /// </summary>
    public static bool ContainsLink(string text)
    {
        var t = text.ToLowerInvariant();
        if (t.Contains("http://") || t.Contains("https://") || t.Contains("www.")) return true;
        // "example dot com" and "example[.]com" are the standard ways round a naive check.
        if (t.Contains("[.]") || t.Contains("(.)") || t.Contains(" dot ")) return true;
        return System.Text.RegularExpressions.Regex.IsMatch(
            t, @"\b[a-z0-9-]+\.(com|net|org|io|co|ru|cn|info|biz|xyz|top|link)\b");
    }
}
