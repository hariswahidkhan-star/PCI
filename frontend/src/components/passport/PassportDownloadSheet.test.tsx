import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// The printable-documents sheet (Phase 5A): two verification-QR formats with honest physical
// dimensions and print guidance, an authenticated blob download (bearer header — a bare link
// would arrive tokenless), and a disabled state that MIRRORS the server's publication gate and
// explains what unlocks it. The QR-purpose note must say verification, not event entry.

const h = vi.hoisted(() => ({ getToken: vi.fn(() => 'tok-abc' as string | null) }))

vi.mock('../../api/client', () => ({ getToken: () => h.getToken() }))

import PassportDownloadSheet from './PassportDownloadSheet'
import type { PassportSummary } from '../../api/types'

function summary(over: Partial<PassportSummary> = {}): PassportSummary {
  return {
    schemaVersion: 1,
    studentNumber: 'PCI-2026-000042',
    displayName: 'Jordan Q.',
    participantState: 'active',
    passportState: 'published',
    completedChallengeCount: 7,
    selectedEvidenceCount: 3,
    evidenceHighlights: [],
    disclosureSettings: { scores: true, profiles: false, dates: true },
    canShowPublicUrl: false,
    expiresAt: null,
    lastUpdatedAt: null,
    availableActions: ['manage'],
    ...over,
  }
}

const realFetch = globalThis.fetch
const realCreate = URL.createObjectURL
const realRevoke = URL.revokeObjectURL
// jsdom cannot navigate; a programmatic anchor click would log a noisy not-implemented error.
const realAnchorClick = HTMLAnchorElement.prototype.click

beforeEach(() => {
  h.getToken.mockReturnValue('tok-abc')
  URL.createObjectURL = vi.fn(() => 'blob:doc')
  URL.revokeObjectURL = vi.fn()
  HTMLAnchorElement.prototype.click = vi.fn()
})

afterEach(() => {
  globalThis.fetch = realFetch
  URL.createObjectURL = realCreate
  URL.revokeObjectURL = realRevoke
  HTMLAnchorElement.prototype.click = realAnchorClick
})

describe('PassportDownloadSheet', () => {
  it('lists both formats with physical dimensions, print guidance and the QR-purpose note', () => {
    render(<PassportDownloadSheet summary={summary()} />)
    const sheet = screen.getByRole('region', { name: 'Printable Passport documents' })
    expect(sheet).toBeInTheDocument()
    expect(screen.getByText('Wallet card')).toBeInTheDocument()
    expect(screen.getByText(/85\.60 × 53\.98 mm/)).toBeInTheDocument()
    expect(screen.getByText('Event badge')).toBeInTheDocument()
    expect(screen.getByText(/4 × 6 in portrait/)).toBeInTheDocument()
    // print guidance: actual size, never fit-to-page
    expect(screen.getByText(/actual size \(100% scale/)).toBeInTheDocument()
    // the QR is public verification, NOT an event pass, and carries no personal data
    expect(screen.getByText(/public Passport verification/)).toBeInTheDocument()
    expect(screen.getByText(/not an event entry\s+pass/)).toBeInTheDocument()
    expect(screen.getByText(/never encodes your Student Number, email/)).toBeInTheDocument()
  })

  it('published: both downloads are enabled and fetch with the bearer header', async () => {
    const fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      status: 200,
      blob: () => Promise.resolve(new Blob(['%PDF'], { type: 'application/pdf' })),
    })
    globalThis.fetch = fetchMock as unknown as typeof fetch

    render(<PassportDownloadSheet summary={summary()} />)
    const buttons = screen.getAllByRole('button', { name: 'Download PDF' })
    expect(buttons).toHaveLength(2)
    buttons.forEach((b) => expect(b).toBeEnabled())

    await userEvent.click(buttons[0])
    await waitFor(() => expect(fetchMock).toHaveBeenCalled())
    const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit]
    expect(url).toBe('/api/me/world-passport/documents/wallet')
    expect((init.headers as Record<string, string>).Authorization).toBe('Bearer tok-abc')
    await waitFor(() => expect(URL.createObjectURL).toHaveBeenCalled())
    expect(URL.revokeObjectURL).toHaveBeenCalled()
  })

  it('the badge row downloads the badge format', async () => {
    const fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      status: 200,
      blob: () => Promise.resolve(new Blob(['%PDF'], { type: 'application/pdf' })),
    })
    globalThis.fetch = fetchMock as unknown as typeof fetch

    render(<PassportDownloadSheet summary={summary()} />)
    await userEvent.click(screen.getAllByRole('button', { name: 'Download PDF' })[1])
    await waitFor(() =>
      expect(fetchMock).toHaveBeenCalledWith('/api/me/world-passport/documents/badge', expect.anything()),
    )
  })

  it('not published: downloads are disabled with the publish explanation', () => {
    render(<PassportDownloadSheet summary={summary({ passportState: 'private' })} />)
    screen.getAllByRole('button', { name: 'Download PDF' }).forEach((b) => expect(b).toBeDisabled())
    expect(screen.getByRole('note')).toHaveTextContent(/Publish your Passport in PCI World first/)
    expect(screen.getByRole('note')).toHaveTextContent(/unlock once your Passport is published/)
  })

  it('expired: the explanation says renew, not publish', () => {
    render(<PassportDownloadSheet summary={summary({ passportState: 'expired' })} />)
    screen.getAllByRole('button', { name: 'Download PDF' }).forEach((b) => expect(b).toBeDisabled())
    expect(screen.getByRole('note')).toHaveTextContent(/expired — renew it in PCI World/)
  })

  it('suspended: the explanation says no verifiable document can be issued', () => {
    render(<PassportDownloadSheet summary={summary({ passportState: 'suspended' })} />)
    screen.getAllByRole('button', { name: 'Download PDF' }).forEach((b) => expect(b).toBeDisabled())
    expect(screen.getByRole('note')).toHaveTextContent(/suspended/)
  })

  it('a 404 from the server (the publication gate) renders the honest refusal', async () => {
    const fetchMock = vi.fn().mockResolvedValue({ ok: false, status: 404, blob: () => Promise.resolve(new Blob()) })
    globalThis.fetch = fetchMock as unknown as typeof fetch

    render(<PassportDownloadSheet summary={summary()} />)
    await userEvent.click(screen.getAllByRole('button', { name: 'Download PDF' })[0])
    expect(await screen.findByRole('alert')).toHaveTextContent(
      /only available while your Passport is published and current/,
    )
    expect(URL.createObjectURL).not.toHaveBeenCalled()
  })

  it('a network failure renders a retryable error, never a crash', async () => {
    globalThis.fetch = vi.fn().mockRejectedValue(new Error('offline')) as unknown as typeof fetch
    render(<PassportDownloadSheet summary={summary()} />)
    await userEvent.click(screen.getAllByRole('button', { name: 'Download PDF' })[0])
    expect(await screen.findByRole('alert')).toHaveTextContent(/check your connection/)
  })
})
