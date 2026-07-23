import { test, expect } from '@playwright/test'

// Certification catalogue + enrolment hand-off. The catalogue cards are injected server-side
// from the certifications table (Core/CertCatalogue.cs fills the <!--PCI-CERTS--> region from
// the MultiCert seed), so a rendered card proves the DB-backed pipeline end to end.
test.describe('certification catalogue and enrolment hand-off', () => {
  test('the catalogue page renders seeded certification cards', async ({ page }) => {
    const resp = await page.goto('/certifications.html')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
    const cards = page.locator('article.cert-card')
    await expect(cards.first()).toBeVisible()
    expect(await cards.count()).toBeGreaterThanOrEqual(1)
    // Every card links out to its own certification page.
    await expect(cards.first().getByRole('link', { name: 'Learn more' })).toBeVisible()
  })

  test('the three AI certifications appear as distinct catalogue entries', async ({ page }) => {
    await page.goto('/certifications.html')
    const cards = page.locator('article.cert-card')
    for (const code of ['PCL-AI', 'PFL-AI', 'PML-AI']) {
      await expect(cards.filter({ hasText: code })).toHaveCount(1)
    }
  })

  test('the clean /certifications route serves the same catalogue', async ({ page }) => {
    const resp = await page.goto('/certifications')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.locator('article.cert-card').first()).toBeVisible()
  })

  test('the retired checkout page hands off to portal registration, keeping the enrolment intent', async ({ page }) => {
    // checkout.html is a purposeful redirect stub: purchases now happen inside the portal, and
    // the product/cert deep-link must survive the hop so the buyer lands on the right flow.
    await page.goto('/checkout.html?product=exam&cert=pcl-ai')
    await expect(page).toHaveURL(/\/app\/register\?product=exam&cert=pcl-ai$/)
    // The registration screen renders with the deep-link intact (form ready to create the account).
    await expect(page.getByLabel('Email address')).toBeVisible()
    await expect(page.getByRole('button', { name: /create free account/i })).toBeVisible()
  })
})
