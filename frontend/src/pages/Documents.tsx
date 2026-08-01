import { useState } from 'react'
import { useQuery } from '../api/hooks'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote, Empty, Badge } from '../components/ui'
import { DocumentRow, ViewDownloadActions } from '../components/documents/DocumentActions'
import { studentToken } from '../files'
import { fmtDate, titleCase } from '../format'
import { useT } from '../i18n'
import { PageHeader } from '../components/premium'

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
 * grouped per certification. Stored files view/download through the authenticated (and, when
 * flagged, personally watermarked) endpoint; link rows open their external URL. */
function BooksSection() {
  const t = useT()
  const { data, loading, error } = useQuery<{ rows: BookRow[] }>('/api/me/cert-documents')

  if (loading && !data) return null
  if (error) return (
    <div>
      <h2 style={{ margin: 0 }}>{t('doc.booksTitle')}</h2>
      <ErrorNote>{error}</ErrorNote>
    </div>
  )
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
        <h2 style={{ margin: 0 }}>{t('doc.booksTitle')}</h2>
        <p className="muted">{t('doc.booksIntro')}</p>
      </div>
      {[...groups.entries()].map(([group, items]) => (
        <Card title={group} key={'books-' + group}>
          <div className="res-grid">
            {items.map((r) => (
              <DocumentRow
                key={r.id}
                info={{
                  title: r.title,
                  description: r.description,
                  filename: r.filename,
                  sizeBytes: r.size_bytes,
                  meta: [titleCase((r.kind || 'book').replace('_', ' ')), r.watermark ? t('doc.personalisedCopy') : null],
                }}
                actions={
                  r.has_file ? (
                    <ViewDownloadActions
                      info={{ title: r.title, filename: r.filename, sizeBytes: r.size_bytes }}
                      inlineUrl={`/api/me/cert-documents/${r.id}/download?inline=1`}
                      downloadUrl={`/api/me/cert-documents/${r.id}/download`}
                      token={studentToken()}
                    />
                  ) : r.url ? (
                    <a className="btn sm secondary" href={r.url} target="_blank" rel="noreferrer">{t('doc.open')}</a>
                  ) : (
                    <span className="muted small">{t('doc.comingSoon')}</span>
                  )
                }
              />
            ))}
          </div>
        </Card>
      ))}
    </>
  )
}

/** Documents the signed-in student has been assigned (GET /api/me/documents), with the universal
 * View + Download pair, acknowledge / locked / restricted / view-only handling. */
export default function Documents() {
  const t = useT()
  const { data, loading, error, refetch } = useQuery<{ rows: DocRow[] }>('/api/me/documents')
  const [ackBusy, setAckBusy] = useState<number | null>(null)

  // Record the student's acknowledgement, then refetch so the row flips to downloadable.
  const acknowledge = async (id: number) => {
    setAckBusy(id)
    try {
      await api.post(`/api/me/documents/${id}/acknowledge`, {})
      refetch()
    } catch (e) {
      alert(e instanceof Error ? e.message : t('doc.openError'))
    } finally {
      setAckBusy(null)
    }
  }

  const renderActions = (r: DocRow) => {
    // Time-gated: released to the student on a future date.
    if (r.restricted) return <Badge tone="warn">{t('doc.availableOn', { date: fmtDate(r.restricted_until) })}</Badge>
    // Locked for another reason (expired / suspended / archived / …).
    if (r.locked) return <span className="muted small">{r.lock_reason ? titleCase(r.lock_reason) : t('doc.unavailable')}</span>
    // Must be acknowledged before it becomes downloadable.
    if (r.ack_required && !r.acknowledged) {
      return (
        <>
          <button className="btn sm" disabled={ackBusy === r.id} onClick={() => acknowledge(r.id)}>
            {ackBusy === r.id ? t('doc.saving') : t('doc.acknowledge')}
          </button>
          <span className="muted small">{t('doc.ackRequired')}</span>
        </>
      )
    }
    // Ready: every downloadable document gets View AND (unless view-only) Download.
    if (r.downloadable) {
      return (
        <>
          <ViewDownloadActions
            info={{ title: r.title, filename: r.filename, mime: r.mime, sizeBytes: r.size_bytes, version: r.version }}
            inlineUrl={`/api/me/documents/${r.id}/download?inline=1`}
            downloadUrl={`/api/me/documents/${r.id}/download`}
            token={studentToken()}
            canDownload={!r.view_only}
            canPrint={!r.view_only}
          />
          {r.acknowledged && <span className="muted small">{t('doc.acknowledged')}</span>}
        </>
      )
    }
    return <span className="muted small">{t('doc.unavailable')}</span>
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
    <div className="page fade-stagger">
      <PageHeader eyebrow={t('nav.documents')} title={t('doc.myDocuments')} subtitle={t('doc.myDocumentsIntro')} />

      {rows.length === 0 ? (
        <Card><Empty>{t('doc.noDocuments')}</Empty></Card>
      ) : (
        [...groups.entries()].map(([category, items]) => (
          <Card title={category} key={category}>
            <div className="res-grid">
              {items.map((r) => (
                <DocumentRow
                  key={r.id}
                  info={{
                    title: r.title,
                    description: r.description,
                    filename: r.filename,
                    mime: r.mime,
                    sizeBytes: r.size_bytes,
                    version: r.version,
                    meta: [r.doc_type],
                  }}
                  actions={renderActions(r)}
                />
              ))}
            </div>
          </Card>
        ))
      )}

      <BooksSection />
    </div>
  )
}
