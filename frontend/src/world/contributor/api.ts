// PCI World contributor publishing — typed client for the authoring surface.
//
// Rides the same Passport session as the rest of the World app: the token comes from ../api rather
// than a second copy of the storage key, because two copies of 'pciworld_account' would be two
// places for a sign-out to miss. X-World-Account, never Authorization — a World session and a
// portal session must not share a carrier.
//
// Every field below is mirrored from backend/Endpoints/WorldContributorsApi.cs. Where the server
// sends a `notice`, that sentence is shown VERBATIM: a moderation or editorial outcome paraphrased
// by the client is how an interface starts disagreeing with what actually happened.

import { getToken } from '../api'

export class ContributorError extends Error {
  status: number
  error: string
  /** The server's own words, when it sent any. Null means the interface owns the wording. */
  notice: string | null
  constructor(status: number, error: string, message: string, notice: string | null = null) {
    super(message)
    this.status = status
    this.error = error
    this.notice = notice
  }
}

async function call<T>(path: string, method: string, body?: unknown): Promise<T> {
  const headers: Record<string, string> = {}
  const token = getToken()
  if (token) headers['X-World-Account'] = token
  if (body !== undefined) headers['Content-Type'] = 'application/json'

  const res = await fetch(path, {
    method,
    headers,
    body: body === undefined ? undefined : JSON.stringify(body),
    credentials: 'same-origin',
  })

  const text = await res.text()
  let data: Record<string, unknown> = {}
  try { data = text ? JSON.parse(text) : {} } catch { /* non-JSON handled below */ }

  if (!res.ok) {
    const code = String(data.error ?? 'request_failed')
    throw new ContributorError(
      res.status, code, reasonText(code),
      typeof data.notice === 'string' ? data.notice : null,
    )
  }
  return data as T
}

const get = <T,>(p: string) => call<T>(p, 'GET')
const post = <T,>(p: string, b?: unknown) => call<T>(p, 'POST', b)
const patch = <T,>(p: string, b?: unknown) => call<T>(p, 'PATCH', b)

// Every refusal the server can send, in words someone can act on. The server deliberately sends
// stable codes rather than prose — phrasing belongs next to the interface. Anything unmapped falls
// back to a neutral sentence rather than showing a raw code, because "not_submittable" tells nobody
// what to do next.
export const REASONS: Record<string, string> = {
  no_session: 'Please sign in to your PCI World account.',
  not_a_contributor: 'Writing for PCI World needs contributor standing, which an editor grants.',
  email_not_verified: 'Verify your email address first — an editorial decision needs somewhere to reach you.',
  terms_unavailable: 'Contributor applications are not open yet.',
  already_applied: 'Your application is already with the editorial team.',
  already_granted: 'You already have contributor standing.',
  revoked: 'Your contributor standing was withdrawn. Restoring it is an editorial decision, so re-applying will not do it.',
  bad_title: 'Give the piece a working title of at least eight characters.',
  draft_budget_exhausted: 'You have a lot of pieces open. Finish or withdraw one before starting another.',
  not_editable: 'This piece is with an editor, so it can no longer be edited here.',
  not_submittable: 'This piece cannot be submitted from its current state.',
  already_submitted: 'This piece is already with an editor. Your declarations from that submission stand.',
  declarations_required: 'Answer the four declarations before submitting.',
  empty: 'Write something first.',
  too_long: 'That message is too long.',
  conflict: 'Someone else changed this at the same time. Reload and try again.',
}

export function reasonText(code: string | null | undefined): string {
  if (!code) return 'That could not be completed.'
  return REASONS[code] ?? 'That could not be completed.'
}

// ── Wire types (mirror the server DTOs) ──

/** applied | granted | declined | revoked, or 'none' when the person has never applied. */
export type Standing = 'none' | 'applied' | 'granted' | 'declined' | 'revoked'

export interface Me {
  state: Standing
  /** Authoritative on the server; used here only to decide what to draw. */
  may_write: boolean
  terms_version: string | null
  granted_at: string | null
  revoked_at: string | null
  /** Recorded when standing was withdrawn. Shown to the person it happened to. */
  revoke_reason: string | null
  /** False when no contributor terms have been published — applications answer 503. */
  terms_available: boolean
}

export interface ArticleSummary {
  id: number
  slug: string
  title: string
  status: string
  version: number
  published_at: string | null
  updated_at: string | null
  /** The server's view of whether the contributor's copy is still open. */
  editable: boolean
}

export interface HistoryEntry {
  event: string
  from: string | null
  to: string | null
  by: 'contributor' | 'editor' | 'system'
  note: string | null
  at: string
}

export interface Message {
  from: 'contributor' | 'editor'
  body: string
  at: string
}

export interface ArticleDetail {
  id: number
  slug: string
  title: string
  dek: string | null
  body_md: string | null
  status: string
  version: number
  published_at: string | null
  editable: boolean
  history: HistoryEntry[]
  messages: Message[]
}

/** The four questions frozen into the submission event. Free text, not a checkbox. */
export interface Declarations {
  conflict: string
  sponsorship: string
  ai_assistance: string
  originality: string
}

export const me = () => get<Me>('/api/world/contributor/me')

export const apply = (statement: string) =>
  post<{ ok: boolean; state: string; notice: string }>('/api/world/contributor/apply', { statement })

export const listArticles = () =>
  get<{ articles: ArticleSummary[] }>('/api/world/contributor/articles')

export const getArticle = (id: number) =>
  get<ArticleDetail>(`/api/world/contributor/articles/${id}`)

export const createArticle = (title: string, dek?: string, body_md?: string) =>
  post<{ ok: boolean; article_id: number; slug: string; status: string }>(
    '/api/world/contributor/articles', { title, dek, body_md })

export const saveArticle = (id: number, fields: { title?: string; dek?: string; body_md?: string }) =>
  patch<{ ok: boolean }>(`/api/world/contributor/articles/${id}`, fields)

export const submitArticle = (id: number, declarations: Declarations) =>
  post<{ ok: boolean; status: string; notice: string }>(
    `/api/world/contributor/articles/${id}/submit`, { declarations })

export const sendMessage = (id: number, body: string) =>
  post<{ ok: boolean }>(`/api/world/contributor/articles/${id}/messages`, { body })

// The editorial status chain, in the words a writer would use rather than the engine's own.
// `drafting` and `technical_review` are the only two the contributor can act on; everything after
// is an editor's business, and the labels say so instead of implying the writer is holding it up.
export const STATUS_LABEL: Record<string, string> = {
  idea: 'Idea',
  drafting: 'Drafting',
  technical_review: 'With an editor',
  fact_check: 'Being checked',
  legal_review: 'Legal review',
  seo_review: 'Final read',
  approved: 'Accepted — awaiting publication',
  published: 'Published',
  archived: 'Withdrawn',
}

export const statusLabel = (s: string) => STATUS_LABEL[s] ?? s

/** Which pill tone a status gets. State is always carried by the WORD; this only reinforces it. */
export function statusTone(s: string): 'live' | 'held' | 'stop' | 'plain' {
  if (s === 'published' || s === 'approved') return 'live'
  if (s === 'archived') return 'stop'
  if (s === 'drafting' || s === 'idea') return 'plain'
  return 'held'
}
