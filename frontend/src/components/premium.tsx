import type { ReactNode } from 'react'
import { Link } from 'react-router-dom'
import { Icon, type IconName } from './icons'

/* Shared premium primitives for BOTH portals (student /app and admin /admin).
   These are presentation-only: no data fetching, no portal-specific imports, so the
   student portal can wrap them in i18n and the admin console can use them bare.
   Styling lives in premium.css. */

export type Tone = 'brand' | 'ok' | 'warn' | 'err' | 'neutral'

/* ---------------------------------------------------------------- page head */

/** The masthead every portal screen shares: optional eyebrow, title, sub-line and actions. */
export function PageHeader({
  eyebrow,
  title,
  subtitle,
  actions,
}: {
  eyebrow?: ReactNode
  title: ReactNode
  subtitle?: ReactNode
  actions?: ReactNode
}) {
  return (
    <div className="page-head">
      <div className="page-head-text">
        {eyebrow && <span className="eyebrow">{eyebrow}</span>}
        <h1>{title}</h1>
        {subtitle && <p className="page-head-sub">{subtitle}</p>}
      </div>
      {actions && <div className="page-head-actions">{actions}</div>}
    </div>
  )
}

/* --------------------------------------------------------------- KPI tiles */

/** A dashboard metric: icon, value, label and an optional line of context.
 *  Pass `to` (router link) or `href` to make the whole tile navigate.
 *  Renders the legacy `.stat`/`.stat .n`/`.stat .k` structure inside, so existing
 *  assertions and any CSS keyed on the old stat markup keep working. */
export function KpiTile({
  icon,
  label,
  value,
  hint,
  tone = 'brand',
  to,
  href,
}: {
  icon?: IconName
  label: ReactNode
  value: ReactNode
  hint?: ReactNode
  tone?: Tone
  to?: string
  href?: string
}) {
  const nav = !!(to || href)
  const body = (
    <>
      {(icon || nav) && (
        <div className="kpi-top">
          {icon && (
            <span className="kpi-ic">
              <Icon name={icon} size={17} />
            </span>
          )}
          {nav && <Icon name="arrow-up-right" size={15} className="kpi-go" />}
        </div>
      )}
      {/* .stat / .n / .k keep the legacy hooks (and the label appears exactly once);
          the premium type comes from .kpi-value / .kpi-label layered on top. */}
      <div className="stat">
        <span className="n kpi-value">{value}</span>
        <span className="k kpi-label">{label}</span>
      </div>
      {/* A div, not a p: callers legitimately pass block content here (the CPD tile passes a
          <Meter/>), and a div inside a p is invalid HTML the browser silently un-nests. */}
      {hint && <div className="kpi-hint">{hint}</div>}
    </>
  )
  const cls = `kpi tone-${tone}`
  if (to) return <Link className={cls} to={to}>{body}</Link>
  if (href) return <a className={cls} href={href}>{body}</a>
  return <div className={cls}>{body}</div>
}

export function KpiGrid({ children }: { children: ReactNode }) {
  return <div className="kpi-grid">{children}</div>
}

/* ------------------------------------------------------------------ toolbar */

/** Search + filter bar that sits above a list. `count` is rendered right-aligned. */
export function Toolbar({ children, count }: { children: ReactNode; count?: ReactNode }) {
  return (
    <div className="toolbar">
      {children}
      {count != null && <span className="toolbar-count">{count}</span>}
    </div>
  )
}

export function SearchInput({
  value,
  onChange,
  placeholder = 'Search…',
  label = 'Search',
}: {
  value: string
  onChange: (v: string) => void
  placeholder?: string
  label?: string
}) {
  return (
    <div className="search-field grow">
      <Icon name="search" size={15} />
      <input
        type="search"
        value={value}
        aria-label={label}
        placeholder={placeholder}
        onChange={(e) => onChange(e.target.value)}
      />
      {value && (
        <button type="button" className="clear" aria-label={`Clear ${label.toLowerCase()}`} onClick={() => onChange('')}>
          <Icon name="x" size={13} />
        </button>
      )}
    </div>
  )
}

export function FilterSelect({
  label,
  value,
  onChange,
  options,
  allLabel = 'All',
}: {
  label: string
  value: string
  onChange: (v: string) => void
  options: { value: string; label: string }[]
  allLabel?: string
}) {
  const id = `flt-${label.replace(/\W+/g, '-').toLowerCase()}`
  return (
    <div className="filter-field">
      <label htmlFor={id}>{label}</label>
      <select id={id} value={value} onChange={(e) => onChange(e.target.value)}>
        <option value="">{allLabel}</option>
        {options.map((o) => (
          <option key={o.value} value={o.value}>
            {o.label}
          </option>
        ))}
      </select>
    </div>
  )
}

/** Compact mutually-exclusive filter with optional per-option counts. */
export function Segmented<T extends string>({
  value,
  onChange,
  options,
  label,
}: {
  value: T
  onChange: (v: T) => void
  options: { value: T; label: string; n?: number }[]
  label?: string
}) {
  return (
    <div className="segmented" role="group" aria-label={label}>
      {options.map((o) => (
        <button
          key={o.value}
          type="button"
          aria-pressed={value === o.value}
          onClick={() => onChange(o.value)}
        >
          {o.label}
          {o.n != null && <span className="seg-n">{o.n}</span>}
        </button>
      ))}
    </div>
  )
}

/** A sortable `<th>`. Pair with useTableControls: `<SortHeader {...t.sortProps('name')}
 *  onSort={() => t.toggleSort('name')}>Name</SortHeader>`. */
export function SortHeader({
  children,
  onSort,
  className,
  ...rest
}: {
  children: ReactNode
  onSort: () => void
  className?: string
  'aria-sort'?: 'ascending' | 'descending' | 'none'
}) {
  const dir = rest['aria-sort']
  return (
    <th className={className} aria-sort={dir}>
      <button type="button" onClick={onSort}>
        {children}
        <Icon
          name={dir === 'ascending' ? 'chevron-down' : dir === 'descending' ? 'chevron-down' : 'sort'}
          size={12}
          className={'caret' + (dir === 'ascending' ? ' up' : '')}
        />
      </button>
    </th>
  )
}

/* ------------------------------------------------------------ empty states */

export function EmptyState({
  icon = 'inbox',
  title,
  detail,
  action,
}: {
  icon?: IconName
  title: ReactNode
  detail?: ReactNode
  action?: ReactNode
}) {
  return (
    <div className="empty-state">
      <span className="es-ic">
        <Icon name={icon} size={22} />
      </span>
      <span className="es-title">{title}</span>
      {detail && <p className="es-detail">{detail}</p>}
      {action && <div className="es-action">{action}</div>}
    </div>
  )
}

/* --------------------------------------------------------------- skeletons */

/** Shape-accurate loading state for a table — steadier than a centred spinner. */
export function SkeletonTable({ rows = 5, cols = 4 }: { rows?: number; cols?: number }) {
  return (
    <div className="sk-table" role="status" aria-label="Loading">
      <div className="sk-row head">
        {Array.from({ length: cols }, (_, i) => (
          <span className="skeleton" key={i} />
        ))}
      </div>
      {Array.from({ length: rows }, (_, r) => (
        <div className="sk-row" key={r}>
          {Array.from({ length: cols }, (_, c) => (
            <span className="skeleton" key={c} />
          ))}
        </div>
      ))}
    </div>
  )
}

export function SkeletonCards({ n = 4 }: { n?: number }) {
  return (
    <div className="sk-cards" role="status" aria-label="Loading">
      {Array.from({ length: n }, (_, i) => (
        <span className="skeleton sk-card" key={i} />
      ))}
    </div>
  )
}

/* -------------------------------------------------------------- pagination */

export function Pagination({
  page,
  pages,
  total,
  pageSize,
  onPage,
}: {
  page: number
  pages: number
  total: number
  pageSize: number
  onPage: (p: number) => void
}) {
  if (pages <= 1) return null
  const from = total === 0 ? 0 : (page - 1) * pageSize + 1
  const to = Math.min(page * pageSize, total)
  return (
    <div className="pager">
      <span className="pager-info">
        Showing {from}–{to} of {total}
      </span>
      <div className="pager-controls">
        <button className="btn secondary sm" disabled={page <= 1} onClick={() => onPage(page - 1)} aria-label="Previous page">
          <Icon name="chevron-left" size={14} />
        </button>
        <span className="page-n">
          Page {page} of {pages}
        </span>
        <button className="btn secondary sm" disabled={page >= pages} onClick={() => onPage(page + 1)} aria-label="Next page">
          <Icon name="chevron-right" size={14} />
        </button>
      </div>
    </div>
  )
}

/* ------------------------------------------------------- progress & series */

export function Meter({ value, max = 100, tone }: { value: number; max?: number; tone?: 'ok' | 'warn' | 'err' }) {
  const pct = max > 0 ? Math.max(0, Math.min(100, (value / max) * 100)) : 0
  return (
    <div
      className={'meter' + (tone ? ' ' + tone : '')}
      role="progressbar"
      aria-valuenow={Math.round(pct)}
      aria-valuemin={0}
      aria-valuemax={100}
    >
      <i style={{ width: pct + '%' }} />
    </div>
  )
}

/** A labelled horizontal bar series — funnels, product mixes, category splits. */
export function BarSeries({
  items,
  format,
}: {
  items: { key: string; label: string; value: number; tone?: 'ok' | 'warn' | 'err' }[]
  format?: (n: number) => string
}) {
  const max = Math.max(1, ...items.map((i) => i.value))
  const fmt = format ?? ((n: number) => String(n))
  return (
    <ul className="bars">
      {items.map((i) => (
        <li key={i.key}>
          <div className="bar-head">
            <span className="bar-k">{i.label}</span>
            <span className="bar-v">{fmt(i.value)}</span>
          </div>
          <Meter value={i.value} max={max} tone={i.tone} />
        </li>
      ))}
    </ul>
  )
}

/** A compact column trend (12-month revenue and similar). Pure CSS bars — no chart
 *  library, no layout thrash, and it degrades to nothing when there is no data. */
export function TrendColumns({
  points,
  format,
}: {
  points: { key: string; value: number }[]
  format?: (n: number) => string
}) {
  if (points.length === 0) return null
  const max = Math.max(1, ...points.map((p) => p.value))
  const fmt = format ?? ((n: number) => String(n))
  return (
    <div>
      <div className="trend-cols">
        {points.map((p) => (
          <i key={p.key} style={{ height: Math.max(2, (p.value / max) * 100) + '%' }} title={`${p.key}: ${fmt(p.value)}`} />
        ))}
      </div>
      <div className="trend-foot">
        <span>{points[0].key}</span>
        <span>{points[points.length - 1].key}</span>
      </div>
    </div>
  )
}

/* ------------------------------------------------------------- work queues */

/** One actionable row in an admin work queue: what needs attention, how many, where to go. */
export function QueueRow({
  icon = 'clock',
  label,
  detail,
  n,
  to,
  hot,
}: {
  icon?: IconName
  label: ReactNode
  detail?: ReactNode
  n: ReactNode
  to?: string
  hot?: boolean
}) {
  const body = (
    <>
      <span className="queue-ic">
        <Icon name={icon} size={15} />
      </span>
      <span className="queue-text">
        <span className="qt-label">{label}</span>
        {detail && <span className="qt-detail">{detail}</span>}
      </span>
      <span className="queue-n">{n}</span>
    </>
  )
  const cls = 'queue-row' + (hot ? ' hot' : '')
  return to ? (
    <Link className={cls} to={to}>
      {body}
    </Link>
  ) : (
    <div className={cls}>{body}</div>
  )
}

/* ------------------------------------------------------------- quick links */

export function QuickTile({
  icon,
  label,
  to,
  href,
  n,
}: {
  icon: IconName
  label: ReactNode
  to?: string
  href?: string
  n?: ReactNode
}) {
  const body = (
    <>
      <span className="qt-ic">
        <Icon name={icon} size={16} />
      </span>
      <span className="truncate">{label}</span>
      {n != null && <span className="qt-n">{n}</span>}
    </>
  )
  if (to) return <Link className="quick-tile" to={to}>{body}</Link>
  return <a className="quick-tile" href={href}>{body}</a>
}

/* --------------------------------------------------------- definition list */

export function DefList({ items }: { items: { label: ReactNode; value: ReactNode }[] }) {
  return (
    <dl className="dl">
      {items.map((it, i) => (
        <div key={i}>
          <dt>{it.label}</dt>
          <dd>{it.value}</dd>
        </div>
      ))}
    </dl>
  )
}
