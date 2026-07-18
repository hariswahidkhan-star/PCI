import { useState, useRef } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'

// Admin Console → Integrations (Phase 9 — ERP foundation). Manage outbound connectors (generic signed
// webhook today; ERP connectors in Phase 10), and inspect the event outbox + delivery ledger. Secrets are
// write-only — set here, never shown back.

interface Connector {
  id: number; provider: string; name: string; enabled: number; endpoint_url: string | null
  has_secret: boolean; secret_fields: string[]; config: Record<string, string> | null
  event_filter: string | null; status: string | null; last_delivery_at: string | null
}
interface ProviderInfo { key: string; label: string; description: string }
interface EventRow { id: number; event_type: string; entity_type: string | null; entity_id: number | null; payload: string | null; created_at: string }
interface DeliveryRow { id: number; event_id: number; integration_id: number; integration_name: string | null; event_type: string | null; status: string; attempts: number; response_code: number | null; last_error: string | null; next_attempt_at: string | null; updated_at: string }

const STATUS_TONE: Record<string, 'ok' | 'warn' | 'err' | 'brand' | 'neutral'> = { delivered: 'ok', pending: 'warn', failed: 'err', skipped: 'neutral', ok: 'ok', error: 'err', idle: 'neutral' }
const PROVIDER_LABEL: Record<string, string> = { webhook: 'Generic webhook', quickbooks: 'QuickBooks Online' }
const TABS = ['Connectors', 'Events', 'Deliveries', 'Certuvo'] as const

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
      {tab === 'Certuvo' && <CertuvoTab />}
    </div>
  )
}

// Certuvo external practice-platform integration — auto-provision a member's Certuvo account on membership.
interface CertuvoAccount {
  user_id: number; email: string; is_test?: number | null; status: string; username: string | null; external_id?: string | null
  member_type?: string | null; email_conflict?: number | null; must_change_password?: number | null
  provisioned_at: string | null; activated_at?: string | null; credentials_sent_at?: string | null; last_error: string | null
  retry_count?: number | null; next_retry_at?: string | null; suspended_at?: string | null; revoked_at?: string | null
}
interface CertuvoConfig {
  enabled: boolean; api_base: string; provision_path: string; deactivate_path: string; login_url: string; auth_header: string
  has_api_key: boolean; has_webhook_secret: boolean; requires: string; retry_max: number
  username_prefix?: string; password_length?: number; email_conflict?: string; honorary_grants_membership?: boolean
  accounts: CertuvoAccount[]
}
function CertuvoTab() {
  const { data, loading, error, refetch } = useAdminQuery<CertuvoConfig>('/api/admin/certuvo')
  const [f, setF] = useState<Record<string, string>>({})
  const [enabled, setEnabled] = useState(false)
  const [honorary, setHonorary] = useState(true)
  const [apiKey, setApiKey] = useState('')
  const [webhookSecret, setWebhookSecret] = useState('')
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState<string | null>(null)
  const seeded = useRef(false)
  if (data && !seeded.current) {
    seeded.current = true
    setEnabled(data.enabled)
    setHonorary(data.honorary_grants_membership !== false)
    setF({ api_base: data.api_base, provision_path: data.provision_path, deactivate_path: data.deactivate_path, login_url: data.login_url, auth_header: data.auth_header, requires: data.requires, retry_max: String(data.retry_max), username_prefix: data.username_prefix ?? 'PCI', password_length: String(data.password_length ?? 14), email_conflict: data.email_conflict ?? 'dedicated' })
  }
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const set = (k: string) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })
  async function save() {
    setBusy(true); setNote(null)
    try {
      const body: Record<string, unknown> = { enabled, api_base: f.api_base, provision_path: f.provision_path, deactivate_path: f.deactivate_path, login_url: f.login_url, auth_header: f.auth_header, requires: f.requires, username_prefix: f.username_prefix, email_conflict: f.email_conflict, honorary_grants_membership: honorary }
      if ((f.retry_max ?? '') !== '') body.retry_max = Number(f.retry_max)
      if ((f.password_length ?? '') !== '') body.password_length = Number(f.password_length)
      if (apiKey) body.api_key = apiKey
      if (webhookSecret) body.webhook_secret = webhookSecret
      await adminApi.post('/api/admin/certuvo', body); setApiKey(''); setWebhookSecret(''); setNote('Saved.'); refetch()
    } catch (e) { setNote((e as Error).message) } finally { setBusy(false) }
  }
  async function reprovision(uid: number, reactivate: boolean) {
    setBusy(true); setNote(null)
    try { const r = await adminApi.post<{ ok: boolean; status: string; error: string | null }>(`/api/admin/certuvo/${uid}/provision`, reactivate ? { reactivate: true } : {}); setNote(r.ok ? `Provisioned (${r.status}).` : `Failed: ${r.error || r.status}`); refetch() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(false) }
  }
  async function act(uid: number, action: 'suspend' | 'revoke' | 'resend' | 'regenerate-username' | 'new-password') {
    setBusy(true); setNote(null)
    try {
      const r = await adminApi.post<{ username?: string; ok?: boolean }>(`/api/admin/certuvo/${uid}/${action}`, {})
      setNote(
        action === 'suspend' ? 'Access suspended.'
        : action === 'revoke' ? 'Access revoked.'
        : action === 'resend' ? 'Access instructions re-sent.'
        : action === 'regenerate-username' ? `New username assigned${r.username ? ` (${r.username})` : ''}.`
        : 'New temporary password issued — the student can see it on their Certuvo page.')
      refetch()
    } catch (e) { setNote((e as Error).message) } finally { setBusy(false) }
  }
  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <Card title="Certuvo practice-platform integration">
        <p className="muted small" style={{ marginTop: 0 }}>When a student's membership is settled, PCI provisions them a Certuvo account through Certuvo's API and shares the credentials in the student panel. Set the base URL + API key from your Certuvo onboarding; point <em>API base</em> at a mock to validate first.</p>
        {note && <div className="notice" role="status" style={{ marginBottom: '.6rem' }}>{note}</div>}
        <div style={{ display: 'grid', gap: '.55rem', maxWidth: 640 }}>
          <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={enabled} onChange={(e) => setEnabled(e.target.checked)} style={{ width: 'auto' }} /> Enabled (auto-provision on membership)</label>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>API base<input value={f.api_base ?? ''} onChange={set('api_base')} placeholder="https://api.certuvo.example" /></label>
            <label>Provision path<input value={f.provision_path ?? ''} onChange={set('provision_path')} placeholder="/api/accounts" /></label>
            <label>Deactivation path<input value={f.deactivate_path ?? ''} onChange={set('deactivate_path')} placeholder="/api/accounts/deactivate" /></label>
            <label>Student login URL<input value={f.login_url ?? ''} onChange={set('login_url')} placeholder="https://app.certuvo.example/login" /></label>
            <label>Auth header<input value={f.auth_header ?? ''} onChange={set('auth_header')} placeholder="Authorization" /></label>
            <label>API key <span className="muted small">(write-only{data?.has_api_key ? ' — set' : ''})</span><input type="password" value={apiKey} placeholder={data?.has_api_key ? '•••••• set — blank to keep' : ''} onChange={(e) => setApiKey(e.target.value)} /></label>
            <label>Webhook secret <span className="muted small">(write-only{data?.has_webhook_secret ? ' — set' : ''})</span><input type="password" value={webhookSecret} placeholder={data?.has_webhook_secret ? '•••••• set — blank to keep' : ''} onChange={(e) => setWebhookSecret(e.target.value)} /></label>
            <label>Access rule
              <select value={f.requires ?? 'membership'} onChange={set('requires')}>
                <option value="membership">Active membership</option>
                <option value="membership_and_enrolment">Membership + certification enrolment</option>
              </select>
            </label>
            <label>Max automatic retries<input type="number" min="0" max="50" value={f.retry_max ?? ''} onChange={set('retry_max')} /></label>
            <label>Username prefix <span className="muted small">(PCI-generated, e.g. PCI-2026-000001)</span><input value={f.username_prefix ?? ''} onChange={set('username_prefix')} placeholder="PCI" maxLength={12} /></label>
            <label>Temp-password length<input type="number" min="10" max="64" value={f.password_length ?? ''} onChange={set('password_length')} /></label>
            <label>Email-conflict rule
              <select value={f.email_conflict ?? 'dedicated'} onChange={set('email_conflict')}>
                <option value="dedicated">Dedicated PCI login (proceed on the PCI username)</option>
                <option value="manual">Park for manual review (never overwrite)</option>
              </select>
            </label>
          </div>
          <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={honorary} onChange={(e) => setHonorary(e.target.checked)} style={{ width: 'auto' }} /> Honorary approval grants a full student membership + Certuvo</label>
          <p className="muted small" style={{ margin: 0 }}>PCI generates and owns each Certuvo login — a unique username (never the student's email) and an encrypted temporary password, pushed to Certuvo. An existing Certuvo account under the same email is never overwritten.</p>
          <div className="row"><button className="btn sm" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save'}</button></div>
          <p className="muted small" style={{ margin: 0 }}>Inbound webhook: <code>POST /api/certuvo/webhook</code> with header <code>X-Certuvo-Secret</code>.</p>
        </div>
      </Card>
      <Card title={`Provisioned accounts (${data?.accounts.length ?? 0})`}>
        {(data?.accounts.length ?? 0) === 0 ? <Empty>No Certuvo accounts provisioned yet.</Empty> : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead><tr><th>Member</th><th>Type</th><th>Status</th><th>PCI username</th><th>Provisioned</th><th>Retries</th><th /></tr></thead>
              <tbody>
                {data!.accounts.map((a) => {
                  const suspendedOrRevoked = a.status === 'suspended' || a.status === 'revoked'
                  return (
                    <tr key={a.user_id}>
                      <td className="small">{a.email}{a.is_test ? <> <Badge tone="warn">TEST</Badge></> : null}{a.email_conflict ? <> <Badge tone="warn">EMAIL CONFLICT</Badge></> : null}</td>
                      <td className="small muted">{a.member_type || '—'}</td>
                      <td><Badge tone={a.status === 'active' ? 'ok' : a.status === 'error' || a.status === 'conflict' ? 'err' : 'warn'}>{a.status}</Badge>{a.last_error ? <div className="muted small">{a.last_error}</div> : null}</td>
                      <td className="small">{a.username || '—'}{a.must_change_password ? <div className="muted small">must change on first login</div> : null}</td>
                      <td className="small muted">{a.provisioned_at || '—'}{a.activated_at ? <div className="muted small">1st login {a.activated_at}</div> : null}</td>
                      <td className="small">{a.retry_count ?? 0}{a.next_retry_at ? <div className="muted small">next {a.next_retry_at}</div> : null}</td>
                      <td>
                        <div className="row" style={{ gap: '.3rem', justifyContent: 'flex-end', flexWrap: 'wrap' }}>
                          <button className="btn ghost sm" disabled={busy} onClick={() => reprovision(a.user_id, suspendedOrRevoked)}>Re-provision</button>
                          <button className="btn ghost sm" disabled={busy} onClick={() => act(a.user_id, 'regenerate-username')}>New username</button>
                          <button className="btn ghost sm" disabled={busy} onClick={() => act(a.user_id, 'new-password')}>New password</button>
                          {!suspendedOrRevoked && <button className="btn ghost sm" disabled={busy} onClick={() => act(a.user_id, 'suspend')}>Suspend</button>}
                          {a.status !== 'revoked' && <button className="btn ghost sm danger" disabled={busy} onClick={() => act(a.user_id, 'revoke')}>Revoke</button>}
                          {a.status === 'active' && <button className="btn ghost sm" disabled={busy} onClick={() => act(a.user_id, 'resend')}>Resend</button>}
                        </div>
                      </td>
                    </tr>
                  )
                })}
              </tbody>
            </table>
          </div>
        )}
      </Card>
    </div>
  )
}

function ConnectorsTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Connector[]; events: string[]; providers: ProviderInfo[] }>('/api/admin/integrations')
  const [edit, setEdit] = useState<Connector | 'new' | null>(null)
  const [note, setNote] = useState<string | null>(null)
  const [busy, setBusy] = useState<number | null>(null)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []

  async function toggle(c: Connector) {
    setBusy(c.id); setNote(null)
    // Include provider so enabling/disabling never rewrites the connector type; config/secret are preserved server-side.
    try { await adminApi.post('/api/admin/integrations', { id: c.id, provider: c.provider, name: c.name, endpoint_url: c.endpoint_url, enabled: !c.enabled }); refetch() }
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
          <thead><tr><th>Name</th><th>Destination</th><th>Events</th><th>Secret</th><th>Status</th><th /></tr></thead>
          <tbody>
            {rows.map((c) => (
              <tr key={c.id}>
                <td><strong>{c.name}</strong><div className="muted small">{PROVIDER_LABEL[c.provider] || c.provider}{c.enabled ? '' : ' · disabled'}</div></td>
                <td className="small" style={{ maxWidth: 240, wordBreak: 'break-all' }}>{c.provider === 'quickbooks' ? (c.config?.realm_id ? `company ${c.config.realm_id} (${c.config.environment || 'production'})` : <span className="muted">not configured</span>) : (c.endpoint_url || <span className="muted">—</span>)}</td>
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
      {edit && <ConnectorEditor connector={edit === 'new' ? null : edit} events={data?.events ?? []} providers={data?.providers ?? []}
        onClose={() => setEdit(null)} onSaved={() => { setEdit(null); refetch() }} />}
    </Card>
  )
}

function ConnectorEditor({ connector, events, providers, onClose, onSaved }:
  { connector: Connector | null; events: string[]; providers: ProviderInfo[]; onClose: () => void; onSaved: () => void }) {
  const [provider, setProvider] = useState(connector?.provider ?? 'webhook')
  const [name, setName] = useState(connector?.name ?? '')
  const [endpoint, setEndpoint] = useState(connector?.endpoint_url ?? '')
  const [secret, setSecret] = useState('')
  const [enabled, setEnabled] = useState(!!connector?.enabled)
  const [filter, setFilter] = useState<Set<string>>(new Set(connector?.event_filter ? (JSON.parse(connector.event_filter) as string[]) : []))
  // QuickBooks fields
  const cfg = connector?.config ?? {}
  const [qbo, setQbo] = useState<Record<string, string>>({
    environment: cfg.environment ?? 'production', realm_id: cfg.realm_id ?? '', item_ref: cfg.item_ref ?? '1',
    api_base: cfg.api_base ?? '', client_id: cfg.client_id ?? '', client_secret: '', refresh_token: '', access_token: '',
  })
  const setQ = (k: string) => (e: { target: { value: string } }) => setQbo({ ...qbo, [k]: e.target.value })
  const setSF = new Set(connector?.secret_fields ?? [])
  const [err, setErr] = useState<string | null>(null)
  const [saving, setSaving] = useState(false)
  const toggleEv = (e: string) => { const n = new Set(filter); n.has(e) ? n.delete(e) : n.add(e); setFilter(n) }
  const secretPh = (k: string) => (setSF.has(k) ? '•••••• set — blank to keep' : '')

  async function save() {
    setSaving(true); setErr(null)
    const body: Record<string, unknown> = { provider, name, enabled, event_filter: Array.from(filter) }
    if (connector) body.id = connector.id
    if (provider === 'quickbooks') {
      Object.assign(body, { environment: qbo.environment, realm_id: qbo.realm_id, item_ref: qbo.item_ref, api_base: qbo.api_base, client_id: qbo.client_id })
      // only send secret fields the admin actually typed (write-only, merged server-side)
      for (const k of ['client_secret', 'refresh_token', 'access_token']) if (qbo[k]) body[k] = qbo[k]
    } else {
      body.endpoint_url = endpoint
      if (secret) body.secret = secret
    }
    try { await adminApi.post('/api/admin/integrations', body); onSaved() }
    catch (e) { setErr((e as Error).message) } finally { setSaving(false) }
  }

  return (
    <div style={{ marginTop: '.9rem', borderTop: '1px solid var(--line,#e2e8f0)', paddingTop: '.9rem' }}>
      <h3 style={{ marginBottom: '.5rem' }}>{connector ? 'Edit connector' : 'Add connector'}</h3>
      <div style={{ display: 'grid', gap: '.55rem', maxWidth: 640 }}>
        <label>Type
          <select value={provider} onChange={(e) => setProvider(e.target.value)} disabled={!!connector}>
            {providers.map((p) => <option key={p.key} value={p.key}>{p.label}</option>)}
          </select>
        </label>
        <p className="muted small" style={{ margin: '-.2rem 0 .2rem' }}>{providers.find((p) => p.key === provider)?.description}</p>
        <label>Name<input value={name} onChange={(e) => setName(e.target.value)} placeholder={PROVIDER_LABEL[provider]} /></label>

        {provider === 'webhook' ? (
          <>
            <label>Endpoint URL<input value={endpoint} placeholder="https://your-erp-bridge.example/pci-hook" onChange={(e) => setEndpoint(e.target.value)} /></label>
            <label>Signing secret <span className="muted small">(write-only{connector?.has_secret ? ' — set; blank keeps it' : ''}). Deliveries are HMAC-SHA256 signed in X-PCI-Signature.</span>
              <input type="password" value={secret} placeholder={connector?.has_secret ? '•••••• set — blank to keep' : 'shared secret for signature verification'} onChange={(e) => setSecret(e.target.value)} /></label>
          </>
        ) : (
          <>
            <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
              <label>Environment<select value={qbo.environment} onChange={setQ('environment')}><option value="production">Production</option><option value="sandbox">Sandbox</option></select></label>
              <label>Company (realm) id<input value={qbo.realm_id} onChange={setQ('realm_id')} placeholder="e.g. 4620816365…" /></label>
              <label>OAuth client id<input value={qbo.client_id} onChange={setQ('client_id')} /></label>
              <label>Sales item ref<input value={qbo.item_ref} onChange={setQ('item_ref')} placeholder="QBO Item id (default 1)" /></label>
            </div>
            <p className="muted small" style={{ margin: 0 }}>OAuth secrets are write-only. Provide a refresh token (recommended) so PCI can mint access tokens, or paste a short-lived access token directly.</p>
            <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
              <label>Client secret<input type="password" value={qbo.client_secret} placeholder={secretPh('client_secret')} onChange={setQ('client_secret')} /></label>
              <label>Refresh token<input type="password" value={qbo.refresh_token} placeholder={secretPh('refresh_token')} onChange={setQ('refresh_token')} /></label>
              <label>Access token <span className="muted small">(optional)</span><input type="password" value={qbo.access_token} placeholder={secretPh('access_token')} onChange={setQ('access_token')} /></label>
              <label>API base override <span className="muted small">(optional)</span><input value={qbo.api_base} onChange={setQ('api_base')} placeholder="blank = Intuit host" /></label>
            </div>
          </>
        )}

        <div>
          <div className="muted small" style={{ marginBottom: '.3rem' }}>Events to send (none selected = all){provider === 'quickbooks' ? ' — QuickBooks maps member.registered → Customer, payment.recorded → Sales Receipt' : ''}</div>
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
