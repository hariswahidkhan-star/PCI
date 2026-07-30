import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Spinner, ErrorNote } from '../../components/ui'
import { PageHeader } from '../../components/premium'

/**
 * PCI World → Launch. The one screen that turns each World feature on.
 *
 * Everything here is a mirror of the server. `can_enable`, the prerequisite state and the refusals
 * all come from /api/admin/world-launch; this component never decides that a switch should be
 * disabled, because a client-side rule is a suggestion — the gate lives in the POST, and the same
 * refusal comes back whether the request came from this page or from curl.
 *
 * Two things are deliberately not one click. Recording a prerequisite asks for a note, because the
 * record exists to be read by somebody later and "yes" tells them nothing. And a feature that owes
 * something to a third party cannot be switched on in the same gesture as recording its
 * prerequisite — you record it, read what you recorded, then switch on.
 */

interface Recorded { by: string | null; at: string | null; note: string | null }
interface Requires { key: string; label: string; detail: string; recorded: Recorded | null }
interface Feature {
  flag: string
  title: string
  turns_on: string
  url: string
  when_off: string
  on: boolean
  advisory: string | null
  depends_on: string | null
  depends_on_met: boolean
  depends_on_title: string | null
  requires: Requires | null
  can_enable: boolean
}

function AckForm({ req, onDone }: { req: Requires; onDone: () => void }) {
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  async function record() {
    setBusy(true); setErr(null)
    try {
      await adminApi.post('/api/admin/world-launch/ack', { key: req.key, note: note.trim() })
      setNote('')
      onDone()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not record it.')
    } finally { setBusy(false) }
  }

  async function withdraw() {
    setBusy(true); setErr(null)
    try {
      await adminApi.post('/api/admin/world-launch/ack', { key: req.key, withdraw: true })
      onDone()
    } catch (e) {
      setErr(e instanceof Error ? e.message : 'Could not withdraw it.')
    } finally { setBusy(false) }
  }

  if (req.recorded) {
    return (
      <div className="notice" style={{ marginTop: '.6rem' }}>
        <strong>{req.label}</strong>
        <p className="small" style={{ margin: '.3rem 0' }}>
          Recorded{req.recorded.by ? ` by ${req.recorded.by}` : ''}{req.recorded.at ? ` on ${req.recorded.at} UTC` : ''}.
        </p>
        {req.recorded.note && <p className="small" style={{ margin: '.3rem 0', fontStyle: 'italic' }}>“{req.recorded.note}”</p>}
        {err && <div className="notice err" role="status" style={{ marginBottom: '.4rem' }}>{err}</div>}
        <button className="btn ghost sm" disabled={busy} onClick={withdraw}>
          {busy ? 'Working…' : 'Withdraw'}
        </button>
        <span className="muted small" style={{ marginLeft: '.5rem' }}>
          Withdrawing locks the feature again. Both the record and the withdrawal stay in the audit log.
        </span>
      </div>
    )
  }

  return (
    <div className="notice err" style={{ marginTop: '.6rem' }}>
      <strong>Before this can be switched on: {req.label}</strong>
      <p className="small" style={{ margin: '.3rem 0' }}>{req.detail}</p>
      {err && <div className="notice err" role="status" style={{ marginBottom: '.4rem' }}>{err}</div>}
      <div className="field">
        <label htmlFor={`ack-${req.key}`}>What was done, and where can it be checked?</label>
        <textarea
          id={`ack-${req.key}`}
          rows={2}
          value={note}
          placeholder="A contract reference, a URL, or the name of whoever signed it off."
          onChange={(e) => setNote(e.target.value)}
        />
      </div>
      <button className="btn sm" disabled={busy || note.trim().length < 8} onClick={record}>
        {busy ? 'Recording…' : 'Record it'}
      </button>
      <span className="muted small" style={{ marginLeft: '.5rem' }}>
        Your name and the time are recorded with it.
      </span>
    </div>
  )
}

function FeatureCard({ f, onChanged }: { f: Feature; onChanged: (msg?: string) => void }) {
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  async function toggle() {
    setBusy(true); setErr(null)
    try {
      const r = await adminApi.post<{ on: boolean; orphaned?: string[] }>(
        '/api/admin/world-launch', { flag: f.flag, on: !f.on })
      const orphaned = r.orphaned?.length
        ? ` ${r.orphaned.join(' and ')} ${r.orphaned.length > 1 ? 'are' : 'is'} still switched on but has nothing to attach to — switch ${r.orphaned.length > 1 ? 'them' : 'it'} off too, or turn this back on.`
        : ''
      onChanged(`${f.title} is ${r.on ? 'live' : 'off'}.${orphaned}`)
    } catch (e) {
      // The server's refusals are written to be read by a person; show them as they are rather
      // than replacing them with a generic failure.
      setErr(e instanceof Error ? e.message : 'Could not change it.')
      setBusy(false)
      return
    }
    setBusy(false)
  }

  const blockedByDependency = !f.on && !f.depends_on_met

  return (
    <Card title={f.title}>
      <div className="row" style={{ justifyContent: 'space-between', alignItems: 'flex-start', gap: '1rem', flexWrap: 'wrap' }}>
        <div style={{ flex: 1, minWidth: 260 }}>
          <p className="small" style={{ marginTop: 0 }}>{f.turns_on}</p>
          <p className="muted small" style={{ margin: 0 }}>
            {f.on
              ? <>Live at <a href={f.url}>{f.url}</a></>
              : <>Will be served at <code>{f.url}</code>. {f.when_off}</>}
          </p>
        </div>
        <div style={{ textAlign: 'right' }}>
          <div style={{ marginBottom: '.4rem' }}>
            <span className={'badge ' + (f.on ? 'ok' : '')}>{f.on ? 'Live' : 'Off'}</span>
          </div>
          <button
            className={'btn sm' + (f.on ? ' ghost' : '')}
            disabled={busy || (!f.on && !f.can_enable)}
            onClick={toggle}
          >
            {busy ? 'Working…' : f.on ? 'Switch off' : 'Switch on'}
          </button>
        </div>
      </div>

      {err && <div className="notice err" role="status" style={{ marginTop: '.6rem' }}>{err}</div>}

      {blockedByDependency && (
        <div className="notice" style={{ marginTop: '.6rem' }}>
          Needs <strong>{f.depends_on_title}</strong> switched on first — there is nothing for this to attach to without it.
        </div>
      )}

      {f.requires && <AckForm req={f.requires} onDone={() => onChanged()} />}

      {f.advisory && !f.on && (
        <p className="muted small" style={{ marginTop: '.6rem', marginBottom: 0 }}>{f.advisory}</p>
      )}
    </Card>
  )
}

export default function WorldLaunch() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Feature[] }>('/api/admin/world-launch')
  const [msg, setMsg] = useState<string | null>(null)

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  const rows = data?.rows ?? []
  const live = rows.filter((r) => r.on).length

  return (
    <div className="page">
      <PageHeader title="PCI World — Launch" />
      {msg && <div className="notice" role="status">{msg}</div>}
      <p className="muted small">
        Every PCI World feature ships switched off, so a deployment is never a launch by accident.
        {' '}{live} of {rows.length} {live === 1 ? 'is' : 'are'} live. Switching one on takes effect
        immediately — there is no redeploy and no cache to clear.
      </p>
      <p className="muted small">
        Three of these carry an obligation to somebody who is not you: guests whose images are shown
        to others, candidates whose CVs are stored, contributors whose words are published. Those
        will not switch on until the prerequisite below them is recorded.
      </p>

      {rows.map((f) => (
        <FeatureCard
          key={f.flag}
          f={f}
          onChanged={(m) => { setMsg(m ?? null); refetch() }}
        />
      ))}
    </div>
  )
}
