import { useState } from 'react'
import { useAdminQuery, runMutation } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../../components/ui'
import { fmtDate } from '../../format'

// Admin review of the casework students submit from their portal (Appeals & Accommodations page):
//   • Appeals — exam result, invalidated attempt, complaint, ethics → under_review / upheld / dismissed.
//   • Accommodations — extra time, separate setting, assistive tech, other → under_review / approved / rejected.
// Backend endpoints live in Casework.cs (gated 'tickets').

interface AppealRow {
  id: number; user_id: number; attempt_id?: number | null; credential_id?: string | null
  type: string; reason?: string | null; evidence_name?: string | null; status: string
  submitted_at?: string | null; decision?: string | null; decided_at?: string | null
  email?: string | null; first_name?: string | null; last_name?: string | null
}
interface AccomRow {
  id: number; user_id: number; request_type: string; description?: string | null; evidence_name?: string | null
  status: string; approved_extra_minutes?: number | null; admin_note?: string | null
  created_at?: string | null; decided_at?: string | null
  email?: string | null; first_name?: string | null; last_name?: string | null
}

const APPEAL_LABEL: Record<string, string> = {
  result_appeal: 'Exam result appeal', invalidation_appeal: 'Invalidated-attempt appeal', complaint: 'Complaint', ethics: 'Ethics concern',
}
const ACCOM_LABEL: Record<string, string> = {
  extra_time: 'Additional time', separate_setting: 'Separate / quiet setting', assistive_technology: 'Assistive technology', other: 'Other',
}

// Authenticated evidence download (mirrors the admin document blob flow).
async function downloadEvidence(kind: 'appeals' | 'accommodations', id: number, filename?: string | null) {
  const tok = adminApi.getToken() ?? ''
  const res = await fetch(`/api/admin/${kind}/${id}/evidence`, { headers: { Authorization: 'Bearer ' + tok } })
  if (!res.ok) { alert('Could not open the evidence file.'); return }
  const a = document.createElement('a')
  a.href = URL.createObjectURL(await res.blob())
  a.download = filename || `${kind}-evidence-${id}`
  document.body.appendChild(a); a.click(); a.remove(); URL.revokeObjectURL(a.href)
}

function name(r: { first_name?: string | null; last_name?: string | null; email?: string | null; user_id: number }) {
  return [r.first_name, r.last_name].filter(Boolean).join(' ') || r.email || `user ${r.user_id}`
}

function Appeals() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: AppealRow[] }>('/api/admin/appeals')
  const decide = (id: number, status: string, decision?: string) =>
    runMutation(async () => { await adminApi.post(`/api/admin/appeals/${id}/decide`, { status, decision }); refetch() })

  const uphold = (r: AppealRow) => { const d = window.prompt('Decision note sent to the candidate (upholding the appeal):', ''); if (d == null) return; decide(r.id, 'upheld', d) }
  const dismiss = (r: AppealRow) => { const d = window.prompt('Decision note sent to the candidate (dismissing the appeal):', ''); if (d == null) return; decide(r.id, 'dismissed', d) }

  return (
    <Card title="Appeals & complaints">
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
        <Empty>No appeals submitted.</Empty>
      ) : (
        <table className="data">
          <thead><tr><th>Candidate</th><th>Type</th><th>Grounds</th><th>Submitted</th><th>Status</th><th></th></tr></thead>
          <tbody>
            {data.rows.map((r) => {
              const open = r.status === 'submitted' || r.status === 'under_review'
              return (
                <tr key={r.id}>
                  <td><div>{name(r)}</div><div className="muted small">{r.email} · #{r.user_id}{r.attempt_id ? ` · attempt ${r.attempt_id}` : ''}{r.credential_id ? ` · ${r.credential_id}` : ''}</div></td>
                  <td className="small">{APPEAL_LABEL[r.type] ?? r.type}</td>
                  <td className="small" style={{ maxWidth: 300 }}>
                    <div>{r.reason}</div>
                    {r.evidence_name && <button className="btn ghost sm" onClick={() => downloadEvidence('appeals', r.id, r.evidence_name)}>Evidence: {r.evidence_name}</button>}
                    {r.decision && <div className="muted">Decision: {r.decision}</div>}
                  </td>
                  <td className="small">{fmtDate(r.submitted_at)}</td>
                  <td>{r.status === 'upheld' ? <Badge tone="ok">Upheld</Badge> : r.status === 'dismissed' ? <Badge tone="warn">Dismissed</Badge> : r.status === 'under_review' ? <Badge tone="brand">In review</Badge> : <Badge tone="warn">New</Badge>}</td>
                  <td>
                    <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                      {open && r.status === 'submitted' && <button className="btn sm secondary" onClick={() => decide(r.id, 'under_review')}>Start review</button>}
                      {open && <button className="btn sm" onClick={() => uphold(r)}>Uphold</button>}
                      {open && <button className="btn sm ghost" onClick={() => dismiss(r)}>Dismiss</button>}
                    </div>
                  </td>
                </tr>
              )
            })}
          </tbody>
        </table>
      )}
    </Card>
  )
}

function Accommodations() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: AccomRow[] }>('/api/admin/accommodations')
  const decide = (id: number, status: string, body?: Record<string, unknown>) =>
    runMutation(async () => { await adminApi.post(`/api/admin/accommodations/${id}/decide`, { status, ...body }); refetch() })

  const approve = (r: AccomRow) => {
    const mins = window.prompt('Approved extra time in minutes (0–120):', r.request_type === 'extra_time' ? '30' : '0')
    if (mins == null) return
    const n = Math.max(0, Math.min(120, parseInt(mins, 10) || 0))
    const note = window.prompt('Note to the candidate (optional):', '') ?? ''
    decide(r.id, 'approved', { approved_extra_minutes: n, admin_note: note })
  }
  const reject = (r: AccomRow) => { const note = window.prompt('Reason for declining (sent to the candidate):', ''); if (note == null) return; decide(r.id, 'rejected', { admin_note: note }) }

  return (
    <Card title="Exam accommodation requests">
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
        <Empty>No accommodation requests.</Empty>
      ) : (
        <table className="data">
          <thead><tr><th>Candidate</th><th>Request</th><th>Details</th><th>Submitted</th><th>Status</th><th></th></tr></thead>
          <tbody>
            {data.rows.map((r) => {
              const open = r.status === 'submitted' || r.status === 'under_review'
              return (
                <tr key={r.id}>
                  <td><div>{name(r)}</div><div className="muted small">{r.email} · #{r.user_id}</div></td>
                  <td className="small">{ACCOM_LABEL[r.request_type] ?? r.request_type}</td>
                  <td className="small" style={{ maxWidth: 300 }}>
                    <div>{r.description}</div>
                    {r.evidence_name && <button className="btn ghost sm" onClick={() => downloadEvidence('accommodations', r.id, r.evidence_name)}>Evidence: {r.evidence_name}</button>}
                    {r.admin_note && <div className="muted">Note: {r.admin_note}</div>}
                    {!!r.approved_extra_minutes && <div className="muted">+{r.approved_extra_minutes} min approved</div>}
                  </td>
                  <td className="small">{fmtDate(r.created_at)}</td>
                  <td>{r.status === 'approved' ? <StatusBadge status="approved" /> : r.status === 'rejected' ? <Badge tone="warn">Declined</Badge> : r.status === 'under_review' ? <Badge tone="brand">In review</Badge> : <Badge tone="warn">New</Badge>}</td>
                  <td>
                    <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                      {open && r.status === 'submitted' && <button className="btn sm secondary" onClick={() => decide(r.id, 'under_review')}>Start review</button>}
                      {open && <button className="btn sm" onClick={() => approve(r)}>Approve</button>}
                      {open && <button className="btn sm ghost" onClick={() => reject(r)}>Decline</button>}
                    </div>
                  </td>
                </tr>
              )
            })}
          </tbody>
        </table>
      )}
    </Card>
  )
}

export default function Casework() {
  const [tab, setTab] = useState<'appeals' | 'accommodations'>('appeals')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Appeals &amp; Accommodations</h1>
        <p className="muted small" style={{ margin: 0 }}>Review the appeals, complaints and exam-accommodation requests candidates submit from their portal.</p>
      </div>
      <div className="row" style={{ gap: '.4rem' }}>
        <button className={'btn sm' + (tab === 'appeals' ? '' : ' secondary')} onClick={() => setTab('appeals')}>Appeals</button>
        <button className={'btn sm' + (tab === 'accommodations' ? '' : ' secondary')} onClick={() => setTab('accommodations')}>Accommodations</button>
      </div>
      {tab === 'appeals' ? <Appeals /> : <Accommodations />}
    </div>
  )
}
