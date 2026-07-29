import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE — the in-portal messages inbox. Decisions worth pinning: the unread count gates the "Mark all
// read" control and each message's New badge + per-message "Mark as read" action; marking read/all
// POSTs to the right endpoint; and field() falls back across alternative key names for the title/body.
// useMe/useQuery/runMutation and the fetch client are mocked at the module boundary.
const h = vi.hoisted(() => ({ rows: [] as unknown[], post: vi.fn(), refetch: vi.fn(), refetchMe: vi.fn() }))

vi.mock('../data/MeContext', () => ({ useMe: () => ({ refetch: h.refetchMe }) }))
vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: { rows: h.rows }, loading: false, error: null, refetch: h.refetch }),
  runMutation: (fn: () => Promise<void>) => fn(),
}))
vi.mock('../api/client', () => ({ api: { post: (...args: unknown[]) => h.post(...args) } }))

import Messages from './Messages'

const msg = (over: Record<string, unknown> = {}) => ({
  id: 1, title: 'Welcome', body: 'Hello there', created_at: '2026-02-01T00:00:00Z', read_at: null, ...over,
})

describe('Messages (inbox)', () => {
  beforeEach(() => {
    h.rows = []
    h.post.mockReset()
    h.refetch.mockReset()
    h.refetchMe.mockReset()
  })

  it('shows the empty state when there are no messages', () => {
    renderWithProviders(<Messages />)
    expect(screen.getByText('No messages yet.')).toBeInTheDocument()
  })

  it('flags an unread message and marks it read on click', async () => {
    const user = userEvent.setup()
    h.rows = [msg({ read_at: null })]
    renderWithProviders(<Messages />)
    expect(screen.getByText('New')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Mark all read' })).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Mark as read' }))
    await waitFor(() => expect(h.post).toHaveBeenCalled())
    expect(h.post.mock.calls[0][0]).toBe('/api/me/messages/1/read')
  })

  it('hides the read controls once a message has been read', () => {
    h.rows = [msg({ read_at: '2026-02-02T00:00:00Z' })]
    renderWithProviders(<Messages />)
    expect(screen.queryByText('New')).toBeNull()
    expect(screen.queryByRole('button', { name: 'Mark as read' })).toBeNull()
    expect(screen.queryByRole('button', { name: 'Mark all read' })).toBeNull() // unread count is 0
  })

  it('marks every message read from the header control', async () => {
    const user = userEvent.setup()
    h.rows = [msg({ id: 1 }), msg({ id: 2, title: 'Second' })]
    renderWithProviders(<Messages />)
    await user.click(screen.getByRole('button', { name: 'Mark all read' }))
    await waitFor(() => expect(h.post).toHaveBeenCalled())
    expect(h.post.mock.calls[0][0]).toBe('/api/me/messages/read-all')
  })

  it('falls back across alternative key names for the title and body', () => {
    h.rows = [{ id: 3, subject: 'Payment received', content: 'Your fee has been recorded.', sent_at: '2026-02-01T00:00:00Z', read_at: '2026-02-01T00:00:00Z' }]
    renderWithProviders(<Messages />)
    expect(screen.getByText('Payment received')).toBeInTheDocument() // subject → title
    expect(screen.getByText('Your fee has been recorded.')).toBeInTheDocument() // content → body
  })
})

// ---- premium inbox upgrade: the notification's own action, plus search/state/category filters.
// cta_label/cta_route are written by the backend (Support replies, new documents, exam-exception
// updates, founding access…) and were previously stored but never rendered — following one is the
// step that actually resolves the notification, so it must reach the student.
describe('Messages (premium inbox)', () => {
  beforeEach(() => {
    h.rows = []
    h.post.mockReset()
    h.refetch.mockReset()
    h.refetchMe.mockReset()
  })

  const withCta = (over: Record<string, unknown> = {}) =>
    msg({ id: 9, category: 'Support', cta_label: 'View reply', cta_route: '/support', ...over })

  // The support journey's e2e spec locates a notification with
  // `page.locator('.card').filter({ hasText: ticket.reference })`. Pinning the hook here means
  // a future restyle fails in seconds instead of eight minutes into the Playwright job.
  it('keeps the .card hook on each message row', () => {
    h.rows = [msg({ id: 7, title: 'Support replied to your ticket', body: 'TKT-ABC123' })]
    const { container } = renderWithProviders(<Messages />)
    const row = container.querySelector('.msg-row') as HTMLElement
    expect(row).toHaveClass('card')
    expect(row).toHaveTextContent('TKT-ABC123')
  })

  it('renders the notification’s call to action as a link to its route', () => {
    h.rows = [withCta()]
    renderWithProviders(<Messages />)
    expect(screen.getByRole('link', { name: /View reply/ })).toHaveAttribute('href', '/support')
  })

  it('falls back to a generic label when only a route is stored', () => {
    h.rows = [withCta({ cta_label: null })]
    renderWithProviders(<Messages />)
    expect(screen.getByRole('link', { name: /Open/ })).toHaveAttribute('href', '/support')
  })

  it('ignores a cta_route that is not an in-portal path', () => {
    h.rows = [withCta({ cta_route: 'https://example.com/phish' })]
    renderWithProviders(<Messages />)
    expect(screen.queryByRole('link', { name: /View reply/ })).toBeNull()
  })

  it('following the action also clears the unread flag', async () => {
    const user = userEvent.setup()
    h.rows = [withCta({ read_at: null })]
    renderWithProviders(<Messages />)
    await user.click(screen.getByRole('link', { name: /View reply/ }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/messages/9/read'))
  })

  it('narrows the inbox to unread notifications', async () => {
    const user = userEvent.setup()
    h.rows = [msg({ id: 1, title: 'Unread one', read_at: null }), msg({ id: 2, title: 'Already read', read_at: '2026-02-02T00:00:00Z' })]
    renderWithProviders(<Messages />)
    expect(screen.getByText('Already read')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: /Unread/ }))
    expect(screen.queryByText('Already read')).toBeNull()
    expect(screen.getByText('Unread one')).toBeInTheDocument()
  })

  it('searches across the title and body', async () => {
    const user = userEvent.setup()
    h.rows = [msg({ id: 1, title: 'Exam booked', body: 'See you there' }), msg({ id: 2, title: 'Fee received', body: 'Thank you' })]
    renderWithProviders(<Messages />)
    await user.type(screen.getByLabelText('Search notifications'), 'thank')
    expect(screen.getByText('Fee received')).toBeInTheDocument()
    expect(screen.queryByText('Exam booked')).toBeNull()
  })

  it('offers a category filter only once more than one category is present', () => {
    h.rows = [msg({ id: 1, category: 'Support' })]
    const { unmount } = renderWithProviders(<Messages />)
    expect(screen.queryByLabelText('Category')).toBeNull()
    unmount()

    h.rows = [msg({ id: 1, category: 'Support' }), msg({ id: 2, category: 'Documents' })]
    renderWithProviders(<Messages />)
    expect(screen.getByLabelText('Category')).toBeInTheDocument()
  })

  it('explains an over-filtered inbox and offers a way back', async () => {
    const user = userEvent.setup()
    h.rows = [msg({ id: 1, title: 'Exam booked' })]
    renderWithProviders(<Messages />)
    await user.type(screen.getByLabelText('Search notifications'), 'nothing matches this')
    expect(screen.getByText('No notification matches')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Clear filters' }))
    expect(screen.getByText('Exam booked')).toBeInTheDocument()
  })
})
