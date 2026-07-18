import { useState } from 'react'
import { useQuery } from '../api/hooks'
import { api, ApiError } from '../api/client'
import { Card, Badge, Spinner, ErrorNote, Empty, Stat } from '../components/ui'
import { fmtDate } from '../format'

// Certuvo — the study & practice engine (Phase 8). Reuses the practice pool via /api/me/certuvo/*.
// Formative only: immediate feedback + explanations; the real credential is still earned on the exam.

interface Domain { domain: string; count: number }
interface Recent { mode: string; domain: string | null; score: number; total: number; pct: number; completed_at: string }
interface Overview {
  enabled: boolean; total_questions: number; pass_mark: number
  domains: Domain[]
  readiness: { attempts: number; overall_pct: number; label: string; per_domain: { domain: string; answered: number; pct: number }[] }
  recent: Recent[]
}
interface Question { id: number; question: string; options: string[]; domain: string | null }
interface Session { attempt_id: number; mode: string; domain: string | null; count: number; duration_minutes: number | null; questions: Question[] }
interface ResultItem { id: number; question: string; options: string[]; domain: string; chosen: number | null; correct_index: number; is_correct: boolean; explanation: string | null }
interface SubmitResult {
  score: number; total: number; pct: number; mode: string
  domain_breakdown: { domain: string; correct: number; total: number; pct: number }[]
  results: ResultItem[]
}

const tone = (pct: number, pass: number): 'ok' | 'warn' | 'err' => (pct >= pass + 10 ? 'ok' : pct >= pass ? 'warn' : 'err')

// The member's external Certuvo practice-platform account (credentials shared automatically after membership).
interface CertuvoAccess {
  enabled: boolean
  status?: string
  /** friendly student-facing copy set whenever the account is not active — never a raw error */
  message?: string | null
  username?: string | null
  password?: string | null
  must_change_password?: boolean
  notice?: string | null
  login_url?: string | null
  provisioned_at?: string | null
  activated_at?: string | null
  credentials_sent_at?: string | null
  expires?: string | null
}
function CertuvoAccessPanel() {
  const { data } = useQuery<CertuvoAccess>('/api/me/certuvo/access')
  const [resending, setResending] = useState(false)
  const [resendNote, setResendNote] = useState<{ ok: boolean; text: string } | null>(null)
  if (!data || !data.enabled) return null
  const status = data.status ?? ''
  const active = status === 'active'
  const badge = active
    ? <Badge tone="ok">Ready</Badge>
    : status === 'suspended' || status === 'revoked'
      ? <Badge tone="warn">Unavailable</Badge>
      : <Badge tone="warn">{status === 'not_provisioned' ? 'After membership' : 'Being set up'}</Badge>

  async function resend() {
    setResending(true)
    setResendNote(null)
    try {
      await api.post('/api/me/certuvo/resend', {})
      setResendNote({ ok: true, text: 'Access instructions sent to your email.' })
    } catch (e) {
      const body = e instanceof ApiError && e.body && typeof e.body === 'object' ? (e.body as Record<string, unknown>) : null
      setResendNote({
        ok: false,
        text: e instanceof ApiError && e.status === 429
          ? 'Please wait a while before resending.'
          : (typeof body?.message === 'string' && body.message) || 'Could not resend the instructions. Please try again later.',
      })
    } finally {
      setResending(false)
    }
  }

  return (
    <Card title="Your Certuvo practice access" action={badge}>
      {active ? (
        <div style={{ display: 'grid', gap: '.4rem' }}>
          <p className="muted small" style={{ margin: 0 }}>Practice on the Certuvo platform with the account we created for you. Your progress there is separate from this portal.</p>
          <div className="small">Username: <strong>{data.username}</strong></div>
          {data.password && <div className="small">Temporary password: <strong>{data.password}</strong></div>}
          {data.must_change_password && <div className="small muted">You'll be asked to set your own password the first time you sign in to Certuvo.</div>}
          {data.provisioned_at && <div className="small muted">Access granted {fmtDate(data.provisioned_at)}</div>}
          {data.expires && <div className="small muted">Membership valid until {fmtDate(data.expires)}</div>}
          <div className="row" style={{ marginTop: '.4rem', flexWrap: 'wrap' }}>
            {data.login_url && <a className="btn sm" href={data.login_url} target="_blank" rel="noreferrer">Open Certuvo ↗</a>}
            <button className="btn sm secondary" disabled={resending} onClick={resend}>{resending ? 'Sending…' : 'Resend access instructions'}</button>
          </div>
          {resendNote && <div className={'small' + (resendNote.ok ? ' muted' : '')} style={resendNote.ok ? undefined : { color: 'var(--err, #c2410c)' }} role="status">{resendNote.text}</div>}
        </div>
      ) : data.message ? (
        <p className="muted small" style={{ margin: 0 }}>{data.message}</p>
      ) : (
        <p className="muted small" style={{ margin: 0 }}>Your Certuvo practice account is set up automatically once your membership is active. It will appear here shortly after payment.</p>
      )}
      {/* Mandated notice: PCI shows only the access card; all practice lives in Certuvo. */}
      <p className="muted small" style={{ margin: '.6rem 0 0', borderTop: '1px solid var(--border,#e5e7eb)', paddingTop: '.5rem' }}>
        {data.notice ?? 'Certuvo is an external practice platform. All practice questions, mock examinations, study tools, AI coaching, progress tracking and learning activities are available directly within Certuvo.'}
      </p>
    </Card>
  )
}

export default function Certuvo() {
  const { data, loading, error, refetch } = useQuery<Overview>('/api/me/certuvo/overview')
  const [session, setSession] = useState<Session | null>(null)
  const [answers, setAnswers] = useState<Record<number, number>>({})
  const [result, setResult] = useState<SubmitResult | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  async function start(mode: 'quiz' | 'mock', domain?: string) {
    setBusy(true); setErr(null); setResult(null)
    try {
      const s = await api.post<Session>('/api/me/certuvo/start', { mode, domain })
      setSession(s); setAnswers({})
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not start practice.') } finally { setBusy(false) }
  }
  async function submit() {
    if (!session) return
    setBusy(true); setErr(null)
    try {
      const r = await api.post<SubmitResult>('/api/me/certuvo/submit', { attempt_id: session.attempt_id, answers })
      setResult(r); setSession(null); refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not submit.') } finally { setBusy(false) }
  }
  function reset() { setResult(null); setSession(null); setAnswers({}); refetch() }

  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (data && !data.enabled) return <Card><Empty>Practice is currently unavailable.</Empty></Card>

  // ---------- results ----------
  if (result) {
    return (
      <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <div className="spread" style={{ flexWrap: 'wrap', gap: '.6rem' }}>
          <div><h1>Your results</h1><p className="muted">{result.mode === 'mock' ? 'Full-length mock' : 'Practice quiz'} · {result.score}/{result.total} correct</p></div>
          <button className="btn" onClick={reset}>Back to Certuvo</button>
        </div>
        <div style={{ display: 'grid', gap: '.8rem', gridTemplateColumns: 'repeat(auto-fit,minmax(150px,1fr))' }}>
          <Stat n={result.pct + '%'} k="Score" />
          <Stat n={`${result.score}/${result.total}`} k="Correct" />
          <Stat n={<Badge tone={tone(result.pct, data?.pass_mark ?? 65)}>{result.pct >= (data?.pass_mark ?? 65) ? 'On track' : 'Keep practising'}</Badge>} k={`Pass mark ${data?.pass_mark ?? 65}%`} />
        </div>
        {result.domain_breakdown.length > 1 && (
          <Card title="By domain">
            <table className="data"><thead><tr><th>Domain</th><th>Correct</th><th>Score</th></tr></thead>
              <tbody>{result.domain_breakdown.map((d) => (
                <tr key={d.domain}><td className="small">{d.domain}</td><td className="small">{d.correct}/{d.total}</td>
                  <td><Badge tone={tone(d.pct, data?.pass_mark ?? 65)}>{d.pct}%</Badge></td></tr>
              ))}</tbody></table>
          </Card>
        )}
        <Card title="Review">
          <div style={{ display: 'grid', gap: '1rem' }}>
            {result.results.map((q, i) => (
              <div key={q.id} style={{ borderTop: i ? '1px solid var(--line,#e2e8f0)' : 'none', paddingTop: i ? '.9rem' : 0 }}>
                <div className="row" style={{ gap: '.5rem', alignItems: 'baseline' }}>
                  <Badge tone={q.is_correct ? 'ok' : 'err'}>{q.is_correct ? 'Correct' : 'Incorrect'}</Badge>
                  <strong style={{ fontSize: '.95rem' }}>{i + 1}. {q.question}</strong>
                </div>
                <ul className="clean" style={{ listStyle: 'none', paddingLeft: 0, margin: '.5rem 0', display: 'grid', gap: '.25rem' }}>
                  {q.options.map((o, idx) => {
                    const isCorrect = idx === q.correct_index
                    const isChosen = idx === q.chosen
                    return (
                      <li key={idx} className="small" style={{ padding: '.35rem .55rem', borderRadius: 8,
                        background: isCorrect ? '#ecfdf5' : isChosen ? '#fef2f2' : 'transparent',
                        border: '1px solid ' + (isCorrect ? '#a7f3d0' : isChosen ? '#fecaca' : 'transparent') }}>
                        {String.fromCharCode(65 + idx)}. {o}
                        {isCorrect && <strong> ✓ correct</strong>}
                        {isChosen && !isCorrect && <span> — your answer</span>}
                      </li>
                    )
                  })}
                </ul>
                {q.explanation && <p className="small muted" style={{ margin: 0 }}><strong>Why:</strong> {q.explanation}</p>}
              </div>
            ))}
          </div>
        </Card>
      </div>
    )
  }

  // ---------- active session ----------
  if (session) {
    const answered = Object.keys(answers).length
    return (
      <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
        <div className="spread" style={{ flexWrap: 'wrap', gap: '.6rem' }}>
          <div>
            <h1>{session.mode === 'mock' ? 'Mock examination' : 'Practice quiz'}</h1>
            <p className="muted">{session.domain || 'Mixed domains'} · {session.count} questions{session.duration_minutes ? ` · suggested ${session.duration_minutes} min` : ' · untimed'}</p>
          </div>
          <button className="btn secondary" onClick={reset}>Cancel</button>
        </div>
        {err && <ErrorNote>{err}</ErrorNote>}
        {session.questions.map((q, i) => (
          <Card key={q.id}>
            <div className="row" style={{ gap: '.4rem', alignItems: 'baseline', marginBottom: '.5rem' }}>
              <strong>{i + 1}.</strong><span>{q.question}</span>
            </div>
            <div style={{ display: 'grid', gap: '.35rem' }}>
              {q.options.map((o, idx) => (
                <label key={idx} className="row" style={{ gap: '.5rem', cursor: 'pointer', padding: '.3rem .4rem', borderRadius: 8,
                  background: answers[q.id] === idx ? 'var(--brand-50,#eff6ff)' : 'transparent' }}>
                  <input type="radio" name={`q${q.id}`} checked={answers[q.id] === idx} onChange={() => setAnswers({ ...answers, [q.id]: idx })} style={{ width: 'auto' }} />
                  <span className="small">{String.fromCharCode(65 + idx)}. {o}</span>
                </label>
              ))}
            </div>
          </Card>
        ))}
        <div className="row" style={{ gap: '.6rem', position: 'sticky', bottom: 0, padding: '.6rem 0' }}>
          <button className="btn" disabled={busy} onClick={submit}>{busy ? 'Submitting…' : `Submit (${answered}/${session.count} answered)`}</button>
        </div>
      </div>
    )
  }

  // ---------- overview ----------
  const r = data?.readiness
  return (
    <div className="stack fade-stagger" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Certuvo</h1>
        <p className="muted">PCI's official study &amp; practice for the PCP-AI — scenario-based practice that mirrors the exam, with instant feedback and explanations. Practice is formative; the credential is still earned on the real examination.</p>
      </div>
      {err && <ErrorNote>{err}</ErrorNote>}

      <CertuvoAccessPanel />

      <div style={{ display: 'grid', gap: '.8rem', gridTemplateColumns: 'repeat(auto-fit,minmax(160px,1fr))' }}>
        <Stat n={<Badge tone={r && r.attempts ? tone(r.overall_pct, data!.pass_mark) : 'neutral'}>{r?.label ?? 'Not started'}</Badge>} k="Readiness" />
        <Stat n={r?.attempts ? r.overall_pct + '%' : '—'} k="Overall score" />
        <Stat n={r?.attempts ?? 0} k="Sessions completed" />
        <Stat n={data?.total_questions ?? 0} k="Practice questions" />
      </div>

      <Card title="Start practising" action={
        <button className="btn" disabled={busy || !(data && data.total_questions)} onClick={() => start('mock')}>
          {busy ? 'Loading…' : 'Start a mock exam'}
        </button>
      }>
        {!(data && data.total_questions) ? <Empty>No practice questions are available yet.</Empty> : (
          <table className="data">
            <thead><tr><th>Domain</th><th>Questions</th><th>Your score</th><th /></tr></thead>
            <tbody>
              {data!.domains.map((d) => {
                const dr = r?.per_domain.find((x) => x.domain === d.domain)
                return (
                  <tr key={d.domain}>
                    <td className="small"><strong>{d.domain}</strong></td>
                    <td className="small">{d.count}</td>
                    <td>{dr && dr.answered ? <Badge tone={tone(dr.pct, data!.pass_mark)}>{dr.pct}%</Badge> : <span className="muted small">—</span>}</td>
                    <td><button className="btn ghost sm" disabled={busy} onClick={() => start('quiz', d.domain)}>Practise</button></td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        )}
      </Card>

      {data && data.recent.length > 0 && (
        <Card title="Recent sessions">
          <table className="data">
            <thead><tr><th>When</th><th>Type</th><th>Focus</th><th>Score</th></tr></thead>
            <tbody>
              {data.recent.map((a, i) => (
                <tr key={i}>
                  <td className="small muted">{(a.completed_at || '').slice(0, 10)}</td>
                  <td className="small">{a.mode === 'mock' ? 'Mock' : 'Quiz'}</td>
                  <td className="small">{a.domain || 'Mixed'}</td>
                  <td><Badge tone={tone(a.pct, data!.pass_mark)}>{a.pct}% ({a.score}/{a.total})</Badge></td>
                </tr>
              ))}
            </tbody>
          </table>
        </Card>
      )}
    </div>
  )
}
