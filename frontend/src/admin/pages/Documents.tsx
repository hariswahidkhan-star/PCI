import { Fragment, useEffect, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { ViewDownloadActions } from '../../components/documents/DocumentActions'
import { fmtDateTime } from '../../format'

// Admin Console → Documents (gate 'documents'). Upload private, per-student documents into
// content-addressed storage, target them at a student or a resolved group, publish (which
// materialises the concrete grants + notifies), version them without ever overwriting history,
// and inspect recipients + the full download/acknowledgement audit. Distinct from the public,
// URL-based `resources` list — these are access-controlled FILES.

// ─────────────────────────── API shapes ───────────────────────────
interface Category {
  id: number
  name: string
  slug?: string | null
  description?: string | null
  active?: number | null
  sort_order?: number | null
  created_at?: string | null
}

// A row from GET /api/admin/documents — every `documents` column plus the three counts.
interface DocRow {
  id: number
  title: string
  description?: string | null
  category?: string | null
  doc_type?: string | null
  status: string
  filename?: string | null
  mime?: string | null
  size_bytes?: number | null
  version?: number | null
  view_only?: number | null
  ack_required?: number | null
  watermark?: number | null
  restricted_until?: string | null
  expires_at?: string | null
  publish_at?: string | null
  published_at?: string | null
  assignment_type?: string | null
  assignment_config?: string | null
  include_test?: number | null
  visible_to_cs?: number | null
  is_test?: number | null
  created_at?: string | null
  assigned_count?: number | null
  ack_count?: number | null
  download_count?: number | null
}

interface DocVersion {
  id: number
  version: number
  filename?: string | null
  mime?: string | null
  size_bytes?: number | null
  sha256?: string | null
  status: string
  replace_reason?: string | null
  restored_from_id?: number | null
  created_by?: number | null
  created_at?: string | null
}

interface Recipient {
  user_id: number
  assignment_type?: string | null
  source?: string | null
  status: string
  assigned_at?: string | null
  revoked_at?: string | null
  first_name?: string | null
  last_name?: string | null
  email?: string | null
  acknowledged_at?: string | null
  last_download?: string | null
}

interface DocDetail {
  document: DocRow
  versions: DocVersion[]
  recipients: Recipient[]
}

interface PreviewSample { id: number; name?: string | null; email?: string | null }
interface PreviewResp { count: number; sample: PreviewSample[] }

interface AuditResp {
  document: { id: number; title?: string | null; status?: string | null }
  summary: {
    active_grants: number
    revoked_grants: number
    acknowledgements: number
    distinct_downloaders: number
    total_downloads: number
    total_views: number
  }
  recent: { user_id?: number | null; actor?: string | null; role?: string | null; action?: string | null; result?: string | null; version?: number | null; created_at?: string | null }[]
}

// ─────────────────────────── constants ───────────────────────────
const DOC_STATUSES = ['draft', 'pending', 'published', 'active', 'scheduled', 'expired', 'archived', 'replaced', 'suspended', 'rejected', 'test'] as const

type Tone = 'ok' | 'warn' | 'err' | 'brand' | 'neutral'
const STATUS_TONE: Record<string, Tone> = {
  draft: 'neutral', pending: 'warn', published: 'ok', active: 'ok', scheduled: 'brand',
  expired: 'err', archived: 'neutral', replaced: 'neutral', suspended: 'err', rejected: 'err', test: 'warn',
}
function DocStatus({ status }: { status?: string | null }) {
  const s = (status ?? 'draft').toLowerCase()
  return <Badge tone={STATUS_TONE[s] ?? 'neutral'}>{s}</Badge>
}

// Assignment audiences (mirror DocAccess.Types) and the config field(s) each one needs.
type AssignFieldKey = 'user_id' | 'user_ids' | 'certification_id' | 'membership_type' | 'code' | 'country' | 'partner_id'
const ASSIGN_TYPES = ['all', 'student', 'students', 'custom_group', 'membership', 'certification', 'exam', 'course', 'passed', 'honorary', 'institution', 'discount_code', 'country', 'test_users'] as const
const ASSIGN_LABELS: Record<string, string> = {
  all: 'All active students', student: 'A single student', students: 'Several students (list)',
  custom_group: 'Custom group (list)', membership: 'By membership', certification: 'By certification',
  exam: 'By exam', course: 'By course', passed: 'Passed / credentialed', honorary: 'Honorary fellows',
  institution: 'By institution / partner', discount_code: 'By discount code', country: 'By country', test_users: 'Test accounts',
}
const ASSIGN_FIELDS: Record<string, AssignFieldKey[]> = {
  student: ['user_id'], students: ['user_ids'], custom_group: ['user_ids'],
  membership: ['membership_type'], certification: ['certification_id'], exam: ['certification_id'], course: ['certification_id'],
  institution: ['partner_id'], discount_code: ['code'], country: ['country'],
}

interface AssignForm {
  assignment_type: string
  user_id: string
  user_ids: string
  certification_id: string
  membership_type: string
  code: string
  country: string
  partner_id: string
  include_test: boolean
}
const emptyAssign = (): AssignForm => ({ assignment_type: 'all', user_id: '', user_ids: '', certification_id: '', membership_type: '', code: '', country: '', partner_id: '', include_test: false })

// Flatten the assignment form into the flat keys the API expects (numbers coerced, blanks dropped).
function assignPayload(a: AssignForm): Record<string, unknown> {
  const p: Record<string, unknown> = { assignment_type: a.assignment_type, include_test: a.include_test }
  for (const f of ASSIGN_FIELDS[a.assignment_type] ?? []) {
    const raw = a[f].trim()
    if (!raw) continue
    if (f === 'user_id' || f === 'certification_id' || f === 'partner_id') p[f] = Number(raw)
    else p[f] = raw
  }
  return p
}

// ─────────────────────────── file → data URI ───────────────────────────
const MAX_BYTES = 25 * 1024 * 1024
const ACCEPT = '.pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.csv,.txt,.png,.jpg,.jpeg,.zip,application/pdf,image/png,image/jpeg,application/zip'
function readFileAsDataUri(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const r = new FileReader()
    r.onload = () => resolve(String(r.result))
    r.onerror = () => reject(new Error('Could not read that file.'))
    r.readAsDataURL(file)
  })
}
function fmtBytes(n?: number | null): string {
  if (n == null) return '—'
  if (n < 1024) return n + ' B'
  if (n < 1024 * 1024) return (n / 1024).toFixed(0) + ' KB'
  return (n / (1024 * 1024)).toFixed(1) + ' MB'
}

// ─────────────────────────── page ───────────────────────────
export default function Documents() {
  const [status, setStatus] = useState('')
  const [category, setCategory] = useState('')
  const [q, setQ] = useState('')
  const [dq, setDq] = useState('')
  const [open, setOpen] = useState<number | null>(null)
  const [showNew, setShowNew] = useState(false)

  // Debounce the text search so the list doesn't refetch on every keystroke.
  useEffect(() => {
    const t = setTimeout(() => setDq(q.trim()), 300)
    return () => clearTimeout(t)
  }, [q])

  const cats = useAdminQuery<{ rows: Category[] }>('/api/admin/document-categories')
  const params = new URLSearchParams()
  if (status) params.set('status', status)
  if (category) params.set('category', category)
  if (dq) params.set('q', dq)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: DocRow[] }>(`/api/admin/documents${qs ? '?' + qs : ''}`)
  const rows = data?.rows ?? []
  const categories = cats.data?.rows ?? []

  return (
    <div className="page">
      <PageHeader
        title="Documents"
        subtitle="Private, per-student documents. Upload a file, target it at a student or a group, then publish to grant access and notify. Versions never overwrite history, and every view, download and acknowledgement is audited."
      />

      <Card>
        <div className="row" style={{ flexWrap: 'wrap', alignItems: 'flex-end', justifyContent: 'space-between' }}>
          <div className="row" style={{ flexWrap: 'wrap', gap: '.5rem' }}>
            <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }} aria-label="Status filter">
              <option value="">All statuses</option>
              {DOC_STATUSES.map((s) => <option key={s} value={s}>{s}</option>)}
            </select>
            <select value={category} onChange={(e) => setCategory(e.target.value)} style={{ maxWidth: 220 }} aria-label="Category filter">
              <option value="">All categories</option>
              {categories.map((c) => <option key={c.id} value={c.name}>{c.name}</option>)}
            </select>
            <input placeholder="Search title or description…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 260 }} aria-label="Search" />
          </div>
          <button className="btn sm" onClick={() => setShowNew(true)}>Upload document</button>
        </div>
      </Card>

      <Card>
        {loading && !data ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <Empty>No documents match. Upload one to get started.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Title</th><th>Category</th><th>Type</th><th>Status</th><th>Ver</th><th>Assigned</th><th>Acks</th><th>Downloads</th><th>Created</th></tr>
            </thead>
            <tbody>
              {rows.map((r) => (
                <Fragment key={r.id}>
                  <tr
                    role="button"
                    tabIndex={0}
                    style={{ cursor: 'pointer' }}
                    onClick={() => setOpen(open === r.id ? null : r.id)}
                    onKeyDown={(e) => { if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); setOpen(open === r.id ? null : r.id) } }}
                    aria-expanded={open === r.id}
                  >
                    <td><strong>{r.title}</strong>{r.filename && <div className="muted small">{r.filename}</div>}</td>
                    <td className="small">{r.category || '—'}</td>
                    <td className="small">{r.doc_type || '—'}</td>
                    <td><DocStatus status={r.status} /></td>
                    <td className="small">v{r.version ?? 1}</td>
                    <td className="small">{r.assigned_count ?? 0}</td>
                    <td className="small">{r.ack_count ?? 0}</td>
                    <td className="small">{r.download_count ?? 0}</td>
                    <td className="small muted">{fmtDateTime(r.created_at)}</td>
                  </tr>
                  {open === r.id && (
                    <tr>
                      <td colSpan={9} style={{ background: 'var(--canvas)' }}>
                        <DocDetailPanel row={r} onChanged={refetch} />
                      </td>
                    </tr>
                  )}
                </Fragment>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      <CategoriesCard />

      {showNew && (
        <NewDocumentDrawer
          categories={categories}
          onClose={() => setShowNew(false)}
          onSaved={() => { setShowNew(false); refetch() }}
        />
      )}
    </div>
  )
}

// ─────────────────────────── assignment picker + preview ───────────────────────────
function AssignmentPicker({ value, onChange }: { value: AssignForm; onChange: (a: AssignForm) => void }) {
  const fields = ASSIGN_FIELDS[value.assignment_type] ?? []
  const set = (patch: Partial<AssignForm>) => onChange({ ...value, ...patch })
  return (
    <div style={{ display: 'grid', gap: '.5rem' }}>
      <label>Assign to
        <select value={value.assignment_type} onChange={(e) => set({ assignment_type: e.target.value })}>
          {ASSIGN_TYPES.map((t) => <option key={t} value={t}>{ASSIGN_LABELS[t] ?? t}</option>)}
        </select>
      </label>
      {fields.includes('user_id') && <label>Student user ID<input value={value.user_id} onChange={(e) => set({ user_id: e.target.value })} placeholder="e.g. 1042" /></label>}
      {fields.includes('user_ids') && <label>Student user IDs <span className="muted small">(comma-separated)</span><input value={value.user_ids} onChange={(e) => set({ user_ids: e.target.value })} placeholder="1042, 1043, 1055" /></label>}
      {fields.includes('certification_id') && <label>Certification ID <span className="muted small">(optional — blank = any)</span><input value={value.certification_id} onChange={(e) => set({ certification_id: e.target.value })} /></label>}
      {fields.includes('membership_type') && <label>Membership type <span className="muted small">(optional — blank = any active member)</span><input value={value.membership_type} onChange={(e) => set({ membership_type: e.target.value })} /></label>}
      {fields.includes('partner_id') && <label>Institution / partner ID<input value={value.partner_id} onChange={(e) => set({ partner_id: e.target.value })} /></label>}
      {fields.includes('code') && <label>Discount code<input value={value.code} onChange={(e) => set({ code: e.target.value })} /></label>}
      {fields.includes('country') && <label>Country<input value={value.country} onChange={(e) => set({ country: e.target.value })} placeholder="e.g. GB" /></label>}
      <label className="row" style={{ gap: '.4rem', alignItems: 'center' }}>
        <input type="checkbox" checked={value.include_test} onChange={(e) => set({ include_test: e.target.checked })} style={{ width: 'auto' }} /> Include test accounts in group audiences
      </label>
    </div>
  )
}

function RecipientPreview({ build }: { build: () => Record<string, unknown> }) {
  const [res, setRes] = useState<PreviewResp | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  async function run() {
    setBusy(true); setErr(null); setRes(null)
    try { setRes(await adminApi.post<PreviewResp>('/api/admin/documents/preview-recipients', build())) }
    catch (e) { setErr(e instanceof Error ? e.message : 'Preview failed.') }
    finally { setBusy(false) }
  }
  return (
    <div style={{ display: 'grid', gap: '.35rem' }}>
      <div><button type="button" className="btn ghost sm" disabled={busy} onClick={run}>{busy ? 'Checking…' : 'Preview recipients'}</button></div>
      {err && <div className="notice err" role="alert">{err}</div>}
      {res && (
        <div className="small">
          <strong>{res.count}</strong> student{res.count === 1 ? '' : 's'} will receive this.
          {res.sample.length > 0 && (
            <div className="muted" style={{ marginTop: '.2rem' }}>
              {res.sample.map((s) => s.name || s.email || `#${s.id}`).join(', ')}{res.count > res.sample.length ? ', …' : ''}
            </div>
          )}
        </div>
      )}
    </div>
  )
}

// ─────────────────────────── new document (upload) drawer ───────────────────────────
function NewDocumentDrawer({ categories, onClose, onSaved }: { categories: Category[]; onClose: () => void; onSaved: () => void }) {
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [category, setCategory] = useState('')
  const [docType, setDocType] = useState('general')
  const [file, setFile] = useState<File | null>(null)
  const [fileKey, setFileKey] = useState(0)
  const [assign, setAssign] = useState<AssignForm>(emptyAssign())
  const [viewOnly, setViewOnly] = useState(false)
  const [ackRequired, setAckRequired] = useState(false)
  const [watermark, setWatermark] = useState(false)
  const [restrictedUntil, setRestrictedUntil] = useState('')
  const [publishAt, setPublishAt] = useState('')
  const [expiresAt, setExpiresAt] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  function pick(f: File | null) {
    setErr(null)
    if (f && f.size > MAX_BYTES) { setErr('That file is larger than 25 MB. Please choose a smaller file.'); setFile(null); setFileKey((k) => k + 1); return }
    setFile(f)
  }

  async function submit(publishNow: boolean) {
    if (!title.trim()) { setErr('A document title is required.'); return }
    if (!file) { setErr('Please choose a file to upload.'); return }
    setBusy(true); setErr(null)
    try {
      const dataUri = await readFileAsDataUri(file)
      const body: Record<string, unknown> = {
        title: title.trim(),
        file: dataUri,
        filename: file.name,
        doc_type: docType.trim() || 'general',
        ...assignPayload(assign),
        view_only: viewOnly,
        ack_required: ackRequired,
        watermark,
      }
      if (description.trim()) body.description = description.trim()
      if (category.trim()) body.category = category.trim()
      if (restrictedUntil) body.restricted_until = restrictedUntil
      if (publishAt) body.publish_at = publishAt
      if (expiresAt) body.expires_at = expiresAt
      if (publishNow) body.publish = true
      await adminApi.post('/api/admin/documents', body)
      onSaved()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not save the document.')
    } finally { setBusy(false) }
  }

  const buildPreview = () => ({ ...assignPayload(assign) })

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Upload a document</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}

        <div style={{ display: 'grid', gap: '.6rem' }}>
          <label>Title *<input value={title} onChange={(e) => setTitle(e.target.value)} /></label>
          <label>Description<textarea rows={2} value={description} onChange={(e) => setDescription(e.target.value)} /></label>
          <div style={{ display: 'grid', gap: '.6rem', gridTemplateColumns: 'repeat(auto-fit,minmax(160px,1fr))' }}>
            <label>Category <span className="muted small">(pick or type)</span>
              <input list="doc-cat-list" value={category} onChange={(e) => setCategory(e.target.value)} placeholder="General" />
              <datalist id="doc-cat-list">
                {categories.map((c) => <option key={c.id} value={c.name} />)}
              </datalist>
            </label>
            <label>Document type<input value={docType} onChange={(e) => setDocType(e.target.value)} placeholder="general" /></label>
          </div>

          <label>File * <span className="muted small">(PDF, Word, Excel, PowerPoint, CSV, text, PNG/JPG, ZIP — max 25 MB)</span>
            <input key={fileKey} type="file" accept={ACCEPT} onChange={(e) => pick(e.target.files?.[0] ?? null)} />
          </label>
          {file && <div className="small muted">{file.name} · {fmtBytes(file.size)}</div>}

          <fieldset style={{ border: '1px solid var(--line,#e2e8f0)', borderRadius: 10, padding: '.75rem', margin: 0 }}>
            <legend className="small" style={{ fontWeight: 700, padding: '0 .3rem' }}>Audience</legend>
            <AssignmentPicker value={assign} onChange={setAssign} />
            <div style={{ marginTop: '.5rem' }}><RecipientPreview build={buildPreview} /></div>
          </fieldset>

          <fieldset style={{ border: '1px solid var(--line,#e2e8f0)', borderRadius: 10, padding: '.75rem', margin: 0 }}>
            <legend className="small" style={{ fontWeight: 700, padding: '0 .3rem' }}>Options</legend>
            <div className="row" style={{ flexWrap: 'wrap', gap: '1rem' }}>
              <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={viewOnly} onChange={(e) => setViewOnly(e.target.checked)} style={{ width: 'auto' }} /> View-only</label>
              <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={ackRequired} onChange={(e) => setAckRequired(e.target.checked)} style={{ width: 'auto' }} /> Acknowledgement required</label>
              <label className="row" style={{ gap: '.4rem' }}><input type="checkbox" checked={watermark} onChange={(e) => setWatermark(e.target.checked)} style={{ width: 'auto' }} /> Watermark</label>
            </div>
            <div style={{ display: 'grid', gap: '.6rem', gridTemplateColumns: 'repeat(auto-fit,minmax(180px,1fr))', marginTop: '.6rem' }}>
              <label>Restricted until<input type="datetime-local" value={restrictedUntil} onChange={(e) => setRestrictedUntil(e.target.value)} /></label>
              <label>Publish at <span className="muted small">(future = scheduled)</span><input type="datetime-local" value={publishAt} onChange={(e) => setPublishAt(e.target.value)} /></label>
              <label>Expires at<input type="datetime-local" value={expiresAt} onChange={(e) => setExpiresAt(e.target.value)} /></label>
            </div>
          </fieldset>
        </div>

        <div className="row" style={{ gap: '.5rem', marginTop: '.9rem', flexWrap: 'wrap' }}>
          <button className="btn secondary" disabled={busy} onClick={() => submit(false)}>{busy ? 'Saving…' : 'Save as draft'}</button>
          <button className="btn" disabled={busy} onClick={() => submit(true)}>{busy ? 'Working…' : 'Publish now'}</button>
          <button className="btn ghost" onClick={onClose}>Cancel</button>
        </div>
      </div>
    </div>
  )
}

// ─────────────────────────── row detail ───────────────────────────
function DocDetailPanel({ row, onChanged }: { row: DocRow; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<DocDetail>(`/api/admin/documents/${row.id}`)
  const audit = useAdminQuery<AuditResp>(`/api/admin/documents/${row.id}/audit`)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [msg, setMsg] = useState<string | null>(null)
  const [newStatus, setNewStatus] = useState(row.status)
  const [verKey, setVerKey] = useState(0)
  const [assign, setAssign] = useState<AssignForm>(() => ({ ...emptyAssign(), assignment_type: row.assignment_type || 'all' }))

  function afterChange(text?: string) {
    setMsg(text ?? null)
    refetch(); audit.refetch(); onChanged()
  }
  async function run(fn: () => Promise<void>, okMsg?: string) {
    setBusy(true); setErr(null); setMsg(null)
    try { await fn(); afterChange(okMsg) }
    catch (e) { setErr(e instanceof Error ? e.message : 'The action could not be completed.') }
    finally { setBusy(false) }
  }

  const [verReason, setVerReason] = useState('')

  async function uploadVersion(file: File | null) {
    if (!file) return
    if (file.size > MAX_BYTES) { setErr('That file is larger than 25 MB.'); setVerKey((k) => k + 1); return }
    await run(async () => {
      const dataUri = await readFileAsDataUri(file)
      await adminApi.post(`/api/admin/documents/${row.id}/version`, { file: dataUri, filename: file.name, reason: verReason.trim() || undefined })
      setVerKey((k) => k + 1); setVerReason('')
    }, 'New version uploaded.')
  }

  // Restore = a NEW version whose file is the chosen older version's file. Nothing newer is erased.
  async function restore(v: DocVersion) {
    const reason = window.prompt(`Restore v${v.version} as the current version?\nOptional reason:`)
    if (reason === null) return
    await run(async () => {
      await adminApi.post(`/api/admin/documents/${row.id}/restore`, { version_id: v.id, reason: reason.trim() || undefined })
    }, `Restored v${v.version} as a new current version.`)
  }

  const recipients = data?.recipients ?? []
  const versions = data?.versions ?? []
  const newestVersionId = versions.length > 0 ? versions[versions.length - 1].id : null

  return (
    <div style={{ display: 'grid', gap: '.9rem', padding: '.5rem 0' }}>
      {err && <div className="notice err" role="alert">{err}</div>}
      {msg && <div className="notice" role="status">{msg}</div>}

      {/* Lifecycle actions */}
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap', alignItems: 'center' }}>
        <button className="btn sm" disabled={busy} onClick={() => run(() => adminApi.post(`/api/admin/documents/${row.id}/publish`).then(() => {}), 'Published.')}>Publish</button>
        <ViewDownloadActions
          info={{ title: row.title, filename: data?.document.filename ?? row.filename, mime: row.mime, sizeBytes: row.size_bytes, version: row.version }}
          inlineUrl={`/api/admin/documents/${row.id}/download?inline=1`}
          downloadUrl={`/api/admin/documents/${row.id}/download`}
          token={adminApi.getToken()}
        />
        <span className="muted small" style={{ marginLeft: '.4rem' }}>Set status</span>
        <select value={newStatus} onChange={(e) => setNewStatus(e.target.value)} disabled={busy} aria-label="New status">
          {DOC_STATUSES.map((s) => <option key={s} value={s}>{s}</option>)}
        </select>
        <button className="btn ghost sm" disabled={busy} onClick={() => run(() => adminApi.post(`/api/admin/documents/${row.id}/status`, { status: newStatus }).then(() => {}), `Status set to ${newStatus}.`)}>Apply</button>
        <button className="btn ghost sm danger" disabled={busy} onClick={() => run(() => adminApi.post(`/api/admin/documents/${row.id}/status`, { status: 'suspended' }).then(() => {}), 'Suspended.')}>Suspend</button>
        <button className="btn ghost sm" disabled={busy} onClick={() => run(() => adminApi.post(`/api/admin/documents/${row.id}/status`, { status: 'archived' }).then(() => {}), 'Archived.')}>Archive</button>
      </div>

      {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : (
        <>
          {/* Versions */}
          <Section title={`Version history (${versions.length})`}>
            {versions.length === 0 ? <Empty>No versions.</Empty> : (
              <table className="data">
                <thead><tr><th>Ver</th><th>File</th><th>Size</th><th>Checksum</th><th>Status</th><th>Reason</th><th>Created</th><th /></tr></thead>
                <tbody>
                  {versions.map((v) => (
                    <tr key={v.id}>
                      <td className="small">v{v.version}{v.restored_from_id != null && <div className="muted">restored</div>}</td>
                      <td className="small">{v.filename || '—'}<div className="muted small">{v.mime || ''}</div></td>
                      <td className="small">{fmtBytes(v.size_bytes)}</td>
                      <td className="small muted" title={v.sha256 ?? undefined}>{v.sha256 ? v.sha256.slice(0, 10) + '…' : '—'}</td>
                      <td><DocStatus status={v.status} /></td>
                      <td className="small muted">{v.replace_reason || '—'}</td>
                      <td className="small muted">{fmtDateTime(v.created_at)}</td>
                      <td>
                        <div className="row" style={{ gap: '.25rem', justifyContent: 'flex-end', flexWrap: 'wrap' }}>
                          <ViewDownloadActions
                            info={{ title: `${row.title} — v${v.version}`, filename: v.filename, mime: v.mime, sizeBytes: v.size_bytes, version: v.version }}
                            inlineUrl={`/api/admin/documents/${v.id}/download?inline=1`}
                            downloadUrl={`/api/admin/documents/${v.id}/download`}
                            token={adminApi.getToken()}
                          />
                          {v.id !== newestVersionId && (
                            <button className="btn ghost sm" disabled={busy} onClick={() => { void restore(v) }}>Restore</button>
                          )}
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
            <div style={{ display: 'grid', gap: '.35rem', marginTop: '.5rem' }}>
              <label className="small">Replacement reason <span className="muted">(optional — recorded in the version history)</span>
                <input value={verReason} onChange={(e) => setVerReason(e.target.value)} maxLength={300} placeholder="e.g. corrected 2026 fee table" />
              </label>
              <label className="small" style={{ display: 'block' }}>Upload a new version <span className="muted">(never overwrites — a live document keeps recipients)</span>
                <input key={verKey} type="file" accept={ACCEPT} disabled={busy} onChange={(e) => uploadVersion(e.target.files?.[0] ?? null)} />
              </label>
            </div>
          </Section>

          {/* Recipients */}
          <Section title={`Recipients (${recipients.length})`}>
            {recipients.length === 0 ? <Empty>No recipients yet. Publish the document to grant access.</Empty> : (
              <table className="data">
                <thead><tr><th>Student</th><th>Source</th><th>Status</th><th>Acknowledged</th><th>Last download</th><th /></tr></thead>
                <tbody>
                  {recipients.map((r) => (
                    <tr key={r.user_id}>
                      <td className="small"><strong>{[r.first_name, r.last_name].filter(Boolean).join(' ') || `#${r.user_id}`}</strong><div className="muted">{r.email || ''}</div></td>
                      <td className="small">{r.assignment_type || r.source || '—'}</td>
                      <td>{r.status === 'active' ? <Badge tone="ok">active</Badge> : <Badge tone="err">{r.status}</Badge>}</td>
                      <td className="small muted">{r.acknowledged_at ? fmtDateTime(r.acknowledged_at) : '—'}</td>
                      <td className="small muted">{r.last_download ? fmtDateTime(r.last_download) : '—'}</td>
                      <td>{r.status === 'active' && (
                        <button className="btn ghost sm danger" disabled={busy} onClick={() => run(() => adminApi.post(`/api/admin/documents/${row.id}/revoke`, { user_id: r.user_id }).then(() => {}), 'Access revoked.')}>Revoke</button>
                      )}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
            {recipients.some((r) => r.status === 'active') && (
              <div style={{ marginTop: '.5rem' }}>
                <button className="btn ghost sm danger" disabled={busy} onClick={() => { if (confirm('Revoke access for ALL recipients?')) run(() => adminApi.post(`/api/admin/documents/${row.id}/revoke`, {}).then(() => {}), 'All access revoked.') }}>Revoke all</button>
              </div>
            )}
          </Section>

          {/* Re-target audience */}
          <Section title="Audience">
            <p className="muted small" style={{ marginTop: 0 }}>Change who this document targets. Saving re-resolves the group and grants access to any newly-qualifying students (a live document only).</p>
            <AssignmentPicker value={assign} onChange={setAssign} />
            <div style={{ marginTop: '.5rem' }}><RecipientPreview build={() => ({ ...assignPayload(assign) })} /></div>
            <div style={{ marginTop: '.5rem' }}>
              <button className="btn sm" disabled={busy} onClick={() => run(() => adminApi.post(`/api/admin/documents/${row.id}/assign`, assignPayload(assign)).then(() => {}), 'Audience updated.')}>Save audience</button>
            </div>
          </Section>

          {/* Audit */}
          <Section title="Audit">
            {audit.loading && !audit.data ? <Spinner /> : audit.error ? <ErrorNote>{audit.error}</ErrorNote> : audit.data ? (
              <>
                <div className="row" style={{ flexWrap: 'wrap', gap: '1.2rem' }}>
                  <Metric k="Active grants" v={audit.data.summary.active_grants} />
                  <Metric k="Revoked" v={audit.data.summary.revoked_grants} />
                  <Metric k="Acknowledgements" v={audit.data.summary.acknowledgements} />
                  <Metric k="Distinct downloaders" v={audit.data.summary.distinct_downloaders} />
                  <Metric k="Total downloads" v={audit.data.summary.total_downloads} />
                  <Metric k="Total views" v={audit.data.summary.total_views} />
                </div>
                {audit.data.recent.length > 0 && (
                  <div style={{ marginTop: '.6rem' }}>
                    <div className="muted small" style={{ marginBottom: '.3rem' }}>Recent activity</div>
                    <table className="data">
                      <thead><tr><th>When</th><th>Actor</th><th>Role</th><th>Action</th><th>Result</th><th>Ver</th></tr></thead>
                      <tbody>
                        {audit.data.recent.slice(0, 20).map((e, i) => (
                          <tr key={i}>
                            <td className="small muted">{fmtDateTime(e.created_at)}</td>
                            <td className="small">{e.actor || (e.user_id != null ? `#${e.user_id}` : '—')}</td>
                            <td className="small">{e.role || '—'}</td>
                            <td className="small">{e.action || '—'}</td>
                            <td className="small">{e.result || '—'}</td>
                            <td className="small">{e.version != null ? `v${e.version}` : '—'}</td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                )}
              </>
            ) : null}
          </Section>
        </>
      )}
    </div>
  )
}

function Metric({ k, v }: { k: string; v: number }) {
  return <div><div style={{ fontSize: '1.3rem', fontWeight: 700 }}>{v}</div><div className="muted small">{k}</div></div>
}

function Section({ title, children }: { title: string; children: React.ReactNode }) {
  return <div><h4 style={{ margin: '0 0 .4rem' }}>{title}</h4><div style={{ display: 'grid', gap: '.3rem' }}>{children}</div></div>
}

// ─────────────────────────── categories management ───────────────────────────
function CategoriesCard() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Category[] }>('/api/admin/document-categories')
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const rows = data?.rows ?? []

  async function add() {
    if (!name.trim()) { setErr('A category name is required.'); return }
    setBusy(true); setErr(null)
    try {
      await adminApi.post('/api/admin/document-categories', { name: name.trim(), description: description.trim() || undefined })
      setName(''); setDescription(''); refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not add the category.') } finally { setBusy(false) }
  }
  async function toggle(c: Category) {
    setErr(null)
    try { await adminApi.patch(`/api/admin/document-categories/${c.id}`, { active: !c.active }); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Update failed.') }
  }
  async function del(c: Category) {
    if (!confirm(`Delete the “${c.name}” category?`)) return
    setErr(null)
    try { await adminApi.del(`/api/admin/document-categories/${c.id}`); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Delete failed.') }
  }

  return (
    <Card title="Document categories">
      {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
      {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
        <Empty>No categories yet — add one below.</Empty>
      ) : (
        <table className="data">
          <thead><tr><th>Name</th><th>Slug</th><th>Description</th><th>Active</th><th /></tr></thead>
          <tbody>
            {rows.map((c) => (
              <tr key={c.id}>
                <td><strong>{c.name}</strong></td>
                <td className="small muted">{c.slug || '—'}</td>
                <td className="small">{c.description || '—'}</td>
                <td>{c.active ? <Badge tone="ok">active</Badge> : <Badge tone="neutral">off</Badge>}</td>
                <td className="row" style={{ gap: '.3rem', justifyContent: 'flex-end' }}>
                  <button className="btn ghost sm" onClick={() => toggle(c)}>{c.active ? 'Deactivate' : 'Activate'}</button>
                  <button className="btn ghost sm danger" onClick={() => del(c)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      <div className="row" style={{ flexWrap: 'wrap', alignItems: 'flex-end', gap: '.5rem', marginTop: '.7rem' }}>
        <label>Name<input value={name} onChange={(e) => setName(e.target.value)} style={{ maxWidth: 200 }} /></label>
        <label>Description<input value={description} onChange={(e) => setDescription(e.target.value)} style={{ maxWidth: 260 }} /></label>
        <button className="btn sm" disabled={busy} onClick={add}>{busy ? 'Adding…' : 'Add category'}</button>
      </div>
    </Card>
  )
}
