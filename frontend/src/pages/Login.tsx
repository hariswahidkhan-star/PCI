import { useState, type FormEvent } from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

export default function Login() {
  const { login } = useAuth()
  const nav = useNavigate()
  const loc = useLocation() as { state?: { from?: string } }
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
      nav(loc.state?.from ?? '/', { replace: true })
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unable to sign in.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="center-page">
      <div className="card login-card">
        <div className="logo">
          <img src="/assets/logo.png" alt="Project Controls Institute" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <h1 style={{ fontSize: '1.25rem', marginTop: '.5rem' }}>Student Portal</h1>
          <p className="muted small">Sign in to manage your certification journey.</p>
        </div>
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
          <a href="/enroll.html">Create an account</a>
        </div>
      </div>
    </div>
  )
}
