import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { Card, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { adminApi } from '../api'

// Admin Console → Analytics: first-party, privacy-first reporting (cookieless server-side events).
// Visitors are daily-rotating hashes — no raw IPs here; country appears only when a CDN geo header
// exists. Conversions carry first-touch attribution (source/medium/campaign) from the pci_attr cookie.

interface Row { [k: string]: string | number | null }
interface Summary {
  page_views: number; visitors: number; registrations: number; logins: number
  purchases: number; revenue: number; memberships_activated: number
  top_pages: Row[]; sources: Row[]; campaigns: Row[]; devices: Row[]
  browsers: Row[]; countries: Row[]; daily: Row[]; conversions_by_source: Row[]
}

export default function Analytics() {
  const [days, setDays] = useState(30)
  const { data, loading, error } = useAdminQuery<Summary>(`/api/admin/analytics/summary?days=${days}`)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null

  const stat = (n: number | string, k: string) => (
    <Card><div><div style={{ fontSize: '1.5rem', fontWeight: 700 }}>{n}</div><div className="muted small">{k}</div></div></Card>
  )
  const table = (title: string, rows: Row[], cols: [string, string][]) => (
    <Card title={title}>
      {rows.length === 0 ? <Empty>No data yet.</Empty> : (
        <table className="data">
          <thead><tr>{cols.map(([k, l]) => <th key={k}>{l}</th>)}</tr></thead>
          <tbody>{rows.map((r, i) => <tr key={i}>{cols.map(([k]) => <td key={k} className="small">{String(r[k] ?? '—')}</td>)}</tr>)}</tbody>
        </table>
      )}
    </Card>
  )

  return (
    <div className="page">
      <div className="spread" style={{ flexWrap: 'wrap', gap: '.6rem' }}>
        <PageHeader
          title="Analytics"
          subtitle="First-party, privacy-first: server-side events, no cookies, no raw IP addresses. Conversions carry first-touch campaign attribution."
        />
        <div className="row" style={{ gap: '.5rem' }}>
          <select value={days} onChange={(e) => setDays(Number(e.target.value))}>
            <option value={7}>Last 7 days</option><option value={30}>Last 30 days</option>
            <option value={90}>Last 90 days</option><option value={365}>Last year</option>
          </select>
          <a className="btn ghost sm" href={`/api/admin/analytics/export.csv?days=${days}`} onClick={(e) => { e.preventDefault(); exportCsv(days) }}>Export CSV</a>
        </div>
      </div>

      <div style={{ display: 'grid', gap: '.8rem', gridTemplateColumns: 'repeat(auto-fit,minmax(140px,1fr))' }}>
        {stat(data.visitors, 'Visitors')}
        {stat(data.page_views, 'Page views')}
        {stat(data.registrations, 'Registrations')}
        {stat(data.logins, 'Logins')}
        {stat(data.purchases, 'Purchases')}
        {stat('$' + data.revenue.toLocaleString(), 'Revenue')}
        {stat(data.memberships_activated, 'Memberships')}
      </div>

      <div className="grid cols-2" style={{ display: 'grid', gap: '.9rem', gridTemplateColumns: 'repeat(auto-fit,minmax(320px,1fr))' }}>
        {table('Top pages', data.top_pages, [['path', 'Page'], ['n', 'Views']])}
        {table('Traffic sources', data.sources, [['source', 'Source'], ['n', 'Views']])}
        {table('Campaigns', data.campaigns, [['campaign', 'Campaign'], ['n', 'Views']])}
        {table('Conversions by source', data.conversions_by_source, [['source', 'Source'], ['registrations', 'Registrations'], ['purchases', 'Purchases'], ['revenue', 'Revenue']])}
        {table('Devices', data.devices, [['device', 'Device'], ['n', 'Views']])}
        {table('Browsers', data.browsers, [['browser', 'Browser'], ['n', 'Views']])}
        {table('Countries', data.countries, [['country', 'Country'], ['n', 'Views']])}
        {table('Daily page views', data.daily, [['day', 'Day'], ['n', 'Views']])}
      </div>
      <p className="muted small">Country shows “(unknown)” until the site runs behind a CDN that supplies a geo header (e.g. Cloudflare). Fine-grained client events (CTA clicks, scroll) are GA4’s role once a measurement ID is set under SEO → Search engines — loaded only after visitor consent.</p>
    </div>
  )
}

async function exportCsv(days: number) {
  // fetch with the bearer token, then download — a plain <a href> would miss the Authorization header
  const tok = adminApi.getToken()
  if (!tok) throw new Error('Your admin session has expired.')
  const resp = await fetch(`/api/admin/analytics/export.csv?days=${days}`, { headers: { Authorization: `Bearer ${tok}` } })
  if (!resp.ok) throw new Error(`Analytics export failed (${resp.status}).`)
  const blob = await resp.blob()
  const a = document.createElement('a')
  a.href = URL.createObjectURL(blob)
  a.download = `pci-analytics-${days}d.csv`
  document.body.appendChild(a)
  a.click()
  a.remove()
  URL.revokeObjectURL(a.href)
}
