// PCI AI Project Controls Simulation Lab — resource-loading chart (hand-authored SVG).
// Demand per period as bars, with any demand above capacity highlighted as overload (red — a
// genuine delivery risk) and the capacity limit drawn as a dashed line. Draws the GIVEN periods
// only; the adjacent evidence table is the accessible alternative.

export interface ResourcePeriod { period: number; demand: number; capacity: number }

export default function ResourceChart({ periods }: { periods: ResourcePeriod[] }) {
  const pts = [...periods].sort((a, b) => a.period - b.period)
  if (pts.length === 0) return null

  const W = 560, H = 220
  const pad = { l: 44, r: 16, t: 14, b: 26 }
  const plotW = W - pad.l - pad.r
  const plotH = H - pad.t - pad.b
  const maxY = Math.max(...pts.flatMap((p) => [p.demand, p.capacity]), 1)
  const x = (i: number) => pad.l + ((i + 0.5) / pts.length) * plotW
  const y = (v: number) => pad.t + plotH - (v / maxY) * plotH
  const barW = Math.min(36, (plotW / pts.length) * 0.6)

  const overloaded = pts.filter((p) => p.demand > p.capacity)
  const peak = Math.max(...pts.map((p) => p.demand))
  const label = `Resource loading over ${pts.length} periods: peak demand ${peak}, ` +
    (overloaded.length > 0
      ? `capacity exceeded in ${overloaded.length} period${overloaded.length === 1 ? '' : 's'} (${overloaded.map((p) => p.period).join(', ')}).`
      : 'demand stays within capacity.')

  const info = 'var(--sl-chart-forecast, #0e7490)'
  const danger = 'var(--sl-chart-critical, #b91c1c)'
  const baseline = 'var(--sl-chart-baseline, #64748b)'

  return (
    <figure style={{ margin: 0 }}>
      <div className="sl-legend">
        <span className="it" style={{ color: info }}><span className="bx" style={{ background: info }} aria-hidden="true" /> Demand</span>
        <span className="it" style={{ color: danger }}><span className="bx" style={{ background: danger }} aria-hidden="true" /> Overload</span>
        <span className="it" style={{ color: baseline }}><span className="sw dashed" aria-hidden="true" /> Capacity</span>
      </div>
      <svg viewBox={`0 0 ${W} ${H}`} role="img" aria-label={label} style={{ width: '100%', height: 'auto', maxWidth: W }}>
        <line x1={pad.l} x2={W - pad.r} y1={y(0)} y2={y(0)} style={{ stroke: 'var(--sl-chart-grid, #e5e7eb)' }} strokeWidth={1} />
        {[0.5, 1].map((f) => (
          <text key={f} x={pad.l - 6} y={y(f * maxY) + 4} textAnchor="end" fontSize={10}
            style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{Math.round(f * maxY)}</text>
        ))}
        {/* demand bars, overload segment highlighted */}
        {pts.map((p, i) => {
          const within = Math.min(p.demand, p.capacity)
          return (
            <g key={p.period}>
              <rect x={x(i) - barW / 2} y={y(within)} width={barW} height={Math.max(0, y(0) - y(within))} rx={2}
                style={{ fill: info }} fillOpacity={0.8} />
              {p.demand > p.capacity && (
                <rect x={x(i) - barW / 2} y={y(p.demand)} width={barW} height={Math.max(2, y(within) - y(p.demand))} rx={2}
                  style={{ fill: danger }} fillOpacity={0.9} />
              )}
            </g>
          )
        })}
        {/* capacity step line */}
        <polyline
          points={pts.flatMap((p, i) => [`${x(i) - barW / 2 - 4},${y(p.capacity)}`, `${x(i) + barW / 2 + 4},${y(p.capacity)}`]).join(' ')}
          fill="none" style={{ stroke: baseline }} strokeWidth={2} strokeDasharray="5 4"
        />
        {/* period ticks */}
        {pts.map((p, i) => (
          <text key={p.period} x={x(i)} y={H - 8} textAnchor="middle" fontSize={10}
            style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{p.period}</text>
        ))}
      </svg>
    </figure>
  )
}
