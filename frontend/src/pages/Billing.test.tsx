import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE-4 — in-portal purchasing. The most important behaviour to pin is the discount/founding-code
// gate: a code is validated for the specific product BEFORE Stripe is opened, so an invalid or
// wrong-product code is caught here instead of silently charging full price. We also pin the
// membership active/inactive branch, the CPD-blocked recert guard, and the payment-history receipt.
const h = vi.hoisted(() => ({
  me: null as unknown,
  refetch: vi.fn(),
  post: vi.fn(),
  startCheckout: vi.fn(),
  pricing: null as unknown,
  certs: { rows: [{ id: 1, code: 'PCL-AI', name: 'Project Controls Lead — AI' }] } as unknown,
}))

vi.mock('../data/MeContext', () => ({
  useMe: () => ({ me: h.me, loading: false, error: null, refetch: h.refetch }),
}))
// Route the two useQuery consumers by path: pricing, the certification catalogue, and (from the
// nested FoundingCard) the founding-application probe, which we leave empty.
vi.mock('../api/hooks', () => ({
  useQuery: (path: string | null) => {
    let data: unknown = null
    if (path && path.startsWith('/api/pricing')) data = h.pricing
    else if (path === '/api/certifications') data = h.certs
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))
vi.mock('../api/client', () => ({ api: { post: (p: string, b?: unknown) => h.post(p, b) } }))
vi.mock('../api/checkout', () => ({
  startCheckout: (opts: unknown) => h.startCheckout(opts),
  checkoutErrorMessage: (e: unknown) => (e instanceof Error ? e.message : 'Could not start checkout.'),
}))

import Billing from './Billing'

const pricing = () => ({
  currency: 'USD',
  membership: { final: 120, standard: 150, defaultDiscount: 30 },
  exam: { final: 300, standard: 300, defaultDiscount: 0 },
  bundle: { final: 400, standard: 450, defaultDiscount: 50 },
  renewal: { final: 100, standard: 100, defaultDiscount: 0 },
  recert: { final: 80, standard: 80, defaultDiscount: 0 },
  cert: null,
})

const baseMe = (over: Record<string, unknown> = {}) => ({
  user: { id: 1, email: 'sam@example.com', first_name: 'Sam', last_name: 'Rivera', registration_no: 'PCI-1', created_at: '2025-01-01T00:00:00Z' },
  lifecycle: { membership_status: 'none' },
  membership: null,
  membership_grade: null,
  membership_dues: null,
  exams: [],
  payments: [],
  ...over,
})

describe('Billing (in-portal purchasing)', () => {
  beforeEach(() => {
    h.refetch.mockReset()
    h.post.mockReset()
    h.startCheckout.mockReset()
    h.pricing = pricing()
    h.certs = { rows: [{ id: 1, code: 'PCL-AI', name: 'Project Controls Lead — AI' }] }
    h.me = baseMe()
  })

  it('shows Active (no activate button) once membership is active', () => {
    h.me = baseMe({ lifecycle: { membership_status: 'active' } })
    renderWithProviders(<Billing />)
    expect(screen.getByText('Active')).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Activate membership' })).not.toBeInTheDocument()
  })

  it('opens checkout for membership with no code entered', async () => {
    const user = userEvent.setup()
    h.startCheckout.mockResolvedValue(undefined)
    renderWithProviders(<Billing />)
    await user.click(screen.getByRole('button', { name: 'Activate membership' }))
    await waitFor(() =>
      expect(h.startCheckout).toHaveBeenCalledWith(expect.objectContaining({ product: 'membership', email: 'sam@example.com' })),
    )
    // No code was entered, so no pre-validation call happened.
    expect(h.post).not.toHaveBeenCalled()
  })

  it('blocks checkout and shows the message when the entered code is invalid for the product', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValueOnce({ valid: false, message: 'This code is not valid for membership.' })
    renderWithProviders(<Billing />)
    await user.type(screen.getByLabelText('Discount or founding code'), 'EXPIRED10')
    await user.click(screen.getByRole('button', { name: 'Activate membership' }))
    // The code is validated for THIS product before Stripe is ever opened.
    await waitFor(() =>
      expect(h.post).toHaveBeenCalledWith('/api/validate-code', { code: 'EXPIRED10', product: 'membership', email: 'sam@example.com' }),
    )
    expect(await screen.findByRole('alert')).toHaveTextContent('This code is not valid for membership.')
    expect(h.startCheckout).not.toHaveBeenCalled()
  })

  it('validates a good code first, then opens checkout carrying the code', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValueOnce({ valid: true })
    h.startCheckout.mockResolvedValue(undefined)
    renderWithProviders(<Billing />)
    await user.type(screen.getByLabelText('Discount or founding code'), 'SAVE10')
    await user.click(screen.getByRole('button', { name: 'Activate membership' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/validate-code', expect.objectContaining({ code: 'SAVE10', product: 'membership' })))
    await waitFor(() => expect(h.startCheckout).toHaveBeenCalledWith(expect.objectContaining({ product: 'membership', code: 'SAVE10' })))
  })

  it('pays the exam fee for the selected certification', async () => {
    const user = userEvent.setup()
    h.startCheckout.mockResolvedValue(undefined)
    renderWithProviders(<Billing />)
    await user.click(screen.getByRole('button', { name: 'Pay exam fee' }))
    await waitFor(() =>
      expect(h.startCheckout).toHaveBeenCalledWith(expect.objectContaining({ product: 'exam', cert: 'PCL-AI' })),
    )
  })

  it('disables the recert button while the CPD requirement is unmet', () => {
    h.me = baseMe({
      lifecycle: { membership_status: 'active' },
      exams: [
        {
          certification_id: 1,
          certification_code: 'PCL-AI',
          certification_name: 'Project Controls Lead — AI',
          payment_id: 9,
          credential: { credential_id: 'C-1', status: 'active', expires_at: '2026-08-01T00:00:00Z' },
          recert_cpd: { required: 30, approved: 10, met: false },
        },
      ],
    })
    renderWithProviders(<Billing />)
    const recert = screen.getByRole('button', { name: 'Recertify' })
    expect(recert).toBeDisabled()
    expect(screen.getByText(/complete your CPD to recertify/i)).toBeInTheDocument()
  })

  it('renders paid history rows and offers a receipt only for paid payments', () => {
    h.me = baseMe({
      payments: [
        { id: 1, product_type: 'membership', reference: 'R-100', final_amount: 120, currency: 'USD', payment_status: 'paid', payment_date: '2025-02-01T00:00:00Z' },
        { id: 2, product_type: 'exam', reference: 'R-101', final_amount: 300, currency: 'USD', payment_status: 'pending', payment_date: '2025-03-01T00:00:00Z' },
      ],
    })
    renderWithProviders(<Billing />)
    const table = screen.getByRole('table')
    const rows = within(table).getAllByRole('row').slice(1) // drop the header row
    expect(rows).toHaveLength(2)
    // The paid row has a Receipt button; the pending one does not.
    expect(within(rows[0]).getByRole('button', { name: 'Receipt' })).toBeInTheDocument()
    expect(within(rows[1]).queryByRole('button', { name: 'Receipt' })).not.toBeInTheDocument()
  })

  it('shows the empty state when there are no payments', () => {
    renderWithProviders(<Billing />)
    expect(screen.getByText('No payments on record.')).toBeInTheDocument()
  })
})
