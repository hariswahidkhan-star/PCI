// PCI World careers — API client for the AUTHENTICATED surfaces: the employer portal and the
// applicant's "my applications" view (docs/pciworld/CCP_PHASE4_DESIGN.md §4, §5, §7).
//
// Public READING needs no client at all: the careers list, job pages and employer profiles are
// server-rendered HTML with JSON-LD (D-009), so this module exists only for the signed-in
// write-and-review paths. It rides the same Passport account session as the rest of the World app —
// the token comes from ../api rather than a copy of the storage key, because two copies of
// 'pciworld_account' would be two places for a sign-out to miss.

import { getToken } from '../api'

export class CareersError extends Error {
  status: number
  error: string
  /** The server's own words for this refusal, when it sent any (e.g. a refused publish carries
   *  "Only a verified organisation can publish a job…"). Shown verbatim by the surfaces —
   *  paraphrasing an outcome is how an interface drifts from what actually happened. */
  notice?: string
  constructor(status: number, error: string, message: string, notice?: string) {
    super(message)
    this.status = status
    this.error = error
    this.notice = notice
  }
}

async function call<T>(path: string, method: string, body?: unknown): Promise<T> {
  const headers: Record<string, string> = {}
  const token = getToken()
  // X-World-Account, never Authorization: World sessions and portal sessions must not share a
  // carrier, or a careers request could quietly ride a student's portal bearer.
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
  try { data = text ? JSON.parse(text) : {} } catch { /* a non-JSON body is handled below */ }

  if (!res.ok) {
    const code = String(data.error ?? 'request_failed')
    throw new CareersError(
      res.status,
      code,
      reasonText(code),
      typeof data.notice === 'string' ? data.notice : undefined,
    )
  }
  return data as T
}

const get = <T,>(p: string) => call<T>(p, 'GET')
const post = <T,>(p: string, body?: unknown) => call<T>(p, 'POST', body)
const del = <T,>(p: string) => call<T>(p, 'DELETE')

// Every refusal code the careers endpoints can send to these surfaces, in words someone can act
// on. The server deliberately sends stable codes rather than prose — phrasing is a product
// decision that belongs next to the interface. Anything unmapped falls back to a neutral sentence
// rather than a raw code, because "posting_budget_exhausted" tells nobody what to do next.
export const REASONS: Record<string, string> = {
  no_session: 'Please sign in to your PCI World account first.',
  bad_name: 'Please give the organisation a name of at least 2 characters.',
  slug_taken: 'That web address is already in use. Please choose another.',
  owner_only: 'Only the organisation owner can do that.',
  not_a_member: 'You are not a member of this organisation.',
  not_permitted: 'You are not permitted to see that. If the organisation was suspended, its access to applicant data stopped at that moment.',
  bad_transition: 'That change is not available from the current state. Reload to see where things stand.',
  conflict: 'Someone else changed this at the same time. Reload and try again.',
  version_conflict: 'This posting changed while you were working. Reload and try again.',
  employer_not_verified: 'Only a verified organisation can publish a job.',
  bad_email: 'Please enter the email address of the account to add.',
  no_such_account: 'No PCI World account was found for that email address.',
  bad_title: 'Please give the posting a title of at least 4 characters.',
  bad_slug: 'That web address is not usable. Please choose another.',
  posting_budget_exhausted: 'This organisation has reached its limit of open postings. Close one before drafting more.',
  currency_required: 'A salary needs its currency. Please choose one, or clear the salary.',
  salary_range_inverted: 'The minimum salary is higher than the maximum.',
  bad_kind: 'That question type is not available. Use text, yes/no, or choice.',
  bad_prompt: 'Please write the question first.',
  bad_decision: 'A decision can only be shortlist or reject.',
  rate_limited: 'You have reached the limit for now. Please try again later.',
  consent_withdrawn: 'The applicant has withdrawn consent, so this application is no longer available to the organisation.',
}

export function reasonText(code: string | null | undefined): string {
  if (!code) return 'That request could not be completed.'
  return REASONS[code] ?? 'That request could not be completed.'
}

/** The words to show for a failed call: the server's own `notice` when it sent one (always
 *  preferred — those are the server's words for what happened), else the REASONS mapping,
 *  else the caller's fallback for non-careers failures (network, etc.). */
export function errText(e: unknown, fallback: string): string {
  return e instanceof CareersError ? (e.notice ?? e.message) : fallback
}

// ── Wire types (mirror the server DTOs in backend/Endpoints/WorldCareers.cs) ──

export interface Employer {
  id: number
  slug: string
  name: string
  state: string // draft | pending_verification | verified | suspended
  website: string | null
  version: number
  /** This account's role in the tenant: 'owner' | 'member'. */
  role: string
  /** Told plainly by the server so the portal can say WHY publish is unavailable rather than
   *  re-deriving the rule from `state`. ADVISORY ONLY: the publish transaction re-checks the
   *  employer row on the server, so a stale copy here can grey a button at worst — it can never
   *  publish anything the server would refuse. */
  may_publish: boolean
}

export interface CreateEmployerResult { ok: boolean; employer_id: number; slug: string; state: string }

/** `notice` is the server's own account of what requesting verification means — a queued human
 *  review, not an approval. Shown verbatim so asking never looks like being verified. */
export interface VerificationResult { ok: boolean; state: string; notice: string }

export interface CreatePostingResult { ok: boolean; posting_id: number; slug: string; state: string }
export interface QuestionResult { ok: boolean; question_id: number }
/** `state` is the server's word for where the row now is — the surfaces render it, never a guess. */
export interface StateResult { ok: boolean; state: string }

/** One applicant row as the employer is allowed to see it. There is deliberately NO score, rank
 *  or match field here and none on the server (§10.7 prohibits automated ranking) — do not add a
 *  client-side one. */
export interface PostingApplicant {
  id: number
  state: string // submitted | shortlisted | rejected
  submitted_at: string | null
  applicant: string | null
  has_cv: boolean
}

export interface ConsentRecord {
  granted_at: string | null
  /** Survives withdrawal: the row is kept and this timestamp is the withdrawal (§5.3). */
  withdrawn_at: string | null
  policy_version: string | null
  active: boolean
}

export interface MyApplication {
  id: number
  state: string // draft | submitted | withdrawn | shortlisted | rejected
  title: string | null
  employer: string | null
  submitted_at: string | null
  withdrawn_at: string | null
  cv_name: string | null
  consent: ConsentRecord
}

export interface ConsentWithdrawResult { ok: boolean; notice: string }

export interface NewPostingBody {
  title: string
  slug?: string
  description?: string
  location?: string
  employment_type?: string
  salary_min_minor?: number
  salary_max_minor?: number
  currency?: string
  closes_at?: string
}

// ── Employer side ──

export const listEmployers = () => get<{ employers: Employer[] }>('/api/world/careers/employers')
export const createEmployer = (name: string, website?: string, slug?: string) =>
  post<CreateEmployerResult>('/api/world/careers/employers', { name, website, slug })
export const requestVerification = (employerId: number) =>
  post<VerificationResult>(`/api/world/careers/employers/${employerId}/request-verification`)
export const addMember = (employerId: number, email: string, role: string) =>
  post<{ ok: boolean }>(`/api/world/careers/employers/${employerId}/members`, { email, role })
export const removeMember = (employerId: number, userId: number) =>
  del<{ ok: boolean }>(`/api/world/careers/employers/${employerId}/members/${userId}`)
export const createPosting = (employerId: number, body: NewPostingBody) =>
  post<CreatePostingResult>(`/api/world/careers/employers/${employerId}/postings`, body)
export const addQuestion = (postingId: number, q: { kind: string; prompt: string; required: boolean }) =>
  post<QuestionResult>(`/api/world/careers/postings/${postingId}/questions`, q)
export const publishPosting = (postingId: number) =>
  post<StateResult>(`/api/world/careers/postings/${postingId}/publish`)
export const closePosting = (postingId: number) =>
  post<StateResult>(`/api/world/careers/postings/${postingId}/close`)
export const listApplicants = (postingId: number) =>
  get<{ applications: PostingApplicant[] }>(`/api/world/careers/postings/${postingId}/applications`)
export const decide = (applicationId: number, decision: 'shortlisted' | 'rejected', note?: string) =>
  post<StateResult>(`/api/world/careers/applications/${applicationId}/decision`, { decision, note })

// ── Applicant side ──

export const myApplications = () =>
  get<{ applications: MyApplication[] }>('/api/world/careers/my/applications')
export const withdrawApplication = (applicationId: number) =>
  post<StateResult>(`/api/world/careers/applications/${applicationId}/withdraw`)
export const withdrawConsent = (applicationId: number) =>
  post<ConsentWithdrawResult>(`/api/world/careers/applications/${applicationId}/consent/withdraw`)
