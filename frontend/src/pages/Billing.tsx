import { useEffect, useMemo, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import { useMe } from '../data/MeContext'
import { useQuery } from '../api/hooks'
import { api } from '../api/client'
import { startCheckout, checkoutErrorMessage } from '../api/checkout'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../components/ui'
import FoundingCard from '../components/FoundingCard'
import { fmtDate, fmtMoney, titleCase } from '../format'
import { openPrintable, escapeHtml as e } from '../print'
import type { Payment } from '../api/types'

interface PriceBlock {
  final: number
  standard: number
  defaultDiscount: number
}
interface PricingResp {
  currency: string
  membership: PriceBlock
  exam: PriceBlock
  bundle: PriceBlock
  renewal: PriceBlock
  recert: PriceBlock
  cert?: { code: string; name: string } | null
}

/** Whole days from now until an ISO date (negative once it's in the past); null if unparseable. */
function daysUntil(iso?: string | null): number | null {
  if (!iso) return null
  const t = new Date(iso).getTime()
  if (Number.isNaN(t)) return null
  return Math.ceil((t - Date.now()) / 86_400_000)
}
// A membership/credential inside this many days of expiry (or already lapsed) surfaces a pay-to-extend row.
const RENEW_WINDOW_DAYS = 120
interface CatalogueCert {
  id: number
  code: string
  name: string
}

/** In-portal purchasing: membership and exam fees are bought right here — Stripe's secure page
 * handles the card, then returns to this page where the webhook-applied purchase shows up. */
function PlansCard() {
  const { me, refetch } = useMe()
  const [params] = useSearchParams()
  const [certSel, setCertSel] = useState('')
  const [code, setCode] = useState('')
  const [busy, setBusy] = useState<string | null>(null)
  const [err, setErr] = useState<string | null>(null)

  const pricingPath = useMemo(
    () => '/api/pricing' + (certSel ? `?cert=${encodeURIComponent(certSel)}` : ''),
    [certSel],
  )
  const { data: pricing } = useQuery<PricingResp>(pricingPath)
  const { data: certData } = useQuery<{ rows: CatalogueCert[] }>('/api/certifications')
  const certs = certData?.rows
  // Default the dropdown to the first certification once the catalogue loads, so the visible
  // selection always matches what "Pay exam fee" actually buys (an empty value silently entitled
  // the backend's default cert while the browser showed the first option highlighted).
  useEffect(() => {
    if (!certSel && certs && certs.length > 0) setCertSel(certs[0].code)
  }, [certs, certSel])

  if (!me) return null
  const memberActive = me.lifecycle.membership_status === 'active'
  const paid = params.get('paid')
  const cancelled = params.get('cancelled')

  // Renewal (membership) and recertification (per credential) surface a pay-to-extend row only inside
  // the renewal window or once lapsed — a freshly-activated member/credential sees neither.
  const membershipExpiry = (me.membership as Record<string, unknown> | null)?.expiry_date as string | undefined
  const renewDays = daysUntil(membershipExpiry)
  const renewalDue = me.membership != null && renewDays != null && renewDays <= RENEW_WINDOW_DAYS
  const seenCert = new Set<string>()
  const recertsDue = (me.exams ?? [])
    .map((x) => ({ code: x.certification_code ?? undefined, name: x.certification_name ?? undefined, exp: x.credential?.expires_at, days: daysUntil(x.credential?.expires_at), hasCred: !!x.credential }))
    .filter((r) => r.hasCred && r.days != null && r.days <= RENEW_WINDOW_DAYS)
    .filter((r) => { const k = r.code ?? ''; if (seenCert.has(k)) return false; seenCert.add(k); return true })

  // `busyKey` disambiguates concurrent buttons: exam/membership/renewal are unique, but a member may
  // hold several credentials, so each recert button keys on `recert:<cert code>`.
  async function buy(
    product: 'membership' | 'exam' | 'renewal' | 'recert',
    opts: { cert?: string; busyKey?: string } = {},
  ) {
    const busyKey = opts.busyKey ?? product
    setErr(null)
    setBusy(busyKey)
    try {
      // Validate a discount/founding code BEFORE opening Stripe, for THIS product — so an invalid or
      // wrong-product code is caught here instead of silently charging full price at checkout.
      const c = code.trim()
      if (c) {
        const v = await api.post<{ valid: boolean; message?: string }>('/api/validate-code', { code: c, product, email: me!.user.email })
        if (!v.valid) {
          setErr(v.message || 'That code is not valid for this purchase.')
          setBusy(null)
          return
        }
      }
      await startCheckout({
        product,
        email: me!.user.email,
        cert: product === 'exam' ? certSel || undefined : product === 'recert' ? opts.cert : undefined,
        code: c || undefined,
        first: me!.user.first_name ?? undefined,
        last: me!.user.last_name ?? undefined,
      })
    } catch (e2) {
      setErr(checkoutErrorMessage(e2))
      setBusy(null)
    }
  }

  return (
    <Card title="Membership & exam fees">
      {paid && (
        <div className="notice" style={{ marginBottom: '.75rem' }}>
          <strong>Payment received — thank you.</strong> Your purchase is being applied to your
          account and will appear below within a minute.{' '}
          <button className="btn ghost sm" onClick={() => refetch()}>Refresh</button>
        </div>
      )}
      {cancelled && (
        <div className="notice warn" style={{ marginBottom: '.75rem' }}>
          Payment was cancelled — nothing was charged. You can try again any time.
        </div>
      )}
      {err && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{err}</div>}

      <div className="plan-row">
        <div>
          <strong>Student membership</strong>
          <div className="muted small">3-year membership — the first step of the certification journey.</div>
        </div>
        <div className="row">
          {pricing && !memberActive && (
            <span className="plan-price">{fmtMoney(pricing.membership.final, pricing.currency)}</span>
          )}
          {memberActive ? (
            <Badge tone="ok">Active</Badge>
          ) : (
            <button className="btn sm" disabled={busy !== null} onClick={() => buy('membership')}>
              {busy === 'membership' ? 'Opening checkout…' : 'Activate membership'}
            </button>
          )}
        </div>
      </div>

      <div className="plan-row">
        <div style={{ flex: 1, minWidth: 220 }}>
          <strong>Certification exam fee</strong>
          <div className="muted small" style={{ marginBottom: '.4rem' }}>
            Buy an exam entitlement — schedule your sitting from the Certifications page afterwards.
          </div>
          <select value={certSel} onChange={(ev) => setCertSel(ev.target.value)} aria-label="Certification">
            {(certs ?? []).map((c) => (
              <option key={c.id} value={c.code}>{c.code} — {c.name}</option>
            ))}
            {(certs ?? []).length === 0 && <option value="">PCP-AI</option>}
          </select>
        </div>
        <div className="row">
          {pricing && <span className="plan-price">{fmtMoney(pricing.exam.final, pricing.currency)}</span>}
          <button className="btn sm" disabled={busy !== null} onClick={() => buy('exam')}>
            {busy === 'exam' ? 'Opening checkout…' : 'Pay exam fee'}
          </button>
        </div>
      </div>

      {renewalDue && pricing && (
        <div className="plan-row">
          <div style={{ flex: 1, minWidth: 220 }}>
            <strong>Renew membership</strong>
            <div className="muted small">
              Your 3-year membership {renewDays != null && renewDays < 0 ? 'lapsed on' : 'expires'} {membershipExpiry ? fmtDate(membershipExpiry) : ''}. Renewing extends it another 3-year term.
            </div>
          </div>
          <div className="row">
            <span className="plan-price">{fmtMoney(pricing.renewal.final, pricing.currency)}</span>
            <button className="btn sm" disabled={busy !== null} onClick={() => buy('renewal')}>
              {busy === 'renewal' ? 'Opening checkout…' : 'Renew membership'}
            </button>
          </div>
        </div>
      )}

      {pricing && recertsDue.map((r) => {
        const key = `recert:${r.code ?? ''}`
        return (
          <div className="plan-row" key={key}>
            <div style={{ flex: 1, minWidth: 220 }}>
              <strong>Recertify {r.code}</strong>
              <div className="muted small">
                Your {r.name || r.code} credential {r.days != null && r.days < 0 ? 'expired on' : 'expires'} {r.exp ? fmtDate(r.exp) : ''}. Recertify for another 3-year cycle.
              </div>
            </div>
            <div className="row">
              <span className="plan-price">{fmtMoney(pricing.recert.final, pricing.currency)}</span>
              <button className="btn sm" disabled={busy !== null} onClick={() => buy('recert', { cert: r.code, busyKey: key })}>
                {busy === key ? 'Opening checkout…' : 'Recertify'}
              </button>
            </div>
          </div>
        )
      })}

      <div className="row" style={{ marginTop: '.75rem', flexWrap: 'wrap' }}>
        <input
          style={{ maxWidth: 220 }}
          placeholder="Discount or founding code (optional)"
          value={code}
          onChange={(ev) => setCode(ev.target.value)}
          aria-label="Discount or founding code"
        />
        <span className="muted small">A discount code may apply to <strong>membership only</strong>, <strong>the exam fee only</strong>, or <strong>both</strong> — depending on the code. It is checked against the purchase you start, so a code that doesn&rsquo;t apply here is caught before payment instead of silently charging you full price. Founding codes are redeemed in the founding card below.</span>
      </div>
    </Card>
  )
}

export default function Billing() {
  const { me, loading, error } = useMe()
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  const receipt = (p: Payment) => {
    const name = `${me.user.first_name ?? ''} ${me.user.last_name ?? ''}`.trim() || me.user.email
    openPrintable(`Receipt ${p.reference ?? p.id}`, `
      <div class="brand">Project Controls Institute Global, Inc.</div>
      <h1>Payment receipt</h1>
      <p class="muted">Reference ${e(p.reference ?? p.id)}</p>
      <table>
        <tr><td>Billed to</td><td class="r">${e(name)} &lt;${e(me.user.email)}&gt;</td></tr>
        <tr><td>Item</td><td class="r">${e(titleCase(p.product_type))}</td></tr>
        <tr><td>Date</td><td class="r">${e(fmtDate(p.payment_date))}</td></tr>
        <tr><td>Status</td><td class="r">${e(p.payment_status)}</td></tr>
        <tr><td><strong>Amount paid</strong></td><td class="r"><strong>${e(fmtMoney(p.final_amount, p.currency))}</strong></td></tr>
      </table>`)
  }

  const paid = me.payments.filter((p) => p.payment_status === 'paid')
  const total = paid.reduce((s, p) => s + (Number(p.final_amount) || 0), 0)
  const currency = paid[0]?.currency || 'USD'

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Billing</h1>
        <p className="muted">Buy membership and exam entitlements, and download receipts — all in one place.</p>
      </div>

      <PlansCard />

      <FoundingCard />

      <Card title="Payment history" action={<span className="muted small">Total paid: <strong>{fmtMoney(total, currency)}</strong></span>}>
        {me.payments.length === 0 ? (
          <Empty>No payments on record.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Date</th><th>Item</th><th>Reference</th><th>Amount</th><th>Status</th><th></th></tr>
            </thead>
            <tbody>
              {me.payments.map((p) => (
                <tr key={p.id}>
                  <td>{fmtDate(p.payment_date)}</td>
                  <td>{titleCase(p.product_type)}</td>
                  <td className="small muted">{p.reference || '—'}</td>
                  <td>{fmtMoney(p.final_amount, p.currency)}</td>
                  <td><StatusBadge status={p.payment_status} /></td>
                  <td>
                    {p.payment_status === 'paid' && (
                      <button className="btn ghost sm" onClick={() => receipt(p)}>Receipt</button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}
