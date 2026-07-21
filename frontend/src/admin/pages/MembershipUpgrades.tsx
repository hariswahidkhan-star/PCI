import { useState } from 'react'
import { useAdminQuery, runMutation } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../../components/ui'
import { fmtDate } from '../../format'

// Admin review of membership grade upgrades — chiefly Fellowship (FPCI) nominations submitted from the
// student Billing page. Professional (MPCI) is self-service once a credential is held; those don't queue here.
// Backend: AdminStudents.cs (gated 'members').

interface UpgradeRow {
  id: number; user_id: number; from_grade?: string | null; to_grade: string
  statement?: string | null; status: string; admin_note?: string | null
  created_at?: string | null; decided_at?: string | null
  email?: string | null; first_name?: string | null; last_name?: string | null
}

const GRADE_LABEL: Record<string, string> = { student: 'Student', associate: 'Associate (APCI)', professional: 'Professional (MPCI)', fellow: 'Fellow (FPCI)' }

export default function MembershipUpgrades() {
  const [status, setStatus] = useState('')
  const qs = status ? `?status=${status}` : ''
  const { data, loading, error, refetch } = useAdminQuery<{ rows: UpgradeRow[]; pending: number }>(`/api/admin/membership-upgrades${qs}`)
  const [busy, setBusy] = useState<number | null>(null)

  const decide = (r: UpgradeRow, action: 'approve' | 'reject') =>
    runMutation(async () => {
      const note = action === 'reject'
        ? window.prompt('Reason for declining this application (recorded, and available to the member):') ?? undefined
        : window.prompt('Optional note to the member on approval:') ?? undefined
      if (action === 'reject' && note === undefined) return
      setBusy(r.id)
      try { await adminApi.post(`/api/admin/membership-upgrades/${r.id}/decide`, { action, note }); refetch() }
      finally { setBusy(null) }
    })

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Membership grades</h1>
        <p className="muted small" style={{ margin: 0 }}>Review Fellowship (FPCI) nominations and grade-upgrade applications.</p>
      </div>

      <Card>
        <div className="row" style={{ flexWrap: 'wrap', marginBottom: '.6rem' }}>
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All statuses</option>
            <option value="pending">Pending</option>
            <option value="approved">Approved</option>
            <option value="rejected">Declined</option>
          </select>
          {data && <span className="muted small" style={{ alignSelf: 'center' }}>{data.pending} pending</span>}
        </div>

        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No grade applications.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Member</th><th>From → To</th><th>Statement</th><th>Submitted</th><th>Status</th><th></th></tr></thead>
            <tbody>
              {data.rows.map((r) => {
                const name = [r.first_name, r.last_name].filter(Boolean).join(' ')
                return (
                  <tr key={r.id}>
                    <td><div>{name || <span className="muted">—</span>}</div><div className="muted small">{r.email} · #{r.user_id}</div></td>
                    <td className="small">{GRADE_LABEL[r.from_grade ?? ''] ?? r.from_grade ?? '—'} → <strong>{GRADE_LABEL[r.to_grade] ?? r.to_grade}</strong></td>
                    <td className="small" style={{ maxWidth: 320, whiteSpace: 'pre-wrap' }}>
                      {r.statement}
                      {r.admin_note && <div className="muted">Note: {r.admin_note}</div>}
                    </td>
                    <td className="small">{fmtDate(r.created_at)}</td>
                    <td>{r.status === 'approved' ? <StatusBadge status="approved" /> : r.status === 'rejected' ? <Badge tone="warn">Declined</Badge> : <Badge tone="brand">Pending</Badge>}</td>
                    <td>
                      {r.status === 'pending' && (
                        <div className="row" style={{ gap: '.35rem', justifyContent: 'flex-end' }}>
                          <button className="btn sm" disabled={busy === r.id} onClick={() => decide(r, 'approve')}>Approve</button>
                          <button className="btn sm ghost" disabled={busy === r.id} onClick={() => decide(r, 'reject')}>Decline</button>
                        </div>
                      )}
                    </td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
