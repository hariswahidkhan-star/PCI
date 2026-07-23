# E2E User Journey Report - 2026-07-23

Branch: `cursor/fix-pml-ai-e2e-d975`

This report records the browser-journey coverage added or updated on this branch and the CI proof
that executed it.

## Status legend

- **CI-PASS** - verified green on the published CI run below.
- **LOCAL-PASS** - verified locally in this branch (non-browser checks).
- **NOT-CLAIMED (external)** - requires live external/provider/operator execution and is outside the
  automated branch claim.

## Local discovery (pre-CI)

- Playwright discovery listed **91 executions** across **18 spec files** and **5 projects**.
- Projects: `chromium`, `firefox`, `webkit`, `mobile-chrome`, `mobile-safari`.

## Journey status by area

| Area | Status | Branch coverage summary | Boundaries / non-claims |
|---|---|---|---|
| Multi-certification PML-AI migration (third cert) | **CI-PASS** | PML-AI is the third suite certification across catalogue/i18n/credential flows; legacy PDL-AI/CPMD naming migrates/redirects to PML-AI. Backend SQLite + MySQL jobs also green. | — |
| Student account/security | **CI-PASS** | Forgot-password non-enumeration, invalid reset-token handoff to `/app/login`, registration/onboarding/logout. | Live email delivery **NOT-CLAIMED (external)**. |
| Admin MFA/RBAC | **CI-PASS** | Admin settings 2FA status card, least-privilege viewer hidden nav / forbidden sections, owner sign-in gates. | — |
| Billing/founding/finance (cert-aware codes + preview) | **CI-PASS** | PML-AI-scoped exam discount rejects PCL-AI and previews against PML-AI. | Live Stripe settlement **NOT-CLAIMED (external)**. |
| Honorary setup_url fallback | **CI-PASS** | Public honorary/founding routes load; honorary application API accepts a complete public application; approval response includes `setup_url` fallback. | Live third-party IDV **NOT-CLAIMED (external)**. |
| Credentials/CPD/documents | **CI-PASS** | Student CPD → admin approval → updated total; document acknowledgement; admin issues/revokes/reinstates a PML-AI credential. | — |
| Proctoring/impersonation/partner | **CI-PASS** | Proctoring sessions smoke, viewer denial, read-only impersonation banner + end, partner login/download guard. | Live exam-vendor sandbox **NOT-CLAIMED (external)**. |
| Audit attribution | **CI-PASS** | Privileged mutations attribute the acting admin; subject retained in details. Covered by backend suites on SQLite and MySQL. | Known `audit_logs.user_id` schema limitation remains open. |
| Public i18n (7 langs) + policies | **CI-PASS** | Full language sequence, Arabic RTL, crawlable policy pages, catalogue/site smoke across Chromium + Firefox/WebKit/mobile. | Live translation-provider ops **NOT-CLAIMED (external)**. |
| CSV whitespace formula neutralization | **CI-PASS** | Whitespace-prefixed formula markers neutralized; genuine signed numbers preserved (`CsvTests` via backend-unit). | — |

## CI status

- CI run URL: https://github.com/hariswahidkhan-star/PCI/actions/runs/30002525905
- Conclusion: **success** (all required jobs green)
- Jobs: `backend`, `backend-mysql`, `backend-unit`, `frontend`, `e2e`, `static-quality`, `secureexam-core-linux`, `secureexam-windows`
- e2e job: https://github.com/hariswahidkhan-star/PCI/actions/runs/30002525905/job/89190815700 (**pass**)
- Playwright report artifact: `playwright-report` (uploaded by the e2e job)

## Local non-browser validation

- TypeScript, ESLint (0 errors / 18 warnings), Vitest, frontend production build, npm production audit: **LOCAL-PASS**
- Python compilation of `integration_test.py`, `git diff --check`: **LOCAL-PASS**
