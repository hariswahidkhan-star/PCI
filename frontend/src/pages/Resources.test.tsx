import { describe, it, expect, vi, beforeEach } from 'vitest'
import { screen } from '@testing-library/react'
import { renderWithProviders } from '../test/utils'

// FE — the in-portal downloads library. Decisions worth pinning: resources group by category, a row
// with a URL offers a Download link while one without shows "Coming soon", and the empty state shows
// when nothing is published. useQuery is mocked at the module boundary.
const h = vi.hoisted(() => ({ rows: [] as unknown[] }))

vi.mock('../api/hooks', () => ({
  useQuery: () => ({ data: { rows: h.rows }, loading: false, error: null, refetch: vi.fn() }),
}))

import Resources from './Resources'

describe('Resources (downloads library)', () => {
  beforeEach(() => { h.rows = [] })

  it('shows the empty state when nothing is published', () => {
    renderWithProviders(<Resources />)
    expect(screen.getByText(/No resources have been published yet/)).toBeInTheDocument()
  })

  it('offers a Download link for a resource with a URL', () => {
    h.rows = [{ title: 'Candidate handbook', category: 'Policies', doc_type: 'PDF', url: 'https://cdn.test/handbook.pdf' }]
    renderWithProviders(<Resources />)
    expect(screen.getByText('Policies')).toBeInTheDocument() // category group card
    expect(screen.getByRole('link', { name: 'Download' })).toHaveAttribute('href', 'https://cdn.test/handbook.pdf')
  })

  it('shows "Coming soon" for a resource without a URL', () => {
    h.rows = [{ title: 'Study guide', category: 'Study', doc_type: 'PDF', url: null }]
    renderWithProviders(<Resources />)
    expect(screen.getByText('Coming soon')).toBeInTheDocument()
    expect(screen.queryByRole('link', { name: 'Download' })).toBeNull()
  })

  it('groups resources under their category (defaulting to General)', () => {
    h.rows = [
      { title: 'A', category: 'Policies', url: 'https://cdn.test/a' },
      { title: 'B', category: null, url: 'https://cdn.test/b' },
    ]
    renderWithProviders(<Resources />)
    expect(screen.getByText('Policies')).toBeInTheDocument()
    expect(screen.getByText('General')).toBeInTheDocument() // null category → General
  })
})
