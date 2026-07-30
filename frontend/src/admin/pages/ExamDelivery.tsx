import { Fragment, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'

// Admin Console → Exam Delivery. Configure the third-party certification exam-delivery / proctoring
// vendors (Pearson VUE/OnVUE, Kryterion Webassessor, PSI, TestReach, Questionmark), map PCI
// certifications to each vendor's exam codes, drive the scheduling lifecycle for a routed booking, and
// ingest results — issuing the PCI credential on a vendor-graded pass. Secrets are write-only.

interface Provider {
  id: number; provider: string; name: string; label: string; enabled: number; is_default: number
  environment: string; config: Record<string, string> | null; secret_fields: string[]
  status: string | null; last_sync_at: string | null
}
interface ConnectorInfo { key: string; label: string; description: string; config_fields: string[]; secret_fields: string[] }
interface Cert { id: number; code: string; name: string }
interface Order {
  id: number; provider: string; provider_name: string | null; status: string; delivery_type: string
  vendor_exam_code: string | null; confirmation_code: string | null; external_appointment_id: string | null
  result_status: string | null; scheduled_at: string | null; candidate_email: string | null
  first_name: string | null; last_name: string | null; certification_id: number | null; last_error: string | null
}
interface LogRow { id: number; operation: string; ok: number; response_code: number | null; detail: string | null; created_at: string }

const STATUS_TONE: Record<string, 'ok' | 'warn' | 'err' | 'brand' | 'neutral'> = {
  ok: 'ok', error: 'err', idle: 'neutral', pending: 'warn', candidate_created: 'warn', authorized: 'brand',
  scheduled: 'brand', awaiting_candidate_schedule: 'warn', awaiting_results: 'warn', delivered: 'brand',
  completed: 'ok', cancelled: 'neutral', failed: 'err', pass: 'ok', fail: 'err',
}
const label = (k: string) => k.replace(/_/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())
const TABS = ['Providers', 'Bookings'] as const

export default function ExamDelivery() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Providers')
  return (
    <div className="page">
      <PageHeader
        title="Exam Delivery"
        subtitle={<>Deliver PCI examinations through third-party certification vendors — Pearson VUE / OnVUE, Kryterion Webassessor, PSI, TestReach and Questionmark. Configure a vendor, map each certification to its exam code, and student bookings are routed automatically: candidate registration → eligibility/authorization → scheduling → results → credential. Vendor credentials are write-only and every call is logged. Point a vendor at a sandbox (or a mock host via <em>API base override</em>) to validate before going live.</>}
      />
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>)}
      </div>
      {tab === 'Providers' && <ProvidersTab />}
      {tab === 'Bookings' && <BookingsTab />}
    </div>
  )
}

function ProvidersTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Provider[]; connectors: ConnectorInfo[]; certifications: Cert[]; mode: string; cert_modes: Record<string, string> }>('/api/admin/exam-delivery')
  const [edit, setEdit] = useState<Provider | 'new' | null>(null)
  const [note, setNote] = useState<string | null>(null)
  const [busy, setBusy] = useState<number | null>(null)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []
  const connectors = data?.connectors ?? []

  async function test(p: Provider) {
    setBusy(p.id); setNote(null)
    try {
      const r = await adminApi.post<{ ok: boolean; code: number | null; detail: string | null }>(`/api/admin/exam-delivery/${p.id}/test`, {})
      setNote(r.ok ? `“${p.name}” connection OK.` : `“${p.name}”: ${r.detail || 'connection failed'}`)
      refetch()
    } catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }
  async function toggle(p: Provider) {
    setBusy(p.id); setNote(null)
    try { await adminApi.post('/api/admin/exam-delivery', { id: p.id, provider: p.provider, name: p.name, environment: p.environment, is_default: p.is_default, enabled: !p.enabled }); refetch() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }
  async function del(p: Provider) {
    if (!confirm(`Delete the ${p.label} provider configuration?`)) return
    setBusy(p.id); setNote(null)
    try { await adminApi.post(`/api/admin/exam-delivery/${p.id}/delete`); refetch() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }

  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
    <DeliveryModeCard mode={data?.mode ?? 'in_house'} certModes={data?.cert_modes ?? {}} providers={rows} certs={data?.certifications ?? []} onSaved={refetch} />
    <Card title={`Vendors (${rows.length})`} action={<button className="btn sm" onClick={() => setEdit('new')}>Add vendor</button>}>
      {note && <div className="notice" role="status" style={{ marginBottom: '.6rem' }}>{note}</div>}
      {rows.length === 0 ? <Empty>No exam-delivery vendors configured. Add one to route bookings to Pearson VUE, Kryterion, PSI, TestReach or Questionmark.</Empty> : (
        <table className="data">
          <thead><tr><th>Vendor</th><th>Environment</th><th>Mapped exams</th><th>Status</th><th /></tr></thead>
          <tbody>
            {rows.map((p) => {
              const mapped = p.config?.exam_map ? Object.keys(p.config.exam_map as unknown as object).length : 0
              return (
                <tr key={p.id}>
                  <td><strong>{p.name}</strong> {p.is_default ? <Badge tone="brand">default</Badge> : null}<div className="muted small">{p.label}{p.enabled ? '' : ' · disabled'}</div></td>
                  <td className="small">{p.environment}</td>
                  <td className="small muted">{mapped} certification{mapped === 1 ? '' : 's'}</td>
                  <td>{p.enabled ? <Badge tone={STATUS_TONE[p.status || 'idle'] ?? 'neutral'}>{p.status || 'idle'}</Badge> : <Badge tone="neutral">off</Badge>}</td>
                  <td className="row" style={{ gap: '.3rem', justifyContent: 'flex-end' }}>
                    <button className="btn ghost sm" disabled={busy === p.id} onClick={() => test(p)}>Test</button>
                    <button className="btn ghost sm" disabled={busy === p.id} onClick={() => toggle(p)}>{p.enabled ? 'Disable' : 'Enable'}</button>
                    <button className="btn ghost sm" onClick={() => setEdit(p)}>Edit</button>
                    <button className="btn ghost sm danger" disabled={busy === p.id} onClick={() => del(p)}>Delete</button>
                  </td>
                </tr>
              )
            })}
          </tbody>
        </table>
      )}
      {edit && <ProviderEditor provider={edit === 'new' ? null : edit} connectors={connectors} certs={data?.certifications ?? []}
        onClose={() => setEdit(null)} onSaved={() => { setEdit(null); refetch() }} />}
    </Card>
    </div>
  )
}

// The delivery-mode switch — the control the operator uses to deliver each certification's exam through
// PCI's own SecureExam software OR an enabled vendor. A per-certification choice overrides the global one.
function DeliveryModeCard({ mode, certModes, providers, certs, onSaved }:
  { mode: string; certModes: Record<string, string>; providers: Provider[]; certs: Cert[]; onSaved: () => void }) {
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState<string | null>(null)
  const enabled = providers.filter((p) => p.enabled)
  const optionsFor = (p: Provider) => p.name || p.label || p.provider
  const globalOpts = [{ v: 'in_house', l: 'Our SecureExam software (in-house)' }, ...enabled.map((p) => ({ v: p.provider, l: optionsFor(p) }))]
  const disabled = enabled.length === 0

  async function setGlobal(v: string) {
    setBusy(true); setNote(null)
    try { await adminApi.post('/api/admin/exam-delivery/mode', { mode: v }); setNote(`Default delivery: ${globalOpts.find((o) => o.v === v)?.l ?? v}.`); onSaved() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(false) }
  }
  async function setCert(id: number, v: string) {
    setBusy(true); setNote(null)
    try { await adminApi.post('/api/admin/exam-delivery/mode', { certification_id: id, cert_mode: v }); onSaved() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(false) }
  }

  return (
    <Card title="Delivery mode — SecureExam vs vendor">
      <p className="muted small" style={{ marginTop: 0 }}>Choose how examinations are delivered. Bookings for a certification route automatically to the selected channel. Only enabled, configured vendors appear as options.{disabled ? ' Add and enable a vendor above to unlock vendor delivery.' : ''}</p>
      {note && <div className="notice" role="status" style={{ marginBottom: '.6rem' }}>{note}</div>}
      <label className="row" style={{ gap: '.6rem', alignItems: 'center', flexWrap: 'wrap' }}>
        <strong style={{ minWidth: 130 }}>Default for all</strong>
        <select value={mode} disabled={busy} onChange={(e) => setGlobal(e.target.value)} style={{ maxWidth: 340 }}>
          {globalOpts.map((o) => <option key={o.v} value={o.v}>{o.l}</option>)}
        </select>
        <Badge tone={mode === 'in_house' ? 'brand' : 'ok'}>{mode === 'in_house' ? 'In-house' : 'Vendor'}</Badge>
      </label>
      {certs.length > 0 && (
        <div style={{ marginTop: '.8rem' }}>
          <div className="muted small" style={{ marginBottom: '.35rem' }}>Per-certification override</div>
          <div style={{ display: 'grid', gap: '.4rem' }}>
            {certs.map((c) => {
              const cm = certModes[String(c.id)] ?? 'inherit'
              return (
                <label key={c.id} className="row" style={{ gap: '.6rem', alignItems: 'center' }}>
                  <span className="small" style={{ minWidth: 200 }}>{c.name} <span className="muted">({c.code})</span></span>
                  <select value={cm} disabled={busy} onChange={(e) => setCert(c.id, e.target.value)} style={{ maxWidth: 340 }}>
                    <option value="inherit">Use default ({globalOpts.find((o) => o.v === mode)?.l ?? mode})</option>
                    <option value="in_house">Our SecureExam software (in-house)</option>
                    {enabled.map((p) => <option key={p.provider} value={p.provider}>{optionsFor(p)}</option>)}
                  </select>
                </label>
              )
            })}
          </div>
        </div>
      )}
    </Card>
  )
}

function ProviderEditor({ provider, connectors, certs, onClose, onSaved }:
  { provider: Provider | null; connectors: ConnectorInfo[]; certs: Cert[]; onClose: () => void; onSaved: () => void }) {
  const [slug, setSlug] = useState(provider?.provider ?? connectors[0]?.key ?? '')
  const info = connectors.find((c) => c.key === slug)
  const cfg = provider?.config ?? {}
  const [name, setName] = useState(provider?.name ?? '')
  const [environment, setEnvironment] = useState(provider?.environment ?? 'sandbox')
  const [enabled, setEnabled] = useState(!!provider?.enabled)
  const [isDefault, setIsDefault] = useState(!!provider?.is_default)
  const [config, setConfig] = useState<Record<string, string>>({ ...(cfg as Record<string, string>) })
  const [secrets, setSecrets] = useState<Record<string, string>>({})
  const [callbackSecret, setCallbackSecret] = useState('')
  const [examMap, setExamMap] = useState<Record<string, string>>(() => {
    const m = (cfg.exam_map as unknown as Record<string, string>) || {}
    return { ...m }
  })
  const setSF = new Set(provider?.secret_fields ?? [])
  const [err, setErr] = useState<string | null>(null)
  const [saving, setSaving] = useState(false)

  const configFields = (info?.config_fields ?? []).filter((f) => f !== 'default_exam_code')
  const secretFields = info?.secret_fields ?? []

  async function save() {
    setSaving(true); setErr(null)
    const body: Record<string, unknown> = { provider: slug, name, environment, enabled, is_default: isDefault }
    if (provider) body.id = provider.id
    for (const f of configFields) if (config[f] != null && config[f] !== '') body[f] = config[f]
    if (config.default_exam_code) body.default_exam_code = config.default_exam_code
    // exam map: certification code (or id) → vendor exam code. Keep only filled rows.
    const map: Record<string, string> = {}
    for (const [k, v] of Object.entries(examMap)) if (v && v.trim()) map[k] = v.trim()
    body.exam_map = map
    if (callbackSecret) body.callback_secret = callbackSecret
    for (const k of secretFields) if (secrets[k]) body[k] = secrets[k]
    try { await adminApi.post('/api/admin/exam-delivery', body); onSaved() }
    catch (e) { setErr((e as Error).message) } finally { setSaving(false) }
  }

  return (
    <div style={{ marginTop: '.9rem', borderTop: '1px solid var(--line,#e2e8f0)', paddingTop: '.9rem' }}>
      <h3 style={{ marginBottom: '.5rem' }}>{provider ? `Edit ${provider.label}` : 'Add vendor'}</h3>
      <div style={{ display: 'grid', gap: '.55rem', maxWidth: 680 }}>
        <label>Vendor
          <select value={slug} onChange={(e) => setSlug(e.target.value)} disabled={!!provider}>
            {connectors.map((c) => <option key={c.key} value={c.key}>{c.label}</option>)}
          </select>
        </label>
        <p className="muted small" style={{ margin: '-.2rem 0 .2rem' }}>{info?.description}</p>
        <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
          <label>Name<input value={name} onChange={(e) => setName(e.target.value)} placeholder={info?.label} /></label>
          <label>Environment<select value={environment} onChange={(e) => setEnvironment(e.target.value)}><option value="sandbox">Sandbox</option><option value="production">Production</option></select></label>
        </div>

        {configFields.length > 0 && (
          <fieldset style={{ border: '1px solid var(--line,#e2e8f0)', borderRadius: 8, padding: '.6rem .7rem' }}>
            <legend className="small muted">Configuration</legend>
            <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
              {configFields.map((f) => (
                <label key={f}>{label(f)}{f === 'api_base' ? <span className="muted small"> (blank = vendor host; set to a mock to test)</span> : null}
                  <input value={config[f] ?? ''} onChange={(e) => setConfig({ ...config, [f]: e.target.value })} />
                </label>
              ))}
            </div>
          </fieldset>
        )}

        <fieldset style={{ border: '1px solid var(--line,#e2e8f0)', borderRadius: 8, padding: '.6rem .7rem' }}>
          <legend className="small muted">Credentials <span className="muted">(write-only — blank keeps the stored value)</span></legend>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            {secretFields.map((k) => (
              <label key={k}>{label(k)}
                <input type="password" value={secrets[k] ?? ''} placeholder={setSF.has(k) ? '•••••• set — blank to keep' : ''} onChange={(e) => setSecrets({ ...secrets, [k]: e.target.value })} />
              </label>
            ))}
            <label>Callback token <span className="muted small">(verifies inbound result callbacks)</span>
              <input type="password" value={callbackSecret} placeholder={cfg.callback_secret ? '•••••• set — blank to keep' : 'shared secret for /api/exam-delivery/callback'} onChange={(e) => setCallbackSecret(e.target.value)} />
            </label>
          </div>
        </fieldset>

        <fieldset style={{ border: '1px solid var(--line,#e2e8f0)', borderRadius: 8, padding: '.6rem .7rem' }}>
          <legend className="small muted">Certification → vendor exam code</legend>
          <div style={{ display: 'grid', gap: '.4rem' }}>
            {certs.map((c) => (
              <label key={c.id} className="row" style={{ gap: '.5rem', alignItems: 'center' }}>
                <span className="small" style={{ minWidth: 200 }}>{c.name} <span className="muted">({c.code})</span></span>
                <input value={examMap[c.code] ?? examMap[String(c.id)] ?? ''} placeholder="vendor exam / assessment code"
                  onChange={(e) => setExamMap({ ...examMap, [c.code]: e.target.value })} />
              </label>
            ))}
            {certs.length === 0 && <p className="muted small" style={{ margin: 0 }}>No active certifications to map.</p>}
          </div>
        </fieldset>

        <div className="row" style={{ gap: '1rem', flexWrap: 'wrap' }}>
          <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={enabled} onChange={(e) => setEnabled(e.target.checked)} style={{ width: 'auto' }} /> Enabled</label>
          <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={isDefault} onChange={(e) => setIsDefault(e.target.checked)} style={{ width: 'auto' }} /> Default — new bookings route here</label>
        </div>
      </div>
      {err && <div className="notice err" style={{ marginTop: '.5rem' }}>{err}</div>}
      <div className="row" style={{ gap: '.5rem', marginTop: '.7rem' }}>
        <button className="btn" disabled={saving} onClick={save}>{saving ? 'Saving…' : 'Save'}</button>
        <button className="btn ghost" onClick={onClose}>Cancel</button>
      </div>
    </div>
  )
}

function BookingsTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Order[] }>('/api/admin/exam-delivery/orders')
  const [busy, setBusy] = useState<number | null>(null)
  const [note, setNote] = useState<string | null>(null)
  const [logFor, setLogFor] = useState<number | null>(null)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []

  async function act(o: Order, action: 'provision' | 'sync' | 'cancel') {
    setBusy(o.id); setNote(null)
    try {
      const r = await adminApi.post<{ ok: boolean; status?: string; result_status?: string; credential?: string | null; detail?: string }>(`/api/admin/exam-delivery/orders/${o.id}/${action}`, {})
      setNote(r.credential ? `Order #${o.id}: pass — credential ${r.credential} issued.` : `Order #${o.id}: ${r.status || r.result_status || (r.ok ? 'done' : 'failed')}${r.detail ? ' — ' + r.detail : ''}`)
      refetch()
    } catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }

  return (
    <Card title={`Routed bookings (${rows.length})`}>
      {note && <div className="notice" role="status" style={{ marginBottom: '.6rem' }}>{note}</div>}
      {rows.length === 0 ? <Empty>No bookings have been routed to a vendor yet. Set a vendor as default and map the certification; new student bookings appear here.</Empty> : (
        <table className="data">
          <thead><tr><th>#</th><th>Candidate</th><th>Vendor</th><th>Exam</th><th>Status</th><th>Result</th><th>Confirmation</th><th /></tr></thead>
          <tbody>
            {rows.map((o) => (
              <Fragment key={o.id}>
                <tr>
                  <td className="small muted">{o.id}</td>
                  <td className="small">{o.first_name || o.last_name ? `${o.first_name ?? ''} ${o.last_name ?? ''}`.trim() : o.candidate_email}<div className="muted small">{o.candidate_email}</div></td>
                  <td className="small">{o.provider_name || o.provider}</td>
                  <td className="small muted">{o.vendor_exam_code || '—'}</td>
                  <td><Badge tone={STATUS_TONE[o.status] ?? 'neutral'}>{o.status.replace(/_/g, ' ')}</Badge></td>
                  <td>{o.result_status ? <Badge tone={STATUS_TONE[o.result_status] ?? 'neutral'}>{o.result_status}</Badge> : <span className="muted small">—</span>}</td>
                  <td className="small muted" style={{ maxWidth: 160, wordBreak: 'break-all' }}>{o.confirmation_code || o.external_appointment_id || '—'}</td>
                  <td className="row" style={{ gap: '.3rem', justifyContent: 'flex-end' }}>
                    <button className="btn ghost sm" disabled={busy === o.id} onClick={() => act(o, 'provision')} title="Run candidate → authorize → schedule">Provision</button>
                    <button className="btn ghost sm" disabled={busy === o.id} onClick={() => act(o, 'sync')} title="Pull status + results; issue credential on a pass">Sync</button>
                    <button className="btn ghost sm" onClick={() => setLogFor(logFor === o.id ? null : o.id)}>Log</button>
                    <button className="btn ghost sm danger" disabled={busy === o.id} onClick={() => act(o, 'cancel')}>Cancel</button>
                  </td>
                </tr>
                {logFor === o.id && <tr><td colSpan={8} style={{ background: 'var(--paper-2,#f8fafc)' }}><OrderLog id={o.id} /></td></tr>}
              </Fragment>
            ))}
          </tbody>
        </table>
      )}
    </Card>
  )
}

function OrderLog({ id }: { id: number }) {
  const { data, loading, error } = useAdminQuery<{ rows: LogRow[] }>(`/api/admin/exam-delivery/orders/${id}/log`)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []
  if (rows.length === 0) return <div className="small muted" style={{ padding: '.5rem' }}>No API calls logged yet.</div>
  return (
    <table className="data" style={{ margin: '.3rem 0' }}>
      <thead><tr><th>When</th><th>Operation</th><th>Result</th><th>Detail</th></tr></thead>
      <tbody>
        {rows.map((l) => (
          <tr key={l.id}>
            <td className="small muted">{l.created_at}</td>
            <td className="small">{l.operation}</td>
            <td>{l.ok ? <Badge tone="ok">ok</Badge> : <Badge tone="err">fail</Badge>}{l.response_code ? <span className="muted small"> {l.response_code}</span> : null}</td>
            <td className="small muted" style={{ maxWidth: 420, wordBreak: 'break-word' }}>{l.detail}</td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
