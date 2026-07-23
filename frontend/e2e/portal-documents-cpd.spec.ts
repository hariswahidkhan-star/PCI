import { readFileSync } from 'node:fs'
import { test, expect } from '@playwright/test'
import { apiLoginAsE2EAdmin, captureStoryEvidence, createTestUser } from './util'

const minimalPdf = Buffer.from('%PDF-1.4\n1 0 obj<</Type/Catalog>>endobj\ntrailer<</Root 1 0 R>>\n%%EOF\n')

test.describe('student records handed off to PCI operators', () => {
  test('assigned document is acknowledged, downloaded and visible in the admin audit', async ({ page, context, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    const student = await createTestUser(request, adminToken, 'ready', page)
    const title = `Browser acknowledgement ${Date.now()}`
    const createResponse = await request.post(`/api/admin/students/${student.id}/documents`, {
      headers: { Authorization: `Bearer ${adminToken}` },
      data: {
        title,
        description: 'A deterministic private document for the complete acknowledgement journey.',
        category: 'Candidate policies',
        doc_type: 'policy',
        filename: 'candidate-policy.pdf',
        file: `data:application/pdf;base64,${minimalPdf.toString('base64')}`,
        ack_required: true,
        publish: true,
      },
    })
    expect(createResponse.ok()).toBeTruthy()
    const created = (await createResponse.json()) as { id: number; status?: string }
    expect(created.id).toBeGreaterThan(0)

    await page.goto('/app/documents')
    await expect(page.getByRole('heading', { name: 'My Documents' })).toBeVisible()
    const documentRow = page.locator('.rep-item').filter({ hasText: title })
    await expect(documentRow).toContainText('Acknowledgement required before download.')
    await expect(documentRow.getByRole('button', { name: 'Download' })).toHaveCount(0)

    const acknowledgeResponse = page.waitForResponse((response) =>
      response.url().endsWith(`/api/me/documents/${created.id}/acknowledge`) && response.request().method() === 'POST')
    await documentRow.getByRole('button', { name: 'Acknowledge' }).click()
    expect((await acknowledgeResponse).ok()).toBeTruthy()
    await expect(documentRow).toContainText('Acknowledged')

    const downloadPromise = page.waitForEvent('download')
    await documentRow.getByRole('button', { name: 'Download' }).click()
    const download = await downloadPromise
    expect(download.suggestedFilename()).toBe('candidate-policy.pdf')
    const path = await download.path()
    expect(path).toBeTruthy()
    expect(readFileSync(path!).subarray(0, 4).toString()).toBe('%PDF')
    await captureStoryEvidence(page, testInfo, 'E4', 'document-acknowledged-and-downloaded')

    const auditResponse = await request.get(`/api/admin/documents/${created.id}/audit`, {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(auditResponse.ok()).toBeTruthy()
    const audit = (await auditResponse.json()) as {
      summary: { acknowledgements: number; total_downloads: number; distinct_downloaders: number }
      recent: Array<{ actor?: string; role?: string; action?: string; result?: string }>
    }
    expect(audit.summary).toMatchObject({ acknowledgements: 1, total_downloads: 1, distinct_downloaders: 1 })
    expect(audit.recent).toEqual(expect.arrayContaining([
      expect.objectContaining({ actor: student.email, role: 'student', action: 'download', result: 'ok' }),
    ]))

    const adminPage = await context.newPage()
    await apiLoginAsE2EAdmin(request, adminPage)
    await adminPage.goto('/admin/documents')
    await adminPage.getByLabel('Search').fill(title)
    // Document rows use role="button" on <tr>, so they are not exposed as ARIA rows.
    const adminRow = adminPage.locator('tbody tr').filter({ hasText: title }).first()
    await expect(adminRow).toBeVisible({ timeout: 15_000 })
    await adminRow.click()
    await expect(adminPage.getByText('Recipients (1)')).toBeVisible()
    await expect(adminPage.getByText(student.email, { exact: true }).first()).toBeVisible()
    await expect(adminPage.getByText('Acknowledgements')).toBeVisible()
    await expect(adminPage.getByText('Total downloads')).toBeVisible()
    await captureStoryEvidence(adminPage, testInfo, 'E4-G17', 'document-admin-audit')
  })

  test('student CPD submission is approved in the admin UI and updates the member total', async ({ page, context, request }, testInfo) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    await createTestUser(request, adminToken, 'ready', page)
    const activity = `AI assurance workshop ${Date.now()}`

    await page.goto('/app/cpd')
    await expect(page.getByRole('heading', { name: 'Continuing professional development' })).toBeVisible()
    await page.getByLabel('Activity').fill(activity)
    await page.getByLabel('Category').selectOption({ label: 'AI currency' })
    await page.getByLabel('Hours').fill('2.5')
    await page.getByLabel('Date').fill(new Date().toISOString().slice(0, 10))
    const submitResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/me/cpd') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Add entry' }).click()
    expect((await submitResponse).ok()).toBeTruthy()
    const studentRow = page.getByRole('row').filter({ hasText: activity })
    await expect(studentRow).toContainText(/recorded/i)

    const adminPage = await context.newPage()
    await apiLoginAsE2EAdmin(request, adminPage)
    await adminPage.goto('/admin/cpd')
    await expect(adminPage.getByRole('heading', { name: 'CPD review' })).toBeVisible()
    const adminRow = adminPage.getByRole('row').filter({ hasText: activity })
    await expect(adminRow).toBeVisible()
    await adminRow.getByPlaceholder('Review note (optional)').fill('Evidence and category accepted by browser operator.')
    const approveResponse = adminPage.waitForResponse((response) =>
      /\/api\/admin\/cpd\/\d+\/review$/.test(new URL(response.url()).pathname) && response.request().method() === 'POST')
    await adminRow.getByRole('button', { name: 'Approve' }).click()
    expect((await approveResponse).ok()).toBeTruthy()
    await expect(adminPage.getByRole('status')).toContainText('marked approved')
    await captureStoryEvidence(adminPage, testInfo, 'F2', 'cpd-approved-by-admin')

    await page.reload()
    const approvedRow = page.getByRole('row').filter({ hasText: activity })
    await expect(approvedRow).toContainText(/approved/i)
    // Seeded sp_cpd_target_hours is 30 (frontend only falls back to 60 when /api/me has not loaded).
    await expect(page.getByText(/2\.5 of 30 hours/i)).toBeVisible({ timeout: 15_000 })
    await page.getByLabel('Your CPD position').selectOption('compliant')
    await page.getByLabel('Notes (optional)').fill('All required CPD is being tracked through the member portal.')
    await page.getByRole('button', { name: 'Submit declaration' }).click()
    const year = new Date().getUTCFullYear()
    await expect(page.getByText(new RegExp(`You have declared your CPD position for ${year}`))).toBeVisible({ timeout: 15_000 })
    await captureStoryEvidence(page, testInfo, 'F2', 'cpd-member-total-and-declaration')
  })
})
