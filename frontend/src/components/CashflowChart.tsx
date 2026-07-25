// PCI AI Project Controls Simulation Lab — cash-flow chart (hand-authored SVG; no chart library).
// Net cash flow per period as bars (emerald inflow-positive, amber outflow-negative) with the
// cumulative position as a line — the funding-exposure story the cashflow engine asks the student
// to read. Draws the GIVEN periods only; the full figures sit in the adjacent evidence table,
// which doubles as the accessible alternative.

export interface CashPeriod { period: number; inflow: number; outflow: number }

const fmtN = (v: number) => (Math.abs(v) >= 1000 ? `${Math.round(v / 1000)}k` : String(Math.round(v)))

export default function CashflowChart({ periods }: { periods: CashPeriod[] }) {
  const pts = [...periods].sort((a, b) => a.period - b.period)
  if (pts.length === 0) return null

  const nets = pts.map((p) => p.inflow - p.outflow)
  const cum: number[] = []
  nets.reduce((acc, n) => { const c = acc + n; cum.push(c); return c }, 0)

  const W = 560, H = 230
  const pad = { l: 48, r: 16, t: 14, b: 26 }
  const plotW = W - pad.l - pad.r
  const plotH = H - pad.t - pad.b
  const all = [...nets, ...cum, 0]
  const lo = Math.min(...all), hi = Math.max(...all)
  const span = hi - lo || 1
  const x = (i: number) => pad.l + ((i + 0.5) / pts.length) * plotW
  const y = (v: number) => pad.t + plotH - ((v - lo) / span) * plotH
  const barW = Math.min(34, (plotW / pts.length) * 0.55)

  const peakExposure = Math.min(...cum)
  const peakPeriod = pts[cum.indexOf(peakExposure)]?.period
  const label = `Cash flow: peak funding exposure ${fmtN(peakExposure)} at period ${peakPeriod}, closing position ${fmtN(cum[cum.length - 1])} after ${pts.length} periods.`

  const success = 'var(--sl-success, #15803d)'
  const warning = 'var(--sl-warning, #b45309)'
  const accent = 'var(--sl-accent, #1d4ed8)'

  return (
    <figure style={{ margin: 0 }}>
      <div className="sl-legend">
        <span className="it" style={{ color: success }}><span className="bx" style={{ background: success }} aria-hidden="true" /> Net inflow</span>
        <span className="it" style={{ color: warning }}><span className="bx" style={{ background: warning }} aria-hidden="true" /> Net outflow</span>
        <span className="it" style={{ color: accent }}><span className="sw" aria-hidden="true" /> Cumulative position</span>
      </div>
      <svg viewBox={`0 0 ${W} ${H}`} role="img" aria-label={label} style={{ width: '100%', height: 'auto', maxWidth: W }}>
        {/* zero axis + y extremes */}
        <line x1={pad.l} x2={W - pad.r} y1={y(0)} y2={y(0)} style={{ stroke: 'var(--sl-border-strong, #c9d5e8)' }} strokeWidth={1} />
        {[lo, hi].map((v) => (
          <text key={v} x={pad.l - 6} y={y(v) + 4} textAnchor="end" fontSize={10}
            style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{fmtN(v)}</text>
        ))}
        {/* net bars */}
        {nets.map((n, i) => {
          const top = Math.min(y(0), y(n))
          const h = Math.max(2, Math.abs(y(n) - y(0)))
          return (
            <rect key={i} x={x(i) - barW / 2} y={top} width={barW} height={h} rx={2}
              style={{ fill: n >= 0 ? success : warning }} fillOpacity={0.8} />
          )
        })}
        {/* cumulative line + endpoints */}
        <polyline
          points={cum.map((c, i) => `${x(i)},${y(c)}`).join(' ')}
          fill="none" style={{ stroke: accent }} strokeWidth={2} strokeLinejoin="round" strokeLinecap="round"
        />
        {cum.map((c, i) => <circle key={i} cx={x(i)} cy={y(c)} r={2.6} style={{ fill: accent }} />)}
        {/* period ticks */}
        {pts.map((p, i) => (
          <text key={p.period} x={x(i)} y={H - 8} textAnchor="middle" fontSize={10}
            style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{p.period}</text>
        ))}
      </svg>
    </figure>
  )
}
