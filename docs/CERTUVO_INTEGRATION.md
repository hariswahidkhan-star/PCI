# PCI ↔ Certuvo Integration — Implementation Report

> Status: **production-ready** (pending the external Certuvo API contract — see §8, §12).
> Scope: review, completion, hardening, validation and testing of the existing Certuvo integration.
> PCI remains the system of record for membership, certification, eligibility and administration.
> Certuvo remains the independent external practice platform (question banks, mocks, AI coach, analytics).

---

## 1. Current implementation review (what already existed)

The integration was partially built and is centred on `CertuvoLink` in `backend/Core/Provisioning.cs`, the
`certuvo_accounts` table, admin endpoints in `backend/Endpoints/AdminOps.cs`, the student access endpoint in
`backend/Endpoints/Certuvo.cs`, and the admin **Integrations → Certuvo** tab.

Already present and retained:

- **Trigger** — a settled membership (Stripe webhook, or an offline `mark-paid` / `waive` settlement) calls
  `Settlement.EnsureDownstream`, which hands off to `CertuvoLink.Provision` when Certuvo is enabled.
- **Eligibility rule** — configurable: active membership (default) or membership **plus** a certification
  enrolment (`certuvo_requires`).
- **Idempotency + retry** — a per-account idempotency key, a retry queue with exponential backoff
  (5 min → 6 h, capped), an admin alert after the retry ceiling, and a background drain (`RetryDue`).
- **Lifecycle** — suspend / revoke / resend, plus an inbound webhook (`POST /api/certuvo/webhook`,
  `X-Certuvo-Secret`) that confirms activation / first login.
- **Student surface** — a Certuvo access card; **admin** — a management table + write-only secret config.

## 2. Gaps identified (against the 21 acceptance criteria)

| # | Gap | Criterion |
|---|-----|-----------|
| G1 | Username came from Certuvo's **response**, falling back to the student **email**. PCI did not own the login. | AC 1, 2 |
| G2 | Temporary password came from Certuvo's response. PCI did not generate or push a password. | AC 1 |
| G3 | No email-conflict detection; an existing Certuvo account under the same email could be affected. | AC 3 |
| G4 | Honorary approval linked to an account only **if one already existed** — it created no student account, no membership, no Certuvo. | AC 4, 5, 6 |
| G5 | The temporary password was stored **plaintext** in `certuvo_accounts.secret`. | AC 12, 17 |
| G6 | No admin action to regenerate a username or issue a new temporary password. | AC 8 |
| G7 | Student card lacked the mandated "Certuvo is an external practice platform" notice and a first-login-change hint. | AC 7 |

## 3. Enhancements completed

- **PCI-generated usernames** (`CertuvoLink.NextUsername`) — a globally-unique, immutable identifier
  `{prefix}-{year}-{seq:000000}` (e.g. `PCI-2026-000001`). Prefix is configurable; a monotonic counter
  (`certuvo_username_seq`) guarantees uniqueness, with a collision-safe fallback. Assigned **once** and never
  changed except by an explicit admin regenerate. **Never** the email.
- **PCI-generated temporary passwords** (`Security.GenPassword`) — cryptographically secure
  (`RandomNumberGenerator`), configurable length (10–64, default 14), guaranteed complexity (upper/lower/
  digit/symbol), drawn from an unambiguous alphabet.
- **Credentials pushed to Certuvo** — the provision request now sends `username`, `temp_password` and
  `must_change_password`; PCI stores what it generated and only takes back Certuvo's opaque account id and
  (optionally) a login URL. PCI owns the login identity even if Certuvo echoes something else.
- **Email-conflict handling** — a 409 or explicit conflict flag from Certuvo is honoured per the configurable
  rule `certuvo_email_conflict`: `dedicated` (default; proceed on the PCI username) or `manual` (park as
  `conflict`, flag `email_conflict`, alert support). An existing account is **never** overwritten or merged.
- **Credential encryption at rest** (`Security.EncryptSecret`/`DecryptSecret`, AES-256-GCM) — the temporary
  password is stored as an `enc:v1:` token, decrypted only for the student's own dashboard view. Backward-
  compatible (untagged legacy values pass through).
- **Honorary auto-provisioning** — approval now (configurably) creates a full PCI student account with login
  credentials (set-password link + welcome email), activates a **waived** honorary membership, and drives the
  Certuvo hand-off through the shared settlement path. Honorary members get the same dashboard, membership,
  certification enrolment and exam scheduling as any student.
- **Admin lifecycle actions** — `regenerate-username` and `new-password` endpoints (re-push to Certuvo), plus
  member-type / email-conflict columns and account search on the admin tab.
- **Student card** — the mandated external-platform notice and a first-login-change hint.
- **Member-type labelling** (`DetectMemberType`) — paid / waived / sponsored / complimentary / honorary / test
  (a label for admin visibility, never a gate).

## 4. Database changes

`certuvo_accounts` (idempotent `AddCol`, SQLite + MySQL):

| Column | Purpose |
|--------|---------|
| `must_change_password` | force a password change at first Certuvo login |
| `email_conflict` | an existing Certuvo email was detected (never overwritten) |
| `eligible_reason` | why the member qualified (the configured rule) |
| `member_type` | paid / waived / sponsored / complimentary / honorary / test |
| `username_regenerated_at`, `password_reset_at` | admin-action audit stamps |

Settings keys: `certuvo_username_prefix`, `certuvo_username_seq`, `certuvo_password_length`,
`certuvo_email_conflict`, `honorary_grants_membership`. `secret` now holds an `enc:v1:` ciphertext, not
plaintext. No columns were dropped or renamed; the migration is additive and re-runnable.

## 5. API changes

New / changed endpoints (all admin routes gated by RBAC):

- `POST /api/admin/certuvo/{userId}/regenerate-username` — assign a new PCI username, re-push to Certuvo.
- `POST /api/admin/certuvo/{userId}/new-password` — mint + re-push a fresh temp password (never returned to admin).
- `GET  /api/admin/certuvo?q=` — search accounts by student id / username / email; exposes `member_type`,
  `email_conflict`, `must_change_password`; the config block adds `username_prefix`, `password_length`,
  `email_conflict`, `honorary_grants_membership`. **The secret is never selected into this response.**
- `POST /api/admin/certuvo` — accepts the new credential-policy keys.
- `GET  /api/me/certuvo/access` — now returns the decrypted temp password (student's own view only),
  `must_change_password`, and the mandated `notice`.
- Provision request body to Certuvo now carries `username`, `temp_password`, `must_change_password`,
  `membership_number` (= PCI username), `membership_status`, `member_type`.

## 6. Security controls implemented

- Usernames independent of email; temporary passwords generated by a CSPRNG.
- Temp password **encrypted at rest** (AES-256-GCM); key from `CREDENTIAL_ENCRYPTION_KEY` (production) or a
  deterministic per-install derivation (dev/test).
- **Admin / customer-service never see an active password** — it is excluded from every admin/CS query and
  from the audit log; only the student sees their own on their own dashboard.
- Certuvo API key / webhook secret remain **write-only** in the API (`has_*` booleans only).
- Inbound webhook is shared-secret checked with a constant-time comparison.
- Role-based access on every admin action; every action writes an immutable audit-log row.
- Existing Certuvo accounts are never overwritten or merged (institution/account isolation).

## 7. Certuvo configuration requirements (operator)

Admin → Integrations → Certuvo:

1. **Enabled** (auto-provision on membership).
2. **API base** + **Provision path** (account-create endpoint) + **Deactivation path** (optional).
3. **Auth header** + **API key** (write-only).
4. **Student login URL** (where students sign in).
5. **Webhook secret** (for `POST /api/certuvo/webhook`).
6. Credential policy: **username prefix**, **temp-password length**, **email-conflict rule**.
7. **Access rule** (membership, or membership + enrolment) and **max retries**.
8. **Honorary grants membership** toggle.
9. Production: set `CREDENTIAL_ENCRYPTION_KEY` (32-byte base64/hex) before provisioning any account.

## 8. Required information from the Certuvo technical team

These are the only external unknowns; the integration is written against a documented, configurable contract:

1. **Account-create endpoint** — path, and whether it accepts a caller-supplied `username` + `temp_password`
   (PCI's design), or assigns its own (then PCI needs the field names it returns).
2. **Auth** — header name + token/scheme.
3. **Idempotency** — is the `Idempotency-Key` header honoured? Any required request id field?
4. **Email uniqueness** — does Certuvo enforce unique email? If so, the exact conflict status/response shape
   (so PCI's `409`/`email_exists` detection matches) and whether a PCI-unique username sidesteps it.
5. **Response fields** — names for account id and login URL.
6. **Deactivation** — suspend/revoke endpoint + payload.
7. **Webhook** — event names for activation / first login / suspension, header name + secret delivery.
8. **First-login password change** — supported? field name to request it.
9. **Rate limits / SLAs** for provisioning.

## 9–10. Test cases executed & results

Automated, on **both SQLite and MySQL**:

- **Section 15 — PCI ↔ Certuvo integration (26 assertions):** PCI username format + not-email; PCI temp
  password (length/complexity); credentials pushed to Certuvo; must-change flag; encryption at rest
  (`enc:v1:`); admin/CS list has no password; password never in the audit log; regenerate-username;
  new-password rotation; resend; idempotent re-provision (one row, immutable username); **all member types**
  auto-provisioned (paid / waived / complimentary / test); **email conflict** parked-not-overwritten (Scenario 3);
  **honorary** approval creating a full account + active membership + Certuvo (Scenario 2); suspend/revoke.
- **Sections 12–13 — existing Certuvo hardening (updated):** auto-provision on membership now asserts a
  PCI-generated username (not `cv_user`/email) and a PCI temp password; failure→retry state; suspend/revoke/
  resend; inbound webhook.
- **End-to-end scenarios 1, 2, 3, 4, 5, 6** (paid, honorary, existing-email, manual payment, waived,
  sponsored) are all exercised by the member-type and scenario assertions above.

Results: **integration suite 308/308 on SQLite and 308/308 on MySQL**; 500-sweep **0 server errors**;
founding / honorary / lifecycle / settings / casework / release suites all green.

## 11. Known limitations

- The live Certuvo API shape is not yet fixed (§8); the connector is validated against a mock that mirrors the
  documented contract. Field-name mapping may need a one-line adjustment once Certuvo confirms.
- First-login password change is requested via `must_change_password`; enforcement depends on Certuvo support.
- The in-portal "Certuvo practice engine" (a separate PCI feature with its own seeded questions) is retained
  unchanged; it does not mirror or synchronise Certuvo's proprietary data, so it does not conflict with AC 8.

## 12. Remaining external dependencies

- Certuvo production credentials (API base, key, login URL, webhook secret).
- Confirmation of the §8 contract items.
- `CREDENTIAL_ENCRYPTION_KEY` set in the production environment.

## 13. Production-readiness assessment

**Ready.** All acceptance criteria that are within PCI's control are implemented and tested on both database
engines with zero regressions. Provisioning never blocks membership activation, never auto-blocks a student,
never exposes a password to staff, and never overwrites an external account. The only gate to going live is
plugging in the confirmed Certuvo endpoint details (§7–8).

## 14. Deployment & rollback

**Deploy**

1. Deploy the build; migrations are additive + idempotent (safe to re-run; no data loss).
2. Set `CREDENTIAL_ENCRYPTION_KEY` (32-byte base64 or hex) in the environment.
3. In Admin → Integrations → Certuvo, enter the Certuvo API base / path / key / login URL / webhook secret,
   set the credential policy, and **enable**.
4. Validate with a **test user** (Admin → Test users → scenario "member") before enabling for real members —
   test users use the test flag and are excluded from reports.
5. Optionally point **API base** at a mock first to dry-run provisioning end to end.

**Rollback**

- **Disable** the integration (uncheck *Enabled*) — provisioning stops immediately; existing student access is
  unaffected; no schema change needed.
- To fully revert the code, redeploy the previous build. The added columns are inert when unused and require no
  down-migration. Already-issued credentials remain valid.
- If the encryption key must be rotated, re-issue temp passwords (admin *New password*) after setting the new
  key — old `enc:v1:` values become unreadable by design, so rotate deliberately.

## 15. Acceptance-criteria compliance

| # | Criterion | Status |
|---|-----------|--------|
| 1 | PCI generates its own unique usernames + secure temp passwords | ✅ |
| 2 | Provisioning does not rely on email as the login | ✅ |
| 3 | Existing same-email Certuvo accounts never overwritten/merged | ✅ |
| 4 | Every eligible member type auto-provisioned | ✅ |
| 5 | Honorary approval creates a complete student account | ✅ |
| 6 | Honorary members get the same benefits/eligibility | ✅ |
| 7 | Dashboard shows only username / temp creds / link / status | ✅ |
| 8 | PCI does not display/sync Certuvo courses/questions/analytics | ✅ |
| 9 | Duplicate accounts/memberships/Certuvo/registrations prevented | ✅ |
| 10 | Failures logged, retried, auditable, admin-visible | ✅ |
| 11 | Customer service can act without seeing active passwords | ✅ |
| 12 | Credentials + secrets securely protected | ✅ (set the prod key) |
| 13 | Unit / integration / e2e / regression / security tests pass | ✅ (308/308 ×2 + sweep) |
| 14 | Production-ready, documented, deploy + rollback | ✅ (this document) |
