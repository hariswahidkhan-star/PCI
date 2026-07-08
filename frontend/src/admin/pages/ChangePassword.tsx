import { useState, type FormEvent } from 'react'
import { useAdminAuth } from '../AdminAuth'

/** Forced when must_change_pw is set (e.g. a freshly-provisioned or reset admin account). */
export default function ChangePassword() {
  const { changePassword } = useAdminAuth()
  const [pw, setPw] = useState('')
  const [confirm, setConfirm] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  async function submit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    if (pw.length < 8) return setError('Password must be at least 8 characters.')
    if (pw !== confirm) return setError('Passwords do not match.')
    setBusy(true)
    try {
      await changePassword(pw)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Could not update password.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="center-page">
      <div className="card login-card">
        <h1 style={{ fontSize: '1.25rem' }}>Set a new password</h1>
        <p className="muted small">Your account requires a password change before continuing.</p>
        <form onSubmit={submit}>
          {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
          <div className="field">
            <label htmlFor="np">New password</label>
            <input id="np" type="password" autoComplete="new-password" required value={pw} onChange={(e) => setPw(e.target.value)} />
          </div>
          <div className="field">
            <label htmlFor="cp">Confirm password</label>
            <input id="cp" type="password" autoComplete="new-password" required value={confirm} onChange={(e) => setConfirm(e.target.value)} />
          </div>
          <button className="btn block" type="submit" disabled={busy}>{busy ? 'Saving…' : 'Update password'}</button>
        </form>
      </div>
    </div>
  )
}
