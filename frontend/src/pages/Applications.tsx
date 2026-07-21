import { useQuery } from '../api/hooks'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate } from '../format'

// Member "My applications" — the roles this signed-in member has applied for through the public
// careers board (applications are linked to the account at submit time). Backend: GET /api/me/applications.
// Only candidate-safe fields are returned; internal recruiter notes are never exposed here.

interface AppRow {
  id: number
  reference?: string | null
  status: string
  created_at?: string | null
  cv_name?: string | null
  title: string
  job_code?: string | null
  organisation?: string | null
  location?: string | null
  country?: string | null
}

const STATUS_LABEL: Record<string, string> = {
  new: 'Submitted', reviewing: 'Under review', shortlisted: 'Shortlisted', rejected: 'Not selected', hired: 'Hired',
}
const STATUS_TONE: Record<string, 'neutral' | 'brand' | 'ok' | 'warn'> = {
  new: 'brand', reviewing: 'brand', shortlisted: 'ok', rejected: 'neutral', hired: 'ok',
}

export default function Applications() {
  const { data, loading, error } = useQuery<{ rows: AppRow[] }>('/api/me/applications')
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>My applications</h1>
        <p className="muted">Roles you have applied for through the PCI careers board. <a href="/careers.html">Browse open positions →</a></p>
      </div>
      <Card>
        {rows.length === 0 ? (
          <Empty>You haven’t applied for any roles yet. <a href="/careers.html">See open positions →</a></Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Role</th><th>Reference</th><th>Applied</th><th>Status</th></tr></thead>
            <tbody>
              {rows.map((a) => (
                <tr key={a.id}>
                  <td>
                    <div style={{ fontWeight: 600 }}>{a.title}</div>
                    <div className="muted small">
                      {[a.organisation, [a.location, a.country].filter(Boolean).join(', ')].filter(Boolean).join(' · ')}
                      {a.job_code ? ` · ${a.job_code}` : ''}
                    </div>
                  </td>
                  <td className="small mono">{a.reference || '—'}</td>
                  <td className="small">{fmtDate(a.created_at)}</td>
                  <td><Badge tone={STATUS_TONE[a.status] ?? 'neutral'}>{STATUS_LABEL[a.status] ?? a.status}</Badge></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
