import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE — My event passes (§7A.12A). Pinned decisions: fixtures mode (?preview=1) renders the sample
// wallet with the QR area explicitly labelled as NOT the public Passport QR; the replace action has
// a confirm step; and a 404 from the (not-yet-built) backend renders the designed "not yet enabled"
// state — never a broken screen. The fetch client is mocked at its module boundary; the REAL seam
// (src/api/eventAdmission.ts) runs, so the 404 -> unavailable mapping is tested end-to-end.

const h = vi.hoisted(() => ({ get: vi.fn(), post: vi.fn() }))

vi.mock('../api/client', async (orig) => {
  const actual = await orig<typeof import('../api/client')>()
  return {
    ...actual,
    api: { ...actual.api, get: (p: string) => h.get(p), post: (p: string, b?: unknown) => h.post(p, b) },
  }
})

import EventPasses, { fmtWallClock } from './EventPasses'
import { ApiError } from '../api/client'

function setUrl(path: string) {
  window.history.replaceState({}, '', path)
}

describe('EventPasses (My event passes wallet)', () => {
  beforeEach(() => {
    h.get.mockReset()
    h.post.mockReset()
    setUrl('/event-passes')
    sessionStorage.clear()
  })
  afterEach(() => setUrl('/'))

  it('renders the sample wallet in fixtures mode (?preview=1)', async () => {
    setUrl('/event-passes?preview=1')
    renderWithProviders(<EventPasses />)

    expect(await screen.findByText('PCI AI Project Controls Summit 2026')).toBeInTheDocument()
    expect(screen.getByText('Regional Workshop — Cost Engineering in Practice')).toBeInTheDocument()

    // No network call happened — fixtures are local.
    expect(h.get).not.toHaveBeenCalled()

    // The QR area is explicitly NOT the public Passport QR, on every pass.
    expect(screen.getAllByText('Event entry pass — NOT your public Passport QR')).toHaveLength(3)
    // Active pass renders the placeholder QR block; inactive pass explains why there is none.
    expect(screen.getAllByRole('img', { name: 'Entry QR code placeholder' }).length).toBeGreaterThan(0)
    expect(screen.getByText(/entry QR will appear here once your pass is active/)).toBeInTheDocument()

    // Registration + payment + pass state chips.
    expect(screen.getByText('Waitlisted')).toBeInTheDocument()
    expect(screen.getByText('Payment pending')).toBeInTheDocument()
    expect(screen.getAllByText('Paid').length).toBeGreaterThan(0)
    expect(screen.getByText('Pass active')).toBeInTheDocument()

    // Entry validity window and admission mode.
    expect(screen.getAllByText('Entry window:').length).toBe(3)
    expect(screen.getByText(/QR scan, with manual desk fallback/)).toBeInTheDocument()

    // Entry/checkout history for the used pass.
    expect(screen.getByText(/Entered · 2 May 2026, 08:12/)).toBeInTheDocument()
    expect(screen.getByText(/Checked out · 2 May 2026, 12:40/)).toBeInTheDocument()

    // Privacy note about what gate staff can see.
    expect(screen.getByText('What gate staff can see')).toBeInTheDocument()
    expect(screen.getByText(/never see your Passport contents/)).toBeInTheDocument()
  })

  it('replace pass requires an explicit confirm step', async () => {
    const user = userEvent.setup()
    setUrl('/event-passes?preview=1')
    renderWithProviders(<EventPasses />)
    await screen.findByText('PCI AI Project Controls Summit 2026')

    await user.click(screen.getAllByRole('button', { name: 'Replace pass' })[0])
    // Confirm step, with a way out.
    expect(screen.getByText(/current QR and reference stop working at the gate immediately/)).toBeInTheDocument()

    await user.click(screen.getByRole('button', { name: 'Keep current pass' }))
    expect(screen.queryByRole('button', { name: 'Yes, replace pass' })).toBeNull()

    await user.click(screen.getAllByRole('button', { name: 'Replace pass' })[0])
    await user.click(screen.getByRole('button', { name: 'Yes, replace pass' }))
    expect(
      await screen.findByText('Pass replaced — the previous QR no longer works at the gate.'),
    ).toBeInTheDocument()
  })

  it('renders the designed "not yet enabled" state on a 404 from the seam', async () => {
    h.get.mockRejectedValue(new ApiError(404, 'not_found', { error: 'not_found' }))
    renderWithProviders(<EventPasses />)
    expect(
      await screen.findByText('Event passes are not yet enabled on this server'),
    ).toBeInTheDocument()
    // The honest state is designed, not broken: registrations are pointed at the Events page.
    expect(screen.getByRole('link', { name: 'Events page' })).toBeInTheDocument()
    expect(h.get).toHaveBeenCalledWith('/api/me/event-passes')
  })

  it('surfaces a real (non-404/501) failure as an error, not the unavailable state', async () => {
    h.get.mockRejectedValue(new ApiError(500, 'server_error', { error: 'server_error' }))
    renderWithProviders(<EventPasses />)
    expect(await screen.findByText('server_error')).toBeInTheDocument()
    expect(screen.queryByText('Event passes are not yet enabled on this server')).toBeNull()
  })

  it('shows the empty state when the backend exists but there are no passes', async () => {
    h.get.mockResolvedValue({ rows: [] })
    renderWithProviders(<EventPasses />)
    expect(await screen.findByText(/No event passes yet/)).toBeInTheDocument()
  })

  it('fmtWallClock renders the wall-clock string without timezone shifting', () => {
    expect(fmtWallClock('2026-09-14 09:00:00')).toBe('14 Sep 2026, 09:00')
    expect(fmtWallClock('not-a-date')).toBe('not-a-date')
  })
})
