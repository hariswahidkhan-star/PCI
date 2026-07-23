import { test, expect } from '@playwright/test'
import { apiLoginAsOwnerAdmin, seedAdminSession, storyScreenshot, uniqueEmail } from './util'

test.describe('admin proctoring', () => {
  test('owner can load proctoring sessions and live heartbeat view', async ({ request, page }, testInfo) => {
    const ownerToken = await apiLoginAsOwnerAdmin(request, page)

    // Candidate heartbeat/proctor channels are API-first:
    // - POST /api/me/exam/heartbeat records candidate status, proctor events and candidate chat.
    // - GET /api/admin/exam-sessions/live reads in-progress attempts with heartbeat freshness.
    // - POST /api/admin/exam-sessions/{id}/message sends proctor messages back to a candidate.
    const sessions = await request.get('/api/admin/exam-sessions', { headers: { Authorization: `Bearer ${ownerToken}` } })
    expect(sessions.ok(), `exam sessions API should load (got ${sessions.status()})`).toBeTruthy()
    const live = await request.get('/api/admin/exam-sessions/live', { headers: { Authorization: `Bearer ${ownerToken}` } })
    expect(live.ok(), `live sessions API should load (got ${live.status()})`).toBeTruthy()

    await page.goto('/admin/proctoring')
    await expect(page.getByRole('heading', { name: /proctoring.*sessions/i })).toBeVisible()
    await page.getByRole('button', { name: 'Live' }).click()
    await expect(page.getByText(/No exams are in progress|Candidate/)).toBeVisible()
    await storyScreenshot(page, testInfo, 'admin-proctoring-live')
  })

  test('viewer is denied proctoring access', async ({ request, page }) => {
    const ownerToken = await apiLoginAsOwnerAdmin(request)
    const email = uniqueEmail('proctor-viewer')
    const password = 'viewer-pass-123'
    const create = await request.post('/api/admin/team', {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: { email, name: 'E2E Proctor Viewer', role: 'viewer', password, force_password_change: false },
    })
    expect(create.ok()).toBeTruthy()
    const login = await request.post('/api/admin/auth/login', { data: { email, password } })
    const viewer = (await login.json()) as { token: string }

    const forbidden = await request.get('/api/admin/exam-sessions', { headers: { Authorization: `Bearer ${viewer.token}` } })
    expect(forbidden.status()).toBe(403)

    await seedAdminSession(page, viewer.token)
    await page.goto('/admin/proctoring')
    await expect(page.getByText(/do not have permission/i)).toBeVisible()
  })
})
