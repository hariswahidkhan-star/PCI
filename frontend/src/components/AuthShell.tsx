import type { ReactNode } from 'react'

/** Premium split-screen frame for the sign-in/sign-up pages: a navy brand pane with the
 * institute's seal engraving on the left, the form floating on the right. Stacks on mobile. */
export default function AuthShell({ children }: { children: ReactNode }) {
  return (
    <div className="auth-split">
      <aside className="auth-brandpane" aria-hidden="true">
        <div className="abp-inner">
          <div className="abp-brand">
            <img src="/assets/logo.png" alt="" onError={(e) => ((e.target as HTMLImageElement).style.display = 'none')} />
            <span>PCI Global</span>
          </div>
          <h1 className="abp-headline">
            The credential for the people who <em>control projects</em>.
          </h1>
          <ul className="abp-points">
            <li>Free account — pay only when you choose to enrol</li>
            <li>Your full professional record: experience, qualifications, credentials</li>
            <li>Exams, CPD, membership and support — one dashboard</li>
          </ul>
          <div className="abp-foot">Project Controls Institute Global, Inc.</div>
        </div>
      </aside>
      <div className="auth-formpane">{children}</div>
    </div>
  )
}
