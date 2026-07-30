import { describe, it, expect, vi } from 'vitest'
import { screen } from '@testing-library/react'
import { renderWithProviders } from '../test/utils'
import { studentMe } from './fixtures'
import { demoGet } from './index'

/* The contract test that matters most for demonstration mode: the fixtures must actually drive
 * the REAL screens.
 *
 * A fixture that merely typechecks is not enough — the `Me` envelope has optional fields that the
 * type permits to be absent but a component reads unconditionally, so a missing array surfaces as
 * a blank "Something went wrong" boundary in the browser and nowhere else. Mounting the genuine
 * component against the genuine fixture is the only thing that catches that, and it catches it in
 * milliseconds instead of a build-deploy-click cycle. */

/* Route the api client through the demo resolver, exactly as demo mode does in the browser, so a
 * component that fetches its own data is checked against the answer it will really receive. */
vi.mock('../api/client', async () => {
  const d = await import('./index')
  return {
    api: {
      get: (p: string) => Promise.resolve(d.demoGet(p)),
      post: (p: string, b?: unknown) => Promise.resolve(d.demoMutate(p, b)),
      patch: (p: string, b?: unknown) => Promise.resolve(d.demoMutate(p, b)),
      del: (p: string) => Promise.resolve(d.demoMutate(p)),
    },
    getToken: () => 'demo-token',
  }
})

vi.mock('../auth/AuthContext', () => ({ useAuth: () => ({ user: { firstName: 'Amara', email: 'a@b.c' } }) }))
vi.mock('../data/MeContext', () => ({
  useMe: () => ({ me: studentMe(), loading: false, error: null, refetch: vi.fn() }),
}))
vi.mock('../components/CountUp', () => ({ default: ({ value }: { value: unknown }) => <>{String(value)}</> }))

import Overview from '../pages/Overview'
import WorldPassportSection from '../pages/WorldPassportSection'

describe('demo fixtures drive the real student portal', () => {
  it('renders the dashboard without a missing-field crash', () => {
    renderWithProviders(<Overview />)
    // the welcome line proves the page rendered rather than the error boundary
    expect(screen.getByRole('heading', { level: 1 })).toBeInTheDocument()
    expect(screen.getByText('Active credentials')).toBeInTheDocument()
  })

  it('shows a mid-journey candidate, not an empty account', () => {
    renderWithProviders(<Overview />)
    // an empty demo account would show none of the product
    expect(screen.getByText('Unread updates')).toBeInTheDocument()
    expect(screen.getByText(/of 6 complete/)).toBeInTheDocument()
  })

  it('supplies every array the Me envelope declares', () => {
    // A component that maps or reads .length on an absent array throws at render, and the type
    // cannot catch it because these are optional. Assert the arrays exist as arrays.
    const me = studentMe() as unknown as Record<string, unknown>
    for (const key of ['credentials', 'exams', 'attempts', 'tickets', 'experiences',
      'qualifications', 'certifications_held', 'honorary', 'payments']) {
      expect(Array.isArray(me[key]), `${key} must be an array`).toBe(true)
    }
    for (const key of ['user', 'lifecycle', 'cpd', 'consents', 'profile', 'membership']) {
      expect(me[key], `${key} must be present`).toBeTruthy()
    }
  })

  it('drives the Passport panel, which reads its arrays unconditionally', async () => {
    // The generic `{ ok, rows }` answer took the entire portal down here: PassportCard reads
    // `evidenceHighlights.length` with no guard, so an unrecognised shape throws inside the
    // dashboard tree and the error boundary replaces every screen. Mount the real panel.
    renderWithProviders(<WorldPassportSection />)
    expect(await screen.findByText('Amara Okafor')).toBeInTheDocument()
    expect(screen.getByText(/Recovering a slipped critical path/)).toBeInTheDocument()
  })

  it('routes the notification list through the resolver with usable actions', () => {
    const r = demoGet('/api/me/messages') as { rows: Record<string, unknown>[] }
    expect(r.rows.length).toBeGreaterThan(0)
    // at least one carries the cta the inbox renders, and every route is in-portal
    const withCta = r.rows.filter((m) => m.cta_route)
    expect(withCta.length).toBeGreaterThan(0)
    for (const m of withCta) expect(String(m.cta_route).startsWith('/')).toBe(true)
  })
})
