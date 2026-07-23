import { Link } from 'react-router-dom'
import { useQuery } from '../api/hooks'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'

// PCI AI Project Controls Simulation Lab — student surface (Phase 1 foundation).
// Applied, in-portal project-controls practice (WBS, schedule, cost, EVM, forecasting, risk, reporting)
// on synthetic scenarios — distinct from Certuvo (external MCQ exam-prep) and from the real examination.
// Access is computed live from the student's existing PCI entitlement; the catalogue lists only
// published labs/drills/scenarios. This foundation renders access + catalogue; interactive workspaces
// arrive in later Phase-1/2 increments.

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

const KIND_LABEL: Record<string, string> = {
  guided_lab: 'Guided lab',
  skill_drill: 'Skill drill',
  scenario: 'Scenario',
  capstone: 'Capstone',
  team: 'Team',
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
}
const titleCaseWords = (s: string) => s.replace(/[_-]+/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())

export default function Lab() {
  const { data: access, loading: aLoading, error: aError } = useQuery<Access>('/api/me/lab/access')
  // Only load the catalogue once access is confirmed (the endpoint is access-gated).
  const { data: cat, loading: cLoading } = useQuery<{ rows: LabRow[] }>(
    access?.has_access ? '/api/me/lab/catalogue' : null,
  )

  if (aLoading) return <Spinner />
  if (aError) return <ErrorNote>{aError}</ErrorNote>

  const rows = cat?.rows ?? []

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
        <div className="grid cols-2">
          {rows.map((r) => (
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

      <p className="muted small" style={{ margin: 0 }}>
        The Practice Lab is an educational simulator using synthetic project data — it is not a
        professional scheduling, ERP or accounting product, and practice never affects your formal PCI
        examination or certification.
      </p>
    </div>
  )
}
