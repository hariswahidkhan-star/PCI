import { useState, useEffect, type FormEvent } from 'react'
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

      <DirectorySettings />
      <TwoFactorCard />
      <CommPreferences />
    </div>
  )
}

// Opt-in public member directory — a certified member can list themselves in the public "find a professional"
// registry and control exactly which fields are shown.
function DirectorySettings() {
  const [d, setD] = useState<{ eligible: boolean; opt_in: boolean; headline?: string | null; show_country: boolean; show_org: boolean; show_linkedin: boolean } | null>(null)
  const [busy, setBusy] = useState(false)
  const [saved, setSaved] = useState(false)

  useEffect(() => {
    let live = true
    api.get<typeof d>('/api/me/directory').then((r) => { if (live) setD(r) }).catch(() => {})
    return () => { live = false }
  }, [])

  if (!d) return null
  const set = <K extends keyof NonNullable<typeof d>>(k: K, v: NonNullable<typeof d>[K]) => { setD((p) => (p ? { ...p, [k]: v } : p)); setSaved(false) }
  async function save() {
    if (!d) return
    setBusy(true); setSaved(false)
    try { await api.post('/api/me/directory', { opt_in: d.opt_in, headline: d.headline ?? '', show_country: d.show_country, show_org: d.show_org, show_linkedin: d.show_linkedin }); setSaved(true) }
    finally { setBusy(false) }
  }

  return (
    <Card title="Public member directory">
      <p className="muted small" style={{ marginTop: 0 }}>
        List yourself in PCI's public <a href="/directory.html" target="_blank" rel="noreferrer">find-a-professional</a> directory.
        You choose to appear and exactly what is shown — your email is never published. You appear only while you hold an active credential.
      </p>
      <label className="row" style={{ alignItems: 'center', gap: '.5rem', fontWeight: 600 }}>
        <input type="checkbox" style={{ width: 'auto' }} checked={d.opt_in} onChange={(e) => set('opt_in', e.target.checked)} />
        List me in the public directory
      </label>
      {!d.eligible && d.opt_in && <p className="small" style={{ color: 'var(--warn,#92400e)', margin: '.4rem 0 0' }}>You'll appear once you hold an active credential.</p>}
      {d.opt_in && (
        <div className="stack" style={{ display: 'grid', gap: '.5rem', marginTop: '.7rem' }}>
          <label className="field"><span>Headline (optional)</span>
            <input value={d.headline ?? ''} maxLength={160} onChange={(e) => set('headline', e.target.value)} placeholder="Senior Planning Engineer, rail & infrastructure" />
          </label>
          <label className="row" style={{ alignItems: 'center', gap: '.5rem' }}><input type="checkbox" style={{ width: 'auto' }} checked={d.show_country} onChange={(e) => set('show_country', e.target.checked)} /> Show my country</label>
          <label className="row" style={{ alignItems: 'center', gap: '.5rem' }}><input type="checkbox" style={{ width: 'auto' }} checked={d.show_org} onChange={(e) => set('show_org', e.target.checked)} /> Show my role &amp; employer (when no headline set)</label>
          <label className="row" style={{ alignItems: 'center', gap: '.5rem' }}><input type="checkbox" style={{ width: 'auto' }} checked={d.show_linkedin} onChange={(e) => set('show_linkedin', e.target.checked)} /> Show my LinkedIn link</label>
        </div>
      )}
      <div className="row" style={{ marginTop: '.8rem', alignItems: 'center', gap: '.6rem' }}>
        <button className="btn sm" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save directory settings'}</button>
        {saved && <span className="small muted">Saved.</span>}
      </div>
    </Card>
  )
}

// Two-factor authentication (TOTP) self-enrolment: setup → scan/enter secret → verify → recovery codes.
function TwoFactorCard() {
  const [status, setStatus] = useState<{ enabled: boolean; pending: boolean; recovery_remaining: number } | null>(null)
  const [secret, setSecret] = useState<string | null>(null)
  const [otpauth, setOtpauth] = useState('')
  const [code, setCode] = useState('')
  const [recovery, setRecovery] = useState<string[] | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const reload = () => api.get<{ enabled: boolean; pending: boolean; recovery_remaining: number }>('/api/me/2fa').then(setStatus).catch(() => setStatus({ enabled: false, pending: false, recovery_remaining: 0 }))
  useEffect(() => { reload() }, [])
  async function begin() {
    setBusy(true); setErr(null)
    try { const r = await api.post<{ secret: string; otpauth: string }>('/api/me/2fa/setup', {}); setSecret(r.secret); setOtpauth(r.otpauth) }
    catch (e) { setErr(e instanceof Error ? e.message : 'Could not start setup.') } finally { setBusy(false) }
  }
  async function verify() {
    setBusy(true); setErr(null)
    try { const r = await api.post<{ recovery_codes: string[] }>('/api/me/2fa/verify', { code: code.trim() }); setRecovery(r.recovery_codes); setSecret(null); setCode(''); await reload() }
    catch (e) { const c = e && typeof e === 'object' && 'body' in e ? (e as { body?: { message?: string } }).body?.message : undefined; setErr(c || 'That code was not valid.') } finally { setBusy(false) }
  }
  async function disable() {
    const c = prompt('Enter a current authenticator code (or a recovery code) to turn off two-factor authentication:')
    if (c === null) return
    setBusy(true); setErr(null)
    try { await api.post('/api/me/2fa/disable', { code: c.trim() }); setRecovery(null); await reload() }
    catch (e) { const m = e && typeof e === 'object' && 'body' in e ? (e as { body?: { message?: string } }).body?.message : undefined; setErr(m || 'Could not disable 2FA.') } finally { setBusy(false) }
  }
  if (!status) return <Card title="Two-factor authentication"><Spinner /></Card>
  return (
    <Card title="Two-factor authentication (2FA)">
      <p className="muted small" style={{ marginTop: 0 }}>
        Add a second step at sign-in using an authenticator app (Google Authenticator, Authy, 1Password, …).
        We never send codes by email or message — they come only from your app.
      </p>
      {err && <ErrorNote>{err}</ErrorNote>}

      {recovery && (
        <div style={{ background: '#ecfdf5', border: '1px solid #a7f3d0', borderRadius: 8, padding: '.8rem', marginBottom: '.8rem' }}>
          <strong>2FA is on. Save your recovery codes now.</strong>
          <p className="muted small" style={{ margin: '.3rem 0' }}>Each code works once if you lose your authenticator. Store them somewhere safe — they won't be shown again.</p>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(120px,1fr))', gap: '.3rem', fontFamily: 'monospace' }}>
            {recovery.map((c) => <span key={c}>{c}</span>)}
          </div>
        </div>
      )}

      {status.enabled && !recovery && (
        <div>
          <p><span className="badge ok">Enabled</span> <span className="muted small">Recovery codes remaining: {status.recovery_remaining}</span></p>
          <button className="btn secondary" disabled={busy} onClick={disable}>Turn off 2FA</button>
        </div>
      )}

      {!status.enabled && !secret && !recovery && (
        <button className="btn" disabled={busy} onClick={begin}>{busy ? 'Starting…' : 'Set up 2FA'}</button>
      )}

      {secret && (
        <div style={{ display: 'grid', gap: '.6rem' }}>
          <p className="small" style={{ margin: 0 }}>1. Add this key to your authenticator app (or paste the setup URL):</p>
          <code style={{ background: 'var(--surface-2,#f1f5f9)', padding: '.5rem .7rem', borderRadius: 6, wordBreak: 'break-all' }}>{secret}</code>
          <details><summary className="small muted">Show setup URL (otpauth://)</summary><code className="small" style={{ wordBreak: 'break-all' }}>{otpauth}</code></details>
          <p className="small" style={{ margin: 0 }}>2. Enter the 6-digit code your app shows:</p>
          <input inputMode="numeric" autoComplete="one-time-code" placeholder="6-digit code" value={code} onChange={(e) => setCode(e.target.value)} style={{ maxWidth: 200 }} />
          <div className="row" style={{ gap: '.5rem' }}>
            <button className="btn" disabled={busy || code.trim().length < 6} onClick={verify}>{busy ? 'Verifying…' : 'Verify & enable'}</button>
            <button className="btn ghost" disabled={busy} onClick={() => { setSecret(null); setCode(''); setErr(null) }}>Cancel</button>
          </div>
        </div>
      )}
    </Card>
  )
}

// Optional-communications consent. Essential communications (security, decisions, payments, exams,
// certificates, privacy, critical operations) are always sent and are deliberately not listed here.
function CommPreferences() {
  const [prefs, setPrefs] = useState<Record<string, number> | null>(null)
  const [loading, setLoading] = useState(true)
  const [busy, setBusy] = useState(false)
  const [saved, setSaved] = useState(false)
  useEffect(() => {
    let live = true
    api.get<{ preferences: Record<string, number> }>('/api/me/preferences')
      .then((d) => { if (live) setPrefs(d.preferences) }).catch(() => { if (live) setPrefs({}) }).finally(() => { if (live) setLoading(false) })
    return () => { live = false }
  }, [])
  const toggle = (k: string) => setPrefs((p) => ({ ...(p ?? {}), [k]: p?.[k] ? 0 : 1 }))
  async function save() {
    if (!prefs) return
    setBusy(true); setSaved(false)
    try { await api.post('/api/me/preferences', prefs); setSaved(true) } finally { setBusy(false) }
  }
  const OPTIONS: { key: string; label: string; hint: string }[] = [
    { key: 'email_marketing', label: 'Marketing email', hint: 'News about certifications, offers and events by email.' },
    { key: 'whatsapp_marketing', label: 'Marketing WhatsApp', hint: 'Occasional updates by WhatsApp (requires opt-in below).' },
    { key: 'whatsapp_optin', label: 'WhatsApp opt-in', hint: 'Allow PCI to contact you on WhatsApp at all.' },
    { key: 'newsletter', label: 'Newsletter', hint: 'The periodic PCI newsletter.' },
    { key: 'events', label: 'Events', hint: 'Invitations to webinars and events.' },
    { key: 'surveys', label: 'Surveys', hint: 'Occasional feedback surveys.' },
  ]
  return (
    <Card title="Communication preferences">
      <p className="muted small" style={{ marginTop: 0 }}>
        Choose which <strong>optional</strong> messages you receive. Essential communications — account security,
        application decisions, payment confirmations, exam scheduling and results, certificates and privacy
        requests — are always sent and cannot be switched off.
      </p>
      {loading || !prefs ? <Spinner /> : (
        <>
          <div style={{ display: 'grid', gap: '.6rem' }}>
            {OPTIONS.map((o) => (
              <label key={o.key} style={{ display: 'flex', gap: '.6rem', alignItems: 'flex-start' }}>
                <input type="checkbox" checked={!!prefs[o.key]} onChange={() => toggle(o.key)} style={{ width: 'auto', marginTop: 4 }} />
                <span><strong>{o.label}</strong><br /><span className="muted small">{o.hint}</span></span>
              </label>
            ))}
          </div>
          <div style={{ marginTop: '.8rem' }}>
            <button className="btn" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save preferences'}</button>
            {saved && <span className="muted small" style={{ marginLeft: '.6rem' }}>Saved.</span>}
          </div>
        </>
      )}
    </Card>
  )
}
