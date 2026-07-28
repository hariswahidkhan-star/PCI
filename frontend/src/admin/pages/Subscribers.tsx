import { useState } from 'react'
import { useAdminQuery, runMutation } from '../hooks'
import { adminApi, type Subscriber } from '../api'
import { Card, StatusBadge, ErrorNote } from '../../components/ui'
import {
  PageHeader,
  Toolbar,
  SearchInput,
  Segmented,
  SortHeader,
  EmptyState,
  SkeletonTable,
  Pagination,
} from '../../components/premium'
import { useTableControls } from '../../components/useTableControls'
import { fmtDate } from '../../format'

/* The newsletter list — the one admin collection with no natural ceiling, and until now the
   only way to find one address among thousands was the browser's own find-in-page.

   Everything is client-side: the endpoint returns the whole list (it has no query parameters),
   so narrowing here costs no round trip and needs no backend change. */

type State = 'all' | 'subscribed' | 'unsubscribed'

/** A subscriber with no status recorded is treated as subscribed — that is what the row
 *  actions already assume, so the filter must agree with them or the counts would disagree
 *  with the badges. */
const isActive = (s: Subscriber) => (s.status ?? 'subscribed') !== 'unsubscribed'

export default function Subscribers() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Subscriber[] }>('/api/admin/subscribers')
  const [state, setState] = useState<State>('all')

  const setStatus = (id: number, status: string) =>
    runMutation(async () => { await adminApi.patch(`/api/admin/subscribers/${id}`, { status }); refetch() })

  const rows = data?.rows ?? []
  const active = rows.filter(isActive).length

  const t = useTableControls(rows, {
    searchKeys: ['email'],
    filters: [state === 'all' ? null : (r: Subscriber) => isActive(r) === (state === 'subscribed')],
    filterKey: state,
    pageSize: 50,
    initialSort: { key: 'created_at', dir: 'desc' },
  })

  return (
    <div className="page">
      <PageHeader
        eyebrow="Community"
        title="Newsletter subscribers"
        subtitle="Everyone who opted in to the mailing list, newest first."
      />

      {rows.length > 0 && (
        <Toolbar count={t.total !== rows.length ? `${t.total} of ${rows.length} shown` : `${rows.length} total`}>
          <SearchInput value={t.query} onChange={t.setQuery} label="Search" placeholder="Search email…" />
          <Segmented<State>
            value={state}
            onChange={setState}
            label="Filter by subscription state"
            options={[
              { value: 'all', label: 'All', n: rows.length },
              { value: 'subscribed', label: 'Subscribed', n: active },
              { value: 'unsubscribed', label: 'Unsubscribed', n: rows.length - active },
            ]}
          />
        </Toolbar>
      )}

      <Card className="flat">
        {loading ? (
          <SkeletonTable rows={8} cols={4} />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <EmptyState
            icon="mail"
            title="No subscribers yet"
            detail="Addresses captured by the newsletter form on the public website appear here."
          />
        ) : t.total === 0 ? (
          <EmptyState
            icon="search"
            title="No match"
            detail="No subscriber matches the current search and filter."
            action={
              <button className="btn secondary sm" onClick={() => { t.setQuery(''); setState('all') }}>
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
                    <SortHeader {...t.sortProps('email')} onSort={() => t.toggleSort('email')}>Email</SortHeader>
                    <SortHeader {...t.sortProps('created_at')} onSort={() => t.toggleSort('created_at')}>Subscribed</SortHeader>
                    <SortHeader {...t.sortProps('status')} onSort={() => t.toggleSort('status')}>Status</SortHeader>
                    <th aria-label="Row actions" />
                  </tr>
                </thead>
                <tbody>
                  {t.rows.map((s) => {
                    const on = isActive(s)
                    return (
                      <tr key={s.id}>
                        <td>{s.email}</td>
                        <td className="small muted">{fmtDate(s.created_at)}</td>
                        <td><StatusBadge status={on ? 'active' : 'unsubscribed'} /></td>
                        <td className="actions">
                          <button className="btn ghost sm" onClick={() => setStatus(s.id, on ? 'unsubscribed' : 'subscribed')}>
                            {on ? 'Unsubscribe' : 'Resubscribe'}
                          </button>
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
