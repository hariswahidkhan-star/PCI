import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import App from './App'

// The contributor desk is REACHABLE from the dashboard.
//
// This file exists for the same reason ForumMount.test.tsx and CareersMount.test.tsx do: a
// component's own unit tests cannot notice that nothing imports it. That has already happened once
// in this increment — a composer shipped with 22 green tests and no mount — and the whole publishing
// phase existed for months as a backend with no interface at all, which is a larger version of the
// same failure. Only a test starting from the app's entry point catches it.
//
// The second assertion is the one worth keeping: the way in must NOT be hidden behind contributor
// standing, because the desk's first job is to let someone apply for that standing.

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

const NO_STANDING = {
  state: 'none', may_write: false, terms_version: null, granted_at: null,
  revoked_at: null, revoke_reason: null, terms_available: true,
}

describe('contributor desk mounting', () => {
  const seen: string[] = []

  beforeEach(() => {
    seen.length = 0
    localStorage.clear()
    localStorage.setItem('pciworld_account', 'tok')
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
      const u = String(url)
      seen.push(u)
      if (u === '/api/world/me/dashboard') return new Response(JSON.stringify(DASHBOARD), { status: 200 })
      if (u === '/api/world/contributor/me') return new Response(JSON.stringify(NO_STANDING), { status: 200 })
      if (u === '/api/world/contributor/articles') return new Response(JSON.stringify({ articles: [] }), { status: 200 })
      return new Response(JSON.stringify({ ok: true }), { status: 200 })
    }))
  })

  afterEach(() => { vi.unstubAllGlobals() })

  it('the dashboard offers a way into the writing desk, and it mounts', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Write for PCI World'))
    expect(await screen.findByRole('heading', { name: 'Write for PCI World', level: 1 })).toBeTruthy()
  })

  it('mounting really calls the contributor API', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Write for PCI World'))
    await waitFor(() => expect(seen).toContain('/api/world/contributor/me'))
  })

  it('someone with no standing still reaches the desk — applying is the point of the entry', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Write for PCI World'))
    // Gating the button on contributor standing would hide the application from everyone who does
    // not already have it, which is everyone who needs it.
    expect(await screen.findByRole('heading', { name: 'Apply to write' })).toBeTruthy()
  })

  it('leaving the desk returns to the dashboard', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Write for PCI World'))
    await screen.findByRole('heading', { name: 'Write for PCI World', level: 1 })
    fireEvent.click(screen.getByText('← Back to your dashboard'))
    expect(await screen.findByRole('heading', { name: /Welcome back/, level: 1 })).toBeTruthy()
  })
})
