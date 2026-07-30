import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, within, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { render } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'

// FE — the generic CRUD factory UI drives ~12 admin content collections from one component, so
// the search/filter/sort/pagination added here lifts every one of them at once. What is pinned:
// the toolbar only appears once there is something to narrow; search spans the list columns;
// select/checkbox columns become dropdown filters that compose with the search; columns sort;
// long collections paginate; and both "nothing here yet" and "nothing matches" are distinct,
// actionable empty states (they call for different responses from the operator).
const h = vi.hoisted(() => ({ rows: [] as Record<string, unknown>[], del: vi.fn(), refetch: vi.fn() }))

vi.mock('./hooks', () => ({
  useAdminQuery: () => ({ data: { rows: h.rows }, loading: false, error: null, refetch: h.refetch }),
}))
vi.mock('./api', () => ({
  adminApi: { post: vi.fn(), patch: vi.fn(), del: (...a: unknown[]) => h.del(...a) },
}))

import CrudSection, { type CrudConfig } from './CrudSection'

const CONFIG: CrudConfig = {
  name: 'faqs',
  path: 'faqs',
  title: 'FAQs',
  perm: 'content',
  description: 'Questions shown on the public site.',
  fields: [
    { key: 'question', label: 'Question', inList: true, required: true },
    { key: 'category', label: 'Category', type: 'select', inList: true, options: [
      { value: 'exams', label: 'Exams' },
      { value: 'fees', label: 'Fees' },
    ] },
    { key: 'published', label: 'Published', type: 'checkbox', inList: true },
    { key: 'body', label: 'Answer', type: 'textarea' },
  ],
}

const row = (id: number, over: Record<string, unknown> = {}) => ({
  id, question: `Question ${id}`, category: 'exams', published: 1, body: '…', ...over,
})

const renderSection = () => render(<MemoryRouter><CrudSection config={CONFIG} /></MemoryRouter>)
const bodyRows = () => within(screen.getByRole('table')).getAllByRole('row').slice(1) // drop the header

describe('CrudSection (premium list)', () => {
  beforeEach(() => {
    h.rows = []
    h.del.mockReset()
    h.refetch.mockReset()
  })

  it('shows an actionable empty state and no toolbar for an empty collection', () => {
    renderSection()
    expect(screen.getByText('No faqs yet')).toBeInTheDocument()
    expect(screen.queryByLabelText('Search')).toBeNull()
    expect(screen.queryByRole('table')).toBeNull()
  })

  it('renders the collection with a record count once there is data', () => {
    h.rows = [row(1), row(2)]
    renderSection()
    expect(screen.getByText('2 records')).toBeInTheDocument()
    expect(bodyRows()).toHaveLength(2)
  })

  it('searches across the listed columns', async () => {
    const user = userEvent.setup()
    h.rows = [row(1, { question: 'How do I book an exam?' }), row(2, { question: 'When is my fee due?' })]
    renderSection()
    await user.type(screen.getByLabelText('Search'), 'book')
    expect(bodyRows()).toHaveLength(1)
    expect(screen.getByText('How do I book an exam?')).toBeInTheDocument()
    expect(screen.getByText('1 of 2 shown')).toBeInTheDocument()
  })

  it('offers a dropdown filter for each select column and composes it with the search', async () => {
    const user = userEvent.setup()
    h.rows = [
      row(1, { question: 'Exam question', category: 'exams' }),
      row(2, { question: 'Fee question', category: 'fees' }),
      row(3, { question: 'Another fee question', category: 'fees' }),
    ]
    renderSection()
    await user.selectOptions(screen.getByLabelText('Category'), 'fees')
    expect(bodyRows()).toHaveLength(2)
    await user.type(screen.getByLabelText('Search'), 'another')
    expect(bodyRows()).toHaveLength(1)
  })

  it('offers a yes/no filter for each checkbox column', async () => {
    const user = userEvent.setup()
    h.rows = [row(1, { published: 1 }), row(2, { published: 0 })]
    renderSection()
    await user.selectOptions(screen.getByLabelText('Published'), 'no')
    expect(bodyRows()).toHaveLength(1)
    expect(screen.getByText('Question 2')).toBeInTheDocument()
  })

  it('sorts by a column and reverses on the second click', async () => {
    const user = userEvent.setup()
    h.rows = [row(1, { question: 'Beta' }), row(2, { question: 'Alpha' })]
    renderSection()
    const header = screen.getByRole('button', { name: /Question/ })

    await user.click(header)
    expect(within(bodyRows()[0]).getByText('Alpha')).toBeInTheDocument()
    expect(screen.getByRole('columnheader', { name: /Question/ })).toHaveAttribute('aria-sort', 'ascending')

    await user.click(header)
    expect(within(bodyRows()[0]).getByText('Beta')).toBeInTheDocument()
    expect(screen.getByRole('columnheader', { name: /Question/ })).toHaveAttribute('aria-sort', 'descending')
  })

  it('paginates a long collection', async () => {
    const user = userEvent.setup()
    h.rows = Array.from({ length: 30 }, (_, i) => row(i + 1))
    renderSection()
    expect(bodyRows()).toHaveLength(25)
    expect(screen.getByText('Showing 1–25 of 30')).toBeInTheDocument()
    await user.click(screen.getByLabelText('Next page'))
    expect(bodyRows()).toHaveLength(5)
  })

  it('distinguishes "nothing matches" from "nothing here yet" and can clear back', async () => {
    const user = userEvent.setup()
    h.rows = [row(1)]
    renderSection()
    await user.type(screen.getByLabelText('Search'), 'zzzz')
    expect(screen.getByText('No match')).toBeInTheDocument()
    expect(screen.queryByText('No faqs yet')).toBeNull()
    await user.click(screen.getByRole('button', { name: 'Clear filters' }))
    expect(screen.getByRole('table')).toBeInTheDocument()
  })

  it('still opens the editor drawer and deletes a row', async () => {
    const user = userEvent.setup()
    vi.spyOn(window, 'confirm').mockReturnValue(true)
    h.rows = [row(1)]
    renderSection()

    await user.click(screen.getByRole('button', { name: 'Edit' }))
    expect(screen.getByRole('dialog', { name: 'Edit faq' })).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Close' }))

    await user.click(screen.getByRole('button', { name: 'Delete' }))
    await waitFor(() => expect(h.del).toHaveBeenCalledWith('/api/admin/faqs/1'))
  })
})
