// PCI AI Project Controls Simulation Lab — Monte-Carlo histogram (hand-authored SVG; no chart library).
// Shows the distribution of simulated finish dates with median (P50) and P80 confidence markers. Purely
// presentational: it draws the simulation OUTPUT computed server-side from the given inputs (a seeded,
// deterministic run) — not an answer to any graded question. A collapsed data table provides the
// accessible non-visual alternative.

export interface McBucket { lo: number; hi: number; count: number }
export interface Mc {
  iterations: number; mean: number; min: number; max: number
  p10: number; p50: number; p80: number; p90: number
  histogram: McBucket[]
}

const n = (v: number) => (Number.isInteger(v) ? String(v) : v.toFixed(1))

export default function Histogram({ mc }: { mc: Mc }) {
  const bins = mc.histogram ?? []
  if (bins.length === 0) return null

  const W = 560, H = 220
  const pad = { l: 40, r: 14, t: 12, b: 30 }
  const plotW = W - pad.l - pad.r
  const plotH = H - pad.t - pad.b
  const lo = mc.min, hi = mc.max
  const span = hi > lo ? hi - lo : 1
  const maxCount = Math.max(...bins.map((b) => b.count), 1)
  const x = (v: number) => pad.l + ((v - lo) / span) * plotW
  const yTop = (count: number) => pad.t + plotH - (count / maxCount) * plotH

  const label = `Monte-Carlo of the finish date over ${mc.iterations} iterations: median (P50) ${n(mc.p50)}, P80 ${n(mc.p80)}, mean ${n(mc.mean)}, range ${n(mc.min)} to ${n(mc.max)}.`

  return (
    <figure style={{ margin: 0 }}>
      <div className="sl-legend" style={{ fontVariantNumeric: 'tabular-nums' }}>
        <span className="it">Monte-Carlo · {mc.iterations.toLocaleString()} runs</span>
        <span className="it" style={{ color: 'var(--sl-chart-earned, #15803d)' }}>P50 {n(mc.p50)}</span>
        <span className="it" style={{ color: 'var(--sl-chart-actual, #b45309)' }}>P80 {n(mc.p80)}</span>
        <span className="it">P90 {n(mc.p90)}</span>
      </div>
      <svg viewBox={`0 0 ${W} ${H}`} role="img" aria-label={label} style={{ width: '100%', height: 'auto', maxWidth: W }}>
        <line x1={pad.l} x2={W - pad.r} y1={pad.t + plotH} y2={pad.t + plotH} style={{ stroke: 'var(--sl-chart-grid, #e5e7eb)' }} strokeWidth={1} />
        {/* bars */}
        {bins.map((b, i) => {
          const bx = x(b.lo)
          const bw = Math.max(1, x(b.hi) - x(b.lo) - 1)
          const by = yTop(b.count)
          return <rect key={i} x={bx} y={by} width={bw} height={pad.t + plotH - by} style={{ fill: 'var(--sl-chart-forecast, #0e7490)' }} fillOpacity={0.75} rx={1} />
        })}
        {/* P50 / P80 markers */}
        {([['P50', mc.p50, 'var(--sl-chart-earned, #15803d)'], ['P80', mc.p80, 'var(--sl-chart-actual, #b45309)']] as const).map(([lbl, v, col]) => (
          <g key={lbl}>
            <line x1={x(v)} x2={x(v)} y1={pad.t} y2={pad.t + plotH} style={{ stroke: col }} strokeWidth={1.5} strokeDasharray="4 3" />
            <text x={x(v)} y={pad.t - 2} textAnchor="middle" fontSize={10} style={{ fill: col }}>{lbl}</text>
          </g>
        ))}
        {/* x-axis endpoints */}
        <text x={pad.l} y={H - 8} textAnchor="start" fontSize={10} style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{n(mc.min)}</text>
        <text x={W - pad.r} y={H - 8} textAnchor="end" fontSize={10} style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{n(mc.max)}</text>
      </svg>
      <details className="sl-chart-data">
        <summary>View the distribution as a table</summary>
        <table className="data">
          <caption className="sr-only">Monte-Carlo simulation buckets and confidence levels</caption>
          <thead>
            <tr><th scope="col">From</th><th scope="col">To</th><th scope="col">Runs</th></tr>
          </thead>
          <tbody>
            {bins.map((b, i) => (
              <tr key={i}><td>{n(b.lo)}</td><td>{n(b.hi)}</td><td>{b.count.toLocaleString()}</td></tr>
            ))}
          </tbody>
          <tfoot>
            <tr><td colSpan={3}>Mean {n(mc.mean)} · P10 {n(mc.p10)} · P50 {n(mc.p50)} · P80 {n(mc.p80)} · P90 {n(mc.p90)}</td></tr>
          </tfoot>
        </table>
      </details>
    </figure>
  )
}
