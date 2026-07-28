import type { ReactNode } from 'react'
import type { PassportSummary } from '../../api/types'
import PassportStatus from './PassportStatus'
import './passport.css'

/** Join the always-disclosed metadata of a highlight into one muted line. */
function meta(parts: Array<string | null | undefined>): string {
  return parts.filter((p): p is string => !!p && p.length > 0).join(' · ')
}

/**
 * The shared Passport card — the SAME data contract on every surface (PassportSummary from
 * /api/me/world-passport/summary, or an equivalent mapping on the World side). Purely
 * presentational: the host surface supplies the buttons (`actions`) and any editor content
 * (`footer`), so MyPCI's SSO behaviour and the World manager's publish/withdraw flow both fit
 * without this component knowing either. Restrained by design: deep navy, warm ivory, one gilt
 * accent, no motion beyond a reduced-motion-respecting fade.
 */
export default function PassportCard({
  summary,
  actions,
  footer,
}: {
  summary: PassportSummary
  actions?: ReactNode
  footer?: ReactNode
}) {
  const s = summary
  return (
    <section className="pp-card" aria-label="PCI World Passport">
      <div className="pp-head">
        <div>
          <p className="pp-eyebrow">PCI World · Practice Passport</p>
          <h3 className="pp-name">{s.displayName || 'Your Practice Passport'}</h3>
          {s.studentNumber && (
            <p className="pp-number" aria-label={`PCI Student Number ${s.studentNumber}`}>
              {s.studentNumber}
            </p>
          )}
          <PassportStatus state={s.passportState} />
        </div>
        <div className="pp-seal" aria-hidden="true">PW</div>
      </div>

      <dl className="pp-stats">
        <div className="pp-stat">
          <dt className="pp-stat-k">Challenges completed</dt>
          <dd className="pp-stat-n">{s.completedChallengeCount}</dd>
        </div>
        <div className="pp-stat">
          <dt className="pp-stat-k">Selected for Passport</dt>
          <dd className="pp-stat-n">{s.selectedEvidenceCount}</dd>
        </div>
        {s.expiresAt && (
          <div className="pp-stat">
            <dt className="pp-stat-k">Link expires</dt>
            <dd className="pp-stat-n" style={{ fontSize: '1rem', lineHeight: '1.9' }}>{s.expiresAt}</dd>
          </div>
        )}
      </dl>

      {s.evidenceHighlights.length > 0 && (
        <ul className="pp-highlights" aria-label="Selected evidence highlights">
          {s.evidenceHighlights.map((h) => (
            <li key={h.reference + h.title}>
              <div className="pp-hl-title">{h.title}</div>
              <div className="pp-hl-meta">
                {meta([h.reference, h.industry, h.difficulty, h.profile, h.completedOn])}
                {typeof h.score === 'number' && (
                  <>
                    {' '}
                    <span className="pp-hl-score">score {h.score}</span>
                  </>
                )}
              </div>
            </li>
          ))}
        </ul>
      )}

      {s.passportState === 'published' && s.canShowPublicUrl && s.publicUrl && (
        <p className="pp-note">
          Public link:{' '}
          <a href={s.publicUrl} style={{ color: 'var(--pp-gilt)' }}>
            {s.publicUrl}
          </a>
        </p>
      )}
      {s.passportState === 'published' && !s.canShowPublicUrl && (
        <p className="pp-note">
          Your Passport is published. Open it in PCI World to view or share the public link.
        </p>
      )}
      {s.passportState === 'suspended' && (
        <p className="pp-note">
          Your PCI World participation is suspended — your PCI student account is unaffected.
        </p>
      )}

      {actions && <div className="pp-actions">{actions}</div>}
      {footer}
    </section>
  )
}
