import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import type { CertApplication, CertRow } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'

const TONE: Record<string, 'ok' | 'err' | 'brand' | 'warn' | 'neutral'> = {
  approved: 'ok', submitted: 'brand', under_review: 'warn', info_requested: 'warn', rejected: 'err', withdrawn: 'neutral',
}

/** Review candidate applications per certification and route (Phase 4b). Each record is keyed by
 *  (candidate, certification, route), so a decision here never touches another credential. Approving
 *  clears the applicant to proceed; rejecting closes the application. */
export default function Applications() {
  const [certId, setCertId] = useState('')
  const [status, setStatus] = useState('submitted')
  const [routeKey, setRouteKey] = useState('')

  const qs = new URLSearchParams()
  if (certId) qs.set('certification_id', certId)
  if (status) qs.set('status', status)
  if (routeKey) qs.set('route_key', routeKey)
  const { data, loading, error, refetch } = useAdminQuery<{ rows: CertApplication[] }>(
    `/api/admin/applications${qs.toString() ? `?${qs}` : ''}`,
  )
  const certs = useAdminQuery<{ rows: CertRow[] }>('/api/certifications')

  const [open, setOpen] = useState<CertApplication | null>(null)
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  function view(a: CertApplication) {
    setErr(null)
    setOpen(a)
    setNote(a.admin_note ?? '')
  }

  async function decide(id: number, action: 'approve' | 'reject' | 'request_info' | 'under_review') {
    setBusy(true)
    setErr(null)
    try {
      await adminApi.post(`/api/admin/applications/${id}/decision`, { action, note })
      setOpen(null)
      setNote('')
      refetch()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Decision failed.')
    } finally {
      setBusy(false)
    }
  }

  const rows = data?.rows ?? []
  const certRows = certs.data?.rows ?? []

  function prettyData(json?: string | null): [string, string][] {
    if (!json) return []
    try {
      const o = JSON.parse(json) as Record<string, unknown>
      return Object.entries(o).map(([k, v]) => [k.replace(/_/g, ' '), typeof v === 'object' ? JSON.stringify(v) : String(v)])
    } catch {
      return []
    }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Applications</h1>
      </div>
      <p className="muted" style={{ margin: 0 }}>
        Candidate applications by certification and route. Each application is scoped to a single credential —
        approving one never affects another. Routes that require approval enter review here; auto-approved routes
        clear on submission.
      </p>

      <Card
        title="Applications"
        action={
          <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
            <select value={certId} onChange={(e) => setCertId(e.target.value)} aria-label="Certification filter">
              <option value="">All certifications</option>
              {certRows.map((c) => (
                <option key={c.id} value={c.id}>{c.acronym || c.code}</option>
              ))}
            </select>
            <select value={status} onChange={(e) => setStatus(e.target.value)} aria-label="Status filter">
              <option value="submitted">Submitted</option>
              <option value="under_review">Under review</option>
              <option value="info_requested">Info requested</option>
              <option value="approved">Approved</option>
              <option value="rejected">Rejected</option>
              <option value="">All</option>
            </select>
            <input
              value={routeKey}
              onChange={(e) => setRouteKey(e.target.value.trim())}
              placeholder="Route (e.g. standard)"
              aria-label="Route filter"
              style={{ maxWidth: 160 }}
            />
          </div>
        }
      >
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <Empty>No applications match these filters.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Applicant</th><th>Certification</th><th>Route</th><th>Submitted</th><th>Status</th><th /></tr>
            </thead>
            <tbody>
              {rows.map((a) => (
                <tr key={a.id}>
                  <td>
                    <strong>{`${a.first_name ?? ''} ${a.last_name ?? ''}`.trim() || a.email}</strong>
                    <div className="muted small">{a.application_no}</div>
                  </td>
                  <td className="small">{a.cert_acronym || a.cert_name || `#${a.certification_id}`}</td>
                  <td className="small">{a.route_key.replace(/_/g, ' ')}</td>
                  <td className="small muted">{fmtDate(a.created_at)}</td>
                  <td><Badge tone={TONE[a.status] ?? 'brand'}>{a.status.replace(/_/g, ' ')}</Badge></td>
                  <td><button className="btn ghost sm" onClick={() => view(a)}>Review</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {open && (
        <div className="drawer-backdrop" onClick={() => setOpen(null)}>
          <div className="drawer" onClick={(e) => e.stopPropagation()}>
            <div className="spread" style={{ marginBottom: '1rem' }}>
              <h2 style={{ margin: 0 }}>{`${open.first_name ?? ''} ${open.last_name ?? ''}`.trim() || open.email}</h2>
              <button className="btn secondary sm" onClick={() => setOpen(null)}>Close</button>
            </div>
            {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
            <div className="spread" style={{ marginBottom: '.75rem' }}>
              <Badge tone={TONE[open.status] ?? 'brand'}>{open.status.replace(/_/g, ' ')}</Badge>
              <span className="muted small">{open.application_no}</span>
            </div>

            <div style={{ marginBottom: '1rem', display: 'grid', gap: '.3rem' }}>
              <div className="spread small"><span className="muted">Email</span><span>{open.email || '—'}</span></div>
              <div className="spread small"><span className="muted">Certification</span><span>{open.cert_acronym || open.cert_name || `#${open.certification_id}`}</span></div>
              <div className="spread small"><span className="muted">Route</span><span>{open.route_key.replace(/_/g, ' ')}</span></div>
              <div className="spread small"><span className="muted">Stage</span><span>{(open.workflow_stage || '').replace(/_/g, ' ') || '—'}</span></div>
              <div className="spread small"><span className="muted">Submitted</span><span>{fmtDate(open.created_at)}</span></div>
            </div>

            {prettyData(open.data_json).length > 0 && (
              <div style={{ marginBottom: '1rem' }}>
                <h4 style={{ margin: '0 0 .4rem' }}>Declared details</h4>
                <div style={{ display: 'grid', gap: '.3rem' }}>
                  {prettyData(open.data_json).map(([k, v]) => (
                    <div key={k} className="spread small"><span className="muted">{k}</span><span style={{ textAlign: 'right' }}>{v}</span></div>
                  ))}
                </div>
              </div>
            )}

            <div className="field" style={{ marginTop: '.5rem' }}>
              <label htmlFor="app-note">Internal / decision note</label>
              <textarea id="app-note" rows={3} value={note} onChange={(e) => setNote(e.target.value)} placeholder="Recorded on the application." />
            </div>
            <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
              {open.status !== 'under_review' && open.status !== 'approved' && (
                <button className="btn sm secondary" disabled={busy} onClick={() => decide(open.id, 'under_review')}>Mark under review</button>
              )}
              {open.status !== 'info_requested' && open.status !== 'approved' && (
                <button className="btn sm secondary" disabled={busy} onClick={() => decide(open.id, 'request_info')}>Request info</button>
              )}
              {open.status !== 'approved' && (
                <button className="btn sm" disabled={busy} onClick={() => decide(open.id, 'approve')}>Approve</button>
              )}
              {open.status !== 'rejected' && open.status !== 'approved' && (
                <button className="btn sm danger" disabled={busy} onClick={() => decide(open.id, 'reject')}>Reject</button>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  )
}
