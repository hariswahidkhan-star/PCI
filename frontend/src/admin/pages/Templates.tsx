import { useState, type FormEvent } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty, Stat } from '../../components/ui'

// Admin Console → Free Templates Library (§6A–6C). Manage the public library of free, downloadable
// project-controls templates (WBS, EVM tracker, risk register, cash-flow, RACI, …). Each template's body is
// plain CSV stored inline; only PUBLISHED templates appear on the public /free-templates.html page and are
// downloadable at /api/public/templates/{slug}/file. Content is synthetic — no student data, no answer keys.

interface Row {
  id: number
  slug: string
  title: string
  category: string
  certification_id: number | null
  summary?: string | null
  format: string
  body: string
  published: boolean
  sort_order: number
  download_count: number
  updated_at?: string | null
}
interface Resp { rows: Row[]; total: number; published: number }

interface DayPoint { day: string; count: number }
interface TopRow { slug: string; title: string; download_count: number; published: boolean }
interface Analytics {
  total_downloads: number; templates: number; published: number
  window_days: number; window_downloads: number; series: DayPoint[]; top: TopRow[]
}

const CATEGORIES = ['scope', 'schedule', 'cost', 'evm', 'risk', 'change', 'cashflow', 'finance', 'delivery', 'quality']
const CERTS: Record<string, string> = { '1': 'PCL-AI', '2': 'PFL-AI', '3': 'PML-AI' }
const titleCase = (s: string) => s.replace(/[_-]+/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())
const certLabel = (id: number | null) => (id && CERTS[String(id)]) || '—'

// Hand-authored SVG bar trend for the 30-day download series — no chart library. Scales to the window max,
// draws a faint baseline, and gives each day a native <title> tooltip. Renders responsively via viewBox.
function DownloadTrend({ series }: { series: DayPoint[] }) {
  const n = series.length || 1
  const max = Math.max(1, ...series.map((s) => s.count))
  const W = 320, H = 64, gap = 2, bw = Math.max(1, W / n - gap)
  return (
    <svg viewBox={`0 0 ${W} ${H}`} width="100%" height="72" role="img"
      aria-label={`Downloads over the last ${n} days`} style={{ display: 'block' }}>
      <line x1="0" y1={H - 1} x2={W} y2={H - 1} stroke="#e5e7eb" strokeWidth="1" />
      {series.map((s, i) => {
        const h = s.count === 0 ? 0 : Math.max(2, Math.round((s.count / max) * (H - 6)))
        const x = i * (W / n)
        return (
          <rect key={s.day} x={x} y={H - 1 - h} width={bw} height={h} rx="1" fill="#2563eb">
            <title>{`${s.day}: ${s.count} download${s.count === 1 ? '' : 's'}`}</title>
          </rect>
        )
      })}
    </svg>
  )
}

export default function Templates() {
  const { data, loading, error, refetch } = useAdminQuery<Resp>('/api/admin/templates')
  const { data: an } = useAdminQuery<Analytics>('/api/admin/templates/analytics')
  const [msg, setMsg] = useState('')
  const [err, setErr] = useState('')
  const [busy, setBusy] = useState(false)
  const [showForm, setShowForm] = useState(false)
  const [editId, setEditId] = useState<number | null>(null)
  // form fields
  const [slug, setSlug] = useState('')
  const [title, setTitle] = useState('')
  const [category, setCategory] = useState('scope')
  const [cert, setCert] = useState('1')
  const [summary, setSummary] = useState('')
  const [body, setBody] = useState('')
  const [published, setPublished] = useState(true)

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  const rows = data?.rows ?? []
  const drafts = rows.filter((r) => !r.published).length
  const totalDownloads = rows.reduce((n, r) => n + (r.download_count || 0), 0)

  const flash = (m: string) => { setMsg(m); setErr('') }
  const fail = (e: unknown) => { setErr(e instanceof Error ? e.message : 'Something went wrong.'); setMsg('') }

  const resetForm = () => {
    setEditId(null); setSlug(''); setTitle(''); setCategory('scope'); setCert('1'); setSummary(''); setBody(''); setPublished(true)
  }
  const openCreate = () => { resetForm(); setShowForm(true) }
  const openEdit = (r: Row) => {
    setEditId(r.id); setSlug(r.slug); setTitle(r.title); setCategory(r.category); setCert(r.certification_id ? String(r.certification_id) : '')
    setSummary(r.summary ?? ''); setBody(r.body); setPublished(r.published); setShowForm(true)
  }

  async function doSubmit(e: FormEvent) {
    e.preventDefault()
    setBusy(true); setErr(''); setMsg('')
    try {
      const payload = {
        title: title.trim(), category, certification_id: cert ? Number(cert) : undefined,
        summary: summary.trim() || undefined, body, published,
      }
      if (editId) {
        await adminApi.patch(`/api/admin/templates/${editId}`, payload)
        flash(`Template “${title.trim()}” updated.`)
      } else {
        await adminApi.post('/api/admin/templates', { ...payload, slug: slug.trim() })
        flash(`Template “${title.trim()}” created.`)
      }
      resetForm(); setShowForm(false); refetch()
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  async function togglePublish(r: Row) {
    setBusy(true); setErr(''); setMsg('')
    try {
      await adminApi.patch(`/api/admin/templates/${r.id}`, { published: !r.published })
      flash(`“${r.title}” ${r.published ? 'unpublished' : 'published'}.`)
      refetch()
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  async function doDelete(r: Row) {
    if (!window.confirm(`Delete the template “${r.title}”? This removes it from the public library.`)) return
    setBusy(true); setErr(''); setMsg('')
    try {
      await adminApi.del(`/api/admin/templates/${r.id}`)
      flash(`Deleted “${r.title}”.`)
      refetch()
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1 style={{ marginBottom: '.2rem' }}>Free Templates Library</h1>
        <p className="muted" style={{ marginTop: 0 }}>
          Free, downloadable project-controls templates for the public website. Published templates appear on{' '}
          <code>/free-templates.html</code> and download as CSV. Content is synthetic and freely reusable.
        </p>
      </div>

      {msg && <div className="notice ok" role="status">{msg}</div>}
      {err && <div className="notice err" role="alert">{err}</div>}

      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', alignItems: 'center' }}>
        <Stat n={data?.total ?? 0} k="Templates" />
        <Stat n={data?.published ?? 0} k="Published" />
        <Stat n={drafts} k="Drafts" />
        <Stat n={totalDownloads} k="Downloads" />
        <button className="btn" style={{ marginLeft: 'auto' }} onClick={() => (showForm ? (setShowForm(false), resetForm()) : openCreate())}>
          {showForm ? 'Cancel' : '+ New template'}
        </button>
      </div>

      {an && (
        <Card title="Downloads">
          <div style={{ display: 'grid', gap: '1rem', gridTemplateColumns: 'minmax(280px, 2fr) minmax(220px, 1fr)' }}>
            <div>
              <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', marginBottom: '.4rem' }}>
                <Stat n={an.total_downloads} k="Total downloads" />
                <Stat n={an.window_downloads} k={`Last ${an.window_days} days`} />
              </div>
              <DownloadTrend series={an.series} />
              <p className="muted" style={{ fontSize: 12, marginTop: '.3rem' }}>Downloads per day over the last {an.window_days} days (UTC).</p>
            </div>
            <div>
              <div className="muted" style={{ fontSize: 12, fontWeight: 700, textTransform: 'uppercase', letterSpacing: '.06em', marginBottom: '.3rem' }}>Most downloaded</div>
              {an.top.filter((t) => t.download_count > 0).length === 0 ? (
                <p className="muted" style={{ fontSize: 13 }}>No downloads recorded yet.</p>
              ) : (
                <ol style={{ margin: 0, paddingLeft: '1.1rem', display: 'grid', gap: '.25rem' }}>
                  {an.top.filter((t) => t.download_count > 0).map((t) => (
                    <li key={t.slug} style={{ fontSize: 13 }}>
                      <span>{t.title}</span>{' '}
                      <span className="muted" style={{ fontVariantNumeric: 'tabular-nums' }}>· {t.download_count}</span>
                      {!t.published && <Badge tone="neutral">Draft</Badge>}
                    </li>
                  ))}
                </ol>
              )}
            </div>
          </div>
        </Card>
      )}

      {showForm && (
        <Card title={editId ? 'Edit template' : 'New template'}>
          <form onSubmit={doSubmit} className="stack" style={{ display: 'grid', gap: '.6rem', maxWidth: 720 }}>
            <label>Slug (URL + filename)
              <input value={slug} onChange={(e) => setSlug(e.target.value)} required={!editId} disabled={!!editId}
                placeholder="wbs-template" />
            </label>
            <label>Title
              <input value={title} onChange={(e) => setTitle(e.target.value)} required placeholder="Work Breakdown Structure (WBS) template" />
            </label>
            <div className="row" style={{ gap: '.6rem', flexWrap: 'wrap' }}>
              <label>Category
                <select value={category} onChange={(e) => setCategory(e.target.value)}>
                  {CATEGORIES.map((c) => <option key={c} value={c}>{titleCase(c)}</option>)}
                </select>
              </label>
              <label>Certification
                <select value={cert} onChange={(e) => setCert(e.target.value)}>
                  <option value="1">PCL-AI</option>
                  <option value="2">PFL-AI</option>
                  <option value="3">PML-AI</option>
                  <option value="">Any</option>
                </select>
              </label>
            </div>
            <label>Summary (one line, shown in the catalogue)
              <input value={summary} onChange={(e) => setSummary(e.target.value)} placeholder="Roll scope down to work packages with leaf budgets." />
            </label>
            <label>Template body (CSV)
              <textarea value={body} onChange={(e) => setBody(e.target.value)} rows={10} required
                placeholder={'WBS ID,Parent ID,Element Name,Leaf Budget\n1,,Project,\n1.1,1,Design,40000'}
                style={{ fontFamily: 'monospace', width: '100%' }} />
            </label>
            <label className="row" style={{ gap: '.4rem', alignItems: 'center' }}>
              <input type="checkbox" checked={published} onChange={(e) => setPublished(e.target.checked)} />
              Published (live on the public library)
            </label>
            <div>
              <button className="btn primary" type="submit" disabled={busy}>{editId ? 'Save changes' : 'Create template'}</button>
            </div>
          </form>
        </Card>
      )}

      <Card title="Templates">
        {rows.length === 0 ? (
          <Empty>No templates yet — create the first one.</Empty>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead>
                <tr>
                  <th>Slug</th><th>Title</th><th>Category</th><th>Cert</th><th>Status</th>
                  <th style={{ textAlign: 'right' }}>Downloads</th><th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {rows.map((r) => (
                  <tr key={r.id}>
                    <td style={{ fontVariantNumeric: 'tabular-nums' }}><code>{r.slug}</code></td>
                    <td>{r.title}</td>
                    <td>{titleCase(r.category)}</td>
                    <td>{certLabel(r.certification_id)}</td>
                    <td><Badge tone={r.published ? 'ok' : 'neutral'}>{r.published ? 'Published' : 'Draft'}</Badge></td>
                    <td style={{ textAlign: 'right' }}>{r.download_count}</td>
                    <td>
                      <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                        <button className="btn sm" disabled={busy} onClick={() => openEdit(r)}>Edit</button>
                        <button className="btn sm secondary" disabled={busy} onClick={() => togglePublish(r)}>
                          {r.published ? 'Unpublish' : 'Publish'}
                        </button>
                        <a className="btn sm secondary" href={`/api/public/templates/${r.slug}/file`} target="_blank" rel="noreferrer">Download</a>
                        <button className="btn sm secondary" disabled={busy} onClick={() => doDelete(r)}>Delete</button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </Card>
    </div>
  )
}
