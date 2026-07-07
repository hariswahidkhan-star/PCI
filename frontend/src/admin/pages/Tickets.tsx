import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type TicketRow, type TicketDetail } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
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
    } finally {
      setBusy(false)
    }
  }
  async function setStatus(status: string) {
    await adminApi.post(`/api/admin/tickets/${id}/status`, { status })
    refetch()
    onChanged()
  }

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
  const { data, loading, error, refetch } = useAdminQuery<{ rows: TicketRow[] }>(`/api/admin/tickets${status ? '?status=' + status : ''}`)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <h1>Support tickets</h1>

      <Card>
        <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 220 }}>
          <option value="">All statuses</option>
          {STATUSES.map((s) => <option key={s} value={s}>{s.replace(/_/g, ' ')}</option>)}
        </select>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No tickets match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Reference</th><th>Subject</th><th>Student</th><th>Messages</th><th>Updated</th><th>Status</th></tr>
            </thead>
            <tbody>
              {data.rows.map((t) => (
                <tr key={t.id} style={{ cursor: 'pointer' }} onClick={() => setSelected(t.id)}>
                  <td className="small muted">{t.reference}</td>
                  <td><strong>{t.subject || '—'}</strong></td>
                  <td className="small">{t.email}</td>
                  <td>{t.msg_count ?? 0}</td>
                  <td className="small muted">{fmtDateTime(t.updated_at)}</td>
                  <td><StatusBadge status={t.status} /></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {selected !== null && <TicketDrawer id={selected} onClose={() => setSelected(null)} onChanged={refetch} />}
    </div>
  )
}
