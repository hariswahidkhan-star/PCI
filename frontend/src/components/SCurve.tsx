// PCI AI Project Controls Simulation Lab — S-curve (hand-authored SVG; no chart library).
// Plots cumulative Planned Value, Earned Value and Actual Cost over time. Purely presentational: it draws
// the GIVEN series only (never a computed answer), so it can sit beside a graded EVM task without leaking.
// Series are told apart by line STYLE as well as colour (colour-blind safe), and a collapsed data table
// provides the accessible non-visual alternative.

export interface EvmPoint { period: number; pv: number; ev: number; ac: number }

const SERIES = [
  { key: 'pv' as const, label: 'Planned Value', color: 'var(--sl-chart-baseline, #64748b)', dash: '6 4', swClass: 'dashed' },
  { key: 'ev' as const, label: 'Earned Value', color: 'var(--sl-chart-earned, #15803d)', dash: undefined, swClass: '' },
  { key: 'ac' as const, label: 'Actual Cost', color: 'var(--sl-chart-actual, #b45309)', dash: '2 3', swClass: 'dotted' },
]

const kfmt = (v: number) => (Math.abs(v) >= 1000 ? `${Math.round(v / 1000)}k` : String(Math.round(v)))
// A "nice" axis maximum a little above the data so the top line isn't clipped.
function niceMax(v: number): number {
  if (v <= 0) return 1
  const mag = Math.pow(10, Math.floor(Math.log10(v)))
  const n = v / mag
  const step = n <= 1 ? 1 : n <= 2 ? 2 : n <= 5 ? 5 : 10
  return step * mag
}

export default function SCurve({ series }: { series: EvmPoint[] }) {
  const pts = [...series].sort((a, b) => a.period - b.period)
  if (pts.length === 0) return null

  const W = 560, H = 240
  const pad = { l: 56, r: 16, t: 16, b: 28 }
  const plotW = W - pad.l - pad.r
  const plotH = H - pad.t - pad.b
  const n = pts.length
  const maxY = niceMax(Math.max(...pts.flatMap((p) => [p.pv, p.ev, p.ac])))

  const x = (i: number) => pad.l + (n === 1 ? plotW / 2 : (i / (n - 1)) * plotW)
  const y = (v: number) => pad.t + plotH - (v / maxY) * plotH
  const line = (key: 'pv' | 'ev' | 'ac') => pts.map((p, i) => `${x(i)},${y(p[key])}`).join(' ')

  const gridVals = [0, 0.25, 0.5, 0.75, 1].map((f) => f * maxY)
  const last = pts[n - 1]
  const label = `S-curve: Planned Value ${kfmt(last.pv)}, Earned Value ${kfmt(last.ev)}, Actual Cost ${kfmt(last.ac)} at period ${last.period}.`

  return (
    <figure style={{ margin: 0 }}>
      <div className="sl-legend">
        {SERIES.map((s) => (
          <span key={s.key} className="it" style={{ color: s.color }}>
            <span className={'sw' + (s.swClass ? ' ' + s.swClass : '')} aria-hidden="true" />
            <span style={{ color: 'inherit' }}>{s.label}</span>
          </span>
        ))}
      </div>
      <svg viewBox={`0 0 ${W} ${H}`} role="img" aria-label={label} style={{ width: '100%', height: 'auto', maxWidth: W }}>
        {/* horizontal gridlines + y labels */}
        {gridVals.map((v, i) => (
          <g key={i}>
            <line x1={pad.l} x2={W - pad.r} y1={y(v)} y2={y(v)} style={{ stroke: 'var(--sl-chart-grid, #e5e7eb)' }} strokeWidth={1} />
            <text x={pad.l - 8} y={y(v) + 4} textAnchor="end" fontSize={11} style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{kfmt(v)}</text>
          </g>
        ))}
        {/* x axis period ticks */}
        {pts.map((p, i) => (
          <text key={p.period} x={x(i)} y={H - 8} textAnchor="middle" fontSize={11} style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{p.period}</text>
        ))}
        {/* area under the Earned Value line */}
        <polygon
          points={`${x(0)},${y(0)} ${line('ev')} ${x(n - 1)},${y(0)}`}
          style={{ fill: 'var(--sl-chart-earned, #15803d)' }} fillOpacity={0.08} stroke="none"
        />
        {/* the three cumulative lines, differentiated by dash pattern as well as colour */}
        {SERIES.map((s) => (
          <polyline
            key={s.key}
            points={line(s.key)}
            fill="none"
            style={{ stroke: s.color }}
            strokeWidth={2}
            strokeDasharray={s.dash}
            strokeLinejoin="round"
            strokeLinecap="round"
          />
        ))}
        {/* emphasized endpoints */}
        {SERIES.map((s) => (
          <circle key={s.key} cx={x(n - 1)} cy={y(last[s.key])} r={3.5} style={{ fill: s.color }} />
        ))}
      </svg>
      <details className="sl-chart-data">
        <summary>View the S-curve as a table</summary>
        <table className="data">
          <caption className="sr-only">Cumulative Planned Value, Earned Value and Actual Cost by period</caption>
          <thead>
            <tr><th scope="col">Period</th><th scope="col">Planned Value</th><th scope="col">Earned Value</th><th scope="col">Actual Cost</th></tr>
          </thead>
          <tbody>
            {pts.map((p) => (
              <tr key={p.period}>
                <td>{p.period}</td>
                <td>{p.pv.toLocaleString()}</td>
                <td>{p.ev.toLocaleString()}</td>
                <td>{p.ac.toLocaleString()}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </details>
    </figure>
  )
}
