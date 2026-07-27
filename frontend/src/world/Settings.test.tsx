import { describe, expect, it, vi, beforeEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import Settings from './Settings'

// Settings mirror the classic page's semantics through the same endpoints — these tests pin the
// client contract: PARTIAL preference patches (only the changed key), the session revoke calls,
// and the two sign-out scopes.

const prefsResponse = {
  linked: true,
  preferences: { goal: 'daily_practice', timezone: null, weekly_target: 3, reminders_enabled: false, reminder_hour: null },
  goals: ['daily_practice', 'certification_prep', 'evidence', 'explore'],
}
const sessionsResponse = {
  rows: [
    { id: 1, created_at: '2026-07-26 10:00:00', expires_at: '2026-08-25 10:00:00', current: true },
    { id: 2, created_at: '2026-07-25 09:00:00', expires_at: '2026-08-24 09:00:00', current: false },
  ],
}

describe('World settings', () => {
  const calls: { url: string; method: string; body: unknown }[] = []

  beforeEach(() => {
    calls.length = 0
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      const u = String(url)
      calls.push({ url: u, method: init?.method ?? 'GET', body: init?.body ? JSON.parse(String(init.body)) : undefined })
      const body = u === '/api/world/me/preferences' && (init?.method ?? 'GET') === 'GET' ? prefsResponse
        : u === '/api/world/me/sessions' ? sessionsResponse
        : { ok: true }
      return new Response(JSON.stringify(body), { status: 200 })
    }))
    localStorage.clear()
  })

  it('a goal change PATCHes ONLY the changed key', async () => {
    render(<Settings onSignOut={() => undefined} />)
    await screen.findByLabelText('Goal')
    fireEvent.change(screen.getByLabelText('Goal'), { target: { value: 'evidence' } })
    await waitFor(() => expect(calls.some(c => c.method === 'PATCH')).toBe(true))
    const patch = calls.find(c => c.method === 'PATCH')!
    expect(patch.url).toBe('/api/world/me/preferences')
    expect(patch.body).toEqual({ goal: 'evidence' })
  })

  it('revoking a session hits the endpoint with its id and refreshes the list', async () => {
    render(<Settings onSignOut={() => undefined} />)
    await screen.findByText(/Session #2/)
    const listCallsBefore = calls.filter(c => c.url === '/api/world/me/sessions').length
    fireEvent.click(screen.getByText('Revoke'))
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/me/sessions/revoke')).toBe(true))
    expect(calls.find(c => c.url === '/api/world/me/sessions/revoke')!.body).toEqual({ id: 2 })
    await waitFor(() =>
      expect(calls.filter(c => c.url === '/api/world/me/sessions').length).toBeGreaterThan(listCallsBefore))
  })

  it('the current session offers no revoke button of its own', async () => {
    render(<Settings onSignOut={() => undefined} />)
    await screen.findByText(/this device/)
    // one non-current session → exactly one Revoke button
    expect(screen.getAllByText('Revoke')).toHaveLength(1)
  })

  it('sign-out-everywhere sends everywhere:true and signs the app out', async () => {
    const onSignOut = vi.fn()
    render(<Settings onSignOut={onSignOut} />)
    await screen.findByText('Sign out of all PCI services')
    fireEvent.click(screen.getByText('Sign out of all PCI services'))
    await waitFor(() => expect(onSignOut).toHaveBeenCalled())
    const logout = calls.find(c => c.url === '/api/world/account/logout')!
    expect(logout.body).toEqual({ everywhere: true })
  })
})
