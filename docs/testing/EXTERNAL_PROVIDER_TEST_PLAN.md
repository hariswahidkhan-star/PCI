# PCI Platform — External Provider Test Plan

_What is exercised against in-repo mocks/sandboxes today, and the operator-executed sandbox runs that
close each provider gap. Egress is blocked in the build/test environment, so live provider calls are
**Operator/External-pending** — documented here, never simulated as if real._

## Legend
- **In-CI (mock):** an in-repo mock server on loopback exercises the code path deterministically.
- **Operator-sandbox:** a human runs the provider's test-mode/sandbox against a staging deployment.
- **Evidence to capture:** what the operator records so the run is auditable and repeatable.

## 1. Stripe (payments, webhooks, dues subscriptions)

| Aspect | In-CI (mock) | Operator-sandbox | Evidence to capture |
|---|---|---|---|
| Checkout session create | placeholder key; URL returned | test-mode Checkout in staging | session id, redirect, return to `/billing` |
| Webhook settle/refund/dispute | **signed** in-suite Charge/Dispute/Invoice/Subscription objects (§1/§29) | Stripe CLI `trigger` against staging endpoint | event id, signature verified, downstream state |
| Membership dues (invoice.paid / subscription.*) | not unit-isolatable (Stripe-object-bound) — **DEF-3** | test-mode subscription lifecycle | renewal date math, mirror rows |
| Idempotency / replay | asserted (§1/§29 replay) | duplicate delivery in test-mode | single settlement, no double-grant |

Keys are test-mode only; the signing secret is the test webhook secret. No live card data.

## 2. Certuvo (external practice/credential platform)

| Aspect | In-CI (mock) | Operator-sandbox | Evidence |
|---|---|---|---|
| Provision / username / temp password | loopback mock (§15) | Certuvo sandbox tenant | account created, credentials **not** logged |
| Retry / suspend / revoke / resend | mock status transitions (§15) | sandbox status calls | status ledger, idempotency key |
| Webhook back-channel | mock signed callback | sandbox webhook | signature, dedup |

## 3. Exam-delivery vendors (Pearson VUE, Kryterion, Questionmark, PSI, TestReach)

| Aspect | In-CI (mock) | Operator-sandbox | Evidence |
|---|---|---|---|
| Register → provision → result → credential | `_MockVendor` (§11); Questionmark sync + PSI callback full-cycle | each vendor's certification/sandbox env | mapping, provisioning id, result parse, credential issued |
| Delivery-mode switch (in-house vs vendor) | asserted (§11/admin) | sandbox routing | routed flag, confirmation, self-schedule link |

CT-3 (drive PVUE/Kryterion/TestReach full-cycle against the mock) is the remaining in-CI item.

## 4. Communications (email, WhatsApp/Meta, marketing OAuth)

| Aspect | In-CI (mock/console) | Operator-sandbox | Evidence |
|---|---|---|---|
| Email send | console provider sink drains to `sent` (§55) | SMTP/Resend test account | provider message id, delivery log |
| WhatsApp opt-in/verification | fail-closed handshake asserted (§55) | Meta sandbox number | verify token, consent state |
| Marketing OAuth (LinkedIn/Meta/GSC) | signed-state verify; token-exchange fail-closed (§56) | provider sandbox app | connection status, **no secret** emitted |
| Lead webhooks | fail-closed (§56) | provider test lead | consent-0 stub, dedup |

## 5. Google Sign-In

| Aspect | In-CI | Operator-sandbox | Evidence |
|---|---|---|---|
| `/api/auth/config` gating | asserted (button hidden when unconfigured) | real client id in staging | button renders, id token round-trip |
| Credential exchange | stubbed in component tests | Google test account | session issued, email match |

## 6. Storage (S3)

| Aspect | In-CI | Operator-sandbox | Evidence |
|---|---|---|---|
| Put/Get, encryption-at-rest, dedup | local provider (xUnit) + **moto** (`storage_s3_test.py`) | real bucket in staging | object key, sha256, encrypted bytes |
| Provider-down / permission-denied | dangling-ref 404 (§59) | fault injection in staging | RES-2 |

## 7. Render deployment

| Aspect | In-CI | Operator | Evidence |
|---|---|---|---|
| Build (Dockerfile) | image builds in CI | Render build | image digest |
| Health / system-check / persistent disk | partial (§health) | live `/api/health`, system-check authz | recovery_configured, checks map, `/data` durability |
| Read-only prod smoke (DEP-1) | — | `smoke-test.sh` read-only variant | headers, catalogue, verify, 401 gates |

## 8. Operator run checklist (per provider)

1. Point a **staging** deployment (never production) at the provider's **sandbox/test-mode**.
2. Use synthetic accounts only; capture the evidence column above.
3. Confirm no secret/PII is written to logs or artifacts.
4. Record pass/fail + evidence in the release record (`RELEASE_READINESS_TEMPLATE.md`,
   "External-provider-pending" section) and flip the RTM row from ⏳ to ✅ once green.
