import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import { useQuery } from '../api/hooks'
import { useI18n } from '../i18n'
import { fmtDateTime } from '../format'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'
import {
  LabHeader, KpiRow, SkeletonCards, InsightCallout, SidePanel, CompetencyMeter,
} from '../components/simlab'

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
interface AttemptRow {
  id: number
  mode: string
  status: string
  score?: number | null
  scenario_code: string
  title: string
  kind: string
  started_at?: string | null
  completed_at?: string | null
}
// A compact in-progress row for the resume strip (the server computes it over the FULL catalogue, so
// it survives an active filter). Carries just what the strip and metaLine need.
interface ResumeRow {
  scenario_code: string
  title: string
  kind: string
  difficulty?: string | null
  industry?: string | null
  est_minutes?: number
  certification_id?: number | null
}
// The catalogue response: matched rows for the cards, full-set facets for the filter controls, and a
// whole-catalogue summary (KPIs) + resume list that are unaffected by the active filter. The client
// asks the server to filter/search (the capacity path) and derives only track/duration/sort locally.
interface CatalogueResp {
  rows: LabRow[]
  total: number
  matched: number
  facets?: { difficulties: string[]; kinds: string[]; industries: string[]; competencies: string[] }
  summary?: { published: number; completed: number; in_progress: number; avg_score: number | null }
  resume?: ResumeRow[]
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
const DIFF_RANK: Record<string, number> = { foundation: 0, intermediate: 1, advanced: 2, expert: 3 }

type SortKey = '' | 'title' | 'difficulty' | 'shortest' | 'longest'

function sortRows(rows: LabRow[], sort: SortKey): LabRow[] {
  if (!sort) return rows // recommended: the server's curated order
  const out = [...rows]
  if (sort === 'title') out.sort((a, b) => a.title.localeCompare(b.title))
  else if (sort === 'difficulty') out.sort((a, b) => (DIFF_RANK[a.difficulty ?? 'foundation'] ?? 0) - (DIFF_RANK[b.difficulty ?? 'foundation'] ?? 0))
  else if (sort === 'shortest') out.sort((a, b) => (a.est_minutes ?? 0) - (b.est_minutes ?? 0))
  else if (sort === 'longest') out.sort((a, b) => (b.est_minutes ?? 0) - (a.est_minutes ?? 0))
  return out
}

/** The card meta line: difficulty · industry · duration · track, tabular and quiet. */
function metaLine(r: Pick<LabRow, 'difficulty' | 'industry' | 'est_minutes' | 'certification_id'>): string {
  const parts = [r.difficulty ? titleCaseWords(r.difficulty) : 'Foundation']
  if (r.industry) parts.push(r.industry)
  if (r.est_minutes) parts.push(`~${r.est_minutes} min`)
  if (r.certification_id != null && CERT_LABEL[r.certification_id]) parts.push(CERT_LABEL[r.certification_id])
  return parts.join(' · ')
}

export default function Lab() {
  const { lang } = useI18n()
  const { data: access, loading: aLoading, error: aError } = useQuery<Access>('/api/me/lab/access')

  // Filters live in the URL so a filtered view survives reload, back/forward and sharing.
  const [params, setParams] = useSearchParams()
  const setParam = useCallback((k: string, v: string) => {
    setParams((prev) => {
      const p = new URLSearchParams(prev)
      if (v) p.set(k, v)
      else p.delete(k)
      return p
    }, { replace: true })
  }, [setParams])
  const q = params.get('q') ?? ''
  const track = params.get('track') ?? ''
  const industry = params.get('industry') ?? ''
  const difficulty = params.get('difficulty') ?? ''
  const kind = params.get('kind') ?? ''
  const competency = params.get('competency') ?? ''
  const duration = (params.get('duration') ?? '') as DurationBucket
  const sort = (params.get('sort') ?? '') as SortKey

  // Debounce the free-text search so we refetch once the student pauses, not on every keystroke.
  const [qDebounced, setQDebounced] = useState(q)
  useEffect(() => {
    const t = setTimeout(() => setQDebounced(q), 250)
    return () => clearTimeout(t)
  }, [q])

  // The catalogue is filtered/searched SERVER-side (the capacity path — the browser never has to hold
  // or scan the whole set): the search text + the facet filters the endpoint understands go in the
  // query string, so changing them refetches the matched rows. Track/duration/sort stay client-side
  // (the server doesn't model them and they act on the small matched set). Only load once access is
  // confirmed — the endpoint is access-gated.
  const catPath = useMemo(() => {
    if (!access?.has_access) return null
    const p = new URLSearchParams()
    if (qDebounced.trim()) p.set('q', qDebounced.trim())
    if (difficulty) p.set('difficulty', difficulty)
    if (kind) p.set('kind', kind)
    if (industry) p.set('industry', industry)
    if (competency) p.set('competency', competency)
    if (lang !== 'en') p.set('lang', lang)   // scenario titles/summaries in the portal language
    const qs = p.toString()
    return qs ? `/api/me/lab/catalogue?${qs}` : '/api/me/lab/catalogue'
  }, [access?.has_access, qDebounced, difficulty, kind, industry, competency, lang])

  const { data: cat, loading: cLoading, error: cError, refetch: refetchCat } = useQuery<CatalogueResp>(catPath)
  const { data: mastery } = useQuery<MasteryResp>(
    access?.has_access ? '/api/me/lab/mastery' : null,
  )
  const { data: history } = useQuery<{ rows: AttemptRow[] }>(
    access?.has_access ? (lang !== 'en' ? `/api/me/lab/attempts?lang=${lang}` : '/api/me/lab/attempts') : null,
  )

  const [detail, setDetail] = useState<LabRow | null>(null)
  const [allHistory, setAllHistory] = useState(false)

  const rows = useMemo(() => cat?.rows ?? [], [cat])

  // Facet dropdowns come from the server's full-set facets (stable regardless of the active filter),
  // falling back to whatever the matched rows expose if an older response omits them.
  const industries = useMemo(
    () => cat?.facets?.industries ?? [...new Set(rows.map((r) => r.industry).filter(Boolean) as string[])].sort(),
    [cat, rows],
  )
  const competencies = useMemo(
    () => cat?.facets?.competencies ?? [...new Set(rows.flatMap((r) => r.competencies ?? []))].sort(),
    [cat, rows],
  )

  // The rows are already server-filtered/searched; apply only the client-side track + duration filters
  // and the chosen sort over the matched set.
  const filtered = useMemo(() => {
    return sortRows(rows.filter((r) => {
      if (track && String(r.certification_id ?? '') !== track) return false
      if (!matchesDuration(r.est_minutes, duration)) return false
      return true
    }), sort)
  }, [rows, track, duration, sort])

  const anyFilter = !!(q || track || industry || difficulty || kind || competency || duration)
  const clearFilters = () => {
    setParams((prev) => {
      const p = new URLSearchParams(prev)
      for (const k of ['q', 'track', 'industry', 'difficulty', 'kind', 'competency', 'duration']) p.delete(k)
      return p
    }, { replace: true })
  }

  // Whole-catalogue KPIs + resume list come from the server summary (unaffected by the active filter).
  const totalPublished = cat?.summary?.published ?? cat?.total ?? 0
  const completedCount = cat?.summary?.completed ?? 0
  const inProgressCount = cat?.summary?.in_progress ?? 0
  const avgScore = cat?.summary?.avg_score ?? null
  const inProgress = useMemo(() => cat?.resume ?? [], [cat])

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
      ) : cLoading && !cat ? (
        <SkeletonCards count={6} />
      ) : cError && !cat ? (
        <InsightCallout tone="danger" title="The catalogue could not be loaded" role="alert">
          <p>Nothing you have done is lost. Check your connection, then try again.</p>
          <p style={{ marginTop: '.6rem' }}>
            <button className="btn secondary sm" type="button" onClick={refetchCat}>Try again</button>
          </p>
        </InsightCallout>
      ) : totalPublished === 0 ? (
        <Card><Empty>No practice labs have been published yet — new labs will appear here automatically.</Empty></Card>
      ) : (
        <>
          <KpiRow
            items={[
              { k: 'Published labs', v: totalPublished },
              { k: 'Labs completed', v: completedCount },
              { k: 'In progress', v: inProgressCount },
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

          {mastery && (mastery.recommended.length > 0 || mastery.weak_competencies.length > 0 || mastery.mastery.length > 0) && (
            <Card title="Your practice focus">
              <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
                {mastery.mastery.length > 0 && (
                  <div className="sl-meters">
                    {mastery.mastery.slice(0, 8).map((m) => (
                      <CompetencyMeter
                        key={m.competency}
                        label={compLabel(m.competency)}
                        score={Math.round(m.avg_score)}
                        detail={`${m.attempts} attempt${m.attempts === 1 ? '' : 's'}${m.level ? ` · ${titleCaseWords(m.level)}` : ''}`}
                      />
                    ))}
                  </div>
                )}
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
                <input value={q} onChange={(e) => setParam('q', e.target.value)} placeholder="Title, code, industry…" aria-label="Search labs" />
              </label>
              <label>
                <span>Track</span>
                <select value={track} onChange={(e) => setParam('track', e.target.value)} aria-label="Filter by track">
                  <option value="">All tracks</option>
                  <option value="1">PCL-AI</option>
                  <option value="2">PFL-AI</option>
                  <option value="3">PML-AI</option>
                </select>
              </label>
              <label>
                <span>Industry</span>
                <select value={industry} onChange={(e) => setParam('industry', e.target.value)} aria-label="Filter by industry">
                  <option value="">All industries</option>
                  {industries.map((i) => <option key={i} value={i}>{i}</option>)}
                </select>
              </label>
              <label>
                <span>Difficulty</span>
                <select value={difficulty} onChange={(e) => setParam('difficulty', e.target.value)} aria-label="Filter by difficulty">
                  <option value="">All levels</option>
                  <option value="foundation">Foundation</option>
                  <option value="intermediate">Intermediate</option>
                  <option value="advanced">Advanced</option>
                  <option value="expert">Expert</option>
                </select>
              </label>
              <label>
                <span>Kind</span>
                <select value={kind} onChange={(e) => setParam('kind', e.target.value)} aria-label="Filter by kind">
                  <option value="">All kinds</option>
                  {Object.entries(KIND_LABEL).map(([k, label]) => <option key={k} value={k}>{label}</option>)}
                </select>
              </label>
              <label>
                <span>Duration</span>
                <select value={duration} onChange={(e) => setParam('duration', e.target.value)} aria-label="Filter by duration">
                  <option value="">Any length</option>
                  <option value="short">≤ 15 min</option>
                  <option value="medium">16–25 min</option>
                  <option value="long">26+ min</option>
                </select>
              </label>
              <label>
                <span>Competency</span>
                <select value={competency} onChange={(e) => setParam('competency', e.target.value)} aria-label="Filter by competency">
                  <option value="">All competencies</option>
                  {competencies.map((c) => (
                    <option key={c} value={c}>{compLabel(c)}</option>
                  ))}
                </select>
              </label>
              <label>
                <span>Sort</span>
                <select value={sort} onChange={(e) => setParam('sort', e.target.value)} aria-label="Sort labs">
                  <option value="">Recommended</option>
                  <option value="title">Title A–Z</option>
                  <option value="difficulty">Easiest first</option>
                  <option value="shortest">Shortest first</option>
                  <option value="longest">Longest first</option>
                </select>
              </label>
            </div>
            <p className="sl-results" role="status">
              Showing {filtered.length} of {totalPublished} labs
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
                      <span className="sl-status-line">
                        <button className="btn ghost sm" type="button" onClick={() => setDetail(r)} aria-label={`Details — ${r.title}`}>
                          Details
                        </button>
                        {r.interactive !== false
                          ? <Link className="btn sm" to={`/lab/${r.scenario_code}`}>{r.attempt_status ? 'Open again' : 'Open lab'}</Link>
                          : (
                            <>
                              <button className="btn sm" disabled aria-describedby={`na-${r.id}`}>Open lab</button>
                              <span id={`na-${r.id}`} className="muted small">Interactive workspace coming soon</span>
                            </>
                          )}
                      </span>
                    </div>
                  </article>
                )
              })}
            </div>
          )}

          {history && history.rows.length > 0 && (
            <Card title="Recent attempts">
              <div style={{ overflowX: 'auto' }}>
                <table className="data sl-history">
                  <thead>
                    <tr>
                      <th className="t">Scenario</th><th>Mode</th><th>Status</th>
                      <th style={{ textAlign: 'end' }}>Score</th><th>When</th><th>Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {(allHistory ? history.rows : history.rows.slice(0, 8)).map((a) => (
                      <tr key={a.id}>
                        <td className="t">{a.title}</td>
                        <td>{a.mode === 'assessment' ? 'Assessment' : 'Training'}</td>
                        <td>
                          <Badge tone={DONE.has(a.status) ? 'ok' : a.status === 'failed' ? 'warn' : 'neutral'}>
                            {titleCaseWords(a.status)}
                          </Badge>
                        </td>
                        <td style={{ textAlign: 'end', fontVariantNumeric: 'tabular-nums' }}>
                          {typeof a.score === 'number' ? `${a.score}%` : '—'}
                        </td>
                        <td className="muted">{fmtDateTime(a.completed_at ?? a.started_at)}</td>
                        <td>
                          {a.status === 'in_progress'
                            ? <Link className="btn sm secondary" to={`/lab/${a.scenario_code}`}>Resume</Link>
                            : <Link className="btn sm secondary" to={`/lab/${a.scenario_code}?attempt=${a.id}`}>Review</Link>}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              {history.rows.length > 8 && (
                <p style={{ margin: '.6rem 0 0' }}>
                  <button type="button" className="sl-clear" onClick={() => setAllHistory((v) => !v)}>
                    {allHistory ? 'Show fewer attempts' : `Show all ${history.rows.length} attempts`}
                  </button>
                </p>
              )}
            </Card>
          )}
        </>
      )}

      <SidePanel
        open={detail !== null}
        eyebrow={detail ? (KIND_LABEL[detail.kind] ?? titleCaseWords(detail.kind)) : undefined}
        title={detail?.title ?? ''}
        onClose={() => setDetail(null)}
      >
        {detail && (
          <>
            {detail.summary && <p style={{ margin: 0 }}>{detail.summary}</p>}
            <dl>
              <dt>Code</dt><dd>{detail.scenario_code}</dd>
              <dt>Difficulty</dt><dd>{detail.difficulty ? titleCaseWords(detail.difficulty) : 'Foundation'}</dd>
              {detail.industry && <><dt>Industry</dt><dd>{detail.industry}</dd></>}
              {detail.est_minutes ? <><dt>Duration</dt><dd>~{detail.est_minutes} min</dd></> : null}
              {detail.certification_id != null && CERT_LABEL[detail.certification_id] && (
                <><dt>Track</dt><dd>{CERT_LABEL[detail.certification_id]}</dd></>
              )}
              <dt>Your status</dt>
              <dd>
                {detail.attempt_status ? titleCaseWords(detail.attempt_status) : 'Not started'}
                {typeof detail.score === 'number' ? ` · ${detail.score}%` : ''}
              </dd>
            </dl>
            {detail.competencies && detail.competencies.length > 0 && (
              <div className="sl-comps">
                {detail.competencies.map((c) => <span key={c} className="sl-comp">{compLabel(c)}</span>)}
              </div>
            )}
            <InsightCallout tone="info" title="Modes">
              <p>
                Training grades your work and then reveals the worked answers, with the Coach available
                throughout. Assessment reports marks only and withholds both the answers and the Coach.
              </p>
            </InsightCallout>
            <div className="sl-panel-actions">
              {detail.interactive !== false ? (
                <>
                  <Link className="btn" to={`/lab/${detail.scenario_code}`} onClick={() => setDetail(null)}>
                    {detail.attempt_status === 'in_progress' ? 'Resume scenario' : 'Start in Training Mode'}
                  </Link>
                  <Link className="btn secondary" to={`/lab/${detail.scenario_code}?mode=assessment`} onClick={() => setDetail(null)}>
                    Start in Assessment Mode
                  </Link>
                </>
              ) : (
                <p className="sl-footnote">The interactive workspace for this lab is not available yet.</p>
              )}
            </div>
          </>
        )}
      </SidePanel>

      <p className="sl-footnote">
        The Practice Lab is an educational simulator using synthetic project data — it is not a
        professional scheduling, ERP or accounting product, and practice never affects your formal PCI
        examination or certification.
      </p>
    </div>
  )
}
