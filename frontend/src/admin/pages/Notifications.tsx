import { useEffect, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, ErrorNote, StatusBadge } from '../../components/ui'
import { PageHeader, EmptyState, SkeletonTable } from '../../components/premium'
import { Icon } from '../../components/icons'
import { fmtDateTime, titleCase } from '../../format'

/* Operational alerts — who hears about what.
 *
 * The backend has carried this the whole time (Endpoints/Notifications.cs: recipients, a
 * per-event on/off switch, a delivery log and a test send) with no console screen in front of
 * it, so the only way to change who gets alerted was to edit site_settings by hand. This is
 * that screen.
 *
 * Two honesty rules the backend already enforces and this surfaces rather than hides:
 *   - `resolved` is the list an alert would ACTUALLY fan out to right now — the free-text
 *     recipients merged with the configured admin address, de-duplicated and validated. It is
 *     shown separately from the raw input, because they routinely differ.
 *   - `mail_configured` is false when no provider is wired. Alerts are still recorded, but they
 *     reach nobody's inbox — a silent failure worth stating loudly. */

interface NotificationsData {
  recipients: string
  admin_email: string
  resolved: string[]
  events: { key: string; label: string }[]
  toggles: Record<string, boolean>
  recent: {
    channel?: string | null
    recipient?: string | null
    subject?: string | null
    status?: string | null
    related_type?: string | null
    created_at?: string | null
  }[]
  mail_configured: boolean
}

export default function Notifications() {
  const { data, loading, error, refetch } = useAdminQuery<NotificationsData>('/api/admin/notifications')
  const [recipients, setRecipients] = useState('')
  const [toggles, setToggles] = useState<Record<string, boolean>>({})
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState<{ ok: boolean; text: string } | null>(null)
  const [dirty, setDirty] = useState(false)

  // Seed the form from the server once it arrives, but never stomp edits in progress.
  useEffect(() => {
    if (!data || dirty) return
    setRecipients(data.recipients ?? '')
    setToggles({ ...data.toggles })
  }, [data, dirty])

  async function save() {
    setBusy(true)
    setNote(null)
    try {
      const body: Record<string, unknown> = { recipients }
      for (const e of data?.events ?? []) body[e.key] = !!toggles[e.key]
      const r = await adminApi.post<{ ok: boolean; resolved: string[] }>('/api/admin/notifications', body)
      setDirty(false)
      setNote({
        ok: true,
        text: r.resolved.length
          ? `Saved. Alerts will reach ${r.resolved.length} address${r.resolved.length === 1 ? '' : 'es'}.`
          : 'Saved — but no valid address is configured, so alerts will reach nobody.',
      })
      refetch()
    } catch (e) {
      setNote({ ok: false, text: e instanceof Error ? e.message : 'Could not save.' })
    } finally {
      setBusy(false)
    }
  }

  async function sendTest() {
    setBusy(true)
    setNote(null)
    try {
      const r = await adminApi.post<{ ok: boolean; sent?: number; message?: string }>('/api/admin/notifications/test', {})
      setNote(
        r.ok
          ? { ok: true, text: `Test alert dispatched to ${r.sent} recipient${r.sent === 1 ? '' : 's'}. Check the delivery log below.` }
          : { ok: false, text: r.message ?? 'Could not send the test alert.' },
      )
      refetch()
    } catch (e) {
      setNote({ ok: false, text: e instanceof Error ? e.message : 'Could not send the test alert.' })
    } finally {
      setBusy(false)
    }
  }

  const set = (k: string, v: boolean) => { setDirty(true); setToggles((p) => ({ ...p, [k]: v })) }
  const on = (data?.events ?? []).filter((e) => toggles[e.key]).length

  return (
    <div className="page">
      <PageHeader
        eyebrow="Operations"
        title="Notifications"
        subtitle="Who is alerted when something happens on the platform, and proof that those alerts are being delivered."
        actions={
          <button className="btn secondary sm" disabled={busy || !data} onClick={sendTest}>
            <Icon name="mail" size={14} /> Send test alert
          </button>
        }
      />

      {loading ? (
        <Card className="flat"><SkeletonTable rows={6} cols={3} /></Card>
      ) : error ? (
        <ErrorNote>{error}</ErrorNote>
      ) : !data ? null : (
        <>
          {/* A missing mail provider is the difference between "alerting" and "logging to nobody". */}
          {!data.mail_configured && (
            <div className="notice warn" role="status">
              <strong>No mail provider is configured.</strong> Alerts are still recorded in the log below, but
              they are not delivered to any inbox. Set <code>RESEND_API_KEY</code> or <code>SMTP_HOST</code> in
              the deployment environment to turn these into real emails.
            </div>
          )}

          {note && (
            <div className={'notice' + (note.ok ? '' : ' err')} role="status">{note.text}</div>
          )}

          <Card
            title="Recipients"
            action={<span className="chip brand">{data.resolved.length} active</span>}
          >
            <div className="field">
              <label htmlFor="notif-recipients">Alert addresses</label>
              <textarea
                id="notif-recipients"
                rows={3}
                value={recipients}
                placeholder="ops@example.org, board@example.org"
                onChange={(e) => { setDirty(true); setRecipients(e.target.value) }}
              />
              <p className="muted small" style={{ margin: '.4rem 0 0' }}>
                Separate with commas, semicolons or new lines. Invalid addresses are dropped and a maximum of
                25 are kept.
              </p>
            </div>

            {/* The raw box and the effective list routinely differ — show what would really be used. */}
            <div className="dl" style={{ marginTop: '.5rem' }}>
              <div>
                <dt>Fallback address</dt>
                <dd>{data.admin_email || <span className="muted">none configured</span>}</dd>
              </div>
              <div>
                <dt>Alerts go to</dt>
                <dd>
                  {data.resolved.length === 0 ? (
                    <span className="muted">Nobody — no valid address is configured.</span>
                  ) : (
                    <span style={{ display: 'flex', gap: '.35rem', flexWrap: 'wrap' }}>
                      {data.resolved.map((r) => <span className="chip" key={r}>{r}</span>)}
                    </span>
                  )}
                </dd>
              </div>
            </div>
          </Card>

          <Card title="Events" action={<span className="chip">{on} of {data.events.length} on</span>}>
            <div className="grid cols-2">
              {data.events.map((e) => (
                <label key={e.key} className="row" style={{ fontWeight: 400, gap: '.55rem', alignItems: 'flex-start' }}>
                  <input
                    type="checkbox"
                    style={{ width: 'auto', marginTop: '.2rem' }}
                    checked={!!toggles[e.key]}
                    onChange={(ev) => set(e.key, ev.target.checked)}
                  />
                  <span>{e.label}</span>
                </label>
              ))}
            </div>
            <div className="row" style={{ marginTop: '1rem' }}>
              <button className="btn" disabled={busy || !dirty} onClick={save}>
                {busy ? 'Saving…' : dirty ? 'Save changes' : 'Saved'}
              </button>
              {dirty && <span className="muted small">Unsaved changes</span>}
            </div>
          </Card>

          <Card className="flat" title="Recent deliveries">
            {data.recent.length === 0 ? (
              <EmptyState
                icon="inbox"
                title="Nothing dispatched yet"
                detail="Every alert attempt is recorded here, delivered or failed. Send a test alert to prove the path end to end."
              />
            ) : (
              <div className="table-scroll">
                <table className="data">
                  <thead>
                    <tr><th>When</th><th>Channel</th><th>Recipient</th><th>Subject</th><th>About</th><th>Status</th></tr>
                  </thead>
                  <tbody>
                    {data.recent.map((r, i) => (
                      <tr key={i}>
                        <td className="small muted" style={{ whiteSpace: 'nowrap' }}>{fmtDateTime(r.created_at)}</td>
                        <td className="small">{titleCase(r.channel ?? '—')}</td>
                        <td className="small">{r.recipient || '—'}</td>
                        <td className="small">{r.subject || '—'}</td>
                        <td className="small muted">{r.related_type ? titleCase(r.related_type) : '—'}</td>
                        <td>
                          {r.status === 'sent'
                            ? <Badge tone="ok">sent</Badge>
                            : <StatusBadge status={r.status} />}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </Card>
        </>
      )}
    </div>
  )
}
