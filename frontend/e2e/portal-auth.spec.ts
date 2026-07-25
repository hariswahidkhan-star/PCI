import { test, expect } from '@playwright/test'
import { captureStoryEvidence, DEMO_STUDENT, uniqueEmail } from './util'

// Student-portal authentication journeys over the React SPA the backend serves under /app/
// (SPA fallback in backend/Program.cs; router basename '/app'). All waits are locator-based
// auto-waits or expect() polls — no fixed sleeps — and every test mints its own state.
test.describe('student portal auth', () => {
  test('a new student can register, skip onboarding and land on the dashboard', async ({ page }, testInfo) => {
    const email = uniqueEmail('register')
    await page.goto('/app/register')
    // The auth shell renders a marketing headline beside the form card, so target the form itself.
    await expect(page.getByLabel('First name')).toBeVisible()

    await page.getByLabel('First name').fill('E2E')
    await page.getByLabel('Last name').fill('Student')
    await page.getByLabel('Email address').fill(email)
    await page.getByLabel('Password (8+ characters)').fill('e2e-pass-123')
    await page.getByLabel('Confirm password').fill('e2e-pass-123')
    // Selecting a country auto-fills the dial code (+44) for the phone field.
    await page.locator('#rcountry').selectOption({ label: 'United Kingdom' })
    await page.locator('#rphone').fill('7700900000')
    await page.getByRole('button', { name: /create free account/i }).click()

    // Fresh signups are routed to the first-run onboarding wizard, which is fully skippable.
    await expect(page).toHaveURL(/\/app\/onboarding$/)
    await page.getByRole('link', { name: /skip for now/i }).click()

    // Dashboard chrome: sidebar navigation and the signed-in header controls.
    await expect(page).toHaveURL(/\/app\/?$/)
    await expect(page.getByRole('link', { name: 'Overview' })).toBeVisible()
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toBeVisible()
    await expect(page.getByText(email).first()).toBeVisible()
    await captureStoryEvidence(page, testInfo, 'B1', 'registered-dashboard')
  })

  test('the seeded demo student can sign in, and sign out back to the login screen', async ({ page }, testInfo) => {
    await page.goto('/app/login')
    await page.getByLabel('Email address').fill(DEMO_STUDENT.email)
    await page.getByLabel('Password', { exact: true }).fill(DEMO_STUDENT.password)
    await page.getByRole('button', { name: 'Sign in' }).click()

    // Portal dashboard renders for the signed-in student.
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toBeVisible()
    await expect(page.getByRole('link', { name: 'Overview' })).toBeVisible()
    await expect(page.getByText(DEMO_STUDENT.email).first()).toBeVisible()
    await captureStoryEvidence(page, testInfo, 'B2', 'signed-in')

    // Signing out revokes the session and returns to the login screen.
    await page.getByRole('button', { name: 'Sign out', exact: true }).click()
    await expect(page).toHaveURL(/\/app\/login$/)
    await expect(page.getByLabel('Email address')).toBeVisible()
  })

  test('a wrong password shows an error and stays on the sign-in screen', async ({ page }) => {
    await page.goto('/app/login')
    // A never-registered address keeps this test independent of every other account
    // (and exercises the same invalid_credentials path without touching lockout counters).
    await page.getByLabel('Email address').fill(uniqueEmail('nobody'))
    await page.getByLabel('Password', { exact: true }).fill('definitely-wrong')
    await page.getByRole('button', { name: 'Sign in' }).click()

    await expect(page.getByRole('alert')).toBeVisible()
    await expect(page).toHaveURL(/\/app\/login$/)
    // Still unauthenticated: the protected shell never appeared.
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toHaveCount(0)
  })

  test('@cross-browser an unauthenticated visit to the portal redirects to the login screen', async ({ page }) => {
    await page.goto('/app/')
    await expect(page).toHaveURL(/\/app\/login$/)
    await expect(page.getByLabel('Email address')).toBeVisible()
    await expect(page.getByLabel('Password', { exact: true })).toBeVisible()
  })
})
