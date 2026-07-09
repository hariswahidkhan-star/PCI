import { useState } from 'react'
import { NavLink, Outlet } from 'react-router-dom'
import { useAdminAuth } from './AdminAuth'
import { CRUD_SECTIONS } from './crudConfigs'
import { initials } from '../format'

// Sections ported to the React admin so far. `perm` null = any authenticated admin;
// `owner` = owner-only; `anyPerm` = visible if the admin holds any listed permission.
// Everything else in the platform remains in the classic panel (linked below), so nothing is lost.
interface NavItem { to: string; label: string; perm?: string | null; owner?: boolean; anyPerm?: string[]; end?: boolean }
const NAV: NavItem[] = [
  { to: '/', label: 'Dashboard', perm: null, end: true },
  { to: '/students', label: 'Students', perm: 'members' },
  { to: '/enrollments', label: 'Enrolments', perm: 'enrollments' },
  { to: '/payments', label: 'Payments', perm: 'payments' },
  { to: '/credentials', label: 'Credentials', perm: 'credentials' },
  { to: '/tickets', label: 'Support tickets', perm: 'tickets' },
  { to: '/certifications', label: 'Certifications', perm: 'exams' },
  { to: '/exams', label: 'Exam registrations', perm: 'exams' },
  { to: '/proctoring', label: 'Proctoring & sessions', perm: 'proctoring' },
  { to: '/codes', label: 'Discount codes', perm: 'codes' },
  { to: '/pages', label: 'Pages & content', perm: 'pages' },
  { to: '/enquiries', label: 'Enquiries', perm: 'inquiries' },
  { to: '/submissions', label: 'Form submissions', perm: 'submissions' },
  { to: '/reviews', label: 'Reviews', perm: 'content' },
  { to: '/content', label: 'Site content', perm: 'content' },
  { to: '/subscribers', label: 'Newsletter', perm: 'subscribers' },
  { to: '/reports', label: 'Reports', perm: 'reports' },
  { to: '/emails', label: 'Email log', perm: 'emails' },
  { to: '/audit', label: 'Audit log', perm: 'audit' },
  // generic content collections (question bank, media, FAQs, resources, news, BoK, governance, nav)
  ...CRUD_SECTIONS.map((c) => ({ to: '/' + c.path, label: c.title, perm: c.perm })),
  { to: '/settings', label: 'Settings', anyPerm: ['settings', 'set_web', 'set_sp', 'set_exam'] },
  { to: '/team', label: 'Team & Access', owner: true },
]

export default function AdminLayout() {
  const { me, logout, can } = useAdminAuth()
  const [menuOpen, setMenuOpen] = useState(false)
  const items = NAV.filter((n) => {
    if (n.owner) return !!me?.is_owner
    if (n.anyPerm) return !!me?.is_owner || n.anyPerm.some((p) => can(p))
    return n.perm == null || can(n.perm)
  })

  return (
    <div className="shell">
      <div className={'nav-backdrop' + (menuOpen ? ' open' : '')} onClick={() => setMenuOpen(false)} />
      <aside className={'sidebar' + (menuOpen ? ' open' : '')}>
        <div className="brand">
          <img src="/assets/logo.png" alt="PCI" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <span>PCI Admin</span>
        </div>
        <nav className="nav">
          {items.map((n) => (
            <NavLink key={n.to} to={n.to} end={n.end} onClick={() => setMenuOpen(false)} className={({ isActive }) => (isActive ? 'active' : '')}>
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
          <div className="row">
            <button className="menu-btn" aria-label="Menu" aria-expanded={menuOpen} onClick={() => setMenuOpen((o) => !o)}>☰</button>
            <strong>Admin Console</strong>
          </div>
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
