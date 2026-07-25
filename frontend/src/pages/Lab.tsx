import { useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { useQuery } from '../api/hooks'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'

// PCI AI Project Controls Simulation Lab — student surface (Phase 1 foundation).
// Applied, in-portal project-controls practice (WBS, schedule, cost, EVM, forecasting, risk, reporting)
// on synthetic scenarios — distinct from Certuvo (external MCQ exam-prep) and from the real examination.
// Access is computed live from the student's existing PCI entitlement; the catalogue lists only
// published labs/drills/scenarios. Filters and mastery recommendations help students find the next drill.

interface Access {
  enabled: boolean
  has_access: boolean
  reason: string
  rule?: string
  source?: string
  member_type?: string | null
  expires_at?: string | null
}
interface LabRow {
  id: number
  scenario_code: string
  title: string
  kind: string
  industry?: string | null
  difficulty?: string | null
  est_minutes?: number
  competencies?: string[]
  certification_id?: number | null
  summary?: string | null
  version?: number
  interactive?: boolean
  attempt_status?: string | null
  score?: number | null
}
interface MasteryRow { competency: string; avg_score: number; attempts: number; level?: string | null }
interface Recommended {
  scenario_code: string
  title: string
  kind: string
  difficulty?: string | null
  est_minutes?: number
  summary?: string | null
  because: string[]
}
interface MasteryResp {
  mastery: MasteryRow[]
  recommended: Recommended[]
  weak_competencies: string[]
}

const KIND_LABEL: Record<string, string> = {
  guided_lab: 'Guided lab',
  skill_drill: 'Skill drill',
  scenario: 'Scenario',
  capstone: 'Capstone',
  team: 'Team',
}
const CERT_LABEL: Record<number, string> = {
  1: 'PCL-AI',
  2: 'PFL-AI',
  3: 'PML-AI',
}
const COMPETENCY_LABEL: Record<string, string> = {
  scope_structuring: 'Scope structuring',
  schedule_development: 'Schedule development',
  schedule_analysis: 'Schedule analysis',
  cost_control: 'Cost control',
  earned_value: 'Earned value',
  forecasting: 'Forecasting',
  risk_management: 'Risk management',
  change_control: 'Change control',
  progress_measurement: 'Progress measurement',
  cash_flow: 'Cash flow',
  productivity_analysis: 'Productivity analysis',
  quantity_surveying: 'Quantity surveying',
  resource_management: 'Resource management',
  procurement_management: 'Procurement management',
  data_quality: 'Data quality',
  portfolio_selection: 'Portfolio selection',
  financial_analysis: 'Financial analysis',
  decision_analysis: 'Decision analysis',
}
const titleCaseWords = (s: string) => s.replace(/[_-]+/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())

type DurationBucket = '' | 'short' | 'medium' | 'long'

function matchesDuration(mins: number | undefined, bucket: DurationBucket): boolean {
  if (!bucket) return true
  const m = mins ?? 0
  if (bucket === 'short') return m > 0 && m <= 15
  if (bucket === 'medium') return m > 15 && m <= 25
  return m > 25
}

export default function Lab() {
  const { data: access, loading: aLoading, error: aError } = useQuery<Access>('/api/me/lab/access')
  // Only load the catalogue once access is confirmed (the endpoint is access-gated).
  const { data: cat, loading: cLoading } = useQuery<{ rows: LabRow[] }>(
    access?.has_access ? '/api/me/lab/catalogue' : null,
  )
  const { data: mastery } = useQuery<MasteryResp>(
    access?.has_access ? '/api/me/lab/mastery' : null,
  )

  const [q, setQ] = useState('')
  const [track, setTrack] = useState('')
  const [industry, setIndustry] = useState('')
  const [difficulty, setDifficulty] = useState('')
  const [kind, setKind] = useState('')
  const [competency, setCompetency] = useState('')
  const [duration, setDuration] = useState<DurationBucket>('')

  const rows = cat?.rows ?? []

  const industries = useMemo(
    () => [...new Set(rows.map((r) => r.industry).filter(Boolean) as string[])].sort(),
    [rows],
  )
  const competencies = useMemo(
    () => [...new Set(rows.flatMap((r) => r.competencies ?? []))].sort(),
    [rows],
  )

  const filtered = useMemo(() => {
    const needle = q.trim().toLowerCase()
    return rows.filter((r) => {
      if (track && String(r.certification_id ?? '') !== track) return false
      if (industry && r.industry !== industry) return false
      if (difficulty && (r.difficulty ?? 'foundation') !== difficulty) return false
      if (kind && r.kind !== kind) return false
      if (competency && !(r.competencies ?? []).includes(competency)) return false
      if (!matchesDuration(r.est_minutes, duration)) return false
      if (needle) {
        const hay = [r.title, r.summary, r.scenario_code, r.industry, ...(r.competencies ?? [])]
          .filter(Boolean).join(' ').toLowerCase()
        if (!hay.includes(needle)) return false
      }
      return true
    })
  }, [rows, q, track, industry, difficulty, kind, competency, duration])

  if (aLoading) return <Spinner />
  if (aError) return <ErrorNote>{aError}</ErrorNote>

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Project Controls Practice Lab</h1>
        <p className="muted">
          Practise the work of a project-controls professional on realistic, synthetic projects — build a
          WBS, analyse a schedule, run earned value, forecast, assess risk and report. A safe place to make
          mistakes; nothing here affects a real project or your examination result.
        </p>
      </div>

      {!access?.has_access ? (
        <Card>
          <Empty>{access?.reason ?? 'The Practice Lab is not currently available.'}</Empty>
        </Card>
      ) : cLoading ? (
        <Spinner />
      ) : rows.length === 0 ? (
        <Card><Empty>No practice labs have been published yet — new labs will appear here automatically.</Empty></Card>
      ) : (
        <>
          {mastery && (mastery.recommended.length > 0 || mastery.mastery.length > 0) && (
            <Card title="Your practice focus">
              <div className="stack" style={{ gap: '.5rem' }}>
                {mastery.weak_competencies.length > 0 && (
                  <div className="row small" style={{ flexWrap: 'wrap', gap: '.3rem', alignItems: 'center' }}>
                    <span className="muted">Strengthen:</span>
                    {mastery.weak_competencies.slice(0, 6).map((c) => (
                      <Badge key={c} tone="warn">{COMPETENCY_LABEL[c] ?? titleCaseWords(c)}</Badge>
                    ))}
                  </div>
                )}
                {mastery.recommended.length > 0 && (
                  <div className="stack" style={{ gap: '.35rem' }}>
                    <div className="small muted">Suggested next labs</div>
                    {mastery.recommended.slice(0, 3).map((r) => (
                      <div key={r.scenario_code} className="row" style={{ justifyContent: 'space-between', gap: '.5rem', flexWrap: 'wrap' }}>
                        <div className="small">
                          <strong>{r.title}</strong>
                          <span className="muted"> — because {r.because.map((c) => COMPETENCY_LABEL[c] ?? titleCaseWords(c)).join(', ')}</span>
                        </div>
                        <Link className="btn sm" to={`/lab/${r.scenario_code}`}>Open</Link>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </Card>
          )}

          <Card title="Find a lab">
            <div className="row" style={{ gap: '.6rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
              <label className="small" style={{ display: 'grid', gap: '.2rem', minWidth: '12rem', flex: 1 }}>
                <span className="muted">Search</span>
                <input value={q} onChange={(e) => setQ(e.target.value)} placeholder="Title, code, industry…" aria-label="Search labs" />
              </label>
              <label className="small" style={{ display: 'grid', gap: '.2rem' }}>
                <span className="muted">Track</span>
                <select value={track} onChange={(e) => setTrack(e.target.value)} aria-label="Filter by track">
                  <option value="">All tracks</option>
                  <option value="1">PCL-AI</option>
                  <option value="2">PFL-AI</option>
                  <option value="3">PML-AI</option>
                </select>
              </label>
              <label className="small" style={{ display: 'grid', gap: '.2rem' }}>
                <span className="muted">Industry</span>
                <select value={industry} onChange={(e) => setIndustry(e.target.value)} aria-label="Filter by industry">
                  <option value="">All industries</option>
                  {industries.map((i) => <option key={i} value={i}>{i}</option>)}
                </select>
              </label>
              <label className="small" style={{ display: 'grid', gap: '.2rem' }}>
                <span className="muted">Difficulty</span>
                <select value={difficulty} onChange={(e) => setDifficulty(e.target.value)} aria-label="Filter by difficulty">
                  <option value="">All levels</option>
                  <option value="foundation">Foundation</option>
                  <option value="intermediate">Intermediate</option>
                  <option value="advanced">Advanced</option>
                  <option value="expert">Expert</option>
                </select>
              </label>
              <label className="small" style={{ display: 'grid', gap: '.2rem' }}>
                <span className="muted">Kind</span>
                <select value={kind} onChange={(e) => setKind(e.target.value)} aria-label="Filter by kind">
                  <option value="">All kinds</option>
                  {Object.entries(KIND_LABEL).map(([k, label]) => <option key={k} value={k}>{label}</option>)}
                </select>
              </label>
              <label className="small" style={{ display: 'grid', gap: '.2rem' }}>
                <span className="muted">Duration</span>
                <select value={duration} onChange={(e) => setDuration(e.target.value as DurationBucket)} aria-label="Filter by duration">
                  <option value="">Any length</option>
                  <option value="short">≤ 15 min</option>
                  <option value="medium">16–25 min</option>
                  <option value="long">26+ min</option>
                </select>
              </label>
              <label className="small" style={{ display: 'grid', gap: '.2rem' }}>
                <span className="muted">Competency</span>
                <select value={competency} onChange={(e) => setCompetency(e.target.value)} aria-label="Filter by competency">
                  <option value="">All competencies</option>
                  {competencies.map((c) => (
                    <option key={c} value={c}>{COMPETENCY_LABEL[c] ?? titleCaseWords(c)}</option>
                  ))}
                </select>
              </label>
            </div>
            <div className="small muted" style={{ marginTop: '.5rem' }}>
              Showing {filtered.length} of {rows.length} labs
            </div>
          </Card>

          {filtered.length === 0 ? (
            <Card><Empty>No labs match these filters — clear a filter to see more.</Empty></Card>
          ) : (
            <div className="grid cols-2">
              {filtered.map((r) => (
                <Card
                  key={r.id}
                  title={r.title}
                  action={<Badge tone="neutral">{KIND_LABEL[r.kind] ?? titleCaseWords(r.kind)}</Badge>}
                >
                  <div className="stack small" style={{ gap: '.4rem' }}>
                    {r.summary && <div>{r.summary}</div>}
                    <div className="muted">
                      {r.difficulty ? titleCaseWords(r.difficulty) : 'Foundation'}
                      {r.industry ? ` · ${r.industry}` : ''}
                      {r.est_minutes ? ` · ~${r.est_minutes} min` : ''}
                      {r.certification_id != null && CERT_LABEL[r.certification_id]
                        ? ` · ${CERT_LABEL[r.certification_id]}`
                        : ''}
                    </div>
                    {r.competencies && r.competencies.length > 0 && (
                      <div className="row" style={{ flexWrap: 'wrap', gap: '.3rem' }}>
                        {r.competencies.map((c) => (
                          <Badge key={c} tone="brand">{COMPETENCY_LABEL[c] ?? titleCaseWords(c)}</Badge>
                        ))}
                      </div>
                    )}
                    <div className="row" style={{ marginTop: '.3rem', alignItems: 'center', gap: '.5rem' }}>
                      {r.attempt_status
                        ? <Badge tone={r.attempt_status === 'passed' || r.attempt_status === 'completed' ? 'ok' : 'warn'}>{titleCaseWords(r.attempt_status)}</Badge>
                        : <span className="muted small">Not started</span>}
                      {r.interactive !== false
                        ? <Link className="btn sm" to={`/lab/${r.scenario_code}`}>{r.attempt_status ? 'Open again' : 'Open lab'}</Link>
                        : <button className="btn sm" disabled title="Interactive workspace coming soon">Open lab</button>}
                    </div>
                  </div>
                </Card>
              ))}
            </div>
          )}
        </>
      )}

      <p className="muted small" style={{ margin: 0 }}>
        The Practice Lab is an educational simulator using synthetic project data — it is not a
        professional scheduling, ERP or accounting product, and practice never affects your formal PCI
        examination or certification.
      </p>
    </div>
  )
}
