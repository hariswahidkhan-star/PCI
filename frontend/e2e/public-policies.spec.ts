import { test, expect } from '@playwright/test'
import { storyScreenshot } from './util'

const policyPages = [
  'policies.html',
  'privacy.html',
  'cookie-policy.html',
  'refund-policy.html',
  'data-protection-policy.html',
  'cpd-policy.html',
  'impartiality-policy.html',
  'quality-policy.html',
  'eligibility-policy.html',
  'disciplinary-policy.html',
  'certification-decision-policy.html',
  'membership-policy.html',
  'ai-policy.html',
  'ai-decision-policy.html',
  'confidentiality-policy.html',
  'copyright-policy.html',
  'sanctions-policy.html',
]

test.describe('public policy pages', () => {
  for (const route of policyPages) {
    test(`${route} returns crawlable main content`, async ({ page }, testInfo) => {
      const resp = await page.goto(`/${route}`)
      expect(resp?.status() ?? 0).toBeLessThan(400)
      const main = page.locator('main, #content').first()
      await expect(page.locator('main h1, #content h1').first()).toBeVisible()
      await expect.poll(async () => (await main.innerText()).trim().length).toBeGreaterThan(100)
      await storyScreenshot(page, testInfo, route.replace('.html', ''))
    })
  }
})
