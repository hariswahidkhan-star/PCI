import { useEffect, useState, useMemo, type FormEvent } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type Settings as SettingsMap } from '../api'
import { useAdminAuth } from '../AdminAuth'
import { Card, Spinner, ErrorNote } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { titleCase } from '../../format'

/** Self-service password change for the signed-in admin. Lives here in Settings → Security so a
 *  password change is available on demand, not forced as a full-screen prompt at every sign-in. */
function ChangePasswordCard() {
  const { changePassword } = useAdminAuth()
  const [pw, setPw] = useState('')
  const [confirm, setConfirm] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)

  async function submit(e: FormEvent) {
    e.preventDefault()
    setMsg(null)
    if (pw.length < 8) return setMsg({ ok: false, text: 'Password must be at least 8 characters.' })
    if (pw !== confirm) return setMsg({ ok: false, text: 'Passwords do not match.' })
    setBusy(true)
    try {
      await changePassword(pw)
      setPw(''); setConfirm('')
      setMsg({ ok: true, text: 'Password updated. It takes effect on your next sign-in; you stay signed in here.' })
    } catch (err) {
      setMsg({ ok: false, text: err instanceof Error ? err.message : 'Could not update password.' })
    } finally { setBusy(false) }
  }

  return (
    <Card title="Change password">
      <p className="muted small" style={{ marginTop: 0 }}>
        Set a new password for your admin account whenever you like — it is optional, not required at sign-in.
      </p>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.6rem' }}>{msg.text}</div>}
      <form onSubmit={submit} style={{ display: 'grid', gap: '.5rem', maxWidth: 360 }}>
        <div className="field">
          <label htmlFor="admin-np">New password</label>
          <input id="admin-np" type="password" autoComplete="new-password" value={pw} onChange={(e) => setPw(e.target.value)} />
        </div>
        <div className="field">
          <label htmlFor="admin-cp">Confirm password</label>
          <input id="admin-cp" type="password" autoComplete="new-password" value={confirm} onChange={(e) => setConfirm(e.target.value)} />
        </div>
        <div><button className="btn sm" type="submit" disabled={busy || !pw || !confirm}>{busy ? 'Saving…' : 'Update password'}</button></div>
      </form>
    </Card>
  )
}

const GROUPS: { key: string; label: string; match: (k: string) => boolean }[] = [
  { key: 'web', label: 'Website', match: (k) => k.startsWith('web_') || k.startsWith('site_') },
  { key: 'sp', label: 'Student panel', match: (k) => k.startsWith('sp_') },
  { key: 'exam', label: 'Live exam', match: (k) => k.startsWith('exam_') || k.startsWith('auto_block') || k.startsWith('critical_') || k.includes('violation') || k.includes('proctor') },
  { key: 'other', label: 'Platform', match: () => true },
]

function labelFor(k: string) {
  return titleCase(k.replace(/^(web_|sp_|exam_|site_)/, ''))
}

/** Account-level TOTP MFA: enrol (pending) → verify with a code (active) → disable with a code. */
function TwoFactorCard() {
  const [status, setStatus] = useState<{ enabled: boolean; pending: boolean; recovery_remaining: number } | null>(null)
  const [setup, setSetup] = useState<{ secret: string; otpauth: string } | null>(null)
  const [code, setCode] = useState('')
  const [disableCode, setDisableCode] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)
  const [copied, setCopied] = useState(false)
  const [recovery, setRecovery] = useState<string[] | null>(null)
  const reload = () => adminApi.get<{ enabled: boolean; pending: boolean; recovery_remaining: number }>('/api/admin/me/2fa')
    .then(setStatus)
    .catch(() => setStatus({ enabled: false, pending: false, recovery_remaining: 0 }))
  useEffect(() => { reload() }, [])

  async function begin() {
    setBusy(true); setMsg(null)
    try { setSetup(await adminApi.post<{ secret: string; otpauth: string }>('/api/admin/me/2fa/setup', {})); setCode('') }
    catch (e) { setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not start 2FA setup.' }) }
    finally { setBusy(false) }
  }
  async function verify() {
    setBusy(true); setMsg(null)
    try {
      const r = await adminApi.post<{ recovery_codes?: string[] }>('/api/admin/me/2fa/verify', { code: code.trim() })
      setSetup(null); setCode('')
      setRecovery(r.recovery_codes ?? [])
      await reload()
      setMsg({ ok: true, text: '2FA enabled — you will be asked for an authentication code at every sign-in.' })
    } catch (e) {
      setMsg({ ok: false, text: e instanceof Error && e.message === 'totp_invalid' ? 'That code is not valid — check your authenticator app and try again.' : (e as Error).message })
    } finally { setBusy(false) }
  }
  async function disable() {
    setBusy(true); setMsg(null)
    try {
      await adminApi.post('/api/admin/me/2fa/disable', { code: disableCode.trim() })
      setDisableCode(''); setSetup(null)
      setRecovery(null)
      await reload()
      setMsg({ ok: true, text: '2FA disabled for your account.' })
    } catch (e) {
      setMsg({ ok: false, text: e instanceof Error && e.message === 'totp_invalid' ? 'Enter a current code from your authenticator app to disable 2FA.' : (e as Error).message })
    } finally { setBusy(false) }
  }
  async function copyOtpauth() {
    if (!setup) return
    try { await navigator.clipboard.writeText(setup.otpauth); setCopied(true); setTimeout(() => setCopied(false), 2000) } catch { /* clipboard blocked */ }
  }

  return (
    <Card title="Two-factor authentication">
      <p className="muted small" style={{ marginTop: 0 }}>
        Protect your admin account with a one-time code from an authenticator app (Google Authenticator,
        1Password, Authy…). Once enabled, every sign-in asks for the 6-digit code as a second factor.
      </p>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.6rem' }}>{msg.text}</div>}
      {recovery && recovery.length > 0 && (
        <div className="notice" style={{ marginBottom: '.6rem', display: 'grid', gap: '.4rem' }}>
          <strong>Save your recovery codes now</strong>
          <p className="muted small" style={{ margin: 0 }}>
            Each code works once if you lose your authenticator. Store them somewhere safe — they will not be shown again.
          </p>
          <div style={{ fontFamily: 'monospace', fontSize: '.95rem', display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(120px, 1fr))', gap: '.25rem' }}>
            {recovery.map((c) => <span key={c}>{c}</span>)}
          </div>
          <div className="row" style={{ gap: '.4rem' }}>
            <button className="btn ghost sm" onClick={() => { navigator.clipboard?.writeText(recovery.join('\n')).catch(() => {}) }}>Copy all</button>
            <button className="btn sm" onClick={() => setRecovery(null)}>I've saved them</button>
          </div>
        </div>
      )}
      {!status ? (
        <Spinner />
      ) : status.enabled && !setup ? (
        <p style={{ margin: 0 }}>
          <span className="badge ok">Enabled</span>{' '}
          <span className="muted small">Recovery codes remaining: {status.recovery_remaining}</span>
        </p>
      ) : !setup ? (
        <button className="btn sm" disabled={busy} onClick={begin}>
          {busy ? 'Working…' : status.pending ? 'Restart 2FA setup' : 'Enable 2FA'}
        </button>
      ) : (
        <div style={{ display: 'grid', gap: '.5rem' }}>
          <div className="small">1. Add this secret to your authenticator app (choose "enter a setup key" — no QR needed):</div>
          <div style={{ fontFamily: 'monospace', fontSize: '1.05rem', fontWeight: 700, letterSpacing: '.08em', overflowWrap: 'anywhere' }}>{setup.secret}</div>
          <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
            <input readOnly value={setup.otpauth} onFocus={(e) => e.target.select()} aria-label="otpauth URL" style={{ flex: 1, minWidth: 220, fontFamily: 'monospace' }} />
            <button className="btn ghost sm" onClick={copyOtpauth}>{copied ? 'Copied' : 'Copy'}</button>
          </div>
          <div className="small">2. Enter the 6-digit code the app now shows, to confirm it works:</div>
          <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
            <input inputMode="numeric" autoComplete="one-time-code" maxLength={8} placeholder="123456" value={code} onChange={(e) => setCode(e.target.value)} style={{ maxWidth: 140 }} aria-label="Authentication code" />
            <button className="btn sm" disabled={busy || !code.trim()} onClick={verify}>{busy ? 'Verifying…' : 'Verify & enable'}</button>
            <button className="btn ghost sm" onClick={() => { setSetup(null); setCode('') }}>Cancel</button>
          </div>
          <p className="muted small" style={{ margin: 0 }}>2FA is not active until the code is confirmed, so a mis-scanned secret can never lock you out.</p>
        </div>
      )}
      {status && (status.enabled || status.pending) && (
      <details style={{ marginTop: '.8rem' }}>
        <summary className="small" style={{ cursor: 'pointer', fontWeight: 600 }}>{status.enabled ? 'Disable 2FA' : 'Cancel pending setup'}</summary>
        <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap', marginTop: '.5rem' }}>
          {status.enabled && <input maxLength={16} placeholder="Current or recovery code" value={disableCode} onChange={(e) => setDisableCode(e.target.value)} style={{ maxWidth: 180 }} aria-label="Current authentication or recovery code" />}
          <button className="btn sm danger" disabled={busy || (status.enabled && !disableCode.trim())} onClick={disable}>{busy ? 'Working…' : status.enabled ? 'Disable 2FA' : 'Cancel setup'}</button>
        </div>
        <p className="muted small" style={{ marginTop: '.4rem' }}>
          {status.enabled ? 'Requires a current authenticator or recovery code while 2FA is active.' : 'The pending secret has not been activated and can be cleared safely.'}
        </p>
      </details>
      )}
    </Card>
  )
}

export default function Settings() {
  const { data, loading, error, refetch } = useAdminQuery<SettingsMap>('/api/admin/settings')
  const [edits, setEdits] = useState<Record<string, string>>({})
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)

  const grouped = useMemo(() => {
    const keys = Object.keys(data ?? {}).sort()
    const out: Record<string, string[]> = {}
    for (const g of GROUPS) out[g.key] = []
    for (const k of keys) {
      const g = GROUPS.find((g) => g.match(k))!
      out[g.key].push(k)
    }
    return out
  }, [data])

  const val = (k: string) => (k in edits ? edits[k] : (data?.[k] ?? ''))

  async function save() {
    if (Object.keys(edits).length === 0) return
    setBusy(true)
    setMsg(null)
    try {
      const res = await adminApi.patch<{ ok: boolean; rejected?: string[] }>('/api/admin/settings', edits)
      setEdits({})
      refetch()
      setMsg({ ok: true, text: res.rejected && res.rejected.length > 0 ? `Saved. Some settings were not permitted: ${res.rejected.join(', ')}` : 'Settings saved.' })
    } catch (e) {
      setMsg({ ok: false, text: e instanceof Error ? e.message : 'Could not save.' })
    } finally {
      setBusy(false)
    }
  }

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  return (
    <div className="page">
      <PageHeader
        title="Settings"
        actions={<button className="btn sm" disabled={busy || Object.keys(edits).length === 0} onClick={save}>{busy ? 'Saving…' : `Save changes${Object.keys(edits).length ? ` (${Object.keys(edits).length})` : ''}`}</button>}
      />
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status">{msg.text}</div>}
      <p className="muted small">Configuration is stored as key/value pairs. Some keys are restricted to specific roles; the server will report any it does not permit you to change.</p>

      {GROUPS.map((g) =>
        grouped[g.key].length === 0 ? null : (
          <Card key={g.key} title={g.label}>
            <div className="grid cols-2">
              {grouped[g.key].map((k) => {
                const v = val(k)
                const long = (v ?? '').length > 80
                return (
                  <div className="field" key={k} style={long ? { gridColumn: '1 / -1' } : undefined}>
                    {/* htmlFor/id, not just adjacency: the label was visible but not associated,
                        so a screen reader announced every settings field as an unnamed text box.
                        The setting key is already unique, which makes it the natural id. */}
                    <label htmlFor={`set-${k}`} title={k}>{labelFor(k)}</label>
                    {long ? (
                      <textarea id={`set-${k}`} rows={2} value={v} onChange={(e) => setEdits({ ...edits, [k]: e.target.value })} />
                    ) : (
                      <input id={`set-${k}`} value={v} onChange={(e) => setEdits({ ...edits, [k]: e.target.value })} />
                    )}
                  </div>
                )
              })}
            </div>
          </Card>
        ),
      )}

      <ChangePasswordCard />
      <TwoFactorCard />
    </div>
  )
}
