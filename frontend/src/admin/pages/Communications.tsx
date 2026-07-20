import { useEffect, useState, type ReactNode } from 'react'
import { adminApi } from '../api'
import { Card, Badge, Spinner, Empty } from '../../components/ui'
import { fmtDateTime } from '../../format'

/* Unified Communications Centre — a single tabbed module. All content (templates, sender profiles,
 * triggers, WhatsApp accounts, rules) is MySQL-backed via the backend; nothing is hardcoded here.
 * Provider SECRETS are never fetched or shown — the Dashboard reports configured/not booleans only. */

type Row = Record<string, any>
const TABS = ['Dashboard', 'Unified Inbox', 'Compose', 'Bulk Campaigns', 'Delivery Queue', 'Analytics', 'Templates', 'Sender Profiles', 'WhatsApp Accounts', 'Triggers', 'Suppression'] as const
type Tab = typeof TABS[number]

const STATUS_TONE: Record<string, 'ok' | 'err' | 'brand' | 'warn'> = {
  sent: 'ok', delivered: 'ok', queued: 'brand', scheduled: 'brand', processing: 'brand', retrying: 'warn',
  failed: 'err', rejected: 'err', cancelled: 'err', hard_bounced: 'err', open: 'brand', resolved: 'ok', escalated: 'warn',
}

export default function Communications() {
  const [tab, setTab] = useState<Tab>('Dashboard')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread"><h1>Communications Centre</h1></div>
      <p className="muted" style={{ margin: 0 }}>
        One place for all student and stakeholder communications — email, WhatsApp and in-app — with a
        reliable MySQL-backed outbox, templates, triggers, a unified inbox and delivery monitoring.
        Provider credentials live only in Render environment variables and are never shown here.
      </p>
      <div className="tabs" style={{ display: 'flex', gap: '.3rem', flexWrap: 'wrap', borderBottom: '1px solid var(--line)', paddingBottom: '.4rem' }}>
        {TABS.map((t) => (
          <button key={t} className={'btn sm ' + (tab === t ? '' : 'ghost')} onClick={() => setTab(t)}>{t}</button>
        ))}
      </div>
      {tab === 'Dashboard' && <Dashboard />}
      {tab === 'Unified Inbox' && <Inbox />}
      {tab === 'Compose' && <Compose />}
      {tab === 'Bulk Campaigns' && <Campaigns />}
      {tab === 'Analytics' && <Analytics />}
      {tab === 'Delivery Queue' && <Queue />}
      {tab === 'Templates' && <Templates />}
      {tab === 'Sender Profiles' && <Senders />}
      {tab === 'WhatsApp Accounts' && <WhatsApp />}
      {tab === 'Triggers' && <Triggers />}
      {tab === 'Suppression' && <Suppression />}
    </div>
  )
}

function useGet<T>(path: string, dep = 0) {
  const [data, setData] = useState<T | null>(null)
  const [loading, setLoading] = useState(true)
  const [err, setErr] = useState<string | null>(null)
  const [n, setN] = useState(0)
  useEffect(() => {
    let live = true; setLoading(true)
    adminApi.get<T>(path).then((d) => { if (live) { setData(d); setErr(null) } }).catch((e) => live && setErr(e instanceof Error ? e.message : 'Load failed')).finally(() => live && setLoading(false))
    return () => { live = false }
  }, [path, dep, n])
  return { data, loading, err, reload: () => setN((x) => x + 1) }
}

function Dashboard() {
  const { data } = useGet<Row>('/api/admin/comms/overview')
  const { data: prov } = useGet<Row>('/api/admin/comms/providers')
  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <Card title="At a glance">
        {!data ? <Spinner /> : (
          <div className="grid cols-4 small" style={{ display: 'grid', gridTemplateColumns: 'repeat(4,1fr)', gap: '.8rem' }}>
            <Stat k="Queued" v={data.queued} /><Stat k="Failed" v={data.failed} /><Stat k="Sent (24h)" v={data.sent_today} />
            <Stat k="Open conversations" v={data.open_conversations} /><Stat k="Sender profiles" v={data.sender_profiles} />
            <Stat k="WhatsApp accounts" v={data.whatsapp_accounts} /><Stat k="Templates" v={data.templates} /><Stat k="Triggers" v={data.triggers} />
          </div>
        )}
      </Card>
      <Card title="Provider status (configured in Render — values never shown)">
        {!prov ? <Spinner /> : (
          <div className="row small" style={{ flexWrap: 'wrap', gap: '.5rem' }}>
            <ProvBadge label="Email (Resend)" on={prov.email_resend} />
            <ProvBadge label="Email (SMTP)" on={prov.email_smtp} />
            <ProvBadge label="WhatsApp token" on={prov.whatsapp_token} />
            <ProvBadge label="WhatsApp verify" on={prov.whatsapp_verify} />
            <ProvBadge label="Email webhook secret" on={prov.email_webhook_secret} />
          </div>
        )}
        <p className="muted small" style={{ marginTop: '.6rem' }}>
          When a channel is not configured, messages are still recorded in the outbox (as a console send)
          so nothing is lost — set the environment variables in Render to activate live delivery.
        </p>
      </Card>
    </div>
  )
}
const Stat = ({ k, v }: { k: string; v: ReactNode }) => (
  <div><div className="muted">{k}</div><div style={{ fontWeight: 800, fontSize: '1.3rem' }}>{v ?? 0}</div></div>
)
const ProvBadge = ({ label, on }: { label: string; on: boolean }) => (
  <Badge tone={on ? 'ok' : 'warn'}>{label}: {on ? 'configured' : 'not set'}</Badge>
)

function Inbox() {
  const [status, setStatus] = useState('open')
  const { data, loading, reload } = useGet<{ rows: Row[] }>(`/api/admin/comms/conversations?status=${status}`)
  const [open, setOpen] = useState<Row | null>(null)
  const [detail, setDetail] = useState<{ conversation: Row; messages: Row[] } | null>(null)
  const [reply, setReply] = useState(''); const [note, setNote] = useState(false); const [busy, setBusy] = useState(false)
  async function view(id: number) { setDetail(await adminApi.get(`/api/admin/comms/conversations/${id}`)); setOpen({ id }) }
  async function send() {
    if (!detail || !reply.trim()) return
    setBusy(true)
    try { await adminApi.post(`/api/admin/comms/conversations/${detail.conversation.id}/reply`, { body: reply, internal_note: note ? 1 : 0 }); setReply(''); await view(detail.conversation.id); reload() }
    finally { setBusy(false) }
  }
  async function setConvStatus(s: string) { if (!detail) return; await adminApi.post(`/api/admin/comms/conversations/${detail.conversation.id}/status`, { status: s, assign_self: 1 }); await view(detail.conversation.id); reload() }
  return (
    <Card title="Unified Inbox" action={<select value={status} onChange={(e) => setStatus(e.target.value)}><option value="open">Open</option><option value="pending">Pending</option><option value="escalated">Escalated</option><option value="resolved">Resolved</option><option value="">All</option></select>}>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No conversations.</Empty> : (
        <table className="data"><thead><tr><th>Reference</th><th>Customer</th><th>Channel</th><th>Subject</th><th>Status</th><th /></tr></thead>
          <tbody>{data!.rows.map((c) => (
            <tr key={c.id}><td className="small">{c.reference}</td><td className="small">{c.customer_email || c.customer_phone}</td>
              <td><Badge>{c.channel}</Badge></td><td className="small">{c.subject}</td>
              <td><Badge tone={STATUS_TONE[c.status] ?? 'brand'}>{c.status}</Badge></td>
              <td><button className="btn ghost sm" onClick={() => view(c.id)}>Open</button></td></tr>
          ))}</tbody></table>
      )}
      {open && detail && (
        <div className="drawer-backdrop" onClick={() => setOpen(null)}>
          <div className="drawer" onClick={(e) => e.stopPropagation()}>
            <div className="spread"><h3 style={{ margin: 0 }}>{detail.conversation.subject}</h3><button className="btn secondary sm" onClick={() => setOpen(null)}>Close</button></div>
            <p className="muted small">{detail.conversation.reference} · {detail.conversation.customer_email || detail.conversation.customer_phone} · {detail.conversation.channel}</p>
            <div style={{ display: 'grid', gap: '.4rem', margin: '.6rem 0', maxHeight: 320, overflowY: 'auto' }}>
              {detail.messages.map((m) => (
                <div key={m.id} style={{ padding: '.5rem .7rem', borderRadius: 8, background: m.is_internal_note ? '#fef9c3' : m.direction === 'out' ? '#eff6ff' : '#f1f5f9' }}>
                  <div className="muted small">{m.direction === 'out' ? 'PCI' : 'Customer'}{m.is_internal_note ? ' · internal note' : ''} · {fmtDateTime(m.created_at)}</div>
                  <div className="small" style={{ whiteSpace: 'pre-wrap' }} dangerouslySetInnerHTML={{ __html: m.body }} />
                </div>
              ))}
            </div>
            <textarea rows={3} value={reply} onChange={(e) => setReply(e.target.value)} placeholder="Type a reply…" />
            <label className="small" style={{ display: 'flex', gap: '.4rem', margin: '.3rem 0' }}><input type="checkbox" checked={note} onChange={(e) => setNote(e.target.checked)} style={{ width: 'auto' }} /> Internal note (never sent to the customer)</label>
            <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
              <button className="btn sm" disabled={busy} onClick={send}>{note ? 'Add note' : 'Send reply'}</button>
              <button className="btn sm secondary" disabled={busy} onClick={() => setConvStatus('resolved')}>Resolve</button>
              <button className="btn sm secondary" disabled={busy} onClick={() => setConvStatus('escalated')}>Escalate</button>
            </div>
          </div>
        </div>
      )}
    </Card>
  )
}

function Compose() {
  const { data: senders } = useGet<{ rows: Row[] }>('/api/admin/comms/senders')
  const [f, setF] = useState({ channel: 'email', to: '', subject: '', body: '', sender_profile_key: 'support', category: 'operational' })
  const [msg, setMsg] = useState<string | null>(null); const [busy, setBusy] = useState(false)
  const set = (k: string, v: string) => setF((p) => ({ ...p, [k]: v }))
  async function send() {
    setBusy(true); setMsg(null)
    try { const r = await adminApi.post<Row>('/api/admin/comms/compose', f); setMsg(`Queued ${r.queued} message(s).`); setF((p) => ({ ...p, to: '', subject: '', body: '' })) }
    catch (e) { setMsg(e instanceof Error ? e.message : 'Send failed') } finally { setBusy(false) }
  }
  return (
    <Card title="Compose a manual message">
      {msg && <div className="notice ok" style={{ marginBottom: '.6rem' }}>{msg}</div>}
      <div style={{ display: 'grid', gap: '.5rem', maxWidth: 640 }}>
        <div className="grid2" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '.5rem' }}>
          <label className="field"><span>Channel</span><select value={f.channel} onChange={(e) => set('channel', e.target.value)}><option value="email">Email</option><option value="whatsapp">WhatsApp</option><option value="inapp">In-app</option></select></label>
          <label className="field"><span>Sender profile</span><select value={f.sender_profile_key} onChange={(e) => set('sender_profile_key', e.target.value)}>{(senders?.rows ?? []).map((s) => <option key={s.key} value={s.key}>{s.name}</option>)}</select></label>
          <label className="field"><span>Category</span><select value={f.category} onChange={(e) => set('category', e.target.value)}><option value="operational">Operational</option><option value="transactional">Transactional</option><option value="support">Support</option><option value="marketing">Marketing</option></select></label>
          <label className="field"><span>Recipient ({f.channel === 'whatsapp' ? 'phone' : 'email'})</span><input value={f.to} onChange={(e) => set('to', e.target.value)} /></label>
        </div>
        {f.channel === 'email' && <label className="field"><span>Subject</span><input value={f.subject} onChange={(e) => set('subject', e.target.value)} /></label>}
        <label className="field"><span>Message ({f.channel === 'email' ? 'HTML allowed' : 'text'}) — supports {'{{variables}}'}</span><textarea rows={5} value={f.body} onChange={(e) => set('body', e.target.value)} /></label>
        <div><button className="btn" disabled={busy || !f.to || !f.body} onClick={send}>{busy ? 'Sending…' : 'Send now'}</button></div>
      </div>
    </Card>
  )
}

function Queue() {
  const [status, setStatus] = useState('')
  const { data, loading, reload } = useGet<{ rows: Row[] }>(`/api/admin/comms/outbox${status ? `?status=${status}` : ''}`)
  async function act(id: number, action: string) { await adminApi.post(`/api/admin/comms/outbox/${id}/${action}`, {}); reload() }
  return (
    <Card title="Delivery Queue (outbox)" action={
      <div className="row" style={{ gap: '.4rem' }}>
        <select value={status} onChange={(e) => setStatus(e.target.value)}><option value="">All</option><option value="queued">Queued</option><option value="sent">Sent</option><option value="retrying">Retrying</option><option value="failed">Failed</option><option value="cancelled">Cancelled</option></select>
        <button className="btn sm secondary" onClick={async () => { await adminApi.post('/api/admin/comms/outbox/drain', {}); reload() }}>Process now</button>
      </div>}>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No messages.</Empty> : (
        <table className="data"><thead><tr><th>ID</th><th>Channel</th><th>To</th><th>Subject</th><th>Status</th><th>Attempts</th><th>Provider</th><th /></tr></thead>
          <tbody>{data!.rows.map((r) => (
            <tr key={r.id}><td>{r.id}</td><td><Badge>{r.channel}</Badge></td><td className="small">{r.to_email || r.to_phone}</td>
              <td className="small">{r.subject}</td><td><Badge tone={STATUS_TONE[r.status] ?? 'brand'}>{r.status}</Badge>{r.last_error && <div className="muted small">{String(r.last_error).slice(0, 60)}</div>}</td>
              <td className="small">{r.attempts}/{r.max_attempts}</td><td className="small">{r.provider || '—'}</td>
              <td className="row" style={{ gap: '.3rem' }}>
                {!['sent', 'delivered'].includes(r.status) && <button className="btn ghost sm" onClick={() => act(r.id, 'retry')}>Retry</button>}
                {!['sent', 'delivered', 'cancelled'].includes(r.status) && <button className="btn ghost sm" onClick={() => act(r.id, 'cancel')}>Cancel</button>}
              </td></tr>
          ))}</tbody></table>
      )}
    </Card>
  )
}

function Templates() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/comms/templates')
  const [edit, setEdit] = useState<Row | null>(null)
  async function save(t: Row) { await adminApi.post('/api/admin/comms/templates', t); setEdit(null); reload() }
  async function setStatus(id: number, status: string) {
    try { await adminApi.post(`/api/admin/comms/templates/${id}/status`, { status }); reload() }
    catch (e) { alert(e instanceof Error ? e.message : 'Failed') }
  }
  return (
    <Card title="Templates" action={<button className="btn sm" onClick={() => setEdit({ kind: 'email', category: 'operational', language: 'en' })}>New template</button>}>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No templates yet.</Empty> : (
        <table className="data"><thead><tr><th>Key</th><th>Name</th><th>Kind</th><th>Ver</th><th>Status</th><th /></tr></thead>
          <tbody>{data!.rows.map((t) => (
            <tr key={t.id}><td className="small">{t.key}</td><td>{t.name}</td><td><Badge>{t.kind}</Badge></td><td>v{t.version}</td>
              <td><Badge tone={t.status === 'published' ? 'ok' : t.status === 'archived' ? 'err' : 'brand'}>{t.status}</Badge></td>
              <td className="row" style={{ gap: '.3rem' }}>
                <button className="btn ghost sm" onClick={() => setEdit(t)}>Edit</button>
                {t.status !== 'published' && <button className="btn ghost sm" onClick={() => setStatus(t.id, 'published')}>Publish</button>}
                {t.status === 'published' && <button className="btn ghost sm" onClick={() => setStatus(t.id, 'archived')}>Archive</button>}
              </td></tr>
          ))}</tbody></table>
      )}
      {edit && <TemplateEditor t={edit} onClose={() => setEdit(null)} onSave={save} />}
    </Card>
  )
}
function TemplateEditor({ t, onClose, onSave }: { t: Row; onClose: () => void; onSave: (t: Row) => void }) {
  const [f, setF] = useState<Row>({ id: t.id, key: t.key ?? '', name: t.name ?? '', kind: t.kind ?? 'email', category: t.category ?? 'operational', subject: t.subject ?? '', body: t.body ?? '', wa_template_name: t.wa_template_name ?? '', language: t.language ?? 'en', required_vars: t.required_vars ?? '' })
  const set = (k: string, v: string) => setF((p) => ({ ...p, [k]: v }))
  return (
    <div className="drawer-backdrop" onClick={onClose}><div className="drawer" onClick={(e) => e.stopPropagation()}>
      <div className="spread"><h3 style={{ margin: 0 }}>{t.id ? 'Edit template' : 'New template'}</h3><button className="btn secondary sm" onClick={onClose}>Close</button></div>
      <div style={{ display: 'grid', gap: '.5rem', marginTop: '.6rem' }}>
        <label className="field"><span>Key</span><input value={f.key} onChange={(e) => set('key', e.target.value)} disabled={!!t.id} /></label>
        <label className="field"><span>Name</span><input value={f.name} onChange={(e) => set('name', e.target.value)} /></label>
        <div className="grid2" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '.5rem' }}>
          <label className="field"><span>Kind</span><select value={f.kind} onChange={(e) => set('kind', e.target.value)}><option value="email">Email</option><option value="whatsapp">WhatsApp</option><option value="auto_response">Auto response</option><option value="internal">Internal alert</option><option value="bulk">Bulk</option></select></label>
          <label className="field"><span>Language</span><input value={f.language} onChange={(e) => set('language', e.target.value)} /></label>
        </div>
        {f.kind === 'email' && <label className="field"><span>Subject</span><input value={f.subject} onChange={(e) => set('subject', e.target.value)} /></label>}
        {f.kind === 'whatsapp' && <label className="field"><span>Approved WhatsApp template name (provider)</span><input value={f.wa_template_name} onChange={(e) => set('wa_template_name', e.target.value)} /></label>}
        <label className="field"><span>Body — supports {'{{variables}}'}</span><textarea rows={6} value={f.body} onChange={(e) => set('body', e.target.value)} /></label>
        <label className="field"><span>Required variables (comma-separated; validated on publish)</span><input value={f.required_vars} onChange={(e) => set('required_vars', e.target.value)} placeholder="student_name,portal_link" /></label>
        <div><button className="btn" disabled={!f.key || !f.name} onClick={() => onSave(f)}>Save</button></div>
      </div>
    </div></div>
  )
}

function Senders() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/comms/senders')
  const [edit, setEdit] = useState<Row | null>(null); const [test, setTest] = useState<number | null>(null)
  async function save(s: Row) { await adminApi.post('/api/admin/comms/senders', s); setEdit(null); reload() }
  async function doTest(id: number) { setTest(id); try { await adminApi.post(`/api/admin/comms/senders/${id}/test`, {}); alert('Test message queued to your admin email.') } finally { setTest(null) } }
  return (
    <Card title="Email Sender Profiles" action={<button className="btn sm" onClick={() => setEdit({ category: 'operational' })}>New profile</button>}>
      <p className="muted small" style={{ marginTop: 0 }}>SMTP/API credentials are never stored or shown here — only the identity. Set provider keys in Render.</p>
      {loading ? <Spinner /> : (
        <table className="data"><thead><tr><th>Key</th><th>Name</th><th>From</th><th>Category</th><th>Verified</th><th>Active</th><th /></tr></thead>
          <tbody>{(data?.rows ?? []).map((s) => (
            <tr key={s.id}><td className="small">{s.key}{s.is_default ? ' ·default' : ''}</td><td>{s.name}</td><td className="small">{s.from_email}</td>
              <td><Badge>{s.category}</Badge></td><td>{s.domain_verified ? '✓' : '—'}</td><td>{s.active ? '✓' : '—'}</td>
              <td className="row" style={{ gap: '.3rem' }}><button className="btn ghost sm" onClick={() => setEdit(s)}>Edit</button><button className="btn ghost sm" disabled={test === s.id} onClick={() => doTest(s.id)}>Test</button></td></tr>
          ))}</tbody></table>
      )}
      {edit && <SenderEditor s={edit} onClose={() => setEdit(null)} onSave={save} />}
    </Card>
  )
}
function SenderEditor({ s, onClose, onSave }: { s: Row; onClose: () => void; onSave: (s: Row) => void }) {
  const [f, setF] = useState<Row>({ key: s.key ?? '', name: s.name ?? '', display_name: s.display_name ?? 'Project Controls Institute', from_email: s.from_email ?? '', reply_to: s.reply_to ?? '', category: s.category ?? 'operational', provider: s.provider ?? 'resend', domain_verified: s.domain_verified ?? 0, is_default: s.is_default ?? 0, active: s.active ?? 1, owner: s.owner ?? '' })
  const set = (k: string, v: any) => setF((p) => ({ ...p, [k]: v }))
  return (
    <div className="drawer-backdrop" onClick={onClose}><div className="drawer" onClick={(e) => e.stopPropagation()}>
      <div className="spread"><h3 style={{ margin: 0 }}>Sender profile</h3><button className="btn secondary sm" onClick={onClose}>Close</button></div>
      <div style={{ display: 'grid', gap: '.5rem', marginTop: '.6rem' }}>
        <label className="field"><span>Key</span><input value={f.key} onChange={(e) => set('key', e.target.value)} disabled={!!s.key} /></label>
        <label className="field"><span>Name</span><input value={f.name} onChange={(e) => set('name', e.target.value)} /></label>
        <label className="field"><span>Display name</span><input value={f.display_name} onChange={(e) => set('display_name', e.target.value)} /></label>
        <div className="grid2" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '.5rem' }}>
          <label className="field"><span>From email</span><input value={f.from_email} onChange={(e) => set('from_email', e.target.value)} /></label>
          <label className="field"><span>Reply-to</span><input value={f.reply_to} onChange={(e) => set('reply_to', e.target.value)} /></label>
          <label className="field"><span>Category</span><select value={f.category} onChange={(e) => set('category', e.target.value)}><option>operational</option><option>transactional</option><option>support</option><option>marketing</option><option>legal</option></select></label>
          <label className="field"><span>Owner</span><input value={f.owner} onChange={(e) => set('owner', e.target.value)} /></label>
        </div>
        <div className="row small" style={{ gap: '1rem' }}>
          <label style={{ display: 'flex', gap: '.4rem' }}><input type="checkbox" checked={!!f.domain_verified} onChange={(e) => set('domain_verified', e.target.checked ? 1 : 0)} style={{ width: 'auto' }} /> Domain verified</label>
          <label style={{ display: 'flex', gap: '.4rem' }}><input type="checkbox" checked={!!f.active} onChange={(e) => set('active', e.target.checked ? 1 : 0)} style={{ width: 'auto' }} /> Active</label>
          <label style={{ display: 'flex', gap: '.4rem' }}><input type="checkbox" checked={!!f.is_default} onChange={(e) => set('is_default', e.target.checked ? 1 : 0)} style={{ width: 'auto' }} /> Default</label>
        </div>
        <div><button className="btn" disabled={!f.key || !f.from_email} onClick={() => onSave(f)}>Save</button></div>
      </div>
    </div></div>
  )
}

function WhatsApp() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/comms/whatsapp')
  const [edit, setEdit] = useState<Row | null>(null)
  async function save(a: Row) { await adminApi.post('/api/admin/comms/whatsapp', a); setEdit(null); reload() }
  return (
    <Card title="WhatsApp Business Accounts" action={<button className="btn sm" onClick={() => setEdit({})}>New account</button>}>
      <p className="muted small" style={{ marginTop: 0 }}>Store only the phone-number ID and the NAME of the env var holding the token — never the token itself.</p>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No WhatsApp accounts configured.</Empty> : (
        <table className="data"><thead><tr><th>Key</th><th>Number</th><th>Provider</th><th>Verified</th><th>Active</th><th /></tr></thead>
          <tbody>{data!.rows.map((a) => (
            <tr key={a.id}><td className="small">{a.key}{a.is_default ? ' ·default' : ''}</td><td className="small">{a.phone_number}</td><td>{a.provider}</td>
              <td><Badge tone={a.verification_status === 'verified' ? 'ok' : 'warn'}>{a.verification_status}</Badge></td><td>{a.active ? '✓' : '—'}</td>
              <td><button className="btn ghost sm" onClick={() => setEdit(a)}>Edit</button></td></tr>
          ))}</tbody></table>
      )}
      {edit && <WaEditor a={edit} onClose={() => setEdit(null)} onSave={save} />}
    </Card>
  )
}
function WaEditor({ a, onClose, onSave }: { a: Row; onClose: () => void; onSave: (a: Row) => void }) {
  const [f, setF] = useState<Row>({ key: a.key ?? '', name: a.name ?? '', display_name: a.display_name ?? '', phone_number: a.phone_number ?? '', provider: a.provider ?? 'meta_cloud', provider_account_id: a.provider_account_id ?? '', token_env: a.token_env ?? 'WHATSAPP_ACCESS_TOKEN', country: a.country ?? '', verification_status: a.verification_status ?? 'unverified', is_default: a.is_default ?? 0, active: a.active ?? 1 })
  const set = (k: string, v: any) => setF((p) => ({ ...p, [k]: v }))
  return (
    <div className="drawer-backdrop" onClick={onClose}><div className="drawer" onClick={(e) => e.stopPropagation()}>
      <div className="spread"><h3 style={{ margin: 0 }}>WhatsApp account</h3><button className="btn secondary sm" onClick={onClose}>Close</button></div>
      <div style={{ display: 'grid', gap: '.5rem', marginTop: '.6rem' }}>
        <label className="field"><span>Key</span><input value={f.key} onChange={(e) => set('key', e.target.value)} disabled={!!a.key} /></label>
        <label className="field"><span>Name</span><input value={f.name} onChange={(e) => set('name', e.target.value)} /></label>
        <div className="grid2" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '.5rem' }}>
          <label className="field"><span>Business phone number</span><input value={f.phone_number} onChange={(e) => set('phone_number', e.target.value)} /></label>
          <label className="field"><span>Provider account / phone-number ID</span><input value={f.provider_account_id} onChange={(e) => set('provider_account_id', e.target.value)} /></label>
          <label className="field"><span>Token env var name</span><input value={f.token_env} onChange={(e) => set('token_env', e.target.value)} /></label>
          <label className="field"><span>Country</span><input value={f.country} onChange={(e) => set('country', e.target.value)} /></label>
        </div>
        <div className="row small" style={{ gap: '1rem' }}>
          <label style={{ display: 'flex', gap: '.4rem' }}><input type="checkbox" checked={f.verification_status === 'verified'} onChange={(e) => set('verification_status', e.target.checked ? 'verified' : 'unverified')} style={{ width: 'auto' }} /> Verified</label>
          <label style={{ display: 'flex', gap: '.4rem' }}><input type="checkbox" checked={!!f.active} onChange={(e) => set('active', e.target.checked ? 1 : 0)} style={{ width: 'auto' }} /> Active</label>
          <label style={{ display: 'flex', gap: '.4rem' }}><input type="checkbox" checked={!!f.is_default} onChange={(e) => set('is_default', e.target.checked ? 1 : 0)} style={{ width: 'auto' }} /> Default</label>
        </div>
        <div><button className="btn" disabled={!f.key || !f.phone_number} onClick={() => onSave(f)}>Save</button></div>
      </div>
    </div></div>
  )
}

function Triggers() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/comms/triggers')
  const [group, setGroup] = useState('')
  const groups = Array.from(new Set((data?.rows ?? []).map((r) => r.event_group)))
  const rows = (data?.rows ?? []).filter((r) => !group || r.event_group === group)
  async function toggle(id: number, field: string, val: number) { await adminApi.patch(`/api/admin/comms/triggers/${id}`, { [field]: val }); reload() }
  return (
    <Card title="Automated Triggers" action={<select value={group} onChange={(e) => setGroup(e.target.value)}><option value="">All groups</option>{groups.map((g) => <option key={g} value={g}>{g}</option>)}</select>}>
      <p className="muted small" style={{ marginTop: 0 }}>Toggle email / WhatsApp / in-app per event. Events marked “needs wiring” are catalogued but no backend event emits them yet.</p>
      {loading ? <Spinner /> : (
        <table className="data"><thead><tr><th>Event</th><th>Group</th><th>Email</th><th>WhatsApp</th><th>In-app</th><th>Backend</th></tr></thead>
          <tbody>{rows.map((t) => (
            <tr key={t.id}><td className="small">{t.name}<div className="muted">{t.code}</div></td><td className="small">{t.event_group}</td>
              <td><Toggle on={!!t.email_enabled} onChange={(v) => toggle(t.id, 'email_enabled', v)} /></td>
              <td><Toggle on={!!t.whatsapp_enabled} onChange={(v) => toggle(t.id, 'whatsapp_enabled', v)} /></td>
              <td><Toggle on={!!t.inapp_enabled} onChange={(v) => toggle(t.id, 'inapp_enabled', v)} /></td>
              <td>{t.backend_wired ? <Badge tone="ok">wired</Badge> : <Badge tone="warn">needs wiring</Badge>}</td></tr>
          ))}</tbody></table>
      )}
    </Card>
  )
}
const Toggle = ({ on, onChange }: { on: boolean; onChange: (v: number) => void }) => (
  <input type="checkbox" checked={on} onChange={(e) => onChange(e.target.checked ? 1 : 0)} style={{ width: 'auto' }} />
)

function Suppression() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/comms/suppression')
  const [addr, setAddr] = useState(''); const [channel, setChannel] = useState('email'); const [reason, setReason] = useState('unsubscribe')
  async function add() { if (!addr.trim()) return; await adminApi.post('/api/admin/comms/suppression', { address: addr, channel, reason, category: 'marketing' }); setAddr(''); reload() }
  async function remove(id: number) { await adminApi.del(`/api/admin/comms/suppression/${id}`); reload() }
  return (
    <Card title="Suppression & unsubscribes">
      <p className="muted small" style={{ marginTop: 0 }}>Marketing messages to a suppressed address are dropped and recorded. Essential/transactional messages are never suppressed.</p>
      <div className="row" style={{ gap: '.4rem', marginBottom: '.6rem', flexWrap: 'wrap' }}>
        <select value={channel} onChange={(e) => setChannel(e.target.value)}><option value="email">Email</option><option value="whatsapp">WhatsApp</option></select>
        <input placeholder="address or number" value={addr} onChange={(e) => setAddr(e.target.value)} />
        <select value={reason} onChange={(e) => setReason(e.target.value)}><option value="unsubscribe">Unsubscribe</option><option value="complaint">Complaint</option><option value="bounce">Hard bounce</option></select>
        <button className="btn sm" onClick={add}>Add</button>
      </div>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No suppressed recipients.</Empty> : (
        <table className="data"><thead><tr><th>Channel</th><th>Address</th><th>Reason</th><th>When</th><th /></tr></thead>
          <tbody>{data!.rows.map((s) => (
            <tr key={s.id}><td>{s.channel}</td><td className="small">{s.address}</td><td>{s.reason}</td><td className="small muted">{fmtDateTime(s.created_at)}</td>
              <td><button className="btn ghost sm" onClick={() => remove(s.id)}>Remove</button></td></tr>
          ))}</tbody></table>
      )}
    </Card>
  )
}

function Campaigns() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/comms/campaigns')
  const [edit, setEdit] = useState<Row | null>(null)
  return (
    <Card title="Bulk Campaigns" action={<button className="btn sm" onClick={() => setEdit({ channel: 'email', category: 'marketing' })}>New campaign</button>}>
      <p className="muted small" style={{ marginTop: 0 }}>Each recipient gets a separate message — addresses are never exposed to others. Consent, suppression and duplicate-prevention are enforced on send.</p>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No campaigns yet.</Empty> : (
        <table className="data"><thead><tr><th>Name</th><th>Channel</th><th>Category</th><th>Status</th><th>Sent</th><th /></tr></thead>
          <tbody>{data!.rows.map((c) => (
            <tr key={c.id}><td>{c.name}</td><td><Badge>{c.channel}</Badge></td><td className="small">{c.category}</td>
              <td><Badge tone={c.status === 'sent' ? 'ok' : c.status === 'cancelled' ? 'err' : 'brand'}>{c.status}</Badge></td>
              <td className="small">{c.queued || 0}/{c.total || 0}</td>
              <td><button className="btn ghost sm" onClick={() => setEdit(c)}>Manage</button></td></tr>
          ))}</tbody></table>
      )}
      {edit && <CampaignEditor c={edit} onClose={() => setEdit(null)} onChanged={reload} />}
    </Card>
  )
}
function CampaignEditor({ c, onClose, onChanged }: { c: Row; onClose: () => void; onChanged: () => void }) {
  const [f, setF] = useState<Row>({ id: c.id, name: c.name ?? '', channel: c.channel ?? 'email', category: c.category ?? 'marketing', subject: c.subject ?? '', body: c.body ?? '', sender_profile_key: c.sender_profile_key ?? 'marketing' })
  const [filters, setFilters] = useState<Row>(() => { try { return c.filters ? JSON.parse(c.filters) : {} } catch { return {} } })
  const [preview, setPreview] = useState<Row | null>(null)
  const [detail, setDetail] = useState<Row | null>(null)
  const [busy, setBusy] = useState(false); const [msg, setMsg] = useState<string | null>(null)
  const set = (k: string, v: string) => setF((p) => ({ ...p, [k]: v }))
  const setFilter = (k: string, v: any) => setFilters((p) => ({ ...p, [k]: v }))
  async function save(): Promise<number> { const r = await adminApi.post<Row>('/api/admin/comms/campaigns', { ...f, filters }); return r.id ?? f.id }
  async function doPreview() { setBusy(true); setMsg(null); try { const id = await save(); setPreview(await adminApi.post(`/api/admin/comms/campaigns/${id}/preview`, {})); setF((p) => ({ ...p, id })) } finally { setBusy(false) } }
  async function doTest() { setBusy(true); try { const id = await save(); await adminApi.post(`/api/admin/comms/campaigns/${id}/test`, {}); setMsg('Test message queued to your admin email.') } finally { setBusy(false) } }
  async function doApprove() { setBusy(true); try { const id = await save(); await adminApi.post(`/api/admin/comms/campaigns/${id}/approve`, {}); setMsg('Approved — you can now send.'); setF((p) => ({ ...p, id })); refreshDetail(id) } finally { setBusy(false) } }
  async function doSend() { if (!window.confirm('Send this campaign to the full audience now?')) return; setBusy(true); try { const id = await save(); const r = await adminApi.post<Row>(`/api/admin/comms/campaigns/${id}/send`, {}); setMsg(`Queued ${r.queued} of ${r.audience} recipients.`); onChanged(); refreshDetail(id) } catch (e) { setMsg(e instanceof Error ? e.message : 'Send failed') } finally { setBusy(false) } }
  async function refreshDetail(id: number) { setDetail(await adminApi.get(`/api/admin/comms/campaigns/${id}`)) }
  return (
    <div className="drawer-backdrop" onClick={onClose}><div className="drawer" onClick={(e) => e.stopPropagation()}>
      <div className="spread"><h3 style={{ margin: 0 }}>{c.id ? 'Manage campaign' : 'New campaign'}</h3><button className="btn secondary sm" onClick={onClose}>Close</button></div>
      {msg && <div className="notice ok" style={{ margin: '.5rem 0' }}>{msg}</div>}
      <div style={{ display: 'grid', gap: '.5rem', marginTop: '.6rem' }}>
        <label className="field"><span>Name</span><input value={f.name} onChange={(e) => set('name', e.target.value)} /></label>
        <div className="grid2" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '.5rem' }}>
          <label className="field"><span>Channel</span><select value={f.channel} onChange={(e) => set('channel', e.target.value)}><option value="email">Email</option><option value="whatsapp">WhatsApp</option></select></label>
          <label className="field"><span>Category</span><select value={f.category} onChange={(e) => set('category', e.target.value)}><option value="marketing">Marketing</option><option value="newsletter">Newsletter</option><option value="operational">Operational</option></select></label>
        </div>
        <label className="field"><span>Subject</span><input value={f.subject} onChange={(e) => set('subject', e.target.value)} /></label>
        <label className="field"><span>Body (HTML, {'{{variables}}'})</span><textarea rows={4} value={f.body} onChange={(e) => set('body', e.target.value)} /></label>
        <div style={{ borderTop: '1px solid var(--line)', paddingTop: '.5rem' }}><strong className="small">Audience filters</strong></div>
        <div className="grid2" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '.5rem' }}>
          <label className="field"><span>Holds certification (name contains)</span><input value={filters.certification ?? ''} onChange={(e) => setFilter('certification', e.target.value)} placeholder="PCL-AI" /></label>
          <label className="field"><span>Registered after</span><input value={filters.registered_after ?? ''} onChange={(e) => setFilter('registered_after', e.target.value)} placeholder="2026-01-01" /></label>
          <label className="small" style={{ display: 'flex', gap: '.4rem', alignItems: 'center' }}><input type="checkbox" checked={!!filters.has_membership} onChange={(e) => setFilter('has_membership', e.target.checked)} style={{ width: 'auto' }} /> Has a membership</label>
          <label className="small" style={{ display: 'flex', gap: '.4rem', alignItems: 'center' }}><input type="checkbox" checked={!!filters.active_only} onChange={(e) => setFilter('active_only', e.target.checked)} style={{ width: 'auto' }} /> Active accounts only</label>
        </div>
        {(f.category === 'marketing' || f.category === 'newsletter') && <p className="muted small" style={{ margin: 0 }}>Marketing/newsletter automatically requires each recipient's email-marketing consent.</p>}
        {preview && <div className="notice" style={{ background: '#eff6ff' }}><strong>{preview.total}</strong> eligible recipient(s){preview.suppressed ? ` · ${preview.suppressed} suppressed` : ''}. Sample: {(preview.sample ?? []).join(', ') || '—'}<div className="muted small">{preview.note}</div></div>}
        {detail?.delivery && <div className="notice" style={{ background: '#f0fdf4' }}>Delivery: {(detail.delivery as Row[]).map((d) => `${d.status}: ${d.n}`).join(' · ') || 'none yet'}</div>}
        <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
          <button className="btn sm secondary" disabled={busy} onClick={doPreview}>Preview audience</button>
          <button className="btn sm secondary" disabled={busy} onClick={doTest}>Send test</button>
          <button className="btn sm secondary" disabled={busy} onClick={doApprove}>Approve</button>
          <button className="btn sm" disabled={busy} onClick={doSend}>Send now</button>
        </div>
      </div>
    </div></div>
  )
}

function Analytics() {
  const { data, loading } = useGet<Row>('/api/admin/comms/analytics')
  if (loading) return <Spinner />
  if (!data) return <Empty>No data.</Empty>
  const Section = ({ title, rows, k, v }: { title: string; rows: Row[]; k: string; v: string }) => (
    <Card title={title}>
      {(rows ?? []).length === 0 ? <Empty>None.</Empty> : (
        <table className="data"><tbody>{rows.map((r, i) => (<tr key={i}><td className="small">{String(r[k] ?? '—')}</td><td style={{ textAlign: 'right', fontWeight: 700 }}>{r[v]}</td></tr>))}</tbody></table>
      )}
    </Card>
  )
  return (
    <div style={{ display: 'grid', gap: '1rem', gridTemplateColumns: '1fr 1fr' }}>
      <Section title="By status" rows={data.by_status} k="status" v="n" />
      <Section title="By channel" rows={data.by_channel} k="channel" v="n" />
      <Section title="Sends per day (14d)" rows={data.by_day} k="day" v="n" />
      <Section title="Top triggers" rows={data.top_triggers} k="trigger_code" v="n" />
      <Section title="Failure reasons" rows={data.failures} k="last_error" v="n" />
    </div>
  )
}
