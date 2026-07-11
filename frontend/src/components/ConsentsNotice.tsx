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

// Proctoring/recording consent captures special-category data (webcam video, audio, room scan)
// and must be given specifically and unbundled — GDPR Art. 7 and two-party recording law. It is
// collected as its own checked item, recorded distinctly from the commercial policy acceptances.
const PROCTORING_KEY = 'proctoring_consent'

// Link each consent to the most specific disclosure available; the rest fall back to the index.
const CONSENT_LINKS: Record<string, string> = {
  proctoring_consent: '/remote-proctoring.html',
  privacy_notice: '/data-protection-policy.html',
}
function linkFor(key: string): string {
  return CONSENT_LINKS[key] ?? '/policies.html'
}

/** In-portal consent review & acceptance — required before an exam can be scheduled.
 * (Previously this lived in the classic panel; it is now part of the dashboard.) */
export default function ConsentsNotice() {
  const { me, refetch } = useMe()
  const [open, setOpen] = useState(false)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const [proctorChecked, setProctorChecked] = useState(false)

  const outstanding = (me?.consents.outstanding ?? []).map(String)
  if (outstanding.length === 0) return null

  const needsProctoring = outstanding.includes(PROCTORING_KEY)
  const otherConsents = outstanding.filter((t) => t !== PROCTORING_KEY)

  async function record(keys: string[]) {
    if (keys.length === 0) return
    setBusy(true)
    setErr(null)
    try {
      await api.post('/api/me/consents', { accept: keys })
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

          {otherConsents.length > 0 && (
            <>
              <ul style={{ margin: '0 0 .6rem', paddingLeft: '1.1rem' }}>
                {otherConsents.map((t) => (
                  <li key={t} className="small">
                    {CONSENT_LABELS[t] ?? t.replace(/_/g, ' ')} —{' '}
                    <a href={linkFor(t)} target="_blank" rel="noreferrer">read the policy</a>
                  </li>
                ))}
              </ul>
              <button className="btn sm" disabled={busy} onClick={() => record(otherConsents)}>
                {busy ? 'Recording…' : 'I have read these — accept all'}
              </button>
            </>
          )}

          {needsProctoring && (
            <div style={{ marginTop: otherConsents.length > 0 ? '.9rem' : 0 }}>
              <label className="small" style={{ display: 'flex', gap: '.5rem', alignItems: 'flex-start' }}>
                <input
                  type="checkbox"
                  checked={proctorChecked}
                  onChange={(e) => setProctorChecked(e.target.checked)}
                  style={{ marginTop: '.2rem' }}
                />
                <span>
                  I explicitly consent to <strong>remote proctoring &amp; monitoring</strong> — the recording of my
                  webcam video, audio and a scan of my room during the exam. See the{' '}
                  <a href="/remote-proctoring.html" target="_blank" rel="noreferrer">remote proctoring</a> and{' '}
                  <a href="/data-protection-policy.html" target="_blank" rel="noreferrer">data-processing</a>{' '}
                  disclosures for exactly what is captured, how long it is kept and who can access it.
                </span>
              </label>
              <button
                className="btn sm"
                disabled={busy || !proctorChecked}
                onClick={() => record([PROCTORING_KEY])}
                style={{ marginTop: '.5rem' }}
              >
                {busy ? 'Recording…' : 'Give proctoring consent'}
              </button>
            </div>
          )}
        </div>
      )}
    </div>
  )
}
