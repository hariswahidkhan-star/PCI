import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import App from './App'

// The forum composer is REACHABLE from the dashboard.
//
// This file exists because the composer shipped unmounted: ForumApp.tsx and its 22 tests were all
// green while nothing in the app imported the component, so every one of those tests was proving
// that a piece of dead code behaved well. A component's own unit tests cannot notice that it is
// unreachable — only a test that starts from the app's entry point can, which is what this does.
//
// It also pins the split the design asks for: READING a thread is server-rendered HTML on
// /world/forum/... and needs no account, so the app owns the WRITE path only. If someone later
// moves reading into the bundle, the last assertion here should fail and prompt that conversation.

const DASHBOARD = {
  email: 'p@x.test', display_name: 'P', email_verified: true,
  primary_action: 'explore', primary_code: null,
  today: { day: 'Mon', timezone: 'UTC', paused: false, code: null, title: null, difficulty: null, est_minutes: null, state: 'none', attempt_id: null },
  passport: { state: 'draft', is_public: false, visible_evidence: 0 },
  progress: { completed: 0, industries: 0, tracks: 0 },
  streak_days: 0, week_done: 0, week_target: null,
  onboarding_state: 'completed',
  products: { pci_world: { state: 'ready' }, pci_ai: { state: 'locked' } },
  recent: [], in_progress: [], recommended: null,
}

describe('forum composer mounting', () => {
  const seen: string[] = []

  beforeEach(() => {
    seen.length = 0
    localStorage.clear()
    localStorage.setItem('pciworld_account', 'tok')
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
      const u = String(url)
      seen.push(u)
      if (u === '/api/world/me/dashboard') return new Response(JSON.stringify(DASHBOARD), { status: 200 })
      if (u === '/api/world/forum/me') return new Response(JSON.stringify({
        level: 'member', posts_are_reviewed_first: false, may_post_links: true,
        may_edit: true, may_flag: true, hourly_post_budget: 10,
      }), { status: 200 })
      if (u === '/api/world/forum/categories') return new Response(JSON.stringify({
        categories: [{ slug: 'estimating', title: 'Estimating', description: null, min_trust: 'new', read_only: false }],
      }), { status: 200 })
      return new Response(JSON.stringify({ ok: true }), { status: 200 })
    }))
  })

  afterEach(() => { vi.unstubAllGlobals() })

  it('the dashboard offers a way into the composer, and it renders', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Start a discussion'))
    // The composer's own heading — proof the real component mounted, not a placeholder.
    expect(await screen.findByRole('heading', { name: 'PCI World forum', level: 1 })).toBeTruthy()
    await screen.findByRole('heading', { name: 'Start a discussion', level: 2 })
  })

  it('mounting actually calls the forum API, so the seam is wired end to end', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Start a discussion'))
    // /me first: an expired session must say "sign in" before someone writes a post that cannot
    // be submitted. Categories follow because the composer needs somewhere to post to.
    await waitFor(() => expect(seen).toContain('/api/world/forum/me'))
    await waitFor(() => expect(seen).toContain('/api/world/forum/categories'))
    expect(await screen.findByText('Estimating')).toBeTruthy()
  })

  it('the app never fetches a transcript — reading stays on the server-rendered pages', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Start a discussion'))
    await waitFor(() => expect(seen).toContain('/api/world/forum/categories'))
    // No threads/posts READ endpoint is called. If reading ever moves into the bundle, the
    // crawlable HTML stops being the only source of truth for what is visible, and a held post
    // could be echoed by a client that disagrees with the server about publication state.
    expect(seen.filter(u => /\/threads\b|\/posts\b/.test(u))).toEqual([])
  })

  it('leaving the composer returns to the dashboard', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Start a discussion'))
    await screen.findByRole('heading', { name: 'PCI World forum', level: 1 })
    fireEvent.click(screen.getByText('← Back to your dashboard'))
    expect(await screen.findByRole('heading', { name: /Welcome back/, level: 1 })).toBeTruthy()
  })
})
