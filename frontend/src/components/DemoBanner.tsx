import { useEffect, useState } from 'react'
import { isDemo, demoReason, onDemoChange, setDemo, DEMO_NOTICE } from '../demo/mode'
import { Icon } from './icons'
import '../demo/banner.css'

/* The demonstration-mode banner.
 *
 * Non-dismissible on purpose. Demo data is fabricated, and a reviewer who scrolls past a
 * dismissible notice will read invented students and invented revenue as real records. The one
 * control offered is a way OUT of demo mode, never a way to hide the fact of it.
 *
 * It re-renders on mode changes because auto mode engages mid-session — the first failed request
 * flips it on, and the banner has to appear at that moment rather than on the next reload. */

export default function DemoBanner() {
  const [, force] = useState(0)
  useEffect(() => onDemoChange(() => force((n) => n + 1)), [])

  if (!isDemo()) return null
  const auto = demoReason() === 'auto'

  return (
    <div className="demo-banner" role="status">
      <Icon name="alert-triangle" size={15} />
      <span className="demo-banner-text">
        <strong>Demonstration data.</strong> {DEMO_NOTICE}
        {auto && ' The backend did not respond, so sample data is being shown in its place.'}
      </span>
      {/* Only offered when the operator chose demo mode. In auto mode the backend is genuinely
          unreachable, so a "use live data" button would promise something it cannot deliver. */}
      {!auto && (
        <button className="demo-banner-exit" onClick={() => { setDemo(false); window.location.reload() }}>
          Use live data
        </button>
      )}
    </div>
  )
}
