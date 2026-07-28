import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, within, render } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'

// FE — the support queue. What is pinned: status stays a SERVER-side filter (it re-queries with
// ?status=, it does not filter a local copy), search runs client-side over whatever came back,
// the queue opens sorted by most-recent activity, and "no tickets in this status" is a different
// message from "no tickets at all" — they call for different responses from the operator.
const h = vi.hoisted(() => ({ rows: [] as unknown[], loading: false, error: null as string | null, paths: [] as (string | null)[] }))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    h.paths.push(path)
    return { data: { rows: h.rows }, loading: h.loading, error: h.error, refetch: vi.fn() }
  },
  runMutation: (fn: () => Promise<void>) => fn(),
}))
vi.mock('../api', () => ({ adminApi: { post: vi.fn() } }))

import Tickets from './Tickets'

const ticket = (over: Record<string, unknown> = {}) => ({
  id: 1, reference: 'TKT-001', subject: 'Cannot schedule my exam', email: 'sam@example.com',
  status: 'open', msg_count: 2, updated_at: '2026-03-01 10:00:00', ...over,
})

let view: ReturnType<typeof render>
const renderPage = () => (view = render(<MemoryRouter><Tickets /></MemoryRouter>))
// Each <tr> carries role="button" (rowActivate, so the drawer is keyboard-reachable), which
// overrides the implicit "row" role — so these rows are not findable via getAllByRole('row').
const bodyRows = () => Array.from(view.container.querySelectorAll('tbody tr')) as HTMLElement[]

describe('Admin support tickets', () => {
  beforeEach(() => {
    h.rows = []
    h.loading = false
    h.error = null
    h.paths = []
  })

  it('distinguishes an empty queue from an empty status filter', async () => {
    const user = userEvent.setup()
    renderPage()
    expect(screen.getByText('No support tickets yet')).toBeInTheDocument()

    await user.selectOptions(screen.getByLabelText('Status'), 'closed')
    expect(screen.getByText('No tickets with this status')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Clear status filter' }))
    expect(screen.getByText('No support tickets yet')).toBeInTheDocument()
  })

  it('re-queries the server when the status filter changes', async () => {
    const user = userEvent.setup()
    h.rows = [ticket()]
    renderPage()
    expect(h.paths).toContain('/api/admin/tickets')

    await user.selectOptions(screen.getByLabelText('Status'), 'resolved')
    expect(h.paths).toContain('/api/admin/tickets?status=resolved')
  })

  it('searches client-side across reference, subject and student', async () => {
    const user = userEvent.setup()
    h.rows = [
      ticket({ id: 1, reference: 'TKT-AAA', subject: 'Exam scheduling', email: 'sam@example.com' }),
      ticket({ id: 2, reference: 'TKT-BBB', subject: 'Invoice query', email: 'mia@example.com' }),
    ]
    renderPage()
    const pathsBefore = h.paths.length

    await user.type(screen.getByLabelText('Search'), 'invoice')
    expect(bodyRows()).toHaveLength(1)
    expect(screen.getByText('Invoice query')).toBeInTheDocument()
    // searching must NOT hit the server — it narrows what is already loaded
    expect(h.paths.slice(pathsBefore).every((p) => p === '/api/admin/tickets')).toBe(true)

    await user.clear(screen.getByLabelText('Search'))
    await user.type(screen.getByLabelText('Search'), 'TKT-AAA')
    expect(bodyRows()).toHaveLength(1)
  })

  it('opens sorted by most recent activity', () => {
    h.rows = [
      ticket({ id: 1, reference: 'TKT-OLD', updated_at: '2026-01-01 09:00:00' }),
      ticket({ id: 2, reference: 'TKT-NEW', updated_at: '2026-05-01 09:00:00' }),
    ]
    renderPage()
    expect(within(bodyRows()[0]).getByText('TKT-NEW')).toBeInTheDocument()
  })

  it('reports how many tickets the search narrowed to', async () => {
    const user = userEvent.setup()
    h.rows = [ticket({ id: 1, subject: 'Alpha' }), ticket({ id: 2, subject: 'Beta' }), ticket({ id: 3, subject: 'Gamma' })]
    renderPage()
    expect(screen.getByText('3 tickets')).toBeInTheDocument()
    await user.type(screen.getByLabelText('Search'), 'alpha')
    expect(screen.getByText('1 of 3 shown')).toBeInTheDocument()
  })

  it('explains an over-narrow search without losing the status filter', async () => {
    const user = userEvent.setup()
    h.rows = [ticket()]
    renderPage()
    await user.type(screen.getByLabelText('Search'), 'zzzzz')
    expect(screen.getByText('No match')).toBeInTheDocument()
    const emptyState = view.container.querySelector('.empty-state') as HTMLElement
    await user.click(within(emptyState).getByRole('button', { name: 'Clear search' }))
    expect(screen.getByRole('table')).toBeInTheDocument()
  })

  it('keeps rows keyboard-activatable so the drawer is reachable without a mouse', () => {
    h.rows = [ticket()]
    renderPage()
    expect(bodyRows()[0]).toHaveAttribute('role', 'button')
    expect(bodyRows()[0]).toHaveAttribute('tabindex', '0')
  })

  it('surfaces a load failure', () => {
    h.error = 'Something went wrong.'
    renderPage()
    expect(screen.getByText('Something went wrong.')).toBeInTheDocument()
  })
})
