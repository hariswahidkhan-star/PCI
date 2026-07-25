// Shared file primitives for every document surface (student + admin SPAs).
//
// Before this module the authenticated-blob download, the File → data-URI reader and the
// byte formatter were each copy-pasted into a dozen pages. Every document surface now goes
// through these helpers so behaviour (auth, error text, object-URL lifetime) stays uniform.

/** Human file size (e.g. 812 KB, 3.4 MB) from a byte count, or null when unknown. */
export function fmtBytes(bytes?: number | null): string | null {
  if (bytes == null || bytes <= 0) return null
  if (bytes < 1024) return `${bytes} B`
  const kb = bytes / 1024
  if (kb < 1024) return `${kb < 10 ? kb.toFixed(1) : Math.round(kb)} KB`
  const mb = kb / 1024
  return `${mb < 10 ? mb.toFixed(1) : Math.round(mb)} MB`
}

/** Read a File as a data: URI (the JSON upload format every PCI upload endpoint accepts). */
export function fileToDataUri(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const r = new FileReader()
    r.onload = () => resolve(String(r.result))
    r.onerror = () => reject(new Error('Could not read the selected file.'))
    r.readAsDataURL(file)
  })
}

export class FileFetchError extends Error {
  status: number
  code?: string
  constructor(status: number, message: string, code?: string) {
    super(message)
    this.status = status
    this.code = code
  }
}

/** Fetch a protected file as a Blob with a bearer token. Never returns an HTML error page as
 *  a "file": a non-2xx response throws with the backend's error code/message when present. */
export async function fetchBlob(path: string, token?: string | null): Promise<Blob> {
  let res: Response
  try {
    res = await fetch(path, { headers: token ? { Authorization: 'Bearer ' + token } : {} })
  } catch {
    throw new FileFetchError(0, 'Network error — please check your connection and try again.')
  }
  if (!res.ok) {
    let code: string | undefined
    let message = `The file could not be retrieved (${res.status}).`
    try {
      const body = (await res.json()) as Record<string, unknown>
      if (typeof body.error === 'string') code = body.error
      if (typeof body.message === 'string' && body.message) message = body.message
      else if (code) message = code.replace(/_/g, ' ')
    } catch { /* non-JSON error body — keep the generic message */ }
    throw new FileFetchError(res.status, message, code)
  }
  return res.blob()
}

/** Save a Blob to disk via a temporary <a download>, revoking the object URL afterwards. */
export function saveBlob(blob: Blob, filename: string): void {
  const href = URL.createObjectURL(blob)
  const a = document.createElement('a')
  a.href = href
  a.download = filename
  document.body.appendChild(a)
  a.click()
  a.remove()
  setTimeout(() => URL.revokeObjectURL(href), 60_000)
}

/** Fetch an authenticated file and save it with its correct name. */
export async function downloadFile(path: string, filename: string, token?: string | null): Promise<void> {
  saveBlob(await fetchBlob(path, token), filename)
}

/** The student session token (mirrors api/client.ts's storage key without importing the client,
 *  so admin bundles that share these helpers don't pull the student client in). */
export function studentToken(): string | null {
  try { return sessionStorage.getItem('pci.session.token') } catch { return null }
}

// ---- lightweight kind detection for the viewer + icons ----

export type FileKind = 'pdf' | 'image' | 'text' | 'csv' | 'office' | 'archive' | 'other'

const IMAGE_MIMES = new Set(['image/png', 'image/jpeg', 'image/webp', 'image/gif'])
const OFFICE_MIMES = new Set([
  'application/msword',
  'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
  'application/vnd.ms-excel',
  'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
  'application/vnd.ms-powerpoint',
  'application/vnd.openxmlformats-officedocument.presentationml.presentation',
])

/** Classify a document by MIME (preferred) or filename extension. Drives which inline viewer
 *  is used; anything unknown falls back to a download-only panel. */
export function fileKind(mime?: string | null, filename?: string | null): FileKind {
  const m = (mime || '').toLowerCase()
  if (m === 'application/pdf') return 'pdf'
  if (IMAGE_MIMES.has(m)) return 'image'
  if (m === 'text/csv') return 'csv'
  if (m === 'text/plain') return 'text'
  if (OFFICE_MIMES.has(m)) return 'office'
  if (m === 'application/zip') return 'archive'
  const ext = (filename || '').toLowerCase().split('.').pop() || ''
  switch (ext) {
    case 'pdf': return 'pdf'
    case 'png': case 'jpg': case 'jpeg': case 'webp': case 'gif': return 'image'
    case 'csv': return 'csv'
    case 'txt': case 'log': case 'md': return 'text'
    case 'doc': case 'docx': case 'xls': case 'xlsx': case 'ppt': case 'pptx': return 'office'
    case 'zip': return 'archive'
    default: return 'other'
  }
}

/** Short uppercase label for the file-type icon chip (e.g. PDF, DOCX, CSV). */
export function fileTypeLabel(mime?: string | null, filename?: string | null): string {
  const ext = (filename || '').split('.').pop() || ''
  if (ext && ext.length <= 4 && /^[A-Za-z0-9]+$/.test(ext)) return ext.toUpperCase()
  switch (fileKind(mime, filename)) {
    case 'pdf': return 'PDF'
    case 'image': return 'IMG'
    case 'csv': return 'CSV'
    case 'text': return 'TXT'
    case 'office': return 'DOC'
    case 'archive': return 'ZIP'
    default: return 'FILE'
  }
}

/** Parse CSV text into rows for the safe tabular preview (quotes + embedded commas/newlines).
 *  Returns at most maxRows rows; the caller states when the preview is truncated. */
export function parseCsvPreview(text: string, maxRows = 200): string[][] {
  const rows: string[][] = []
  let row: string[] = []
  let cell = ''
  let inQuotes = false
  for (let i = 0; i < text.length && rows.length < maxRows; i++) {
    const c = text[i]
    if (inQuotes) {
      if (c === '"') {
        if (text[i + 1] === '"') { cell += '"'; i++ } else inQuotes = false
      } else cell += c
    } else if (c === '"') {
      inQuotes = true
    } else if (c === ',') {
      row.push(cell); cell = ''
    } else if (c === '\n' || c === '\r') {
      if (c === '\r' && text[i + 1] === '\n') i++
      row.push(cell); cell = ''
      if (row.length > 1 || row[0] !== '') rows.push(row)
      row = []
    } else cell += c
  }
  if (rows.length < maxRows && (cell !== '' || row.length > 0)) {
    row.push(cell)
    if (row.length > 1 || row[0] !== '') rows.push(row)
  }
  return rows
}
