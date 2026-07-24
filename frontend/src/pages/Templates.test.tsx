import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen, fireEvent } from '@testing-library/react'
import { renderWithProviders } from '../test/utils'

// FE — the members-only Templates page in the student portal. Pins: templates group by topic, a track badge +
// Download button render per item, the topic/track filter chips appear, and the empty state shows when nothing
// is published. useQuery is mocked at the module boundary (the page fetches /api/me/templates).
const h = vi.hoisted(() => ({ resp: { rows: [], total: 0, categories: [] } as unknown }))
vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: h.resp, loading: false, error: null, refetch: vi.fn() }),
}))

import Templates from './Templates'

const row = (over: Record<string, unknown> = {}) => ({
  slug: 'wbs-template', title: 'Work Breakdown Structure (WBS) template', category: 'scope', certification_id: 1,
  summary: 'Roll scope down to work packages.', format: 'csv', download_url: '/api/me/templates/wbs-template/file',
  ...over,
})

describe('Templates (student portal)', () => {
  beforeEach(() => { h.resp = { rows: [], total: 0, categories: [] } })

  it('shows the empty state when nothing is published', () => {
    renderWithProviders(<Templates />)
    expect(screen.getByText(/No templates are available yet/)).toBeInTheDocument()
  })

  it('renders templates grouped by topic with a track badge and a Download button', () => {
    h.resp = {
      rows: [row(), row({ slug: 'cashflow-forecast', title: 'Cash-flow forecast', category: 'cashflow', certification_id: 2 })],
      total: 2, categories: ['scope', 'cashflow'],
    }
    renderWithProviders(<Templates />)
    expect(screen.getByRole('heading', { name: 'Templates' })).toBeInTheDocument()
    expect(screen.getByText('Work Breakdown Structure (WBS) template')).toBeInTheDocument()
    expect(screen.getByText('Cash-flow forecast')).toBeInTheDocument()
    // PCL-AI appears both as a track chip and as the item's track badge.
    expect(screen.getAllByText('PCL-AI').length).toBeGreaterThanOrEqual(1)
    expect(screen.getAllByRole('button', { name: 'Download' }).length).toBe(2)
  })

  it('shows topic and track filter chips for the topics/tracks present', () => {
    h.resp = { rows: [row()], total: 1, categories: ['scope'] }
    renderWithProviders(<Templates />)
    expect(screen.getByText('Topic')).toBeInTheDocument()
    expect(screen.getByText('Track')).toBeInTheDocument()
    // The topic chip is a button (the group heading with the same label is not).
    expect(screen.getByRole('button', { name: 'Scope & WBS' })).toBeInTheDocument()
  })

  it('badges a template the student already downloaded and offers a New-only filter', () => {
    h.resp = {
      rows: [
        row({ downloaded: true }),
        row({ slug: 'cashflow-forecast', title: 'Cash-flow forecast', category: 'cashflow', certification_id: 2 }),
      ],
      total: 2, categories: ['scope', 'cashflow'],
    }
    renderWithProviders(<Templates />)
    // The already-downloaded item carries a "Downloaded" badge; its action reads "Download again".
    expect(screen.getByText('Downloaded')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Download again' })).toBeInTheDocument()
    // The Show/New filter appears once anything is downloaded; picking New hides the downloaded item.
    expect(screen.getByText('Show')).toBeInTheDocument()
    fireEvent.click(screen.getByRole('button', { name: 'New' }))
    expect(screen.queryByText('Work Breakdown Structure (WBS) template')).not.toBeInTheDocument()
    expect(screen.getByText('Cash-flow forecast')).toBeInTheDocument()
  })
})
