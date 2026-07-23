import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen } from '@testing-library/react'

// FE — the Simulation Lab landing (Phase 1 foundation). Decisions pinned: the access gate (no access →
// the friendly reason, no catalogue fetch), the published-lab grid (kind/difficulty/competency badges),
// the attempt-status vs "Not started" marker, and the empty state. useQuery is mocked at the module
// boundary and routed by path (access vs catalogue).
const h = vi.hoisted(() => ({ access: null as unknown, cat: { rows: [] as unknown[] } as unknown }))

vi.mock('../api/hooks', () => ({
  useQuery: (path: string | null) => {
    const data = path === '/api/me/lab/access' ? h.access : path === '/api/me/lab/catalogue' ? h.cat : null
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))

import Lab from './Lab'

const lab = (over: Record<string, unknown> = {}) => ({
  id: 1, scenario_code: 'GL-WBS-001', title: 'Structure a project WBS', kind: 'guided_lab',
  industry: 'Construction', difficulty: 'foundation', est_minutes: 15,
  competencies: ['scope_structuring'], summary: 'Build a WBS.', version: 1, attempt_status: null, score: null,
  ...over,
})

describe('Lab (Simulation Lab landing)', () => {
  beforeEach(() => {
    h.access = { enabled: true, has_access: true, reason: 'ok' }
    h.cat = { rows: [] }
  })

  it('gates on access and shows the friendly reason when the student has none', () => {
    h.access = { enabled: true, has_access: false, reason: 'Practice Lab access is included with an active PCI membership.' }
    render(<Lab />)
    expect(screen.getByText('Practice Lab access is included with an active PCI membership.')).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Open lab' })).toBeNull() // no catalogue rendered
  })

  it('renders the published labs with kind, difficulty and competency badges', () => {
    h.cat = { rows: [lab()] }
    render(<Lab />)
    expect(screen.getByText('Structure a project WBS')).toBeInTheDocument()
    expect(screen.getByText('Guided lab')).toBeInTheDocument()          // kind label
    expect(screen.getByText(/Foundation/)).toBeInTheDocument()          // difficulty
    expect(screen.getByText('Scope structuring')).toBeInTheDocument()   // competency label
    expect(screen.getByText('Not started')).toBeInTheDocument()
  })

  it('shows the attempt status once a lab has been attempted', () => {
    h.cat = { rows: [lab({ attempt_status: 'completed' })] }
    render(<Lab />)
    expect(screen.getByText('Completed')).toBeInTheDocument()
    expect(screen.queryByText('Not started')).toBeNull()
  })

  it('shows the empty state when access is granted but nothing is published', () => {
    h.cat = { rows: [] }
    render(<Lab />)
    expect(screen.getByText(/No practice labs have been published yet/)).toBeInTheDocument()
  })
})
