import { describe, it, expect } from 'vitest'
import { render } from '@testing-library/react'
import Histogram from './Histogram'

// FE — the Monte-Carlo histogram SVG (Phase 3 dashboards). Pins: an accessible chart with one bar per
// bucket, P50/P80 markers in the aria-label summary, and the empty guard.
const mc = {
  iterations: 4000, mean: 19.3, min: 14, max: 27,
  p10: 16.2, p50: 19.1, p80: 21.6, p90: 22.9,
  histogram: [
    { lo: 14, hi: 17, count: 900 },
    { lo: 17, hi: 20, count: 1800 },
    { lo: 20, hi: 24, count: 1000 },
    { lo: 24, hi: 27, count: 300 },
  ],
}

describe('Histogram (Monte-Carlo distribution)', () => {
  it('renders an accessible chart with a bar per bucket and P50/P80 in the summary', () => {
    const { container, getByRole } = render(<Histogram mc={mc} />)
    const svg = getByRole('img')
    expect(svg.getAttribute('aria-label')).toMatch(/Monte-Carlo/)
    expect(svg.getAttribute('aria-label')).toMatch(/median \(P50\) 19.1/)
    expect(container.querySelectorAll('rect').length).toBe(4)   // one bar per bucket
  })

  it('renders nothing when there is no histogram', () => {
    const { container } = render(<Histogram mc={{ ...mc, histogram: [] }} />)
    expect(container.querySelector('svg')).toBeNull()
  })
})
