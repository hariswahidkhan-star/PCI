import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { screen, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { renderWithProviders } from '../test/utils'

// FE — the student Credentials page. The decisions worth pinning: a lapsed credential (active in the
// DB but past its expiry) is badged "expired" and loses its action row; a near-expiry credential
// shows the days-left warning; honorary recognition renders as its own board-conferred section; the
// empty state shows when nothing is issued; and the PDF download hits the honorary endpoint for an
// award vs the certificate endpoint for a credential. useMe is mocked at the module boundary; fetch +
// alert are stubbed for the download tests (the fetch resolves !ok so the code alerts and returns
// before touching jsdom's blob/anchor APIs).
const h = vi.hoisted(() => ({ me: null as unknown }))

vi.mock('../data/MeContext', () => ({ useMe: () => ({ me: h.me, loading: false, error: null }) }))

import Credentials from './Credentials'

const inDays = (n: number) => new Date(Date.now() + n * 86_400_000).toISOString()

const cred = (over: Record<string, unknown> = {}) => ({
  credential_id: 'PCL-1042',
  credential: 'PCL-AI',
  certification_name: 'PCL-AI',
  certification_acronym: 'PCL-AI',
  status: 'active',
  issued_at: '2026-01-01T00:00:00Z',
  expires_at: inDays(365),
  holder_name: null,
  certificate_wording: null,
  ...over,
})
const mkMe = (over: Record<string, unknown> = {}) => ({
  user: { first_name: 'Sam', last_name: 'Rivera' },
  credentials: [],
  honorary: [],
  site_base_url: 'https://pci.test',
  ...over,
})

describe('Credentials (student)', () => {
  beforeEach(() => { h.me = mkMe() })
  afterEach(() => vi.unstubAllGlobals())

  it('shows the empty state when nothing has been issued', () => {
    renderWithProviders(<Credentials />)
    expect(screen.getByText(/You have no issued credentials yet/)).toBeInTheDocument()
  })

  it('offers the full action row for an active, non-expired credential', () => {
    h.me = mkMe({ credentials: [cred()] })
    renderWithProviders(<Credentials />)
    expect(screen.getByRole('button', { name: 'Download PDF' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Download certificate' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Add to LinkedIn' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Public verify page' })).toBeInTheDocument()
    expect(screen.queryByText(/d left/)).toBeNull() // >90 days out → no warning
  })

  it('badges a lapsed credential "expired" and hides its action row', () => {
    h.me = mkMe({ credentials: [cred({ status: 'active', expires_at: inDays(-1) })] })
    renderWithProviders(<Credentials />)
    expect(screen.getByText('expired')).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: 'Download PDF' })).toBeNull()
    expect(screen.queryByRole('button', { name: 'Download certificate' })).toBeNull()
  })

  it('shows the days-left warning for a credential expiring within 90 days', () => {
    h.me = mkMe({ credentials: [cred({ expires_at: inDays(20) })] })
    renderWithProviders(<Credentials />)
    expect(screen.getByText(/d left/)).toBeInTheDocument()
  })

  it('renders honorary recognition as its own board-conferred section', () => {
    h.me = mkMe({
      honorary: [{ award_no: 'PCI-HON-7', designation: 'Honorary Fellow', citation: 'For service', conferred_at: '2026-01-01T00:00:00Z' }],
    })
    renderWithProviders(<Credentials />)
    expect(screen.getByText('Honorary recognition')).toBeInTheDocument()
    expect(screen.getByText('Board-conferred')).toBeInTheDocument()
    expect(screen.getByText('Honorary Fellow')).toBeInTheDocument()
    expect(screen.getByText('PCI-HON-7')).toBeInTheDocument()
  })

  it('downloads a credential PDF from the certificate endpoint', async () => {
    const user = userEvent.setup()
    const fetchMock = vi.fn().mockResolvedValue({ ok: false })
    vi.stubGlobal('fetch', fetchMock)
    vi.stubGlobal('alert', vi.fn())
    h.me = mkMe({ credentials: [cred()] })
    renderWithProviders(<Credentials />)
    await user.click(screen.getByRole('button', { name: 'Download PDF' }))
    expect(fetchMock).toHaveBeenCalledWith('/api/me/certificate/pdf?id=PCL-1042', expect.anything())
  })

  it('downloads an honorary award from the honorary certificate endpoint', async () => {
    const user = userEvent.setup()
    const fetchMock = vi.fn().mockResolvedValue({ ok: false })
    vi.stubGlobal('fetch', fetchMock)
    vi.stubGlobal('alert', vi.fn())
    h.me = mkMe({ honorary: [{ award_no: 'PCI-HON-7', designation: 'Honorary Fellow', conferred_at: '2026-01-01T00:00:00Z' }] })
    renderWithProviders(<Credentials />)
    const section = screen.getByText('Honorary recognition').closest('section') as HTMLElement
    await user.click(within(section).getByRole('button', { name: 'Download PDF' }))
    expect(fetchMock).toHaveBeenCalledWith('/api/me/honorary-certificate/pdf', expect.anything())
  })
})
