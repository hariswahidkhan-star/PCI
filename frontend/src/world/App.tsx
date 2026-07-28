import { useCallback, useEffect, useState } from 'react'
import { api, clearToken, getToken, redeemHandoff, setToken, WorldApiError } from './api'
import Onboarding from './Onboarding'
import Settings from './Settings'
import Passport from './Passport'
import Sharing from './Sharing'
import ForumApp from './forum/ForumApp'
import EmployerPortal from './careers/EmployerPortal'
import MyApplications from './careers/MyApplications'

// ── Dashboard aggregate (mirror of /api/world/me/dashboard) ──

interface Dashboard {
  email: string
  display_name: string | null
  email_verified: boolean
  primary_action: string
  primary_code: string | null
  today: {
    day: string; timezone: string; paused: boolean
    code: string | null; title: string | null; difficulty: string | null
    est_minutes: number | null; state: string; attempt_id: number | null
  }
  passport: { state: string; is_public: boolean; visible_evidence: number }
  progress: { completed: number; industries: number; tracks: number }
  streak_days: number
  week_done: number
  week_target: number | null
  onboarding_state: string | null
  products: { pci_world: { state: string }; pci_ai: { state: string } }
  recent: { attempt_id: number; code: string; title: string; score: number | null; completed_at: string }[]
  in_progress: { attempt_id: number; code: string; title: string; updated_at: string }[]
  recommended: { code: string; title: string } | null
}

const ACTION_LABEL: Record<string, string> = {
  verify_email: 'Verify your email',
  continue_onboarding: 'Finish setting up',
  continue_today: "Continue today's challenge",
  start_today: "Start today's challenge",
  resume_open: 'Resume your open challenge',
  explore: 'Explore the challenge archive',
}

// The classic server-rendered surfaces stay the play/manage authority for now; this app is the
// account home. Deep links go to those pages on the same origin.
const classic = {
  challenge: (code: string) => `/world/challenge/${encodeURIComponent(code)}`,
  account: '/world/account',
  archive: '/world/challenges',
}

export default function App() {
  const [state, setState] = useState<'boot' | 'auth' | 'suspended' | 'ready' | 'offline'>('boot')
  const [d, setD] = useState<Dashboard | null>(null)
  const [msg, setMsg] = useState('')

  const load = useCallback(async () => {
    try {
      setD(await api<Dashboard>('/api/world/me/dashboard'))
      setState('ready')
    } catch (e) {
      if (e instanceof WorldApiError && e.error === 'account_suspended') { setMsg(e.message); setState('suspended'); return }
      if (e instanceof WorldApiError && (e.error === 'no_token' || e.error === 'world_disabled')) {
        clearToken(); setState('auth'); return
      }
      // Network/5xx: a real session is NOT a sign-out (journey repair P1-04).
      setState(getToken() ? 'offline' : 'auth')
    }
  }, [])

  useEffect(() => {
    void (async () => {
      // Portal handoff: #h=<one-time code> in the fragment — redeemed once, then scrubbed from
      // the address bar so it never lands in history.
      const m = /[#&]h=([A-Za-z0-9]+)/.exec(window.location.hash)
      if (m) {
        history.replaceState(null, '', window.location.pathname)
        try { await redeemHandoff(m[1]) } catch { /* expired/replayed — fall through to sign-in */ }
      }
      await load()
    })()
  }, [load])

  if (state === 'boot') return <Shell><p>Loading your practice account…</p></Shell>
  if (state === 'auth') return <Shell><SignIn onSignedIn={load} /></Shell>
  if (state === 'suspended') return (
    <Shell>
      <div className="card warn">
        <h2>Your PCI World participation is suspended</h2>
        <p>{msg || 'Contact support to resolve this.'}</p>
        <p><small>Your PCI student account is unaffected.</small></p>
      </div>
    </Shell>
  )
  if (state === 'offline' || !d) return (
    <Shell>
      <div className="card">
        <h2>Can't reach PCI World right now</h2>
        <p>You are still signed in — this is a connection problem, not a sign-out.</p>
        <p><button onClick={() => { setState('boot'); void load() }}>Try again</button></p>
      </div>
    </Shell>
  )
  return <Shell><Home d={d} reload={load} onSignOut={() => { clearToken(); setState('auth') }} /></Shell>
}

function Shell({ children }: { children: React.ReactNode }) {
  return (
    <div className="world-shell">
      <header>
        <span className="brand">PCI <b>World</b></span>
        <span className="tag">practice, not certification</span>
      </header>
      <main>{children}</main>
      <footer><small>Operated by the Project Controls Institute. Practice evidence only — never a credential.</small></footer>
    </div>
  )
}

function SignIn({ onSignedIn }: { onSignedIn: () => Promise<void> }) {
  const [mode, setMode] = useState<'signin' | 'register'>('signin')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [displayName, setDisplayName] = useState('')
  const [err, setErr] = useState('')
  const [busy, setBusy] = useState(false)
  const submit = async (e: React.FormEvent) => {
    e.preventDefault()
    setBusy(true); setErr('')
    try {
      const r = mode === 'signin'
        ? await api<{ token: string }>('/api/world/account/login', { email, password })
        : await api<{ token: string }>('/api/world/account/register', { email, password, display_name: displayName })
      setToken(r.token)
      await onSignedIn()
    } catch (e2) {
      // The quarantine conflict answers honestly: this email belongs to a different sign-in.
      setErr(e2 instanceof WorldApiError ? e2.message : `${mode === 'signin' ? 'Sign-in' : 'Registration'} failed — try again.`)
      setBusy(false)
    }
  }
  return (
    <form className="card" onSubmit={(e) => { void submit(e) }}>
      <h2>{mode === 'signin' ? 'Sign in to PCI World' : 'Create your PCI World account'}</h2>
      <p><small>One PCI account works everywhere: the same email and password works on the Institute student
        portal and here — {mode === 'signin' ? 'no second account is ever created' : 'registering here creates your single PCI identity'}.
        {mode === 'register' && ' Anything you practised in this browser before registering is claimed into your account.'}</small></p>
      {mode === 'register' && (
        <>
          <label htmlFor="w_name">Display name (optional — appears on your Passport only if you publish one)</label>
          <input id="w_name" maxLength={80} value={displayName} onChange={(e) => setDisplayName(e.target.value)} />
        </>
      )}
      <label htmlFor="w_email">Email</label>
      <input id="w_email" type="email" autoComplete="email" required value={email} onChange={(e) => setEmail(e.target.value)} />
      <label htmlFor="w_pw">Password{mode === 'register' ? ' (min 10 characters)' : ''}</label>
      <input id="w_pw" type="password" autoComplete={mode === 'signin' ? 'current-password' : 'new-password'} required value={password} onChange={(e) => setPassword(e.target.value)} />
      <p><button disabled={busy}>{busy ? 'Working…' : mode === 'signin' ? 'Sign in' : 'Create account'}</button></p>
      {err && <p role="alert" className="err">{err}</p>}
      <p><small>{mode === 'signin'
        ? <>New here? <button type="button" className="ghost" onClick={() => { setMode('register'); setErr('') }}>Create your account</button></>
        : <>Already registered? <button type="button" className="ghost" onClick={() => { setMode('signin'); setErr('') }}>Sign in instead</button></>}</small></p>
    </form>
  )
}

function Home({ d, reload, onSignOut }: { d: Dashboard; reload: () => Promise<void>; onSignOut: () => void }) {
  const [view, setView] = useState<'home' | 'settings' | 'passport' | 'sharing' | 'forum' | 'employer' | 'applications'>('home')
  const name = d.display_name || d.email
  const onboarding = d.onboarding_state !== null && d.onboarding_state !== 'completed'
  const actionHref =
    d.primary_action === 'continue_onboarding' ? '#onboarding'
    : d.primary_action === 'verify_email' ? classic.account
    : d.primary_code ? classic.challenge(d.primary_code)
    : classic.archive
  if (view !== 'home') {
    return (
      <>
        <p><button className="ghost" onClick={() => setView('home')}>&larr; Back to your dashboard</button></p>
        {view === 'settings' ? <Settings onSignOut={onSignOut} />
          : view === 'passport' ? <Passport />
          : view === 'forum' ? <ForumApp />
          : view === 'employer' ? <EmployerPortal />
          : view === 'applications' ? <MyApplications />
          : <Sharing />}
      </>
    )
  }
  return (
    <>
      {onboarding && <Onboarding state={d.onboarding_state!} onDone={reload} />}
      <div className="card">
        <h1>Welcome back, {name}</h1>
        <p>
          <a className="btn" href={actionHref}>{ACTION_LABEL[d.primary_action] ?? 'Open PCI World'}</a>
        </p>
        <p className="stats">
          <span><b>{d.progress.completed}</b> completed</span>
          <span><b>{d.streak_days}</b>-day streak</span>
          {d.week_target !== null && <span>this week <b>{d.week_done}</b> of <b>{d.week_target}</b></span>}
          <span>Passport: <b>{d.passport.is_public ? 'published' : 'private'}</b></span>
        </p>
        {!d.email_verified && <p className="warn-line">Your email is not verified yet — verification unlocks publishing. <a href={classic.account}>Verify on the account page</a>.</p>}
      </div>

      {d.today.code && (
        <div className="card">
          <h2>Today · {d.today.day}{d.today.paused ? ' (rotation paused — still available)' : ''}</h2>
          <p>{d.today.title} <small>({d.today.difficulty}{d.today.est_minutes ? ` · ~${d.today.est_minutes} min` : ''})</small></p>
          <p><a className="btn secondary" href={classic.challenge(d.today.code)}>
            {d.today.state === 'completed' ? 'Review your result' : d.today.state === 'in_progress' ? 'Continue' : 'Start'}
          </a></p>
        </div>
      )}

      {d.in_progress.length > 0 && (
        <div className="card">
          <h2>Pick up where you left off</h2>
          <ul>{d.in_progress.map(a => (
            <li key={a.attempt_id}><a href={classic.challenge(a.code)}>{a.title}</a> <small>last touched {a.updated_at} UTC</small></li>
          ))}</ul>
        </div>
      )}

      {d.recent.length > 0 && (
        <div className="card">
          <h2>Recent results</h2>
          <ul>{d.recent.map(a => (
            <li key={a.attempt_id}><a href={classic.challenge(a.code)}>{a.title}</a>{a.score !== null && <small> · score {a.score}</small>}</li>
          ))}</ul>
        </div>
      )}

      {d.recommended && (
        <div className="card">
          <h2>Recommended next</h2>
          <p>{d.recommended.title} <small>(rule-based: a challenge you have not completed yet)</small></p>
          <p><a className="btn secondary" href={classic.challenge(d.recommended.code)}>Open this challenge</a></p>
        </div>
      )}

      <div className="card">
        <h2>Your account</h2>
        <p><small>Signed in as <b>{d.email}</b>. Your sign-in email is managed by your PCI account{d.products.pci_ai.state === 'ready' ? ' — change it in the student portal settings.' : '.'}</small></p>
        <p>
          <button className="secondary" onClick={() => setView('passport')}>Practice Passport</button>{' '}
          {/* Reading the forum needs no account and lives on the server-rendered pages; this is the
              write path, so it belongs behind the signed-in dashboard rather than in public nav. */}
          <button className="secondary" onClick={() => setView('forum')}>Start a discussion</button>{' '}
          {/* Careers' public job pages are server-rendered; these are the signed-in surfaces only —
              acting for an employer tenant, and the applicant's own consented record. */}
          <button className="secondary" onClick={() => setView('employer')}>Employer portal</button>{' '}
          <button className="secondary" onClick={() => setView('applications')}>My job applications</button>{' '}
          <button className="secondary" onClick={() => setView('sharing')}>Sharing</button>{' '}
          <button className="secondary" onClick={() => setView('settings')}>Settings &amp; sessions</button>{' '}
          <a className="btn secondary" href={classic.account}>Export &amp; account</a>{' '}
          {d.products.pci_ai.state === 'ready' && <a className="btn secondary" href="/app/">Open the student portal</a>}
        </p>
        <p><button className="ghost" onClick={() => { void reload() }}>Refresh</button></p>
      </div>
    </>
  )
}
