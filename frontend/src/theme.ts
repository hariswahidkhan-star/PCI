/** Theme preference: OS by default, overridable, and remembered.
 *
 * Three states rather than two, deliberately. "System" is the default and is not the same as
 * "light" — a reader whose device switches to dark in the evening should follow it unless they have
 * said otherwise, and a two-state toggle silently pins them to whatever they last tapped.
 *
 * The attribute is written to <html> so CSS can win in both directions: the OS preference applies
 * unless `data-theme="light"` is set, and `data-theme="dark"` applies regardless of the OS.
 */
export type ThemeChoice = 'system' | 'light' | 'dark'

const KEY = 'pci.theme'

export function readTheme(): ThemeChoice {
  try {
    const v = localStorage.getItem(KEY)
    return v === 'light' || v === 'dark' || v === 'system' ? v : 'system'
  } catch {
    // Private browsing or a blocked store: fall back to following the OS rather than failing.
    return 'system'
  }
}

/** Resolve what the reader will actually see, which is what a control should describe. */
export function effectiveTheme(choice: ThemeChoice): 'light' | 'dark' {
  if (choice !== 'system') return choice
  return typeof window !== 'undefined' &&
    window.matchMedia?.('(prefers-color-scheme: dark)').matches
    ? 'dark'
    : 'light'
}

export function applyTheme(choice: ThemeChoice): void {
  const root = document.documentElement
  if (choice === 'system') root.removeAttribute('data-theme')
  else root.setAttribute('data-theme', choice)
  try {
    localStorage.setItem(KEY, choice)
  } catch {
    // Not being able to remember the choice must not stop it applying for this session.
  }
}

/** Call once at start-up, before first paint, so there is no flash of the wrong theme.
 *
 * Also stamps `data-theme-aware`, which marks this document as one that HAS a dark treatment. Only
 * the portal and admin bundles load theme.css and call this; PCI World keeps its parchment identity
 * in every colour scheme. Stylesheets shared by all four bundles (demo/banner.css) gate their
 * prefers-color-scheme rules on the marker, so a dark device does not drop a dark strip onto a page
 * that stays light around it.
 */
export function initTheme(): void {
  const root = document.documentElement
  root.setAttribute('data-theme-aware', '')
  const choice = readTheme()
  if (choice !== 'system') root.setAttribute('data-theme', choice)
}
