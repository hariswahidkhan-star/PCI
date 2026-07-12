import { useQuery, runMutation } from '../api/hooks'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote, Empty, Badge } from '../components/ui'
import { fmtDateTime } from '../format'
import { useT } from '../i18n'
import type { Message } from '../api/types'

function field(m: Message, keys: string[]): string | null {
  for (const k of keys) {
    const v = m[k]
    if (typeof v === 'string' && v.trim()) return v
  }
  return null
}

export default function Messages() {
  const t = useT()
  const { refetch: refetchMe } = useMe()
  const { data, loading, error, refetch } = useQuery<{ rows: Message[] }>('/api/me/messages')

  const markRead = (id: number) =>
    runMutation(async () => { await api.post(`/api/me/messages/${id}/read`); refetch(); refetchMe() })
  const markAll = () =>
    runMutation(async () => { await api.post('/api/me/messages/read-all'); refetch(); refetchMe() })

  const rows = data?.rows ?? []
  const unread = rows.filter((m) => !m.read_at).length

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <div>
          <h1>{t('msg.title')}</h1>
          <p className="muted" style={{ margin: 0 }}>{t('msg.subtitle')}</p>
        </div>
        {unread > 0 && <button className="btn secondary sm" onClick={markAll}>{t('msg.markAllRead')}</button>}
      </div>

      {loading ? (
        <Spinner />
      ) : error ? (
        <ErrorNote>{error}</ErrorNote>
      ) : rows.length === 0 ? (
        <Card><Empty>{t('msg.empty')}</Empty></Card>
      ) : (
        rows.map((m) => {
          const title = field(m, ['title', 'subject', 'heading']) || t('msg.notification')
          const body = field(m, ['body', 'message', 'content', 'text'])
          const when = field(m, ['created_at', 'sent_at'])
          return (
            <Card
              key={m.id}
              title={<span>{title} {!m.read_at && <Badge tone="brand">{t('msg.new')}</Badge>}</span>}
              action={when ? <span className="muted small">{fmtDateTime(when)}</span> : undefined}
            >
              {body && <p style={{ margin: 0 }}>{body}</p>}
              {!m.read_at && (
                <div style={{ marginTop: '.6rem' }}>
                  <button className="btn ghost sm" onClick={() => markRead(m.id)}>{t('msg.markAsRead')}</button>
                </div>
              )}
            </Card>
          )
        })
      )}
    </div>
  )
}
