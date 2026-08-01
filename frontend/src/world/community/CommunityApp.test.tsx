import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import CommunityApp from './CommunityApp'

// PCI World community rooms — the guest surface's contract.
//
// Two properties matter more than the rest and are asserted first: unpublished text is never shown
// as though it posted, and a withheld message does not cost the participant what they typed. The
// accessibility semantics are asserted as SEMANTICS (role, aria-live) rather than as class names,
// because a screen reader reads the former and ignores the latter.

const ROOM = {
  slug: 'lobby', title: 'Lobby', description: 'General discussion', topic: null,
  category: null, locale: 'en', room_type: 'community', state: 'open',
  capacity: 50, guests_welcome: true, slow_mode_seconds: 0, participants: 3,
  accepting: true, images_allowed: false, rules_version: 'v1',
  pinned_welcome: null, retention_class: 'standard',
}

describe('World community rooms', () => {
  const calls: { url: string; method: string; body: unknown }[] = []
  let responder: (url: string, method: string) => unknown

  beforeEach(() => {
    calls.length = 0
    sessionStorage.clear()
    responder = url => {
      if (url.includes('/community/rooms/lobby/messages')) return { messages: [] }
      if (url.includes('/community/rooms/lobby')) return ROOM
      if (url.includes('/community/rooms')) return { rooms: [ROOM] }
      return {}
    }
    vi.stubGlobal('fetch', vi.fn(async (url: string, init?: RequestInit) => {
      const method = init?.method ?? 'GET'
      calls.push({ url, method, body: init?.body ? JSON.parse(String(init.body)) : undefined })
      const payload = responder(url, method)
      const status = (payload as { _status?: number })?._status ?? 200
      return {
        ok: status < 400, status,
        text: async () => JSON.stringify(payload),
      } as unknown as Response
    }))
  })
  afterEach(() => vi.unstubAllGlobals())

  async function enterRoom() {
    render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')
    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'Ali Hassan' } })
    fireEvent.change(screen.getByLabelText('Date of birth'), { target: { value: '1990-05-04' } })
    fireEvent.change(screen.getByLabelText('Country or region'), { target: { value: 'GB' } })
    fireEvent.click(screen.getByRole('checkbox'))
    responder = url => {
      if (url.includes('guest-sessions')) return { ok: true, token: 'tok-1', display_name: 'Ali Hassan' }
      if (url.includes('/messages')) return { messages: [] }
      if (url.includes('/community/rooms/lobby')) return ROOM
      return { rooms: [ROOM] }
    }
    fireEvent.click(screen.getByRole('button', { name: 'Join room' }))
    await screen.findByLabelText('Your message')
  }

  // ── The eligibility gate, in the interface ──

  it('cannot be submitted without the eligibility declaration', async () => {
    // The server refuses entry without it (CCP-P1-003). Enforcing it here too means someone finds
    // out before they have typed a display name and accepted the rules, rather than after.
    render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')

    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'Ali Hassan' } })
    fireEvent.click(screen.getByRole('checkbox'))
    expect(screen.getByRole('button', { name: 'Join room' })).toBeDisabled()

    fireEvent.change(screen.getByLabelText('Date of birth'), { target: { value: '1990-05-04' } })
    expect(screen.getByRole('button', { name: 'Join room' })).toBeDisabled()

    fireEvent.change(screen.getByLabelText('Country or region'), { target: { value: 'GB' } })
    expect(screen.getByRole('button', { name: 'Join room' })).not.toBeDisabled()
  })

  it('tells the person their date of birth is not kept', async () => {
    // The gate records whether the minimum was met, not when anyone was born. Saying so at the
    // point of asking is the difference between a proportionate check and a data grab.
    render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Date of birth')
    expect(screen.getByText(/not stored/i)).toBeTruthy()
  })

  // ── The safety rule, in the interface ──

  it('never renders an unpublished message as posted', async () => {
    await enterRoom()
    responder = url => {
      if (url.includes('/messages') && url.includes('afterSequence')) return { messages: [] }
      if (url.endsWith('/messages')) {
        return { published: false, sequence: null, status: 'blocked', reason: 'abuse_blocked', ejected: false, appeal: null }
      }
      return ROOM
    }
    fireEvent.change(screen.getByLabelText('Your message'), { target: { value: 'you are an idiot' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send' }))

    await screen.findByRole('status')
    // The transcript must not contain it. This is the interface half of the guarantee the backend
    // enforces — a UI that echoed optimistically would defeat a correct server.
    const log = screen.getByRole('log')
    expect(log.textContent).not.toContain('you are an idiot')
  })

  it('keeps what the participant typed when a message is withheld', async () => {
    await enterRoom()
    responder = url => {
      if (url.includes('afterSequence')) return { messages: [] }
      if (url.endsWith('/messages')) {
        return { published: false, sequence: null, status: 'quarantined', reason: 'profanity_review', ejected: false, appeal: null }
      }
      return ROOM
    }
    const box = screen.getByLabelText('Your message') as HTMLTextAreaElement
    fireEvent.change(box, { target: { value: 'quoting a policy document' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send' }))

    await screen.findByRole('status')
    // Losing someone's words because a classifier disagreed is a second punishment on top of the
    // moderation decision.
    expect(box.value).toBe('quoting a policy document')
  })

  it('clears the composer only when the message actually published', async () => {
    await enterRoom()
    responder = url => {
      if (url.includes('afterSequence')) return { messages: [] }
      if (url.endsWith('/messages')) {
        return { published: true, sequence: 1, status: 'allowed', reason: 'ok', ejected: false, appeal: null }
      }
      return ROOM
    }
    const box = screen.getByLabelText('Your message') as HTMLTextAreaElement
    fireEvent.change(box, { target: { value: 'a normal question' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send' }))
    await waitFor(() => expect(box.value).toBe(''))
  })

  // ── Accessibility semantics ──

  it('presents the transcript as a polite live log', async () => {
    await enterRoom()
    const log = screen.getByRole('log')
    // A screen reader acts on these; a CSS class tells it nothing.
    expect(log.getAttribute('aria-live')).toBe('polite')
    expect(log.getAttribute('aria-relevant')).toBe('additions')
    expect(log.getAttribute('aria-label')).toContain('Lobby')
    // And the transcript inside it is still a LIST. role="log" on the <ol> itself would override
    // the implicit list role and orphan every item — axe caught exactly that.
    expect(screen.getByRole('list')).toBeTruthy()
  })

  it('conveys a withheld message in text rather than by colour alone', async () => {
    await enterRoom()
    responder = url => {
      if (url.includes('afterSequence')) return { messages: [] }
      if (url.endsWith('/messages')) {
        return { published: false, sequence: null, status: 'blocked', reason: 'pii_blocked', ejected: false, appeal: null }
      }
      return ROOM
    }
    fireEvent.change(screen.getByLabelText('Your message'), { target: { value: 'call me on 07700900123' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send' }))

    const status = await screen.findByRole('status')
    expect(status.textContent).toMatch(/phone numbers, emails and addresses/i)
  })

  it('associates the entry error with the field that needs fixing', async () => {
    render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')

    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'admin' } })
    fireEvent.change(screen.getByLabelText('Date of birth'), { target: { value: '1990-05-04' } })
    fireEvent.change(screen.getByLabelText('Country or region'), { target: { value: 'GB' } })
    fireEvent.click(screen.getByRole('checkbox'))
    responder = () => ({ _status: 400, error: 'name_reserved' })
    fireEvent.click(screen.getByRole('button', { name: 'Join room' }))

    const err = await screen.findByRole('alert')
    expect(err.textContent).toMatch(/reserved/i)
    expect(screen.getByLabelText('Display name').getAttribute('aria-invalid')).toBe('true')
  })

  it('offers a usable alternative when the name is taken', async () => {
    render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')
    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'Ali Hassan' } })
    fireEvent.change(screen.getByLabelText('Date of birth'), { target: { value: '1990-05-04' } })
    fireEvent.change(screen.getByLabelText('Country or region'), { target: { value: 'GB' } })
    fireEvent.click(screen.getByRole('checkbox'))
    responder = () => ({ _status: 400, error: 'name_taken', suggestion: 'Ali Hassan 2' })
    fireEvent.click(screen.getByRole('button', { name: 'Join room' }))

    const suggest = await screen.findByRole('button', { name: 'Ali Hassan 2' })
    fireEvent.click(suggest)
    expect((screen.getByLabelText('Display name') as HTMLInputElement).value).toBe('Ali Hassan 2')
  })

  // ── Ejection hands over the appeal ticket ──

  it('shows the appeal details once, and does not hide them behind prose', async () => {
    await enterRoom()
    responder = url => {
      if (url.includes('afterSequence')) return { messages: [] }
      if (url.endsWith('/messages')) {
        return {
          published: false, sequence: null, status: 'blocked', reason: 'abuse_severe', ejected: true,
          appeal: { reference: 'PCIW-A1B2', credential: 'cred-abc123', restricted_until: '2026-07-28 10:00:00', how: '/world/appeals' },
        }
      }
      return ROOM
    }
    fireEvent.change(screen.getByLabelText('Your message'), { target: { value: 'abuse' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send' }))

    await screen.findByText('Your session has ended')
    expect(screen.getByText('PCIW-A1B2')).toBeTruthy()
    expect(screen.getByText('cred-abc123')).toBeTruthy()
    // The room token must be gone — an ejected session must not linger in storage.
    expect(sessionStorage.getItem('pciworld_room_session')).toBeNull()
  })

  // ── The room list ──

  it('states room status in words, not by colour', async () => {
    render(<CommunityApp />)
    await screen.findByText('Lobby')
    // Scoped to the card's meta line: /Open/ alone also matches the rules text elsewhere on the
    // page, and a query that matches several nodes proves nothing about what a participant reads.
    const meta = screen.getByText(/3 of 50 here/).closest('p')
    expect(meta?.textContent).toContain('Open')
    expect(meta?.textContent).toContain('3 of 50 here')
  })

  it('explains an empty catalogue rather than showing a blank page', async () => {
    responder = () => ({ rooms: [] })
    render(<CommunityApp />)
    await screen.findByText('No rooms are open right now')
  })

  it('offers a country list when the catalogue names the served jurisdictions', async () => {
    responder = url => {
      if (url.includes('/community/rooms/lobby')) {
        return { ...ROOM, served_jurisdictions: ['GB', 'AE'] }
      }
      if (url.includes('/community/rooms')) {
        return { rooms: [ROOM], served_jurisdictions: ['GB', 'AE'], minimum_age: 18, publishes_messages: true }
      }
      return {}
    }
    render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    const country = await screen.findByLabelText('Country or region')
    expect(country.tagName).toBe('SELECT')
    expect(screen.getByRole('option', { name: 'GB' })).toBeTruthy()
    expect(screen.getByRole('option', { name: 'AE' })).toBeTruthy()
  })

  it('does not offer image sharing, which is not built', async () => {
    await enterRoom()
    expect(screen.queryByLabelText(/image/i)).toBeNull()
    expect(screen.queryByRole('button', { name: /attach/i })).toBeNull()
  })
})
