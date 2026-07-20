# PCI Platform — End-to-End Test Plan, User Stories & Audit Prompt

> Purpose: a single source of truth for testing **every** process on the platform end-to-end —
> public website, student portal, admin console, institution portal, exam software, integrations,
> emails, downloads, and the audit log. Use it to (a) drive a manual/scripted walk-through with a
> test user and sample data, and (b) hand the **audit prompt** (Section 1) to an agent per area.
>
> Scale note: there are **~120 user stories** below. One session cannot screenshot them all — run
> them in **batches by area** (Section 4), one agent/session per batch, each producing screenshots
> and a pass/fail line per story.

---

## 1. The audit prompt (hand one copy per area to an agent)

```
You are QA-testing the PCI platform end-to-end for AREA = "<area name>" (e.g. "Student portal —
enrolment & payment"). A server is running at BASE_URL with a seeded test student and owner admin
(see Section 2). Chromium (Playwright) is available.

For every user story in the assigned area (Section 3):
1. Drive the exact steps in a real browser as the stated actor.
2. Assert the "Expected" outcome. If the API is easier for a step, use it, but the final
   user-visible result MUST be checked in the UI.
3. Capture a screenshot at the key moment (named <STORY-ID>.png) and, where relevant, the
   before/after.
4. Record: PASS / FAIL / BLOCKED, the evidence (screenshot name + one line), and — if FAIL — the
   exact request/response or console error, and whether the same thing is controllable from the
   ADMIN portal (the platform requirement is that everything is admin-controllable).
Also verify each action wrote the expected row to the AUDIT LOG (Section 3.G) attributed to the
acting user. Do not stop at the first failure — complete the whole area, then return a table:
STORY-ID | actor | result | admin-controllable? | evidence | notes.
Be adversarial about data isolation (one student must never see another's data) and about
"admin-controllable" (flag anything only changeable in code/DB).
```

---

## 2. Test users & sample data

Create these once per test run (admin has a one-click "test user" tool; test accounts are excluded
from reports and never issue public credentials):

| Actor | How | Credentials |
| --- | --- | --- |
| Owner admin | seeded | `owner@pci.local` / `changeme-owner` (change on first login) |
| Test student (fully unlocked) | Admin → Students → **Create test user** | generated; use the shown temp password |
| Second student (isolation checks) | signup on the public site | `qa.two@example.test` |
| Institution/partner user | Admin → Training Partners → add partner + user | generated temp password |

Sample profile data to use: Name "QA Tester", Country "Pakistan" (also test "Saudi Arabia" and
"United States" for cross-border wording), Org "QA Ltd", 8+ years experience / 3+ managerial (for the
honorary path), a small JPEG/PNG as photo, a small PDF as ID/evidence.

---

## 3. User-story catalog

Format: **ID — As a `<actor>`, I want `<goal>`** · _Steps_ → **Expected** · Admin-controllable?

### A. Public website (anonymous visitor)
- **PW-01** Browse the homepage. → Loads, hero + catalogue render, no console errors. · Content admin-editable (Pages/Content).
- **PW-02** Open each certification page (PCL-AI / PFL-AI / PDL-AI / all-certifications). → Correct copy + price tags. · Yes (Pricing, Content).
- **PW-03** Switch language (en→ko→ar→es→fr→zh→ru). → Page + nav translate; Arabic renders RTL; choice persists via cookie. · Yes (Translations).
- **PW-04** See the launch announcement popup; dismiss it. → Shows once, dismissable, stays dismissed; renders in the active language. · Yes (Announcement).
- **PW-05** Submit the **Honorary Fellow** application (eligibility, qualifications/certs/experience rows, consents, T&C). → Reference returned; ack email queued. · Yes (Honorary applications).
- **PW-06** Submit a **Training Partner** application. → Reference returned. · Yes (Training Partners).
- **PW-07** Submit contact / newsletter / a form. → Success; rate-limited against spam; lands in admin inbox. · Yes (Enquiries/Submissions/Subscribers).
- **PW-08** Use the public **Downloads Centre**; download a public document (PDF). → File downloads. · Yes (Downloads Centre).
- **PW-09** **Verify a credential** at `verify.html?id=<award/credential>`. → Valid → shows holder + status; unknown → not found. · Yes (Credentials).
- **PW-10** View policy pages (privacy, data-protection, retention, appeals, etc.). → All load. · Yes (Pages/Docs).
- **PW-11** robots.txt / llms.txt / sitemap. → Served, sane. · Yes (AI Visibility/SEO).

### B. Account creation & authentication
- **AU-01** Sign up with email + confirm-password. → Account created; welcome/verification email; mismatch is rejected server-side.
- **AU-02** Sign in / sign out. → Session works; logout revokes the token immediately.
- **AU-03** Google sign-in (if configured). → Account linked/created.
- **AU-04** Forgot password → reset link → set new password. → Link is single-use, expires; other sessions revoked; enumeration-safe (same response for unknown email).
- **AU-05** Set-password link from an admin-provisioned account. → Works once, 14-day expiry.
- **AU-06** Brute-force: 10 wrong passwords. → Account temporarily locked (per-account), plus per-IP throttle.
- **AU-07** Complete the profile wizard (country, details). → Saved; dashboard unlocks. · Country list admin-sourced.

### C. Student portal — membership, enrolment, payment
- **ST-01** View dashboard + journey ("where am I"). → Correct stage + recommended actions.
- **ST-02** Explore certifications, enrol. → Enrolment recorded. · Admin sees it (Enrolments).
- **ST-03** Checkout an exam/membership (Stripe test card `4242…`). → Payment recorded; receipt/invoice; membership activates. · Admin can waive / mark-paid (Payments).
- **ST-04** Apply a discount code at checkout. → Price adjusts; usage limits enforced. · Yes (Discount codes).
- **ST-05** Founding-stage access via code. → Route unlocked. · Yes (Founding).
- **ST-06** Download receipt / invoice. → PDF downloads.

### D. Student portal — exam lifecycle (in-house + vendor)
- **EX-01** Booking blocked before consents/profile/payment. → Correct block reason shown.
- **EX-02** Upload mandatory government ID for exam booking. → Stored (encrypted at rest); booking proceeds.
- **EX-03** Book an exam slot. → Booked; confirmation. · Admin sees registration.
- **EX-04** Reschedule / cancel. → Updated. · Admin visible.
- **EX-05** Start the exam; SecureExam launch code / launcher. → Launches; heartbeat runs.
- **EX-06** Submit answers; receive score / held-result screen. → Scored or held per policy. · Result-hold thresholds admin-set (Settings).
- **EX-07** Proctoring session appears live to admin; violations logged. → Yes (Proctoring).
- **EX-08** **Vendor delivery** (Pearson/Kryterion/etc. or the mock): provider provisions candidate, schedules, returns result. → Order status advances end-to-end. · Yes (Exam delivery vendors; mode switch in-house↔vendor).
- **EX-09** Impersonation ("view as student") is READ-ONLY for exam actions. → Book/start/submit refused in support view.

### E. Student portal — credentials, downloads, resources
- **CR-01** After a pass, credential is issued; download the **certificate PDF** (with QR). → Downloads; QR verifies.
- **CR-02** Download **Books & materials** (BoK/handbook) — personalised watermark. → PDF downloads watermarked; "no file" items show no download. · Admin uploads the files (Books & materials).
- **CR-03** Certuvo practice: start, answer, see explanation + history. → Works; credentials provisioned.
- **CR-04** Documents module: acknowledge a required doc, then download. → Download blocked until acknowledged.
- **CR-05** Honorary certificate download (for honorary members). → Downloads.

### F. Student portal — support & account
- **SU-01** Raise a support ticket; reply. → Thread works. · Admin inbox + SLA.
- **SU-02** Submit CPD evidence. → Recorded. · Admin visible.
- **SU-03** Notifications / messages: mark read / read-all. → State persists.
- **SU-04** Export my account data; request deletion. → Export downloads; deletion request queued. · Admin sees request.
- **SU-05** Enrol student TOTP; revoke other sessions. → Works.

### G. Admin console — everything is admin-controllable (the core requirement)
- **AD-01** Login, 2FA enrol/verify/disable, forced-password-change **toggle** (Settings). → Works; toggle makes new-admin password change optional.
- **AD-02** Dashboard, Reports (+CSV), Analytics. → Data renders/exports.
- **AD-03** Students: search, view journey, impersonate (banner + audit), edit. → Works; impersonation audited.
- **AD-04** Enrolments / Payments: waive, partial waive, mark-paid, reverse, reconcile. → Ledger correct.
- **AD-05** Certifications CRUD; Pricing rules. → Public catalogue + price tags update.
- **AD-06** Credentials: issue, revoke/reinstate, regenerate PDF, **upload a custom certificate PDF (examined OR honorary)**. → Uploaded PDF is what the student/admin download returns.
- **AD-07** Exam registrations, Proctoring, **Exam delivery vendors** (configure provider, api_base validated, sync). → Works; SSRF-guarded.
- **AD-08** Question bank CRUD. → Persists; feeds exams.
- **AD-09** Honorary fellows + **Honorary applications**: review, shortlist → IDV link email, approve → **auto student account + membership + certificate + email**, reject. → Full flow; IDV owner-only + audited.
- **AD-10** Discount codes (approval workflow, constraints, fraud flags); Founding stage. → Enforced.
- **AD-11** Website: Pages, Content, **Announcement**, **Translations**, Reviews, FAQs, Resources, News, BoK, Governance, Nav, **Books & materials (upload PDFs)**, **Downloads Centre**. → All editable; changes reflect on the public site.
- **AD-12** SEO, AI Visibility. → Robots/llms/meta update.
- **AD-13** Training Partners: review applications, manage partners, directory. → Works.
- **AD-14** Integrations & ERP: webhooks + QuickBooks (config, test, deliveries, retry). → Delivers (SSRF-guarded); ledger.
- **AD-15** Marketing, Subscribers, Form submissions, Enquiries. → Manageable.
- **AD-16** Emails: view the email log (every sent mail). → Present.
- **AD-17** **Audit log**: every privileged action + **admin login** is recorded, attributed to the acting admin (user id, not 0), with timestamp/details. → Verify each story above wrote its audit row.
- **AD-18** Team & Access (RBAC): create admin (forced-change per the toggle), set permissions/cert-scope, reset password, last-owner guard. → Enforced.
- **AD-19** Settings: platform/web/exam/student-panel keys; retention days; result-hold thresholds. → Applied.

### H. Institution / partner portal
- **PA-01** Partner login (+ forced first-change), dashboard. → Works.
- **PA-02** View only THIS institution's students; PII masked per privacy settings. → Isolation holds (partner A ≠ partner B).
- **PA-03** Institution documents (watermarked). → Download works, scoped.
- **PA-04** Partner-linked discount codes with enforced limits + usage view. → Enforced.
- **PA-05** Partner password change revokes other sessions. → Verified.

### I. Emails (every transactional mail)
- **EM-01..n** Trigger and confirm each mail exists in the email log with correct content: signup/welcome, set-password, password reset, honorary ack / under-review / approved (with certificate + login guidance) / rejected, IDV link, exam booking/reschedule, payment receipt, credential issued, support replies, partner invite. · Templates admin-configurable where applicable.

### J. Integrations & desktop
- **IN-01** SecureExam desktop: launch, proctoring, held-result screen. → Works on Windows build.
- **IN-02** Webhook fan-out on payment.recorded / membership.activated / member.registered (HMAC-signed, SSRF-guarded, retried). → Delivered + ledgered.
- **IN-03** QuickBooks SalesReceipt/Customer mapping. → Delivered or cleanly skipped when unconfigured.

---

## 4. Execution plan (batch across sessions/agents)

Run one agent/session per batch; each returns the Section-1 table + screenshots. Suggested split (≈10 batches):

1. Public website (PW) + Verify + Downloads
2. Auth & account creation (AU)
3. Membership/enrolment/payment (ST)
4. Exam lifecycle in-house (EX-01..07, EX-09)
5. Exam vendor delivery (EX-08) + Integrations (IN)
6. Credentials/downloads/resources/Certuvo (CR)
7. Support & account (SU) + Emails (EM)
8. Admin console part 1 (AD-01..10)
9. Admin console part 2 (AD-11..19) — content, honorary, audit, RBAC, settings
10. Institution portal (PA) + SecureExam desktop (IN-01)

**Definition of done per story:** UI screenshot of the expected outcome, a PASS/FAIL line, the audit-log
row (Section 3.G AD-17), and a note on admin-controllability. Roll up failures into a fix list.

---

_Regression backing:_ the repo already ships automated suites that exercise most of these flows
headlessly — `backend/tests/integration_test.py` (406 assertions, SQLite + MySQL),
`honorary_application_test.py` (48), and `sweep_500_test.py` (every route, no 5xx). This plan adds the
**human/UI + screenshot** layer on top, per user story.
