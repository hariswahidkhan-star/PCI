import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type DiscountCode } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'

function CreateForm({ onClose, onSaved }: { onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState({ code: '', discount_type: 'percentage', discount_value: '', applies_to: 'all', start_date: '', end_date: '', max_uses: '', single_use_per_email: false, active: true })
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)

  async function save() {
    setBusy(true)
    setError(null)
    try {
      await adminApi.post('/api/admin/codes', {
        code: f.code.toUpperCase(),
        discount_type: f.discount_type,
        discount_value: Number(f.discount_value) || 0,
        applies_to: f.applies_to,
        start_date: f.start_date || null,
        end_date: f.end_date || null,
        max_uses: f.max_uses === '' ? null : Number(f.max_uses),
        single_use_per_email: f.single_use_per_email,
        active: f.active,
      })
      onSaved()
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Could not create code.')
    } finally {
      setBusy(false)
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>New discount code</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
        <div className="grid cols-2">
          <div className="field"><label>Code</label><input value={f.code} onChange={(e) => setF({ ...f, code: e.target.value.toUpperCase() })} placeholder="LAUNCH20" /></div>
          <div className="field"><label>Type</label>
            <select value={f.discount_type} onChange={(e) => setF({ ...f, discount_type: e.target.value })}>
              <option value="percentage">Percentage</option>
              <option value="fixed">Fixed amount</option>
            </select>
          </div>
          <div className="field"><label>Value {f.discount_type === 'percentage' ? '(%)' : '(USD)'}</label><input type="number" value={f.discount_value} onChange={(e) => setF({ ...f, discount_value: e.target.value })} /></div>
          <div className="field"><label>Applies to</label>
            <select value={f.applies_to} onChange={(e) => setF({ ...f, applies_to: e.target.value })}>
              <option value="all">Everything</option>
              <option value="membership">Membership</option>
              <option value="exam">Exam</option>
            </select>
          </div>
          <div className="field"><label>Start date</label><input type="date" value={f.start_date} onChange={(e) => setF({ ...f, start_date: e.target.value })} /></div>
          <div className="field"><label>End date</label><input type="date" value={f.end_date} onChange={(e) => setF({ ...f, end_date: e.target.value })} /></div>
          <div className="field"><label>Max uses (blank = unlimited)</label><input type="number" value={f.max_uses} onChange={(e) => setF({ ...f, max_uses: e.target.value })} /></div>
        </div>
        <div className="row" style={{ gap: '1.5rem', marginBottom: '1rem' }}>
          <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.single_use_per_email} onChange={(e) => setF({ ...f, single_use_per_email: e.target.checked })} /> One use per email</label>
          <label className="row" style={{ fontWeight: 400 }}><input type="checkbox" style={{ width: 'auto' }} checked={f.active} onChange={(e) => setF({ ...f, active: e.target.checked })} /> Active</label>
        </div>
        <button className="btn" disabled={busy || !f.code || !f.discount_value} onClick={save}>{busy ? 'Creating…' : 'Create code'}</button>
      </div>
    </div>
  )
}

export default function Codes() {
  const { data, loading, error, refetch } = useAdminQuery<DiscountCode[]>('/api/admin/codes')
  const [creating, setCreating] = useState(false)

  async function toggle(c: DiscountCode) {
    await adminApi.patch(`/api/admin/codes/${c.id}`, { active: c.active ? false : true })
    refetch()
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Discount codes</h1>
        <button className="btn sm" onClick={() => setCreating(true)}>New code</button>
      </div>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.length === 0 ? (
          <Empty>No discount codes yet.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Code</th><th>Discount</th><th>Applies to</th><th>Uses</th><th>Ends</th><th>Status</th><th></th></tr>
            </thead>
            <tbody>
              {data.map((c) => (
                <tr key={c.id}>
                  <td><strong>{c.code}</strong>{c.code_type === 'referral' && <> <Badge tone="brand">referral</Badge></>}</td>
                  <td>{c.discount_type === 'fixed' ? `$${c.discount_value}` : `${c.discount_value}%`}</td>
                  <td className="small">{c.applies_to || 'all'}</td>
                  <td>{c.used_count ?? 0}{c.max_uses ? ` / ${c.max_uses}` : ''}</td>
                  <td className="small muted">{c.end_date ? fmtDate(c.end_date) : '—'}</td>
                  <td>{c.active ? <Badge tone="ok">active</Badge> : <Badge tone="err">inactive</Badge>}</td>
                  <td><button className="btn ghost sm" onClick={() => toggle(c)}>{c.active ? 'Disable' : 'Enable'}</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {creating && <CreateForm onClose={() => setCreating(false)} onSaved={() => { setCreating(false); refetch() }} />}
    </div>
  )
}
