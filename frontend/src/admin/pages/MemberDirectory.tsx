import { useAdminQuery, runMutation } from '../hooks'
import { adminApi } from '../api'
import { Card, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'

// Admin moderation of the opt-in public member directory. Members list themselves from their profile;
// admins can remove a listing. Backend: MemberDirectory.cs (gated 'members').

interface Listing {
  id: number
  name?: string
  headline?: string | null
  country?: string | null
  linkedin?: string | null
  member_since?: string | null
  credentials?: { credential_id: string; acronym: string; name: string }[]
}

export default function MemberDirectory() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Listing[] }>('/api/admin/directory')

  const unlist = (m: Listing) =>
    runMutation(async () => {
      if (!window.confirm(`Remove ${m.name} from the public directory?`)) return
      await adminApi.post(`/api/admin/directory/${m.id}/unlist`, {}); refetch()
    })

  return (
    <div className="page">
      <PageHeader
        title="Member directory"
        subtitle="Members who opted into the public find-a-professional directory (they also hold an active credential)."
      />
      <Card>
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No members are currently listed.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Member</th><th>Country</th><th>Credentials</th><th></th></tr></thead>
            <tbody>
              {data.rows.map((m) => (
                <tr key={m.id}>
                  <td><div>{m.name}</div>{m.headline && <div className="muted small">{m.headline}</div>}</td>
                  <td className="small">{m.country ?? '—'}</td>
                  <td className="small">{(m.credentials ?? []).map((c) => c.acronym).join(', ')}</td>
                  <td><button className="btn sm ghost danger" onClick={() => unlist(m)}>Remove listing</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
