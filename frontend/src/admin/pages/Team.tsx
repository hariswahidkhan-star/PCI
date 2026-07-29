import { useState } from 'react'
import { useAdminQuery } from '../hooks'
import { adminApi, type TeamMember, type TeamResponse } from '../api'
import { Card, Badge, StatusBadge, Spinner, ErrorNote, Empty } from '../../components/ui'
import { PageHeader } from '../../components/premium'
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

interface CertOpt { id: number; code: string; acronym?: string | null }

function Editor({ member, meta, onClose, onSaved }: { member: TeamMember | null; meta: TeamResponse; onClose: () => void; onSaved: () => void }) {
  const isNew = !member
  const { data: certData } = useAdminQuery<{ rows: CertOpt[] }>('/api/admin/certifications')
  const [email, setEmail] = useState(member?.email ?? '')
  const [name, setName] = useState(member?.name ?? '')
  const [role, setRole] = useState(member?.role ?? 'viewer')
  const [permissions, setPermissions] = useState<string[]>(member?.permissions ?? [])
  const [certScope, setCertScope] = useState<number[]>(member?.cert_scope ?? [])
  const [status, setStatus] = useState(member?.status ?? 'active')
  const [busy, setBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [tempPw, setTempPw] = useState<string | null>(null)

  const toggleCert = (id: number) => setCertScope(certScope.includes(id) ? certScope.filter((x) => x !== id) : [...certScope, id])

  async function save() {
    setBusy(true)
    setError(null)
    try {
      if (isNew) {
        const res = await adminApi.post<{ ok: boolean; temp_password: string }>('/api/admin/team', { email: email.trim().toLowerCase(), name, role, permissions, cert_scope: certScope })
        setTempPw(res.temp_password)
        onSaved()
      } else {
        await adminApi.patch(`/api/admin/team/${member!.id}`, { name, role, permissions, status, cert_scope: certScope })
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

  async function reset2fa() {
    if (!member) return
    if (!confirm(`Reset two-factor authentication for ${member.email}? They will sign in with their password alone and can re-enrol a new authenticator.`)) return
    try {
      await adminApi.post(`/api/admin/team/${member.id}/reset-2fa`)
      setError(null)
      setTempPw(null)
      alert(`Two-factor authentication has been reset for ${member.email}.`)
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Could not reset 2FA.')
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

        <div className="field"><label htmlFor="team-email">Email</label><input id="team-email" type="email" value={email} disabled={!isNew} onChange={(e) => setEmail(e.target.value)} /></div>
        <div className="field"><label htmlFor="team-name">Name</label><input id="team-name" value={name} onChange={(e) => setName(e.target.value)} /></div>
        <div className="field"><label htmlFor="team-role">Role</label>
          <select id="team-role" value={role} onChange={(e) => setRole(e.target.value)}>
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
        {role !== 'owner' && (
          <div className="field">
            <label>Certification scope</label>
            <p className="muted small" style={{ margin: '0 0 .35rem' }}>
              Restrict this member to specific certifications — their exam sessions, applications, credentials,
              question banks and reports are limited to the ticked credentials. Leave all unticked for access to
              every certification.
            </p>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(150px,1fr))', gap: '.35rem' }}>
              {certData?.rows.map((c) => (
                <label key={c.id} className="row small" style={{ fontWeight: 400, gap: '.4rem' }}>
                  <input type="checkbox" style={{ width: 'auto' }} checked={certScope.includes(c.id)} onChange={() => toggleCert(c.id)} /> {c.acronym || c.code}
                </label>
              ))}
            </div>
          </div>
        )}

        <div className="row" style={{ marginTop: '.5rem', flexWrap: 'wrap' }}>
          <button className="btn" disabled={busy || (isNew && !email)} onClick={save}>{busy ? 'Saving…' : isNew ? 'Create member' : 'Save changes'}</button>
          {!isNew && <button className="btn secondary sm" onClick={resetPw}>Reset password</button>}
          {!isNew && <button className="btn secondary sm" onClick={reset2fa}>Reset 2FA</button>}
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
    <div className="page">
      <PageHeader
        title="Team & Access"
        actions={data && <button className="btn sm" onClick={() => setEditing(null)}>Add member</button>}
      />

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
                  <td className="small muted">
                    {m.effective.length >= data.sections.length ? 'All' : `${m.effective.length} section${m.effective.length === 1 ? '' : 's'}`}
                    {(m.cert_scope?.length ?? 0) > 0 && m.role !== 'owner' && (
                      <div><Badge tone="warn">{m.cert_scope!.length} cert{m.cert_scope!.length === 1 ? '' : 's'} only</Badge></div>
                    )}
                  </td>
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
