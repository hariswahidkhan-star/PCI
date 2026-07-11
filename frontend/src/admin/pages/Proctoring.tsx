import { useEffect, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type ExamSessionRow, type ExamSessionDetail } from '../api'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty, rowActivate } from '../../components/ui'
import { fmtDateTime, titleCase } from '../../format'

function sevTone(sev?: string | null): 'err' | 'warn' | 'brand' | 'neutral' {
  const s = (sev ?? '').toLowerCase()
  return s === 'critical' || s === 'high' ? 'err' : s === 'medium' ? 'warn' : s === 'low' ? 'brand' : 'neutral'
}

function mmss(sec?: number | null) {
  if (sec == null || sec < 0) return '—'
  const m = Math.floor(sec / 60), s = sec % 60
  return `${m}:${String(s).padStart(2, '0')}`
}

function SessionDrawer({ id, onClose, onChanged }: { id: number; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<ExamSessionDetail>(`/api/admin/exam-sessions/${id}`)
  const [note, setNote] = useState('')
  const [msg, setMsg] = useState('')
  const [busy, setBusy] = useState<string | null>(null)
  const [actionMsg, setActionMsg] = useState<string | null>(null)

  const attempt = (data?.attempt ?? {}) as Record<string, unknown>
  const held = attempt.result_status === 'auto_held'

  async function review(action: string) {
    if (action === 'invalidate' && !confirm('Invalidate this attempt? Any linked credential will be revoked.')) return
    setBusy(action)
    setActionMsg(null)
    try {
      await adminApi.post(`/api/admin/exam-sessions/${id}/review`, { action, note })
      setNote('')
      refetch()
      onChanged()
      setActionMsg(`Action “${action}” applied.`)
    } catch (e) {
      setActionMsg(e instanceof Error ? e.message : 'Action failed.')
    } finally {
      setBusy(null)
    }
  }
  async function sendMessage() {
    if (!msg.trim()) return
    setBusy('message')
    try {
      await adminApi.post(`/api/admin/exam-sessions/${id}/message`, { body: msg })
      setMsg('')
      refetch()
    } finally {
      setBusy(null)
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Exam session #{id}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data ? null : (
          <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
            <Card
              title={`${data.user.first_name ?? ''} ${data.user.last_name ?? ''}`.trim() || String(data.user.email ?? 'Candidate')}
              action={<StatusBadge status={String(attempt.result_status || attempt.status || '')} />}
            >
              <div className="grid cols-2 small">
                <div><span className="muted">Email</span><div>{String(data.user.email ?? '—')}</div></div>
                <div><span className="muted">Client</span><div>{String(attempt.client_kind ?? '—')}</div></div>
                <div><span className="muted">Started</span><div>{fmtDateTime(attempt.started_at)}</div></div>
                <div><span className="muted">Submitted</span><div>{fmtDateTime(attempt.submitted_at)}</div></div>
                <div><span className="muted">Score</span><div>{held ? <em className="muted">held — not shown</em> : attempt.percent != null ? `${attempt.percent}%` : '—'}</div></div>
                <div><span className="muted">Answered</span><div>{data.summary.answered}</div></div>
              </div>
              {attempt.hold_reason ? <div className="notice warn" style={{ marginTop: '.6rem' }}>Hold reason: {String(attempt.hold_reason)}</div> : null}
              <div className="row" style={{ marginTop: '.6rem', flexWrap: 'wrap', gap: '.4rem' }}>
                <Badge>{data.summary.total_events} events</Badge>
                {Object.entries(data.summary.by_severity).map(([s, n]) => <Badge key={s} tone={sevTone(s)}>{s}: {n}</Badge>)}
              </div>
            </Card>

            {/* Review actions */}
            <Card title="Review">
              {actionMsg && <div className="notice" style={{ marginBottom: '.6rem' }}>{actionMsg}</div>}
              <div className="field" style={{ marginBottom: '.6rem' }}>
                <label>Review note (optional)</label>
                <input value={note} onChange={(e) => setNote(e.target.value)} placeholder="Reason / context for the decision" />
              </div>
              <div className="row" style={{ flexWrap: 'wrap' }}>
                {held && <button className="btn sm" disabled={!!busy} onClick={() => review('release')}>Release held result</button>}
                <button className="btn sm secondary" disabled={!!busy} onClick={() => review('reinstate')}>Reinstate</button>
                <button className="btn sm secondary" disabled={!!busy} onClick={() => review('clear')}>Mark reviewed</button>
                <button className="btn ghost sm" disabled={!!busy} onClick={() => review('invalidate')}>Invalidate</button>
              </div>
            </Card>

            {/* Events */}
            <Card title={`Proctoring events (${data.events.length})`}>
              {data.events.length === 0 ? <Empty>No events recorded.</Empty> : (
                <div className="stack" style={{ display: 'grid', gap: '.4rem', maxHeight: 260, overflowY: 'auto' }}>
                  {data.events.map((e, i) => (
                    <div key={i} className="row" style={{ gap: '.5rem', alignItems: 'baseline' }}>
                      <Badge tone={sevTone(e.severity)}>{e.severity}</Badge>
                      <span style={{ fontWeight: 600 }}>{titleCase(e.type ?? '')}</span>
                      <span className="small muted">{e.detail}</span>
                      <span className="small muted" style={{ marginLeft: 'auto' }}>{fmtDateTime(e.at)}</span>
                    </div>
                  ))}
                </div>
              )}
            </Card>

            {/* Evidence + identity */}
            <div className="grid cols-2">
              <Card title={`Evidence (${data.evidence.length})`}>
                {data.evidence.length === 0 ? <Empty>None.</Empty> : (
                  <ul className="small" style={{ margin: 0, paddingLeft: '1.1rem' }}>
                    {data.evidence.slice(0, 12).map((ev, i) => <li key={i}>{String(ev.kind ?? 'file')} · {String(ev.mime ?? '')} · {ev.size_bytes ? `${Math.round(Number(ev.size_bytes) / 1024)} KB` : ''}</li>)}
                  </ul>
                )}
              </Card>
              <Card title={`Identity checks (${data.identity.length})`}>
                {data.identity.length === 0 ? <Empty>None.</Empty> : (
                  <ul className="small" style={{ margin: 0, paddingLeft: '1.1rem' }}>
                    {data.identity.map((c, i) => <li key={i}>{String(c.result ?? '')}{c.confidence != null ? ` (${c.confidence})` : ''} — {String(c.note ?? '')}</li>)}
                  </ul>
                )}
              </Card>
            </div>

            {/* Messages */}
            <Card title={`Messages (${data.messages.length})`}>
              {data.messages.length > 0 && (
                <div className="stack" style={{ display: 'grid', gap: '.5rem', marginBottom: '.75rem' }}>
                  {data.messages.map((m, i) => (
                    <div key={i} style={{ padding: '.5rem .7rem', borderRadius: 9, background: m.sender === 'candidate' ? 'var(--canvas)' : 'var(--brand-050)' }}>
                      <div className="small" style={{ fontWeight: 700 }}>{m.sender === 'candidate' ? 'Candidate' : 'Proctor'} <span className="muted" style={{ fontWeight: 400 }}>· {fmtDateTime(m.created_at)}</span></div>
                      <div>{m.body}</div>
                    </div>
                  ))}
                </div>
              )}
              <div className="field" style={{ margin: 0 }}>
                <input value={msg} onChange={(e) => setMsg(e.target.value)} placeholder="Message the candidate…" />
              </div>
              <button className="btn sm" style={{ marginTop: '.5rem' }} disabled={busy === 'message' || !msg.trim()} onClick={sendMessage}>Send</button>
            </Card>
          </div>
        )}
      </div>
    </div>
  )
}

export default function Proctoring() {
  const [live, setLive] = useState(false)
  const [selected, setSelected] = useState<number | null>(null)
  const { data, loading, error, refetch } = useAdminQuery<{ rows: ExamSessionRow[] }>(live ? '/api/admin/exam-sessions/live' : '/api/admin/exam-sessions')

  // The Live tab shows server-computed countdowns and heartbeat freshness; without polling they
  // freeze after load, so a proctor sees stale "time left" and a candidate whose heartbeat has
  // actually gone stale still shows green. Poll while Live is active; the interval is cleared when
  // leaving the Live tab or on unmount. The same-path refetch is a background refresh (see
  // useAdminQuery), so it never blanks the table to a spinner.
  useEffect(() => {
    if (!live) return
    const t = setInterval(() => refetch(), 12000)
    return () => clearInterval(t)
  }, [live, refetch])

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Proctoring &amp; sessions</h1>
        <div className="row">
          <button className={'btn sm' + (live ? '' : ' secondary')} onClick={() => setLive(true)}>Live</button>
          <button className={'btn sm' + (!live ? '' : ' secondary')} onClick={() => setLive(false)}>All sessions</button>
          <button className="btn ghost sm" onClick={() => refetch()}>Refresh</button>
        </div>
      </div>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>{live ? 'No exams are in progress right now.' : 'No exam sessions yet.'}</Empty>
        ) : live ? (
          <table className="data">
            <thead>
              <tr><th>Candidate</th><th>Time left</th><th>Heartbeat</th><th>High flags</th><th>Msgs</th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => {
                const stale = (r.seconds_since_beat ?? 0) > 60
                return (
                  <tr key={r.id} {...rowActivate(() => setSelected(r.id))}>
                    <td><strong>{`${r.first_name ?? ''} ${r.last_name ?? ''}`.trim() || r.email}</strong><div className="small muted">{r.email}</div></td>
                    <td>{mmss(r.remaining_s)}</td>
                    <td>{stale ? <Badge tone="err">{r.seconds_since_beat}s ago</Badge> : <Badge tone="ok">live</Badge>}</td>
                    <td>{(r.high_events ?? 0) > 0 ? <Badge tone="err">{r.high_events}</Badge> : 0}</td>
                    <td>{r.candidate_msgs ?? 0}</td>
                    <td><button className="btn ghost sm">Monitor</button></td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Candidate</th><th>Started</th><th>Status</th><th>Result</th><th>Flags</th><th>Review</th></tr>
            </thead>
            <tbody>
              {data.rows.map((r) => {
                const held = r.result_status === 'auto_held'
                return (
                  <tr key={r.id} {...rowActivate(() => setSelected(r.id))}>
                    <td><strong>{`${r.first_name ?? ''} ${r.last_name ?? ''}`.trim() || r.email}</strong><div className="small muted">{r.email}</div></td>
                    <td className="small muted">{fmtDateTime(r.started_at)}</td>
                    <td><StatusBadge status={r.result_status || r.status || ''} /></td>
                    <td>{held ? <em className="muted small">held</em> : r.percent != null ? `${r.percent}%` : '—'}</td>
                    <td>{(r.high_events ?? 0) > 0 ? <Badge tone="err">{r.high_events} high</Badge> : (r.violations ?? 0)}</td>
                    <td className="small muted">{r.review_status || '—'}</td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        )}
      </Card>

      {selected !== null && <SessionDrawer id={selected} onClose={() => setSelected(null)} onChanged={refetch} />}
    </div>
  )
}
