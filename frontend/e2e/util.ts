import type { APIRequestContext, Page } from '@playwright/test'
import { expect } from '@playwright/test'

// Shared helpers for the browser E2E specs. The Playwright webServer boots the backend with
// ASPNETCORE_ENVIRONMENT=Development, so the first-run seeds in backend/Data/Migrate.cs apply:
//  - bootstrap owner admin  owner@pci.local / changeme-owner  (flagged must_change_pw)
//  - demo student           student@pci.local / changeme-student (created while users table is empty)
// Both are loudly logged by the backend as dev-only accounts; the demo student is never seeded in
// Production, so these credentials exist exactly in the environments where this suite runs.
export const DEMO_STUDENT = { email: 'student@pci.local', password: 'changeme-student' }
export const OWNER_ADMIN = { email: 'owner@pci.local', password: 'changeme-owner' }

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
