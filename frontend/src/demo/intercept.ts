import { isDemo, engageAuto, noteSuccess } from './mode'

/* One implementation of the demonstration-mode rule, for the FIVE independent API clients this
 * codebase has (portal/admin bearer, World account, forum, careers, community room).
 *
 * They are separate on purpose — each carries a different credential in a different header, and
 * merging them would let one product's session ride another's request. But the rule about when a
 * request is answered from demonstration data is a single rule, and five copies of it would be
 * five places for it to drift. So the credentials stay apart and the rule lives here.
 *
 * The rule, in full (see mode.ts for why):
 *   - demo mode on          → never touch the network
 *   - fetch threw           → stand in ONLY if this deployment has never had a successful response
 *   - 5xx                   → same test; a misconfigured backend answers exactly like this
 *   - anything else         → the backend is alive; record that and hand the response back
 */

export type Attempt = { demo: true; data: unknown } | { demo: false; res: Response }

/**
 * Perform a request under the demonstration rule.
 *
 * Returns either the live `Response` for the caller to parse in its own way — every client shapes
 * its own errors and this must not flatten that — or the demonstration payload to answer with.
 */
export async function demoAwareFetch(
  path: string,
  init: RequestInit,
  body?: unknown,
): Promise<Attempt> {
  const method = (init.method ?? 'GET').toUpperCase()
  if (isDemo()) return { demo: true, data: await answer(path, method, body) }

  let res: Response
  try {
    res = await fetch(path, init)
  } catch (e) {
    if (engageAuto()) return { demo: true, data: await answer(path, method, body) }
    throw e
  }
  if (res.status >= 500 && engageAuto()) return { demo: true, data: await answer(path, method, body) }
  noteSuccess()
  return { demo: false, res }
}

/** Resolve the demonstration payload. The small delay keeps loading and skeleton states visible —
 *  they are half of what a reviewer is here to look at, and an instant answer hides them. */
async function answer(path: string, method: string, body?: unknown): Promise<unknown> {
  const { demoGet, demoMutate } = await import('./index')
  await new Promise((r) => setTimeout(r, 120))
  return method === 'GET' ? demoGet(path) : demoMutate(path, body)
}
