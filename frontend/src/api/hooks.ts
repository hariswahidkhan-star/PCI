import { useCallback, useEffect, useState } from 'react'
import { api, UnauthorizedError } from './client'
import { useAuth } from '../auth/AuthContext'

interface QueryState<T> {
  data: T | null
  loading: boolean
  error: string | null
  refetch: () => void
}

/**
 * GET a JSON resource with loading/error state and a manual refetch.
 * A 401 anywhere logs the user out (token expired), matching the portal's behaviour.
 */
export function useQuery<T>(path: string | null): QueryState<T> {
  const { logout } = useAuth()
  const [data, setData] = useState<T | null>(null)
  const [loading, setLoading] = useState<boolean>(path !== null)
  const [error, setError] = useState<string | null>(null)
  const [tick, setTick] = useState(0)

  useEffect(() => {
    if (path === null) return
    let cancelled = false
    setLoading(true)
    setError(null)
    api
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
