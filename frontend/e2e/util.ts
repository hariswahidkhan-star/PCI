import type { APIRequestContext, Page, TestInfo } from '@playwright/test'
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
export const E2E_ADMIN = { email: 'browser-admin@pci.test', password: 'BrowserSuiteAdmin-2026' }
export const E2E_STRIPE_WEBHOOK_SECRET = 'whsec_e2e_browser_suite'

let counter = 0
/** Cache the E2E operator token across specs. /api/admin/auth/login is rate-limited (10/min/IP);
 * without a cache the expanded browser suite burns the window and cascades 429 failures. */
let e2eAdminTokenCache: string | null = null

/** A unique, valid email per call: pid + wall-clock + a process-local counter, so parallel
 *  workers, retries and repeated runs against the same reused dev database never collide. */
export function uniqueEmail(prefix = 'e2e'): string {
  counter += 1
  return `${prefix}-${process.pid}-${Date.now()}-${counter}@e2e.pci.local`
}

/** Persist an explicit success-state screenshot in Playwright's per-test output and HTML report.
 * Failure screenshots remain automatic; these named captures provide the audit evidence requested
 * for successful user stories as well. */
export async function captureStoryEvidence(page: Page, testInfo: TestInfo, storyId: string, moment?: string): Promise<void> {
  const label = [storyId, moment].filter(Boolean).join('-').replace(/[^A-Za-z0-9._-]+/g, '-')
  const path = testInfo.outputPath(`${label}.png`)
  // Prefer the viewport: full-page screenshots of tall public pages exceed Chromium's ~32767px
  // dimension limit and abort otherwise-green journeys.
  await page.screenshot({ path, fullPage: false, animations: 'disabled' })
  await testInfo.attach(label, { path, contentType: 'image/png' })
}

/** Keep unrelated public-site consent/announcement overlays from intercepting clicks in a journey
 * whose subject is a form or download. Announcement behaviour has its own dedicated browser test. */
export async function preparePublicJourney(page: Page): Promise<void> {
  await page.addInitScript(() => {
    try { localStorage.setItem('pci-cookie-consent', 'essential') } catch { /* no storage */ }
    try { sessionStorage.setItem('pciannx', '1') } catch { /* no storage */ }
    ;(window as typeof window & { PCI_NO_ANNOUNCE?: boolean }).PCI_NO_ANNOUNCE = true
  })
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
      // Plant once per tab. Re-applying on every navigation would overwrite a fresher UI login
      // (and resurrect a token revoked by Sign out).
      if (sessionStorage.getItem('pci.e2e.session.planted') === '1') return
      sessionStorage.setItem('pci.session.token', t)
      sessionStorage.setItem('pci.e2e.session.planted', '1')
    } catch {
      /* storage unavailable — the test will fail on the auth redirect instead */
    }
  }, token)
}

/** Sign in as the opt-in, non-Production browser-suite owner. The Playwright webServer is the only
 * caller that sets E2E_ADMIN_PASSWORD, so this account never exists in a normal deployment. */
export async function apiLoginAsE2EAdmin(request: APIRequestContext, page?: Page): Promise<string> {
  async function plant(token: string) {
    if (page) {
      await page.addInitScript((t) => {
        try {
          if (sessionStorage.getItem('pci.e2e.admin.planted') === '1') return
          sessionStorage.setItem('pci.admin.token', t)
          sessionStorage.setItem('pci.e2e.admin.planted', '1')
        } catch { /* asserted by the auth redirect */ }
      }, token)
    }
  }
  if (e2eAdminTokenCache) {
    await plant(e2eAdminTokenCache)
    return e2eAdminTokenCache
  }
  const res = await request.post('/api/admin/auth/login', { data: E2E_ADMIN })
  expect(res.ok(), `E2E admin login should succeed (got ${res.status()})`).toBeTruthy()
  const body = (await res.json()) as { token?: string; admin?: { must_change_pw?: boolean } }
  expect(body.token, 'admin login response should carry a session token').toBeTruthy()
  expect(body.admin?.must_change_pw, 'E2E operator should be seeded with a real password (advisory flag clear)').toBe(false)
  const token = body.token as string
  e2eAdminTokenCache = token
  await plant(token)
  return token
}

export type TestUserScenario = 'ready' | 'unpaid' | 'member' | 'waived' | 'incomplete_profile' | 'no_id' | 'certuvo_failed'
export interface TestUser {
  id: number
  email: string
  password: string
  token: string
  scenario: TestUserScenario
}

export interface RegisteredStudent {
  id: number
  email: string
  password: string
  token: string
}

/** Create an ordinary self-service student through the public registration API. Unlike scenario
 * accounts, this exercises the same account/session path used by the registration form and leaves
 * the profile empty enough for onboarding, account-security and recovery journeys. */
export async function registerStudent(
  request: APIRequestContext,
  page?: Page,
  prefix = 'student',
): Promise<RegisteredStudent> {
  const email = uniqueEmail(prefix)
  const password = 'StudentBrowser-2026!'
  const response = await request.post('/api/register', {
    data: {
      firstName: 'Browser',
      lastName: 'Student',
      email,
      password,
      confirmPassword: password,
      country: 'United Kingdom',
      mobile: '+44 7700 900000',
    },
  })
  expect(response.ok(), `student registration should succeed (got ${response.status()})`).toBeTruthy()
  const body = (await response.json()) as {
    token?: string
    user?: { id?: number; email?: string }
  }
  expect(body.token).toBeTruthy()
  expect(body.user?.id).toBeGreaterThan(0)
  expect(body.user?.email).toBe(email)
  const token = body.token as string
  if (page) {
    await page.addInitScript((t) => {
      try {
        // Plant once per tab. After Sign out + UI re-login, do not resurrect this revoked token.
        if (sessionStorage.getItem('pci.e2e.session.planted') === '1') return
        sessionStorage.setItem('pci.session.token', t)
        sessionStorage.setItem('pci.e2e.session.planted', '1')
      } catch { /* asserted by the auth redirect */ }
    }, token)
  }
  return { id: body.user!.id!, email, password, token }
}

/** Create a scenario account through the real admin operator API and optionally adopt its student
 * session in a page. This is deliberately not a DB seed: the journey verifies the admin control. */
export async function createTestUser(
  request: APIRequestContext,
  adminToken: string,
  scenario: TestUserScenario,
  page?: Page,
): Promise<TestUser> {
  const res = await request.post('/api/admin/test-users', {
    headers: { Authorization: `Bearer ${adminToken}` },
    data: { scenario, certification: 'PML-AI' },
  })
  expect(res.ok(), `${scenario} test user should be created (got ${res.status()})`).toBeTruthy()
  const user = (await res.json()) as TestUser
  expect(user.id).toBeGreaterThan(0)
  expect(user.token).toBeTruthy()
  expect(user.scenario).toBe(scenario)
  if (page) {
    await page.addInitScript((t) => {
      try {
        if (sessionStorage.getItem('pci.e2e.session.planted') === '1') return
        sessionStorage.setItem('pci.session.token', t)
        sessionStorage.setItem('pci.e2e.session.planted', '1')
      } catch { /* asserted by the auth redirect */ }
    }, user.token)
  }
  return user
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

/** Generate the RFC 6238 six-digit code used by both student and admin TOTP enrolment. Keeping the
 * authenticator in the test process avoids any provider dependency while still exercising the real
 * server secret, time-step validation, replay guard and login prompts. */
export function totpCode(secret: string, atMs = Date.now()): string {
  const alphabet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ234567'
  let bits = ''
  for (const ch of secret.replace(/=+$/g, '').replace(/\s+/g, '').toUpperCase()) {
    const value = alphabet.indexOf(ch)
    if (value < 0) throw new Error(`Invalid base32 character in TOTP secret: ${ch}`)
    bits += value.toString(2).padStart(5, '0')
  }
  const bytes: number[] = []
  for (let i = 0; i + 8 <= bits.length; i += 8) bytes.push(parseInt(bits.slice(i, i + 8), 2))
  const counter = Buffer.alloc(8)
  counter.writeBigUInt64BE(BigInt(Math.floor(atMs / 30_000)))
  const digest = createHmac('sha1', Buffer.from(bytes)).update(counter).digest()
  const offset = digest[digest.length - 1] & 0x0f
  const binary =
    ((digest[offset] & 0x7f) << 24) |
    ((digest[offset + 1] & 0xff) << 16) |
    ((digest[offset + 2] & 0xff) << 8) |
    (digest[offset + 3] & 0xff)
  return String(binary % 1_000_000).padStart(6, '0')
}
