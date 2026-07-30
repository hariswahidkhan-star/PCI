import type { PassportState } from '../../api/types'
import './passport.css'

// Human labels for every Passport state — honest, and never a certification claim. The badge text
// itself carries the meaning (WCAG: never colour alone), and the machine-readable state rides
// along as data-state for tests and styling.
const LABELS: Record<PassportState, string> = {
  not_created: 'Not created yet',
  draft: 'Private draft',
  private: 'Private — ready to publish',
  published: 'Published',
  expired: 'Public link expired',
  suspended: 'Suspended',
}

export default function PassportStatus({ state }: { state: PassportState }) {
  return (
    <span className={`pp-status pp-status--${state}`} data-state={state}>
      <span className="pp-dot" aria-hidden="true" />
      {LABELS[state] ?? state}
    </span>
  )
}
