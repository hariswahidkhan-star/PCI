import { readFileSync } from 'node:fs'
import { test, expect } from '@playwright/test'
import { apiLoginAsE2EAdmin, captureStoryEvidence, createTestUser, uniqueEmail } from './util'

test.describe('student support, session security and privacy journeys', () => {
  test('student ticket reaches the admin queue, receives a reply and becomes an in-app message', async ({ page, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    const user = await createTestUser(request, adminToken, 'ready', page)
    const subject = `Browser support ${uniqueEmail('case').split('@')[0]}`
    const studentBody = 'The scheduling journey needs a support review.'
    const adminBody = 'We reviewed your eligibility and your scheduling access is ready.'

    await page.goto('/app/support')
    await page.getByLabel('Subject').fill(subject)
    await page.getByLabel('Category').selectOption('technical')
    await page.getByLabel('How can we help?').fill(studentBody)
    const createdResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/me/tickets') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Submit request' }).click()
    const created = await createdResponse
    expect(created.ok()).toBeTruthy()
    const ticket = (await created.json()) as { id: number; reference: string }
    await expect(page.getByRole('status')).toContainText(ticket.reference)
    await expect(page.getByText(subject, { exact: true })).toBeVisible()

    const inboxResponse = await request.get('/api/support/inbox', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(inboxResponse.ok()).toBeTruthy()
    const inbox = (await inboxResponse.json()) as { tickets: Array<Record<string, unknown>> }
    expect(inbox.tickets).toEqual(expect.arrayContaining([
      expect.objectContaining({ id: ticket.id, student_email: user.email, is_test: 1 }),
    ]))

    const replyResponse = await request.post(`/api/support/tickets/${ticket.id}/reply`, {
      headers: { Authorization: `Bearer ${adminToken}` },
      data: { body: adminBody },
    })
    expect(replyResponse.ok()).toBeTruthy()

    await page.goto('/app/messages')
    const replyCard = page.locator('.card').filter({ hasText: ticket.reference }).first()
    await expect(replyCard).toContainText('Support replied to your ticket')
    await expect(replyCard).toContainText(ticket.reference)
    await captureStoryEvidence(page, testInfo, 'F3', 'support-notification')
    const markAll = page.getByRole('button', { name: 'Mark all read' })
    await expect(markAll).toBeVisible()
    await markAll.click()
    await expect(markAll).toHaveCount(0)

    await page.goto('/app/support')
    await page.getByRole('button', { name: new RegExp(subject) }).click()
    await expect(page.getByText(adminBody, { exact: true })).toBeVisible()
    await page.getByPlaceholder('Write a reply…').fill('Thank you — confirmed from the student portal.')
    const studentReply = page.waitForResponse((response) =>
      response.url().endsWith(`/api/me/tickets/${ticket.id}/reply`) && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Reply', exact: true }).click()
    expect((await studentReply).ok()).toBeTruthy()
    await expect(page.getByText('Thank you — confirmed from the student portal.', { exact: true })).toBeVisible()
    await captureStoryEvidence(page, testInfo, 'F1', 'two-way-ticket-thread')
  })

  test('student exports account data, revokes another session and submits an erasure request', async ({ page, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    const user = await createTestUser(request, adminToken, 'ready', page)

    // Create a genuinely separate login so the session-revocation control has something to revoke.
    const secondLoginResponse = await request.post('/api/login', {
      data: { email: user.email, password: user.password },
    })
    expect(secondLoginResponse.ok()).toBeTruthy()
    const secondLogin = (await secondLoginResponse.json()) as { token: string }
    expect(secondLogin.token).toBeTruthy()

    await page.goto('/app/profile')
    await expect(page.getByRole('heading', { name: 'Account security & privacy' })).toBeVisible()

    const downloadPromise = page.waitForEvent('download')
    await page.getByRole('button', { name: 'Export my account data' }).click()
    const download = await downloadPromise
    expect(download.suggestedFilename()).toMatch(/^pci-account-data-\d{4}-\d{2}-\d{2}\.json$/)
    const path = await download.path()
    expect(path).toBeTruthy()
    const exported = JSON.parse(readFileSync(path!, 'utf8')) as { user?: { email?: string }; payments?: unknown[] }
    expect(exported.user?.email).toBe(user.email)
    expect(exported.payments?.length).toBeGreaterThan(0)
    await expect(page.getByRole('status')).toContainText('account-data export has been downloaded')

    const revokeResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/me/sessions/revoke-others') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Sign out other sessions' }).click()
    expect((await revokeResponse).ok()).toBeTruthy()
    await expect(page.getByRole('status')).toContainText('Other sessions have been signed out')

    const revokedProbe = await request.get('/api/me', {
      headers: { Authorization: `Bearer ${secondLogin.token}` },
    })
    expect(revokedProbe.status(), 'the other device token should no longer authenticate').toBe(401)
    const currentProbe = await page.evaluate(async () => (await fetch('/api/me', {
      headers: { Authorization: `Bearer ${sessionStorage.getItem('pci.session.token') ?? ''}` },
    })).status)
    expect(currentProbe, 'the session that requested the revocation must remain active').toBe(200)
    await captureStoryEvidence(page, testInfo, 'F5', 'other-sessions-revoked')

    const reason = 'End-to-end privacy rights verification.'
    await page.getByLabel('Deletion request reason (optional)').fill(reason)
    page.once('dialog', (dialog) => dialog.accept())
    const deletionResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/me/delete-request') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Request account deletion' }).click()
    expect((await deletionResponse).ok()).toBeTruthy()
    await expect(page.getByRole('status')).toContainText('within 30 days')
    await captureStoryEvidence(page, testInfo, 'F4', 'erasure-requested')

    const erasuresResponse = await request.get('/api/admin/erasure-requests?status=pending', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(erasuresResponse.ok()).toBeTruthy()
    const erasures = (await erasuresResponse.json()) as { rows: Array<Record<string, unknown>> }
    expect(erasures.rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ user_id: user.id, email: user.email, reason, status: 'pending' }),
    ]))
  })
})
