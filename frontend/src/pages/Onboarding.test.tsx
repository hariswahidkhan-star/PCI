import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE — the first-run profile wizard. It is "save-as-you-go, every step optional", so the decisions
// worth pinning are the step navigation and the About-step save gate: the profile PATCH fires ONLY
// when a field actually changed (an untouched Save, and the Skip button, advance without any request),
// and a changed field patches just that key. The auth/me contexts, the data hook used by the nested
// RepeaterSections, and the fetch client are mocked at the module boundary; useNavigate is spied.
const h = vi.hoisted(() => ({
  me: { profile: {} as Record<string, unknown> } as Record<string, unknown>,
  refetch: vi.fn(),
  patch: vi.fn(),
  navigate: vi.fn(),
}))

vi.mock('../auth/AuthContext', () => ({ useAuth: () => ({ user: { firstName: 'Sam' } }) }))
vi.mock('../data/MeContext', () => ({ useMe: () => ({ me: h.me, refetch: h.refetch }) }))
vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: { rows: [] }, loading: false, error: null, refetch: vi.fn() }),
  runMutation: (fn: () => Promise<void>) => fn(),
}))
vi.mock('../api/client', () => ({
  api: { patch: (p: string, b?: unknown) => h.patch(p, b), post: vi.fn(), del: vi.fn() },
}))
vi.mock('react-router-dom', async (orig) => ({
  ...(await orig<typeof import('react-router-dom')>()),
  useNavigate: () => h.navigate,
}))

import Onboarding from './Onboarding'

// Every test starts on the welcome step; this advances to the "About you" step.
async function toAbout(user: ReturnType<typeof userEvent.setup>) {
  await user.click(screen.getByRole('button', { name: 'Let’s go' }))
}

describe('Onboarding (profile wizard)', () => {
  beforeEach(() => {
    h.me = { profile: {} }
    h.refetch.mockReset()
    h.patch.mockReset()
    h.navigate.mockReset()
  })

  it('greets the member by name and advances from welcome to the About step', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Onboarding />)
    expect(screen.getByText(/Welcome, Sam/)).toBeInTheDocument()
    await toAbout(user)
    expect(screen.getByRole('heading', { name: 'About you' })).toBeInTheDocument()
  })

  it('reflects the current step on the progress bar', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Onboarding />)
    expect(screen.getByRole('progressbar')).toHaveAttribute('aria-valuenow', '1')
    await toAbout(user)
    expect(screen.getByRole('progressbar')).toHaveAttribute('aria-valuenow', '2')
  })

  it('does NOT PATCH when the About step is saved unchanged, but still advances', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Onboarding />)
    await toAbout(user)
    await user.click(screen.getByRole('button', { name: 'Save & continue' }))
    expect(h.patch).not.toHaveBeenCalled()
    expect(screen.getByRole('heading', { name: 'Work experience' })).toBeInTheDocument()
  })

  it('PATCHes only the changed field when the About step is edited', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Onboarding />)
    await toAbout(user)
    await user.type(screen.getByLabelText('City'), 'Dubai')
    await user.click(screen.getByRole('button', { name: 'Save & continue' }))
    expect(h.patch).toHaveBeenCalledWith('/api/me/profile', { city: 'Dubai' })
    expect(screen.getByRole('heading', { name: 'Work experience' })).toBeInTheDocument()
  })

  it('skips the About step without any request', async () => {
    const user = userEvent.setup()
    renderWithProviders(<Onboarding />)
    await toAbout(user)
    await user.click(screen.getByRole('button', { name: 'Skip' }))
    expect(h.patch).not.toHaveBeenCalled()
    expect(screen.getByRole('heading', { name: 'Work experience' })).toBeInTheDocument()
  })

  it('prefills the country select from the existing profile', async () => {
    const user = userEvent.setup()
    h.me = { profile: { country: 'Australia' } }
    renderWithProviders(<Onboarding />)
    await toAbout(user)
    expect(screen.getByLabelText('Country')).toHaveValue('Australia')
  })
})
