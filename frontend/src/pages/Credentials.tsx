import { useMe } from '../data/MeContext'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../components/ui'
import { fmtDate, isPast, daysUntil } from '../format'
import { openPrintable, escapeHtml as e } from '../print'
import { useT } from '../i18n'
import type { Credential } from '../api/types'

export default function Credentials() {
  const t = useT()
  const { me, loading, error } = useMe()
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  const certificate = (c: Credential) => {
    const holder = c.holder_name || `${me.user.first_name ?? ''} ${me.user.last_name ?? ''}`.trim()
    const verifyUrl = `${e(me.site_base_url || location.origin)}/verify.html?id=${e(c.credential_id)}`
    openPrintable(t('cred.certificateTitle', { id: c.credential_id }), `
      <div class="brand" style="text-align:center">Project Controls Institute Global, Inc.</div>
      <div class="big">${t('cred.certOfCertification')}</div>
      <p style="text-align:center" class="muted">${t('cred.thisCertifies')}</p>
      <h1 style="text-align:center;font-size:1.8rem">${e(holder)}</h1>
      <p style="text-align:center">${t('cred.hasBeenAwarded')}</p>
      <h2 style="text-align:center;color:#1d4ed8">${e(c.certification_name || c.credential || c.credential_id)}</h2>
      ${c.certification_acronym ? `<p style="text-align:center;margin-top:-.4rem" class="muted">${e(c.certification_acronym)}</p>` : ''}
      ${c.certificate_wording ? `<p style="text-align:center;font-style:italic;max-width:34rem;margin:1rem auto 0">${e(c.certificate_wording)}</p>` : ''}
      <table>
        <tr><td>${t('cred.credentialId')}</td><td class="r">${e(c.credential_id)}</td></tr>
        <tr><td>${t('cred.issued')}</td><td class="r">${e(fmtDate(c.issued_at))}</td></tr>
        <tr><td>${t('cred.validUntil')}</td><td class="r">${e(fmtDate(c.expires_at))}</td></tr>
      </table>
      <p class="muted" style="text-align:center;margin-top:1.5rem">${t('cred.verifyAt', { url: verifyUrl })}</p>`)
  }

  // LinkedIn "Add to Profile" — LinkedIn's official certification deep-link (no API, no scraping, no OAuth).
  // Pre-fills the holder's Licenses & Certifications entry with the credential name, PCI as the issuing
  // organisation, the issue/expiry dates, the credential id and the public verification URL. Every credential
  // holder who adds it becomes organic reach for the certification.
  const ORG_NAME = 'Project Controls Institute'
  const addToLinkedIn = (c: Credential) => {
    const name = c.certification_name || c.credential || c.credential_id
    const verifyUrl = `${me.site_base_url || location.origin}/verify.html?id=${encodeURIComponent(c.credential_id)}`
    const ym = (d?: string | null) => {
      if (!d) return {}
      const dt = new Date(d)
      return isNaN(dt.getTime()) ? {} : { year: String(dt.getUTCFullYear()), month: String(dt.getUTCMonth() + 1) }
    }
    const iss = ym(c.issued_at), exp = ym(c.expires_at)
    const p = new URLSearchParams({ startTask: 'CERTIFICATION_NAME', name, organizationName: ORG_NAME, certUrl: verifyUrl, certId: c.credential_id })
    if (iss.year) { p.set('issueYear', iss.year); p.set('issueMonth', iss.month!) }
    if (exp.year) { p.set('expirationYear', exp.year); p.set('expirationMonth', exp.month!) }
    window.open('https://www.linkedin.com/profile/add?' + p.toString(), '_blank', 'noopener,noreferrer')
  }

  // Fetch the official, QR-bearing PDF certificate (authenticated) and save it. This is the real
  // downloadable document; the printable above remains as a quick on-screen view.
  const downloadPdf = async (id: string, honorary = false) => {
    const tok = sessionStorage.getItem('pci.session.token')
    const url = honorary ? '/api/me/honorary-certificate/pdf' : `/api/me/certificate/pdf?id=${encodeURIComponent(id)}`
    try {
      const r = await fetch(url, { headers: tok ? { Authorization: 'Bearer ' + tok } : {} })
      if (!r.ok) { alert('Could not download the certificate right now. Please try again shortly or contact support.'); return }
      const blob = await r.blob()
      const a = document.createElement('a')
      a.href = URL.createObjectURL(blob)
      a.download = `${honorary ? 'honorary-certificate' : 'certificate'}-${id}.pdf`
      document.body.appendChild(a); a.click(); a.remove()
      setTimeout(() => URL.revokeObjectURL(a.href), 60_000)
    } catch { alert('Could not download the certificate right now. Please try again shortly or contact support.') }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>{t('cred.title')}</h1>
        <p className="muted">{t('cred.subtitle')}</p>
      </div>

      {/* Honorary recognition is deliberately separate from exam-earned credentials: it is a
          board-conferred designation, not an examined certification credential. */}
      {me.honorary.length > 0 && (
        <Card title={t('cred.honoraryTitle')} action={<Badge tone="brand">{t('cred.boardConferred')}</Badge>}>
          {me.honorary.map((h) => (
            <div key={h.award_no} className="stack small" style={{ gap: '.35rem' }}>
              <div><strong>{h.designation || t('cred.honoraryDefault')}</strong> · {t('cred.awardNo')} <strong>{h.award_no}</strong></div>
              {h.citation && <div className="muted">“{h.citation}”</div>}
              <div className="muted">
                {t('cred.honoraryConferred', { date: fmtDate(h.conferred_at) })}
              </div>
              <div className="row" style={{ marginTop: '.4rem', flexWrap: 'wrap' }}>
                <button className="btn sm" onClick={() => downloadPdf(h.award_no, true)}>Download PDF</button>
                <a className="btn sm secondary" href={`/verify.html?id=${encodeURIComponent(h.award_no)}`} target="_blank" rel="noreferrer">{t('cred.publicVerifyPage')}</a>
              </div>
            </div>
          ))}
        </Card>
      )}

      {me.credentials.length === 0 ? (
        <Card><Empty>{t('cred.emptyCredentials')}</Empty></Card>
      ) : (
        me.credentials.map((c) => {
          const lapsed = c.status === 'active' && isPast(c.expires_at)
          const dleft = daysUntil(c.expires_at)
          return (
            <Card
              key={c.credential_id}
              title={c.credential || c.credential_id}
              action={<StatusBadge status={lapsed ? 'expired' : c.status} />}
            >
              <div className="grid cols-2 small">
                <div><span className="muted">{t('cred.credentialId')}</span><div><strong>{c.credential_id}</strong></div></div>
                <div><span className="muted">{t('cred.holder')}</span><div>{c.holder_name || `${me.user.first_name} ${me.user.last_name}`}</div></div>
                <div><span className="muted">{t('cred.issued')}</span><div>{fmtDate(c.issued_at)}</div></div>
                <div><span className="muted">{t('cred.expires')}</span><div>{fmtDate(c.expires_at)}{dleft !== null && dleft >= 0 && dleft < 90 && <> <Badge tone="warn">{t('cred.daysLeftShort', { n: dleft })}</Badge></>}</div></div>
              </div>
              {c.certificate_wording && (
                <p className="muted small" style={{ margin: '.6rem 0 0', fontStyle: 'italic' }}>{c.certificate_wording}</p>
              )}
              {c.status === 'active' && !lapsed && (
                <div className="row" style={{ marginTop: '.9rem', flexWrap: 'wrap' }}>
                  <button className="btn sm" onClick={() => downloadPdf(c.credential_id)}>Download PDF</button>
                  <button className="btn sm secondary" onClick={() => certificate(c)}>{t('cred.downloadCertificate')}</button>
                  <a className="btn sm secondary" href={`/verify.html?id=${encodeURIComponent(c.credential_id)}`} target="_blank" rel="noreferrer">{t('cred.publicVerifyPage')}</a>
                  <a className="btn sm secondary" href={`/badge.html?id=${encodeURIComponent(c.credential_id)}`} target="_blank" rel="noreferrer" title="Your shareable, verifiable Open Badge">Digital badge</a>
                  <button className="btn sm secondary" onClick={() => addToLinkedIn(c)} title="Add this credential to your LinkedIn profile">Add to LinkedIn</button>
                </div>
              )}
            </Card>
          )
        })
      )}
    </div>
  )
}
