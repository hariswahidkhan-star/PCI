import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE — the newsletter list is the one admin collection with no natural ceiling, and until the
// search/filter/sort/pagination added here the only way to find one address among thousands was
// find-in-page. What is pinned: a subscriber with NO status recorded counts as subscribed (the
// row action already assumes that, so the filter must agree or the counts contradict the badges);
// everything narrows client-side without a re-query; and the list opens newest-first.
const h = vi.hoisted(() => ({ rows: [] as unknown[], paths: [] as (string | null)[], patch: vi.fn() }))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    h.paths.push(path)
    return { data: { rows: h.rows }, loading: false, error: null, refetch: vi.fn() }
  },
  runMutation: (fn: () => Promise<void>) => fn(),
}))
vi.mock('../api', () => ({ adminApi: { patch: (...a: unknown[]) => h.patch(...a) } }))

import Subscribers from './Subscribers'

const sub = (over: Record<string, unknown> = {}) => ({
  id: 1, email: 'sam@example.com', status: 'subscribed', created_at: '2026-03-01T00:00:00Z', ...over,
})

let view: ReturnType<typeof render>
const renderPage = () => (view = render(<Subscribers />))
const bodyRows = () => Array.from(view.container.querySelectorAll('tbody tr')) as HTMLElement[]

describe('Newsletter subscribers', () => {
  beforeEach(() => {
    h.rows = []
    h.paths = []
    h.patch.mockReset().mockResolvedValue({})
  })

  it('explains an empty list and offers no toolbar to narrow nothing', () => {
    renderPage()
    expect(screen.getByText('No subscribers yet')).toBeInTheDocument()
    expect(screen.queryByLabelText('Search')).toBeNull()
  })

  it('counts a subscriber with no recorded status as subscribed', async () => {
    const user = userEvent.setup()
    h.rows = [sub({ id: 1, status: null, email: 'implicit@example.com' }), sub({ id: 2, status: 'unsubscribed', email: 'off@example.com' })]
    renderPage()
    // Scope to the filter group: the "Subscribed" column sort-header is also a button, so an
    // unscoped name query matches two controls.
    const filters = screen.getByRole('group', { name: 'Filter by subscription state' })
    // the segmented counts must agree with the badges the rows render
    expect(within(filters).getByRole('button', { name: /Subscribed 1/ })).toBeInTheDocument()
    expect(within(filters).getByRole('button', { name: /Unsubscribed 1/ })).toBeInTheDocument()

    await user.click(within(filters).getByRole('button', { name: /^Subscribed/ }))
    expect(bodyRows()).toHaveLength(1)
    expect(screen.getByText('implicit@example.com')).toBeInTheDocument()
  })

  it('searches by email without re-querying the server', async () => {
    const user = userEvent.setup()
    h.rows = [sub({ id: 1, email: 'alpha@example.com' }), sub({ id: 2, email: 'beta@example.com' })]
    renderPage()
    const before = h.paths.length

    await user.type(screen.getByLabelText('Search'), 'beta')
    expect(bodyRows()).toHaveLength(1)
    expect(h.paths.slice(before).every((p) => p === '/api/admin/subscribers')).toBe(true)
  })

  it('opens newest first', () => {
    h.rows = [
      sub({ id: 1, email: 'old@example.com', created_at: '2020-01-01T00:00:00Z' }),
      sub({ id: 2, email: 'new@example.com', created_at: '2026-06-01T00:00:00Z' }),
    ]
    renderPage()
    expect(within(bodyRows()[0]).getByText('new@example.com')).toBeInTheDocument()
  })

  it('reports how far the filters narrowed the list', async () => {
    const user = userEvent.setup()
    h.rows = [sub({ id: 1, email: 'a@x.com' }), sub({ id: 2, email: 'b@x.com' }), sub({ id: 3, email: 'c@x.com' })]
    renderPage()
    expect(screen.getByText('3 total')).toBeInTheDocument()
    await user.type(screen.getByLabelText('Search'), 'a@')
    expect(screen.getByText('1 of 3 shown')).toBeInTheDocument()
  })

  it('offers a way back from an over-narrow filter', async () => {
    const user = userEvent.setup()
    h.rows = [sub()]
    renderPage()
    await user.type(screen.getByLabelText('Search'), 'nobody')
    expect(screen.getByText('No match')).toBeInTheDocument()
    const empty = view.container.querySelector('.empty-state') as HTMLElement
    await user.click(within(empty).getByRole('button', { name: 'Clear filters' }))
    expect(bodyRows()).toHaveLength(1)
  })

  it('still toggles a subscription from the row', async () => {
    const user = userEvent.setup()
    h.rows = [sub({ id: 5 })]
    renderPage()
    await user.click(screen.getByRole('button', { name: 'Unsubscribe' }))
    expect(h.patch).toHaveBeenCalledWith('/api/admin/subscribers/5', { status: 'unsubscribed' })
  })
})
