import { useCallback, useEffect, useRef, useState } from 'react'
import * as C from './api'

// PCI World community rooms — the guest participant surface.
//
// ACCESSIBILITY IS BUILT IN HERE, NOT RETROFITTED (§18.1, PW-US-038/039).
//
//   • The transcript is role="log" with aria-live="polite", so a screen reader announces new
//     messages without interrupting what the person is currently doing.
//   • Focus is NEVER moved when a message arrives. Stealing focus mid-sentence is the single most
//     common way a live region makes a chat unusable with assistive technology.
//   • Every state — pending, withheld, ejected, degraded — is conveyed in TEXT, not by colour
//     alone, so it survives both colour blindness and a monochrome display.
//   • The composer keeps the participant's text when a message is withheld. Losing what someone
//     typed because a classifier disagreed is a punishment on top of a moderation decision.
//
// THE SLICE'S SAFETY RULE SHOWS UP IN THE UI AS WELL: a message is rendered as PENDING until the
// server says it published. There is no optimistic echo, because an optimistic echo is exactly the
// thing that would show a participant their own blocked message as though it had been posted.

type View = 'rooms' | 'entry' | 'room' | 'ejected'

const POLL_MS = 2500

export default function CommunityApp() {
  const [view, setView] = useState<View>('rooms')
  const [rooms, setRooms] = useState<C.RoomSummary[]>([])
  const [room, setRoom] = useState<C.RoomDetail | null>(null)
  const [messages, setMessages] = useState<C.Message[]>([])
  const [pending, setPending] = useState<{ id: string; body: string } | null>(null)
  const [notice, setNotice] = useState<{ tone: 'info' | 'withheld' | 'error'; text: string } | null>(null)
  const [ejection, setEjection] = useState<C.SendResult['appeal'] | null>(null)
  const [loading, setLoading] = useState(true)

  const lastSeq = useRef(0)
  const pollTimer = useRef<number | null>(null)

  // ── Room catalogue ──
  const loadRooms = useCallback(async () => {
    setLoading(true)
    try {
      const r = await C.listRooms()
      setRooms(r.rooms)
      setNotice(null)
    } catch (e) {
      setNotice({ tone: 'error', text: e instanceof C.CommunityError ? e.message : 'Rooms are unavailable right now.' })
    } finally { setLoading(false) }
  }, [])

  useEffect(() => { void loadRooms() }, [loadRooms])

  // ── Transcript polling ──
  //
  // Polling rather than the hub, deliberately, for this first release: the durable history is the
  // source of truth and replaying by sequence is correct regardless of transport, so the realtime
  // connection is an optimisation that can be layered on without changing any of this logic.
  const pump = useCallback(async (slug: string) => {
    try {
      const r = await C.since(slug, lastSeq.current)
      if (r.messages.length > 0) {
        lastSeq.current = r.messages[r.messages.length - 1].sequence
        setMessages(prev => [...prev, ...r.messages])
      }
    } catch { /* a transient read failure resolves on the next tick; no need to alarm anyone */ }
  }, [])

  useEffect(() => {
    if (view !== 'room' || !room) return
    void pump(room.slug)
    pollTimer.current = window.setInterval(() => void pump(room.slug), POLL_MS)
    return () => { if (pollTimer.current) window.clearInterval(pollTimer.current) }
  }, [view, room, pump])

  async function enterRoom(slug: string) {
    try {
      const detail = await C.getRoom(slug)
      setRoom(detail)
      setView(C.getRoomToken() ? 'room' : 'entry')
      if (C.getRoomToken()) { lastSeq.current = 0; setMessages([]) }
    } catch (e) {
      setNotice({ tone: 'error', text: e instanceof C.CommunityError ? e.message : 'That room is unavailable.' })
    }
  }

  return (
    <div className="cw">
      <header className="cw-head">
        <h1>PCI World community</h1>
        <p className="cw-sub">Professional rooms for people who control projects.</p>
      </header>

      {notice && (
        // role="status" rather than "alert": these are outcomes, not emergencies, and an assertive
        // region would interrupt a screen reader mid-word.
        <p role="status" className={`cw-notice cw-notice-${notice.tone}`}>{notice.text}</p>
      )}

      {view === 'rooms' && (
        <RoomList rooms={rooms} loading={loading} onEnter={enterRoom} onRefresh={() => void loadRooms()} />
      )}

      {view === 'entry' && room && (
        <EntryForm
          room={room}
          onJoined={() => { lastSeq.current = 0; setMessages([]); setView('room') }}
          onCancel={() => setView('rooms')}
        />
      )}

      {view === 'room' && room && (
        <Room
          room={room}
          messages={messages}
          pending={pending}
          setPending={setPending}
          onWithheld={t => setNotice({ tone: 'withheld', text: t })}
          onEjected={appeal => { setEjection(appeal); setView('ejected'); C.clearRoomToken() }}
          onLeave={async () => {
            try { await C.leave() } catch { /* leaving is best-effort; the session expires anyway */ }
            C.clearRoomToken(); setView('rooms'); setMessages([]); lastSeq.current = 0
            void loadRooms()
          }}
        />
      )}

      {view === 'ejected' && <Ejected appeal={ejection} onBack={() => { setEjection(null); setView('rooms'); void loadRooms() }} />}
    </div>
  )
}

// ── Room catalogue ──────────────────────────────────────────────────────────────────────────

function RoomList({ rooms, loading, onEnter, onRefresh }: {
  rooms: C.RoomSummary[]; loading: boolean
  onEnter: (slug: string) => void; onRefresh: () => void
}) {
  if (loading) return <p role="status">Loading rooms…</p>
  if (rooms.length === 0) {
    return (
      <div className="cw-empty">
        <h2>No rooms are open right now</h2>
        <p>Community rooms open on a schedule. Please check back later.</p>
        <button type="button" onClick={onRefresh}>Refresh</button>
      </div>
    )
  }
  return (
    <section aria-labelledby="rooms-h">
      <h2 id="rooms-h">Open rooms</h2>
      <ul className="cw-rooms">
        {rooms.map(r => (
          <li key={r.slug} className="cw-room-card">
            <h3>{r.title}</h3>
            {r.description && <p>{r.description}</p>}
            <p className="cw-room-meta">
              {/* Status is spelled out, never signalled by colour alone. */}
              <span>{r.state === 'open' ? 'Open' : r.state === 'slow_mode' ? 'Open — slow mode' : 'Read only'}</span>
              {' · '}
              <span>{r.participants} of {r.capacity} here</span>
              {r.slow_mode_seconds > 0 && <> · <span>one message every {r.slow_mode_seconds}s</span></>}
            </p>
            <button type="button" onClick={() => onEnter(r.slug)}>
              Enter {r.title}
            </button>
          </li>
        ))}
      </ul>
    </section>
  )
}

// ── Guest entry: one screen, per §7.1 ───────────────────────────────────────────────────────

function EntryForm({ room, onJoined, onCancel }: {
  room: C.RoomDetail; onJoined: () => void; onCancel: () => void
}) {
  const [name, setName] = useState('')
  const [accepted, setAccepted] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [suggestion, setSuggestion] = useState<string | null>(null)
  const [busy, setBusy] = useState(false)
  const nameRef = useRef<HTMLInputElement>(null)

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    setError(null); setSuggestion(null); setBusy(true)
    try {
      const r = await C.join(room.slug, name, room.rules_version)
      C.setRoomToken(r.token)
      onJoined()
    } catch (err) {
      if (err instanceof C.CommunityError) {
        setError(err.message)
        setSuggestion(err.suggestion ?? null)
      } else setError('Could not join the room.')
      // Return focus to the field that needs fixing — moving focus TO an error is helpful; moving
      // it away from someone mid-task is not.
      nameRef.current?.focus()
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="entry-h" className="cw-entry">
      <h2 id="entry-h">Join {room.title}</h2>

      <form onSubmit={submit} noValidate>
        <label htmlFor="cw-name">Display name</label>
        <input
          id="cw-name" ref={nameRef} value={name} required maxLength={32}
          onChange={e => setName(e.target.value)}
          aria-describedby={`cw-name-help${error ? ' cw-name-err' : ''}`}
          aria-invalid={error ? true : undefined}
          autoComplete="off"
        />
        <p id="cw-name-help" className="cw-help">
          Shown to everyone in the room. No contact details, and nothing suggesting you work for PCI.
        </p>
        {error && (
          <p id="cw-name-err" role="alert" className="cw-err">
            {error}
            {suggestion && <> Try <button type="button" className="cw-linkish" onClick={() => setName(suggestion)}>{suggestion}</button>.</>}
          </p>
        )}

        <div className="cw-rules">
          <h3>Room rules</h3>
          <ul>
            <li>Be professional. No abuse, harassment or hate.</li>
            <li>No phone numbers, emails, addresses or links.</li>
            <li>Messages are checked before anyone else sees them.</li>
            <li>Moderators may remove messages and end sessions.</li>
          </ul>
          <p className="cw-help">
            This room is for adults working in project controls. Messages are retained under the
            room&rsquo;s <strong>{room.retention_class}</strong> retention policy.
          </p>
        </div>

        <label className="cw-check">
          <input type="checkbox" checked={accepted} onChange={e => setAccepted(e.target.checked)} required />
          <span>I have read and accept the room rules ({room.rules_version}).</span>
        </label>

        <div className="cw-actions">
          <button type="submit" disabled={busy || !accepted || name.trim().length < 2}>
            {busy ? 'Joining…' : 'Join room'}
          </button>
          <button type="button" onClick={onCancel}>Back to rooms</button>
        </div>
      </form>
    </section>
  )
}

// ── The room ────────────────────────────────────────────────────────────────────────────────

function Room({ room, messages, pending, setPending, onWithheld, onEjected, onLeave }: {
  room: C.RoomDetail
  messages: C.Message[]
  pending: { id: string; body: string } | null
  setPending: (p: { id: string; body: string } | null) => void
  onWithheld: (text: string) => void
  onEjected: (appeal: C.SendResult['appeal']) => void
  onLeave: () => void
}) {
  const [draft, setDraft] = useState('')
  const [busy, setBusy] = useState(false)
  const [cooldown, setCooldown] = useState(0)
  const composerRef = useRef<HTMLTextAreaElement>(null)

  useEffect(() => {
    if (cooldown <= 0) return
    const t = window.setTimeout(() => setCooldown(c => c - 1), 1000)
    return () => window.clearTimeout(t)
  }, [cooldown])

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    const body = draft.trim()
    if (!body || busy) return
    const clientId = `${Date.now()}-${Math.random().toString(36).slice(2, 10)}`
    setBusy(true)
    setPending({ id: clientId, body })

    try {
      const r = await C.send(room.slug, body, clientId)
      setPending(null)
      if (r.ejected) { onEjected(r.appeal); return }
      if (r.published) {
        // Cleared only on success. The poll brings the message back with its real sequence, so
        // there is no optimistic copy to reconcile — and nothing a participant could mistake for
        // published when it was not.
        setDraft('')
      } else {
        onWithheld(C.reasonText(r.reason))
        // The text is deliberately KEPT. Someone who was quoting a policy document should not lose
        // what they wrote because a classifier disagreed.
      }
    } catch (err) {
      setPending(null)
      if (err instanceof C.CommunityError) {
        onWithheld(err.message)
        if (err.error === 'slow_mode') setCooldown(room.slow_mode_seconds)
      } else onWithheld('Your message could not be sent.')
    } finally {
      setBusy(false)
      composerRef.current?.focus()
    }
  }

  return (
    <section aria-labelledby="room-h" className="cw-room">
      <div className="cw-room-bar">
        <h2 id="room-h">{room.title}</h2>
        <button type="button" onClick={onLeave}>Leave room</button>
      </div>

      {room.pinned_welcome && <p className="cw-pinned">{room.pinned_welcome}</p>}
      {!room.accepting && (
        <p role="status" className="cw-notice cw-notice-info">
          This room is not accepting messages right now.
        </p>
      )}

      {/* role="log" + polite: new messages are announced without interrupting, and focus stays
          exactly where the participant put it. */}
      <ol className="cw-log" role="log" aria-live="polite" aria-relevant="additions" aria-label={`${room.title} messages`}>
        {messages.map(m => (
          <li key={m.sequence} className="cw-msg">
            <span className="cw-author">{m.author ?? 'Guest'}</span>
            <span className="cw-body">{m.body}</span>
          </li>
        ))}
        {pending && (
          <li className="cw-msg cw-msg-pending" aria-label="Your message is being checked">
            <span className="cw-author">You</span>
            <span className="cw-body">{pending.body}</span>
            <span className="cw-tag">Checking…</span>
          </li>
        )}
      </ol>

      <form onSubmit={submit} className="cw-composer">
        <label htmlFor="cw-draft">Your message</label>
        <textarea
          id="cw-draft" ref={composerRef} value={draft} rows={3} maxLength={2000}
          onChange={e => setDraft(e.target.value)}
          onKeyDown={e => { if (e.key === 'Enter' && !e.shiftKey) { e.preventDefault(); void submit(e) } }}
          aria-describedby="cw-draft-help"
          disabled={!room.accepting}
        />
        <p id="cw-draft-help" className="cw-help">
          Messages are checked before anyone else sees them. Press Enter to send, Shift+Enter for a new line.
          {cooldown > 0 && <> Slow mode: you can send again in {cooldown}s.</>}
        </p>
        <button type="submit" disabled={busy || cooldown > 0 || !room.accepting || draft.trim().length === 0}>
          {busy ? 'Sending…' : 'Send'}
        </button>
      </form>
    </section>
  )
}

// ── Ejection + appeal ───────────────────────────────────────────────────────────────────────

function Ejected({ appeal, onBack }: { appeal: C.SendResult['appeal']; onBack: () => void }) {
  const [copied, setCopied] = useState(false)
  return (
    <section aria-labelledby="ej-h" className="cw-ejected">
      <h2 id="ej-h">Your session has ended</h2>
      <p>A moderator action has ended your session in this room.</p>

      {appeal && (
        <>
          <h3>If you think this was wrong</h3>
          {/* Shown ONCE. If the participant loses this, the appeal is unrecoverable — so it is
              presented prominently, selectable, and copyable rather than buried in prose. */}
          <p className="cw-help">
            Keep these details. They are shown only once and are the only way to appeal.
          </p>
          <dl className="cw-appeal">
            <dt>Reference</dt>
            <dd><code>{appeal.reference}</code></dd>
            <dt>Appeal code</dt>
            <dd><code>{appeal.credential}</code></dd>
            {appeal.restricted_until && (<><dt>Restriction ends</dt><dd>{appeal.restricted_until} UTC</dd></>)}
          </dl>
          <button
            type="button"
            onClick={() => {
              void navigator.clipboard?.writeText(`Reference: ${appeal.reference}\nAppeal code: ${appeal.credential}`)
              setCopied(true)
            }}
          >
            Copy appeal details
          </button>
          {copied && <span role="status"> Copied.</span>}
          <p><a href={appeal.how}>How to appeal</a></p>
        </>
      )}

      <button type="button" onClick={onBack}>Back to rooms</button>
    </section>
  )
}
