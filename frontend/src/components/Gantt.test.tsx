import { describe, it, expect } from 'vitest'
import { render } from '@testing-library/react'
import Gantt from './Gantt'

// FE — the hand-authored Gantt SVG (Phase 2 dashboards). Pins: an accessible SVG with one bar per
// activity, the critical activities named in the aria-label, a float tail only where float > 0, and the
// empty guard. Shown post-grade in the result view (it is the schedule the student worked out).

const bars = [
  { id: 'A', duration: 3, es: 0, ef: 3, ls: 0, lf: 3, total_float: 0, critical: true },
  { id: 'C', duration: 2, es: 3, ef: 5, ls: 5, lf: 7, total_float: 2, critical: false },
]

describe('Gantt (schedule dashboard)', () => {
  it('renders an accessible chart with a bar per activity and names the critical path', () => {
    const { container, getByRole } = render(<Gantt bars={bars} projectDuration={13} />)
    const svg = getByRole('img')
    expect(svg.getAttribute('aria-label')).toMatch(/project duration 13/)
    expect(svg.getAttribute('aria-label')).toMatch(/Critical activities: A/)
    // one activity bar per row + one float tail (only C has float) = 3 rects
    expect(container.querySelectorAll('rect').length).toBe(3)
  })

  it('renders nothing for an empty schedule', () => {
    const { container } = render(<Gantt bars={[]} projectDuration={0} />)
    expect(container.querySelector('svg')).toBeNull()
  })
})
