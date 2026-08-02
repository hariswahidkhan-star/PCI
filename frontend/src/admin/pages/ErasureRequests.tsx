import { useState } from 'react'
import { useAdminQuery, runMutation } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtDate } from '../../format'

// GDPR right-to-erasure queue. A student's "delete my data" request lands here with a 30-day due date.
// An admin acknowledges, then either completes (anonymises the member) or rejects with a documented reason.

interface ErasureRow {
  id: number
  user_id: number
  email?: string | null
  reason?: string | null
  status: string
  requested_at?: string | null
  due_at?: string | null
  acknowledged_at?: string | null
  reviewed_at?: string | null
  admin_note?: string | null
  completed_at?: string | null
  first_name?: string | null
  last_name?: string | null
  user_status?: string | null
  overdue?: boolean
}

export default function ErasureRequests() {
  const [status, setStatus] = useState('')
  const qs = status ? `?status=${status}` : ''
  const { data, loading, error, refetch } = useAdminQuery<{ rows: ErasureRow[]; open: number }>(`/api/admin/erasure-requests${qs}`)
  const [busy, setBusy] = useState<number | null>(null)

  const act = (id: number, path: string, body?: Record<string, unknown>) =>
    runMutation(async () => {
      setBusy(id)
      try { await adminApi.post(`/api/admin/erasure-requests/${id}/${path}`, body ?? {}); refetch() }
      finally { setBusy(null) }
    })

  const acknowledge = (r: ErasureRow) => act(r.id, 'acknowledge')
  const reject = (r: ErasureRow) => {
    const note = window.prompt('Reason for declining this erasure request (e.g. a legal retention basis). This is recorded.')
    if (note == null) return
    if (note.trim().length < 5) { alert('Please give a short reason (at least 5 characters).'); return }
    act(r.id, 'reject', { note: note.trim() })
  }
  const complete = (r: ErasureRow) => {
    const name = [r.first_name, r.last_name].filter(Boolean).join(' ') || r.email || `user ${r.user_id}`
    if (!window.confirm(`Permanently anonymise ALL personal data for ${name}?\n\nThis deletes their profile, identity documents, learning and casework history, and locks the account. Payments and issued credentials are retained but de-identified. This cannot be undone.`)) return
    act(r.id, 'complete', { confirm: true })
  }

  const badge = (r: ErasureRow) =>
    r.status === 'completed' ? <StatusBadge status="completed" />
      : r.status === 'rejected' ? <Badge tone="warn">Declined</Badge>
        : r.overdue ? <Badge tone="err">Overdue</Badge>
          : r.status === 'acknowledged' ? <Badge tone="brand">In review</Badge>
            : <Badge tone="warn">Pending</Badge>

  return (
    <div className="page">
      <PageHeader
        title="Data erasure requests"
        subtitle="GDPR right-to-erasure. Each request has a 30-day fulfilment window. Completing a request anonymises the member's personal data; records the institute must retain (payments, issued credentials) are kept in de-identified form."
      />

      <Card>
        <div className="row" style={{ flexWrap: 'wrap', marginBottom: '.6rem' }}>
          <select aria-label="Filter by status" value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }}>
            <option value="">All statuses</option>
            <option value="pending">Pending</option>
            <option value="acknowledged">In review</option>
            <option value="completed">Completed</option>
            <option value="rejected">Declined</option>
          </select>
          {data && <span className="muted small" style={{ alignSelf: 'center' }}>{data.open} open</span>}
        </div>

        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No erasure requests.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Member</th><th>Requested</th><th>Due</th><th>Status</th><th>Reason / note</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => {
                const name = [r.first_name, r.last_name].filter(Boolean).join(' ')
                const open = r.status === 'pending' || r.status === 'acknowledged'
                return (
                  <tr key={r.id}>
                    <td>
                      <div>{name || <span className="muted">—</span>}</div>
                      <div className="muted small">{r.email} · #{r.user_id}</div>
                    </td>
                    <td className="small">{fmtDate(r.requested_at)}</td>
                    <td className="small">{r.due_at ? fmtDate(r.due_at) : '—'}</td>
                    <td>{badge(r)}</td>
                    <td className="small" style={{ maxWidth: 280 }}>
                      {r.reason && <div>{r.reason}</div>}
                      {r.admin_note && <div className="muted">Note: {r.admin_note}</div>}
                    </td>
                    <td>
                      <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                        {r.status === 'pending' && <button className="btn sm secondary" disabled={busy === r.id} onClick={() => acknowledge(r)}>Acknowledge</button>}
                        {open && <button className="btn sm danger" disabled={busy === r.id} onClick={() => complete(r)}>{busy === r.id ? 'Working…' : 'Anonymise'}</button>}
                        {open && <button className="btn sm ghost" disabled={busy === r.id} onClick={() => reject(r)}>Decline</button>}
                      </div>
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
