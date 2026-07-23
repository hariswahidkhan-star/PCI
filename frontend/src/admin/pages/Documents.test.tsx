import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE-12 — the admin Documents console manages private, per-student documents (upload → target →
// publish → version). This pins the roster surface: a document row with its status badge, the
// status-filter query path, and the empty state. useAdminQuery + adminApi mocked at the boundary.
const h = vi.hoisted(() => ({ docs: { rows: [] as unknown[] }, paths: [] as (string | null)[] }))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    h.paths.push(path)
    let data: unknown = null
    if (path === '/api/admin/document-categories') data = { rows: [] }
    else if (path && path.startsWith('/api/admin/documents')) data = h.docs
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))
vi.mock('../api', () => ({ adminApi: { post: vi.fn(), patch: vi.fn(), getToken: () => 't' } }))

import Documents from './Documents'

const doc = (over: Record<string, unknown> = {}) => ({
  id: 3, title: 'Enrolment letter', filename: 'letter.pdf', status: 'published', version: 1,
  category: 'general', created_at: '2025-02-01T00:00:00Z', ...over,
})

describe('Documents admin console', () => {
  beforeEach(() => { h.docs = { rows: [] }; h.paths = [] })

  it('renders a document row with its title and status', () => {
    h.docs = { rows: [doc()] }
    render(<Documents />)
    const row = screen.getByText('Enrolment letter').closest('tr') as HTMLElement
    expect(within(row).getByText('letter.pdf')).toBeInTheDocument()
    expect(within(row).getByText('published')).toBeInTheDocument()
  })

  it('folds a status filter into the documents query path', async () => {
    const user = userEvent.setup()
    render(<Documents />)
    const docPaths = () => h.paths.filter((p) => p && p.startsWith('/api/admin/documents'))
    expect(docPaths()[0]).toBe('/api/admin/documents')
    await user.selectOptions(screen.getByLabelText('Status filter'), 'archived')
    await waitFor(() => expect(docPaths()[docPaths().length - 1]).toContain('status=archived'))
  })

  it('shows the empty state when no documents match', () => {
    render(<Documents />)
    expect(screen.getByText(/No documents match/)).toBeInTheDocument()
  })
})
