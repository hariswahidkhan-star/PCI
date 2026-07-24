import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import { MemoryRouter, Routes, Route } from 'react-router-dom'

// FE — the Simulation Lab interactive runner (Phase 1). Decisions pinned: start renders the brief + the
// answer inputs; submitting shows the deterministic grade; and Assessment Mode withholds the correct
// values from the result table. The API client is mocked at the module boundary and routed by path.
const post = vi.fn()
vi.mock('../api/client', () => ({
  api: { post: (...a: unknown[]) => post(...a) },
  ApiError: class ApiError extends Error {},
}))

import LabRunner from './LabRunner'

const startResp = (assessment: boolean) => ({
  attempt_id: 5, resumed: false,
  scenario: { id: 1, scenario_code: 'GL-EVM-001', title: 'Calculate the core EVM measures', kind: 'guided_lab', difficulty: 'foundation' },
  task: {
    task: 'evm', prompt: 'Compute the earned-value measures.',
    given: { pv: 100000, ev: 90000, ac: 95000, bac: 200000 },
    ask: [{ key: 'spi', label: 'Schedule Performance Index (SPI)', type: 'number' }],
    mode: assessment ? 'assessment' : 'training', assessment,
  },
})

const renderRunner = () => render(
  <MemoryRouter initialEntries={['/lab/GL-EVM-001']}>
    <Routes><Route path="/lab/:code" element={<LabRunner />} /></Routes>
  </MemoryRouter>,
)

describe('LabRunner (Simulation Lab workspace)', () => {
  beforeEach(() => post.mockReset())

  it('starts an attempt, shows the brief + inputs, and renders the deterministic grade on submit', async () => {
    post.mockImplementation((path: string) => {
      if (path === '/api/me/lab/attempts') return Promise.resolve(startResp(false))
      if (String(path).endsWith('/submit')) return Promise.resolve({
        score: 100, passed: true, correct: 1, total: 1, mode: 'training', assessment: false,
        measures: [{ key: 'spi', label: 'Schedule Performance Index (SPI)', is_correct: true, correct_value: 0.9, your_value: 0.9 }],
        competencies: [{ competency: 'earned_value', score: 100, level: 'advanced' }],
      })
      return Promise.resolve({})
    })
    renderRunner()
    expect(await screen.findByText('Compute the earned-value measures.')).toBeInTheDocument()
    expect(screen.getByText('Planned Value (PV)')).toBeInTheDocument()   // given rendered

    fireEvent.change(screen.getByPlaceholderText('number'), { target: { value: '0.9' } })
    fireEvent.click(screen.getByRole('button', { name: 'Submit for grading' }))

    expect(await screen.findByText('Result — 100%')).toBeInTheDocument()
    expect(screen.getByText('Passed')).toBeInTheDocument()
    expect(screen.getAllByText('0.9').length).toBeGreaterThanOrEqual(2)  // your answer + correct, both revealed
    expect(screen.getByText(/Earned Value · Advanced/)).toBeInTheDocument()
  })

  it('fetches and shows a coaching message on request (Training Mode)', async () => {
    post.mockImplementation((path: string) => {
      if (path === '/api/me/lab/attempts') return Promise.resolve(startResp(false))
      if (String(path).endsWith('/submit')) return Promise.resolve({
        score: 50, passed: false, correct: 0, total: 1, mode: 'training', assessment: false,
        measures: [{ key: 'spi', label: 'Schedule Performance Index (SPI)', is_correct: false, correct_value: 0.9, your_value: 1.2 }],
        competencies: [],
      })
      if (String(path).endsWith('/coach')) return Promise.resolve({ ok: true, message: 'The SPI below 1 means behind schedule.', source: 'builtin', ai: false })
      return Promise.resolve({})
    })
    renderRunner()
    await screen.findByText('Compute the earned-value measures.')
    fireEvent.change(screen.getByPlaceholderText('number'), { target: { value: '1.2' } })
    fireEvent.click(screen.getByRole('button', { name: 'Submit for grading' }))
    await screen.findByText('Result — 50%')

    fireEvent.click(screen.getByRole('button', { name: 'Ask the coach' }))
    expect(await screen.findByText('The SPI below 1 means behind schedule.')).toBeInTheDocument()
    expect(screen.getByText('Guide')).toBeInTheDocument()   // deterministic (non-AI) badge
  })

  it('withholds the correct values in Assessment Mode', async () => {
    post.mockImplementation((path: string) => {
      if (path === '/api/me/lab/attempts') return Promise.resolve(startResp(true))
      if (String(path).endsWith('/submit')) return Promise.resolve({
        score: 0, passed: false, correct: 0, total: 1, mode: 'assessment', assessment: true,
        measures: [{ key: 'spi', label: 'Schedule Performance Index (SPI)', is_correct: false, correct_value: null, your_value: null }],
        competencies: [],
      })
      return Promise.resolve({})
    })
    renderRunner()
    await screen.findByText('Compute the earned-value measures.')
    fireEvent.change(screen.getByPlaceholderText('number'), { target: { value: '1.2' } })
    fireEvent.click(screen.getByRole('button', { name: 'Submit for grading' }))

    await screen.findByText('Result — 0%')
    // No "Correct" column value is shown, and the withholding note is present.
    expect(screen.getByText(/the worked answers are withheld/)).toBeInTheDocument()
    await waitFor(() => expect(post).toHaveBeenCalledTimes(2))
  })

  it('runs a linked multi-step scenario: a decision updates the downstream given and submit sends per-step answers + decisions', async () => {
    const multistepStart = {
      attempt_id: 9, resumed: false,
      scenario: { id: 2, scenario_code: 'MS-RECOVERY-001', title: 'Recover a programme', kind: 'scenario', difficulty: 'intermediate' },
      task: {
        multistep: true, prompt: 'Linked recovery scenario.', mode: 'training', assessment: false,
        steps: [
          {
            id: 's1', title: 'Baseline', task: 'evm', prompt: 'Compute CPI.',
            given: { pv: 100, ev: 90, ac: 95, bac: 200 }, ask: [{ key: 'cpi', label: 'CPI', type: 'number' }],
            decision: { key: 'recovery', prompt: 'Recover how?', options: [
              { value: 'crash', label: 'Crash (+50 AC)', effects: [{ step: 's2', path: 'ac', op: 'add', value: 50 }] },
              { value: 'descope', label: 'Descope', effects: [] },
            ] },
          },
          {
            id: 's2', title: 'Forecast', task: 'evm', prompt: 'Compute EAC.',
            given: { pv: 100, ev: 90, ac: 100, bac: 200 }, ask: [{ key: 'eac', label: 'EAC', type: 'number' }],
          },
        ],
      },
    }
    post.mockImplementation((path: string) => {
      if (path === '/api/me/lab/attempts') return Promise.resolve(multistepStart)
      if (String(path).endsWith('/submit')) return Promise.resolve({
        score: 100, passed: true, correct: 2, total: 2, mode: 'training', assessment: false,
        measures: [
          { key: 's1.cpi', label: 'Baseline: CPI', is_correct: true, correct_value: 0.95, your_value: 0.95 },
          { key: 's2.eac', label: 'Forecast: EAC', is_correct: true, correct_value: 296, your_value: 296 },
        ],
        competencies: [],
      })
      return Promise.resolve({})
    })
    renderRunner()
    await screen.findByText('Linked recovery scenario.')
    expect(screen.getByText('Baseline')).toBeInTheDocument()
    expect(screen.getByText('Forecast')).toBeInTheDocument()
    expect(screen.getByText('Recover how?')).toBeInTheDocument()

    // Downstream step s2 starts at AC 100; choosing "crash" applies +50 → 150 (client mirror of the engine).
    expect(screen.queryByText('150')).toBeNull()
    fireEvent.click(screen.getByRole('radio', { name: 'Crash (+50 AC)' }))
    expect(await screen.findByText('150')).toBeInTheDocument()

    const nums = screen.getAllByPlaceholderText('number')
    fireEvent.change(nums[0], { target: { value: '0.95' } })   // s1.cpi
    fireEvent.change(nums[1], { target: { value: '296' } })    // s2.eac
    fireEvent.click(screen.getByRole('button', { name: 'Submit for grading' }))

    await screen.findByText('Result — 100%')
    const submitCall = post.mock.calls.find((c) => String(c[0]).endsWith('/submit'))!
    const body = submitCall[1] as { answers: { steps: Record<string, Record<string, unknown>>; decisions: Record<string, unknown> } }
    expect(body.answers.decisions).toEqual({ recovery: 'crash' })
    expect(body.answers.steps.s1.cpi).toBe(0.95)
    expect(body.answers.steps.s2.eac).toBe(296)
  })

  it('hydrates saved answers when an in-progress attempt is resumed', async () => {
    post.mockImplementation((path: string) => {
      if (path === '/api/me/lab/attempts') {
        return Promise.resolve({
          ...startResp(false),
          resumed: true,
          answers: { spi: 0.9 },
        })
      }
      return Promise.resolve({})
    })
    renderRunner()
    expect(await screen.findByText('Resumed your saved answers')).toBeInTheDocument()
    expect(screen.getByPlaceholderText('number')).toHaveValue('0.9')
  })
})
