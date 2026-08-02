import { useCallback, useEffect, useRef, useState } from 'react'
import { useParams, useSearchParams, Link } from 'react-router-dom'
import { api, ApiError } from '../api/client'
import { useI18n } from '../i18n'
import { fmtDateTime } from '../format'
import { openPrintable } from '../print'
import { Card, Badge, Spinner, ErrorNote } from '../components/ui'
import {
  SegmentedControl, SaveIndicator, ConfirmDialog, InsightCallout, LevelDots, PanelSub, Chip,
  type SaveState,
} from '../components/simlab'
import SCurve, { type EvmPoint } from '../components/SCurve'
import Gantt, { type GanttBar } from '../components/Gantt'
import Histogram, { type Mc } from '../components/Histogram'
import CashflowChart from '../components/CashflowChart'
import RiskMatrix from '../components/RiskMatrix'
import ResourceChart from '../components/ResourceChart'

// PCI AI Project Controls Simulation Lab — interactive workspace.
// Starts (or resumes) an attempt, presents the synthetic task and its evidence, collects the
// student's computed measures, and shows the deterministic grade. Correct answers are computed and
// checked entirely on the server; in Assessment Mode the graded feedback deliberately withholds them.
// The premium layer adds the workspace shell (header + analytical canvas + Coach rail), a persistent
// save indicator, and a confirmation before a mode switch discards the visible working state.

interface Ask { key: string; label: string; type: string }
// Multi-step (P1): a scenario can be a linked sequence of steps; a step may carry a decision whose chosen
// option applies deterministic effects to a LATER step's given. Effects are visible scenario mechanics
// (the consequence of a choice), never the answer key — every answer is still graded server-side.
interface StepEffect { step: string; path: string; op: string; value: number }
interface StepOption { value: string; label: string; effects: StepEffect[] }
interface StepDecision { key: string; prompt: string; options: StepOption[] }
interface StepDef { id: string; title: string; task: string; prompt: string; given: Record<string, unknown>; ask: Ask[]; decision?: StepDecision | null }
interface Task {
  task?: string; prompt: string; given?: Record<string, unknown>; ask?: Ask[]; mode: string; assessment: boolean; montecarlo?: Mc | null
  multistep?: boolean; steps?: StepDef[]
}
interface ScenarioMeta { id: number; scenario_code: string; title: string; kind: string; difficulty?: string; summary?: string }
/** Free Templates Library entry; `engine` is the Lab task this working sheet serves ("" = general artefact). */
interface TemplateRow { doc_group: string; title: string; engine: string; description: string; url: string }
interface StartResp {
  attempt_id: number
  resumed: boolean
  period?: number
  answers?: Record<string, unknown> | null
  scenario: ScenarioMeta
  task: Task
}
interface Measure { key: string; label: string; is_correct: boolean; correct_value: unknown; your_value: unknown }
interface Competency { competency: string; score: number; level: string }
interface Schedule { project_duration: number; critical_path: string[]; bars: GanttBar[] }
interface Grade { score: number; passed: boolean; correct: number; total: number; mode: string; assessment: boolean; measures: Measure[]; competencies: Competency[]; schedule?: Schedule | null }
interface CoachResp { ok: boolean; message: string; ai: boolean; coach_mode?: string; hint_level?: number }
interface AttemptDetail {
  attempt_id: number
  status: string
  mode: string
  answers?: Record<string, unknown> | null
  scenario: ScenarioMeta
  task: Task
  score?: number | null
  started_at?: string | null
  completed_at?: string | null
  grade?: Grade | null
}

type Mode = 'training' | 'assessment'

// Per-engine method reference shown in the workspace rail. Generic course knowledge
// only — definitions and formulas, never a scenario number or an answer.
const REFERENCE: Record<string, { term: string; formula: string }[]> = {
  evm: [
    { term: 'Schedule Variance', formula: 'SV = EV − PV' },
    { term: 'Cost Variance', formula: 'CV = EV − AC' },
    { term: 'Schedule Performance Index', formula: 'SPI = EV ÷ PV' },
    { term: 'Cost Performance Index', formula: 'CPI = EV ÷ AC' },
    { term: 'Estimate at Completion (CPI basis)', formula: 'EAC = BAC ÷ CPI' },
    { term: 'Estimate to Complete', formula: 'ETC = EAC − AC' },
    { term: 'Variance at Completion', formula: 'VAC = BAC − EAC' },
    { term: 'To-Complete Performance Index', formula: 'TCPI = (BAC − EV) ÷ (BAC − AC)' },
    { term: 'Percent complete', formula: '% complete = EV ÷ BAC' },
  ],
  earned_schedule: [
    { term: 'Earned Schedule', formula: 'ES = time at which cumulative PV equals current EV (interpolate between periods)' },
    { term: 'Schedule Variance (time)', formula: 'SV(t) = ES − AT' },
    { term: 'Schedule Performance Index (time)', formula: 'SPI(t) = ES ÷ AT' },
    { term: 'Forecast duration', formula: 'planned duration ÷ SPI(t)' },
  ],
  cpm: [
    { term: 'Forward pass', formula: 'ES = max(EF of predecessors) · EF = ES + duration' },
    { term: 'Backward pass', formula: 'LF = min(LS of successors) · LS = LF − duration' },
    { term: 'Total float', formula: 'TF = LS − ES = LF − EF' },
    { term: 'Critical path', formula: 'the longest path — activities with zero total float' },
  ],
  pert: [
    { term: 'Expected duration', formula: 'tₑ = (O + 4M + P) ÷ 6' },
    { term: 'Standard deviation', formula: 'σ = (P − O) ÷ 6' },
    { term: 'Path deviation', formula: 'σ(path) = √(Σ σ²) along the path' },
    { term: 'Confidence', formula: 'Z = (deadline − tₑ) ÷ σ(path)' },
  ],
  risk: [
    { term: 'Risk exposure', formula: 'exposure = probability × impact' },
    { term: 'Ranking', formula: 'order the register by exposure, highest first' },
  ],
  cashflow: [
    { term: 'Net cash flow', formula: 'net = inflow − outflow (per period)' },
    { term: 'Cumulative position', formula: 'running total of net cash flow' },
    { term: 'Peak funding exposure', formula: 'the most negative cumulative position' },
  ],
  timeline: [
    { term: 'Schedule Variance', formula: 'SV = EV − PV' },
    { term: 'Cost Variance', formula: 'CV = EV − AC' },
    { term: 'Schedule Performance Index', formula: 'SPI = EV ÷ PV' },
    { term: 'Cost Performance Index', formula: 'CPI = EV ÷ AC' },
    { term: 'Estimate at Completion (CPI basis)', formula: 'EAC = BAC ÷ CPI' },
  ],
  progress: [
    { term: 'Weighted progress', formula: '% complete = Σ(weight × %) ÷ Σ(weight)' },
  ],
  productivity: [
    { term: 'Productivity factor', formula: 'PF = earned ÷ actual (quantities or hours)' },
    { term: 'Unit rate', formula: 'rate = hours ÷ quantity' },
  ],
  boq: [
    { term: 'Line amount', formula: 'amount = quantity × rate' },
    { term: 'Bill total', formula: 'Σ line amounts' },
  ],
  resource: [
    { term: 'Overload', formula: 'overload = max(0, demand − capacity) per period' },
  ],
  wbs: [
    { term: 'Roll-up', formula: 'parent value = Σ of its children · only leaves carry budget' },
  ],
  cbs: [
    { term: 'Roll-up', formula: 'parent budget/actual = Σ of its children' },
    { term: 'Variance', formula: 'variance = budget − actual' },
  ],
  change: [
    { term: 'Revised budget', formula: 'revised BAC = baseline BAC + Σ approved cost Δ' },
    { term: 'Revised duration', formula: 'revised duration = baseline + Σ approved schedule Δ' },
    { term: 'Scope discipline', formula: 'only approved changes move the baseline' },
  ],
  portfolio: [
    { term: 'Weighted score', formula: 'score = w₁×NPV′ + w₂×fit′ − w₃×risk′ (normalised criteria, given weights)' },
  ],
  decision: [
    { term: 'Weighted score', formula: 'score per option = Σ(weight × criterion), using the given weights' },
  ],
  procurement: [
    { term: 'Schedule impact', formula: 'impact = max(0, supplier delay − remaining float)' },
  ],
  data_quality: [
    { term: 'Anomaly test', formula: 'flag when |value − expected| > threshold, or the value is missing' },
  ],
}

const titleCase = (s: string) => s.replace(/[_-]+/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())
const fmt = (v: unknown): string => {
  if (v === null || v === undefined) return '—'
  if (Array.isArray(v)) return v.join(', ')
  if (typeof v === 'boolean') return v ? 'Yes' : 'No'
  if (typeof v === 'number') return Number.isInteger(v) ? String(v) : String(Number(v.toFixed(2)))
  return String(v)
}

/** Map server-saved answers back into the string-keyed form fields. */
function hydrateAnswers(ask: Ask[], saved: Record<string, unknown> | null | undefined): Record<string, string> {
  const out: Record<string, string> = {}
  if (!saved) return out
  for (const a of ask) {
    const v = saved[a.key]
    if (v == null) continue
    if (a.type === 'set' && Array.isArray(v)) out[a.key] = v.map(String).join(', ')
    else if (a.type === 'bool') out[a.key] = v === true || v === 'true' ? 'yes' : v === false || v === 'false' ? 'no' : String(v)
    else out[a.key] = String(v)
  }
  return out
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
// Apply the effects of the learner's recorded decisions to a step's given — the client-side mirror of the
// server's SimStep.EffectiveGiven, so the learner sees the numbers their choice produces before computing.
function effectiveGiven(step: StepDef, steps: StepDef[], decisions: Record<string, string>): Record<string, unknown> {
  const g: Record<string, unknown> = { ...step.given }
  for (const s of steps) {
    if (!s.decision) continue
    const chosen = decisions[s.decision.key]
    if (!chosen) continue
    const opt = s.decision.options.find((o) => o.value === chosen)
    if (!opt) continue
    for (const e of opt.effects) {
      if (e.step !== step.id) continue
      const cur = typeof g[e.path] === 'number' ? (g[e.path] as number) : Number(g[e.path]) || 0
      g[e.path] = e.op === 'add' ? cur + e.value : e.op === 'mul' ? cur * e.value : e.value
    }
  }
  return g
}

function GivenView({ task }: { task: Task }) {
  const g = task.given ?? {}
  if (task.task === 'evm') {
    const labels: Record<string, string> = { pv: 'Planned Value (PV)', ev: 'Earned Value (EV)', ac: 'Actual Cost (AC)', bac: 'Budget at Completion (BAC)' }
    return (
      <table className="data">
        <tbody>
          {Object.keys(labels).filter((k) => k in g).map((k) => (
            <tr key={k}><th style={{ textAlign: 'start' }}>{labels[k]}</th><td>{fmt(g[k])}</td></tr>
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
      <>
        <div style={{ margin: '.2rem 0 .8rem' }}><CashflowChart periods={periods} /></div>
        <table className="data">
          <thead><tr><th>Period</th><th>Inflow</th><th>Outflow</th><th>Net</th></tr></thead>
          <tbody>
            {periods.map((p) => (
              <tr key={p.period}><td>{p.period}</td><td>{fmt(p.inflow)}</td><td>{fmt(p.outflow)}</td><td>{fmt(p.inflow - p.outflow)}</td></tr>
            ))}
          </tbody>
        </table>
      </>
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
      <>
        <div style={{ margin: '.2rem 0 .8rem' }}><RiskMatrix risks={risks} /></div>
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
      </>
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
              <td style={{ paddingInlineStart: `${(n.id.split('.').length - 1) * 1.2}rem` }}>{n.id}</td>
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
              <td style={{ paddingInlineStart: `${(n.id.split('.').length - 1) * 1.2}rem` }}>{n.id}</td>
              <td>{n.name ?? '—'}</td>
              <td>{n.value != null ? fmt(n.value) : '—'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'productivity') {
    const labels: Record<string, string> = {
      planned_qty: 'Planned quantity', planned_hours: 'Planned hours',
      earned_qty: 'Earned quantity', actual_hours: 'Actual hours',
    }
    return (
      <table className="data">
        <tbody>
          {Object.keys(labels).filter((k) => k in g).map((k) => (
            <tr key={k}><th style={{ textAlign: 'start' }}>{labels[k]}</th><td>{fmt(g[k])}</td></tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'boq') {
    const lines = (g.lines as { id: string; qty: number; rate: number }[]) ?? []
    return (
      <table className="data">
        <thead><tr><th>Line</th><th>Qty</th><th>Rate</th><th>Amount</th></tr></thead>
        <tbody>
          {lines.map((l) => (
            <tr key={l.id}><td>{l.id}</td><td>{fmt(l.qty)}</td><td>{fmt(l.rate)}</td><td>{fmt(l.qty * l.rate)}</td></tr>
          ))}
        </tbody>
      </table>
    )
  }
  if (task.task === 'resource') {
    const periods = (g.periods as { period: number; demand: number; capacity: number }[]) ?? []
    return (
      <>
        <div style={{ margin: '.2rem 0 .8rem' }}><ResourceChart periods={periods} /></div>
        <table className="data">
          <thead><tr><th>Period</th><th>Demand</th><th>Capacity</th><th>Overload</th></tr></thead>
          <tbody>
            {periods.map((p) => (
              <tr key={p.period}>
                <td>{p.period}</td><td>{fmt(p.demand)}</td><td>{fmt(p.capacity)}</td>
                <td>{fmt(Math.max(0, p.demand - p.capacity))}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </>
    )
  }
  if (task.task === 'procurement') {
    return (
      <table className="data">
        <tbody>
          <tr><th style={{ textAlign: 'start' }}>Project duration (days)</th><td>{fmt(g.project_duration)}</td></tr>
          <tr><th style={{ textAlign: 'start' }}>Remaining float (days)</th><td>{fmt(g.remaining_float)}</td></tr>
          <tr><th style={{ textAlign: 'start' }}>Supplier delay (days)</th><td>{fmt(g.supplier_delay_days)}</td></tr>
        </tbody>
      </table>
    )
  }
  if (task.task === 'portfolio') {
    const projects = (g.projects as { id: string; npv: number; risk: number; fit: number }[]) ?? []
    return (
      <>
        <div className="small muted" style={{ marginBottom: '.3rem' }}>
          Weights — NPV {fmt(g.w_npv)} · risk {fmt(g.w_risk)} · fit {fmt(g.w_fit)}
        </div>
        <table className="data">
          <thead><tr><th>Project</th><th>NPV</th><th>Risk</th><th>Fit</th></tr></thead>
          <tbody>
            {projects.map((p) => (
              <tr key={p.id}><td>{p.id}</td><td>{fmt(p.npv)}</td><td>{fmt(p.risk)}</td><td>{fmt(p.fit)}</td></tr>
            ))}
          </tbody>
        </table>
      </>
    )
  }
  if (task.task === 'decision') {
    const options = (g.options as { id: string; cost: number; schedule: number; risk: number }[]) ?? []
    return (
      <>
        <div className="small muted" style={{ marginBottom: '.3rem' }}>
          Weights — cost {fmt(g.w_cost)} · schedule {fmt(g.w_sched)} · risk {fmt(g.w_risk)}
        </div>
        <table className="data">
          <thead><tr><th>Option</th><th>Cost impact</th><th>Schedule impact</th><th>Risk</th></tr></thead>
          <tbody>
            {options.map((o) => (
              <tr key={o.id}><td>{o.id}</td><td>{fmt(o.cost)}</td><td>{fmt(o.schedule)}</td><td>{fmt(o.risk)}</td></tr>
            ))}
          </tbody>
        </table>
      </>
    )
  }
  if (task.task === 'data_quality') {
    const rowsDq = (g.rows as { value: number | null; expected: number }[]) ?? []
    return (
      <>
        <div className="small muted" style={{ marginBottom: '.3rem' }}>Anomaly threshold: {fmt(g.threshold)}</div>
        <table className="data">
          <thead><tr><th>#</th><th>Value</th><th>Expected</th></tr></thead>
          <tbody>
            {rowsDq.map((r, i) => (
              <tr key={i}><td>{i + 1}</td><td>{r.value == null ? '—' : fmt(r.value)}</td><td>{fmt(r.expected)}</td></tr>
            ))}
          </tbody>
        </table>
      </>
    )
  }
  return <pre className="small muted" style={{ whiteSpace: 'pre-wrap' }}>{JSON.stringify(g, null, 2)}</pre>
}

/** Coach message with identity, source and mode — calm, never dominating the canvas. */
function CoachMessage({ coach }: { coach: CoachResp }) {
  return (
    <div>
      <div className="sl-coach-head" style={{ marginBottom: '.35rem' }}>
        <strong>Coach</strong>
        <Badge tone={coach.ai ? 'brand' : 'neutral'}>{coach.ai ? 'AI' : 'Guide'}</Badge>
        {coach.coach_mode && <Badge tone="neutral">{titleCase(coach.coach_mode)}</Badge>}
        {coach.hint_level != null && coach.hint_level > 0 && (
          <LevelDots level={coach.hint_level} max={6} label={`Hint level ${coach.hint_level} of 6`} />
        )}
      </div>
      <div className="sl-coach-msg" role="status">{coach.message}</div>
    </div>
  )
}

export default function LabRunner() {
  const { code } = useParams<{ code: string }>()
  const { lang } = useI18n()
  const [params, setParams] = useSearchParams()
  // /lab/:code?mode=assessment launches straight into Assessment Mode (from the details panel);
  // /lab/:code?attempt=ID opens a completed attempt read-only for review.
  const reviewId = params.get('attempt')
  const [mode, setMode] = useState<Mode>(() => (params.get('mode') === 'assessment' ? 'assessment' : 'training'))
  const [review, setReview] = useState<AttemptDetail | null>(null)
  const [railTab, setRailTab] = useState<'coach' | 'reference'>('coach')
  const [pendingMode, setPendingMode] = useState<Mode | null>(null)
  const [start, setStart] = useState<StartResp | null>(null)
  const [answers, setAnswers] = useState<Record<string, string>>({})
  const [stepAnswers, setStepAnswers] = useState<Record<string, Record<string, string>>>({})   // multi-step: per-step measures
  const [decisions, setDecisions] = useState<Record<string, string>>({})                        // multi-step: chosen options
  const [grade, setGrade] = useState<Grade | null>(null)
  const [templates, setTemplates] = useState<TemplateRow[]>([])
  const [coach, setCoach] = useState<CoachResp | null>(null)
  const [coaching, setCoaching] = useState(false)
  const [coachMode, setCoachMode] = useState('guided')
  const [hintLevel, setHintLevel] = useState(2)
  const [saveState, setSaveState] = useState<SaveState>('idle')
  const [saveNote, setSaveNote] = useState<string | null>(null)
  const [reportNote, setReportNote] = useState<string | null>(null)
  const [reporting, setReporting] = useState(false)
  const [loading, setLoading] = useState(true)
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const skipAutosave = useRef(true)
  const answersRef = useRef(answers)
  answersRef.current = answers
  const stepAnswersRef = useRef(stepAnswers)
  stepAnswersRef.current = stepAnswers
  const decisionsRef = useRef(decisions)
  decisionsRef.current = decisions
  const debriefRef = useRef<HTMLDivElement>(null)

  const begin = useCallback(async (m: Mode) => {
    // Drop the previous attempt immediately so debounced autosave cannot write empty answers
    // into the old (or new) attempt while the start request is in flight.
    skipAutosave.current = true
    setLoading(true); setError(null); setGrade(null); setStart(null); setAnswers({}); setStepAnswers({}); setDecisions({}); setCoach(null)
    setReview(null); setSaveState('idle'); setSaveNote(null); setReportNote(null)
    try {
      const s = await api.post<StartResp>('/api/me/lab/attempts', { scenario_code: code, mode: m, lang })
      skipAutosave.current = true
      setStart(s)
      let restoredCount = 0
      if (s.task.multistep && s.task.steps) {
        // Resume: saved multi-step answers arrive as { steps: {id:{key:val}}, decisions: {key:val} }.
        const saved = (s.answers ?? {}) as { steps?: Record<string, Record<string, unknown>>; decisions?: Record<string, unknown> }
        const sa: Record<string, Record<string, string>> = {}
        for (const st of s.task.steps) sa[st.id] = hydrateAnswers(st.ask, saved.steps?.[st.id])
        const dec: Record<string, string> = {}
        for (const [k, v] of Object.entries(saved.decisions ?? {})) if (v != null) dec[k] = String(v)
        setStepAnswers(sa); setDecisions(dec)
        restoredCount = Object.values(sa).reduce((n, mm) => n + Object.keys(mm).length, 0) + Object.keys(dec).length
      } else {
        const restored = hydrateAnswers(s.task.ask ?? [], s.answers)
        setAnswers(restored)
        restoredCount = Object.keys(restored).length
      }
      if (s.resumed && restoredCount > 0) {
        setSaveState('saved'); setSaveNote('Resumed your saved answers')
      }
    } catch (e) {
      setError(mapError(e)); setStart(null)
    } finally {
      setLoading(false)
    }
  }, [code, lang])

  // Load a past attempt read-only. An in-progress attempt falls back to the normal
  // resume flow (the server resumes it when a new start is requested in its mode).
  const loadReview = useCallback(async (id: string) => {
    skipAutosave.current = true
    setLoading(true); setError(null); setGrade(null); setStart(null); setAnswers({}); setCoach(null)
    setReview(null); setSaveState('idle'); setSaveNote(null); setReportNote(null)
    try {
      const a = await api.get<AttemptDetail>(`/api/me/lab/attempts/${id}${lang !== 'en' ? `?lang=${lang}` : ''}`)
      if (a.status === 'in_progress' || !a.grade) {
        if (a.mode === 'assessment') setMode('assessment')
        setParams((prev) => {
          const p = new URLSearchParams(prev)
          p.delete('attempt')
          return p
        }, { replace: true })
        return // the effect re-runs without ?attempt and resumes normally
      }
      skipAutosave.current = true
      setReview(a)
      setStart({ attempt_id: a.attempt_id, resumed: false, answers: a.answers, scenario: a.scenario, task: a.task })
      setGrade(a.grade)
    } catch (e) {
      setError(mapError(e)); setStart(null)
    } finally {
      setLoading(false)
    }
  }, [setParams, lang])

  useEffect(() => {
    if (reviewId) void loadReview(reviewId)
    else void begin(mode)
  }, [begin, loadReview, mode, reviewId])

  // The Free Templates Library, fetched once so the graded result can offer the working sheet for the
  // technique just practised. Wrapped so that ANY failure — a rejection, or a client that returns nothing
  // at all — is swallowed: the download is a bonus, and must never take the workspace down with it.
  useEffect(() => {
    let cancelled = false
    void (async () => {
      try {
        const r = await api.get<{ rows: TemplateRow[] }>('/api/me/lab/templates')
        if (!cancelled && Array.isArray(r?.rows)) setTemplates(r.rows)
      } catch { /* library unavailable — the workspace works exactly as before */ }
    })()
    return () => { cancelled = true }
  }, [])

  const coerce = (a: Ask, raw: string): unknown => {
    const v = (raw ?? '').trim()
    if (a.type === 'set') return v.split(/[,\s]+/).filter(Boolean)
    if (a.type === 'bool') return /^(y|yes|true|valid)$/i.test(v) ? true : /^(n|no|false|invalid)$/i.test(v) ? false : null
    return v === '' ? null : Number(v)
  }

  const buildAnswersPayload = useCallback((from?: Record<string, string>): Record<string, unknown> => {
    if (!start) return {}
    // Multi-step: { steps: { id: { key: value } }, decisions: { key: option } } — the shape SimStep grades.
    if (start.task.multistep && start.task.steps) {
      const steps: Record<string, Record<string, unknown>> = {}
      for (const st of start.task.steps) {
        const m: Record<string, unknown> = {}
        for (const a of st.ask) m[a.key] = coerce(a, stepAnswersRef.current[st.id]?.[a.key] ?? '')
        steps[st.id] = m
      }
      return { steps, decisions: decisionsRef.current }
    }
    const src = from ?? answersRef.current
    const payload: Record<string, unknown> = {}
    for (const a of start.task.ask ?? []) payload[a.key] = coerce(a, src[a.key] ?? '')
    return payload
  }, [start])

  const autosave = useCallback(async (silent = false) => {
    if (!start || grade) return
    setSaveState('saving'); if (!silent) setSaveNote(null)
    try {
      await api.post(`/api/me/lab/attempts/${start.attempt_id}/autosave`, { answers: buildAnswersPayload() })
      setSaveState('saved')
      setSaveNote(silent ? null : 'Progress saved')
    } catch {
      setSaveState('error')
      setSaveNote(silent ? null : 'Autosave unavailable — your answers stay in this browser until you submit.')
    }
  }, [start, grade, buildAnswersPayload])

  // Debounced autosave after the student edits answers (skips the resume hydrate tick).
  useEffect(() => {
    if (!start || grade) return
    if (skipAutosave.current) {
      skipAutosave.current = false
      return
    }
    const t = window.setTimeout(() => { void autosave(true) }, 1500)
    return () => window.clearTimeout(t)
  }, [answers, stepAnswers, decisions, start, grade, autosave])

  // Ctrl/Cmd+S saves progress instead of invoking the browser's save dialog.
  useEffect(() => {
    const onKey = (e: KeyboardEvent) => {
      if ((e.ctrlKey || e.metaKey) && e.key.toLowerCase() === 's') {
        e.preventDefault()
        if (start && !grade) void autosave(false)
      }
    }
    window.addEventListener('keydown', onKey)
    return () => window.removeEventListener('keydown', onKey)
  }, [start, grade, autosave])

  // A mode switch opens a separate attempt: confirm first when visible work could leave the screen.
  const requestMode = (m: Mode) => {
    if (m === mode) return
    const hasWork = !grade && Object.values(answersRef.current).some((v) => v.trim() !== '')
    if (hasWork) setPendingMode(m)
    else setMode(m)
  }
  const confirmMode = () => {
    if (!pendingMode) return
    void autosave(true) // best effort: capture anything the debounce has not flushed yet
    setMode(pendingMode)
    setPendingMode(null)
  }

  const reportProblem = async () => {
    if (!start) return
    setReporting(true); setReportNote(null)
    try {
      const r = await api.post<{ reference: string }>('/api/me/tickets', {
        subject: `Lab content issue: ${start.scenario.scenario_code}`,
        category: 'simlab',
        body: [
          `Scenario: ${start.scenario.scenario_code} — ${start.scenario.title}`,
          `Attempt: #${start.attempt_id}`,
          `Mode: ${mode}`,
          '',
          'Please describe what looks wrong (typo, wrong figure, unclear brief, grading concern):',
          '',
        ].join('\n'),
      })
      setReportNote(`Ticket ${r.reference} opened — reply from Support with more detail if needed.`)
    } catch {
      setReportNote('Could not open a ticket — use Support from your account menu.')
    } finally {
      setReporting(false)
    }
  }

  const submit = async () => {
    if (!start || busy) return
    setBusy(true); setError(null)
    try {
      const g = await api.post<Grade>(`/api/me/lab/attempts/${start.attempt_id}/submit`, { answers: buildAnswersPayload() })
      setGrade(g)
    } catch (e) {
      setError(mapError(e))
    } finally {
      setBusy(false)
    }
  }

  // Move focus to the debrief once the grade lands so keyboard and screen-reader
  // users are not left on an unmounted submit button.
  useEffect(() => {
    if (grade) debriefRef.current?.focus()
  }, [grade])

  // After grading, surface the mastery engine's next-scenario recommendations in the debrief.
  const [nextUp, setNextUp] = useState<{ scenario_code: string; title: string; because?: string[] }[] | null>(null)
  useEffect(() => {
    if (!grade) { setNextUp(null); return }
    let cancelled = false
    Promise.resolve(api.get<{ recommended?: { scenario_code: string; title: string; because?: string[] }[] }>('/api/me/lab/mastery'))
      .then((r) => {
        if (cancelled) return
        const recs = Array.isArray(r?.recommended)
          ? r.recommended.filter((x) => x.scenario_code !== code).slice(0, 3)
          : []
        setNextUp(recs)
      })
      .catch(() => { if (!cancelled) setNextUp([]) })
    return () => { cancelled = true }
  }, [grade, code])

  const askCoach = async (levelOverride?: number) => {
    if (!start) return
    setCoaching(true)
    const level = levelOverride ?? hintLevel
    try {
      const r = await api.post<CoachResp>(
        `/api/me/lab/attempts/${start.attempt_id}/coach`,
        { answers: buildAnswersPayload(), coach_mode: coachMode, hint_level: level },
      )
      setCoach(r)
      if (r.ok && !grade) setHintLevel((h) => Math.min(6, Math.max(h, level) + 1))
    } catch {
      setCoach({ ok: false, message: 'The coach is unavailable right now — please try again shortly.', ai: false })
    } finally {
      setCoaching(false)
    }
  }

  const assessment = !!start?.task.assessment

  // The working sheet for this scenario's engine. Multi-step scenarios drive several engines, so they get
  // the template for the step the learner is on; single-task scenarios match on the task itself.
  const engineNow = start?.task.multistep
    ? start.task.steps?.[0]?.task ?? ''
    : start?.task.task ?? ''
  const workingTemplate = templates.find((t) => t.engine && t.engine === engineNow) ?? null

  // Printable debrief summary via the platform's existing printable-document pattern.
  const printSummary = () => {
    if (!start || !grade) return
    const esc = (s: unknown) => String(s ?? '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
    const rows = grade.measures.map((m) => `<tr>
      <td>${esc(m.label)}</td>
      <td>${m.is_correct ? '✓ Correct' : '✗ Check this'}</td>
      <td class="r">${esc(grade.assessment ? '—' : fmt(m.your_value))}</td>
      ${grade.assessment ? '' : `<td class="r">${esc(fmt(m.correct_value))}</td>`}
    </tr>`).join('')
    const comps = grade.competencies.map((c) => `${esc(titleCase(c.competency))} · ${esc(titleCase(c.level))}`).join(', ')
    openPrintable(`Simulation debrief — ${start.scenario.scenario_code}`, `
      <div class="brand">PCI Simulation Lab</div>
      <h1>${esc(start.scenario.title)}</h1>
      <div class="muted">${esc(start.scenario.scenario_code)} · ${esc(titleCase(start.scenario.kind))}
        · ${grade.assessment ? 'Assessment Mode' : 'Training Mode'}
        · ${esc(fmtDateTime(review?.completed_at ?? new Date().toISOString()))}</div>
      <div class="big">${grade.score}% — ${grade.correct} of ${grade.total} measures correct</div>
      <table><thead><tr><th>Measure</th><th>Verdict</th><th class="r">Your answer</th>${grade.assessment ? '' : '<th class="r">Correct</th>'}</tr></thead>
      <tbody>${rows}</tbody></table>
      ${comps ? `<p class="muted">Competency evidence: ${comps}</p>` : ''}
      ${grade.assessment ? '<p class="muted">Assessment Mode reports marks only — the worked answers are withheld.</p>' : ''}
      <p class="muted">Educational simulator using synthetic project data. Practice never affects a formal PCI examination or certification.</p>
    `)
  }

  // Retrying from a review starts a fresh attempt in the reviewed attempt's mode.
  const retryFromReview = () => {
    if (review?.mode === 'assessment') setMode('assessment')
    setParams((prev) => {
      const p = new URLSearchParams(prev)
      p.delete('attempt')
      return p
    }, { replace: true })
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <header className="sl-runhead">
        <div className="sl-runhead-top">
          <Link to="/lab" className="sl-back">← Back to the Practice Lab</Link>
          <SaveIndicator state={saveState} note={saveNote} />
        </div>
        <div className="sl-runhead-main">
          <div>
            <h1>{start?.scenario.title ?? code}</h1>
            {start && (
              <div className="row small muted" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
                <Badge tone="neutral">{titleCase(start.scenario.kind)}</Badge>
                {start.scenario.difficulty && <Badge tone="neutral">{titleCase(start.scenario.difficulty)}</Badge>}
                <span style={{ fontVariantNumeric: 'tabular-nums' }}>{start.scenario.scenario_code}</span>
              </div>
            )}
          </div>
          {review ? (
            <InsightCallout tone="info" title="Reviewing a completed attempt">
              <p>
                {review.mode === 'assessment' ? 'Assessment' : 'Training'} attempt
                {review.completed_at ? ` · completed ${fmtDateTime(review.completed_at)}` : ''}. This view is read-only.
              </p>
            </InsightCallout>
          ) : (
            <SegmentedControl<Mode>
              legend="Mode"
              value={mode}
              onChange={requestMode}
              disabled={busy || loading}
              options={[{ value: 'training', label: 'Training' }, { value: 'assessment', label: 'Assessment' }]}
              hint={mode === 'training'
                ? 'Training reveals the worked answers after grading.'
                : 'Assessment reports marks only — answers are withheld.'}
            />
          )}
        </div>
      </header>

      <ConfirmDialog
        open={pendingMode !== null}
        title={`Switch to ${pendingMode === 'assessment' ? 'Assessment' : 'Training'} Mode?`}
        body="Switching mode opens a separate attempt of this scenario. The answers you have entered stay saved with your current attempt."
        confirmLabel="Switch mode"
        cancelLabel="Stay here"
        onConfirm={confirmMode}
        onCancel={() => setPendingMode(null)}
      />

      {loading ? (
        <Spinner />
      ) : error && !start ? (
        <Card><ErrorNote>{error}</ErrorNote></Card>
      ) : start ? (
        <div className="sl-workspace">
          <div className="sl-canvas">
            <Card title="Brief">
              <p style={{ marginTop: 0 }}>{start.task.prompt}</p>
              {!start.task.multistep && (
                <>
                  <PanelSub>Evidence</PanelSub>
                  {Array.isArray(start.task.given?.series) && (start.task.given!.series as EvmPoint[]).length > 0 && (
                    <div style={{ margin: '.2rem 0 .8rem' }}><SCurve series={start.task.given!.series as EvmPoint[]} /></div>
                  )}
                  <div style={{ overflowX: 'auto' }}><GivenView task={start.task} /></div>
                  {start.task.montecarlo && (
                    <div style={{ margin: '.8rem 0 .2rem' }}><Histogram mc={start.task.montecarlo} /></div>
                  )}
                </>
              )}
              {/* The technique this scenario teaches, as a working sheet for a real project. Only shown
                  once the attempt is graded, so it can never become a hint mid-attempt. */}
              {grade && workingTemplate && (
                <div style={{ marginTop: '.9rem' }}>
                  <PanelSub>Take it to your own project</PanelSub>
                  <p className="muted small" style={{ margin: '.2rem 0 .4rem' }}>{workingTemplate.description}</p>
                  <a className="btn sm" href={workingTemplate.url} download>
                    Download “{workingTemplate.title}”
                  </a>
                </div>
              )}
            </Card>

            {!grade ? (
              <Card title="Your answers" action={assessment ? <Badge tone="warn">Assessment</Badge> : undefined}>
                {start.task.multistep && start.task.steps ? (
                  <div className="sl-answers">
                    <ol className="stack" style={{ display: 'grid', gap: '1rem', margin: 0, paddingLeft: '1.1rem' }}>
                      {start.task.steps.map((st, i) => {
                        const eg = effectiveGiven(st, start.task.steps!, decisions)
                        const chosen = st.decision ? decisions[st.decision.key] : undefined
                        return (
                          <li key={st.id} aria-label={`Step ${i + 1}${st.title ? `: ${st.title}` : ''}`}>
                            <div style={{ fontWeight: 600 }}>{st.title || `Step ${i + 1}`}</div>
                            {st.prompt && <p className="small" style={{ margin: '.2rem 0 .5rem' }}>{st.prompt}</p>}
                            <div style={{ overflowX: 'auto', marginBottom: '.5rem' }}><GivenView task={{ task: st.task, given: eg, prompt: '', mode: start.task.mode, assessment: start.task.assessment }} /></div>
                            {st.decision && (
                              <fieldset style={{ border: '1px solid var(--line, #e2e8f0)', borderRadius: '.5rem', padding: '.6rem .8rem', margin: '0 0 .6rem' }}>
                                <legend className="small" style={{ padding: '0 .3rem' }}>{st.decision.prompt || 'Decision'}</legend>
                                <div className="stack" style={{ display: 'grid', gap: '.3rem' }}>
                                  {st.decision.options.map((o) => (
                                    <label key={o.value} className="row small" style={{ gap: '.4rem', alignItems: 'flex-start' }}>
                                      <input type="radio" name={`dec-${st.decision!.key}`} value={o.value}
                                        checked={chosen === o.value}
                                        onChange={() => setDecisions((d) => ({ ...d, [st.decision!.key]: o.value }))} />
                                      <span>{o.label}</span>
                                    </label>
                                  ))}
                                </div>
                              </fieldset>
                            )}
                            <div className="sl-answers">
                              {st.ask.map((a) => (
                                <label key={a.key} className="sl-answer">
                                  <span>{a.label}</span>
                                  <input
                                    inputMode={a.type === 'number' ? 'decimal' : 'text'}
                                    placeholder={a.type === 'set' ? 'e.g. A, B, D' : a.type === 'bool' ? 'yes / no' : 'number'}
                                    value={stepAnswers[st.id]?.[a.key] ?? ''}
                                    onChange={(e) => setStepAnswers((s) => ({ ...s, [st.id]: { ...s[st.id], [a.key]: e.target.value } }))}
                                  />
                                </label>
                              ))}
                            </div>
                          </li>
                        )
                      })}
                    </ol>
                    {error && <ErrorNote>{error}</ErrorNote>}
                    <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'center', marginTop: '.6rem' }}>
                      <button className="btn" onClick={submit} disabled={busy}>{busy ? 'Grading…' : 'Submit for grading'}</button>
                      <button className="btn secondary" type="button" onClick={() => { void autosave(false) }} disabled={busy} title="Ctrl+S / ⌘S">Save progress</button>
                      <button className="btn ghost sm" type="button" onClick={() => { void reportProblem() }} disabled={reporting || busy}>
                        {reporting ? 'Opening ticket…' : 'Report a problem'}
                      </button>
                      {reportNote && <span className="small muted" role="status">{reportNote}</span>}
                    </div>
                  </div>
                ) : (
                <div className="sl-answers">
                  {(start.task.ask ?? []).map((a) => (
                    <label key={a.key} className="sl-answer">
                      <span>{a.label}</span>
                      <input
                        inputMode={a.type === 'number' ? 'decimal' : 'text'}
                        placeholder={a.type === 'set' ? 'e.g. A, B, D' : a.type === 'bool' ? 'yes / no' : 'number'}
                        value={answers[a.key] ?? ''}
                        onChange={(e) => setAnswers((s) => ({ ...s, [a.key]: e.target.value }))}
                      />
                    </label>
                  ))}
                  {error && <ErrorNote>{error}</ErrorNote>}
                  <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'center', marginTop: '.3rem' }}>
                    <button className="btn" onClick={submit} disabled={busy}>{busy ? 'Grading…' : 'Submit for grading'}</button>
                    <button className="btn secondary" type="button" onClick={() => { void autosave(false) }} disabled={busy} title="Ctrl+S / ⌘S">Save progress</button>
                    <button className="btn ghost sm" type="button" onClick={() => { void reportProblem() }} disabled={reporting || busy}>
                      {reporting ? 'Opening ticket…' : 'Report a problem'}
                    </button>
                    {reportNote && <span className="small muted" role="status">{reportNote}</span>}
                  </div>
                </div>
                )}
              </Card>
            ) : (
              <Card className="entity" title="Debrief" action={<Badge tone={grade.passed ? 'ok' : 'warn'}>{grade.passed ? 'Passed' : 'Keep practising'}</Badge>}>
                <div ref={debriefRef} tabIndex={-1} style={{ outline: 'none' }} className="stack" >
                  <div className="sl-scorehero">
                    <span className="n" aria-hidden="true">{grade.score}<small>%</small></span>
                    <div className="sl-scoresub">
                      <span className="t">
                        <span className="sr-only">{`Result — ${grade.score}%`}</span>
                        <span aria-hidden="true">{grade.passed ? 'Scenario passed' : 'Scenario graded'}</span>
                      </span>
                      <span className="d">{grade.correct} of {grade.total} measures correct · {assessment ? 'Assessment Mode' : 'Training Mode'}</span>
                    </div>
                  </div>

                  <PanelSub>Measures</PanelSub>
                  <table className="data">
                    <thead>
                      <tr><th>Measure</th><th>Verdict</th><th>Your answer</th>{!grade.assessment && <th>Correct</th>}</tr>
                    </thead>
                    <tbody>
                      {grade.measures.map((m) => (
                        <tr key={m.key}>
                          <td>{m.label}</td>
                          <td>
                            {m.is_correct
                              ? <span className="sl-verdict ok">✓ Correct</span>
                              : <span className="sl-verdict miss">✗ Check this</span>}
                          </td>
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
                      <PanelSub>Computed schedule</PanelSub>
                      <div className="small muted" style={{ marginBottom: '.2rem', fontVariantNumeric: 'tabular-nums' }}>
                        Critical path {grade.schedule.critical_path.join(' → ')} · duration {grade.schedule.project_duration}
                      </div>
                      <div style={{ overflowX: 'auto' }}>
                        <Gantt bars={grade.schedule.bars} projectDuration={grade.schedule.project_duration} />
                      </div>
                    </div>
                  )}

                  {grade.competencies.length > 0 && (
                    <div>
                      <PanelSub>Competency evidence</PanelSub>
                      <div className="row" style={{ flexWrap: 'wrap', gap: '.3rem', alignItems: 'center' }}>
                        {grade.competencies.map((c) => (
                          <Badge key={c.competency} tone="brand">{titleCase(c.competency)} · {titleCase(c.level)}</Badge>
                        ))}
                      </div>
                    </div>
                  )}

                  {nextUp && nextUp.length > 0 && (
                    <div>
                      <PanelSub>Recommended next</PanelSub>
                      <div className="stack" style={{ display: 'grid', gap: '.45rem' }}>
                        {nextUp.map((r) => (
                          <div key={r.scenario_code} className="row" style={{ justifyContent: 'space-between', gap: '.5rem', flexWrap: 'wrap' }}>
                            <div className="small" style={{ minWidth: 0 }}>
                              <strong>{r.title}</strong>
                              {r.because && r.because.length > 0 && (
                                <span className="muted"> — strengthens {r.because.map(titleCase).join(', ')}</span>
                              )}
                            </div>
                            <Link className="btn sm secondary" to={`/lab/${r.scenario_code}`}>Open</Link>
                          </div>
                        ))}
                      </div>
                    </div>
                  )}

                  <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', marginTop: '.4rem' }}>
                    <button className="btn secondary" onClick={() => (review ? retryFromReview() : void begin(mode))}>
                      Try again
                    </button>
                    <button className="btn secondary sm" type="button" onClick={printSummary}>Print summary</button>
                    <Link className="btn sm ghost" to="/lab">Back to labs</Link>
                    <button className="btn ghost sm" type="button" onClick={() => { void reportProblem() }} disabled={reporting}>
                      {reporting ? 'Opening ticket…' : 'Report a problem'}
                    </button>
                    {reportNote && <span className="small muted" role="status">{reportNote}</span>}
                  </div>
                </div>
              </Card>
            )}
          </div>

          <aside className="sl-rail" aria-label="Coach and reference">
            <Card
              title={railTab === 'coach' ? 'Coach' : 'Method reference'}
              action={
                <span className="row" style={{ gap: '.3rem' }}>
                  <Chip pressed={railTab === 'coach'} onClick={() => setRailTab('coach')}>Coach</Chip>
                  <Chip pressed={railTab === 'reference'} onClick={() => setRailTab('reference')}>Reference</Chip>
                </span>
              }
            >
              {railTab === 'reference' ? (
                <div className="sl-ref">
                  <p className="sl-footnote">
                    Definitions and formulas for this task type — generic course knowledge, never a
                    scenario figure.
                  </p>
                  <dl>
                    {(REFERENCE[start.task.task ?? ''] ?? [{ term: 'Method', formula: 'Work from the definitions in the brief — the Coach can explain the approach.' }]).map((f) => (
                      <span key={f.term}>
                        <dt>{f.term}</dt>
                        <dd><code>{f.formula}</code></dd>
                      </span>
                    ))}
                  </dl>
                </div>
              ) : (
              <div className="sl-coach">
                <div className="sl-coach-head">
                  <span className="sl-coach-id" aria-hidden="true">PC</span>
                  <div className="small muted">
                    Professional project-controls mentor. Hints reference the brief and evidence — never the answer key.
                  </div>
                </div>

                {assessment ? (
                  <InsightCallout tone="info" title="Assessment Mode">
                    The Coach steps back during assessment so your marks reflect your own analysis. Switch to
                    Training Mode for guided help.
                  </InsightCallout>
                ) : !grade ? (
                  <div className="sl-coach-controls">
                    <div className="row-2">
                      <label className="small" style={{ margin: 0 }}>Coach mode
                        <select value={coachMode} onChange={(e) => setCoachMode(e.target.value)} aria-label="Coach mode">
                          <option value="socratic">Socratic</option>
                          <option value="guided">Guided</option>
                          <option value="explain">Explain</option>
                          <option value="review">Review</option>
                          <option value="language">Language / accessibility</option>
                        </select>
                      </label>
                      <label className="small" style={{ margin: 0 }}>Hint level
                        <select value={hintLevel} onChange={(e) => setHintLevel(Number(e.target.value))} aria-label="Hint level">
                          {[1, 2, 3, 4, 5].map((n) => <option key={n} value={n}>{n}</option>)}
                        </select>
                      </label>
                    </div>
                    <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap' }}>
                      <button className="btn secondary sm" type="button" onClick={() => { void askCoach() }} disabled={coaching}>
                        {coaching ? 'Asking…' : 'Ask for a hint'}
                      </button>
                      {coach && !coaching && hintLevel > 1 && (
                        <button className="btn ghost sm" type="button" onClick={() => { void askCoach(1) }} disabled={coaching}>
                          Give me a smaller hint
                        </button>
                      )}
                    </div>
                  </div>
                ) : (
                  <div className="sl-coach-controls">
                    <label className="small" style={{ margin: 0 }}>Debrief mode
                      <select value={coachMode} onChange={(e) => setCoachMode(e.target.value)} aria-label="Debrief coach mode">
                        <option value="debrief">Debrief</option>
                        <option value="explain">Explain</option>
                        <option value="review">Review</option>
                        <option value="language">Language / accessibility</option>
                      </select>
                    </label>
                    <div>
                      <button className="btn secondary sm" onClick={() => { setCoachMode('debrief'); void askCoach(6) }} disabled={coaching}>
                        {coaching ? 'Asking the coach…' : 'Ask the coach'}
                      </button>
                    </div>
                  </div>
                )}

                {!assessment && coach && <CoachMessage coach={coach} />}
              </div>
              )}
            </Card>
          </aside>
        </div>
      ) : null}

      <p className="sl-footnote">
        Educational simulator using synthetic project data. Practice never affects your formal PCI
        examination or certification.
      </p>
    </div>
  )
}
