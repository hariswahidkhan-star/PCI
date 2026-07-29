import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type EnrollmentRow } from '../api'
import { Card, StatusBadge, ErrorNote } from '../../components/ui'
import { PageHeader, Toolbar, SearchInput, FilterSelect, EmptyState, SkeletonTable } from '../../components/premium'
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
    <div className="page">
      <PageHeader
        eyebrow="Students"
        title="Enrolments"
        subtitle="Every enrolment session, including the ones that stalled before payment."
      />

      {/* Search and status are SERVER-side here (the endpoint owns ?q= and ?status=), so the
          toolbar drives the query rather than filtering a local copy. */}
      <Toolbar count={data ? `${data.total} total` : undefined}>
        <SearchInput value={q} onChange={setQ} label="Search" placeholder="Search email…" />
        <FilterSelect
          label="Status"
          value={status}
          onChange={setStatus}
          allLabel="All statuses"
          options={[
            { value: 'in_progress', label: 'In progress' },
            { value: 'paid', label: 'Paid' },
            { value: 'abandoned', label: 'Abandoned' },
          ]}
        />
      </Toolbar>

      <Card className="flat">
        {loading ? (
          <SkeletonTable rows={6} cols={6} />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <EmptyState
            icon="clipboard"
            title={q || status ? 'No enrolment matches' : 'No enrolments yet'}
            detail={
              q || status
                ? 'No enrolment session matches the current search and status.'
                : 'Sessions appear here as soon as a candidate begins the enrolment flow.'
            }
            action={
              q || status ? (
                <button className="btn secondary sm" onClick={() => { setQ(''); setStatus('') }}>Clear filters</button>
              ) : undefined
            }
          />
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
