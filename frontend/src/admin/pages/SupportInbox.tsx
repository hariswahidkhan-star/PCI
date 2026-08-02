import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useAdminQuery } from '../hooks'
import { adminApi, type TeamResponse } from '../api'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty, Stat, rowActivate } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { ViewDownloadActions } from '../../components/documents/DocumentActions'
import { fmtDate, fmtDateTime } from '../../format'
import { ApiError } from '../../api/client'

// Unified support inbox (gate 'inbox'): tickets + live chats + website enquiries in one queue,
// with SLA metrics on top and a full conversation drawer (reply, notes, assignment, KB) per ticket.

interface InboxTicket {
  id: number
  reference?: string | null
  subject?: string | null
  status: string
  priority?: string | null
  tags?: string | null
  escalated?: number | null
  assigned_to?: number | null
  assigned_email?: string | null
  user_id?: number | null
  student_email?: string | null
  is_test?: number | null
  updated_at?: string | null
  first_response_at?: string | null
  messages?: number | null
}
interface InboxChat {
  id: number
  visitor_name?: string | null
  status: string
  assigned_to?: number | null
  assigned_email?: string | null
  linked_user_id?: number | null
  student_email?: string | null
  last_activity_at?: string | null
}
interface InboxEnquiry {
  id: number
  name?: string | null
  email?: string | null
  subject?: string | null
  status?: string | null
  created_at?: string | null
}
interface InboxData { tickets: InboxTicket[]; chats: InboxChat[]; enquiries: InboxEnquiry[]; unassigned: number }

interface SupportMetrics {
  by_status: { status?: string | null; n?: number | null }[]
  escalated: number
  unassigned: number
  waiting_chats: number
  overdue_first_response: number
  overdue_resolution: number
  avg_first_response_mins: number | null
  avg_resolution_mins: number | null
  csat_avg: number | null
  csat_count: number
  sla: { first_response_mins: number; resolution_mins: number }
  agent_workload: { email?: string | null; n?: number | null }[]
}

interface TicketDetailResp {
  ticket: Record<string, unknown>
  messages: Record<string, unknown>[]
  notes: Record<string, unknown>[]
  student: Record<string, unknown> | null
  membership_status?: string | null
  recent_payments: Record<string, unknown>[]
  error_reports: Record<string, unknown>[]
}

const TICKET_STATUSES = ['open', 'awaiting_student', 'pending_internal', 'escalated', 'resolved', 'closed', 'spam']
const PRIORITIES = ['low', 'normal', 'high', 'urgent']

const s = (v: unknown): string => (v == null ? '' : String(v))

/** Priority chip: urgent red, high amber; low/normal stay quiet. */
function PriorityChip({ priority }: { priority?: string | null }) {
  const p = (priority ?? 'normal').toLowerCase()
  if (p === 'urgent') return <Badge tone="err">urgent</Badge>
  if (p === 'high') return <Badge tone="warn">high</Badge>
  return <Badge tone="neutral">{p}</Badge>
}

/** Minutes → friendly "3h 20m" for SLA averages. */
function fmtMins(v: number | null | undefined): string {
  if (v == null) return '—'
  const m = Math.round(v)
  if (m < 60) return `${m}m`
  const h = Math.floor(m / 60)
  if (h < 48) return `${h}h ${m % 60}m`
  return `${Math.round(h / 24)}d`
}

/** SLA targets form — the server enforces the 'support_admin' gate; a 403 is shown inline. */
function SlaForm({ sla, onSaved }: { sla: { first_response_mins: number; resolution_mins: number }; onSaved: () => void }) {
  const [first, setFirst] = useState(String(sla.first_response_mins))
  const [res, setRes] = useState(String(sla.resolution_mins))
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)
  async function save() {
    setBusy(true); setMsg(null)
    try {
      await adminApi.post('/api/support/sla', { first_response_mins: Number(first) || sla.first_response_mins, resolution_mins: Number(res) || sla.resolution_mins })
      setMsg({ ok: true, text: 'SLA targets saved.' })
      onSaved()
    } catch (e) {
      setMsg({ ok: false, text: e instanceof ApiError && e.status === 403 ? 'You need the support admin permission to change SLA targets.' : (e as Error).message })
    } finally { setBusy(false) }
  }
  return (
    <div>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
        <label className="small">First response (mins)<input type="number" min="5" value={first} onChange={(e) => setFirst(e.target.value)} style={{ maxWidth: 130 }} /></label>
        <label className="small">Resolution (mins)<input type="number" min="30" value={res} onChange={(e) => setRes(e.target.value)} style={{ maxWidth: 130 }} /></label>
        <button className="btn sm secondary" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save SLA targets'}</button>
      </div>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginTop: '.5rem' }}>{msg.text}</div>}
    </div>
  )
}

function MetricsRow({ onSlaSaved }: { onSlaSaved: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<SupportMetrics>('/api/support/metrics')
  if (loading && !data) return <Card><Spinner /></Card>
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null
  const byStatus = new Map((data.by_status ?? []).map((r) => [s(r.status), Number(r.n ?? 0)]))
  const red = (n: number) => (n > 0 ? <span style={{ color: 'var(--err,#dc2626)' }}>{n}</span> : n)
  return (
    <>
      <div className="grid cols-3">
        <Card><Stat n={byStatus.get('open') ?? 0} k="Open" /></Card>
        <Card><Stat n={byStatus.get('awaiting_student') ?? 0} k="Awaiting student" /></Card>
        <Card><Stat n={byStatus.get('pending_internal') ?? 0} k="Pending internal" /></Card>
        <Card><Stat n={red(data.escalated)} k="Escalated" /></Card>
        <Card><Stat n={data.unassigned} k="Unassigned" /></Card>
        <Card><Stat n={data.waiting_chats} k="Waiting chats" /></Card>
        <Card><Stat n={red(data.overdue_first_response)} k="Overdue first response" /></Card>
        <Card><Stat n={red(data.overdue_resolution)} k="Overdue resolution" /></Card>
        <Card><Stat n={data.csat_avg != null ? `★ ${data.csat_avg.toFixed(2)}` : '—'} k={`CSAT (${data.csat_count} rating${data.csat_count === 1 ? '' : 's'})`} /></Card>
      </div>
      <Card title="Service levels">
        <div className="small" style={{ marginBottom: '.6rem' }}>
          Avg first response: <strong>{fmtMins(data.avg_first_response_mins)}</strong> (target {fmtMins(data.sla.first_response_mins)}) ·{' '}
          Avg resolution: <strong>{fmtMins(data.avg_resolution_mins)}</strong> (target {fmtMins(data.sla.resolution_mins)})
        </div>
        <SlaForm sla={data.sla} onSaved={() => { refetch(); onSlaSaved() }} />
      </Card>
    </>
  )
}

// ---------------- conversation drawer ----------------

function AssignControl({ id, assignedTo, onDone }: { id: number; assignedTo: number | null; onDone: () => void }) {
  // /api/admin/team is owner-only — when it is not accessible, fall back to a free admin-id input.
  const { data: team, error: teamErr } = useAdminQuery<TeamResponse>('/api/admin/team')
  const [freeId, setFreeId] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  async function assign(adminId: number | null) {
    setBusy(true); setErr(null)
    try { await adminApi.post(`/api/support/tickets/${id}/assign`, { admin_id: adminId }); onDone() }
    catch (e) { setErr((e as Error).message) } finally { setBusy(false) }
  }
  return (
    <div>
      {team ? (
        <select
          aria-label="Assign to"
          value={assignedTo ?? ''}
          disabled={busy}
          onChange={(e) => assign(e.target.value === '' ? null : Number(e.target.value))}
        >
          <option value="">Unassigned</option>
          {team.rows.filter((a) => a.status === 'active').map((a) => <option key={a.id} value={a.id}>{a.email}</option>)}
        </select>
      ) : (
        <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap' }}>
          <input placeholder="Admin ID" value={freeId} onChange={(e) => setFreeId(e.target.value)} style={{ maxWidth: 110 }} aria-label="Admin ID" />
          <button className="btn sm secondary" disabled={busy} onClick={() => assign(freeId.trim() === '' ? null : Number(freeId))}>{freeId.trim() === '' ? 'Unassign' : 'Assign'}</button>
          {teamErr && <span className="muted small">Team list unavailable — enter the admin's ID.</span>}
        </div>
      )}
      {err && <div className="notice err" role="alert" style={{ marginTop: '.4rem' }}>{err}</div>}
    </div>
  )
}

function KbPanel({ onError }: { onError: (m: string) => void }) {
  const [q, setQ] = useState('')
  const [rows, setRows] = useState<{ id: number; question?: string | null; answer?: string | null }[] | null>(null)
  const [busy, setBusy] = useState(false)
  const [copied, setCopied] = useState<number | null>(null)
  const [sq, setSq] = useState('')
  const [sa, setSa] = useState('')
  const [suggestBusy, setSuggestBusy] = useState(false)
  const [suggestNote, setSuggestNote] = useState<string | null>(null)

  async function search() {
    setBusy(true)
    try { setRows((await adminApi.get<{ rows: { id: number; question?: string | null; answer?: string | null }[] }>(`/api/support/kb?q=${encodeURIComponent(q.trim())}`)).rows) }
    catch (e) { onError((e as Error).message) } finally { setBusy(false) }
  }
  async function copy(r: { id: number; answer?: string | null }) {
    try { await navigator.clipboard.writeText(s(r.answer)); setCopied(r.id); setTimeout(() => setCopied(null), 2000) }
    catch { onError('Could not copy to the clipboard.') }
  }
  async function suggest() {
    if (!sq.trim() || !sa.trim()) { setSuggestNote('A question and an answer are both required.'); return }
    setSuggestBusy(true); setSuggestNote(null)
    try {
      const r = await adminApi.post<{ note?: string }>('/api/support/kb/suggest', { question: sq.trim(), answer: sa.trim() })
      setSq(''); setSa(''); setSuggestNote(r.note ?? 'Saved as a draft — a content editor reviews and enables it.')
    } catch (e) { setSuggestNote((e as Error).message) } finally { setSuggestBusy(false) }
  }

  return (
    <Card title="Knowledge base">
      <div className="row" style={{ gap: '.4rem' }}>
        <input aria-label="Search approved answers" placeholder="Search approved answers…" value={q} onChange={(e) => setQ(e.target.value)} onKeyDown={(e) => { if (e.key === 'Enter') search() }} />
        <button className="btn sm secondary" disabled={busy} onClick={search}>{busy ? 'Searching…' : 'Search'}</button>
      </div>
      {rows && (
        rows.length === 0 ? <Empty>No approved articles match.</Empty> : (
          <div style={{ display: 'grid', gap: '.5rem', marginTop: '.6rem' }}>
            {rows.map((r) => (
              <div key={r.id} style={{ padding: '.5rem .7rem', borderRadius: 10, background: 'var(--canvas)' }}>
                <div className="small" style={{ fontWeight: 700 }}>{s(r.question)}</div>
                <div className="small" style={{ whiteSpace: 'pre-wrap' }}>{s(r.answer)}</div>
                <button className="btn ghost sm" style={{ marginTop: '.3rem' }} onClick={() => copy(r)}>{copied === r.id ? 'Copied' : 'Copy answer'}</button>
              </div>
            ))}
          </div>
        )
      )}
      <details style={{ marginTop: '.7rem' }}>
        <summary className="small" style={{ cursor: 'pointer', fontWeight: 600 }}>Suggest an article</summary>
        <div style={{ display: 'grid', gap: '.4rem', marginTop: '.5rem' }}>
          <input placeholder="Question" value={sq} onChange={(e) => setSq(e.target.value)} />
          <textarea rows={3} placeholder="Answer" value={sa} onChange={(e) => setSa(e.target.value)} />
          <div><button className="btn sm secondary" disabled={suggestBusy} onClick={suggest}>{suggestBusy ? 'Saving…' : 'Suggest article'}</button></div>
          {suggestNote && <div className="muted small" role="status">{suggestNote}</div>}
        </div>
      </details>
    </Card>
  )
}

/** Files the student attached to the ticket, streamed over the staff-side audited routes —
 *  previously the inbox had no way to see what a student uploaded. */
function TicketAttachments({ id }: { id: number }) {
  const { data, error } = useAdminQuery<{ rows: { id: number; filename?: string | null; mime?: string | null; size_bytes?: number | null; created_at?: string | null }[] }>(`/api/support/tickets/${id}/attachments`)
  const rows = data?.rows ?? []
  if (error || rows.length === 0) return null
  return (
    <Card title={`Attachments (${rows.length})`}>
      <div style={{ display: 'grid', gap: '.4rem' }}>
        {rows.map((a) => (
          <div key={a.id} className="row" style={{ gap: '.6rem', alignItems: 'center', flexWrap: 'wrap' }}>
            <span className="small"><strong>{a.filename || `attachment-${a.id}`}</strong> <span className="muted">{fmtDateTime(a.created_at)}</span></span>
            <ViewDownloadActions
              info={{ title: a.filename || `attachment-${a.id}`, filename: a.filename, mime: a.mime, sizeBytes: a.size_bytes }}
              inlineUrl={`/api/support/tickets/${id}/attachments/${a.id}?inline=1`}
              downloadUrl={`/api/support/tickets/${id}/attachments/${a.id}`}
              token={adminApi.getToken()}
            />
          </div>
        ))}
      </div>
    </Card>
  )
}

function ConversationDrawer({ id, onClose, onChanged }: { id: number; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<TicketDetailResp>(`/api/support/tickets/${id}`)
  const { data: templates } = useAdminQuery<{ rows: { id: number; title?: string | null; body?: string | null; category?: string | null }[] }>('/api/support/templates')
  const [reply, setReply] = useState('')
  const [note, setNote] = useState('')
  const [mentions, setMentions] = useState('')
  const [tags, setTags] = useState<string | null>(null)
  const [followup, setFollowup] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  const done = () => { refetch(); onChanged() }
  const t = (data?.ticket ?? {}) as Record<string, unknown>
  const student = (data?.student ?? null) as Record<string, unknown> | null

  async function post(path: string, body: Record<string, unknown>, after?: () => void) {
    setBusy(true); setErr(null)
    try { await adminApi.post(`/api/support/tickets/${id}/${path}`, body); after?.(); done() }
    catch (e) { setErr((e as Error).message) } finally { setBusy(false) }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Conversation</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data ? null : (
          <div className="page">
            {err && <div className="notice err" role="alert">{err}</div>}

            <Card
              title={s(t.subject) || s(t.reference) || 'Ticket'}
              action={<span className="row" style={{ gap: '.35rem' }}><PriorityChip priority={s(t.priority) || 'normal'} /><StatusBadge status={s(t.status)} /></span>}
            >
              <div className="small muted">{s(t.reference)}{t.category ? ` · ${s(t.category)}` : ''} · opened {fmtDateTime(t.created_at)}</div>
              <div className="row" style={{ marginTop: '.7rem', flexWrap: 'wrap', gap: '.6rem', alignItems: 'flex-end' }}>
                <label className="small">Status
                  <select value={s(t.status)} disabled={busy} onChange={(e) => post('status', { status: e.target.value })}>
                    {TICKET_STATUSES.map((st) => <option key={st} value={st}>{st.replace(/_/g, ' ')}</option>)}
                  </select>
                </label>
                <label className="small">Priority
                  <select value={s(t.priority) || 'normal'} disabled={busy} onChange={(e) => post('priority', { priority: e.target.value })}>
                    {PRIORITIES.map((p) => <option key={p} value={p}>{p}</option>)}
                  </select>
                </label>
                <label className="small">Assigned to<AssignControl id={id} assignedTo={t.assigned_to == null ? null : Number(t.assigned_to)} onDone={done} /></label>
              </div>
              <div className="row" style={{ marginTop: '.6rem', flexWrap: 'wrap', gap: '.6rem', alignItems: 'flex-end' }}>
                <label className="small" style={{ flex: 1, minWidth: 180 }}>Tags <span className="muted">(comma-separated)</span>
                  <input value={tags ?? s(t.tags)} onChange={(e) => setTags(e.target.value)} placeholder="billing, refund…" />
                </label>
                <button className="btn sm secondary" disabled={busy || tags === null} onClick={() => post('tags', { tags: tags ?? '' }, () => setTags(null))}>Save tags</button>
                <label className="small">Follow-up
                  <input type="date" value={followup || s(t.followup_at).slice(0, 10)} onChange={(e) => setFollowup(e.target.value)} />
                </label>
                <button className="btn sm secondary" disabled={busy || !followup} onClick={() => post('followup', { at: followup }, () => setFollowup(''))}>Set follow-up</button>
              </div>
            </Card>

            <TicketAttachments id={id} />

            <Card title={`Messages (${data.messages.length})`}>
              {data.messages.length === 0 ? <Empty>No messages yet.</Empty> : (
                <div style={{ display: 'grid', gap: '.6rem' }}>
                  {data.messages.map((m, i) => {
                    const admin = s(m.sender) === 'admin'
                    return (
                      <div key={i} style={{ padding: '.6rem .8rem', borderRadius: 10, background: admin ? 'var(--brand-050)' : 'var(--canvas)', marginLeft: admin ? '1.5rem' : 0, marginRight: admin ? 0 : '1.5rem' }}>
                        <div className="small" style={{ fontWeight: 700 }}>{admin ? 'Support' : 'Student'} <span className="muted" style={{ fontWeight: 400 }}>· {fmtDateTime(m.created_at)}</span></div>
                        <div style={{ whiteSpace: 'pre-wrap' }}>{s(m.body)}</div>
                      </div>
                    )
                  })}
                </div>
              )}
              <div className="field" style={{ marginTop: '1rem', marginBottom: 0 }}>
                <label>Reply to the student</label>
                <textarea rows={3} value={reply} onChange={(e) => setReply(e.target.value)} placeholder="Type a reply…" />
              </div>
              <div className="row" style={{ marginTop: '.5rem', flexWrap: 'wrap', gap: '.5rem' }}>
                <select
                  aria-label="Insert template"
                  value=""
                  onChange={(e) => {
                    const tpl = (templates?.rows ?? []).find((r) => String(r.id) === e.target.value)
                    if (tpl) setReply((prev) => (prev ? prev + '\n\n' : '') + s(tpl.body))
                  }}
                  style={{ maxWidth: 240 }}
                >
                  <option value="">Insert template…</option>
                  {(templates?.rows ?? []).map((r) => <option key={r.id} value={r.id}>{s(r.title)}</option>)}
                </select>
                <button className="btn sm" disabled={busy || !reply.trim()} onClick={() => post('reply', { body: reply.trim() }, () => setReply(''))}>{busy ? 'Working…' : 'Send reply'}</button>
              </div>
            </Card>

            <Card title={`Internal notes (${data.notes.length})`}>
              <p className="muted small" style={{ marginTop: 0 }}>Internal — never shown to the student.</p>
              {data.notes.length === 0 ? <Empty>No internal notes yet.</Empty> : (
                <div style={{ display: 'grid', gap: '.5rem' }}>
                  {data.notes.map((n, i) => (
                    <div key={i} style={{ padding: '.6rem .8rem', borderRadius: 10, background: 'var(--warn-bg,#fef3c7)', borderLeft: '3px solid var(--warn,#d97706)' }}>
                      <div className="small" style={{ fontWeight: 700 }}>{s(n.admin_email) || 'Admin'} <span className="muted" style={{ fontWeight: 400 }}>· {fmtDateTime(n.created_at)}</span></div>
                      <div className="small" style={{ whiteSpace: 'pre-wrap' }}>{s(n.body)}</div>
                      {s(n.mentions) && <div className="muted small" style={{ marginTop: '.2rem' }}>Mentioned: {s(n.mentions)}</div>}
                    </div>
                  ))}
                </div>
              )}
              <div className="field" style={{ marginTop: '.8rem', marginBottom: 0 }}>
                <label>Add a note</label>
                <textarea rows={2} value={note} onChange={(e) => setNote(e.target.value)} placeholder="Visible to the team only…" />
              </div>
              <div className="row" style={{ marginTop: '.5rem', flexWrap: 'wrap', gap: '.5rem' }}>
                <input placeholder="Mention admins (emails, comma-separated)" value={mentions} onChange={(e) => setMentions(e.target.value)} style={{ maxWidth: 300 }} />
                <button className="btn sm secondary" disabled={busy || !note.trim()} onClick={() => post('note', { body: note.trim(), mentions: mentions.trim() || undefined }, () => { setNote(''); setMentions('') })}>Add note</button>
              </div>
            </Card>

            <Card title="Student context">
              {!student ? <Empty>No linked student account.</Empty> : (
                <div className="small" style={{ display: 'grid', gap: '.35rem' }}>
                  <div>
                    <span className="muted">Email </span>
                    <Link to="/students">{s(student.email) || '—'}</Link>
                    {Number(student.is_test ?? 0) === 1 && <> <Badge tone="warn">TEST</Badge></>}
                  </div>
                  <div><span className="muted">Name </span>{`${s(student.first_name)} ${s(student.last_name)}`.trim() || '—'}</div>
                  <div><span className="muted">Membership </span>{data.membership_status || 'None'}</div>
                  <div><span className="muted">Account status </span><StatusBadge status={s(student.status)} /></div>
                </div>
              )}
              {data.recent_payments.length > 0 && (
                <>
                  <h4 style={{ margin: '.8rem 0 .3rem' }}>Last payments</h4>
                  <table className="data">
                    <thead><tr><th>Date</th><th>Product</th><th>Amount</th><th>Status</th></tr></thead>
                    <tbody>
                      {data.recent_payments.map((p, i) => (
                        <tr key={i}>
                          <td className="small muted">{fmtDate(p.payment_date)}</td>
                          <td className="small">{s(p.product_type)}</td>
                          <td className="small">{s(p.final_amount)}</td>
                          <td><StatusBadge status={s(p.payment_status)} /></td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </>
              )}
              {data.error_reports.length > 0 && (
                <>
                  <h4 style={{ margin: '.8rem 0 .3rem' }}>Error references</h4>
                  <div style={{ display: 'grid', gap: '.25rem' }}>
                    {data.error_reports.map((r, i) => (
                      <div key={i} className="small">
                        <span style={{ fontFamily: 'monospace' }}>{s(r.reference)}</span>
                        <span className="muted"> · {s(r.page) || '—'} · {fmtDate(r.created_at)} · </span>
                        <StatusBadge status={s(r.status)} />
                      </div>
                    ))}
                  </div>
                </>
              )}
            </Card>

            <KbPanel onError={setErr} />
          </div>
        )}
      </div>
    </div>
  )
}

// ---------------- queue list ----------------

export default function SupportInbox() {
  const { data, loading, error, refetch } = useAdminQuery<InboxData>('/api/support/inbox')
  const [selected, setSelected] = useState<number | null>(null)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <PageHeader
        title="Support inbox"
        actions={data && <span className="muted small">{data.unassigned} unassigned</span>}
      />

      <MetricsRow onSlaSaved={refetch} />

      {loading && !data ? (
        <Card><Spinner /></Card>
      ) : error ? (
        <ErrorNote>{error}</ErrorNote>
      ) : !data ? null : (
        <>
          <Card title={`Tickets (${data.tickets.length})`}>
            {data.tickets.length === 0 ? <Empty>No open tickets — the queue is clear.</Empty> : (
              <table className="data">
                <thead>
                  <tr><th>Reference</th><th>Subject</th><th>Student</th><th>Priority</th><th>Assigned</th><th>Updated</th><th>Status</th></tr>
                </thead>
                <tbody>
                  {data.tickets.map((t) => (
                    <tr key={t.id} {...rowActivate(() => setSelected(t.id))}>
                      <td className="small muted">{t.reference || `#${t.id}`}</td>
                      <td>
                        <strong>{t.subject || '—'}</strong>
                        {Number(t.is_test ?? 0) === 1 && <> <Badge tone="warn">TEST</Badge></>}
                        {Number(t.escalated ?? 0) === 1 && <> <Badge tone="err">escalated</Badge></>}
                        {t.tags && <div className="muted small">{t.tags}</div>}
                      </td>
                      <td className="small">{t.student_email || '—'}</td>
                      <td><PriorityChip priority={t.priority} /></td>
                      <td className="small">{t.assigned_email || <span className="muted">Unassigned</span>}</td>
                      <td className="small muted">{fmtDateTime(t.updated_at)}</td>
                      <td><StatusBadge status={t.status} /></td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </Card>

          <Card title={`Live chats (${data.chats.length})`} action={<a className="btn ghost sm" href="/admin-chat.html">Open chat console ↗</a>}>
            {data.chats.length === 0 ? <Empty>No chats waiting or live.</Empty> : (
              <table className="data">
                <thead><tr><th>Visitor</th><th>Student</th><th>Assigned</th><th>Last activity</th><th>Status</th><th /></tr></thead>
                <tbody>
                  {data.chats.map((c) => (
                    <tr key={c.id}>
                      <td><strong>{c.visitor_name || `Chat #${c.id}`}</strong></td>
                      <td className="small">{c.student_email || '—'}</td>
                      <td className="small">{c.assigned_email || <span className="muted">Unassigned</span>}</td>
                      <td className="small muted">{fmtDateTime(c.last_activity_at)}</td>
                      <td><StatusBadge status={c.status} /></td>
                      <td><a className="btn ghost sm" href="/admin-chat.html">Answer ↗</a></td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </Card>

          <Card title={`Enquiries (${data.enquiries.length})`} action={<Link className="btn ghost sm" to="/enquiries">All enquiries</Link>}>
            {data.enquiries.length === 0 ? <Empty>No open website enquiries.</Empty> : (
              <table className="data">
                <thead><tr><th>From</th><th>Subject</th><th>Received</th><th>Status</th></tr></thead>
                <tbody>
                  {data.enquiries.map((q) => (
                    <tr key={q.id}>
                      <td className="small"><strong>{q.name || '—'}</strong><div className="muted">{q.email || ''}</div></td>
                      <td className="small">{q.subject || '—'}</td>
                      <td className="small muted">{fmtDateTime(q.created_at)}</td>
                      <td><StatusBadge status={q.status} /></td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </Card>
        </>
      )}

      {selected !== null && <ConversationDrawer id={selected} onClose={() => setSelected(null)} onChanged={refetch} />}
    </div>
  )
}
