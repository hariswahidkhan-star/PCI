import { useState } from 'react'
import { useMe } from '../data/MeContext'
import { api, ApiError } from '../api/client'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate, fmtDateTime, daysUntil } from '../format'
import type { ExamEntry } from '../api/types'

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
            <a className="btn sm" href="/exam-ui.html">Go to exam</a>
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

export default function Certifications() {
  const { me, loading, error, refetch } = useMe()
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Certifications &amp; exams</h1>
        <p className="muted">Schedule your exams and track each credential you have paid for.</p>
      </div>
      {me.exams.length === 0 ? (
        <Card>
          <Empty>You don’t have any exam entitlements yet.</Empty>
          <a className="btn sm" href="/enroll.html">Explore certifications</a>
        </Card>
      ) : (
        me.exams.map((e) => <EntryCard key={e.payment_id} entry={e} onChanged={refetch} />)
      )}
    </div>
  )
}
