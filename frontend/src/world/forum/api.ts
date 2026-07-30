// PCI World forum — API client for the AUTHENTICATED composer surface.
//
// Reading needs no client at all: thread pages are server-rendered HTML with JSON-LD (D-009, §6 of
// docs/pciworld/CCP_PHASE3_DESIGN.md), so this module exists only for the write path. It rides the
// same Passport account session as the rest of the World app — the token comes from ../api rather
// than a copy of the storage key, because two copies of 'pciworld_account' would be two places for
// a sign-out to miss.

import { getToken } from '../api'
import { demoAwareFetch } from '../../demo/intercept'

export class ForumError extends Error {
  status: number
  error: string
  retryAfterMinutes?: number
  constructor(status: number, error: string, message: string, retryAfterMinutes?: number) {
    super(message)
    this.status = status
    this.error = error
    this.retryAfterMinutes = retryAfterMinutes
  }
}

async function call<T>(path: string, method: string, body?: unknown): Promise<T> {
  const headers: Record<string, string> = {}
  const token = getToken()
  // X-World-Account, never Authorization: World sessions and portal sessions must not share a
  // carrier, or a forum request could quietly ride a student's portal bearer.
  if (token) headers['X-World-Account'] = token
  if (body !== undefined) headers['Content-Type'] = 'application/json'

  // Demonstration mode short-circuits before the network; see src/demo/intercept.ts for the rule.
  const attempt = await demoAwareFetch(path, {
    method,
    headers,
    body: body === undefined ? undefined : JSON.stringify(body),
    credentials: 'same-origin',
  }, body)
  if (attempt.demo) return attempt.data as T
  const res = attempt.res

  const text = await res.text()
  let data: Record<string, unknown> = {}
  try { data = text ? JSON.parse(text) : {} } catch { /* a non-JSON body is handled below */ }

  if (!res.ok) {
    const code = String(data.error ?? 'request_failed')
    throw new ForumError(
      res.status,
      code,
      reasonText(code),
      typeof data.retry_after_minutes === 'number' ? data.retry_after_minutes : undefined,
    )
  }
  return data as T
}

export const get = <T,>(p: string) => call<T>(p, 'GET')
export const post = <T,>(p: string, body?: unknown) => call<T>(p, 'POST', body)
export const patch = <T,>(p: string, body?: unknown) => call<T>(p, 'PATCH', body)
export const del = <T,>(p: string) => call<T>(p, 'DELETE')

// Every refusal code the server can send, in words an author can act on.
//
// The server deliberately sends stable codes rather than prose — phrasing is a product decision
// that belongs next to the interface. Anything unmapped falls back to a neutral sentence rather
// than a raw code, because "insufficient_trust_for_category" tells nobody what to do next.
export const REASONS: Record<string, string> = {
  no_session: 'Please sign in to your PCI World account to post.',
  bad_title: 'Please give your discussion a title between 8 and 200 characters.',
  category_not_open: 'This category is not accepting new discussions right now.',
  thread_not_found: 'That discussion no longer exists.',
  thread_not_open: 'This discussion is closed to new replies.',
  insufficient_trust_for_category: 'Your account does not yet have the standing this category asks for.',
  rate_limited: 'You have reached your posting limit for now. Please try again in about an hour.',
  empty_post: 'Please write something first.',
  post_too_long: 'That post is too long.',
  links_not_allowed_yet: 'New accounts cannot include links yet. Please remove the link and try again.',
  editing_not_allowed_yet: 'Your account cannot edit posts yet. Editing opens up as your posts are accepted.',
  not_your_post: 'You can only change your own posts.',
  flagging_not_allowed_yet: 'Your account cannot flag posts yet.',
  cannot_flag_your_own_post: 'You cannot flag your own post. You can edit or delete it instead.',
}

export function reasonText(code: string | null | undefined): string {
  if (!code) return 'That could not be posted.'
  return REASONS[code] ?? 'That could not be posted.'
}

// The trust ladder, mirrored from ForumTrust.Level so a disabled category can explain itself.
// ADVISORY ONLY: the server re-checks trust on every write, so a stale or tampered copy here can
// grey out a control at worst — it can never publish anything the server would refuse.
const LEVELS: readonly string[] = ['new', 'basic', 'member', 'trusted', 'staff']

export function atLeast(actual: string, required: string): boolean {
  // Unknown names mean 'new', matching the server's ForumTrust.Parse — guessing HIGHER would grey
  // out a category the server would actually accept.
  const idx = (level: string) => Math.max(0, LEVELS.indexOf(level.toLowerCase()))
  return idx(actual) >= idx(required)
}

// ── Wire types (mirror the server DTOs) ──

export interface Category {
  slug: string
  title: string
  description: string | null
  // Sent so the composer can explain WHY posting is unavailable — "needs member standing" is
  // actionable; a greyed button is not. That is the stated reason the server includes it.
  min_trust: string
  read_only: boolean
}

export interface Me {
  level: string
  // Stated plainly by the server so a new member is not confused by their first post vanishing.
  posts_are_reviewed_first: boolean
  may_post_links: boolean
  may_edit: boolean
  may_flag: boolean
  hourly_post_budget: number
}

export interface ThreadResult {
  ok: boolean
  thread_id: number
  slug: string
  url: string
  state: string
  published: boolean
  // The server's own words for a held post. Shown verbatim — paraphrasing a moderation outcome is
  // how an interface drifts from what actually happened.
  notice: string | null
}

export interface PostResult {
  ok: boolean
  post_id: number
  state: string
  published: boolean
  reason: string
  notice: string | null
}

// An edit carries no server notice — only state — so the interface owns the wording for this one.
export interface EditResult { ok: boolean; state: string; published: boolean; reason: string }

export interface DeleteResult { ok: boolean; notice: string }

// `already_flagged` arrives instead of `notice` when the unique index caught a double-tap; the
// server reports that as success so it does not confirm whether a previous flag exists.
export interface FlagResult { ok: boolean; already_flagged?: boolean; notice?: string }

export const listCategories = () => get<{ categories: Category[] }>('/api/world/forum/categories')
export const me = () => get<Me>('/api/world/forum/me')
export const createThread = (category: string, title: string, body: string) =>
  post<ThreadResult>(`/api/world/forum/categories/${encodeURIComponent(category)}/threads`, { title, body })
export const reply = (threadId: number, body: string) =>
  post<PostResult>(`/api/world/forum/threads/${threadId}/posts`, { body })
export const editPost = (postId: number, body: string, edit_reason?: string) =>
  patch<EditResult>(`/api/world/forum/posts/${postId}`, { body, edit_reason })
export const deletePost = (postId: number) => del<DeleteResult>(`/api/world/forum/posts/${postId}`)
export const flagPost = (postId: number, reason: string) =>
  post<FlagResult>(`/api/world/forum/posts/${postId}/flag`, { reason })
