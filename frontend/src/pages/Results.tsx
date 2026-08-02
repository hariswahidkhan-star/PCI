import { useMemo } from 'react'
import { Link } from 'react-router-dom'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Badge, Spinner, StatusBadge } from '../components/ui'
import { PageHeader, EmptyState } from '../components/premium'
import { fmtDateTime } from '../format'
import { Dial, DomainBands, parseBreakdown } from '../components/results'
import type { Attempt, Me } from '../api/types'

/**
 * Results — the full examination result inside the React portal (the classic panel's Results
 * section, rebuilt). Latest result with the score dial and per-domain bands, the attempt history,
 * and the printable score report. Held and invalidated results follow the same disclosure rules
 * the server enforces: a held attempt shows no score or pass/fail; an invalidated one points at
 * the Appeals process.
 */

function esc(s: unknown): string {
  return String(s ?? '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;')
}

interface ReportExtra {
  registration_no?: string | null
  unanswered?: number | null
  flagged_events?: number | null
  released_at?: string | null
}

/** Build and save the printable score report (HTML), enriched from the report endpoint. */
async function downloadScore(a: Attempt, me: Me) {
  let rep: ReportExtra | null = null
  try { rep = await api.get<ReportExtra>(`/api/me/results/${a.id}/report`) } catch { /* base data still prints */ }
  const bd = parseBreakdown(a.domain_breakdown)
  const u = me.user
  const cred = me.credentials.find((c) => (c.certification_id ?? 1) === (a.certification_id ?? 1) && c.status === 'active')
  const verify = (id: string) => `${(me.site_base_url || window.location.origin).replace(/\/$/, '')}/verify.html?id=${encodeURIComponent(id)}`
  const rows = bd.map((b) =>
    `<tr><td style="padding:6px 10px;border-bottom:1px solid #eee">${esc(b.domain)}</td><td style="padding:6px 10px;border-bottom:1px solid #eee">${b.pct}%</td><td style="padding:6px 10px;border-bottom:1px solid #eee">${b.band === 'above' ? 'Above target' : b.band === 'at' ? 'At target' : 'Below target'}</td></tr>`).join('')
  const html = `<!doctype html><html><head><meta charset="utf-8"><title>PCI Score Report</title></head>
<body style="font-family:Georgia,serif;max-width:700px;margin:40px auto;color:#0B0F19">
<div style="text-align:center"><div style="font-size:30px;font-weight:700;color:#15347F">PCI Global</div>
<div style="letter-spacing:4px;font-size:10px;color:#98A0AF">PROJECT CONTROLS INSTITUTE</div>
<h2 style="letter-spacing:3px;color:#96690C;font-weight:600;margin-top:24px">OFFICIAL SCORE REPORT</h2></div>
<p><b>Candidate:</b> ${esc(`${u.first_name} ${u.last_name || ''}`.trim())} &nbsp; <b>Email:</b> ${esc(u.email)}</p>
<p><b>Examination:</b> ${esc(a.certification_name || a.certification_code || 'PCI examination')} &nbsp; <b>Date:</b> ${esc(fmtDateTime(a.submitted_at))} &nbsp; <b>Attempt:</b> #${a.id}</p>
<p><b>Overall:</b> ${a.percent}% — <b style="color:${a.result === 'pass' ? '#0E7C43' : '#B4380F'}">${a.result === 'pass' ? 'PASS' : 'NOT PASSED'}</b> (PCI pass mark applies)</p>
<p style="font-size:12.5px;color:#334155"><b>Registration:</b> ${esc(rep?.registration_no || u.registration_no || '—')}${rep?.unanswered != null ? ` &nbsp; <b>Unanswered:</b> ${rep.unanswered}` : ''}${rep?.flagged_events != null ? ` &nbsp; <b>Integrity events:</b> ${rep.flagged_events}` : ''}${rep?.released_at ? ` &nbsp; <b>Released:</b> ${esc(fmtDateTime(rep.released_at))}` : ''}</p>
<table style="width:100%;border-collapse:collapse;font-family:Arial;font-size:13px">
<tr><th align="left" style="padding:6px 10px;border-bottom:2px solid #ddd">Domain</th><th align="left" style="padding:6px 10px;border-bottom:2px solid #ddd">Score</th><th align="left" style="padding:6px 10px;border-bottom:2px solid #ddd">Band</th></tr>${rows}</table>
${cred ? `<p style="margin-top:18px"><b>Credential:</b> ${esc(cred.credential_id)} — verify: <a href="${verify(cred.credential_id)}">${verify(cred.credential_id)}</a></p>` : ''}
<p style="font-size:11px;color:#98A0AF;margin-top:26px">Bands report performance relative to the standard; item content is never disclosed.</p>
</body></html>`
  const blob = new Blob([html], { type: 'text/html' })
  const el = document.createElement('a')
  el.href = URL.createObjectURL(blob)
  el.download = `PCI-Score-Report-${a.id}.html`
  document.body.appendChild(el)
  el.click()
  el.remove()
  URL.revokeObjectURL(el.href)
}

function HistoryRow({ a, me }: { a: Attempt; me: Me }) {
  const held = a.result_status === 'auto_held'
  return (
    <div className="row" style={{ gap: '.6rem', alignItems: 'center', padding: '.45rem 0', borderBottom: '1px solid var(--line)' }}>
      {held ? <Badge tone="warn">Held</Badge> : <Badge tone={a.result === 'pass' ? 'ok' : 'err'}>{a.result === 'pass' ? 'Pass' : 'Not passed'}</Badge>}
      <div style={{ flex: 1, minWidth: 0 }}>
        <b className="small">{a.certification_code || 'EXAM'} · {held ? 'Under review' : `${a.percent}%`}</b>
        <div className="muted small">{fmtDateTime(a.submitted_at)}</div>
      </div>
      {!held && a.result && (
        <button className="btn ghost sm" onClick={() => { void downloadScore(a, me) }}>Report</button>
      )}
    </div>
  )
}

export default function Results() {
  const { me, loading } = useMe()

  const attempts = useMemo(
    () => (me?.attempts ?? []).filter((a) => a.kind === 'exam' && a.status === 'submitted'),
    [me],
  )

  if (loading || !me) return <Spinner />

  if (!attempts.length) {
    return (
      <div className="page">
        <PageHeader eyebrow="Results" title="Examination results" />
        <Card>
          <EmptyState
            icon="activity"
            title="No examination attempts yet"
            detail="Your results, domain breakdown and printable score report appear here after you sit the exam."
          />
          <div style={{ textAlign: 'center', marginTop: '.5rem' }}>
            <Link className="btn" to="/certifications">Go to Certifications</Link>
          </div>
        </Card>
      </div>
    )
  }

  const latest = attempts[0]
  const held = latest.result_status === 'auto_held'
  const invalidated = latest.result_status === 'invalidated' || latest.result_status === 'credential_revoked'
  const pass = latest.result === 'pass'
  const breakdown = parseBreakdown(latest.domain_breakdown)

  return (
    <div className="page fade-stagger">
      <PageHeader eyebrow="Results" title="Examination results" />

      <div className="grid cols-2" style={{ alignItems: 'start' }}>
        <Card
          title={`Latest ${latest.certification_code || ''} result`.trim()}
          action={<span className="muted small">{fmtDateTime(latest.submitted_at)}</span>}
        >
          {held ? (
            <div style={{ textAlign: 'center', padding: '1.6rem 1rem' }}>
              <div style={{ fontSize: '2rem' }}>⏳</div>
              <h3 style={{ color: 'var(--warn)', margin: '.5rem 0' }}>Result on hold</h3>
              <p className="muted" style={{ maxWidth: '46ch', margin: '0 auto' }}>
                Your result could not be published automatically because the submission did not pass
                a technical validity check. PCI will look into it and notify you. This is not a
                proctoring or manual-marking review.
              </p>
              <p className="muted small" style={{ marginTop: '.6rem' }}>
                No pass/fail outcome is shown until the technical issue is resolved.
              </p>
            </div>
          ) : invalidated ? (
            <div style={{ textAlign: 'center', padding: '1.6rem 1rem' }}>
              <div style={{ fontSize: '2rem' }}>✕</div>
              <h3 style={{ color: 'var(--err)', margin: '.5rem 0' }}>Result invalidated</h3>
              <p className="muted" style={{ maxWidth: '48ch', margin: '0 auto' }}>
                This attempt was invalidated following an examination integrity review
                {latest.result_status === 'credential_revoked' ? ' and the associated credential has been revoked' : ''}.
                You may appeal this decision.
              </p>
              <Link className="btn" to="/appeals" style={{ marginTop: '1rem' }}>Appeal this decision</Link>
            </div>
          ) : (
            <div style={{ textAlign: 'center' }}>
              <Dial pct={latest.percent ?? 0} />
              <h2 style={{ color: pass ? 'var(--ok)' : 'var(--err)', margin: '.7rem 0 .1rem' }}>
                {pass ? 'PASS' : 'Not passed'}
              </h2>
              <p className="muted small">
                Pass mark 65% · Above ≥80 · At 65–79 · Below &lt;65
                {latest.violations ? ` · ${latest.violations} integrity event(s) recorded` : ''}
              </p>
              <div style={{ textAlign: 'start', maxWidth: 520, margin: '1.1rem auto 0' }}>
                <DomainBands breakdown={breakdown} />
              </div>
              <div className="row" style={{ justifyContent: 'center', gap: '.6rem', marginTop: '1.1rem', flexWrap: 'wrap' }}>
                <button className="btn secondary sm" onClick={() => { void downloadScore(latest, me) }}>
                  Download score report
                </button>
                {pass ? (
                  <Link className="btn sm" to="/credentials">View credential</Link>
                ) : (
                  <Link className="btn sm" to="/billing">Plan a retake</Link>
                )}
              </div>
            </div>
          )}
        </Card>

        <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
          <Card title="Attempt history">
            {attempts.map((a) => <HistoryRow key={a.id} a={a} me={me} />)}
          </Card>
          {!held && !invalidated && !pass && (
            <Card>
              <div className="notice">
                <strong>Retake guidance.</strong> Focus on domains marked Below target. Retakes
                follow the retake policy — the fee is published before launch and a short interval
                applies.
              </div>
            </Card>
          )}
          {attempts.some((a) => a.status === 'submitted') && (
            <Card>
              <div className="muted small">
                Latest attempt status: <StatusBadge status={latest.result_status || latest.status} />
              </div>
            </Card>
          )}
        </div>
      </div>
    </div>
  )
}
