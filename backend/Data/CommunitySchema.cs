namespace PCI.Backend.Data;

/// <summary>
/// PCI World community rooms — data model installer (docs/pciworld/CCP_PHASE1_DESIGN.md §2).
///
/// Same installer shape as <see cref="WorldSchema"/>: an idempotent Ensure(db) authored in the
/// SQLite dialect that Db.cs translates to MySQL, run on every boot on both providers so the
/// migration-parity gates cover these tables automatically.
///
/// The specification names these tables <c>world_*</c>; they are installed as <c>pciworld_*</c> to
/// match the thirty tables already in this product (decision D-004 in CCP_DECISION_LOG.md). The
/// mapping is 1:1 and mechanical.
///
/// TWO INVARIANTS ARE ENFORCED HERE RATHER THAN IN APPLICATION CODE, because application code is
/// where races live:
///
///   1. <c>UNIQUE(room_id, sequence)</c> on messages. Ordered reconnect recovery is only sound if a
///      sequence identifies exactly one message. Two concurrent accepts that both computed the same
///      next sequence must have one of them fail, loudly, rather than quietly interleave.
///   2. <c>UNIQUE(room_id, client_message_id)</c> on messages. A double-click, a retried request or
///      a hub reconnect must not produce two messages. The client's own idempotency key is what
///      makes the accept path safe to retry.
///
/// A third invariant — one active display name per room — is a PARTIAL unique index, which Db.cs
/// rewrites for MySQL (which has no partial indexes) as documented in MYSQL.md. It is partial
/// because a name must become reusable once its session ends.
///
/// NOTHING HERE IS ENABLED BY DEFAULT. The tables install on every boot so migrations stay
/// idempotent and testable, but the feature reads <c>world_community_enabled</c>, seeded false: see
/// CCP-P1-003 (legal prerequisites) and CCP-P1-004 (moderation provider) in the issue register.
/// Installing the schema is not launching the feature.
/// </summary>
public static class CommunitySchema
{
    public static void Ensure(Db db)
    {
        Tables(db);
        Seed(db);
        // Phase 2 media tables. Installed here (after the message table exists, which its message
        // linkage upgrade depends on) so that both installers are covered by the same
        // migration-parity gates. Also off by default — see CommunityMediaSchema.
        CommunityMediaSchema.Ensure(db);
    }

    static void Tables(Db db)
    {
        // NOTE ON DATETIME COLUMNS: every stored datetime here is VARCHAR(32), never TEXT.
        // These are 19-character 'YYYY-MM-DD HH:MM:SS' UTC strings, so 32 is generous — but
        // the reason is structural, not stylistic. TEXT becomes LONGTEXT on MySQL, and an
        // index touching a LONGTEXT needs a key prefix that exceeds InnoDB's 3072-byte
        // limit, so the whole schema install fails with 'Specified key was too long'.
        // Several of these columns ARE indexed (queue age, decision history, report age).
        // Bounding all of them means adding an index later cannot resurrect that failure.
        // Same rule as pciworld_attempts.completed_at in WorldSchema.
        // ── Rooms. `state` is the operating lifecycle; `emergency_state` is a SEPARATE axis so a
        //    kill switch never destroys the room's real schedule — an operator who locks a room
        //    during an incident must get the same room back afterwards, not a room whose scheduled
        //    open/close was overwritten. `next_sequence` is the per-room monotonic counter that
        //    message ordering is allocated from (see CommunityRooms.AllocateSequence). ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_community_rooms(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(120) UNIQUE NOT NULL,
            title TEXT NOT NULL,
            description TEXT,
            topic TEXT,
            category VARCHAR(60),
            locale VARCHAR(16) DEFAULT 'en',
            room_type VARCHAR(24) NOT NULL DEFAULT 'community',   -- community|peer_support|official_support
            state VARCHAR(16) NOT NULL DEFAULT 'draft',           -- draft|scheduled|open|slow_mode|read_only|closed|archived
            emergency_state VARCHAR(24),                          -- NULL|degraded|locked|globally_disabled
            capacity INTEGER NOT NULL DEFAULT 200,
            guest_allowed INTEGER NOT NULL DEFAULT 1,
            member_allowed INTEGER NOT NULL DEFAULT 1,
            text_allowed INTEGER NOT NULL DEFAULT 1,
            -- Phase 2 (images) is not built. The column exists so enabling it later is a settings
            -- change rather than a migration; every write path rejects it while the phase is ungated.
            image_allowed INTEGER NOT NULL DEFAULT 0,
            slow_mode_seconds INTEGER NOT NULL DEFAULT 0,
            message_cooldown_ms INTEGER NOT NULL DEFAULT 750,
            max_messages_per_session INTEGER NOT NULL DEFAULT 200,
            opens_at VARCHAR(32),
            closes_at VARCHAR(32),
            timezone VARCHAR(64) DEFAULT 'UTC',
            pinned_welcome TEXT,
            learning_prompt TEXT,
            resources_json TEXT,
            retention_class VARCHAR(32) NOT NULL DEFAULT 'standard',
            discoverable INTEGER NOT NULL DEFAULT 1,
            rules_version VARCHAR(32) NOT NULL DEFAULT 'v1',
            policy_version_id INTEGER,
            next_sequence INTEGER NOT NULL DEFAULT 1,
            version INTEGER NOT NULL DEFAULT 1,                   -- optimistic concurrency (§13.6)
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcroom_state ON pciworld_community_rooms(state,discoverable)");

        // ── Guest sessions. The token is stored ONLY as SHA-256, like every other opaque token in
        //    this product. `risk_key` is a rotating peppered hash of network signals — never a raw
        //    IP, which is never stored and never logged (§19.2). Guest-ban evasion by clearing a
        //    cookie or changing proxy remains possible; this is a deterrent, not enforcement, and
        //    §28 forbids describing it otherwise. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_guest_sessions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            token_sha VARCHAR(64) UNIQUE NOT NULL,
            room_id INTEGER NOT NULL,
            display_name TEXT NOT NULL,
            display_name_folded VARCHAR(64) NOT NULL,
            -- The folded name WHILE ACTIVE, NULL once the session ends. See the index note below:
            -- this column exists so one-active-name-per-room is expressible as a plain unique
            -- index that behaves identically on SQLite and MySQL.
            active_name_key VARCHAR(64),
            locale VARCHAR(16) DEFAULT 'en',
            rules_version_accepted VARCHAR(32) NOT NULL,
            risk_key VARCHAR(64),
            status VARCHAR(16) NOT NULL DEFAULT 'active',   -- active|left|expired|ejected
            message_count INTEGER NOT NULL DEFAULT 0,
            last_message_at VARCHAR(32),
            last_seen_sequence INTEGER NOT NULL DEFAULT 0,
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            expires_at VARCHAR(32),
            ended_at VARCHAR(32),
            ended_reason VARCHAR(48))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcguest_room ON pciworld_guest_sessions(room_id,status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcguest_risk ON pciworld_guest_sessions(risk_key)");
        // One ACTIVE display name per room, and the name frees up when the session ends — a guest
        // who leaves must not permanently reserve a common first name.
        //
        // This is deliberately NOT written as `... (room_id,display_name_folded) WHERE
        // status='active'`. Db.Translate STRIPS the WHERE clause from a partial unique index for
        // MySQL (Data/Db.cs:199), on the reasoning that MySQL already exempts NULLs from UNIQUE.
        // That reasoning holds for a `WHERE col IS NOT NULL` partial index, but not for a value
        // predicate like `status='active'`: on MySQL the filter would simply vanish, making the
        // constraint span ENDED sessions too, so the first "Ali" to leave a room would reserve that
        // name in it forever — while SQLite behaved correctly. A silent provider divergence, and
        // the kind that only shows up in production.
        //
        // Instead the predicate is carried in the DATA: active_name_key holds the folded name while
        // the session is active and is nulled when it ends. Both providers exempt NULLs from a
        // unique index, so one plain index gives identical behaviour on each.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcguest_name
                  ON pciworld_guest_sessions(room_id,active_name_key)");

        // ── Messages. A row is inserted `pending` and moves to its terminal status IN THE SAME
        //    TRANSACTION as its moderation decision, so there is no window in which a message
        //    exists without a verdict. Only `allowed` ever receives a published_at and an outbox
        //    event — blocked and quarantined rows are retained for review and never broadcast. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_community_messages(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            room_id INTEGER NOT NULL,
            sequence INTEGER,                              -- NULL until allowed: unpublished text takes no slot
            guest_session_id INTEGER,
            world_user_id INTEGER,
            client_message_id VARCHAR(64) NOT NULL,
            body TEXT NOT NULL,
            body_normalized TEXT,
            reply_to_message_id INTEGER,
            status VARCHAR(16) NOT NULL DEFAULT 'pending', -- pending|allowed|blocked|quarantined
            decision_id INTEGER,
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            published_at VARCHAR(32))");
        // The two ordering/idempotency invariants.
        //
        // The sequence index relies on both providers exempting NULLs from UNIQUE, which is what
        // lets many un-published messages coexist while allocated sequences stay unique. It is
        // written WITHOUT a WHERE clause on purpose: Db.Translate would strip one for MySQL
        // (Data/Db.cs:199) and the two providers would then be agreeing only by accident. Stating
        // the constraint the same way for both means what is tested is what runs.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcmsg_sequence
                  ON pciworld_community_messages(room_id,sequence)");
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcmsg_client
                  ON pciworld_community_messages(room_id,client_message_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcmsg_room ON pciworld_community_messages(room_id,status,sequence)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcmsg_session ON pciworld_community_messages(guest_session_id)");

        // ── Moderation decisions: APPEND-ONLY, and deliberately CONTENT-FREE. Only a hash of the
        //    content plus the metadata §8.7 lists. Blocked and quarantined text belongs in the
        //    restricted evidence store, never in an audit row or telemetry. An overturned appeal
        //    writes a NEW decision; it never rewrites history. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_moderation_decisions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            scope VARCHAR(24) NOT NULL,                    -- message|display_name|report
            subject_ref VARCHAR(64),
            content_hash VARCHAR(64),
            policy_version_id INTEGER,
            rule_id INTEGER,
            provider VARCHAR(48),
            provider_version VARCHAR(48),
            category VARCHAR(48),
            severity VARCHAR(16),
            confidence_band VARCHAR(16),                   -- low|medium|high (calibrated per D-008)
            context_rule VARCHAR(48),
            repetition_count INTEGER NOT NULL DEFAULT 0,
            outcome VARCHAR(16) NOT NULL,                  -- allow|block|quarantine|eject|escalate
            reason_code VARCHAR(48) NOT NULL,
            correlation_id VARCHAR(64),
            -- VARCHAR(32), not TEXT: this column is INDEXED below, and TEXT becomes
            -- LONGTEXT on MySQL, whose key prefix exceeds InnoDB's 3072-byte limit.
            -- Same bounded-datetime rule as pciworld_attempts.completed_at.
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcdecision_subject ON pciworld_moderation_decisions(scope,subject_ref)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcdecision_outcome ON pciworld_moderation_decisions(outcome,created_at)");

        // ── Cases and their append-only event trail. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_moderation_cases(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            room_id INTEGER,
            subject_kind VARCHAR(24) NOT NULL,             -- guest_session|world_user|message
            subject_ref VARCHAR(64) NOT NULL,
            severity VARCHAR(16) NOT NULL DEFAULT 'normal',
            status VARCHAR(24) NOT NULL DEFAULT 'open',    -- open|in_review|actioned|dismissed|escalated
            restricted INTEGER NOT NULL DEFAULT 0,         -- 1 = legal/safety escalation, out of the ordinary queue
            assigned_to INTEGER,
            reason_code VARCHAR(48),
            correlation_id VARCHAR(64),
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            closed_at VARCHAR(32))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wccase_queue ON pciworld_moderation_cases(status,severity,created_at)");

        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_moderation_case_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            case_id INTEGER NOT NULL,
            actor_id INTEGER,
            actor_kind VARCHAR(24) NOT NULL DEFAULT 'system',   -- system|world_admin
            event VARCHAR(48) NOT NULL,
            detail TEXT,
            previous_state VARCHAR(24),
            new_state VARCHAR(24),
            correlation_id VARCHAR(64),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wccaseev_case ON pciworld_moderation_case_events(case_id,id)");

        // ── Reports. UNIQUE(message, reporter) stops one participant inflating a report count by
        //    reporting the same message repeatedly (PW-US-021). Coordinated mass-reporting is a
        //    separate problem handled by risk scoring, not by this constraint. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_community_reports(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            room_id INTEGER NOT NULL,
            message_id INTEGER,
            reported_session_id INTEGER,
            reporter_session_id INTEGER,
            reporter_user_id INTEGER,
            reason_code VARCHAR(48) NOT NULL,
            note TEXT,
            status VARCHAR(24) NOT NULL DEFAULT 'open',
            case_id INTEGER,
            -- VARCHAR(32), not TEXT: this column is INDEXED below, and TEXT becomes
            -- LONGTEXT on MySQL, whose key prefix exceeds InnoDB's 3072-byte limit.
            -- Same bounded-datetime rule as pciworld_attempts.completed_at.
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        // Same reasoning as the message indexes: no WHERE clause, relying on NULL-exemption, which
        // both providers implement identically. A report with no message_id (a participant-level
        // report) carries a NULL and so never collides.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcreport_once
                  ON pciworld_community_reports(message_id,reporter_session_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcreport_status ON pciworld_community_reports(status,created_at)");

        // ── Sanctions. Every row carries actor, reason code, scope, policy version and correlation
        //    id (§8.6). `approved_by` is the maker-checker slot: permanent sanctions require a
        //    second authorised reviewer, enforced by the endpoint, recorded here. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_sanctions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            subject_kind VARCHAR(24) NOT NULL,             -- guest_session|risk_key|world_user
            subject_ref VARCHAR(64) NOT NULL,
            room_id INTEGER,
            sanction_type VARCHAR(32) NOT NULL,            -- warning|slow_mode|mute|eject|restrict|permanent
            reason_code VARCHAR(48) NOT NULL,
            policy_version_id INTEGER,
            scope VARCHAR(24) NOT NULL DEFAULT 'room',     -- room|world
            issued_by INTEGER,
            issued_by_kind VARCHAR(24) NOT NULL DEFAULT 'system',
            approved_by INTEGER,
            starts_at VARCHAR(32) DEFAULT (datetime('now')),
            expires_at VARCHAR(32),
            revoked_at VARCHAR(32),
            revoked_by INTEGER,
            case_id INTEGER,
            correlation_id VARCHAR(64),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcsanction_subject ON pciworld_sanctions(subject_kind,subject_ref,expires_at)");

        // ── Appeals. The PUBLIC reference is safe to quote in support mail and reveals nothing on
        //    its own — no status, no content, no personal data. The CREDENTIAL is a separate
        //    high-entropy value shown once and stored only as a hash, scoped to this case,
        //    expiring, rotatable, rate-limited by `attempts` (§8.6). A guest can appeal without an
        //    account, which is the entire point. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_appeals(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            case_id INTEGER NOT NULL,
            public_reference VARCHAR(32) UNIQUE NOT NULL,
            credential_sha VARCHAR(64) NOT NULL,
            credential_expires_at VARCHAR(32),
            attempts INTEGER NOT NULL DEFAULT 0,
            status VARCHAR(24) NOT NULL DEFAULT 'issued',  -- issued|submitted|under_review|upheld|overturned|expired
            submission TEXT,
            outcome TEXT,
            reviewed_by INTEGER,
            reviewed_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcappeal_case ON pciworld_appeals(case_id)");

        // ── Policy as DATA, not as scattered conditionals (§8.4.1). A decision records which rule
        //    row produced it, so "why was this blocked?" is answerable months later against the
        //    exact version that was live at the time. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_policy_versions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            version_label VARCHAR(48) UNIQUE NOT NULL,
            status VARCHAR(24) NOT NULL DEFAULT 'draft',   -- draft|staged|active|retired
            notes TEXT,
            created_by INTEGER,
            approved_by INTEGER,
            activated_at VARCHAR(32),
            retired_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");

        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_policy_rules(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            policy_version_id INTEGER NOT NULL,
            content_type VARCHAR(24) NOT NULL DEFAULT 'text',
            category VARCHAR(48) NOT NULL,
            severity VARCHAR(16) NOT NULL DEFAULT 'medium',
            confidence_band VARCHAR(16) NOT NULL,          -- low|medium|high
            context_rule VARCHAR(48),                      -- NULL|educational|quoted|news
            repetition_min INTEGER NOT NULL DEFAULT 0,
            outcome VARCHAR(16) NOT NULL,                  -- allow|block|quarantine|eject|escalate
            reason_code VARCHAR(48) NOT NULL,
            sort INTEGER NOT NULL DEFAULT 100)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcrule_lookup ON pciworld_policy_rules(policy_version_id,content_type,category,confidence_band)");

        // ── Risk restrictions: the time-bounded consequence of an ejection, keyed on the rotating
        //    risk identifier rather than on anything durable about a person. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_risk_restrictions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            risk_key VARCHAR(64) NOT NULL,
            scope VARCHAR(24) NOT NULL DEFAULT 'room',
            room_id INTEGER,
            reason_code VARCHAR(48) NOT NULL,
            case_id INTEGER,
            expires_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcrisk_key ON pciworld_risk_restrictions(risk_key,expires_at)");

        // ── The durable outbox. Column shape mirrors comm_outbox so Core/WorkerLease.cs claims
        //    rows unchanged. Broadcast happens FROM HERE, after the accept transaction commits —
        //    never inside it (§28.18) — which is what lets a reconnect replay the gap from MySQL
        //    after a dropped broadcast, a crashed worker or a node restart. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_community_outbox(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            event_type VARCHAR(48) NOT NULL,
            room_id INTEGER,
            payload_json TEXT NOT NULL,
            status VARCHAR(16) NOT NULL DEFAULT 'queued',  -- queued|processing|sent|failed
            attempts INTEGER NOT NULL DEFAULT 0,
            next_attempt_at VARCHAR(32),
            lease_owner VARCHAR(64),
            lease_until VARCHAR(32),
            last_error TEXT,
            correlation_id VARCHAR(64),
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            -- Required by WorkerLease: both TryClaim and RecoverExpired write updated_at on every
            -- claim, so it is part of the contract for any table they manage, not bookkeeping we
            -- chose. Omitting it made every claim fail with 'no such column'.
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            sent_at VARCHAR(32))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcoutbox_due ON pciworld_community_outbox(status,next_attempt_at)");
        // Additive upgrade: the table shipped once without updated_at, so an install created from
        // that revision needs the column added rather than only fresh installs getting it.
        var outboxCols = db.Columns("pciworld_community_outbox");
        if (outboxCols.Count > 0 && !outboxCols.Contains("updated_at"))
            db.Exec("ALTER TABLE pciworld_community_outbox ADD COLUMN updated_at VARCHAR(32)");
    }

    static void Seed(Db db)
    {
        // The feature flag. Seeded FALSE, and INSERT OR IGNORE so a later boot never overwrites an
        // operator's choice: someone who turned rooms on must not have a deploy silently turn them
        // off, and someone who has not satisfied the legal prerequisites (CCP-P1-003) must not have
        // a deploy turn them on. Same skey/svalue shape and '0'/'1' convention as world_enabled.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('world_community_enabled','0')");
    }
}
