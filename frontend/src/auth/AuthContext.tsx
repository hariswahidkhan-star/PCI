import { createContext, useContext, useEffect, useState, useCallback, type ReactNode } from 'react'
import { api, getToken, setToken, UnauthorizedError } from '../api/client'
import type { LoginResponse } from '../api/types'

interface SessionUser {
  id: number
  email: string
  firstName: string
  lastName: string
}

interface AuthState {
  user: SessionUser | null
  ready: boolean // finished the initial token check
  login: (email: string, password: string) => Promise<void>
  register: (data: { firstName: string; lastName: string; email: string; password: string; country?: string; mobile?: string }) => Promise<void>
  googleSignIn: (credential: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthState | null>(null)

// A lightweight endpoint to confirm an existing token is still valid on reload.
interface MeProbe {
  user: { id: number; email: string; first_name: string; last_name: string }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<SessionUser | null>(null)
  const [ready, setReady] = useState(false)

  const logout = useCallback(() => {
    // revoke server-side first (fire-and-forget), then clear local state
    api.post('/api/logout', {}, { allowUnauthorized: true }).catch(() => {})
    setToken(null)
    setUser(null)
  }, [])

  // A 401 on ANY request (query or mutation) clears the session and returns to login.
  useEffect(() => {
    api.onUnauthorized(() => setUser(null))
    return () => api.onUnauthorized(null)
  }, [])

  // On first load, if a token is present, confirm it still resolves to a user. A 401 means the token
  // is dead (clear it); a transient network/5xx error is retried briefly so a backend blip on reload
  // doesn't bounce a still-valid session to the login screen.
  useEffect(() => {
    let cancelled = false
    async function boot() {
      if (!getToken()) {
        setReady(true)
        return
      }
      for (let attempt = 0; attempt < 3 && !cancelled; attempt++) {
        try {
          const me = await api.get<MeProbe>('/api/me')
          if (!cancelled) setUser({ id: me.user.id, email: me.user.email, firstName: me.user.first_name, lastName: me.user.last_name })
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

  const login = useCallback(async (email: string, password: string) => {
    const res = await api.post<LoginResponse>('/api/login', { email, password }, { allowUnauthorized: true })
    setToken(res.token)
    setUser(res.user)
  }, [])

  const register = useCallback(async (data: { firstName: string; lastName: string; email: string; password: string; country?: string; mobile?: string }) => {
    const res = await api.post<LoginResponse>('/api/register', data, { allowUnauthorized: true })
    setToken(res.token)
    setUser(res.user)
  }, [])

  const googleSignIn = useCallback(async (credential: string) => {
    const res = await api.post<LoginResponse>('/api/auth/google', { credential }, { allowUnauthorized: true })
    setToken(res.token)
    setUser(res.user)
  }, [])

  return <AuthContext.Provider value={{ user, ready, login, register, googleSignIn, logout }}>{children}</AuthContext.Provider>
}

export function useAuth(): AuthState {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
