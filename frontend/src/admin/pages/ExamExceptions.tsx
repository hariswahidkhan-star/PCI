import { useState, useEffect, type ReactNode, type CSSProperties } from 'react'
import { useAdminQuery } from '../hooks'
import { useAdminAuth } from '../AdminAuth'
import {
  adminApi,
  type ExamExceptionRow,
  type ExamExceptionDetail,
  type ExamIncidentRow,
  type ExamWindowRule,
} from '../api'
import { Card, StatusBadge, Badge, Spinner, ErrorNote, Empty, rowActivate } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtDate, fmtDateTime, titleCase } from '../../format'

// ---------------------------------------------------------------- shared helpers

const sv = (v: unknown): string => (v == null || v === '' ? '—' : String(v))
const num = (v: string): number | undefined => (v.trim() === '' ? undefined : Number(v))
// datetime-local yields local wall-clock "YYYY-MM-DDTHH:mm"; send as ISO so the server anchors it.
const toIso = (local: string): string | undefined => (local ? new Date(local).toISOString() : undefined)

type Note = { ok: boolean; text: string } | null

function Msg({ msg, style }: { msg: Note; style?: CSSProperties }) {
  if (!msg) return null
  return (
    <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginTop: '.5rem', overflowWrap: 'anywhere', ...style }}>
      {msg.text}
    </div>
  )
}

/** Small certifications catalogue used to populate filter/form dropdowns. */
interface CertOpt { id: number; code: string; name: string }
function useCertOptions(): CertOpt[] {
  const { data } = useAdminQuery<{ rows: CertOpt[] }>('/api/certifications')
  return data?.rows ?? []
}

interface HistCol { key: string; label: string; fmt?: (v: unknown) => ReactNode }
function HistoryCard({ title, rows, cols }: { title: string; rows: Record<string, unknown>[]; cols: HistCol[] }) {
  return (
    <Card title={`${title} (${rows.length})`}>
      {rows.length === 0 ? <Empty>None recorded.</Empty> : (
        <div style={{ overflowX: 'auto' }}>
          <table className="data">
            <thead><tr>{cols.map((c) => <th key={c.key}>{c.label}</th>)}</tr></thead>
            <tbody>
              {rows.map((r, i) => (
                <tr key={i}>{cols.map((c) => <td key={c.key} className="small">{c.fmt ? c.fmt(r[c.key]) : sv(r[c.key])}</td>)}</tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </Card>
  )
}

const GRANT_TYPES = ['replacement', 'additional', 'complimentary', 'paid', 'technical', 'administrative', 'special', 'appeal', 'manual'] as const
const FEE_TYPES = ['exam', 'retake', 'reschedule'] as const
const DECISIONS = [
  'no_action', 'extend_deadline', 'reopen_scheduling', 'reschedule', 'grant_replacement', 'grant_additional',
  'waive_waiting_period', 'waive_exam_fee', 'waive_reschedule_fee', 'waive_retake_fee', 'restore_attempt',
  'cancel_invalid_attempt', 'under_review', 'escalate', 'request_information', 'reject',
] as const
const CATEGORIES = [
  'exam_wont_launch', 'identity_failed', 'proctor_absent', 'disconnection', 'power_failure', 'browser_crash',
  'submission_failure', 'timer_malfunction', 'wrong_exam', 'content_failed', 'system_outage', 'illness', 'other',
] as const

// ---------------------------------------------------------------- action cards (authorization drawer)

function ExtendCard({ authId, onDone }: { authId: number; onDone: () => void }) {
  const { can } = useAdminAuth()
  const allowed = can('ex_extend')
  const [newDeadline, setNewDeadline] = useState('')
  const [newExpiry, setNewExpiry] = useState('')
  const [reason, setReason] = useState('')
  const [note, setNote] = useState('')
  const [feeApplies, setFeeApplies] = useState(false)
  const [isFree, setIsFree] = useState(false)
  const [evidence, setEvidence] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)

  async function go() {
    if (!newDeadline) { setMsg({ ok: false, text: 'A new deadline is required.' }); return }
    if (!reason.trim()) { setMsg({ ok: false, text: 'A reason is required.' }); return }
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/exam-authorizations/${authId}/extend`, {
        new_deadline: toIso(newDeadline),
        new_expiry: toIso(newExpiry),
        reason: reason.trim(),
        note: note || undefined,
        fee_applies: feeApplies,
        is_free: isFree,
        evidence_ref: evidence || undefined,
      })
      setMsg({ ok: true, text: 'Deadline extended — the student can schedule against the new date.' })
      setReason(''); setNote('')
      onDone()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not extend the deadline.' }) } finally { setBusy(false) }
  }

  return (
    <Card title="Extend deadline">
      <p className="muted small" style={{ marginTop: 0 }}>Move the scheduling deadline (and optionally the access expiry) to a later date.</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label>New deadline<input type="datetime-local" value={newDeadline} onChange={(e) => setNewDeadline(e.target.value)} /></label>
        <label>New access expiry (optional)<input type="datetime-local" value={newExpiry} onChange={(e) => setNewExpiry(e.target.value)} /></label>
        <label style={{ flex: 1, minWidth: 180 }}>Reason (required)<input value={reason} onChange={(e) => setReason(e.target.value)} placeholder="e.g. incident on exam day" /></label>
        <label style={{ flex: 1, minWidth: 160 }}>Note (optional)<input value={note} onChange={(e) => setNote(e.target.value)} /></label>
        <label>Evidence ref (optional)<input value={evidence} onChange={(e) => setEvidence(e.target.value)} style={{ maxWidth: 160 }} /></label>
      </div>
      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', marginTop: '.5rem' }}>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={feeApplies} onChange={(e) => setFeeApplies(e.target.checked)} style={{ width: 'auto' }} /> Fee applies</label>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={isFree} onChange={(e) => setIsFree(e.target.checked)} style={{ width: 'auto' }} /> Free (no fee)</label>
        <button className="btn sm" disabled={busy || !allowed} onClick={go}>{busy ? 'Extending…' : 'Extend deadline'}</button>
      </div>
      {!allowed && <p className="muted small" style={{ margin: '.4rem 0 0' }}>You do not have the extend permission.</p>}
      <Msg msg={msg} />
    </Card>
  )
}

function RescheduleCard({ authId, onDone }: { authId: number; onDone: () => void }) {
  const { can } = useAdminAuth()
  const allowed = can('ex_reschedule')
  const tz = Intl.DateTimeFormat().resolvedOptions().timeZone
  const [when, setWhen] = useState('')
  const [reason, setReason] = useState('')
  const [note, setNote] = useState('')
  const [feeApplies, setFeeApplies] = useState(false)
  const [feeWaived, setFeeWaived] = useState(false)
  const [deliveryChange, setDeliveryChange] = useState('')
  const [providerChange, setProviderChange] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)

  async function go() {
    if (!when) { setMsg({ ok: false, text: 'A new date & time is required.' }); return }
    if (!reason.trim()) { setMsg({ ok: false, text: 'A reason is required.' }); return }
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/exam-authorizations/${authId}/reschedule`, {
        scheduled_at: toIso(when),
        timezone: tz,
        reason: reason.trim(),
        note: note || undefined,
        fee_applies: feeApplies,
        fee_waived: feeWaived,
        delivery_change: deliveryChange || undefined,
        provider_change: providerChange || undefined,
      })
      setMsg({ ok: true, text: 'Exam rescheduled — the student has been notified.' })
      setReason(''); setNote('')
      onDone()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not reschedule the exam.' }) } finally { setBusy(false) }
  }

  return (
    <Card title="Reschedule">
      <p className="muted small" style={{ marginTop: 0 }}>Move the booked sitting to a new time ({tz}), optionally changing delivery mode or provider.</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label>New date &amp; time<input type="datetime-local" value={when} onChange={(e) => setWhen(e.target.value)} /></label>
        <label style={{ flex: 1, minWidth: 180 }}>Reason (required)<input value={reason} onChange={(e) => setReason(e.target.value)} placeholder="e.g. candidate request" /></label>
        <label style={{ flex: 1, minWidth: 160 }}>Note (optional)<input value={note} onChange={(e) => setNote(e.target.value)} /></label>
        <label>Delivery change (optional)<input value={deliveryChange} onChange={(e) => setDeliveryChange(e.target.value)} style={{ maxWidth: 160 }} /></label>
        <label>Provider change (optional)<input value={providerChange} onChange={(e) => setProviderChange(e.target.value)} style={{ maxWidth: 160 }} /></label>
      </div>
      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', marginTop: '.5rem' }}>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={feeApplies} onChange={(e) => setFeeApplies(e.target.checked)} style={{ width: 'auto' }} /> Fee applies</label>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={feeWaived} onChange={(e) => setFeeWaived(e.target.checked)} style={{ width: 'auto' }} /> Fee waived</label>
        <button className="btn sm" disabled={busy || !allowed} onClick={go}>{busy ? 'Rescheduling…' : 'Reschedule'}</button>
      </div>
      {!allowed && <p className="muted small" style={{ margin: '.4rem 0 0' }}>You do not have the reschedule permission.</p>}
      <Msg msg={msg} />
    </Card>
  )
}

function GrantAttemptCard({ userId, certificationId, onDone }: { userId: number; certificationId: number; onDone: () => void }) {
  const { can } = useAdminAuth()
  const [grantType, setGrantType] = useState<(typeof GRANT_TYPES)[number]>('replacement')
  const [countsAsAttempt, setCountsAsAttempt] = useState(false)
  const [reason, setReason] = useState('')
  const [note, setNote] = useState('')
  const [incidentId, setIncidentId] = useState('')
  const [feeApplies, setFeeApplies] = useState(false)
  const [feeWaived, setFeeWaived] = useState(false)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)
  const perm = grantType === 'replacement' ? 'ex_grant_replacement' : 'ex_grant_additional'
  const allowed = can(perm)

  async function go() {
    if (!reason.trim()) { setMsg({ ok: false, text: 'A reason is required.' }); return }
    setBusy(true); setMsg(null)
    try {
      await adminApi.post('/api/admin/exam-attempts/grant', {
        user_id: userId,
        certification_id: certificationId,
        grant_type: grantType,
        counts_as_attempt: countsAsAttempt,
        reason: reason.trim(),
        note: note || undefined,
        incident_id: num(incidentId),
        fee_applies: feeApplies,
        fee_waived: feeWaived,
      })
      setMsg({ ok: true, text: `Granted a ${grantType} attempt.` })
      setReason(''); setNote('')
      onDone()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not grant the attempt.' }) } finally { setBusy(false) }
  }

  return (
    <Card title="Grant attempt">
      <p className="muted small" style={{ marginTop: 0 }}>Grant an extra exam attempt (replacement, additional, complimentary, …).</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label>Type<select value={grantType} onChange={(e) => setGrantType(e.target.value as (typeof GRANT_TYPES)[number])}>{GRANT_TYPES.map((g) => <option key={g} value={g}>{titleCase(g)}</option>)}</select></label>
        <label>Incident # (optional)<input value={incidentId} onChange={(e) => setIncidentId(e.target.value)} style={{ maxWidth: 120 }} /></label>
        <label style={{ flex: 1, minWidth: 180 }}>Reason (required)<input value={reason} onChange={(e) => setReason(e.target.value)} /></label>
        <label style={{ flex: 1, minWidth: 160 }}>Note (optional)<input value={note} onChange={(e) => setNote(e.target.value)} /></label>
      </div>
      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', marginTop: '.5rem' }}>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={countsAsAttempt} onChange={(e) => setCountsAsAttempt(e.target.checked)} style={{ width: 'auto' }} /> Counts as an attempt</label>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={feeApplies} onChange={(e) => setFeeApplies(e.target.checked)} style={{ width: 'auto' }} /> Fee applies</label>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={feeWaived} onChange={(e) => setFeeWaived(e.target.checked)} style={{ width: 'auto' }} /> Fee waived</label>
        <button className="btn sm" disabled={busy || !allowed} onClick={go}>{busy ? 'Granting…' : 'Grant attempt'}</button>
      </div>
      {!allowed && <p className="muted small" style={{ margin: '.4rem 0 0' }}>You do not have the {perm.replace('ex_', '')} permission.</p>}
      <Msg msg={msg} />
    </Card>
  )
}

function WaiveFeeCard({ userId, certificationId, onDone }: { userId: number; certificationId: number; onDone: () => void }) {
  const { can } = useAdminAuth()
  const [feeType, setFeeType] = useState<(typeof FEE_TYPES)[number]>('exam')
  const [percent, setPercent] = useState('100')
  const [reason, setReason] = useState('')
  const [note, setNote] = useState('')
  const [sponsor, setSponsor] = useState('')
  const [evidence, setEvidence] = useState('')
  const [expires, setExpires] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)
  const perm = feeType === 'exam' ? 'ex_waive_exam' : feeType === 'retake' ? 'ex_waive_retake' : 'ex_waive_resched'
  const allowed = can(perm)

  async function go() {
    if (!reason.trim()) { setMsg({ ok: false, text: 'A reason is required — every waiver must record why.' }); return }
    if (busy) return
    setBusy(true); setMsg(null)
    // Durable client key — required server-side for partial and reschedule-only waivers.
    const idempotencyKey = crypto.randomUUID()
    try {
      const r = await adminApi.post<{ payable?: number; skips_checkout?: boolean }>('/api/admin/exam-fee-waiver', {
        user_id: userId,
        certification_id: certificationId,
        fee_type: feeType,
        percent: num(percent) ?? 100,
        reason: reason.trim(),
        note: note || undefined,
        sponsor: sponsor || undefined,
        evidence_ref: evidence || undefined,
        expires: toIso(expires),
        idempotency_key: idempotencyKey,
      })
      setMsg({ ok: true, text: r.skips_checkout ? `${titleCase(feeType)} fee waived — access granted (no checkout needed).` : `${titleCase(feeType)} fee waiver recorded — payable $${r.payable ?? 0}.` })
      setReason(''); setNote('')
      onDone()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not record the waiver.' }) } finally { setBusy(false) }
  }

  return (
    <Card title="Waive fee">
      <p className="muted small" style={{ marginTop: 0 }}>Waive the exam, retake or reschedule fee — 100% grants access immediately.</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label>Fee type<select value={feeType} onChange={(e) => setFeeType(e.target.value as (typeof FEE_TYPES)[number])}>{FEE_TYPES.map((f) => <option key={f} value={f}>{titleCase(f)}</option>)}</select></label>
        <label>Percent<input type="number" min="1" max="100" value={percent} onChange={(e) => setPercent(e.target.value)} style={{ maxWidth: 100 }} /></label>
        <label style={{ flex: 1, minWidth: 180 }}>Reason (required)<input value={reason} onChange={(e) => setReason(e.target.value)} placeholder="scholarship / sponsorship / incident" /></label>
        <label>Sponsor (optional)<input value={sponsor} onChange={(e) => setSponsor(e.target.value)} style={{ maxWidth: 160 }} /></label>
        <label style={{ flex: 1, minWidth: 160 }}>Note (optional)<input value={note} onChange={(e) => setNote(e.target.value)} /></label>
        <label>Evidence ref (optional)<input value={evidence} onChange={(e) => setEvidence(e.target.value)} style={{ maxWidth: 160 }} /></label>
        <label>Expires (optional)<input type="date" value={expires} onChange={(e) => setExpires(e.target.value)} style={{ maxWidth: 160 }} /></label>
      </div>
      <div className="row" style={{ marginTop: '.5rem' }}>
        <button className="btn sm" disabled={busy || !allowed} onClick={go}>{busy ? 'Waiving…' : 'Waive fee'}</button>
      </div>
      {!allowed && <p className="muted small" style={{ margin: '.4rem 0 0' }}>You do not have the {perm.replace('ex_', '')} permission.</p>}
      <Msg msg={msg} />
    </Card>
  )
}

function WaiveWaitCard({ authId, onDone }: { authId: number; onDone: () => void }) {
  const { can } = useAdminAuth()
  const allowed = can('ex_waive_wait')
  const [reason, setReason] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)

  async function go() {
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/exam-authorizations/${authId}/waive-waiting-period`, { reason: reason.trim() || undefined })
      setMsg({ ok: true, text: 'Retake waiting period waived — the student can rebook immediately.' })
      setReason('')
      onDone()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not waive the waiting period.' }) } finally { setBusy(false) }
  }

  return (
    <Card title="Waive waiting period">
      <p className="muted small" style={{ marginTop: 0 }}>Remove the retake cooling-off period so the student can rebook without waiting.</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label style={{ flex: 1, minWidth: 220 }}>Reason (optional)<input value={reason} onChange={(e) => setReason(e.target.value)} /></label>
        <button className="btn sm" disabled={busy || !allowed} onClick={go}>{busy ? 'Waiving…' : 'Waive waiting period'}</button>
      </div>
      {!allowed && <p className="muted small" style={{ margin: '.4rem 0 0' }}>You do not have the waive-wait permission.</p>}
      <Msg msg={msg} />
    </Card>
  )
}

/** Attempts list with per-row Restore / Invalidate (each reason-required). */
function AttemptsCard({ attempts, onDone }: { attempts: Record<string, unknown>[]; onDone: () => void }) {
  const { can } = useAdminAuth()
  const [active, setActive] = useState<{ id: number; action: 'restore' | 'invalidate' } | null>(null)
  const [reason, setReason] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)

  async function confirm() {
    if (!active) return
    if (!reason.trim()) { setMsg({ ok: false, text: 'A reason is required.' }); return }
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/exam-attempts/${active.id}/${active.action}`, { reason: reason.trim() })
      setMsg({ ok: true, text: `Attempt #${active.id} ${active.action === 'restore' ? 'restored' : 'invalidated'}.` })
      setActive(null); setReason('')
      onDone()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Action failed.' }) } finally { setBusy(false) }
  }

  return (
    <Card title={`Attempts (${attempts.length})`}>
      <p className="muted small" style={{ marginTop: 0 }}>Restore an invalidated attempt, or invalidate one affected by an incident.</p>
      <Msg msg={msg} style={{ marginTop: 0, marginBottom: '.6rem' }} />
      {attempts.length === 0 ? <Empty>No attempts recorded.</Empty> : (
        <div style={{ overflowX: 'auto' }}>
          <table className="data">
            <thead><tr><th>#</th><th>Kind</th><th>Started</th><th>Status</th><th>Result</th><th /></tr></thead>
            <tbody>
              {attempts.map((a, i) => {
                const id = Number(a.id)
                const status = String(a.result_status ?? a.status ?? '')
                return (
                  <tr key={i}>
                    <td className="small">{sv(a.id)}</td>
                    <td className="small">{sv(a.kind)}</td>
                    <td className="small muted">{fmtDateTime(a.started_at)}</td>
                    <td><StatusBadge status={status} /></td>
                    <td className="small">{sv(a.result)}</td>
                    <td>
                      {active && active.id === id ? (
                        <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                          <input placeholder="Reason" value={reason} onChange={(e) => setReason(e.target.value)} style={{ maxWidth: 150 }} />
                          <button className={'btn sm' + (active.action === 'invalidate' ? ' danger' : '')} disabled={busy} onClick={confirm}>{active.action === 'restore' ? 'Restore' : 'Invalidate'}</button>
                          <button className="btn sm secondary" onClick={() => { setActive(null); setReason('') }}>Cancel</button>
                        </div>
                      ) : (
                        <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                          {can('ex_restore') && <button className="btn ghost sm" onClick={() => { setActive({ id, action: 'restore' }); setReason(''); setMsg(null) }}>Restore</button>}
                          {can('ex_invalidate') && <button className="btn ghost sm danger" onClick={() => { setActive({ id, action: 'invalidate' }); setReason(''); setMsg(null) }}>Invalidate</button>}
                        </div>
                      )}
                    </td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        </div>
      )}
    </Card>
  )
}

// ---------------------------------------------------------------- authorization drawer

function AuthDrawer({ row, onClose, onChanged }: { row: ExamExceptionRow; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<ExamExceptionDetail>(`/api/admin/exam-exceptions/${row.id}`)
  const refresh = () => { refetch(); onChanged() }
  const name = `${row.first_name ?? ''} ${row.last_name ?? ''}`.trim() || row.email || `Student #${row.user_id}`
  const certLabel = row.certification_acronym || row.certification_name || `Certification #${row.certification_id}`

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Authorization #{row.id}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data ? null : (
          <div className="page">
            <Card className="entity" title={name} action={<StatusBadge status={row.status} />}>
              <div className="grid cols-2 small">
                <div><span className="muted">Email</span><div>{sv(row.email)}</div></div>
                <div><span className="muted">Certification</span><div>{certLabel}</div></div>
                <div><span className="muted">Original deadline</span><div>{fmtDateTime(row.original_deadline)}</div></div>
                <div><span className="muted">Current deadline</span><div>{fmtDateTime(row.current_deadline)}</div></div>
                <div><span className="muted">Access expiry</span><div>{fmtDateTime(row.access_expiry)}</div></div>
                <div><span className="muted">Retake wait until</span><div>{fmtDateTime(row.retake_wait_until)}</div></div>
                <div><span className="muted">Attempts</span><div>{data.attempts_used} used / {data.attempts_permitted} permitted</div></div>
                <div><span className="muted">Window</span><div>{row.window_days != null ? `${row.window_days} days` : '—'}{row.window_source ? ` · ${row.window_source}` : ''}</div></div>
                <div><span className="muted">Country</span><div>{sv(row.country)}</div></div>
                <div><span className="muted">Created</span><div>{fmtDate(row.created_at)}</div></div>
              </div>
            </Card>

            <ExtendCard authId={row.id} onDone={refresh} />
            <RescheduleCard authId={row.id} onDone={refresh} />
            <GrantAttemptCard userId={row.user_id} certificationId={row.certification_id} onDone={refresh} />
            <AttemptsCard attempts={data.attempts} onDone={refresh} />
            <WaiveFeeCard userId={row.user_id} certificationId={row.certification_id} onDone={refresh} />
            <WaiveWaitCard authId={row.id} onDone={refresh} />

            <HistoryCard title="Extensions" rows={data.extensions} cols={[
              { key: 'created_at', label: 'When', fmt: (v) => fmtDateTime(v) },
              { key: 'new_deadline', label: 'New deadline', fmt: (v) => fmtDateTime(v) },
              { key: 'reason', label: 'Reason' },
            ]} />
            <HistoryCard title="Reschedules" rows={data.reschedules} cols={[
              { key: 'created_at', label: 'When', fmt: (v) => fmtDateTime(v) },
              { key: 'scheduled_at', label: 'Scheduled for', fmt: (v) => fmtDateTime(v) },
              { key: 'reason', label: 'Reason' },
            ]} />
            <HistoryCard title="Attempt grants" rows={data.attempt_grants} cols={[
              { key: 'created_at', label: 'When', fmt: (v) => fmtDateTime(v) },
              { key: 'grant_type', label: 'Type', fmt: (v) => titleCase(String(v ?? '')) },
              { key: 'reason', label: 'Reason' },
            ]} />
            <HistoryCard title="Waivers" rows={data.waivers} cols={[
              { key: 'created_at', label: 'When', fmt: (v) => fmtDateTime(v) },
              { key: 'fee_type', label: 'Fee', fmt: (v) => titleCase(String(v ?? '')) },
              { key: 'percent', label: 'Percent' },
              { key: 'status', label: 'Status', fmt: (v) => <StatusBadge status={String(v ?? '')} /> },
            ]} />
            <HistoryCard title="Bookings" rows={data.bookings} cols={[
              { key: 'scheduled_at', label: 'Scheduled for', fmt: (v) => fmtDateTime(v) },
              { key: 'timezone', label: 'Timezone' },
              { key: 'status', label: 'Status', fmt: (v) => <StatusBadge status={String(v ?? '')} /> },
            ]} />
            <HistoryCard title="Incidents" rows={data.incidents} cols={[
              { key: 'created_at', label: 'When', fmt: (v) => fmtDateTime(v) },
              { key: 'category', label: 'Category', fmt: (v) => titleCase(String(v ?? '')) },
              { key: 'status', label: 'Status', fmt: (v) => <StatusBadge status={String(v ?? '')} /> },
              { key: 'decision', label: 'Decision', fmt: (v) => titleCase(String(v ?? '')) },
            ]} />
          </div>
        )}
      </div>
    </div>
  )
}

// ---------------------------------------------------------------- reopen scheduling (single + bulk)

function ReopenCard({ certs, onDone }: { certs: CertOpt[]; onDone: () => void }) {
  const { can } = useAdminAuth()
  const allowed = can('ex_reopen')
  const [userId, setUserId] = useState('')
  const [certId, setCertId] = useState('')
  const [reason, setReason] = useState('')
  const [newDeadline, setNewDeadline] = useState('')
  const [waiveFee, setWaiveFee] = useState(false)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)

  async function go() {
    if (!userId.trim()) { setMsg({ ok: false, text: 'A user ID is required.' }); return }
    if (!reason.trim()) { setMsg({ ok: false, text: 'A reason is required.' }); return }
    setBusy(true); setMsg(null)
    try {
      await adminApi.post('/api/admin/exam-authorizations/reopen', {
        user_id: Number(userId),
        certification_id: num(certId),
        reason: reason.trim(),
        new_deadline: toIso(newDeadline),
        waive_fee: waiveFee,
      })
      setMsg({ ok: true, text: `Scheduling reopened for user #${userId}.` })
      setReason('')
      onDone()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not reopen scheduling.' }) } finally { setBusy(false) }
  }

  return (
    <Card title="Reopen scheduling">
      <p className="muted small" style={{ marginTop: 0 }}>Reopen scheduling for one student whose window has closed.</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label>User ID<input value={userId} onChange={(e) => setUserId(e.target.value)} style={{ maxWidth: 120 }} /></label>
        <label>Certification<select value={certId} onChange={(e) => setCertId(e.target.value)}><option value="">Any / default</option>{certs.map((c) => <option key={c.id} value={c.id}>{c.code} — {c.name}</option>)}</select></label>
        <label>New deadline (optional)<input type="datetime-local" value={newDeadline} onChange={(e) => setNewDeadline(e.target.value)} /></label>
        <label style={{ flex: 1, minWidth: 180 }}>Reason (required)<input value={reason} onChange={(e) => setReason(e.target.value)} /></label>
      </div>
      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', marginTop: '.5rem' }}>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={waiveFee} onChange={(e) => setWaiveFee(e.target.checked)} style={{ width: 'auto' }} /> Waive fee</label>
        <button className="btn sm" disabled={busy || !allowed} onClick={go}>{busy ? 'Reopening…' : 'Reopen scheduling'}</button>
      </div>
      {!allowed && <p className="muted small" style={{ margin: '.4rem 0 0' }}>You do not have the reopen permission.</p>}
      <Msg msg={msg} />
    </Card>
  )
}

function BulkReopenCard({ certs, onDone }: { certs: CertOpt[]; onDone: () => void }) {
  const { can } = useAdminAuth()
  const allowed = can('ex_bulk')
  const [idsText, setIdsText] = useState('')
  const [certId, setCertId] = useState('')
  const [reason, setReason] = useState('')
  const [confirmCount, setConfirmCount] = useState('')
  const [waiveFee, setWaiveFee] = useState(false)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)

  const userIds = idsText.split(/[\s,]+/).map((s) => Number(s)).filter((n) => Number.isFinite(n) && n > 0)

  async function go() {
    if (userIds.length === 0) { setMsg({ ok: false, text: 'Enter at least one user ID.' }); return }
    if (!reason.trim()) { setMsg({ ok: false, text: 'A reason is required.' }); return }
    if (num(confirmCount) !== userIds.length) { setMsg({ ok: false, text: `Confirm count must equal the ${userIds.length} user ID(s) entered.` }); return }
    setBusy(true); setMsg(null)
    try {
      await adminApi.post('/api/admin/exam-authorizations/bulk-reopen', {
        user_ids: userIds,
        certification_id: num(certId),
        reason: reason.trim(),
        confirm_count: userIds.length,
        waive_fee: waiveFee,
      })
      setMsg({ ok: true, text: `Scheduling reopened for ${userIds.length} student(s).` })
      setIdsText(''); setConfirmCount(''); setReason('')
      onDone()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Bulk reopen failed.' }) } finally { setBusy(false) }
  }

  return (
    <Card title="Bulk reopen">
      <p className="muted small" style={{ marginTop: 0 }}>Reopen scheduling for many students at once — enter user IDs, then type the count to confirm.</p>
      <div className="field" style={{ marginBottom: '.6rem' }}>
        <label htmlFor="bulk-ids">User IDs (comma, space or newline separated)</label>
        <textarea id="bulk-ids" value={idsText} onChange={(e) => setIdsText(e.target.value)} rows={3} placeholder="101, 102, 103" />
        <div className="muted small" style={{ marginTop: '.25rem' }}>{userIds.length} valid ID(s) detected.</div>
      </div>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label>Certification<select value={certId} onChange={(e) => setCertId(e.target.value)}><option value="">Any / default</option>{certs.map((c) => <option key={c.id} value={c.id}>{c.code} — {c.name}</option>)}</select></label>
        <label style={{ flex: 1, minWidth: 180 }}>Reason (required)<input value={reason} onChange={(e) => setReason(e.target.value)} /></label>
        <label>Confirm count<input value={confirmCount} onChange={(e) => setConfirmCount(e.target.value)} style={{ maxWidth: 120 }} placeholder={String(userIds.length)} /></label>
      </div>
      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', marginTop: '.5rem' }}>
        <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={waiveFee} onChange={(e) => setWaiveFee(e.target.checked)} style={{ width: 'auto' }} /> Waive fee</label>
        <button className="btn sm danger" disabled={busy || !allowed} onClick={go}>{busy ? 'Reopening…' : `Reopen ${userIds.length || ''} student(s)`}</button>
      </div>
      {!allowed && <p className="muted small" style={{ margin: '.4rem 0 0' }}>You do not have the bulk permission.</p>}
      <Msg msg={msg} />
    </Card>
  )
}

// ---------------------------------------------------------------- Authorizations tab

function AuthorizationsTab() {
  const certs = useCertOptions()
  const [q, setQ] = useState('')
  const [dq, setDq] = useState('')
  const [status, setStatus] = useState('')
  const [certId, setCertId] = useState('')
  const [selected, setSelected] = useState<ExamExceptionRow | null>(null)
  useEffect(() => { const t = setTimeout(() => setDq(q), 300); return () => clearTimeout(t) }, [q])

  const params = new URLSearchParams()
  if (dq) params.set('q', dq)
  if (status) params.set('status', status)
  if (certId) params.set('certification_id', certId)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: ExamExceptionRow[] }>(`/api/admin/exam-exceptions${qs ? '?' + qs : ''}`)

  return (
    <>
      <div className="grid cols-2">
        <ReopenCard certs={certs} onDone={refetch} />
        <BulkReopenCard certs={certs} onDone={refetch} />
      </div>

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search name or email…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 300 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All statuses</option>
            <option value="active">Active</option>
            <option value="scheduled">Scheduled</option>
            <option value="expired">Expired</option>
            <option value="completed">Completed</option>
            <option value="closed">Closed</option>
            <option value="reopened">Reopened</option>
          </select>
          <select value={certId} onChange={(e) => setCertId(e.target.value)} style={{ maxWidth: 240 }}>
            <option value="">All certifications</option>
            {certs.map((c) => <option key={c.id} value={c.id}>{c.code} — {c.name}</option>)}
          </select>
        </div>
      </Card>

      <Card>
        {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No authorizations match.</Empty>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead>
                <tr><th>Student</th><th>Certification</th><th>Deadline</th><th>Attempts</th><th>Exceptions</th><th>Status</th></tr>
              </thead>
              <tbody>
                {data.rows.map((r) => (
                  <tr key={r.id} {...rowActivate(() => setSelected(r))}>
                    <td><strong>{`${r.first_name ?? ''} ${r.last_name ?? ''}`.trim() || '—'}</strong><div className="small muted">{r.email}</div></td>
                    <td className="small">{r.certification_acronym || r.certification_name || `#${r.certification_id}`}</td>
                    <td className="small muted">{fmtDate(r.current_deadline)}</td>
                    <td className="small">{r.attempts_used_total ?? r.attempts_used ?? 0} / {r.attempts_permitted_total ?? r.attempts_permitted ?? 0}</td>
                    <td>
                      <div className="row" style={{ gap: '.25rem', flexWrap: 'wrap' }}>
                        {(r.extensions ?? 0) > 0 && <Badge tone="brand">{r.extensions} ext</Badge>}
                        {(r.reschedules ?? 0) > 0 && <Badge tone="brand">{r.reschedules} resch</Badge>}
                        {(r.grants ?? 0) > 0 && <Badge tone="warn">{r.grants} grant</Badge>}
                        {(r.incidents ?? 0) > 0 && <Badge tone="err">{r.incidents} inc</Badge>}
                      </div>
                    </td>
                    <td><StatusBadge status={r.status} /></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </Card>

      {selected && <AuthDrawer row={selected} onClose={() => setSelected(null)} onChanged={refetch} />}
    </>
  )
}

// ---------------------------------------------------------------- Incidents tab

function IncidentDrawer({ id, onClose, onChanged }: { id: number; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<{
    incident: Record<string, unknown>
    student: Record<string, unknown>
    attempt: Record<string, unknown> | null
    proctor_events: { type?: string | null; severity?: string | null; detail?: string | null; at?: string | null }[]
  }>(`/api/admin/exam-incidents/${id}`)
  const [decision, setDecision] = useState<(typeof DECISIONS)[number]>('no_action')
  const [decNote, setDecNote] = useState('')
  const [invResult, setInvResult] = useState('')
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState<string | null>(null)
  const [msg, setMsg] = useState<Note>(null)
  const refresh = () => { refetch(); onChanged() }

  async function decide() {
    setBusy('decide'); setMsg(null)
    try {
      await adminApi.post(`/api/admin/exam-incidents/${id}/decide`, { decision, note: decNote || undefined, investigation_result: invResult || undefined })
      setMsg({ ok: true, text: `Decision recorded: ${titleCase(decision)}.` })
      setDecNote(''); setInvResult('')
      refresh()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not record the decision.' }) } finally { setBusy(null) }
  }
  async function addNote() {
    if (!note.trim()) { setMsg({ ok: false, text: 'Enter a note.' }); return }
    setBusy('note'); setMsg(null)
    try {
      await adminApi.post(`/api/admin/exam-incidents/${id}/note`, { note: note.trim() })
      setMsg({ ok: true, text: 'Note added.' })
      setNote('')
      refresh()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not add the note.' }) } finally { setBusy(null) }
  }

  const inc = data?.incident ?? {}
  const stu = data?.student ?? {}
  const stuName = `${sv(stu.first_name)} ${sv(stu.last_name)}`.replace(/—/g, '').trim() || sv(stu.email)

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Incident #{id}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data ? null : (
          <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
            <Card title={titleCase(String(inc.category ?? 'Incident'))} action={<StatusBadge status={String(inc.status ?? '')} />}>
              <div className="grid cols-2 small">
                <div><span className="muted">Reference</span><div>{sv(inc.reference)}</div></div>
                <div><span className="muted">Severity</span><div>{sv(inc.severity)}</div></div>
                <div><span className="muted">Student</span><div>{stuName}</div></div>
                <div><span className="muted">Email</span><div>{sv(stu.email)}</div></div>
                <div><span className="muted">Reported</span><div>{fmtDateTime(inc.created_at)}</div></div>
                <div><span className="muted">Decision</span><div>{inc.decision ? titleCase(String(inc.decision)) : '—'}</div></div>
              </div>
              {inc.student_explanation ? <div className="notice" style={{ marginTop: '.6rem' }}><strong>Student explanation:</strong> {String(inc.student_explanation)}</div> : null}
            </Card>

            <Card title={`Proctoring events (${data.proctor_events.length})`}>
              {data.proctor_events.length === 0 ? <Empty>No proctoring events.</Empty> : (
                <div className="stack" style={{ display: 'grid', gap: '.4rem', maxHeight: 240, overflowY: 'auto' }}>
                  {data.proctor_events.map((e, i) => (
                    <div key={i} className="row" style={{ gap: '.5rem', alignItems: 'baseline' }}>
                      <Badge tone={(e.severity ?? '').toLowerCase() === 'high' || (e.severity ?? '').toLowerCase() === 'critical' ? 'err' : (e.severity ?? '').toLowerCase() === 'medium' ? 'warn' : 'brand'}>{e.severity}</Badge>
                      <span style={{ fontWeight: 600 }}>{titleCase(e.type ?? '')}</span>
                      <span className="small muted">{e.detail}</span>
                      <span className="small muted" style={{ marginLeft: 'auto' }}>{fmtDateTime(e.at)}</span>
                    </div>
                  ))}
                </div>
              )}
            </Card>

            <Card title="Decide">
              <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
                <label>Decision<select value={decision} onChange={(e) => setDecision(e.target.value as (typeof DECISIONS)[number])}>{DECISIONS.map((d) => <option key={d} value={d}>{titleCase(d)}</option>)}</select></label>
                <label style={{ flex: 1, minWidth: 180 }}>Note (optional)<input value={decNote} onChange={(e) => setDecNote(e.target.value)} /></label>
                <label style={{ flex: 1, minWidth: 180 }}>Investigation result (optional)<input value={invResult} onChange={(e) => setInvResult(e.target.value)} /></label>
                <button className="btn sm" disabled={busy === 'decide'} onClick={decide}>{busy === 'decide' ? 'Saving…' : 'Record decision'}</button>
              </div>
              <div className="field" style={{ margin: '.75rem 0 0' }}>
                <label htmlFor="inc-note">Add an investigation note</label>
                <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
                  <input id="inc-note" value={note} onChange={(e) => setNote(e.target.value)} placeholder="Internal note" style={{ flex: 1, minWidth: 200 }} />
                  <button className="btn sm secondary" disabled={busy === 'note'} onClick={addNote}>{busy === 'note' ? 'Adding…' : 'Add note'}</button>
                </div>
              </div>
              <Msg msg={msg} />
            </Card>

            {data.attempt && (
              <Card title="Linked attempt">
                <div className="grid cols-2 small">
                  <div><span className="muted">Attempt</span><div>{sv(data.attempt.id)}</div></div>
                  <div><span className="muted">Status</span><div><StatusBadge status={String(data.attempt.result_status ?? data.attempt.status ?? '')} /></div></div>
                  <div><span className="muted">Started</span><div>{fmtDateTime(data.attempt.started_at)}</div></div>
                  <div><span className="muted">Result</span><div>{sv(data.attempt.result)}</div></div>
                </div>
              </Card>
            )}
          </div>
        )}
      </div>
    </div>
  )
}

function IncidentsTab() {
  const certs = useCertOptions()
  const [q, setQ] = useState('')
  const [dq, setDq] = useState('')
  const [status, setStatus] = useState('')
  const [severity, setSeverity] = useState('')
  const [category, setCategory] = useState('')
  const [certId, setCertId] = useState('')
  const [selected, setSelected] = useState<number | null>(null)
  useEffect(() => { const t = setTimeout(() => setDq(q), 300); return () => clearTimeout(t) }, [q])

  const params = new URLSearchParams()
  if (dq) params.set('q', dq)
  if (status) params.set('status', status)
  if (severity) params.set('severity', severity)
  if (category) params.set('category', category)
  if (certId) params.set('certification_id', certId)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: ExamIncidentRow[] }>(`/api/admin/exam-incidents${qs ? '?' + qs : ''}`)

  return (
    <>
      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 240 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 160 }}>
            <option value="">All statuses</option>
            <option value="open">Open</option>
            <option value="under_review">Under review</option>
            <option value="resolved">Resolved</option>
            <option value="rejected">Rejected</option>
            <option value="escalated">Escalated</option>
          </select>
          <select value={severity} onChange={(e) => setSeverity(e.target.value)} style={{ maxWidth: 150 }}>
            <option value="">All severities</option>
            <option value="low">Low</option>
            <option value="medium">Medium</option>
            <option value="high">High</option>
            <option value="critical">Critical</option>
          </select>
          <select value={category} onChange={(e) => setCategory(e.target.value)} style={{ maxWidth: 200 }}>
            <option value="">All categories</option>
            {CATEGORIES.map((c) => <option key={c} value={c}>{titleCase(c)}</option>)}
          </select>
          <select value={certId} onChange={(e) => setCertId(e.target.value)} style={{ maxWidth: 220 }}>
            <option value="">All certifications</option>
            {certs.map((c) => <option key={c.id} value={c.id}>{c.code} — {c.name}</option>)}
          </select>
        </div>
      </Card>

      <Card>
        {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No incidents match.</Empty>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead>
                <tr><th>Reference</th><th>Student</th><th>Category</th><th>Severity</th><th>Reported</th><th>Status</th></tr>
              </thead>
              <tbody>
                {data.rows.map((r) => (
                  <tr key={r.id} {...rowActivate(() => setSelected(r.id))}>
                    <td className="small muted">{r.reference || `#${r.id}`}</td>
                    <td><strong>{`${r.first_name ?? ''} ${r.last_name ?? ''}`.trim() || '—'}</strong><div className="small muted">{r.email}</div></td>
                    <td className="small">{titleCase(r.category ?? '')}</td>
                    <td className="small">{r.severity ? <Badge tone={r.severity === 'high' || r.severity === 'critical' ? 'err' : r.severity === 'medium' ? 'warn' : 'brand'}>{r.severity}</Badge> : '—'}</td>
                    <td className="small muted">{fmtDate(r.created_at)}</td>
                    <td><StatusBadge status={r.status} /></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </Card>

      {selected !== null && <IncidentDrawer id={selected} onClose={() => setSelected(null)} onChanged={refetch} />}
    </>
  )
}

// ---------------------------------------------------------------- Window rules tab

interface WindowRulesResp { rows: ExamWindowRule[]; defaults: Record<string, unknown>; scope_types: string[] }

function WindowRulesTab() {
  const { can } = useAdminAuth()
  const canEdit = can('ex_approve')
  const { data, loading, error, refetch } = useAdminQuery<WindowRulesResp>('/api/admin/exam-window-rules')
  const scopeTypes = data?.scope_types ?? []

  const [editId, setEditId] = useState<number | null>(null)
  const [scopeType, setScopeType] = useState('')
  const [scopeValue, setScopeValue] = useState('')
  const [windowDays, setWindowDays] = useState('')
  const [accessExpiryDays, setAccessExpiryDays] = useState('')
  const [attemptsPermitted, setAttemptsPermitted] = useState('')
  const [retakeWaitDays, setRetakeWaitDays] = useState('')
  const [active, setActive] = useState(true)
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<Note>(null)

  function reset() {
    setEditId(null); setScopeType(''); setScopeValue(''); setWindowDays(''); setAccessExpiryDays('')
    setAttemptsPermitted(''); setRetakeWaitDays(''); setActive(true); setNote('')
  }
  function edit(r: ExamWindowRule) {
    setEditId(r.id)
    setScopeType(r.scope_type ?? '')
    setScopeValue(r.scope_value ?? '')
    setWindowDays(r.window_days != null ? String(r.window_days) : '')
    setAccessExpiryDays(r.access_expiry_days != null ? String(r.access_expiry_days) : '')
    setAttemptsPermitted(r.attempts_permitted != null ? String(r.attempts_permitted) : '')
    setRetakeWaitDays(r.retake_wait_days != null ? String(r.retake_wait_days) : '')
    setActive(Number(r.active ?? 1) === 1)
    setNote(r.note ?? '')
    setMsg(null)
  }

  async function save() {
    if (!scopeType) { setMsg({ ok: false, text: 'Choose a scope type.' }); return }
    setBusy(true); setMsg(null)
    try {
      await adminApi.post('/api/admin/exam-window-rules', {
        id: editId ?? undefined,
        scope_type: scopeType,
        scope_value: scopeValue || undefined,
        window_days: num(windowDays),
        access_expiry_days: num(accessExpiryDays),
        attempts_permitted: num(attemptsPermitted),
        retake_wait_days: num(retakeWaitDays),
        active,
        note: note || undefined,
      })
      setMsg({ ok: true, text: editId ? 'Rule updated.' : 'Rule created.' })
      reset()
      refetch()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not save the rule.' }) } finally { setBusy(false) }
  }
  async function remove(id: number) {
    if (!confirm('Delete this window rule?')) return
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/exam-window-rules/${id}/delete`, {})
      setMsg({ ok: true, text: 'Rule deleted.' })
      if (editId === id) reset()
      refetch()
    } catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not delete the rule.' }) } finally { setBusy(false) }
  }

  const defaults = data?.defaults ?? {}

  return (
    <>
      {canEdit && (
        <Card title={editId ? `Edit rule #${editId}` : 'Add window rule'}>
          <p className="muted small" style={{ marginTop: 0 }}>Scheduling window, access expiry, attempts and retake wait, scoped to a certification, institution, country or the global default.</p>
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
            <label>Scope type
              <select value={scopeType} onChange={(e) => setScopeType(e.target.value)}>
                <option value="">Select…</option>
                {scopeTypes.map((s) => <option key={s} value={s}>{titleCase(s)}</option>)}
              </select>
            </label>
            <label>Scope value<input value={scopeValue} onChange={(e) => setScopeValue(e.target.value)} placeholder="e.g. cert id / country" style={{ maxWidth: 160 }} /></label>
            <label>Window days<input type="number" value={windowDays} onChange={(e) => setWindowDays(e.target.value)} style={{ maxWidth: 110 }} /></label>
            <label>Access expiry days<input type="number" value={accessExpiryDays} onChange={(e) => setAccessExpiryDays(e.target.value)} style={{ maxWidth: 130 }} /></label>
            <label>Attempts<input type="number" value={attemptsPermitted} onChange={(e) => setAttemptsPermitted(e.target.value)} style={{ maxWidth: 100 }} /></label>
            <label>Retake wait days<input type="number" value={retakeWaitDays} onChange={(e) => setRetakeWaitDays(e.target.value)} style={{ maxWidth: 120 }} /></label>
            <label style={{ flex: 1, minWidth: 160 }}>Note (optional)<input value={note} onChange={(e) => setNote(e.target.value)} /></label>
          </div>
          <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', marginTop: '.5rem' }}>
            <label className="row small" style={{ gap: '.35rem' }}><input type="checkbox" checked={active} onChange={(e) => setActive(e.target.checked)} style={{ width: 'auto' }} /> Active</label>
            <button className="btn sm" disabled={busy} onClick={save}>{busy ? 'Saving…' : editId ? 'Update rule' : 'Add rule'}</button>
            {editId && <button className="btn sm secondary" onClick={reset}>Cancel edit</button>}
          </div>
          <Msg msg={msg} />
        </Card>
      )}

      <Card title="Window rules">
        {Object.keys(defaults).length > 0 && (
          <p className="muted small" style={{ marginTop: 0 }}>
            Defaults: {Object.entries(defaults).map(([k, v]) => `${k.replace(/_/g, ' ')} ${sv(v)}`).join(' · ')}
          </p>
        )}
        {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No custom window rules — the defaults apply everywhere.</Empty>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead>
                <tr><th>Scope</th><th>Window</th><th>Expiry</th><th>Attempts</th><th>Retake wait</th><th>Active</th><th /></tr>
              </thead>
              <tbody>
                {data.rows.map((r) => (
                  <tr key={r.id}>
                    <td className="small"><strong>{titleCase(r.scope_type)}</strong>{r.scope_value ? ` · ${r.scope_value}` : ''}{r.note ? <div className="muted small">{r.note}</div> : null}</td>
                    <td className="small">{r.window_days != null ? `${r.window_days}d` : '—'}</td>
                    <td className="small">{r.access_expiry_days != null ? `${r.access_expiry_days}d` : '—'}</td>
                    <td className="small">{r.attempts_permitted ?? '—'}</td>
                    <td className="small">{r.retake_wait_days != null ? `${r.retake_wait_days}d` : '—'}</td>
                    <td>{Number(r.active ?? 1) === 1 ? <Badge tone="ok">active</Badge> : <Badge>inactive</Badge>}</td>
                    <td>
                      {canEdit && (
                        <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                          <button className="btn ghost sm" onClick={() => edit(r)}>Edit</button>
                          <button className="btn ghost sm danger" disabled={busy} onClick={() => remove(r.id)}>Delete</button>
                        </div>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </Card>
    </>
  )
}

// ---------------------------------------------------------------- page

const TABS = ['Authorizations', 'Incidents', 'Window rules'] as const

export default function ExamExceptions() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Authorizations')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <PageHeader title="Exam Exceptions & Authorizations" />
      </div>
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>)}
      </div>
      {tab === 'Authorizations' ? <AuthorizationsTab /> : tab === 'Incidents' ? <IncidentsTab /> : <WindowRulesTab />}
    </div>
  )
}
