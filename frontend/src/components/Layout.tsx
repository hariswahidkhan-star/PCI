import { useState } from 'react'
import { NavLink, Outlet } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { initials } from '../format'

const NAV = [
  { to: '/', label: 'Overview', end: true },
  { to: '/certifications', label: 'Certifications' },
  { to: '/credentials', label: 'Credentials' },
  { to: '/cpd', label: 'CPD' },
  { to: '/billing', label: 'Billing' },
  { to: '/messages', label: 'Messages', badgeKey: 'unread' as const },
  { to: '/profile', label: 'Profile' },
]

export default function Layout() {
  const { user, logout } = useAuth()
  const { me } = useMe()
  const unread = me?.unread ?? 0
  const [menuOpen, setMenuOpen] = useState(false)

  return (
    <div className="shell">
      <div className={'nav-backdrop' + (menuOpen ? ' open' : '')} onClick={() => setMenuOpen(false)} />
      <aside className={'sidebar' + (menuOpen ? ' open' : '')}>
        <div className="brand">
          <img src="/assets/logo.png" alt="PCI" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <span>PCI Portal</span>
        </div>
        <nav className="nav">
          {NAV.map((n) => (
            <NavLink key={n.to} to={n.to} end={n.end} onClick={() => setMenuOpen(false)} className={({ isActive }) => (isActive ? 'active' : '')}>
              <span>{n.label}</span>
              {n.badgeKey === 'unread' && unread > 0 && <span className="pill">{unread}</span>}
            </NavLink>
          ))}
        </nav>
        <div style={{ marginTop: '1.5rem' }}>
          <a className="btn ghost small" href="/student.html">Classic portal ↗</a>
        </div>
      </aside>

      <div className="main">
        <header className="topbar">
          <div className="row">
            <button className="menu-btn" aria-label="Menu" aria-expanded={menuOpen} onClick={() => setMenuOpen((o) => !o)}>☰</button>
            <strong>Student Portal</strong>
          </div>
          <div className="row">
            <div className="avatar" title={user?.email}>{initials(user?.firstName, user?.lastName)}</div>
            <div className="small" style={{ lineHeight: 1.2 }}>
              <div style={{ fontWeight: 700 }}>{user ? `${user.firstName} ${user.lastName}`.trim() || user.email : ''}</div>
              <div className="muted">{user?.email}</div>
            </div>
            <button className="btn secondary sm" onClick={logout}>Sign out</button>
          </div>
        </header>
        <main className="content">
          <Outlet />
        </main>
      </div>
    </div>
  )
}
