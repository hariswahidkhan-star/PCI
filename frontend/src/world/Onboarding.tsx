import { useState } from 'react'
import { api, WorldApiError } from './api'

// The onboarding walkthrough, in-app (§7.3). The ORDER lives on the server — this component only
// renders the current step and posts each completion; a refresh or device switch resumes exactly
// where the person stopped, and a manipulated client can never skip a required step.

const GOALS: [string, string][] = [
  ['daily_practice', 'Daily practice'],
  ['certification_prep', 'Certification preparation'],
  ['evidence', 'Build verifiable practice evidence'],
  ['explore', 'Explore project controls'],
]

export default function Onboarding({ state, onDone }: { state: string; onDone: () => Promise<void> }) {
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState('')
  const [goal, setGoal] = useState('daily_practice')
  const [timezone, setTimezone] = useState('')
  const [weeklyTarget, setWeeklyTarget] = useState('')

  const advance = async (step: string, before?: () => Promise<void>) => {
    setBusy(true); setErr('')
    try {
      if (before) await before()
      await api('/api/world/me/onboarding', { step })
      // The privacy step is the last one — finishing it completes onboarding.
      if (step === 'privacy') await api('/api/world/me/onboarding', { step: 'completed' })
      await onDone()
    } catch (e) {
      setErr(e instanceof WorldApiError ? e.message : 'Could not save this step — try again.')
      setBusy(false)
    }
  }

  return (
    <div className="card onboarding" id="onboarding">
      <p className="kicker">Getting started</p>
      {state === 'not_started' && (
        <>
          <h2>Welcome to PCI World</h2>
          <p>A realistic project decision every day, with deterministic scoring and synthetic data.
            PCI World is <b>practice, not certification</b> — completing challenges builds verifiable
            practice evidence, never a formal credential. It is operated by the Project Controls Institute.</p>
          <p><button disabled={busy} onClick={() => { void advance('welcome') }}>Got it — continue</button></p>
        </>
      )}
      {state === 'welcome' && (
        <>
          <h2>Why are you practising?</h2>
          <p>Your goal shapes what the dashboard suggests — nothing else. You can change it any time.</p>
          <label htmlFor="ob_goal">Goal</label>
          <select id="ob_goal" value={goal} onChange={(e) => setGoal(e.target.value)}>
            {GOALS.map(([v, l]) => <option key={v} value={v}>{l}</option>)}
          </select>
          <p><button disabled={busy} onClick={() => {
            void advance('goal', () => api('/api/world/me/preferences', { goal }, 'PATCH').then(() => undefined))
          }}>Save &amp; continue</button></p>
        </>
      )}
      {state === 'goal' && (
        <>
          <h2>Your practice rhythm</h2>
          <label htmlFor="ob_tz">Timezone (optional)</label>
          <input id="ob_tz" maxLength={64} placeholder="Europe/Berlin" value={timezone} onChange={(e) => setTimezone(e.target.value)} />
          <label htmlFor="ob_wt">Weekly target (daily challenges per week, optional)</label>
          <select id="ob_wt" value={weeklyTarget} onChange={(e) => setWeeklyTarget(e.target.value)}>
            <option value="">No target</option>
            {[1, 2, 3, 4, 5, 6, 7].map(n => <option key={n} value={String(n)}>{n}</option>)}
          </select>
          <p><button disabled={busy} onClick={() => {
            const body: Record<string, unknown> = {}
            if (timezone.trim()) body.timezone = timezone.trim()
            if (weeklyTarget) body.weekly_target = parseInt(weeklyTarget, 10)
            void advance('preferences', Object.keys(body).length
              ? () => api('/api/world/me/preferences', body, 'PATCH').then(() => undefined)
              : undefined)
          }}>Save &amp; continue</button></p>
        </>
      )}
      {state === 'preferences' && (
        <>
          <h2>Private until you say otherwise</h2>
          <p>Your challenge answers stay between you and the scoring engine — no public surface ever
            shows them. Nothing about you is public by default: your Passport publishes only when you
            deliberately publish it, and every evidence item and field on it is a separate choice.</p>
          <p><button disabled={busy} onClick={() => { void advance('privacy') }}>I understand — finish setup</button></p>
        </>
      )}
      {err && <p role="alert" className="err">{err}</p>}
    </div>
  )
}
