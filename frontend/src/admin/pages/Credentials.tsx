import { useState, useEffect } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type CredentialRow, type MemberRow } from '../api'
import { ApiError } from '../../api/client'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate, isPast } from '../../format'

function IssueForm({ onClose, onSaved }: { onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState({ credential_id: '', user_id: '', holder_name: '', certification_id: '', expires_at: '' })
  const [studentQuery, setStudentQuery] = useState('')
  const [studentSearch, setStudentSearch] = useState('')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const set = (k: keyof typeof f, v: string) => setF((p) => ({ ...p, [k]: v }))
  const { data: certifications } = useAdminQuery<{ rows: { id: number; code: string; acronym?: string; name: string }[] }>('/api/certifications')
  const { data: members } = useAdminQuery<{ rows: MemberRow[] }>(
    studentSearch.length >= 2 ? `/api/admin/members?q=${encodeURIComponent(studentSearch)}&limit=20` : null,
  )
  useEffect(() => {
    const t = setTimeout(() => setStudentSearch(studentQuery.trim()), 300)
    return () => clearTimeout(t)
  }, [studentQuery])

  function chooseStudent(id: string) {
    const member = members?.rows.find((m) => String(m.id) === id)
    setF((prev) => ({
      ...prev,
      user_id: id,
      holder_name: member
        ? `${member.first_name ?? ''} ${member.last_name ?? ''}`.trim() || member.email
        : prev.holder_name,
    }))
  }

  async function save() {
    setBusy(true)
    setError(null)
    try {
      await adminApi.post('/api/admin/credentials', {
        credential_id: f.credential_id,
        user_id: f.user_id ? Number(f.user_id) : null,
        holder_name: f.holder_name,
        certification_id: f.certification_id ? Number(f.certification_id) : undefined,
        expires_at: f.expires_at || null,
      })
      onSaved()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setError(code === 'duplicate_or_invalid'
        ? 'That credential ID already exists.'
        : code === 'bad_user'
          ? 'Select a valid student account.'
          : code === 'missing_fields'
            ? 'Credential ID and holder name are required.'
            : e instanceof Error ? e.message : 'Could not issue.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Issue credential</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
        <div className="field"><label>Credential ID</label><input value={f.credential_id} onChange={(e) => set('credential_id', e.target.value.toUpperCase())} placeholder="PCI-PCLAI-2026-000001" /></div>
        <div className="field">
          <label>Find student <span className="muted small">(linking enables portal download)</span></label>
          <input value={studentQuery} onChange={(e) => setStudentQuery(e.target.value)} placeholder="Search by name or email…" />
        </div>
        {studentSearch.length >= 2 && (
          <div className="field">
            <label>Student account</label>
            <select value={f.user_id} onChange={(e) => chooseStudent(e.target.value)}>
              <option value="">— leave unlinked —</option>
              {(members?.rows ?? []).map((m) => (
                <option key={m.id} value={m.id}>
                  {`${m.first_name ?? ''} ${m.last_name ?? ''}`.trim() || 'Unnamed'} — {m.email}
                </option>
              ))}
            </select>
            {members && members.rows.length === 0 && <div className="muted small">No matching student.</div>}
          </div>
        )}
        <div className="field"><label>Holder name</label><input value={f.holder_name} onChange={(e) => set('holder_name', e.target.value)} /></div>
        <div className="field">
          <label>Certification</label>
          <select value={f.certification_id} onChange={(e) => set('certification_id', e.target.value)}>
            <option value="">Default certification</option>
            {(certifications?.rows ?? []).map((c) => (
              <option key={c.id} value={c.id}>{c.code || c.acronym} — {c.name}</option>
            ))}
          </select>
        </div>
        <div className="field"><label>Expires</label><input type="date" value={f.expires_at} onChange={(e) => set('expires_at', e.target.value)} /></div>
        <button className="btn" disabled={busy || !f.credential_id || !f.holder_name} onClick={save}>{busy ? 'Issuing…' : 'Issue credential'}</button>
      </div>
    </div>
  )
}

// Optional Credly-network export status + bulk sync. Shows nothing intrusive when not configured.
function CredlyPanel() {
  const { data } = useAdminQuery<{ configured: boolean }>('/api/admin/credly/status')
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState<string | null>(null)
  if (!data) return null
  async function sync() {
    setBusy(true); setNote(null)
    try {
      const r = await adminApi.post<{ pushed: number; failed: number }>('/api/admin/credly/sync', {})
      setNote(`Pushed ${r.pushed} credential(s) to Credly${r.failed ? `, ${r.failed} failed` : ''}.`)
    } catch (e) {
      setNote(e instanceof Error ? e.message : 'Sync failed.')
    } finally { setBusy(false) }
  }
  return (
    <Card title="Credly network export">
      {data.configured ? (
        <div className="row" style={{ flexWrap: 'wrap', gap: '.5rem', alignItems: 'center' }}>
          <StatusBadge status="active" />
          <span className="small muted">Earned credentials with a mapped Credly badge-template are exported to Credly. Revocations propagate automatically.</span>
          <button className="btn sm" disabled={busy} onClick={sync}>{busy ? 'Syncing…' : 'Sync pending to Credly'}</button>
          {note && <span className="small">{note}</span>}
        </div>
      ) : (
        <div className="small muted">
          PCI issues its own verifiable badges (Open Badges) already. To ALSO push badges to the Credly network,
          set <code>CREDLY_API_TOKEN</code> and <code>CREDLY_ORG_ID</code> in the environment and map each
          certification to a Credly badge-template ID (in the certification editor).
        </div>
      )}
    </Card>
  )
}

export default function Credentials() {
  const [status, setStatus] = useState('')
  const [q, setQ] = useState('')
  const [dq, setDq] = useState('')
  const [issuing, setIssuing] = useState(false)
  // Debounce the search term so the query path (and its page-level Spinner) doesn't churn on
  // every keystroke — the input stays instant while the list refetches once typing settles.
  useEffect(() => {
    const t = setTimeout(() => setDq(q), 300)
    return () => clearTimeout(t)
  }, [q])
  const params = new URLSearchParams()
  if (status) params.set('status', status)
  if (dq) params.set('q', dq)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: CredentialRow[] }>(`/api/admin/credentials${qs ? '?' + qs : ''}`)

  async function setCredStatus(c: CredentialRow, s: string) {
    if (s === 'revoked' && !confirm(`Revoke credential ${c.credential_id}?`)) return
    try {
      await adminApi.post(`/api/admin/credentials/${c.id}/status`, { status: s })
      refetch()
    } catch (e) {
      alert(e instanceof Error ? e.message : 'Could not update.')
    }
  }

  // Upload a custom certificate PDF (examined or honorary) to replace the auto-generated one; the
  // student/admin download routes then serve the uploaded file. Read as a data URI and POST it.
  const [uploading, setUploading] = useState('')
  function uploadCert(c: CredentialRow, file: File) {
    if (file.type !== 'application/pdf') { alert('Please choose a PDF file.'); return }
    const reader = new FileReader()
    reader.onload = async () => {
      setUploading(c.credential_id)
      try {
        await adminApi.post(`/api/admin/credentials/${encodeURIComponent(c.credential_id)}/upload-certificate`, { data_uri: reader.result })
        alert(`Certificate uploaded for ${c.credential_id}.`)
        refetch()
      } catch (e) { alert(e instanceof Error ? e.message : 'Upload failed.') }
      finally { setUploading('') }
    }
    reader.readAsDataURL(file)
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Credentials</h1>
        <button className="btn sm" onClick={() => setIssuing(true)}>Issue credential</button>
      </div>

      <CredlyPanel />

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search ID or holder…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 300 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All statuses</option>
            <option value="active">Active</option>
            <option value="expired">Expired</option>
            <option value="revoked">Revoked</option>
          </select>
        </div>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No credentials match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Credential ID</th><th>Holder</th><th>Type</th><th>Issued</th><th>Expires</th><th>Status</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((c) => {
                const lapsed = c.status === 'active' && isPast(c.expires_at)
                return (
                  <tr key={c.id}>
                    <td><strong>{c.credential_id}</strong></td>
                    <td>{c.holder_name}</td>
                    <td className="small">{c.credential}</td>
                    <td className="small muted">{fmtDate(c.issued_at)}</td>
                    <td className="small">{fmtDate(c.expires_at)}</td>
                    <td><StatusBadge status={lapsed ? 'expired' : c.status} /></td>
                    <td>
                      <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                        <label className="btn ghost sm" style={{ cursor: 'pointer', margin: 0 }} title="Upload a custom certificate PDF (examined or honorary)">
                          {uploading === c.credential_id ? 'Uploading…' : 'Upload cert'}
                          <input type="file" accept="application/pdf" style={{ display: 'none' }}
                            onChange={(e) => { const fl = e.target.files?.[0]; if (fl) uploadCert(c, fl); e.currentTarget.value = '' }} />
                        </label>
                        {c.status === 'active' ? (
                          <button className="btn ghost sm" onClick={() => setCredStatus(c, 'revoked')}>Revoke</button>
                        ) : c.status === 'revoked' ? (
                          <button className="btn ghost sm" onClick={() => setCredStatus(c, 'active')}>Reinstate</button>
                        ) : null}
                      </div>
                    </td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        )}
      </Card>

      {issuing && <IssueForm onClose={() => setIssuing(false)} onSaved={() => { setIssuing(false); refetch() }} />}
    </div>
  )
}
