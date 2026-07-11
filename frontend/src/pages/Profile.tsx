import { useState, type FormEvent } from 'react'
import { Link } from 'react-router-dom'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote } from '../components/ui'
import Ring from '../components/Ring'
import RepeaterSection from '../components/RepeaterSection'
import { EXPERIENCE_FIELDS, QUALIFICATION_FIELDS, HELD_CERT_FIELDS, monthsCovered } from '../data/wizardFields'
import { fmtDate } from '../format'
import type { Experience, Qualification, HeldCertification } from '../api/types'

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
  const completion = Number(profile.profile_completion_percentage ?? 20)
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
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread" style={{ alignItems: 'flex-start', flexWrap: 'wrap', gap: '1rem' }}>
        <div>
          <h1>Profile</h1>
          <p className="muted" style={{ marginBottom: 0 }}>
            Your professional record — experience, qualifications and credentials in one place.{' '}
            <Link to="/onboarding">Run the guided setup →</Link>
          </p>
        </div>
        <Ring value={completion} label="complete" />
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

      <Card>
        <RepeaterSection<Experience>
          title="Work experience"
          blurb="One entry per role — add as many companies as you need. Overlapping months count once."
          route="/api/me/experiences"
          fields={EXPERIENCE_FIELDS}
          addLabel="Add experience"
          emptyHint="No roles yet. Your experience record supports exam eligibility."
          itemTitle={(r) => `${r.title} — ${r.company}`}
          itemSub={(r) => [r.start_date, r.is_current ? 'present' : r.end_date].filter(Boolean).join(' → ') + (r.country ? ` · ${r.country}` : '')}
          itemBody={(r) => r.summary}
          footer={(rows) => {
            const months = monthsCovered(rows)
            return months > 0 ? (
              <p className="muted small" style={{ margin: '.75rem 0 0' }}>
                Total recorded: <strong>{Math.floor(months / 12)}y {months % 12}m</strong> across {rows.length} role{rows.length === 1 ? '' : 's'} (overlaps counted once).
              </p>
            ) : null
          }}
        />
      </Card>

      <Card>
        <RepeaterSection<Qualification>
          title="Qualifications"
          blurb="Degrees, diplomas and academic awards."
          route="/api/me/qualifications"
          fields={QUALIFICATION_FIELDS}
          addLabel="Add qualification"
          emptyHint="No qualifications recorded yet."
          itemTitle={(r) => r.degree}
          itemSub={(r) => [r.institution, r.year_completed, r.country].filter(Boolean).join(' · ')}
        />
      </Card>

      <Card>
        <RepeaterSection<HeldCertification>
          title="Certifications you hold"
          blurb="Credentials from other bodies (PMP, CCP, PSP…)."
          route="/api/me/certifications-held"
          fields={HELD_CERT_FIELDS}
          addLabel="Add certification"
          emptyHint="No external certifications recorded."
          itemTitle={(r) => r.name}
          itemSub={(r) => [r.issuer, r.issued_year && `since ${r.issued_year}`, r.expires_year && `expires ${r.expires_year}`].filter(Boolean).join(' · ')}
        />
      </Card>
    </div>
  )
}
