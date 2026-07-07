import { createContext, useContext, type ReactNode } from 'react'
import { useQuery } from '../api/hooks'
import type { Me } from '../api/types'

interface MeState {
  me: Me | null
  loading: boolean
  error: string | null
  refetch: () => void
}

const MeContext = createContext<MeState | null>(null)

/** Loads /api/me once for the whole authenticated portal; pages read it via useMe(). */
export function MeProvider({ children }: { children: ReactNode }) {
  const { data, loading, error, refetch } = useQuery<Me>('/api/me')
  return <MeContext.Provider value={{ me: data, loading, error, refetch }}>{children}</MeContext.Provider>
}

export function useMe(): MeState {
  const ctx = useContext(MeContext)
  if (!ctx) throw new Error('useMe must be used within MeProvider')
  return ctx
}
