import { useState, type FormEvent } from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import { useAdminAuth } from '../AdminAuth'
import { ApiError } from '../../api/client'

export default function AdminLogin() {
  const { login } = useAdminAuth()
  const nav = useNavigate()
  const loc = useLocation() as { state?: { from?: string } }
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  // Revealed when the server answers 401 {error:'totp_required'} — the account has 2FA enrolled.
  const [needsTotp, setNeedsTotp] = useState(false)
  const [totp, setTotp] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  async function submit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    setBusy(true)
    try {
      await login(email.trim().toLowerCase(), password, needsTotp ? totp.trim() : undefined)
      nav(loc.state?.from ?? '/', { replace: true })
    } catch (err) {
      const code = err instanceof ApiError && err.body && typeof err.body === 'object' && 'error' in err.body
        ? String((err.body as Record<string, unknown>).error)
        : ''
      if (code === 'totp_required') {
        setNeedsTotp(true)
        setError('This account has two-factor authentication — enter the 6-digit code from your authenticator app.')
      } else if (code === 'totp_invalid') {
        setNeedsTotp(true)
        setError('That authentication code is not valid — check your authenticator app and try again.')
      } else {
        setError(err instanceof Error ? err.message : 'Unable to sign in.')
      }
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="center-page">
      <div className="card login-card">
        <div className="logo">
          <img src="/assets/logo.png" alt="Project Controls Institute" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <h1 style={{ fontSize: '1.25rem', marginTop: '.5rem' }}>Admin Console</h1>
          <p className="muted small">Staff sign-in.</p>
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
          {needsTotp && (
            <div className="field">
              <label htmlFor="totp">Authentication code</label>
              <input id="totp" inputMode="numeric" autoComplete="one-time-code" maxLength={8} required placeholder="123456" value={totp} onChange={(e) => setTotp(e.target.value)} />
            </div>
          )}
          <button className="btn block" type="submit" disabled={busy}>{busy ? 'Signing in…' : 'Sign in'}</button>
        </form>
      </div>
    </div>
  )
}
