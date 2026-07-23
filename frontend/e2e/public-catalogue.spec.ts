import { test, expect } from '@playwright/test'

interface CatalogueCertification {
  code: string
  name: string
  acronym: string
  slug: string
  exam_price: number
  duration_minutes: number
  pass_mark_pct: number
}

function amount(value: number): string {
  return Number.isInteger(value) ? String(value) : value.toFixed(2)
}

// Certification catalogue + enrolment hand-off. The catalogue cards are injected server-side
// from the certifications table (Core/CertCatalogue.cs fills the <!--PCI-CERTS--> region from
// the MultiCert seed), so a rendered card proves the DB-backed pipeline end to end.
test.describe('certification catalogue and enrolment hand-off', () => {
  test('@cross-browser every certification stays consistent from API to catalogue card to detail page', async ({ page, request }) => {
    const catalogueResponse = await request.get('/api/certifications')
    expect(catalogueResponse.ok()).toBeTruthy()
    const catalogue = (await catalogueResponse.json()) as { rows: CatalogueCertification[] }
    const certifications = catalogue.rows.filter((certification) =>
      ['PCL-AI', 'PFL-AI', 'PML-AI'].includes(certification.code),
    )
    expect(certifications.map((certification) => certification.code)).toEqual(['PCL-AI', 'PFL-AI', 'PML-AI'])

    const resp = await page.goto('/certifications.html')
    expect(resp?.status() ?? 0).toBeLessThan(400)
    await expect(page.getByRole('heading', { level: 1 }).first()).toBeVisible()
    const cards = page.locator('article.cert-card')
    await expect(cards).toHaveCount(3)

    for (const certification of certifications) {
      const card = cards.filter({ hasText: certification.acronym })
      await expect(card, `${certification.code} should have exactly one catalogue card`).toHaveCount(1)
      await expect(card.getByRole('heading', { name: certification.name })).toBeVisible()
      await expect(card).toContainText(`USD ${amount(certification.exam_price)}`)
      await expect(card).toContainText(`${certification.duration_minutes} minutes`)
      await expect(card).toContainText(`${amount(certification.pass_mark_pct)}% to pass`)
      await expect(card.getByRole('link', { name: 'Learn more' })).toHaveAttribute('href', `/certifications/${certification.slug}`)
      await expect(card.getByRole('link', { name: 'Apply now' })).toHaveAttribute(
        'href',
        `/app/register?product=exam&cert=${certification.code}`,
      )
    }

    // Follow each database-backed route. This catches a card/detail drift that a catalogue-only
    // assertion misses (wrong slug, stale name, stale fee or enrolment intent).
    for (const certification of certifications) {
      const detail = await page.goto(`/certifications/${certification.slug}`)
      expect(detail?.status() ?? 0, `${certification.code} detail route should load`).toBeLessThan(400)
      await expect(page.getByRole('heading', { level: 1, name: certification.name })).toBeVisible()
      await expect(page.locator('.cert-facts')).toContainText(`USD ${amount(certification.exam_price)}`)
      await expect(page.locator('.cert-facts')).toContainText(`${certification.duration_minutes} minutes`)
      await expect(page.getByRole('link', { name: 'Apply now' }).first()).toHaveAttribute(
        'href',
        `/app/register?product=exam&cert=${certification.code}`,
      )
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
