import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'
import { ApiError } from '../../api/client'

// Admin Console → Training Partners (Phase 7). Two tabs:
//   Directory   — CRUD over published/unpublished partner entries; publishing (listed) renders them
//                 on the public website via the PCI-PARTNERS marker.
//   Applications — review public "become a partner" submissions; approving creates a directory entry
//                 (unlisted) which you then publish.
// Certification stays independent of training — partners deliver exam preparation only.

interface Partner {
  id: number; name: string; slug?: string | null; tier: string
  country?: string | null; region?: string | null; city?: string | null
  website?: string | null; logo_url?: string | null; summary?: string | null
  description?: string | null; specialties?: string | null; contact_email?: string | null
  listed: number; sort_order?: number | null; created_at?: string | null
  // sponsorship limits — ceilings inside which this institution's discount codes operate
  max_discount_percent?: number | null; max_codes?: number | null; max_uses_per_code?: number | null
  total_allocation?: number | null; allow_full_sponsorship?: number | null
  // agreement / portal fields
  status?: string | null; institution_type?: string | null
  agreement_start?: string | null; agreement_end?: string | null
  auto_approve_codes?: number | null; privacy_fields?: string | null; eligible_countries?: string | null
  // token portal / commissions / sponsorship dashboard
  partner_type?: string | null; contact_name?: string | null
  commission_pct?: number | null; sponsor_enabled?: number | null
  /** scenario-based test institution (never listed publicly, excluded from reports) */
  is_test?: number | null
}
interface PartnerUser {
  id: number; email: string; name?: string | null; role: string; status?: string | null
  must_change_pw?: number | null; last_login_at?: string | null; created_at?: string | null
}
interface CommLedger {
  commission_pct: number; attributed_revenue: number; accrued: number; paid_out: number; balance: number
  payments: { reference?: string | null; product_type?: string | null; final_amount: number; payment_date?: string | null; code?: string | null }[]
  payouts: { amount: number; note?: string | null; paid_at?: string | null }[]
}
interface SponsoredRow {
  id: number; candidate_email: string; candidate_name?: string | null; created_at?: string | null
  cert_acronym?: string | null; cert_code?: string | null; application_status?: string | null
  entitlement_status?: string | null; exam_result?: string | null; credential_id?: string | null
}
interface TPApp {
  id: number; reference: string; org_name?: string | null; website?: string | null
  contact_name?: string | null; contact_email?: string | null; contact_phone?: string | null
  country?: string | null; city?: string | null; region?: string | null
  delivery_modes?: string | null; specialties?: string | null; learners_per_year?: number | null
  description?: string | null; status: string; proposed_tier?: string | null
  partner_id?: number | null; admin_note?: string | null; created_at?: string | null; doc_count?: number | null
}
interface TPDoc { id: number; doc_kind: string; filename?: string | null; mime?: string | null; size_bytes?: number | null }

const TIERS = ['registered', 'authorized', 'premier'] as const
const TIER_TONE: Record<string, 'ok' | 'warn' | 'brand'> = { premier: 'brand', authorized: 'warn', registered: 'ok' }
const STATUS_TONE: Record<string, 'ok' | 'err' | 'brand' | 'warn'> = { approved: 'ok', pending_review: 'brand', under_review: 'warn', rejected: 'err' }
const DOC_LABEL: Record<string, string> = { accreditation: 'Accreditation', company_profile: 'Company profile', curriculum: 'Curriculum', supporting: 'Supporting document' }
const TABS = ['Directory', 'Applications'] as const

export default function TrainingPartners() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Directory')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Training Partners</h1>
        <p className="muted">Recognised organisations that deliver examination preparation, plus institutions and sponsors. Certification stays independent of training — partners prepare or fund candidates; PCI owns the examination and the certification decision. Use <strong>Portal logins</strong> to manage institution access, and <strong>Commissions</strong> for sponsorship progress and commission payouts.</p>
      </div>
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>)}
      </div>
      {tab === 'Directory' && <DirectoryTab />}
      {tab === 'Applications' && <ApplicationsTab />}
    </div>
  )
}

// ---------------- Test institutions ----------------
// Scenario-based test partner accounts (mirrors the student test-user mechanism): a flagged,
// never-listed institution with a portal login and a ready session into /partner.html, so every
// partner-dashboard journey can be exercised without touching a real institution.
const TEST_PARTNER_SCENARIOS = [
  { value: 'active', label: 'Active — sponsorship enabled' },
  { value: 'fresh_login', label: 'Fresh login — must change password' },
  { value: 'no_sponsor', label: 'Sponsorship disabled' },
  { value: 'suspended', label: 'Suspended institution' },
]
interface TestPartnerResult {
  institution: string; email: string; password: string
  token?: string | null; scenario: string
}
function TestPartnerButton({ onCreated }: { onCreated: () => void }) {
  const [scenario, setScenario] = useState('active')
  const [busy, setBusy] = useState(false)
  const [res, setRes] = useState<TestPartnerResult | null>(null)
  const [err, setErr] = useState<string | null>(null)
  async function create() {
    setBusy(true); setErr(null); setRes(null)
    try { setRes(await adminApi.post('/api/admin/test-partners', { scenario })); onCreated() }
    catch (e) { setErr((e as Error).message) } finally { setBusy(false) }
  }
  return (
    <>
      <select value={scenario} onChange={(e) => setScenario(e.target.value)} aria-label="Test-partner scenario" style={{ width: 'auto', maxWidth: 240 }}>
        {TEST_PARTNER_SCENARIOS.map((s) => <option key={s.value} value={s.value}>{s.label}</option>)}
      </select>
      <button className="btn sm secondary" disabled={busy} onClick={create}>{busy ? 'Creating…' : '+ Test partner'}</button>
      {(res || err) && (
        <div className="drawer-backdrop" onClick={() => { setRes(null); setErr(null) }}>
          <div className="drawer" onClick={(e) => e.stopPropagation()} style={{ maxWidth: 460 }}>
            <div className="spread" style={{ marginBottom: '.8rem' }}>
              <h2 style={{ margin: 0 }}>Test partner</h2>
              <button className="btn secondary sm" onClick={() => { setRes(null); setErr(null) }}>Close</button>
            </div>
            {err ? <ErrorNote>{err}</ErrorNote> : res ? (
              <Card title="Test institution created">
                <p className="muted small" style={{ marginTop: 0 }}>
                  Ready to exercise the partner dashboard without touching a real institution. Test
                  institutions are never listed publicly and are excluded from discount and commission
                  reports.
                </p>
                <div className="small" style={{ display: 'grid', gap: '.35rem' }}>
                  <div>Scenario: <strong>{TEST_PARTNER_SCENARIOS.find((s) => s.value === res.scenario)?.label ?? res.scenario}</strong></div>
                  <div>Institution: <strong>{res.institution}</strong></div>
                  <div>Email: <strong>{res.email}</strong></div>
                  <div>Password: <strong>{res.password}</strong></div>
                </div>
                <div className="row" style={{ marginTop: '.7rem' }}>
                  {res.token
                    ? <a className="btn sm" href={`/partner.html#t=${res.token}`} target="_blank" rel="noreferrer">Open partner dashboard as this institution ↗</a>
                    : <span className="muted small">No session for this scenario — sign in at /partner.html to exercise the lockout.</span>}
                </div>
              </Card>
            ) : null}
          </div>
        </div>
      )}
    </>
  )
}

// ---------------- Directory (CRUD) ----------------
function DirectoryTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Partner[] }>('/api/admin/training-partners')
  const [edit, setEdit] = useState<Partner | 'new' | null>(null)
  const [usageFor, setUsageFor] = useState<Partner | null>(null)
  const [loginsFor, setLoginsFor] = useState<Partner | null>(null)
  const [portal, setPortal] = useState<Partner | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const rows = data?.rows ?? []

  async function togglePublish(p: Partner) {
    setErr(null)
    try { await adminApi.patch(`/api/admin/training-partners/${p.id}`, { listed: !p.listed }); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Update failed.') }
  }
  async function del(p: Partner) {
    if (!confirm(p.is_test
      ? `Delete test institution “${p.name}” and all of its data (logins, codes, sponsorships)?`
      : `Delete “${p.name}” from the directory?`)) return
    setErr(null)
    try {
      // Test institutions are removed through the test-partner endpoint so their portal logins,
      // codes and sponsorships are cleaned up with them.
      await adminApi.post(p.is_test ? `/api/admin/test-partners/${p.id}/delete` : `/api/admin/training-partners/${p.id}/delete`)
      refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Delete failed.') }
  }

  return (
    <Card
      title={`Partner directory (${rows.length})`}
      action={
        <span className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
          <TestPartnerButton onCreated={refetch} />
          <button className="btn sm" onClick={() => setEdit('new')}>Add partner</button>
        </span>
      }
    >
      {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
        <Empty>No partners yet. Add one, or approve an application.</Empty>
      ) : (
        <table className="data">
          <thead><tr><th>Name</th><th>Tier</th><th>Location</th><th>Published</th><th /></tr></thead>
          <tbody>
            {rows.map((p) => (
              <tr key={p.id}>
                <td>
                  <strong>{p.name}</strong>{p.is_test ? <> <Badge tone="warn">Test</Badge></> : null}
                  {p.website && <div className="muted small">{p.website}</div>}
                </td>
                <td><Badge tone={TIER_TONE[p.tier] ?? 'ok'}>{p.tier}</Badge></td>
                <td className="small">{[p.city, p.region, p.country].filter(Boolean).join(', ') || '—'}</td>
                <td>{p.listed ? <Badge tone="ok">published</Badge> : <Badge tone="warn">draft</Badge>}</td>
                <td className="row" style={{ gap: '.3rem', justifyContent: 'flex-end' }}>
                  <button className="btn ghost sm" onClick={() => setPortal(p)}>Commissions</button>
                  <button className="btn ghost sm" onClick={() => setUsageFor(p)}>Codes &amp; usage</button>
                  <button className="btn ghost sm" onClick={() => setLoginsFor(p)}>Portal logins</button>
                  <button className="btn ghost sm" onClick={() => setEdit(p)}>Edit</button>
                  <button className="btn ghost sm" onClick={() => togglePublish(p)}>{p.listed ? 'Unpublish' : 'Publish'}</button>
                  <button className="btn ghost sm danger" onClick={() => del(p)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {edit && <PartnerEditor partner={edit === 'new' ? null : edit} busy={busy} setBusy={setBusy}
        onClose={() => setEdit(null)} onSaved={() => { setEdit(null); refetch() }} />}
      {usageFor && <PartnerUsage partner={usageFor} onClose={() => setUsageFor(null)} />}
      {loginsFor && <PartnerLogins partner={loginsFor} onClose={() => setLoginsFor(null)} />}
      {portal && <PortalDrawer partner={portal} onClose={() => { setPortal(null); refetch() }} />}
    </Card>
  )
}

// ---------------- Portal logins (institution users of the partner portal) ----------------
const PARTNER_ROLES = ['admin', 'finance', 'reporting', 'support'] as const

function PartnerLogins({ partner, onClose }: { partner: Partner; onClose: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: PartnerUser[] }>(`/api/admin/training-partners/${partner.id}/users`)
  const [email, setEmail] = useState('')
  const [name, setName] = useState('')
  const [role, setRole] = useState<string>('admin')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  // The one-off temp password (create or reset) — shown once, never retrievable again.
  const [created, setCreated] = useState<{ email: string; temp_password: string; portal_url?: string } | null>(null)
  const rows = data?.rows ?? []

  async function create() {
    if (!email.trim().includes('@')) { setErr('A valid email address is required.'); return }
    setBusy(true); setErr(null); setCreated(null)
    try {
      const r = await adminApi.post<{ email: string; temp_password: string; portal_url?: string }>(
        `/api/admin/training-partners/${partner.id}/users`,
        { email: email.trim().toLowerCase(), name: name.trim() || undefined, role },
      )
      setCreated(r); setEmail(''); setName(''); refetch()
    } catch (e) {
      const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : null
      setErr(body?.error === 'email_taken' ? 'That email already has a partner login.' : (e as Error).message)
    } finally { setBusy(false) }
  }
  async function resetPassword(u: PartnerUser) {
    setBusy(true); setErr(null); setCreated(null)
    try {
      const r = await adminApi.post<{ temp_password: string }>(`/api/admin/partner-users/${u.id}/reset-password`, {})
      setCreated({ email: u.email, temp_password: r.temp_password })
    } catch (e) { setErr((e as Error).message) } finally { setBusy(false) }
  }
  async function toggleStatus(u: PartnerUser) {
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/partner-users/${u.id}/status`, { status: u.status === 'suspended' ? 'active' : 'suspended' })
      refetch()
    } catch (e) { setErr((e as Error).message) } finally { setBusy(false) }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{partner.name} — portal logins</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        {created && (
          <div className="notice" role="status" style={{ marginBottom: '.8rem' }}>
            <strong>Temporary password for {created.email}: <span style={{ fontFamily: 'monospace' }}>{created.temp_password}</span></strong>
            <div className="small" style={{ marginTop: '.25rem' }}>Share it securely — it is shown once and cannot be retrieved again. The user must change it on first sign-in.</div>
            {created.portal_url && <div className="small" style={{ marginTop: '.2rem' }}>Portal: <a href={created.portal_url} target="_blank" rel="noreferrer">{created.portal_url}</a></div>}
          </div>
        )}

        <Section title={`Logins (${rows.length})`}>
          {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
            <p className="muted small">No portal logins yet — create the institution's first admin below.</p>
          ) : (
            <table className="data">
              <thead><tr><th>Email</th><th>Name</th><th>Role</th><th>Status</th><th>Last login</th><th /></tr></thead>
              <tbody>
                {rows.map((u) => (
                  <tr key={u.id}>
                    <td className="small"><strong>{u.email}</strong></td>
                    <td className="small">{u.name || '—'}</td>
                    <td className="small">{u.role}</td>
                    <td>{u.status === 'suspended' ? <Badge tone="err">suspended</Badge> : <Badge tone="ok">active</Badge>}{u.must_change_pw ? <div className="muted small">must change pw</div> : null}</td>
                    <td className="small muted">{u.last_login_at ? fmtDate(u.last_login_at) : 'never'}</td>
                    <td>
                      <div className="row" style={{ gap: '.3rem', flexWrap: 'wrap' }}>
                        <button className="btn ghost sm" disabled={busy} onClick={() => resetPassword(u)}>Reset password</button>
                        <button className={'btn ghost sm' + (u.status === 'suspended' ? '' : ' danger')} disabled={busy} onClick={() => toggleStatus(u)}>
                          {u.status === 'suspended' ? 'Reactivate' : 'Suspend'}
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </Section>

        <Section title="Create a login">
          <div className="row" style={{ flexWrap: 'wrap', alignItems: 'flex-end', gap: '.5rem' }}>
            <label>Email<input value={email} onChange={(e) => setEmail(e.target.value)} placeholder="admin@institution.edu" style={{ maxWidth: 220 }} /></label>
            <label>Name<input value={name} onChange={(e) => setName(e.target.value)} style={{ maxWidth: 180 }} /></label>
            <label>Role
              <select value={role} onChange={(e) => setRole(e.target.value)} style={{ maxWidth: 140 }}>
                {PARTNER_ROLES.map((r) => <option key={r} value={r}>{r}</option>)}
              </select>
            </label>
            <button className="btn sm" disabled={busy} onClick={create}>{busy ? 'Creating…' : 'Create login'}</button>
          </div>
        </Section>
      </div>
    </div>
  )
}

// ---------------- Commissions drawer: sponsorship progress + commission ledger + payouts ----------------
// Portal access itself is managed under "Portal logins" (partner_users with email/password).
function PortalDrawer({ partner, onClose }: { partner: Partner; onClose: () => void }) {
  const ledger = useAdminQuery<CommLedger>(`/api/admin/training-partners/${partner.id}/commissions`)
  const cands = useAdminQuery<{ rows: SponsoredRow[] }>(`/api/admin/training-partners/${partner.id}/candidates`)
  const [payAmount, setPayAmount] = useState('')
  const [payNote, setPayNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  async function recordPayout() {
    const amount = Number(payAmount)
    if (!(amount > 0)) { setErr('Enter a payout amount greater than zero.'); return }
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/training-partners/${partner.id}/payouts`, { amount, note: payNote })
      setPayAmount(''); setPayNote(''); ledger.refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not record the payout.') } finally { setBusy(false) }
  }

  const L = ledger.data
  const money = (v?: number) => `USD ${Number(v ?? 0).toFixed(2)}`
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Commissions &amp; sponsorship — {partner.name}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}

        <Section title="Commissions">
          {ledger.loading ? <Spinner /> : ledger.error ? <ErrorNote>{ledger.error}</ErrorNote> : L && (
            <>
              <div className="grid cols-2 small" style={{ gap: '.3rem' }}>
                <div><span className="muted">Attributed revenue</span><div><strong>{money(L.attributed_revenue)}</strong></div></div>
                <div><span className="muted">Accrued ({L.commission_pct}%)</span><div><strong>{money(L.accrued)}</strong></div></div>
                <div><span className="muted">Paid out</span><div><strong>{money(L.paid_out)}</strong></div></div>
                <div><span className="muted">Balance due</span><div><strong>{money(L.balance)}</strong></div></div>
              </div>
              <div className="row" style={{ gap: '.4rem', marginTop: '.6rem', flexWrap: 'wrap', alignItems: 'flex-end' }}>
                <label className="small" style={{ display: 'grid', gap: '.15rem' }}>Payout amount (USD)
                  <input type="number" min="0" step="0.01" value={payAmount} onChange={(e) => setPayAmount(e.target.value)} style={{ maxWidth: 140 }} />
                </label>
                <label className="small" style={{ display: 'grid', gap: '.15rem', flex: 1, minWidth: 160 }}>Note
                  <input value={payNote} onChange={(e) => setPayNote(e.target.value)} placeholder="e.g. Q3 settlement, wire ref…" />
                </label>
                <button className="btn sm" disabled={busy} onClick={recordPayout}>Record payout</button>
              </div>
              {L.payments.length > 0 && (
                <table className="data" style={{ marginTop: '.7rem' }}>
                  <thead><tr><th>Date</th><th>Reference</th><th>Code</th><th style={{ textAlign: 'right' }}>Amount</th></tr></thead>
                  <tbody>
                    {L.payments.slice(0, 15).map((p, i) => (
                      <tr key={i}>
                        <td className="small muted">{fmtDate(p.payment_date)}</td>
                        <td className="small">{p.reference || '—'}</td>
                        <td className="small">{p.code || '—'}</td>
                        <td className="small" style={{ textAlign: 'right' }}>{money(p.final_amount)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </>
          )}
        </Section>

        <Section title={`Sponsored candidates (${cands.data?.rows.length ?? 0})`}>
          {cands.loading ? <Spinner /> : cands.error ? <ErrorNote>{cands.error}</ErrorNote> : (cands.data?.rows.length ?? 0) === 0 ? (
            <p className="muted small" style={{ margin: 0 }}>No sponsored candidates yet.</p>
          ) : (
            <table className="data">
              <thead><tr><th>Candidate</th><th>Certification</th><th>Access</th><th>Result</th><th>Credential</th></tr></thead>
              <tbody>
                {cands.data!.rows.map((r) => (
                  <tr key={r.id}>
                    <td className="small"><strong>{r.candidate_name || r.candidate_email}</strong><div className="muted">{r.candidate_email}</div></td>
                    <td className="small">{r.cert_acronym || r.cert_code || '—'}</td>
                    <td><Badge tone={r.entitlement_status === 'available' ? 'brand' : r.entitlement_status === 'consumed' ? 'ok' : 'neutral'}>{(r.entitlement_status || '—').replace(/_/g, ' ')}</Badge></td>
                    <td>{r.exam_result ? <Badge tone={r.exam_result === 'pass' ? 'ok' : 'err'}>{r.exam_result}</Badge> : <span className="muted small">—</span>}</td>
                    <td className="small">{r.credential_id || <span className="muted">—</span>}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </Section>
      </div>
    </div>
  )
}

function PartnerEditor({ partner, busy, setBusy, onClose, onSaved }:
  { partner: Partner | null; busy: boolean; setBusy: (b: boolean) => void; onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState<Partial<Partner>>(partner ?? { tier: 'registered', listed: 0 })
  // "Share student names" drives privacy_fields: '["name"]' authorises names, '[]' masks them.
  const [shareNames, setShareNames] = useState<boolean>((partner?.privacy_fields ?? '').includes('name'))
  const [err, setErr] = useState<string | null>(null)
  const set = (k: keyof Partner) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })
  async function save() {
    if (!(f.name ?? '').trim()) { setErr('A name is required.'); return }
    setBusy(true); setErr(null)
    // blank limit → null (no limit); the limits only apply on PATCH — the create POST ignores them
    const num = (v: unknown) => (v === '' || v == null ? null : Number(v))
    const body = {
      name: f.name, tier: f.tier, country: f.country ?? '', region: f.region ?? '', city: f.city ?? '',
      website: f.website ?? '', logo_url: f.logo_url ?? '', summary: f.summary ?? '',
      description: f.description ?? '', specialties: f.specialties ?? '', contact_email: f.contact_email ?? '',
      listed: !!f.listed, sort_order: Number(f.sort_order ?? 0),
      max_discount_percent: num(f.max_discount_percent), max_codes: num(f.max_codes),
      max_uses_per_code: num(f.max_uses_per_code), total_allocation: num(f.total_allocation),
      allow_full_sponsorship: !!f.allow_full_sponsorship,
      // agreement / portal fields
      status: f.status ?? 'active', institution_type: f.institution_type ?? '',
      agreement_start: f.agreement_start ?? '', agreement_end: f.agreement_end ?? '',
      auto_approve_codes: !!f.auto_approve_codes,
      privacy_fields: shareNames ? '["name"]' : '[]',
      eligible_countries: f.eligible_countries ?? '',
      partner_type: f.partner_type ?? 'training', contact_name: f.contact_name ?? '',
      commission_pct: Number(f.commission_pct ?? 0), sponsor_enabled: !!f.sponsor_enabled,
    }
    try {
      if (partner) await adminApi.patch(`/api/admin/training-partners/${partner.id}`, body)
      else await adminApi.post('/api/admin/training-partners', body)
      onSaved()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Save failed.') } finally { setBusy(false) }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{partner ? 'Edit partner' : 'Add partner'}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        <div style={{ display: 'grid', gap: '.55rem' }}>
          <label>Organisation name *<input value={f.name ?? ''} onChange={set('name')} /></label>
          <label>Tier
            <select value={f.tier ?? 'registered'} onChange={set('tier')}>
              {TIERS.map((t) => <option key={t} value={t}>{t}</option>)}
            </select>
          </label>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: 'repeat(auto-fit,minmax(150px,1fr))' }}>
            <label>City<input value={f.city ?? ''} onChange={set('city')} /></label>
            <label>Region<input value={f.region ?? ''} onChange={set('region')} /></label>
            <label>Country<input value={f.country ?? ''} onChange={set('country')} /></label>
          </div>
          <label>Website<input value={f.website ?? ''} onChange={set('website')} placeholder="https://…" /></label>
          <label>Logo URL<input value={f.logo_url ?? ''} onChange={set('logo_url')} placeholder="https://…/logo.png" /></label>
          <label>Summary <span className="muted small">(one line, shown on the directory card)</span><input value={f.summary ?? ''} onChange={set('summary')} /></label>
          <label>Specialties <span className="muted small">(comma or newline separated)</span><textarea rows={2} value={f.specialties ?? ''} onChange={set('specialties')} /></label>
          <label>Description<textarea rows={4} value={f.description ?? ''} onChange={set('description')} /></label>
          <label>Contact email<input value={f.contact_email ?? ''} onChange={set('contact_email')} /></label>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Contact name<input value={f.contact_name ?? ''} onChange={set('contact_name')} /></label>
            <label>Partner type
              <select value={f.partner_type ?? 'training'} onChange={set('partner_type')}>
                <option value="training">Training provider</option>
                <option value="institution">Institution</option>
                <option value="sponsor">Sponsor / employer</option>
              </select>
            </label>
          </div>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Commission % <span className="muted small">(on attributed payments)</span>
              <input type="number" min="0" max="100" step="0.5" value={String(f.commission_pct ?? 0)} onChange={set('commission_pct')} />
            </label>
            <label className="row" style={{ gap: '.4rem', alignItems: 'center', marginTop: '1.5rem' }}>
              <input type="checkbox" checked={!!f.sponsor_enabled} onChange={(e) => setF({ ...f, sponsor_enabled: e.target.checked ? 1 : 0 })} style={{ width: 'auto' }} /> Can bulk-sponsor candidates from the partner portal
            </label>
          </div>
          <fieldset style={{ border: '1px solid var(--line,#e2e8f0)', borderRadius: 10, padding: '.75rem', margin: 0 }}>
            <legend className="small" style={{ fontWeight: 700, padding: '0 .3rem' }}>Agreement &amp; portal</legend>
            <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: 'repeat(auto-fit,minmax(150px,1fr))' }}>
              <label>Status
                <select value={f.status ?? 'active'} onChange={set('status')}>
                  <option value="active">Active</option>
                  <option value="suspended">Suspended</option>
                  <option value="terminated">Terminated</option>
                </select>
              </label>
              <label>Institution type<input value={f.institution_type ?? ''} onChange={set('institution_type')} placeholder="university / bootcamp / employer…" /></label>
              <label>Agreement start<input type="date" value={(f.agreement_start ?? '').slice(0, 10)} onChange={set('agreement_start')} /></label>
              <label>Agreement end<input type="date" value={(f.agreement_end ?? '').slice(0, 10)} onChange={set('agreement_end')} /></label>
            </div>
            <label style={{ marginTop: '.5rem', display: 'block' }}>Eligible countries <span className="muted small">(comma-separated, blank = all)</span>
              <input value={f.eligible_countries ?? ''} onChange={set('eligible_countries')} placeholder="e.g. GB, IE, AE" />
            </label>
            <label className="row" style={{ gap: '.4rem', marginTop: '.5rem' }}>
              <input type="checkbox" checked={!!f.auto_approve_codes} onChange={(e) => setF({ ...f, auto_approve_codes: e.target.checked ? 1 : 0 })} style={{ width: 'auto' }} /> Auto-approve this institution's codes (skip the PCI approval queue)
            </label>
            <label className="row" style={{ gap: '.4rem', marginTop: '.35rem' }}>
              <input type="checkbox" checked={shareNames} onChange={(e) => setShareNames(e.target.checked)} style={{ width: 'auto' }} /> Share student names with this institution (otherwise only masked emails)
            </label>
          </fieldset>
          <fieldset style={{ border: '1px solid var(--line,#e2e8f0)', borderRadius: 10, padding: '.75rem', margin: 0 }}>
            <legend className="small" style={{ fontWeight: 700, padding: '0 .3rem' }}>Sponsorship limits</legend>
            <p className="muted small" style={{ marginTop: 0 }}>Ceilings for this institution's sponsorship codes. Leave a field blank for no limit.</p>
            <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: 'repeat(auto-fit,minmax(140px,1fr))' }}>
              <label>Max discount %<input type="number" min="0" max="100" value={String(f.max_discount_percent ?? '')} onChange={set('max_discount_percent')} /></label>
              <label>Max codes<input type="number" min="0" value={String(f.max_codes ?? '')} onChange={set('max_codes')} /></label>
              <label>Max uses per code<input type="number" min="0" value={String(f.max_uses_per_code ?? '')} onChange={set('max_uses_per_code')} /></label>
              <label>Total allocation<input type="number" min="0" value={String(f.total_allocation ?? '')} onChange={set('total_allocation')} /></label>
            </div>
            <label className="row" style={{ gap: '.4rem', marginTop: '.5rem' }}>
              <input type="checkbox" checked={!!f.allow_full_sponsorship} onChange={(e) => setF({ ...f, allow_full_sponsorship: e.target.checked ? 1 : 0 })} style={{ width: 'auto' }} /> Allow 100% sponsorship
            </label>
          </fieldset>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Sort order<input type="number" value={String(f.sort_order ?? 0)} onChange={set('sort_order')} /></label>
            <label className="row" style={{ gap: '.4rem', alignItems: 'center', marginTop: '1.5rem' }}>
              <input type="checkbox" checked={!!f.listed} onChange={(e) => setF({ ...f, listed: e.target.checked ? 1 : 0 })} style={{ width: 'auto' }} /> Published to public directory
            </label>
          </div>
        </div>
        <div className="row" style={{ gap: '.5rem', marginTop: '.8rem' }}>
          <button className="btn" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save'}</button>
          <button className="btn ghost" onClick={onClose}>Cancel</button>
        </div>
      </div>
    </div>
  )
}

// ---------------- Codes & usage (institution sponsorship) ----------------
interface PartnerCode {
  id: number; code: string; discount_value: number; applies_to?: string | null
  max_uses?: number | null; used_count?: number | null; active?: number | null
  end_date?: string | null; created_at?: string | null
}
interface SponsoredStudent { id?: number | null; email?: string | null; first_name?: string | null; last_name?: string | null; redeemed_at?: string | null; code?: string | null }
interface PartnerUsageResp {
  partner: Record<string, unknown>
  codes: PartnerCode[]
  used_total: number
  allocation: number | null
  remaining: number | null
  students: SponsoredStudent[]
}

// The 422s the code-creation endpoint returns when a request breaches the partner's ceilings.
function codeCreateError(e: unknown): string {
  const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : null
  const code = typeof body?.error === 'string' ? body.error : ''
  const limit = body?.limit != null ? String(body.limit) : ''
  switch (code) {
    case 'over_percent_limit': return `Over the partner's discount limit (max ${limit}%).`
    case 'full_sponsorship_not_allowed': return '100% sponsorship is not allowed for this partner.'
    case 'over_uses_limit': return `Over the per-code uses limit (max ${limit}).`
    case 'over_code_limit': return `This partner has reached its code limit (max ${limit}).`
    case 'over_total_allocation': return `Over the partner's total allocation (${limit} sponsored registrations).`
    case 'code_taken': return 'That code already exists — choose another.'
    default: return e instanceof Error ? e.message : 'Could not create the code.'
  }
}

function PartnerUsage({ partner, onClose }: { partner: Partner; onClose: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<PartnerUsageResp>(`/api/admin/training-partners/${partner.id}/usage`)
  const [percent, setPercent] = useState('10')
  const [maxUses, setMaxUses] = useState('1')
  const [appliesTo, setAppliesTo] = useState('all')
  const [endDate, setEndDate] = useState('')
  const [customCode, setCustomCode] = useState('')
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)

  async function createCode() {
    setBusy(true); setMsg(null)
    try {
      const body: Record<string, unknown> = { percent: Number(percent) || 0, max_uses: Number(maxUses) || 1, applies_to: appliesTo }
      if (endDate) body.end_date = endDate
      if (customCode.trim()) body.code = customCode.trim()
      const r = await adminApi.post<{ code: string }>(`/api/admin/training-partners/${partner.id}/codes`, body)
      setMsg({ ok: true, text: `Code ${r.code} created.` })
      setCustomCode('')
      refetch()
    } catch (e) {
      setMsg({ ok: false, text: codeCreateError(e) })
    } finally { setBusy(false) }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{partner.name} — codes &amp; usage</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading && !data ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data ? null : (
          <>
            <p className="small" style={{ marginTop: 0 }}>
              <strong>{data.allocation != null ? `Used ${data.used_total} of ${data.allocation} (${data.remaining ?? 0} remaining)` : `Used ${data.used_total}`}</strong>
            </p>

            <Section title={`Codes (${data.codes.length})`}>
              {data.codes.length === 0 ? <p className="muted small">No codes yet.</p> : (
                <table className="data">
                  <thead><tr><th>Code</th><th>%</th><th>Uses</th><th>Active</th><th>Expiry</th></tr></thead>
                  <tbody>
                    {data.codes.map((c) => (
                      <tr key={c.id}>
                        <td className="small"><strong>{c.code}</strong>{c.applies_to && c.applies_to !== 'all' ? <div className="muted small">{c.applies_to}</div> : null}</td>
                        <td className="small">{c.discount_value}%</td>
                        <td className="small">{c.used_count ?? 0}/{c.max_uses ?? '∞'}</td>
                        <td>{c.active ? <Badge tone="ok">active</Badge> : <Badge tone="neutral">off</Badge>}</td>
                        <td className="small muted">{c.end_date ? fmtDate(c.end_date) : '—'}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Section>

            <Section title="Create a code">
              {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.5rem' }}>{msg.text}</div>}
              <div className="row" style={{ flexWrap: 'wrap', alignItems: 'flex-end', gap: '.5rem' }}>
                <label>Discount %<input type="number" min="1" max="100" value={percent} onChange={(e) => setPercent(e.target.value)} style={{ maxWidth: 110 }} /></label>
                <label>Max uses<input type="number" min="1" value={maxUses} onChange={(e) => setMaxUses(e.target.value)} style={{ maxWidth: 100 }} /></label>
                <label>Applies to
                  <select value={appliesTo} onChange={(e) => setAppliesTo(e.target.value)} style={{ maxWidth: 150 }}>
                    <option value="all">All products</option>
                    <option value="membership">Membership</option>
                    <option value="exam">Exam</option>
                  </select>
                </label>
                <label>End date<input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} style={{ maxWidth: 160 }} /></label>
                <label>Custom code <span className="muted small">(optional)</span><input value={customCode} onChange={(e) => setCustomCode(e.target.value)} placeholder="auto-generated" style={{ maxWidth: 150 }} /></label>
                <button className="btn sm" disabled={busy} onClick={createCode}>{busy ? 'Creating…' : 'Create code'}</button>
              </div>
            </Section>

            <Section title="Sponsored registrations">
              {data.students.length === 0 ? <p className="muted small">No sponsored registrations yet.</p> : (
                <table className="data">
                  <thead><tr><th>Student</th><th>Code</th><th>Date</th></tr></thead>
                  <tbody>
                    {data.students.map((s, i) => (
                      <tr key={i}>
                        <td className="small">{s.email || '—'}</td>
                        <td className="small">{s.code || '—'}</td>
                        <td className="small muted">{fmtDate(s.redeemed_at)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </Section>
          </>
        )}
      </div>
    </div>
  )
}

// ---------------- Applications (review) ----------------
function ApplicationsTab() {
  const [status, setStatus] = useState('pending_review')
  const { data, loading, error, refetch } = useAdminQuery<{ rows: TPApp[] }>(`/api/admin/training-partner-applications${status ? `?status=${status}` : ''}`)
  const [open, setOpen] = useState<{ application: TPApp; documents: TPDoc[] } | null>(null)
  const [note, setNote] = useState('')
  const [tier, setTier] = useState<string>('registered')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const rows = data?.rows ?? []

  async function view(id: number) {
    setErr(null)
    try {
      const d = await adminApi.get<{ application: TPApp; documents: TPDoc[] }>(`/api/admin/training-partner-applications/${id}`)
      setOpen(d); setNote(d.application.admin_note ?? ''); setTier(d.application.proposed_tier || 'registered')
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not load the application.') }
  }
  async function download(appId: number, docId: number) {
    setErr(null)
    try {
      const res = await fetch(`/api/admin/training-partner-applications/${appId}/documents/${docId}/file`, { headers: { Authorization: 'Bearer ' + (adminApi.getToken() ?? '') } })
      if (!res.ok) throw new Error('Could not load the file.')
      const url = URL.createObjectURL(await res.blob())
      window.open(url, '_blank', 'noopener'); setTimeout(() => URL.revokeObjectURL(url), 60_000)
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not load the file.') }
  }
  async function decide(id: number, s: 'approved' | 'rejected' | 'under_review' | '') {
    setBusy(true); setErr(null)
    try { await adminApi.post(`/api/admin/training-partner-applications/${id}/decide`, { status: s, admin_note: note, tier }); setOpen(null); setNote(''); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Decision failed.') } finally { setBusy(false) }
  }

  return (
    <>
      <Card title="Applications" action={
        <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }} aria-label="Status filter">
          <option value="pending_review">Pending review</option><option value="under_review">Under review</option>
          <option value="approved">Approved</option><option value="rejected">Rejected</option><option value="">All</option>
        </select>
      }>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
          <Empty>No applications with this status.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Organisation</th><th>Contact</th><th>Country</th><th>Docs</th><th>Submitted</th><th>Status</th><th /></tr></thead>
            <tbody>
              {rows.map((a) => (
                <tr key={a.id}>
                  <td><strong>{a.org_name || '—'}</strong><div className="muted small">{a.reference}</div></td>
                  <td className="small">{a.contact_name || '—'}<div className="muted">{a.contact_email || ''}</div></td>
                  <td className="small">{a.country || '—'}</td>
                  <td>{a.doc_count ?? 0}</td>
                  <td className="small muted">{fmtDate(a.created_at)}</td>
                  <td><Badge tone={STATUS_TONE[a.status] ?? 'brand'}>{a.status.replace(/_/g, ' ')}</Badge></td>
                  <td><button className="btn ghost sm" onClick={() => view(a.id)}>Review</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {open && (
        <div className="drawer-backdrop" onClick={() => setOpen(null)}>
          <div className="drawer" onClick={(e) => e.stopPropagation()}>
            <div className="spread" style={{ marginBottom: '1rem' }}>
              <h2 style={{ margin: 0 }}>{open.application.org_name}</h2>
              <button className="btn secondary sm" onClick={() => setOpen(null)}>Close</button>
            </div>
            {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
            <div className="spread" style={{ marginBottom: '.75rem' }}>
              <Badge tone={STATUS_TONE[open.application.status] ?? 'brand'}>{open.application.status.replace(/_/g, ' ')}</Badge>
              <span className="muted small">{open.application.reference}{open.application.partner_id ? ` · partner #${open.application.partner_id}` : ''}</span>
            </div>

            <Section title="Contact">
              <KV k="Contact name" v={open.application.contact_name} />
              <KV k="Email" v={open.application.contact_email} />
              <KV k="Phone" v={open.application.contact_phone} />
              <KV k="Website" v={open.application.website} />
              <KV k="Location" v={[open.application.city, open.application.region, open.application.country].filter(Boolean).join(', ')} />
            </Section>
            <Section title="Programme">
              <KV k="Delivery modes" v={open.application.delivery_modes} />
              <KV k="Learners / year" v={open.application.learners_per_year != null ? String(open.application.learners_per_year) : null} />
              <KV k="Specialties" v={open.application.specialties} block />
              <KV k="About" v={open.application.description} block />
            </Section>
            <Section title={`Documents (${open.documents.length})`}>
              {open.documents.length === 0 ? <p className="muted small">No documents attached.</p> : (
                <ul className="clean" style={{ display: 'grid', gap: '.4rem', paddingLeft: 0, listStyle: 'none' }}>
                  {open.documents.map((d) => (
                    <li key={d.id} className="row" style={{ justifyContent: 'space-between' }}>
                      <span className="small">{DOC_LABEL[d.doc_kind] || d.doc_kind}{d.filename ? ` — ${d.filename}` : ''}</span>
                      <button className="btn ghost sm" onClick={() => download(open.application.id, d.id)}>Download</button>
                    </li>
                  ))}
                </ul>
              )}
            </Section>

            <div className="field" style={{ marginTop: '.5rem' }}>
              <label htmlFor="tp-tier">Tier to grant on approval</label>
              <select id="tp-tier" value={tier} onChange={(e) => setTier(e.target.value)} style={{ maxWidth: 220 }}>
                {TIERS.map((t) => <option key={t} value={t}>{t}</option>)}
              </select>
            </div>
            <div className="field" style={{ marginTop: '.5rem' }}>
              <label htmlFor="tp-note">Internal note / decision note</label>
              <textarea id="tp-note" rows={3} value={note} onChange={(e) => setNote(e.target.value)} placeholder="Recorded on the application; sent to the applicant on a decision." />
            </div>
            <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
              <button className="btn sm secondary" disabled={busy} onClick={() => decide(open.application.id, '')}>Save note</button>
              {open.application.status !== 'under_review' && open.application.status !== 'approved' && (
                <button className="btn sm secondary" disabled={busy} onClick={() => decide(open.application.id, 'under_review')}>Mark under review</button>
              )}
              {open.application.status !== 'approved' && (
                <button className="btn sm" disabled={busy} onClick={() => decide(open.application.id, 'approved')}>Approve &amp; create entry</button>
              )}
              {open.application.status !== 'rejected' && open.application.status !== 'approved' && (
                <button className="btn sm danger" disabled={busy} onClick={() => decide(open.application.id, 'rejected')}>Reject</button>
              )}
            </div>
            {open.application.status === 'approved' && open.application.partner_id && (
              <p className="muted small" style={{ marginTop: '.6rem' }}>A directory entry was created (unlisted). Publish it under the <strong>Directory</strong> tab to show it on the public site.</p>
            )}
          </div>
        </div>
      )}
    </>
  )
}

function Section({ title, children }: { title: string; children: React.ReactNode }) {
  return <div style={{ marginBottom: '1rem' }}><h4 style={{ margin: '0 0 .4rem' }}>{title}</h4><div style={{ display: 'grid', gap: '.3rem' }}>{children}</div></div>
}
function KV({ k, v, block }: { k: string; v?: string | null; block?: boolean }) {
  if (block) return <div><div className="muted small">{k}</div><div className="small" style={{ whiteSpace: 'pre-wrap' }}>{v || '—'}</div></div>
  return <div className="spread small"><span className="muted">{k}</span><span style={{ textAlign: 'right' }}>{v || '—'}</span></div>
}
