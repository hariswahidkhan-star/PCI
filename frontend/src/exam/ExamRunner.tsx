import { useCallback, useEffect, useMemo, useRef, useState, type ReactNode } from 'react'
import { createPortal } from 'react-dom'
import { Link } from 'react-router-dom'
import { api, ApiError } from '../api/client'
import { Dial, DomainBands, parseBreakdown, type DomainBand } from '../components/results'
import './exam.css'

/**
 * The in-app secure exam runner — the browser sitting, fully inside the React portal (no classic
 * panel). Mirrors the classic runner's behaviour exactly where it matters for integrity:
 *
 *  • the SERVER owns the clock — the deadline comes from started_at + duration and is re-synced
 *    from every heartbeat response; at 0 the runner submits automatically;
 *  • answers autosave locally per attempt and ride on every heartbeat (12 s cadence, plus an
 *    immediate beat on each answer change, debounced), so a crash/refresh resumes losslessly via
 *    /api/me/exam/start's resume path;
 *  • secure mode: fullscreen is requested, copy/cut/paste/context-menu are blocked, and
 *    tab-switch/window-blur raise recorded integrity events (violations) sent with heartbeats —
 *    honest degradation, exactly like the classic runner;
 *  • a held result never shows a score or pass/fail.
 */

export interface ExamItem {
  id: number
  question: string
  options: string[]
  domain?: string | null
}

/** POST /api/me/exam/start response (create-or-resume). */
export interface StartPayload {
  attempt_id: number
  certification_id?: number
  duration_minutes: number
  started_at: string
  items: ExamItem[]
  saved_answers?: Record<string, number> | null
  violations?: number
  resumed?: boolean
}

interface SubmitResult {
  ok: boolean
  held: boolean
  result_status?: string
  hold_reason?: string | null
  message?: string
  score?: number
  max?: number
  pct?: number
  result?: string
  breakdown?: DomainBand[]
  credential?: string | null
  unanswered?: number
}

interface HeartbeatResponse {
  ok: boolean
  deadline?: string
  remaining_s?: number
  forceSubmit?: boolean
  messages?: { from?: string; body?: string; at?: string }[]
}

/** Backend datetimes are 'YYYY-MM-DD HH:MM:SS' (UTC) or ISO — normalise both to epoch millis. */
function utcMs(v: string): number {
  const s = String(v).trim()
  const normalized = /Z$/i.test(s) ? s : s.includes('T') ? `${s}Z` : `${s.replace(' ', 'T')}Z`
  const ms = Date.parse(normalized)
  return Number.isFinite(ms) ? ms : Date.now()
}

const HEARTBEAT_MS = 12_000
const LETTERS = 'ABCDE'

// ── pocket calculator (same safe evaluator as the classic runner) ──
function calcEval(e: string): number | null {
  const x = e.replace(/×/g, '*').replace(/÷/g, '/').replace(/−/g, '-')
  if (!/^[0-9+\-*/(). ]{0,80}$/.test(x) || !x.trim()) return null
  try {
    const v = Function('"use strict";return (' + x + ')')() as unknown
    return typeof v === 'number' && isFinite(v) ? Math.round(v * 1e10) / 1e10 : null
  } catch {
    return null
  }
}

const CALC_KEYS = ['C', '(', ')', '⌫', '7', '8', '9', '÷', '4', '5', '6', '×', '1', '2', '3', '−', '0', '.', '√', '+']

function Calculator({ onClose }: { onClose: () => void }) {
  const [expr, setExpr] = useState('')
  const value = calcEval(expr)
  const press = (k: string) => {
    if (k === 'C') setExpr('')
    else if (k === '⌫') setExpr((e) => e.slice(0, -1))
    else if (k === '√') {
      const v = calcEval(expr)
      if (v != null && v >= 0) setExpr(String(Math.round(Math.sqrt(v) * 1e10) / 1e10))
    } else if (k === '=') {
      const v = calcEval(expr)
      if (v != null) setExpr(String(v))
    } else setExpr((e) => e + k)
  }
  return (
    <div className="xr-calc" role="dialog" aria-label="Calculator">
      <div className="head">
        Calculator
        <button className="btn ghost sm" onClick={onClose} aria-label="Close calculator">×</button>
      </div>
      <div className="scr">
        <div className="e">{expr || ' '}</div>
        <div className="v">{value != null ? value : 0}</div>
      </div>
      <div className="grid">
        {CALC_KEYS.map((k) => (
          <button key={k} className={'÷×−+()√'.includes(k) ? 'op' : ''} onClick={() => press(k)}>{k}</button>
        ))}
        <button className="eq" onClick={() => press('=')}>=</button>
      </div>
    </div>
  )
}

export default function ExamRunner({ start, certLabel, onExit }: {
  start: StartPayload
  certLabel: string
  /** Called when the candidate leaves the runner; finished=true when the attempt was submitted. */
  onExit: (finished: boolean) => void
}) {
  const storageKey = `pci.exam.answers.${start.attempt_id}`
  const [idx, setIdx] = useState(0)
  const [answers, setAnswers] = useState<Record<string, number>>(() => {
    let local: Record<string, number> = {}
    try { local = JSON.parse(localStorage.getItem(storageKey) || '{}') } catch { /* fresh */ }
    return { ...(start.saved_answers || {}), ...local }
  })
  const [flags, setFlags] = useState<Record<string, boolean>>({})
  const [violations, setViolations] = useState(start.violations || 0)
  const [flash, setFlash] = useState<string | null>(null)
  const [proctorMsg, setProctorMsg] = useState<string | null>(null)
  const [remaining, setRemaining] = useState<number>(start.duration_minutes * 60)
  const [confirming, setConfirming] = useState(false)
  const [submitting, setSubmitting] = useState(false)
  const [result, setResult] = useState<SubmitResult | null>(null)
  const [finalizedElsewhere, setFinalizedElsewhere] = useState(false)
  const [showCalc, setShowCalc] = useState(false)

  // Live values the interval callbacks read without re-subscribing.
  const answersRef = useRef(answers)
  const violationsRef = useRef(violations)
  const deadlineRef = useRef(utcMs(start.started_at) + start.duration_minutes * 60_000)
  const doneRef = useRef(false)          // set once the attempt leaves in_progress (any path)
  const autoSubmittedRef = useRef(false)
  answersRef.current = answers
  violationsRef.current = violations

  const items = start.items
  const item = items[idx]
  const answered = Object.keys(answers).length

  // ── heartbeat: answers + violations up, canonical deadline + proctor messages down ──
  const beat = useCallback(async () => {
    if (doneRef.current) return
    try {
      const r = await api.post<HeartbeatResponse>('/api/me/exam/heartbeat', {
        attempt_id: start.attempt_id,
        answers: answersRef.current,
        violations: violationsRef.current,
      })
      if (r.deadline) deadlineRef.current = utcMs(r.deadline)
      const msgs = r.messages || []
      if (msgs.length) {
        setProctorMsg(msgs.map((m) => String(m.body || '').slice(0, 200)).join(' · '))
        window.setTimeout(() => setProctorMsg(null), 8000)
      }
    } catch (e) {
      // not_active: the server already finalised this attempt (hard-stop auto-timeout).
      if (e instanceof ApiError && e.body && typeof e.body === 'object' && (e.body as Record<string, unknown>).error === 'not_active') {
        doneRef.current = true
        setFinalizedElsewhere(true)
      }
      // Network blips are tolerated — answers stay in localStorage and ride the next beat.
    }
  }, [start.attempt_id])

  const beatDebounce = useRef<number | undefined>(undefined)
  const queueBeat = useCallback(() => {
    window.clearTimeout(beatDebounce.current)
    beatDebounce.current = window.setTimeout(() => { void beat() }, 1500)
  }, [beat])

  // ── submit (manual, timer-expiry, or post-violation) ──
  const submit = useCallback(async (auto: boolean) => {
    if (doneRef.current) return
    doneRef.current = true
    setConfirming(false)
    setSubmitting(true)
    try {
      const r = await api.post<SubmitResult>('/api/me/exam/submit', {
        attempt_id: start.attempt_id,
        answers: answersRef.current,
        client_kind: 'browser',
      })
      try { localStorage.removeItem(storageKey) } catch { /* ignore */ }
      setResult(r)
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' ? String((e.body as Record<string, unknown>).error ?? '') : ''
      if (code === 'already_submitted') {
        // The server finalised the sitting first (auto-timeout) — the answers it held at the
        // deadline were scored. Point the candidate at Results rather than pretending failure.
        try { localStorage.removeItem(storageKey) } catch { /* ignore */ }
        setFinalizedElsewhere(true)
      } else {
        // A transient failure must not strand the sitting: allow retrying the submit.
        doneRef.current = false
        setSubmitting(false)
        setFlash(auto ? 'Time is up but the submission failed — retrying…' : 'Could not submit — please try again.')
        window.setTimeout(() => setFlash(null), 2600)
        if (auto) window.setTimeout(() => { void submit(true) }, 4000)
        return
      }
    }
    setSubmitting(false)
  }, [start.attempt_id, storageKey])

  // ── clock: 500 ms tick against the server-synced deadline; 0 → auto-submit once ──
  useEffect(() => {
    const t = window.setInterval(() => {
      const left = Math.max(0, Math.floor((deadlineRef.current - Date.now()) / 1000))
      setRemaining(left)
      if (left <= 0 && !autoSubmittedRef.current && !doneRef.current) {
        autoSubmittedRef.current = true
        void submit(true)
      }
    }, 500)
    return () => window.clearInterval(t)
  }, [submit])

  // ── heartbeat cadence ──
  useEffect(() => {
    void beat()
    const t = window.setInterval(() => { void beat() }, HEARTBEAT_MS)
    return () => window.clearInterval(t)
  }, [beat])

  // ── secure mode: fullscreen + focus/clipboard guards → recorded integrity events ──
  useEffect(() => {
    const violate = (msg: string) => {
      if (doneRef.current) return
      setViolations((v) => v + 1)
      setFlash(`${msg} — the event has been recorded and will be reviewed.`)
      window.setTimeout(() => setFlash(null), 2600)
      window.setTimeout(() => { void beat() }, 50)
    }
    const onVis = () => { if (document.hidden) violate('You left the exam tab') }
    const onBlur = () => violate('The exam window lost focus')
    const block = (e: Event) => e.preventDefault()
    const blockAndRecord = (e: Event) => { e.preventDefault(); violate('Copy/paste is not permitted') }
    document.addEventListener('visibilitychange', onVis)
    window.addEventListener('blur', onBlur)
    document.addEventListener('contextmenu', block)
    document.addEventListener('copy', blockAndRecord)
    document.addEventListener('cut', blockAndRecord)
    document.addEventListener('paste', blockAndRecord)
    document.documentElement.requestFullscreen?.().catch(() => { /* honest degradation */ })
    document.body.style.overflow = 'hidden'
    return () => {
      document.removeEventListener('visibilitychange', onVis)
      window.removeEventListener('blur', onBlur)
      document.removeEventListener('contextmenu', block)
      document.removeEventListener('copy', blockAndRecord)
      document.removeEventListener('cut', blockAndRecord)
      document.removeEventListener('paste', blockAndRecord)
      document.body.style.overflow = ''
      if (document.fullscreenElement) document.exitFullscreen?.().catch(() => { /* ignore */ })
    }
  }, [beat])

  const pick = (optionIdx: number) => {
    if (doneRef.current) return
    const next = { ...answersRef.current, [String(item.id)]: optionIdx }
    setAnswers(next)
    try { localStorage.setItem(storageKey, JSON.stringify(next)) } catch { /* memory only */ }
    queueBeat()
  }

  const timeStr = useMemo(() => {
    const h = Math.floor(remaining / 3600)
    const m = Math.floor((remaining % 3600) / 60)
    const s = remaining % 60
    const mm = String(m).padStart(2, '0')
    const ss = String(s).padStart(2, '0')
    return h > 0 ? `${h}:${mm}:${ss}` : `${mm}:${ss}`
  }, [remaining])

  // The runner mounts inside Layout's animated route container (main.content.route-fade), whose
  // filling transform animation makes it the containing block for position:fixed descendants —
  // trapping and clipping the overlay. Portal to document.body like the app's other full-screen
  // surfaces (simlab, DocumentViewer) so the overlay genuinely covers the viewport.
  const portal = (node: ReactNode) => createPortal(node, document.body)

  // ── terminal screens ──
  if (finalizedElsewhere) {
    return portal(
      <div className="xr-overlay">
        <div className="xr-top"><span className="xr-title">Examination submitted</span></div>
        <div className="card" style={{ maxWidth: 560, margin: '2rem auto', textAlign: 'center', padding: '2rem' }}>
          <h3>Your sitting has been finalised</h3>
          <p className="muted">
            Time expired and the examination was submitted automatically with the answers saved up
            to the deadline. Your result is available on the Results page.
          </p>
          <button className="btn" style={{ marginTop: '1rem' }} onClick={() => onExit(true)}>View my result</button>
        </div>
      </div>
    )
  }

  if (result) {
    if (result.held) {
      return portal(
        <div className="xr-overlay">
          <div className="xr-top"><span className="xr-title">Examination result</span></div>
          <div className="card" style={{ maxWidth: 640, margin: '2rem auto', textAlign: 'center', padding: '2.2rem 1.8rem' }}>
            <div style={{ fontSize: '2rem' }}>⏳</div>
            <h3 style={{ color: 'var(--warn)', margin: '.5rem 0' }}>Result on hold — under review</h3>
            <p className="muted" style={{ maxWidth: '48ch', margin: '0 auto' }}>
              {result.message || 'Your submission is being verified. PCI will notify you once the review is complete.'}
            </p>
            <p className="muted small" style={{ marginTop: '.8rem' }}>
              No pass/fail outcome is shown until the review is resolved. You do not need to do anything.
            </p>
            <button className="btn" style={{ marginTop: '1.2rem' }} onClick={() => onExit(true)}>Back to my dashboard</button>
          </div>
        </div>
      )
    }
    const pass = result.result === 'pass'
    return portal(
      <div className="xr-overlay">
        <div className="xr-top"><span className="xr-title">Examination result</span></div>
        <div className="card" style={{ maxWidth: 640, margin: '2rem auto', textAlign: 'center', padding: '2.2rem 1.8rem' }}>
          <div className="small" style={{ letterSpacing: 2, textTransform: 'uppercase', color: 'var(--brand)', fontWeight: 700, marginBottom: '1rem' }}>
            Official result
          </div>
          <Dial pct={result.pct ?? 0} />
          <h2 style={{ color: pass ? 'var(--ok)' : 'var(--err)', margin: '.8rem 0 .2rem' }}>
            {pass ? 'PASS' : 'Not passed'}
          </h2>
          <p className="muted" style={{ margin: 0 }}>{result.score} of {result.max} scored items correct</p>
          {pass && result.credential && (
            <div className="notice" style={{ marginTop: '.9rem' }}>
              Credential issued: <strong>{result.credential}</strong>
            </div>
          )}
          <div style={{ textAlign: 'start', maxWidth: 460, margin: '1.4rem auto 0' }}>
            <DomainBands breakdown={parseBreakdown(result.breakdown)} />
          </div>
          <div className="row" style={{ justifyContent: 'center', gap: '.6rem', marginTop: '1.5rem', flexWrap: 'wrap' }}>
            {pass && <Link className="btn" to="/credentials" onClick={() => onExit(true)}>View my credential</Link>}
            <button className={pass ? 'btn secondary' : 'btn'} onClick={() => onExit(true)}>
              {pass ? 'Close' : 'View full result'}
            </button>
          </div>
        </div>
      </div>
    )
  }

  // ── the live sitting ──
  return portal(
    <div className="xr-overlay">
      <div className="xr-top">
        <span className="xr-title">{certLabel} — Examination</span>
        <span className={`xr-chip${violations ? ' v' : ''}`} title="Focus, clipboard and tab changes are recorded">
          {violations ? `⚠ ${violations} integrity event${violations > 1 ? 's' : ''} recorded` : 'Secure mode'}
        </span>
        <button className="btn ghost sm" onClick={() => setShowCalc((v) => !v)}>Calculator</button>
        <span className={`xr-timer${remaining < 300 ? ' low' : ''}`} aria-live="off">{timeStr}</span>
      </div>

      {proctorMsg && (
        <div className="notice" style={{ margin: '0.6rem 1rem 0' }} role="status">
          <strong>Proctor:</strong> {proctorMsg}
        </div>
      )}

      <div className="xr-body">
        <div className="card" style={{ padding: '1.2rem' }}>
          <div className="muted small">Question {idx + 1} of {items.length}</div>
          {item.domain && <div className="muted small" style={{ marginTop: '.15rem' }}>{item.domain}</div>}
          <h3 style={{ margin: '.6rem 0 .4rem' }}>{item.question}</h3>
          {item.options.map((o, i) => (
            <div
              key={i}
              role="button"
              tabIndex={0}
              className={`xr-opt${answers[String(item.id)] === i ? ' sel' : ''}`}
              onClick={() => pick(i)}
              onKeyDown={(e) => { if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); pick(i) } }}
            >
              <span className="k">{LETTERS[i]}</span>
              <div>{o}</div>
            </div>
          ))}
          <div className="row" style={{ marginTop: '1rem', gap: '.5rem', flexWrap: 'wrap' }}>
            <button className="btn secondary sm" disabled={idx === 0} onClick={() => setIdx((i) => i - 1)}>Previous</button>
            <button
              className="btn secondary sm"
              onClick={() => setFlags((f) => ({ ...f, [String(item.id)]: !f[String(item.id)] }))}
            >
              {flags[String(item.id)] ? 'Unflag' : 'Flag for review'}
            </button>
            <div style={{ flex: 1 }} />
            {idx < items.length - 1 ? (
              <button className="btn sm" onClick={() => setIdx((i) => i + 1)}>Next</button>
            ) : (
              <button className="btn sm" disabled={submitting} onClick={() => setConfirming(true)}>Submit examination</button>
            )}
          </div>
        </div>

        <div className="card" style={{ padding: '1rem' }}>
          <h4 style={{ marginBottom: '.6rem' }}>Questions</h4>
          <div className="xr-pal">
            {items.map((q, i) => (
              <button
                key={q.id}
                className={[
                  answers[String(q.id)] != null ? 'a' : '',
                  flags[String(q.id)] ? 'f' : '',
                  i === idx ? 'c' : '',
                ].join(' ').trim()}
                onClick={() => setIdx(i)}
              >
                {i + 1}
              </button>
            ))}
          </div>
          <p className="muted small" style={{ marginTop: '.7rem', marginBottom: 0 }}>
            {answered} of {items.length} answered
            {Object.values(flags).filter(Boolean).length > 0 && <> · {Object.values(flags).filter(Boolean).length} flagged</>}
          </p>
          <button className="btn block" style={{ marginTop: '.8rem' }} disabled={submitting} onClick={() => setConfirming(true)}>
            Submit examination
          </button>
        </div>
      </div>

      {flash && (
        <div className="xr-flash">
          <div className="card" style={{ padding: '1.2rem 1.4rem' }}>
            <b style={{ color: 'var(--err)' }}>Secure exam notice</b>
            <p className="small" style={{ margin: '.5rem 0 0' }}>{flash}</p>
          </div>
        </div>
      )}

      {confirming && (
        <div className="xr-modal" role="dialog" aria-modal="true" aria-label="Confirm submission">
          <div className="card" style={{ maxWidth: 440, padding: '1.4rem' }}>
            <h3>Submit your examination?</h3>
            <p className="muted" style={{ margin: '.5rem 0' }}>
              {items.length - answered > 0
                ? `${items.length - answered} question${items.length - answered > 1 ? 's are' : ' is'} unanswered and will be marked incorrect.`
                : 'All questions are answered.'}
            </p>
            <p className="muted small">This is final — you cannot reopen the examination after submitting.</p>
            <div className="row" style={{ justifyContent: 'flex-end', gap: '.6rem', marginTop: '1rem' }}>
              <button className="btn secondary sm" onClick={() => setConfirming(false)}>Keep working</button>
              <button className="btn sm" disabled={submitting} onClick={() => { void submit(false) }}>
                {submitting ? 'Submitting…' : 'Submit now'}
              </button>
            </div>
          </div>
        </div>
      )}

      {showCalc && <Calculator onClose={() => setShowCalc(false)} />}
    </div>
  )
}
