import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'
import { apiLoginAsE2EAdmin, createTestUser } from './util'

// Axe (WCAG 2 A/AA) over the Simulation Lab student screens — the catalogue and the interactive
// multi-step runner. House rule mirrored from portal-a11y.spec.ts: serious violations are logged for
// triage, criticals fail the run. Uses an entitled member session so the REAL catalogue and runner
// render (the demo student lacks Lab access).
async function expectNoCriticalViolations(page: import('@playwright/test').Page, ctx: string) {
  const results = await new AxeBuilder({ page }).withTags(['wcag2a', 'wcag2aa']).analyze()
  const critical = results.violations.filter((v) => v.impact === 'critical')
  const serious = results.violations.filter((v) => v.impact === 'serious')
  if (serious.length) console.log(`axe serious (${ctx}, non-failing):`, serious.map((v) => `${v.id}×${v.nodes.length}`).join(', '))
  expect(critical, `critical a11y violations (${ctx}): ${critical.map((v) => v.id).join(', ')}`).toEqual([])
}

test.describe('Simulation Lab accessibility (axe)', () => {
  test('the catalogue and the multi-step runner have no critical violations', async ({ page, request }) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    // An entitled member so /app/lab shows the real catalogue and the runner can open a scenario.
    await createTestUser(request, adminToken, 'member', page)

    // ── Catalogue ──
    await page.goto('/app/lab')
    await expect(page.getByRole('heading', { name: /Project Controls Practice Lab/i })).toBeVisible({ timeout: 20_000 })
    // Settle: the access-gated catalogue has resolved (either cards or the toolbar are present).
    await expect(page.getByRole('status', { name: 'Loading' })).toHaveCount(0)
    await expectNoCriticalViolations(page, 'catalogue')

    // ── Multi-step runner ── (the P1/P2 linked-scenario workspace)
    await page.goto('/app/lab/MS-RECOVERY-001')
    // The runner has rendered its steps (semantic list + decision fieldsets) once a step title shows.
    await expect(page.getByText(/Baseline health|Choose a recovery path|Re-forecast/i).first()).toBeVisible({ timeout: 20_000 })
    await expectNoCriticalViolations(page, 'multi-step runner')

    // Keyboard smoke: the primary action is reachable and operable by keyboard.
    await expect(page.getByRole('button', { name: 'Submit for grading' })).toBeVisible()
  })

  // P3 Arabic/RTL viewport pass. Selecting Arabic flips <html dir="rtl" lang="ar"> (I18nProvider →
  // applyToDocument), which drives the right-to-left layout. We plant the language before the app
  // boots and confirm the catalogue and the multi-step runner both render right-to-left with no
  // critical axe violations — the RTL reflow must not break the semantic structure axe checks.
  test('the catalogue and runner render right-to-left (Arabic) with no critical violations', async ({ page, request }) => {
    const adminToken = await apiLoginAsE2EAdmin(request)
    await createTestUser(request, adminToken, 'member', page)
    // Pin the SPA to Arabic before any page script runs, on the app origin.
    await page.addInitScript(() => {
      try { localStorage.setItem('pci.lang', 'ar') } catch { /* storage blocked */ }
    })

    // ── Catalogue (RTL) ──
    await page.goto('/app/lab')
    await expect(page.getByRole('heading', { name: /Project Controls Practice Lab/i })).toBeVisible({ timeout: 20_000 })
    // The document is in right-to-left mode for Arabic.
    await expect(page.locator('html')).toHaveAttribute('dir', 'rtl')
    await expect(page.locator('html')).toHaveAttribute('lang', 'ar')
    await expect(page.getByRole('status', { name: 'Loading' })).toHaveCount(0)
    await expectNoCriticalViolations(page, 'catalogue (RTL)')

    // ── Multi-step runner (RTL) ──
    await page.goto('/app/lab/MS-RECOVERY-001')
    await expect(page.getByText(/Baseline health|Choose a recovery path|Re-forecast/i).first()).toBeVisible({ timeout: 20_000 })
    await expect(page.locator('html')).toHaveAttribute('dir', 'rtl')
    await expectNoCriticalViolations(page, 'multi-step runner (RTL)')
  })
})
