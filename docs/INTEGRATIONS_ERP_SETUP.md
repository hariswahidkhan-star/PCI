# ERP connector setup — Zoho Books & Odoo

Both connectors plug into the platform's existing integration pipeline (outbox → delivery ledger →
retry/backoff → atomic worker lease). Nothing about delivery, auditing or retry is connector-specific;
each adds only its vendor's authentication and object mapping.

Events both connectors handle:

| Canonical event | Zoho Books | Odoo |
|---|---|---|
| `member.registered` | Contact | `res.partner` |
| `payment.recorded` | Invoice | `account.move` (`out_invoice`) |
| `membership.activated` | *skipped* — a CRM state, not an accounting entry | *skipped* |

A skipped event is terminal, not a failure: it is never retried and never counts as an error.

Configure both in **Admin Console → Integrations**. `GET /api/admin/integrations` returns a
`provider_fields` block describing every field below (label, required, secret, help), so the console
renders each vendor's form from the server rather than hardcoding it.

---

## Zoho Books

### 1. Create the credentials

1. Go to the [Zoho API console](https://api-console.zoho.com/) and create a **Self Client**.
2. Generate a refresh token with **exactly** these scopes:
   ```
   ZohoBooks.contacts.CREATE,ZohoBooks.contacts.READ,ZohoBooks.invoices.CREATE
   ```
   The connector creates contacts, reads them to avoid duplicates, and creates invoices — nothing
   else. Do not grant `DELETE`.
3. Copy your **Organization ID** from Zoho Books → *Settings → Organizations*.

### 2. Pick the right data centre

Zoho runs separate data centres and **a token minted in one is invalid in another** — the most common
setup failure. Choose the region your Zoho account lives in:

| Setting | API host | Accounts host |
|---|---|---|
| `com` (default, US) | `www.zohoapis.com` | `accounts.zoho.com` |
| `eu` | `www.zohoapis.eu` | `accounts.zoho.eu` |
| `in` | `www.zohoapis.in` | `accounts.zoho.in` |
| `com.au` | `www.zohoapis.com.au` | `accounts.zoho.com.au` |
| `jp` | `www.zohoapis.jp` | `accounts.zoho.jp` |
| `ca` | `www.zohoapis.ca` | `accounts.zoho.ca` |

### 3. Fields

| Field | Required | Notes |
|---|---|---|
| `organization_id` | ✅ | Sent on every request. Without it Zoho answers *"Organization not found"* whatever the token. |
| `region` | ✅ | See the table above. |
| `client_id` / `client_secret` / `refresh_token` | — | The durable choice: access tokens are minted automatically and persisted encrypted. |
| `access_token` | — | Shortcut for testing. Zoho access tokens expire in ~1 hour. |
| `invoice_status` | — | `draft` (default) or `sent`. `sent` makes **Zoho email the customer** — switch on deliberately. |
| `item_id` | — | Bill an existing Books item so revenue lands on your chart of accounts. |
| `tax_id`, `currency`, `place_of_supply`, `payment_terms`, `salesperson`, `notes` | — | Omitted entirely when blank, so Books applies its organisation default. |
| `contact_reuse` | — | Default **on**: a returning payer reuses their contact. Off mints a new contact per payment (duplicates customers). |
| `api_base` | — | Point deliveries at a test receiver. **The OAuth accounts host is deliberately not redirected**, so real credentials are never sent to a test endpoint. |

### 4. Limits and behaviour

- **100 API calls/minute per organization**; Zoho answers HTTP 429 beyond it. A payment delivery costs
  up to three calls (token → contact lookup → invoice), so budget accordingly on bulk backfills.
- Zoho Books is customer-centric: an invoice must belong to a contact, so a payment resolves the
  contact by email and creates one when absent.
- A refresh that Zoho rejects returns HTTP 200 with an `{"error": …}` body; the connector treats that
  as a failure rather than sending an empty `Authorization` header.

---

## Odoo

### 1. Create the credentials

1. In Odoo: **Preferences → Account Security → New API Key**.
2. Use your **login email** and that key. The key replaces the password everywhere.
3. The **database name** is the one in your instance URL.

### 2. Fields

| Field | Required | Notes |
|---|---|---|
| `url` | ✅ | `https://your-instance.odoo.com`. Calls go to `{url}/jsonrpc`. |
| `database` | ✅ | |
| `login` | ✅ | The user email the API key belongs to. |
| `api_key` | ✅ | Write-only; never read back by the API. |
| `company_id` | — | Numeric `res.company` id — needed on multi-company databases. |
| `journal_id`, `product_id`, `account_id`, `tax_id`, `currency_id`, `payment_term_id`, `team_id` | — | Numeric record ids. Rejected at save time if not numeric, rather than being silently ignored later. |
| `partner_reuse` | — | Default **on**. Off creates a new partner per payment. |
| `partner_model` / `invoice_model` | — | Override `res.partner` / `account.move` for customised databases. |

### 3. Behaviour worth knowing

- **JSON-RPC faults arrive as HTTP 200.** Odoo reports failures with status 200 and an `error` member,
  so a status-code-only check would record deliveries that never happened. This connector inspects the
  body: a fault, or `authenticate` returning `false`, records a **failed** delivery with the reason.
- **Invoices are created as drafts.** Posting is a second RPC (`action_post`) after the create returns
  an id, and a delivery here is exactly one request whose status is the delivery's outcome. Rather than
  ship a "post the invoice" setting that silently left everything in draft, posting is left to an
  accountant in Odoo — which is also the safer accounting default.
- A tax is written with the many2many replace command `(6, 0, [id])`. A bare id is silently ignored by
  Odoo, which would produce untaxed invoices.

---

## Verifying a connector

1. **Save** it, then check `GET /api/admin/integrations`: `secret_fields` lists which credentials are
   set; the values are never returned.
2. **Test delivery** (`POST /api/admin/integrations/{id}/test`) sends a `ping`. Both ERP connectors
   answer `skipped` for a ping — that is correct, since neither has an accounting object for it. It
   proves the connector is reachable and configured, not that a mapping works.
3. **Real proof** is a real event: register a test member or record a settlement, then check
   *Integrations → Deliveries*. `delivered` means the vendor accepted the mapped object.
4. Failures carry the vendor's own reason in `last_error`, and are retried with exponential backoff.

## Security posture

- Secrets are envelope-encrypted at rest and **write-only** over the API — the console shows only
  which fields are populated.
- Every outbound URL (instance URL, API base override) goes through the egress guard, so a connector
  cannot be pointed at loopback/private/metadata addresses in production.
- Deliveries are claimed with an atomic worker lease, so two instances never double-post an invoice.
