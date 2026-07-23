import { test, expect } from '@playwright/test'
import {
  E2E_ADMIN,
  apiLoginAsE2EAdmin,
  captureStoryEvidence,
  registerStudent,
} from './util'

const minimalPdf = Buffer.from('%PDF-1.4\n1 0 obj<</Type/Catalog>>endobj\n%%EOF\n')

test.describe('billing, discount and founding journeys', () => {
  test('admin creates scoped and founding codes, then the student previews and completes board approval', async ({ page, context, request }, testInfo) => {
    test.slow()
    const adminToken = await apiLoginAsE2EAdmin(request, page)
    const discountCode = `PML${Date.now()}`
    const foundingCode = `FOUND${Date.now()}`
    const closes = new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().slice(0, 10)

    await page.goto('/admin/codes')
    await page.getByRole('button', { name: 'New code' }).click()
    let drawer = page.locator('.drawer')
    await drawer.getByPlaceholder('LAUNCH20').fill(discountCode)
    await drawer.locator('.field').filter({ hasText: 'Value (%)' }).locator('input').fill('25')
    await drawer.locator('.field').filter({ hasText: 'Applies to' }).locator('select').selectOption('exam')
    const certificationSelect = drawer.locator('.field').filter({ hasText: 'Certification' }).locator('select')
    await expect(certificationSelect.locator('option', { hasText: 'PML-AI' })).toHaveCount(1)
    const pmlOption = certificationSelect.locator('option').filter({ hasText: 'PML-AI' }).first()
    await expect(pmlOption).toHaveCount(1)
    await certificationSelect.selectOption({ value: await pmlOption.getAttribute('value') || '' })
    await drawer.getByLabel('One use per email').check()
    const discountCreatePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/admin/codes') && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: 'Create code' }).click()
    expect((await discountCreatePromise).ok()).toBeTruthy()
    const discountRow = page.getByRole('row').filter({ hasText: discountCode })
    await expect(discountRow).toContainText('25% on exam fee')
    await expect(discountRow).toContainText('active')

    await page.getByRole('button', { name: 'New code' }).click()
    drawer = page.locator('.drawer')
    await drawer.locator('.field').filter({ hasText: 'Kind' }).locator('select').selectOption('founding')
    await drawer.getByPlaceholder('FOUNDING2026').fill(foundingCode)
    await drawer.locator('.field').filter({ hasText: 'Window closes' }).locator('input').fill(closes)
    await drawer.locator('.field').filter({ hasText: 'Max total uses' }).locator('input').fill('2')
    await drawer.getByLabel(/Require an application/).check()
    // Nested founding criteria card — click the label text (Playwright setChecked fights the controlled input).
    const autoApprove = drawer.locator('label').filter({ hasText: /Auto-approve applications that meet the criteria/ })
    await expect(autoApprove.locator('input[type="checkbox"]')).toBeChecked()
    await autoApprove.click()
    await expect(autoApprove.locator('input[type="checkbox"]')).not.toBeChecked()
    await drawer.locator('.field').filter({ hasText: 'Minimum experience (years)' }).locator('input').fill('5')
    const foundingCreatePromise = page.waitForResponse((response) =>
      response.url().endsWith('/api/admin/codes') && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: 'Create code' }).click()
    expect((await foundingCreatePromise).ok()).toBeTruthy()
    const foundingRow = page.getByRole('row').filter({ hasText: foundingCode })
    await expect(foundingRow).toContainText('founding · application')
    await expect(foundingRow).toContainText('membership + exam + study at USD 0')
    await captureStoryEvidence(page, testInfo, 'G10', 'scoped-discount-and-founding-codes-created')

    const studentPage = await context.newPage()
    const student = await registerStudent(request, studentPage, 'billing-codes')
    await studentPage.goto('/app/billing')
    const plansCard = studentPage.locator('.card').filter({ has: studentPage.getByRole('heading', { name: 'Plans & pricing' }) })
    await plansCard.getByLabel('Discount or founding code').fill(discountCode)
    await plansCard.getByRole('button', { name: 'Check code' }).click()
    await expect(plansCard.getByRole('status')).toContainText('only valid for PML-AI')

    await plansCard.getByLabel('Certification').selectOption('PML-AI')
    await plansCard.getByRole('button', { name: 'Check code' }).click()
    const preview = plansCard.getByRole('status')
    await expect(preview).toContainText(`Code ${discountCode} applies to the exam fee`)
    await expect(preview).toContainText('Saves')
    await expect(preview).toContainText('preview total')
    await captureStoryEvidence(studentPage, testInfo, 'C4', 'certification-scoped-code-preview')

    const foundingCard = studentPage.locator('.card').filter({ has: studentPage.getByRole('heading', { name: 'Founding access' }) })
    await foundingCard.getByLabel('Founding code').fill(foundingCode)
    const validateFoundingPromise = studentPage.waitForResponse((response) =>
      response.url().endsWith('/api/founding/validate') && response.request().method() === 'POST')
    await foundingCard.getByRole('button', { name: 'Check code', exact: true }).click()
    expect((await validateFoundingPromise).ok()).toBeTruthy()
    await expect(foundingCard.getByText('Founding access — fees waived.')).toBeVisible()
    await foundingCard.getByLabel('Years of experience').fill('10')
    await foundingCard.getByLabel('Current role').fill('Project controls lead')
    await foundingCard.getByLabel('Highest qualification').fill('BSc Engineering')
    await foundingCard.getByLabel(/Evidence/).setInputFiles({
      name: 'founding-evidence.pdf',
      mimeType: 'application/pdf',
      buffer: minimalPdf,
    })
    const applyPromise = studentPage.waitForResponse((response) =>
      response.url().endsWith('/api/me/founding-application') && response.request().method() === 'POST')
    await foundingCard.getByRole('button', { name: 'Submit founding application' }).click()
    expect((await applyPromise).ok()).toBeTruthy()
    await expect(foundingCard.getByRole('status')).toContainText('under review')
    await expect(foundingCard.getByText(/evidence submitted: founding-evidence\.pdf/)).toBeVisible()
    await captureStoryEvidence(studentPage, testInfo, 'C5', 'founding-application-under-review')

    await page.goto('/admin/founding')
    const applicationRow = page.getByRole('row').filter({ hasText: student.email })
    await expect(applicationRow).toContainText(foundingCode)
    await expect(applicationRow).toContainText('10 yrs')
    await expect(applicationRow.getByRole('button', { name: 'Evidence' })).toBeVisible()
    await page.getByLabel('Decision note (recorded and sent to the applicant)').fill('Evidence verified by the founding board.')
    const approvePromise = page.waitForResponse((response) =>
      response.url().match(/\/api\/admin\/founding-applications\/\d+\/decide$/) !== null
      && response.request().method() === 'POST')
    await applicationRow.getByRole('button', { name: 'Approve' }).click()
    expect((await approvePromise).ok()).toBeTruthy()
    await page.getByLabel('Status filter').selectOption('approved')
    const approvedRow = page.getByRole('row').filter({ hasText: student.email })
    await expect(approvedRow).toContainText('approved')
    await expect(approvedRow).toContainText('Evidence verified by the founding board.')
    await captureStoryEvidence(page, testInfo, 'C5-G10', 'board-approves-founding-application')

    await studentPage.reload()
    await expect(studentPage.getByText(/Founding application.*Approved/)).toBeVisible()
    await expect(studentPage.getByText('Active', { exact: true }).first()).toBeVisible()
    const meResponse = await request.get('/api/me', {
      headers: { Authorization: `Bearer ${student.token}` },
    })
    expect(meResponse.ok()).toBeTruthy()
    const member = (await meResponse.json()) as {
      lifecycle: { membership_status: string }
      exams: Array<{ certification_code?: string; entitlement_status?: string }>
      payments: Array<{ payment_provider?: string; payment_status?: string; final_amount?: number }>
    }
    expect(member.lifecycle.membership_status).toBe('active')
    expect(member.exams).toEqual(expect.arrayContaining([
      expect.objectContaining({ entitlement_status: 'available' }),
    ]))
    expect(member.payments).toEqual(expect.arrayContaining([
      expect.objectContaining({ payment_provider: 'founding_waiver', payment_status: 'paid', final_amount: 0 }),
    ]))

    const auditResponse = await request.get('/api/admin/audit', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    const audit = ((await auditResponse.json()) as {
      rows: Array<{ action?: string; details?: string; actor?: string }>
    }).rows
    expect(audit).toEqual(expect.arrayContaining([
      expect.objectContaining({ action: 'code_created', details: expect.stringContaining(discountCode), actor: E2E_ADMIN.email }),
      expect.objectContaining({ action: 'code_created', details: expect.stringContaining(foundingCode), actor: E2E_ADMIN.email }),
      expect.objectContaining({ action: 'founding_app_approved', details: expect.stringContaining(`subject ${student.id}`), actor: E2E_ADMIN.email }),
    ]))
  })

  test('a manually recorded payment appears in billing, invoices and a printable receipt', async ({ page, context, request }, testInfo) => {
    const studentPage = await context.newPage()
    const student = await registerStudent(request, studentPage, 'receipt')
    const adminToken = await apiLoginAsE2EAdmin(request, page)
    const gatewayReference = `RECEIPT-E2E-${Date.now()}`
    const marked = await request.post(`/api/admin/students/${student.id}/mark-paid`, {
      headers: { Authorization: `Bearer ${adminToken}` },
      data: {
        product: 'membership',
        amount: 149,
        method: 'bank_transfer',
        gateway_reference: gatewayReference,
        receipt_no: `RCT-${Date.now()}`,
        note: 'Browser receipt journey',
      },
    })
    expect(marked.ok()).toBeTruthy()
    const payment = (await marked.json()) as { payment_id: number; reference: string }

    await studentPage.goto('/app/billing')
    const paymentRow = studentPage.getByRole('row').filter({ hasText: payment.reference })
    await expect(paymentRow).toContainText('Membership')
    await expect(paymentRow).toContainText('$149.00')
    await expect(paymentRow).toContainText('paid')

    const receiptPagePromise = context.waitForEvent('page')
    await paymentRow.getByRole('button', { name: 'Receipt' }).click()
    const receiptPage = await receiptPagePromise
    await expect(receiptPage.getByRole('heading', { name: 'Payment receipt' })).toBeVisible()
    await expect(receiptPage.locator('body')).toContainText(student.email)
    await expect(receiptPage.locator('body')).toContainText(payment.reference)
    await expect(receiptPage.locator('body')).toContainText('$149.00')
    await expect(receiptPage.getByRole('button', { name: 'Print / Save as PDF' })).toBeVisible()
    await captureStoryEvidence(receiptPage, testInfo, 'C6', 'printable-payment-receipt')

    const invoiceResponse = await request.get('/api/me/invoices', {
      headers: { Authorization: `Bearer ${student.token}` },
    })
    expect(invoiceResponse.ok()).toBeTruthy()
    expect(await invoiceResponse.json()).toMatchObject({
      rows: expect.arrayContaining([
        expect.objectContaining({
          id: payment.payment_id,
          reference: payment.reference,
          product_type: 'membership',
          final_amount: 149,
          payment_status: 'paid',
        }),
      ]),
    })
  })
})
