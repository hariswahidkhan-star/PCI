import type { ReactNode } from 'react'
import LanguageSwitcher from './LanguageSwitcher'
import { useT } from '../i18n'

/** Premium split-screen frame for the sign-in/sign-up pages: a navy brand pane with the
 * institute's seal engraving on the left, the form floating on the right. Stacks on mobile. */
export default function AuthShell({ children }: { children: ReactNode }) {
  const t = useT()
  return (
    <div className="auth-split">
      <div className="auth-lang"><LanguageSwitcher /></div>
      <aside className="auth-brandpane">
        <div className="abp-inner">
          <div className="abp-brand">
            <img src="/assets/logo.png" alt="" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
            <span>PCI Global</span>
          </div>
          <h1 className="abp-headline">{t('abp.headline')}</h1>
          <ul className="abp-points">
            <li>{t('abp.point1')}</li>
            <li>{t('abp.point2')}</li>
            <li>{t('abp.point3')}</li>
          </ul>
          <div className="abp-foot">Project Controls Institute Global, Inc.</div>
        </div>
      </aside>
      <div className="auth-formpane">{children}</div>
    </div>
  )
}
