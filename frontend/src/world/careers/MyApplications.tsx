import { useCallback, useEffect, useState } from 'react'
import * as C from './api'

// PCI World careers — the applicant's own record (docs/pciworld/CCP_PHASE4_DESIGN.md §5).
//
// THE CONSENT RECORD OUTLIVES THE CONSENT. Withdrawal is a timestamp on the row, never its
// deletion (§5.3), and this view renders the record the server sends INCLUDING after withdrawal:
// the row survives, `consent.active` goes false, and the granted-at and policy version stay
// visible — because "was there consent at the time?" must stay answerable, and the person who
// gave it is entitled to see that it is.
//
// WITHDRAWING CONSENT IS NOT ERASURE, and the server says so in its own sentence (`notice`).
// That sentence is shown verbatim — paraphrasing it into "your data is gone" would promise an
// erasure that did not happen.
//
// NOTHING IS ECHOED THAT THE SERVER DID NOT CONFIRM: after either withdrawal the list is re-read
// from the server, so every state word on screen is the server's, never a local guess.

type Notice = { tone: 'info' | 'held' | 'error'; text: string } | null

export default function MyApplications() {
  const [apps, setApps] = useState<C.MyApplication[] | null>(null)
  const [signedOut, setSignedOut] = useState(false)
  const [unavailable, setUnavailable] = useState(false)
  const [notice, setNotice] = useState<Notice>(null)

  const load = useCallback(async () => {
    try {
      setApps((await C.myApplications()).applications)
    } catch (e) {
      if (e instanceof C.CareersError && e.status === 401) setSignedOut(true)
      else if (e instanceof C.CareersError && e.status === 404) setUnavailable(true)
      else setNotice({ tone: 'error', text: 'Your applications could not be loaded right now. Please try again shortly.' })
    }
  }, [])

  useEffect(() => { void load() }, [load])

  return (
    <div className="wc">
      <header className="wc-head"><h1>My applications</h1></header>

      {notice && (
        // role="status", not "alert": these are outcomes, not emergencies. The tone class only
        // reinforces words that already carry the whole meaning.
        <p role="status" className={`wc-notice wc-notice-${notice.tone}`}>{notice.text}</p>
      )}

      {signedOut && (
        <p className="wc-signin">
          Please <a href="/world/">sign in to your PCI World account</a> to see your applications.
        </p>
      )}
      {unavailable && <p className="wc-signin">PCI World Careers is not open yet.</p>}
      {!apps && !signedOut && !unavailable && <p role="status">Loading…</p>}

      {apps && apps.length === 0 && <p>You have not applied to any postings yet.</p>}
      {apps && apps.length > 0 && (
        <ul className="wc-rows">
          {apps.map(a => <ApplicationRow key={a.id} app={a} onNotice={setNotice} reload={load} />)}
        </ul>
      )}
    </div>
  )
}

function ApplicationRow({ app, onNotice, reload }: {
  app: C.MyApplication; onNotice: (n: Notice) => void; reload: () => Promise<void>
}) {
  const [busy, setBusy] = useState(false)
  const [confirming, setConfirming] = useState(false)

  async function withdrawApp() {
    if (busy) return
    setBusy(true); onNotice(null)
    try {
      const r = await C.withdrawApplication(app.id)
      // The server's state word, then a re-read — the row below renders what the server now says.
      onNotice({ tone: 'info', text: `Your application for "${app.title}" is now ${r.state}. The record of it stays in this list.` })
      await reload()
    } catch (e) {
      onNotice({ tone: 'error', text: C.errText(e, 'The application could not be withdrawn.') })
    } finally { setBusy(false) }
  }

  async function withdrawConsent() {
    if (busy) return
    setBusy(true); onNotice(null)
    try {
      const r = await C.withdrawConsent(app.id)
      // The server's sentence, VERBATIM. It is careful to say the employer's access stops, that
      // the record is kept, and that withdrawal is not erasure — those words are the outcome.
      onNotice({ tone: 'info', text: r.notice })
      setConfirming(false)
      await reload()
    } catch (e) {
      onNotice({ tone: 'error', text: C.errText(e, 'Consent could not be withdrawn.') })
      setConfirming(false)
    } finally { setBusy(false) }
  }

  const c = app.consent
  return (
    <li className="wc-row">
      <p><b>{app.title}</b> — {app.employer}</p>
      <p>
        Status: <span className="wc-state">{app.state}</span>
        {app.submitted_at && <> · submitted {app.submitted_at} UTC</>}
        {app.withdrawn_at && <> · withdrawn {app.withdrawn_at} UTC</>}
        {app.cv_name && <> · CV: {app.cv_name}</>}
      </p>

      {/* The consent record, rendered from the server row and never hidden by withdrawal. */}
      {c.granted_at ? (
        c.active ? (
          <p>Consent: <span className="wc-state">active</span> — granted {c.granted_at} UTC under policy {c.policy_version}.</p>
        ) : (
          <p>
            Consent: <span className="wc-state">withdrawn</span> {c.withdrawn_at} UTC. The record
            remains: you granted consent {c.granted_at} UTC under policy {c.policy_version}.
          </p>
        )
      ) : (
        <p>No consent is recorded for this application.</p>
      )}

      <div className="wc-actions">
        {app.state !== 'withdrawn' && (
          <button type="button" className="secondary" onClick={() => { void withdrawApp() }} disabled={busy}>
            Withdraw application {app.id}
          </button>
        )}
        {c.active && !confirming && (
          <button type="button" className="secondary" onClick={() => setConfirming(true)} disabled={busy}>
            Withdraw consent for application {app.id}
          </button>
        )}
      </div>

      {confirming && (
        <div className="wc-confirm">
          {/* The consequence, stated BEFORE the click, in the same terms the server will use
              after it: access stops, the record stays, and this is not erasure. */}
          <p>
            Withdrawing consent stops the employer opening this application and your CV from this
            moment. The record that you applied, and that you consented at the time, is kept —
            withdrawing consent is not the same as erasure.
          </p>
          <button type="button" onClick={() => { void withdrawConsent() }} disabled={busy}>
            Yes, withdraw consent
          </button>{' '}
          <button type="button" className="secondary" onClick={() => setConfirming(false)} disabled={busy}>
            Keep consent
          </button>
        </div>
      )}
    </li>
  )
}
