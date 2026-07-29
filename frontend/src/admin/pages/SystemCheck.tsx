import { useAdminQuery } from '../hooks'
import { Card, ErrorNote } from '../../components/ui'
import { PageHeader, KpiGrid, KpiTile, EmptyState, SkeletonCards } from '../../components/premium'
import { Icon, type IconName } from '../../components/icons'

/* Readiness — the owner-only view of whether this deployment is actually configured.
 *
 * GET /api/admin/system-check has existed since the config validator shipped and had no screen
 * in front of it, so the only way to see a production configuration problem was to read the
 * container's boot log. That is precisely the class of failure that goes unnoticed: the app
 * refuses to boot on a bad config and the PREVIOUS deploy keeps serving, so the platform looks
 * healthy from the outside while every new release is silently rejected.
 *
 * The endpoint returns the validator's own issue list (the same one that decides whether boot
 * proceeds), so this screen agrees with the boot decision by construction rather than
 * re-implementing it. Errors block a production boot; warnings do not. */

interface SystemCheck {
  ok: boolean
  environment: string
  checks: Record<string, boolean | string>
  issues: { sev: string; key: string; msg: string }[]
}

/** Human labels for the raw check keys, and which way is healthy. Everything here is a boolean
 *  where TRUE is the good state — the backend is careful to phrase them that way (…_configured,
 *  …_disabled, cors_locked), so a red tile always means "attention", never "off". */
const LABELS: Record<string, string> = {
  base_url_set: 'Public base URL set',
  cors_locked: 'CORS locked to an explicit origin',
  persistent_db: 'Database on durable storage',
  migrations_applied: 'Migrations applied',
  owner_password_changed: 'Bootstrap owner password changed',
  legacy_admin_token_disabled: 'Legacy admin token disabled',
  security_headers: 'Security headers active',
  csp_enforced: 'CSP enforced (not report-only)',
  request_body_capped: 'Request body size capped',
  retention_scheduled: 'Daily retention purge scheduled',
  smtp_configured: 'Mail provider configured',
  stripe_configured: 'Stripe key configured',
  stripe_webhook_configured: 'Stripe webhook secret configured',
  stripe_webhook_url_ok: 'Stripe webhook URL points at /api/webhook',
  meta_leads_secret_configured: 'Meta lead-ads secret configured',
  storage_local: 'Evidence storage on local disk',
  storage_s3_misconfigured: 'S3 storage misconfigured',
}

/** Checks whose healthy state is FALSE — the two "is something wrong" flags among the positives. */
const INVERTED = new Set(['storage_s3_misconfigured'])

/** Checks that are informational rather than pass/fail: local storage is a legitimate posture. */
const INFORMATIONAL = new Set(['storage_local'])

export default function SystemCheck() {
  const { data, loading, error, refetch } = useAdminQuery<SystemCheck>('/api/admin/system-check')

  if (loading) {
    return (
      <div className="page">
        <PageHeader eyebrow="Operations" title="Readiness" subtitle="Checking this deployment's configuration…" />
        <SkeletonCards n={4} />
      </div>
    )
  }
  // A non-owner gets a 403 here by design; surface the reason rather than an empty screen.
  if (error) {
    return (
      <div className="page">
        <PageHeader eyebrow="Operations" title="Readiness" />
        <ErrorNote>{error}</ErrorNote>
      </div>
    )
  }
  if (!data) return null

  const errors = data.issues.filter((i) => i.sev === 'error')
  const warnings = data.issues.filter((i) => i.sev !== 'error')
  const entries = Object.entries(data.checks).filter(([, v]) => typeof v === 'boolean') as [string, boolean][]
  const healthy = (k: string, v: boolean) => (INVERTED.has(k) ? !v : v)
  const passing = entries.filter(([k, v]) => INFORMATIONAL.has(k) || healthy(k, v)).length

  return (
    <div className="page">
      <PageHeader
        eyebrow="Operations"
        title="Readiness"
        subtitle="Whether this deployment is configured to run in production. These are the validator's own checks — the ones that decide whether a boot is allowed to proceed."
        actions={<button className="btn secondary sm" onClick={() => refetch()}><Icon name="refresh" size={14} /> Re-check</button>}
      />

      <KpiGrid>
        <KpiTile
          icon={data.ok ? 'check-circle' : 'alert-triangle'}
          label="Boot status"
          value={data.ok ? 'Ready' : 'Blocked'}
          hint={data.ok ? 'No blocking configuration errors' : 'A production boot would be refused'}
          tone={data.ok ? 'ok' : 'err'}
        />
        <KpiTile icon="sliders" label="Environment" value={data.environment} hint="Runtime environment name" tone="neutral" />
        <KpiTile
          icon="alert-triangle"
          label="Blocking errors"
          value={errors.length}
          hint="Must be zero to deploy"
          tone={errors.length > 0 ? 'err' : 'ok'}
        />
        <KpiTile
          icon="check"
          label="Checks passing"
          value={`${passing}/${entries.length}`}
          hint="Warnings do not block a boot"
          tone={warnings.length > 0 ? 'warn' : 'ok'}
        />
      </KpiGrid>

      {errors.length > 0 && (
        <Card className="rail-err pad-lg" title="What is blocking a production boot">
          <p className="muted" style={{ marginTop: 0 }}>
            A production start refuses with exit code 78 while any of these stand. The previously deployed
            release keeps serving, so the platform looks healthy from outside even though new releases are
            being rejected.
          </p>
          <ul className="queue">
            {errors.map((i) => (
              <li key={i.key} className="queue-row hot">
                <span className="queue-ic"><Icon name="alert-triangle" size={15} /></span>
                <span className="queue-text">
                  <span className="qt-label">{i.key}</span>
                  <span className="qt-detail">{i.msg}</span>
                </span>
              </li>
            ))}
          </ul>
        </Card>
      )}

      {warnings.length > 0 && (
        <Card className="rail-warn" title="Warnings">
          <ul className="queue">
            {warnings.map((i) => (
              <li key={i.key} className="queue-row">
                <span className="queue-ic"><Icon name="alert-triangle" size={15} /></span>
                <span className="queue-text">
                  <span className="qt-label">{i.key}</span>
                  <span className="qt-detail">{i.msg}</span>
                </span>
              </li>
            ))}
          </ul>
        </Card>
      )}

      {errors.length === 0 && warnings.length === 0 && (
        <Card className="flat">
          <EmptyState
            icon="check-circle"
            title="No configuration issues"
            detail="Every production check the validator runs is satisfied for this environment."
          />
        </Card>
      )}

      <Card className="flat" title="All checks">
        <div className="quick-grid">
          {entries.map(([k, v]) => {
            const info = INFORMATIONAL.has(k)
            const good = healthy(k, v)
            const icon: IconName = info ? 'sliders' : good ? 'check-circle' : 'alert-triangle'
            return (
              <div className="quick-tile" key={k} style={{ cursor: 'default' }}>
                <span
                  className="qt-ic"
                  style={
                    info
                      ? undefined
                      : good
                        ? { background: 'var(--ok-bg)', color: 'var(--ok)', borderColor: 'rgba(21,128,61,.18)' }
                        : { background: 'var(--warn-bg)', color: 'var(--warn)', borderColor: 'rgba(180,83,9,.18)' }
                  }
                >
                  <Icon name={icon} size={16} />
                </span>
                <span style={{ minWidth: 0 }}>{LABELS[k] ?? k}</span>
              </div>
            )
          })}
        </div>
        {typeof data.checks.stripe_webhook_path === 'string' && (
          <p className="muted small" style={{ margin: '.9rem 0 0' }}>
            Stripe webhook path: <code>{data.checks.stripe_webhook_path}</code>
          </p>
        )}
      </Card>
    </div>
  )
}
