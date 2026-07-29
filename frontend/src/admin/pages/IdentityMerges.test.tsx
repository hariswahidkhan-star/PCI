import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE — the maker-checker approval workflow, which had a complete backend and no console screen.
// What this pins: approving is confirm-guarded because a merge is irreversible; the backend's
// two-person refusal (409 maker_checker) is translated into the RULE rather than the error code;
// the per-table record counts for BOTH accounts are shown before a decision, since "which account
// survives" is not answerable from ids alone; and a decided request offers no decision controls.
const h = vi.hoisted(() => ({
  rows: [] as unknown[],
  detail: null as unknown,
  loading: false,
  error: null as string | null,
  post: vi.fn(),
}))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string) => {
    const isDetail = /\/identity\/merges\/\d+$/.test(path)
    return {
      data: isDetail ? h.detail : { ok: true, rows: h.rows },
      loading: h.loading,
      error: h.error,
      refetch: vi.fn(),
    }
  },
}))
vi.mock('../api', () => ({ adminApi: { post: (...a: unknown[]) => h.post(...a) } }))

import IdentityMerges from './IdentityMerges'

const row = (over: Record<string, unknown> = {}) => ({
  id: 7, source_user_id: 101, target_user_id: 202, reason: 'Duplicate registration',
  status: 'pending', requested_by: 5, requested_at: '2026-03-01 10:00:00', ...over,
})

const detail = (over: Record<string, unknown> = {}) => ({
  ok: true,
  merge: {
    request: row(over),
    preview: { source: { payments: 2, exam_attempts: 1 }, target: { payments: 9, issued_credentials: 1 } },
  },
})

describe('Identity merges (approval workflow)', () => {
  beforeEach(() => {
    h.rows = []
    h.detail = detail()
    h.loading = false
    h.error = null
    h.post.mockReset().mockResolvedValue({ ok: true })
    vi.spyOn(window, 'confirm').mockReturnValue(true)
  })

  it('explains the queue when nothing has been raised', () => {
    render(<IdentityMerges />)
    expect(screen.getByText('No merge requests')).toBeInTheDocument()
  })

  it('flags how many requests await a decision', () => {
    h.rows = [row({ id: 1 }), row({ id: 2 }), row({ id: 3, status: 'approved' })]
    render(<IdentityMerges />)
    expect(screen.getByText('2 awaiting a decision')).toBeInTheDocument()
  })

  it('omits the chip when nothing is pending', () => {
    h.rows = [row({ status: 'approved' })]
    render(<IdentityMerges />)
    expect(screen.queryByText(/awaiting a decision/)).toBeNull()
  })

  it('shows what each account holds before a decision is made', async () => {
    const user = userEvent.setup()
    h.rows = [row()]
    const { container } = render(<IdentityMerges />)
    await user.click(container.querySelectorAll('tbody tr')[0] as HTMLElement)

    const drawer = await screen.findByRole('dialog')
    // both sides' counts, so the survivor choice is evidence-based rather than guessed from ids
    expect(within(drawer).getByText('payments')).toBeInTheDocument()
    expect(within(drawer).getByText('9')).toBeInTheDocument()
    expect(within(drawer).getByText('issued credentials')).toBeInTheDocument()
  })

  it('confirms before executing an irreversible merge', async () => {
    const user = userEvent.setup()
    vi.spyOn(window, 'confirm').mockReturnValue(false)
    h.rows = [row()]
    const { container } = render(<IdentityMerges />)
    await user.click(container.querySelectorAll('tbody tr')[0] as HTMLElement)
    await user.click(await screen.findByRole('button', { name: 'Approve and merge' }))
    expect(h.post).not.toHaveBeenCalled()
  })

  it('posts the approval with the decision note', async () => {
    const user = userEvent.setup()
    h.rows = [row()]
    const { container } = render(<IdentityMerges />)
    await user.click(container.querySelectorAll('tbody tr')[0] as HTMLElement)
    await user.type(await screen.findByLabelText(/Note/), 'Verified by phone')
    await user.click(screen.getByRole('button', { name: 'Approve and merge' }))
    await waitFor(() => expect(h.post).toHaveBeenCalled())
    expect(h.post.mock.calls[0][0]).toBe('/api/admin/identity/merges/7/approve')
    expect(h.post.mock.calls[0][1]).toEqual({ note: 'Verified by phone' })
  })

  it('rejecting needs no confirmation — refusing to merge is the safe direction', async () => {
    const user = userEvent.setup()
    const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true)
    h.rows = [row()]
    const { container } = render(<IdentityMerges />)
    await user.click(container.querySelectorAll('tbody tr')[0] as HTMLElement)
    confirmSpy.mockClear()
    await user.click(await screen.findByRole('button', { name: 'Reject' }))
    expect(confirmSpy).not.toHaveBeenCalled()
    await waitFor(() => expect(h.post.mock.calls[0][0]).toBe('/api/admin/identity/merges/7/reject'))
  })

  it('translates the two-person refusal into the rule, not the error code', async () => {
    const user = userEvent.setup()
    h.post.mockRejectedValue(new Error('maker_checker'))
    h.rows = [row()]
    const { container } = render(<IdentityMerges />)
    await user.click(container.querySelectorAll('tbody tr')[0] as HTMLElement)
    await user.click(await screen.findByRole('button', { name: 'Approve and merge' }))
    expect(await screen.findByText(/you cannot approve it. Identity merges need a second admin/)).toBeInTheDocument()
    expect(screen.queryByText('maker_checker')).toBeNull()
  })

  it('offers no decision controls on an already-decided request', async () => {
    const user = userEvent.setup()
    h.rows = [row({ status: 'approved' })]
    h.detail = detail({ status: 'approved', decided_by: 9, decided_at: '2026-03-02 09:00:00' })
    const { container } = render(<IdentityMerges />)
    await user.click(container.querySelectorAll('tbody tr')[0] as HTMLElement)
    await screen.findByRole('dialog')
    expect(screen.queryByRole('button', { name: 'Approve and merge' })).toBeNull()
    expect(screen.queryByRole('button', { name: 'Reject' })).toBeNull()
  })

  it('filters by status', async () => {
    const user = userEvent.setup()
    h.rows = [row({ id: 1, status: 'pending' }), row({ id: 2, status: 'rejected' })]
    const { container } = render(<IdentityMerges />)
    expect(container.querySelectorAll('tbody tr')).toHaveLength(2)
    await user.selectOptions(screen.getByLabelText('Status'), 'rejected')
    expect(container.querySelectorAll('tbody tr')).toHaveLength(1)
  })
})
