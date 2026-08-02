import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen } from '@testing-library/react'
import { renderWithProviders } from '../test/utils'

// FE — the exam-day check-in mirrors the server's launch gates: the window phases (countdown /
// open / closed) come from the booking slot, and the launch button stays disabled until the
// checklist AND (when required) a passed readiness check are done. The server still re-checks
// everything on /api/me/exam/start — this page only tells the truth about it.
const h = vi.hoisted(() => ({
  me: null as unknown,
  queries: {} as Record<string, unknown>,
  post: vi.fn(),
}))

vi.mock('../data/MeContext', () => ({
  useMe: () => ({ me: h.me, loading: false, error: null, refetch: vi.fn() }),
}))
vi.mock('../api/hooks', () => ({
  useQuery: (path: string | null) => ({
    data: path ? h.queries[path.split('?')[0]] ?? null : null,
    loading: false,
    error: null,
    refetch: vi.fn(),
  }),
}))
vi.mock('../api/client', () => ({
  api: { get: vi.fn(), post: (p: string, b?: unknown) => h.post(p, b) },
  ApiError: class ApiError extends Error {
    body: unknown
    constructor(status: number, msg: string, body: unknown) { super(msg); this.body = body }
  },
}))

import ExamDay from './ExamDay'

// SQLite-style UTC datetime for a JS instant.
const sqlite = (ms: number) => new Date(ms).toISOString().slice(0, 19).replace('T', ' ')

function mkMe(slotMs: number | null) {
  return {
    exams: slotMs === null ? [] : [{
      certification_id: 2,
      certification_code: 'PCL-AI',
      certification_name: 'Certified AI Project Controls Professional',
      booking: { id: 5, scheduled_at: sqlite(slotMs), timezone: 'UTC', status: 'scheduled' },
    }],
  }
}

describe('ExamDay (check-in inside the portal)', () => {
  beforeEach(() => {
    h.me = null
    h.queries = {
      '/api/me/readiness': { required: true, latest: null },
      '/api/me/config': { exam_open_before_minutes: '15', sp_readiness_required: '1' },
      '/api/me/exam/delivery': { routed: false },
    }
    h.post.mockReset()
    localStorage.clear()
  })

  it('shows the empty state when nothing is scheduled', () => {
    h.me = mkMe(null)
    renderWithProviders(<ExamDay />)
    expect(screen.getByText('No scheduled examination')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Go to Certifications' })).toHaveAttribute('href', '/certifications')
  })

  it('counts down while the launch window has not opened', () => {
    h.me = mkMe(Date.now() + 2 * 3600_000)
    renderWithProviders(<ExamDay />)
    expect(screen.getByText('until launch opens')).toBeInTheDocument()
    expect(screen.getByText(/Launch activates 15 minutes before/)).toBeInTheDocument()
  })

  it('inside the window, launch stays disabled until the checklist and readiness pass', () => {
    h.me = mkMe(Date.now() + 5 * 60_000)   // open = slot−15min (past), close = slot+30min (future)
    renderWithProviders(<ExamDay />)
    const btn = screen.getByRole('button', { name: 'Launch my examination' })
    expect(btn).toBeDisabled()
    expect(screen.getByText(/Run and pass the system readiness check/)).toBeInTheDocument()
  })

  it('inside the window, launch enables once the checklist is done and readiness passed', () => {
    localStorage.setItem('pci.examday.check', JSON.stringify([true, true, true, true]))
    h.queries['/api/me/readiness'] = { required: true, latest: { passed: 1 } }
    h.me = mkMe(Date.now() + 5 * 60_000)
    renderWithProviders(<ExamDay />)
    expect(screen.getByRole('button', { name: 'Launch my examination' })).toBeEnabled()
    expect(screen.getByRole('button', { name: 'Open in the PCI Secure Exam app instead' })).toBeInTheDocument()
  })

  it('after the grace period the window reads closed with a support path', () => {
    h.me = mkMe(Date.now() - 3600_000)
    renderWithProviders(<ExamDay />)
    expect(screen.getByText('Launch window closed')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Open a ticket' })).toHaveAttribute('href', '/support')
  })

  it('a vendor-delivered exam shows the provider panel instead of a launch button', () => {
    h.queries['/api/me/exam/delivery'] = { routed: true, provider_name: 'TestVendor Inc', confirmation: 'TV-99' }
    h.me = mkMe(Date.now() + 5 * 60_000)
    renderWithProviders(<ExamDay />)
    expect(screen.getByText('TestVendor Inc')).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Launch my examination' })).not.toBeInTheDocument()
  })
})
