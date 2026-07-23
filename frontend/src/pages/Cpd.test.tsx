import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE — the CPD screen carries several member-facing decisions worth pinning:
//  • the progress ring maths — pct = min(100, round(total/target*100)), and the "awaiting review"
//    note that only appears when there are pending (un-approved) hours;
//  • the add-activity POST — the free-text hours field is parsed to a Number and the category
//    defaults to the first framework category, with a mandatory-AI-currency hint when that
//    category is chosen;
//  • the annual CPD declaration state machine — a recorded declaration shows a read-only summary
//    with an "Update declaration" reopen, and submitting posts the chosen position.
// useMe, the CPD data hook and the fetch client are mocked at the module boundary so the whole page
// mounts hermetically with no network.
const h = vi.hoisted(() => ({
  me: { cpd: { total: 30, target: 60, pending: 0 } } as Record<string, unknown>,
  data: null as unknown,
  refetch: vi.fn(),
  refetchMe: vi.fn(),
  post: vi.fn(),
  del: vi.fn(),
}))

vi.mock('../data/MeContext', () => ({
  useMe: () => ({ me: h.me, loading: false, error: null, refetch: h.refetchMe }),
}))
vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: h.data, loading: false, error: null, refetch: h.refetch }),
  runMutation: (fn: () => Promise<void>) => fn(),
}))
vi.mock('../api/client', () => ({
  api: {
    post: (path: string, body?: unknown) => h.post(path, body),
    del: (path: string) => h.del(path),
  },
}))

import Cpd from './Cpd'

describe('Cpd (continuing professional development)', () => {
  beforeEach(() => {
    h.me = { cpd: { total: 30, target: 60, pending: 0 } }
    h.data = null
    h.refetch.mockReset()
    h.refetchMe.mockReset()
    h.post.mockReset()
    h.del.mockReset()
  })

  it('renders progress as a clamped percentage of the target', () => {
    renderWithProviders(<Cpd />)
    expect(screen.getByText('30 of 60 hours')).toBeInTheDocument()
    expect(screen.getByText('50%')).toBeInTheDocument()
    expect(screen.getByText(/Only admin-approved hours count toward your total/)).toBeInTheDocument()
  })

  it('clamps progress to 100% once the target is exceeded', () => {
    h.me = { cpd: { total: 120, target: 60, pending: 0 } }
    renderWithProviders(<Cpd />)
    expect(screen.getByText('100%')).toBeInTheDocument()
  })

  it('surfaces an awaiting-review note only when hours are pending approval', () => {
    h.me = { cpd: { total: 30, target: 60, pending: 4 } }
    renderWithProviders(<Cpd />)
    expect(screen.getByText(/You have 4 hour\(s\) awaiting review/)).toBeInTheDocument()
  })

  it('parses the hours field to a number and defaults the category on add', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Cpd />)
    await user.type(screen.getByLabelText('Activity'), 'EVM masterclass')
    await user.type(screen.getByLabelText('Hours'), '2.5')
    await user.click(screen.getByRole('button', { name: 'Add entry' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledTimes(1))
    const [path, body] = h.post.mock.calls[0]
    expect(path).toBe('/api/me/cpd')
    expect(body).toMatchObject({ description: 'EVM masterclass', category: 'Structured learning', hours: 2.5 })
    expect(typeof (body as { hours: unknown }).hours).toBe('number') // parseFloat, not the raw string
  })

  it('shows the mandatory-AI-currency hint only when that category is selected', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Cpd />)
    expect(screen.queryByText('Mandatory AI-currency component of the framework.')).toBeNull()
    await user.selectOptions(screen.getByLabelText('Category'), 'AI currency')
    expect(screen.getByText('Mandatory AI-currency component of the framework.')).toBeInTheDocument()
  })

  it('shows a read-only summary once the year is declared, then reopens the form on request', async () => {
    const user = userEvent.setup()
    h.data = {
      rows: [],
      declaration_year: 2026,
      declared_this_year: true,
      latest_declaration: { cycle_year: 2026, position: 'compliant', created_at: '2026-03-01T00:00:00Z' },
    }
    renderWithProviders(<Cpd />)
    // Declared → the summary carries the friendly position label; the position field is hidden.
    expect(screen.getByText(/Compliant — I have met my CPD/)).toBeInTheDocument()
    expect(screen.queryByLabelText('Your CPD position')).toBeNull()
    await user.click(screen.getByRole('button', { name: 'Update declaration' }))
    expect(screen.getByLabelText('Your CPD position')).toBeInTheDocument()
  })

  it('posts the chosen position when the annual declaration is submitted', async () => {
    const user = userEvent.setup()
    h.data = { rows: [], declaration_year: 2026, declared_this_year: false }
    renderWithProviders(<Cpd />)
    await user.selectOptions(screen.getByLabelText('Your CPD position'), 'career_break')
    await user.click(screen.getByRole('button', { name: 'Submit declaration' }))
    await waitFor(() =>
      expect(h.post).toHaveBeenCalledWith(
        '/api/me/cpd/declaration',
        expect.objectContaining({ position: 'career_break' }),
      ),
    )
    expect(screen.getByText('Your 2026 CPD declaration has been recorded.')).toBeInTheDocument()
  })
})
