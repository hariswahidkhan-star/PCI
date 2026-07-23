import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { ApiError } from '../../api/client'

// FE-12 — admin operational consoles. Payments is the money-decision surface: the filtered ledger,
// and the finance-gated Reconciliation tab whose reprocess (idempotent) and reverse (reason-required,
// reversible-only-for-manual-providers) actions move real entitlements. useAdminQuery + adminApi are
// mocked; the real ApiError is kept so the 403 → "requires finance" branch runs as in production.
const h = vi.hoisted(() => ({
  payments: null as unknown,
  paths: [] as (string | null)[],
  get: vi.fn(),
  post: vi.fn(),
}))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    h.paths.push(path)
    return { data: h.payments, loading: false, error: null, refetch: vi.fn() }
  },
}))
vi.mock('../api', () => ({
  adminApi: {
    get: (path: string) => h.get(path),
    post: (path: string, body?: unknown) => h.post(path, body),
  },
}))

import Payments from './Payments'

const paymentsResp = () => ({
  totals: { paid: 420, n: 2, refunded: 0 },
  rows: [
    { id: 1, reference: 'PAY-1', email: 'a@ex.co', product_type: 'membership', final_amount: 120, currency: 'USD', payment_status: 'paid', payment_date: '2025-02-01T00:00:00Z' },
    { id: 2, reference: 'PAY-2', email: 'b@ex.co', product_type: 'exam', final_amount: 300, currency: 'USD', payment_status: 'paid', payment_date: '2025-03-01T00:00:00Z' },
  ],
})

const reconResp = () => ({
  exceptions: 1,
  rows: [
    // A Stripe-paid exam with a missing entitlement — NOT reversible (only manual providers are).
    { id: 10, user_id: 1, email: 'a@ex.co', is_test: false, product: 'exam', amount: 300, provider: 'stripe', gateway_id: 'pi_1', status: 'paid', entitlement_ok: false, membership_status: null, certuvo_status: null, reconciled: false, exception: 'entitlement_missing' },
    // An admin fee waiver — reversible (manual provider + waived).
    { id: 11, user_id: 2, email: 'b@ex.co', is_test: false, product: 'membership', amount: 0, waived_amount: 150, provider: 'admin_waiver', gateway_id: null, status: 'waived', entitlement_ok: true, membership_status: 'active', certuvo_status: null, reconciled: true, exception: null },
  ],
})

describe('Payments admin console', () => {
  beforeEach(() => {
    h.payments = paymentsResp()
    h.paths = []
    h.get.mockReset()
    h.post.mockReset()
  })

  describe('Payments ledger tab', () => {
    it('renders the totals and one row per payment', () => {
      render(<Payments />)
      const table = screen.getByRole('table')
      expect(within(table).getAllByRole('row').slice(1)).toHaveLength(2)
      expect(screen.getByText('PAY-1')).toBeInTheDocument()
      expect(screen.getByText('b@ex.co')).toBeInTheDocument()
    })

    it('folds a status filter into the query path', async () => {
      const user = userEvent.setup()
      render(<Payments />)
      // The initial load has no query string.
      expect(h.paths[0]).toBe('/api/admin/payments')
      // The status select is the first of the two unlabeled comboboxes in the filter bar.
      await user.selectOptions(screen.getAllByRole('combobox')[0], 'paid')
      await waitFor(() => expect(h.paths[h.paths.length - 1]).toContain('status=paid'))
    })

    it('shows the empty state when nothing matches', () => {
      h.payments = { totals: { paid: 0, n: 0, refunded: 0 }, rows: [] }
      render(<Payments />)
      expect(screen.getByText('No payments match.')).toBeInTheDocument()
    })
  })

  describe('Reconciliation tab (finance-gated)', () => {
    async function openRecon(user: ReturnType<typeof userEvent.setup>) {
      await user.click(screen.getByRole('button', { name: 'Reconciliation' }))
    }

    it('renders the friendly forbidden note when the finance permission is missing (403)', async () => {
      const user = userEvent.setup()
      h.get.mockRejectedValue(new ApiError(403, 'forbidden', {}))
      render(<Payments />)
      await openRecon(user)
      expect(await screen.findByText('Requires the finance permission.')).toBeInTheDocument()
    })

    it('lists rows with the exceptions banner and only offers Reverse on manual-provider rows', async () => {
      const user = userEvent.setup()
      h.get.mockResolvedValue(reconResp())
      render(<Payments />)
      await openRecon(user)
      expect(await screen.findByText(/1 exception needs attention/)).toBeInTheDocument()
      // Every row can be reprocessed; only the admin_waiver row can be reversed.
      expect(screen.getAllByRole('button', { name: 'Reprocess' })).toHaveLength(2)
      expect(screen.getAllByRole('button', { name: 'Reverse' })).toHaveLength(1)
    })

    it('reprocess posts to the idempotent endpoint and reports what was ensured', async () => {
      const user = userEvent.setup()
      h.get.mockResolvedValue(reconResp())
      render(<Payments />)
      await openRecon(user)
      await screen.findByText(/1 exception needs attention/)
      h.post.mockResolvedValueOnce({ ok: true, ensured: ['exam_entitlement'] })
      await user.click(screen.getAllByRole('button', { name: 'Reprocess' })[0])
      await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/admin/payments/10/reprocess', {}))
      expect(await screen.findByText(/ensured: exam entitlement/)).toBeInTheDocument()
    })

    it('reverse refuses to proceed without a reason, then posts the reason', async () => {
      const user = userEvent.setup()
      h.get.mockResolvedValue(reconResp())
      render(<Payments />)
      await openRecon(user)
      await screen.findByText(/1 exception needs attention/)
      // Open the reversal form for the reversible (admin_waiver) row.
      await user.click(screen.getByRole('button', { name: 'Reverse' }))
      // Reverse with an empty reason is blocked client-side.
      await user.click(screen.getByRole('button', { name: 'Reverse' }))
      expect(await screen.findByText('A reversal must record why.')).toBeInTheDocument()
      expect(h.post).not.toHaveBeenCalled()
      // With a reason it posts.
      await user.type(screen.getByPlaceholderText('Reason'), 'duplicate charge')
      h.post.mockResolvedValueOnce({ ok: true })
      await user.click(screen.getByRole('button', { name: 'Reverse' }))
      await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/admin/payments/11/reverse', { reason: 'duplicate charge' }))
      expect(await screen.findByText(/reversed/)).toBeInTheDocument()
    })
  })
})
