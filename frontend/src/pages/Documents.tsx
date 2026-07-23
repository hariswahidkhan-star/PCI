import { useState } from 'react'
import { useQuery } from '../api/hooks'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote, Empty, Badge } from '../components/ui'
import { fmtDate, titleCase } from '../format'

interface DocRow {
  id: number
  title: string
  description?: string | null
  category?: string | null
  doc_type?: string | null
  filename?: string | null
  mime?: string | null
  size_bytes?: number | null
  version?: number | null
  view_only: boolean
  ack_required: boolean
  acknowledged: boolean
  restricted_until?: string | null
  restricted: boolean
  expires_at?: string | null
  published_at?: string | null
  downloadable: boolean
  locked: boolean
  lock_reason?: string | null
}

/** Human file size (e.g. 812 KB, 3.4 MB) from a byte count, or null when unknown. */
function fmtSize(bytes?: number | null): string | null {
  if (bytes == null || bytes <= 0) return null
  if (bytes < 1024) return `${bytes} B`
  const kb = bytes / 1024
  if (kb < 1024) return `${kb < 10 ? kb.toFixed(1) : Math.round(kb)} KB`
  const mb = kb / 1024
  return `${mb < 10 ? mb.toFixed(1) : Math.round(mb)} MB`
}

/** Friendly label for why a document is locked (expired / suspended / archived / …). */
function lockLabel(reason?: string | null): string {
  return reason ? titleCase(reason) : 'Unavailable'
}

interface BookRow {
  id: number
  certification_id?: number | null
  kind?: string | null
  title: string
  description?: string | null
  url?: string | null
  watermark: number
  has_file: number
  filename?: string | null
  size_bytes?: number | null
  cert_acronym?: string | null
  cert_name?: string | null
}

/** Books & study materials for the student's certifications (GET /api/me/cert-documents),
 * grouped per certification; stored files download through the authenticated (and, when
 * flagged, personally watermarked) endpoint, link rows open their external URL. */
function BooksSection() {
  const { data, loading, error } = useQuery<{ rows: BookRow[] }>('/api/me/cert-documents')
  const [busy, setBusy] = useState<number | null>(null)

  const download = async (r: BookRow) => {
    setBusy(r.id)
    const tok = sessionStorage.getItem('pci.session.token')
    try {
      const res = await fetch(`/api/me/cert-documents/${r.id}/download`, {
        headers: tok ? { Authorization: 'Bearer ' + tok } : {},
      })
      if (!res.ok) { alert('Could not open this material right now. Please try again shortly or contact support.'); return }
      const blob = await res.blob()
      const href = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = href
      a.download = r.filename || `${r.title}.pdf`
      document.body.appendChild(a); a.click(); a.remove()
      setTimeout(() => URL.revokeObjectURL(href), 60_000)
    } catch { alert('Could not open this material right now. Please try again shortly or contact support.') }
    finally { setBusy(null) }
  }

  if (loading && !data) return null
  if (error) return null
  const rows = data?.rows ?? []
  if (rows.length === 0) return null

  const groups = new Map<string, BookRow[]>()
  for (const r of rows) {
    const g = r.cert_acronym || r.cert_name || 'General resources'
    if (!groups.has(g)) groups.set(g, [])
    groups.get(g)!.push(r)
  }

  return (
    <>
      <div>
        <h2 style={{ margin: 0 }}>Books &amp; study materials</h2>
        <p className="muted">Handbooks, Bodies of Knowledge and study guides for your certifications. Watermarked copies are personalised to you and not for redistribution.</p>
      </div>
      {[...groups.entries()].map(([group, items]) => (
        <Card title={group} key={'books-' + group}>
          <div className="res-grid">
            {items.map((r) => {
              const meta = [titleCase((r.kind || 'book').replace('_', ' ')), fmtSize(r.size_bytes), r.watermark ? 'Personalised copy' : null]
                .filter(Boolean).join(' · ')
              return (
                <div className="rep-item" key={r.id} style={{ alignItems: 'center' }}>
                  <div className="rep-item-main row" style={{ gap: '.8rem' }}>
                    <span className="res-ic">{(r.kind || 'BOOK').slice(0, 4).toUpperCase()}</span>
                    <div>
                      <strong>{r.title}</strong>
                      {meta && <div className="muted small">{meta}</div>}
                      {r.description && <div className="muted small">{r.description}</div>}
                    </div>
                  </div>
                  <div className="rep-item-actions" style={{ display: 'grid', gap: '.35rem', justifyItems: 'end' }}>
                    {r.has_file ? (
                      <button className="btn sm" disabled={busy === r.id} onClick={() => download(r)}>
                        {busy === r.id ? 'Preparing…' : 'Download'}
                      </button>
                    ) : r.url ? (
                      <a className="btn sm secondary" href={r.url} target="_blank" rel="noreferrer">Open</a>
                    ) : (
                      <span className="muted small">Coming soon</span>
                    )}
                  </div>
                </div>
              )
            })}
          </div>
        </Card>
      ))}
    </>
  )
}

/** Documents the signed-in student has been assigned (GET /api/me/documents), with
 * download / acknowledge / locked / restricted / view-only handling. */
export default function Documents() {
  const { data, loading, error, refetch } = useQuery<{ rows: DocRow[] }>('/api/me/documents')
  const [ackBusy, setAckBusy] = useState<number | null>(null)

  // Acknowledged, view-only, and download-required documents all fetch the file with the bearer
  // token (a plain <a href> would 401). view_only rows open in a new tab; the rest force a save.
  const openDoc = async (r: DocRow) => {
    const tok = sessionStorage.getItem('pci.session.token')
    try {
      const res = await fetch(`/api/me/documents/${r.id}/download`, {
        headers: tok ? { Authorization: 'Bearer ' + tok } : {},
      })
      if (!res.ok) { alert('Could not open this document right now. Please try again shortly or contact support.'); return }
      const blob = await res.blob()
      const href = URL.createObjectURL(blob)
      if (r.view_only) {
        // Keep the object URL alive for the new tab; the browser releases it when that tab closes.
        window.open(href)
      } else {
        const a = document.createElement('a')
        a.href = href
        a.download = r.filename || `document-${r.id}`
        document.body.appendChild(a); a.click(); a.remove()
        setTimeout(() => URL.revokeObjectURL(href), 60_000)
      }
    } catch { alert('Could not open this document right now. Please try again shortly or contact support.') }
  }

  // Record the student's acknowledgement, then refetch so the row flips to downloadable.
  const acknowledge = async (id: number) => {
    setAckBusy(id)
    try {
      await api.post(`/api/me/documents/${id}/acknowledge`, {})
      refetch()
    } catch (e) {
      alert(e instanceof Error ? e.message : 'Could not record your acknowledgement. Please try again.')
    } finally {
      setAckBusy(null)
    }
  }

  const renderActions = (r: DocRow) => {
    // Time-gated: released to the student on a future date.
    if (r.restricted) return <Badge tone="warn">Available {fmtDate(r.restricted_until)}</Badge>
    // Locked for another reason (expired / suspended / archived / …).
    if (r.locked) return <span className="muted small">{lockLabel(r.lock_reason)}</span>
    // Must be acknowledged before it becomes downloadable.
    if (r.ack_required && !r.acknowledged) {
      return (
        <>
          <button className="btn sm" disabled={ackBusy === r.id} onClick={() => acknowledge(r.id)}>
            {ackBusy === r.id ? 'Saving…' : 'Acknowledge'}
          </button>
          <span className="muted small">Acknowledgement required before download.</span>
        </>
      )
    }
    // Ready to view or download.
    if (r.downloadable) {
      return (
        <>
          <button className="btn sm" onClick={() => openDoc(r)}>{r.view_only ? 'View' : 'Download'}</button>
          {r.acknowledged && <span className="muted small">Acknowledged</span>}
        </>
      )
    }
    return <span className="muted small">Unavailable</span>
  }

  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []

  const groups = new Map<string, DocRow[]>()
  for (const r of rows) {
    const g = (r.category || 'General').trim() || 'General'
    if (!groups.has(g)) groups.set(g, [])
    groups.get(g)!.push(r)
  }

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>My Documents</h1>
        <p className="muted">Documents shared with you by the Institute. Download or review each one below.</p>
      </div>

      {rows.length === 0 ? (
        <Card><Empty>You don't have any documents yet.</Empty></Card>
      ) : (
        [...groups.entries()].map(([category, items]) => (
          <Card title={category} key={category}>
            <div className="res-grid">
              {items.map((r) => {
                const meta = [r.doc_type, fmtSize(r.size_bytes)].filter(Boolean).join(' · ')
                return (
                  <div className="rep-item" key={r.id} style={{ alignItems: 'center' }}>
                    <div className="rep-item-main row" style={{ gap: '.8rem' }}>
                      <span className="res-ic">{(r.doc_type || 'DOC').slice(0, 4).toUpperCase()}</span>
                      <div>
                        <strong>{r.title}</strong>
                        {r.version != null && r.version > 1 && <> <Badge>v{r.version}</Badge></>}
                        {meta && <div className="muted small">{meta}</div>}
                        {r.description && <div className="muted small">{r.description}</div>}
                      </div>
                    </div>
                    <div className="rep-item-actions" style={{ display: 'grid', gap: '.35rem', justifyItems: 'end' }}>
                      {renderActions(r)}
                    </div>
                  </div>
                )
              })}
            </div>
          </Card>
        ))
      )}

      <BooksSection />
    </div>
  )
}
