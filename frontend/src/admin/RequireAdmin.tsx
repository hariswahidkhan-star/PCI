import { Navigate, useLocation } from 'react-router-dom'
import type { ReactNode } from 'react'
import { useAdminAuth } from './AdminAuth'
import ChangePassword from './pages/ChangePassword'

/** Gate for the admin console: wait for the token check, force login, then force a password
 *  change if the account is flagged (must_change_pw) before any section is reachable. */
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
  if (me.must_change_pw) return <ChangePassword />
  return <>{children}</>
}
