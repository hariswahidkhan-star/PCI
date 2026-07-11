import { useMe } from '../data/MeContext'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../components/ui'
import { fmtDate, isPast, daysUntil } from '../format'
import { openPrintable, escapeHtml as e } from '../print'
import type { Credential } from '../api/types'

export default function Credentials() {
  const { me, loading, error } = useMe()
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!me) return null

  const certificate = (c: Credential) => {
    const holder = c.holder_name || `${me.user.first_name ?? ''} ${me.user.last_name ?? ''}`.trim()
    openPrintable(`Certificate ${c.credential_id}`, `
      <div class="brand" style="text-align:center">PROJECT CONTROLS INSTITUTE</div>
      <div class="big">Certificate of Certification</div>
      <p style="text-align:center" class="muted">This certifies that</p>
      <h1 style="text-align:center;font-size:1.8rem">${e(holder)}</h1>
      <p style="text-align:center">has been awarded the credential</p>
      <h2 style="text-align:center;color:#1d4ed8">${e(c.credential || c.credential_id)}</h2>
      <table>
        <tr><td>Credential ID</td><td class="r">${e(c.credential_id)}</td></tr>
        <tr><td>Issued</td><td class="r">${e(fmtDate(c.issued_at))}</td></tr>
        <tr><td>Valid until</td><td class="r">${e(fmtDate(c.expires_at))}</td></tr>
      </table>
      <p class="muted" style="text-align:center;margin-top:1.5rem">Verify this credential at ${e((me.site_base_url || location.origin))}/verify.html?id=${e(c.credential_id)}</p>`)
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Credentials</h1>
        <p className="muted">Your issued certifications and their validity.</p>
      </div>

      {/* Honorary recognition is deliberately separate from exam-earned credentials: it is a
          board-conferred designation, not the PCP-AI examination credential. */}
      {me.honorary.length > 0 && (
        <Card title="Honorary recognition" action={<Badge tone="brand">Board-conferred</Badge>}>
          {me.honorary.map((h) => (
            <div key={h.award_no} className="stack small" style={{ gap: '.35rem' }}>
              <div><strong>{h.designation || 'Honorary Fellow (PCI)'}</strong> · award no. <strong>{h.award_no}</strong></div>
              {h.citation && <div className="muted">“{h.citation}”</div>}
              <div className="muted">
                Conferred {fmtDate(h.conferred_at)} — an honorary recognition of distinguished contribution,
                distinct from the examined PCP-AI credential.
              </div>
              <div className="row" style={{ marginTop: '.4rem' }}>
                <a className="btn sm secondary" href={`/verify.html?id=${encodeURIComponent(h.award_no)}`} target="_blank" rel="noreferrer">Public verify page</a>
              </div>
            </div>
          ))}
        </Card>
      )}

      {me.credentials.length === 0 ? (
        <Card><Empty>You have no issued credentials yet. They appear here once you pass an exam.</Empty></Card>
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
                <div><span className="muted">Credential ID</span><div><strong>{c.credential_id}</strong></div></div>
                <div><span className="muted">Holder</span><div>{c.holder_name || `${me.user.first_name} ${me.user.last_name}`}</div></div>
                <div><span className="muted">Issued</span><div>{fmtDate(c.issued_at)}</div></div>
                <div><span className="muted">Expires</span><div>{fmtDate(c.expires_at)}{dleft !== null && dleft >= 0 && dleft < 90 && <> <Badge tone="warn">{dleft}d left</Badge></>}</div></div>
              </div>
              {c.status === 'active' && !lapsed && (
                <div className="row" style={{ marginTop: '.9rem' }}>
                  <button className="btn sm" onClick={() => certificate(c)}>Download certificate</button>
                  <a className="btn sm secondary" href={`/verify.html?id=${encodeURIComponent(c.credential_id)}`} target="_blank" rel="noreferrer">Public verify page</a>
                </div>
              )}
            </Card>
          )
        })
      )}
    </div>
  )
}
