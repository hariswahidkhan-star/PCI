import { useState, useEffect } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type CredentialRow } from '../api'
import { ApiError } from '../../api/client'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate, isPast } from '../../format'

function IssueForm({ onClose, onSaved }: { onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState({ credential_id: '', holder_name: '', credential: 'PCP-AI', expires_at: '' })
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const set = (k: keyof typeof f, v: string) => setF((p) => ({ ...p, [k]: v }))

  async function save() {
    setBusy(true)
    setError(null)
    try {
      await adminApi.post('/api/admin/credentials', { ...f, expires_at: f.expires_at || null })
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
        <div className="field"><label>Credential ID</label><input value={f.credential_id} onChange={(e) => set('credential_id', e.target.value.toUpperCase())} placeholder="PCP-AI-2026-0001" /></div>
        <div className="field"><label>Holder name</label><input value={f.holder_name} onChange={(e) => set('holder_name', e.target.value)} /></div>
        <div className="field"><label>Credential</label><input value={f.credential} onChange={(e) => set('credential', e.target.value)} /></div>
        <div className="field"><label>Expires</label><input type="date" value={f.expires_at} onChange={(e) => set('expires_at', e.target.value)} /></div>
        <button className="btn" disabled={busy || !f.credential_id || !f.holder_name} onClick={save}>{busy ? 'Issuing…' : 'Issue credential'}</button>
      </div>
    </div>
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

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Credentials</h1>
        <button className="btn sm" onClick={() => setIssuing(true)}>Issue credential</button>
      </div>

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
                      {c.status === 'active' ? (
                        <button className="btn ghost sm" onClick={() => setCredStatus(c, 'revoked')}>Revoke</button>
                      ) : c.status === 'revoked' ? (
                        <button className="btn ghost sm" onClick={() => setCredStatus(c, 'active')}>Reinstate</button>
                      ) : null}
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
