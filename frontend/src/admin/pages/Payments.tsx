import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import type { PaymentRow } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate, fmtMoney, titleCase } from '../../format'

interface PaymentsResp {
  rows: PaymentRow[]
  totals: { paid: number; n: number; refunded: number }
}

export default function Payments() {
  const [status, setStatus] = useState('')
  const [product, setProduct] = useState('')
  const [q, setQ] = useState('')
  const params = new URLSearchParams()
  if (status) params.set('status', status)
  if (product) params.set('product', product)
  if (q) params.set('q', q)
  const qs = params.toString()
  const { data, loading, error } = useAdminQuery<PaymentsResp>(`/api/admin/payments${qs ? '?' + qs : ''}`)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <h1>Payments</h1>

      {data && (
        <div className="grid cols-3">
          <Card><div className="stat"><span className="n">{fmtMoney(data.totals.paid)}</span><span className="k">Paid (filtered)</span></div></Card>
          <Card><div className="stat"><span className="n">{fmtMoney(data.totals.refunded)}</span><span className="k">Refunded</span></div></Card>
          <Card><div className="stat"><span className="n">{data.totals.n}</span><span className="k">Transactions</span></div></Card>
        </div>
      )}

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search ref or email…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 280 }} />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 180 }}>
            <option value="">All statuses</option>
            <option value="paid">Paid</option>
            <option value="refunded">Refunded</option>
            <option value="failed">Failed</option>
          </select>
          <select value={product} onChange={(e) => setProduct(e.target.value)} style={{ maxWidth: 180 }}>
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
        )}
      </Card>
    </div>
  )
}
