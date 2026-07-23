import { test, expect } from '@playwright/test'
import { apiLoginAsE2EAdmin, preparePublicJourney, uniqueEmail } from './util'

const minimalPdf = Buffer.from('%PDF-1.4\n1 0 obj<</Type/Catalog>>endobj\n%%EOF\n')

test.describe('public form-to-admin journeys', () => {
  test('honorary application submits its structured history and CV to the board queue', async ({ page, request }) => {
    await preparePublicJourney(page)
    const adminToken = await apiLoginAsE2EAdmin(request)
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

    const queueResponse = await request.get('/api/admin/honorary-applications?status=pending_review', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(queueResponse.ok()).toBeTruthy()
    const queue = (await queueResponse.json()) as { rows: Array<Record<string, unknown>> }
    expect(queue.rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ email, reference: result.reference, doc_count: 1, certification_name: 'PCI Project Management Leader – AI' }),
    ]))
  })

  test('training-provider application reaches the partner review queue with evidence', async ({ page, request }) => {
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

    const queueResponse = await request.get('/api/admin/training-partner-applications?status=pending_review', {
      headers: { Authorization: `Bearer ${adminToken}` },
    })
    expect(queueResponse.ok()).toBeTruthy()
    const queue = (await queueResponse.json()) as { rows: Array<Record<string, unknown>> }
    expect(queue.rows).toEqual(expect.arrayContaining([
      expect.objectContaining({ org_name: organisation, contact_email: email, reference: result.reference, doc_count: 1 }),
    ]))
  })

  test('contact enquiry and newsletter opt-in are visible to administrators', async ({ page, request }) => {
    await preparePublicJourney(page)
    const adminToken = await apiLoginAsE2EAdmin(request)
    const inquiryEmail = uniqueEmail('inquiry')
    const subscriberEmail = uniqueEmail('subscriber')
    const subject = `Browser enquiry ${Date.now()}`

    await page.goto('/contact.html')
    await page.waitForFunction(() => Boolean((window as typeof window & { PCI_API_BASE?: string }).PCI_API_BASE))
    await page.getByLabel('Name').fill('Browser Visitor')
    await page.getByLabel('Email').fill(inquiryEmail)
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
