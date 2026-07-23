import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import userEvent from '@testing-library/user-event'

// FE-12 — the Discount & founding codes console is the money/fraud-control surface: it mints codes
// that reduce or waive fees. This pins the codes list + enable/disable toggle and the create-code
// form's validation gate + payload (including the founding branch that waives the discount value).
const h = vi.hoisted(() => ({
  codes: [] as unknown[],
  post: vi.fn(),
  patch: vi.fn(),
}))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    let data: unknown = null
    if (path === '/api/admin/codes') data = h.codes // a bare DiscountCode[] on this endpoint
    else if (path === '/api/admin/certifications' || path === '/api/admin/training-partners') data = { rows: [] }
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))
vi.mock('../api', () => ({
  adminApi: {
    post: (p: string, b?: unknown) => h.post(p, b),
    patch: (p: string, b?: unknown) => h.patch(p, b),
  },
}))

import Codes from './Codes'

const renderCodes = () => render(<MemoryRouter><Codes /></MemoryRouter>)

const pctCode = (over: Record<string, unknown> = {}) => ({
  id: 7, code: 'LAUNCH20', discount_type: 'percentage', discount_value: 20, applies_to: 'exam',
  used_count: 3, max_uses: 100, active: true, ...over,
})

describe('Codes admin console', () => {
  beforeEach(() => {
    h.codes = []
    h.post.mockReset()
    h.patch.mockReset()
  })

  describe('codes list', () => {
    it('renders a percentage code with its discount summary and use count', () => {
      h.codes = [pctCode()]
      renderCodes()
      expect(screen.getByText('LAUNCH20')).toBeInTheDocument()
      expect(screen.getByText(/20% on exam fee/)).toBeInTheDocument()
      expect(screen.getByText('3 / 100')).toBeInTheDocument()
    })

    it('shows the empty state when there are no codes', () => {
      renderCodes()
      expect(screen.getByText('No discount codes yet.')).toBeInTheDocument()
    })

    it('disables an active code via PATCH', async () => {
      const user = userEvent.setup()
      h.codes = [pctCode()]
      h.patch.mockResolvedValueOnce(undefined)
      renderCodes()
      await user.click(screen.getByRole('button', { name: 'Disable' }))
      await waitFor(() => expect(h.patch).toHaveBeenCalledWith('/api/admin/codes/7', { active: false }))
    })

    it('enables an inactive code via PATCH', async () => {
      const user = userEvent.setup()
      h.codes = [pctCode({ active: false })]
      h.patch.mockResolvedValueOnce(undefined)
      renderCodes()
      await user.click(screen.getByRole('button', { name: 'Enable' }))
      await waitFor(() => expect(h.patch).toHaveBeenCalledWith('/api/admin/codes/7', { active: true }))
    })
  })

  describe('create code form', () => {
    async function openForm(user: ReturnType<typeof userEvent.setup>) {
      await user.click(screen.getByRole('button', { name: 'New code' }))
      return screen.getByRole('button', { name: 'Create code' })
    }
    const valueInput = () => screen.getByText(/^Value/).closest('.field')!.querySelector('input') as HTMLInputElement

    it('keeps Create disabled until a standard code has both a code and a value', async () => {
      const user = userEvent.setup()
      renderCodes()
      const create = await openForm(user)
      expect(create).toBeDisabled()
      await user.type(screen.getByPlaceholderText('LAUNCH20'), 'save25')
      expect(create).toBeDisabled() // still no value
      await user.type(valueInput(), '25')
      expect(create).toBeEnabled()
    })

    it('posts the create with an upper-cased code and a numeric discount value', async () => {
      const user = userEvent.setup()
      renderCodes()
      const create = await openForm(user)
      await user.type(screen.getByPlaceholderText('LAUNCH20'), 'save25')
      await user.type(valueInput(), '25')
      h.post.mockResolvedValueOnce(undefined)
      await user.click(create)
      await waitFor(() =>
        expect(h.post).toHaveBeenCalledWith('/api/admin/codes', expect.objectContaining({
          code: 'SAVE25', discount_type: 'percentage', discount_value: 25,
        })),
      )
    })

    it('a founding-kind code needs only a code (the discount value is waived)', async () => {
      const user = userEvent.setup()
      renderCodes()
      const create = await openForm(user)
      const kind = screen.getByText('Kind').closest('.field')!.querySelector('select') as HTMLSelectElement
      await user.selectOptions(kind, 'founding')
      // The value field is gone in founding mode…
      expect(screen.queryByText(/^Value/)).not.toBeInTheDocument()
      // …and a code alone enables Create.
      await user.type(screen.getByPlaceholderText('FOUNDING2026'), 'gold2026')
      expect(create).toBeEnabled()
    })
  })
})
