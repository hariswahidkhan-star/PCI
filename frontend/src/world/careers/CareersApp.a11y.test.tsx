import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen } from '@testing-library/react'
import axe from 'axe-core'
import EmployerPortal from './EmployerPortal'
import MyApplications from './MyApplications'

// PCI World careers surfaces — automated accessibility checks (WCAG 2.2 AA, structural rules).
//
// WHAT THIS COVERS AND WHAT IT DOES NOT — stated up front, because an a11y suite that implies
// more than it checks is worse than none.
//
// COVERED: the structural rules, which are the ones these interfaces most plausibly get wrong —
// every control has an accessible name, the many form fields are labelled and their help text
// associated, ARIA attributes are valid for their roles, headings and landmarks are ordered, and
// the disabled publish button still exposes its explanation.
//
// NOT COVERED: colour contrast, focus-visible appearance, reflow at 320px, and anything else
// needing real layout — jsdom has no rendering engine, so axe cannot evaluate them and silently
// skips them. Those need the browser pass the Phase 4 design's §9 test plan calls for, which
// stays a recorded gap rather than being implied by this file passing.
//
// The run fails on `critical` and `serious`, matching the community and forum suites.

const ACME_DRAFT = { id: 1, slug: 'acme', name: 'Acme Controls', state: 'draft', website: null, version: 1, role: 'owner', may_publish: false }
const ACME_VERIFIED = { ...ACME_DRAFT, state: 'verified', may_publish: true }

const DANA = { id: 11, state: 'submitted', submitted_at: '2026-07-01 10:00:00', applicant: 'Dana', has_cv: true }

const APP_ACTIVE = {
  id: 1, state: 'submitted', title: 'Senior planner', employer: 'Acme Controls',
  submitted_at: '2026-07-01 09:00:00', withdrawn_at: null, cv_name: 'dana-cv.pdf',
  consent: { granted_at: '2026-07-01 09:00:00', withdrawn_at: null, policy_version: 'v1', active: true },
}
const APP_CONSENT_WITHDRAWN = {
  id: 2, state: 'submitted', title: 'Cost engineer', employer: 'Beta Rail',
  submitted_at: '2026-06-20 10:00:00', withdrawn_at: null, cv_name: null,
  consent: { granted_at: '2026-06-20 10:00:00', withdrawn_at: '2026-07-02 12:00:00', policy_version: 'v1', active: false },
}

async function scan(container: HTMLElement) {
  const results = await axe.run(container, {
    // Only the rules jsdom can actually evaluate. Asking for the rest would produce "incomplete"
    // results that are easy to mistake for passes.
    runOnly: { type: 'tag', values: ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'] },
    rules: {
      'color-contrast': { enabled: false },   // needs layout; jsdom cannot evaluate it — browser-only
      'target-size': { enabled: false },      // needs layout; jsdom cannot evaluate it — browser-only
    },
  })
  const blocking = results.violations.filter(v => v.impact === 'critical' || v.impact === 'serious')
  return blocking.map(v => `${v.id} (${v.impact}) ×${v.nodes.length}: ${v.help}`)
}

describe('World careers surfaces — accessibility', () => {
  let responder: (url: string, method: string) => unknown

  beforeEach(() => {
    localStorage.clear()
    localStorage.setItem('pciworld_account', 'tok-account')
    responder = url => (url.endsWith('/careers/employers') ? { employers: [ACME_DRAFT] } : {})
    vi.stubGlobal('fetch', vi.fn(async (url: string, init?: RequestInit) => {
      const payload = responder(url, init?.method ?? 'GET')
      const status = (payload as { _status?: number })?._status ?? 200
      return { ok: status < 400, status, text: async () => JSON.stringify(payload) } as unknown as Response
    }))
  })
  afterEach(() => vi.unstubAllGlobals())

  it('the employer portal with an unverified tenant and a drafted posting has no blocking violations', async () => {
    // Scanned in the richest unverified state: the disabled publish button with its explanation,
    // the member forms, the whole posting form and a session-drafted row with its question form.
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/postings')) return { ok: true, posting_id: 7, slug: 'senior-planner', state: 'draft' }
      return url.endsWith('/careers/employers') ? { employers: [ACME_DRAFT] } : {}
    }
    const { container } = render(<EmployerPortal />)
    fireEvent.click(await screen.findByRole('button', { name: 'Act for Acme Controls' }))
    await screen.findByLabelText('Posting title')
    fireEvent.change(screen.getByLabelText('Posting title'), { target: { value: 'Senior planner' } })
    fireEvent.click(screen.getByRole('button', { name: 'Save draft posting' }))
    // Guard the fixture: if the row did not render, the scan below proves nothing about it.
    expect(await screen.findByRole('button', { name: 'Publish Senior planner' })).toBeDisabled()
    expect(await scan(container)).toEqual([])
  })

  it('the applicant review list with decision controls has no blocking violations', async () => {
    responder = url => {
      if (url.includes('/postings/7/applications')) return { applications: [DANA] }
      return url.endsWith('/careers/employers') ? { employers: [ACME_VERIFIED] } : {}
    }
    const { container } = render(<EmployerPortal />)
    fireEvent.click(await screen.findByRole('button', { name: 'Act for Acme Controls' }))
    fireEvent.change(await screen.findByLabelText('Posting id'), { target: { value: '7' } })
    fireEvent.click(screen.getByRole('button', { name: 'Load applicants' }))
    expect(await screen.findByRole('button', { name: 'Shortlist applicant 11' })).toBeTruthy()
    expect(await scan(container)).toEqual([])
  })

  it('the signed-out employer portal has no blocking violations', async () => {
    responder = () => ({ _status: 401, error: 'no_session' })
    const { container } = render(<EmployerPortal />)
    await screen.findByText(/sign in to your PCI World account/i)
    expect(await scan(container)).toEqual([])
  })

  it('my applications — including a withdrawn consent and the open consequence step — has no blocking violations', async () => {
    // The states a person most needs the semantics in: a consent record that survived its own
    // withdrawal, and the consequence box open before confirming a new withdrawal.
    responder = () => ({ applications: [APP_ACTIVE, APP_CONSENT_WITHDRAWN] })
    const { container } = render(<MyApplications />)
    await screen.findByText('Cost engineer')
    fireEvent.click(screen.getByRole('button', { name: 'Withdraw consent for application 1' }))
    expect(screen.getByRole('button', { name: 'Yes, withdraw consent' })).toBeTruthy()
    expect(await scan(container)).toEqual([])
  })

  it('my applications stays accessible while reporting the consent-withdrawal outcome', async () => {
    const NOTICE =
      'Consent withdrawn. The employer can no longer open this application or your CV. '
      + 'The record that you applied, and that you consented at the time, is kept — '
      + 'withdrawing consent is not the same as erasure.'
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/consent/withdraw')) return { ok: true, notice: NOTICE }
      return { applications: [APP_ACTIVE] }
    }
    const { container } = render(<MyApplications />)
    await screen.findByText('Senior planner')
    fireEvent.click(screen.getByRole('button', { name: 'Withdraw consent for application 1' }))
    fireEvent.click(screen.getByRole('button', { name: 'Yes, withdraw consent' }))
    // The only account of what happened is the status region — scanned in exactly that state.
    await screen.findByText(NOTICE)
    expect(await scan(container)).toEqual([])
  })
})
