import { useMemo, useState } from 'react'
import { useAdminQuery } from '../hooks'
import type { AuditRow } from '../api'
import { Card, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDateTime, titleCase } from '../../format'

export default function Audit() {
  const { data, loading, error } = useAdminQuery<{ rows: AuditRow[] }>('/api/admin/audit')
  const [q, setQ] = useState('')

  const rows = useMemo(() => {
    const all = data?.rows ?? []
    const needle = q.trim().toLowerCase()
    return needle ? all.filter((r) => (r.action ?? '').toLowerCase().includes(needle) || (r.details ?? '').toLowerCase().includes(needle)) : all
  }, [data, q])

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Audit log</h1>
        <p className="muted">The 300 most recent recorded actions.</p>
      </div>

      <Card>
        <input placeholder="Filter by action or detail…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 320 }} />
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <Empty>No matching audit entries.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>When</th><th>Who</th><th>Action</th><th>Details</th></tr>
            </thead>
            <tbody>
              {rows.map((r) => (
                <tr key={r.id}>
                  <td className="small muted" style={{ whiteSpace: 'nowrap' }}>{fmtDateTime(r.created_at)}</td>
                  <td className="small">{r.actor || (r.user_id ? `#${r.user_id}` : '—')}</td>
                  <td>{titleCase(r.action ?? '')}</td>
                  <td className="small">{r.details}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
