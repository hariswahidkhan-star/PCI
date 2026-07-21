import { Fragment, useState } from 'react'
import { useAdminQuery, runMutation } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../../components/ui'
import { fmtDate } from '../../format'

// Admin management for the dynamic careers / job board. Create/edit/publish postings, and review applicants
// (with CV download + status). Backend: Careers.cs (gated 'content').

interface Job {
  id: number; job_code?: string | null; title: string; organisation?: string | null; location?: string | null; country?: string | null
  employment_type?: string; remote_type?: string; sector?: string | null
  description?: string | null; requirements?: string | null; responsibilities?: string | null
  salary_min?: number | null; salary_max?: number | null; salary_currency?: string | null; salary_period?: string | null
  apply_method?: string; apply_url?: string | null; apply_email?: string | null
  featured?: number; status: string; posted_at?: string | null; closes_at?: string | null; applications?: number
  // Increment 1: richer job model
  department?: string | null; experience_level?: string | null; vacancies?: number | null
  benefits?: string | null; education?: string | null; languages?: string | null; certifications?: string | null
  reporting_line?: string | null; expected_start?: string | null; application_instructions?: string | null
  eo_statement?: string | null; salary_visible?: number; urgent?: number; publish_at?: string | null
}
interface Application {
  id: number; job_id: number; name?: string | null; email?: string | null; phone?: string | null
  cover_message?: string | null; cv_name?: string | null; status: string; admin_note?: string | null; created_at?: string | null
  reference?: string | null; answers_json?: string | null; assigned_to?: number | null; assignee_name?: string | null
}
interface Question { id?: number; qtype: string; label: string; options?: string | null; required?: number }
const Q_TYPES: [string, string][] = [
  ['short_text', 'Short text'], ['long_text', 'Long text'], ['yesno', 'Yes / No'], ['single', 'Single choice'],
  ['multi', 'Multiple choice'], ['dropdown', 'Dropdown'], ['number', 'Number'], ['date', 'Date'], ['consent', 'Consent checkbox'],
]

const EMPTY: Partial<Job> = { title: '', employment_type: 'full_time', remote_type: 'onsite', apply_method: 'inplatform', status: 'draft', salary_currency: 'USD', salary_period: 'year' }
const APP_STATUSES = ['new', 'reviewing', 'shortlisted', 'interview', 'assessment', 'offer', 'hired', 'rejected', 'withdrawn', 'closed']
interface AppEvent { id: number; kind: string; from_status?: string | null; to_status?: string | null; body?: string | null; scheduled_at?: string | null; actor_name?: string | null; created_at?: string | null }

export default function Careers() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Job[] }>('/api/admin/careers')
  const [edit, setEdit] = useState<Partial<Job> | null>(null)
  const [appsFor, setAppsFor] = useState<Job | null>(null)
  const [questionsFor, setQuestionsFor] = useState<Job | null>(null)
  const [q, setQ] = useState('')
  const [fStatus, setFStatus] = useState('')
  const [fCountry, setFCountry] = useState('')
  const [fType, setFType] = useState('')

  const del = (j: Job) =>
    runMutation(async () => { if (!window.confirm(`Delete "${j.title}" and its applications?`)) return; await adminApi.post(`/api/admin/careers/${j.id}/delete`, {}); refetch() })

  const all = data?.rows ?? []
  const countries = Array.from(new Set(all.map((j) => j.country).filter(Boolean))) as string[]
  const ql = q.trim().toLowerCase()
  const rows = all.filter((j) =>
    (!ql || [j.title, j.organisation, j.job_code, j.location, j.country, j.sector].some((v) => (v ?? '').toLowerCase().includes(ql)))
    && (!fStatus || j.status === fStatus)
    && (!fCountry || j.country === fCountry)
    && (!fType || j.employment_type === fType))

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <div><h1>Careers</h1><p className="muted small" style={{ margin: 0 }}>Manage the public job board and review applicants.</p></div>
        <button className="btn sm" onClick={() => setEdit({ ...EMPTY })}>New posting</button>
      </div>
      <Card>
        <div className="row" style={{ flexWrap: 'wrap', marginBottom: '.6rem', gap: '.5rem' }}>
          <input placeholder="Search title, employer, code, country…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 280 }} />
          <select value={fStatus} onChange={(e) => setFStatus(e.target.value)} style={{ maxWidth: 150 }}>
            <option value="">All statuses</option><option value="published">Published</option><option value="draft">Draft</option><option value="closed">Closed</option>
          </select>
          <select value={fCountry} onChange={(e) => setFCountry(e.target.value)} style={{ maxWidth: 170 }}>
            <option value="">All countries</option>{countries.map((c) => <option key={c} value={c}>{c}</option>)}
          </select>
          <select value={fType} onChange={(e) => setFType(e.target.value)} style={{ maxWidth: 150 }}>
            <option value="">All types</option><option value="full_time">Full-time</option><option value="part_time">Part-time</option><option value="contract">Contract</option><option value="internship">Internship</option><option value="temporary">Temporary</option>
          </select>
          <span className="muted small" style={{ alignSelf: 'center' }}>{rows.length} of {all.length}</span>
        </div>
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
          <Empty>{all.length === 0 ? 'No job postings yet.' : 'No postings match your filters.'}</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Code</th><th>Title</th><th>Employer</th><th>Location</th><th>Type</th><th>Applications</th><th>Status</th><th></th></tr></thead>
            <tbody>
              {rows.map((j) => (
                <tr key={j.id}>
                  <td className="small mono">{j.job_code}</td>
                  <td>{j.title}{j.featured ? ' ⭐' : ''}</td>
                  <td className="small">{j.organisation}</td>
                  <td className="small">{[j.location, j.country].filter(Boolean).join(', ')}</td>
                  <td className="small">{j.employment_type}{j.remote_type && j.remote_type !== 'onsite' ? ` · ${j.remote_type}` : ''}</td>
                  <td className="small">{j.applications ?? 0}</td>
                  <td>{j.status === 'published' ? <Badge tone="ok">Published</Badge> : j.status === 'closed' ? <Badge tone="warn">Closed</Badge> : <Badge tone="neutral">Draft</Badge>}</td>
                  <td>
                    <div className="row" style={{ gap: '.35rem', justifyContent: 'flex-end' }}>
                      <button className="btn sm secondary" onClick={() => setAppsFor(j)}>Applicants ({j.applications ?? 0})</button>
                      <button className="btn sm ghost" onClick={() => setQuestionsFor(j)}>Questions</button>
                      <button className="btn sm ghost" onClick={() => setEdit(j)}>Edit</button>
                      <button className="btn sm ghost danger" onClick={() => del(j)}>Delete</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
      {edit && <JobEditor initial={edit} onClose={() => setEdit(null)} onSaved={() => { setEdit(null); refetch() }} />}
      {appsFor && <Applicants job={appsFor} onClose={() => setAppsFor(null)} />}
      {questionsFor && <QuestionsEditor job={questionsFor} onClose={() => setQuestionsFor(null)} />}
    </div>
  )
}

function JobEditor({ initial, onClose, onSaved }: { initial: Partial<Job>; onClose: () => void; onSaved: () => void }) {
  const [d, setD] = useState<Partial<Job>>(initial)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const set = <K extends keyof Job>(k: K, v: Job[K]) => setD((p) => ({ ...p, [k]: v }))
  const num = (v: string) => (v === '' ? undefined : Number(v))

  async function save() {
    setBusy(true); setErr(null)
    try { await adminApi.post('/api/admin/careers', d); onSaved() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Could not save.') }
    finally { setBusy(false) }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{d.id ? 'Edit posting' : 'New posting'}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{err}</div>}
        <div className="grid cols-2">
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Title</label><input value={d.title ?? ''} onChange={(e) => set('title', e.target.value)} /></div>
          <div className="field"><label>Job code <span className="muted small">(blank = auto)</span></label><input value={d.job_code ?? ''} onChange={(e) => set('job_code', e.target.value)} placeholder="auto: PCI-2026-0001" /></div>
          <div className="field"><label>Employer / organisation</label><input value={d.organisation ?? ''} onChange={(e) => set('organisation', e.target.value)} /></div>
          <div className="field"><label>City / location</label><input value={d.location ?? ''} onChange={(e) => set('location', e.target.value)} placeholder="London" /></div>
          <div className="field"><label>Country</label><input value={d.country ?? ''} onChange={(e) => set('country', e.target.value)} placeholder="United Kingdom" /></div>
          <div className="field"><label>Type</label>
            <select value={d.employment_type ?? 'full_time'} onChange={(e) => set('employment_type', e.target.value)}>
              <option value="full_time">Full-time</option><option value="part_time">Part-time</option><option value="contract">Contract</option><option value="internship">Internship</option><option value="temporary">Temporary</option>
            </select>
          </div>
          <div className="field"><label>Location type</label>
            <select value={d.remote_type ?? 'onsite'} onChange={(e) => set('remote_type', e.target.value)}>
              <option value="onsite">On-site</option><option value="remote">Remote</option><option value="hybrid">Hybrid</option>
            </select>
          </div>
          <div className="field"><label>Sector</label><input value={d.sector ?? ''} onChange={(e) => set('sector', e.target.value)} placeholder="Energy, Rail…" /></div>
          <div className="field"><label>Status</label>
            <select value={d.status ?? 'draft'} onChange={(e) => set('status', e.target.value)}>
              <option value="draft">Draft</option><option value="published">Published</option><option value="closed">Closed</option>
            </select>
          </div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>About the role</label><textarea rows={4} value={d.description ?? ''} onChange={(e) => set('description', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Responsibilities</label><textarea rows={3} value={d.responsibilities ?? ''} onChange={(e) => set('responsibilities', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Requirements</label><textarea rows={3} value={d.requirements ?? ''} onChange={(e) => set('requirements', e.target.value)} /></div>
          <div className="field"><label>Department</label><input value={d.department ?? ''} onChange={(e) => set('department', e.target.value)} placeholder="Cost & Commercial" /></div>
          <div className="field"><label>Experience level</label>
            <select value={d.experience_level ?? ''} onChange={(e) => set('experience_level', e.target.value)}>
              <option value="">—</option><option value="entry">Entry</option><option value="junior">Junior</option><option value="mid">Mid</option><option value="senior">Senior</option><option value="lead">Lead</option><option value="principal">Principal</option><option value="director">Director</option>
            </select>
          </div>
          <div className="field"><label>Vacancies</label><input type="number" value={d.vacancies ?? ''} onChange={(e) => set('vacancies', num(e.target.value) as number)} placeholder="1" /></div>
          <div className="field"><label>Reporting line</label><input value={d.reporting_line ?? ''} onChange={(e) => set('reporting_line', e.target.value)} placeholder="Reports to Controls Manager" /></div>
          <div className="field"><label>Expected start</label><input value={d.expected_start ?? ''} onChange={(e) => set('expected_start', e.target.value)} placeholder="2026-09 or ASAP" /></div>
          <div className="field"><label>Language requirements</label><input value={d.languages ?? ''} onChange={(e) => set('languages', e.target.value)} placeholder="English (fluent)" /></div>
          <div className="field"><label>Required certifications</label><input value={d.certifications ?? ''} onChange={(e) => set('certifications', e.target.value)} placeholder="PMP, PCL-AI" /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Benefits</label><textarea rows={2} value={d.benefits ?? ''} onChange={(e) => set('benefits', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Education requirements</label><textarea rows={2} value={d.education ?? ''} onChange={(e) => set('education', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Application instructions</label><textarea rows={2} value={d.application_instructions ?? ''} onChange={(e) => set('application_instructions', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Equal-opportunity statement</label><textarea rows={2} value={d.eo_statement ?? ''} onChange={(e) => set('eo_statement', e.target.value)} /></div>
          <div className="field"><label>Salary min</label><input type="number" value={d.salary_min ?? ''} onChange={(e) => set('salary_min', num(e.target.value) as number)} /></div>
          <div className="field"><label>Salary max</label><input type="number" value={d.salary_max ?? ''} onChange={(e) => set('salary_max', num(e.target.value) as number)} /></div>
          <div className="field"><label>Currency</label><input value={d.salary_currency ?? 'USD'} onChange={(e) => set('salary_currency', e.target.value)} /></div>
          <div className="field"><label>Per</label>
            <select value={d.salary_period ?? 'year'} onChange={(e) => set('salary_period', e.target.value)}>
              <option value="year">year</option><option value="month">month</option><option value="day">day</option><option value="hour">hour</option>
            </select>
          </div>
          <div className="field"><label>Apply method</label>
            <select value={d.apply_method ?? 'inplatform'} onChange={(e) => set('apply_method', e.target.value)}>
              <option value="inplatform">In-platform (collect applications here)</option><option value="url">External URL</option><option value="email">Email</option>
            </select>
          </div>
          {d.apply_method === 'url' && <div className="field"><label>Apply URL</label><input value={d.apply_url ?? ''} onChange={(e) => set('apply_url', e.target.value)} placeholder="https://…" /></div>}
          {d.apply_method === 'email' && <div className="field"><label>Apply email</label><input value={d.apply_email ?? ''} onChange={(e) => set('apply_email', e.target.value)} placeholder="jobs@…" /></div>}
          <div className="field"><label>Closes on (optional)</label><input value={d.closes_at ?? ''} onChange={(e) => set('closes_at', e.target.value)} placeholder="2026-12-31" /></div>
          <div className="field"><label>Schedule publish (optional)</label><input value={d.publish_at ?? ''} onChange={(e) => set('publish_at', e.target.value)} placeholder="2026-09-01" /></div>
          <label className="row" style={{ fontWeight: 400, alignItems: 'center', gap: '.4rem' }}><input type="checkbox" style={{ width: 'auto' }} checked={!!d.featured} onChange={(e) => set('featured', (e.target.checked ? 1 : 0) as number)} /> Featured</label>
          <label className="row" style={{ fontWeight: 400, alignItems: 'center', gap: '.4rem' }}><input type="checkbox" style={{ width: 'auto' }} checked={!!d.urgent} onChange={(e) => set('urgent', (e.target.checked ? 1 : 0) as number)} /> Urgent</label>
          <label className="row" style={{ fontWeight: 400, alignItems: 'center', gap: '.4rem' }}><input type="checkbox" style={{ width: 'auto' }} checked={d.salary_visible == null ? true : !!d.salary_visible} onChange={(e) => set('salary_visible', (e.target.checked ? 1 : 0) as number)} /> Show salary publicly</label>
        </div>
        <div className="row" style={{ marginTop: '1rem' }}>
          <button className="btn" disabled={busy || !(d.title && d.title.length >= 3)} onClick={save}>{busy ? 'Saving…' : d.id ? 'Save changes' : 'Create posting'}</button>
        </div>
      </div>
    </div>
  )
}

function Applicants({ job, onClose }: { job: Job; onClose: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Application[] }>(`/api/admin/careers/${job.id}/applications`)
  const [openId, setOpenId] = useState<number | null>(null)

  const setStatus = (a: Application, status: string) =>
    runMutation(async () => { await adminApi.post(`/api/admin/careers/applications/${a.id}/status`, { status }); refetch() })

  async function downloadCv(a: Application) {
    const tok = adminApi.getToken() ?? ''
    const res = await fetch(`/api/admin/careers/applications/${a.id}/cv`, { headers: { Authorization: 'Bearer ' + tok } })
    if (!res.ok) { alert('No CV on file for this applicant.'); return }
    const el = document.createElement('a'); el.href = URL.createObjectURL(await res.blob()); el.download = a.cv_name || `cv-${a.id}`
    document.body.appendChild(el); el.click(); el.remove(); URL.revokeObjectURL(el.href)
  }

  function exportCsv() {
    const rows = data?.rows ?? []
    const head = ['reference', 'name', 'email', 'phone', 'status', 'assignee', 'applied']
    const esc = (v: unknown) => `"${String(v ?? '').replace(/"/g, '""')}"`
    const body = rows.map((a) => [a.reference, a.name, a.email, a.phone, a.status, a.assignee_name, a.created_at].map(esc).join(','))
    const csv = [head.join(','), ...body].join('\n')
    const el = document.createElement('a'); el.href = URL.createObjectURL(new Blob([csv], { type: 'text/csv' }))
    el.download = `applicants-${job.job_code || job.id}.csv`; document.body.appendChild(el); el.click(); el.remove(); URL.revokeObjectURL(el.href)
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Applicants — {job.title}</h2>
          <div className="row" style={{ gap: '.4rem' }}>
            {(data?.rows.length ?? 0) > 0 && <button className="btn secondary sm" onClick={exportCsv}>Export CSV</button>}
            <button className="btn secondary sm" onClick={onClose}>Close</button>
          </div>
        </div>
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No applications yet.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Applicant</th><th>Assignee</th><th>Status</th><th></th></tr></thead>
            <tbody>
              {data.rows.map((a) => (
                <Fragment key={a.id}>
                  <tr>
                    <td>
                      <div>{a.name}{a.reference ? <span className="muted small mono"> · {a.reference}</span> : null}</div>
                      <div className="muted small">{a.email}{a.phone ? ` · ${a.phone}` : ''} · {fmtDate(a.created_at)}</div>
                      {a.cover_message && <div className="small" style={{ maxWidth: 320, marginTop: '.2rem' }}>{a.cover_message}</div>}
                      {parseAnswers(a.answers_json).map((qa, i) => (
                        <div key={i} className="small" style={{ maxWidth: 320, marginTop: '.2rem' }}><strong>{qa.label}:</strong> {qa.value}</div>
                      ))}
                      {a.cv_name && <button className="btn ghost sm" style={{ marginTop: '.2rem' }} onClick={() => downloadCv(a)}>CV: {a.cv_name}</button>}
                    </td>
                    <td className="small">{a.assignee_name || <span className="muted">—</span>}</td>
                    <td>
                      <StatusBadge status={a.status} />
                      <select value={a.status} onChange={(e) => setStatus(a, e.target.value)} style={{ marginTop: '.3rem' }}>
                        {APP_STATUSES.map((s) => <option key={s} value={s}>{s}</option>)}
                      </select>
                    </td>
                    <td><button className="btn ghost sm" onClick={() => setOpenId(openId === a.id ? null : a.id)}>{openId === a.id ? 'Hide' : 'Manage'}</button></td>
                  </tr>
                  {openId === a.id && (
                    <tr><td colSpan={4} style={{ background: 'var(--bg-soft,#f8fafc)' }}><ApplicantDetail app={a} onChanged={refetch} /></td></tr>
                  )}
                </Fragment>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}

function ApplicantDetail({ app, onChanged }: { app: Application; onChanged: () => void }) {
  const { data, loading, refetch } = useAdminQuery<{ rows: AppEvent[] }>(`/api/admin/careers/applications/${app.id}/events`)
  const [note, setNote] = useState(''); const [msg, setMsg] = useState(''); const [ivWhen, setIvWhen] = useState(''); const [ivNote, setIvNote] = useState('')
  const post = (path: string, body: unknown) => runMutation(async () => { await adminApi.post(`/api/admin/careers/applications/${app.id}/${path}`, body); refetch(); onChanged() })
  const rows = data?.rows ?? []
  return (
    <div style={{ display: 'grid', gap: '.7rem', padding: '.4rem 0' }}>
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {app.assigned_to ? <button className="btn ghost sm" onClick={() => post('assign', { unassign: true })}>Unassign ({app.assignee_name})</button>
          : <button className="btn secondary sm" onClick={() => post('assign', {})}>Assign to me</button>}
      </div>
      <div className="grid cols-2" style={{ gap: '.6rem' }}>
        <div className="field"><label>Internal note <span className="muted small">(not shown to candidate)</span></label>
          <div className="row" style={{ gap: '.4rem' }}><input value={note} onChange={(e) => setNote(e.target.value)} /><button className="btn sm" disabled={!note.trim()} onClick={() => { post('note', { body: note }); setNote('') }}>Add</button></div>
        </div>
        <div className="field"><label>Message to candidate <span className="muted small">(visible in their portal)</span></label>
          <div className="row" style={{ gap: '.4rem' }}><input value={msg} onChange={(e) => setMsg(e.target.value)} /><button className="btn sm" disabled={!msg.trim()} onClick={() => { post('message', { body: msg }); setMsg('') }}>Send</button></div>
        </div>
        <div className="field" style={{ gridColumn: '1 / -1' }}><label>Schedule interview</label>
          <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
            <input type="datetime-local" value={ivWhen} onChange={(e) => setIvWhen(e.target.value)} />
            <input placeholder="Details (link, location…)" value={ivNote} onChange={(e) => setIvNote(e.target.value)} style={{ flex: 1, minWidth: 160 }} />
            <button className="btn sm" disabled={!ivWhen} onClick={() => { post('interview', { scheduled_at: ivWhen, body: ivNote }); setIvWhen(''); setIvNote('') }}>Schedule</button>
          </div>
        </div>
      </div>
      <div>
        <div className="muted small" style={{ marginBottom: '.3rem' }}>Timeline</div>
        {loading ? <Spinner /> : rows.length === 0 ? <div className="muted small">No activity yet.</div> : (
          <div className="stack" style={{ display: 'grid', gap: '.3rem' }}>
            {rows.map((e) => (
              <div key={e.id} className="small">
                <span className="muted">{fmtDate(e.created_at)} · {e.actor_name || 'System'} · </span>
                {e.kind === 'status' ? <>moved {e.from_status || '—'} → <strong>{e.to_status}</strong>{e.body ? ` (${e.body})` : ''}</>
                  : e.kind === 'interview' ? <><strong>Interview</strong> {e.scheduled_at}{e.body ? ` — ${e.body}` : ''}</>
                  : e.kind === 'message' ? <><strong>→ candidate:</strong> {e.body}</>
                  : <><strong>note:</strong> {e.body}</>}
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}

function parseAnswers(json?: string | null): { label: string; value: string }[] {
  if (!json) return []
  try { const a = JSON.parse(json); return Array.isArray(a) ? a : [] } catch { return [] }
}

function QuestionsEditor({ job, onClose }: { job: Job; onClose: () => void }) {
  const { data, loading } = useAdminQuery<{ rows: Question[] }>(`/api/admin/careers/${job.id}/questions`)
  const [rows, setRows] = useState<Question[] | null>(null)
  const [busy, setBusy] = useState(false)
  const list = rows ?? data?.rows ?? []
  const upd = (i: number, patch: Partial<Question>) => setRows(list.map((q, j) => (j === i ? { ...q, ...patch } : q)))
  const add = () => setRows([...list, { qtype: 'short_text', label: '', options: '', required: 0 }])
  const remove = (i: number) => setRows(list.filter((_, j) => j !== i))
  const move = (i: number, dir: number) => { const n = [...list]; const t = i + dir; if (t < 0 || t >= n.length) return; [n[i], n[t]] = [n[t], n[i]]; setRows(n) }
  async function save() {
    setBusy(true)
    try { await adminApi.post(`/api/admin/careers/${job.id}/questions`, { questions: list.filter((q) => q.label.trim()) }); onClose() }
    finally { setBusy(false) }
  }
  const needsOptions = (t: string) => t === 'single' || t === 'multi' || t === 'dropdown'
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Application questions — {job.title}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        <p className="muted small" style={{ marginTop: 0 }}>Job-specific questions candidates answer when applying. Options (one per line) apply to choice / dropdown types.</p>
        {loading && !rows ? <Spinner /> : (
          <div className="stack" style={{ display: 'grid', gap: '.7rem' }}>
            {list.length === 0 && <Empty>No questions yet.</Empty>}
            {list.map((q, i) => (
              <div key={i} className="row" style={{ gap: '.4rem', alignItems: 'flex-start', flexWrap: 'wrap', borderBottom: '1px solid var(--line,#e2e8f0)', paddingBottom: '.5rem' }}>
                <select value={q.qtype} onChange={(e) => upd(i, { qtype: e.target.value })} style={{ maxWidth: 150 }}>
                  {Q_TYPES.map(([v, l]) => <option key={v} value={v}>{l}</option>)}
                </select>
                <input placeholder="Question label" value={q.label} onChange={(e) => upd(i, { label: e.target.value })} style={{ flex: 1, minWidth: 180 }} />
                {needsOptions(q.qtype) && <textarea placeholder="Options (one per line)" value={q.options ?? ''} onChange={(e) => upd(i, { options: e.target.value })} rows={2} style={{ minWidth: 160 }} />}
                <label className="row small" style={{ gap: '.3rem', alignSelf: 'center' }}><input type="checkbox" style={{ width: 'auto' }} checked={!!q.required} onChange={(e) => upd(i, { required: e.target.checked ? 1 : 0 })} /> required</label>
                <div className="row" style={{ gap: '.2rem' }}>
                  <button className="btn ghost sm" onClick={() => move(i, -1)}>↑</button>
                  <button className="btn ghost sm" onClick={() => move(i, 1)}>↓</button>
                  <button className="btn ghost sm danger" onClick={() => remove(i)}>✕</button>
                </div>
              </div>
            ))}
            <div className="row" style={{ gap: '.5rem' }}>
              <button className="btn secondary sm" onClick={add}>+ Add question</button>
              <button className="btn" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save questions'}</button>
            </div>
          </div>
        )}
      </div>
    </div>
  )
}
