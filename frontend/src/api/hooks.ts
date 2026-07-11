import { useCallback, useEffect, useRef, useState } from 'react'
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
  const lastPath = useRef<string | null>(null)
  const dataRef = useRef<T | null>(null)

  useEffect(() => {
    if (path === null) return
    let cancelled = false
    // A refetch of the SAME path is a background refresh: keep the current data rendered instead
    // of flipping to a page-level spinner (which unmounts children and eats their success notices).
    // A path change is a genuinely new query and shows the loading state as before.
    // On a genuine path change this is a fresh query: show the spinner and forget the old path's
    // data so a first-load failure on the new path still surfaces its error.
    if (lastPath.current !== path) { setLoading(true); dataRef.current = null }
    lastPath.current = path
    setError(null)
    api
      .get<T>(path)
      .then((d) => {
        if (!cancelled) { setData(d); dataRef.current = d; setError(null) }
      })
      .catch((e) => {
        if (cancelled) return
        if (e instanceof UnauthorizedError) {
          logout()
          return
        }
        // A failed BACKGROUND refresh (we already have data for this path, e.g. a refetch after a
        // successful mutation) must not blank the page — keep the good data and its success notice.
        // Only surface a page-level error when there is nothing to show.
        if (dataRef.current == null) setError(e instanceof Error ? e.message : 'Something went wrong.')
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

/** Run a mutation, surfacing non-auth failures. A 401 is handled globally (client onUnauthorized). */
export async function runMutation(fn: () => Promise<void>) {
  try {
    await fn()
  } catch (e) {
    if (e instanceof UnauthorizedError) return
    alert(e instanceof Error ? e.message : 'The action could not be completed. Please try again.')
  }
}
