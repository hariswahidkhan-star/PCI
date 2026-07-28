import { useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { useQuery, runMutation } from '../api/hooks'
import { useMe } from '../data/MeContext'
import { api } from '../api/client'
import { Card, Spinner, ErrorNote, Badge } from '../components/ui'
import { PageHeader, Toolbar, SearchInput, Segmented, EmptyState } from '../components/premium'
import { Icon } from '../components/icons'
import { fmtDateTime, titleCase } from '../format'
import { useT } from '../i18n'
import type { Message } from '../api/types'

/* The student's notification inbox.

   The backend writes notifications with an optional cta_label/cta_route pair (Support replies,
   new documents, exam-exception updates, founding access, sponsorship…) — the action that
   resolves the notification. This page renders that action, filters by read state and category,
   and searches title/body, so an inbox that has accumulated a year of activity stays usable. */

function field(m: Message, keys: string[]): string | null {
  for (const k of keys) {
    const v = m[k]
    if (typeof v === 'string' && v.trim()) return v
  }
  return null
}

type ReadFilter = 'all' | 'unread'

export default function Messages() {
  const t = useT()
  const { refetch: refetchMe } = useMe()
  const { data, loading, error, refetch } = useQuery<{ rows: Message[] }>('/api/me/messages')
  const [readFilter, setReadFilter] = useState<ReadFilter>('all')
  const [category, setCategory] = useState('')
  const [query, setQuery] = useState('')

  const markRead = (id: number) =>
    runMutation(async () => { await api.post(`/api/me/messages/${id}/read`); refetch(); refetchMe() })
  const markAll = () =>
    runMutation(async () => { await api.post('/api/me/messages/read-all'); refetch(); refetchMe() })

  const rows = useMemo(() => data?.rows ?? [], [data])
  const unread = rows.filter((m) => !m.read_at).length

  // Categories are whatever the backend has actually written for this student — never a
  // hardcoded list that could drift from the notifications being produced.
  const categories = useMemo(() => {
    const seen = new Set<string>()
    for (const m of rows) {
      const c = field(m, ['category'])
      if (c) seen.add(c)
    }
    return [...seen].sort()
  }, [rows])

  const shown = useMemo(() => {
    const needle = query.trim().toLowerCase()
    return rows.filter((m) => {
      if (readFilter === 'unread' && m.read_at) return false
      if (category && field(m, ['category']) !== category) return false
      if (!needle) return true
      const hay = [field(m, ['title', 'subject', 'heading']), field(m, ['body', 'message', 'content', 'text'])]
        .filter(Boolean)
        .join(' ')
        .toLowerCase()
      return hay.includes(needle)
    })
  }, [rows, readFilter, category, query])

  const filtering = readFilter !== 'all' || !!category || !!query.trim()

  return (
    <div className="page">
      <PageHeader
        eyebrow={t('nav.messages')}
        title={t('msg.title')}
        subtitle={t('msg.subtitle')}
        actions={unread > 0 ? <button className="btn secondary sm" onClick={markAll}>{t('msg.markAllRead')}</button> : undefined}
      />

      {rows.length > 0 && (
        <Toolbar count={filtering ? t('msg.countFiltered', { n: shown.length, total: rows.length }) : t('msg.count', { n: rows.length })}>
          <SearchInput value={query} onChange={setQuery} label={t('msg.search')} placeholder={t('msg.searchPlaceholder')} />
          <Segmented<ReadFilter>
            value={readFilter}
            onChange={setReadFilter}
            label={t('msg.filterByState')}
            options={[
              { value: 'all', label: t('msg.filterAll'), n: rows.length },
              { value: 'unread', label: t('msg.filterUnread'), n: unread },
            ]}
          />
          {categories.length > 1 && (
            <div className="filter-field">
              <label htmlFor="msg-cat">{t('msg.category')}</label>
              <select id="msg-cat" value={category} onChange={(e) => setCategory(e.target.value)}>
                <option value="">{t('msg.allCategories')}</option>
                {categories.map((c) => (
                  <option key={c} value={c}>{titleCase(c)}</option>
                ))}
              </select>
            </div>
          )}
        </Toolbar>
      )}

      {loading ? (
        <Spinner />
      ) : error ? (
        <ErrorNote>{error}</ErrorNote>
      ) : rows.length === 0 ? (
        <Card className="flat">
          <EmptyState icon="bell" title={t('msg.empty')} detail={t('msg.emptyDetail')} />
        </Card>
      ) : shown.length === 0 ? (
        <Card className="flat">
          <EmptyState
            icon="search"
            title={t('msg.noMatch')}
            detail={t('msg.noMatchDetail')}
            action={
              <button
                className="btn secondary sm"
                onClick={() => { setQuery(''); setCategory(''); setReadFilter('all') }}
              >
                {t('msg.clearFilters')}
              </button>
            }
          />
        </Card>
      ) : (
        <div className="stack" style={{ display: 'grid', gap: '.6rem' }}>
          {shown.map((m) => {
            const title = field(m, ['title', 'subject', 'heading']) || t('msg.notification')
            const body = field(m, ['body', 'message', 'content', 'text'])
            const when = field(m, ['created_at', 'sent_at'])
            const cat = field(m, ['category'])
            const ctaLabel = field(m, ['cta_label'])
            // Only in-portal routes are followed. cta_route is written by the backend, but
            // routing a non-"/" value through <Link> would resolve it relative to the current
            // page and land the student somewhere meaningless.
            const rawRoute = field(m, ['cta_route'])
            const ctaRoute = rawRoute && rawRoute.startsWith('/') ? rawRoute : null
            const isUnread = !m.read_at
            return (
              <article key={m.id} className={'msg-row' + (isUnread ? ' unread' : '')}>
                <div className="msg-row-top">
                  {isUnread && <Badge tone="brand">{t('msg.new')}</Badge>}
                  {cat && <span className="chip">{titleCase(cat)}</span>}
                  <h2 className="msg-row-title">{title}</h2>
                  {when && <span className="msg-row-when">{fmtDateTime(when)}</span>}
                </div>
                {body && <p className="msg-row-body">{body}</p>}
                {(ctaRoute || isUnread) && (
                  <div className="msg-row-actions">
                    {/* The action the backend attached to this notification — the thing that
                        actually resolves it. Following it also clears the unread flag. */}
                    {ctaRoute && (
                      <Link
                        className="btn sm"
                        to={ctaRoute}
                        onClick={() => { if (isUnread) markRead(m.id) }}
                      >
                        {ctaLabel || t('msg.open')}
                        <Icon name="arrow-right" size={14} />
                      </Link>
                    )}
                    {isUnread && (
                      <button className="btn ghost sm" onClick={() => markRead(m.id)}>{t('msg.markAsRead')}</button>
                    )}
                  </div>
                )}
              </article>
            )
          })}
        </div>
      )}
    </div>
  )
}
