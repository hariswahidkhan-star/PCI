namespace PCI.Backend.Data;

/// <summary>
/// PCI World community images — Phase 2 data model installer
/// (docs/pciworld/CCP_PHASE2_DESIGN.md §4 and §5).
///
/// Installed on every boot, on both providers, exactly like the Phase 1 tables — and, exactly like
/// them, INSTALLING THE SCHEMA IS NOT LAUNCHING THE FEATURE. Images stay off behind
/// <c>pciworld_community_images_enabled</c> (seeded '0') until CCP-P1-003 (legal/child-safety
/// prerequisites) and CCP-P1-004 (moderation provider and calibrated bands) are signed off by their
/// owners. Nothing in this repository can satisfy either, and nothing here pretends to.
///
/// THE PUBLICATION BOUNDARY IS EXPRESSED IN THE SCHEMA, NOT ONLY IN CODE.
///
/// The rule Phase 2 exists to keep is "no image byte reaches a second participant before the server
/// records an allow verdict" (§28.4's image restatement). Two columns carry that rule:
///
///   • <c>storage_ref</c> is the bytes as uploaded. Nothing serves them to a participant, ever.
///   • <c>derivative_ref</c> is the re-encoded, EXIF-stripped copy that IS served — and it is only
///     written when the state becomes 'allowed'.
///
/// So "served before the verdict" is not a code convention that a refactor could drop; there is
/// simply no renderable artefact to serve until the verdict exists. The same trick Phase 1 uses for
/// text ordering (a NULL sequence takes no slot) is used here for bytes.
///
/// AND FOR SUSPECTED ILLEGAL MATERIAL, THE CONTROL IS ABSENCE (§28.7).
///
/// A 'restricted' item never gets a derivative generated at all, and its original is moved out to
/// <c>pciworld_restricted_evidence</c> with <c>storage_ref</c> nulled on the media row. An ordinary
/// admin thumbnail cannot expose it because there is nothing for a thumbnail to resolve. Access is
/// two-person (requester != approver) and every attempt is audited.
///
/// This installer does NOT claim any classifier detects illegal material (§28.6). It provides the
/// container, the escalation path and the preservation guarantees so that a specialist arrangement
/// can be connected the day one exists.
/// </summary>
public static class CommunityMediaSchema
{
    public static void Ensure(Db db)
    {
        Tables(db);
        Seed(db);
    }

    static void Tables(Db db)
    {
        // Datetime columns are VARCHAR(32), never TEXT — same structural reason as Phase 1: TEXT
        // becomes LONGTEXT on MySQL and an index touching it blows InnoDB's 3072-byte key limit.
        // Several columns here are indexed (queue age, hold expiry), and bounding all of them means
        // adding an index later cannot resurrect that failure.

        // ── Media. One row per uploaded image, from the moment the bytes are accepted for
        //    inspection. The row exists BEFORE any verdict, which is the point: an upload that is
        //    never allowed still leaves a record a reviewer and an appellant can both reason about.
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_community_media(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            room_id INTEGER NOT NULL,
            guest_session_id INTEGER,
            world_user_id INTEGER,
            message_id INTEGER,
            client_upload_id VARCHAR(64) NOT NULL,
            state VARCHAR(16) NOT NULL DEFAULT 'pending',  -- pending|scanning|allowed|withheld|restricted|expired
            declared_mime VARCHAR(64),
            sniffed_mime VARCHAR(64),
            byte_size INTEGER NOT NULL DEFAULT 0,
            width INTEGER NOT NULL DEFAULT 0,
            height INTEGER NOT NULL DEFAULT 0,
            content_sha256 VARCHAR(64),
            perceptual_hash VARCHAR(64),
            alt_text TEXT,
            -- The two references the publication boundary is built on. See the class comment.
            storage_ref TEXT,
            derivative_ref TEXT,
            scan_provider VARCHAR(48),
            scan_provider_version VARCHAR(48),
            scan_band VARCHAR(16),                         -- clear|low|medium|high|restricted
            scan_confidence REAL,
            policy_version_id INTEGER,
            withheld_reason VARCHAR(48),
            verdict_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");

        // Upload idempotency. A retried POST, a double-tap or a reconnect must not create a second
        // media row — and must not cause a second scan to be paid for. Same reasoning as
        // ux_wcmsg_client on Phase 1 messages: the client's own key is what makes retry safe.
        // Written without a WHERE clause on purpose — Db.Translate strips predicates from partial
        // indexes for MySQL, so a partial index would leave the two providers agreeing only by
        // accident (the mistake already paid for once on ux_wcguest_name).
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcmedia_upload
                  ON pciworld_community_media(guest_session_id,client_upload_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcmedia_room ON pciworld_community_media(room_id,state)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcmedia_hash ON pciworld_community_media(content_sha256)");
        // The review queue reads state + age; without this it is a full scan of every image ever
        // uploaded once the room has any history.
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcmedia_queue ON pciworld_community_media(state,created_at)");

        // ── Scan attempts. APPEND-ONLY, and it records FAILURES as well as verdicts — the timeout
        //    and error rows are precisely the ones needed to explain why something was withheld.
        //    A re-scan writes a new row; it never edits the previous one, mirroring the append-only
        //    moderation-decision history Phase 1 established.
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_media_scans(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            media_id INTEGER NOT NULL,
            provider VARCHAR(48) NOT NULL,
            provider_version VARCHAR(48),
            requested_at VARCHAR(32),
            completed_at VARCHAR(32),
            outcome VARCHAR(16) NOT NULL,                  -- allowed|refused|restricted|error|timeout|unsupported
            band VARCHAR(16),
            confidence REAL,
            raw_labels_json TEXT,
            error_text TEXT,
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcscan_media ON pciworld_media_scans(media_id,id)");

        // ── Restricted evidence (§31). Suspected illegal material lives here and NOWHERE else:
        //    the media row's storage_ref is nulled when a row lands here, so no ordinary handler can
        //    resolve the bytes. There is deliberately no derivative column — nothing renderable is
        //    ever produced for these items, which is how §28.7 is enforced by absence rather than by
        //    a UI flag.
        //
        //    legal_hold and preserved_until are read by the retention purge, which must skip held
        //    rows. "Never delete production data" is enforced by that check and its test, not by a
        //    convention someone has to remember.
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_restricted_evidence(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            media_id INTEGER NOT NULL,
            case_id INTEGER,
            storage_ref TEXT,
            content_sha256 VARCHAR(64),
            reason VARCHAR(64) NOT NULL,
            preserved_until VARCHAR(32),
            legal_hold INTEGER NOT NULL DEFAULT 0,
            -- Two-person access: a request and a DISTINCT approval. Approver != requester is
            -- enforced server-side, the same maker-checker rule Phase 1 applies to sanctions.
            requested_by INTEGER,
            requested_at VARCHAR(32),
            approved_by INTEGER,
            approved_at VARCHAR(32),
            access_expires_at VARCHAR(32),
            accessed_count INTEGER NOT NULL DEFAULT 0,
            last_accessed_at VARCHAR(32),
            created_at VARCHAR(32) DEFAULT (datetime('now')),
            updated_at VARCHAR(32) DEFAULT (datetime('now')),
            version INTEGER NOT NULL DEFAULT 1)");
        // One evidence record per media item — a second escalation of the same item must reuse the
        // record (and its hold), not create a parallel one that a purge could miss.
        db.Exec(@"CREATE UNIQUE INDEX IF NOT EXISTS ux_wcevid_media
                  ON pciworld_restricted_evidence(media_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_wcevid_hold ON pciworld_restricted_evidence(legal_hold,preserved_until)");

        // ── Message linkage. Phase 1's message table gains a kind and a media reference rather
        //    than a parallel table, so ordering, idempotency and the sequence-is-publication rule
        //    are inherited unchanged. Guarded AddCol-style so existing databases converge.
        var msgCols = db.Columns("pciworld_community_messages");
        if (msgCols.Count > 0)
        {
            if (!msgCols.Contains("message_kind"))
                db.Exec("ALTER TABLE pciworld_community_messages ADD COLUMN message_kind VARCHAR(16) DEFAULT 'text'");
            if (!msgCols.Contains("media_id"))
                db.Exec("ALTER TABLE pciworld_community_messages ADD COLUMN media_id INTEGER");
        }
    }

    static void Seed(Db db)
    {
        // The image kill switch, seeded FALSE. INSERT OR IGNORE for the same reason as the Phase 1
        // flag: a deploy must never turn this on for an operator who has not satisfied the legal
        // prerequisites, and must never turn it off for one who has.
        //
        // This flag is NOT the legal gate. Turning it on is a decision a human makes after
        // CCP-P1-003 resolves; the code cannot assert the prerequisites are met, and the admin
        // screen says so rather than showing a green tick this system did not earn.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('pciworld_community_images_enabled','0')");
    }
}
