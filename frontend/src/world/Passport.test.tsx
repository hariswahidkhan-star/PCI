import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import Passport from './Passport'

// The Passport manager's client contract: paged evidence appends without double counting,
// visibility toggles hit the endpoint with the right payload, publish refusals render in place,
// withdrawal is confirm-gated, and disclosure patches carry ONLY the changed key (P0-06).

function page(offset: number, total: number, rows: number[]) {
  return {
    display_name: 'P', email_verified: true, passport_public: false,
    completed: total, industries: 2, tracks: 1,
    evidence_total: total, evidence_offset: offset,
    evidence: rows.map(i => ({
      attempt_id: i, title: `Challenge ${i}`, code: `WC-${i}`, version: 1,
      difficulty: 'foundation', score: 70, completed_at: '2026-07-01', passport_visible: i % 2 === 0,
    })),
    show_scores: false, show_profiles: false, show_dates: false,
    expires_at: null, expired: false,
  }
}

describe('World Passport manager', () => {
  const calls: { url: string; method: string; body: unknown }[] = []
  let responder: (url: string) => unknown

  beforeEach(() => {
    calls.length = 0
    responder = (url) => url.includes('offset=2') ? page(2, 4, [3, 4]) : page(0, 4, [1, 2])
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      const u = String(url)
      calls.push({ url: u, method: init?.method ?? 'GET', body: init?.body ? JSON.parse(String(init.body)) : undefined })
      const body = u.startsWith('/api/world/passport?') || (u === '/api/world/passport' && (init?.method ?? 'GET') === 'GET')
        ? responder(u) : { ok: true, url: '/world/p/tok' }
      return new Response(JSON.stringify(body), { status: 200 })
    }))
    localStorage.clear()
  })

  afterEach(() => { vi.unstubAllGlobals() })

  it('Load older appends the next window without double counting', async () => {
    render(<Passport />)
    await screen.findByText(/showing 2 of 4/)
    fireEvent.click(screen.getByText('Load older evidence'))
    await screen.findByText(/showing 4 of 4/)
    expect(calls.some(c => c.url === '/api/world/passport?offset=2')).toBe(true)
    expect(screen.getAllByText(/Challenge \d/)).toHaveLength(4)
    expect(screen.queryByText('Load older evidence')).toBeNull()
  })

  it('a visibility toggle posts attempt_id + the NEW state', async () => {
    render(<Passport />)
    await screen.findByText(/Challenge 1/)
    fireEvent.click(screen.getByLabelText(/Challenge 1/))
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/passport/evidence')).toBe(true))
    expect(calls.find(c => c.url === '/api/world/passport/evidence')!.body).toEqual({ attempt_id: 1, visible: true })
  })

  it('a publish refusal renders the server message in place', async () => {
    render(<Passport />)
    await screen.findByText('Publish my Passport')
    vi.stubGlobal('fetch', vi.fn(async () =>
      new Response(JSON.stringify({ error: 'no_evidence', message: 'Select at least one completed challenge to appear on your Passport before publishing.' }), { status: 409 })))
    fireEvent.click(screen.getByText('Publish my Passport'))
    await waitFor(() => expect(screen.getByRole('alert')).toHaveTextContent('Select at least one completed challenge'))
  })

  it('withdrawal is confirm-gated', async () => {
    responder = () => ({ ...page(0, 4, [1, 2]), passport_public: true })
    vi.stubGlobal('confirm', vi.fn(() => false))
    render(<Passport />)
    await screen.findByText('Withdraw public Passport')
    fireEvent.click(screen.getByText('Withdraw public Passport'))
    expect(calls.some(c => c.url === '/api/world/passport/publish')).toBe(false)
  })

  it('a disclosure change sends ONLY the changed key — never the expiry', async () => {
    render(<Passport />)
    await screen.findByLabelText('Scores')
    fireEvent.click(screen.getByLabelText('Scores'))
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/passport/disclosure')).toBe(true))
    expect(calls.find(c => c.url === '/api/world/passport/disclosure')!.body).toEqual({ show_scores: true })
  })
})
