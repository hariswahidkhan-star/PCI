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
import type { Payment, Me } from '../api/types'

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
    product: 'membership' | 'exam' | 'bundle' | 'renewal' | 'recert',
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
        cert: product === 'exam' || product === 'bundle' ? certSel || undefined : product === 'recert' ? opts.cert : undefined,
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
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      {me.membership_grade && <MembershipGradeCard grade={me.membership_grade} onDone={refetch} />}
      {me.membership_dues?.available && <DuesCard dues={me.membership_dues} />}
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

      {!memberActive && pricing && (
        <div className="plan-row" style={{ background: 'var(--tint, #eef2ff)', borderRadius: 10 }}>
          <div style={{ flex: 1, minWidth: 220 }}>
            <strong>{t('billing.bundleTitle')}</strong>
            <div className="muted small">{t('billing.bundleDesc', { cert: certSel || (certs?.[0]?.code ?? '') })}</div>
          </div>
          <div className="row">
            <span className="plan-price">{fmtMoney(pricing.bundle.final, pricing.currency)}</span>
            <button className="btn sm" disabled={busy !== null} onClick={() => buy('bundle')}>
              {busy === 'bundle' ? t('billing.openingCheckout') : t('billing.payBundle')}
            </button>
          </div>
        </div>
      )}

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
    </div>
  )
}

// Membership grade & progression (Student → Associate → Professional (MPCI) → Fellow (FPCI)).
function MembershipGradeCard({ grade, onDone }: { grade: NonNullable<Me['membership_grade']>; onDone: () => void }) {
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState<{ ok: boolean; text: string } | null>(null)
  const [fellowOpen, setFellowOpen] = useState(false)
  const [statement, setStatement] = useState('')

  async function upgrade(to: string, body?: Record<string, unknown>) {
    setBusy(true); setNote(null)
    try {
      const r = await api.post<{ pending?: boolean; message?: string }>('/api/me/membership/upgrade', { to_grade: to, ...body })
      setNote({ ok: true, text: r.message || 'Your membership grade has been updated.' })
      setFellowOpen(false); setStatement(''); onDone()
    } catch (e) {
      const b = e instanceof Object && 'body' in e ? (e as { body?: { message?: string } }).body : null
      setNote({ ok: false, text: b?.message || (e instanceof Error ? e.message : 'Could not update your grade.') })
    } finally { setBusy(false) }
  }

  const pending = grade.pending_application
  const up = grade.eligible_upgrade

  return (
    <Card title="Membership grade" action={<Badge tone="brand">{grade.post_nominal}</Badge>}>
      <div className="stack small" style={{ gap: '.4rem' }}>
        <div>You are a <strong>{grade.label}</strong> — you may use the post-nominal <strong>{grade.post_nominal}</strong>.</div>
        {note && <div className={note.ok ? 'muted' : ''} style={note.ok ? undefined : { color: 'var(--err, #c2410c)' }} role="status">{note.text}</div>}

        {pending ? (
          <div className="muted">Your application to become a {pending.to_grade === 'fellow' ? 'Fellow (FPCI)' : pending.to_grade} is under review.</div>
        ) : (
          <div className="row" style={{ flexWrap: 'wrap', gap: '.4rem', marginTop: '.2rem' }}>
            {up && (
              <button className="btn sm" disabled={busy} onClick={() => upgrade(up.key)}>
                {busy ? 'Working…' : `Upgrade to ${up.label} (${up.post_nominal})`}
              </button>
            )}
            {grade.can_apply_fellow && !fellowOpen && (
              <button className="btn sm secondary" onClick={() => setFellowOpen(true)}>Apply for Fellowship (FPCI)</button>
            )}
          </div>
        )}

        {fellowOpen && !pending && (
          <div className="stack" style={{ display: 'grid', gap: '.4rem', marginTop: '.3rem' }}>
            <p className="muted small" style={{ margin: 0 }}>Fellowship is by nomination — describe your sustained contribution to the profession and the institute (min 40 characters).</p>
            <textarea rows={4} maxLength={4000} value={statement} onChange={(e) => setStatement(e.target.value)} placeholder="Leadership, mentoring, standards work, speaking, publications…" />
            <div className="row" style={{ gap: '.4rem' }}>
              <button className="btn sm" disabled={busy || statement.trim().length < 40} onClick={() => upgrade('fellow', { statement: statement.trim() })}>{busy ? 'Submitting…' : 'Submit nomination'}</button>
              <button className="btn ghost sm" onClick={() => setFellowOpen(false)}>Cancel</button>
            </div>
          </div>
        )}
      </div>
    </Card>
  )
}

// Recurring annual-dues subscription (Stripe). Only rendered when the operator has enabled the feature.
function DuesCard({ dues }: { dues: NonNullable<Me['membership_dues']> }) {
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  async function go(path: 'subscribe' | 'portal') {
    setBusy(true); setErr(null)
    try {
      const r = await api.post<{ url?: string }>(`/api/me/membership/${path}`, {})
      if (r.url) window.location.href = r.url
      else setErr('Could not open the billing page. Please try again.')
    } catch (e) {
      const b = e instanceof Object && 'body' in e ? (e as { body?: { message?: string } }).body : null
      setErr(b?.message || (e instanceof Error ? e.message : 'Something went wrong.')); setBusy(false)
    }
  }

  return (
    <Card title="Annual membership (auto-renew)">
      {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
      {dues.subscribed ? (
        <div className="stack small" style={{ gap: '.4rem' }}>
          <div>Your membership renews automatically each year{dues.status && dues.status !== 'active' ? ` (status: ${dues.status})` : ''}.</div>
          {dues.cancel_at_period_end && <div className="muted">It is set to end at the close of the current period — you can resume from the billing portal.</div>}
          <div><button className="btn sm secondary" disabled={busy} onClick={() => go('portal')}>{busy ? 'Opening…' : 'Manage subscription'}</button></div>
        </div>
      ) : (
        <div className="stack small" style={{ gap: '.4rem' }}>
          <div>Set up an auto-renewing annual membership so your standing never lapses — cancel any time.</div>
          <div><button className="btn sm" disabled={busy} onClick={() => go('subscribe')}>{busy ? 'Opening…' : 'Set up auto-renew'}</button></div>
        </div>
      )}
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
