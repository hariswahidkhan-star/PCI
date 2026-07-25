import { useState, type ReactNode } from 'react'
import DocumentViewer, { type ViewerDocument } from './DocumentViewer'
import { downloadFile, fileTypeLabel, fmtBytes } from '../../files'
import { useTSafe } from '../../i18n'

// Universal document row + View/Download action pair, used by every document surface in both
// SPAs instead of per-page copies of the icon/meta/blob-download logic. The row is layout-only;
// ViewDownloadActions is self-contained (owns its viewer modal, busy state and error text) so a
// page integrates a compliant document row in a couple of lines.

export interface DocInfo {
  title: string
  description?: string | null
  filename?: string | null
  mime?: string | null
  sizeBytes?: number | null
  version?: number | null
  /** Extra meta segments (doc type, dates, badges-as-text) shown dot-separated after size. */
  meta?: (string | null | undefined | false)[]
}

/** One document row: type icon, title (+version), meta line, description, and actions area. */
export function DocumentRow({ info, badge, actions }: { info: DocInfo; badge?: ReactNode; actions: ReactNode }) {
  const t = useTSafe()
  const metaLine = [fmtBytes(info.sizeBytes), ...(info.meta ?? [])].filter(Boolean).join(' · ')
  return (
    <div className="rep-item" style={{ alignItems: 'center' }}>
      <div className="rep-item-main row" style={{ gap: '.8rem' }}>
        <span className="res-ic" aria-hidden>{fileTypeLabel(info.mime, info.filename)}</span>
        <div>
          <strong>{info.title}</strong>
          {info.version != null && info.version > 1 && (
            <span className="badge" style={{ marginInlineStart: '.4rem' }}>{t('doc.version', { n: info.version })}</span>
          )}
          {badge}
          {metaLine && <div className="muted small">{metaLine}</div>}
          {info.description && <div className="muted small">{info.description}</div>}
        </div>
      </div>
      <div className="rep-item-actions" style={{ display: 'grid', gap: '.35rem', justifyItems: 'end' }}>
        {actions}
      </div>
    </div>
  )
}

export interface ViewDownloadProps {
  info: DocInfo
  /** Authenticated inline-serve URL (adds nothing itself — pass the `?inline=1` variant). */
  inlineUrl: string
  /** Save-to-disk URL; defaults to inlineUrl. */
  downloadUrl?: string | null
  token?: string | null
  canView?: boolean
  canDownload?: boolean
  canPrint?: boolean
  /** Extra buttons rendered after View/Download (Replace, Acknowledge, …). */
  children?: ReactNode
}

/** The universal View + Download pair. View opens the secure in-app viewer; Download streams the
 *  authenticated blob and saves it under its correct filename. Errors render inline, not alert(). */
export function ViewDownloadActions({ info, inlineUrl, downloadUrl, token, canView = true, canDownload = true, canPrint = true, children }: ViewDownloadProps) {
  const t = useTSafe()
  const [viewing, setViewing] = useState(false)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  const doc: ViewerDocument = {
    title: info.title,
    filename: info.filename,
    mime: info.mime,
    version: info.version,
    sizeBytes: info.sizeBytes,
    inlineUrl,
    downloadUrl: canDownload ? (downloadUrl || inlineUrl) : null,
  }

  const download = async () => {
    setBusy(true); setErr(null)
    try {
      await downloadFile(downloadUrl || inlineUrl, info.filename || info.title || 'document', token)
    } catch {
      setErr(t('doc.downloadError'))
    } finally { setBusy(false) }
  }

  return (
    <>
      <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
        {canView && <button className="btn sm secondary" onClick={() => { setErr(null); setViewing(true) }}>{t('doc.view')}</button>}
        {canDownload && (
          <button className="btn sm" onClick={download} disabled={busy}>
            {busy ? t('doc.preparing') : t('doc.download')}
          </button>
        )}
        {children}
      </div>
      {err && <span className="muted small" role="alert">{err}</span>}
      {viewing && (
        <DocumentViewer doc={doc} token={token} canDownload={canDownload} canPrint={canPrint} onClose={() => setViewing(false)} />
      )}
    </>
  )
}
