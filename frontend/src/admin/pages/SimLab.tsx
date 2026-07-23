import { useAdminQuery } from '../hooks'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty, Stat } from '../../components/ui'

// Admin Console → Simulation Lab (Phase 1 foundation). A read-only operator view of the guided-lab /
// scenario catalogue across all statuses, with per-scenario practice aggregates (attempts / completions).
// Authoring & publishing lifecycle arrive in a later increment; this gives operators visibility now.

interface ScenarioRow {
  id: number
  scenario_code: string
  title: string
  kind: string
  industry?: string | null
  difficulty?: string | null
  est_minutes?: number
  competencies: string[]
  status: string
  version: number
  interactive: boolean
  attempts: number
  completed: number
  updated_at?: string | null
}
interface Resp { rows: ScenarioRow[]; total: number; published: number }

const titleCase = (s: string) => s.replace(/[_-]+/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())

export default function SimLab() {
  const { data, loading, error } = useAdminQuery<Resp>('/api/admin/lab/scenarios')
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  const rows = data?.rows ?? []
  const totalAttempts = rows.reduce((n, r) => n + (r.attempts || 0), 0)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1 style={{ marginBottom: '.2rem' }}>Simulation Lab</h1>
        <p className="muted" style={{ marginTop: 0 }}>
          The applied project-controls practice catalogue. Students run these guided labs on synthetic
          scenarios; grading is deterministic and practice is kept entirely separate from formal exam records.
        </p>
      </div>

      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap' }}>
        <Stat n={data?.total ?? 0} k="Scenarios" />
        <Stat n={data?.published ?? 0} k="Published" />
        <Stat n={totalAttempts} k="Attempts" />
      </div>

      <Card title="Scenarios">
        {rows.length === 0 ? (
          <Empty>No scenarios have been created yet.</Empty>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead>
                <tr>
                  <th>Code</th><th>Title</th><th>Kind</th><th>Difficulty</th>
                  <th>Competencies</th><th>Status</th><th>Interactive</th>
                  <th style={{ textAlign: 'right' }}>Attempts</th><th style={{ textAlign: 'right' }}>Completed</th>
                </tr>
              </thead>
              <tbody>
                {rows.map((r) => (
                  <tr key={r.id}>
                    <td style={{ fontVariantNumeric: 'tabular-nums' }}>{r.scenario_code}</td>
                    <td>{r.title}</td>
                    <td>{titleCase(r.kind)}</td>
                    <td>{r.difficulty ? titleCase(r.difficulty) : '—'}</td>
                    <td>
                      <div className="row" style={{ flexWrap: 'wrap', gap: '.2rem' }}>
                        {r.competencies.length ? r.competencies.map((c) => (
                          <Badge key={c} tone="neutral">{titleCase(c)}</Badge>
                        )) : '—'}
                      </div>
                    </td>
                    <td><StatusBadge status={r.status} /></td>
                    <td>{r.interactive ? <Badge tone="ok">Yes</Badge> : <Badge tone="warn">No</Badge>}</td>
                    <td style={{ textAlign: 'right' }}>{r.attempts}</td>
                    <td style={{ textAlign: 'right' }}>{r.completed}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </Card>
    </div>
  )
}
