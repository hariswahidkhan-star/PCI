import { useEffect, useMemo, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { useMe } from '../data/MeContext'
import { useQuery } from '../api/hooks'
import { api, ApiError } from '../api/client'
import { Card, Badge, Spinner } from '../components/ui'
import { PageHeader, EmptyState } from '../components/premium'
import { fmtDateTime } from '../format'
import ExamRunner, { type StartPayload } from '../exam/ExamRunner'
import type { ExamEntry } from '../api/types'

/**
 * Exam-day check-in — the whole launch journey inside the React portal (no classic panel):
 * booking summary, the pre-exam checklist, the system readiness check (camera/mic/network probes
 * recorded via POST /api/me/readiness), the launch window countdown, and the two launch paths —
 * the in-app browser runner (exam/ExamRunner.tsx) or the PCI Secure Exam desktop app via a
 * single-use launch code. The server owns every gate (open window, readiness, eligibility); this
 * page mirrors them so the buttons tell the truth, but never decides on its own.
 */

const CHECKLIST: { title: string; sub: string }[] = [
  { title: 'Read the examination rules', sub: 'Identity, environment and permitted materials — no surprises.' },
  { title: 'Run the system check', sub: 'Camera, microphone and connection verified below.' },
  { title: 'Prepare your ID', sub: 'Government photo ID matching your enrolment name.' },
  { title: 'Clear your workspace', sub: 'Quiet room, single screen, notes away.' },
]
const CHECK_KEY = 'pci.examday.check'

/** Backend datetimes are 'YYYY-MM-DD HH:MM:SS' (UTC) or ISO — normalise to epoch millis. */
function utcMs(v: unknown): number | null {
  if (!v) return null
  const s = String(v).trim()
  const normalized = /Z$/i.test(s) ? s : s.includes('T') ? `${s}Z` : `${s.replace(' ', 'T')}Z`
  const ms = Date.parse(normalized)
  return Number.isFinite(ms) ? ms : null
}

function fmtCountdown(totalS: number): string {
  const d = Math.floor(totalS / 86400)
  const h = Math.floor((totalS % 86400) / 3600)
  const m = Math.floor((totalS % 3600) / 60)
  const s = totalS % 60
  return `${d > 0 ? `${d}d ` : ''}${String(h).padStart(2, '0')}:${String(m).padStart(2, '0')}:${String(s).padStart(2, '0')}`
}

interface ReadinessState {
  required: boolean
  latest?: { passed?: number | boolean } | null
}

interface ProbeRow {
  label: string
  ok: boolean
  hint: string
}

const START_ERRORS: Record<string, string> = {
  no_booking: 'There is no scheduled booking for this examination.',
  not_open: 'The launch window has not opened yet — it opens shortly before your slot.',
  missed: 'The launch window has closed and the booking is marked missed. Contact support — documented incidents are considered under the examination administration policy.',
  readiness_required: 'Run and pass the system readiness check below before launching.',
  not_eligible: 'Your exam access is on hold. Resolve the outstanding requirement (identity document, consents, or account status) before launching.',
  no_items: 'This certification’s examination is not yet published. Contact support.',
  already_submitted: 'This examination has already been submitted — see your result on the Results page.',
  external_delivery: 'This examination is delivered by an external test provider — use the scheduling details they sent you.',
}

export default function ExamDay() {
  const { certId } = useParams()
  const navigate = useNavigate()
  const { me, loading, refetch } = useMe()
  const [now, setNow] = useState(() => Date.now())
  const [check, setCheck] = useState<boolean[]>(() => {
    try { return (JSON.parse(localStorage.getItem(CHECK_KEY) || '[]') as boolean[]).slice(0, 4) } catch { return [] }
  })
  const [probes, setProbes] = useState<ProbeRow[] | null>(null)
  const [probing, setProbing] = useState(false)
  const [readyOk, setReadyOk] = useState<boolean | null>(null)
  const [launchErr, setLaunchErr] = useState<string | null>(null)
  const [launching, setLaunching] = useState(false)
  const [desktopNote, setDesktopNote] = useState<string | null>(null)
  const [runner, setRunner] = useState<StartPayload | null>(null)

  const readiness = useQuery<ReadinessState>('/api/me/readiness')
  const cfg = useQuery<Record<string, string>>('/api/me/config')

  // The entry this check-in is for: the route's certification, else the first scheduled booking.
  const entry: ExamEntry | null = useMemo(() => {
    const exams = me?.exams ?? []
    const withBooking = exams.filter((e) => e.booking && String((e.booking as Record<string, unknown>).status) === 'scheduled')
    if (certId) return withBooking.find((e) => e.certification_id === Number(certId)) ?? null
    return withBooking[0] ?? null
  }, [me, certId])

  const delivery = useQuery<{ routed: boolean; provider_name?: string; provider?: string; self_schedule?: boolean; confirmation?: string | null }>(
    entry ? `/api/me/exam/delivery?certification_id=${entry.certification_id}` : null)
  const vendor = delivery.data?.routed ? delivery.data : null

  useEffect(() => {
    const t = window.setInterval(() => setNow(Date.now()), 1000)
    return () => window.clearInterval(t)
  }, [])

  if (loading || !me) return <Spinner />

  const certLabel = entry?.certification_name || entry?.certification_code || 'PCI examination'

  if (runner && entry) {
    return (
      <ExamRunner
        start={runner}
        certLabel={String(entry.certification_code || certLabel)}
        onExit={(finished) => {
          setRunner(null)
          void refetch()
          if (finished) navigate('/results')
        }}
      />
    )
  }

  if (!entry) {
    return (
      <div className="page">
        <PageHeader eyebrow="Exam day" title="Exam day check-in" />
        <Card>
          <EmptyState
            icon="calendar"
            title="No scheduled examination"
            detail="Schedule your examination from the Certifications page first — check-in opens once a booking is confirmed."
          />
          <div style={{ textAlign: 'center', marginTop: '.5rem' }}>
            <Link className="btn" to="/certifications">Go to Certifications</Link>
          </div>
        </Card>
      </div>
    )
  }

  const booking = entry.booking as Record<string, unknown>
  const slotMs = utcMs(booking.scheduled_at) ?? now
  const openBefore = Number(cfg.data?.exam_open_before_minutes ?? 15) || 15
  const openMs = slotMs - openBefore * 60_000
  const closeMs = slotMs + 30 * 60_000
  const phase: 'before' | 'open' | 'closed' = now < openMs ? 'before' : now <= closeMs ? 'open' : 'closed'

  const readinessRequired = (cfg.data?.sp_readiness_required ?? '1') === '1' || readiness.data?.required === true
  const readinessPassed = readyOk ?? Boolean(readiness.data?.latest && (readiness.data.latest.passed === 1 || readiness.data.latest.passed === true))
  const allChecked = CHECKLIST.every((_, i) => check[i]) && (!readinessRequired || readinessPassed)

  const toggleCheck = (i: number) => {
    const next = [...check]
    next[i] = !next[i]
    setCheck(next)
    try { localStorage.setItem(CHECK_KEY, JSON.stringify(next)) } catch { /* ignore */ }
  }

  // The readiness probes: capability checks only — tracks are stopped immediately after the probe.
  async function runReadiness() {
    setProbing(true)
    const rows: ProbeRow[] = []
    let camera = false
    let microphone = false
    try {
      const cam = await navigator.mediaDevices.getUserMedia({ video: true })
      camera = true
      cam.getTracks().forEach((t) => t.stop())
    } catch { camera = false }
    try {
      const mic = await navigator.mediaDevices.getUserMedia({ audio: true })
      microphone = true
      mic.getTracks().forEach((t) => t.stop())
    } catch { microphone = false }
    const network = navigator.onLine !== false
    const screenOk = window.screen.width >= 1024
    const fullscreen = Boolean(document.documentElement.requestFullscreen)
    rows.push({ label: 'Camera', ok: camera, hint: 'allow camera access' })
    rows.push({ label: 'Microphone', ok: microphone, hint: 'allow microphone access' })
    rows.push({ label: 'Connection', ok: network, hint: 'check your internet' })
    rows.push({ label: 'Browser', ok: Boolean(window.fetch), hint: 'use a modern browser' })
    rows.push({ label: 'Screen size', ok: screenOk, hint: 'minimum 1024px width' })
    setProbes(rows)
    let passed = camera && microphone && network
    try {
      const r = await api.post<{ passed: boolean }>('/api/me/readiness', {
        camera, microphone, network, fullscreen, environment: true,
        browser: navigator.userAgent.slice(0, 120),
        screen: `${window.screen.width}×${window.screen.height}`,
      })
      passed = Boolean(r.passed)
    } catch { /* the local verdict stands; the next run re-records */ }
    setReadyOk(passed)
    setProbing(false)
  }

  async function launchBrowser() {
    setLaunchErr(null)
    setLaunching(true)
    try {
      const r = await api.post<StartPayload>('/api/me/exam/start', { certification_id: entry!.certification_id })
      setRunner(r)
    } catch (e) {
      const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : {}
      const code = String(body.error ?? '')
      setLaunchErr(START_ERRORS[code] || (e instanceof Error ? e.message : 'Could not start the examination.'))
      if (code === 'missed') void refetch()
      if (code === 'already_submitted') navigate('/results')
    } finally {
      setLaunching(false)
    }
  }

  async function launchDesktop() {
    setLaunchErr(null)
    setDesktopNote(null)
    try {
      const r = await api.post<{ code: string; uri?: string }>('/api/me/exam/launch-code', {})
      const uri = `${r.uri || `pciexam://start?code=${r.code}`}&api=${encodeURIComponent(window.location.origin)}`
      window.location.href = uri
      setDesktopNote('Opening the PCI Secure Exam app… if nothing happens, install it from your account and try again.')
    } catch {
      setLaunchErr('Could not create a launch code — please try again.')
    }
  }

  const slotDate = new Date(slotMs)

  return (
    <div className="page fade-stagger">
      <PageHeader
        eyebrow="Exam day"
        title="Exam day check-in"
        subtitle={`${certLabel} · ${fmtDateTime(booking.scheduled_at, booking.timezone)}${booking.timezone ? ` (${String(booking.timezone)})` : ''}`}
      />

      <div className="grid cols-2" style={{ alignItems: 'start' }}>
        <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
          <Card title="Your booking" action={<Badge tone="ok">Confirmed</Badge>}>
            <div className="row" style={{ gap: '1rem', alignItems: 'center', flexWrap: 'wrap' }}>
              <div style={{ background: 'var(--brand)', color: '#fff', borderRadius: 14, padding: '.8rem 1.1rem', textAlign: 'center', minWidth: 88 }}>
                <b style={{ fontSize: '1.6rem', display: 'block', lineHeight: 1 }}>{slotDate.getDate()}</b>
                <span style={{ fontSize: '.7rem', letterSpacing: 1, textTransform: 'uppercase', opacity: .85 }}>
                  {slotDate.toLocaleDateString(undefined, { month: 'short' })}
                </span>
              </div>
              <div style={{ flex: 1, minWidth: 200 }}>
                <b>{fmtDateTime(booking.scheduled_at, booking.timezone)}</b>
                <div className="muted small">{certLabel}{booking.timezone ? ` · ${String(booking.timezone)}` : ''}</div>
              </div>
              <Link className="btn secondary sm" to="/certifications">Reschedule</Link>
            </div>
          </Card>

          <Card title="Launch">
            {vendor ? (
              <div className="notice">
                Delivered by <strong>{vendor.provider_name || vendor.provider}</strong>
                {vendor.confirmation ? <> · confirmation <strong>{vendor.confirmation}</strong></> : null}
                <div className="muted small" style={{ marginTop: '.4rem' }}>
                  This examination is sat with the external test provider — use the appointment
                  details they emailed you. Your result posts back here automatically.
                </div>
              </div>
            ) : phase === 'before' ? (
              <div style={{ textAlign: 'center', padding: '.6rem 0' }}>
                <div style={{ fontSize: '1.8rem', fontWeight: 700, fontVariantNumeric: 'tabular-nums' }}>
                  {fmtCountdown(Math.floor((openMs - now) / 1000))}
                </div>
                <div className="muted small">until launch opens</div>
                <p className="muted small" style={{ margin: '.7rem 0 0' }}>
                  Launch activates {openBefore} minutes before your slot and stays open for a 30-minute grace period.
                </p>
              </div>
            ) : phase === 'open' ? (
              <div style={{ textAlign: 'center', padding: '.4rem 0' }}>
                <Badge tone="ok">Launch window open — closes {fmtDateTime(new Date(closeMs).toISOString())}</Badge>
                <div style={{ marginTop: '.9rem' }}>
                  <button className="btn" disabled={!allChecked || launching} onClick={() => { void launchBrowser() }}>
                    {launching ? 'Starting…' : 'Launch my examination'}
                  </button>
                </div>
                {allChecked && (
                  <div style={{ marginTop: '.6rem' }}>
                    <button className="btn secondary sm" onClick={() => { void launchDesktop() }}>
                      Open in the PCI Secure Exam app instead
                    </button>
                  </div>
                )}
                {!allChecked && (
                  <p className="muted small" style={{ margin: '.7rem 0 0' }}>
                    {readinessRequired && !readinessPassed
                      ? 'Run and pass the system readiness check'
                      : 'Complete the checklist'}{' '}
                    to enable launch.
                  </p>
                )}
              </div>
            ) : (
              <div style={{ textAlign: 'center', padding: '.4rem 0' }}>
                <h4 style={{ color: 'var(--err)' }}>Launch window closed</h4>
                <p className="muted small" style={{ maxWidth: '46ch', margin: '.4rem auto 0' }}>
                  The grace period passed and the booking is marked missed. Contact support —
                  documented incidents are considered under the examination administration policy.
                </p>
                <Link className="btn secondary sm" to="/support" style={{ marginTop: '.8rem' }}>Open a ticket</Link>
              </div>
            )}
            {launchErr && <div className="notice" style={{ marginTop: '.8rem', color: 'var(--err)' }} role="alert">{launchErr}</div>}
            {desktopNote && <div className="notice" style={{ marginTop: '.8rem' }} role="status">{desktopNote}</div>}
          </Card>
        </div>

        <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
          <Card title="Before you launch">
            <div style={{ display: 'grid', gap: '.6rem' }}>
              {CHECKLIST.map((c, i) => (
                <label key={c.title} className="row" style={{ gap: '.6rem', alignItems: 'flex-start', cursor: 'pointer' }}>
                  {/* the global `input { width:100% }` rule would stretch a bare checkbox */}
                  <input
                    type="checkbox"
                    checked={Boolean(check[i])}
                    onChange={() => toggleCheck(i)}
                    style={{ marginTop: '.25rem', width: 'auto', flex: '0 0 auto' }}
                  />
                  <span>
                    <b className="small">{c.title}</b>
                    <span className="muted small" style={{ display: 'block' }}>{c.sub}</span>
                  </span>
                </label>
              ))}
            </div>
          </Card>

          <Card
            title="System readiness check"
            action={
              <button className="btn sm" disabled={probing} onClick={() => { void runReadiness() }}>
                {probing ? 'Checking…' : 'Run check'}
              </button>
            }
          >
            {probes === null ? (
              <p className="muted small" style={{ margin: 0 }}>
                Checks your camera, microphone, connection and screen.
                {readinessRequired && ' A passed check is required before you can launch.'}
                {!probes && readinessPassed && (
                  <span style={{ display: 'block', marginTop: '.4rem', color: 'var(--ok)' }}>
                    Your last readiness check passed.
                  </span>
                )}
              </p>
            ) : (
              <>
                <div style={{ display: 'grid', gap: '.4rem' }}>
                  {probes.map((p) => (
                    <div key={p.label} className="row" style={{ justifyContent: 'space-between' }}>
                      <span className="small">{p.ok ? '✅' : '❌'} {p.label}</span>
                      <span className="muted small">{p.ok ? 'OK' : p.hint}</span>
                    </div>
                  ))}
                </div>
                {readyOk !== null && (
                  <div className="notice" style={{ marginTop: '.7rem' }}>
                    {readyOk ? (
                      <><strong>Ready.</strong> Your system meets the requirements for a proctored exam.</>
                    ) : (
                      <><strong>Not ready.</strong> Camera, microphone and a working connection are all
                        required. Fix the items above and run the check again.</>
                    )}
                  </div>
                )}
              </>
            )}
          </Card>
        </div>
      </div>
    </div>
  )
}
