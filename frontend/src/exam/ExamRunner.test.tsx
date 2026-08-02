import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'
import { I18nProvider } from '../i18n'

// FE — the in-app secure runner. What matters: answers persist locally and ride heartbeats, the
// submit confirmation counts unanswered items, a held submission shows NO score or pass/fail, and
// a released one shows the verdict with domain bands. The server clock stays authoritative — the
// deadline is re-synced from heartbeat responses.
const h = vi.hoisted(() => ({ post: vi.fn() }))

vi.mock('../api/client', () => ({
  api: { post: (p: string, b?: unknown) => h.post(p, b) },
  ApiError: class ApiError extends Error {
    status: number
    body: unknown
    constructor(status: number, msg: string, body?: unknown) { super(msg); this.status = status; this.body = body }
  },
}))

import ExamRunner, { type StartPayload } from './ExamRunner'

function mkStart(over: Partial<StartPayload> = {}): StartPayload {
  return {
    attempt_id: 77,
    duration_minutes: 90,
    started_at: new Date().toISOString(),
    items: [
      { id: 1, question: 'What is EVM?', options: ['Earned value', 'Extra vacation', 'Neither'], domain: 'Planning' },
      { id: 2, question: 'CPI below 1 means?', options: ['Over budget', 'Under budget'], domain: 'Cost' },
    ],
    ...over,
  }
}

const renderRunner = (start = mkStart(), onExit = vi.fn()) => {
  render(
    <I18nProvider>
      <MemoryRouter>
        <ExamRunner start={start} certLabel="PCL-AI" onExit={onExit} />
      </MemoryRouter>
    </I18nProvider>,
  )
  return onExit
}

describe('ExamRunner (in-app browser sitting)', () => {
  beforeEach(() => {
    h.post.mockReset()
    h.post.mockResolvedValue({ ok: true })
    localStorage.clear()
  })

  it('renders the sitting with palette, secure chip and timer', () => {
    renderRunner()
    expect(screen.getByText('PCL-AI — Examination')).toBeInTheDocument()
    expect(screen.getByText('Secure mode')).toBeInTheDocument()
    expect(screen.getByText('Question 1 of 2')).toBeInTheDocument()
    expect(screen.getByText('What is EVM?')).toBeInTheDocument()
    expect(screen.getByText('0 of 2 answered')).toBeInTheDocument()
  })

  it('answering selects the option, persists locally, and advances via the palette', async () => {
    const user = userEvent.setup()
    renderRunner()
    await user.click(screen.getByText('Earned value'))
    expect(screen.getByText('1 of 2 answered')).toBeInTheDocument()
    expect(JSON.parse(localStorage.getItem('pci.exam.answers.77') || '{}')).toEqual({ '1': 0 })
    await user.click(screen.getByRole('button', { name: '2' }))
    expect(screen.getByText('CPI below 1 means?')).toBeInTheDocument()
  })

  it('resumes from saved server answers', () => {
    renderRunner(mkStart({ saved_answers: { '1': 0, '2': 1 }, resumed: true }))
    expect(screen.getByText('2 of 2 answered')).toBeInTheDocument()
  })

  it('the submit confirmation counts unanswered items', async () => {
    const user = userEvent.setup()
    renderRunner()
    await user.click(screen.getByText('Earned value'))
    await user.click(screen.getAllByRole('button', { name: 'Submit examination' })[0])
    expect(screen.getByText(/1 question is unanswered and will be marked incorrect/)).toBeInTheDocument()
  })

  it('a held submission shows the hold notice and never a score or pass/fail', async () => {
    const user = userEvent.setup()
    h.post.mockImplementation((p: string) =>
      p === '/api/me/exam/submit'
        ? Promise.resolve({ ok: true, held: true, result_status: 'auto_held', message: 'Integrity check pending.' })
        : Promise.resolve({ ok: true }))
    renderRunner()
    await user.click(screen.getAllByRole('button', { name: 'Submit examination' })[0])
    await user.click(screen.getByRole('button', { name: 'Submit now' }))
    expect(await screen.findByText('Result on hold — under review')).toBeInTheDocument()
    expect(screen.getByText('Integrity check pending.')).toBeInTheDocument()
    expect(screen.queryByText(/PASS/)).not.toBeInTheDocument()
    expect(screen.queryByText(/%/)).not.toBeInTheDocument()
  })

  it('a released pass shows verdict, score line, credential and domain bands', async () => {
    const user = userEvent.setup()
    h.post.mockImplementation((p: string) =>
      p === '/api/me/exam/submit'
        ? Promise.resolve({
            ok: true, held: false, result: 'pass', pct: 76.5, score: 13, max: 17,
            credential: 'PCL-AI-2026-12345',
            breakdown: [{ domain: 'Planning', pct: 80, band: 'above' }],
          })
        : Promise.resolve({ ok: true }))
    renderRunner()
    await user.click(screen.getAllByRole('button', { name: 'Submit examination' })[0])
    await user.click(screen.getByRole('button', { name: 'Submit now' }))
    expect(await screen.findByText('PASS')).toBeInTheDocument()
    expect(screen.getByText('13 of 17 scored items correct')).toBeInTheDocument()
    expect(screen.getByText('PCL-AI-2026-12345')).toBeInTheDocument()
    expect(screen.getByText('Planning')).toBeInTheDocument()
    // clears the local autosave for this attempt
    expect(localStorage.getItem('pci.exam.answers.77')).toBeNull()
  })

  it('answers and violations ride the heartbeat', async () => {
    renderRunner()
    await waitFor(() => {
      expect(h.post).toHaveBeenCalledWith('/api/me/exam/heartbeat', expect.objectContaining({ attempt_id: 77 }))
    })
  })

  it('when the server already finalised the attempt, submit routes to the result instead of erroring', async () => {
    const user = userEvent.setup()
    const { ApiError } = await import('../api/client')
    h.post.mockImplementation((p: string) =>
      p === '/api/me/exam/submit'
        ? Promise.reject(new (ApiError as unknown as new (s: number, m: string, b?: unknown) => Error)(400, 'already_submitted', { error: 'already_submitted' }))
        : Promise.resolve({ ok: true }))
    const onExit = renderRunner()
    await user.click(screen.getAllByRole('button', { name: 'Submit examination' })[0])
    await user.click(screen.getByRole('button', { name: 'Submit now' }))
    expect(await screen.findByText('Your sitting has been finalised')).toBeInTheDocument()
    await user.click(screen.getByRole('button', { name: 'View my result' }))
    expect(onExit).toHaveBeenCalledWith(true)
  })
})
