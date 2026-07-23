import { test, expect } from '@playwright/test'
import { OWNER_ADMIN, uniqueEmail } from './util'

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

  test('the seeded owner signs in and is forced to set a new password before the console opens', async ({ page }) => {
    await page.goto('/admin/login')
    await page.getByLabel('Email address').fill(OWNER_ADMIN.email)
    await page.getByLabel('Password').fill(OWNER_ADMIN.password)
    await page.getByRole('button', { name: 'Sign in' }).click()

    // must_change_pw gate: the console stays closed until a new password is set (also enforced
    // server-side for every /api/admin/* call). We assert the gate and stop — never change it.
    await expect(page.getByRole('heading', { name: 'Set a new password' })).toBeVisible()
    await expect(page.getByLabel('New password')).toBeVisible()
    await expect(page.getByLabel('Confirm password')).toBeVisible()
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
