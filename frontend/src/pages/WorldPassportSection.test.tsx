import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// The MyPCI dashboard Passport module: consumes GET /api/me/world-passport/summary (the one shared
// read model), crosses into PCI World through the EXISTING SSO bridge, and degrades to a quiet
// fallback card on failure — the rest of the dashboard must never depend on this fetch.
const h = vi.hoisted(() => ({ get: vi.fn(), post: vi.fn() }))

vi.mock('../api/client', () => ({
  api: {
    get: (path: string) => h.get(path),
    post: (path: string, body?: unknown) => h.post(path, body),
  },
}))

import WorldPassportSection from './WorldPassportSection'
import type { PassportSummary } from '../api/types'

function summary(over: Partial<PassportSummary> = {}): PassportSummary {
  return {
    schemaVersion: 1,
    studentNumber: 'PCI-2026-000042',
    displayName: 'Jordan Q.',
    participantState: 'active',
    passportState: 'published',
    completedChallengeCount: 7,
    selectedEvidenceCount: 3,
    evidenceHighlights: [{ title: 'Earned value under pressure', reference: 'WC-EVM-001 · v1' }],
    disclosureSettings: { scores: true, profiles: false, dates: true },
    canShowPublicUrl: false,
    expiresAt: null,
    lastUpdatedAt: '2026-06-01 09:00:00',
    availableActions: ['manage'],
    ...over,
  }
}

describe('WorldPassportSection (MyPCI dashboard module)', () => {
  beforeEach(() => {
    h.get.mockReset()
    h.post.mockReset()
    localStorage.clear()
  })

  it('renders the shared card from the summary read model', async () => {
    h.get.mockResolvedValueOnce(summary())
    render(<WorldPassportSection />)
    expect(await screen.findByText('Jordan Q.')).toBeInTheDocument()
    expect(h.get).toHaveBeenCalledWith('/api/me/world-passport/summary')
    expect(screen.getByText('Published')).toBeInTheDocument()
    expect(screen.getByText('Earned value under pressure')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'View full Passport' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Manage in PCI World' })).toBeInTheDocument()
  })

  it('a fetch failure degrades to the quiet fallback card — never a crash', async () => {
    h.get.mockRejectedValueOnce(new Error('backend down'))
    render(<WorldPassportSection />)
    expect(await screen.findByText('Passport temporarily unavailable')).toBeInTheDocument()
    expect(screen.getByText(/Everything else in MyPCI is unaffected/)).toBeInTheDocument()
    // it still offers the door into PCI World
    expect(screen.getByRole('button', { name: 'Open PCI World' })).toBeInTheDocument()
  })

  it('not_created renders the create-onboarding state and the create action uses the SSO bridge', async () => {
    h.get.mockResolvedValueOnce(summary({ participantState: 'none', passportState: 'not_created', availableActions: ['create'] }))
    // jsdom cannot navigate; reject so the click settles on the error path after the POST fired.
    h.post.mockRejectedValueOnce(new Error('nav'))
    render(<WorldPassportSection />)
    const btn = await screen.findByRole('button', { name: 'Create my Passport' })
    await userEvent.click(btn)
    await waitFor(() => expect(h.post).toHaveBeenCalled())
    const [path, body] = h.post.mock.calls[0] as [string, Record<string, unknown>]
    expect(path).toBe('/api/me/world-passport/sso')       // the EXISTING bridge from Profile.tsx
    expect(body.return_to).toBe('/world/account')
  })

  it('link_pending surfaces the linking journey, not a passport card', async () => {
    h.get.mockResolvedValueOnce(summary({ participantState: 'link_pending', passportState: 'not_created', displayName: null, availableActions: ['link'] }))
    render(<WorldPassportSection />)
    expect(await screen.findByText('A PCI World account is waiting to be linked')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Link my PCI World account' })).toBeInTheDocument()
    expect(screen.queryByText('Published')).toBeNull()
  })

  it('shows the loading skeleton while the summary is in flight', () => {
    h.get.mockReturnValueOnce(new Promise(() => { /* never settles */ }))
    render(<WorldPassportSection />)
    expect(screen.getByLabelText('Loading your PCI World Passport')).toHaveAttribute('aria-busy', 'true')
  })

  it('an SSO failure renders the error in place without unmounting the card', async () => {
    h.get.mockResolvedValueOnce(summary())
    h.post.mockRejectedValueOnce(new Error('Could not open PCI World.'))
    render(<WorldPassportSection />)
    await userEvent.click(await screen.findByRole('button', { name: 'View full Passport' }))
    expect(await screen.findByRole('alert')).toHaveTextContent('Could not open PCI World.')
    expect(screen.getByText('Jordan Q.')).toBeInTheDocument()
  })
})
