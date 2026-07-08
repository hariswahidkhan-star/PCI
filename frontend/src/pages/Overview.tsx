import { Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { Card, Stat, StatusBadge, Spinner, ErrorNote, Badge } from '../components/ui'
import { fmtDate, titleCase } from '../format'
import type { Lifecycle } from '../api/types'

interface Journey {
  label: string
  state: 'done' | 'current' | 'blocked' | 'todo'
  detail?: string
}

// Build the visual candidate journey from the backend lifecycle state.
function buildJourney(lc: Lifecycle): Journey[] {
  const memberActive = lc.membership_status === 'active'
  const paid = lc.candidate_status === 'exam_fee_paid'
  const blocked = paid && lc.blocking_items.length > 0
  const scheduled = ['booked', 'in_progress', 'submitted'].includes(lc.exam_status)
  const taken = lc.exam_status === 'submitted' || !!lc.result_status
  const credentialed = lc.credential_status === 'active'

  const step = (label: string, done: boolean, current: boolean, detail?: string, isBlocked = false): Journey => ({
    label,
    state: done ? 'done' : isBlocked ? 'blocked' : current ? 'current' : 'todo',
    detail,
  })

  return [
    step('Membership active', memberActive, !memberActive),
    step('Exam fee paid', paid, memberActive && !paid),
    step('Eligibility cleared', paid && !blocked, paid && blocked, blocked ? lc.blocking_items.map(titleCase).join(', ') : undefined, blocked),
    step('Exam scheduled', scheduled, paid && !blocked && !scheduled),
    step('Exam completed', taken, scheduled && !taken),
    step('Credential issued', credentialed, taken && !credentialed),
  ]
}

const NEXT_STEP: Record<string, { title: string; detail: string; cta?: { label: string; to?: string; href?: string } }> = {
  activate_membership: { title: 'Activate your membership', detail: 'Membership is the first step of the certification journey.', cta: { label: 'Get started', href: '/enroll.html' } },
  pay_exam_fee: { title: 'Pay your exam fee', detail: 'Secure your certification exam entitlement to continue.', cta: { label: 'Continue enrolment', href: '/enroll.html' } },
  complete_eligibility: { title: 'Complete your eligibility items', detail: 'A few items are needed before you can schedule your exam.', cta: { label: 'Review profile', to: '/profile' } },
  schedule_exam: { title: 'Schedule your exam', detail: 'You are eligible — choose a slot to sit your certification exam.', cta: { label: 'Schedule now', to: '/certifications' } },
  prepare_launch: { title: 'Prepare for exam day', detail: 'Your exam is booked. Review the requirements and get your launch code when it is time.', cta: { label: 'View exam', to: '/certifications' } },
  await_result: { title: 'Result pending', detail: 'Your exam has been submitted. We will notify you as soon as the result is available.' },
  await_review: { title: 'Under review', detail: 'Your attempt is being reviewed. This is routine and results are released once complete.' },
  review_retake: { title: 'Review and retake', detail: 'Review your result and, when ready, schedule a retake.', cta: { label: 'View options', to: '/certifications' } },
  maintain_credential: { title: 'Maintain your credential', detail: 'Keep your certification current by logging continuing professional development.', cta: { label: 'Log CPD', to: '/cpd' } },
}

export default function Overview() {
  const { user } = useAuth()
  const { me, loading, error } = useMe()

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  const journey = buildJourney(me.lifecycle)
  const next = NEXT_STEP[me.lifecycle.next_step]
  const activeCreds = me.credentials.filter((c) => c.status === 'active').length
  const outstanding = me.consents.outstanding.length

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Welcome back, {user?.firstName || 'there'}</h1>
        <p className="muted">Registration no. {me.user.registration_no} · Member since {fmtDate(me.user.created_at)}</p>
      </div>

      {outstanding > 0 && (
        <div className="notice warn">
          You have {outstanding} outstanding consent{outstanding > 1 ? 's' : ''} to review.{' '}
          <a href="/student.html#consents">Review now</a>
        </div>
      )}

      <div className="grid cols-3">
        <Card><Stat n={activeCreds} k="Active credentials" /></Card>
        <Card><Stat n={`${me.cpd.total}/${me.cpd.target}`} k="CPD hours" /></Card>
        <Card><Stat n={me.exams.length} k="Exam entitlements" /></Card>
      </div>

      {next && (
        <Card title="Your next step">
          <div className="spread">
            <div>
              <h3 style={{ marginBottom: '.25rem' }}>{next.title}</h3>
              <p className="muted" style={{ margin: 0 }}>{next.detail}</p>
            </div>
            {next.cta &&
              (next.cta.to ? (
                <Link className="btn" to={next.cta.to}>{next.cta.label}</Link>
              ) : (
                <a className="btn" href={next.cta.href}>{next.cta.label}</a>
              ))}
          </div>
        </Card>
      )}

      <div className="grid cols-2">
        <Card title="Certification journey">
          <ul className="steps">
            {journey.map((s, i) => (
              <li key={i} className={s.state}>
                <span className="dot">{s.state === 'done' ? '✓' : i + 1}</span>
                <span>
                  <span className="label">{s.label}</span>
                  {s.state === 'current' && <> <Badge tone="brand">In progress</Badge></>}
                  {s.state === 'blocked' && <> <Badge tone="err">Action needed</Badge></>}
                  {s.detail && <div className="detail">{s.detail}</div>}
                </span>
              </li>
            ))}
          </ul>
        </Card>

        <Card title="Recent activity">
          {me.attempts.length === 0 ? (
            <p className="muted small">No exam attempts yet.</p>
          ) : (
            <table className="data">
              <thead>
                <tr><th>Type</th><th>Date</th><th>Status</th></tr>
              </thead>
              <tbody>
                {me.attempts.slice(0, 5).map((a) => (
                  <tr key={a.id}>
                    <td>{titleCase(a.kind)}</td>
                    <td>{fmtDate(a.submitted_at || a.started_at)}</td>
                    <td><StatusBadge status={a.result_status || a.status} /></td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </Card>
      </div>
    </div>
  )
}
