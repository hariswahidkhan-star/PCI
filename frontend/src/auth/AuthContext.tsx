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

  // On first load, if a token is present, confirm it still resolves to a user.
  useEffect(() => {
    let cancelled = false
    async function boot() {
      if (!getToken()) {
        setReady(true)
        return
      }
      try {
        const me = await api.get<MeProbe>('/api/me')
        if (!cancelled)
          setUser({ id: me.user.id, email: me.user.email, firstName: me.user.first_name, lastName: me.user.last_name })
      } catch (e) {
        if (e instanceof UnauthorizedError) setToken(null)
      } finally {
        if (!cancelled) setReady(true)
      }
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

  const logout = useCallback(() => {
    setToken(null)
    setUser(null)
  }, [])

  return <AuthContext.Provider value={{ user, ready, login, logout }}>{children}</AuthContext.Provider>
}

export function useAuth(): AuthState {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
