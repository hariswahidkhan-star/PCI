import { test, expect } from '@playwright/test'
import { storyScreenshot, uniqueEmail } from './util'

test.describe('institution partner portal', () => {
  test('partner portal login loads and unauthenticated document downloads are forbidden', async ({ request, page }, testInfo) => {
    const resp = await page.goto('/partner.html')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.getByRole('heading', { name: /institution partner portal/i })).toBeVisible()
    await expect(page.locator('#inEmail')).toBeVisible()
    await expect(page.locator('#inPass')).toBeVisible()

    await page.locator('#inEmail').fill(uniqueEmail('partner'))
    await page.locator('#inPass').fill('definitely-wrong')
    await page.getByRole('button', { name: /sign in/i }).click()
    await expect(page.locator('#loginErr')).toContainText(/invalid email or password/i)

    const unauthDownload = await request.get('/api/partner/documents/1/download')
    expect(unauthDownload.status()).toBe(401)
    await storyScreenshot(page, testInfo, 'partner-login-form')
  })
})
