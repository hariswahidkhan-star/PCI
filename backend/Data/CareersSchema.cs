namespace PCI.Backend.Data;

/// <summary>
/// PCI World careers marketplace — Phase 4 data model installer
/// (docs/pciworld/CCP_PHASE4_DESIGN.md §10).
///
/// Installed on every boot on both providers, exactly like the community, media and forum
/// installers before it — and, exactly like them, INSTALLING THE SCHEMA IS NOT LAUNCHING THE
/// FEATURE. `pciworld_careers_enabled` seeds '0' and stays there until an operator turns it on.
///
/// TWO INVARIANTS LIVE HERE RATHER THAN IN APPLICATION CODE, because application code is where
/// races live:
///
///   1. <c>UNIQUE(posting_id, applicant_user_id)</c> on applications. One person applies to a
///      posting once. Without the constraint, a double-submit (or a deliberate resubmission after
///      an unfavourable answer) creates two applications, and an employer comparing candidates has
///      no way to know which one the person stands behind.
///   2. Answers are SNAPSHOTS. The question's prompt is COPIED into the answer row at submission,
///      so editing or deleting the question later cannot change what an application appears to
///      have said. §28 forbids modifying a submitted application snapshot in place — the same rule
///      the forum enforces for published posts, applied to the thing a hiring decision will be
///      justified by.
///
/// WHY EMPLOYERS ARE A STATE MACHINE AND NOT A FLAG. `verified` is an assertion PCI makes to
/// candidates about who is behind a posting, so the row records WHO made it and WHEN
/// (verified_by_admin_id, verified_at) — a bare boolean would be an unattributable claim.
///
/// MONEY IS INTEGER MINOR UNITS, NEVER FLOATING POINT. A REAL salary column rounds, and a rounded
/// salary is a wrong salary — the same rule the platform's Stripe integration follows. `currency`
/// travels beside the amounts because a number without its currency is not an amount at all.
/// </summary>
public static class CareersSchema
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
        // InnoDB's 3072-byte key limit. Several of these ARE indexed (public listing recency).

        // ── Employers. The tenant boundary of the whole marketplace: everything an employer-side
        //    user may see is scoped through a row here. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_employers(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            -- Globally unique because the employer page is a crawlable URL; a duplicate slug is a
            -- permanently ambiguous address, same reasoning as forum categories.
            slug VARCHAR(160) UNIQUE NOT NULL,
            name TEXT NOT NULL,
            website VARCHAR(255),
            state VARCHAR(24) NOT NULL DEFAULT 'draft',    -- draft|pending_verification|verified|suspended
            -- Verification is an attributable act, not a flag: WHO asserted it and WHEN survives
            -- the next edit, so 'verified' can be explained to a candidate who relied on it.
            verified_at VARCHAR(32),
            verified_by_admin_id INTEGER,
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcaremp_state ON pciworld_employers(state)");

        // ── Employer members: which world users may act FOR an employer. Acting rights are rows,
        //    not a password shared around an office. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_employer_members(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            employer_id INTEGER NOT NULL,
            user_id INTEGER NOT NULL,
            role VARCHAR(16) NOT NULL DEFAULT 'member',    -- owner|member
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        // One membership row per person per employer. Two rows with different roles would make
        // "may this person act for this employer, and as what" have two answers — a role change is
        // an UPDATE, not a second row.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcarmem_once
                  ON pciworld_employer_members(employer_id,user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarmem_user ON pciworld_employer_members(user_id)");

        // ── Job postings. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_job_postings(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            employer_id INTEGER NOT NULL,
            slug VARCHAR(160) NOT NULL,
            title TEXT NOT NULL,
            description TEXT,
            location TEXT,
            employment_type VARCHAR(24),                   -- full_time|part_time|contract|internship
            -- INTEGER minor units (pence/cents), NEVER floating point: a REAL here rounds, and a
            -- rounded salary is a wrong salary shown to a person deciding whether to apply.
            salary_min_minor INTEGER,
            salary_max_minor INTEGER,
            currency VARCHAR(8),
            state VARCHAR(16) NOT NULL DEFAULT 'draft',    -- draft|published|closed|withdrawn
            published_at VARCHAR(32),
            closes_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");
        // Unique per EMPLOYER rather than globally: two employers may each legitimately post a
        // 'senior-planner' role, and a global constraint would make the first to exist win for
        // ever. Same shape as forum thread slugs within a category.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcarpost_slug
                  ON pciworld_job_postings(employer_id,slug)");
        // The public board reads state + recency; without this it is a full scan of every posting
        // ever made once the board has any history.
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarpost_public ON pciworld_job_postings(state,published_at)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarpost_employer ON pciworld_job_postings(employer_id,state)");

        // ── Screening questions. Per-posting, ordered, and deliberately NOT the record of what an
        //    applicant was asked — that record is the snapshot on the answer row, because the
        //    employer can keep editing these right up to (and after) any given submission. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_job_questions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            posting_id INTEGER NOT NULL,
            sort INTEGER NOT NULL DEFAULT 100,
            kind VARCHAR(16) NOT NULL DEFAULT 'text',      -- text|boolean|choice
            prompt TEXT NOT NULL,
            required INTEGER NOT NULL DEFAULT 0,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarq_posting ON pciworld_job_questions(posting_id,sort)");

        // ── Applications. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_applications(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            posting_id INTEGER NOT NULL,
            applicant_user_id INTEGER NOT NULL,
            state VARCHAR(16) NOT NULL DEFAULT 'draft',    -- draft|submitted|withdrawn|shortlisted|rejected
            -- Frozen at submission; see the AddCol block below for why these live here and not on
            -- the applicant's profile. Fresh installs get them here, existing ones converge there.
            cv_ref VARCHAR(255),
            cv_sha256 VARCHAR(64),
            cv_name VARCHAR(255),
            cv_mime VARCHAR(96),
            submitted_at VARCHAR(32),
            withdrawn_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");
        // One application per person per posting — invariant 1. Written WITHOUT a WHERE clause on
        // purpose: Db.Translate strips predicates from partial indexes for MySQL, so a partial
        // index (say, WHERE state!='withdrawn') would leave the two providers agreeing only by
        // accident. Withdrawal is therefore a STATE on this one row, not permission for a second
        // row — reapplying reopens the record a person already owns.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcarapp_once
                  ON pciworld_applications(posting_id,applicant_user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarapp_user ON pciworld_applications(applicant_user_id,state)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarapp_posting ON pciworld_applications(posting_id,state)");

        // ── Answers — FROZEN at submission (invariant 2; §28). The prompt is COPIED here so the
        //    row is self-contained evidence: editing the question later cannot change what this
        //    applicant appears to have been asked, and deleting it cannot orphan what they said.
        //    question_id remains only as provenance, never as the text's source of truth. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_application_answers(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            application_id INTEGER NOT NULL,
            question_id INTEGER,
            prompt_snapshot TEXT NOT NULL,
            answer_snapshot TEXT,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarans_app ON pciworld_application_answers(application_id)");

        // ── Consents. Purpose-scoped per application per employer, and pinned to the policy text
        //    the person actually saw (policy_version) — consent to last year's policy is not
        //    consent to this year's. Withdrawal is a TIMESTAMP, not a delete: "was there consent
        //    on the day the employer read this?" must stay answerable after it is withdrawn. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_application_consents(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            application_id INTEGER NOT NULL,
            employer_id INTEGER NOT NULL,
            purpose VARCHAR(48) NOT NULL,
            granted_at VARCHAR(32),
            withdrawn_at VARCHAR(32),
            policy_version VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarcons_app ON pciworld_application_consents(application_id)");

        // ── The CV reference, FROZEN at submission (§5.2, §6). ──
        //
        // These live on the application rather than on the applicant's profile, and that placement
        // is the whole point: the applicant may replace their stored CV tomorrow, and what the
        // employer reviewed must not change underneath them. `cv_sha256` pins the exact bytes, so
        // "is this the document they consented to disclose" is answerable from the row rather than
        // from trust in the storage layer.
        //
        // There is NO public URL column and there never will be — the reference resolves only
        // through the authenticated download endpoint that re-checks all four conditions of §6 on
        // every request. A column holding a servable URL would be a way to bypass that check, so
        // the absence is structural, not an omission.
        AddCol(db, "pciworld_applications", "cv_ref", "cv_ref VARCHAR(255)");
        AddCol(db, "pciworld_applications", "cv_sha256", "cv_sha256 VARCHAR(64)");
        AddCol(db, "pciworld_applications", "cv_name", "cv_name VARCHAR(255)");
        AddCol(db, "pciworld_applications", "cv_mime", "cv_mime VARCHAR(96)");

        // ── Application events. Every state change after submission is a ROW, never an overwritten
        //    column (§5.2). Main-PCI still overwrites `admin_note` in place (Careers.cs:377); that
        //    is the mistake this table exists not to repeat — a note that replaces the previous note
        //    destroys the reason a candidate was moved, which is precisely what a dispute asks for.
        //    Append-only: there is no UPDATE path to this table anywhere in the codebase. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_application_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            application_id INTEGER NOT NULL,
            event VARCHAR(32) NOT NULL,          -- submitted|withdrawn|reinstated|shortlisted|rejected|note
            from_state VARCHAR(16),
            to_state VARCHAR(16),
            actor_kind VARCHAR(16) NOT NULL,     -- applicant|employer|admin|system
            actor_id INTEGER,
            note TEXT,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarev_app ON pciworld_application_events(application_id,id)");

        // ── CV access log. Written on EVERY download, employer or World admin (§6, §8).
        //
        // The stated purpose is to make bulk access a query rather than a forensic project: "one
        // account pulled 400 CVs in an hour" has to be answerable, because the realistic scraping
        // threat on this surface is a compromised member of a legitimate employer, not an outsider.
        // Refused attempts are logged too (`allowed=0`) — a run of refusals is the signal that
        // someone is probing ids, and logging only successes would hide exactly that. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_cv_access_log(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            application_id INTEGER NOT NULL,
            employer_id INTEGER,
            actor_kind VARCHAR(16) NOT NULL,     -- employer|admin
            actor_id INTEGER,
            allowed INTEGER NOT NULL DEFAULT 0,
            refused_reason VARCHAR(48),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarcv_actor ON pciworld_cv_access_log(actor_kind,actor_id,created_at)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcarcv_app ON pciworld_cv_access_log(application_id)");
    }

    // Add a column only if the table exists and lacks it, so an install created before these
    // columns converges on the next boot without a migration step anyone has to remember to run.
    // Mirrors Migrate.cs's AddCol; safe on both providers.
    static void AddCol(Db db, string table, string col, string ddl)
    {
        var have = db.Columns(table);
        if (have.Count > 0 && !have.Contains(col)) db.Exec($"ALTER TABLE {table} ADD COLUMN {ddl}");
    }

    static void Seed(Db db)
    {
        // Off by default, INSERT OR IGNORE so a later boot never overwrites an operator's choice —
        // the same reasoning as every other flag in this increment.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('pciworld_careers_enabled','0')");
    }
}
