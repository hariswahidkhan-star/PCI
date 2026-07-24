namespace PCI.Backend.Data;

/// <summary>
/// PCI World — data model installer (Phase 0 foundation; see docs/pciworld/).
///
/// PCI World is a distinct product: a free, global project-challenge platform operated by the
/// Project Controls Institute. It is a SEPARATE REALM inside this codebase — every table is
/// pciworld_-prefixed, participants are anonymous sessions (never the platform's `users`), and
/// administration uses its own pciworld_admin_* identity, wholly separate from admin_users (the
/// partner-portal precedent). No PCI World table references exams, entitlements, credentials or
/// membership, and no PCI World code path reads them: challenge play can never touch formal
/// certification records.
///
/// Same installer pattern as MarketingSchema/SimLabSchema: idempotent Ensure(db) in the SQLite
/// dialect that Db.cs translates to MySQL 8 (the production provider), run on every boot on both
/// providers so migration-parity gates hold.
/// </summary>
public static class WorldSchema
{
    public static void Ensure(Db db)
    {
        Tables(db);
        Seed(db);
        WorldContentPack.Seed(db);
        WorldArticlePack.Seed(db);
    }

    static void Tables(Db db)
    {
        // ── Challenge authoring rows. `status` is the WORKING-COPY lifecycle (draft → in_review →
        //    approved → published); serving is decided by current_version>=1 AND retired=0, so a
        //    revision in progress never takes the last published version off the air. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_challenges(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            code VARCHAR(64) UNIQUE NOT NULL,
            title TEXT NOT NULL,
            hook TEXT,
            industry VARCHAR(64),
            role TEXT,
            track VARCHAR(32) DEFAULT 'project_controls',
            difficulty VARCHAR(16) DEFAULT 'foundation',   -- foundation|developing|professional|advanced|expert
            est_minutes INTEGER DEFAULT 8,
            competencies_json TEXT,
            synthetic_declared INTEGER DEFAULT 0,
            config_json TEXT,                              -- working copy; NEVER the replay authority
            status VARCHAR(16) NOT NULL DEFAULT 'draft',   -- draft|in_review|approved|published
            retired INTEGER DEFAULT 0,
            current_version INTEGER DEFAULT 0,             -- 0 = never published
            author_id INTEGER,                             -- pciworld_admin_users.id; NULL = house content
            reviewed_by INTEGER,
            approved_by INTEGER,
            review_note TEXT,
            published_at TEXT,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldch_status ON pciworld_challenges(status)");
        // Servability — `current_version>=1 AND retired=0` — is the predicate on the archive, the
        // rotation eligibility query and every admin count. Unindexed it was a full table scan on
        // each one, which is survivable at 30 challenges and is not at 10,000.
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldch_servable ON pciworld_challenges(retired, current_version)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldch_facets ON pciworld_challenges(industry, difficulty, track)");

        // ── Immutable published snapshots — the ONLY replay/serving authority for attempts.
        //    Written once at publish; never updated, never deleted (retire hides the challenge
        //    from rotation without touching history). ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_challenge_versions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            challenge_id INTEGER NOT NULL,
            version INTEGER NOT NULL,
            title TEXT NOT NULL,
            hook TEXT,
            industry VARCHAR(64),
            role TEXT,
            track VARCHAR(32),
            difficulty VARCHAR(16),
            est_minutes INTEGER,
            competencies_json TEXT,
            config_json TEXT NOT NULL,
            published_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_worldver ON pciworld_challenge_versions(challenge_id, version)");

        // ── Daily calendar override; without a row the day's challenge is the deterministic
        //    day-of-year rotation over servable challenges. Days are UTC (shown on the page). ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_calendar(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            day_utc VARCHAR(10) UNIQUE NOT NULL,           -- YYYY-MM-DD
            challenge_id INTEGER NOT NULL,
            note TEXT,
            created_at TEXT DEFAULT (datetime('now')))");

        // ── Daily rotation ledger (Core/WorldRotation.cs). The featured challenge used to be a
        //    stateless `DayOfYear % count` recomputed per request: it moved mid-day whenever the
        //    catalogue changed, left no record of what had been featured, and could never reach more
        //    than ~366 challenges. These three tables replace that with an append-only ledger.
        //
        //    periods: one row per rotation day. UNIQUE(day_key, revision) is what makes the boundary
        //    idempotent — a second run of the same day is an ignored insert, not a re-pick. A
        //    substitution appends revision+1 and stamps superseded_at on the row it replaces, so the
        //    record of what was displaced survives. `version` records the snapshot that was live
        //    when the day opened; attempts still pin their own version independently. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_rotation_periods(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            day_key VARCHAR(10) NOT NULL,                  -- YYYY-MM-DD in the configured rotation timezone
            revision INTEGER NOT NULL DEFAULT 1,
            challenge_id INTEGER NOT NULL,
            version INTEGER NOT NULL,
            cycle_no INTEGER NOT NULL DEFAULT 1,
            seq_no INTEGER NOT NULL DEFAULT 0,
            source VARCHAR(16) NOT NULL DEFAULT 'auto',    -- auto | calendar | substitution
            reason TEXT,
            superseded_at TEXT,
            created_by INTEGER,                            -- pciworld_admin_users.id for substitutions
            opened_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_worldrotper ON pciworld_rotation_periods(day_key, revision)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldrotper_ch ON pciworld_rotation_periods(challenge_id)");

        // ── The materialised running order for a cycle: a deterministic seeded shuffle computed
        //    ONCE per cycle. Never recompute an order over a live query — that is how a rotation
        //    silently reorders itself when the catalogue changes. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_rotation_order(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            cycle_no INTEGER NOT NULL,
            seq_no INTEGER NOT NULL,
            challenge_id INTEGER NOT NULL,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_worldrotord ON pciworld_rotation_order(cycle_no, seq_no)");

        // ── What the scheduler did on each wake, INCLUDING when it did nothing and why. Without
        //    this an operator cannot distinguish "healthy and idle" from "silently broken". ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_rotation_runs(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            day_key VARCHAR(10),
            outcome VARCHAR(32) NOT NULL,                  -- created|catch_up|skipped_exists|skipped_locked|paused|no_content|calendar_ineligible|catch_up_truncated|error
            periods_created INTEGER DEFAULT 0,
            detail TEXT,
            owner VARCHAR(64),
            duration_ms INTEGER,
            ran_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldrotrun_at ON pciworld_rotation_runs(ran_at)");

        // ── Single-row advisory lock for the boundary job, claimed through WorkerLease exactly like
        //    the platform's dispatchers. Render runs multiple instances; without this, two of them
        //    open the same day and the cycle double-advances. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_rotation_lock(
            id INTEGER PRIMARY KEY,
            status VARCHAR(16) NOT NULL DEFAULT 'queued',
            lease_owner VARCHAR(64),
            lease_until TEXT,
            updated_at TEXT DEFAULT (datetime('now')))");

        // ── Anonymous participant sessions: an opaque 128-bit token, stored only as SHA-256.
        //    No email, no name, no IP — continuity only. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_sessions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            token_sha VARCHAR(64) UNIQUE NOT NULL,
            created_at TEXT DEFAULT (datetime('now')),
            last_seen_at TEXT DEFAULT (datetime('now')))");

        // ── Attempts pin (challenge_id, version) at start and replay only from the snapshot.
        //    result_token_sha is the opaque public verification token (nullable until shared,
        //    revocable); display_name is participant-chosen and optional. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_attempts(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            session_id INTEGER NOT NULL,
            challenge_id INTEGER NOT NULL,
            version INTEGER NOT NULL,
            status VARCHAR(16) NOT NULL DEFAULT 'in_progress',  -- in_progress|completed
            answers_json TEXT,
            score REAL,
            dimensions_json TEXT,
            profile_key TEXT,
            display_name TEXT,
            result_token_sha VARCHAR(64),
            result_revoked INTEGER DEFAULT 0,
            invite_id INTEGER,
            started_at TEXT DEFAULT (datetime('now')),
            completed_at TEXT,
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldatt_session ON pciworld_attempts(session_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldatt_challenge ON pciworld_attempts(challenge_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldatt_token ON pciworld_attempts(result_token_sha)");

        // ── Challenge-a-friend invitations: opaque revocable token → same challenge AND version
        //    as the inviter's attempt. Never exposes the inviter's answers. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_invites(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            attempt_id INTEGER NOT NULL,
            token_sha VARCHAR(64) UNIQUE NOT NULL,
            inviter_name TEXT,                             -- NULL = anonymous invitation
            revoked INTEGER DEFAULT 0,
            created_at TEXT DEFAULT (datetime('now')))");

        // ── Participant accounts (Phase 1b). Wholly separate from the platform's `users` — a PCI
        //    World account is practice identity only and can never reach exam or credential data.
        //    The passport token is the opaque, revocable public-profile URL; publication is
        //    consent-based and requires a verified email + a chosen display name. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_users(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            email VARCHAR(190) UNIQUE NOT NULL,
            password_hash TEXT NOT NULL,
            display_name TEXT,
            status VARCHAR(16) NOT NULL DEFAULT 'active',
            email_verified INTEGER DEFAULT 0,
            passport_public INTEGER DEFAULT 0,
            passport_token_sha VARCHAR(64),
            passport_show_scores INTEGER DEFAULT 1,
            passport_show_profiles INTEGER DEFAULT 1,
            passport_show_dates INTEGER DEFAULT 1,
            passport_expires_at TEXT,
            failed_logins INTEGER DEFAULT 0,
            lockout_until TEXT,
            last_login_at TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_user_sessions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            token VARCHAR(64) NOT NULL,
            expires_at TEXT NOT NULL,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldusess_token ON pciworld_user_sessions(token)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_user_tokens(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            purpose VARCHAR(16) NOT NULL,                  -- verify | reset
            token_sha VARCHAR(64) UNIQUE NOT NULL,
            expires_at TEXT NOT NULL,
            created_at TEXT DEFAULT (datetime('now')))");

        // Additive upgrade columns for installs created before Phase 1b (fresh installs get them
        // from CREATE TABLE below/above; both providers share this code path).
        void AddCol(string table, string col, string ddl)
        {
            var have = db.Columns(table);
            if (have.Count > 0 && !have.Contains(col)) db.Exec($"ALTER TABLE {table} ADD COLUMN {ddl}");
        }
        AddCol("pciworld_attempts", "user_id", "user_id INTEGER");
        AddCol("pciworld_attempts", "passport_visible", "passport_visible INTEGER DEFAULT 0");

        // Passport disclosure is per FIELD as well as per item: publishing evidence of what you
        // have practised should not force you to publish your scores. Defaults preserve the
        // behaviour of every Passport published before these columns existed.
        AddCol("pciworld_users", "passport_show_scores", "passport_show_scores INTEGER DEFAULT 1");
        AddCol("pciworld_users", "passport_show_profiles", "passport_show_profiles INTEGER DEFAULT 1");
        AddCol("pciworld_users", "passport_show_dates", "passport_show_dates INTEGER DEFAULT 1");
        // A public link that never expires is a decision nobody consciously made. NULL = no expiry
        // (the existing behaviour); a date makes the link stop resolving on its own.
        AddCol("pciworld_users", "passport_expires_at", "passport_expires_at TEXT");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldatt_user ON pciworld_attempts(user_id)");

        // ── Separate PCI World admin realm (partner-portal precedent: wholly separate from
        //    admin_users and students). Roles: owner|author|reviewer|publisher|viewer. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_admin_users(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            email VARCHAR(190) UNIQUE NOT NULL,
            name TEXT,
            role VARCHAR(16) NOT NULL DEFAULT 'viewer',
            password_hash TEXT NOT NULL,
            status VARCHAR(16) NOT NULL DEFAULT 'active',
            failed_logins INTEGER DEFAULT 0,
            lockout_until TEXT,
            last_login_at TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_admin_sessions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            admin_id INTEGER NOT NULL,
            token VARCHAR(64) NOT NULL,
            expires_at TEXT NOT NULL,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldadm_token ON pciworld_admin_sessions(token)");

        // ── Append-only admin audit. Never UPDATE/DELETE. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_audit(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            admin_id INTEGER,
            action VARCHAR(64) NOT NULL,
            detail TEXT,
            created_at TEXT DEFAULT (datetime('now')))");

        // ── Content reports (§ content correction/reporting): anyone — anonymous included — can
        //    flag a challenge. No PII is required or stored; the optional session link exists only
        //    so abuse can be rate-limited and mass reports deduplicated. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_reports(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            challenge_id INTEGER,
            category VARCHAR(24) NOT NULL,                 -- content_error | calculation | accessibility | inappropriate | other
            message TEXT NOT NULL,
            session_id INTEGER,
            status VARCHAR(16) NOT NULL DEFAULT 'open',    -- open | resolved
            resolution TEXT,
            resolved_by INTEGER,
            resolved_at TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldreports_status ON pciworld_reports(status)");

        // ── Editorial platform (Core/WorldEditorial.cs). One CMS serves the blog and the newsroom:
        //    they differ in OBLIGATIONS (a news item must trace every material claim to a saved
        //    source) rather than in machinery. Published text is versioned and corrections are
        //    appended visibly — there is deliberately no code path that edits a published article
        //    silently. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_articles(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(160) UNIQUE NOT NULL,
            kind VARCHAR(8) NOT NULL DEFAULT 'blog',       -- blog | news
            title TEXT NOT NULL,
            dek TEXT,                                      -- standfirst: used by every listing and search result
            body_md TEXT,                                  -- authored Markdown subset; rendered server-side after escaping
            author_name TEXT,                              -- a real named person, or 'PCI World Editorial'. Never invented.
            tags_json TEXT,
            seo_title TEXT,
            seo_desc TEXT,
            corrections_json TEXT,                         -- appended, dated, public. Never rewritten.
            status VARCHAR(20) NOT NULL DEFAULT 'idea',    -- idea|drafting|technical_review|fact_check|legal_review|seo_review|approved|published|archived
            current_version INTEGER DEFAULT 0,
            author_id INTEGER,                             -- pciworld_admin_users.id; NULL = house content
            approved_by INTEGER,
            review_note TEXT,
            published_at TEXT,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldart_kind ON pciworld_articles(kind, status, published_at)");

        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_article_versions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            article_id INTEGER NOT NULL,
            version INTEGER NOT NULL,
            title TEXT NOT NULL,
            dek TEXT,
            body_md TEXT,
            author_name TEXT,
            seo_title TEXT,
            seo_desc TEXT,
            tags_json TEXT,
            published_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_worldartver ON pciworld_article_versions(article_id, version)");

        // ── Source registry. A source is a record of something we actually read: where it was, who
        //    published it, when, and when we retrieved it. Model memory is never a source. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_sources(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            url TEXT NOT NULL,
            publisher TEXT,
            title TEXT,
            published_at TEXT,                             -- the source's own publication date
            retrieved_at TEXT DEFAULT (datetime('now')),
            tier VARCHAR(24),                              -- official|regulator|company|exchange|multilateral|journalism
            note TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldsrc_url ON pciworld_sources(url)");

        // Each link records WHICH claim the source supports — a bibliography proves nothing.
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_article_sources(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            article_id INTEGER NOT NULL,
            source_id INTEGER NOT NULL,
            claim TEXT,
            confidence TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldartsrc ON pciworld_article_sources(article_id)");

        // ── Entity registry + mentions. Naming a company is a legal exposure, so a mention is a
        //    tracked object that forces a legal review before approval. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_entities(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            legal_name TEXT NOT NULL,
            trademark_spelling TEXT,
            aliases_json TEXT,
            risk_note TEXT,
            logo_permission INTEGER DEFAULT 0,             -- 0 = no logo may be used. The default is deliberate.
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_entity_mentions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            article_id INTEGER NOT NULL,
            entity_id INTEGER NOT NULL,
            context TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldentmention ON pciworld_entity_mentions(article_id)");

        // ── Review evidence: append-only, one row per review performed. Not a status field, because
        //    'who checked this, when, and what did they conclude' has to survive the next edit. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_article_reviews(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            article_id INTEGER NOT NULL,
            kind VARCHAR(16) NOT NULL,                     -- technical | fact_check | legal | seo
            reviewer_id INTEGER,
            outcome VARCHAR(8) NOT NULL,                   -- pass | fail
            note TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldartrev ON pciworld_article_reviews(article_id, kind)");

        // ── Privacy-aware analytics: event name + optional challenge/session ids only. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            event VARCHAR(48) NOT NULL,
            challenge_id INTEGER,
            session_id INTEGER,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldev_event ON pciworld_events(event)");
        // Highest-volume table in the realm (a row per page view) — the retention sweep and every
        // time-bounded analytics query need a date index or they scan the whole history.
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldev_at ON pciworld_events(created_at)");

        // ── Remaining hot-path indexes identified by the Phase 0 scale audit. Each backs a query
        //    that runs on a public request or an admin page load. ──
        // Public Passport URL lookup — a per-request key that was entirely unindexed.
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldusers_passport ON pciworld_users(passport_token_sha)");
        // Attempt resume: filters session_id + challenge_id + version + status together.
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldatt_resume ON pciworld_attempts(session_id, challenge_id, version, status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldatt_completed ON pciworld_attempts(status, completed_at)");
        // Admin queues, ordered and paginated by recency.
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldaudit_at ON pciworld_audit(created_at)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldreports_ch ON pciworld_reports(challenge_id, status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldinv_attempt ON pciworld_invites(attempt_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldsess_seen ON pciworld_sessions(last_seen_at)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldusess_exp ON pciworld_user_sessions(expires_at)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldutok_user ON pciworld_user_tokens(user_id, purpose)");
    }

    static void Seed(Db db)
    {
        // Operator flags and link targets — never hard-coded in pages.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_enabled','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_institute_url','https://projectcontrolsinstitute.org')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_simlab_url','/app/lab')");

        // Rotation controls — operator-owned, never hard-coded. `world_rotation_timezone` accepts an
        // IANA id (Europe/London) or a fixed offset (+04:00); `world_rotation_shuffle` off means the
        // bank plays in catalogue order; `world_rotation_flag_threshold` is the number of OPEN content
        // reports at which a challenge stops being eligible to be featured.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_rotation_enabled','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_rotation_timezone','UTC')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_rotation_shuffle','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_rotation_flag_threshold','3')");

        // Bootstrap the PCI World owner admin on first boot — separate credentials from every other
        // realm. Override the initial password via PCIWORLD_OWNER_PASSWORD; change it after first
        // sign-in (Settings). Same bootstrap posture as the platform's owner admin.
        if (db.Scalar<long>("SELECT COUNT(*) FROM pciworld_admin_users") == 0)
        {
            var pw = Environment.GetEnvironmentVariable("PCIWORLD_OWNER_PASSWORD");
            var generated = false;
            if (string.IsNullOrWhiteSpace(pw))
            {
                // A published default password is a published credential: the world admin holds
                // publication rights over every challenge, so a production install must never boot
                // with one an attacker can read in this repository. When the operator has not set
                // PCIWORLD_OWNER_PASSWORD, mint a random one and print it once — the operator reads
                // it from the deploy log and changes it at first sign-in. Development and the E2E
                // harness keep the well-known default so the suites stay deterministic.
                if (IsProductionPosture()) { pw = Core.Security.RandomHex(12); generated = true; }
                else pw = "changeme-world-owner";
            }
            db.Execute("INSERT INTO pciworld_admin_users(email,name,role,password_hash) VALUES(?,?,?,?)",
                "owner@pciworld.local", "PCI World Owner", "owner", BCrypt.Net.BCrypt.HashPassword(pw));
            Console.WriteLine(generated
                ? $"[seed] PCI World owner admin created: owner@pciworld.local — ONE-TIME PASSWORD: {pw}\n" +
                  "[seed] Sign in at /world-admin and change it now. This password is not shown again."
                : "[seed] PCI World owner admin created: owner@pciworld.local — change the password after first sign-in");
        }
    }

    /// <summary>True when this process is configured as a real deployment rather than a developer
    /// machine or test run. Mirrors the boot config validator's definition.</summary>
    internal static bool IsProductionPosture() =>
        string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Production", StringComparison.OrdinalIgnoreCase)
        || string.Equals(Environment.GetEnvironmentVariable("APP_ENV"), "production", StringComparison.OrdinalIgnoreCase);

    /// <summary>True when the bootstrap owner still holds the well-known development password.
    /// Surfaced as a boot warning so an install can never quietly sit on a published credential.</summary>
    public static bool OwnerHasDefaultPassword(Db db)
    {
        try
        {
            var a = db.QueryOne("SELECT password_hash FROM pciworld_admin_users WHERE email='owner@pciworld.local'");
            var hash = a?["password_hash"] as string;
            return hash is not null && BCrypt.Net.BCrypt.Verify("changeme-world-owner", hash);
        }
        catch { return false; }
    }
}
