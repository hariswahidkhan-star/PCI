import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type CertRow, CERT_STATUSES } from '../api'
import { ApiError } from '../../api/client'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtMoney } from '../../format'

type Draft = Partial<CertRow>

const CERT_ERRORS: Record<string, string> = {
  bad_code: 'Code must be 2–20 characters: letters, digits and hyphens (e.g. PCP-COST).',
  name_required: 'A name is required.',
  code_exists: 'A certification with that code already exists.',
  slug_exists: 'That slug is already used by another certification.',
  bad_status: 'Status must be one of the defined lifecycle statuses.',
  bad_slug: 'Slug must contain letters or digits (a–z, 0–9, hyphens).',
  prefix_reserved: 'PCI-HON is reserved for honorary awards.',
  founding_cert_permanent: 'The founding certification (PCP-AI) cannot be deactivated.',
}

// competency areas are stored inside content_json; edit them as one-per-line text
function competenciesFrom(json?: string | null): string {
  try { const o = JSON.parse(json || '{}'); return Array.isArray(o.competencies) ? o.competencies.join('\n') : '' } catch { return '' }
}
function withCompetencies(json: string | null | undefined, text: string): string {
  let o: Record<string, unknown> = {}
  try { o = JSON.parse(json || '{}') || {} } catch { o = {} }
  o.competencies = text.split('\n').map((s) => s.trim()).filter(Boolean)
  return JSON.stringify(o)
}

function Editor({ initial, onClose, onSaved }: { initial: Draft | null; onClose: () => void; onSaved: () => void }) {
  const isNew = !initial?.id
  const [d, setD] = useState<Draft>(
    initial ?? { code: '', name: '', expiry_years: 3, pass_mark_pct: 70, duration_minutes: 90, active: 1, status: 'Active', level: 'Professional', certuvo_enabled: 1 },
  )
  const [comps, setComps] = useState(competenciesFrom(initial?.content_json))
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const set = (k: keyof CertRow, v: unknown) => setD((p) => ({ ...p, [k]: v }))
  const num = (v: string) => (v === '' ? null : Number(v))

  async function save() {
    setBusy(true)
    setError(null)
    try {
      const payload = {
        code: d.code,
        name: d.name,
        acronym: d.acronym,
        public_title: d.public_title,
        tagline: d.tagline,
        short_description: d.short_description,
        description: d.description,
        audience: d.audience,
        category: d.category,
        level: d.level,
        status: d.status,
        slug: d.slug,
        credential_prefix: d.credential_prefix,
        pass_mark_pct: d.pass_mark_pct,
        duration_minutes: d.duration_minutes,
        expiry_years: d.expiry_years,
        exam_price: d.exam_price,
        application_fee: d.application_fee,
        membership_required: !!d.membership_required,
        meta_title: d.meta_title,
        meta_description: d.meta_description,
        keywords: d.keywords,
        certuvo_enabled: !!d.certuvo_enabled,
        certuvo_product: d.certuvo_product,
        content_json: withCompetencies(d.content_json, comps),
        // send a real JSON boolean — the backend keys the founding-cert guard and the create default on
        // JsonValueKind.False, so a numeric 0 would be misread as "active".
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
          <h2 style={{ margin: 0 }}>{isNew ? 'New certification' : `Edit ${d.code ?? ''}`}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}

        <h3 className="section-label">Identity &amp; public catalogue</h3>
        <div className="grid cols-2">
          <div className="field"><label>Code {isNew ? '' : '(permanent)'}</label><input value={d.code ?? ''} disabled={!isNew} onChange={(e) => set('code', e.target.value.toUpperCase())} placeholder="PCP-COST" /></div>
          <div className="field"><label>Acronym</label><input value={d.acronym ?? ''} onChange={(e) => set('acronym', e.target.value)} placeholder="PCP-COST" /></div>
          <div className="field"><label>Name</label><input value={d.name ?? ''} onChange={(e) => set('name', e.target.value)} /></div>
          <div className="field"><label>Public title</label><input value={d.public_title ?? ''} onChange={(e) => set('public_title', e.target.value)} /></div>
          <div className="field"><label>Status</label>
            <select value={d.status ?? 'Active'} onChange={(e) => set('status', e.target.value)}>
              {CERT_STATUSES.map((s) => <option key={s} value={s}>{s}</option>)}
            </select>
          </div>
          <div className="field"><label>URL slug (public page /certifications/…)</label><input value={d.slug ?? ''} onChange={(e) => set('slug', e.target.value)} placeholder="pcp-cost" /></div>
          <div className="field"><label>Category</label><input value={d.category ?? ''} onChange={(e) => set('category', e.target.value)} placeholder="Cost Engineering" /></div>
          <div className="field"><label>Level</label><input value={d.level ?? ''} onChange={(e) => set('level', e.target.value)} placeholder="Professional" /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Tagline</label><input value={d.tagline ?? ''} onChange={(e) => set('tagline', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Short description (catalogue card)</label><textarea rows={2} value={d.short_description ?? ''} onChange={(e) => set('short_description', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Overview / description</label><textarea rows={3} value={d.description ?? ''} onChange={(e) => set('description', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Who it’s for (audience)</label><textarea rows={2} value={d.audience ?? ''} onChange={(e) => set('audience', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Competency areas — one per line</label><textarea rows={4} value={comps} onChange={(e) => setComps(e.target.value)} placeholder={'Investment appraisal\nFinancial modelling\n…'} /></div>
        </div>

        <h3 className="section-label">Examination, fees &amp; membership</h3>
        <div className="grid cols-2">
          <div className="field"><label>Credential prefix</label><input value={d.credential_prefix ?? ''} onChange={(e) => set('credential_prefix', e.target.value)} placeholder="PCP" /></div>
          <div className="field"><label>Exam price (USD) — blank = standard</label><input type="number" step="1" value={d.exam_price ?? ''} onChange={(e) => set('exam_price', num(e.target.value))} /></div>
          <div className="field"><label>Application fee (USD)</label><input type="number" step="1" value={d.application_fee ?? ''} onChange={(e) => set('application_fee', num(e.target.value))} /></div>
          <div className="field"><label>Pass mark (%) — blank = global</label><input type="number" value={d.pass_mark_pct ?? ''} onChange={(e) => set('pass_mark_pct', num(e.target.value))} /></div>
          <div className="field"><label>Duration (min) — blank = global</label><input type="number" value={d.duration_minutes ?? ''} onChange={(e) => set('duration_minutes', num(e.target.value))} /></div>
          <div className="field"><label>Expiry (years)</label><input type="number" value={d.expiry_years ?? ''} onChange={(e) => set('expiry_years', num(e.target.value))} /></div>
          <div className="field"><label>Membership required</label>
            <select value={d.membership_required ? '1' : '0'} onChange={(e) => set('membership_required', e.target.value === '1' ? 1 : 0)}>
              <option value="0">No</option>
              <option value="1">Yes</option>
            </select>
          </div>
          <div className="field"><label>Sort order</label><input type="number" value={d.sort_order ?? ''} onChange={(e) => set('sort_order', num(e.target.value))} /></div>
          <div className="field"><label>Active</label>
            <select value={d.active ? '1' : '0'} onChange={(e) => set('active', e.target.value === '1')}>
              <option value="1">Active</option>
              <option value="0">Inactive</option>
            </select>
          </div>
        </div>

        <h3 className="section-label">Certuvo (study &amp; practice)</h3>
        <div className="grid cols-2">
          <div className="field"><label>Certuvo enabled</label>
            <select value={d.certuvo_enabled ? '1' : '0'} onChange={(e) => set('certuvo_enabled', e.target.value === '1' ? 1 : 0)}>
              <option value="1">Enabled</option>
              <option value="0">Disabled</option>
            </select>
          </div>
          <div className="field"><label>Certuvo product (mapping)</label><input value={d.certuvo_product ?? ''} onChange={(e) => set('certuvo_product', e.target.value)} placeholder="PCL-AI" /></div>
        </div>

        <h3 className="section-label">SEO</h3>
        <div className="grid cols-2">
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Meta title</label><input value={d.meta_title ?? ''} onChange={(e) => set('meta_title', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Meta description</label><textarea rows={2} value={d.meta_description ?? ''} onChange={(e) => set('meta_description', e.target.value)} /></div>
          <div className="field" style={{ gridColumn: '1 / -1' }}><label>Keywords</label><input value={d.keywords ?? ''} onChange={(e) => set('keywords', e.target.value)} /></div>
        </div>

        <button className="btn" disabled={busy || !d.code || !d.name} onClick={save} style={{ marginTop: '1rem' }}>{busy ? 'Saving…' : 'Save certification'}</button>
      </div>
    </div>
  )
}

function statusTone(s?: string | null): 'ok' | 'warn' | 'err' | 'neutral' {
  if (s === 'Active' || s === 'Open for Applications') return 'ok'
  if (s === 'Temporarily Suspended' || s === 'Closed' || s === 'Archived') return 'err'
  return 'warn'
}

interface RouteRow {
  id: number; route_key: string; label: string; description?: string | null
  enabled?: number; public?: number; exam_required?: number; requires_approval?: number
  fee_mode?: string | null; sort_order?: number
}
const FEE_MODES = ['standard', 'free', 'waived_partial', 'sponsored', 'custom'] as const

// Per-certification application routes: Standard / Founding / Honorary / Sponsored / Complimentary /
// Fully & Partially Waived / Test User — each toggled, gated and priced independently.
function RoutesDrawer({ cert, onClose }: { cert: CertRow; onClose: () => void }) {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: RouteRow[] }>(`/api/admin/certifications/${cert.id}/routes`)
  async function patch(rid: number, body: Record<string, unknown>) {
    try { await adminApi.patch(`/api/admin/certification-routes/${rid}`, body); refetch() }
    catch (e) { alert(e instanceof Error ? e.message : 'Could not save.') }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>Application routes — {cert.acronym || cert.code}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        <p className="muted" style={{ marginTop: 0 }}>Enable, expose publicly, require an exam or approval, and set the fee mode for each route — independently for this certification.</p>
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : (
          <table className="data">
            <thead><tr><th>Route</th><th>Enabled</th><th>Public</th><th>Exam</th><th>Approval</th><th>Fee mode</th></tr></thead>
            <tbody>
              {data?.rows.map((r) => (
                <tr key={r.id}>
                  <td><strong>{r.label}</strong><br /><span className="muted" style={{ fontSize: '.8em' }}>{r.route_key}</span></td>
                  <td><input type="checkbox" checked={!!r.enabled} onChange={(e) => patch(r.id, { enabled: e.target.checked })} /></td>
                  <td><input type="checkbox" checked={!!r.public} onChange={(e) => patch(r.id, { public: e.target.checked })} /></td>
                  <td><input type="checkbox" checked={!!r.exam_required} onChange={(e) => patch(r.id, { exam_required: e.target.checked })} /></td>
                  <td><input type="checkbox" checked={!!r.requires_approval} onChange={(e) => patch(r.id, { requires_approval: e.target.checked })} /></td>
                  <td>
                    <select value={r.fee_mode ?? 'standard'} onChange={(e) => patch(r.id, { fee_mode: e.target.value })}>
                      {FEE_MODES.map((m) => <option key={m} value={m}>{m}</option>)}
                    </select>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}

export default function Certifications() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: CertRow[] }>('/api/admin/certifications')
  const [editing, setEditing] = useState<Draft | null | undefined>(undefined) // undefined = closed
  const [routesCert, setRoutesCert] = useState<CertRow | null>(null)

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
              <tr><th>Code</th><th>Name</th><th>Status</th><th>Price</th><th>Pass</th><th>Bank</th><th>Entitlements</th><th>Creds</th><th></th><th></th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((c) => (
                <tr key={c.id}>
                  <td><strong>{c.code}</strong>{!c.active && <> <Badge tone="err">off</Badge></>}</td>
                  <td>{c.public_title || c.name}</td>
                  <td><Badge tone={statusTone(c.status)}>{c.status || (c.active ? 'Active' : 'Inactive')}</Badge></td>
                  <td>{c.exam_price != null ? fmtMoney(c.exam_price) : '—'}</td>
                  <td>{c.pass_mark_pct ?? '—'}%</td>
                  <td>{c.bank_size ?? 0}</td>
                  <td>{c.entitlements ?? 0}</td>
                  <td>{c.credentials ?? 0}</td>
                  <td><button className="btn ghost sm" onClick={() => setRoutesCert(c)}>Routes</button></td>
                  <td><button className="btn ghost sm" onClick={() => setEditing(c)}>Edit</button></td>
                  <td>{c.id === 1 ? null : <button className="btn ghost sm" onClick={() => remove(c)}>Delete</button>}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {editing !== undefined && <Editor initial={editing} onClose={() => setEditing(undefined)} onSaved={() => { setEditing(undefined); refetch() }} />}
      {routesCert && <RoutesDrawer cert={routesCert} onClose={() => setRoutesCert(null)} />}
    </div>
  )
}
