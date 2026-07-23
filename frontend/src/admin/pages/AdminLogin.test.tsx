import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'
import { render } from '@testing-library/react'
import { ApiError } from '../../api/client'

// FE-11 — admin console sign-in. The admin console has no i18n provider (English-only), so this
// screen is rendered under a bare MemoryRouter. useAdminAuth.login and adminApi (forgot/recover)
// are mocked to drive the TOTP step-up and the two password-recovery sub-flows.
const h = vi.hoisted(() => ({ login: vi.fn(), post: vi.fn(), navigate: vi.fn() }))
vi.mock('../AdminAuth', () => ({ useAdminAuth: () => ({ login: h.login }) }))
vi.mock('../api', () => ({ adminApi: { post: h.post } }))
vi.mock('react-router-dom', async (orig) => ({
  ...(await orig<typeof import('react-router-dom')>()),
  useNavigate: () => h.navigate,
}))

import AdminLogin from './AdminLogin'

// The login endpoint rejects with an ApiError whose body carries the failure code.
const apiErr = (code: string) => new ApiError(401, 'unauthorized', { error: code })

function renderLogin() {
  return render(
    <MemoryRouter initialEntries={['/']}>
      <AdminLogin />
    </MemoryRouter>,
  )
}

describe('AdminLogin', () => {
  beforeEach(() => {
    h.login.mockReset()
    h.post.mockReset()
    h.navigate.mockReset()
  })

  it('signs in with lowercased email and navigates on success', async () => {
    const user = userEvent.setup()
    h.login.mockResolvedValue(undefined)
    renderLogin()
    await user.type(screen.getByLabelText('Email address'), 'Admin@PCI.io')
    await user.type(screen.getByLabelText('Password'), 'secret')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    await waitFor(() => expect(h.login).toHaveBeenCalledWith('admin@pci.io', 'secret', undefined))
    await waitFor(() => expect(h.navigate).toHaveBeenCalledWith('/', { replace: true }))
  })

  it('reveals the TOTP field and message when the account has 2FA (totp_required)', async () => {
    const user = userEvent.setup()
    h.login.mockRejectedValueOnce(apiErr('totp_required'))
    renderLogin()
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    expect(await screen.findByLabelText('Authentication code')).toBeInTheDocument()
    expect(screen.getByRole('alert')).toHaveTextContent(/two-factor authentication/i)
    expect(h.navigate).not.toHaveBeenCalled()
  })

  it('shows the invalid-code message on totp_invalid but keeps the field', async () => {
    const user = userEvent.setup()
    h.login.mockRejectedValueOnce(apiErr('totp_invalid'))
    renderLogin()
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    expect(await screen.findByLabelText('Authentication code')).toBeInTheDocument()
    expect(screen.getByRole('alert')).toHaveTextContent(/not valid/i)
  })

  it('sends the TOTP code on the follow-up attempt', async () => {
    const user = userEvent.setup()
    h.login.mockRejectedValueOnce(apiErr('totp_required')).mockResolvedValueOnce(undefined)
    renderLogin()
    await user.type(screen.getByLabelText('Email address'), 'a@b.co')
    await user.type(screen.getByLabelText('Password'), 'pw')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    await user.type(await screen.findByLabelText('Authentication code'), ' 654321 ')
    await user.click(screen.getByRole('button', { name: 'Sign in' }))
    await waitFor(() => expect(h.login).toHaveBeenLastCalledWith('a@b.co', 'pw', '654321'))
  })

  it('forgot-password flow always confirms a link was sent (no account enumeration)', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValue(undefined)
    renderLogin()
    await user.click(screen.getByRole('button', { name: 'Forgot password?' }))
    await user.type(screen.getByLabelText('Email address'), 'admin@pci.io')
    await user.click(screen.getByRole('button', { name: 'Send reset link' }))
    expect(await screen.findByText(/password-reset link is on its way/i)).toBeInTheDocument()
    expect(h.post).toHaveBeenCalledWith('/api/admin/auth/forgot', { email: 'admin@pci.io' })
  })

  it('recover flow rejects a short new password client-side before calling the API', async () => {
    const user = userEvent.setup()
    renderLogin()
    await user.click(screen.getByRole('button', { name: 'Use a recovery code' }))
    await user.type(screen.getByLabelText('Email address'), 'admin@pci.io')
    await user.type(screen.getByLabelText('Recovery code'), 'RC-1')
    await user.type(screen.getByLabelText('New password'), 'short')
    await user.click(screen.getByRole('button', { name: 'Reset password' }))
    expect(await screen.findByRole('alert')).toHaveTextContent(/at least 8 characters/i)
    expect(h.post).not.toHaveBeenCalled()
  })

  it('recover flow posts the code + new password and confirms on success', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValue(undefined)
    renderLogin()
    await user.click(screen.getByRole('button', { name: 'Use a recovery code' }))
    await user.type(screen.getByLabelText('Email address'), 'Admin@PCI.io')
    await user.type(screen.getByLabelText('Recovery code'), ' RC-9 ')
    await user.type(screen.getByLabelText('New password'), 'longenough1')
    await user.click(screen.getByRole('button', { name: 'Reset password' }))
    await waitFor(() =>
      expect(h.post).toHaveBeenCalledWith('/api/admin/auth/recover', {
        email: 'admin@pci.io',
        code: 'RC-9',
        new_password: 'longenough1',
      }),
    )
    expect(await screen.findByText(/password has been reset/i)).toBeInTheDocument()
  })
})
