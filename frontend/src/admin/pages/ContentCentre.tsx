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

const TABS = ['Dashboard', 'Blog posts', 'Taxonomy', 'Social', 'Syndication', 'Import', 'Backlinks', 'Analytics', 'Distribution', 'AI Studio'] as const

export default function ContentCentre() {
  const { can, me } = useAdminAuth()
  const [tab, setTab] = useState<(typeof TABS)[number]>('Dashboard')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Content, SEO &amp; Distribution</h1>
        <p className="muted">A dynamic blog CMS with editorial workflow, server-rendered public articles, SEO, sitemaps and syndication feeds, an honest integration capability registry, and an assist-only AI studio for PCL-AI, PFL-AI and PML-AI content.</p>
      </div>
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => (<button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>))}
      </div>
      {tab === 'Dashboard' && <DashboardTab />}
      {tab === 'Blog posts' && <PostsTab canEdit={can('cc_edit') || can('cc_author')} canPublish={can('cc_publish')} canReview={can('cc_review')} canArchive={can('cc_archive')} canDelete={can('cc_delete')} canLinks={can('cc_links')} isOwner={!!me?.is_owner} />}
      {tab === 'Taxonomy' && <TaxonomyTab canEdit={can('cc_edit')} />}
      {tab === 'Social' && <SocialTab canSocial={can('cc_social')} />}
      {tab === 'Syndication' && <SyndicationTab canSyndicate={can('cc_syndicate')} />}
      {tab === 'Import' && <ImportTab canReview={can('cc_review')} canLegal={can('cc_legal')} />}
      {tab === 'Backlinks' && <BacklinksTab canManage={can('cc_backlinks')} />}
      {tab === 'Analytics' && <AnalyticsTab canManage={can('cc_seo')} />}
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
  const ok = ['published', 'live', 'agreed']
  const err = ['withdrawn', 'unpublished', 'lost', 'removed', 'redirected', 'unreachable', 'declined', 'rejected']
  const tone = ok.includes(status) ? 'ok' : err.includes(status) ? 'err' : status.includes('review') ? 'warn' : 'neutral'
  return <Badge tone={tone as never}>{status.replace(/_/g, ' ')}</Badge>
}

interface PostPerms { canEdit: boolean; canPublish: boolean; canReview: boolean; canArchive: boolean; canDelete: boolean; canLinks: boolean; isOwner: boolean }
function PostsTab(perms: PostPerms) {
  const { canEdit } = perms
  const [statusFilter, setStatusFilter] = useState('')
  const [editing, setEditing] = useState<number | 'new' | null>(null)
  const { data, loading, error, refetch } = useAdminQuery<{ rows: PostRow[] }>('/api/admin/content/posts' + (statusFilter ? '?status=' + statusFilter : ''))
  if (editing !== null) return <PostEditor id={editing} perms={perms} onClose={() => { setEditing(null); refetch() }} />
  return (
    <Card title="Blog &amp; news posts" action={canEdit ? <button className="btn sm" onClick={() => setEditing('new')}>New post</button> : null}>
      <div className="row" style={{ gap: '.4rem', marginBottom: '.6rem' }}>
        <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)}>
          <option value="">All statuses</option>
          {['draft', 'editorial_review', 'approved', 'scheduled', 'published', 'unpublished', 'archived', 'deleted'].map((s) => <option key={s} value={s}>{s.replace(/_/g, ' ')}</option>)}
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

function PostEditor({ id, perms, onClose }: { id: number | 'new'; perms: PostPerms; onClose: () => void }) {
  const isNew = id === 'new'
  const { data, loading, refetch } = useAdminQuery<{ post: Record<string, string | number | null>; tags: string[]; versions: Array<Record<string, string | number | null>>; public_url: string; seo: Array<{ code: string; severity: string; message: string }> }>(isNew ? null : `/api/admin/content/posts/${id}`)
  const authors = useAdminQuery<{ rows: Author[] }>('/api/admin/content/authors')
  const cats = useAdminQuery<{ rows: Category[] }>('/api/admin/content/categories')
  const [f, setF] = useState<Record<string, string>>({})
  const [tags, setTags] = useState<string | null>(null)   // null = untouched; a string once the editor edits it
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState('')
  const post = data?.post
  const status = String(post?.status ?? '')
  const val = (k: string) => (k in f ? f[k] : String(post?.[k] ?? ''))
  const set = (k: string) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })
  const tagsVal = tags !== null ? tags : (data?.tags || []).join(', ')

  async function save(): Promise<number | null> {
    setBusy(true); setMsg('')
    try {
      const body: Record<string, unknown> = { ...f }
      let outId: number
      if (isNew) { const r = await adminApi.post<{ id: number }>('/api/admin/content/posts', body); outId = r.id }
      else { await adminApi.patch(`/api/admin/content/posts/${id}`, body); outId = id as number }
      if (!isNew && tags !== null) await adminApi.post(`/api/admin/content/posts/${outId}/tags`, { tags: tags.split(',').map((t) => t.trim()).filter(Boolean) })
      setMsg('Saved'); setBusy(false); return outId
    } catch (e) { setMsg((e as Error).message); setBusy(false); return null }
  }
  async function act(path: string, extra?: Record<string, unknown>) {
    setBusy(true); setMsg('')
    try { await adminApi.post(`/api/admin/content/posts/${id}${path}`, extra || {}); setMsg('Done'); setBusy(false); if (!isNew) refetch() }
    catch (e) { setMsg((e as Error).message); setBusy(false) }
  }
  async function confirmAct(path: string, prompt: string) { if (window.confirm(prompt)) await act(path) }
  const inReview = ['author_review', 'editorial_review', 'technical_review', 'seo_review', 'legal_review', 'changes_requested'].includes(status)

  if (!isNew && loading) return <Spinner />
  return (
    <Card title={isNew ? 'New post' : `Edit: ${post?.title || ''}`} action={<button className="btn sm ghost" onClick={onClose}>← Back</button>}>
      <div style={{ display: 'grid', gap: '.7rem', maxWidth: 780 }}>
        <div className="row" style={{ gap: '.6rem' }}>
          <label style={{ flex: 2 }}>Title<input value={val('title')} onChange={set('title')} /></label>
          <label style={{ width: 170 }}>Type
            <select value={val('structured_type') || 'BlogPosting'} onChange={set('structured_type')}>
              <option value="BlogPosting">Blog post</option><option value="NewsArticle">News article</option><option value="Article">Article</option>
            </select></label>
        </div>
        {!isNew && <label>Slug (URL)<input value={val('slug')} onChange={set('slug')} placeholder="my-post" /><span className="muted" style={{ fontSize: '.78rem' }}>Renaming a live post keeps the old URL working via a 301.</span></label>}
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
        {!isNew && <label>Tags (comma-separated)<input value={tagsVal} onChange={(e) => setTags(e.target.value)} placeholder="AI, EVM, forecasting" /></label>}
        <label>Body<textarea rows={12} value={val('body')} onChange={set('body')} style={{ fontFamily: 'monospace', fontSize: '.85rem' }} /></label>
        <label>Featured image URL<input value={val('featured_image')} onChange={set('featured_image')} placeholder="/assets/..." /></label>
        <label>Featured image alt text<input value={val('featured_image_alt')} onChange={set('featured_image_alt')} /></label>
        <details><summary style={{ cursor: 'pointer', fontWeight: 600 }}>SEO</summary>
          <div style={{ display: 'grid', gap: '.5rem', marginTop: '.5rem' }}>
            <label>SEO title<input value={val('seo_title')} onChange={set('seo_title')} /></label>
            <label>Meta description<textarea rows={2} value={val('meta_description')} onChange={set('meta_description')} /></label>
            <label>Primary keyword<input value={val('primary_keyword')} onChange={set('primary_keyword')} /></label>
            <label>Secondary keywords (comma-separated)<input value={val('secondary_keywords')} onChange={set('secondary_keywords')} /></label>
            <label>Canonical URL (leave blank for the default)<input value={val('canonical_url')} onChange={set('canonical_url')} /></label>
          </div>
        </details>
        {!isNew && data && data.seo.length > 0 && (
          <div className="muted" style={{ fontSize: '.85rem' }}>SEO checks: {data.seo.map((s) => <Badge key={s.code} tone={s.severity === 'high' ? 'err' : s.severity === 'medium' ? 'warn' : 'neutral' as never}>{s.message}</Badge>)}</div>
        )}
        {msg && <div className="muted">{msg}</div>}
        <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap' }}>
          <button className="btn sm" disabled={busy} onClick={async () => { const nid = await save(); if (isNew && nid) onClose(); else refetch() }}>Save</button>
          {!isNew && <button className="btn sm ghost" disabled={busy} onClick={() => act('/submit', { stage: 'editorial_review' })}>Submit for review</button>}
          {!isNew && perms.canReview && inReview && <>
            <button className="btn sm" disabled={busy} onClick={() => act('/review', { decision: 'approved' })}>Approve</button>
            <button className="btn sm ghost" disabled={busy} onClick={() => act('/review', { decision: 'changes_requested' })}>Request changes</button>
            <button className="btn sm ghost" disabled={busy} onClick={() => act('/review', { decision: 'rejected' })}>Reject</button>
          </>}
          {!isNew && perms.canPublish && status !== 'published' && <button className="btn sm" disabled={busy} onClick={() => act('/publish')}>Publish</button>}
          {!isNew && perms.canPublish && status === 'published' && <button className="btn sm ghost" disabled={busy} onClick={() => act('/unpublish')}>Unpublish</button>}
          {!isNew && post?.published === 1 && <a className="btn sm ghost" href={data?.public_url} target="_blank" rel="noreferrer">View live ↗</a>}
          {!isNew && perms.canArchive && status !== 'archived' && status !== 'deleted' && <button className="btn sm ghost" disabled={busy} onClick={() => confirmAct('/archive', 'Archive this post? It leaves the public site but keeps its full history.')}>Archive</button>}
          {!isNew && perms.canDelete && status !== 'deleted' && <button className="btn sm ghost" disabled={busy} style={{ color: '#b91c1c' }} onClick={() => confirmAct('/delete', 'Soft-delete this post? It is hidden but recoverable, and its version history is kept.')}>Delete</button>}
          {!isNew && perms.isOwner && <button className="btn sm ghost" disabled={busy} style={{ color: '#b91c1c' }} onClick={() => confirmAct('/purge', 'PERMANENTLY delete this post and its entire history? This cannot be undone.')}>Purge</button>}
        </div>
        {!isNew && data && data.versions.length > 0 && (
          <details><summary style={{ cursor: 'pointer' }}>Version history ({data.versions.length}) — nothing is ever overwritten</summary>
            <table className="tbl" style={{ marginTop: '.4rem' }}><thead><tr><th>v</th><th>Status</th><th>Reason</th><th>When</th>{perms.canEdit ? <th></th> : null}</tr></thead>
              <tbody>{data.versions.map((v) => <tr key={String(v.id)}><td>{String(v.version)}</td><td>{String(v.status_at || '')}</td><td>{String(v.change_reason || '')}</td><td className="muted">{String(v.created_at || '')}</td>{perms.canEdit ? <td><button className="btn sm ghost" disabled={busy} onClick={() => confirmAct(`/restore/${v.version}`, `Restore version ${v.version}? The current content is snapshotted first, so nothing is lost.`)}>Restore</button></td> : null}</tr>)}</tbody></table>
          </details>
        )}
        {!isNew && perms.canLinks && <LinksPanel postId={id as number} />}
      </div>
    </Card>
  )
}

interface LinkRow { id: number; url: string; kind: string; anchor_text?: string; rel: string; is_citation: number; approved: number; status: string; http_code?: number | null; last_checked_at?: string | null; clicks?: number; active: number }
function LinksPanel({ postId }: { postId: number }) {
  const { data, loading, refetch } = useAdminQuery<{ rows: LinkRow[] }>(`/api/admin/content/posts/${postId}/links`)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState('')
  async function scan() { setBusy(true); setMsg(''); try { const r = await adminApi.post<{ found: number; added: number }>(`/api/admin/content/posts/${postId}/links/scan`, {}); setMsg(`Scanned: ${r.found} link(s), ${r.added} new`); refetch() } catch (e) { setMsg((e as Error).message) } setBusy(false) }
  async function patch(id: number, body: Record<string, unknown>) { setBusy(true); try { await adminApi.patch(`/api/admin/content/links/${id}`, body); refetch() } catch (e) { setMsg((e as Error).message) } setBusy(false) }
  async function check(id: number) { setBusy(true); try { const r = await adminApi.post<{ status: string; code: number }>(`/api/admin/content/links/${id}/check`, {}); setMsg(`Checked: ${r.status}${r.code ? ' (' + r.code + ')' : ''}`); refetch() } catch (e) { setMsg((e as Error).message) } setBusy(false) }
  const rows = (data?.rows || []).filter((r) => r.active)
  return (
    <details><summary style={{ cursor: 'pointer', fontWeight: 600 }}>Links ({rows.length}) — internal &amp; external, rel policy, citations, status</summary>
      <div className="row" style={{ gap: '.4rem', margin: '.5rem 0', alignItems: 'center' }}>
        <button className="btn sm" disabled={busy} onClick={scan}>Scan body for links</button>
        {msg && <span className="muted" style={{ fontSize: '.82rem' }}>{msg}</span>}
      </div>
      {loading ? <Spinner /> : rows.length === 0 ? <Empty>No links recorded. Scan the body to build the registry.</Empty> : (
        <table className="tbl"><thead><tr><th>URL</th><th>Kind</th><th>rel</th><th>Citation</th><th>Status</th><th>Clicks</th><th></th></tr></thead>
          <tbody>{rows.map((l) => (
            <tr key={l.id}>
              <td style={{ maxWidth: 260, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}><span title={l.url}>{l.url}</span>{l.anchor_text ? <div className="muted" style={{ fontSize: '.75rem' }}>{l.anchor_text}</div> : null}</td>
              <td><Badge tone={l.kind === 'internal' ? 'neutral' : 'ok' as never}>{l.kind}</Badge></td>
              <td><select value={l.rel} onChange={(e) => patch(l.id, { rel: e.target.value })}>{['auto', 'dofollow', 'nofollow', 'sponsored', 'ugc'].map((r) => <option key={r} value={r}>{r}</option>)}</select></td>
              <td><input type="checkbox" checked={l.is_citation === 1} onChange={(e) => patch(l.id, { is_citation: e.target.checked })} />{l.is_citation === 1 ? <span className="muted" style={{ fontSize: '.72rem' }}> {l.approved === 1 ? 'approved' : 'unapproved'}</span> : null}</td>
              <td>{l.status === 'unchecked' ? <span className="muted">—</span> : <Badge tone={l.status === 'live' ? 'ok' : l.status === 'internal' ? 'neutral' : 'err' as never}>{l.status}{l.http_code ? ' ' + l.http_code : ''}</Badge>}</td>
              <td style={{ fontVariantNumeric: 'tabular-nums' }}>{l.clicks || 0}</td>
              <td className="row" style={{ gap: '.3rem' }}>{l.kind === 'external' ? <button className="btn sm ghost" disabled={busy} onClick={() => check(l.id)}>Check</button> : null}{l.is_citation === 1 ? <button className="btn sm ghost" disabled={busy} onClick={() => patch(l.id, { approved: l.approved !== 1 })}>{l.approved === 1 ? 'Unapprove' : 'Approve'}</button> : null}</td>
            </tr>))}</tbody>
        </table>
      )}
    </details>
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

interface SocialAccount { id: number; platform_key: string; label: string; status: string; last_error?: string | null; active: number; has_secret: boolean }
interface SocialDraft { id: number; post_id: number; platform_key: string; account_id: number; text: string; status: string; public_url?: string | null; acct_label?: string | null }

const SOCIAL_FIELDS: Record<string, { secretLabel: string; extra: Array<{ key: string; label: string; ph?: string }> }> = {
  discord: { secretLabel: 'Channel webhook URL', extra: [] },
  telegram: { secretLabel: 'Bot token', extra: [{ key: 'chat_id', label: 'Channel / chat id', ph: '@yourchannel or -100…' }, { key: 'channel_username', label: 'Public channel username (optional)', ph: 'yourchannel' }] },
  mastodon: { secretLabel: 'Access token', extra: [{ key: 'instance', label: 'Instance URL', ph: 'https://mastodon.social' }] },
  bluesky: { secretLabel: 'App password', extra: [{ key: 'handle', label: 'Handle', ph: 'you.bsky.social' }] },
}

function SocialTab({ canSocial }: { canSocial: boolean }) {
  const accounts = useAdminQuery<{ rows: SocialAccount[]; live_platforms: string[] }>('/api/admin/content/social/accounts')
  const [platform, setPlatform] = useState('discord')
  const [f, setF] = useState<Record<string, string>>({})
  const [msg, setMsg] = useState('')
  const [postId, setPostId] = useState('')
  const drafts = useAdminQuery<{ rows: SocialDraft[] }>(postId ? `/api/admin/content/social/drafts?post_id=${postId}` : '/api/admin/content/social/drafts')
  const spec = SOCIAL_FIELDS[platform]

  async function connect() {
    setMsg('')
    try { await adminApi.post('/api/admin/content/social/accounts', { platform_key: platform, label: f.label || platform, secret: f.secret, ...f }); setF({}); setMsg('Connected'); accounts.refetch() }
    catch (e) { setMsg((e as Error).message) }
  }
  async function act(path: string, then?: () => void) { try { await adminApi.post(path, {}); then?.() } catch (e) { setMsg((e as Error).message) } }

  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <p className="muted" style={{ margin: 0 }}>Only the platforms whose official APIs need no provider review are connectable here (Discord, Telegram, Mastodon, Bluesky). Credentials are encrypted at rest and never shown again. LinkedIn / Meta / X / TikTok / Pinterest stay in the Distribution registry until provider approval is complete.</p>
      {canSocial && (
        <Card title="Connect an account">
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'end' }}>
            <label>Platform<select value={platform} onChange={(e) => { setPlatform(e.target.value); setF({}) }}>
              {(accounts.data?.live_platforms || ['discord', 'telegram', 'mastodon', 'bluesky']).map((p) => <option key={p} value={p}>{p}</option>)}
            </select></label>
            <label>Label<input value={f.label || ''} onChange={(e) => setF({ ...f, label: e.target.value })} placeholder="PCI Discord" /></label>
            <label style={{ minWidth: 260 }}>{spec.secretLabel}<input type="password" value={f.secret || ''} onChange={(e) => setF({ ...f, secret: e.target.value })} /></label>
            {spec.extra.map((x) => <label key={x.key}>{x.label}<input value={f[x.key] || ''} onChange={(e) => setF({ ...f, [x.key]: e.target.value })} placeholder={x.ph} /></label>)}
            <button className="btn sm" onClick={connect} disabled={!f.secret}>Connect</button>
          </div>
          {msg && <div className="muted" style={{ marginTop: '.4rem' }}>{msg}</div>}
        </Card>
      )}
      <Card title="Connected accounts">
        {accounts.loading ? <Spinner /> : !accounts.data?.rows.length ? <Empty>No accounts connected</Empty> : (
          <table className="tbl"><thead><tr><th>Platform</th><th>Label</th><th>Status</th><th></th></tr></thead>
            <tbody>{accounts.data.rows.map((a) => (
              <tr key={a.id}>
                <td>{a.platform_key}</td><td>{a.label}</td>
                <td><Badge tone={a.status === 'connected' && a.active ? 'ok' : a.status === 'error' ? 'err' : 'neutral' as never}>{a.active ? a.status : 'disconnected'}</Badge>{a.last_error ? <span className="muted" style={{ fontSize: '.78rem' }}> — {a.last_error}</span> : null}</td>
                <td>{canSocial && a.active ? <span className="row" style={{ gap: '.3rem' }}>
                  <button className="btn sm ghost" onClick={() => act(`/api/admin/content/social/accounts/${a.id}/test`, accounts.refetch)}>Test</button>
                  <button className="btn sm ghost" onClick={() => act(`/api/admin/content/social/accounts/${a.id}/disconnect`, accounts.refetch)}>Disconnect</button>
                </span> : null}</td>
              </tr>))}</tbody>
          </table>
        )}
      </Card>
      <Card title="Social drafts" action={canSocial ? <span className="row" style={{ gap: '.4rem' }}>
        <input placeholder="Post id" value={postId} onChange={(e) => setPostId(e.target.value.replace(/\D/g, ''))} style={{ width: 90 }} />
        <button className="btn sm" disabled={!postId} onClick={() => act(`/api/admin/content/posts/${postId}/social/generate`, drafts.refetch)}>Generate for post</button>
        <button className="btn sm ghost" onClick={() => act('/api/admin/content/social/drain', drafts.refetch)}>Run queue now</button>
      </span> : null}>
        {drafts.loading ? <Spinner /> : !drafts.data?.rows.length ? <Empty>No drafts</Empty> : (
          <table className="tbl"><thead><tr><th>Platform</th><th>Text</th><th>Status</th><th></th></tr></thead>
            <tbody>{drafts.data.rows.map((d) => (
              <tr key={d.id}>
                <td>{d.platform_key}<div className="muted" style={{ fontSize: '.75rem' }}>post #{d.post_id}</div></td>
                <td style={{ maxWidth: 380, fontSize: '.85rem' }}>{d.text}{d.public_url ? <div className="muted"><a href={d.public_url.startsWith('http') ? d.public_url : undefined} target="_blank" rel="noreferrer">{d.public_url}</a></div> : null}</td>
                <td><StatusPill status={d.status} /></td>
                <td>{canSocial && ['draft', 'approved', 'retrying'].includes(d.status) ? <span className="row" style={{ gap: '.3rem' }}>
                  <button className="btn sm" onClick={() => act(`/api/admin/content/social/drafts/${d.id}/publish`, drafts.refetch)}>Publish</button>
                  <button className="btn sm ghost" onClick={() => act(`/api/admin/content/social/drafts/${d.id}/cancel`, drafts.refetch)}>Cancel</button>
                </span> : null}</td>
              </tr>))}</tbody>
          </table>
        )}
      </Card>
    </div>
  )
}

interface SynDest { id: number; platform_key: string; label: string; base_url: string; mode: string; default_status: string; status: string; last_error?: string | null; active: number; has_secret: boolean }
interface SynPost { id: number; post_id: number; destination_id: number; external_url?: string | null; canonical_url?: string | null; status: string; last_error?: string | null; dest_platform?: string | null; dest_label?: string | null }

const SYN_PLATFORMS: Record<string, { label: string; secretLabel: string; extra: Array<{ key: string; label: string; ph?: string }> }> = {
  wordpress_selfhosted: { label: 'WordPress (self-hosted)', secretLabel: 'Application password', extra: [{ key: 'username', label: 'WordPress username', ph: 'editor' }] },
  ghost: { label: 'Ghost', secretLabel: 'Admin API key (id:secret)', extra: [] },
  forem_dev: { label: 'Forem / DEV', secretLabel: 'API key', extra: [] },
}

function SyndicationTab({ canSyndicate }: { canSyndicate: boolean }) {
  const dests = useAdminQuery<{ rows: SynDest[]; live_platforms: string[] }>('/api/admin/content/syndication/destinations')
  const [platform, setPlatform] = useState('wordpress_selfhosted')
  const [f, setF] = useState<Record<string, string>>({})
  const [msg, setMsg] = useState('')
  const [postId, setPostId] = useState('')
  const posts = useAdminQuery<{ rows: SynPost[] }>(postId ? `/api/admin/content/syndication/posts?post_id=${postId}` : '/api/admin/content/syndication/posts')
  const spec = SYN_PLATFORMS[platform]

  async function connect() {
    setMsg('')
    try { await adminApi.post('/api/admin/content/syndication/destinations', { platform_key: platform, base_url: f.base_url, secret: f.secret, label: f.label || spec.label, mode: f.mode || 'create', default_status: f.default_status || 'draft', ...f }); setF({}); setMsg('Connected'); dests.refetch() }
    catch (e) { setMsg((e as Error).message) }
  }
  async function act(path: string, then?: () => void) { try { await adminApi.post(path, {}); then?.() } catch (e) { setMsg((e as Error).message) } }

  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <p className="muted" style={{ margin: 0 }}>Cross-post a published article to partner CMS platforms whose official APIs need no provider onboarding (WordPress self-hosted, Ghost, Forem/DEV). Every syndicated copy sets its canonical back to the PCI original, so search engines consolidate duplicate content to the source. Credentials are encrypted at rest and never shown again.</p>
      {canSyndicate && (
        <Card title="Connect a destination">
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'end' }}>
            <label>Platform<select value={platform} onChange={(e) => { setPlatform(e.target.value); setF({}) }}>
              {(dests.data?.live_platforms || Object.keys(SYN_PLATFORMS)).map((p) => <option key={p} value={p}>{SYN_PLATFORMS[p]?.label || p}</option>)}
            </select></label>
            <label>Label<input value={f.label || ''} onChange={(e) => setF({ ...f, label: e.target.value })} placeholder={spec.label} /></label>
            <label style={{ minWidth: 220 }}>Site URL<input value={f.base_url || ''} onChange={(e) => setF({ ...f, base_url: e.target.value })} placeholder="https://blog.example.com" /></label>
            <label style={{ minWidth: 220 }}>{spec.secretLabel}<input type="password" value={f.secret || ''} onChange={(e) => setF({ ...f, secret: e.target.value })} /></label>
            {spec.extra.map((x) => <label key={x.key}>{x.label}<input value={f[x.key] || ''} onChange={(e) => setF({ ...f, [x.key]: e.target.value })} placeholder={x.ph} /></label>)}
            <label>Publish as<select value={f.default_status || 'draft'} onChange={(e) => setF({ ...f, default_status: e.target.value })}><option value="draft">Draft on destination</option><option value="published">Published</option></select></label>
            <label>Updates<select value={f.mode || 'create'} onChange={(e) => setF({ ...f, mode: e.target.value })}><option value="create">Create only</option><option value="create_update">Create + update</option></select></label>
            <button className="btn sm" onClick={connect} disabled={!f.secret || !f.base_url}>Connect</button>
          </div>
          {msg && <div className="muted" style={{ marginTop: '.4rem' }}>{msg}</div>}
        </Card>
      )}
      <Card title="Destinations">
        {dests.loading ? <Spinner /> : !dests.data?.rows.length ? <Empty>No destinations connected</Empty> : (
          <table className="tbl"><thead><tr><th>Platform</th><th>Label</th><th>Site</th><th>Status</th><th></th></tr></thead>
            <tbody>{dests.data.rows.map((d) => (
              <tr key={d.id}>
                <td>{d.platform_key}</td><td>{d.label}</td><td className="muted" style={{ fontSize: '.8rem' }}>{d.base_url}</td>
                <td><Badge tone={d.status === 'connected' && d.active ? 'ok' : d.status === 'error' ? 'err' : 'neutral' as never}>{d.active ? d.status : 'disconnected'}</Badge>{d.last_error ? <span className="muted" style={{ fontSize: '.78rem' }}> — {d.last_error}</span> : null}</td>
                <td>{canSyndicate && d.active ? <span className="row" style={{ gap: '.3rem' }}>
                  <button className="btn sm ghost" onClick={() => act(`/api/admin/content/syndication/destinations/${d.id}/test`, dests.refetch)}>Test</button>
                  <button className="btn sm ghost" onClick={() => act(`/api/admin/content/syndication/destinations/${d.id}/disconnect`, dests.refetch)}>Disconnect</button>
                </span> : null}</td>
              </tr>))}</tbody>
          </table>
        )}
      </Card>
      <Card title="Syndicated posts" action={canSyndicate ? <span className="row" style={{ gap: '.4rem' }}>
        <input placeholder="Post id" value={postId} onChange={(e) => setPostId(e.target.value.replace(/\D/g, ''))} style={{ width: 90 }} />
        <button className="btn sm" disabled={!postId} onClick={() => act(`/api/admin/content/posts/${postId}/syndicate`, posts.refetch)}>Syndicate post</button>
        <button className="btn sm ghost" onClick={() => act('/api/admin/content/syndication/drain', posts.refetch)}>Run queue now</button>
      </span> : null}>
        {posts.loading ? <Spinner /> : !posts.data?.rows.length ? <Empty>No syndicated posts</Empty> : (
          <table className="tbl"><thead><tr><th>Destination</th><th>External URL</th><th>Status</th><th></th></tr></thead>
            <tbody>{posts.data.rows.map((s) => (
              <tr key={s.id}>
                <td>{s.dest_label || s.dest_platform}<div className="muted" style={{ fontSize: '.75rem' }}>post #{s.post_id}</div></td>
                <td style={{ maxWidth: 320, fontSize: '.82rem' }}>{s.external_url ? <a href={s.external_url} target="_blank" rel="noreferrer">{s.external_url}</a> : <span className="muted">—</span>}{s.last_error ? <div className="muted">{s.last_error}</div> : null}</td>
                <td><StatusPill status={s.status} /></td>
                <td>{canSyndicate && ['failed', 'retrying'].includes(s.status) ? <button className="btn sm" onClick={() => act(`/api/admin/content/syndication/posts/${s.id}/retry`, posts.refetch)}>Retry</button> : null}</td>
              </tr>))}</tbody>
          </table>
        )}
      </Card>
    </div>
  )
}

interface ExtSource { id: number; name: string; domain?: string; feed_url?: string; license: string; allowed_use: string; permission_ref?: string; active: number; last_fetched_at?: string | null; last_error?: string | null }
interface ExtItem { id: number; source_id: number; title?: string; source_url?: string; author?: string; status: string; source_name?: string; source_domain?: string; allowed_use?: string; pci_post_id?: number | null }

const LICENSES = ['all_rights_reserved', 'permission_granted', 'cc_by', 'cc_by_sa', 'public_domain']
const USES = [{ v: 'curated_link', l: 'Curated link (summary + link)' }, { v: 'excerpt', l: 'Excerpt' }, { v: 'full', l: 'Full republication (licence required)' }]

function ImportTab({ canReview, canLegal }: { canReview: boolean; canLegal: boolean }) {
  const sources = useAdminQuery<{ rows: ExtSource[] }>('/api/admin/content/import/sources')
  const [f, setF] = useState<Record<string, string>>({})
  const [msg, setMsg] = useState('')
  const [sourceId, setSourceId] = useState('')
  const items = useAdminQuery<{ rows: ExtItem[] }>(sourceId ? `/api/admin/content/import/items?source_id=${sourceId}` : '/api/admin/content/import/items?status=retrieved')

  async function addSource() {
    setMsg('')
    try { await adminApi.post('/api/admin/content/import/sources', { name: f.name, feed_url: f.feed_url, domain: f.domain, license: f.license || 'all_rights_reserved', allowed_use: f.allowed_use || 'curated_link', permission_ref: f.permission_ref }); setF({}); sources.refetch() }
    catch (e) { setMsg((e as Error).message) }
  }
  async function act(path: string, body: Record<string, unknown>, then?: () => void) { setMsg(''); try { await adminApi.post(path, body); then?.() } catch (e) { setMsg((e as Error).message) } }

  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <p className="muted" style={{ margin: 0 }}>Import from approved RSS/Atom sources into a review queue. PCI never republishes a whole third-party article just because it is public: the default is a curated link (a PCI-written summary + a link to the original), and full republication requires a recorded licence plus elevated approval. Every imported copy carries attribution and a canonical pointing at the original.</p>
      {canReview && (
        <Card title="Add a source">
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'end' }}>
            <label>Name<input value={f.name || ''} onChange={(e) => setF({ ...f, name: e.target.value })} placeholder="PC Weekly" /></label>
            <label>Domain<input value={f.domain || ''} onChange={(e) => setF({ ...f, domain: e.target.value })} placeholder="pcweekly.example" /></label>
            <label style={{ minWidth: 240 }}>Feed URL<input value={f.feed_url || ''} onChange={(e) => setF({ ...f, feed_url: e.target.value })} placeholder="https://…/feed.xml" /></label>
            <label>Licence<select value={f.license || 'all_rights_reserved'} onChange={(e) => setF({ ...f, license: e.target.value })}>{LICENSES.map((l) => <option key={l} value={l}>{l.replace(/_/g, ' ')}</option>)}</select></label>
            <label>Allowed use<select value={f.allowed_use || 'curated_link'} onChange={(e) => setF({ ...f, allowed_use: e.target.value })}>{USES.map((u) => <option key={u.v} value={u.v}>{u.l}</option>)}</select></label>
            <label>Permission ref<input value={f.permission_ref || ''} onChange={(e) => setF({ ...f, permission_ref: e.target.value })} placeholder="MOU / licence id" /></label>
            <button className="btn sm" onClick={addSource} disabled={!f.name || !f.feed_url}>Add</button>
          </div>
          {msg && <div className="muted" style={{ marginTop: '.4rem' }}>{msg}</div>}
        </Card>
      )}
      <Card title="Sources">
        {sources.loading ? <Spinner /> : !sources.data?.rows.length ? <Empty>No sources</Empty> : (
          <table className="tbl"><thead><tr><th>Source</th><th>Licence / use</th><th>Last fetch</th><th></th></tr></thead>
            <tbody>{sources.data.rows.filter((s) => s.active).map((s) => (
              <tr key={s.id}>
                <td>{s.name}<div className="muted" style={{ fontSize: '.78rem' }}>{s.domain}</div></td>
                <td><Badge tone="neutral">{s.license.replace(/_/g, ' ')}</Badge> <span className="muted" style={{ fontSize: '.8rem' }}>{s.allowed_use.replace(/_/g, ' ')}</span></td>
                <td className="muted" style={{ fontSize: '.8rem' }}>{s.last_fetched_at || '—'}{s.last_error ? <div style={{ color: 'var(--err,#b00)' }}>{s.last_error}</div> : null}</td>
                <td>{canReview ? <span className="row" style={{ gap: '.3rem' }}>
                  <button className="btn sm" onClick={() => act(`/api/admin/content/import/sources/${s.id}/fetch`, {}, () => { items.refetch(); sources.refetch() })}>Fetch now</button>
                  <button className="btn sm ghost" onClick={() => setSourceId(String(s.id))}>Queue</button>
                  <button className="btn sm ghost" onClick={() => act(`/api/admin/content/import/sources/${s.id}/delete`, {}, sources.refetch)}>Remove</button>
                </span> : null}</td>
              </tr>))}</tbody>
          </table>
        )}
      </Card>
      <Card title="Review queue" action={sourceId ? <button className="btn sm ghost" onClick={() => setSourceId('')}>All sources</button> : null}>
        {items.loading ? <Spinner /> : !items.data?.rows.length ? <Empty>Nothing to review</Empty> : (
          <table className="tbl"><thead><tr><th>Item</th><th>Source</th><th>Status</th><th>Action</th></tr></thead>
            <tbody>{items.data.rows.map((it) => (
              <tr key={it.id}>
                <td style={{ maxWidth: 320 }}>{it.title}{it.source_url ? <div className="muted" style={{ fontSize: '.76rem' }}><a href={it.source_url} target="_blank" rel="noreferrer">{it.source_url}</a></div> : null}</td>
                <td>{it.source_name}<div className="muted" style={{ fontSize: '.75rem' }}>{(it.allowed_use || '').replace(/_/g, ' ')}</div></td>
                <td><StatusPill status={it.status} /></td>
                <td>{canReview && it.status === 'retrieved' ? <span className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                  <button className="btn sm" onClick={() => act(`/api/admin/content/import/items/${it.id}/approve`, { mode: 'link' }, items.refetch)}>Curated link</button>
                  {it.allowed_use !== 'curated_link' ? <button className="btn sm ghost" onClick={() => act(`/api/admin/content/import/items/${it.id}/approve`, { mode: 'excerpt' }, items.refetch)}>Excerpt</button> : null}
                  {it.allowed_use === 'full' && canLegal ? <button className="btn sm ghost" onClick={() => act(`/api/admin/content/import/items/${it.id}/approve`, { mode: 'full' }, items.refetch)}>Full</button> : null}
                  <button className="btn sm ghost" onClick={() => act(`/api/admin/content/import/items/${it.id}/reject`, {}, items.refetch)}>Reject</button>
                </span> : it.pci_post_id ? <span className="muted" style={{ fontSize: '.8rem' }}>→ draft #{it.pci_post_id}</span> : null}</td>
              </tr>))}</tbody>
          </table>
        )}
      </Card>
    </div>
  )
}

interface Prospect { id: number; name: string; domain?: string; url?: string; category: string; authority?: number | null; relevance: string; status: string; owner?: string; contact_name?: string; contact_email?: string; next_action_at?: string | null }
interface Backlink { id: number; prospect_id?: number | null; source_url: string; source_domain?: string; target_url?: string; anchor_text?: string; rel: string; link_type: string; status: string; discovered_via: string; last_checked_at?: string | null; last_status_code?: number | null; first_seen_at?: string | null }
interface BLOverview { prospects: Array<{ status: string; n: number }>; links: Array<{ status: string; n: number }>; follow_ups_due: number; live: number; lost: number }

const PROSPECT_STATUSES = ['prospect', 'contacted', 'in_talks', 'agreed', 'live', 'declined', 'dormant']
const LINK_REL = ['unknown', 'dofollow', 'nofollow', 'ugc', 'sponsored']

function BacklinksTab({ canManage }: { canManage: boolean }) {
  const overview = useAdminQuery<BLOverview>('/api/admin/content/backlinks/overview')
  const [statusFilter, setStatusFilter] = useState('')
  const prospects = useAdminQuery<{ rows: Prospect[] }>('/api/admin/content/backlinks/prospects' + (statusFilter ? '?status=' + statusFilter : ''))
  const links = useAdminQuery<{ rows: Backlink[] }>('/api/admin/content/backlinks/links')
  const [p, setP] = useState<Record<string, string>>({})
  const [l, setL] = useState<Record<string, string>>({})
  const [csv, setCsv] = useState('')
  const [outreachFor, setOutreachFor] = useState<number | null>(null)
  const [o, setO] = useState<Record<string, string>>({})
  const [msg, setMsg] = useState('')

  async function call(fn: () => Promise<unknown>, then?: () => void) { setMsg(''); try { await fn(); then?.() } catch (e) { setMsg((e as Error).message) } }
  const addProspect = () => call(() => adminApi.post('/api/admin/content/backlinks/prospects', { name: p.name, domain: p.domain, url: p.url, category: p.category || 'publication', contact_email: p.contact_email, owner: p.owner, status: p.status || 'prospect' }), () => { setP({}); prospects.refetch(); overview.refetch() })
  const setProspectStatus = (id: number, status: string) => call(() => adminApi.patch(`/api/admin/content/backlinks/prospects/${id}`, { status }), () => { prospects.refetch(); overview.refetch() })
  const logOutreach = (id: number) => call(() => adminApi.post('/api/admin/content/backlinks/outreach', { prospect_id: id, channel: o.channel || 'email', subject: o.subject, outcome: o.outcome || 'sent', follow_up_at: o.follow_up_at, prospect_status: o.prospect_status }), () => { setOutreachFor(null); setO({}); prospects.refetch(); overview.refetch() })
  const addLink = () => call(() => adminApi.post('/api/admin/content/backlinks/links', { source_url: l.source_url, target_url: l.target_url, anchor_text: l.anchor_text, rel: l.rel || 'unknown' }), () => { setL({}); links.refetch(); overview.refetch() })
  const verifyLink = (id: number) => call(() => adminApi.post(`/api/admin/content/backlinks/links/${id}/verify`, {}), () => { links.refetch(); overview.refetch() })
  const importCsv = () => {
    const rows = csv.split('\n').map((ln) => ln.trim()).filter(Boolean).map((ln) => { const [source_url, target_url, anchor_text, rel] = ln.split(',').map((s) => s.trim()); return { source_url, target_url, anchor_text, rel } }).filter((r) => r.source_url)
    if (!rows.length) return
    call(() => adminApi.post('/api/admin/content/backlinks/links/import', { rows }), () => { setCsv(''); links.refetch(); overview.refetch() })
  }

  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <p className="muted" style={{ margin: 0 }}>Earn and monitor inbound links the honest way. PCI does not scrape the web, buy links, or run automated discovery — prospects, outreach and earned links are logged manually or by CSV. The one automated action is on-demand <strong>verification</strong>: fetching a single recorded source page (through the SSRF-guarded egress client) to confirm the link to PCI still exists. Bulk backlink discovery would require a licensed third-party backlink index and is not enabled.</p>
      {overview.data && (
        <div className="statgrid" style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(140px,1fr))', gap: '.8rem' }}>
          <Stat n={overview.data.live} k="Live links" />
          <Stat n={overview.data.lost} k="Lost / removed" />
          <Stat n={overview.data.prospects.reduce((s, r) => s + r.n, 0)} k="Prospects" />
          <Stat n={overview.data.follow_ups_due} k="Follow-ups due" />
        </div>
      )}
      {msg && <ErrorNote>{msg}</ErrorNote>}

      {canManage && (
        <Card title="Add a link prospect">
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'end' }}>
            <label>Name<input value={p.name || ''} onChange={(e) => setP({ ...p, name: e.target.value })} placeholder="PC Journal" /></label>
            <label>Domain<input value={p.domain || ''} onChange={(e) => setP({ ...p, domain: e.target.value })} placeholder="pcjournal.example" /></label>
            <label style={{ minWidth: 200 }}>URL<input value={p.url || ''} onChange={(e) => setP({ ...p, url: e.target.value })} placeholder="https://…" /></label>
            <label>Category<input value={p.category || ''} onChange={(e) => setP({ ...p, category: e.target.value })} placeholder="publication" /></label>
            <label>Contact email<input value={p.contact_email || ''} onChange={(e) => setP({ ...p, contact_email: e.target.value })} placeholder="editor@…" /></label>
            <button className="btn sm" onClick={addProspect} disabled={!p.name}>Add</button>
          </div>
        </Card>
      )}

      <Card title="Prospects" action={
        <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)}>
          <option value="">All statuses</option>
          {PROSPECT_STATUSES.map((s) => <option key={s} value={s}>{s.replace(/_/g, ' ')}</option>)}
        </select>
      }>
        {prospects.loading ? <Spinner /> : !prospects.data?.rows.length ? <Empty>No prospects</Empty> : (
          <table className="tbl"><thead><tr><th>Prospect</th><th>Status</th><th>Contact</th><th></th></tr></thead>
            <tbody>{prospects.data.rows.map((pr) => (
              <>
                <tr key={pr.id}>
                  <td>{pr.name}<div className="muted" style={{ fontSize: '.78rem' }}>{pr.domain}{pr.category ? ' · ' + pr.category : ''}</div></td>
                  <td>{canManage ? <select value={pr.status} onChange={(e) => setProspectStatus(pr.id, e.target.value)}>{PROSPECT_STATUSES.map((s) => <option key={s} value={s}>{s.replace(/_/g, ' ')}</option>)}</select> : <StatusPill status={pr.status} />}</td>
                  <td className="muted" style={{ fontSize: '.8rem' }}>{pr.contact_email || pr.contact_name || '—'}{pr.next_action_at ? <div>next: {pr.next_action_at}</div> : null}</td>
                  <td>{canManage ? <span className="row" style={{ gap: '.3rem' }}>
                    <button className="btn sm ghost" onClick={() => { setOutreachFor(outreachFor === pr.id ? null : pr.id); setO({}) }}>Outreach</button>
                    <button className="btn sm ghost" onClick={() => call(() => adminApi.post(`/api/admin/content/backlinks/prospects/${pr.id}/delete`, {}), () => { prospects.refetch(); overview.refetch() })}>Remove</button>
                  </span> : null}</td>
                </tr>
                {outreachFor === pr.id && canManage && (
                  <tr key={pr.id + '-o'}><td colSpan={4} style={{ background: 'var(--panel,#f6f6f8)' }}>
                    <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'end', padding: '.4rem 0' }}>
                      <label>Channel<select value={o.channel || 'email'} onChange={(e) => setO({ ...o, channel: e.target.value })}>{['email', 'call', 'social', 'event', 'form'].map((c) => <option key={c} value={c}>{c}</option>)}</select></label>
                      <label style={{ minWidth: 200 }}>Subject / note<input value={o.subject || ''} onChange={(e) => setO({ ...o, subject: e.target.value })} placeholder="Intro email sent" /></label>
                      <label>Outcome<select value={o.outcome || 'sent'} onChange={(e) => setO({ ...o, outcome: e.target.value })}>{['sent', 'replied', 'agreed', 'declined', 'no_response', 'bounced'].map((c) => <option key={c} value={c}>{c.replace(/_/g, ' ')}</option>)}</select></label>
                      <label>Advance status<select value={o.prospect_status || ''} onChange={(e) => setO({ ...o, prospect_status: e.target.value })}><option value="">(unchanged)</option>{PROSPECT_STATUSES.map((s) => <option key={s} value={s}>{s.replace(/_/g, ' ')}</option>)}</select></label>
                      <label>Follow-up<input type="date" value={o.follow_up_at || ''} onChange={(e) => setO({ ...o, follow_up_at: e.target.value })} /></label>
                      <button className="btn sm" onClick={() => logOutreach(pr.id)}>Log</button>
                    </div>
                  </td></tr>
                )}
              </>
            ))}</tbody>
          </table>
        )}
      </Card>

      {canManage && (
        <Card title="Record a backlink">
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'end' }}>
            <label style={{ minWidth: 240 }}>Source URL (the page linking to us)<input value={l.source_url || ''} onChange={(e) => setL({ ...l, source_url: e.target.value })} placeholder="https://site/article" /></label>
            <label style={{ minWidth: 200 }}>Target URL (our page)<input value={l.target_url || ''} onChange={(e) => setL({ ...l, target_url: e.target.value })} placeholder="https://projectcontrolsinstitute.org/…" /></label>
            <label>Anchor<input value={l.anchor_text || ''} onChange={(e) => setL({ ...l, anchor_text: e.target.value })} placeholder="Project Controls Institute" /></label>
            <label>Rel<select value={l.rel || 'unknown'} onChange={(e) => setL({ ...l, rel: e.target.value })}>{LINK_REL.map((r) => <option key={r} value={r}>{r}</option>)}</select></label>
            <button className="btn sm" onClick={addLink} disabled={!l.source_url}>Add</button>
          </div>
          <details style={{ marginTop: '.6rem' }}>
            <summary className="muted" style={{ cursor: 'pointer' }}>Bulk import (CSV: source_url,target_url,anchor,rel — one per line)</summary>
            <textarea value={csv} onChange={(e) => setCsv(e.target.value)} rows={4} style={{ width: '100%', marginTop: '.4rem' }} placeholder="https://site/a,https://projectcontrolsinstitute.org/x,PCI,dofollow" />
            <button className="btn sm" onClick={importCsv} disabled={!csv.trim()}>Import rows</button>
          </details>
        </Card>
      )}

      <Card title="Backlinks">
        {links.loading ? <Spinner /> : !links.data?.rows.length ? <Empty>No backlinks recorded</Empty> : (
          <table className="tbl"><thead><tr><th>Source → target</th><th>Anchor / rel</th><th>Status</th><th>Last checked</th><th></th></tr></thead>
            <tbody>{links.data.rows.map((bl) => (
              <tr key={bl.id}>
                <td style={{ maxWidth: 320 }}><a href={bl.source_url} target="_blank" rel="noreferrer">{bl.source_domain || bl.source_url}</a>{bl.target_url ? <div className="muted" style={{ fontSize: '.75rem' }}>→ {bl.target_url}</div> : null}</td>
                <td className="muted" style={{ fontSize: '.8rem' }}>{bl.anchor_text || '—'}<div><Badge tone="neutral">{bl.rel}</Badge></div></td>
                <td><StatusPill status={bl.status} /></td>
                <td className="muted" style={{ fontSize: '.78rem' }}>{bl.last_checked_at || '—'}{bl.last_status_code ? <div>HTTP {bl.last_status_code}</div> : null}</td>
                <td>{canManage ? <span className="row" style={{ gap: '.3rem' }}>
                  <button className="btn sm" onClick={() => verifyLink(bl.id)}>Verify</button>
                  <button className="btn sm ghost" onClick={() => call(() => adminApi.post(`/api/admin/content/backlinks/links/${bl.id}/delete`, {}), () => { links.refetch(); overview.refetch() })}>Remove</button>
                </span> : null}</td>
              </tr>))}</tbody>
          </table>
        )}
      </Card>
    </div>
  )
}

interface AnSource { id: number; provider: string; label?: string; property?: string; api_base?: string; auth_kind: string; range_days: number; status: string; last_synced_at?: string | null; last_error?: string | null; has_secret: boolean }
interface AnTotal { source_id: number; provider: string; label?: string; status: string; clicks?: number | null; impressions?: number | null; ctr?: number | null; position?: number | null; sessions?: number | null; users?: number | null; pageviews?: number | null; last_synced_at?: string | null }
interface AnMetric { id: number; source_id: number; dimension: string; dim_value?: string | null; metric_date?: string | null; clicks?: number | null; impressions?: number | null; ctr?: number | null; position?: number | null; sessions?: number | null; users?: number | null; pageviews?: number | null }

const AN_PROVIDERS = [{ v: 'gsc', l: 'Google Search Console' }, { v: 'bing', l: 'Bing Webmaster Tools' }, { v: 'ga4', l: 'Google Analytics 4' }]

interface ArticleStat { path: string; slug: string; title: string; section: string; status: string; views: number; link_clicks: number }
function PerArticlePanel() {
  const [days, setDays] = useState(28)
  const { data, loading } = useAdminQuery<{ days: number; articles: ArticleStat[] }>(`/api/admin/content/analytics/articles?days=${days}`)
  const arts = data?.articles || []
  return (
    <Card title="Per-article performance (first-party, cookieless)" action={
      <select value={days} onChange={(e) => setDays(Number(e.target.value))}>{[7, 28, 90, 365].map((d) => <option key={d} value={d}>Last {d} days</option>)}</select>
    }>
      {loading ? <Spinner /> : arts.length === 0 ? <Empty>No article views recorded yet in this window.</Empty> : (
        <table className="tbl"><thead><tr><th>Article</th><th>Section</th><th>Views</th><th>Link clicks</th></tr></thead>
          <tbody>{arts.map((a) => (
            <tr key={a.path}>
              <td><a href={a.path} target="_blank" rel="noreferrer">{a.title || a.slug}</a>{a.status !== 'published' ? <span className="muted" style={{ fontSize: '.75rem' }}> ({a.status})</span> : null}</td>
              <td><Badge tone={a.section === 'news' ? 'ok' : 'neutral' as never}>{a.section}</Badge></td>
              <td style={{ fontVariantNumeric: 'tabular-nums' }}>{a.views}</td>
              <td style={{ fontVariantNumeric: 'tabular-nums' }}>{a.link_clicks}</td>
            </tr>))}</tbody>
        </table>
      )}
    </Card>
  )
}

function AnalyticsTab({ canManage }: { canManage: boolean }) {
  const overview = useAdminQuery<{ sources: AnSource[]; totals: AnTotal[] }>('/api/admin/content/analytics/overview')
  const [f, setF] = useState<Record<string, string>>({ provider: 'gsc' })
  const [openSource, setOpenSource] = useState<number | null>(null)
  const metrics = useAdminQuery<{ rows: AnMetric[] }>(openSource ? `/api/admin/content/analytics/metrics?source_id=${openSource}&dimension=query` : null)
  const [msg, setMsg] = useState('')

  async function call(fn: () => Promise<unknown>, then?: () => void) { setMsg(''); try { await fn(); then?.() } catch (e) { setMsg((e as Error).message) } }
  const add = () => call(() => adminApi.post('/api/admin/content/analytics/sources', { provider: f.provider, label: f.label, property: f.property, api_base: f.api_base, secret: f.secret, range_days: f.range_days ? Number(f.range_days) : undefined }), () => { setF({ provider: 'gsc' }); overview.refetch() })
  const sync = (id: number) => call(() => adminApi.post(`/api/admin/content/analytics/sources/${id}/sync`, {}), () => { overview.refetch(); if (openSource === id) metrics.refetch() })

  const totalFor = (id: number) => overview.data?.totals.find((t) => t.source_id === id)
  const isSearch = (p: string) => p === 'gsc' || p === 'bing'

  return (
    <div style={{ display: 'grid', gap: '1rem' }}>
      <p className="muted" style={{ margin: 0 }}>Read-only search &amp; traffic analytics. PCI connects to providers whose official APIs are read-only — Google Search Console, Bing Webmaster Tools and Google Analytics 4 — and only ever <strong>reads</strong>: nothing is written back. Credentials are stored encrypted and never shown again. Google access tokens are short-lived; mint one via a service account or OAuth and re-paste it when a sync reports an auth error (automatic refresh is a planned follow-up).</p>
      {msg && <ErrorNote>{msg}</ErrorNote>}

      <PerArticlePanel />

      {canManage && (
        <Card title="Connect an analytics source">
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'end' }}>
            <label>Provider<select value={f.provider} onChange={(e) => setF({ ...f, provider: e.target.value })}>{AN_PROVIDERS.map((p) => <option key={p.v} value={p.v}>{p.l}</option>)}</select></label>
            <label>Label<input value={f.label || ''} onChange={(e) => setF({ ...f, label: e.target.value })} placeholder="PCI — production" /></label>
            <label style={{ minWidth: 220 }}>{f.provider === 'ga4' ? 'GA4 property id' : 'Site URL'}<input value={f.property || ''} onChange={(e) => setF({ ...f, property: e.target.value })} placeholder={f.provider === 'ga4' ? '123456789' : 'https://projectcontrolsinstitute.org/'} /></label>
            <label style={{ minWidth: 180 }}>{f.provider === 'bing' ? 'API key' : 'Access token'}<input type="password" value={f.secret || ''} onChange={(e) => setF({ ...f, secret: e.target.value })} placeholder="write-only" /></label>
            <label style={{ width: 90 }}>Days<input type="number" value={f.range_days || ''} onChange={(e) => setF({ ...f, range_days: e.target.value })} placeholder="28" /></label>
            <button className="btn sm" onClick={add} disabled={!f.provider || !f.property}>Connect</button>
          </div>
          <details style={{ marginTop: '.5rem' }}><summary className="muted" style={{ cursor: 'pointer' }}>Advanced</summary>
            <label style={{ display: 'block', marginTop: '.4rem' }}>API base override (optional)<input value={f.api_base || ''} onChange={(e) => setF({ ...f, api_base: e.target.value })} placeholder="leave blank for the provider default" style={{ width: '100%' }} /></label>
          </details>
        </Card>
      )}

      <Card title="Sources">
        {overview.loading ? <Spinner /> : !overview.data?.sources.length ? <Empty>No analytics sources connected</Empty> : (
          <table className="tbl"><thead><tr><th>Source</th><th>Status</th><th>Totals (window)</th><th>Last sync</th><th></th></tr></thead>
            <tbody>{overview.data.sources.map((s) => { const t = totalFor(s.id); return (
              <tr key={s.id}>
                <td>{s.label || s.provider.toUpperCase()}<div className="muted" style={{ fontSize: '.76rem' }}>{AN_PROVIDERS.find((p) => p.v === s.provider)?.l || s.provider} · {s.property}</div></td>
                <td><StatusPill status={s.status} />{!s.has_secret ? <div className="muted" style={{ fontSize: '.72rem' }}>no credential</div> : null}{s.last_error ? <div style={{ color: 'var(--err,#b00)', fontSize: '.72rem' }}>{s.last_error}</div> : null}</td>
                <td className="muted" style={{ fontSize: '.8rem' }}>{t && isSearch(s.provider) ? <span>{t.clicks ?? 0} clicks · {t.impressions ?? 0} impr{t.position != null ? ` · pos ${Number(t.position).toFixed(1)}` : ''}</span> : t && s.provider === 'ga4' ? <span>{t.sessions ?? 0} sessions · {t.users ?? 0} users · {t.pageviews ?? 0} views</span> : '—'}</td>
                <td className="muted" style={{ fontSize: '.78rem' }}>{s.last_synced_at || 'never'}</td>
                <td>{canManage ? <span className="row" style={{ gap: '.3rem' }}>
                  <button className="btn sm" onClick={() => sync(s.id)} disabled={!s.has_secret}>Sync</button>
                  {isSearch(s.provider) ? <button className="btn sm ghost" onClick={() => setOpenSource(openSource === s.id ? null : s.id)}>Queries</button> : null}
                  <button className="btn sm ghost" onClick={() => call(() => adminApi.post(`/api/admin/content/analytics/sources/${s.id}/delete`, {}), overview.refetch)}>Remove</button>
                </span> : null}</td>
              </tr>) })}</tbody>
          </table>
        )}
      </Card>

      {openSource && (
        <Card title="Top queries" action={<button className="btn sm ghost" onClick={() => setOpenSource(null)}>Close</button>}>
          {metrics.loading ? <Spinner /> : !metrics.data?.rows.length ? <Empty>No query data — run a sync first</Empty> : (
            <table className="tbl"><thead><tr><th>Query</th><th>Clicks</th><th>Impressions</th><th>CTR</th><th>Position</th></tr></thead>
              <tbody>{metrics.data.rows.map((m) => (
                <tr key={m.id}>
                  <td>{m.dim_value}</td>
                  <td>{m.clicks ?? 0}</td>
                  <td>{m.impressions ?? 0}</td>
                  <td>{m.ctr != null ? (Number(m.ctr) * 100).toFixed(1) + '%' : '—'}</td>
                  <td>{m.position != null ? Number(m.position).toFixed(1) : '—'}</td>
                </tr>))}</tbody>
            </table>
          )}
        </Card>
      )}
    </div>
  )
}
