import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type ReportData } from '../api'
import { Card, Stat, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtMoney, titleCase } from '../../format'

const EXPORTS = ['members', 'payments', 'enrollments', 'credentials', 'inquiries', 'redemptions', 'subscribers']

export default function Reports() {
  // default range: last 30 days (computed on the client, sent to the server)
  const today = new Date().toISOString().slice(0, 10)
  const monthAgo = new Date(Date.now() - 29 * 86_400_000).toISOString().slice(0, 10)
  const [from, setFrom] = useState(monthAgo)
  const [to, setTo] = useState(today)
  const [applied, setApplied] = useState({ from: monthAgo, to: today })
  const [downloading, setDownloading] = useState<string | null>(null)

  const { data, loading, error } = useAdminQuery<ReportData>(`/api/admin/reports?from=${applied.from}&to=${applied.to}`)

  async function download(entity: string) {
    setDownloading(entity)
    try {
      // CSV endpoint returns text/csv; the client returns it as a raw string. Trigger a browser download
      // with the bearer token attached (a plain <a href> could not send the Authorization header).
      const csv = await adminApi.get<string>(`/api/admin/export?entity=${entity}`)
      const blob = new Blob([typeof csv === 'string' ? csv : ''], { type: 'text/csv' })
      const url = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = `${entity}.csv`
      document.body.appendChild(a)
      a.click()
      a.remove()
      URL.revokeObjectURL(url)
    } catch (e) {
      alert(e instanceof Error ? e.message : 'Export failed.')
    } finally {
      setDownloading(null)
    }
  }

  return (
    <div className="page">
      <PageHeader title="Reports" />

      <Card title="Date range">
        <div className="row" style={{ flexWrap: 'wrap', alignItems: 'flex-end' }}>
          <div className="field" style={{ margin: 0 }}><label>From</label><input type="date" value={from} onChange={(e) => setFrom(e.target.value)} /></div>
          <div className="field" style={{ margin: 0 }}><label>To</label><input type="date" value={to} onChange={(e) => setTo(e.target.value)} /></div>
          <button className="btn sm" onClick={() => setApplied({ from, to })}>Apply</button>
        </div>
      </Card>

      {loading ? (
        <Spinner />
      ) : error ? (
        <ErrorNote>{error}</ErrorNote>
      ) : !data ? null : (
        <>
          <div className="grid cols-3">
            <Card><Stat n={fmtMoney(data.totals.revenue)} k="Revenue" /></Card>
            <Card><Stat n={data.totals.payments} k="Payments" /></Card>
            <Card><Stat n={fmtMoney(data.totals.avg_order)} k="Avg order" /></Card>
            <Card><Stat n={data.new_members} k="New students" /></Card>
            <Card><Stat n={data.funnel.started} k="Enrolments started" /></Card>
            <Card><Stat n={data.funnel.paid} k="Enrolments paid" /></Card>
          </div>

          <div className="grid cols-2">
            <Card title="Revenue by product">
              {data.by_product.length === 0 ? <Empty>No paid transactions in range.</Empty> : (
                <table className="data">
                  <thead><tr><th>Product</th><th>Count</th><th style={{ textAlign: 'right' }}>Revenue</th></tr></thead>
                  <tbody>
                    {data.by_product.map((p) => (
                      <tr key={p.product_type}><td>{titleCase(p.product_type)}</td><td>{p.n}</td><td style={{ textAlign: 'right' }}>{fmtMoney(p.revenue)}</td></tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Card>
            <Card title="Revenue by country">
              {data.by_country.length === 0 ? <Empty>No data in range.</Empty> : (
                <table className="data">
                  <thead><tr><th>Country</th><th>Count</th><th style={{ textAlign: 'right' }}>Revenue</th></tr></thead>
                  <tbody>
                    {data.by_country.map((c) => (
                      <tr key={c.country}><td>{c.country}</td><td>{c.n}</td><td style={{ textAlign: 'right' }}>{fmtMoney(c.revenue)}</td></tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Card>
          </div>

          <div className="grid cols-2">
            <Card title="Revenue by certification">
              {(!data.by_certification || data.by_certification.length === 0) ? <Empty>No paid transactions in range.</Empty> : (
                <table className="data">
                  <thead><tr><th>Certification</th><th>Count</th><th style={{ textAlign: 'right' }}>Revenue</th></tr></thead>
                  <tbody>
                    {data.by_certification.map((c) => (
                      <tr key={c.certification}><td>{c.certification}</td><td>{c.n}</td><td style={{ textAlign: 'right' }}>{fmtMoney(c.revenue)}</td></tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Card>
            <Card title="Certificates issued by certification">
              {(!data.certificates_by_certification || data.certificates_by_certification.length === 0) ? <Empty>No certificates issued in range.</Empty> : (
                <table className="data">
                  <thead><tr><th>Certification</th><th style={{ textAlign: 'right' }}>Issued</th></tr></thead>
                  <tbody>
                    {data.certificates_by_certification.map((c) => (
                      <tr key={c.certification}><td>{c.certification}</td><td style={{ textAlign: 'right' }}>{c.issued}</td></tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Card>
          </div>
        </>
      )}

      <Card title="Export data (CSV)">
        <p className="muted small" style={{ marginTop: 0 }}>Download a spreadsheet of any dataset.</p>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          {EXPORTS.map((e) => (
            <button key={e} className="btn secondary sm" disabled={downloading === e} onClick={() => download(e)}>
              {downloading === e ? 'Preparing…' : titleCase(e)}
            </button>
          ))}
        </div>
      </Card>
    </div>
  )
}
