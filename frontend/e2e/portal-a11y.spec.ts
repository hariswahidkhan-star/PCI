import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'
import { apiLoginAsDemoStudent } from './util'

// Axe (WCAG 2 A/AA) over the student-portal screens — the public pages already have axe smoke
// coverage in public-site.spec.ts; this extends it behind the login. House rule mirrored from
// there: serious violations are logged for triage, criticals fail the run.
async function expectNoCriticalViolations(page: import('@playwright/test').Page) {
  const results = await new AxeBuilder({ page }).withTags(['wcag2a', 'wcag2aa']).analyze()
  const critical = results.violations.filter((v) => v.impact === 'critical')
  const serious = results.violations.filter((v) => v.impact === 'serious')
  if (serious.length) console.log('axe serious (non-failing):', serious.map((v) => `${v.id}×${v.nodes.length}`).join(', '))
  expect(critical, `critical a11y violations: ${critical.map((v) => v.id).join(', ')}`).toEqual([])
}

test.describe('student portal accessibility (axe)', () => {
  test('the sign-in screen has no critical violations', async ({ page }) => {
    await page.goto('/app/login')
    await expect(page.getByLabel('Email address')).toBeVisible()
    await expectNoCriticalViolations(page)
  })

  test('the authenticated dashboard has no critical violations', async ({ page, request }) => {
    await apiLoginAsDemoStudent(request, page)
    await page.goto('/app/')
    // Settle the screen first: shell chrome present and the data spinner gone.
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toBeVisible()
    await expect(page.getByRole('status', { name: 'Loading' })).toHaveCount(0)
    await expectNoCriticalViolations(page)
  })

  test('the authenticated profile screen has no critical violations', async ({ page, request }) => {
    await apiLoginAsDemoStudent(request, page)
    await page.goto('/app/profile')
    await expect(page.getByRole('button', { name: 'Sign out', exact: true })).toBeVisible()
    await expect(page.getByRole('status', { name: 'Loading' })).toHaveCount(0)
    await expectNoCriticalViolations(page)
  })
})
