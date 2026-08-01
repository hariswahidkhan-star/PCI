import { useState } from 'react'
import { PageHeader } from '../components/premium'
import { useQuery } from '../api/hooks'
import { api } from '../api/client'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate, fmtDateTime } from '../format'

// Member "My applications" — the roles this signed-in member has applied for through the public
// careers board (applications are linked to the account at submit time). Backend: /api/me/applications.
// Only candidate-safe fields are returned; internal recruiter notes are never exposed here.

interface AppRow {
  id: number
  reference?: string | null
  status: string
  created_at?: string | null
  cv_name?: string | null
  title: string
  job_code?: string | null
  organisation?: string | null
  location?: string | null
  country?: string | null
}
interface AppEvent { kind: string; to_status?: string | null; body?: string | null; scheduled_at?: string | null; created_at?: string | null }

const STATUS_LABEL: Record<string, string> = {
  new: 'Submitted', reviewing: 'Under review', shortlisted: 'Shortlisted', interview: 'Interview',
  assessment: 'Assessment', offer: 'Offer in progress', hired: 'Hired', rejected: 'Not selected',
  withdrawn: 'Withdrawn', closed: 'Position closed',
}
const STATUS_TONE: Record<string, 'neutral' | 'brand' | 'ok' | 'warn'> = {
  new: 'brand', reviewing: 'brand', shortlisted: 'ok', interview: 'ok', assessment: 'brand',
  offer: 'ok', hired: 'ok', rejected: 'neutral', withdrawn: 'neutral', closed: 'neutral',
}
const label = (s: string) => STATUS_LABEL[s] ?? s

export default function Applications() {
  const { data, loading, error, refetch } = useQuery<{ rows: AppRow[] }>('/api/me/applications')
  const [openId, setOpenId] = useState<number | null>(null)
  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  const rows = data?.rows ?? []
  return (
    <div className="page">
      <PageHeader
        eyebrow="Careers"
        title="My applications"
        subtitle={<>Roles you have applied for through the PCI careers board. <a href="/careers.html">Browse open positions →</a></>}
      />
      <Card>
        {rows.length === 0 ? (
          <Empty>You haven’t applied for any roles yet. <a href="/careers.html">See open positions →</a></Empty>
        ) : (
          <div className="stack" style={{ display: 'grid', gap: '.7rem' }}>
            {rows.map((a) => (
              <div key={a.id} style={{ borderBottom: '1px solid var(--line,#e2e8f0)', paddingBottom: '.7rem' }}>
                <div className="spread" style={{ alignItems: 'flex-start', gap: '.6rem' }}>
                  <div>
                    <div style={{ fontWeight: 600 }}>{a.title}</div>
                    <div className="muted small">
                      {[a.organisation, [a.location, a.country].filter(Boolean).join(', ')].filter(Boolean).join(' · ')}
                      {a.job_code ? ` · ${a.job_code}` : ''} · applied {fmtDate(a.created_at)}
                      {a.reference ? <> · <span className="mono">{a.reference}</span></> : null}
                    </div>
                  </div>
                  <div className="row" style={{ gap: '.4rem', alignItems: 'center' }}>
                    <Badge tone={STATUS_TONE[a.status] ?? 'neutral'}>{label(a.status)}</Badge>
                    <button className="btn ghost sm" onClick={() => setOpenId(openId === a.id ? null : a.id)}>{openId === a.id ? 'Hide' : 'View'}</button>
                  </div>
                </div>
                {openId === a.id && <AppDetail id={a.id} status={a.status} onChanged={refetch} />}
              </div>
            ))}
          </div>
        )}
      </Card>
    </div>
  )
}

function AppDetail({ id, status, onChanged }: { id: number; status: string; onChanged: () => void }) {
  const { data, loading } = useQuery<{ events: AppEvent[] }>(`/api/me/applications/${id}`)
  const [busy, setBusy] = useState(false)
  const canWithdraw = status !== 'withdrawn' && status !== 'hired' && status !== 'closed'
  async function withdraw() {
    if (!window.confirm('Withdraw this application? This cannot be undone.')) return
    setBusy(true)
    try { await api.post(`/api/me/applications/${id}/withdraw`, {}); onChanged() }
    catch (e) { alert(e instanceof Error ? e.message : 'Could not withdraw.') }
    finally { setBusy(false) }
  }
  const events = data?.events ?? []
  return (
    <div style={{ marginTop: '.6rem', paddingLeft: '.2rem' }}>
      {loading ? <Spinner /> : events.length === 0 ? <div className="muted small">No updates yet.</div> : (
        <div className="stack" style={{ display: 'grid', gap: '.35rem' }}>
          {events.map((e, i) => (
            <div key={i} className="small">
              <span className="muted">{fmtDate(e.created_at)} · </span>
              {e.kind === 'status' ? <>Status: <strong>{label(e.to_status ?? '')}</strong></>
                : e.kind === 'interview' ? <><strong>Interview scheduled</strong> {fmtDateTime(e.scheduled_at)}{e.body ? ` — ${e.body}` : ''}</>
                : <><strong>Message from PCI:</strong> {e.body}</>}
            </div>
          ))}
        </div>
      )}
      {canWithdraw && <button className="btn ghost sm danger" style={{ marginTop: '.5rem' }} disabled={busy} onClick={withdraw}>Withdraw application</button>}
    </div>
  )
}
