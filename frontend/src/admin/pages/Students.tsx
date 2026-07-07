import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type MemberRow, type MemberDetail } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate, fmtMoney } from '../../format'

function MemberDrawer({ id, onClose, onChanged }: { id: number; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<MemberDetail>(`/api/admin/members/${id}`)
  const [busy, setBusy] = useState(false)

  async function setStatus(status: string) {
    setBusy(true)
    try {
      await adminApi.post(`/api/admin/members/${id}/status`, { status })
      refetch()
      onChanged()
    } finally {
      setBusy(false)
    }
  }

  const u = (data?.user ?? {}) as Record<string, unknown>
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Student detail</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data ? null : (
          <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
            <Card title={`${u.first_name ?? ''} ${u.last_name ?? ''}`.trim() || String(u.email ?? '')} action={<StatusBadge status={String(u.status ?? '')} />}>
              <div className="grid cols-2 small">
                <div><span className="muted">Email</span><div>{String(u.email ?? '—')}</div></div>
                <div><span className="muted">Registration</span><div>{String(u.registration_no ?? '—')}</div></div>
                <div><span className="muted">Joined</span><div>{fmtDate(u.created_at)}</div></div>
                <div><span className="muted">Membership</span><div>{data.membership ? String((data.membership as Record<string, unknown>).status ?? '—') : 'None'}</div></div>
              </div>
              <div className="row" style={{ marginTop: '.9rem', flexWrap: 'wrap' }}>
                <span className="muted small">Set status:</span>
                {['active', 'pending', 'deactivated'].map((s) => (
                  <button key={s} className="btn sm secondary" disabled={busy || u.status === s} onClick={() => setStatus(s)}>{s}</button>
                ))}
              </div>
            </Card>

            <Card title={`Payments (${data.payments.length})`}>
              {data.payments.length === 0 ? <Empty>No payments.</Empty> : (
                <table className="data">
                  <thead><tr><th>Date</th><th>Product</th><th>Amount</th><th>Status</th></tr></thead>
                  <tbody>
                    {data.payments.map((p, i) => (
                      <tr key={i}>
                        <td>{fmtDate(p.payment_date)}</td>
                        <td>{String(p.product_type ?? '')}</td>
                        <td>{fmtMoney(p.final_amount, String(p.currency ?? 'USD'))}</td>
                        <td><StatusBadge status={String(p.payment_status ?? '')} /></td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Card>

            <Card title={`Credentials (${data.credentials.length})`}>
              {data.credentials.length === 0 ? <Empty>No credentials issued.</Empty> : (
                <table className="data">
                  <thead><tr><th>ID</th><th>Status</th><th>Expires</th></tr></thead>
                  <tbody>
                    {data.credentials.map((c, i) => (
                      <tr key={i}>
                        <td>{String(c.credential_id ?? '')}</td>
                        <td><StatusBadge status={String(c.status ?? '')} /></td>
                        <td>{fmtDate(c.expires_at)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Card>
          </div>
        )}
      </div>
    </div>
  )
}

export default function Students() {
  const [q, setQ] = useState('')
  const [status, setStatus] = useState('')
  const [selected, setSelected] = useState<number | null>(null)
  const params = new URLSearchParams()
  if (q) params.set('q', q)
  if (status) params.set('status', status)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: MemberRow[]; total: number }>(`/api/admin/members${qs ? '?' + qs : ''}`)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Students</h1>
        {data && <span className="muted small">{data.total} total</span>}
      </div>

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search name or email…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 320 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }}>
            <option value="">All statuses</option>
            <option value="active">Active</option>
            <option value="pending">Pending</option>
            <option value="deactivated">Deactivated</option>
          </select>
        </div>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No students match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Name</th><th>Email</th><th>Status</th><th>Membership</th><th>Paid</th><th>Creds</th><th>Joined</th></tr>
            </thead>
            <tbody>
              {data.rows.map((m) => (
                <tr key={m.id} style={{ cursor: 'pointer' }} onClick={() => setSelected(m.id)}>
                  <td><strong>{`${m.first_name ?? ''} ${m.last_name ?? ''}`.trim() || '—'}</strong></td>
                  <td className="small">{m.email}</td>
                  <td><StatusBadge status={m.status} /></td>
                  <td className="small">{m.membership_status || '—'}</td>
                  <td>{fmtMoney(m.paid_total ?? 0)}</td>
                  <td>{m.credentials ?? 0}</td>
                  <td className="small muted">{fmtDate(m.created_at)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {selected !== null && <MemberDrawer id={selected} onClose={() => setSelected(null)} onChanged={refetch} />}
    </div>
  )
}
