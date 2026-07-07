import { useState, type FormEvent } from 'react'
import { useQuery } from '../api/hooks'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote, Empty, StatusBadge } from '../components/ui'
import { fmtDate } from '../format'

interface CpdRow {
  id: number
  activity_date?: string | null
  category?: string | null
  hours?: number | null
  description?: string | null
  status?: string | null
}

const CATEGORIES = ['General', 'Training course', 'Conference', 'Self-study', 'Mentoring', 'Publication', 'On-the-job']

export default function Cpd() {
  const { me, refetch: refetchMe } = useMe()
  const { data, loading, error, refetch } = useQuery<{ rows: CpdRow[] }>('/api/me/cpd')
  const [form, setForm] = useState({ description: '', category: 'General', hours: '', activity_date: '' })
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<string | null>(null)

  const target = me?.cpd.target ?? 60
  const total = me?.cpd.total ?? 0
  const pct = Math.min(100, Math.round((total / target) * 100))

  async function add(e: FormEvent) {
    e.preventDefault()
    setBusy(true)
    setMsg(null)
    try {
      await api.post('/api/me/cpd', { ...form, hours: parseFloat(form.hours) || 0 })
      setForm({ description: '', category: 'General', hours: '', activity_date: '' })
      refetch()
      refetchMe()
    } catch (err) {
      setMsg(err instanceof Error ? err.message : 'Could not save entry.')
    } finally {
      setBusy(false)
    }
  }

  async function remove(id: number) {
    await api.del(`/api/me/cpd/${id}`)
    refetch()
    refetchMe()
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Continuing professional development</h1>
        <p className="muted">Log CPD to keep your credential current. Target: {target} hours.</p>
      </div>

      <Card title="Progress">
        <div className="spread" style={{ marginBottom: '.5rem' }}>
          <strong>{total} of {target} hours</strong>
          <span className="muted small">{pct}%</span>
        </div>
        <div style={{ height: 10, background: 'var(--canvas)', borderRadius: 999, overflow: 'hidden' }}>
          <div style={{ width: `${pct}%`, height: '100%', background: 'var(--brand)' }} />
        </div>
      </Card>

      <Card title="Add a CPD activity">
        {msg && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{msg}</div>}
        <form onSubmit={add}>
          <div className="grid cols-2">
            <div className="field">
              <label>Activity</label>
              <input required value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} placeholder="e.g. EVM masterclass" />
            </div>
            <div className="field">
              <label>Category</label>
              <select value={form.category} onChange={(e) => setForm({ ...form, category: e.target.value })}>
                {CATEGORIES.map((c) => <option key={c}>{c}</option>)}
              </select>
            </div>
            <div className="field">
              <label>Hours</label>
              <input type="number" step="0.5" min="0" required value={form.hours} onChange={(e) => setForm({ ...form, hours: e.target.value })} />
            </div>
            <div className="field">
              <label>Date</label>
              <input type="date" value={form.activity_date} onChange={(e) => setForm({ ...form, activity_date: e.target.value })} />
            </div>
          </div>
          <button className="btn sm" disabled={busy}>{busy ? 'Saving…' : 'Add entry'}</button>
        </form>
      </Card>

      <Card title="Logged activities">
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No CPD logged yet.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Date</th><th>Activity</th><th>Category</th><th>Hours</th><th>Status</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => (
                <tr key={r.id}>
                  <td>{fmtDate(r.activity_date)}</td>
                  <td>{r.description}</td>
                  <td>{r.category}</td>
                  <td>{r.hours}</td>
                  <td><StatusBadge status={r.status || 'pending'} /></td>
                  <td><button className="btn ghost sm" onClick={() => remove(r.id)} aria-label="Delete">Remove</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
