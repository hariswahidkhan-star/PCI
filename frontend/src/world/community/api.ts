// PCI World community rooms — API client for the GUEST surface.
//
// Deliberately separate from ../api.ts. That client carries the Passport account session; this one
// carries a room session, which is a different credential for a different audience with a different
// lifetime. Mixing them would let an account token leak into a guest request or vice versa, and the
// product rule is that a guest room needs no account at all.

import { demoAwareFetch } from '../../demo/intercept'

const ROOM_TOKEN_KEY = 'pciworld_room_session'

export class CommunityError extends Error {
  status: number
  error: string
  suggestion?: string
  constructor(status: number, error: string, message: string, suggestion?: string) {
    super(message)
    this.status = status
    this.error = error
    this.suggestion = suggestion
  }
}

export function getRoomToken(): string | null {
  return sessionStorage.getItem(ROOM_TOKEN_KEY)
}
// sessionStorage, not localStorage: a room session is meant to end with the tab. A guest identity
// that quietly persists across days on a shared machine is the opposite of what a guest room is for.
export function setRoomToken(t: string): void { sessionStorage.setItem(ROOM_TOKEN_KEY, t) }
export function clearRoomToken(): void { sessionStorage.removeItem(ROOM_TOKEN_KEY) }

async function call<T>(path: string, method: string, body?: unknown): Promise<T> {
  const headers: Record<string, string> = {}
  const token = getRoomToken()
  if (token) headers['X-Community-Session'] = token
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
    throw new CommunityError(
      res.status,
      String(data.error ?? 'request_failed'),
      String(data.message ?? REASONS[String(data.error)] ?? 'Something went wrong.'),
      data.suggestion ? String(data.suggestion) : undefined,
    )
  }
  return data as T
}

export const get = <T,>(p: string) => call<T>(p, 'GET')
export const post = <T,>(p: string, body?: unknown) => call<T>(p, 'POST', body)
export const del = <T,>(p: string) => call<T>(p, 'DELETE')

// Every reason code the server can return, in words a participant can act on.
//
// The server deliberately does not send prose for these — a reason code is stable and auditable,
// and phrasing is a product decision that belongs next to the interface. Anything unmapped falls
// back to a neutral sentence rather than showing a raw code, because "name_confusable_skeleton"
// tells a visitor nothing about what to type instead.
export const REASONS: Record<string, string> = {
  name_required: 'Please choose a display name.',
  name_too_short: 'That name is too short — please use at least two characters.',
  name_too_long: 'That name is too long.',
  name_not_readable: 'Please use a name with some letters in it.',
  name_offensive: 'Please choose a different name.',
  name_reserved: 'That name is reserved. Please choose another.',
  name_impersonation: 'That name could be mistaken for PCI staff. Please choose another.',
  name_misleading_badge: 'That name suggests an official role. Please choose another.',
  name_contact_details: 'Please leave contact details out of your display name.',
  name_unsupported_characters: 'That name uses characters we cannot display safely.',
  name_taken: 'Someone in this room is already using that name.',
  access_restricted: 'You cannot join this room at the moment.',
  room_full: 'This room is full. Please try again shortly.',
  room_not_open: 'This room is not open right now.',
  rules_version_stale: 'The room rules have been updated — please read and accept them again.',
  guests_not_allowed: 'This room is for signed-in members.',
  no_session: 'Your room session has ended.',
  rate_limited: 'You are sending messages too quickly. Please slow down.',
  slow_mode: 'Slow mode is on in this room.',
  message_too_long: 'That message is too long.',
  empty_message: 'Please write something first.',
  room_not_accepting: 'This room is not accepting messages right now.',
  moderation_unavailable: 'Messages cannot be checked right now, so nothing is being posted. Please try again shortly.',
  // Withheld-message reasons. Phrased so a person understands the outcome without being handed a
  // map of the thresholds — §8.4 asks for a safe reason, not an evasion guide.
  profanity_review: 'That message is being checked by a moderator.',
  profanity_blocked: 'That message was not posted.',
  profanity_severe: 'That message was not posted.',
  abuse_review: 'That message is being checked by a moderator.',
  abuse_blocked: 'That message was not posted.',
  abuse_severe: 'That message was not posted.',
  abuse_repeated: 'That message was not posted.',
  hate_review: 'That message is being checked by a moderator.',
  hate_blocked: 'That message was not posted.',
  hate_severe: 'That message was not posted.',
  sexual_review: 'That message is being checked by a moderator.',
  sexual_blocked: 'That message was not posted.',
  sexual_severe: 'That message was not posted.',
  violence_review: 'That message is being checked by a moderator.',
  violence_blocked: 'That message was not posted.',
  violence_credible_threat: 'That message was not posted.',
  self_harm_review: 'That message is being checked by a moderator.',
  self_harm_crisis: 'That message was not posted. If you need support, help is available.',
  grooming_review: 'That message is being checked by a moderator.',
  grooming_restricted: 'That message was not posted.',
  grooming_severe: 'That message was not posted.',
  illegal_review: 'That message is being checked by a moderator.',
  illegal_blocked: 'That message was not posted.',
  illegal_solicitation: 'That message was not posted.',
  spam_review: 'That message is being checked by a moderator.',
  spam_blocked: 'That message was not posted.',
  spam_repeated: 'That message was not posted.',
  spam_coordinated: 'That message was not posted.',
  pii_blocked: 'Please keep phone numbers, emails and addresses out of the room.',
  pii_doxxing_severe: 'That message was not posted.',
  rights_review: 'That message is being checked by a moderator.',
  rights_refused: 'That message was not posted.',
  no_policy_rule: 'That message is being checked by a moderator.',
  duplicate: 'That message was already sent.',
}

export function reasonText(code: string | null | undefined): string {
  if (!code) return 'That message was not posted.'
  return REASONS[code] ?? 'That message was not posted.'
}

// ── Wire types (mirror the server DTOs) ──

export interface RoomSummary {
  slug: string; title: string; description: string | null; topic: string | null
  category: string | null; locale: string; room_type: string; state: string
  capacity: number; guests_welcome: boolean; slow_mode_seconds: number; participants: number
}

export interface RoomDetail extends RoomSummary {
  accepting: boolean; images_allowed: boolean; rules_version: string
  pinned_welcome: string | null; retention_class: string
}

export interface Message {
  sequence: number; body: string; author: string | null
  reply_to: number | null; at: string | null
  // Phase 2. `kind` is absent on servers that predate images, so it is optional and defaults to
  // text at the render site — an older server must not make the transcript blank.
  kind?: string
  media_id?: number | null
}

export interface UploadResult {
  ok: boolean; media_id: number | null; state: string; reason: string; notice: string | null
}

export interface SendResult {
  published: boolean; sequence: number | null; status: string; reason: string
  ejected: boolean
  appeal: { reference: string; credential: string; restricted_until: string | null; how: string } | null
}

export const listRooms = () => get<{ rooms: RoomSummary[] }>('/api/world/community/rooms')
export const getRoom = (slug: string) => get<RoomDetail>(`/api/world/community/rooms/${encodeURIComponent(slug)}`)
export const since = (slug: string, afterSequence: number) =>
  get<{ messages: Message[] }>(`/api/world/community/rooms/${encodeURIComponent(slug)}/messages?afterSequence=${afterSequence}`)
// birth_date and jurisdiction are the eligibility declaration (CCP-P1-003). The server refuses
// entry without them, and refuses it again if the declaration does not meet the configured policy —
// so this client cannot decide anybody is eligible, it can only pass on what they said.
export const join = (room: string, display_name: string, rules_version: string,
                     birth_date: string, jurisdiction: string) =>
  post<{ ok: boolean; token: string; display_name: string }>('/api/world/community/guest-sessions',
    { room, display_name, rules_version, birth_date, jurisdiction })
export const leave = () => del<{ ok: boolean }>('/api/world/community/guest-sessions/current')
export const send = (slug: string, body: string, client_message_id: string) =>
  post<SendResult>(`/api/world/community/rooms/${encodeURIComponent(slug)}/messages`, { body, client_message_id })
// The upload returns a PENDING handle and no URL: at this point nothing renderable exists on the
// server, which is the contract, not an omission. The image appears in the transcript later — if
// and only if the server allows it — exactly like a text message does.
export const uploadImage = (slug: string, data: string, alt: string, client_upload_id: string) =>
  post<UploadResult>(`/api/world/community/rooms/${encodeURIComponent(slug)}/images`,
    { data, alt, client_upload_id })

// The one URL a client may build for an image. It 404s for anything not published, so it is safe
// to render for any allowed message without the client having to re-check state it cannot see.
export const mediaUrl = (id: number) => `/api/world/community/media/${id}`

export const report = (sequence: number, reason: string, note?: string) =>
  post<{ ok: boolean }>('/api/world/community/reports', { sequence, reason, note })
export const submitAppeal = (reference: string, credential: string, submission: string) =>
  post<{ ok: boolean }>('/api/world/community/appeals', { reference, credential, submission })
export const appealStatus = (reference: string, credential: string) =>
  post<{ reference: string; status: string; outcome: string | null; submitted: boolean; decided_at: string | null }>(
    '/api/world/community/appeals/status', { reference, credential })
