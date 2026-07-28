import { useRef, useState, type FormEvent } from 'react'
import { useQuery, runMutation } from '../api/hooks'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote, Empty, StatusBadge } from '../components/ui'
import { ViewDownloadActions } from '../components/documents/DocumentActions'
import { fileToDataUri, fmtBytes, studentToken } from '../files'
import { fmtDate } from '../format'
import { useT } from '../i18n'
import { PageHeader } from '../components/premium'

interface CpdRow {
  id: number
  activity_date?: string | null
  category?: string | null
  hours?: number | null
  description?: string | null
  evidence_name?: string | null
  status?: string | null
}

// Evidence uploads go through Storage.DecodeDataUri server-side (3 MB, jpeg/png/webp/pdf).
const EVIDENCE_MAX_BYTES = 3_000_000
const EVIDENCE_ACCEPT = '.pdf,.png,.jpg,.jpeg,.webp,application/pdf,image/png,image/jpeg,image/webp'

/** Per-entry evidence cell: view/download what is on file, or attach a file (returns the entry to
 *  the review queue). Backed by POST/GET /api/me/cpd/{id}/evidence with ownership checks. */
function CpdEvidenceCell({ row, onChanged }: { row: CpdRow; onChanged: () => void }) {
  const t = useT()
  const inputRef = useRef<HTMLInputElement>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  async function attach(f: File | null) {
    if (!f) return
    setErr(null)
    if (f.size > EVIDENCE_MAX_BYTES) { setErr(t('doc.fileTooLarge', { size: fmtBytes(EVIDENCE_MAX_BYTES) ?? '3 MB' })); return }
    setBusy(true)
    try {
      await api.post(`/api/me/cpd/${row.id}/evidence`, { filename: f.name, data_uri: await fileToDataUri(f) })
      onChanged()
    } catch (e) {
      setErr(e instanceof Error ? e.message : t('doc.uploadFailed'))
    } finally {
      setBusy(false)
      if (inputRef.current) inputRef.current.value = ''
    }
  }

  return (
    <div style={{ display: 'grid', gap: '.25rem', justifyItems: 'start' }}>
      {row.evidence_name ? (
        <ViewDownloadActions
          info={{ title: row.evidence_name, filename: row.evidence_name }}
          inlineUrl={`/api/me/cpd/${row.id}/evidence?inline=1`}
          downloadUrl={`/api/me/cpd/${row.id}/evidence`}
          token={studentToken()}
        />
      ) : (
        <>
          <button className="btn ghost sm" disabled={busy} onClick={() => inputRef.current?.click()}>
            {busy ? t('doc.uploading') : t('doc.addEvidence')}
          </button>
          <input ref={inputRef} type="file" accept={EVIDENCE_ACCEPT} style={{ display: 'none' }}
            onChange={(e) => attach(e.target.files?.[0] ?? null)} aria-label={t('doc.addEvidence')} />
        </>
      )}
      {err && <span className="small" role="alert" style={{ color: 'var(--err, #c2410c)' }}>{err}</span>}
    </div>
  )
}
interface CpdData {
  rows: CpdRow[]
  categories?: string[]
  ai_category?: string
  declaration_year?: number
  declared_this_year?: boolean
  latest_declaration?: { cycle_year: number; position: string; statement?: string | null; created_at?: string | null } | null
}

const FALLBACK_CATEGORIES = ['Structured learning', 'Practice & application', 'Contribution', 'Events & webinars', 'AI currency']
const POSITION_LABEL: Record<string, string> = { compliant: 'Compliant — I have met my CPD', career_break: 'On a career break / adjusted period', not_met: 'Not yet met' }

export default function Cpd() {
  const t = useT()
  const { me, refetch: refetchMe } = useMe()
  const { data, loading, error, refetch } = useQuery<CpdData>('/api/me/cpd')
  const categories = data?.categories?.length ? data.categories : FALLBACK_CATEGORIES
  const aiCategory = data?.ai_category ?? 'AI currency'
  const [form, setForm] = useState({ description: '', category: '', hours: '', activity_date: '' })
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<string | null>(null)
  const category = form.category || categories[0]

  const target = me?.cpd.target ?? 60
  const total = me?.cpd.total ?? 0
  const pct = Math.min(100, Math.round((total / target) * 100))

  async function add(e: FormEvent) {
    e.preventDefault()
    setBusy(true)
    setMsg(null)
    try {
      await api.post('/api/me/cpd', { ...form, category, hours: parseFloat(form.hours) || 0 })
      setForm({ description: '', category: '', hours: '', activity_date: '' })
      refetch()
      refetchMe()
    } catch (err) {
      setMsg(err instanceof Error ? err.message : t('cpd.saveError'))
    } finally {
      setBusy(false)
    }
  }

  const remove = (id: number) =>
    runMutation(async () => { await api.del(`/api/me/cpd/${id}`); refetch(); refetchMe() })

  return (
    <div className="page">
      <PageHeader eyebrow={t('nav.cpd')} title={t('cpd.heading')} subtitle={t('cpd.subtitle', { target })} />

      <Card title={t('cpd.progress')}>
        <div className="spread" style={{ marginBottom: '.5rem' }}>
          <strong>{t('cpd.hoursOf', { total, target })}</strong>
          <span className="muted small">{pct}%</span>
        </div>
        <div style={{ height: 10, background: 'var(--canvas)', borderRadius: 999, overflow: 'hidden' }}>
          <div style={{ width: `${pct}%`, height: '100%', background: 'var(--brand)' }} />
        </div>
        <p className="muted small" style={{ margin: '.5rem 0 0' }}>
          Only admin-approved hours count toward your total.
          {(me?.cpd.pending ?? 0) > 0 && ` You have ${me?.cpd.pending} hour(s) awaiting review.`}
        </p>
      </Card>

      <DeclarationCard data={data} onDone={refetch} />

      <Card title={t('cpd.addActivity')}>
        {msg && <div className="notice err" role="alert" style={{ marginBottom: '.75rem' }}>{msg}</div>}
        <form onSubmit={add}>
          <div className="grid cols-2">
            <div className="field">
              <label htmlFor="cpd-activity">{t('cpd.activity')}</label>
              <input id="cpd-activity" required value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} placeholder={t('cpd.activityPlaceholder')} />
            </div>
            <div className="field">
              <label htmlFor="cpd-category">{t('cpd.category')}</label>
              <select id="cpd-category" value={category} onChange={(e) => setForm({ ...form, category: e.target.value })}>
                {categories.map((c) => <option key={c}>{c}</option>)}
              </select>
              {category === aiCategory && <span className="muted small">Mandatory AI-currency component of the framework.</span>}
            </div>
            <div className="field">
              <label htmlFor="cpd-hours">{t('cpd.hours')}</label>
              <input id="cpd-hours" type="number" step="0.5" min="0" required value={form.hours} onChange={(e) => setForm({ ...form, hours: e.target.value })} />
            </div>
            <div className="field">
              <label htmlFor="cpd-date">{t('cpd.date')}</label>
              <input id="cpd-date" type="date" value={form.activity_date} onChange={(e) => setForm({ ...form, activity_date: e.target.value })} />
            </div>
          </div>
          <button className="btn sm" disabled={busy}>{busy ? t('cpd.saving') : t('cpd.addEntry')}</button>
        </form>
      </Card>

      <Card title={t('cpd.loggedActivities')}>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>{t('cpd.empty')}</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>{t('cpd.date')}</th><th>{t('cpd.activity')}</th><th>{t('cpd.category')}</th><th>{t('cpd.hours')}</th><th>{t('cpd.status')}</th><th>{t('doc.evidence')}</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => (
                <tr key={r.id}>
                  <td>{fmtDate(r.activity_date)}</td>
                  <td>{r.description}</td>
                  <td>{r.category}</td>
                  <td>{r.hours}</td>
                  <td><StatusBadge status={r.status || 'pending'} /></td>
                  <td><CpdEvidenceCell row={r} onChanged={() => { refetch(); refetchMe() }} /></td>
                  <td><button className="btn ghost sm" onClick={() => remove(r.id)} aria-label={t('cpd.deleteAria')}>{t('cpd.remove')}</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>
    </div>
  )
}

// Annual CPD declaration — "declared, not discovered". The member states their CPD position for the year.
function DeclarationCard({ data, onDone }: { data?: CpdData | null; onDone: () => void }) {
  const year = data?.declaration_year ?? new Date().getUTCFullYear()
  const declared = !!data?.declared_this_year
  const latest = data?.latest_declaration ?? null
  const [position, setPosition] = useState('compliant')
  const [statement, setStatement] = useState('')
  const [busy, setBusy] = useState(false)
  const [note, setNote] = useState<{ ok: boolean; text: string } | null>(null)
  const [reopen, setReopen] = useState(false)

  async function submit() {
    setBusy(true); setNote(null)
    try {
      await api.post('/api/me/cpd/declaration', { position, statement: statement.trim() || undefined })
      setNote({ ok: true, text: `Your ${year} CPD declaration has been recorded.` })
      setReopen(false); onDone()
    } catch (e) {
      setNote({ ok: false, text: e instanceof Error ? e.message : 'Could not record your declaration.' })
    } finally { setBusy(false) }
  }

  return (
    <Card title={`Annual CPD declaration (${year})`}>
      {declared && !reopen ? (
        <div className="stack small" style={{ gap: '.35rem' }}>
          <div><StatusBadge status="completed" /> You have declared your CPD position for {year}
            {latest?.position ? `: ${POSITION_LABEL[latest.position] ?? latest.position}` : ''}
            {latest?.created_at ? ` (${fmtDate(latest.created_at)})` : ''}.</div>
          <div><button className="btn ghost sm" onClick={() => setReopen(true)}>Update declaration</button></div>
        </div>
      ) : (
        <div className="stack" style={{ display: 'grid', gap: '.5rem' }}>
          <p className="muted small" style={{ margin: 0 }}>
            Confirm your continuing professional development position for {year}. What matters is that your
            position is declared, not discovered — tell PCI early if you are on a career break or adjusted period.
          </p>
          <div className="field">
            <label htmlFor="cpd-position">Your CPD position</label>
            <select id="cpd-position" value={position} onChange={(e) => setPosition(e.target.value)}>
              <option value="compliant">{POSITION_LABEL.compliant}</option>
              <option value="career_break">{POSITION_LABEL.career_break}</option>
              <option value="not_met">{POSITION_LABEL.not_met}</option>
            </select>
          </div>
          <div className="field">
            <label htmlFor="cpd-statement">Notes (optional)</label>
            <textarea id="cpd-statement" rows={2} maxLength={2000} value={statement} onChange={(e) => setStatement(e.target.value)} placeholder="Anything PCI should know about your CPD this period." />
          </div>
          {note && <div className={'small' + (note.ok ? ' muted' : '')} style={note.ok ? undefined : { color: 'var(--err, #c2410c)' }} role="status">{note.text}</div>}
          <div className="row">
            <button className="btn sm" disabled={busy} onClick={submit}>{busy ? 'Submitting…' : 'Submit declaration'}</button>
            {reopen && <button className="btn ghost sm" onClick={() => setReopen(false)}>Cancel</button>}
          </div>
        </div>
      )}
    </Card>
  )
}
