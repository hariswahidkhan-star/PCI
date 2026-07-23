import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, within } from '@testing-library/react'
import { renderWithProviders } from '../test/utils'

// FE — the dashboard's real logic is three pure builders surfaced through the render: buildJourney
// (lifecycle → a 6-step candidate journey with done/current/blocked/todo states), buildChecklist
// (profile/exam signals → done flags), and the lapsed-credential exclusion for the "active
// credentials" stat. This drives those decisions via the DOM. useMe/useAuth are mocked at the module
// boundary; CountUp is stubbed to render its value directly (its mount animation starts at 0 and is
// non-deterministic under jsdom, which would make the stat assertions flaky).
const h = vi.hoisted(() => ({ me: null as unknown, refetch: vi.fn() }))

vi.mock('../auth/AuthContext', () => ({ useAuth: () => ({ user: { firstName: 'Sam' } }) }))
vi.mock('../data/MeContext', () => ({
  useMe: () => ({ me: h.me, loading: false, error: null, refetch: h.refetch }),
}))
vi.mock('../components/CountUp', () => ({ default: ({ value }: { value: number | string }) => <>{value}</> }))

import Overview from './Overview'

// A minimal `me` covering everything Overview (and the embedded ConsentsNotice) reads; `lc` overrides
// the lifecycle sub-fields, everything else is spread over the defaults.
function mkMe({ lc = {}, ...rest }: { lc?: Record<string, unknown>; [k: string]: unknown } = {}) {
  return {
    user: { registration_no: 'PCI-100', created_at: '2026-01-01T00:00:00Z' },
    lifecycle: {
      membership_status: 'inactive',
      candidate_status: '',
      blocking_items: [],
      exam_status: '',
      result_status: null,
      credential_status: '',
      next_step: 'activate_membership',
      ...lc,
    },
    credentials: [],
    cpd: { total: 0, target: 60 },
    exams: [],
    attempts: [],
    experiences: [],
    qualifications: [],
    profile: {},
    identity_document: null,
    consents: { outstanding: [] },
    ...rest,
  }
}

const statValue = (label: string) => {
  const stat = screen.getByText(label).closest('.stat') as HTMLElement
  return stat.querySelector('.n')?.textContent
}

describe('Overview (candidate dashboard)', () => {
  beforeEach(() => {
    h.me = null
    h.refetch.mockReset()
  })

  it('marks the membership step "current" for a brand-new member', () => {
    h.me = mkMe()
    renderWithProviders(<Overview />)
    const li = screen.getByText('Membership active').closest('li') as HTMLElement
    expect(within(li).getByText('In progress')).toBeInTheDocument()
  })

  it('marks the eligibility step "blocked" with the titleCased blocking items', () => {
    h.me = mkMe({
      lc: { membership_status: 'active', candidate_status: 'exam_fee_paid', blocking_items: ['identity_document'], next_step: 'complete_eligibility' },
    })
    renderWithProviders(<Overview />)
    const li = screen.getByText('Eligibility cleared').closest('li') as HTMLElement
    expect(within(li).getByText('Action needed')).toBeInTheDocument()
    expect(within(li).getByText('Identity Document')).toBeInTheDocument() // titleCase(blocking_item)
  })

  it('shows no in-progress/action-needed badges once fully credentialed', () => {
    h.me = mkMe({
      lc: {
        membership_status: 'active',
        candidate_status: 'exam_fee_paid',
        exam_status: 'submitted',
        result_status: 'pass',
        credential_status: 'active',
        next_step: 'maintain_credential',
      },
    })
    renderWithProviders(<Overview />)
    expect(screen.queryByText('In progress')).toBeNull()
    expect(screen.queryByText('Action needed')).toBeNull()
  })

  it('renders done checklist items as text and outstanding ones as links', () => {
    h.me = mkMe({ profile: { current_role: 'Planner' } }) // → "about you" done, experience still outstanding
    renderWithProviders(<Overview />)
    expect(screen.getByText('Tell us about yourself').closest('li')).toHaveClass('done')
    expect(screen.queryByRole('link', { name: 'Tell us about yourself' })).toBeNull()
    expect(screen.getByRole('link', { name: 'Add your work experience' })).toBeInTheDocument()
  })

  it('renders the next-step card with the CTA for the lifecycle next_step', () => {
    h.me = mkMe({
      lc: {
        membership_status: 'active',
        candidate_status: 'exam_fee_paid',
        exam_status: 'submitted',
        result_status: 'pass',
        credential_status: 'active',
        next_step: 'maintain_credential',
      },
    })
    renderWithProviders(<Overview />)
    expect(screen.getByRole('heading', { name: 'Maintain your credential' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Log CPD' })).toHaveAttribute('href', '/cpd')
  })

  it('counts only active, non-expired credentials in the active-credentials stat', () => {
    h.me = mkMe({
      credentials: [
        { status: 'active', expires_at: '2099-01-01T00:00:00Z' }, // counts
        { status: 'active', expires_at: '2020-01-01T00:00:00Z' }, // lapsed — excluded
        { status: 'revoked', expires_at: '2099-01-01T00:00:00Z' }, // not active — excluded
      ],
    })
    renderWithProviders(<Overview />)
    expect(statValue('Active credentials')).toBe('1')
  })
})
