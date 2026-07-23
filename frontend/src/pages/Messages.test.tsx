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
