import { useEffect, useRef, useState } from 'react'
import { NavLink, Outlet, useLocation } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { useT } from '../i18n'
import LanguageSwitcher from './LanguageSwitcher'
import { initials } from '../format'

const NAV = [
  { to: '/', tkey: 'nav.overview', end: true },
  { to: '/certifications', tkey: 'nav.certifications' },
  { to: '/credentials', tkey: 'nav.credentials' },
  { to: '/cpd', tkey: 'nav.cpd' },
  { to: '/certuvo', tkey: 'nav.certuvo' },
  { to: '/billing', tkey: 'nav.billing' },
  { to: '/resources', tkey: 'nav.resources' },
  { to: '/events', tkey: 'nav.events' },
  { to: '/documents', tkey: 'nav.documents' },
  { to: '/messages', tkey: 'nav.messages', badgeKey: 'unread' as const },
  { to: '/support', tkey: 'nav.support' },
  { to: '/appeals', tkey: 'nav.appeals' },
  { to: '/profile', tkey: 'nav.profile', badgeKey: 'profile' as const },
]

const TITLE_KEYS: Record<string, string> = {
  '/': 'nav.overview',
  '/certifications': 'nav.certifications',
  '/credentials': 'nav.credentials',
  '/cpd': 'nav.cpd',
  '/certuvo': 'nav.certuvo',
  '/billing': 'nav.billing',
  '/resources': 'nav.resources',
  '/events': 'nav.events',
  '/documents': 'nav.documents',
  '/messages': 'nav.messages',
  '/support': 'nav.support',
  '/appeals': 'nav.appeals',
  '/profile': 'nav.profile',
}

export default function Layout() {
  const { user, logout } = useAuth()
  const { me } = useMe()
  const t = useT()
  const loc = useLocation()
  const unread = me?.unread ?? 0
  const completion = Number((me?.profile as Record<string, unknown> | null)?.profile_completion_percentage ?? 100)
  const memberActive = me?.lifecycle.membership_status === 'active'
  // Staff support view (admin impersonation): a permanent, non-dismissible banner on every page.
  const impersonated = !!(user?.impersonated || me?.user.impersonated)
  const [menuOpen, setMenuOpen] = useState(false)
  // .main is the app's scroll container (not the body), so reset it on navigation
  const mainRef = useRef<HTMLDivElement>(null)
  useEffect(() => { mainRef.current?.scrollTo(0, 0) }, [loc.pathname])

  return (
    <div className="shell">
      <div className={'nav-backdrop' + (menuOpen ? ' open' : '')} onClick={() => setMenuOpen(false)} />
      <aside className={'sidebar' + (menuOpen ? ' open' : '')}>
        <div className="brand">
          <img src="/assets/logo.png" alt="PCI Global" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <span>{t('shell.portal')}</span>
        </div>
        <div className="nav-label">{t('shell.menu')}</div>
        <nav className="nav">
          {NAV.map((n) => (
            <NavLink key={n.to} to={n.to} end={n.end} onClick={() => setMenuOpen(false)} className={({ isActive }) => (isActive ? 'active' : '')}>
              <span>{t(n.tkey)}</span>
              {n.badgeKey === 'unread' && unread > 0 && <span className="pill">{unread}</span>}
              {n.badgeKey === 'profile' && completion < 100 && <span className="pill dim">{completion}%</span>}
            </NavLink>
          ))}
        </nav>
        <div className="sidebar-foot">
          <div className={'member-chip' + (memberActive ? ' on' : '')}>
            <span className="mc-dot" />
            <span>
              <strong>{memberActive ? (me?.founding_member ? t('shell.foundingMember') : t('shell.member')) : t('shell.guest')}</strong>
              <em>{memberActive ? (me?.founding_member ? t('shell.feesWaived') : t('shell.membershipActive')) : t('shell.membershipNotActive')}</em>
            </span>
          </div>
        </div>
      </aside>

      <div className="main-col">
        {impersonated && (
          <div className="impersonation-banner" role="status">
            Support view — you are viewing this account as PCI staff. Actions that belong to the student are disabled.
          </div>
        )}
        <div className="main" ref={mainRef}>
          <header className="topbar">
            <div className="row">
              <button className="menu-btn" aria-label="Menu" aria-expanded={menuOpen} onClick={() => setMenuOpen((o) => !o)}>☰</button>
              <div>
                <div className="tb-crumb">{t('shell.studentPortal')}</div>
                <strong className="tb-title">{t(TITLE_KEYS[loc.pathname] ?? 'shell.studentPortal')}</strong>
              </div>
            </div>
            <div className="row">
              <LanguageSwitcher />
              <div className="avatar" title={user?.email}>{initials(user?.firstName, user?.lastName)}</div>
              <div className="small" style={{ lineHeight: 1.2 }}>
                <div style={{ fontWeight: 700 }}>{user ? `${user.firstName} ${user.lastName}`.trim() || user.email : ''}</div>
                <div className="muted">{user?.email}</div>
              </div>
              <button className="btn secondary sm" onClick={logout}>{t('shell.signOut')}</button>
            </div>
          </header>
          <main className="content route-fade" key={loc.pathname}>
            <Outlet />
          </main>
        </div>
      </div>
    </div>
  )
}
