namespace PCI.Backend.Data;

/// <summary>
/// Partner finance — the immutable commission ledger (Phase 1 of the Marketing Partner module).
/// Design and decisions: docs/finance/PARTNER_FINANCE_PHASE0_AUDIT.md.
///
/// Replaces the derived calculation (paid attributed revenue × the partner's CURRENT commission_pct)
/// with a transaction-level ledger. Attribution is unchanged and already idempotent — one payment
/// carries at most one code_redemption, which carries at most one partner — so a settled payment
/// produces exactly one commission transaction whose rate, basis and rule are SNAPSHOTTED at the
/// moment of settlement. Changing an agreement afterwards can never restate history.
///
/// Money is stored as INTEGER MINOR UNITS (cents) with an explicit currency, never REAL/DECIMAL:
/// DECIMAL is exact on MySQL but only NUMERIC *affinity* on SQLite (stored as float, not rounded),
/// and Microsoft.Data.Sqlite binds a C# decimal as TEXT, which would break SUM(). Minor units are
/// exact and identically summable on both providers and match Stripe's own representation.
///
/// Follows the MarketingSchema / SimLabSchema / TemplatesSchema pattern: a self-contained, idempotent
/// Ensure(db) written in the SQLite dialect that Db.Translate auto-converts for MySQL. Therefore:
/// every keyed/indexed string column is VARCHAR(n) (MySQL cannot index TEXT without a prefix length),
/// there are no foreign keys (the platform enforces relationships in code; an inline REFERENCES would
/// be enforced on MySQL but only advisory on SQLite), and no unique index carries a WHERE clause
/// (Db.Translate strips it, which would silently widen the constraint).
/// </summary>
public static class FinanceSchema
{
    public static void Ensure(Db db)
    {
        // ---- agreements: effective-dated commercial terms, one partner may have a history ----
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_agreements(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            partner_id INTEGER NOT NULL,
            agreement_number VARCHAR(40) NOT NULL,
            effective_from VARCHAR(10) NOT NULL,               -- YYYY-MM-DD
            effective_to VARCHAR(10),                          -- NULL = open-ended
            currency VARCHAR(3) NOT NULL DEFAULT 'USD',
            payment_terms_days INTEGER NOT NULL DEFAULT 30,
            minimum_payout_minor INTEGER NOT NULL DEFAULT 0,   -- cents
            refund_hold_days INTEGER NOT NULL DEFAULT 30,
            tax_treatment VARCHAR(32),
            status VARCHAR(16) NOT NULL DEFAULT 'draft',       -- draft|active|superseded|terminated
            note TEXT,
            created_by INTEGER, approved_by INTEGER, approved_at TEXT,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_partner_agreement_no ON partner_agreements(agreement_number)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_partner_agreements_partner ON partner_agreements(partner_id)");

        // ---- commission rules: effective-dated + prioritised. A NULL dimension means "any". ----
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_commission_rules(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            agreement_id INTEGER NOT NULL,
            partner_id INTEGER NOT NULL,
            certification_id INTEGER,                          -- NULL = any certification
            route_key VARCHAR(40),                             -- NULL = any route
            product_type VARCHAR(32),                          -- NULL = any product (exam|membership|bundle|…)
            country VARCHAR(64),                               -- NULL = any country
            commission_type VARCHAR(12) NOT NULL DEFAULT 'percentage',   -- percentage|fixed
            commission_rate_bp INTEGER NOT NULL DEFAULT 0,     -- percentage: basis points (750 = 7.50%)
            commission_fixed_minor INTEGER NOT NULL DEFAULT 0, -- fixed: cents per qualifying payment
            commission_basis VARCHAR(24) NOT NULL DEFAULT 'net_after_discount',
            effective_from VARCHAR(10), effective_to VARCHAR(10),
            priority INTEGER NOT NULL DEFAULT 100,             -- lower wins; ties broken by specificity, then id
            active INTEGER NOT NULL DEFAULT 1,
            created_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pcr_partner ON partner_commission_rules(partner_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pcr_agreement ON partner_commission_rules(agreement_id)");

        // ---- the ledger. Value columns are NEVER updated after insert; corrections are new rows. ----
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_commission_transactions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            txn_ref VARCHAR(32),                               -- human reference, PCT-000123
            -- Idempotency carrier. 'pay:{paymentId}:p:{partnerId}' for an original, 'rev:{sourceId}:{cause}'
            -- for a reversal, 'adj:{key}' for a manual adjustment. A UNIQUE index over one NOT NULL column
            -- behaves identically on both providers (a composite key containing NULLs would not).
            dedupe_key VARCHAR(120) NOT NULL,
            partner_id INTEGER NOT NULL,
            agreement_id INTEGER, commission_rule_id INTEGER,
            discount_code_id INTEGER, code_redemption_id INTEGER,
            payment_id INTEGER,
            user_id INTEGER,                                   -- internal only; never returned to a partner
            certification_id INTEGER, route_key VARCHAR(40), product_type VARCHAR(32),
            currency VARCHAR(3) NOT NULL DEFAULT 'USD',
            gross_minor INTEGER NOT NULL DEFAULT 0,
            discount_minor INTEGER NOT NULL DEFAULT 0,
            eligible_net_minor INTEGER NOT NULL DEFAULT 0,
            commission_type VARCHAR(12),                       -- SNAPSHOT of the rule at settlement
            commission_rate_bp INTEGER,                        -- SNAPSHOT
            commission_basis VARCHAR(24),                      -- SNAPSHOT
            commission_minor INTEGER NOT NULL DEFAULT 0,       -- negative for reversals
            status VARCHAR(28) NOT NULL DEFAULT 'payment_received',
            earned_at TEXT, due_at TEXT, hold_until TEXT,
            approved_at TEXT, approved_by INTEGER,
            reversal_of_transaction_id INTEGER,
            reason TEXT,
            requires_finance_review INTEGER NOT NULL DEFAULT 0, -- set by backfill when the historic rate is unproven
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_pct_dedupe ON partner_commission_transactions(dedupe_key)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pct_partner ON partner_commission_transactions(partner_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pct_payment ON partner_commission_transactions(payment_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pct_status ON partner_commission_transactions(status)");

        // ---- append-only state history (typed actor: solves the audit_logs identity collision locally) ----
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_commission_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            transaction_id INTEGER NOT NULL,
            from_status VARCHAR(28), to_status VARCHAR(28) NOT NULL,
            actor_type VARCHAR(16) NOT NULL DEFAULT 'system',  -- admin|partner|system
            actor_id INTEGER,
            reason TEXT, reference VARCHAR(64),
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pce_txn ON partner_commission_events(transaction_id)");

        // ---- settlement batches (maker-checker) and their allocations ----
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_settlements(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            settlement_no VARCHAR(32) NOT NULL,
            partner_id INTEGER NOT NULL,
            period_start VARCHAR(10), period_end VARCHAR(10),
            currency VARCHAR(3) NOT NULL DEFAULT 'USD',
            opening_balance_minor INTEGER NOT NULL DEFAULT 0,
            eligible_commission_minor INTEGER NOT NULL DEFAULT 0,
            adjustments_minor INTEGER NOT NULL DEFAULT 0,
            amount_approved_minor INTEGER NOT NULL DEFAULT 0,
            amount_paid_minor INTEGER NOT NULL DEFAULT 0,
            closing_balance_minor INTEGER NOT NULL DEFAULT 0,
            status VARCHAR(20) NOT NULL DEFAULT 'draft',       -- draft|pending_approval|approved|scheduled|paid|cancelled
            prepared_by INTEGER, reviewed_by INTEGER, approved_by INTEGER,
            scheduled_date VARCHAR(10), paid_at TEXT,
            payment_method VARCHAR(32), payment_reference VARCHAR(120),
            proof_storage_ref VARCHAR(255),
            internal_note TEXT, partner_note TEXT,
            legacy_payout_id INTEGER,                          -- set when imported from partner_payouts
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_settlement_no ON partner_settlements(settlement_no)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_settlements_partner ON partner_settlements(partner_id)");

        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_settlement_items(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            settlement_id INTEGER NOT NULL,
            transaction_id INTEGER NOT NULL,
            amount_allocated_minor INTEGER NOT NULL DEFAULT 0,
            created_at TEXT DEFAULT (datetime('now')))");
        // A transaction can never be allocated twice within one settlement; across settlements the
        // SUM(allocated) <= commission_minor check in the settlement service prevents double payment.
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_psi_pair ON partner_settlement_items(settlement_id, transaction_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_psi_txn ON partner_settlement_items(transaction_id)");

        // ---- disputes: a partner's formal challenge to a figure, with a threaded, append-only history ----
        // A dispute never edits the ledger. It records the challenge and its outcome; any correction is
        // made as a NEW adjustment transaction, so the original figure and the reason it changed both
        // survive. Either a transaction or a settlement may be disputed (or neither — a general query).
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_disputes(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            dispute_no VARCHAR(32) NOT NULL,
            partner_id INTEGER NOT NULL,
            transaction_id INTEGER, settlement_id INTEGER,
            category VARCHAR(32) NOT NULL DEFAULT 'other',     -- missing_commission|wrong_rate|wrong_amount|not_paid|other
            subject VARCHAR(200) NOT NULL,
            detail TEXT,
            claimed_amount_minor INTEGER NOT NULL DEFAULT 0,
            currency VARCHAR(3) NOT NULL DEFAULT 'USD',
            status VARCHAR(20) NOT NULL DEFAULT 'open',        -- open|under_review|resolved|rejected|withdrawn
            resolution TEXT,
            adjustment_transaction_id INTEGER,                 -- set when the outcome created an adjustment
            raised_by_partner_user_id INTEGER,
            assigned_to INTEGER, resolved_by INTEGER, resolved_at TEXT,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_dispute_no ON partner_disputes(dispute_no)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_disputes_partner ON partner_disputes(partner_id, status)");

        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_dispute_messages(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            dispute_id INTEGER NOT NULL,
            author_type VARCHAR(12) NOT NULL,                  -- partner|admin|system
            author_id INTEGER,
            body TEXT NOT NULL,
            internal INTEGER NOT NULL DEFAULT 0,               -- 1 = admin-only note, never shown to the partner
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_dispute_msgs ON partner_dispute_messages(dispute_id, id)");

        // ---- campaign links: a shareable, tracked URL that carries one of the partner's codes ----
        // The link is the top of the funnel; the CODE it carries is what ties the funnel to money, through
        // the same code_redemptions chain the commission ledger already uses. Nothing is inferred.
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_campaign_links(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            token VARCHAR(24) NOT NULL,                        -- the /r/{token} slug; opaque and unguessable
            partner_id INTEGER NOT NULL,
            code_id INTEGER,                                   -- NULL = an untracked-to-money link (clicks only)
            name VARCHAR(120) NOT NULL,
            destination VARCHAR(255),                          -- site-relative path only; never an absolute URL
            utm_source VARCHAR(80), utm_medium VARCHAR(80), utm_campaign VARCHAR(120),
            active INTEGER NOT NULL DEFAULT 1,
            click_count INTEGER NOT NULL DEFAULT 0,
            created_by_partner_user INTEGER,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_pcl_token ON partner_campaign_links(token)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pcl_partner ON partner_campaign_links(partner_id)");

        // One row per click. 'visitor' is the platform's existing rotating, non-reversible daily hash, so
        // it de-duplicates traffic WITHIN a day and cannot identify anyone across days — by design.
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_link_clicks(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            link_id INTEGER NOT NULL,
            visitor VARCHAR(32), country VARCHAR(80), device VARCHAR(24), browser VARCHAR(32),
            referrer VARCHAR(255),
            -- Bounded, not TEXT: it is the second column of the index below, and MySQL rejects a
            -- COMPOSITE key containing an unbounded text column (error 1071) even though it happily
            -- prefixes a lone one. The failure is swallowed by the installer, so the only symptom
            -- would be this index quietly not existing on the production provider — on the
            -- highest-volume table in the module. An ISO-8601 stamp never exceeds 32 characters.
            created_at VARCHAR(32) DEFAULT (datetime('now')))");
        // CREATE TABLE IF NOT EXISTS cannot alter a table an earlier build already created.
        if (db.Provider == Db.Kind.MySql &&
            db.Scalar<long>(@"SELECT COUNT(*) FROM information_schema.columns
                              WHERE table_schema=DATABASE() AND table_name='partner_link_clicks'
                                AND column_name='created_at' AND data_type='text'") > 0)
            db.Exec("ALTER TABLE partner_link_clicks MODIFY created_at VARCHAR(32)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_plc_link ON partner_link_clicks(link_id, created_at)");
    }
}
