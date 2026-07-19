import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { adminApi } from '../api'
import { ApiError } from '../../api/client'

/** Consumes a one-time reset token from the emailed link (/admin/reset-password?token=…) and sets a
 *  new admin password. The token is read from the query string; on success the admin is sent to the
 *  login page to sign in with the new password. Public route — no session required. */
export default function AdminResetPassword() {
  const nav = useNavigate()
  const token = new URLSearchParams(window.location.search).get('token') ?? ''
  const [pw, setPw] = useState('')
  const [pw2, setPw2] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)
  const [done, setDone] = useState(false)

  async function submit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    if (pw.length < 8) { setError('Use at least 8 characters.'); return }
    if (pw !== pw2) { setError('The two passwords do not match.'); return }
    setBusy(true)
    try {
      await adminApi.post('/api/admin/auth/reset', { token, new_password: pw })
      setDone(true)
      setTimeout(() => nav('/login', { replace: true }), 2500)
    } catch (err) {
      const msg = err instanceof ApiError && err.body && typeof err.body === 'object' && 'message' in err.body
        ? String((err.body as Record<string, unknown>).message)
        : (err instanceof Error ? err.message : 'Could not reset your password.')
      setError(msg)
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="center-page">
      <div className="card login-card">
        <div className="logo">
          <img src="/assets/logo.png" alt="Project Controls Institute" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <h1 style={{ fontSize: '1.25rem', marginTop: '.5rem' }}>Set a new password</h1>
          <p className="muted small">Admin Console</p>
        </div>
        {!token ? (
          <div>
            <div className="notice err" role="alert">This reset link is missing its token. Please use the link from your email, or request a new one.</div>
            <button className="btn block secondary" onClick={() => nav('/login', { replace: true })}>Back to sign in</button>
          </div>
        ) : done ? (
          <div className="notice ok" role="status">Your password has been updated. Redirecting you to sign in…</div>
        ) : (
          <form onSubmit={submit}>
            {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
            <div className="field">
              <label htmlFor="np">New password</label>
              <input id="np" type="password" autoComplete="new-password" required value={pw} onChange={(e) => setPw(e.target.value)} />
            </div>
            <div className="field">
              <label htmlFor="np2">Confirm new password</label>
              <input id="np2" type="password" autoComplete="new-password" required value={pw2} onChange={(e) => setPw2(e.target.value)} />
            </div>
            <button className="btn block" type="submit" disabled={busy}>{busy ? 'Saving…' : 'Set password'}</button>
          </form>
        )}
      </div>
    </div>
  )
}
