import type { ReactNode } from 'react'

export function Card({ title, action, children, className }: { title?: ReactNode; action?: ReactNode; children: ReactNode; className?: string }) {
  return (
    <section className={'card' + (className ? ' ' + className : '')}>
      {(title || action) && (
        <div className="spread" style={{ marginBottom: '.9rem' }}>
          {title && <h2 style={{ margin: 0 }}>{title}</h2>}
          {action}
        </div>
      )}
      {children}
    </section>
  )
}

type Tone = 'ok' | 'warn' | 'err' | 'brand' | 'neutral'
export function Badge({ tone = 'neutral', children }: { tone?: Tone; children: ReactNode }) {
  return <span className={'badge' + (tone !== 'neutral' ? ' ' + tone : '')}>{children}</span>
}

/** Map a status string onto a coloured badge with sensible tones. */
export function StatusBadge({ status }: { status?: string | null }) {
  const s = (status ?? '').toLowerCase()
  const ok = ['active', 'paid', 'pass', 'passed', 'issued', 'completed', 'released', 'approved', 'open', 'valid']
  const warn = ['pending', 'scheduled', 'in_progress', 'processing', 'held', 'auto_held', 'awaiting', 'review']
  const err = ['revoked', 'expired', 'fail', 'failed', 'cancelled', 'canceled', 'rejected', 'inactive', 'closed', 'overdue']
  const tone: Tone = ok.includes(s) ? 'ok' : warn.includes(s) ? 'warn' : err.includes(s) ? 'err' : 'neutral'
  return <Badge tone={tone}>{(status ?? 'unknown').replace(/_/g, ' ')}</Badge>
}

export function Spinner() {
  return (
    <div style={{ display: 'grid', placeItems: 'center', padding: '2rem' }}>
      <div className="spinner" role="status" aria-label="Loading" />
    </div>
  )
}

export function Empty({ children }: { children: ReactNode }) {
  return <p className="muted small" style={{ margin: '.5rem 0' }}>{children}</p>
}

export function ErrorNote({ children }: { children: ReactNode }) {
  return <div className="notice err" role="alert">{children}</div>
}

export function Stat({ n, k }: { n: ReactNode; k: ReactNode }) {
  return (
    <div className="stat">
      <span className="n">{n}</span>
      <span className="k">{k}</span>
    </div>
  )
}
