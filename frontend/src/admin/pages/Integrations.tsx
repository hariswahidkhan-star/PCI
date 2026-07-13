import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'

// Admin Console → Integrations (Phase 9 — ERP foundation). Manage outbound connectors (generic signed
// webhook today; ERP connectors in Phase 10), and inspect the event outbox + delivery ledger. Secrets are
// write-only — set here, never shown back.

interface Connector {
  id: number; provider: string; name: string; enabled: number; endpoint_url: string | null
  has_secret: boolean; event_filter: string | null; status: string | null; last_delivery_at: string | null
}
interface EventRow { id: number; event_type: string; entity_type: string | null; entity_id: number | null; payload: string | null; created_at: string }
interface DeliveryRow { id: number; event_id: number; integration_id: number; integration_name: string | null; event_type: string | null; status: string; attempts: number; response_code: number | null; last_error: string | null; next_attempt_at: string | null; updated_at: string }

const STATUS_TONE: Record<string, 'ok' | 'warn' | 'err' | 'brand' | 'neutral'> = { delivered: 'ok', pending: 'warn', failed: 'err', ok: 'ok', error: 'err', idle: 'neutral' }
const TABS = ['Connectors', 'Events', 'Deliveries'] as const

export default function Integrations() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Connectors')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Integrations</h1>
        <p className="muted">Push PCI business events (payments, memberships, new members) to an ERP, accounting system, or automation bridge. The generic webhook connector delivers signed JSON to any HTTPS endpoint; deliveries are queued, retried and logged. Dedicated ERP connectors build on this same pipeline.</p>
      </div>
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>)}
      </div>
      {tab === 'Connectors' && <ConnectorsTab />}
      {tab === 'Events' && <EventsTab />}
      {tab === 'Deliveries' && <DeliveriesTab />}
    </div>
  )
}

function ConnectorsTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Connector[]; events: string[] }>('/api/admin/integrations')
  const [edit, setEdit] = useState<Connector | 'new' | null>(null)
  const [note, setNote] = useState<string | null>(null)
  const [busy, setBusy] = useState<number | null>(null)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []

  async function toggle(c: Connector) {
    setBusy(c.id); setNote(null)
    try { await adminApi.post('/api/admin/integrations', { id: c.id, name: c.name, endpoint_url: c.endpoint_url, enabled: !c.enabled }); refetch() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }
  async function test(c: Connector) {
    setBusy(c.id); setNote(null)
    try {
      const r = await adminApi.post<{ ok: boolean; status: string; response_code: number | null; error: string | null }>(`/api/admin/integrations/${c.id}/test`, {})
      setNote(r.ok ? `Test delivered to “${c.name}” (HTTP ${r.response_code}).` : `Test to “${c.name}”: ${r.status}${r.error ? ' — ' + r.error : ''}`)
      refetch()
    } catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }
  async function del(c: Connector) {
    if (!confirm(`Delete connector “${c.name}” and its delivery history?`)) return
    setBusy(c.id); setNote(null)
    try { await adminApi.post(`/api/admin/integrations/${c.id}/delete`); refetch() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }

  return (
    <Card title={`Connectors (${rows.length})`} action={<button className="btn sm" onClick={() => setEdit('new')}>Add connector</button>}>
      {note && <div className="notice" role="status" style={{ marginBottom: '.6rem' }}>{note}</div>}
      {rows.length === 0 ? <Empty>No connectors yet. Add a webhook to start syncing events.</Empty> : (
        <table className="data">
          <thead><tr><th>Name</th><th>Endpoint</th><th>Events</th><th>Secret</th><th>Status</th><th /></tr></thead>
          <tbody>
            {rows.map((c) => (
              <tr key={c.id}>
                <td><strong>{c.name}</strong><div className="muted small">{c.provider}{c.enabled ? '' : ' · disabled'}</div></td>
                <td className="small" style={{ maxWidth: 240, wordBreak: 'break-all' }}>{c.endpoint_url || <span className="muted">—</span>}</td>
                <td className="small muted">{c.event_filter ? (JSON.parse(c.event_filter) as string[]).length + ' selected' : 'all'}</td>
                <td>{c.has_secret ? <Badge tone="ok">signed</Badge> : <Badge tone="warn">unsigned</Badge>}</td>
                <td>{c.enabled ? <Badge tone={STATUS_TONE[c.status || 'idle'] ?? 'neutral'}>{c.status || 'idle'}</Badge> : <Badge tone="neutral">off</Badge>}</td>
                <td className="row" style={{ gap: '.3rem', justifyContent: 'flex-end' }}>
                  <button className="btn ghost sm" disabled={busy === c.id} onClick={() => test(c)}>Test</button>
                  <button className="btn ghost sm" disabled={busy === c.id} onClick={() => toggle(c)}>{c.enabled ? 'Disable' : 'Enable'}</button>
                  <button className="btn ghost sm" onClick={() => setEdit(c)}>Edit</button>
                  <button className="btn ghost sm danger" disabled={busy === c.id} onClick={() => del(c)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {edit && <ConnectorEditor connector={edit === 'new' ? null : edit} events={data?.events ?? []}
        onClose={() => setEdit(null)} onSaved={() => { setEdit(null); refetch() }} />}
    </Card>
  )
}

function ConnectorEditor({ connector, events, onClose, onSaved }:
  { connector: Connector | null; events: string[]; onClose: () => void; onSaved: () => void }) {
  const [name, setName] = useState(connector?.name ?? 'Webhook')
  const [endpoint, setEndpoint] = useState(connector?.endpoint_url ?? '')
  const [secret, setSecret] = useState('')
  const [enabled, setEnabled] = useState(!!connector?.enabled)
  const [filter, setFilter] = useState<Set<string>>(new Set(connector?.event_filter ? (JSON.parse(connector.event_filter) as string[]) : []))
  const [err, setErr] = useState<string | null>(null)
  const [saving, setSaving] = useState(false)
  const toggleEv = (e: string) => { const n = new Set(filter); n.has(e) ? n.delete(e) : n.add(e); setFilter(n) }
  async function save() {
    setSaving(true); setErr(null)
    const body: Record<string, unknown> = { name, endpoint_url: endpoint, enabled, event_filter: Array.from(filter) }
    if (connector) body.id = connector.id
    if (secret) body.secret = secret            // only send the secret when the admin typed one
    try { await adminApi.post('/api/admin/integrations', body); onSaved() }
    catch (e) { setErr((e as Error).message) } finally { setSaving(false) }
  }
  return (
    <div style={{ marginTop: '.9rem', borderTop: '1px solid var(--line,#e2e8f0)', paddingTop: '.9rem' }}>
      <h3 style={{ marginBottom: '.5rem' }}>{connector ? 'Edit connector' : 'Add webhook connector'}</h3>
      <div style={{ display: 'grid', gap: '.55rem', maxWidth: 640 }}>
        <label>Name<input value={name} onChange={(e) => setName(e.target.value)} /></label>
        <label>Endpoint URL<input value={endpoint} placeholder="https://your-erp-bridge.example/pci-hook" onChange={(e) => setEndpoint(e.target.value)} /></label>
        <label>Signing secret <span className="muted small">(write-only{connector?.has_secret ? ' — a secret is set; blank keeps it' : ''}). Deliveries are signed HMAC-SHA256 in the X-PCI-Signature header.</span>
          <input type="password" value={secret} placeholder={connector?.has_secret ? '•••••• set — blank to keep' : 'shared secret for signature verification'} onChange={(e) => setSecret(e.target.value)} /></label>
        <div>
          <div className="muted small" style={{ marginBottom: '.3rem' }}>Events to send (none selected = all)</div>
          <div className="row" style={{ gap: '.8rem', flexWrap: 'wrap' }}>
            {events.map((ev) => (
              <label key={ev} className="row" style={{ gap: '.35rem' }}>
                <input type="checkbox" checked={filter.has(ev)} onChange={() => toggleEv(ev)} style={{ width: 'auto' }} /> <span className="small">{ev}</span>
              </label>
            ))}
          </div>
        </div>
        <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={enabled} onChange={(e) => setEnabled(e.target.checked)} style={{ width: 'auto' }} /> Enabled (receives events)</label>
      </div>
      {err && <div className="notice err" style={{ marginTop: '.5rem' }}>{err}</div>}
      <div className="row" style={{ gap: '.5rem', marginTop: '.7rem' }}>
        <button className="btn" disabled={saving} onClick={save}>{saving ? 'Saving…' : 'Save'}</button>
        <button className="btn ghost" onClick={onClose}>Cancel</button>
      </div>
    </div>
  )
}

function EventsTab() {
  const { data, loading, error } = useAdminQuery<{ rows: EventRow[] }>('/api/admin/integrations/events')
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []
  return (
    <Card title={`Event outbox (${rows.length})`}>
      {rows.length === 0 ? <Empty>No events recorded yet.</Empty> : (
        <table className="data">
          <thead><tr><th>When</th><th>Event</th><th>Entity</th><th>Payload</th></tr></thead>
          <tbody>
            {rows.map((e) => (
              <tr key={e.id}>
                <td className="small muted">{e.created_at}</td>
                <td><Badge tone="brand">{e.event_type}</Badge></td>
                <td className="small">{e.entity_type}{e.entity_id ? ` #${e.entity_id}` : ''}</td>
                <td className="small muted" style={{ maxWidth: 360, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>{e.payload}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </Card>
  )
}

function DeliveriesTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: DeliveryRow[] }>('/api/admin/integrations/deliveries')
  const [busy, setBusy] = useState<number | null>(null)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []
  async function retry(id: number) {
    setBusy(id)
    try { await adminApi.post(`/api/admin/integrations/deliveries/${id}/retry`); refetch() } finally { setBusy(null) }
  }
  return (
    <Card title={`Delivery ledger (${rows.length})`}>
      {rows.length === 0 ? <Empty>No deliveries yet. Enable a connector, then a business event (or a Test) will appear here.</Empty> : (
        <table className="data">
          <thead><tr><th>When</th><th>Connector</th><th>Event</th><th>Status</th><th>Attempts</th><th>Detail</th><th /></tr></thead>
          <tbody>
            {rows.map((d) => (
              <tr key={d.id}>
                <td className="small muted">{d.updated_at}</td>
                <td className="small">{d.integration_name || `#${d.integration_id}`}</td>
                <td className="small">{d.event_type}</td>
                <td><Badge tone={STATUS_TONE[d.status] ?? 'neutral'}>{d.status}</Badge></td>
                <td className="small">{d.attempts}</td>
                <td className="small muted" style={{ maxWidth: 240, wordBreak: 'break-all' }}>{d.response_code ? `HTTP ${d.response_code}` : ''}{d.last_error ? ` ${d.last_error}` : ''}</td>
                <td>{d.status !== 'delivered' && <button className="btn ghost sm" disabled={busy === d.id} onClick={() => retry(d.id)}>Retry now</button>}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </Card>
  )
}
