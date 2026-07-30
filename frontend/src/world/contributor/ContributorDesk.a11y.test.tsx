import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import axe from 'axe-core'
import ContributorDesk from './ContributorDesk'

// Accessibility of the contributor desk, in jsdom.
//
// colour-contrast and target-size are DISABLED here and that is not a convenience: both rules need
// layout, which jsdom does not do, so axe silently reports them as inapplicable rather than as
// passes. Leaving them on would produce a clean sweep the suite had not earned. The browser suite
// is where those two are actually measured — the same split as the forum and careers specs.
async function scan(container: HTMLElement) {
  const results = await axe.run(container, {
    // Only the rules jsdom can actually evaluate. Asking for the rest yields "incomplete" results
    // that are easy to mistake for passes.
    runOnly: { type: 'tag', values: ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'] },
    rules: {
      'color-contrast': { enabled: false },   // needs layout; browser-only
      'target-size': { enabled: false },      // needs layout; browser-only
    },
  })
  return results.violations
    .filter(v => v.impact === 'critical' || v.impact === 'serious')
    .map(v => `${v.id} (${v.impact}) ×${v.nodes.length}: ${v.help}`)
}

const ME = {
  state: 'granted', may_write: true, terms_version: 'v1', granted_at: '2026-03-04 10:00:00',
  revoked_at: null, revoke_reason: null, terms_available: true,
}
const ARTICLE = {
  id: 7, slug: 'a', title: 'Recovering a slipped programme', dek: 'What works.',
  body_md: 'Text.', status: 'drafting', version: 0, published_at: null, editable: true,
  history: [{ event: 'submitted', from: 'drafting', to: 'technical_review', by: 'contributor', note: null, at: '2026-04-01 09:00:00' }],
  messages: [{ from: 'editor', body: 'Can you attribute the figure?', at: '2026-04-02 09:00:00' }],
}

function stub(routes: Record<string, unknown>) {
  vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
    const u = String(url)
    for (const [p, body] of Object.entries(routes)) {
      if (u === p) return new Response(JSON.stringify(body), { status: 200 })
    }
    return new Response(JSON.stringify({ ok: true }), { status: 200 })
  }))
}

beforeEach(() => { localStorage.clear(); localStorage.setItem('pciworld_account', 'tok') })
afterEach(() => { vi.unstubAllGlobals() })

describe('contributor desk accessibility', () => {
  it('the application screen has no violations', async () => {
    stub({ '/api/world/contributor/me': {
      state: 'none', may_write: false, terms_version: null, granted_at: null,
      revoked_at: null, revoke_reason: null, terms_available: true } })
    const { container } = render(<ContributorDesk />)
    await screen.findByRole('heading', { name: 'Apply to write' })
    expect(await scan(container)).toEqual([])
  })

  it('the desk with pieces on it has no violations', async () => {
    stub({ '/api/world/contributor/me': ME,
           '/api/world/contributor/articles': { articles: [ARTICLE] } })
    const { container } = render(<ContributorDesk />)
    await screen.findByRole('heading', { name: 'Your pieces' })
    expect(await scan(container)).toEqual([])
  })

  it('the editor, declarations and thread have no violations', async () => {
    stub({ '/api/world/contributor/me': ME,
           '/api/world/contributor/articles': { articles: [ARTICLE] },
           '/api/world/contributor/articles/7': ARTICLE })
    const { container } = render(<ContributorDesk />)
    fireEvent.click(await screen.findByRole('button', { name: /Continue writing/ }))
    await screen.findByLabelText('The piece')
    expect(await scan(container)).toEqual([])
  })

  it('every form control is labelled — including the declarations', async () => {
    stub({ '/api/world/contributor/me': ME,
           '/api/world/contributor/articles': { articles: [ARTICLE] },
           '/api/world/contributor/articles/7': ARTICLE })
    render(<ContributorDesk />)
    fireEvent.click(await screen.findByRole('button', { name: /Continue writing/ }))
    await screen.findByLabelText('The piece')
    for (const l of [/commercial interest/i, /sponsored or paid for/i, /AI assistance/i,
                     /own original work/i, 'Title', 'Standfirst', 'Reply']) {
      expect(screen.getByLabelText(l)).toBeTruthy()
    }
  })

  it('each section is announced by its own heading', async () => {
    stub({ '/api/world/contributor/me': ME,
           '/api/world/contributor/articles': { articles: [ARTICLE] },
           '/api/world/contributor/articles/7': ARTICLE })
    render(<ContributorDesk />)
    fireEvent.click(await screen.findByRole('button', { name: /Continue writing/ }))
    await screen.findByLabelText('The piece')
    // A region labelled by a heading is how a screen-reader user moves between them; an unlabelled
    // <section> is announced as nothing at all.
    for (const n of ['Submit for review', 'Your editor', 'What has happened to this piece']) {
      expect(screen.getByRole('heading', { name: n })).toBeTruthy()
    }
  })

  it('there is exactly one h1, and it names the surface', async () => {
    stub({ '/api/world/contributor/me': ME,
           '/api/world/contributor/articles': { articles: [] } })
    const { container } = render(<ContributorDesk />)
    await screen.findByRole('heading', { name: 'Your pieces' })
    const h1s = container.querySelectorAll('h1')
    expect(h1s.length).toBe(1)
    expect(h1s[0].textContent).toBe('Write for PCI World')
  })
})
