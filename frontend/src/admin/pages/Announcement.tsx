import { useEffect, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Spinner, ErrorNote } from '../../components/ui'

/** Every field the public announcement modal renders. `key` is the suffix stored as `announce_<key>`
 * in site_settings and sent (without the prefix) on save; the backend resolves the {date} token. */
const FIELDS: { key: string; label: string; hint?: string; long?: boolean }[] = [
  { key: 'date', label: 'Launch date', hint: 'Shown wherever {date} appears in the text below — change it here in one place.' },
  { key: 'eyebrow', label: 'Eyebrow (small heading)' },
  { key: 'title', label: 'Title', hint: 'You can use {date}.', long: true },
  { key: 'subtitle', label: 'Subtitle' },
  { key: 'intro', label: 'Intro paragraph', hint: 'You can use {date}.', long: true },
  { key: 'p1_title', label: 'Point 1 — bold lead' },
  { key: 'p1_text', label: 'Point 1 — text', long: true },
  { key: 'p2_title', label: 'Point 2 — bold lead' },
  { key: 'p2_text', label: 'Point 2 — text', long: true },
  { key: 'p3_title', label: 'Point 3 — bold lead' },
  { key: 'p3_text', label: 'Point 3 — text', long: true },
  { key: 'note', label: 'Legal / fine-print note', long: true },
  { key: 'cta_label', label: 'Button label' },
  { key: 'cta_href', label: 'Button link', hint: 'A page on the site (e.g. honorary-application.html) or a full https:// URL.' },
  { key: 'dismiss', label: 'Dismiss button label', hint: 'The secondary “Continue browsing” action.' },
  { key: 'version', label: 'Version tag', hint: 'Change this (or the date) to re-show the notice to visitors who already dismissed it.' },
]

type Cfg = Record<string, string | boolean>

/** Admin editor for the public site-wide announcement modal: show/hide, launch date and every line
 * of copy. Empty a field to fall back to its built-in default. Saved via /api/admin/announcement. */
export default function Announcement() {
  const { data, loading, error, refetch } = useAdminQuery<Cfg>('/api/admin/announcement')
  const [form, setForm] = useState<Record<string, string>>({})
  const [enabled, setEnabled] = useState(true)
  const [msg, setMsg] = useState('')
  const [err, setErr] = useState('')
  const [busy, setBusy] = useState(false)

  useEffect(() => {
    if (!data) return
    const f: Record<string, string> = {}
    for (const { key } of FIELDS) f[key] = String(data['announce_' + key] ?? '')
    setForm(f)
    setEnabled(Boolean(data['announce_enabled']))
  }, [data])

  const set = (k: string, v: string) => setForm((p) => ({ ...p, [k]: v }))

  const save = async (nextEnabled?: boolean) => {
    setBusy(true); setErr(''); setMsg('')
    try {
      const on = nextEnabled ?? enabled
      await adminApi.post('/api/admin/announcement', { ...form, announce_enabled: on })
      setEnabled(on)
      setMsg(nextEnabled === false ? 'Announcement hidden.' : nextEnabled === true ? 'Announcement shown.' : 'Saved.')
      refetch()
    } catch (e) { setErr(e instanceof Error ? e.message : 'Could not save.') }
    finally { setBusy(false) }
  }

  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Announcement</h1>
        <p className="muted">The pop-up shown to visitors across the public website. Turn it on or off, change the
          launch date, and edit every line here — no code change needed. The token <code>{'{date}'}</code> is replaced
          with the launch date wherever it appears. Empty a field to use its built-in default.</p>
      </div>
      {msg && <div className="note ok">{msg}</div>}
      {err && <ErrorNote>{err}</ErrorNote>}

      <Card title="Visibility">
        <div className="row" style={{ gap: '.8rem', alignItems: 'center', flexWrap: 'wrap' }}>
          <span>
            Status:{' '}
            <strong style={{ color: enabled ? '#15803d' : '#b45309' }}>
              {enabled ? 'Shown to visitors' : 'Hidden'}
            </strong>
          </span>
          <button className="btn" disabled={busy || enabled} onClick={() => save(true)}>Show</button>
          <button className="btn ghost" disabled={busy || !enabled} onClick={() => save(false)}>Hide</button>
        </div>
      </Card>

      <Card title="Content">
        <div style={{ display: 'grid', gap: '.9rem', maxWidth: 760 }}>
          {FIELDS.map(({ key, label, hint, long }) => (
            <div className="field" key={key}>
              <label>{label}</label>
              {long
                ? <textarea rows={key === 'note' || key === 'intro' ? 4 : 2} value={form[key] ?? ''} onChange={(e) => set(key, e.target.value)} />
                : <input value={form[key] ?? ''} onChange={(e) => set(key, e.target.value)} />}
              {hint && <div className="muted small">{hint}</div>}
            </div>
          ))}
          <div className="row" style={{ gap: '.6rem' }}>
            <button className="btn" disabled={busy} onClick={() => save()}>{busy ? 'Saving…' : 'Save changes'}</button>
          </div>
        </div>
      </Card>
    </div>
  )
}
