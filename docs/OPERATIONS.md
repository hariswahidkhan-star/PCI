# PCI Platform — Operations & Activation Runbook

The single go-live checklist for the Project Controls Institute platform. It consolidates every
subsystem shipped through master-plan Phases 0–10 and the one-time steps an operator performs to
activate each. Nothing here requires code changes — these are configuration and credential steps.

Companion docs: `DEPLOY.md` (deploy mechanics), `docs/MYSQL_MIGRATION.md` (database cutover),
`docs/README-SECUREEXAM.md` (desktop exam client), `docs/PROJECT_STATUS.md` (what was built).

---

## 0. One-time deploy

The whole platform (public website + student portal `/app` + admin console `/admin`) is one Docker
web service. Deploy via the Render Blueprint (`render.yaml`) or any Docker host.

After **any** merge to `main`, trigger a **Manual Deploy** so the running service picks up the new
build — the app serves the compiled SPAs and static site from the image.

Required environment variables (set the `sync:false` ones in the host dashboard — never commit them):

| Variable | Purpose |
|---|---|
| `DB_PROVIDER=mysql` | **Required in production.** The app refuses to boot in Production without it — there is no SQLite fallback. |
| `MYSQL_HOST/PORT/DATABASE/USER/PASSWORD` (or `MYSQL_CONNECTION_STRING`) | External managed MySQL 8.x / MariaDB 10.11+ (InnoDB, utf8mb4, UTC). Render has no managed MySQL — use PlanetScale / Aiven / RDS / DigitalOcean. |
| `MYSQL_SSL=required` | TLS to the database. |
| `ADMIN_OWNER_EMAIL / ADMIN_OWNER_PASSWORD / ADMIN_OWNER_NAME` | The bootstrap owner account. **Set these before first boot** so the well-known default (`owner@pci.local` / `changeme-owner`) never exists in production. |
| `ALLOWED_ORIGIN` | The single approved browser origin (production rejects `*`). |
| `STRIPE_SECRET_KEY`, `STRIPE_WEBHOOK_SECRET` | Payments (see §9). |
| `RESEND_API_KEY` **or** `SMTP_HOST/PORT/USER/PASSWORD` | Email delivery (see §10). |
| `GOOGLE_CLIENT_ID` | Optional — enables student Google sign-in. |

The persistent disk (mounted at `/data`, auto-detected) holds **only uploaded files** (exam evidence,
ID documents, attachments); the database is MySQL.

> **First boot:** the bootstrap owner has a forced-password-change flag. Sign in at `/admin/`, set a
> new password (every `/api/admin/*` call except auth returns 403 until you do), then **deactivate or
> rotate the seeded demo student** `student@pci.local`.

---

## 1. Database cutover (Phase 1)

Production is MySQL-only. To migrate an existing SQLite database:

1. Copy the live `pci.db`.
2. Run `backend/tools/migrate_sqlite_to_mysql.py` (preserves all ids, password hashes, relationships;
   reconciles row counts + financial sums + FK orphans; writes a JSON report; exit 0 = clean).
3. Point `MYSQL_*` at the target and deploy with `DB_PROVIDER=mysql`.
4. Schedule `backend/tools/mysql_backup.sh` (mysqldump `--single-transaction` + retention).

Full runbook: **`docs/MYSQL_MIGRATION.md`**.

---

## 2. Domains, redirects & indexing (Phase 2)

- Canonical public host: **`https://projectcontrolsinstitute.org`** (non-www). Set `CANONICAL_HOST`
  if different.
- `www.*` and `pciglobal.ai` **301 page-to-page** to the canonical host automatically; `http→https`
  is enforced; unknown hosts pass through (so the Render URL keeps working during DNS transition).
- Portal/admin live under the portal domain (`mypci.org`) and are `noindex` + `X-Robots-Tag: noindex`.
- `robots.txt`, `sitemap.xml` and `llms.txt` are generated **dynamically** from published, indexable
  pages — no manual maintenance.
- **DNS:** point the apex + `www` + `pciglobal.ai` at the host; the app does the rest.

---

## 3. SEO — Search engines (Phases 3–4)

**Admin → SEO → Search engines.** All optional; nothing is emitted until you set an ID.

| Field | Where to get it | Effect |
|---|---|---|
| GA4 measurement ID (`G-…`) | Google Analytics | Loads gtag on every public page — **after consent** (§4). |
| GTM container ID (`GTM-…`) | Google Tag Manager | Same, consent-gated. |
| Microsoft Clarity project id | Clarity | Session insight, consent-gated. |
| `google-site-verification` token | Search Console → HTML tag | Injected as a meta on every page; then verify the property. |
| `msvalidate.01` (Bing) token | Bing Webmaster Tools | Same for Bing. |
| PageSpeed API key | Google Cloud | Optional — raises the PSI quota. PageSpeed audit runs keyless at low volume. |

**IndexNow** needs no account — the site key is generated automatically and served at
`/{key}.txt`. Use **Submit all public URLs** once after go-live; every content edit can be resubmitted.

Managed 301 redirects and a site audit (missing H1/canonical/JSON-LD/alt, broken links, duplicate
metadata) live under the other SEO tabs.

---

## 4. Analytics, consent & attribution (Phase 5)

- First-party analytics is **cookieless and server-side** — visitors are daily-rotating salted hashes
  (no raw IPs stored), bots filtered, country only from a CDN geo header. It works with zero config;
  see **Admin → Analytics**.
- **Google Consent Mode v2**: GA4/GTM/Clarity default to *denied* and load only after the visitor
  accepts the built-in banner. Sensitive pages (checkout, login, reset, payment, applications) never
  receive analytics tags.
- Attribution is first-touch (`pci_attr` cookie), copied onto conversions (registration, login,
  purchase, membership) so revenue is attributable to campaigns.
- **Activation:** nothing required — data accrues on real visits. Add a GA4 ID (§3) for client-side
  event granularity.

---

## 5. AI Visibility (Phase 6)

- **`/llms.txt`** (llmstxt.org) is generated live from indexable content — a curated map for AI answer
  engines. **`/robots.txt`** is policy-aware.
- **Admin → AI Visibility**: readiness (llms/robots/sitemap, structured-data coverage), observed
  AI-crawler traffic (GPTBot, ClaudeBot, PerplexityBot, …), and an **Access** grid to allow/block each
  crawler. Answer engines are allowed by default so PCI can be cited.
- **Activation:** none — works out of the box; crawler traffic appears once the site is live.

---

## 6. Multilingual (Phase 2 pack + admin control)

- A starter translation pack (public site in ko/ar/es/fr/zh/ru) is **seeded on boot**, so the language
  switcher and translated homepage work with no API key.
- **Admin → Translations** (owner-only): set a provider (Anthropic/OpenAI/custom) + key to
  auto-translate the long tail, or edit any string per page.

---

## 7. Training Partners (Phase 7)

- Public pages: `/training-partners.html` (directory) and `/become-a-training-partner.html` (application).
- **Admin → Training Partners**: review applications → **Approve** (creates an unlisted directory entry
  at a chosen tier) → **Publish** to show it on the public directory.
- Certification stays independent of training — partners deliver exam preparation only.
- **Activation:** none — applications flow in and the review UI is live.

---

## 8. Certuvo — study & practice (Phase 8)

- **Student portal → Certuvo**: scenario-based practice + full mock exams with instant feedback and
  explanations. A **40-question starter pack** across all 8 BoK domains is seeded on boot, so it works
  immediately. Practice is formative; the credential is still earned on the real examination.
- Add or edit practice questions under **Admin → (question bank)** with the new *explanation* and
  *difficulty* fields (`is_practice = 1`). The secure exam item bank (`is_practice = 0`) is separate.
- **Activation:** none.

---

## 9. Payments (Stripe)

- Set `STRIPE_SECRET_KEY` and `STRIPE_WEBHOOK_SECRET`. Point a Stripe webhook at
  `POST /api/webhook` (not `/api/payments/webhook` — that route does not exist). Inbound events are
  **HMAC-verified** and processed exactly once (idempotent by event id). Until configured, payment
  endpoints answer 503. Optionally set `STRIPE_WEBHOOK_URL` to the exact URL registered in Stripe so
  the production config preflight and `/api/admin/system-check` can verify it ends with `/api/webhook`.

---

## 10. Email

- One provider activates delivery: `RESEND_API_KEY` **or** `SMTP_HOST/PORT/USER/PASSWORD`. Without
  either, emails print to the console (safe for staging). Notification recipients and per-event
  on/off switches live in `site_settings` — never hardcoded.

---

## 11. Integrations & ERP (Phases 9–10)

**Admin → Integrations & ERP.** Push business events (`payment.recorded`, `membership.activated`,
`member.registered`) to external systems. Events are captured in a durable outbox and delivered by a
background worker with retry/backoff; every attempt is logged in the delivery ledger.

### Generic webhook connector
1. Add a connector → **Generic webhook**.
2. Endpoint URL (HTTPS) + a **signing secret** (write-only).
3. Deliveries are signed **HMAC-SHA256** in `X-PCI-Signature` (`sha256=…` over `{timestamp}.{body}`),
   with `X-PCI-Timestamp`. Your receiver recomputes it with the shared secret to verify authenticity.
4. **Test** sends a signed `ping` immediately. Point it at your own endpoint or an automation bridge
   (Zapier / Make / n8n) to fan out to any ERP.

### QuickBooks Online connector
Maps `member.registered → Customer` and `payment.recorded → SalesReceipt` via the QuickBooks API.

1. Create an app in the **Intuit developer portal**; note the **client id / client secret**.
2. Run the OAuth2 authorization once to obtain a **refresh token** for your company (realm).
3. Add a connector → **QuickBooks Online**: environment (production/sandbox), **company (realm) id**,
   client id, client secret, refresh token, and the **sales item ref** (a QBO Item id for the line).
4. All OAuth secrets are **write-only**. PCI mints short-lived access tokens from the refresh token at
   delivery time; you may instead paste a short-lived access token directly for testing.
5. **Test / a real event** delivers a Customer or Sales Receipt; failures (e.g. not-yet-authorized)
   are recorded honestly in the ledger and retried — never faked.

> **Activation pending until** you supply the Intuit app credentials and complete the one-time OAuth
> grant. The connector, mapping and delivery are complete and tested against a stand-in.

---

## 12. Security posture & trust boundaries

- **Secrets are write-only.** Connector signing secrets, QuickBooks OAuth secrets and the PageSpeed
  key are never returned by any API (only a `has_secret` / `secret_fields` flag). Store real secrets in
  host environment variables or the write-only admin fields — never in the repo.
- **RBAC.** Every admin surface is permission-gated (`pages`, `reports`, `partners`, `integrations`,
  …); the owner holds all. Team & Access manages per-admin grants.
- **Outbound connectors are an admin trust boundary.** The webhook/QuickBooks/PageSpeed features make
  the server call an admin-supplied URL by design. They are reachable **only** by an admin holding the
  relevant permission — treat the `integrations` and `pages` permissions as network-egress-capable and
  grant them accordingly.
- **Webhook signatures.** Inbound Stripe events are HMAC-verified; outbound integration deliveries are
  HMAC-signed.
- **Audit logging.** Admin mutations are written to the audit log.
- **Private surfaces** (`/admin`, `/app`, `/api`) carry `X-Robots-Tag: noindex` and are excluded from
  `robots.txt`, `sitemap.xml` and `llms.txt`.
- **No hardcoded credentials.** The only seeded default is the bootstrap owner, overridable by env and
  gated by a forced password change.

---

## 13. Post-deploy smoke checklist

- [ ] `/admin/` loads; owner signs in; forced password change done; demo student deactivated.
- [ ] Public site loads on the canonical host; `www`/`pciglobal.ai` 301 to it.
- [ ] `/robots.txt`, `/sitemap.xml`, `/llms.txt` all respond.
- [ ] Language switcher appears; a non-English homepage renders.
- [ ] A test enrolment → Stripe webhook settles once → receipt emailed.
- [ ] Student portal → Certuvo: start a quiz, submit, see explanations.
- [ ] Admin → SEO: paste GA4 + verification tokens; IndexNow submit.
- [ ] Admin → Integrations: add a webhook, **Test** → delivered + valid signature at your receiver.
- [ ] (When ready) QuickBooks connector authorized and a real event posts a Customer/SalesReceipt.
