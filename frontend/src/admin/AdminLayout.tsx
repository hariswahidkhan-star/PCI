import { NavLink, Outlet } from 'react-router-dom'
import { useAdminAuth } from './AdminAuth'
import { initials } from '../format'

// Sections ported to the React admin so far. `perm` null = any authenticated admin.
// Everything else in the platform remains in the classic panel (linked below), so nothing is lost.
const NAV: { to: string; label: string; perm: string | null; end?: boolean }[] = [
  { to: '/', label: 'Dashboard', perm: null, end: true },
  { to: '/students', label: 'Students', perm: 'members' },
  { to: '/enrollments', label: 'Enrolments', perm: 'enrollments' },
  { to: '/payments', label: 'Payments', perm: 'payments' },
  { to: '/credentials', label: 'Credentials', perm: 'credentials' },
  { to: '/tickets', label: 'Support tickets', perm: 'tickets' },
  { to: '/certifications', label: 'Certifications', perm: 'exams' },
  { to: '/codes', label: 'Discount codes', perm: 'codes' },
  { to: '/pages', label: 'Pages & content', perm: 'pages' },
]

export default function AdminLayout() {
  const { me, logout, can } = useAdminAuth()
  const items = NAV.filter((n) => n.perm === null || can(n.perm))

  return (
    <div className="shell">
      <aside className="sidebar">
        <div className="brand">
          <img src="/assets/logo.png" alt="PCI" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <span>PCI Admin</span>
        </div>
        <nav className="nav">
          {items.map((n) => (
            <NavLink key={n.to} to={n.to} end={n.end} className={({ isActive }) => (isActive ? 'active' : '')}>
              {n.label}
            </NavLink>
          ))}
        </nav>
        <div style={{ marginTop: '1.5rem' }}>
          <a className="btn ghost small" href="/admin.html">Classic admin panel ↗</a>
          <p className="muted small" style={{ marginTop: '.4rem' }}>All other sections live in the classic panel.</p>
        </div>
      </aside>

      <div className="main">
        <header className="topbar">
          <strong>Admin Console</strong>
          <div className="row">
            <div className="avatar" title={me?.email}>{initials(me?.name?.split(' ')[0], me?.name?.split(' ')[1])}</div>
            <div className="small" style={{ lineHeight: 1.2 }}>
              <div style={{ fontWeight: 700 }}>{me?.name || me?.email}</div>
              <div className="muted">{me?.role}{me?.is_owner ? ' · owner' : ''}</div>
            </div>
            <button className="btn secondary sm" onClick={logout}>Sign out</button>
          </div>
        </header>
        <main className="content" style={{ maxWidth: 1180 }}>
          <Outlet />
        </main>
      </div>
    </div>
  )
}
