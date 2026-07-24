import { useState } from 'react'
import { useQuery } from '../api/hooks'
import { Card, Spinner, ErrorNote, Empty, Badge } from '../components/ui'

// Student portal → Templates. The members-only project-controls templates library (WBS, EVM tracker, risk
// register, cash-flow, RACI, …). Content is admin-managed and served only to logged-in students at
// /api/me/templates; each file downloads through the authenticated endpoint (a plain <a href> would 401).

interface Row {
  slug: string
  title: string
  category: string
  certification_id: number | null
  summary?: string | null
  format: string
  download_url: string
}
interface Resp { rows: Row[]; total: number; categories: string[] }

const CAT_LABELS: Record<string, string> = {
  scope: 'Scope & WBS', schedule: 'Schedule', cost: 'Cost control', evm: 'Earned value', risk: 'Risk',
  change: 'Change control', cashflow: 'Cash flow', finance: 'Project finance', delivery: 'Delivery & governance',
  quality: 'Quality & lessons', general: 'General',
}
const CERTS: Record<number, string> = { 1: 'PCL-AI', 2: 'PFL-AI', 3: 'PML-AI' }
const catLabel = (c: string) => CAT_LABELS[c] || c.charAt(0).toUpperCase() + c.slice(1)

export default function Templates() {
  const { data, loading, error } = useQuery<Resp>('/api/me/templates')
  const [cat, setCat] = useState<string>('all')
  const [track, setTrack] = useState<number | 'all'>('all')
  const [busy, setBusy] = useState<string | null>(null)
  const [msg, setMsg] = useState('')

  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  const rows = data?.rows ?? []
  // Only offer chips for topics/tracks that actually have templates.
  const cats = [...new Set(rows.map((r) => r.category || 'general'))]
  const tracks = [...new Set(rows.map((r) => r.certification_id).filter((x): x is number => x != null))].sort()

  // A NULL-track template applies to every track, so it stays visible under any track filter.
  const visible = rows.filter(
    (r) => (cat === 'all' || (r.category || 'general') === cat)
      && (track === 'all' || r.certification_id == null || r.certification_id === track),
  )
  const groups = new Map<string, Row[]>()
  for (const r of visible) {
    const g = r.category || 'general'
    if (!groups.has(g)) groups.set(g, [])
    groups.get(g)!.push(r)
  }

  async function download(r: Row) {
    setBusy(r.slug); setMsg('')
    const tok = sessionStorage.getItem('pci.session.token')
    try {
      const res = await fetch(r.download_url, { headers: tok ? { Authorization: 'Bearer ' + tok } : {} })
      if (!res.ok) { setMsg('Could not download that template right now. Please try again shortly.'); return }
      const blob = await res.blob()
      const href = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = href
      a.download = `${r.slug}.${r.format || 'csv'}`
      document.body.appendChild(a); a.click(); a.remove()
      setTimeout(() => URL.revokeObjectURL(href), 60_000)
    } catch { setMsg('Could not download that template right now. Please try again shortly.') }
    finally { setBusy(null) }
  }

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Templates</h1>
        <p className="muted">
          Ready-to-use project-controls templates — download, adapt and reuse them on your own projects. Content is
          synthetic and freely reusable.
        </p>
      </div>

      {msg && <div className="notice err" role="alert">{msg}</div>}

      {rows.length === 0 ? (
        <Card><Empty>No templates are available yet — check back soon.</Empty></Card>
      ) : (
        <>
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'center' }}>
            <span className="small muted" style={{ fontWeight: 700 }}>Topic</span>
            <button className={'btn sm' + (cat === 'all' ? '' : ' secondary')} onClick={() => setCat('all')}>All</button>
            {cats.map((c) => (
              <button key={c} className={'btn sm' + (cat === c ? '' : ' secondary')} onClick={() => setCat(c)}>{catLabel(c)}</button>
            ))}
            {tracks.length > 0 && (
              <>
                <span className="small muted" style={{ fontWeight: 700, marginLeft: '.5rem' }}>Track</span>
                <button className={'btn sm' + (track === 'all' ? '' : ' secondary')} onClick={() => setTrack('all')}>All</button>
                {tracks.map((tk) => (
                  <button key={tk} className={'btn sm' + (track === tk ? '' : ' secondary')} onClick={() => setTrack(tk)}>{CERTS[tk] || `Track ${tk}`}</button>
                ))}
              </>
            )}
          </div>

          {visible.length === 0 ? (
            <Card><Empty>No templates match that filter.</Empty></Card>
          ) : (
            [...groups.entries()].map(([category, items]) => (
              <Card title={catLabel(category)} key={category}>
                <div className="res-grid">
                  {items.map((r) => (
                    <div className="rep-item" key={r.slug} style={{ alignItems: 'center' }}>
                      <div className="rep-item-main row" style={{ gap: '.8rem' }}>
                        <span className="res-ic">{(r.format || 'CSV').slice(0, 4).toUpperCase()}</span>
                        <div>
                          <strong>{r.title}</strong>
                          {r.summary && <div className="muted small">{r.summary}</div>}
                        </div>
                      </div>
                      <div className="rep-item-actions row" style={{ gap: '.4rem', alignItems: 'center' }}>
                        {r.certification_id != null && <Badge>{CERTS[r.certification_id] || `Track ${r.certification_id}`}</Badge>}
                        <button className="btn sm" disabled={busy === r.slug} onClick={() => download(r)}>
                          {busy === r.slug ? 'Downloading…' : 'Download'}
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              </Card>
            ))
          )}
        </>
      )}
    </div>
  )
}
