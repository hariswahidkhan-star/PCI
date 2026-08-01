import { useState, useEffect } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type MemberRow, type MemberDetail, type IdentityDocRow } from '../api'
import { Card, StatusBadge, Badge, Spinner, ErrorNote, Empty, rowActivate } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtDate, fmtDateTime, fmtMoney } from '../../format'
import { ApiError } from '../../api/client'

/** Fee waiver: grant an exam entitlement without payment (mirrors the webhook grant server-side). */
function WaiverCard({ id, onGranted }: { id: number; onGranted: () => void }) {
  const { data } = useAdminQuery<{ rows: { id: number; code: string; name: string }[] }>('/api/certifications')
  const [cert, setCert] = useState('')
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)

  async function grant() {
    setBusy(true)
    setMsg(null)
    try {
      const r = await adminApi.post<{ reference: string }>(`/api/admin/students/${id}/exam-waiver`, { certification_id: cert || undefined, note: note || undefined })
      setMsg({ ok: true, text: `Exam access granted without fee — reference ${r.reference}. The student has been notified in their portal.` })
      setNote('')
      onGranted()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setMsg({ ok: false, text: code === 'already_entitled' ? 'This student already has an unused exam entitlement for that certification.' : e instanceof Error ? e.message : 'Waiver failed.' })
    } finally {
      setBusy(false)
    }
  }

  return (
    <Card title="Waive exam fee">
      <p className="muted small" style={{ marginTop: 0 }}>
        Grants this student exam access without payment (scholarship, comp, corporate seat). All the
        usual eligibility and one-attempt rules still apply. To undo, mark the WAIVE payment refunded.
      </p>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.6rem' }}>{msg.text}</div>}
      <div className="row" style={{ flexWrap: 'wrap' }}>
        <select value={cert} onChange={(e) => setCert(e.target.value)} style={{ maxWidth: 260 }}>
          <option value="">Default certification</option>
          {(data?.rows ?? []).map((c) => <option key={c.id} value={c.id}>{c.code} — {c.name}</option>)}
        </select>
        <input placeholder="Internal note (optional)" value={note} onChange={(e) => setNote(e.target.value)} style={{ maxWidth: 260 }} />
        <button className="btn sm" disabled={busy} onClick={grant}>{busy ? 'Granting…' : 'Grant exam access'}</button>
      </div>
    </Card>
  )
}

const DOC_TONE: Record<string, 'ok' | 'err' | 'brand'> = { verified: 'ok', rejected: 'err', submitted: 'brand' }

/** Identity documents: view the uploaded file (authenticated fetch → blob tab) and verify/reject. */
function IdentityDocs({ id, docs, onChanged }: { id: number; docs: IdentityDocRow[]; onChanged: () => void }) {
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState('')
  const [err, setErr] = useState<string | null>(null)

  async function review(docId: number, status: 'verified' | 'rejected') {
    setBusy(true)
    setErr(null)
    try {
      await adminApi.post(`/api/admin/students/${id}/identity-document/${docId}/review`, { status, note })
      setNote('')
      onChanged()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Review failed.')
    } finally {
      setBusy(false)
    }
  }

  async function view(docId: number) {
    setErr(null)
    try {
      const res = await fetch(`/api/admin/students/${id}/identity-document/${docId}/file`, {
        headers: { Authorization: 'Bearer ' + (adminApi.getToken() ?? '') },
      })
      if (!res.ok) throw new Error('Could not load the file.')
      const url = URL.createObjectURL(await res.blob())
      window.open(url, '_blank', 'noopener')
      setTimeout(() => URL.revokeObjectURL(url), 60_000)
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not load the file.')
    }
  }

  return (
    <Card title={`Identity documents (${docs.length})`}>
      {docs.length === 0 ? (
        <Empty>No government ID uploaded yet — the student cannot book an exam until they provide one.</Empty>
      ) : (
        <>
          {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
          <table className="data">
            <thead><tr><th>Type</th><th>File</th><th>Uploaded</th><th>Status</th><th /></tr></thead>
            <tbody>
              {docs.map((d) => (
                <tr key={d.id}>
                  <td>{(d.doc_kind ?? '').replace(/_/g, ' ')}</td>
                  <td className="small">{d.filename ?? '—'}{d.size_bytes ? ` · ${Math.round(Number(d.size_bytes) / 1024)} KB` : ''}</td>
                  <td className="small muted">{fmtDate(d.created_at)}</td>
                  <td>
                    <Badge tone={DOC_TONE[d.status] ?? 'brand'}>{d.status}</Badge>
                    {d.review_note && <div className="muted small">{d.review_note}</div>}
                  </td>
                  <td>
                    <div className="row" style={{ flexWrap: 'wrap', gap: '.35rem' }}>
                      <button className="btn sm secondary" onClick={() => view(d.id)}>View</button>
                      {d.status !== 'verified' && <button className="btn sm" disabled={busy} onClick={() => review(d.id, 'verified')}>Verify</button>}
                      {d.status !== 'rejected' && <button className="btn sm danger" disabled={busy} onClick={() => review(d.id, 'rejected')}>Reject</button>}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className="field" style={{ marginTop: '.75rem', marginBottom: 0 }}>
            <label htmlFor="idd-note">Review note (sent to the student with a rejection)</label>
            <input id="idd-note" value={note} onChange={(e) => setNote(e.target.value)} placeholder="e.g. photo unreadable — please re-upload" />
          </div>
        </>
      )}
    </Card>
  )
}

/** "View as student": mint a 60-minute support-view session (reason required, audit-logged). */
function ImpersonateControls({ id }: { id: number }) {
  const [open, setOpen] = useState(false)
  const [reason, setReason] = useState('')
  const [session, setSession] = useState<{ login_url: string } | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [note, setNote] = useState<string | null>(null)

  async function start() {
    if (!reason.trim()) { setErr('A reason is required (e.g. the ticket reference).'); return }
    setBusy(true); setErr(null); setNote(null)
    try {
      const r = await adminApi.post<{ login_url: string }>(`/api/admin/members/${id}/impersonate`, { reason: reason.trim() })
      setSession(r); setOpen(false); setReason('')
    } catch (e) {
      const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : null
      setErr(e instanceof ApiError && e.status === 403
        ? "You don't have the impersonate permission."
        : (typeof body?.message === 'string' && body.message) || (e as Error).message)
    } finally { setBusy(false) }
  }
  async function end() {
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/members/${id}/impersonate/end`, {})
      setSession(null); setNote('Impersonation sessions ended.')
    } catch (e) {
      setErr(e instanceof ApiError && e.status === 403 ? "You don't have the impersonate permission." : (e as Error).message)
    } finally { setBusy(false) }
  }

  return (
    <div style={{ marginTop: '.6rem' }}>
      {!session && !open && <button className="btn sm secondary" onClick={() => { setOpen(true); setNote(null) }}>View as student</button>}
      {!session && open && (
        <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
          <input placeholder="Reason (e.g. ticket reference)" value={reason} onChange={(e) => setReason(e.target.value)} style={{ maxWidth: 260 }} />
          <button className="btn sm" disabled={busy} onClick={start}>{busy ? 'Starting…' : 'Confirm'}</button>
          <button className="btn sm secondary" onClick={() => { setOpen(false); setReason(''); setErr(null) }}>Cancel</button>
        </div>
      )}
      {session && (
        <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap' }}>
          <a className="btn sm" href={session.login_url} target="_blank" rel="noreferrer">Open student portal ↗</a>
          <span className="muted small">expires in 60 min</span>
          <button className="btn sm danger" disabled={busy} onClick={end}>End session</button>
        </div>
      )}
      {note && <div className="muted small" style={{ marginTop: '.35rem' }} role="status">{note}</div>}
      {err && <div className="notice err" role="alert" style={{ marginTop: '.5rem' }}>{err}</div>}
    </div>
  )
}

function MemberDrawer({ id, onClose, onChanged }: { id: number; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<MemberDetail>(`/api/admin/members/${id}`)
  const [busy, setBusy] = useState(false)
  const [statusErr, setStatusErr] = useState<string | null>(null)

  async function setStatus(status: string) {
    setBusy(true)
    setStatusErr(null)
    try {
      await adminApi.post(`/api/admin/members/${id}/status`, { status })
      refetch()
      onChanged()
    } catch (e) {
      setStatusErr(e instanceof Error ? e.message : 'Could not change the member status.')
    } finally {
      setBusy(false)
    }
  }

  const u = (data?.user ?? {}) as Record<string, unknown>
  const isTest = Number(u.is_test ?? 0) === 1
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Student detail</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data ? null : (
          <div className="page">
            <Card className="entity" title={`${u.first_name ?? ''} ${u.last_name ?? ''}`.trim() || String(u.email ?? '')}
              action={<span className="row" style={{ gap: '.35rem' }}>{isTest && <Badge tone="warn">TEST ACCOUNT</Badge>}<StatusBadge status={String(u.status ?? '')} /></span>}>
              {isTest && <p className="muted small" style={{ marginTop: 0 }}>Test account — not a real candidate. Excluded from reports and the public register.</p>}
              <div className="grid cols-2 small">
                <div><span className="muted">Email</span><div>{String(u.email ?? '—')}</div></div>
                <div><span className="muted">Registration</span><div>{String(u.registration_no ?? '—')}</div></div>
                <div><span className="muted">Joined</span><div>{fmtDate(u.created_at)}</div></div>
                <div><span className="muted">Membership</span><div>{data.membership ? String((data.membership as Record<string, unknown>).status ?? '—') : 'None'}</div></div>
              </div>
              <div className="row" style={{ marginTop: '.9rem', flexWrap: 'wrap' }}>
                <span className="muted small">Set status:</span>
                {['active', 'pending', 'deactivated'].map((s) => (
                  <button key={s} className="btn sm secondary" disabled={busy || u.status === s} onClick={() => setStatus(s)}>{s}</button>
                ))}
              </div>
              {statusErr && <div className="notice err" role="alert" style={{ marginTop: '.6rem' }}>{statusErr}</div>}
              <ImpersonateControls id={id} />
            </Card>

            <Card title={`Payments (${data.payments.length})`}>
              {data.payments.length === 0 ? <Empty>No payments.</Empty> : (
                <table className="data">
                  <thead><tr><th>Date</th><th>Product</th><th>Amount</th><th>Status</th></tr></thead>
                  <tbody>
                    {data.payments.map((p, i) => (
                      <tr key={i}>
                        <td>{fmtDate(p.payment_date)}</td>
                        <td>{String(p.product_type ?? '')}</td>
                        <td>{fmtMoney(p.final_amount, String(p.currency ?? 'USD'))}</td>
                        <td><StatusBadge status={String(p.payment_status ?? '')} /></td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Card>

            <JourneyCard id={id} />

            <SecurityCard id={id} />

            <MarkPaidCard id={id} onDone={() => { refetch(); onChanged() }} />

            <WaiveCard id={id} onDone={() => { refetch(); onChanged() }} />

            <WaiverCard id={id} onGranted={() => { refetch(); onChanged() }} />

            <IdentityDocs id={id} docs={data.identity_documents ?? []} onChanged={refetch} />

            <Card title={`Credentials (${data.credentials.length})`}>
              {data.credentials.length === 0 ? <Empty>No credentials issued.</Empty> : (
                <table className="data">
                  <thead><tr><th>ID</th><th>Status</th><th>Expires</th></tr></thead>
                  <tbody>
                    {data.credentials.map((c, i) => (
                      <tr key={i}>
                        <td>{String(c.credential_id ?? '')}</td>
                        <td><StatusBadge status={String(c.status ?? '')} /></td>
                        <td>{fmtDate(c.expires_at)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Card>
          </div>
        )}
      </div>
    </div>
  )
}

// The student's end-to-end pipeline — where they are and, if stuck, why (and what to do about it).
interface JourneyStage { key: string; label: string; status: string; detail: string; recommended_action?: string | null; refs?: Record<string, unknown> | null }
interface Journey {
  stages: JourneyStage[]
  stuck_at: string | null
  stuck_reason: string | null
  stuck_action?: string | null
  membership_status: string
  delivery_mode: string
  open_tickets?: number
}
const STAGE_TONE: Record<string, 'ok' | 'warn' | 'err' | 'brand' | 'neutral'> = { done: 'ok', pending: 'neutral', action_required: 'warn', blocked: 'err' }
function JourneyCard({ id }: { id: number }) {
  const { data, loading, error } = useAdminQuery<Journey>(`/api/admin/members/${id}/journey`)
  return (
    <Card title="Student journey">
      {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : data ? (
        <>
          {data.stuck_at
            ? (
              <div className="notice" style={{ marginBottom: '.6rem', borderLeft: '3px solid var(--err,#dc2626)' }}>
                <strong>Stuck at: {data.stuck_at}</strong> — {data.stuck_reason}
                {data.stuck_action && <div className="small" style={{ marginTop: '.2rem' }}>→ {data.stuck_action}</div>}
              </div>
            )
            : <div className="notice" style={{ marginBottom: '.6rem' }}>No blockers — the student can proceed.</div>}
          <div style={{ display: 'grid', gap: '.35rem' }}>
            {data.stages.map((s) => (
              <div key={s.key}>
                <div className="row" style={{ gap: '.6rem', alignItems: 'center' }}>
                  <Badge tone={STAGE_TONE[s.status] ?? 'neutral'}>{s.status === 'action_required' ? 'action' : s.status}</Badge>
                  <span style={{ minWidth: 150 }}><strong>{s.label}</strong></span>
                  <span className="small muted">{s.detail}</span>
                </div>
                {s.recommended_action && (s.status === 'blocked' || s.status === 'action_required') && (
                  <div className="small muted" style={{ margin: '.1rem 0 0 .6rem' }}>→ {s.recommended_action}</div>
                )}
              </div>
            ))}
          </div>
          <div className="small muted" style={{ marginTop: '.5rem' }}>Delivery: {data.delivery_mode === 'in_house' ? 'in-house SecureExam' : data.delivery_mode} · Membership: {data.membership_status}</div>
          {(data.open_tickets ?? 0) > 0 && <div className="small" style={{ marginTop: '.35rem' }}>Open support tickets: {data.open_tickets}</div>}
        </>
      ) : null}
    </Card>
  )
}

// Security & access diagnostics: everything support needs to answer "why can't this student log
// in / what happened on this account" — plus the account-recovery and session-revocation actions.
interface SecurityData {
  logins: Record<string, unknown>[]
  security_events: Record<string, unknown>[]
  active_sessions: number
  impersonation_sessions: number
  pending_setup_links: number
  notifications: Record<string, unknown>[]
  error_reports: Record<string, unknown>[]
  open_tickets: Record<string, unknown>[]
}
const sv = (v: unknown): string => (v == null ? '' : String(v))

function ImpersonationHistory({ id }: { id: number }) {
  const [open, setOpen] = useState(false)
  // Fetch lazily — the ledger is gated on the 'impersonate' permission, so only load when opened.
  const { data, loading, error } = useAdminQuery<{ sessions: Record<string, unknown>[] }>(open ? `/api/admin/members/${id}/impersonations` : null)
  return (
    <div style={{ marginTop: '.75rem' }}>
      <button className="btn ghost sm" onClick={() => setOpen((o) => !o)} aria-expanded={open}>
        {open ? 'Hide impersonation history' : 'Impersonation history'}
      </button>
      {open && (
        loading && !data ? <Spinner /> : error ? <div className="notice err" role="alert" style={{ marginTop: '.5rem' }}>{error}</div> : !data ? null : (
          data.sessions.length === 0 ? <Empty>No staff has viewed this account as the student.</Empty> : (
            <table className="data" style={{ marginTop: '.5rem' }}>
              <thead><tr><th>Admin</th><th>Reason</th><th>Started</th><th>Ended</th><th>Events</th></tr></thead>
              <tbody>
                {data.sessions.map((r, i) => (
                  <tr key={i}>
                    <td className="small">{sv(r.admin_email) || `admin #${sv(r.admin_id)}`}</td>
                    <td className="small">{sv(r.reason) || '—'}</td>
                    <td className="small muted">{fmtDateTime(r.started_at)}</td>
                    <td className="small muted">{r.ended_at ? fmtDateTime(r.ended_at) : 'active'}</td>
                    <td className="small">{sv(r.events) || '0'}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )
        )
      )}
    </div>
  )
}

function SecurityCard({ id }: { id: number }) {
  const { data, loading, error, refetch } = useAdminQuery<SecurityData>(`/api/admin/members/${id}/security`)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)

  async function resendSetup() {
    setBusy(true); setMsg(null)
    try {
      const r = await adminApi.post<{ ok: boolean; setup_url?: string }>(`/api/admin/members/${id}/resend-setup`, {})
      setMsg({ ok: true, text: 'Password-reset link sent to the student.' + (r.setup_url ? ` Direct link (share securely): ${r.setup_url}` : '') })
      refetch()
    } catch (e) {
      setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not send the link.' })
    } finally { setBusy(false) }
  }
  async function revokeSessions() {
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/students/${id}/revoke-sessions`, {})
      setMsg({ ok: true, text: 'All active sessions revoked — the student must sign in again.' })
      refetch()
    } catch (e) {
      setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not revoke the sessions.' })
    } finally { setBusy(false) }
  }
  async function resetTwoFactor() {
    if (!window.confirm('Reset this student’s two-factor authentication? They will be able to sign in without a code and can re-enrol an authenticator. Active sessions are also revoked.')) return
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/students/${id}/reset-2fa`, {})
      setMsg({ ok: true, text: 'Two-factor authentication reset — the student can now sign in and re-enrol.' })
      refetch()
    } catch (e) {
      setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not reset two-factor authentication.' })
    } finally { setBusy(false) }
  }

  // login_events rows can vary by deployment — pick the common columns defensively.
  const loginWhen = (r: Record<string, unknown>) => fmtDateTime(r.created_at ?? r.at ?? r.ts)

  return (
    <Card title="Security & access">
      {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data ? null : (
        <>
          <div className="grid cols-2 small" style={{ marginBottom: '.6rem' }}>
            <div><span className="muted">Active sessions</span><div><strong>{data.active_sessions}</strong></div></div>
            <div><span className="muted">Pending setup links</span><div><strong>{data.pending_setup_links}</strong></div></div>
            <div><span className="muted">Impersonation sessions (live)</span><div><strong>{data.impersonation_sessions}</strong></div></div>
            <div><span className="muted">Open tickets</span><div><strong>{data.open_tickets.length}</strong></div></div>
          </div>
          <div className="row" style={{ flexWrap: 'wrap', gap: '.4rem' }}>
            <button className="btn sm secondary" disabled={busy} onClick={resendSetup}>Send password-reset link</button>
            <button className="btn sm secondary" disabled={busy} onClick={resetTwoFactor}>Reset 2FA</button>
            <button className="btn sm danger" disabled={busy} onClick={revokeSessions}>Revoke sessions</button>
          </div>
          {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginTop: '.5rem', overflowWrap: 'anywhere' }}>{msg.text}</div>}

          <h4 style={{ margin: '.9rem 0 .3rem' }}>Last logins</h4>
          {data.logins.length === 0 ? <Empty>No logins recorded.</Empty> : (
            <table className="data">
              <thead><tr><th>When</th><th>IP</th><th>Device</th><th>Outcome</th></tr></thead>
              <tbody>
                {data.logins.slice(0, 5).map((r, i) => (
                  <tr key={i}>
                    <td className="small muted">{loginWhen(r)}</td>
                    <td className="small">{sv(r.ip) || '—'}</td>
                    <td className="small">{sv(r.device) || sv(r.user_agent).slice(0, 40) || '—'}</td>
                    <td><StatusBadge status={sv(r.outcome) || 'success'} /></td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}

          {data.security_events.length > 0 && (
            <>
              <h4 style={{ margin: '.9rem 0 .3rem' }}>Security events</h4>
              <table className="data">
                <thead><tr><th>When</th><th>Kind</th><th>Detail</th></tr></thead>
                <tbody>
                  {data.security_events.slice(0, 5).map((r, i) => (
                    <tr key={i}>
                      <td className="small muted">{fmtDateTime(r.created_at ?? r.at)}</td>
                      <td className="small">{sv(r.kind) || '—'}</td>
                      <td className="small">{sv(r.detail) || '—'}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </>
          )}

          {data.error_reports.length > 0 && (
            <>
              <h4 style={{ margin: '.9rem 0 .3rem' }}>Recent error reports</h4>
              <div style={{ display: 'grid', gap: '.25rem' }}>
                {data.error_reports.slice(0, 5).map((r, i) => (
                  <div key={i} className="small">
                    <span style={{ fontFamily: 'monospace' }}>{sv(r.reference)}</span>
                    <span className="muted"> · {sv(r.page) || '—'} · </span>
                    <StatusBadge status={sv(r.status)} />
                  </div>
                ))}
              </div>
            </>
          )}

          {data.open_tickets.length > 0 && (
            <>
              <h4 style={{ margin: '.9rem 0 .3rem' }}>Open tickets</h4>
              <div style={{ display: 'grid', gap: '.25rem' }}>
                {data.open_tickets.map((t, i) => (
                  <div key={i} className="small">
                    <strong>{sv(t.reference)}</strong> {sv(t.subject) || '—'} · <StatusBadge status={sv(t.status)} />
                  </div>
                ))}
              </div>
            </>
          )}

          <ImpersonationHistory id={id} />
        </>
      )}
    </Card>
  )
}

// Mark a fee as paid offline (real payment that didn't reflect) or grant it free (amount 0 = waiver).
function MarkPaidCard({ id, onDone }: { id: number; onDone: () => void }) {
  const [product, setProduct] = useState('exam')
  const [amount, setAmount] = useState('0')
  const [note, setNote] = useState('')
  // payment evidence (all optional — recorded on the payment row for reconciliation)
  const [method, setMethod] = useState('')
  const [bankRef, setBankRef] = useState('')
  const [gatewayRef, setGatewayRef] = useState('')
  const [receiptNo, setReceiptNo] = useState('')
  const [paidAt, setPaidAt] = useState('')
  // duplicate-grant override: offered only after the backend refuses with already_entitled / membership_already_active
  const [needsOverride, setNeedsOverride] = useState(false)
  const [allowDup, setAllowDup] = useState(false)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; warn?: boolean; text: string } | null>(null)
  async function go() {
    setBusy(true); setMsg(null)
    try {
      const body: Record<string, unknown> = { product, amount: parseFloat(amount) || 0, note: note || undefined }
      if (method) body.method = method
      if (bankRef) body.bank_reference = bankRef
      if (gatewayRef) body.gateway_reference = gatewayRef
      if (receiptNo) body.receipt_no = receiptNo
      if (paidAt) body.paid_at = paidAt
      if (allowDup) body.allow_duplicate = true
      const r = await adminApi.post<{ reference: string; free: boolean; mismatch?: boolean; list_price?: number }>(`/api/admin/students/${id}/mark-paid`, body)
      setNeedsOverride(false); setAllowDup(false)
      if (r.mismatch) setMsg({ ok: true, warn: true, text: `Recorded — note: amount differs from the $${r.list_price} list price.` })
      else setMsg({ ok: true, text: `${product} recorded (${r.free ? 'free / waived' : 'ref ' + r.reference}). The student can now proceed.` })
      onDone()
    } catch (e) {
      const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : null
      const code = typeof body?.error === 'string' ? body.error : ''
      if (code === 'duplicate_reference') setMsg({ ok: false, text: (typeof body?.message === 'string' && body.message) || 'That gateway reference is already recorded on another payment.' })
      else if (code === 'already_entitled' || code === 'membership_already_active') { setNeedsOverride(true); setMsg({ ok: false, text: "Already granted — tick 'Record anyway' to override." }) }
      else setMsg({ ok: false, text: (e as Error).message })
    } finally { setBusy(false) }
  }
  return (
    <Card title="Mark as paid / waive fee">
      <p className="muted small" style={{ marginTop: 0 }}>Record an offline payment, or set amount 0 to grant it free. Creates the same paid entitlement a card payment would, so the student can schedule immediately.</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label>Product<select value={product} onChange={(e) => setProduct(e.target.value)}><option value="exam">Exam</option><option value="membership">Membership</option><option value="bundle">Bundle (membership + exam)</option></select></label>
        <label>Amount (USD)<input type="number" min="0" value={amount} onChange={(e) => setAmount(e.target.value)} style={{ maxWidth: 120 }} /></label>
        <label style={{ flex: 1, minWidth: 160 }}>Note (optional)<input value={note} onChange={(e) => setNote(e.target.value)} placeholder="e.g. bank transfer received" /></label>
        <button className="btn sm" disabled={busy} onClick={go}>{busy ? 'Recording…' : (parseFloat(amount) > 0 ? 'Mark paid' : 'Grant free')}</button>
      </div>
      <details style={{ marginTop: '.6rem' }}>
        <summary className="small" style={{ cursor: 'pointer', fontWeight: 600 }}>Payment evidence (optional)</summary>
        <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end', marginTop: '.5rem' }}>
          <label>Method
            <select value={method} onChange={(e) => setMethod(e.target.value)} style={{ maxWidth: 160 }}>
              <option value="">—</option>
              <option value="bank_transfer">Bank transfer</option>
              <option value="cheque">Cheque</option>
              <option value="invoice">Invoice</option>
              <option value="gateway">Gateway</option>
              <option value="other">Other</option>
            </select>
          </label>
          <label>Bank reference<input value={bankRef} onChange={(e) => setBankRef(e.target.value)} style={{ maxWidth: 160 }} /></label>
          <label>Gateway reference<input value={gatewayRef} onChange={(e) => setGatewayRef(e.target.value)} style={{ maxWidth: 160 }} /></label>
          <label>Receipt no.<input value={receiptNo} onChange={(e) => setReceiptNo(e.target.value)} style={{ maxWidth: 140 }} /></label>
          <label>Paid on<input type="date" value={paidAt} onChange={(e) => setPaidAt(e.target.value)} style={{ maxWidth: 160 }} /></label>
        </div>
      </details>
      {needsOverride && (
        <label className="row small" style={{ gap: '.4rem', marginTop: '.5rem' }}>
          <input type="checkbox" checked={allowDup} onChange={(e) => setAllowDup(e.target.checked)} style={{ width: 'auto' }} /> Record anyway (override the duplicate check)
        </label>
      )}
      {msg && <div className={'notice' + (msg.ok ? (msg.warn ? ' warn' : '') : ' err')} style={{ marginTop: '.5rem' }}>{msg.text}</div>}
    </Card>
  )
}

// Waive a fee entirely (immediate access) or partially (a single-use, student-locked discount code).
function WaiveCard({ id, onDone }: { id: number; onDone: () => void }) {
  const [product, setProduct] = useState('exam')
  const [percent, setPercent] = useState('100')
  const [reason, setReason] = useState('')
  const [note, setNote] = useState('')
  const [expires, setExpires] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)
  const pct = parseFloat(percent) || 0
  async function go() {
    if (!reason.trim()) { setMsg({ ok: false, text: 'A reason is required — every waiver must record why.' }); return }
    if (busy) return
    setBusy(true); setMsg(null)
    // Durable client key so a retried/partial network submit cannot mint a second waiver or code.
    const idempotencyKey = crypto.randomUUID()
    try {
      const body: Record<string, unknown> = { product, percent: pct || 100, reason: reason.trim(), note: note || undefined, idempotency_key: idempotencyKey }
      if (pct < 100 && expires) body.expires = expires
      const r = await adminApi.post<{ kind: string; payment_id?: number; code?: string; percent?: number; payable?: number }>(`/api/admin/students/${id}/waive`, body)
      setMsg({
        ok: true,
        text: r.kind === 'full'
          ? `Waived in full — access granted (payment #${r.payment_id}).`
          : `Code ${r.code} created: ${r.percent}% off, payable $${r.payable}.`,
      })
      onDone()
    } catch (e) {
      const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : null
      const code = typeof body?.error === 'string' ? body.error : ''
      setMsg({ ok: false, text: code === 'reason_required' ? 'A reason is required — every waiver must record why.' : (e as Error).message })
    } finally { setBusy(false) }
  }
  return (
    <Card title="Waive fee">
      <p className="muted small" style={{ marginTop: 0 }}>100% grants access immediately; less than 100% creates a discount code locked to this student, who pays the remainder through the normal checkout.</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label>Product<select value={product} onChange={(e) => setProduct(e.target.value)}><option value="exam">Exam</option><option value="membership">Membership</option><option value="bundle">Bundle (membership + exam)</option></select></label>
        <label>Percent<input type="number" min="1" max="100" value={percent} onChange={(e) => setPercent(e.target.value)} style={{ maxWidth: 100 }} /></label>
        <label style={{ flex: 1, minWidth: 180 }}>Reason (required)<input value={reason} onChange={(e) => setReason(e.target.value)} placeholder="scholarship / sponsorship / promotion…" /></label>
        <label style={{ flex: 1, minWidth: 160 }}>Note (optional)<input value={note} onChange={(e) => setNote(e.target.value)} /></label>
        {pct < 100 && <label>Code expires<input type="date" value={expires} onChange={(e) => setExpires(e.target.value)} style={{ maxWidth: 160 }} /></label>}
        <button className="btn sm" disabled={busy} onClick={go}>{busy ? 'Waiving…' : pct >= 100 ? 'Waive in full' : 'Create code'}</button>
      </div>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} style={{ marginTop: '.5rem' }}>{msg.text}</div>}
    </Card>
  )
}

// Scenario-based test account (no payment needed) — returns login credentials + a ready session.
const TEST_SCENARIOS = [
  { value: 'ready', label: 'Ready — fully unlocked' },
  { value: 'unpaid', label: 'Unpaid — fresh account' },
  { value: 'member', label: 'Member — membership only' },
  { value: 'waived', label: 'Waived — exam waived' },
  { value: 'incomplete_profile', label: 'Incomplete profile' },
  { value: 'no_id', label: 'No ID' },
  { value: 'certuvo_failed', label: 'Certuvo failed' },
]
function TestUserButton({ onCreated }: { onCreated: () => void }) {
  const [scenario, setScenario] = useState('ready')
  const [busy, setBusy] = useState(false)
  const [res, setRes] = useState<{ email: string; password: string; token: string; scenario: string } | null>(null)
  const [err, setErr] = useState<string | null>(null)
  async function create() {
    setBusy(true); setErr(null); setRes(null)
    try { setRes(await adminApi.post('/api/admin/test-users', { scenario })); onCreated() }
    catch (e) { setErr((e as Error).message) } finally { setBusy(false) }
  }
  return (
    <>
      <select value={scenario} onChange={(e) => setScenario(e.target.value)} aria-label="Test-user scenario" style={{ width: 'auto', maxWidth: 220 }}>
        {TEST_SCENARIOS.map((s) => <option key={s.value} value={s.value}>{s.label}</option>)}
      </select>
      <button className="btn sm" disabled={busy} onClick={create}>{busy ? 'Creating…' : '+ Test user'}</button>
      {(res || err) && (
        <div className="drawer-backdrop" onClick={() => { setRes(null); setErr(null) }}>
          <div className="drawer" onClick={(e) => e.stopPropagation()} style={{ maxWidth: 460 }}>
            <div className="spread" style={{ marginBottom: '.8rem' }}><h2 style={{ margin: 0 }}>Test user</h2><button className="btn secondary sm" onClick={() => { setRes(null); setErr(null) }}>Close</button></div>
            {err ? <ErrorNote>{err}</ErrorNote> : res ? (
              <Card title="Test account created">
                <p className="muted small" style={{ marginTop: 0 }}>Ready to exercise the portal without paying or touching real data. Test accounts are excluded from reports and the public register.</p>
                <div className="small" style={{ display: 'grid', gap: '.35rem' }}>
                  <div>Scenario: <strong>{TEST_SCENARIOS.find((s) => s.value === res.scenario)?.label ?? res.scenario}</strong></div>
                  <div>Email: <strong>{res.email}</strong></div>
                  <div>Password: <strong>{res.password}</strong></div>
                </div>
                <div className="row" style={{ marginTop: '.7rem' }}>
                  <a className="btn sm" href={`/app/#t=${res.token}`} target="_blank" rel="noreferrer">Open portal as this user ↗</a>
                </div>
              </Card>
            ) : null}
          </div>
        </div>
      )}
    </>
  )
}

export default function Students() {
  const [q, setQ] = useState('')
  const [dq, setDq] = useState('')
  const [status, setStatus] = useState('')
  const [selected, setSelected] = useState<number | null>(null)
  // Debounce the search term so the query path (and its page-level Spinner) doesn't churn on
  // every keystroke — the input stays instant while the list refetches once typing settles.
  useEffect(() => {
    const t = setTimeout(() => setDq(q), 300)
    return () => clearTimeout(t)
  }, [q])
  const params = new URLSearchParams()
  if (dq) params.set('q', dq)
  if (status) params.set('status', status)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: MemberRow[]; total: number }>(`/api/admin/members${qs ? '?' + qs : ''}`)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <PageHeader
        title="Students"
        actions={<div className="row" style={{ gap: '.5rem', alignItems: 'center' }}>
          {data && <span className="muted small">{data.total} total</span>}
          <TestUserButton onCreated={refetch} />
        </div>}
      />

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input aria-label="Search name or email" placeholder="Search name or email…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 320 }} />
          <select aria-label="Filter by status" value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }}>
            <option value="">All statuses</option>
            <option value="active">Active</option>
            <option value="pending">Pending</option>
            <option value="deactivated">Deactivated</option>
          </select>
        </div>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No students match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Name</th><th>Email</th><th>Status</th><th>Membership</th><th>Paid</th><th>Creds</th><th>Joined</th></tr>
            </thead>
            <tbody>
              {data.rows.map((m) => (
                <tr key={m.id} {...rowActivate(() => setSelected(m.id))}>
                  <td><strong>{`${m.first_name ?? ''} ${m.last_name ?? ''}`.trim() || '—'}</strong>{m.is_test ? <> <Badge tone="warn">TEST ACCOUNT</Badge></> : null}</td>
                  <td className="small">{m.email}</td>
                  <td><StatusBadge status={m.status} /></td>
                  <td className="small">{m.membership_status || '—'}</td>
                  <td>{fmtMoney(m.paid_total ?? 0)}</td>
                  <td>{m.credentials ?? 0}</td>
                  <td className="small muted">{fmtDate(m.created_at)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {selected !== null && <MemberDrawer id={selected} onClose={() => setSelected(null)} onChanged={refetch} />}
    </div>
  )
}
