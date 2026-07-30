import { describe, it, expect } from 'vitest'
import { checkoutErrorMessage } from './checkout'
import { ApiError } from './client'

// Maps API failures to the user-facing copy the checkout button shows.
describe('checkoutErrorMessage', () => {
  it('explains a 503 (payments not live yet)', () => {
    expect(checkoutErrorMessage(new ApiError(503, 'x', null))).toContain('not open yet')
  })

  it('surfaces a rejected discount-code message verbatim', () => {
    const e = new ApiError(400, 'x', { error: 'code_invalid', message: 'Code scoped to membership' })
    expect(checkoutErrorMessage(e)).toBe('Code scoped to membership')
  })

  it('asks the user to retry when the idempotency key is missing', () => {
    const e = new ApiError(400, 'x', { error: 'idempotency_key_required' })
    expect(checkoutErrorMessage(e)).toContain('try checkout again')
  })

  it('gives membership-required guidance', () => {
    const e = new ApiError(400, 'x', { error: 'membership_required', message: '' })
    expect(checkoutErrorMessage(e)).toContain('membership')
  })

  it('falls back to a generic message for other 400s', () => {
    const e = new ApiError(400, 'x', { error: 'something_else' })
    expect(checkoutErrorMessage(e)).toContain('not available right now')
  })

  it('uses an Error message, or a default for non-errors', () => {
    expect(checkoutErrorMessage(new Error('boom'))).toBe('boom')
    expect(checkoutErrorMessage('nope')).toBe('Could not start checkout.')
  })
})
