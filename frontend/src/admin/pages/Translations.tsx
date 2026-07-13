import { useEffect, useMemo, useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi } from '../api'
import { Card, Badge, Spinner, ErrorNote, Empty } from '../../components/ui'

// Backend-owned multilingual control. All translation state lives in content_i18n and is managed here:
// coverage per language, a configurable machine-translation provider, one-click auto-translate, and a
// per-page inline editor. Owner-only.

interface Coverage {
  coverage: { total: number; langs: Record<string, { done: number; total: number }> }
  languages: { code: string; name: string }[]
  provider: { provider: string; model: string; endpoint: string; has_key: boolean; configured: boolean }
  pages: string[]
}
interface RegionRow {
  scope: string; slug: string; ckey: string; ctype: string; english: string; translation: string | null
}

export default function Translations() {
  const { data, loading, error, refetch } = useAdminQuery<Coverage>('/api/admin/i18n/coverage')
  const [busy, setBusy] = useState<string | null>(null)
  const [note, setNote] = useState<string | null>(null)

  if (loading && !data) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>
  if (!data) return null
  const { coverage, languages, provider, pages } = data

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div>
        <h1>Translations</h1>
        <p className="muted">Control the public website’s languages from the backend — translations are stored in the database, machine-translated with your configured provider, and editable by hand. English is always the source.</p>
      </div>
      {note && <div className="notice" role="status">{note}</div>}

      <ProviderCard provider={provider} onSaved={() => { setNote('Provider settings saved.'); refetch() }} />

      <Card title="Coverage & auto-translate">
        <p className="muted small" style={{ marginTop: 0 }}>{coverage.total.toLocaleString()} translatable strings across the public site.</p>
        <table className="data">
          <thead><tr><th>Language</th><th>Progress</th><th style={{ width: 90 }}>Done</th><th></th></tr></thead>
          <tbody>
            {languages.map((l) => {
              const c = coverage.langs[l.code] || { done: 0, total: coverage.total }
              const pct = c.total ? Math.round((c.done / c.total) * 100) : 0
              return (
                <tr key={l.code}>
                  <td><strong>{l.name}</strong> <span className="muted small">({l.code})</span></td>
                  <td>
                    <div style={{ background: 'var(--mist,#eef2f7)', borderRadius: 6, height: 8, overflow: 'hidden', minWidth: 160 }}>
                      <div style={{ width: pct + '%', height: '100%', background: pct === 100 ? 'var(--ok,#16a34a)' : 'var(--brand,#1d4ed8)' }} />
                    </div>
                  </td>
                  <td>{pct}%<div className="muted small">{c.done.toLocaleString()}/{c.total.toLocaleString()}</div></td>
                  <td className="row" style={{ gap: '.4rem', justifyContent: 'flex-end' }}>
                    <button className="btn sm" disabled={busy !== null || !provider.configured}
                      title={provider.configured ? 'Machine-translate every untranslated string' : 'Configure a provider first'}
                      onClick={() => autoTranslate(l.code, l.name)}>
                      {busy === 'tr:' + l.code ? 'Translating…' : 'Auto-translate'}
                    </button>
                    <button className="btn ghost sm" disabled={busy !== null || c.done === 0}
                      onClick={() => clearLang(l.code, l.name)}>Clear</button>
                  </td>
                </tr>
              )
            })}
          </tbody>
        </table>
        {!provider.configured && <p className="muted small">Auto-translate is disabled until a provider and API key are set above. You can still translate by hand in the editor below.</p>}
      </Card>

      <PageEditor pages={pages} languages={languages} onChanged={refetch} />
    </div>
  )

  async function autoTranslate(lang: string, name: string) {
    setBusy('tr:' + lang); setNote(null)
    try {
      let total = 0, guard = 0
      // The endpoint translates up to `limit` strings per call and reports how many remain, so loop.
      for (;;) {
        const r = await adminApi.post<{ translated: number; remaining: number }>('/api/admin/i18n/translate', { lang, limit: 200 })
        total += r.translated
        setNote(`${name}: translated ${total.toLocaleString()} — ${r.remaining.toLocaleString()} remaining…`)
        if (r.remaining <= 0 || r.translated === 0 || ++guard > 200) break
      }
      setNote(`${name}: auto-translate complete (${total.toLocaleString()} strings).`)
      refetch()
    } catch (e) {
      setNote(`${name}: ${(e as Error).message || 'translation failed'}`)
    } finally { setBusy(null) }
  }

  async function clearLang(lang: string, name: string) {
    if (!confirm(`Remove all ${name} translations? This cannot be undone.`)) return
    setBusy('cl:' + lang); setNote(null)
    try { await adminApi.post('/api/admin/i18n/clear', { lang }); setNote(`${name} translations cleared.`); refetch() }
    catch (e) { setNote((e as Error).message) } finally { setBusy(null) }
  }
}

function ProviderCard({ provider, onSaved }: { provider: Coverage['provider']; onSaved: () => void }) {
  const [prov, setProv] = useState(provider.provider || '')
  const [model, setModel] = useState(provider.model || '')
  const [endpoint, setEndpoint] = useState(provider.endpoint || '')
  const [key, setKey] = useState('')
  const [saving, setSaving] = useState(false)
  const [err, setErr] = useState<string | null>(null)

  async function save() {
    setSaving(true); setErr(null)
    try {
      await adminApi.post('/api/admin/i18n/config', { provider: prov, model, endpoint, ...(key ? { api_key: key } : {}) })
      setKey('')
      onSaved()
    } catch (e) { setErr((e as Error).message) } finally { setSaving(false) }
  }

  return (
    <Card title="Translation provider" action={provider.configured ? <Badge tone="ok">Configured</Badge> : <Badge tone="warn">Not configured</Badge>}>
      <p className="muted small" style={{ marginTop: 0 }}>Set a machine-translation provider for auto-translate. The key is stored server-side and never shown again. Leave blank to translate manually only.</p>
      <div className="grid" style={{ display: 'grid', gap: '.6rem', gridTemplateColumns: 'repeat(auto-fit,minmax(200px,1fr))' }}>
        <label>Provider
          <select value={prov} onChange={(e) => setProv(e.target.value)}>
            <option value="">None (manual only)</option>
            <option value="anthropic">Anthropic</option>
            <option value="openai">OpenAI</option>
            <option value="custom">Custom (OpenAI-compatible)</option>
          </select>
        </label>
        <label>Model <input value={model} placeholder="e.g. claude-sonnet-5" onChange={(e) => setModel(e.target.value)} /></label>
        <label>API key <input type="password" value={key} placeholder={provider.has_key ? '•••••• (set — leave blank to keep)' : 'paste key'} onChange={(e) => setKey(e.target.value)} /></label>
        {prov === 'custom' && <label>Endpoint <input value={endpoint} placeholder="https://…" onChange={(e) => setEndpoint(e.target.value)} /></label>}
      </div>
      {err && <div className="notice err" style={{ marginTop: '.6rem' }}>{err}</div>}
      <div style={{ marginTop: '.7rem' }}><button className="btn" disabled={saving} onClick={save}>{saving ? 'Saving…' : 'Save provider'}</button></div>
    </Card>
  )
}

function PageEditor({ pages, languages, onChanged }: { pages: string[]; languages: { code: string; name: string }[]; onChanged: () => void }) {
  const [slug, setSlug] = useState(pages[0] || '')
  const [lang, setLang] = useState(languages[0]?.code || 'ko')
  const path = useMemo(() => (slug && lang ? `/api/admin/i18n/regions?slug=${encodeURIComponent(slug)}&lang=${lang}` : null), [slug, lang])
  const { data, loading, error, refetch } = useAdminQuery<{ rows: RegionRow[] }>(path)
  const [edits, setEdits] = useState<Record<string, string>>({})
  const [savingKey, setSavingKey] = useState<string | null>(null)
  useEffect(() => { setEdits({}) }, [slug, lang])

  async function saveRow(r: RegionRow) {
    const k = r.scope + r.slug + r.ckey
    const val = edits[k] ?? r.translation ?? ''
    setSavingKey(k)
    try {
      await adminApi.post('/api/admin/i18n/set', { lang, scope: r.scope, slug: r.slug, ckey: r.ckey, cvalue: val })
      await refetch(); onChanged()
    } finally { setSavingKey(null) }
  }

  return (
    <Card title="Edit translations by page">
      <div className="row" style={{ gap: '.6rem', marginBottom: '.8rem', flexWrap: 'wrap' }}>
        <label>Page <select value={slug} onChange={(e) => setSlug(e.target.value)}>{pages.map((p) => <option key={p} value={p}>{p}</option>)}</select></label>
        <label>Language <select value={lang} onChange={(e) => setLang(e.target.value)}>{languages.map((l) => <option key={l.code} value={l.code}>{l.name}</option>)}</select></label>
      </div>
      {loading && <Spinner />}
      {error && <ErrorNote>{error}</ErrorNote>}
      {data && (data.rows.length === 0 ? <Empty>No translatable regions on this page.</Empty> : (
        <table className="data">
          <thead><tr><th>English</th><th>Translation</th><th style={{ width: 80 }}></th></tr></thead>
          <tbody>
            {data.rows.map((r) => {
              const k = r.scope + r.slug + r.ckey
              const cur = edits[k] ?? r.translation ?? ''
              const dirty = cur !== (r.translation ?? '')
              return (
                <tr key={k}>
                  <td style={{ maxWidth: 360 }}><span className="small">{r.english}</span>{r.ctype === 'html' && <> <Badge tone="brand">HTML</Badge></>}</td>
                  <td><textarea rows={cur.length > 60 ? 3 : 1} style={{ width: '100%', minWidth: 220 }} value={cur} placeholder="—" onChange={(e) => setEdits({ ...edits, [k]: e.target.value })} /></td>
                  <td><button className="btn sm" disabled={!dirty || savingKey === k} onClick={() => saveRow(r)}>{savingKey === k ? '…' : 'Save'}</button></td>
                </tr>
              )
            })}
          </tbody>
        </table>
      ))}
    </Card>
  )
}
