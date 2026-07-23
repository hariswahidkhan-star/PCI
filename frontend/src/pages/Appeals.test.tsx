import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE — Appeals & Accommodations. The decisions worth pinning are the client-side casework guards:
//  • a 20-character minimum on the grounds/description BEFORE any POST (both forms);
//  • optional attempt/credential fields fold into the body only when chosen, and attempt_id is sent
//    as a Number (not the option's string value);
//  • the "related exam attempt" picker only appears for the attempt-based appeal types;
//  • a structured ApiError surfaces its body.message to the member.
// useMe, the two data hooks and the fetch client are mocked at the module boundary; the REAL ApiError
// is kept (the module maps it via `instanceof`).
const h = vi.hoisted(() => ({
  me: { attempts: [] as unknown[], credentials: [] as unknown[] } as Record<string, unknown>,
  appeals: { rows: [] } as unknown,
  accoms: { rows: [] } as unknown,
  post: vi.fn(),
  refetch: vi.fn(),
}))

vi.mock('../data/MeContext', () => ({ useMe: () => ({ me: h.me }) }))
vi.mock('../api/hooks', () => ({
  useQuery: (path: string) => {
    const data = path === '/api/me/appeals' ? h.appeals : path === '/api/me/accommodations' ? h.accoms : null
    return { data, loading: false, error: null, refetch: h.refetch }
  },
}))
vi.mock('../api/client', async (orig) => {
  const actual = await orig<typeof import('../api/client')>()
  return { ...actual, api: { post: (p: string, b?: unknown) => h.post(p, b) } }
})

import Appeals from './Appeals'
import { ApiError } from '../api/client'

const appealForm = () => screen.getByRole('button', { name: 'Submit appeal' }).closest('form') as HTMLElement
const accomForm = () => screen.getByRole('button', { name: 'Submit request' }).closest('form') as HTMLElement

describe('Appeals & Accommodations', () => {
  beforeEach(() => {
    h.me = { attempts: [], credentials: [] }
    h.appeals = { rows: [] }
    h.accoms = { rows: [] }
    h.post.mockReset()
    h.refetch.mockReset()
  })

  it('blocks an appeal whose grounds are under 20 characters', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Appeals />)
    await user.type(within(appealForm()).getByLabelText(/Grounds for your appeal/), 'too short')
    await user.click(screen.getByRole('button', { name: 'Submit appeal' }))
    expect(
      within(appealForm()).getByText('Please describe the grounds for your appeal (at least 20 characters).'),
    ).toBeInTheDocument()
    expect(h.post).not.toHaveBeenCalled()
  })

  it('submits a minimal appeal body with only the type and trimmed reason', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Appeals />)
    await user.type(
      within(appealForm()).getByLabelText(/Grounds for your appeal/),
      'The score does not reflect my performance.',
    )
    await user.click(screen.getByRole('button', { name: 'Submit appeal' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledTimes(1))
    const [path, body] = h.post.mock.calls[0]
    expect(path).toBe('/api/me/appeals')
    expect(body).toEqual({ type: 'result_appeal', reason: 'The score does not reflect my performance.' })
    expect(body).not.toHaveProperty('attempt_id') // not selected → omitted
    expect(within(appealForm()).getByText(/Your appeal has been submitted/)).toBeInTheDocument()
  })

  it('shows the exam-attempt picker only for attempt-based appeal types', async () => {
    const user = userEvent.setup()
    h.me = { attempts: [{ id: 42, kind: 'PCL-AI', result: 'fail' }], credentials: [] }
    renderWithProviders(<Appeals />)
    // Default type is result_appeal → the attempt picker is offered.
    expect(within(appealForm()).getByText(/Related exam attempt/)).toBeInTheDocument()
    // Switching to a complaint (not attempt-based) hides it.
    await user.selectOptions(within(appealForm()).getAllByRole('combobox')[0], 'complaint')
    expect(within(appealForm()).queryByText(/Related exam attempt/)).toBeNull()
  })

  it('folds a chosen attempt into the body as a numeric attempt_id', async () => {
    const user = userEvent.setup()
    h.me = { attempts: [{ id: 42, kind: 'PCL-AI', result: 'fail' }], credentials: [] }
    renderWithProviders(<Appeals />)
    const attemptSel = within(appealForm())
      .getAllByRole('combobox')
      .find((s) => within(s).queryByRole('option', { name: /select an attempt/ }))!
    await user.selectOptions(attemptSel, '42')
    await user.type(
      within(appealForm()).getByLabelText(/Grounds for your appeal/),
      'The invalidation was applied in error.',
    )
    await user.click(screen.getByRole('button', { name: 'Submit appeal' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledTimes(1))
    const body = h.post.mock.calls[0][1] as { attempt_id: unknown }
    expect(body.attempt_id).toBe(42)
    expect(typeof body.attempt_id).toBe('number') // Number(attemptId), not the option string
  })

  it('blocks an accommodation request whose description is under 20 characters', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Appeals />)
    await user.type(within(accomForm()).getByLabelText(/Details/), 'need help')
    await user.click(screen.getByRole('button', { name: 'Submit request' }))
    expect(
      within(accomForm()).getByText('Please describe the accommodation you need (at least 20 characters).'),
    ).toBeInTheDocument()
    expect(h.post).not.toHaveBeenCalled()
  })

  it('submits a valid accommodation request to the accommodations endpoint', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Appeals />)
    await user.type(
      within(accomForm()).getByLabelText(/Details/),
      'I need additional time due to a documented condition.',
    )
    await user.click(screen.getByRole('button', { name: 'Submit request' }))
    await waitFor(() =>
      expect(h.post).toHaveBeenCalledWith('/api/me/accommodations', {
        request_type: 'extra_time',
        description: 'I need additional time due to a documented condition.',
      }),
    )
    expect(within(accomForm()).getByText(/Your accommodation request has been submitted/)).toBeInTheDocument()
  })

  it('surfaces a structured ApiError message from the server', async () => {
    const user = userEvent.setup()
    h.post.mockRejectedValue(new ApiError(409, 'conflict', { message: 'This attempt is not eligible for appeal.' }))
    renderWithProviders(<Appeals />)
    await user.type(
      within(appealForm()).getByLabelText(/Grounds for your appeal/),
      'Please reconsider this decision carefully.',
    )
    await user.click(screen.getByRole('button', { name: 'Submit appeal' }))
    expect(await within(appealForm()).findByText('This attempt is not eligible for appeal.')).toBeInTheDocument()
  })

  it('lists submitted appeals and accommodations with their status', () => {
    h.appeals = {
      rows: [{ id: 1, type: 'result_appeal', reason: 'Grounds here', status: 'under_review', submitted_at: '2026-02-01T00:00:00Z' }],
    }
    h.accoms = {
      rows: [{ id: 2, request_type: 'extra_time', description: 'Extra time', status: 'approved', approved_extra_minutes: 30, created_at: '2026-02-02T00:00:00Z' }],
    }
    renderWithProviders(<Appeals />)
    // Assert on row-unique content — the type labels ("Appeal an exam result" / "Additional time")
    // also appear as <option>s in the two Type selects, so scope to the reason/description/decision.
    expect(screen.getByText('Grounds here')).toBeInTheDocument()
    expect(screen.getByText('Extra time')).toBeInTheDocument()
    expect(screen.getByText(/\+30 min approved/)).toBeInTheDocument()
  })
})
