import { api, ApiError } from './client'

/** Start a Stripe Checkout from inside the portal. The backend returns the hosted payment URL;
 * after paying, Stripe sends the student back to /app/billing and the webhook applies the
 * membership/entitlement to this same account (matched by email). */
export async function startCheckout(opts: {
  product: 'membership' | 'exam' | 'bundle' | 'renewal' | 'recert'
  email: string
  cert?: string
  code?: string
  first?: string
  last?: string
}): Promise<void> {
  const res = await api.post<{ url?: string }>('/api/create-checkout-session', {
    product: opts.product,
    email: opts.email,
    cert: opts.cert,
    code: opts.code,
    first: opts.first,
    last: opts.last,
    portal: '1',
  })
  if (!res.url) throw new Error('Could not start checkout.')
  window.location.href = res.url
}

export function checkoutErrorMessage(e: unknown): string {
  if (e instanceof ApiError && e.status === 503)
    return 'Online payment is not open yet — you will be able to pay right here as soon as payments go live.'
  if (e instanceof ApiError && e.status === 400) {
    // A rejected discount code carries its own explanation (e.g. scoped to the other product) — show it.
    const body = e.body as { error?: string; message?: string } | null
    if (body?.error === 'code_invalid' && body.message) return body.message
    // Exam purchase blocked until membership is paid — surface the server's guidance.
    if (body?.error === 'membership_required') return body.message || 'Please pay your membership fee before purchasing an exam.'
    return 'That selection is not available right now. Please refresh and try again.'
  }
  return e instanceof Error ? e.message : 'Could not start checkout.'
}
