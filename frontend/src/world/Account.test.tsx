import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import App from './App'
import Settings from './Settings'

// Registration and the data/account section: the anonymous-session claim rides the register call,
// quarantine conflicts render honestly, deletion is confirm-gated with the canonical-password
// step-up, and the export downloads what the server returns.

describe('World registration', () => {
  const calls: { url: string; headers: Record<string, string>; body: unknown }[] = []

  beforeEach(() => {
    calls.length = 0
    localStorage.clear()
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      const u = String(url)
      calls.push({ url: u, headers: (init?.headers ?? {}) as Record<string, string>, body: init?.body ? JSON.parse(String(init.body)) : undefined })
      if (u === '/api/world/me/dashboard') return new Response(JSON.stringify({ error: 'no_token' }), { status: 401 })
      if (u === '/api/world/account/register') return new Response(JSON.stringify({ ok: true, token: 't-new' }), { status: 200 })
      return new Response(JSON.stringify({ ok: true }), { status: 200 })
    }))
  })

  afterEach(() => { vi.unstubAllGlobals() })

  it('register posts the account fields and carries the anonymous session for the claim', async () => {
    localStorage.setItem('world_session', 'anon-tok')
    render(<App />)
    fireEvent.click(await screen.findByText('Create your account'))
    fireEvent.change(screen.getByLabelText(/Display name/), { target: { value: 'New P' } })
    fireEvent.change(screen.getByLabelText('Email'), { target: { value: 'new@x.test' } })
    fireEvent.change(screen.getByLabelText(/Password/), { target: { value: 'long-password-1' } })
    fireEvent.click(screen.getByText('Create account'))
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/account/register')).toBe(true))
    const reg = calls.find(c => c.url === '/api/world/account/register')!
    expect(reg.body).toEqual({ email: 'new@x.test', password: 'long-password-1', display_name: 'New P' })
    expect(reg.headers['X-World-Session']).toBe('anon-tok')
  })

  it('a quarantine conflict renders the honest message', async () => {
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
      const u = String(url)
      if (u === '/api/world/me/dashboard') return new Response(JSON.stringify({ error: 'no_token' }), { status: 401 })
      return new Response(JSON.stringify({ error: 'duplicate_email', message: 'An account with this email already exists — sign in instead.' }), { status: 409 })
    }))
    render(<App />)
    fireEvent.click(await screen.findByText('Create your account'))
    fireEvent.change(screen.getByLabelText('Email'), { target: { value: 'taken@x.test' } })
    fireEvent.change(screen.getByLabelText(/Password/), { target: { value: 'long-password-1' } })
    fireEvent.click(screen.getByText('Create account'))
    await waitFor(() => expect(screen.getByRole('alert')).toHaveTextContent('already exists'))
  })
})

describe('World data & account', () => {
  const calls: { url: string; body: unknown }[] = []

  beforeEach(() => {
    calls.length = 0
    localStorage.clear()
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      const u = String(url)
      calls.push({ url: u, body: init?.body ? JSON.parse(String(init.body)) : undefined })
      if (u === '/api/world/me/preferences') return new Response(JSON.stringify({
        linked: true, preferences: { goal: null, timezone: null, weekly_target: null, reminders_enabled: false, reminder_hour: null }, goals: [] }), { status: 200 })
      if (u === '/api/world/me/sessions') return new Response(JSON.stringify({ rows: [] }), { status: 200 })
      if (u === '/api/world/account/export') return new Response(JSON.stringify({ account: { email: 'e@x' } }), { status: 200 })
      return new Response(JSON.stringify({ ok: true }), { status: 200 })
    }))
    vi.stubGlobal('confirm', vi.fn(() => true))
    URL.createObjectURL = vi.fn(() => 'blob:x')
    URL.revokeObjectURL = vi.fn()
  })

  afterEach(() => { vi.unstubAllGlobals() })

  it('export fetches the data-access payload and triggers a download', async () => {
    render(<Settings onSignOut={() => undefined} />)
    fireEvent.click(await screen.findByText('Export my PCI World data (JSON)'))
    await waitFor(() => expect(calls.some(c => c.url === '/api/world/account/export')).toBe(true))
    expect(URL.createObjectURL).toHaveBeenCalled()
  })

  it('deletion is confirm-gated and sends the canonical-password step-up', async () => {
    const onSignOut = vi.fn()
    render(<Settings onSignOut={onSignOut} />)
    const pw = await screen.findByLabelText(/Confirm with your PCI account password/)
    fireEvent.change(pw, { target: { value: 'my-real-password' } })
    fireEvent.click(screen.getByText('Delete my PCI World participation'))
    await waitFor(() => expect(onSignOut).toHaveBeenCalled())
    expect(calls.find(c => c.url === '/api/world/account/delete')!.body).toEqual({ password: 'my-real-password' })
  })

  it('a declined confirm sends nothing', async () => {
    vi.stubGlobal('confirm', vi.fn(() => false))
    render(<Settings onSignOut={() => undefined} />)
    const pw = await screen.findByLabelText(/Confirm with your PCI account password/)
    fireEvent.change(pw, { target: { value: 'pw' } })
    fireEvent.click(screen.getByText('Delete my PCI World participation'))
    expect(calls.some(c => c.url === '/api/world/account/delete')).toBe(false)
  })
})
