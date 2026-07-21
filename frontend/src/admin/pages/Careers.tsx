import { useState } from 'react'
import { useAdminQuery, runMutation } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../../components/ui'
import { fmtDate } from '../../format'

// Admin management for the dynamic careers / job board. Create/edit/publish postings, and review applicants
// (with CV download + status). Backend: Careers.cs (gated 'content').

interface Job {
  id: number; title: string; organisation?: string | null; location?: string | null
  employment_type?: string; remote_type?: string; sector?: string | null
  description?: string | null; requirements?: string | null; responsibilities?: string | null
  salary_min?: number | null; salary_max?: number | null; salary_currency?: string | null; salary_period?: string | null
  apply_method?: string; apply_url?: string | null; apply_email?: string | null
  featured?: number; status: string; posted_at?: string | null; closes_at?: string | null; applications?: number
}
interface Application {
  id: number; job_id: number; name?: string | null; email?: string | null; phone?: string | null
  cover_message?: string | null; cv_name?: string | null; status: string; admin_note?: string | null; created_at?: string | null
}

const EMPTY: Partial<Job> = { title: '', employment_type: 'full_time', remote_type: 'onsite', apply_method: 'inplatform', status: 'draft', salary_currency: 'USD', salary_period: 'year' }
const APP_STATUSES = ['new', 'reviewing', 'shortlisted', 'rejected', 'hired']

export default function Careers() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Job[] }>('/api/admin/careers')
  const [edit, setEdit] = useState<Partial<Job> | null>(null)
  const [appsFor, setAppsFor] = useState<Job | null>(null)

  const del = (j: Job) =>
    runMutation(async () => { if (!window.confirm(`Delete "${j.title}" and its applications?`)) return; await adminApi.post(`/api/admin/careers/${j.id}/delete`, {}); refetch() })

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <div><h1>Careers</h1><p className="muted small" style={{ margin: 0 }}>Manage the public job board and review applicants.</p></div>
        <button className="btn sm" onClick={() => setEdit({ ...EMPTY })}>New posting</button>
      </div>
      <Card>
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No job postings yet.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Title</th><th>Employer</th><th>Type</th><th>Applications</th><th>Status</th><th></th></tr></thead>
            <tbody>
              {data.rows.map((j) => (
                <tr key={j.id}>
                  <td>{j.title}{j.featured ? ' ⭐' : ''}</td>
                  <td className="small">{j.organisation}</td>
                  <td className="small">{j.employment_type}{j.remote_type && j.remote_type !== 'onsite' ? ` · ${j.remote_type}` : ''}</td>
                  <td className="small">{j.applications ?? 0}</td>
                  <td>{j.status === 'published' ? <Badge tone="ok">Published</Badge> : j.status === 'closed' ? <Badge tone="warn">Closed</Badge> : <Badge tone="neutral">Draft</Badge>}</td>
                  <td>
                    <div className="row" style={{ gap: '.35rem', justifyContent: 'flex-end' }}>
                      <button className="btn sm secondary" onClick={() => setAppsFor(j)}>Applicants ({j.applications ?? 0})</button>
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
          <div className="field"><label>Employer / organisation</label><input value={d.organisation ?? ''} onChange={(e) => set('organisation', e.target.value)} /></div>
          <div className="field"><label>Location</label><input value={d.location ?? ''} onChange={(e) => set('location', e.target.value)} placeholder="London, UK" /></div>
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
          <label className="row" style={{ fontWeight: 400, alignItems: 'center', gap: '.4rem' }}><input type="checkbox" style={{ width: 'auto' }} checked={!!d.featured} onChange={(e) => set('featured', (e.target.checked ? 1 : 0) as number)} /> Featured</label>
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

  const setStatus = (a: Application, status: string) =>
    runMutation(async () => { await adminApi.post(`/api/admin/careers/applications/${a.id}/status`, { status }); refetch() })

  async function downloadCv(a: Application) {
    const tok = adminApi.getToken() ?? ''
    const res = await fetch(`/api/admin/careers/applications/${a.id}/cv`, { headers: { Authorization: 'Bearer ' + tok } })
    if (!res.ok) { alert('No CV on file for this applicant.'); return }
    const el = document.createElement('a'); el.href = URL.createObjectURL(await res.blob()); el.download = a.cv_name || `cv-${a.id}`
    document.body.appendChild(el); el.click(); el.remove(); URL.revokeObjectURL(el.href)
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Applicants — {job.title}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No applications yet.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Applicant</th><th>Applied</th><th>Status</th><th></th></tr></thead>
            <tbody>
              {data.rows.map((a) => (
                <tr key={a.id}>
                  <td>
                    <div>{a.name}</div>
                    <div className="muted small">{a.email}{a.phone ? ` · ${a.phone}` : ''}</div>
                    {a.cover_message && <div className="small" style={{ maxWidth: 320, marginTop: '.2rem' }}>{a.cover_message}</div>}
                    {a.cv_name && <button className="btn ghost sm" style={{ marginTop: '.2rem' }} onClick={() => downloadCv(a)}>CV: {a.cv_name}</button>}
                  </td>
                  <td className="small">{fmtDate(a.created_at)}</td>
                  <td><StatusBadge status={a.status} /></td>
                  <td>
                    <select value={a.status} onChange={(e) => setStatus(a, e.target.value)}>
                      {APP_STATUSES.map((s) => <option key={s} value={s}>{s}</option>)}
                    </select>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}
