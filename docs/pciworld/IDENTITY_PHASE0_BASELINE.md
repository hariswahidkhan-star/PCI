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
| ID-06 | `Core/Erasure.cs` | Erasure clears `registration_no`, making the number **reusable** | P0 | `UPDATE users SET … registration_no=NULL` | No reservation ledger existed | **Mitigated** — the registry now retains the reservation permanently (E1/E2). The erasure endpoint itself still needs the explicit retire transition: Phase 2 |
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

## 5. Next, in order

1. **Phase 2** — backfill and cutover: dry-run counts, quarantine duplicates, resumable batches,
   reconcile registry against projection, narrow `registration_no` to bounded VARCHAR, add the
   explicit retire transition to `Erasure.cs`, then delete the `/api/me` backstop.
3. **Phase 3+** — handoff symmetry, shared Passport, verification, events, sharing.

Nothing past Phase 1a is started, and nothing above claims otherwise.
