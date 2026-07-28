import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen } from '@testing-library/react'
import axe from 'axe-core'
import ForumApp from './ForumApp'

// PCI World forum composer — automated accessibility checks (§18.1, WCAG 2.2 AA).
//
// WHAT THIS COVERS AND WHAT IT DOES NOT — stated up front, because an a11y suite that implies more
// than it checks is worse than none.
//
// COVERED: the structural rules, which are the ones this interface most plausibly gets wrong —
// every control has an accessible name, form fields are labelled and their help text associated,
// ARIA attributes are valid for their roles, headings and landmarks are ordered, and the disabled
// category radios still expose their explanation.
//
// NOT COVERED: colour contrast, focus-visible appearance, reflow at 320px, and anything else
// needing real layout — jsdom has no rendering engine, so axe cannot evaluate them and silently
// skips them. Those need the browser pass the design's §9 test plan calls for, which stays a
// recorded gap rather than being implied by this file passing.
//
// The run fails on `critical` and `serious`, matching the community suite: for the structural
// rules axe CAN evaluate here, "serious" means a screen-reader user is genuinely blocked.

const CATS = {
  categories: [
    { slug: 'general', title: 'General', description: 'Anything project controls', min_trust: 'new', read_only: false },
    { slug: 'announcements', title: 'Announcements', description: null, min_trust: 'staff', read_only: false },
    { slug: 'archive', title: 'Archive', description: null, min_trust: 'new', read_only: true },
  ],
}

const ME_NEW = {
  level: 'new', posts_are_reviewed_first: true, may_post_links: false,
  may_edit: false, may_flag: false, hourly_post_budget: 3,
}
const ME_MEMBER = {
  level: 'member', posts_are_reviewed_first: false, may_post_links: true,
  may_edit: true, may_flag: true, hourly_post_budget: 30,
}

async function scan(container: HTMLElement) {
  const results = await axe.run(container, {
    // Only the rules jsdom can actually evaluate. Asking for the rest would produce "incomplete"
    // results that are easy to mistake for passes.
    runOnly: { type: 'tag', values: ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'] },
    rules: {
      'color-contrast': { enabled: false },   // needs layout; jsdom cannot evaluate it — browser-only
      'target-size': { enabled: false },      // needs layout; jsdom cannot evaluate it — browser-only
    },
  })
  const blocking = results.violations.filter(v => v.impact === 'critical' || v.impact === 'serious')
  return blocking.map(v => `${v.id} (${v.impact}) ×${v.nodes.length}: ${v.help}`)
}

describe('World forum composer — accessibility', () => {
  let responder: (url: string, method: string) => unknown

  beforeEach(() => {
    localStorage.clear()
    localStorage.setItem('pciworld_account', 'tok-account')
    responder = url => {
      if (url.includes('/forum/me')) return ME_NEW
      if (url.includes('/forum/categories')) return CATS
      return {}
    }
    vi.stubGlobal('fetch', vi.fn(async (url: string, init?: RequestInit) => {
      const payload = responder(url, init?.method ?? 'GET')
      const status = (payload as { _status?: number })?._status ?? 200
      return { ok: status < 400, status, text: async () => JSON.stringify(payload) } as unknown as Response
    }))
  })
  afterEach(() => vi.unstubAllGlobals())

  it('the thread composer has no blocking violations', async () => {
    // Scanned as a NEW member, deliberately: that state carries the review-first notice and the
    // disabled category radios with their explanations — the richest structure this surface has.
    const { container } = render(<ForumApp />)
    await screen.findByLabelText('Title')
    expect(screen.getByRole('radio', { name: 'Announcements' })).toBeDisabled()
    expect(await scan(container)).toEqual([])
  })

  it('the composer stays accessible while reporting a held post', async () => {
    // The state a person is actually in when they most need the semantics: their post has just
    // been held and the only account of what happened is the status region.
    const { container } = render(<ForumApp />)
    await screen.findByLabelText('Title')
    responder = (url, method) => {
      if (url.includes('/threads') && method === 'POST') {
        return {
          ok: true, thread_id: 5, slug: 't', url: '/world/forum/general/t', state: 'pending',
          published: false, notice: 'Your thread is waiting for a moderator. New accounts are reviewed before publication.',
        }
      }
      return url.includes('/forum/me') ? ME_NEW : CATS
    }
    fireEvent.click(screen.getByRole('radio', { name: 'General' }))
    fireEvent.change(screen.getByLabelText('Title'), { target: { value: 'A perfectly good title' } })
    fireEvent.change(screen.getByLabelText('Your post'), { target: { value: 'A body.' } })
    fireEvent.click(screen.getByRole('button', { name: 'Post discussion' }))
    await screen.findByRole('status')
    expect(await scan(container)).toEqual([])
  })

  it('the reply composer has no blocking violations', async () => {
    const { container } = render(<ForumApp threadId={9} threadTitle="Escalation practice" />)
    await screen.findByLabelText('Your reply')
    expect(await scan(container)).toEqual([])
  })

  it('the post tools with an open edit form have no blocking violations', async () => {
    responder = url => (url.includes('/forum/me') ? ME_MEMBER : {})
    const { container } = render(
      <ForumApp threadId={9} posts={[
        { id: 4, mine: true, body: 'original text' },
        { id: 5, mine: false, body: '' },
      ]} />,
    )
    // Guard the fixture: if the edit form did not open, the scan below proves nothing about it.
    fireEvent.click(await screen.findByRole('button', { name: 'Edit post 4' }))
    expect(screen.getByLabelText('New text for post 4')).toBeTruthy()
    expect(await scan(container)).toEqual([])
  })

  it('the flag form has no blocking violations', async () => {
    responder = url => (url.includes('/forum/me') ? ME_MEMBER : {})
    const { container } = render(<ForumApp threadId={9} posts={[{ id: 5, mine: false, body: '' }]} />)
    fireEvent.click(await screen.findByRole('button', { name: 'Flag post 5' }))
    expect(screen.getByLabelText('Why should a moderator look at post 5?')).toBeTruthy()
    expect(await scan(container)).toEqual([])
  })

  it('the signed-out view has no blocking violations', async () => {
    responder = url => (url.includes('/forum/me') ? { _status: 401, error: 'no_session' } : CATS)
    const { container } = render(<ForumApp />)
    await screen.findByText(/sign in to your PCI World account/i)
    expect(await scan(container)).toEqual([])
  })
})
