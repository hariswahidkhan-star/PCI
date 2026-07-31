import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'
import { ApiError } from '../api/client'

// FE-3 — the portal's most complex screen. It gates exam scheduling behind eligibility holds, drives
// a book/reschedule form (with the BOOK_ERRORS code map), renders the per-entitlement state machine
// (certified / scheduled / attempted / not-scheduled), the government-ID upload (with the client-side
// size guard), and the membership-gated exam catalogue. useMe/useQuery/checkout are mocked; the real
// ApiError + getToken are kept so the error-code mapping runs exactly as in production.
const h = vi.hoisted(() => ({
  me: null as unknown,
  refetch: vi.fn(),
  post: vi.fn(),
  startCheckout: vi.fn(),
  certs: { rows: [] as unknown[] },
}))

vi.mock('../data/MeContext', () => ({
  useMe: () => ({ me: h.me, loading: false, error: null, refetch: h.refetch }),
}))
vi.mock('../api/hooks', () => ({
  useQuery: (path: string | null) => {
    let data: unknown = null
    if (path === '/api/certifications') data = h.certs
    else if (path && path.startsWith('/api/me/exam/delivery')) data = { routed: false } // in-house delivery
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))
vi.mock('../api/client', async (orig) => {
  const actual = await orig<typeof import('../api/client')>()
  return { ...actual, api: { ...actual.api, post: (p: string, b?: unknown) => h.post(p, b) } }
})
vi.mock('../api/checkout', () => ({
  startCheckout: (opts: unknown) => h.startCheckout(opts),
  checkoutErrorMessage: (e: unknown) => (e instanceof Error ? e.message : 'Could not start checkout.'),
}))

import Certifications from './Certifications'

const baseMe = (over: Record<string, unknown> = {}) => ({
  user: { id: 1, email: 'sam@example.com', first_name: 'Sam', last_name: 'Rivera', registration_no: 'PCI-1', created_at: '2025-01-01T00:00:00Z' },
  lifecycle: { membership_status: 'active', blocking_items: [] as string[] },
  identity_document: { doc_kind: 'passport', status: 'verified', filename: 'p.pdf', created_at: '2025-02-01T00:00:00Z' },
  exams: [] as unknown[],
  ...over,
})

const notScheduledEntry = () => ({
  payment_id: 10,
  certification_id: 1,
  certification_code: 'PCL-AI',
  certification_name: 'Project Controls Lead — AI',
  reference: 'PAY-1',
  // The backend stores the deadline WITHOUT a trailing 'Z' (the screen appends one to anchor it UTC).
  deadline: '2027-06-01 00:00:00',
  booking: null,
  latest_attempt: null,
  credential: null,
})

describe('Certifications', () => {
  beforeEach(() => {
    h.refetch.mockReset()
    h.post.mockReset()
    h.startCheckout.mockReset()
    h.certs = { rows: [] }
    h.me = baseMe()
  })

  describe('scheduling eligibility gate', () => {
    it('disables Schedule exam and names the hold when an eligibility item is blocking', () => {
      h.me = baseMe({
        identity_document: null,
        lifecycle: { membership_status: 'active', blocking_items: ['identity_document_missing'] },
        exams: [notScheduledEntry()],
      })
      renderWithProviders(<Certifications />)
      const schedule = screen.getByRole('button', { name: 'Schedule exam' })
      expect(schedule).toBeDisabled()
      expect(screen.getByText(/To unlock scheduling:/)).toBeInTheDocument()
      expect(screen.getByText(/upload your government-issued ID/)).toBeInTheDocument()
    })

    it('enables Schedule exam when there are no holds', () => {
      h.me = baseMe({ exams: [notScheduledEntry()] })
      renderWithProviders(<Certifications />)
      expect(screen.getByRole('button', { name: 'Schedule exam' })).toBeEnabled()
    })
  })

  // Drives the calendar + slot-grid scheduling window: pick the last bookable day of the
  // displayed month (always fully in range against the 2027 deadline), then its LAST available
  // slot — the last enabled day always offers at least one slot, while a named time would flake
  // whenever the wall clock has already passed it on a day that still has later slots.
  const pickDayAndSlot = async (user: ReturnType<typeof userEvent.setup>) => {
    const days = document.querySelectorAll<HTMLButtonElement>('.schedm-day:not([disabled])')
    expect(days.length).toBeGreaterThan(0)
    await user.click(days[days.length - 1])
    const slots = document.querySelectorAll<HTMLButtonElement>('.schedm-slot')
    expect(slots.length).toBeGreaterThan(0)
    await user.click(slots[slots.length - 1])
  }

  describe('booking form', () => {
    it('opens the scheduling window, keeps Confirm disabled until a day and time are chosen, then POSTs the booking', async () => {
      const user = userEvent.setup()
      h.me = baseMe({ exams: [notScheduledEntry()] })
      renderWithProviders(<Certifications />)
      await user.click(screen.getByRole('button', { name: 'Schedule exam' }))

      const confirm = await screen.findByRole('button', { name: 'Confirm slot' })
      expect(confirm).toBeDisabled()
      const days = document.querySelectorAll<HTMLButtonElement>('.schedm-day:not([disabled])')
      expect(days.length).toBeGreaterThan(0)
      await user.click(days[days.length - 1])
      expect(confirm).toBeDisabled() // a day alone is not enough
      const slots = document.querySelectorAll<HTMLButtonElement>('.schedm-slot')
      expect(slots.length).toBeGreaterThan(0)
      await user.click(slots[slots.length - 1])
      expect(confirm).toBeEnabled()

      h.post.mockResolvedValueOnce(undefined)
      await user.click(confirm)
      await waitFor(() =>
        expect(h.post).toHaveBeenCalledWith('/api/me/exam/book', expect.objectContaining({ certification_id: 1 })),
      )
      expect(h.refetch).toHaveBeenCalled()
    })

    it('maps a backend BOOK_ERRORS code to its friendly message', async () => {
      const user = userEvent.setup()
      h.me = baseMe({ exams: [notScheduledEntry()] })
      renderWithProviders(<Certifications />)
      await user.click(screen.getByRole('button', { name: 'Schedule exam' }))
      await screen.findByRole('button', { name: 'Confirm slot' })
      await pickDayAndSlot(user)
      h.post.mockRejectedValueOnce(new ApiError(400, 'bad', { error: 'bad_slot' }))
      await user.click(screen.getByRole('button', { name: 'Confirm slot' }))
      expect(await screen.findByRole('alert')).toHaveTextContent('Please choose a time at least 2 hours from now.')
    })
  })

  describe('entitlement state machine', () => {
    it('shows the certified state with the credential id when an active credential exists', () => {
      h.me = baseMe({
        exams: [{ ...notScheduledEntry(), credential: { credential_id: 'PCL-2025-0001', status: 'active', expires_at: '2028-01-01T00:00:00Z' } }],
      })
      renderWithProviders(<Certifications />)
      expect(screen.getByText('Certified')).toBeInTheDocument()
      expect(screen.getByText('PCL-2025-0001')).toBeInTheDocument()
      expect(screen.queryByRole('button', { name: 'Schedule exam' })).not.toBeInTheDocument()
    })

    it('shows the scheduled state when a booking exists', () => {
      h.me = baseMe({
        exams: [{ ...notScheduledEntry(), booking: { scheduled_at: '2027-01-01T10:00:00Z', timezone: 'UTC' } }],
      })
      renderWithProviders(<Certifications />)
      expect(screen.getByText('Scheduled')).toBeInTheDocument()
      expect(screen.getByText(/Your exam is scheduled for/)).toBeInTheDocument()
    })

    it('offers a retake link after a failed attempt', () => {
      h.me = baseMe({
        exams: [{ ...notScheduledEntry(), latest_attempt: { result: 'fail', status: 'completed', result_status: 'failed' } }],
      })
      renderWithProviders(<Certifications />)
      expect(screen.getByText('Exam taken')).toBeInTheDocument()
      expect(screen.getByRole('link', { name: /Buy a retake/ })).toBeInTheDocument()
    })
  })

  describe('identity document upload', () => {
    it('rejects an over-size file client-side without calling the API', async () => {
      const user = userEvent.setup()
      h.me = baseMe({ identity_document: null, exams: [] })
      renderWithProviders(<Certifications />)
      const big = new File([new ArrayBuffer(3_000_001)], 'big.pdf', { type: 'application/pdf' })
      await user.upload(document.getElementById('idd-file') as HTMLInputElement, big)
      await user.click(screen.getByRole('button', { name: 'Submit document' }))
      expect(await screen.findByText(/maximum is 3 MB/)).toBeInTheDocument()
      expect(h.post).not.toHaveBeenCalled()
    })

    it('uploads a valid document as a data URI', async () => {
      const user = userEvent.setup()
      h.me = baseMe({ identity_document: null, exams: [] })
      renderWithProviders(<Certifications />)
      const ok = new File(['hello'], 'passport.png', { type: 'image/png' })
      await user.upload(document.getElementById('idd-file') as HTMLInputElement, ok)
      h.post.mockResolvedValueOnce(undefined)
      await user.click(screen.getByRole('button', { name: 'Submit document' }))
      await waitFor(() =>
        expect(h.post).toHaveBeenCalledWith('/api/me/identity-document', expect.objectContaining({ doc_kind: 'passport', filename: 'passport.png' })),
      )
      expect(h.refetch).toHaveBeenCalled()
    })
  })

  describe('exam catalogue (membership gate)', () => {
    it('offers the membership+exam bundle when membership is not active', () => {
      h.me = baseMe({ lifecycle: { membership_status: 'none', blocking_items: [] }, exams: [] })
      h.certs = { rows: [{ id: 1, code: 'PCL-AI', name: 'Project Controls Lead — AI', exam_price: 300 }] }
      renderWithProviders(<Certifications />)
      expect(screen.getByRole('button', { name: 'Pay membership + exam' })).toBeInTheDocument()
      expect(screen.queryByRole('button', { name: 'Pay exam fee' })).not.toBeInTheDocument()
    })

    it('offers the exam fee directly once membership is active', () => {
      h.me = baseMe({ lifecycle: { membership_status: 'active', blocking_items: [] }, exams: [] })
      h.certs = { rows: [{ id: 1, code: 'PCL-AI', name: 'Project Controls Lead — AI', exam_price: 300 }] }
      renderWithProviders(<Certifications />)
      expect(screen.getByRole('button', { name: 'Pay exam fee' })).toBeInTheDocument()
    })

    it('marks an already-owned certification as enrolled (no buy button)', () => {
      h.me = baseMe({ lifecycle: { membership_status: 'active', blocking_items: [] }, exams: [notScheduledEntry()] })
      h.certs = { rows: [{ id: 1, code: 'PCL-AI', name: 'Project Controls Lead — AI', exam_price: 300 }] }
      renderWithProviders(<Certifications />)
      // The catalogue tile for the owned cert shows the disabled "Already enrolled" CTA.
      expect(screen.getByText('Already enrolled')).toBeInTheDocument()
    })
  })
})
