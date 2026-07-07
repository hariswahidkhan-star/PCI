import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type FormSubmission } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate, titleCase } from '../../format'

export default function Submissions() {
  const [status, setStatus] = useState('')
  const [open, setOpen] = useState<FormSubmission | null>(null)
  const { data, loading, error, refetch } = useAdminQuery<{ rows: FormSubmission[] }>(`/api/admin/form_submissions${status ? '?status=' + status : ''}`)

  async function setSubStatus(id: number, s: string) {
    await adminApi.post(`/api/admin/form_submissions/${id}/status`, { status: s })
    refetch()
    setOpen(null)
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <h1>Form submissions</h1>

      <Card>
        <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }}>
          <option value="">All statuses</option>
          <option value="new">New</option>
          <option value="in_progress">In progress</option>
          <option value="closed">Closed</option>
        </select>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No submissions match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Date</th><th>Form</th><th>From</th><th>Subject</th><th>Status</th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => (
                <tr key={r.id} style={{ cursor: 'pointer' }} onClick={() => setOpen(r)}>
                  <td className="small muted">{fmtDate(r.created_at)}</td>
                  <td>{titleCase(r.form_type ?? '—')}</td>
                  <td className="small">{r.name || r.email}</td>
                  <td className="small">{r.subject || '—'}</td>
                  <td><StatusBadge status={r.status || 'new'} /></td>
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
              <h2 style={{ margin: 0 }}>Submission</h2>
              <button className="btn secondary sm" onClick={() => setOpen(null)}>Close</button>
            </div>
            <Card title={open.subject || titleCase(open.form_type ?? 'Submission')} action={<StatusBadge status={open.status || 'new'} />}>
              <div className="small muted">{open.name} · {open.email} · {open.reference}</div>
              {open.message && <p style={{ marginTop: '.75rem', whiteSpace: 'pre-wrap' }}>{open.message}</p>}
              <div className="row" style={{ marginTop: '.75rem', flexWrap: 'wrap' }}>
                <span className="muted small">Set status:</span>
                {['new', 'in_progress', 'closed'].map((s) => (
                  <button key={s} className="btn sm secondary" disabled={(open.status || 'new') === s} onClick={() => setSubStatus(open.id, s)}>{s.replace(/_/g, ' ')}</button>
                ))}
              </div>
            </Card>
          </div>
        </div>
      )}
    </div>
  )
}
