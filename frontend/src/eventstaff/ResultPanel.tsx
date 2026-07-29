import type { CSSProperties, ReactNode } from 'react'
import type { AdmissionDecision, AdmissionResult } from '../api/types'

// The four oversized gate outcomes (§7A.8). Each state is communicated three ways at once —
// TEXT + ICON + COLOUR — so it stays unambiguous for colour-blind staff, in glare, at arm's
// length. The panel only RENDERS a server decision; it never makes one.

interface ResultStyle {
  label: string
  icon: ReactNode
  bg: string
  fg: string
  border: string
}

const STYLES: Record<AdmissionResult, ResultStyle> = {
  admit: { label: 'ADMIT', icon: '✓', bg: '#dcfce7', fg: '#14532d', border: '#16a34a' },
  already_checked_in: { label: 'ALREADY CHECKED IN', icon: '↻', bg: '#fef9c3', fg: '#713f12', border: '#ca8a04' },
  manual_review: { label: 'MANUAL REVIEW', icon: '!', bg: '#e0f2fe', fg: '#0c4a6e', border: '#0284c7' },
  do_not_admit: { label: 'DO NOT ADMIT', icon: '✕', bg: '#fee2e2', fg: '#7f1d1d', border: '#dc2626' },
}

const REASON_COPY: Record<string, string> = {
  pass_already_used: 'This pass has already been used to enter.',
  payment_pending: 'Payment is still pending — send the attendee to the registration desk.',
  outside_entry_window: 'The entry window for this pass is not open.',
  pass_revoked: 'This pass has been revoked.',
  pass_replaced: 'This pass was replaced — ask the attendee to open their current pass.',
  pass_not_found: 'No pass matches this code.',
  registration_cancelled: 'The registration behind this pass was cancelled.',
}

export default function ResultPanel({ decision }: { decision: AdmissionDecision }) {
  const s = STYLES[decision.result]
  const panel: CSSProperties = {
    background: s.bg,
    color: s.fg,
    border: `4px solid ${s.border}`,
    borderRadius: 16,
    padding: '1.5rem',
    textAlign: 'center',
  }
  return (
    <div role="status" aria-live="assertive" data-testid={`result-${decision.result}`} style={panel}>
      <div aria-hidden="true" style={{ fontSize: '4.5rem', lineHeight: 1, fontWeight: 800 }}>
        {s.icon}
      </div>
      <div style={{ fontSize: '2.6rem', fontWeight: 900, letterSpacing: '.04em', marginTop: '.25rem' }}>
        {s.label}
      </div>

      {decision.reason && (
        <div style={{ fontSize: '1.1rem', marginTop: '.5rem' }}>
          {REASON_COPY[decision.reason] ?? decision.reason}
        </div>
      )}

      {decision.result === 'already_checked_in' && decision.firstEntryAt && (
        <div style={{ fontSize: '1.05rem', marginTop: '.35rem' }}>
          First entry: {decision.firstEntryAt}
          {decision.firstEntryGate ? ` at ${decision.firstEntryGate}` : ''}
        </div>
      )}

      {decision.attendee && (
        <div
          data-testid="attendee-details"
          style={{
            marginTop: '1rem',
            background: 'rgba(255,255,255,.65)',
            borderRadius: 10,
            padding: '.75rem',
            fontSize: '1.15rem',
          }}
        >
          <div style={{ fontWeight: 800 }}>{decision.attendee.displayName}</div>
          <div>Registration: {decision.attendee.registrationReference}</div>
          {decision.attendee.studentNumber && <div>Student Number: {decision.attendee.studentNumber}</div>}
        </div>
      )}
    </div>
  )
}
