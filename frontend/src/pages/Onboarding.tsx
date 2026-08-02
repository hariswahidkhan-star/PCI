import { useState, type FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { api } from '../api/client'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { useT } from '../i18n'
import RepeaterSection from '../components/RepeaterSection'
import Ring from '../components/Ring'
import { EXPERIENCE_FIELDS, QUALIFICATION_FIELDS, HELD_CERT_FIELDS, monthsCovered } from '../data/wizardFields'
import { COUNTRIES } from '../data/countries'
import type { Experience, Qualification, HeldCertification } from '../api/types'

const STEPS = ['Welcome', 'About you', 'Experience', 'Qualifications', 'Certifications', 'Done']

// Maps each STEPS entry to its translation key; STEPS values stay as the source/React keys.
const STEP_KEYS: Record<string, string> = {
  Welcome: 'onb.step.welcome',
  'About you': 'onb.step.about',
  Experience: 'onb.step.experience',
  Qualifications: 'onb.step.qualifications',
  Certifications: 'onb.step.certifications',
  Done: 'onb.step.done',
}

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
  const t = useT()
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
      setErr(e2 instanceof Error ? e2.message : t('onb.saveError'))
    } finally {
      setBusy(false)
    }
  }

  const val = (k: string) => (k in form ? form[k] : String(profile[k] ?? ''))

  return (
    <div className="wizpage">
      <header className="wiz-head">
        <div className="row">
          <img src="/assets/logo.png" alt="PCI AI" style={{ height: 30, borderRadius: 7 }} onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <strong>{t('onb.headerTitle')}</strong>
        </div>
        <Link className="small muted" to="/">{t('onb.skipForNow')}</Link>
      </header>

      <div className="wiz-progress" role="progressbar" aria-valuenow={step + 1} aria-valuemin={1} aria-valuemax={STEPS.length}>
        <i style={{ width: `${((step + 1) / STEPS.length) * 100}%` }} />
      </div>
      <ol className="wiz-steps small">
        {STEPS.map((s, i) => (
          <li key={s} className={i === step ? 'on' : i < step ? 'done' : ''}>{t(STEP_KEYS[s])}</li>
        ))}
      </ol>

      <div className="wiz-body fade-up" key={step}>
        {step === 0 && (
          <div className="card wiz-card">
            <h1>{user?.firstName ? t('onb.welcome', { name: user.firstName }) : t('onb.welcomeNoName')}</h1>
            <p className="muted">{t('onb.welcomeBlurb')}</p>
            <ul className="steps" style={{ margin: '1rem 0' }}>
              <li><span className="dot">1</span><span><span className="label">{t('onb.welcomeStep1Label')}</span><div className="detail">{t('onb.welcomeStep1Detail')}</div></span></li>
              <li><span className="dot">2</span><span><span className="label">{t('onb.welcomeStep2Label')}</span><div className="detail">{t('onb.welcomeStep2Detail')}</div></span></li>
              <li><span className="dot">3</span><span><span className="label">{t('onb.welcomeStep3Label')}</span><div className="detail">{t('onb.welcomeStep3Detail')}</div></span></li>
            </ul>
            <button className="btn" onClick={() => setStep(1)}>{t('onb.letsGo')}</button>
          </div>
        )}

        {step === 1 && (
          <div className="card wiz-card">
            <h2>{t('onb.aboutTitle')}</h2>
            <p className="muted small">{t('onb.aboutSubtitle')}</p>
            {err && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{err}</div>}
            <form onSubmit={saveAbout}>
              <div className="grid cols-2">
                {ABOUT_FIELDS.map((f) => (
                  <div className="field" key={f.key}>
                    <label htmlFor={'ob_' + f.key}>{t('onb.field.' + f.key)}</label>
                    {f.key === 'country' ? (
                      <select id="ob_country" value={val('country')} onChange={(e) => setForm({ ...form, country: e.target.value })}>
                        <option value="">{t('onb.selectCountry')}</option>
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
                <button className="btn" disabled={busy}>{busy ? t('onb.saving') : t('onb.saveContinue')}</button>
                <button className="btn ghost" type="button" onClick={() => setStep(2)}>{t('onb.skip')}</button>
              </div>
            </form>
          </div>
        )}

        {step === 2 && (
          <div className="card wiz-card">
            <RepeaterSection<Experience>
              title={t('onb.expTitle')}
              blurb={t('onb.expBlurb')}
              route="/api/me/experiences"
              fields={EXPERIENCE_FIELDS}
              addLabel={t('onb.expAdd')}
              emptyHint={t('onb.expEmpty')}
              itemTitle={(r) => `${r.title} — ${r.company}`}
              itemSub={(r) => [r.start_date, r.is_current ? t('onb.present') : r.end_date].filter(Boolean).join(' → ') + (r.country ? ` · ${r.country}` : '')}
              itemBody={(r) => r.summary}
              footer={(rows) => {
                const months = monthsCovered(rows)
                return months > 0 ? (
                  <div className="notice" style={{ marginTop: '1rem' }}>
                    <strong>{t('onb.expDuration', { years: Math.floor(months / 12), months: months % 12 })}</strong>
                    {t(rows.length === 1 ? 'onb.expFooterOne' : 'onb.expFooterMany', { count: rows.length })}
                  </div>
                ) : null
              }}
            />
            <div className="row" style={{ marginTop: '1.25rem' }}>
              <button className="btn" onClick={() => setStep(3)}>{t('onb.continue')}</button>
              <button className="btn ghost" onClick={() => setStep(1)}>{t('onb.back')}</button>
            </div>
          </div>
        )}

        {step === 3 && (
          <div className="card wiz-card">
            <RepeaterSection<Qualification>
              title={t('onb.qualTitle')}
              blurb={t('onb.qualBlurb')}
              route="/api/me/qualifications"
              fields={QUALIFICATION_FIELDS}
              addLabel={t('onb.qualAdd')}
              emptyHint={t('onb.qualEmpty')}
              itemTitle={(r) => r.degree}
              itemSub={(r) => [r.institution, r.year_completed, r.country].filter(Boolean).join(' · ')}
            />
            <div className="row" style={{ marginTop: '1.25rem' }}>
              <button className="btn" onClick={() => setStep(4)}>{t('onb.continue')}</button>
              <button className="btn ghost" onClick={() => setStep(2)}>{t('onb.back')}</button>
            </div>
          </div>
        )}

        {step === 4 && (
          <div className="card wiz-card">
            <RepeaterSection<HeldCertification>
              title={t('onb.certTitle')}
              blurb={t('onb.certBlurb')}
              route="/api/me/certifications-held"
              fields={HELD_CERT_FIELDS}
              addLabel={t('onb.certAdd')}
              emptyHint={t('onb.certEmpty')}
              itemTitle={(r) => r.name}
              itemSub={(r) => [r.issuer, r.issued_year && t('onb.since', { year: r.issued_year })].filter(Boolean).join(' · ')}
            />
            <div className="row" style={{ marginTop: '1.25rem' }}>
              <button className="btn" onClick={() => { refetch(); setStep(5) }}>{t('onb.continue')}</button>
              <button className="btn ghost" onClick={() => setStep(3)}>{t('onb.back')}</button>
            </div>
          </div>
        )}

        {step === 5 && (
          <div className="card wiz-card" style={{ textAlign: 'center' }}>
            <div style={{ display: 'flex', justifyContent: 'center', margin: '0 0 1rem' }}>
              <Ring value={completion} label={t('onb.ringComplete')} />
            </div>
            <h2>{t('onb.doneTitle')}</h2>
            <p className="muted">{t('onb.doneBlurb')}</p>
            <div className="row" style={{ justifyContent: 'center', flexWrap: 'wrap' }}>
              <button className="btn" onClick={() => nav('/')}>{t('onb.goDashboard')}</button>
              <Link className="btn secondary" to="/certifications">{t('onb.exploreCerts')}</Link>
            </div>
          </div>
        )}
      </div>
    </div>
  )
}
