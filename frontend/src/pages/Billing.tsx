import { useEffect, useMemo, useState } from 'react'
import { useSearchParams, Link } from 'react-router-dom'
import { useMe } from '../data/MeContext'
import { useQuery } from '../api/hooks'
import { api } from '../api/client'
import { startCheckout, checkoutErrorMessage } from '../api/checkout'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../components/ui'
import FoundingCard from '../components/FoundingCard'
import { fmtDate, fmtMoney, titleCase } from '../format'
import { openPrintable, escapeHtml as e } from '../print'
import { useT } from '../i18n'
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
  const t = useT()
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
    if (!certSel && certs && certs.length > 0) {
      // Honour a deep-linked ?cert=CODE (from the public "Enrol in <CERT>" CTA, carried through
      // register/login) so the buyer pays for the certification they clicked — not the first one.
      const wanted = params.get('cert')
      const match = wanted ? certs.find((c) => c.code.toLowerCase() === wanted.toLowerCase()) : undefined
      setCertSel((match ?? certs[0]).code)
    }
  }, [certs, certSel, params])

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
    .map((x) => ({ code: x.certification_code ?? undefined, name: x.certification_name ?? undefined, exp: x.credential?.expires_at, days: daysUntil(x.credential?.expires_at), hasCred: !!x.credential, cpd: x.recert_cpd ?? null }))
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
          setErr(v.message || t('billing.codeInvalid'))
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
    <Card title={t('billing.plansTitle')}>
      {paid && (
        <div className="notice" style={{ marginBottom: '.75rem' }}>
          <strong>{t('billing.paymentReceivedThanks')}</strong> {t('billing.purchaseApplying')}{' '}
          <button className="btn ghost sm" onClick={() => refetch()}>{t('billing.refresh')}</button>
        </div>
      )}
      {cancelled && (
        <div className="notice warn" style={{ marginBottom: '.75rem' }}>
          {t('billing.paymentCancelled')}
        </div>
      )}
      {err && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{err}</div>}

      <div className="plan-row">
        <div>
          <strong>{t('billing.studentMembership')}</strong>
          <div className="muted small">{t('billing.studentMembershipDesc')}</div>
        </div>
        <div className="row">
          {pricing && !memberActive && (
            <span className="plan-price">{fmtMoney(pricing.membership.final, pricing.currency)}</span>
          )}
          {memberActive ? (
            <Badge tone="ok">{t('billing.active')}</Badge>
          ) : (
            <button className="btn sm" disabled={busy !== null} onClick={() => buy('membership')}>
              {busy === 'membership' ? t('billing.openingCheckout') : t('billing.activateMembership')}
            </button>
          )}
        </div>
      </div>

      <div className="plan-row">
        <div style={{ flex: 1, minWidth: 220 }}>
          <strong>{t('billing.examFeeTitle')}</strong>
          <div className="muted small" style={{ marginBottom: '.4rem' }}>
            {t('billing.examFeeDesc')}
          </div>
          <select value={certSel} onChange={(ev) => setCertSel(ev.target.value)} aria-label={t('billing.certificationAria')}>
            {(certs ?? []).map((c) => (
              <option key={c.id} value={c.code}>{c.code} — {c.name}</option>
            ))}
            {(certs ?? []).length === 0 && <option value="">PCL-AI</option>}
          </select>
        </div>
        <div className="row">
          {pricing && <span className="plan-price">{fmtMoney(pricing.exam.final, pricing.currency)}</span>}
          <button className="btn sm" disabled={busy !== null} onClick={() => buy('exam')}>
            {busy === 'exam' ? t('billing.openingCheckout') : t('billing.payExamFee')}
          </button>
        </div>
      </div>

      {renewalDue && pricing && (
        <div className="plan-row">
          <div style={{ flex: 1, minWidth: 220 }}>
            <strong>{t('billing.renewMembership')}</strong>
            <div className="muted small">
              {renewDays != null && renewDays < 0
                ? t('billing.renewDescLapsed', { date: membershipExpiry ? fmtDate(membershipExpiry) : '' })
                : t('billing.renewDescExpires', { date: membershipExpiry ? fmtDate(membershipExpiry) : '' })}
            </div>
          </div>
          <div className="row">
            <span className="plan-price">{fmtMoney(pricing.renewal.final, pricing.currency)}</span>
            <button className="btn sm" disabled={busy !== null} onClick={() => buy('renewal')}>
              {busy === 'renewal' ? t('billing.openingCheckout') : t('billing.renewMembership')}
            </button>
          </div>
        </div>
      )}

      {pricing && recertsDue.map((r) => {
        const key = `recert:${r.code ?? ''}`
        // A CPD requirement that isn't yet met blocks recertification (the checkout would refuse it too).
        const cpdBlocked = !!r.cpd && !r.cpd.met
        return (
          <div className="plan-row" key={key}>
            <div style={{ flex: 1, minWidth: 220 }}>
              <strong>{t('billing.recertifyTitle', { code: r.code ?? '' })}</strong>
              <div className="muted small">
                {r.days != null && r.days < 0
                  ? t('billing.recertDescExpired', { name: r.name || r.code || '', date: r.exp ? fmtDate(r.exp) : '' })
                  : t('billing.recertDescExpires', { name: r.name || r.code || '', date: r.exp ? fmtDate(r.exp) : '' })}
              </div>
              {r.cpd && (
                <div className="small" style={{ marginTop: '.25rem', color: cpdBlocked ? 'var(--err, #c2410c)' : 'var(--ok, #15803d)' }}>
                  {cpdBlocked
                    ? `CPD ${r.cpd.approved}/${r.cpd.required} approved hours — complete your CPD to recertify.`
                    : `CPD requirement met (${r.cpd.approved}/${r.cpd.required} approved hours).`}
                  {!!r.cpd.ai_required && r.cpd.ai_required > 0 && (
                    <> {' '}(incl. AI-currency {r.cpd.ai_approved ?? 0}/{r.cpd.ai_required}{r.cpd.ai_met ? ' ✓' : ''})</>
                  )}
                  {cpdBlocked && <> <Link to="/cpd">Log CPD</Link></>}
                </div>
              )}
            </div>
            <div className="row">
              <span className="plan-price">{fmtMoney(pricing.recert.final, pricing.currency)}</span>
              <button className="btn sm" disabled={busy !== null || cpdBlocked} title={cpdBlocked ? 'Complete your CPD requirement before recertifying.' : undefined} onClick={() => buy('recert', { cert: r.code, busyKey: key })}>
                {busy === key ? t('billing.openingCheckout') : t('billing.recertifyBtn')}
              </button>
            </div>
          </div>
        )
      })}

      <div className="row" style={{ marginTop: '.75rem', flexWrap: 'wrap' }}>
        <input
          style={{ maxWidth: 220 }}
          placeholder={t('billing.discountCodePlaceholder')}
          value={code}
          onChange={(ev) => setCode(ev.target.value)}
          aria-label={t('billing.discountCodeAria')}
        />
        <span className="muted small">{t('billing.discountApplyIntro')}<strong>{t('billing.discountScopeMembership')}</strong>{t('billing.discountSep1')}<strong>{t('billing.discountScopeExam')}</strong>{t('billing.discountSep2')}<strong>{t('billing.discountScopeBoth')}</strong>{t('billing.discountApplyOutro')}</span>
      </div>
    </Card>
  )
}

export default function Billing() {
  const t = useT()
  const { me, loading, error } = useMe()
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  const receipt = (p: Payment) => {
    const name = `${me.user.first_name ?? ''} ${me.user.last_name ?? ''}`.trim() || me.user.email
    openPrintable(t('billing.receiptDocTitle', { ref: p.reference ?? p.id }), `
      <div class="brand">Project Controls Institute Global, Inc.</div>
      <h1>${t('billing.receiptHeading')}</h1>
      <p class="muted">${t('billing.receiptReferenceLabel')} ${e(p.reference ?? p.id)}</p>
      <table>
        <tr><td>${t('billing.receiptBilledTo')}</td><td class="r">${e(name)} &lt;${e(me.user.email)}&gt;</td></tr>
        <tr><td>${t('billing.receiptItem')}</td><td class="r">${e(titleCase(p.product_type))}</td></tr>
        <tr><td>${t('billing.receiptDate')}</td><td class="r">${e(fmtDate(p.payment_date))}</td></tr>
        <tr><td>${t('billing.receiptStatus')}</td><td class="r">${e(p.payment_status)}</td></tr>
        <tr><td><strong>${t('billing.receiptAmountPaid')}</strong></td><td class="r"><strong>${e(fmtMoney(p.final_amount, p.currency))}</strong></td></tr>
      </table>`)
  }

  const paid = me.payments.filter((p) => p.payment_status === 'paid')
  const total = paid.reduce((s, p) => s + (Number(p.final_amount) || 0), 0)
  const currency = paid[0]?.currency || 'USD'

  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>{t('billing.title')}</h1>
        <p className="muted">{t('billing.subtitle')}</p>
      </div>

      <PlansCard />

      <FoundingCard />

      <Card title={t('billing.paymentHistory')} action={<span className="muted small">{t('billing.totalPaid')} <strong>{fmtMoney(total, currency)}</strong></span>}>
        {me.payments.length === 0 ? (
          <Empty>{t('billing.noPayments')}</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>{t('billing.colDate')}</th><th>{t('billing.colItem')}</th><th>{t('billing.colReference')}</th><th>{t('billing.colAmount')}</th><th>{t('billing.colStatus')}</th><th></th></tr>
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
                      <button className="btn ghost sm" onClick={() => receipt(p)}>{t('billing.receipt')}</button>
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
