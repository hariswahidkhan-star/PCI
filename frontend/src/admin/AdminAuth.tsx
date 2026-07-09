import { createContext, useContext, useEffect, useState, useCallback, type ReactNode } from 'react'
import { adminApi, type AdminMe, type AdminLoginResponse } from './api'
import { UnauthorizedError } from '../api/client'

interface AdminAuthState {
  me: AdminMe | null
  ready: boolean
  login: (email: string, password: string) => Promise<void>
  logout: () => void
  changePassword: (newPassword: string) => Promise<void>
  /** owners see everything; otherwise the section must be in the permissions list */
  can: (section: string) => boolean
}

const Ctx = createContext<AdminAuthState | null>(null)

export function AdminAuthProvider({ children }: { children: ReactNode }) {
  const [me, setMe] = useState<AdminMe | null>(null)
  const [ready, setReady] = useState(false)

  useEffect(() => {
    let cancelled = false
    // A 401 on ANY admin request (query or mutation) drops the session and returns to login.
    adminApi.onUnauthorized(() => setMe(null))
    async function boot() {
      if (!adminApi.getToken()) {
        setReady(true)
        return
      }
      // 401 => dead token; transient network/5xx => retry briefly so a backend blip on reload doesn't
      // bounce a still-valid admin session to the login screen.
      for (let attempt = 0; attempt < 3 && !cancelled; attempt++) {
        try {
          const who = await adminApi.get<AdminMe>('/api/admin/me')
          if (!cancelled) setMe(who)
          break
        } catch (e) {
          if (e instanceof UnauthorizedError) { adminApi.setToken(null); break }
          if (attempt === 2) break
          await new Promise((r) => setTimeout(r, 600 * (attempt + 1)))
        }
      }
      if (!cancelled) setReady(true)
    }
    boot()
    return () => {
      cancelled = true
      adminApi.onUnauthorized(null)
    }
  }, [])

  const login = useCallback(async (email: string, password: string) => {
    const res = await adminApi.post<AdminLoginResponse>('/api/admin/auth/login', { email, password }, { allowUnauthorized: true })
    adminApi.setToken(res.token)
    setMe({
      id: res.admin.id,
      email: res.admin.email,
      name: res.admin.name,
      role: res.admin.role,
      is_owner: res.admin.role === 'owner',
      must_change_pw: res.admin.must_change_pw,
      permissions: res.permissions ?? [],
    })
  }, [])

  const logout = useCallback(() => {
    adminApi.post('/api/admin/auth/logout').catch(() => {})
    adminApi.setToken(null)
    setMe(null)
  }, [])

  const changePassword = useCallback(async (newPassword: string) => {
    await adminApi.post('/api/admin/me/password', { new_password: newPassword })
    setMe((m) => (m ? { ...m, must_change_pw: false } : m))
  }, [])

  const can = useCallback(
    (section: string) => !!me && (me.is_owner || me.permissions.includes(section)),
    [me],
  )

  return <Ctx.Provider value={{ me, ready, login, logout, changePassword, can }}>{children}</Ctx.Provider>
}

export function useAdminAuth(): AdminAuthState {
  const c = useContext(Ctx)
  if (!c) throw new Error('useAdminAuth must be used within AdminAuthProvider')
  return c
}
