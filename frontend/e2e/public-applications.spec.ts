import { readFileSync } from 'node:fs'
import { test, expect } from '@playwright/test'
import { E2E_ADMIN, apiLoginAsE2EAdmin, captureStoryEvidence, preparePublicJourney, uniqueEmail } from './util'

const minimalPdf = Buffer.from('%PDF-1.4\n1 0 obj<</Type/Catalog>>endobj\n%%EOF\n')
const onePixelPng = Buffer.from(
  'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=',
  'base64',
)

test.describe('public form-to-admin journeys', () => {
  test('honorary application completes identity review, board approval, portal delivery and revocation', async ({ page, context, request }, testInfo) => {
    test.slow()
    await preparePublicJourney(page)
    const adminToken = await apiLoginAsE2EAdmin(request, page)
    const email = uniqueEmail('honorary')

    await page.goto('/honorary-application.html')
    const form = page.locator('#honForm')
    await form.locator('[name="first_name"]').fill('Browser')
    await form.locator('[name="last_name"]').fill('Honorary')
    await form.locator('[name="email"]').fill(email)
    await form.locator('[name="phone"]').fill('7700900010')
    await form.locator('[name="country"]').selectOption('United Kingdom')
    await form.locator('[name="city"]').fill('London')
    await form.locator('[name="nationality"]').fill('British')
    await form.locator('[name="job_title"]').fill('Programme Controls Director')
    await form.locator('[name="employer"]').fill('Browser Infrastructure Group')
    await form.locator('[name="years_experience"]').fill('18')
    await form.locator('[name="industry"]').fill('Infrastructure')
    await expect(form.locator('#honCert option[value="PML-AI"]')).toHaveCount(1)
    await form.locator('#honCert').selectOption('PML-AI')

    await form.locator('#honQualRows .hr-a').first().fill('MSc Project Management')
    await form.locator('#honQualRows .hr-b').first().fill('Browser University')
    await form.locator('#honQualRows .hr-year').first().fill('2010')
    await form.locator('#honCertRows .hr-a').first().fill('PMP')
    await form.locator('#honCertRows .hr-b').first().fill('PMI')
    await form.locator('#honCertRows .hr-year').first().fill('2012')
    await form.locator('#honExpRows .hr-a').first().fill('Programme Controls Director')
    await form.locator('#honExpRows .hr-b').first().fill('Browser Infrastructure Group')
    await form.locator('#honExpRows .hr-from').first().fill('2014')
    await form.locator('[name="relevant_experience"]').fill('Led governance, forecasting and delivery assurance across major programmes for more than a decade.')
    await form.locator('[name="professional_summary"]').fill('Project controls leader with sustained professional contribution and mentoring experience.')
    await form.locator('#doc_resume').setInputFiles({ name: 'browser-cv.pdf', mimeType: 'application/pdf', buffer: minimalPdf })
    for (const id of ['honSuit', 'honEligible', 'honDecl', 'honTerms']) await form.locator(`#${id}`).check()

    const submitResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/honorary-application') && response.request().method() === 'POST')
    await form.getByRole('button', { name: /Submit application/i }).click()
    const submitted = await submitResponse
    expect(submitted.ok()).toBeTruthy()
    const result = (await submitted.json()) as { reference: string }
    await expect(page.locator('#honOk')).toContainText(result.reference)
    await expect(form).toBeHidden()
    await captureStoryEvidence(page, testInfo, 'A5', 'honorary-submitted')

    const queueResponse = await request.get('/api/admin/honorary-applications?status=pending_review', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(queueResponse.ok()).toBeTruthy()
    const queue = (await queueResponse.json()) as { rows: Array<Record<string, unknown>> }
    expect(queue.rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ email, reference: result.reference, doc_count: 1, certification_name: 'PCI Project Management Leader – AI' }),
    ]))
    const queued = queue.rows.find((row) => row.email === email && row.reference === result.reference)
    expect(queued?.id).toBeTruthy()

    await page.goto('/admin/honorary-applications')
    const applicationRow = page.getByRole('row').filter({ hasText: result.reference })
    await expect(applicationRow).toContainText('pending review')
    await applicationRow.getByRole('button', { name: 'Review' }).click()
    let drawer = page.locator('.drawer')
    await expect(drawer).toContainText(email)
    await expect(drawer).toContainText('browser-cv.pdf')
    const shortlistResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith(`/api/admin/honorary-applications/${queued!.id}/shortlist`)
      && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: /Shortlist & generate secure link/ }).click()
    expect((await shortlistResponsePromise).ok()).toBeTruthy()
    const secureLinkInput = drawer.locator('input[readonly]')
    await expect(secureLinkInput).toHaveValue(/honorary-verification\.html\?token=/)
    const secureLink = await secureLinkInput.inputValue()
    await captureStoryEvidence(page, testInfo, 'G9', 'board-shortlists-and-generates-idv-link')

    const applicantPage = await context.newPage()
    await preparePublicJourney(applicantPage)
    await applicantPage.goto(secureLink)
    await expect(applicantPage.locator('#vForm')).toBeVisible()
    await applicantPage.locator('#vPhoto').setInputFiles({
      name: 'passport-photo.png',
      mimeType: 'image/png',
      buffer: onePixelPng,
    })
    await applicantPage.locator('#vGovId').setInputFiles({
      name: 'government-id.pdf',
      mimeType: 'application/pdf',
      buffer: minimalPdf,
    })
    for (const id of ['vTruthful', 'vBackground', 'vConsent']) await applicantPage.locator(`#${id}`).check()
    const idvResponsePromise = applicantPage.waitForResponse((response) =>
      response.url().includes('/api/honorary-idv/') && response.request().method() === 'POST')
    await applicantPage.getByRole('button', { name: /Submit securely/ }).click()
    expect((await idvResponsePromise).ok()).toBeTruthy()
    await expect(applicantPage.locator('#vDone')).toBeVisible()
    await captureStoryEvidence(applicantPage, testInfo, 'A5', 'applicant-completes-one-time-idv')

    const burnedToken = new URL(secureLink).searchParams.get('token')
    expect(burnedToken).toBeTruthy()
    const tokenReuse = await request.get(`/api/honorary-idv/${encodeURIComponent(burnedToken!)}`)
    expect(tokenReuse.status()).toBe(404)

    await page.reload()
    const reviewedRow = page.getByRole('row').filter({ hasText: result.reference })
    await reviewedRow.getByRole('button', { name: 'Review' }).click()
    drawer = page.locator('.drawer')
    await expect(drawer).toContainText('submitted')
    await expect(drawer).toContainText('passport-photo.png')
    await expect(drawer).toContainText('government-id.pdf')
    await drawer.getByLabel('Internal note / decision note').fill('Approved after board review and identity verification.')
    const approveResponsePromise = page.waitForResponse((response) =>
      response.url().endsWith(`/api/admin/honorary-applications/${queued!.id}/decide`)
      && response.request().method() === 'POST')
    await drawer.getByRole('button', { name: 'Approve & confer' }).click()
    const approveResponse = await approveResponsePromise
    expect(approveResponse.ok()).toBeTruthy()
    const approved = (await approveResponse.json()) as { award_no: string; setup_url: string }
    expect(approved.award_no).toMatch(/^PCI-HON-/)
    expect(approved.setup_url).toContain('/reset-password.html?token=')
    await expect(page.getByRole('status')).toContainText(approved.award_no)
    await expect(page.getByRole('link', { name: 'open password setup' })).toHaveAttribute('href', approved.setup_url)

    const setupUrl = new URL(approved.setup_url)
    const password = 'HonoraryBrowser-2026!'
    await applicantPage.goto(setupUrl.pathname + setupUrl.search)
    await applicantPage.getByLabel('New password', { exact: true }).fill(password)
    await applicantPage.getByLabel('Confirm new password', { exact: true }).fill(password)
    const setPasswordResponsePromise = applicantPage.waitForResponse((response) =>
      response.url().endsWith('/api/set-password') && response.request().method() === 'POST')
    await applicantPage.getByRole('button', { name: 'Reset password' }).click()
    expect((await setPasswordResponsePromise).ok()).toBeTruthy()
    await expect(applicantPage).toHaveURL(/\/app\/login$/)
    await applicantPage.getByLabel('Email address').fill(email)
    await applicantPage.getByLabel('Password', { exact: true }).fill(password)
    await applicantPage.getByRole('button', { name: 'Sign in' }).click()
    await applicantPage.goto('/app/credentials')
    const honoraryCard = applicantPage.locator('.card').filter({ hasText: approved.award_no })
    await expect(honoraryCard).toContainText('Honorary Fellow')
    await expect(honoraryCard).toContainText('Board-conferred')
    const certificateDownloadPromise = applicantPage.waitForEvent('download')
    await honoraryCard.getByRole('button', { name: 'Download PDF' }).click()
    const certificateDownload = await certificateDownloadPromise
    const certificatePath = await certificateDownload.path()
    expect(certificatePath).toBeTruthy()
    expect(readFileSync(certificatePath!).subarray(0, 4).toString()).toBe('%PDF')
    await captureStoryEvidence(applicantPage, testInfo, 'E5-G9', 'honorary-member-downloads-board-certificate')

    const publicActive = await request.get(`/api/verify?id=${encodeURIComponent(approved.award_no)}`)
    expect(await publicActive.json()).toMatchObject({
      found: true,
      type: 'honorary',
      valid: true,
      state: 'active',
      award_no: approved.award_no,
    })

    await page.goto('/admin/honorary')
    const awardRow = page.getByRole('row').filter({ hasText: approved.award_no })
    await expect(awardRow).toContainText(email)
    await page.getByLabel('Revocation reason (recorded with the action)').fill('E2E board lifecycle test')
    page.once('dialog', (dialog) => dialog.accept())
    const revokeResponsePromise = page.waitForResponse((response) =>
      response.url().match(/\/api\/admin\/honorary\/\d+\/revoke$/) !== null
      && response.request().method() === 'POST')
    await awardRow.getByRole('button', { name: 'Revoke' }).click()
    expect((await revokeResponsePromise).ok()).toBeTruthy()
    await expect(awardRow).toContainText('revoked')

    const publicRevoked = await request.get(`/api/verify?id=${encodeURIComponent(approved.award_no)}`)
    expect(await publicRevoked.json()).toMatchObject({
      found: true,
      type: 'honorary',
      valid: false,
      state: 'revoked',
    })
    const studentToken = await applicantPage.evaluate(() => sessionStorage.getItem('pci.session.token'))
    expect(studentToken).toBeTruthy()
    const revokedDownload = await request.get('/api/me/honorary-certificate/pdf', {
      headers: { Authorization: `Bearer ${studentToken}` },
    })
    expect(revokedDownload.status()).toBe(404)

    const auditResponse = await request.get('/api/admin/audit', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    const audit = ((await auditResponse.json()) as {
      rows: Array<{ action?: string; details?: string; actor?: string }>
    }).rows
    expect(audit).toEqual(expect.arrayContaining([
      expect.objectContaining({ action: 'honorary_shortlisted', details: expect.stringContaining(result.reference), actor: E2E_ADMIN.email }),
      expect.objectContaining({ action: 'honorary_application_approved', details: expect.stringContaining(approved.award_no), actor: E2E_ADMIN.email }),
      expect.objectContaining({ action: 'honorary_revoked', details: expect.stringContaining(approved.award_no), actor: E2E_ADMIN.email }),
    ]))
  })

  test('training-provider application reaches the partner review queue with evidence', async ({ page, request }, testInfo) => {
    await preparePublicJourney(page)
    const adminToken = await apiLoginAsE2EAdmin(request)
    const email = uniqueEmail('training-partner')
    const organisation = `Browser Training ${Date.now()}`

    await page.goto('/become-a-training-partner.html')
    const form = page.locator('#tpForm')
    await form.locator('[name="org_name"]').fill(organisation)
    await form.locator('[name="website"]').fill('https://training.example.test')
    await form.locator('[name="contact_name"]').fill('Browser Partner')
    await form.locator('[name="contact_email"]').fill(email)
    await form.locator('[name="phone"]').fill('7700900011')
    await form.locator('[name="country"]').selectOption('United Kingdom')
    await form.locator('[name="city"]').fill('Manchester')
    await form.locator('[name="region"]').fill('Europe and Middle East')
    await form.locator('[name="delivery_modes"]').fill('Live online, in-person')
    await form.locator('[name="learners_per_year"]').fill('250')
    await form.locator('[name="specialties"]').fill('Planning, project controls, project finance and governed AI.')
    await form.locator('[name="description"]').fill('Independent training provider with qualified instructors and candidate-support controls.')
    await form.locator('#doc_company_profile').setInputFiles({ name: 'company-profile.pdf', mimeType: 'application/pdf', buffer: minimalPdf })
    await form.locator('#tpDecl').check()

    const submitResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/training-partner-application') && response.request().method() === 'POST')
    await form.getByRole('button', { name: /Submit application/i }).click()
    const submitted = await submitResponse
    expect(submitted.ok()).toBeTruthy()
    const result = (await submitted.json()) as { reference: string }
    await expect(page.locator('#tpOk')).toContainText(result.reference)
    await expect(form).toBeHidden()
    await captureStoryEvidence(page, testInfo, 'A6', 'partner-application-submitted')

    const queueResponse = await request.get('/api/admin/training-partner-applications?status=pending_review', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(queueResponse.ok()).toBeTruthy()
    const queue = (await queueResponse.json()) as { rows: Array<Record<string, unknown>> }
    expect(queue.rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ org_name: organisation, contact_email: email, reference: result.reference, doc_count: 1 }),
    ]))
  })

  test('contact enquiry and newsletter opt-in are visible to administrators', async ({ page, request }, testInfo) => {
    await preparePublicJourney(page)
    const adminToken = await apiLoginAsE2EAdmin(request)
    const inquiryEmail = uniqueEmail('inquiry')
    const subscriberEmail = uniqueEmail('subscriber')
    const subject = `Browser enquiry ${Date.now()}`

    await page.goto('/contact.html')
    await page.waitForFunction(() => Boolean((window as typeof window & { PCI_API_BASE?: string }).PCI_API_BASE))
    await page.getByLabel('Name').fill('Browser Visitor')
    await page.getByLabel('Email', { exact: true }).fill(inquiryEmail)
    await page.getByLabel('Subject').fill(subject)
    await page.getByLabel('Message').fill('Please send details about the three-certification journey.')
    const inquiryResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/inquiry') && response.request().method() === 'POST')
    await page.getByRole('button', { name: /Send message/i }).click()
    const inquirySubmitted = await inquiryResponse
    expect(inquirySubmitted.ok()).toBeTruthy()
    const inquiry = (await inquirySubmitted.json()) as { reference: string }
    await expect(page.locator('#contactForm .proto-msg')).toContainText(inquiry.reference)

    await page.getByLabel('Email address').fill(subscriberEmail)
    const newsletterResponse = page.waitForResponse((response) =>
      response.url().endsWith('/api/newsletter') && response.request().method() === 'POST')
    await page.getByRole('button', { name: 'Subscribe' }).click()
    expect((await newsletterResponse).ok()).toBeTruthy()
    await expect(page.locator('#nlBtn')).toHaveText('Subscribed ✓')
    await captureStoryEvidence(page, testInfo, 'A7', 'contact-and-newsletter')

    const [inquiriesResponse, subscribersResponse] = await Promise.all([
      request.get(`/api/admin/inquiries?q=${encodeURIComponent(inquiryEmail)}`, { headers: { Authorization: `Bearer ${adminToken}` } }),
      request.get('/api/admin/subscribers', { headers: { Authorization: `Bearer ${adminToken}` } }),
    ])
    expect(inquiriesResponse.ok()).toBeTruthy()
    expect(subscribersResponse.ok()).toBeTruthy()
    const inquiries = (await inquiriesResponse.json()) as { rows: Array<Record<string, unknown>> }
    const subscribers = (await subscribersResponse.json()) as { rows: Array<Record<string, unknown>> }
    expect(inquiries.rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ email: inquiryEmail, topic: subject, reference: inquiry.reference }),
    ]))
    expect(subscribers.rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ email: subscriberEmail, status: 'subscribed' }),
    ]))
  })
})
