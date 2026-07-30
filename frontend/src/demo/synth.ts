/* Deterministic sample-data synthesis.
 *
 * Everything here is seeded from a string, so the same screen shows the same people, references
 * and dates on every reload. A demo that reshuffles itself on each render is unreviewable — you
 * cannot point at a row and discuss it — and screenshots would never match.
 *
 * Values are inferred from FIELD NAMES (email, *_at, amount, status, published…), which is what
 * makes one small generator able to fill hundreds of endpoints plausibly rather than filling
 * every column with "Lorem ipsum". */

/** xmur3 + mulberry32: a tiny, dependency-free seeded PRNG. */
function rng(seed: string): () => number {
  let h = 1779033703 ^ seed.length
  for (let i = 0; i < seed.length; i++) {
    h = Math.imul(h ^ seed.charCodeAt(i), 3432918353)
    h = (h << 13) | (h >>> 19)
  }
  let a = h >>> 0
  return () => {
    a |= 0
    a = (a + 0x6d2b79f5) | 0
    let t = Math.imul(a ^ (a >>> 15), 1 | a)
    t = (t + Math.imul(t ^ (t >>> 7), 61 | t)) ^ t
    return ((t ^ (t >>> 14)) >>> 0) / 4294967296
  }
}

export class Rand {
  private r: () => number
  constructor(seed: string) { this.r = rng(seed) }
  next() { return this.r() }
  int(min: number, max: number) { return min + Math.floor(this.r() * (max - min + 1)) }
  pick<T>(xs: readonly T[]): T { return xs[Math.floor(this.r() * xs.length) % xs.length] }
  bool(p = 0.5) { return this.r() < p }
  /** An ISO timestamp `daysBack` days ago at most, always in the past. */
  pastISO(maxDaysBack = 400): string {
    const ms = Date.parse('2026-07-01T09:00:00Z') - this.int(0, maxDaysBack) * 86400000 - this.int(0, 86399) * 1000
    return new Date(ms).toISOString()
  }
  /** An ISO timestamp in the future — expiry dates, deadlines. */
  futureISO(maxDaysAhead = 900): string {
    const ms = Date.parse('2026-07-01T09:00:00Z') + this.int(1, maxDaysAhead) * 86400000
    return new Date(ms).toISOString()
  }
}

const FIRST = ['Amara', 'Sam', 'Mia', 'Noor', 'Diego', 'Priya', 'Liam', 'Zara', 'Tomas', 'Aisha',
  'Kwame', 'Elena', 'Hiro', 'Fatima', 'Lucas', 'Ingrid', 'Omar', 'Chen', 'Rosa', 'Yusuf']
const LAST = ['Okafor', 'Rivera', 'Chen', 'Diaz', 'Haddad', 'Nair', 'Murphy', 'Ali', 'Novak', 'Bello',
  'Mensah', 'Petrov', 'Tanaka', 'Rahman', 'Silva', 'Larsen', 'Farouk', 'Wu', 'Santos', 'Demir']
const COMPANY = ['Meridian Infrastructure', 'Kestrel Energy', 'Northgate Rail', 'Atlas Marine',
  'Verdant Utilities', 'Copperline Mining', 'Halcyon Aviation', 'Steelbrook Construction']
const COUNTRY = ['United Kingdom', 'Nigeria', 'India', 'Brazil', 'Germany', 'Singapore', 'Canada',
  'South Africa', 'Australia', 'United Arab Emirates']
const ROLE = ['Project Controls Manager', 'Senior Planner', 'Cost Engineer', 'Scheduling Lead',
  'Risk Analyst', 'PMO Director', 'Estimator', 'Change Manager']

export const CERTS = [
  { code: 'PML-AI', name: 'PCI Project Management Leader – AI' },
  { code: 'PFL-AI', name: 'PCI Project Finance Leader – AI' },
  { code: 'PCL-AI', name: 'PCI Project Controls Leader – AI' },
  { code: 'PRM-AI', name: 'PCI Project Risk Manager – AI' },
]

export const person = (r: Rand) => {
  const first = r.pick(FIRST)
  const last = r.pick(LAST)
  return {
    first_name: first,
    last_name: last,
    name: `${first} ${last}`,
    email: `${first}.${last}`.toLowerCase().replace(/[^a-z.]/g, '') + '@example.org',
  }
}

export const company = (r: Rand) => r.pick(COMPANY)
export const country = (r: Rand) => r.pick(COUNTRY)
export const jobRole = (r: Rand) => r.pick(ROLE)

/** Reference codes that look like the platform's own (PCI-…, TKT-…). */
export const ref = (r: Rand, prefix: string) =>
  `${prefix}-${r.int(1000, 9999)}${String.fromCharCode(65 + r.int(0, 25))}${r.int(10, 99)}`

const STATUS_BY_HINT: Record<string, readonly string[]> = {
  payment: ['paid', 'paid', 'paid', 'refunded', 'failed'],
  ticket: ['open', 'awaiting_student', 'resolved', 'closed'],
  credential: ['active', 'active', 'active', 'expired', 'revoked'],
  application: ['submitted', 'under_review', 'approved', 'rejected'],
  merge: ['pending', 'approved', 'rejected'],
  exam: ['booked', 'in_progress', 'submitted', 'released'],
  default: ['active', 'pending', 'approved', 'closed'],
}

/** Infer a plausible value for one column from its NAME — the trick that lets a single generator
 *  fill hundreds of endpoints without a bespoke fixture for each. */
export function valueFor(key: string, r: Rand, ctx: { index: number; collection: string }): unknown {
  const k = key.toLowerCase()
  const p = person(r)

  if (k === 'id') return ctx.index + 1
  if (k.endsWith('_id') && !k.includes('credential')) return r.int(100, 999)
  if (k === 'email' || k.endsWith('_email')) return p.email
  if (k === 'first_name') return p.first_name
  if (k === 'last_name') return p.last_name
  if (k === 'holder_name' || k === 'student_name' || k === 'name' || k.endsWith('_name')) return p.name
  if (k === 'company' || k === 'employer' || k === 'organisation' || k === 'organization') return company(r)
  if (k === 'country') return country(r)
  if (k === 'current_role' || k === 'job_title' || k === 'title' || k === 'position') {
    return ctx.collection === 'news' || ctx.collection === 'resources' || ctx.collection === 'media_assets'
      ? `${r.pick(['Guidance on', 'Introducing', 'A practical view of', 'Standards update:'])} ${r.pick(['earned value', 'schedule risk', 'cost baselines', 'AI in project controls', 'change control'])}`
      : jobRole(r)
  }
  if (k === 'credential_id') return `${r.pick(CERTS).code}-2026-${String(ctx.index + 1).padStart(4, '0')}`
  if (k === 'credential' || k === 'certification' || k === 'cert_code') return r.pick(CERTS).code
  if (k === 'reference') return ref(r, 'PCI')
  if (k === 'correlation_id') return `cor_${r.int(100000, 999999)}`
  if (k === 'status' || k === 'session_status' || k.endsWith('_status')) {
    const hint = Object.keys(STATUS_BY_HINT).find((h) => ctx.collection.includes(h) || k.includes(h))
    return r.pick(STATUS_BY_HINT[hint ?? 'default'])
  }
  if (k.endsWith('_at') || k === 'date' || k.endsWith('_date')) {
    return k.includes('expire') || k.includes('deadline') ? r.futureISO() : r.pastISO()
  }
  if (k.includes('amount') || k.includes('price') || k.includes('revenue') || k === 'total') return r.int(15, 480) * 5
  if (k.includes('currency')) return 'USD'
  if (k.includes('count') || k.startsWith('n_') || k === 'n' || k.includes('_hours')) return r.int(0, 40)
  if (k === 'published' || k === 'visible' || k === 'active' || k === 'enabled' || k.startsWith('is_')) return r.bool(0.75) ? 1 : 0
  if (k === 'sort_order') return (ctx.index + 1) * 10
  if (k === 'url' || k.endsWith('_url') || k === 'link') return `https://example.org/${ctx.collection}/${ctx.index + 1}`
  if (k === 'slug') return `${ctx.collection}-item-${ctx.index + 1}`
  if (k.includes('description') || k.includes('body') || k.includes('summary') || k.includes('reason') || k.includes('note')) {
    return r.pick([
      'Recorded during the standard review cycle; no further action is outstanding.',
      'Submitted with supporting evidence and accepted by the reviewing panel.',
      'Awaiting confirmation from the candidate before this can be progressed.',
      'Cross-checked against the register and confirmed consistent.',
    ])
  }
  if (k.includes('question')) return r.pick([
    'Which technique separates schedule variance from cost variance?',
    'What does a CPI below 1.0 indicate about a project?',
    'When should a change request be escalated to the sponsor?',
  ])
  if (k.includes('category') || k === 'domain' || k === 'kind' || k === 'type' || k.endsWith('_type')) {
    return r.pick(['Cost', 'Schedule', 'Risk', 'Governance', 'Quality'])
  }
  return `${key.replace(/_/g, ' ')} ${ctx.index + 1}`
}

/** Build `n` rows for a collection from a list of column names. */
export function rowsFor(collection: string, keys: string[], n: number): Record<string, unknown>[] {
  return Array.from({ length: n }, (_, i) => {
    const r = new Rand(`${collection}:${i}`)
    const row: Record<string, unknown> = {}
    for (const k of keys) row[k] = valueFor(k, r, { index: i, collection })
    if (!('id' in row)) row.id = i + 1
    return row
  })
}
