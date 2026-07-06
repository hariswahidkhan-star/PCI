# PCI — Website ⇄ Backend Integration Guide (plug-and-play)

Everything below is already built and configured. A developer only performs the **connect** steps.

## 1. Run the backend (5 minutes)
```bash
unzip pci-enrollment-backend.zip && cd pci-enrollment-backend
npm install
cp .env.example .env        # fill in: STRIPE keys, SMTP, ADMIN_TOKEN, ALLOWED_ORIGIN
node src/server.js          # schema + full real-site seed load automatically on first boot
```
Smoke-test: `curl http://localhost:8080/api/health` → `{"ok":true}`
The **admin dashboard is served by the backend** at `http://localhost:8080/admin.html` (sign in with your backend URL + ADMIN_TOKEN).

## 2. Connect the website (one line, site-wide)
Every page already contains `<meta name="pci-api" content=""/>`. Point it at your backend:
```bash
# run inside the unzipped PCI_website folder
sed -i 's|<meta name="pci-api" content=""/>|<meta name="pci-api" content="https://api.yourdomain.com"/>|' *.html
```
That single value activates, automatically, on every page:
- **Enrolment wizard & checkout** → real Stripe checkout (`/api/create-checkout-session`, discount codes, pricing)
- **Login / password set & reset** → `/api/login`, `/api/set-password`, `/api/forgot-password`
- **Contact & corporate forms** → `/api/inquiry` (auto-acknowledgement emails)
- **Credential verification** → `/api/verify`
- **Newsletter “Stay in the loop”** → `/api/newsletter`
- **CMS loader** (`assets/cms-loader.js`) → pulls `/api/content`; applies the announcement banner and any admin-edited text bound via `data-cms` (currently `newsletter_heading`, `footer_tagline`); fails silently if the backend is unreachable, so the site never breaks.

## 2b. Student portal
The backend serves a complete student portal at **`/student.html`** — sign-in (30-day session tokens issued by `/api/login`), exam scheduling with the 12-month window, launch-window exam delivery with server-side scoring, results with domain bands, instant verifiable credential on pass, practice centre, downloads, receipts, and support tickets answered from the admin dashboard's **Support tickets** section. The website's login page hands off automatically once `pci-api` is set.
**Exam resilience:** attempts are server-anchored — answers persist via `/api/me/exam/heartbeat` every few seconds and on every answer, so a crash or power-off resumes with answers intact while the clock keeps running (per policy). Browser-level secure mode records focus/tab violations on the attempt; full device lockdown requires a dedicated secure browser and is stated as such in-product. An in-exam calculator is built in.
**CPD & renewals:** students record CPD (`/api/me/cpd`), renew membership (`renewal`, USD 99/yr) and recertify (`recert`, USD 99/3yr) through the same Stripe checkout; the webhook extends membership/credential validity automatically, and the portal computes live credential standing (active / at-risk / expired) from fee + CPD state.
**Student panel (v2):** the backend serves a rebuilt premium panel at **`/student.html`** covering all fifteen objectives — dashboard, membership status & access rights, resume-enrolment, learning pathway & progress, exam readiness, handbook/policies, profile & account settings, payments/invoices/receipts, announcements & reminders, resources, CPD, support, and clear certification-vs-membership messaging throughout.
**Membership & payments:** `/api/me/invoices` returns fully enriched rows (invoice number `INV-YYYY-NNNNN`, receipt number, product, standard price, discount + code, final amount, currency, method, status); invoices/receipts download as documents; renewal (USD 99/yr) and recertification (USD 99/3yr) run through Stripe with the webhook extending validity.
**Communication centre:** `/api/me/messages` (+`/:id/read`, `/read-all`) with server-generated system announcements, payment/renewal reminders, policy & exam notices, and support replies, categorised (Account/Learning/Certification/Payment/Policy/Support/General) with unread counts.
**Security:** `/api/me/security` (login history + active sessions + device), `/api/me/2fa` toggle, `/api/me/sessions/revoke-others`, `/api/me/account-data` (data export), `/api/me/delete-request` (policy-bound). Login is via 30-day session tokens issued by `/api/login`; passwords are set via secure emailed link (never sent in plain text); magic-link and forgot-password flows are surfaced.
**Resume enrolment:** `/api/enrollment/save` (upsert by email, saves progress every step, issues a resume token) and `/api/enrollment/resume` (by token or email) — message: “Your progress is saved. You can return later using the same email address.” Abandoned sessions are visible to admin.
**Verification deep-link:** `verify.html?id=PCP-AI-…` prefills and auto-runs the public checker; the portal, certificate and score reports all link to it.

## 3. Stripe & email
- Webhook: point Stripe to `POST /api/webhook` with `STRIPE_WEBHOOK_SECRET`. On payment it records the transaction, creates the account, sends receipt + credentials + welcome (and exam confirmation with the 12-month window for exam/bundle).
- SMTP: any provider (examples for SendGrid/Mailgun/Postmark/SES/Zoho/Google in `.env.example`). Add SPF/DKIM/DMARC.
- Abandoned-enrolment reminders: `REMINDER_AUTORUN=1` or cron `POST /api/admin/run-reminders`.

## 4. What the dashboard controls (pre-seeded with the real site)
All **211 pages** (SEO title/description/indexing, live *View* links), navigation, **discount codes v2** (types: general/institution/student/referral/campaign, batch generation, per-person limits, redemption ledger, referral attribution), **business reports** (58 endpoints total), FAQs, Body of Knowledge, sample questions, governance, resources, news/announcement, media (59 real images), newsletter subscribers, form submissions, members, enrolments, payments, exams, credentials, discount codes, pricing, email log, audit log, settings.

## 5. Editable `data-cms` bindings on the site
`newsletter_heading`, `footer_tagline` (safe plain-text). Hero title/subtitle are exposed via `/api/content` (`home_hero_title`, `home_hero_subtitle`, `home_hero_cta`) for template rebuilds; they are intentionally not live-swapped because the hero uses rich markup.

## 6. Environment variables
`APP_BASE_URL, PORT, ADMIN_TOKEN, ALLOWED_ORIGIN, STRIPE_SECRET_KEY, STRIPE_WEBHOOK_SECRET, SMTP_HOST/PORT/SECURE/USER/PASS, MAIL_FROM, REMINDER_AUTORUN, DATABASE_FILE`

## 7. Complete endpoint catalog (auto-generated from the running code)
### Public / website & student APIs (44)
| Method | Endpoint |
|---|---|
| DELETE | `/api/me/cpd/:id` |
| GET | `/api/content` |
| GET | `/api/enrollment/resume` |
| GET | `/api/health` |
| GET | `/api/me` |
| GET | `/api/me/account-data` |
| GET | `/api/me/attempts/:id` |
| GET | `/api/me/cpd` |
| GET | `/api/me/downloads` |
| GET | `/api/me/faqs` |
| GET | `/api/me/invoices` |
| GET | `/api/me/messages` |
| GET | `/api/me/practice` |
| GET | `/api/me/security` |
| GET | `/api/me/tickets` |
| GET | `/api/pricing` |
| GET | `/api/session/resume` |
| GET | `/api/verify` |
| PATCH | `/api/me/profile` |
| POST | `/api/create-checkout-session` |
| POST | `/api/enrollment/save` |
| POST | `/api/forgot-password` |
| POST | `/api/form-submit` |
| POST | `/api/inquiry` |
| POST | `/api/login` |
| POST | `/api/me/2fa` |
| POST | `/api/me/cpd` |
| POST | `/api/me/delete-request` |
| POST | `/api/me/exam/book` |
| POST | `/api/me/exam/heartbeat` |
| POST | `/api/me/exam/reschedule` |
| POST | `/api/me/exam/start` |
| POST | `/api/me/exam/submit` |
| POST | `/api/me/messages/:id/read` |
| POST | `/api/me/messages/read-all` |
| POST | `/api/me/sessions/revoke-others` |
| POST | `/api/me/tickets` |
| POST | `/api/me/tickets/:id/reply` |
| POST | `/api/newsletter` |
| POST | `/api/session/save` |
| POST | `/api/session/start` |
| POST | `/api/set-password` |
| POST | `/api/validate-code` |
| POST | `/api/webhook` |

### Admin APIs (51) — require header `x-admin-token`
| Method | Endpoint |
|---|---|
| DELETE | `/api/admin/` |
| GET | `/api/admin/` |
| GET | `/api/admin/abandoned` |
| GET | `/api/admin/audit` |
| GET | `/api/admin/codes` |
| GET | `/api/admin/codes-v2` |
| GET | `/api/admin/codes/:id/redemptions` |
| GET | `/api/admin/content` |
| GET | `/api/admin/credentials` |
| GET | `/api/admin/emails` |
| GET | `/api/admin/enrollments` |
| GET | `/api/admin/exams` |
| GET | `/api/admin/export` |
| GET | `/api/admin/form_submissions` |
| GET | `/api/admin/inquiries` |
| GET | `/api/admin/members` |
| GET | `/api/admin/members/:id` |
| GET | `/api/admin/overview` |
| GET | `/api/admin/pages` |
| GET | `/api/admin/payments` |
| GET | `/api/admin/payments/:id` |
| GET | `/api/admin/pricing` |
| GET | `/api/admin/reports` |
| GET | `/api/admin/settings` |
| GET | `/api/admin/students` |
| GET | `/api/admin/subscribers` |
| GET | `/api/admin/tickets` |
| GET | `/api/admin/tickets/:id` |
| PATCH | `/api/admin/` |
| PATCH | `/api/admin/codes/:id` |
| PATCH | `/api/admin/content/:id` |
| PATCH | `/api/admin/pages/:id` |
| PATCH | `/api/admin/pricing/:id` |
| PATCH | `/api/admin/settings` |
| PATCH | `/api/admin/subscribers/:id` |
| POST | `/api/admin/` |
| POST | `/api/admin/codes` |
| POST | `/api/admin/codes/generate` |
| POST | `/api/admin/credentials` |
| POST | `/api/admin/credentials/:id/status` |
| POST | `/api/admin/enrollments/:id/remind` |
| POST | `/api/admin/form_submissions/:id/status` |
| POST | `/api/admin/inquiries/:id/status` |
| POST | `/api/admin/members/:id/referral-code` |
| POST | `/api/admin/members/:id/resend-setup` |
| POST | `/api/admin/members/:id/status` |
| POST | `/api/admin/resend-resume` |
| POST | `/api/admin/resend-welcome` |
| POST | `/api/admin/run-reminders` |
| POST | `/api/admin/tickets/:id/reply` |
| POST | `/api/admin/tickets/:id/status` |

> Production security note: front the admin token with real authentication (SSO/2FA, IP allow-list) before managing live data.
