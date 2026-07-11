import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useAdminQuery } from '../hooks'
import { useAdminAuth } from '../AdminAuth'
import { adminApi, type FoundingApplication, type FoundingStat } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'

const STATUS_TONE: Record<string, 'ok' | 'err' | 'brand' | 'warn'> = {
  auto_approved: 'ok', approved: 'ok', pending_review: 'brand', rejected: 'err', revoked: 'err',
}

function StatsRow() {
  const { data, loading, error } = useAdminQuery<{ rows: FoundingStat[] }>('/api/admin/founding-stats')
  if (loading) return <Card><Spinner /></Card>
  if (error) return <Card><ErrorNote>{error}</ErrorNote></Card>
  const rows = data?.rows ?? []
  if (rows.length === 0)
    return (
      <Card>
        <Empty>
          No founding codes yet — create one under <Link to="/codes">Discount &amp; founding codes</Link>.
        </Empty>
      </Card>
    )
  return (
    <Card title="Founding codes — live state">
      <table className="data">
        <thead>
          <tr><th>Code</th><th>Route</th><th>Grants</th><th>Redeemed</th><th>Remaining</th><th>Window</th><th>Applications</th><th>State</th></tr>
        </thead>
        <tbody>
          {rows.map((s) => (
            <tr key={s.id}>
              <td><strong>{s.code}</strong></td>
              <td className="small">Founding{!s.auto_approve ? ' · manual review' : ''}</td>
              <td className="small">{['membership', 'exam', 'study'].filter((k) => (s.grants as Record<string, boolean>)[k]).join(' + ')}</td>
              <td>{s.uses.redeemed}</td>
              <td>{s.uses.remaining == null ? '∞' : s.uses.remaining}</td>
              <td className="small muted">
                {s.window.until ? <>until {fmtDate(s.window.until)}{s.window.days_left != null && s.window.days_left >= 0 ? ` · ${Math.ceil(s.window.days_left)}d left` : ''}</> : 'open-ended'}
              </td>
              <td className="small">
                {Object.entries(s.applications).length === 0 ? '—'
                  : Object.entries(s.applications).map(([k, n]) => `${k.replace(/_/g, ' ')}: ${n}`).join(' · ')}
              </td>
              <td>{s.usable ? <Badge tone="ok">open</Badge> : <Badge tone="err">{s.state.replace(/_/g, ' ')}</Badge>}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </Card>
  )
}

export default function Founding() {
  const { me, can } = useAdminAuth()
  // The page is reachable with EITHER perm (members or codes), but its two panels hit
  // separately-gated endpoints: founding-stats needs 'codes', founding-applications needs 'members'.
  // Render each only for admins who can load it, so a single-perm role sees a working panel
  // instead of a half-broken page with a permission error where the other panel would be.
  const canCodes = !!me?.is_owner || can('codes')
  const canMembers = !!me?.is_owner || can('members')
  const [status, setStatus] = useState('pending_review')
  const { data, loading, error, refetch } = useAdminQuery<{ rows: FoundingApplication[] }>(
    canMembers ? `/api/admin/founding-applications${status ? `?status=${status}` : ''}` : null,
  )
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState('')
  const [err, setErr] = useState<string | null>(null)

  async function decide(id: number, s: 'approved' | 'rejected' | 'revoked') {
    setBusy(true)
    setErr(null)
    try {
      await adminApi.post(`/api/admin/founding-applications/${id}/decide`, { status: s, admin_note: note })
      setNote('')
      refetch()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Decision failed.')
    } finally {
      setBusy(false)
    }
  }

  async function viewEvidence(id: number) {
    setErr(null)
    try {
      const res = await fetch(`/api/admin/founding-applications/${id}/evidence`, {
        headers: { Authorization: 'Bearer ' + (adminApi.getToken() ?? '') },
      })
      if (!res.ok) throw new Error('Could not load the evidence file.')
      const url = URL.createObjectURL(await res.blob())
      window.open(url, '_blank', 'noopener')
      setTimeout(() => URL.revokeObjectURL(url), 60_000)
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not load the evidence file.')
    }
  }

  const rows = data?.rows ?? []

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Founding stage</h1>
        <Link className="btn secondary sm" to="/codes">Manage codes</Link>
      </div>

      {canCodes && <StatsRow />}

      {canMembers && (
      <Card
        title="Founding applications"
        action={
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }} aria-label="Status filter">
            <option value="pending_review">Pending review</option>
            <option value="auto_approved">Auto-approved</option>
            <option value="approved">Approved</option>
            <option value="rejected">Rejected</option>
            <option value="revoked">Revoked</option>
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
          <>
            <table className="data">
              <thead>
                <tr><th>Applicant</th><th>Code</th><th>Declared</th><th>Submitted</th><th>Status</th><th /></tr>
              </thead>
              <tbody>
                {rows.map((a) => (
                  <tr key={a.id}>
                    <td>
                      <strong>{`${a.first_name ?? ''} ${a.last_name ?? ''}`.trim() || a.email}</strong>
                      <div className="muted small">{a.email}</div>
                    </td>
                    <td className="small">{a.code}</td>
                    <td className="small">
                      {a.declared_experience_years ?? 0} yrs · {a.declared_role || '—'}
                      <div className="muted">{a.declared_qualification || 'no qualification declared'}</div>
                    </td>
                    <td className="small muted">{fmtDate(a.created_at)}</td>
                    <td>
                      <Badge tone={STATUS_TONE[a.status] ?? 'brand'}>{a.status.replace(/_/g, ' ')}</Badge>
                      {a.admin_note && <div className="muted small">{a.admin_note}</div>}
                    </td>
                    <td>
                      <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap' }}>
                        {a.has_evidence === 1 && <button className="btn sm secondary" onClick={() => viewEvidence(a.id)}>Evidence</button>}
                        {a.status === 'pending_review' && <button className="btn sm" disabled={busy} onClick={() => decide(a.id, 'approved')}>Approve</button>}
                        {a.status === 'pending_review' && <button className="btn sm danger" disabled={busy} onClick={() => decide(a.id, 'rejected')}>Reject</button>}
                        {(a.status === 'approved' || a.status === 'auto_approved') && (
                          <button className="btn sm danger" disabled={busy} onClick={() => decide(a.id, 'revoked')}>Revoke</button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <div className="field" style={{ marginTop: '.75rem', marginBottom: 0 }}>
              <label htmlFor="fnd-note">Decision note (recorded and sent to the applicant)</label>
              <input id="fnd-note" value={note} onChange={(e) => setNote(e.target.value)} placeholder="e.g. evidence does not support the declared experience" />
            </div>
            <p className="muted small" style={{ marginTop: '.5rem', marginBottom: 0 }}>
              Approving grants membership/exam through the same settlement path as a payment. Revoking
              deactivates an <em>unused</em> grant only — a credential already earned by a passed exam is never revoked.
            </p>
          </>
        )}
      </Card>
      )}
    </div>
  )
}
