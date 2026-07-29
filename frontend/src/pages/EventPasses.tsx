import { useCallback, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { PageHeader } from '../components/premium'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'
import { openPrintable, escapeHtml } from '../print'
import {
  EventAdmissionUnavailableError,
  fetchMyEventPasses,
  fixturesEnabled,
  replaceEventPass,
} from '../api/eventAdmission'
import type { EventPass, PassState, PaymentState, RegistrationState } from '../api/types'
import { buildPassIcs } from './eventPassIcs'

// My event passes (spec §7A.12A) — the student's wallet of entry passes for physical events.
// Registration itself stays on the Events page; this page is only about getting through the door.
// The backend for passes does not exist yet: the seam (src/api/eventAdmission.ts) maps 404/501 to
// EventAdmissionUnavailableError and this page renders a designed "not yet enabled" state.
// `?preview=1` turns on fixtures mode with realistic sample passes.

const MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']

/** Pass times are wall-clock in the EVENT's timezone (typed contract), so render them textually —
 *  parsing through Date would shift them into the viewer's timezone. */
export function fmtWallClock(dt: string): string {
  const m = /^(\d{4})-(\d{2})-(\d{2})[ T](\d{2}):(\d{2})/.exec(dt)
  if (!m) return dt
  return `${Number(m[3])} ${MONTHS[Number(m[2]) - 1] ?? m[2]} ${m[1]}, ${m[4]}:${m[5]}`
}

const REG_LABEL: Record<RegistrationState, { text: string; tone: 'ok' | 'warn' | 'err' | 'neutral' }> = {
  registered: { text: 'Registered', tone: 'ok' },
  waitlisted: { text: 'Waitlisted', tone: 'warn' },
  cancelled: { text: 'Registration cancelled', tone: 'err' },
}

const PAY_LABEL: Record<PaymentState, { text: string; tone: 'ok' | 'warn' | 'err' | 'neutral' }> = {
  not_required: { text: 'No payment required', tone: 'neutral' },
  pending: { text: 'Payment pending', tone: 'warn' },
  paid: { text: 'Paid', tone: 'ok' },
  refunded: { text: 'Refunded', tone: 'err' },
  waived: { text: 'Fee waived', tone: 'ok' },
}

const PASS_LABEL: Record<PassState, { text: string; tone: 'ok' | 'warn' | 'err' | 'neutral' }> = {
  issued: { text: 'Pass issued', tone: 'neutral' },
  active: { text: 'Pass active', tone: 'ok' },
  used: { text: 'Pass used', tone: 'neutral' },
  expired: { text: 'Pass expired', tone: 'err' },
  revoked: { text: 'Pass revoked', tone: 'err' },
  replaced: { text: 'Pass replaced', tone: 'warn' },
}

const MODE_LABEL: Record<EventPass['admissionMode'], string> = {
  qr: 'QR scan at the gate',
  manual: 'Manual check-in at the desk',
  hybrid: 'QR scan, with manual desk fallback',
}

function downloadIcs(pass: EventPass) {
  const blob = new Blob([buildPassIcs(pass)], { type: 'text/calendar;charset=utf-8' })
  const url = URL.createObjectURL(blob)
  const a = document.createElement('a')
  a.href = url
  a.download = `pci-event-${pass.event.id}.ics`
  document.body.appendChild(a)
  a.click()
  a.remove()
  setTimeout(() => URL.revokeObjectURL(url), 10_000)
}

function printPass(pass: EventPass) {
  const e = escapeHtml
  openPrintable(
    `PCI event pass — ${pass.event.title}`,
    `<div class="brand">PROJECT CONTROLS INSTITUTE</div>
     <h1>${e(pass.event.title)}</h1>
     <div class="muted">${e(pass.event.venueName)}${pass.event.venueAddress ? ' — ' + e(pass.event.venueAddress) : ''}</div>
     <div class="big">${e(pass.passReference)}</div>
     <table>
       <tr><th>Event starts</th><td>${e(fmtWallClock(pass.event.startsAt))} (${e(pass.event.timezone)})</td></tr>
       <tr><th>Entry window</th><td>${e(fmtWallClock(pass.validity.opensAt))} – ${e(fmtWallClock(pass.validity.closesAt))} (${e(pass.event.timezone)})</td></tr>
       <tr><th>Admission</th><td>${e(MODE_LABEL[pass.admissionMode])}</td></tr>
     </table>
     <p class="muted">Present this reference at the manual-entry desk with photo ID. Your live entry QR
     stays in the portal (My event passes) — it is not printed here, so a lost printout cannot be
     scanned by someone else. This pass is not your public Passport.</p>`,
    false,
  )
}

/** Decorative QR placeholder for fixtures mode — a real pass QR is rendered by the backend build. */
function QrPlaceholder({ token }: { token: string }) {
  return (
    <div
      aria-label="Entry QR code placeholder"
      role="img"
      style={{
        width: 132,
        height: 132,
        borderRadius: 8,
        border: '1px solid var(--line, #e3e8ef)',
        background:
          'repeating-linear-gradient(0deg, #0f172a 0 8px, #fff 8px 16px), repeating-linear-gradient(90deg, #0f172a 0 8px, #fff 8px 16px)',
        backgroundBlendMode: 'screen',
        flex: '0 0 auto',
      }}
      title={`Preview placeholder for entry token ${token}`}
    />
  )
}

function PassCard({ pass, onReplaced }: { pass: EventPass; onReplaced: (fresh: EventPass) => void }) {
  const [confirmingReplace, setConfirmingReplace] = useState(false)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const reg = REG_LABEL[pass.registrationState]
  const pay = PAY_LABEL[pass.paymentState]
  const st = PASS_LABEL[pass.state]

  async function doReplace() {
    setBusy(true)
    setErr(null)
    try {
      const fresh = await replaceEventPass(pass.passId)
      // The parent swaps the pass (new passId => this card remounts) and shows the notice.
      onReplaced(fresh)
    } catch (e2) {
      setErr(e2 instanceof Error ? e2.message : 'Could not replace the pass. Please try again.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <Card
      title={pass.event.title}
      action={
        <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap' }}>
          <Badge tone={reg.tone}>{reg.text}</Badge>
          <Badge tone={pay.tone}>{pay.text}</Badge>
          <Badge tone={st.tone}>{st.text}</Badge>
        </div>
      }
    >
      <div className="stack small" style={{ gap: '.45rem' }}>
        <div className="muted">
          {fmtWallClock(pass.event.startsAt)}
          {pass.event.endsAt ? ` – ${fmtWallClock(pass.event.endsAt)}` : ''} ({pass.event.timezone})
        </div>
        <div className="muted">
          {pass.event.venueName}
          {pass.event.venueAddress ? ` — ${pass.event.venueAddress}` : ''}
        </div>
        <div>
          <strong>Entry window:</strong> {fmtWallClock(pass.validity.opensAt)} – {fmtWallClock(pass.validity.closesAt)}{' '}
          ({pass.event.timezone})
        </div>
        <div>
          <strong>Admission:</strong> {MODE_LABEL[pass.admissionMode]} · <strong>Pass reference:</strong>{' '}
          {pass.passReference}
        </div>

        {/* The event QR area — explicitly NOT the public Passport QR. */}
        <div
          style={{
            display: 'flex',
            gap: '1rem',
            alignItems: 'center',
            border: '1px dashed var(--line, #cbd5e1)',
            borderRadius: 10,
            padding: '.75rem',
            marginTop: '.25rem',
          }}
        >
          {pass.entryToken ? (
            <>
              <QrPlaceholder token={pass.entryToken} />
              <div className="stack small" style={{ gap: '.3rem' }}>
                <strong>Event entry pass — NOT your public Passport QR</strong>
                <span className="muted">
                  This QR admits you to this event only. Scanning your public Passport QR at the gate
                  will not admit anyone, and this entry QR reveals nothing from your Passport.
                </span>
                <span className="muted">Manual fallback code: {pass.entryToken}</span>
              </div>
            </>
          ) : (
            <div className="stack small" style={{ gap: '.3rem' }}>
              <strong>Event entry pass — NOT your public Passport QR</strong>
              <span className="muted">
                Your entry QR will appear here once your pass is active
                {pass.paymentState === 'pending' ? ' (payment pending)' : ''}
                {pass.registrationState === 'waitlisted' ? ' (currently waitlisted)' : ''}.
              </span>
            </div>
          )}
        </div>

        {err && <div className="notice err" role="alert">{err}</div>}

        {confirmingReplace ? (
          <div className="notice warn" role="alertdialog" aria-label="Confirm pass replacement">
            <div style={{ marginBottom: '.4rem' }}>
              Replace this pass? The current QR and reference stop working at the gate immediately —
              do this if the pass may have been shared, printed, or compromised.
            </div>
            <div className="row" style={{ gap: '.4rem' }}>
              <button className="btn sm" disabled={busy} onClick={() => void doReplace()}>
                {busy ? 'Replacing…' : 'Yes, replace pass'}
              </button>
              <button className="btn sm secondary" disabled={busy} onClick={() => setConfirmingReplace(false)}>
                Keep current pass
              </button>
            </div>
          </div>
        ) : (
          <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap', marginTop: '.2rem' }}>
            <button className="btn sm secondary" onClick={() => printPass(pass)}>
              Printable pass
            </button>
            <button className="btn sm secondary" onClick={() => downloadIcs(pass)}>
              Add to calendar (.ics)
            </button>
            {pass.state !== 'revoked' && pass.state !== 'replaced' && (
              <button className="btn sm secondary" onClick={() => setConfirmingReplace(true)}>
                Replace pass
              </button>
            )}
          </div>
        )}

        <div style={{ marginTop: '.35rem' }}>
          <strong>Entry &amp; checkout history</strong>
          {pass.history.length === 0 ? (
            <div className="muted">No entries recorded yet.</div>
          ) : (
            <ul style={{ margin: '.25rem 0 0', paddingLeft: '1.1rem' }}>
              {pass.history.map((h, i) => (
                <li key={i} className="muted">
                  {h.kind === 'entry' ? 'Entered' : 'Checked out'} · {fmtWallClock(h.at)} ({pass.event.timezone}) ·{' '}
                  {h.gateName} · {h.method === 'qr' ? 'QR scan' : 'manual check-in'}
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </Card>
  )
}

export default function EventPasses() {
  const [passes, setPasses] = useState<EventPass[] | null>(null)
  const [loading, setLoading] = useState(true)
  const [unavailable, setUnavailable] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [notice, setNotice] = useState<string | null>(null)

  const load = useCallback(async () => {
    setLoading(true)
    setUnavailable(false)
    setError(null)
    try {
      setPasses(await fetchMyEventPasses())
    } catch (e) {
      if (e instanceof EventAdmissionUnavailableError) setUnavailable(true)
      else setError(e instanceof Error ? e.message : 'Something went wrong.')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    void load()
  }, [load])

  const onReplaced = (fresh: EventPass) => {
    setPasses((prev) => {
      if (!prev) return prev
      // The replaced pass keeps its slot; the fresh pass takes over the same event.
      return prev.map((p) => (p.event.id === fresh.event.id && p.state !== 'used' ? fresh : p))
    })
    setNotice('Pass replaced — the previous QR no longer works at the gate.')
  }

  return (
    <div className="page">
      <PageHeader
        eyebrow="Events"
        title="My event passes"
        subtitle="Entry passes for in-person PCI events. Register on the Events page; your pass for each registration lives here."
      />
      {fixturesEnabled() && (
        <div className="notice" role="status">
          Preview mode — showing sample passes. Nothing here touches the server.
        </div>
      )}
      {notice && <div className="notice ok" role="status">{notice}</div>}

      {loading ? (
        <Spinner />
      ) : unavailable ? (
        <Card title="Event passes are not yet enabled on this server">
          <div className="stack small" style={{ gap: '.5rem' }}>
            <div>
              This installation does not have the event-admission service switched on yet. Your event
              registrations are unaffected — they live on the{' '}
              <Link to="/events">Events page</Link>. When admission goes live, a pass appears here
              automatically for every in-person registration.
            </div>
            <div className="muted">
              Reviewers: append <code>?preview=1</code> to this page's URL to see the wallet with
              sample data.
            </div>
          </div>
        </Card>
      ) : error ? (
        <ErrorNote>{error}</ErrorNote>
      ) : !passes || passes.length === 0 ? (
        <Card>
          <Empty>
            No event passes yet. Register for an in-person event on the{' '}
            <Link to="/events">Events page</Link> and your pass will appear here.
          </Empty>
        </Card>
      ) : (
        <>
          {passes.map((p) => (
            <PassCard key={p.passId} pass={p} onReplaced={onReplaced} />
          ))}
          <Card title="What gate staff can see">
            <div className="stack small muted" style={{ gap: '.35rem' }}>
              <div>
                When your pass is scanned or looked up at the gate, staff see only: your name, your
                registration reference, your Student Number, this event's registration, payment and
                pass status, and this event's entry history.
              </div>
              <div>
                They never see your Passport contents, scores, exam history, contact details, or
                anything from other events. Camera frames used for scanning are processed on the
                gate device only and are never stored or uploaded.
              </div>
            </div>
          </Card>
        </>
      )}
    </div>
  )
}
