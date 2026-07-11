import { Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { Card, Stat, StatusBadge, Spinner, Badge } from '../components/ui'
import Ring from '../components/Ring'
import CountUp from '../components/CountUp'
import ConsentsNotice from '../components/ConsentsNotice'
import { fmtDate, titleCase, isPast } from '../format'
import type { Lifecycle, Me } from '../api/types'

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
  activate_membership: { title: 'Activate your membership', detail: 'Membership is the first step of the certification journey — pay when you are ready, right here in the portal.', cta: { label: 'Activate membership', to: '/billing' } },
  pay_exam_fee: { title: 'Pay your exam fee', detail: 'Secure your certification exam entitlement to continue — payable from your Billing page.', cta: { label: 'Pay exam fee', to: '/billing' } },
  complete_eligibility: { title: 'Complete your eligibility items', detail: 'A few items are needed before you can schedule your exam.', cta: { label: 'Review profile', to: '/profile' } },
  schedule_exam: { title: 'Schedule your exam', detail: 'You are eligible — choose a slot to sit your certification exam.', cta: { label: 'Schedule now', to: '/certifications' } },
  prepare_launch: { title: 'Prepare for exam day', detail: 'Your exam is booked. Review the requirements and get your launch code when it is time.', cta: { label: 'View exam', to: '/certifications' } },
  await_result: { title: 'Result pending', detail: 'Your exam has been submitted. We will notify you as soon as the result is available.' },
  await_review: { title: 'Under review', detail: 'Your attempt is being reviewed. This is routine and results are released once complete.' },
  review_retake: { title: 'Review and retake', detail: 'Review your result and, when ready, schedule a retake.', cta: { label: 'View options', to: '/certifications' } },
  maintain_credential: { title: 'Maintain your credential', detail: 'Keep your certification current by logging continuing professional development.', cta: { label: 'Log CPD', to: '/cpd' } },
}

interface ChecklistItem {
  label: string
  done: boolean
  to?: string
  href?: string
}

function buildChecklist(me: Me): ChecklistItem[] {
  const p = (me.profile ?? {}) as Record<string, unknown>
  return [
    { label: 'Tell us about yourself', done: Boolean(p.current_role || p.country), to: '/onboarding' },
    { label: 'Add your work experience', done: me.experiences.length > 0, to: '/profile' },
    { label: 'Add a qualification', done: me.qualifications.length > 0, to: '/profile' },
    { label: 'Upload a government-issued ID', done: !!me.identity_document && me.identity_document.status !== 'rejected', to: '/certifications' },
    { label: 'Activate your membership', done: me.lifecycle.membership_status === 'active', to: '/billing' },
    { label: 'Book your exam', done: ['booked', 'in_progress', 'submitted'].includes(me.lifecycle.exam_status), to: '/certifications' },
  ]
}

export default function Overview() {
  const { user } = useAuth()
  const { me, loading, error, refetch } = useMe()

  if (loading) return <Spinner />
  if (error)
    return (
      <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <div>
          <h1>Welcome back, {user?.firstName || 'there'}</h1>
        </div>
        <Card>
          <h3 style={{ marginBottom: '.35rem' }}>We couldn’t load your dashboard</h3>
          <p className="muted" style={{ marginBottom: '.9rem' }}>
            Something went wrong while loading your account. This is usually temporary.
          </p>
          <button className="btn" onClick={() => refetch()}>Try again</button>
        </Card>
      </div>
    )
  if (!me) return null

  const journey = buildJourney(me.lifecycle)
  const next = NEXT_STEP[me.lifecycle.next_step]
  // a lapsed-but-not-revoked credential keeps status='active' in the DB (expiry is derived at read time),
  // so exclude past-expiry ones to match the Credentials page's "Expired" treatment.
  const activeCreds = me.credentials.filter((c) => c.status === 'active' && !isPast(c.expires_at)).length
  const completion = Number((me.profile as Record<string, unknown> | null)?.profile_completion_percentage ?? 20)
  const checklist = buildChecklist(me)
  const remaining = checklist.filter((c) => !c.done)

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Welcome back, {user?.firstName || 'there'}</h1>
        <p className="muted">Registration no. {me.user.registration_no} · Member since {fmtDate(me.user.created_at)}</p>
      </div>

      <ConsentsNotice />

      <div className="grid cols-3">
        <Card><Stat n={<CountUp value={activeCreds} />} k="Active credentials" /></Card>
        <Card><Stat n={<><CountUp value={me.cpd.total} />/{me.cpd.target}</>} k="CPD hours" /></Card>
        <Card><Stat n={<CountUp value={me.exams.length} />} k="Exam entitlements" /></Card>
      </div>

      {remaining.length > 0 && (
        <Card>
          <div className="row" style={{ alignItems: 'flex-start', gap: '1.25rem', flexWrap: 'wrap' }}>
            <Ring value={completion} label="profile" />
            <div style={{ flex: 1, minWidth: 240 }}>
              <h3 style={{ marginBottom: '.35rem' }}>Get set up</h3>
              <p className="muted small" style={{ marginBottom: '.6rem' }}>
                {remaining.length} step{remaining.length > 1 ? 's' : ''} left — a complete profile speeds up exam eligibility.
              </p>
              <ul className="checklist-mini">
                {checklist.map((c) => (
                  <li key={c.label} className={c.done ? 'done' : ''}>
                    <span className="ck">{c.done ? '✓' : ''}</span>
                    {c.done ? (
                      <span>{c.label}</span>
                    ) : c.to ? (
                      <Link to={c.to}>{c.label}</Link>
                    ) : (
                      <a href={c.href}>{c.label}</a>
                    )}
                  </li>
                ))}
              </ul>
            </div>
          </div>
        </Card>
      )}

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
