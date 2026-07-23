import { test, expect } from '@playwright/test'
import { dismissPublicOverlays, E2E_STUDENT_PASSWORD, storyScreenshot, uniqueEmail } from './util'

test.describe('student account security', () => {
  test('forgot password returns a non-enumerating message', async ({ page }, testInfo) => {
    const email = uniqueEmail('forgot')
    const resp = await page.goto('/forgot-password.html')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await dismissPublicOverlays(page)
    await page.locator('#forgotEmail').fill(email)
    await page.getByRole('button', { name: /send reset link/i }).click()
    await expect(page.locator('#forgotMsg')).toContainText(/if an account exists/i)
    await storyScreenshot(page, testInfo, 'forgot-password-non-enumerating')
  })

  test('invalid reset token hands off to the portal login', async ({ page }) => {
    await page.goto('/reset-password.html?token=bad')
    await dismissPublicOverlays(page)
    await page.locator('#newPass').fill(E2E_STUDENT_PASSWORD)
    await page.locator('#confirmPass').fill(E2E_STUDENT_PASSWORD)
    await page.locator('#resetForm button[type="submit"]').click()
    await expect(page.locator('#resetMsg')).toContainText(/invalid|expired|missing/i)
    await expect(page).toHaveURL(/\/app\/login\/?$/, { timeout: 10_000 })
  })

  test('new student registration can progress through onboarding and logout', async ({ page }, testInfo) => {
    const email = uniqueEmail('onboarding')
    await page.goto('/app/register')
    await page.getByLabel('First name').fill('E2E')
    await page.getByLabel('Last name').fill('Security')
    await page.getByLabel('Email address').fill(email)
    await page.getByLabel('Password (8+ characters)').fill(E2E_STUDENT_PASSWORD)
    await page.getByLabel('Confirm password').fill(E2E_STUDENT_PASSWORD)
    await page.locator('#rcountry').selectOption({ label: 'United Kingdom' })
    await page.locator('#rphone').fill('7700900002')
    await page.getByRole('button', { name: /create free account/i }).click()

    await expect(page).toHaveURL(/\/app\/onboarding$/)
    await page.getByRole('button', { name: /let/i }).click()
    await page.locator('#ob_city').fill('London')
    await page.locator('#ob_current_role').fill('Planner')
    await page.locator('#ob_company').fill('E2E Controls')
    await page.locator('#ob_years_experience').fill('5')
    await page.getByRole('button', { name: /save.*continue/i }).click()
    await expect(page.getByText(/experience/i).first()).toBeVisible()

    await page.getByRole('link', { name: /skip for now/i }).click()
    await expect(page).toHaveURL(/\/app\/?$/)
    await expect(page.getByText(email).first()).toBeVisible()
    await storyScreenshot(page, testInfo, 'onboarding-progress')

    await page.getByRole('button', { name: 'Sign out' }).click()
    await expect(page).toHaveURL(/\/app\/login$/)
  })
})
