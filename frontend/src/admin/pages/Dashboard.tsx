import { Link } from 'react-router-dom'
import { useAdminQuery } from '../hooks'
import { useAdminAuth } from '../AdminAuth'
import type { Overview } from '../api'
import { Card, ErrorNote } from '../../components/ui'
import {
  PageHeader,
  KpiGrid,
  KpiTile,
  QueueRow,
  BarSeries,
  TrendColumns,
  EmptyState,
  SkeletonCards,
  SkeletonTable,
} from '../../components/premium'
import { Icon, type IconName } from '../../components/icons'
import { fmtMoney, fmtDateTime, titleCase } from '../../format'

/* The operations console's landing screen. Everything here is derived from the single
   /api/admin/overview payload the backend already returns — including revSeries, the
   12-month revenue trend the previous dashboard fetched but never rendered.

   Three bands, in the order an operator actually reads them:
     1. KPIs        — the state of the institute right now
     2. Needs attention — the work queues, each linking to the section that owns it
     3. Trends + activity — revenue, mix, funnel, and the audit trail
   Every link is permission-gated, so an admin is never sent to a 403. */

/** Short month label for the trend axis: "2026-03" → "Mar 26". */
function monthLabel(ym: string): string {
  const [y, m] = ym.split('-')
  const idx = Number(m) - 1
  const names = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
  return names[idx] ? `${names[idx]} ${y?.slice(2) ?? ''}`.trim() : ym
}

interface QueueDef {
  key: string
  icon: IconName
  label: string
  detail: string
  n: number
  to: string
  perm: string
}

export default function Dashboard() {
  const { data, loading, error } = useAdminQuery<Overview>('/api/admin/overview')
  const { me, can } = useAdminAuth()
  const isOwner = !!me?.is_owner
  const allow = (perm: string) => isOwner || can(perm)
  /** Only link a tile/queue row when the signed-in admin can open the destination. */
  const linkIf = (perm: string, to: string) => (allow(perm) ? to : undefined)

  if (loading) {
    return (
      <div className="page">
        <PageHeader eyebrow="Overview" title="Dashboard" subtitle="Loading the current state of the institute…" />
        <SkeletonCards n={8} />
        <Card className="flat">
          <SkeletonTable rows={5} cols={3} />
        </Card>
      </div>
    )
  }
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null

  const k = data.kpis

  // Work queues: things a human has to act on. The KPI band above reports STATE; this band is
  // phrased as the WORK, so the two never restate each other. Only non-empty queues are shown,
  // so a quiet console stays quiet instead of displaying a wall of zeros.
  const queues: QueueDef[] = [
    { key: 'pending', icon: 'user', label: 'Activate pending students', detail: 'Registered but not yet active', n: k.pendingMembers, to: '/students', perm: 'members' },
    { key: 'exams', icon: 'award', label: 'Book unscheduled exams', detail: 'Paid entitlements not yet booked', n: k.examsDue, to: '/exams', perm: 'exams' },
    { key: 'inq', icon: 'message-circle', label: 'Reply to enquiries', detail: 'Awaiting a first or follow-up reply', n: k.openInq, to: '/enquiries', perm: 'inquiries' },
    { key: 'abandoned', icon: 'clock', label: 'Recover abandoned enrolments', detail: 'No activity for 72 hours or more', n: k.abandoned, to: '/enrollments', perm: 'enrollments' },
    { key: 'failed', icon: 'alert-triangle', label: 'Resolve failed charges', detail: 'Payments that did not complete', n: k.failedPay, to: '/payments', perm: 'payments' },
    { key: 'refunded', icon: 'banknote', label: 'Check refunded payments', detail: 'Reversed — confirm entitlement state', n: k.refunded, to: '/payments', perm: 'payments' },
  ]
  const openQueues = queues.filter((q) => q.n > 0 && allow(q.perm))

  const trend = data.revSeries.map((r) => ({ key: monthLabel(r.ym), value: r.amount }))
  const mix = data.productMix
    .slice()
    .sort((a, b) => b.amount - a.amount)
    .map((p) => ({ key: p.product_type, label: `${titleCase(p.product_type)} · ${p.n}`, value: p.amount }))
  const funnel = [
    { key: 'started', label: 'Enrolment started', value: data.funnel.started },
    { key: 'in_progress', label: 'In progress', value: data.funnel.in_progress },
    { key: 'paid', label: 'Paid', value: data.funnel.paid },
  ]
  // Conversion is only meaningful once somebody has actually started an enrolment.
  const conversion = data.funnel.started > 0 ? Math.round((data.funnel.paid / data.funnel.started) * 100) : null

  return (
    <div className="page">
      <PageHeader
        eyebrow="Overview"
        title="Dashboard"
        subtitle="The state of the institute right now — students, revenue, credentials and everything waiting on a decision."
      />

      {/* ---- band 1: the state of the institute ---- */}
      <KpiGrid>
        <KpiTile
          icon="users"
          label="Students"
          value={k.members}
          hint={<><strong>{k.activeMembers}</strong> active · <strong>{k.pendingMembers}</strong> pending</>}
          to={linkIf('members', '/students')}
        />
        <KpiTile
          icon="banknote"
          label="Revenue (all time)"
          value={fmtMoney(k.revenue)}
          hint={<><strong>{k.payCount}</strong> paid transactions</>}
          tone="ok"
          to={linkIf('payments', '/payments')}
        />
        <KpiTile
          icon="trending-up"
          label="Revenue this month"
          value={fmtMoney(k.revenueMonth)}
          hint="Excludes test accounts"
          tone="ok"
          to={linkIf('reports', '/reports')}
        />
        <KpiTile
          icon="shield-check"
          label="Active credentials"
          value={k.credActive}
          hint={<><strong>{k.credTotal}</strong> issued in total</>}
          to={linkIf('credentials', '/credentials')}
        />
        <KpiTile
          icon="award"
          label="Exams to schedule"
          value={k.examsDue}
          hint="Paid, still within the booking window"
          tone={k.examsDue > 0 ? 'warn' : 'neutral'}
          to={linkIf('exams', '/exams')}
        />
        <KpiTile
          icon="message-circle"
          label="Open enquiries"
          value={k.openInq}
          hint="Not yet closed"
          tone={k.openInq > 0 ? 'warn' : 'neutral'}
          to={linkIf('inquiries', '/enquiries')}
        />
        <KpiTile
          icon="clock"
          label="Abandoned enrolments"
          value={k.abandoned}
          hint="Idle for 72 hours or more"
          tone={k.abandoned > 0 ? 'warn' : 'neutral'}
          to={linkIf('enrollments', '/enrollments')}
        />
        <KpiTile
          icon="alert-triangle"
          label="Payment exceptions"
          value={k.failedPay + k.refunded}
          hint={<><strong>{k.failedPay}</strong> failed · <strong>{k.refunded}</strong> refunded</>}
          tone={k.failedPay + k.refunded > 0 ? 'err' : 'neutral'}
          to={linkIf('payments', '/payments')}
        />
      </KpiGrid>

      {/* ---- band 2: what needs a decision ---- */}
      <Card className="flat" title="Needs attention">
        {openQueues.length === 0 ? (
          <EmptyState
            icon="check-circle"
            title="Nothing is waiting"
            detail="No pending activations, unscheduled exams, open enquiries, abandoned enrolments or payment exceptions."
          />
        ) : (
          <div className="queue">
            {openQueues.map((q) => (
              <QueueRow key={q.key} icon={q.icon} label={q.label} detail={q.detail} n={q.n} to={q.to} hot />
            ))}
          </div>
        )}
      </Card>

      {/* ---- band 3: trends ---- */}
      <div className="grid cols-2">
        <Card className="flat" title="Revenue — last 12 months">
          {trend.length === 0 ? (
            <EmptyState icon="bar-chart" title="No revenue yet" detail="Paid transactions appear here as soon as the first payment settles." />
          ) : (
            <>
              <TrendColumns points={trend} format={(n) => fmtMoney(n)} />
              <p className="muted small" style={{ margin: '.75rem 0 0' }}>
                Highest month {fmtMoney(Math.max(...trend.map((p) => p.value)))} · test accounts excluded.
              </p>
            </>
          )}
        </Card>

        <Card className="flat" title="Revenue by product">
          {mix.length === 0 ? (
            <EmptyState icon="tag" title="No paid transactions yet" detail="Once payments settle, the product split is broken down here." />
          ) : (
            <BarSeries items={mix} format={(n) => fmtMoney(n)} />
          )}
        </Card>
      </div>

      <div className="grid cols-2">
        <Card
          className="flat"
          title="Enrolment funnel"
          action={conversion != null ? <span className="chip brand">{conversion}% converted</span> : undefined}
        >
          {data.funnel.started === 0 ? (
            <EmptyState icon="activity" title="No enrolments yet" detail="The funnel fills in as candidates begin the enrolment flow." />
          ) : (
            <BarSeries items={funnel} />
          )}
        </Card>

        <Card className="flat" title="Recent activity">
          {data.recent.length === 0 ? (
            <EmptyState icon="activity" title="No activity recorded yet" detail="Privileged actions are written to the audit log and surface here." />
          ) : (
            <>
              <div className="table-scroll">
                <table className="data">
                  <thead>
                    <tr><th>When</th><th>Action</th><th>Details</th></tr>
                  </thead>
                  <tbody>
                    {data.recent.map((r, i) => (
                      <tr key={i}>
                        <td className="small muted" style={{ whiteSpace: 'nowrap' }}>{fmtDateTime(r.ts)}</td>
                        <td className="small"><strong>{titleCase(r.action)}</strong></td>
                        <td className="small muted">{r.details}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              {allow('audit') && (
                <p style={{ margin: '.85rem 0 0' }}>
                  <Link to="/audit" className="small" style={{ fontWeight: 700 }}>
                    Open the full audit log <Icon name="arrow-right" size={13} />
                  </Link>
                </p>
              )}
            </>
          )}
        </Card>
      </div>
    </div>
  )
}
