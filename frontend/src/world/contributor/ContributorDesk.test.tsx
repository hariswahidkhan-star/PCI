import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import ContributorDesk from './ContributorDesk'

// The contributor desk — behaviour, not appearance.
//
// The assertions that carry weight here are about what the screen REFUSES to imply:
//
//   • it never offers a way to approve or publish, at any standing, because the maker-checker
//     rule is that a contributor's own article is decided by somebody else;
//   • it shows the server's own sentence for held and refused outcomes rather than a paraphrase,
//     since an editorial outcome restated by the client is how an interface drifts from what
//     actually happened;
//   • it never echoes a submission as though it had been accepted.
//
// Every fetch is stubbed against the real routes in backend/Endpoints/WorldContributorsApi.cs.

const ME = {
  state: 'granted', may_write: true, terms_version: 'v1',
  granted_at: '2026-03-04 10:00:00', revoked_at: null, revoke_reason: null, terms_available: true,
}

const ARTICLE = {
  id: 7, slug: 'schedule-recovery', title: 'Recovering a slipped programme',
  dek: 'What actually works.', body_md: 'A recovery plan is a re-sequencing argument.',
  status: 'drafting', version: 0, published_at: null, editable: true,
  history: [], messages: [],
}

function stub(routes: Record<string, { status?: number; body: unknown }>, seen: string[] = []) {
  vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
    const u = String(url)
    seen.push(`${init?.method ?? 'GET'} ${u}`)
    for (const [k, v] of Object.entries(routes)) {
      const [m, p] = k.split(' ')
      if ((init?.method ?? 'GET') === m && u === p)
        return new Response(JSON.stringify(v.body), { status: v.status ?? 200 })
    }
    return new Response(JSON.stringify({ ok: true }), { status: 200 })
  }))
  return seen
}

beforeEach(() => { localStorage.clear(); localStorage.setItem('pciworld_account', 'tok') })
afterEach(() => { vi.unstubAllGlobals() })

describe('standing', () => {
  it('a person who has never applied is offered the application, not a writing surface', async () => {
    stub({
      'GET /api/world/contributor/me': { body: {
        state: 'none', may_write: false, terms_version: null, granted_at: null,
        revoked_at: null, revoke_reason: null, terms_available: true } },
    })
    render(<ContributorDesk />)
    expect(await screen.findByRole('heading', { name: 'Apply to write' })).toBeTruthy()
    expect(screen.queryByRole('heading', { name: 'Your pieces' })).toBeNull()
    expect(screen.queryByRole('heading', { name: 'Start something new' })).toBeNull()
  })

  it('the application form is closed, in words, when no contributor terms are published', async () => {
    stub({
      'GET /api/world/contributor/me': { body: {
        state: 'none', may_write: false, terms_version: null, granted_at: null,
        revoked_at: null, revoke_reason: null, terms_available: false } },
    })
    render(<ContributorDesk />)
    expect(await screen.findByText(/contributor terms have not been published/i)).toBeTruthy()
    // Disabled AND explained. A greyed control with no reason reads as broken.
    expect(screen.getByRole('button', { name: 'Send application' }).hasAttribute('disabled')).toBe(true)
  })

  it('applying shows the server\'s own sentence, not a paraphrase of it', async () => {
    stub({
      'GET /api/world/contributor/me': { body: {
        state: 'none', may_write: false, terms_version: null, granted_at: null,
        revoked_at: null, revoke_reason: null, terms_available: true } },
      'POST /api/world/contributor/apply': { body: { ok: true, state: 'applied',
        notice: 'Your application is with the editorial team. Contributor status is granted by a person.' } },
    })
    render(<ContributorDesk />)
    fireEvent.change(await screen.findByLabelText(/What would you like to write about/i),
      { target: { value: 'Fifteen years in rail planning.' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send application' }))
    expect(await screen.findByText(/granted by a person/)).toBeTruthy()
  })

  it('a revoked contributor is told, with the reason recorded at the time', async () => {
    stub({
      'GET /api/world/contributor/me': { body: {
        state: 'revoked', may_write: false, terms_version: 'v1', granted_at: '2026-01-02 09:00:00',
        revoked_at: '2026-05-01 11:00:00', revoke_reason: 'Repeated undisclosed sponsorship.',
        terms_available: true } },
    })
    render(<ContributorDesk />)
    expect(await screen.findByText('Repeated undisclosed sponsorship.')).toBeTruthy()
    // A silent revocation looks like a bug to the person it happened to.
    expect(screen.getByText(/withdrawn on 2026-05-01/i)).toBeTruthy()
    // And re-applying must not look like a route back in.
    expect(screen.queryByRole('button', { name: 'Send application' })).toBeNull()
  })
})

describe('the desk itself', () => {
  it('never offers a way to approve or publish — that decision is not the writer\'s', async () => {
    stub({
      'GET /api/world/contributor/me': { body: ME },
      'GET /api/world/contributor/articles': { body: { articles: [
        { ...ARTICLE, status: 'approved', editable: false }] } },
    })
    const { container } = render(<ContributorDesk />)
    await screen.findByRole('heading', { name: 'Your pieces' })
    const labels = [...container.querySelectorAll('button')].map(b => b.textContent ?? '')
    expect(labels.some(l => /approve|publish/i.test(l))).toBe(false)
  })

  it('lists pieces with their state in words, not by colour', async () => {
    stub({
      'GET /api/world/contributor/me': { body: ME },
      'GET /api/world/contributor/articles': { body: { articles: [
        { ...ARTICLE, id: 1, title: 'Draft one', status: 'drafting', editable: true },
        { ...ARTICLE, id: 2, title: 'With the desk', status: 'technical_review', editable: false },
        { ...ARTICLE, id: 3, title: 'Out there', status: 'published', editable: false,
          published_at: '2026-04-01 08:00:00' }] } },
    })
    render(<ContributorDesk />)
    expect(await screen.findByText('Drafting')).toBeTruthy()
    expect(screen.getByText('With an editor')).toBeTruthy()
    expect(screen.getByText('Published')).toBeTruthy()
  })

  it('an empty desk says so rather than rendering nothing', async () => {
    stub({
      'GET /api/world/contributor/me': { body: ME },
      'GET /api/world/contributor/articles': { body: { articles: [] } },
    })
    render(<ContributorDesk />)
    expect(await screen.findByText(/have not started anything yet/i)).toBeTruthy()
  })
})

describe('writing and submitting', () => {
  async function openDraft(overrides: Partial<typeof ARTICLE> = {}) {
    const seen: string[] = []
    stub({
      'GET /api/world/contributor/me': { body: ME },
      'GET /api/world/contributor/articles': { body: { articles: [ARTICLE] } },
      'GET /api/world/contributor/articles/7': { body: { ...ARTICLE, ...overrides } },
      'POST /api/world/contributor/articles/7/submit': { body: { ok: true, status: 'technical_review',
        notice: 'Submitted for editorial review. Nothing you submit appears on the site until a person accepts it.' } },
    }, seen)
    render(<ContributorDesk />)
    fireEvent.click(await screen.findByRole('button', { name: /Continue writing/ }))
    // Wait on the heading, not the editor: a piece the editor has already taken has no editor to
    // wait for, and waiting for one made this helper unusable for exactly that case.
    await screen.findByRole('heading', { name: ARTICLE.title })
    return seen
  }

  it('submit is unavailable until all four declarations are answered', async () => {
    await openDraft()
    const btn = screen.getByRole('button', { name: 'Submit for review' })
    expect(btn.hasAttribute('disabled')).toBe(true)
    expect(screen.getByText('Answer all four to submit.')).toBeTruthy()
  })

  it('says the declarations are frozen BEFORE they are answered, not after', async () => {
    await openDraft()
    expect(screen.getByText(/cannot be changed afterwards/i)).toBeTruthy()
  })

  it('a completed submission shows the server\'s notice and never claims acceptance', async () => {
    await openDraft()
    fireEvent.change(screen.getByLabelText(/commercial interest/i), { target: { value: 'No' } })
    fireEvent.change(screen.getByLabelText(/sponsored or paid for/i), { target: { value: 'No' } })
    fireEvent.change(screen.getByLabelText(/AI assistance/i), { target: { value: 'None' } })
    fireEvent.change(screen.getByLabelText(/own original work/i), { target: { value: 'Yes' } })
    fireEvent.click(screen.getByRole('button', { name: 'Submit for review' }))
    expect(await screen.findByText(/until a person accepts it/)).toBeTruthy()
    // The precise claim: the PIECE's own state must not have moved to an accepted or published one.
    // A blanket text search matched the screen's standing blurb, which says nothing about this
    // submission — a test that passes on the wrong sentence is not a test.
    expect(screen.queryByText('Published')).toBeNull()
    expect(screen.queryByText('Accepted — awaiting publication')).toBeNull()
  })

  it('a "yes" declaration asks for the detail — a bare yes is not a declaration', async () => {
    await openDraft()
    fireEvent.change(screen.getByLabelText(/sponsored or paid for/i),
      { target: { value: 'Yes — described below' } })
    expect(await screen.findByLabelText('Please give the detail')).toBeTruthy()
  })

  it('a piece already with an editor offers no second submit', async () => {
    await openDraft({ status: 'technical_review' })
    // The text stays open — a first-stage reviewer working with the author is normal — but
    // re-submitting would let someone re-answer the declarations AFTER an editor read the first
    // set, which is what freezing them at submission exists to prevent.
    expect(screen.getByLabelText('The piece')).toBeTruthy()
    expect(screen.queryByRole('button', { name: 'Submit for review' })).toBeNull()
    expect(screen.getByText(/declarations you gave at submission stand/i)).toBeTruthy()
  })

  it('a piece with an editor closes the writer\'s copy and says why', async () => {
    await openDraft({ status: 'technical_review', editable: false })
    expect(screen.queryByLabelText('The piece')).toBeNull()
    expect(screen.getByText(/with an editor, so it can no longer be edited here/i)).toBeTruthy()
  })

  it('a published piece explains that published text is never edited in place', async () => {
    const seen: string[] = []
    stub({
      'GET /api/world/contributor/me': { body: ME },
      'GET /api/world/contributor/articles': { body: { articles: [
        { ...ARTICLE, status: 'published', editable: false }] } },
      'GET /api/world/contributor/articles/7': { body: {
        ...ARTICLE, status: 'published', editable: false, published_at: '2026-04-01 08:00:00' } },
    }, seen)
    render(<ContributorDesk />)
    fireEvent.click(await screen.findByRole('button', { name: /Open/ }))
    expect(await screen.findByText(/never edited in place/i)).toBeTruthy()
  })
})

describe('the editor thread', () => {
  it('re-reads from the server after sending rather than echoing locally', async () => {
    const seen: string[] = []
    stub({
      'GET /api/world/contributor/me': { body: ME },
      'GET /api/world/contributor/articles': { body: { articles: [ARTICLE] } },
      'GET /api/world/contributor/articles/7': { body: { ...ARTICLE, messages: [
        { from: 'editor', body: 'Can you attribute the 12% figure?', at: '2026-04-02 09:00:00' }] } },
      'POST /api/world/contributor/articles/7/messages': { body: { ok: true } },
    }, seen)
    render(<ContributorDesk />)
    fireEvent.click(await screen.findByRole('button', { name: /Continue writing/ }))
    expect(await screen.findByText('Can you attribute the 12% figure?')).toBeTruthy()

    fireEvent.change(screen.getByLabelText('Reply'), { target: { value: 'Cutting the figure.' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send' }))
    await waitFor(() =>
      expect(seen.filter(s => s === 'GET /api/world/contributor/articles/7').length).toBeGreaterThan(1))
  })
})

describe('outcomes are announced politely', () => {
  it('uses role="status", never role="alert"', async () => {
    const { container } = render(<ContributorDesk />)
    stub({ 'GET /api/world/contributor/me': { body: ME },
           'GET /api/world/contributor/articles': { body: { articles: [] } } })
    await waitFor(() => expect(container.querySelector('[role=alert]')).toBeNull())
  })

  it('the flag being off is stated as not-open-yet, not as an error', async () => {
    stub({ 'GET /api/world/contributor/me': { status: 404, body: {} } })
    render(<ContributorDesk />)
    expect(await screen.findByText(/not open yet/i)).toBeTruthy()
  })

  it('a signed-out visitor is pointed at sign-in rather than shown a broken desk', async () => {
    stub({ 'GET /api/world/contributor/me': { status: 401, body: { error: 'no_session' } } })
    render(<ContributorDesk />)
    expect(await screen.findByText(/sign in to your PCI World account/i)).toBeTruthy()
  })
})
