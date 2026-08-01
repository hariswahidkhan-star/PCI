import { useEffect, useState } from 'react'
import { useAdminQuery } from '../hooks'
import type { EmailLog } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtDateTime, titleCase } from '../../format'

export default function Emails() {
  const [status, setStatus] = useState('')
  const [q, setQ] = useState('')
  const [dq, setDq] = useState('')
  // Debounce the search so the list doesn't refetch (and flash a spinner) on every keystroke.
  useEffect(() => {
    const t = setTimeout(() => setDq(q), 300)
    return () => clearTimeout(t)
  }, [q])
  const params = new URLSearchParams()
  if (status) params.set('status', status)
  if (dq) params.set('q', dq)
  const qs = params.toString()
  const { data, loading, error } = useAdminQuery<{ rows: EmailLog[] }>(`/api/admin/emails${qs ? '?' + qs : ''}`)

  return (
    <div className="page">
      <PageHeader
        title="Email log"
        subtitle="A record of every email the platform has sent (or attempted)."
      />

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search recipient…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 280 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All statuses</option>
            <option value="sent">Sent</option>
            <option value="console">Console (not delivered)</option>
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
