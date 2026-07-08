import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import type { EmailLog } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDateTime, titleCase } from '../../format'

export default function Emails() {
  const [status, setStatus] = useState('')
  const [q, setQ] = useState('')
  const params = new URLSearchParams()
  if (status) params.set('status', status)
  if (q) params.set('q', q)
  const qs = params.toString()
  const { data, loading, error } = useAdminQuery<{ rows: EmailLog[] }>(`/api/admin/emails${qs ? '?' + qs : ''}`)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Email log</h1>
        <p className="muted">A record of every email the platform has sent (or attempted).</p>
      </div>

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search recipient…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 280 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All statuses</option>
            <option value="sent">Sent</option>
            <option value="queued">Queued</option>
            <option value="failed">Failed</option>
          </select>
        </div>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No emails logged yet.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Sent</th><th>Recipient</th><th>Type</th><th>Subject</th><th>Status</th></tr>
            </thead>
            <tbody>
              {data.rows.map((e) => (
                <tr key={e.id}>
                  <td className="small muted" style={{ whiteSpace: 'nowrap' }}>{fmtDateTime(e.sent_at)}</td>
                  <td className="small">{e.email}</td>
                  <td className="small">{titleCase(e.email_type ?? '')}</td>
                  <td className="small">{e.subject}</td>
                  <td><StatusBadge status={e.status || 'sent'} /></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
