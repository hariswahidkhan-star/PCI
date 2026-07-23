import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useAdminQuery } from '../hooks'
import { adminApi, type DiscountCode } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'

interface CertOpt { id: number; code: string; acronym?: string | null }
interface PartnerOpt { id: number; name: string }
function CreateForm({ onClose, onSaved }: { onClose: () => void; onSaved: () => void }) {
  const { data: certData } = useAdminQuery<{ rows: CertOpt[] }>('/api/admin/certifications')
  const { data: partnerData } = useAdminQuery<{ rows: PartnerOpt[] }>('/api/admin/training-partners')
  const [f, setF] = useState({
    code: '', discount_type: 'percentage', discount_value: '', applies_to: 'all',
    certification_id: '', max_discount: '', partner_id: '',
    start_date: '', end_date: '', max_uses: '', single_use_per_email: false, active: true,
    founding_route: '', grants_membership: true, grants_exam: true, grants_study_access: true,
    requires_application: false,
    auto_approve: true, membership_months: '12', min_years: '', require_qual: false, allowed_roles: '',
  })
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const founding = f.founding_route !== ''

  async function save() {
    setBusy(true)
    setError(null)
    try {
      const criteria: Record<string, unknown> = {}
      if (f.min_years !== '') criteria.min_experience_years = Number(f.min_years)
      if (f.require_qual) criteria.require_qualification = true
      if (f.allowed_roles.trim()) criteria.allowed_roles = f.allowed_roles.split(',').map((r) => r.trim()).filter(Boolean)
      await adminApi.post('/api/admin/codes', {
        code: f.code.toUpperCase(),
        discount_type: f.discount_type,
        discount_value: Number(f.discount_value) || 0,
        applies_to: f.applies_to,
        certification_id: f.certification_id === '' ? null : Number(f.certification_id),
        max_discount: f.max_discount === '' ? null : Number(f.max_discount),
        partner_id: f.partner_id === '' ? null : Number(f.partner_id),
        start_date: f.start_date || null,
        end_date: f.end_date || null,
        max_uses: f.max_uses === '' ? null : Number(f.max_uses),
        single_use_per_email: f.single_use_per_email,
        active: f.active,
        ...(founding
          ? {
              founding_route: 'founding',
              grants_membership: f.grants_membership,
              grants_exam: f.grants_exam,
              grants_study_access: f.grants_study_access,
              requires_application: f.requires_application,
              auto_approve: f.auto_approve,
              membership_months: Number(f.membership_months) || 12,
              criteria_json: f.requires_application && Object.keys(criteria).length > 0 ? JSON.stringify(criteria) : null,
            }
          : {}),
      })
      onSaved()
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Could not create code.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>New code</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
        <div className="field"><label>Kind</label>
          <select value={f.founding_route} onChange={(e) => setF({ ...f, founding_route: e.target.value })}>
            <option value="">Standard discount code</option>
            <option value="founding">Founding code — waives membership, study and exam fees (time-boxed)</option>
          </select>
        </div>
        <div className="grid cols-2">
          <div className="field"><label>Code</label><input value={f.code} onChange={(e) => setF({ ...f, code: e.target.value.toUpperCase() })} placeholder={founding ? 'FOUNDING2026' : 'LAUNCH20'} /></div>
          {!founding && (
            <div className="field"><label>Type</label>
              <select value={f.discount_type} onChange={(e) => setF({ ...f, discount_type: e.target.value })}>
                <option value="percentage">Percentage</option>
                <option value="fixed">Fixed amount</option>
              </select>
            </div>
          )}
          {!founding && (
            <div className="field"><label>Value {f.discount_type === 'percentage' ? '(%)' : '(USD)'}</label><input type="number" value={f.discount_value} onChange={(e) => setF({ ...f, discount_value: e.target.value })} /></div>
          )}
          {!founding && (
            <div className="field"><label>Applies to</label>
              <select value={f.applies_to} onChange={(e) => setF({ ...f, applies_to: e.target.value })}>
                <option value="all">Both — membership + exam fee</option>
                <option value="membership">Membership only</option>
                <option value="exam">Exam fee only</option>
              </select>
              <span className="muted small">Scopes the discount. An exam-only code is rejected on a membership purchase, and vice-versa.</span>
            </div>
          )}
          {!founding && (
            <div className="field"><label>Certification</label>
              <select value={f.certification_id} onChange={(e) => setF({ ...f, certification_id: e.target.value })}>
                <option value="">All certifications</option>
                {certData?.rows.map((c) => <option key={c.id} value={c.id}>{c.acronym || c.code}</option>)}
              </select>
              <span className="muted small">Restrict the code to one credential, or leave as “All certifications”.</span>
            </div>
          )}
          {!founding && (
            <div className="field"><label>Max discount (USD, blank = none)</label><input type="number" value={f.max_discount} onChange={(e) => setF({ ...f, max_discount: e.target.value })} /></div>
          )}
          {!founding && (
            <div className="field"><label>Partner attribution</label>
              <select value={f.partner_id} onChange={(e) => setF({ ...f, partner_id: e.target.value })}>
                <option value="">None</option>
                {partnerData?.rows.map((p) => <option key={p.id} value={p.id}>{p.name}</option>)}
              </select>
              <span className="muted small">Paid redemptions of this code accrue commission for the chosen partner.</span>
            </div>
          )}
          <div className="field"><label>Window opens</label><input type="date" value={f.start_date} onChange={(e) => setF({ ...f, start_date: e.target.value })} /></div>
          <div className="field"><label>Window closes {founding ? '(the founding end date)' : ''}</label><input type="date" value={f.end_date} onChange={(e) => setF({ ...f, end_date: e.target.value })} /></div>
          <div className="field"><label>Max total uses (blank = unlimited)</label><input type="number" value={f.max_uses} onChange={(e) => setF({ ...f, max_uses: e.target.value })} /></div>
          {founding && (
            <div className="field"><label>Membership term (months)</label><input type="number" value={f.membership_months} onChange={(e) => setF({ ...f, membership_months: e.target.value })} /></div>
          )}
        </div>

        {founding && (
          <>
            <p className="muted small" style={{ marginTop: 0 }}>
              A founding code grants the full free path to the credential — the exam is the same real
              exam, only the fees are waived. Untick a grant only if the board wants a narrower code.
            </p>
            <div className="row" style={{ gap: '1.5rem', marginBottom: '1rem', flexWrap: 'wrap' }}>
              <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.grants_membership} onChange={(e) => setF({ ...f, grants_membership: e.target.checked })} /> Grants membership</label>
              <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.grants_exam} onChange={(e) => setF({ ...f, grants_exam: e.target.checked })} /> Grants exam entry</label>
              <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.grants_study_access} onChange={(e) => setF({ ...f, grants_study_access: e.target.checked })} /> Grants study access</label>
              <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.requires_application} onChange={(e) => setF({ ...f, requires_application: e.target.checked, auto_approve: e.target.checked ? false : f.auto_approve })} /> Require an application (evidence + criteria)</label>
            </div>
            {f.requires_application && (
              <Card title="Eligibility criteria (board-set — enforced on application)">
                <div className="grid cols-2">
                  <div className="field"><label>Minimum experience (years)</label><input type="number" value={f.min_years} onChange={(e) => setF({ ...f, min_years: e.target.value })} placeholder="e.g. 3" /></div>
                  <div className="field"><label>Allowed roles (comma-separated, blank = any)</label><input value={f.allowed_roles} onChange={(e) => setF({ ...f, allowed_roles: e.target.value })} placeholder="project controls, planner, cost engineer" /></div>
                </div>
                <div className="row" style={{ gap: '1.5rem', flexWrap: 'wrap' }}>
                  <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.require_qual} onChange={(e) => setF({ ...f, require_qual: e.target.checked })} /> Require a qualification</label>
                  <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.auto_approve} onChange={(e) => setF({ ...f, auto_approve: e.target.checked })} /> Auto-approve applications that meet the criteria</label>
                </div>
              </Card>
            )}
          </>
        )}

        <div className="row" style={{ gap: '1.5rem', margin: '1rem 0' }}>
          <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.single_use_per_email} onChange={(e) => setF({ ...f, single_use_per_email: e.target.checked })} /> One use per email</label>
          <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.active} onChange={(e) => setF({ ...f, active: e.target.checked })} /> Active</label>
        </div>
        <button className="btn" disabled={busy || !f.code || (!founding && !f.discount_value)} onClick={save}>{busy ? 'Creating…' : 'Create code'}</button>
      </div>
    </div>
  )
}

/** Window/caps/approval editor — lets the board extend the founding window with one edit. */
function EditForm({ code, onClose, onSaved }: { code: DiscountCode; onClose: () => void; onSaved: () => void }) {
  const { data: certData } = useAdminQuery<{ rows: CertOpt[] }>('/api/admin/certifications')
  const { data: partnerData } = useAdminQuery<{ rows: PartnerOpt[] }>('/api/admin/training-partners')
  const [f, setF] = useState({
    start_date: code.start_date ?? '',
    end_date: code.end_date ?? '',
    max_uses: code.max_uses == null ? '' : String(code.max_uses),
    applies_to: code.applies_to ?? 'all',
    certification_id: code.certification_id == null ? '' : String(code.certification_id),
    max_discount: code.max_discount == null ? '' : String(code.max_discount),
    partner_id: code.partner_id == null ? '' : String(code.partner_id),
    discount_type: code.discount_type ?? 'percentage',
    discount_value: code.discount_value == null ? '' : String(code.discount_value),
    auto_approve: (code.auto_approve ?? 1) === 1,
    membership_months: String(code.membership_months ?? 12),
    criteria_json: code.criteria_json ?? '',
    grants_membership: (code.grants_membership ?? 1) === 1,
    grants_exam: (code.grants_exam ?? 1) === 1,
    grants_study_access: (code.grants_study_access ?? 1) === 1,
    requires_application: (code.requires_application ?? 0) === 1,
  })
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const founding = !!code.founding_route

  async function save() {
    setBusy(true)
    setError(null)
    try {
      if (f.criteria_json.trim()) JSON.parse(f.criteria_json) // guard: reject malformed board JSON client-side
      await adminApi.patch(`/api/admin/codes/${code.id}`, {
        start_date: f.start_date || null,
        end_date: f.end_date || null,
        max_uses: f.max_uses === '' ? null : Number(f.max_uses),
        ...(founding
          ? {}
          : {
              applies_to: f.applies_to,
              discount_type: f.discount_type,
              discount_value: Number(f.discount_value) || 0,
              certification_id: f.certification_id === '' ? null : Number(f.certification_id),
              max_discount: f.max_discount === '' ? null : Number(f.max_discount),
              partner_id: f.partner_id === '' ? null : Number(f.partner_id),
            }),
        ...(founding
          ? {
              auto_approve: f.auto_approve,
              membership_months: Number(f.membership_months) || 12,
              criteria_json: f.criteria_json.trim() || null,
              grants_membership: f.grants_membership,
              grants_exam: f.grants_exam,
              grants_study_access: f.grants_study_access,
              requires_application: f.requires_application,
            }
          : {}),
      })
      onSaved()
    } catch (e) {
      setError(e instanceof SyntaxError ? 'Criteria must be valid JSON.' : e instanceof Error ? e.message : 'Could not save.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Edit {code.code}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
        <div className="grid cols-2">
          {!founding && (
            <div className="field"><label>Type</label>
              <select value={f.discount_type} onChange={(e) => setF({ ...f, discount_type: e.target.value })}>
                <option value="percentage">Percentage</option>
                <option value="fixed">Fixed amount</option>
              </select>
            </div>
          )}
          {!founding && (
            <div className="field"><label>Value {f.discount_type === 'percentage' ? '(%)' : '(USD)'}</label><input type="number" value={f.discount_value} onChange={(e) => setF({ ...f, discount_value: e.target.value })} /></div>
          )}
          {!founding && (
            <div className="field"><label>Applies to</label>
              <select value={f.applies_to} onChange={(e) => setF({ ...f, applies_to: e.target.value })}>
                <option value="all">Both — membership + exam fee</option>
                <option value="membership">Membership only</option>
                <option value="exam">Exam fee only</option>
              </select>
            </div>
          )}
          {!founding && (
            <div className="field"><label>Certification</label>
              <select value={f.certification_id} onChange={(e) => setF({ ...f, certification_id: e.target.value })}>
                <option value="">All certifications</option>
                {certData?.rows.map((c) => <option key={c.id} value={c.id}>{c.acronym || c.code}</option>)}
              </select>
            </div>
          )}
          {!founding && (
            <div className="field"><label>Max discount (USD, blank = none)</label><input type="number" value={f.max_discount} onChange={(e) => setF({ ...f, max_discount: e.target.value })} /></div>
          )}
          {!founding && (
            <div className="field"><label>Partner attribution</label>
              <select value={f.partner_id} onChange={(e) => setF({ ...f, partner_id: e.target.value })}>
                <option value="">None</option>
                {partnerData?.rows.map((p) => <option key={p.id} value={p.id}>{p.name}</option>)}
              </select>
            </div>
          )}
          <div className="field"><label>Window opens</label><input type="date" value={f.start_date.slice(0, 10)} onChange={(e) => setF({ ...f, start_date: e.target.value })} /></div>
          <div className="field"><label>Window closes</label><input type="date" value={f.end_date.slice(0, 10)} onChange={(e) => setF({ ...f, end_date: e.target.value })} /></div>
          <div className="field"><label>Max total uses (blank = unlimited)</label><input type="number" value={f.max_uses} onChange={(e) => setF({ ...f, max_uses: e.target.value })} /></div>
          {founding && (
            <div className="field"><label>Membership term (months)</label><input type="number" value={f.membership_months} onChange={(e) => setF({ ...f, membership_months: e.target.value })} /></div>
          )}
        </div>
        {founding && (
          <div className="row" style={{ gap: '1.5rem', marginBottom: '1rem', flexWrap: 'wrap' }}>
            <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.grants_membership} onChange={(e) => setF({ ...f, grants_membership: e.target.checked })} /> Grants membership</label>
            <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.grants_exam} onChange={(e) => setF({ ...f, grants_exam: e.target.checked })} /> Grants exam entry</label>
            <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.grants_study_access} onChange={(e) => setF({ ...f, grants_study_access: e.target.checked })} /> Grants study access</label>
            <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.requires_application} onChange={(e) => setF({ ...f, requires_application: e.target.checked, auto_approve: e.target.checked ? false : f.auto_approve })} /> Require an application</label>
          </div>
        )}
        {founding && f.requires_application && (
          <>
            <div className="row" style={{ marginBottom: '1rem' }}>
              <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.auto_approve} onChange={(e) => setF({ ...f, auto_approve: e.target.checked })} /> Auto-approve applications that meet the criteria</label>
            </div>
            <div className="field"><label>Criteria (JSON — board thresholds)</label>
              <textarea rows={3} value={f.criteria_json} onChange={(e) => setF({ ...f, criteria_json: e.target.value })} placeholder={'{"min_experience_years": 3, "require_qualification": true, "allowed_roles": ["project controls"]}'} />
            </div>
          </>
        )}
        <button className="btn" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save changes'}</button>
      </div>
    </div>
  )
}

// ---------------- Approvals: institution codes awaiting a PCI decision ----------------
interface ApprovalRow {
  id: number
  code: string
  discount_value?: number | null
  applies_to?: string | null
  max_uses?: number | null
  start_date?: string | null
  end_date?: string | null
  campaign_name?: string | null
  notes?: string | null
  created_at?: string | null
  institution?: string | null
  requested_by?: string | null
}

function ApprovalsTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: ApprovalRow[] }>('/api/admin/code-approvals')
  const [rejecting, setRejecting] = useState<number | null>(null)
  const [reason, setReason] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)
  const rows = data?.rows ?? []

  async function approve(r: ApprovalRow) {
    setBusy(true); setMsg(null)
    try { await adminApi.post(`/api/admin/code-approvals/${r.id}/approve`, {}); setMsg({ ok: true, text: `Code ${r.code} approved and live.` }); refetch() }
    catch (e) { setMsg({ ok: false, text: (e as Error).message }) } finally { setBusy(false) }
  }
  async function reject(r: ApprovalRow) {
    if (!reason.trim()) { setMsg({ ok: false, text: 'A rejection reason is required — it is sent to the institution.' }); return }
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/code-approvals/${r.id}/reject`, { reason: reason.trim() })
      setMsg({ ok: true, text: `Code ${r.code} rejected.` }); setRejecting(null); setReason(''); refetch()
    } catch (e) { setMsg({ ok: false, text: (e as Error).message }) } finally { setBusy(false) }
  }

  return (
    <Card title={`Pending institution codes (${rows.length})`}>
      <p className="muted small" style={{ marginTop: 0 }}>Codes submitted by training partners that need a PCI decision before they go live. The institution is notified either way.</p>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.6rem' }}>{msg.text}</div>}
      {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
        <Empty>Nothing awaiting approval.</Empty>
      ) : (
        <table className="data">
          <thead><tr><th>Code</th><th>Institution</th><th>%</th><th>Uses</th><th>Requested by</th><th>Window</th><th /></tr></thead>
          <tbody>
            {rows.map((r) => (
              <tr key={r.id}>
                <td><strong>{r.code}</strong>{r.campaign_name && <div className="muted small">{r.campaign_name}</div>}</td>
                <td className="small">{r.institution || '—'}</td>
                <td className="small">{r.discount_value ?? 0}%{r.applies_to && r.applies_to !== 'all' ? ` (${r.applies_to})` : ''}</td>
                <td className="small">{r.max_uses ?? '∞'}</td>
                <td className="small">{r.requested_by || '—'}<div className="muted">{fmtDate(r.created_at)}</div></td>
                <td className="small muted">{r.start_date ? `${fmtDate(r.start_date)} – ` : ''}{r.end_date ? fmtDate(r.end_date) : '—'}</td>
                <td>
                  {rejecting === r.id ? (
                    <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                      <input placeholder="Reason (sent to the institution)" value={reason} onChange={(e) => setReason(e.target.value)} style={{ maxWidth: 220 }} />
                      <button className="btn sm danger" disabled={busy} onClick={() => reject(r)}>Reject</button>
                      <button className="btn ghost sm" onClick={() => { setRejecting(null); setReason('') }}>Cancel</button>
                    </div>
                  ) : (
                    <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                      <button className="btn sm" disabled={busy} onClick={() => approve(r)}>Approve</button>
                      <button className="btn ghost sm danger" disabled={busy} onClick={() => { setRejecting(r.id); setReason('') }}>Reject…</button>
                    </div>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </Card>
  )
}

// ---------------- Review queue: automatic abuse flags on code usage ----------------
interface FraudFlag {
  id: number
  kind: string
  code_id?: number | null
  code?: string | null
  user_id?: number | null
  email?: string | null
  detail?: string | null
  status?: string | null
  created_at?: string | null
}

function ReviewQueueTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: FraudFlag[] }>('/api/admin/fraud-flags')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)
  const rows = data?.rows ?? []

  async function act(f: FraudFlag, action: 'clear' | 'suspend_code') {
    setBusy(true); setMsg(null)
    try {
      await adminApi.post(`/api/admin/fraud-flags/${f.id}/action`, { action })
      setMsg({ ok: true, text: action === 'clear' ? 'Flag cleared.' : `Flag actioned — code ${f.code ?? ''} suspended.` })
      refetch()
    } catch (e) { setMsg({ ok: false, text: (e as Error).message }) } finally { setBusy(false) }
  }

  return (
    <Card title={`Review queue (${rows.length})`}>
      <p className="muted small" style={{ marginTop: 0 }}>Automatic abuse flags on discount-code usage. Clear a false positive, or suspend the code while you investigate.</p>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.6rem' }}>{msg.text}</div>}
      {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
        <Empty>No open flags — nothing to review.</Empty>
      ) : (
        <table className="data">
          <thead><tr><th>Kind</th><th>Code</th><th>Email</th><th>Detail</th><th>Created</th><th /></tr></thead>
          <tbody>
            {rows.map((f) => (
              <tr key={f.id}>
                <td><Badge tone="warn">{f.kind.replace(/_/g, ' ')}</Badge></td>
                <td className="small"><strong>{f.code || '—'}</strong></td>
                <td className="small">{f.email || '—'}</td>
                <td className="small">{f.detail || '—'}</td>
                <td className="small muted">{fmtDate(f.created_at)}</td>
                <td>
                  <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                    <button className="btn sm secondary" disabled={busy} onClick={() => act(f, 'clear')}>Clear</button>
                    <button className="btn sm danger" disabled={busy || !f.code_id} onClick={() => act(f, 'suspend_code')}>Suspend code</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </Card>
  )
}

const CODE_TABS = ['Codes', 'Approvals', 'Review queue'] as const

export default function Codes() {
  const [tab, setTab] = useState<(typeof CODE_TABS)[number]>('Codes')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <h1>Discount &amp; founding codes</h1>
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {CODE_TABS.map((t) => <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>)}
      </div>
      {tab === 'Codes' && <CodesTab />}
      {tab === 'Approvals' && <ApprovalsTab />}
      {tab === 'Review queue' && <ReviewQueueTab />}
    </div>
  )
}

function CodesTab() {
  const { data, loading, error, refetch } = useAdminQuery<DiscountCode[]>('/api/admin/codes')
  const [creating, setCreating] = useState(false)
  const [editing, setEditing] = useState<DiscountCode | null>(null)

  async function toggle(c: DiscountCode) {
    await adminApi.patch(`/api/admin/codes/${c.id}`, { active: c.active ? false : true })
    refetch()
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <span className="muted small">Codes created by PCI, plus approved institution codes.</span>
        <div className="row">
          <Link className="btn secondary sm" to="/founding">Founding dashboard</Link>
          <button className="btn sm" onClick={() => setCreating(true)}>New code</button>
        </div>
      </div>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.length === 0 ? (
          <Empty>No discount codes yet.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Code</th><th>Grants / discount</th><th>Uses</th><th>Window</th><th>Status</th><th></th></tr>
            </thead>
            <tbody>
              {data.map((c) => (
                <tr key={c.id}>
                  <td>
                    <strong>{c.code}</strong>
                    {c.code_type === 'referral' && <> <Badge tone="brand">referral</Badge></>}
                    {c.founding_route && <> <Badge tone="warn">founding{c.requires_application ? ' · application' : ''}</Badge></>}
                  </td>
                  <td className="small">
                    {c.founding_route
                      ? ['membership', 'exam', 'study'].filter((_, i) => [c.grants_membership, c.grants_exam, c.grants_study_access][i]).join(' + ') + ' at USD 0'
                      : `${c.discount_type === 'fixed' ? `$${c.discount_value}` : `${c.discount_value}%`} on ${c.applies_to === 'membership' ? 'membership' : c.applies_to === 'exam' ? 'exam fee' : 'membership + exam'}`}
                  </td>
                  <td>{c.used_count ?? 0}{c.max_uses ? ` / ${c.max_uses}` : ''}</td>
                  <td className="small muted">{c.start_date ? `${fmtDate(c.start_date)} – ` : ''}{c.end_date ? fmtDate(c.end_date) : '—'}</td>
                  <td>{c.active ? <Badge tone="ok">active</Badge> : <Badge tone="err">inactive</Badge>}</td>
                  <td>
                    <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap' }}>
                      <button className="btn ghost sm" onClick={() => setEditing(c)}>Edit</button>
                      <button className="btn ghost sm" onClick={() => toggle(c)}>{c.active ? 'Disable' : 'Enable'}</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {creating && <CreateForm onClose={() => setCreating(false)} onSaved={() => { setCreating(false); refetch() }} />}
      {editing && <EditForm code={editing} onClose={() => setEditing(null)} onSaved={() => { setEditing(null); refetch() }} />}
    </div>
  )
}
