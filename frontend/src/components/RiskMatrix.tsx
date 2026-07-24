// PCI AI Project Controls Simulation Lab — probability × impact plot (hand-authored SVG).
// Impacts in the register are SIGNED: negative = threat, positive = opportunity, so this is a
// scatter across a zero-impact axis — threats sit in the red-washed left region, opportunities in
// the emerald-washed right region, probability on the vertical axis. Draws the GIVEN register
// only; the adjacent evidence table is the accessible alternative.

export interface RiskItem { id: string; name?: string; probability: number; impact: number }

const fmtN = (v: number) => (Math.abs(v) >= 1000 ? `${Math.round(v / 1000)}k` : String(Math.round(v)))

export default function RiskMatrix({ risks }: { risks: RiskItem[] }) {
  if (!risks || risks.length === 0) return null

  const W = 560, H = 250
  const pad = { l: 46, r: 16, t: 16, b: 30 }
  const plotW = W - pad.l - pad.r
  const plotH = H - pad.t - pad.b

  const maxAbs = Math.max(...risks.map((r) => Math.abs(r.impact)), 1)
  const maxP = Math.max(...risks.map((r) => r.probability), 1) // probability given on 0..1
  const x = (impact: number) => pad.l + ((impact + maxAbs) / (2 * maxAbs)) * plotW
  const y = (p: number) => pad.t + plotH - (p / maxP) * plotH

  const threats = risks.filter((r) => r.impact < 0)
  const opps = risks.filter((r) => r.impact >= 0)
  const byExposure = (a: RiskItem, b: RiskItem) => Math.abs(b.probability * b.impact) - Math.abs(a.probability * a.impact)
  const topThreat = [...threats].sort(byExposure)[0]
  const topOpp = [...opps].sort(byExposure)[0]
  const label = `Probability–impact plot of ${risks.length} risks.` +
    (topThreat ? ` Largest threat exposure ${topThreat.id}: probability ${topThreat.probability} × impact ${fmtN(topThreat.impact)}.` : '') +
    (topOpp ? ` Largest opportunity ${topOpp.id}: probability ${topOpp.probability} × impact ${fmtN(topOpp.impact)}.` : '')

  const danger = 'var(--sl-danger, #b91c1c)'
  const success = 'var(--sl-success, #15803d)'

  return (
    <figure style={{ margin: 0 }}>
      <div className="sl-legend">
        <span className="it" style={{ color: danger }}><span className="bx" style={{ background: danger }} aria-hidden="true" /> Threat</span>
        <span className="it" style={{ color: success }}><span className="bx" style={{ background: success }} aria-hidden="true" /> Opportunity</span>
        <span className="it">Vertical axis: probability · horizontal: impact</span>
      </div>
      <svg viewBox={`0 0 ${W} ${H}`} role="img" aria-label={label} style={{ width: '100%', height: 'auto', maxWidth: W }}>
        {/* threat / opportunity washes */}
        <rect x={pad.l} y={pad.t} width={plotW / 2} height={plotH} style={{ fill: 'var(--sl-danger-wash, #fef2f2)' }} />
        <rect x={pad.l + plotW / 2} y={pad.t} width={plotW / 2} height={plotH} style={{ fill: 'var(--sl-success-wash, #ecfdf3)' }} />
        {/* probability gridlines */}
        {[0.25, 0.5, 0.75].map((f) => (
          <line key={f} x1={pad.l} x2={W - pad.r} y1={y(f * maxP)} y2={y(f * maxP)}
            style={{ stroke: 'var(--sl-chart-grid, #e5e7eb)' }} strokeWidth={1} />
        ))}
        {/* zero-impact axis */}
        <line x1={x(0)} x2={x(0)} y1={pad.t} y2={pad.t + plotH} style={{ stroke: 'var(--sl-border-strong, #c9d5e8)' }} strokeWidth={1.5} />
        {/* axis labels */}
        {[0, 0.5, 1].map((f) => (
          <text key={f} x={pad.l - 6} y={y(f * maxP) + 4} textAnchor="end" fontSize={10}
            style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{(f * maxP).toFixed(1)}</text>
        ))}
        <text x={x(-maxAbs)} y={H - 8} textAnchor="start" fontSize={10} style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{fmtN(-maxAbs)}</text>
        <text x={x(0)} y={H - 8} textAnchor="middle" fontSize={10} style={{ fill: 'var(--sl-chart-axis, #94a3b8)' }}>0</text>
        <text x={x(maxAbs)} y={H - 8} textAnchor="end" fontSize={10} style={{ fill: 'var(--sl-chart-axis, #94a3b8)', fontVariantNumeric: 'tabular-nums' }}>{fmtN(maxAbs)}</text>
        {/* risk dots + ids */}
        {risks.map((r) => (
          <g key={r.id}>
            <circle cx={x(r.impact)} cy={y(r.probability)} r={5}
              style={{ fill: r.impact < 0 ? danger : success }} fillOpacity={0.85} />
            <text x={x(r.impact) + 8} y={y(r.probability) + 4} fontSize={10.5} fontWeight={700}
              style={{ fill: '#334155' }}>{r.id}</text>
          </g>
        ))}
      </svg>
    </figure>
  )
}
