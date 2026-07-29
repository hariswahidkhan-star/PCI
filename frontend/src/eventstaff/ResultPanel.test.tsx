import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import ResultPanel from './ResultPanel'
import type { AdmissionDecision } from '../api/types'

// The four oversized gate outcomes (§7A.8). Pinned: every state carries its TEXT label and an
// ICON alongside the colour — never colour alone — and the states that identify an attendee show
// the minimal identity block while do_not_admit shows none.

const base: Omit<AdmissionDecision, 'result'> = {
  decisionId: 'dec_1',
  reason: null,
  attendee: {
    displayName: 'Jordan Meyer',
    registrationReference: 'PCI-EV-2026-000731',
    studentNumber: 'PCI-2024-018233',
    passState: 'active',
  },
}

describe('ResultPanel (four result states)', () => {
  it('ADMIT renders text + icon', () => {
    render(<ResultPanel decision={{ ...base, result: 'admit' }} />)
    const panel = screen.getByTestId('result-admit')
    expect(panel).toHaveTextContent('ADMIT')
    expect(panel).toHaveTextContent('✓')
    expect(screen.getByText('Jordan Meyer')).toBeInTheDocument()
    expect(screen.getByText(/PCI-2024-018233/)).toBeInTheDocument()
  })

  it('ALREADY CHECKED IN renders text + icon and the first-entry details', () => {
    render(
      <ResultPanel
        decision={{
          ...base,
          result: 'already_checked_in',
          reason: 'pass_already_used',
          firstEntryAt: '2026-05-02 08:12:00',
          firstEntryGate: 'Hall 4 — Gate A',
        }}
      />,
    )
    const panel = screen.getByTestId('result-already_checked_in')
    expect(panel).toHaveTextContent('ALREADY CHECKED IN')
    expect(panel).toHaveTextContent('↻')
    expect(panel).toHaveTextContent('First entry: 2026-05-02 08:12:00 at Hall 4 — Gate A')
    expect(panel).toHaveTextContent('This pass has already been used to enter.')
  })

  it('MANUAL REVIEW renders text + icon and the operator reason', () => {
    render(<ResultPanel decision={{ ...base, result: 'manual_review', reason: 'payment_pending' }} />)
    const panel = screen.getByTestId('result-manual_review')
    expect(panel).toHaveTextContent('MANUAL REVIEW')
    expect(panel).toHaveTextContent('!')
    expect(panel).toHaveTextContent('Payment is still pending')
  })

  it('DO NOT ADMIT renders text + icon and no attendee block when there is no match', () => {
    render(
      <ResultPanel
        decision={{ decisionId: 'dec_2', result: 'do_not_admit', reason: 'pass_not_found', attendee: null }}
      />,
    )
    const panel = screen.getByTestId('result-do_not_admit')
    expect(panel).toHaveTextContent('DO NOT ADMIT')
    expect(panel).toHaveTextContent('✕')
    expect(panel).toHaveTextContent('No pass matches this code.')
    expect(screen.queryByTestId('attendee-details')).toBeNull()
  })

  it('announces the outcome to assistive tech (role=status, assertive)', () => {
    render(<ResultPanel decision={{ ...base, result: 'admit' }} />)
    const panel = screen.getByRole('status')
    expect(panel).toHaveAttribute('aria-live', 'assertive')
  })
})
