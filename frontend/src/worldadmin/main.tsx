import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import ShareConsole from './ShareConsole'
import './worldadmin.css'

/** The /world-admin-app/ mount. Authentication belongs to the World-admin realm: the console's
 *  seam sends the bearer the server-rendered /world-admin shell stores in localStorage after its
 *  own sign-in. With no token present we do not render a broken console — we say where to sign in.
 *  This gate is presentation only; every API the console calls enforces the session server-side. */
function Gate() {
  if (!localStorage.getItem('world_admin_token')) {
    return (
      <main style={{ maxWidth: '36rem', margin: '4rem auto', fontFamily: 'system-ui, sans-serif' }}>
        <h1>PCI World Admin — share management</h1>
        <p>
          Sign in to the PCI World admin first — this console uses that session.{' '}
          <a href="/world-admin">Go to the World admin sign-in</a>, then return here.
        </p>
      </main>
    )
  }
  return <ShareConsole />
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Gate />
  </StrictMode>,
)
