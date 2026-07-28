import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import Sharing from './Sharing'

// Sharing management's client contract: live/withdrawn labelling, per-item revoke payloads,
// list refresh after a withdrawal, and the two withdraw-all endpoints.

const sharesResponse = {
  caption: 'P — PCI World Passport. Practice evidence only — never a credential.',
  results: [
    { attempt_id: 11, code: 'WC-A', title: 'Alpha', completed_at: '2026-07-01', created_at: '2026-07-05', revoked: false },
    { attempt_id: 12, code: 'WC-B', title: 'Beta', completed_at: '2026-07-02', revoked: true },
  ],
  invitations: [
    { invite_id: 21, code: 'WC-A', title: 'Alpha', version: 3, created_at: '2026-07-03', inviter_name: 'P', revoked: false },
  ],
}

describe('World sharing management', () => {
  const calls: { url: string; body: unknown }[] = []

  beforeEach(() => {
    calls.length = 0
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      const u = String(url)
      calls.push({ url: u, body: init?.body ? JSON.parse(String(init.body)) : undefined })
      return new Response(JSON.stringify(u === '/api/world/me/shares' ? sharesResponse : { ok: true }), { status: 200 })
    }))
    localStorage.clear()
  })

  afterEach(() => { vi.unstubAllGlobals() })

  it('labels withdrawn links and offers Withdraw only on live ones', async () => {
    render(<Sharing />)
    await screen.findByText(/Shared result links · 1 live/)
    expect(screen.getAllByText('· withdrawn')).toHaveLength(1)
    // the per-link mint time travels when the server provides it, and is absent-safe when not
    expect(screen.getByText(/link created 2026-07-05/)).toBeInTheDocument()
    // one live result + one live invitation → two per-item Withdraw buttons
    expect(screen.getAllByText('Withdraw')).toHaveLength(2)
  })

  it('withdrawing a result link posts its attempt_id and refreshes', async () => {
    render(<Sharing />)
    await screen.findAllByText(/Alpha/)
    const before = calls.filter(c => c.url === '/api/world/me/shares').length
    fireEvent.click(screen.getAllByText('Withdraw')[0])
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/me/shares/revoke')).toBe(true))
    expect(calls.find(c => c.url === '/api/world/me/shares/revoke')!.body).toEqual({ attempt_id: 11 })
    await waitFor(() => expect(calls.filter(c => c.url === '/api/world/me/shares').length).toBeGreaterThan(before))
  })

  it('withdrawing an invitation posts its invite_id', async () => {
    render(<Sharing />)
    await screen.findByText(/Challenge invitations · 1 live/)
    fireEvent.click(screen.getAllByText('Withdraw')[1])
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/me/invitations/revoke')).toBe(true))
    expect(calls.find(c => c.url === '/api/world/me/invitations/revoke')!.body).toEqual({ invite_id: 21 })
  })

  it('the withdraw-all controls hit their endpoints', async () => {
    render(<Sharing />)
    await screen.findByText('Withdraw all result links')
    fireEvent.click(screen.getByText('Withdraw all result links'))
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/me/shares/revoke-all')).toBe(true))
    fireEvent.click(screen.getByText('Withdraw all invitations'))
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/me/invitations/revoke-all')).toBe(true))
  })
})
