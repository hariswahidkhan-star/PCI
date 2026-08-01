import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE — the student topbar's notification centre. The decisions worth pinning: the unread badge
// comes from the already-loaded `me` payload (so the bell costs no request until it is opened);
// the list is fetched only on open; following a notification's cta_route marks it read AND
// navigates; and Escape/outside-click dismiss the popover.
const h = vi.hoisted(() => ({
  me: { unread: 0 } as { unread: number } | null,
  refetchMe: vi.fn(),
  get: vi.fn(),
  post: vi.fn(),
  navigate: vi.fn(),
}))

vi.mock('../data/MeContext', () => ({ useMe: () => ({ me: h.me, refetch: h.refetchMe }) }))
vi.mock('../api/client', () => ({
  api: {
    get: (...a: unknown[]) => h.get(...a),
    post: (...a: unknown[]) => h.post(...a),
  },
}))
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual<typeof import('react-router-dom')>('react-router-dom')
  return { ...actual, useNavigate: () => h.navigate }
})

import NotificationBell from './NotificationBell'

const notif = (over: Record<string, unknown> = {}) => ({
  id: 1, category: 'Support', title: 'Support replied to your ticket',
  body: 'We have answered your question.', cta_label: 'View reply', cta_route: '/support',
  created_at: '2026-03-01T00:00:00Z', read_at: null, ...over,
})

describe('NotificationBell', () => {
  beforeEach(() => {
    h.me = { unread: 0 }
    h.refetchMe.mockReset()
    h.navigate.mockReset()
    h.post.mockReset().mockResolvedValue({})
    h.get.mockReset().mockResolvedValue({ rows: [notif()] })
  })

  it('shows no badge and fetches nothing until it is opened', () => {
    renderWithProviders(<NotificationBell />)
    expect(screen.queryByText('1')).toBeNull()
    expect(h.get).not.toHaveBeenCalled()
  })

  it('carries the unread count from the me payload', () => {
    h.me = { unread: 3 }
    renderWithProviders(<NotificationBell />)
    expect(screen.getByText('3')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Notifications — 3 unread' })).toBeInTheDocument()
  })

  it('caps a very large unread count at 99+', () => {
    h.me = { unread: 250 }
    renderWithProviders(<NotificationBell />)
    expect(screen.getByText('99+')).toBeInTheDocument()
  })

  it('loads the list on open and renders the notification', async () => {
    const user = userEvent.setup()
    h.me = { unread: 1 }
    renderWithProviders(<NotificationBell />)
    await user.click(screen.getByRole('button', { name: /Notifications/ }))
    await waitFor(() => expect(h.get).toHaveBeenCalledWith('/api/me/messages'))
    expect(await screen.findByText('Support replied to your ticket')).toBeInTheDocument()
  })

  it('following a notification marks it read and navigates to its cta_route', async () => {
    const user = userEvent.setup()
    h.me = { unread: 1 }
    renderWithProviders(<NotificationBell />)
    await user.click(screen.getByRole('button', { name: /Notifications/ }))
    await user.click(await screen.findByText('Support replied to your ticket'))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/messages/1/read'))
    expect(h.navigate).toHaveBeenCalledWith('/support')
  })

  it('marks everything read from the popover header', async () => {
    const user = userEvent.setup()
    h.me = { unread: 2 }
    renderWithProviders(<NotificationBell />)
    await user.click(screen.getByRole('button', { name: /Notifications/ }))
    await user.click(await screen.findByRole('button', { name: 'Mark all read' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/messages/read-all'))
    expect(h.refetchMe).toHaveBeenCalled()
  })

  it('shows the empty copy when there is nothing to read', async () => {
    const user = userEvent.setup()
    h.get.mockResolvedValue({ rows: [] })
    renderWithProviders(<NotificationBell />)
    await user.click(screen.getByRole('button', { name: /Notifications/ }))
    expect(await screen.findByText('No messages yet.')).toBeInTheDocument()
  })

  it('closes on Escape', async () => {
    const user = userEvent.setup()
    renderWithProviders(<NotificationBell />)
    const bell = screen.getByRole('button', { name: /Notifications/ })
    await user.click(bell)
    expect(await screen.findByRole('dialog')).toBeInTheDocument()
    await user.keyboard('{Escape}')
    await waitFor(() => expect(screen.queryByRole('dialog')).toBeNull())
  })

  it('keeps the popover usable when the notification fetch fails', async () => {
    const user = userEvent.setup()
    h.get.mockRejectedValue(new Error('offline'))
    renderWithProviders(<NotificationBell />)
    await user.click(screen.getByRole('button', { name: /Notifications/ }))
    // an honest failure state with a retry — never the misleading "No messages yet." empty state
    expect(await screen.findByText(/Could not load notifications/)).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'View all notifications' })).toBeInTheDocument()
    // retry recovers once the network is back
    h.get.mockResolvedValue({ rows: [] })
    await user.click(screen.getByRole('button', { name: 'Retry' }))
    expect(await screen.findByText('No messages yet.')).toBeInTheDocument()
  })
})
