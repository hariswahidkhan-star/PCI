import { useMemo, useState } from 'react'
import { useAdminQuery } from '../hooks'
import type { ExamReg } from '../api'
import { Card, Badge, ErrorNote } from '../../components/ui'
import {
  PageHeader,
  Toolbar,
  SearchInput,
  FilterSelect,
  SortHeader,
  EmptyState,
  SkeletonTable,
  Pagination,
} from '../../components/premium'
import { useTableControls } from '../../components/useTableControls'
import { fmtDate, titleCase } from '../../format'

/* Everyone who has paid for an exam but not yet sat it, ordered by scheduling deadline.

   This list is worked by deadline: the operator's real question is "who is about to run out
   of time?", so alongside free-text search there is an urgency filter derived from days_left
   (the same thresholds that colour the badge), and every column sorts. Filtering is
   client-side — the endpoint returns the whole set and it is operator-scale. */

type Urgency = 'overdue' | 'soon' | 'ok'

/** Bucket a registration by how much of its scheduling window is left. */
function urgencyOf(days: number | null | undefined): Urgency | null {
  if (days == null) return null
  if (days < 0) return 'overdue'
  if (days <= 14) return 'soon'
  return 'ok'
}

const URGENCY_TONE: Record<Urgency, 'err' | 'warn' | 'ok'> = { overdue: 'err', soon: 'warn', ok: 'ok' }

export default function Exams() {
  const { data, loading, error } = useAdminQuery<{ rows: ExamReg[] }>('/api/admin/exams')
  const [urgency, setUrgency] = useState('')

  // A searchable/sortable projection: the candidate's name is two columns in the payload but
  // one thing an operator types, and urgency has to exist as a value to filter and sort on.
  const rows = useMemo(
    () =>
      (data?.rows ?? []).map((r) => ({
        ...r,
        candidate: `${r.first_name ?? ''} ${r.last_name ?? ''}`.trim(),
        urgency: urgencyOf(r.days_left) ?? '',
      })),
    [data],
  )

  const t = useTableControls(rows, {
    searchKeys: ['candidate', 'email', 'reference', 'product_type'],
    filters: [urgency ? (r) => r.urgency === urgency : null],
    filterKey: urgency,
    pageSize: 25,
  })

  const total = rows.length
  const overdue = rows.filter((r) => r.urgency === 'overdue').length

  return (
    <div className="page">
      <PageHeader
        eyebrow="Examinations"
        title="Exam registrations"
        subtitle="Everyone who has paid for an exam, ordered by their scheduling deadline."
        actions={overdue > 0 ? <span className="chip err">{overdue} past deadline</span> : undefined}
      />

      {total > 0 && (
        <Toolbar count={t.total !== total ? `${t.total} of ${total} shown` : `${total} registrations`}>
          <SearchInput
            value={t.query}
            onChange={t.setQuery}
            label="Search"
            placeholder="Search candidate, email or reference…"
          />
          <FilterSelect
            label="Time left"
            value={urgency}
            onChange={setUrgency}
            options={[
              { value: 'overdue', label: 'Past deadline' },
              { value: 'soon', label: 'Due within 14 days' },
              { value: 'ok', label: 'On track' },
            ]}
          />
        </Toolbar>
      )}

      <Card className="flat">
        {loading ? (
          <SkeletonTable rows={6} cols={6} />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : total === 0 ? (
          <EmptyState
            icon="award"
            title="No paid exam registrations yet"
            detail="A candidate appears here as soon as their exam fee settles, with the deadline by which they must schedule."
          />
        ) : t.total === 0 ? (
          <EmptyState
            icon="search"
            title="No match"
            detail="No registration matches the current search and filter."
            action={
              <button className="btn secondary sm" onClick={() => { t.setQuery(''); setUrgency('') }}>
                Clear filters
              </button>
            }
          />
        ) : (
          <>
            <div className="table-scroll">
              <table className="data">
                <thead>
                  <tr>
                    <SortHeader {...t.sortProps('candidate')} onSort={() => t.toggleSort('candidate')}>Candidate</SortHeader>
                    <SortHeader {...t.sortProps('product_type')} onSort={() => t.toggleSort('product_type')}>Product</SortHeader>
                    <SortHeader {...t.sortProps('reference')} onSort={() => t.toggleSort('reference')}>Reference</SortHeader>
                    <SortHeader {...t.sortProps('payment_date')} onSort={() => t.toggleSort('payment_date')}>Paid</SortHeader>
                    <SortHeader {...t.sortProps('exam_schedule_deadline')} onSort={() => t.toggleSort('exam_schedule_deadline')}>Deadline</SortHeader>
                    <SortHeader {...t.sortProps('days_left')} onSort={() => t.toggleSort('days_left')}>Time left</SortHeader>
                  </tr>
                </thead>
                <tbody>
                  {t.rows.map((r) => {
                    const d = r.days_left
                    const u = urgencyOf(d)
                    return (
                      <tr key={r.id}>
                        <td>
                          <strong>{r.candidate || '—'}</strong>
                          <div className="small muted">{r.email}</div>
                        </td>
                        <td>{titleCase(r.product_type)}</td>
                        <td className="small muted">{r.reference || '—'}</td>
                        <td className="small">{fmtDate(r.payment_date)}</td>
                        <td className="small">{fmtDate(r.exam_schedule_deadline)}</td>
                        <td>
                          {d == null || u == null ? '—' : (
                            <Badge tone={URGENCY_TONE[u]}>{d < 0 ? `${-d}d overdue` : `${d}d left`}</Badge>
                          )}
                        </td>
                      </tr>
                    )
                  })}
                </tbody>
              </table>
            </div>
            <Pagination page={t.page} pages={t.pages} total={t.total} pageSize={t.pageSize} onPage={t.setPage} />
          </>
        )}
      </Card>
    </div>
  )
}
