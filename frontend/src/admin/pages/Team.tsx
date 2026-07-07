import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type TeamMember, type TeamResponse } from '../api'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { fmtDateTime, titleCase } from '../../format'

function PermissionPicker({ sections, roleGrants, role, permissions, onChange }: {
  sections: string[]; roleGrants: Record<string, string[]>; role: string; permissions: string[]; onChange: (p: string[]) => void
}) {
  if (role !== 'custom') {
    const granted = roleGrants[role] ?? []
    return (
      <div className="small muted">
        The <strong>{role}</strong> role grants:{' '}
        {granted.length === 0 ? 'no sections' : granted.length >= sections.length ? 'all sections' : granted.map(titleCase).join(', ')}
      </div>
    )
  }
  const toggle = (s: string) => onChange(permissions.includes(s) ? permissions.filter((x) => x !== s) : [...permissions, s])
  return (
    <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(150px,1fr))', gap: '.35rem' }}>
      {sections.map((s) => (
        <label key={s} className="row small" style={{ fontWeight: 400, gap: '.4rem' }}>
          <input type="checkbox" style={{ width: 'auto' }} checked={permissions.includes(s)} onChange={() => toggle(s)} /> {titleCase(s)}
        </label>
      ))}
    </div>
  )
}

function Editor({ member, meta, onClose, onSaved }: { member: TeamMember | null; meta: TeamResponse; onClose: () => void; onSaved: () => void }) {
  const isNew = !member
  const [email, setEmail] = useState(member?.email ?? '')
  const [name, setName] = useState(member?.name ?? '')
  const [role, setRole] = useState(member?.role ?? 'viewer')
  const [permissions, setPermissions] = useState<string[]>(member?.permissions ?? [])
  const [status, setStatus] = useState(member?.status ?? 'active')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [tempPw, setTempPw] = useState<string | null>(null)

  async function save() {
    setBusy(true)
    setError(null)
    try {
      if (isNew) {
        const res = await adminApi.post<{ ok: boolean; temp_password: string }>('/api/admin/team', { email: email.trim().toLowerCase(), name, role, permissions })
        setTempPw(res.temp_password)
        onSaved()
      } else {
        await adminApi.patch(`/api/admin/team/${member!.id}`, { name, role, permissions, status })
        onSaved()
        onClose()
      }
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Could not save.')
    } finally {
      setBusy(false)
    }
  }

  async function resetPw() {
    if (!member) return
    try {
      const res = await adminApi.post<{ temp_password: string }>(`/api/admin/team/${member.id}/reset-password`)
      setTempPw(res.temp_password)
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Could not reset.')
    }
  }

  return (
    <div className="drawer-backdrop" onClick={onClose}>
      <div className="drawer" onClick={(e) => e.stopPropagation()}>
        <div className="spread" style={{ marginBottom: '1rem' }}>
          <h2 style={{ margin: 0 }}>{isNew ? 'Add team member' : 'Edit team member'}</h2>
          <button className="btn secondary sm" onClick={onClose}>Close</button>
        </div>
        {error && <div className="notice err" role="alert" style={{ marginBottom: '1rem' }}>{error}</div>}
        {tempPw && (
          <div className="notice" style={{ marginBottom: '1rem' }}>
            Temporary password: <strong>{tempPw}</strong> — share it securely; they’ll be asked to change it at first sign-in.
          </div>
        )}

        <div className="field"><label>Email</label><input type="email" value={email} disabled={!isNew} onChange={(e) => setEmail(e.target.value)} /></div>
        <div className="field"><label>Name</label><input value={name} onChange={(e) => setName(e.target.value)} /></div>
        <div className="field"><label>Role</label>
          <select value={role} onChange={(e) => setRole(e.target.value)}>
            {meta.roles.map((r) => <option key={r} value={r}>{titleCase(r)}</option>)}
          </select>
        </div>
        {!isNew && (
          <div className="field"><label>Status</label>
            <select value={status} onChange={(e) => setStatus(e.target.value)}>
              <option value="active">Active</option>
              <option value="suspended">Suspended</option>
            </select>
          </div>
        )}
        <div className="field">
          <label>Permissions</label>
          <PermissionPicker sections={meta.sections} roleGrants={meta.role_grants} role={role} permissions={permissions} onChange={setPermissions} />
        </div>

        <div className="row" style={{ marginTop: '.5rem', flexWrap: 'wrap' }}>
          <button className="btn" disabled={busy || (isNew && !email)} onClick={save}>{busy ? 'Saving…' : isNew ? 'Create member' : 'Save changes'}</button>
          {!isNew && <button className="btn secondary sm" onClick={resetPw}>Reset password</button>}
        </div>
      </div>
    </div>
  )
}

export default function Team() {
  const { data, loading, error, refetch } = useAdminQuery<TeamResponse>('/api/admin/team')
  const [editing, setEditing] = useState<TeamMember | null | undefined>(undefined)

  async function remove(m: TeamMember) {
    if (!confirm(`Remove ${m.email} from the team?`)) return
    try {
      await adminApi.del(`/api/admin/team/${m.id}`)
      refetch()
    } catch (e) {
      alert(e instanceof Error ? e.message : 'Could not remove.')
    }
  }

  return (
    <div className="stack" style={{ display: 'grid', gap: '1rem' }}>
      <div className="spread">
        <h1>Team &amp; Access</h1>
        {data && <button className="btn sm" onClick={() => setEditing(null)}>Add member</button>}
      </div>

      <Card>
        {loading ? (
          <Spinner />
        ) : error ? (
          <ErrorNote>{error}</ErrorNote>
        ) : !data || data.rows.length === 0 ? (
          <Empty>No team members.</Empty>
        ) : (
          <table className="data">
            <thead>
              <tr><th>Email</th><th>Name</th><th>Role</th><th>Access</th><th>Last login</th><th>Status</th><th></th><th></th></tr>
            </thead>
            <tbody>
              {data.rows.map((m) => (
                <tr key={m.id}>
                  <td><strong>{m.email}</strong></td>
                  <td>{m.name || '—'}</td>
                  <td>{titleCase(m.role)}{m.role === 'owner' && <> <Badge tone="brand">owner</Badge></>}</td>
                  <td className="small muted">{m.effective.length >= data.sections.length ? 'All' : `${m.effective.length} section${m.effective.length === 1 ? '' : 's'}`}</td>
                  <td className="small muted">{m.last_login_at ? fmtDateTime(m.last_login_at) : 'Never'}</td>
                  <td><StatusBadge status={m.status} /></td>
                  <td><button className="btn ghost sm" onClick={() => setEditing(m)}>Edit</button></td>
                  <td>{m.role !== 'owner' && <button className="btn ghost sm" onClick={() => remove(m)}>Remove</button>}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </Card>

      {editing !== undefined && data && (
        <Editor member={editing} meta={data} onClose={() => setEditing(undefined)} onSaved={refetch} />
      )}
    </div>
  )
}
