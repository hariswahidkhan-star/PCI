import { useState, useEffect, useRef } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type PaymentRow } from '../api'
import { Card, StatusBadge, Badge, Spinner, ErrorNote, Empty, Stat } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtDate, fmtMoney, titleCase } from '../../format'
import { ApiError, UnauthorizedError } from '../../api/client'

interface PaymentsResp {
  rows: PaymentRow[]
  totals: { paid: number; n: number; refunded: number }
}

const TABS = ['Payments', 'Reconciliation'] as const

export default function Payments() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Payments')
  return (
    <div className="page">
      <PageHeader title="Payments" />
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>)}
      </div>
      {tab === 'Payments' ? <PaymentsTab /> : <ReconciliationTab />}
    </div>
  )
}

function PaymentsTab() {
  const [status, setStatus] = useState('')
  const [product, setProduct] = useState('')
  const [q, setQ] = useState('')
  const [dq, setDq] = useState('')
  // Debounce the search term so the query path (and its page-level Spinner) doesn't churn on
  // every keystroke — the input stays instant while the list refetches once typing settles.
  useEffect(() => {
    const t = setTimeout(() => setDq(q), 300)
    return () => clearTimeout(t)
  }, [q])
  const params = new URLSearchParams()
  if (status) params.set('status', status)
  if (product) params.set('product', product)
  if (dq) params.set('q', dq)
  const qs = params.toString()
  const { data, loading, error } = useAdminQuery<PaymentsResp>(`/api/admin/payments${qs ? '?' + qs : ''}`)

  return (
    <>
      {data && (
        <div className="grid cols-3">
          <Card><Stat n={fmtMoney(data.totals.paid)} k="Paid (filtered)" /></Card>
          <Card><Stat n={fmtMoney(data.totals.refunded)} k="Refunded" /></Card>
          <Card><Stat n={data.totals.n} k="Transactions" /></Card>
        </div>
      )}

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input aria-label="Search ref or email" placeholder="Search ref or email…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 280 }} />
          <select aria-label="Filter by status" value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All statuses</option>
            <option value="paid">Paid</option>
            <option value="refunded">Refunded</option>
            <option value="failed">Failed</option>
          </select>
          <select aria-label="Filter by product" value={product} onChange={(e) => setProduct(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All products</option>
            <option value="membership">Membership</option>
            <option value="exam">Exam</option>
            <option value="bundle">Bundle</option>
            <option value="renewal">Renewal</option>
          </select>
        </div>
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No payments match.</Empty>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead>
                <tr><th>Date</th><th>Reference</th><th>Customer</th><th>Product</th><th>Amount</th><th>Status</th></tr>
              </thead>
              <tbody>
                {data.rows.map((p) => (
                  <tr key={p.id}>
                    <td className="small">{fmtDate(p.payment_date)}</td>
                    <td className="small muted">{p.reference || '—'}</td>
                    <td className="small">{p.email || '—'}</td>
                    <td>{titleCase(p.product_type)}</td>
                    <td>{fmtMoney(p.final_amount, p.currency || 'USD')}</td>
                    <td><StatusBadge status={p.payment_status} /></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </Card>
    </>
  )
}

// ---------------- reconciliation: every payment with its downstream state ----------------
interface ReconRow {
  id: number
  user_id: number
  email?: string | null
  is_test: boolean
  product: string
  amount: number
  waived_amount?: number | null
  provider?: string | null
  gateway_id?: string | null
  status: string
  date?: string | null
  reference?: string | null
  method?: string | null
  entitlement_ok: boolean
  membership_status?: string | null
  certuvo_status?: string | null
  reconciled: boolean
  exception?: string | null
}
interface ReconResp { rows: ReconRow[]; exceptions: number }

const EXCEPTION_LABEL: Record<string, string> = {
  entitlement_missing: 'Entitlement missing',
  membership_not_active: 'Membership not active',
  certuvo_not_provisioned: 'Certuvo not provisioned',
  no_linked_student: 'No linked student',
}
const REVERSIBLE_PROVIDERS = ['admin_manual', 'admin_waiver', 'admin_test_user']

function ReconciliationTab() {
  const [data, setData] = useState<ReconResp | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  // requires the 'finance' permission — a 403 renders a friendly note instead of the table
  const [forbidden, setForbidden] = useState(false)
  const [tick, setTick] = useState(0)
  const [busy, setBusy] = useState<number | null>(null)
  const [note, setNote] = useState<{ ok: boolean; text: string } | null>(null)
  const [reversing, setReversing] = useState<number | null>(null)
  const [revReason, setRevReason] = useState('')
  const dataRef = useRef<ReconResp | null>(null)

  // Loaded by hand (not useAdminQuery) so a 403 — no 'finance' permission — can be told apart
  // from a real failure. A failed background refresh keeps the good data, matching the hooks.
  useEffect(() => {
    let cancelled = false
    adminApi
      .get<ReconResp>('/api/admin/payments/reconciliation')
      .then((d) => { if (!cancelled) { setData(d); dataRef.current = d; setError(null) } })
      .catch((e) => {
        if (cancelled) return
        if (e instanceof UnauthorizedError) return
        if (e instanceof ApiError && e.status === 403) setForbidden(true)
        else if (dataRef.current == null) setError(e instanceof Error ? e.message : 'Something went wrong.')
      })
      .finally(() => { if (!cancelled) setLoading(false) })
    return () => { cancelled = true }
  }, [tick])
  const refetch = () => setTick((t) => t + 1)

  // Flash the outcome (what was ensured / reversed) briefly, then clear.
  useEffect(() => {
    if (!note) return
    const t = setTimeout(() => setNote(null), 8000)
    return () => clearTimeout(t)
  }, [note])

  async function reprocess(id: number) {
    setBusy(id); setNote(null)
    try {
      const r = await adminApi.post<{ ok: boolean; error?: string; ensured?: string[]; already_complete?: boolean }>(`/api/admin/payments/${id}/reprocess`, {})
      if (!r.ok) setNote({ ok: false, text: `Payment #${id}: ${(r.error ?? 'could not reprocess').replace(/_/g, ' ')}.` })
      else setNote({ ok: true, text: r.ensured && r.ensured.length > 0 ? `Payment #${id} reprocessed — ensured: ${r.ensured.map((s) => s.replace(/_/g, ' ')).join(', ')}.` : `Payment #${id} was already complete — nothing to re-apply.` })
      refetch()
    } catch (e) { setNote({ ok: false, text: (e as Error).message }) } finally { setBusy(null) }
  }

  async function reverse(id: number) {
    if (!revReason.trim()) { setNote({ ok: false, text: 'A reversal must record why.' }); return }
    setBusy(id); setNote(null)
    try {
      const r = await adminApi.post<{ ok: boolean; error?: string; detail?: string }>(`/api/admin/payments/${id}/reverse`, { reason: revReason.trim() })
      if (!r.ok) setNote({ ok: false, text: `Payment #${id}: ${r.detail ?? (r.error ?? 'not reversible').replace(/_/g, ' ')}` })
      else setNote({ ok: true, text: `Payment #${id} reversed.` })
      setReversing(null); setRevReason('')
      refetch()
    } catch (e) { setNote({ ok: false, text: (e as Error).message }) } finally { setBusy(null) }
  }

  if (forbidden) return <Card><Empty>Requires the finance permission.</Empty></Card>
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null

  return (
    <Card title="Reconciliation">
      <p className="muted small" style={{ marginTop: 0 }}>Every payment with its downstream state — entitlement, membership and Certuvo — so “gateway says paid but the student has nothing” is visible on one screen. Reprocess is idempotent and never double-grants.</p>
      <div
        className="notice"
        role="status"
        style={{ marginBottom: '.6rem', borderLeftColor: data.exceptions > 0 ? 'var(--warn,#b45309)' : 'var(--ok,#15803d)', ...(data.exceptions === 0 ? { background: 'var(--ok-bg,#ecfdf3)', borderColor: '#a7f3d0', color: 'var(--ok,#15803d)' } : {}) }}
      >
        <strong>{data.exceptions} exception{data.exceptions === 1 ? '' : 's'} need{data.exceptions === 1 ? 's' : ''} attention</strong>
      </div>
      {note && <div className={'notice' + (note.ok ? '' : ' err')} role="status" style={{ marginBottom: '.6rem' }}>{note.text}</div>}
      {data.rows.length === 0 ? (
        <Empty>No payments recorded yet.</Empty>
      ) : (
        <div style={{ overflowX: 'auto' }}>
          <table className="data">
            <thead>
              <tr><th>Student</th><th>Product</th><th>Amount</th><th>Provider</th><th>Gateway ref</th><th>Status</th><th>Downstream</th><th>Exception</th><th /></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => {
                const chips: { label: string; ok: boolean }[] = []
                if (r.product === 'exam' || r.product === 'bundle') chips.push({ label: 'entitlement', ok: r.entitlement_ok })
                if (r.product === 'membership' || r.product === 'bundle') chips.push({ label: 'membership', ok: r.membership_status === 'active' })
                if (r.certuvo_status) chips.push({ label: 'certuvo', ok: r.certuvo_status === 'active' })
                const reversible = REVERSIBLE_PROVIDERS.includes(r.provider ?? '') && (r.status === 'paid' || r.status === 'waived')
                return (
                  <tr key={r.id}>
                    <td className="small">{r.email || '—'}{r.is_test ? <> <Badge tone="warn">TEST</Badge></> : null}</td>
                    <td className="small">{titleCase(r.product)}</td>
                    <td>
                      {r.status === 'waived'
                        ? <><Badge tone="warn">waived</Badge>{r.waived_amount != null && <div className="muted small">{fmtMoney(r.waived_amount)}</div>}</>
                        : fmtMoney(r.amount)}
                    </td>
                    <td className="small">{(r.provider ?? '—').replace(/_/g, ' ')}</td>
                    <td className="small muted" style={{ maxWidth: 140, wordBreak: 'break-all' }}>{r.gateway_id || '—'}</td>
                    <td><StatusBadge status={r.status} /></td>
                    <td>
                      <div className="row" style={{ gap: '.25rem', flexWrap: 'wrap' }}>
                        {chips.length === 0 ? <span className="muted small">—</span> : chips.map((c) => (
                          <Badge key={c.label} tone={c.ok ? 'ok' : 'err'}>{c.ok ? '✓' : '✗'} {c.label}</Badge>
                        ))}
                      </div>
                    </td>
                    <td className="small" style={{ color: r.exception ? 'var(--err,#c2410c)' : undefined, fontWeight: r.exception ? 600 : undefined }}>
                      {r.exception ? (EXCEPTION_LABEL[r.exception] ?? titleCase(r.exception)) : ''}
                    </td>
                    <td>
                      {reversing === r.id ? (
                        <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                          <input placeholder="Reason" value={revReason} onChange={(e) => setRevReason(e.target.value)} style={{ maxWidth: 150 }} />
                          <button className="btn sm danger" disabled={busy === r.id} onClick={() => reverse(r.id)}>Reverse</button>
                          <button className="btn sm secondary" onClick={() => { setReversing(null); setRevReason('') }}>Cancel</button>
                        </div>
                      ) : (
                        <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                          <button className="btn ghost sm" disabled={busy === r.id} onClick={() => reprocess(r.id)}>Reprocess</button>
                          {reversible && <button className="btn ghost sm danger" disabled={busy === r.id} onClick={() => { setReversing(r.id); setRevReason('') }}>Reverse</button>}
                        </div>
                      )}
                    </td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        </div>
      )}
    </Card>
  )
}
