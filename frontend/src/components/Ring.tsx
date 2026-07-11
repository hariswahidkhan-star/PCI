/** Animated completeness ring — pure SVG, no dependencies. Respects reduced motion via CSS. */
export default function Ring({ value, size = 92, label }: { value: number; size?: number; label?: string }) {
  const v = Math.max(0, Math.min(100, Math.round(value)))
  const r = (size - 10) / 2
  const c = 2 * Math.PI * r
  return (
    <div className="ring-wrap" role="img" aria-label={`${label ?? 'Progress'}: ${v}%`}>
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`}>
        <circle cx={size / 2} cy={size / 2} r={r} fill="none" stroke="var(--line)" strokeWidth="8" />
        <circle
          cx={size / 2}
          cy={size / 2}
          r={r}
          fill="none"
          stroke="url(#ringGrad)"
          strokeWidth="8"
          strokeLinecap="round"
          strokeDasharray={c}
          strokeDashoffset={c - (c * v) / 100}
          transform={`rotate(-90 ${size / 2} ${size / 2})`}
          className="ring-arc"
        />
        <defs>
          <linearGradient id="ringGrad" x1="0" y1="0" x2="1" y2="1">
            <stop offset="0%" stopColor="#3b82f6" />
            <stop offset="100%" stopColor="#1d4ed8" />
          </linearGradient>
        </defs>
      </svg>
      <div className="ring-num">
        <strong>{v}%</strong>
        {label && <span className="small muted">{label}</span>}
      </div>
    </div>
  )
}
