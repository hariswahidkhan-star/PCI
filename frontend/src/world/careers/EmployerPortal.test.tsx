import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import EmployerPortal from './EmployerPortal'

// PCI World careers — the employer portal's contract.
//
// The properties that matter most are asserted first: an UNVERIFIED employer can draft everything
// and publish nothing, with the WHY in words rather than a bare grey button; a refused publish
// shows the SERVER'S notice verbatim; requesting verification never reads as being verified; and
// no outcome is ever echoed before the server confirms it. Tenancy is asserted too: switching
// organisations drops everything loaded for the previous one.

const NOTICE_QUEUED =
  'Your organisation is queued for review by a person. Verification checks who is behind a '
  + 'posting; it is not automatic and there is no guaranteed timescale.'
const NOTICE_NOT_VERIFIED =
  'Only a verified organisation can publish a job. Your draft is saved and stays private until '
  + 'verification completes.'

const ACME_DRAFT = { id: 1, slug: 'acme', name: 'Acme Controls', state: 'draft', website: null, version: 1, role: 'owner', may_publish: false }
const ACME_VERIFIED = { ...ACME_DRAFT, state: 'verified', may_publish: true }
const BETA = { id: 2, slug: 'beta', name: 'Beta Rail', state: 'draft', website: null, version: 1, role: 'member', may_publish: false }

const DANA = { id: 11, state: 'submitted', submitted_at: '2026-07-01 10:00:00', applicant: 'Dana', has_cv: true }

describe('World careers employer portal', () => {
  const calls: { url: string; method: string; headers: Record<string, string>; body: unknown }[] = []
  let responder: (url: string, method: string) => unknown

  beforeEach(() => {
    calls.length = 0
    localStorage.clear()
    // The account session the portal must ride (the same key the classic World pages use).
    localStorage.setItem('pciworld_account', 'tok-account')
    responder = url => (url.endsWith('/careers/employers') ? { employers: [ACME_DRAFT] } : {})
    vi.stubGlobal('fetch', vi.fn(async (url: string, init?: RequestInit) => {
      const method = init?.method ?? 'GET'
      calls.push({
        url, method,
        headers: (init?.headers ?? {}) as Record<string, string>,
        body: init?.body ? JSON.parse(String(init.body)) : undefined,
      })
      // A responder may return a Promise, so a test can hold the server's answer back and prove
      // nothing is echoed before it arrives.
      const payload = await responder(url, method)
      const status = (payload as { _status?: number })?._status ?? 200
      return { ok: status < 400, status, text: async () => JSON.stringify(payload) } as unknown as Response
    }))
  })
  afterEach(() => vi.unstubAllGlobals())

  async function openPortal(employers: unknown[]) {
    responder = url => (url.endsWith('/careers/employers') ? { employers } : {})
    render(<EmployerPortal />)
    await screen.findByRole('heading', { name: 'Create an organisation', level: 2 })
  }

  async function selectAcme(employer: { name: string }) {
    fireEvent.click(screen.getByRole('button', { name: `Act for ${employer.name}` }))
    await screen.findByRole('heading', { name: employer.name, level: 2 })
  }

  async function draftPosting(title = 'Senior planner') {
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/postings')) return { ok: true, posting_id: 7, slug: 'senior-planner', state: 'draft' }
      return url.endsWith('/careers/employers') ? { employers: [ACME_DRAFT] } : {}
    }
    fireEvent.change(screen.getByLabelText('Posting title'), { target: { value: title } })
    fireEvent.click(screen.getByRole('button', { name: 'Save draft posting' }))
    await screen.findByRole('button', { name: `Publish ${title}` })
  }

  // ── Session and identity ──

  it('rides the World account header, never a portal bearer', async () => {
    await openPortal([ACME_DRAFT])
    const listCall = calls.find(c => c.url.endsWith('/careers/employers'))
    expect(listCall?.headers['X-World-Account']).toBe('tok-account')
    expect(listCall?.headers['Authorization']).toBeUndefined()
  })

  it('tells a signed-out visitor to sign in, and offers no forms', async () => {
    responder = () => ({ _status: 401, error: 'no_session' })
    render(<EmployerPortal />)
    await screen.findByText(/sign in to your PCI World account/i)
    expect(screen.queryByLabelText('Organisation name')).toBeNull()
  })

  it('says the feature is not open when the flag is off (404)', async () => {
    responder = () => ({ _status: 404 })
    render(<EmployerPortal />)
    await screen.findByText('PCI World Careers is not open yet.')
    expect(screen.queryByLabelText('Organisation name')).toBeNull()
  })

  // ── Draft anything, publish nothing ──

  it('lets an unverified employer draft a posting and questions, but publish is unavailable WITH the why in words', async () => {
    await openPortal([ACME_DRAFT])
    await selectAcme(ACME_DRAFT)
    // The why is on screen before anyone drafts anything, from the server's may_publish flag.
    expect(screen.getAllByText(/Only a verified organisation can publish a job/i).length).toBeGreaterThan(0)

    await draftPosting()
    // Drafting worked — the row is there, in the server's state word — but publish is refused
    // up front, and the refusal explains itself beside the button.
    const publish = screen.getByRole('button', { name: 'Publish Senior planner' })
    expect(publish).toBeDisabled()
    expect(screen.getAllByText(/Only a verified organisation can publish a job/i).length).toBeGreaterThan(1)

    // Screening questions attach to the draft just fine.
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/questions')) return { ok: true, question_id: 3 }
      return url.endsWith('/careers/employers') ? { employers: [ACME_DRAFT] } : {}
    }
    fireEvent.change(screen.getByLabelText('Screening question for Senior planner'), { target: { value: 'Years of P6 experience?' } })
    fireEvent.click(screen.getByRole('button', { name: 'Add question to Senior planner' }))
    const status = await screen.findByText('Screening question added to the draft.')
    expect(status).toBeTruthy()
    const qCall = calls.find(c => c.url === '/api/world/careers/postings/7/questions')
    expect(qCall?.body).toEqual({ kind: 'text', prompt: 'Years of P6 experience?', required: false })
  })

  // ── Requesting verification is a request, not a verdict ──

  it('shows the server notice for a verification request verbatim, and never reads as verified', async () => {
    await openPortal([ACME_DRAFT])
    await selectAcme(ACME_DRAFT)
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/request-verification')) {
        return { ok: true, state: 'pending_verification', notice: NOTICE_QUEUED }
      }
      // The reload after the request now reports the pending state.
      return url.endsWith('/careers/employers')
        ? { employers: [{ ...ACME_DRAFT, state: 'pending_verification' }] }
        : {}
    }
    fireEvent.click(screen.getByRole('button', { name: 'Request verification' }))

    // The server's OWN words — "queued for review by a person", "not automatic". Paraphrasing a
    // pending review into anything warmer would claim a verification nobody made.
    const status = await screen.findByText(NOTICE_QUEUED)
    expect(status.getAttribute('role')).toBe('status')
    await screen.findByText(/awaiting verification — queued for review by a person/i)
    // Nothing anywhere says plain "verified": asking must not look like being verified.
    expect(screen.queryByText('verified')).toBeNull()
  })

  it('does not offer the verification request to a non-owner, and says why', async () => {
    await openPortal([{ ...ACME_DRAFT, role: 'member' }])
    await selectAcme(ACME_DRAFT)
    expect(screen.queryByRole('button', { name: 'Request verification' })).toBeNull()
    expect(screen.getByText('Only the organisation owner can request verification.')).toBeTruthy()
  })

  // ── Publish: the server's answer, in the server's words ──

  it('shows a refused publish in the SERVER notice, verbatim, and leaves the draft a draft', async () => {
    // may_publish arrived stale-true, so the button is live — the server still refuses, and its
    // words are what the person reads. The UI must not re-derive or soften them.
    await openPortal([ACME_VERIFIED])
    await selectAcme(ACME_VERIFIED)
    await draftPosting()
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/publish')) {
        return { _status: 403, error: 'employer_not_verified', notice: NOTICE_NOT_VERIFIED }
      }
      return url.endsWith('/careers/employers') ? { employers: [ACME_VERIFIED] } : {}
    }
    fireEvent.click(screen.getByRole('button', { name: 'Publish Senior planner' }))

    const status = await screen.findByText(NOTICE_NOT_VERIFIED)
    expect(status.getAttribute('role')).toBe('status')
    // The row still says what the server last confirmed: draft. No optimistic flip.
    expect(screen.getByText('draft')).toBeTruthy()
    expect(screen.queryByText('published')).toBeNull()
  })

  it('reports publish and close in the server state word only after the server confirms', async () => {
    await openPortal([ACME_VERIFIED])
    await selectAcme(ACME_VERIFIED)
    await draftPosting()
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/publish')) return { ok: true, state: 'published' }
      if (method === 'POST' && url.includes('/close')) return { ok: true, state: 'closed' }
      return url.endsWith('/careers/employers') ? { employers: [ACME_VERIFIED] } : {}
    }
    fireEvent.click(screen.getByRole('button', { name: 'Publish Senior planner' }))
    await screen.findByText('"Senior planner" is now published.')
    expect(calls.some(c => c.url === '/api/world/careers/postings/7/publish' && c.method === 'POST')).toBe(true)
    expect(screen.getByText('published')).toBeTruthy()

    fireEvent.click(screen.getByRole('button', { name: 'Close Senior planner' }))
    await screen.findByText(/"Senior planner" is now closed\./)
    expect(screen.getByText('closed')).toBeTruthy()
  })

  // ── Reviewing applicants ──

  it('lists a posting\'s applicants and records a decision through the server, re-reading the list', async () => {
    await openPortal([ACME_VERIFIED])
    await selectAcme(ACME_VERIFIED)
    let decided = false
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/decision')) { decided = true; return { ok: true, state: 'shortlisted' } }
      if (url.includes('/postings/7/applications')) {
        return { applications: [decided ? { ...DANA, state: 'shortlisted' } : DANA] }
      }
      return url.endsWith('/careers/employers') ? { employers: [ACME_VERIFIED] } : {}
    }
    fireEvent.change(screen.getByLabelText('Posting id'), { target: { value: '7' } })
    fireEvent.click(screen.getByRole('button', { name: 'Load applicants' }))
    await screen.findByText('Dana')
    expect(screen.getByText('submitted')).toBeTruthy()
    expect(screen.getByText(/CV attached/)).toBeTruthy()

    fireEvent.change(screen.getByLabelText('Note for the record (optional)'), { target: { value: 'strong references' } })
    fireEvent.click(screen.getByRole('button', { name: 'Shortlist applicant 11' }))
    await screen.findByText('Application 11 is now shortlisted.')

    const decision = calls.find(c => c.url === '/api/world/careers/applications/11/decision')
    expect(decision?.body).toEqual({ decision: 'shortlisted', note: 'strong references' })
    // The state on screen comes from a fresh server read, not a local edit of the row.
    expect(calls.filter(c => c.url === '/api/world/careers/postings/7/applications').length).toBe(2)
    expect(await screen.findByText('shortlisted')).toBeTruthy()
  })

  it('echoes nothing about a decision before the server confirms it', async () => {
    await openPortal([ACME_VERIFIED])
    await selectAcme(ACME_VERIFIED)
    let release: () => void = () => {}
    const gate = new Promise<void>(r => { release = r })
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/decision')) return gate.then(() => ({ ok: true, state: 'shortlisted' }))
      if (url.includes('/postings/7/applications')) return { applications: [DANA] }
      return url.endsWith('/careers/employers') ? { employers: [ACME_VERIFIED] } : {}
    }
    fireEvent.change(screen.getByLabelText('Posting id'), { target: { value: '7' } })
    fireEvent.click(screen.getByRole('button', { name: 'Load applicants' }))
    await screen.findByText('Dana')
    fireEvent.click(screen.getByRole('button', { name: 'Shortlist applicant 11' }))

    // The server has not answered: the row still says what the server last said, and there is no
    // "is now" claim anywhere. Optimistic UI is exactly what this asserts against.
    expect(screen.getByText('submitted')).toBeTruthy()
    expect(screen.queryByText(/is now/)).toBeNull()
    release()
    await screen.findByText('Application 11 is now shortlisted.')
  })

  it('never shows a candidate score, rank or match percentage — there is none', async () => {
    await openPortal([ACME_VERIFIED])
    await selectAcme(ACME_VERIFIED)
    responder = url => {
      if (url.includes('/postings/7/applications')) return { applications: [DANA, { id: 12, state: 'submitted', submitted_at: '2026-07-02 09:00:00', applicant: 'Elio', has_cv: false }] }
      return url.endsWith('/careers/employers') ? { employers: [ACME_VERIFIED] } : {}
    }
    fireEvent.change(screen.getByLabelText('Posting id'), { target: { value: '7' } })
    fireEvent.click(screen.getByRole('button', { name: 'Load applicants' }))
    await screen.findByText('Dana')
    // §10.7: no scoring code path exists on the server, and none may be invented here.
    expect(document.body.textContent).not.toMatch(/\bscore\b|\brank\b|\bmatch\b|%/i)
  })

  // ── Tenancy: nothing follows you between organisations ──

  it('drops the previous tenant\'s postings and applicants when switching organisations', async () => {
    await openPortal([ACME_VERIFIED, BETA])
    await selectAcme(ACME_VERIFIED)
    await draftPosting()
    responder = url => {
      if (url.includes('/postings/7/applications')) return { applications: [DANA] }
      return url.endsWith('/careers/employers') ? { employers: [ACME_VERIFIED, BETA] } : {}
    }
    fireEvent.change(screen.getByLabelText('Posting id'), { target: { value: '7' } })
    fireEvent.click(screen.getByRole('button', { name: 'Load applicants' }))
    await screen.findByText('Dana')

    fireEvent.click(screen.getByRole('button', { name: 'Act for Beta Rail' }))
    await screen.findByRole('heading', { name: 'Beta Rail', level: 2 })
    // The server scopes every list to the acting tenant; the client holds no cross-employer
    // cache, so nothing loaded as Acme survives the switch.
    expect(screen.queryByText('Dana')).toBeNull()
    expect(screen.queryByRole('button', { name: 'Publish Senior planner' })).toBeNull()
  })

  // ── Membership ──

  it('adds and removes members through the server and reports only its confirmation', async () => {
    await openPortal([ACME_DRAFT])
    await selectAcme(ACME_DRAFT)
    responder = (url, method) => {
      if (method === 'POST' && url.includes('/members')) return { ok: true }
      if (method === 'DELETE') return { ok: true }
      return url.endsWith('/careers/employers') ? { employers: [ACME_DRAFT] } : {}
    }
    fireEvent.change(screen.getByLabelText('Email of the PCI World account to add'), { target: { value: 'colleague@acme.test' } })
    fireEvent.click(screen.getByRole('button', { name: 'Add member' }))
    await screen.findByText('Member added.')
    const add = calls.find(c => c.method === 'POST' && c.url === '/api/world/careers/employers/1/members')
    expect(add?.body).toEqual({ email: 'colleague@acme.test', role: 'member' })

    fireEvent.change(screen.getByLabelText('Account id to remove'), { target: { value: '42' } })
    fireEvent.click(screen.getByRole('button', { name: 'Remove member' }))
    await screen.findByText('Member removed.')
    expect(calls.some(c => c.method === 'DELETE' && c.url === '/api/world/careers/employers/1/members/42')).toBe(true)
  })

  it('does not offer member management to a non-owner, and says why', async () => {
    await openPortal([{ ...ACME_DRAFT, role: 'member' }])
    await selectAcme(ACME_DRAFT)
    expect(screen.queryByLabelText('Email of the PCI World account to add')).toBeNull()
    expect(screen.getByText('Only the organisation owner can add or remove members.')).toBeTruthy()
  })

  // ── Outcomes are results, not emergencies ──

  it('announces outcomes via status, never alert', async () => {
    await openPortal([ACME_DRAFT])
    await selectAcme(ACME_DRAFT)
    await draftPosting()
    await waitFor(() => expect(screen.getByRole('status')).toBeTruthy())
    expect(screen.queryByRole('alert')).toBeNull()
  })
})
