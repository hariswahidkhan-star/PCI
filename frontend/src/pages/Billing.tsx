import { useMe } from '../data/MeContext'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate, fmtMoney, titleCase } from '../format'
import { openPrintable, escapeHtml as e } from '../print'
import type { Payment } from '../api/types'

export default function Billing() {
  const { me, loading, error } = useMe()
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  const receipt = (p: Payment) => {
    const name = `${me.user.first_name ?? ''} ${me.user.last_name ?? ''}`.trim() || me.user.email
    openPrintable(`Receipt ${p.reference ?? p.id}`, `
      <div class="brand">PROJECT CONTROLS INSTITUTE</div>
      <h1>Payment receipt</h1>
      <p class="muted">Reference ${e(p.reference ?? p.id)}</p>
      <table>
        <tr><td>Billed to</td><td class="r">${e(name)} &lt;${e(me.user.email)}&gt;</td></tr>
        <tr><td>Item</td><td class="r">${e(titleCase(p.product_type))}</td></tr>
        <tr><td>Date</td><td class="r">${e(fmtDate(p.payment_date))}</td></tr>
        <tr><td>Status</td><td class="r">${e(p.payment_status)}</td></tr>
        <tr><td><strong>Amount paid</strong></td><td class="r"><strong>${e(fmtMoney(p.final_amount, p.currency))}</strong></td></tr>
      </table>`)
  }

  const paid = me.payments.filter((p) => p.payment_status === 'paid')
  const total = paid.reduce((s, p) => s + (Number(p.final_amount) || 0), 0)
  const currency = paid[0]?.currency || 'USD'

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Billing</h1>
        <p className="muted">Your payments and receipts.</p>
      </div>

      <Card title="Payment history" action={<span className="muted small">Total paid: <strong>{fmtMoney(total, currency)}</strong></span>}>
        {me.payments.length === 0 ? (
          <Empty>No payments on record.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Date</th><th>Item</th><th>Reference</th><th>Amount</th><th>Status</th><th></th></tr>
            </thead>
            <tbody>
              {me.payments.map((p) => (
                <tr key={p.id}>
                  <td>{fmtDate(p.payment_date)}</td>
                  <td>{titleCase(p.product_type)}</td>
                  <td className="small muted">{p.reference || '—'}</td>
                  <td>{fmtMoney(p.final_amount, p.currency)}</td>
                  <td><StatusBadge status={p.payment_status} /></td>
                  <td>
                    {p.payment_status === 'paid' && (
                      <button className="btn ghost sm" onClick={() => receipt(p)}>Receipt</button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
