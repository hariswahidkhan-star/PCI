import { describe, it, expect } from 'vitest'
import { render } from '@testing-library/react'
import CashflowChart from './CashflowChart'

// FE — the cash-flow SVG (round 3 dashboards). Pins: an accessible chart with one net bar per
// period, a cumulative polyline, the peak funding exposure summarised in the aria-label, and the
// empty guard. The adjacent evidence table is the data alternative.

const periods = [
  { period: 1, inflow: 0, outflow: 40 },
  { period: 2, inflow: 20, outflow: 50 },
  { period: 3, inflow: 60, outflow: 45 },
  { period: 4, inflow: 80, outflow: 35 },
]

describe('CashflowChart (funding exposure)', () => {
  it('renders an accessible chart with a bar per period and the peak exposure summarised', () => {
    const { container, getByRole } = render(<CashflowChart periods={periods} />)
    const svg = getByRole('img')
    // cumulative: -40, -70, -55, -10 → peak funding exposure -70 at period 2
    expect(svg.getAttribute('aria-label')).toMatch(/peak funding exposure -70 at period 2/)
    expect(container.querySelectorAll('rect').length).toBe(4)      // one net bar per period
    expect(container.querySelectorAll('polyline').length).toBe(1)  // cumulative line
    expect(container.querySelectorAll('circle').length).toBe(4)    // cumulative points
  })

  it('renders nothing for an empty series', () => {
    const { container } = render(<CashflowChart periods={[]} />)
    expect(container.querySelector('svg')).toBeNull()
  })
})
