import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { I18nProvider } from '../i18n'

// FE — the Simulation Lab landing (Phase 1 foundation). Decisions pinned: the access gate (no access →
// the friendly reason, no catalogue fetch), the published-lab grid (kind/difficulty/competency badges),
// the attempt-status vs "Not started" marker, the "Open lab" link into the runner, and the empty state.
// useQuery is mocked at the module boundary and routed by path (access vs catalogue).
const h = vi.hoisted(() => ({
  access: null as unknown,
  cat: { rows: [] as unknown[] } as unknown,
  mastery: { mastery: [] as unknown[], recommended: [] as unknown[], weak_competencies: [] as string[] } as unknown,
  attempts: { rows: [] as unknown[] } as unknown,
}))

vi.mock('../api/hooks', () => ({
  useQuery: (path: string | null) => {
    // The catalogue is filtered SERVER-side since the P3 capacity path: the component puts
    // q/difficulty/kind/industry/competency in the query string and renders whatever comes
    // back, alongside a full-set `total`/`summary`. The mock emulates that contract — it
    // filters the fixture rows by the same params and reports full-set counts — so these
    // tests exercise the page against the response shape the real endpoint serves.
    let data: unknown = null
    if (path === '/api/me/lab/access') data = h.access
    else if (path === '/api/me/lab/mastery') data = h.mastery
    else if (path === '/api/me/lab/attempts') data = h.attempts
    else if (path !== null && path.startsWith('/api/me/lab/catalogue')) {
      const c = h.cat as { rows?: Record<string, unknown>[] } & Record<string, unknown>
      const all = c.rows ?? []
      const p = new URLSearchParams(path.split('?')[1] ?? '')
      const q = (p.get('q') ?? '').toLowerCase()
      const rows = all.filter((r) =>
        (!p.get('industry') || r.industry === p.get('industry')) &&
        (!p.get('difficulty') || r.difficulty === p.get('difficulty')) &&
        (!p.get('kind') || r.kind === p.get('kind')) &&
        (!p.get('competency') || (r.competencies as string[] | undefined)?.includes(p.get('competency') ?? '')) &&
        (!q || String(r.title ?? '').toLowerCase().includes(q)))
      const inProgress = all.filter((r) => r.attempt_status === 'in_progress')
      data = {
        total: all.length, matched: rows.length,
        summary: { published: all.length, completed: 0, in_progress: inProgress.length, avg_score: null },
        // The server computes the resume strip over the FULL catalogue, so it survives filters.
        resume: inProgress.map((r) => ({
          scenario_code: r.scenario_code, title: r.title, kind: r.kind,
          difficulty: r.difficulty, industry: r.industry, est_minutes: r.est_minutes,
        })),
        ...c, rows,
      }
    }
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))

import Lab from './Lab'

const renderLab = (initialEntries: string[] = ['/lab']) =>
  render(<I18nProvider><MemoryRouter initialEntries={initialEntries}><Lab /></MemoryRouter></I18nProvider>)

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
    expect(screen.getAllByText('Guided lab').length).toBeGreaterThanOrEqual(1) // badge (+ filter option)
    expect(screen.getByText(/Foundation · Construction/)).toBeInTheDocument()
    expect(screen.getAllByText('Scope structuring').length).toBeGreaterThanOrEqual(1)
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

  it('filters the catalogue by industry and shows track badges', () => {
    h.cat = {
      rows: [
        lab({ id: 1, scenario_code: 'GL-WBS-001', title: 'Structure a project WBS', industry: 'Construction', certification_id: 1 }),
        lab({ id: 2, scenario_code: 'GL-CASH-001', title: 'Model cash exposure', industry: 'Finance', certification_id: 2, competencies: ['cash_flow'] }),
      ],
    }
    renderLab()
    expect(screen.getByText('Structure a project WBS')).toBeInTheDocument()
    expect(screen.getByText('Model cash exposure')).toBeInTheDocument()
    fireEvent.change(screen.getByLabelText('Filter by industry'), { target: { value: 'Finance' } })
    expect(screen.queryByText('Structure a project WBS')).toBeNull()
    expect(screen.getByText('Model cash exposure')).toBeInTheDocument()
    expect(screen.getByText(/· PFL-AI/)).toBeInTheDocument()
  })

  it('announces the result count and restores the catalogue via Clear all filters', () => {
    h.cat = {
      rows: [
        lab({ id: 1, industry: 'Construction' }),
        lab({ id: 2, scenario_code: 'GL-CASH-001', title: 'Model cash exposure', industry: 'Finance' }),
      ],
    }
    renderLab()
    expect(screen.getByRole('status')).toHaveTextContent('Showing 2 of 2 labs')
    fireEvent.change(screen.getByLabelText('Filter by industry'), { target: { value: 'Finance' } })
    expect(screen.getByRole('status')).toHaveTextContent('Showing 1 of 2 labs')
    fireEvent.click(screen.getByRole('button', { name: 'Clear all filters' }))
    expect(screen.getByRole('status')).toHaveTextContent('Showing 2 of 2 labs')
    expect(screen.getByText('Structure a project WBS')).toBeInTheDocument()
  })

  it('surfaces in-progress labs in a resume strip', () => {
    h.cat = { rows: [lab({ attempt_status: 'in_progress' })] }
    renderLab()
    expect(screen.getByText('Continue where you left off')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Resume scenario' })).toHaveAttribute('href', '/lab/GL-WBS-001')
  })

  it('initialises filters from the URL so a filtered view survives reload', () => {
    h.cat = {
      rows: [
        lab({ id: 1, industry: 'Construction' }),
        lab({ id: 2, scenario_code: 'GL-CASH-001', title: 'Model cash exposure', industry: 'Finance' }),
      ],
    }
    renderLab(['/lab?industry=Finance'])
    expect(screen.getByLabelText('Filter by industry')).toHaveValue('Finance')
    expect(screen.getByRole('status')).toHaveTextContent('Showing 1 of 2 labs')
    expect(screen.queryByText('Structure a project WBS')).toBeNull()
  })

  it('opens the scenario details panel with both launch modes', () => {
    h.cat = { rows: [lab()] }
    renderLab()
    fireEvent.click(screen.getByRole('button', { name: 'Details — Structure a project WBS' }))
    const dialog = screen.getByRole('dialog', { name: 'Structure a project WBS' })
    expect(dialog).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Start in Training Mode' })).toHaveAttribute('href', '/lab/GL-WBS-001')
    expect(screen.getByRole('link', { name: 'Start in Assessment Mode' })).toHaveAttribute('href', '/lab/GL-WBS-001?mode=assessment')
    fireEvent.click(screen.getByRole('button', { name: 'Close details' }))
    expect(screen.queryByRole('dialog')).toBeNull()
  })

  it('renders competency progress meters from mastery data', () => {
    h.cat = { rows: [lab()] }
    h.mastery = {
      mastery: [{ competency: 'earned_value', avg_score: 74.2, attempts: 3, level: 'developing' }],
      recommended: [], weak_competencies: [],
    }
    renderLab()
    expect(screen.getByRole('progressbar', { name: 'Earned value: 74 out of 100' })).toBeInTheDocument()
    expect(screen.getByText(/3 attempts · Developing/)).toBeInTheDocument()
  })

  it('lists recent attempts with Resume and Review actions', () => {
    h.cat = { rows: [lab()] }
    h.attempts = {
      rows: [
        { id: 11, mode: 'training', status: 'in_progress', score: null, scenario_code: 'GL-WBS-001', title: 'Structure a project WBS', kind: 'guided_lab', started_at: '2026-07-20T10:00:00Z', completed_at: null },
        { id: 9, mode: 'assessment', status: 'passed', score: 92, scenario_code: 'GL-WBS-001', title: 'Structure a project WBS', kind: 'guided_lab', started_at: '2026-07-18T10:00:00Z', completed_at: '2026-07-18T10:20:00Z' },
      ],
    }
    renderLab()
    expect(screen.getByText('Recent attempts')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Resume' })).toHaveAttribute('href', '/lab/GL-WBS-001')
    expect(screen.getByRole('link', { name: 'Review' })).toHaveAttribute('href', '/lab/GL-WBS-001?attempt=9')
    expect(screen.getByText('92%')).toBeInTheDocument()
  })
})
