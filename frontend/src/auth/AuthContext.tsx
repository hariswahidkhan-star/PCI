import { createContext, useContext, useEffect, useState, useCallback, type ReactNode } from 'react'
import { api, getToken, setToken, UnauthorizedError } from '../api/client'
import type { LoginResponse } from '../api/types'

interface SessionUser {
  id: number
  email: string
  firstName: string
  lastName: string
  /** staff support view (admin impersonation) — set from the /api/me probe, never from a normal login */
  impersonated?: boolean
}

interface AuthState {
  user: SessionUser | null
  ready: boolean // finished the initial token check
  /** Gentle sign-in note (e.g. an expired World handoff link) — informational, never an error wall. */
  notice: string | null
  login: (email: string, password: string, totp?: string) => Promise<void>
  register: (data: { firstName: string; lastName: string; email: string; password: string; confirmPassword?: string; country?: string; mobile?: string }) => Promise<void>
  googleSignIn: (credential: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthState | null>(null)

// A lightweight endpoint to confirm an existing token is still valid on reload.
interface MeProbe {
  user: { id: number; email: string; first_name: string; last_name: string; impersonated?: boolean }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<SessionUser | null>(null)
  const [ready, setReady] = useState(false)
  const [notice, setNotice] = useState<string | null>(null)

  const logout = useCallback(() => {
    // revoke server-side first (fire-and-forget), then clear local state
    api.post('/api/logout', {}, { allowUnauthorized: true }).catch(() => {})
    setToken(null)
    setUser(null)
  }, [])

  // A 401 on ANY request (query or mutation) clears the session and returns to login.
  useEffect(() => {
    api.onUnauthorized(() => { setToken(null); setUser(null) })
    return () => api.onUnauthorized(null)
  }, [])

  // On first load, if a token is present, confirm it still resolves to a user. A 401 means the token
  // is dead (clear it); a transient network/5xx error is retried briefly so a backend blip on reload
  // doesn't bounce a still-valid session to the login screen.
  useEffect(() => {
    let cancelled = false
    // World → MyPCI portal handoff: the World switcher navigates here with a one-time 90-second
    // code in the FRAGMENT (#world-code=…) — never a query string, so it never reaches a server
    // log. The fragment is cleared IMMEDIATELY, before any other work, so the code cannot linger
    // in the address bar, history, or anything that reads location later; redemption happens in
    // boot() below against the copy captured here.
    let worldCode: string | null = null
    // Support-view / test-user hand-off: the admin console links to /app/#t=<token> with a ready
    // session token (impersonation or test account). Adopt it — replacing any existing session —
    // and scrub it from the URL so the token never lingers in the address bar or history.
    try {
      const h = window.location.hash
      const wc = /[#&]world-code=([A-Za-z0-9]+)/.exec(h)
      if (wc) {
        worldCode = wc[1]
        history.replaceState(null, '', window.location.pathname + window.location.search)
      } else if (h.startsWith('#t=')) {
        const t = decodeURIComponent(h.slice(3))
        if (t) setToken(t)
        history.replaceState(null, '', window.location.pathname + window.location.search)
      }
    } catch { /* malformed hash — ignore */ }
    async function boot() {
      if (worldCode) {
        // One-use code → a FRESH 30-day portal session, stored exactly the way login stores it.
        try {
          const res = await api.post<LoginResponse>('/api/portal-handoff/redeem', { code: worldCode }, { allowUnauthorized: true })
          setToken(res.token)
          setUser({ id: res.user.id, email: res.user.email, firstName: res.user.firstName, lastName: res.user.lastName })
          if (!cancelled) setReady(true)
          return
        } catch {
          // Expired, replayed or offline: fall through to the normal boot — any existing session
          // still signs in, and an anonymous visitor lands on the ordinary sign-in screen with a
          // gentle notice, never an error wall.
          setNotice('That sign-in link from PCI World has expired — please sign in below.')
        }
      }
      if (!getToken()) {
        setReady(true)
        return
      }
      for (let attempt = 0; attempt < 3 && !cancelled; attempt++) {
        try {
          const me = await api.get<MeProbe>('/api/me')
          if (!cancelled) setUser({ id: me.user.id, email: me.user.email, firstName: me.user.first_name, lastName: me.user.last_name, impersonated: me.user.impersonated })
          break
        } catch (e) {
          if (e instanceof UnauthorizedError) { setToken(null); break }
          if (attempt === 2) break // give up after retries; token retained for a later reload
          await new Promise((r) => setTimeout(r, 600 * (attempt + 1)))
        }
      }
      if (!cancelled) setReady(true)
    }
    boot()
    return () => {
      cancelled = true
    }
  }, [])

  const login = useCallback(async (email: string, password: string, totp?: string) => {
    const res = await api.post<LoginResponse>('/api/login', { email, password, totp }, { allowUnauthorized: true })
    setToken(res.token)
    setUser(res.user)
  }, [])

  const register = useCallback(async (data: { firstName: string; lastName: string; email: string; password: string; confirmPassword?: string; country?: string; mobile?: string }) => {
    const res = await api.post<LoginResponse>('/api/register', data, { allowUnauthorized: true })
    setToken(res.token)
    setUser(res.user)
  }, [])

  const googleSignIn = useCallback(async (credential: string) => {
    const res = await api.post<LoginResponse>('/api/auth/google', { credential }, { allowUnauthorized: true })
    setToken(res.token)
    setUser(res.user)
  }, [])

  return <AuthContext.Provider value={{ user, ready, notice, login, register, googleSignIn, logout }}>{children}</AuthContext.Provider>
}

export function useAuth(): AuthState {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
