import { useEffect, useRef, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { api } from '../api/client'
import { useMe } from '../data/MeContext'
import { useT } from '../i18n'
import { fmtDateTime } from '../format'
import { Icon } from './icons'
import type { Message } from '../api/types'

/* The student portal's notification centre: a topbar bell carrying the unread count, and a
   popover with the most recent notifications.

   Two deliberate choices:
   - The badge count comes from the `me` payload the shell already loads, so the bell costs
     no extra request on page load.
   - The list is fetched only when the popover is first opened (and refreshed on each open),
     so a student who never opens it never pays for it.

   Notifications carry an optional cta_label/cta_route pair written by the backend
   (Support, Documents, Exam Exception, Founding, …). Following one marks it read and lands
   the student on the page that resolves it — which is the whole point of the notification. */

const PREVIEW = 6

function str(m: Message, keys: string[]): string | null {
  for (const k of keys) {
    const v = m[k]
    if (typeof v === 'string' && v.trim()) return v
  }
  return null
}

export default function NotificationBell() {
  const t = useT()
  const nav = useNavigate()
  const { me, refetch: refetchMe } = useMe()
  const unread = me?.unread ?? 0
  const [open, setOpen] = useState(false)
  const [rows, setRows] = useState<Message[] | null>(null)
  const [busy, setBusy] = useState(false)
  const wrapRef = useRef<HTMLDivElement>(null)
  const btnRef = useRef<HTMLButtonElement>(null)

  // Load (and refresh) the preview each time the popover opens.
  useEffect(() => {
    if (!open) return
    let cancelled = false
    setBusy(true)
    api
      .get<{ rows: Message[] }>('/api/me/messages')
      .then((r) => { if (!cancelled) setRows(r.rows ?? []) })
      .catch(() => { if (!cancelled) setRows([]) })
      .finally(() => { if (!cancelled) setBusy(false) })
    return () => { cancelled = true }
  }, [open])

  // Dismiss on an outside click or Escape, returning focus to the bell.
  useEffect(() => {
    if (!open) return
    const onDown = (e: MouseEvent) => {
      if (!wrapRef.current?.contains(e.target as Node)) setOpen(false)
    }
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        setOpen(false)
        btnRef.current?.focus()
      }
    }
    document.addEventListener('mousedown', onDown)
    document.addEventListener('keydown', onKey)
    return () => {
      document.removeEventListener('mousedown', onDown)
      document.removeEventListener('keydown', onKey)
    }
  }, [open])

  async function markRead(id: number) {
    try {
      await api.post(`/api/me/messages/${id}/read`)
      setRows((prev) => prev?.map((m) => (m.id === id ? { ...m, read_at: new Date().toISOString() } : m)) ?? prev)
      refetchMe()
    } catch { /* a failed read-receipt must never block the student's navigation */ }
  }

  async function markAll() {
    try {
      await api.post('/api/me/messages/read-all')
      setRows((prev) => prev?.map((m) => (m.read_at ? m : { ...m, read_at: new Date().toISOString() })) ?? prev)
      refetchMe()
    } catch { /* ignore — the Messages page offers the same control */ }
  }

  function follow(m: Message) {
    const route = str(m, ['cta_route'])
    if (!m.read_at) void markRead(m.id)
    setOpen(false)
    if (route) nav(route)
  }

  const preview = (rows ?? []).slice(0, PREVIEW)

  return (
    <div className="notif-wrap" ref={wrapRef}>
      <button
        ref={btnRef}
        type="button"
        className="notif-btn"
        aria-expanded={open}
        aria-haspopup="dialog"
        aria-label={unread > 0 ? t('notif.ariaWithCount', { n: unread }) : t('notif.aria')}
        onClick={() => setOpen((o) => !o)}
      >
        <Icon name="bell" size={17} />
        {unread > 0 && <span className="notif-dot">{unread > 99 ? '99+' : unread}</span>}
      </button>

      {open && (
        <div className="notif-pop" role="dialog" aria-label={t('notif.title')}>
          <div className="notif-pop-head">
            <strong>{t('notif.title')}</strong>
            {unread > 0 && (
              <button className="btn ghost sm" onClick={markAll}>{t('msg.markAllRead')}</button>
            )}
          </div>

          <div className="notif-list">
            {busy && rows === null ? (
              <p className="muted small" style={{ padding: '.8rem', margin: 0 }}>{t('notif.loading')}</p>
            ) : preview.length === 0 ? (
              <p className="muted small" style={{ padding: '.8rem', margin: 0 }}>{t('msg.empty')}</p>
            ) : (
              preview.map((m) => {
                const title = str(m, ['title', 'subject', 'heading']) || t('msg.notification')
                const body = str(m, ['body', 'message', 'content', 'text'])
                const when = str(m, ['created_at', 'sent_at'])
                const route = str(m, ['cta_route'])
                const unreadRow = !m.read_at
                // Rows that lead somewhere (or are unread and can be dismissed) are buttons;
                // a read row with no destination is inert text, not a fake control.
                const interactive = !!route || unreadRow
                const content = (
                  <>
                    <span className="notif-item-top">
                      {unreadRow && <span className="chip brand">{t('msg.new')}</span>}
                      <span className="notif-item-title">{title}</span>
                      {when && <span className="notif-item-when">{fmtDateTime(when)}</span>}
                    </span>
                    {body && <span className="notif-item-body">{body}</span>}
                  </>
                )
                return interactive ? (
                  <button
                    key={m.id}
                    type="button"
                    className={'notif-item' + (unreadRow ? ' unread' : '')}
                    onClick={() => follow(m)}
                  >
                    {content}
                  </button>
                ) : (
                  <div key={m.id} className="notif-item">{content}</div>
                )
              })
            )}
          </div>

          <div className="notif-pop-foot">
            <Link to="/messages" onClick={() => setOpen(false)}>{t('notif.viewAll')}</Link>
          </div>
        </div>
      )}
    </div>
  )
}
