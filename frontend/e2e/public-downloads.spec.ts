import { readFileSync } from 'node:fs'
import { test, expect } from '@playwright/test'
import { captureStoryEvidence, preparePublicJourney } from './util'

// Public Downloads Centre: the page shell is static (downloads-centre.html) but the document
// grid is fetched from /api/public/documents, which Data/PublicDocsSeed.cs populates on first
// boot — so a rendered card proves the API + seed pipeline, not just the HTML.
test.describe('public downloads centre', () => {
  test('the downloads centre lists seeded public documents', async ({ page }, testInfo) => {
    await preparePublicJourney(page)
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

    const downloadPromise = page.waitForEvent('download')
    await cards.first().getByRole('link', { name: 'Download' }).click()
    const download = await downloadPromise
    const path = await download.path()
    expect(path).toBeTruthy()
    // The centre serves more than one document type now — the governance PDFs plus the CSV templates
    // library — so assert the bytes match the file the card actually offered rather than assuming PDF.
    // This keeps the real point of the check (the pipeline serves genuine file content, not an error page)
    // while staying correct as the register grows.
    const bytes = readFileSync(path!)
    expect(bytes.byteLength).toBeGreaterThan(0)
    if (download.suggestedFilename().toLowerCase().endsWith('.csv')) {
      expect(bytes.toString('utf8')).toContain(',')
    } else {
      expect(bytes.subarray(0, 4).toString()).toBe('%PDF')
    }
    await captureStoryEvidence(page, testInfo, 'A8', 'download-centre')
  })

  test('the clean /downloads route serves the centre as well', async ({ page }) => {
    const resp = await page.goto('/downloads')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.locator('#dlxCount')).toHaveText(/\d+ documents?/)
    await expect(page.locator('.dlx-card').first()).toBeVisible()
  })
})
