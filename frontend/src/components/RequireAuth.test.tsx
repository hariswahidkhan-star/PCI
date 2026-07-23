import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter, Routes, Route } from 'react-router-dom'

// Mock the auth hook so the guard's three states can be driven directly — no network probe,
// no real AuthProvider (whose mount fetches /api/me). vi.hoisted keeps the shared value safe
// under vi.mock's hoisting.
const h = vi.hoisted(() => ({ auth: { user: null as unknown, ready: false } }))
vi.mock('../auth/AuthContext', () => ({ useAuth: () => h.auth }))

import RequireAuth from './RequireAuth'

function renderGuarded() {
  return render(
    <MemoryRouter initialEntries={['/secret']}>
      <Routes>
        <Route path="/secret" element={<RequireAuth><div>secret content</div></RequireAuth>} />
        <Route path="/login" element={<div>login page</div>} />
      </Routes>
    </MemoryRouter>,
  )
}

describe('RequireAuth', () => {
  it('shows a loading spinner until the auth check is ready', () => {
    h.auth = { user: null, ready: false }
    renderGuarded()
    expect(screen.getByRole('status', { name: 'Loading' })).toBeInTheDocument()
    expect(screen.queryByText('secret content')).not.toBeInTheDocument()
  })

  it('redirects to /login when ready and unauthenticated', () => {
    h.auth = { user: null, ready: true }
    renderGuarded()
    expect(screen.getByText('login page')).toBeInTheDocument()
    expect(screen.queryByText('secret content')).not.toBeInTheDocument()
  })

  it('renders the children when authenticated', () => {
    h.auth = { user: { id: 1, email: 'a@b.co' }, ready: true }
    renderGuarded()
    expect(screen.getByText('secret content')).toBeInTheDocument()
  })
})
