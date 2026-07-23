import { test, expect } from '@playwright/test'
import {
  E2E_ADMIN,
  apiLoginAsE2EAdmin,
  captureStoryEvidence,
  registerStudent,
} from './util'

test.describe('admin student operations', () => {
  test('support view is read-only and finance actions reconcile, reverse and attribute the actor', async ({ page, context, request }, testInfo) => {
    test.slow()
    const student = await registerStudent(request, undefined, 'admin-ops')
    const adminToken = await apiLoginAsE2EAdmin(request, page)
    const supportReason = `E2E support ticket ${Date.now()}`
    const gatewayReference = `BANK-E2E-${Date.now()}`

    await page.goto('/admin/students')
    await page.getByPlaceholder('Search name or email…').fill(student.email)
    const studentRow = page.locator('tbody tr').filter({ hasText: student.email })
    await expect(studentRow).toBeVisible()
    await studentRow.click()

    const drawer = page.locator('.drawer')
    await expect(drawer.getByRole('heading', { name: 'Student detail' })).toBeVisible()
    await expect(drawer).toContainText(student.email)
    await expect(drawer.getByRole('heading', { name: 'Student journey' })).toBeVisible()
    await expect(drawer.getByRole('heading', { name: 'Security & access' })).toBeVisible()

    await drawer.getByRole('button', { name: 'View as student' }).click()
    await drawer.getByRole('button', { name: 'Confirm' }).click()
    await expect(drawer.getByRole('alert')).toContainText('A reason is required')
    await drawer.getByPlaceholder('Reason (e.g. ticket reference)').fill(supportReason)
    const startResponsePromise = page.waitForResponse((response) =>
      response.url().includes(`/api/admin/members/${student.id}/impersonate`)
      && !response.url().endsWith('/end')
      && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: 'Confirm' }).click()
    expect((await startResponsePromise).ok()).toBeTruthy()

    const supportPagePromise = context.waitForEvent('page')
    await drawer.getByRole('link', { name: 'Open student portal ↗' }).click()
    const supportPage = await supportPagePromise
    await expect(supportPage.locator('.impersonation-banner')).toContainText('Support view')
    await expect(supportPage.locator('.impersonation-banner')).toContainText('Actions that belong to the student are disabled')
    await expect.poll(
      () => supportPage.evaluate(() => sessionStorage.getItem('pci.session.token')),
    ).not.toBeNull()
    const supportToken = await supportPage.evaluate(() => sessionStorage.getItem('pci.session.token'))
    expect(supportToken).toBeTruthy()

    const forbiddenConsent = await request.post('/api/me/consents', {
      headers: { Authorization: `Bearer ${supportToken}` },
      data: { accept_all: true },
    })
    expect(forbiddenConsent.status()).toBe(403)
    expect(await forbiddenConsent.json()).toMatchObject({ error: 'impersonation_readonly' })
    await captureStoryEvidence(supportPage, testInfo, 'D9-G3', 'permanent-read-only-support-view')

    const endResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith(`/api/admin/members/${student.id}/impersonate/end`)
      && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: 'End session' }).click()
    expect((await endResponsePromise).ok()).toBeTruthy()
    await expect(drawer.getByRole('status')).toContainText('Impersonation sessions ended')
    const endedSession = await request.get('/api/me', {
      headers: { Authorization: `Bearer ${supportToken}` },
    })
    expect(endedSession.status()).toBe(401)

    await drawer.getByRole('button', { name: 'Impersonation history' }).click()
    const history = drawer.getByRole('table').filter({ hasText: supportReason })
    await expect(history).toContainText(E2E_ADMIN.email)
    await expect(history).toContainText(supportReason)
    await expect(history).not.toContainText('active')

    const markPaid = drawer.locator('.card').filter({ hasText: 'Mark as paid / waive fee' }).first()
    await expect(markPaid.getByRole('heading', { name: 'Mark as paid / waive fee' })).toBeVisible()
    const markSelects = markPaid.locator('select')
    await markSelects.nth(0).selectOption('membership')
    await markPaid.locator('input[type="number"]').fill('149')
    await markPaid.locator('input[placeholder="e.g. bank transfer received"]').fill('E2E bank reconciliation')
    await markPaid.getByText('Payment evidence (optional)').click()
    await markPaid.locator('label').filter({ hasText: 'Method' }).locator('select').selectOption('bank_transfer')
    await markPaid.locator('label').filter({ hasText: 'Bank reference' }).locator('input').fill(gatewayReference)
    await markPaid.locator('label').filter({ hasText: 'Gateway reference' }).locator('input').fill(gatewayReference)
    await markPaid.locator('label').filter({ hasText: 'Receipt no.' }).locator('input').fill(`RCT-${Date.now()}`)
    const paidResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith(`/api/admin/students/${student.id}/mark-paid`)
      && response.request().method() === 'POST')
    await markPaid.getByRole('button', { name: 'Mark paid' }).click()
    const paidResponse = await paidResponsePromise
    expect(paidResponse.ok()).toBeTruthy()
    const paid = (await paidResponse.json()) as { payment_id: number }
    await expect(markPaid).toContainText('Recorded')

    const waive = drawer.locator('.card').filter({ hasText: 'Waive fee' }).filter({ hasNotText: 'Mark as paid' }).first()
    await expect(waive.getByRole('heading', { name: 'Waive fee', exact: true })).toBeVisible()
    await waive.locator('select').first().selectOption('exam')
    await waive.locator('input[type="number"]').fill('100')
    await waive.locator('input[placeholder="scholarship / sponsorship / promotion…"]').fill('E2E scholarship approval')
    const waiverResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith(`/api/admin/students/${student.id}/waive`)
      && response.request().method() === 'POST')
    await waive.getByRole('button', { name: 'Waive in full' }).click()
    expect((await waiverResponsePromise).ok()).toBeTruthy()
    await expect(waive).toContainText('Waived in full')
    await captureStoryEvidence(page, testInfo, 'G4', 'student-360-finance-actions')

    await page.goto('/admin/payments')
    await page.getByRole('button', { name: 'Reconciliation' }).click()
    const reconRow = page.getByRole('row').filter({ hasText: student.email }).filter({ hasText: /membership/i })
    await expect(reconRow).toContainText(gatewayReference)
    await expect(reconRow).toContainText(/paid/i)
    await reconRow.getByRole('button', { name: 'Reverse' }).click()
    await reconRow.getByPlaceholder('Reason').fill('E2E reconciliation correction')
    const reverseResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith(`/api/admin/payments/${paid.payment_id}/reverse`)
      && response.request().method() === 'POST')
    await reconRow.getByRole('button', { name: 'Reverse' }).click()
    expect((await reverseResponsePromise).ok()).toBeTruthy()
    await expect(page.getByRole('status')).toContainText(`Payment #${paid.payment_id} reversed`)
    await expect(reconRow).toContainText('refunded')
    await captureStoryEvidence(page, testInfo, 'G4-D4', 'reconciled-payment-reversed')

    const memberResponse = await request.get(`/api/admin/members/${student.id}`, {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(memberResponse.ok()).toBeTruthy()
    expect(await memberResponse.json()).toMatchObject({ membership: { status: 'expired' } })

    const auditResponse = await request.get('/api/admin/audit', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(auditResponse.ok()).toBeTruthy()
    const audit = ((await auditResponse.json()) as {
      rows: Array<{ action?: string; details?: string; actor?: string }>
    }).rows
    for (const action of [
      'impersonation_started',
      'impersonation_ended',
      'admin_mark_paid',
      'fee_waiver_full',
      'payment_reversed',
    ]) {
      expect(audit).toEqual(expect.arrayContaining([
        expect.objectContaining({
          action,
          actor: E2E_ADMIN.email,
          details: expect.stringContaining(`subject ${student.id}`),
        }),
      ]))
    }
  })
})
