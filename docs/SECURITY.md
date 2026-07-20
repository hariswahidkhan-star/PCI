# PCI Platform — Security Overview & Controls

> Scope: the PCI web platform (public website, student portal, admin console, institution
> portal) and its backend (ASP.NET Core 8, MySQL in production). This document is the technical
> security reference; the standards mapping lives in [`ISO_COMPLIANCE.md`](./ISO_COMPLIANCE.md).
>
> **No system is unhackable.** The goal here is defence-in-depth: multiple independent controls so
> that a single failure does not expose student data, and a clear split between what the code
> enforces and what the operator must configure.

## 1. Identity, authentication & sessions

| Control | Implementation |
| --- | --- |
| Password storage | bcrypt (cost 11, per-user salt). Plaintext never stored or logged. `Core/Security.cs` |
| Session tokens | 24–32-byte CSPRNG, stored **only as SHA-256**. Admin/partner 12 h, student 30 d. Revoked on logout, password change, and reset. `Core/Auth.cs`, `Program.cs` |
| Brute-force — per IP | Fixed-window 10/min on every auth + sensitive public path (`_rlPaths`, `Program.cs`) keyed on the trusted last-hop IP (forgery-resistant). |
| Brute-force — per account | **NEW.** After 10 consecutive wrong passwords an account is locked for 15 min (`LoginGuard`, `Core/Auth.cs`) on student, admin and partner logins — defeats slow/distributed guessing that rotates IP. |
| Username enumeration | **NEW.** Unknown accounts still run a throwaway bcrypt verify (`LoginGuard.BurnTime`) so response timing does not reveal which emails exist. Forgot-password always answers `ok`. |
| Admin MFA (TOTP) | Optional per admin; **replay-protected** — the matched timestep is recorded and must strictly advance, so a captured code cannot be reused inside its window. `Program.cs`, `Security.VerifyTotpStep` |
| Recovery | Email reset link (single-use, hashed, 1 h) + operator break-glass `ADMIN_RECOVERY_CODE` / `ADMIN_OWNER_RESET_PASSWORD` env. Constant-time compared. |
| Failed-login visibility | **NEW.** Failed student logins recorded in `login_events`; failed admin logins audited (`admin_login_failed`). |

## 2. Authorization, IDOR & tenant isolation

- **Deny-by-default RBAC** on the admin console (`GateFn`): every section requires the matching
  permission; governance surfaces (team, settings, honorary, translations) are owner-only.
- **Student data is scoped to the session user** on every `/api/me/*` endpoint — documents,
  payments, exam attempts, certificates, tickets, IDV. Changing an id in the URL cannot reach
  another student's record.
- **Institution portal** isolates every read to the caller's `partner_id`; student PII is masked,
  names gated behind per-institution privacy settings.
- **Impersonation ("view as student")** is read-only: state-changing exam actions (book, start,
  submit, heartbeat) and profile edits refuse an impersonation session (`impersonation_readonly`),
  so support staff cannot alter a candidate's record or sit their exam.

## 3. Data protection at rest

| Data | Protection |
| --- | --- |
| Government-ID scans, passport photos, identity/evidence uploads | **NEW — AES-256-GCM envelope encryption** before bytes touch disk or object storage (`Security.EncryptBytes` in `Core/Storage.cs`). Content-addressing/dedup preserved; a versioned magic header lets pre-existing plaintext read back with no migration. S3 objects additionally get SSE-AES256. |
| Displayable credentials (Certuvo temp passwords) | AES-256-GCM (`EncryptSecret`). |
| Passwords | bcrypt (one-way). |
| Encryption key | `CREDENTIAL_ENCRYPTION_KEY` (32-byte). **NEW — production refuses to boot without a dedicated key** (no predictable derived fallback). |
| Honorary IDV data minimisation | Only the document image + attestation **booleans** are stored — never an ID number or criminal-history free text. Owner-only access, fully audited, retention-bounded. |

## 4. Injection, uploads & SSRF

- **SQL injection:** every query is parameterised (`Data/Db.cs`); dynamic identifiers come from
  code constants / server-side whitelists, never request text.
- **XSS:** admin/user-authored rich text is sanitised server-side (tag + attribute allow-list,
  `javascript:`/`data:` schemes stripped, `<script>/<svg>/<iframe>` dropped).
- **Path traversal:** uploads are content-addressed; `..`, absolute and back-slash paths rejected.
- **File uploads:** MIME allow-list (no SVG/HTML), magic-byte sniff, 3 MB cap, executable rejection.
- **SSRF:** all admin-configurable outbound URLs (webhooks, QuickBooks, **exam-delivery vendors**,
  **translator endpoint**) go through `Core/Egress` — DNS is resolved in the connect callback and
  only public IPs are dialled (loopback/RFC1918/link-local + cloud-metadata `169.254.169.254`
  blocked), redirects disabled. Save-time validation gives fast operator feedback.
  `INTEGRATIONS_ALLOW_PRIVATE_EGRESS=true` is the documented opt-out for self-hosted private bridges.

## 5. Transport, headers & abuse

- Canonical HTTPS redirect; **HSTS** (1 year, includeSubDomains) when the proxy reports https.
- Security headers on every response: `X-Content-Type-Options`, `X-Frame-Options: DENY`,
  `Referrer-Policy`, `Cross-Origin-Opener-Policy`, a **Content-Security-Policy** with
  `default-src 'self'`, `object-src 'none'`, `frame-ancestors 'none'`, and **NEW** a
  `Permissions-Policy` denying camera/mic/geolocation/USB and restricting payment to same-origin.
- **CORS** locked to `ALLOWED_ORIGIN` (production refuses wildcard); no `Allow-Credentials`.
- Auth is **Bearer-token**, not cookies → no CSRF surface on state-changing endpoints.
- Request body capped at 6 MB; public forms (`/api/inquiry`, `/newsletter`, `/form-submit`) are
  **NEW** rate-limited against spam/email-bomb.
- **Webhooks:** Stripe signature verified, fails closed, replay-safe ledger; integration webhooks
  HMAC-SHA256 signed; exam-delivery callbacks constant-time token compared.

## 6. Production boot preflight (fail-closed)

`Program.cs` refuses to start in Production (unless `ALLOW_INSECURE_PRODUCTION=true`) when any of:
MySQL not selected / incomplete; non-HTTPS `APP_BASE_URL`; wildcard CORS; legacy admin token on;
Stripe enabled without webhook secret; **NEW — no dedicated `CREDENTIAL_ENCRYPTION_KEY`**.

## 7. Operator responsibilities (not enforceable by code)

These must be set in the deployment environment (Render → Environment):

1. **`CREDENTIAL_ENCRYPTION_KEY`** — 32-byte base64/hex/passphrase. Without it prod won't boot.
   Store in a secrets manager; rotating it requires re-encrypting existing artefacts.
2. **`MYSQL_SSL=required`** — encrypt DB traffic to the managed MySQL (already default in `render.yaml`).
3. **Enrol admin TOTP 2FA** for the owner and every privileged admin.
4. **`ALLOWED_ORIGIN` / `APP_BASE_URL`** — the real public HTTPS origin.
5. Rotate `ADMIN_OWNER_PASSWORD` at first login; keep `ADMIN_RECOVERY_CODE` in a vault.
6. Enable managed-database automated backups + encryption; restrict bucket/disk access.
7. Keep dependencies patched; review the audit log and failed-login events periodically.

## 8. Known limitations / roadmap (tracked, not yet implemented)

- CSP still allows `'unsafe-inline'` for scripts (the static site relies on inline handlers); the
  server-side sanitizer is the primary XSS control. Moving to nonce-based CSP is planned.
- Integration/exam-delivery **secrets** (webhook signing, OAuth tokens) are redacted on read but
  stored without field-level encryption; envelope-encrypting them is planned (DB-level encryption
  covers them in the interim).
- Voluntary password change does not yet require the current password (forced first-change flow
  keeps the API simple); planned once the portal UI adds the field.
- Enforced 2FA for the owner role and printable 2FA backup codes are planned enhancements.

_Last reviewed: 2026-07 (five-dimension internal security audit + hardening). This is internal
documentation, not a certification or a warranty; see `ISO_COMPLIANCE.md` for standards scope._
