import { useAdminQuery } from '../hooks'
import type { Overview } from '../api'
import { Card, Stat, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtMoney, fmtDateTime, titleCase } from '../../format'

export default function Dashboard() {
  const { data, loading, error } = useAdminQuery<Overview>('/api/admin/overview')
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null
  const k = data.kpis

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <h1>Dashboard</h1>

      <div className="grid cols-3">
        <Card><Stat n={k.members} k="Total students" /></Card>
        <Card><Stat n={k.activeMembers} k="Active students" /></Card>
        <Card><Stat n={k.pendingMembers} k="Pending students" /></Card>
        <Card><Stat n={fmtMoney(k.revenue)} k="Revenue (all time)" /></Card>
        <Card><Stat n={fmtMoney(k.revenueMonth)} k="Revenue (this month)" /></Card>
        <Card><Stat n={k.payCount} k="Paid transactions" /></Card>
        <Card><Stat n={k.credActive} k="Active credentials" /></Card>
        <Card><Stat n={k.examsDue} k="Exams to schedule" /></Card>
        <Card><Stat n={k.openInq} k="Open enquiries" /></Card>
      </div>

      <div className="grid cols-2">
        <Card title="Enrolment funnel">
          <table className="data">
            <tbody>
              <tr><td>Started</td><td style={{ textAlign: 'right' }}><strong>{data.funnel.started}</strong></td></tr>
              <tr><td>In progress</td><td style={{ textAlign: 'right' }}><strong>{data.funnel.in_progress}</strong></td></tr>
              <tr><td>Paid</td><td style={{ textAlign: 'right' }}><strong>{data.funnel.paid}</strong></td></tr>
              <tr><td>Abandoned (72h+)</td><td style={{ textAlign: 'right' }}><strong>{k.abandoned}</strong></td></tr>
            </tbody>
          </table>
        </Card>

        <Card title="Revenue by product">
          {data.productMix.length === 0 ? (
            <Empty>No paid transactions yet.</Empty>
          ) : (
            <table className="data">
              <thead><tr><th>Product</th><th>Count</th><th style={{ textAlign: 'right' }}>Amount</th></tr></thead>
              <tbody>
                {data.productMix.map((p) => (
                  <tr key={p.product_type}>
                    <td>{titleCase(p.product_type)}</td>
                    <td>{p.n}</td>
                    <td style={{ textAlign: 'right' }}>{fmtMoney(p.amount)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </Card>
      </div>

      <Card title="Recent activity">
        {data.recent.length === 0 ? (
          <Empty>No activity recorded yet.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>When</th><th>Action</th><th>Details</th></tr></thead>
            <tbody>
              {data.recent.map((r, i) => (
                <tr key={i}>
                  <td className="small muted">{fmtDateTime(r.ts)}</td>
                  <td>{titleCase(r.action)}</td>
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
