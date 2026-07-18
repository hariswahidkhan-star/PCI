import { Fragment, useEffect, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDateTime } from '../../format'

// Error-reference queue (gate 'inbox'): every captured student-facing error, searchable by the
// PCI-XXXX reference a student quotes to support. Expanding a row shows exactly what the student
// saw plus the technical summary; the status select drives open → investigating → resolved.

interface ErrorRow {
  id: number
  reference: string
  user_id?: number | null
  email?: string | null
  page?: string | null
  category?: string | null
  user_message?: string | null
  tech_summary?: string | null
  browser?: string | null
  os?: string | null
  app_version?: string | null
  status: string
  resolution_note?: string | null
  created_at?: string | null
}

const STATUSES = ['open', 'investigating', 'resolved'] as const
const STATUS_COLOUR: Record<string, string> = { open: 'var(--err,#dc2626)', investigating: 'var(--warn,#d97706)', resolved: 'var(--ok,#16a34a)' }

export default function ErrorReports() {
  const [ref, setRef] = useState('')
  const [dref, setDref] = useState('')
  const [status, setStatus] = useState('')
  const [open, setOpen] = useState<number | null>(null)
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  // Debounce the reference search so the list doesn't refetch on every keystroke.
  useEffect(() => {
    const t = setTimeout(() => setDref(ref.trim()), 300)
    return () => clearTimeout(t)
  }, [ref])
  const params = new URLSearchParams()
  if (dref) params.set('ref', dref)
  if (status) params.set('status', status)
  const qs = params.toString()
  const { data, loading, error, refetch } = useAdminQuery<{ rows: ErrorRow[] }>(`/api/admin/errors${qs ? '?' + qs : ''}`)

  async function setRowStatus(row: ErrorRow, newStatus: string) {
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/errors/${row.id}/status`, { status: newStatus, note: open === row.id && note.trim() ? note.trim() : undefined })
      if (open === row.id) setNote('')
      refetch()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not update the status.')
    } finally { setBusy(false) }
  }

  const rows = data?.rows ?? []
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Error reports</h1>
        <p className="muted">When something fails, the student sees a reference like <span style={{ fontFamily: 'monospace' }}>PCI-2026-A1B2C3</span>. Search it here to see what happened without asking them for screenshots.</p>
      </div>

      <Card>
        <div className="row" style={{ flexWrap: 'wrap' }}>
          <input placeholder="Search by reference (PCI-…)" value={ref} onChange={(e) => setRef(e.target.value)} style={{ maxWidth: 280 }} aria-label="Reference" />
          <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }} aria-label="Status filter">
            <option value="">All statuses</option>
            {STATUSES.map((st) => <option key={st} value={st}>{st}</option>)}
          </select>
        </div>
      </Card>

      <Card>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        {loading && !data ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <Empty>No error reports match.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Reference</th><th>Student</th><th>Page</th><th>Category</th><th>Browser / OS</th><th>Created</th><th>Status</th></tr>
            </thead>
            <tbody>
              {rows.map((r) => (
                <Fragment key={r.id}>
                  <tr
                    role="button"
                    tabIndex={0}
                    style={{ cursor: 'pointer' }}
                    onClick={() => { setOpen(open === r.id ? null : r.id); setNote('') }}
                    onKeyDown={(e) => { if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); setOpen(open === r.id ? null : r.id); setNote('') } }}
                    aria-expanded={open === r.id}
                  >
                    <td className="small" style={{ fontFamily: 'monospace' }}>{r.reference}</td>
                    <td className="small">{r.email || <span className="muted">anonymous</span>}</td>
                    <td className="small">{r.page || '—'}</td>
                    <td className="small">{r.category || '—'}</td>
                    <td className="small muted">{[r.browser, r.os].filter(Boolean).join(' / ') || '—'}</td>
                    <td className="small muted">{fmtDateTime(r.created_at)}</td>
                    <td onClick={(e) => e.stopPropagation()}>
                      <select
                        value={r.status}
                        disabled={busy}
                        onChange={(e) => setRowStatus(r, e.target.value)}
                        aria-label={`Status for ${r.reference}`}
                        style={{ color: STATUS_COLOUR[r.status] }}
                      >
                        {STATUSES.map((st) => <option key={st} value={st}>{st}</option>)}
                      </select>
                    </td>
                  </tr>
                  {open === r.id && (
                    <tr>
                      <td colSpan={7} style={{ background: 'var(--canvas)' }}>
                        <div style={{ display: 'grid', gap: '.5rem', padding: '.4rem 0' }}>
                          <div>
                            <div className="muted small">What the student saw</div>
                            <div className="small" style={{ whiteSpace: 'pre-wrap' }}>{r.user_message || '—'}</div>
                          </div>
                          <div>
                            <div className="muted small">Technical summary</div>
                            <div className="small" style={{ whiteSpace: 'pre-wrap', fontFamily: 'monospace' }}>{r.tech_summary || '—'}</div>
                          </div>
                          {r.app_version && <div className="muted small">App version: {r.app_version}</div>}
                          {r.resolution_note && (
                            <div>
                              <div className="muted small">Resolution note</div>
                              <div className="small" style={{ whiteSpace: 'pre-wrap' }}>{r.resolution_note}</div>
                            </div>
                          )}
                          <label className="small">Note <span className="muted">(optional — saved with the next status change)</span>
                            <input value={note} onChange={(e) => setNote(e.target.value)} placeholder="e.g. fixed in tonight's deploy" />
                          </label>
                        </div>
                      </td>
                    </tr>
                  )}
                </Fragment>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
