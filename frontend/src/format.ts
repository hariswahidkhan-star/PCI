// Display formatting helpers, kept framework-free so they're easy to unit-test.

// The UI's selected language (I18nProvider keeps <html lang> in sync); falls back to the
// browser locale where no provider set it (e.g. the admin SPA).
const uiLocale = () => (typeof document !== 'undefined' && document.documentElement.lang) || undefined

export function fmtDate(v: unknown): string {
  if (!v) return '—'
  const s = String(v)
  // A bare calendar date (YYYY-MM-DD, e.g. a credential expiry or CPD date) is timezone-agnostic —
  // parse it as LOCAL midnight, not UTC, so viewers west of UTC don't see the previous day.
  const bare = /^\d{4}-\d{2}-\d{2}$/.exec(s)
  const d = bare ? new Date(Number(bare[0].slice(0, 4)), Number(bare[0].slice(5, 7)) - 1, Number(bare[0].slice(8, 10)))
                 : new Date(s.includes('T') ? s : s.replace(' ', 'T') + 'Z')
  if (isNaN(d.getTime())) return s
  return d.toLocaleDateString(uiLocale(), { year: 'numeric', month: 'short', day: 'numeric' })
}

export function fmtDateTime(v: unknown, tz?: unknown): string {
  if (!v) return '—'
  const s = String(v)
  const d = new Date(s.includes('T') ? s : s.replace(' ', 'T') + 'Z')
  if (isNaN(d.getTime())) return s
  // When a timezone is supplied (e.g. an exam booking's tz) format the instant IN that zone, so the
  // displayed clock matches the "(America/New_York)" label instead of the viewer's local time.
  const zone = typeof tz === 'string' && tz ? tz : undefined
  try {
    return d.toLocaleString(uiLocale(), { dateStyle: 'medium', timeStyle: 'short', ...(zone ? { timeZone: zone } : {}) })
  } catch {
    return d.toLocaleString(uiLocale(), { dateStyle: 'medium', timeStyle: 'short' })
  }
}

export function fmtMoney(amount: unknown, currency = 'USD'): string {
  const n = typeof amount === 'number' ? amount : parseFloat(String(amount ?? ''))
  if (isNaN(n)) return '—'
  try {
    return new Intl.NumberFormat(uiLocale(), { style: 'currency', currency: (currency || 'USD').toUpperCase() }).format(n)
  } catch {
    return `${currency} ${n.toFixed(2)}`
  }
}

export function titleCase(s: unknown): string {
  const str = String(s ?? '').replace(/[_-]+/g, ' ').trim()
  return str.replace(/\b\w/g, (c) => c.toUpperCase())
}

export function initials(first?: string, last?: string): string {
  return ((first?.[0] ?? '') + (last?.[0] ?? '')).toUpperCase() || 'U'
}

/** true when an ISO/SQL datetime is in the past (used for credential expiry). */
export function isPast(v: unknown): boolean {
  if (!v) return false
  const s = String(v)
  const d = new Date(s.includes('T') ? s : s.replace(' ', 'T') + 'Z')
  return !isNaN(d.getTime()) && d.getTime() < Date.now()
}

export function daysUntil(v: unknown): number | null {
  if (!v) return null
  const s = String(v)
  const d = new Date(s.includes('T') ? s : s.replace(' ', 'T') + 'Z')
  if (isNaN(d.getTime())) return null
  return Math.ceil((d.getTime() - Date.now()) / 86_400_000)
}
