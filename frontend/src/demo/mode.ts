/* Demonstration mode — the whole platform, visible without a working backend.
 *
 * WHY THIS EXISTS
 * The deployed service currently cannot serve its API (a production configuration blocker that
 * needs an operator environment change). With every request failing, each screen renders an error
 * or an empty state, so the platform is impossible to review or hand to a backend developer as a
 * specification. Demo mode fills every screen with realistic, self-consistent sample data so the
 * ENTIRE product is walkable with no server at all.
 *
 * THE HONESTY RULE
 * Demo data must never be mistakable for real data. Three guarantees:
 *   1. A permanent, non-dismissible banner sits above every screen while it is on.
 *   2. It NEVER pre-empts a working backend — live data always wins. Auto mode engages only
 *      after a request has actually failed on a deployment that never once answered, and it is
 *      not remembered across reloads, so a backend that comes back is picked up on the next load.
 *   3. Writes are not persisted anywhere; they are acknowledged in-session and lost on reload.
 *
 * HOW IT TURNS ON
 *   ?demo=1        — explicit, remembered for the browser (?demo=0 clears it)
 *   VITE_DEMO=1    — baked in at build time, for a dedicated showcase deployment
 *   automatic      — after a network error or a 5xx, so a broken backend degrades to a full
 *                    walkable product instead of a wall of error notices
 */

const KEY = 'pci.demo'

export type DemoReason = 'explicit' | 'build' | 'auto'

let state: { on: boolean; reason: DemoReason } = { on: false, reason: 'explicit' }
const listeners = new Set<() => void>()

function persisted(): boolean {
  try {
    return localStorage.getItem(KEY) === '1'
  } catch {
    return false
  }
}

function persist(on: boolean) {
  try {
    if (on) localStorage.setItem(KEY, '1')
    else localStorage.removeItem(KEY)
  } catch {
    /* storage blocked — the in-memory flag still applies for this page view */
  }
}

/** Read `?demo=` once at start-up; it wins over anything remembered and is then remembered. */
function fromUrl(): boolean | null {
  try {
    const v = new URLSearchParams(window.location.search).get('demo')
    if (v === null) return null
    return v !== '0' && v !== 'false'
  } catch {
    return null
  }
}

function buildFlag(): boolean {
  try {
    return String((import.meta as { env?: Record<string, unknown> }).env?.VITE_DEMO ?? '') === '1'
  } catch {
    return false
  }
}

/** Resolve the initial mode. Call once from the app entry, before the first render. */
export function initDemoMode(): void {
  const url = fromUrl()
  if (url !== null) {
    persist(url)
    state = { on: url, reason: 'explicit' }
    return
  }
  if (buildFlag()) {
    state = { on: true, reason: 'build' }
    return
  }
  state = { on: persisted(), reason: 'explicit' }
  if (!state.on) void probe()
}

/* Ask the backend once, at start-up, whether it is there.
 *
 * Without this, the banner only appears after some screen happens to make a request — so the
 * sign-in page, the first thing anyone sees, would show a demonstration sign-in form with nothing
 * saying so. A notice that arrives on the second screen is not a notice.
 *
 * Deliberately fire-and-forget against /api/health: it never blocks the first paint, and on a
 * healthy deployment it just records the success that the blip guard wants recorded early anyway. */
async function probe(): Promise<void> {
  try {
    const res = await fetch('/api/health', { method: 'GET' })
    if (res.status >= 500) engageAuto()
    else noteSuccess()
  } catch {
    engageAuto()
  }
}

export const isDemo = (): boolean => state.on
export const demoReason = (): DemoReason => state.reason

/** Subscribe to mode changes so the banner appears the moment auto mode engages. */
export function onDemoChange(fn: () => void): () => void {
  listeners.add(fn)
  return () => listeners.delete(fn)
}

function announce() {
  for (const fn of listeners) {
    try { fn() } catch { /* a listener must never break the request path */ }
  }
}

/* Auto-fallback is deliberately limited to deployments that have NEVER reached their backend.
 *
 * Without this limit, a transient network blip in a healthy production deployment would replace
 * a student's own record with a fabricated one — they would see an invented name and invented
 * credentials and have no reason to doubt them. Requiring that the app has never once had a
 * successful response separates the two cases cleanly:
 *   - backend absent or misconfigured  → the first request fails → full demo, zero configuration
 *   - backend healthy, connection blips → we already succeeded once → the honest error stands
 */
let everSucceeded = false

/** Record that the backend answered. Also leaves auto mode, since it is demonstrably alive. */
export function noteSuccess(): void {
  everSucceeded = true
  releaseAuto()
}

/** Engage automatically because the backend could not answer. Returns whether it took effect —
 *  the caller must throw its normal error when it did not.
 *
 *  Deliberately NOT persisted. Once on, requests short-circuit before the network, so this session
 *  cannot notice the backend recovering; leaving it out of storage means the very next page load
 *  probes for real and picks up a backend that has come back. */
export function engageAuto(): boolean {
  if (state.on) return true
  if (everSucceeded) return false
  state = { on: true, reason: 'auto' }
  announce()
  return true
}

/** A request succeeded, so the backend is alive after all — leave auto mode.
 *  Explicitly requested demo mode is deliberately NOT cancelled by this. */
export function releaseAuto(): void {
  if (!state.on || state.reason !== 'auto') return
  state = { on: false, reason: 'auto' }
  announce()
}

/** Turn demo mode on/off from the UI, remembering the choice. */
export function setDemo(on: boolean): void {
  persist(on)
  state = { on, reason: 'explicit' }
  announce()
}

/* The banner's words. They live in this leaf module rather than beside the resolver so the banner
 * can import them without dragging every fixture into the entry chunk — the fixtures are meant to
 * load only when a request is actually answered from them. */
export const DEMO_NOTICE =
  'This deployment is not connected to a live backend. Nothing shown is a real record, and changes are not saved.'
