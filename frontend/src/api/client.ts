// Thin typed fetch wrapper for the PCI JSON API.
//
// Auth mirrors the existing portals exactly: a bearer session token, kept in sessionStorage
// (cleared when the tab closes — deliberate for shared computers), sent as
// `Authorization: Bearer <token>` on every call. The client is created per token key so the
// student app (/api/login) and the admin app (/api/admin/auth/login) never share a session.

import { demoAwareFetch } from '../demo/intercept'

export class ApiError extends Error {
  status: number
  body: unknown
  constructor(status: number, message: string, body: unknown) {
    super(message)
    this.status = status
    this.body = body
  }
}

/** Raised on 401 so the auth layer can redirect to login and clear the stale token. */
export class UnauthorizedError extends ApiError {}

export interface RequestOptions {
  method?: string
  body?: unknown
  // when true, a 401 does NOT throw UnauthorizedError (used by the login probe)
  allowUnauthorized?: boolean
}

export interface ApiClient {
  getToken: () => string | null
  setToken: (token: string | null) => void
  /** Register a handler invoked whenever any request (query OR mutation) gets a 401, so the auth
   *  layer can clear the stale token and redirect to login from a single place. */
  onUnauthorized: (fn: (() => void) | null) => void
  get: <T>(path: string) => Promise<T>
  post: <T>(path: string, body?: unknown, extra?: RequestOptions) => Promise<T>
  patch: <T>(path: string, body?: unknown) => Promise<T>
  del: <T>(path: string) => Promise<T>
}

// Pull the backend error code (or a fallback) out of a JSON error body.
function errorCode(data: unknown, status: number): string {
  if (data && typeof data === 'object') {
    const rec = data as Record<string, unknown>
    if (typeof rec.error === 'string') return rec.error
    if (typeof rec.message === 'string') return rec.message
  }
  return `Request failed (${status})`
}

// Map a few known backend error codes to friendly copy; otherwise pass through.
function humanize(code: string): string {
  switch (code) {
    case 'invalid_credentials':
      return 'Email or password is incorrect.'
    case 'inactive':
      return 'This account is not active. Please contact support.'
    case 'no_token':
    case 'unauthorized':
      return 'Your session has expired. Please sign in again.'
    case 'forbidden':
      return 'You do not have permission to view this.'
    case 'weak_password':
      return 'Password must be at least 8 characters.'
    case 'password_mismatch':
      return 'Passwords do not match. Please re-enter them.'
    case 'email_exists':
      return 'An admin with this email already exists.'
    case 'maker_checker':
      // Not a failure to retry: the scenario needs a different admin, and saying so stops an
      // operator hunting for a fault that isn't there.
      return 'The approver must be different from the author — a second admin has to approve this.'
    default:
      return code
  }
}

/** Build an API client bound to a sessionStorage token key. */
export function makeClient(tokenKey: string): ApiClient {
  // In-memory fallback so the app still works if storage is blocked (private mode / kiosk).
  let memToken: string | null = null
  let unauthorizedHandler: (() => void) | null = null

  const getToken = (): string | null => {
    try {
      return sessionStorage.getItem(tokenKey) ?? memToken
    } catch {
      return memToken
    }
  }

  const setToken = (token: string | null) => {
    memToken = token
    try {
      if (token) sessionStorage.setItem(tokenKey, token)
      else sessionStorage.removeItem(tokenKey)
    } catch {
      /* storage blocked — memToken already updated */
    }
  }

  async function request<T>(path: string, opts: RequestOptions = {}): Promise<T> {
    const headers: Record<string, string> = { 'Content-Type': 'application/json' }
    const token = getToken()
    if (token) headers['Authorization'] = 'Bearer ' + token

    // Demonstration mode short-circuits before the network. See src/demo/intercept.ts for the
    // rule and src/demo/mode.ts for why it exists — in particular, it never pre-empts a working
    // backend: a deployment that has answered once keeps getting the honest error.
    let attempt
    try {
      attempt = await demoAwareFetch(path, {
        method: opts.method ?? 'GET',
        headers,
        body: opts.body !== undefined ? JSON.stringify(opts.body) : undefined,
      }, opts.body)
    } catch (e) {
      throw new ApiError(0, 'Network error — please check your connection and try again.', e)
    }
    if (attempt.demo) return attempt.data as T
    const res = attempt.res

    let data: unknown = null
    const text = await res.text()
    if (text) {
      try {
        data = JSON.parse(text)
      } catch {
        data = text
      }
    }

    if (!res.ok) {
      const code = errorCode(data, res.status)
      let msg = humanize(code)
      // Unmapped slug: prefer the backend's human-readable message over the raw machine code.
      if (msg === code && data && typeof data === 'object' && typeof (data as Record<string, unknown>).message === 'string') {
        msg = (data as Record<string, unknown>).message as string
      }
      if (res.status === 401 && !opts.allowUnauthorized) {
        // fire the central handler (clear token + redirect) for EVERY 401, mutations included
        try { unauthorizedHandler?.() } catch { /* ignore */ }
        throw new UnauthorizedError(401, msg, data)
      }
      throw new ApiError(res.status, msg, data)
    }
    return data as T
  }

  return {
    getToken,
    setToken,
    onUnauthorized: (fn) => { unauthorizedHandler = fn },
    get: <T>(path: string) => request<T>(path),
    post: <T>(path: string, body?: unknown, extra?: RequestOptions) => request<T>(path, { ...extra, method: 'POST', body }),
    patch: <T>(path: string, body?: unknown) => request<T>(path, { method: 'PATCH', body }),
    del: <T>(path: string) => request<T>(path, { method: 'DELETE' }),
  }
}

// The student portal's client (default export surface, unchanged for existing imports).
const student = makeClient('pci.session.token')
export const api = student
export const getToken = student.getToken
export const setToken = student.setToken
