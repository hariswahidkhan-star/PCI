import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'
import { ApiError } from '../../api/client'

interface HonoraryRow {
  id: number
  award_no: string
  recipient_name: string
  user_id?: number | null
  linked_email?: string | null
  citation?: string | null
  designation?: string | null
  status: string
  conferred_at?: string | null
  revoked_at?: string | null
  revoke_reason?: string | null
}

/** Honorary Fellow (PCI) — board/owner-only. A separate recognition registry: no exam, no
 *  entitlement, never a PCP-AI credential. The server enforces owner-only on every call. */
export default function Honorary() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: HonoraryRow[] }>('/api/admin/honorary')
  const [f, setF] = useState({ recipient_name: '', citation: '', user_email: '' })
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)
  const [revokeReason, setRevokeReason] = useState('')

  async function confer() {
    setBusy(true)
    setMsg(null)
    try {
      const r = await adminApi.post<{ award_no: string }>('/api/admin/honorary', {
        recipient_name: f.recipient_name,
        citation: f.citation,
        user_email: f.user_email || undefined,
      })
      setMsg({ ok: true, text: `Conferred — award number ${r.award_no}. The public verify page now shows it, clearly marked honorary.` })
      setF({ recipient_name: '', citation: '', user_email: '' })
      refetch()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setMsg({ ok: false, text: code === 'user_not_found' ? 'No account exists for that email — leave it blank to confer without a linked account.' : e instanceof Error ? e.message : 'Conferral failed.' })
    } finally {
      setBusy(false)
    }
  }

  async function revoke(id: number) {
    if (!window.confirm('Revoke this honorary award? This cannot be undone — the award will show as revoked on the public verify page.')) return
    setBusy(true)
    setMsg(null)
    try {
      await adminApi.post(`/api/admin/honorary/${id}/revoke`, { reason: revokeReason })
      setRevokeReason('')
      refetch()
    } catch (e) {
      setMsg({ ok: false, text: e instanceof Error ? e.message : 'Revoke failed.' })
    } finally {
      setBusy(false)
    }
  }

  const rows = data?.rows ?? []

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Honorary Fellows</h1>
        <p className="muted">
          A board-conferred recognition of distinguished contribution. It is <strong>not</strong> the
          PCP-AI credential, involves no exam, and lives in its own registry (PCI-HON numbers) —
          it can never appear as a passed examination.
        </p>
      </div>

      <Card title="Confer an honorary fellowship" action={<Badge tone="warn">Board / owner only</Badge>}>
        {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.75rem' }}>{msg.text}</div>}
        <div className="grid cols-2">
          <div className="field"><label htmlFor="hon-name">Recipient name (as it will appear)</label>
            <input id="hon-name" value={f.recipient_name} onChange={(e) => setF({ ...f, recipient_name: e.target.value })} placeholder="e.g. Prof. Ayesha Rahman" /></div>
          <div className="field"><label htmlFor="hon-email">Link to an existing account (optional email)</label>
            <input id="hon-email" value={f.user_email} onChange={(e) => setF({ ...f, user_email: e.target.value })} placeholder="their portal email, if any" /></div>
        </div>
        <div className="field"><label htmlFor="hon-citation">Citation (the board's reason — shown on the verify page)</label>
          <textarea id="hon-citation" rows={2} value={f.citation} onChange={(e) => setF({ ...f, citation: e.target.value })} placeholder="For distinguished service to the project controls profession…" /></div>
        <button className="btn" disabled={busy || f.recipient_name.trim().length < 2} onClick={confer}>{busy ? 'Conferring…' : 'Confer Honorary Fellow (PCI)'}</button>
      </Card>

      <Card title={`Awards (${rows.length})`}>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <Empty>No honorary fellowships conferred yet.</Empty>
        ) : (
          <>
            <table className="data">
              <thead><tr><th>Award no.</th><th>Recipient</th><th>Citation</th><th>Conferred</th><th>Status</th><th /></tr></thead>
              <tbody>
                {rows.map((r) => (
                  <tr key={r.id}>
                    <td><strong>{r.award_no}</strong></td>
                    <td>{r.recipient_name}{r.linked_email && <div className="muted small">{r.linked_email}</div>}</td>
                    <td className="small">{r.citation || '—'}</td>
                    <td className="small muted">{fmtDate(r.conferred_at)}</td>
                    <td>
                      {r.status === 'revoked' ? <Badge tone="err">revoked</Badge> : <Badge tone="ok">active</Badge>}
                      {r.revoke_reason && <div className="muted small">{r.revoke_reason}</div>}
                    </td>
                    <td>
                      <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap' }}>
                        <a className="btn sm secondary" href={`/verify.html?id=${encodeURIComponent(r.award_no)}`} target="_blank" rel="noreferrer">Verify</a>
                        {r.status !== 'revoked' && <button className="btn sm danger" disabled={busy} onClick={() => revoke(r.id)}>Revoke</button>}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <div className="field" style={{ marginTop: '.75rem', marginBottom: 0 }}>
              <label htmlFor="hon-reason">Revocation reason (recorded with the action)</label>
              <input id="hon-reason" value={revokeReason} onChange={(e) => setRevokeReason(e.target.value)} placeholder="e.g. conduct inconsistent with the recognition" />
            </div>
            <p className="muted small" style={{ marginTop: '.5rem', marginBottom: 0 }}>
              Revoking an honorary award never touches any exam-earned credential.
            </p>
          </>
        )}
      </Card>
    </div>
  )
}
