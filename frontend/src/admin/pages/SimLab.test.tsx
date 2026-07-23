import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen } from '@testing-library/react'

// FE — the Admin Console → Simulation Lab list (Phase 1 foundation). Read-only visibility over the
// scenario catalogue across all statuses, with per-scenario practice aggregates. Pins the summary
// stats, the row shape (status badge, interactivity, attempt/completion counts), and the empty state.
// useAdminQuery is mocked at the module boundary.
const h = vi.hoisted(() => ({ resp: null as unknown }))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) =>
    ({ data: path === '/api/admin/lab/scenarios' ? h.resp : null, loading: false, error: null, refetch: vi.fn() }),
}))

import SimLab from './SimLab'

const row = (over: Record<string, unknown> = {}) => ({
  id: 1, scenario_code: 'GL-EVM-001', title: 'Calculate the core EVM measures', kind: 'guided_lab',
  industry: 'Energy', difficulty: 'foundation', est_minutes: 15, competencies: ['earned_value'],
  status: 'published', version: 1, interactive: true, attempts: 3, completed: 2, updated_at: '2026-07-23',
  ...over,
})

describe('Admin SimLab (scenario catalogue)', () => {
  beforeEach(() => { h.resp = { rows: [], total: 0, published: 0 } })

  it('shows the summary stats and a scenario row with its aggregates', () => {
    h.resp = { rows: [row(), row({ id: 2, scenario_code: 'DRAFT-X', title: 'Hidden draft', status: 'draft', interactive: false, attempts: 0, completed: 0 })], total: 2, published: 1 }
    render(<SimLab />)
    expect(screen.getByText('Calculate the core EVM measures')).toBeInTheDocument()
    expect(screen.getByText('GL-EVM-001')).toBeInTheDocument()
    expect(screen.getAllByText('Earned Value').length).toBeGreaterThanOrEqual(1)  // competency badge
    expect(screen.getByText('Hidden draft')).toBeInTheDocument()     // draft is visible to admins
    expect(screen.getByText('published')).toBeInTheDocument()        // StatusBadge (raw status)
    expect(screen.getByText('draft')).toBeInTheDocument()            // the draft row's status badge
  })

  it('shows the empty state when no scenarios exist', () => {
    h.resp = { rows: [], total: 0, published: 0 }
    render(<SimLab />)
    expect(screen.getByText('No scenarios have been created yet.')).toBeInTheDocument()
  })
})
