import { useState, type FormEvent } from 'react'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import GoogleButton from '../components/GoogleButton'
import AuthShell from '../components/AuthShell'
import { COUNTRIES, DIAL_CODES, dialForCountry } from '../data/countries'
import { useT } from '../i18n'

/** Free account creation — no payment required. Students build their profile first and pay
 * membership or exam fees whenever they choose (from the site or the portal). */
export default function Register() {
  const { register } = useAuth()
  const t = useT()
  const nav = useNavigate()
  const [params] = useSearchParams()
  // Deep-link intent carried from the public site so nothing is a dead-end after signup:
  //  ?founding[=CODE] → Billing to redeem the founding code (FoundingCard);
  //  ?product=exam&cert=CODE (from the certification catalogue "Enrol" CTA) → Billing to buy it.
  const foundingPresent = params.has('founding')
  const founding = params.get('founding') ?? ''
  const product = params.get('product')
  const cert = params.get('cert')
  const afterSignup = foundingPresent
    ? `/billing?founding=${encodeURIComponent(founding)}`
    : product
      ? `/billing?product=${encodeURIComponent(product)}${cert ? `&cert=${encodeURIComponent(cert)}` : ''}`
      : '/onboarding'
  const [form, setForm] = useState({ firstName: '', lastName: '', email: '', password: '', confirmPassword: '', country: '', dial: '', phone: '' })
  const [error, setError] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  const set = (k: keyof typeof form) => (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) =>
    setForm({ ...form, [k]: e.target.value })

  // Selecting a country pre-fills the matching phone dialling code (the student can still override it),
  // so the two contact fields stay consistent without extra typing.
  const setCountry = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const name = e.target.value
    const d = dialForCountry(name)
    setForm((f) => ({ ...f, country: name, dial: d || f.dial }))
  }

  async function submit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    if (form.password.length < 8) {
      setError(t('reg.min8'))
      return
    }
    if (form.password !== form.confirmPassword) {
      setError(t('reg.passwordsNoMatch'))
      return
    }
    if (!form.country) {
      setError(t('reg.selectCountryError'))
      return
    }
    if (!form.dial || !form.phone.trim()) {
      setError(t('reg.phoneError'))
      return
    }
    setBusy(true)
    try {
      const { dial, phone, confirmPassword, ...rest } = form
      const mobile = `${dial} ${phone.trim()}`.trim()
      // confirm_password is also sent so the server can re-check the match (defence in depth).
      await register({ ...rest, email: form.email.trim().toLowerCase(), mobile, confirmPassword })
      nav(afterSignup, { replace: true })
    } catch (err) {
      const msg = err instanceof Error ? err.message : ''
      setError(
        msg.includes('account_exists')
          ? 'An account with this email already exists — try signing in instead.'
          : msg || 'Unable to create your account. Please try again.',
      )
    } finally {
      setBusy(false)
    }
  }

  return (
    <AuthShell>
      <div className="card login-card fade-up">
        <div className="logo">
          <img src="/assets/logo.png" alt="PCI AI" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <h1 style={{ fontSize: '1.25rem', marginTop: '.5rem' }}>{t('reg.title')}</h1>
          <p className="muted small">{t('reg.subtitle')}</p>
        </div>

        {foundingPresent && (
          <div className="notice" style={{ marginBottom: '1rem' }}>
            {founding
              ? <>Founding code <strong>{founding.toUpperCase()}</strong> — create your free account and you will be taken straight to redeem it.</>
              : <>Have a founding code? Create your free account and you will be taken straight to the page where you can enter and redeem it.</>}
          </div>
        )}
        {!foundingPresent && product === 'exam' && (
          <div className="notice" style={{ marginBottom: '1rem' }}>
            Create your free account first — you will then be taken to pay the exam fee{cert ? <> for <strong>{cert.toUpperCase()}</strong></> : ''} securely.
          </div>
        )}

        <GoogleButton onError={setError} destination={afterSignup} />

        <form onSubmit={submit}>
          {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
          <div className="grid cols-2">
            <div className="field">
              <label htmlFor="firstName">{t('reg.firstName')}</label>
              <input id="firstName" autoComplete="given-name" required value={form.firstName} onChange={set('firstName')} />
            </div>
            <div className="field">
              <label htmlFor="lastName">{t('reg.lastName')}</label>
              <input id="lastName" autoComplete="family-name" required value={form.lastName} onChange={set('lastName')} />
            </div>
          </div>
          <div className="field">
            <label htmlFor="remail">{t('auth.email')}</label>
            <input id="remail" type="email" autoComplete="email" required value={form.email} onChange={set('email')} />
          </div>
          <div className="field">
            <label htmlFor="rpassword">{t('reg.password')}</label>
            <input id="rpassword" type="password" autoComplete="new-password" required minLength={8} value={form.password} onChange={set('password')} />
          </div>
          <div className="field">
            <label htmlFor="rconfirm">{t('reg.confirmPassword')}</label>
            <input
              id="rconfirm"
              type="password"
              autoComplete="new-password"
              required
              minLength={8}
              value={form.confirmPassword}
              onChange={set('confirmPassword')}
              aria-invalid={form.confirmPassword.length > 0 && form.password !== form.confirmPassword}
            />
            {form.confirmPassword.length > 0 && form.password !== form.confirmPassword && (
              <span className="muted small" style={{ color: 'var(--err, #dc2626)' }}>{t('reg.passwordsNoMatch')}</span>
            )}
          </div>
          <div className="field">
            <label htmlFor="rcountry">{t('reg.country')}</label>
            <select id="rcountry" autoComplete="country-name" required value={form.country} onChange={setCountry}>
              <option value="" disabled>{t('reg.selectCountry')}</option>
              {COUNTRIES.map((c) => (
                <option key={c.iso} value={c.name}>{c.name}</option>
              ))}
            </select>
          </div>
          <div className="field">
            <label htmlFor="rphone">{t('reg.contactPhone')}</label>
            <div className="row" style={{ gap: '.5rem' }}>
              <select
                aria-label={t('reg.contactPhone')}
                style={{ maxWidth: 120 }}
                required
                value={form.dial}
                onChange={set('dial')}
              >
                <option value="" disabled>{t('reg.code')}</option>
                {DIAL_CODES.map((d) => (
                  <option key={d} value={d}>{d}</option>
                ))}
              </select>
              <input
                id="rphone"
                type="tel"
                inputMode="tel"
                autoComplete="tel-national"
                required
                placeholder={t('reg.phonePlaceholder')}
                style={{ flex: 1 }}
                value={form.phone}
                onChange={set('phone')}
              />
            </div>
          </div>
          <button className="btn block" type="submit" disabled={busy}>
            {busy ? t('reg.creating') : t('reg.createBtn')}
          </button>
          <p className="muted small" style={{ marginTop: '.75rem', marginBottom: 0 }}>
            By creating an account you agree to our <a href="/terms.html" target="_blank" rel="noreferrer">terms</a>,{' '}
            <a href="/terms-of-enrollment.html" target="_blank" rel="noreferrer">terms of enrolment</a> and{' '}
            <a href="/privacy.html" target="_blank" rel="noreferrer">privacy policy</a>.
          </p>
        </form>
        <div className="spread small" style={{ marginTop: '1rem' }}>
          <span className="muted">{t('reg.haveAccount')}</span>
          <Link to="/login">{t('auth.signIn')}</Link>
        </div>
      </div>
    </AuthShell>
  )
}
