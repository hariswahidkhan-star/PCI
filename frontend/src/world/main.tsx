import React from 'react'
import ReactDOM from 'react-dom/client'
import { initDemoMode } from '../demo/mode'
import App from './App'
import CommunityApp from './community/CommunityApp'
import ForumApp from './forum/ForumApp'
import VerifyNumber from './VerifyNumber'
import './world.css'

// Several audiences share this bundle, and they are NOT the same product surface.
//
// The account app requires a PCI World Passport session and is the participant's own home. The
// community rooms are open to a visitor with no account at all — that is the entire point of a
// guest room — so mounting them inside the authenticated shell would put a sign-in wall in front
// of the one surface designed not to have one.
//
// The forum WRITE path (/world-app/forum) is signed-in, but it also has to be reachable from the
// server-rendered thread pages via ?thread=… deep links — so it mounts beside the account shell
// rather than only behind the dashboard "Start a discussion" button.
//
// The split is by path rather than by a router at the root because the account app is a state
// machine, not a routed tree, and wrapping it would be a larger change than this needs.
const path = window.location.pathname
const isCommunity = path.startsWith('/world-app/community')
const isForum = path.startsWith('/world-app/forum')
// Passport verification by Student Number is likewise a PUBLIC surface (Phase 5): a recruiter
// checking a number must never be asked to sign in, so it mounts beside — not inside — the
// authenticated account shell.
const isVerify = path.startsWith('/world-app/verify-number')

function ForumMount() {
  const q = new URLSearchParams(window.location.search)
  const threadRaw = q.get('thread')
  const threadId = threadRaw && /^\d+$/.test(threadRaw) ? Number(threadRaw) : undefined
  const threadTitle = q.get('title') || undefined
  return (
    <div className="world-shell">
      <header>
        <span className="brand">PCI <b>World</b></span>
        <span className="tag">forum</span>
        <p style={{ margin: '0.5rem 0 0' }}>
          <a href="/world/forum">Browse discussions</a>
          {' · '}
          <a href="/world-app/community">Community rooms</a>
          {' · '}
          <a href="/world-app/">Your dashboard</a>
        </p>
      </header>
      <main>
        <ForumApp threadId={threadId} threadTitle={threadTitle} />
      </main>
    </div>
  )
}

initDemoMode()

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    {isVerify ? <VerifyNumber />
      : isCommunity ? <CommunityApp />
        : isForum ? <ForumMount />
          : <App />}
  </React.StrictMode>,
)
