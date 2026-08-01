import { useMemo, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'

// Admin Console → SEO. One section, four tabs: Overview (health), Page SEO (per-page fields +
// issue flags; saves through the existing pages PATCH), Redirects (managed 301s, single-hop
// enforced server-side), Audit (practical site scan). Content editing stays in Pages & content —
// this section owns only the SEO surface.

interface Overview {
  canonical_host: string
  pages: number; published: number; noindex: number; sitemap_urls: number; redirects: number
  issues: { missing_title: number; missing_meta: number; duplicate_titles: number; duplicate_descriptions: number }
}
interface SeoPage {
  id: number; slug: string; title: string | null; meta_description: string | null
  noindex: number; published: number; canonical_url: string | null; og_image: string | null
  issues: { missing_title: boolean; missing_meta: boolean; duplicate_title: boolean; duplicate_meta: boolean }
}
interface Redirect { id: number; from_path: string; to_url: string; status: number; active: number; note: string | null }
interface Audit {
  page_count: number
  missing_title: string[]; missing_meta: string[]; duplicate_titles: { title: string; count: number }[]
  missing_h1: string[]; missing_canonical: string[]; missing_structured_data: string[]
  images_missing_alt: { page: string; images: number }[]
  broken_internal_links: { page: string; targets: string[] }[]
  redirect_chains: { from: string; to: string }[]
}

const TABS = ['Overview', 'Page SEO', 'Redirects', 'Search engines', 'Audit'] as const

export default function Seo() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Overview')
  return (
    <div className="page">
      <PageHeader
        title="SEO"
        subtitle="Search visibility for the public website — metadata health, canonical URLs, managed redirects and a practical site audit. Content itself is edited under Pages & content."
      />
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => (
          <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>
        ))}
      </div>
      {tab === 'Overview' && <OverviewTab />}
      {tab === 'Page SEO' && <PagesTab />}
      {tab === 'Redirects' && <RedirectsTab />}
      {tab === 'Search engines' && <EnginesTab />}
      {tab === 'Audit' && <AuditTab />}
    </div>
  )
}

function OverviewTab() {
  const { data, loading, error } = useAdminQuery<Overview>('/api/admin/seo/overview')
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null
  const iss = data.issues
  const stat = (n: number, k: string, warn = false) => (
    <Card><div><div style={{ fontSize: '1.6rem', fontWeight: 700, color: warn && n > 0 ? 'var(--err,#b91c1c)' : undefined }}>{n}</div><div className="muted small">{k}</div></div></Card>
  )
  return (
    <>
      <p className="muted small">Canonical host: <strong>{data.canonical_host}</strong> · robots.txt and sitemap.xml are generated automatically (published, indexable pages only).</p>
      <div className="grid cols-3" style={{ display: 'grid', gap: '.8rem', gridTemplateColumns: 'repeat(auto-fit,minmax(150px,1fr))' }}>
        {stat(data.pages, 'Pages')}
        {stat(data.published, 'Published')}
        {stat(data.sitemap_urls, 'In sitemap')}
        {stat(data.noindex, 'Noindex')}
        {stat(data.redirects, 'Redirects')}
        {stat(iss.missing_title + iss.missing_meta, 'Missing metadata', true)}
        {stat(iss.duplicate_titles, 'Duplicate titles', true)}
        {stat(iss.duplicate_descriptions, 'Duplicate descriptions', true)}
      </div>
    </>
  )
}

function PagesTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: SeoPage[] }>('/api/admin/seo/pages')
  const [q, setQ] = useState('')
  const [onlyIssues, setOnlyIssues] = useState(false)
  const [edit, setEdit] = useState<SeoPage | null>(null)
  const rows = useMemo(() => {
    let r = data?.rows ?? []
    if (q) r = r.filter((p) => p.slug.includes(q) || (p.title ?? '').toLowerCase().includes(q.toLowerCase()))
    if (onlyIssues) r = r.filter((p) => p.issues.missing_title || p.issues.missing_meta || p.issues.duplicate_title || p.issues.duplicate_meta)
    return r
  }, [data, q, onlyIssues])
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  return (
    <Card title={`Page SEO (${rows.length})`}>
      <div className="row" style={{ gap: '.6rem', marginBottom: '.7rem', flexWrap: 'wrap' }}>
        <input placeholder="Filter by slug or title…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 260 }} />
        <label className="row" style={{ gap: '.35rem' }}><input type="checkbox" checked={onlyIssues} onChange={(e) => setOnlyIssues(e.target.checked)} style={{ width: 'auto' }} /> issues only</label>
      </div>
      {rows.length === 0 ? <Empty>No pages match.</Empty> : (
        <table className="data">
          <thead><tr><th>Page</th><th>Title</th><th>Description</th><th>Flags</th><th></th></tr></thead>
          <tbody>
            {rows.map((p) => (
              <tr key={p.id}>
                <td className="small">{p.slug}</td>
                <td className="small" style={{ maxWidth: 260 }}>{p.title || <span className="muted">—</span>}</td>
                <td className="small muted" style={{ maxWidth: 300 }}>{(p.meta_description || '').slice(0, 90) || '—'}</td>
                <td>
                  {p.issues.missing_title && <Badge tone="err">no title</Badge>}{' '}
                  {p.issues.missing_meta && <Badge tone="err">no description</Badge>}{' '}
                  {p.issues.duplicate_title && <Badge tone="warn">dup title</Badge>}{' '}
                  {p.issues.duplicate_meta && <Badge tone="warn">dup desc</Badge>}{' '}
                  {!!p.noindex && <Badge tone="brand">noindex</Badge>}{' '}
                  {!p.published && <Badge tone="warn">unpublished</Badge>}
                </td>
                <td><button className="btn ghost sm" onClick={() => setEdit(p)}>Edit</button></td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {edit && <EditDrawer page={edit} onClose={() => setEdit(null)} onSaved={() => { setEdit(null); refetch() }} />}
    </Card>
  )
}

function EditDrawer({ page, onClose, onSaved }: { page: SeoPage; onClose: () => void; onSaved: () => void }) {
  const [title, setTitle] = useState(page.title ?? '')
  const [meta, setMeta] = useState(page.meta_description ?? '')
  const [canonical, setCanonical] = useState(page.canonical_url ?? '')
  const [ogImage, setOgImage] = useState(page.og_image ?? '')
  const [noindex, setNoindex] = useState(!!page.noindex)
  const [saving, setSaving] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  async function save() {
    setSaving(true); setErr(null)
    try {
      await adminApi.patch(`/api/admin/pages/${page.id}`, {
        title, meta_description: meta, canonical_url: canonical, og_image: ogImage, noindex,
      })
      onSaved()
    } catch (e) { setErr((e as Error).message) } finally { setSaving(false) }
  }
  return (
    <div style={{ marginTop: '.9rem', borderTop: '1px solid var(--line,#e2e8f0)', paddingTop: '.9rem' }}>
      <h3 style={{ marginBottom: '.5rem' }}>SEO — {page.slug}</h3>
      <div style={{ display: 'grid', gap: '.55rem', maxWidth: 640 }}>
        <label>SEO title ({title.length} chars)<input value={title} onChange={(e) => setTitle(e.target.value)} /></label>
        <label>Meta description ({meta.length} chars)<textarea rows={2} value={meta} onChange={(e) => setMeta(e.target.value)} /></label>
        <label>Canonical URL override <span className="muted small">(blank = automatic)</span><input value={canonical} placeholder="https://projectcontrolsinstitute.org/…" onChange={(e) => setCanonical(e.target.value)} /></label>
        <label>Social image (og:image) override<input value={ogImage} placeholder="https://…/image.jpg" onChange={(e) => setOgImage(e.target.value)} /></label>
        <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={noindex} onChange={(e) => setNoindex(e.target.checked)} style={{ width: 'auto' }} /> Hide from search engines (noindex + out of sitemap)</label>
      </div>
      {err && <div className="notice err" style={{ marginTop: '.5rem' }}>{err}</div>}
      <div className="row" style={{ gap: '.5rem', marginTop: '.7rem' }}>
        <button className="btn" disabled={saving} onClick={save}>{saving ? 'Saving…' : 'Save'}</button>
        <button className="btn ghost" onClick={onClose}>Cancel</button>
      </div>
    </div>
  )
}

function RedirectsTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Redirect[] }>('/api/admin/seo/redirects')
  const [from, setFrom] = useState(''); const [to, setTo] = useState(''); const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false); const [err, setErr] = useState<string | null>(null)
  async function add() {
    setBusy(true); setErr(null)
    try { await adminApi.post('/api/admin/seo/redirects', { from_path: from, to_url: to, note }); setFrom(''); setTo(''); setNote(''); refetch() }
    catch (e) { setErr((e as Error).message) } finally { setBusy(false) }
  }
  async function del(id: number) {
    if (!confirm('Remove this redirect?')) return
    setErr(null)
    try { await adminApi.post(`/api/admin/seo/redirects/${id}/delete`); refetch() }
    catch (e) { setErr((e as Error).message) }
  }
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  return (
    <Card title="Managed redirects">
      <p className="muted small" style={{ marginTop: 0 }}>Permanent (301) redirects for renamed or retired pages, applied server-side. Chains and loops are rejected — always point at the final URL. Domain-level redirects (www / pciglobal.ai) are automatic and not listed here.</p>
      <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', marginBottom: '.8rem' }}>
        <input placeholder="/old-page.html" value={from} onChange={(e) => setFrom(e.target.value)} style={{ maxWidth: 220 }} />
        <span className="muted">→</span>
        <input placeholder="/new-page.html or https://…" value={to} onChange={(e) => setTo(e.target.value)} style={{ maxWidth: 260 }} />
        <input placeholder="note (optional)" value={note} onChange={(e) => setNote(e.target.value)} style={{ maxWidth: 200 }} />
        <button className="btn sm" disabled={busy || !from || !to} onClick={add}>{busy ? 'Adding…' : 'Add redirect'}</button>
      </div>
      {err && <div className="notice err" style={{ marginBottom: '.6rem' }}>{err}</div>}
      {(data?.rows.length ?? 0) === 0 ? <Empty>No managed redirects yet.</Empty> : (
        <table className="data">
          <thead><tr><th>From</th><th>To</th><th>Status</th><th>Note</th><th></th></tr></thead>
          <tbody>
            {data!.rows.map((r) => (
              <tr key={r.id}>
                <td className="small">{r.from_path}</td>
                <td className="small">{r.to_url}</td>
                <td><Badge tone="brand">{r.status}</Badge></td>
                <td className="small muted">{r.note || '—'}</td>
                <td><button className="btn ghost sm" onClick={() => del(r.id)}>Delete</button></td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </Card>
  )
}

interface Integrations {
  ga4_measurement_id: string; gtm_container_id: string; clarity_project_id: string
  google_site_verification: string; bing_site_verification: string; psi_has_key: boolean
  indexnow_key: string; indexnow_key_url: string
  submissions: { engine: string; url_count: number; status: string; detail: string; created_at: string }[]
}

function EnginesTab() {
  const { data, loading, error, refetch } = useAdminQuery<Integrations>('/api/admin/seo/integrations')
  const [form, setForm] = useState<Record<string, string>>({})
  const [busy, setBusy] = useState<string | null>(null)
  const [note, setNote] = useState<string | null>(null)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null
  const val = (k: keyof Integrations & string) => form[k] ?? String(data[k] ?? '')
  const field = (k: keyof Integrations & string, label: string, ph: string) => (
    <label>{label}<input value={val(k)} placeholder={ph} onChange={(e) => setForm({ ...form, [k]: e.target.value })} /></label>
  )
  async function save() {
    setBusy('save'); setNote(null)
    try { await adminApi.post('/api/admin/seo/integrations', form); setForm({}); setNote('Saved — tags apply to all public pages immediately.'); refetch() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }
  async function submitIndexNow() {
    setBusy('inx'); setNote(null)
    try {
      const r = await adminApi.post<{ ok: boolean; submitted: number; detail: string }>('/api/admin/seo/indexnow/submit', {})
      setNote(r.ok ? `IndexNow: submitted ${r.submitted} URLs (${r.detail}).` : `IndexNow failed: ${r.detail}`); refetch()
    } catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }
  async function runPsi() {
    setBusy('psi'); setNote(null)
    try {
      // A key pasted next to "Run check" would otherwise be silently ignored — persist it first.
      if (form.psi_api_key?.trim()) {
        await adminApi.post('/api/admin/seo/integrations', { psi_api_key: form.psi_api_key.trim() })
        const { psi_api_key: _dropped, ...rest } = form
        setForm(rest); refetch()
      }
      const r = await adminApi.post<{ ok: boolean; url: string; performance: number; seo: number }>('/api/admin/seo/pagespeed', { url: form.psi_url || undefined })
      setNote(`PageSpeed (${r.url}): performance ${r.performance}, SEO ${r.seo}`)
    } catch (e) { setNote(`PageSpeed: ${(e as Error).message}`) } finally { setBusy(null) }
  }
  return (
    <div style={{ display: 'grid', gap: '.9rem' }}>
      {note && <div className="notice" role="status">{note}</div>}
      <Card title="Analytics & tag IDs">
        <p className="muted small" style={{ marginTop: 0 }}>Set an ID to inject the official tag into every public page server-side; clear it to remove. Nothing is emitted while a field is empty.</p>
        <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: 'repeat(auto-fit,minmax(230px,1fr))' }}>
          {field('ga4_measurement_id', 'Google Analytics 4 (measurement ID)', 'G-XXXXXXXXXX')}
          {field('gtm_container_id', 'Google Tag Manager (container ID)', 'GTM-XXXXXXX')}
          {field('clarity_project_id', 'Microsoft Clarity (project ID)', 'abcdefghij')}
        </div>
      </Card>
      <Card title="Site verification">
        <p className="muted small" style={{ marginTop: 0 }}>Paste the token from Google Search Console / Bing Webmaster Tools ("HTML tag" method) — the meta tag is injected on every page. Then verify the property in the provider console. Search-performance imports require API credentials and are activated separately.</p>
        <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: 'repeat(auto-fit,minmax(230px,1fr))' }}>
          {field('google_site_verification', 'google-site-verification', 'token from Search Console')}
          {field('bing_site_verification', 'msvalidate.01 (Bing)', 'token from Bing Webmaster Tools')}
        </div>
      </Card>
      <div><button className="btn" disabled={busy !== null} onClick={save}>{busy === 'save' ? 'Saving…' : 'Save settings'}</button></div>

      <Card title="IndexNow" action={<Badge tone="ok">Ready</Badge>}>
        <p className="muted small" style={{ marginTop: 0 }}>Instant URL notification to Bing, Yandex and other IndexNow engines — no account needed. The site key is generated automatically and served at:<br /><code>{data.indexnow_key_url}</code></p>
        <button className="btn sm" disabled={busy !== null} onClick={submitIndexNow}>{busy === 'inx' ? 'Submitting…' : 'Submit all public URLs'}</button>
        {data.submissions.length > 0 && (
          <table className="data" style={{ marginTop: '.7rem' }}>
            <thead><tr><th>When</th><th>Engine</th><th>URLs</th><th>Status</th><th>Detail</th></tr></thead>
            <tbody>
              {data.submissions.map((s, i) => (
                <tr key={i}><td className="small">{s.created_at}</td><td>{s.engine}</td><td>{s.url_count}</td>
                  <td><Badge tone={s.status === 'submitted' ? 'ok' : 'err'}>{s.status}</Badge></td>
                  <td className="small muted">{s.detail}</td></tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      <Card title="PageSpeed Insights">
        <p className="muted small" style={{ marginTop: 0 }}>Measures a public URL via Google's API (works without a key at low volume; add an API key for higher quota{data.psi_has_key ? ' — key configured' : ''}). Note: it tests the LIVE site, so results reflect the deployed version.</p>
        <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap' }}>
          <input placeholder="https://projectcontrolsinstitute.org/" value={form.psi_url ?? ''} onChange={(e) => setForm({ ...form, psi_url: e.target.value })} style={{ maxWidth: 320 }} />
          <input type="password" placeholder={data.psi_has_key ? 'API key set — blank to keep' : 'PSI API key (optional)'} value={form.psi_api_key ?? ''} onChange={(e) => setForm({ ...form, psi_api_key: e.target.value })} style={{ maxWidth: 240 }} />
          <button className="btn sm" disabled={busy !== null} onClick={runPsi}>{busy === 'psi' ? 'Running…' : 'Run check'}</button>
        </div>
      </Card>
    </div>
  )
}

function AuditTab() {
  const [report, setReport] = useState<Audit | null>(null)
  const [running, setRunning] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  async function run() {
    setRunning(true); setErr(null)
    try { setReport(await adminApi.get<Audit>('/api/admin/seo/audit')) }
    catch (e) { setErr((e as Error).message) } finally { setRunning(false) }
  }
  const section = (title: string, items: unknown[], render: (x: never) => string) => (
    <Card title={`${title} (${items.length})`}>
      {items.length === 0 ? <p className="muted small" style={{ margin: 0 }}>None — all good.</p> : (
        <ul className="small" style={{ margin: 0, paddingLeft: '1.1rem', maxHeight: 220, overflowY: 'auto' }}>
          {items.slice(0, 200).map((x, i) => <li key={i}>{render(x as never)}</li>)}
        </ul>
      )}
    </Card>
  )
  return (
    <div style={{ display: 'grid', gap: '.9rem' }}>
      <div className="row" style={{ gap: '.6rem' }}>
        <button className="btn" disabled={running} onClick={run}>{running ? 'Auditing…' : report ? 'Re-run audit' : 'Run site audit'}</button>
        {report && <span className="muted small">{report.page_count} public pages scanned</span>}
      </div>
      {err && <ErrorNote>{err}</ErrorNote>}
      {report && (
        <>
          {section('Pages missing a title', report.missing_title, (s: string) => s)}
          {section('Pages missing a description', report.missing_meta, (s: string) => s)}
          {section('Duplicate titles', report.duplicate_titles, (d: { title: string; count: number }) => `"${d.title}" × ${d.count}`)}
          {section('Pages missing an H1', report.missing_h1, (s: string) => s)}
          {section('Pages missing a canonical tag', report.missing_canonical, (s: string) => s)}
          {section('Pages missing structured data (JSON-LD)', report.missing_structured_data, (s: string) => s)}
          {section('Images without alt text', report.images_missing_alt, (d: { page: string; images: number }) => `${d.page} — ${d.images} image(s)`)}
          {section('Broken internal links', report.broken_internal_links, (d: { page: string; targets: string[] }) => `${d.page} → ${d.targets.join(', ')}`)}
          {section('Redirect chains', report.redirect_chains, (d: { from: string; to: string }) => `${d.from} → ${d.to}`)}
        </>
      )}
    </div>
  )
}
