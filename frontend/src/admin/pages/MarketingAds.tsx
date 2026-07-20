import { useEffect, useState } from 'react'
import { adminApi } from '../api'
import { Card, Badge, Spinner, Empty, Stat } from '../../components/ui'
import { fmtDateTime } from '../../format'

/* Marketing, Ads and Search Console centre. All data is MySQL-backed via the .NET backend. The guiding
 * principle is HONESTY: nothing is presented as operational until a real, provider-approved connection
 * exists. Capability statuses reflect true access; provider secrets are never shown (booleans only);
 * OAuth tokens never reach the browser. Permissions are enforced by the backend, not just these tabs. */

type Row = Record<string, any>
const TABS = [
  'Dashboard', 'Capability Registry', 'Connected Accounts', 'LinkedIn Posts', 'Manual Outreach',
  'Campaigns', 'Promotions', 'Lead Centre', 'Audiences', 'Creatives', 'Landing Pages',
  'Conversions', 'Search Console', 'Alerts', 'Audit',
] as const
type Tab = typeof TABS[number]

// Capability status → human label + tone. This is the honesty vocabulary from the spec.
const CAP: Record<string, { label: string; tone: 'ok' | 'warn' | 'err' | 'brand' | 'neutral' }> = {
  available: { label: 'Available', tone: 'ok' },
  available_with_permission: { label: 'Available with permission', tone: 'ok' },
  provider_approval_required: { label: 'Provider approval required', tone: 'warn' },
  account_upgrade_required: { label: 'Account upgrade required', tone: 'warn' },
  read_only: { label: 'Read only', tone: 'brand' },
  manual_workflow_only: { label: 'Manual workflow only', tone: 'brand' },
  temporarily_unavailable: { label: 'Temporarily unavailable', tone: 'err' },
  deprecated: { label: 'Deprecated', tone: 'err' },
  unsupported: { label: 'Unsupported', tone: 'err' },
  not_connected: { label: 'Not connected', tone: 'neutral' },
}
const capBadge = (s: string) => { const c = CAP[s] ?? { label: s, tone: 'neutral' as const }; return <Badge tone={c.tone}>{c.label}</Badge> }

export default function MarketingAds() {
  const [tab, setTab] = useState<Tab>('Dashboard')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread"><h1>Marketing, Ads and Search Console</h1></div>
      <p className="muted" style={{ margin: 0 }}>
        Manage LinkedIn, Google and Meta advertising, organic LinkedIn posts, Google Search Console,
        promotions, leads and reporting — through official APIs only. Features become operational only
        when a real, provider-approved connection exists; the Capability Registry shows the true status of
        each one. Provider credentials live only in Render environment variables and are never shown here.
      </p>
      <div className="tabs" style={{ display: 'flex', gap: '.3rem', flexWrap: 'wrap', borderBottom: '1px solid var(--line)', paddingBottom: '.4rem' }}>
        {TABS.map((t) => <button key={t} className={'btn sm ' + (tab === t ? '' : 'ghost')} onClick={() => setTab(t)}>{t}</button>)}
      </div>
      {tab === 'Dashboard' && <Dashboard />}
      {tab === 'Capability Registry' && <Capabilities />}
      {tab === 'Connected Accounts' && <Connections />}
      {tab === 'LinkedIn Posts' && <Posts />}
      {tab === 'Manual Outreach' && <Outreach />}
      {tab === 'Campaigns' && <Campaigns />}
      {tab === 'Promotions' && <Promotions />}
      {tab === 'Lead Centre' && <Leads />}
      {tab === 'Audiences' && <SimpleList title="Audiences" path="audiences" fields={[['name', 'Name', true], ['platform_code', 'Platform'], ['purpose', 'Purpose'], ['countries', 'Countries'], ['professional_criteria', 'Professional criteria']]} note="Do not upload identity documents, exam scores or other sensitive certification data to advertising platforms." />}
      {tab === 'Creatives' && <SimpleList title="Creative Library" path="creatives" fields={[['name', 'Name', true], ['format', 'Format'], ['headline', 'Headline'], ['primary_text', 'Primary text'], ['description', 'Description'], ['cta', 'Call to action'], ['destination_url', 'Destination URL'], ['platform_scope', 'Platform scope']]} note="Approved assets can be reused across LinkedIn, Google and Meta where technically and contractually permitted. Avoid unsupported claims." />}
      {tab === 'Landing Pages' && <SimpleList title="Landing Pages" path="landing-pages" fields={[['name', 'Name', true], ['url', 'URL'], ['headline', 'Headline'], ['description', 'Description'], ['cta', 'Call to action'], ['application_link', 'Application link']]} note="Validate fees, certification and claims before linking a landing page to a live campaign." />}
      {tab === 'Conversions' && <SimpleList title="Conversion Actions" path="conversions" fields={[['name', 'Name', true], ['platform_code', 'Platform'], ['business_event', 'Business event'], ['value', 'Value'], ['currency', 'Currency']]} note="Do not transmit unnecessary sensitive personal information. Attribution is never perfect where consent or browser limits apply." />}
      {tab === 'Search Console' && <SearchConsole />}
      {tab === 'Alerts' && <Alerts />}
      {tab === 'Audit' && <Audit />}
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

// ───────── Dashboard ─────────
function Dashboard() {
  const { data } = useGet<Row>('/api/admin/marketing/overview')
  const { data: prov } = useGet<Row>('/api/admin/marketing/providers')
  const c = data?.counts ?? {}
  const Cfg = ({ on }: { on: boolean }) => <Badge tone={on ? 'ok' : 'neutral'}>{on ? 'Configured' : 'Not configured'}</Badge>
  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <Card title="Overview">
        <div className="stat-grid" style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit,minmax(120px,1fr))', gap: '.6rem' }}>
          <Stat n={c.connections ?? 0} k="Connected" />
          <Stat n={c.campaigns ?? 0} k="Campaigns" />
          <Stat n={c.posts ?? 0} k="LinkedIn posts" />
          <Stat n={c.promotions ?? 0} k="Active promos" />
          <Stat n={c.leads ?? 0} k="Leads" />
          <Stat n={c.open_alerts ?? 0} k="Open alerts" />
        </div>
      </Card>
      <Card title="Provider configuration (Render environment)">
        <p className="muted small" style={{ marginTop: 0 }}>Secrets are stored only in Render environment variables. This shows whether each provider app is configured — never the secret value.</p>
        {prov ? (
          <table className="data"><tbody>
            <tr><td>Token encryption key (dedicated)</td><td><Cfg on={!!prov.token_encryption} /></td></tr>
            <tr><td>LinkedIn OAuth app</td><td><Cfg on={!!prov.linkedin?.configured} /></td></tr>
            <tr><td>LinkedIn webhook secret</td><td><Cfg on={!!prov.linkedin?.webhook_secret} /></td></tr>
            <tr><td>Google OAuth app</td><td><Cfg on={!!prov.google?.configured} /></td></tr>
            <tr><td>Google Ads developer token</td><td><Cfg on={!!prov.google?.ads_developer_token} /></td></tr>
            <tr><td>Meta app</td><td><Cfg on={!!prov.meta?.configured} /></td></tr>
            <tr><td>Meta webhook secret</td><td><Cfg on={!!prov.meta?.webhook_secret} /></td></tr>
          </tbody></table>
        ) : <Spinner />}
      </Card>
    </div>
  )
}

// ───────── Capability Registry (the honesty layer) ─────────
function Capabilities() {
  const { data, loading } = useGet<{ rows: Row[] }>('/api/admin/marketing/capabilities')
  return (
    <Card title="Capability Registry">
      <p className="muted small" style={{ marginTop: 0 }}>What each platform currently permits. A feature is not operational merely because its screen exists — the effective status reflects real, provider-approved access on a live connection.</p>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No capabilities seeded.</Empty> : (
        <div style={{ overflowX: 'auto' }}>
          <table className="data"><thead><tr><th>Platform</th><th>Feature</th><th>Status</th><th>Required</th><th>Limitation / operator action</th></tr></thead>
            <tbody>{data!.rows.map((r) => (
              <tr key={r.id}>
                <td className="small">{r.platform_name || r.platform_code}</td>
                <td className="small">{r.feature}<div className="muted">{r.required_api}</div></td>
                <td>{capBadge(r.effective_status || r.status)}</td>
                <td className="small muted">{[r.required_permission, r.required_account_type].filter(Boolean).join(' · ')}</td>
                <td className="small muted">{r.limitation}{r.operator_action ? <div style={{ marginTop: 2 }}><strong>Action:</strong> {r.operator_action}</div> : null}</td>
              </tr>
            ))}</tbody></table>
        </div>
      )}
    </Card>
  )
}

// ───────── Connected Accounts ─────────
function Connections() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/marketing/connections')
  const { data: platforms } = useGet<{ rows: Row[] }>('/api/admin/marketing/platforms')
  const [pf, setPf] = useState(''); const [label, setLabel] = useState(''); const [busy, setBusy] = useState(false); const [msg, setMsg] = useState('')
  async function add() {
    if (!pf) return; setBusy(true); setMsg('')
    try { const r = await adminApi.post<Row>('/api/admin/marketing/connections', { platform_code: pf, label }); setLabel(''); if (!r.family_configured) setMsg('Registered. This provider app is not yet configured in Render — set its OAuth client id/secret to enable connecting.'); reload() }
    finally { setBusy(false) }
  }
  async function connect(id: number) {
    const r = await adminApi.post<Row>(`/api/admin/marketing/connections/${id}/oauth-url`, {})
    setMsg(r.operator_action || (r.ok ? 'Ready' : 'Connection not available yet.'))
  }
  async function disconnect(id: number) { if (!confirm('Disconnect this account and clear its tokens?')) return; await adminApi.post(`/api/admin/marketing/connections/${id}/disconnect`, {}); reload() }
  return (
    <Card title="Connected Accounts">
      <p className="muted small" style={{ marginTop: 0 }}>Connect PCI accounts through official OAuth. Access and refresh tokens are stored encrypted and never shown. No usernames or passwords are stored.</p>
      <div className="row" style={{ gap: '.4rem', marginBottom: '.6rem', flexWrap: 'wrap' }}>
        <select value={pf} onChange={(e) => setPf(e.target.value)}><option value="">Select platform…</option>{(platforms?.rows ?? []).map((p) => <option key={p.code} value={p.code}>{p.name}</option>)}</select>
        <input placeholder="Label (optional)" value={label} onChange={(e) => setLabel(e.target.value)} />
        <button className="btn sm" disabled={busy || !pf} onClick={add}>Register</button>
      </div>
      {msg && <p className="small" style={{ background: '#fffbeb', border: '1px solid #fde68a', borderRadius: 8, padding: '.5rem .7rem' }}>{msg}</p>}
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No accounts registered.</Empty> : (
        <table className="data"><thead><tr><th>Platform</th><th>Label</th><th>Status</th><th>Approval</th><th>Token expiry</th><th /></tr></thead>
          <tbody>{data!.rows.map((r) => (
            <tr key={r.id}>
              <td className="small">{r.platform_code}</td><td className="small">{r.label || '—'}</td>
              <td><Badge tone={r.status === 'connected' ? 'ok' : r.status === 'error' || r.status === 'expired' ? 'err' : 'neutral'}>{r.status}</Badge></td>
              <td className="small muted">{r.approval_status}</td>
              <td className="small muted">{r.token_expires_at ? fmtDateTime(r.token_expires_at) : '—'}</td>
              <td style={{ whiteSpace: 'nowrap' }}>
                <button className="btn ghost sm" onClick={() => connect(r.id)}>Connect</button>
                <button className="btn ghost sm" onClick={() => disconnect(r.id)}>Disconnect</button>
              </td>
            </tr>
          ))}</tbody></table>
      )}
    </Card>
  )
}

// ───────── LinkedIn Posts ─────────
const BLANK_POST: Row = { post_type: 'text', body: '', article_title: '', article_url: '', hashtags: '', cta: '', language: 'en' }
function Posts() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/marketing/posts')
  const [edit, setEdit] = useState<Row | null>(null); const [msg, setMsg] = useState('')
  async function save(p: Row) { await adminApi.post('/api/admin/marketing/posts', p); setEdit(null); reload() }
  async function approve(id: number) { await adminApi.post(`/api/admin/marketing/posts/${id}/approve`, {}); reload() }
  async function publish(id: number) { const r = await adminApi.post<Row>(`/api/admin/marketing/posts/${id}/publish`, {}); setMsg(r.operator_action || (r.ok ? 'Publishing…' : 'Not published.')); reload() }
  return (
    <Card title="LinkedIn Company Page Posts" action={<button className="btn sm" onClick={() => setEdit({ ...BLANK_POST })}>New post</button>}>
      <p className="muted small" style={{ marginTop: 0 }}>Draft → approve → publish. Publishing goes live only when the LinkedIn Company Page connection and Community Management access are approved; until then approved posts are queued and marked clearly.</p>
      {msg && <p className="small" style={{ background: '#fffbeb', border: '1px solid #fde68a', borderRadius: 8, padding: '.5rem .7rem' }}>{msg}</p>}
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No posts yet.</Empty> : (
        <table className="data"><thead><tr><th>Type</th><th>Preview</th><th>Approval</th><th>Status</th><th /></tr></thead>
          <tbody>{data!.rows.map((p) => (
            <tr key={p.id}>
              <td className="small">{p.post_type}</td>
              <td className="small">{(p.article_title || p.body || '').slice(0, 60) || '—'}</td>
              <td><Badge tone={p.approval_status === 'approved' ? 'ok' : 'neutral'}>{p.approval_status}</Badge></td>
              <td><Badge tone={p.status === 'published' ? 'ok' : p.status === 'failed' ? 'err' : 'brand'}>{p.status}</Badge></td>
              <td style={{ whiteSpace: 'nowrap' }}>
                <button className="btn ghost sm" onClick={() => setEdit(p)}>Edit</button>
                {p.approval_status !== 'approved' && <button className="btn ghost sm" onClick={() => approve(p.id)}>Approve</button>}
                {p.approval_status === 'approved' && p.status !== 'published' && <button className="btn ghost sm" onClick={() => publish(p.id)}>Publish</button>}
              </td>
            </tr>
          ))}</tbody></table>
      )}
      {edit && <PostEditor post={edit} onClose={() => setEdit(null)} onSave={save} />}
    </Card>
  )
}
function PostEditor({ post, onClose, onSave }: { post: Row; onClose: () => void; onSave: (p: Row) => void }) {
  const [f, setF] = useState<Row>(post); const set = (k: string, v: any) => setF((p) => ({ ...p, [k]: v }))
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread"><h3 style={{ margin: 0 }}>{f.id ? 'Edit' : 'New'} LinkedIn post</h3><button className="btn secondary sm" onClick={onClose}>Close</button></div>
        <div style={{ display: 'grid', gap: '.6rem', marginTop: '.6rem' }}>
          <label className="field"><span>Post type</span><select value={f.post_type} onChange={(e) => set('post_type', e.target.value)}><option value="text">Text</option><option value="article">Article link</option><option value="image">Image</option><option value="multi_image">Multi-image</option><option value="video">Video</option><option value="document">Document</option></select></label>
          <label className="field"><span>Body</span><textarea rows={5} value={f.body || ''} onChange={(e) => set('body', e.target.value)} /></label>
          {(f.post_type === 'article') && <><label className="field"><span>Article title</span><input value={f.article_title || ''} onChange={(e) => set('article_title', e.target.value)} /></label>
            <label className="field"><span>Article URL</span><input value={f.article_url || ''} onChange={(e) => set('article_url', e.target.value)} /></label></>}
          <label className="field"><span>Hashtags</span><input value={f.hashtags || ''} onChange={(e) => set('hashtags', e.target.value)} placeholder="#ProjectControls #PCLAI" /></label>
          <label className="field"><span>Call to action</span><input value={f.cta || ''} onChange={(e) => set('cta', e.target.value)} /></label>
          <label className="field"><span>Scheduled date/time (optional)</span><input value={f.scheduled_at || ''} onChange={(e) => set('scheduled_at', e.target.value)} placeholder="2026-08-01 09:00" /></label>
          <button className="btn" onClick={() => onSave(f)}>Save draft</button>
        </div>
      </div>
    </div>
  )
}

// ───────── Manual Outreach ─────────
function Outreach() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/marketing/outreach')
  const [f, setF] = useState<Row>({ prospect_name: '', profile_url: '', suggested_message: '' })
  async function add() { if (!f.prospect_name) return; await adminApi.post('/api/admin/marketing/outreach', f); setF({ prospect_name: '', profile_url: '', suggested_message: '' }); reload() }
  async function markSent(r: Row) { await adminApi.post('/api/admin/marketing/outreach', { ...r, sent_manually: 1 }); reload() }
  return (
    <Card title="Manual LinkedIn Outreach">
      <p className="muted small" style={{ marginTop: 0 }}>PCI never auto-sends ordinary personal LinkedIn messages. Prepare a suggested message, open LinkedIn, send it manually, then record that the outreach occurred.</p>
      <div style={{ display: 'grid', gap: '.4rem', marginBottom: '.7rem', maxWidth: 640 }}>
        <input placeholder="Prospect name" value={f.prospect_name} onChange={(e) => setF({ ...f, prospect_name: e.target.value })} />
        <input placeholder="LinkedIn profile URL" value={f.profile_url} onChange={(e) => setF({ ...f, profile_url: e.target.value })} />
        <textarea rows={3} placeholder="Suggested message (you send this manually inside LinkedIn)" value={f.suggested_message} onChange={(e) => setF({ ...f, suggested_message: e.target.value })} />
        <div><button className="btn sm" onClick={add}>Add outreach record</button></div>
      </div>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No outreach recorded.</Empty> : (
        <table className="data"><thead><tr><th>Prospect</th><th>Profile</th><th>Sent manually</th><th /></tr></thead>
          <tbody>{data!.rows.map((r) => (
            <tr key={r.id}><td className="small">{r.prospect_name}</td>
              <td className="small">{r.profile_url ? <a href={r.profile_url} target="_blank" rel="noreferrer">Open</a> : '—'}</td>
              <td>{r.sent_manually ? <Badge tone="ok">yes · {r.sent_at ? fmtDateTime(r.sent_at) : ''}</Badge> : <Badge tone="neutral">no</Badge>}</td>
              <td>{!r.sent_manually && <button className="btn ghost sm" onClick={() => markSent(r)}>Mark sent</button>}</td></tr>
          ))}</tbody></table>
      )}
    </Card>
  )
}

// ───────── Campaigns ─────────
const BLANK_CAMP: Row = { name: '', code: '', objective: 'leads', total_budget: 0, budget_currency: 'USD', alloc_linkedin: 0, alloc_google: 0, alloc_meta: 0 }
function Campaigns() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/marketing/campaigns')
  const [edit, setEdit] = useState<Row | null>(null)
  async function save(c: Row) { await adminApi.post('/api/admin/marketing/campaigns', c); setEdit(null); reload() }
  async function approve(id: number) { await adminApi.post(`/api/admin/marketing/campaigns/${id}/approve`, {}); reload() }
  return (
    <Card title="Unified Campaigns" action={<button className="btn sm" onClick={() => setEdit({ ...BLANK_CAMP })}>New campaign</button>}>
      <p className="muted small" style={{ marginTop: 0 }}>One PCI campaign can span LinkedIn, Google and Meta with per-platform allocations. Launching to a platform is gated by that platform's approved connection (Phase 2).</p>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No campaigns.</Empty> : (
        <table className="data"><thead><tr><th>Name</th><th>Objective</th><th>Budget</th><th>Status</th><th>Approval</th><th /></tr></thead>
          <tbody>{data!.rows.map((c) => (
            <tr key={c.id}><td className="small">{c.name}<div className="muted">{c.code}</div></td><td className="small">{c.objective}</td>
              <td className="small">{c.total_budget} {c.budget_currency}</td>
              <td><Badge tone={c.status === 'active' ? 'ok' : 'brand'}>{c.status}</Badge></td>
              <td><Badge tone={c.approval_status === 'approved' ? 'ok' : 'neutral'}>{c.approval_status}</Badge></td>
              <td style={{ whiteSpace: 'nowrap' }}>
                <button className="btn ghost sm" onClick={() => setEdit(c)}>Edit</button>
                {c.approval_status !== 'approved' && <button className="btn ghost sm" onClick={() => approve(c.id)}>Approve</button>}
              </td></tr>
          ))}</tbody></table>
      )}
      {edit && <CampaignEditor camp={edit} onClose={() => setEdit(null)} onSave={save} />}
    </Card>
  )
}
function CampaignEditor({ camp, onClose, onSave }: { camp: Row; onClose: () => void; onSave: (c: Row) => void }) {
  const [f, setF] = useState<Row>(camp); const set = (k: string, v: any) => setF((p) => ({ ...p, [k]: v }))
  const alloc = (Number(f.alloc_linkedin) || 0) + (Number(f.alloc_google) || 0) + (Number(f.alloc_meta) || 0)
  const over = alloc > (Number(f.total_budget) || 0)
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread"><h3 style={{ margin: 0 }}>{f.id ? 'Edit' : 'New'} campaign</h3><button className="btn secondary sm" onClick={onClose}>Close</button></div>
        <div style={{ display: 'grid', gap: '.6rem', marginTop: '.6rem' }}>
          <label className="field"><span>Name</span><input value={f.name} onChange={(e) => set('name', e.target.value)} /></label>
          <label className="field"><span>Internal code</span><input value={f.code || ''} onChange={(e) => set('code', e.target.value)} /></label>
          <label className="field"><span>Objective</span><select value={f.objective} onChange={(e) => set('objective', e.target.value)}><option value="awareness">Awareness</option><option value="traffic">Traffic</option><option value="engagement">Engagement</option><option value="leads">Leads</option><option value="conversions">Conversions</option><option value="event">Event</option></select></label>
          <label className="field"><span>Objective / geography</span><input value={f.geography || ''} onChange={(e) => set('geography', e.target.value)} placeholder="Countries / regions" /></label>
          <label className="field"><span>Total budget</span><input type="number" value={f.total_budget} onChange={(e) => set('total_budget', Number(e.target.value))} /></label>
          <label className="field"><span>Currency</span><input value={f.budget_currency} onChange={(e) => set('budget_currency', e.target.value)} /></label>
          <div className="row" style={{ gap: '.4rem' }}>
            <label className="field" style={{ flex: 1 }}><span>LinkedIn</span><input type="number" value={f.alloc_linkedin} onChange={(e) => set('alloc_linkedin', Number(e.target.value))} /></label>
            <label className="field" style={{ flex: 1 }}><span>Google</span><input type="number" value={f.alloc_google} onChange={(e) => set('alloc_google', Number(e.target.value))} /></label>
            <label className="field" style={{ flex: 1 }}><span>Meta</span><input type="number" value={f.alloc_meta} onChange={(e) => set('alloc_meta', Number(e.target.value))} /></label>
          </div>
          {over && <p className="small" style={{ color: '#b91c1c', margin: 0 }}>Platform allocations ({alloc}) exceed the total budget ({f.total_budget}).</p>}
          <button className="btn" disabled={over} onClick={() => onSave(f)}>Save campaign</button>
        </div>
      </div>
    </div>
  )
}

// ───────── Promotions ─────────
const BLANK_PROMO: Row = { name: '', code: '', promo_type: 'percentage', fee_type: 'exam', currency: 'USD', status: 'draft' }
function Promotions() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/marketing/promotions')
  const [edit, setEdit] = useState<Row | null>(null)
  async function save(p: Row) { await adminApi.post('/api/admin/marketing/promotions', p); setEdit(null); reload() }
  return (
    <Card title="Promotions" action={<button className="btn sm" onClick={() => setEdit({ ...BLANK_PROMO })}>New promotion</button>}>
      <p className="muted small" style={{ marginTop: 0 }}>Ads must display accurate pricing and conditions. Expired or inactive promotions stop appearing in new campaigns.</p>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No promotions.</Empty> : (
        <table className="data"><thead><tr><th>Name</th><th>Code</th><th>Type</th><th>Window</th><th>Status</th><th /></tr></thead>
          <tbody>{data!.rows.map((p) => (
            <tr key={p.id}><td className="small">{p.name}</td><td className="small">{p.code || '—'}</td><td className="small">{p.promo_type}</td>
              <td className="small muted">{[p.start_date, p.end_date].filter(Boolean).join(' → ') || '—'}</td>
              <td><Badge tone={p.status === 'active' ? 'ok' : p.status === 'expired' ? 'err' : 'neutral'}>{p.status}</Badge></td>
              <td><button className="btn ghost sm" onClick={() => setEdit(p)}>Edit</button></td></tr>
          ))}</tbody></table>
      )}
      {edit && <PromoEditor promo={edit} onClose={() => setEdit(null)} onSave={save} />}
    </Card>
  )
}
function PromoEditor({ promo, onClose, onSave }: { promo: Row; onClose: () => void; onSave: (p: Row) => void }) {
  const [f, setF] = useState<Row>(promo); const set = (k: string, v: any) => setF((p) => ({ ...p, [k]: v }))
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread"><h3 style={{ margin: 0 }}>{f.id ? 'Edit' : 'New'} promotion</h3><button className="btn secondary sm" onClick={onClose}>Close</button></div>
        <div style={{ display: 'grid', gap: '.6rem', marginTop: '.6rem' }}>
          <label className="field"><span>Name</span><input value={f.name} onChange={(e) => set('name', e.target.value)} /></label>
          <label className="field"><span>Promotion code</span><input value={f.code || ''} onChange={(e) => set('code', e.target.value)} /></label>
          <label className="field"><span>Type</span><select value={f.promo_type} onChange={(e) => set('promo_type', e.target.value)}><option value="percentage">Percentage discount</option><option value="fixed">Fixed discount</option><option value="free_membership">Complimentary membership</option><option value="free_exam">Complimentary exam</option><option value="app_fee_waiver">Application-fee waiver</option><option value="exam_fee_waiver">Exam-fee waiver</option></select></label>
          <label className="field"><span>Fee type</span><select value={f.fee_type} onChange={(e) => set('fee_type', e.target.value)}><option value="exam">Exam</option><option value="application">Application</option><option value="membership">Membership</option></select></label>
          <div className="row" style={{ gap: '.4rem' }}>
            <label className="field" style={{ flex: 1 }}><span>Original</span><input type="number" value={f.original_amount || ''} onChange={(e) => set('original_amount', Number(e.target.value))} /></label>
            <label className="field" style={{ flex: 1 }}><span>Discount %</span><input type="number" value={f.discount_percent || ''} onChange={(e) => set('discount_percent', Number(e.target.value))} /></label>
            <label className="field" style={{ flex: 1 }}><span>Net</span><input type="number" value={f.net_amount || ''} onChange={(e) => set('net_amount', Number(e.target.value))} /></label>
          </div>
          <div className="row" style={{ gap: '.4rem' }}>
            <label className="field" style={{ flex: 1 }}><span>Start date</span><input value={f.start_date || ''} onChange={(e) => set('start_date', e.target.value)} placeholder="2026-08-01" /></label>
            <label className="field" style={{ flex: 1 }}><span>End date</span><input value={f.end_date || ''} onChange={(e) => set('end_date', e.target.value)} placeholder="2026-08-31" /></label>
          </div>
          <label className="field"><span>Status</span><select value={f.status} onChange={(e) => set('status', e.target.value)}><option value="draft">Draft</option><option value="active">Active</option><option value="paused">Paused</option><option value="expired">Expired</option></select></label>
          <button className="btn" onClick={() => onSave(f)}>Save promotion</button>
        </div>
      </div>
    </div>
  )
}

// ───────── Lead Centre ─────────
const LEAD_STATUSES = ['new', 'assigned', 'contacted', 'qualified', 'interested', 'followup', 'application_started', 'application_submitted', 'converted', 'not_interested', 'invalid', 'duplicate', 'do_not_contact']
function Leads() {
  const [status, setStatus] = useState('')
  const { data, loading, reload } = useGet<{ rows: Row[] }>(`/api/admin/marketing/leads${status ? `?status=${status}` : ''}`)
  async function setLeadStatus(id: number, s: string) { await adminApi.post(`/api/admin/marketing/leads/${id}/status`, { status: s, assign_self: 1 }); reload() }
  return (
    <Card title="Lead Centre" action={<select value={status} onChange={(e) => setStatus(e.target.value)}><option value="">All</option>{LEAD_STATUSES.map((s) => <option key={s} value={s}>{s.replace(/_/g, ' ')}</option>)}</select>}>
      <p className="muted small" style={{ marginTop: 0 }}>Unified leads from LinkedIn, Meta, the website, webinars and inquiries. An advertising lead does not automatically create a student account.</p>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No leads.</Empty> : (
        <div style={{ overflowX: 'auto' }}>
          <table className="data"><thead><tr><th>Name</th><th>Email</th><th>Source</th><th>Interest</th><th>Status</th><th /></tr></thead>
            <tbody>{data!.rows.map((l) => (
              <tr key={l.id}><td className="small">{l.name || '—'}</td><td className="small">{l.email || l.phone || '—'}</td>
                <td className="small muted">{l.source_platform || '—'}</td><td className="small">{l.certification_interest || '—'}</td>
                <td><Badge tone={l.status === 'converted' ? 'ok' : l.status === 'invalid' || l.status === 'do_not_contact' ? 'err' : 'brand'}>{(l.status || 'new').replace(/_/g, ' ')}</Badge></td>
                <td><select value="" onChange={(e) => e.target.value && setLeadStatus(l.id, e.target.value)}><option value="">Set status…</option>{LEAD_STATUSES.map((s) => <option key={s} value={s}>{s.replace(/_/g, ' ')}</option>)}</select></td></tr>
            ))}</tbody></table>
        </div>
      )}
    </Card>
  )
}

// ───────── Generic simple list + quick-create (audiences/creatives/landing/conversions) ─────────
type FieldDef = [key: string, label: string, required?: boolean]
function SimpleList({ title, path, fields, note }: { title: string; path: string; fields: FieldDef[]; note?: string }) {
  const { data, loading, reload } = useGet<{ rows: Row[] }>(`/api/admin/marketing/${path}`)
  const [f, setF] = useState<Row>({})
  const req = fields.find((x) => x[2])?.[0] ?? fields[0][0]
  async function add() { if (!f[req]) return; await adminApi.post(`/api/admin/marketing/${path}`, f); setF({}); reload() }
  return (
    <Card title={title}>
      {note && <p className="muted small" style={{ marginTop: 0 }}>{note}</p>}
      <div style={{ display: 'grid', gap: '.4rem', marginBottom: '.7rem', maxWidth: 640 }}>
        {fields.map(([k, label, required]) => (
          <input key={k} placeholder={label + (required ? ' *' : '')} value={f[k] ?? ''} onChange={(e) => setF({ ...f, [k]: e.target.value })} />
        ))}
        <div><button className="btn sm" onClick={add}>Add</button></div>
      </div>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>Nothing yet.</Empty> : (
        <div style={{ overflowX: 'auto' }}>
          <table className="data"><thead><tr>{fields.map(([, label]) => <th key={label}>{label}</th>)}<th>Status</th></tr></thead>
            <tbody>{data!.rows.map((r) => (
              <tr key={r.id}>{fields.map(([k]) => <td key={k} className="small">{String(r[k] ?? '—').slice(0, 60)}</td>)}<td className="small muted">{r.status || r.approval_status || '—'}</td></tr>
            ))}</tbody></table>
        </div>
      )}
    </Card>
  )
}

// ───────── Search Console ─────────
function SearchConsole() {
  const { data, loading, reload } = useGet<{ rows: Row[]; sitemaps: Row[] }>('/api/admin/marketing/gsc/properties')
  const [msg, setMsg] = useState('')
  async function submit() { const r = await adminApi.post<Row>('/api/admin/marketing/gsc/sitemaps/submit', {}); setMsg(r.operator_action || 'Done'); reload() }
  return (
    <Card title="Google Search Console">
      <p className="muted small" style={{ marginTop: 0 }}>Search performance, sitemaps and URL inspection for the verified PCI property — this is search analytics, not paid advertising. Sitemap submission is a discovery signal and does not guarantee indexing.</p>
      {msg && <p className="small" style={{ background: '#fffbeb', border: '1px solid #fde68a', borderRadius: 8, padding: '.5rem .7rem' }}>{msg}</p>}
      <div className="row" style={{ gap: '.4rem', marginBottom: '.6rem' }}><button className="btn sm" onClick={submit}>Submit sitemap</button></div>
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No verified property connected yet. Connect Google Search Console under Connected Accounts.</Empty> : (
        <table className="data"><thead><tr><th>Property</th><th>Verified</th><th>Last synced</th></tr></thead>
          <tbody>{data!.rows.map((p) => (
            <tr key={p.id}><td className="small">{p.property}</td><td>{p.verified ? <Badge tone="ok">yes</Badge> : <Badge tone="neutral">no</Badge>}</td><td className="small muted">{p.last_synced_at ? fmtDateTime(p.last_synced_at) : '—'}</td></tr>
          ))}</tbody></table>
      )}
    </Card>
  )
}

// ───────── Alerts ─────────
function Alerts() {
  const { data, loading, reload } = useGet<{ rows: Row[] }>('/api/admin/marketing/alerts')
  async function ack(id: number) { await adminApi.post(`/api/admin/marketing/alerts/${id}/ack`, {}); reload() }
  return (
    <Card title="Alerts">
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No alerts.</Empty> : (
        <table className="data"><thead><tr><th>Severity</th><th>Kind</th><th>Message</th><th>Status</th><th /></tr></thead>
          <tbody>{data!.rows.map((a) => (
            <tr key={a.id}><td><Badge tone={a.severity === 'critical' ? 'err' : a.severity === 'warning' ? 'warn' : 'brand'}>{a.severity}</Badge></td>
              <td className="small">{a.kind}</td><td className="small">{a.message}</td><td className="small muted">{a.status}</td>
              <td>{a.status === 'open' && <button className="btn ghost sm" onClick={() => ack(a.id)}>Acknowledge</button>}</td></tr>
          ))}</tbody></table>
      )}
    </Card>
  )
}

// ───────── Audit ─────────
function Audit() {
  const { data, loading } = useGet<{ rows: Row[] }>('/api/admin/marketing/audit')
  return (
    <Card title="Audit history">
      {loading ? <Spinner /> : (data?.rows.length ?? 0) === 0 ? <Empty>No marketing actions logged yet.</Empty> : (
        <table className="data"><thead><tr><th>When</th><th>Action</th><th>Details</th></tr></thead>
          <tbody>{data!.rows.map((r) => (
            <tr key={r.id}><td className="small muted">{fmtDateTime(r.created_at)}</td><td className="small">{r.action}</td><td className="small muted">{r.details}</td></tr>
          ))}</tbody></table>
      )}
    </Card>
  )
}
