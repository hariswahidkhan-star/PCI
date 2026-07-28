import { useState } from 'react'
import { useAdminQuery, runMutation } from '../hooks'
import { adminApi, type TicketRow, type TicketDetail } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, rowActivate } from '../../components/ui'
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
import { fmtDateTime, titleCase } from '../../format'

const STATUSES = ['open', 'awaiting_student', 'resolved', 'closed']

function TicketDrawer({ id, onClose, onChanged }: { id: number; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<TicketDetail>(`/api/admin/tickets/${id}`)
  const [reply, setReply] = useState('')
  const [busy, setBusy] = useState(false)

  async function sendReply() {
    if (!reply.trim()) return
    setBusy(true)
    try {
      await adminApi.post(`/api/admin/tickets/${id}/reply`, { message: reply })
      setReply('')
      refetch()
      onChanged()
    } catch (e) {
      alert(e instanceof Error ? e.message : 'Could not send the reply.')
    } finally {
      setBusy(false)
    }
  }
  const setStatus = (status: string) =>
    runMutation(async () => { await adminApi.post(`/api/admin/tickets/${id}/status`, { status }); refetch(); onChanged() })

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Ticket</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data ? null : (
          <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
            <Card title={String(data.subject ?? data.reference ?? 'Ticket')} action={<StatusBadge status={data.status} />}>
              <div className="small muted">{data.email} · {data.reference} · {titleCase(data.category ?? '')}</div>
              <div className="row" style={{ marginTop: '.75rem', flexWrap: 'wrap' }}>
                <span className="muted small">Set status:</span>
                {STATUSES.map((s) => (
                  <button key={s} className="btn sm secondary" disabled={data.status === s} onClick={() => setStatus(s)}>{s.replace(/_/g, ' ')}</button>
                ))}
              </div>
            </Card>

            <Card title={`Conversation (${data.messages.length})`}>
              {data.messages.length === 0 ? (
                <Empty>No messages yet.</Empty>
              ) : (
                <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
                  {data.messages.map((m, i) => (
                    <div key={i} style={{ padding: '.6rem .8rem', borderRadius: 10, background: m.sender === 'admin' ? 'var(--brand-050)' : 'var(--canvas)' }}>
                      <div className="small" style={{ fontWeight: 700 }}>{m.sender === 'admin' ? 'Support' : 'Student'} <span className="muted" style={{ fontWeight: 400 }}>· {fmtDateTime(m.created_at)}</span></div>
                      <div style={{ whiteSpace: 'pre-wrap' }}>{m.body}</div>
                    </div>
                  ))}
                </div>
              )}
              <div className="field" style={{ marginTop: '1rem', marginBottom: 0 }}>
                <label>Reply</label>
                <textarea rows={3} value={reply} onChange={(e) => setReply(e.target.value)} placeholder="Type a reply to the student…" />
              </div>
              <button className="btn sm" style={{ marginTop: '.5rem' }} disabled={busy || !reply.trim()} onClick={sendReply}>{busy ? 'Sending…' : 'Send reply'}</button>
            </Card>
          </div>
        )}
      </div>
    </div>
  )
}

export default function Tickets() {
  const [status, setStatus] = useState('')
  const [selected, setSelected] = useState<number | null>(null)
  // Status stays a SERVER-side filter (the endpoint already supports ?status=); search, sort and
  // pagination run over whatever that returns. Narrowing on the server first keeps the payload
  // small on a busy queue, and the toolbar reads as one control set either way.
  const { data, loading, error, refetch } = useAdminQuery<{ rows: TicketRow[] }>(`/api/admin/tickets${status ? '?status=' + status : ''}`)

  const rows = data?.rows ?? []
  const t = useTableControls(rows, {
    searchKeys: ['reference', 'subject', 'email', 'first_name', 'last_name'],
    pageSize: 25,
    initialSort: { key: 'updated_at', dir: 'desc' },
  })

  return (
    <div className="page">
      <PageHeader
        eyebrow="Support"
        title="Support tickets"
        subtitle="Every request raised from the student portal, newest activity first."
      />

      <Toolbar count={t.total !== rows.length ? `${t.total} of ${rows.length} shown` : `${rows.length} ${rows.length === 1 ? 'ticket' : 'tickets'}`}>
        <SearchInput
          value={t.query}
          onChange={t.setQuery}
          label="Search"
          placeholder="Search reference, subject or student…"
        />
        <FilterSelect
          label="Status"
          value={status}
          onChange={setStatus}
          allLabel="All statuses"
          options={STATUSES.map((s) => ({ value: s, label: s.replace(/_/g, ' ') }))}
        />
      </Toolbar>

      <Card className="flat">
        {loading ? (
          <SkeletonTable rows={6} cols={6} />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <EmptyState
            icon="life-buoy"
            title={status ? 'No tickets with this status' : 'No support tickets yet'}
            detail={
              status
                ? 'Nothing is currently in this state — clear the status filter to see the whole queue.'
                : 'Requests raised from the student portal arrive here, with the full conversation thread.'
            }
            action={status ? <button className="btn secondary sm" onClick={() => setStatus('')}>Clear status filter</button> : undefined}
          />
        ) : t.total === 0 ? (
          <EmptyState
            icon="search"
            title="No match"
            detail="No ticket matches the current search."
            action={<button className="btn secondary sm" onClick={() => t.setQuery('')}>Clear search</button>}
          />
        ) : (
          <>
            <div className="table-scroll">
              <table className="data">
                <thead>
                  <tr>
                    <SortHeader {...t.sortProps('reference')} onSort={() => t.toggleSort('reference')}>Reference</SortHeader>
                    <SortHeader {...t.sortProps('subject')} onSort={() => t.toggleSort('subject')}>Subject</SortHeader>
                    <SortHeader {...t.sortProps('email')} onSort={() => t.toggleSort('email')}>Student</SortHeader>
                    <SortHeader {...t.sortProps('msg_count')} onSort={() => t.toggleSort('msg_count')}>Messages</SortHeader>
                    <SortHeader {...t.sortProps('updated_at')} onSort={() => t.toggleSort('updated_at')}>Updated</SortHeader>
                    <SortHeader {...t.sortProps('status')} onSort={() => t.toggleSort('status')}>Status</SortHeader>
                  </tr>
                </thead>
                <tbody>
                  {t.rows.map((row) => (
                    <tr key={row.id} {...rowActivate(() => setSelected(row.id))}>
                      <td className="small muted">{row.reference}</td>
                      <td><strong>{row.subject || '—'}</strong></td>
                      <td className="small">{row.email}</td>
                      <td>{row.msg_count ?? 0}</td>
                      <td className="small muted">{fmtDateTime(row.updated_at)}</td>
                      <td><StatusBadge status={row.status} /></td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <Pagination page={t.page} pages={t.pages} total={t.total} pageSize={t.pageSize} onPage={t.setPage} />
          </>
        )}
      </Card>

      {selected !== null && <TicketDrawer id={selected} onClose={() => setSelected(null)} onChanged={refetch} />}
    </div>
  )
}
