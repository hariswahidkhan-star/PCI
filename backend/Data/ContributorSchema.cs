namespace PCI.Backend.Data;

/// <summary>
/// PCI World contributor publishing — Phase 6 data model installer
/// (docs/pciworld/CCP_PHASE6_DESIGN.md §4, §5).
///
/// Installed on every boot on both providers, like every schema installer in this increment — and,
/// like every one of them, INSTALLING THE SCHEMA IS NOT LAUNCHING THE FEATURE.
/// `pciworld_contributors_enabled` seeds '0' and stays there until a person turns it on.
///
/// THIS FILE EXISTS TO CLOSE A REAL HOLE, not only to add tables. The editorial engine's
/// maker-checker compares <c>pciworld_articles.author_id</c> to the acting admin
/// (<c>WorldEditorial.Approve</c>), and both are <c>pciworld_admin_users</c> ids — correct for
/// house content. A CONTRIBUTOR is a <c>pciworld_users</c> row. The two id spaces are disjoint and
/// shared no recorded link, so that comparison could never fire for a contributor's article: a
/// staff editor who was also the contributor could approve and publish their own work, which is
/// the one thing this phase is required never to allow.
///
/// Comparing emails instead would be folklore, not identity. The fix is an EXPLICIT, owner-managed
/// link — <c>pciworld_admin_users.world_user_id</c> — so the comparison has a namespace to make it
/// in. What the link cannot do is prove that two accounts belong to one person when nobody has said
/// so; that residue is a conflict-of-interest matter for the editorial policy (CCP-P6-017), and the
/// design says so rather than implying the code closes it.
/// </summary>
public static class ContributorSchema
{
    public static void Ensure(Db db)
    {
        Tables(db);
        Upgrades(db);
        Seed(db);
    }

    static void Tables(Db db)
    {
        // ── The grant. One standing row per person: a state change is an UPDATE, never a second
        //    row, because "may this person publish under PCI's byline, and on whose authority"
        //    must have exactly one answer. The employer-member precedent. ──
        //
        // Granting is an ATTRIBUTABLE ACT, not a threshold crossing. The forum trust ladder is
        // evidence put in front of the editor, never the decision: an earned-only gate is a ratchet
        // an abuser plays for — post innocuously until it opens, then publish spam under PCI's
        // brand — and unlike a forum post, a published article is indexed under PCI's name in
        // caches nobody at PCI controls.
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_contributors(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            state VARCHAR(16) NOT NULL DEFAULT 'applied',   -- applied|granted|declined|revoked
            statement TEXT,
            terms_version VARCHAR(32),
            granted_at VARCHAR(32),
            granted_by_admin_id INTEGER,
            revoked_at VARCHAR(32),
            revoked_by_admin_id INTEGER,
            revoke_reason TEXT,
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_wcontrib_user ON pciworld_contributors(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcontrib_state ON pciworld_contributors(state)");

        // ── Append-only submission history. There is no UPDATE path to this table anywhere in the
        //    codebase: a status change that can be overwritten is a status change nobody can later
        //    explain, and "why was this refused" is precisely what a rejected contributor asks. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_contributor_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            article_id INTEGER NOT NULL,
            event VARCHAR(32) NOT NULL,          -- submitted|assigned|returned|accepted|declined|published|archived|note
            from_status VARCHAR(24),
            to_status VARCHAR(24),
            actor_kind VARCHAR(16) NOT NULL,     -- contributor|editor|system
            actor_id INTEGER,
            note TEXT,
            declarations_json TEXT,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcontrev_article ON pciworld_contributor_events(article_id,id)");

        // ── Which editor owns which manuscript. Unassignment is a timestamp rather than a delete,
        //    so "who was responsible for this on the day it published" stays answerable. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_contributor_assignments(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            article_id INTEGER NOT NULL,
            editor_admin_id INTEGER NOT NULL,
            assigned_at VARCHAR(32) DEFAULT (datetime('now')),
            unassigned_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcontrasg_article ON pciworld_contributor_assignments(article_id,id)");

        // ── The contributor↔editor thread, per manuscript.
        //
        //    This looks like the free-form messaging Phase 4 deliberately refused, so the
        //    difference is worth stating: employer↔applicant messaging is stranger-to-stranger
        //    through PCI's pipes, a moderation problem PCI would own with nobody in the loop. Every
        //    message here has an accountable staff member as one of its two parties, exists only
        //    per-manuscript, and is the alternative to the same conversation happening in
        //    unrecorded email — which it otherwise would.
        //
        //    Plain text, rendered escaped. There is no markup in messages, so there is no sanitiser
        //    to get wrong. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_contributor_messages(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            article_id INTEGER NOT NULL,
            sender_kind VARCHAR(16) NOT NULL,    -- contributor|editor
            sender_id INTEGER,
            body TEXT NOT NULL,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcontrmsg_article ON pciworld_contributor_messages(article_id,id)");
    }

    static void Upgrades(Db db)
    {
        // The contributor's identity on the manuscript. DISTINCT from author_id, which is the
        // editorial owner and a pciworld_admin_users id — conflating them is what would make the
        // maker-checker compare across namespaces and silently pass.
        AddCol(db, "pciworld_articles", "contributor_user_id", "contributor_user_id INTEGER");
        AddCol(db, "pciworld_articles", "contributor_terms_version", "contributor_terms_version VARCHAR(32)");
        AddCol(db, "pciworld_articles", "declarations_json", "declarations_json TEXT");

        // THE LINK THE MAKER-CHECKER NEEDS. Set deliberately by an owner and audited; NULL means
        // "this admin has not declared a World account", which is honest rather than assumed.
        AddCol(db, "pciworld_admin_users", "world_user_id", "world_user_id INTEGER");

        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldart_contrib ON pciworld_articles(contributor_user_id,status)");
    }

    static void Seed(Db db)
    {
        // Off by default; INSERT OR IGNORE so a later boot never overwrites an operator's choice.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('pciworld_contributors_enabled','0')");
        // Deliberately seeded EMPTY, not with a placeholder. terms_version must pin words the
        // applicant actually saw; recording acceptance of a placeholder would be recording
        // acceptance of nothing, so the application endpoint answers 503 while this is blank
        // (CCP-P6-019).
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('pciworld_contributor_terms_version','')");
    }

    static void AddCol(Db db, string table, string col, string ddl)
    {
        db.AddColumn(table, col, ddl);
    }
}
