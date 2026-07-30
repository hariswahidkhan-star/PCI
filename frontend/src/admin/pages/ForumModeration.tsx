import { useAdminQuery, runMutation } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, Spinner, ErrorNote, Empty, Badge } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtDate } from '../../format'

// Admin moderation for the public community forum. Community flags auto-hide a post at 3 reports; this
// queue surfaces flagged/hidden posts and hidden threads for a decision. Backend: Forum.cs (gated 'content').

interface QueuePost {
  id: number; thread_id: number; author_name?: string | null; body?: string | null
  status: string; flags?: number | null; created_at?: string | null; thread_title?: string | null
}
interface QueueThread {
  id: number; category?: string | null; title?: string | null; author_name?: string | null
  status: string; reply_count?: number | null; created_at?: string | null; last_post_at?: string | null
}

export default function ForumModeration() {
  const { data, loading, error, refetch } = useAdminQuery<{ posts: QueuePost[]; threads: QueueThread[] }>('/api/admin/forum/queue')

  const postAction = (id: number, action: string) =>
    runMutation(async () => { await adminApi.post(`/api/admin/forum/posts/${id}`, { action }); refetch() })
  const threadAction = (id: number, action: string) =>
    runMutation(async () => { await adminApi.post(`/api/admin/forum/threads/${id}`, { action }); refetch() })

  const delPost = (p: QueuePost) => { if (window.confirm('Permanently delete this post?')) postAction(p.id, 'delete') }
  const delThread = (t: QueueThread) => { if (window.confirm(`Permanently delete the thread "${t.title}" and all its posts?`)) threadAction(t.id, 'delete') }

  const postBadge = (p: QueuePost) =>
    p.status === 'hidden' ? <Badge tone="err">Hidden{p.flags ? ` · ${p.flags} flags` : ''}</Badge>
      : p.flags ? <Badge tone="warn">{p.flags} flag{p.flags === 1 ? '' : 's'}</Badge>
        : <StatusBadge status={p.status} />

  return (
    <div className="page">
      <PageHeader
        title="Forum moderation"
        subtitle="Flagged and hidden community posts. Posts auto-hide after 3 community reports; restore, hide or delete them here. The author IP is never shown."
      />

      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : (
        <>
          <Card title="Flagged & hidden posts">
            {!data || data.posts.length === 0 ? (
              <Empty>Nothing in the moderation queue.</Empty>
            ) : (
              <table className="data">
                <thead><tr><th>Thread</th><th>Author</th><th>Post</th><th>Submitted</th><th>Status</th><th></th></tr></thead>
                <tbody>
                  {data.posts.map((p) => (
                    <tr key={p.id}>
                      <td className="small">{p.thread_title}</td>
                      <td className="small">{p.author_name}</td>
                      <td className="small" style={{ maxWidth: 320, whiteSpace: 'pre-wrap' }}>{p.body}</td>
                      <td className="small">{fmtDate(p.created_at)}</td>
                      <td>{postBadge(p)}</td>
                      <td>
                        <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                          {p.status === 'hidden'
                            ? <button className="btn sm secondary" onClick={() => postAction(p.id, 'restore')}>Restore</button>
                            : <button className="btn sm secondary" onClick={() => postAction(p.id, 'hide')}>Hide</button>}
                          <button className="btn sm danger" onClick={() => delPost(p)}>Delete</button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </Card>

          <Card title="Hidden threads">
            {!data || data.threads.length === 0 ? (
              <Empty>No hidden threads.</Empty>
            ) : (
              <table className="data">
                <thead><tr><th>Title</th><th>Category</th><th>Author</th><th>Replies</th><th>Created</th><th></th></tr></thead>
                <tbody>
                  {data.threads.map((t) => (
                    <tr key={t.id}>
                      <td className="small">{t.title}</td>
                      <td className="small">{t.category}</td>
                      <td className="small">{t.author_name}</td>
                      <td className="small">{t.reply_count ?? 0}</td>
                      <td className="small">{fmtDate(t.created_at)}</td>
                      <td>
                        <div className="row" style={{ gap: '.35rem', flexWrap: 'wrap', justifyContent: 'flex-end' }}>
                          <button className="btn sm secondary" onClick={() => threadAction(t.id, 'restore')}>Restore</button>
                          <button className="btn sm danger" onClick={() => delThread(t)}>Delete</button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </Card>
        </>
      )}
    </div>
  )
}
