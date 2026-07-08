import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type CertRow } from '../api'
import { ApiError } from '../../api/client'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtMoney } from '../../format'

type Draft = Partial<CertRow>

const CERT_ERRORS: Record<string, string> = {
  bad_code: 'Code must be 2–20 characters: letters, digits and hyphens (e.g. PCP-COST).',
  name_required: 'A name is required.',
  code_exists: 'A certification with that code already exists.',
}

function Editor({ initial, onClose, onSaved }: { initial: Draft | null; onClose: () => void; onSaved: () => void }) {
  const isNew = !initial?.id
  const [d, setD] = useState<Draft>(
    initial ?? { code: '', name: '', expiry_years: 3, pass_mark_pct: 70, duration_minutes: 90, active: 1 },
  )
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const set = (k: keyof CertRow, v: unknown) => setD((p) => ({ ...p, [k]: v }))

  async function save() {
    setBusy(true)
    setError(null)
    try {
      const payload = {
        code: d.code,
        name: d.name,
        description: d.description,
        credential_prefix: d.credential_prefix,
        pass_mark_pct: d.pass_mark_pct,
        duration_minutes: d.duration_minutes,
        expiry_years: d.expiry_years,
        exam_price: d.exam_price,
        // send a real JSON boolean — the backend keys the founding-cert guard and the create default on
        // JsonValueKind.False, so a numeric 0 would be misread as "active" and defeat both.
        active: !!d.active,
        sort_order: d.sort_order,
      }
      if (isNew) await adminApi.post('/api/admin/certifications', payload)
      else await adminApi.patch(`/api/admin/certifications/${d.id}`, payload)
      onSaved()
    } catch (e) {
      const code = e instanceof ApiError && e.body && typeof e.body === 'object' && 'error' in e.body ? String((e.body as Record<string, unknown>).error) : ''
      setError(CERT_ERRORS[code] || (e instanceof Error ? e.message : 'Could not save.'))
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{isNew ? 'New certification' : 'Edit certification'}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
        <div className="grid cols-2">
          <div className="field"><label>Code</label><input value={d.code ?? ''} disabled={!isNew} onChange={(e) => set('code', e.target.value.toUpperCase())} placeholder="PCP-COST" /></div>
          <div className="field"><label>Name</label><input value={d.name ?? ''} onChange={(e) => set('name', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Description</label><textarea rows={2} value={d.description ?? ''} onChange={(e) => set('description', e.target.value)} /></div>
          <div className="field"><label>Credential prefix</label><input value={d.credential_prefix ?? ''} onChange={(e) => set('credential_prefix', e.target.value)} placeholder="PCP" /></div>
          <div className="field"><label>Exam price (USD)</label><input type="number" step="1" value={d.exam_price ?? ''} onChange={(e) => set('exam_price', e.target.value === '' ? null : Number(e.target.value))} /></div>
          <div className="field"><label>Pass mark (%)</label><input type="number" value={d.pass_mark_pct ?? ''} onChange={(e) => set('pass_mark_pct', Number(e.target.value))} /></div>
          <div className="field"><label>Duration (min)</label><input type="number" value={d.duration_minutes ?? ''} onChange={(e) => set('duration_minutes', Number(e.target.value))} /></div>
          <div className="field"><label>Expiry (years)</label><input type="number" value={d.expiry_years ?? ''} onChange={(e) => set('expiry_years', Number(e.target.value))} /></div>
          <div className="field"><label>Sort order</label><input type="number" value={d.sort_order ?? ''} onChange={(e) => set('sort_order', Number(e.target.value))} /></div>
          <div className="field"><label>Active</label>
            <select value={d.active ? '1' : '0'} onChange={(e) => set('active', e.target.value === '1')}>
              <option value="1">Active</option>
              <option value="0">Inactive</option>
            </select>
          </div>
        </div>
        <button className="btn" disabled={busy || !d.code || !d.name} onClick={save}>{busy ? 'Saving…' : 'Save certification'}</button>
      </div>
    </div>
  )
}

export default function Certifications() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: CertRow[] }>('/api/admin/certifications')
  const [editing, setEditing] = useState<Draft | null | undefined>(undefined) // undefined = closed

  async function remove(c: CertRow) {
    if (!confirm(`Delete certification "${c.name}"? This cannot be undone.`)) return
    try {
      await adminApi.del(`/api/admin/certifications/${c.id}`)
      refetch()
    } catch (e) {
      alert(e instanceof Error ? e.message : 'Could not delete.')
    }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Certifications</h1>
        <button className="btn sm" onClick={() => setEditing(null)}>New certification</button>
      </div>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No certifications yet.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Code</th><th>Name</th><th>Price</th><th>Pass</th><th>Bank</th><th>Entitlements</th><th>Creds</th><th></th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((c) => (
                <tr key={c.id}>
                  <td><strong>{c.code}</strong>{!c.active && <> <Badge tone="err">inactive</Badge></>}</td>
                  <td>{c.name}</td>
                  <td>{c.exam_price != null ? fmtMoney(c.exam_price) : '—'}</td>
                  <td>{c.pass_mark_pct ?? '—'}%</td>
                  <td>{c.bank_size ?? 0}</td>
                  <td>{c.entitlements ?? 0}</td>
                  <td>{c.credentials ?? 0}</td>
                  <td><button className="btn ghost sm" onClick={() => setEditing(c)}>Edit</button></td>
                  <td><button className="btn ghost sm" onClick={() => remove(c)}>Delete</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {editing !== undefined && <Editor initial={editing} onClose={() => setEditing(undefined)} onSaved={() => { setEditing(undefined); refetch() }} />}
    </div>
  )
}
