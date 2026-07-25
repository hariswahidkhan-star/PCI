import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, waitFor, within, fireEvent } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE-12 — the Credentials console is credential-integrity: revoke / reinstate move a holder's live
// standing (and the public verify + download gates key off it). This pins the revoke confirm-guard,
// the reinstate transition, the lapsed-vs-active display, the status filter, and the PDF-only upload
// guard. useAdminQuery + adminApi are mocked; window.confirm/alert are stubbed.
const h = vi.hoisted(() => ({
  creds: { rows: [] as unknown[] },
  paths: [] as (string | null)[],
  post: vi.fn(),
}))

vi.mock('../hooks', () => ({
  useAdminQuery: (path: string | null) => {
    h.paths.push(path)
    let data: unknown = null
    if (path === '/api/admin/credly/status') data = { configured: false }
    else if (path && path.startsWith('/api/admin/credentials')) data = h.creds
    return { data, loading: false, error: null, refetch: vi.fn() }
  },
}))
vi.mock('../api', () => ({
  adminApi: { post: (path: string, body?: unknown) => h.post(path, body) },
}))

import Credentials from './Credentials'

const active = (over: Record<string, unknown> = {}) => ({
  id: 1, credential_id: 'PCL-2025-0001', holder_name: 'Sam Rivera', credential: 'PCL-AI',
  issued_at: '2025-01-01T00:00:00Z', expires_at: '2028-01-01T00:00:00Z', status: 'active', ...over,
})

describe('Credentials admin console', () => {
  beforeEach(() => {
    h.creds = { rows: [] }
    h.paths = []
    h.post.mockReset()
    vi.spyOn(window, 'confirm').mockReturnValue(true)
    vi.spyOn(window, 'alert').mockImplementation(() => {})
  })
  afterEach(() => vi.restoreAllMocks())

  describe('status transitions', () => {
    it('does not revoke when the confirm dialog is dismissed', async () => {
      const user = userEvent.setup()
      h.creds = { rows: [active()] }
      ;(window.confirm as ReturnType<typeof vi.fn>).mockReturnValue(false)
      render(<Credentials />)
      await user.click(screen.getByRole('button', { name: 'Revoke' }))
      expect(window.confirm).toHaveBeenCalled()
      expect(h.post).not.toHaveBeenCalled()
    })

    it('revokes after confirmation', async () => {
      const user = userEvent.setup()
      h.creds = { rows: [active()] }
      h.post.mockResolvedValueOnce(undefined)
      render(<Credentials />)
      await user.click(screen.getByRole('button', { name: 'Revoke' }))
      await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/admin/credentials/1/status', { status: 'revoked' }))
    })

    it('reinstates a revoked credential (no confirm needed)', async () => {
      const user = userEvent.setup()
      h.creds = { rows: [active({ status: 'revoked' })] }
      h.post.mockResolvedValueOnce(undefined)
      render(<Credentials />)
      expect(screen.queryByRole('button', { name: 'Revoke' })).not.toBeInTheDocument()
      await user.click(screen.getByRole('button', { name: 'Reinstate' }))
      await waitFor(() => expect(h.post).toHaveBeenCalledWith('/api/admin/credentials/1/status', { status: 'active' }))
    })

    it('offers neither action for an already-expired credential', () => {
      h.creds = { rows: [active({ status: 'expired' })] }
      render(<Credentials />)
      expect(screen.queryByRole('button', { name: 'Revoke' })).not.toBeInTheDocument()
      expect(screen.queryByRole('button', { name: 'Reinstate' })).not.toBeInTheDocument()
    })
  })

  it('shows an expired badge for an active credential past its expiry (still revocable)', () => {
    h.creds = { rows: [active({ expires_at: '2020-01-01T00:00:00Z' })] }
    render(<Credentials />)
    const row = screen.getByText('PCL-2025-0001').closest('tr') as HTMLElement
    expect(within(row).getByText('expired')).toBeInTheDocument()
    // The row's status is still 'active', so the revoke control remains available.
    expect(within(row).getByRole('button', { name: 'Revoke' })).toBeInTheDocument()
  })

  it('folds a status filter into the credentials query path', async () => {
    const user = userEvent.setup()
    render(<Credentials />)
    const credPaths = () => h.paths.filter((p) => p && p.startsWith('/api/admin/credentials'))
    expect(credPaths()[0]).toBe('/api/admin/credentials')
    const statusSel = screen.getAllByRole('combobox').find((s) => within(s).queryByRole('option', { name: 'Revoked' }))!
    await user.selectOptions(statusSel, 'revoked')
    await waitFor(() => expect(credPaths()[credPaths().length - 1]).toContain('status=revoked'))
  })

  it('rejects a non-PDF certificate upload without calling the API', () => {
    h.creds = { rows: [active()] }
    const { container } = render(<Credentials />)
    const fileInput = container.querySelector('input[type="file"]') as HTMLInputElement
    // fireEvent bypasses the input's accept filter, mimicking a browser that lets a wrong file through.
    fireEvent.change(fileInput, { target: { files: [new File(['x'], 'notes.txt', { type: 'text/plain' })] } })
    expect(window.alert).toHaveBeenCalledWith('Please choose a PDF file.')
    expect(h.post).not.toHaveBeenCalled()
  })

  it('shows the empty state when nothing matches', () => {
    render(<Credentials />)
    expect(screen.getByText('No credentials match.')).toBeInTheDocument()
  })
})
