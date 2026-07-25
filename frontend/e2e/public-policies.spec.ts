import { test, expect } from '@playwright/test'
import { preparePublicJourney } from './util'

const policyPages = [
  'policies',
  'terms-of-enrollment',
  'refund-policy',
  'privacy',
  'cookie-policy',
  'data-protection-policy',
  'terms',
  'website-disclaimer',
  'copyright-policy',
  'accessibility-statement',
  'quality-policy',
  'confidentiality-policy',
  'impartiality-policy',
  'ai-policy',
  'ai-decision-policy',
  'eligibility-policy',
  'certification-decision-policy',
  'examination-security',
  'exam-misconduct',
  'retake-policy',
  'cpd-policy',
  'membership-policy',
  'disciplinary-policy',
  'sanctions-policy',
  'records-retention',
  'fellowship-policy',
] as const

test.describe('public policy library', () => {
  test('every named policy route opens with crawlable content and visual evidence', async ({ page }, testInfo) => {
    test.slow()
    await preparePublicJourney(page)

    for (const slug of policyPages) {
      const response = await page.goto(`/${slug}.html`)
      expect(response?.status() ?? 0, `${slug} should return a successful document`).toBeLessThan(400)
      expect(response?.headers()['content-type'] ?? '').toContain('text/html')
      await expect(page.locator('main#content')).toBeVisible()
      await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
      await expect(page).not.toHaveTitle(/404|not found|error/i)

      const path = testInfo.outputPath(`A10-${slug}.png`)
      await page.screenshot({ path, animations: 'disabled' })
      await testInfo.attach(`A10-${slug}`, { path, contentType: 'image/png' })
    }
  })
})
