import { useState } from 'react'
import { useAdminQuery, runMutation } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../../components/ui'
import { fmtDate } from '../../format'

// Admin management of member events & webinars. Create/edit/publish events; view registrations; mark
// attendance (which auto-credits the attendee's CPD for the event's hours). Backend: Events.cs (gated 'content').

interface EventRow {
  id: number; title: string; summary?: string | null; description?: string | null
  event_type?: string | null; starts_at?: string | null; ends_at?: string | null; timezone?: string | null
  location?: string | null; join_url?: string | null; capacity?: number | null
  cpd_hours?: number | null; cpd_category?: string | null; status: string
  registrations?: number; attended?: number
}
interface RegRow {
  id: number; user_id: number; status: string; registered_at?: string | null; attended_at?: string | null
  email?: string | null; first_name?: string | null; last_name?: string | null
}

const EMPTY: Partial<EventRow> = { title: '', event_type: 'webinar', status: 'draft', cpd_hours: 1, cpd_category: 'AI currency', capacity: 0 }

export default function Events() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: EventRow[] }>('/api/admin/events')
  const [edit, setEdit] = useState<Partial<EventRow> | null>(null)
  const [regsFor, setRegsFor] = useState<EventRow | null>(null)

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <div>
          <h1>Events &amp; webinars</h1>
          <p className="muted small" style={{ margin: 0 }}>Publish member events; marking attendance auto-credits the attendee's CPD.</p>
        </div>
        <button className="btn sm" onClick={() => setEdit({ ...EMPTY })}>New event</button>
      </div>

      <Card>
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No events yet.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Title</th><th>Type</th><th>When</th><th>CPD</th><th>Registered</th><th>Status</th><th></th></tr></thead>
            <tbody>
              {data.rows.map((e) => (
                <tr key={e.id}>
                  <td>{e.title}</td>
                  <td className="small">{e.event_type}</td>
                  <td className="small">{e.starts_at ? fmtDate(e.starts_at) : '—'}</td>
                  <td className="small">{e.cpd_hours ? `${e.cpd_hours}h · ${e.cpd_category}` : '—'}</td>
                  <td className="small">{e.registrations ?? 0}{e.capacity ? ` / ${e.capacity}` : ''}{e.attended ? ` (${e.attended} attended)` : ''}</td>
                  <td>{e.status === 'published' ? <Badge tone="ok">Published</Badge> : e.status === 'cancelled' ? <Badge tone="warn">Cancelled</Badge> : <Badge tone="neutral">Draft</Badge>}</td>
                  <td>
                    <div className="row" style={{ gap: '.35rem', justifyContent: 'flex-end' }}>
                      <button className="btn sm secondary" onClick={() => setRegsFor(e)}>Registrations</button>
                      <button className="btn sm ghost" onClick={() => setEdit(e)}>Edit</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {edit && <EventEditor initial={edit} onClose={() => setEdit(null)} onSaved={() => { setEdit(null); refetch() }} />}
      {regsFor && <Registrations event={regsFor} onClose={() => setRegsFor(null)} />}
    </div>
  )
}

function EventEditor({ initial, onClose, onSaved }: { initial: Partial<EventRow>; onClose: () => void; onSaved: () => void }) {
  const [d, setD] = useState<Partial<EventRow>>(initial)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const set = <K extends keyof EventRow>(k: K, v: EventRow[K]) => setD((p) => ({ ...p, [k]: v }))
  const num = (v: string) => (v === '' ? undefined : Number(v))

  async function save() {
    setBusy(true); setErr(null)
    try { await adminApi.post('/api/admin/events', d); onSaved() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Could not save.') }
    finally { setBusy(false) }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{d.id ? 'Edit event' : 'New event'}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{err}</div>}
        <div className="grid cols-2">
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Title</label><input value={d.title ?? ''} onChange={(e) => set('title', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Summary</label><input value={d.summary ?? ''} onChange={(e) => set('summary', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Description</label><textarea rows={3} value={d.description ?? ''} onChange={(e) => set('description', e.target.value)} /></div>
          <div className="field"><label>Type</label>
            <select value={d.event_type ?? 'webinar'} onChange={(e) => set('event_type', e.target.value)}>
              <option value="webinar">Webinar</option><option value="workshop">Workshop</option><option value="conference">Conference</option><option value="course">Course</option>
            </select>
          </div>
          <div className="field"><label>Status</label>
            <select value={d.status ?? 'draft'} onChange={(e) => set('status', e.target.value)}>
              <option value="draft">Draft</option><option value="published">Published</option><option value="cancelled">Cancelled</option>
            </select>
          </div>
          <div className="field"><label>Starts (YYYY-MM-DD HH:MM)</label><input value={d.starts_at ?? ''} onChange={(e) => set('starts_at', e.target.value)} placeholder="2026-09-01 15:00" /></div>
          <div className="field"><label>Ends</label><input value={d.ends_at ?? ''} onChange={(e) => set('ends_at', e.target.value)} placeholder="2026-09-01 16:00" /></div>
          <div className="field"><label>Timezone</label><input value={d.timezone ?? ''} onChange={(e) => set('timezone', e.target.value)} placeholder="UTC" /></div>
          <div className="field"><label>Location</label><input value={d.location ?? ''} onChange={(e) => set('location', e.target.value)} placeholder="Online" /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Join URL (shown only to registrants)</label><input value={d.join_url ?? ''} onChange={(e) => set('join_url', e.target.value)} placeholder="https://…" /></div>
          <div className="field"><label>Capacity (0 = unlimited)</label><input type="number" min="0" value={d.capacity ?? 0} onChange={(e) => set('capacity', num(e.target.value) as number)} /></div>
          <div className="field"><label>CPD hours (credited on attendance)</label><input type="number" min="0" step="0.5" value={d.cpd_hours ?? 0} onChange={(e) => set('cpd_hours', num(e.target.value) as number)} /></div>
          <div className="field"><label>CPD category</label>
            <select value={d.cpd_category ?? 'Events & webinars'} onChange={(e) => set('cpd_category', e.target.value)}>
              <option>Events &amp; webinars</option><option>Structured learning</option><option>AI currency</option><option>Contribution</option>
            </select>
          </div>
        </div>
        <div className="row" style={{ marginTop: '1rem' }}>
          <button className="btn" disabled={busy || !(d.title && d.title.length >= 3)} onClick={save}>{busy ? 'Saving…' : d.id ? 'Save changes' : 'Create event'}</button>
        </div>
      </div>
    </div>
  )
}

function Registrations({ event, onClose }: { event: EventRow; onClose: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: RegRow[] }>(`/api/admin/events/${event.id}/registrations`)

  const mark = (r: RegRow, attended: boolean) =>
    runMutation(async () => { await adminApi.post(`/api/admin/events/${event.id}/attendance`, { user_id: r.user_id, attended }); refetch() })

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Registrations — {event.title}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        <p className="muted small" style={{ marginTop: 0 }}>Marking a registrant attended credits {event.cpd_hours ?? 0}h CPD ({event.cpd_category}).</p>
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
          <Empty>No registrations yet.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Member</th><th>Registered</th><th>Status</th><th></th></tr></thead>
            <tbody>
              {data.rows.filter((r) => r.status !== 'cancelled').map((r) => (
                <tr key={r.id}>
                  <td><div>{[r.first_name, r.last_name].filter(Boolean).join(' ') || '—'}</div><div className="muted small">{r.email} · #{r.user_id}</div></td>
                  <td className="small">{fmtDate(r.registered_at)}</td>
                  <td>{r.status === 'attended' ? <StatusBadge status="attended" /> : <Badge tone="neutral">Registered</Badge>}</td>
                  <td>
                    {r.status === 'attended'
                      ? <button className="btn sm ghost" onClick={() => mark(r, false)}>Un-mark</button>
                      : <button className="btn sm" onClick={() => mark(r, true)}>Mark attended</button>}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}
