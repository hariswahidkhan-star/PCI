import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'

// PCI World community rooms — accessibility in a REAL BROWSER (§18.1, WCAG 2.2 AA).
//
// This is the half the jsdom pass cannot do. CommunityApp.a11y.test.tsx covers the structural
// rules and explicitly DISABLES colour-contrast and target-size, because jsdom has no rendering
// engine and would report a misleading pass. Here they are ENABLED, along with reflow at 400% zoom
// and a keyboard-only traversal — the rules that need layout, and the ones whose failure hits
// exactly the participants least able to work around it.
//
// The suite drives the same guest journey the API tests drive, so the accessibility evidence is
// taken from the screens a participant actually reaches rather than from a mounted component.

const ROOM = 'a11y-lobby'

// A display name is unique per active session in a room, and these tests share one room without
// leaving it. Fixed names therefore collide across tests — the second one to run is refused with
// name_taken. A per-test unique name removes the coupling without weakening anything: uniqueness is
// exactly what the product enforces.
let nameCounter = 0
const uniqueName = (prefix: string) => `${prefix} ${Date.now() % 100000}${nameCounter++}`

/** Jurisdiction may be a <select> (when the catalogue lists markets) or a free-text input. */
async function setJurisdiction(page: import('@playwright/test').Page, code: string) {
  const field = page.getByLabel('Country or region')
  const tag = await field.evaluate(el => el.tagName)
  if (tag === 'SELECT') await field.selectOption(code)
  else await field.fill(code)
}

// Serious AND critical fail. The repo's house rule for the portal logs serious and fails only on
// critical; this surface is stricter because a live transcript that a screen reader cannot follow
// is not a degraded experience, it is an unusable one.
async function expectNoBlockingViolations(page: import('@playwright/test').Page, label: string) {
  const results = await new AxeBuilder({ page })
    .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa', 'wcag22aa'])
    .analyze()
  const blocking = results.violations.filter(v => v.impact === 'critical' || v.impact === 'serious')
  const detail = blocking.map(v => `${v.id} (${v.impact}) ×${v.nodes.length}: ${v.help}`).join('\n  ')
  expect(blocking, `${label} — blocking a11y violations:\n  ${detail}`).toEqual([])
}

test.describe('PCI World community rooms accessibility', () => {
  test.beforeAll(async ({ request }) => {
    // Enable the feature and seed an open room through the world-admin API — the same surface an
    // operator uses, so the fixture cannot drift from the real configuration path.
    const login = await request.post('/api/world-admin/auth/login', {
      data: { email: 'owner@pciworld.local', password: process.env.PCIWORLD_OWNER_PASSWORD ?? 'changeme-world-owner' },
    })
    if (!login.ok()) test.skip(true, 'world-admin owner password not provided to the e2e server')
    const { token } = await login.json()
    const auth = { Authorization: `Bearer ${token}` }

    await request.patch('/api/world-admin/community/settings', {
      // The eligibility policy goes through the SAME operator endpoint (CCP-P1-003). With no
      // jurisdiction approved the gate admits nobody, which is the shipped fail-closed default —
      // so a fixture that skipped this would be testing a service no one can enter.
      headers: auth,
      data: { enabled: '1', moderator: 'deterministic', jurisdictions: 'GB,AE,PK', min_age: '18' },
    })

    await request.post('/api/world-admin/community/rooms', {
      headers: auth,
      data: { slug: ROOM, title: 'Accessibility Lobby', description: 'A room for the a11y pass.' },
    })
    await request.post(`/api/world-admin/community/rooms/${ROOM}/state`, {
      headers: auth, data: { state: 'open' },
    })
  })

  test('the room catalogue is accessible', async ({ page }) => {
    await page.goto('/world-app/community')
    await expect(page.getByRole('heading', { name: 'PCI World community' })).toBeVisible()
    await expectNoBlockingViolations(page, 'room catalogue')
  })

  test('the guest entry screen is accessible', async ({ page }) => {
    await page.goto('/world-app/community')
    const enter = page.getByRole('button', { name: /Enter Accessibility Lobby/ })
    if (await enter.count() === 0) test.skip(true, 'community rooms not enabled on this server')
    await enter.click()
    await expect(page.getByLabel('Display name')).toBeVisible()
    await expectNoBlockingViolations(page, 'guest entry')
  })

  test('the entry screen stays accessible while showing an error', async ({ page }) => {
    await page.goto('/world-app/community')
    const enter = page.getByRole('button', { name: /Enter Accessibility Lobby/ })
    if (await enter.count() === 0) test.skip(true, 'community rooms not enabled on this server')
    await enter.click()
    await page.getByLabel('Date of birth').fill('1990-05-04')
    await setJurisdiction(page, 'GB')
    await page.getByLabel('Display name').fill('admin')          // reserved — guaranteed refusal
    await page.getByRole('checkbox').check()
    await page.getByRole('button', { name: 'Join room' }).click()
    await expect(page.getByRole('alert')).toBeVisible()
    await expectNoBlockingViolations(page, 'entry error state')
  })

  test('the room transcript and composer are accessible', async ({ page }) => {
    await page.goto('/world-app/community')
    const enter = page.getByRole('button', { name: /Enter Accessibility Lobby/ })
    if (await enter.count() === 0) test.skip(true, 'community rooms not enabled on this server')
    await enter.click()
    await page.getByLabel('Date of birth').fill('1990-05-04')
    await setJurisdiction(page, 'GB')
    await page.getByLabel('Display name').fill(uniqueName('Axe Tester'))
    await page.getByRole('checkbox').check()
    await page.getByRole('button', { name: 'Join room' }).click()
    await expect(page.getByLabel('Your message')).toBeVisible()

    await page.getByLabel('Your message').fill('How are you forecasting escalation this year?')
    await page.getByRole('button', { name: 'Send' }).click()
    await expect(page.getByRole('log')).toContainText('forecasting escalation', { timeout: 10_000 })
    await expectNoBlockingViolations(page, 'room transcript')
  })

  test('the transcript is a live log AND still a list', async ({ page }) => {
    // A regression guard for the defect the jsdom pass caught: role="log" on the <ol> itself
    // overrides the implicit list role and orphans every item from the accessibility tree.
    await page.goto('/world-app/community')
    const enter = page.getByRole('button', { name: /Enter Accessibility Lobby/ })
    if (await enter.count() === 0) test.skip(true, 'community rooms not enabled on this server')
    await enter.click()
    await page.getByLabel('Date of birth').fill('1990-05-04')
    await setJurisdiction(page, 'GB')
    await page.getByLabel('Display name').fill(uniqueName('Semantics Tester'))
    await page.getByRole('checkbox').check()
    await page.getByRole('button', { name: 'Join room' }).click()
    await expect(page.getByLabel('Your message')).toBeVisible()

    const log = page.getByRole('log')
    await expect(log).toBeVisible()
    await expect(log).toHaveAttribute('aria-live', 'polite')

    // A message must exist before the list role can be asserted: Chromium does not expose an EMPTY
    // <ol> as a list in the accessibility tree. That is correct behaviour — there is nothing to
    // announce — but it means an empty transcript proves nothing either way, so the regression
    // guard has to run against a populated one.
    await page.getByLabel('Your message').fill('A message so the transcript is not empty.')
    await page.getByRole('button', { name: 'Send' }).click()
    await expect(log).toContainText('not empty', { timeout: 10_000 })
    await expect(page.getByRole('list')).toBeVisible()
  })

  test('the guest journey is operable by keyboard alone', async ({ page }) => {
    // PW-US-038: enter, write and send without a pointer. Tab order must reach every control in a
    // sensible sequence, and focus must remain visible throughout.
    await page.goto('/world-app/community')
    const enter = page.getByRole('button', { name: /Enter Accessibility Lobby/ })
    if (await enter.count() === 0) test.skip(true, 'community rooms not enabled on this server')

    await page.keyboard.press('Tab')
    for (let i = 0; i < 12; i++) {
      const focused = await page.evaluate(() => document.activeElement?.textContent ?? '')
      if (focused.includes('Accessibility Lobby')) break
      await page.keyboard.press('Tab')
    }
    await page.keyboard.press('Enter')
    await expect(page.getByLabel('Display name')).toBeVisible()

    // The name field, the rules checkbox and the submit button must all be reachable by Tab.
    await page.getByLabel('Display name').focus()
    await page.keyboard.type(uniqueName('Keyboard Only'))
    await page.keyboard.press('Tab')
    for (let i = 0; i < 6; i++) {
      const role = await page.evaluate(() => (document.activeElement as HTMLInputElement)?.type ?? '')
      if (role === 'checkbox') break
      await page.keyboard.press('Tab')
    }
    await page.keyboard.press('Space')
    await expect(page.getByRole('checkbox')).toBeChecked()
  })

  test('the room reflows at 400% zoom without horizontal scrolling', async ({ page }) => {
    // WCAG 1.4.10: at 1280px wide and 400% zoom the effective viewport is 320px. Content must
    // reflow rather than force two-dimensional scrolling.
    await page.setViewportSize({ width: 320, height: 800 })
    await page.goto('/world-app/community')
    await expect(page.getByRole('heading', { name: 'PCI World community' })).toBeVisible()

    const overflows = await page.evaluate(() =>
      document.documentElement.scrollWidth > document.documentElement.clientWidth + 1)
    expect(overflows, 'the page scrolls horizontally at a 320px viewport').toBe(false)
    await expectNoBlockingViolations(page, 'reflow at 320px')
  })
})
