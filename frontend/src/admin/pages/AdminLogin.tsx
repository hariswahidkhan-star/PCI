import { useState, type FormEvent } from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import { useAdminAuth } from '../AdminAuth'
import { ApiError } from '../../api/client'
import { adminApi } from '../api'

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
  // Forgot-password sub-flow (request a reset link).
  const [mode, setMode] = useState<'login' | 'forgot'>('login')
  const [sent, setSent] = useState(false)

  async function requestReset(e: FormEvent) {
    e.preventDefault()
    setError(null)
    setBusy(true)
    try {
      await adminApi.post('/api/admin/auth/forgot', { email: email.trim().toLowerCase() })
      setSent(true)
    } catch {
      // The endpoint always succeeds; only a network error lands here.
      setSent(true)
    } finally {
      setBusy(false)
    }
  }

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
          <p className="muted small">{mode === 'login' ? 'Staff sign-in.' : 'Reset your password.'}</p>
        </div>

        {mode === 'login' ? (
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
            <div style={{ textAlign: 'center', marginTop: '.9rem' }}>
              <button type="button" className="linkish" style={{ background: 'none', border: 0, color: 'var(--brand,#1D4ED8)', cursor: 'pointer', font: 'inherit', fontSize: '.85rem' }}
                onClick={() => { setMode('forgot'); setError(null); setSent(false) }}>
                Forgot password?
              </button>
            </div>
          </form>
        ) : sent ? (
          <div>
            <div className="notice ok" role="status" style={{ marginBottom: '1rem' }}>
              If an admin account exists for <strong>{email || 'that address'}</strong>, a password-reset link is on its way.
              The link is valid for one hour. Check your inbox (and spam).
            </div>
            <p className="muted small">Didn’t get it? Your PCI instance may not have email configured yet — in that case the link is written to the server’s email log. Ask whoever manages the deployment, or use the environment-variable reset.</p>
            <button className="btn block secondary" onClick={() => { setMode('login'); setSent(false) }}>Back to sign in</button>
          </div>
        ) : (
          <form onSubmit={requestReset}>
            <p className="muted small" style={{ marginBottom: '1rem' }}>Enter your admin email and we’ll send a link to reset your password.</p>
            <div className="field">
              <label htmlFor="femail">Email address</label>
              <input id="femail" type="email" autoComplete="username" required value={email} onChange={(e) => setEmail(e.target.value)} />
            </div>
            <button className="btn block" type="submit" disabled={busy}>{busy ? 'Sending…' : 'Send reset link'}</button>
            <div style={{ textAlign: 'center', marginTop: '.9rem' }}>
              <button type="button" style={{ background: 'none', border: 0, color: 'var(--brand,#1D4ED8)', cursor: 'pointer', font: 'inherit', fontSize: '.85rem' }}
                onClick={() => { setMode('login'); setError(null) }}>
                Back to sign in
              </button>
            </div>
          </form>
        )}
      </div>
    </div>
  )
}
