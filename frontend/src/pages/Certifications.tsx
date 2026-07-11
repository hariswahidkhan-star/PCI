import { useState } from 'react'
import { useMe } from '../data/MeContext'
import { useQuery } from '../api/hooks'
import { api, ApiError, getToken } from '../api/client'
import { startCheckout, checkoutErrorMessage } from '../api/checkout'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate, fmtDateTime, fmtMoney, daysUntil } from '../format'
import type { ExamEntry, IdentityDocument } from '../api/types'

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
  no_booking: 'No scheduled exam was found to reschedule.',
  locked: 'Your sitting is too close to reschedule online — please contact support.',
  max_reschedules: 'This booking has reached its reschedule limit — please contact support.',
  no_entitlement: 'No exam entitlement was found for this certification.',
  not_eligible: 'A few eligibility items must be completed before you can schedule.',
  window_lapsed: 'The scheduling window for this entitlement has closed.',
  already_booked: 'This certification already has a scheduled exam.',
  payment_already_used: 'This exam payment has already been used for a sitting.',
  exam_already_taken: 'An exam has already been taken for this entitlement.',
  bad_slot: 'Please choose a time at least 2 hours from now.',
  beyond_window: 'That time is after your scheduling deadline.',
}

// User-level eligibility holds (from lifecycle.blocking_items) that pause the Schedule button —
// payment-level items like exam_fee_unpaid are handled by whether an entitlement card exists at all.
const HOLD_LABELS: Record<string, string> = {
  identity_document_missing: 'upload your government-issued ID (above)',
  consents_pending: 'accept the exam consents on your Overview page',
  profile_incomplete: 'complete your profile (country is required)',
  account_hold: 'your account is on hold — contact support',
  booking_closed: 'exam booking is temporarily closed',
}

function ScheduleForm({ entry, onDone, mode = 'book' }: { entry: ExamEntry; onDone: () => void; mode?: 'book' | 'reschedule' }) {
  const [when, setWhen] = useState('')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const tz = Intl.DateTimeFormat().resolvedOptions().timeZone

  async function submit() {
    setError(null)
    setBusy(true)
    try {
      // datetime-local yields local wall-clock "YYYY-MM-DDTHH:mm"; send with timezone so the server anchors it.
      await api.post(mode === 'reschedule' ? '/api/me/exam/reschedule' : '/api/me/exam/book', {
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

const UPLOAD_ERRORS: Record<string, string> = {
  file_type_not_allowed: 'Please upload a JPG, PNG, WEBP or PDF file.',
  file_too_large: 'That file is too large — the maximum is 3 MB.',
  content_mime_mismatch: 'That file does not appear to be a valid image or PDF.',
  too_many_uploads: 'Upload limit reached — please contact support to update your ID.',
}

const DOC_KINDS = [
  { value: 'passport', label: 'Passport' },
  { value: 'national_id', label: 'National ID card' },
  { value: 'driving_licence', label: 'Driving licence' },
  { value: 'other', label: 'Other government-issued ID' },
]

/** Government-ID upload + status. A document on file (not rejected) is required before any exam
 *  can be scheduled — the backend enforces this in Lifecycle.BookingBlockers. */
function IdentityCard({ doc, onChanged }: { doc: IdentityDocument | null; onChanged: () => void }) {
  const [kind, setKind] = useState('passport')
  const [file, setFile] = useState<File | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [replacing, setReplacing] = useState(false)

  const showForm = !doc || doc.status === 'rejected' || replacing

  async function submit() {
    if (!file) return
    if (file.size > 3_000_000) { setErr(UPLOAD_ERRORS.file_too_large); return }
    setBusy(true)
    setErr(null)
    try {
      const dataUri = await new Promise<string>((resolve, reject) => {
        const r = new FileReader()
        r.onload = () => resolve(String(r.result))
        r.onerror = () => reject(new Error('Could not read the file — please try again.'))
        r.readAsDataURL(file)
      })
      await api.post('/api/me/identity-document', { doc_kind: kind, filename: file.name, data_uri: dataUri })
      setFile(null)
      setReplacing(false)
      onChanged()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setErr(UPLOAD_ERRORS[code] || (e instanceof Error ? e.message : 'Upload failed.'))
    } finally {
      setBusy(false)
    }
  }

  const badge =
    !doc ? <Badge tone="warn">Required</Badge>
    : doc.status === 'verified' ? <Badge tone="ok">Verified</Badge>
    : doc.status === 'rejected' ? <Badge tone="err">Rejected</Badge>
    : <Badge tone="brand">Submitted</Badge>

  return (
    <Card title="Identity verification" action={badge}>
      {!doc && (
        <p className="muted small" style={{ marginTop: 0 }}>
          A government-issued photo ID (passport, national ID card or driving licence) is required
          before you can schedule any exam. It is reviewed by the institute and used only to verify
          your identity on exam day.
        </p>
      )}
      {doc && doc.status === 'rejected' && (
        <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>
          Your document could not be accepted{doc.review_note ? ` — ${doc.review_note}` : ''}. Please upload a new one to keep exam scheduling available.
        </div>
      )}
      {doc && doc.status !== 'rejected' && (
        <div className="small stack" style={{ gap: '.2rem' }}>
          <div><span className="muted">Document:</span> <strong>{DOC_KINDS.find((k) => k.value === doc.doc_kind)?.label ?? doc.doc_kind}</strong>{doc.filename ? ` · ${doc.filename}` : ''}</div>
          <div className="muted">Uploaded {fmtDate(doc.created_at)}{doc.status === 'submitted' ? ' · pending review — you can already schedule your exam' : ''}</div>
        </div>
      )}

      {showForm ? (
        <div className="stack" style={{ marginTop: doc ? '.75rem' : 0 }}>
          {err && <div className="notice err" role="alert">{err}</div>}
          <div className="field" style={{ margin: 0 }}>
            <label htmlFor="idd-kind">Document type</label>
            <select id="idd-kind" value={kind} onChange={(e) => setKind(e.target.value)}>
              {DOC_KINDS.map((k) => <option key={k.value} value={k.value}>{k.label}</option>)}
            </select>
          </div>
          <div className="field" style={{ margin: 0 }}>
            <label htmlFor="idd-file">File (JPG, PNG, WEBP or PDF — max 3 MB)</label>
            <input id="idd-file" type="file" accept=".jpg,.jpeg,.png,.webp,.pdf,image/jpeg,image/png,image/webp,application/pdf"
              onChange={(e) => setFile(e.target.files?.[0] ?? null)} />
          </div>
          <div className="row">
            <button className="btn sm" disabled={!file || busy} onClick={submit}>{busy ? 'Uploading…' : 'Submit document'}</button>
            {replacing && <button className="btn sm secondary" onClick={() => { setReplacing(false); setErr(null) }}>Cancel</button>}
          </div>
        </div>
      ) : (
        <div className="row" style={{ marginTop: '.75rem' }}>
          <button className="btn sm secondary" onClick={() => setReplacing(true)}>Replace document</button>
        </div>
      )}
    </Card>
  )
}

function EntryCard({ entry, onChanged, holds }: { entry: ExamEntry; onChanged: () => void; holds: string[] }) {
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
          <div className="row" style={{ marginTop: '.6rem', flexWrap: 'wrap' }}>
            <a className="btn sm" href={`/student.html#t=${getToken() ?? ''}`}>Exam day check-in ↗</a>
            <button className="btn sm secondary" onClick={() => setScheduling((v) => !v)}>
              {scheduling ? 'Close' : 'Reschedule'}
            </button>
          </div>
          {scheduling && (
            <div style={{ marginTop: '.6rem' }}>
              <ScheduleForm entry={entry} mode="reschedule" onDone={() => { setScheduling(false); onChanged() }} />
            </div>
          )}
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
            <>
              <button className="btn sm" disabled={holds.length > 0} onClick={() => setScheduling(true)}>Schedule exam</button>
              {holds.length > 0 && (
                <div className="muted small" style={{ marginTop: '.5rem' }}>
                  To unlock scheduling: {holds.map((h) => HOLD_LABELS[h] ?? h.replace(/_/g, ' ')).join(' · ')}.
                </div>
              )}
            </>
          )}
        </div>
      )}
    </Card>
  )
}

/** Live catalogue of every certification the Institute offers — driven entirely by the backend, so a
 *  credential added in the admin console appears here automatically. Exam fees are paid right here in
 *  the portal; Stripe's secure page handles the card and returns to Billing. */
function Catalogue({ ownedCodes }: { ownedCodes: Set<string> }) {
  const { me } = useMe()
  const { data, loading, error } = useQuery<{ rows: CatalogueCert[] }>('/api/certifications')
  const [buying, setBuying] = useState<string | null>(null)
  const [buyErr, setBuyErr] = useState<string | null>(null)

  async function buy(certCode: string) {
    if (!me) return
    setBuyErr(null)
    setBuying(certCode)
    try {
      await startCheckout({
        product: 'exam',
        email: me.user.email,
        cert: certCode,
        first: me.user.first_name ?? undefined,
        last: me.user.last_name ?? undefined,
      })
    } catch (e) {
      setBuyErr(checkoutErrorMessage(e))
      setBuying(null)
    }
  }

  if (loading) return <Card><Spinner /></Card>
  if (error) return <Card><ErrorNote>{error}</ErrorNote></Card>
  const rows = data?.rows ?? []
  if (rows.length === 0) return <Card><Empty>No certifications are open for enrolment right now.</Empty></Card>

  return (
    <>
    {buyErr && <div className="notice err" role="alert">{buyErr}</div>}
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
              <button className="btn sm cert-tile-cta" disabled={buying !== null} onClick={() => buy(c.code)}>
                {buying === c.code ? 'Opening checkout…' : 'Pay exam fee'}
              </button>
            )}
          </div>
        )
      })}
    </div>
    </>
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
  // User-level holds that pause the Schedule button on every entitlement (payment-level items are
  // expressed by whether an entitlement card exists at all).
  const holds = me.lifecycle.blocking_items.filter((b) => b in HOLD_LABELS)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1.75rem' }}>
      <div>
        <h1>Certifications &amp; exams</h1>
        <p className="muted">Schedule your exams, track each credential you hold, and enrol in more.</p>
      </div>

      <IdentityCard doc={me.identity_document} onChanged={refetch} />

      <section className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <h2 className="section-title">Your certifications</h2>
        {me.exams.length === 0 ? (
          <Card title="How exam scheduling works">
            <ul className="steps">
              <li className="current">
                <span className="dot">1</span>
                <span>
                  <span className="label">Pay the exam fee</span>
                  <div className="detail">Choose a certification below — your entitlement appears here immediately after payment.</div>
                </span>
              </li>
              <li>
                <span className="dot">2</span>
                <span>
                  <span className="label">Complete eligibility</span>
                  <div className="detail">Upload your government-issued ID above, accept the exam consents on your Overview page and complete your profile.</div>
                </span>
              </li>
              <li>
                <span className="dot">3</span>
                <span>
                  <span className="label">Schedule your sitting</span>
                  <div className="detail">The button below unlocks — pick any slot up to your deadline, reschedule online if plans change.</div>
                </span>
              </li>
            </ul>
            <div style={{ marginTop: '1rem' }}>
              <button className="btn sm" disabled title="Available after your exam fee is paid">Schedule exam</button>
              <div className="muted small" style={{ marginTop: '.5rem' }}>
                Unlocks automatically once your exam fee is paid — or waived by the institute.
              </div>
            </div>
          </Card>
        ) : (
          me.exams.map((e) => <EntryCard key={e.payment_id} entry={e} onChanged={refetch} holds={holds} />)
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
