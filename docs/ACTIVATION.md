# PCI Operator Activation Checklist — Communications & Marketing

Everything in the Communications Centre and the Marketing, Ads & Search Console
centre is **built and honestly gated**: the code records what it *would* send/call
and reports a clear "not configured / not connected" state until you complete the
steps below. Nothing goes live by accident.

## Golden rules

- **Secrets live only in Render environment variables.** Never put a provider
  secret, API key, token, or app secret in MySQL, in React, in the Admin Portal,
  in screenshots, or in logs. The Admin Portal only ever shows *configured / not
  configured* booleans.
- OAuth access/refresh tokens are stored **encrypted** (AES-GCM) and never leave
  the backend. They are encrypted with `CREDENTIAL_ENCRYPTION_KEY` — set a
  dedicated one **before** connecting any account.
- After changing env vars in Render, **redeploy** so the app process picks them up.

---

## 0. If the Render deploy "fails" / the service won't start

The app runs a **production preflight** and **deliberately refuses to boot** (exit
non-zero → Render health check fails → "deploy failed") if any of these are
missing or wrong. This is the #1 reason a deploy shows as failed even though the
build succeeded and the code is fine. Set **all** of these in Render, then
redeploy:

| Variable | Hard requirement in Production |
|---|---|
| `APP_BASE_URL` | Public HTTPS URL (not localhost/127.0.0.1). |
| `ALLOWED_ORIGIN` | An explicit origin — **not** `*`. |
| `DB_PROVIDER` | `mysql` (SQLite is refused in production). |
| `MYSQL_HOST` + `MYSQL_PASSWORD` | (or a full `MYSQL_CONNECTION_STRING`) — MySQL must be reachable. |
| `CREDENTIAL_ENCRYPTION_KEY` | A dedicated **32-byte** key (base64/hex/passphrase). |
| `STRIPE_WEBHOOK_SECRET` | Required **only if** `STRIPE_SECRET_KEY` is set. |
| `ENABLE_LEGACY_ADMIN_TOKEN` | Must **not** be `true`. |

The boot log names the exact offender(s) as `[config:error] <VAR> — <reason>` and
ends with `Refusing to start: N production configuration error(s)`. Open Render →
the failed deploy → **Logs** and look for those lines.

> Verified on a real MariaDB 10.11: with these set, the full migration + seed +
> health check pass cleanly (200) — every `mkt_*`/comms table and query included.
> The schema and code are MySQL-ready; the only thing gating a green deploy is
> this configuration.

`STRIPE_SECRET_KEY`, `SMTP_HOST`/`RESEND_API_KEY`, and `ADMIN_OWNER_PASSWORD`
produce **warnings** only — they don't block boot (payments/email degrade until
set), so they are not the cause of a failed deploy.

---

## 1. Environment variables (exact names)

Set these in **Render → your service → Environment**. "Fallback" means the app
still works without it but uses a weaker/derived default — set the dedicated value
in production.

### Core / crypto / base URL

| Variable | Used by | Notes |
|---|---|---|
| `CREDENTIAL_ENCRYPTION_KEY` | Token encryption (Marketing OAuth), unsubscribe & OAuth-state signing fallback | **32-byte** key (base64 or hex). Set this first; rotating it invalidates stored OAuth tokens (reconnect accounts). |
| `APP_BASE_URL` | Email links, OAuth **redirect URI**, webhook URLs | Public HTTPS origin, e.g. `https://app.projectcontrolsinstitute.org`. `SITE_BASE_URL` is an accepted alias. |

### Email (choose ONE delivery path)

| Variable | Used by | Notes |
|---|---|---|
| `RESEND_API_KEY` | Email delivery (preferred) | Resend HTTPS API. Simplest production path. |
| `MAIL_FROM` | Email "From" | Must be a **verified domain** on the Resend account (or their onboarding sender for tests). `SMTP_FROM` is the fallback. |
| `SMTP_HOST` | Email delivery (alternative to Resend) | Classic SMTP. Only used when `RESEND_API_KEY` is absent. |
| `SMTP_PORT` | SMTP | Default `587`. |
| `SMTP_USER` / `SMTP_PASS` | SMTP auth | |
| `SMTP_FROM` | SMTP "From" | |
| `SMTP_SSL` | SMTP TLS | Set to `false` only to disable STARTTLS (default on). |

> With neither `RESEND_API_KEY` nor `SMTP_HOST`, email is written to the console
> and recorded in the outbox as a `console` send — nothing is lost, nothing is
> delivered.

### WhatsApp (Meta Cloud API — optional)

| Variable | Used by | Notes |
|---|---|---|
| `WHATSAPP_ACCESS_TOKEN` | WhatsApp send | Default env-var **name** for the account token. Each WhatsApp account row in the Admin Portal stores the env-var *name* (`token_env`) — never the token itself — and its `phone_number_id` (not a secret). |
| `WHATSAPP_VERIFY_TOKEN` | WhatsApp inbound webhook verification | Any random string you also enter in the Meta webhook config. |

### Communications webhooks & signing

| Variable | Used by | Notes |
|---|---|---|
| `EMAIL_WEBHOOK_SECRET` | Inbound email → unified inbox (`/api/webhooks/email-inbound`) | Shared secret; required to accept inbound-email parse posts. |
| `UNSUBSCRIBE_SECRET` | One-click unsubscribe token signing | Fallback: `CREDENTIAL_ENCRYPTION_KEY`. |

### LinkedIn (organic posts + advertising + lead gen)

| Variable | Used by | Notes |
|---|---|---|
| `LINKEDIN_CLIENT_ID` | OAuth begin/exchange | From your LinkedIn developer app. |
| `LINKEDIN_CLIENT_SECRET` | OAuth exchange | |
| `LINKEDIN_WEBHOOK_SECRET` | LinkedIn webhooks (reported as configured/not) | |

### Google (Search Console + Ads)

| Variable | Used by | Notes |
|---|---|---|
| `GOOGLE_OAUTH_CLIENT_ID` | OAuth begin/exchange | From a Google Cloud OAuth client. |
| `GOOGLE_OAUTH_CLIENT_SECRET` | OAuth exchange | |
| `GOOGLE_ADS_DEVELOPER_TOKEN` | Google Ads API calls | Required for Google Ads campaign create/insights (not for Search Console). |

### Meta (Facebook/Instagram ads + lead ads)

| Variable | Used by | Notes |
|---|---|---|
| `META_APP_ID` | OAuth begin/exchange | From your Meta app. |
| `META_APP_SECRET` | OAuth exchange | |
| `META_WEBHOOK_SECRET` | Meta webhooks (reported as configured/not) | |
| `META_LEADS_VERIFY_TOKEN` | Meta Lead Ads webhook verification (`/api/webhooks/meta-leads`) | Any random string you also enter in Meta's webhook config. Falls back to `META_WEBHOOK_SECRET`. |

### Marketing signing & lead intake

| Variable | Used by | Notes |
|---|---|---|
| `MARKETING_OAUTH_SECRET` | OAuth CSRF **state** signing | Fallback: `CREDENTIAL_ENCRYPTION_KEY`. |
| `MARKETING_LEAD_WEBHOOK_SECRET` | Website/partner lead intake (`/api/webhooks/lead-intake`) | Fallback: `EMAIL_WEBHOOK_SECRET`. |

---

## 2. URLs to register with providers

All are relative to `APP_BASE_URL`.

| Purpose | URL | Notes |
|---|---|---|
| OAuth redirect (LinkedIn, Google, Meta — all three) | `{APP_BASE_URL}/api/marketing/oauth/callback` | Register this exact URI in each provider's OAuth app. |
| Comms — inbound email | `{APP_BASE_URL}/api/webhooks/email-inbound?secret={EMAIL_WEBHOOK_SECRET}` | Point your inbound-parse provider here. |
| Comms — WhatsApp | `{APP_BASE_URL}/api/webhooks/whatsapp` | Verify token = `WHATSAPP_VERIFY_TOKEN`. |
| Meta — Lead Ads | `{APP_BASE_URL}/api/webhooks/meta-leads` | Verify token = `META_LEADS_VERIFY_TOKEN`; subscribe the `leadgen` field. |
| Website/partner lead intake | `{APP_BASE_URL}/api/webhooks/lead-intake?secret={MARKETING_LEAD_WEBHOOK_SECRET}` | POST JSON lead fields; deduped, no auto-account. |
| Unsubscribe (auto, in email links) | `{APP_BASE_URL}/api/comms/unsubscribe?token=…` | No setup — generated into marketing emails. |

---

## 3. Provider setup steps

### LinkedIn
1. Create/confirm a LinkedIn **developer app**; confirm the PCI **Company Page**
   and that the connecting user holds a **Page admin** role.
2. Request **Community Management API** access (organisation posting).
3. Request **Advertising API** access (Development → Standard requires approval);
   confirm the **ad account**; request **Conversation Ads** capability and
   **Lead Gen** / lead-sync permission if needed.
4. Add the OAuth redirect URI (above); set `LINKEDIN_CLIENT_ID` /
   `LINKEDIN_CLIENT_SECRET` (and `LINKEDIN_WEBHOOK_SECRET` if using webhooks).
5. In Admin → Marketing → **Connected Accounts**, register the LinkedIn Company
   Page (and ad account), enter the **organisation id** / **ad-account id**, then
   **Connect** and complete OAuth.

### Google Search Console
1. **Verify** the property (`projectcontrolsinstitute.org`) in Search Console.
2. Create a Google Cloud project; **enable the Search Console API**; configure the
   OAuth consent screen + an OAuth client; add the redirect URI.
3. Set `GOOGLE_OAUTH_CLIENT_ID` / `GOOGLE_OAUTH_CLIENT_SECRET`.
4. Connect the verified property in Connected Accounts; test Search Analytics,
   Sitemaps, URL Inspection.

### Google Ads
1. Confirm the Google Ads account (and manager account, if used).
2. Obtain a **developer token**; set `GOOGLE_ADS_DEVELOPER_TOKEN`.
3. Reuse the Google OAuth client (or a dedicated one). Connect the account,
   entering the **customer id** (and manager/login-customer id in the business-id
   field). Test a low-budget campaign (created **PAUSED**) and conversions.

### Meta (Facebook + Instagram)
1. Confirm the Meta **Business** account, **Facebook Page**, **Instagram
   professional** account, and **ad account**.
2. Create/configure the Meta app; request **`ads_management`** (App Review) and,
   for lead ads, **`leads_retrieval`** + Page permissions.
3. Add the OAuth redirect URI; set `META_APP_ID` / `META_APP_SECRET` /
   `META_WEBHOOK_SECRET` / `META_LEADS_VERIFY_TOKEN`.
4. Register the Lead Ads webhook (above) and subscribe `leadgen`.
5. Connect the ad account in Connected Accounts, entering the **ad-account id**;
   test a draft campaign and the lead webhook.

---

## 4. Recommended activation order

1. `CREDENTIAL_ENCRYPTION_KEY` + `APP_BASE_URL` → redeploy.
2. **Email** (`RESEND_API_KEY` + `MAIL_FROM` on a verified domain) → redeploy.
   Verify: Admin → Communications → Dashboard shows *Email (Resend): configured*;
   send a sender-profile test.
3. **Comms inbound** (`EMAIL_WEBHOOK_SECRET`, optional `WHATSAPP_*`) → register
   webhooks → send a test inbound message; confirm it lands in the Unified Inbox.
4. **One marketing platform at a time.** Set that provider's env vars, register the
   redirect/webhook URLs, complete provider approval, redeploy, then Connect in the
   Admin Portal. The **Capability Registry** tab will move that feature from
   *provider approval required / not connected* to *available*.
5. For paid campaigns: create a PCI campaign → add a platform variant → approve →
   **Launch** (provider campaign is created **PAUSED**; activate it in the provider
   account after review). Use **Reporting → Sync spend/insights now** once live.

## 5. How to confirm each piece is live

- **Providers configured?** Admin → Communications → Dashboard, and Marketing →
  Dashboard (both show configured/not booleans only).
- **Capabilities real?** Marketing → **Capability Registry** shows the true,
  live status per feature.
- **Delivery working?** Communications → Delivery Queue (outbox) and Marketing →
  **Provider Jobs** show each attempt with the provider's actual response;
  failures carry a clear reason and safe retry.

> If a feature still shows *not connected* or a job fails with `no_access_token` /
> `provider_approval_required`, the corresponding step above is not yet complete —
> the code is ready and will start working as soon as it is.
