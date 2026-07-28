namespace PCI.Backend.Data;

/// <summary>
/// PCI World forum — data model installer (docs/pciworld/CCP_PHASE3_DESIGN.md §4–§6).
///
/// Built new per decision D-002. The legacy main-PCI `forum_*` tables stay exactly where they are,
/// unmigrated, still serving the Institute site: their content is anonymous free-text display names
/// with no accounts, and §9.1 forbids assigning anonymous historical content to real members. There
/// is therefore nothing to migrate *into* a member-authored model, and pretending otherwise would
/// attribute strangers' words to named professionals.
///
/// Installed on every boot on both providers, and — as with every other flag in this increment —
/// INSTALLING THE SCHEMA IS NOT LAUNCHING THE FEATURE. `pciworld_forum_enabled` seeds '0'.
///
/// TWO INVARIANTS LIVE HERE RATHER THAN IN APPLICATION CODE, because application code is where
/// races live:
///
///   1. <c>UNIQUE(post_id, revision_no)</c> on revisions. An edit writes a NEW revision and
///      repoints the post; §28.11 forbids modifying published content in place. Two concurrent
///      edits that both computed revision 4 must have one of them fail loudly rather than quietly
///      interleave — otherwise a moderator reviewing a report cannot know which text was reported.
///   2. <c>UNIQUE(slug)</c> on categories, tags and (per category) threads. A duplicate slug is a
///      permanently ambiguous URL, and this surface is crawlable.
///
/// WHY THE TAXONOMY IS DATA. The legacy forum hardcodes five categories in C#. Here they are rows,
/// which is what makes `min_trust_to_post` possible at all: a category like Announcements becomes
/// read-only to most people without inventing a second permission system beside WorldRbac.
/// </summary>
public static class ForumSchema
{
    public static void Ensure(Db db)
    {
        Tables(db);
        Seed(db);
    }

    static void Tables(Db db)
    {
        // Datetime columns are VARCHAR(32), never TEXT — the same structural reason as every other
        // table in this increment: TEXT becomes LONGTEXT on MySQL, and an index touching it exceeds
        // InnoDB's 3072-byte key limit. Several of these ARE indexed (thread recency, queue age).

        // ── Categories. `min_trust_to_post` is the whole reason the taxonomy is data. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_categories(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(120) UNIQUE NOT NULL,
            title TEXT NOT NULL,
            description TEXT,
            sort INTEGER NOT NULL DEFAULT 100,
            -- Stored as the level NAME, not an integer: a numeric column silently reinterprets
            -- itself if the ladder ever gains a rung, and 'trusted' cannot.
            min_trust_to_post VARCHAR(16) NOT NULL DEFAULT 'new',
            state VARCHAR(16) NOT NULL DEFAULT 'open',     -- open|read_only|hidden|archived
            locale VARCHAR(16) NOT NULL DEFAULT 'en',
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wforumcat_state ON pciworld_forum_categories(state,sort)");

        // ── Threads. `solved_post_id` is both a courtesy to the next reader and what makes QAPage
        //    structured data honest — claiming an accepted answer without one would be a lie to a
        //    search engine as well as to a person. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_threads(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            category_id INTEGER NOT NULL,
            slug VARCHAR(160) NOT NULL,
            title TEXT NOT NULL,
            author_user_id INTEGER,
            state VARCHAR(16) NOT NULL DEFAULT 'open',     -- open|locked|archived|hidden
            is_pinned INTEGER NOT NULL DEFAULT 0,
            reply_count INTEGER NOT NULL DEFAULT 0,
            last_post_at VARCHAR(32),
            last_post_user_id INTEGER,
            view_count INTEGER NOT NULL DEFAULT 0,
            solved_post_id INTEGER,
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");
        // Unique per CATEGORY rather than globally: two categories may each legitimately have a
        // 'welcome' thread, and a global constraint would make the first one to exist win for ever.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wforumthread_slug
                  ON pciworld_forum_threads(category_id,slug)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wforumthread_recent ON pciworld_forum_threads(category_id,state,last_post_at)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wforumthread_author ON pciworld_forum_threads(author_user_id)");

        // ── Posts. `current_revision_id` points at the text that is live; the revision rows are the
        //    history. A post is a container with a state, NOT the words — which is what lets an
        //    author delete without erasing evidence, and lets a moderator see the revision that was
        //    actually reported rather than whatever it has since become. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_posts(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            thread_id INTEGER NOT NULL,
            author_user_id INTEGER,
            current_revision_id INTEGER,
            state VARCHAR(16) NOT NULL DEFAULT 'pending',  -- pending|published|withheld|withdrawn|deleted
            kind VARCHAR(16) NOT NULL DEFAULT 'reply',     -- opening|reply
            reply_to_post_id INTEGER,
            flag_weight INTEGER NOT NULL DEFAULT 0,
            decision_id INTEGER,
            published_at VARCHAR(32),
            edited_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wforumpost_thread ON pciworld_forum_posts(thread_id,state,id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wforumpost_author ON pciworld_forum_posts(author_user_id,state)");
        // The moderation queue reads state + age; without this it is a full scan once the forum has
        // any history at all.
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wforumpost_queue ON pciworld_forum_posts(state,created_at)");

        // ── Revisions. APPEND-ONLY (§28.11). ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_post_revisions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            post_id INTEGER NOT NULL,
            revision_no INTEGER NOT NULL,
            body TEXT NOT NULL,
            -- Sanitised at WRITE time, so the read path never renders unsanitised author input and
            -- a later change to the sanitiser cannot retroactively expose an old post. The raw body
            -- is kept alongside it for editing and for evidence.
            body_rendered TEXT,
            body_hash VARCHAR(64),
            edited_by_user_id INTEGER,
            edit_reason TEXT,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        // The ordering invariant. Two concurrent edits that both computed revision 4 must not both
        // succeed: a moderator reviewing a report has to be able to say which text was reported.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wforumrev_no
                  ON pciworld_forum_post_revisions(post_id,revision_no)");

        // ── Tags. Free-form taxonomy alongside the fixed category tree. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_tags(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(80) UNIQUE NOT NULL,
            label TEXT NOT NULL,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");

        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_thread_tags(
            thread_id INTEGER NOT NULL,
            tag_id INTEGER NOT NULL,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        // Composite uniqueness rather than a PRIMARY KEY clause: Db.Translate handles a unique index
        // identically on both providers, and a table-level composite PK does not translate as
        // cleanly. Same guarantee, one dialect.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wforumthreadtag
                  ON pciworld_forum_thread_tags(thread_id,tag_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wforumthreadtag_tag ON pciworld_forum_thread_tags(tag_id)");

        // ── Flags. NOT reports — a flag is 'this is low quality', a report is 'this breaks the
        //    rules'. Reports reuse the community tables and open the same cases; flags feed a
        //    weighted counter that can hide a post pending review and can never sanction anybody. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_flags(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            post_id INTEGER NOT NULL,
            flagged_by_user_id INTEGER NOT NULL,
            weight INTEGER NOT NULL DEFAULT 0,
            reason VARCHAR(48),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        // One flag per person per post. Without this, one account refreshing a page could reach the
        // hide threshold alone — which is precisely what the weight cap exists to prevent.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wforumflag_once
                  ON pciworld_forum_flags(post_id,flagged_by_user_id)");

        // ── Trust standing. The ladder is computed from facts by ForumTrust; this is where the
        //    facts and the resulting level live, plus an APPEND-ONLY history so a demotion can be
        //    explained rather than merely asserted. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_standing(
            user_id INTEGER PRIMARY KEY,
            level VARCHAR(16) NOT NULL DEFAULT 'new',
            accepted_posts INTEGER NOT NULL DEFAULT 0,
            upheld_reports INTEGER NOT NULL DEFAULT 0,
            accurate_flags INTEGER NOT NULL DEFAULT 0,
            first_post_at VARCHAR(32),
            staff_granted INTEGER NOT NULL DEFAULT 0,
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");

        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_forum_standing_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            from_level VARCHAR(16),
            to_level VARCHAR(16) NOT NULL,
            reason VARCHAR(64) NOT NULL,
            actor_admin_id INTEGER,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wforumstanding_user ON pciworld_forum_standing_events(user_id,id)");
    }

    static void Seed(Db db)
    {
        // Off by default, INSERT OR IGNORE so a later boot never overwrites an operator's choice —
        // the same reasoning as every other flag in this increment.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('pciworld_forum_enabled','0')");
    }
}
