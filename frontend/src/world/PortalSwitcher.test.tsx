import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import PortalSwitcher from './PortalSwitcher'

// The World → MyPCI switcher's security contract at the client: the one-time code travels ONLY in
// the URL fragment (never a query string), failures render a recoverable message with a retry —
// never an error wall — and the control is a single real button (native keyboard activation).

describe('PortalSwitcher', () => {
  const calls: { url: string; body: unknown }[] = []

  beforeEach(() => {
    calls.length = 0
    localStorage.clear()
    localStorage.setItem('pciworld_account', 'world-token')
  })

  afterEach(() => { vi.unstubAllGlobals() })

  function stubFetch(response: () => Response) {
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      calls.push({ url: String(url), body: init?.body ? JSON.parse(String(init.body)) : undefined })
      return response()
    }))
  }

  it('mints a code and navigates with it in the FRAGMENT, never a query string', async () => {
    stubFetch(() => new Response(JSON.stringify({ ok: true, code: 'onetimecode123', return_to: '/app/' }), { status: 200 }))
    const navigate = vi.fn()
    render(<PortalSwitcher navigate={navigate} />)
    fireEvent.click(screen.getByRole('button', { name: /MyPCI/ }))
    await waitFor(() => expect(navigate).toHaveBeenCalledWith('/app/#world-code=onetimecode123'))
    expect(calls.some(c => c.url === '/api/world/account/portal-handoffs/mypci')).toBe(true)
    // The code must never ride a query string or the request URL.
    const url = navigate.mock.calls[0][0] as string
    expect(url.includes('?')).toBe(false)
    expect(url.indexOf('#')).toBeLessThan(url.indexOf('onetimecode123'))
  })

  it('renders the switcher copy and stays a single keyboard-reachable button', () => {
    stubFetch(() => new Response(JSON.stringify({ ok: true, code: 'c', return_to: '/app/' }), { status: 200 }))
    render(<PortalSwitcher navigate={vi.fn()} />)
    // The decorative slash separator is aria-hidden, so the accessible name reads cleanly.
    const btn = screen.getByRole('button', { name: /PCI Global MyPCI/ })
    expect(btn).toHaveTextContent('Manage applications, membership, learning access and certifications.')
    expect(screen.getAllByRole('button')).toHaveLength(1)
  })

  it('identity_link_pending renders the linking guidance with a retry, not an error wall', async () => {
    stubFetch(() => new Response(JSON.stringify({ error: 'identity_link_pending', message: 'link first' }), { status: 409 }))
    const navigate = vi.fn()
    render(<PortalSwitcher navigate={navigate} />)
    fireEvent.click(screen.getByRole('button', { name: /MyPCI/ }))
    await waitFor(() => expect(screen.getByRole('alert')).toHaveTextContent(/Link your PCI account first/))
    expect(navigate).not.toHaveBeenCalled()
    expect(screen.getByText('Try again')).toBeInTheDocument()
  })

  it('a network failure offers retry, and a later retry succeeds', async () => {
    let fail = true
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL) => {
      calls.push({ url: String(url), body: undefined })
      if (fail) throw new TypeError('offline')
      return new Response(JSON.stringify({ ok: true, code: 'after-retry', return_to: '/app/' }), { status: 200 })
    }))
    const navigate = vi.fn()
    render(<PortalSwitcher navigate={navigate} />)
    fireEvent.click(screen.getByRole('button', { name: /MyPCI/ }))
    await waitFor(() => expect(screen.getByRole('alert')).toHaveTextContent(/Can't reach PCI right now/))
    fail = false
    fireEvent.click(screen.getByText('Try again'))
    await waitFor(() => expect(navigate).toHaveBeenCalledWith('/app/#world-code=after-retry'))
  })

  it('shows a busy state and ignores double activation while in flight', async () => {
    let resolveFetch: (r: Response) => void = () => {}
    vi.stubGlobal('fetch', vi.fn((url: RequestInfo | URL) => {
      calls.push({ url: String(url), body: undefined })
      return new Promise<Response>(res => { resolveFetch = res })
    }))
    const navigate = vi.fn()
    render(<PortalSwitcher navigate={navigate} />)
    const btn = screen.getByRole('button', { name: /MyPCI/ })
    fireEvent.click(btn)
    await waitFor(() => expect(btn).toHaveAttribute('aria-busy', 'true'))
    expect(btn).toBeDisabled()
    fireEvent.click(btn)
    expect(calls.filter(c => c.url === '/api/world/account/portal-handoffs/mypci')).toHaveLength(1)
    resolveFetch(new Response(JSON.stringify({ ok: true, code: 'x', return_to: '/app/' }), { status: 200 }))
    await waitFor(() => expect(navigate).toHaveBeenCalled())
  })
})
