import { useEffect, useId, useRef, type ReactNode } from 'react'
import '../simlab.css'

// Simulation Lab reusable primitives, shared by the student catalogue, the
// simulation workspace and the Admin Studio. Presentation only — no data or
// business logic lives here. Everything is keyboard-operable and announces
// state changes politely; styles live in simlab.css on the --sl-* tokens.

/** Premium page header: eyebrow + title + one orientation sentence + optional actions. */
export function LabHeader({ eyebrow, title, lede, actions, children }: {
  eyebrow?: ReactNode
  title: ReactNode
  lede?: ReactNode
  actions?: ReactNode
  children?: ReactNode
}) {
  return (
    <header className="sl-header">
      {eyebrow && <div className="sl-eyebrow">{eyebrow}</div>}
      <div className="sl-header-row">
        <div>
          <h1>{title}</h1>
          {lede && <p className="sl-lede">{lede}</p>}
        </div>
        {actions}
      </div>
      {children}
    </header>
  )
}

export interface Kpi { k: ReactNode; v: ReactNode; suffix?: string }
/** Quiet KPI strip — hairline separators, tabular numerals, no boxes. */
export function KpiRow({ items }: { items: Kpi[] }) {
  if (items.length === 0) return null
  return (
    <div className="sl-kpis">
      {items.map((it, i) => (
        <div className="sl-kpi" key={i}>
          <span className="v">{it.v}{it.suffix && <small>{it.suffix}</small>}</span>
          <span className="k">{it.k}</span>
        </div>
      ))}
    </div>
  )
}

export interface SegOption<T extends string> { value: T; label: ReactNode }
/** Segmented control on native radios: full keyboard support for free. */
export function SegmentedControl<T extends string>({ legend, options, value, onChange, disabled, hint }: {
  legend: string
  options: SegOption<T>[]
  value: T
  onChange: (v: T) => void
  disabled?: boolean
  hint?: ReactNode
}) {
  const name = useId()
  return (
    <fieldset className="sl-seg">
      <legend>{legend}</legend>
      <div className="sl-seg-track">
        {options.map((o) => (
          <label className="sl-seg-opt" key={o.value}>
            <input
              type="radio"
              name={name}
              value={o.value}
              checked={value === o.value}
              disabled={disabled}
              onChange={() => onChange(o.value)}
            />
            <span>{o.label}</span>
          </label>
        ))}
      </div>
      {hint && <p className="sl-seg-hint">{hint}</p>}
    </fieldset>
  )
}

/** Toggle chip (filters). Pressed state is exposed via aria-pressed. */
export function Chip({ pressed, onClick, children }: { pressed: boolean; onClick: () => void; children: ReactNode }) {
  return (
    <button type="button" className="sl-chip" aria-pressed={pressed} onClick={onClick}>
      {children}
    </button>
  )
}

export type SaveState = 'idle' | 'saving' | 'saved' | 'error'
/** Persistent, unobtrusive save indicator. Announced politely via role="status". */
export function SaveIndicator({ state, note }: { state: SaveState; note?: string | null }) {
  const text =
    note ?? (state === 'saving' ? 'Saving…' : state === 'saved' ? 'Saved' : state === 'error' ? 'Not saved' : '')
  return (
    <span className={'sl-save ' + state} role="status">
      <span className="dot" aria-hidden="true" />
      {text && <span>{text}</span>}
    </span>
  )
}

/** Insight/callout. Red is reserved for genuine risk or destructive outcomes. */
export function InsightCallout({ tone = 'accent', title, children, role }: {
  tone?: 'accent' | 'success' | 'warning' | 'danger' | 'info'
  title?: ReactNode
  children: ReactNode
  role?: 'status' | 'alert'
}) {
  return (
    <div className={'sl-callout' + (tone !== 'accent' ? ' ' + tone : '')} role={role}>
      {title && <div className="sl-callout-title">{title}</div>}
      {typeof children === 'string' ? <p>{children}</p> : children}
    </div>
  )
}

/** Catalogue loading state: skeleton cards matching the final layout (no shift). */
export function SkeletonCards({ count = 6 }: { count?: number }) {
  return (
    <div className="sl-skel-cards" role="status" aria-label="Loading the catalogue">
      {Array.from({ length: count }, (_, i) => (
        <div className="sl-skel-card" key={i} aria-hidden="true">
          <div className="skeleton" /><div className="skeleton" /><div className="skeleton" />
          <div className="skeleton" /><div className="skeleton" />
        </div>
      ))}
    </div>
  )
}

/** Hint-level progression dots (n of max). */
export function LevelDots({ level, max = 5, label }: { level: number; max?: number; label?: string }) {
  return (
    <span className="sl-leveldots" role="img" aria-label={label ?? `Hint level ${level} of ${max}`}>
      {Array.from({ length: max }, (_, i) => <i key={i} className={i < level ? 'on' : ''} />)}
    </span>
  )
}

/** Debrief score hero: outcome first, evidence second. */
export function ScoreSummary({ score, heading, sub, detail, badge }: {
  score: number
  heading: ReactNode
  sub?: ReactNode
  detail?: ReactNode
  badge?: ReactNode
}) {
  return (
    <div className="sl-scorehero">
      <span className="n">{score}<small>%</small></span>
      <div className="sl-scoresub">
        <span className="t">{heading} {badge}</span>
        {sub && <span className="d">{sub}</span>}
        {detail && <span className="d">{detail}</span>}
      </div>
    </div>
  )
}

/** Accessible confirmation dialog for irreversible or work-losing choices.
 *  Focus moves to the cancel action on open, Escape cancels, backdrop cancels,
 *  and focus returns to the previously focused element on close. */
export function ConfirmDialog({ open, title, body, confirmLabel, cancelLabel = 'Cancel', danger, onConfirm, onCancel }: {
  open: boolean
  title: string
  body: ReactNode
  confirmLabel: string
  cancelLabel?: string
  danger?: boolean
  onConfirm: () => void
  onCancel: () => void
}) {
  const cancelRef = useRef<HTMLButtonElement>(null)
  const titleId = useId()
  useEffect(() => {
    if (!open) return
    const previous = document.activeElement as HTMLElement | null
    cancelRef.current?.focus()
    return () => previous?.focus?.()
  }, [open])
  if (!open) return null
  return (
    <div
      className="sl-dialog-backdrop"
      onMouseDown={(e) => { if (e.target === e.currentTarget) onCancel() }}
      onKeyDown={(e) => { if (e.key === 'Escape') { e.stopPropagation(); onCancel() } }}
    >
      <div className="sl-dialog" role="dialog" aria-modal="true" aria-labelledby={titleId}>
        <h2 id={titleId}>{title}</h2>
        {typeof body === 'string' ? <p>{body}</p> : body}
        <div className="sl-dialog-actions">
          <button ref={cancelRef} type="button" className="btn secondary" onClick={onCancel}>{cancelLabel}</button>
          <button type="button" className={'btn' + (danger ? ' danger' : '')} onClick={onConfirm}>{confirmLabel}</button>
        </div>
      </div>
    </div>
  )
}

/** Quiet uppercase section subheading used inside work panels. */
export function PanelSub({ children }: { children: ReactNode }) {
  return <h3 className="sl-panel-sub">{children}</h3>
}
