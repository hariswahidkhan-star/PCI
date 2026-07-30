import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'

// PCI World forum — accessibility of the SERVER-RENDERED thread page, in a real browser.
//
// This is the counterpart to the jsdom scans elsewhere, and it exists for the rules jsdom cannot
// evaluate: colour contrast, target size and reflow all need layout, and a scan that silently skips
// them reports a pass it has not earned.
//
// The fixture is a real journey rather than a database fixture, deliberately. A new account's first
// post is HELD, so the only way to get a crawlable thread is for a moderator to release it — which
// is exactly what will happen in production, and exercising it here means the a11y scan runs
// against a page produced the way real pages are produced.

const OWNER_PW = process.env.PCIWORLD_OWNER_PASSWORD ?? 'changeme-world-owner'
const CAT = 'a11y-estimating'
const unique = (s: string) => `${s} ${Date.now().toString(36)}${Math.random().toString(36).slice(2, 6)}`

async function blockingViolations(page: import('@playwright/test').Page, label: string) {
  const results = await new AxeBuilder({ page })
    .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
    .analyze()
  const blocking = results.violations.filter(v => v.impact === 'critical' || v.impact === 'serious')
  return blocking.map(v => `${label}: ${v.id} (${v.impact}) ×${v.nodes.length}: ${v.help}`)
}

test.describe('PCI World forum accessibility', () => {
  let threadUrl = ''

  test.beforeAll(async ({ request }) => {
    const login = await request.post('/api/world-admin/auth/login', {
      data: { email: 'owner@pciworld.local', password: OWNER_PW },
    })
    // These guards THROW rather than skip. A fixture that cannot be built is a broken test, and a
    // silently skipped accessibility suite reports a clean sweep it never ran — the exact vacuous
    // pass this whole file exists to avoid. The message carries the status and body so the cause is
    // in the failure, not in a second debugging round.
    if (!login.ok()) throw new Error(`world-admin login failed: ${login.status()} ${await login.text()}`)
    const { token } = await login.json()
    const auth = { Authorization: `Bearer ${token}` }

    // Enabling the forum goes through the same settings surface an operator uses, so the fixture
    // cannot drift from the real configuration path.
    await request.patch('/api/world-admin/community/settings', {
      headers: auth, data: { enabled: '1', moderator: 'deterministic', forum_enabled: '1' },
    })
    const enable = await request.post('/api/world-admin/forum/categories', {
      headers: auth, data: { slug: CAT, title: 'Estimating', min_trust_to_post: 'new' },
    })
    // A repeat run finds the category already there; that is fine, not a failure.
    if (!enable.ok() && enable.status() !== 409)
      throw new Error(`category create failed: ${enable.status()} ${await enable.text()}`)

    // A member registers and starts a thread. Their first post is held — which is the point.
    const email = `a11y-${Date.now()}@forum.test`
    const reg = await request.post('/api/world/account/register', {
      data: { email, password: 'ForumA11yPass!1', display_name: 'A11y Tester' },
    })
    if (!reg.ok()) throw new Error(`world account register failed: ${reg.status()} ${await reg.text()}`)
    const { token: userToken } = await reg.json()

    const created = await request.post(`/api/world/forum/categories/${CAT}/threads`, {
      headers: { 'X-World-Account': userToken },
      data: {
        title: unique('How do you model escalation on a long job'),
        body: 'We used a blended index rather than a single CPI series, and rebaselined quarterly.',
      },
    })
    if (!created.ok()) throw new Error(`thread creation refused: ${created.status()} ${await created.text()}`)
    const thread = await created.json()
    threadUrl = thread.url

    // Held, as expected for a new account — so a moderator releases it, which is the journey.
    const queue = await request.get('/api/world-admin/forum/queue', { headers: auth })
    const { posts } = await queue.json()
    for (const p of posts) {
      await request.post(`/api/world-admin/forum/posts/${p.id}/release`, {
        headers: auth, data: { reason: 'fixture' },
      })
    }
  })

  test('a released thread is actually published and readable', async ({ page }) => {
    // Guards the fixture. Without this, every scan below could be passing against a 404 — an empty
    // page has no accessibility violations, and would report a clean sweep it has not earned.
    const res = await page.goto(threadUrl)
    expect(res?.status()).toBe(200)
    await expect(page.getByText('blended index')).toBeVisible()
  })

  test('the thread page is a complete, titled document', async ({ page }) => {
    await page.goto(threadUrl)
    // The three things a bare fragment lacks, and the reason the renderer emits a whole document.
    await expect(page).toHaveTitle(/PCI World forum/)
    expect(await page.locator('html').getAttribute('lang')).toBe('en')
    expect(await page.locator('link[rel="canonical"]').count()).toBe(1)
  })

  test('the stylesheet actually loads, so the contrast scan is not vacuous', async ({ page }) => {
    // The scan below enables colour-contrast. On an UNSTYLED page that is black on white and passes
    // trivially — which is what was happening before /assets/world.css existed, and a pass the
    // suite had not earned. This guards the guard.
    await page.goto(threadUrl)
    const colour = await page.evaluate(() => getComputedStyle(document.body).color)
    expect(colour, 'body colour is the UA default — world.css did not load').not.toBe('rgb(0, 0, 0)')
  })

  test('the thread page passes an automated WCAG 2.x AA scan', async ({ page }) => {
    // Contrast, target size and reflow are all ENABLED here — these are precisely the rules jsdom
    // cannot evaluate, which is why this spec exists alongside the jsdom ones.
    await page.goto(threadUrl)
    expect(await blockingViolations(page, 'thread page')).toEqual([])
  })

  test('the discussion has one h1 and a labelled breadcrumb', async ({ page }) => {
    await page.goto(threadUrl)
    await expect(page.locator('h1')).toHaveCount(1)
    await expect(page.getByRole('navigation', { name: 'Breadcrumb' })).toBeVisible()
  })

  test('the transcript is a list, so a screen reader announces its length', async ({ page }) => {
    await page.goto(threadUrl)
    await expect(page.getByRole('list').filter({ has: page.locator('.wf-post') })).toHaveCount(1)
  })

  test('the skip link reaches the discussion by keyboard', async ({ page }) => {
    await page.goto(threadUrl)
    await page.keyboard.press('Tab')
    const href = await page.evaluate(() => (document.activeElement as HTMLAnchorElement)?.getAttribute('href'))
    expect(href).toBe('#wf-main')
    await page.keyboard.press('Enter')
    // The target must exist, or the skip link is a promise the page does not keep.
    await expect(page.locator('#wf-main')).toBeVisible()
  })

  test('the page reflows at 400% zoom without horizontal scrolling', async ({ page }) => {
    // WCAG 1.4.10: 1280px at 400% zoom is a 320px effective viewport. A forum page is mostly prose,
    // which makes a long unbroken URL or code span the realistic way this breaks.
    await page.setViewportSize({ width: 320, height: 800 })
    await page.goto(threadUrl)
    await expect(page.locator('h1')).toBeVisible()

    const overflows = await page.evaluate(() =>
      document.documentElement.scrollWidth > document.documentElement.clientWidth + 1)
    expect(overflows, 'the page scrolls horizontally at a 320px viewport').toBe(false)
    expect(await blockingViolations(page, 'reflow at 320px')).toEqual([])
  })

  test('structured data is present and parses', async ({ page }) => {
    // Emitted for crawlers, so nobody looks at it — which is exactly how it rots. Parsing it here
    // means a malformed blob fails a test rather than silently degrading search results.
    await page.goto(threadUrl)
    const blobs = await page.locator('script[type="application/ld+json"]').allTextContents()
    expect(blobs.length).toBeGreaterThan(0)
    const parsed = blobs.map(b => JSON.parse(b))
    expect(parsed.some(p => p['@type'] === 'DiscussionForumPosting' || p['@type'] === 'QAPage')).toBe(true)
    expect(parsed.some(p => p['@type'] === 'BreadcrumbList')).toBe(true)
  })
})
