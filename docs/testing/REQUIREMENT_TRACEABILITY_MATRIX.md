# PCI Platform — Requirement Traceability Matrix (RTM)

_Maps each high-risk platform requirement to the tests that verify it and the evidence (suite + case
ids). Closes COV-3. This is the "does every requirement have a test?" view; `TEST_COVERAGE_MATRIX.md`
is the "does every module have coverage, and where are the gaps?" view. Evidence ids:_
- _`§N` = `backend/tests/integration_test.py` section (live-HTTP, run on **both** SQLite and MySQL)._
- _`xU:Class` = `backend/tests/PCI.Backend.Tests` xUnit class._
- _`FE:File` = `frontend/src/**/*.test.tsx` Vitest/RTL._
- _`E2E:spec` = `frontend/e2e/*.spec.ts` Playwright._
- _Status: ✅ automated · 🟡 partial/one-layer · ⏳ operator/external-pending._

## Authentication, sessions, 2FA

| Req | Requirement | Tests | Evidence | Status |
|---|---|---|---|---|
| AUTH-1 | Password login issues a session; wrong password is rejected without leaking account existence | §14, E2E portal-auth, FE:Login | §14, `portal-auth.spec.ts`, `Login.test.tsx` | ✅ |
| AUTH-2 | Per-account lockout after N failures; correct password refused while locked; cleared on expiry | §14u/§28, xU:—(LoginGuard via HTTP) | §28a–28e | ✅ |
| AUTH-3 | TOTP step-up: `totp_required`/`totp_invalid`, replay guard (consumed timestep refused) | §14u5/u6, FE:Login, FE:AdminLogin | §28, `Login.test.tsx`, `AdminLogin.test.tsx` | ✅ |
| AUTH-4 | TOTP is RFC-6238 correct; recovery codes one-time; secrets never emitted | xU:Security, FE:Profile (enrolment) | `SecurityTests`, `Profile.test.tsx` | ✅ |
| AUTH-5 | Admin password recovery (forgot link no-enumeration; recovery-code reset with min-length) | FE:AdminLogin | `AdminLogin.test.tsx` | 🟡 (UI; server via §14) |

## Payments, settlement, refunds

| Req | Requirement | Tests | Evidence | Status |
|---|---|---|---|---|
| PAY-1 | Signed webhook settles a purchase; idempotent; unknown/invalid rejected | §1, xU:SettlementTests | §1, `SettlementTests` | ✅ |
| PAY-2 | Refund/dispute reverses payment, lapses membership, revokes unused entitlement; idempotent | §29 | §29a–29j | ✅ |
| PAY-3 | Discount/founding code validated for the product **before** checkout opens | §40, FE:Billing | §40, `Billing.test.tsx` | ✅ |
| PAY-4 | Waiver vs paid classification, waived-amount math, reversal lapse-only-if-no-other-live | xU:SettlementTests | `SettlementTests` (17) | ✅ |
| PAY-5 | Admin reconciliation: idempotent reprocess; reversal reason-required; manual-provider-only | FE:Payments, §(recon) | `Payments.test.tsx` | 🟡 (UI + recon API) |
| PAY-6 | Membership term arithmetic + dues webhooks (renew/recert/invoice.paid/subscription.*) | — | Payments.cs (Stripe-object-bound) | ⏳ DEF-3 deferred (integration-layer) |

## Authorization (RBAC / IDOR)

| Req | Requirement | Tests | Evidence | Status |
|---|---|---|---|---|
| AUTHZ-1 | Every privileged admin section is 403 for a viewer lacking it (≈46 sections) | §38, FE consoles | §38a/b, `Payments`/`ExamExceptions` RBAC | ✅ |
| AUTHZ-2 | Cross-user file access (support attachment IDOR) is refused | §30 | §30a–30e | ✅ |
| AUTHZ-3 | Per-certification scope (exam_manager can't reach another cert's artefacts) | §59 | §59 evidence-scope | ✅ |
| AUTHZ-4 | Owner-only surfaces (translations, ops sweeps, IDV) reject viewer admins | §53/§54/§60 | §53/§54/§60 | ✅ |

## Privacy / PII / injection

| Req | Requirement | Tests | Evidence | Status |
|---|---|---|---|---|
| PRIV-1 | Right-to-erasure lifecycle (request→ack→complete→anonymise); queue not student-reachable | §27 | §27a–27l | ✅ |
| PRIV-2 | IDV documents stored metadata-only; raw bytes encrypted at rest, never in JSON; retention purge | §54, xU:—(HonoraryIdv.PurgeExpired) | §54, `PCI.Backend.Tests` purge | ✅ |
| PRIV-3 | Analytics store a rotating visitor hash, never a raw IP | §(analytics) | analytics-events shape | 🟡 |
| PRIV-4 | CSV export neutralises formula-injection triggers (CWE-1236) | xU:CsvTests, §59n | `CsvTests` (18) — SEC-2 fixed | ✅ |
| PRIV-5 | Operator rich-text blocks sanitised (stored-XSS): scriptable subtrees dropped, unsafe URLs/handlers stripped | xU:HtmlSanitizeTests | `HtmlSanitizeTests` (20) | ✅ |
| PRIV-6 | Printable receipt escapes interpolated member data (render-XSS) | FE:print | `print.test.ts` (5) | ✅ |

## Exam integrity & credentials

| Req | Requirement | Tests | Evidence | Status |
|---|---|---|---|---|
| EXAM-1 | Booking/launch blockers isolate each eligibility hold; scheduling gated on holds | xU:LifecycleEligibilityTests, FE:Certifications | `Lifecycle…` (46), `Certifications.test.tsx` | ✅ |
| EXAM-2 | Exam authorization windows: 8-scope precedence; write-through deadline; manual-extension protected | xU:ExamAuthorizationTests | `ExamAuthorization…` (20) | ✅ |
| EXAM-3 | Attempt allowance/grant, reschedule caps, incident reporting | §32, FE:Certifications, FE:ExamExceptions | §32, component tests | ✅ |
| EXAM-4 | Credential issuance (number format, idempotent, wording snapshot) + CPD gate | xU:CredentialCpdTests | `CredentialCpd…` (19) | ✅ |
| EXAM-5 | Suspend/revoke/reinstate gates public verify + download; admin transitions | §31, FE:Credentials | §31, `Credentials.test.tsx` | ✅ |
| EXAM-6 | Scoring pass-boundary / rounding | §2/§9e12/§3 | integration boundary | ⏳ DEF-4 deferred (needs source seam) |
| EXAM-7 | Retake waiting period populated & enforced | pinned finding | `ExamAuthorizationTests` `…IsNeverPersisted` | ⚠️ DEF-2 OPEN (product decision) |

## Transport / headers / CORS / hostile input

| Req | Requirement | Tests | Evidence | Status |
|---|---|---|---|---|
| SEC-H1 | Security headers: nosniff, CSP frame-ancestors none, XFO, HSTS-behind-proxy, COOP, Permissions-Policy, X-Robots-Tag (private only) | §9b8–9b11 (+ existing §9b) | integration §9b | ✅ |
| SEC-H2 | CORS: fixed Allow-Origin, arbitrary Origin never reflected, 204 preflight advertises method/header allow-list | §9b12–9b18 | integration §9b | ✅ |
| SEC-H3 | Hostile-file robustness: watermarker null-not-throw; badge SVG escaping; SVG/HTML MIME refusal + sniffing | xU:PdfWatermarkTests/BadgeSvgTests/StorageTests | SEC-4 classes | ✅ |
| SEC-H4 | Oversized request body rejected before buffering | §9b7 | integration §9b | ✅ |
| SEC-H5 | SSRF egress guard (loopback/RFC1918/metadata/CGNAT/IPv6-mapped) | xU:—(Egress.IsBlockedIp) | `PCI.Backend.Tests` Egress | ✅ |

## Data / migrations / providers

| Req | Requirement | Tests | Evidence | Status |
|---|---|---|---|---|
| DATA-1 | Migrations idempotent on re-run; schema.sql conformance; SQLite↔MySQL parity | migration_integrity_test.py | 191 tables, 0 drift | ✅ |
| DATA-2 | All app-code DB behaviour verified on both SQLite and MySQL | §all ×2 providers | 967/967 each | ✅ |
| PROV-1 | Exam-vendor connectors book→provision→result→credential | §11 (mock vendors) | §11 | 🟡 (mock; CT-3 remaining) |
| PROV-2 | Live provider sandboxes (Stripe/Certuvo/vendors/WhatsApp/Meta/Google) | — | `EXTERNAL_PROVIDER_TEST_PLAN.md` | ⏳ operator |

## Deployment / DR / performance

| Req | Requirement | Tests | Evidence | Status |
|---|---|---|---|---|
| DEP-1 | Health/system-check body + authz; non-destructive prod smoke | §(health), smoke-test.sh | partial | 🟡 + ⏳ |
| DR-1 | Backup produces a restorable dump; restore→boot round-trip | `DR_RESTORE_RUNBOOK.md` | runbook + CI candidate | 🟡 + ⏳ |
| PERF-1 | k6 smoke + thresholds; prod-scale load | — | `EXTERNAL_PROVIDER_TEST_PLAN.md` (I) | ⏳ operator |

## How to keep this current

When a new test lands, add/adjust its row here **and** the module row in `TEST_COVERAGE_MATRIX.md`.
A requirement is only ✅ when it is covered at the appropriate layer and green in CI on both DB
providers where DB behaviour is involved.
