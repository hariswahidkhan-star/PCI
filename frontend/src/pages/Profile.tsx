import { useState, type FormEvent } from 'react'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote } from '../components/ui'
import { fmtDate } from '../format'

// Mirrors the allow-list in PATCH /api/me/profile (backend).
const FIELDS: { key: string; label: string; type?: string }[] = [
  { key: 'mobile', label: 'Mobile' },
  { key: 'country', label: 'Country' },
  { key: 'city', label: 'City' },
  { key: 'preferred_language', label: 'Preferred language' },
  { key: 'current_role', label: 'Current role' },
  { key: 'company', label: 'Company' },
  { key: 'industry_sector', label: 'Industry sector' },
  { key: 'years_experience', label: 'Years of experience', type: 'number' },
  { key: 'highest_qualification', label: 'Highest qualification' },
  { key: 'project_controls_area', label: 'Project controls area' },
  { key: 'linkedin_url', label: 'LinkedIn URL', type: 'url' },
]

export default function Profile() {
  const { me, loading, error, refetch } = useMe()
  const [form, setForm] = useState<Record<string, string>>({})
  const [busy, setBusy] = useState(false)
  const [saved, setSaved] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  const profile = (me.profile ?? {}) as Record<string, unknown>
  const val = (k: string) => (k in form ? form[k] : String(profile[k] ?? ''))

  async function save(e: FormEvent) {
    e.preventDefault()
    setBusy(true)
    setSaved(false)
    setErr(null)
    try {
      await api.patch('/api/me/profile', form)
      setForm({})
      setSaved(true)
      refetch()
    } catch (e2) {
      setErr(e2 instanceof Error ? e2.message : 'Could not save profile.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Profile</h1>
        <p className="muted">Keep your details up to date — some are required before you can schedule an exam.</p>
      </div>

      <Card title="Account">
        <div className="grid cols-2 small">
          <div><span className="muted">Name</span><div><strong>{me.user.first_name} {me.user.last_name}</strong></div></div>
          <div><span className="muted">Email</span><div>{me.user.email}</div></div>
          <div><span className="muted">Registration no.</span><div>{me.user.registration_no}</div></div>
          <div><span className="muted">Member since</span><div>{fmtDate(me.user.created_at)}</div></div>
        </div>
        <p className="muted small" style={{ marginTop: '.75rem', marginBottom: 0 }}>
          To change your name or email, please <a href="/contact.html">contact support</a>.
        </p>
      </Card>

      <Card title="Details">
        {saved && <div className="notice" style={{ marginBottom: '.75rem' }}>Profile saved.</div>}
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{err}</div>}
        <form onSubmit={save}>
          <div className="grid cols-2">
            {FIELDS.map((f) => (
              <div className="field" key={f.key}>
                <label htmlFor={f.key}>{f.label}</label>
                <input
                  id={f.key}
                  type={f.type ?? 'text'}
                  value={val(f.key)}
                  onChange={(e) => setForm({ ...form, [f.key]: e.target.value })}
                />
              </div>
            ))}
          </div>
          <button className="btn sm" disabled={busy || Object.keys(form).length === 0}>
            {busy ? 'Saving…' : 'Save changes'}
          </button>
        </form>
      </Card>
    </div>
  )
}
