import { Fragment, useEffect, useRef, useState } from 'react'
import { NavLink, Outlet, useLocation } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { useMe } from '../data/MeContext'
import { useT } from '../i18n'
import LanguageSwitcher from './LanguageSwitcher'
import NotificationBell from './NotificationBell'
import ThemeToggle from './ThemeToggle'
import DemoBanner from './DemoBanner'
import SkipLink from './SkipLink'
import { initials } from '../format'
import { Icon, type IconName } from './icons'

const NAV: { to: string; tkey: string; icon: IconName; end?: boolean; badgeKey?: 'unread' | 'profile' }[] = [
  { to: '/', tkey: 'nav.overview', icon: 'dashboard', end: true },
  { to: '/certifications', tkey: 'nav.certifications', icon: 'award' },
  { to: '/credentials', tkey: 'nav.credentials', icon: 'shield-check' },
  { to: '/cpd', tkey: 'nav.cpd', icon: 'refresh' },
  { to: '/certuvo', tkey: 'nav.certuvo', icon: 'sparkles' },
  { to: '/lab', tkey: 'nav.lab', icon: 'flask' },
  { to: '/billing', tkey: 'nav.billing', icon: 'credit-card' },
  { to: '/resources', tkey: 'nav.resources', icon: 'book-open' },
  { to: '/templates', tkey: 'nav.templates', icon: 'file-text' },
  { to: '/events', tkey: 'nav.events', icon: 'calendar' },
  { to: '/event-passes', tkey: 'nav.eventPasses', icon: 'tag' },
  { to: '/documents', tkey: 'nav.documents', icon: 'folder' },
  { to: '/messages', tkey: 'nav.messages', icon: 'mail', badgeKey: 'unread' },
  { to: '/support', tkey: 'nav.support', icon: 'life-buoy' },
  { to: '/appeals', tkey: 'nav.appeals', icon: 'scale' },
  { to: '/applications', tkey: 'nav.applications', icon: 'clipboard' },
  { to: '/profile', tkey: 'nav.profile', icon: 'user', badgeKey: 'profile' },
]

const TITLE_KEYS: Record<string, string> = {
  '/': 'nav.overview',
  '/certifications': 'nav.certifications',
  '/credentials': 'nav.credentials',
  '/cpd': 'nav.cpd',
  '/certuvo': 'nav.certuvo',
  '/lab': 'nav.lab',
  '/billing': 'nav.billing',
  '/resources': 'nav.resources',
  '/templates': 'nav.templates',
  '/events': 'nav.events',
  '/event-passes': 'nav.eventPasses',
  '/documents': 'nav.documents',
  '/applications': 'nav.applications',
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
      <SkipLink />
      <div className={'nav-backdrop' + (menuOpen ? ' open' : '')} onClick={() => setMenuOpen(false)} />
      <aside className={'sidebar sidebar--bright' + (menuOpen ? ' open' : '')}>
        <div className="brand">
          <img src="/assets/logo.png" alt="PCI ai" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
          <span>{t('shell.portal')}</span>
        </div>
        <div className="nav-label">{t('shell.menu')}</div>
        <nav className="nav">
          {NAV.map((n) => (
            <Fragment key={n.to}>
              <NavLink to={n.to} end={n.end} onClick={() => setMenuOpen(false)} className={({ isActive }) => (isActive ? 'active' : '')}>
                <Icon name={n.icon} size={17} className="nav-ic" />
                <span>{t(n.tkey)}</span>
                {n.badgeKey === 'unread' && unread > 0 && <span className="pill">{unread}</span>}
                {n.badgeKey === 'profile' && completion < 100 && <span className="pill dim">{completion}%</span>}
              </NavLink>
            </Fragment>
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
        <DemoBanner />
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
                <strong className="tb-title">
                  {t(TITLE_KEYS[loc.pathname] ?? (loc.pathname.startsWith('/lab/') ? 'nav.lab' : 'shell.studentPortal'))}
                </strong>
              </div>
            </div>
            <div className="row">
              <LanguageSwitcher />
              <ThemeToggle />
              <NotificationBell />
              <div className="avatar" title={user?.email}>{initials(user?.firstName, user?.lastName)}</div>
              <div className="small" style={{ lineHeight: 1.2 }}>
                <div style={{ fontWeight: 700 }}>{user ? `${user.firstName} ${user.lastName}`.trim() || user.email : ''}</div>
                <div className="muted">{user?.email}</div>
              </div>
              <button className="btn secondary sm" onClick={logout}>{t('shell.signOut')}</button>
            </div>
          </header>
          <main id="main-content" tabIndex={-1} className="content route-fade" key={loc.pathname}>
            <Outlet />
          </main>
        </div>
      </div>
    </div>
  )
}
