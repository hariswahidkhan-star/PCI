import { Navigate, useLocation } from 'react-router-dom'
import type { ReactNode } from 'react'
import { useAdminAuth } from './AdminAuth'

/** Gate for the admin console: wait for the token check, then require a signed-in admin. Password
 *  changes are self-service in Settings → Security — there is no forced full-screen prompt on sign-in. */
export default function RequireAdmin({ children }: { children: ReactNode }) {
  const { me, ready } = useAdminAuth()
  const loc = useLocation()

  if (!ready) {
    return (
      <div className="center-page">
        <div className="spinner" role="status" aria-label="Loading" />
      </div>
    )
  }
  if (!me) return <Navigate to="/login" replace state={{ from: loc.pathname }} />
  return <>{children}</>
}
