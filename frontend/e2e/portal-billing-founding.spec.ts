import { test, expect } from '@playwright/test'
import { apiLoginAsOwnerAdmin, apiRegisterStudent, certificationId, seedStudentSession, storyScreenshot } from './util'

test.describe('portal billing and certification-scoped codes', () => {
  test('PML-AI-scoped exam discount rejects PCL-AI and previews for PML-AI', async ({ request, page }, testInfo) => {
    const ownerToken = await apiLoginAsOwnerAdmin(request)
    const pmlId = await certificationId(request, 'pml-ai')
    const code = `PML${Date.now()}${process.pid}`.toUpperCase()
    const create = await request.post('/api/admin/codes', {
      headers: { Authorization: `Bearer ${ownerToken}` },
      data: {
        code,
        discount_type: 'percent',
        discount_value: 35,
        applies_to: 'exam',
        certification_id: pmlId,
        active: true,
      },
    })
    expect(create.ok(), `discount code creation should succeed (got ${create.status()})`).toBeTruthy()

    const student = await apiRegisterStudent(request, 'billing')
    const pclPreview = await request.post('/api/validate-code', {
      data: { code, product: 'exam', cert: 'pcl-ai', email: student.email },
    })
    expect(pclPreview.ok()).toBeTruthy()
    expect((await pclPreview.json()) as { valid?: boolean }).toMatchObject({ valid: false })

    const pmlPreview = await request.post('/api/validate-code', {
      data: { code, product: 'exam', cert: 'pml-ai', email: student.email },
    })
    expect(pmlPreview.ok()).toBeTruthy()
    expect((await pmlPreview.json()) as { valid?: boolean }).toMatchObject({ valid: true })

    await seedStudentSession(page, student.token)
    await page.route('**/api/create-checkout-session', async (route) => {
      await route.fulfill({
        status: 503,
        contentType: 'application/json',
        body: JSON.stringify({ error: 'payments_not_configured' }),
      })
    })
    await page.goto('/app/billing?cert=pml-ai')
    await expect(page.getByRole('heading', { name: /billing/i })).toBeVisible()
    await page.getByLabel(/certification/i).selectOption('pml-ai')
    await page.getByLabel(/discount code/i).fill(code)
    await page.getByRole('button', { name: /pay exam fee/i }).click()
    await expect(page.getByRole('status')).toContainText(/discount preview/i)
    await expect(page.getByText(/PML-AI/).first()).toBeVisible()
    await storyScreenshot(page, testInfo, 'pml-ai-discount-preview')
  })
})
