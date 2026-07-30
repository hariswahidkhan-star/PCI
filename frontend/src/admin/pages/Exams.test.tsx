import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, within, render } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'

// FE — the exam-registration queue is worked by DEADLINE, so the decisions pinned here are the
// urgency bucketing derived from days_left (past deadline / due within 14 days / on track — the
// same thresholds that colour the badge), that search spans the candidate's name as one field
// even though the payload carries it as two columns, and that a null days_left is neither
// overdue nor on track (it is excluded from every urgency filter rather than defaulting).
const h = vi.hoisted(() => ({ rows: [] as unknown[], loading: false, error: null as string | null }))

vi.mock('../hooks', () => ({
  useAdminQuery: () => ({ data: { rows: h.rows }, loading: h.loading, error: h.error, refetch: vi.fn() }),
}))

import Exams from './Exams'

const reg = (over: Record<string, unknown> = {}) => ({
  id: 1, first_name: 'Sam', last_name: 'Okafor', email: 'sam@example.com',
  product_type: 'exam', reference: 'PCI-PAY-1', payment_date: '2026-01-05',
  exam_schedule_deadline: '2026-06-05', days_left: 60, ...over,
})

const renderPage = () => render(<MemoryRouter><Exams /></MemoryRouter>)
const bodyRows = () => within(screen.getByRole('table')).getAllByRole('row').slice(1)

describe('Admin exam registrations', () => {
  beforeEach(() => {
    h.rows = []
    h.loading = false
    h.error = null
  })

  it('explains an empty queue rather than showing a bare table', () => {
    renderPage()
    expect(screen.getByText('No paid exam registrations yet')).toBeInTheDocument()
    expect(screen.queryByRole('table')).toBeNull()
    expect(screen.queryByLabelText('Search')).toBeNull()
  })

  it('flags how many registrations are past their deadline', () => {
    h.rows = [reg({ id: 1, days_left: -3 }), reg({ id: 2, days_left: -1 }), reg({ id: 3, days_left: 40 })]
    renderPage()
    expect(screen.getByText('2 past deadline')).toBeInTheDocument()
  })

  it('omits the overdue chip when nothing is late', () => {
    h.rows = [reg({ days_left: 40 })]
    renderPage()
    expect(screen.queryByText(/past deadline/)).toBeNull()
  })

  it('searches the candidate name as one field across first and last name', async () => {
    const user = userEvent.setup()
    h.rows = [reg({ id: 1, first_name: 'Sam', last_name: 'Okafor' }), reg({ id: 2, first_name: 'Mia', last_name: 'Chen' })]
    renderPage()
    await user.type(screen.getByLabelText('Search'), 'sam okafor')
    expect(bodyRows()).toHaveLength(1)
    expect(screen.getByText('Sam Okafor')).toBeInTheDocument()
  })

  it('searches by email and reference too', async () => {
    const user = userEvent.setup()
    h.rows = [reg({ id: 1, reference: 'PCI-PAY-AAA' }), reg({ id: 2, reference: 'PCI-PAY-BBB', email: 'mia@example.com' })]
    renderPage()
    await user.type(screen.getByLabelText('Search'), 'bbb')
    expect(bodyRows()).toHaveLength(1)
    await user.clear(screen.getByLabelText('Search'))
    await user.type(screen.getByLabelText('Search'), 'mia@')
    expect(bodyRows()).toHaveLength(1)
  })

  it('filters to the registrations that are past their deadline', async () => {
    const user = userEvent.setup()
    h.rows = [reg({ id: 1, days_left: -3 }), reg({ id: 2, days_left: 7 }), reg({ id: 3, days_left: 40 })]
    renderPage()
    await user.selectOptions(screen.getByLabelText('Time left'), 'overdue')
    expect(bodyRows()).toHaveLength(1)
    expect(screen.getByText('3d overdue')).toBeInTheDocument()
  })

  it('filters to the registrations due within 14 days', async () => {
    const user = userEvent.setup()
    h.rows = [reg({ id: 1, days_left: -3 }), reg({ id: 2, days_left: 7 }), reg({ id: 3, days_left: 14 }), reg({ id: 4, days_left: 40 })]
    renderPage()
    await user.selectOptions(screen.getByLabelText('Time left'), 'soon')
    expect(bodyRows()).toHaveLength(2) // 7d and 14d — the boundary is inclusive
  })

  it('excludes a registration with no deadline from every urgency filter', async () => {
    const user = userEvent.setup()
    // A null days_left means no scheduling window is recorded. That is genuinely unknown, so it
    // must not be silently bucketed as "on track" — it shows unfiltered and disappears under
    // every urgency filter, rather than defaulting into one of them.
    h.rows = [reg({ id: 1, days_left: null, first_name: 'Nil', last_name: 'Deadline' }), reg({ id: 2, days_left: 40 })]
    renderPage()
    expect(bodyRows()).toHaveLength(2)

    for (const bucket of ['overdue', 'soon', 'ok']) {
      await user.selectOptions(screen.getByLabelText('Time left'), bucket)
      expect(screen.queryByText('Nil Deadline')).toBeNull()
    }
    // and the on-track bucket keeps the row that genuinely is on track
    expect(bodyRows()).toHaveLength(1)
    expect(screen.getByText('40d left')).toBeInTheDocument()
  })

  it('sorts by time left so the most urgent can be brought to the top', async () => {
    const user = userEvent.setup()
    h.rows = [reg({ id: 1, days_left: 40 }), reg({ id: 2, days_left: -5 }), reg({ id: 3, days_left: 9 })]
    renderPage()
    await user.click(screen.getByRole('button', { name: /Time left/ }))
    expect(within(bodyRows()[0]).getByText('5d overdue')).toBeInTheDocument()
  })

  it('offers a way back when the filters match nothing', async () => {
    const user = userEvent.setup()
    h.rows = [reg({ days_left: 40 })]
    renderPage()
    await user.selectOptions(screen.getByLabelText('Time left'), 'overdue')
    expect(screen.getByText('No match')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Clear filters' }))
    expect(screen.getByRole('table')).toBeInTheDocument()
  })

  it('surfaces a load failure', () => {
    h.error = 'Something went wrong.'
    renderPage()
    expect(screen.getByText('Something went wrong.')).toBeInTheDocument()
  })
})
