# E2E User Journey Report - 2026-07-23

Branch: `cursor/fix-pml-ai-e2e-d975`

This report records the browser-journey coverage added or updated on this branch. It is intentionally
conservative: local work discovered the Playwright suite shape, but browser execution has not been
claimed green from this environment. Playwright runtime, .NET build/unit, and SQLite/MySQL integration
results are **PENDING-CI** until the branch CI publishes a run.

## Status legend

- **LOCAL-PASS** - verified locally in this branch.
- **PENDING-CI** - authored or discovered locally, but runtime proof must come from CI.
- **NOT-CLAIMED (external)** - requires live external/provider/operator execution and is outside the
  automated branch claim.

## Local discovery

- Playwright discovery lists **91 executions** across **18 spec files** and **5 projects**.
- Discovery is not execution. Do not treat the listed browser journeys as passed until CI runs them.
- Relevant Playwright projects: `chromium`, `firefox`, `webkit`, `mobile-chrome`, `mobile-safari`.
- Branch specs include the pre-existing public/portal specs plus new/expanded specs for PML-AI,
  student security, admin security/RBAC, billing/founding, credentials/CPD/documents, proctoring,
  impersonation/operations, partner portal, i18n, policies, and public applications.

## Journey status by area

| Area | Status | Branch coverage summary | Boundaries / non-claims |
|---|---|---|---|
| Multi-certification PML-AI migration (third cert) | **PENDING-CI** | PML-AI is represented as the third suite certification across catalogue/i18n/credential flows, with legacy PDL-AI/CPMD naming migrated or redirected toward PML-AI. `public-catalogue.spec.ts`, `public-i18n.spec.ts`, `admin-credentials.spec.ts`. | Browser pass, backend migration pass, and MySQL parity are pending CI. |
| Student account/security | **PENDING-CI** | Forgot-password non-enumeration, invalid reset-token handoff to portal login, registration/onboarding/logout, and existing portal auth coverage. `portal-account-security.spec.ts`, `portal-auth.spec.ts`. | No claim of live email delivery. |
| Admin MFA/RBAC | **PENDING-CI** | Admin settings 2FA status card, least-privilege viewer hidden nav / forbidden sections, owner sign-in gates, and failed admin sign-in. `admin-security-rbac.spec.ts`, `admin-console.spec.ts`. | Browser runtime and server RBAC verification are pending CI. |
| Billing/founding/finance (cert-aware codes + preview) | **PENDING-CI** | PML-AI-scoped exam discount rejects the wrong certification and previews against PML-AI; billing/founding UI paths stay certification-aware. `portal-billing-founding.spec.ts`. | Live Stripe/provider settlement is **NOT-CLAIMED (external)**; automated code-validation/browser preview is pending CI. |
| Honorary setup_url fallback | **PENDING-CI** | Public honorary/founding routes load, honorary application API accepts a complete public application, and branch backend changes preserve reset/setup handoff behavior. `public-applications.spec.ts`. | Identity verification with real third-party IDV is **NOT-CLAIMED (external)**. |
| Credentials/CPD/documents | **PENDING-CI** | Student CPD submission followed by admin approval and visible approved total; assigned document acknowledgement; admin issues, revokes, and reinstates a PML-AI credential. `portal-documents-cpd.spec.ts`, `admin-credentials.spec.ts`. | PDF/watermarking/provider storage runtime proof remains pending CI unless separately reported. |
| Proctoring/impersonation/partner | **PENDING-CI** | Admin proctoring sessions/live heartbeat smoke, viewer denial, read-only impersonation support-view banner with end action, owner operations-page smoke, and partner portal login/download guard. `admin-proctoring.spec.ts`, `admin-operations.spec.ts`, `partner-portal.spec.ts`. | Live exam-vendor/proctoring provider execution is **NOT-CLAIMED (external)**. |
| Audit attribution | **PENDING-CI** | Branch fixes attribute audit events to the effective admin/support actor where the UI action is proxied or impersonated. | Known `audit_logs.user_id` schema limitation remains open; see `DEFECT_REGISTER.md`. |
| Public i18n (7 langs) + policies | **PENDING-CI** | Public language metadata/cookie/switcher coverage, PML-AI slug coverage, crawlable policy pages, catalogue cards, downloads, public site smoke, chat, and axe smoke. `public-i18n.spec.ts`, `public-policies.spec.ts`, `public-catalogue.spec.ts`, `public-downloads.spec.ts`, `public-site.spec.ts`, `public-chat.spec.ts`. | Translation-provider calls are **NOT-CLAIMED (external)**; browser execution pending CI. |
| CSV whitespace formula neutralization | **PENDING-CI** | Branch backend CSV handling neutralizes spreadsheet formulas even when formula triggers are preceded by whitespace/control characters, while preserving non-formula values. | .NET CsvTests and SQLite/MySQL integration proof are pending CI. |

## CI status

- CI run URL: **TBD**
- Required before release claim:
  - Playwright E2E job completes successfully on CI.
  - .NET build/unit tests complete successfully on CI.
  - Backend integration suites complete successfully on both SQLite and MySQL.
  - CI artifacts are attached or linked in the evidence manifest.
