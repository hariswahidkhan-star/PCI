import { useCallback, useEffect, useState } from 'react'
import { adminApi } from './api'
import { UnauthorizedError } from '../api/client'
import { useAdminAuth } from './AdminAuth'

interface QueryState<T> {
  data: T | null
  loading: boolean
  error: string | null
  refetch: () => void
}

/** GET an admin resource with loading/error/refetch. A 401 logs the admin out. */
export function useAdminQuery<T>(path: string | null): QueryState<T> {
  const { logout } = useAdminAuth()
  const [data, setData] = useState<T | null>(null)
  const [loading, setLoading] = useState<boolean>(path !== null)
  const [error, setError] = useState<string | null>(null)
  const [tick, setTick] = useState(0)

  useEffect(() => {
    if (path === null) return
    let cancelled = false
    setLoading(true)
    setError(null)
    adminApi
      .get<T>(path)
      .then((d) => {
        if (!cancelled) setData(d)
      })
      .catch((e) => {
        if (cancelled) return
        if (e instanceof UnauthorizedError) {
          logout()
          return
        }
        setError(e instanceof Error ? e.message : 'Something went wrong.')
      })
      .finally(() => {
        if (!cancelled) setLoading(false)
      })
    return () => {
      cancelled = true
    }
  }, [path, tick, logout])

  const refetch = useCallback(() => setTick((t) => t + 1), [])
  return { data, loading, error, refetch }
}
