import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { useAdminAuth } from '../AdminAuth'
import { Card, Badge, Spinner, ErrorNote, Empty, Stat } from '../../components/ui'

// Admin Console → Content, SEO & Distribution Centre (Phase 1).
// Tabs: Dashboard (editorial pipeline), Blog posts (CMS + workflow + SEO), Taxonomy (authors +
// categories), Distribution (the Integration Capability Registry — honest per-platform status), and
// AI Studio (assist-only OpenAI/Claude drafting). Everything is dynamic; nothing is hardcoded.

interface Overview {
  posts_total: number; published: number; drafts: number; in_review: number; scheduled: number
  authors: number; categories: number
  ai: { openai: boolean; anthropic: boolean }
  recent: Array<Record<string, string | number | null>>
}
interface PostRow { id: number; slug: string; title: string; status: string; published: number; author_name?: string | null; category_name?: string | null; language?: string; updated_at?: string; version?: number }
interface Author { id: number; slug: string; name: string; title?: string; active: number }
interface Category { id: number; slug: string; name: string; active: number }
interface Capability { id: number; platform_key: string; platform: string; kind: string; capability: string; publish_mode?: string; requires_approval: number; official_api: number; connected: boolean; notes?: string; doc_url?: string }

const TABS = ['Dashboard', 'Blog posts', 'Taxonomy', 'Distribution', 'AI Studio'] as const

export default function ContentCentre() {
  const { can } = useAdminAuth()
  const [tab, setTab] = useState<(typeof TABS)[number]>('Dashboard')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Content, SEO &amp; Distribution</h1>
        <p className="muted">A dynamic blog CMS with editorial workflow, server-rendered public articles, SEO, sitemaps and syndication feeds, an honest integration capability registry, and an assist-only AI studio for PCL-AI, PFL-AI and PDL-AI content.</p>
      </div>
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => (<button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>))}
      </div>
      {tab === 'Dashboard' && <DashboardTab />}
      {tab === 'Blog posts' && <PostsTab canEdit={can('cc_edit') || can('cc_author')} canPublish={can('cc_publish')} />}
      {tab === 'Taxonomy' && <TaxonomyTab canEdit={can('cc_edit')} />}
      {tab === 'Distribution' && <DistributionTab />}
      {tab === 'AI Studio' && <AiTab />}
    </div>
  )
}

function DashboardTab() {
  const { data, loading, error } = useAdminQuery<Overview>('/api/admin/content/overview')
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return <Empty>No data</Empty>
  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <div className="statgrid" style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(140px,1fr))', gap: '.8rem' }}>
        <Stat n={data.posts_total} k="Total posts" />
        <Stat n={data.published} k="Published" />
        <Stat n={data.drafts} k="Drafts" />
        <Stat n={data.in_review} k="In review" />
        <Stat n={data.scheduled} k="Scheduled" />
        <Stat n={data.authors} k="Authors" />
        <Stat n={data.categories} k="Categories" />
      </div>
      <Card title="AI Content Studio">
        <p className="muted" style={{ margin: 0 }}>
          OpenAI: <Badge tone={data.ai.openai ? 'ok' : 'neutral'}>{data.ai.openai ? 'Configured' : 'Not configured'}</Badge>{' '}
          Anthropic (Claude): <Badge tone={data.ai.anthropic ? 'ok' : 'neutral'}>{data.ai.anthropic ? 'Configured' : 'Not configured'}</Badge>
        </p>
        <p className="muted" style={{ marginBottom: 0 }}>Assist-only. Set <code>OPENAI_API_KEY</code> / <code>ANTHROPIC_API_KEY</code> as environment variables to enable. AI never publishes on its own.</p>
      </Card>
      <Card title="Recently updated">
        {data.recent.length === 0 ? <Empty>No posts yet</Empty> : (
          <table className="tbl"><thead><tr><th>Title</th><th>Status</th><th>Updated</th></tr></thead>
            <tbody>{data.recent.map((r) => (<tr key={String(r.id)}><td>{String(r.title)}</td><td><StatusPill status={String(r.status)} /></td><td className="muted">{String(r.updated_at || '')}</td></tr>))}</tbody>
          </table>
        )}
      </Card>
    </div>
  )
}

function StatusPill({ status }: { status: string }) {
  const tone = status === 'published' ? 'ok' : status === 'withdrawn' || status === 'unpublished' ? 'err' : status.includes('review') ? 'warn' : 'neutral'
  return <Badge tone={tone as never}>{status.replace(/_/g, ' ')}</Badge>
}

function PostsTab({ canEdit, canPublish }: { canEdit: boolean; canPublish: boolean }) {
  const [statusFilter, setStatusFilter] = useState('')
  const [editing, setEditing] = useState<number | 'new' | null>(null)
  const { data, loading, error, refetch } = useAdminQuery<{ rows: PostRow[] }>('/api/admin/content/posts' + (statusFilter ? '?status=' + statusFilter : ''))
  if (editing !== null) return <PostEditor id={editing} canPublish={canPublish} onClose={() => { setEditing(null); refetch() }} />
  return (
    <Card title="Blog posts" action={canEdit ? <button className="btn sm" onClick={() => setEditing('new')}>New post</button> : null}>
      <div className="row" style={{ gap: '.4rem', marginBottom: '.6rem' }}>
        <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)}>
          <option value="">All statuses</option>
          {['draft', 'editorial_review', 'approved', 'scheduled', 'published', 'unpublished'].map((s) => <option key={s} value={s}>{s.replace(/_/g, ' ')}</option>)}
        </select>
      </div>
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? <Empty>No posts</Empty> : (
        <table className="tbl"><thead><tr><th>Title</th><th>Status</th><th>Author</th><th>Category</th><th>v</th></tr></thead>
          <tbody>{data.rows.map((p) => (
            <tr key={p.id} style={{ cursor: 'pointer' }} onClick={() => setEditing(p.id)}>
              <td>{p.title}<div className="muted" style={{ fontSize: '.8rem' }}>/blog/{p.slug}</div></td>
              <td><StatusPill status={p.status} /></td><td>{p.author_name || '—'}</td><td>{p.category_name || '—'}</td><td className="muted">{p.version}</td>
            </tr>))}</tbody>
        </table>
      )}
    </Card>
  )
}

function PostEditor({ id, canPublish, onClose }: { id: number | 'new'; canPublish: boolean; onClose: () => void }) {
  const isNew = id === 'new'
  const { data, loading } = useAdminQuery<{ post: Record<string, string | number | null>; tags: string[]; versions: Array<Record<string, string | number | null>>; public_url: string; seo: Array<{ code: string; severity: string; message: string }> }>(isNew ? null : `/api/admin/content/posts/${id}`)
  const authors = useAdminQuery<{ rows: Author[] }>('/api/admin/content/authors')
  const cats = useAdminQuery<{ rows: Category[] }>('/api/admin/content/categories')
  const [f, setF] = useState<Record<string, string>>({})
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState('')
  const post = data?.post
  const val = (k: string) => (k in f ? f[k] : String(post?.[k] ?? ''))
  const set = (k: string) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })

  async function save(): Promise<number | null> {
    setBusy(true); setMsg('')
    try {
      const body: Record<string, unknown> = { ...f }
      if (isNew) { const r = await adminApi.post<{ id: number }>('/api/admin/content/posts', body); setMsg('Created'); setBusy(false); return r.id }
      await adminApi.patch(`/api/admin/content/posts/${id}`, body); setMsg('Saved'); setBusy(false); return id as number
    } catch (e) { setMsg((e as Error).message); setBusy(false); return null }
  }
  async function act(path: string, extra?: Record<string, unknown>) {
    setBusy(true); setMsg('')
    try { await adminApi.post(`/api/admin/content/posts/${id}${path}`, extra || {}); setMsg('Done'); setBusy(false) }
    catch (e) { setMsg((e as Error).message); setBusy(false) }
  }

  if (!isNew && loading) return <Spinner />
  return (
    <Card title={isNew ? 'New post' : `Edit: ${post?.title || ''}`} action={<button className="btn sm ghost" onClick={onClose}>← Back</button>}>
      <div style={{ display: 'grid', gap: '.7rem', maxWidth: 780 }}>
        <label>Title<input value={val('title')} onChange={set('title')} /></label>
        <label>Subtitle<input value={val('subtitle')} onChange={set('subtitle')} /></label>
        <label>Summary<textarea rows={2} value={val('summary')} onChange={set('summary')} /></label>
        <div className="row" style={{ gap: '.6rem' }}>
          <label style={{ flex: 1 }}>Author
            <select value={val('author_id')} onChange={set('author_id')}>
              <option value="">—</option>{authors.data?.rows.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
            </select></label>
          <label style={{ flex: 1 }}>Category
            <select value={val('category_id')} onChange={set('category_id')}>
              <option value="">—</option>{cats.data?.rows.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select></label>
          <label style={{ width: 120 }}>Format
            <select value={val('body_format') || 'html'} onChange={set('body_format')}><option value="html">HTML</option><option value="markdown">Markdown</option></select></label>
        </div>
        <label>Body<textarea rows={12} value={val('body')} onChange={set('body')} style={{ fontFamily: 'monospace', fontSize: '.85rem' }} /></label>
        <label>Featured image URL<input value={val('featured_image')} onChange={set('featured_image')} placeholder="/assets/..." /></label>
        <label>Featured image alt text<input value={val('featured_image_alt')} onChange={set('featured_image_alt')} /></label>
        <details><summary style={{ cursor: 'pointer', fontWeight: 600 }}>SEO</summary>
          <div style={{ display: 'grid', gap: '.5rem', marginTop: '.5rem' }}>
            <label>SEO title<input value={val('seo_title')} onChange={set('seo_title')} /></label>
            <label>Meta description<textarea rows={2} value={val('meta_description')} onChange={set('meta_description')} /></label>
            <label>Primary keyword<input value={val('primary_keyword')} onChange={set('primary_keyword')} /></label>
            <label>Canonical URL (leave blank for the default)<input value={val('canonical_url')} onChange={set('canonical_url')} /></label>
          </div>
        </details>
        {!isNew && data && data.seo.length > 0 && (
          <div className="muted" style={{ fontSize: '.85rem' }}>SEO checks: {data.seo.map((s) => <Badge key={s.code} tone={s.severity === 'high' ? 'err' : s.severity === 'medium' ? 'warn' : 'neutral' as never}>{s.message}</Badge>)}</div>
        )}
        {msg && <div className="muted">{msg}</div>}
        <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap' }}>
          <button className="btn sm" disabled={busy} onClick={async () => { const nid = await save(); if (isNew && nid) onClose() }}>Save</button>
          {!isNew && <button className="btn sm ghost" disabled={busy} onClick={() => act('/submit', { stage: 'editorial_review' })}>Submit for review</button>}
          {!isNew && canPublish && post?.status !== 'published' && <button className="btn sm" disabled={busy} onClick={() => act('/publish')}>Publish</button>}
          {!isNew && canPublish && post?.status === 'published' && <button className="btn sm ghost" disabled={busy} onClick={() => act('/unpublish')}>Unpublish</button>}
          {!isNew && post?.published === 1 && <a className="btn sm ghost" href={data?.public_url} target="_blank" rel="noreferrer">View live ↗</a>}
        </div>
        {!isNew && data && data.versions.length > 0 && (
          <details><summary style={{ cursor: 'pointer' }}>Version history ({data.versions.length}) — nothing is ever overwritten</summary>
            <table className="tbl" style={{ marginTop: '.4rem' }}><thead><tr><th>v</th><th>Status</th><th>Reason</th><th>When</th></tr></thead>
              <tbody>{data.versions.map((v) => <tr key={String(v.id)}><td>{String(v.version)}</td><td>{String(v.status_at || '')}</td><td>{String(v.change_reason || '')}</td><td className="muted">{String(v.created_at || '')}</td></tr>)}</tbody></table>
          </details>
        )}
      </div>
    </Card>
  )
}

function TaxonomyTab({ canEdit }: { canEdit: boolean }) {
  const authors = useAdminQuery<{ rows: Author[] }>('/api/admin/content/authors')
  const cats = useAdminQuery<{ rows: Category[] }>('/api/admin/content/categories')
  const [aName, setAName] = useState(''); const [cName, setCName] = useState('')
  return (
    <div style={{ display: 'grid', gap: '1rem', gridTemplateColumns: 'repeat(auto-fit,minmax(320px,1fr))' }}>
      <Card title="Authors">
        {canEdit && <div className="row" style={{ gap: '.4rem', marginBottom: '.6rem' }}>
          <input placeholder="New author name" value={aName} onChange={(e) => setAName(e.target.value)} />
          <button className="btn sm" onClick={async () => { if (aName.trim()) { await adminApi.post('/api/admin/content/authors', { name: aName }); setAName(''); authors.refetch() } }}>Add</button>
        </div>}
        {authors.loading ? <Spinner /> : (authors.data?.rows.length ? <ul>{authors.data.rows.map((a) => <li key={a.id}>{a.name} {a.title ? <span className="muted">— {a.title}</span> : null} {a.active ? null : <Badge tone={'neutral' as never}>inactive</Badge>}</li>)}</ul> : <Empty>No authors</Empty>)}
      </Card>
      <Card title="Categories">
        {canEdit && <div className="row" style={{ gap: '.4rem', marginBottom: '.6rem' }}>
          <input placeholder="New category name" value={cName} onChange={(e) => setCName(e.target.value)} />
          <button className="btn sm" onClick={async () => { if (cName.trim()) { await adminApi.post('/api/admin/content/categories', { name: cName }); setCName(''); cats.refetch() } }}>Add</button>
        </div>}
        {cats.loading ? <Spinner /> : (cats.data?.rows.length ? <ul>{cats.data.rows.map((c) => <li key={c.id}>{c.name} <span className="muted">/blog/category/{c.slug}</span></li>)}</ul> : <Empty>No categories</Empty>)}
      </Card>
    </div>
  )
}

function capTone(cap: string): string {
  if (cap.startsWith('Direct Publishing Available') || cap === 'Read Only') return 'ok'
  if (cap.includes('Requires Approval') || cap === 'Under Review' || cap === 'Integration Under Review') return 'warn'
  if (cap === 'Unsupported' || cap === 'Temporarily Unavailable') return 'err'
  return 'neutral'
}

function DistributionTab() {
  const { data, loading, error } = useAdminQuery<{ rows: Capability[] }>('/api/admin/content/capabilities')
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows || []
  const kinds = [...new Set(rows.map((r) => r.kind))]
  const kindLabel: Record<string, string> = { search: 'Search & indexing', ai_discovery: 'AI answer engines', ai_provider: 'AI content providers', social: 'Social publishing', syndication: 'Content syndication', import: 'External import', analytics: 'Analytics & backlinks', comms: 'Newsletter & messaging' }
  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <p className="muted" style={{ margin: 0 }}>Every destination is classified honestly. There is no single API that publishes everywhere — many platforms need an account type, app review, audit or OAuth first. This registry never implies a connection that does not exist.</p>
      {kinds.map((k) => (
        <Card key={k} title={kindLabel[k] || k}>
          <table className="tbl"><thead><tr><th>Platform</th><th>Capability</th><th>State</th><th>Notes</th></tr></thead>
            <tbody>{rows.filter((r) => r.kind === k).map((r) => (
              <tr key={r.id}>
                <td>{r.platform}{r.doc_url ? <> <a href={r.doc_url} target="_blank" rel="noreferrer" className="muted">docs↗</a></> : null}</td>
                <td><Badge tone={capTone(r.capability) as never}>{r.capability}</Badge></td>
                <td>{r.connected ? <Badge tone={'ok' as never}>Connected</Badge> : r.requires_approval ? <Badge tone={'warn' as never}>Approval required</Badge> : <Badge tone={'neutral' as never}>Not connected</Badge>}</td>
                <td className="muted" style={{ fontSize: '.82rem', maxWidth: 360 }}>{r.notes}</td>
              </tr>))}</tbody>
          </table>
        </Card>
      ))}
    </div>
  )
}

function AiTab() {
  const { data } = useAdminQuery<{ openai: boolean; anthropic: boolean; note: string }>('/api/admin/content/ai/status')
  const [provider, setProvider] = useState('openai')
  const [useCase, setUseCase] = useState('draft')
  const [prompt, setPrompt] = useState('')
  const [out, setOut] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState('')
  const ready = provider === 'openai' ? data?.openai : data?.anthropic
  async function gen() {
    setBusy(true); setErr(''); setOut('')
    try { const r = await adminApi.post<{ text: string }>('/api/admin/content/ai/generate', { provider, use_case: useCase, prompt }); setOut(r.text) }
    catch (e) { setErr((e as Error).message) } finally { setBusy(false) }
  }
  return (
    <Card title="AI Content Studio">
      <p className="muted">{data?.note || 'AI assists with drafting, editing, SEO and social variants. It never publishes — every output is reviewed by a human.'}</p>
      <div className="row" style={{ gap: '.5rem', marginBottom: '.5rem', flexWrap: 'wrap' }}>
        <label>Provider <select value={provider} onChange={(e) => setProvider(e.target.value)}>
          <option value="openai">OpenAI {data?.openai ? '' : '(not configured)'}</option>
          <option value="anthropic">Claude {data?.anthropic ? '' : '(not configured)'}</option></select></label>
        <label>Use case <select value={useCase} onChange={(e) => setUseCase(e.target.value)}>
          {['draft', 'outline', 'seo_meta', 'social', 'translate', 'citation_check'].map((u) => <option key={u} value={u}>{u.replace(/_/g, ' ')}</option>)}</select></label>
      </div>
      {!ready && <ErrorNote>This provider is not configured. Set the {provider === 'openai' ? 'OPENAI_API_KEY' : 'ANTHROPIC_API_KEY'} environment variable to enable it.</ErrorNote>}
      <label>Prompt / brief<textarea rows={4} value={prompt} onChange={(e) => setPrompt(e.target.value)} /></label>
      <div style={{ marginTop: '.5rem' }}><button className="btn sm" disabled={busy || !prompt.trim()} onClick={gen}>{busy ? 'Generating…' : 'Generate (assist)'}</button></div>
      {err && <ErrorNote>{err}</ErrorNote>}
      {out && (<div style={{ marginTop: '.6rem' }}><Badge tone={'warn' as never}>AI-assisted draft — review before use</Badge><textarea rows={10} value={out} readOnly style={{ width: '100%', marginTop: '.4rem' }} /></div>)}
    </Card>
  )
}
