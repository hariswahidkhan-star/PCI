import { describe, it, expect } from 'vitest'
import { render } from '@testing-library/react'
import SCurve from './SCurve'

// FE — the hand-authored S-curve SVG (Phase 2 dashboards). Pins: it renders an accessible SVG that
// summarises the final PV/EV/AC in its aria-label, draws one polyline per series, and marks the three
// endpoints — from the GIVEN series only (no computed answer). Empty input renders nothing.

const series = [
  { period: 1, pv: 100000, ev: 90000, ac: 95000 },
  { period: 2, pv: 220000, ev: 195000, ac: 205000 },
  { period: 3, pv: 340000, ev: 300000, ac: 320000 },
]

describe('SCurve (S-curve dashboard)', () => {
  it('renders an accessible chart with a line per series and endpoint markers', () => {
    const { container, getByRole } = render(<SCurve series={series} />)
    const svg = getByRole('img')
    expect(svg.getAttribute('aria-label')).toMatch(/Planned Value/)
    expect(svg.getAttribute('aria-label')).toMatch(/340k/)          // final PV summarised
    expect(container.querySelectorAll('polyline').length).toBe(3)     // PV / EV / AC lines
    expect(container.querySelectorAll('circle').length).toBe(3)       // three endpoints
  })

  it('renders nothing for an empty series', () => {
    const { container } = render(<SCurve series={[]} />)
    expect(container.querySelector('svg')).toBeNull()
  })
})
