import { useCallback, useEffect, useRef, useState } from 'react'
import * as C from './api'

// PCI World contributor publishing — the writer's desk (docs/pciworld/CCP_PHASE6_DESIGN.md §4, §5).
//
// WHAT THIS SURFACE IS NOT. It is not a publishing tool. Nothing here reaches the public site: the
// contributor writes, submits, and then an EDITOR WHO IS NOT THEM decides. There is deliberately no
// approve control, no publish control and no route to one — the surest way to be certain a control
// cannot be bypassed is not to draw the bypass. The maker-checker lives in the server and this
// screen simply has no opinion about it.
//
// THREE THINGS THE DESIGN INSISTS ON, and each shows up in the markup below:
//
//   1. Standing is a GRANT, not a threshold. The screen never implies that writing enough forum
//      posts unlocks anything: applying is an application, and an editor decides. A revoked
//      contributor is told plainly, with the reason recorded at the time — a silent revocation
//      looks like a bug to the person it happened to.
//   2. Submission FREEZES the declarations. They are answered here, sent with the submission, and
//      cannot be edited afterwards — so the form says so before it is submitted, not after.
//   3. A published piece is never edited in place. Once the server says a piece is no longer
//      editable, the editor here closes and says why; corrections are a new version with a public
//      note, and that is an editorial act rather than a text box.
//
// ACCESSIBILITY: every outcome is TEXT, in a role="status" region — these are results, not
// emergencies, and an assertive alert would interrupt a screen reader mid-word. The only
// programmatic focus move is back toward the field a person's own submit was refused on.

type Notice = { tone: 'info' | 'held' | 'error'; text: string } | null

const BLANK: C.Declarations = { conflict: '', sponsorship: '', ai_assistance: '', originality: '' }

export default function ContributorDesk() {
  const [me, setMe] = useState<C.Me | null>(null)
  const [articles, setArticles] = useState<C.ArticleSummary[]>([])
  const [openId, setOpenId] = useState<number | null>(null)
  const [loading, setLoading] = useState(true)
  const [signedOut, setSignedOut] = useState(false)
  const [unavailable, setUnavailable] = useState(false)
  const [notice, setNotice] = useState<Notice>(null)

  const load = useCallback(async () => {
    // Only blank the screen when there is nothing on it yet. A reload AFTER an action must not
    // unmount the children: doing so destroyed the very notice the action had just produced, so
    // applying to contribute showed its confirmation for one frame and then lost it. Refreshing
    // data is not a reason to take the page away from someone mid-read.
    setLoading(prev => prev || me === null)
    try {
      // /me first: standing decides the entire shape of this screen, and half-rendering a desk to
      // someone who cannot write would invite them to write something they cannot submit.
      const who = await C.me()
      setMe(who)
      if (who.may_write) setArticles((await C.listArticles()).articles)
    } catch (e) {
      if (e instanceof C.ContributorError && e.status === 401) setSignedOut(true)
      // 404 is the feature flag being off, which is a deliberate state rather than a fault.
      else if (e instanceof C.ContributorError && e.status === 404) setUnavailable(true)
      else setNotice({ tone: 'error', text: 'The writing desk is unavailable right now. Please try again shortly.' })
    } finally { setLoading(false) }
    // `me` is read only to decide whether this is the first load; including it in the deps would
    // rebuild the callback on every refresh and re-fire the effect below.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  useEffect(() => { void load() }, [load])

  if (loading && me === null) return <div className="cd"><p role="status">Loading your writing desk…</p></div>

  if (signedOut) return (
    <div className="cd">
      <h1>Write for PCI World</h1>
      <p className="cd-signin">Please <a href="/world-app/">sign in to your PCI World account</a> to
        apply or to write.</p>
    </div>
  )

  if (unavailable) return (
    <div className="cd">
      <h1>Write for PCI World</h1>
      <p className="cd-help">Contributor publishing is not open yet.</p>
    </div>
  )

  return (
    <div className="cd">
      <h1>Write for PCI World</h1>

      {notice && (
        <p role="status" className={`cd-notice cd-notice-${notice.tone}`}>{notice.text}</p>
      )}

      <Standing me={me} onChanged={load} onNotice={setNotice} />

      {me?.may_write && (openId === null ? (
        <>
          <Manuscripts articles={articles} onOpen={setOpenId} />
          <NewManuscript onCreated={async (id) => { await load(); setOpenId(id) }} onNotice={setNotice} />
        </>
      ) : (
        <Manuscript
          id={openId}
          onClose={() => { setOpenId(null); void load() }}
          onNotice={setNotice}
        />
      ))}
    </div>
  )
}

// ── Standing ────────────────────────────────────────────────────────────────────────────────

function Standing({ me, onChanged, onNotice }: {
  me: C.Me | null; onChanged: () => Promise<void>; onNotice: (n: Notice) => void
}) {
  const [statement, setStatement] = useState('')
  const [busy, setBusy] = useState(false)
  const [serverNotice, setServerNotice] = useState<string | null>(null)
  const ref = useRef<HTMLTextAreaElement>(null)

  if (me?.state === 'granted') return (
    <section aria-labelledby="cd-standing-h" className="cd-section">
      <h2 id="cd-standing-h">Your standing</h2>
      <p><span className="cd-pill cd-pill-live">Contributor</span>{' '}
        {me.granted_at && <span className="cd-help">Granted {me.granted_at} UTC.</span>}</p>
      {/* Said before anyone writes anything: publication is somebody else's decision. */}
      <p className="cd-help">You may write and submit. Publication is decided by an editor who is
        not you — nothing you submit appears on the site until a person accepts it.</p>
    </section>
  )

  if (me?.state === 'revoked') return (
    <section aria-labelledby="cd-standing-h" className="cd-section">
      <h2 id="cd-standing-h">Your standing</h2>
      <p><span className="cd-pill cd-pill-stop">Withdrawn</span></p>
      {/* The reason is shown to the person it happened to. A revocation nobody can explain is
          indistinguishable from a mistake, and cannot be appealed. */}
      <p>Your contributor standing was withdrawn{me.revoked_at ? ` on ${me.revoked_at} UTC` : ''}.</p>
      {me.revoke_reason && <p className="cd-quote">{me.revoke_reason}</p>}
      <p className="cd-help">Anything you already published stays published. Restoring standing is
        an editorial decision, so re-applying here will not do it — contact the editorial team.</p>
    </section>
  )

  if (me?.state === 'applied') return (
    <section aria-labelledby="cd-standing-h" className="cd-section">
      <h2 id="cd-standing-h">Your application</h2>
      <p><span className="cd-pill cd-pill-held">With the editorial team</span></p>
      <p className="cd-help">Contributor status is granted by a person, not automatically, and there
        is no guaranteed timescale.</p>
    </section>
  )

  // 'none' or 'declined' — the application form. A declined applicant may apply again; that is the
  // editor's call to reverse, unlike a revocation.
  async function submit(e: React.FormEvent) {
    e.preventDefault()
    if (busy) return
    setBusy(true); onNotice(null); setServerNotice(null)
    try {
      const r = await C.apply(statement.trim())
      setServerNotice(r.notice)          // the server's own words, verbatim
      setStatement('')
      await onChanged()
    } catch (err) {
      if (err instanceof C.ContributorError) {
        // 503 carries its own sentence explaining that the terms have not been published; showing
        // our paraphrase instead would hide which of the two doors is shut.
        onNotice({ tone: err.status === 503 ? 'held' : 'error', text: err.notice ?? err.message })
      } else {
        onNotice({ tone: 'error', text: 'Your application could not be sent.' })
      }
      ref.current?.focus()
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="cd-apply-h" className="cd-section">
      <h2 id="cd-apply-h">Apply to write</h2>
      <p className="cd-help">PCI World publishes practitioners writing about their own work.
        Contributor standing is granted by an editor after reading your application — there is no
        threshold that grants it automatically.</p>

      {me && !me.terms_available && (
        <p role="status" className="cd-notice cd-notice-held">Applications are not open yet: the
          contributor terms have not been published.</p>
      )}

      {serverNotice && <p role="status" className="cd-notice cd-notice-held">{serverNotice}</p>}

      <form onSubmit={submit} noValidate>
        <label htmlFor="cd-stmt">What would you like to write about, and what is your experience of it?</label>
        <textarea
          id="cd-stmt" ref={ref} rows={5} value={statement} maxLength={4000}
          onChange={e => setStatement(e.target.value)}
          aria-describedby="cd-stmt-help"
        />
        <p id="cd-stmt-help" className="cd-help">A few sentences is plenty. An editor reads this
          alongside your account history.</p>
        <p><button disabled={busy || !me?.terms_available}>
          {busy ? 'Sending…' : 'Send application'}</button></p>
      </form>
    </section>
  )
}

// ── The list ────────────────────────────────────────────────────────────────────────────────

function Manuscripts({ articles, onOpen }: { articles: C.ArticleSummary[]; onOpen: (id: number) => void }) {
  if (articles.length === 0) return (
    <section aria-labelledby="cd-list-h" className="cd-section">
      <h2 id="cd-list-h">Your pieces</h2>
      {/* Said in words rather than left as an empty box, which reads as a fault. */}
      <p className="cd-help">You have not started anything yet.</p>
    </section>
  )
  return (
    <section aria-labelledby="cd-list-h" className="cd-section">
      <h2 id="cd-list-h">Your pieces</h2>
      <ul className="cd-rows">
        {articles.map(a => (
          <li key={a.id} className="cd-row">
            <p className="cd-row-title">{a.title}</p>
            <p className="cd-row-meta">
              <span className={`cd-pill cd-pill-${C.statusTone(a.status)}`}>{C.statusLabel(a.status)}</span>
              {a.published_at && <span className="cd-help"> Published {a.published_at} UTC</span>}
            </p>
            <p>
              <button className="cd-linkish" onClick={() => onOpen(a.id)}>
                {a.editable ? 'Continue writing' : 'Open'}
                <span className="cd-visually-hidden"> — {a.title}</span>
              </button>
            </p>
          </li>
        ))}
      </ul>
    </section>
  )
}

function NewManuscript({ onCreated, onNotice }: {
  onCreated: (id: number) => Promise<void>; onNotice: (n: Notice) => void
}) {
  const [title, setTitle] = useState('')
  const [busy, setBusy] = useState(false)
  const ref = useRef<HTMLInputElement>(null)

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    if (busy) return
    setBusy(true); onNotice(null)
    try {
      const r = await C.createArticle(title.trim())
      setTitle('')
      await onCreated(r.article_id)
    } catch (err) {
      onNotice({ tone: 'error', text: err instanceof C.ContributorError ? err.message : 'That could not be started.' })
      ref.current?.focus()
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="cd-new-h" className="cd-section">
      <h2 id="cd-new-h">Start something new</h2>
      <form onSubmit={submit} noValidate>
        <label htmlFor="cd-new-title">Working title</label>
        <input id="cd-new-title" ref={ref} value={title} maxLength={200}
               onChange={e => setTitle(e.target.value)} aria-describedby="cd-new-help" />
        <p id="cd-new-help" className="cd-help">You can change this until you submit.</p>
        <p><button disabled={busy}>{busy ? 'Starting…' : 'Start writing'}</button></p>
      </form>
    </section>
  )
}

// ── One manuscript: edit, submit, and the editor thread ─────────────────────────────────────

function Manuscript({ id, onClose, onNotice }: {
  id: number; onClose: () => void; onNotice: (n: Notice) => void
}) {
  const [a, setA] = useState<C.ArticleDetail | null>(null)
  const [title, setTitle] = useState('')
  const [dek, setDek] = useState('')
  const [body, setBody] = useState('')
  const [saved, setSaved] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  const load = useCallback(async () => {
    try {
      const d = await C.getArticle(id)
      setA(d); setTitle(d.title); setDek(d.dek ?? ''); setBody(d.body_md ?? '')
    } catch {
      onNotice({ tone: 'error', text: 'That piece could not be opened.' })
    }
  }, [id, onNotice])

  useEffect(() => { void load() }, [load])

  if (!a) return <p role="status">Loading…</p>

  async function save() {
    if (busy || !a) return
    setBusy(true); setSaved(null); onNotice(null)
    try {
      await C.saveArticle(a.id, { title: title.trim(), dek, body_md: body })
      setSaved('Saved.')
    } catch (err) {
      onNotice({ tone: 'error', text: err instanceof C.ContributorError ? err.message : 'That could not be saved.' })
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="cd-ms-h" className="cd-section">
      <p><button className="cd-linkish" onClick={onClose}>&larr; Back to your pieces</button></p>
      <h2 id="cd-ms-h">{a.title}</h2>
      <p>
        <span className={`cd-pill cd-pill-${C.statusTone(a.status)}`}>{C.statusLabel(a.status)}</span>
        {a.published_at && <span className="cd-help"> Published {a.published_at} UTC</span>}
      </p>

      {a.editable ? (
        <>
          <label htmlFor="cd-t">Title</label>
          <input id="cd-t" value={title} maxLength={200} onChange={e => setTitle(e.target.value)} />

          <label htmlFor="cd-d">Standfirst</label>
          <input id="cd-d" value={dek} onChange={e => setDek(e.target.value)}
                 aria-describedby="cd-d-help" />
          <p id="cd-d-help" className="cd-help">One sentence. Every listing and search result uses it.</p>

          <label htmlFor="cd-b">The piece</label>
          <textarea id="cd-b" rows={16} value={body} onChange={e => setBody(e.target.value)}
                    aria-describedby="cd-b-help" />
          {/* Told up front rather than discovered on refusal. The subset is the allowlist: there is
              no HTML intake at all, so there is no sanitiser to get wrong. */}
          <p id="cd-b-help" className="cd-help">
            Plain text with a small set of marks: <code>##</code> for a heading, <code>-</code> for a
            bullet, <code>&gt;</code> for a quote, <code>**bold**</code>, <code>*italic*</code> and
            <code>[text](https://…)</code> for a link. Anything else is shown as you typed it.
            Around 200 words is the minimum a piece can be published at.
          </p>

          <p className="cd-actions">
            <button onClick={() => void save()} disabled={busy}>{busy ? 'Saving…' : 'Save draft'}</button>
            {saved && <span role="status" className="cd-help">{saved}</span>}
          </p>

          {a.status === 'idea' || a.status === 'drafting'
            ? <SubmitPanel article={a} onSubmitted={load} onNotice={onNotice} />
            : (
              // Editable but no longer submittable. Saying which is which matters: leaving the
              // submit button there invited a second set of declarations AFTER an editor had read
              // the first, and a greyed button with no sentence beside it reads as a fault.
              <p className="cd-notice cd-notice-held" role="status">
                Already with an editor. You can still refine the text, and your editor sees your
                changes — but the declarations you gave at submission stand, and are not re-asked.
              </p>
            )}
        </>
      ) : (
        <>
          {/* The editor closed the contributor's copy. Saying WHY matters: a disabled form with no
              explanation reads as broken, and "a published piece is never edited in place" is a
              rule worth stating rather than implying. */}
          <p className="cd-notice cd-notice-held" role="status">
            {a.status === 'published'
              ? 'This piece is published. Published text is never edited in place — a correction is a new version with a dated public note, which an editor makes.'
              : 'This piece is with an editor, so it can no longer be edited here. Use the thread below if something needs changing.'}
          </p>
          <article className="cd-readonly">
            {a.dek && <p><strong>{a.dek}</strong></p>}
            <p className="cd-preserve">{a.body_md}</p>
          </article>
        </>
      )}

      <Thread article={a} onSent={load} onNotice={onNotice} />
      <History entries={a.history} />
    </section>
  )
}

// ── Declarations and submission ─────────────────────────────────────────────────────────────

function SubmitPanel({ article, onSubmitted, onNotice }: {
  article: C.ArticleDetail; onSubmitted: () => Promise<void>; onNotice: (n: Notice) => void
}) {
  const [d, setD] = useState<C.Declarations>(BLANK)
  const [busy, setBusy] = useState(false)
  const [serverNotice, setServerNotice] = useState<string | null>(null)
  const first = useRef<HTMLSelectElement>(null)

  const set = (k: keyof C.Declarations) => (v: string) => setD(prev => ({ ...prev, [k]: v }))
  const complete = Object.values(d).every(v => v.trim().length > 0)

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    if (busy) return
    setBusy(true); onNotice(null); setServerNotice(null)
    try {
      const r = await C.submitArticle(article.id, d)
      setServerNotice(r.notice)          // verbatim
      await onSubmitted()
    } catch (err) {
      onNotice({ tone: 'error', text: err instanceof C.ContributorError ? err.message : 'That could not be submitted.' })
      first.current?.focus()
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="cd-sub-h" className="cd-subsection">
      <h3 id="cd-sub-h">Submit for review</h3>
      {/* Stated BEFORE the answers are given. What was declared at submission is the thing a later
          dispute turns on, so it is frozen — and a person should know that while answering. */}
      <p className="cd-help">Four questions, answered in your own words. Your answers are recorded
        as they are at the moment you submit and cannot be changed afterwards.</p>

      {serverNotice && <p role="status" className="cd-notice cd-notice-held">{serverNotice}</p>}

      <form onSubmit={submit} noValidate>
        <Decl id="cd-conflict" selectRef={first} label="Do you have a commercial interest in anything named in this piece?"
              value={d.conflict} onChange={set('conflict')}
              options={['No', 'Yes — described below']} />
        <Decl id="cd-sponsor" label="Was any part of this sponsored or paid for?"
              value={d.sponsorship} onChange={set('sponsorship')}
              options={['No', 'Yes — described below']} />
        <Decl id="cd-ai" label="Did you use AI assistance, and how?"
              value={d.ai_assistance} onChange={set('ai_assistance')}
              options={['None', 'Research only', 'Drafting help, reviewed and rewritten by me']} />
        <Decl id="cd-orig" label="Is this your own original work, not published elsewhere?"
              value={d.originality} onChange={set('originality')}
              options={['Yes', 'Published elsewhere — details below']} />

        <p className="cd-actions">
          <button disabled={busy || !complete}>{busy ? 'Submitting…' : 'Submit for review'}</button>
          {!complete && <span className="cd-help">Answer all four to submit.</span>}
        </p>
      </form>
    </section>
  )
}

// A declaration is a choice PLUS free text when the choice needs explaining — not a checkbox.
// "Yes" without the detail is not a declaration, it is a shrug, and the detail box appearing only
// when it is needed keeps the honest case a single click.
function Decl({ id, label, value, onChange, options, selectRef }: {
  id: string
  label: string
  value: string
  onChange: (v: string) => void
  options: string[]
  selectRef?: React.Ref<HTMLSelectElement>
}) {
  const [choice, setChoice] = useState('')
  const [detail, setDetail] = useState('')
  const needsDetail = (c: string) => c.includes('—') || c.startsWith('Published')

  // The value handed upward is the sentence that gets frozen into the submission, so it carries
  // the detail inline rather than as a second field the server would have to reassemble.
  const push = (c: string, dt: string) =>
    onChange(!c ? '' : needsDetail(c) ? `${c}: ${dt}`.trim() : c)

  return (
    <div className="cd-decl">
      <label htmlFor={id}>{label}</label>
      <select id={id} ref={selectRef} value={choice}
              onChange={e => { setChoice(e.target.value); push(e.target.value, detail) }}>
        <option value="">Choose…</option>
        {options.map(o => <option key={o} value={o}>{o}</option>)}
      </select>
      {needsDetail(choice) && (
        <>
          <label htmlFor={`${id}-d`}>Please give the detail</label>
          <input id={`${id}-d`} value={detail}
                 onChange={e => { setDetail(e.target.value); push(choice, e.target.value) }} />
        </>
      )}
      {value && <p className="cd-visually-hidden">Recorded as: {value}</p>}
    </div>
  )
}

// ── The editor thread ───────────────────────────────────────────────────────────────────────

function Thread({ article, onSent, onNotice }: {
  article: C.ArticleDetail; onSent: () => Promise<void>; onNotice: (n: Notice) => void
}) {
  const [body, setBody] = useState('')
  const [busy, setBusy] = useState(false)
  const ref = useRef<HTMLTextAreaElement>(null)

  async function send(e: React.FormEvent) {
    e.preventDefault()
    if (busy || !body.trim()) return
    setBusy(true); onNotice(null)
    try {
      await C.sendMessage(article.id, body.trim())
      setBody('')
      // Re-read rather than append locally: the thread on screen must be the thread on the server,
      // and an optimistic echo is how the two start disagreeing.
      await onSent()
    } catch (err) {
      onNotice({ tone: 'error', text: err instanceof C.ContributorError ? err.message : 'That message could not be sent.' })
      ref.current?.focus()
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="cd-thread-h" className="cd-subsection">
      <h3 id="cd-thread-h">Your editor</h3>
      {article.messages.length === 0
        ? <p className="cd-help">No messages yet.</p>
        : (
          <ul className="cd-thread">
            {article.messages.map((m, i) => (
              <li key={i} className={m.from === 'editor' ? 'cd-msg cd-msg-editor' : 'cd-msg'}>
                <p className="cd-msg-who">{m.from === 'editor' ? 'Editor' : 'You'}
                  <span className="cd-help"> · {m.at} UTC</span></p>
                <p className="cd-msg-body">{m.body}</p>
              </li>
            ))}
          </ul>
        )}
      <form onSubmit={send} noValidate>
        <label htmlFor="cd-msg">Reply</label>
        <textarea id="cd-msg" ref={ref} rows={3} value={body} maxLength={4000}
                  onChange={e => setBody(e.target.value)} />
        <p className="cd-actions"><button disabled={busy || !body.trim()}>
          {busy ? 'Sending…' : 'Send'}</button></p>
      </form>
    </section>
  )
}

// ── History ─────────────────────────────────────────────────────────────────────────────────

function History({ entries }: { entries: C.HistoryEntry[] }) {
  if (entries.length === 0) return null
  return (
    <section aria-labelledby="cd-hist-h" className="cd-subsection">
      <h3 id="cd-hist-h">What has happened to this piece</h3>
      {/* Append-only on the server, so this list only ever grows — a later note never replaces an
          earlier one, which is what makes "why was this refused" answerable later. */}
      <ul className="cd-rows">
        {entries.map((e, i) => (
          <li key={i} className="cd-row">
            <p className="cd-row-title">{describe(e)}</p>
            <p className="cd-row-meta cd-help">{e.at} UTC{e.note ? ` — ${e.note}` : ''}</p>
          </li>
        ))}
      </ul>
    </section>
  )
}

function describe(e: C.HistoryEntry): string {
  const who = e.by === 'editor' ? 'An editor' : e.by === 'system' ? 'The system' : 'You'
  switch (e.event) {
    case 'submitted': return `${who} submitted it for review`
    case 'assigned': return 'An editor took it on'
    case 'returned': return 'An editor sent it back for changes'
    case 'accepted': return 'An editor accepted it'
    case 'declined': return 'An editor declined it'
    case 'published': return 'It was published'
    case 'archived': return 'It was withdrawn from the site'
    default: return e.to ? `${who} moved it to ${C.statusLabel(e.to)}` : `${who} added a note`
  }
}
