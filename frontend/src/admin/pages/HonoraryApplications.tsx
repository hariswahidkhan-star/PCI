import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'

// Row/record types are inline (matching Honorary.tsx) — this surface is owner-only and self-contained.
interface HonApp {
  id: number
  reference: string
  first_name?: string | null
  last_name?: string | null
  email?: string | null
  mobile?: string | null
  country?: string | null
  city?: string | null
  nationality?: string | null
  job_title?: string | null
  employer?: string | null
  years_experience?: number | null
  industry?: string | null
  certification_name?: string | null
  qualifications_json?: string | null
  certifications_json?: string | null
  experience_json?: string | null
  highest_qualification?: string | null
  professional_certifications?: string | null
  relevant_experience?: string | null
  professional_summary?: string | null
  status: string
  award_no?: string | null
  admin_note?: string | null
  created_at?: string | null
  doc_count?: number | null
  shortlisted?: number | null
  idv_status?: string | null
  idv_submitted_at?: string | null
  background_declaration?: number | null
}
interface HonDoc {
  id: number
  doc_kind: string
  filename?: string | null
  mime?: string | null
  size_bytes?: number | null
}
interface HonDetail { application: HonApp; documents: HonDoc[]; idv_documents: HonDoc[] }

/** Parse a structured-history JSON column ([{...}]) defensively; anything unparseable renders as absent. */
function parseRows(json?: string | null): Record<string, string | number | null>[] {
  try {
    const v = JSON.parse(json || '[]')
    return Array.isArray(v) ? v : []
  } catch { return [] }
}
const yearSpan = (r: Record<string, string | number | null>) =>
  r.from_year || r.to_year ? ` (${r.from_year ?? '…'}–${r.to_year ?? 'present'})` : ''
const yearOf = (r: Record<string, string | number | null>) => (r.year ? ` (${r.year})` : '')

const TONE: Record<string, 'ok' | 'err' | 'brand' | 'warn'> = {
  approved: 'ok', pending_review: 'brand', under_review: 'warn', rejected: 'err',
}
const DOC_LABEL: Record<string, string> = {
  resume: 'Résumé / CV', academic: 'Academic qualification', certifications: 'Professional certification', supporting: 'Supporting document',
  photo: 'Passport-style photograph', government_id: 'Government-issued ID',
}

/** Board review of public Honorary Fellow (PCI) applications: view the applicant, download their
 *  documents, add notes, and approve (which confers a real PCI-HON award) / reject / mark under review. */
export default function HonoraryApplications() {
  const [status, setStatus] = useState('pending_review')
  const { data, loading, error, refetch } = useAdminQuery<{ rows: HonApp[] }>(
    `/api/admin/honorary-applications${status ? `?status=${status}` : ''}`,
  )
  const [open, setOpen] = useState<HonDetail | null>(null)
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [idvLink, setIdvLink] = useState<string | null>(null)
  const [linkMsg, setLinkMsg] = useState<string | null>(null)

  async function view(id: number) {
    setErr(null); setIdvLink(null); setLinkMsg(null)
    try {
      const d = await adminApi.get<HonDetail>(`/api/admin/honorary-applications/${id}`)
      setOpen(d)
      setNote(d.application.admin_note ?? '')
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not load the application.')
    }
  }

  async function shortlist(id: number) {
    setBusy(true); setErr(null); setLinkMsg(null)
    try {
      const r = await adminApi.post<{ link: string; emailed: boolean; expires_days: number }>(`/api/admin/honorary-applications/${id}/shortlist`, {})
      setIdvLink(r.link)
      setLinkMsg(r.emailed ? `Secure link emailed to the applicant (expires in ${r.expires_days} days). You can also copy it below.` : `Secure link generated (expires in ${r.expires_days} days). Email is off — copy and send it to the applicant.`)
      await view(id)
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not shortlist.') }
    finally { setBusy(false) }
  }

  async function idvAction(id: number, path: string, confirmMsg?: string) {
    if (confirmMsg && !window.confirm(confirmMsg)) return
    setBusy(true); setErr(null)
    try { await adminApi.post(`/api/admin/honorary-applications/${id}/${path}`, {}); setIdvLink(null); setLinkMsg(null); await view(id) }
    catch (e) { setErr(e instanceof Error ? e.message : 'Action failed.') }
    finally { setBusy(false) }
  }

  async function downloadIdv(appId: number, docId: number) {
    setErr(null)
    try {
      const res = await fetch(`/api/admin/honorary-applications/${appId}/idv/${docId}/file`, {
        headers: { Authorization: 'Bearer ' + (adminApi.getToken() ?? '') },
      })
      if (!res.ok) throw new Error('Could not load the file.')
      const url = URL.createObjectURL(await res.blob())
      window.open(url, '_blank', 'noopener')
      setTimeout(() => URL.revokeObjectURL(url), 60_000)
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not load the file.') }
  }

  async function download(appId: number, docId: number) {
    setErr(null)
    try {
      const res = await fetch(`/api/admin/honorary-applications/${appId}/documents/${docId}/file`, {
        headers: { Authorization: 'Bearer ' + (adminApi.getToken() ?? '') },
      })
      if (!res.ok) throw new Error('Could not load the file.')
      const url = URL.createObjectURL(await res.blob())
      window.open(url, '_blank', 'noopener')
      setTimeout(() => URL.revokeObjectURL(url), 60_000)
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not load the file.')
    }
  }

  async function decide(id: number, s: 'approved' | 'rejected' | 'under_review' | '') {
    setBusy(true)
    setErr(null)
    try {
      await adminApi.post(`/api/admin/honorary-applications/${id}/decide`, { status: s, admin_note: note })
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

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Honorary applications</h1>
      </div>
      <p className="muted" style={{ margin: 0 }}>
        Public applications for the board-conferred <strong>Honorary Fellow (PCI)</strong> recognition. Approving an
        application confers a real, verifiable PCI-HON award — it never issues an examined certification credential.
      </p>

      <Card
        title="Applications"
        action={
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }} aria-label="Status filter">
            <option value="pending_review">Pending review</option>
            <option value="under_review">Under review</option>
            <option value="approved">Approved</option>
            <option value="rejected">Rejected</option>
            <option value="">All</option>
          </select>
        }
      >
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <Empty>No applications with this status.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Applicant</th><th>Role</th><th>Country</th><th>Docs</th><th>Submitted</th><th>Status</th><th /></tr>
            </thead>
            <tbody>
              {rows.map((a) => (
                <tr key={a.id}>
                  <td>
                    <strong>{`${a.first_name ?? ''} ${a.last_name ?? ''}`.trim() || a.email}</strong>
                    <div className="muted small">{a.reference}</div>
                  </td>
                  <td className="small">{a.job_title || '—'}<div className="muted">{a.employer || ''}</div></td>
                  <td className="small">{a.country || '—'}</td>
                  <td>{a.doc_count ?? 0}</td>
                  <td className="small muted">{fmtDate(a.created_at)}</td>
                  <td>
                    <Badge tone={TONE[a.status] ?? 'brand'}>{a.status.replace(/_/g, ' ')}</Badge>
                    {a.award_no && <div className="muted small">{a.award_no}</div>}
                  </td>
                  <td><button className="btn ghost sm" onClick={() => view(a.id)}>Review</button></td>
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
              <h2 style={{ margin: 0 }}>{`${open.application.first_name ?? ''} ${open.application.last_name ?? ''}`.trim()}</h2>
              <button className="btn secondary sm" onClick={() => setOpen(null)}>Close</button>
            </div>
            {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
            <div className="spread" style={{ marginBottom: '.75rem' }}>
              <Badge tone={TONE[open.application.status] ?? 'brand'}>{open.application.status.replace(/_/g, ' ')}</Badge>
              <span className="muted small">{open.application.reference}{open.application.award_no ? ` · ${open.application.award_no}` : ''}</span>
            </div>

            <Section title="Contact">
              <KV k="Email" v={open.application.email} />
              <KV k="Mobile" v={open.application.mobile} />
              <KV k="Country" v={open.application.country} />
              <KV k="City" v={open.application.city} />
              <KV k="Nationality" v={open.application.nationality} />
            </Section>
            <Section title="Professional">
              <KV k="Current job title" v={open.application.job_title} />
              <KV k="Employer / organisation" v={open.application.employer} />
              <KV k="Years of experience" v={open.application.years_experience != null ? String(open.application.years_experience) : null} />
              <KV k="Industry" v={open.application.industry} />
              <KV k="Certification of interest" v={open.application.certification_name} />
            </Section>
            <Section title="Qualifications">
              {parseRows(open.application.qualifications_json).length > 0 ? (
                <ul className="clean small" style={{ margin: 0, paddingLeft: '1rem' }}>
                  {parseRows(open.application.qualifications_json).map((q, i) => (
                    <li key={i}>{q.qualification}{q.institution ? ` — ${q.institution}` : ''}{yearOf(q)}</li>
                  ))}
                </ul>
              ) : (
                <KV k="Highest qualification" v={open.application.highest_qualification} />
              )}
              {parseRows(open.application.certifications_json).length > 0 ? (
                <div>
                  <div className="muted small" style={{ marginTop: '.35rem' }}>Professional certifications</div>
                  <ul className="clean small" style={{ margin: 0, paddingLeft: '1rem' }}>
                    {parseRows(open.application.certifications_json).map((c, i) => (
                      <li key={i}>{c.name}{c.issuer ? ` — ${c.issuer}` : ''}{yearOf(c)}</li>
                    ))}
                  </ul>
                </div>
              ) : (
                <KV k="Professional certifications" v={open.application.professional_certifications} />
              )}
              {parseRows(open.application.experience_json).length > 0 && (
                <div>
                  <div className="muted small" style={{ marginTop: '.35rem' }}>Career history</div>
                  <ul className="clean small" style={{ margin: 0, paddingLeft: '1rem' }}>
                    {parseRows(open.application.experience_json).map((e, i) => (
                      <li key={i}>{[e.role, e.employer].filter(Boolean).join(' — ')}{yearSpan(e)}</li>
                    ))}
                  </ul>
                </div>
              )}
              <KV k="Relevant experience" v={open.application.relevant_experience} block />
              <KV k="Professional summary" v={open.application.professional_summary} block />
            </Section>

            <Section title="Identity verification (shortlist stage)">
              <div className="small" style={{ marginBottom: '.4rem' }}>
                Status:{' '}
                <Badge tone={open.application.idv_status === 'submitted' ? 'ok' : open.application.idv_status === 'deleted' ? 'err' : open.application.idv_status === 'invited' ? 'warn' : 'brand'}>
                  {(open.application.idv_status || 'none').replace(/_/g, ' ')}
                </Badge>
                {open.application.idv_submitted_at && <span className="muted"> · submitted {fmtDate(open.application.idv_submitted_at)}</span>}
              </div>
              {open.application.idv_status === 'submitted' && (
                <div className="small muted" style={{ marginBottom: '.4rem' }}>
                  Background &amp; truthfulness declaration: {open.application.background_declaration ? 'accepted' : '—'}.
                </div>
              )}
              <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap', marginBottom: '.5rem' }}>
                <button className="btn ghost sm" disabled={busy} onClick={() => shortlist(open.application.id)}>
                  {open.application.status === 'approved'
                    ? (open.application.shortlisted ? 'Resend ID verification link' : 'Send ID verification link (photo + government ID)')
                    : (open.application.shortlisted ? 'Regenerate secure link' : 'Shortlist & generate secure link')}
                </button>
                {open.application.idv_status === 'invited' && (
                  <button className="btn ghost sm" disabled={busy} onClick={() => idvAction(open.application.id, 'idv/revoke')}>Revoke link</button>
                )}
                {open.idv_documents.length > 0 && (
                  <button className="btn ghost sm danger" disabled={busy}
                    onClick={() => idvAction(open.application.id, 'idv/delete', 'Delete the applicant’s identity documents? This permanently removes the photo and ID (data minimisation).')}>
                    Delete identity documents
                  </button>
                )}
              </div>
              {linkMsg && <div className="notice small" style={{ marginBottom: '.4rem' }}>{linkMsg}</div>}
              {idvLink && (
                <div className="field" style={{ marginBottom: '.5rem' }}>
                  <input readOnly value={idvLink} onFocus={(e) => e.currentTarget.select()} style={{ fontSize: '.8rem' }} />
                  <button className="btn ghost sm" type="button" style={{ marginTop: '.3rem' }}
                    onClick={() => { navigator.clipboard?.writeText(idvLink); setLinkMsg('Link copied to clipboard.') }}>Copy link</button>
                </div>
              )}
              {open.idv_documents.length === 0 ? (
                <p className="muted small">No identity documents submitted yet.</p>
              ) : (
                <ul className="clean" style={{ display: 'grid', gap: '.4rem', paddingLeft: 0, listStyle: 'none' }}>
                  {open.idv_documents.map((d) => (
                    <li key={d.id} className="row" style={{ justifyContent: 'space-between' }}>
                      <span className="small">{DOC_LABEL[d.doc_kind] || d.doc_kind}{d.filename ? ` — ${d.filename}` : ''}</span>
                      <button className="btn ghost sm" onClick={() => downloadIdv(open.application.id, d.id)}>View</button>
                    </li>
                  ))}
                </ul>
              )}
            </Section>

            <Section title={`Documents (${open.documents.length})`}>
              {open.documents.length === 0 ? (
                <p className="muted small">No documents attached.</p>
              ) : (
                <ul className="clean" style={{ display: 'grid', gap: '.4rem', paddingLeft: 0, listStyle: 'none' }}>
                  {open.documents.map((d) => (
                    <li key={d.id} className="row" style={{ justifyContent: 'space-between' }}>
                      <span className="small">{DOC_LABEL[d.doc_kind] || d.doc_kind}{d.filename ? ` — ${d.filename}` : ''}</span>
                      <button className="btn ghost sm" onClick={() => download(open.application.id, d.id)}>Download</button>
                    </li>
                  ))}
                </ul>
              )}
            </Section>

            <div className="field" style={{ marginTop: '.5rem' }}>
              <label htmlFor="hon-note">Internal note / decision note</label>
              <textarea id="hon-note" rows={3} value={note} onChange={(e) => setNote(e.target.value)} placeholder="Recorded on the application; sent to the applicant on a decision." />
            </div>
            <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
              <button className="btn sm secondary" disabled={busy} onClick={() => decide(open.application.id, '')}>Save note</button>
              {open.application.status !== 'under_review' && open.application.status !== 'approved' && (
                <button className="btn sm secondary" disabled={busy} onClick={() => decide(open.application.id, 'under_review')}>Mark under review</button>
              )}
              {open.application.status !== 'approved' && (
                <button className="btn sm" disabled={busy} onClick={() => decide(open.application.id, 'approved')}>Approve &amp; confer</button>
              )}
              {open.application.status !== 'rejected' && open.application.status !== 'approved' && (
                <button className="btn sm danger" disabled={busy} onClick={() => decide(open.application.id, 'rejected')}>Reject</button>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

function Section({ title, children }: { title: string; children: React.ReactNode }) {
  return (
    <div style={{ marginBottom: '1rem' }}>
      <h4 style={{ margin: '0 0 .4rem' }}>{title}</h4>
      <div style={{ display: 'grid', gap: '.3rem' }}>{children}</div>
    </div>
  )
}
function KV({ k, v, block }: { k: string; v?: string | null; block?: boolean }) {
  if (block) return (
    <div>
      <div className="muted small">{k}</div>
      <div className="small" style={{ whiteSpace: 'pre-wrap' }}>{v || '—'}</div>
    </div>
  )
  return (
    <div className="spread small">
      <span className="muted">{k}</span>
      <span style={{ textAlign: 'right' }}>{v || '—'}</span>
    </div>
  )
}
