// PCI World participant app — API client.
//
// World sessions ride the `X-World-Account` header (plus an HttpOnly cookie the server also
// accepts), NOT the portal's Authorization bearer — the two products' sessions never share a
// carrier or a storage key. localStorage (not sessionStorage) matches the classic World pages,
// so the classic /world/* surfaces and this app see the same signed-in state.

const TOKEN_KEY = 'pciworld_account'   // same key the classic World pages use — one sign-in state

export class WorldApiError extends Error {
  status: number
  error: string
  constructor(status: number, error: string, message: string) {
    super(message)
    this.status = status
    this.error = error
  }
}

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY)
}

export function setToken(token: string): void {
  localStorage.setItem(TOKEN_KEY, token)
}

export function clearToken(): void {
  localStorage.removeItem(TOKEN_KEY)
}

export async function api<T>(path: string, body?: unknown, method?: string): Promise<T> {
  const headers: Record<string, string> = {}
  const token = getToken()
  if (token) headers['X-World-Account'] = token
  if (body !== undefined) headers['Content-Type'] = 'application/json'
  const res = await fetch(path, {
    method: method ?? (body !== undefined ? 'POST' : 'GET'),
    headers,
    body: body !== undefined ? JSON.stringify(body) : undefined,
  })
  const text = await res.text()
  let parsed: unknown = null
  try { parsed = text ? JSON.parse(text) : null } catch { /* non-JSON error body */ }
  if (!res.ok) {
    const o = (parsed ?? {}) as { error?: string; message?: string }
    throw new WorldApiError(res.status, o.error ?? 'error', o.message ?? o.error ?? `HTTP ${res.status}`)
  }
  return parsed as T
}

// ── PKCE (RFC 7636, S256) — used when this app is sent through the OAuth authorize flow. ──

function base64url(bytes: Uint8Array): string {
  let s = ''
  for (const b of bytes) s += String.fromCharCode(b)
  return btoa(s).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '')
}

export function newVerifier(): string {
  return base64url(crypto.getRandomValues(new Uint8Array(48)))
}

export async function challengeS256(verifier: string): Promise<string> {
  const digest = await crypto.subtle.digest('SHA-256', new TextEncoder().encode(verifier))
  return base64url(new Uint8Array(digest))
}

/** Exchange an OAuth authorization code (arriving as ?code= on the auth route) for a session. */
export async function exchangeOAuthCode(code: string, verifier: string, redirectUri: string): Promise<void> {
  const r = await api<{ access_token: string }>('/api/oauth/world/token', {
    grant_type: 'authorization_code',
    code,
    code_verifier: verifier,
    client_id: 'pciworld-app',
    redirect_uri: redirectUri,
  })
  setToken(r.access_token)
}

/** Redeem a one-time portal handoff code (arriving as #h= — never sent to any server log). */
export async function redeemHandoff(code: string): Promise<string> {
  const r = await api<{ token: string; return_to: string }>('/api/world/account/handoff', { code })
  setToken(r.token)
  return r.return_to
}
