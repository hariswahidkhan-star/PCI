# Marketing Partner finance module — Phase 0: audit, gap map and design

Status: **design only — no production code changed in this phase.**
Repository state audited: `main` @ `3ea3ece` (+ open PR #121).

---

## 1. What was inspected

Read directly (not from prior documentation):

| Area | Files |
| --- | --- |
| Money path | `Endpoints/Payments.cs` (checkout, webhook, refund/dispute), `Core/Provisioning.cs` (`Settlement.Grant`, `EnsureDownstream`, `Reverse`), `Endpoints/Public.cs` (`Pricing`, `ValidateCode`) |
| Partner module | `Endpoints/PartnerPortal.cs`, `Endpoints/Partners.cs` (commission ledger + payouts), `Endpoints/TrainingPartners.cs`, `wwwroot/partner.html` |
| Schema | `schema.sql`, `schema.mysql.sql`, `Data/Migrate.cs`, `Data/Db.cs` (`Translate`, `Bind`) |
| Authorization / audit | `Program.cs` (`GateFn`), `Core/Security.cs` (`Rbac`), `Core/Auth.cs` (`PartnerFromReq`) |
| Tests | `tests/integration_test.py`, `tests/migration_integrity_test.py`, `PCI.Backend.Tests/PricingDecisionTests.cs`, `SettlementTests.cs`, `e2e/partner-portal.spec.ts`, `e2e/portal-billing-founding.spec.ts`, `e2e/admin-operations.spec.ts` |

---

## 2. Corrections to the brief's premises

These are stated up front because they change what should be built.

**2.1 SQLite is not being "introduced" — it is the existing default provider, and
production already refuses to run on it.**
`Db.cs:31` selects the provider from **`DB_PROVIDER`** (default `sqlite`), and
the production preflight at `Program.cs:321-323` **errors out unless
`DB_PROVIDER=mysql`**. So "MySQL is mandatory in production" is already enforced
in code. SQLite remains the local-dev and unit/CI default: the `backend` CI job,
the whole `backend-unit` xUnit job, and ~20 python suites run on it.
Removing it would break dev onboarding and most of the CI matrix for zero
business benefit.

**Adopted interpretation:** MySQL stays authoritative for production; every new
finance object is verified on MySQL in CI; no new SQLite-only construct is
introduced; the SQLite dev/test path keeps working. Flagged for confirmation (§11, D1).

**2.2 Money storage is already DECIMAL on MySQL; the defect is the arithmetic.**
`schema.mysql.sql` declares `DECIMAL(12,2)` for `payments.final_amount`,
`discount_codes.discount_value`, `code_redemptions.amount_before/discount_amount`
etc. Two real problems sit on top:
- every column added later through `Migrate.AddCol` is written in SQLite dialect
  and uses `REAL` — which becomes **DOUBLE** on MySQL (e.g.
  `payments.waived_amount`, `payments.amount_refunded`, `discount_codes.min_payable`,
  `partner_payouts.amount`);
- **all C# money arithmetic is `double`** (`H.D() => Convert.ToDouble`,
  `Public.Pricing`, `CommissionLedger`, `Math.Round(double)`, Stripe
  `AmountRefunded / 100.0`).

So "use decimal" is an application-layer and AddCol-convention fix, not a base-schema rewrite.

**2.3 Prices are not client-trusted today.**
`/api/create-checkout-session` validates the code server-side
(`Public.ValidateCode`), computes the price server-side (`Public.Pricing`),
persists a `pricing_snapshot` on the enrollment session, and *writes the Stripe
metadata itself* from that computation. The webhook then reads back that
server-authored metadata. The residual weakness is narrower than the brief
assumes: the webhook prefers `metadata.final_amount` over Stripe's own
`session.AmountTotal` (`Payments.cs:256`) **without asserting they agree**.
Fix = assert-and-reconcile, not a redesign.

**2.4 Partner discount codes are ~70% complete, and unit-tested.**
`discount_codes` already carries `certification_id`, `route_key`,
`min_transaction`, `max_discount`, `min_payable`, `eligible_countries`,
`per_user_limit`, `campaign_name`, and a real
`draft → pending_approval → active → suspended|rejected|cancelled` lifecycle
with an admin approval queue. `Public.ValidateCode` enforces certification
scope, dates, usage caps, per-email limits, partner status, partner agreement
end and partner total allocation — with ~30 xUnit facts in
`PricingDecisionTests.cs` pinning that behaviour, including four partner cases.

The genuine Phase-4 gaps are narrow (§4, P1-F/G/H) — mostly that the *partner-facing*
create endpoint doesn't expose the scope fields that validation already honours.
**Do not rebuild this.**

**2.5 Multi-currency is not real.** Stripe is hard-coded to `usd`
(`Payments.cs:98`) and every money column defaults `'USD'`. The ledger should
*carry* currency and *reject* mismatches; building an FX/exchange-rate engine
would be speculative.

**2.6 Server-side PDF already exists.** `PDFsharp 6.1.1` is a dependency with
`Core/PdfWatermark.cs` and `Core/CertIssue.cs` in production use — statements
and remittance advice reuse this, not a new library.

---

## 3. How commission works today (the thing being replaced)

`Partners.cs:35-63`, one function, computed on every request:

```
attributed = SUM(payments.final_amount)
             WHERE code_redemptions → discount_codes.partner_id = P
               AND payments.payment_status = 'paid'
accrued    = round(attributed × training_partners.commission_pct / 100, 2)
paid_out   = SUM(partner_payouts.amount)
balance    = accrued − paid_out
```

Attribution itself is sound and idempotent: `code_redemptions.payment_id` is
`UNIQUE` and the row is written inside the webhook's atomic transaction with
`INSERT OR IGNORE` (`Payments.cs:348`), so **one payment → at most one
redemption → at most one partner**. That gives the new ledger a clean natural key.

Everything downstream of attribution is the problem.

---

## 4. Gap map

Severity: **P0** = financially unsafe, **P1** = control/governance gap, **P2** = missing capability.

| # | Finding | Evidence | Impact | Phase |
| --- | --- | --- | --- | --- |
| **P0-A** | Commission is recomputed from the partner's *current* `commission_pct` over all history. Changing the rate silently restates every past period. | `Partners.cs:37,50` | Historic restatement; no auditable basis | 1 |
| **P0-B** | Payout accepts any positive amount — **no check against available balance**. Overpayment is possible today. | `Partners.cs:196-200` (only `amount <= 0` rejected) | Cash loss | 3 |
| **P0-C** | Payouts are gated on `partners` (partner-directory permission), not `finance`, and have **no approval step** — one person records money out. | `Partners.cs:191`; cf. `finance` gate used elsewhere `AdminOps.cs:47-231` | Segregation-of-duties failure | 3 |
| **P0-D** | Refunds are invisible to the ledger. `attributed` counts only `payment_status='paid'`, so a refund makes revenue *vanish* rather than create a reversal; `partially_refunded` is excluded **entirely** (full value lost, not the refunded part). If a payout already occurred, `balance` silently goes negative with no recoverable record. | `Partners.cs:41-49`; refund statuses set at `Payments.cs:431,443` | Unrecoverable overpayment, no audit trail | 2 |
| **P0-E** | All money arithmetic is binary floating point. | `H.D()`, `Public.Pricing:17-45`, `Partners.cs:50` | Cent drift, non-reproducible totals | 1 |
| **P1-F** | Partner code **cancel does not block after use** — `used_count` is selected then ignored. | `PartnerPortal.cs:204-207` | Partner can cancel a code students already redeemed | 4 |
| **P1-G** | Invalid discount percentage is **silently clamped**, not rejected (`Math.Clamp(…,1,100)`). | `PartnerPortal.cs:159` | Silent divergence from intent | 4 |
| **P1-H** | No `start_date <= end_date` validation on partner code creation. | `PartnerPortal.cs:190-194` | Un-redeemable/absurd windows | 4 |
| **P1-I** | Partner roles (`admin/finance/reporting/support`) are enforced at exactly **two** call sites. `reporting` and `support` can read the full money ledger and cancel codes. | `PartnerPortal.cs:155`, `Partners.cs:95` only | Least-privilege failure inside the tenant | 3 |
| **P1-J** | Partner-user actions are audited with `user_id = NULL`; actor identity only in free-text. Known open defect DEF-14 (`audit_logs.user_id` collides between identity spaces). | `PartnerPortal.cs:75,197,208`; `docs/testing/DEFECT_REGISTER.md` DEF-14 | Non-attributable financial audit | 1 |
| **P1-K** | Webhook trusts `metadata.final_amount` over Stripe's `AmountTotal` without asserting equality. | `Payments.cs:256` | Recorded ≠ charged, undetected | 2 |
| **P1-L** | Existing maker-checker precedents **fail open when the author id is NULL/0**. | `AdminSimLab.cs:156-161`; `Core/WorldLifecycle.cs:71` | Must not be copied verbatim for money | 3 |
| **P2-M** | No statements, no remittance advice, no disputes. | — | Brief §10 | 3 |
| **P2-N** | No campaign links / funnel attribution (clicks → registration → checkout → paid). | — | Brief §8 | 5 |
| **P2-O** | Hard `LIMIT 500` on the ledger payment list; no server-side filtering or pagination. | `Partners.cs:44` | Brief §9 | 5 |
| **P2-P** | Partner code create endpoint cannot set `certification_id`, `route_key`, `min_transaction`, `max_discount`, `eligible_countries` — columns validation already honours. | `PartnerPortal.cs:190-194` vs `Public.cs:66-115` | Brief §7 | 4 |
| **P0-Q** | `schema.mysql.sql` is machine-generated by `tools/sqlite_to_mysql.py` **but has been hand-tuned afterwards**: every money column was manually changed `DOUBLE → DECIMAL(12,2)`, plus index prefixes and VARCHAR widths. Re-running the generator — as `MYSQL.md:33` instructs — **silently reverts every money column to `DOUBLE`**. 122 code-only diff lines between committed and regenerated. | `schema.mysql.sql:43,51,75-76,88-89,388,760,1111` vs `tools/sqlite_to_mysql.py:33-94` | A routine "regenerate the schema" turns exact money into binary float platform-wide | 1 |
| **P1-R** | `CREATE INDEX IF NOT EXISTS` (234 occurrences) is **MariaDB-only syntax**; MySQL 8 does not support it. CI tests `mariadb:10.11` only, while `docs/OPERATIONS.md:25` and `render.yaml` advertise "MySQL 8.x / MariaDB 10.11+". | `.github/workflows/build.yml:128` | The migration mechanism would not boot on real MySQL 8 | 1 (decision D9) |
| **P1-S** | *Suspected, unverified:* `certification_routes` declares `route_key TEXT` and then builds a **unique index on it** (`MultiCert.cs:518-535`). The table is in neither schema file, so on MySQL the raw DDL runs and should raise err 1170 (TEXT in key without prefix length) — swallowed by `MultiCert.Seed`'s catch, leaving the table unindexed and unseeded on MySQL. Could not be executed here (no MySQL in sandbox). | `MultiCert.cs:23-27,518-535` | Exactly the failure mode new finance tables must avoid | verify in Phase 1 |
| **P2-T** | The parity gate compares **column names only** — types, indexes, unique constraints and FKs are explicitly out of scope. A money-type regression (P0-Q) is invisible to CI. | `tests/migration_integrity_test.py:20-24,351-368` | No CI protection for the ledger's most important property | 1 |

---

## 5. Target architecture

**Principle: attribution stays where it is; valuation becomes immutable.**

```
payment settles (webhook, atomic)
        │
        ├── code_redemptions row            ← EXISTS, idempotent, unique per payment
        │
        └── NEW: partner_commission_transactions row
                 · rate/basis/rule SNAPSHOTTED at settlement
                 · UNIQUE(payment_id) → replay-safe
                 · status machine, never mutated in place for value
                          │
      refund/chargeback ──┴── NEW: reversal transaction (linked, negative)
                          │
        Finance approves ─┴── NEW: partner_settlements + _items (maker-checker)
                                   · allocation ≤ approved balance
                                   · one transaction never paid twice
                                          │
                                          └── statement (PDF/CSV via PDFsharp)
```

Four new tables, one new permission, no change to how a payment or a redemption
is recorded.

---

## 6. Schema proposal

### 6.1 Where the tables live, and the dual-provider rules

**Declare the finance tables in a new `Data/FinanceSchema.cs`** (following the
existing `MarketingSchema` / `SimLabSchema` / `TemplatesSchema` pattern, wired in
`Program.cs` beside lines 53-57) — and **do not add them to `schema.sql`**.

That is the established convention (141 of the 168 C#-created tables, including
`partner_payouts` and `fee_waivers`, are in neither schema file) and here it is
also a safety requirement: adding to `schema.sql` invites a regeneration of
`schema.mysql.sql`, which would wipe the hand-applied DECIMAL money types (P0-Q).

Consequence: the DDL is written **once, in SQLite dialect**, and must survive
`Db.Translate` (`Db.cs:114-153`) unaided.

| Rule | Why |
| --- | --- |
| PK written **exactly** `id INTEGER PRIMARY KEY AUTOINCREMENT` | `Db.cs:146` matches that literal string; any variant loses auto-increment on MySQL. |
| Every column that is UNIQUE / PK / named in a `CREATE INDEX` → `VARCHAR(n)`, **never `TEXT`** | `Translate` has no key-prefix logic; MySQL raises err 1170. This is the P1-S failure mode. Precedent: `partner_users.email VARCHAR(255)`, `partner_sessions.token VARCHAR(64)`. |
| Money → **integer minor units** in `BIGINT` columns (see 6.2) | Exact and identically summable on *both* providers. `DECIMAL(12,2)` is exact on MySQL but only NUMERIC **affinity** on SQLite (stored as REAL, not rounded), and `Microsoft.Data.Sqlite` binds a C# `decimal` as **TEXT**, breaking `SUM`. Decision D8. |
| `INTEGER` auto-rewritten to `BIGINT` | `Translate` regex `\bINTEGER\b` — keep writing `INTEGER`. |
| Timestamps → `TEXT DEFAULT (datetime('now'))` | 116 existing uses; translated to a `UTC_TIMESTAMP()` expression. Never `DATETIME`/`TIMESTAMP` — the app parses `YYYY-MM-DD HH:MM:SS` strings everywhere. |
| **No foreign keys** | Zero `REFERENCES` exist in any C# DDL and the generator strips them deliberately. Inline FKs would be *enforced* on MySQL but only advisory on SQLite — an asymmetric insert-order failure. Integrity is enforced in code, as the rest of the platform does. |
| **No partial unique indexes** | `Db.cs:149` **strips the `WHERE`** on MySQL. A `WHERE status='active'` predicate silently becomes a global unique constraint. Use full keys over stable columns. |
| **No `ON CONFLICT … DO UPDATE`** | Only the one hard-coded `skey/svalue` shape is translated (`Db.cs:141`); anything else is a MySQL syntax error. Use `INSERT OR IGNORE` + `UPDATE`. |
| `AddCol` only *after* the `CREATE TABLE`, same method | `AddCol` silently no-ops when the table does not exist yet (`have.Count > 0`, `Migrate.cs:23-34`). |
| Avoid reserved words as column names | `Db.Translate` backticks nothing (only the generator does, and only for two names). |

### 6.2 Tables

**Money representation.** All amounts are **integer minor units** (cents) in
`INTEGER` columns (→ `BIGINT` on MySQL), with an explicit `currency`. Rates are
**basis points** (`750` = 7.50%). This is exact and identically summable on both
providers, avoids the SQLite `DECIMAL`-affinity and `decimal`-as-TEXT traps
(§6.1), and matches the payment source of truth — Stripe already reports
`AmountTotal` in minor units, which today is divided by `100.0` into a `double`
(`Payments.cs:246`). Conversion to a display decimal happens once, at the API
boundary. See decision **D8** if PCI prefers literal `DECIMAL(12,2)` instead.

```sql
-- Effective-dated agreements. One partner may have a history of agreements.
CREATE TABLE IF NOT EXISTS partner_agreements(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  partner_id INTEGER NOT NULL,
  agreement_number VARCHAR(40) NOT NULL,
  effective_from VARCHAR(10) NOT NULL,          -- YYYY-MM-DD
  effective_to   VARCHAR(10),                   -- NULL = open-ended
  currency VARCHAR(3) NOT NULL DEFAULT 'USD',
  payment_terms_days INTEGER DEFAULT 30,
  minimum_payout_minor INTEGER NOT NULL DEFAULT 0,
  refund_hold_days INTEGER DEFAULT 30,
  tax_treatment VARCHAR(32),
  status VARCHAR(16) NOT NULL DEFAULT 'draft',  -- draft|active|superseded|terminated
  created_by INTEGER, approved_by INTEGER, approved_at TEXT,
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')));
CREATE UNIQUE INDEX IF NOT EXISTS ux_partner_agreement_no ON partner_agreements(agreement_number);
CREATE INDEX IF NOT EXISTS ix_partner_agreements_partner ON partner_agreements(partner_id, effective_from);

-- Effective-dated, prioritised commission rules. NULL dimension = "any".
CREATE TABLE IF NOT EXISTS partner_commission_rules(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  agreement_id INTEGER NOT NULL, partner_id INTEGER NOT NULL,
  certification_id INTEGER, route_key VARCHAR(40), product_type VARCHAR(32), country VARCHAR(64),
  commission_type VARCHAR(12) NOT NULL DEFAULT 'percentage',   -- percentage|fixed
  commission_rate_bp INTEGER NOT NULL DEFAULT 0,       -- basis points: 750 = 7.50%,
  commission_basis VARCHAR(24) NOT NULL DEFAULT 'net_after_discount',
  effective_from VARCHAR(10), effective_to VARCHAR(10),
  priority INTEGER NOT NULL DEFAULT 100,        -- lower wins; ties broken by specificity then id
  active INTEGER NOT NULL DEFAULT 1,
  created_by INTEGER, created_at TEXT DEFAULT (datetime('now')));
CREATE INDEX IF NOT EXISTS ix_pcr_partner ON partner_commission_rules(partner_id, active);

-- Immutable per-payment commission. Value fields are NEVER updated after insert.
CREATE TABLE IF NOT EXISTS partner_commission_transactions(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  txn_ref VARCHAR(32) NOT NULL,                 -- human/immutable, e.g. PCT-000123
  partner_id INTEGER NOT NULL,
  agreement_id INTEGER, commission_rule_id INTEGER,
  discount_code_id INTEGER, code_redemption_id INTEGER,
  payment_id INTEGER,                           -- NULL only for manual adjustments
  user_id INTEGER,                              -- internal only; never returned to a partner
  certification_id INTEGER, route_key VARCHAR(40), product_type VARCHAR(32),
  currency VARCHAR(3) NOT NULL DEFAULT 'USD',
  gross_minor INTEGER NOT NULL DEFAULT 0,
  discount_minor INTEGER NOT NULL DEFAULT 0,
  eligible_net_minor INTEGER NOT NULL DEFAULT 0,
  commission_type VARCHAR(12), commission_rate_bp INTEGER,     -- SNAPSHOT
  commission_basis VARCHAR(24),                                -- SNAPSHOT
  commission_minor INTEGER NOT NULL DEFAULT 0,                 -- negative for reversals
  status VARCHAR(28) NOT NULL DEFAULT 'payment_received',
  earned_at TEXT, due_at TEXT, hold_until TEXT,
  approved_at TEXT, approved_by INTEGER,
  reversal_of_transaction_id INTEGER, reason TEXT,
  requires_finance_review INTEGER NOT NULL DEFAULT 0,          -- migration/backfill flag
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')));
CREATE UNIQUE INDEX IF NOT EXISTS ux_pct_ref ON partner_commission_transactions(txn_ref);
-- Idempotency: one ORIGINAL commission per payment per partner. Reversals carry
-- reversal_of_transaction_id, so the key includes it (0 for originals) rather than
-- relying on a partial index, which Translate would strip on MySQL.
CREATE UNIQUE INDEX IF NOT EXISTS ux_pct_payment
  ON partner_commission_transactions(payment_id, partner_id, reversal_of_transaction_id);
CREATE INDEX IF NOT EXISTS ix_pct_partner_status ON partner_commission_transactions(partner_id, status);

-- Append-only state history.
CREATE TABLE IF NOT EXISTS partner_commission_events(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  transaction_id INTEGER NOT NULL,
  from_status VARCHAR(28), to_status VARCHAR(28) NOT NULL,
  actor_type VARCHAR(16) NOT NULL,              -- admin|partner|system
  actor_id INTEGER, reason TEXT, reference VARCHAR(64),
  created_at TEXT DEFAULT (datetime('now')));
CREATE INDEX IF NOT EXISTS ix_pce_txn ON partner_commission_events(transaction_id);

-- Settlement batches (maker-checker) + their allocations.
CREATE TABLE IF NOT EXISTS partner_settlements(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  settlement_no VARCHAR(32) NOT NULL, partner_id INTEGER NOT NULL,
  period_start VARCHAR(10), period_end VARCHAR(10),
  currency VARCHAR(3) NOT NULL DEFAULT 'USD',
  opening_balance_minor INTEGER DEFAULT 0, eligible_commission_minor INTEGER DEFAULT 0,
  adjustments_minor INTEGER DEFAULT 0, amount_approved_minor INTEGER DEFAULT 0,
  amount_paid_minor INTEGER DEFAULT 0, closing_balance_minor INTEGER DEFAULT 0,
  status VARCHAR(20) NOT NULL DEFAULT 'draft',  -- draft|pending_approval|approved|scheduled|paid|cancelled
  prepared_by INTEGER, reviewed_by INTEGER, approved_by INTEGER,
  scheduled_date VARCHAR(10), paid_at TEXT,
  payment_method VARCHAR(32), payment_reference VARCHAR(120),
  proof_storage_ref VARCHAR(255), internal_note TEXT, partner_note TEXT,
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')));
CREATE UNIQUE INDEX IF NOT EXISTS ux_settlement_no ON partner_settlements(settlement_no);
CREATE UNIQUE INDEX IF NOT EXISTS ux_settlement_payref ON partner_settlements(partner_id, payment_reference);

CREATE TABLE IF NOT EXISTS partner_settlement_items(
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  settlement_id INTEGER NOT NULL, transaction_id INTEGER NOT NULL,
  amount_allocated_minor INTEGER NOT NULL,
  created_at TEXT DEFAULT (datetime('now')));
-- A transaction can never be allocated to the same settlement twice; cross-batch
-- double payment is prevented by the SUM(allocated) <= commission_minor check.
CREATE UNIQUE INDEX IF NOT EXISTS ux_psi_pair ON partner_settlement_items(settlement_id, transaction_id);
```

`partner_payouts` is **retained** and imported as legacy settlements (§7), never dropped.

### 6.3 Commission state machine

`pending_student_payment → payment_received → on_refund_hold → due → pending_review →
approved_for_payment → scheduled → partially_paid → paid`, with `on_hold`, `disputed`,
`reversed` reachable per an explicit transition table. Enforced server-side in a single
`CommissionState.CanTransition(from,to)` helper — the `SimReview.cs` pattern, but
**fail-closed** (P1-L): an unknown/NULL actor is rejected, never allowed through.

Partners may **read** status only. No partner role can approve, schedule, pay or reverse.

---

## 7. Migration and backfill plan

1. **Additive only.** New tables + new columns; no existing column is dropped or retyped in Phase 1. `partner_payouts` and the derived view keep working until Phase 3 cuts over.
2. **Dry-run first.** `POST /api/admin/partner-finance/backfill?dry_run=1` reports what *would* be written, per partner, with an exception list. Repeatable and resumable — the `ux_pct_payment` unique key makes re-runs no-ops.
3. **Rate determination for history**, in order: (a) an explicit historical agreement/rule if one is created before backfill; (b) otherwise the partner's current `commission_pct` **recorded as a snapshot** and the row flagged `requires_finance_review = 1`. Never guessed silently.
4. **Legacy payouts** are imported as `partner_settlements` rows with `status='paid'`, `settlement_no = 'LEGACY-<id>'`, `payment_reference` preserved, and *no* item allocations — labelled clearly so opening balances reconcile without fabricating allocations.
5. **Reconciliation report** after backfill: attributed payments vs redemptions vs transactions vs reversals vs allocations vs payouts, with every mismatch listed. Discrepancies are **reported, never auto-corrected**.
6. **Rollback:** Phase 1-2 are additive, so rollback = stop writing + drop the new tables; no existing row was modified. Phase 3 cutover is the first irreversible step and gets its own rehearsal on a MySQL restore (`tools/mysql_backup.sh` + `backup_restore_test.py` already exist).

---

## 8. Security model

| Control | Decision |
| --- | --- |
| New permission | **`partner_finance`** — approve/settle/reverse. Placed in the `operations` group alongside `finance`/`impersonate`/`test_users`, which by deliberate design (`Security.cs:275-279`) belong to **no** role bundle and must be granted per person. |
| Read vs write | Existing `partners` continues to grant read-only commission/statement views. Money movement requires `partner_finance`. |
| Maker-checker | `prepared_by ≠ approved_by`, enforced **inside the handler** — `GateFn`'s owner bypass (`Program.cs:1097`) means a gate alone cannot enforce it. Fail-closed on NULL/0 actor (unlike P1-L precedents). |
| Partner roles | Enforce the existing four roles properly: `admin`/`finance` → codes + statements; `reporting` → read-only; `support` → no money surfaces. Closes P1-I. Add a `RequireRole` helper so new endpoints cannot forget. |
| Audit | Every finance mutation writes `partner_commission_events` (typed actor: `admin`/`partner`/`system` + id) **in the same transaction as the state change** — solving DEF-14 locally without a platform-wide `audit_logs` migration. `audit_logs` continues to receive a parallel line. |
| Idempotency | Natural DB keys, not application guards: `ux_pct_payment`, `ux_psi_pair`, `ux_settlement_payref`. Settlement approval/payment run inside `db.Transaction` with `SELECT … FOR UPDATE` on MySQL. |
| Tenant isolation | Unchanged per-query `WHERE partner_id = p.PartnerId`; new endpoints get IDOR tests (§9). |
| Exports | Reuse `Csv.Field` (existing formula-injection guard, already regression-tested). |
| Evidence files | Reuse `DocStore`/`Storage` (MIME + size validation, retention) for proof-of-payment; never a new upload path. |
| Currency | Store it, enforce equality between agreement / transaction / settlement, reject mismatch. No FX engine (§2.5). |

---

## 9. Test matrix

Reusing the existing three-layer harness (xUnit units, python integration against the real DLL, Playwright e2e).

**Unit (`PCI.Backend.Tests`)** — new `PartnerCommissionTests.cs`, `SettlementApprovalTests.cs`:
rule selection by certification/product/route/country + priority; decimal rounding at the cent
(mirroring the existing banker's-rounding facts); rate snapshot survives an agreement change;
state-machine legal/illegal transitions; reversal arithmetic for full and partial refunds;
allocation never exceeds `commission_minor`; currency mismatch rejected.

**Integration (`tests/partner_finance_test.py`, new, MySQL-capable)** —
paid redemption creates exactly one transaction; webhook replay creates none;
refund before/after due/after payout; chargeback then recovery; duplicate refund webhook;
maker≠checker enforced; overpayment rejected; duplicate allocation rejected;
partner cannot reach finance endpoints; `reporting`/`support` role limits;
cross-partner IDOR on every new endpoint; CSV formula injection; backfill dry-run idempotence.

**e2e (extend `partner-portal.spec.ts`)** — the brief's critical journey end to end:
partner creates a certification-scoped code → PCI approves → student pays with it →
immutable commission appears → hold expires → Finance approves (second admin) →
partial settlement paid with proof → statement downloads → partner sees the exact
figures and can raise a dispute.

**Personas** (all eight from the brief) map onto existing fixtures: `E2E_ADMIN` (owner),
a `partner_finance` preparer and a separate approver (new test admins via the existing
least-privilege viewer pattern in `admin-security-rbac.spec.ts`), partner `admin`/`finance`/
`reporting` users (the test-partner mechanism already mints these), the demo student,
a refunded student, and a read-only auditor.

**Guardrail — and it needs strengthening.** `migration_integrity_test.py` must keep
passing (migration idempotence, non-destruction, MySQL/SQLite column-name parity).
But it compares **names only** (P2-T), so it would not catch a money column
degrading to `DOUBLE` (P0-Q) or a missing unique index. Phase 1 therefore extends it
with, for the finance tables specifically: a **column-type** assertion, a
**unique-index existence** assertion, and a boot on a **pre-existing** MySQL database
(today every MySQL CI run is a fresh first boot, so the `AddCol`/`ALTER` upgrade path
is exercised only on SQLite).

---

## 10. Delivery phases

| Phase | Scope | Exit criteria |
| --- | --- | --- |
| **0** (this doc) | Audit + design | Premises corrected, gaps evidenced, schema/security/test design agreed |
| **1** | `FinanceSchema.cs` (agreements, rules, immutable transactions, state history), minor-unit money helper, typed-actor audit, parity-gate hardening, P0-Q guard (regeneration warning + type assertion), P1-S verification on MySQL | P0-A, P0-E, P1-J, P2-T closed; historical rate provably stable across a rate change; replay-safe; MySQL type parity asserted in CI |
| **2** | Reversals (full/partial/chargeback), webhook amount assertion, reconciliation report, backfill (dry-run → live) | P0-D, P1-K closed; reconciliation clean or every exception listed |
| **3** | Settlements, maker-checker, partial payment, proof upload, statements (PDF/CSV), disputes, `partner_finance` permission, partner role enforcement | P0-B, P0-C, P1-I, P2-M closed; overpayment and double-payment provably impossible |
| **4** | Partner code completion: expose existing scope columns, reject-don't-clamp, start≤end, block cancel-after-use, edit/resubmit/duplicate | P1-F/G/H, P2-P closed |
| **5** | Campaign links + funnel attribution, dashboard filters, pagination replacing LIMIT 500, exports | P2-N, P2-O closed |
| **6** | Hardening: load, a11y, browser matrix, migration rehearsal on a MySQL restore, backup/restore drill | Full suite green on both providers |

No phase starts while a critical defect from the previous one is open.

---

## 11. Decisions needed from PCI (blocking where noted)

| # | Question | Recommendation |
| --- | --- | --- |
| **D1** *(blocking §2.1)* | Confirm SQLite remains the dev/test provider with MySQL authoritative in CI/production. | Yes — removing it breaks dev + ~20 suites for no benefit. |
| **D2** *(blocking Phase 1)* | Default `commission_basis`: gross, net-after-discount, or net-after-tax? | `net_after_discount` — commission on what PCI actually collected. |
| **D3** *(blocking Phase 2 backfill)* | For historical payments with no recorded rate: snapshot today's `commission_pct` and flag `requires_finance_review`, or leave unvalued pending Finance input? | Snapshot + flag; Finance reviews the exception list before the first settlement. |
| **D4** | `refund_hold_days` default before commission becomes payable. | 30 days, per-agreement override. |
| **D5** | Minimum payout threshold and payment terms. | `minimum_payout` 100, `payment_terms_days` 30, both per-agreement. |
| **D6** | Should an admin-created partner code (`TrainingPartners.cs:340`) also earn commission? Today it is attributed identically. | Yes — attribution is by `partner_id`, not by who typed it. Confirm. |
| **D7** | Does a **sponsored** candidate (partner pays PCI) ever earn the partner commission? | No — sponsorship and commission are opposite money directions; exclude `route_key='sponsored'` from commission rules by default. |
| **D8** *(blocking Phase 1)* | Money representation in the ledger: **integer minor units** (recommended, §6.2) or literal `DECIMAL(12,2)`? | Minor units. `DECIMAL` is exact only on MySQL — on SQLite it is NUMERIC *affinity* (unrounded REAL), and binding a C# `decimal` through `Microsoft.Data.Sqlite` stores **TEXT**, breaking `SUM`. Minor units are exact on both, match Stripe's own representation, and make "never float" enforceable. |
| **D9** *(blocking Phase 1 if MySQL 8 is real)* | Is the production target **MariaDB 10.11** (what CI tests) or **MySQL 8** (what `OPERATIONS.md`/`render.yaml` advertise)? | Confirm MariaDB. The 234 `CREATE INDEX IF NOT EXISTS` statements are MariaDB-only syntax and would fail on MySQL 8 (P1-R) — this affects the whole platform, not just finance, so it needs an answer before more DDL is added. |

---

## 12. Phase 0 completion report

1. **Inspected** — §1 (money path, partner module, schema/migration/dialect layer, authorization, audit, all three test layers).
2. **Changed** — nothing in production code. This document only.
3. **Migrations** — none. Proposal in §6, rules in §6.1.
4. **Files changed** — `docs/finance/PARTNER_FINANCE_PHASE0_AUDIT.md` (new).
5. **Tests run** — none required (no code change). Existing coverage inventoried in §9; guardrail identified (`migration_integrity_test.py`).
6. **E2E evidence** — n/a this phase.
7. **Reconciliation** — the current derived ledger is **not** reconcilable to payments once a refund exists (P0-D); this is the primary correctness driver for Phase 2.
8. **Security/privacy** — model in §8; five concrete gaps found (P0-C, P1-I, P1-J, P1-L, plus export/evidence reuse decisions).
9. **Existing data preserved** — nothing touched. Phases 1-2 are additive by construction; `partner_payouts` is retained and imported, never dropped.
10. **Risks / blocked decisions** — D1, D2, D3, D8 block Phase 1; D9 is platform-wide and blocks further DDL if MySQL 8 is a real target. Largest technical risks, in order: (i) **P0-Q** — regenerating `schema.mysql.sql` silently converts every money column to `DOUBLE`, and CI cannot see it (P2-T); (ii) **P1-R** — `CREATE INDEX IF NOT EXISTS` is MariaDB-only; (iii) partial-unique-index stripping in `Db.Translate`, mitigated by the full-key design in §6.2; (iv) **P1-S** — a suspected pre-existing MySQL DDL failure in `certification_routes`, swallowed by a catch, which is exactly the trap the finance tables must avoid.
11. **Rollback** — no change to roll back. Phase 1-2 rollback = drop new tables.
12. **Recommended next** — Phase 1 (immutable ledger) once **D1-D3, D8** are answered (and D9 confirmed). Phase 4 items P1-F/G/H are small, isolated and independently shippable if a quick win is wanted first.
