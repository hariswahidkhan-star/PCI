import { useState, useEffect } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type CredentialRow } from '../api'
import { ApiError } from '../../api/client'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate, isPast } from '../../format'

interface MemberOption { id: number; first_name?: string | null; last_name?: string | null; email?: string | null }
interface CertOption { id: number; code: string; name: string }

function IssueForm({ onClose, onSaved }: { onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState({ credential_id: '', holder_name: '', credential: '', user_id: '', certification_id: '', expires_at: '' })
  const [members, setMembers] = useState<MemberOption[]>([])
  const [certs, setCerts] = useState<CertOption[]>([])
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const set = (k: keyof typeof f, v: string) => setF((p) => ({ ...p, [k]: v }))

  useEffect(() => {
    let live = true
    adminApi.get<{ rows: MemberOption[] }>('/api/admin/members?limit=500')
      .then((d) => { if (live) setMembers(d.rows ?? []) })
      .catch(() => { if (live) setMembers([]) })
    adminApi.get<{ rows: CertOption[] }>('/api/certifications')
      .then((d) => {
        if (!live) return
        setCerts(d.rows ?? [])
        if (d.rows?.[0]?.id) setF((p) => p.certification_id ? p : ({ ...p, certification_id: String(d.rows[0].id) }))
      })
      .catch(() => { if (live) setCerts([]) })
    return () => { live = false }
  }, [])

  function memberName(m?: MemberOption | null) {
    return [m?.first_name, m?.last_name].filter(Boolean).join(' ').trim()
  }

  function chooseUser(id: string) {
    const m = members.find((x) => String(x.id) === id)
    setF((p) => ({
      ...p,
      user_id: id,
      holder_name: p.holder_name.trim() ? p.holder_name : (memberName(m) || p.holder_name),
    }))
  }

  async function save() {
    setBusy(true)
    setError(null)
    try {
      const linked = members.find((m) => String(m.id) === f.user_id)
      const holder = f.holder_name.trim() || memberName(linked)
      const body: Record<string, unknown> = {
        credential_id: f.credential_id.trim(),
        holder_name: holder,
        expires_at: f.expires_at || null,
      }
      if (f.credential.trim()) body.credential = f.credential.trim()
      if (f.user_id) body.user_id = Number(f.user_id)
      if (f.certification_id) body.certification_id = Number(f.certification_id)
      await adminApi.post('/api/admin/credentials', body)
      onSaved()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setError(code === 'duplicate_or_invalid' ? 'That credential ID already exists.' : code === 'missing_fields' ? 'Credential ID and holder name are required.' : e instanceof Error ? e.message : 'Could not issue.')
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
          <label>Link student account</label>
          <input list="credential-members" value={f.user_id} onChange={(e) => chooseUser(e.target.value)} placeholder="Optional user ID" />
          <datalist id="credential-members">
            {members.map((m) => (
              <option key={m.id} value={m.id}>{memberName(m) || m.email || `#${m.id}`} — {m.email}</option>
            ))}
          </datalist>
          <p className="muted small" style={{ margin: '.25rem 0 0' }}>When linked, the certificate appears in the student's portal.</p>
        </div>
        <div className="field"><label>Holder name</label><input value={f.holder_name} onChange={(e) => set('holder_name', e.target.value)} /></div>
        <div className="field">
          <label>Certification</label>
          <select value={f.certification_id} onChange={(e) => set('certification_id', e.target.value)}>
            {certs.map((c) => <option key={c.id} value={c.id}>{c.code} — {c.name}</option>)}
            {certs.length === 0 && <option value="">Default certification</option>}
          </select>
        </div>
        <div className="field"><label>Credential label (optional)</label><input value={f.credential} onChange={(e) => set('credential', e.target.value)} placeholder="Leave blank to use certification default" /></div>
        <div className="field"><label>Expires</label><input type="date" value={f.expires_at} onChange={(e) => set('expires_at', e.target.value)} /></div>
        <button className="btn" disabled={busy || !f.credential_id || !(f.holder_name.trim() || memberName(members.find((m) => String(m.id) === f.user_id)))} onClick={save}>{busy ? 'Issuing…' : 'Issue credential'}</button>
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
