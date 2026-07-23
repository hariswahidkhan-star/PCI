import { test, expect } from '@playwright/test'
import { apiLoginAsOwnerAdmin, apiRegisterStudent, storyScreenshot } from './util'

test.describe('admin operations workflows', () => {
  test('impersonation support-view banner is read-only and can be ended', async ({ request, page }, testInfo) => {
    const ownerToken = await apiLoginAsOwnerAdmin(request, page)
    const student = await apiRegisterStudent(request, 'impersonation')
    const start = await request.post(`/api/admin/members/${student.user.id}/impersonate`, {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: { reason: 'E2E support-view verification' },
    })
    expect(start.ok(), `impersonation start should succeed (got ${start.status()})`).toBeTruthy()
    const session = (await start.json()) as { token: string; login_url: string }

    await page.goto(session.login_url)
    await expect(page.locator('.impersonation-banner')).toContainText(/support view/i)
    await expect(page.getByText(student.email).first()).toBeVisible()
    await storyScreenshot(page, testInfo, 'impersonation-banner')

    const forbiddenMutation = await request.post('/api/me/cpd', {
      headers: { Authorization: `Bearer ${session.token}` },
      data: { description: 'Forbidden support-view CPD', category: 'Structured learning', hours: 1 },
    })
    expect(forbiddenMutation.status()).toBe(403)
    expect(await forbiddenMutation.json()).toMatchObject({ error: 'impersonation_readonly' })

    const end = await request.post(`/api/admin/members/${student.user.id}/impersonate/end`, {
      headers: { Authorization: `Bearer ${ownerToken}` },
    })
    expect(end.ok(), `impersonation end should succeed (got ${end.status()})`).toBeTruthy()
  })

  test('owner can smoke key operations pages', async ({ request, page }, testInfo) => {
    await apiLoginAsOwnerAdmin(request, page)
    const pages = [
      { path: '/admin/emails', heading: /email log/i },
      { path: '/admin/audit', heading: /audit log/i },
      { path: '/admin/settings', heading: /settings/i },
    ]
    for (const p of pages) {
      await page.goto(p.path)
      await expect(page.getByRole('heading', { name: p.heading })).toBeVisible()
    }
    await storyScreenshot(page, testInfo, 'admin-operations-pages')
  })
})
