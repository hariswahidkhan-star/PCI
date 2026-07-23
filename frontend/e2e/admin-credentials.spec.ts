import { readFileSync } from 'node:fs'
import { test, expect } from '@playwright/test'
import {
  E2E_ADMIN,
  apiLoginAsE2EAdmin,
  captureStoryEvidence,
  registerStudent,
} from './util'

const customCertificate = Buffer.from(
  '%PDF-1.4\n% PCI BROWSER CUSTOM CERTIFICATE\n1 0 obj<</Type/Catalog>>endobj\ntrailer<</Root 1 0 R>>\n%%EOF\n',
)

test.describe('credential operator to holder journey', () => {
  test('admin links, uploads, revokes and reinstates a credential the holder can download and verify', async ({ page, context, request }, testInfo) => {
    const studentPage = await context.newPage()
    const student = await registerStudent(request, studentPage, 'credential-holder')
    const adminToken = await apiLoginAsE2EAdmin(request, page)
    const credentialId = `PCI-PMLAI-E2E-${Date.now()}`
    const expires = new Date(Date.now() + 365 * 24 * 60 * 60 * 1000).toISOString().slice(0, 10)

    await page.goto('/admin/credentials')
    await page.getByRole('button', { name: 'Issue credential' }).click()
    const drawer = page.locator('.drawer')
    await drawer.getByLabel('Credential ID').fill(credentialId)
    await drawer.getByLabel('Find student').fill(student.email)
    await expect(drawer.getByLabel('Student account').locator('option')).toHaveCount(2)
    await drawer.getByLabel('Student account').selectOption(String(student.id))
    await expect(drawer.getByLabel('Holder name')).toHaveValue('Browser Student')
    const certSelect = drawer.getByLabel('Certification')
    const pmlOpt = certSelect.locator('option').filter({ hasText: 'PML-AI' }).first()
    await expect(pmlOpt).toHaveCount(1)
    await certSelect.selectOption({ value: await pmlOpt.getAttribute('value') || '' })
    await drawer.getByLabel('Expires').fill(expires)
    const issueResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/admin/credentials') && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: 'Issue credential' }).click()
    expect((await issueResponsePromise).ok()).toBeTruthy()

    await page.getByPlaceholder('Search ID or holder…').fill(credentialId)
    const row = page.getByRole('row').filter({ hasText: credentialId })
    await expect(row).toContainText('Browser Student')
    await expect(row).toContainText('PML-AI')

    const uploadResponsePromise = page.waitForResponse((response) =>
      response.url().includes(`/api/admin/credentials/${credentialId}/upload-certificate`)
      && response.request().method() === 'POST')
    page.once('dialog', async (dialog) => {
      expect(dialog.message()).toContain(`Certificate uploaded for ${credentialId}`)
      await dialog.accept()
    })
    await row.locator('input[type="file"]').setInputFiles({
      name: 'custom-pml-ai-certificate.pdf',
      mimeType: 'application/pdf',
      buffer: customCertificate,
    })
    expect((await uploadResponsePromise).ok()).toBeTruthy()
    await captureStoryEvidence(page, testInfo, 'G6-E6', 'credential-linked-and-custom-certificate-uploaded')

    await studentPage.goto('/app/credentials')
    const holderCard = studentPage.locator('.card').filter({ hasText: credentialId })
    await expect(holderCard).toContainText('PML-AI')
    await expect(holderCard).toContainText('Active')
    const holderDownload = studentPage.waitForEvent('download')
    await holderCard.getByRole('button', { name: 'Download PDF' }).click()
    const downloaded = await holderDownload
    const path = await downloaded.path()
    expect(path).toBeTruthy()
    expect(readFileSync(path!, 'utf8')).toContain('PCI BROWSER CUSTOM CERTIFICATE')

    const publicBefore = await request.get(`/api/verify?id=${encodeURIComponent(credentialId)}`)
    expect(publicBefore.ok()).toBeTruthy()
    expect(await publicBefore.json()).toMatchObject({
      found: true,
      valid: true,
      state: 'active',
      holder_name: 'Browser Student',
      certification_code: 'PML-AI',
    })
    await captureStoryEvidence(studentPage, testInfo, 'E6', 'holder-downloads-custom-certificate')

    page.once('dialog', (dialog) => dialog.accept())
    await row.getByRole('button', { name: 'Revoke' }).click()
    await expect(row.getByRole('button', { name: 'Reinstate' })).toBeVisible()
    await studentPage.reload()
    await expect(holderCard).toContainText('Revoked')
    await expect(holderCard.getByRole('button', { name: 'Download PDF' })).toHaveCount(0)
    const publicRevoked = await request.get(`/api/verify?id=${encodeURIComponent(credentialId)}`)
    expect(await publicRevoked.json()).toMatchObject({ found: true, valid: false, state: 'revoked' })

    await row.getByRole('button', { name: 'Reinstate' }).click()
    await expect(row.getByRole('button', { name: 'Revoke' })).toBeVisible()
    const publicRestored = await request.get(`/api/verify?id=${encodeURIComponent(credentialId)}`)
    expect(await publicRestored.json()).toMatchObject({ found: true, valid: true, state: 'active' })

    const auditResponse = await request.get('/api/admin/audit', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    const audit = ((await auditResponse.json()) as {
      rows: Array<{ action?: string; details?: string; actor?: string }>
    }).rows
    expect(audit).toEqual(expect.arrayContaining([
      expect.objectContaining({ action: 'credential_issued', details: expect.stringContaining(credentialId), actor: E2E_ADMIN.email }),
      expect.objectContaining({ action: 'certificate_uploaded', details: expect.stringContaining(credentialId), actor: E2E_ADMIN.email }),
    ]))
  })
})
