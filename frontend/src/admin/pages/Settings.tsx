import { useState, useMemo } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type Settings as SettingsMap } from '../api'
import { Card, Spinner, ErrorNote } from '../../components/ui'
import { titleCase } from '../../format'

const GROUPS: { key: string; label: string; match: (k: string) => boolean }[] = [
  { key: 'web', label: 'Website', match: (k) => k.startsWith('web_') || k.startsWith('site_') },
  { key: 'sp', label: 'Student panel', match: (k) => k.startsWith('sp_') },
  { key: 'exam', label: 'Live exam', match: (k) => k.startsWith('exam_') || k.startsWith('auto_block') || k.startsWith('critical_') || k.includes('violation') || k.includes('proctor') },
  { key: 'other', label: 'Platform', match: () => true },
]

function labelFor(k: string) {
  return titleCase(k.replace(/^(web_|sp_|exam_|site_)/, ''))
}

export default function Settings() {
  const { data, loading, error, refetch } = useAdminQuery<SettingsMap>('/api/admin/settings')
  const [edits, setEdits] = useState<Record<string, string>>({})
  const [busy, setBusy] = useState(false)
  const [msg, setMsg] = useState<string | null>(null)

  const grouped = useMemo(() => {
    const keys = Object.keys(data ?? {}).sort()
    const out: Record<string, string[]> = {}
    for (const g of GROUPS) out[g.key] = []
    for (const k of keys) {
      const g = GROUPS.find((g) => g.match(k))!
      out[g.key].push(k)
    }
    return out
  }, [data])

  const val = (k: string) => (k in edits ? edits[k] : (data?.[k] ?? ''))

  async function save() {
    if (Object.keys(edits).length === 0) return
    setBusy(true)
    setMsg(null)
    try {
      const res = await adminApi.patch<{ ok: boolean; rejected?: string[] }>('/api/admin/settings', edits)
      setEdits({})
      refetch()
      setMsg(res.rejected && res.rejected.length > 0 ? `Saved. Some settings were not permitted: ${res.rejected.join(', ')}` : 'Settings saved.')
    } catch (e) {
      setMsg(e instanceof Error ? e.message : 'Could not save.')
    } finally {
      setBusy(false)
    }
  }

  if (loading) return <Spinner />
  if (error) return <ErrorNote>{error}</ErrorNote>

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Settings</h1>
        <button className="btn sm" disabled={busy || Object.keys(edits).length === 0} onClick={save}>{busy ? 'Saving…' : `Save changes${Object.keys(edits).length ? ` (${Object.keys(edits).length})` : ''}`}</button>
      </div>
      {msg && <div className={'notice' + (msg.includes('not permitted') || msg.includes('Could not') ? ' warn' : '')}>{msg}</div>}
      <p className="muted small">Configuration is stored as key/value pairs. Some keys are restricted to specific roles; the server will report any it does not permit you to change.</p>

      {GROUPS.map((g) =>
        grouped[g.key].length === 0 ? null : (
          <Card key={g.key} title={g.label}>
            <div className="grid cols-2">
              {grouped[g.key].map((k) => {
                const v = val(k)
                const long = (v ?? '').length > 80
                return (
                  <div className="field" key={k} style={long ? { gridColumn: '1 / -1' } : undefined}>
                    <label title={k}>{labelFor(k)}</label>
                    {long ? (
                      <textarea rows={2} value={v} onChange={(e) => setEdits({ ...edits, [k]: e.target.value })} />
                    ) : (
                      <input value={v} onChange={(e) => setEdits({ ...edits, [k]: e.target.value })} />
                    )}
                  </div>
                )
              })}
            </div>
          </Card>
        ),
      )}
    </div>
  )
}
