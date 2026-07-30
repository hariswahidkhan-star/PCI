import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import VerifyNumber from './VerifyNumber'

// The public verification form's client contract: the number travels ONLY in a POST body (never
// a URL), input is normalised before sending, a malformed number never reaches the network, the
// neutral answer renders verbatim without embellishment, a success renders only what the server
// disclosed, and a 429 is explained as rate limiting rather than a failure.

const FOUND = {
  found: true,
  student_number: 'PCI-2026-000123',
  display_name: 'Jordan Q. Practitioner',
  passport: { status: 'published', discloses: 'scores, dates', expires_on: null, completed: 2, industries: 1, tracks: 1 },
  evidence: [
    { title: 'Earned value under pressure', reference: 'WC-EVM-001 · v1', industry: 'Energy', track: 'project_controls', difficulty: 'professional', score: 82.5, completed_on: '2026-06-01' },
  ],
  evidence_total: 2,
  photo: null,
  verify_page: '/world/verify',
  verified_at: '2026-07-28 12:00:00 UTC',
  disclaimer: 'PCI World Passport records selected professional practice and challenge evidence. It is not, by itself, a PCI certification, examination result, accreditation, licence, or guarantee of professional competence.',
}

const NEUTRAL = { found: false, message: 'No public PCI World Passport is currently available for that number.' }

describe('public Passport verification by Student Number', () => {
  const calls: { url: string; method: string; body: unknown }[] = []
  let respond: () => { status: number; body: unknown }

  beforeEach(() => {
    calls.length = 0
    respond = () => ({ status: 200, body: NEUTRAL })
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      calls.push({ url: String(url), method: init?.method ?? 'GET', body: init?.body ? JSON.parse(String(init.body)) : undefined })
      const r = respond()
      return new Response(JSON.stringify(r.body), { status: r.status })
    }))
    sessionStorage.clear()
  })

  afterEach(() => { vi.unstubAllGlobals() })

  const type = (v: string) => fireEvent.change(screen.getByLabelText('PCI Student Number'), { target: { value: v } })
  const go = () => fireEvent.click(screen.getByText('Verify this number'))

  it('POSTs the normalised number in the body — never in the URL', async () => {
    render(<VerifyNumber />)
    type('  pci-2026-000123 ')
    go()
    await waitFor(() => expect(calls).toHaveLength(1))
    expect(calls[0].method).toBe('POST')
    expect(calls[0].url).toBe('/api/public/world-passports/verify')
    expect(calls[0].url).not.toContain('PCI-2026')
    expect(calls[0].body).toEqual({ number: 'PCI-2026-000123' })
  })

  it('a malformed number is refused client-side and nothing is sent', async () => {
    render(<VerifyNumber />)
    type('PCI-2026')
    go()
    await screen.findByRole('alert')
    expect(calls).toHaveLength(0)
  })

  it('the neutral answer renders the server sentence verbatim', async () => {
    render(<VerifyNumber />)
    type('PCI-2026-999999')
    go()
    await screen.findByText(NEUTRAL.message)
    expect(screen.getByText(/deliberately indistinguishable/)).toBeTruthy()
  })

  it('a success renders the disclosed record and the practice disclaimer', async () => {
    respond = () => ({ status: 200, body: FOUND })
    render(<VerifyNumber />)
    type('PCI-2026-000123')
    go()
    await screen.findByText('Jordan Q. Practitioner')
    expect(screen.getByText('PCI-2026-000123')).toBeTruthy()
    expect(screen.getByText(/Earned value under pressure/)).toBeTruthy()
    expect(screen.getByText(/score 82.5/)).toBeTruthy()
    expect(screen.getByText(/It is not, by itself, a PCI certification/)).toBeTruthy()
    expect(screen.getByText(/Showing 1 of 2 published challenges/)).toBeTruthy()
  })

  it('a 429 is explained as rate limiting', async () => {
    render(<VerifyNumber />)
    respond = () => ({ status: 429, body: { error: 'rate_limited' } })
    type('PCI-2026-000123')
    go()
    await screen.findByText(/Too many checks from this connection/)
  })

  it('no Authorization header rides on the public call', async () => {
    const spy = vi.mocked(globalThis.fetch)
    render(<VerifyNumber />)
    type('PCI-2026-000123')
    go()
    await waitFor(() => expect(calls).toHaveLength(1))
    const init = spy.mock.calls[0][1] as RequestInit
    expect((init.headers as Record<string, string>).Authorization).toBeUndefined()
  })
})
