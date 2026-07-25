import { useMemo, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Spinner, ErrorNote, Empty, Badge } from '../../components/ui'
import { ViewDownloadActions } from '../../components/documents/DocumentActions'

interface BookRow {
  id: number
  certification_id?: number | null
  kind?: string | null
  title: string
  description?: string | null
  url?: string | null
  route_key?: string | null
  watermark: number
  published: number
  sort_order?: number | null
  filename?: string | null
  mime?: string | null
  size_bytes?: number | null
  storage_ref?: string | null
}
interface CertRow { id: number; code: string; name: string; acronym?: string | null }

const KINDS = ['handbook', 'bok', 'study_guide', 'book', 'form', 'policy', 'general']

function readAsDataUrl(file: File): Promise<string> {
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

/** Books & study materials per certification: list, upload (stored privately, personalised
 * watermark on download when flagged), publish/unpublish and delete. */
export default function Books() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: BookRow[] }>('/api/admin/cert_documents')
  const { data: certs } = useAdminQuery<{ rows: CertRow[] }>('/api/certifications')
  const [msg, setMsg] = useState('')
  const [err, setErr] = useState('')
  const [busy, setBusy] = useState(false)

  const [form, setForm] = useState({ certification_id: '', kind: 'book', title: '', description: '', watermark: true })
  const [file, setFile] = useState<File | null>(null)

  const certName = useMemo(() => {
    const m = new Map<number, string>()
    for (const c of certs?.rows ?? []) m.set(c.id, c.acronym || c.code)
    return m
  }, [certs])

  const upload = async () => {
    if (!file) { setErr('Choose a PDF file to upload.'); return }
    if (!form.title.trim()) { setErr('Give the book a title.'); return }
    setBusy(true); setErr(''); setMsg('')
    try {
      const dataUri = await readAsDataUrl(file)
      await adminApi.post('/api/admin/cert-documents/upload', {
        certification_id: form.certification_id || undefined,
        kind: form.kind, title: form.title.trim(), description: form.description.trim() || undefined,
        watermark: form.watermark, file: dataUri, filename: file.name,
      })
      setMsg('Uploaded.'); setForm({ certification_id: '', kind: 'book', title: '', description: '', watermark: true }); setFile(null)
      refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Upload failed.') }
    finally { setBusy(false) }
  }

  const replaceFile = async (row: BookRow, f: File) => {
    // Optional provenance: recorded on the superseded file's history row.
    const reason = window.prompt('Reason for replacing this file (optional):', '')
    if (reason === null) return
    setBusy(true); setErr(''); setMsg('')
    try {
      const dataUri = await readAsDataUrl(f)
      await adminApi.post('/api/admin/cert-documents/upload', { id: row.id, file: dataUri, filename: f.name, reason: reason.trim() || undefined })
      setMsg('File replaced — the previous file is kept in the version history.'); refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Upload failed.') }
    finally { setBusy(false) }
  }

  const patch = async (row: BookRow, body: Record<string, unknown>, done: string) => {
    setBusy(true); setErr(''); setMsg('')
    try { await adminApi.patch(`/api/admin/cert_documents/${row.id}`, body); setMsg(done); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Update failed.') }
    finally { setBusy(false) }
  }

  const remove = async (row: BookRow) => {
    if (!window.confirm(`Delete "${row.title}"? Students lose access immediately.`)) return
    setBusy(true); setErr(''); setMsg('')
    try { await adminApi.del(`/api/admin/cert_documents/${row.id}`); setMsg('Deleted.'); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Delete failed.') }
    finally { setBusy(false) }
  }

  const [history, setHistory] = useState<BookRow | null>(null)

  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Books &amp; materials</h1>
        <p className="muted">Candidate handbooks, Bodies of Knowledge and study guides, assignable per certification. Uploaded files are stored privately and served through the authenticated student download — watermarked copies are personalised per student.</p>
      </div>
      {msg && <div className="note ok">{msg}</div>}
      {err && <ErrorNote>{err}</ErrorNote>}

      <Card title="Upload a book">
        <div className="row" style={{ gap: '.6rem', flexWrap: 'wrap', alignItems: 'end' }}>
          <div className="field"><label>Certification</label>
            <select value={form.certification_id} onChange={(e) => setForm({ ...form, certification_id: e.target.value })}>
              <option value="">All certifications</option>
              {(certs?.rows ?? []).map((c) => <option key={c.id} value={c.id}>{c.acronym || c.code}</option>)}
            </select>
          </div>
          <div className="field"><label>Kind</label>
            <select value={form.kind} onChange={(e) => setForm({ ...form, kind: e.target.value })}>
              {KINDS.map((k) => <option key={k} value={k}>{k.replace('_', ' ')}</option>)}
            </select>
          </div>
          <div className="field" style={{ minWidth: 220 }}><label>Title</label>
            <input value={form.title} onChange={(e) => setForm({ ...form, title: e.target.value })} placeholder="PCI PFL-AI Body of Knowledge" />
          </div>
          <div className="field" style={{ minWidth: 220 }}><label>Description</label>
            <input value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
          </div>
          <div className="field"><label>File (PDF)</label>
            <input type="file" accept="application/pdf" onChange={(e) => setFile(e.target.files?.[0] ?? null)} />
          </div>
          <label className="row" style={{ gap: '.35rem', alignItems: 'center' }}>
            <input type="checkbox" checked={form.watermark} onChange={(e) => setForm({ ...form, watermark: e.target.checked })} />
            Personalised watermark
          </label>
          <button className="btn" disabled={busy} onClick={upload}>{busy ? 'Working…' : 'Upload'}</button>
        </div>
      </Card>

      <Card title={`Library (${rows.length})`}>
        {rows.length === 0 ? <Empty>No books yet — upload the first one above.</Empty> : (
          <table className="table">
            <thead><tr><th>Certification</th><th>Kind</th><th>Title</th><th>File</th><th>Watermark</th><th>Status</th><th></th></tr></thead>
            <tbody>
              {rows.map((r) => (
                <tr key={r.id}>
                  <td>{r.certification_id ? (certName.get(r.certification_id) ?? r.certification_id) : 'All'}</td>
                  <td>{(r.kind || 'general').replace('_', ' ')}</td>
                  <td><strong>{r.title}</strong>{r.description && <div className="muted small">{r.description}</div>}</td>
                  <td>{r.storage_ref ? <>{r.filename || 'file'} <span className="muted small">({fmtBytes(r.size_bytes)})</span></> : r.url ? <span className="muted small">external link</span> : <span className="muted small">no file</span>}</td>
                  <td>{r.watermark ? <Badge tone="ok">Personalised</Badge> : <span className="muted small">—</span>}</td>
                  <td>{r.published ? <Badge tone="ok">Published</Badge> : <Badge tone="warn">Hidden</Badge>}</td>
                  <td style={{ whiteSpace: 'nowrap' }}>
                    {r.storage_ref && (
                      <ViewDownloadActions
                        info={{ title: r.title, filename: r.filename, mime: r.mime, sizeBytes: r.size_bytes }}
                        inlineUrl={`/api/admin/cert-documents/${r.id}/file?inline=1`}
                        downloadUrl={`/api/admin/cert-documents/${r.id}/file`}
                        token={adminApi.getToken()}
                      />
                    )}
                    <label className="btn ghost sm" style={{ cursor: 'pointer' }}>
                      Replace file
                      <input type="file" accept="application/pdf" style={{ display: 'none' }}
                        onChange={(e) => { const f = e.target.files?.[0]; if (f) replaceFile(r, f); e.target.value = '' }} />
                    </label>{' '}
                    <button className="btn ghost sm" disabled={busy} onClick={() => setHistory(r)}>History</button>{' '}
                    <button className="btn ghost sm" disabled={busy} onClick={() => patch(r, { published: r.published ? 0 : 1 }, r.published ? 'Hidden.' : 'Published.')}>
                      {r.published ? 'Hide' : 'Publish'}
                    </button>{' '}
                    <button className="btn ghost sm" disabled={busy} onClick={() => remove(r)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {history && <BookHistoryDrawer book={history} onClose={() => setHistory(null)} onChanged={() => { refetch(); setHistory(null) }} />}
    </div>
  )
}

interface BookVersion {
  id: number
  version: number
  filename?: string | null
  mime?: string | null
  size_bytes?: number | null
  sha256?: string | null
  replaced_by?: number | null
  replace_reason?: string | null
  restored_from_id?: number | null
  created_at?: string | null
}

/** Superseded-file history for one book: every replaced file with who/why, per-version
 *  View/Download, and Restore (which snapshots the current file first — nothing is lost). */
function BookHistoryDrawer({ book, onClose, onChanged }: { book: BookRow; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: BookVersion[] }>(`/api/admin/cert-documents/${book.id}/versions`)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const rows = data?.rows ?? []

  async function restore(v: BookVersion) {
    const reason = window.prompt(`Restore v${v.version} (${v.filename || 'file'}) as the current file?\nOptional reason:`)
    if (reason === null) return
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/cert-documents/${book.id}/restore`, { version_id: v.id, reason: reason.trim() || undefined })
      refetch(); onChanged()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Restore failed.') }
    finally { setBusy(false) }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Version history — {book.title}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {err && <ErrorNote>{err}</ErrorNote>}
        <p className="muted small" style={{ marginTop: 0 }}>
          Current file: <strong>{book.filename || '—'}</strong> ({fmtBytes(book.size_bytes)}).
          Every replaced file is kept below and can be inspected or restored; restoring snapshots the
          current file too, so no version is ever lost.
        </p>
        {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
          <Empty>No superseded versions — this file has never been replaced.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Ver</th><th>File</th><th>Checksum</th><th>Reason</th><th>Replaced</th><th /></tr></thead>
            <tbody>
              {rows.map((v) => (
                <tr key={v.id}>
                  <td className="small">v{v.version}{v.restored_from_id != null && <div className="muted">restored</div>}</td>
                  <td className="small">{v.filename || '—'}<div className="muted">{fmtBytes(v.size_bytes)}</div></td>
                  <td className="small muted" title={v.sha256 ?? undefined}>{v.sha256 ? v.sha256.slice(0, 10) + '…' : '—'}</td>
                  <td className="small muted">{v.replace_reason || '—'}</td>
                  <td className="small muted">{v.created_at || '—'}</td>
                  <td>
                    <div className="row" style={{ gap: '.25rem', justifyContent: 'flex-end', flexWrap: 'wrap' }}>
                      <ViewDownloadActions
                        info={{ title: `${book.title} — v${v.version}`, filename: v.filename, mime: v.mime, sizeBytes: v.size_bytes, version: v.version }}
                        inlineUrl={`/api/admin/cert-documents/${book.id}/versions/${v.id}/file?inline=1`}
                        downloadUrl={`/api/admin/cert-documents/${book.id}/versions/${v.id}/file`}
                        token={adminApi.getToken()}
                      />
                      <button className="btn ghost sm" disabled={busy} onClick={() => { void restore(v) }}>Restore</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}
