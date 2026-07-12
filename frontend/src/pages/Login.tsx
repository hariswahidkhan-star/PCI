import { useState, type FormEvent } from 'react'
import { Link, useNavigate, useLocation, useSearchParams } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import GoogleButton from '../components/GoogleButton'
import AuthShell from '../components/AuthShell'

export default function Login() {
  const { login } = useAuth()
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
  const [error, setError] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  async function submit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    setBusy(true)
    try {
      await login(email.trim().toLowerCase(), password)
      nav(deepDest ?? loc.state?.from ?? '/', { replace: true })
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to sign in.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <AuthShell>
      <div className="card login-card fade-up">
        <div className="logo">
          <img src="/assets/logo.png" alt="PCI Global" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <h1 style={{ fontSize: '1.25rem', marginTop: '.5rem' }}>Student Portal</h1>
          <p className="muted small">Sign in to manage your certification journey.</p>
        </div>

        <GoogleButton onError={setError} destination={deepDest ?? '/'} />

        <form onSubmit={submit}>
          {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
          <div className="field">
            <label htmlFor="email">Email address</label>
            <input id="email" type="email" autoComplete="username" required value={email} onChange={(e) => setEmail(e.target.value)} />
          </div>
          <div className="field">
            <label htmlFor="password">Password</label>
            <input id="password" type="password" autoComplete="current-password" required value={password} onChange={(e) => setPassword(e.target.value)} />
          </div>
          <button className="btn block" type="submit" disabled={busy}>
            {busy ? 'Signing in…' : 'Sign in'}
          </button>
        </form>
        <div className="spread small" style={{ marginTop: '1rem' }}>
          <a href="/forgot-password.html">Forgot password?</a>
          <Link to="/register">Create a free account</Link>
        </div>
      </div>
    </AuthShell>
  )
}
