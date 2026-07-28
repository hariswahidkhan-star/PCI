import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import CommunityApp from './community/CommunityApp'
import VerifyNumber from './VerifyNumber'
import './world.css'

// Two audiences share this bundle, and they are NOT the same product surface.
//
// The account app requires a PCI World Passport session and is the participant's own home. The
// community rooms are open to a visitor with no account at all — that is the entire point of a
// guest room — so mounting them inside the authenticated shell would put a sign-in wall in front
// of the one surface designed not to have one.
//
// The split is by path rather than by a router at the root because the account app is a state
// machine, not a routed tree, and wrapping it would be a larger change than this needs.
const isCommunity = window.location.pathname.startsWith('/world-app/community')
// Passport verification by Student Number is likewise a PUBLIC surface (Phase 5): a recruiter
// checking a number must never be asked to sign in, so it mounts beside — not inside — the
// authenticated account shell.
const isVerify = window.location.pathname.startsWith('/world-app/verify-number')

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    {isVerify ? <VerifyNumber /> : isCommunity ? <CommunityApp /> : <App />}
  </React.StrictMode>,
)
