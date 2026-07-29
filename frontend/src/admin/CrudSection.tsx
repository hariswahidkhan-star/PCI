import { useMemo, useState, type ReactNode } from 'react'
import { useAdminQuery } from './hooks'
import { adminApi } from './api'
import { Card, Badge, ErrorNote } from '../components/ui'
import {
  PageHeader,
  Toolbar,
  SearchInput,
  FilterSelect,
  SortHeader,
  EmptyState,
  SkeletonTable,
  Pagination,
} from '../components/premium'
import { useTableControls } from '../components/useTableControls'
import { Icon } from '../components/icons'

export interface CrudField {
  key: string
  label: string
  type?: 'text' | 'textarea' | 'number' | 'checkbox' | 'select'
  options?: { value: string; label: string }[]
  inList?: boolean // show as a table column
  placeholder?: string
  required?: boolean // block save + surface an error when left blank (NOT NULL columns)
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
  const noun = config.title.replace(/s$/, '').toLowerCase()

  async function save() {
    setError(null)
    // block on empty required fields (NOT NULL columns) before hitting the server
    const missing = config.fields.find((f) => f.required && !String(form[f.key] ?? '').trim())
    if (missing) {
      setError(`${missing.label} is required.`)
      return
    }
    setBusy(true)
    try {
      // send booleans as-is (server maps true/false -> 1/0); coerce numbers; blank text -> null so
      // UNIQUE-but-nullable columns don't collide on empty strings.
      const payload: Record<string, unknown> = {}
      for (const f of config.fields) {
        let v = form[f.key]
        if (f.type === 'checkbox') v = isTruthy(v)
        else if (f.type === 'number') v = v === '' || v == null ? null : Number(v)
        else if (typeof v === 'string' && v.trim() === '') v = null
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
      <div className="drawer" onClick={(e) => e.stopPropagation()} role="dialog" aria-modal="true" aria-label={isNew ? `New ${noun}` : `Edit ${noun}`}>
        <div className="drawer-head">
          <h2>{isNew ? `New ${noun}` : `Edit ${noun}`}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
        <div className="grid cols-2">
          {config.fields.map((f) => {
            const full = f.type === 'textarea'
            const id = `crud-${config.name}-${f.key}`
            const labelText = f.label + (f.required ? ' *' : '')
            return (
              <div className="field" key={f.key} style={full ? { gridColumn: '1 / -1' } : undefined}>
                {f.type !== 'checkbox' && <label htmlFor={id}>{labelText}</label>}
                {f.type === 'textarea' ? (
                  <textarea id={id} rows={3} value={String(form[f.key] ?? '')} onChange={(e) => set(f.key, e.target.value)} placeholder={f.placeholder} />
                ) : f.type === 'checkbox' ? (
                  <label className="row" style={{ fontWeight: 400 }}>
                    <input id={id} type="checkbox" style={{ width: 'auto' }} checked={isTruthy(form[f.key])} onChange={(e) => set(f.key, e.target.checked)} /> {f.label}
                  </label>
                ) : f.type === 'select' ? (
                  <select id={id} value={String(form[f.key] ?? '')} onChange={(e) => set(f.key, e.target.value)}>
                    <option value="">—</option>
                    {f.options?.map((o) => <option key={o.value} value={o.value}>{o.label}</option>)}
                  </select>
                ) : (
                  <input id={id} type={f.type === 'number' ? 'number' : 'text'} value={String(form[f.key] ?? '')} onChange={(e) => set(f.key, e.target.value)} placeholder={f.placeholder} />
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
  const listFields = useMemo(() => config.fields.filter((f) => f.inList), [config.fields])

  // Dropdown filters are offered for every select/checkbox column the list shows — those are the
  // columns with a small, known value set, which is exactly what an operator filters on.
  const filterFields = useMemo(
    () => listFields.filter((f) => f.type === 'select' || f.type === 'checkbox'),
    [listFields],
  )
  const [filterVals, setFilterVals] = useState<Record<string, string>>({})
  const setFilter = (k: string, v: string) => setFilterVals((p) => ({ ...p, [k]: v }))

  const searchKeys = useMemo(() => listFields.map((f) => f.key), [listFields])
  const filters = filterFields.map((f) => {
    const want = filterVals[f.key]
    if (!want) return null
    if (f.type === 'checkbox') return (r: Row) => (want === 'yes') === isTruthy(r[f.key])
    return (r: Row) => String(r[f.key] ?? '') === want
  })

  const t = useTableControls<Row>(data?.rows, {
    searchKeys,
    filters,
    // The dropdown values ARE the filter state — a stable signature the hook can both
    // memoise on and use to detect that the active filters changed.
    filterKey: JSON.stringify(filterVals),
    pageSize: 25,
  })

  async function remove(row: Row) {
    if (!confirm('Delete this item? This cannot be undone.')) return
    try {
      await adminApi.del(`/api/admin/${config.name}/${row.id}`)
      refetch()
    } catch (e) {
      alert(e instanceof Error ? e.message : 'Could not delete.')
    }
  }

  const totalRows = data?.rows.length ?? 0
  const filtered = t.total !== totalRows
  const noun = config.title.toLowerCase()

  return (
    <div className="page">
      <PageHeader
        title={config.title}
        subtitle={config.description}
        actions={
          <button className="btn sm" onClick={() => setEditing(null)}>
            <Icon name="plus" size={14} />
            {config.addLabel ?? 'Add'}
          </button>
        }
      />

      {/* The toolbar is only useful once there is something to narrow. */}
      {totalRows > 0 && (
        <Toolbar count={filtered ? `${t.total} of ${totalRows} shown` : `${totalRows} ${totalRows === 1 ? 'record' : 'records'}`}>
          <SearchInput
            value={t.query}
            onChange={t.setQuery}
            label="Search"
            placeholder={`Search ${noun}…`}
          />
          {filterFields.map((f) => (
            <FilterSelect
              key={f.key}
              label={f.label}
              value={filterVals[f.key] ?? ''}
              onChange={(v) => setFilter(f.key, v)}
              options={
                f.type === 'checkbox'
                  ? [{ value: 'yes', label: 'Yes' }, { value: 'no', label: 'No' }]
                  : (f.options ?? [])
              }
            />
          ))}
        </Toolbar>
      )}

      <Card className="flat">
        {loading ? (
          <SkeletonTable rows={6} cols={Math.max(2, listFields.length)} />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : totalRows === 0 ? (
          <EmptyState
            icon="archive"
            title={`No ${noun} yet`}
            detail={`Nothing has been added to ${noun} yet. Everything you add here is published through the platform immediately.`}
            action={<button className="btn sm" onClick={() => setEditing(null)}>{config.addLabel ?? 'Add'}</button>}
          />
        ) : t.total === 0 ? (
          <EmptyState
            icon="search"
            title="No match"
            detail="No record matches the current search and filters."
            action={
              <button
                className="btn secondary sm"
                onClick={() => {
                  t.setQuery('')
                  setFilterVals({})
                }}
              >
                Clear filters
              </button>
            }
          />
        ) : (
          <>
            <div className="table-scroll">
              <table className="data">
                <thead>
                  <tr>
                    {listFields.map((f) => (
                      <SortHeader key={f.key} {...t.sortProps(f.key)} onSort={() => t.toggleSort(f.key)}>
                        {f.label}
                      </SortHeader>
                    ))}
                    <th aria-label="Row actions" />
                  </tr>
                </thead>
                <tbody>
                  {t.rows.map((row) => (
                    <tr key={String(row.id)}>
                      {listFields.map((f) => <td key={f.key} className="small">{cell(f, row[f.key])}</td>)}
                      <td className="actions">
                        <button className="btn ghost sm" onClick={() => setEditing(row)}>Edit</button>
                        <button className="btn ghost sm" onClick={() => remove(row)}>Delete</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <Pagination page={t.page} pages={t.pages} total={t.total} pageSize={t.pageSize} onPage={t.setPage} />
          </>
        )}
      </Card>

      {editing !== undefined && (
        <Editor config={config} row={editing} onClose={() => setEditing(undefined)} onSaved={() => { setEditing(undefined); refetch() }} />
      )}
    </div>
  )
}
