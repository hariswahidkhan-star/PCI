import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent, act } from '@testing-library/react'

// FE — the Admin Console → Simulation Lab Studio (Phase 5A). A governed authoring surface over the scenario
// engine: create a draft, validate against the §14 publication gate, walk the review workflow, revise a
// published version. Pins the summary stats, the row's review-state badge, the create-form toggle, and the
// on-demand validation verdict. useAdminQuery and the admin API client are mocked at the module boundary.
const h = vi.hoisted(() => ({ resp: null as unknown }))
vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) =>
    ({ data: path === '/api/admin/lab/scenarios' ? h.resp : null, loading: false, error: null, refetch: vi.fn() }),
}))
const api = vi.hoisted(() => ({ post: vi.fn(), get: vi.fn(), patch: vi.fn(), del: vi.fn(), getToken: vi.fn(() => 'admin-tok') }))
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
    // review-state badge (title-cased); also appears as a review-state filter option
    expect(screen.getAllByText('Draft').length).toBeGreaterThanOrEqual(1)
    expect(screen.getAllByText('Published').length).toBeGreaterThanOrEqual(1)
  })

  it('filters the scenario table by search text and announces the result count', () => {
    h.resp = {
      rows: [
        row(),
        row({ id: 2, scenario_code: 'GL-CPM-001', title: 'Analyse the critical path', competencies: ['schedule_analysis'] }),
      ],
      total: 2, published: 2,
    }
    render(<SimLab />)
    expect(screen.getByRole('status')).toHaveTextContent('Showing 2 of 2 scenarios')
    fireEvent.change(screen.getByLabelText('Search scenarios'), { target: { value: 'critical path' } })
    expect(screen.getByRole('status')).toHaveTextContent('Showing 1 of 2 scenarios')
    expect(screen.queryByText('Calculate the core EVM measures')).toBeNull()
    expect(screen.getByText('Analyse the critical path')).toBeInTheDocument()
  })

  it('sorts the table by attempts and flips direction on a second click', () => {
    h.resp = {
      rows: [
        row({ id: 1, scenario_code: 'A-LOW', title: 'Low attempts', attempts: 2 }),
        row({ id: 2, scenario_code: 'B-HIGH', title: 'High attempts', attempts: 90 }),
      ],
      total: 2, published: 2,
    }
    render(<SimLab />)
    const attemptsSort = screen.getByRole('button', { name: 'Attempts' })
    fireEvent.click(attemptsSort) // ascending
    let cells = screen.getAllByRole('row').slice(1).map((r) => r.textContent ?? '')
    expect(cells[0]).toContain('A-LOW')
    fireEvent.click(attemptsSort) // descending
    cells = screen.getAllByRole('row').slice(1).map((r) => r.textContent ?? '')
    expect(cells[0]).toContain('B-HIGH')
  })

  it('shows the completion rate per scenario and sorts by it', () => {
    h.resp = {
      rows: [
        row({ id: 1, scenario_code: 'A-HALF', title: 'Half done', attempts: 10, completed: 5 }),
        row({ id: 2, scenario_code: 'B-FULL', title: 'All done', attempts: 4, completed: 4 }),
        row({ id: 3, scenario_code: 'C-NONE', title: 'Untouched', attempts: 0, completed: 0 }),
      ],
      total: 3, published: 3,
    }
    render(<SimLab />)
    expect(screen.getByText('50%')).toBeInTheDocument()
    expect(screen.getByText('100%')).toBeInTheDocument()
    fireEvent.click(screen.getByRole('button', { name: 'Completion' })) // ascending: unattempted first
    const rowsText = screen.getAllByRole('row').slice(1).map((r) => r.textContent ?? '')
    expect(rowsText[0]).toContain('C-NONE')
    expect(rowsText[2]).toContain('B-FULL')
  })

  it('asks for confirmation before retiring a published scenario', () => {
    h.resp = { rows: [row()], total: 1, published: 1 }
    api.post.mockResolvedValue({})
    render(<SimLab />)
    fireEvent.click(screen.getByRole('button', { name: 'Retire' }))
    expect(api.post).not.toHaveBeenCalled() // nothing happens until confirmed
    expect(screen.getByRole('dialog', { name: 'Retire GL-EVM-001?' })).toBeInTheDocument()
    fireEvent.click(screen.getByRole('button', { name: 'Retire scenario' }))
    expect(api.post).toHaveBeenCalledWith('/api/admin/lab/scenarios/1/review', { to: 'retired' })
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

  // §5B.4 manifest export. The download must carry the server's exact bytes (the manifest is meant to be
  // diffed and checksum-compared), so the page fetches raw text rather than round-tripping the JSON client.
  describe('manifest export', () => {
    const body = '{\n  "manifest_version": 1,\n  "checksum": "abc123",\n  "scenario": {}\n}'
    let blobs: Blob[]
    let clicked: HTMLAnchorElement[]

    const mockFetch = (dispo: string | null) => {
      const f = vi.fn().mockResolvedValue({
        ok: true,
        text: () => Promise.resolve(body),
        headers: { get: (h: string) => (h === 'Content-Disposition' ? dispo : null) },
      })
      vi.stubGlobal('fetch', f)
      return f
    }

    beforeEach(() => {
      blobs = []; clicked = []
      URL.createObjectURL = vi.fn((b: Blob) => { blobs.push(b); return 'blob:manifest' })
      URL.revokeObjectURL = vi.fn()
      vi.spyOn(HTMLAnchorElement.prototype, 'click').mockImplementation(function (this: HTMLAnchorElement) { clicked.push(this) })
      h.resp = { rows: [row()], total: 1, published: 1 }
    })
    afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

    it('downloads the raw manifest bytes under the filename the server chose', async () => {
      const f = mockFetch('attachment; filename="GL-EVM-001-v1.pcisim.json"')
      render(<SimLab />)
      await act(async () => { fireEvent.click(screen.getByRole('button', { name: 'Export' })) })

      expect(f).toHaveBeenCalledWith('/api/admin/lab/scenarios/1/manifest',
        { headers: { Authorization: 'Bearer admin-tok' } })
      expect(screen.getByText('Exported manifest for “GL-EVM-001”.')).toBeInTheDocument()
      expect(clicked[0].download).toBe('GL-EVM-001-v1.pcisim.json')
      // byte-for-byte: the blob is the server's response, not a re-serialised object
      expect(blobs[0].size).toBe(body.length)
      expect(blobs[0].type).toBe('application/json')
      expect(URL.revokeObjectURL).toHaveBeenCalledWith('blob:manifest')
    })

    it('falls back to a sanitised local filename when the server sends no disposition', async () => {
      mockFetch(null)
      h.resp = { rows: [row({ scenario_code: 'bad"code/x', version: 2 })], total: 1, published: 1 }
      render(<SimLab />)
      await act(async () => { fireEvent.click(screen.getByRole('button', { name: 'Export' })) })
      expect(screen.getByText(/Exported manifest/)).toBeInTheDocument()
      expect(clicked[0].download).toBe('badcodex-v2.pcisim.json')
    })

    it('surfaces a failed export instead of downloading an error page', async () => {
      vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
        ok: false, status: 403, text: () => Promise.resolve('{"error":"forbidden"}'),
        headers: { get: () => null },
      }))
      render(<SimLab />)
      await act(async () => { fireEvent.click(screen.getByRole('button', { name: 'Export' })) })
      expect(screen.getByRole('alert')).toHaveTextContent('Export failed (403).')
      expect(clicked).toHaveLength(0)
    })
  })

  it('shows the empty state when no scenarios exist', () => {
    h.resp = { rows: [], total: 0, published: 0 }
    render(<SimLab />)
    expect(screen.getByText('No scenarios have been created yet.')).toBeInTheDocument()
  })
})
