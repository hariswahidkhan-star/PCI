import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import EventStaffApp from './EventStaffApp'

// FE — gate-staff scanner root (§7A.8), run against the REAL seam. Pinned: fixtures mode drives
// the full flow (event+gate selection -> manual entry -> decision -> explicit confirm -> rapid
// reset clearing prior-attendee PII, duplicate-scan debounce, jsdom's missing camera API falls
// back to manual entry); and without fixtures a 404 from the not-yet-built backend renders the
// designed "not yet enabled" state via a stubbed global fetch — never a broken screen.

function setUrl(path: string) {
  window.history.replaceState({}, '', path)
}

async function startScanning(user: ReturnType<typeof userEvent.setup>) {
  render(<EventStaffApp />)
  await screen.findByLabelText('Event')
  await user.selectOptions(screen.getByLabelText('Event'), '501')
  await user.selectOptions(screen.getByLabelText('Gate'), 'gate_a')
  await user.click(screen.getByRole('button', { name: 'Start scanning' }))
  await screen.findByLabelText('Pass code')
}

describe('EventStaffApp (gate scanner)', () => {
  beforeEach(() => {
    setUrl('/?preview=1')
    sessionStorage.clear()
  })
  afterEach(() => {
    setUrl('/')
    vi.unstubAllGlobals()
  })

  it('selects event + gate, checks a pass manually, confirms, and resets clearing PII', async () => {
    const user = userEvent.setup()
    await startScanning(user)

    // jsdom has no BarcodeDetector: camera scanning honestly reports unsupported and manual
    // entry stands as the primary path.
    expect(screen.getByTestId('camera-unsupported')).toBeInTheDocument()

    await user.type(screen.getByLabelText('Pass code'), 'EVT-OK-501-9f3ab2c8d1')
    await user.click(screen.getByRole('button', { name: 'Check pass' }))

    const panel = await screen.findByTestId('result-admit')
    expect(panel).toHaveTextContent('ADMIT')
    expect(screen.getByText('Jordan Meyer')).toBeInTheDocument()

    // Explicit confirm step records the admission.
    await user.click(screen.getByRole('button', { name: 'Confirm admission' }))
    expect(await screen.findByText('Admission recorded.')).toBeInTheDocument()

    // Rapid reset clears every trace of the prior attendee.
    await user.click(screen.getByTestId('next-attendee'))
    expect(screen.queryByText('Jordan Meyer')).toBeNull()
    expect(screen.queryByTestId('result-admit')).toBeNull()
    expect(screen.getByLabelText('Pass code')).toHaveValue('')
  })

  it('debounces a duplicate scan of the same token', async () => {
    const user = userEvent.setup()
    await startScanning(user)

    await user.type(screen.getByLabelText('Pass code'), 'EVT-OK-XYZ')
    await user.click(screen.getByRole('button', { name: 'Check pass' }))
    await screen.findByTestId('result-admit')
    await user.click(screen.getByTestId('next-attendee'))

    // Same token again, well inside the debounce window: no new decision, a visible note instead.
    await user.type(screen.getByLabelText('Pass code'), 'EVT-OK-XYZ')
    await user.click(screen.getByRole('button', { name: 'Check pass' }))
    expect(await screen.findByTestId('duplicate-note')).toBeInTheDocument()
    expect(screen.queryByTestId('result-admit')).toBeNull()
  })

  it('renders the other server decisions (already checked in / manual review / do not admit)', async () => {
    const user = userEvent.setup()
    await startScanning(user)

    await user.type(screen.getByLabelText('Pass code'), 'EVT-USED-502')
    await user.click(screen.getByRole('button', { name: 'Check pass' }))
    const used = await screen.findByTestId('result-already_checked_in')
    expect(used).toHaveTextContent('ALREADY CHECKED IN')
    expect(used).toHaveTextContent('First entry: 2026-05-02 08:12:00')
    // No "Confirm admission" on a non-admit outcome.
    expect(screen.queryByRole('button', { name: 'Confirm admission' })).toBeNull()
    await user.click(screen.getByTestId('next-attendee'))

    await user.type(screen.getByLabelText('Pass code'), 'EVT-REVIEW-1')
    await user.click(screen.getByRole('button', { name: 'Check pass' }))
    expect(await screen.findByTestId('result-manual_review')).toHaveTextContent('MANUAL REVIEW')
    await user.click(screen.getByTestId('next-attendee'))

    await user.type(screen.getByLabelText('Pass code'), 'EVT-NOPE')
    await user.click(screen.getByRole('button', { name: 'Check pass' }))
    expect(await screen.findByTestId('result-do_not_admit')).toHaveTextContent('DO NOT ADMIT')
  })

  it('manual lookup is labelled as an identity lookup, not authentication', async () => {
    const user = userEvent.setup()
    await startScanning(user)
    expect(screen.getByText(/identity lookup/)).toBeInTheDocument()
    expect(screen.getByText(/does not verify who is standing in front of you/)).toBeInTheDocument()

    await user.type(
      screen.getByLabelText('Registration reference or Student Number'),
      'PCI-EV-2026-000214',
    )
    await user.click(screen.getByRole('button', { name: 'Look up' }))
    expect(await screen.findByTestId('result-already_checked_in')).toBeInTheDocument()
  })

  it('shows the offline banner and blocks pass checks while offline', async () => {
    const user = userEvent.setup()
    const desc = Object.getOwnPropertyDescriptor(Navigator.prototype, 'onLine')
    Object.defineProperty(window.navigator, 'onLine', { value: false, configurable: true })
    try {
      await startScanning(user)
      expect(screen.getByTestId('offline-banner')).toHaveTextContent('Offline')
      await user.type(screen.getByLabelText('Pass code'), 'EVT-OK-1')
      expect(screen.getByRole('button', { name: 'Check pass' })).toBeDisabled()
    } finally {
      if (desc) Object.defineProperty(Navigator.prototype, 'onLine', desc)
      // remove the instance override so other tests see the prototype getter again
      delete (window.navigator as unknown as Record<string, unknown>)['onLine']
    }
  })

  it('renders the designed "not yet enabled" state when the backend answers 404', async () => {
    setUrl('/') // real seam path, no fixtures
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue(
        new Response(JSON.stringify({ error: 'not_found' }), {
          status: 404,
          headers: { 'Content-Type': 'application/json' },
        }),
      ),
    )
    render(<EventStaffApp />)
    const panel = await screen.findByTestId('staff-unavailable')
    expect(panel).toHaveTextContent('Event passes are not yet enabled on this server')
  })
})
