import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import App from './App'

// The careers surfaces are REACHABLE from the dashboard.
//
// This file exists for the same reason ForumMount.test.tsx does: the forum composer once shipped
// unmounted — 22 green tests on a component nothing imported. A component's own tests cannot
// notice that it is dead code; only a test that starts from the app's entry point can. So this
// proves the dashboard offers a way into BOTH careers surfaces, that the real components mount
// (their own headings and API calls, not placeholders), and that leaving returns to the dashboard.
//
// It also pins the Phase 4 split: public job READING is server-rendered on /world/careers/... and
// needs no bundle, so the app owns only the authenticated employer portal and the applicant's own
// record. If public reading ever moves into the bundle, the last assertion should fail and prompt
// that conversation.

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

describe('careers surfaces mounting', () => {
  const seen: string[] = []

  beforeEach(() => {
    seen.length = 0
    localStorage.clear()
    localStorage.setItem('pciworld_account', 'tok')
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
      const u = String(url)
      seen.push(u)
      if (u === '/api/world/me/dashboard') return new Response(JSON.stringify(DASHBOARD), { status: 200 })
      if (u === '/api/world/careers/employers') return new Response(JSON.stringify({
        employers: [{ id: 1, slug: 'acme', name: 'Acme Controls', state: 'draft', website: null, version: 1, role: 'owner', may_publish: false }],
      }), { status: 200 })
      if (u === '/api/world/careers/my/applications') return new Response(JSON.stringify({ applications: [] }), { status: 200 })
      return new Response(JSON.stringify({ ok: true }), { status: 200 })
    }))
  })

  afterEach(() => { vi.unstubAllGlobals() })

  it('the dashboard offers a way into the employer portal, and it renders against the API', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Employer portal'))
    // The portal's own heading and a section only the real component renders — proof the real
    // component mounted, not a placeholder.
    expect(await screen.findByRole('heading', { name: 'Employer portal', level: 1 })).toBeTruthy()
    await screen.findByRole('heading', { name: 'Create an organisation', level: 2 })
    // And it is wired end to end: the scoped employer list was actually requested.
    await waitFor(() => expect(seen).toContain('/api/world/careers/employers'))
    expect(await screen.findByRole('button', { name: 'Act for Acme Controls' })).toBeTruthy()
  })

  it('the dashboard offers a way into my applications, and it renders against the API', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('My job applications'))
    expect(await screen.findByRole('heading', { name: 'My applications', level: 1 })).toBeTruthy()
    await waitFor(() => expect(seen).toContain('/api/world/careers/my/applications'))
    expect(await screen.findByText('You have not applied to any postings yet.')).toBeTruthy()
  })

  it('leaving a careers surface returns to the dashboard', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Employer portal'))
    await screen.findByRole('heading', { name: 'Employer portal', level: 1 })
    fireEvent.click(screen.getByText('← Back to your dashboard'))
    expect(await screen.findByRole('heading', { name: /Welcome back/, level: 1 })).toBeTruthy()
  })

  it('the app never fetches public job pages — reading stays on the server-rendered pages', async () => {
    render(<App />)
    fireEvent.click(await screen.findByText('Employer portal'))
    await waitFor(() => expect(seen).toContain('/api/world/careers/employers'))
    // Only the authenticated /api/world/careers/* surface is touched. If a /world/careers page
    // fetch ever appears here, the crawlable HTML has stopped being the sole public read path.
    expect(seen.filter(u => u.startsWith('/world/careers'))).toEqual([])
  })
})
