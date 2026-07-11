import { useEffect, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import { api, ApiError } from '../api/client'
import { useMe } from '../data/MeContext'
import { useQuery } from '../api/hooks'
import { Card, Badge } from './ui'
import { fmtDate } from '../format'

interface ValidateResp {
  valid: boolean
  in_window: boolean
  route?: string | null
  grants?: { membership: boolean; exam: boolean; study: boolean } | null
  requires_application?: boolean
  window_ends?: string | null
}

interface FoundingApp {
  id: number
  status: string
  route?: string | null
  admin_note?: string | null
  created_at?: string | null
  code?: string | null
  evidence_name?: string | null
  evidence_size?: number | null
}

const APP_BADGE: Record<string, { tone: 'ok' | 'err' | 'brand' | 'warn'; label: string }> = {
  auto_approved: { tone: 'ok', label: 'Approved' },
  approved: { tone: 'ok', label: 'Approved' },
  pending_review: { tone: 'brand', label: 'Under review' },
  rejected: { tone: 'err', label: 'Not approved' },
  revoked: { tone: 'err', label: 'Revoked' },
}

function grantsText(g?: { membership: boolean; exam: boolean; study: boolean } | null) {
  if (!g) return ''
  const parts = []
  if (g.membership) parts.push('membership')
  if (g.exam) parts.push('certification exam entry')
  if (g.study) parts.push('full study access')
  return parts.join(' + ')
}

/** Founding-stage access: enter a board-issued code to waive fees during the founding window.
 *  Route 1 redeems instantly; Route 2 opens the founding application (evidence required). */
export default function FoundingCard() {
  const { refetch } = useMe()
  const [params] = useSearchParams()
  const [code, setCode] = useState(params.get('founding')?.toUpperCase() ?? '')
  const [checked, setChecked] = useState<ValidateResp | null>(null)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)
  // application form state (Route 2)
  const [years, setYears] = useState('')
  const [role, setRole] = useState('')
  const [qual, setQual] = useState('')
  const [file, setFile] = useState<File | null>(null)

  const { data: appData, refetch: refetchApp } = useQuery<{ latest: FoundingApp | null; rows: FoundingApp[] }>('/api/me/founding-application')
  const latest = appData?.latest ?? null

  // deep-linked code (?founding=CODE from the public site or registration) → validate on arrival
  useEffect(() => {
    if (params.get('founding') && !checked) void check()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  async function check() {
    if (!code.trim()) return
    setBusy(true)
    setMsg(null)
    try {
      const v = await api.post<ValidateResp>('/api/founding/validate', { code: code.trim() })
      setChecked(v)
      if (!v.valid) setMsg({ ok: false, text: 'This code is not valid or its founding window has closed — standard pricing applies.' })
    } catch {
      setMsg({ ok: false, text: 'Could not check the code — please try again.' })
    } finally {
      setBusy(false)
    }
  }

  async function redeem() {
    setBusy(true)
    setMsg(null)
    try {
      const r = await api.post<{ ok: boolean; needs_application?: boolean }>('/api/founding/redeem', { code: code.trim() })
      if (r.needs_application) return // handled by form reveal below
      setMsg({ ok: true, text: 'Founding access applied — welcome to the founding cohort. Your account has been updated.' })
      setChecked(null)
      refetch()
    } catch (e) {
      const codeStr = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setMsg({ ok: false, text: codeStr === 'already_redeemed' ? 'You have already redeemed this code.' : 'This code could not be redeemed — standard pricing applies.' })
    } finally {
      setBusy(false)
    }
  }

  async function apply() {
    if (!file) { setMsg({ ok: false, text: 'Please attach evidence (CV, certificate or reference — JPG, PNG, WEBP or PDF, max 3 MB).' }); return }
    if (file.size > 3_000_000) { setMsg({ ok: false, text: 'The evidence file is too large — the maximum is 3 MB.' }); return }
    setBusy(true)
    setMsg(null)
    try {
      const dataUri = await new Promise<string>((resolve, reject) => {
        const r = new FileReader()
        r.onload = () => resolve(String(r.result))
        r.onerror = () => reject(new Error('Could not read the file.'))
        r.readAsDataURL(file)
      })
      const r = await api.post<{ status: string }>('/api/me/founding-application', {
        code: code.trim(),
        experience_years: Number(years) || 0,
        role,
        qualification: qual,
        evidence: dataUri,
        evidence_name: file.name,
      })
      if (r.status === 'auto_approved') {
        setMsg({ ok: true, text: 'Your founding application was approved — membership and exam access have been granted at USD 0.' })
        refetch()
      } else {
        setMsg({ ok: true, text: 'Your founding application is under review. We will notify you of the outcome.' })
      }
      setChecked(null)
      refetchApp()
    } catch (e) {
      const codeStr = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      const friendly: Record<string, string> = {
        evidence_required: 'Evidence is required — please attach a file.',
        file_type_not_allowed: 'Please upload a JPG, PNG, WEBP or PDF file.',
        file_too_large: 'That file is too large — the maximum is 3 MB.',
        application_exists: 'You already have a founding application for this code.',
        already_redeemed: 'You have already redeemed this code.',
      }
      setMsg({ ok: false, text: friendly[codeStr] || 'The application could not be submitted — please try again.' })
    } finally {
      setBusy(false)
    }
  }

  const badge = latest ? APP_BADGE[latest.status] : null

  return (
    <Card title="Founding access" action={badge ? <Badge tone={badge.tone}>{badge.label}</Badge> : <Badge tone="warn">Limited period</Badge>}>
      {latest && (
        <div className={'notice' + (latest.status === 'rejected' || latest.status === 'revoked' ? ' err' : '')} style={{ marginBottom: '.75rem' }}>
          Founding application ({latest.code}): <strong>{APP_BADGE[latest.status]?.label ?? latest.status}</strong>
          {latest.evidence_name ? <> · evidence submitted: {latest.evidence_name}{latest.evidence_size ? ` (${Math.round(Number(latest.evidence_size) / 1024)} KB)` : ''}</> : null}
          {latest.admin_note ? <> — {latest.admin_note}</> : null}
          {latest.status === 'pending_review' && ' — we will notify you of the outcome.'}
        </div>
      )}
      <p className="muted small" style={{ marginTop: 0 }}>
        Have a founding code from the institute? Enter it here — during the founding window it waives
        the relevant fees entirely. Without a valid code, standard pricing above applies.
      </p>
      {msg && <div className={'notice' + (msg.ok ? '' : ' err')} role="status" style={{ marginBottom: '.75rem' }}>{msg.text}</div>}

      <div className="row" style={{ flexWrap: 'wrap' }}>
        <input
          style={{ maxWidth: 220, textTransform: 'uppercase' }}
          placeholder="Founding code"
          value={code}
          onChange={(ev) => { setCode(ev.target.value.toUpperCase()); setChecked(null) }}
          aria-label="Founding code"
        />
        <button className="btn sm secondary" disabled={busy || !code.trim()} onClick={check}>{busy ? 'Checking…' : 'Check code'}</button>
      </div>

      {checked?.valid && (
        <div style={{ marginTop: '.9rem' }}>
          <div className="notice" style={{ marginBottom: '.75rem' }}>
            <strong>Founding access — fees waived.</strong>{' '}
            This code grants {grantsText(checked.grants)} at USD 0
            {checked.window_ends ? <> — valid until {fmtDate(checked.window_ends)}</> : null}.
            {checked.grants?.exam ? ' The exam itself is the same real exam — the credential is earned by passing it.' : ''}
          </div>
          {checked.requires_application ? (
            <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
              <p className="muted small" style={{ margin: 0 }}>
                This route asks for a short founding application — your declared experience plus one
                piece of evidence (CV, certificate or reference).
              </p>
              <div className="grid cols-2">
                <div className="field" style={{ margin: 0 }}>
                  <label htmlFor="fnd-years">Years of experience</label>
                  <input id="fnd-years" type="number" min={0} max={60} value={years} onChange={(ev) => setYears(ev.target.value)} />
                </div>
                <div className="field" style={{ margin: 0 }}>
                  <label htmlFor="fnd-role">Current role</label>
                  <input id="fnd-role" value={role} onChange={(ev) => setRole(ev.target.value)} placeholder="e.g. Project controls lead" />
                </div>
              </div>
              <div className="field" style={{ margin: 0 }}>
                <label htmlFor="fnd-qual">Highest qualification</label>
                <input id="fnd-qual" value={qual} onChange={(ev) => setQual(ev.target.value)} placeholder="e.g. BSc Engineering" />
              </div>
              <div className="field" style={{ margin: 0 }}>
                <label htmlFor="fnd-evidence">Evidence (JPG, PNG, WEBP or PDF — max 3 MB)</label>
                <input id="fnd-evidence" type="file" accept=".jpg,.jpeg,.png,.webp,.pdf,image/jpeg,image/png,image/webp,application/pdf"
                  onChange={(ev) => setFile(ev.target.files?.[0] ?? null)} />
              </div>
              <div className="row">
                <button className="btn sm" disabled={busy} onClick={apply}>{busy ? 'Submitting…' : 'Submit founding application'}</button>
              </div>
            </div>
          ) : (
            <button className="btn sm" disabled={busy} onClick={redeem}>{busy ? 'Applying…' : 'Redeem — USD 0'}</button>
          )}
        </div>
      )}
    </Card>
  )
}
