import { useMemo, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import { adminApi } from '../api'
import { useAdminQuery } from '../hooks'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'

// Admin → Partner Finance. The operator console over the immutable partner-finance module
// (/api/admin/partner-finance/*): the per-transaction commission ledger, effective-dated
// agreements + commission rules, the maker-checker settlement flow (prepare → approve → pay,
// enforced server-side — the approver must differ from the preparer), statements and disputes.
// Everything shown here is the same data the partner sees in their portal, so the two sides
// can never disagree about a number. Serves training AND marketing partners alike.

interface PartnerRow { id: number; name: string; partner_type?: string | null; status?: string | null; is_test?: number | null }

interface LedgerTotals { gross: number; net: number; commission: number; pending: number; due: number; approved: number; paid: number; reversed: number }
interface LedgerTxn {
  id: number; reference?: string | null; status?: string | null; product_type?: string | null; currency?: string | null
  gross: number; discount: number; eligible_net: number; commission_type?: string | null; commission_percent: number
  commission_basis?: string | null; commission: number; earned_at?: string | null; hold_until?: string | null
  reversal_of?: number | null; requires_finance_review?: boolean
}
interface LedgerResp { totals: LedgerTotals; count: number; transactions: LedgerTxn[] }

interface Agreement {
  id: number; agreement_number?: string | null; effective_from?: string | null; effective_to?: string | null
  currency?: string | null; payment_terms_days?: number | null; minimum_payout_minor?: number | null
  refund_hold_days?: number | null; tax_treatment?: string | null; status?: string | null; note?: string | null
}
interface Rule {
  id: number; agreement_id: number; certification_id?: number | null; route_key?: string | null
  product_type?: string | null; country?: string | null; commission_type?: string | null
  commission_rate_bp?: number | null; commission_fixed_minor?: number | null; commission_basis?: string | null
  priority?: number | null; active?: number | null; effective_from?: string | null; effective_to?: string | null
}
interface Settlement {
  id: number; settlement_no?: string | null; period_start?: string | null; period_end?: string | null
  currency?: string | null; eligible_commission_minor?: number | null; adjustments_minor?: number | null
  amount_approved_minor?: number | null; amount_paid_minor?: number | null; closing_balance_minor?: number | null
  status?: string | null; prepared_by?: number | null; approved_by?: number | null; paid_at?: string | null
  payment_method?: string | null; payment_reference?: string | null
}
interface SettlementsResp {
  payable: number; recoverable: number; recoverable_outstanding: number
  by_currency?: Array<{ currency?: string; payable?: number }> | Record<string, unknown>
  settlements: Settlement[]
}
interface Dispute {
  id: number; dispute_no?: string | null; partner_id: number; partner_name?: string | null; category?: string | null
  subject?: string | null; detail?: string | null; claimed_amount_minor?: number | null; status?: string | null
  resolution?: string | null; created_at?: string | null
}
interface DisputeMessage { id: number; author_type?: string | null; body?: string | null; internal?: number | null; created_at?: string | null }

const money = (v: unknown) => (v == null || Number.isNaN(Number(v)) ? '—' : Number(v).toLocaleString('en-GB', { minimumFractionDigits: 2, maximumFractionDigits: 2 }))
const minor = (v: unknown) => money(Number(v ?? 0) / 100)
const day = (s?: string | null) => (s ? String(s).slice(0, 10) : '—')

const STATUS_TONE: Record<string, 'ok' | 'warn' | 'err' | 'brand' | 'neutral'> = {
  paid: 'ok', approved: 'brand', prepared: 'warn', cancelled: 'err', partially_paid: 'warn',
  due: 'warn', on_refund_hold: 'warn', reversed: 'err', settled: 'ok', open: 'warn', under_review: 'brand',
  resolved: 'ok', rejected: 'err', withdrawn: 'neutral', active: 'ok', superseded: 'neutral',
}
const tone = (s?: string | null) => STATUS_TONE[String(s ?? '').toLowerCase()] ?? 'neutral'
const StatusChip = ({ s }: { s?: string | null }) => <Badge tone={tone(s)}>{String(s ?? '—').replace(/_/g, ' ')}</Badge>

/** Authenticated file download (remittance/statement PDFs & CSVs) — a plain <a href> would 401. */
async function downloadFile(path: string, filename: string, onErr: (m: string) => void) {
  try {
    const tok = sessionStorage.getItem('pci.admin.token')
    const res = await fetch(path, { headers: tok ? { Authorization: 'Bearer ' + tok } : {} })
    if (!res.ok) { onErr(`Download failed (${res.status}).`); return }
    const href = URL.createObjectURL(await res.blob())
    const a = document.createElement('a')
    a.href = href; a.download = filename
    document.body.appendChild(a); a.click(); a.remove()
    setTimeout(() => URL.revokeObjectURL(href), 60_000)
  } catch { onErr('Download failed.') }
}

const TABS = ['Ledger', 'Agreements', 'Settlements', 'Statement', 'Disputes'] as const

export default function PartnerFinance() {
  const [params, setParams] = useSearchParams()
  const partnerId = Number(params.get('partner') ?? 0) || null
  const [tab, setTab] = useState<(typeof TABS)[number]>('Ledger')
  const { data: dir, loading, error } = useAdminQuery<{ rows: PartnerRow[] }>('/api/admin/training-partners')
  // Test institutions stay pickable (flagged) so the whole finance flow can be exercised safely.
  const partners = useMemo(() => dir?.rows ?? [], [dir])
  const partner = partners.find((p) => p.id === partnerId) ?? null

  return (
    <div className="page">
      <PageHeader
        title="Partner Finance"
        subtitle={<>The operator side of the partner-finance module: the immutable commission ledger, effective-dated agreements and rules, maker-checker settlements (a settlement must be approved by someone other than its preparer), statements and disputes. Figures come from the same ledger the partner portal reads.</>}
      />
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : (
        <>
          <div className="row" style={{ gap: '.6rem', flexWrap: 'wrap', alignItems: 'center' }}>
            <label className="row" style={{ gap: '.4rem', alignItems: 'center' }}>
              <span className="small muted" style={{ fontWeight: 700 }}>Partner</span>
              <select
                value={partnerId ?? ''}
                onChange={(e) => setParams(e.target.value ? { partner: e.target.value } : {})}
                style={{ width: 'auto', minWidth: 260 }}
              >
                <option value="">Choose a partner…</option>
                {partners.map((p) => (
                  <option key={p.id} value={p.id}>{p.name}{p.partner_type === 'marketing' ? ' — marketing' : ''}{p.is_test ? ' (test)' : ''}</option>
                ))}
              </select>
            </label>
            {partner?.partner_type === 'marketing' && <Badge tone="brand">marketing</Badge>}
            {partner && TABS.map((t) => (
              <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>
            ))}
          </div>
          {!partner ? (
            <Card><Empty>Pick a partner to see their ledger, agreements, settlements, statement and disputes. Disputes across all partners are under any partner's Disputes tab.</Empty></Card>
          ) : (
            <>
              {tab === 'Ledger' && <LedgerTab partnerId={partner.id} />}
              {tab === 'Agreements' && <AgreementsTab partnerId={partner.id} />}
              {tab === 'Settlements' && <SettlementsTab partnerId={partner.id} />}
              {tab === 'Statement' && <StatementTab partnerId={partner.id} />}
              {tab === 'Disputes' && <DisputesTab partnerId={partner.id} />}
            </>
          )}
        </>
      )}
    </div>
  )
}

// ---------------- Ledger ----------------
function LedgerTab({ partnerId }: { partnerId: number }) {
  const { data, loading, error } = useAdminQuery<LedgerResp>(`/api/admin/partner-finance/${partnerId}/ledger?limit=100`)
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const t = data?.totals
  const rows = data?.transactions ?? []
  return (
    <>
      <div className="grid cols-4">
        <Card title="Eligible net"><strong>{money(t?.net)}</strong><div className="muted small">gross {money(t?.gross)}</div></Card>
        <Card title="Commission earned"><strong>{money(t?.commission)}</strong><div className="muted small">{data?.count ?? 0} transactions</div></Card>
        <Card title="On hold / due / approved"><strong>{money((t?.pending ?? 0) + (t?.due ?? 0) + (t?.approved ?? 0))}</strong><div className="muted small">hold {money(t?.pending)} · due {money(t?.due)} · approved {money(t?.approved)}</div></Card>
        <Card title="Paid"><strong>{money(t?.paid)}</strong><div className="muted small">reversed {money(t?.reversed)}</div></Card>
      </div>
      <Card title="Ledger transactions (latest 100)">
        {rows.length === 0 ? <Empty>No commission transactions yet.</Empty> : (
          <table className="data">
            <thead><tr><th>Date</th><th>Reference</th><th>Type</th><th style={{ textAlign: 'right' }}>Eligible net</th><th style={{ textAlign: 'right' }}>Rate</th><th style={{ textAlign: 'right' }}>Commission</th><th>Status</th></tr></thead>
            <tbody>
              {rows.map((x) => (
                <tr key={x.id}>
                  <td className="small">{day(x.earned_at)}</td>
                  <td className="small">
                    <span style={{ fontFamily: 'ui-monospace, monospace' }}>{x.reference ?? '—'}</span>
                    {x.reversal_of ? <div className="muted small">reverses #{x.reversal_of}</div> : null}
                    {x.requires_finance_review ? <div><Badge tone="warn">needs finance review</Badge></div> : null}
                  </td>
                  <td className="small">{x.product_type ?? '—'}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{x.currency ?? 'USD'} {money(x.eligible_net)}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{x.commission_type === 'fixed' ? 'fixed' : `${x.commission_percent}%`}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{x.currency ?? 'USD'} {money(x.commission)}</td>
                  <td><StatusChip s={x.status} /></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </>
  )
}

// ---------------- Agreements & rules ----------------
function AgreementsTab({ partnerId }: { partnerId: number }) {
  const { data, loading, error, refetch } = useAdminQuery<{ agreements: Agreement[]; rules: Rule[] }>(`/api/admin/partner-finance/${partnerId}/agreements`)
  const [showNew, setShowNew] = useState(false)
  const [ruleFor, setRuleFor] = useState<Agreement | null>(null)
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const agreements = data?.agreements ?? []
  const rules = data?.rules ?? []
  return (
    <>
      <Card title={`Agreements (${agreements.length})`} action={<button className="btn sm" onClick={() => setShowNew(true)}>New agreement</button>}>
        <p className="muted small" style={{ marginTop: 0 }}>Commercial terms are effective-dated; creating a new agreement supersedes the currently active one, so exactly one is in force at a time. Rates live in the rules below, matched per transaction by certification / product / country specificity.</p>
        {agreements.length === 0 ? <Empty>No agreement yet — commissions snapshot the partner's flat percentage and are flagged for finance review.</Empty> : (
          <table className="data">
            <thead><tr><th>Number</th><th>Window</th><th>Currency</th><th style={{ textAlign: 'right' }}>Terms</th><th style={{ textAlign: 'right' }}>Min payout</th><th style={{ textAlign: 'right' }}>Refund hold</th><th>Status</th><th /></tr></thead>
            <tbody>
              {agreements.map((a) => (
                <tr key={a.id}>
                  <td className="small" style={{ fontFamily: 'ui-monospace, monospace' }}>{a.agreement_number}</td>
                  <td className="small">{day(a.effective_from)} → {a.effective_to ? day(a.effective_to) : 'open'}</td>
                  <td className="small">{a.currency ?? 'USD'}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{a.payment_terms_days ?? 0} d</td>
                  <td className="small" style={{ textAlign: 'right' }}>{minor(a.minimum_payout_minor)}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{a.refund_hold_days ?? 0} d</td>
                  <td><StatusChip s={a.status} /></td>
                  <td style={{ textAlign: 'right' }}><button className="btn ghost sm" onClick={() => setRuleFor(a)}>Add rule</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
      <Card title={`Commission rules (${rules.length})`}>
        {rules.length === 0 ? <Empty>No rules yet. Add one to an agreement — dimensions left blank match anything; the most specific rule wins.</Empty> : (
          <table className="data">
            <thead><tr><th>Agreement</th><th>Scope</th><th style={{ textAlign: 'right' }}>Rate</th><th>Basis</th><th style={{ textAlign: 'right' }}>Priority</th><th>Status</th></tr></thead>
            <tbody>
              {rules.map((r) => (
                <tr key={r.id}>
                  <td className="small">#{r.agreement_id}</td>
                  <td className="small">{[
                    r.certification_id ? `cert ${r.certification_id}` : null,
                    r.product_type ? `product ${r.product_type}` : null,
                    r.country ? `country ${r.country}` : null,
                    r.route_key ? `route ${r.route_key}` : null,
                  ].filter(Boolean).join(' · ') || 'any'}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{r.commission_type === 'fixed' ? minor(r.commission_fixed_minor) + ' fixed' : `${(Number(r.commission_rate_bp ?? 0) / 100)}%`}</td>
                  <td className="small">{String(r.commission_basis ?? '').replace(/_/g, ' ') || '—'}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{r.priority ?? 100}</td>
                  <td>{r.active ? <Badge tone="ok">active</Badge> : <Badge tone="neutral">inactive</Badge>}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
      {showNew && <NewAgreementDrawer partnerId={partnerId} onClose={() => setShowNew(false)} onSaved={() => { setShowNew(false); refetch() }} />}
      {ruleFor && <NewRuleDrawer agreement={ruleFor} onClose={() => setRuleFor(null)} onSaved={() => { setRuleFor(null); refetch() }} />}
    </>
  )
}

function NewAgreementDrawer({ partnerId, onClose, onSaved }: { partnerId: number; onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState({ effective_from: '', effective_to: '', currency: 'USD', payment_terms_days: '30', minimum_payout: '0', refund_hold_days: '30', tax_treatment: '', note: '' })
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const set = (k: keyof typeof f) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })
  async function save() {
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/partner-finance/${partnerId}/agreements`, {
        effective_from: f.effective_from, effective_to: f.effective_to || null, currency: f.currency,
        payment_terms_days: Number(f.payment_terms_days || 30), minimum_payout: Number(f.minimum_payout || 0),
        refund_hold_days: Number(f.refund_hold_days || 30), tax_treatment: f.tax_treatment || null, note: f.note || null,
      })
      onSaved()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Save failed.') } finally { setBusy(false) }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()} style={{ maxWidth: 480 }}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>New agreement</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        <p className="muted small">Creating this supersedes the partner's currently active agreement.</p>
        {err && <ErrorNote>{err}</ErrorNote>}
        <div style={{ display: 'grid', gap: '.55rem' }}>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Effective from *<input type="date" value={f.effective_from} onChange={set('effective_from')} /></label>
            <label>Effective to <span className="muted small">(blank = open)</span><input type="date" value={f.effective_to} onChange={set('effective_to')} /></label>
          </div>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr 1fr' }}>
            <label>Currency<input value={f.currency} onChange={set('currency')} maxLength={3} /></label>
            <label>Terms (days)<input type="number" min="0" max="365" value={f.payment_terms_days} onChange={set('payment_terms_days')} /></label>
            <label>Refund hold (days)<input type="number" min="0" max="365" value={f.refund_hold_days} onChange={set('refund_hold_days')} /></label>
          </div>
          <label>Minimum payout<input type="number" min="0" step="0.01" value={f.minimum_payout} onChange={set('minimum_payout')} /></label>
          <label>Tax treatment<input value={f.tax_treatment} onChange={set('tax_treatment')} placeholder="e.g. self-billing, withholding 5%" /></label>
          <label>Note<textarea rows={2} value={f.note} onChange={set('note')} /></label>
          <div className="row" style={{ justifyContent: 'flex-end' }}>
            <button className="btn" disabled={busy || f.effective_from.length !== 10} onClick={save}>{busy ? 'Saving…' : 'Create agreement'}</button>
          </div>
        </div>
      </div>
    </div>
  )
}

function NewRuleDrawer({ agreement, onClose, onSaved }: { agreement: Agreement; onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState({ commission_type: 'percentage', commission_percent: '10', commission_fixed: '0', commission_basis: 'net_after_discount', certification_id: '', product_type: '', country: '', priority: '100' })
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const set = (k: keyof typeof f) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })
  async function save() {
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/partner-finance/agreements/${agreement.id}/rules`, {
        commission_type: f.commission_type,
        commission_percent: Number(f.commission_percent || 0),
        commission_fixed: Number(f.commission_fixed || 0),
        commission_basis: f.commission_basis,
        certification_id: f.certification_id ? Number(f.certification_id) : null,
        product_type: f.product_type || null,
        country: f.country || null,
        priority: Number(f.priority || 100),
      })
      onSaved()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Save failed.') } finally { setBusy(false) }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()} style={{ maxWidth: 480 }}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Add rule to {agreement.agreement_number}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        <p className="muted small">Blank dimensions match anything; the most specific matching rule wins per transaction. An out-of-range rate is rejected, never clamped.</p>
        {err && <ErrorNote>{err}</ErrorNote>}
        <div style={{ display: 'grid', gap: '.55rem' }}>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Type
              <select value={f.commission_type} onChange={set('commission_type')}>
                <option value="percentage">Percentage</option>
                <option value="fixed">Fixed amount</option>
              </select>
            </label>
            {f.commission_type === 'percentage'
              ? <label>Percent<input type="number" min="0" max="100" step="0.25" value={f.commission_percent} onChange={set('commission_percent')} /></label>
              : <label>Fixed amount<input type="number" min="0" step="0.01" value={f.commission_fixed} onChange={set('commission_fixed')} /></label>}
          </div>
          <label>Basis
            <select value={f.commission_basis} onChange={set('commission_basis')}>
              <option value="net_after_discount">Net after discount</option>
              <option value="gross">Gross</option>
              <option value="net_after_tax">Net after tax</option>
            </select>
          </label>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr 1fr' }}>
            <label>Certification id<input type="number" value={f.certification_id} onChange={set('certification_id')} placeholder="any" /></label>
            <label>Product type<input value={f.product_type} onChange={set('product_type')} placeholder="any" /></label>
            <label>Country<input value={f.country} onChange={set('country')} placeholder="any" /></label>
          </div>
          <label>Priority <span className="muted small">(lower wins first; 1–1000)</span><input type="number" min="1" max="1000" value={f.priority} onChange={set('priority')} /></label>
          <div className="row" style={{ justifyContent: 'flex-end' }}>
            <button className="btn" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Add rule'}</button>
          </div>
        </div>
      </div>
    </div>
  )
}

// ---------------- Settlements ----------------
function SettlementsTab({ partnerId }: { partnerId: number }) {
  const { data, loading, error, refetch } = useAdminQuery<SettlementsResp>(`/api/admin/partner-finance/${partnerId}/settlements`)
  const [showPrepare, setShowPrepare] = useState(false)
  const [payFor, setPayFor] = useState<Settlement | null>(null)
  const [busyId, setBusyId] = useState<number | null>(null)
  const [err, setErr] = useState<string | null>(null)
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.settlements ?? []

  async function act(s: Settlement, path: string, body: Record<string, unknown>, confirmMsg?: string) {
    if (confirmMsg && !confirm(confirmMsg)) return
    setBusyId(s.id); setErr(null)
    try { await adminApi.post(`/api/admin/partner-finance/settlements/${s.id}/${path}`, body); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Action failed.') } finally { setBusyId(null) }
  }

  return (
    <>
      {err && <div className="notice err" role="alert">{err}</div>}
      <div className="grid cols-3">
        <Card title="Payable now"><strong>{money(data?.payable)}</strong><div className="muted small">due + approved commission not yet settled</div></Card>
        <Card title="Recoverable"><strong>{money(data?.recoverable)}</strong><div className="muted small">outstanding {money(data?.recoverable_outstanding)}</div></Card>
        <Card title="Maker-checker"><div className="muted small" style={{ marginTop: '.2rem' }}>A settlement must be approved by a different admin than the one who prepared it — the engine refuses self-approval even for owners.</div></Card>
      </div>
      <Card title={`Settlements (${rows.length})`} action={<button className="btn sm" onClick={() => setShowPrepare(true)}>Prepare settlement</button>}>
        {rows.length === 0 ? <Empty>No settlements yet. Prepare one to batch the partner's due commission for payment.</Empty> : (
          <table className="data">
            <thead><tr><th>Settlement</th><th>Period</th><th style={{ textAlign: 'right' }}>Eligible</th><th style={{ textAlign: 'right' }}>Approved</th><th style={{ textAlign: 'right' }}>Paid</th><th>Status</th><th /></tr></thead>
            <tbody>
              {rows.map((s) => (
                <tr key={s.id}>
                  <td className="small" style={{ fontFamily: 'ui-monospace, monospace' }}>{s.settlement_no}</td>
                  <td className="small">{day(s.period_start)} → {day(s.period_end)}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{s.currency ?? 'USD'} {minor(s.eligible_commission_minor)}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{minor(s.amount_approved_minor)}</td>
                  <td className="small" style={{ textAlign: 'right' }}>{minor(s.amount_paid_minor)}{s.paid_at ? <div className="muted small">{day(s.paid_at)}</div> : null}</td>
                  <td><StatusChip s={s.status} /></td>
                  <td className="row" style={{ gap: '.3rem', justifyContent: 'flex-end' }}>
                    {s.status === 'prepared' && (
                      <button className="btn ghost sm" disabled={busyId === s.id} onClick={() => act(s, 'approve', {})}>Approve</button>
                    )}
                    {(s.status === 'approved' || s.status === 'partially_paid') && (
                      <button className="btn ghost sm" disabled={busyId === s.id} onClick={() => setPayFor(s)}>Record payment</button>
                    )}
                    {(s.status === 'prepared' || s.status === 'approved') && (
                      <button className="btn ghost sm danger" disabled={busyId === s.id}
                        onClick={() => { const reason = prompt('Reason for cancelling this settlement:'); if (reason) void act(s, 'cancel', { reason }) }}>Cancel</button>
                    )}
                    <button className="btn ghost sm" onClick={() => void downloadFile(`/api/admin/partner-finance/settlements/${s.id}/remittance`, `remittance-${s.id}.pdf`, setErr)}>Remittance</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
      {showPrepare && <PrepareDrawer partnerId={partnerId} onClose={() => setShowPrepare(false)} onSaved={() => { setShowPrepare(false); refetch() }} />}
      {payFor && (
        <PayDrawer settlement={payFor} onClose={() => setPayFor(null)}
          onSaved={() => { setPayFor(null); refetch() }} />
      )}
    </>
  )
}

function PrepareDrawer({ partnerId, onClose, onSaved }: { partnerId: number; onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState({ period_start: '', period_end: '', cap_amount: '', note: '' })
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const set = (k: keyof typeof f) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })
  async function save() {
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/partner-finance/${partnerId}/settlements`, {
        period_start: f.period_start || null, period_end: f.period_end || null,
        cap_amount: f.cap_amount ? Number(f.cap_amount) : null, note: f.note || null,
      })
      onSaved()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Prepare failed.') } finally { setBusy(false) }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()} style={{ maxWidth: 460 }}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Prepare settlement</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        <p className="muted small">Batches the partner's due commission (one currency per batch). Nothing becomes payable until a different admin approves it.</p>
        {err && <ErrorNote>{err}</ErrorNote>}
        <div style={{ display: 'grid', gap: '.55rem' }}>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Period start<input type="date" value={f.period_start} onChange={set('period_start')} /></label>
            <label>Period end<input type="date" value={f.period_end} onChange={set('period_end')} /></label>
          </div>
          <label>Cap amount <span className="muted small">(optional — limit this batch)</span><input type="number" min="0" step="0.01" value={f.cap_amount} onChange={set('cap_amount')} /></label>
          <label>Note<textarea rows={2} value={f.note} onChange={set('note')} /></label>
          <div className="row" style={{ justifyContent: 'flex-end' }}>
            <button className="btn" disabled={busy} onClick={save}>{busy ? 'Preparing…' : 'Prepare'}</button>
          </div>
        </div>
      </div>
    </div>
  )
}

function PayDrawer({ settlement, onClose, onSaved }: { settlement: Settlement; onClose: () => void; onSaved: () => void }) {
  const outstanding = (Number(settlement.amount_approved_minor ?? 0) - Number(settlement.amount_paid_minor ?? 0)) / 100
  const [f, setF] = useState({ amount: '', method: 'bank_transfer', reference: '', partner_note: '' })
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const set = (k: keyof typeof f) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })
  async function save() {
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/partner-finance/settlements/${settlement.id}/pay`, {
        amount: f.amount ? Number(f.amount) : undefined,
        method: f.method || null, reference: f.reference || null, partner_note: f.partner_note || null,
      })
      onSaved()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Payment record failed.') } finally { setBusy(false) }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()} style={{ maxWidth: 460 }}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Record payment — {settlement.settlement_no}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        <p className="muted small">Outstanding: <strong>{settlement.currency ?? 'USD'} {money(outstanding)}</strong>. Leave the amount blank to pay it in full; partial payments are supported and re-checked by the engine.</p>
        {err && <ErrorNote>{err}</ErrorNote>}
        <div style={{ display: 'grid', gap: '.55rem' }}>
          <label>Amount <span className="muted small">(blank = outstanding balance)</span><input type="number" min="0.01" step="0.01" value={f.amount} onChange={set('amount')} placeholder={String(outstanding)} /></label>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Method
              <select value={f.method} onChange={set('method')}>
                <option value="bank_transfer">Bank transfer</option>
                <option value="wise">Wise</option>
                <option value="paypal">PayPal</option>
                <option value="cheque">Cheque</option>
                <option value="other">Other</option>
              </select>
            </label>
            <label>Reference<input value={f.reference} onChange={set('reference')} placeholder="bank ref / txn id" /></label>
          </div>
          <label>Note to partner<textarea rows={2} value={f.partner_note} onChange={set('partner_note')} /></label>
          <div className="row" style={{ justifyContent: 'flex-end' }}>
            <button className="btn" disabled={busy} onClick={save}>{busy ? 'Recording…' : 'Record payment'}</button>
          </div>
        </div>
      </div>
    </div>
  )
}

// ---------------- Statement ----------------
function StatementTab({ partnerId }: { partnerId: number }) {
  const now = new Date()
  const defaultMonth = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`
  const [month, setMonth] = useState(defaultMonth)
  const [err, setErr] = useState<string | null>(null)
  const { data, loading, error } = useAdminQuery<Record<string, unknown>>(
    month.length === 7 ? `/api/admin/partner-finance/${partnerId}/statement?month=${month}` : null)
  const totals = (data?.totals ?? data?.summary ?? null) as Record<string, unknown> | null
  const lines = (data?.lines ?? data?.transactions ?? []) as Array<Record<string, unknown>>
  return (
    <Card
      title="Monthly statement"
      action={
        <span className="row" style={{ gap: '.4rem', alignItems: 'center' }}>
          <input type="month" value={month} onChange={(e) => setMonth(e.target.value)} style={{ width: 'auto' }} />
          <button className="btn sm secondary" onClick={() => void downloadFile(`/api/admin/partner-finance/${partnerId}/statement?month=${month}&format=csv`, `statement-${partnerId}-${month}.csv`, setErr)}>CSV</button>
          <button className="btn sm secondary" onClick={() => void downloadFile(`/api/admin/partner-finance/${partnerId}/statement?month=${month}&format=pdf`, `statement-${partnerId}-${month}.pdf`, setErr)}>PDF</button>
        </span>
      }
    >
      <p className="muted small" style={{ marginTop: 0 }}>Derived from the ledger on demand and identical to the partner's own statement — one builder serves both sides. Balances to opening + earned + reversals − paid = closing.</p>
      {err && <div className="notice err" role="alert">{err}</div>}
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : (
        <>
          {totals && (
            <div className="row" style={{ gap: '1.2rem', flexWrap: 'wrap', marginBottom: '.7rem' }}>
              {Object.entries(totals).filter(([, v]) => typeof v === 'number' || typeof v === 'string').slice(0, 8).map(([k, v]) => (
                <div key={k}><div className="muted small" style={{ textTransform: 'capitalize' }}>{k.replace(/_/g, ' ')}</div><strong>{typeof v === 'number' ? money(v) : String(v)}</strong></div>
              ))}
            </div>
          )}
          {lines.length === 0 ? <Empty>No commission lines in this month.</Empty> : (
            <table className="data">
              <thead><tr><th>Date</th><th>Reference</th><th>Type</th><th style={{ textAlign: 'right' }}>Eligible net</th><th style={{ textAlign: 'right' }}>Commission</th><th>Status</th></tr></thead>
              <tbody>
                {lines.map((l, i) => (
                  <tr key={i}>
                    <td className="small">{day(String(l.earned_at ?? l.date ?? ''))}</td>
                    <td className="small" style={{ fontFamily: 'ui-monospace, monospace' }}>{String(l.reference ?? l.txn_ref ?? '—')}</td>
                    <td className="small">{String(l.product_type ?? l.type ?? '—')}</td>
                    <td className="small" style={{ textAlign: 'right' }}>{money(l.eligible_net ?? l.net)}</td>
                    <td className="small" style={{ textAlign: 'right' }}>{money(l.commission)}</td>
                    <td><StatusChip s={String(l.status ?? '')} /></td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </>
      )}
    </Card>
  )
}

// ---------------- Disputes ----------------
function DisputesTab({ partnerId }: { partnerId: number }) {
  const [scope, setScope] = useState<'partner' | 'all'>('partner')
  const { data, loading, error, refetch } = useAdminQuery<{ disputes: Dispute[] }>(
    `/api/admin/partner-finance/disputes${scope === 'partner' ? `?partner_id=${partnerId}` : ''}`)
  const [open, setOpen] = useState<Dispute | null>(null)
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.disputes ?? []
  return (
    <Card
      title={`Disputes (${rows.length})`}
      action={
        <span className="row" style={{ gap: '.3rem' }}>
          <button className={'btn sm' + (scope === 'partner' ? '' : ' ghost')} onClick={() => setScope('partner')}>This partner</button>
          <button className={'btn sm' + (scope === 'all' ? '' : ' ghost')} onClick={() => setScope('all')}>All partners</button>
        </span>
      }
    >
      <p className="muted small" style={{ marginTop: 0 }}>Nothing in the ledger is ever edited to settle a dispute — a correction lands as a new linked adjustment, so the original figure and the reason it changed both stay visible.</p>
      {rows.length === 0 ? <Empty>No disputes.</Empty> : (
        <table className="data">
          <thead><tr><th>Reference</th><th>Partner</th><th>Subject</th><th>Category</th><th style={{ textAlign: 'right' }}>Claimed</th><th>Status</th><th /></tr></thead>
          <tbody>
            {rows.map((d) => (
              <tr key={d.id}>
                <td className="small" style={{ fontFamily: 'ui-monospace, monospace' }}>{d.dispute_no}</td>
                <td className="small">{d.partner_name ?? `#${d.partner_id}`}</td>
                <td className="small">{d.subject}</td>
                <td className="small">{String(d.category ?? '—').replace(/_/g, ' ')}</td>
                <td className="small" style={{ textAlign: 'right' }}>{d.claimed_amount_minor ? minor(d.claimed_amount_minor) : '—'}</td>
                <td><StatusChip s={d.status} /></td>
                <td style={{ textAlign: 'right' }}><button className="btn ghost sm" onClick={() => setOpen(d)}>Open</button></td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {open && <DisputeDrawer dispute={open} onClose={() => setOpen(null)} onChanged={() => { setOpen(null); refetch() }} />}
    </Card>
  )
}

function DisputeDrawer({ dispute, onClose, onChanged }: { dispute: Dispute; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<{ dispute: Dispute; messages: DisputeMessage[] }>(`/api/admin/partner-finance/disputes/${dispute.id}`)
  const [reply, setReply] = useState('')
  const [visibility, setVisibility] = useState<'partner' | 'internal'>('partner')
  const [status, setStatus] = useState('')
  const [resolution, setResolution] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const d = data?.dispute ?? dispute
  async function save() {
    setBusy(true); setErr(null)
    try {
      await adminApi.post(`/api/admin/partner-finance/disputes/${dispute.id}`, {
        reply: reply || undefined, visibility, status: status || undefined, resolution: resolution || undefined,
      })
      setReply('')
      if (status === 'resolved' || status === 'rejected') onChanged()
      else refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Update failed.') } finally { setBusy(false) }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()} style={{ maxWidth: 560 }}>
        <div className="spread" style={{ marginBottom: '.8rem' }}>
          <h2 style={{ margin: 0 }}>{d.dispute_no} — {d.subject}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        <div className="row" style={{ gap: '.4rem', marginBottom: '.6rem' }}>
          <StatusChip s={d.status} />
          <span className="muted small">{String(d.category ?? '').replace(/_/g, ' ')} · raised {day(d.created_at)}</span>
        </div>
        {d.detail && <p className="small" style={{ whiteSpace: 'pre-wrap' }}>{d.detail}</p>}
        {d.resolution && <div className="notice" style={{ marginBottom: '.6rem' }}>Resolution: {d.resolution}</div>}
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : (
          <div style={{ display: 'grid', gap: '.45rem', marginBottom: '.8rem' }}>
            {(data?.messages ?? []).map((m) => (
              <div key={m.id} className="small" style={{ padding: '.5rem .7rem', borderRadius: 9, background: m.author_type === 'admin' ? 'var(--brand-050)' : 'var(--canvas)' }}>
                <div className="muted small">{m.author_type === 'admin' ? 'PCI' : 'Partner'}{m.internal ? ' · internal' : ''} · {day(m.created_at)}</div>
                <div style={{ whiteSpace: 'pre-wrap' }}>{m.body}</div>
              </div>
            ))}
          </div>
        )}
        {err && <ErrorNote>{err}</ErrorNote>}
        <div style={{ display: 'grid', gap: '.55rem' }}>
          <label>Reply<textarea rows={3} value={reply} onChange={(e) => setReply(e.target.value)} /></label>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Visibility
              <select value={visibility} onChange={(e) => setVisibility(e.target.value as 'partner' | 'internal')}>
                <option value="partner">Visible to partner</option>
                <option value="internal">Internal note</option>
              </select>
            </label>
            <label>Status
              <select value={status} onChange={(e) => setStatus(e.target.value)}>
                <option value="">Leave unchanged</option>
                <option value="under_review">Under review</option>
                <option value="resolved">Resolved</option>
                <option value="rejected">Rejected</option>
              </select>
            </label>
          </div>
          {(status === 'resolved' || status === 'rejected') && (
            <label>Resolution * <span className="muted small">(required when closing)</span>
              <textarea rows={2} value={resolution} onChange={(e) => setResolution(e.target.value)} />
            </label>
          )}
          <div className="row" style={{ justifyContent: 'flex-end' }}>
            <button className="btn" disabled={busy || (!reply && !status)} onClick={save}>{busy ? 'Saving…' : 'Save'}</button>
          </div>
        </div>
      </div>
    </div>
  )
}
