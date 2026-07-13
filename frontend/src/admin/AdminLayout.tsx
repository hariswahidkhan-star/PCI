import { useEffect, useRef, useState } from 'react'
import { NavLink, Outlet, useLocation } from 'react-router-dom'
import { useAdminAuth } from './AdminAuth'
import { CRUD_SECTIONS } from './crudConfigs'
import { initials } from '../format'

// Sections ported to the React admin so far. `perm` null = any authenticated admin;
// `owner` = owner-only; `anyPerm` = visible if the admin holds any listed permission.
// Items are segregated into major categories; a category heading renders only when the
// signed-in admin can see at least one item inside it.
interface NavItem { to: string; label: string; perm?: string | null; owner?: boolean; anyPerm?: string[]; end?: boolean; group: string }
const crud = (path: string) => CRUD_SECTIONS.find((c) => c.path === path)!
const crudItem = (path: string, group: string): NavItem => ({ to: '/' + path, label: crud(path).title, perm: crud(path).perm, group })
const NAV: NavItem[] = [
  { to: '/', label: 'Dashboard', perm: null, end: true, group: 'Overview' },
  { to: '/reports', label: 'Reports', perm: 'reports', group: 'Overview' },

  { to: '/students', label: 'Students', perm: 'members', group: 'Students' },
  { to: '/enrollments', label: 'Enrolments', perm: 'enrollments', group: 'Students' },
  { to: '/payments', label: 'Payments', perm: 'payments', group: 'Students' },
  { to: '/tickets', label: 'Support tickets', perm: 'tickets', group: 'Students' },

  { to: '/certifications', label: 'Certifications', perm: 'exams', group: 'Examinations' },
  { to: '/exams', label: 'Exam registrations', perm: 'exams', group: 'Examinations' },
  { to: '/proctoring', label: 'Proctoring & sessions', perm: 'proctoring', group: 'Examinations' },
  crudItem('questions', 'Examinations'),
  { to: '/credentials', label: 'Credentials', perm: 'credentials', group: 'Examinations' },

  { to: '/codes', label: 'Discount codes', perm: 'codes', group: 'Access & pricing' },
  { to: '/founding', label: 'Founding stage', anyPerm: ['members', 'codes'], group: 'Access & pricing' },
  { to: '/honorary', label: 'Honorary fellows', owner: true, group: 'Access & pricing' },
  { to: '/honorary-applications', label: 'Honorary applications', owner: true, group: 'Access & pricing' },
  crudItem('pricing', 'Access & pricing'),

  { to: '/pages', label: 'Pages & content', perm: 'pages', group: 'Website' },
  { to: '/content', label: 'Site content', perm: 'content', group: 'Website' },
  { to: '/translations', label: 'Translations', owner: true, group: 'Website' },
  { to: '/reviews', label: 'Reviews', perm: 'content', group: 'Website' },
  crudItem('faqs', 'Website'),
  crudItem('resources', 'Website'),
  crudItem('news', 'Website'),
  crudItem('media', 'Website'),
  crudItem('bok', 'Website'),
  crudItem('governance', 'Website'),
  crudItem('navigation', 'Website'),

  { to: '/enquiries', label: 'Enquiries', perm: 'inquiries', group: 'Community' },
  { to: '/submissions', label: 'Form submissions', perm: 'submissions', group: 'Community' },
  { to: '/subscribers', label: 'Newsletter', perm: 'subscribers', group: 'Community' },

  { to: '/emails', label: 'Email log', perm: 'emails', group: 'Operations' },
  { to: '/audit', label: 'Audit log', perm: 'audit', group: 'Operations' },
  { to: '/settings', label: 'Settings', anyPerm: ['settings', 'set_web', 'set_sp', 'set_exam'], group: 'Operations' },
  { to: '/team', label: 'Team & Access', owner: true, group: 'Operations' },
]

export default function AdminLayout() {
  const { me, logout, can } = useAdminAuth()
  const [menuOpen, setMenuOpen] = useState(false)
  const loc = useLocation()
  // .main is the app's scroll container (not the body), so reset it on navigation
  const mainRef = useRef<HTMLDivElement>(null)
  useEffect(() => { mainRef.current?.scrollTo(0, 0) }, [loc.pathname])
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
          <img src="/assets/logo.png" alt="PCI Global" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <span>PCI Global Admin</span>
        </div>
        <nav className="nav">
          {items.map((n, i) => (
            <span key={n.to} style={{ display: 'contents' }}>
              {(i === 0 || items[i - 1].group !== n.group) && <div className="nav-label">{n.group}</div>}
              <NavLink to={n.to} end={n.end} onClick={() => setMenuOpen(false)} className={({ isActive }) => (isActive ? 'active' : '')}>
                {n.label}
              </NavLink>
            </span>
          ))}
        </nav>
        <div style={{ marginTop: '1.5rem' }}>
          <a className="btn ghost sm" href="/admin.html">Classic admin panel ↗</a>
          <p className="muted small" style={{ marginTop: '.4rem' }}>All other sections live in the classic panel.</p>
        </div>
      </aside>

      <div className="main" ref={mainRef}>
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
