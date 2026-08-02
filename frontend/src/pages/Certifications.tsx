import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useMe } from '../data/MeContext'
import { useQuery } from '../api/hooks'
import { api, ApiError } from '../api/client'
import { startCheckout, checkoutErrorMessage } from '../api/checkout'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty } from '../components/ui'
import { ViewDownloadActions } from '../components/documents/DocumentActions'
import { studentToken } from '../files'
import { fmtDate, fmtDateTime, fmtMoney, daysUntil } from '../format'
import { useT, useI18n } from '../i18n'
import { PageHeader } from '../components/premium'
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

// Fixed daily slot times, identical to the classic panel's scheduler — there is no
// backend availability endpoint; the server validates the chosen instant on book.
const SLOT_TIMES = ['09:00', '10:30', '12:00', '14:00', '15:30', '17:00']

/** The scheduling window: a month calendar + slot grid in a modal, replacing the old bare
 * datetime-local input so booking happens fully inside this panel (never the classic one). */
function ScheduleForm({ entry, onDone, onClose, mode = 'book' }: { entry: ExamEntry; onDone: () => void; onClose: () => void; mode?: 'book' | 'reschedule' }) {
  const { t, lang } = useI18n()
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const tz = Intl.DateTimeFormat().resolvedOptions().timeZone

  // Earliest bookable instant is 2 h out (server enforces the same); latest is the entitlement deadline.
  const minMs = Date.now() + 2 * 3600_000
  // Backend deadlines are 'yyyy-MM-dd HH:mm:ss' (no zone). Treat them as UTC. Do not append 'Z'
  // when one is already present — '…Z' + 'Z' parses as NaN and disables every calendar day.
  const maxMs = (() => {
    if (!entry.deadline) return Number.POSITIVE_INFINITY
    const raw = String(entry.deadline).trim()
    const normalized = /Z$/i.test(raw) ? raw : raw.includes('T') ? `${raw}Z` : `${raw.replace(' ', 'T')}Z`
    const parsed = Date.parse(normalized)
    return Number.isFinite(parsed) ? parsed : Number.POSITIVE_INFINITY
  })()
  const slotMs = (day: Date, hm: string) => {
    const [h, m] = hm.split(':').map(Number)
    return new Date(day.getFullYear(), day.getMonth(), day.getDate(), h, m).getTime()
  }
  const daySlots = (day: Date) => SLOT_TIMES.filter((s) => { const ms = slotMs(day, s); return ms >= minMs && ms <= maxMs })

  // Open on the month of the first day that still has a bookable slot — late on the last day of a
  // month, "now + 2 h" sits past the final slot time, and the month it falls in would render with
  // zero clickable days.
  const firstBookable = (() => {
    const d = new Date(minMs)
    const sameDay = new Date(d.getFullYear(), d.getMonth(), d.getDate())
    return daySlots(sameDay).length > 0 ? sameDay : new Date(d.getFullYear(), d.getMonth(), d.getDate() + 1)
  })()
  const [month, setMonth] = useState(() => new Date(firstBookable.getFullYear(), firstBookable.getMonth(), 1))
  const [selDay, setSelDay] = useState<Date | null>(null)
  const [selSlot, setSelSlot] = useState<string | null>(null)

  async function submit() {
    if (!selDay || !selSlot) return
    setError(null)
    setBusy(true)
    try {
      await api.post(mode === 'reschedule' ? '/api/me/exam/reschedule' : '/api/me/exam/book', {
        certification_id: entry.certification_id,
        scheduled_at: new Date(slotMs(selDay, selSlot)).toISOString(),
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

  // Calendar grid: Monday-first weeks, localised labels via Intl (2024-01-01 was a Monday).
  const weekdays = Array.from({ length: 7 }, (_, i) => new Intl.DateTimeFormat(lang, { weekday: 'short' }).format(new Date(2024, 0, 1 + i)))
  const monthLabel = new Intl.DateTimeFormat(lang, { month: 'long', year: 'numeric' }).format(month)
  const firstWd = (new Date(month.getFullYear(), month.getMonth(), 1).getDay() + 6) % 7
  const daysInMonth = new Date(month.getFullYear(), month.getMonth() + 1, 0).getDate()
  const cells: (Date | null)[] = [
    ...Array.from({ length: firstWd }, () => null),
    ...Array.from({ length: daysInMonth }, (_, i) => new Date(month.getFullYear(), month.getMonth(), i + 1)),
  ]
  const sameDay = (a: Date, b: Date | null) => !!b && a.getFullYear() === b.getFullYear() && a.getMonth() === b.getMonth() && a.getDate() === b.getDate()
  const selectedLabel = selDay && selSlot
    ? `${new Intl.DateTimeFormat(lang, { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' }).format(selDay)} · ${selSlot}`
    : null

  return (
    <div className="schedm-backdrop" onClick={(e) => { if (e.target === e.currentTarget) onClose() }}>
      <div className="schedm" role="dialog" aria-modal="true" aria-label={mode === 'reschedule' ? t('cert.reschedule') : t('cert.scheduleExam')}>
        <div className="schedm-head">
          <div>
            <h3 style={{ margin: 0 }}>{mode === 'reschedule' ? t('cert.reschedule') : t('cert.scheduleExam')}</h3>
            <div className="muted small">{entry.certification_name || t('cert.untitledCert', { id: entry.certification_id })}</div>
          </div>
          <button className="btn sm secondary" onClick={onClose}>{t('cert.close')}</button>
        </div>
        {error && <div className="notice err" role="alert">{error}</div>}
        <div className="schedm-body">
          <div>
            <div className="schedm-cal-head">
              <button className="btn sm secondary" aria-label={t('cert.sched.prevMonth')} onClick={() => setMonth(new Date(month.getFullYear(), month.getMonth() - 1, 1))}>‹</button>
              <strong>{monthLabel}</strong>
              <button className="btn sm secondary" aria-label={t('cert.sched.nextMonth')} onClick={() => setMonth(new Date(month.getFullYear(), month.getMonth() + 1, 1))}>›</button>
            </div>
            <div className="schedm-cal" role="grid">
              {weekdays.map((w) => <span key={w} className="schedm-wd">{w}</span>)}
              {cells.map((d, i) => d === null
                ? <span key={`b${i}`} />
                : (
                  <button
                    key={d.getDate()}
                    className={'schedm-day' + (sameDay(d, selDay) ? ' sel' : '')}
                    disabled={daySlots(d).length === 0}
                    onClick={() => { setSelDay(d); setSelSlot(null) }}
                  >
                    {d.getDate()}
                  </button>
                ))}
            </div>
          </div>
          <div>
            <div className="small" style={{ fontWeight: 700, marginBottom: '.4rem' }}>{selDay ? t('cert.sched.pickTime') : t('cert.sched.pickDay')}</div>
            {selDay && (
              daySlots(selDay).length > 0 ? (
                <div className="schedm-slots">
                  {daySlots(selDay).map((s) => (
                    <button key={s} className={'schedm-slot' + (s === selSlot ? ' sel' : '')} onClick={() => setSelSlot(s)}>{s}</button>
                  ))}
                </div>
              ) : <div className="muted small">{t('cert.sched.noSlots')}</div>
            )}
            <p className="muted small" style={{ marginTop: '.8rem' }}>{t('cert.sched.tzNote', { tz })}</p>
            <p className="muted small">{t('cert.sched.policy')}</p>
            {entry.deadline && <p className="muted small">{t('cert.schedulingDeadline')} {fmtDate(entry.deadline)}</p>}
          </div>
        </div>
        <div className="schedm-foot">
          <div className="small muted">{selectedLabel ? t('cert.sched.selected', { when: selectedLabel }) : ''}</div>
          <button className="btn" disabled={!selDay || !selSlot || busy} onClick={submit}>
            {busy ? t('cert.scheduling') : t('cert.confirmSlot')}
          </button>
        </div>
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
        <div className="row" style={{ marginTop: '.75rem', gap: '.35rem', flexWrap: 'wrap' }}>
          {/* Students can always see what is on file for them (served over the authenticated
              own-document endpoint; staff views of the same file are separately audited). */}
          <ViewDownloadActions
            info={{ title: doc?.filename || t('cert.identityVerification'), filename: doc?.filename }}
            inlineUrl="/api/me/identity-document/file"
            token={studentToken()}
            canDownload={false}
            canPrint={false}
          />
          <button className="btn sm secondary" onClick={() => setReplacing(true)}>{t('cert.replaceDocument')}</button>
        </div>
      )}
    </Card>
  )
}

// Exam-day incident categories a candidate can report (POST /api/me/exam/incident). Values mirror
// the backend enum; labels are i18n keys resolved with t().
const INCIDENT_CATEGORIES = [
  { value: 'exam_wont_launch', labelKey: 'cert.issueCat.examWontLaunch' },
  { value: 'identity_failed', labelKey: 'cert.issueCat.identityFailed' },
  { value: 'proctor_absent', labelKey: 'cert.issueCat.proctorAbsent' },
  { value: 'disconnection', labelKey: 'cert.issueCat.disconnection' },
  { value: 'power_failure', labelKey: 'cert.issueCat.powerFailure' },
  { value: 'browser_crash', labelKey: 'cert.issueCat.browserCrash' },
  { value: 'submission_failure', labelKey: 'cert.issueCat.submissionFailure' },
  { value: 'timer_malfunction', labelKey: 'cert.issueCat.timerMalfunction' },
  { value: 'wrong_exam', labelKey: 'cert.issueCat.wrongExam' },
  { value: 'content_failed', labelKey: 'cert.issueCat.contentFailed' },
  { value: 'system_outage', labelKey: 'cert.issueCat.systemOutage' },
  { value: 'illness', labelKey: 'cert.issueCat.illness' },
  { value: 'other', labelKey: 'cert.issueCat.other' },
]

/** Report an exam-day problem for investigation. On success the backend returns a reference the
 *  candidate can quote to support. */
function IncidentReportForm({ certificationId, onClose }: { certificationId: number; onClose: () => void }) {
  const t = useT()
  const [category, setCategory] = useState('exam_wont_launch')
  const [details, setDetails] = useState('')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [reference, setReference] = useState<string | null>(null)

  async function submit() {
    setBusy(true)
    setError(null)
    try {
      const r = await api.post<{ reference?: string; ref?: string }>('/api/me/exam/incident', {
        category,
        certification_id: certificationId,
        student_explanation: details,
      })
      setReference(r.reference || r.ref || '')
    } catch (e) {
      setError(e instanceof Error ? e.message : t('cert.issueFailed'))
    } finally {
      setBusy(false)
    }
  }

  if (reference !== null) {
    return (
      <div className="notice ok" role="status" style={{ marginTop: '.6rem' }}>
        {t('cert.issueSubmitted', { ref: reference || '—' })}
      </div>
    )
  }

  return (
    <div className="stack" style={{ marginTop: '.6rem', display: 'grid', gap: '.6rem' }}>
      <p className="muted small" style={{ margin: 0 }}>{t('cert.reportIssueIntro')}</p>
      {error && <div className="notice err" role="alert">{error}</div>}
      <div className="field" style={{ margin: 0 }}>
        <label htmlFor={`inc-cat-${certificationId}`}>{t('cert.issueCategory')}</label>
        <select id={`inc-cat-${certificationId}`} value={category} onChange={(e) => setCategory(e.target.value)}>
          {INCIDENT_CATEGORIES.map((c) => <option key={c.value} value={c.value}>{t(c.labelKey)}</option>)}
        </select>
      </div>
      <div className="field" style={{ margin: 0 }}>
        <label htmlFor={`inc-det-${certificationId}`}>{t('cert.issueDetails')}</label>
        <textarea id={`inc-det-${certificationId}`} value={details} onChange={(e) => setDetails(e.target.value)} rows={3} placeholder={t('cert.issueDetailsPlaceholder')} />
      </div>
      <div className="row">
        <button className="btn sm" disabled={busy || !details.trim()} onClick={submit}>{busy ? t('cert.submittingIssue') : t('cert.submitIssue')}</button>
        <button className="btn sm secondary" onClick={onClose}>{t('cert.close')}</button>
      </div>
    </div>
  )
}

function EntryCard({ entry, onChanged, holds }: { entry: ExamEntry; onChanged: () => void; holds: string[] }) {
  const t = useT()
  const [scheduling, setScheduling] = useState(false)
  const [reporting, setReporting] = useState(false)
  const booking = entry.booking as Record<string, unknown> | null
  const attempt = entry.latest_attempt as Record<string, unknown> | null
  const cred = entry.credential
  // Is this certification's exam delivered by an external vendor? (Delivery-mode switch in the admin.)
  const delivery = useQuery<{ routed: boolean; provider_name?: string; provider?: string; status?: string; self_schedule?: boolean; confirmation?: string | null; result_status?: string | null }>(
    booking ? `/api/me/exam/delivery?certification_id=${entry.certification_id}` : null)
  const vendor = delivery.data?.routed ? delivery.data : null
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
      action={
        <span className="row" style={{ gap: '.35rem' }}>
          {entry.scheduling_status && <StatusBadge status={entry.scheduling_status} />}
          <Badge tone={state.tone}>{state.label}</Badge>
        </span>
      }
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
        {entry.extended && <div>{t('cert.extendedNewDeadline', { date: fmtDate(entry.deadline) })}</div>}
        {entry.attempts_permitted != null && (
          <div>{t('cert.attemptsSummary', { used: entry.attempts_used ?? 0, remaining: Math.max(0, (entry.attempts_permitted ?? 0) - (entry.attempts_used ?? 0)) })}</div>
        )}
      </div>

      {entry.waiver && (
        <div className="notice" style={{ marginTop: '.6rem' }}>{t('cert.waiverApplied')}</div>
      )}

      {cred?.status === 'active' ? (
        <div className="notice" style={{ marginTop: '.75rem' }}>
          {t('cert.credentialLabel')} <strong>{cred.credential_id}</strong> · {t('cert.expiresOn', { date: fmtDate(cred.expires_at) })}
        </div>
      ) : booking ? (
        <div className="notice" style={{ marginTop: '.75rem' }}>
          {t('cert.examScheduledFor')} <strong>{fmtDateTime(booking.scheduled_at, booking.timezone)}</strong>
          {booking.timezone ? ` (${booking.timezone})` : ''}.
          {vendor ? (
            <div style={{ marginTop: '.55rem' }}>
              <div className="small">Delivered by <strong>{vendor.provider_name || vendor.provider}</strong>
                {vendor.confirmation ? <> · confirmation <strong>{vendor.confirmation}</strong></> : null}
              </div>
              {vendor.self_schedule && (
                <div className="small muted" style={{ marginTop: '.3rem' }}>
                  Your exam eligibility has been sent to {vendor.provider_name || 'the test provider'}. Book your seat or online slot using the scheduling link they email you; your result posts back here automatically.
                </div>
              )}
              {vendor.result_status && (
                <div className="small" style={{ marginTop: '.3rem' }}>Vendor result: <strong>{vendor.result_status}</strong></div>
              )}
              <div className="row" style={{ marginTop: '.6rem', flexWrap: 'wrap' }}>
                <button className="btn sm secondary" onClick={() => setScheduling((v) => !v)}>
                  {scheduling ? t('cert.close') : t('cert.reschedule')}
                </button>
              </div>
            </div>
          ) : (
            <div className="row" style={{ marginTop: '.6rem', flexWrap: 'wrap' }}>
              <Link className="btn sm" to={`/exam-day/${entry.certification_id}`}>{t('cert.examDayCheckIn')}</Link>
              <button className="btn sm secondary" onClick={() => setScheduling((v) => !v)}>
                {scheduling ? t('cert.close') : t('cert.reschedule')}
              </button>
            </div>
          )}
          {scheduling && (
            <ScheduleForm entry={entry} mode="reschedule" onDone={() => { setScheduling(false); onChanged() }} onClose={() => setScheduling(false)} />
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
                <Link className="btn sm secondary" to="/results">{t('cert.viewFullResult')}</Link>
                {failed && <Link className="btn sm" to="/billing">{t('cert.buyRetake')} →</Link>}
              </div>
            </div>
          )
        })()
      ) : (
        <div style={{ marginTop: '.75rem' }}>
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
          {scheduling && (
            <ScheduleForm entry={entry} onDone={() => { setScheduling(false); onChanged() }} onClose={() => setScheduling(false)} />
          )}
        </div>
      )}

      <div style={{ marginTop: '.9rem', borderTop: '1px solid var(--line, #e5e7eb)', paddingTop: '.6rem' }}>
        {reporting ? (
          <IncidentReportForm certificationId={entry.certification_id} onClose={() => setReporting(false)} />
        ) : (
          <button className="btn ghost sm" onClick={() => setReporting(true)}>{t('cert.reportIssue')}</button>
        )}
      </div>
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
  // The exam fee can only be paid once membership is active. A student without membership is asked to
  // pay the membership fee first (or use the membership + exam bundle, on Billing, to pay both together).
  const memberActive = me?.lifecycle.membership_status === 'active'

  async function buy(certCode: string) {
    if (!me) return
    if (!memberActive) {
      setBuyErr(t('cert.membershipRequired'))
      return
    }
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

  // Pay the membership fee and this certification's exam fee together, in one checkout.
  async function buyBundle(certCode: string) {
    if (!me) return
    setBuyErr(null)
    setBuying(certCode)
    try {
      await startCheckout({
        product: 'bundle',
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
    {!memberActive && (
      <div className="notice" role="status" style={{ display: 'flex', flexWrap: 'wrap', gap: '.5rem', alignItems: 'center', justifyContent: 'space-between' }}>
        <span>{t('cert.membershipRequired')}</span>
        <Link className="btn sm" to="/billing">{t('cert.payMembershipFee')}</Link>
      </div>
    )}
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
            ) : !memberActive ? (
              <>
                <button className="btn sm cert-tile-cta" disabled={buying !== null} onClick={() => buyBundle(c.code)}>
                  {buying === c.code ? t('cert.openingCheckout') : t('cert.payMembershipAndExam')}
                </button>
                <div className="muted" style={{ fontSize: '.72rem', marginTop: '.3rem' }}>{t('cert.bundleAddsMembership')}</div>
              </>
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
    <div className="page" style={{ gap: '1.75rem' }}>
      <PageHeader eyebrow={t('nav.certifications')} title={t('cert.title')} subtitle={t('cert.subtitle')} />

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
