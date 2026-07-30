import { describe, expect, it, vi, beforeEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import Onboarding from './Onboarding'

// The walkthrough posts each step completion to the server, which owns the ORDER — these tests
// pin the client's side of that contract: the right endpoint per step, preferences saved before
// the step advances, and the final step chaining the completed post.

function jsonOk(body: unknown = { ok: true }): Response {
  return new Response(JSON.stringify(body), { status: 200, headers: { 'Content-Type': 'application/json' } })
}

describe('World onboarding walkthrough', () => {
  const calls: { url: string; body: unknown }[] = []

  beforeEach(() => {
    calls.length = 0
    vi.stubGlobal('fetch', vi.fn(async (url: RequestInfo | URL, init?: RequestInit) => {
      calls.push({ url: String(url), body: init?.body ? JSON.parse(String(init.body)) : undefined })
      return jsonOk()
    }))
    localStorage.clear()
  })

  it('welcome step posts its completion and reloads', async () => {
    const onDone = vi.fn(async () => undefined)
    render(<Onboarding state="not_started" onDone={onDone} />)
    fireEvent.click(screen.getByText('Got it — continue'))
    await waitFor(() => expect(onDone).toHaveBeenCalled())
    expect(calls).toEqual([{ url: '/api/world/me/onboarding', body: { step: 'welcome' } }])
  })

  it('goal step saves the preference BEFORE advancing', async () => {
    const onDone = vi.fn(async () => undefined)
    render(<Onboarding state="welcome" onDone={onDone} />)
    fireEvent.change(screen.getByLabelText('Goal'), { target: { value: 'certification_prep' } })
    fireEvent.click(screen.getByText('Save & continue'))
    await waitFor(() => expect(onDone).toHaveBeenCalled())
    expect(calls.map(c => c.url)).toEqual(['/api/world/me/preferences', '/api/world/me/onboarding'])
    expect(calls[0].body).toEqual({ goal: 'certification_prep' })
    expect(calls[1].body).toEqual({ step: 'goal' })
  })

  it('the privacy step chains the completed post', async () => {
    const onDone = vi.fn(async () => undefined)
    render(<Onboarding state="preferences" onDone={onDone} />)
    fireEvent.click(screen.getByText('I understand — finish setup'))
    await waitFor(() => expect(onDone).toHaveBeenCalled())
    expect(calls.map(c => c.body)).toEqual([{ step: 'privacy' }, { step: 'completed' }])
  })

  it('a server refusal shows the message and does not reload', async () => {
    vi.stubGlobal('fetch', vi.fn(async () =>
      new Response(JSON.stringify({ error: 'out_of_order', message: 'Complete the earlier steps first.' }), { status: 400 })))
    const onDone = vi.fn(async () => undefined)
    render(<Onboarding state="not_started" onDone={onDone} />)
    fireEvent.click(screen.getByText('Got it — continue'))
    await waitFor(() => expect(screen.getByRole('alert')).toHaveTextContent('Complete the earlier steps first.'))
    expect(onDone).not.toHaveBeenCalled()
  })
})
