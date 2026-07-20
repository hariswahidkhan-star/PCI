import { useState } from 'react'
import { useQuery } from '../api/hooks'
import { api, ApiError } from '../api/client'
import { Card, Badge } from '../components/ui'
import { fmtDate } from '../format'

// Certuvo — the student's access page for PCI's external study & practice platform. This portal shares only
// the Certuvo access credentials; all practice questions, mock exams and study tools live inside Certuvo.
// The practice API endpoints remain available server-side and are simply not rendered here.

// The member's external Certuvo practice-platform account (credentials shared automatically after membership).
interface CertuvoAccess {
  enabled: boolean
  status?: string
  /** friendly student-facing copy set whenever the account is not active — never a raw error */
  message?: string | null
  username?: string | null
  password?: string | null
  must_change_password?: boolean
  notice?: string | null
  login_url?: string | null
  /** platform sign-in URL, present whenever the operator has configured it (even before the account is active) */
  portal_url?: string | null
  provisioned_at?: string | null
  activated_at?: string | null
  credentials_sent_at?: string | null
  expires?: string | null
}
function CertuvoAccessPanel() {
  const { data } = useQuery<CertuvoAccess>('/api/me/certuvo/access')
  const [resending, setResending] = useState(false)
  const [resendNote, setResendNote] = useState<{ ok: boolean; text: string } | null>(null)
  if (!data || !data.enabled) return null
  const status = data.status ?? ''
  const active = status === 'active'
  const badge = active
    ? <Badge tone="ok">Ready</Badge>
    : status === 'suspended' || status === 'revoked'
      ? <Badge tone="warn">Unavailable</Badge>
      : <Badge tone="warn">{status === 'not_provisioned' ? 'After membership' : 'Being set up'}</Badge>

  async function resend() {
    setResending(true)
    setResendNote(null)
    try {
      await api.post('/api/me/certuvo/resend', {})
      setResendNote({ ok: true, text: 'Access instructions sent to your email.' })
    } catch (e) {
      const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : null
      setResendNote({
        ok: false,
        text: e instanceof ApiError && e.status === 429
          ? 'Please wait a while before resending.'
          : (typeof body?.message === 'string' && body.message) || 'Could not resend the instructions. Please try again later.',
      })
    } finally {
      setResending(false)
    }
  }

  // The link a student uses to reach Certuvo: their own account URL once active, otherwise the platform sign-in.
  const certuvoUrl = data.login_url || data.portal_url || null

  return (
    <Card title="Your Certuvo practice access" action={badge}>
      {active ? (
        <div style={{ display: 'grid', gap: '.4rem' }}>
          <p className="muted small" style={{ margin: 0 }}>Practice on the Certuvo platform with the account we created for you. Your progress there is separate from this portal.</p>
          <div className="small">Username: <strong>{data.username}</strong></div>
          {data.password && <div className="small">Temporary password: <strong>{data.password}</strong></div>}
          {data.must_change_password && <div className="small muted">You'll be asked to set your own password the first time you sign in to Certuvo.</div>}
          {data.provisioned_at && <div className="small muted">Access granted {fmtDate(data.provisioned_at)}</div>}
          {data.expires && <div className="small muted">Access valid until {fmtDate(data.expires)}</div>}
          <div className="row" style={{ marginTop: '.4rem', flexWrap: 'wrap' }}>
            {certuvoUrl && <a className="btn sm" href={certuvoUrl} target="_blank" rel="noreferrer">Open Certuvo ↗</a>}
            <button className="btn sm secondary" disabled={resending} onClick={resend}>{resending ? 'Sending…' : 'Resend access instructions'}</button>
          </div>
          {resendNote && <div className={'small' + (resendNote.ok ? ' muted' : '')} style={resendNote.ok ? undefined : { color: 'var(--err, #c2410c)' }} role="status">{resendNote.text}</div>}
        </div>
      ) : (
        <div style={{ display: 'grid', gap: '.5rem' }}>
          <p className="muted small" style={{ margin: 0 }}>
            {data.message ?? 'Your Certuvo practice account is set up automatically once your membership is active. It will appear here shortly after payment.'}
          </p>
          {certuvoUrl && (
            <div className="row">
              <a className="btn sm secondary" href={certuvoUrl} target="_blank" rel="noreferrer">Go to Certuvo ↗</a>
            </div>
          )}
        </div>
      )}
      {/* Mandated notice: PCI shows only the access card; all practice lives in Certuvo. */}
      <p className="muted small" style={{ margin: '.6rem 0 0', borderTop: '1px solid var(--border,#e5e7eb)', paddingTop: '.5rem' }}>
        {data.notice ?? 'Certuvo is an external practice platform. All practice questions, mock examinations, study tools, AI coaching, progress tracking and learning activities are available directly within Certuvo.'}
      </p>
    </Card>
  )
}

// Certuvo is an external practice platform. This page shares the student's Certuvo access credentials only;
// all practice questions, mock exams and study tools live inside Certuvo (the in-portal practice engine was
// intentionally removed here). The practice API endpoints remain available server-side and are simply not
// rendered on this page, so nothing is lost and this can be re-enabled if the policy changes.
export default function Certuvo() {
  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Certuvo</h1>
        <p className="muted">
          Certuvo is PCI's official study &amp; practice platform for your PCI certification. Your Certuvo
          account is created for you once your membership is active — use the access details below to sign in.
          All practice questions, mock examinations and study tools live inside Certuvo. The credential is
          still earned on the real examination.
        </p>
      </div>
      <CertuvoAccessPanel />
    </div>
  )
}
