import { useState } from 'react'
import { useQuery } from '../api/hooks'
import { api, ApiError } from '../api/client'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../components/ui'
import { fmtDate } from '../format'

// Member events & webinars. Register for published upcoming events; attending an event (marked by PCI)
// credits CPD automatically. Backend: /api/me/events.

interface EventRow {
  id: number
  title: string
  summary?: string | null
  event_type?: string | null
  starts_at?: string | null
  ends_at?: string | null
  timezone?: string | null
  location?: string | null
  join_url?: string | null
  capacity?: number | null
  cpd_hours?: number | null
  cpd_category?: string | null
  registered?: boolean
  attended?: boolean
  seats_left?: number | null
}

const TYPE_LABEL: Record<string, string> = { webinar: 'Webinar', workshop: 'Workshop', conference: 'Conference', course: 'Course' }

export default function Events() {
  const { data, loading, error, refetch } = useQuery<{ rows: EventRow[] }>('/api/me/events')
  const [busy, setBusy] = useState<number | null>(null)
  const [msg, setMsg] = useState<string | null>(null)

  async function act(id: number, path: 'register' | 'cancel') {
    setBusy(id); setMsg(null)
    try {
      await api.post(`/api/me/events/${id}/${path}`, {})
      refetch()
    } catch (e) {
      const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : null
      setMsg((typeof body?.message === 'string' && body.message) || 'Could not update your registration. Please try again.')
    } finally { setBusy(null) }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Events &amp; webinars</h1>
        <p className="muted">Register for PCI events. Attending an event earns CPD toward your recertification automatically.</p>
      </div>
      {msg && <div className="notice err" role="alert">{msg}</div>}

      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data || data.rows.length === 0 ? (
        <Card><Empty>No upcoming events right now. Check back soon.</Empty></Card>
      ) : (
        data.rows.map((e) => {
          const full = !e.registered && e.seats_left != null && e.seats_left <= 0
          return (
            <Card
              key={e.id}
              title={e.title}
              action={
                <div className="row" style={{ gap: '.35rem' }}>
                  <Badge tone="neutral">{TYPE_LABEL[e.event_type ?? ''] ?? e.event_type}</Badge>
                  {!!e.cpd_hours && e.cpd_hours > 0 && <Badge tone="brand">{e.cpd_hours}h CPD</Badge>}
                  {e.attended ? <Badge tone="ok">Attended</Badge> : e.registered ? <Badge tone="ok">Registered</Badge> : null}
                </div>
              }
            >
              <div className="stack small" style={{ gap: '.35rem' }}>
                {e.summary && <div>{e.summary}</div>}
                <div className="muted">
                  {e.starts_at ? fmtDate(e.starts_at) : 'Date to be announced'}{e.timezone ? ` (${e.timezone})` : ''}
                  {e.location ? ` · ${e.location}` : ''}
                  {e.cpd_category ? ` · CPD: ${e.cpd_category}` : ''}
                  {e.seats_left != null && !e.registered ? ` · ${e.seats_left} seat(s) left` : ''}
                </div>
                <div className="row" style={{ marginTop: '.3rem', flexWrap: 'wrap', gap: '.4rem' }}>
                  {e.registered && e.join_url && <a className="btn sm" href={e.join_url} target="_blank" rel="noreferrer">Join ↗</a>}
                  {!e.registered && (
                    <button className="btn sm" disabled={busy === e.id || full} onClick={() => act(e.id, 'register')}>
                      {full ? 'Fully booked' : busy === e.id ? 'Registering…' : 'Register'}
                    </button>
                  )}
                  {e.registered && !e.attended && (
                    <button className="btn sm secondary" disabled={busy === e.id} onClick={() => act(e.id, 'cancel')}>
                      {busy === e.id ? 'Cancelling…' : 'Cancel registration'}
                    </button>
                  )}
                </div>
              </div>
            </Card>
          )
        })
      )}
    </div>
  )
}
