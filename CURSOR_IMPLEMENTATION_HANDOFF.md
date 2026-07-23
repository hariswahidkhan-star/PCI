# Cursor implementation handoff — PCI end-to-end fixes

## Objective

Publish, run in CI, and review the completed PCI multi-certification and end-to-end journey fixes. The implementation already exists in this repository. Do **not** recreate it from a prompt or overwrite it with a fresh implementation.

## Repository state

- Repository: `https://github.com/hariswahidkhan-star/PCI.git`
- Local repository: `/workspace/scratch/5f64226a1a59/pci-phase2`
- Completed branch: `codex/fix-pml-ai`
- Upstream PR branch incorporated through: `b92e632c73d12a177efcc06d917a475b47ab9c6e`
- Completed local head before this handoff commit: `c631637dc0a874629567ae96204d7ea1fcf26642`
- Portable bundle: `/workspace/scratch/5f64226a1a59/pci-phase2-e2e-fixes.bundle`
- Working tree was clean before this handoff document was added.

## Instructions for Cursor

1. Open the existing local repository and switch to `codex/fix-pml-ai`.
2. Do not reset, rebase, squash, or discard existing commits.
3. Inspect the commit range and the two testing reports listed below.
4. Push `codex/fix-pml-ai` to the `origin` repository.
5. Open a **draft pull request** against `main`, unless an existing PR should instead be updated.
6. Run the complete GitHub Actions pipeline.
7. Fix only failures caused by this branch. Preserve unrelated upstream work and user changes.
8. Do not report Playwright journeys as passed until they have actually run successfully in CI.

## Completed implementation

### Multi-certification migration

- Migrated the third certification from legacy PDL/CPMD naming to **PML-AI**.
- Added/updated redirects, catalogue data, public content, Books/BoK resources, schemas, seeds, tests, and student/admin references.
- Preserved separate PCL-AI, PFL-AI and PML-AI enrolments, exams and credentials.
- Added browser coverage for three-certification isolation.

### Student account and security journeys

- Forgot-password UI is non-enumerating and checks real delivery history.
- Admin-provisioned password setup is browser-tested for one-time use.
- Full onboarding is browser-driven through 100% profile completion.
- Student TOTP enrolment, step-up login, recovery-code disable and session revocation are covered.
- Obsolete reset-password links now hand off to `/app/login`.

### Admin security and RBAC

- Added `GET /api/admin/me/2fa`.
- Active admin TOTP cannot be silently replaced by restarting setup.
- Recovery codes are consumed.
- Settings shows real enabled/pending/recovery state.
- Browser coverage creates a least-privilege viewer and verifies hidden/forbidden sections.
- Owner settings changes are persisted and audit-attributed.

### Billing, founding and finance

- Billing code validation now includes the selected certification for exam/bundle/recertification products.
- Added a product-aware discount preview showing applicability, savings and final amount.
- Browser journey creates a PML-AI-scoped code, rejects it under PCL-AI, then accepts it under PML-AI.
- Founding code creation, evidence application, board approval, membership grant, exam entitlement and zero-value settlement are covered.
- Manual payment, fee waiver, reconciliation, reversal, invoice listing and printable receipt are covered.

### Honorary journey

- Fixed the board shortlist flow clearing a newly generated IDV link.
- Owner approval now returns a fallback one-time account setup URL when email delivery is unavailable.
- Added full public application → shortlist → IDV upload → approval → account setup → award download/verify → revocation coverage.
- Honorary recognition remains distinct from examined credentials.

### Credentials, documents and CPD

- Credential issuance can link to a real student and certification.
- Holder name can be derived from the linked account.
- Added custom certificate upload, holder download, public verification, revoke and reinstate journey.
- Added a CPD review page and admin navigation/route.
- Added student submission → admin approval → updated CPD total coverage.
- Added assigned-document acknowledgement, download and audit coverage.

### Proctoring, support and institution partner journeys

- Added live candidate heartbeat, critical event, two-way chat, proctor review and audit coverage.
- Added visible read-only impersonation banner, forbidden mutation, session end and history assertions.
- Added partner private-document browser download and cross-institution isolation.
- Existing partner provisioning, forced password change, code creation and candidate sponsorship remain covered.

### Audit attribution

Privileged mutations now log the acting admin, while the affected student/user is retained in details. Updated areas include:

- fee waivers and manual finance actions;
- test-user create/reset/delete;
- impersonation end;
- Certuvo provision/suspend/revoke/resend/regenerate/password;
- student status/setup/ID/erasure/grade actions;
- credentials;
- support;
- founding and honorary decisions;
- directory and event attendance.

Known schema limitation: `audit_logs.user_id` is still a single numeric identity and can collide between student/admin number sequences. A future migration should introduce an explicit actor type and actor ID.

### Public website coverage

- Full supported-language sequence: English, Korean, Arabic, Spanish, French, Chinese and Russian.
- Arabic RTL and language persistence.
- Active-language announcement and language-stable dismissal key.
- Every named public policy route opens with crawlable content and a story-named evidence attachment.
- Catalogue and verification cross-browser smoke remain configured.

### CSV security retained during upstream merge

- All CSV exports use the shared encoder.
- Spreadsheet formula markers are neutralised even after leading whitespace.
- Genuine signed numeric values remain numeric.
- Added regression cases for whitespace-prefixed payloads.

## Browser suite

- 22 Playwright spec files.
- 58 Chromium tests.
- 82 configured executions across Chromium, Firefox, WebKit, Pixel 7 and iPhone 15 profiles.
- Story-named success screenshots are attached to Playwright reports.
- Automatic failure screenshot, trace and video remain enabled.

Important new/expanded specs:

- `frontend/e2e/admin-credentials.spec.ts`
- `frontend/e2e/admin-operations.spec.ts`
- `frontend/e2e/admin-proctoring.spec.ts`
- `frontend/e2e/admin-security-rbac.spec.ts`
- `frontend/e2e/portal-account-security.spec.ts`
- `frontend/e2e/portal-billing-founding.spec.ts`
- `frontend/e2e/portal-documents-cpd.spec.ts`
- `frontend/e2e/public-applications.spec.ts`
- `frontend/e2e/public-i18n.spec.ts`
- `frontend/e2e/public-policies.spec.ts`
- `frontend/e2e/partner-portal.spec.ts`

## Locally completed validation

These checks passed:

```text
TypeScript:                 PASS
ESLint:                     PASS, 0 errors and 18 existing warnings
Vitest:                     PASS, 79/79
Frontend production build: PASS
npm production audit:      PASS, 0 vulnerabilities
Backend logic suites:      PASS, six suites
Python compilation:        PASS
git diff --check:          PASS
Playwright discovery:      PASS, 82 executions in 22 files
```

The local environment did not provide the .NET SDK, MySQL service or browser engines. Therefore live backend integration and Playwright execution remain **PENDING-CI**, not passed.

## Validation commands

From `frontend/`:

```bash
npm ci
npm run typecheck
npm run lint
npm test -- --run
npm run build
npm audit --omit=dev --audit-level=high
E2E_NO_SERVER=1 npm run e2e -- --list
```

From the repository root:

```bash
python3 -m py_compile backend/tests/integration_test.py
git diff --check
```

Where the full CI dependencies are available:

```bash
dotnet build backend/PCI.Backend.csproj -c Release
dotnet test backend/tests/PCI.Backend.Tests/PCI.Backend.Tests.csproj -c Release
cd backend
python3 tests/integration_test.py
python3 tests/founding_test.py
cd ../frontend
npx playwright install --with-deps
npm run e2e
```

Run both SQLite and MySQL jobs exactly as configured in `.github/workflows/build.yml`. Do not weaken or remove provider-parity assertions to make CI pass.

## External/operator-only journeys

Do not fake or mark these as passed without approved environments:

- Google OAuth consent/login;
- live Stripe hosted checkout and webhook account;
- Certuvo or another exam-vendor sandbox;
- QuickBooks sandbox;
- deployed Render production smoke;
- interactive Windows SecureExam journey.

## Reports

- `docs/testing/E2E_USER_JOURNEY_REPORT_2026-07-23.md`
- `docs/testing/E2E_EVIDENCE_MANIFEST_2026-07-23.md`
- `docs/testing/TEST_COVERAGE_MATRIX.md`
- `docs/testing/DEFECT_REGISTER.md`

## Importing the bundle on another computer

If the local repository is unavailable, copy `pci-phase2-e2e-fixes.bundle` to the computer running Cursor:

```bash
git clone https://github.com/hariswahidkhan-star/PCI.git PCI
cd PCI
git fetch /path/to/pci-phase2-e2e-fixes.bundle codex/fix-pml-ai:codex/fix-pml-ai
git switch codex/fix-pml-ai
git bundle verify /path/to/pci-phase2-e2e-fixes.bundle
git push -u origin codex/fix-pml-ai
```

## Draft pull-request summary

Suggested title:

```text
Fix remaining cross-role journeys and expand end-to-end coverage
```

Suggested body:

```text
Closes the remaining self-contained student, admin, honorary, founding,
credential, finance, CPD, proctoring, partner-document, localization and
policy-library gaps identified by the end-to-end audit.

It also corrects privileged audit attribution, hardens admin MFA downgrade
protection and recovery handling, adds product/certification-aware discount
preview, fixes Honorary IDV/setup-link delivery, and expands the browser suite
to 82 configured executions across 22 spec files.

Local validation: TypeScript, ESLint (0 errors), 79 Vitest tests, production
builds, six backend logic suites, Python compilation, production npm audit
(0 vulnerabilities), diff checks and Playwright discovery all pass.

Full .NET/MySQL and browser runtime execution remains pending GitHub Actions.
External provider/operator journeys are explicitly not claimed as passed.
```

## Definition of done

- Remote branch exists and points to the completed local head.
- Draft PR is open against `main`.
- Required GitHub Actions jobs run.
- Any branch-caused failures are fixed without weakening tests.
- Browser evidence artifacts are produced only by passing runtime tests.
- Reports are updated with actual CI run URLs and final status.
