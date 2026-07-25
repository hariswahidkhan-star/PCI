import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE-12 — the Exam Exceptions & Authorizations console is where staff extend deadlines, grant
// attempts, waive fees and reopen closed windows: exam-integrity decisions. This pins the
// Authorizations tab's list/filter surface and the RBAC-gated, reason-required "Reopen scheduling"
// decision. useAdminQuery + useAdminAuth (can) + adminApi are mocked at the module boundary.
const h = vi.hoisted(() => ({
  exceptions: { rows: [] as unknown[] },
  perms: [] as string[],
  paths: [] as (string | null)[],
  post: vi.fn(),
}))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    h.paths.push(path)
    let data: unknown = null
    if (path === '/api/certifications') data = { rows: [{ id: 1, code: 'PCL-AI', name: 'Project Controls Lead — AI' }] }
    else if (path && path.startsWith('/api/admin/exam-exceptions')) data = h.exceptions
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))
vi.mock('../AdminAuth', () => ({
  useAdminAuth: () => ({ can: (p: string) => h.perms.includes(p) }),
}))
vi.mock('../api', () => ({
  adminApi: { post: (path: string, body?: unknown) => h.post(path, body) },
}))

import ExamExceptions from './ExamExceptions'

// The status filter is one of several comboboxes; find it by its "All statuses" option.
function statusSelect() {
  return screen.getAllByRole('combobox').find((s) => within(s).queryByRole('option', { name: 'All statuses' }))!
}

describe('ExamExceptions admin console', () => {
  beforeEach(() => {
    h.exceptions = { rows: [] }
    h.perms = []
    h.paths = []
    h.post.mockReset()
  })

  describe('Reopen scheduling (RBAC + validation)', () => {
    it('disables the reopen action and explains when the reopen permission is missing', () => {
      render(<ExamExceptions />)
      expect(screen.getByRole('button', { name: 'Reopen scheduling' })).toBeDisabled()
      expect(screen.getByText('You do not have the reopen permission.')).toBeInTheDocument()
    })

    it('enables the action for an admin holding ex_reopen', () => {
      h.perms = ['ex_reopen']
      render(<ExamExceptions />)
      expect(screen.getByRole('button', { name: 'Reopen scheduling' })).toBeEnabled()
    })

    it('requires a user ID and a reason before posting', async () => {
      const user = userEvent.setup()
      h.perms = ['ex_reopen']
      render(<ExamExceptions />)
      const btn = screen.getByRole('button', { name: 'Reopen scheduling' })
      await user.click(btn)
      expect(await screen.findByText('A user ID is required.')).toBeInTheDocument()
      expect(h.post).not.toHaveBeenCalled()

      await user.type(screen.getByLabelText('User ID'), '42')
      await user.click(btn)
      expect(await screen.findByText('A reason is required.')).toBeInTheDocument()
      expect(h.post).not.toHaveBeenCalled()
    })

    it('posts the reopen with its user, reason and waive-fee flag', async () => {
      const user = userEvent.setup()
      h.perms = ['ex_reopen']
      render(<ExamExceptions />)
      // 'Reason (required)' also appears on the Bulk-reopen card, so scope to the Reopen card.
      const reopenCard = screen.getByRole('heading', { name: 'Reopen scheduling' }).closest('section') as HTMLElement
      await user.type(screen.getByLabelText('User ID'), '42')
      await user.type(within(reopenCard).getByLabelText('Reason (required)'), 'incident on exam day')
      await user.click(within(reopenCard).getByRole('checkbox', { name: /Waive fee/ }))
      h.post.mockResolvedValueOnce(undefined)
      await user.click(screen.getByRole('button', { name: 'Reopen scheduling' }))
      await waitFor(() =>
        expect(h.post).toHaveBeenCalledWith(
          '/api/admin/exam-authorizations/reopen',
          expect.objectContaining({ user_id: 42, reason: 'incident on exam day', waive_fee: true }),
        ),
      )
      expect(await screen.findByText(/Scheduling reopened for user #42/)).toBeInTheDocument()
    })
  })

  describe('authorizations list', () => {
    it('folds a status filter into the exam-exceptions query path', async () => {
      const user = userEvent.setup()
      render(<ExamExceptions />)
      const examPaths = () => h.paths.filter((p) => p && p.startsWith('/api/admin/exam-exceptions'))
      expect(examPaths()[0]).toBe('/api/admin/exam-exceptions')
      await user.selectOptions(statusSelect(), 'active')
      await waitFor(() => expect(examPaths()[examPaths().length - 1]).toContain('status=active'))
    })

    it('renders a row with its exception-count badges', () => {
      h.exceptions = {
        rows: [
          { id: 5, first_name: 'Sam', last_name: 'Rivera', email: 'sam@ex.co', certification_id: 1, certification_acronym: 'PCL-AI', current_deadline: '2027-01-01', attempts_used_total: 1, attempts_permitted_total: 3, extensions: 2, grants: 1, incidents: 1, status: 'active' },
        ],
      }
      render(<ExamExceptions />)
      const row = screen.getByText('Sam Rivera').closest('tr') as HTMLElement
      expect(within(row).getByText('2 ext')).toBeInTheDocument()
      expect(within(row).getByText('1 grant')).toBeInTheDocument()
      expect(within(row).getByText('1 inc')).toBeInTheDocument()
    })

    it('shows the empty state when nothing matches', () => {
      render(<ExamExceptions />)
      expect(screen.getByText('No authorizations match.')).toBeInTheDocument()
    })
  })
})
