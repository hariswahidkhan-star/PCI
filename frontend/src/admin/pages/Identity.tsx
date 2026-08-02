import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import {
  adminApi,
  type IdentityHealth,
  type IdentityBackfillPreview,
  type IdentityBackfillResult,
  type IdentityReconciliation,
  type IdentityMergeRow,
  type IdentityMergeDetail,
} from '../api'
import { ApiError, UnauthorizedError } from '../../api/client'
import { useAdminAuth } from '../AdminAuth'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty, Stat, rowActivate } from '../../components/ui'
import { PageHeader } from '../../components/premium'
import { fmtDate } from '../../format'

// PCI AI identity console (spec §10.1): the health of the Student Number estate, the audited
// backfill for accounts that predate transactional issuance, ledger-versus-projection
// reconciliation, and the maker-checker duplicate-account merge queue.
//
// Two rules shape this page:
//   • There is NO way to type a Student Number and set it — the backend deliberately has no such
//    endpoint. Issuance belongs to the creating transaction or to the audited backfill; a number
//    an operator can type is a number an operator can collide. The page says so instead of
//    pretending otherwise.
//   • Hiding a card here is presentation only. Every endpoint enforces its own id_* permission
//    server-side; the gates below just avoid rendering sections the admin cannot use.

/** The literal phrase an admin must type before the backfill run button arms. */
export const RUN_CONFIRM_PHRASE = 'run backfill'

/** Pull the machine error code out of an ApiError body ({ error: "..." }). */
function codeOf(e: unknown): string {
  return e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body
    ? String((e.body as Record<string, unknown>).error)
    : ''
}

function msgOf(e: unknown): string {
  return e instanceof Error ? e.message : 'The action could not be completed. Please try again.'
}

/** Render a count that may be -1 ("this probe failed") — unknown is never shown as zero. */
function Count({ v }: { v: number }) {
  return v < 0 ? <span title="This check could not be run">unknown</span> : <>{v}</>
}

const label = (k: string) => k.replace(/_/g, ' ')

// ── Health dashboard (id_read) ─────────────────────────────────────────────

function HealthCard() {
  const { data, loading, error, refetch } = useAdminQuery<{ ok: boolean; health: IdentityHealth }>(
    '/api/admin/identity/student-numbers/health',
  )
  return (
    <Card
      title="Student Number health"
      action={<button className="btn sm secondary" onClick={refetch}>Refresh</button>}
    >
      <p className="muted small" style={{ marginTop: 0 }}>
        Counts only — this report never contains member records, so it is safe to paste into a ticket.
      </p>
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : !data ? (
        <Empty>No health data.</Empty>
      ) : (
        <>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(150px,1fr))', gap: '.8rem' }}>
            <Stat n={<Count v={data.health.eligible_students} />} k="Eligible students" />
            <Stat n={<Count v={data.health.missing_number} />} k="Missing number" />
            <Stat n={<Count v={data.health.blank_number} />} k="Blank number" />
            <Stat n={<Count v={data.health.duplicate_numbers} />} k="Duplicate numbers" />
            <Stat n={<Count v={data.health.malformed_numbers} />} k="Malformed numbers" />
            <Stat n={<Count v={data.health.projection_without_reservation} />} k="Number without registry entry" />
            <Stat n={<Count v={data.health.reservation_without_projection} />} k="Registry entry without number" />
            <Stat n={<Count v={data.health.retired_reservations} />} k="Retired reservations" />
            <Stat n={<Count v={data.health.quarantined_reservations} />} k="Quarantined" />
            <Stat n={<Count v={data.health.students_without_profile} />} k="Students without profile" />
          </div>
          <p style={{ marginBottom: 0 }}>
            Uniqueness index:{' '}
            {data.health.projection_index_eligible ? (
              <Badge tone="ok">eligible — the next boot will enforce uniqueness</Badge>
            ) : (
              <Badge tone="warn">blocked — duplicates must be resolved first</Badge>
            )}
          </p>
        </>
      )}
    </Card>
  )
}

// ── Backfill: dry-run preview, then a typed-confirmation run (id_backfill) ─

function BackfillCard({ onIssued }: { onIssued: () => void }) {
  const [limit, setLimit] = useState('100')
  const [preview, setPreview] = useState<IdentityBackfillPreview | null>(null)
  const [previewBusy, setPreviewBusy] = useState(false)
  const [previewError, setPreviewError] = useState<string | null>(null)

  const [batch, setBatch] = useState('200')
  const [confirm, setConfirm] = useState('')
  const [runBusy, setRunBusy] = useState(false)
  const [runError, setRunError] = useState<string | null>(null)
  const [run, setRun] = useState<{ correlation_id: string; result: IdentityBackfillResult } | null>(null)

  const armed = confirm === RUN_CONFIRM_PHRASE

  const doPreview = async () => {
    setPreviewBusy(true)
    setPreviewError(null)
    try {
      const n = Math.max(1, Math.min(500, Math.floor(Number(limit) || 100)))
      const res = await adminApi.post<{ ok: boolean; preview: IdentityBackfillPreview }>(
        `/api/admin/identity/student-numbers/backfill/preview?limit=${n}`,
      )
      setPreview(res.preview)
    } catch (e) {
      if (e instanceof UnauthorizedError) return
      setPreviewError(msgOf(e))
    } finally {
      setPreviewBusy(false)
    }
  }

  const doRun = async () => {
    if (!armed) return
    setRunBusy(true)
    setRunError(null)
    try {
      const n = Math.max(1, Math.min(1000, Math.floor(Number(batch) || 200)))
      const res = await adminApi.post<{ ok: boolean; correlation_id: string; result: IdentityBackfillResult }>(
        '/api/admin/identity/student-numbers/backfill/run',
        { batch_size: n },
      )
      setRun({ correlation_id: res.correlation_id, result: res.result })
      setConfirm('') // disarm — the next batch needs a fresh, deliberate confirmation
      setPreview(null) // the dry-run is stale the moment a real run has written
      onIssued()
    } catch (e) {
      if (e instanceof UnauthorizedError) return
      setRunError(msgOf(e))
    } finally {
      setRunBusy(false)
    }
  }

  return (
    <Card title="Backfill missing numbers">
      <p className="muted small" style={{ marginTop: 0 }}>
        Issues Student Numbers to accounts that predate transactional issuance. Resumable and
        idempotent: each batch selects only accounts that still have no number, and a collision
        quarantines that one account for a human instead of failing the batch. Preview first.
      </p>

      <div className="row" style={{ flexWrap: 'wrap', gap: '.5rem', alignItems: 'center' }}>
        <label className="small" htmlFor="id-preview-limit">Preview limit</label>
        <input id="id-preview-limit" type="number" min={1} max={500} value={limit}
          onChange={(e) => setLimit(e.target.value)} style={{ width: 90 }} />
        <button className="btn sm secondary" disabled={previewBusy} onClick={doPreview}>
          {previewBusy ? 'Previewing…' : 'Preview (dry run)'}
        </button>
      </div>
      {previewError && <ErrorNote>{previewError}</ErrorNote>}
      {preview && (
        <div style={{ marginTop: '.8rem' }}>
          <div className="notice" role="status" style={{ marginBottom: '.6rem' }}>
            <strong>Dry run — this preview wrote nothing.</strong>{' '}
            It shows the number each account <em>would</em> receive ({preview.total_missing} missing
            in total, showing {preview.sample_size}).
          </div>
          {preview.sample.length === 0 ? (
            <Empty>No accounts are missing a number.</Empty>
          ) : (
            <table className="data">
              <thead>
                <tr><th>User</th><th>Account created</th><th>Would issue</th><th>Conflict</th></tr>
              </thead>
              <tbody>
                {preview.sample.map((r) => (
                  <tr key={r.user_id}>
                    <td>#{r.user_id}</td>
                    <td className="small">{fmtDate(r.created_at)}</td>
                    <td><code>{r.would_issue}</code></td>
                    <td>{r.conflict
                      ? <Badge tone="err">conflict — would be quarantined</Badge>
                      : <Badge tone="ok">clear</Badge>}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}

      <hr style={{ margin: '1rem 0', border: 'none', borderTop: '1px solid #e5e7eb' }} />

      <div className="notice warn" style={{ marginBottom: '.6rem' }}>
        Running the backfill <strong>writes identity data</strong>: it permanently reserves and
        issues Student Numbers. To arm the button, type <strong>{RUN_CONFIRM_PHRASE}</strong> below.
      </div>
      <div className="row" style={{ flexWrap: 'wrap', gap: '.5rem', alignItems: 'center' }}>
        <label className="small" htmlFor="id-run-batch">Batch size</label>
        <input id="id-run-batch" type="number" min={1} max={1000} value={batch}
          onChange={(e) => setBatch(e.target.value)} style={{ width: 90 }} />
        <input
          aria-label={`Type "${RUN_CONFIRM_PHRASE}" to confirm`}
          placeholder={`Type "${RUN_CONFIRM_PHRASE}" to confirm`}
          value={confirm}
          onChange={(e) => setConfirm(e.target.value)}
          style={{ width: 230 }}
        />
        <button className="btn sm danger" disabled={!armed || runBusy} onClick={doRun}>
          {runBusy ? 'Running…' : 'Run backfill'}
        </button>
      </div>
      {runError && <ErrorNote>{runError}</ErrorNote>}
      {run && (
        <div style={{ marginTop: '.8rem' }}>
          <div className="notice" role="status" style={{ marginBottom: '.6rem' }}>
            Batch complete (audit correlation <code>{run.correlation_id}</code>).
          </div>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(130px,1fr))', gap: '.8rem' }}>
            <Stat n={run.result.examined} k="Examined" />
            <Stat n={run.result.issued} k="Issued" />
            <Stat n={run.result.quarantined} k="Quarantined this batch" />
            <Stat n={run.result.remaining} k="Remaining" />
            <Stat n={`#${run.result.high_water_user_id}`} k="High-water user" />
          </div>
          {run.result.failures.length > 0 && (
            <table className="data" style={{ marginTop: '.6rem' }}>
              <thead><tr><th>User</th><th>Quarantine reason</th></tr></thead>
              <tbody>
                {run.result.failures.map((f) => (
                  <tr key={f.user_id}><td>#{f.user_id}</td><td className="small">{f.reason}</td></tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      )}
    </Card>
  )
}

// ── Reconciliation: ledger versus projection (id_audit) ────────────────────

function ReconcileCard() {
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [rec, setRec] = useState<IdentityReconciliation | null>(null)

  const doReconcile = async () => {
    setBusy(true)
    setError(null)
    try {
      const res = await adminApi.get<{ ok: boolean; reconciliation: IdentityReconciliation }>(
        '/api/admin/identity/student-numbers/reconcile',
      )
      setRec(res.reconciliation)
    } catch (e) {
      if (e instanceof UnauthorizedError) return
      setError(msgOf(e))
    } finally {
      setBusy(false)
    }
  }

  return (
    <Card
      title="Reconcile registry against projection"
      action={<button className="btn sm secondary" disabled={busy} onClick={doReconcile}>{busy ? 'Checking…' : 'Run reconciliation'}</button>}
    >
      <p className="muted small" style={{ marginTop: 0 }}>
        Compares the permanent number registry (the ledger) with <code>users.registration_no</code>
        (the projection), in both directions. Samples list the numbers involved — never the people.
      </p>
      {error && <ErrorNote>{error}</ErrorNote>}
      {rec && (
        <>
          <p>
            {rec.reconciled
              ? <Badge tone="ok">Reconciled — no drift in either direction</Badge>
              : <Badge tone="err">Drift detected</Badge>}
          </p>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(200px,1fr))', gap: '.8rem' }}>
            <Stat n={rec.projection_without_reservation} k="Numbers with no registry entry" />
            <Stat n={rec.reservation_without_projection} k="Registry entries with no number" />
          </div>
          {rec.sample_projection_without_reservation.length > 0 && (
            <p className="small">
              <strong>Unregistered numbers (sample):</strong>{' '}
              {rec.sample_projection_without_reservation.map((n) => <code key={n} style={{ marginRight: '.4rem' }}>{n}</code>)}
            </p>
          )}
          {rec.sample_reservation_without_projection.length > 0 && (
            <p className="small">
              <strong>Unprojected reservations (sample):</strong>{' '}
              {rec.sample_reservation_without_projection.map((n) => <code key={n} style={{ marginRight: '.4rem' }}>{n}</code>)}
            </p>
          )}
        </>
      )}
    </Card>
  )
}

// ── Maker-checker merge queue (id_read to see; request/approve split) ──────

function MergeCounts({ detail }: { detail: IdentityMergeDetail }) {
  const keys = Array.from(new Set([...Object.keys(detail.preview.source), ...Object.keys(detail.preview.target)]))
  return (
    <table className="data">
      <thead>
        <tr>
          <th>Records</th>
          <th>Source #{detail.request.source_user_id} (absorbed)</th>
          <th>Target #{detail.request.target_user_id} (survivor)</th>
        </tr>
      </thead>
      <tbody>
        {keys.map((k) => (
          <tr key={k}>
            <td className="small">{label(k)}</td>
            <td><Count v={detail.preview.source[k] ?? 0} /></td>
            <td><Count v={detail.preview.target[k] ?? 0} /></td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}

function MergeQueue({ canRequest, canApprove, onMerged }: { canRequest: boolean; canApprove: boolean; onMerged: () => void }) {
  const merges = useAdminQuery<{ ok: boolean; rows: IdentityMergeRow[] }>('/api/admin/identity/merges')
  const [selected, setSelected] = useState<number | null>(null)
  const detail = useAdminQuery<{ ok: boolean; merge: IdentityMergeDetail }>(
    selected != null ? `/api/admin/identity/merges/${selected}` : null,
  )

  // create form (maker)
  const [source, setSource] = useState('')
  const [target, setTarget] = useState('')
  const [reason, setReason] = useState('')
  const [createBusy, setCreateBusy] = useState(false)
  const [createError, setCreateError] = useState<string | null>(null)
  const [createNotice, setCreateNotice] = useState<string | null>(null)

  // decision (checker)
  const [note, setNote] = useState('')
  const [decideBusy, setDecideBusy] = useState<string | null>(null)
  const [decideError, setDecideError] = useState<string | null>(null)
  const [decideNotice, setDecideNotice] = useState<string | null>(null)

  const doCreate = async () => {
    setCreateBusy(true)
    setCreateError(null)
    setCreateNotice(null)
    try {
      const res = await adminApi.post<{ ok: boolean; id: number }>('/api/admin/identity/merges', {
        source_user_id: Number(source),
        target_user_id: Number(target),
        ...(reason.trim() ? { reason: reason.trim() } : {}),
      })
      setSource(''); setTarget(''); setReason('')
      setCreateNotice(`Merge request #${res.id} created. A different admin with approval permission must now decide it.`)
      merges.refetch()
    } catch (e) {
      if (e instanceof UnauthorizedError) return
      const friendly: Record<string, string> = {
        self_merge: 'Source and target must be two different accounts.',
        unknown_user: 'One of those user ids does not exist.',
        not_student: 'Both accounts must be student accounts.',
        not_mergeable: 'One of the accounts has been erased or already merged, so it cannot take part in a merge.',
        already_pending: 'One of these accounts is already part of a pending merge request — that one must be decided first.',
      }
      setCreateError(friendly[codeOf(e)] ?? msgOf(e))
    } finally {
      setCreateBusy(false)
    }
  }

  const doDecide = async (id: number, action: 'approve' | 'reject') => {
    setDecideBusy(action)
    setDecideError(null)
    setDecideNotice(null)
    try {
      await adminApi.post(`/api/admin/identity/merges/${id}/${action}`, {
        ...(note.trim() ? { note: note.trim() } : {}),
      })
      setNote('')
      setDecideNotice(action === 'approve'
        ? 'Merge approved and executed. The survivor keeps its Student Number; the absorbed account resolves to it.'
        : 'Merge request rejected.')
      merges.refetch()
      detail.refetch()
      onMerged()
    } catch (e) {
      if (e instanceof UnauthorizedError) return
      const code = codeOf(e)
      if (code === 'maker_checker') {
        // Not a fault to retry — the rule is that a different admin must decide this.
        setDecideError(
          'A different admin must decide this: you requested this merge, and the admin who requests '
          + 'a merge can never be the one who approves it — even with approval permission. '
          + 'Ask a colleague with merge-approval access to review it.',
        )
      } else if (code === 'already_decided') {
        setDecideNotice('This request was already decided by another admin — the queue has been refreshed.')
        merges.refetch()
        detail.refetch()
      } else {
        setDecideError(msgOf(e))
      }
    } finally {
      setDecideBusy(null)
    }
  }

  const d = detail.data?.merge
  const pending = d?.request.status === 'pending'

  return (
    <Card title="Duplicate-account merges">
      <p className="muted small" style={{ marginTop: 0 }}>
        Maker-checker, enforced by the server: one admin requests a merge, and a <strong>different</strong>{' '}
        admin approves or rejects it — the requester can never decide their own request. Approval
        executes the merge in one transaction: the survivor keeps its Student Number, the absorbed
        account&apos;s number is retired to it, and sessions on both sides are revoked.
      </p>

      {canRequest && (
        <div style={{ marginBottom: '1rem' }}>
          <div className="row" style={{ flexWrap: 'wrap', gap: '.5rem', alignItems: 'center' }}>
            <input aria-label="Source user id (absorbed)" placeholder="Source user id (absorbed)"
              value={source} onChange={(e) => setSource(e.target.value)} style={{ width: 190 }} />
            <input aria-label="Target user id (survivor)" placeholder="Target user id (survivor)"
              value={target} onChange={(e) => setTarget(e.target.value)} style={{ width: 190 }} />
            <input aria-label="Reason" placeholder="Reason (recorded)"
              value={reason} onChange={(e) => setReason(e.target.value)} style={{ width: 220 }} />
            <button className="btn sm" disabled={createBusy || !(Number(source) > 0 && Number(target) > 0)} onClick={doCreate}>
              {createBusy ? 'Requesting…' : 'Request merge'}
            </button>
          </div>
          {createError && <ErrorNote>{createError}</ErrorNote>}
          {createNotice && <div className="notice" role="status" style={{ marginTop: '.5rem' }}>{createNotice}</div>}
        </div>
      )}

      {merges.loading ? <Spinner /> : merges.error ? <ErrorNote>{merges.error}</ErrorNote>
        : !merges.data || merges.data.rows.length === 0 ? (
          <Empty>No merge requests.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>#</th><th>Source → Target</th><th>Status</th><th>Requested by</th><th>Requested</th><th>Decided by</th><th>Note</th></tr>
            </thead>
            <tbody>
              {merges.data.rows.map((r) => (
                <tr key={r.id} {...rowActivate(() => setSelected(r.id))}
                  style={{ cursor: 'pointer', background: selected === r.id ? 'var(--brand-050)' : undefined }}>
                  <td>{r.id}</td>
                  <td>#{r.source_user_id} → #{r.target_user_id}</td>
                  <td><StatusBadge status={r.status} /></td>
                  <td>admin #{r.requested_by}</td>
                  <td className="small">{fmtDate(r.requested_at)}</td>
                  <td className="small">{r.decided_by != null ? `admin #${r.decided_by}` : '—'}</td>
                  <td className="small" style={{ maxWidth: 220 }}>{r.decision_note || r.reason || '—'}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}

      {selected != null && (
        <div style={{ marginTop: '1rem' }}>
          {detail.loading ? <Spinner /> : detail.error ? <ErrorNote>{detail.error}</ErrorNote> : d && (
            <>
              <h3 style={{ margin: '0 0 .4rem' }}>Merge request #{d.request.id} — what would move</h3>
              <p className="muted small" style={{ marginTop: 0 }}>
                The preview is per-table record counts (the blast radius), never member records.
                Requested by admin #{d.request.requested_by}
                {pending && ' — a different admin must approve or reject it.'}
              </p>
              <MergeCounts detail={d} />
              {decideError && <ErrorNote>{decideError}</ErrorNote>}
              {decideNotice && <div className="notice" role="status" style={{ marginTop: '.5rem' }}>{decideNotice}</div>}
              {canApprove && pending && (
                <div className="row" style={{ flexWrap: 'wrap', gap: '.5rem', alignItems: 'center', marginTop: '.6rem' }}>
                  <input aria-label="Decision note" placeholder="Decision note (recorded)"
                    value={note} onChange={(e) => setNote(e.target.value)} style={{ width: 260 }} />
                  <button className="btn sm danger" disabled={decideBusy != null}
                    onClick={() => doDecide(d.request.id, 'approve')}>
                    {decideBusy === 'approve' ? 'Approving…' : 'Approve & execute'}
                  </button>
                  <button className="btn sm ghost" disabled={decideBusy != null}
                    onClick={() => doDecide(d.request.id, 'reject')}>
                    {decideBusy === 'reject' ? 'Rejecting…' : 'Reject'}
                  </button>
                </div>
              )}
            </>
          )}
        </div>
      )}
    </Card>
  )
}

// ── Page ───────────────────────────────────────────────────────────────────

export default function Identity() {
  const { can } = useAdminAuth()
  // A key so the health card refetches after a write elsewhere on the page (backfill run, merge).
  const [healthEpoch, setHealthEpoch] = useState(0)
  const bumpHealth = () => setHealthEpoch((e) => e + 1)

  return (
    <div className="page">
      <PageHeader
        title="Identity & Student Numbers"
        subtitle="Canonical identity operations: estate health, the audited backfill for accounts that predate transactional issuance, registry reconciliation, and maker-checker duplicate-account merges."
      />

      <div className="notice" role="note" style={{ marginBottom: '1rem' }}>
        <strong>Student Numbers cannot be set by hand — anywhere.</strong> There is deliberately no
        endpoint that assigns a Student Number an operator typed: issuance happens only inside the
        account-creating transaction or through the audited backfill below, because a number an
        operator can type is a number an operator can collide. If a number looks wrong, use the
        health report, reconciliation, or a merge request — not manual entry.
      </div>

      {can('id_read') && <HealthCard key={healthEpoch} />}
      {can('id_backfill') && <BackfillCard onIssued={bumpHealth} />}
      {can('id_audit') && <ReconcileCard />}
      {can('id_read') && (
        <MergeQueue canRequest={can('id_merge_request')} canApprove={can('id_merge_approve')} onMerged={bumpHealth} />
      )}
      {!can('id_read') && (
        <Empty>
          Your account holds an identity permission but not <code>id_read</code>, so the health
          report and merge queue are not shown. The server enforces the same rule on every call.
        </Empty>
      )}
    </div>
  )
}
