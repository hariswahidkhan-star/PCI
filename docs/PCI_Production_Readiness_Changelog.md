# PCI Platform — Production-Readiness Changelog

Work against the production-readiness specification. This session focused on the **core result-publication
lifecycle (Section A)**, **consents + eligibility**, **formal entitlements/idempotency**, the **lifecycle
aggregate**, **rate limiting**, and the **legal copy** — implemented and verified against a real .NET 8 SDK
(compile) and real SQLite (logic). Per the spec's own rule ("any feature not fully implemented must be
clearly disabled or labelled, not shown as working"), deferred items are listed honestly at the end rather
than stubbed to look functional.

Because much of Phase 1 was completed and verified in earlier sessions, it is summarised (not re-done) below.

---

## Already delivered in earlier sessions (verified; summarised)
- **RBAC:** every admin route section-gated; deny-by-default; owner-only for team/settings. (Exhaustively tested.)
- **Tokens hashed:** setup, reset, enrolment-resume, and desktop launch codes are SHA-256 at rest.
- **Legacy admin token removed** from auth and CORS.
- **Stripe webhook:** wrapped in a transaction; idempotency-first (payment `INSERT OR IGNORE` before side effects).
- **Enrolment resume/save** requires a valid hashed token (email alone cannot resume).
- **Desktop exam auth** redesigned: launch code → short-lived exam session token exchange (fixed a critical
  401-on-every-call bug); API host pinning in the client.
- **Practice/live split** via `is_practice`; **no answer keys** in any student payload; **server-side timing**
  on start/authorize/heartbeat/submit; **late submit never issues a credential**.
- **Duplicate pricing seeds** fixed (idempotent); **checkout wired to backend pricing**.
- **A critical `Db.Transaction` bug** (commands not associated with the transaction → every webhook 500) found
  and fixed; **submit/heartbeat TOCTOU** closed.

---

## This session — implemented and verified

### A. Result-publication lifecycle (the core business rule)
- **Schema:** `exam_attempts` gained `result_status` (default `not_started`), `hold_reason`, `released_at`,
  `answer_key_version`, `bank_version`. New tables: `exam_score_snapshots` (immutable, unique per attempt),
  `exam_entitlements`, `candidate_consents`, `webhook_events`, `security_events`. All added idempotently in
  both `schema.sql` and `Migrate.cs` (existing databases are upgraded without data loss).
- **`Core/Lifecycle.cs`** centralises the rules so every route applies them identically:
  - `AutoHoldReason(...)` returns `null` for a **clean** attempt or a machine reason
    (`submitted_after_deadline`, `booking_missing`, `payment_reversed`, `account_hold`,
    `critical_proctor_violation`, `identity_failed`, `duplicate_attempt`).
  - `ReleaseStatus(...)` maps a scored attempt to `released_pass` / `released_fail` / `auto_held`.
  - `WriteScoreSnapshot(...)` writes the immutable score record (idempotent via a unique index).
  - `BookingBlockers(...)` and `OutstandingConsents(...)` drive eligibility.
  - `BuildLifecycle(...)` produces the dashboard lifecycle object.
- **Submit endpoint** now: scores server-side → evaluates auto-hold → sets `result_status` → writes the
  immutable snapshot → **releases immediately for clean attempts** and **auto-issues a credential only for a
  clean pass** (moving `result_status` to `credential_issued`). Held attempts return a hold message and do
  **not** disclose pass/fail. Entitlement is consumed on submit.
- **Verify:** `python3 lifecycle_test.py` — consents gate (7/7 outstanding blocks; 0 after accept), clean pass
  → `released_pass` + credential, critical proctor → `auto_held`, late → `auto_held` no credential, payment
  reversed → `auto_held`, immutable snapshot preserved under re-write, webhook ledger idempotent. Entitlement
  chain (available → booked → consumed, then re-book blocked) proven in a dedicated test.

### B. Consents, eligibility, lifecycle object
- **`GET/POST /api/me/consents`** (versioned acceptance with IP/UA); **`GET /api/me/results/{id}/report`**
  (held attempts do not reveal pass/fail).
- **Booking endpoint** now calls the eligibility gate (`BookingBlockers`) — blocks on `consents_pending`,
  `profile_incomplete`, `payment_not_valid`, `entitlement_expired`, `account_hold`.
- **`/api/me`** now returns a `lifecycle` object (`membership_status`, `candidate_status`, `exam_status`,
  `result_status`, `credential_status`, `next_step`, `blocking_items`) and a `consents` summary, so the
  front-end can drive off backend state instead of hardcoded values.

### E/H. Payments & idempotency
- **`webhook_events`** ledger recorded before side effects (belt-and-braces with the payment unique index);
  a replayed event id is ignored.
- **`exam_entitlements`** created when an exam/bundle payment settles (unique per payment), linked to the
  booking at book-time and consumed at submit — enforcing one attempt per entitlement.

### F. Auth hardening
- **Rate limiting** (fixed-window, 10/min/IP, `429` + `Retry-After`) via path middleware on `/api/login`,
  `/api/admin/auth/login`, `/api/forgot-password`, `/api/validate-code`, `/api/set-password`, and
  `/api/exam/authorize`. (Implemented as middleware after an initial per-route attempt proved fragile.)

### G. Legal copy (registered-nonprofit update)
- Across **213 pages**: PCI is now stated as a **registered nonprofit organisation pursuing 501(c)(3)
  recognition (not yet granted)**; donations are **not** represented as tax-deductible; **not** ISO/IEC 17024
  accredited. Replaced "certificate appears instantly" with the integrity-check wording, and the
  membership/credential renewal wording with the compliant version. British English; ld+json still valid;
  0 broken internal links after the edit.

---

## Deferred — NOT implemented this session (documented honestly, not stubbed as working)
These require substantial additional work and are scoped as follow-ups. Where a UI entry exists it should be
labelled "coming soon" rather than shown as functional:
- **Full LMS / learning progress** (Section B14) — real lesson/quiz tracking.
- **Certificate PDF generation & document vault** (B5, B12) — server-side certificate rendering + downloads.
- **Accommodations** (B8) and **appeals/complaints** (B10) workflows — tables/UX/decisioning.
- **CPD evidence upload + audit** (B7) — attachments and review states.
- **Readiness/system checks capture** (C9) — the `exam_readiness_checks` table exists conceptually but the
  browser capture + gating UI is not built.
- **TOTP 2FA** (F3) — should remain a "coming soon" label, not an active toggle, until implemented.
- **Separate `exam_questions` bank model** (C1) — the current `is_practice` split is in place and safe; the
  richer bank/versioning model is a larger migration.

---

## Verification summary (this session)
- Backend compiles **0 errors / 0 warnings** (.NET 8.0.128) with the transaction-guard-enforcing stub harness.
- `SecureExam.Core` compiles.
- **265** SQL queries valid against the schema; migrations idempotent; all `.cs` brace/paren-balanced.
- App-shell JS (`student`, `admin`, `exam-ui`, `checkout`) all pass `node --check`.
- Lifecycle, consents, entitlement, and idempotency behaviours tested against **real SQLite**.

## How to verify locally
1. `cd PCI.Backend && dotnet build` (with packages restored, or use the CI workflow which restores + boots).
2. Run `lifecycle_test.py` against `schema.sql` to reproduce the Section-A checks.
3. Boot and exercise: accept consents → book → sit → submit; confirm a clean pass shows immediately with a
   credential, and a forced late/critical-violation attempt shows the hold message with no pass/fail.

---

# PHASE 1 (of 3) — Result-lifecycle loop completed end-to-end

The previous session created the auto-hold system but left it with **no release path** — a held candidate
was stuck forever. Phase 1 closes that loop and fixes four real bugs found in the process.

## Bugs found and fixed
1. **Admin "review" never touched the lifecycle.** Clearing a held attempt updated only the legacy
   `review_status`; the student stayed on the hold screen permanently and a released held pass never issued
   a credential. The endpoint is rewritten around `result_status`/`released_at`.
2. **Hardcoded pass mark 65 in reinstate.** With the pass mark configured to 70%, a 68% attempt would have
   been wrongly reinstated as a pass. Now uses the configured `exam_pass_mark_pct`. (Proven by test.)
3. **Audit actor recorded as `0`.** Result actions now log the acting admin's real id.
4. **Held results leaked server-side.** `/api/me` and the account-data export returned `percent`/`result`
   for auto-held attempts (and the export leaked `item_ids` bank metadata via `SELECT *`). Both are now
   redacted server-side with explicit column lists.
5. **Verify endpoint reported expired credentials as active.** `/api/verify` now computes an expiry-aware
   `state` and `valid` flag; a credential past `expires_at` is never reported valid.
6. **Consents gate had no UI.** The booking eligibility gate added last session would have blocked every
   real user with no way to accept the agreements. The portal now has a full consent flow.

## What was built
- **Admin result management** (`/api/admin/exam-sessions/{id}/review`, RBAC `proctoring`): new `release`
  action (held → released_pass/released_fail; issues the credential on a released pass), lifecycle-aware
  `invalidate` (revokes the attempt-linked credential; `result_status` → `credential_revoked`) and
  `reinstate` (configured pass mark; reactivates or issues the credential). All actions audited with the
  real admin id.
- **Credential ↔ attempt linkage:** `issued_credentials.attempt_id` + partial unique index — at most one
  credential per attempt, enforced by the database (spec constraint H). Shared `Lifecycle.IssueCredential`
  helper used by both submit and admin release (idempotent: re-release returns the same credential).
- **Admin UI (proctoring):** "Held" badges with hold-reason tooltips, a Held KPI, release-status and
  hold-reason rows in the session drawer, and a **Release held result** button; hardcoded "pass ≥65%" text
  removed.
- **Student portal:** held submissions show the integrity-hold screen (no pass/fail, no 0% dial) in both the
  live result screen and the Results view; attempt history marks held attempts "Under review".
- **Consent flow:** an agreements modal (all 7 policies, v1.0, single acceptance) shown proactively on the
  booking card and reactively when booking returns `not_eligible`; other blockers map to readable reasons
  (profile incomplete, payment invalid, window lapsed, account hold). Demo mode mirrors the full flow.
- **Certificate wording** in the portal corrected to the integrity-check version ("appears here instantly"
  removed — the earlier site-wide edit had not covered the app shells).
- **Desktop client:** `SubmitResult` gained `Held`/`Message`/`ResultStatus` (additive; Core compiles) and
  `ExamFlow.ResultHeld`, so a held desktop submission is not rendered as a misleading 0% result. The WPF
  UI branch itself is Windows-only and not compile-verifiable here — flagged for Phase-2 desktop polish.

## Verification
Backend **0 errors / 0 warnings**; **271** SQL queries valid; new portal calls contract-checked against
backend routes; `student.html`/`admin.html` pass `node --check`; and two SQLite test suites pass:
the Section-A lifecycle suite plus a new release-loop suite (held pass → release → credential; idempotent
re-release; invalidate → revoke; reinstate honours the configured pass mark; expiry-aware verify).

## Phase 2 / Phase 3 (planned)
Phase 2: certificate & score-report documents, appeals, accommodations, CPD evidence, support attachments,
admin student-360 result actions, desktop held-result UI. Phase 3: readiness checks, honest 2FA labelling,
sp_* settings, mobile card layouts, full smoke suite + CI wiring.

---

# PHASE 2 (of 3) — Documents & workflows

Appeals, accommodations (with a real effect on exam duration), CPD evidence + review, support-ticket
attachments, and the certificate document flow — implemented end-to-end (schema → endpoints → both UIs)
and verified against real SQLite.

## New database objects (idempotent in schema.sql + Migrate.cs; no data loss)
`appeals` (result/invalidation appeals, complaints, ethics — one open appeal per attempt),
`accommodation_requests` (typed requests; `approved_extra_minutes`), `support_attachments`
(size-capped data-URI files on tickets), and `cpd_entries` + evidence/review columns
(`evidence_name/evidence_data/admin_note/reviewed_by/reviewed_at`).

## Backend (`Endpoints/Casework.cs`, compiled 0/0)
- **Uploads validated server-side everywhere:** PDF/PNG/JPEG/WebP only, ~1.5 MB cap, filename sanitised —
  proven by tests (exe rejected, oversized rejected, PDF accepted).
- **Appeals:** `POST/GET /api/me/appeals` (ownership of the referenced attempt/credential enforced; one open
  appeal per attempt); admin `GET /api/admin/appeals`, `POST .../{id}/decide` (under_review/upheld/dismissed
  with a written decision), evidence viewer — all gated `tickets`, all audited with the real admin id.
- **Accommodations:** `POST/GET /api/me/accommodations` (one open request at a time); admin decide with
  0–120 extra minutes + note. **The approval is real:** `Lifecycle.ApprovedExtraMinutes` (largest single
  approval, rejected requests ignored) now extends `duration_minutes` at **both** launch points — browser
  `start` and desktop `authorize` — proven 90→120 min in tests.
- **Support attachments:** `POST/GET /api/me/tickets/{id}/attachments` (ownership enforced, 10-file cap,
  bumps ticket `updated_at`); stored as data URIs consistent with the existing evidence pattern.
- **CPD:** `POST /api/me/cpd/{id}/evidence` (attaching evidence returns the entry to the review queue);
  admin `GET /api/admin/cpd?status=`, `POST .../{id}/review` (approve/reject + note, actor recorded) —
  gated `members`. The CPD list no longer ships bulky `evidence_data` with every row.
- **Certificate data:** `GET /api/me/certificate` — expiry-aware `state`/`valid` (same rule as public
  verify), registration number, verify path.

## Admin panel
- **Tickets view** gains two live panels: **Appeals & complaints** (mark reviewing / uphold / dismiss with a
  written decision; evidence viewer; note that releasing/invalidating the underlying result stays in
  proctoring) and **Accommodation requests** (approve with minutes + note / reject with reason; approved
  minutes labelled as automatically applied).
- **Members view** gains the **CPD review queue** (entries awaiting review, evidence viewer, approve /
  reject-with-reason).

## Student portal
- **Request an accommodation** from the booking card (type, description, optional evidence); live status
  callouts ("under review — consider waiting", "approved +X minutes applied automatically").
- **Appeal an invalidated result** from Results (grounds + optional evidence); the invalidated/revoked state
  now renders its own card with the appeal's live status and the recorded decision.
- **Ticket attachments:** 📎 upload + downloadable attachment chips in the ticket drawer.
- **CPD:** per-entry review badges (In review / Approved / Rejected-with-note) and an evidence attach button.
- **Certificate:** authoritative state check — a **revoked** credential now shows a revocation notice instead
  of a printable certificate; new **Download PDF** generates a branded A4 certificate via pdf-lib (holder,
  credential ID, registration no, issue/expiry, verify URL) alongside the existing print flow.
- **Score report** now merges the backend report (registration no, unanswered count, integrity events,
  release time) and the hardcoded "pass mark 65%" is gone.
- Demo mode mirrors every new flow; `pickFile` enforces the same type/size rules client-side.

## Verification
Backend **0 errors / 0 warnings**; **302** SQL queries valid; **17/17** new-route contract checks pass;
all shells pass `node --check`; and the three suites — now shipped in **`tests/`** inside the backend zip
with a README — pass **10/10, 8/8, 15/15** against real SQLite (the one prior "failure" was a test-setup
artifact, fixed by modelling the real webhook→book→consume chain).

## Still open for Phase 3
Readiness/system checks, honest 2FA labelling, `sp_*` settings enforcement, mobile card layouts,
payments-page cards, desktop WPF held-result screen (DTO shipped in Phase 1; XAML is Windows-only),
smoke-suite/CI wiring.

---

# PHASE 3 (of 3) — Hardening, honesty & CI lock-in

The final layer: enforce the admin portal settings that were previously display-only, add real pre-exam
readiness checks, stop the 2FA toggle from lying, tidy mobile UX, and wire everything into CI so it can't
silently regress.

## Bugs / gaps found and fixed
1. **`sp_*` settings were decorative.** The portal exposed `sp_exam_booking_open`, `sp_results_visible`,
   `sp_practice_enabled`, `sp_support_tickets_enabled`, `sp_certificate_download` etc., but **no backend
   endpoint enforced them** — a client could bypass a "closed" booking window or "hidden" results. They are
   now enforced server-side (booking blocker + 403s on the relevant routes) and obeyed by the UI.
2. **The 2FA toggle was dishonest.** It set `two_factor_enabled=1` and displayed "Enabled — codes at
   sign-in", but **nothing challenged at login** — a candidate was told they were protected when they were
   not. Now `/api/me/2fa` returns `coming_soon` (persists no misleading state) and the security page shows a
   plain "Coming soon" tag with no fake toggle.
3. **Readiness checks didn't exist.** The old "System check" explicitly avoided camera/microphone, yet a
   proctored exam needs them. There is now a real readiness probe and a launch gate.

## What was built
- **Readiness checks (Section C9):** `exam_readiness_checks` table; `GET/POST /api/me/readiness`. The portal
  actually probes camera, microphone, network, screen and fullscreen (camera/mic tracks are requested only to
  confirm availability and stopped immediately — nothing recorded), then records the outcome. When
  `sp_readiness_required` is on, **launch is blocked** on both browser `start` and desktop `authorize` until a
  passed check exists (`Lifecycle.ReadinessSatisfied`). Camera + microphone + network are all mandatory for a
  pass. Proven by tests (failed check does not satisfy; passed check does; not-required ⇒ always allowed).
- **Settings enforcement:** `sp_exam_booking_open` → `booking_closed` blocker; `sp_practice_enabled`,
  `sp_results_visible`, `sp_support_tickets_enabled` → 403 on their routes; `sp_certificate_download` disables
  the certificate download; `sp_banner_enabled`/`sp_banner_text` render a site banner; disabled sections
  (CPD, Support) are hidden from the portal nav. `/api/me/config` now also reports `sp_readiness_required`.
- **Honest 2FA:** coming-soon everywhere; no state that could display as active.
- **Mobile UX:** the payments table becomes a **card layout** under 640px (invoice, product, date, method,
  amount, status — tappable to the invoice drawer); the readiness panel and callouts are mobile-friendly.
- **CI lock-in:** `.github/workflows/build.yml` now runs, on every push/PR: real restore+build, the **four
  logic suites** (fails on any `FAIL`/✗), a **JS syntax gate** for all six app shells, then boots the real
  backend and runs the live smoke suite. The four suites live in `tests/` with a README.

## Verification (all three phases, final)
Backend **0 errors / 0 warnings** (.NET 8.0.128); **304** SQL queries valid; **45 assertions across 4 SQLite
suites pass with 0 failures** (lifecycle 10, release 8, casework 15, settings 12); all **six** app shells
pass `node --check`; new readiness routes registered; CI extended to gate all of it.

## Honest status at end of Phase 3
Delivered and verified across the three phases: immediate result lifecycle with auto-hold + admin
release/invalidate/reinstate; consents + eligibility; formal entitlements + webhook idempotency; MCQ 4-field
+ bulk upload; reviews; watermarked downloads; appeals; accommodations (with real exam-duration effect);
CPD evidence + review; support attachments; certificate document + PDF; readiness checks; server-side
enforcement of portal settings; honest 2FA; mobile card layouts; registered-nonprofit legal copy; and CI
that locks it in.

**Genuinely still not built (documented, not faked):** full TOTP 2FA (labelled coming soon, by design);
the richer separate `exam_questions` bank/versioning model (the `is_practice` split is in place and safe);
server-side certificate PDF/blob storage (generation is client-side; attachments are size-capped data URIs);
a full LMS (learning progress remains roadmap-marked); and the desktop WPF held-result **screen** (its DTO
contract shipped in Phase 1; the XAML is Windows-only and not compile-verifiable in this environment). These
are the honest remainder — each is a real piece of work, not a checkbox, and none is presented as functional
where it isn't.
