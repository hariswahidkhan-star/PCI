import { useEffect, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { ViewDownloadActions } from '../../components/documents/DocumentActions'
import { fileToDataUri } from '../../files'
import { fmtDate } from '../../format'

// Mirrors the backend allow-lists in PublicDocuments.cs / PublicDocsSeed.cs.
const CATEGORIES = [
  ['certification-governance', 'Certification governance'], ['candidate-handbooks', 'Candidate handbooks'],
  ['application-routes', 'Application routes'], ['exams', 'Exams'], ['privacy-and-legal', 'Privacy & legal'],
  ['fees-and-refunds', 'Fees & refunds'], ['marketing-partners', 'Marketing partners'], ['institutions', 'Institutions'],
  ['accessibility', 'Accessibility'], ['policies', 'Policies'], ['general', 'General PCI'],
] as const
const STATUSES = ['draft', 'under_review', 'legal_review_required', 'approved', 'scheduled', 'published', 'superseded', 'archived', 'withdrawn']
const LEGAL = ['draft', 'internal_review_completed', 'external_legal_review_pending', 'external_legal_review_completed', 'approved_for_publication']
const ROUTES = ['', 'standard', 'founding', 'honorary', 'sponsored', 'complimentary']
const STATUS_TONE: Record<string, 'ok' | 'err' | 'brand' | 'warn'> = {
  published: 'ok', draft: 'brand', under_review: 'warn', legal_review_required: 'warn', approved: 'brand',
  scheduled: 'brand', superseded: 'warn', archived: 'err', withdrawn: 'err',
}
const pretty = (s?: string | null) => (s ?? '').replace(/_/g, ' ')

interface Doc {
  id: number; doc_group: string; title: string; description?: string | null; category: string
  certification_id?: number | null; certification_name?: string | null; route_key?: string | null
  language?: string | null; version?: string | null; status: string; legal_review_status?: string | null
  visibility?: string | null; owner?: string | null; approver?: string | null; effective_date?: string | null
  published_at?: string | null; next_review_date?: string | null; filename?: string | null
  size_bytes?: number | null; is_current?: boolean; download_count?: number | null; has_file?: boolean
  updated_at?: string | null
}
interface Cert { id: number; code: string; name: string }
interface Detail { document: Doc; versions: Array<{ id: number; version: string; status: string; is_current: number; published_at?: string | null; download_count?: number | null }>; recent_downloads: Array<{ ip?: string | null; ua?: string | null; created_at?: string | null }> }

/** Admin management of the Public Downloads Centre: create, upload, version, publish, replace, withdraw and
 *  archive documents; set legal-review status; and view version history and download analytics. All data is
 *  stored dynamically in MySQL via the .NET backend — nothing here is hardcoded. */
export default function PublicDownloads() {
  const [status, setStatus] = useState('')
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Doc[] }>(`/api/admin/public-documents${status ? `?status=${status}` : ''}`)
  const [certs, setCerts] = useState<Cert[]>([])
  const [open, setOpen] = useState<Detail | null>(null)
  const [creating, setCreating] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  useEffect(() => { adminApi.get<{ rows: Cert[] }>('/api/certifications').then((d) => setCerts(d.rows)).catch(() => {}) }, [])

  async function view(id: number) {
    setErr(null)
    try { setOpen(await adminApi.get<Detail>(`/api/admin/public-documents/${id}`)) }
    catch (e) { setErr(e instanceof Error ? e.message : 'Could not load the document.') }
  }
  async function refreshOpen() { if (open) await view(open.document.id) }

  async function save(patch: Record<string, unknown>) {
    if (!open) return
    setBusy(true); setErr(null)
    try { await adminApi.patch(`/api/admin/public-documents/${open.document.id}`, patch); await refreshOpen(); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Update failed.') } finally { setBusy(false) }
  }
  async function setDocStatus(s: string) {
    if (!open) return
    setBusy(true); setErr(null)
    try { await adminApi.post(`/api/admin/public-documents/${open.document.id}/status`, { status: s }); await refreshOpen(); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Status change failed.') } finally { setBusy(false) }
  }
  async function upload(file: File, asNewVersion: boolean) {
    if (!open) return
    setBusy(true); setErr(null)
    try {
      const data_uri = await fileToDataUri(file)
      const path = asNewVersion ? `/api/admin/public-documents/${open.document.id}/replace` : `/api/admin/public-documents/${open.document.id}/file`
      await adminApi.post(path, { data_uri, filename: file.name })
      await refreshOpen(); refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Upload failed.') } finally { setBusy(false) }
  }
  async function remove() {
    if (!open || !confirm('Delete this draft permanently?')) return
    setBusy(true); setErr(null)
    try { await adminApi.del(`/api/admin/public-documents/${open.document.id}`); setOpen(null); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Delete failed.') } finally { setBusy(false) }
  }

  const rows = data?.rows ?? []

  return (
    <div className="page">
      <PageHeader
        title="Downloads Centre"
        actions={<button className="btn sm" onClick={() => setCreating(true)}>New document</button>}
      />
      <p className="muted" style={{ margin: 0 }}>
        Public governance, policy and legal documents distributed at <strong>/downloads</strong> without login.
        Only <strong>published + public</strong> versions are ever served; drafts, internal, withdrawn, archived
        and superseded documents stay private. Manage versions, legal-review status and publication here.
      </p>

      <Card
        title="Documents"
        action={
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 220 }} aria-label="Status filter">
            <option value="">All statuses</option>
            {STATUSES.map((s) => <option key={s} value={s}>{pretty(s)}</option>)}
          </select>
        }
      >
        {err && !open && !creating && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
          <Empty>No documents with this status.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Title</th><th>Category</th><th>Cert / route</th><th>Version</th><th>Legal review</th><th>Downloads</th><th>Status</th><th /></tr></thead>
            <tbody>
              {rows.map((d) => (
                <tr key={d.id}>
                  <td><strong>{d.title}</strong><div className="muted small">{d.doc_group}{d.is_current ? ' · current' : ''}</div></td>
                  <td className="small">{CATEGORIES.find((c) => c[0] === d.category)?.[1] ?? d.category}</td>
                  <td className="small">{d.certification_name || 'All / General'}<div className="muted">{d.route_key || ''}</div></td>
                  <td className="small">v{d.version}</td>
                  <td className="small">{pretty(d.legal_review_status)}</td>
                  <td>{d.download_count ?? 0}</td>
                  <td><Badge tone={STATUS_TONE[d.status] ?? 'brand'}>{pretty(d.status)}</Badge></td>
                  <td><button className="btn ghost sm" onClick={() => view(d.id)}>Manage</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {creating && <CreateDrawer certs={certs} onClose={() => setCreating(false)} onDone={() => { setCreating(false); refetch() }} />}

      {open && (
        <div className="drawer-backdrop" onClick={() => setOpen(null)}>
          <div className="drawer" onClick={(e) => e.stopPropagation()}>
            <div className="spread" style={{ marginBottom: '1rem' }}>
              <h2 style={{ margin: 0 }}>{open.document.title}</h2>
              <button className="btn secondary sm" onClick={() => setOpen(null)}>Close</button>
            </div>
            {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
            <div className="spread small" style={{ marginBottom: '.6rem' }}>
              <Badge tone={STATUS_TONE[open.document.status] ?? 'brand'}>{pretty(open.document.status)}</Badge>
              <span className="muted">{open.document.doc_group} · v{open.document.version} · {open.document.download_count ?? 0} downloads</span>
            </div>

            <EditFields doc={open.document} certs={certs} busy={busy} onSave={save} />

            <div className="field" style={{ marginTop: '.5rem' }}>
              <label>File {open.document.has_file ? `— ${open.document.filename ?? 'attached'}` : '— none attached'}</label>
              <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'center' }}>
                {/* In-place file attach/replace is only possible pre-publication; after publication the
                    bytes are immutable history and only "Upload as new version" remains (the backend
                    enforces this with a 409 either way). */}
                {['draft', 'under_review', 'legal_review_required', 'approved'].includes(open.document.status) && (
                  <label className="btn sm secondary" style={{ cursor: 'pointer' }}>
                    {open.document.has_file ? 'Replace file' : 'Upload file'}
                    <input type="file" accept="application/pdf" hidden disabled={busy} onChange={(e) => { const f = e.target.files?.[0]; if (f) upload(f, false) }} />
                  </label>
                )}
                {open.document.has_file && (
                  <>
                    <ViewDownloadActions
                      info={{ title: open.document.title, filename: open.document.filename, mime: 'application/pdf', sizeBytes: open.document.size_bytes }}
                      inlineUrl={`/api/admin/public-documents/${open.document.id}/file`}
                      downloadUrl={`/api/admin/public-documents/${open.document.id}/file?dl=1`}
                      token={adminApi.getToken()}
                    />
                    <label className="btn sm secondary" style={{ cursor: 'pointer' }}>
                      Upload as new version
                      <input type="file" accept="application/pdf" hidden disabled={busy} onChange={(e) => { const f = e.target.files?.[0]; if (f) upload(f, true) }} />
                    </label>
                  </>
                )}
              </div>
            </div>

            <div className="field">
              <label>Lifecycle</label>
              <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap' }}>
                {open.document.status !== 'published' && <button className="btn sm" disabled={busy} onClick={() => setDocStatus('published')}>Publish</button>}
                {STATUSES.filter((s) => s !== 'published' && s !== open.document.status).map((s) => (
                  <button key={s} className="btn sm ghost" disabled={busy} onClick={() => setDocStatus(s)}>{pretty(s)}</button>
                ))}
                {open.document.status === 'draft' && <button className="btn sm danger" disabled={busy} onClick={remove}>Delete</button>}
              </div>
            </div>

            <div style={{ marginTop: '1rem' }}>
              <h4 style={{ margin: '0 0 .4rem' }}>Version history</h4>
              <table className="data"><thead><tr><th>Version</th><th>Status</th><th>Current</th><th>Published</th><th>Downloads</th></tr></thead>
                <tbody>{open.versions.map((v) => (
                  <tr key={v.id}><td>v{v.version}</td><td className="small">{pretty(v.status)}</td><td>{v.is_current ? '✓' : ''}</td><td className="small muted">{fmtDate(v.published_at)}</td><td>{v.download_count ?? 0}</td></tr>
                ))}</tbody>
              </table>
            </div>

            {open.recent_downloads.length > 0 && (
              <div style={{ marginTop: '1rem' }}>
                <h4 style={{ margin: '0 0 .4rem' }}>Recent downloads ({open.recent_downloads.length})</h4>
                <ul className="clean small" style={{ paddingLeft: 0, listStyle: 'none', display: 'grid', gap: '.2rem' }}>
                  {open.recent_downloads.slice(0, 10).map((r, i) => <li key={i} className="muted">{fmtDate(r.created_at)} · {r.ip || '—'}</li>)}
                </ul>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  )
}

function EditFields({ doc, certs, busy, onSave }: { doc: Doc; certs: Cert[]; busy: boolean; onSave: (p: Record<string, unknown>) => void }) {
  const [f, setF] = useState({
    title: doc.title, description: doc.description ?? '', category: doc.category,
    certification: doc.certification_id ? String(doc.certification_id) : '', route_key: doc.route_key ?? '',
    language: doc.language ?? 'en', version: doc.version ?? '1.0', legal_review_status: doc.legal_review_status ?? 'draft',
    visibility: doc.visibility ?? 'public', owner: doc.owner ?? '', approver: doc.approver ?? '',
    effective_date: doc.effective_date ?? '', next_review_date: doc.next_review_date ?? '',
  })
  useEffect(() => { setF({
    title: doc.title, description: doc.description ?? '', category: doc.category,
    certification: doc.certification_id ? String(doc.certification_id) : '', route_key: doc.route_key ?? '',
    language: doc.language ?? 'en', version: doc.version ?? '1.0', legal_review_status: doc.legal_review_status ?? 'draft',
    visibility: doc.visibility ?? 'public', owner: doc.owner ?? '', approver: doc.approver ?? '',
    effective_date: doc.effective_date ?? '', next_review_date: doc.next_review_date ?? '',
  }) }, [doc])
  const set = (k: keyof typeof f, v: string) => setF((p) => ({ ...p, [k]: v }))
  return (
    <div style={{ display: 'grid', gap: '.5rem' }}>
      <div className="field"><label>Title</label><input value={f.title} onChange={(e) => set('title', e.target.value)} /></div>
      <div className="field"><label>Description</label><textarea rows={2} value={f.description} onChange={(e) => set('description', e.target.value)} /></div>
      <div className="grid2" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '.5rem' }}>
        <div className="field"><label>Category</label><select value={f.category} onChange={(e) => set('category', e.target.value)}>{CATEGORIES.map((c) => <option key={c[0]} value={c[0]}>{c[1]}</option>)}</select></div>
        <div className="field"><label>Certification</label><select aria-label="Filter by / General" value={f.certification} onChange={(e) => set('certification', e.target.value)}><option value="">All / General</option>{certs.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}</select></div>
        <div className="field"><label>Route</label><select value={f.route_key} onChange={(e) => set('route_key', e.target.value)}>{ROUTES.map((r) => <option key={r} value={r}>{r || 'All routes'}</option>)}</select></div>
        <div className="field"><label>Language</label><input value={f.language} onChange={(e) => set('language', e.target.value)} /></div>
        <div className="field"><label>Version</label><input value={f.version} onChange={(e) => set('version', e.target.value)} /></div>
        <div className="field"><label>Visibility</label><select value={f.visibility} onChange={(e) => set('visibility', e.target.value)}><option value="public">Public</option><option value="internal">Internal</option></select></div>
        <div className="field"><label>Legal review status</label><select value={f.legal_review_status} onChange={(e) => set('legal_review_status', e.target.value)}>{LEGAL.map((s) => <option key={s} value={s}>{pretty(s)}</option>)}</select></div>
        <div className="field"><label>Owner</label><input value={f.owner} onChange={(e) => set('owner', e.target.value)} /></div>
        <div className="field"><label>Approver</label><input value={f.approver} onChange={(e) => set('approver', e.target.value)} /></div>
        <div className="field"><label>Effective date</label><input value={f.effective_date} onChange={(e) => set('effective_date', e.target.value)} placeholder="e.g. 19 July 2026" /></div>
        <div className="field"><label>Next review date</label><input value={f.next_review_date} onChange={(e) => set('next_review_date', e.target.value)} /></div>
      </div>
      <div><button className="btn sm" disabled={busy} onClick={() => onSave(f)}>Save changes</button></div>
    </div>
  )
}

function CreateDrawer({ certs, onClose, onDone }: { certs: Cert[]; onClose: () => void; onDone: () => void }) {
  const [f, setF] = useState({ title: '', description: '', category: 'certification-governance', certification: '', route_key: '', language: 'en', version: '1.0', legal_review_status: 'external_legal_review_pending', visibility: 'public' })
  const [file, setFile] = useState<File | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const set = (k: keyof typeof f, v: string) => setF((p) => ({ ...p, [k]: v }))
  async function create() {
    if (!f.title.trim()) { setErr('A title is required.'); return }
    setBusy(true); setErr(null)
    try {
      const body: Record<string, unknown> = { ...f }
      if (file) { body.data_uri = await fileToDataUri(file); body.filename = file.name }
      await adminApi.post('/api/admin/public-documents', body)
      onDone()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Create failed.') } finally { setBusy(false) }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}><h2 style={{ margin: 0 }}>New document</h2><button className="btn secondary sm" onClick={onClose}>Close</button></div>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        <div style={{ display: 'grid', gap: '.5rem' }}>
          <div className="field"><label>Title</label><input value={f.title} onChange={(e) => set('title', e.target.value)} /></div>
          <div className="field"><label>Description</label><textarea rows={2} value={f.description} onChange={(e) => set('description', e.target.value)} /></div>
          <div className="grid2" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '.5rem' }}>
            <div className="field"><label>Category</label><select value={f.category} onChange={(e) => set('category', e.target.value)}>{CATEGORIES.map((c) => <option key={c[0]} value={c[0]}>{c[1]}</option>)}</select></div>
            <div className="field"><label>Certification</label><select aria-label="Filter by / General" value={f.certification} onChange={(e) => set('certification', e.target.value)}><option value="">All / General</option>{certs.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}</select></div>
            <div className="field"><label>Route</label><select value={f.route_key} onChange={(e) => set('route_key', e.target.value)}>{ROUTES.map((r) => <option key={r} value={r}>{r || 'All routes'}</option>)}</select></div>
            <div className="field"><label>Version</label><input value={f.version} onChange={(e) => set('version', e.target.value)} /></div>
            <div className="field"><label>Legal review status</label><select value={f.legal_review_status} onChange={(e) => set('legal_review_status', e.target.value)}>{LEGAL.map((s) => <option key={s} value={s}>{pretty(s)}</option>)}</select></div>
            <div className="field"><label>Visibility</label><select value={f.visibility} onChange={(e) => set('visibility', e.target.value)}><option value="public">Public</option><option value="internal">Internal</option></select></div>
          </div>
          <div className="field"><label>PDF file (optional now, required to publish)</label><input type="file" accept="application/pdf" onChange={(e) => setFile(e.target.files?.[0] ?? null)} /></div>
          <div><button className="btn" disabled={busy} onClick={create}>{busy ? 'Creating…' : 'Create document'}</button></div>
        </div>
      </div>
    </div>
  )
}
