import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'

// PCI World — public journey + separate admin (docs/pciworld/PLAN.md test plan).
//
// The /world pages are server-rendered by the backend directly (no SPA build involved), so these
// specs run against the same webServer as the rest of the suite. They prove the four product
// rules end-to-end in a real browser: the anonymous journey works without an account, answers
// never appear on public surfaces, the Institute relationship is visible on every page, and the
// PCI World admin is a separate realm with its own login.

test.describe('PCI World — public journey', () => {
  test('anonymous visitor completes a challenge and shares a verified result', async ({ page }) => {
    await page.goto('/world')
    await expect(page).toHaveTitle(/PCI World/)
    // Institute relationship: header link + operated-by footer are product law on every page.
    await expect(page.locator('header.world a.ext')).toContainText('Visit the Project Controls Institute')
    await expect(page.locator('footer.world')).toContainText('operated by the Project Controls Institute')

    // A stable, known challenge (the daily rotation varies; the archive keeps everything playable).
    await page.goto('/world/challenge/WC-EVM-001')
    await expect(page.locator('h1')).toContainText('The metro package is bleeding money')
    await expect(page.locator('main')).toContainText('not PCI certification examinations')

    await page.getByRole('button', { name: 'Start the challenge' }).click()
    await expect(page.locator('#evidence tbody tr')).toHaveCount(5)

    // Reference answers for the pinned v1 content — grading is deterministic.
    await page.locator('#ask_sv').fill('-420000')
    await page.locator('#ask_cv').fill('-320000')
    await page.locator('#ask_spi').fill('0.9')
    await page.locator('#ask_cpi').fill('0.92')
    await page.getByRole('radio').nth(1).check()   // option b — the measured-indices briefing
    await page.getByRole('button', { name: 'Submit my answers' }).click()

    const result = page.locator('#result')
    await expect(result).toContainText('Your result')
    await expect(result.locator('.score').first()).toHaveText('100')
    await expect(result).toContainText('Decision replay')
    await expect(result).toContainText('Consequence')

    // Mint the verified public link and open it: name-free, answer-free, Institute-linked.
    await result.getByRole('button', { name: 'Get my verified result link' }).click()
    const shareHref = await result.locator('#sharebox a[href^="/world/r/"]').first().getAttribute('href')
    expect(shareHref).toBeTruthy()
    await page.goto(shareHref!)
    await expect(page.locator('main')).toContainText('A PCI World participant')
    await expect(page.locator('main')).toContainText('Verified by PCI World')
    const publicHtml = await page.locator('main').innerHTML()
    expect(publicHtml).not.toContain('-420000')            // answers never appear publicly
    expect(publicHtml).not.toContain('measured indices')   // nor chosen decisions
  })

  test('archive filters narrow the catalogue server-side', async ({ page }) => {
    await page.goto('/world/archive')
    await expect(page.locator('main')).toContainText('30 challenges')
    await page.locator('#f_dif').selectOption('expert')
    await page.getByRole('button', { name: 'Filter' }).click()
    await expect(page.locator('main')).toContainText('2 challenges')
    await expect(page.locator('main')).toContainText('The wind farm at the crossroads')
  })

  test('yes/no questions render real choices and forgiving number entry grades correctly', async ({ page }) => {
    await page.goto('/world/challenge/WC-WBS-026')
    await page.getByRole('button', { name: 'Start the challenge' }).click()
    // The 100%-rule question is a radio pair — never a numeric keypad text box.
    const yes = page.getByRole('radio', { name: 'Yes' })
    await expect(yes).toBeVisible()
    await expect(page.locator('#ask_hundred_percent_valid')).toHaveCount(0)
    // Numbers typed the way the evidence shows them (with thousands separators) must grade.
    await page.locator('#ask_root_total').fill('1,200,000')
    await yes.check()
    await page.getByRole('button', { name: 'Submit my answers' }).click()
    const rows = page.locator('#result table tbody tr')
    await expect(rows).toHaveCount(2)
    await expect(rows.nth(0)).toContainText('Correct')
    await expect(rows.nth(1)).toContainText('Correct')
    await expect(rows.nth(1)).toContainText('yes')
  })

  test('a participant can report a content issue from the workspace', async ({ page }) => {
    await page.goto('/world/challenge/WC-SCH-002')
    await page.getByText('Report an issue with this challenge').click()
    await page.locator('#rep_cat').selectOption('calculation')
    await page.locator('#rep_msg').fill('The float question wording could name the units explicitly.')
    await page.getByRole('button', { name: 'Send report' }).click()
    await expect(page.locator('#rep_out')).toContainText('ref WR-')
  })

  test('the home page passes an automated WCAG 2.x AA scan', async ({ page }) => {
    await page.goto('/world')
    const results = await new AxeBuilder({ page }).withTags(['wcag2a', 'wcag2aa']).analyze()
    expect(results.violations).toEqual([])
  })
})

test.describe('PCI World — separate admin realm', () => {
  test('world admin signs in on its own login and sees the governed catalogue', async ({ page }) => {
    await page.goto('/world-admin')
    await expect(page).toHaveTitle('PCI World Administration')
    await page.locator('#em').fill('owner@pciworld.local')
    await page.locator('#pw').fill(process.env.PCIWORLD_OWNER_PASSWORD || 'changeme-world-owner')
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.locator('#tab-overview')).toContainText('servable 30')

    // The reports queue is reachable and renders (empty or with the public test's report).
    await page.getByRole('tab', { name: 'Reports' }).click()
    await expect(page.locator('#tab-reports h2')).toContainText('Open content reports')
  })

  test('the PCI admin SPA carries no PCI World navigation', async ({ page }) => {
    // Structural separation: the Institute admin never links into PCI World administration.
    await page.goto('/admin/')
    const html = await page.content()
    expect(html.toLowerCase()).not.toContain('world-admin')
    expect(html).not.toContain('PCI World')
  })
})
