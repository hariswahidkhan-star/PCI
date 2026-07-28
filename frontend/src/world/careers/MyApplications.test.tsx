import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen } from '@testing-library/react'
import MyApplications from './MyApplications'

// PCI World careers — the applicant's own record.
//
// The property that matters most is asserted first: the consent record is shown INCLUDING after
// withdrawal — the row survives, `consent.active` goes false, and the granted-at and policy
// version stay on screen. Withdrawing consent shows the SERVER'S sentence verbatim ("…kept —
// withdrawing consent is not the same as erasure"), and no outcome is ever echoed before the
// server confirms it.

// The exact sentence the server sends on consent withdrawal (WorldCareers.cs). Verbatim by
// design: it is the server's careful account that the record is KEPT and this is not erasure.
const CONSENT_NOTICE =
  'Consent withdrawn. The employer can no longer open this application or your CV. '
  + 'The record that you applied, and that you consented at the time, is kept — '
  + 'withdrawing consent is not the same as erasure.'

const APP_ACTIVE = {
  id: 1, state: 'submitted', title: 'Senior planner', employer: 'Acme Controls',
  submitted_at: '2026-07-01 09:00:00', withdrawn_at: null, cv_name: 'dana-cv.pdf',
  consent: { granted_at: '2026-07-01 09:00:00', withdrawn_at: null, policy_version: 'v1', active: true },
}
const APP_CONSENT_WITHDRAWN = {
  id: 2, state: 'submitted', title: 'Cost engineer', employer: 'Beta Rail',
  submitted_at: '2026-06-20 10:00:00', withdrawn_at: null, cv_name: null,
  consent: { granted_at: '2026-06-20 10:00:00', withdrawn_at: '2026-07-02 12:00:00', policy_version: 'v1', active: false },
}

describe('World careers — my applications', () => {
  const calls: { url: string; method: string; headers: Record<string, string>; body: unknown }[] = []
  let responder: (url: string, method: string) => unknown

  beforeEach(() => {
    calls.length = 0
    localStorage.clear()
    localStorage.setItem('pciworld_account', 'tok-account')
    responder = () => ({ applications: [APP_ACTIVE, APP_CONSENT_WITHDRAWN] })
    vi.stubGlobal('fetch', vi.fn(async (url: string, init?: RequestInit) => {
      const method = init?.method ?? 'GET'
      calls.push({
        url, method,
        headers: (init?.headers ?? {}) as Record<string, string>,
        body: init?.body ? JSON.parse(String(init.body)) : undefined,
      })
      const payload = await responder(url, method)
      const status = (payload as { _status?: number })?._status ?? 200
      return { ok: status < 400, status, text: async () => JSON.stringify(payload) } as unknown as Response
    }))
  })
  afterEach(() => vi.unstubAllGlobals())

  it('rides the World account header, never a portal bearer', async () => {
    render(<MyApplications />)
    await screen.findByText('Senior planner')
    const listCall = calls.find(c => c.url === '/api/world/careers/my/applications')
    expect(listCall?.headers['X-World-Account']).toBe('tok-account')
    expect(listCall?.headers['Authorization']).toBeUndefined()
  })

  it('tells a signed-out visitor to sign in', async () => {
    responder = () => ({ _status: 401, error: 'no_session' })
    render(<MyApplications />)
    await screen.findByText(/sign in to your PCI World account/i)
    expect(screen.queryByText('Senior planner')).toBeNull()
  })

  // ── The consent record outlives the consent ──

  it('shows the consent record INCLUDING after withdrawal — the row survives, active goes false', async () => {
    render(<MyApplications />)
    await screen.findByText('Cost engineer')
    // The withdrawn consent still shows everything the record holds: when it was withdrawn, when
    // it was granted, and under which policy words. Withdrawal is a timestamp, not an erasure.
    const record = screen.getByText(/The record remains/)
    expect(record.textContent).toContain('2026-07-02 12:00:00')
    expect(record.textContent).toContain('you granted consent 2026-06-20 10:00:00 UTC under policy v1')
    // With consent inactive there is nothing left to withdraw — but the application itself still
    // shows, and its own withdrawal stays available.
    expect(screen.queryByRole('button', { name: 'Withdraw consent for application 2' })).toBeNull()
    expect(screen.getByRole('button', { name: 'Withdraw application 2' })).toBeTruthy()
    // The active row shows its record too.
    expect(screen.getByText(/granted 2026-07-01 09:00:00 UTC under policy v1/)).toBeTruthy()
  })

  it('withdraws consent only after stating the consequence, then shows the server sentence verbatim', async () => {
    render(<MyApplications />)
    await screen.findByText('Senior planner')
    fireEvent.click(screen.getByRole('button', { name: 'Withdraw consent for application 1' }))

    // Step one is words, not a request: the consequence — access stops, the record is KEPT, this
    // is not erasure — is on screen before anything is sent.
    expect(screen.getByText(/withdrawing consent is not the same as erasure/i)).toBeTruthy()
    expect(calls.some(c => c.method === 'POST')).toBe(false)

    responder = (url, method) => {
      if (method === 'POST' && url.includes('/consent/withdraw')) return { ok: true, notice: CONSENT_NOTICE }
      // The reload now reports the withdrawn-but-kept record.
      return { applications: [{ ...APP_ACTIVE, consent: { ...APP_ACTIVE.consent, withdrawn_at: '2026-07-03 08:00:00', active: false } }] }
    }
    fireEvent.click(screen.getByRole('button', { name: 'Yes, withdraw consent' }))

    // The server's sentence, VERBATIM — paraphrasing it into "your data is gone" would promise
    // an erasure that did not happen.
    const status = await screen.findByText(CONSENT_NOTICE)
    expect(status.getAttribute('role')).toBe('status')
    expect(calls.some(c => c.url === '/api/world/careers/applications/1/consent/withdraw')).toBe(true)
    // The list was re-read, and the record is still on screen after withdrawal.
    expect(calls.filter(c => c.url === '/api/world/careers/my/applications').length).toBe(2)
    expect(await screen.findByText(/The record remains/)).toBeTruthy()
  })

  // ── Withdrawing the application itself ──

  it('withdraws an application through the server and re-reads the list for the new state', async () => {
    responder = () => ({ applications: [APP_ACTIVE] })
    render(<MyApplications />)
    await screen.findByText('Senior planner')
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/withdraw')) return { ok: true, state: 'withdrawn' }
      return { applications: [{ ...APP_ACTIVE, state: 'withdrawn', withdrawn_at: '2026-07-03 08:00:00' }] }
    }
    fireEvent.click(screen.getByRole('button', { name: 'Withdraw application 1' }))

    await screen.findByText(/is now withdrawn\. The record of it stays in this list\./)
    expect(calls.some(c => c.method === 'POST' && c.url === '/api/world/careers/applications/1/withdraw')).toBe(true)
    expect(calls.filter(c => c.url === '/api/world/careers/my/applications').length).toBe(2)
    // The row now says what the server says — and a withdrawn application offers no second
    // withdrawal.
    expect(screen.getByText('withdrawn')).toBeTruthy()
    expect(screen.queryByRole('button', { name: 'Withdraw application 1' })).toBeNull()
  })

  it('echoes nothing about a withdrawal before the server confirms it', async () => {
    responder = () => ({ applications: [APP_ACTIVE] })
    render(<MyApplications />)
    await screen.findByText('Senior planner')
    let release: () => void = () => {}
    const gate = new Promise<void>(r => { release = r })
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/withdraw')) return gate.then(() => ({ ok: true, state: 'withdrawn' }))
      return { applications: [{ ...APP_ACTIVE, state: 'withdrawn', withdrawn_at: '2026-07-03 08:00:00' }] }
    }
    fireEvent.click(screen.getByRole('button', { name: 'Withdraw application 1' }))

    // The server has not answered: the row still shows the last server state, and no claim of
    // withdrawal appears anywhere.
    expect(screen.getByText('submitted')).toBeTruthy()
    expect(screen.queryByText(/is now withdrawn/)).toBeNull()
    release()
    await screen.findByText(/is now withdrawn/)
  })

  it('relays a refused withdrawal in words a person can act on, changing nothing', async () => {
    responder = () => ({ applications: [APP_ACTIVE] })
    render(<MyApplications />)
    await screen.findByText('Senior planner')
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/withdraw')) return { _status: 400, error: 'bad_transition' }
      return { applications: [APP_ACTIVE] }
    }
    fireEvent.click(screen.getByRole('button', { name: 'Withdraw application 1' }))

    await screen.findByText(/That change is not available from the current state/)
    // Still exactly what the server last said.
    expect(screen.getByText('submitted')).toBeTruthy()
  })

  // ── Outcomes are results, not emergencies ──

  it('announces outcomes via status, never alert', async () => {
    responder = () => ({ applications: [APP_ACTIVE] })
    render(<MyApplications />)
    await screen.findByText('Senior planner')
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/consent/withdraw')) return { ok: true, notice: CONSENT_NOTICE }
      return { applications: [APP_ACTIVE] }
    }
    fireEvent.click(screen.getByRole('button', { name: 'Withdraw consent for application 1' }))
    fireEvent.click(screen.getByRole('button', { name: 'Yes, withdraw consent' }))
    await screen.findByText(CONSENT_NOTICE)
    expect(screen.queryByRole('alert')).toBeNull()
  })
})
