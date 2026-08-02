// Shared result visuals — the score dial and the per-domain performance bands — used by the
// in-app exam runner's result screen (exam/ExamRunner.tsx) and the Results page. Ported from the
// classic panel so the two surfaces read a result identically.

/** One row of the server's domain_breakdown JSON: [{domain, pct, band}] */
export interface DomainBand {
  domain: string
  pct: number
  band: 'above' | 'at' | 'below' | string
}

export function parseBreakdown(raw: unknown): DomainBand[] {
  if (Array.isArray(raw)) return raw as DomainBand[]
  if (typeof raw === 'string' && raw) {
    try {
      const v = JSON.parse(raw)
      return Array.isArray(v) ? (v as DomainBand[]) : []
    } catch {
      return []
    }
  }
  return []
}

/** The circular score dial (percent 0–100, coloured by the 65% pass mark). */
export function Dial({ pct }: { pct: number }) {
  const r = 62
  const c = 2 * Math.PI * r
  const off = c * (1 - Math.min(pct, 100) / 100)
  const col = pct >= 65 ? 'var(--ok)' : 'var(--err)'
  return (
    <div style={{ position: 'relative', width: 150, height: 150, margin: '0 auto' }}>
      <svg width="150" height="150" role="img" aria-label={`Score ${pct}%`}>
        <circle cx="75" cy="75" r={r} fill="none" stroke="#eef1f6" strokeWidth="12" />
        <circle
          cx="75" cy="75" r={r} fill="none" stroke={col} strokeWidth="12" strokeLinecap="round"
          strokeDasharray={c} strokeDashoffset={off} transform="rotate(-90 75 75)"
        />
      </svg>
      <div style={{ position: 'absolute', inset: 0, display: 'grid', placeContent: 'center', textAlign: 'center' }}>
        <b style={{ fontSize: '1.6rem' }}>{pct}%</b>
        <span className="muted small">score</span>
      </div>
    </div>
  )
}

const BAND_LABEL: Record<string, string> = { above: 'Above target', at: 'At target', below: 'Below target' }
const BAND_COLOR: Record<string, string> = { above: 'var(--ok)', at: 'var(--brand)', below: 'var(--err)' }

/** Per-domain performance bands with a filled meter each. */
export function DomainBands({ breakdown }: { breakdown: DomainBand[] }) {
  if (!breakdown.length) return null
  return (
    <div style={{ display: 'grid', gap: '.6rem' }}>
      {breakdown.map((b) => (
        <div key={b.domain}>
          <div className="row" style={{ justifyContent: 'space-between', marginBottom: '.25rem' }}>
            <b className="small">{b.domain}</b>
            <span className="small" style={{ color: BAND_COLOR[b.band] || 'var(--slate)' }}>
              {BAND_LABEL[b.band] || b.band}
            </span>
          </div>
          <div style={{ height: 8, borderRadius: 6, background: '#eef1f6', overflow: 'hidden' }}>
            <i
              style={{
                display: 'block', height: '100%', width: `${Math.max(0, Math.min(100, b.pct))}%`,
                background: BAND_COLOR[b.band] || 'var(--slate)', borderRadius: 6,
                transition: 'width .8s var(--ease, ease)',
              }}
            />
          </div>
        </div>
      ))}
    </div>
  )
}
