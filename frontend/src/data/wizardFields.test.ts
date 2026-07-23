import { describe, it, expect, vi, afterEach } from 'vitest'
import { monthsCovered } from './wizardFields'

// The live eligibility tally: merge overlapping employment intervals, count inclusive months once.
describe('monthsCovered', () => {
  afterEach(() => vi.useRealTimers())

  it('counts the inclusive months of a single closed interval', () => {
    expect(monthsCovered([{ start_date: '2020-01', end_date: '2020-12', is_current: 0 }])).toBe(12)
  })

  it('counts overlapping months only once', () => {
    expect(monthsCovered([
      { start_date: '2020-01', end_date: '2020-06', is_current: 0 },
      { start_date: '2020-04', end_date: '2020-09', is_current: 0 },
    ])).toBe(9)
  })

  it('does not bridge a gap between two separate intervals', () => {
    expect(monthsCovered([
      { start_date: '2020-01', end_date: '2020-03', is_current: 0 },
      { start_date: '2021-01', end_date: '2021-03', is_current: 0 },
    ])).toBe(6)
  })

  it('clamps an is_current row to the current month', () => {
    vi.useFakeTimers(); vi.setSystemTime(new Date('2020-06-15T12:00:00Z'))
    // Jan 2020 .. now (Jun 2020) inclusive = 6 months
    expect(monthsCovered([{ start_date: '2020-01', end_date: null, is_current: 1 }])).toBe(6)
  })

  it('ignores rows whose start date is unparseable', () => {
    expect(monthsCovered([{ start_date: '', end_date: '2020-12', is_current: 0 }])).toBe(0)
  })
})
