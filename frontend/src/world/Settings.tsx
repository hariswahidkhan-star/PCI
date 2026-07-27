import { useCallback, useEffect, useState } from 'react'
import { api, WorldApiError } from './api'

// Settings, in-app: World preferences (product data on the participation row — never profile or
// credential data), the sessions view with immediate revocation, and the two sign-out scopes with
// their plain-words copy. Semantics mirror the classic account page exactly — same endpoints,
// same rules, one behaviour.

interface Prefs {
  goal: string | null
  timezone: string | null
  weekly_target: number | null
  reminders_enabled: boolean
  reminder_hour: number | null
}

interface SessionRow {
  id: number
  created_at: string
  expires_at: string
  current: boolean
}

function DataAndAccount({ onSignOut }: { onSignOut: () => void }) {
  const [pw, setPw] = useState('')
  const [err, setErr] = useState('')

  const exportData = async () => {
    setErr('')
    try {
      const data = await api<unknown>('/api/world/account/export')
      const url = URL.createObjectURL(new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' }))
      const a = document.createElement('a')
      a.href = url
      a.download = 'pci-world-export.json'
      a.click()
      URL.revokeObjectURL(url)
    } catch {
      setErr('Could not export your data — try again.')
    }
  }

  const del = async () => {
    if (!confirm('Delete your PCI World participation? Your World attempts are de-identified, your Passport and every shared link stop resolving, and your World sign-ins end everywhere. Your PCI student account, credentials and profile are NOT touched. This cannot be undone.')) return
    setErr('')
    try {
      await api('/api/world/account/delete', { password: pw })
      onSignOut()
    } catch (e) {
      setErr(e instanceof WorldApiError ? e.message : 'Deletion failed — check your password.')
    }
  }

  return (
    <div className="card">
      <h2>Your data &amp; account</h2>
      <p><small>The export contains everything PCI World holds about you — including the answers you gave,
        which no other surface ever shows. Deletion is WORLD-ONLY in scope: it removes your practice
        participation and de-identifies your attempts, while your PCI student account, sign-in and profile
        remain exactly as they are.</small></p>
      <p><button className="secondary" onClick={() => { void exportData() }}>Export my PCI World data (JSON)</button></p>
      <label htmlFor="da_pw">Confirm with your PCI account password to delete</label>
      <input id="da_pw" type="password" autoComplete="current-password" value={pw} onChange={(e) => setPw(e.target.value)} />
      <p><button className="secondary" disabled={!pw} onClick={() => { void del() }}>Delete my PCI World participation</button></p>
      {err && <p role="alert" className="err">{err}</p>}
    </div>
  )
}

const GOAL_LABELS: Record<string, string> = {
  daily_practice: 'Daily practice',
  certification_prep: 'Certification preparation',
  evidence: 'Build verifiable practice evidence',
  explore: 'Explore project controls',
}

export default function Settings({ onSignOut }: { onSignOut: () => void }) {
  const [prefs, setPrefs] = useState<Prefs | null>(null)
  const [goals, setGoals] = useState<string[]>([])
  const [sessions, setSessions] = useState<SessionRow[]>([])
  const [msg, setMsg] = useState('')
  const [err, setErr] = useState('')

  const load = useCallback(async () => {
    const p = await api<{ linked: boolean; preferences: Prefs | null; goals: string[] }>('/api/world/me/preferences')
    setPrefs(p.preferences)
    setGoals(p.goals)
    const s = await api<{ rows: SessionRow[] }>('/api/world/me/sessions')
    setSessions(s.rows)
  }, [])

  useEffect(() => { void load().catch(() => setErr('Could not load settings.')) }, [load])

  const save = async (patch: Record<string, unknown>) => {
    setMsg(''); setErr('')
    try {
      await api('/api/world/me/preferences', patch, 'PATCH')
      setMsg('Saved.')
      await load()
    } catch (e) {
      setErr(e instanceof WorldApiError ? e.message : 'Could not save.')
    }
  }

  const revoke = async (id: number) => {
    await api('/api/world/me/sessions/revoke', { id })
    await load()
  }

  const revokeOthers = async () => {
    await api('/api/world/me/sessions/revoke-others', {})
    await load()
  }

  if (!prefs) return <div className="card"><h2>Settings</h2><p>{err || 'Loading…'}</p></div>

  return (
    <>
      <div className="card">
        <h2>Practice preferences</h2>
        <p><small>Product settings only — your name, profile and sign-in email live with your PCI account.</small></p>
        <label htmlFor="st_goal">Goal</label>
        <select id="st_goal" value={prefs.goal ?? ''} onChange={(e) => { void save({ goal: e.target.value }) }}>
          {prefs.goal === null && <option value="">Not set</option>}
          {goals.map(g => <option key={g} value={g}>{GOAL_LABELS[g] ?? g}</option>)}
        </select>
        <label htmlFor="st_tz">Timezone</label>
        <input id="st_tz" maxLength={64} defaultValue={prefs.timezone ?? ''} placeholder="Europe/Berlin"
          onBlur={(e) => { if (e.target.value.trim() !== (prefs.timezone ?? '')) void save({ timezone: e.target.value.trim() }) }} />
        <label htmlFor="st_wt">Weekly target</label>
        <select id="st_wt" value={prefs.weekly_target === null ? '' : String(prefs.weekly_target)}
          onChange={(e) => { void save({ weekly_target: e.target.value === '' ? null : parseInt(e.target.value, 10) }) }}>
          <option value="">No target</option>
          {[1, 2, 3, 4, 5, 6, 7].map(n => <option key={n} value={String(n)}>{n} per week</option>)}
        </select>
        <p style={{ marginTop: 12 }}>
          <label htmlFor="st_rem" style={{ display: 'inline' }}>
            <input id="st_rem" type="checkbox" checked={prefs.reminders_enabled}
              onChange={(e) => { void save({ reminders_enabled: e.target.checked }) }} />{' '}
            Practice reminders (opt-in — nothing is sent unless you turn this on)
          </label>
        </p>
        {prefs.reminders_enabled && (
          <>
            <label htmlFor="st_rh">Reminder hour (UTC)</label>
            <select id="st_rh" value={prefs.reminder_hour === null ? '' : String(prefs.reminder_hour)}
              onChange={(e) => { if (e.target.value !== '') void save({ reminder_hour: parseInt(e.target.value, 10) }) }}>
              {prefs.reminder_hour === null && <option value="">Choose an hour</option>}
              {Array.from({ length: 24 }, (_, h) => <option key={h} value={String(h)}>{String(h).padStart(2, '0')}:00</option>)}
            </select>
          </>
        )}
        {msg && <p role="status">{msg}</p>}
        {err && <p role="alert" className="err">{err}</p>}
      </div>

      <div className="card">
        <h2>Signed-in sessions</h2>
        <p><small>PCI World sessions only — your student-portal sessions live in the portal's own security view. Revocation is immediate.</small></p>
        <ul>
          {sessions.map(s => (
            <li key={s.id}>
              Session #{s.id} · since {s.created_at} UTC{s.current ? ' · this device' : ''}{' '}
              {!s.current && <button className="ghost" onClick={() => { void revoke(s.id) }}>Revoke</button>}
            </li>
          ))}
        </ul>
        {sessions.filter(s => !s.current).length > 0 && (
          <p><button className="ghost" onClick={() => { void revokeOthers() }}>Sign out all other devices</button></p>
        )}
      </div>

      <DataAndAccount onSignOut={onSignOut} />

      <div className="card">
        <h2>Sign out</h2>
        <p><small>&ldquo;Sign out&rdquo; ends this device&rsquo;s PCI World session only. &ldquo;Sign out of all PCI
          services&rdquo; ends every PCI World session on every device and, when your account is linked, your
          Institute student-portal sessions too.</small></p>
        <p>
          <button className="secondary" onClick={() => { void api('/api/world/account/logout', {}).catch(() => undefined); onSignOut() }}>Sign out</button>{' '}
          <button className="secondary" onClick={() => { void api('/api/world/account/logout', { everywhere: true }).catch(() => undefined); onSignOut() }}>Sign out of all PCI services</button>
        </p>
      </div>
    </>
  )
}
