import { describe, it, expect, vi, afterEach } from 'vitest'
import { makeClient, ApiError, UnauthorizedError } from './client'

// A fake fetch Response with a JSON (or empty) body.
function fakeFetch(status: number, body?: unknown) {
  return vi.fn().mockResolvedValue({
    ok: status >= 200 && status < 300,
    status,
    text: async () => (body === undefined ? '' : JSON.stringify(body)),
  } as Response)
}

describe('makeClient (fetch wrapper)', () => {
  afterEach(() => vi.unstubAllGlobals())

  it('adds the Authorization header only once a token is set', async () => {
    const f = fakeFetch(200, { ok: true })
    vi.stubGlobal('fetch', f)
    const c = makeClient('test.key.auth')
    c.setToken(null)
    await c.get('/api/x')
    expect((f.mock.calls[0][1] as RequestInit).headers as Record<string, string>).not.toHaveProperty('Authorization')
    c.setToken('tok123')
    await c.get('/api/x')
    expect(((f.mock.calls[1][1] as RequestInit).headers as Record<string, string>).Authorization).toBe('Bearer tok123')
    c.setToken(null)
  })

  it('throws UnauthorizedError on 401 and fires the central handler', async () => {
    vi.stubGlobal('fetch', fakeFetch(401, { error: 'no_token' }))
    const c = makeClient('test.key.401')
    const handler = vi.fn()
    c.onUnauthorized(handler)
    await expect(c.get('/api/me')).rejects.toBeInstanceOf(UnauthorizedError)
    expect(handler).toHaveBeenCalledTimes(1)
  })

  it('allowUnauthorized: a 401 throws ApiError (not Unauthorized) and does NOT fire the handler', async () => {
    vi.stubGlobal('fetch', fakeFetch(401, { error: 'no_token' }))
    const c = makeClient('test.key.allow')
    const handler = vi.fn()
    c.onUnauthorized(handler)
    let err: unknown
    try { await c.post('/api/login', {}, { allowUnauthorized: true }) } catch (e) { err = e }
    expect(err).toBeInstanceOf(ApiError)
    expect(err).not.toBeInstanceOf(UnauthorizedError)
    expect(handler).not.toHaveBeenCalled()
  })

  it('wraps a network rejection as ApiError with status 0', async () => {
    vi.stubGlobal('fetch', vi.fn().mockRejectedValue(new Error('down')))
    const c = makeClient('test.key.net')
    await expect(c.get('/api/x')).rejects.toMatchObject({ status: 0 })
  })
})
