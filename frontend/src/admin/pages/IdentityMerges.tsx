import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, StatusBadge, ErrorNote, Spinner, rowActivate } from '../../components/ui'
import {
  PageHeader,
  Toolbar,
  SearchInput,
  FilterSelect,
  SortHeader,
  EmptyState,
  SkeletonTable,
  Pagination,
  DefList,
} from '../../components/premium'
import { useTableControls } from '../../components/useTableControls'
import { Icon } from '../../components/icons'
import { fmtDateTime } from '../../format'

/* Identity merges — the platform's maker-checker approval workflow, which had a complete
 * backend (Endpoints/AdminIdentity.cs) and no console screen.
 *
 * Merging two accounts is irreversible and moves real certification history, so the backend
 * enforces a TWO-PERSON rule by identity, not by permission: an admin holding both
 * id_merge_request and id_merge_approve still cannot approve a request they raised. That is
 * returned as a 409 `maker_checker`, and this screen states the rule up front rather than
 * letting an operator discover it by being refused.
 *
 * The detail view shows the backend's live per-table counts for BOTH accounts before a decision,
 * because "which of these two is the survivor" is not answerable from names alone. */

interface MergeRow extends Record<string, unknown> {
  id: number
  source_user_id: number
  target_user_id: number
  reason?: string | null
  status: string
  requested_by?: number | null
  requested_at?: string | null
  decided_by?: number | null
  decided_at?: string | null
  decision_note?: string | null
  correlation_id?: string | null
}

interface MergeDetail {
  request: MergeRow
  preview: { source: Record<string, number>; target: Record<string, number> }
}

/** The backend's error keys, in the operator's language. `maker_checker` is the one that will
 *  actually be hit in normal use, so it explains the rule rather than restating the code. */
const ERRORS: Record<string, string> = {
  maker_checker: 'You raised this request, so you cannot approve it. Identity merges need a second admin.',
  already_decided: 'This request has already been decided.',
  not_found: 'This request no longer exists.',
  unknown_user: 'One of the two accounts no longer exists.',
  not_mergeable: 'These accounts cannot be merged in their current state.',
}

function MergeDrawer({ id, onClose, onChanged }: { id: number; onClose: () => void; onChanged: () => void }) {
  const { data, loading, error } = useAdminQuery<{ ok: boolean; merge: MergeDetail }>(`/api/admin/identity/merges/${id}`)
  const [note, setNote] = useState('')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  async function decide(action: 'approve' | 'reject') {
    if (action === 'approve' && !confirm('Approve and execute this merge? Identity merges are irreversible.')) return
    setBusy(true)
    setErr(null)
    try {
      await adminApi.post(`/api/admin/identity/merges/${id}/${action}`, { note: note.trim() || undefined })
      onChanged()
      onClose()
    } catch (e) {
      // Surface the backend's own refusal key in operator language, not a raw error string.
      const raw = e instanceof Error ? e.message : ''
      const key = Object.keys(ERRORS).find((k) => raw.includes(k))
      setErr(key ? ERRORS[key] : raw || 'The decision could not be recorded.')
    } finally {
      setBusy(false)
    }
  }

  const m = data?.merge
  const pending = m?.request.status === 'pending'
  const keys = m ? [...new Set([...Object.keys(m.preview.source), ...Object.keys(m.preview.target)])].sort() : []

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()} role="dialog" aria-modal="true" aria-label="Identity merge request">
        <div className="drawer-head">
          <h2>Merge request #{id}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>

        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !m ? null : (
          <>
            {err && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{err}</div>}

            <Card title="Request" action={<StatusBadge status={m.request.status} />}>
              <DefList
                items={[
                  { label: 'Source account', value: <>#{m.request.source_user_id} <span className="muted small">(absorbed)</span></> },
                  { label: 'Target account', value: <>#{m.request.target_user_id} <span className="muted small">(survives)</span></> },
                  { label: 'Reason', value: m.request.reason || <span className="muted">—</span> },
                  { label: 'Requested', value: <>{fmtDateTime(m.request.requested_at)} {m.request.requested_by ? `by admin #${m.request.requested_by}` : ''}</> },
                  ...(m.request.decided_at
                    ? [{ label: 'Decided', value: <>{fmtDateTime(m.request.decided_at)} {m.request.decided_by ? `by admin #${m.request.decided_by}` : ''}</> }]
                    : []),
                  ...(m.request.decision_note ? [{ label: 'Note', value: m.request.decision_note }] : []),
                  ...(m.request.correlation_id ? [{ label: 'Correlation', value: <code>{m.request.correlation_id}</code> }] : []),
                ]}
              />
            </Card>

            {/* Names alone don't answer "which is the survivor" — the record counts do. */}
            <Card title="What each account holds">
              {keys.length === 0 ? (
                <EmptyState icon="archive" title="No records on either account" detail="Neither account holds data in the merged tables." />
              ) : (
                <div className="table-scroll">
                  <table className="data">
                    <thead>
                      <tr>
                        <th>Records</th>
                        <th className="n">Source #{m.request.source_user_id}</th>
                        <th className="n">Target #{m.request.target_user_id}</th>
                      </tr>
                    </thead>
                    <tbody>
                      {keys.map((k) => (
                        <tr key={k}>
                          <td className="small">{k.replace(/_/g, ' ')}</td>
                          <td className="n small">{m.preview.source[k] ?? 0}</td>
                          <td className="n small">{m.preview.target[k] ?? 0}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </Card>

            {pending && (
              <Card className="rail-warn" title="Decision">
                <p className="muted small" style={{ marginTop: 0 }}>
                  Approving executes the merge immediately and cannot be undone. A second admin is required:
                  whoever raised this request cannot approve it.
                </p>
                <div className="field">
                  <label htmlFor="merge-note">Note <span className="muted small">(recorded against the decision)</span></label>
                  <textarea id="merge-note" rows={2} value={note} onChange={(e) => setNote(e.target.value)} />
                </div>
                <div className="row">
                  <button className="btn" disabled={busy} onClick={() => decide('approve')}>
                    {busy ? 'Working…' : 'Approve and merge'}
                  </button>
                  <button className="btn secondary" disabled={busy} onClick={() => decide('reject')}>Reject</button>
                </div>
              </Card>
            )}
          </>
        )}
      </div>
    </div>
  )
}

export default function IdentityMerges() {
  const { data, loading, error, refetch } = useAdminQuery<{ ok: boolean; rows: MergeRow[] }>('/api/admin/identity/merges')
  const [status, setStatus] = useState('')
  const [selected, setSelected] = useState<number | null>(null)

  const rows = data?.rows ?? []
  const pending = rows.filter((r) => r.status === 'pending').length

  const t = useTableControls(rows, {
    searchKeys: ['id', 'source_user_id', 'target_user_id', 'reason', 'correlation_id'],
    filters: [status ? (r: MergeRow) => r.status === status : null],
    filterKey: status,
    pageSize: 25,
    initialSort: { key: 'id', dir: 'desc' },
  })

  return (
    <div className="page">
      <PageHeader
        eyebrow="Students"
        title="Identity merges"
        subtitle="Requests to merge two accounts into one. Merges are irreversible and move certification history, so each needs a second admin to approve it."
        actions={pending > 0 ? <span className="chip warn">{pending} awaiting a decision</span> : undefined}
      />

      {rows.length > 0 && (
        <Toolbar count={t.total !== rows.length ? `${t.total} of ${rows.length} shown` : `${rows.length} requests`}>
          <SearchInput value={t.query} onChange={t.setQuery} label="Search" placeholder="Search account id, reason or correlation…" />
          <FilterSelect
            label="Status"
            value={status}
            onChange={setStatus}
            allLabel="All statuses"
            options={[
              { value: 'pending', label: 'Pending' },
              { value: 'approved', label: 'Approved' },
              { value: 'rejected', label: 'Rejected' },
            ]}
          />
        </Toolbar>
      )}

      <Card className="flat">
        {loading ? (
          <SkeletonTable rows={6} cols={5} />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : rows.length === 0 ? (
          <EmptyState
            icon="users"
            title="No merge requests"
            detail="When two records turn out to be the same person, a merge request is raised here for a second admin to approve."
          />
        ) : t.total === 0 ? (
          <EmptyState
            icon="search"
            title="No match"
            detail="No request matches the current search and status."
            action={<button className="btn secondary sm" onClick={() => { t.setQuery(''); setStatus('') }}>Clear filters</button>}
          />
        ) : (
          <>
            <div className="table-scroll">
              <table className="data">
                <thead>
                  <tr>
                    <SortHeader {...t.sortProps('id')} onSort={() => t.toggleSort('id')}>Request</SortHeader>
                    <SortHeader {...t.sortProps('source_user_id')} onSort={() => t.toggleSort('source_user_id')}>Source</SortHeader>
                    <SortHeader {...t.sortProps('target_user_id')} onSort={() => t.toggleSort('target_user_id')}>Target</SortHeader>
                    <SortHeader {...t.sortProps('requested_at')} onSort={() => t.toggleSort('requested_at')}>Requested</SortHeader>
                    <SortHeader {...t.sortProps('status')} onSort={() => t.toggleSort('status')}>Status</SortHeader>
                  </tr>
                </thead>
                <tbody>
                  {t.rows.map((r) => (
                    <tr key={r.id} {...rowActivate(() => setSelected(r.id))}>
                      <td><strong>#{r.id}</strong>{r.reason && <div className="small muted">{r.reason}</div>}</td>
                      <td className="small">#{r.source_user_id}</td>
                      <td className="small">#{r.target_user_id}</td>
                      <td className="small muted" style={{ whiteSpace: 'nowrap' }}>{fmtDateTime(r.requested_at)}</td>
                      <td>
                        <StatusBadge status={r.status} />
                        {r.status === 'pending' && <> <Icon name="chevron-right" size={13} /></>}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <Pagination page={t.page} pages={t.pages} total={t.total} pageSize={t.pageSize} onPage={t.setPage} />
          </>
        )}
      </Card>

      {selected !== null && (
        <MergeDrawer id={selected} onClose={() => setSelected(null)} onChanged={refetch} />
      )}
    </div>
  )
}
