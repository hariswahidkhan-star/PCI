import { useState, type FormEvent } from 'react'
import { api } from '../api/client'
import { useQuery } from '../api/hooks'
import { Card, Spinner, ErrorNote, StatusBadge } from '../components/ui'
import { fmtDate } from '../format'
import { useT } from '../i18n'

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
  const t = useT()
  const { data, loading, error, refetch } = useQuery<{ rows: TicketRow[] }>('/api/me/tickets')
  const [subject, setSubject] = useState('')
  const [category, setCategory] = useState('general')
  const [body, setBody] = useState('')
  const [busy, setBusy] = useState(false)
  const [openId, setOpenId] = useState<number | null>(null)
  const [reply, setReply] = useState('')
  const [replyBusy, setReplyBusy] = useState(false)
  const [replyErr, setReplyErr] = useState<string | null>(null)
  const [note, setNote] = useState<{ ok: boolean; text: string } | null>(null)

  async function create(e: FormEvent) {
    e.preventDefault()
    setBusy(true)
    setNote(null)
    try {
      const r = await api.post<{ reference: string }>('/api/me/tickets', { subject, category, body })
      setSubject('')
      setBody('')
      setNote({ ok: true, text: t('sup.ticketCreated', { ref: r.reference }) })
      refetch()
    } catch (e2) {
      setNote({ ok: false, text: e2 instanceof Error ? e2.message : t('sup.createError') })
    } finally {
      setBusy(false)
    }
  }

  async function sendReply(id: number) {
    if (!reply.trim()) return
    setReplyBusy(true)
    setReplyErr(null)
    try {
      await api.post(`/api/me/tickets/${id}/reply`, { body: reply })
      setReply('')
      refetch()
    } catch (e2) {
      setReplyErr(e2 instanceof Error ? e2.message : t('sup.replyError'))
    } finally {
      setReplyBusy(false)
    }
  }

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>{t('sup.title')}</h1>
        <p className="muted">{t('sup.subtitle')}</p>
      </div>

      <Card title={t('sup.newRequest')}>
        {note && <div className={'notice' + (note.ok ? '' : ' err')} role={note.ok ? 'status' : 'alert'} style={{ marginBottom: '.75rem' }}>{note.text}</div>}
        <form onSubmit={create}>
          <div className="grid cols-2">
            <div className="field">
              <label htmlFor="tsub">{t('sup.subject')}</label>
              <input id="tsub" required maxLength={140} value={subject} onChange={(e) => setSubject(e.target.value)} />
            </div>
            <div className="field">
              <label htmlFor="tcat">{t('sup.category')}</label>
              <select id="tcat" value={category} onChange={(e) => setCategory(e.target.value)}>
                {CATEGORIES.map((c) => <option key={c} value={c}>{t('sup.cat.' + c)}</option>)}
              </select>
            </div>
          </div>
          <div className="field">
            <label htmlFor="tbody">{t('sup.howCanWeHelp')}</label>
            <textarea id="tbody" rows={4} required value={body} onChange={(e) => setBody(e.target.value)} />
          </div>
          <button className="btn sm" disabled={busy}>{busy ? t('sup.sending') : t('sup.submitRequest')}</button>
        </form>
      </Card>

      <Card title={t('sup.yourTickets', { count: rows.length })}>
        {rows.length === 0 ? (
          <p className="muted small" style={{ margin: 0 }}>{t('sup.noTickets')}</p>
        ) : (
          <div className="stack">
            {rows.map((ticket) => (
              <div className="rep-item" key={ticket.id} style={{ flexDirection: 'column', alignItems: 'stretch' }}>
                <button
                  type="button"
                  className="spread ticket-head"
                  onClick={() => setOpenId(openId === ticket.id ? null : ticket.id)}
                  aria-expanded={openId === ticket.id}
                >
                  <span>
                    <strong>{ticket.subject}</strong>
                    <span className="muted small"> · {ticket.reference} · {fmtDate(ticket.updated_at)}</span>
                  </span>
                  <StatusBadge status={ticket.status} />
                </button>
                {openId === ticket.id && (
                  <div className="ticket-thread fade-up">
                    {ticket.messages.map((m, i) => (
                      <div key={i} className={'tmsg ' + (m.sender === 'user' ? 'mine' : 'theirs')}>
                        <div className="small muted">{m.sender === 'user' ? t('sup.you') : 'PCI Global'} · {fmtDate(m.created_at)}</div>
                        <div>{m.body}</div>
                      </div>
                    ))}
                    <div className="row" style={{ marginTop: '.5rem' }}>
                      <input placeholder={t('sup.replyPlaceholder')} value={reply} onChange={(e) => setReply(e.target.value)} />
                      <button className="btn sm" type="button" disabled={replyBusy} onClick={() => sendReply(ticket.id)}>{replyBusy ? t('sup.sending') : t('sup.reply')}</button>
                    </div>
                    {replyErr && <div className="notice err" role="alert" style={{ marginTop: '.5rem' }}>{replyErr}</div>}
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
