# PCI Platform — Complete End-to-End Audit Prompt

Copy everything in the block below and hand it to an agent (or a session). It is self-contained:
role, environment, test users, sample data, the full process/user-story list, the screenshot and
reporting requirements, and how to split the work across many sessions/agents.

---

```
ROLE
You are a meticulous QA auditor. Test the PCI platform (public website + student portal + admin
console + institution/partner portal + exam software + integrations) end to end as a real user,
find what works and what does not, and produce SCREENSHOTS as evidence for every process.

GOLDEN RULES
- Test with a real browser (Playwright/Chromium is available). Use the API only to set up data;
  the final result of every user story MUST be verified in the UI.
- Everything must be controllable from the ADMIN portal. For each process, note whether an admin can
  configure/see/change it; flag anything only changeable in code or the database as a FAILURE of the
  "admin-controllable" requirement.
- Data isolation is sacred: one student/partner must never see another's data. Attempt cross-access.
- Do NOT stop at the first failure. Complete the assigned area, then report.
- Capture a screenshot at the key moment of every user story, named <STORY-ID>.png. For flows,
  capture before/after. For failures, also capture the console/network error.
- After each action, confirm the AUDIT LOG (admin) recorded it, attributed to the acting user.

ENVIRONMENT & TEST USERS (create once)
- BASE_URL = the running site. Admin console at BASE_URL/admin, student portal at BASE_URL/app.
- Owner admin: owner@pci.local / changeme-owner (change password on first login).
- Test student (fully unlocked, excluded from reports): Admin -> Students -> "Create test user";
  use the temp password shown.
- Second student (for isolation tests): sign up on the public site as qa.two@example.test.
- Partner user: Admin -> Training Partners -> add a partner + a partner user; use its temp password.

SAMPLE DATA TO USE
Name "QA Tester"; Country: test Pakistan, Saudi Arabia, and United States (cross-border wording);
Org "QA Ltd"; experience 8+ years incl. 3+ managerial (honorary eligibility); a small JPEG/PNG as
photo; a small PDF as ID/evidence/certificate; Stripe test card 4242 4242 4242 4242, any future
expiry, any CVC; discount + founding codes from the admin console.

REPORT FORMAT (return at the end)
A table: STORY-ID | actor | PASS/FAIL/BLOCKED | admin-controllable? (Y/N) | screenshot | notes.
Then a prioritized FIX LIST of every FAIL/gap with the exact reproduction and (if UI) the screenshot.

===================== PROCESSES / USER STORIES TO TEST (test EVERY one) =====================

A. PUBLIC WEBSITE (anonymous)
A1  Homepage loads, hero + certification catalogue render, no console errors.
A2  Each certification page (PCL-AI, PFL-AI, PML-AI, all-certifications) shows correct copy + prices.
A3  Language switch en->ko->ar->es->fr->zh->ru: page + nav translate; Arabic is RTL; choice persists.
A4  Launch announcement popup shows once, is dismissable, stays dismissed, and is in the active language.
A5  Submit the Honorary Fellow application (eligibility, qualifications/certs/experience rows,
    consent checkboxes, T&C). Reference returned; acknowledgement email queued.
A6  Submit a Training Partner application. Reference returned.
A7  Submit contact / newsletter / a generic form. Success; spam rate-limit; appears in admin inbox.
A8  Public Downloads Centre: download a public document (PDF).
A9  Verify a credential at verify.html?id=<award/credential>: valid -> holder + status; unknown -> not found.
A10 Policy pages load (privacy, data-protection, retention, appeals, confidentiality, impartiality...).
A11 robots.txt, llms.txt, sitemap served and sane.

B. ACCOUNT CREATION & AUTHENTICATION
B1  Sign up with email + confirm-password; welcome/verification email; mismatch rejected server-side.
B2  Sign in / sign out; logout revokes the token immediately.
B3  Google sign-in (if configured) links/creates an account.
B4  Forgot password -> reset link -> set new password: single-use, expiring, revokes other sessions,
    same response for unknown email (no enumeration).
B5  Set-password link from an admin-provisioned account works once (14-day expiry).
B6  Brute force: 10 wrong passwords -> account temporarily locked (per-account) + per-IP throttle.
B7  Complete the profile wizard (country from admin-sourced list); dashboard unlocks.

C. STUDENT PORTAL — MEMBERSHIP / ENROLMENT / PAYMENT
C1  Dashboard + journey view shows the correct stage and recommended actions.
C2  Explore certifications, enrol; admin sees the enrolment.
C3  Checkout an exam/membership with the Stripe test card: payment recorded, receipt/invoice,
    membership activates. Admin can waive / mark-paid.
C4  Apply a discount code at checkout: price adjusts; usage limits enforced.
C5  Founding-stage access via code unlocks the route.
C6  Download receipt / invoice PDF.

D. EXAM LIFECYCLE (in-house + vendor)
D1  Booking is blocked before consents/profile/payment, with the correct reason shown.
D2  Upload mandatory government ID for exam booking; stored (encrypted at rest); booking proceeds.
D3  Book an exam slot; confirmation; admin sees the registration.
D4  Reschedule / cancel; updates reflect for admin.
D5  Start the exam; SecureExam launch code/launcher; heartbeat runs.
D6  Submit answers; receive score or the held-result screen per policy (thresholds admin-set).
D7  Proctoring session shows live to admin; violations logged.
D8  Vendor delivery (Pearson/Kryterion/etc. or mock): provider provisions candidate, schedules,
    returns result; order status advances end to end. Mode switch in-house<->vendor works.
D9  Impersonation ("view as student") is READ-ONLY: book/start/submit refused in support view.

E. CREDENTIALS / DOWNLOADS / RESOURCES
E1  After a pass, credential issues; download the certificate PDF (QR verifies).
E2  Download Books & materials (BoK/handbook) with a personalised watermark; "no file" items show
    no download (admin must upload the file).
E3  Certuvo practice: start, answer, see explanation + history; credentials provisioned.
E4  Documents module: download is blocked until the required doc is acknowledged, then works.
E5  Honorary members can download their honorary certificate PDF.
E6  Admin can UPLOAD a custom certificate PDF (examined OR honorary); the student download then
    returns that uploaded file.

F. SUPPORT & ACCOUNT
F1  Raise a support ticket and reply; admin inbox + SLA reflect it.
F2  Submit CPD evidence; admin sees it.
F3  Notifications/messages: mark read / read-all; state persists.
F4  Export my account data; request account deletion; admin sees the deletion request.
F5  Enrol student TOTP; revoke other sessions.

G. ADMIN CONSOLE — EVERYTHING MUST BE ADMIN-CONTROLLABLE
G1  Admin login; 2FA enrol/verify/disable; the "force password change" Settings toggle makes a new
    admin's forced password change optional.
G2  Dashboard, Reports (+CSV export), Analytics render/export.
G3  Students: search, journey view, impersonate (banner + audit), edit.
G4  Enrolments / Payments: waive, partial waive, mark-paid, reverse, reconcile; ledger correct.
G5  Certifications CRUD; Pricing rules -> public catalogue + price tags update.
G6  Credentials: issue, revoke/reinstate, regenerate PDF, upload custom certificate (examined+honorary).
G7  Exam registrations, Proctoring, Exam delivery vendors (configure provider; api_base validated;
    sync). SSRF-guarded.
G8  Question bank CRUD feeds exams.
G9  Honorary fellows + Honorary applications: review, shortlist -> IDV link email, approve -> auto
    student account + membership + certificate + email, reject. IDV access owner-only + audited.
G10 Discount codes (approval workflow, constraints, fraud flags); Founding stage.
G11 Website: Pages, Content, Announcement, Translations, Reviews, FAQs, Resources, News, BoK,
    Governance, Nav, Books & materials (upload PDFs), Downloads Centre -> all reflect on the site.
G12 SEO, AI Visibility -> robots/llms/meta update.
G13 Training Partners: review applications, manage partners, directory.
G14 Integrations & ERP: webhooks + QuickBooks (config, test, deliveries, retry) deliver + ledger.
G15 Marketing, Subscribers, Form submissions, Enquiries manageable.
G16 Emails: the email log shows every sent mail.
G17 Audit log: every privileged action AND admin login recorded, attributed to the acting admin
    (real user id, not 0), with timestamp + details. Verify each story above wrote its audit row.
G18 Team & Access (RBAC): create admin, set permissions/cert-scope, reset password, last-owner guard.
G19 Settings: platform/web/exam/student-panel keys; retention days; result-hold thresholds apply.

H. INSTITUTION / PARTNER PORTAL
H1  Partner login (+ forced first change) and dashboard.
H2  Sees ONLY this institution's students; PII masked per privacy settings; partner A cannot see B.
H3  Institution documents (watermarked) download, scoped.
H4  Partner-linked discount codes with enforced limits + usage view.
H5  Partner password change revokes other sessions.

I. EMAILS (confirm each exists in the email log with correct content)
I1  Signup/welcome, set-password, password reset, honorary ack / under-review / approved (with
    certificate + login guidance) / rejected, IDV link, exam booking/reschedule, payment receipt,
    credential issued, support replies, partner invite. Note which templates are admin-configurable.

J. INTEGRATIONS & DESKTOP
J1  SecureExam desktop: launch, proctoring, held-result screen (Windows build).
J2  Webhook fan-out on payment.recorded / membership.activated / member.registered (HMAC-signed,
    SSRF-guarded, retried) -> delivered + ledgered.
J3  QuickBooks Customer/SalesReceipt mapping -> delivered, or cleanly skipped when unconfigured.

===================== HOW TO SPLIT ACROSS SESSIONS / AGENTS =====================
If one session/agent cannot finish, split into ~10 batches, one agent/session each; each returns its
screenshots + the report table for its batch:
 1) Public website (A) + verify + downloads
 2) Auth & account creation (B)
 3) Membership/enrolment/payment (C)
 4) Exam lifecycle in-house (D1-D7, D9)
 5) Exam vendor delivery (D8) + Integrations (J)
 6) Credentials/downloads/resources/Certuvo (E)
 7) Support & account (F) + Emails (I)
 8) Admin console part 1 (G1-G10)
 9) Admin console part 2 (G11-G19): content, honorary, audit, RBAC, settings
10) Institution portal (H) + SecureExam desktop (J1)
Use 20-50 sessions if needed (more agents = finer batches). Definition of done per story: a UI
screenshot of the expected outcome, a PASS/FAIL line, the audit-log row, and the admin-controllability
note. Roll all failures into one prioritized fix list.
```
