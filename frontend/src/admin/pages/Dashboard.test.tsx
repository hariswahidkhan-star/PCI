import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, within } from '@testing-library/react'
import { render } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'

// FE — the operations console's landing screen. Decisions worth pinning: every KPI/queue link is
// permission-gated (an admin is never sent to a section they'd get a 403 from); the "needs
// attention" band lists only queues that actually have work, so a quiet console stays quiet; and
// the 12-month revenue trend — returned by /api/admin/overview all along but never rendered by the
// previous dashboard — is now shown.
const h = vi.hoisted(() => ({
  data: null as unknown,
  loading: false,
  error: null as string | null,
  isOwner: true,
  perms: [] as string[],
}))

vi.mock('../hooks', () => ({
  useAdminQuery: () => ({ data: h.data, loading: h.loading, error: h.error, refetch: vi.fn() }),
}))
vi.mock('../AdminAuth', () => ({
  useAdminAuth: () => ({
    me: { id: 1, email: 'a@pci.local', name: 'A', role: h.isOwner ? 'owner' : 'viewer', is_owner: h.isOwner, must_change_pw: false, permissions: h.perms },
    logout: vi.fn(),
    can: (p: string) => h.perms.includes(p),
  }),
}))

import Dashboard from './Dashboard'

const KPIS = {
  members: 120, activeMembers: 90, pendingMembers: 4, inProgress: 2, paidSessions: 40,
  abandoned: 3, payCount: 55, revenue: 41000, revenueMonth: 5200, refunded: 1, failedPay: 2,
  credActive: 33, credTotal: 40, openInq: 6, examsDue: 7,
}

const mkData = (over: Record<string, unknown> = {}) => ({
  kpis: { ...KPIS },
  revSeries: [
    { ym: '2026-01', amount: 1000, n: 3 },
    { ym: '2026-02', amount: 4200, n: 9 },
  ],
  productMix: [
    { product_type: 'exam', n: 30, amount: 30000 },
    { product_type: 'membership', n: 25, amount: 11000 },
  ],
  funnel: { started: 100, in_progress: 2, paid: 40 },
  recent: [{ ts: '2026-03-01 10:00:00', action: 'cred_issue', details: 'PCI-100' }],
  ...over,
})

const renderDash = () => render(<MemoryRouter><Dashboard /></MemoryRouter>)

describe('Admin Dashboard', () => {
  beforeEach(() => {
    h.data = mkData()
    h.loading = false
    h.error = null
    h.isOwner = true
    h.perms = []
  })

  it('shows a shaped loading state rather than a bare spinner', () => {
    h.loading = true
    const { container } = renderDash()
    expect(container.querySelectorAll('.skeleton').length).toBeGreaterThan(0)
  })

  it('surfaces a load failure', () => {
    h.error = 'Something went wrong.'
    renderDash()
    expect(screen.getByText('Something went wrong.')).toBeInTheDocument()
  })

  it('renders the headline KPIs with their context lines', () => {
    renderDash()
    const tile = screen.getByText('Students').closest('.kpi') as HTMLElement
    expect(within(tile).getByText('120')).toBeInTheDocument()
    expect(within(tile).getByText('90')).toBeInTheDocument() // active
    expect(within(tile).getByText('4')).toBeInTheDocument() // pending
  })

  it('combines failed and refunded payments into one exceptions KPI', () => {
    renderDash()
    const tile = screen.getByText('Payment exceptions').closest('.kpi') as HTMLElement
    expect(within(tile).getByText('3')).toBeInTheDocument() // 2 failed + 1 refunded
    expect(tile).toHaveClass('tone-err')
  })

  it('lists only the queues that actually have work', () => {
    h.data = mkData({ kpis: { ...KPIS, pendingMembers: 0, openInq: 0, abandoned: 0, failedPay: 0, refunded: 0 } })
    const { container } = renderDash()
    const queue = container.querySelector('.queue') as HTMLElement
    expect(within(queue).getByText('Book unscheduled exams')).toBeInTheDocument()
    expect(within(queue).queryByText('Reply to enquiries')).toBeNull()
  })

  it('says so plainly when nothing is waiting', () => {
    h.data = mkData({ kpis: { ...KPIS, pendingMembers: 0, openInq: 0, abandoned: 0, failedPay: 0, refunded: 0, examsDue: 0 } })
    renderDash()
    expect(screen.getByText('Nothing is waiting')).toBeInTheDocument()
  })

  it('renders the 12-month revenue trend the previous dashboard dropped', () => {
    const { container } = renderDash()
    expect(screen.getByText('Revenue — last 12 months')).toBeInTheDocument()
    expect(container.querySelectorAll('.trend-cols > i')).toHaveLength(2)
    expect(screen.getByText('Jan 26')).toBeInTheDocument()
    expect(screen.getByText('Feb 26')).toBeInTheDocument()
  })

  it('reports the enrolment conversion rate', () => {
    renderDash()
    expect(screen.getByText('40% converted')).toBeInTheDocument() // 40 paid of 100 started
  })

  it('omits the conversion chip when nobody has started an enrolment', () => {
    h.data = mkData({ funnel: { started: 0, in_progress: 0, paid: 0 } })
    renderDash()
    expect(screen.queryByText(/converted/)).toBeNull()
    expect(screen.getByText('No enrolments yet')).toBeInTheDocument()
  })

  it('links KPIs and queues for an owner', () => {
    renderDash()
    expect(screen.getByText('Students').closest('a')).toHaveAttribute('href', '/students')
    expect(screen.getByText('Book unscheduled exams').closest('a')).toHaveAttribute('href', '/exams')
  })

  it('never links a section the signed-in admin cannot open', () => {
    h.isOwner = false
    h.perms = ['exams'] // exam manager: no members, no payments
    renderDash()
    expect(screen.getByText('Students').closest('a')).toBeNull()
    expect(screen.getByText('Book unscheduled exams').closest('a')).toHaveAttribute('href', '/exams')
    // a payments queue they cannot open is dropped from "needs attention" entirely
    expect(screen.queryByText('Resolve failed charges')).toBeNull()
  })
})
