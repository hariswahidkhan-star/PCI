import { Navigate, useLocation } from 'react-router-dom'
import type { ReactNode } from 'react'
import { useAuth } from '../auth/AuthContext'

/** Gate for authenticated routes. Waits for the initial token check, then redirects to login. */
export default function RequireAuth({ children }: { children: ReactNode }) {
  const { user, ready } = useAuth()
  const loc = useLocation()

  if (!ready) {
    return (
      <div className="center-page">
        <div className="spinner" role="status" aria-label="Loading" />
      </div>
    )
  }
  if (!user) return <Navigate to="/login" replace state={{ from: loc.pathname }} />
  return <>{children}</>
}
