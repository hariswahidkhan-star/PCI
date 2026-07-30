import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import userEvent from '@testing-library/user-event'
import { ApiError } from '../../api/client'

// The identity console operates on canonical identity data (spec §10.1). These tests pin the three
// behaviours that must never regress:
//   • the backfill RUN is armed only by literally typing "run backfill" (it writes identity data),
//     while the preview clearly says it writes nothing;
//   • a 409 maker_checker on a merge decision is explained as "a different admin must decide this",
//     not surfaced as a generic failure, and 409 already_decided refreshes the queue;
//   • the health dashboard renders its counts, its index-eligibility state, and an honest fetch
//     failure, and every section hides (presentation only) without its id_* permission.
const h = vi.hoisted(() => ({
  perms: [] as string[],
  health: null as unknown,
  healthLoading: false,
  healthError: null as string | null,
  merges: { ok: true, rows: [] as unknown[] },
  mergeDetail: null as unknown,
  refetchMerges: vi.fn(),
  refetchDetail: vi.fn(),
  get: vi.fn(),
  post: vi.fn(),
  queried: [] as (string | null)[],
}))

vi.mock('../AdminAuth', () => ({
  useAdminAuth: () => ({
    me: { id: 1, email: 'a@pci.local', name: 'A', role: 'custom', is_owner: false, must_change_pw: false, permissions: h.perms },
    can: (s: string) => h.perms.includes(s),
    logout: vi.fn(),
  }),
}))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    h.queried.push(path)
    if (path === '/api/admin/identity/student-numbers/health')
      return { data: h.health, loading: h.healthLoading, error: h.healthError, refetch: vi.fn() }
    if (path === '/api/admin/identity/merges')
      return { data: h.merges, loading: false, error: null, refetch: h.refetchMerges }
    if (path != null && path.startsWith('/api/admin/identity/merges/'))
      return { data: h.mergeDetail, loading: false, error: null, refetch: h.refetchDetail }
    return { data: null, loading: false, error: null, refetch: vi.fn() }
  },
}))

vi.mock('../api', async (importOriginal) => {
  const orig = await importOriginal<typeof import('../api')>()
  return {
    ...orig,
    adminApi: {
      get: (p: string) => h.get(p),
      post: (p: string, b?: unknown) => h.post(p, b),
    },
  }
})

import Identity from './Identity'

const ALL_PERMS = ['id_read', 'id_backfill', 'id_merge_request', 'id_merge_approve', 'id_audit']

const healthOk = (over: Record<string, unknown> = {}) => ({
  ok: true,
  health: {
    eligible_students: 1200, missing_number: 37, blank_number: 2, duplicate_numbers: 0,
    malformed_numbers: 3, projection_without_reservation: 5, reservation_without_projection: 1,
    retired_reservations: 8, quarantined_reservations: 4, students_without_profile: 6,
    projection_index_eligible: true, ...over,
  },
})

const pendingMerge = (over: Record<string, unknown> = {}) => ({
  id: 11, source_user_id: 101, target_user_id: 202, reason: 'duplicate signup', status: 'pending',
  requested_by: 7, requested_at: '2026-07-01 10:00:00', decided_by: null, decided_at: null,
  decision_note: null, correlation_id: 'abc123', ...over,
})

const mergeDetailFor = (request: Record<string, unknown>) => ({
  ok: true,
  merge: {
    request,
    preview: {
      source: { payments: 2, credentials: 0, sessions: 1 },
      target: { payments: 5, credentials: 1, sessions: 2 },
    },
  },
})

const renderPage = () => render(<MemoryRouter><Identity /></MemoryRouter>)

beforeEach(() => {
  h.perms = [...ALL_PERMS]
  h.health = healthOk()
  h.healthLoading = false
  h.healthError = null
  h.merges = { ok: true, rows: [] }
  h.mergeDetail = null
  h.refetchMerges.mockReset()
  h.refetchDetail.mockReset()
  h.get.mockReset()
  h.post.mockReset()
  h.queried = []
})

describe('health dashboard', () => {
  it('renders the estate counts and the index-eligible state', () => {
    renderPage()
    expect(screen.getByText('Student Number health')).toBeInTheDocument()
    expect(screen.getByText('37')).toBeInTheDocument() // missing_number
    expect(screen.getByText('Missing number')).toBeInTheDocument()
    expect(screen.getByText('Duplicate numbers')).toBeInTheDocument()
    expect(screen.getByText('Quarantined')).toBeInTheDocument()
    expect(screen.getByText(/eligible — the next boot will enforce uniqueness/)).toBeInTheDocument()
  })

  it('shows the blocked badge when duplicates prevent the uniqueness index', () => {
    h.health = healthOk({ projection_index_eligible: false, duplicate_numbers: 9 })
    renderPage()
    expect(screen.getByText(/blocked — duplicates must be resolved first/)).toBeInTheDocument()
  })

  it('renders a -1 probe count as unknown, never as zero', () => {
    h.health = healthOk({ malformed_numbers: -1 })
    renderPage()
    expect(screen.getByText('unknown')).toBeInTheDocument()
  })

  it('surfaces a fetch failure as an error, not an empty dashboard', () => {
    h.health = null
    h.healthError = 'Network error — please check your connection and try again.'
    renderPage()
    expect(screen.getByRole('alert')).toHaveTextContent('Network error')
  })

  it('shows the loading state while the report is being fetched', () => {
    h.health = null
    h.healthLoading = true
    renderPage()
    expect(screen.getByRole('status', { name: 'Loading' })).toBeInTheDocument()
  })

  it('is hidden (and never queried) without id_read', () => {
    h.perms = ['id_backfill']
    renderPage()
    expect(screen.queryByText('Student Number health')).not.toBeInTheDocument()
    expect(h.queried).not.toContain('/api/admin/identity/student-numbers/health')
  })
})

describe('the no-manual-issuance rule', () => {
  it('explains that Student Numbers can never be typed in by hand', () => {
    renderPage()
    expect(screen.getByText(/Student Numbers cannot be set by hand/)).toBeInTheDocument()
    expect(screen.getByText(/deliberately no\s+endpoint that assigns a Student Number/)).toBeInTheDocument()
  })
})

describe('backfill preview (dry run)', () => {
  it('posts the preview and shows the writes-nothing banner with the would-issue table', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValueOnce({
      ok: true,
      preview: {
        total_missing: 37, sample_size: 2, writes: 0,
        sample: [
          { user_id: 5, created_at: '2024-03-01 09:00:00', would_issue: 'PCI-2024-000005', conflict: false },
          { user_id: 9, created_at: '2025-01-15 09:00:00', would_issue: 'PCI-2025-000009', conflict: true },
        ],
      },
    })
    renderPage()
    await user.click(screen.getByRole('button', { name: 'Preview (dry run)' }))
    await waitFor(() =>
      expect(h.post).toHaveBeenCalledWith('/api/admin/identity/student-numbers/backfill/preview?limit=100', undefined),
    )
    expect(screen.getByText(/Dry run — this preview wrote nothing\./)).toBeInTheDocument()
    expect(screen.getByText('PCI-2024-000005')).toBeInTheDocument()
    expect(screen.getByText(/conflict — would be quarantined/)).toBeInTheDocument()
  })

  it('surfaces a preview failure', async () => {
    const user = userEvent.setup()
    h.post.mockRejectedValueOnce(new ApiError(500, 'Request failed (500)', {}))
    renderPage()
    await user.click(screen.getByRole('button', { name: 'Preview (dry run)' }))
    expect(await screen.findByText('Request failed (500)')).toBeInTheDocument()
  })
})

describe('backfill run confirmation gate', () => {
  const confirmBox = () => screen.getByLabelText('Type "run backfill" to confirm')
  const runButton = () => screen.getByRole('button', { name: 'Run backfill' })

  it('keeps the run disarmed until "run backfill" is typed literally', async () => {
    const user = userEvent.setup()
    renderPage()
    expect(runButton()).toBeDisabled()
    await user.type(confirmBox(), 'run the backfill')
    expect(runButton()).toBeDisabled()
    await user.clear(confirmBox())
    await user.type(confirmBox(), 'RUN BACKFILL') // case matters — the phrase is literal
    expect(runButton()).toBeDisabled()
    await user.clear(confirmBox())
    await user.type(confirmBox(), 'run backfill')
    expect(runButton()).toBeEnabled()
    expect(h.post).not.toHaveBeenCalled() // arming alone never calls the API
  })

  it('runs with the chosen batch size and renders the result, then disarms again', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValueOnce({
      ok: true,
      correlation_id: 'cafe1234',
      result: { examined: 50, issued: 48, quarantined: 2, high_water_user_id: 912, remaining: 120, failures: [{ user_id: 42, reason: 'number reserved to another identity' }] },
    })
    renderPage()
    await user.clear(screen.getByLabelText('Batch size'))
    await user.type(screen.getByLabelText('Batch size'), '50')
    await user.type(confirmBox(), 'run backfill')
    await user.click(runButton())
    await waitFor(() =>
      expect(h.post).toHaveBeenCalledWith('/api/admin/identity/student-numbers/backfill/run', { batch_size: 50 }),
    )
    expect(screen.getByText('cafe1234')).toBeInTheDocument()
    expect(screen.getByText('Issued')).toBeInTheDocument()
    expect(screen.getByText('48')).toBeInTheDocument()
    expect(screen.getByText('Quarantined this batch')).toBeInTheDocument()
    expect(screen.getByText(/number reserved to another identity/)).toBeInTheDocument()
    // the confirmation is consumed: the next batch needs a fresh deliberate arming
    expect(confirmBox()).toHaveValue('')
    expect(runButton()).toBeDisabled()
  })

  it('is hidden without id_backfill', () => {
    h.perms = ['id_read']
    renderPage()
    expect(screen.queryByRole('button', { name: 'Run backfill' })).not.toBeInTheDocument()
    expect(screen.queryByText('Backfill missing numbers')).not.toBeInTheDocument()
  })
})

describe('reconciliation', () => {
  it('fetches on demand and reports drift with number-only samples', async () => {
    const user = userEvent.setup()
    h.get.mockResolvedValueOnce({
      ok: true,
      reconciliation: {
        projection_without_reservation: 1, reservation_without_projection: 0,
        sample_projection_without_reservation: ['PCI-2023-000123'],
        sample_reservation_without_projection: [],
        reconciled: false,
      },
    })
    renderPage()
    await user.click(screen.getByRole('button', { name: 'Run reconciliation' }))
    await waitFor(() => expect(h.get).toHaveBeenCalledWith('/api/admin/identity/student-numbers/reconcile'))
    expect(screen.getByText('Drift detected')).toBeInTheDocument()
    expect(screen.getByText('PCI-2023-000123')).toBeInTheDocument()
  })

  it('is hidden without id_audit', () => {
    h.perms = ['id_read']
    renderPage()
    expect(screen.queryByRole('button', { name: 'Run reconciliation' })).not.toBeInTheDocument()
  })
})

describe('merge queue (maker-checker)', () => {
  it('lists requests with who requested them', () => {
    h.merges = { ok: true, rows: [pendingMerge()] }
    renderPage()
    expect(screen.getByText('#101 → #202')).toBeInTheDocument()
    expect(screen.getByText('admin #7')).toBeInTheDocument()
    expect(screen.getByText('pending')).toBeInTheDocument()
  })

  it('opens the per-table counts preview when a row is selected', async () => {
    const user = userEvent.setup()
    h.merges = { ok: true, rows: [pendingMerge()] }
    h.mergeDetail = mergeDetailFor(pendingMerge())
    renderPage()
    await user.click(screen.getByText('#101 → #202'))
    expect(await screen.findByText(/what would move/)).toBeInTheDocument()
    expect(screen.getByText('Source #101 (absorbed)')).toBeInTheDocument()
    expect(screen.getByText('Target #202 (survivor)')).toBeInTheDocument()
    expect(screen.getByText('payments')).toBeInTheDocument()
    // counts, not member records — the preview never shows names or emails
    expect(screen.getByText(/per-table record counts/)).toBeInTheDocument()
  })

  it('explains a 409 maker_checker as "a different admin must decide this"', async () => {
    const user = userEvent.setup()
    h.merges = { ok: true, rows: [pendingMerge()] }
    h.mergeDetail = mergeDetailFor(pendingMerge())
    h.post.mockRejectedValueOnce(new ApiError(409, 'maker_checker', { error: 'maker_checker' }))
    renderPage()
    await user.click(screen.getByText('#101 → #202'))
    await user.click(await screen.findByRole('button', { name: 'Approve & execute' }))
    const alert = await screen.findByRole('alert')
    expect(alert).toHaveTextContent(/A different admin must decide this/)
    expect(alert).toHaveTextContent(/can never be the one who approves it/)
    // and it is NOT rendered as the raw code or a generic failure
    expect(alert).not.toHaveTextContent('maker_checker')
  })

  it('refreshes the queue on a 409 already_decided instead of erroring', async () => {
    const user = userEvent.setup()
    h.merges = { ok: true, rows: [pendingMerge()] }
    h.mergeDetail = mergeDetailFor(pendingMerge())
    h.post.mockRejectedValueOnce(new ApiError(409, 'already_decided', { error: 'already_decided' }))
    renderPage()
    await user.click(screen.getByText('#101 → #202'))
    await user.click(await screen.findByRole('button', { name: 'Approve & execute' }))
    expect(await screen.findByText(/already decided by another admin — the queue has been refreshed/)).toBeInTheDocument()
    expect(h.refetchMerges).toHaveBeenCalled()
    expect(h.refetchDetail).toHaveBeenCalled()
    expect(screen.queryByRole('alert')).not.toBeInTheDocument()
  })

  it('approves with the note and reports the executed merge', async () => {
    const user = userEvent.setup()
    h.merges = { ok: true, rows: [pendingMerge()] }
    h.mergeDetail = mergeDetailFor(pendingMerge())
    h.post.mockResolvedValueOnce({ ok: true, result: {} })
    renderPage()
    await user.click(screen.getByText('#101 → #202'))
    await user.type(await screen.findByLabelText('Decision note'), 'verified same person')
    await user.click(screen.getByRole('button', { name: 'Approve & execute' }))
    await waitFor(() =>
      expect(h.post).toHaveBeenCalledWith('/api/admin/identity/merges/11/approve', { note: 'verified same person' }),
    )
    expect(screen.getByText(/Merge approved and executed/)).toBeInTheDocument()
  })

  it('creates a merge request and maps the already_pending refusal to plain language', async () => {
    const user = userEvent.setup()
    h.post.mockRejectedValueOnce(new ApiError(409, 'already_pending', { error: 'already_pending' }))
    renderPage()
    await user.type(screen.getByLabelText('Source user id (absorbed)'), '101')
    await user.type(screen.getByLabelText('Target user id (survivor)'), '202')
    await user.click(screen.getByRole('button', { name: 'Request merge' }))
    await waitFor(() =>
      expect(h.post).toHaveBeenCalledWith('/api/admin/identity/merges', { source_user_id: 101, target_user_id: 202 }),
    )
    expect(screen.getByText(/already part of a pending merge request/)).toBeInTheDocument()
  })

  it('hides decision buttons without id_merge_approve and the request form without id_merge_request', async () => {
    const user = userEvent.setup()
    h.perms = ['id_read']
    h.merges = { ok: true, rows: [pendingMerge()] }
    h.mergeDetail = mergeDetailFor(pendingMerge())
    renderPage()
    expect(screen.queryByRole('button', { name: 'Request merge' })).not.toBeInTheDocument()
    await user.click(screen.getByText('#101 → #202'))
    expect(await screen.findByText(/what would move/)).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Approve & execute' })).not.toBeInTheDocument()
  })
})
