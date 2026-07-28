import { useState } from 'react'
import { useQuery } from '../api/hooks'
import { useT } from '../i18n'
import { PageHeader } from '../components/premium'
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
  downloaded?: boolean
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
  const t = useT()
  const { data, loading, error } = useQuery<Resp>('/api/me/templates')
  const [cat, setCat] = useState<string>('all')
  const [track, setTrack] = useState<number | 'all'>('all')
  const [show, setShow] = useState<'all' | 'new'>('all')
  const [q, setQ] = useState('')
  // Slugs downloaded this session — merged with the server's `downloaded` flag so a freshly grabbed template
  // shows its badge immediately, without a refetch.
  const [got, setGot] = useState<Set<string>>(new Set())
  const [busy, setBusy] = useState<string | null>(null)
  const [msg, setMsg] = useState('')

  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  const rows = data?.rows ?? []
  const isDownloaded = (r: Row) => got.has(r.slug) || !!r.downloaded
  // Only offer chips for topics/tracks that actually have templates.
  const cats = [...new Set(rows.map((r) => r.category || 'general'))]
  const tracks = [...new Set(rows.map((r) => r.certification_id).filter((x): x is number => x != null))].sort()
  const anyDownloaded = rows.some((r) => isDownloaded(r))

  // A free-text search over the title, summary and slug — trimmed and case-insensitive.
  const needle = q.trim().toLowerCase()
  // A NULL-track template applies to every track, so it stays visible under any track filter.
  const visible = rows.filter(
    (r) => (cat === 'all' || (r.category || 'general') === cat)
      && (track === 'all' || r.certification_id == null || r.certification_id === track)
      && (show === 'all' || !isDownloaded(r))
      && (needle === '' || `${r.title} ${r.summary ?? ''} ${r.slug}`.toLowerCase().includes(needle)),
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
      if (!res.ok) { setMsg(t('tpl.downloadError')); return }
      const blob = await res.blob()
      const href = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = href
      a.download = `${r.slug}.${r.format || 'csv'}`
      document.body.appendChild(a); a.click(); a.remove()
      setTimeout(() => URL.revokeObjectURL(href), 60_000)
      // Mark it downloaded so the badge appears immediately (the server has recorded it in the student's history).
      setGot((prev) => new Set(prev).add(r.slug))
    } catch { setMsg(t('tpl.downloadError')) }
    finally { setBusy(null) }
  }

  return (
    <div className="page fade-stagger">
      <PageHeader eyebrow={t('nav.templates')} title={t('tpl.title')} subtitle={t('tpl.subtitle')} />

      {msg && <div className="notice err" role="alert">{msg}</div>}

      {rows.length === 0 ? (
        <Card><Empty>{t('tpl.emptyAll')}</Empty></Card>
      ) : (
        <>
          <input
            type="search"
            value={q}
            onChange={(e) => setQ(e.target.value)}
            placeholder={t('tpl.searchPlaceholder')}
            aria-label={t('tpl.searchLabel')}
            style={{ maxWidth: 360 }}
          />
          <div className="row" style={{ gap: '.5rem', flexWrap: 'wrap', alignItems: 'center' }}>
            <span className="small muted" style={{ fontWeight: 700 }}>{t('tpl.topic')}</span>
            <button className={'btn sm' + (cat === 'all' ? '' : ' secondary')} onClick={() => setCat('all')}>{t('tpl.all')}</button>
            {cats.map((c) => (
              <button key={c} className={'btn sm' + (cat === c ? '' : ' secondary')} onClick={() => setCat(c)}>{catLabel(c)}</button>
            ))}
            {tracks.length > 0 && (
              <>
                <span className="small muted" style={{ fontWeight: 700, marginLeft: '.5rem' }}>{t('tpl.track')}</span>
                <button className={'btn sm' + (track === 'all' ? '' : ' secondary')} onClick={() => setTrack('all')}>{t('tpl.all')}</button>
                {tracks.map((tk) => (
                  <button key={tk} className={'btn sm' + (track === tk ? '' : ' secondary')} onClick={() => setTrack(tk)}>{CERTS[tk] || `Track ${tk}`}</button>
                ))}
              </>
            )}
            {anyDownloaded && (
              <>
                <span className="small muted" style={{ fontWeight: 700, marginLeft: '.5rem' }}>{t('tpl.show')}</span>
                <button className={'btn sm' + (show === 'all' ? '' : ' secondary')} onClick={() => setShow('all')}>{t('tpl.all')}</button>
                <button className={'btn sm' + (show === 'new' ? '' : ' secondary')} onClick={() => setShow('new')}>{t('tpl.new')}</button>
              </>
            )}
          </div>

          {visible.length === 0 ? (
            <Card><Empty>{t('tpl.emptyFilter')}</Empty></Card>
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
                        {isDownloaded(r) && <Badge tone="ok">{t('tpl.downloaded')}</Badge>}
                        <button className="btn sm" disabled={busy === r.slug} onClick={() => download(r)}>
                          {busy === r.slug ? t('tpl.downloading') : isDownloaded(r) ? t('tpl.downloadAgain') : t('tpl.download')}
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
