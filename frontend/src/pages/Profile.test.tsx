import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE-8 — the account/profile screen. It renders several self-fetching cards; the two that matter
// most for security are the profile-details save (allow-listed fields → PATCH /api/me/profile) and
// the two-factor enrolment card (setup → verify → recovery codes). The data hook and fetch client
// are mocked at the module boundary so the whole page mounts hermetically.
const h = vi.hoisted(() => ({
  me: {} as Record<string, unknown>,
  refetch: vi.fn(),
  get: vi.fn(),
  post: vi.fn(),
  patch: vi.fn(),
  twofa: { enabled: false, pending: false, recovery_remaining: 0 } as {
    enabled: boolean
    pending: boolean
    recovery_remaining: number
  },
}))

vi.mock('../data/MeContext', () => ({
  useMe: () => ({ me: h.me, loading: false, error: null, refetch: h.refetch }),
}))
// The three RepeaterSections (experiences/qualifications/certifications-held) go through useQuery —
// stub it to an empty, settled list so they render their empty state without any network or auth.
vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: { rows: [] }, loading: false, error: null, refetch: vi.fn() }),
}))
vi.mock('../api/client', () => ({
  api: {
    get: (path: string) => h.get(path),
    post: (path: string, body?: unknown) => h.post(path, body),
    patch: (path: string, body?: unknown) => h.patch(path, body),
  },
}))

import Profile from './Profile'

const baseMe = () => ({
  user: {
    first_name: 'Sam',
    last_name: 'Rivera',
    email: 'sam@example.com',
    registration_no: 'PCI-000123',
    created_at: '2025-01-15T00:00:00Z',
  },
  profile: { mobile: '', country: '', profile_completion_percentage: 40 },
})

// api.get is path-routed: the directory + preferences cards resolve to benign defaults; the 2FA card
// resolves to whatever the current test set in h.twofa.
function wireGet() {
  h.get.mockImplementation((path: string) => {
    if (path === '/api/me/2fa') return Promise.resolve(h.twofa)
    if (path === '/api/me/directory')
      return Promise.resolve({ eligible: false, opt_in: false, show_country: false, show_org: false, show_linkedin: false })
    if (path === '/api/me/preferences') return Promise.resolve({ preferences: { email_marketing: 1, newsletter: 0 } })
    return Promise.resolve(null)
  })
}

describe('Profile', () => {
  beforeEach(() => {
    h.me = baseMe()
    h.refetch.mockReset()
    h.get.mockReset()
    h.post.mockReset()
    h.patch.mockReset()
    h.twofa = { enabled: false, pending: false, recovery_remaining: 0 }
    wireGet()
  })

  describe('profile details save', () => {
    it('keeps the save button disabled until a field is edited, then PATCHes only the change', async () => {
      const user = userEvent.setup()
      renderWithProviders(<Profile />)
      const save = await screen.findByRole('button', { name: 'Save changes' })
      expect(save).toBeDisabled()

      const mobile = screen.getByLabelText('Mobile')
      await user.type(mobile, '+15551234')
      expect(save).toBeEnabled()

      h.patch.mockResolvedValueOnce(undefined)
      await user.click(save)
      await waitFor(() => expect(h.patch).toHaveBeenCalledWith('/api/me/profile', { mobile: '+15551234' }))
      expect(await screen.findByText('Profile saved.')).toBeInTheDocument()
      expect(h.refetch).toHaveBeenCalled()
    })

    it('surfaces the server error message and does not clear the form', async () => {
      const user = userEvent.setup()
      renderWithProviders(<Profile />)
      const mobile = await screen.findByLabelText('Mobile')
      await user.type(mobile, '+1999')
      h.patch.mockRejectedValueOnce(new Error('Mobile number is invalid'))
      await user.click(screen.getByRole('button', { name: 'Save changes' }))
      expect(await screen.findByRole('alert')).toHaveTextContent('Mobile number is invalid')
      expect(screen.getByLabelText('Mobile')).toHaveValue('+1999')
    })
  })

  describe('two-factor enrolment', () => {
    it('runs setup → verify and then reveals the one-time recovery codes', async () => {
      const user = userEvent.setup()
      renderWithProviders(<Profile />)
      const begin = await screen.findByRole('button', { name: 'Set up 2FA' })

      h.post.mockResolvedValueOnce({ secret: 'JBSWY3DPEHPK3PXP', otpauth: 'otpauth://totp/PCI:sam' })
      await user.click(begin)
      expect(await screen.findByText('JBSWY3DPEHPK3PXP')).toBeInTheDocument()
      expect(h.post).toHaveBeenCalledWith('/api/me/2fa/setup', {})

      const verify = screen.getByRole('button', { name: 'Verify & enable' })
      expect(verify).toBeDisabled() // no code entered yet
      await user.type(screen.getByPlaceholderText('6-digit code'), '123456')
      expect(verify).toBeEnabled()

      h.post.mockResolvedValueOnce({ recovery_codes: ['aaaa-1111', 'bbbb-2222'] })
      // The verify → reload sequence re-reads /api/me/2fa; report it as enabled afterwards.
      h.twofa = { enabled: true, pending: false, recovery_remaining: 2 }
      await user.click(verify)
      await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/2fa/verify', { code: '123456' }))
      expect(await screen.findByText('aaaa-1111')).toBeInTheDocument()
      expect(screen.getByText('bbbb-2222')).toBeInTheDocument()
      expect(screen.getByText(/Save your recovery codes now/i)).toBeInTheDocument()
    })

    it('shows the enabled state with remaining recovery-code count and a disable control', async () => {
      h.twofa = { enabled: true, pending: false, recovery_remaining: 7 }
      renderWithProviders(<Profile />)
      expect(await screen.findByText('Enabled')).toBeInTheDocument()
      expect(screen.getByText(/Recovery codes remaining: 7/)).toBeInTheDocument()
      expect(screen.getByRole('button', { name: 'Turn off 2FA' })).toBeInTheDocument()
    })

    it('surfaces a verify error from the server body message', async () => {
      const user = userEvent.setup()
      renderWithProviders(<Profile />)
      h.post.mockResolvedValueOnce({ secret: 'S', otpauth: 'o' })
      await user.click(await screen.findByRole('button', { name: 'Set up 2FA' }))
      await user.type(await screen.findByPlaceholderText('6-digit code'), '000000')
      h.post.mockRejectedValueOnce(Object.assign(new Error('x'), { body: { message: 'That code was not valid.' } }))
      await user.click(screen.getByRole('button', { name: 'Verify & enable' }))
      expect(await screen.findByText('That code was not valid.')).toBeInTheDocument()
    })
  })

  describe('communication preferences', () => {
    it('toggles an optional channel and POSTs the full preference map', async () => {
      const user = userEvent.setup()
      renderWithProviders(<Profile />)
      // Newsletter starts off (0); turning it on and saving should send newsletter:1.
      const newsletter = await screen.findByRole('checkbox', { name: /Newsletter/i })
      expect(newsletter).not.toBeChecked()
      await user.click(newsletter)
      expect(newsletter).toBeChecked()

      h.post.mockResolvedValueOnce(undefined)
      await user.click(screen.getByRole('button', { name: 'Save preferences' }))
      await waitFor(() =>
        expect(h.post).toHaveBeenCalledWith('/api/me/preferences', expect.objectContaining({ newsletter: 1, email_marketing: 1 })),
      )
      expect(await screen.findByText('Saved.')).toBeInTheDocument()
    })
  })

  it('shows the read-only account facts from /api/me', async () => {
    renderWithProviders(<Profile />)
    const account = (await screen.findByText('Sam Rivera')).closest('section') as HTMLElement
    expect(within(account).getByText('sam@example.com')).toBeInTheDocument()
    expect(within(account).getByText('PCI-000123')).toBeInTheDocument()
  })
})
