# PCI World Shared Identity — Phase 0 baseline and issue register

Scope: canonical identity and the PCI Student Number, the two-way portal handoff, the shared
Passport, privacy-safe verification, printable Passport / event admission, and premium social
sharing. This document is the evidence-based baseline the phased plan is built on. It is updated
as each phase closes; it is not a design document (see `IDENTITY_PHASE1_DESIGN.md` when written).

## 0. Environment note — what can and cannot be verified locally

This matters for reading every "verified" claim below.

| Toolchain | Available here | Consequence |
|---|---|---|
| Python 3.11 + SQLite | yes | Logic suites (`backend/tests/*.py`) run — real schema, real SQL |
| Node 22 / npm | yes | Frontend typecheck and build run |
| .NET 8 SDK | **no** | `dot.net` is outside the sandbox's proxy allowlist and the installer downloads an empty body. The backend **cannot be compiled or unit-tested in this environment.** |
| MySQL server | no | MySQL parity is asserted by generated DDL review, not by execution |

So: C# changes here are verified by (a) transcribing the exact SQL into a Python suite that runs
against the real `schema.sql`, and (b) an xUnit suite committed for CI, which does have the SDK.
Until CI runs, "the C# compiles" is an unverified claim and is not made anywhere in this document.

## 1. Confirmed repository state

- Branch `claude/pci-world-identity-passport-sharing-niogba`, forked at `80fb501`, which is also
  `origin/main`. The prompt's stated baseline `3c1e85e` is 8 commits behind current `main`; every
  finding below was re-confirmed against the working tree, not inherited from the prompt.
- The backend is ASP.NET Core 8 minimal API; identity lives in `users` + `student_profiles`;
  `Core/WorldIdentity.cs` already establishes those two as authoritative for both products and maps
  `pciworld_participants.user_id` one-to-one. That single-source model is correct and is preserved.

## 2. Baseline issue register

Severity per the brief: P0 = identity takeover / collision / private exposure; P1 = duplicate
account, wrong number, blocked journey; P2 = partial workflow; P3 = cosmetic.

| ID | Module | Issue | Sev | Reproduction | Root cause | Status |
|---|---|---|---|---|---|---|
| ID-01 | `Endpoints/StudentExam.cs` | `GET /api/me` **minted** the Student Number — a read endpoint mutating identity data | P1 | Read the handler at the `registration_no` block; the `UPDATE users SET registration_no=…` ran on read | Issuance was never part of account creation, so a lazy writer was needed to cover the gap | **Fixed** (routed through the issuer; removal of the backstop is Phase 2) |
| ID-02 | `Endpoints/StudentExam.cs` | The number used `DateTime.UtcNow` — the year of the *read*, not of the account | P1 | An account created 2024 first read in 2026 was stamped `PCI-2026-…` | Same as ID-01 | **Fixed + regression-tested** (A5, and xUnit `Issuance_takes_the_year_from_the_account_not_from_the_clock`) |
| ID-03 | `Endpoints/Account.cs` | Signup created `users` + `student_profiles` with **no number and no transaction** | P1 | `CreateStudent` did two unwrapped inserts and returned | Account creation predates the number | **Fixed** (single transaction, issues in-band) |
| ID-04 | `schema.sql` / `schema.mysql.sql` | `users.registration_no` nullable `TEXT`, **no uniqueness** | P0 (collision risk) | `grep registration_no schema*.sql`; no index anywhere in `Migrate.cs` | Column added additively, constraint never followed | **Partly fixed** — guarded unique index added; type narrowing deferred to Phase 2 |
| ID-05 | `Endpoints/Books.cs` | Watermarked the internal `users.id` on distributable PDFs, labelled "PCI Student ID" | P2 | Line ~204, `$"… | PCI Student ID: {u.Id:D6} | …"` | Written before a public number existed | **Fixed** (uses the canonical number, or omits it) |
| ID-06 | `Core/Erasure.cs` | Erasure clears `registration_no`, making the number **reusable** | P0 | `UPDATE users SET … registration_no=NULL` | No reservation ledger existed | **Fixed** (Phase 2) — erasure retires the reservation *before* clearing the projection, and retiring an unrecorded historic number creates its reservation rather than leaving it unclaimed. Evidence G1–G3 |
| ID-07 | 6 production paths wrote `users` directly | payment, admin-created, honorary, partner, World-canonical, self-signup — any could create a numberless student | P1 | `grep -rn "INSERT INTO users" --include=*.cs` | No central account service | **Fixed** — all six now call the one issuer |
| ID-08 | `Endpoints/WorldAccount.cs` | Canonical-mapping failure is caught and registration continues → split identity | P1 | `try { WorldIdentity.MapOne(db, id); } catch { }` in `Register` | Defensive catch added to avoid breaking signup — it broke it instead | **Fixed** (Phase 1b; xUnit `WorldIdentityLinkTests`, CI-gated) |
| ID-09 | `Core/WorldPages.cs` | Token-specific Passport/result pages advertise `/world` as canonical and `og:url` | P2 | Layout passes a fixed `/world` path | Metadata written for the homepage first | **Open — Phase 6** |
| ID-10 | `Endpoints/Events.cs` | Capacity check is not under a transaction/lock → final-seat overbooking | P1 | `test_events_module` only covers a sequential capacity=1 flow | Count-then-insert without a guard | **Open — Phase 5A** (not yet reproduced; needs a MySQL race harness, which needs the SDK) |
| ID-11 | `Endpoints/Events.cs` | Attendance gated on broad `content` permission; cancelled registrations can be marked attended | P2 | Admin attendance handler | Granular event permissions never introduced | **Open — Phase 5A** |
| ID-12 | `Endpoints/Events.cs` | Attendance + CPD writes not confirmed transactional/exactly-once | P1 | Retry between the two writes | No exactly-once CPD source constraint | **Open — Phase 5A** |

ID-10 through ID-12 are carried from the brief and confirmed present by code reading, but are
**not yet reproduced with a failing test** — the race harnesses need the .NET SDK. They are not
claimed as diagnosed.

## 3. Phase 1a — what was implemented

One rule: **`Core/StudentNumbers.cs` is the only code that may issue a Student Number.**

- `pci_student_number_registry` — the permanent reservation and audit ledger. Not a profile, not a
  second identity authority; `users.registration_no` stays the compatibility projection that every
  existing reader already uses. The registry is what makes non-reuse real: after a merge,
  retirement or erasure the person is released and the number is not.
- `users.registration_no_issued_at`.
- `StudentNumbers.GetOrIssue` — idempotent, reserves before projecting, derives the year from the
  account's own `created_at`, throws rather than returning a wrong number, and never opens its own
  transaction so it commits atomically with the account that called it.
- `StudentNumbers.Read` — the read-side helper, which returns null rather than issuing.
- All six production creation paths wired: self-signup/Google (`Account.cs`), payment
  (`Payments.cs`), admin-created (`AdminStudents.cs`), honorary conversion
  (`HonoraryApplication.cs`), partner (`Partners.cs`), World-canonical (`WorldIdentity.cs`).
  `Payments.cs` was already inside a transaction, so issuance joins it rather than nesting.

### The uniqueness index is deliberately guarded

`ux_users_registration_no` is created **only when the data is already clean**. An existing database
may carry duplicates from the lazy-writer era, and an unconditional `CREATE UNIQUE INDEX` would
fail boot for exactly the installs that most need repair. Fresh installs get the index immediately;
existing ones get it on the first boot after Phase 2's backfill/quarantine pass. The check counts
`''` as a value, because the partial predicate exempts only `NULL`.

## 4. Verification evidence

`python3 backend/tests/student_number_test.py` — 16/16 PASS against real SQLite built from the real
`schema.sql`, covering: format and padding; year-from-account (ID-02); idempotent re-issue; historic
numbers never rewritten; a number reserved to another identity refused **with the loser left
numberless rather than wrong**; duplicate projection rejected; multiple NULLs allowed during the
backfill window; reservation surviving erasure and blocking re-reservation; reads issuing nothing.

Regression: `lifecycle`, `release`, `casework`, `settings`, `publication`, `storage`,
`migration_integrity` — all still pass with the schema change applied.

MySQL: `tools/sqlite_to_mysql.py` regenerated `schema.mysql.sql`; the emitted registry DDL was
reviewed and matches the generator's conventions. `Db.Exec`'s existing prefix-retry covers the TEXT
key length, and `Db.Translate` strips the partial predicate (MySQL exempts NULL from UNIQUE natively).
**Not executed** — no MySQL server here.

`backend/tests/PCI.Backend.Tests/StudentNumberTests.cs` — 7 facts committed for CI, which runs on
both SQLite and MariaDB. **Not executed locally** (no SDK); CI is the gate.

## 4a. Phase 1b — World registration can no longer report success on a split identity

`WorldAccount.Register` wrapped `pciworld_users` insert + canonical mapping + attempt claim in one
transaction, and separated two outcomes the old single `catch` had conflated:

- **Genuine failure to establish identity** → nothing is created; the caller gets
  `identity_unavailable` and HTTP **503** (ours to fix, retryable — not a 400 inviting the person to
  correct input that was never wrong).
- **Quarantined email collision** → the account *is* created, in `identity_link_pending`. This is a
  designed safety outcome, not an error: an email matching an existing canonical account must never
  merge on a string match. `WorldIdentity.LinkPending` derives the state from the absence of a
  canonical link rather than storing a duplicate flag, so it cannot drift.

`PublishPassport` refuses `identity_link_pending` **before** every other precondition — a public
Passport speaks for a canonical identity, carrying its Student Number and evidence, so an account
that has not proven ownership has nothing it is entitled to publish. The gate is in the domain
function, not the endpoint, so future admin and handoff callers inherit it. `GET /api/world/account`
exposes `identity_state` purely so the UI can offer the linking journey; it is never the gate.

The refusal message confirms nothing about the existing account — "an account with your email
exists" is itself a disclosure, and the person may not be its owner.

Evidence: `tests/PCI.Backend.Tests/WorldIdentityLinkTests.cs`, 5 facts, **CI-gated** (no local SDK).
The rollback fact is SQLite-only and says why: its forcing function is DDL, which implicitly commits
on MySQL and would leak into TestEnv's shared template.

## 4b. Phase 2 (part 1) — backfill, reconciliation, retirement

`Core/StudentNumberBackfill.cs`, plus `Endpoints/AdminIdentity.cs` and a new `identity` permission
group. The ordering is the design: uniqueness cannot be enforced until the data is clean, and the
data cannot be cleaned until somebody can see what is in it.

- `Health` — counts only, never people. An operator diagnosing the estate does not need member
  records to do it. Includes registry drift **in both directions** and `projection_index_eligible`,
  which is the same predicate `Migrate` uses, so an operator can see whether the next boot will pick
  the uniqueness index up.
- `Preview` — returns the number that *would* be issued for each account, including whether it would
  collide, and writes nothing. Verified as writing nothing (A3/A4), not merely asserted.
- `Run` — resumable and idempotent *by construction*, not bookkeeping: it selects rows that still
  lack a number, so restarting resumes and re-running after completion selects nothing. Each account
  is its own transaction, so one collision quarantines that account and the batch carries on rather
  than one bad row rolling back thousands of good ones.
- `Retire` — the fix for ID-06, wired into `Erasure.cs` *before* the column is cleared.

Permissions: `id_read`, `id_backfill`, `id_merge_request`, `id_merge_approve`, `id_audit`, in a group
deliberately outside every named role bundle (owner excepted) — the `operations` pattern. Merge is
split so maker-checker is enforced by the permission model itself, not a convention in a handler.
There is **no issue permission**: a number an operator can type is a number an operator can collide.

Evidence: `tests/student_number_backfill_test.py`, **24/24** against real SQLite — preview writes
nothing; admin and erased accounts are never issued numbers; batch/resume/no-op; a collision
quarantines one account and leaves it numberless rather than wrong; valid and malformed historic
values both preserved; erasure retires and cannot be re-reserved; drift detected both directions.

## 4c. Merged wave 1 (PR #186 → main caa15b3, main CI green — the merged C# is fully verified)

Phases 0/1a/1b/2-part-1 plus the three parallel-agent deliverables: event defects ID-10/11/12
(capacity race, granular events RBAC, exactly-once CPD), ID-09 canonical/og:url + full share-card
metadata, and POST /api/public/world-passports/verify with the neutral-response invariant.

## 4d. Wave 2 (this branch)

- **Phase 2 part 2** — `registration_no` bounded to VARCHAR(32) (guarded MySQL MODIFY; SQLite is
  affinity), and the `/api/me` lazy backstop behind `identity_lazy_backstop` (default ON). Cutover
  gate is the Health report, not a date; flipping back is the rollback.
- **Phase 2 part 3** — maker-checker merges: `pci_identity_merges`, preview-as-counts, deterministic
  lock order, survivor keeps the number, loser's registry row → `merged` resolving to the survivor,
  sessions/handoff codes revoked both sides, before/after snapshots. 47/47 local assertions.
- **Phase 3** — symmetric World→MyPCI handoff on the existing primitives: 90 s hashed one-use code in
  `login_tokens` (`purpose='portal_handoff'`, no new table), fragment-only carriage cleared before
  any network call, DELETE-inside-transaction as the redemption lock, status re-validated at
  redemption, `identity_link_pending` refused at mint, all failures one generic 401. Premium
  switcher in the World app; MyPCI bootstrap redeems and stores the session exactly like login.
  33/33 backend + 387/387 vitest + tsc clean. `/api/portal-handoff/redeem` added to the global
  rate-limiter path list alongside `/api/login`.

CI gates 13 logic suites.

## 4e. Wave 3 (this branch)

- **Phase 4** — one authoritative Passport in both portals: `GET /api/me/world-passport/summary`
  builds `PassportSummaryDto` by CALLING the existing WorldPassport/WorldAccount functions (no new
  count/status SQL), resolves canonical student → World account, folds state to
  not_created|draft|private|published|expired|suspended. Shared React `components/passport/` family
  consumed by the MyPCI dashboard module AND the World Passport page. Hash-only public token →
  publicUrl omitted, never re-minted. World outage → quiet fallback card, MyPCI unaffected.
  33/33 backend + 409/409 vitest + tsc 0. No iframe, no copied rows, no internal ids in the payload.
- **Phase 7** — claim/referral hardened: one-winner claim race (guarded UPDATE … WHERE user_id IS
  NULL, proven with a real two-connection interleave), idempotent re-claim, duplicate-submit
  protection, tamper-proof backend scoring, invitation version pinning proven after edit,
  privacy-safe `pciworld_referrals` (sha refs, counts-only sharer view, de-identified on account
  delete), and an SQLite-authorizer proof the whole journey writes only `pciworld_*` tables. 43/43.

CI gates 15 logic suites. Render deploy note: a deploy failure surfaced for the #185 merge; DEPLOY.md
§"Deploys suddenly failing" documents the likely cause (service still on the SQLite-in-production
posture fails every deploy at health check, exit 78, prior deploy stays live) — resolution is a
Render env change (managed MySQL, or ALLOW_SQLITE_IN_PRODUCTION=true) that only the operator can make.

## 4f. Wave 4 (this branch)

- **Phase 6 Release-1** — premium share sheet in the World app (LinkedIn/X/Facebook/WhatsApp/
  Telegram/email/copy/native/QR; focus-trapped, labelled, reduced-motion, clipboard-denied
  fallback), server-truth caption from PUBLIC fields only (hostile names neutralized — 34/34),
  capability-honesty footnote, no fake direct-post button, Instagram deliberately absent with a
  labelled copy-caption path. URL-sharing only; provider APIs and comment sync stay unbuilt by
  design. One CI-caught test bug fixed (hardcoded unique-indexed id — the wave-1 constraint
  working as designed).
- **Phase 5A Release-1** — printable CR80 wallet card + 4×6 badge, exact page boxes, vector text,
  ECC-M QR ≥0.45mm modules with 4-module quiet zone, refusal matrix (published+unexpired+active+
  numbered only). Design decision: the raw public token is unrecoverable server-side (hash-only at
  rest, everywhere), so the printed QR encodes a one-way /world/pd/{hash} route with the identical
  lifecycle and resolution predicate — unpublish kills every printed QR instantly. 77/77 backend,
  433/433 vitest. Known limitation: the documents endpoint is MyPCI-bearer-authenticated, so the
  World app keeps its existing PDF rather than the new sheet. Event admission (entry passes,
  gates, scanners, offline) is deliberately OUT of the Release-1 slice, stated on the sheet itself.

CI gates 17 logic suites.

## 5. Next, in order

1. Acceptance sweep against spec §20 → `IDENTITY_ACCEPTANCE.md`: dry-run counts, quarantine duplicates, resumable batches,
   reconcile registry against projection, narrow `registration_no` to bounded VARCHAR, add the
   explicit retire transition to `Erasure.cs`, then delete the `/api/me` backstop.
3. **Phase 3+** — handoff symmetry, shared Passport, verification, events, sharing.

Nothing past Phase 1a is started, and nothing above claims otherwise.
