import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Empty, ErrorNote, Spinner, StatusBadge } from '../../components/ui'
import { fmtDate } from '../../format'

interface CpdRow {
  id: number
  user_id: number
  email?: string | null
  first_name?: string | null
  last_name?: string | null
  activity_date?: string | null
  category?: string | null
  hours?: number | null
  description?: string | null
  evidence_name?: string | null
  status?: string | null
  admin_note?: string | null
}

async function downloadEvidence(row: CpdRow) {
  const response = await fetch(`/api/admin/cpd/${row.id}/evidence`, {
    headers: { Authorization: `Bearer ${adminApi.getToken() ?? ''}` },
  })
  if (!response.ok) {
    alert('Could not download the CPD evidence.')
    return
  }
  const link = document.createElement('a')
  link.href = URL.createObjectURL(await response.blob())
  link.download = row.evidence_name || `cpd-evidence-${row.id}`
  document.body.appendChild(link)
  link.click()
  link.remove()
  URL.revokeObjectURL(link.href)
}

export default function CpdReview() {
  const [status, setStatus] = useState('recorded')
  const [notes, setNotes] = useState<Record<number, string>>({})
  const [busy, setBusy] = useState<number | null>(null)
  const [message, setMessage] = useState<{ ok: boolean; text: string } | null>(null)
  const query = status ? `?status=${encodeURIComponent(status)}` : ''
  const { data, loading, error, refetch } = useAdminQuery<{ rows: CpdRow[] }>(`/api/admin/cpd${query}`)

  async function review(row: CpdRow, next: 'approved' | 'rejected' | 'recorded') {
    setBusy(row.id)
    setMessage(null)
    try {
      await adminApi.post(`/api/admin/cpd/${row.id}/review`, {
        status: next,
        admin_note: (notes[row.id] ?? '').trim() || undefined,
      })
      setMessage({ ok: true, text: `CPD entry #${row.id} marked ${next}.` })
      await refetch()
    } catch (e) {
      setMessage({ ok: false, text: e instanceof Error ? e.message : 'Could not review the CPD entry.' })
    } finally {
      setBusy(null)
    }
  }

  const rows = data?.rows ?? []
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>CPD review</h1>
        <p className="muted">
          Review continuing-professional-development activities submitted by credential holders.
          Only approved hours count toward the member’s annual target.
        </p>
      </div>

      {message && <div className={'notice' + (message.ok ? '' : ' err')} role="status">{message.text}</div>}

      <Card>
        <label style={{ maxWidth: 260 }}>
          Status
          <select aria-label="CPD status" value={status} onChange={(e) => setStatus(e.target.value)}>
            <option value="recorded">Awaiting review</option>
            <option value="approved">Approved</option>
            <option value="rejected">Rejected</option>
            <option value="">All entries</option>
          </select>
        </label>
      </Card>

      <Card>
        {loading && !data ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <Empty>No CPD entries match this status.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Member</th><th>Date</th><th>Activity</th><th>Category</th><th>Hours</th><th>Status</th><th>Review</th></tr>
            </thead>
            <tbody>
              {rows.map((row) => (
                <tr key={row.id}>
                  <td>
                    <strong>{[row.first_name, row.last_name].filter(Boolean).join(' ') || row.email || `User #${row.user_id}`}</strong>
                    {row.email && <div className="muted small">{row.email}</div>}
                  </td>
                  <td className="small">{fmtDate(row.activity_date)}</td>
                  <td className="small" style={{ maxWidth: 280 }}>
                    {row.description || '—'}
                    {row.evidence_name && (
                      <div><button className="btn ghost sm" onClick={() => downloadEvidence(row)}>Evidence: {row.evidence_name}</button></div>
                    )}
                    {row.admin_note && <div className="muted">Previous note: {row.admin_note}</div>}
                  </td>
                  <td className="small">{row.category || '—'}</td>
                  <td>{row.hours ?? 0}</td>
                  <td><StatusBadge status={row.status || 'recorded'} /></td>
                  <td style={{ minWidth: 220 }}>
                    <input
                      aria-label={`Review note for CPD ${row.id}`}
                      placeholder="Review note (optional)"
                      value={notes[row.id] ?? ''}
                      onChange={(e) => setNotes({ ...notes, [row.id]: e.target.value })}
                    />
                    <div className="row" style={{ gap: '.35rem', marginTop: '.35rem', flexWrap: 'wrap' }}>
                      {row.status !== 'approved' && <button className="btn sm" disabled={busy === row.id} onClick={() => review(row, 'approved')}>Approve</button>}
                      {row.status !== 'rejected' && <button className="btn sm danger" disabled={busy === row.id} onClick={() => review(row, 'rejected')}>Reject</button>}
                      {row.status !== 'recorded' && <button className="btn sm secondary" disabled={busy === row.id} onClick={() => review(row, 'recorded')}>Reopen</button>}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
