import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'

interface CpdRow {
  id: number
  user_id: number
  activity_date?: string | null
  category?: string | null
  hours?: number | null
  description?: string | null
  evidence_name?: string | null
  status: string
  admin_note?: string | null
  created_at?: string | null
  email?: string | null
  first_name?: string | null
  last_name?: string | null
}

export default function CpdReview() {
  const [status, setStatus] = useState('recorded')
  const [notes, setNotes] = useState<Record<number, string>>({})
  const [busy, setBusy] = useState<number | null>(null)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)
  const qs = status ? `?status=${encodeURIComponent(status)}` : ''
  const { data, loading, error, refetch } = useAdminQuery<{ rows: CpdRow[] }>(`/api/admin/cpd${qs}`)

  async function review(row: CpdRow, next: 'approved' | 'rejected' | 'recorded') {
    setBusy(row.id); setMsg(null)
    try {
      await adminApi.post(`/api/admin/cpd/${row.id}/review`, { status: next, admin_note: notes[row.id] ?? row.admin_note ?? '' })
      setMsg({ ok: true, text: `CPD entry ${next}.` })
      refetch()
    } catch (e) {
      setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not review CPD entry.' })
    } finally {
      setBusy(null)
    }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <div>
          <h1>CPD review</h1>
          <p className="muted">Review member CPD submissions and approve hours for renewal eligibility.</p>
        </div>
        <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }} aria-label="CPD status filter">
          <option value="recorded">Awaiting review</option>
          <option value="approved">Approved</option>
          <option value="rejected">Rejected</option>
          <option value="">All statuses</option>
        </select>
      </div>

      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status">{msg.text}</div>}

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No CPD entries match this filter.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Member</th><th>Activity</th><th>Hours</th><th>Status</th><th>Review note</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => {
                const name = [r.first_name, r.last_name].filter(Boolean).join(' ') || r.email || `#${r.user_id}`
                return (
                  <tr key={r.id}>
                    <td>
                      <strong>{name}</strong>
                      <div className="muted small">{r.email} · #{r.user_id}</div>
                    </td>
                    <td>
                      <div className="small"><strong>{r.category || 'CPD activity'}</strong> · {fmtDate(r.activity_date)}</div>
                      <div className="muted small">{r.description || '—'}</div>
                      {r.evidence_name && <div className="small">Evidence: {r.evidence_name}</div>}
                    </td>
                    <td>{Number(r.hours ?? 0).toFixed(1)}</td>
                    <td><StatusBadge status={r.status} /></td>
                    <td>
                      <input
                        value={notes[r.id] ?? r.admin_note ?? ''}
                        onChange={(e) => setNotes((p) => ({ ...p, [r.id]: e.target.value }))}
                        placeholder="Optional note"
                        style={{ minWidth: 180 }}
                      />
                    </td>
                    <td>
                      <div className="row" style={{ justifyContent: 'flex-end', gap: '.35rem', flexWrap: 'wrap' }}>
                        <button className="btn ghost sm" disabled={busy === r.id} onClick={() => review(r, 'approved')}>Approve</button>
                        <button className="btn ghost sm danger" disabled={busy === r.id} onClick={() => review(r, 'rejected')}>Reject</button>
                        {r.status !== 'recorded' && <button className="btn ghost sm" disabled={busy === r.id} onClick={() => review(r, 'recorded')}>Reopen</button>}
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
