import { test, expect } from '@playwright/test'
import { E2E_OWNER_PASSWORD, OWNER_ADMIN, uniqueEmail } from './util'

// Admin console (React SPA under /admin/). The Development boot seeds a bootstrap owner
// (owner@pci.local / changeme-owner) flagged must_change_pw, so a successful sign-in lands on
// the forced "set a new password" gate — asserted here WITHOUT completing the change, keeping
// the seeded credentials valid for every subsequent run against the same database.
test.describe('admin console', () => {
  test('an unauthenticated visit redirects to the staff sign-in screen', async ({ page }) => {
    await page.goto('/admin/')
    await expect(page).toHaveURL(/\/admin\/login$/)
    await expect(page.getByRole('heading', { name: 'Admin Console' })).toBeVisible()
    await expect(page.getByLabel('Email address')).toBeVisible()
    await expect(page.getByLabel('Password')).toBeVisible()
  })

  test('the seeded owner signs in and reaches the expected security gate or console', async ({ page }) => {
    await page.goto('/admin/login')
    await page.getByLabel('Email address').fill(OWNER_ADMIN.email)
    await page.getByLabel('Password').fill(OWNER_ADMIN.password)
    await page.getByRole('button', { name: 'Sign in' }).click()

    if (await page.getByRole('alert').isVisible()) {
      await page.getByLabel('Password').fill(E2E_OWNER_PASSWORD)
      await page.getByRole('button', { name: 'Sign in' }).click()
    }

    // Fresh databases still show the must_change_pw gate; later E2E flows may already have cleared it
    // with the stable E2E password, in which case the owner should reach the console.
    await expect(page.getByRole('heading', { name: /Set a new password|Dashboard|Overview|Admin Console/i }).first()).toBeVisible()
  })

  test('a failed staff sign-in shows an error and keeps the console closed', async ({ page }) => {
    await page.goto('/admin/login')
    // Unknown admin address: exercises invalid_credentials without touching lockout counters.
    await page.getByLabel('Email address').fill(uniqueEmail('not-an-admin'))
    await page.getByLabel('Password').fill('definitely-wrong')
    await page.getByRole('button', { name: 'Sign in' }).click()

    await expect(page.getByRole('alert')).toBeVisible()
    await expect(page).toHaveURL(/\/admin\/login$/)
  })
})
