import { useCallback, useEffect, useState } from 'react'
import type { EventGate, StaffEvent } from '../api/types'
import { EventAdmissionUnavailableError, enableFixtures, fetchStaffEvents, fixturesEnabled } from '../api/eventAdmission'
import ScanScreen from './ScanScreen'
import { S } from './styles'

// Root of the gate-staff scanner (§7A.8): pick event + gate, then scan. Self-contained — see
// README.md: this tree is intentionally NOT mounted in any app shell yet; the mount decision
// (own vite entry vs. a route in an operator surface) is deferred to integration.

export default function EventStaffApp() {
  const [events, setEvents] = useState<StaffEvent[] | null>(null)
  const [loading, setLoading] = useState(true)
  const [unavailable, setUnavailable] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const [eventId, setEventId] = useState<number | ''>('')
  const [gateId, setGateId] = useState<string>('')
  const [started, setStarted] = useState(false)

  // Offline banner: gate connectivity is flaky by nature and no decision can be made offline.
  const [online, setOnline] = useState(typeof navigator === 'undefined' ? true : navigator.onLine)
  useEffect(() => {
    const up = () => setOnline(true)
    const down = () => setOnline(false)
    window.addEventListener('online', up)
    window.addEventListener('offline', down)
    return () => {
      window.removeEventListener('online', up)
      window.removeEventListener('offline', down)
    }
  }, [])

  const load = useCallback(async () => {
    setLoading(true)
    setUnavailable(false)
    setError(null)
    try {
      setEvents(await fetchStaffEvents())
    } catch (e) {
      if (e instanceof EventAdmissionUnavailableError) setUnavailable(true)
      else setError(e instanceof Error ? e.message : 'Could not load events.')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    void load()
  }, [load])

  const selectedEvent = events?.find((e) => e.id === eventId) ?? null
  const selectedGate: EventGate | null = selectedEvent?.gates.find((g) => g.id === gateId) ?? null

  return (
    <div style={S.root}>
      <h1 style={S.h1}>PCI event check-in</h1>
      <p style={S.muted}>Gate staff scanner — admission decisions are made by the PCI server.</p>
      {fixturesEnabled() && (
        <div role="status" style={{ ...S.warnNote, marginBottom: '.75rem' }}>
          Preview mode — sample events and a mock decision service. Nothing touches the server.
        </div>
      )}
      {!online && (
        <div role="alert" style={S.offlineBanner} data-testid="offline-banner">
          Offline — passes cannot be checked until the connection returns. Do not admit on a stale
          screen.
        </div>
      )}

      {loading ? (
        <p style={S.muted}>Loading events…</p>
      ) : unavailable ? (
        <div style={S.panel} data-testid="staff-unavailable">
          <h2 style={S.h2}>Event passes are not yet enabled on this server</h2>
          <p style={S.muted}>
            The event-admission service is not switched on for this installation. There is nothing
            to scan yet — contact the event administrator.
          </p>
          <button
            type="button"
            style={S.button}
            onClick={() => { enableFixtures(); window.location.reload() }}
          >
            Try the scanner with sample data
          </button>
        </div>
      ) : error ? (
        <div role="alert" style={S.errNote}>
          {error}
        </div>
      ) : !events || events.length === 0 ? (
        <div style={S.panel}>
          <p style={S.muted}>No events with gate scanning are running right now.</p>
        </div>
      ) : started && selectedEvent && selectedGate ? (
        <ScanScreen
          event={selectedEvent}
          gate={selectedGate}
          online={online}
          onChangeGate={() => setStarted(false)}
        />
      ) : (
        <div style={S.panel}>
          <h2 style={S.h2}>Select your event and gate</h2>
          <div style={{ ...S.stack, gap: '.6rem' }}>
            <label style={{ display: 'block' }}>
              <div style={S.muted}>Event</div>
              <select
                style={{ ...S.input, width: '100%' }}
                value={eventId === '' ? '' : String(eventId)}
                onChange={(e) => {
                  const v = e.target.value
                  setEventId(v === '' ? '' : Number(v))
                  setGateId('')
                }}
                aria-label="Event"
              >
                <option value="">Choose an event…</option>
                {events.map((ev) => (
                  <option key={ev.id} value={ev.id}>
                    {ev.title}
                  </option>
                ))}
              </select>
            </label>
            <label style={{ display: 'block' }}>
              <div style={S.muted}>Gate</div>
              <select
                style={{ ...S.input, width: '100%' }}
                value={gateId}
                onChange={(e) => setGateId(e.target.value)}
                disabled={!selectedEvent}
                aria-label="Gate"
              >
                <option value="">Choose a gate…</option>
                {selectedEvent?.gates.map((g) => (
                  <option key={g.id} value={g.id}>
                    {g.name}
                  </option>
                ))}
              </select>
            </label>
            <div>
              <button
                style={S.btn}
                disabled={!selectedEvent || !selectedGate}
                onClick={() => setStarted(true)}
              >
                Start scanning
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}
