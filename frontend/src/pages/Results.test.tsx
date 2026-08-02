import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen } from '@testing-library/react'
import { renderWithProviders } from '../test/utils'

// FE — the Results page mirrors the server's disclosure rules: a held attempt shows no score or
// pass/fail anywhere, an invalidated one routes to Appeals, and a released one shows the dial,
// the domain bands and the report download. Data comes straight from the `me` payload.
const h = vi.hoisted(() => ({ me: null as unknown }))

vi.mock('../data/MeContext', () => ({
  useMe: () => ({ me: h.me, loading: false, error: null, refetch: vi.fn() }),
}))
vi.mock('../api/client', () => ({
  api: { get: vi.fn(), post: vi.fn() },
}))

import Results from './Results'

function mkMe(attempts: unknown[]) {
  return {
    user: { first_name: 'Sam', last_name: 'Lee', email: 's@x.test', registration_no: 'PCI-1' },
    attempts,
    credentials: [],
    site_base_url: 'https://pci.example',
  }
}

const released = {
  id: 9,
  kind: 'exam',
  status: 'submitted',
  certification_code: 'PCL-AI',
  submitted_at: '2026-07-01 10:00:00',
  percent: 72,
  result: 'pass',
  result_status: 'credential_issued',
  violations: 0,
  domain_breakdown: JSON.stringify([
    { domain: 'Planning', pct: 80, band: 'above' },
    { domain: 'Cost', pct: 66, band: 'at' },
  ]),
}

describe('Results (full result view)', () => {
  beforeEach(() => { h.me = null })

  it('shows the empty state before any submitted attempt', () => {
    h.me = mkMe([])
    renderWithProviders(<Results />)
    expect(screen.getByText('No examination attempts yet')).toBeInTheDocument()
  })

  it('renders a released result with dial, verdict and domain bands', () => {
    h.me = mkMe([released])
    renderWithProviders(<Results />)
    expect(screen.getByText('PASS')).toBeInTheDocument()
    expect(screen.getByRole('img', { name: 'Score 72%' })).toBeInTheDocument()
    expect(screen.getByText('Planning')).toBeInTheDocument()
    expect(screen.getByText('Above target')).toBeInTheDocument()
    expect(screen.getByText('Download score report')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'View credential' })).toHaveAttribute('href', '/credentials')
  })

  it('a held result reveals no score, percent or pass/fail', () => {
    h.me = mkMe([{ ...released, percent: null, result: null, domain_breakdown: null, result_status: 'auto_held' }])
    renderWithProviders(<Results />)
    expect(screen.getByText('Result on hold')).toBeInTheDocument()
    expect(screen.queryByText('PASS')).not.toBeInTheDocument()
    expect(screen.queryByText(/72/)).not.toBeInTheDocument()
    expect(screen.queryByText('Download score report')).not.toBeInTheDocument()
  })

  it('an invalidated result routes to the Appeals process', () => {
    h.me = mkMe([{ ...released, result_status: 'invalidated' }])
    renderWithProviders(<Results />)
    expect(screen.getByText('Result invalidated')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Appeal this decision' })).toHaveAttribute('href', '/appeals')
  })
})
