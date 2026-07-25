// PCI AI Project Controls Simulation Lab — Gantt chart (hand-authored SVG; no chart library).
// Renders the computed schedule (early start → early finish per activity) with critical activities
// highlighted and total float shown as a lighter tail out to the late finish. Shown only in the result
// view once an attempt has been graded — it is the schedule the student was asked to work out.
// A collapsed data table provides the accessible non-visual alternative.

export interface GanttBar {
  id: string
  duration: number
  es: number
  ef: number
  ls: number
  lf: number
  total_float: number
  critical: boolean
}

const critColor = 'var(--sl-chart-critical, #b91c1c)'
const barColor = 'var(--sl-chart-forecast, #0e7490)'
const floatColor = 'var(--sl-chart-float, #cbd5e1)'

export default function Gantt({ bars, projectDuration }: { bars: GanttBar[]; projectDuration: number }) {
  if (!bars || bars.length === 0) return null
  const span = Math.max(projectDuration, ...bars.map((b) => b.lf), 1)

  const rowH = 26
  const pad = { l: 44, r: 16, t: 8, b: 22 }
  const W = 560
  const plotW = W - pad.l - pad.r
  const H = pad.t + pad.b + bars.length * rowH
  const x = (t: number) => pad.l + (t / span) * plotW
  const rowY = (i: number) => pad.t + i * rowH

  // Vertical time gridlines at whole-unit steps (kept sparse for wide spans).
  const step = span <= 12 ? 1 : span <= 30 ? 5 : 10
  const ticks: number[] = []
  for (let t = 0; t <= span; t += step) ticks.push(t)

  const label = `Gantt chart: ${bars.length} activities, project duration ${projectDuration}. Critical activities: ${bars.filter((b) => b.critical).map((b) => b.id).join(', ') || 'none'}.`

  return (
    <figure style={{ margin: 0 }}>
      <div className="sl-legend">
        <span className="it" style={{ color: critColor }}><span className="bx" style={{ background: critColor }} aria-hidden="true" /> Critical</span>
        <span className="it" style={{ color: barColor }}><span className="bx" style={{ background: barColor }} aria-hidden="true" /> Activity</span>
        <span className="it"><span className="bx" style={{ background: floatColor }} aria-hidden="true" /> Total float</span>
      </div>
      <svg viewBox={`0 0 ${W} ${H}`} role="img" aria-label={label} style={{ width: '100%', height: 'auto', maxWidth: W }}>
        {ticks.map((t) => (
          <g key={t}>
            <line x1={x(t)} x2={x(t)} y1={pad.t} y2={H - pad.b} style={{ stroke: 'var(--sl-chart-grid, #eef2f7)' }} strokeWidth={1} />
            <text x={x(t)} y={H - 6} textAnchor="middle" fontSize={10} style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{t}</text>
          </g>
        ))}
        {bars.map((b, i) => {
          const y = rowY(i) + 4
          const h = rowH - 12
          return (
            <g key={b.id}>
              <text x={pad.l - 8} y={y + h - 1} textAnchor="end" fontSize={11} style={{ fill: '#334155' }}>{b.id}</text>
              {b.total_float > 0 && (
                <rect x={x(b.ef)} y={y + h / 2 - 2} width={Math.max(0, x(b.lf) - x(b.ef))} height={4} style={{ fill: floatColor }} rx={2} />
              )}
              <rect x={x(b.es)} y={y} width={Math.max(2, x(b.ef) - x(b.es))} height={h} rx={3}
                style={{ fill: b.critical ? critColor : barColor }} />
            </g>
          )
        })}
      </svg>
      <details className="sl-chart-data">
        <summary>View the schedule as a table</summary>
        <table className="data">
          <caption className="sr-only">Computed schedule: early/late dates, total float and criticality per activity</caption>
          <thead>
            <tr>
              <th scope="col">Activity</th><th scope="col">Duration</th><th scope="col">Early start</th>
              <th scope="col">Early finish</th><th scope="col">Late start</th><th scope="col">Late finish</th>
              <th scope="col">Total float</th><th scope="col">Critical</th>
            </tr>
          </thead>
          <tbody>
            {bars.map((b) => (
              <tr key={b.id}>
                <td>{b.id}</td><td>{b.duration}</td><td>{b.es}</td><td>{b.ef}</td>
                <td>{b.ls}</td><td>{b.lf}</td><td>{b.total_float}</td><td>{b.critical ? 'Yes' : 'No'}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </details>
    </figure>
  )
}
