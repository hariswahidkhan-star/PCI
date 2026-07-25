# PCI Platform Audit and Remediation Register

**Audit date:** 25 July 2026  
**Original audit branch:** `codex/fix-pml-ai`  
**This-cycle branch:** `cursor/audit-register-waiver-idempotency-f22e` (off current `main`)  
**Repository:** hariswahidkhan-star/PCI  
**Audit cycle:** Phase 1 baseline plus risk-prioritized remediation  
**Method:** Discover → reproduce → diagnose → fix → test → verify → document → re-scan

---

## 0. Reconciliation against current `main` (this cycle)

The narrative register below was authored against `codex/fix-pml-ai`. That tip is an **ancestor of current `main`**. Several residual product claims in §8 are therefore **stale relative to current `main`**:

| Register ID | Original claim | Current `main` evidence |
|-------------|----------------|-------------------------|
| RES-005 | PCI World absent | Present (`WorldPages` / `WorldAdmin`, passport, SEO layers, tests) |
| RES-006 | Free templates absent | Present (`/app/templates`, admin Templates Studio) |
| RES-007 | Simulation lab incomplete | Substantial SimLab + AI coach + content packs shipped |
| RES-014 / ops | Backup/PITR unevidenced | Scripts exist; full restore drill still required |
| REM-* file names | `StripeSettlement.cs`, `StartupConfig.cs`, … | Equivalent controls live under other names on `main` (e.g. `Settlement`, production config guards, payment webhook gates) |

**Production recommendation remains NO-GO** until green GitHub CI (including Oracle MySQL), full Playwright, vendor sandboxes, and exercised recovery are evidenced. This cycle does **not** promote any historical P0 to Verified.

### This-cycle remediation

| ID | Change | Status |
|----|--------|--------|
| RES-026 | Partial and reschedule-only fee-waiver ledger idempotency (client key + unique schema guard + replay + no duplicate `fee_waiver` on full exam-fee grant) | **Testing** (local xUnit + integration assertions updated; CI/MySQL concurrency still required) |
| RES-001 | Checkout reservation: durable client idempotency key, `checkout_reservations` table, `discount_codes.reserved_count` capacity holds converted on settle | **Testing** |
| RES-002 | Immutable partner attribution snapshotted onto reservation → payment; commission / legacy ledger prefer `payments.partner_id` | **Testing** |

### This-cycle local verification

| Check | Result |
|-------|--------|
| `dotnet build -c Release` (backend) | Pass |
| `dotnet test` `FeeWaiverIdempotencyTests` (5) | Pass |
| Python `lifecycle` / `release` / `casework` / `settings` / `publication` | Pass |
| Frontend `tsc --noEmit` | Pass |
| Frontend unit/component tests | Pass (44 files / 291 tests) |
| Oracle MySQL / Docker / full Playwright / vendor sandboxes | Not run in this environment |

---

## 1. Executive decision

**Production recommendation: NO-GO.**

Static review found no currently confirmed, unresolved P0 defect after prior remediation work, but that statement is deliberately narrower than saying the P0s are resolved. Four historical P0 remediations remain at **Testing** until GitHub CI and runtime integration evidence are available:

1. Oracle MySQL migration compatibility, authority, and migration coordination.
2. Production owner-bootstrap and startup-preflight protection.
3. Stripe settlement authority, replay resistance, and payment-state integrity.
4. Exam booking, timing, attempt, bank, and finalization integrity.

All implemented remediations in this register are marked **Testing**, never **Verified**, unless an authoritative CI/runtime package is attached later.

---

## 2. Status semantics

| Status | Meaning |
|--------|---------|
| Discovered | Evidence identifies a gap; remediation not started |
| Reproduced | Reliable code/test/runtime reproduction exists |
| Diagnosed | Root cause understood |
| Fix in progress | Owning workstream still changing implementation |
| Testing | Remediation exists but lacks required runtime/CI evidence |
| Verified | Required automated and runtime evidence passed authoritatively |
| Deferred | Explicitly postponed with reason and owner |
| Blocked | Environment, credentials, or decision unavailable |
| Not reproducible | Available evidence did not reproduce the reported behaviour |

---

## 3. Historical P0 remediation register (unchanged posture)

| ID | Module | Status | Required promotion evidence |
|----|--------|--------|-----------------------------|
| P0-01 | Database startup/migrations | Testing | .NET build, Oracle MySQL 8.4 clean/concurrent/second boot, migration integrity |
| P0-02 | Production owner/startup | Testing | Startup-config xUnit, clean production boot negatives, owner lifecycle |
| P0-03 | Payments | Testing | Payment xUnit, MySQL idempotency/concurrency, Stripe sandbox matrix |
| P0-04 | Examination integrity | Testing | Backend/secure-exam build, timing/bank suites, MySQL races, browser interruption |

---

## 4. Residual issue register (priority slice)

Severity reflects potential impact, not proof of exploitability. Product items marked *stale vs main* still need acceptance testing even when code exists.

| ID | Module | Severity | Status | Notes |
|----|--------|----------|--------|-------|
| RES-001 | Checkout concurrency | P1 | **Testing (fixed this cycle)** | See §5a |
| RES-002 | Partner checkout | P1 | **Testing (fixed this cycle)** | See §5a |
| RES-003 | Commission lifecycle | P1 | Discovered | Full due/review/approval/dispute ledger incomplete |
| RES-004 | Certification applications | P1 | Discovered | Complete six-route save/resume wizard incomplete |
| RES-005 | PCI World | P1 | *Stale — product present on main* | Still needs React-migration / acceptance gates |
| RES-006 | Free templates | P1 | *Stale — product present on main* | Still needs full content/security matrix |
| RES-007 | Simulation lab | P1 | *Partially stale — major modules present* | Expanded bank / coach eval gates continue |
| RES-008 | Zoho ERP | P1 | Discovered | Not implemented |
| RES-009 | Odoo | P1 | Discovered | Not implemented |
| RES-010 | MySQL referential integrity | P1 | Blocked | Needs production data profile |
| RES-011 | Existing monetary data | P1 | Blocked | Fresh DECIMAL schema ≠ production conversion |
| RES-012 | Email durability | P1 | Diagnosed | Uneven outbox/retry coverage |
| RES-013 | Domain separation | P1 | Diagnosed | `projectcontrolsinstitute.org` / `mypci.org` |
| RES-014 | Backup/PITR | P1 | Discovered | Restore drill not evidenced |
| RES-015 | Formal question content | P1 | Blocked | SME / confidential bank gate |
| RES-016 | Vendor validation | P1 | Blocked | Sandbox credentials |
| RES-017 | Browser release gate | P1 | Blocked | Playwright must run in CI |
| RES-018 | Backend release gate | P1 | Blocked locally previously; **build runnable here** | Full xUnit suite still needs CI artifact |
| RES-026 | Exam waiver ledger idempotency | P1 | **Testing (fixed this cycle)** | See §5 |

---

## 5a. RES-001 / RES-002 remediation detail (this cycle)

### Issues

- Checkout create used a minute-bucket Stripe idempotency key and did not hold discount-code capacity, so concurrent checkouts could oversell `max_uses` / partner allocation at settlement.
- Partner attribution was re-read from live `discount_codes.partner_id` at settle / in the derived admin ledger, so reassignment could restate history.

### Fix

- `checkout_reservations` + `discount_codes.reserved_count` + `payments.discount_code_id` / `payments.partner_id`
- `Core/CheckoutReservation.cs` — reserve / replay / expire / settle / stamp attribution
- `POST /api/create-checkout-session` requires client idempotency key; Stripe key = client key; capacity held before session create
- Webhook converts hold → `used_count` (Founding-style abort if oversold); stamps payment partner snapshot
- `PartnerCommission.EnsureForPayment` and `Partners.CommissionLedger` prefer snapshotted `payments.partner_id`
- Student portal + classic `student.html` send `idempotency_key`

### Tests

`CheckoutReservationTests` (6), existing `PartnerCommissionTests` still green.

---

## 5. RES-026 remediation detail (this cycle)

### Issue

Partial and reschedule-only waiver ledger rows lacked a client idempotency key and schema-level uniqueness guard. Retries could duplicate `fee_waivers` rows and (for student partial waivers) mint a second discount code. The exam-fee full-waiver path also wrote a duplicate ledger row via `GrantAttempt` + a second `INSERT`.

### Fix

- `fee_waivers.idempotency_key` + unique index `ux_fee_waivers_idem` (Migrate).
- `Core/FeeWaiverLedger.cs` — normalize/resolve key, `INSERT OR IGNORE`, replay payload.
- `POST /api/admin/exam-fee-waiver` — key **required** for partial and reschedule-only; claim ledger before seat grant; single ledger row (`recordFeeWaiver: false` on `GrantAttempt`).
- `POST /api/admin/students/{id}/waive` — key **required** for partial; claim ledger before code mint / settlement.
- Admin React clients send `idempotency_key: crypto.randomUUID()` on submit.
- xUnit `FeeWaiverIdempotencyTests` + integration assertions for missing key / replay.

### Principal files

`backend/Core/FeeWaiverLedger.cs`, `backend/Core/ExamAuthorization.cs`, `backend/Data/Migrate.cs`, `backend/Endpoints/ExamExceptions.cs`, `backend/Endpoints/AdminOps.cs`, `backend/tests/PCI.Backend.Tests/FeeWaiverIdempotencyTests.cs`, `backend/tests/integration_test.py`, `frontend/src/admin/pages/Students.tsx`, `frontend/src/admin/pages/ExamExceptions.tsx`

### Still required for Verified

.NET + MySQL concurrency/race matrix in CI; admin browser journey asserting network retry replay; finance sign-off on ledger uniqueness.

---

## 6. Final recommendation

The remediation cycle (prior work plus this RES-026 increment) improves the static risk posture. **Final recommendation remains NO-GO** pending green GitHub CI, authoritative MySQL/browser evidence, required product-gap decisions, vendor validation, and an exercised deployment/rollback/recovery process.

See also: `PHASE_0_PLATFORM_AUDIT_2026-07-25.md`, `PHASE_1_MYSQL_MONEY_PARITY_2026-07-25.md`.
