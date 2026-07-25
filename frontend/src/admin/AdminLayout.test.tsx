import { describe, it, expect, vi, beforeAll } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter, Routes, Route } from 'react-router-dom'

// Premium shell chrome around every admin screen: RBAC-filtered sidebar with a quick-filter,
// breadcrumb topbar naming the current section, and the Ctrl/Cmd+K command palette. The admin
// console has no i18n provider (English-only), so this renders under a bare MemoryRouter with
// useAdminAuth mocked as a signed-in owner (sees every section) — the AdminLogin.test.tsx pattern.
const h = vi.hoisted(() => ({ logout: vi.fn() }))
vi.mock('./AdminAuth', () => ({
  useAdminAuth: () => ({
    me: { id: 1, email: 'owner@pci.local', name: 'Site Owner', role: 'owner', is_owner: true, must_change_pw: false, permissions: [] },
    logout: h.logout,
    can: () => true,
  }),
}))

import AdminLayout from './AdminLayout'

beforeAll(() => {
  // jsdom has no Element.scrollTo; the layout calls it to reset the scroll container on navigation
  if (!Element.prototype.scrollTo) Element.prototype.scrollTo = () => {}
})

function renderShell(route: string) {
  return render(
    <MemoryRouter initialEntries={[route]}>
      <Routes>
        <Route element={<AdminLayout />}>
          <Route index element={<div>DASHBOARD BODY</div>} />
          <Route path="payments" element={<div>PAYMENTS BODY</div>} />
          <Route path="emails" element={<div>EMAILS BODY</div>} />
        </Route>
      </Routes>
    </MemoryRouter>,
  )
}

describe('AdminLayout premium shell', () => {
  it('names the current section in the breadcrumb topbar', () => {
    const { container } = renderShell('/payments')
    expect(screen.getByText('PAYMENTS BODY')).toBeInTheDocument()
    expect(container.querySelector('.tb-title')).toHaveTextContent('Payments')
    expect(container.querySelector('.tb-crumb')).toHaveTextContent('Admin Console · Students')
  })

  it('quick-filter narrows the sidebar to matching sections', async () => {
    const user = userEvent.setup()
    renderShell('/')
    expect(screen.getByRole('link', { name: 'Students' })).toBeInTheDocument()
    await user.type(screen.getByLabelText('Filter sections'), 'audit')
    expect(screen.getByRole('link', { name: 'Audit log' })).toBeInTheDocument()
    expect(screen.queryByRole('link', { name: 'Students' })).not.toBeInTheDocument()
    // clearing restores the full nav
    await user.click(screen.getByLabelText('Clear filter'))
    expect(screen.getByRole('link', { name: 'Students' })).toBeInTheDocument()
  })

  it('Ctrl+K opens the command palette and Enter navigates to the top match', async () => {
    const user = userEvent.setup()
    renderShell('/')
    expect(screen.getByText('DASHBOARD BODY')).toBeInTheDocument()
    await user.keyboard('{Control>}k{/Control}')
    const box = screen.getByLabelText('Search sections')
    await user.type(box, 'email log{enter}')
    expect(screen.getByText('EMAILS BODY')).toBeInTheDocument()
    // palette closes after navigating
    expect(screen.queryByLabelText('Search sections')).not.toBeInTheDocument()
  })

  it('palette shows an empty state for a query matching nothing', async () => {
    const user = userEvent.setup()
    renderShell('/')
    await user.click(screen.getByRole('button', { name: /Go to section/ }))
    await user.type(screen.getByLabelText('Search sections'), 'zzzz')
    expect(screen.getByText(/No section matches/)).toBeInTheDocument()
    await user.keyboard('{Escape}')
    expect(screen.queryByLabelText('Search sections')).not.toBeInTheDocument()
  })
})
