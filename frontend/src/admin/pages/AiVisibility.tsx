import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'

// Admin Console → AI Visibility (Phase 6 — Generative Engine Optimization). Three tabs:
// Overview (readiness: llms.txt / robots.txt / sitemap, structured-data coverage, crawler totals),
// AI crawlers (observed answer-engine traffic), Access (which AI crawlers may fetch the site — the
// robots.txt policy). Content and page metadata stay in Pages & content / SEO.

interface Overview {
  canonical_host: string; llms_url: string; robots_url: string; sitemap_url: string
  structured_data: { pages: number; with_json_ld: number; coverage_pct: number }
  crawlers: { known: number; blocked: number; allowed: number }
  traffic: { hits_30d: number; distinct_bots: number }
}
interface Row { [k: string]: string | number | null }
interface Crawlers { total: number; by_bot: Row[]; by_page: Row[]; daily: Row[] }
interface PolicyRow { token: string; label: string; vendor: string; purpose: string; detectable: boolean; blocked: boolean }
interface Policy { rows: PolicyRow[]; blocked_count: number; robots_preview: string }

const TABS = ['Overview', 'AI crawlers', 'Access'] as const

export default function AiVisibility() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Overview')
  return (
    <div className="page">
      <PageHeader
        title="AI Visibility"
        subtitle={<>How AI answer engines (ChatGPT, Claude, Perplexity, Gemini, Copilot) discover and cite PCI. Publishes an <code>llms.txt</code> map, controls which AI crawlers may access the site, and reports the AI-crawler traffic you actually receive.</>}
      />
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => (
          <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>
        ))}
      </div>
      {tab === 'Overview' && <OverviewTab />}
      {tab === 'AI crawlers' && <CrawlersTab />}
      {tab === 'Access' && <AccessTab />}
    </div>
  )
}

function OverviewTab() {
  const { data, loading, error } = useAdminQuery<Overview>('/api/admin/ai-visibility/overview')
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null
  const sd = data.structured_data
  const stat = (n: number | string, k: string, warn = false) => (
    <Card><div><div style={{ fontSize: '1.6rem', fontWeight: 700, color: warn ? 'var(--err,#b91c1c)' : undefined }}>{n}</div><div className="muted small">{k}</div></div></Card>
  )
  const link = (label: string, url: string) => (
    <Card><div><a href={url} target="_blank" rel="noreferrer" style={{ fontWeight: 700 }}>{label} ↗</a><div className="muted small" style={{ wordBreak: 'break-all' }}>{url}</div></div></Card>
  )
  return (
    <>
      <p className="muted small">Canonical host: <strong>{data.canonical_host}</strong>. These files are generated live from your published, indexable content.</p>
      <div className="grid cols-3" style={{ display: 'grid', gap: '.8rem', gridTemplateColumns: 'repeat(auto-fit,minmax(220px,1fr))' }}>
        {link('llms.txt', data.llms_url)}
        {link('robots.txt', data.robots_url)}
        {link('sitemap.xml', data.sitemap_url)}
      </div>
      <div className="grid cols-3" style={{ display: 'grid', gap: '.8rem', gridTemplateColumns: 'repeat(auto-fit,minmax(150px,1fr))' }}>
        {stat(sd.coverage_pct + '%', 'Structured-data coverage', sd.coverage_pct < 90)}
        {stat(`${sd.with_json_ld}/${sd.pages}`, 'Pages with JSON-LD')}
        {stat(data.crawlers.allowed, 'AI crawlers allowed')}
        {stat(data.crawlers.blocked, 'AI crawlers blocked')}
        {stat(data.traffic.hits_30d, 'AI crawler hits (30d)')}
        {stat(data.traffic.distinct_bots, 'Distinct AI bots seen')}
      </div>
      <p className="muted small">Structured data (Schema.org JSON-LD) is what lets AI engines extract facts — organization, courses, FAQs — directly. Missing coverage shows under SEO → Audit. Answer engines are allowed by default so PCI can be cited; tighten access under the <strong>Access</strong> tab.</p>
    </>
  )
}

function CrawlersTab() {
  const [days, setDays] = useState(30)
  const { data, loading, error } = useAdminQuery<Crawlers>(`/api/admin/ai-visibility/crawlers?days=${days}`)
  const table = (title: string, rows: Row[], cols: [string, string][]) => (
    <Card title={title}>
      {rows.length === 0 ? <Empty>No AI-crawler visits recorded yet.</Empty> : (
        <table className="data">
          <thead><tr>{cols.map(([k, l]) => <th key={k}>{l}</th>)}</tr></thead>
          <tbody>{rows.map((r, i) => <tr key={i}>{cols.map(([k]) => <td key={k} className="small">{String(r[k] ?? '—')}</td>)}</tr>)}</tbody>
        </table>
      )}
    </Card>
  )
  return (
    <div style={{ display: 'grid', gap: '.9rem' }}>
      <div className="row" style={{ gap: '.5rem' }}>
        <select value={days} onChange={(e) => setDays(Number(e.target.value))}>
          <option value={7}>Last 7 days</option><option value={30}>Last 30 days</option>
          <option value={90}>Last 90 days</option><option value={365}>Last year</option>
        </select>
        {data && <span className="muted small">{data.total} AI-crawler request(s) in range</span>}
      </div>
      {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : data && (
        <div className="grid cols-2" style={{ display: 'grid', gap: '.9rem', gridTemplateColumns: 'repeat(auto-fit,minmax(320px,1fr))' }}>
          {table('By AI crawler', data.by_bot, [['bot', 'Crawler'], ['n', 'Hits'], ['last_seen', 'Last seen']])}
          {table('Most-crawled pages', data.by_page, [['path', 'Page'], ['n', 'Hits']])}
          {table('Daily', data.daily, [['day', 'Day'], ['n', 'Hits']])}
        </div>
      )}
      <p className="muted small">Only crawlers that fetch under their own user-agent appear here. Google-Extended and Applebot-Extended control AI use through robots.txt but fetch under the shared Googlebot / Applebot agent, so their access is managed under <strong>Access</strong> rather than measured here.</p>
    </div>
  )
}

function AccessTab() {
  const { data, loading, error, refetch } = useAdminQuery<Policy>('/api/admin/ai-visibility/policy')
  const [blocked, setBlocked] = useState<Set<string> | null>(null)
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState<string | null>(null)
  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null
  const set = blocked ?? new Set(data.rows.filter((r) => r.blocked).map((r) => r.token))
  const toggle = (token: string) => {
    const next = new Set(set)
    next.has(token) ? next.delete(token) : next.add(token)
    setBlocked(next)
  }
  async function save() {
    setBusy(true); setNote(null)
    try {
      await adminApi.post('/api/admin/ai-visibility/policy', { blocked: Array.from(set) })
      setNote('Saved — robots.txt updated immediately.'); setBlocked(null); refetch()
    } catch (e) { setNote((e as Error).message) } finally { setBusy(false) }
  }
  const tone = (p: string) => (p === 'answer' ? 'ok' : p === 'training' ? 'warn' : 'brand') as 'ok' | 'warn' | 'brand'
  return (
    <div style={{ display: 'grid', gap: '.9rem' }}>
      {note && <div className="notice" role="status">{note}</div>}
      <Card title="AI crawler access">
        <p className="muted small" style={{ marginTop: 0 }}>Tick a crawler to <strong>block</strong> it in robots.txt. Answer engines are allowed by default so PCI content can be cited in AI assistants; you may want to block training-only crawlers. Allowed crawlers still cannot reach private/authenticated areas.</p>
        <table className="data">
          <thead><tr><th>Block</th><th>Crawler</th><th>Vendor</th><th>Purpose</th></tr></thead>
          <tbody>
            {data.rows.map((r) => (
              <tr key={r.token}>
                <td><input type="checkbox" checked={set.has(r.token)} onChange={() => toggle(r.token)} style={{ width: 'auto' }} /></td>
                <td className="small"><strong>{r.label}</strong>{!r.detectable && <span className="muted"> · robots-only</span>}</td>
                <td className="small muted">{r.vendor}</td>
                <td><Badge tone={tone(r.purpose)}>{r.purpose}</Badge></td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="row" style={{ gap: '.5rem', marginTop: '.7rem' }}>
          <button className="btn" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save access policy'}</button>
          <span className="muted small">{set.size === 0 ? 'All AI crawlers allowed' : `${set.size} blocked`}</span>
        </div>
      </Card>
      <Card title="robots.txt preview">
        <pre className="small" style={{ margin: 0, whiteSpace: 'pre-wrap', maxHeight: 260, overflowY: 'auto' }}>{data.robots_preview}</pre>
        <p className="muted small" style={{ marginBottom: 0 }}>Save to regenerate. Purpose legend: <Badge tone="ok">answer</Badge> feeds a live AI answer product (citation opportunity), <Badge tone="warn">training</Badge> feeds model training, <Badge tone="brand">both</Badge> does both.</p>
      </Card>
    </div>
  )
}
