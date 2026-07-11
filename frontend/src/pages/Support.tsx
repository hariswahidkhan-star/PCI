import { useState, type FormEvent } from 'react'
import { api } from '../api/client'
import { useQuery } from '../api/hooks'
import { Card, Spinner, ErrorNote, StatusBadge } from '../components/ui'
import { fmtDate } from '../format'

interface TicketMsg { sender: string; body: string; created_at?: string | null }
interface TicketRow {
  id: number
  reference: string
  subject: string
  category: string
  status: string
  updated_at?: string | null
  messages: TicketMsg[]
}

const CATEGORIES = ['general', 'exams', 'membership', 'billing', 'technical']

/** Support centre — real tickets against the same casework queue the admin team answers. */
export default function Support() {
  const { data, loading, error, refetch } = useQuery<{ rows: TicketRow[] }>('/api/me/tickets')
  const [subject, setSubject] = useState('')
  const [category, setCategory] = useState('general')
  const [body, setBody] = useState('')
  const [busy, setBusy] = useState(false)
  const [openId, setOpenId] = useState<number | null>(null)
  const [reply, setReply] = useState('')
  const [note, setNote] = useState<string | null>(null)

  async function create(e: FormEvent) {
    e.preventDefault()
    setBusy(true)
    setNote(null)
    try {
      const r = await api.post<{ reference: string }>('/api/me/tickets', { subject, category, body })
      setSubject('')
      setBody('')
      setNote(`Ticket ${r.reference} created — we'll reply here and by email.`)
      refetch()
    } catch (e2) {
      setNote(e2 instanceof Error ? e2.message : 'Could not create the ticket.')
    } finally {
      setBusy(false)
    }
  }

  async function sendReply(id: number) {
    if (!reply.trim()) return
    await api.post(`/api/me/tickets/${id}/reply`, { body: reply })
    setReply('')
    refetch()
  }

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Support</h1>
        <p className="muted">Questions about exams, membership or billing — answered by the PCI Global team.</p>
      </div>

      <Card title="New request">
        {note && <div className="notice" style={{ marginBottom: '.75rem' }}>{note}</div>}
        <form onSubmit={create}>
          <div className="grid cols-2">
            <div className="field">
              <label htmlFor="tsub">Subject</label>
              <input id="tsub" required maxLength={140} value={subject} onChange={(e) => setSubject(e.target.value)} />
            </div>
            <div className="field">
              <label htmlFor="tcat">Category</label>
              <select id="tcat" value={category} onChange={(e) => setCategory(e.target.value)}>
                {CATEGORIES.map((c) => <option key={c} value={c}>{c[0].toUpperCase() + c.slice(1)}</option>)}
              </select>
            </div>
          </div>
          <div className="field">
            <label htmlFor="tbody">How can we help?</label>
            <textarea id="tbody" rows={4} required value={body} onChange={(e) => setBody(e.target.value)} />
          </div>
          <button className="btn sm" disabled={busy}>{busy ? 'Sending…' : 'Submit request'}</button>
        </form>
      </Card>

      <Card title={`Your tickets (${rows.length})`}>
        {rows.length === 0 ? (
          <p className="muted small" style={{ margin: 0 }}>No tickets yet — anything you raise appears here with its full history.</p>
        ) : (
          <div className="stack">
            {rows.map((t) => (
              <div className="rep-item" key={t.id} style={{ flexDirection: 'column', alignItems: 'stretch' }}>
                <button
                  type="button"
                  className="spread ticket-head"
                  onClick={() => setOpenId(openId === t.id ? null : t.id)}
                  aria-expanded={openId === t.id}
                >
                  <span>
                    <strong>{t.subject}</strong>
                    <span className="muted small"> · {t.reference} · {fmtDate(t.updated_at)}</span>
                  </span>
                  <StatusBadge status={t.status} />
                </button>
                {openId === t.id && (
                  <div className="ticket-thread fade-up">
                    {t.messages.map((m, i) => (
                      <div key={i} className={'tmsg ' + (m.sender === 'user' ? 'mine' : 'theirs')}>
                        <div className="small muted">{m.sender === 'user' ? 'You' : 'PCI Global'} · {fmtDate(m.created_at)}</div>
                        <div>{m.body}</div>
                      </div>
                    ))}
                    <div className="row" style={{ marginTop: '.5rem' }}>
                      <input placeholder="Write a reply…" value={reply} onChange={(e) => setReply(e.target.value)} />
                      <button className="btn sm" type="button" onClick={() => sendReply(t.id)}>Reply</button>
                    </div>
                  </div>
                )}
              </div>
            ))}
          </div>
        )}
      </Card>
    </div>
  )
}
