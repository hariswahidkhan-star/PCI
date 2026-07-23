import { test, expect } from '@playwright/test'
import { storyScreenshot, uniqueEmail } from './util'

const samplePdf = `data:application/pdf;base64,${Buffer.from('%PDF-1.4 e2e application evidence').toString('base64')}`

test.describe('public application routes', () => {
  test('honorary and founding route pages load with crawlable headings', async ({ page }, testInfo) => {
    for (const route of ['honorary-application.html', 'route-standard.html', 'route-founding.html', 'route-honorary.html']) {
      const resp = await page.goto(`/${route}`)
      expect(resp?.status() ?? 0).toBeLessThan(400)
      await expect(page.locator('h1').first()).toBeVisible()
    }
    await storyScreenshot(page, testInfo, 'application-routes')
  })

  test('honorary application API accepts a complete public application', async ({ request, page }, testInfo) => {
    const email = uniqueEmail('honorary')
    const res = await request.post('/api/honorary-application', {
      data: {
        first_name: 'E2E',
        last_name: 'Honorary',
        email,
        mobile: '+447700900001',
        country: 'United Kingdom',
        city: 'London',
        nationality: 'British',
        job_title: 'Programme Controls Director',
        employer: 'E2E Infrastructure Ltd',
        years_experience: 12,
        industry: 'Infrastructure',
        highest_qualification: 'MSc Project Controls',
        relevant_experience: 'Led multi-disciplinary project controls teams across capital programmes.',
        professional_summary: 'A senior controls practitioner with sustained contribution to standards, mentoring and delivery governance.',
        certification: 'pml-ai',
        declaration: true,
        eligibility_confirmed: true,
        terms_accepted: true,
        documents: [{ doc_kind: 'resume', filename: 'e2e-cv.pdf', data_uri: samplePdf }],
      },
    })
    expect(res.ok(), `honorary application should be accepted (got ${res.status()})`).toBeTruthy()
    const body = (await res.json()) as { ok?: boolean; reference?: string }
    expect(body.ok).toBeTruthy()
    expect(body.reference).toMatch(/^PCI-HONAPP-/)

    await page.goto('/honorary-application.html')
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
    await storyScreenshot(page, testInfo, 'honorary-application-accepted')
  })
})
