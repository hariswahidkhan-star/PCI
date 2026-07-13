import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDate } from '../../format'

// Admin Console → Training Partners (Phase 7). Two tabs:
//   Directory   — CRUD over published/unpublished partner entries; publishing (listed) renders them
//                 on the public website via the PCI-PARTNERS marker.
//   Applications — review public "become a partner" submissions; approving creates a directory entry
//                 (unlisted) which you then publish.
// Certification stays independent of training — partners deliver exam preparation only.

interface Partner {
  id: number; name: string; slug?: string | null; tier: string
  country?: string | null; region?: string | null; city?: string | null
  website?: string | null; logo_url?: string | null; summary?: string | null
  description?: string | null; specialties?: string | null; contact_email?: string | null
  listed: number; sort_order?: number | null; created_at?: string | null
}
interface TPApp {
  id: number; reference: string; org_name?: string | null; website?: string | null
  contact_name?: string | null; contact_email?: string | null; contact_phone?: string | null
  country?: string | null; city?: string | null; region?: string | null
  delivery_modes?: string | null; specialties?: string | null; learners_per_year?: number | null
  description?: string | null; status: string; proposed_tier?: string | null
  partner_id?: number | null; admin_note?: string | null; created_at?: string | null; doc_count?: number | null
}
interface TPDoc { id: number; doc_kind: string; filename?: string | null; mime?: string | null; size_bytes?: number | null }

const TIERS = ['registered', 'authorized', 'premier'] as const
const TIER_TONE: Record<string, 'ok' | 'warn' | 'brand'> = { premier: 'brand', authorized: 'warn', registered: 'ok' }
const STATUS_TONE: Record<string, 'ok' | 'err' | 'brand' | 'warn'> = { approved: 'ok', pending_review: 'brand', under_review: 'warn', rejected: 'err' }
const DOC_LABEL: Record<string, string> = { accreditation: 'Accreditation', company_profile: 'Company profile', curriculum: 'Curriculum', supporting: 'Supporting document' }
const TABS = ['Directory', 'Applications'] as const

export default function TrainingPartners() {
  const [tab, setTab] = useState<(typeof TABS)[number]>('Directory')
  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Training Partners</h1>
        <p className="muted">Recognised organisations that deliver PCP-AI examination preparation. Certification stays independent of training — partners prepare candidates; PCI owns the examination and the certification decision.</p>
      </div>
      <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
        {TABS.map((t) => <button key={t} className={'btn sm' + (tab === t ? '' : ' ghost')} onClick={() => setTab(t)}>{t}</button>)}
      </div>
      {tab === 'Directory' && <DirectoryTab />}
      {tab === 'Applications' && <ApplicationsTab />}
    </div>
  )
}

// ---------------- Directory (CRUD) ----------------
function DirectoryTab() {
  const { data, loading, error, refetch } = useAdminQuery<{ rows: Partner[] }>('/api/admin/training-partners')
  const [edit, setEdit] = useState<Partner | 'new' | null>(null)
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const rows = data?.rows ?? []

  async function togglePublish(p: Partner) {
    setErr(null)
    try { await adminApi.patch(`/api/admin/training-partners/${p.id}`, { listed: !p.listed }); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Update failed.') }
  }
  async function del(p: Partner) {
    if (!confirm(`Delete “${p.name}” from the directory?`)) return
    setErr(null)
    try { await adminApi.post(`/api/admin/training-partners/${p.id}/delete`); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Delete failed.') }
  }

  return (
    <Card title={`Partner directory (${rows.length})`} action={<button className="btn sm" onClick={() => setEdit('new')}>Add partner</button>}>
      {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
      {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
        <Empty>No partners yet. Add one, or approve an application.</Empty>
      ) : (
        <table className="data">
          <thead><tr><th>Name</th><th>Tier</th><th>Location</th><th>Published</th><th /></tr></thead>
          <tbody>
            {rows.map((p) => (
              <tr key={p.id}>
                <td><strong>{p.name}</strong>{p.website && <div className="muted small">{p.website}</div>}</td>
                <td><Badge tone={TIER_TONE[p.tier] ?? 'ok'}>{p.tier}</Badge></td>
                <td className="small">{[p.city, p.region, p.country].filter(Boolean).join(', ') || '—'}</td>
                <td>{p.listed ? <Badge tone="ok">published</Badge> : <Badge tone="warn">draft</Badge>}</td>
                <td className="row" style={{ gap: '.3rem', justifyContent: 'flex-end' }}>
                  <button className="btn ghost sm" onClick={() => setEdit(p)}>Edit</button>
                  <button className="btn ghost sm" onClick={() => togglePublish(p)}>{p.listed ? 'Unpublish' : 'Publish'}</button>
                  <button className="btn ghost sm danger" onClick={() => del(p)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {edit && <PartnerEditor partner={edit === 'new' ? null : edit} busy={busy} setBusy={setBusy}
        onClose={() => setEdit(null)} onSaved={() => { setEdit(null); refetch() }} />}
    </Card>
  )
}

function PartnerEditor({ partner, busy, setBusy, onClose, onSaved }:
  { partner: Partner | null; busy: boolean; setBusy: (b: boolean) => void; onClose: () => void; onSaved: () => void }) {
  const [f, setF] = useState<Partial<Partner>>(partner ?? { tier: 'registered', listed: 0 })
  const [err, setErr] = useState<string | null>(null)
  const set = (k: keyof Partner) => (e: { target: { value: string } }) => setF({ ...f, [k]: e.target.value })
  async function save() {
    if (!(f.name ?? '').trim()) { setErr('A name is required.'); return }
    setBusy(true); setErr(null)
    const body = {
      name: f.name, tier: f.tier, country: f.country ?? '', region: f.region ?? '', city: f.city ?? '',
      website: f.website ?? '', logo_url: f.logo_url ?? '', summary: f.summary ?? '',
      description: f.description ?? '', specialties: f.specialties ?? '', contact_email: f.contact_email ?? '',
      listed: !!f.listed, sort_order: Number(f.sort_order ?? 0),
    }
    try {
      if (partner) await adminApi.patch(`/api/admin/training-partners/${partner.id}`, body)
      else await adminApi.post('/api/admin/training-partners', body)
      onSaved()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Save failed.') } finally { setBusy(false) }
  }
  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{partner ? 'Edit partner' : 'Add partner'}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        <div style={{ display: 'grid', gap: '.55rem' }}>
          <label>Organisation name *<input value={f.name ?? ''} onChange={set('name')} /></label>
          <label>Tier
            <select value={f.tier ?? 'registered'} onChange={set('tier')}>
              {TIERS.map((t) => <option key={t} value={t}>{t}</option>)}
            </select>
          </label>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: 'repeat(auto-fit,minmax(150px,1fr))' }}>
            <label>City<input value={f.city ?? ''} onChange={set('city')} /></label>
            <label>Region<input value={f.region ?? ''} onChange={set('region')} /></label>
            <label>Country<input value={f.country ?? ''} onChange={set('country')} /></label>
          </div>
          <label>Website<input value={f.website ?? ''} onChange={set('website')} placeholder="https://…" /></label>
          <label>Logo URL<input value={f.logo_url ?? ''} onChange={set('logo_url')} placeholder="https://…/logo.png" /></label>
          <label>Summary <span className="muted small">(one line, shown on the directory card)</span><input value={f.summary ?? ''} onChange={set('summary')} /></label>
          <label>Specialties <span className="muted small">(comma or newline separated)</span><textarea rows={2} value={f.specialties ?? ''} onChange={set('specialties')} /></label>
          <label>Description<textarea rows={4} value={f.description ?? ''} onChange={set('description')} /></label>
          <label>Contact email<input value={f.contact_email ?? ''} onChange={set('contact_email')} /></label>
          <div style={{ display: 'grid', gap: '.55rem', gridTemplateColumns: '1fr 1fr' }}>
            <label>Sort order<input type="number" value={String(f.sort_order ?? 0)} onChange={set('sort_order')} /></label>
            <label className="row" style={{ gap: '.4rem', alignItems: 'center', marginTop: '1.5rem' }}>
              <input type="checkbox" checked={!!f.listed} onChange={(e) => setF({ ...f, listed: e.target.checked ? 1 : 0 })} style={{ width: 'auto' }} /> Published to public directory
            </label>
          </div>
        </div>
        <div className="row" style={{ gap: '.5rem', marginTop: '.8rem' }}>
          <button className="btn" disabled={busy} onClick={save}>{busy ? 'Saving…' : 'Save'}</button>
          <button className="btn ghost" onClick={onClose}>Cancel</button>
        </div>
      </div>
    </div>
  )
}

// ---------------- Applications (review) ----------------
function ApplicationsTab() {
  const [status, setStatus] = useState('pending_review')
  const { data, loading, error, refetch } = useAdminQuery<{ rows: TPApp[] }>(`/api/admin/training-partner-applications${status ? `?status=${status}` : ''}`)
  const [open, setOpen] = useState<{ application: TPApp; documents: TPDoc[] } | null>(null)
  const [note, setNote] = useState('')
  const [tier, setTier] = useState<string>('registered')
  const [busy, setBusy] = useState(false)
  const [err, setErr] = useState<string | null>(null)
  const rows = data?.rows ?? []

  async function view(id: number) {
    setErr(null)
    try {
      const d = await adminApi.get<{ application: TPApp; documents: TPDoc[] }>(`/api/admin/training-partner-applications/${id}`)
      setOpen(d); setNote(d.application.admin_note ?? ''); setTier(d.application.proposed_tier || 'registered')
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not load the application.') }
  }
  async function download(appId: number, docId: number) {
    setErr(null)
    try {
      const res = await fetch(`/api/admin/training-partner-applications/${appId}/documents/${docId}/file`, { headers: { Authorization: 'Bearer ' + (adminApi.getToken() ?? '') } })
      if (!res.ok) throw new Error('Could not load the file.')
      const url = URL.createObjectURL(await res.blob())
      window.open(url, '_blank', 'noopener'); setTimeout(() => URL.revokeObjectURL(url), 60_000)
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not load the file.') }
  }
  async function decide(id: number, s: 'approved' | 'rejected' | 'under_review' | '') {
    setBusy(true); setErr(null)
    try { await adminApi.post(`/api/admin/training-partner-applications/${id}/decide`, { status: s, admin_note: note, tier }); setOpen(null); setNote(''); refetch() }
    catch (e) { setErr(e instanceof Error ? e.message : 'Decision failed.') } finally { setBusy(false) }
  }

  return (
    <>
      <Card title="Applications" action={
        <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ maxWidth: 200 }} aria-label="Status filter">
          <option value="pending_review">Pending review</option><option value="under_review">Under review</option>
          <option value="approved">Approved</option><option value="rejected">Rejected</option><option value="">All</option>
        </select>
      }>
        {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
        {loading ? <Spinner /> : error ? <ErrorNote>{error}</ErrorNote> : rows.length === 0 ? (
          <Empty>No applications with this status.</Empty>
        ) : (
          <table className="data">
            <thead><tr><th>Organisation</th><th>Contact</th><th>Country</th><th>Docs</th><th>Submitted</th><th>Status</th><th /></tr></thead>
            <tbody>
              {rows.map((a) => (
                <tr key={a.id}>
                  <td><strong>{a.org_name || '—'}</strong><div className="muted small">{a.reference}</div></td>
                  <td className="small">{a.contact_name || '—'}<div className="muted">{a.contact_email || ''}</div></td>
                  <td className="small">{a.country || '—'}</td>
                  <td>{a.doc_count ?? 0}</td>
                  <td className="small muted">{fmtDate(a.created_at)}</td>
                  <td><Badge tone={STATUS_TONE[a.status] ?? 'brand'}>{a.status.replace(/_/g, ' ')}</Badge></td>
                  <td><button className="btn ghost sm" onClick={() => view(a.id)}>Review</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {open && (
        <div className="drawer-backdrop" onClick={() => setOpen(null)}>
          <div className="drawer" onClick={(e) => e.stopPropagation()}>
            <div className="spread" style={{ marginBottom: '1rem' }}>
              <h2 style={{ margin: 0 }}>{open.application.org_name}</h2>
              <button className="btn secondary sm" onClick={() => setOpen(null)}>Close</button>
            </div>
            {err && <div className="notice err" role="alert" style={{ marginBottom: '.6rem' }}>{err}</div>}
            <div className="spread" style={{ marginBottom: '.75rem' }}>
              <Badge tone={STATUS_TONE[open.application.status] ?? 'brand'}>{open.application.status.replace(/_/g, ' ')}</Badge>
              <span className="muted small">{open.application.reference}{open.application.partner_id ? ` · partner #${open.application.partner_id}` : ''}</span>
            </div>

            <Section title="Contact">
              <KV k="Contact name" v={open.application.contact_name} />
              <KV k="Email" v={open.application.contact_email} />
              <KV k="Phone" v={open.application.contact_phone} />
              <KV k="Website" v={open.application.website} />
              <KV k="Location" v={[open.application.city, open.application.region, open.application.country].filter(Boolean).join(', ')} />
            </Section>
            <Section title="Programme">
              <KV k="Delivery modes" v={open.application.delivery_modes} />
              <KV k="Learners / year" v={open.application.learners_per_year != null ? String(open.application.learners_per_year) : null} />
              <KV k="Specialties" v={open.application.specialties} block />
              <KV k="About" v={open.application.description} block />
            </Section>
            <Section title={`Documents (${open.documents.length})`}>
              {open.documents.length === 0 ? <p className="muted small">No documents attached.</p> : (
                <ul className="clean" style={{ display: 'grid', gap: '.4rem', paddingLeft: 0, listStyle: 'none' }}>
                  {open.documents.map((d) => (
                    <li key={d.id} className="row" style={{ justifyContent: 'space-between' }}>
                      <span className="small">{DOC_LABEL[d.doc_kind] || d.doc_kind}{d.filename ? ` — ${d.filename}` : ''}</span>
                      <button className="btn ghost sm" onClick={() => download(open.application.id, d.id)}>Download</button>
                    </li>
                  ))}
                </ul>
              )}
            </Section>

            <div className="field" style={{ marginTop: '.5rem' }}>
              <label htmlFor="tp-tier">Tier to grant on approval</label>
              <select id="tp-tier" value={tier} onChange={(e) => setTier(e.target.value)} style={{ maxWidth: 220 }}>
                {TIERS.map((t) => <option key={t} value={t}>{t}</option>)}
              </select>
            </div>
            <div className="field" style={{ marginTop: '.5rem' }}>
              <label htmlFor="tp-note">Internal note / decision note</label>
              <textarea id="tp-note" rows={3} value={note} onChange={(e) => setNote(e.target.value)} placeholder="Recorded on the application; sent to the applicant on a decision." />
            </div>
            <div className="row" style={{ gap: '.4rem', flexWrap: 'wrap' }}>
              <button className="btn sm secondary" disabled={busy} onClick={() => decide(open.application.id, '')}>Save note</button>
              {open.application.status !== 'under_review' && open.application.status !== 'approved' && (
                <button className="btn sm secondary" disabled={busy} onClick={() => decide(open.application.id, 'under_review')}>Mark under review</button>
              )}
              {open.application.status !== 'approved' && (
                <button className="btn sm" disabled={busy} onClick={() => decide(open.application.id, 'approved')}>Approve &amp; create entry</button>
              )}
              {open.application.status !== 'rejected' && open.application.status !== 'approved' && (
                <button className="btn sm danger" disabled={busy} onClick={() => decide(open.application.id, 'rejected')}>Reject</button>
              )}
            </div>
            {open.application.status === 'approved' && open.application.partner_id && (
              <p className="muted small" style={{ marginTop: '.6rem' }}>A directory entry was created (unlisted). Publish it under the <strong>Directory</strong> tab to show it on the public site.</p>
            )}
          </div>
        </div>
      )}
    </>
  )
}

function Section({ title, children }: { title: string; children: React.ReactNode }) {
  return <div style={{ marginBottom: '1rem' }}><h4 style={{ margin: '0 0 .4rem' }}>{title}</h4><div style={{ display: 'grid', gap: '.3rem' }}>{children}</div></div>
}
function KV({ k, v, block }: { k: string; v?: string | null; block?: boolean }) {
  if (block) return <div><div className="muted small">{k}</div><div className="small" style={{ whiteSpace: 'pre-wrap' }}>{v || '—'}</div></div>
  return <div className="spread small"><span className="muted">{k}</span><span style={{ textAlign: 'right' }}>{v || '—'}</span></div>
}
