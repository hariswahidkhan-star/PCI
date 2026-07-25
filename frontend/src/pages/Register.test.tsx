import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, fireEvent, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE-2 — free account creation. `submit()` runs a client-side validation gate (min length, password
// match, country, phone) BEFORE it ever calls the auth hook, then routes to the correct post-signup
// destination: /onboarding by default, or a Billing deep-link carried from the public site
// (?founding, ?product=exam&cert=…). The auth hook is mocked so we can drive register's success and
// account_exists outcomes without a backend; useNavigate is spied to pin the destination; GoogleButton
// is stubbed (it fetches /api/auth/config on mount). Submission is driven via fireEvent.submit so the
// component's own validation layer is exercised directly (it is defence-in-depth behind the native
// required/minLength attributes, and is what actually guards the register() call).
const h = vi.hoisted(() => ({ register: vi.fn(), navigate: vi.fn() }))
vi.mock('../auth/AuthContext', () => ({ useAuth: () => ({ register: h.register }) }))
vi.mock('../components/GoogleButton', () => ({ default: () => null }))
vi.mock('react-router-dom', async (orig) => ({
  ...(await orig<typeof import('react-router-dom')>()),
  useNavigate: () => h.navigate,
}))

import Register from './Register'

// Fill every required field with valid values; individual tests override one field to probe a branch.
// Selecting the country auto-fills the matching dial code (+61 for Australia), so the phone field is
// the only contact input a test must type. The email is deliberately mixed-case + padded to prove the
// submit handler trims and lowercases it before calling register().
async function fillValid(user: ReturnType<typeof userEvent.setup>) {
  await user.type(screen.getByLabelText('First name'), 'Sam')
  await user.type(screen.getByLabelText('Last name'), 'Rivera')
  await user.type(screen.getByLabelText('Email address'), '  SAM@Example.CO  ')
  await user.type(screen.getByLabelText('Password (8+ characters)'), 'correcthorse')
  await user.type(screen.getByLabelText('Confirm password'), 'correcthorse')
  await user.selectOptions(screen.getByLabelText('Country'), 'Australia')
  await user.type(screen.getByPlaceholderText('Phone number'), '400 000 000')
}

const formOf = (c: HTMLElement) => c.querySelector('form') as HTMLFormElement

describe('Register (free signup)', () => {
  beforeEach(() => {
    h.register.mockReset()
    h.navigate.mockReset()
  })

  it('renders the signup form with the free-account heading and required fields', () => {
    renderWithProviders(<Register />)
    expect(screen.getByRole('heading', { name: 'Create your free account' })).toBeInTheDocument()
    expect(screen.getByLabelText('First name')).toBeInTheDocument()
    expect(screen.getByLabelText('Last name')).toBeInTheDocument()
    expect(screen.getByLabelText('Email address')).toBeInTheDocument()
    expect(screen.getByLabelText('Password (8+ characters)')).toBeInTheDocument()
    expect(screen.getByLabelText('Confirm password')).toBeInTheDocument()
    expect(screen.getByLabelText('Country')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Create free account' })).toBeInTheDocument()
  })

  it('blocks a short password and never calls register()', async () => {
    const user = userEvent.setup()
    const { container } = renderWithProviders(<Register />)
    await user.type(screen.getByLabelText('Password (8+ characters)'), 'short')
    fireEvent.submit(formOf(container))
    expect(screen.getByRole('alert')).toHaveTextContent('Password must be at least 8 characters.')
    expect(h.register).not.toHaveBeenCalled()
  })

  it('blocks a password/confirm mismatch and never calls register()', async () => {
    const user = userEvent.setup()
    const { container } = renderWithProviders(<Register />)
    await user.type(screen.getByLabelText('Password (8+ characters)'), 'correcthorse')
    await user.type(screen.getByLabelText('Confirm password'), 'batterystaple')
    fireEvent.submit(formOf(container))
    // The error banner (role=alert) carries the mismatch message; the inline field hint is a span, so
    // querying by role keeps this assertion unambiguous.
    expect(screen.getByRole('alert')).toHaveTextContent('Passwords do not match.')
    expect(h.register).not.toHaveBeenCalled()
  })

  it('submits a normalised payload and lands on /onboarding by default', async () => {
    const user = userEvent.setup()
    h.register.mockResolvedValue(undefined)
    const { container } = renderWithProviders(<Register />)
    await fillValid(user)
    fireEvent.submit(formOf(container))
    await waitFor(() => expect(h.register).toHaveBeenCalledTimes(1))
    expect(h.register).toHaveBeenCalledWith(
      expect.objectContaining({
        firstName: 'Sam',
        lastName: 'Rivera',
        email: 'sam@example.co', // trimmed + lowercased
        password: 'correcthorse',
        country: 'Australia',
        mobile: '+61 400 000 000', // dial auto-filled from the country + trimmed phone
        confirmPassword: 'correcthorse',
      }),
    )
    await waitFor(() => expect(h.navigate).toHaveBeenCalledWith('/onboarding', { replace: true }))
  })

  it('carries an exam deep-link straight to Billing after signup', async () => {
    const user = userEvent.setup()
    h.register.mockResolvedValue(undefined)
    const { container } = renderWithProviders(<Register />, {
      route: '/register?product=exam&cert=pcl-ai',
    })
    await fillValid(user)
    fireEvent.submit(formOf(container))
    await waitFor(() =>
      expect(h.navigate).toHaveBeenCalledWith('/billing?product=exam&cert=pcl-ai', { replace: true }),
    )
  })

  it('maps an account_exists failure to a sign-in hint and does not navigate', async () => {
    const user = userEvent.setup()
    h.register.mockRejectedValue(new Error('account_exists'))
    const { container } = renderWithProviders(<Register />)
    await fillValid(user)
    fireEvent.submit(formOf(container))
    await waitFor(() =>
      expect(screen.getByRole('alert')).toHaveTextContent(
        'An account with this email already exists — try signing in instead.',
      ),
    )
    expect(h.navigate).not.toHaveBeenCalled()
  })
})
