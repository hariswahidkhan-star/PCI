import { readFileSync } from 'node:fs'
import { test, expect } from '@playwright/test'
import { demoAnswers, sourceDemoQuestion } from './demo-exam'
import { settleExamPurchase, uniqueEmail } from './util'

const tinyPng = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg=='

test.describe('three-certification isolation', () => {
  test('one browser student keeps PCL-AI, PFL-AI and PML-AI enrolments separate', async ({ page, request }) => {
    const email = uniqueEmail('suite-isolation')
    await page.goto('/app/register')
    await page.getByLabel('First name').fill('Browser')
    await page.getByLabel('Last name').fill('Isolation')
    await page.getByLabel('Email address').fill(email)
    await page.getByLabel('Password (8+ characters)').fill('e2e-pass-123')
    await page.getByLabel('Confirm password').fill('e2e-pass-123')
    await page.locator('#rcountry').selectOption({ label: 'United Kingdom' })
    await page.locator('#rphone').fill('7700900001')
    await page.getByRole('button', { name: /create free account/i }).click()
    await expect(page).toHaveURL(/\/app\/onboarding$/)
    await page.getByRole('link', { name: /skip for now/i }).click()

    // The first settlement is the membership+exam bundle; once membership is active, the remaining
    // two certifications use the normal exam-only product. This mirrors the real checkout policy.
    await settleExamPurchase(request, email, 'PCL-AI', 'bundle')
    for (const code of ['PFL-AI', 'PML-AI'] as const)
      await settleExamPurchase(request, email, code)

    await page.goto('/app/certifications')
    await expect(page.getByRole('heading', { level: 1 })).toBeVisible()
    for (const [code, name] of [
      ['PCL-AI', 'PCI AI Project Controls Leader'],
      ['PFL-AI', 'PCI AI Project Finance Leader'],
      ['PML-AI', 'PCI Project Management Leader – AI'],
    ] as const) {
      await expect(page.getByText(name, { exact: true }).first()).toBeVisible()
      await expect(page.getByText(code, { exact: true }).first()).toBeVisible()
    }
    await expect(page.getByText('PCI AI Project Delivery Leader', { exact: true })).toHaveCount(0)

    // Browser-created session + browser-triggered settlements, then a real authenticated read: the
    // three UI cards must be backed by three distinct certification_id relationships, not one row
    // whose label was overwritten three times.
    const token = await page.evaluate(() => sessionStorage.getItem('pci.session.token'))
    expect(token).toBeTruthy()
    const meResponse = await request.get('/api/me', { headers: { Authorization: `Bearer ${token}` } })
    expect(meResponse.ok()).toBeTruthy()
    const me = (await meResponse.json()) as { exams?: Array<{ certification_id?: number; certification_code?: string }> }
    const rows = (me.exams ?? []).map((row) => [row.certification_id, row.certification_code] as const)
    expect(new Set(rows.map(([id]) => id)).size).toBe(3)
    expect(new Set(rows.map(([, code]) => code))).toEqual(new Set(['PCL-AI', 'PFL-AI', 'PML-AI']))

    const headers = { Authorization: `Bearer ${token}` }
    for (const response of [
      await request.post('/api/me/consents', { headers, data: { accept_all: true } }),
      await request.patch('/api/me/profile', { headers, data: { country: 'United Kingdom', city: 'London' } }),
      await request.post('/api/me/identity-document', { headers, data: { doc_kind: 'passport', filename: 'id.png', data_uri: tinyPng } }),
    ]) expect(response.ok()).toBeTruthy()

    // Complete one isolated sitting per certification. Each demo bank is scoped to its certification
    // id, and each issued credential must keep the matching prefix and metadata all the way to the UI.
    const prefixes = { 'PCL-AI': 'PCI-PCLAI-', 'PFL-AI': 'PCI-PFLAI-', 'PML-AI': 'PCI-PMLAI-' }
    const issued: string[] = []
    for (const [certificationId, code] of rows) {
      expect(certificationId).toBeTruthy()
      expect(code && code in prefixes).toBeTruthy()
      const certCode = code as keyof typeof prefixes
      const booked = await request.post('/api/me/exam/book', {
        headers,
        data: { certification_id: certificationId, scheduled_at: new Date(Date.now() + 30 * 3_600_000).toISOString(), timezone: 'UTC' },
      })
      expect(booked.ok(), `${code} booking should succeed`).toBeTruthy()
      const readiness = await request.post('/api/me/readiness', {
        headers,
        data: { camera: true, microphone: true, network: true },
      })
      expect(readiness.ok(), `${code} readiness should succeed`).toBeTruthy()
      const started = await request.post('/api/me/exam/start', { headers, data: { certification_id: certificationId } })
      expect(started.ok(), `${code} start should succeed`).toBeTruthy()
      const start = (await started.json()) as { attempt_id: number; items: Array<{ id: number; question: string }> }
      const answerPayload: Record<string, number> = {}
      for (const item of start.items) {
        const sourceQuestion = sourceDemoQuestion(item.question)
        const answer = demoAnswers.get(sourceQuestion)
        expect(answer, `${code} answer should exist for: ${sourceQuestion}`).toBeDefined()
        answerPayload[String(item.id)] = answer!
      }
      const submitted = await request.post('/api/me/exam/submit', {
        headers,
        data: { attempt_id: start.attempt_id, answers: answerPayload },
      })
      expect(submitted.ok(), `${code} submit should succeed`).toBeTruthy()
      const result = (await submitted.json()) as { result?: string; credential?: string }
      expect(result.result).toBe('pass')
      expect(result.credential?.startsWith(prefixes[certCode])).toBeTruthy()
      issued.push(result.credential!)
    }
    expect(new Set(issued).size).toBe(3)

    await page.goto('/app/credentials')
    for (const code of ['PCL-AI', 'PFL-AI', 'PML-AI'])
      await expect(page.getByText(code, { exact: true })).toBeVisible()
    for (const credential of issued)
      await expect(page.getByText(credential, { exact: true })).toBeVisible()

    await page.goto('/app/documents')
    const pmlMaterials = page.locator('.card').filter({ hasText: 'PCI PML-AI' })
    await expect(pmlMaterials).toContainText('PCI PML-AI Body of Knowledge')
    const materialDownload = page.waitForEvent('download')
    await pmlMaterials.getByRole('button', { name: 'Download', exact: true }).click()
    const material = await materialDownload
    expect(material.suggestedFilename()).toBe('pml-ai-bok.pdf')
    const materialPath = await material.path()
    expect(materialPath).toBeTruthy()
    expect(readFileSync(materialPath!).subarray(0, 4).toString()).toBe('%PDF')

    // The legacy exam portal used to hard-code PCL-AI on every certificate. Its credential picker and
    // selected certificate must now bind the requested PML-AI record end-to-end.
    await page.goto(`/student.html#t=${token}`)
    await expect(page.locator('#app')).toBeVisible()
    await page.locator('#nav a[data-id="cert"]').click()
    for (const name of [
      'PCI AI Project Controls Leader',
      'PCI AI Project Finance Leader',
      'PCI Project Management Leader – AI',
    ]) await expect(page.getByText(name, { exact: true })).toBeVisible()
    await page.locator('.srow').filter({ hasText: 'PML-AI' }).getByRole('button', { name: 'View certificate' }).click()
    await expect(page.locator('#certWrap')).toContainText('PCI Project Management Leader – AI (PCI PML-AI)')
    await expect(page.locator('#certWrap')).not.toContainText('PCI AI Project Controls Leader')
  })
})
