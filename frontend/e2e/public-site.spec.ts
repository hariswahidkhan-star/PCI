import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'

// Browser E2E + accessibility over the backend-served public site. Runs on CI runners (which boot the
// backend); the local sandbox blocks a server bind. The CI job is non-blocking until it has a green
// history, so these can never turn the pipeline red while unproven.
test.describe('public site', () => {
  test('home page loads with the right title, language and a heading', async ({ page }) => {
    const resp = await page.goto('/')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page).toHaveTitle(/Project Controls Institute/i)
    await expect(page.locator('html')).toHaveAttribute('lang', 'en-GB')
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
    // a skip-to-content affordance is present
    await expect(page.locator('a[href="#content"]').first()).toHaveText(/skip to main content/i)
  })

  test('the verify page renders the credential lookup', async ({ page }) => {
    const resp = await page.goto('/verify.html')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page).toHaveTitle(/Verify a Credential/i)
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
  })

  test('home page has no CRITICAL accessibility violations (axe, WCAG 2 A/AA)', async ({ page }) => {
    await page.goto('/')
    const results = await new AxeBuilder({ page }).withTags(['wcag2a', 'wcag2aa']).analyze()
    const critical = results.violations.filter((v) => v.impact === 'critical')
    const serious = results.violations.filter((v) => v.impact === 'serious')
    // Serious issues are logged for triage but don't fail this smoke; criticals must be zero.
    if (serious.length) console.log('axe serious (non-failing):', serious.map((v) => `${v.id}×${v.nodes.length}`).join(', '))
    if (critical.length) console.log('axe critical:', JSON.stringify(critical.map((v) => ({ id: v.id, nodes: v.nodes.length })), null, 2))
    expect(critical, `critical a11y violations: ${critical.map((v) => v.id).join(', ')}`).toEqual([])
  })
})
