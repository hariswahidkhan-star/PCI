import { useState, type FormEvent, type ReactNode } from 'react'
import { api } from '../api/client'
import { useQuery } from '../api/hooks'
import { useMe } from '../data/MeContext'
import { Spinner, ErrorNote } from './ui'

export interface RepeaterField {
  key: string
  label: string
  type?: 'text' | 'month' | 'number' | 'textarea' | 'checkbox' | 'url'
  placeholder?: string
  required?: boolean
  span2?: boolean
}

interface Props<T extends { id: number }> {
  title: string
  blurb?: string
  route: string // e.g. /api/me/experiences
  fields: RepeaterField[]
  addLabel: string
  emptyHint: string
  itemTitle: (row: T) => string
  itemSub: (row: T) => string
  itemBody?: (row: T) => string | null | undefined
  footer?: (rows: T[]) => ReactNode
  onChanged?: (rows: T[]) => void
}

/** "Add more" collection editor — the PMI-style repeater used for work experience,
 * qualifications and held certifications, in both the onboarding wizard and the profile. */
export default function RepeaterSection<T extends { id: number }>(p: Props<T>) {
  const { refetch: refetchMe } = useMe()
  const { data, loading, error, refetch } = useQuery<{ rows: T[] }>(p.route)
  const [editing, setEditing] = useState<'new' | number | null>(null)
  const [form, setForm] = useState<Record<string, string>>({})
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  const rows = data?.rows ?? []

  function openNew() {
    setForm({})
    setErr(null)
    setEditing('new')
  }
  function openEdit(row: T) {
    const f: Record<string, string> = {}
    for (const fd of p.fields) f[fd.key] = String((row as Record<string, unknown>)[fd.key] ?? '')
    setForm(f)
    setErr(null)
    setEditing(row.id)
  }

  async function save(e: FormEvent) {
    e.preventDefault()
    setErr(null)
    for (const fd of p.fields)
      if (fd.required && !String(form[fd.key] ?? '').trim()) {
        setErr(`${fd.label} is required.`)
        return
      }
    setBusy(true)
    try {
      if (editing === 'new') await api.post(p.route, form)
      else await api.patch(`${p.route}/${editing}`, form)
      setEditing(null)
      refetch()
      refetchMe()
    } catch (e2) {
      setErr(e2 instanceof Error ? e2.message : 'Could not save. Please try again.')
    } finally {
      setBusy(false)
    }
  }

  async function remove(id: number) {
    if (!confirm('Remove this entry?')) return
    await api.del(`${p.route}/${id}`).catch(() => {})
    refetch()
    refetchMe()
  }

  return (
    <div className="rep">
      <div className="spread">
        <div>
          <h3 className="section-title">{p.title}</h3>
          {p.blurb && <p className="muted small" style={{ margin: '.2rem 0 0' }}>{p.blurb}</p>}
        </div>
        {editing === null && (
          <button className="btn secondary sm" type="button" onClick={openNew}>+ {p.addLabel}</button>
        )}
      </div>

      {loading ? (
        <Spinner />
      ) : error && rows.length === 0 && editing === null ? (
        <ErrorNote>
          <div className="spread" style={{ gap: '.75rem', alignItems: 'center' }}>
            <span>We couldn’t load this section. {error}</span>
            <button className="btn ghost sm" type="button" onClick={refetch}>Try again</button>
          </div>
        </ErrorNote>
      ) : rows.length === 0 && editing === null ? (
        <div className="rep-empty">
          <p className="muted small" style={{ margin: 0 }}>{p.emptyHint}</p>
          <button className="btn sm" type="button" onClick={openNew}>+ {p.addLabel}</button>
        </div>
      ) : (
        <div className="rep-list stagger">
          {rows.map((row) => (
            <div className="rep-item" key={row.id}>
              <div className="rep-item-main">
                <strong>{p.itemTitle(row)}</strong>
                <div className="muted small">{p.itemSub(row)}</div>
                {p.itemBody?.(row) && <p className="small" style={{ margin: '.35rem 0 0' }}>{p.itemBody(row)}</p>}
              </div>
              <div className="rep-item-actions">
                <button className="btn ghost sm" type="button" onClick={() => openEdit(row)}>Edit</button>
                <button className="btn ghost sm" type="button" onClick={() => remove(row.id)}>Remove</button>
              </div>
            </div>
          ))}
        </div>
      )}

      {editing !== null && (
        <form className="rep-form" onSubmit={save}>
          {err && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{err}</div>}
          <div className="grid cols-2">
            {p.fields.map((fd) => (
              <div className={'field' + (fd.span2 || fd.type === 'textarea' ? ' span2' : '')} key={fd.key}>
                {fd.type === 'checkbox' ? (
                  <label className="row" style={{ fontWeight: 500, textTransform: 'none' }}>
                    <input
                      type="checkbox"
                      style={{ width: 'auto' }}
                      checked={form[fd.key] === '1' || form[fd.key] === 'true'}
                      onChange={(e) => setForm({ ...form, [fd.key]: e.target.checked ? '1' : '0' })}
                    />
                    {fd.label}
                  </label>
                ) : (
                  <>
                    <label htmlFor={p.route + fd.key}>{fd.label}{fd.required && ' *'}</label>
                    {fd.type === 'textarea' ? (
                      <textarea
                        id={p.route + fd.key}
                        rows={3}
                        placeholder={fd.placeholder}
                        value={form[fd.key] ?? ''}
                        onChange={(e) => setForm({ ...form, [fd.key]: e.target.value })}
                      />
                    ) : (
                      <input
                        id={p.route + fd.key}
                        type={fd.type ?? 'text'}
                        placeholder={fd.placeholder}
                        value={form[fd.key] ?? ''}
                        onChange={(e) => setForm({ ...form, [fd.key]: e.target.value })}
                      />
                    )}
                  </>
                )}
              </div>
            ))}
          </div>
          <div className="row">
            <button className="btn sm" disabled={busy}>{busy ? 'Saving…' : editing === 'new' ? 'Add' : 'Save'}</button>
            <button className="btn ghost sm" type="button" onClick={() => setEditing(null)}>Cancel</button>
          </div>
        </form>
      )}

      {p.footer?.(rows)}
    </div>
  )
}
