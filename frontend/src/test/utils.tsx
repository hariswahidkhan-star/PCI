import type { ReactElement, ReactNode } from 'react'
import { render, type RenderOptions } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { I18nProvider } from '../i18n'

// Shared harness for component tests. Wraps the unit under test in the two providers that are
// effectively global to every portal screen: the real I18nProvider (so `useT` resolves the actual
// English catalog strings instead of a stub — the tests assert against real copy) and a MemoryRouter
// (so `useNavigate`/`useLocation`/`useSearchParams`/`<Link>` work without a real browser history).
//
// These two are used REAL (not mocked) on purpose: they are pure, deterministic, and exercising them
// is part of the screen's behaviour (deep-link query parsing, translated labels). Data/auth
// dependencies (`useMe`, `useAuth`, the fetch client) are mocked per-test at their module boundary,
// following the existing RequireAuth.test.tsx pattern.
export function renderWithProviders(
  ui: ReactElement,
  opts: { route?: string; renderOptions?: RenderOptions } = {},
) {
  const { route = '/', renderOptions } = opts
  function Wrapper({ children }: { children: ReactNode }) {
    return (
      <I18nProvider>
        <MemoryRouter initialEntries={[route]}>{children}</MemoryRouter>
      </I18nProvider>
    )
  }
  return render(ui, { wrapper: Wrapper, ...renderOptions })
}
