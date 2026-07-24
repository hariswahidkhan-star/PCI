import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'

// FE — Admin Console → Free Templates Library (§6A–6C). Pins the summary stats, a template row with its
// published badge, the create-form toggle, the empty state, and the publish action. useAdminQuery and the
// admin API client are mocked at the module boundary.
const h = vi.hoisted(() => ({ resp: null as unknown, an: null as unknown }))
vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => ({
    data: path === '/api/admin/templates' ? h.resp : path === '/api/admin/templates/analytics' ? h.an : null,
    loading: false, error: null, refetch: vi.fn(),
  }),
}))
const api = vi.hoisted(() => ({ post: vi.fn(), get: vi.fn(), patch: vi.fn(), del: vi.fn() }))
vi.mock('../api', () => ({ adminApi: api }))

import Templates from './Templates'

const row = (over: Record<string, unknown> = {}) => ({
  id: 1, slug: 'wbs-template', title: 'Work Breakdown Structure (WBS) template', category: 'scope',
  certification_id: 1, summary: 'Roll scope down to work packages.', format: 'csv',
  body: 'WBS ID,Parent ID\n1,,', published: true, sort_order: 0, download_count: 7,
  ...over,
})

describe('Admin Free Templates', () => {
  beforeEach(() => { h.resp = { rows: [], total: 0, published: 0 }; h.an = null; api.post.mockReset(); api.patch.mockReset() })

  it('shows the library, stats and a template row with its published badge', () => {
    h.resp = {
      rows: [row(), row({ id: 2, slug: 'draft-x', title: 'Hidden draft', published: false, download_count: 0 })],
      total: 2, published: 1,
    }
    render(<Templates />)
    expect(screen.getByRole('heading', { name: 'Free Templates Library' })).toBeInTheDocument()
    expect(screen.getByText('Work Breakdown Structure (WBS) template')).toBeInTheDocument()
    expect(screen.getByText('Hidden draft')).toBeInTheDocument()   // draft visible to admins
    expect(screen.getByText('Draft')).toBeInTheDocument()
    expect(screen.getAllByText('Published').length).toBeGreaterThanOrEqual(1)
  })

  it('reveals the create form when + New template is clicked', () => {
    render(<Templates />)
    expect(screen.queryByText('New template')).not.toBeInTheDocument()
    fireEvent.click(screen.getByRole('button', { name: '+ New template' }))
    expect(screen.getByText('New template')).toBeInTheDocument()
    expect(screen.getByPlaceholderText('wbs-template')).toBeInTheDocument()
  })

  it('publishes a draft via the admin API', () => {
    h.resp = { rows: [row({ published: false })], total: 1, published: 0 }
    api.patch.mockResolvedValue({})
    render(<Templates />)
    fireEvent.click(screen.getByRole('button', { name: 'Publish' }))
    expect(api.patch).toHaveBeenCalledWith('/api/admin/templates/1', { published: true })
  })

  it('shows the empty state when no templates exist', () => {
    h.resp = { rows: [], total: 0, published: 0 }
    render(<Templates />)
    expect(screen.getByText('No templates yet — create the first one.')).toBeInTheDocument()
  })

  it('renders the download analytics panel with an SVG trend and a top list', () => {
    h.resp = { rows: [row()], total: 1, published: 1 }
    h.an = {
      total_downloads: 42, templates: 1, published: 1, window_days: 30, window_downloads: 12,
      series: Array.from({ length: 30 }, (_, i) => ({ day: `2026-07-${String(i + 1).padStart(2, '0')}`, count: i % 4 })),
      top: [{ slug: 'wbs-template', title: 'Roll-down WBS pack', download_count: 42, published: true }],
    }
    render(<Templates />)
    expect(screen.getByText('Total downloads')).toBeInTheDocument()
    expect(screen.getByText('Most downloaded')).toBeInTheDocument()
    expect(screen.getByText('Roll-down WBS pack')).toBeInTheDocument()
    expect(screen.getByRole('img', { name: /Downloads over the last 30 days/i })).toBeInTheDocument()
  })
})
