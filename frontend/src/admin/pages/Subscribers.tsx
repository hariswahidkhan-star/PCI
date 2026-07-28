import { useAdminQuery, runMutation } from '../hooks'
import { adminApi, type Subscriber } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtDate } from '../../format'

export default function Subscribers() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Subscriber[] }>('/api/admin/subscribers')

  const setStatus = (id: number, status: string) =>
    runMutation(async () => { await adminApi.patch(`/api/admin/subscribers/${id}`, { status }); refetch() })

  return (
    <div className="page">
      <PageHeader
        title="Newsletter subscribers"
        actions={data && <span className="muted small">{data.rows.length} total</span>}
      />

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No subscribers yet.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Email</th><th>Subscribed</th><th>Status</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((s) => {
                const active = (s.status ?? 'subscribed') !== 'unsubscribed'
                return (
                  <tr key={s.id}>
                    <td>{s.email}</td>
                    <td className="small muted">{fmtDate(s.created_at)}</td>
                    <td><StatusBadge status={active ? 'active' : 'unsubscribed'} /></td>
                    <td>
                      <button className="btn ghost sm" onClick={() => setStatus(s.id, active ? 'unsubscribed' : 'subscribed')}>
                        {active ? 'Unsubscribe' : 'Resubscribe'}
                      </button>
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
