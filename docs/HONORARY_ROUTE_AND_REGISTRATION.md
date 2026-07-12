# Honorary Route application, registration & notification changes

This document summarises the feature set added in this change: registration hardening, the public
certification-routes table, the **Honorary Route public application system** (apply → board review →
conferral) with document uploads, a reusable **notification service** with admin-configurable settings,
and the tests that cover it all.

---

## 1. Registration — Confirm Password

- **`frontend/src/pages/Register.tsx`** — added a **Confirm password** field; the form blocks submission
  when it does not match the password (inline message + submit-time error). `confirmPassword` is sent to
  the server for a defence-in-depth re-check.
- **`frontend/src/auth/AuthContext.tsx`** — `register()` type now accepts `confirmPassword`.
- **`frontend/src/api/client.ts`** — friendly copy for the `password_mismatch` error code.
- **`backend/Endpoints/Account.cs`** (`POST /api/register`) — server-side: if `confirmPassword`/`confirm_password`
  is present and differs from `password`, returns `400 password_mismatch`. A mismatched pair can never
  create an account even if the client check is bypassed.

## 2. Country dropdown (list of values, single source)

Already delivered in the prior change and reused here: **`frontend/src/data/countries.ts`** is the single
source for the ~200-country list + dialling codes, consumed by `Register.tsx` and `Onboarding.tsx`
(country `<select>`; phone dial-code auto-fills from the country). The public honorary form ships its own
inline copy because it is a static HTML page that cannot import the TS module.

## 3. Public certification-routes table

- **`backend/wwwroot/membership.html`** — the existing "Three routes into PCI" section now includes a
  responsive **Route / Fee / Exam / Result / Who it's for** table (horizontal-scroll wrapper for mobile),
  matching the approved spec:

  | Route | Fee | Exam | Result |
  |---|---|---|---|
  | Standard (always on) | Paid | Yes | PCP-AI (earned) |
  | Founding (time-boxed, one tier) | Free | Yes | PCP-AI (earned) |
  | Honorary (board-conferred) | Free | No | Honorary Fellow (PCI) — labelled honorary |

  The Honorary card/row now links to the new application form and states the recognition is conferred at
  the board's discretion (never the examined PCP-AI credential).

## 4–5. Honorary Route application (public) + board review (admin)

Modelled on the existing founding-application flow and identity-document upload pipeline.

- **Public form — `backend/wwwroot/honorary-application.html`** (linked from the routes section): sections
  for Personal information, Professional information, Qualifications, document uploads (Résumé/CV
  **required**; academic, certifications, supporting optional), and a declaration + consent checkbox.
  Files are read to data URIs client-side (type + 3 MB checks) and POSTed as JSON. On success it shows the
  application **reference**. Uses the shared site header/footer/CSS.
- **Backend — `backend/Endpoints/HonoraryApplication.cs`** (registered in `Program.cs`):
  - `POST /api/honorary-application` *(public, rate-limited)* — validates every required field + email
    format + declaration; validates and stores each document through `Storage` (MIME allow-list + magic-byte
    sniff + 3 MB cap); mints a unique `PCI-HONAPP-YYYY-NNNN` reference; inserts the application + documents;
    fires applicant + admin notifications. Returns `{ ok, reference }`.
  - `GET /api/admin/honorary-applications?status=` *(owner-only)* — list with document counts.
  - `GET /api/admin/honorary-applications/{id}` *(owner-only)* — full applicant record + document metadata
    (never exposes `storage_ref`/`sha256`).
  - `GET /api/admin/honorary-applications/{id}/documents/{docId}/file` *(owner-only)* — streams a stored file.
  - `POST /api/admin/honorary-applications/{id}/decide` *(owner-only)* — `approved | rejected | under_review`
    (or note-only). **Approve** confers a real, verifiable honorary award via the shared
    `Honorary.ConferAward(...)` (mints `PCI-HON-YYYY-NNNN`, links the applicant's account if one exists),
    stamps `award_no` and marks the application approved. Each decision emails the applicant.
- **`backend/Endpoints/Honorary.cs`** — extracted the award-number generation + insert into a shared
  `public static string? ConferAward(...)` used by both manual conferral and application approval.
- **Admin UI — `frontend/src/admin/pages/HonoraryApplications.tsx`** (owner-only), wired in
  `AdminApp.tsx` (route) and `AdminLayout.tsx` (nav → *Access & pricing → Honorary applications*): status
  filter, list, and a detail drawer to view all fields, **download** each document, add an internal/decision
  note, and **Approve & confer / Reject / Mark under review**.

**Invariant preserved:** approving an honorary application never creates an `issued_credentials`,
`exam_entitlements` or `exam_attempts` row — it is honorary recognition, always labelled honorary, never
the examined PCP-AI credential (asserted by test `ha3f`).

## 6–8. Reusable notification service + admin configuration

- **`backend/Core/Notify.cs`** — a channel-based notification helper. `Notify.Email(...)` sends via the
  existing provider-agnostic `Mailer` (Resend / SMTP / console) **and** records every attempt in a new
  `notification_history` ledger. `Channel { Email, Sms, InApp }` is the seam for future SMS / in-app
  delivery. `Notify.AdminEmail(db)` resolves the recipient from settings (with fallbacks);
  `Notify.Enabled(db, "honorary")` reads the per-event on/off flag.
- **Configurable, never hardcoded** — new owner-editable keys appear automatically in **Admin → Settings**
  (Platform group): `notify_admin_email` (board recipient), `notify_honorary_enabled` (on/off),
  `notify_honorary_ack_subject` / `notify_honorary_admin_subject` (subject overrides; blank = sensible
  default). SMTP/sender remain environment-configured through `Mailer` (`RESEND_API_KEY` / `SMTP_*` /
  `MAIL_FROM`).
- **Emails sent:** applicant receives a confirmation with the reference + "under review" explanation on
  submission, and a decision email on approve/reject/under-review; the board recipient receives an alert
  containing the applicant name, email, country, submission date, reference, document count and a link to
  the admin portal (files download securely from the application detail).

## 9. Security

- Uploads are validated by the shared `Storage` pipeline: **MIME allow-list** (`pdf/jpg/png/webp`), **3 MB**
  hard cap, and **magic-byte sniffing** (renamed payloads are rejected). Enforced server-side regardless of
  the client checks.
- All form inputs are trimmed and length-capped server-side; email is regex-validated; the public endpoint
  is added to the per-IP **rate limiter** (`Program.cs _rlPaths`).
- Admin review is **owner-only** (401 unauthenticated / 403 non-owner). Document downloads are gated and
  scoped to the parent application.

## 10. Database changes

Added to **`backend/schema.sql`**, **`backend/schema.mysql.sql`**, and idempotently in
**`backend/Data/Migrate.cs`** (so existing live DBs upgrade without a wipe):

- **`honorary_applications`** — reference, applicant personal/professional/qualification fields,
  `declaration`, `status` (`pending_review | under_review | approved | rejected`), `award_no`, `decided_by`,
  `decided_at`, `admin_note`, `created_at`, `updated_at`.
- **`honorary_application_documents`** — `application_id`, `doc_kind` (`resume | academic | certifications |
  supporting`), `filename`, `mime`, `size_bytes`, `storage_ref`, `sha256`.
- **`notification_history`** — `channel`, `recipient`, `subject`, `status`, `related_type`, `related_id`.
- **`site_settings`** seeds — `notify_honorary_enabled`, `notify_admin_email`,
  `notify_honorary_ack_subject`, `notify_honorary_admin_subject`.

## 11. Tests

- **`backend/tests/honorary_application_test.py`** (wired into CI — `.github/workflows/build.yml`, both the
  SQLite and MySQL jobs): 20 assertions — valid submission + reference; required-field / declaration /
  résumé / file-type / email validation; owner-only RBAC; list / detail / **document download**; approve →
  **verifiable PCI-HON award**; no exam-shaped rows created; double-approve 409; notification-ledger rows;
  registration confirm-password server-side enforcement.
- **`backend/smoke-test.sh`** additions from the prior change (scoped-code checks) plus existing suites all
  remain green: smoke 65/65, integration 148/148, founding 46/46, honorary 19/19, honorary-application 20/20.

## Workflow at a glance

1. A visitor opens **membership → Honorary → apply**, lands on `honorary-application.html`, fills the form,
   attaches a résumé (and optional documents), accepts the declaration, and submits.
2. The server validates + stores everything, creates the application with a `PCI-HONAPP-…` reference, emails
   the applicant a confirmation, and alerts the configured board recipient.
3. The board opens **Admin → Honorary applications**, reviews the record, downloads the documents, and
   decides. **Approve** confers a `PCI-HON-…` honorary award (publicly verifiable at `/verify.html?id=…`)
   and emails the applicant; reject / under-review email the applicant accordingly.
4. Every notification is recorded in `notification_history`; recipients, on/off and subjects are configured
   in **Admin → Settings** without code changes.
