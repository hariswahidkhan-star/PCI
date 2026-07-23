# PCI Platform — Release Readiness Report (Template)

_Copy this file to `RELEASE_READINESS_<version-or-date>.md` and fill it in for each release decision.
It is the go/no-go record. Per the programme rule, results are separated into **Automated**,
**Manually-verified**, **External-provider-pending**, **Operator-config-pending** and **Residual-risk** —
and it never claims "100% defect-free". The final merge/deploy is a **human** decision._

---

## Release

- **Version / commit:** `__________`  (branch: `__________`, PR: `#____`)
- **Date (UTC):** `__________`
- **Reviewer (go/no-go owner):** `__________`
- **Summary of change:** `__________`

## 1. Automated — CI gate (must be green on the PR head)

| Job | Result | Count / note |
|---|---|---|
| `static-quality` (dep/secret/lint/dockerfile) | ☐ pass | gitleaks clean; nuget vuln = allow-listed residual only |
| `backend` (integration, SQLite) | ☐ pass | ____ / ____ assertions |
| `backend-mysql` (integration, MySQL) | ☐ pass | ____ / ____ assertions |
| `backend-unit` (xUnit) | ☐ pass | ____ tests |
| `frontend` (vitest + lint + tsc + build) | ☐ pass | ____ tests |
| `e2e` (Playwright + axe) | ☐ pass | ____ tests |
| `secureexam-core-linux` / `secureexam-windows` | ☐ pass | ____ tests |

- DB-behaviour changes verified on **both** providers? ☐ yes ☐ n/a
- Any assertion weakened or test deleted to go green? ☐ **no** (required)
- Coverage matrix + defect register updated in this PR? ☐ yes

## 2. Manually-verified

| Item | Verifier | Result / evidence |
|---|---|---|
| Critical UI journey spot-check (login→dashboard, purchase→gate lifts) | | |
| Legal/wording review of any changed public copy | | |
| Accessibility spot-check beyond automated axe | | |

## 3. External-provider-pending

_From `EXTERNAL_PROVIDER_TEST_PLAN.md` — sandbox runs an operator executes against **staging**._

| Provider | Sandbox run done? | Evidence / ⏳ pending |
|---|---|---|
| Stripe (checkout, webhook, dues) | ☐ | |
| Certuvo | ☐ | |
| Exam-delivery vendors | ☐ | |
| Email / WhatsApp / marketing OAuth | ☐ | |
| Google Sign-In | ☐ | |
| S3 real bucket | ☐ | |

## 4. Operator-config-pending

| Item | Done? | Note |
|---|---|---|
| Env/secrets set (Stripe live keys, encryption key, ALLOWED_ORIGIN, provider creds) | ☐ | never in repo/logs |
| Render deploy + `/api/health` + system-check authz | ☐ | recovery_configured true |
| Persistent disk (`/data`) durability + non-root disk perms | ☐ | SQ-10 deferred item |
| Read-only prod smoke (`smoke-test.sh` variant) | ☐ | DEP-1 |
| Backup scheduled + **restore rehearsal** proven (`DR_RESTORE_RUNBOOK.md`) | ☐ | DR-1/2/3 |
| Dependabot/renovate, trivy image scan, SHA-pinned actions | ☐ | SQ-8/12/13 backlog |

## 5. Residual-risk (accepted, with rationale)

| Risk | Rationale / mitigation | Owner sign-off |
|---|---|---|
| CVE-2025-6965 (SQLitePCLRaw, no upstream patch) | allow-listed; no user-supplied SQL; prod uses MySQL | ☐ |
| DEF-2 exam retake-wait dead-end | surfaced-but-inert control; product decision pending | ☐ |
| Other open `DEFECT_REGISTER.md` items | see register | ☐ |

## 6. Decision

- **Go / No-go:** ☐ GO  ☐ NO-GO
- **Conditions / follow-ups:** `__________`
- **Signature:** `__________`  **Date:** `__________`

> Reminder: the release PR is **not** auto-merged. This report is the human record behind the merge.
