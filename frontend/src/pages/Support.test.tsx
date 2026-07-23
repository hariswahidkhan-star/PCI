import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE — the support centre. The decisions worth pinning are the ticket lifecycle guards, not the
// styling: a new ticket POSTs {subject, category, body} and echoes the returned reference; an empty
// reply is a no-op (the `!reply.trim()` guard) while a real one posts to the reply endpoint; and the
// satisfaction-rating widget appears ONLY for a resolved/closed ticket that has not yet been rated,
// then posts the chosen star. The ticket data hook and the fetch client are mocked at the module
// boundary so the page mounts with no network.
const h = vi.hoisted(() => ({
  data: { rows: [] as unknown[] } as unknown,
  refetch: vi.fn(),
  post: vi.fn(),
}))

vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: h.data, loading: false, error: null, refetch: h.refetch }),
}))
vi.mock('../api/client', () => ({
  api: { post: (path: string, body?: unknown) => h.post(path, body) },
}))

import Support from './Support'

const ticket = (over: Record<string, unknown> = {}) => ({
  id: 7,
  reference: 'PCI-7',
  subject: 'Exam reschedule',
  category: 'exams',
  status: 'open',
  updated_at: '2026-02-01T00:00:00Z',
  resolved_at: null,
  rating: null,
  messages: [{ sender: 'user', body: 'Please help', created_at: '2026-02-01T00:00:00Z' }],
  ...over,
})

describe('Support (ticket lifecycle)', () => {
  beforeEach(() => {
    h.data = { rows: [] }
    h.refetch.mockReset()
    h.post.mockReset()
  })

  it('creates a ticket with the chosen category and echoes the reference', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValue({ reference: 'PCI-1042' })
    renderWithProviders(<Support />)
    await user.type(screen.getByLabelText('Subject'), 'Exam reschedule')
    await user.selectOptions(screen.getByLabelText('Category'), 'billing')
    await user.type(screen.getByLabelText('How can we help?'), 'I paid twice by mistake.')
    await user.click(screen.getByRole('button', { name: 'Submit request' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledTimes(1))
    expect(h.post).toHaveBeenCalledWith('/api/me/tickets', {
      subject: 'Exam reschedule',
      category: 'billing',
      body: 'I paid twice by mistake.',
    })
    expect(await screen.findByText("Ticket PCI-1042 created — we'll reply here and by email.")).toBeInTheDocument()
  })

  it('surfaces the server error when ticket creation fails', async () => {
    const user = userEvent.setup()
    h.post.mockRejectedValue(new Error('queue is down'))
    renderWithProviders(<Support />)
    await user.type(screen.getByLabelText('Subject'), 'Help')
    await user.type(screen.getByLabelText('How can we help?'), 'Something broke.')
    await user.click(screen.getByRole('button', { name: 'Submit request' }))
    expect(await screen.findByRole('alert')).toHaveTextContent('queue is down')
  })

  it('ignores an empty reply (the trim guard) but posts a real one', async () => {
    const user = userEvent.setup()
    h.data = { rows: [ticket()] }
    renderWithProviders(<Support />)
    await user.click(screen.getByRole('button', { name: /Exam reschedule/ }))
    // Empty reply → the guard returns before any request.
    await user.click(screen.getByRole('button', { name: 'Reply' }))
    expect(h.post).not.toHaveBeenCalled()
    // A real reply posts to the ticket's reply endpoint.
    await user.type(screen.getByPlaceholderText('Write a reply…'), 'Any update?')
    await user.click(screen.getByRole('button', { name: 'Reply' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/tickets/7/reply', { body: 'Any update?' }))
  })

  it('hides the rating widget for an open ticket', async () => {
    const user = userEvent.setup()
    h.data = { rows: [ticket({ status: 'open' })] }
    renderWithProviders(<Support />)
    await user.click(screen.getByRole('button', { name: /Exam reschedule/ }))
    expect(screen.queryByText('How did we do? Rate this conversation:')).toBeNull()
  })

  it('offers the rating widget on a resolved unrated ticket and posts the chosen star', async () => {
    const user = userEvent.setup()
    h.data = { rows: [ticket({ status: 'resolved', rating: null })] }
    renderWithProviders(<Support />)
    await user.click(screen.getByRole('button', { name: /Exam reschedule/ }))
    expect(screen.getByText('How did we do? Rate this conversation:')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'Rate 4 out of 5' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/tickets/7/rate', { rating: 4 }))
    expect(await screen.findByText('Thank you — your rating has been recorded.')).toBeInTheDocument()
  })

  it('shows the recorded rating (no prompt) once a ticket has been rated', async () => {
    const user = userEvent.setup()
    h.data = { rows: [ticket({ status: 'closed', rating: 5 })] }
    renderWithProviders(<Support />)
    await user.click(screen.getByRole('button', { name: /Exam reschedule/ }))
    expect(screen.queryByText('How did we do? Rate this conversation:')).toBeNull()
    expect(screen.getByText('Thank you — your rating has been recorded.')).toBeInTheDocument()
  })
})
