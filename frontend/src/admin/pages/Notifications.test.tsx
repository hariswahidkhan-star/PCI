import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'

// FE — operational alerts. The backend has carried recipients, per-event toggles, a delivery log
// and a test send since Endpoints/Notifications.cs shipped, with no console screen in front of it.
// What this pins: the RAW recipients box and the RESOLVED fan-out list are shown separately (they
// routinely differ, because the resolved list merges the fallback address, validates and dedupes);
// a missing mail provider is stated loudly rather than hidden; the save posts every toggle plus the
// recipients string; and a test send with no recipients surfaces the backend's own refusal.
const h = vi.hoisted(() => ({ data: null as unknown, error: null as string | null, post: vi.fn() }))

vi.mock('../hooks', () => ({
  useAdminQuery: () => ({ data: h.data, loading: false, error: h.error, refetch: vi.fn() }),
}))
vi.mock('../api', () => ({ adminApi: { post: (...a: unknown[]) => h.post(...a) } }))

import Notifications from './Notifications'

const mk = (over: Record<string, unknown> = {}) => ({
  recipients: 'ops@example.org',
  admin_email: 'owner@example.org',
  resolved: ['ops@example.org', 'owner@example.org'],
  events: [
    { key: 'chat', label: 'Live chat requests' },
    { key: 'finance', label: 'Manual payments, waivers & mismatches' },
  ],
  toggles: { chat: true, finance: false },
  recent: [],
  mail_configured: true,
  ...over,
})

describe('Admin notifications', () => {
  beforeEach(() => {
    h.data = mk()
    h.error = null
    h.post.mockReset().mockResolvedValue({ ok: true, resolved: ['ops@example.org'] })
  })

  it('shows the resolved fan-out list separately from the raw recipients box', () => {
    render(<Notifications />)
    expect(screen.getByLabelText('Alert addresses')).toHaveValue('ops@example.org')
    // the resolved list includes the fallback address, which is NOT in the raw box
    expect(screen.getByText('2 active')).toBeInTheDocument()
    expect(screen.getAllByText('owner@example.org').length).toBeGreaterThan(0)
  })

  it('says plainly when no address is configured at all', () => {
    h.data = mk({ recipients: '', admin_email: '', resolved: [] })
    render(<Notifications />)
    expect(screen.getByText('Nobody — no valid address is configured.')).toBeInTheDocument()
  })

  it('warns loudly when no mail provider is wired', () => {
    h.data = mk({ mail_configured: false })
    render(<Notifications />)
    expect(screen.getByText('No mail provider is configured.')).toBeInTheDocument()
  })

  it('does not warn when a provider is configured', () => {
    render(<Notifications />)
    expect(screen.queryByText('No mail provider is configured.')).toBeNull()
  })

  it('counts the enabled events', () => {
    render(<Notifications />)
    expect(screen.getByText('1 of 2 on')).toBeInTheDocument()
  })

  it('saves the recipients and every toggle together', async () => {
    const user = userEvent.setup()
    render(<Notifications />)
    // save is inert until something actually changes
    expect(screen.getByRole('button', { name: 'Saved' })).toBeDisabled()

    await user.click(screen.getByLabelText('Manual payments, waivers & mismatches'))
    await user.click(screen.getByRole('button', { name: 'Save changes' }))

    await waitFor(() => expect(h.post).toHaveBeenCalled())
    const [path, body] = h.post.mock.calls[0]
    expect(path).toBe('/api/admin/notifications')
    // every event key is sent, not just the changed one — the backend writes each toggle it sees
    expect(body).toEqual({ recipients: 'ops@example.org', chat: true, finance: true })
  })

  it('reports when a save leaves nobody configured', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValue({ ok: true, resolved: [] })
    render(<Notifications />)
    await user.clear(screen.getByLabelText('Alert addresses'))
    await user.click(screen.getByRole('button', { name: 'Save changes' }))
    expect(await screen.findByText(/alerts will reach nobody/)).toBeInTheDocument()
  })

  it('surfaces the backend refusal when a test send has no recipients', async () => {
    const user = userEvent.setup()
    h.post.mockResolvedValue({ ok: false, error: 'no_recipients', message: 'Add at least one email address first.' })
    render(<Notifications />)
    await user.click(screen.getByRole('button', { name: /Send test alert/ }))
    expect(await screen.findByText('Add at least one email address first.')).toBeInTheDocument()
  })

  it('explains an empty delivery log rather than showing a bare table', () => {
    render(<Notifications />)
    expect(screen.getByText('Nothing dispatched yet')).toBeInTheDocument()
  })

  it('renders the delivery log with its outcome', () => {
    h.data = mk({
      recent: [
        { channel: 'email', recipient: 'ops@example.org', subject: 'PCI test notification', status: 'sent', related_type: 'test', created_at: '2026-03-01 10:00:00' },
        { channel: 'email', recipient: 'bad@example.org', subject: 'PCI test notification', status: 'failed', related_type: 'test', created_at: '2026-03-01 10:00:00' },
      ],
    })
    const { container } = render(<Notifications />)
    const rows = container.querySelectorAll('tbody tr')
    expect(rows).toHaveLength(2)
    expect(within(rows[0] as HTMLElement).getByText('sent')).toBeInTheDocument()
    expect(within(rows[1] as HTMLElement).getByText('failed')).toBeInTheDocument()
  })

  it('surfaces a load failure', () => {
    h.error = 'Something went wrong.'
    h.data = null
    render(<Notifications />)
    expect(screen.getByText('Something went wrong.')).toBeInTheDocument()
  })
})
