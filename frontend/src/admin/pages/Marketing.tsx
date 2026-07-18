import { Fragment, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { useAdminAuth } from '../AdminAuth'
import { adminApi } from '../api'
import { Card, Spinner, ErrorNote, Empty, Badge, StatusBadge, Stat } from '../../components/ui'
import { fmtDateTime } from '../../format'

// Marketing dashboard: one place for acquisition analytics (traffic, sources, UTM campaigns,
// conversions, revenue) and outbound email campaigns (draft → audience preview → test → send),
// plus the suppression list. Analytics needs 'reports'; campaigns/suppression need 'subscribers' —
// each section renders only for admins holding that permission (the server enforces the same).

interface Summary {
  page_views: number; visitors: number; registrations: number; logins: number
  purchases: number; revenue: number; memberships_activated: number
  sources: { source: string; n: number }[]
  campaigns: { campaign: string; n: number }[]
  conversions_by_source: { source: string; registrations: number; purchases: number; revenue: number }[]
}

interface CampaignRow {
  id: number; name: string; subject: string; audience: string; status: string
  total: number; sent: number; failed: number; suppressed: number
  created_at?: string | null; sent_at?: string | null
}

interface CampaignDetail {
  campaign: CampaignRow & { body_html?: string | null }
  recipients: { email: string; first_name?: string | null; status: string; error?: string | null; sent_at?: string | null }[]
  counts: { total: number; sent: number; failed: number; pending: number }
}

interface SuppressionRow { id: number; email: string; reason?: string | null; created_at?: string | null }

const AUDIENCES = [
  ['subscribers', 'Newsletter subscribers'],
  ['students', 'All students'],
  ['all', 'Students + subscribers'],
] as const

export default function Marketing() {
  const { can } = useAdminAuth()
  const canReports = can('reports')
  const canCampaigns = can('subscribers')

  // ---- analytics (reports) ----
  const [days, setDays] = useState(30)
  const { data: sum, loading: sumLoading, error: sumError } =
    useAdminQuery<Summary>(canReports ? `/api/admin/analytics/summary?days=${days}` : null)

  // ---- campaigns (subscribers) ----
  const { data: camps, loading: campsLoading, error: campsError, refetch: refetchCamps } =
    useAdminQuery<{ rows: CampaignRow[] }>(canCampaigns ? '/api/admin/campaigns' : null)
  const { data: supp, refetch: refetchSupp } =
    useAdminQuery<{ rows: SuppressionRow[] }>(canCampaigns ? '/api/admin/suppression' : null)

  const [err, setErr] = useState<string | null>(null)
  const [notice, setNotice] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)

  // new-campaign form
  const [name, setName] = useState('')
  const [subject, setSubject] = useState('')
  const [audience, setAudience] = useState<string>('subscribers')
  const [body, setBody] = useState('')
  const [preview, setPreview] = useState<{ count: number; suppressed: number } | null>(null)

  // per-row state
  const [open, setOpen] = useState<number | null>(null)
  const [detail, setDetail] = useState<CampaignDetail | null>(null)
  const [testTo, setTestTo] = useState('')

  // suppression form
  const [suppEmail, setSuppEmail] = useState('')

  async function act(fn: () => Promise<void>, okMsg?: string) {
    setBusy(true); setErr(null); setNotice(null)
    try { await fn(); if (okMsg) setNotice(okMsg) }
    catch (e) { setErr(e instanceof Error ? e.message : 'The action failed.') }
    finally { setBusy(false) }
  }

  const previewAudience = () => act(async () => {
    setPreview(await adminApi.post<{ count: number; suppressed: number }>('/api/admin/campaigns/audience-preview', { audience }))
  })

  const createDraft = () => act(async () => {
    await adminApi.post('/api/admin/campaigns', { name, subject, body, audience })
    setName(''); setSubject(''); setBody(''); setPreview(null)
    refetchCamps()
  }, 'Draft created. Send yourself a test before dispatching.')

  const toggleRow = (id: number) => act(async () => {
    if (open === id) { setOpen(null); setDetail(null); return }
    setOpen(id); setDetail(null); setTestTo('')
    setDetail(await adminApi.get<CampaignDetail>(`/api/admin/campaigns/${id}`))
  })

  const sendTest = (id: number) => act(async () => {
    await adminApi.post(`/api/admin/campaigns/${id}/test`, { to: testTo.trim() })
  }, `Test sent to ${testTo.trim()} (subject prefixed [TEST]).`)

  const sendCampaign = (c: CampaignRow) => {
    if (!window.confirm(`Send “${c.name}” to its full audience now? This cannot be undone.`)) return
    void act(async () => {
      const r = await adminApi.post<{ total: number; suppressed: number }>(`/api/admin/campaigns/${c.id}/send`)
      refetchCamps(); setOpen(null); setDetail(null)
      setNotice(`Sending to ${r.total} recipient(s); ${r.suppressed} suppressed. Delivery runs in the background — refresh for live counts.`)
    })
  }

  const suppress = (action: 'add' | 'remove', email: string) => act(async () => {
    await adminApi.post('/api/admin/suppression', { email, action })
    setSuppEmail(''); refetchSupp()
  }, action === 'add' ? 'Address suppressed — it will never be mailed.' : 'Address removed from the suppression list.')

  const money = (v: number) => '$' + Number(v || 0).toLocaleString(undefined, { maximumFractionDigits: 2 })

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Marketing</h1>
        <p className="muted">Acquisition analytics and outbound email campaigns in one place. Every campaign email automatically carries the compliance footer with a working one-click unsubscribe, and the suppression list is always honoured.</p>
      </div>

      {err && <div className="notice err" role="alert">{err}</div>}
      {notice && <div className="notice ok" role="status">{notice}</div>}

      {/* ================= Acquisition analytics ================= */}
      {canReports && (
        <>
          <Card
            title="Acquisition"
            action={
              <select value={days} onChange={(e) => setDays(Number(e.target.value))} aria-label="Analytics window">
                {[7, 30, 90].map((d) => <option key={d} value={d}>Last {d} days</option>)}
              </select>
            }
          >
            {sumLoading && !sum ? <Spinner /> : sumError ? <ErrorNote>{sumError}</ErrorNote> : sum ? (
              <div className="grid cols-3" style={{ gap: '.8rem' }}>
                <Stat n={sum.visitors.toLocaleString()} k="Visitors" />
                <Stat n={sum.page_views.toLocaleString()} k="Page views" />
                <Stat n={sum.registrations.toLocaleString()} k="Registrations" />
                <Stat n={sum.purchases.toLocaleString()} k="Purchases" />
                <Stat n={money(sum.revenue)} k="Revenue" />
                <Stat n={sum.memberships_activated.toLocaleString()} k="Memberships activated" />
              </div>
            ) : null}
          </Card>

          {sum && (
            <div className="grid cols-2" style={{ gap: '1rem', alignItems: 'start' }}>
              <Card title="Traffic by source">
                {sum.sources.length === 0 ? <Empty>No traffic recorded yet.</Empty> : (
                  <table className="data">
                    <thead><tr><th>Source</th><th>Page views</th></tr></thead>
                    <tbody>{sum.sources.map((s) => <tr key={s.source}><td>{s.source}</td><td>{s.n.toLocaleString()}</td></tr>)}</tbody>
                  </table>
                )}
              </Card>
              <Card title="Conversions & revenue by source">
                {sum.conversions_by_source.length === 0 ? <Empty>No conversions in this window.</Empty> : (
                  <table className="data">
                    <thead><tr><th>Source</th><th>Registrations</th><th>Purchases</th><th>Revenue</th></tr></thead>
                    <tbody>{sum.conversions_by_source.map((s) => (
                      <tr key={s.source}><td>{s.source}</td><td>{s.registrations}</td><td>{s.purchases}</td><td>{money(s.revenue)}</td></tr>
                    ))}</tbody>
                  </table>
                )}
              </Card>
            </div>
          )}

          {sum && sum.campaigns.length > 0 && (
            <Card title="UTM campaign traffic">
              <table className="data">
                <thead><tr><th>Campaign (utm_campaign)</th><th>Page views</th></tr></thead>
                <tbody>{sum.campaigns.map((c) => <tr key={c.campaign}><td>{c.campaign}</td><td>{c.n.toLocaleString()}</td></tr>)}</tbody>
              </table>
            </Card>
          )}
        </>
      )}

      {/* ================= Email campaigns ================= */}
      {canCampaigns && (
        <>
          <Card title="New email campaign">
            <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
              <div className="grid cols-2" style={{ gap: '.6rem' }}>
                <label className="small">Internal name
                  <input value={name} onChange={(e) => setName(e.target.value)} placeholder="e.g. March exam-window announcement" />
                </label>
                <label className="small">Subject
                  <input value={subject} onChange={(e) => setSubject(e.target.value)} placeholder="Subject line — {'{{first_name}}'} is personalised" />
                </label>
              </div>
              <div className="row" style={{ flexWrap: 'wrap', alignItems: 'end', gap: '.6rem' }}>
                <label className="small">Audience
                  <select value={audience} onChange={(e) => { setAudience(e.target.value); setPreview(null) }}>
                    {AUDIENCES.map(([v, l]) => <option key={v} value={v}>{l}</option>)}
                  </select>
                </label>
                <button className="btn sm secondary" onClick={previewAudience} disabled={busy}>Preview audience</button>
                {preview && (
                  <span className="small muted">
                    <strong>{preview.count.toLocaleString()}</strong> deliverable · {preview.suppressed} suppressed
                  </span>
                )}
              </div>
              <label className="small">Body (HTML — <span style={{ fontFamily: 'monospace' }}>{'{{first_name}}'}</span> and <span style={{ fontFamily: 'monospace' }}>{'{{email}}'}</span> are personalised; the unsubscribe footer is appended automatically)
                <textarea rows={6} value={body} onChange={(e) => setBody(e.target.value)} placeholder="<p>Hello {{first_name}},</p><p>…</p>" />
              </label>
              <div className="row">
                <button className="btn" onClick={createDraft} disabled={busy || !name.trim() || !subject.trim() || body.trim().length < 10}>Create draft</button>
              </div>
            </div>
          </Card>

          <Card title="Campaigns">
            {campsLoading && !camps ? <Spinner /> : campsError ? <ErrorNote>{campsError}</ErrorNote> :
              (camps?.rows ?? []).length === 0 ? <Empty>No campaigns yet — create a draft above.</Empty> : (
                <table className="data">
                  <thead>
                    <tr><th>Name</th><th>Audience</th><th>Status</th><th>Sent</th><th>Failed</th><th>Suppressed</th><th>Created</th><th></th></tr>
                  </thead>
                  <tbody>
                    {(camps?.rows ?? []).map((c) => (
                      <Fragment key={c.id}>
                        <tr>
                          <td><strong>{c.name}</strong><div className="muted small">{c.subject}</div></td>
                          <td className="small">{c.audience}</td>
                          <td><StatusBadge status={c.status} /></td>
                          <td className="small">{c.status === 'draft' ? '—' : `${c.sent}/${c.total}`}</td>
                          <td className="small">{c.failed || 0}</td>
                          <td className="small">{c.suppressed || 0}</td>
                          <td className="small muted">{fmtDateTime(c.created_at)}</td>
                          <td>
                            <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap' }}>
                              <button className="btn sm secondary" onClick={() => toggleRow(c.id)} disabled={busy}>{open === c.id ? 'Close' : 'Detail'}</button>
                              {c.status === 'draft' && <button className="btn sm" onClick={() => sendCampaign(c)} disabled={busy}>Send</button>}
                            </div>
                          </td>
                        </tr>
                        {open === c.id && (
                          <tr>
                            <td colSpan={8} style={{ background: 'var(--canvas)' }}>
                              {!detail ? <Spinner /> : (
                                <div className="stack" style={{ display: 'grid', gap: '.6rem', padding: '.4rem 0' }}>
                                  <div className="row" style={{ gap: '.8rem', flexWrap: 'wrap' }}>
                                    <Badge>total {detail.counts.total}</Badge>
                                    <Badge tone="ok">sent {detail.counts.sent}</Badge>
                                    <Badge tone={detail.counts.failed > 0 ? 'err' : 'neutral'}>failed {detail.counts.failed}</Badge>
                                    <Badge>pending {detail.counts.pending}</Badge>
                                    {c.sent_at && <span className="small muted">dispatched {fmtDateTime(c.sent_at)}</span>}
                                  </div>
                                  <div className="row" style={{ alignItems: 'end', gap: '.5rem', flexWrap: 'wrap' }}>
                                    <label className="small">Send a test to
                                      <input value={testTo} onChange={(e) => setTestTo(e.target.value)} placeholder="you@example.com" style={{ maxWidth: 260 }} />
                                    </label>
                                    <button className="btn sm secondary" onClick={() => sendTest(c.id)} disabled={busy || !testTo.includes('@')}>Send test</button>
                                  </div>
                                  {detail.recipients.length > 0 && (
                                    <div>
                                      <div className="muted small" style={{ marginBottom: '.25rem' }}>Recipients (first 20)</div>
                                      <table className="data">
                                        <thead><tr><th>Email</th><th>Status</th><th>Sent at</th><th>Error</th></tr></thead>
                                        <tbody>{detail.recipients.map((r, i) => (
                                          <tr key={i}><td className="small">{r.email}</td><td><StatusBadge status={r.status} /></td>
                                            <td className="small muted">{fmtDateTime(r.sent_at)}</td><td className="small muted">{r.error || '—'}</td></tr>
                                        ))}</tbody>
                                      </table>
                                    </div>
                                  )}
                                </div>
                              )}
                            </td>
                          </tr>
                        )}
                      </Fragment>
                    ))}
                  </tbody>
                </table>
              )}
          </Card>

          <Card title="Suppression list" action={<span className="muted small">{(supp?.rows ?? []).length} address(es)</span>}>
            <div className="row" style={{ alignItems: 'end', gap: '.5rem', flexWrap: 'wrap', marginBottom: '.6rem' }}>
              <label className="small">Email
                <input value={suppEmail} onChange={(e) => setSuppEmail(e.target.value)} placeholder="never-mail@example.com" style={{ maxWidth: 280 }} />
              </label>
              <button className="btn sm" onClick={() => suppress('add', suppEmail.trim().toLowerCase())} disabled={busy || !suppEmail.includes('@')}>Suppress</button>
            </div>
            {(supp?.rows ?? []).length === 0 ? <Empty>No suppressed addresses. One-click unsubscribes land here automatically.</Empty> : (
              <table className="data">
                <thead><tr><th>Email</th><th>Reason</th><th>Added</th><th></th></tr></thead>
                <tbody>{(supp?.rows ?? []).map((s) => (
                  <tr key={s.id}>
                    <td className="small">{s.email}</td><td className="small">{s.reason || '—'}</td>
                    <td className="small muted">{fmtDateTime(s.created_at)}</td>
                    <td><button className="btn sm secondary" onClick={() => suppress('remove', s.email)} disabled={busy}>Remove</button></td>
                  </tr>
                ))}</tbody>
              </table>
            )}
          </Card>
        </>
      )}
    </div>
  )
}
