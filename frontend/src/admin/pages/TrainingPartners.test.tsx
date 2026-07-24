import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'

// FE — Admin Console → Training Partners: the test-institution mechanism (mirrors the student
// test-user pattern). Pins: the scenario picker + create call, the credentials drawer with the
// /partner.html#t= hand-off link, the Test badge on directory rows, and the test-aware delete
// that routes through /api/admin/test-partners so all partner data is cleaned up.
const h = vi.hoisted(() => ({ partners: { rows: [] as unknown[] } as unknown }))
vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) =>
    ({ data: path === '/api/admin/training-partners' ? h.partners : null, loading: false, error: null, refetch: vi.fn() }),
}))
const api = vi.hoisted(() => ({ post: vi.fn(), get: vi.fn(), patch: vi.fn(), del: vi.fn() }))
vi.mock('../api', () => ({ adminApi: api }))

import TrainingPartners from './TrainingPartners'

const partner = (over: Record<string, unknown> = {}) => ({
  id: 1, name: 'Institute of Works', tier: 'registered', listed: 1, is_test: 0,
  country: 'UK', city: 'London', ...over,
})

describe('Admin Training Partners — test institutions', () => {
  beforeEach(() => { h.partners = { rows: [] }; api.post.mockReset(); api.patch.mockReset() })

  it('creates a scenario-based test partner and shows the dashboard hand-off', async () => {
    api.post.mockResolvedValue({
      ok: true, partner_id: 9, institution: 'Test Institution AB12',
      email: 'test.partner.ab12@pci.test', password: 'TestPass!ab12', token: 'tok123', scenario: 'active',
    })
    render(<TrainingPartners />)
    fireEvent.change(screen.getByLabelText('Test-partner scenario'), { target: { value: 'fresh_login' } })
    fireEvent.click(screen.getByRole('button', { name: '+ Test partner' }))
    expect(api.post).toHaveBeenCalledWith('/api/admin/test-partners', { scenario: 'fresh_login' })
    expect(await screen.findByText('Test institution created')).toBeInTheDocument()
    expect(screen.getByText('test.partner.ab12@pci.test')).toBeInTheDocument()
    expect(screen.getByText('TestPass!ab12')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: /Open partner dashboard as this institution/ }))
      .toHaveAttribute('href', '/partner.html#t=tok123')
  })

  it('explains when a scenario mints no session (suspended lockout)', async () => {
    api.post.mockResolvedValue({
      ok: true, partner_id: 9, institution: 'Test Institution CD34',
      email: 'test.partner.cd34@pci.test', password: 'TestPass!cd34', token: null, scenario: 'suspended',
    })
    render(<TrainingPartners />)
    fireEvent.click(screen.getByRole('button', { name: '+ Test partner' }))
    expect(await screen.findByText(/No session for this scenario/)).toBeInTheDocument()
    expect(screen.queryByRole('link', { name: /Open partner dashboard/ })).toBeNull()
  })

  it('flags test institutions in the directory and deletes them through the test-partner endpoint', async () => {
    h.partners = { rows: [partner(), partner({ id: 2, name: 'Test Institution AB12', is_test: 1, listed: 0 })] }
    api.post.mockResolvedValue({ ok: true })
    const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true)
    render(<TrainingPartners />)
    expect(screen.getByText('Test')).toBeInTheDocument()          // badge only on the test row
    const deleteButtons = screen.getAllByRole('button', { name: 'Delete' })
    fireEvent.click(deleteButtons[1])                              // the test institution's row
    expect(confirmSpy).toHaveBeenCalledWith(expect.stringContaining('test institution'))
    expect(api.post).toHaveBeenCalledWith('/api/admin/test-partners/2/delete')
    confirmSpy.mockRestore()
  })
})
