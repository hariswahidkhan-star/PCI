import { useMemo, useRef, useState, type ChangeEvent, type FormEvent } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { LabHeader, KpiRow, ConfirmDialog, InsightCallout, SortHeader } from '../../components/simlab'

// Admin Console → Simulation Lab Studio (Phase 5A). A governed authoring surface over the deterministic
// scenario engine: create a draft, validate it against the §14 publication gate (the reference solver runs
// every asked measure through the engine), walk it through the review workflow (draft → calc_review →
// learning_review → safety_review → pilot → approved → published), and revise a published version into a new
// draft. Approval is maker-checker (the approver must differ from the author) and approve/publish are blocked
// unless the validator passes — all enforced server-side; this page just drives it. Practice stays entirely
// separate from formal exam records, and no answer key is ever shown. The premium layer adds catalogue
// search/filtering, a sticky-header table, and accessible dialogs in place of window.prompt.

interface TemplateRow {
  task: string
  label: string
  hint: string
  config: string
}
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
  review_due?: string | null
  expires_at?: string | null
  governance?: string
  days_to_review?: number | null
  days_to_expiry?: number | null
}
interface Resp { rows: ScenarioRow[]; total: number; published: number }
interface Issue { severity: string; code: string; message: string }
interface Validation { id: number; scenario_code: string; review_state: string; publishable: boolean; errors: number; warnings: number; issues: Issue[] }
interface ImportResult { id: number; scenario_code: string; checksum: string; imported_version: number; publishable: boolean; errors: number; warnings: number; issues: Issue[] }

// Read a picked file as text. FileReader rather than Blob.text(): it is available in every environment
// the console runs in, including older Safari and the jsdom used by the tests.
const readText = (file: File) =>
  new Promise<string>((resolve, reject) => {
    const r = new FileReader()
    r.onload = () => resolve(String(r.result ?? ''))
    r.onerror = () => reject(new Error('The file could not be read.'))
    r.readAsText(file)
  })

// The scenario code a manifest names, used only to suggest a non-colliding code on a duplicate.
const manifestCode = (doc: unknown): string => {
  const s = (doc as { scenario?: { scenario_code?: unknown } } | null)?.scenario?.scenario_code
  return typeof s === 'string' ? s : 'scenario'
}

const titleCase = (s: string) => s.replace(/[_-]+/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase())

// The authoring workflow, in order. `nextState` is the single forward step the "Advance" button offers.
const FORWARD = ['draft', 'calc_review', 'learning_review', 'safety_review', 'pilot', 'approved', 'published']
const nextState = (s: string): string | null => {
  const i = FORWARD.indexOf(s)
  return i >= 0 && i + 1 < FORWARD.length ? FORWARD[i + 1] : null
}
const reviewTone = (s: string) => (s === 'published' ? 'ok' : s === 'draft' ? 'neutral' : s === 'retired' ? 'warn' : 'brand')

// Review-due / expiry governance (§13) surfaced as an amber/red flag alongside the review state.
const govLabel = (g?: string) =>
  g === 'expired' ? 'Expired' : g === 'review_overdue' ? 'Review overdue' : g === 'review_due_soon' ? 'Review due soon' : ''
const govTone = (g?: string) => (g === 'expired' || g === 'review_overdue' ? 'err' : g === 'review_due_soon' ? 'warn' : 'neutral')

const DIFF_RANK: Record<string, number> = { foundation: 0, intermediate: 1, advanced: 2, expert: 3 }
type SortCol = 'code' | 'title' | 'difficulty' | 'state' | 'attempts' | 'completion'
const COMPARE: Record<SortCol, (a: ScenarioRow, b: ScenarioRow) => number> = {
  code: (a, b) => a.scenario_code.localeCompare(b.scenario_code),
  title: (a, b) => a.title.localeCompare(b.title),
  difficulty: (a, b) => (DIFF_RANK[a.difficulty ?? 'foundation'] ?? 0) - (DIFF_RANK[b.difficulty ?? 'foundation'] ?? 0),
  state: (a, b) => FORWARD.indexOf(a.review_state) - FORWARD.indexOf(b.review_state),
  attempts: (a, b) => a.attempts - b.attempts,
  // Unattempted scenarios sort below any real completion rate.
  completion: (a, b) => (a.attempts > 0 ? a.completed / a.attempts : -1) - (b.attempts > 0 ? b.completed / b.attempts : -1),
}

export default function SimLab() {
  const { data, loading, error, refetch } = useAdminQuery<Resp>('/api/admin/lab/scenarios')
  const [msg, setMsg] = useState('')
  const [err, setErr] = useState('')
  const [busy, setBusy] = useState(false)
  const [showCreate, setShowCreate] = useState(false)
  const [validation, setValidation] = useState<Validation | null>(null)
  // catalogue search / filter / sort (client-side over the loaded rows)
  const [q, setQ] = useState('')
  const [stateFilter, setStateFilter] = useState('')
  const [sortCol, setSortCol] = useState<SortCol | null>(null)
  const [sortDir, setSortDir] = useState<'asc' | 'desc'>('asc')
  // dialogs (replacing window.prompt / silent retire)
  const [govRow, setGovRow] = useState<ScenarioRow | null>(null)
  const [govReview, setGovReview] = useState('')
  const [govExpiry, setGovExpiry] = useState('')
  const [importDoc, setImportDoc] = useState<unknown>(null)
  const [importCode, setImportCode] = useState('')
  const fileRef = useRef<HTMLInputElement>(null)
  const [reviseRow, setReviseRow] = useState<ScenarioRow | null>(null)
  const [reviseCode, setReviseCode] = useState('')
  const [retireRow, setRetireRow] = useState<ScenarioRow | null>(null)
  // create-form fields
  const [code, setCode] = useState('')
  const [title, setTitle] = useState('')
  const [difficulty, setDifficulty] = useState('foundation')
  const [cert, setCert] = useState('1')
  const [comps, setComps] = useState('earned_value')
  const [config, setConfig] = useState('')
  const [synthetic, setSynthetic] = useState(true)
  const [templateTask, setTemplateTask] = useState('')

  // Authoring starters, one per engine task. Each is proven publishable server-side (SimTemplatesTests),
  // so picking one gives the author a draft that already passes the validator.
  const { data: templateData } = useAdminQuery<{ rows: TemplateRow[] }>('/api/admin/lab/templates')
  const templates = useMemo(() => templateData?.rows ?? [], [templateData])
  const chosenTemplate = templates.find((t) => t.task === templateTask)

  function applyTemplate(task: string) {
    setTemplateTask(task)
    const t = templates.find((x) => x.task === task)
    if (!t) return
    setConfig(t.config.trim())
    // Prefill the competencies the starter declares, so the two stay consistent out of the box.
    try {
      const parsed = JSON.parse(t.config) as { competencies?: string[] }
      if (Array.isArray(parsed.competencies) && parsed.competencies.length) setComps(parsed.competencies.join(', '))
    } catch { /* the server-side test guarantees valid JSON; ignore anything unexpected */ }
  }

  const rows = useMemo(() => data?.rows ?? [], [data])
  const filtered = useMemo(() => {
    const needle = q.trim().toLowerCase()
    const out = rows.filter((r) => {
      if (stateFilter && r.review_state !== stateFilter) return false
      if (needle) {
        const hay = [r.scenario_code, r.title, r.industry, r.difficulty, ...(r.competencies ?? [])]
          .filter(Boolean).join(' ').toLowerCase()
        if (!hay.includes(needle)) return false
      }
      return true
    })
    if (sortCol) {
      out.sort(COMPARE[sortCol])
      if (sortDir === 'desc') out.reverse()
    }
    return out
  }, [rows, q, stateFilter, sortCol, sortDir])

  const toggleSort = (col: SortCol) => {
    if (sortCol === col) setSortDir((d) => (d === 'asc' ? 'desc' : 'asc'))
    else { setSortCol(col); setSortDir('asc') }
  }

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

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

  // Manifest import (§5B.5). Reads a manifest file, posts it for verification, and lands a new DRAFT —
  // the server refuses a file whose content no longer matches its checksum, and nothing in the file can
  // grant published state. A code collision reopens as a dialog so the operator can land it under a
  // different code rather than losing the upload.
  async function sendImport(doc: unknown, asCode?: string) {
    setBusy(true); setErr(''); setMsg(''); setValidation(null)
    try {
      const r = await adminApi.post<ImportResult>('/api/admin/lab/scenarios/import',
        asCode ? { manifest: doc, as_code: asCode } : { manifest: doc })
      setImportDoc(null)
      flash(`Imported “${r.scenario_code}” as a draft — ${r.publishable
        ? 'it passes validation here and can walk the review workflow.'
        : `validation found ${r.errors} error(s) to fix before it can be approved.`}`)
      // Surface the verdict in the same panel the Validate button uses.
      setValidation({ id: r.id, scenario_code: r.scenario_code, review_state: 'draft',
        publishable: r.publishable, errors: r.errors, warnings: r.warnings, issues: r.issues })
      refetch()
    } catch (e) {
      // The client passes an unrecognised backend code through as the message (see humanize()).
      if (e instanceof Error && e.message === 'duplicate_code' && doc) {
        setImportDoc(doc)
        setImportCode(`${manifestCode(doc)}-imported`)
        setErr('')
      } else fail(e)
    } finally { setBusy(false) }
  }

  async function onImportFile(e: ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0]
    e.target.value = ''          // let the same file be picked again after a failed attempt
    if (!file) return
    let doc: unknown
    try {
      doc = JSON.parse(await readText(file))
    } catch {
      fail(new Error('That file is not valid JSON. Choose a manifest exported from the Simulation Lab.'))
      return
    }
    await sendImport(doc)
  }

  // Manifest downloads (§5B.4 single, §5B.6 bundle). Fetched as raw text rather than through the shared
  // JSON client: re-serialising the parsed document would reformat it and break the byte/checksum
  // comparison the manifests exist for. The server names the file; the fallback only covers a proxy that
  // strips the header.
  async function downloadJson(url: string, fallbackName: string) {
    const res = await fetch(url, { headers: { Authorization: `Bearer ${adminApi.getToken() ?? ''}` } })
    if (!res.ok) throw new Error(`Export failed (${res.status}).`)
    const text = await res.text()
    const named = /filename="([^"]+)"/.exec(res.headers.get('Content-Disposition') ?? '')?.[1]
    const objUrl = URL.createObjectURL(new Blob([text], { type: 'application/json' }))
    const a = document.createElement('a')
    a.href = objUrl
    a.download = named || fallbackName
    document.body.appendChild(a)
    a.click()
    a.remove()
    URL.revokeObjectURL(objUrl)
  }

  async function doExport(r: ScenarioRow) {
    setBusy(true); setErr(''); setMsg('')
    try {
      await downloadJson(`/api/admin/lab/scenarios/${r.id}/manifest`,
        `${r.scenario_code.replace(/[^A-Za-z0-9._-]/g, '')}-v${r.version}.pcisim.json`)
      flash(`Exported manifest for “${r.scenario_code}”.`)
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  // Whole-catalogue bundle — one file for backup or migration, instead of exporting row by row. The API
  // also takes ?status= for scripted backups; the button deliberately exports everything, since a partial
  // backup that looks like a whole one is the more dangerous default.
  async function doExportAll() {
    setBusy(true); setErr(''); setMsg('')
    try {
      await downloadJson('/api/admin/lab/manifest-bundle', 'pci-simulation-catalogue.pcisim-bundle.json')
      flash(`Exported a bundle of all ${rows.length} scenarios.`)
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

  async function doRevise() {
    if (!reviseRow || !reviseCode.trim()) return
    const row = reviseRow
    setReviseRow(null)
    setBusy(true); setErr(''); setMsg('')
    try {
      await adminApi.post(`/api/admin/lab/scenarios/${row.id}/revise`, { new_code: reviseCode.trim() })
      flash(`Revised into new draft “${reviseCode.trim()}”.`)
      refetch()
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  async function doGovernance() {
    if (!govRow) return
    const row = govRow
    setGovRow(null)
    setBusy(true); setErr(''); setMsg('')
    try {
      await adminApi.patch(`/api/admin/lab/scenarios/${row.id}/governance`, { review_due: govReview.trim(), expires_at: govExpiry.trim() })
      flash('Governance dates updated.')
      refetch()
    } catch (e) { fail(e) } finally { setBusy(false) }
  }

  async function doRetire() {
    if (!retireRow) return
    const row = retireRow
    setRetireRow(null)
    await doReview(row.id, 'retired')
  }

  const frozen = (s: string) => s === 'approved' || s === 'published' || s === 'retired'
  const reviewStates = [...new Set(rows.map((r) => r.review_state))].sort()

  return (
    <div className="stack" style={{ display: 'grid', gap: '1.15rem' }}>
      <LabHeader
        eyebrow="Admin Studio"
        title="Simulation Lab Studio"
        lede="Author, validate and publish applied project-controls scenarios. A scenario can only be
          approved or published once the validator confirms the engine can grade every measure, and
          practice stays entirely separate from formal exam records."
        actions={
          <>
            <input ref={fileRef} type="file" accept="application/json,.json" onChange={(e) => { void onImportFile(e) }}
              style={{ display: 'none' }} data-testid="manifest-file" aria-hidden="true" tabIndex={-1} />
            <button className="btn secondary" disabled={busy || rows.length === 0} onClick={() => { void doExportAll() }}>
              Export all
            </button>
            <button className="btn secondary" disabled={busy} onClick={() => fileRef.current?.click()}>
              Import manifest
            </button>
            <button className="btn" onClick={() => setShowCreate((v) => !v)}>
              {showCreate ? 'Cancel' : '+ New scenario'}
            </button>
          </>
        }
      />

      {msg && <div className="notice ok" role="status">{msg}</div>}
      {err && <div className="notice err" role="alert">{err}</div>}

      <KpiRow
        items={[
          { k: 'Scenarios', v: data?.total ?? 0 },
          { k: 'Published', v: data?.published ?? 0 },
          { k: 'In review', v: inReview },
          { k: 'Attempts', v: totalAttempts },
        ]}
      />

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
            <label>Start from a template
              <select value={templateTask} onChange={(e) => applyTemplate(e.target.value)}>
                <option value="">Blank — write the task definition myself</option>
                {templates.map((t) => (
                  <option key={t.task} value={t.task}>{t.label}</option>
                ))}
              </select>
            </label>
            {chosenTemplate && (
              <p className="muted small" style={{ margin: '-.2rem 0 0' }}>{chosenTemplate.hint}</p>
            )}
            <label>Task definition (config JSON)
              <textarea value={config} onChange={(e) => setConfig(e.target.value)} rows={6}
                placeholder='{"task":"evm","prompt":"…","given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},"ask":[{"key":"cpi","type":"number"}]}'
                style={{ fontFamily: 'ui-monospace, SFMono-Regular, Menlo, monospace', width: '100%' }} />
            </label>
            <label className="row" style={{ gap: '.4rem', alignItems: 'center' }}>
              <input type="checkbox" checked={synthetic} onChange={(e) => setSynthetic(e.target.checked)} style={{ width: 'auto' }} />
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
            <span className="muted" style={{ fontVariantNumeric: 'tabular-nums' }}>
              {validation.errors} error(s), {validation.warnings} warning(s)
            </span>
          </div>
          {validation.issues.length === 0
            ? <Empty>No issues — this scenario passes every check.</Empty>
            : (
              <ul style={{ margin: 0, paddingInlineStart: '1.1rem', display: 'grid', gap: '.3rem' }}>
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
          <div className="stack" style={{ display: 'grid', gap: '.7rem' }}>
            <div className="sl-toolbar-row">
              <label className="sl-search">
                <span>Search</span>
                <input value={q} onChange={(e) => setQ(e.target.value)} placeholder="Code, title, industry, competency…" aria-label="Search scenarios" />
              </label>
              <label>
                <span>Review state</span>
                <select value={stateFilter} onChange={(e) => setStateFilter(e.target.value)} aria-label="Filter by review state">
                  <option value="">All states</option>
                  {reviewStates.map((s) => <option key={s} value={s}>{titleCase(s)}</option>)}
                </select>
              </label>
            </div>
            <p className="sl-results" role="status">Showing {filtered.length} of {rows.length} scenarios</p>
            {filtered.length === 0 ? (
              <InsightCallout tone="info" title="No scenarios match">
                <p>Widen the search or clear the review-state filter.</p>
              </InsightCallout>
            ) : (
              <div className="sl-tablewrap">
                <table className="data">
                  <thead>
                    <tr>
                      <SortHeader label="Code" active={sortCol === 'code'} dir={sortDir} onSort={() => toggleSort('code')} />
                      <SortHeader label="Title" active={sortCol === 'title'} dir={sortDir} onSort={() => toggleSort('title')} />
                      <SortHeader label="Difficulty" active={sortCol === 'difficulty'} dir={sortDir} onSort={() => toggleSort('difficulty')} />
                      <th>Status</th>
                      <SortHeader label="Review state" active={sortCol === 'state'} dir={sortDir} onSort={() => toggleSort('state')} />
                      <SortHeader label="Attempts" active={sortCol === 'attempts'} dir={sortDir} onSort={() => toggleSort('attempts')} align="end" />
                      <SortHeader label="Completion" active={sortCol === 'completion'} dir={sortDir} onSort={() => toggleSort('completion')} align="end" />
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {filtered.map((r) => {
                      const nxt = nextState(r.review_state)
                      return (
                        <tr key={r.id}>
                          <td style={{ fontVariantNumeric: 'tabular-nums', whiteSpace: 'nowrap' }}>{r.scenario_code}</td>
                          <td>{r.title}</td>
                          <td>{r.difficulty ? titleCase(r.difficulty) : '—'}</td>
                          <td><StatusBadge status={r.status} /></td>
                          <td>
                            <Badge tone={reviewTone(r.review_state)}>{titleCase(r.review_state)}</Badge>
                            {r.governance && r.governance !== 'ok' && (
                              <>{' '}<Badge tone={govTone(r.governance)}>{govLabel(r.governance)}</Badge></>
                            )}
                          </td>
                          <td style={{ textAlign: 'end', fontVariantNumeric: 'tabular-nums' }}>{r.attempts}</td>
                          <td style={{ textAlign: 'end', fontVariantNumeric: 'tabular-nums' }}>
                            {r.attempts > 0 ? `${Math.round((r.completed / r.attempts) * 100)}%` : '—'}
                          </td>
                          <td>
                            <div className="sl-actions">
                              <button className="btn sm" disabled={busy} onClick={() => doValidate(r.id)}>Validate</button>
                              <button className="btn sm secondary" disabled={busy} onClick={() => { void doExport(r) }}>Export</button>
                              <button
                                className="btn sm secondary" disabled={busy}
                                onClick={() => {
                                  setGovRow(r)
                                  setGovReview((r.review_due ?? '').slice(0, 10))
                                  setGovExpiry((r.expires_at ?? '').slice(0, 10))
                                }}
                              >Dates</button>
                              {nxt && (
                                <button className="btn sm" disabled={busy} onClick={() => doReview(r.id, nxt)}>
                                  → {titleCase(nxt)}
                                </button>
                              )}
                              {r.review_state !== 'draft' && r.review_state !== 'published' && r.review_state !== 'retired' && (
                                <button className="btn sm secondary" disabled={busy} onClick={() => doReview(r.id, 'draft')}>Return to draft</button>
                              )}
                              {r.review_state === 'published' && (
                                <button className="btn sm secondary" disabled={busy} onClick={() => setRetireRow(r)}>Retire</button>
                              )}
                              {frozen(r.review_state) && (
                                <button
                                  className="btn sm secondary" disabled={busy}
                                  onClick={() => { setReviseRow(r); setReviseCode(`${r.scenario_code}-v2`) }}
                                >Revise</button>
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
          </div>
        )}
      </Card>

      {/* Governance dates — replaces window.prompt with an accessible dialog. */}
      <ConfirmDialog
        open={govRow !== null}
        title={`Governance dates — ${govRow?.scenario_code ?? ''}`}
        body={
          <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
            <p>Review-due and expiry drive the amber/red governance flags. Leave a date blank to clear it.</p>
            <label>Review-due date
              <input type="date" value={govReview} onChange={(e) => setGovReview(e.target.value)} />
            </label>
            <label>Expiry date
              <input type="date" value={govExpiry} onChange={(e) => setGovExpiry(e.target.value)} />
            </label>
          </div>
        }
        confirmLabel="Save dates"
        onConfirm={() => { void doGovernance() }}
        onCancel={() => setGovRow(null)}
      />

      {/* A manifest whose code is already taken here — land it under a different one. */}
      <ConfirmDialog
        open={importDoc !== null}
        title="That scenario code is already in use"
        body={
          <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
            <p>
              This environment already has a scenario called <code>{manifestCode(importDoc)}</code>.
              Import the manifest under a different code — the content is unchanged, and it lands as a draft
              that still has to walk the review workflow.
            </p>
            <label>Import as code
              <input value={importCode} onChange={(e) => setImportCode(e.target.value)} required aria-label="Import as code" />
            </label>
          </div>
        }
        confirmLabel="Import as draft"
        onConfirm={() => { const d = importDoc; setImportDoc(null); void sendImport(d, importCode.trim()) }}
        onCancel={() => setImportDoc(null)}
      />

      {/* Revise a frozen scenario into a new draft. */}
      <ConfirmDialog
        open={reviseRow !== null}
        title={`Revise ${reviseRow?.scenario_code ?? ''}`}
        body={
          <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
            <p>Creates a new draft copy under a new code. The {reviseRow?.review_state === 'published' ? 'published' : 'frozen'} version stays live and unchanged.</p>
            <label>New scenario code
              <input value={reviseCode} onChange={(e) => setReviseCode(e.target.value)} required />
            </label>
          </div>
        }
        confirmLabel="Create revision draft"
        onConfirm={() => { void doRevise() }}
        onCancel={() => setReviseRow(null)}
      />

      {/* Retiring removes the scenario from the student catalogue — confirm it. */}
      <ConfirmDialog
        open={retireRow !== null}
        title={`Retire ${retireRow?.scenario_code ?? ''}?`}
        body="Retiring removes this scenario from the student catalogue immediately. Existing attempt history is kept. To change it later, revise it into a new draft."
        confirmLabel="Retire scenario"
        danger
        onConfirm={() => { void doRetire() }}
        onCancel={() => setRetireRow(null)}
      />
    </div>
  )
}
