import { useMemo, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { ApiError } from '../../api/client'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'

// Admin Console → Social media. The database-backed configuration surface for every public
// social-media profile (rendered server-side into the footer) and every page-sharing button. Nothing
// here is hardcoded in the public website: platforms, URLs, order, placement and visibility all live
// in social_accounts and take effect immediately (the render cache is invalidated on every save).

interface Account {
  id: number
  platform: string
  display_name: string | null
  handle: string | null
  url: string
  icon_type: string | null
  custom_icon: string | null
  aria_label: string | null
  tooltip: string | null
  locations: string | null
  display_order: number | null
  active: number
  is_official: number
  open_new_tab: number
  rel_nofollow: number
  language: string | null
  country: string | null
  effective_date: string | null
  expiry_date: string | null
  approval_status: string | null
  link_status: string | null
  link_checked_at: string | null
}
interface Platform { key: string; label: string; color: string; domains: string[]; svg: string }
interface LocationOpt { key: string; label: string }
interface ShareRow { content_type: string; buttons: string; enabled: number }
interface AuditRow { id: number; account_id: number | null; actor_name: string | null; action: string; detail: string | null; created_at: string | null }
interface Payload {
  enabled: boolean
  analytics: boolean
  accounts: Account[]
  platforms: Platform[]
  locations: LocationOpt[]
  share_targets: LocationOpt[]
  share: ShareRow[]
  audit: AuditRow[]
}

const TABS = ['Profiles', 'Sharing buttons', 'Audit history'] as const

export default function SocialMedia() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Profiles')
  const { data, loading, error, refetch } = useAdminQuery<Payload>('/api/admin/social')
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null
  return (
    <div className="page">
      <PageHeader
        title="Social media"
        subtitle="Configure the Institute's social-media profiles and the page-sharing buttons for public content. Profile links appear in the website footer; changes go live immediately. Nothing is hardcoded — the database is the single source of truth."
      />
      <MasterBar data={data} onSaved={refetch} />
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => (
          <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>
        ))}
      </div>
      {tab === 'Profiles' && <ProfilesTab data={data} onChanged={refetch} />}
      {tab === 'Sharing buttons' && <SharingTab data={data} onChanged={refetch} />}
      {tab === 'Audit history' && <AuditTab rows={data.audit} />}
    </div>
  )
}

function MasterBar({ data, onSaved }: { data: Payload; onSaved: () => void }) {
  const [busy, setBusy] = useState(false)
  async function toggle(patch: Record<string, boolean>) {
    setBusy(true)
    try { await adminApi.post('/api/admin/social', patch); onSaved() } finally { setBusy(false) }
  }
  return (
    <Card>
      <div className="row" style={{ gap: '1.4rem', flexWrap: 'wrap', alignItems: 'center' }}>
        <label className="row" style={{ gap: '.5rem', fontWeight: 600 }}>
          <input type="checkbox" checked={data.enabled} disabled={busy} onChange={(e) => toggle({ social_enabled: e.target.checked })} style={{ width: 'auto' }} />
          Show social icons on the public website {data.enabled ? <Badge tone="ok">Live</Badge> : <Badge tone="warn">Off</Badge>}
        </label>
        <label className="row" style={{ gap: '.5rem' }}>
          <input type="checkbox" checked={data.analytics} disabled={busy} onChange={(e) => toggle({ social_analytics_enabled: e.target.checked })} style={{ width: 'auto' }} />
          <span className="small">Tag icons for click analytics</span>
        </label>
        {!data.enabled && <span className="muted small">While off, no social icons render anywhere — even if profiles are configured below.</span>}
      </div>
    </Card>
  )
}

const linkTone = (s?: string | null): 'ok' | 'warn' | 'err' | 'neutral' =>
  s === 'valid' ? 'ok' : s === 'warning' ? 'warn' : s === 'invalid' || s === 'unreachable' ? 'err' : 'neutral'

function ProfilesTab({ data, onChanged }: { data: Payload; onChanged: () => void }) {
  const [edit, setEdit] = useState<Account | 'new' | null>(null)
  const [busyId, setBusyId] = useState<number | null>(null)
  const platByKey = useMemo(() => Object.fromEntries(data.platforms.map((p) => [p.key, p])), [data.platforms])

  async function action(id: number, path: string, body?: unknown) {
    setBusyId(id)
    try { await adminApi.post(`/api/admin/social/account/${id}/${path}`, body ?? {}); onChanged() }
    catch (e) { alert(e instanceof Error ? e.message : 'Action failed.') }
    finally { setBusyId(null) }
  }

  return (
    <>
      <Card title={`Profiles (${data.accounts.length})`} action={<button className="btn sm" onClick={() => setEdit('new')}>+ Add profile</button>}>
        {data.accounts.length === 0 ? <Empty>No social-media profiles yet. Add one to show it in the footer.</Empty> : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead><tr><th></th><th>Platform</th><th>Handle / URL</th><th>Placement</th><th>Order</th><th>Status</th><th>Link check</th><th></th></tr></thead>
              <tbody>
                {data.accounts.map((a) => {
                  const p = platByKey[a.platform]
                  return (
                    <tr key={a.id}>
                      <td><span className="soc-ic" style={{ color: p?.color }} dangerouslySetInnerHTML={{ __html: p?.svg ?? '' }} /></td>
                      <td className="small">{p?.label ?? a.platform}{!!a.is_official && <> <Badge tone="brand">official</Badge></>}</td>
                      <td className="small" style={{ maxWidth: 260 }}>
                        {a.handle && <div>{a.handle}</div>}
                        <a href={a.url} target="_blank" rel="noreferrer noopener" className="muted" style={{ wordBreak: 'break-all' }}>{a.url}</a>
                      </td>
                      <td className="small muted">{(a.locations || '').split(',').filter(Boolean).length} location(s)</td>
                      <td className="small">{a.display_order}</td>
                      <td>
                        {a.approval_status === 'archived' ? <Badge tone="neutral">archived</Badge>
                          : a.approval_status === 'draft' ? <Badge tone="warn">draft</Badge>
                          : a.active ? <Badge tone="ok">active</Badge> : <Badge tone="neutral">inactive</Badge>}
                      </td>
                      <td><Badge tone={linkTone(a.link_status)}>{a.link_status || 'not_checked'}</Badge></td>
                      <td>
                        <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                          <button className="btn ghost sm" onClick={() => setEdit(a)}>Edit</button>
                          <button className="btn ghost sm" disabled={busyId === a.id} onClick={() => action(a.id, 'check')}>Check</button>
                          <button className="btn ghost sm" disabled={busyId === a.id} onClick={() => action(a.id, 'activate', { active: !a.active })}>{a.active ? 'Deactivate' : 'Activate'}</button>
                          <button className="btn ghost sm" disabled={busyId === a.id} onClick={() => action(a.id, 'duplicate')}>Duplicate</button>
                          <button className="btn ghost sm" disabled={busyId === a.id} onClick={() => { if (confirm('Archive this profile? It will be hidden from the website.')) action(a.id, 'delete') }}>Archive</button>
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
      {edit && <EditDrawer account={edit === 'new' ? null : edit} data={data} onClose={() => setEdit(null)} onSaved={() => { setEdit(null); onChanged() }} />}
    </>
  )
}

function EditDrawer({ account, data, onClose, onSaved }: { account: Account | null; data: Payload; onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState<Record<string, string>>(() => ({
    platform: account?.platform ?? 'linkedin',
    display_name: account?.display_name ?? '',
    handle: account?.handle ?? '',
    url: account?.url ?? '',
    icon_type: account?.icon_type ?? 'builtin',
    custom_icon: account?.custom_icon ?? '',
    aria_label: account?.aria_label ?? '',
    tooltip: account?.tooltip ?? '',
    display_order: String(account?.display_order ?? ''),
    language: account?.language ?? '',
    country: account?.country ?? '',
    effective_date: account?.effective_date ?? '',
    expiry_date: account?.expiry_date ?? '',
    approval_status: account?.approval_status === 'archived' ? 'approved' : (account?.approval_status ?? 'approved'),
  }))
  const [locs, setLocs] = useState<Set<string>>(new Set((account?.locations ?? 'footer').split(',').map((x) => x.trim()).filter(Boolean)))
  const [flags, setFlags] = useState({
    active: account ? !!account.active : true,
    is_official: account ? !!account.is_official : true,
    open_new_tab: account ? !!account.open_new_tab : true,
    rel_nofollow: account ? !!account.rel_nofollow : true,
  })
  const [saving, setSaving] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [override, setOverride] = useState<string | null>(null)

  const plat = data.platforms.find((p) => p.key === f.platform)
  const set = (k: string, v: string) => setF((s) => ({ ...s, [k]: v }))
  const clientUrlHint = (() => {
    const u = f.url.trim()
    if (!u) return null
    if (!/^https:\/\//i.test(u)) return { tone: 'warn', msg: 'Use a full https:// address.' }
    if (plat && plat.domains.length && !plat.domains.some((d) => { try { return new URL(u).host.includes(d.replace(/\.$/, '')) } catch { return false } }))
      return { tone: 'warn', msg: `Does not look like an official ${plat.label} domain.` }
    return { tone: 'ok', msg: 'Looks valid.' }
  })()

  async function save(withOverride = false) {
    setSaving(true); setErr(null); if (!withOverride) setOverride(null)
    try {
      await adminApi.post('/api/admin/social/account', {
        id: account?.id,
        ...f,
        locations: [...locs],
        active: flags.active, is_official: flags.is_official, open_new_tab: flags.open_new_tab, rel_nofollow: flags.rel_nofollow,
        override_domain: withOverride,
      })
      onSaved()
    } catch (e) {
      if (e instanceof ApiError && e.status === 422 && (e.body as { needs_override?: boolean })?.needs_override) {
        setOverride((e.body as { warning?: string }).warning ?? 'Domain does not match the selected platform.')
      } else setErr(e instanceof Error ? e.message : 'Could not save.')
    } finally { setSaving(false) }
  }

  return (
    <div style={{ marginTop: '.9rem', borderTop: '1px solid var(--line,#e2e8f0)', paddingTop: '.9rem' }}>
      <div className="row" style={{ gap: '.6rem', alignItems: 'center', marginBottom: '.6rem' }}>
        <span className="soc-ic" style={{ color: plat?.color, display: 'inline-flex' }} dangerouslySetInnerHTML={{ __html: plat?.svg ?? '' }} />
        <h3 style={{ margin: 0 }}>{account ? 'Edit profile' : 'New profile'}</h3>
      </div>
      <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: 'repeat(auto-fit,minmax(220px,1fr))', maxWidth: 900 }}>
        <label>Platform
          <select value={f.platform} onChange={(e) => set('platform', e.target.value)}>
            {data.platforms.map((p) => <option key={p.key} value={p.key}>{p.label}</option>)}
          </select>
        </label>
        <label>Display name <span className="muted small">(optional)</span>
          <input value={f.display_name} onChange={(e) => set('display_name', e.target.value)} placeholder="Project Controls Institute" />
        </label>
        <label>Username / handle <span className="muted small">(optional)</span>
          <input value={f.handle} onChange={(e) => set('handle', e.target.value)} placeholder="@pciglobal" />
        </label>
        <label style={{ gridColumn: '1 / -1' }}>Profile URL
          <input value={f.url} onChange={(e) => set('url', e.target.value)} placeholder="https://www.linkedin.com/company/…" />
          {clientUrlHint && <span className={'small'} style={{ color: clientUrlHint.tone === 'ok' ? 'var(--ok,#16a34a)' : 'var(--warn,#b45309)' }}>{clientUrlHint.msg}</span>}
        </label>
        <label>Accessible label <span className="muted small">(screen readers)</span>
          <input value={f.aria_label} onChange={(e) => set('aria_label', e.target.value)} placeholder="PCI on LinkedIn" />
        </label>
        <label>Tooltip <span className="muted small">(optional)</span>
          <input value={f.tooltip} onChange={(e) => set('tooltip', e.target.value)} placeholder="Follow us on LinkedIn" />
        </label>
        <label>Display order
          <input type="number" value={f.display_order} onChange={(e) => set('display_order', e.target.value)} placeholder="0" />
        </label>
        <label>Approval status
          <select value={f.approval_status} onChange={(e) => set('approval_status', e.target.value)}>
            <option value="approved">Approved (can display)</option>
            <option value="draft">Draft (hidden)</option>
          </select>
        </label>
        <label>Icon
          <select value={f.icon_type} onChange={(e) => set('icon_type', e.target.value)}>
            <option value="builtin">Official platform icon</option>
            <option value="custom">Custom SVG</option>
          </select>
        </label>
        <label>Language <span className="muted small">(blank = all)</span>
          <input value={f.language} onChange={(e) => set('language', e.target.value)} placeholder="en" />
        </label>
        <label>Country <span className="muted small">(blank = all)</span>
          <input value={f.country} onChange={(e) => set('country', e.target.value)} placeholder="SA" />
        </label>
        <label>Effective date <span className="muted small">(blank = now)</span>
          <input type="date" value={f.effective_date?.slice(0, 10) ?? ''} onChange={(e) => set('effective_date', e.target.value)} />
        </label>
        <label>Expiry date <span className="muted small">(blank = never)</span>
          <input type="date" value={f.expiry_date?.slice(0, 10) ?? ''} onChange={(e) => set('expiry_date', e.target.value)} />
        </label>
        {f.icon_type === 'custom' && (
          <label style={{ gridColumn: '1 / -1' }}>Custom icon SVG <span className="muted small">(inline &lt;svg&gt; or a path definition; scripts are stripped)</span>
            <textarea rows={3} value={f.custom_icon} onChange={(e) => set('custom_icon', e.target.value)} placeholder="<svg viewBox='0 0 24 24'>…</svg>" />
          </label>
        )}
      </div>

      <fieldset style={{ margin: '.8rem 0', border: '1px solid var(--line,#e2e8f0)', borderRadius: 8, padding: '.6rem .8rem' }}>
        <legend className="small muted">Display locations</legend>
        <div className="row" style={{ gap: '.7rem 1rem', flexWrap: 'wrap' }}>
          {data.locations.map((l) => (
            <label key={l.key} className="row small" style={{ gap: '.3rem' }}>
              <input type="checkbox" checked={locs.has(l.key)} style={{ width: 'auto' }}
                onChange={(e) => setLocs((s) => { const n = new Set(s); e.target.checked ? n.add(l.key) : n.delete(l.key); return n })} />
              {l.label}
            </label>
          ))}
        </div>
      </fieldset>

      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', marginBottom: '.6rem' }}>
        {([['active', 'Active'], ['is_official', 'Official (in structured data)'], ['open_new_tab', 'Open in new tab'], ['rel_nofollow', 'rel=nofollow']] as const).map(([k, lbl]) => (
          <label key={k} className="row small" style={{ gap: '.35rem' }}>
            <input type="checkbox" checked={flags[k]} style={{ width: 'auto' }} onChange={(e) => setFlags((s) => ({ ...s, [k]: e.target.checked }))} />
            {lbl}
          </label>
        ))}
      </div>

      {override && (
        <div className="notice warn" style={{ marginBottom: '.5rem' }}>
          {override} You can publish anyway with a documented override.
          <div style={{ marginTop: '.4rem' }}><button className="btn sm" disabled={saving} onClick={() => save(true)}>Save with override</button></div>
        </div>
      )}
      {err && <div className="notice err" style={{ marginBottom: '.5rem' }}>{err}</div>}
      <div className="row" style={{ gap: '.5rem' }}>
        <button className="btn" disabled={saving} onClick={() => save(false)}>{saving ? 'Saving…' : 'Save profile'}</button>
        {f.url && <a className="btn ghost" href={f.url} target="_blank" rel="noreferrer noopener">Test link ↗</a>}
        <button className="btn ghost" onClick={onClose}>Cancel</button>
      </div>
    </div>
  )
}

function SharingTab({ data, onChanged }: { data: Payload; onChanged: () => void }) {
  const [rows, setRows] = useState(() => data.share.map((r) => ({
    content_type: r.content_type,
    enabled: !!r.enabled,
    buttons: new Set((r.buttons || '').split(',').map((x) => x.trim()).filter(Boolean)),
  })))
  const [saving, setSaving] = useState(false)
  const [note, setNote] = useState<string | null>(null)

  async function save() {
    setSaving(true); setNote(null)
    try {
      await adminApi.post('/api/admin/social/share', { rows: rows.map((r) => ({ content_type: r.content_type, enabled: r.enabled, buttons: [...r.buttons] })) })
      setNote('Saved.'); onChanged()
    } catch (e) { setNote(e instanceof Error ? e.message : 'Could not save.') }
    finally { setSaving(false) }
  }

  return (
    <Card title="Page-sharing buttons">
      <p className="muted small" style={{ marginTop: 0 }}>Share buttons for public content only — they use each page's own URL, title and Open Graph image. Private portal, payment, exam and certificate URLs are never shareable. These are configured separately from the Institute's own profile links above.</p>
      {note && <div className="notice" role="status" style={{ marginBottom: '.6rem' }}>{note}</div>}
      <div style={{ overflowX: 'auto' }}>
        <table className="data">
          <thead><tr><th>Content type</th><th>Enabled</th><th>Buttons</th></tr></thead>
          <tbody>
            {rows.map((r, i) => (
              <tr key={r.content_type}>
                <td className="small" style={{ textTransform: 'capitalize' }}>{r.content_type}</td>
                <td>
                  <input type="checkbox" checked={r.enabled} style={{ width: 'auto' }}
                    onChange={(e) => setRows((s) => s.map((x, j) => j === i ? { ...x, enabled: e.target.checked } : x))} />
                </td>
                <td>
                  <div className="row" style={{ gap: '.5rem .9rem', flexWrap: 'wrap' }}>
                    {data.share_targets.map((t) => (
                      <label key={t.key} className="row small" style={{ gap: '.3rem' }}>
                        <input type="checkbox" checked={r.buttons.has(t.key)} style={{ width: 'auto' }}
                          onChange={(e) => setRows((s) => s.map((x, j) => {
                            if (j !== i) return x
                            const n = new Set(x.buttons); e.target.checked ? n.add(t.key) : n.delete(t.key)
                            return { ...x, buttons: n }
                          }))} />
                        {t.label}
                      </label>
                    ))}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <div style={{ marginTop: '.7rem' }}><button className="btn" disabled={saving} onClick={save}>{saving ? 'Saving…' : 'Save sharing settings'}</button></div>
    </Card>
  )
}

function AuditTab({ rows }: { rows: AuditRow[] }) {
  if (rows.length === 0) return <Card title="Change history"><Empty>No changes recorded yet.</Empty></Card>
  return (
    <Card title={`Change history (${rows.length})`}>
      <div style={{ overflowX: 'auto' }}>
        <table className="data">
          <thead><tr><th>When</th><th>Who</th><th>Action</th><th>Detail</th></tr></thead>
          <tbody>
            {rows.map((r) => (
              <tr key={r.id}>
                <td className="small muted">{r.created_at}</td>
                <td className="small">{r.actor_name || '—'}</td>
                <td><Badge tone="brand">{r.action}</Badge></td>
                <td className="small muted" style={{ maxWidth: 420 }}>{r.detail || '—'}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </Card>
  )
}
