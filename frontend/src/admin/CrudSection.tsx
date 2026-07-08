import { useState, type ReactNode } from 'react'
import { useAdminQuery } from './hooks'
import { adminApi } from './api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'

export interface CrudField {
  key: string
  label: string
  type?: 'text' | 'textarea' | 'number' | 'checkbox' | 'select'
  options?: { value: string; label: string }[]
  inList?: boolean // show as a table column
  placeholder?: string
}

export interface CrudConfig {
  name: string // table / endpoint segment
  path: string // route segment under /admin/
  title: string
  perm: string
  description?: string
  fields: CrudField[]
  addLabel?: string
}

type Row = Record<string, unknown>

function isTruthy(v: unknown): boolean {
  return v === 1 || v === true || v === '1' || v === 'true'
}

function Editor({ config, row, onClose, onSaved }: { config: CrudConfig; row: Row | null; onClose: () => void; onSaved: () => void }) {
  const isNew = !row
  const init: Record<string, unknown> = {}
  for (const f of config.fields) init[f.key] = row ? row[f.key] : f.type === 'checkbox' ? false : ''
  const [form, setForm] = useState<Record<string, unknown>>(init)
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const set = (k: string, v: unknown) => setForm((p) => ({ ...p, [k]: v }))

  async function save() {
    setBusy(true)
    setError(null)
    try {
      // send booleans as-is (server maps true/false -> 1/0); coerce number fields
      const payload: Record<string, unknown> = {}
      for (const f of config.fields) {
        let v = form[f.key]
        if (f.type === 'checkbox') v = isTruthy(v)
        else if (f.type === 'number') v = v === '' || v == null ? null : Number(v)
        payload[f.key] = v
      }
      if (isNew) await adminApi.post(`/api/admin/${config.name}`, payload)
      else await adminApi.patch(`/api/admin/${config.name}/${row!.id}`, payload)
      onSaved()
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Could not save.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{isNew ? `New ${config.title.replace(/s$/, '').toLowerCase()}` : `Edit ${config.title.replace(/s$/, '').toLowerCase()}`}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
        <div className="grid cols-2">
          {config.fields.map((f) => {
            const full = f.type === 'textarea'
            return (
              <div className="field" key={f.key} style={full ? { gridColumn: '1 / -1' } : undefined}>
                <label>{f.label}</label>
                {f.type === 'textarea' ? (
                  <textarea rows={3} value={String(form[f.key] ?? '')} onChange={(e) => set(f.key, e.target.value)} placeholder={f.placeholder} />
                ) : f.type === 'checkbox' ? (
                  <label className="row" style={{ fontWeight: 400 }}>
                    <input type="checkbox" style={{ width: 'auto' }} checked={isTruthy(form[f.key])} onChange={(e) => set(f.key, e.target.checked)} /> {f.label}
                  </label>
                ) : f.type === 'select' ? (
                  <select value={String(form[f.key] ?? '')} onChange={(e) => set(f.key, e.target.value)}>
                    <option value="">—</option>
                    {f.options?.map((o) => <option key={o.value} value={o.value}>{o.label}</option>)}
                  </select>
                ) : (
                  <input type={f.type === 'number' ? 'number' : 'text'} value={String(form[f.key] ?? '')} onChange={(e) => set(f.key, e.target.value)} placeholder={f.placeholder} />
                )}
              </div>
            )
          })}
        </div>
        <button className="btn" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save'}</button>
      </div>
    </div>
  )
}

function cell(f: CrudField, v: unknown): ReactNode {
  if (f.type === 'checkbox') return isTruthy(v) ? <Badge tone="ok">yes</Badge> : <span className="muted small">no</span>
  const s = v == null ? '' : String(v)
  return s.length > 80 ? s.slice(0, 80) + '…' : s || <span className="muted">—</span>
}

export default function CrudSection({ config }: { config: CrudConfig }) {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Row[] }>(`/api/admin/${config.name}`)
  const [editing, setEditing] = useState<Row | null | undefined>(undefined) // undefined = closed
  const listFields = config.fields.filter((f) => f.inList)

  async function remove(row: Row) {
    if (!confirm('Delete this item? This cannot be undone.')) return
    try {
      await adminApi.del(`/api/admin/${config.name}/${row.id}`)
      refetch()
    } catch (e) {
      alert(e instanceof Error ? e.message : 'Could not delete.')
    }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <div>
          <h1>{config.title}</h1>
          {config.description && <p className="muted" style={{ margin: 0 }}>{config.description}</p>}
        </div>
        <button className="btn sm" onClick={() => setEditing(null)}>{config.addLabel ?? 'Add'}</button>
      </div>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>Nothing here yet.</Empty>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="data">
              <thead>
                <tr>{listFields.map((f) => <th key={f.key}>{f.label}</th>)}<th></th><th></th></tr>
              </thead>
              <tbody>
                {data.rows.map((row) => (
                  <tr key={String(row.id)}>
                    {listFields.map((f) => <td key={f.key} className="small">{cell(f, row[f.key])}</td>)}
                    <td><button className="btn ghost sm" onClick={() => setEditing(row)}>Edit</button></td>
                    <td><button className="btn ghost sm" onClick={() => remove(row)}>Delete</button></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </Card>

      {editing !== undefined && (
        <Editor config={config} row={editing} onClose={() => setEditing(undefined)} onSaved={() => { setEditing(undefined); refetch() }} />
      )}
    </div>
  )
}
