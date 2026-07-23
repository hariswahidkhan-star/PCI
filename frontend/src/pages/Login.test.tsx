import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE-1 — student sign-in with step-up TOTP. The auth hook is mocked so we can drive login's three
// server outcomes (success / totp_required / totp_invalid / generic failure) without a backend, and
// useNavigate is spied to pin the post-login destination (including the founding/product deep-links).
const h = vi.hoisted(() => ({ login: vi.fn(), navigate: vi.fn() }))
vi.mock('../auth/AuthContext', () => ({ useAuth: () => ({ login: h.login }) }))
// GoogleButton fetches /api/auth/config on mount — stub it out so the test stays hermetic.
vi.mock('../components/GoogleButton', () => ({ default: () => null }))
vi.mock('react-router-dom', async (orig) => ({
  ...(await orig<typeof import('react-router-dom')>()),
  useNavigate: () => h.navigate,
}))

import Login from './Login'

// A backend error carries the failure code under `.body.error`, matching the real ApiError shape.
const codeErr = (error: string) => Object.assign(new Error('login failed'), { body: { error } })

describe('Login (student, TOTP step-up)', () => {
  beforeEach(() => {
    h.login.mockReset()
    h.navigate.mockReset()
  })

  it('renders email + password but no TOTP field until the server asks for it', () => {
    renderWithProviders(<Login />)
    expect(screen.getByLabelText('Email address')).toBeInTheDocument()
    expect(screen.getByLabelText('Password')).toBeInTheDocument()
    expect(screen.queryByLabelText('Authentication code')).not.toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Sign in' })).toBeInTheDocument()
  })

  it('lowercases + trims the email and omits TOTP on the first attempt', async () => {
    const user = userEvent.setup()
    h.login.mockResolvedValue(undefined)
    renderWithProviders(<Login />)
    await user.type(screen.getByLabelText('Email address'), '  Alice@Example.COM ')
    await user.type(screen.getByLabelText('Password'), 'hunter2')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    await waitFor(() => expect(h.login).toHaveBeenCalledWith('alice@example.com', 'hunter2', undefined))
    // Default destination is the dashboard root.
    await waitFor(() => expect(h.navigate).toHaveBeenCalledWith('/', { replace: true }))
  })

  it('reveals the TOTP field (no error) when the server answers totp_required', async () => {
    const user = userEvent.setup()
    h.login.mockRejectedValueOnce(codeErr('totp_required'))
    renderWithProviders(<Login />)
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    expect(await screen.findByLabelText('Authentication code')).toBeInTheDocument()
    // totp_required is not an error the user caused — no alert is shown.
    expect(screen.queryByRole('alert')).not.toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Verify & sign in' })).toBeInTheDocument()
    expect(h.navigate).not.toHaveBeenCalled()
  })

  it('reveals the TOTP field WITH an error when a supplied code is invalid', async () => {
    const user = userEvent.setup()
    h.login.mockRejectedValueOnce(codeErr('totp_invalid'))
    renderWithProviders(<Login />)
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    expect(await screen.findByLabelText('Authentication code')).toBeInTheDocument()
    expect(screen.getByRole('alert')).toHaveTextContent(/not valid/i)
  })

  it('passes the entered TOTP code on the follow-up attempt', async () => {
    const user = userEvent.setup()
    h.login.mockRejectedValueOnce(codeErr('totp_required')).mockResolvedValueOnce(undefined)
    renderWithProviders(<Login />)
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    const totp = await screen.findByLabelText('Authentication code')
    await user.type(totp, '  123456 ')
    await user.click(screen.getByRole('button', { name: 'Verify & sign in' }))
    await waitFor(() => expect(h.login).toHaveBeenLastCalledWith('a@b.co', 'pw', '123456'))
  })

  it('shows the generic message text for a non-TOTP failure', async () => {
    const user = userEvent.setup()
    h.login.mockRejectedValueOnce(new Error('Account is locked'))
    renderWithProviders(<Login />)
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    expect(await screen.findByRole('alert')).toHaveTextContent('Account is locked')
    expect(screen.queryByLabelText('Authentication code')).not.toBeInTheDocument()
  })

  it('carries a founding deep-link through sign-in to /billing', async () => {
    const user = userEvent.setup()
    h.login.mockResolvedValue(undefined)
    renderWithProviders(<Login />, { route: '/login?founding=GOLD-123' })
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    await waitFor(() => expect(h.navigate).toHaveBeenCalledWith('/billing?founding=GOLD-123', { replace: true }))
  })

  it('carries a product+cert deep-link through sign-in to /billing', async () => {
    const user = userEvent.setup()
    h.login.mockResolvedValue(undefined)
    renderWithProviders(<Login />, { route: '/login?product=exam&cert=PCL-AI' })
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    await waitFor(() =>
      expect(h.navigate).toHaveBeenCalledWith('/billing?product=exam&cert=PCL-AI', { replace: true }),
    )
  })
})
