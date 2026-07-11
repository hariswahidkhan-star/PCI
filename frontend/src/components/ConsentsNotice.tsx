import { useState } from 'react'
import { api } from '../api/client'
import { useMe } from '../data/MeContext'

// Keys MUST match Lifecycle.RequiredConsents on the backend, or the label falls through to a
// generic de-underscored string.
const CONSENT_LABELS: Record<string, string> = {
  candidate_handbook: 'Candidate handbook',
  exam_rules: 'Examination rules & candidate agreement',
  proctoring_consent: 'Remote proctoring & monitoring consent',
  privacy_notice: 'Privacy & data-processing notice',
  refund_policy: 'Refund policy',
  misconduct_policy: 'Examination misconduct policy',
  credential_terms: 'Credential terms',
}

/** In-portal consent review & acceptance — required before an exam can be scheduled.
 * (Previously this lived in the classic panel; it is now part of the dashboard.) */
export default function ConsentsNotice() {
  const { me, refetch } = useMe()
  const [open, setOpen] = useState(false)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  const outstanding = (me?.consents.outstanding ?? []).map(String)
  if (outstanding.length === 0) return null

  async function acceptAll() {
    setBusy(true)
    setErr(null)
    try {
      await api.post('/api/me/consents', { accept: outstanding })
      refetch()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not record your consent. Please try again.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="notice warn">
      <div className="spread" style={{ flexWrap: 'wrap' }}>
        <span>
          <strong>{outstanding.length} consent{outstanding.length > 1 ? 's' : ''}</strong> to review before you can
          schedule an exam.
        </span>
        <button className="btn ghost sm" onClick={() => setOpen((v) => !v)}>{open ? 'Hide' : 'Review now'}</button>
      </div>
      {open && (
        <div className="fade-up" style={{ marginTop: '.6rem' }}>
          {err && <div className="notice err" role="alert" style={{ marginBottom: '.5rem' }}>{err}</div>}
          <ul style={{ margin: '0 0 .6rem', paddingLeft: '1.1rem' }}>
            {outstanding.map((t) => (
              <li key={t} className="small">
                {CONSENT_LABELS[t] ?? t.replace(/_/g, ' ')} —{' '}
                <a href="/policies.html" target="_blank" rel="noreferrer">read the policy</a>
              </li>
            ))}
          </ul>
          <button className="btn sm" disabled={busy} onClick={acceptAll}>
            {busy ? 'Recording…' : 'I have read these — accept all'}
          </button>
        </div>
      )}
    </div>
  )
}
