import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE — the Certuvo access page. It shares only the external practice-platform credentials, so the
// decisions worth pinning are: the panel is hidden unless the account is enabled; the status maps to
// the right badge (active → Ready, suspended/revoked → Unavailable, not_provisioned → After
// membership); the active card shows the username + an Open-Certuvo link (login_url, falling back to
// portal_url) and reveals the temporary password only when present; the inactive card surfaces the
// friendly server message (never a raw error); and resend maps a 429 to a "please wait" note while a
// structured ApiError shows its body.message. The data hook + fetch client are mocked at the module
// boundary; the REAL ApiError is kept (the module maps it via instanceof + status).
const h = vi.hoisted(() => ({ data: null as unknown, post: vi.fn() }))

vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: h.data, loading: false, error: null, refetch: vi.fn() }),
}))
vi.mock('../api/client', async (orig) => {
  const actual = await orig<typeof import('../api/client')>()
  return { ...actual, api: { post: (p: string, b?: unknown) => h.post(p, b) } }
})

import Certuvo from './Certuvo'
import { ApiError } from '../api/client'

describe('Certuvo access page', () => {
  beforeEach(() => {
    h.data = null
    h.post.mockReset()
  })

  it('hides the access card entirely when Certuvo is not enabled', () => {
    h.data = { enabled: false }
    render(<Certuvo />)
    expect(screen.getByRole('heading', { name: 'Certuvo' })).toBeInTheDocument() // page intro still renders
    expect(screen.queryByText('Your Certuvo practice access')).toBeNull()
  })

  it('shows the active account with its username, Open-Certuvo link and Ready badge', () => {
    h.data = {
      enabled: true,
      status: 'active',
      username: 'stu-1042',
      login_url: 'https://certuvo.test/u/1042',
      must_change_password: true,
    }
    render(<Certuvo />)
    expect(screen.getByText('Ready')).toBeInTheDocument()
    expect(screen.getByText('stu-1042')).toBeInTheDocument()
    expect(screen.getByText(/asked to set your own password/)).toBeInTheDocument()
    expect(screen.getByRole('link', { name: /Open Certuvo/ })).toHaveAttribute('href', 'https://certuvo.test/u/1042')
  })

  it('reveals the temporary password only when the server supplies one', () => {
    h.data = { enabled: true, status: 'active', username: 'stu-1', password: 'Temp-9x!' }
    const { rerender } = render(<Certuvo />)
    expect(screen.getByText('Temp-9x!')).toBeInTheDocument()
    h.data = { enabled: true, status: 'active', username: 'stu-1' }
    rerender(<Certuvo />)
    expect(screen.queryByText('Temp-9x!')).toBeNull()
  })

  it('falls back to the portal_url for the Open-Certuvo link when there is no per-user login_url', () => {
    h.data = { enabled: true, status: 'active', username: 'stu-2', portal_url: 'https://certuvo.test/signin' }
    render(<Certuvo />)
    expect(screen.getByRole('link', { name: /Open Certuvo/ })).toHaveAttribute('href', 'https://certuvo.test/signin')
  })

  it('labels a not-yet-provisioned account "After membership" and shows the friendly message', () => {
    h.data = { enabled: true, status: 'not_provisioned', message: 'Your Certuvo account activates once your membership is active.' }
    render(<Certuvo />)
    expect(screen.getByText('After membership')).toBeInTheDocument()
    expect(screen.getByText('Your Certuvo account activates once your membership is active.')).toBeInTheDocument()
    // Inactive → no username row is rendered.
    expect(screen.queryByText(/Username:/)).toBeNull()
  })

  it('resends access instructions and confirms success', async () => {
    const user = userEvent.setup()
    h.data = { enabled: true, status: 'active', username: 'stu-3' }
    render(<Certuvo />)
    await user.click(screen.getByRole('button', { name: 'Resend access instructions' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/certuvo/resend', {}))
    expect(await screen.findByText('Access instructions sent to your email.')).toBeInTheDocument()
  })

  it('maps a 429 on resend to a friendly wait message', async () => {
    const user = userEvent.setup()
    h.data = { enabled: true, status: 'active', username: 'stu-4' }
    h.post.mockRejectedValue(new ApiError(429, 'too many', { message: 'rate limited' }))
    render(<Certuvo />)
    await user.click(screen.getByRole('button', { name: 'Resend access instructions' }))
    expect(await screen.findByText('Please wait a while before resending.')).toBeInTheDocument()
  })
})
