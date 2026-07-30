import type { ReactNode } from 'react'
import './passport.css'

export type PassportEmptyVariant = 'create' | 'link' | 'unavailable'

const COPY: Record<PassportEmptyVariant, { title: string; body: string }> = {
  create: {
    title: 'Start your PCI World Passport',
    body:
      'Complete free PCI World practice challenges and collect them as verifiable practice ' +
      'evidence — you choose exactly what is shared, and nothing is public until you publish. ' +
      'Practice evidence, never a certification claim.',
  },
  link: {
    title: 'A PCI World account is waiting to be linked',
    body:
      'A PCI World account was created with your email address but has not yet been linked to ' +
      'your PCI identity. Open PCI World with this login to prove ownership and bring your ' +
      'practice evidence together — nothing merges until you do.',
  },
  unavailable: {
    title: 'Passport temporarily unavailable',
    body:
      'Your PCI World Passport could not be loaded just now. Everything else in MyPCI is ' +
      'unaffected — try again shortly, or open PCI World directly.',
  },
}

/** The Passport module when there is no card to show yet: create onboarding, the link-pending
 *  journey, or a graceful temporarily-unavailable note. Same premium frame, quieter contents. */
export default function PassportEmptyState({
  variant,
  actions,
}: {
  variant: PassportEmptyVariant
  actions?: ReactNode
}) {
  const c = COPY[variant]
  return (
    <section className="pp-card pp-empty" aria-label="PCI World Passport">
      <div className="pp-head">
        <div>
          <p className="pp-eyebrow">PCI World · Practice Passport</p>
          <h3 className="pp-empty-title">{c.title}</h3>
        </div>
        <div className="pp-seal" aria-hidden="true">PW</div>
      </div>
      <p className="pp-empty-body">{c.body}</p>
      {actions && <div className="pp-actions">{actions}</div>}
    </section>
  )
}
