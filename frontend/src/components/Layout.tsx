import { useState } from 'react'
import { NavLink, Outlet, useLocation } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { initials } from '../format'

const NAV = [
  { to: '/', label: 'Overview', end: true },
  { to: '/certifications', label: 'Certifications' },
  { to: '/credentials', label: 'Credentials' },
  { to: '/cpd', label: 'CPD' },
  { to: '/billing', label: 'Billing' },
  { to: '/resources', label: 'Resources' },
  { to: '/messages', label: 'Messages', badgeKey: 'unread' as const },
  { to: '/support', label: 'Support' },
  { to: '/profile', label: 'Profile', badgeKey: 'profile' as const },
]

const TITLES: Record<string, string> = {
  '/': 'Overview',
  '/certifications': 'Certifications',
  '/credentials': 'Credentials',
  '/cpd': 'CPD',
  '/billing': 'Billing',
  '/resources': 'Resources',
  '/messages': 'Messages',
  '/support': 'Support',
  '/profile': 'Profile',
}

export default function Layout() {
  const { user, logout } = useAuth()
  const { me } = useMe()
  const loc = useLocation()
  const unread = me?.unread ?? 0
  const completion = Number((me?.profile as Record<string, unknown> | null)?.profile_completion_percentage ?? 100)
  const memberActive = me?.lifecycle.membership_status === 'active'
  const [menuOpen, setMenuOpen] = useState(false)

  return (
    <div className="shell">
      <div className={'nav-backdrop' + (menuOpen ? ' open' : '')} onClick={() => setMenuOpen(false)} />
      <aside className={'sidebar' + (menuOpen ? ' open' : '')}>
        <div className="brand">
          <img src="/assets/logo.png" alt="PCI Global" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <span>PCI Global Portal</span>
        </div>
        <div className="nav-label">Menu</div>
        <nav className="nav">
          {NAV.map((n) => (
            <NavLink key={n.to} to={n.to} end={n.end} onClick={() => setMenuOpen(false)} className={({ isActive }) => (isActive ? 'active' : '')}>
              <span>{n.label}</span>
              {n.badgeKey === 'unread' && unread > 0 && <span className="pill">{unread}</span>}
              {n.badgeKey === 'profile' && completion < 100 && <span className="pill dim">{completion}%</span>}
            </NavLink>
          ))}
        </nav>
        <div className="sidebar-foot">
          <div className={'member-chip' + (memberActive ? ' on' : '')}>
            <span className="mc-dot" />
            <span>
              <strong>{memberActive ? (me?.founding_member ? 'Founding member' : 'Member') : 'Guest account'}</strong>
              <em>{memberActive ? (me?.founding_member ? 'Fees waived — founding cohort' : 'Membership active') : 'Membership not active'}</em>
            </span>
          </div>
        </div>
      </aside>

      <div className="main">
        <header className="topbar">
          <div className="row">
            <button className="menu-btn" aria-label="Menu" aria-expanded={menuOpen} onClick={() => setMenuOpen((o) => !o)}>☰</button>
            <div>
              <div className="tb-crumb">Student Portal</div>
              <strong className="tb-title">{TITLES[loc.pathname] ?? 'Student Portal'}</strong>
            </div>
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
        <main className="content route-fade" key={loc.pathname}>
          <Outlet />
        </main>
      </div>
    </div>
  )
}
