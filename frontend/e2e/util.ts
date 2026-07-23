import type { APIRequestContext, Page, TestInfo } from '@playwright/test'
import { expect } from '@playwright/test'

// Shared helpers for the browser E2E specs. The Playwright webServer boots the backend with
// ASPNETCORE_ENVIRONMENT=Development, so the first-run seeds in backend/Data/Migrate.cs apply:
//  - bootstrap owner admin  owner@pci.local / changeme-owner  (flagged must_change_pw)
//  - demo student           student@pci.local / changeme-student (created while users table is empty)
// Both are loudly logged by the backend as dev-only accounts; the demo student is never seeded in
// Production, so these credentials exist exactly in the environments where this suite runs.
export const DEMO_STUDENT = { email: 'student@pci.local', password: 'changeme-student' }
export const OWNER_ADMIN = { email: 'owner@pci.local', password: 'changeme-owner' }
export const E2E_OWNER_PASSWORD = process.env.E2E_OWNER_PASSWORD || 'changeme-owner-e2e-2026'
export const E2E_STUDENT_PASSWORD = 'e2e-pass-123'

let counter = 0
let ownerTokenCache: string | null = null

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
  await seedStudentSession(page, token)
}

export async function seedStudentSession(page: Page, token: string): Promise<void> {
  await page.addInitScript((t) => {
    try {
      sessionStorage.setItem('pci.session.token', t)
    } catch {
      /* storage unavailable — the test will fail on the auth redirect instead */
    }
  }, token)
}

export async function seedAdminSession(page: Page, token: string): Promise<void> {
  await page.addInitScript((t) => {
    try {
      sessionStorage.setItem('pci.admin.token', t)
    } catch {
      /* storage unavailable — the test will fail on the auth redirect instead */
    }
  }, token)
}

export async function apiRegisterStudent(
  request: APIRequestContext,
  prefix = 'student',
): Promise<{ email: string; password: string; token: string; user: { id?: number; email?: string } }> {
  const email = uniqueEmail(prefix)
  const res = await request.post('/api/register', {
    data: {
      firstName: 'E2E',
      lastName: 'Student',
      email,
      password: E2E_STUDENT_PASSWORD,
      confirmPassword: E2E_STUDENT_PASSWORD,
      country: 'United Kingdom',
      mobile: '+447700900000',
    },
  })
  expect(res.ok(), `student registration should succeed (got ${res.status()})`).toBeTruthy()
  const body = (await res.json()) as { token?: string; user?: { id?: number; email?: string } }
  expect(body.token, 'registration response should include a session token').toBeTruthy()
  let user = body.user ?? {}
  if (!user.id) {
    const me = await request.get('/api/me', { headers: { Authorization: `Bearer ${body.token}` } })
    if (me.ok()) {
      const meBody = (await me.json()) as { user?: { id?: number; email?: string } }
      user = meBody.user ?? user
    }
  }
  expect(user.id, 'registered student id should be available').toBeTruthy()
  return { email, password: E2E_STUDENT_PASSWORD, token: body.token as string, user }
}

export async function apiLoginAsOwnerAdmin(request: APIRequestContext, page?: Page): Promise<string> {
  if (ownerTokenCache) {
    if (page) await seedAdminSession(page, ownerTokenCache)
    return ownerTokenCache
  }

  async function login(password: string) {
    const res = await request.post('/api/admin/auth/login', { data: { email: OWNER_ADMIN.email, password } })
    if (!res.ok()) return null
    return (await res.json()) as {
      token?: string
      admin?: { must_change_pw?: boolean }
    }
  }

  let body = await login(OWNER_ADMIN.password)
  if (!body?.token) body = await login(E2E_OWNER_PASSWORD)
  expect(body?.token, 'owner admin login should succeed with seeded or E2E password').toBeTruthy()
  const token = body!.token as string

  if (body!.admin?.must_change_pw) {
    const res = await request.post('/api/admin/me/password', {
      headers: { Authorization: `Bearer ${token}` },
      data: { new_password: E2E_OWNER_PASSWORD },
    })
    expect(res.ok(), `owner password change should succeed (got ${res.status()})`).toBeTruthy()
  }

  ownerTokenCache = token
  if (page) await seedAdminSession(page, token)
  return token
}

export async function storyScreenshot(page: Page, testInfo: TestInfo, name: string): Promise<void> {
  await page.screenshot({ path: testInfo.outputPath(`${name}.png`), fullPage: true })
}

/** Dismiss the public cookie banner / overlays that can intercept clicks on static auth pages. */
export async function dismissPublicOverlays(page: Page): Promise<void> {
  const accept = page.locator('#ckAccept')
  if (await accept.isVisible().catch(() => false)) {
    await accept.click()
  }
  await page.evaluate(() => {
    try {
      localStorage.setItem('pci-cookie-consent', 'essential')
    } catch {
      /* ignore */
    }
    document.getElementById('ckBanner')?.classList.remove('show')
  })
}

export async function certificationId(request: APIRequestContext, code: string): Promise<number> {
  const res = await request.get('/api/certifications')
  expect(res.ok(), `certifications should load (got ${res.status()})`).toBeTruthy()
  const data = (await res.json()) as { rows: Array<{ id: number; code: string }> }
  const cert = data.rows.find((r) => r.code.toLowerCase() === code.toLowerCase())
  expect(cert, `${code} should exist`).toBeTruthy()
  return cert!.id
}
