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
    await expect(page.locator('main')).toContainText('50 challenges')
    await page.locator('#f_dif').selectOption('expert')
    await page.getByRole('button', { name: 'Filter' }).click()
    await expect(page.locator('main')).toContainText('5 challenges')
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

  // Scanning only the home page is how three contrast failures survived the Phase 0 audit: they
  // render on the result, Passport and account surfaces, none of which were ever scanned.
  for (const [name, path] of [
    ['home', '/world'],
    ['challenge workspace', '/world/challenge/WC-EVM-001'],
    ['archive', '/world/archive'],
    ['about', '/world/about'],
    ['account', '/world/account'],
  ] as const) {
    test(`the ${name} page passes an automated WCAG 2.x AA scan`, async ({ page }) => {
      await page.goto(path)
      const results = await new AxeBuilder({ page }).withTags(['wcag2a', 'wcag2aa']).analyze()
      expect(results.violations).toEqual([])
    })
  }

  test('the completed-result view passes an automated WCAG 2.x AA scan', async ({ page }) => {
    // The result is rendered client-side after grading, so it needs a real completed attempt.
    await page.goto('/world/challenge/WC-WBS-026')
    await page.getByRole('button', { name: 'Start the challenge' }).click()
    await page.locator('#ask_root_total').fill('1200000')
    await page.getByRole('radio', { name: 'Yes' }).check()
    await page.getByRole('button', { name: 'Submit my answers' }).click()
    await expect(page.locator('#result')).toContainText('Your result')
    const results = await new AxeBuilder({ page }).withTags(['wcag2a', 'wcag2aa']).analyze()
    expect(results.violations).toEqual([])
  })

  test('a Passport link that does not resolve is reported without revealing whether it ever did', async ({ page }) => {
    await page.goto('/world/verify')
    await expect(page.locator('h1')).toContainText('Verify a PCI World Passport')
    await page.locator('#vt').fill('https://example.test/world/p/' + 'f'.repeat(64))
    await page.getByRole('button', { name: 'Verify' }).click()
    await expect(page.locator('main')).toContainText('does not resolve')
    // "withdrawn" / "expired" would confirm a Passport once existed at that address.
    const html = await page.locator('main').innerHTML()
    expect(html).not.toContain('was withdrawn')
    expect(html).not.toContain('has expired')
  })

  test('a new account can set field-level Passport disclosure', async ({ page }) => {
    await page.goto('/world/account')
    const email = `e2e-passport-${Date.now()}@example.test`
    await page.locator('#r_email').fill(email)
    await page.locator('#r_pw').fill('a-long-enough-password')
    await page.locator('#r_name').fill('E2E Participant')
    await page.getByRole('button', { name: 'Create account' }).click()

    // Disclosure is per field, not one switch: publishing what you practised must not force
    // publishing your scores.
    const scores = page.locator('#sw_scores')
    await expect(scores).toBeVisible()
    await expect(scores).toBeChecked()
    await scores.uncheck()
    await page.locator('#sw_exp').selectOption('90')
    await page.getByRole('button', { name: 'Save these settings' }).click()
    await expect(page.locator('#showmsg')).toContainText('Saved')
    await expect(page.locator('#sw_scores')).not.toBeChecked()   // survives the reload
    await expect(page.locator('main')).toContainText('Current link expires')
  })

  test('the skip link becomes visible on focus and moves focus to the content', async ({ page }) => {
    await page.goto('/world')
    await page.keyboard.press('Tab')                       // the skip link is the first stop
    const skip = page.getByRole('link', { name: 'Skip to content' })
    await expect(skip).toBeFocused()
    await expect(skip).toBeVisible()                       // it must not stay clipped when focused
    await page.keyboard.press('Enter')
    await expect(page.locator('#main')).toBeFocused()
  })

  test('starting and submitting a challenge keeps focus inside the page', async ({ page }) => {
    // Hiding the container that holds the focused control drops focus to <body>, stranding
    // keyboard and screen-reader users at the top of the document with no announcement.
    await page.goto('/world/challenge/WC-WBS-026')
    await page.getByRole('button', { name: 'Start the challenge' }).click()
    await expect(page.locator('#work')).toBeFocused()
    await page.locator('#ask_root_total').fill('1200000')
    await page.getByRole('radio', { name: 'Yes' }).check()
    await page.getByRole('button', { name: 'Submit my answers' }).click()
    await expect(page.locator('#result')).toBeFocused()
  })
})

test.describe('PCI World — separate admin realm', () => {
  test('world admin signs in on its own login and sees the governed catalogue', async ({ page }) => {
    await page.goto('/world-admin')
    await expect(page).toHaveTitle('PCI World Administration')
    await page.locator('#em').fill('owner@pciworld.local')
    await page.locator('#pw').fill(process.env.PCIWORLD_OWNER_PASSWORD || 'changeme-world-owner')
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.locator('#tab-overview')).toContainText('servable 50')

    // The reports queue is reachable and renders (empty or with the public test's report).
    await page.getByRole('tab', { name: 'Reports' }).click()
    await expect(page.locator('#tab-reports h2')).toContainText('Open content reports')
  })

  test('the rotation console shows the recorded day and substitutes without erasing it', async ({ page }) => {
    await page.goto('/world-admin')
    await page.locator('#em').fill('owner@pciworld.local')
    await page.locator('#pw').fill(process.env.PCIWORLD_OWNER_PASSWORD || 'changeme-world-owner')
    await page.getByRole('button', { name: 'Sign in' }).click()
    await expect(page.locator('#tab-overview')).toContainText('servable 50')

    await page.getByRole('tab', { name: 'Rotation' }).click()
    const panel = page.locator('#tab-rotation')
    await expect(panel.getByRole('heading', { name: 'Daily rotation' })).toBeVisible()
    // The day is a recorded period with a cycle position — not a value recomputed per request.
    await expect(panel).toContainText('cycle')
    await expect(panel).toContainText('Recorded days')

    // Re-running the boundary is idempotent: the scheduler log records the no-op rather than
    // opening the day twice.
    await panel.getByRole('button', { name: 'Run the boundary now' }).click()
    await expect(panel).toContainText('skipped_exists')

    // A substitution needs a reason, and the day it replaces stays in the ledger.
    await panel.locator('#sub_code').fill('WC-EVM-001')
    await panel.locator('#sub_why').fill('E2E: verifying the substitution ledger')
    await panel.getByRole('button', { name: 'Substitute' }).click()
    await expect(panel).toContainText('substitution')
    await expect(panel).toContainText('(superseded)')
  })

  test('the PCI admin SPA carries no PCI World navigation', async ({ page }) => {
    // Structural separation: the Institute admin never links into PCI World administration.
    await page.goto('/admin/')
    const html = await page.content()
    expect(html.toLowerCase()).not.toContain('world-admin')
    expect(html).not.toContain('PCI World')
  })
})
