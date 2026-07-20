import { useState, type FormEvent } from 'react'
import { useQuery } from '../api/hooks'
import { useMe } from '../data/MeContext'
import { api, ApiError } from '../api/client'
import { Card, Spinner, ErrorNote, Empty, StatusBadge } from '../components/ui'
import { fmtDate } from '../format'

// Appeals & Accommodations — student surface for the formal casework the backend already supports
// (backend/Endpoints/Casework.cs). Appeals let a candidate contest a result or an invalidated attempt,
// raise a complaint, or report an ethics concern; accommodation requests capture disability / special
// exam arrangements before booking. Both are reviewed by PCI and answered through the portal.

interface AppealRow {
  id: number
  attempt_id?: number | null
  credential_id?: string | null
  type: string
  reason?: string | null
  evidence_name?: string | null
  status?: string | null
  submitted_at?: string | null
  decision?: string | null
  decided_at?: string | null
}
interface AccomRow {
  id: number
  request_type: string
  description?: string | null
  evidence_name?: string | null
  status?: string | null
  approved_extra_minutes?: number | null
  admin_note?: string | null
  created_at?: string | null
  decided_at?: string | null
}

const APPEAL_TYPES: { value: string; label: string }[] = [
  { value: 'result_appeal', label: 'Appeal an exam result' },
  { value: 'invalidation_appeal', label: 'Appeal an invalidated attempt' },
  { value: 'complaint', label: 'Raise a complaint' },
  { value: 'ethics', label: 'Report an ethics concern' },
]
const ACCOM_TYPES: { value: string; label: string }[] = [
  { value: 'extra_time', label: 'Additional time' },
  { value: 'separate_setting', label: 'Separate / quiet setting' },
  { value: 'assistive_technology', label: 'Assistive technology' },
  { value: 'other', label: 'Other arrangement' },
]

const APPEAL_LABEL: Record<string, string> = Object.fromEntries(APPEAL_TYPES.map((t) => [t.value, t.label]))
const ACCOM_LABEL: Record<string, string> = Object.fromEntries(ACCOM_TYPES.map((t) => [t.value, t.label]))

// Read a File as a data: URI for the evidence upload (same pattern as the ID-document upload).
function toDataUri(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const r = new FileReader()
    r.onload = () => resolve(String(r.result))
    r.onerror = () => reject(new Error('Could not read the selected file.'))
    r.readAsDataURL(file)
  })
}

function errText(e: unknown, fallback: string): string {
  if (e instanceof ApiError && e.body && typeof e.body === 'object') {
    const body = e.body as Record<string, unknown>
    if (typeof body.message === 'string' && body.message) return body.message
  }
  return e instanceof Error ? e.message : fallback
}

export default function Appeals() {
  const { me } = useMe()
  const appeals = useQuery<{ rows: AppealRow[] }>('/api/me/appeals')
  const accoms = useQuery<{ rows: AccomRow[] }>('/api/me/accommodations')

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Appeals &amp; Accommodations</h1>
        <p className="muted">
          Formally contest an exam result or invalidated attempt, raise a complaint or ethics concern, or request
          special exam arrangements. PCI reviews each submission and responds through your portal.
        </p>
      </div>

      <AppealForm attempts={me?.attempts ?? []} credentials={me?.credentials ?? []} onDone={appeals.refetch} />
      <AppealList q={appeals} />

      <AccommodationForm onDone={accoms.refetch} />
      <AccommodationList q={accoms} />
    </div>
  )
}

function EvidenceField({ file, setFile }: { file: File | null; setFile: (f: File | null) => void }) {
  return (
    <label>
      Supporting evidence <span className="muted small">(optional, max 5 MB)</span>
      <input
        type="file"
        onChange={(e) => {
          const f = e.target.files?.[0] ?? null
          if (f && f.size > 5_000_000) { alert('Please choose a file under 5 MB.'); e.target.value = ''; return }
          setFile(f)
        }}
      />
      {file && <span className="muted small">Attached: {file.name}</span>}
    </label>
  )
}

function AppealForm({
  attempts,
  credentials,
  onDone,
}: {
  attempts: { id: number; kind: string; result?: string | null; submitted_at?: string | null }[]
  credentials: { credential_id: string; credential?: string | null; certification_name?: string | null }[]
  onDone: () => void
}) {
  const [type, setType] = useState('result_appeal')
  const [attemptId, setAttemptId] = useState('')
  const [credentialId, setCredentialId] = useState('')
  const [reason, setReason] = useState('')
  const [file, setFile] = useState<File | null>(null)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)

  const needsAttempt = type === 'result_appeal' || type === 'invalidation_appeal'

  async function submit(e: FormEvent) {
    e.preventDefault()
    if (reason.trim().length < 20) { setMsg({ ok: false, text: 'Please describe the grounds for your appeal (at least 20 characters).' }); return }
    setBusy(true)
    setMsg(null)
    try {
      const body: Record<string, unknown> = { type, reason: reason.trim() }
      if (attemptId) body.attempt_id = Number(attemptId)
      if (credentialId) body.credential_id = credentialId
      if (file) { body.evidence_data = await toDataUri(file); body.evidence_name = file.name }
      await api.post('/api/me/appeals', body)
      setReason(''); setAttemptId(''); setCredentialId(''); setFile(null)
      setMsg({ ok: true, text: 'Your appeal has been submitted. PCI will review it and respond through your portal.' })
      onDone()
    } catch (err) {
      setMsg({ ok: false, text: errText(err, 'Could not submit your appeal. Please try again.') })
    } finally {
      setBusy(false)
    }
  }

  return (
    <Card title="Submit an appeal or complaint">
      <form onSubmit={submit} className="stack" style={{ display: 'grid', gap: '.6rem' }}>
        <label>
          Type
          <select value={type} onChange={(e) => setType(e.target.value)}>
            {APPEAL_TYPES.map((t) => <option key={t.value} value={t.value}>{t.label}</option>)}
          </select>
        </label>
        {needsAttempt && attempts.length > 0 && (
          <label>
            Related exam attempt <span className="muted small">(optional)</span>
            <select value={attemptId} onChange={(e) => setAttemptId(e.target.value)}>
              <option value="">— select an attempt —</option>
              {attempts.map((a) => (
                <option key={a.id} value={a.id}>
                  #{a.id} · {a.kind}{a.result ? ` · ${a.result}` : ''}{a.submitted_at ? ` · ${fmtDate(a.submitted_at)}` : ''}
                </option>
              ))}
            </select>
          </label>
        )}
        {credentials.length > 0 && (
          <label>
            Related credential <span className="muted small">(optional)</span>
            <select value={credentialId} onChange={(e) => setCredentialId(e.target.value)}>
              <option value="">— select a credential —</option>
              {credentials.map((c) => (
                <option key={c.credential_id} value={c.credential_id}>
                  {c.certification_name || c.credential || c.credential_id} · {c.credential_id}
                </option>
              ))}
            </select>
          </label>
        )}
        <label>
          Grounds for your appeal
          <textarea
            value={reason}
            onChange={(e) => setReason(e.target.value)}
            rows={5}
            maxLength={5000}
            placeholder="Explain clearly why you are appealing, with any relevant dates and details (at least 20 characters)."
          />
          <span className="muted small">{reason.trim().length}/5000</span>
        </label>
        <EvidenceField file={file} setFile={setFile} />
        {msg && (
          <div className={'small' + (msg.ok ? ' muted' : '')} style={msg.ok ? undefined : { color: 'var(--err, #c2410c)' }} role="status">
            {msg.text}
          </div>
        )}
        <div className="row">
          <button className="btn" type="submit" disabled={busy}>{busy ? 'Submitting…' : 'Submit appeal'}</button>
        </div>
      </form>
    </Card>
  )
}

function AppealList({ q }: { q: ReturnType<typeof useQuery<{ rows: AppealRow[] }>> }) {
  const { data, loading, error } = q
  return (
    <Card title="Your appeals">
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
        <Empty>You have not submitted any appeals.</Empty>
      ) : (
        <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
          {data.rows.map((r) => (
            <div key={r.id} className="stack small" style={{ gap: '.25rem', borderBottom: '1px solid var(--border,#e5e7eb)', paddingBottom: '.5rem' }}>
              <div className="spread">
                <strong>{APPEAL_LABEL[r.type] ?? r.type}</strong>
                <StatusBadge status={r.status} />
              </div>
              {r.reason && <div className="muted">{r.reason}</div>}
              <div className="muted">
                Submitted {fmtDate(r.submitted_at)}
                {r.attempt_id ? ` · attempt #${r.attempt_id}` : ''}{r.credential_id ? ` · ${r.credential_id}` : ''}
                {r.evidence_name ? ' · evidence attached' : ''}
              </div>
              {r.decision && (
                <div className="small" style={{ marginTop: '.2rem' }}>
                  <span className="muted">PCI decision{r.decided_at ? ` (${fmtDate(r.decided_at)})` : ''}:</span> {r.decision}
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </Card>
  )
}

function AccommodationForm({ onDone }: { onDone: () => void }) {
  const [type, setType] = useState('extra_time')
  const [desc, setDesc] = useState('')
  const [file, setFile] = useState<File | null>(null)
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<{ ok: boolean; text: string } | null>(null)

  async function submit(e: FormEvent) {
    e.preventDefault()
    if (desc.trim().length < 20) { setMsg({ ok: false, text: 'Please describe the accommodation you need (at least 20 characters).' }); return }
    setBusy(true)
    setMsg(null)
    try {
      const body: Record<string, unknown> = { request_type: type, description: desc.trim() }
      if (file) { body.evidence_data = await toDataUri(file); body.evidence_name = file.name }
      await api.post('/api/me/accommodations', body)
      setDesc(''); setFile(null)
      setMsg({ ok: true, text: 'Your accommodation request has been submitted for review. Please allow time before booking your exam.' })
      onDone()
    } catch (err) {
      setMsg({ ok: false, text: errText(err, 'Could not submit your request. Please try again.') })
    } finally {
      setBusy(false)
    }
  }

  return (
    <Card title="Request an exam accommodation">
      <form onSubmit={submit} className="stack" style={{ display: 'grid', gap: '.6rem' }}>
        <p className="muted small" style={{ margin: 0 }}>
          Request disability or special exam arrangements. Submit this before booking your exam so PCI can review the
          request and apply any approved arrangement to your sitting.
        </p>
        <label>
          Arrangement needed
          <select value={type} onChange={(e) => setType(e.target.value)}>
            {ACCOM_TYPES.map((t) => <option key={t.value} value={t.value}>{t.label}</option>)}
          </select>
        </label>
        <label>
          Details
          <textarea
            value={desc}
            onChange={(e) => setDesc(e.target.value)}
            rows={4}
            maxLength={5000}
            placeholder="Describe the arrangement you need and why (at least 20 characters)."
          />
          <span className="muted small">{desc.trim().length}/5000</span>
        </label>
        <EvidenceField file={file} setFile={setFile} />
        {msg && (
          <div className={'small' + (msg.ok ? ' muted' : '')} style={msg.ok ? undefined : { color: 'var(--err, #c2410c)' }} role="status">
            {msg.text}
          </div>
        )}
        <div className="row">
          <button className="btn" type="submit" disabled={busy}>{busy ? 'Submitting…' : 'Submit request'}</button>
        </div>
      </form>
    </Card>
  )
}

function AccommodationList({ q }: { q: ReturnType<typeof useQuery<{ rows: AccomRow[] }>> }) {
  const { data, loading, error } = q
  return (
    <Card title="Your accommodation requests">
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
        <Empty>You have not requested any accommodations.</Empty>
      ) : (
        <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
          {data.rows.map((r) => (
            <div key={r.id} className="stack small" style={{ gap: '.25rem', borderBottom: '1px solid var(--border,#e5e7eb)', paddingBottom: '.5rem' }}>
              <div className="spread">
                <strong>{ACCOM_LABEL[r.request_type] ?? r.request_type}</strong>
                <StatusBadge status={r.status} />
              </div>
              {r.description && <div className="muted">{r.description}</div>}
              <div className="muted">
                Submitted {fmtDate(r.created_at)}
                {r.approved_extra_minutes ? ` · +${r.approved_extra_minutes} min approved` : ''}
                {r.evidence_name ? ' · evidence attached' : ''}
              </div>
              {r.admin_note && (
                <div className="small" style={{ marginTop: '.2rem' }}>
                  <span className="muted">PCI note{r.decided_at ? ` (${fmtDate(r.decided_at)})` : ''}:</span> {r.admin_note}
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </Card>
  )
}
