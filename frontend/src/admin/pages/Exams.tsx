import { useAdminQuery } from '../hooks'
import type { ExamReg } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate, titleCase } from '../../format'

export default function Exams() {
  const { data, loading, error } = useAdminQuery<{ rows: ExamReg[] }>('/api/admin/exams')

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Exam registrations</h1>
        <p className="muted">Everyone who has paid for an exam, ordered by their scheduling deadline.</p>
      </div>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No paid exam registrations yet.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Candidate</th><th>Product</th><th>Reference</th><th>Paid</th><th>Deadline</th><th>Time left</th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => {
                const d = r.days_left
                const tone = d == null ? 'neutral' : d < 0 ? 'err' : d <= 14 ? 'warn' : 'ok'
                return (
                  <tr key={r.id}>
                    <td><strong>{`${r.first_name ?? ''} ${r.last_name ?? ''}`.trim() || '—'}</strong><div className="small muted">{r.email}</div></td>
                    <td>{titleCase(r.product_type)}</td>
                    <td className="small muted">{r.reference || '—'}</td>
                    <td className="small">{fmtDate(r.payment_date)}</td>
                    <td className="small">{fmtDate(r.exam_schedule_deadline)}</td>
                    <td>{d == null ? '—' : <Badge tone={tone as 'ok' | 'warn' | 'err' | 'neutral'}>{d < 0 ? `${-d}d overdue` : `${d}d left`}</Badge>}</td>
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
