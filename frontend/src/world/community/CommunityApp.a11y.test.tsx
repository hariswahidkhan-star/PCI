import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest'
import { fireEvent, render, screen } from '@testing-library/react'
import axe from 'axe-core'
import CommunityApp from './CommunityApp'

// PCI World community rooms — automated accessibility checks (§18.1, WCAG 2.2 AA).
//
// WHAT THIS COVERS AND WHAT IT DOES NOT — stated up front, because an a11y suite that implies more
// than it checks is worse than none.
//
// COVERED: the structural rules, which are the ones this interface most plausibly gets wrong —
// every control has an accessible name, form fields are labelled and their errors associated, ARIA
// attributes are valid for their roles, headings and landmarks are ordered, and list semantics are
// well formed.
//
// NOT COVERED: colour contrast, focus-visible appearance, reflow at 400% zoom, and anything else
// needing real layout — jsdom has no rendering engine, so axe cannot evaluate them and silently
// skips them. Those need a browser pass over the built bundle, which is recorded as a gap in
// CCP-P2-011 rather than implied by this file passing.
//
// The run fails on `critical` and `serious`. The repo's Playwright house rule logs serious and
// fails only on critical; this is stricter because the structural rules axe CAN evaluate here are
// exactly the ones where "serious" means a screen-reader user is genuinely blocked.

const ROOM = {
  slug: 'lobby', title: 'Lobby', description: 'General discussion', topic: null,
  category: null, locale: 'en', room_type: 'community', state: 'open',
  capacity: 50, guests_welcome: true, slow_mode_seconds: 0, participants: 3,
  accepting: true, images_allowed: false, rules_version: 'v1',
  pinned_welcome: 'Welcome — please read the room rules.', retention_class: 'standard',
}

async function scan(container: HTMLElement) {
  const results = await axe.run(container, {
    // Only the rules jsdom can actually evaluate. Asking for the rest would produce "incomplete"
    // results that are easy to mistake for passes.
    runOnly: { type: 'tag', values: ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'] },
    rules: {
      'color-contrast': { enabled: false },   // needs layout; browser-only
      'target-size': { enabled: false },      // needs layout; browser-only
    },
  })
  const blocking = results.violations.filter(v => v.impact === 'critical' || v.impact === 'serious')
  return blocking.map(v => `${v.id} (${v.impact}) ×${v.nodes.length}: ${v.help}`)
}

describe('World community rooms — accessibility', () => {
  let responder: (url: string) => unknown

  beforeEach(() => {
    sessionStorage.clear()
    responder = url => {
      if (url.includes('/community/rooms/lobby/messages')) return { messages: [] }
      if (url.includes('/community/rooms/lobby')) return ROOM
      if (url.includes('/community/rooms')) return { rooms: [ROOM] }
      return {}
    }
    vi.stubGlobal('fetch', vi.fn(async (url: string) => {
      const payload = responder(url)
      const status = (payload as { _status?: number })?._status ?? 200
      return { ok: status < 400, status, text: async () => JSON.stringify(payload) } as unknown as Response
    }))
  })
  afterEach(() => vi.unstubAllGlobals())

  it('the room catalogue has no blocking violations', async () => {
    const { container } = render(<CommunityApp />)
    await screen.findByText('Lobby')
    expect(await scan(container)).toEqual([])
  })

  it('the empty catalogue has no blocking violations', async () => {
    responder = () => ({ rooms: [] })
    const { container } = render(<CommunityApp />)
    await screen.findByText('No rooms are open right now')
    expect(await scan(container)).toEqual([])
  })

  it('the guest entry form has no blocking violations', async () => {
    const { container } = render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')
    expect(await scan(container)).toEqual([])
  })

  it('the entry form stays accessible while showing an error', async () => {
    // The state a person is actually in when they need the semantics most.
    const { container } = render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')
    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'admin' } })
    fireEvent.change(screen.getByLabelText('Date of birth'), { target: { value: '1990-05-04' } })
    fireEvent.change(screen.getByLabelText('Country or region'), { target: { value: 'GB' } })
    fireEvent.click(screen.getByRole('checkbox'))
    responder = () => ({ _status: 400, error: 'name_reserved', suggestion: 'admin 2' })
    fireEvent.click(screen.getByRole('button', { name: 'Join room' }))
    await screen.findByRole('alert')
    expect(await scan(container)).toEqual([])
  })

  it('the room transcript and composer have no blocking violations', async () => {
    const { container } = render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')
    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'Ali Hassan' } })
    fireEvent.change(screen.getByLabelText('Date of birth'), { target: { value: '1990-05-04' } })
    fireEvent.change(screen.getByLabelText('Country or region'), { target: { value: 'GB' } })
    fireEvent.click(screen.getByRole('checkbox'))
    responder = url => {
      if (url.includes('guest-sessions')) return { ok: true, token: 't', display_name: 'Ali Hassan' }
      if (url.includes('afterSequence')) {
        return { messages: [
          { sequence: 1, body: 'How are you forecasting escalation?', author: 'Sara Khan', reply_to: null, at: '2026-07-27 10:00:00' },
          { sequence: 2, body: 'We use a blended rate.', author: 'Omar Farouk', reply_to: null, at: '2026-07-27 10:01:00' },
        ] }
      }
      return ROOM
    }
    fireEvent.click(screen.getByRole('button', { name: 'Join room' }))
    await screen.findByLabelText('Your message')
    await screen.findByText('How are you forecasting escalation?')
    expect(await scan(container)).toEqual([])
  })

  it('the ejection screen has no blocking violations', async () => {
    const { container } = render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')
    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'Ali Hassan' } })
    fireEvent.change(screen.getByLabelText('Date of birth'), { target: { value: '1990-05-04' } })
    fireEvent.change(screen.getByLabelText('Country or region'), { target: { value: 'GB' } })
    fireEvent.click(screen.getByRole('checkbox'))
    responder = url => {
      if (url.includes('guest-sessions')) return { ok: true, token: 't', display_name: 'Ali Hassan' }
      if (url.includes('afterSequence')) return { messages: [] }
      return ROOM
    }
    fireEvent.click(screen.getByRole('button', { name: 'Join room' }))
    await screen.findByLabelText('Your message')

    responder = url => {
      if (url.includes('afterSequence')) return { messages: [] }
      if (url.endsWith('/messages')) {
        return {
          published: false, sequence: null, status: 'blocked', reason: 'abuse_severe', ejected: true,
          appeal: { reference: 'PCIW-A1B2', credential: 'cred-abc', restricted_until: '2026-07-28 10:00:00', how: '/world/appeals' },
        }
      }
      return ROOM
    }
    fireEvent.change(screen.getByLabelText('Your message'), { target: { value: 'x' } })
    fireEvent.click(screen.getByRole('button', { name: 'Send' }))
    await screen.findByText('Your session has ended')
    // The screen carrying the only copy of someone's appeal code must be readable by everyone.
    expect(await scan(container)).toEqual([])
  })

  it('the image composer is accessible', async () => {
    // Scanned separately because ROOM above has images_allowed: false — the composer only renders
    // where the room actually permits images, so a scan of the ordinary room would silently cover
    // nothing at all and report a pass.
    const IMAGE_ROOM = { ...ROOM, slug: 'lobby', images_allowed: true }
    responder = url => {
      if (url.includes('/community/rooms/lobby/messages')) return { messages: [] }
      if (url.includes('/community/rooms/lobby')) return IMAGE_ROOM
      return { rooms: [IMAGE_ROOM] }
    }
    const { container } = render(<CommunityApp />)
    await screen.findByText('Lobby')
    fireEvent.click(screen.getByRole('button', { name: /Enter Lobby/ }))
    await screen.findByLabelText('Display name')
    fireEvent.change(screen.getByLabelText('Display name'), { target: { value: 'Ali Hassan' } })
    fireEvent.change(screen.getByLabelText('Date of birth'), { target: { value: '1990-05-04' } })
    fireEvent.change(screen.getByLabelText('Country or region'), { target: { value: 'GB' } })
    fireEvent.click(screen.getByRole('checkbox'))
    responder = url => {
      if (url.includes('guest-sessions')) return { ok: true, token: 't', display_name: 'Ali Hassan' }
      if (url.includes('afterSequence')) return { messages: [] }
      return IMAGE_ROOM
    }
    fireEvent.click(screen.getByRole('button', { name: 'Join room' }))

    // Guard the fixture: if the composer did not render, the scan below proves nothing.
    expect(await screen.findByLabelText('Image file')).toBeTruthy()
    expect(screen.getByLabelText('Describe the image')).toBeTruthy()
    expect(await scan(container)).toEqual([])
  })
})
