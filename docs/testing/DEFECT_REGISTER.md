# Defect Register

_A living record of defects and notable behavioural findings surfaced by the PCI testing programme.
Each row states the finding, how it was surfaced, its status, and the evidence. Findings are recorded
honestly — a test that exposes a defect is never weakened or deleted to go green; the defect is fixed
(with a regression test) or logged here with its residual risk._

Status legend: **FIXED** (corrected + regression test) · **OPEN** (real, not yet fixed) ·
**DEFERRED** (real, out of the current increment's scope, with rationale) · **BY-DESIGN** (confirmed
intended behaviour after review).

## Register

| ID | Area | Finding | Severity | Surfaced by | Status | Evidence / resolution |
|----|------|---------|----------|-------------|--------|-----------------------|
| DEF-1 | Security — CSV export | Admin analytics + partner-portal CSV exports RFC-4180-quoted delimiters but did **not** neutralise spreadsheet formula triggers (`= + - @`), so an attacker-controlled `utm_*` / `referrer` / `landing` value could execute as a formula when an admin opens the file (CWE-1236 / CSV injection). | High | Phase-2 re-audit (SEC-2) | **FIXED** | Shared `Core/Csv.Field` (formula-injection guard on non-numeric `= + - @` / TAB / CR, then RFC-4180 quote; genuine numbers preserved). All three export sites routed through it (AdminAnalytics + PartnerPortal were vulnerable; AdminMgmt already guarded, now consolidated & no longer corrupts negatives). Regression: `CsvTests` (18). Verified xUnit 425/425 + integration 956/956 on SQLite **and** MySQL (§59n green). Commit `0666d63`. |
| DEF-2 | Exam authorization — retake wait | `ExamAuthorization.ResolveWindow` computes a `RetakeWaitDays` and the value is surfaced to students / "waivable" by admins, but **no code path ever writes `exam_authorizations.retake_wait_until` to a non-null value** — `WaiveWaitingPeriod` only clears it and StudentExam/ExamExceptions only read it. The retake waiting period is therefore never populated or enforced (a dead-end). | Medium | Increment B (BD-9 unit tests) | **OPEN** (pinned) | `ExamAuthorizationTests.EnsureForPayment_ResolvedRetakeWait_IsNeverPersisted` pins the current real behaviour (column stays NULL despite a configured 45-day rule) with a `// FINDING` comment. Not fixed in a test-only increment — flagged for a product decision: either wire the write-through + enforcement, or remove the surfaced-but-inert control. |
| DEF-3 | Exam authorization — BD-4 | Membership renewal/dues webhook date-math (renew from `max(expiry, now)`, recert cycle extension, `invoice.paid` / `subscription.*` mirrors) is not independently unit-testable — it is bound to live Stripe objects. | — (coverage gap, not a defect) | Phase-2 re-audit (BD-4) | **DEFERRED** | Recorded in the coverage matrix as integration-layer; exercised through the signed-webhook path in the integration suite, not the unit tier. No behavioural defect observed. |
| DEF-4 | Scoring — BD-10 | `ScoreAttempt` (pass-boundary / rounding) is private behind `InitScorer`, so the exact `pct == passMark` boundary and one-decimal rounding cannot be unit-pinned without a source seam. | — (coverage gap, not a defect) | Phase-2 re-audit (BD-10) | **DEFERRED** | Boundary is covered at the integration layer (§2 pass, §9e12 66.7% vs 80% fail, §3 fail). Adding a minimal test seam is a candidate follow-up. |

## Notes

- The Phase-0/PR-#73 findings ledger (verify-suspended-credential, membership time-based expiry,
  blog on-demand sweep hook, partner-commission accrual) is **empty** — every one of those was fixed
  and is covered by tests; see `TEST_COVERAGE_MATRIX.md`.
- This register is updated as new increments surface findings. It is the single place a reviewer can
  see what the test programme has learned about the product's real behaviour, separate from the
  coverage matrix (which tracks test presence).
