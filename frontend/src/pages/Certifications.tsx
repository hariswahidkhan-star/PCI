import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useMe } from '../data/MeContext'
import { useQuery } from '../api/hooks'
import { api, ApiError, getToken } from '../api/client'
import { startCheckout, checkoutErrorMessage } from '../api/checkout'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate, fmtDateTime, fmtMoney, daysUntil } from '../format'
import { useT } from '../i18n'
import type { ExamEntry, IdentityDocument } from '../api/types'

/** A certification from the public, backend-controlled catalogue (GET /api/certifications). */
interface CatalogueCert {
  id: number
  code: string
  name: string
  acronym?: string | null
  short_description?: string | null
  slug?: string | null
  description?: string | null
  expiry_years?: number | null
  duration_minutes?: number | null
  pass_mark_pct?: number | null
  exam_price?: number | null
}

// Maps a backend error code to its i18n key; resolved with t() at the point of display.
const BOOK_ERRORS: Record<string, string> = {
  no_booking: 'cert.bookErr.noBooking',
  locked: 'cert.bookErr.locked',
  max_reschedules: 'cert.bookErr.maxReschedules',
  no_entitlement: 'cert.bookErr.noEntitlement',
  not_eligible: 'cert.bookErr.notEligible',
  window_lapsed: 'cert.bookErr.windowLapsed',
  already_booked: 'cert.bookErr.alreadyBooked',
  payment_already_used: 'cert.bookErr.paymentAlreadyUsed',
  exam_already_taken: 'cert.bookErr.examAlreadyTaken',
  bad_slot: 'cert.bookErr.badSlot',
  beyond_window: 'cert.bookErr.beyondWindow',
}

// User-level eligibility holds (from lifecycle.blocking_items) that pause the Schedule button —
// payment-level items like exam_fee_unpaid are handled by whether an entitlement card exists at all.
// Values are i18n keys, resolved with t() where the hold is displayed.
const HOLD_LABELS: Record<string, string> = {
  identity_document_missing: 'cert.hold.identityDocMissing',
  consents_pending: 'cert.hold.consentsPending',
  profile_incomplete: 'cert.hold.profileIncomplete',
  account_hold: 'cert.hold.accountHold',
  booking_closed: 'cert.hold.bookingClosed',
}

// Holds that are states with no student action to take. They read wrongly under the imperative
// "To unlock scheduling:" frame, so they are shown under a "Scheduling is unavailable:" frame instead.
const STATE_HOLDS = new Set(['account_hold', 'booking_closed'])

function ScheduleForm({ entry, onDone, mode = 'book' }: { entry: ExamEntry; onDone: () => void; mode?: 'book' | 'reschedule' }) {
  const t = useT()
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
      setError(BOOK_ERRORS[code] ? t(BOOK_ERRORS[code]) : (e instanceof Error ? e.message : t('cert.unableToSchedule')))
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
        <label htmlFor="sched-when">{t('cert.chooseDateTime', { tz })}</label>
        <input id="sched-when" type="datetime-local" value={when} min={min} max={max} onChange={(e) => setWhen(e.target.value)} />
      </div>
      <div className="row">
        <button className="btn sm" disabled={!when || busy} onClick={submit}>{busy ? t('cert.scheduling') : t('cert.confirmSlot')}</button>
      </div>
    </div>
  )
}

// Values are i18n keys, resolved with t() where the error is displayed.
const UPLOAD_ERRORS: Record<string, string> = {
  file_type_not_allowed: 'cert.uploadErr.fileType',
  file_too_large: 'cert.uploadErr.fileTooLarge',
  content_mime_mismatch: 'cert.uploadErr.mimeMismatch',
  too_many_uploads: 'cert.uploadErr.tooMany',
}

const DOC_KINDS = [
  { value: 'passport', labelKey: 'cert.docKind.passport' },
  { value: 'national_id', labelKey: 'cert.docKind.nationalId' },
  { value: 'driving_licence', labelKey: 'cert.docKind.drivingLicence' },
  { value: 'other', labelKey: 'cert.docKind.other' },
]

/** Government-ID upload + status. A document on file (not rejected) is required before any exam
 *  can be scheduled — the backend enforces this in Lifecycle.BookingBlockers. */
function IdentityCard({ doc, onChanged }: { doc: IdentityDocument | null; onChanged: () => void }) {
  const t = useT()
  const [kind, setKind] = useState('passport')
  const [file, setFile] = useState<File | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [replacing, setReplacing] = useState(false)

  const showForm = !doc || doc.status === 'rejected' || replacing

  async function submit() {
    if (!file) return
    if (file.size > 3_000_000) { setErr(t(UPLOAD_ERRORS.file_too_large)); return }
    setBusy(true)
    setErr(null)
    try {
      const dataUri = await new Promise<string>((resolve, reject) => {
        const r = new FileReader()
        r.onload = () => resolve(String(r.result))
        r.onerror = () => reject(new Error(t('cert.fileReadError')))
        r.readAsDataURL(file)
      })
      await api.post('/api/me/identity-document', { doc_kind: kind, filename: file.name, data_uri: dataUri })
      setFile(null)
      setReplacing(false)
      onChanged()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setErr(UPLOAD_ERRORS[code] ? t(UPLOAD_ERRORS[code]) : (e instanceof Error ? e.message : t('cert.uploadFailed')))
    } finally {
      setBusy(false)
    }
  }

  const badge =
    !doc ? <Badge tone="warn">{t('cert.badgeRequired')}</Badge>
    : doc.status === 'verified' ? <Badge tone="ok">{t('cert.badgeVerified')}</Badge>
    : doc.status === 'rejected' ? <Badge tone="err">{t('cert.badgeRejected')}</Badge>
    : <Badge tone="brand">{t('cert.badgeSubmitted')}</Badge>

  return (
    <Card title={t('cert.identityVerification')} action={badge}>
      {!doc && (
        <p className="muted small" style={{ marginTop: 0 }}>
          {t('cert.idIntro')}
        </p>
      )}
      {doc && doc.status === 'rejected' && (
        <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>
          {t('cert.docRejectedLead')}{doc.review_note ? ` — ${doc.review_note}` : ''}{t('cert.docRejectedTail')}
        </div>
      )}
      {doc && doc.status !== 'rejected' && (
        <div className="small stack" style={{ gap: '.2rem' }}>
          <div><span className="muted">{t('cert.documentLabel')}</span> <strong>{(() => { const dk = DOC_KINDS.find((k) => k.value === doc.doc_kind); return dk ? t(dk.labelKey) : doc.doc_kind })()}</strong>{doc.filename ? ` · ${doc.filename}` : ''}</div>
          <div className="muted">{t('cert.uploadedOn', { date: fmtDate(doc.created_at) })}{doc.status === 'submitted' ? t('cert.pendingReviewSuffix') : ''}</div>
        </div>
      )}

      {showForm ? (
        <div className="stack" style={{ marginTop: doc ? '.75rem' : 0 }}>
          {err && <div className="notice err" role="alert">{err}</div>}
          <div className="field" style={{ margin: 0 }}>
            <label htmlFor="idd-kind">{t('cert.documentType')}</label>
            <select id="idd-kind" value={kind} onChange={(e) => setKind(e.target.value)}>
              {DOC_KINDS.map((k) => <option key={k.value} value={k.value}>{t(k.labelKey)}</option>)}
            </select>
          </div>
          <div className="field" style={{ margin: 0 }}>
            <label htmlFor="idd-file">{t('cert.fileFieldLabel')}</label>
            <input id="idd-file" type="file" accept=".jpg,.jpeg,.png,.webp,.pdf,image/jpeg,image/png,image/webp,application/pdf"
              onChange={(e) => setFile(e.target.files?.[0] ?? null)} />
          </div>
          <div className="row">
            <button className="btn sm" disabled={!file || busy} onClick={submit}>{busy ? t('cert.uploading') : t('cert.submitDocument')}</button>
            {replacing && <button className="btn sm secondary" onClick={() => { setReplacing(false); setErr(null) }}>{t('cert.cancel')}</button>}
          </div>
        </div>
      ) : (
        <div className="row" style={{ marginTop: '.75rem' }}>
          <button className="btn sm secondary" onClick={() => setReplacing(true)}>{t('cert.replaceDocument')}</button>
        </div>
      )}
    </Card>
  )
}

function EntryCard({ entry, onChanged, holds }: { entry: ExamEntry; onChanged: () => void; holds: string[] }) {
  const t = useT()
  const [scheduling, setScheduling] = useState(false)
  const booking = entry.booking as Record<string, unknown> | null
  const attempt = entry.latest_attempt as Record<string, unknown> | null
  const cred = entry.credential
  const deadlineDays = daysUntil(entry.deadline)
  const actionHolds = holds.filter((h) => !STATE_HOLDS.has(h))
  const stateHolds = holds.filter((h) => STATE_HOLDS.has(h))

  let state: { tone: 'ok' | 'warn' | 'brand' | 'neutral' | 'err'; label: string }
  if (cred?.status === 'active') state = { tone: 'ok', label: t('cert.stateCertified') }
  else if (attempt) state = { tone: 'brand', label: t('cert.stateExamTaken') }
  else if (booking) state = { tone: 'warn', label: t('cert.stateScheduled') }
  else state = { tone: 'neutral', label: t('cert.stateNotScheduled') }

  return (
    <Card
      title={entry.certification_name || t('cert.untitledCert', { id: entry.certification_id })}
      action={<Badge tone={state.tone}>{state.label}</Badge>}
    >
      <div className="small muted stack">
        {entry.certification_code && <div>{t('cert.codeLabel')} <strong>{entry.certification_code}</strong></div>}
        {entry.reference && <div>{t('cert.paymentRef')} {entry.reference}</div>}
        {entry.deadline && (
          <div>
            {t('cert.schedulingDeadline')} {fmtDate(entry.deadline)}
            {deadlineDays !== null && deadlineDays >= 0 && <> · {deadlineDays === 1 ? t('cert.oneDayLeft') : t('cert.daysLeft', { n: deadlineDays })}</>}
          </div>
        )}
      </div>

      {cred?.status === 'active' ? (
        <div className="notice" style={{ marginTop: '.75rem' }}>
          {t('cert.credentialLabel')} <strong>{cred.credential_id}</strong> · {t('cert.expiresOn', { date: fmtDate(cred.expires_at) })}
        </div>
      ) : booking ? (
        <div className="notice" style={{ marginTop: '.75rem' }}>
          {t('cert.examScheduledFor')} <strong>{fmtDateTime(booking.scheduled_at)}</strong>
          {booking.timezone ? ` (${booking.timezone})` : ''}.
          <div className="row" style={{ marginTop: '.6rem', flexWrap: 'wrap' }}>
            <a className="btn sm" href={`/student.html#t=${getToken() ?? ''}`}>{t('cert.examDayCheckIn')} ↗</a>
            <button className="btn sm secondary" onClick={() => setScheduling((v) => !v)}>
              {scheduling ? t('cert.close') : t('cert.reschedule')}
            </button>
          </div>
          {scheduling && (
            <div style={{ marginTop: '.6rem' }}>
              <ScheduleForm entry={entry} mode="reschedule" onDone={() => { setScheduling(false); onChanged() }} />
            </div>
          )}
        </div>
      ) : attempt ? (
        (() => {
          const rs = String(attempt.result_status || attempt.status || '')
          const res = String(attempt.result || '')          // redacted to '' while a result is held
          const passed = rs === 'credential_issued' || res === 'pass'
          const failed = res === 'fail' && !passed
          const underReview = !passed && !failed
          return (
            <div className="notice" style={{ marginTop: '.75rem' }}>
              <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap' }}>
                <span className="muted small">{t('cert.latestAttempt')}</span> <StatusBadge status={rs} />
              </div>
              {underReview && (
                <div className="muted small" style={{ marginTop: '.5rem' }}>
                  {t('cert.resultFinalising')}
                </div>
              )}
              {failed && (
                <div className="muted small" style={{ marginTop: '.5rem' }}>
                  {t('cert.retakeInfo')}
                </div>
              )}
              <div className="row" style={{ marginTop: '.6rem', flexWrap: 'wrap' }}>
                <a className="btn sm secondary" href={`/student.html#t=${getToken() ?? ''}`}>{t('cert.viewFullResult')} ↗</a>
                {failed && <Link className="btn sm" to="/billing">{t('cert.buyRetake')} →</Link>}
              </div>
            </div>
          )
        })()
      ) : (
        <div style={{ marginTop: '.75rem' }}>
          {scheduling ? (
            <ScheduleForm entry={entry} onDone={() => { setScheduling(false); onChanged() }} />
          ) : (
            <>
              <button className="btn sm" disabled={holds.length > 0} onClick={() => setScheduling(true)}>{t('cert.scheduleExam')}</button>
              {actionHolds.length > 0 && (
                <div className="muted small" style={{ marginTop: '.5rem' }}>
                  {t('cert.toUnlockScheduling')} {actionHolds.map((h) => HOLD_LABELS[h] ? t(HOLD_LABELS[h]) : h.replace(/_/g, ' ')).join(' · ')}.
                </div>
              )}
              {stateHolds.length > 0 && (
                <div className="muted small" style={{ marginTop: '.5rem' }}>
                  {t('cert.schedulingUnavailable')} {stateHolds.map((h) => HOLD_LABELS[h] ? t(HOLD_LABELS[h]) : h.replace(/_/g, ' ')).join(' · ')}.
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
  const t = useT()
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
  if (rows.length === 0) return <Card><Empty>{t('cert.catalogueEmpty')}</Empty></Card>

  return (
    <>
    {buyErr && <div className="notice err" role="alert">{buyErr}</div>}
    <div className="cert-catalogue-grid">
      {rows.map((c) => {
        const owned = ownedCodes.has((c.code || '').toUpperCase())
        return (
          <div className="cert-tile" key={c.id}>
            <div className="cert-tile-head">
              <span className="cert-tile-code">{c.acronym || c.code}</span>
              {owned && <Badge tone="ok">{t('cert.enrolled')}</Badge>}
            </div>
            <h3 className="cert-tile-name">{c.name}</h3>
            {(c.short_description || c.description) && <p className="muted small cert-tile-desc">{c.short_description || c.description}</p>}
            <ul className="cert-tile-meta">
              {c.exam_price != null && <li><strong>{fmtMoney(c.exam_price)}</strong> {t('cert.examFee')}</li>}
              {c.duration_minutes != null && c.pass_mark_pct != null && (
                <li>{c.duration_minutes} {t('cert.minAbbrev')} · {t('cert.passMark', { pct: c.pass_mark_pct })}</li>
              )}
              {c.expiry_years != null && <li>{c.expiry_years === 1 ? t('cert.validOneYear') : t('cert.validYears', { n: c.expiry_years })}</li>}
            </ul>
            {owned ? (
              <span className="btn sm secondary cert-tile-cta" aria-disabled="true" style={{ opacity: 0.6, pointerEvents: 'none' }}>{t('cert.alreadyEnrolled')}</span>
            ) : (
              <button className="btn sm cert-tile-cta" disabled={buying !== null} onClick={() => buy(c.code)}>
                {buying === c.code ? t('cert.openingCheckout') : t('cert.payExamFee')}
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
  const t = useT()
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
        <h1>{t('cert.title')}</h1>
        <p className="muted">{t('cert.subtitle')}</p>
      </div>

      <IdentityCard doc={me.identity_document} onChanged={refetch} />

      <section className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <h2 className="section-title">{t('cert.yourCertifications')}</h2>
        {me.exams.length === 0 ? (
          <Card title={t('cert.howSchedulingWorks')}>
            <ul className="steps">
              <li className="current">
                <span className="dot">1</span>
                <span>
                  <span className="label">{t('cert.step1Label')}</span>
                  <div className="detail">{t('cert.step1Detail')}</div>
                </span>
              </li>
              <li>
                <span className="dot">2</span>
                <span>
                  <span className="label">{t('cert.step2Label')}</span>
                  <div className="detail">{t('cert.step2Detail')}</div>
                </span>
              </li>
              <li>
                <span className="dot">3</span>
                <span>
                  <span className="label">{t('cert.step3Label')}</span>
                  <div className="detail">{t('cert.step3Detail')}</div>
                </span>
              </li>
            </ul>
            <div style={{ marginTop: '1rem' }}>
              <button className="btn sm" disabled title={t('cert.availableAfterPaid')}>{t('cert.scheduleExam')}</button>
              <div className="muted small" style={{ marginTop: '.5rem' }}>
                {t('cert.unlocksAutomatically')}
              </div>
            </div>
          </Card>
        ) : (
          me.exams.map((e) => <EntryCard key={e.payment_id} entry={e} onChanged={refetch} holds={holds} />)
        )}
      </section>

      <section className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <div>
          <h2 className="section-title">{t('cert.exploreCertifications')}</h2>
          <p className="muted small" style={{ margin: '.25rem 0 0' }}>{t('cert.exploreSubtitle')}</p>
        </div>
        <Catalogue ownedCodes={ownedCodes} />
      </section>
    </div>
  )
}
