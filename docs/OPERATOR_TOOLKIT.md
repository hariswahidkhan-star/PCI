# Operator Toolkit — Waivers, Test Users, Student Journey & Certuvo

Four operator capabilities that let staff unblock, test, diagnose, and provision students without
touching the database. Everything lives in the Admin Console under **Students** and **Integrations**.

| Capability | Where | Permission |
|---|---|---|
| Mark paid / waive / grant free | Students → *student drawer* → **Mark as paid / waive fee** | `members` |
| One-click test users | Students → **+ Test user** (list header) | `members` |
| Student journey (where they're stuck) | Students → *student drawer* → **Student journey** | `members` |
| Certuvo practice-platform config | Integrations → **Certuvo** tab | `integrations` |
| Re-provision a Certuvo account | Students → *student drawer* / Integrations → Certuvo | `members` |

---

## 1. Mark paid / waive / grant free

Sometimes a student pays out-of-band (bank transfer, corporate PO) and the payment never reflects, or
you want to comp a seat entirely. The **Mark as paid / waive fee** card records an offline settlement
that is identical to a card payment — it writes a `paid` payment row and the same entitlement, so the
student can schedule immediately.

- **Product** — `Exam`, `Membership`, or `Bundle` (exam + membership).
- **Amount (USD)** — the amount actually received. **`0` grants it free** (scholarship / comp / corporate
  seat); the button label switches to **Grant free**.
- **Note** — free-text reference (e.g. "bank transfer 12 Jul", "corporate PO #4471"); stored on the
  payment for the audit trail.

Free grants use provider `admin_waiver`; non-zero offline settlements use `admin_manual`. Exam grants set
the one-year `exam_schedule_deadline` exactly as a real purchase does. All the usual eligibility,
government-ID, and one-attempt rules still apply — a waiver removes the *fee* blocker, nothing else. To
undo, mark the settlement payment refunded.

## 2. One-click test users

**+ Test user** creates a fully-unlocked account so staff can exercise every gated feature — scheduling,
delivery, credentials — without paying. The account is:

- flagged `is_test = 1` (so it can be filtered and bulk-removed),
- pre-seeded with an accepted consent set, a completed student profile, and an **approved** government-ID
  record,
- granted a free **bundle** (membership + exam access) via the same settlement path as a waiver,
- returned with its **email, generated password, and a ready session token**.

Because the session token is minted server-side, the **"Open portal as this user ↗"** link
(`/app/#t=<token>`) drops you straight into that student's portal — no login, so it is never blocked by the
login rate-limiter. Test users are listed under **Students** and can be deleted from the drawer; deletion
removes every dependent row (payments, bookings, attempts, consents, Certuvo account, sessions, …) before
the user, leaf-first, so no foreign-key remnants are left behind.

## 3. Student journey — where is the student stuck?

The **Student journey** card renders the student's real progress as an ordered set of stages, each with a
status:

| Status | Meaning |
|---|---|
| **DONE** | completed |
| **ACTION** | needs the student (or you) to act — this is a *soft* block |
| **BLOCKED** | a hard prerequisite is missing; scheduling is impossible until it clears |
| **PENDING** | not reached yet |

Stages: **Account → Consents → Profile → Government ID → Exam fee → Exam scheduled → Exam taken →
Credential**, plus **Certuvo practice access** when the Certuvo integration is enabled. A banner at the top
calls out the **first** blocked/action stage — e.g. *"Stuck at: Exam fee — Exam fee unpaid; use Mark paid
/ Waive to grant access"* — so you can see the problem and fix it from the same drawer. The card also
surfaces the delivery mode (in-house SecureExam vs. an external vendor) and membership state. Stuck logic
reuses the same `BookingBlockers` the student portal enforces, so the journey never disagrees with what the
student actually sees.

## 4. Certuvo — external practice platform

Certuvo is a **separate** practice platform. When a student's **membership** is settled, PCI calls Certuvo's
API to provision them an account and then surfaces the credentials in the **student panel** (Certuvo page →
*Your Certuvo practice access*). Nothing is shared before payment; the panel shows an "after membership"
placeholder until provisioning completes.

Configure it in **Integrations → Certuvo**:

| Field | Default | Notes |
|---|---|---|
| Enabled | off | master switch for auto-provision on membership |
| API base | — | e.g. `https://api.certuvo.com` |
| Provision path | `/api/accounts` | appended to API base for the create-account call |
| Student login URL | — | where the student signs in to Certuvo |
| Auth header | `Authorization` | header carrying the API key |
| API key | — | **write-only**; stored server-side, never returned to the browser |

**Provisioning flow.** On membership settlement PCI `POST`s `{external_ref, email, first_name, last_name}`
to `{api_base}{provision_path}` with the API key on the configured header. It parses the returned
`username` / `password` (or `secret`) / `id` / `login_url`, stores them against the user
(`certuvo_accounts`), marks the account `active`, and notifies the student. On failure the account is left
`pending`/`error` with the error recorded, and can be retried with **Re-provision** from either the student
drawer or the Integrations → Certuvo accounts table. Point **API base** at a mock endpoint first to
validate the round-trip before going live.

---

### Verification

The whole toolkit is covered by the integration suite (section 12): journey stuck-detection, mark-paid
(free + membership) unblocking scheduling, test-user create / session / list / delete, Certuvo
auto-provision on membership and credential delivery to the student, and RBAC on every endpoint. All admin
endpoints are gated (`members` / `integrations`) and refuse view-only roles with `403`.

---

# v2 — Finance controls, impersonation, scenarios, Certuvo hardening, institution limits

The toolkit above was extended into a full operator/finance layer. Everything below is enforced
server-side, audit-logged, and covered by integration suite section 13 (SQLite **and** MySQL).

## Finance (permission: `finance` — explicit, never bundled into a named role)

- **Waive fee** (Students → drawer → *Waive fee*): full waiver settles immediately as
  `payment_status='waived'` (never a fabricated paid transaction; excluded from revenue); partial
  waiver issues a single-use percentage code **locked to that student's email**, so the balance flows
  through the normal checkout. Reason is mandatory; every waiver lands in the `fee_waivers` ledger
  (original / waived / payable, approver, expiry).
- **Mark as paid** now carries evidence — method, bank reference, gateway reference, receipt number,
  paid-on date, recorder — and refuses duplicate gateway references (409) and already-live grants
  unless explicitly overridden (`allow_duplicate`). List-price mismatches are flagged, not blocked.
- **Reverse** (Payments → Reconciliation): admin-recorded settlements can be reversed with a mandatory
  reason — the payment becomes `refunded`, the unconsumed entitlement is revoked, scheduled bookings
  cancel, and the membership lapses only if no other settlement supports it. Stripe money is refunded
  at the gateway (webhook applies it here).
- **Reconciliation** (Payments → Reconciliation): every payment with its downstream state
  (entitlement / membership / Certuvo) and an exception reason; **Reprocess** idempotently re-applies
  missing downstream effects — safe to click any number of times, never double-grants.

## Impersonation — "View as student" (permission: `impersonate`)

Students → drawer → *View as student*: a reason is required, a 60-minute session opens the portal
exactly as the student sees it, under a permanent amber banner. Consent acceptance and identity
uploads are refused in support view. Start and end are audit-logged; *End session* revokes it.

## Test users v2 (permission: `test_users`)

Scenario presets: `ready`, `unpaid`, `member`, `waived`, `incomplete_profile`, `no_id`,
`certuvo_failed` — plus **reset** to re-run any scenario on the same account. Test accounts wear a
TEST badge everywhere, never reach revenue reports, and their credentials are invisible to the
public verification register.

## Certuvo integration v2

- Idempotency keys on every provisioning call; repeated settlements/webhooks can never double-create.
- Automatic retries with exponential backoff (5 min → 6 h cap, configurable maximum) run on the
  background dispatcher; support is alerted when retries are exhausted. Membership activation is
  never blocked by a Certuvo failure, and the student sees a plain-language "still setting up"
  message — never an API error.
- Admin actions per account: re-provision (with `reactivate` for suspended/revoked), suspend, revoke
  (best-effort remote deactivation via the configurable endpoint), resend instructions.
- Inbound webhook `POST /api/certuvo/webhook` (header `X-Certuvo-Secret`) records activation /
  first-login so the tracker shows "first login confirmed".
- Configurable business rule: access on active membership alone, or membership + certification
  enrolment (`Admin → Integrations → Certuvo → Access rule`).

## Institution sponsorship (Training partners)

Per-partner admin-defined ceilings — max discount %, max codes, max uses per code, total allocation,
and whether 100% sponsorship is allowed — enforced when partner-linked codes are created **and** at
redemption (a spent allocation stops honouring codes). The *Codes & usage* view shows codes,
redemptions, remaining allocation and sponsored registrations; an alert fires at 80% consumption.
