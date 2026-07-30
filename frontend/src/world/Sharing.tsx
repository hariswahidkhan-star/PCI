import { useCallback, useEffect, useState } from 'react'
import { api, WorldApiError } from './api'

// Sharing management, in-app (PW-US-039/040): every public surface this account has minted, in
// one place, with the power to withdraw each one — or all of them — immediately. Raw tokens can
// never appear here: the server stores only their hashes, so a link, once shown at creation, is
// unrecoverable by design; what remains manageable is whether it still RESOLVES.

interface ResultLink {
  attempt_id: number
  code: string
  title: string
  completed_at: string
  /** When the link itself was minted (server truth; absent on older payloads). */
  created_at?: string
  revoked: boolean
}

interface Invitation {
  invite_id: number
  code: string
  title: string
  version: number
  created_at: string
  inviter_name: string | null
  revoked: boolean
}

export default function Sharing() {
  const [results, setResults] = useState<ResultLink[]>([])
  const [invitations, setInvitations] = useState<Invitation[]>([])
  const [loaded, setLoaded] = useState(false)
  const [err, setErr] = useState('')

  const load = useCallback(async () => {
    const r = await api<{ results: ResultLink[]; invitations: Invitation[] }>('/api/world/me/shares')
    setResults(r.results)
    setInvitations(r.invitations)
    setLoaded(true)
  }, [])

  useEffect(() => { void load().catch(() => setErr('Could not load your shared links.')) }, [load])

  const call = (fn: () => Promise<unknown>) => {
    setErr('')
    void fn().then(load).catch((e: unknown) =>
      setErr(e instanceof WorldApiError ? e.message : 'Could not withdraw — try again.'))
  }

  if (!loaded) return <div className="card"><h2>Sharing</h2><p>{err || 'Loading…'}</p></div>

  const liveResults = results.filter(r => !r.revoked).length
  const liveInvites = invitations.filter(i => !i.revoked).length

  return (
    <>
      <div className="card">
        <h2>Shared result links · {liveResults} live</h2>
        <p><small>Links minted from your results, on any device. The link itself was shown once when you created
          it and is stored only as a hash — what you control here is whether it still resolves. Withdrawal is
          immediate and permanent for that link; the result itself is untouched.</small></p>
        {results.length === 0 && <p>No result links yet.</p>}
        <ul>
          {results.map(r => (
            <li key={r.attempt_id}>
              {r.title} <small>({r.code} · {r.completed_at}{r.created_at ? ` · link created ${r.created_at}` : ''})</small>{' '}
              {r.revoked
                ? <small>· withdrawn</small>
                : <button className="ghost" onClick={() => call(() => api('/api/world/me/shares/revoke', { attempt_id: r.attempt_id }))}>Withdraw</button>}
            </li>
          ))}
        </ul>
        {liveResults > 0 && (
          <p><button className="ghost" onClick={() => call(() => api('/api/world/me/shares/revoke-all', {}))}>Withdraw all result links</button></p>
        )}
      </div>

      <div className="card">
        <h2>Challenge invitations · {liveInvites} live</h2>
        <p><small>Invitations pin the challenge version that was live when you sent them, so an invitee always
          plays what you played. Withdrawing stops the invitation link resolving; anyone who already finished
          keeps their result.</small></p>
        {invitations.length === 0 && <p>No invitations yet.</p>}
        <ul>
          {invitations.map(i => (
            <li key={i.invite_id}>
              {i.title} <small>({i.code} v{i.version} · sent {i.created_at})</small>{' '}
              {i.revoked
                ? <small>· withdrawn</small>
                : <button className="ghost" onClick={() => call(() => api('/api/world/me/invitations/revoke', { invite_id: i.invite_id }))}>Withdraw</button>}
            </li>
          ))}
        </ul>
        {liveInvites > 0 && (
          <p><button className="ghost" onClick={() => call(() => api('/api/world/me/invitations/revoke-all', {}))}>Withdraw all invitations</button></p>
        )}
      </div>
      {err && <p role="alert" className="err">{err}</p>}
    </>
  )
}
