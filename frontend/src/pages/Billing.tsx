import { useMe } from '../data/MeContext'
import { Card, StatusBadge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate, fmtMoney, titleCase } from '../format'

export default function Billing() {
  const { me, loading, error } = useMe()
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

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
                      <a className="btn ghost sm" href={`/api/me/invoices?payment_id=${p.id}`} target="_blank" rel="noreferrer">Receipt</a>
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
