import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type MemberRow, type MemberDetail, type IdentityDocRow } from '../api'
import { Card, StatusBadge, Badge, Spinner, ErrorNote, Empty, rowActivate } from '../../components/ui'
import { fmtDate, fmtMoney } from '../../format'
import { ApiError } from '../../api/client'

/** Fee waiver: grant an exam entitlement without payment (mirrors the webhook grant server-side). */
function WaiverCard({ id, onGranted }: { id: number; onGranted: () => void }) {
  const { data } = useAdminQuery<{ rows: { id: number; code: string; name: string }[] }>('/api/certifications')
  const [cert, setCert] = useState('')
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)

  async function grant() {
    setBusy(true)
    setMsg(null)
    try {
      const r = await adminApi.post<{ reference: string }>(`/api/admin/students/${id}/exam-waiver`, { certification_id: cert || undefined, note: note || undefined })
      setMsg({ ok: true, text: `Exam access granted without fee — reference ${r.reference}. The student has been notified in their portal.` })
      setNote('')
      onGranted()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setMsg({ ok: false, text: code === 'already_entitled' ? 'This student already has an unused exam entitlement for that certification.' : e instanceof Error ? e.message : 'Waiver failed.' })
    } finally {
      setBusy(false)
    }
  }

  return (
    <Card title="Waive exam fee">
      <p className="muted small" style={{ marginTop: 0 }}>
        Grants this student exam access without payment (scholarship, comp, corporate seat). All the
        usual eligibility and one-attempt rules still apply. To undo, mark the WAIVE payment refunded.
      </p>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.6rem' }}>{msg.text}</div>}
      <div className="row" style={{ flexWrap: 'wrap' }}>
        <select value={cert} onChange={(e) => setCert(e.target.value)} style={{ maxWidth: 260 }}>
          <option value="">Default certification</option>
          {(data?.rows ?? []).map((c) => <option key={c.id} value={c.id}>{c.code} — {c.name}</option>)}
        </select>
        <input placeholder="Internal note (optional)" value={note} onChange={(e) => setNote(e.target.value)} style={{ maxWidth: 260 }} />
        <button className="btn sm" disabled={busy} onClick={grant}>{busy ? 'Granting…' : 'Grant exam access'}</button>
      </div>
    </Card>
  )
}

const DOC_TONE: Record<string, 'ok' | 'err' | 'brand'> = { verified: 'ok', rejected: 'err', submitted: 'brand' }

/** Identity documents: view the uploaded file (authenticated fetch → blob tab) and verify/reject. */
function IdentityDocs({ id, docs, onChanged }: { id: number; docs: IdentityDocRow[]; onChanged: () => void }) {
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState('')
  const [err, setErr] = useState<string | null>(null)

  async function review(docId: number, status: 'verified' | 'rejected') {
    setBusy(true)
    setErr(null)
    try {
      await adminApi.post(`/api/admin/students/${id}/identity-document/${docId}/review`, { status, note })
      setNote('')
      onChanged()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Review failed.')
    } finally {
      setBusy(false)
    }
  }

  async function view(docId: number) {
    setErr(null)
    try {
      const res = await fetch(`/api/admin/students/${id}/identity-document/${docId}/file`, {
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

  return (
    <Card title={`Identity documents (${docs.length})`}>
      {docs.length === 0 ? (
        <Empty>No government ID uploaded yet — the student cannot book an exam until they provide one.</Empty>
      ) : (
        <>
          {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
          <table className="data">
            <thead><tr><th>Type</th><th>File</th><th>Uploaded</th><th>Status</th><th /></tr></thead>
            <tbody>
              {docs.map((d) => (
                <tr key={d.id}>
                  <td>{(d.doc_kind ?? '').replace(/_/g, ' ')}</td>
                  <td className="small">{d.filename ?? '—'}{d.size_bytes ? ` · ${Math.round(Number(d.size_bytes) / 1024)} KB` : ''}</td>
                  <td className="small muted">{fmtDate(d.created_at)}</td>
                  <td>
                    <Badge tone={DOC_TONE[d.status] ?? 'brand'}>{d.status}</Badge>
                    {d.review_note && <div className="muted small">{d.review_note}</div>}
                  </td>
                  <td>
                    <div className="row" style={{ flexWrap: 'wrap', gap: '.35rem' }}>
                      <button className="btn sm secondary" onClick={() => view(d.id)}>View</button>
                      {d.status !== 'verified' && <button className="btn sm" disabled={busy} onClick={() => review(d.id, 'verified')}>Verify</button>}
                      {d.status !== 'rejected' && <button className="btn sm danger" disabled={busy} onClick={() => review(d.id, 'rejected')}>Reject</button>}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className="field" style={{ marginTop: '.75rem', marginBottom: 0 }}>
            <label htmlFor="idd-note">Review note (sent to the student with a rejection)</label>
            <input id="idd-note" value={note} onChange={(e) => setNote(e.target.value)} placeholder="e.g. photo unreadable — please re-upload" />
          </div>
        </>
      )}
    </Card>
  )
}

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

            <WaiverCard id={id} onGranted={() => { refetch(); onChanged() }} />

            <IdentityDocs id={id} docs={data.identity_documents ?? []} onChanged={refetch} />

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
                <tr key={m.id} {...rowActivate(() => setSelected(m.id))}>
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
