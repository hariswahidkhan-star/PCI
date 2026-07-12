import { useState, type FormEvent } from 'react'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import GoogleButton from '../components/GoogleButton'
import AuthShell from '../components/AuthShell'

/** Free account creation — no payment required. Students build their profile first and pay
 * membership or exam fees whenever they choose (from the site or the portal). */
export default function Register() {
  const { register } = useAuth()
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
  const [form, setForm] = useState({ firstName: '', lastName: '', email: '', password: '', country: '' })
  const [error, setError] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  const set = (k: keyof typeof form) => (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm({ ...form, [k]: e.target.value })

  async function submit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    if (form.password.length < 8) {
      setError('Password must be at least 8 characters.')
      return
    }
    setBusy(true)
    try {
      await register({ ...form, email: form.email.trim().toLowerCase() })
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
          <img src="/assets/logo.png" alt="PCI Global" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <h1 style={{ fontSize: '1.25rem', marginTop: '.5rem' }}>Create your free account</h1>
          <p className="muted small">
            Join in under a minute — build your profile now, pay only when you choose to enrol.
          </p>
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
              <label htmlFor="firstName">First name</label>
              <input id="firstName" autoComplete="given-name" required value={form.firstName} onChange={set('firstName')} />
            </div>
            <div className="field">
              <label htmlFor="lastName">Last name</label>
              <input id="lastName" autoComplete="family-name" required value={form.lastName} onChange={set('lastName')} />
            </div>
          </div>
          <div className="field">
            <label htmlFor="remail">Email address</label>
            <input id="remail" type="email" autoComplete="email" required value={form.email} onChange={set('email')} />
          </div>
          <div className="field">
            <label htmlFor="rpassword">Password (8+ characters)</label>
            <input id="rpassword" type="password" autoComplete="new-password" required minLength={8} value={form.password} onChange={set('password')} />
          </div>
          <div className="field">
            <label htmlFor="rcountry">Country (optional)</label>
            <input id="rcountry" autoComplete="country-name" value={form.country} onChange={set('country')} />
          </div>
          <button className="btn block" type="submit" disabled={busy}>
            {busy ? 'Creating account…' : 'Create free account'}
          </button>
          <p className="muted small" style={{ marginTop: '.75rem', marginBottom: 0 }}>
            By creating an account you agree to our <a href="/terms.html" target="_blank" rel="noreferrer">terms</a>,{' '}
            <a href="/terms-of-enrollment.html" target="_blank" rel="noreferrer">terms of enrolment</a> and{' '}
            <a href="/privacy.html" target="_blank" rel="noreferrer">privacy policy</a>.
          </p>
        </form>
        <div className="spread small" style={{ marginTop: '1rem' }}>
          <span className="muted">Already have an account?</span>
          <Link to="/login">Sign in</Link>
        </div>
      </div>
    </AuthShell>
  )
}
