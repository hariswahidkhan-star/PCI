import { useState, useMemo } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type SiteContentRow } from '../api'
import { Card, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { titleCase } from '../../format'

function Row({ row }: { row: SiteContentRow }) {
  const [val, setVal] = useState(row.cvalue ?? '')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<string | null>(null)
  const dirty = val !== (row.cvalue ?? '')
  const long = (row.cvalue ?? '').length > 80 || row.ctype === 'textarea' || row.ctype === 'html'

  async function save() {
    setBusy(true)
    setMsg(null)
    try {
      await adminApi.patch(`/api/admin/content/${row.id}`, { cvalue: val })
      row.cvalue = val
      setMsg('Saved')
    } catch (e) {
      setMsg(e instanceof Error ? e.message : 'Error')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div>
      <label>{row.label || titleCase(row.ckey)}</label>
      {long ? (
        <textarea rows={3} value={val} onChange={(e) => setVal(e.target.value)} />
      ) : (
        <input value={val} onChange={(e) => setVal(e.target.value)} />
      )}
      <div className="row" style={{ marginTop: '.4rem' }}>
        <button className="btn sm" disabled={busy || !dirty} onClick={save}>{busy ? 'Saving…' : 'Save'}</button>
        {msg && <span className="small muted">{msg}</span>}
      </div>
    </div>
  )
}

export default function Content() {
  const { data, loading, error } = useAdminQuery<{ rows: SiteContentRow[] }>('/api/admin/content')
  const [q, setQ] = useState('')

  const groups = useMemo(() => {
    const rows = (data?.rows ?? []).filter((r) => {
      const needle = q.trim().toLowerCase()
      return !needle || r.ckey.toLowerCase().includes(needle) || (r.label ?? '').toLowerCase().includes(needle) || (r.cvalue ?? '').toLowerCase().includes(needle)
    })
    const m = new Map<string, SiteContentRow[]>()
    for (const r of rows) {
      const g = r.cgroup || 'General'
      if (!m.has(g)) m.set(g, [])
      m.get(g)!.push(r)
    }
    return [...m.entries()]
  }, [data, q])

  return (
    <div className="page">
      <PageHeader
        title="Site content"
        subtitle="Shared text used across the website (headers, footers, calls-to-action). Changes go live immediately."
      />

      <Card>
        <input aria-label="Search content" placeholder="Search content…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 320 }} />
      </Card>

      {loading ? (
        <Spinner />
      ) : error ? (
        <ErrorNote>{error}</ErrorNote>
      ) : groups.length === 0 ? (
        <Card><Empty>No content entries match.</Empty></Card>
      ) : (
        groups.map(([group, rows]) => (
          <Card key={group} title={titleCase(group)}>
            <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
              {rows.map((r) => <Row key={r.id} row={r} />)}
            </div>
          </Card>
        ))
      )}
    </div>
  )
}
