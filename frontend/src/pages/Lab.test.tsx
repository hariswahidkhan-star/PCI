import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'

// FE — the Simulation Lab landing (Phase 1 foundation). Decisions pinned: the access gate (no access →
// the friendly reason, no catalogue fetch), the published-lab grid (kind/difficulty/competency badges),
// the attempt-status vs "Not started" marker, the "Open lab" link into the runner, and the empty state.
// useQuery is mocked at the module boundary and routed by path (access vs catalogue).
const h = vi.hoisted(() => ({ access: null as unknown, cat: { rows: [] as unknown[] } as unknown }))

vi.mock('../api/hooks', () => ({
  useQuery: (path: string | null) => {
    const data = path === '/api/me/lab/access' ? h.access : path === '/api/me/lab/catalogue' ? h.cat : null
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))

import Lab from './Lab'

const renderLab = () => render(<MemoryRouter><Lab /></MemoryRouter>)

const lab = (over: Record<string, unknown> = {}) => ({
  id: 1, scenario_code: 'GL-WBS-001', title: 'Structure a project WBS', kind: 'guided_lab',
  industry: 'Construction', difficulty: 'foundation', est_minutes: 15,
  competencies: ['scope_structuring'], summary: 'Build a WBS.', version: 1,
  interactive: true, attempt_status: null, score: null,
  ...over,
})

describe('Lab (Simulation Lab landing)', () => {
  beforeEach(() => {
    h.access = { enabled: true, has_access: true, reason: 'ok' }
    h.cat = { rows: [] }
  })

  it('gates on access and shows the friendly reason when the student has none', () => {
    h.access = { enabled: true, has_access: false, reason: 'Practice Lab access is included with an active PCI membership.' }
    renderLab()
    expect(screen.getByText('Practice Lab access is included with an active PCI membership.')).toBeInTheDocument()
    expect(screen.queryByRole('link', { name: /Open lab/ })).toBeNull() // no catalogue rendered
  })

  it('renders the published labs with kind, difficulty and competency badges and a runner link', () => {
    h.cat = { rows: [lab()] }
    renderLab()
    expect(screen.getByText('Structure a project WBS')).toBeInTheDocument()
    expect(screen.getByText('Guided lab')).toBeInTheDocument()          // kind label
    expect(screen.getByText(/Foundation/)).toBeInTheDocument()          // difficulty
    expect(screen.getByText('Scope structuring')).toBeInTheDocument()   // competency label
    expect(screen.getByText('Not started')).toBeInTheDocument()
    const open = screen.getByRole('link', { name: 'Open lab' })
    expect(open).toHaveAttribute('href', '/lab/GL-WBS-001')
  })

  it('shows the attempt status and an "Open again" link once a lab has been attempted', () => {
    h.cat = { rows: [lab({ attempt_status: 'completed' })] }
    renderLab()
    expect(screen.getByText('Completed')).toBeInTheDocument()
    expect(screen.queryByText('Not started')).toBeNull()
    expect(screen.getByRole('link', { name: 'Open again' })).toBeInTheDocument()
  })

  it('shows the empty state when access is granted but nothing is published', () => {
    h.cat = { rows: [] }
    renderLab()
    expect(screen.getByText(/No practice labs have been published yet/)).toBeInTheDocument()
  })
})
