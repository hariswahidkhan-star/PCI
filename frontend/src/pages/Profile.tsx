import { useState, type FormEvent } from 'react'
import { Link } from 'react-router-dom'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote } from '../components/ui'
import Ring from '../components/Ring'
import RepeaterSection from '../components/RepeaterSection'
import { EXPERIENCE_FIELDS, QUALIFICATION_FIELDS, HELD_CERT_FIELDS, monthsCovered } from '../data/wizardFields'
import { fmtDate } from '../format'
import { useT } from '../i18n'
import type { Experience, Qualification, HeldCertification } from '../api/types'

// Mirrors the allow-list in PATCH /api/me/profile (backend). `labelKey` resolves via the i18n catalog.
const FIELDS: { key: string; labelKey: string; type?: string }[] = [
  { key: 'mobile', labelKey: 'prof.mobile' },
  { key: 'country', labelKey: 'prof.country' },
  { key: 'city', labelKey: 'prof.city' },
  { key: 'preferred_language', labelKey: 'prof.preferredLanguage' },
  { key: 'current_role', labelKey: 'prof.currentRole' },
  { key: 'company', labelKey: 'prof.company' },
  { key: 'industry_sector', labelKey: 'prof.industrySector' },
  { key: 'years_experience', labelKey: 'prof.yearsExperience', type: 'number' },
  { key: 'highest_qualification', labelKey: 'prof.highestQualification' },
  { key: 'project_controls_area', labelKey: 'prof.projectControlsArea' },
  { key: 'linkedin_url', labelKey: 'prof.linkedinUrl', type: 'url' },
]

export default function Profile() {
  const t = useT()
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
      setErr(e2 instanceof Error ? e2.message : t('prof.saveError'))
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread" style={{ alignItems: 'flex-start', flexWrap: 'wrap', gap: '1rem' }}>
        <div>
          <h1>{t('prof.title')}</h1>
          <p className="muted" style={{ marginBottom: 0 }}>
            {t('prof.subtitle')}{' '}
            <Link to="/onboarding">{t('prof.runGuidedSetup')}</Link>
          </p>
        </div>
        <Ring value={completion} label={t('prof.complete')} />
      </div>

      <Card title={t('prof.account')}>
        <div className="grid cols-2 small">
          <div><span className="muted">{t('prof.name')}</span><div><strong>{me.user.first_name} {me.user.last_name}</strong></div></div>
          <div><span className="muted">{t('prof.email')}</span><div>{me.user.email}</div></div>
          <div><span className="muted">{t('prof.registrationNo')}</span><div>{me.user.registration_no}</div></div>
          <div><span className="muted">{t('prof.memberSince')}</span><div>{fmtDate(me.user.created_at)}</div></div>
        </div>
        <p className="muted small" style={{ marginTop: '.75rem', marginBottom: 0 }}>
          {t('prof.changeNameEmailPre')}<a href="/contact.html">{t('prof.contactSupport')}</a>{t('prof.changeNameEmailPost')}
        </p>
      </Card>

      <Card title={t('prof.details')}>
        {saved && <div className="notice" style={{ marginBottom: '.75rem' }}>{t('prof.saved')}</div>}
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{err}</div>}
        <form onSubmit={save}>
          <div className="grid cols-2">
            {FIELDS.map((f) => (
              <div className="field" key={f.key}>
                <label htmlFor={f.key}>{t(f.labelKey)}</label>
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
            {busy ? t('prof.saving') : t('prof.saveChanges')}
          </button>
        </form>
      </Card>

      <Card>
        <RepeaterSection<Experience>
          title={t('prof.workExperience')}
          blurb={t('prof.workExperienceBlurb')}
          route="/api/me/experiences"
          fields={EXPERIENCE_FIELDS}
          addLabel={t('prof.addExperience')}
          emptyHint={t('prof.noExperience')}
          itemTitle={(r) => `${r.title} — ${r.company}`}
          itemSub={(r) => [r.start_date, r.is_current ? t('prof.present') : r.end_date].filter(Boolean).join(' → ') + (r.country ? ` · ${r.country}` : '')}
          itemBody={(r) => r.summary}
          footer={(rows) => {
            const months = monthsCovered(rows)
            return months > 0 ? (
              <p className="muted small" style={{ margin: '.75rem 0 0' }}>
                {t('prof.totalRecorded')} <strong>{t('prof.yearsMonths', { y: Math.floor(months / 12), m: months % 12 })}</strong> {t('prof.acrossRoles', { count: rows.length })}
              </p>
            ) : null
          }}
        />
      </Card>

      <Card>
        <RepeaterSection<Qualification>
          title={t('prof.qualifications')}
          blurb={t('prof.qualificationsBlurb')}
          route="/api/me/qualifications"
          fields={QUALIFICATION_FIELDS}
          addLabel={t('prof.addQualification')}
          emptyHint={t('prof.noQualifications')}
          itemTitle={(r) => r.degree}
          itemSub={(r) => [r.institution, r.year_completed, r.country].filter(Boolean).join(' · ')}
        />
      </Card>

      <Card>
        <RepeaterSection<HeldCertification>
          title={t('prof.certsHeld')}
          blurb={t('prof.certsHeldBlurb')}
          route="/api/me/certifications-held"
          fields={HELD_CERT_FIELDS}
          addLabel={t('prof.addCertification')}
          emptyHint={t('prof.noCertsHeld')}
          itemTitle={(r) => r.name}
          itemSub={(r) => [r.issuer, r.issued_year && t('prof.since', { year: r.issued_year }), r.expires_year && t('prof.expires', { year: r.expires_year })].filter(Boolean).join(' · ')}
        />
      </Card>
    </div>
  )
}
