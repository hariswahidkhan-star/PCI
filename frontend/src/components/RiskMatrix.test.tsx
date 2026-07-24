import { describe, it, expect } from 'vitest'
import { render } from '@testing-library/react'
import RiskMatrix from './RiskMatrix'

// FE — the probability × impact SVG (round 3 dashboards). Pins: an accessible plot with one dot
// per risk, signed impacts split across the zero axis (threats vs opportunities), the largest
// exposures named in the aria-label, and the empty guard.

const risks = [
  { id: 'R1', name: 'Rig stranded', probability: 0.3, impact: -20000 },
  { id: 'R2', name: 'Beam damaged', probability: 0.2, impact: -15000 },
  { id: 'R3', name: 'Supplier discount', probability: 0.5, impact: 8000 },
  { id: 'R4', name: 'Nesting stoppage', probability: 0.1, impact: -40000 },
]

describe('RiskMatrix (probability × impact)', () => {
  it('renders an accessible plot with a dot per risk and names the largest exposures', () => {
    const { container, getByRole } = render(<RiskMatrix risks={risks} />)
    const svg = getByRole('img')
    // exposures: R1 -6000, R2 -3000, R3 +4000, R4 -4000 → top threat R1, top opportunity R3
    expect(svg.getAttribute('aria-label')).toMatch(/Largest threat exposure R1/)
    expect(svg.getAttribute('aria-label')).toMatch(/Largest opportunity R3/)
    expect(container.querySelectorAll('circle').length).toBe(4)   // one dot per risk
  })

  it('renders nothing for an empty register', () => {
    const { container } = render(<RiskMatrix risks={[]} />)
    expect(container.querySelector('svg')).toBeNull()
  })
})
