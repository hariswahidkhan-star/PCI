import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type Inquiry } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate, titleCase } from '../../format'

export default function Enquiries() {
  const [status, setStatus] = useState('')
  const [q, setQ] = useState('')
  const [open, setOpen] = useState<Inquiry | null>(null)
  const params = new URLSearchParams()
  if (status) params.set('status', status)
  if (q) params.set('q', q)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Inquiry[] }>(`/api/admin/inquiries${qs ? '?' + qs : ''}`)

  async function setInqStatus(id: number, s: string) {
    await adminApi.post(`/api/admin/inquiries/${id}/status`, { status: s })
    refetch()
    setOpen(null)
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <h1>Enquiries</h1>

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search email, org, topic…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 300 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All statuses</option>
            <option value="new">New</option>
            <option value="in_progress">In progress</option>
            <option value="closed">Closed</option>
          </select>
        </div>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No enquiries match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Date</th><th>Type</th><th>From</th><th>Topic</th><th>Status</th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => (
                <tr key={r.id} style={{ cursor: 'pointer' }} onClick={() => setOpen(r)}>
                  <td className="small muted">{fmtDate(r.created_at)}</td>
                  <td>{titleCase(r.type ?? '—')}</td>
                  <td className="small">{r.email}{r.org ? ` · ${r.org}` : ''}</td>
                  <td className="small">{r.topic || '—'}</td>
                  <td><StatusBadge status={r.status} /></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {open && (
        <div className="drawer-backdrop" onClick={() => setOpen(null)}>
          <div className="drawer" onClick={(e) => e.stopPropagation()}>
            <div className="spread" style={{ marginBottom: '1rem' }}>
              <h2 style={{ margin: 0 }}>Enquiry</h2>
              <button className="btn secondary sm" onClick={() => setOpen(null)}>Close</button>
            </div>
            <Card title={open.topic || titleCase(open.type ?? 'Enquiry')} action={<StatusBadge status={open.status} />}>
              <div className="small muted stack">
                <div>{open.first_name} · {open.email}</div>
                {open.org && <div>Organisation: {open.org}</div>}
                {open.seats && <div>Seats: {open.seats}</div>}
                <div>Reference: {open.reference}</div>
              </div>
              {open.message && <p style={{ marginTop: '.75rem', whiteSpace: 'pre-wrap' }}>{open.message}</p>}
              <div className="row" style={{ marginTop: '.75rem', flexWrap: 'wrap' }}>
                <span className="muted small">Set status:</span>
                {['new', 'in_progress', 'closed'].map((s) => (
                  <button key={s} className="btn sm secondary" disabled={open.status === s} onClick={() => setInqStatus(open.id, s)}>{s.replace(/_/g, ' ')}</button>
                ))}
              </div>
            </Card>
          </div>
        </div>
      )}
    </div>
  )
}
