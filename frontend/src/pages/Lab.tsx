import { useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { useQuery } from '../api/hooks'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'
import { LabHeader, KpiRow, SkeletonCards, InsightCallout } from '../components/simlab'

// PCI AI Project Controls Simulation Lab — student catalogue.
// Applied, in-portal project-controls practice (WBS, schedule, cost, EVM, forecasting, risk, reporting)
// on synthetic scenarios — distinct from Certuvo (external MCQ exam-prep) and from the real examination.
// Access is computed live from the student's existing PCI entitlement; the catalogue lists only
// published labs/drills/scenarios. The premium layer adds a live progress strip, a resume surface,
// mastery recommendations, and an accessible filter toolbar — all on the same access-gated endpoints.

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
const compLabel = (c: string) => COMPETENCY_LABEL[c] ?? titleCaseWords(c)

type DurationBucket = '' | 'short' | 'medium' | 'long'

function matchesDuration(mins: number | undefined, bucket: DurationBucket): boolean {
  if (!bucket) return true
  const m = mins ?? 0
  if (bucket === 'short') return m > 0 && m <= 15
  if (bucket === 'medium') return m > 15 && m <= 25
  return m > 25
}

const DONE = new Set(['passed', 'completed'])

/** The card meta line: difficulty · industry · duration · track, tabular and quiet. */
function metaLine(r: LabRow): string {
  const parts = [r.difficulty ? titleCaseWords(r.difficulty) : 'Foundation']
  if (r.industry) parts.push(r.industry)
  if (r.est_minutes) parts.push(`~${r.est_minutes} min`)
  if (r.certification_id != null && CERT_LABEL[r.certification_id]) parts.push(CERT_LABEL[r.certification_id])
  return parts.join(' · ')
}

export default function Lab() {
  const { data: access, loading: aLoading, error: aError } = useQuery<Access>('/api/me/lab/access')
  // Only load the catalogue once access is confirmed (the endpoint is access-gated).
  const { data: cat, loading: cLoading, error: cError, refetch: refetchCat } = useQuery<{ rows: LabRow[] }>(
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

  const rows = useMemo(() => cat?.rows ?? [], [cat])

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

  const anyFilter = !!(q || track || industry || difficulty || kind || competency || duration)
  const clearFilters = () => {
    setQ(''); setTrack(''); setIndustry(''); setDifficulty(''); setKind(''); setCompetency(''); setDuration('')
  }

  const inProgress = useMemo(() => rows.filter((r) => r.attempt_status === 'in_progress'), [rows])
  const completedCount = useMemo(() => rows.filter((r) => DONE.has(r.attempt_status ?? '')).length, [rows])
  const scored = useMemo(() => rows.filter((r) => typeof r.score === 'number'), [rows])
  const avgScore = scored.length
    ? Math.round(scored.reduce((n, r) => n + (r.score ?? 0), 0) / scored.length)
    : null

  if (aLoading) return <Spinner />
  if (aError) return <ErrorNote>{aError}</ErrorNote>

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1.15rem' }}>
      <LabHeader
        eyebrow="Simulation Lab"
        title="Project Controls Practice Lab"
        lede="Practise the work of a project-controls professional on realistic, synthetic projects — build a
          WBS, analyse a schedule, run earned value, forecast, assess risk and report. Mistakes here cost
          nothing; practice never touches a real project or your examination record."
      />

      {!access?.has_access ? (
        <Card>
          <Empty>{access?.reason ?? 'The Practice Lab is not currently available.'}</Empty>
        </Card>
      ) : cLoading ? (
        <SkeletonCards count={6} />
      ) : cError && rows.length === 0 ? (
        <InsightCallout tone="danger" title="The catalogue could not be loaded" role="alert">
          <p>Nothing you have done is lost. Check your connection, then try again.</p>
          <p style={{ marginTop: '.6rem' }}>
            <button className="btn secondary sm" type="button" onClick={refetchCat}>Try again</button>
          </p>
        </InsightCallout>
      ) : rows.length === 0 ? (
        <Card><Empty>No practice labs have been published yet — new labs will appear here automatically.</Empty></Card>
      ) : (
        <>
          <KpiRow
            items={[
              { k: 'Published labs', v: rows.length },
              { k: 'Labs completed', v: completedCount },
              { k: 'In progress', v: inProgress.length },
              ...(avgScore != null ? [{ k: 'Average score', v: avgScore, suffix: '%' }] : []),
            ]}
          />

          {inProgress.length > 0 && (
            <Card title="Continue where you left off">
              <div className="sl-resume">
                {inProgress.slice(0, 3).map((r) => (
                  <div key={r.scenario_code} className="sl-resume-item">
                    <div style={{ minWidth: 0 }}>
                      <strong>{r.title}</strong>
                      <div className="small muted">{metaLine(r)}</div>
                    </div>
                    <Link className="btn sm" to={`/lab/${r.scenario_code}`}>Resume scenario</Link>
                  </div>
                ))}
              </div>
            </Card>
          )}

          {mastery && (mastery.recommended.length > 0 || mastery.weak_competencies.length > 0) && (
            <Card title="Your practice focus">
              <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
                {mastery.weak_competencies.length > 0 && (
                  <div className="row small" style={{ flexWrap: 'wrap', gap: '.3rem', alignItems: 'center' }}>
                    <span className="muted">Strengthen:</span>
                    {mastery.weak_competencies.slice(0, 6).map((c) => (
                      <Badge key={c} tone="warn">{compLabel(c)}</Badge>
                    ))}
                  </div>
                )}
                {mastery.recommended.length > 0 && (
                  <div className="stack" style={{ display: 'grid', gap: '.45rem' }}>
                    <div className="small muted">Suggested next labs</div>
                    {mastery.recommended.slice(0, 3).map((r) => (
                      <div key={r.scenario_code} className="row" style={{ justifyContent: 'space-between', gap: '.5rem', flexWrap: 'wrap' }}>
                        <div className="small" style={{ minWidth: 0 }}>
                          <strong>{r.title}</strong>
                          <span className="muted"> — because {r.because.map(compLabel).join(', ')}</span>
                        </div>
                        <Link className="btn sm secondary" to={`/lab/${r.scenario_code}`}>Open</Link>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </Card>
          )}

          <section className="sl-toolbar" aria-label="Find a lab">
            <div className="sl-toolbar-row">
              <label className="sl-search">
                <span>Search</span>
                <input value={q} onChange={(e) => setQ(e.target.value)} placeholder="Title, code, industry…" aria-label="Search labs" />
              </label>
              <label>
                <span>Track</span>
                <select value={track} onChange={(e) => setTrack(e.target.value)} aria-label="Filter by track">
                  <option value="">All tracks</option>
                  <option value="1">PCL-AI</option>
                  <option value="2">PFL-AI</option>
                  <option value="3">PML-AI</option>
                </select>
              </label>
              <label>
                <span>Industry</span>
                <select value={industry} onChange={(e) => setIndustry(e.target.value)} aria-label="Filter by industry">
                  <option value="">All industries</option>
                  {industries.map((i) => <option key={i} value={i}>{i}</option>)}
                </select>
              </label>
              <label>
                <span>Difficulty</span>
                <select value={difficulty} onChange={(e) => setDifficulty(e.target.value)} aria-label="Filter by difficulty">
                  <option value="">All levels</option>
                  <option value="foundation">Foundation</option>
                  <option value="intermediate">Intermediate</option>
                  <option value="advanced">Advanced</option>
                  <option value="expert">Expert</option>
                </select>
              </label>
              <label>
                <span>Kind</span>
                <select value={kind} onChange={(e) => setKind(e.target.value)} aria-label="Filter by kind">
                  <option value="">All kinds</option>
                  {Object.entries(KIND_LABEL).map(([k, label]) => <option key={k} value={k}>{label}</option>)}
                </select>
              </label>
              <label>
                <span>Duration</span>
                <select value={duration} onChange={(e) => setDuration(e.target.value as DurationBucket)} aria-label="Filter by duration">
                  <option value="">Any length</option>
                  <option value="short">≤ 15 min</option>
                  <option value="medium">16–25 min</option>
                  <option value="long">26+ min</option>
                </select>
              </label>
              <label>
                <span>Competency</span>
                <select value={competency} onChange={(e) => setCompetency(e.target.value)} aria-label="Filter by competency">
                  <option value="">All competencies</option>
                  {competencies.map((c) => (
                    <option key={c} value={c}>{compLabel(c)}</option>
                  ))}
                </select>
              </label>
            </div>
            <p className="sl-results" role="status">
              Showing {filtered.length} of {rows.length} labs
              {anyFilter && (
                <>
                  {' · '}
                  <button type="button" className="sl-clear" onClick={clearFilters}>Clear all filters</button>
                </>
              )}
            </p>
          </section>

          {filtered.length === 0 ? (
            <InsightCallout tone="info" title="No labs match these filters">
              <p>Widen the search or clear a filter to see more of the catalogue.</p>
              <p style={{ marginTop: '.6rem' }}>
                <button className="btn secondary sm" type="button" onClick={clearFilters}>Clear all filters</button>
              </p>
            </InsightCallout>
          ) : (
            <div className="sl-cards">
              {filtered.map((r) => {
                const done = DONE.has(r.attempt_status ?? '')
                return (
                  <article className="sl-scenario" key={r.id}>
                    <div className="sl-scenario-top">
                      <span className="sl-kind">{KIND_LABEL[r.kind] ?? titleCaseWords(r.kind)}</span>
                      {r.attempt_status
                        ? <Badge tone={done ? 'ok' : 'warn'}>{titleCaseWords(r.attempt_status)}</Badge>
                        : <span className="muted small">Not started</span>}
                    </div>
                    <h3>{r.title}</h3>
                    {r.summary && <p className="sl-summary">{r.summary}</p>}
                    {r.competencies && r.competencies.length > 0 && (
                      <div className="sl-comps">
                        {r.competencies.slice(0, 4).map((c) => (
                          <span key={c} className="sl-comp">{compLabel(c)}</span>
                        ))}
                        {r.competencies.length > 4 && (
                          <span className="sl-comp" title={r.competencies.slice(4).map(compLabel).join(', ')}>
                            +{r.competencies.length - 4} more
                          </span>
                        )}
                      </div>
                    )}
                    <div className="sl-meta">{metaLine(r)}</div>
                    <div className="sl-scenario-foot">
                      <span className="sl-score">
                        {typeof r.score === 'number' ? `Score ${r.score}%` : ''}
                      </span>
                      {r.interactive !== false
                        ? <Link className="btn sm" to={`/lab/${r.scenario_code}`}>{r.attempt_status ? 'Open again' : 'Open lab'}</Link>
                        : (
                          <span className="sl-status-line">
                            <button className="btn sm" disabled aria-describedby={`na-${r.id}`}>Open lab</button>
                            <span id={`na-${r.id}`} className="muted small">Interactive workspace coming soon</span>
                          </span>
                        )}
                    </div>
                  </article>
                )
              })}
            </div>
          )}
        </>
      )}

      <p className="sl-footnote">
        The Practice Lab is an educational simulator using synthetic project data — it is not a
        professional scheduling, ERP or accounting product, and practice never affects your formal PCI
        examination or certification.
      </p>
    </div>
  )
}
