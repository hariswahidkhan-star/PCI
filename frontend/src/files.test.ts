import { describe, it, expect, vi, afterEach } from 'vitest'
import { fmtBytes, fileKind, fileTypeLabel, parseCsvPreview, fetchBlob, FileFetchError } from './files'

// FE — the shared file primitives every document surface now uses (byte formatting, kind
// detection for the viewer, safe CSV parsing, and the authenticated blob fetch that must never
// hand an HTML error page over as if it were the file).

describe('fmtBytes', () => {
  it('formats across unit boundaries', () => {
    expect(fmtBytes(null)).toBeNull()
    expect(fmtBytes(0)).toBeNull()
    expect(fmtBytes(512)).toBe('512 B')
    expect(fmtBytes(2048)).toBe('2.0 KB')
    expect(fmtBytes(204800)).toBe('200 KB')
    expect(fmtBytes(3.4 * 1024 * 1024)).toBe('3.4 MB')
  })
})

describe('fileKind / fileTypeLabel', () => {
  it('classifies by MIME first', () => {
    expect(fileKind('application/pdf', 'x.bin')).toBe('pdf')
    expect(fileKind('image/webp', null)).toBe('image')
    expect(fileKind('text/csv', null)).toBe('csv')
    expect(fileKind('text/plain', null)).toBe('text')
    expect(fileKind('application/vnd.openxmlformats-officedocument.wordprocessingml.document', null)).toBe('office')
    expect(fileKind('application/zip', null)).toBe('archive')
  })
  it('falls back to the filename extension', () => {
    expect(fileKind(null, 'report.PDF')).toBe('pdf')
    expect(fileKind('', 'notes.md')).toBe('text')
    expect(fileKind(undefined, 'deck.pptx')).toBe('office')
    expect(fileKind(undefined, 'mystery')).toBe('other')
  })
  it('labels the icon chip from the extension, capped at 4 chars', () => {
    expect(fileTypeLabel('application/pdf', 'handbook.pdf')).toBe('PDF')
    expect(fileTypeLabel(null, 'x.docx')).toBe('DOCX')
    expect(fileTypeLabel('image/png', null)).toBe('IMG')
    expect(fileTypeLabel(null, null)).toBe('FILE')
  })
})

describe('parseCsvPreview', () => {
  it('parses quoted cells, embedded commas and newlines', () => {
    const rows = parseCsvPreview('a,b,c\n"1,5","line\nbreak","he said ""hi"""\nx,y,z')
    expect(rows).toEqual([
      ['a', 'b', 'c'],
      ['1,5', 'line\nbreak', 'he said "hi"'],
      ['x', 'y', 'z'],
    ])
  })
  it('handles CRLF and trailing newline without phantom rows', () => {
    expect(parseCsvPreview('a,b\r\nc,d\r\n')).toEqual([['a', 'b'], ['c', 'd']])
  })
  it('caps at maxRows', () => {
    const text = Array.from({ length: 50 }, (_, i) => `r${i},v${i}`).join('\n')
    expect(parseCsvPreview(text, 10)).toHaveLength(10)
  })
})

describe('fetchBlob', () => {
  afterEach(() => vi.unstubAllGlobals())

  it('returns the blob on success and sends the bearer token', async () => {
    const fetchMock = vi.fn(async () => new Response('%PDF-1.7', { status: 200, headers: { 'Content-Type': 'application/pdf' } }))
    vi.stubGlobal('fetch', fetchMock)
    const got = await fetchBlob('/api/me/documents/1/download', 'tok123')
    expect(await got.text()).toBe('%PDF-1.7')
    expect(fetchMock).toHaveBeenCalledWith('/api/me/documents/1/download', { headers: { Authorization: 'Bearer tok123' } })
  })

  it('throws with the backend error message instead of returning an error page as a file', async () => {
    vi.stubGlobal('fetch', vi.fn(async () =>
      new Response(JSON.stringify({ error: 'not_assigned', message: 'You do not have access to this document.' }), { status: 403 })))
    await expect(fetchBlob('/api/me/documents/1/download')).rejects.toMatchObject({
      status: 403, code: 'not_assigned', message: 'You do not have access to this document.',
    })
  })

  it('wraps network failures in FileFetchError', async () => {
    vi.stubGlobal('fetch', vi.fn(async () => { throw new TypeError('offline') }))
    await expect(fetchBlob('/x')).rejects.toBeInstanceOf(FileFetchError)
  })
})
