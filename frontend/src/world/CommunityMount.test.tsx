import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import App from './App'

// Community rooms must be reachable from the signed-in dashboard — the same class of bug the
// forum mount test caught: a finished surface with no way in from the product people actually use.

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

describe('community rooms mounting from the dashboard', () => {
  beforeEach(() => {
    localStorage.clear()
    localStorage.setItem('pciworld_account', 'tok')
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
      if (String(url) === '/api/world/me/dashboard') {
        return new Response(JSON.stringify(DASHBOARD), { status: 200 })
      }
      return new Response(JSON.stringify({ ok: true }), { status: 200 })
    }))
  })
  afterEach(() => { vi.unstubAllGlobals() })

  it('offers Community rooms and Browse the forum from the home dashboard', async () => {
    render(<App />)
    expect(await screen.findByRole('heading', { name: 'Talk with practitioners', level: 2 })).toBeTruthy()
    const rooms = screen.getByRole('link', { name: 'Community rooms' })
    expect(rooms.getAttribute('href')).toBe('/world-app/community')
    const forum = screen.getByRole('link', { name: 'Browse the forum' })
    expect(forum.getAttribute('href')).toBe('/world/forum')
    expect(screen.getByRole('button', { name: 'Start a discussion' })).toBeTruthy()
  })
})
