import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { I18nProvider } from '../i18n'

// FE — the student "My Documents" page. Its real logic is renderActions(): a per-document state
// machine with a strict precedence — time-restricted → locked → acknowledgement-required →
// downloadable → unavailable — plus grouping by category, the version badge, and the empty state.
// This drives each branch. The two data hooks (documents + the embedded BooksSection's
// cert-documents) and the fetch client are mocked at the module boundary.
const h = vi.hoisted(() => ({
  docs: { rows: [] as unknown[] } as unknown,
  books: { rows: [] } as unknown,
  post: vi.fn(),
  refetch: vi.fn(),
}))

vi.mock('../api/hooks', () => ({
  useQuery: (path: string) => {
    const data = path === '/api/me/documents' ? h.docs : path === '/api/me/cert-documents' ? h.books : null
    return { data, loading: false, error: null, refetch: h.refetch }
  },
}))
vi.mock('../api/client', () => ({ api: { post: (p: string, b?: unknown) => h.post(p, b) } }))

import Documents from './Documents'

const renderPage = () => render(<I18nProvider><Documents /></I18nProvider>)

const doc = (over: Record<string, unknown> = {}) => ({
  id: 5,
  title: 'Candidate handbook',
  description: null,
  category: 'General',
  doc_type: 'PDF',
  filename: 'handbook.pdf',
  mime: 'application/pdf',
  size_bytes: 204800,
  version: 1,
  view_only: false,
  ack_required: false,
  acknowledged: false,
  restricted_until: null,
  restricted: false,
  expires_at: null,
  published_at: null,
  downloadable: true,
  locked: false,
  lock_reason: null,
  ...over,
})

describe('Documents (student — renderActions state machine)', () => {
  beforeEach(() => {
    h.docs = { rows: [] }
    h.books = { rows: [] }
    h.post.mockReset()
    h.refetch.mockReset()
  })

  it('shows the empty state when the student has no documents', () => {
    renderPage()
    expect(screen.getByText("You don't have any documents yet.")).toBeInTheDocument()
  })

  it('shows a time-restricted document as an "Available {date}" badge, not an action', () => {
    h.docs = { rows: [doc({ restricted: true, restricted_until: '2027-01-01T00:00:00Z', downloadable: false })] }
    renderPage()
    expect(screen.getByText(/Available/)).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Download' })).toBeNull()
    expect(screen.queryByRole('button', { name: 'View' })).toBeNull()
  })

  it('shows the friendly lock label for a locked document', () => {
    h.docs = { rows: [doc({ locked: true, lock_reason: 'archived', downloadable: false })] }
    renderPage()
    expect(screen.getByText('Archived')).toBeInTheDocument() // titleCase(lock_reason)
    expect(screen.queryByRole('button', { name: 'Download' })).toBeNull()
  })

  it('requires acknowledgement before download and posts it on click', async () => {
    const user = userEvent.setup()
    h.docs = { rows: [doc({ ack_required: true, acknowledged: false, downloadable: false })] }
    renderPage()
    expect(screen.getByText('Acknowledgement required before download.')).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Download' })).toBeNull()
    await user.click(screen.getByRole('button', { name: 'Acknowledge' }))
    await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/me/documents/5/acknowledge', {}))
    expect(h.refetch).toHaveBeenCalled()
  })

  it('gives a view-only document View but never Download', () => {
    h.docs = { rows: [doc({ view_only: true, downloadable: true })] }
    renderPage()
    expect(screen.getByRole('button', { name: 'View' })).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Download' })).toBeNull()
  })

  it('pairs View AND Download on a downloadable document, and marks it acknowledged', () => {
    h.docs = { rows: [doc({ downloadable: true, ack_required: true, acknowledged: true })] }
    renderPage()
    expect(screen.getByRole('button', { name: 'View' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Download' })).toBeInTheDocument()
    expect(screen.getByText('Acknowledged')).toBeInTheDocument()
  })

  it('opens the secure viewer from View (dialog with title, loading state, close)', async () => {
    const user = userEvent.setup()
    const fetchMock = vi.fn(() => new Promise<Response>(() => { /* stay loading */ }))
    vi.stubGlobal('fetch', fetchMock)
    h.docs = { rows: [doc({ downloadable: true })] }
    renderPage()
    await user.click(screen.getByRole('button', { name: 'View' }))
    const dialog = await screen.findByRole('dialog', { name: 'Candidate handbook' })
    expect(dialog).toBeInTheDocument()
    expect(fetchMock).toHaveBeenCalledWith('/api/me/documents/5/download?inline=1', expect.anything())
    await user.click(screen.getByRole('button', { name: 'Close' }))
    expect(screen.queryByRole('dialog')).toBeNull()
    vi.unstubAllGlobals()
  })

  it('groups documents by category and shows the version badge past v1', () => {
    h.docs = { rows: [doc({ id: 9, title: 'Exam rules', category: 'Policies', version: 3 })] }
    renderPage()
    expect(screen.getByText('Policies')).toBeInTheDocument() // category group card title
    expect(screen.getByText('Version 3')).toBeInTheDocument()
  })

  it('books section pairs View and Download for stored files', () => {
    h.books = { rows: [{ id: 2, kind: 'bok', title: 'PFL-AI Body of Knowledge', watermark: 1, has_file: 1, filename: 'bok.pdf', size_bytes: 1024 }] }
    renderPage()
    expect(screen.getByText('PFL-AI Body of Knowledge')).toBeInTheDocument()
    expect(screen.getByText(/Personalised copy/)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'View' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Download' })).toBeInTheDocument()
  })
})
