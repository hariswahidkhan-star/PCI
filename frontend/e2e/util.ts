import type { APIRequestContext, Page } from '@playwright/test'
import { expect } from '@playwright/test'
import { createHmac } from 'node:crypto'

// Shared helpers for the browser E2E specs. The Playwright webServer boots the backend with
// ASPNETCORE_ENVIRONMENT=Development, so the first-run seeds in backend/Data/Migrate.cs apply:
//  - bootstrap owner admin  owner@pci.local / changeme-owner  (flagged must_change_pw)
//  - demo student           student@pci.local / changeme-student (created while users table is empty)
// Both are loudly logged by the backend as dev-only accounts; the demo student is never seeded in
// Production, so these credentials exist exactly in the environments where this suite runs.
export const DEMO_STUDENT = { email: 'student@pci.local', password: 'changeme-student' }
export const OWNER_ADMIN = { email: 'owner@pci.local', password: 'changeme-owner' }
export const E2E_STRIPE_WEBHOOK_SECRET = 'whsec_e2e_browser_suite'

let counter = 0

/** A unique, valid email per call: pid + wall-clock + a process-local counter, so parallel
 *  workers, retries and repeated runs against the same reused dev database never collide. */
export function uniqueEmail(prefix = 'e2e'): string {
  counter += 1
  return `${prefix}-${process.pid}-${Date.now()}-${counter}@e2e.pci.local`
}

/** Sign the demo student in via the API and plant the session token where the SPA looks for it
 *  (sessionStorage 'pci.session.token' — see frontend/src/api/client.ts), so authenticated screens
 *  can be exercised without repeating the UI login journey in every spec. */
export async function apiLoginAsDemoStudent(request: APIRequestContext, page: Page): Promise<void> {
  const res = await request.post('/api/login', { data: DEMO_STUDENT })
  expect(res.ok(), `demo student login should succeed (got ${res.status()})`).toBeTruthy()
  const body = (await res.json()) as { token?: string }
  expect(body.token, 'login response should carry a session token').toBeTruthy()
  const token = body.token as string
  await page.addInitScript((t) => {
    try {
      sessionStorage.setItem('pci.session.token', t)
    } catch {
      /* storage unavailable — the test will fail on the auth redirect instead */
    }
  }, token)
}

/** Settle a synthetic exam purchase through the real signed Stripe webhook. The browser job owns an
 * isolated Development database and a test-only signing secret; no Stripe network call is made. */
export async function settleExamPurchase(
  request: APIRequestContext,
  email: string,
  certification: 'PCL-AI' | 'PFL-AI' | 'PML-AI',
  product: 'exam' | 'bundle' = 'exam',
): Promise<void> {
  const nonce = `${process.pid}-${Date.now()}-${certification.toLowerCase()}`
  const sessionId = `cs_e2e_${nonce}`
  const paymentIntent = `pi_e2e_${nonce}`
  const event = {
    id: `evt_e2e_${nonce}`,
    object: 'event',
    api_version: '2024-06-20',
    created: Math.floor(Date.now() / 1000),
    livemode: false,
    pending_webhooks: 1,
    request: { id: null, idempotency_key: null },
    type: 'checkout.session.completed',
    data: {
      previous_attributes: null,
      object: {
        id: sessionId,
        object: 'checkout.session',
        amount_total: product === 'bundle' ? 39_900 : 35_000,
        customer_email: email,
        customer_details: { email },
        payment_intent: paymentIntent,
        mode: 'payment',
        payment_status: 'paid',
        metadata: {
          product, certification,
          first_name: 'Browser', last_name: 'Isolation', country: 'GB',
          standard_amount: product === 'bundle' ? '399' : '350', default_discount: '0', code_amount: '0', final_amount: product === 'bundle' ? '399' : '350',
        },
      },
    },
  }
  const payload = JSON.stringify(event)
  const timestamp = Math.floor(Date.now() / 1000)
  const signature = createHmac('sha256', E2E_STRIPE_WEBHOOK_SECRET)
    .update(`${timestamp}.${payload}`)
    .digest('hex')
  const response = await request.post('/api/webhook', {
    data: payload,
    headers: {
      'Content-Type': 'application/json',
      'Stripe-Signature': `t=${timestamp},v1=${signature}`,
    },
  })
  expect(response.ok(), `signed ${product} settlement for ${certification} should succeed (got ${response.status()})`).toBeTruthy()
}
