import { useState } from 'react'
import { useMe } from '../data/MeContext'
import { useQuery } from '../api/hooks'
import { api, ApiError } from '../api/client'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate, fmtDateTime, fmtMoney, daysUntil } from '../format'
import type { ExamEntry } from '../api/types'

/** A certification from the public, backend-controlled catalogue (GET /api/certifications). */
interface CatalogueCert {
  id: number
  code: string
  name: string
  description?: string | null
  expiry_years?: number | null
  duration_minutes?: number | null
  pass_mark_pct?: number | null
  exam_price?: number | null
}

const BOOK_ERRORS: Record<string, string> = {
  no_entitlement: 'No exam entitlement was found for this certification.',
  not_eligible: 'A few eligibility items must be completed before you can schedule.',
  window_lapsed: 'The scheduling window for this entitlement has closed.',
  already_booked: 'This certification already has a scheduled exam.',
  payment_already_used: 'This exam payment has already been used for a sitting.',
  exam_already_taken: 'An exam has already been taken for this entitlement.',
  bad_slot: 'Please choose a time at least 2 hours from now.',
  beyond_window: 'That time is after your scheduling deadline.',
}

function ScheduleForm({ entry, onDone }: { entry: ExamEntry; onDone: () => void }) {
  const [when, setWhen] = useState('')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const tz = Intl.DateTimeFormat().resolvedOptions().timeZone

  async function submit() {
    setError(null)
    setBusy(true)
    try {
      // datetime-local yields local wall-clock "YYYY-MM-DDTHH:mm"; send with timezone so the server anchors it.
      await api.post('/api/me/exam/book', {
        certification_id: entry.certification_id,
        scheduled_at: new Date(when).toISOString(),
        timezone: tz,
      })
      onDone()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setError(BOOK_ERRORS[code] || (e instanceof Error ? e.message : 'Unable to schedule.'))
    } finally {
      setBusy(false)
    }
  }

  // datetime-local reads its value/min/max as LOCAL wall-clock, so build the bounds in local time
  // (not via toISOString, which is UTC). The deadline is stored as UTC, so parse it with a 'Z'.
  const toLocalInput = (d: Date) => new Date(d.getTime() - d.getTimezoneOffset() * 60000).toISOString().slice(0, 16)
  const min = toLocalInput(new Date(Date.now() + 2 * 3600_000))
  const max = entry.deadline ? toLocalInput(new Date(String(entry.deadline).replace(' ', 'T') + 'Z')) : undefined

  return (
    <div className="stack" style={{ marginTop: '.75rem' }}>
      {error && <div className="notice err" role="alert">{error}</div>}
      <div className="field" style={{ margin: 0 }}>
        <label htmlFor="sched-when">Choose a date &amp; time ({tz})</label>
        <input id="sched-when" type="datetime-local" value={when} min={min} max={max} onChange={(e) => setWhen(e.target.value)} />
      </div>
      <div className="row">
        <button className="btn sm" disabled={!when || busy} onClick={submit}>{busy ? 'Scheduling…' : 'Confirm slot'}</button>
      </div>
    </div>
  )
}

function EntryCard({ entry, onChanged }: { entry: ExamEntry; onChanged: () => void }) {
  const [scheduling, setScheduling] = useState(false)
  const booking = entry.booking as Record<string, unknown> | null
  const attempt = entry.latest_attempt as Record<string, unknown> | null
  const cred = entry.credential
  const deadlineDays = daysUntil(entry.deadline)

  let state: { tone: 'ok' | 'warn' | 'brand' | 'neutral' | 'err'; label: string }
  if (cred?.status === 'active') state = { tone: 'ok', label: 'Certified' }
  else if (attempt) state = { tone: 'brand', label: 'Exam taken' }
  else if (booking) state = { tone: 'warn', label: 'Scheduled' }
  else state = { tone: 'neutral', label: 'Not scheduled' }

  return (
    <Card
      title={entry.certification_name || `Certification #${entry.certification_id}`}
      action={<Badge tone={state.tone}>{state.label}</Badge>}
    >
      <div className="small muted stack">
        {entry.certification_code && <div>Code: <strong>{entry.certification_code}</strong></div>}
        {entry.reference && <div>Payment ref: {entry.reference}</div>}
        {entry.deadline && (
          <div>
            Scheduling deadline: {fmtDate(entry.deadline)}
            {deadlineDays !== null && deadlineDays >= 0 && <> · {deadlineDays} day{deadlineDays === 1 ? '' : 's'} left</>}
          </div>
        )}
      </div>

      {cred?.status === 'active' ? (
        <div className="notice" style={{ marginTop: '.75rem' }}>
          Credential <strong>{cred.credential_id}</strong> · expires {fmtDate(cred.expires_at)}
        </div>
      ) : booking ? (
        <div className="notice" style={{ marginTop: '.75rem' }}>
          Your exam is scheduled for <strong>{fmtDateTime(booking.scheduled_at)}</strong>
          {booking.timezone ? ` (${booking.timezone})` : ''}.
          <div className="row" style={{ marginTop: '.6rem' }}>
            <a className="btn sm" href="/student.html#exam">Go to exam</a>
            <a className="btn sm secondary" href="/student.html#exam">Reschedule</a>
          </div>
        </div>
      ) : attempt ? (
        <div style={{ marginTop: '.75rem' }} className="row">
          <span className="muted small">Latest attempt:</span> <StatusBadge status={String(attempt.result_status || attempt.status || '')} />
        </div>
      ) : (
        <div style={{ marginTop: '.75rem' }}>
          {scheduling ? (
            <ScheduleForm entry={entry} onDone={() => { setScheduling(false); onChanged() }} />
          ) : (
            <button className="btn sm" onClick={() => setScheduling(true)}>Schedule exam</button>
          )}
        </div>
      )}
    </Card>
  )
}

/** Live catalogue of every certification the Institute offers — driven entirely by the backend, so a
 *  credential added in the admin console appears here automatically. Enrolment links carry the cert code
 *  so the checkout prices and books that specific credential. */
function Catalogue({ ownedCodes }: { ownedCodes: Set<string> }) {
  const { data, loading, error } = useQuery<{ rows: CatalogueCert[] }>('/api/certifications')

  if (loading) return <Card><Spinner /></Card>
  if (error) return <Card><ErrorNote>{error}</ErrorNote></Card>
  const rows = data?.rows ?? []
  if (rows.length === 0) return <Card><Empty>No certifications are open for enrolment right now.</Empty></Card>

  return (
    <div className="cert-catalogue-grid">
      {rows.map((c) => {
        const owned = ownedCodes.has((c.code || '').toUpperCase())
        return (
          <div className="cert-tile" key={c.id}>
            <div className="cert-tile-head">
              <span className="cert-tile-code">{c.code}</span>
              {owned && <Badge tone="ok">Enrolled</Badge>}
            </div>
            <h3 className="cert-tile-name">{c.name}</h3>
            {c.description && <p className="muted small cert-tile-desc">{c.description}</p>}
            <ul className="cert-tile-meta">
              {c.exam_price != null && <li><strong>{fmtMoney(c.exam_price)}</strong> exam fee</li>}
              {c.duration_minutes != null && c.pass_mark_pct != null && (
                <li>{c.duration_minutes} min · {c.pass_mark_pct}% to pass</li>
              )}
              {c.expiry_years != null && <li>Valid {c.expiry_years} year{c.expiry_years === 1 ? '' : 's'}</li>}
            </ul>
            {owned ? (
              <span className="btn sm secondary cert-tile-cta" aria-disabled="true" style={{ opacity: 0.6, pointerEvents: 'none' }}>Already enrolled</span>
            ) : (
              <a className="btn sm cert-tile-cta" href={`/checkout.html?product=exam&cert=${encodeURIComponent(c.code)}`}>Enrol in this exam</a>
            )}
          </div>
        )
      })}
    </div>
  )
}

export default function Certifications() {
  const { me, loading, error, refetch } = useMe()
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  const ownedCodes = new Set(
    me.exams.map((e) => (e.certification_code || '').toUpperCase()).filter(Boolean),
  )

  return (
    <div className="stack" style={{ display: 'grid', gap: '1.75rem' }}>
      <div>
        <h1>Certifications &amp; exams</h1>
        <p className="muted">Schedule your exams, track each credential you hold, and enrol in more.</p>
      </div>

      <section className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <h2 className="section-title">Your certifications</h2>
        {me.exams.length === 0 ? (
          <Card>
            <Empty>You don’t have any exam entitlements yet — explore the certifications below to get started.</Empty>
          </Card>
        ) : (
          me.exams.map((e) => <EntryCard key={e.payment_id} entry={e} onChanged={refetch} />)
        )}
      </section>

      <section className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <div>
          <h2 className="section-title">Explore certifications</h2>
          <p className="muted small" style={{ margin: '.25rem 0 0' }}>Every credential the Institute offers, kept current automatically.</p>
        </div>
        <Catalogue ownedCodes={ownedCodes} />
      </section>
    </div>
  )
}
