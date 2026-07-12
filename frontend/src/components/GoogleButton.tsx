import { useEffect, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { api } from '../api/client'
import type { AuthConfig } from '../api/types'
import { useAuth } from '../auth/AuthContext'
import { renderGoogleButton } from '../auth/google'

/** "Continue with Google" — renders the official button only when the backend reports a configured
 * client id, so the page is clean when Google sign-in is off. */
export default function GoogleButton({ onError, destination = '/' }: { onError: (msg: string) => void; destination?: string }) {
  const { googleSignIn } = useAuth()
  const nav = useNavigate()
  const slot = useRef<HTMLDivElement>(null)
  const [enabled, setEnabled] = useState(false)

  useEffect(() => {
    let cancelled = false
    api
      .get<AuthConfig>('/api/auth/config')
      .then(async (cfg) => {
        if (cancelled || !cfg.googleClientId || !slot.current) return
        await renderGoogleButton(slot.current, cfg.googleClientId, async (credential) => {
          try {
            await googleSignIn(credential)
            // Honour the deep-link intent (founding code / exam purchase) carried into signup, so a
            // Google account isn't dropped straight on the dashboard with its code silently lost.
            nav(destination, { replace: true })
          } catch (e) {
            onError(e instanceof Error ? e.message : 'Google sign-in failed. Please try again.')
          }
        })
        if (!cancelled) setEnabled(true)
      })
      .catch(() => {})
    return () => {
      cancelled = true
    }
  }, [googleSignIn, nav, onError, destination])

  return (
    <div style={{ display: enabled ? 'block' : 'none' }}>
      <div ref={slot} className="gsi-slot" />
      <div className="auth-or-line small muted">or</div>
    </div>
  )
}
