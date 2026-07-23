import { test, expect } from '@playwright/test'
import {
  E2E_ADMIN,
  apiLoginAsE2EAdmin,
  captureStoryEvidence,
  totpCode,
  uniqueEmail,
} from './util'

test.describe('admin account security, RBAC and settings', () => {
  test('owner provisions a viewer whose UI and API remain least-privileged', async ({ page, context, request }, testInfo) => {
    const ownerToken = await apiLoginAsE2EAdmin(request, page)
    const email = uniqueEmail('admin-viewer')
    await page.goto('/admin/team')
    await expect(page.getByRole('heading', { name: 'Team & Access' })).toBeVisible()
    await page.getByRole('button', { name: 'Add member' }).click()
    await page.getByLabel('Email', { exact: true }).fill(email)
    await page.getByLabel('Name', { exact: true }).fill('Browser Read-only Reviewer')
    await page.getByLabel('Role', { exact: true }).selectOption('viewer')
    const createResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/admin/team') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Create member' }).click()
    expect((await createResponsePromise).ok()).toBeTruthy()
    const temporary = page.getByText(/Temporary password:/)
    await expect(temporary).toBeVisible()
    const tempText = await temporary.innerText()
    const passwordMatch = /Temporary password:\s*([a-f0-9]+)/i.exec(tempText)
    expect(passwordMatch?.[1]).toBeTruthy()
    const temporaryPassword = passwordMatch![1]
    await captureStoryEvidence(page, testInfo, 'G18', 'viewer-created-with-role-summary')

    const viewerPage = await context.newPage()
    await viewerPage.goto('/admin/login')
    await viewerPage.getByLabel('Email address').fill(email)
    await viewerPage.getByLabel('Password').fill(temporaryPassword)
    await viewerPage.getByRole('button', { name: 'Sign in' }).click()
    await expect(viewerPage.getByRole('heading', { name: 'Set a new password' })).toBeVisible()
    const newPassword = 'ViewerBrowser-2026!'
    await viewerPage.getByLabel('New password', { exact: true }).fill(newPassword)
    await viewerPage.getByLabel('Confirm password', { exact: true }).fill(newPassword)
    await viewerPage.getByRole('button', { name: 'Update password' }).click()
    await expect(viewerPage.getByText('Browser Read-only Reviewer')).toBeVisible()
    await expect(viewerPage.getByRole('link', { name: 'Reports' })).toBeVisible()
    await expect(viewerPage.getByRole('link', { name: 'Students' })).toHaveCount(0)
    await expect(viewerPage.getByRole('link', { name: 'Team & Access' })).toHaveCount(0)

    const viewerToken = await viewerPage.evaluate(() => sessionStorage.getItem('pci.admin.token'))
    expect(viewerToken).toBeTruthy()
    const forbiddenApi = await request.get('/api/admin/members', {
      headers: { Authorization: `Bearer ${viewerToken}` },
    })
    expect(forbiddenApi.status()).toBe(403)
    await viewerPage.goto('/admin/students')
    await expect(viewerPage.getByText('You do not have permission to view this section.')).toBeVisible()
    await captureStoryEvidence(viewerPage, testInfo, 'G18', 'viewer-access-denied')

    const auditResponse = await request.get('/api/admin/audit', {
      headers: { Authorization: `Bearer ${ownerToken}` },
    })
    expect(auditResponse.ok()).toBeTruthy()
    const audit = ((await auditResponse.json()) as {
      rows: Array<{ action?: string; details?: string; actor?: string }>
    }).rows
    expect(audit).toEqual(expect.arrayContaining([
      expect.objectContaining({ action: 'admin_created', details: expect.stringContaining(email), actor: E2E_ADMIN.email }),
    ]))
  })

  test('a scoped admin completes TOTP login and recovery without allowing factor downgrade', async ({ page, request }, testInfo) => {
    const ownerToken = await apiLoginAsE2EAdmin(request)
    const email = uniqueEmail('admin-totp')
    const password = 'AdminTotpBrowser-2026!'
    const createResponse = await request.post('/api/admin/team', {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: {
        email,
        name: 'Browser MFA Admin',
        role: 'custom',
        permissions: ['settings'],
        password,
        force_password_change: false,
      },
    })
    expect(createResponse.ok()).toBeTruthy()

    await page.goto('/admin/login')
    await page.getByLabel('Email address').fill(email)
    await page.getByLabel('Password', { exact: true }).fill(password)
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.getByText('Browser MFA Admin')).toBeVisible({ timeout: 30_000 })
    await page.goto('/admin/settings')
    await expect(page.getByRole('heading', { name: 'Settings' })).toBeVisible({ timeout: 30_000 })

    const setupResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/admin/me/2fa/setup') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Enable 2FA' }).click()
    const setupResponse = await setupResponsePromise
    expect(setupResponse.ok()).toBeTruthy()
    const setup = (await setupResponse.json()) as { secret: string }
    await expect(page.getByText(setup.secret, { exact: true })).toBeVisible()

    const verifyResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/admin/me/2fa/verify') && response.request().method() === 'POST')
    await page.getByLabel('Authentication code').fill(totpCode(setup.secret))
    await page.getByRole('button', { name: 'Verify & enable' }).click()
    const verifyResponse = await verifyResponsePromise
    expect(verifyResponse.ok()).toBeTruthy()
    const verified = (await verifyResponse.json()) as { recovery_codes: string[] }
    expect(verified.recovery_codes).toHaveLength(10)
    await expect(page.getByText('2FA enabled — you will be asked for an authentication code at every sign-in.')).toBeVisible()
    await expect(page.getByText('Enabled', { exact: true })).toBeVisible()

    const adminToken = await page.evaluate(() => sessionStorage.getItem('pci.admin.token'))
    expect(adminToken).toBeTruthy()
    const downgradeAttempt = await request.post('/api/admin/me/2fa/setup', {
      headers: { Authorization: `Bearer ${adminToken}` },
      data: {},
    })
    expect(downgradeAttempt.status()).toBe(409)
    expect(await downgradeAttempt.json()).toMatchObject({ error: 'already_enabled' })
    const stillEnabled = await request.get('/api/admin/me/2fa', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(await stillEnabled.json()).toMatchObject({ enabled: true, pending: false, recovery_remaining: 10 })

    await page.getByRole('button', { name: 'Sign out', exact: true }).click()
    await page.getByLabel('Email address').fill(email)
    await page.getByLabel('Password').fill(password)
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.getByLabel('Authentication code')).toBeVisible()
    await page.getByLabel('Authentication code').fill('000000')
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.getByRole('alert')).toContainText('not valid')
    await page.getByLabel('Authentication code').fill(totpCode(setup.secret))
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.getByText('Browser MFA Admin')).toBeVisible()

    await page.goto('/admin/settings')
    await page.getByText('Disable 2FA', { exact: true }).click()
    await page.getByLabel('Current authentication or recovery code').fill(verified.recovery_codes[0])
    await page.getByRole('button', { name: 'Disable 2FA' }).click()
    await expect(page.getByText('2FA disabled for your account.')).toBeVisible()
    await expect(page.getByRole('button', { name: 'Enable 2FA' })).toBeVisible()
    await captureStoryEvidence(page, testInfo, 'G1', 'admin-totp-lifecycle')
  })

  test('owner changes a student-panel setting and the audit attributes the mutation', async ({ page, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request, page)
    const beforeResponse = await request.get('/api/admin/settings', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(beforeResponse.ok()).toBeTruthy()
    const before = (await beforeResponse.json()) as Record<string, string>
    const original = before.sp_banner_text ?? ''
    const value = `Browser settings round trip ${Date.now()}`

    await page.goto('/admin/settings')
    await expect(page.getByRole('heading', { name: 'Settings' })).toBeVisible({ timeout: 30_000 })
    const setting = page.getByText('Banner text', { exact: true }).locator('..').locator('input')
    await expect(setting).toBeVisible({ timeout: 15_000 })
    await setting.fill(value)
    const saveResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/admin/settings') && response.request().method() === 'PATCH')
    await page.getByRole('button', { name: /Save changes/ }).click()
    expect((await saveResponsePromise).ok()).toBeTruthy()
    await expect(page.getByRole('status')).toContainText('Settings saved')

    const afterResponse = await request.get('/api/admin/settings', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect((await afterResponse.json()) as Record<string, string>).toMatchObject({ sp_banner_text: value })
    const auditResponse = await request.get('/api/admin/audit', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    const audit = ((await auditResponse.json()) as {
      rows: Array<{ action?: string; details?: string; actor?: string }>
    }).rows
    expect(audit).toEqual(expect.arrayContaining([
      expect.objectContaining({ action: 'settings_update', details: expect.stringContaining('sp_banner_text'), actor: E2E_ADMIN.email }),
    ]))
    await captureStoryEvidence(page, testInfo, 'G17-G19', 'settings-and-audit-attribution')

    // Leave the shared browser database in its original state for subsequent specs and retries.
    const restore = await request.patch('/api/admin/settings', {
      headers: { Authorization: `Bearer ${adminToken}` },
      data: { sp_banner_text: original },
    })
    expect(restore.ok()).toBeTruthy()
  })
})
