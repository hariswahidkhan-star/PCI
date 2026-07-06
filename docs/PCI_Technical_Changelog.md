# PCI Platform — Technical Production-Readiness Changelog

Technical-only audit and fixes, delivered in phases. This is **Phase 1**: it makes immediate, automatic
result publication the true default (proctoring/identity become audit-only unless explicitly configured),
and adds production configuration validation. Verified against a real .NET 8 SDK (compile) and real SQLite.

Note on prior work: many items in the technical spec (RBAC gating, token hashing, webhook transaction +
idempotency, legacy-token removal, enrolment resume-token security, desktop launch-code auth + API pinning,
practice/live answer-key separation, server-side timing on all four exam routes, one-payment-one-booking,
idempotent pricing seeds, checkout↔backend pricing, DB constraints/indexes, readiness checks, rate limiting)
were implemented and test-covered in earlier sessions; the four existing suites still pass and are unchanged.

## Phase 1 — Immediate result publication is the default (matches the exam-result rule)

### The gap
The result-lifecycle previously **auto-held** on a critical proctoring violation and on a failed identity
check *by default*. The required behaviour is the opposite: a technically valid result must publish
immediately, proctoring/identity evidence must be **audit-only**, and results may be blocked **only** for
technical-invalidity reasons — with any proctoring-based blocking being **opt-in via settings** and still
fully automatic (never a manual review).

### Fixes
- **`Core/Lifecycle.AutoHoldReason` reworked.** It now blocks **only** on technical invalidity:
  `submitted_after_deadline`, `booking_missing`, `booking_invalid` (cancelled/missed/expired),
  `payment_reversed`, `duplicate_attempt`. Wrong-user and already-submitted are enforced at the endpoint
  before scoring. Proctoring/identity are evaluated **only if** the corresponding setting is enabled.
- **New configurable hard rules, all OFF by default** (seeded in `schema.sql`, honoured by the code):
  `auto_block_result_on_tampered_attempt`, `auto_block_result_on_critical_violation`
  (+ `critical_violation_threshold`), `auto_block_result_on_identity_fail`. With these off, a critical
  violation or failed identity check does **not** delay publication — the event is still stored for audit.
- **Item-set integrity added to submit** (technical invalidity): if submitted answers reference any item the
  server did not issue for that attempt, the result is blocked as `item_set_mismatch` (replay/tamper signal).
  The scorer already ignores foreign items; this makes the mismatch an explicit block.
- **Credential issuance unchanged and still safe:** issued only for a clean, technically valid pass, exactly
  once per attempt (DB partial-unique index on `attempt_id`), idempotent across refresh/replay — verified in
  the existing release suite.
- **Admin UI:** a new **Result publication** settings group exposes the four flags with copy stating the
  default is immediate publication and proctoring is audit-only.
- **Student/portal wording** for the (now rare, technical-only) hold changed from "examination integrity
  check / under review" to "did not pass a technical validity check … this is not a proctoring or
  manual-marking review", so the UI never implies human marking.

### Configuration validation (Section 21)
- **Startup validation** logs every production config issue and, in `Production`, **refuses to boot** on hard
  errors (missing `STRIPE_WEBHOOK_SECRET` when Stripe is on, wildcard/absent `ALLOWED_ORIGIN`, localhost/empty
  `APP_BASE_URL`, `/tmp` database, legacy admin token enabled) unless `ALLOW_INSECURE_PRODUCTION=true`.
- **`GET /api/admin/system-check`** (owner-only) reports operational readiness as booleans/severities —
  Stripe/webhook/SMTP configured, CORS locked, legacy token disabled, persistent DB, owner password changed,
  migrations applied — **without ever returning secret values**.

### Tests
- **New `tests/publication_test.py` (10 assertions):** critical violation + identity fail with flags OFF →
  publishes immediately; proctor event still stored; late / item-mismatch / cancelled-booking /
  reversed-payment / duplicate all still block; enabling each flag makes it block; threshold respected;
  clean pass publishes + credential.
- The one prior assertion that encoded the *old* default (critical proctor → held) was updated to enable the
  flag first, so it now tests the opt-in block path.
- **CI** runs all five suites (`publication_test` added) and probes `/api/admin/system-check` after boot.

### Verification
Backend **0 errors / 0 warnings** (.NET 8.0.128); **306** SQL queries valid; **55 assertions across 5 SQLite
suites pass, 0 failures**; both app shells pass `node --check`; new route + config seeds present.

## Files changed (Phase 1)
`Core/Lifecycle.cs` (auto-hold rework), `Endpoints/StudentExam.cs` (item-set mismatch block),
`Program.cs` (config validation + `/api/admin/system-check`), `schema.sql` (auto_block flag seeds),
`wwwroot/admin.html` (Result publication settings + wording), `wwwroot/student.html` (hold wording),
`.github/workflows/build.yml` (publication_test + system-check probe), `tests/publication_test.py` (new).

## Planned next phases
- **Phase 2:** evidence storage abstraction (metadata + reference instead of inline data URIs; size/MIME
  limits; retention setting) — Section 19; plus any remaining student/admin contract nits (Sections 13–14).
- **Phase 3:** security headers + CORS hardening + request-size limits (Section 18) and the full technical
  documentation refresh (Section 23), then a final consolidated changelog.

## Known technical limitations (unchanged, documented not hidden)
Full TOTP 2FA is deliberately "coming soon" (Section 16 — misleading toggle already removed); certificate PDF
is generated client-side and attachments use size-capped data URIs pending the Phase-2 storage abstraction;
the richer separate `exam_questions` bank/versioning model remains future work (the safe `is_practice` split
is in place); and the desktop WPF held-result screen's XAML is Windows-only and not compile-verifiable here
(its DTO contract is shipped).

---

## Phase 2 — Evidence storage abstraction & upload hardening (Section 19)

### The problem
Exam proctoring frames were stored as **inline base64 data URIs in SQLite** (up to 4 MB each, many per
attempt), and the Phase-earlier casework attachments (support/appeals/accommodations/CPD) did the same. This
bloats the database, makes backups huge, and couples binary storage to the row store.

### What was built
- **`Core/Storage.cs` — a pluggable storage abstraction.** Bytes are written to a configurable backend and
  the database stores only **metadata + a provider-qualified reference** (e.g. `local:evidence/ab/<sha>.jpg`).
  - Backends via `STORAGE_PROVIDER`: `local` (default, under `STORAGE_ROOT`, sharded by hash) with a
    documented, provider-agnostic seam for object storage (S3-style `PutObject`/`GetObject`) that needs no
    call-site changes; a non-local provider that isn't wired yet falls back to local with a startup warning.
  - **Content-addressed** (SHA-256) so identical frames de-duplicate automatically.
  - **Validation on every upload:** MIME allow-list (JPEG/PNG/WebP/PDF), 3 MB cap, and **magic-byte
    sniffing** so a renamed/mislabeled payload (e.g. an exe posing as a PDF) is rejected.
  - **Path-traversal-safe reads**; references never leak a filesystem absolute path to clients.
  - **Retention:** `PurgeOlderThan(days)` + `evidence_retention_days` setting (default 365).
- **Rewired every write path to the abstraction:** exam evidence (`/api/exam/evidence`), support attachments,
  appeal/accommodation/CPD evidence. **No code path writes inline bytes to the DB anymore.**
- **Authenticated serve endpoints** stream bytes by reference: `GET /api/admin/evidence/{id}` (proctoring-
  gated), the appeal/accommodation/CPD evidence viewers, and `GET /api/me/tickets/{tid}/attachments/{aid}`
  (ownership-enforced). All are **backward-compatible** — pre-migration rows with an inline `data_uri` still
  serve correctly (12 legacy-compat read paths retained).
- **Admin + student UIs updated to fetch artefacts as authenticated blobs** (images can't send a bearer token
  via `src`): the proctoring evidence gallery hydrates thumbnails via `apiBlob`, and student ticket
  attachments download via an authed fetch → object URL. Demo mode handled.
- **Ops:** `POST /api/admin/storage/purge` (owner-only) applies the retention window and is audited;
  `system-check` now reports `storage_local`; startup logs the active storage provider.

### Schema
`exam_evidence` +`storage_ref`/`size_bytes`/`sha256`; `support_attachments` +`storage_ref`/`sha256`;
appeal/accommodation/CPD reuse `evidence_data` to hold the `local:` reference. All added idempotently in
`schema.sql` **and** `Migrate.cs`, so existing databases upgrade with no data loss and old inline artefacts
remain viewable.

### Tests & verification
- **New `tests/storage_test.py` (10 assertions)** mirrors the helper's rules; a **C# runtime test proved the
  real helper on disk (11/11)**: reference format, round-trip, dedupe, MIME allow-list, magic-byte mismatch,
  oversize, path-traversal, retention purge.
- Full suite now **61 assertions across 6 SQLite suites, 0 failures**; backend **0 errors / 0 warnings**;
  **308** SQL queries valid; four app shells pass `node --check`; new routes registered; CI runs the storage
  suite too.

### Files changed (Phase 2)
`Core/Storage.cs` (new), `Endpoints/ExamClient.cs`, `Endpoints/Casework.cs`, `Endpoints/AdminProctoring.cs`,
`Program.cs` (storage boot notice, `system-check.storage_local`, purge endpoint), `schema.sql`,
`Data/Migrate.cs`, `wwwroot/admin.html`, `wwwroot/student.html`, `.github/workflows/build.yml`,
`tests/storage_test.py` (new).

### Honest notes
The object-storage backend is a **documented seam, not a live S3 integration** — selecting a non-local
provider currently falls back to local (with a warning) until `PutObject`/`GetObject` are implemented; the
reference format and all call sites are already provider-agnostic so that wiring is isolated. Local storage
assumes a persistent volume in production (the config validator already flags a `/tmp` database; operators
should likewise point `STORAGE_ROOT` at durable storage). Retention purge is on-demand via the endpoint; a
scheduled trigger (cron/hosted service) is left to the deployment.

### Remaining
**Phase 3:** security headers (HSTS, X-Content-Type-Options, Referrer-Policy, CSP, frame-ancestors), CORS
hardening (no wildcard in production — the validator already errors on it; add the response-header enforcement),
request-size limits for uploads/JSON bodies (Section 18), and the full technical documentation refresh
(Section 23), then a final consolidated changelog.
