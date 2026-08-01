import { useState, useMemo } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type PageRow, type PageBlock } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'

function PageEditor({ page, onClose, onSaved }: { page: PageRow; onClose: () => void; onSaved: () => void }) {
  const [title, setTitle] = useState(page.title ?? '')
  const [meta, setMeta] = useState(page.meta_description ?? '')
  const [published, setPublished] = useState(page.published !== 0)
  const [noindex, setNoindex] = useState(page.noindex === 1)
  const [savingMeta, setSavingMeta] = useState(false)
  const [metaMsg, setMetaMsg] = useState<string | null>(null)
  const [blockQ, setBlockQ] = useState('')

  const blocks = useAdminQuery<{ rows: PageBlock[] }>(`/api/admin/page-blocks?slug=${encodeURIComponent(page.slug)}`)
  const visibleBlocks = useMemo(() => {
    const rows = blocks.data?.rows ?? []
    const needle = blockQ.trim().toLowerCase()
    if (!needle) return rows
    return rows.filter((b) => (b.label ?? '').toLowerCase().includes(needle) || (b.cvalue ?? '').toLowerCase().includes(needle))
  }, [blocks.data, blockQ])

  async function saveMeta() {
    setSavingMeta(true)
    setMetaMsg(null)
    try {
      await adminApi.patch(`/api/admin/pages/${page.id}`, { title, meta_description: meta, published, noindex })
      setMetaMsg('Saved.')
      onSaved()
    } catch (e) {
      setMetaMsg(e instanceof Error ? e.message : 'Could not save.')
    } finally {
      setSavingMeta(false)
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <div>
            <h2 style={{ margin: 0 }}>{page.slug}</h2>
            <a className="small" href={'/' + page.slug} target="_blank" rel="noreferrer">View page ↗</a>
          </div>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>

        <Card title="Search & metadata">
          {metaMsg && <div className={'notice' + (metaMsg === 'Saved.' ? '' : ' err')} style={{ marginBottom: '.75rem' }}>{metaMsg}</div>}
          <div className="field"><label>Browser / SEO title</label><input value={title} onChange={(e) => setTitle(e.target.value)} /></div>
          <div className="field"><label>Meta description</label><textarea rows={2} value={meta} onChange={(e) => setMeta(e.target.value)} /></div>
          <div className="row" style={{ gap: '1.5rem' }}>
            <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={published} onChange={(e) => setPublished(e.target.checked)} /> Published</label>
            <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={noindex} onChange={(e) => setNoindex(e.target.checked)} /> Hide from search engines</label>
          </div>
          <button className="btn sm" style={{ marginTop: '.75rem' }} disabled={savingMeta} onClick={saveMeta}>{savingMeta ? 'Saving…' : 'Save metadata'}</button>
        </Card>

        <Card title="Editable content">
          {blocks.loading ? (
            <Spinner />
          ) : blocks.error ? (
            <ErrorNote>{blocks.error}</ErrorNote>
          ) : !blocks.data || blocks.data.rows.length === 0 ? (
            <Empty>No editable content blocks for this page.</Empty>
          ) : (
            <div className="page">
              <div>
                <input placeholder={`Filter ${blocks.data.rows.length} text blocks…`} value={blockQ} onChange={(e) => setBlockQ(e.target.value)} />
                <div className="small muted" style={{ marginTop: '.35rem' }}>
                  Every text on this page is editable, in page order. Basic formatting tags (links, bold, lists) are kept; anything unsafe is removed on save.
                </div>
              </div>
              {visibleBlocks.length === 0 ? (
                <Empty>No blocks match the filter.</Empty>
              ) : (
                visibleBlocks.map((b) => <BlockEditor key={b.id} block={b} onSaved={onSaved} onReset={() => blocks.refetch()} />)
              )}
            </div>
          )}
        </Card>
      </div>
    </div>
  )
}

function BlockEditor({ block, onSaved, onReset }: { block: PageBlock; onSaved: () => void; onReset: () => void }) {
  const [val, setVal] = useState(block.cvalue ?? '')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<string | null>(null)
  const dirty = val !== (block.cvalue ?? '')

  async function save() {
    setBusy(true)
    setMsg(null)
    try {
      await adminApi.patch(`/api/admin/page-blocks/${block.id}`, { cvalue: val })
      block.cvalue = val
      setMsg('Saved')
      onSaved()
    } catch (e) {
      setMsg(e instanceof Error ? e.message : 'Error')
    } finally {
      setBusy(false)
    }
  }

  // deleting the row reverts the region to the text shipped in the page file
  async function reset() {
    if (!confirm('Restore this text to the original shipped with the website?\n\nNote: the page reverts immediately, but this block will disappear from the editor until the next server restart/deploy re-scans the page.')) return
    setBusy(true)
    try {
      await adminApi.del(`/api/admin/page-blocks/${block.id}`)
      onSaved()
      onReset()
    } catch (e) {
      setMsg(e instanceof Error ? e.message : 'Error')
      setBusy(false)
    }
  }

  const label = block.label || (block.block_key === '_h1' ? 'Headline' : block.block_key)
  const long = (block.cvalue ?? '').length > 80 || block.ctype === 'html'
  return (
    <div>
      <label>{label} {block.block_key === '_h1' && <Badge tone="brand">headline</Badge>} {block.ctype === 'html' && <Badge>rich text</Badge>}</label>
      {long ? (
        <textarea rows={3} value={val} onChange={(e) => setVal(e.target.value)} />
      ) : (
        <input value={val} onChange={(e) => setVal(e.target.value)} />
      )}
      <div className="row" style={{ marginTop: '.4rem' }}>
        <button className="btn sm" disabled={busy || !dirty} onClick={save}>{busy ? 'Saving…' : 'Save'}</button>
        <button className="btn ghost sm" disabled={busy} onClick={reset}>Reset to original</button>
        {msg && <span className="small muted">{msg}</span>}
      </div>
    </div>
  )
}

export default function Pages() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: PageRow[] }>('/api/admin/pages')
  const [q, setQ] = useState('')
  const [editing, setEditing] = useState<PageRow | null>(null)

  const filtered = useMemo(() => {
    const rows = data?.rows ?? []
    const needle = q.trim().toLowerCase()
    return needle ? rows.filter((p) => p.slug.toLowerCase().includes(needle) || (p.title ?? '').toLowerCase().includes(needle)) : rows
  }, [data, q])

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <PageHeader
        title="Pages & content"
        subtitle="Edit any page’s headline, SEO title and description. Changes go live immediately — no redeploy."
      />

      <Card>
        <input placeholder="Search pages…" value={q} onChange={(e) => setQ(e.target.value)} style={{ maxWidth: 320 }} />
      </Card>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : filtered.length === 0 ? (
          <Empty>No pages match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Page</th><th>Title</th><th>Group</th><th>Status</th><th></th></tr>
            </thead>
            <tbody>
              {filtered.map((p) => (
                <tr key={p.id}>
                  <td><strong>{p.slug}</strong></td>
                  <td className="small">{p.title || <span className="muted">—</span>}</td>
                  <td className="small muted">{p.nav_group || '—'}</td>
                  <td>
                    {p.published === 0 ? <Badge tone="err">unpublished</Badge> : <Badge tone="ok">live</Badge>}
                    {p.noindex === 1 && <> <Badge tone="warn">noindex</Badge></>}
                  </td>
                  <td><button className="btn ghost sm" onClick={() => setEditing(p)}>Edit</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {editing && <PageEditor page={editing} onClose={() => setEditing(null)} onSaved={refetch} />}
    </div>
  )
}
