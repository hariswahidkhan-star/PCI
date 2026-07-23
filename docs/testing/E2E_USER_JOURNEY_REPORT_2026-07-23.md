# PCI end-to-end user-journey report — 23 July 2026

## Outcome

The repository now has deeper deterministic coverage for the public visitor, candidate, PCI operator and institution-partner personas. The new browser tranche takes the suite to **14 spec files, 40 Chromium tests and 60 configured executions** across Chromium, Firefox, WebKit, Pixel 7 and iPhone 15 profiles. It also creates named success-state screenshots in the Playwright report, in addition to automatic failure screenshots, traces and video.

This document deliberately does not convert test discovery into a pass. The current sandbox has no .NET SDK or browser engines, and repository writes are denied to the GitHub integration. Therefore:

- the latest upstream PR head `d1adf2f` is independently green in GitHub Actions run [#350](https://github.com/hariswahidkhan-star/PCI/actions/runs/29974673700), including backend SQLite/MySQL, backend unit, frontend, SecureExam Linux/Windows and the prior browser suite;
- the deeper local change set passes every runnable local gate (six backend logic suites, Python/JavaScript syntax, ESLint with zero errors, TypeScript, 30 Vitest tests, production builds, npm production audit, and Playwright discovery of all 60 executions);
- the new .NET integration assertions and browser journeys remain **PENDING-CI** until the local commits can be pushed by a credential with repository write access; and
- live Google/Stripe/vendor/QuickBooks/Windows-GUI journeys remain **BLOCKED** until approved sandboxes or an operator environment exist.

Status meanings: **PASS** = the exact automated contract is green on the upstream CI baseline; **PENDING-CI** = implementation and static discovery are complete but runtime execution is unavailable; **PARTIAL** = important backend/API coverage exists but the complete UI/admin/audit/screenshot journey is not yet proven; **BLOCKED** = requires an external provider, deployed environment or interactive desktop.

## Test-user matrix

| Scenario | Intended first stop | Isolated state now asserted | Browser evidence |
|---|---|---|---|
| `ready` | Exam scheduled | consents, profile, ID, membership and fee complete; Schedule enabled | `C1-ready-test-user.png` (CI pending) |
| `unpaid` | Consents | fresh account; no accidental grants | API matrix (CI pending) |
| `member` | Exam fee | membership complete; no exam entitlement | API matrix (CI pending) |
| `waived` | Exam scheduled | fee waiver visible; Schedule enabled without checkout | Playwright journey (CI pending) |
| `incomplete_profile` | Profile | ID and fee complete; only country/profile blocks | `D1-profile-only-blocker.png` (CI pending) |
| `no_id` | Government ID | profile and fee complete; only ID blocks | `D1-D2-id-only-blocker.png` (CI pending) |
| `certuvo_failed` | Exam fee + Certuvo action | member-facing copy hides diagnostics; admin journey retains exact error | Playwright journey (CI pending) |

The `incomplete_profile` and `no_id` fixtures previously carried multiple unrelated blockers. That was a product-testability defect because the UI could not be evaluated against the scenario name; it is fixed in `AdminOps.ApplyScenario` and protected by both live-HTTP and browser assertions.

## Complete journey register

| STORY-ID | Actor | Status | Admin-controllable? | Screenshot/evidence | Notes |
|---|---|---|---|---|---|
| A1 | Anonymous visitor | PASS | Y | `A1-homepage.png` on next run; prior CI E2E green | Home, heading, language and skip link covered; success capture newly added. |
| A2 | Anonymous visitor | PENDING-CI | Y | `A2-catalogue.png` + one image per certification | API → card → PCL-AI/PFL-AI/PML-AI detail route now asserts names, fees, duration, pass mark and enrolment intent across five profiles. |
| A3 | Anonymous visitor | PARTIAL | Y | `A3-arabic-rtl.png` (CI pending) | French, Arabic RTL, Spanish persistence and visible switcher covered; full en→ko→ar→es→fr→zh→ru copy/nav sequence remains. |
| A4 | Anonymous visitor | PENDING-CI | Y | `A4-announcement-visible.png` | Admin API contract already green; visible dialog, accessibility, dismissal key and reload persistence added. Active-language copy still needs a dedicated assertion. |
| A5 | Honorary applicant | PENDING-CI | Y | `A5-honorary-submitted.png` | Full structured history, certification, eligibility declarations and PDF reach the pending board queue. Existing API decision lifecycle is green. |
| A6 | Training-provider applicant | PENDING-CI | Y | `A6-partner-application-submitted.png` | Browser submission with evidence reaches the admin review queue; API decision lifecycle is already green. |
| A7 | Anonymous visitor | PENDING-CI | Y | `A7-contact-and-newsletter.png` | Contact and newsletter reach their admin lists. Honeypot/rate-limit branches remain API-only. |
| A8 | Anonymous visitor | PENDING-CI | Y | `A8-download-centre.png` | Browser now opens the live register, downloads the file and verifies `%PDF`. |
| A9 | Candidate/verifier | PASS | Y | `A9-E1-credential-verified.png` on next run; prior lifecycle CI green | Issued credential resolves publicly with active status/name/id; unknown-id API path is covered. |
| A10 | Anonymous visitor | PARTIAL | Y | No per-page success captures | Policy routes are covered by route/500/static checks, but every named policy has not been opened and visually evidenced. |
| A11 | Anonymous/crawler | PASS | Y | API/text assertions | `robots.txt`, `llms.txt` and sitemap contracts are automated; no visual screenshot is appropriate. |
| B1 | New candidate | PASS | Y | `B1-registered-dashboard.png` on next run | Browser registration and dashboard are green upstream; confirm-password mismatch and notification ledger are API-covered. |
| B2 | Candidate | PASS | Y | `B2-signed-in.png` on next run | Browser sign-in/out is green; token revocation is also asserted at API level. |
| B3 | Candidate | BLOCKED | Partial/env | None | Requires configured Google OAuth sandbox and consent screen. Local code must not fake this provider journey. |
| B4 | Candidate | PARTIAL | Y | No complete browser capture | Single-use/expiry/no-enumeration/session-revocation contracts are API-tested; forgot/reset UI sequence remains. |
| B5 | Admin-provisioned candidate | PARTIAL | Y | No complete browser capture | Set-password token contract and Honorary provisioning are API-tested; browser single-use/14-day path remains. |
| B6 | Candidate/attacker | PASS | Y | Security API evidence | Per-account ten-failure lock and per-IP controls are automated; a UI screenshot would add little security evidence. |
| B7 | Candidate | PARTIAL | Y | Registration dashboard capture only | Onboarding may be skipped in current browser test; full profile wizard, country source and dashboard-unlock sequence remain. |
| C1 | Candidate/admin | PENDING-CI | Y | `C1-ready-test-user.png` | All seven scenario journey states are checked through the admin model; key blocker states are rendered in the portal. |
| C2 | Candidate/admin | PASS | Y | `C2-three-enrolments.png` on next run | Browser-held PCL-AI/PFL-AI/PML-AI enrolments stay certification-scoped; admin enrolment visibility is API-covered. |
| C3 | Candidate/finance admin | BLOCKED | Y + provider | Mock signed settlement is automated; live Stripe test-card/hosted-checkout journey needs an approved Stripe test account. |
| C4 | Candidate/admin | PARTIAL | Y | No browser capture | Extensive code rejection/limit/price matrix is green; Billing UI valid/invalid/founding interactions remain. |
| C5 | Founding applicant/admin | PARTIAL | Y | No browser capture | Founding code, criteria, application and waiver settlement suites are green; Founding card and admin decision UI remain. |
| C6 | Candidate | PARTIAL | Y | PDF assertions, no invoice UI capture | Invoice/receipt endpoints and PDF bytes are covered; browser invoice-list/download journey remains. |
| D1 | Candidate/admin | PENDING-CI | Y | `D1-profile-only-blocker.png`, `D1-D2-id-only-blocker.png` | Named blockers are now isolated instead of overlapping. |
| D2 | Candidate/admin | PASS | Y | Lifecycle browser + ID API | Real browser lifecycle uploads an ID prerequisite and proceeds; at-rest encryption/storage assertions exist below UI. |
| D3 | Candidate/admin | PASS | Y | `D3-D4-booked-and-rescheduled.png` on next run | Browser booking is green; admin registration visibility is API-covered. |
| D4 | Candidate/admin | PARTIAL | Y | `D3-D4-booked-and-rescheduled.png` on next run | Reschedule is browser-covered; cancel plus reflected admin UI remains API-only. |
| D5 | Candidate/proctor | PASS | Y | `D5-D6-exam-pass.png` on next run | In-house launch, readiness and every seeded question are driven in the real web runner; desktop client is J1. |
| D6 | Candidate/admin | PASS | Y | `D5-D6-exam-pass.png` on next run | Clean score/pass/credential path is browser-covered; held-result thresholds and release are API/unit-covered. |
| D7 | Candidate/proctor admin | PARTIAL | Y | No admin-board capture | Heartbeat/evidence/violations/live-board/dossier contracts are API-covered; proctor console UI journey remains. |
| D8 | Candidate/vendor admin | PARTIAL | Y + provider | Mock connector lifecycle and SSRF/config guards are covered; full provider UI and live sandbox callbacks remain. |
| D9 | Support admin/candidate | PASS | Y | API/audit evidence | Impersonation tokens are read-only on booking/start/submit and privileged actions. UI banner capture remains desirable. |
| E1 | Credential holder/verifier | PASS | Y | `A9-E1-credential-verified.png`, `E1-multi-cert-credential-isolation.png` on next run | Pass → issue → authoritative PDF → QR/public verification is browser-covered. |
| E2 | Candidate | PASS | Y | `E2-pml-materials.png` on next run | PML-AI BoK is downloaded and verified as PDF; missing-file and watermark contracts are API-tested. |
| E3 | Candidate/admin | PARTIAL | Y + provider | Practice/history/provisioning contracts exist; friendly Certuvo failure UI is CI-pending and live Certuvo is external. |
| E4 | Candidate/admin | PARTIAL | Y | No browser capture | Assignment/acknowledgement/download gates and IDOR are API-tested; Documents UI sequence remains. |
| E5 | Honorary member/admin | PARTIAL | Y | Backend PDF evidence | Honorary issuance/download/verify is green in dedicated suites; member browser download remains. |
| E6 | Candidate/admin | PARTIAL | Y | API byte assertions | Examined/Honorary custom certificate precedence is covered below UI; admin upload and student download UI remains. |
| F1 | Candidate/support admin | PENDING-CI | Y | `F1-two-way-ticket-thread.png` | Student submit → admin inbox/reply → student thread/reply is implemented end to end. |
| F2 | Candidate/admin | PARTIAL | Y | No browser capture | CPD submission/review/policy contracts are API/unit-covered; portal/admin UI handoff remains. |
| F3 | Candidate/support admin | PENDING-CI | Y | `F3-support-notification.png` | Admin support reply creates an in-app notification; read-all removes the action and persists through API. |
| F4 | Candidate/privacy admin | PENDING-CI | Y | `F4-erasure-requested.png` | New Profile controls download JSON and submit erasure; admin pending queue and identity/reason are asserted. |
| F5 | Candidate | PARTIAL | Y | `F5-other-sessions-revoked.png` (CI pending) | Other-session revocation UI is complete; TOTP enrol/verify/disable is API-tested but not browser-driven. |
| G1 | Owner/admin | PARTIAL | Y | Existing admin gate E2E | Login/forced-password gate is browser-tested; admin TOTP lifecycle and settings toggle need one UI journey. |
| G2 | Reports admin | PENDING-CI | Y | `G2-analytics-export.png` | Fixes wrong token storage in Analytics export; browser download validates the exact CSV header. Reports UI export breadth remains. |
| G3 | Members admin | PARTIAL | Y | `G3-test-user-created.png` (CI pending) | Students console creates a no-ID persona. Search/journey/edit/impersonation banner/audit still need one consolidated UI test. |
| G4 | Finance admin | PARTIAL | Y | API ledger evidence | Waive/partial/mark-paid/reverse/reconcile are extensively API-covered; decision UI remains. |
| G5 | Certification admin/public | PARTIAL | Y | A2 evidence pending | CRUD/pricing/cache invalidation are API-covered and public catalogue consistency is CI-pending; admin edit UI remains. |
| G6 | Credential admin | PARTIAL | Y | API/PDF evidence | Issue/revoke/reinstate/regenerate/custom-upload contracts exist; complete admin UI journey remains. |
| G7 | Exam/proctor admin | PARTIAL | Y | API evidence | Registrations/proctoring/provider validation/sync/SSRF contracts exist; admin consoles remain lightly browser-tested. |
| G8 | Exam-content admin | PARTIAL | Y | API exam-bank evidence | Question-bank CRUD feeds scoped exams below UI; admin CRUD-to-candidate UI proof remains. |
| G9 | Honorary board admin | PARTIAL | Y | A5 pending + dedicated API suites | Public submission is CI-pending and full board lifecycle is API-green; shortlist/IDV/approve/reject admin UI remains. |
| G10 | Codes admin | PARTIAL | Y | API suites | Approval constraints/fraud/founding coverage is strong below UI; admin workflow UI remains. |
| G11 | Content admin/public | PARTIAL | Y | Public spot checks + API suites | Content modules are broadly API-tested; not every console mutation has a browser reflection capture. |
| G12 | SEO admin/crawler | PARTIAL | Y | robots/llms API evidence | SEO/AI-visibility contracts exist; admin edit → public meta/crawler UI sequence remains. |
| G13 | Partner admin/applicant | PENDING-CI | Y | `A6-partner-application-submitted.png` | Application reaches review queue; partner provisioning is browser-driven in H1. Directory decision UI remains. |
| G14 | Integration admin | BLOCKED | Y + provider | Webhook retry/ledger and fail-closed QuickBooks paths are automated; live QuickBooks/provider delivery needs approved credentials. |
| G15 | Marketing admin/visitor | PENDING-CI | Y | `A7-contact-and-newsletter.png` | Visitor handoff reaches inquiries/subscribers. Broader marketing/submission console decisions remain API-only. |
| G16 | Communications admin | PARTIAL | Y | Notification-history assertions | Many templates/log events are tested; I1's every-message content checklist is not complete in UI. |
| G17 | Auditor | PARTIAL | Y | API audit assertions | Privileged actions and logins are broadly audited, but the prompt's “after every story” attribution sweep is not yet automated. |
| G18 | Owner/access admin | PARTIAL | Y | RBAC/last-owner API evidence | Section denial and ownership guards are green; create/scope/reset UI journey remains. |
| G19 | Owner/settings admin | PARTIAL | Y | Logic/API evidence | Settings enforcement, retention and result-hold decisions are tested; full Settings UI round trip remains. |
| H1 | Institution admin | PENDING-CI | Y | `H1-H5-first-login-and-session-revocation.png` | PCI admin creates institution/user; partner logs in, is forced to change password and reaches dashboard. |
| H2 | Two institutions | PENDING-CI | Y | `H2-sponsored-candidate.png` | Two partners sponsor distinct candidates; each candidate/code view is proven mutually exclusive. Masked redemption roster remains API-covered. |
| H3 | Institution user | PARTIAL | Y | API watermark/isolation evidence | Institution-scoped document listing/download/watermark is covered below UI; browser download remains. |
| H4 | Institution admin | PENDING-CI | Y | `H4-partner-code.png` | Partner creates an active constrained code and sees it; API suites enforce ceilings and usage isolation. |
| H5 | Institution admin | PENDING-CI | Y | `H1-H5-first-login-and-session-revocation.png` | A second pre-change session becomes 401 while the changing browser session remains active. |
| I1 | Every message recipient | PARTIAL | Mostly Y | Email-history assertions, no complete screenshot set | Individual flows/templates are tested across suites, but every named template, final content and configurability flag has not been rolled into one audit. |
| J1 | SecureExam candidate/proctor | BLOCKED | Y | Upstream Windows build/tests only | Core and Windows CI are green, but a real interactive desktop launch/proctor/held-result journey needs a Windows GUI runner/operator. |
| J2 | Integration subscriber | PARTIAL | Y | API/HMAC/retry ledger | HMAC, SSRF guard, idempotency, retry and ledger paths are automated; a live receiving endpoint remains external. |
| J3 | Finance integration admin | BLOCKED | Y + provider | Clean unconfigured/fail-closed API evidence | Live Customer/SalesReceipt mapping and delivery requires an approved QuickBooks sandbox. |

## Confirmed fixes in this tranche

1. **Scenario fidelity:** `incomplete_profile` and `no_id` now isolate exactly one candidate blocker; regression checks cover the stage model and visible portal state.
2. **Analytics export authentication:** the React admin page now reads the actual `sessionStorage` token through `adminApi.getToken()` instead of the unused `localStorage` key, checks HTTP errors and reliably mounts/removes the download anchor.
3. **CSV formula injection:** attacker-controlled strings beginning with optional whitespace plus `=`, `+`, `-` or `@` are forced to text in Analytics, generic admin and discount-report exports. Real numeric negative values remain numeric. Integration §59o probes all three paths.
4. **Privacy usability:** existing account-data, session-revocation and deletion-request APIs now have usable student Profile controls with success/error feedback and a real JSON download.
5. **Test-only administration:** deterministic owner access exists only when explicitly enabled outside Production. An unset environment is treated as Production, and both standard .NET environment variables are honoured.
6. **Evidence:** covered success states attach story-named screenshots to the Playwright HTML report; failure screenshots, traces and videos remain automatic.

## Prioritised remaining work

1. **P0 — run the new branch in CI.** The local repository is ready, but both HTTPS push (no credential) and the GitHub integration write endpoint (`403 Resource not accessible by integration`) are blocked. A repository writer should push/cherry-pick the local commits, then require green backend, backend-mysql, backend-unit, frontend, E2E, SecureExam and static-quality jobs before merge.
2. **P1 — close the highest-risk browser gaps:** password reset/set-password; full onboarding; Founding route; student/admin TOTP; document acknowledgement; CPD; admin finance/credential/proctoring/Honorary/RBAC decisions; institution document download.
3. **P1 — complete audit attribution:** assert the acting admin/user, timestamp and detail for each privileged story, not only representative operations.
4. **P1 — provider/operator runs:** Google OAuth, live Stripe test checkout, one exam-vendor sandbox, QuickBooks sandbox, deployed Render smoke and an interactive Windows SecureExam journey.
5. **P2 — evidence completeness:** add success screenshots for every remaining browser story and a manifest that maps each attachment name to its story and CI run.
6. **P2 — broaden cross-browser/mobile tags:** current five-profile execution deliberately targets stable public/auth/catalogue smoke; authenticated decision screens still run in Chromium only.

No unexecuted provider journey is reported as a pass, and no screenshot is claimed to exist until the CI runner has produced the corresponding artifact.
