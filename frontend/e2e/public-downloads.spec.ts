import { test, expect } from '@playwright/test'

// Public Downloads Centre: the page shell is static (downloads-centre.html) but the document
// grid is fetched from /api/public/documents, which Data/PublicDocsSeed.cs populates on first
// boot — so a rendered card proves the API + seed pipeline, not just the HTML.
test.describe('public downloads centre', () => {
  test('the downloads centre lists seeded public documents', async ({ page }) => {
    const resp = await page.goto('/downloads-centre.html')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()

    // The count label settles once the fetch resolves ("N documents"), and cards render.
    await expect(page.locator('#dlxCount')).toHaveText(/\d+ documents?/)
    const cards = page.locator('.dlx-card')
    await expect(cards.first()).toBeVisible()
    // Each card offers both a View and a Download action for its file.
    await expect(cards.first().getByRole('link', { name: 'View' })).toBeVisible()
    await expect(cards.first().getByRole('link', { name: 'Download' })).toBeVisible()
  })

  test('the clean /downloads route serves the centre as well', async ({ page }) => {
    const resp = await page.goto('/downloads')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.locator('#dlxCount')).toHaveText(/\d+ documents?/)
    await expect(page.locator('.dlx-card').first()).toBeVisible()
  })
})
