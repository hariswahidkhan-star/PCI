import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'
import { captureStoryEvidence } from './util'

// Browser E2E + accessibility over the backend-served public site. Runs on CI runners (which boot the
// backend). The CI job is gating; Chromium runs the full suite and the tagged smoke paths also run
// in Firefox, WebKit, Mobile Chrome and Mobile Safari profiles.
test.describe('public site', () => {
  test('@cross-browser home page loads with the right title, language and a heading', async ({ page }, testInfo) => {
    const resp = await page.goto('/')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page).toHaveTitle(/Project Controls Institute/i)
    await expect(page.locator('html')).toHaveAttribute('lang', 'en-GB')
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
    // a skip-to-content affordance is present
    await expect(page.locator('a[href="#content"]').first()).toHaveText(/skip to main content/i)
    await captureStoryEvidence(page, testInfo, 'A1', 'homepage')
  })

  test('@cross-browser the verify page renders the credential lookup', async ({ page }) => {
    const resp = await page.goto('/verify.html')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page).toHaveTitle(/Verify a Credential/i)
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
  })

  test('legacy student-login and student-dashboard pages hand off to the React portal (no auth bypass)', async ({ page }) => {
    // DEF-AUDIT-01: the old static student-login button previously navigated to student-dashboard.html
    // without calling /api/login. Both pages must now redirect into /app.
    await page.goto('/student-login.html')
    await expect(page).toHaveURL(/\/app\/login/)
    await page.goto('/student-dashboard.html')
    await expect(page).toHaveURL(/\/app\/?(\?|$)/)
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

  test('the admin-controlled announcement is accessible, dismissible and stays dismissed for the visit', async ({ page, request }, testInfo) => {
    await page.addInitScript(() => {
      try { localStorage.setItem('pci-cookie-consent', 'essential') } catch { /* no storage */ }
    })
    const configResponse = await request.get('/api/announcement')
    expect(configResponse.ok()).toBeTruthy()
    const config = (await configResponse.json()) as { enabled: boolean; key: string; title: string; dismiss: string }
    expect(config.enabled).toBe(true)

    await page.goto('/')
    const dialog = page.locator('#pciAnx')
    await expect(dialog).toBeVisible()
    await expect(dialog).toHaveAttribute('role', 'dialog')
    await expect(dialog.getByRole('heading')).toHaveText(config.title)
    await captureStoryEvidence(page, testInfo, 'A4', 'announcement-visible')
    // Prefer the secondary CTA — the X control shares data-anx-close but must not share this name.
    await dialog.locator('.pci-anx-secondary').click()
    await expect(dialog).toHaveCount(0)
    expect(await page.evaluate((key) => sessionStorage.getItem(`pci.anx.${key}`), config.key)).toBe('1')

    const announcementResponse = page.waitForResponse((response) => response.url().endsWith('/api/announcement'))
    await page.reload()
    await (await announcementResponse).finished()
    await page.evaluate(() => new Promise<void>((resolve) => requestAnimationFrame(() => requestAnimationFrame(() => resolve()))))
    await expect(page.locator('#pciAnx')).toHaveCount(0)
  })
})
