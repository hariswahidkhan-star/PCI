# PCI Platform — Master Verification, Critical Review & End-to-End Audit

**Audit date:** 18 July 2026 · **Branch audited:** `claude/pci-production-readiness-yj8vz8` (PR #52, head `628ec25`) · **Method:** evidence-based — every claim below is backed by a grep, a live API/UI test, a test-suite run, or a CI log. Nothing is marked working because a file exists.

---

## 0. Executive verdict — read this first

**The specification audited against describes a platform this repository does not yet contain.**

The spec requires the **PCI AI Project Leadership Certification Suite** — three live certifications (PCI PCL-AI™, PCI PFL-AI™, PCI PDL-AI™), the portfolio name, and the tagline *"Finance intelligently. Control predictively. Deliver successfully."* A full-repository search (source, schema, seeds, content, config, tests) found:

| Spec term | Occurrences in source |
|---|---|
| PCL-AI / PFL-AI / PDL-AI (and PCLAI/PFLAI/PDLAI) | **0** |
| "PCI AI Project Leadership Certification Suite" | **0** |
| "Finance intelligently. Control predictively. Deliver successfully." | **0** |
| Legacy names the spec says to purge (PFIP, PFIP-AI, CPMD, CPMD-AI, PML-AI) | **0** (clean) |

What the repository actually contains is a **single live certification — PCP-AI** (Certified Project Controls Professional — AI, seeded as certification id 1) — running on a **genuinely multi-certification engine** that I re-proved live during this audit (§3), plus a set of platform modules (documents, watermarking, partner portal, certificates, marketing, support, Certuvo) that are heavily tested and verified.

**Production-readiness decision (as the spec demands, one of four):**

> ### ❌ Not Ready for Production — *against this specification*
>
> The three Leadership Suite certifications, their branding, public pages, and several required modules (books, commission ledger, certificate-template editor, per-certification email templates) do not exist. **Estimated completion vs this spec: ~55–60%.**

**Important nuance the decision must carry:** the platform *as scoped until today* (PCP-AI + all shipped modules) is in strong shape — 378/378 integration assertions on **both SQLite and MySQL**, a 1,131-call zero-500 sweep across all 378 routes, 5/5 CI checks green, live screenshots of every new screen. And the audit's most important structural test **passed**: I created a new certification named "PCI AI Project Controls Leader" (code PCL-AI) purely through the admin API — no code changes — and it propagated automatically to the public catalogue API, the server-rendered public catalogue page, the question bank, credential issuance under its own prefix, and public verification (§3). **The engine can host the Suite; the Suite itself has not been built.**

---

## 0a. Post-audit addendum (18 July 2026, later the same day) — the Suite landed on `main`

The findings in §0 were accurate for the tree audited (`628ec25`). **Hours later, `main` moved forward by 23 commits** (a parallel work stream) that build a large part of the Leadership Suite, and that work has since been **merged into this branch**. The §0 zero-occurrence table is therefore **no longer true of the current tree**. What the merged tree now contains, re-verified by test runs on this branch:

- **The three Suite certifications are seeded**: id 1 renamed in place PCP-AI → **PCL-AI** ("PCI AI Project Controls Leader™"), plus **PFL-AI** and **PDL-AI** rows, with the portfolio name and the "Finance intelligently. Control predictively. Deliver successfully." tagline (`backend/Data/MultiCert.cs`).
- **Credential numbering** moved to `PCI-<PREFIX>-[ROUTE-]<YEAR>-<seq>` with route markers (FND/HON), route-key provenance and per-route certificate wording snapshots (`Core/Lifecycle.cs`).
- **Partner sponsorship + commission ledger** (`/api/partner/candidates`, `/api/partner/commissions`, payouts) and per-certification admin scoping (`admin_users.cert_scope`).
- **Per-certification applications** (admin Applications page) and per-certification documents/books scaffolding (`cert_documents`, now exposed at `/api/me/cert-documents`).

The merge also surfaced (and this branch fixes) defects that arrived with that work — main's own CI was red at its head `e2e7c25`: stale test expectations from the rename (old `PCP-AI` exam-delivery `exam_map` keys, old credential-format and `/api/me` assertions), `smoke-test.sh` probing the deleted classic `/admin.html`, a route collision on `GET /api/me/documents`, and a MySQL boot failure (unindexable `TEXT` columns in the new `certification_applications` table). After those fixes, **this merged branch passes everything main could not**: integration 378/378 on SQLite **and** MySQL, smoke 65/65, founding 46/46, honorary 19/19, honorary-application 20/20, 6/6 logic suites, 0-error 500-sweep (1,173 calls / 392 routes). Remaining Suite gaps (books content, certificate-template editor, per-certification email templates) still stand from §0's gap analysis.

---

## 1. Technology-stack verification (§2 of the spec)

| Layer | Claimed | Verified | Evidence |
|---|---|---|---|
| Frontend | React | ✅ React **18.3.1**, Vite 5.4, react-router-dom 6.26 — two real SPAs (student `/app`, admin `/admin`) + a static public site + static partner portal | `frontend/package.json`; `npm run build` = `tsc --noEmit` + both Vite builds, **passing** (81 admin modules, 71 student modules) |
| Backend | .NET / ASP.NET Core | ✅ **net8.0** minimal-API backend, DI, session auth, RBAC gates, rate limiting, error references, health check, background services (retention, integration dispatcher) | `PCI.Backend.csproj`; `dotnet build -c Release` = **0 errors** |
| Database | MySQL | ✅ MySQL is the enforced production database: **production refuses to boot on a non-MySQL provider** (`Program.cs:276` guard). SQLite exists **only** as the dev/test provider of the dual-provider `Db` layer; parity is *proven*, not assumed — the full suite runs on both | Suite: **378/378 SQLite** and **378/378 MySQL (MariaDB 10.11)**, including in CI (`backend` + `backend-mysql` jobs, both green on `628ec25`) |
| API wiring | React ↔ .NET ↔ MySQL | ✅ Bearer-token JSON APIs; every journey test asserts the DB row after the HTTP call (the suite opens the DB directly and verifies persistence) | `backend/tests/integration_test.py` (378 assertions) |
| Data at rest | No browser-storage/JSON-file business data | ✅ Business data persists in the DB; files in private content-addressed storage (`local:`/`s3:` refs, never raw paths); session tokens in `sessionStorage` (by design, hashed server-side) | `Core/Storage.cs`, `Core/DocStore.cs` |

Not run in this audit (declared, not hidden): ESLint (no lint script is configured — gap), .NET static analyzers beyond compiler warnings (17 benign warnings), dependency-vulnerability scan, WCAG-depth accessibility pass.

## 2. MySQL & migration verification

- **Clean-database path:** exercised on every MySQL suite run — the harness drops and recreates the database, boots the app, `schema.mysql.sql` + `Migrate.Run` apply, then 378 assertions pass. ✅
- **Existing-database path:** all migrations are idempotent (`CREATE TABLE IF NOT EXISTS`, guarded `ALTER TABLE ADD COLUMN`); re-running against a populated DB is a no-op — this is how every local run works. ✅
- Foreign keys ON (SQLite pragma) / InnoDB; unique constraints on the critical idempotency points (webhook event id, entitlement↔payment, credential↔attempt, document assignment, acknowledgement, launch-code hash). ✅
- Transactions wrap multi-step settlement (payment → account → membership → entitlement), with the historical deadlock fix (external calls deferred to post-commit). ✅
- Immutable audit surfaces: `audit_logs`, `certificate_downloads`, `document_downloads`, `impersonation_events`, `exam_delivery_log` are append-only in code. ✅
- ⚠ **Money columns:** amounts use REAL/double in several tables (SQLite heritage) rather than DECIMAL — totals are computed in one place and rounding is applied, but the spec's "no unsafe floating point for money" standard is not met. *Priority: Medium; change: DECIMAL(12,2) on MySQL DDL + decimal in C# money paths.*
- ⚠ **Backup/restore:** `tools/mysql_backup.sh` exists (dump + retention); an actual backup-and-restore drill was **not executed** in this audit. *Not Tested.*

## 3. Multi-certification architecture — the decisive live test (§4)

Executed against a booted server, via the admin API only (transcript: `scratchpad/cert4_test.py`):

| # | Check | Result |
|---|---|---|
| A | Admin creates a brand-new certification (`PCL-AI`, "PCI AI Project Controls Leader", prefix `PCI-PCLAI`, own pass mark 70%, own duration 120 min, own price 425, expiry 3y) — **data only, zero code changes** | ✅ |
| B | It appears automatically in the public catalogue API (`/api/certifications`) | ✅ |
| C | Catalogue price is the certification's own price **through the pricing engine** (effective price after the configured default discount — dynamic as designed; the probe initially expected the raw price) | ✅* |
| D | Question bank accepts content scoped to the new certification (`certification_id`) | ✅ |
| E | A credential is issued **under the new certification** | ✅ |
| F | Public verify (`/api/verify`) shows the new credential with **its** label (`PCI-PCLAI-…`) | ✅ |
| G | The server-rendered public catalogue page (`certification.html`) shows the new certification **without any rebuild** | ✅ |
| H | Deleting a certification with history is **refused** (409 — data-safety guard) | ✅ |
| I | Archiving (deactivate) works | ✅ |
| J | An archived certification disappears from all public surfaces | ✅ |

Combined with the standing suite's multi-certification section (independent entitlements/bookings/attempts/credentials per `certification_id` for one student), this proves the platform is **one engine, N certifications — not copies**. Per-certification scoping is wired through: catalogue, pricing, question bank, entitlements, bookings, attempts, credentials, certificate PDF (title/prefix from the cert record), documents (certification audience), discount scoping (product/route level), Certuvo eligibility, reports (test-user exclusion; per-cert filters partial — see gaps).

**But note §5 below: an engine that *can* host three certifications is not the same as three certifications existing.** Only PCP-AI is live.

## 4. Hardcoding audit (§3 of the spec)

Business configuration is overwhelmingly DB-driven: certifications, pricing, discount codes (full lifecycle + constraints), fee waivers, document categories, page content (every public heading/paragraph is a `page_blocks`/`site_content` row), FAQs/news/resources/BoK/governance, email/notification toggles + recipients, Certuvo config + product rule, exam settings (global + per-cert), SEO/analytics IDs, social links, translation provider. The following true hardcodes were found:

| Value | Location | Assessment | Priority |
|---|---|---|---|
| `https://projectcontrolsinstitute.org` fallback | `Core/CertIssue.cs:21` | Last-resort QR base when `APP_BASE_URL`/`public_base_url` unset. Legitimate fallback, but should come from a required setting in production boot checks | Low |
| `no-reply@projectcontrolsinstitute.org` fallback sender | `Core/Mailer.cs:104` | Same class of fallback | Low |
| `"Certified Project Controls Professional"` fallback title | `Core/CertIssue.cs:37` | Used only if the certification row lookup fails; should fall back to a neutral string, not PCP-AI wording | **Medium** (wrong-brand certificate possible on a data error) |
| `"PCP-AI"` fallback credential label | `Endpoints/AdminMgmt.cs:398`, `Core/Certs.cs:46` | Fallback when a certification has no prefix/code; should be neutral | Medium |
| `"pci-unsub-secret"` fallback HMAC secret | `Endpoints/Campaigns.cs:45` | Unsubscribe tokens fall back to a public default if `NEWSLETTER_SALT`/`FORUM_SALT` unset — forgeable unsubscribe links | **Medium (security)** — require the env var in production |
| Derived credential-encryption key fallback | `Core/Security.cs` | Already documented in-code: production must set `CREDENTIAL_ENCRYPTION_KEY` | Medium (ops checklist) |
| Bootstrap `changeme-owner` / demo student | `Data/Migrate.cs` | First-run only, loudly logged, demo student blocked in Production without explicit opt-in | Acceptable with ops note |
| Exam defaults (90 min / 65%) | `Core/H.cs` | Defaults only — overridden by settings and per-certification values (live test used 120/70) | Legitimate |
| `PCI-HON` award prefix | `Endpoints/Honorary.cs` | Reserved namespace constant, enforced against collisions — legitimate technical constant | Legitimate |
| Credential number format `{prefix}-{year}-{5 digits}` | `Core/Lifecycle.cs:209` | Prefix and year are dynamic; the spec's format is `PCI-XXXX-YYYY-NNNNNN` (6 digits). 🟡 5-digit random vs 6-digit — make width/scheme configurable | Low |
| PCP-AI wording across public HTML | `wwwroot/*.html` | This is **content**, and it is admin-editable at runtime (`page_blocks` captures every text region); the *files* carry PCP-AI seeds. Rebranding to the Suite = content + data work, not code | See §5 |

## 5. Branding verification (§5) — ❌ Not Implemented

Every production-facing surface currently presents **PCP-AI / Project Controls Institute** branding. None of the required Suite branding exists anywhere (0 hits — §0 table). Trademark symbols are not used in any key/identifier (there are none to check — the names are absent). **This entire section of the spec is outstanding work**: 3 certification records + 3 public certification pages + comparison page + portfolio/tagline content + SEO/social metadata + email/document/certificate wording. The mechanism to do all of it without code changes exists (proven in §3 + runtime-editable content), but the work has not been done, and the three certifications' Bodies of Knowledge, eligibility rules, fees, handbooks and exam blueprints are **content that only PCI can author**.

## 6–22. Feature audit table (spec §24 format, condensed)

Statuses: ✅ verified end-to-end · 🟡 partial · ❌ missing. "MySQL" = persistence asserted on MySQL in the suite. Evidence: S = integration suite (378/378 both providers, 18 Jul 2026), C = CI green (`628ec25`), P = live screenshot captured this session, L = live audit test (§3), G = grep/code inspection.

| Feature | UI | API | MySQL | Dynamic | E2E tested | Status | Evidence / Gap |
|---|---|---|---|---|---|---|---|
| Registration → login → profile → portal (Journey A: dup email, pw rules, reset, sessions, rate limits) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S |
| Payment settlement (Stripe-signed webhooks, replay-idempotent, failed grants nothing) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S (test-mode; real gateway keys are ops) |
| Exam lifecycle: booking→proctored attempt→result→released/held→credential | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S (attack paths incl. late/dup submit, foreign items, refund-mid-attempt) |
| Failure scenarios: dup result callback, wrong-cert result, invalid score, missing mapping | — | ✅ | ✅ | ✅ | ✅ | ✅ | S (exam-delivery + lifecycle suites) |
| Multi-certification independence (one student, parallel certs, separate everything) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S + L |
| **New certification without code changes** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | **L (§3, 9/10 + 1 probe error)** |
| Certificates: PDF+QR, tamper hash, public verify + file check, revoke blocks, test isolation, download audit | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S §16 + P |
| Certificate **template upload/editor/field-mapping/preview** | ❌ | ❌ | ❌ | — | — | ❌ | Missing — single built-in design; spec §12 requires template management |
| Certificate replace/correct/reissue/suspend/reinstate | 🟡 | 🟡 | 🟡 | ✅ | 🟡 | 🟡 | Status changes + regenerate exist; formal replace-with-history chain missing |
| Documents module (upload/validate/assign/preview/publish/version/ack/restrict/revoke/audit/CS read) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S §17 (52 asserts) + P |
| Per-recipient **watermarking** (student + institution), master never modified, honest fallback | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S + P (rasterised page) |
| **Books module** (master PDF, editions, per-cert books, personalised copy ID, regeneration queue) | ❌ | ❌ | ❌ | — | — | ❌ | Missing — watermark engine exists and is reusable, but no books schema/UI/lifecycle |
| Partner (institution) portal: login, codes w/ limits, students, notices, **documents tab** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S §14+§17 + P |
| **Marketing-partner commissions** (rates, basis, Due→Partial→Paid, payment proof, reversal, statements) | ❌ | ❌ | ❌ | — | — | ❌ | Missing — attribution exists (partner-linked codes + redemptions + discount reports); no commission ledger |
| Discount codes: lifecycle, approval, constraints, country/scope limits, fraud queue, reporting | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S §13/§14. 🟡 per-**certification** restriction is product/route level, not per-cert-id |
| Marketing dashboard: KPIs, sources, conversions, campaigns (preview/test/send/suppression) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | P + code; campaign send E2E has no automated suite section (manual-tested) 🟡 |
| Routes: Standard / Founding (codes, caps, windows, applications, approval) / Honorary (public form→board→account+membership+cert, never claims exam) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S + founding 46/46 + honorary 19/19 suites |
| Sponsored route (employer/government invoicing) | 🟡 | 🟡 | 🟡 | ✅ | 🟡 | 🟡 | Institution sponsorship via codes + full-sponsorship flag + mark-paid works; no sponsor-invoice document flow |
| Complimentary / waived (full+partial waiver, mark-paid w/ evidence, reversal, waiver ledger) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S §13 |
| Certuvo: PCI-owned usernames, encrypted temp passwords, conflict handling, retry, statuses, webhook | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S §15 (26 asserts, mock vendor). 🟡 product mapping is single-product; per-certification product map needed for the Suite |
| Email/notifications: provider seam, logs, ledger, per-event toggles, recipients, unsubscribe compliance | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S. ❌ per-certification **template manager** (templates are files + settings, not per-cert DB records) |
| Workflow / journey viewer + current blocker + fix-now + error references | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S §12/§14. 🟡 stages are code-defined (cert-agnostic), not admin-configurable |
| RBAC: owner/roles/custom perms, section gates on every admin route, portal isolation (admin/student/partner), impersonation ledger, TOTP | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | S (per-role probes; students blocked from admin APIs; partner/institution isolation; CS cannot upload) |
| Security: sig-verified webhooks, upload sniffing+size+malware seam, no storage paths, hashed tokens, no viewable passwords, headers, rate limits, deny-by-default settings | — | ✅ | ✅ | ✅ | ✅ | ✅ | S attack sections + 500-sweep (1,131 calls, 0 5xx) + prior 50-reviewer audit fixes |
| Public site: 213+ pages, SSR catalogue/pricing/lists, i18n, SEO/sitemap/robots/llms.txt, mobile passes | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | Prior crawl/sweeps + S. ❌ the three Suite cert pages/comparison don't exist |
| Reports & exports: finance, discounts, analytics, CSV, audit logs | ✅ | ✅ | ✅ | ✅ | 🟡 | 🟡 | Work; per-certification filter dimension incomplete |
| Performance/reliability: indexes on hot paths, batched sends, retry queues, watermark ≤25MB bound | — | — | — | — | 🟡 | 🟡 | Sweep + suites only; no formal load test |

## 7. Defect register (from this audit)

| ID | Severity | Finding | Fix |
|---|---|---|---|
| D-1 | **Critical (vs spec)** | The three Suite certifications, portfolio branding, tagline, public pages, BoKs, fees, blueprints do not exist | Author as data/content (mechanism proven §3) — requires PCI's content decisions |
| D-2 | High (vs spec) | Books module absent | New module (schema+admin+student+tests); watermark engine reusable |
| D-3 | High (vs spec) | Marketing-commission ledger absent (calc, Due/Partial/Paid, proof upload, reversal, statements) | New module on top of existing attribution |
| D-4 | High (vs spec) | Certificate-template management absent | New module (upload, mapping, versioned preview) |
| D-5 | Medium | Per-certification email-template manager absent | Move templates to DB keyed by (event, certification_id) with fallback |
| D-6 | Medium | Money stored as REAL not DECIMAL | DDL + code pass |
| D-7 | Medium (security hardening) | Unsubscribe-token fallback secret; credential-key derivation fallback | Enforce env vars at production boot |
| D-8 | Medium | PCP-AI-flavoured fallback strings (`CertIssue.cs:37`, `AdminMgmt.cs:398`, `Certs.cs:46`) | Neutral fallbacks |
| D-9 | Low | Credential number 5-digit random vs spec 6-digit | Configurable width/scheme |
| D-10 | Low | Discount restriction lacks certification-id dimension | Add `certification_id` to code constraints |
| D-11 | Low | No ESLint config; formal a11y/load tests absent | Tooling additions |

(For completeness: the only defect *found and fixed during* this audit window was CI lacking `pypdf` — diagnosed from CI logs, fixed, and re-proven green the same day.)

## 8. What this audit did NOT execute (declared honestly)

Real-money Stripe charges (test-mode signatures only), the real Certuvo vendor (documented mock), real SMTP delivery (console/Resend seam + logs), a production backup/restore drill, formal load testing, WCAG-depth accessibility, and — impossibly — the PCL/PFL/PDL journeys, since those certifications don't exist yet.

## 9. Path to "Ready for Production" against this spec

1. **Suite data & content (D-1)** — 3 certification records (proven trivial) + the real content: pages, BoKs, eligibility, fees, handbooks, exam blueprints, SEO. *Blocked on PCI's content; platform-side effort small.*
2. **Books module (D-2)** — schema (`books`, `book_editions`, `book_copies`), admin upload/eligibility, per-student personalised copy via the existing watermark engine, regeneration/status, tests. *~1 focused phase.*
3. **Commissions (D-3)** — rates/basis/terms on partners, ledger rows on attributed payments, Due→Approved→Partial→Paid with proof uploads, reversal on refund, statements, partner-dashboard views, tests. *~1 focused phase.*
4. **Certificate templates (D-4)** + replace-chain depth. *~1 phase.*
5. **Hardening batch (D-5…D-10)** — smaller, can ride along.
6. Re-run this audit's full matrix, including three complete Suite journeys.

---

*All numeric results in this report are from runs executed on 18 July 2026 on this branch: integration 378/378 (SQLite) + 378/378 (MySQL), founding 46/46, honorary 19/19, storage 10/10, lifecycle/casework/settings/release/publication suites green, 500-sweep 1,131 calls / 378 routes / 0 server errors, CI 5/5 green, live screenshots: admin documents, upload drawer, student documents (desktop+mobile), marketing dashboard, partner documents tab, rasterised watermarked page.*

---

# Part 2 — Post-merge audit of the Suite modules (18 July 2026, evening)

Continuation of the audit on the merged tree (PR #52 head), covering the modules the parallel main
work stream added and the naming-spec sections that needed functional verification. Method unchanged:
every verdict is backed by a code citation, a live API run against a freshly seeded server, a test
run, or a screenshot. Two read-only exploration passes mapped the subsystems; every load-bearing
claim below was then re-confirmed against the source or exercised live.

## 2.1 Marketing-partner sponsorship & commission ledger — ✅ working, with model deviations

**Live end-to-end run (fresh DB):** created partner "Audit Academy" (20% commission, sponsorship
enabled) → partner finance user → partner-linked code `INST-0321D3A0` (25%) → student paid a
PFL-AI exam with the code (Stripe-shaped signed webhook, USD 262.50) → redemption attributed →
ledger showed **attributed revenue 262.50 / accrued 52.50 / balance 52.50** identically on the
partner portal and the admin drawer → admin recorded a USD 52.50 payout (audited
`partner_payout_recorded`) → balance 0, payout visible partner-side. Screenshot: partner portal
Commissions tab. Sponsorship: partner sponsored a PDL-AI candidate → account created + approved
`sponsored` application (`PCI-APP-…`) + sponsor-funded entitlement + in-app notification + live
progress row (`Partners.cs:92-167`), unique per (partner, candidate, certification).

**Deviations vs the naming spec §14 (by design, documented in `Migrate.cs:123-127`):**
- Commission is a **derived running balance** (paid redemptions × `commission_pct` at read time,
  `Partners.cs:35-63`) — there are **no per-transaction commission records** with
  Due → Partially Paid → Paid statuses. "Due" exists only as `balance > 0`.
- **Payment proof is a free-text payout note**, not a file upload (`Partners.cs:194-197`).
- **The ledger is not filterable by certification** (no `certification_id` in the query,
  `Partners.cs:39-49`) and one `commission_pct` applies across all three credentials. Candidate
  tracking IS per-certification (`Partners.cs:68-77`).

One earlier probe artefact worth recording: passing the discount code under the wrong webhook
metadata key silently skips attribution — the real checkout always sends `discount_code`
(`Payments.cs:209-219`), which correctly records the redemption, increments `used_count`, and runs
fraud checks.

## 2.2 Certification applications & routes — ✅ working end-to-end

All **eight routes** are seeded per certification (`MultiCert.cs:204-214`): standard, founding,
honorary, sponsored, complimentary, waived_full, waived_partial, test (internal). Live run:
`GET /api/certifications/2/routes` returned the seven public routes; a student submitted a
**standard** PFL-AI application (`PCI-APP-2026-100001`, status `submitted`); the admin list showed
it (cert-scope filtered, `Applications.cs:63`); admin approved; the student saw `approved`. Free
and sponsored routes auto-grant a paid-at-zero entitlement on approval; fee-bearing routes
correctly do not (`Applications.cs:99-111`). Route provenance is stamped on the entitlement and the
credential wording snapshot (`Migrate.cs:515-517`). Admin UI: Applications page with certification/
status filters and a review drawer (approve / reject / request info / under review). Student UI:
apply + track inside the portal Certifications page.

## 2.3 Books / per-certification documents — ⚠️ partial

Working and live-verified: `cert_documents` schema (kind handbook|bok|study_guide|book|…,
per-certification, per-route, `watermark` flag), admin CRUD at `/api/admin/cert_documents`
(perm `resources`, **cert-scope enforced**, `AdminMgmt.cs:259`), per-cert Candidate Handbook + Body
of Knowledge rows seeded for every certification (`MultiCert.cs:185-200`), and the student endpoint
`GET /api/me/cert-documents` correctly **isolating by entitlement/credential** — the live probe's
PFL-AI student saw the PFL-AI book, the seeded PFL-AI handbook/BoK and the general guide, and never
the PDL-AI handbook.

**Gaps:** (1) the `watermark` flag on cert documents is **inert** — rows are URL passthrough; no
download endpoint stamps books (real watermarking exists only in the assigned-documents module,
`Documents.cs:616-624`); (2) **no UI consumes the endpoint yet** — neither portal renders books, and
there is no admin Books page (API + seeds only).

## 2.4 Certuvo per-certification mapping — ❌ schema/UI only (dead code)

`certifications.certuvo_product` and per-cert `certuvo_enabled` exist (`Migrate.cs:533`), are
admin-editable (`Certifications.tsx:150`), and are backfilled to the cert code — but **nothing
reads them**: provisioning is global and membership-driven (`Provisioning.cs:109-118, 321-337`,
gated on the global `certuvo_enabled` setting), and the provision request carries no
certification/product field. Spec §16 is not met in behaviour.

## 2.5 Emails & notifications — ⚠️ partial, one violation found & fixed

The file-based template engine substitutes `{{VARS}}` (`Mailer.cs:40-57`) but has **no
`{{certification_name}}` variable**, no DB-backed template editor (the admin "Emails" page is a
log viewer), and three shipped templates (`exam-confirmation`, `payment-confirmation`,
`credentials`) are **dormant** — no code path sends them. Code-composed notifications that name a
certification are dynamic (e.g. `Applications.cs:44` uses the cert row's acronym). **Found during
this audit and fixed on this branch:** the *live* welcome email and two other templates still said
"PCP-AI" (8 occurrences across `backend/emails/*.html`) — the naming sweep had missed this
directory; all templates are now certification-neutral.

## 2.6 Per-certification admin scoping (`cert_scope`) — ✅ implemented and enforced

Owner-managed via Team & Access (`Team.tsx:40-57`, persisted `Program.cs:560-607`, owners forced
unrestricted). Enforced server-side via `CanCert`/`CertFilterSql` (`Auth.cs:13-36`) on: generic
cert-scoped CRUD (question bank, cert documents), certification edits, proctoring/session lists,
credential release, the applications list + decisions, and both per-certification report blocks
(`AdminExtra.cs:156,160`).

## 2.7 Recertification & reports — ⚠️ one defect found & fixed; reports per-cert ✅

Issuance honours per-certification `expiry_years` (`Lifecycle.cs:203,237`). **Defect found by this
audit:** the recert webhook branch extended credentials by a **hardcoded 3 years** regardless of the
certification's cycle (`Payments.cs:204`) — a 2-year credential would silently gain 3. **Fixed on
this branch**: recert now extends by the paid certification's own `expiry_years` (falls back to 3),
and still never resurrects a revoked credential (regression R10 passing). Admin reports break down
revenue and issued certificates **by certification**, cert-scope filtered (`AdminExtra.cs:153-160`,
rendered in Reports.tsx); AdminAnalytics has no per-cert breakdown (site analytics only).

## 2.8 Updated verdict

With the Suite merged and the defects above fixed, the platform now covers substantially more of
the specification than Part 1's ~55-60%: the three certifications are live together end-to-end
(section-18 regression: one candidate, three credentials, zero leakage), applications/routes,
sponsorship + commissions, per-cert scoping, documents/watermarking (assigned-documents module) and
reports all verify. **Estimated completion vs the Suite specification: ~80%.** The remaining
material gaps, in priority order:

1. **Books delivery** — a download endpoint for `cert_documents` that applies the existing
   PdfWatermark engine (the flag is stored but unused) + portal/admin UI.
2. **Certuvo per-certification products** — read `certifications.certuvo_product`/`certuvo_enabled`
   in the provisioning path; today the stored mapping has no effect.
3. **Commission ledger statuses** — per-transaction commission records (Due → Partially Paid →
   Paid), payment-proof upload, per-certification filtering, if the aggregate-balance model is not
   acceptable to Finance.
4. **Email templates** — `{{certification_name}}`-family variables + wiring the dormant
   exam/payment/credential templates (or a DB-backed template editor).
5. **Certificate-template editor & digital badges** — unchanged from Part 1's gap list (route
   wording is admin-configurable per route; a visual template editor and standalone badge artifacts
   are not built).

*Numbers for Part 2: integration 395/395 on SQLite AND MySQL after the fixes (includes the new
section 18 Suite regression); live probes and screenshots as described; CI green through commit
`92e6170`, later commits pending at the time of writing.*
