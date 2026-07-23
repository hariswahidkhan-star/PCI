import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE — member events & webinars. The decisions worth pinning: an unregistered event with no seats is
// "Fully booked" (disabled); registering/cancelling POST to the right sub-path and refetch; a
// registered event exposes the Join link + Cancel action and its badge, while an attended one drops
// the Cancel action; the CPD-hours badge shows only when there are hours; and a structured ApiError
// surfaces its body.message. useQuery + the fetch client are mocked; the REAL ApiError is kept.
const h = vi.hoisted(() => ({ rows: [] as unknown[], post: vi.fn(), refetch: vi.fn() }))

vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: { rows: h.rows }, loading: false, error: null, refetch: h.refetch }),
}))
vi.mock('../api/client', async (orig) => {
  const actual = await orig<typeof import('../api/client')>()
  return { ...actual, api: { post: (p: string, b?: unknown) => h.post(p, b) } }
})

import Events from './Events'
import { ApiError } from '../api/client'

const ev = (over: Record<string, unknown> = {}) => ({
  id: 1,
  title: 'AI in Project Controls',
  summary: 'A live webinar.',
  event_type: 'webinar',
  starts_at: '2027-01-01T00:00:00Z',
  registered: false,
  attended: false,
  seats_left: null,
  cpd_hours: 0,
  join_url: null,
  ...over,
})

describe('Events (member events & webinars)', () => {
  beforeEach(() => {
    h.rows = []
    h.post.mockReset()
    h.refetch.mockReset()
  })

  it('shows the empty state when there are no upcoming events', () => {
    render(<Events />)
    expect(screen.getByText('No upcoming events right now. Check back soon.')).toBeInTheDocument()
  })

  it('registers for an open event and refetches', async () => {
    const user = userEvent.setup()
    h.rows = [ev({ seats_left: 5 })]
    render(<Events />)
    expect(screen.getByText(/5 seat\(s\) left/)).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Register' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/events/1/register', {}))
    expect(h.refetch).toHaveBeenCalled()
  })

  it('disables registration for a fully-booked event', () => {
    h.rows = [ev({ seats_left: 0 })]
    render(<Events />)
    const btn = screen.getByRole('button', { name: 'Fully booked' })
    expect(btn).toBeDisabled()
    expect(screen.queryByRole('button', { name: 'Register' })).toBeNull()
  })

  it('exposes Join + Cancel for a registered event and cancels on click', async () => {
    const user = userEvent.setup()
    h.rows = [ev({ registered: true, join_url: 'https://join.test/1' })]
    render(<Events />)
    expect(screen.getByText('Registered')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: /Join/ })).toHaveAttribute('href', 'https://join.test/1')
    await user.click(screen.getByRole('button', { name: 'Cancel registration' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/events/1/cancel', {}))
  })

  it('drops the Cancel action once an event has been attended', () => {
    h.rows = [ev({ registered: true, attended: true })]
    render(<Events />)
    expect(screen.getByText('Attended')).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Cancel registration' })).toBeNull()
  })

  it('shows the CPD-hours badge only when the event carries hours', () => {
    h.rows = [ev({ cpd_hours: 2 })]
    render(<Events />)
    expect(screen.getByText('2h CPD')).toBeInTheDocument()
  })

  it('surfaces a structured ApiError message when registration fails', async () => {
    const user = userEvent.setup()
    h.rows = [ev({ seats_left: 3 })]
    h.post.mockRejectedValue(new ApiError(409, 'conflict', { message: 'Registration has closed for this event.' }))
    render(<Events />)
    await user.click(screen.getByRole('button', { name: 'Register' }))
    expect(await screen.findByRole('alert')).toHaveTextContent('Registration has closed for this event.')
  })
})
