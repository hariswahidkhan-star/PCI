import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type EnrollmentRow } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDateTime, titleCase } from '../../format'

export default function Enrollments() {
  const [status, setStatus] = useState('')
  const [q, setQ] = useState('')
  const [reminding, setReminding] = useState<number | null>(null)
  const params = new URLSearchParams()
  if (status) params.set('status', status)
  if (q) params.set('q', q)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: EnrollmentRow[]; total: number }>(`/api/admin/enrollments${qs ? '?' + qs : ''}`)

  async function remind(id: number) {
    setReminding(id)
    try {
      await adminApi.post(`/api/admin/enrollments/${id}/remind`)
      refetch()
    } finally {
      setReminding(null)
    }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Enrolments</h1>
        {data && <span className="muted small">{data.total} total</span>}
      </div>

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search email…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 280 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }}>
            <option value="">All statuses</option>
            <option value="in_progress">In progress</option>
            <option value="paid">Paid</option>
            <option value="abandoned">Abandoned</option>
          </select>
        </div>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No enrolments match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Email</th><th>Product</th><th>Step</th><th>Status</th><th>Reminders</th><th>Last activity</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((e) => (
                <tr key={e.id}>
                  <td className="small">{e.email}</td>
                  <td>{titleCase(e.selected_product ?? '—')}</td>
                  <td className="small muted">{e.current_step || '—'}</td>
                  <td><StatusBadge status={e.session_status} /></td>
                  <td>{e.reminders_sent ?? 0}</td>
                  <td className="small muted">{fmtDateTime(e.last_activity_at)}</td>
                  <td>
                    {e.session_status === 'in_progress' && (
                      <button className="btn ghost sm" disabled={reminding === e.id} onClick={() => remind(e.id)}>{reminding === e.id ? 'Sending…' : 'Remind'}</button>
                    )}
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
