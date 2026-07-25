import { describe, it, expect } from 'vitest'
import { render } from '@testing-library/react'
import ResourceChart from './ResourceChart'

// FE — the resource-loading SVG (round 3 dashboards). Pins: an accessible chart with a demand bar
// per period, an extra overload segment only where demand exceeds capacity, the overloaded periods
// named in the aria-label, and the empty guard.

const periods = [
  { period: 1, demand: 6, capacity: 8 },
  { period: 2, demand: 10, capacity: 8 },
  { period: 3, demand: 7, capacity: 8 },
]

describe('ResourceChart (loading vs capacity)', () => {
  it('renders an accessible chart and highlights only the overloaded periods', () => {
    const { container, getByRole } = render(<ResourceChart periods={periods} />)
    const svg = getByRole('img')
    expect(svg.getAttribute('aria-label')).toMatch(/peak demand 10/)
    expect(svg.getAttribute('aria-label')).toMatch(/capacity exceeded in 1 period \(2\)/)
    // 3 demand bars + 1 overload segment (period 2 only) = 4 rects
    expect(container.querySelectorAll('rect').length).toBe(4)
  })

  it('renders nothing for an empty plan', () => {
    const { container } = render(<ResourceChart periods={[]} />)
    expect(container.querySelector('svg')).toBeNull()
  })
})
