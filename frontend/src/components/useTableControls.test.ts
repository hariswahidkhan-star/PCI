import { describe, it, expect } from 'vitest'
import { renderHook, act } from '@testing-library/react'
import { useTableControls } from './useTableControls'

// FE — the client-side search/sort/pagination engine behind every upgraded admin list.
// The decisions worth pinning: free-text search only looks at the declared searchKeys;
// dropdown filters compose with AND; sorting is numeric-aware and always puts empty cells
// last in BOTH directions (so blanks never crowd out real rows); and any change to what is
// matched returns the operator to page 1 rather than stranding them past the last page.

interface Row extends Record<string, unknown> {
  id: number
  name: string
  status: string
  score: number | null
}

const ROWS: Row[] = [
  { id: 1, name: 'Zara Ali', status: 'active', score: 90 },
  { id: 2, name: 'adam brooks', status: 'pending', score: 8 },
  { id: 3, name: 'Mia Chen', status: 'active', score: null },
  { id: 4, name: 'Noor Diaz', status: 'closed', score: 100 },
]

const setup = (opts: Parameters<typeof useTableControls<Row>>[1] = {}) =>
  renderHook(() => useTableControls<Row>(ROWS, opts))

describe('useTableControls', () => {
  it('returns every row untouched when nothing is applied', () => {
    const { result } = setup()
    expect(result.current.rows.map((r) => r.id)).toEqual([1, 2, 3, 4])
    expect(result.current.total).toBe(4)
    expect(result.current.pages).toBe(1)
  })

  it('searches case-insensitively across the declared keys only', () => {
    const { result } = setup({ searchKeys: ['name'] })
    act(() => result.current.setQuery('ADAM'))
    expect(result.current.rows.map((r) => r.id)).toEqual([2])

    // 'active' lives in `status`, which is not searchable here — so nothing matches.
    act(() => result.current.setQuery('active'))
    expect(result.current.rows).toHaveLength(0)
  })

  it('composes dropdown filters with the search box', () => {
    const { result } = setup({
      searchKeys: ['name'],
      filters: [(r) => r.status === 'active'],
    })
    expect(result.current.rows.map((r) => r.id)).toEqual([1, 3])
    act(() => result.current.setQuery('mia'))
    expect(result.current.rows.map((r) => r.id)).toEqual([3])
  })

  it('ignores falsy filter slots (an unset dropdown)', () => {
    const { result } = setup({ filters: [null, false, undefined] })
    expect(result.current.total).toBe(4)
  })

  it('re-matches when a filter’s VALUE changes, not just when one is added or removed', () => {
    // The predicate array is a fresh set of closures every render, so the filterKey is what
    // tells the hook the active filters actually changed. Switching 'active' → 'closed' keeps
    // the filter COUNT at one; without the key the list would silently go stale.
    const { result, rerender } = renderHook(
      ({ status }: { status: string }) =>
        useTableControls<Row>(ROWS, { filters: [(r) => r.status === status], filterKey: status }),
      { initialProps: { status: 'active' } },
    )
    expect(result.current.rows.map((r) => r.id)).toEqual([1, 3])
    rerender({ status: 'closed' })
    expect(result.current.rows.map((r) => r.id)).toEqual([4])
  })

  it('returns to page 1 when a filter value changes', () => {
    const { result, rerender } = renderHook(
      ({ status }: { status: string }) =>
        useTableControls<Row>(ROWS, { filters: [(r) => r.status !== status], filterKey: status, pageSize: 1 }),
      { initialProps: { status: 'nothing' } },
    )
    act(() => result.current.setPage(3))
    expect(result.current.page).toBe(3)
    rerender({ status: 'active' })
    expect(result.current.page).toBe(1)
  })

  it('sorts text naturally and toggles direction on the second click', () => {
    const { result } = setup()
    act(() => result.current.toggleSort('name'))
    expect(result.current.rows.map((r) => r.name)).toEqual(['adam brooks', 'Mia Chen', 'Noor Diaz', 'Zara Ali'])
    expect(result.current.sortProps('name')['aria-sort']).toBe('ascending')

    act(() => result.current.toggleSort('name'))
    expect(result.current.rows.map((r) => r.name)).toEqual(['Zara Ali', 'Noor Diaz', 'Mia Chen', 'adam brooks'])
    expect(result.current.sortProps('name')['aria-sort']).toBe('descending')
  })

  it('sorts numbers numerically, not lexically', () => {
    const { result } = setup()
    act(() => result.current.toggleSort('score'))
    // lexical order would put 100 before 8 — numeric order must not.
    expect(result.current.rows.map((r) => r.score)).toEqual([8, 90, 100, null])
  })

  it('keeps empty cells last in both sort directions', () => {
    const { result } = setup()
    act(() => result.current.toggleSort('score'))
    expect(result.current.rows[result.current.rows.length - 1].score).toBeNull()
    act(() => result.current.toggleSort('score')) // descending
    expect(result.current.rows[result.current.rows.length - 1].score).toBeNull()
  })

  it('paginates and reports the page count', () => {
    const { result } = setup({ pageSize: 2 })
    expect(result.current.pages).toBe(2)
    expect(result.current.rows.map((r) => r.id)).toEqual([1, 2])
    act(() => result.current.setPage(2))
    expect(result.current.rows.map((r) => r.id)).toEqual([3, 4])
  })

  it('returns to page 1 when the search narrows the result set', () => {
    const { result } = setup({ pageSize: 2, searchKeys: ['name'] })
    act(() => result.current.setPage(2))
    expect(result.current.page).toBe(2)
    act(() => result.current.setQuery('a'))
    expect(result.current.page).toBe(1)
  })

  it('never sorts the caller’s array in place', () => {
    const before = ROWS.map((r) => r.id)
    const { result } = setup()
    act(() => result.current.toggleSort('name'))
    expect(ROWS.map((r) => r.id)).toEqual(before)
  })

  it('honours an initial sort', () => {
    const { result } = setup({ initialSort: { key: 'score', dir: 'desc' } })
    expect(result.current.rows.map((r) => r.score)).toEqual([100, 90, 8, null])
  })

  it('survives a null collection (first load, before the fetch resolves)', () => {
    const { result } = renderHook(() => useTableControls<Row>(null))
    expect(result.current.rows).toEqual([])
    expect(result.current.total).toBe(0)
    expect(result.current.pages).toBe(1)
  })
})
