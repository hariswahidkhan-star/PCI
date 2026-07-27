import { useCallback, useEffect, useState } from 'react'
import { api, WorldApiError } from './api'

// The Passport manager, in-app. Same endpoints and rules as the classic page: publication needs a
// deliberate act with at least one selected evidence item; disclosure fields are INDEPENDENT —
// only a changed key is ever sent, and the link expiry changes only when the person picks a new
// value (P0-06: an unrelated save can never move it); whole-history counts with a paged window.

interface EvidenceRow {
  attempt_id: number
  title: string
  code: string
  version: number
  difficulty: string | null
  score: number | null
  completed_at: string
  passport_visible: boolean
}

interface PassportData {
  display_name: string | null
  email_verified: boolean
  passport_public: boolean
  completed: number
  industries: number
  tracks: number
  evidence_total: number
  evidence_offset: number
  evidence: EvidenceRow[]
  show_scores: boolean
  show_profiles: boolean
  show_dates: boolean
  expires_at: string | null
  expired: boolean
}

const URL_KEY = 'pciworld_passport_url'   // same key the classic page uses — one memory of the link

export default function Passport() {
  const [d, setD] = useState<PassportData | null>(null)
  const [rows, setRows] = useState<EvidenceRow[]>([])
  const [err, setErr] = useState('')
  const [msg, setMsg] = useState('')
  const [publishedUrl, setPublishedUrl] = useState<string | null>(localStorage.getItem(URL_KEY))

  const load = useCallback(async () => {
    const p = await api<PassportData>('/api/world/passport')
    setD(p)
    setRows(p.evidence)
  }, [])

  useEffect(() => { void load().catch(() => setErr('Could not load your Passport.')) }, [load])

  if (!d) return <div className="card"><h2>Practice Passport</h2><p>{err || 'Loading…'}</p></div>

  const call = async (fn: () => Promise<void>) => {
    setErr(''); setMsg('')
    try { await fn() } catch (e) {
      setErr(e instanceof WorldApiError ? e.message : 'Something went wrong — try again.')
    }
  }

  const loadMore = () => call(async () => {
    const p = await api<PassportData>(`/api/world/passport?offset=${rows.length}`)
    setRows(r => [...r, ...p.evidence])
  })

  const toggle = (row: EvidenceRow) => call(async () => {
    await api('/api/world/passport/evidence', { attempt_id: row.attempt_id, visible: !row.passport_visible })
    setRows(rs => rs.map(r => r.attempt_id === row.attempt_id ? { ...r, passport_visible: !row.passport_visible } : r))
  })

  const publish = () => call(async () => {
    const r = await api<{ url: string }>('/api/world/passport/publish', { publish: true })
    localStorage.setItem(URL_KEY, r.url)
    setPublishedUrl(r.url)
    setMsg('Published.')
    await load()
  })

  const withdraw = () => {
    if (!confirm('Withdraw your public Passport? The public link stops resolving immediately. Nothing else changes — you can publish again any time.')) return
    void call(async () => {
      await api('/api/world/passport/publish', { publish: false })
      setMsg('Withdrawn — the public link no longer resolves.')
      await load()
    })
  }

  const disclose = (key: 'show_scores' | 'show_profiles' | 'show_dates', value: boolean) => call(async () => {
    // ONLY the changed key travels — absent keys leave stored values (incl. expiry) untouched.
    await api('/api/world/passport/disclosure', { [key]: value })
    await load()
  })

  const setExpiry = (days: string) => {
    if (days === 'keep') return
    void call(async () => {
      await api('/api/world/passport/disclosure', { expires_in_days: parseInt(days, 10) })
      await load()
    })
  }

  const visibleCount = rows.filter(r => r.passport_visible).length

  return (
    <>
      <div className="card">
        <h2>Practice Passport</h2>
        <p className="stats">
          <span><b>{d.completed}</b> completed</span>
          <span><b>{d.industries}</b> industries</span>
          <span><b>{d.tracks}</b> tracks</span>
          <span>state: <b>{d.passport_public ? (d.expired ? 'published (link expired)' : 'published') : 'private'}</b></span>
        </p>
        {d.passport_public ? (
          <>
            {publishedUrl && <p><small>Public link: <a href={publishedUrl}>{publishedUrl}</a></small></p>}
            <p><button className="secondary" onClick={withdraw}>Withdraw public Passport</button></p>
          </>
        ) : (
          <>
            <p><small>Publishing creates a public page under an unguessable link showing ONLY the evidence and fields you
              selected below — never your answers, never certifications. Nothing is public until you press this.</small></p>
            <p><button onClick={() => { void publish() }}>Publish my Passport</button></p>
          </>
        )}
        {msg && <p role="status">{msg}</p>}
        {err && <p role="alert" className="err">{err}</p>}
      </div>

      <div className="card">
        <h2>What the public page shows</h2>
        <p>
          <label><input type="checkbox" checked={d.show_scores} onChange={(e) => { void disclose('show_scores', e.target.checked) }} /> Scores</label>{' '}
          <label><input type="checkbox" checked={d.show_profiles} onChange={(e) => { void disclose('show_profiles', e.target.checked) }} /> Decision profiles</label>{' '}
          <label><input type="checkbox" checked={d.show_dates} onChange={(e) => { void disclose('show_dates', e.target.checked) }} /> Dates</label>
        </p>
        <label htmlFor="pp_exp">Link expiry — current: {d.expires_at ?? 'never'}</label>
        <select id="pp_exp" defaultValue="keep" onChange={(e) => { setExpiry(e.target.value); e.target.value = 'keep' }}>
          <option value="keep">Keep current expiry</option>
          <option value="0">No expiry</option>
          <option value="30">30 days from today</option>
          <option value="90">90 days from today</option>
          <option value="365">One year from today</option>
        </select>
      </div>

      <div className="card">
        <h2>Evidence · {visibleCount} selected — showing {rows.length} of {d.evidence_total}</h2>
        <ul>
          {rows.map(r => (
            <li key={r.attempt_id}>
              <label>
                <input type="checkbox" checked={r.passport_visible} onChange={() => { void toggle(r) }} />{' '}
                {r.title} <small>({r.code} v{r.version}{r.score !== null ? ` · score ${r.score}` : ''} · {r.completed_at})</small>
              </label>
            </li>
          ))}
        </ul>
        {rows.length < d.evidence_total && (
          <p><button className="ghost" onClick={() => { void loadMore() }}>Load older evidence</button></p>
        )}
      </div>
    </>
  )
}
