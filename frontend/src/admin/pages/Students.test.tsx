import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE-12 — the Students console is member management (status, membership, impersonation, test users).
// This pins the roster surface: the total count, the test-account marker, the status-filter query
// path, and the empty state. useAdminQuery + adminApi mocked; the real ApiError is kept for the
// drawer's impersonation error mapping (unused at the list level but imported by the module).
const h = vi.hoisted(() => ({ members: { rows: [] as unknown[], total: 0 }, paths: [] as (string | null)[] }))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    h.paths.push(path)
    const data = path && path.startsWith('/api/admin/members') ? h.members : null
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))
vi.mock('../api', () => ({ adminApi: { post: vi.fn(), getToken: () => 't' } }))

import Students from './Students'

const member = (over: Record<string, unknown> = {}) => ({
  id: 1, first_name: 'Sam', last_name: 'Rivera', email: 'sam@ex.co', status: 'active',
  membership_status: 'active', paid_total: 120, credentials: 1, is_test: 0, created_at: '2025-01-01T00:00:00Z', ...over,
})

describe('Students admin console', () => {
  beforeEach(() => { h.members = { rows: [], total: 0 }; h.paths = [] })

  it('shows the total count and one row per member', () => {
    h.members = { rows: [member(), member({ id: 2, first_name: 'Ada', email: 'ada@ex.co' })], total: 2 }
    render(<Students />)
    expect(screen.getByText('2 total')).toBeInTheDocument()
    expect(screen.getByText('Sam Rivera')).toBeInTheDocument()
    expect(screen.getByText('ada@ex.co')).toBeInTheDocument()
  })

  it('marks a test account with its badge', () => {
    h.members = { rows: [member({ is_test: 1 })], total: 1 }
    render(<Students />)
    const row = screen.getByText('Sam Rivera').closest('tr') as HTMLElement
    expect(within(row).getByText('TEST ACCOUNT')).toBeInTheDocument()
  })

  it('folds a status filter into the members query path', async () => {
    const user = userEvent.setup()
    render(<Students />)
    expect(h.paths.some((p) => p === '/api/admin/members')).toBe(true)
    const statusSel = screen.getAllByRole('combobox').find((s) => within(s).queryByRole('option', { name: 'Deactivated' }))!
    await user.selectOptions(statusSel, 'deactivated')
    await waitFor(() => expect(h.paths[h.paths.length - 1]).toContain('status=deactivated'))
  })

  it('shows the empty state when no students match', () => {
    render(<Students />)
    expect(screen.getByText('No students match.')).toBeInTheDocument()
  })
})
