import { useCallback, useEffect, useState } from 'react'
import { useParams, Link } from 'react-router-dom'
import { api, ApiError } from '../api/client'
import { Card, Badge, Spinner, ErrorNote } from '../components/ui'
import SCurve, { type EvmPoint } from '../components/SCurve'
import Gantt, { type GanttBar } from '../components/Gantt'
import Histogram, { type Mc } from '../components/Histogram'

// PCI AI Project Controls Simulation Lab — interactive workspace (Phase 1 foundation).
// Starts (or resumes) an attempt at a guided lab, presents the synthetic task and its inputs, collects the
// student's computed measures, and shows the deterministic grade. The correct answers are computed and
// checked entirely on the server; in Assessment Mode the graded feedback deliberately withholds them.

interface Ask { key: string; label: string; type: string }
interface Task { task: string; prompt: string; given: Record<string, unknown>; ask: Ask[]; mode: string; assessment: boolean; montecarlo?: Mc | null }
interface ScenarioMeta { id: number; scenario_code: string; title: string; kind: string; difficulty?: string; summary?: string }
interface StartResp { attempt_id: number; resumed: boolean; scenario: ScenarioMeta; task: Task }
interface Measure { key: string; label: string; is_correct: boolean; correct_value: unknown; your_value: unknown }
interface Competency { competency: string; score: number; level: string }
interface Schedule { project_duration: number; critical_path: string[]; bars: GanttBar[] }
interface Grade { score: number; passed: boolean; correct: number; total: number; mode: string; assessment: boolean; measures: Measure[]; competencies: Competency[]; schedule?: Schedule | null }

type Mode = 'training' | 'assessment'

const titleCase = (s: string) => s.replace(/[_-]+/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())
const fmt = (v: unknown): string => {
  if (v === null || v === undefined) return '—'
  if (Array.isArray(v)) return v.join(', ')
  if (typeof v === 'boolean') return v ? 'Yes' : 'No'
  if (typeof v === 'number') return Number.isInteger(v) ? String(v) : String(Number(v.toFixed(2)))
  return String(v)
}

function mapError(e: unknown): string {
  if (e instanceof ApiError) {
    const body = e.body as { error?: string; message?: string } | undefined
    if (body?.message) return body.message
    if (body?.error === 'no_access') return 'This lab needs an active PCI membership or a paid exam enrolment.'
    if (body?.error === 'not_found') return 'This lab could not be found.'
    if (body?.error === 'not_interactive') return 'This lab is not interactive yet.'
    if (body?.error === 'rate_limited') return 'You are moving through labs quickly — take a short break and try again.'
  }
  return 'Something went wrong opening this lab. Please try again.'
}

// ── Given-data renderers (one per engine) ─────────────────────────────────────────────────────────
function GivenView({ task }: { task: Task }) {
  const g = task.given
  if (task.task === 'evm') {
    const labels: Record<string, string> = { pv: 'Planned Value (PV)', ev: 'Earned Value (EV)', ac: 'Actual Cost (AC)', bac: 'Budget at Completion (BAC)' }
    return (
      <table className="data">
        <tbody>
          {Object.keys(labels).filter((k) => k in g).map((k) => (
            <tr key={k}><th style={{ textAlign: 'left' }}>{labels[k]}</th><td>{fmt(g[k])}</td></tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'cpm') {
    const acts = (g.activities as { id: string; dur: number; preds: string[] }[]) ?? []
    return (
      <table className="data">
        <thead><tr><th>Activity</th><th>Duration (days)</th><th>Predecessors</th></tr></thead>
        <tbody>
          {acts.map((a) => (
            <tr key={a.id}><td>{a.id}</td><td>{a.dur}</td><td>{a.preds.length ? a.preds.join(', ') : '—'}</td></tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'change') {
    const changes = (g.changes as { id: string; title?: string; status: string; cost_delta: number; schedule_delta: number }[]) ?? []
    return (
      <>
        <div className="small muted" style={{ marginBottom: '.3rem' }}>
          Baseline: BAC {fmt(g.baseline_bac as number)} · {fmt(g.baseline_duration as number)} days
        </div>
        <table className="data">
          <thead><tr><th>Change</th><th>Description</th><th>Status</th><th>Cost Δ</th><th>Schedule Δ</th></tr></thead>
          <tbody>
            {changes.map((ch) => (
              <tr key={ch.id}>
                <td>{ch.id}</td><td>{ch.title ?? '—'}</td>
                <td><Badge tone={ch.status === 'approved' ? 'ok' : ch.status === 'rejected' ? 'warn' : 'neutral'}>{titleCase(ch.status)}</Badge></td>
                <td>{fmt(ch.cost_delta)}</td><td>{fmt(ch.schedule_delta)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </>
    )
  }
  if (task.task === 'cashflow') {
    const periods = (g.periods as { period: number; inflow: number; outflow: number }[]) ?? []
    return (
      <table className="data">
        <thead><tr><th>Period</th><th>Inflow</th><th>Outflow</th><th>Net</th></tr></thead>
        <tbody>
          {periods.map((p) => (
            <tr key={p.period}><td>{p.period}</td><td>{fmt(p.inflow)}</td><td>{fmt(p.outflow)}</td><td>{fmt(p.inflow - p.outflow)}</td></tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'timeline') {
    const series = (g.series as { period: number; pv: number; ev: number; ac: number }[]) ?? []
    return (
      <>
        {g.bac != null && <div className="small muted" style={{ marginBottom: '.3rem' }}>Budget at Completion (BAC): {fmt(g.bac as number)}</div>}
        <table className="data">
          <thead><tr><th>Period</th><th>Planned Value</th><th>Earned Value</th><th>Actual Cost</th></tr></thead>
          <tbody>
            {series.map((p) => (
              <tr key={p.period}><td>{p.period}</td><td>{fmt(p.pv)}</td><td>{fmt(p.ev)}</td><td>{fmt(p.ac)}</td></tr>
            ))}
          </tbody>
        </table>
      </>
    )
  }
  if (task.task === 'earned_schedule') {
    const plan = (g.plan as { period: number; pv: number }[]) ?? []
    return (
      <>
        <div className="small muted" style={{ marginBottom: '.3rem' }}>
          Status at month {fmt(g.at as number)}: Earned Value {fmt(g.ev as number)} · planned duration {fmt(g.planned_duration as number)} months
        </div>
        <table className="data">
          <thead><tr><th>Month</th><th>Cumulative Planned Value</th></tr></thead>
          <tbody>
            {plan.map((p) => (
              <tr key={p.period}><td>{p.period}</td><td>{fmt(p.pv)}</td></tr>
            ))}
          </tbody>
        </table>
      </>
    )
  }
  if (task.task === 'risk') {
    const risks = (g.risks as { id: string; name?: string; probability: number; impact: number }[]) ?? []
    return (
      <table className="data">
        <thead><tr><th>Risk</th><th>Description</th><th>Probability</th><th>Impact</th></tr></thead>
        <tbody>
          {risks.map((r) => (
            <tr key={r.id}>
              <td>{r.id}</td>
              <td>{r.name ?? '—'}</td>
              <td>{fmt(r.probability)}</td>
              <td>{fmt(r.impact)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'pert') {
    const acts = (g.activities as { id: string; o: number; m: number; p: number }[]) ?? []
    const deadline = g.deadline as number | undefined
    return (
      <>
        <table className="data">
          <thead><tr><th>Activity</th><th>Optimistic</th><th>Most likely</th><th>Pessimistic</th></tr></thead>
          <tbody>
            {acts.map((a) => (
              <tr key={a.id}><td>{a.id}</td><td>{a.o}</td><td>{a.m}</td><td>{a.p}</td></tr>
            ))}
          </tbody>
        </table>
        {deadline != null && <div className="small muted" style={{ marginTop: '.3rem' }}>Deadline: day {fmt(deadline)}</div>}
      </>
    )
  }
  if (task.task === 'progress') {
    const nodes = (g.nodes as { id: string; name?: string; weight?: number; percent?: number }[]) ?? []
    return (
      <table className="data">
        <thead><tr><th>Package</th><th>Work</th><th>Weight</th><th>% complete</th></tr></thead>
        <tbody>
          {nodes.map((n) => (
            <tr key={n.id}>
              <td>{n.id}</td>
              <td>{n.name ?? '—'}</td>
              <td>{n.weight != null ? fmt(n.weight) : '—'}</td>
              <td>{n.percent != null ? `${fmt(n.percent)}%` : '—'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'cbs') {
    const nodes = (g.nodes as { id: string; parent: string | null; name?: string; budget?: number; actual?: number }[]) ?? []
    return (
      <table className="data">
        <thead><tr><th>Account</th><th>Element</th><th>Budget</th><th>Actual</th></tr></thead>
        <tbody>
          {nodes.map((n) => (
            <tr key={n.id}>
              <td style={{ paddingLeft: `${(n.id.split('.').length - 1) * 1.2}rem` }}>{n.id}</td>
              <td>{n.name ?? '—'}</td>
              <td>{n.budget != null ? fmt(n.budget) : '—'}</td>
              <td>{n.actual != null ? fmt(n.actual) : '—'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'wbs') {
    const nodes = (g.nodes as { id: string; parent: string | null; name?: string; value?: number }[]) ?? []
    return (
      <table className="data">
        <thead><tr><th>WBS</th><th>Element</th><th>Leaf budget</th></tr></thead>
        <tbody>
          {nodes.map((n) => (
            <tr key={n.id}>
              <td style={{ paddingLeft: `${(n.id.split('.').length - 1) * 1.2}rem` }}>{n.id}</td>
              <td>{n.name ?? '—'}</td>
              <td>{n.value != null ? fmt(n.value) : '—'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    )
  }
  return <pre className="small muted" style={{ whiteSpace: 'pre-wrap' }}>{JSON.stringify(g, null, 2)}</pre>
}

export default function LabRunner() {
  const { code } = useParams<{ code: string }>()
  const [mode, setMode] = useState<Mode>('training')
  const [start, setStart] = useState<StartResp | null>(null)
  const [answers, setAnswers] = useState<Record<string, string>>({})
  const [grade, setGrade] = useState<Grade | null>(null)
  const [coach, setCoach] = useState<{ ok: boolean; message: string; ai: boolean } | null>(null)
  const [coaching, setCoaching] = useState(false)
  const [loading, setLoading] = useState(true)
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const begin = useCallback(async (m: Mode) => {
    setLoading(true); setError(null); setGrade(null); setAnswers({}); setCoach(null)
    try {
      const s = await api.post<StartResp>('/api/me/lab/attempts', { scenario_code: code, mode: m })
      setStart(s)
    } catch (e) {
      setError(mapError(e)); setStart(null)
    } finally {
      setLoading(false)
    }
  }, [code])

  useEffect(() => { begin(mode) }, [begin, mode])

  const submit = async () => {
    if (!start) return
    setBusy(true); setError(null)
    try {
      const payload: Record<string, unknown> = {}
      for (const a of start.task.ask) {
        const raw = (answers[a.key] ?? '').trim()
        if (a.type === 'set') payload[a.key] = raw.split(/[,\s]+/).filter(Boolean)
        else if (a.type === 'bool') payload[a.key] = /^(y|yes|true|valid)$/i.test(raw) ? true : /^(n|no|false|invalid)$/i.test(raw) ? false : null
        else payload[a.key] = raw === '' ? null : Number(raw)
      }
      const g = await api.post<Grade>(`/api/me/lab/attempts/${start.attempt_id}/submit`, { answers: payload })
      setGrade(g)
    } catch (e) {
      setError(mapError(e))
    } finally {
      setBusy(false)
    }
  }

  const askCoach = async () => {
    if (!start) return
    setCoaching(true)
    try {
      const r = await api.post<{ ok: boolean; message: string; ai: boolean }>(`/api/me/lab/attempts/${start.attempt_id}/coach`, {})
      setCoach(r)
    } catch {
      setCoach({ ok: false, message: 'The coach is unavailable right now — please try again shortly.', ai: false })
    } finally {
      setCoaching(false)
    }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="row" style={{ justifyContent: 'space-between', alignItems: 'flex-start', gap: '1rem', flexWrap: 'wrap' }}>
        <div>
          <Link to="/lab" className="small muted">← Back to the Practice Lab</Link>
          <h1 style={{ margin: '.2rem 0 0' }}>{start?.scenario.title ?? code}</h1>
          {start && (
            <div className="row small muted" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
              <Badge tone="neutral">{titleCase(start.scenario.kind)}</Badge>
              {start.scenario.difficulty && <Badge tone="neutral">{titleCase(start.scenario.difficulty)}</Badge>}
              <span>{start.scenario.scenario_code}</span>
            </div>
          )}
        </div>
        <label className="small" style={{ display: 'grid', gap: '.2rem' }}>
          <span className="muted">Mode</span>
          <select value={mode} onChange={(e) => setMode(e.target.value as Mode)} disabled={busy}>
            <option value="training">Training — shows the worked answers</option>
            <option value="assessment">Assessment — marks only, answers withheld</option>
          </select>
        </label>
      </div>

      {loading ? (
        <Spinner />
      ) : error && !start ? (
        <Card><ErrorNote>{error}</ErrorNote></Card>
      ) : start ? (
        <>
          <Card title="Brief">
            <p style={{ marginTop: 0 }}>{start.task.prompt}</p>
            {Array.isArray(start.task.given.series) && (start.task.given.series as EvmPoint[]).length > 0 && (
              <div style={{ margin: '.2rem 0 .8rem' }}><SCurve series={start.task.given.series as EvmPoint[]} /></div>
            )}
            <div style={{ overflowX: 'auto' }}><GivenView task={start.task} /></div>
            {start.task.montecarlo && (
              <div style={{ margin: '.8rem 0 .2rem' }}><Histogram mc={start.task.montecarlo} /></div>
            )}
          </Card>

          {!grade ? (
            <Card title="Your answers" action={start.task.assessment ? <Badge tone="warn">Assessment</Badge> : undefined}>
              <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
                {start.task.ask.map((a) => (
                  <label key={a.key} className="row" style={{ justifyContent: 'space-between', gap: '1rem', alignItems: 'center' }}>
                    <span>{a.label}</span>
                    <input
                      style={{ maxWidth: '14rem' }}
                      inputMode={a.type === 'number' ? 'decimal' : 'text'}
                      placeholder={a.type === 'set' ? 'e.g. A, B, D' : a.type === 'bool' ? 'yes / no' : 'number'}
                      value={answers[a.key] ?? ''}
                      onChange={(e) => setAnswers((s) => ({ ...s, [a.key]: e.target.value }))}
                    />
                  </label>
                ))}
                {error && <ErrorNote>{error}</ErrorNote>}
                <div className="row" style={{ gap: '.5rem' }}>
                  <button className="btn" onClick={submit} disabled={busy}>{busy ? 'Grading…' : 'Submit for grading'}</button>
                </div>
              </div>
            </Card>
          ) : (
            <Card
              title={`Result — ${grade.score}%`}
              action={<Badge tone={grade.passed ? 'ok' : 'warn'}>{grade.passed ? 'Passed' : 'Keep practising'}</Badge>}
            >
              <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
                <p className="muted" style={{ marginTop: 0 }}>{grade.correct} of {grade.total} correct.</p>
                <table className="data">
                  <thead>
                    <tr><th>Measure</th><th></th><th>Your answer</th>{!grade.assessment && <th>Correct</th>}</tr>
                  </thead>
                  <tbody>
                    {grade.measures.map((m) => (
                      <tr key={m.key}>
                        <td>{m.label}</td>
                        <td>{m.is_correct ? <Badge tone="ok">✓</Badge> : <Badge tone="warn">✗</Badge>}</td>
                        <td>{grade.assessment ? '—' : fmt(m.your_value)}</td>
                        {!grade.assessment && <td>{fmt(m.correct_value)}</td>}
                      </tr>
                    ))}
                  </tbody>
                </table>
                {grade.assessment && (
                  <p className="small muted" style={{ margin: 0 }}>
                    Assessment Mode reports your marks only — the worked answers are withheld. Switch to Training
                    Mode to see the full solution.
                  </p>
                )}
                {grade.schedule?.bars && grade.schedule.bars.length > 0 && (
                  <div>
                    <div className="small muted" style={{ marginBottom: '.2rem' }}>
                      Computed schedule — critical path {grade.schedule.critical_path.join(' → ')}, duration {grade.schedule.project_duration}
                    </div>
                    <div style={{ overflowX: 'auto' }}>
                      <Gantt bars={grade.schedule.bars} projectDuration={grade.schedule.project_duration} />
                    </div>
                  </div>
                )}
                {grade.competencies.length > 0 && (
                  <div className="row" style={{ flexWrap: 'wrap', gap: '.3rem', alignItems: 'center' }}>
                    <span className="small muted">Competency evidence:</span>
                    {grade.competencies.map((c) => (
                      <Badge key={c.competency} tone="brand">{titleCase(c.competency)} · {titleCase(c.level)}</Badge>
                    ))}
                  </div>
                )}
                {!grade.assessment && (
                  coach ? (
                    <div style={{ background: 'var(--wash, #f6f8fb)', borderRadius: '.5rem', padding: '.7rem .8rem' }}>
                      <div className="row" style={{ gap: '.4rem', alignItems: 'center', marginBottom: '.3rem' }}>
                        <strong>Coach</strong>
                        <Badge tone={coach.ai ? 'brand' : 'neutral'}>{coach.ai ? 'AI' : 'Guide'}</Badge>
                      </div>
                      <div style={{ whiteSpace: 'pre-wrap' }}>{coach.message}</div>
                    </div>
                  ) : (
                    <div>
                      <button className="btn secondary sm" onClick={askCoach} disabled={coaching}>
                        {coaching ? 'Asking the coach…' : 'Ask the coach'}
                      </button>
                    </div>
                  )
                )}
                <div className="row" style={{ gap: '.5rem' }}>
                  <button className="btn secondary" onClick={() => begin(mode)}>Try again</button>
                  <Link className="btn sm" to="/lab">Back to labs</Link>
                </div>
              </div>
            </Card>
          )}

          <p className="muted small" style={{ margin: 0 }}>
            Educational simulator using synthetic project data. Practice never affects your formal PCI
            examination or certification.
          </p>
        </>
      ) : null}
    </div>
  )
}
