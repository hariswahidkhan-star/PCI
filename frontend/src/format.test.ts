import { describe, it, expect, vi, afterEach } from 'vitest'
import { fmtDate, fmtDateTime, fmtMoney, titleCase, initials, isPast, daysUntil } from './format'

// Clock-free, locale-free helpers — fully deterministic.
describe('titleCase', () => {
  it('splits on _ / - and capitalizes each word', () => {
    expect(titleCase('exam_fee_paid')).toBe('Exam Fee Paid')
    expect(titleCase('hello-world')).toBe('Hello World')
  })
  it('returns an empty string for null/undefined', () => {
    expect(titleCase(null)).toBe('')
    expect(titleCase(undefined)).toBe('')
  })
})

describe('initials', () => {
  it('takes the first letters, uppercased', () => {
    expect(initials('Haris', 'Khan')).toBe('HK')
    expect(initials('haris')).toBe('H')
  })
  it("falls back to 'U' when there is nothing to abbreviate", () => {
    expect(initials()).toBe('U')
    expect(initials('', '')).toBe('U')
  })
})

// Locale-dependent formatters: assert the deterministic branches strictly, and the formatted
// output against the runner's OWN Intl result so the test is robust to the CI locale.
describe('fmtMoney', () => {
  it('formats a number as currency', () => {
    expect(fmtMoney(1234.5)).toBe(new Intl.NumberFormat(undefined, { style: 'currency', currency: 'USD' }).format(1234.5))
    expect(fmtMoney(1000, 'gbp')).toBe(new Intl.NumberFormat(undefined, { style: 'currency', currency: 'GBP' }).format(1000))
  })
  it("returns '—' for a non-numeric amount", () => {
    expect(fmtMoney('abc')).toBe('—')
    expect(fmtMoney(null)).toBe('—')
  })
})

describe('fmtDate', () => {
  it("returns '—' for falsy input", () => {
    expect(fmtDate('')).toBe('—')
    expect(fmtDate(null)).toBe('—')
  })
  it('passes an unparseable string through unchanged', () => {
    expect(fmtDate('not-a-date')).toBe('not-a-date')
  })
  it('renders a bare YYYY-MM-DD as LOCAL midnight', () => {
    const expected = new Date(2024, 2, 5).toLocaleDateString(undefined, { year: 'numeric', month: 'short', day: 'numeric' })
    expect(fmtDate('2024-03-05')).toBe(expected)
  })
})

describe('fmtDateTime', () => {
  it("returns '—' for falsy and passes invalid strings through", () => {
    expect(fmtDateTime('')).toBe('—')
    expect(fmtDateTime('nope')).toBe('nope')
  })
  it('falls back to local formatting when the timezone is invalid', () => {
    const iso = '2024-03-05T12:00:00Z'
    const expected = new Date(iso).toLocaleString(undefined, { dateStyle: 'medium', timeStyle: 'short' })
    expect(fmtDateTime(iso, 'Not/AZone')).toBe(expected)
  })
})

describe('isPast / daysUntil (clock-pinned)', () => {
  afterEach(() => vi.useRealTimers())
  it('isPast is true for a past instant, false for future/invalid', () => {
    vi.useFakeTimers(); vi.setSystemTime(new Date('2024-06-01T00:00:00Z'))
    expect(isPast('2000-01-01')).toBe(true)
    expect(isPast('2999-01-01')).toBe(false)
    expect(isPast(null)).toBe(false)
    expect(isPast('garbage')).toBe(false)
  })
  it('daysUntil counts whole days ahead (ceil), null on bad input', () => {
    vi.useFakeTimers(); vi.setSystemTime(new Date('2024-01-01T00:00:00Z'))
    expect(daysUntil('2024-01-11T00:00:00Z')).toBe(10)
    expect(daysUntil('2023-12-31T00:00:00Z')).toBe(-1)
    expect(daysUntil(null)).toBeNull()
    expect(daysUntil('nope')).toBeNull()
  })
})
