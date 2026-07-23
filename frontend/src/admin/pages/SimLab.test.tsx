import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'

// FE — the Admin Console → Simulation Lab Studio (Phase 5A). A governed authoring surface over the scenario
// engine: create a draft, validate against the §14 publication gate, walk the review workflow, revise a
// published version. Pins the summary stats, the row's review-state badge, the create-form toggle, and the
// on-demand validation verdict. useAdminQuery and the admin API client are mocked at the module boundary.
const h = vi.hoisted(() => ({ resp: null as unknown }))
vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) =>
    ({ data: path === '/api/admin/lab/scenarios' ? h.resp : null, loading: false, error: null, refetch: vi.fn() }),
}))
const api = vi.hoisted(() => ({ post: vi.fn(), get: vi.fn(), patch: vi.fn(), del: vi.fn() }))
vi.mock('../api', () => ({ adminApi: api }))

import SimLab from './SimLab'

const row = (over: Record<string, unknown> = {}) => ({
  id: 1, scenario_code: 'GL-EVM-001', title: 'Calculate the core EVM measures', kind: 'guided_lab',
  industry: 'Energy', difficulty: 'foundation', competencies: ['earned_value'],
  status: 'published', review_state: 'published', version: 1, interactive: true, attempts: 3, completed: 2,
  ...over,
})

describe('Admin SimLab Studio', () => {
  beforeEach(() => { h.resp = { rows: [], total: 0, published: 0 }; api.post.mockReset(); api.get.mockReset() })

  it('shows the studio, stats and a scenario row with its review state', () => {
    h.resp = {
      rows: [row(), row({ id: 2, scenario_code: 'DRAFT-X', title: 'Hidden draft', status: 'draft', review_state: 'draft', interactive: false, attempts: 0, completed: 0 })],
      total: 2, published: 1,
    }
    render(<SimLab />)
    expect(screen.getByRole('heading', { name: 'Simulation Lab Studio' })).toBeInTheDocument()
    expect(screen.getByText('Calculate the core EVM measures')).toBeInTheDocument()
    expect(screen.getByText('Hidden draft')).toBeInTheDocument()  // draft visible to admins
    expect(screen.getByText('Draft')).toBeInTheDocument()          // review-state badge (title-cased)
    expect(screen.getAllByText('Published').length).toBeGreaterThanOrEqual(1)
  })

  it('reveals the create form when + New scenario is clicked', () => {
    render(<SimLab />)
    expect(screen.queryByText('New draft scenario')).not.toBeInTheDocument()
    fireEvent.click(screen.getByRole('button', { name: '+ New scenario' }))
    expect(screen.getByText('New draft scenario')).toBeInTheDocument()
    expect(screen.getByPlaceholderText('GL-EVM-010')).toBeInTheDocument()
  })

  it('validates a scenario on demand and shows the verdict', async () => {
    h.resp = { rows: [row()], total: 1, published: 1 }
    api.get.mockResolvedValue({ id: 1, scenario_code: 'GL-EVM-001', review_state: 'published', publishable: true, errors: 0, warnings: 0, issues: [] })
    render(<SimLab />)
    fireEvent.click(screen.getByRole('button', { name: 'Validate' }))
    expect(api.get).toHaveBeenCalledWith('/api/admin/lab/scenarios/1/validate')
    expect(await screen.findByText('Publishable')).toBeInTheDocument()
  })

  it('offers the next workflow step for a draft scenario', () => {
    h.resp = { rows: [row({ status: 'draft', review_state: 'draft' })], total: 1, published: 0 }
    render(<SimLab />)
    // draft → calc_review is the single forward step
    expect(screen.getByRole('button', { name: '→ Calc Review' })).toBeInTheDocument()
  })

  it('flags a scenario whose review-due date has passed', () => {
    h.resp = { rows: [row({ governance: 'review_overdue', review_due: '2020-01-01', days_to_review: -900 })], total: 1, published: 1 }
    render(<SimLab />)
    expect(screen.getByText('Review overdue')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Dates' })).toBeInTheDocument()
  })

  it('shows the empty state when no scenarios exist', () => {
    h.resp = { rows: [], total: 0, published: 0 }
    render(<SimLab />)
    expect(screen.getByText('No scenarios have been created yet.')).toBeInTheDocument()
  })
})
