import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest'
import {
  EventAdmissionUnavailableError,
  fetchMyEventPasses,
  fixturesEnabled,
  mockDecision,
  submitScan,
} from '../api/eventAdmission'

// The ONE seam module for event admission. Pinned: fixtures-mode detection (?preview=1 or the
// sticky sessionStorage flag); the deterministic mock decision map used only in fixtures; and the
// honest 404/501 -> EventAdmissionUnavailableError mapping on the real path, while genuine
// failures (500) pass through untouched. Lives under src/eventstaff/ with the rest of this
// workstream's tests.

function setUrl(path: string) {
  window.history.replaceState({}, '', path)
}

function stubFetchStatus(status: number) {
  vi.stubGlobal(
    'fetch',
    vi.fn().mockResolvedValue(
      new Response(JSON.stringify({ error: 'x' }), {
        status,
        headers: { 'Content-Type': 'application/json' },
      }),
    ),
  )
}

describe('event-admission seam', () => {
  beforeEach(() => {
    setUrl('/')
    sessionStorage.clear()
  })
  afterEach(() => {
    setUrl('/')
    vi.unstubAllGlobals()
  })

  it('fixtures mode is off by default, on with ?preview=1, on with the sessionStorage flag', () => {
    expect(fixturesEnabled()).toBe(false)
    setUrl('/?preview=1')
    expect(fixturesEnabled()).toBe(true)
    setUrl('/')
    sessionStorage.setItem('pci.eventAdmission.fixtures', '1')
    expect(fixturesEnabled()).toBe(true)
  })

  it('fixtures mode serves sample passes without any network call', async () => {
    setUrl('/?preview=1')
    const spy = vi.fn()
    vi.stubGlobal('fetch', spy)
    const passes = await fetchMyEventPasses()
    expect(passes.length).toBe(3)
    expect(passes[0].passReference).toBe('PCI-EV-2026-000731')
    expect(spy).not.toHaveBeenCalled()
  })

  it('mock decision map covers all four results deterministically', () => {
    expect(mockDecision('EVT-OK-1').result).toBe('admit')
    expect(mockDecision('evt-used-9').result).toBe('already_checked_in')
    expect(mockDecision('EVT-REVIEW-2').result).toBe('manual_review')
    expect(mockDecision('EVT-ZZZ').result).toBe('do_not_admit')
    // do_not_admit exposes no attendee PII
    expect(mockDecision('EVT-ZZZ').attendee).toBeNull()
  })

  it('maps 404 and 501 to EventAdmissionUnavailableError on the real path', async () => {
    stubFetchStatus(404)
    await expect(fetchMyEventPasses()).rejects.toBeInstanceOf(EventAdmissionUnavailableError)
    stubFetchStatus(501)
    await expect(
      submitScan({ eventId: 1, gateId: 'g', token: 'EVT-OK-WOULD-ADMIT-IN-FIXTURES' }),
    ).rejects.toBeInstanceOf(EventAdmissionUnavailableError)
  })

  it('lets a genuine 500 through as a normal error (not the unavailable state)', async () => {
    stubFetchStatus(500)
    await expect(fetchMyEventPasses()).rejects.toSatisfy(
      (e: unknown) => !(e instanceof EventAdmissionUnavailableError),
    )
  })

  it('real seam path never decides client-side: an unreachable server yields no decision at all', async () => {
    // Network down entirely — the scan must reject, never resolve to any AdmissionDecision.
    vi.stubGlobal('fetch', vi.fn().mockRejectedValue(new TypeError('network down')))
    await expect(submitScan({ eventId: 1, gateId: 'g', token: 'EVT-OK-1' })).rejects.toBeTruthy()
  })
})
