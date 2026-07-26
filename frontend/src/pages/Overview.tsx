import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { useT } from '../i18n'
import { Card, Stat, StatusBadge, Spinner, Badge } from '../components/ui'
import Ring from '../components/Ring'
import CountUp from '../components/CountUp'
import ConsentsNotice from '../components/ConsentsNotice'
import { fmtDate, titleCase, isPast } from '../format'
import type { Lifecycle, Me } from '../api/types'

// Translation function signature (matches useT() from ../i18n).
type TFn = (key: string, vars?: Record<string, string | number>) => string

interface Journey {
  label: string
  state: 'done' | 'current' | 'blocked' | 'todo'
  detail?: string
}

// Build the visual candidate journey from the backend lifecycle state.
function buildJourney(lc: Lifecycle, t: TFn): Journey[] {
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
    step(t('overview.journeyMembershipActive'), memberActive, !memberActive),
    step(t('overview.journeyExamFeePaid'), paid, memberActive && !paid),
    step(t('overview.journeyEligibilityCleared'), paid && !blocked, paid && blocked, blocked ? lc.blocking_items.map(titleCase).join(', ') : undefined, blocked),
    step(t('overview.journeyExamScheduled'), scheduled, paid && !blocked && !scheduled),
    step(t('overview.journeyExamCompleted'), taken, scheduled && !taken),
    step(t('overview.journeyCredentialIssued'), credentialed, taken && !credentialed),
  ]
}

function buildNextSteps(t: TFn): Record<string, { title: string; detail: string; cta?: { label: string; to?: string; href?: string } }> {
  return {
    activate_membership: { title: t('overview.activateMembershipTitle'), detail: t('overview.activateMembershipDetail'), cta: { label: t('overview.activateMembershipCta'), to: '/billing' } },
    pay_exam_fee: { title: t('overview.payExamFeeTitle'), detail: t('overview.payExamFeeDetail'), cta: { label: t('overview.payExamFeeCta'), to: '/billing' } },
    complete_eligibility: { title: t('overview.completeEligibilityTitle'), detail: t('overview.completeEligibilityDetail'), cta: { label: t('overview.completeEligibilityCta'), to: '/profile' } },
    schedule_exam: { title: t('overview.scheduleExamTitle'), detail: t('overview.scheduleExamDetail'), cta: { label: t('overview.scheduleExamCta'), to: '/certifications' } },
    prepare_launch: { title: t('overview.prepareLaunchTitle'), detail: t('overview.prepareLaunchDetail'), cta: { label: t('overview.prepareLaunchCta'), to: '/certifications' } },
    await_result: { title: t('overview.awaitResultTitle'), detail: t('overview.awaitResultDetail') },
    await_review: { title: t('overview.awaitReviewTitle'), detail: t('overview.awaitReviewDetail') },
    review_retake: { title: t('overview.reviewRetakeTitle'), detail: t('overview.reviewRetakeDetail'), cta: { label: t('overview.reviewRetakeCta'), to: '/certifications' } },
    maintain_credential: { title: t('overview.maintainCredentialTitle'), detail: t('overview.maintainCredentialDetail'), cta: { label: t('overview.maintainCredentialCta'), to: '/cpd' } },
  }
}

interface ChecklistItem {
  label: string
  done: boolean
  to?: string
  href?: string
}

function buildChecklist(me: Me, t: TFn): ChecklistItem[] {
  const p = (me.profile ?? {}) as Record<string, unknown>
  return [
    { label: t('overview.checklistAboutYou'), done: Boolean(p.current_role || p.country), to: '/onboarding' },
    { label: t('overview.checklistExperience'), done: me.experiences.length > 0, to: '/profile' },
    { label: t('overview.checklistQualification'), done: me.qualifications.length > 0, to: '/profile' },
    { label: t('overview.checklistId'), done: !!me.identity_document && me.identity_document.status !== 'rejected', to: '/certifications' },
    { label: t('overview.checklistActivateMembership'), done: me.lifecycle.membership_status === 'active', to: '/billing' },
    { label: t('overview.checklistPayExamFee'), done: me.lifecycle.candidate_status === 'exam_fee_paid' || me.exams.length > 0, to: '/billing' },
    { label: t('overview.checklistBookExam'), done: ['booked', 'in_progress', 'submitted'].includes(me.lifecycle.exam_status), to: '/certifications' },
  ]
}

interface WorldToday {
  available: boolean
  code?: string
  title?: string
  difficulty?: string
  est_minutes?: number
}

/** "Continue learning" card (journey repair P1-09): today's PCI World challenge on the Overview,
 * entered through the secure one-time handoff so completed work is owned by this account from the
 * first answer. Honest copy — practice evidence, never a certification claim. */
function WorldCard() {
  const t = useT()
  const [today, setToday] = useState<WorldToday | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  useEffect(() => {
    fetch('/api/world/today')
      .then((r) => (r.ok ? r.json() : { available: false }))
      .then(setToday)
      .catch(() => setToday({ available: false }))
  }, [])

  async function open(returnTo: string) {
    setBusy(true)
    setErr(null)
    try {
      // The SSO bridge claims this browser's anonymous World work and answers with a one-time
      // fragment-carried handoff code — never a reusable token through this origin.
      const r = await api.post<{ url?: string }>('/api/me/world-passport/sso', {
        world_session: localStorage.getItem('world_session') || undefined,
        return_to: returnTo,
      })
      window.location.assign(r.url || '/world')
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not open PCI World.')
      setBusy(false)
    }
  }

  if (today === null) return null
  return (
    <Card title={t('world.cardTitle')}>
      <div className="spread">
        <div>
          {today.available ? (
            <>
              <h3 style={{ marginBottom: '.25rem' }}>{today.title}</h3>
              <p className="muted small" style={{ margin: 0 }}>
                {[today.difficulty && titleCase(today.difficulty), today.est_minutes ? `~${today.est_minutes} min` : null]
                  .filter(Boolean)
                  .join(' · ')}
              </p>
            </>
          ) : (
            <h3 style={{ marginBottom: '.25rem' }}>{t('world.cardTitle')}</h3>
          )}
          <p className="muted small" style={{ marginBottom: 0, marginTop: '.4rem' }}>{t('world.blurb')}</p>
          {err && <p className="muted small" style={{ color: 'var(--err, #b00020)' }}>{err}</p>}
        </div>
        <div style={{ display: 'flex', gap: '.5rem', flexWrap: 'wrap', alignItems: 'flex-start' }}>
          {today.available && today.code && (
            <button className="btn" disabled={busy} onClick={() => open(`/world/challenge/${today.code}`)}>
              {t('world.startToday')}
            </button>
          )}
          <button className="btn secondary" disabled={busy} onClick={() => open('/world/account')}>
            {t('world.open')}
          </button>
        </div>
      </div>
    </Card>
  )
}

export default function Overview() {
  const { user } = useAuth()
  const { me, loading, error, refetch } = useMe()
  const t = useT()

  if (loading) return <Spinner />
  if (error)
    return (
      <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <div>
          <h1>{t('overview.welcome', { name: user?.firstName || t('overview.friendlyName') })}</h1>
        </div>
        <Card>
          <h3 style={{ marginBottom: '.35rem' }}>{t('overview.loadError')}</h3>
          <p className="muted" style={{ marginBottom: '.9rem' }}>
            {t('overview.loadErrorDetail')}
          </p>
          <button className="btn" onClick={() => refetch()}>{t('overview.tryAgain')}</button>
        </Card>
      </div>
    )
  if (!me) return null

  const journey = buildJourney(me.lifecycle, t)
  const next = buildNextSteps(t)[me.lifecycle.next_step]
  // a lapsed-but-not-revoked credential keeps status='active' in the DB (expiry is derived at read time),
  // so exclude past-expiry ones to match the Credentials page's "Expired" treatment.
  const activeCreds = me.credentials.filter((c) => c.status === 'active' && !isPast(c.expires_at)).length
  const completion = Number((me.profile as Record<string, unknown> | null)?.profile_completion_percentage ?? 20)
  const checklist = buildChecklist(me, t)
  const remaining = checklist.filter((c) => !c.done)

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>{t('overview.welcome', { name: user?.firstName || t('overview.friendlyName') })}</h1>
        <p className="muted">{t('overview.registrationLine', { no: me.user.registration_no, date: fmtDate(me.user.created_at) })}</p>
      </div>

      <ConsentsNotice />

      <div className="grid cols-3">
        <Card><Stat n={<CountUp value={activeCreds} />} k={t('overview.activeCredentials')} /></Card>
        <Card><Stat n={<><CountUp value={me.cpd.total} />/{me.cpd.target}</>} k={t('overview.cpdHours')} /></Card>
        <Card><Stat n={<CountUp value={me.exams.length} />} k={t('overview.examEntitlements')} /></Card>
      </div>

      {remaining.length > 0 && (
        <Card>
          <div className="row" style={{ alignItems: 'flex-start', gap: '1.25rem', flexWrap: 'wrap' }}>
            <Ring value={completion} label={t('overview.profile')} />
            <div style={{ flex: 1, minWidth: 240 }}>
              <h3 style={{ marginBottom: '.35rem' }}>{t('overview.getSetUp')}</h3>
              <p className="muted small" style={{ marginBottom: '.6rem' }}>
                {t('overview.stepsLeft', { n: remaining.length })}
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
        <Card title={t('overview.yourNextStep')}>
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

      <WorldCard />

      <div className="grid cols-2">
        <Card title={t('overview.certificationJourney')}>
          <ul className="steps">
            {journey.map((s, i) => (
              <li key={i} className={s.state}>
                <span className="dot">{s.state === 'done' ? '✓' : i + 1}</span>
                <span>
                  <span className="label">{s.label}</span>
                  {s.state === 'current' && <> <Badge tone="brand">{t('overview.inProgress')}</Badge></>}
                  {s.state === 'blocked' && <> <Badge tone="err">{t('overview.actionNeeded')}</Badge></>}
                  {s.detail && <div className="detail">{s.detail}</div>}
                </span>
              </li>
            ))}
          </ul>
        </Card>

        <Card title={t('overview.recentActivity')}>
          {me.attempts.length === 0 ? (
            <p className="muted small">{t('overview.noAttempts')}</p>
          ) : (
            <table className="data">
              <thead>
                <tr><th>{t('overview.tableType')}</th><th>{t('overview.tableDate')}</th><th>{t('overview.tableStatus')}</th></tr>
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
