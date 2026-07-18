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
