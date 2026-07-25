import { useEffect, useMemo, useRef, useState } from 'react'
import { createPortal } from 'react-dom'
import { fetchBlob, fileKind, fileTypeLabel, fmtBytes, parseCsvPreview, saveBlob, type FileKind } from '../../files'
import { useTSafe } from '../../i18n'

// Secure in-platform document viewer, shared by the student and admin SPAs.
//
// The file is fetched over the AUTHENTICATED inline endpoint (bearer token, re-authorised
// server-side on every request) into a Blob; only a transient object URL ever reaches the DOM,
// so no permanent storage URL appears in markup. PDFs render in the browser's native viewer
// (page navigation, zoom, text search, print come with it); images, plain text and CSV render
// natively; anything else gets an honest fallback with a download action when permitted.

export interface ViewerDocument {
  title: string
  filename?: string | null
  mime?: string | null
  version?: number | null
  sizeBytes?: number | null
  /** Authenticated URL that serves the bytes inline (Content-Disposition: inline). */
  inlineUrl: string
  /** URL for the save-to-disk action; defaults to inlineUrl when omitted. */
  downloadUrl?: string | null
}

interface Props {
  doc: ViewerDocument
  token?: string | null
  canDownload?: boolean
  canPrint?: boolean
  onClose: () => void
}

const CSV_PREVIEW_ROWS = 200

export default function DocumentViewer({ doc, token, canDownload = true, canPrint = true, onClose }: Props) {
  const t = useTSafe()
  const [blob, setBlob] = useState<Blob | null>(null)
  const [objectUrl, setObjectUrl] = useState<string | null>(null)
  const [text, setText] = useState<string | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [busy, setBusy] = useState<'download' | null>(null)
  const [full, setFull] = useState(false)
  const closeRef = useRef<HTMLButtonElement>(null)
  const frameRef = useRef<HTMLIFrameElement>(null)
  const restoreFocus = useRef<Element | null>(null)

  const kind: FileKind = fileKind(doc.mime, doc.filename)

  // Fetch once per document; revoke the object URL on unmount/close.
  useEffect(() => {
    let cancelled = false
    let url: string | null = null
    setBlob(null); setObjectUrl(null); setText(null); setError(null)
    fetchBlob(doc.inlineUrl, token)
      .then(async (b) => {
        if (cancelled) return
        if (b.size === 0) { setError(t('doc.corrupt')); return }
        setBlob(b)
        if (kind === 'text' || kind === 'csv') {
          setText(await b.text())
        } else if (kind === 'pdf' || kind === 'image') {
          url = URL.createObjectURL(b)
          setObjectUrl(url)
        }
      })
      .catch(() => { if (!cancelled) setError(t('doc.openError')) })
    return () => {
      cancelled = true
      if (url) URL.revokeObjectURL(url)
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [doc.inlineUrl])

  // Keyboard: Esc closes. Focus moves into the dialog and back out on close.
  useEffect(() => {
    restoreFocus.current = document.activeElement
    closeRef.current?.focus()
    const onKey = (e: KeyboardEvent) => { if (e.key === 'Escape') onClose() }
    window.addEventListener('keydown', onKey)
    return () => {
      window.removeEventListener('keydown', onKey)
      const el = restoreFocus.current
      if (el instanceof HTMLElement) el.focus()
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  const download = async () => {
    setBusy('download')
    try {
      const b = blob ?? await fetchBlob(doc.downloadUrl || doc.inlineUrl, token)
      saveBlob(b, doc.filename || doc.title || 'document')
    } catch {
      setError(t('doc.downloadError'))
    } finally { setBusy(null) }
  }

  // Print via the PDF iframe's own viewer; other kinds do not offer print (download instead).
  const print = () => { try { frameRef.current?.contentWindow?.print() } catch { /* viewer blocked it */ } }

  const csvRows = useMemo(() => (kind === 'csv' && text != null ? parseCsvPreview(text, CSV_PREVIEW_ROWS) : null), [kind, text])
  const meta = [fileTypeLabel(doc.mime, doc.filename), fmtBytes(doc.sizeBytes)].filter(Boolean).join(' · ')

  const body = () => {
    if (error) return <div className="notice err" role="alert" style={{ margin: '1rem' }}>{error}</div>
    if (kind === 'pdf') {
      if (!objectUrl) return <ViewerLoading label={t('doc.loading')} />
      return <iframe ref={frameRef} src={objectUrl} title={doc.title} className="docviewer-frame" />
    }
    if (kind === 'image') {
      if (!objectUrl) return <ViewerLoading label={t('doc.loading')} />
      return <div className="docviewer-scroll docviewer-center"><img src={objectUrl} alt={doc.title} className="docviewer-img" /></div>
    }
    if (kind === 'csv') {
      if (csvRows == null) return <ViewerLoading label={t('doc.loading')} />
      return (
        <div className="docviewer-scroll" style={{ padding: '1rem' }}>
          {csvRows.length >= CSV_PREVIEW_ROWS && <p className="muted small">{t('doc.csvTruncated', { n: CSV_PREVIEW_ROWS })}</p>}
          <table className="data">
            <tbody>
              {csvRows.map((row, i) => (
                <tr key={i}>{row.map((cell, j) => i === 0 ? <th key={j}>{cell}</th> : <td key={j} className="small">{cell}</td>)}</tr>
              ))}
            </tbody>
          </table>
        </div>
      )
    }
    if (kind === 'text') {
      if (text == null) return <ViewerLoading label={t('doc.loading')} />
      return <div className="docviewer-scroll" style={{ padding: '1rem' }}><pre className="docviewer-pre">{text}</pre></div>
    }
    // Office / archive / unknown: honest fallback with the download escape hatch.
    return (
      <div className="docviewer-center" style={{ padding: '2rem', textAlign: 'center' }}>
        <div style={{ display: 'grid', gap: '.6rem', justifyItems: 'center' }}>
          <span className="res-ic" aria-hidden>{fileTypeLabel(doc.mime, doc.filename)}</span>
          <p className="muted" style={{ maxWidth: 420 }}>{t('doc.previewUnavailable')}</p>
          {canDownload && (
            <button className="btn" onClick={download} disabled={busy === 'download'}>
              {busy === 'download' ? t('doc.preparing') : t('doc.download')}
            </button>
          )}
        </div>
      </div>
    )
  }

  return createPortal(
    <div className="docviewer-backdrop" onClick={onClose}>
      <div
        className={'docviewer' + (full ? ' full' : '')}
        role="dialog"
        aria-modal="true"
        aria-label={doc.title}
        onClick={(e) => e.stopPropagation()}
      >
        <div className="docviewer-head">
          <div className="docviewer-title">
            <strong>{doc.title}</strong>
            <span className="muted small">
              {meta}
              {doc.version != null && <> · {t('doc.version', { n: doc.version })}</>}
            </span>
          </div>
          <div className="docviewer-actions">
            {canPrint && kind === 'pdf' && objectUrl && (
              <button className="btn ghost sm" onClick={print}>{t('doc.print')}</button>
            )}
            {canDownload && (
              <button className="btn ghost sm" onClick={download} disabled={busy === 'download'}>
                {busy === 'download' ? t('doc.preparing') : t('doc.download')}
              </button>
            )}
            <button className="btn ghost sm" onClick={() => setFull((f) => !f)} aria-pressed={full}>
              {full ? t('doc.exitFullscreen') : t('doc.fullscreen')}
            </button>
            <button ref={closeRef} className="btn secondary sm" onClick={onClose}>{t('doc.close')}</button>
          </div>
        </div>
        <div className="docviewer-body">{body()}</div>
      </div>
    </div>,
    document.body,
  )
}

function ViewerLoading({ label }: { label: string }) {
  return (
    <div className="docviewer-center" style={{ padding: '3rem' }}>
      <div style={{ display: 'grid', gap: '.6rem', justifyItems: 'center' }}>
        <div className="spinner" role="status" aria-label={label} />
        <span className="muted small">{label}</span>
      </div>
    </div>
  )
}
