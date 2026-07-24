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

        // ── Privacy-aware analytics: event name + optional challenge/session ids only. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            event VARCHAR(48) NOT NULL,
            challenge_id INTEGER,
            session_id INTEGER,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldev_event ON pciworld_events(event)");
    }

    static void Seed(Db db)
    {
        // Operator flags and link targets — never hard-coded in pages.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_enabled','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_institute_url','https://projectcontrolsinstitute.org')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_simlab_url','/app/lab')");

        // Bootstrap the PCI World owner admin on first boot — separate credentials from every other
        // realm. Override the initial password via PCIWORLD_OWNER_PASSWORD; change it after first
        // sign-in (Settings). Same bootstrap posture as the platform's owner admin.
        if (db.Scalar<long>("SELECT COUNT(*) FROM pciworld_admin_users") == 0)
        {
            var pw = Environment.GetEnvironmentVariable("PCIWORLD_OWNER_PASSWORD");
            if (string.IsNullOrWhiteSpace(pw)) pw = "changeme-world-owner";
            db.Execute("INSERT INTO pciworld_admin_users(email,name,role,password_hash) VALUES(?,?,?,?)",
                "owner@pciworld.local", "PCI World Owner", "owner", BCrypt.Net.BCrypt.HashPassword(pw));
            Console.WriteLine("[seed] PCI World owner admin created: owner@pciworld.local — change the password after first sign-in");
        }
    }
}
