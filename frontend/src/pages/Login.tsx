import { useState, type FormEvent } from 'react'
import { Link, useNavigate, useLocation, useSearchParams } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import GoogleButton from '../components/GoogleButton'
import AuthShell from '../components/AuthShell'
import { useT } from '../i18n'

export default function Login() {
  const { login, notice } = useAuth()
  const t = useT()
  const nav = useNavigate()
  const loc = useLocation() as { state?: { from?: string } }
  const [params] = useSearchParams()
  // Preserve a founding/exam deep-link through sign-in, so an existing-account invitee who followed
  // "Redeem your founding code" isn't dropped on the dashboard with the code silently lost.
  const founding = params.get('founding') ?? ''
  const product = params.get('product')
  const cert = params.get('cert')
  const deepDest = params.has('founding')
    ? `/billing?founding=${encodeURIComponent(founding)}`
    : product
      ? `/billing?product=${encodeURIComponent(product)}${cert ? `&cert=${encodeURIComponent(cert)}` : ''}`
      : null
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [totp, setTotp] = useState('')
  const [needTotp, setNeedTotp] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  async function submit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    setBusy(true)
    try {
      await login(email.trim().toLowerCase(), password, needTotp ? totp.trim() : undefined)
      nav(deepDest ?? loc.state?.from ?? '/', { replace: true })
    } catch (err) {
      const code = err && typeof err === 'object' && 'body' in err ? (err as { body?: { error?: string } }).body?.error : undefined
      if (code === 'totp_required' || code === 'totp_invalid') {
        setNeedTotp(true)
        setError(code === 'totp_invalid' ? 'That code was not valid. Try again, or use a recovery code.' : null)
      } else {
        setError(err instanceof Error ? err.message : t('auth.unableSignIn'))
      }
    } finally {
      setBusy(false)
    }
  }

  return (
    <AuthShell>
      <div className="card login-card fade-up">
        <div className="logo">
          <img src="/assets/logo.png" alt="PCI AI" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <h1 style={{ fontSize: '1.25rem', marginTop: '.5rem' }}>{t('shell.studentPortal')}</h1>
          <p className="muted small">{t('auth.signInSubtitle')}</p>
        </div>

        <GoogleButton onError={setError} destination={deepDest ?? '/'} />

        <form onSubmit={submit}>
          {/* Gentle, informational note (e.g. an expired World handoff link) — never an error wall. */}
          {notice && !error && <div className="notice" role="status" style={{ marginBottom: '1rem' }}>{notice}</div>}
          {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
          <div className="field">
            <label htmlFor="email">{t('auth.email')}</label>
            <input id="email" type="email" autoComplete="username" required value={email} onChange={(e) => setEmail(e.target.value)} />
          </div>
          <div className="field">
            <label htmlFor="password">{t('auth.password')}</label>
            <input id="password" type="password" autoComplete="current-password" required value={password} onChange={(e) => setPassword(e.target.value)} />
          </div>
          {needTotp && (
            <div className="field">
              <label htmlFor="totp">Authentication code</label>
              <input id="totp" inputMode="numeric" autoComplete="one-time-code" autoFocus placeholder="6-digit code or recovery code"
                value={totp} onChange={(e) => setTotp(e.target.value)} />
              <p className="muted small" style={{ marginTop: '.3rem' }}>Enter the code from your authenticator app, or one of your recovery codes.</p>
            </div>
          )}
          <button className="btn block" type="submit" disabled={busy}>
            {busy ? t('auth.signingIn') : needTotp ? 'Verify & sign in' : t('auth.signIn')}
          </button>
        </form>
        <div className="spread small" style={{ marginTop: '1rem' }}>
          <a href="/forgot-password.html">{t('auth.forgot')}</a>
          <Link to="/register">{t('auth.createAccount')}</Link>
        </div>
      </div>
    </AuthShell>
  )
}
