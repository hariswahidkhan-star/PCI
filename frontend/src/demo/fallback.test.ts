import { describe, it, expect, vi, beforeEach } from 'vitest'

/* The automatic fallback rule, tested from the untouched module state it is actually about.
 *
 * This is the one rule in demonstration mode where getting it wrong is worse than not having the
 * feature: engage too eagerly and a student on a healthy deployment sees an invented name and
 * invented credentials during a two-second connection blip, with nothing to tell them apart from
 * their own record. So both directions are pinned here.
 *
 * The module graph is reset per test because `everSucceeded` is deliberately a once-only latch —
 * a shared instance would make the second test depend on the first. Note also that the global
 * harness (src/test/setup.ts) declares the backend reachable for every OTHER test file; these
 * import fresh copies precisely to escape that. */

async function freshClient() {
  vi.resetModules()
  const mode = await import('./mode')
  const { makeClient } = await import('../api/client')
  return { mode, client: makeClient('test.fallback.key') }
}

describe('automatic demonstration fallback', () => {
  beforeEach(() => {
    vi.unstubAllGlobals()
    try { localStorage.removeItem('pci.demo') } catch { /* ignore */ }
  })

  it('stands in for a backend that has never answered', async () => {
    vi.stubGlobal('fetch', vi.fn().mockRejectedValue(new TypeError('Failed to fetch')))
    const { mode, client } = await freshClient()

    const me = await client.get<{ lifecycle: { membership_status: string } }>('/api/me')
    expect(me.lifecycle.membership_status).toBe('active')
    // and it says so — the banner keys off exactly this
    expect(mode.isDemo()).toBe(true)
    expect(mode.demoReason()).toBe('auto')
  })

  it('does NOT stand in once the backend has answered — the honest error survives a blip', async () => {
    const { mode, client } = await freshClient()

    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response('{"ok":true}', { status: 200 })))
    await client.get('/api/health')

    vi.stubGlobal('fetch', vi.fn().mockRejectedValue(new TypeError('Failed to fetch')))
    await expect(client.get('/api/me')).rejects.toMatchObject({ status: 0 })
    expect(mode.isDemo()).toBe(false)
  })

  it('treats a 5xx like an unreachable backend — that is how a misconfigured deploy answers', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response('boom', { status: 503 })))
    const { client } = await freshClient()

    const r = await client.get<{ rows: unknown[] }>('/api/admin/faqs')
    expect(Array.isArray(r.rows)).toBe(true)
  })

  it('does not remember auto mode, so the next page load probes the backend again', async () => {
    vi.stubGlobal('fetch', vi.fn().mockRejectedValue(new TypeError('Failed to fetch')))
    const { mode, client } = await freshClient()
    await client.get('/api/me')
    expect(mode.isDemo()).toBe(true)

    // Once engaged, requests short-circuit before the network, so THIS session cannot see a
    // recovery. What must hold is that nothing was written down: a fresh load starts from off
    // and asks the backend for real. Storage is the whole mechanism, so assert on storage.
    expect(localStorage.getItem('pci.demo')).toBeNull()

    vi.resetModules()
    const next = await import('./mode')
    next.initDemoMode()
    expect(next.isDemo()).toBe(false)
  })

  it('covers PCI World too — it has its own client and must follow the same rule', async () => {
    vi.stubGlobal('fetch', vi.fn().mockRejectedValue(new TypeError('Failed to fetch')))
    vi.resetModules()
    const { api } = await import('../world/api')

    const d = await api<{ passport: { state: string } }>('/api/world/me/dashboard')
    expect(d.passport.state).toBe('published')
  })
})
