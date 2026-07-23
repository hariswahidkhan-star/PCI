import { useState, type FormEvent } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty, Stat } from '../../components/ui'

// Admin Console → Simulation Lab Studio (Phase 5A). A governed authoring surface over the deterministic
// scenario engine: create a draft, validate it against the §14 publication gate (the reference solver runs
// every asked measure through the engine), walk it through the review workflow (draft → calc_review →
// learning_review → safety_review → pilot → approved → published), and revise a published version into a new
// draft. Approval is maker-checker (the approver must differ from the author) and approve/publish are blocked
// unless the validator passes — all enforced server-side; this page just drives it. Practice stays entirely
// separate from formal exam records, and no answer key is ever shown.

interface ScenarioRow {
  id: number
  scenario_code: string
  title: string
  kind: string
  industry?: string | null
  difficulty?: string | null
  competencies: string[]
  status: string
  review_state: string
  version: number
  interactive: boolean
  attempts: number
  completed: number
}
interface Resp { rows: ScenarioRow[]; total: number; published: number }
interface Issue { severity: string; code: string; message: string }
interface Validation { id: number; scenario_code: string; review_state: string; publishable: boolean; errors: number; warnings: number; issues: Issue[] }

const titleCase = (s: string) => s.replace(/[_-]+/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())

// The authoring workflow, in order. `nextState` is the single forward step the "Advance" button offers.
const FORWARD = ['draft', 'calc_review', 'learning_review', 'safety_review', 'pilot', 'approved', 'published']
const nextState = (s: string): string | null => {
  const i = FORWARD.indexOf(s)
  return i >= 0 && i + 1 < FORWARD.length ? FORWARD[i + 1] : null
}
const reviewTone = (s: string) => (s === 'published' ? 'ok' : s === 'draft' ? 'neutral' : s === 'retired' ? 'warn' : 'brand')

export default function SimLab() {
  const { data, loading, error, refetch } = useAdminQuery<Resp>('/api/admin/lab/scenarios')
  const [msg, setMsg] = useState('')
  const [err, setErr] = useState('')
  const [busy, setBusy] = useState(false)
  const [showCreate, setShowCreate] = useState(false)
  const [validation, setValidation] = useState<Validation | null>(null)
  // create-form fields
  const [code, setCode] = useState('')
  const [title, setTitle] = useState('')
  const [difficulty, setDifficulty] = useState('foundation')
  const [cert, setCert] = useState('1')
  const [comps, setComps] = useState('earned_value')
  const [config, setConfig] = useState('')
  const [synthetic, setSynthetic] = useState(true)

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  const rows = data?.rows ?? []
  const totalAttempts = rows.reduce((n, r) => n + (r.attempts || 0), 0)
  const inReview = rows.filter((r) => r.review_state !== 'draft' && r.review_state !== 'published' && r.review_state !== 'retired').length

  const flash = (m: string) => { setMsg(m); setErr(''); setValidation(null) }
  const fail = (e: unknown) => { setErr(e instanceof Error ? e.message : 'Something went wrong.'); setMsg('') }

  async function doCreate(e: FormEvent) {
    e.preventDefault()
    setBusy(true); setErr(''); setMsg('')
    try {
      const body = {
        scenario_code: code.trim(),
        title: title.trim(),
        difficulty,
        certification_id: cert ? Number(cert) : undefined,
        competencies: comps.split(',').map((s) => s.trim()).filter(Boolean),
        config_json: config.trim() || undefined,
        synthetic_declared: synthetic,
      }
      await adminApi.post('/api/admin/lab/scenarios', body)
      flash(`Draft scenario “${code.trim()}” created.`)
      setCode(''); setTitle(''); setConfig(''); setShowCreate(false)
      refetch()
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  async function doValidate(id: number) {
    setBusy(true); setErr(''); setMsg('')
    try {
      const v = await adminApi.get<Validation>(`/api/admin/lab/scenarios/${id}/validate`)
      setValidation(v)
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  async function doReview(id: number, to: string) {
    setBusy(true); setErr(''); setMsg('')
    try {
      await adminApi.post(`/api/admin/lab/scenarios/${id}/review`, { to })
      flash(`Moved to ${titleCase(to)}.`)
      refetch()
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  async function doRevise(id: number, fromCode: string) {
    const newCode = window.prompt(`New scenario code for the revision of ${fromCode}:`, `${fromCode}-v2`)
    if (!newCode) return
    setBusy(true); setErr(''); setMsg('')
    try {
      await adminApi.post(`/api/admin/lab/scenarios/${id}/revise`, { new_code: newCode.trim() })
      flash(`Revised into new draft “${newCode.trim()}”.`)
      refetch()
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  const frozen = (s: string) => s === 'approved' || s === 'published' || s === 'retired'

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1 style={{ marginBottom: '.2rem' }}>Simulation Lab Studio</h1>
        <p className="muted" style={{ marginTop: 0 }}>
          Author, validate and publish applied project-controls scenarios. Grading is deterministic and a
          scenario can only be approved or published once the validator confirms the engine can grade every
          measure. Practice is kept entirely separate from formal exam records.
        </p>
      </div>

      {msg && <div className="notice ok" role="status">{msg}</div>}
      {err && <div className="notice err" role="alert">{err}</div>}

      <div className="row" style={{ gap: '1rem', flexWrap: 'wrap', alignItems: 'center' }}>
        <Stat n={data?.total ?? 0} k="Scenarios" />
        <Stat n={data?.published ?? 0} k="Published" />
        <Stat n={inReview} k="In review" />
        <Stat n={totalAttempts} k="Attempts" />
        <button className="btn" style={{ marginLeft: 'auto' }} onClick={() => setShowCreate((v) => !v)}>
          {showCreate ? 'Cancel' : '+ New scenario'}
        </button>
      </div>

      {showCreate && (
        <Card title="New draft scenario">
          <form onSubmit={doCreate} className="stack" style={{ display: 'grid', gap: '.6rem', maxWidth: 640 }}>
            <label>Scenario code
              <input value={code} onChange={(e) => setCode(e.target.value)} required placeholder="GL-EVM-010" />
            </label>
            <label>Title
              <input value={title} onChange={(e) => setTitle(e.target.value)} required placeholder="Calculate the core EVM measures" />
            </label>
            <div className="row" style={{ gap: '.6rem', flexWrap: 'wrap' }}>
              <label>Difficulty
                <select value={difficulty} onChange={(e) => setDifficulty(e.target.value)}>
                  <option value="foundation">Foundation</option>
                  <option value="intermediate">Intermediate</option>
                  <option value="advanced">Advanced</option>
                  <option value="expert">Expert</option>
                </select>
              </label>
              <label>Certification
                <select value={cert} onChange={(e) => setCert(e.target.value)}>
                  <option value="1">PCL-AI</option>
                  <option value="2">PFL-AI</option>
                  <option value="3">PML-AI</option>
                  <option value="">Any</option>
                </select>
              </label>
            </div>
            <label>Competencies (comma-separated)
              <input value={comps} onChange={(e) => setComps(e.target.value)} placeholder="earned_value, forecasting" />
            </label>
            <label>Task definition (config JSON)
              <textarea value={config} onChange={(e) => setConfig(e.target.value)} rows={6}
                placeholder='{"task":"evm","prompt":"…","given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},"ask":[{"key":"cpi","type":"number"}]}'
                style={{ fontFamily: 'monospace', width: '100%' }} />
            </label>
            <label className="row" style={{ gap: '.4rem', alignItems: 'center' }}>
              <input type="checkbox" checked={synthetic} onChange={(e) => setSynthetic(e.target.checked)} />
              Synthetic data only (required to publish)
            </label>
            <div>
              <button className="btn primary" type="submit" disabled={busy}>Create draft</button>
            </div>
          </form>
        </Card>
      )}

      {validation && (
        <Card title={`Validation — ${validation.scenario_code}`}>
          <div className="row" style={{ gap: '.5rem', alignItems: 'center', marginBottom: '.4rem' }}>
            {validation.publishable
              ? <Badge tone="ok">Publishable</Badge>
              : <Badge tone="warn">Not publishable</Badge>}
            <span className="muted">{validation.errors} error(s), {validation.warnings} warning(s)</span>
          </div>
          {validation.issues.length === 0
            ? <Empty>No issues — this scenario passes every check.</Empty>
            : (
              <ul style={{ margin: 0, paddingLeft: '1.1rem' }}>
                {validation.issues.map((i, n) => (
                  <li key={n}>
                    <Badge tone={i.severity === 'error' ? 'warn' : 'neutral'}>{i.severity}</Badge>{' '}
                    <code>{i.code}</code> — {i.message}
                  </li>
                ))}
              </ul>
            )}
        </Card>
      )}

      <Card title="Scenarios">
        {rows.length === 0 ? (
          <Empty>No scenarios have been created yet.</Empty>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead>
                <tr>
                  <th>Code</th><th>Title</th><th>Difficulty</th><th>Status</th><th>Review state</th>
                  <th style={{ textAlign: 'right' }}>Attempts</th><th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {rows.map((r) => {
                  const nxt = nextState(r.review_state)
                  return (
                    <tr key={r.id}>
                      <td style={{ fontVariantNumeric: 'tabular-nums' }}>{r.scenario_code}</td>
                      <td>{r.title}</td>
                      <td>{r.difficulty ? titleCase(r.difficulty) : '—'}</td>
                      <td><StatusBadge status={r.status} /></td>
                      <td><Badge tone={reviewTone(r.review_state)}>{titleCase(r.review_state)}</Badge></td>
                      <td style={{ textAlign: 'right' }}>{r.attempts}</td>
                      <td>
                        <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                          <button className="btn sm" disabled={busy} onClick={() => doValidate(r.id)}>Validate</button>
                          {nxt && (
                            <button className="btn sm" disabled={busy} onClick={() => doReview(r.id, nxt)}>
                              → {titleCase(nxt)}
                            </button>
                          )}
                          {r.review_state !== 'draft' && r.review_state !== 'published' && r.review_state !== 'retired' && (
                            <button className="btn sm secondary" disabled={busy} onClick={() => doReview(r.id, 'draft')}>Return to draft</button>
                          )}
                          {r.review_state === 'published' && (
                            <button className="btn sm secondary" disabled={busy} onClick={() => doReview(r.id, 'retired')}>Retire</button>
                          )}
                          {frozen(r.review_state) && (
                            <button className="btn sm secondary" disabled={busy} onClick={() => doRevise(r.id, r.scenario_code)}>Revise</button>
                          )}
                        </div>
                      </td>
                    </tr>
                  )
                })}
              </tbody>
            </table>
          </div>
        )}
      </Card>
    </div>
  )
}
