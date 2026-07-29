// THE ONE API SEAM for the §7A event-admission build (wallet page + staff scanner).
//
// The backend for these endpoints DOES NOT EXIST YET. Every network call in this file therefore
// handles 404/501 by throwing EventAdmissionUnavailableError, which the screens render as a
// designed "Event passes are not yet enabled on this server" state — never a broken screen.
// When the SQL/endpoint build lands, THIS FILE is the only frontend file that needs wiring.
//
// Fixtures mode (`?preview=1`, or sessionStorage 'pci.eventAdmission.fixtures' = '1' so the flag
// survives SPA navigation) serves realistic sample data so the UI is fully reviewable today. The
// mock admission-decision function exists ONLY on the fixtures path: in the real seam every
// decision comes from the server — there is deliberately NO client-side "admit" path, so a
// scanner that cannot reach the backend can never admit anyone.
//
// Planned wire contract (mirrored by the types in ./types.ts):
//   GET  /api/me/event-passes                       -> { rows: EventPass[] }        (student bearer)
//   POST /api/me/event-passes/{id}/replace          -> { pass: EventPass }          (student bearer)
//   GET  /api/eventstaff/events                     -> { rows: StaffEvent[] }       (staff bearer)
//   POST /api/eventstaff/scan                       -> AdmissionDecision            (staff bearer)
//   POST /api/eventstaff/lookup                     -> AdmissionDecision            (staff bearer)
//   POST /api/eventstaff/decisions/{id}/confirm     -> { ok: true }                 (staff bearer)

import { api, makeClient, ApiError } from './client'
import type {
  AdmissionDecision,
  EventPass,
  StaffEvent,
} from './types'

/** Thrown when the event-admission backend is not present on this server (404/501). */
export class EventAdmissionUnavailableError extends Error {
  constructor() {
    super('Event passes are not yet enabled on this server.')
    this.name = 'EventAdmissionUnavailableError'
  }
}

const FIXTURES_STORAGE_KEY = 'pci.eventAdmission.fixtures'

/** Dev fixtures mode: `?preview=1` on the current URL, or the sticky sessionStorage flag. */
export function fixturesEnabled(): boolean {
  try {
    if (new URLSearchParams(window.location.search).get('preview') === '1') return true
  } catch {
    /* no window (SSR/test) */
  }
  try {
    if (sessionStorage.getItem(FIXTURES_STORAGE_KEY) === '1') return true
  } catch {
    /* storage blocked */
  }
  return false
}

// Map "this endpoint does not exist" onto the honest unavailable state; let real errors through.
async function guard<T>(p: Promise<T>): Promise<T> {
  try {
    return await p
  } catch (e) {
    if (e instanceof ApiError && (e.status === 404 || e.status === 501)) {
      throw new EventAdmissionUnavailableError()
    }
    throw e
  }
}

// Gate staff are not students: their calls carry a separate bearer token under a separate key,
// following the repo's makeClient(tokenKey) convention (student and admin never share either).
const staffClient = makeClient('pci.eventstaff.token')

// ---------------------------------------------------------------------------
// Fixtures (dev preview only — realistic sample data, no network)
// ---------------------------------------------------------------------------

const FIXTURE_PASSES: EventPass[] = [
  {
    passId: 'pass_01',
    passReference: 'PCI-EV-2026-000731',
    state: 'active',
    event: {
      id: 501,
      title: 'PCI Global Project Controls Summit 2026',
      startsAt: '2026-09-14 09:00:00',
      endsAt: '2026-09-14 17:30:00',
      timezone: 'Europe/London',
      venueName: 'Queen Elizabeth II Centre',
      venueAddress: 'Broad Sanctuary, Westminster, London SW1P 3EE',
    },
    registrationState: 'registered',
    paymentState: 'paid',
    admissionMode: 'hybrid',
    validity: { opensAt: '2026-09-14 07:30:00', closesAt: '2026-09-14 12:00:00' },
    entryToken: 'EVT-OK-501-9f3ab2c8d1',
    history: [],
  },
  {
    passId: 'pass_02',
    passReference: 'PCI-EV-2026-000214',
    state: 'used',
    event: {
      id: 502,
      title: 'Regional Workshop — Cost Engineering in Practice',
      startsAt: '2026-05-02 08:30:00',
      endsAt: '2026-05-02 16:00:00',
      timezone: 'Asia/Dubai',
      venueName: 'Dubai World Trade Centre, Hall 4',
      venueAddress: 'Sheikh Zayed Rd, Dubai',
    },
    registrationState: 'registered',
    paymentState: 'paid',
    admissionMode: 'qr',
    validity: { opensAt: '2026-05-02 07:00:00', closesAt: '2026-05-02 11:00:00' },
    entryToken: 'EVT-USED-502-4c77e1aa02',
    history: [
      { at: '2026-05-02 08:12:00', kind: 'entry', gateName: 'Hall 4 — Gate A', method: 'qr' },
      { at: '2026-05-02 12:40:00', kind: 'checkout', gateName: 'Hall 4 — Gate A', method: 'manual' },
    ],
  },
  {
    passId: 'pass_03',
    passReference: 'PCI-EV-2026-000982',
    state: 'issued',
    event: {
      id: 503,
      title: 'AI in Project Controls — Evening Seminar',
      startsAt: '2026-11-05 18:00:00',
      endsAt: '2026-11-05 20:30:00',
      timezone: 'America/New_York',
      venueName: 'PCI Members Hub, New York',
      venueAddress: null,
    },
    registrationState: 'waitlisted',
    paymentState: 'pending',
    admissionMode: 'manual',
    validity: { opensAt: '2026-11-05 17:00:00', closesAt: '2026-11-05 19:00:00' },
    entryToken: null,
    history: [],
  },
]

const FIXTURE_STAFF_EVENTS: StaffEvent[] = [
  {
    id: 501,
    title: 'PCI Global Project Controls Summit 2026',
    startsAt: '2026-09-14 09:00:00',
    timezone: 'Europe/London',
    gates: [
      { id: 'gate_a', name: 'Main Hall — Gate A' },
      { id: 'gate_b', name: 'Main Hall — Gate B' },
      { id: 'gate_acc', name: 'Accessible entrance' },
    ],
  },
  {
    id: 503,
    title: 'AI in Project Controls — Evening Seminar',
    startsAt: '2026-11-05 18:00:00',
    timezone: 'America/New_York',
    gates: [{ id: 'gate_main', name: 'Front desk' }],
  },
]

// Deep-clone so screens can never mutate the fixture source between renders.
function clone<T>(v: T): T {
  return JSON.parse(JSON.stringify(v)) as T
}

let fixtureReplaceCounter = 0

/** FIXTURES-ONLY mock decision. Deterministic on the scanned token / lookup query so every result
 *  state is reachable in review: OK -> admit, USED -> already checked in, REVIEW -> manual review,
 *  anything else -> do not admit. Never used on the real seam path. */
export function mockDecision(tokenOrQuery: string): AdmissionDecision {
  const t = tokenOrQuery.trim().toUpperCase()
  const attendee = {
    displayName: 'Jordan Meyer',
    registrationReference: 'PCI-EV-2026-000731',
    studentNumber: 'PCI-2024-018233',
    passState: 'active' as const,
  }
  if (t.includes('USED') || t === 'PCI-EV-2026-000214') {
    return {
      decisionId: 'dec_fixture_used',
      result: 'already_checked_in',
      reason: 'pass_already_used',
      attendee: { ...attendee, displayName: 'Amara Okafor', registrationReference: 'PCI-EV-2026-000214', passState: 'used' },
      firstEntryAt: '2026-05-02 08:12:00',
      firstEntryGate: 'Hall 4 — Gate A',
    }
  }
  if (t.includes('REVIEW')) {
    return {
      decisionId: 'dec_fixture_review',
      result: 'manual_review',
      reason: 'payment_pending',
      attendee: { ...attendee, displayName: 'Luca Bianchi', registrationReference: 'PCI-EV-2026-000982', passState: 'issued' },
    }
  }
  if (t.includes('OK') || t === 'PCI-EV-2026-000731' || t === 'PCI-2024-018233') {
    return { decisionId: 'dec_fixture_admit', result: 'admit', reason: null, attendee }
  }
  return { decisionId: 'dec_fixture_deny', result: 'do_not_admit', reason: 'pass_not_found', attendee: null }
}

// ---------------------------------------------------------------------------
// Student wallet (My event passes, §7A.12A)
// ---------------------------------------------------------------------------

export async function fetchMyEventPasses(): Promise<EventPass[]> {
  if (fixturesEnabled()) return clone(FIXTURE_PASSES)
  const res = await guard(api.get<{ rows: EventPass[] }>('/api/me/event-passes'))
  return res.rows
}

/** Replace a pass (lost/compromised): the old QR stops working immediately, server-side. */
export async function replaceEventPass(passId: string): Promise<EventPass> {
  if (fixturesEnabled()) {
    const src = FIXTURE_PASSES.find((p) => p.passId === passId) ?? FIXTURE_PASSES[0]
    fixtureReplaceCounter += 1
    const fresh = clone(src)
    fresh.passId = `${src.passId}_r${fixtureReplaceCounter}`
    fresh.passReference = `${src.passReference}-R${fixtureReplaceCounter}`
    fresh.state = src.entryToken ? 'active' : 'issued'
    fresh.entryToken = src.entryToken ? `EVT-OK-${src.event.id}-replaced${fixtureReplaceCounter}` : null
    return fresh
  }
  const res = await guard(
    api.post<{ pass: EventPass }>(`/api/me/event-passes/${encodeURIComponent(passId)}/replace`, {}),
  )
  return res.pass
}

// ---------------------------------------------------------------------------
// Staff scanner (§7A.8) — decisions are ALWAYS server-made on the real path
// ---------------------------------------------------------------------------

export async function fetchStaffEvents(): Promise<StaffEvent[]> {
  if (fixturesEnabled()) return clone(FIXTURE_STAFF_EVENTS)
  const res = await guard(staffClient.get<{ rows: StaffEvent[] }>('/api/eventstaff/events'))
  return res.rows
}

/** Submit a scanned entry token. The server returns the decision; the client only renders it. */
export async function submitScan(input: { eventId: number; gateId: string; token: string }): Promise<AdmissionDecision> {
  if (fixturesEnabled()) return mockDecision(input.token)
  return guard(staffClient.post<AdmissionDecision>('/api/eventstaff/scan', input))
}

/** Manual lookup by EXACT registration reference or Student Number. This is an identity LOOKUP,
 *  not authentication — staff must still check photo ID per event policy. */
export async function lookupAttendee(input: { eventId: number; gateId: string; query: string }): Promise<AdmissionDecision> {
  if (fixturesEnabled()) return mockDecision(input.query)
  return guard(staffClient.post<AdmissionDecision>('/api/eventstaff/lookup', input))
}

/** Record the operator's explicit confirm of an admit decision. */
export async function confirmDecision(input: { decisionId: string; gateId: string }): Promise<void> {
  if (fixturesEnabled()) return
  await guard(
    staffClient.post<{ ok: boolean }>(
      `/api/eventstaff/decisions/${encodeURIComponent(input.decisionId)}/confirm`,
      { gateId: input.gateId },
    ),
  )
}
