import { test, expect } from '@playwright/test'
import { apiLoginAsOwnerAdmin, seedAdminSession, storyScreenshot, uniqueEmail } from './util'

test.describe('admin security and RBAC', () => {
  test('owner reaches settings and the 2FA status card', async ({ request, page }, testInfo) => {
    await apiLoginAsOwnerAdmin(request, page)
    await page.goto('/admin/settings')
    await expect(page.getByRole('heading', { name: 'Settings' })).toBeVisible()
    await expect(page.getByText('Two-factor authentication')).toBeVisible()
    await expect(page.getByText(/Status:/)).toBeVisible()
    await storyScreenshot(page, testInfo, 'admin-settings-2fa-status')
  })

  test('least-privilege viewer has hidden nav and forbidden sections', async ({ request, page }, testInfo) => {
    const ownerToken = await apiLoginAsOwnerAdmin(request)
    const email = uniqueEmail('admin-viewer')
    const password = 'viewer-pass-123'
    const create = await request.post('/api/admin/team', {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: { email, name: 'E2E Viewer', role: 'viewer', password, force_password_change: false },
    })
    expect(create.ok(), `viewer admin creation should succeed (got ${create.status()})`).toBeTruthy()

    const login = await request.post('/api/admin/auth/login', { data: { email, password } })
    expect(login.ok(), `viewer admin login should succeed (got ${login.status()})`).toBeTruthy()
    const body = (await login.json()) as { token: string }
    await seedAdminSession(page, body.token)
    await page.goto('/admin/')

    await expect(page.getByText(email).first()).toBeVisible()
    await expect(page.getByRole('link', { name: 'Reports' })).toBeVisible()
    await expect(page.getByRole('link', { name: 'Team & Access' })).toHaveCount(0)
    await expect(page.getByRole('link', { name: 'Credentials' })).toHaveCount(0)

    await page.goto('/admin/team')
    await expect(page.getByText(/owners only|do not have permission/i)).toBeVisible()
    const forbidden = await request.get('/api/admin/team', { headers: { Authorization: `Bearer ${body.token}` } })
    expect(forbidden.status()).toBe(403)
    await storyScreenshot(page, testInfo, 'viewer-forbidden-team')
  })
})
