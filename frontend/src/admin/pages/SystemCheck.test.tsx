import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, within } from '@testing-library/react'

// FE — the owner readiness screen over GET /api/admin/system-check, which had no UI at all.
// This is the class of failure that goes unnoticed: a bad production config makes the app refuse
// to boot (exit 78) while the PREVIOUS deploy keeps serving, so the platform looks healthy from
// outside while every new release is silently rejected. What this pins: errors and warnings are
// separated (only errors block a boot); a check whose healthy state is FALSE is not painted red
// for being false; an informational posture is neither pass nor fail; and a 403 for a non-owner
// is surfaced rather than rendering an empty screen.
const h = vi.hoisted(() => ({ data: null as unknown, loading: false, error: null as string | null }))

vi.mock('../hooks', () => ({
  useAdminQuery: () => ({ data: h.data, loading: h.loading, error: h.error, refetch: vi.fn() }),
}))

import SystemCheck from './SystemCheck'

const mk = (over: Record<string, unknown> = {}) => ({
  ok: true,
  environment: 'Production',
  checks: {
    base_url_set: true,
    cors_locked: true,
    persistent_db: true,
    migrations_applied: true,
    storage_local: true,
    storage_s3_misconfigured: false,
    stripe_webhook_path: '/api/webhook',
  },
  issues: [] as { sev: string; key: string; msg: string }[],
  ...over,
})

describe('Readiness (system check)', () => {
  beforeEach(() => {
    h.data = mk()
    h.loading = false
    h.error = null
  })

  it('reports a ready deployment', () => {
    render(<SystemCheck />)
    const tile = screen.getByText('Boot status').closest('.kpi') as HTMLElement
    expect(within(tile).getByText('Ready')).toBeInTheDocument()
    expect(tile).toHaveClass('tone-ok')
    expect(screen.getByText('No configuration issues')).toBeInTheDocument()
  })

  it('reports a blocked deployment and explains the silent-failure mode', () => {
    h.data = mk({
      ok: false,
      issues: [{ sev: 'error', key: 'MYSQL_HOST', msg: 'MySQL is selected but connection settings are incomplete.' }],
    })
    render(<SystemCheck />)
    const tile = screen.getByText('Boot status').closest('.kpi') as HTMLElement
    expect(within(tile).getByText('Blocked')).toBeInTheDocument()
    expect(tile).toHaveClass('tone-err')
    expect(screen.getByText('MYSQL_HOST')).toBeInTheDocument()
    expect(screen.getByText(/previously deployed release keeps serving/)).toBeInTheDocument()
  })

  it('separates warnings from blocking errors', () => {
    h.data = mk({
      ok: true,
      issues: [
        { sev: 'warn', key: 'SMTP_HOST', msg: 'No mail provider configured.' },
        { sev: 'error', key: 'APP_BASE_URL', msg: 'Must be a public https URL.' },
      ],
    })
    render(<SystemCheck />)
    // only the error counts toward the blocking tally
    const errTile = screen.getByText('Blocking errors').closest('.kpi') as HTMLElement
    expect(within(errTile).getByText('1')).toBeInTheDocument()
    expect(screen.getByText('Warnings')).toBeInTheDocument()
    expect(screen.getByText('What is blocking a production boot')).toBeInTheDocument()
  })

  it('does not paint an inverted check red merely for being false', () => {
    // storage_s3_misconfigured: false is the HEALTHY state — the count must treat it as passing
    render(<SystemCheck />)
    const tile = screen.getByText('Checks passing').closest('.kpi') as HTMLElement
    expect(within(tile).getByText('6/6')).toBeInTheDocument()
  })

  it('counts an inverted check as failing when it is true', () => {
    h.data = mk({ checks: { ...mk().checks, storage_s3_misconfigured: true } })
    render(<SystemCheck />)
    const tile = screen.getByText('Checks passing').closest('.kpi') as HTMLElement
    expect(within(tile).getByText('5/6')).toBeInTheDocument()
  })

  it('labels the raw check keys in human terms', () => {
    render(<SystemCheck />)
    expect(screen.getByText('CORS locked to an explicit origin')).toBeInTheDocument()
    expect(screen.getByText('Database on durable storage')).toBeInTheDocument()
  })

  it('shows the environment it is reporting on', () => {
    h.data = mk({ environment: 'Staging' })
    render(<SystemCheck />)
    expect(screen.getByText('Staging')).toBeInTheDocument()
  })

  it('surfaces the 403 a non-owner gets rather than an empty screen', () => {
    h.data = null
    h.error = 'forbidden'
    render(<SystemCheck />)
    expect(screen.getByText('forbidden')).toBeInTheDocument()
  })

  it('shows a shaped loading state', () => {
    h.loading = true
    h.data = null
    const { container } = render(<SystemCheck />)
    expect(container.querySelectorAll('.skeleton').length).toBeGreaterThan(0)
  })
})
