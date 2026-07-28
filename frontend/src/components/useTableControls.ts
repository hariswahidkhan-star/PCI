import { useMemo, useState, useEffect, useRef } from 'react'

/* Client-side search + sort + pagination for the admin console's list screens.
   The admin endpoints already return whole collections (they are operator-scale, not
   public-scale), so filtering in the browser keeps every existing endpoint untouched
   while giving each list the advanced search/filter/sort the console was missing.
   If a collection ever outgrows this, the same hook shape can be backed by query
   params without changing a single call site. */

export type SortDir = 'asc' | 'desc'

export interface TableControls<T> {
  /** Rows after search + filters + sort + pagination — what the table renders. */
  rows: T[]
  /** Rows after search + filters + sort, before pagination — what the count reports. */
  matched: T[]
  query: string
  setQuery: (v: string) => void
  sortKey: string | null
  sortDir: SortDir
  /** Toggle sorting on a column: first click ascending, second descending. */
  toggleSort: (key: string) => void
  /** Spread onto a <th> to make it a sortable header. */
  sortProps: (key: string) => { className: string; 'aria-sort': 'ascending' | 'descending' | 'none' }
  page: number
  setPage: (p: number) => void
  pages: number
  total: number
  pageSize: number
}

/** Normalise any cell value to a lower-case string for substring searching. */
function haystack(v: unknown): string {
  if (v == null) return ''
  if (typeof v === 'string') return v.toLowerCase()
  if (typeof v === 'number' || typeof v === 'boolean') return String(v).toLowerCase()
  return ''
}

const isEmpty = (v: unknown): boolean => v == null || v === ''

/** Compare two NON-EMPTY cell values: numbers numerically, everything else as natural text. */
function compare(a: unknown, b: unknown): number {
  if (typeof a === 'number' && typeof b === 'number') return a - b
  const an = Number(a)
  const bn = Number(b)
  if (!Number.isNaN(an) && !Number.isNaN(bn) && String(a).trim() !== '' && String(b).trim() !== '') return an - bn
  return String(a).localeCompare(String(b), undefined, { numeric: true, sensitivity: 'base' })
}

/** Order two rows by one column. Empty cells always sort LAST — in both directions, which is
 *  why the direction is applied to the value comparison rather than by swapping the arguments:
 *  a descending sort that floated every blank to the top would bury the rows the operator is
 *  actually looking for. */
function orderBy(a: unknown, b: unknown, dir: SortDir): number {
  const ae = isEmpty(a)
  const be = isEmpty(b)
  if (ae || be) return ae && be ? 0 : ae ? 1 : -1
  return dir === 'asc' ? compare(a, b) : compare(b, a)
}

export function useTableControls<T extends Record<string, unknown>>(
  all: T[] | null | undefined,
  opts: {
    /** Columns searched by the free-text box. Defaults to every key on the first row. */
    searchKeys?: string[]
    /** Extra predicates (dropdown filters). Falsy entries are ignored. */
    filters?: (((row: T) => boolean) | null | false | undefined)[]
    /** A serialisable signature of the filter *state* (e.g. JSON.stringify of the dropdown
     *  values). Filter predicates are fresh closures on every render, so their identity can
     *  neither memoise the result nor detect a change; this string can do both. Omit it when
     *  there are no filters. */
    filterKey?: string
    pageSize?: number
    initialSort?: { key: string; dir?: SortDir }
  } = {},
): TableControls<T> {
  const { searchKeys, filters, filterKey = '', pageSize = 25, initialSort } = opts
  const [query, setQuery] = useState('')
  const [sortKey, setSortKey] = useState<string | null>(initialSort?.key ?? null)
  const [sortDir, setSortDir] = useState<SortDir>(initialSort?.dir ?? 'asc')
  const [page, setPage] = useState(1)

  const rowsIn = useMemo(() => all ?? [], [all])

  // `filters` is a fresh array of closures on every render, so it can't be a useMemo dependency
  // (it would defeat the memo) and it can't detect a change (a re-render is not a filter change).
  // The caller's filterKey stands in for both. Held in a ref so the memo can read the current
  // predicates without depending on their identity.
  const filtersRef = useRef(filters)
  filtersRef.current = filters

  const matched = useMemo(() => {
    const needle = query.trim().toLowerCase()
    const keys = searchKeys ?? (rowsIn.length > 0 ? Object.keys(rowsIn[0]) : [])
    const active = (filtersRef.current ?? []).filter(Boolean) as ((row: T) => boolean)[]
    let out = rowsIn
    if (active.length > 0) out = out.filter((r) => active.every((f) => f(r)))
    if (needle) out = out.filter((r) => keys.some((k) => haystack(r[k]).includes(needle)))
    if (sortKey) {
      // slice() first — never sort the caller's array in place.
      out = out.slice().sort((a, b) => orderBy(a[sortKey], b[sortKey], sortDir))
    }
    return out
    // filterKey is deliberately a dependency even though the body never reads it: it is the
    // caller's stand-in for `filtersRef.current`, which the linter cannot see through a ref.
    // Dropping it would leave the list stale when a dropdown value changes.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [rowsIn, query, sortKey, sortDir, searchKeys, filterKey])

  const total = matched.length
  const pages = Math.max(1, Math.ceil(total / pageSize))

  // Narrowing the result set can leave the viewer stranded past the last page.
  useEffect(() => {
    if (page > pages) setPage(pages)
  }, [page, pages])

  // Any change to what is being matched sends the operator back to the first page.
  useEffect(() => {
    setPage(1)
  }, [query, sortKey, sortDir, filterKey])

  const rows = useMemo(() => matched.slice((page - 1) * pageSize, page * pageSize), [matched, page, pageSize])

  function toggleSort(key: string) {
    if (sortKey === key) {
      setSortDir((d) => (d === 'asc' ? 'desc' : 'asc'))
    } else {
      setSortKey(key)
      setSortDir('asc')
    }
  }

  function sortProps(key: string) {
    return {
      className: 'sortable',
      'aria-sort': (sortKey === key ? (sortDir === 'asc' ? 'ascending' : 'descending') : 'none') as
        | 'ascending'
        | 'descending'
        | 'none',
    }
  }

  return { rows, matched, query, setQuery, sortKey, sortDir, toggleSort, sortProps, page, setPage, pages, total, pageSize }
}
