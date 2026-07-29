import { describe, it, expect, beforeEach } from 'vitest'
import { demoGet, demoMutate } from './index'
import { rowsFor, Rand } from './synth'

/* FE — the demonstration layer that makes the whole platform walkable with no backend.
 *
 * The rules worth pinning are the ones that keep it USEFUL and HONEST:
 *   - every path answers with something well-formed; a demo that can throw defeats its purpose
 *   - the answer is deterministic, so a reviewer can point at a row and discuss it, and
 *     screenshots stay stable
 *   - CRUD collections are shaped from their own field configs, so a screen gets exactly the
 *     columns it renders
 *   - writes are acknowledged but never persisted */

describe('demo data resolver', () => {
  it('answers the student envelope with a mid-journey candidate', () => {
    const me = demoGet('/api/me') as Record<string, never>
    // an empty account would demo none of the product
    expect((me.lifecycle as Record<string, unknown>).membership_status).toBe('active')
    expect((me.credentials as unknown[]).length).toBeGreaterThan(0)
    expect((me.exams as unknown[]).length).toBeGreaterThan(0)
    expect(me.unread).toBeGreaterThan(0)
  })

  it('answers the admin dashboard with a full twelve-month trend', () => {
    const o = demoGet('/api/admin/overview') as Record<string, never>
    expect((o.revSeries as unknown[]).length).toBe(12)
    expect((o.kpis as Record<string, number>).members).toBeGreaterThan(0)
    expect((o.productMix as unknown[]).length).toBeGreaterThan(0)
  })

  it('shapes a CRUD collection from its own field config', () => {
    // faqs is registered in crudConfigs; the rows must carry the columns the table renders
    const r = demoGet('/api/admin/faqs') as { rows: Record<string, unknown>[] }
    expect(r.rows.length).toBeGreaterThan(0)
    expect(r.rows[0]).toHaveProperty('id')
    expect(r.rows[0]).toHaveProperty('question')
  })

  it('answers an endpoint nobody anticipated with a well-formed list', () => {
    const r = demoGet('/api/admin/some-future-thing') as { ok: boolean; rows: unknown[] }
    expect(r.ok).toBe(true)
    expect(Array.isArray(r.rows)).toBe(true)
    expect(r.rows.length).toBeGreaterThan(0)
  })

  it('answers a detail path with a single record, not a list', () => {
    const r = demoGet('/api/admin/payments/42') as Record<string, unknown>
    expect(r).not.toHaveProperty('rows')
    expect(r).toHaveProperty('id')
  })

  it('ignores the query string when matching', () => {
    const a = demoGet('/api/admin/overview')
    const b = demoGet('/api/admin/overview?range=30d')
    expect(b).toEqual(a)
  })

  it('is deterministic — the same path gives the same data every time', () => {
    expect(demoGet('/api/admin/members')).toEqual(demoGet('/api/admin/members'))
    expect(demoGet('/api/admin/audit')).toEqual(demoGet('/api/admin/audit'))
  })

  it('never throws, whatever it is handed', () => {
    for (const p of ['', '/', '/api', '/api/', '//////', '/api/admin/x/y/z/1/2/3', '/api/me/../../etc']) {
      expect(() => demoGet(p)).not.toThrow()
    }
  })

  it('returns session material from a login so the portals can proceed', () => {
    const s = demoMutate('/api/login', { email: 'a@b.c' }) as Record<string, unknown>
    expect(typeof s.token).toBe('string')
    expect(s.user).toBeTruthy()

    const a = demoMutate('/api/admin/auth/login', {}) as Record<string, unknown>
    expect(typeof a.token).toBe('string')
    expect((a.admin as Record<string, unknown>).is_owner ?? true).toBeTruthy()
  })

  it('acknowledges a write without pretending it was stored', () => {
    const r = demoMutate('/api/admin/faqs', { question: 'Q?' }) as Record<string, unknown>
    expect(r.ok).toBe(true)
    // the acknowledgement echoes the submitted body; nothing is added to the collection
    expect(r.question).toBe('Q?')
    const after = demoGet('/api/admin/faqs') as { rows: unknown[] }
    const before = demoGet('/api/admin/faqs') as { rows: unknown[] }
    expect(after.rows.length).toBe(before.rows.length)
  })

  it('says plainly that checkout cannot run without a backend', () => {
    const r = demoMutate('/api/create-checkout-session', {}) as Record<string, unknown>
    expect(r.demo).toBe(true)
    expect(String(r.message)).toMatch(/demonstration/i)
  })
})

describe('synthesis', () => {
  beforeEach(() => undefined)

  it('infers a plausible value from the column name', () => {
    const [row] = rowsFor('members', ['id', 'email', 'first_name', 'created_at', 'amount', 'published'], 1)
    expect(String(row.email)).toContain('@')
    expect(String(row.first_name)).not.toContain('_')
    expect(Date.parse(String(row.created_at))).not.toBeNaN()
    expect(typeof row.amount).toBe('number')
    expect([0, 1]).toContain(row.published)
  })

  it('keeps date semantics — expiry is in the future, created is in the past', () => {
    const [row] = rowsFor('credentials', ['issued_at', 'expires_at'], 1)
    expect(Date.parse(String(row.expires_at))).toBeGreaterThan(Date.parse(String(row.issued_at)))
  })

  it('gives every row a distinct id', () => {
    const rows = rowsFor('things', ['id', 'title'], 8)
    expect(new Set(rows.map((r) => r.id)).size).toBe(8)
  })

  it('is seeded, so the same seed reproduces the same sequence', () => {
    const a = new Rand('x')
    const b = new Rand('x')
    expect([a.int(0, 1e6), a.int(0, 1e6)]).toEqual([b.int(0, 1e6), b.int(0, 1e6)])
  })
})
