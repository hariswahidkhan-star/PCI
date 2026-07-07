import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type Review } from '../api'
import { Card, Badge, StatusBadge, Stat, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'

interface ReviewsResp {
  reviews: Review[]
  counts: { pending: number; published: number; featured: number }
}

export default function Reviews() {
  const [status, setStatus] = useState('')
  const { data, loading, error, refetch } = useAdminQuery<ReviewsResp>(`/api/admin/reviews${status ? '?status=' + status : ''}`)

  async function setReviewStatus(id: number, s: string) {
    await adminApi.post(`/api/admin/reviews/${id}/status`, { status: s })
    refetch()
  }
  async function toggleFeatured(r: Review) {
    await adminApi.patch(`/api/admin/reviews/${r.id}`, { featured: r.featured ? 0 : 1 })
    refetch()
  }
  async function remove(id: number) {
    if (!confirm('Delete this review permanently?')) return
    await adminApi.del(`/api/admin/reviews/${id}`)
    refetch()
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <h1>Reviews</h1>

      {data && (
        <div className="grid cols-3">
          <Card><Stat n={data.counts.pending} k="Pending" /></Card>
          <Card><Stat n={data.counts.published} k="Published" /></Card>
          <Card><Stat n={data.counts.featured} k="Featured" /></Card>
        </div>
      )}

      <Card>
        <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }}>
          <option value="">All statuses</option>
          <option value="pending">Pending</option>
          <option value="published">Published</option>
          <option value="rejected">Rejected</option>
        </select>
      </Card>

      {loading ? (
        <Spinner />
      ) : error ? (
        <ErrorNote>{error}</ErrorNote>
      ) : !data || data.reviews.length === 0 ? (
        <Card><Empty>No reviews match.</Empty></Card>
      ) : (
        data.reviews.map((r) => (
          <Card
            key={r.id}
            title={<span>{r.title || 'Review'} {r.featured ? <Badge tone="brand">featured</Badge> : null}</span>}
            action={<StatusBadge status={r.status} />}
          >
            <div className="small muted">{r.name}{r.designation ? `, ${r.designation}` : ''}{r.company ? ` · ${r.company}` : ''}{r.country ? ` · ${r.country}` : ''} · {fmtDate(r.created_at)}{r.rating ? ` · ${r.rating}★` : ''}</div>
            {r.body && <p style={{ marginTop: '.5rem', whiteSpace: 'pre-wrap' }}>{r.body}</p>}
            <div className="row" style={{ marginTop: '.5rem', flexWrap: 'wrap' }}>
              {r.status !== 'published' && <button className="btn sm" onClick={() => setReviewStatus(r.id, 'published')}>Publish</button>}
              {r.status !== 'rejected' && <button className="btn sm secondary" onClick={() => setReviewStatus(r.id, 'rejected')}>Reject</button>}
              {r.status === 'published' && <button className="btn sm secondary" onClick={() => toggleFeatured(r)}>{r.featured ? 'Unfeature' : 'Feature'}</button>}
              <button className="btn ghost sm" onClick={() => remove(r.id)}>Delete</button>
            </div>
          </Card>
        ))
      )}
    </div>
  )
}
