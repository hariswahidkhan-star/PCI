import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { useT } from '../i18n'
import { Card, StatusBadge, Spinner, Badge } from '../components/ui'
import { PageHeader, KpiGrid, KpiTile, Meter, QuickTile, EmptyState } from '../components/premium'
import { Icon, type IconName } from '../components/icons'
import Ring from '../components/Ring'
import CountUp from '../components/CountUp'
import ConsentsNotice from '../components/ConsentsNotice'
import WorldPassportSection from './WorldPassportSection'
import { fmtDate, titleCase, isPast } from '../format'
import type { Lifecycle, Me } from '../api/types'

// Translation function signature (matches useT() from ../i18n).
type TFn = (key: string, vars?: Record<string, string | number>) => string

interface Journey {
  label: string
  state: 'done' | 'current' | 'blocked' | 'todo'
  detail?: string
  /** Where the student goes to advance this step — surfaced only while it is live. */
  to?: string
}

// Build the visual candidate journey from the backend lifecycle state.
function buildJourney(lc: Lifecycle, t: TFn): Journey[] {
  const memberActive = lc.membership_status === 'active'
  const paid = lc.candidate_status === 'exam_fee_paid'
  const blocked = paid && lc.blocking_items.length > 0
  const scheduled = ['booked', 'in_progress', 'submitted'].includes(lc.exam_status)
  const taken = lc.exam_status === 'submitted' || !!lc.result_status
  const credentialed = lc.credential_status === 'active'

  const step = (label: string, done: boolean, current: boolean, to?: string, detail?: string, isBlocked = false): Journey => ({
    label,
    state: done ? 'done' : isBlocked ? 'blocked' : current ? 'current' : 'todo',
    detail,
    to,
  })

  return [
    step(t('overview.journeyMembershipActive'), memberActive, !memberActive, '/billing'),
    step(t('overview.journeyExamFeePaid'), paid, memberActive && !paid, '/billing'),
    step(t('overview.journeyEligibilityCleared'), paid && !blocked, paid && blocked, '/profile', blocked ? lc.blocking_items.map(titleCase).join(', ') : undefined, blocked),
    step(t('overview.journeyExamScheduled'), scheduled, paid && !blocked && !scheduled, '/certifications'),
    step(t('overview.journeyExamCompleted'), taken, scheduled && !taken, '/certifications'),
    step(t('overview.journeyCredentialIssued'), credentialed, taken && !credentialed, '/credentials'),
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

/* The quick-links band: every portal surface reachable in one tap from the dashboard, with a
   live count where the `me` payload already carries one (so no extra request is made).

   The count accessors are defensive about missing collections: a badge on a navigation tile
   is never worth taking the whole dashboard down for, so an absent array reads as "no count"
   rather than throwing. */
const len = (v: unknown): number => (Array.isArray(v) ? v.length : 0)
const QUICK_LINKS: { to: string; tkey: string; icon: IconName; count?: (me: Me) => number }[] = [
  { to: '/certifications', tkey: 'nav.certifications', icon: 'award', count: (me) => len(me.exams) },
  { to: '/credentials', tkey: 'nav.credentials', icon: 'shield-check', count: (me) => len(me.credentials) },
  { to: '/applications', tkey: 'nav.applications', icon: 'clipboard' },
  { to: '/billing', tkey: 'nav.billing', icon: 'credit-card' },
  { to: '/documents', tkey: 'nav.documents', icon: 'folder' },
  { to: '/resources', tkey: 'nav.resources', icon: 'book-open' },
  { to: '/templates', tkey: 'nav.templates', icon: 'file-text' },
  { to: '/events', tkey: 'nav.events', icon: 'calendar' },
  { to: '/cpd', tkey: 'nav.cpd', icon: 'refresh' },
  { to: '/messages', tkey: 'nav.messages', icon: 'mail', count: (me) => me.unread ?? 0 },
  { to: '/support', tkey: 'nav.support', icon: 'life-buoy', count: (me) => len(me.tickets) },
  { to: '/profile', tkey: 'nav.profile', icon: 'user' },
]

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
  const doneSteps = journey.filter((s) => s.state === 'done').length
  const cpdOnTrack = me.cpd.target > 0 && me.cpd.total >= me.cpd.target

  return (
    <div className="page fade-stagger">
      <PageHeader
        eyebrow={t('shell.studentPortal')}
        title={t('overview.welcome', { name: user?.firstName || t('overview.friendlyName') })}
        subtitle={t('overview.registrationLine', { no: me.user.registration_no, date: fmtDate(me.user.created_at) })}
      />

      <ConsentsNotice />

      <KpiGrid>
        <KpiTile
          icon="shield-check"
          label={t('overview.activeCredentials')}
          value={<CountUp value={activeCreds} />}
          hint={t('overview.activeCredentialsHint')}
          tone={activeCreds > 0 ? 'ok' : 'neutral'}
          to="/credentials"
        />
        <KpiTile
          icon="refresh"
          label={t('overview.cpdHours')}
          value={<><CountUp value={me.cpd.total} />/{me.cpd.target}</>}
          hint={<Meter value={me.cpd.total} max={Math.max(1, me.cpd.target)} tone={cpdOnTrack ? 'ok' : undefined} />}
          tone={cpdOnTrack ? 'ok' : 'brand'}
          to="/cpd"
        />
        <KpiTile
          icon="award"
          label={t('overview.examEntitlements')}
          value={<CountUp value={me.exams.length} />}
          hint={t('overview.examEntitlementsHint')}
          to="/certifications"
        />
        <KpiTile
          icon="bell"
          label={t('overview.unreadUpdates')}
          value={<CountUp value={me.unread ?? 0} />}
          hint={t('overview.unreadUpdatesHint')}
          tone={(me.unread ?? 0) > 0 ? 'warn' : 'neutral'}
          to="/messages"
        />
      </KpiGrid>

      {/* The next step is the single most important thing on this page — it gets the accent
          rail and sits above everything except the outstanding-setup checklist. */}
      {next && (
        <Card className="rail pad-lg" title={t('overview.yourNextStep')}>
          <div className="spread">
            <div style={{ minWidth: 0 }}>
              <h3 style={{ marginBottom: '.25rem' }}>{next.title}</h3>
              <p className="muted" style={{ margin: 0 }}>{next.detail}</p>
            </div>
            {next.cta &&
              (next.cta.to ? (
                <Link className="btn" to={next.cta.to}>{next.cta.label}<Icon name="arrow-right" size={15} /></Link>
              ) : (
                <a className="btn" href={next.cta.href}>{next.cta.label}<Icon name="arrow-right" size={15} /></a>
              ))}
          </div>
        </Card>
      )}

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

      <WorldPassportSection />

      <WorldCard />

      <div className="grid cols-2">
        <Card
          title={t('overview.certificationJourney')}
          action={<span className="chip brand">{t('overview.journeyProgress', { done: doneSteps, total: journey.length })}</span>}
        >
          <ul className="steps">
            {journey.map((s, i) => (
              <li key={i} className={s.state}>
                <span className="dot">{s.state === 'done' ? '✓' : i + 1}</span>
                <span>
                  <span className="label">{s.label}</span>
                  {s.state === 'current' && <> <Badge tone="brand">{t('overview.inProgress')}</Badge></>}
                  {s.state === 'blocked' && <> <Badge tone="err">{t('overview.actionNeeded')}</Badge></>}
                  {s.detail && <div className="detail">{s.detail}</div>}
                  {/* A live step is actionable — take the student straight to where it is resolved. */}
                  {(s.state === 'current' || s.state === 'blocked') && s.to && (
                    <div className="step-cta">
                      <Link className="btn ghost sm" to={s.to}>
                        {t('overview.journeyGo')} <Icon name="arrow-right" size={13} />
                      </Link>
                    </div>
                  )}
                </span>
              </li>
            ))}
          </ul>
        </Card>

        <Card title={t('overview.recentActivity')}>
          {me.attempts.length === 0 ? (
            <EmptyState icon="activity" title={t('overview.noAttempts')} detail={t('overview.noAttemptsDetail')} />
          ) : (
            <div className="table-scroll">
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
            </div>
          )}
        </Card>
      </div>

      {/* Every portal surface, one tap away — the "everything at a glance" band the
          request asks for, with live counts where the shell already knows them. */}
      <Card title={t('overview.quickLinks')}>
        <div className="quick-grid">
          {QUICK_LINKS.map((q) => (
            <QuickTile key={q.to} icon={q.icon} label={t(q.tkey)} to={q.to} n={q.count?.(me) || undefined} />
          ))}
        </div>
      </Card>
    </div>
  )
}
