import { useCallback, useEffect, useState } from 'react'
import * as C from './api'

// PCI World careers — the EMPLOYER portal (docs/pciworld/CCP_PHASE4_DESIGN.md §4, §5, §7).
//
// PUBLIC READING IS DELIBERATELY NOT HERE: job pages, the careers list and employer profiles are
// server-rendered HTML with JSON-LD. This surface is the tenant's private side only — creating the
// organisation, membership, drafting, publish/close, and reviewing applicants.
//
// THE PHASE'S RULE SHOWS UP IN THE UI: an unverified employer can draft everything and publish
// nothing. The server says so twice — `may_publish` on the employer list, and a `notice` on a
// refused publish — and this portal repeats those words rather than re-deriving the rule from
// `state`. Requesting verification is rendered as exactly that, a request, in the server's own
// notice; it must never look like being verified.
//
// NOTHING IS EVER ECHOED THAT THE SERVER DID NOT CONFIRM. Rows appear only from server responses,
// state words come from the server's reply, and the applicant list is re-read after a decision
// rather than edited locally. The client also keeps NO cross-employer cache: the server scopes
// every list to the acting tenant, and switching organisations here drops everything loaded for
// the previous one.
//
// DELIBERATELY ABSENT: any candidate score, rank or match percentage. §10.7 prohibits automated
// ranking and the backend has no such field — there is nothing to display and nothing may be
// invented client-side.
//
// Outcomes land in a role="status" region (results, not emergencies), and every state is carried
// by TEXT — the notice colours only reinforce sentences that already say everything.

type Notice = { tone: 'info' | 'held' | 'error'; text: string } | null

/** Human gloss for employer states — the raw state word is shown alongside elsewhere. The
 *  pending gloss is careful to describe a queue, not an outcome. */
const EMPLOYER_STATE: Record<string, string> = {
  draft: 'draft — not yet submitted for verification',
  pending_verification: 'awaiting verification — queued for review by a person',
  verified: 'verified',
  suspended: 'suspended — public postings and applicant access are switched off',
}

/** A posting created in THIS session, pinned to the employer it belongs to. Rows are added only
 *  from a server confirmation and rendered filtered by the selected employer. */
interface PostingRow { id: number; employerId: number; slug: string; title: string; state: string }

export default function EmployerPortal() {
  const [employers, setEmployers] = useState<C.Employer[]>([])
  const [loaded, setLoaded] = useState(false)
  const [selectedId, setSelectedId] = useState<number | null>(null)
  const [signedOut, setSignedOut] = useState(false)
  const [unavailable, setUnavailable] = useState(false)
  const [notice, setNotice] = useState<Notice>(null)
  // There is no server endpoint that lists an employer's existing postings, so only postings
  // drafted in this session can be shown — stated in the UI rather than papered over.
  const [postings, setPostings] = useState<PostingRow[]>([])
  const [review, setReview] = useState<{ postingId: number; rows: C.PostingApplicant[] } | null>(null)

  const load = useCallback(async () => {
    try {
      setEmployers((await C.listEmployers()).employers)
      setLoaded(true)
    } catch (e) {
      if (e instanceof C.CareersError && e.status === 401) setSignedOut(true)
      else if (e instanceof C.CareersError && e.status === 404) setUnavailable(true)
      else setNotice({ tone: 'error', text: 'The employer portal is unavailable right now. Please try again shortly.' })
    }
  }, [])

  useEffect(() => { void load() }, [load])

  const selected = employers.find(e => e.id === selectedId) ?? null

  function select(id: number) {
    setSelectedId(id)
    // No cross-employer cache: the applicant view loaded for the previous tenant is dropped
    // outright, the session-drafted postings render filtered by the selected employer, and a
    // notice about the previous tenant's data does not follow into the next one.
    setReview(null)
    setNotice(null)
  }

  async function loadReview(postingId: number) {
    setNotice(null)
    try {
      const r = await C.listApplicants(postingId)
      setReview({ postingId, rows: r.applications })
    } catch (e) {
      setReview(null)
      setNotice({ tone: 'error', text: C.errText(e, 'Those applications could not be loaded.') })
    }
  }

  return (
    <div className="wc">
      <header className="wc-head"><h1>Employer portal</h1></header>

      {notice && (
        // role="status" rather than "alert": these are outcomes, not emergencies. The tone class
        // is decoration on top of words that already carry the whole meaning.
        <p role="status" className={`wc-notice wc-notice-${notice.tone}`}>{notice.text}</p>
      )}

      {!loaded && !signedOut && !unavailable && <p role="status">Loading…</p>}

      {signedOut && (
        <p className="wc-signin">
          To act for an organisation, please <a href="/world/">sign in to your PCI World account</a>.
        </p>
      )}
      {unavailable && <p className="wc-signin">PCI World Careers is not open yet.</p>}

      {loaded && (
        <>
          <EmployerPicker employers={employers} selectedId={selectedId} onSelect={select} />

          {selected && (
            <>
              <EmployerStatus employer={selected} onNotice={setNotice} reload={load} />
              {selected.role === 'owner'
                ? <Members employer={selected} onNotice={setNotice} />
                : <p className="wc-help">Only the organisation owner can add or remove members.</p>}
              <NewPosting
                employer={selected}
                onNotice={setNotice}
                onCreated={p => setPostings(prev => [...prev, p])}
              />
              <Postings
                employer={selected}
                postings={postings.filter(p => p.employerId === selected.id)}
                onNotice={setNotice}
                onState={(id, state) => setPostings(prev => prev.map(p => (p.id === id ? { ...p, state } : p)))}
                onReview={id => { void loadReview(id) }}
              />
              <Review review={review} onLoad={id => { void loadReview(id) }} />
            </>
          )}

          <CreateEmployer onNotice={setNotice} onCreated={load} />
        </>
      )}
    </div>
  )
}

// ── Choosing which tenant to act for ────────────────────────────────────────────────────────

function EmployerPicker({ employers, selectedId, onSelect }: {
  employers: C.Employer[]; selectedId: number | null; onSelect: (id: number) => void
}) {
  if (employers.length === 0) {
    return <p>You are not a member of any organisation yet. You can create one below.</p>
  }
  return (
    <section aria-labelledby="wc-orgs-h" className="wc-section">
      <h2 id="wc-orgs-h">Your organisations</h2>
      <ul className="wc-rows">
        {employers.map(e => (
          <li key={e.id} className="wc-row">
            <button
              type="button" className="secondary"
              aria-pressed={e.id === selectedId}
              onClick={() => onSelect(e.id)}
            >
              {e.id === selectedId ? `Working as ${e.name}` : `Act for ${e.name}`}
            </button>{' '}
            <span>state: <span className="wc-state">{e.state}</span> · your role: {e.role}</span>
          </li>
        ))}
      </ul>
    </section>
  )
}

// ── The tenant's standing, and asking for verification ──────────────────────────────────────

function EmployerStatus({ employer, onNotice, reload }: {
  employer: C.Employer; onNotice: (n: Notice) => void; reload: () => Promise<void>
}) {
  const [busy, setBusy] = useState(false)

  async function request() {
    if (busy) return
    setBusy(true); onNotice(null)
    try {
      const r = await C.requestVerification(employer.id)
      // The server's own words, verbatim, in the held tone: its notice is careful to say a PERSON
      // reviews the request, with no guaranteed timescale. Softening that into anything that
      // reads as "verified" would be a claim the server never made.
      onNotice({ tone: 'held', text: r.notice })
      await reload()
    } catch (e) {
      onNotice({ tone: 'error', text: C.errText(e, 'Verification could not be requested.') })
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="wc-status-h" className="wc-section">
      <h2 id="wc-status-h">{employer.name}</h2>
      <p>Status: <span className="wc-state">{EMPLOYER_STATE[employer.state] ?? employer.state}</span></p>
      {!employer.may_publish && (
        // The WHY, in words, from the server's may_publish flag — not re-derived from `state`.
        // A greyed publish button with no explanation reads as broken.
        <p className="wc-help">
          Only a verified organisation can publish a job. Drafting works now; every draft stays
          private until verification completes.
        </p>
      )}
      {employer.state === 'draft' && (
        employer.role === 'owner' ? (
          <button type="button" onClick={() => { void request() }} disabled={busy}>
            {busy ? 'Requesting…' : 'Request verification'}
          </button>
        ) : (
          <p className="wc-help">Only the organisation owner can request verification.</p>
        )
      )}
    </section>
  )
}

// ── Membership: acting rights are rows, not a shared password ───────────────────────────────

function Members({ employer, onNotice }: { employer: C.Employer; onNotice: (n: Notice) => void }) {
  const [email, setEmail] = useState('')
  const [role, setRole] = useState('member')
  const [removeId, setRemoveId] = useState('')
  const [busy, setBusy] = useState(false)

  async function add(e: React.FormEvent) {
    e.preventDefault()
    if (busy || email.trim().length === 0) return
    setBusy(true); onNotice(null)
    try {
      await C.addMember(employer.id, email.trim(), role)
      // The server sends only ok:true here — so "added" is all that may be claimed.
      onNotice({ tone: 'info', text: 'Member added.' })
      setEmail('')
    } catch (err) {
      onNotice({ tone: 'error', text: C.errText(err, 'That member could not be added.') })
    } finally { setBusy(false) }
  }

  async function remove(e: React.FormEvent) {
    e.preventDefault()
    const id = Number(removeId.trim())
    if (busy || !Number.isInteger(id) || id <= 0) return
    setBusy(true); onNotice(null)
    try {
      await C.removeMember(employer.id, id)
      onNotice({ tone: 'info', text: 'Member removed.' })
      setRemoveId('')
    } catch (err) {
      onNotice({ tone: 'error', text: C.errText(err, 'That member could not be removed.') })
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="wc-members-h" className="wc-section">
      <h2 id="wc-members-h">Members</h2>
      <p className="wc-help">
        Acting rights are per-person memberships, never a shared password. There is no member-list
        endpoint yet, so this section can add and remove members but not show who they are;
        removing needs the person&rsquo;s account id.
      </p>
      <form onSubmit={e => { void add(e) }} noValidate>
        <label htmlFor="wc-m-email">Email of the PCI World account to add</label>
        <input id="wc-m-email" type="email" value={email} onChange={e => setEmail(e.target.value)} />
        <label htmlFor="wc-m-role">Role</label>
        <select id="wc-m-role" value={role} onChange={e => setRole(e.target.value)}>
          <option value="member">member</option>
          <option value="owner">owner</option>
        </select>
        <p><button type="submit" disabled={busy || email.trim().length === 0}>Add member</button></p>
      </form>
      <form onSubmit={e => { void remove(e) }} noValidate>
        <label htmlFor="wc-m-remove">Account id to remove</label>
        <input id="wc-m-remove" inputMode="numeric" value={removeId} onChange={e => setRemoveId(e.target.value)} />
        <p><button type="submit" className="secondary" disabled={busy || removeId.trim().length === 0}>Remove member</button></p>
      </form>
    </section>
  )
}

// ── Drafting a posting: the working copy, private until published ───────────────────────────

/** '' → undefined; digits → the number; anything else → null (refused before the request). */
function minorUnits(s: string): number | undefined | null {
  const t = s.trim()
  if (!t) return undefined
  return /^\d+$/.test(t) ? Number(t) : null
}

function NewPosting({ employer, onNotice, onCreated }: {
  employer: C.Employer; onNotice: (n: Notice) => void; onCreated: (p: PostingRow) => void
}) {
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [location, setLocation] = useState('')
  const [employmentType, setEmploymentType] = useState('')
  const [salaryMin, setSalaryMin] = useState('')
  const [salaryMax, setSalaryMax] = useState('')
  const [currency, setCurrency] = useState('')
  const [closesAt, setClosesAt] = useState('')
  const [busy, setBusy] = useState(false)

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    if (busy || title.trim().length < 4) return
    const min = minorUnits(salaryMin)
    const max = minorUnits(salaryMax)
    if (min === null || max === null) {
      onNotice({ tone: 'error', text: 'Salary must be a whole number of minor units (for example pence), not a decimal.' })
      return
    }
    setBusy(true); onNotice(null)
    try {
      const r = await C.createPosting(employer.id, {
        title: title.trim(),
        description: description.trim() || undefined,
        location: location.trim() || undefined,
        employment_type: employmentType.trim() || undefined,
        salary_min_minor: min,
        salary_max_minor: max,
        currency: currency.trim() || undefined,
        closes_at: closesAt.trim() || undefined,
      })
      // The row appears only now, from the server's confirmation, in the server's state word.
      onCreated({ id: r.posting_id, employerId: employer.id, slug: r.slug, title: title.trim(), state: r.state })
      onNotice({ tone: 'info', text: `Saved. "${title.trim()}" is a private ${r.state} until it is published.` })
      setTitle(''); setDescription(''); setLocation(''); setEmploymentType('')
      setSalaryMin(''); setSalaryMax(''); setCurrency(''); setClosesAt('')
    } catch (err) {
      // The draft stays in the form: a refusal must not also cost what was typed.
      onNotice({ tone: 'error', text: C.errText(err, 'The posting could not be saved.') })
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="wc-newpost-h" className="wc-section">
      <h2 id="wc-newpost-h">Draft a posting</h2>
      <form onSubmit={e => { void submit(e) }} noValidate>
        <label htmlFor="wc-p-title">Posting title</label>
        <input id="wc-p-title" value={title} onChange={e => setTitle(e.target.value)} aria-describedby="wc-p-title-help" />
        <p id="wc-p-title-help" className="wc-help">At least 4 characters. Drafts are private to your organisation.</p>

        <label htmlFor="wc-p-desc">Description (optional)</label>
        <textarea id="wc-p-desc" rows={5} value={description} onChange={e => setDescription(e.target.value)} />

        <label htmlFor="wc-p-loc">Location (optional)</label>
        <input id="wc-p-loc" value={location} onChange={e => setLocation(e.target.value)} />

        <label htmlFor="wc-p-type">Employment type (optional)</label>
        <input id="wc-p-type" value={employmentType} onChange={e => setEmploymentType(e.target.value)} />

        <label htmlFor="wc-p-smin">Salary minimum, in minor units (optional)</label>
        <input id="wc-p-smin" inputMode="numeric" value={salaryMin} onChange={e => setSalaryMin(e.target.value)} aria-describedby="wc-p-salary-help" />
        <label htmlFor="wc-p-smax">Salary maximum, in minor units (optional)</label>
        <input id="wc-p-smax" inputMode="numeric" value={salaryMax} onChange={e => setSalaryMax(e.target.value)} aria-describedby="wc-p-salary-help" />
        <label htmlFor="wc-p-cur">Salary currency (needed with any salary)</label>
        <input id="wc-p-cur" maxLength={3} value={currency} onChange={e => setCurrency(e.target.value)} aria-describedby="wc-p-salary-help" />
        <p id="wc-p-salary-help" className="wc-help">
          Whole numbers of minor units — 5000000 pence, not £50,000.00 — with the currency beside
          them. An amount without its currency is refused.
        </p>

        <label htmlFor="wc-p-closes">Closing date, UTC (optional, YYYY-MM-DD HH:MM:SS)</label>
        <input id="wc-p-closes" value={closesAt} onChange={e => setClosesAt(e.target.value)} />

        <p><button type="submit" disabled={busy || title.trim().length < 4}>
          {busy ? 'Saving…' : 'Save draft posting'}
        </button></p>
      </form>
    </section>
  )
}

// ── Postings drafted this session: questions, publish, close ────────────────────────────────

function Postings({ employer, postings, onNotice, onState, onReview }: {
  employer: C.Employer
  postings: PostingRow[]
  onNotice: (n: Notice) => void
  onState: (id: number, state: string) => void
  onReview: (postingId: number) => void
}) {
  return (
    <section aria-labelledby="wc-postings-h" className="wc-section">
      <h2 id="wc-postings-h">Postings drafted this session</h2>
      <p className="wc-help">
        There is no endpoint yet that lists an organisation&rsquo;s existing postings, so only
        postings created in this session appear here. Earlier postings still exist on the server —
        their applicants can be reviewed by posting id below.
      </p>
      {postings.length === 0
        ? <p>No postings drafted in this session.</p>
        : (
          <ul className="wc-rows">
            {postings.map(p => (
              <PostingTools
                key={p.id} employer={employer} posting={p}
                onNotice={onNotice} onState={onState} onReview={onReview}
              />
            ))}
          </ul>
        )}
    </section>
  )
}

function PostingTools({ employer, posting, onNotice, onState, onReview }: {
  employer: C.Employer
  posting: PostingRow
  onNotice: (n: Notice) => void
  onState: (id: number, state: string) => void
  onReview: (postingId: number) => void
}) {
  const [busy, setBusy] = useState(false)
  const [kind, setKind] = useState('text')
  const [prompt, setPrompt] = useState('')
  const [required, setRequired] = useState(false)

  const whyId = employer.may_publish ? undefined : `wc-whynot-${posting.id}`

  async function publish() {
    if (busy) return
    setBusy(true); onNotice(null)
    try {
      const r = await C.publishPosting(posting.id)
      // The server's word for the new state — never assumed from the click.
      onState(posting.id, r.state)
      onNotice({ tone: 'info', text: `"${posting.title}" is now ${r.state}.` })
    } catch (e) {
      // On a refusal the server's own notice ("Only a verified organisation can publish a job…")
      // is shown verbatim via errText; the posting stays whatever the server says it is.
      onNotice({ tone: 'error', text: C.errText(e, 'The posting could not be published.') })
    } finally { setBusy(false) }
  }

  async function close() {
    if (busy) return
    setBusy(true); onNotice(null)
    try {
      const r = await C.closePosting(posting.id)
      onState(posting.id, r.state)
      onNotice({ tone: 'info', text: `"${posting.title}" is now ${r.state}. Its page stays reachable but says it is no longer accepting applications.` })
    } catch (e) {
      onNotice({ tone: 'error', text: C.errText(e, 'The posting could not be closed.') })
    } finally { setBusy(false) }
  }

  async function addQuestion(e: React.FormEvent) {
    e.preventDefault()
    if (busy || prompt.trim().length === 0) return
    setBusy(true); onNotice(null)
    try {
      await C.addQuestion(posting.id, { kind, prompt: prompt.trim(), required })
      onNotice({ tone: 'info', text: 'Screening question added to the draft.' })
      setPrompt(''); setRequired(false)
    } catch (err) {
      onNotice({ tone: 'error', text: C.errText(err, 'The question could not be added.') })
    } finally { setBusy(false) }
  }

  return (
    <li className="wc-row">
      <p><b>{posting.title}</b> — state: <span className="wc-state">{posting.state}</span> <small>({posting.slug})</small></p>

      <div className="wc-actions">
        <button
          type="button"
          onClick={() => { void publish() }}
          disabled={busy || !employer.may_publish || posting.state !== 'draft'}
          aria-describedby={whyId}
        >
          Publish {posting.title}
        </button>
        <button
          type="button" className="secondary"
          onClick={() => { void close() }}
          disabled={busy || posting.state !== 'published'}
        >
          Close {posting.title}
        </button>
        <button type="button" className="secondary" onClick={() => onReview(posting.id)} disabled={busy}>
          Review applicants for {posting.title}
        </button>
      </div>
      {whyId && (
        // The WHY beside the greyed control, driven by the server's may_publish — a disabled
        // button with no explanation reads as broken.
        <p id={whyId} className="wc-help">
          Only a verified organisation can publish a job. This draft stays private until
          verification completes.
        </p>
      )}

      <form onSubmit={e => { void addQuestion(e) }} noValidate>
        <label htmlFor={`wc-q-${posting.id}`}>Screening question for {posting.title}</label>
        <input id={`wc-q-${posting.id}`} value={prompt} onChange={e => setPrompt(e.target.value)} aria-describedby={`wc-q-help-${posting.id}`} />
        <label htmlFor={`wc-qk-${posting.id}`}>Question type</label>
        <select id={`wc-qk-${posting.id}`} value={kind} onChange={e => setKind(e.target.value)}>
          <option value="text">text</option>
          <option value="boolean">yes / no</option>
          <option value="choice">choice</option>
        </select>
        <label>
          <input type="checkbox" checked={required} onChange={e => setRequired(e.target.checked)} />
          {' '}An answer is required to apply
        </label>
        <p id={`wc-q-help-${posting.id}`} className="wc-help">
          Applicants&rsquo; answers are frozen with the question&rsquo;s wording at the moment they
          apply — editing the question later cannot change what anyone was asked.
        </p>
        <p><button type="submit" className="secondary" disabled={busy || prompt.trim().length === 0}>
          Add question to {posting.title}
        </button></p>
      </form>
    </li>
  )
}

// ── Reviewing applicants ────────────────────────────────────────────────────────────────────

function Review({ review, onLoad }: {
  review: { postingId: number; rows: C.PostingApplicant[] } | null
  onLoad: (postingId: number) => void
}) {
  const [postingId, setPostingId] = useState('')

  function load(e: React.FormEvent) {
    e.preventDefault()
    const id = Number(postingId.trim())
    if (!Number.isInteger(id) || id <= 0) return
    onLoad(id)
  }

  return (
    <section aria-labelledby="wc-review-h" className="wc-section">
      <h2 id="wc-review-h">Applicants</h2>
      <form onSubmit={load} noValidate>
        <label htmlFor="wc-review-id">Posting id</label>
        <input id="wc-review-id" inputMode="numeric" value={postingId} onChange={e => setPostingId(e.target.value)} aria-describedby="wc-review-help" />
        <p id="wc-review-help" className="wc-help">
          Applicants appear in the order they applied. Nothing here reorders them for you.
        </p>
        <p><button type="submit" className="secondary" disabled={postingId.trim().length === 0}>Load applicants</button></p>
      </form>

      {review && (
        review.rows.length === 0
          ? <p>No applications are available for posting {review.postingId} — none exist yet, or their consent has been withdrawn.</p>
          : (
            <ul className="wc-rows">
              {review.rows.map(a => (
                <ApplicantRow key={a.id} app={a} onLoad={() => onLoad(review.postingId)} />
              ))}
            </ul>
          )
      )}
    </section>
  )
}

function ApplicantRow({ app, onLoad }: { app: C.PostingApplicant; onLoad: () => void }) {
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [confirmed, setConfirmed] = useState<string | null>(null)

  async function decideAs(decision: 'shortlisted' | 'rejected') {
    if (busy) return
    setBusy(true); setErr(null); setConfirmed(null)
    try {
      const r = await C.decide(app.id, decision, note.trim() || undefined)
      // Confirmed in the server's own word for the new state, then the list is re-read from the
      // server — the rows on screen are never a local guess about what the decision did.
      setConfirmed(`Application ${app.id} is now ${r.state}.`)
      setNote('')
      onLoad()
    } catch (e) {
      setErr(C.errText(e, 'That decision could not be recorded.'))
    } finally { setBusy(false) }
  }

  return (
    <li className="wc-row">
      <p>
        <b>{app.applicant ?? 'Unnamed applicant'}</b> — status: <span className="wc-state">{app.state}</span>
        {app.submitted_at && <> · applied {app.submitted_at} UTC</>}
        {' · '}{app.has_cv ? 'CV attached' : 'no CV'}
      </p>
      {(confirmed ?? err) && (
        <p role="status" className={`wc-notice wc-notice-${err ? 'error' : 'info'}`}>{err ?? confirmed}</p>
      )}
      <label htmlFor={`wc-note-${app.id}`}>Note for the record (optional)</label>
      <input id={`wc-note-${app.id}`} value={note} onChange={e => setNote(e.target.value)} aria-describedby={`wc-note-help-${app.id}`} />
      <p id={`wc-note-help-${app.id}`} className="wc-help">
        Notes are kept as events: each is recorded with the decision, and none overwrites an
        earlier one.
      </p>
      <div className="wc-actions">
        <button type="button" onClick={() => { void decideAs('shortlisted') }} disabled={busy}>
          Shortlist applicant {app.id}
        </button>
        <button type="button" className="secondary" onClick={() => { void decideAs('rejected') }} disabled={busy}>
          Reject applicant {app.id}
        </button>
      </div>
    </li>
  )
}

// ── Creating a tenant ───────────────────────────────────────────────────────────────────────

function CreateEmployer({ onNotice, onCreated }: {
  onNotice: (n: Notice) => void; onCreated: () => Promise<void>
}) {
  const [name, setName] = useState('')
  const [website, setWebsite] = useState('')
  const [busy, setBusy] = useState(false)

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    if (busy || name.trim().length < 2) return
    setBusy(true); onNotice(null)
    try {
      const r = await C.createEmployer(name.trim(), website.trim() || undefined)
      // The server's state word: every organisation starts as a draft, by design — it may never
      // be created verified by any route.
      onNotice({
        tone: 'info',
        text: `${name.trim()} is created as a ${r.state} organisation. It can draft postings now and publish once a person verifies it.`,
      })
      setName(''); setWebsite('')
      await onCreated()
    } catch (err) {
      onNotice({ tone: 'error', text: C.errText(err, 'The organisation could not be created.') })
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="wc-neworg-h" className="wc-section">
      <h2 id="wc-neworg-h">Create an organisation</h2>
      <form onSubmit={e => { void submit(e) }} noValidate>
        <label htmlFor="wc-org-name">Organisation name</label>
        <input id="wc-org-name" value={name} onChange={e => setName(e.target.value)} aria-describedby="wc-org-help" />
        <label htmlFor="wc-org-web">Website (optional)</label>
        <input id="wc-org-web" value={website} onChange={e => setWebsite(e.target.value)} />
        <p id="wc-org-help" className="wc-help">
          New organisations start as drafts: they can prepare postings but publish nothing until a
          person at PCI verifies who is behind them.
        </p>
        <p><button type="submit" disabled={busy || name.trim().length < 2}>
          {busy ? 'Creating…' : 'Create organisation'}
        </button></p>
      </form>
    </section>
  )
}
