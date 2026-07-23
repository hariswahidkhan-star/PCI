# Suppression & Allow-list Process

_Owner: engineering. Introduced in the Phase-2 static-quality increment (Increment A)._

The static-quality gates in CI (`static-quality` job and the per-language gates in `.github/workflows/build.yml`)
are designed to **fail on new problems**. A suppression is the deliberate, reviewed exception to one of
those gates. This document is the single place that explains **when a suppression is allowed, how it must
be recorded, and how it is reviewed** — and it carries the live register of every active suppression.

The guiding rule from the Phase-2 programme applies without exception:

> Never suppress a finding that reflects a **real defect**. Suppress only findings that are provably
> non-issues (test fixtures, unreachable code paths) or residuals with **no available fix** — and record
> the residual risk honestly. Fix, don't silence.

## What counts as a suppression here

| Mechanism | Where | Gate it affects |
|---|---|---|
| `#pragma warning disable` (C#) | inline in a `.cs` file | `TreatWarningsAsErrors` (SQ-1) |
| NuGet vulnerability allow-list | `backend/tools/nuget-vuln-allowlist.json` | `check_nuget_vulns.py` (SQ-5) |
| Secret-scan allow-list | `.gitleaks.toml` `[allowlist]` | gitleaks (SQ-7) |
| Dockerfile rule ignore | `.hadolint.yaml` `ignored:` | hadolint (SQ-10) |
| ESLint rule downgrade to `warn`/`off` | `frontend/eslint.config.js` | ESLint (SQ-2) |

## Requirements for any new suppression

1. **Prefer the fix.** Upgrade the dependency, correct the code, remove the secret from source. A
   suppression is only for cases where the fix is unavailable, or the finding is a proven false positive.
2. **Scope it as narrowly as possible.** Suppress one rule / one advisory / one file — never a
   whole category. Path-based allow-lists must name the specific test directories, not `**`.
3. **Justify it in place.** The suppression must carry a comment (or JSON field) stating *why* it is safe,
   with enough detail that a reviewer who has never seen it can judge it.
4. **Assess residual risk** for anything that is a real-but-unfixable vulnerability (e.g. an unpatched
   transitive CVE): state what an attacker would need, and why it is not reachable in this system.
5. **Set a review date** (`review_by`) for residual-risk suppressions so they are re-checked when an
   upstream fix may have shipped. False-positive suppressions (test fixtures) do not need a date.
6. **Record it in the register below.**

## Review cadence

- Residual-risk suppressions are re-checked on their `review_by` date (or sooner if an advisory updates).
- The `check_nuget_vulns.py` gate prints a **STALE** note when an allow-list entry no longer matches a live
  advisory — that is the signal to delete the entry (a fix has shipped) and let the gate resume blocking.
- Scanner rule ignores (`.hadolint.yaml`, ESLint downgrades) are revisited whenever the related file is
  materially changed.

---

## Active suppression register

### 1. C# `CS1998` — synchronous connector no-ops
- **Where:** `backend/Core/ExamDeliveryConnectors.cs` (file-level `#pragma warning disable CS1998`).
- **Why:** several methods implement the async `IExamDeliveryConnector` contract but are legitimately
  synchronous — the vendor folds the step into another call (e.g. Pearson VUE authorize → schedule) or
  makes it candidate-driven. They return a truthful `ConnResult` with no network I/O to await. The async
  signature is kept for a uniform interface.
- **Type:** false positive (no behavioural issue). **Review:** on next change to that file.

### 2. NuGet `GHSA-2m69-gcr7-jv3q` (CVE-2025-6965) — SQLitePCLRaw
- **Where:** `backend/tools/nuget-vuln-allowlist.json`.
- **Why:** memory-corruption defect in the native SQLite bundled by `SQLitePCLRaw.lib.e_sqlite3`,
  affecting all versions ≤ 2.1.11 with **no patched release available upstream**. We already pin the
  newest available (2.1.11), which supersedes the transitive 2.1.6.
- **Residual risk:** exploitation requires attacker-controlled SQL text. The backend issues only
  parameterised, developer-authored SQL and never executes user-supplied SQL, so the path is not reachable
  from any request surface. Production additionally runs on MySQL, where this native library is not loaded.
  Assessed **LOW**.
- **Type:** unpatched residual. **Review:** `review_by` 2026-10-23 (see the JSON entry). Remove and re-pin
  once a patched release exists.

### 3. gitleaks — synthetic test placeholders
- **Where:** `.gitleaks.toml` `[allowlist]`.
- **Why:** the adversarial test suites hard-code fake tokens (`whsec_integration_test_secret`,
  `sk_test_integration`, `whsec_cv_13`, `cv_key`) to drive signature/HMAC verification against their own
  mock servers. They authenticate nothing real. Allow-listed by exact value and by test-directory path.
- **Type:** false positive (non-secret fixtures). Production code is still fully scanned.

### 4. hadolint `DL3002` — container runs as root
- **Where:** `.hadolint.yaml` `ignored:`, and the hadolint step is **informational** for now.
- **Why:** the runtime writes the SQLite DB and evidence store to the `/data` persistent mount, which the
  hosting platform (Render) mounts root-owned at runtime. Switching to a non-root `USER` without also
  arranging mount ownership would break those writes in production.
- **Type:** accepted-with-follow-up (not a false positive). Running non-root is a **tracked hardening
  follow-up** that must be coordinated with the deploy/disk configuration (entrypoint chown + privilege
  drop, or platform `fsGroup`). Recorded as residual DEP hardening in `TEST_COVERAGE_MATRIX.md`.

### 5. ESLint rule downgrades — stylistic noise
- **Where:** `frontend/eslint.config.js`.
- **Why:** `@typescript-eslint/no-explicit-any`, `no-empty-object-type`, and `ban-ts-comment` are set to
  `warn` (not `error`) because they fire widely on the existing 110-file codebase and are not correctness
  bugs; `no-unused-vars` is `warn` because `tsconfig` already enforces `noUnusedLocals/Parameters` more
  precisely. The React Hooks rules and `no-unused-expressions` (with idiomatic ternary/short-circuit
  allowances) remain **errors** — those catch real runtime bugs.
- **Type:** severity calibration, not silencing. Warnings still surface in CI logs.
