import { useQuery } from '../api/hooks'
import { Card, Spinner, ErrorNote, Empty, Badge } from '../components/ui'

interface ResourceRow {
  title: string
  category?: string | null
  doc_type?: string | null
  url?: string | null
}

/** Study & policy downloads — the same admin-managed library the public site shows,
 * available inside the portal (GET /api/me/downloads). */
export default function Resources() {
  const { data, loading, error } = useQuery<{ rows: ResourceRow[] }>('/api/me/downloads')

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []

  const groups = new Map<string, ResourceRow[]>()
  for (const r of rows) {
    const g = (r.category || 'General').trim() || 'General'
    if (!groups.has(g)) groups.set(g, [])
    groups.get(g)!.push(r)
  }

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Resources</h1>
        <p className="muted">
          Handbooks, policies and study documents — everything the institute publishes, in one place.
        </p>
      </div>

      {rows.length === 0 ? (
        <Card><Empty>No resources have been published yet — new documents will appear here automatically.</Empty></Card>
      ) : (
        [...groups.entries()].map(([category, items]) => (
          <Card title={category} key={category}>
            <div className="res-grid">
              {items.map((r, i) => (
                <div className="rep-item" key={i} style={{ alignItems: 'center' }}>
                  <div className="rep-item-main row" style={{ gap: '.8rem' }}>
                    <span className="res-ic">{(r.doc_type || 'PDF').slice(0, 4).toUpperCase()}</span>
                    <div>
                      <strong>{r.title}</strong>
                      {r.doc_type && <div className="muted small">{r.doc_type}</div>}
                    </div>
                  </div>
                  <div className="rep-item-actions">
                    {r.url ? (
                      <a className="btn sm" href={r.url} target="_blank" rel="noreferrer" download>
                        Download
                      </a>
                    ) : (
                      <Badge>Coming soon</Badge>
                    )}
                  </div>
                </div>
              ))}
            </div>
          </Card>
        ))
      )}
    </div>
  )
}
