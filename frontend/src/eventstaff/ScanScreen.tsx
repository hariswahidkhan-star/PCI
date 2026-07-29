import { useCallback, useEffect, useRef, useState, type FormEvent } from 'react'
import type { AdmissionDecision, EventGate, StaffEvent } from '../api/types'
import {
  EventAdmissionUnavailableError,
  confirmDecision,
  lookupAttendee,
  submitScan,
} from '../api/eventAdmission'
import { makeScanDebouncer } from './debounce'
import ResultPanel from './ResultPanel'
import { S } from './styles'

// The gate screen (§7A.8). Camera scanning is an ENHANCEMENT layered on top of the manual-entry
// form, which is the primary, always-available path: BarcodeDetector + getUserMedia are
// feature-detected, permission is requested only after an explanation, and when either is missing
// the manual form simply stands alone. Camera frames are handed to the platform detector
// in-memory and are NEVER captured, stored, or uploaded. Every decision comes from the seam
// (server-made on the real path); this screen only renders and confirms it.

interface BarcodeDetectorLike {
  detect(source: HTMLVideoElement): Promise<Array<{ rawValue: string }>>
}
type BarcodeDetectorCtor = new (options?: { formats?: string[] }) => BarcodeDetectorLike

function getBarcodeDetectorCtor(): BarcodeDetectorCtor | null {
  const w = window as unknown as { BarcodeDetector?: BarcodeDetectorCtor }
  return typeof w.BarcodeDetector === 'function' ? w.BarcodeDetector : null
}

function cameraSupported(): boolean {
  return (
    typeof navigator !== 'undefined' &&
    !!navigator.mediaDevices &&
    typeof navigator.mediaDevices.getUserMedia === 'function' &&
    getBarcodeDetectorCtor() !== null
  )
}

export default function ScanScreen({
  event,
  gate,
  online,
  onChangeGate,
}: {
  event: StaffEvent
  gate: EventGate
  online: boolean
  onChangeGate: () => void
}) {
  const [decision, setDecision] = useState<AdmissionDecision | null>(null)
  const [confirmed, setConfirmed] = useState(false)
  const [busy, setBusy] = useState(false)
  const [unavailable, setUnavailable] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [duplicateNote, setDuplicateNote] = useState(false)
  const [manualToken, setManualToken] = useState('')
  const [lookupQuery, setLookupQuery] = useState('')

  // Camera state
  const [camOn, setCamOn] = useState(false)
  const [camError, setCamError] = useState<string | null>(null)
  const videoRef = useRef<HTMLVideoElement | null>(null)
  const streamRef = useRef<MediaStream | null>(null)

  const debouncer = useRef(makeScanDebouncer())
  const decisionRef = useRef<AdmissionDecision | null>(null)
  decisionRef.current = decision

  /** Rapid reset between attendees — clears ALL prior-attendee PII from the screen. */
  const resetForNext = useCallback(() => {
    setDecision(null)
    setConfirmed(false)
    setError(null)
    setDuplicateNote(false)
    setManualToken('')
    setLookupQuery('')
  }, [])

  const handleDecision = useCallback(async (fn: () => Promise<AdmissionDecision>) => {
    setBusy(true)
    setError(null)
    setDuplicateNote(false)
    try {
      const d = await fn()
      setConfirmed(false)
      setDecision(d)
    } catch (e) {
      if (e instanceof EventAdmissionUnavailableError) setUnavailable(true)
      else setError(e instanceof Error ? e.message : 'The check could not be completed.')
    } finally {
      setBusy(false)
    }
  }, [])

  const handleToken = useCallback(
    (token: string) => {
      const t = token.trim()
      if (!t || busy || decisionRef.current) return
      if (!debouncer.current.accept(t)) {
        setDuplicateNote(true)
        return
      }
      void handleDecision(() => submitScan({ eventId: event.id, gateId: gate.id, token: t }))
    },
    [busy, event.id, gate.id, handleDecision],
  )

  function submitManualToken(e: FormEvent) {
    e.preventDefault()
    handleToken(manualToken)
  }

  function submitLookup(e: FormEvent) {
    e.preventDefault()
    const q = lookupQuery.trim()
    if (!q || busy || decision) return
    void handleDecision(() => lookupAttendee({ eventId: event.id, gateId: gate.id, query: q }))
  }

  async function confirmAdmit() {
    if (!decision) return
    setBusy(true)
    setError(null)
    try {
      await confirmDecision({ decisionId: decision.decisionId, gateId: gate.id })
      setConfirmed(true)
    } catch (e) {
      if (e instanceof EventAdmissionUnavailableError) setUnavailable(true)
      else setError(e instanceof Error ? e.message : 'The confirmation could not be recorded.')
    } finally {
      setBusy(false)
    }
  }

  // --- camera plumbing (no frame ever leaves the detector call) ---
  const stopCamera = useCallback(() => {
    streamRef.current?.getTracks().forEach((t) => t.stop())
    streamRef.current = null
    setCamOn(false)
  }, [])

  async function startCamera() {
    setCamError(null)
    const Ctor = getBarcodeDetectorCtor()
    if (!Ctor || !navigator.mediaDevices?.getUserMedia) {
      setCamError('Camera scanning is not supported on this device — use manual entry below.')
      return
    }
    try {
      const stream = await navigator.mediaDevices.getUserMedia({
        video: { facingMode: 'environment' },
        audio: false,
      })
      streamRef.current = stream
      if (videoRef.current) {
        videoRef.current.srcObject = stream
        await videoRef.current.play().catch(() => undefined)
      }
      setCamOn(true)
    } catch {
      setCamError('Camera permission was declined — manual entry below works without it.')
    }
  }

  useEffect(() => {
    if (!camOn) return
    const Ctor = getBarcodeDetectorCtor()
    if (!Ctor) return
    const detector = new Ctor({ formats: ['qr_code'] })
    const timer = window.setInterval(() => {
      const video = videoRef.current
      if (!video || video.readyState < 2 || decisionRef.current) return
      // Frames are read by the platform detector in-memory only — nothing is drawn, kept or sent.
      void detector
        .detect(video)
        .then((codes) => {
          const raw = codes[0]?.rawValue
          if (raw) handleToken(raw)
        })
        .catch(() => undefined)
    }, 350)
    return () => window.clearInterval(timer)
  }, [camOn, handleToken])

  useEffect(() => stopCamera, [stopCamera])

  if (unavailable) {
    return (
      <div style={S.panel} data-testid="scanner-unavailable">
        <h2 style={S.h2}>Event passes are not yet enabled on this server</h2>
        <p style={S.muted}>
          The admission service is not switched on for this installation, so no entry decisions can
          be made here. Admit attendees per the event's paper procedure, and contact the event
          administrator to enable event passes.
        </p>
      </div>
    )
  }

  return (
    <div>
      <div style={{ ...S.row, justifyContent: 'space-between', marginBottom: '.75rem' }}>
        <div>
          <strong>{event.title}</strong>
          <div style={S.muted}>
            {gate.name} · {event.timezone}
          </div>
        </div>
        <button style={S.btnSecondary} onClick={onChangeGate}>
          Change event / gate
        </button>
      </div>

      {decision ? (
        <div>
          <ResultPanel decision={decision} />
          {error && (
            <div role="alert" style={S.errNote}>
              {error}
            </div>
          )}
          <div style={{ ...S.row, marginTop: '1rem', gap: '.6rem' }}>
            {decision.result === 'admit' && !confirmed && (
              <button style={S.btnBig} disabled={busy || !online} onClick={() => void confirmAdmit()}>
                {busy ? 'Recording…' : 'Confirm admission'}
              </button>
            )}
            {decision.result === 'admit' && confirmed && (
              <div role="status" style={S.okNote}>
                Admission recorded.
              </div>
            )}
            <button style={S.btnBigSecondary} disabled={busy} onClick={resetForNext} data-testid="next-attendee">
              Next attendee
            </button>
          </div>
        </div>
      ) : (
        <div style={S.stack}>
          {error && (
            <div role="alert" style={S.errNote}>
              {error}
            </div>
          )}
          {duplicateNote && (
            <div role="status" style={S.warnNote} data-testid="duplicate-note">
              Duplicate scan ignored — this code was just checked.
            </div>
          )}

          {/* PRIMARY PATH: manual entry — works on every device, no camera or Barcode API needed. */}
          <form onSubmit={submitManualToken} style={S.panel} aria-label="Manual pass entry">
            <h2 style={S.h2}>Enter pass code</h2>
            <p style={S.muted}>Type or paste the code printed under the attendee's entry QR.</p>
            <div style={S.row}>
              <input
                style={S.input}
                value={manualToken}
                onChange={(e) => setManualToken(e.target.value)}
                placeholder="e.g. EVT-…"
                aria-label="Pass code"
                autoComplete="off"
              />
              <button style={S.btn} type="submit" disabled={busy || !online || !manualToken.trim()}>
                {busy ? 'Checking…' : 'Check pass'}
              </button>
            </div>
            {!online && <p style={S.muted}>Reconnect to check passes — decisions are made by the server.</p>}
          </form>

          <div style={S.panel}>
            <h2 style={S.h2}>Camera scanning</h2>
            {camOn ? (
              <div>
                <video
                  ref={videoRef}
                  style={{ width: '100%', maxHeight: 320, borderRadius: 10, background: '#000' }}
                  muted
                  playsInline
                />
                <div style={{ ...S.row, marginTop: '.5rem' }}>
                  <button style={S.btnSecondary} onClick={stopCamera}>
                    Stop camera
                  </button>
                </div>
              </div>
            ) : (
              <div>
                <p style={S.muted}>
                  The camera is used only to read entry-pass QR codes. Frames are processed on this
                  device and are never stored or uploaded.
                </p>
                {cameraSupported() ? (
                  <button style={S.btnSecondary} onClick={() => void startCamera()} disabled={!online}>
                    Enable camera
                  </button>
                ) : (
                  <p style={S.muted} data-testid="camera-unsupported">
                    Camera scanning is not available on this device or browser — use manual entry
                    above.
                  </p>
                )}
                {camError && (
                  <p role="status" style={{ ...S.muted, marginTop: '.4rem' }}>
                    {camError}
                  </p>
                )}
              </div>
            )}
          </div>

          {/* Identity LOOKUP — not authentication. */}
          <form onSubmit={submitLookup} style={S.panel} aria-label="Manual attendee lookup">
            <h2 style={S.h2}>Look up attendee</h2>
            <p style={S.muted}>
              Exact registration reference or Student Number. This is an identity lookup — it
              confirms a registration exists, it does not verify who is standing in front of you.
              Check photo ID per event policy.
            </p>
            <div style={S.row}>
              <input
                style={S.input}
                value={lookupQuery}
                onChange={(e) => setLookupQuery(e.target.value)}
                placeholder="PCI-EV-… or PCI-2024-…"
                aria-label="Registration reference or Student Number"
                autoComplete="off"
              />
              <button style={S.btn} type="submit" disabled={busy || !online || !lookupQuery.trim()}>
                {busy ? 'Looking up…' : 'Look up'}
              </button>
            </div>
          </form>
        </div>
      )}
    </div>
  )
}
