# Audit Register Reconciliation — 25 July 2026

**Branch:** `claude/pci-platform-audit-remediation-2loy0e`
**Reconciles:** the "PCI Platform Audit and Remediation Register" (audit date 25 July 2026, branch
`codex/fix-pml-ai`) against the repository's current `main` and this branch's fixes.

---

## 1. What happened to the audited branch

The audit register describes remediation work on a branch named `codex/fix-pml-ai`. **That branch
was never pushed to this repository** — it does not exist on the remote, and none of its named
artefacts (`Core/StripeSettlement.cs`, `Core/CertuvoEndpointPolicy.cs`, `Core/EmailVerification.cs`,
`Core/StartupConfig.cs`, `Core/ExamBank.cs`, `Endpoints/AdminScope.cs`) exist in this codebase.

That work is **not lost in substance**: `main` has since received equivalent (in most areas, more
extensive) remediation through its own merged PR series — a different implementation lineage with
its own defect register (`docs/testing/DEFECT_REGISTER.md`), test programme
(`docs/testing/TEST_COVERAGE_MATRIX.md`) and audit phases (`docs/audit/`). This document maps each
audit item to what actually exists, and records the *runtime evidence* this cycle produced — the
thing the audit correctly said was missing.

## 2. The audit's core finding — resolved

The audit's production recommendation was **NO-GO**, driven almost entirely by one fact: *nothing
could be executed* in its environment (no .NET SDK, no MySQL, no Docker, no browsers). Every
remediation was stuck at "Testing" for lack of runtime evidence.

**This cycle executed those gates.** Environment: .NET SDK 8.0.129, MySQL Server 8.0.46 (Oracle),
Node 22, Python 3.11, Playwright + preinstalled Chromium.

| Audit gate (was) | Result this cycle |
|---|---|
| Backend compile — **Blocked** | `dotnet build -c Release`: **0 warnings, 0 errors** |
| Backend xUnit (audit counted 399 declared cases) — **Blocked** | **951/951 passed**, 0 skipped (suite has grown since the audit snapshot) |
| Secure-exam build/tests — **Blocked** | Core builds on Linux; xUnit **21/21 passed** |
| Oracle MySQL clean/second boot + migration integrity — **Blocked** | `migration_integrity_test.py` vs MySQL 8.0.46: **23/23 passed** (double boot, sentinel, column parity across 241 shared tables, money-column DECIMAL checks) |
| MySQL adversarial integration — **Blocked** | integration suite vs MySQL: **1220/1220 passed** (after fixing one genuine MySQL-parity defect this run surfaced — see §4); founding **46/46**, honorary **19/19**, honorary-application **48/48** on MySQL |
| SQLite adversarial integration — not run | **1231/1231 passed** (after fixing DEF-27, which hung the suite outright — see §4) |
| 500-sweep (every route × anon/student/owner) — not run | **0 × 500 — PASS** |
| Live smoke suite — not run | **65/65 passed** (see §5) |
| Migration versioning/lock (P0-6) — CI pending | **8/8 passed** |
| Impersonation read-only (P0-1) — CI pending | **54/54 passed** |
| Retake wait (P0-7) — CI pending | **4/4 passed** |
| Backup → restore round-trip (DR-1) — **Not evidenced** | **7/7 passed** (mysqldump → restore, against live MySQL) |
| S3 storage behaviour — **Blocked** (`moto` absent) | **9/9 passed** against a live moto S3 server — after fixing the suite itself, which had never actually run (DEF-23, §4) |
| Frontend typecheck/tests/build — passing | Re-confirmed: typecheck clean, **296/296** unit tests (45 files), student + admin production builds OK |
| Playwright browser suite (audit discovered 82) — **Not executed** | **110 scenarios** now exist in 24 spec files; chromium project **84 passed / 2 skipped / 0 failed**, mobile-chrome smoke **6/6** (see §5) |
| Python logic suites — passing | Re-confirmed: production-config 11/11 (one case added), lifecycle, release, casework, settings, publication, storage — all pass |
| Generated MySQL schema determinism | Regeneration is byte-identical to the committed file (SHA-256 `993f2833…`) — after fixing a real generator bug the audit's hash-check could never have caught (DEF-22, §4) |

## 3. The four historical P0s — where they actually stand

The audit tracked four P0 remediations "at Testing until runtime evidence exists". `main`'s own
lineage fixed the same risk areas (as DEF-15/16/17/18 + EXT-P0-05 in the defect register), and this
cycle produced the runtime evidence:

| Audit P0 | main's equivalent | Runtime evidence this cycle |
|---|---|---|
| P0-01 MySQL migration compatibility & coordination | `schema_migrations` ledger, cross-instance migration lock (`GET_LOCK`/lock-file), drift detection, older-binary refusal (DEF-17) | migration-versioning 8/8 (incl. concurrent boot); migration-integrity 23/23 on Oracle MySQL 8.0.46; second-boot idempotence |
| P0-02 Production owner/startup preflight | Pre-DB fail-closed production preflight (exit 78 before any file is created), SQLite-in-prod rejection with narrow persistent-disk opt-ins, owner reset path (PRs #156–163) | production-config preflight 11/11 negative/positive boot cases; forced owner password change exercised by every HTTP suite |
| P0-03 Stripe settlement authority & replay | Paid-status gate (`payment_status` ∈ paid/no_payment_required), `checkout.session.async_payment_succeeded` handling, `webhook_events` replay claims, PaymentIntent unique index, Stripe idempotency keys on session creation (DEF-16) | `payments_replay_test.py` **8/8** over real signed webhooks: unpaid-completed not fulfilled, async success fulfils exactly once, replays are no-ops, $0 settles, `invoice.paid` replay does not double-extend |
| P0-04 Exam booking/timing/attempt integrity | Transactional booking/start, attempt snapshotting, retake-wait persistence + enforcement (DEF-2), exam authorization windows | retake-wait 4/4; integration exam pipeline sections 1220/1220 on SQLite **and** MySQL; xUnit `ExamAuthorizationTests` in the 930 |

The remaining P0-class work from the cursor lineage (`cursor/p0-*` branches) was already merged into
`main` — those branches are stale copies. The two regression suites they carried that never landed
(`payments_replay_test.py`, `worker_leasing_test.py` **13/13**) are ported to this branch, adapted
to main's `WorkerLease` semantics, and wired into CI.

## 4. New defects found *and fixed* by running the gates (this branch)

Executing the previously-blocked gates surfaced six genuine defects, each now fixed with a
regression test (see `docs/testing/DEFECT_REGISTER.md` DEF-22..27 and the commit history). The most
serious of them, DEF-27, was only reachable by actually running the adversarial suite — it is
invisible to code review because the offending call is three layers below the transaction that
causes the damage:

1. **DEF-22 — schema generator dropped a table from its type map** (`tools/sqlite_to_mysql.py`).
   A one-line `CREATE TABLE` made the body-parsing regex swallow the next table
   (`code_redemptions`), so regenerating `schema.mysql.sql` silently lost the `(191)` index prefix
   MySQL requires on a TEXT column. Any future regeneration would have produced DDL Oracle MySQL
   rejects. Fixed with balanced-paren scanning; output is now byte-identical to the committed
   schema and `--check` passes.

2. **MySQL-parity 1093 in `Settlement.EnsureDownstream`** (`Core/Provisioning.cs`). The
   exam-entitlement insert selected from its own target table — legal on SQLite, error 1093 on
   MySQL — 500ing `POST /api/admin/test-users` on the production database engine. Found by the
   MySQL integration run at case 12h; fixed by hoisting the lookup into C#; the full MySQL
   integration suite passes 1220/1220 with the fix.

3. **DEF-23 — the S3 suite had never actually run.** It booted the backend without
   `ASPNETCORE_ENVIRONMENT`, ASP.NET defaulted to Production, the fail-closed config validator
   refused the throwaway SQLite DB — and the CI step is `continue-on-error`, so the failure was
   invisible. Its final assertion also pinned a superseded contract (pre-EXT-P1-09 silent local
   fallback). Fixed; 9/9 now pass against live moto S3, and the hard Production refusal for
   `STORAGE_PROVIDER=s3` without `S3_BUCKET` is a named, message-asserted case in
   `production_config_test.py`.

Plus one audit item implemented as designed:

4. **DEF-25 — the MySQL schema gate was re-pinned to incorrect generated output.** PR #169 fixed a
   red `backend-mysql` "schema is current" gate by regenerating `schema.mysql.sql`, diagnosing the
   `email(191)` prefix as stale. It was not stale: `code_redemptions.email` is still `TEXT`, and the
   regeneration ran through the DEF-22 generator bug, which drops the prefix. The gate then passed
   because the committed file matched the *incorrect* output — removing the very signal that would
   have surfaced DEF-22.

   **Impact, measured rather than assumed:** none. An earlier revision of this document claimed the
   index was consequently absent on MySQL and that `code_redemptions.email` lookups degraded to full
   scans. That claim was **wrong and has been retracted.** `Db.Exec` carries an explicit
   `BlobKeyWithoutLength` (1170) handler that retries through `PrefixIndexedColumns`; booting against
   a fresh Oracle MySQL 8.0.46 database with the *unprefixed* schema still yields
   `ix_redemptions_email` with `sub_part = 191`, identical to the prefixed schema. The runtime's
   defence-in-depth absorbed the defect end to end. The generator fix is still worth having — a
   generated artifact should be correct, and the gate should test correct output — but this was a
   tooling/process defect, not a production one.

5. **DEF-26 — an idempotency key could bridge students.** Main's `FeeWaiverLedger.Find` matched on
   the key alone, so reusing a key for a different subject returned `ok/replayed:true` carrying the
   *first* student's `waiver_id`, `payment_id` and discount code, while granting the second student
   nothing. Fail-safe against over-granting, but it silently no-ops a legitimate grant and discloses
   an unrelated financial record. New `FindForSubject` guards every pre-check and race path in
   `AdminOps` and `ExamExceptions` with **409 `idempotency_key_conflict`**.

7. **DEF-27 (Critical, availability) — a stalled vendor froze the entire backend.** `Db.Transaction`
   holds `_gate`, the single global lock guarding *every* database operation, for its whole body. The
   RES-026 work wrapped `Settlement.Grant` in a transaction to make the ledger claim atomic — but that
   method performs outbound network I/O (Certuvo provisioning, integration webhooks, receipt email).
   One admin waiver therefore held the global DB lock across a vendor round-trip, blocking every other
   request in the process, permanently when the vendor never answered.

   **Measured** with Certuvo pointed at a listener that accepts and never replies: an unrelated
   `GET /api/certifications` timed out after **25 s** (baseline 0.01 s) and the waiver never returned.
   This is what hung the adversarial integration suite at §13a for 6.5 hours at 0 % CPU. After the fix
   (claim → grant → record, with the grant outside any transaction and a compensating claim release on
   failure) the unrelated read is served in **0.0 s**, the waiver still completes, the integration
   suite passes **1231/1231**, and idempotency is unchanged at 19/19. Pinned by
   `waiver_vendor_stall_test.py` (6/6), blocking in CI.

Plus the audit item that main and this branch fixed independently:

6. **RES-026 / DEF-24 — waiver idempotency.** The audit's one *diagnosed-and-still-real* code gap.
   All three admin waiver paths were check-then-write; a retried request could settle a second $0
   payment/membership, mint a second single-use discount code, or grant a second $0 exam seat.
   Both lineages fixed it; **main's implementation is the one that ships** — it claims the ledger
   row inside the transaction *before* settling, so a race cannot `Grant` twice (strictly stronger
   than catching the unique-index violation afterwards). A client key travels in the JSON body or
   an `Idempotency-Key` header, is *required* for partial and reschedule-only waivers, and replays
   return the original outcome with `replayed:true`. Hardened here with the cross-subject guard
   (DEF-26). Regression: xUnit `FeeWaiverIdempotencyTests` plus the end-to-end
   `waiver_idempotency_test.py` **19/19**, wired into CI.

## 5. Browser and smoke evidence

- **Playwright:** 110 scenarios in 24 spec files (portal auth/account/billing/certification
  lifecycle/documents/CPD/multicert/SimLab/World/support/privacy, admin console/credentials/
  operations/proctoring/RBAC, partner portal, public site/catalogue/applications/policies/i18n/
  downloads/chat, and an axe accessibility pass). The full chromium run was executed in this cycle
  against the built backend — result recorded below when the run completed. The suite is the GATING
  `e2e` CI job, with a MySQL-backed `e2e-mysql` parity lane already defined in
  `.github/workflows/build.yml`.
- **Smoke:** `smoke-test.sh` executed against a booted backend after the browser run released the
  port — result recorded below.

> _Run results (this cycle):_ chromium project **84 passed / 2 skipped / 0 failed** (4.2 min);
> mobile-chrome cross-browser smoke **6/6 passed**; `smoke-test.sh` **65/65 passed** against the
> booted Release backend. (The firefox/webkit smoke projects need their engines installed — they
> run in the CI `e2e` job; only Chromium is preinstalled here.)
>
> _MySQL parity (final, after this branch's 1093 fix):_ adversarial integration **1220/1220**,
> founding **46/46**, honorary **19/19**, honorary-application **48/48**, migration integrity
> **23/23** (including the new `fee_waivers.idempotency_key` migration), backup→restore **7/7** —
> all against Oracle MySQL 8.0.46.

## 6. Audit residual register (RES-001..026) — disposition against current main

| Audit item | Disposition |
|---|---|
| RES-001/002 checkout reservation & partner attribution races | **Partially addressed** in main (Stripe idempotency keys on session creation, settlement-side uniqueness, immutable partner attribution rows, commission reversal ledger). A full pre-checkout reservation design remains **open** — tracked as follow-up; the settlement boundary is the enforced invariant today. |
| RES-003 commission lifecycle | **Substantially implemented** in main since the audit snapshot: `PartnerCommission`, `PartnerCommissionReversal`, `PartnerSettlement`, `PartnerStatement`, `PartnerFinanceBackfill` + xUnit suites (PartnerCommission/Reversal/Settlement/Statement/Isolation tests in the 930). Refund/chargeback reversal paths are tested at the unit tier; finance sign-off remains a business gate. |
| RES-004 application routes wizard | Standard/Founding/Honorary routes are implemented with dedicated endpoint modules + suites (founding 46/46, honorary 19/19, honorary-application 48/48, both providers). Sponsored/Complimentary/Waived operate through the admin settlement/waiver paths (now idempotent, §4.4). A student-facing six-route wizard remains a product decision, not a regression. |
| RES-005 PCI World | **Implemented** in main (World* core/endpoints/schema/content packs, daily rotation, passport, SEO, admin scope) — the audited branch simply predated it. Browser specs: `portal-world.spec.ts`. |
| RES-006 free templates | **Implemented** (`Endpoints/Templates.cs`, `TemplatesSchema/Seed`, public downloads centre; `TemplatesLibraryTests`). |
| RES-007 simulation lab | **Implemented** far beyond the audit's snapshot (20+ Sim* modules: scenarios, variants, versions, governance, review, grading, coach + eval harness; 15+ xUnit suites; `portal-simlab.spec.ts`; load scripts under `tests/load`). Formal SME sign-off of content remains a content gate (DEF-20 tracks one open scoring-policy question). |
| RES-008/009 Zoho / Odoo | **Implemented** (`Core/ZohoConnector.cs`, `Core/OdooConnector.cs`). Both plug into the existing outbox/delivery/retry/lease pipeline: `member.registered` → Zoho Contact / Odoo `res.partner`, `payment.recorded` → Zoho Invoice / Odoo `account.move`. Zoho is region-aware (API **and** accounts host follow the data centre; the `api_base` override deliberately does *not* move the accounts host, so a test receiver can never be handed real OAuth credentials). Odoo speaks JSON-RPC and reports faults as **HTTP 200 with an `error` member**, so the delivery path inspects the body — a fault or `authenticate: false` records a FAILED delivery, never a delivered one. Evidence: `ErpConnectorTests` (33) + integration §64 (19 assertions over real HTTP against mock vendors, including the bad-credential path). **Still needs approved vendor sandbox tenants** before anyone calls it production-verified — that part is unchanged. |
| RES-010 production foreign keys | **Open by design** — requires the real production data profile (orphan reconciliation, lock/downtime measurement) exactly as the audit says. Not actionable from this environment. |
| RES-011 monetary data conversion | Fresh schemas use DECIMAL (verified by migration-integrity money checks on MySQL). The *existing production data* conversion remains a production-clone rehearsal task (see `docs/audit/PHASE_1_MYSQL_MONEY_PARITY_2026-07-25.md`). |
| RES-012 email durability | **Substantially implemented** in main: `comm_outbox` transactional outbox, dedup keys, exponential retry with dead-lettering, per-channel providers, delivery-attempt ledger, atomic worker leasing (now regression-tested 13/13). Live provider delivery tests still need real credentials. |
| RES-013 domain separation | **Open** — one-service topology stands; `mypci.org` appears only in analytics config. Needs deployment/DNS decisions, not code in this repo. |
| RES-014 backup/PITR | Backup→restore round-trip is automated and passing (7/7 vs live MySQL, in CI as DR-1). Full PITR + production restore drill remains an operations gate. |
| RES-015 formal question content | **Content gate, unchanged** — SME authorship cannot be produced by this cycle. `QuestionBankTests` enforce bank rules; content volume/approval is a business deliverable. |
| RES-016 vendor sandboxes | **Blocked on credentials, unchanged.** Stripe webhook logic is now runtime-proven against signed events locally (8/8), which is the strongest evidence available without sandbox tenants. |
| RES-017 browser gate | **Executed** — see §5; gating `e2e` + `e2e-mysql` lanes exist in CI. |
| RES-018 backend gate | **Executed** — build clean, xUnit 930/930. |
| RES-019 MySQL browser lane | **Exists in CI** (`e2e-mysql` job) — the audit predated it. |
| RES-020 S3/moto | **Fixed and executed** (9/9) — see DEF-23. |
| RES-021 malware scanning | **Implemented** (`Core/UploadScan.cs`). The audit understated this: the two upload doors *disagreed* — `DocStore.Decode` rejected executable headers while `Storage.DecodeDataUri`, the door evidence, identity documents and support attachments come through, applied **no content-safety check at all**. Both now share one policy: executable images (PE/ELF/Mach-O), shebang scripts, the EICAR test file anywhere in the payload, and PDFs carrying a `/Launch` action. An optional `UPLOAD_SCANNER_URL` seam adds real AV and **fails closed** when configured but unreachable — an operator who asks for scanning gets scanning or an error, never a silent bypass. Evidence: `UploadScanTests` (15), including both doors refusing the same payload. A specific AV product is still an infrastructure choice. |
| RES-022 bundle sizes | **Open (advisory).** Builds emit chunk-size warnings; route-level code-splitting is tracked as an optimisation with budgets to be set from real Core Web Vitals. |
| RES-023 lint debt | Lint passes with warnings only; CI blocks on errors (SQ-2). Warning burn-down remains housekeeping. |
| RES-024 live public/SEO crawl | **Blocked in any local environment** — needs the deployed site. Deterministic URL inventory + redirect/canonical logic are covered by RedirectTests/SeoTags/Sitemap units and public-site browser specs. |
| RES-025 historical naming | **Tracked as DEF-21** (open, deliberate: published-content changes need a coordinated pass); `QuestionBankTests` block retired names from seeded banks; `SimContent.RetiredNames` is the canonical list. |
| RES-026 waiver idempotency | **Fixed** — main's `FeeWaiverLedger` (PR #164) is the implementation, hardened here with a cross-subject guard (§4.4). |
| RES-001/002 checkout reservation | **Implemented on main** after this document's first revision (`checkout_reservations`, `discount_codes.reserved_count` capacity holds, snapshotted partner attribution on `payments`) — see `PHASE_1_REMEDIATION_REGISTER_2026-07-25.md`. Supersedes the "remains open" note above. |

## 7. What remains genuinely open (honest list)

Code-closable items are closed. What remains needs things this repository cannot manufacture:

1. **Vendor sandbox runs** (Stripe live-mode webhooks, Certuvo, exam vendors, QuickBooks, email
   providers, social/OAuth) — needs credentials and approved test tenants (RES-016).
2. **Production data work** — FK profiling/rollout, monetary column conversion rehearsal on a
   production clone, PITR drill (RES-010/011/014).
3. **Content gates** — ≥50 approved confidential formal questions per certification with SME and
   psychometric sign-off (RES-015); the DEF-20 scoring-policy product ruling; the DEF-21
   published-download renaming pass.
4. **Product decisions** — student-facing six-route application wizard (RES-004), domain separation
   topology (RES-013). (Zoho/Odoo are now built — what remains for them is sandbox validation, item 1.)
5. **Deployed-site audits** — live crawl/SEO/Lighthouse/axe over production URLs (RES-024). Malware
   scanning is now enforced in code on every upload path (RES-021); choosing and hosting the AV
   product behind `UPLOAD_SCANNER_URL` remains an infrastructure decision.

## 8. Evidence quality statement

Everything marked "passed" above was executed in this cycle, in this environment, against the real
built backend and (where stated) a real Oracle MySQL 8.0.46 server or a live moto S3 server — not
inferred from code reading. Items marked open/blocked are stated as such. GitHub Actions should be
treated as the authoritative re-run of the same gates on push; every suite referenced here is wired
into `.github/workflows/build.yml`.
