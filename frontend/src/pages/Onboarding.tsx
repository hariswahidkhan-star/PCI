import { useState, type FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { api } from '../api/client'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import RepeaterSection from '../components/RepeaterSection'
import Ring from '../components/Ring'
import { EXPERIENCE_FIELDS, QUALIFICATION_FIELDS, HELD_CERT_FIELDS, monthsCovered } from '../data/wizardFields'
import { COUNTRIES } from '../data/countries'
import type { Experience, Qualification, HeldCertification } from '../api/types'

const STEPS = ['Welcome', 'About you', 'Experience', 'Qualifications', 'Certifications', 'Done']

const ABOUT_FIELDS: { key: string; label: string; type?: string }[] = [
  { key: 'country', label: 'Country' },
  { key: 'city', label: 'City' },
  { key: 'mobile', label: 'Mobile' },
  { key: 'current_role', label: 'Current role' },
  { key: 'company', label: 'Company' },
  { key: 'industry_sector', label: 'Industry sector' },
  { key: 'years_experience', label: 'Years of experience', type: 'number' },
  { key: 'project_controls_area', label: 'Main project-controls area' },
  { key: 'highest_qualification', label: 'Highest qualification' },
  { key: 'linkedin_url', label: 'LinkedIn URL', type: 'url' },
]

/** First-run profile wizard — save-as-you-go, every step optional, nothing to pay.
 * Experience/qualifications/certifications write straight to the account as they are added. */
export default function Onboarding() {
  const { user } = useAuth()
  const { me, refetch } = useMe()
  const nav = useNavigate()
  const [step, setStep] = useState(0)
  const [form, setForm] = useState<Record<string, string>>({})
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  const profile = (me?.profile ?? {}) as Record<string, unknown>
  const completion = Number(profile.profile_completion_percentage ?? 20)

  async function saveAbout(e: FormEvent) {
    e.preventDefault()
    setBusy(true)
    setErr(null)
    try {
      if (Object.keys(form).length > 0) {
        await api.patch('/api/me/profile', form)
        setForm({})
        refetch()
      }
      setStep(2)
    } catch (e2) {
      setErr(e2 instanceof Error ? e2.message : 'Could not save your details. Please try again.')
    } finally {
      setBusy(false)
    }
  }

  const val = (k: string) => (k in form ? form[k] : String(profile[k] ?? ''))

  return (
    <div className="wizpage">
      <header className="wiz-head">
        <div className="row">
          <img src="/assets/logo.png" alt="PCI Global" style={{ height: 30, borderRadius: 7 }} onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <strong>Set up your profile</strong>
        </div>
        <Link className="small muted" to="/">Skip for now →</Link>
      </header>

      <div className="wiz-progress" role="progressbar" aria-valuenow={step + 1} aria-valuemin={1} aria-valuemax={STEPS.length}>
        <i style={{ width: `${((step + 1) / STEPS.length) * 100}%` }} />
      </div>
      <ol className="wiz-steps small">
        {STEPS.map((s, i) => (
          <li key={s} className={i === step ? 'on' : i < step ? 'done' : ''}>{s}</li>
        ))}
      </ol>

      <div className="wiz-body fade-up" key={step}>
        {step === 0 && (
          <div className="card wiz-card">
            <h1>Welcome, {user?.firstName || 'there'} 👋</h1>
            <p className="muted">
              You're in — no payment needed. A complete profile speeds up exam eligibility later and
              takes about three minutes. Everything saves as you go, and you can finish any time from
              your Profile page.
            </p>
            <ul className="steps" style={{ margin: '1rem 0' }}>
              <li><span className="dot">1</span><span><span className="label">Tell us about yourself</span><div className="detail">Role, sector and where you work</div></span></li>
              <li><span className="dot">2</span><span><span className="label">Add your work experience</span><div className="detail">As many roles as you like — overlaps are counted once</div></span></li>
              <li><span className="dot">3</span><span><span className="label">Add qualifications & certifications</span><div className="detail">Degrees plus any credentials you already hold</div></span></li>
            </ul>
            <button className="btn" onClick={() => setStep(1)}>Let's go</button>
          </div>
        )}

        {step === 1 && (
          <div className="card wiz-card">
            <h2>About you</h2>
            <p className="muted small">All fields optional — the more you add, the more complete your profile.</p>
            {err && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{err}</div>}
            <form onSubmit={saveAbout}>
              <div className="grid cols-2">
                {ABOUT_FIELDS.map((f) => (
                  <div className="field" key={f.key}>
                    <label htmlFor={'ob_' + f.key}>{f.label}</label>
                    {f.key === 'country' ? (
                      <select id="ob_country" value={val('country')} onChange={(e) => setForm({ ...form, country: e.target.value })}>
                        <option value="">Select your country…</option>
                        {COUNTRIES.map((c) => (
                          <option key={c.iso} value={c.name}>{c.name}</option>
                        ))}
                      </select>
                    ) : (
                      <input id={'ob_' + f.key} type={f.type ?? 'text'} value={val(f.key)} onChange={(e) => setForm({ ...form, [f.key]: e.target.value })} />
                    )}
                  </div>
                ))}
              </div>
              <div className="row">
                <button className="btn" disabled={busy}>{busy ? 'Saving…' : 'Save & continue'}</button>
                <button className="btn ghost" type="button" onClick={() => setStep(2)}>Skip</button>
              </div>
            </form>
          </div>
        )}

        {step === 2 && (
          <div className="card wiz-card">
            <RepeaterSection<Experience>
              title="Work experience"
              blurb="Add each role separately — multiple companies welcome. Overlapping months count once."
              route="/api/me/experiences"
              fields={EXPERIENCE_FIELDS}
              addLabel="Add experience"
              emptyHint="No roles added yet. Your experience record supports exam eligibility and your public credential later."
              itemTitle={(r) => `${r.title} — ${r.company}`}
              itemSub={(r) => [r.start_date, r.is_current ? 'present' : r.end_date].filter(Boolean).join(' → ') + (r.country ? ` · ${r.country}` : '')}
              itemBody={(r) => r.summary}
              footer={(rows) => {
                const months = monthsCovered(rows)
                return months > 0 ? (
                  <div className="notice" style={{ marginTop: '1rem' }}>
                    <strong>{Math.floor(months / 12)}y {months % 12}m</strong> of experience recorded across {rows.length} role{rows.length === 1 ? '' : 's'} (overlaps counted once).
                  </div>
                ) : null
              }}
            />
            <div className="row" style={{ marginTop: '1.25rem' }}>
              <button className="btn" onClick={() => setStep(3)}>Continue</button>
              <button className="btn ghost" onClick={() => setStep(1)}>Back</button>
            </div>
          </div>
        )}

        {step === 3 && (
          <div className="card wiz-card">
            <RepeaterSection<Qualification>
              title="Qualifications"
              blurb="Degrees, diplomas and other academic awards."
              route="/api/me/qualifications"
              fields={QUALIFICATION_FIELDS}
              addLabel="Add qualification"
              emptyHint="No qualifications added yet."
              itemTitle={(r) => r.degree}
              itemSub={(r) => [r.institution, r.year_completed, r.country].filter(Boolean).join(' · ')}
            />
            <div className="row" style={{ marginTop: '1.25rem' }}>
              <button className="btn" onClick={() => setStep(4)}>Continue</button>
              <button className="btn ghost" onClick={() => setStep(2)}>Back</button>
            </div>
          </div>
        )}

        {step === 4 && (
          <div className="card wiz-card">
            <RepeaterSection<HeldCertification>
              title="Certifications you already hold"
              blurb="PMP, CCP, PSP, PRINCE2 — anything from another body."
              route="/api/me/certifications-held"
              fields={HELD_CERT_FIELDS}
              addLabel="Add certification"
              emptyHint="No certifications added yet — that's fine, most candidates start here."
              itemTitle={(r) => r.name}
              itemSub={(r) => [r.issuer, r.issued_year && `since ${r.issued_year}`].filter(Boolean).join(' · ')}
            />
            <div className="row" style={{ marginTop: '1.25rem' }}>
              <button className="btn" onClick={() => { refetch(); setStep(5) }}>Continue</button>
              <button className="btn ghost" onClick={() => setStep(3)}>Back</button>
            </div>
          </div>
        )}

        {step === 5 && (
          <div className="card wiz-card" style={{ textAlign: 'center' }}>
            <div style={{ display: 'flex', justifyContent: 'center', margin: '0 0 1rem' }}>
              <Ring value={completion} label="complete" />
            </div>
            <h2>Your profile is live</h2>
            <p className="muted">
              You can edit everything from your Profile page. When you're ready, activate your
              membership or explore the certification catalogue — your call, your pace.
            </p>
            <div className="row" style={{ justifyContent: 'center', flexWrap: 'wrap' }}>
              <button className="btn" onClick={() => nav('/')}>Go to my dashboard</button>
              <Link className="btn secondary" to="/certifications">Explore certifications</Link>
            </div>
          </div>
        )}
      </div>
    </div>
  )
}
