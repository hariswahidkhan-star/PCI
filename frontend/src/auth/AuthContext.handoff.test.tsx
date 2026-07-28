import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import { AuthProvider, useAuth } from './AuthContext'

// The student-app side of the World → MyPCI handoff: the #world-code fragment is cleared
// IMMEDIATELY (before any network work), redeemed exactly once against /api/portal-handoff/redeem,
// the session token is stored the same way login stores it (sessionStorage, pci.session.token),
// and a dead code degrades to the ordinary signed-out state with a gentle notice.

function Probe() {
  const { user, ready, notice } = useAuth()
  if (!ready) return <p>booting</p>
  return (
    <div>
      <p>{user ? `signed-in:${user.email}` : 'signed-out'}</p>
      {notice && <p role="status">{notice}</p>}
    </div>
  )
}

describe('World → MyPCI handoff bootstrap', () => {
  const calls: { url: string; body: unknown }[] = []

  beforeEach(() => {
    calls.length = 0
    sessionStorage.clear()
    window.location.hash = ''
  })

  afterEach(() => {
    vi.unstubAllGlobals()
    window.location.hash = ''
  })

  it('clears the fragment before redeeming, redeems, and stores the session like login does', async () => {
    window.location.hash = '#world-code=abc123def'
    let hashAtFetchTime: string | null = null
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      hashAtFetchTime = window.location.hash
      calls.push({ url: String(url), body: init?.body ? JSON.parse(String(init.body)) : undefined })
      return new Response(JSON.stringify({
        ok: true, token: 'fresh-portal-token',
        user: { id: 7, email: 'w@x.test', firstName: 'W', lastName: 'P' },
      }), { status: 200 })
    }))

    render(<AuthProvider><Probe /></AuthProvider>)
    await screen.findByText('signed-in:w@x.test')

    // The one-time code was scrubbed from the address bar BEFORE any request went out.
    expect(hashAtFetchTime).toBe('')
    expect(window.location.hash).toBe('')
    const redeem = calls.find(c => c.url === '/api/portal-handoff/redeem')
    expect(redeem).toBeTruthy()
    expect(redeem!.body).toEqual({ code: 'abc123def' })
    // Stored exactly the way login stores it — the shared client's sessionStorage key.
    expect(sessionStorage.getItem('pci.session.token')).toBe('fresh-portal-token')
    // The handoff replaced the boot probe: one redeem call, no duplicate redemption.
    expect(calls.filter(c => c.url === '/api/portal-handoff/redeem')).toHaveLength(1)
  })

  it('an expired code lands on the ordinary signed-out state with a gentle notice', async () => {
    window.location.hash = '#world-code=deadcode'
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      calls.push({ url: String(url), body: init?.body ? JSON.parse(String(init.body)) : undefined })
      return new Response(JSON.stringify({ error: 'invalid_code' }), { status: 401 })
    }))

    render(<AuthProvider><Probe /></AuthProvider>)
    await screen.findByText('signed-out')

    expect(window.location.hash).toBe('')
    expect(sessionStorage.getItem('pci.session.token')).toBeNull()
    expect(screen.getByRole('status')).toHaveTextContent(/link from PCI World has expired/)
    // The dead code was tried exactly once and never retried.
    expect(calls.filter(c => c.url === '/api/portal-handoff/redeem')).toHaveLength(1)
  })

  it('a failed redemption never destroys an existing portal session', async () => {
    sessionStorage.setItem('pci.session.token', 'existing-session')
    window.location.hash = '#world-code=deadcode'
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
      const u = String(url)
      calls.push({ url: u, body: undefined })
      if (u === '/api/portal-handoff/redeem')
        return new Response(JSON.stringify({ error: 'invalid_code' }), { status: 401 })
      // the normal boot probe for the existing session
      return new Response(JSON.stringify({
        user: { id: 3, email: 'kept@x.test', first_name: 'K', last_name: 'S' },
      }), { status: 200 })
    }))

    render(<AuthProvider><Probe /></AuthProvider>)
    await screen.findByText('signed-in:kept@x.test')
    expect(sessionStorage.getItem('pci.session.token')).toBe('existing-session')
  })

  it('without a fragment, boot never calls the redeem endpoint', async () => {
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
      calls.push({ url: String(url), body: undefined })
      return new Response(JSON.stringify({ error: 'no_token' }), { status: 401 })
    }))
    render(<AuthProvider><Probe /></AuthProvider>)
    await screen.findByText('signed-out')
    expect(calls.some(c => c.url === '/api/portal-handoff/redeem')).toBe(false)
  })
})
