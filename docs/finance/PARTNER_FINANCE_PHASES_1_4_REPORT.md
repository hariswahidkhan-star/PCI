# Marketing Partner finance module — completion reports, Phases 1–4

Design, gap map and decisions: [`PARTNER_FINANCE_PHASE0_AUDIT.md`](./PARTNER_FINANCE_PHASE0_AUDIT.md).
Each report follows the twelve-item format fixed in Phase 0 §12.

Findings closed so far: **P0-B, P0-C, P0-D, P1-F, P1-G, P1-H, P1-I, P2-M**.
Still open: **P0-Q, P1-R, P1-S, P2-T** (all platform-wide, listed in §5 below), plus Phases 5–6.

---

## Phase 1 completion report — immutable commission ledger

1. **Inspected** — the payment/provisioning path (`Endpoints/Payments.cs`, `Core/Provisioning.cs`, `Core/Settlement.cs`), the derived commission calculation in `Endpoints/Partners.cs`, attribution through `code_redemptions`, and the dual-provider DDL rules in `Data/Db.cs`.
2. **Changed** — commission stopped being *recalculated on every read from the partner's current percentage* and became a transaction-level ledger written once, at settlement, with the governing rate, basis and rule **snapshotted**. Changing an agreement can no longer restate history.
3. **Migrations** — six additive tables via `Data/FinanceSchema.cs`, installed at boot alongside the existing `MarketingSchema`/`SimLabSchema`/`TemplatesSchema` installers. No existing table altered; no data moved.
4. **Files changed** — new: `Data/FinanceSchema.cs`, `Core/Money.cs`, `Core/PartnerCommission.cs`, `tests/…/PartnerCommissionTests.cs`. Modified: `Core/Provisioning.cs`, `Endpoints/Payments.cs`, `Endpoints/Partners.cs`, `Program.cs`.
5. **Tests run** — 21 xUnit facts covering rule resolution (priority then specificity), snapshotting, idempotency, hold computation and the money helper's rounding. Full CI green on merge (PR #126).
6. **E2E evidence** — the existing payment e2e suite exercises the webhook path the commission hook sits in; it passed unchanged, which is the evidence that the hook is non-intrusive.
7. **Reconciliation** — a settled, partner-attributed payment now produces exactly one commission transaction, keyed `pay:{paymentId}:p:{partnerId}`. One payment → one redemption → one partner, so the chain is 1:1 and countable.
8. **Security/privacy** — `user_id` is recorded on the transaction for internal reconciliation and is never returned on a partner-facing route.
9. **Existing data preserved** — entirely additive. `partner_payouts` and the derived view are untouched and still served.
10. **Risks** — money is stored as **integer minor units**, not `DECIMAL`: `DECIMAL` is exact on MySQL but only NUMERIC *affinity* on SQLite, and `Microsoft.Data.Sqlite` binds a C# `decimal` as TEXT, which breaks `SUM()`. Rates are basis points for the same reason.
11. **Rollback** — drop the six tables; the derived view still answers. No payment path depends on the ledger succeeding (the hook is inside a `try`).
12. **Recommended next** — Phase 2, because until refunds reverse, the ledger is complete but not yet *correct*.

---

## Phase 2 completion report — reversals, backfill and reconciliation

1. **Inspected** — all three refund paths (partial-refund webhook branch, full refund/dispute branch, `Settlement.Reverse`), and the historic `partner_payouts` data.
2. **Changed** — closes **P0-D**. A refund previously made attributed revenue *vanish* from the calculation, and a partially refunded payment was dropped whole rather than in part. Money going back to the student now pulls the commission back with it via a linked negative transaction; the original is never edited.
3. **Migrations** — none. Reversals are rows in the Phase 1 table.
4. **Files changed** — new: `Core/PartnerCommissionReversal.cs`, `Core/PartnerFinanceBackfill.cs`, `tests/…/PartnerCommissionReversalTests.cs`. Modified: `Endpoints/Payments.cs`, `Core/Provisioning.cs`, `Endpoints/Partners.cs`.
5. **Tests run** — 8 xUnit facts: full refund nets to zero; a retried webhook reverses nothing further; a partial refund reverses its proportion and leaves the original open; a larger later refund reverses only the increment; a chargeback reverses in full; a post-payout reversal leaves the original `paid` and records a recoverable; unattributed and un-refunded payments reverse nothing.
6. **E2E evidence** — the refund e2e path passed unchanged.
7. **Reconciliation** — `GET /api/admin/partner-finance/reconcile` compares attributed payments → redemptions → transactions → reversals → allocations and reports missing commissions, refunds without a reversal, over-allocated transactions and rows awaiting Finance review. **Discrepancies are reported, never auto-corrected** — silently repairing financial history is what an audit trail exists to prevent.
8. **Security/privacy** — the backfill and reconcile routes are behind the `finance` permission (owner + explicit grant only).
9. **Existing data preserved** — the backfill is **dry-run by default** and idempotent through the same UNIQUE `dedupe_key` the live path uses. Legacy `partner_payouts` are imported as `LEGACY-` settlements with **no invented item allocations**: a payout never referenced specific transactions, and fabricating that link would be a lie in the audit trail.
10. **Risks** — where a historic rate cannot be proven from an agreement, the partner's current percentage is snapshotted and the row flagged `requires_finance_review`. It is never guessed silently and never left unvalued, but those rows do need a human pass before their first settlement.
11. **Rollback** — delete rows where `reversal_of_transaction_id IS NOT NULL` and the `LEGACY-` settlements; nothing else changes.
12. **Recommended next** — Phase 3: the ledger is now correct, but money still leaves through the uncontrolled Phase 0 payout path.

---

## Phase 3 completion report — settlements, statements and disputes

1. **Inspected** — the existing payout endpoint (`POST /api/admin/training-partners/{id}/payouts`), the platform's two maker-checker precedents (`Core/SimReview.cs`, `Core/WorldLifecycle.cs`), the permission gate's owner bypass in `Program.cs`, and `Core/SimplePdf.cs`.
2. **Changed** — closes **P0-B** (payout with no balance check), **P0-C** (payout gated on `partners`, no approval), **P1-I** (no payment evidence) and **P2-M** (no partner-facing statement). The free-form payout row is replaced by `prepare → approve → pay` with every included commission allocated explicitly.
3. **Migrations** — two additive tables (`partner_disputes`, `partner_dispute_messages`) in `Data/FinanceSchema.cs`. The settlement tables already existed from Phase 1. One state-machine change: `scheduled → approved_for_payment` was added, without which cancelling a settlement silently stranded its commissions as permanently `scheduled`.
4. **Files changed** — new: `Core/PartnerSettlement.cs`, `Core/PartnerStatement.cs`, `tests/…/PartnerSettlementTests.cs`, `tests/…/PartnerStatementTests.cs`. Modified: `Data/FinanceSchema.cs`, `Core/PartnerCommission.cs`, `Core/Security.cs`, `Endpoints/Partners.cs`.
5. **Tests run** — 18 settlement facts and 12 statement facts. Every refusal is asserted **by its reason code**, so a change that blocks the right thing for the wrong reason still shows up. Statements are tested to balance — `opening + earned + reversals − paid = closing` — across period boundaries.
6. **E2E evidence** — full CI green: `backend`, `backend-mysql`, `e2e`, `frontend`, `static-quality`, `secureexam-core-linux`, `secureexam-windows`. `backend-mysql` passing is the evidence that the new DDL installs on MariaDB, not only SQLite.
7. **Reconciliation** — `PayableMinor` = approved commission − allocations to live batches − unrecovered balance, and the Phase 2 reconcile route gained an over-allocation check. Statements are derived on demand and never stored, so a statement cannot drift from the ledger it describes.
8. **Security/privacy** — new granular `partner_finance` section (`pf_view`, `pf_agreements`, `pf_prepare`, `pf_approve`, `pf_pay`, `pf_dispute`), owner + explicit-grant only. **Maker-checker is enforced in the engine, not the gate**, because the gate has an owner bypass — an owner could otherwise approve their own batch. Unlike the platform's existing precedents this one is **fail-closed**: a settlement with no recorded preparer cannot be approved at all. Partner-facing output omits `user_id` and student identity; internal notes are never returned; a partner can only reference their own transactions and settlements, since accepting another partner's id would leak their ledger through the error responses.
9. **Existing data preserved** — the legacy payout endpoint is untouched and still functions; nothing was migrated or deleted.
10. **Risks / defects found while building** — three real bugs were found by tracing the arithmetic rather than by testing the happy path, and all three are now pinned by tests:
    - `PayableMinor` summed *approved* commission over three statuses but subtracted allocations over **all** of them, so every historic payout was deducted a second time and the balance went permanently negative.
    - A recoverable (commission paid out, then refunded) was deducted from **every** future settlement forever, underpaying the partner without limit. It is now recouped exactly once, recorded as the batch's own negative adjustment, and released again if that batch is cancelled.
    - The over-allocation check summed only the batch being paid, missing the case it exists for — the same commission placed in two batches.
11. **Rollback** — drop `partner_disputes` and `partner_dispute_messages`; revoke the `pf_*` grants. Settlements already paid stay as the record of money that genuinely left.
12. **Recommended next** — Phase 5 (campaign attribution, dashboard filters, pagination, exports), then Phase 6 hardening. Phase 4 shipped alongside this one.

### Known limitation

A settlement pays in **one currency**. When a partner has approved commission in more than one, the extra currencies are left for their own batch and reported back in `skipped_currencies` rather than being summed into a meaningless total. Multi-currency netting is not attempted and is not in scope for Phases 1–6.

---

## Phase 4 completion report — complete partner-managed codes

1. **Inspected** — `POST /api/partner/codes` and `/cancel` in `Endpoints/PartnerPortal.cs`, the `discount_codes` scope columns, and where each is enforced at redemption (`Endpoints/Public.cs`).
2. **Changed** — closes **P1-F, P1-G, P1-H**. Two defects fixed:
    - **Reject, never clamp.** Every percentage ran through `Math.Clamp(1..100)`, so a partner who typed 150 silently got a live **100% full-sponsorship** code and one who typed 0 got 1%. Both are terms the partner never entered and had no way to notice.
    - **A used code could be withdrawn.** `cancel` selected `used_count` and then ignored it, so a code with redemptions against it could be cancelled, stranding the redemptions, commission transactions and entitlements referencing it. Cancel also had **no role check at all**.
3. **Migrations** — none. Every column used already existed.
4. **Files changed** — new: `Core/PartnerCodeRules.cs`, `tests/…/PartnerCodeRulesTests.cs`. Modified: `Endpoints/PartnerPortal.cs`.
5. **Tests run** — 30 facts over the rules, including every clamp case that previously produced silently different terms.
6. **E2E evidence** — the existing partner-portal e2e suite (including the test-partner scenarios) passed unchanged.
7. **Reconciliation** — n/a; this phase does not touch money movement. It does make scope explicit, which improves commission attribution accuracy downstream.
8. **Security/privacy** — every mutating code route now requires an institution **admin/finance** login, matching the pre-existing rule on code creation. A `reporting` or `support` login can no longer cancel a code.
9. **Existing data preserved** — no code row is rewritten by this change. `POST /api/partner/codes` deliberately keeps its original inline ceiling checks so its established error payloads, which carry a `limit` field the portal displays, are unchanged.
10. **Risks / deliberate omission** — `route_key` is **not** exposed to partners. It drives commission attribution but is *not* enforced at redemption, so offering it as a restriction would be a control that does nothing. Enforcing it would change redemption behaviour for existing admin-created codes that already set it, which is outside this phase's remit. Every field that *is* offered — `certification_id`, `per_user_limit`, `min_transaction`, `max_discount`, `eligible_countries` — was verified as genuinely enforced in `Public.ValidateCode`/`Public.Pricing` before being exposed.
11. **Rollback** — revert the one file plus the rules class; no data implications.
12. **Recommended next** — Phase 5.

---

## §5. Platform-wide findings still open

These sit outside the partner module but were found during it and remain unfixed:

| Id | Finding |
| --- | --- |
| **P0-Q** | `schema.mysql.sql` is generated by `tools/sqlite_to_mysql.py` but **hand-tuned** for `DECIMAL(12,2)` money columns and index prefix lengths. Regenerating it silently reverts every money column to `DOUBLE`. |
| **P2-T** | `migration_integrity_test.py` compares table and column **names only**, so it cannot see P0-Q. Hardening it to assert finance column *types* and unique-index existence is still outstanding. |
| **P1-R** | `CREATE INDEX IF NOT EXISTS` is MariaDB-only. CI runs MariaDB 10.11, so this is invisible today and would fail on MySQL 8. |
| **P1-S** | A suspected pre-existing MySQL DDL failure in `certification_routes` (err-1170), swallowed by a `catch` — exactly the trap the finance tables were designed to avoid. Unverified. |
