import { useState } from 'react'
import { api, WorldApiError } from './api'

// World → MyPCI portal switcher (Phase 3 symmetric handoff).
//
// One press mints a one-time, 90-second, server-hashed code and carries it to the student portal
// in the URL FRAGMENT only (#world-code=…) — browsers never transmit a fragment, so the code can
// never appear in a query string, server log, referrer or proxy. The portal exchanges it for its
// own fresh session on arrival; nothing of the World session crosses over.

interface HandoffResponse {
  code: string
  return_to: string
}

export default function PortalSwitcher({ navigate }: { navigate?: (url: string) => void }) {
  const [state, setState] = useState<'idle' | 'busy' | 'error'>('idle')
  const [msg, setMsg] = useState('')
  const go = () => (navigate ?? ((u: string) => window.location.assign(u)))

  const open = async () => {
    if (state === 'busy') return
    setState('busy')
    setMsg('')
    try {
      const r = await api<HandoffResponse>('/api/world/account/portal-handoffs/mypci', {})
      go()(`${r.return_to}#world-code=${encodeURIComponent(r.code)}`)
    } catch (e) {
      setState('error')
      if (e instanceof WorldApiError) {
        setMsg(
          e.error === 'identity_link_pending'
            ? 'Link your PCI account first — sign in once with your PCI credentials to prove it is yours, then try again.'
            : e.error === 'rate_limited'
              ? 'Too many attempts just now — wait a moment, then try again.'
              : e.message || 'That did not work — try again.',
        )
      } else {
        setMsg("Can't reach PCI right now — check your connection and try again.")
      }
    }
  }

  return (
    <div className="portal-switcher">
      <button
        type="button"
        className="portal-switcher-btn"
        onClick={() => { void open() }}
        disabled={state === 'busy'}
        aria-busy={state === 'busy'}
        aria-describedby="ps-desc"
      >
        <span className="ps-brand">PCI ai <span className="ps-sep" aria-hidden="true">/</span> MyPCI</span>
        <span className="ps-sub" id="ps-desc">Manage applications, membership, learning access and certifications.</span>
        <span className="ps-action">
          {state === 'busy' ? 'Opening MyPCI…' : 'Switch to MyPCI'}
          <span className="ps-arrow" aria-hidden="true">&rarr;</span>
        </span>
      </button>
      {state === 'error' && (
        <p role="alert" className="err">
          {msg}{' '}
          <button type="button" className="ghost" onClick={() => { void open() }}>Try again</button>
        </p>
      )}
    </div>
  )
}
