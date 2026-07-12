import { useState, type FormEvent } from 'react'
import { useQuery, runMutation } from '../api/hooks'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote, Empty, StatusBadge } from '../components/ui'
import { fmtDate } from '../format'
import { useT } from '../i18n'

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
  const t = useT()
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
      setMsg(err instanceof Error ? err.message : t('cpd.saveError'))
    } finally {
      setBusy(false)
    }
  }

  const remove = (id: number) =>
    runMutation(async () => { await api.del(`/api/me/cpd/${id}`); refetch(); refetchMe() })

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>{t('cpd.heading')}</h1>
        <p className="muted">{t('cpd.subtitle', { target })}</p>
      </div>

      <Card title={t('cpd.progress')}>
        <div className="spread" style={{ marginBottom: '.5rem' }}>
          <strong>{t('cpd.hoursOf', { total, target })}</strong>
          <span className="muted small">{pct}%</span>
        </div>
        <div style={{ height: 10, background: 'var(--canvas)', borderRadius: 999, overflow: 'hidden' }}>
          <div style={{ width: `${pct}%`, height: '100%', background: 'var(--brand)' }} />
        </div>
      </Card>

      <Card title={t('cpd.addActivity')}>
        {msg && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{msg}</div>}
        <form onSubmit={add}>
          <div className="grid cols-2">
            <div className="field">
              <label htmlFor="cpd-activity">{t('cpd.activity')}</label>
              <input id="cpd-activity" required value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} placeholder={t('cpd.activityPlaceholder')} />
            </div>
            <div className="field">
              <label htmlFor="cpd-category">{t('cpd.category')}</label>
              <select id="cpd-category" value={form.category} onChange={(e) => setForm({ ...form, category: e.target.value })}>
                {CATEGORIES.map((c) => <option key={c}>{c}</option>)}
              </select>
            </div>
            <div className="field">
              <label htmlFor="cpd-hours">{t('cpd.hours')}</label>
              <input id="cpd-hours" type="number" step="0.5" min="0" required value={form.hours} onChange={(e) => setForm({ ...form, hours: e.target.value })} />
            </div>
            <div className="field">
              <label htmlFor="cpd-date">{t('cpd.date')}</label>
              <input id="cpd-date" type="date" value={form.activity_date} onChange={(e) => setForm({ ...form, activity_date: e.target.value })} />
            </div>
          </div>
          <button className="btn sm" disabled={busy}>{busy ? t('cpd.saving') : t('cpd.addEntry')}</button>
        </form>
      </Card>

      <Card title={t('cpd.loggedActivities')}>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>{t('cpd.empty')}</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>{t('cpd.date')}</th><th>{t('cpd.activity')}</th><th>{t('cpd.category')}</th><th>{t('cpd.hours')}</th><th>{t('cpd.status')}</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => (
                <tr key={r.id}>
                  <td>{fmtDate(r.activity_date)}</td>
                  <td>{r.description}</td>
                  <td>{r.category}</td>
                  <td>{r.hours}</td>
                  <td><StatusBadge status={r.status || 'pending'} /></td>
                  <td><button className="btn ghost sm" onClick={() => remove(r.id)} aria-label={t('cpd.deleteAria')}>{t('cpd.remove')}</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
