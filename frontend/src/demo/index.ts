import { CRUD_SECTIONS } from '../admin/crudConfigs'
import { rowsFor, Rand, person, ref, CERTS } from './synth'
import * as F from './fixtures'

export { isDemo, initDemoMode, engageAuto, releaseAuto, setDemo, onDemoChange, demoReason, DEMO_NOTICE } from './mode'

/* The demo resolver: any API path in, plausible data out.
 *
 * Three tiers, most specific first:
 *   1. EXACT   — payloads whose shape the UI genuinely depends on (fixtures.ts)
 *   2. CRUD    — the ~12 generic collections, synthesised from their OWN field configs, so the
 *                columns a screen renders are exactly the columns it gets
 *   3. GENERIC — anything else, shaped from the path and filled by column-name inference
 *
 * Tier 3 is what makes this cover the whole console rather than a demo-able subset: an endpoint
 * nobody anticipated still answers with a well-formed list instead of an error. */

type Json = Record<string, unknown>

/** Strip the query string and normalise a trailing slash. */
const clean = (p: string) => p.split('?')[0].replace(/\/+$/, '') || '/'

/** Numeric id out of a path like /api/admin/identity/merges/3 */
const lastId = (p: string): number => {
  const m = clean(p).match(/\/(\d+)$/)
  return m ? Number(m[1]) : 1
}

/** The collection word a generic path is about — the last non-numeric segment. */
function collectionOf(path: string): string {
  const parts = clean(path).split('/').filter(Boolean).filter((s) => !/^\d+$/.test(s))
  return parts[parts.length - 1] ?? 'items'
}

/** Columns to synthesise for a collection we have no config for. Kept deliberately small and
 *  generic; the important collections are covered by tiers 1 and 2. */
function genericKeys(collection: string): string[] {
  const base = ['id', 'created_at', 'status']
  const c = collection.toLowerCase()
  if (/member|student|user|subscriber|applicant|holder/.test(c)) return [...base, 'first_name', 'last_name', 'email', 'country']
  if (/payment|invoice|refund|transaction|finance/.test(c)) return [...base, 'reference', 'email', 'amount', 'currency', 'product_type']
  if (/ticket|enquir|inquir|support|message|submission/.test(c)) return [...base, 'reference', 'subject', 'email', 'category']
  if (/exam|attempt|session|booking|proctor/.test(c)) return [...base, 'reference', 'email', 'certification', 'scheduled_at']
  if (/credential|certificate|award/.test(c)) return [...base, 'credential_id', 'holder_name', 'credential', 'issued_at', 'expires_at']
  if (/doc|file|download|evidence|attachment/.test(c)) return [...base, 'title', 'filename', 'size_bytes', 'category']
  if (/event|webinar/.test(c)) return [...base, 'title', 'description', 'starts_at', 'location']
  if (/log|audit|history|error|delivery/.test(c)) return [...base, 'action', 'details', 'actor']
  if (/partner|institution|employer/.test(c)) return [...base, 'name', 'country', 'contact_email']
  return [...base, 'title', 'description']
}

/** How many rows a collection shows — enough for pagination to be visible on the big ones. */
function rowCount(collection: string): number {
  if (/audit|log|payment|member|student|subscriber/.test(collection)) return 34
  if (/ticket|enquir|inquir|credential|exam/.test(collection)) return 14
  return 8
}

/** Tier 2: the CRUD collections, from their own declared fields. */
function crudResponse(path: string): Json | null {
  const p = clean(path)
  const m = p.match(/^\/api\/admin\/([a-z0-9_]+)$/)
  if (!m) return null
  const cfg = CRUD_SECTIONS.find((c) => c.name === m[1])
  if (!cfg) return null
  const keys = ['id', ...cfg.fields.map((f) => f.key)]
  return { rows: rowsFor(cfg.name, keys, 9) }
}

/** Tier 1: exact shapes the UI depends on. */
function exactResponse(path: string): Json | null {
  const p = clean(path)

  // ---- student ----
  if (p === '/api/me') return F.studentMe()
  if (p === '/api/me/messages') return F.studentMessages()
  if (p === '/api/me/world-passport/summary') return F.passportSummary()
  if (p === '/api/certifications') return F.certifications()
  if (p === '/api/pricing') return F.pricing(new URLSearchParams(path.split('?')[1] ?? '').get('cert'))
  if (p === '/api/auth/config') return { google: false }
  if (p === '/api/me/consents') return { outstanding: [] }
  if (p === '/api/me/certuvo/access') {
    return {
      enabled: true, status: 'active', username: 'amara.okafor', password: null,
      must_change_password: false, login_url: 'https://certuvo.example.org/login',
      portal_url: 'https://certuvo.example.org', provisioned_at: '2026-01-05T00:00:00Z',
      expires: '2027-01-05T00:00:00Z', notice: null,
    }
  }
  if (p === '/api/world/today') {
    return { available: true, code: 'WC-2026-188', title: 'Recovering a slipped critical path', difficulty: 'intermediate', est_minutes: 25 }
  }

  // ---- PCI World (its own product, its own client, its own nested shapes) ----
  if (p === '/api/world/me/dashboard') return F.worldDashboard()
  if (p === '/api/world/passport') return F.worldPassport()
  if (p === '/api/world/me/shares') return F.worldShares()
  if (p === '/api/world/me/preferences') return F.worldPreferences()
  if (p === '/api/world/me/sessions') return F.worldSessions()
  if (p === '/api/world/me/onboarding') return { state: 'complete', goal: 'certification', timezone: 'Europe/London', weekly_target: 5 }
  if (p === '/api/world/forum/me') {
    return {
      level: 'member', posts_are_reviewed_first: false, may_post_links: true,
      may_edit: true, may_flag: true, hourly_post_budget: 10,
    }
  }
  if (p === '/api/world/forum/categories') return F.forumCategories()
  if (p === '/api/world/careers/my/applications') return F.careerApplications()
  if (p === '/api/world/careers/employers') return F.careerEmployers()
  if (/^\/api\/world\/careers\/postings\/\d+\/applications$/.test(p)) return F.careerApplicants()
  if (p === '/api/world/community/rooms') return F.communityRooms()
  // A room's transcript, then the room itself — the messages path is the longer one, so it is
  // tested first or the room detail would swallow it.
  if (/^\/api\/world\/community\/rooms\/[^/]+\/messages$/.test(p)) return F.communityMessages()
  const room = p.match(/^\/api\/world\/community\/rooms\/([^/]+)$/)
  if (room) return F.communityRoom(room[1])

  // ---- admin ----
  if (p === '/api/admin/overview') return F.adminOverview()
  if (p === '/api/admin/system-check') return F.adminSystemCheck()
  if (p === '/api/admin/notifications') return F.adminNotifications()
  if (p === '/api/admin/identity/merges') return F.adminIdentityMerges()
  if (/^\/api\/admin\/identity\/merges\/\d+$/.test(p)) return F.adminIdentityMergeDetail(lastId(p))
  if (p === '/api/admin/members' || p === '/api/admin/students') return F.adminStudents()
  if (p === '/api/admin/certifications') return F.certifications()
  if (p === '/api/admin/me') {
    return {
      id: 1, email: 'owner@example.org', name: 'Site Owner', role: 'owner',
      is_owner: true, must_change_pw: false, permissions: [],
    }
  }
  // The console's summary endpoints — each is read as a nested object, so each needs a real shape.
  if (p === '/api/admin/payments') return F.paymentTotals()
  if (p === '/api/admin/payments/reconciliation') return F.paymentReconciliation()
  if (p === '/api/admin/codes') return F.discountCodes() as unknown as Json
  if (p === '/api/admin/founding-stats') return F.foundingStats()
  if (p === '/api/support/inbox') return F.supportInbox()
  if (p === '/api/support/metrics') return F.supportMetrics()
  if (p === '/api/admin/forum/queue') return F.forumQueue()
  if (p === '/api/admin/reviews') return F.adminReviews()
  if (p === '/api/admin/reports') return F.adminReports()
  if (p === '/api/admin/analytics/summary') return F.analyticsSummary()
  if (p === '/api/admin/team') return F.adminTeam()
  if (p === '/api/admin/templates') return F.adminTemplates()
  if (p === '/api/admin/templates/analytics') return F.templateAnalytics()
  if (p === '/api/admin/seo/overview') return F.seoOverview()
  if (p === '/api/admin/seo/audit') return F.seoAudit()
  if (p === '/api/admin/seo/integrations') return F.seoIntegrations()
  if (p === '/api/admin/ai-visibility/overview') return F.aiVisibilityOverview()
  if (p === '/api/admin/ai-visibility/crawlers') return F.aiVisibilityCrawlers()
  if (p === '/api/admin/ai-visibility/policy') return F.aiVisibilityPolicy()
  if (p === '/api/admin/campaigns') return F.marketingCampaigns()
  if (p === '/api/admin/suppression') return F.marketingSuppression()
  if (p === '/api/admin/social') return F.socialMedia()
  if (p === '/api/admin/lab/scenarios') return F.labScenarios()
  if (p === '/api/admin/lab/templates') return F.labTemplates()
  if (p === '/api/admin/i18n/coverage') return F.i18nCoverage()
  if (p === '/api/admin/content/overview') return F.contentOverview()

  if (p === '/api/admin/fraud-flags') return F.fraudFlags()
  if (p === '/api/admin/seo/pages') return F.seoPages()
  if (p === '/api/admin/content/backlinks/overview') return F.backlinksOverview()
  if (p === '/api/admin/content/analytics/overview') return F.contentAnalyticsOverview()
  if (p === '/api/admin/content/capabilities') return F.contentCapabilities()
  if (p === '/api/admin/certuvo') return F.certuvoConfig()

  if (p === '/api/admin/settings') {
    return {
      web_site_title: 'Project Controls Institute',
      web_contact_email: 'hello@example.org',
      sp_membership_price: '150',
      exam_pass_mark: '70',
      notify_recipients: 'operations@example.org',
    }
  }
  return null
}

/** Tier 3: a well-formed generic answer for anything not covered above. */
function genericResponse(path: string): Json {
  const p = clean(path)
  const collection = collectionOf(p)

  // A detail path (…/123) answers with a single record rather than a list.
  if (/\/\d+$/.test(p)) {
    const keys = genericKeys(collection)
    const [row] = rowsFor(collection, keys, 1)
    return { ok: true, ...row }
  }

  const rows = rowsFor(collection, genericKeys(collection), rowCount(collection))
  return { ok: true, rows, total: rows.length }
}

/** Resolve a GET. Never throws — a demo that can 500 defeats its own purpose. */
export function demoGet(path: string): unknown {
  try {
    return exactResponse(path) ?? crudResponse(path) ?? genericResponse(path)
  } catch {
    return { ok: true, rows: [] }
  }
}

/** Resolve a mutation. Writes are acknowledged but NOT persisted — they are lost on reload,
 *  which is the honest behaviour when there is no server to keep them. Login is the one that
 *  must return real-looking session material so the portals can proceed past their gate. */
export function demoMutate(path: string, body?: unknown): unknown {
  const p = clean(path)
  const r = new Rand(p)

  if (p === '/api/login' || p === '/api/admin/auth/login') {
    const admin = p.includes('admin')
    return {
      token: 'demo-' + ref(r, 'SESSION'),
      user: admin ? undefined : { id: 1, first_name: 'Amara', last_name: 'Okafor', email: 'amara.okafor@example.org', registration_no: 'PCI-100482' },
      admin: admin ? { id: 1, email: 'owner@example.org', name: 'Site Owner', role: 'owner', must_change_pw: false } : undefined,
      permissions: admin ? [] : undefined,
    }
  }
  if (p === '/api/admin/notifications/test') {
    return { ok: true, sent: 3, recipients: F.adminNotifications().resolved }
  }
  if (p.endsWith('/preview')) {
    const q = new Rand(p)
    return { ok: true, rows: Array.from({ length: 5 }, (_, i) => ({ id: i + 1, ...person(q), change: 'would be updated' })) }
  }
  if (p === '/api/create-checkout-session') {
    return { url: null, demo: true, message: 'Checkout is disabled in demonstration mode.' }
  }
  if (/\/(approve|reject|release|invalidate|reinstate|revoke|activate)$/.test(p)) {
    return { ok: true, result: { source_user_id: 811, target_user_id: 204, correlation_id: 'cor_demo' } }
  }
  // The generic acknowledgement: echo an id so optimistic UI has something to key on.
  const b = (body && typeof body === 'object' ? body : {}) as Json
  return { ok: true, id: lastId(p) || r.int(100, 999), ...b }
}


export const DEMO_CERTS = CERTS
